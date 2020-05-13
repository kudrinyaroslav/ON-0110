///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include "Formats.h"

enum ProcessingStep
{
  step_Constructed,
  step_InitEnvironment,
  step_OPTIONS,
  step_CheckOptions,
  step_DESCRIBE,
  step_OpenStream,
  step_SETUP,
  step_PLAY,
  step_WaitStream,
  step_PAUSE,
  step_StopThread,
  step_TEARDOWN,
  step_HaltEnvironment
};

class MessageLog
{
  char  IniName[260];
  int ErrorReport;
  CQueue<LPOLESTR> Lines;
  long Count;
  void DropIni(const char* Message);
  int Step;
public:
  MessageLog() : Lines(100) { Count = 0; ErrorReport = 0; IniName[0] = 0; Step = 0;};
  void AttachIni(const char* Name);
  bool CanGet() const { return Count > 0; };
  LPOLESTR GetTop();
  void Drop(const char* Format, ...);
  //void Drop(const WCHAR* Format, ...);
  void SetStep(int Step);
  void SetStepEnd(int PassBits, int FailBits);
};

//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow
//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow whitelist
//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow_audio
//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow_enc

struct CapsuleSettings
{
  char Address[260];
  int  TunnelPortNumber;
  bool UseTCPTunnel;
  bool UseOptions;
  char User[64];
  char Password[64];
  bool UseAudio;
  bool UseKeepAlive;
  bool UseKeepAliveOptions;
  //int VideoWidth;
  //int VideoHeight;
  int TimeOut;
  bool UseMetadata;
  bool Multicast;
  bool UseVideo;

  int  VideoWidth;
  int  VideoHeight;
  bool RTSP;
  int  VideoFPS;
  char VideoCodecName[64];
  char AudioCodecName[64];
  char MulticastAddress[64];
  int  MulticastRtpPortVideo;
  int  MulticastRtpPortAudio;
  int  MulticastTTL;

  char CustomSetupFields[1024];
  char CustomPlayFields[1024];
  char CustomPauseFields[1024];
  bool DoSetupOnReplay;
  bool ReplayMode;
  int  ReplayMaxDuration;
  int  ReplayPauseWait;
  bool ReplayReverse;

  CapsuleSettings()
  {
    Address[0] = 0;
    TunnelPortNumber = 0;
    UseTCPTunnel = false;
    UseOptions = false;
    User[0] = 0;
    Password[0] = 0;
    UseAudio = false;
    UseKeepAlive = false;
    UseKeepAliveOptions = false;
    //VideoWidth = 0;
    //VideoHeight = 0;
    TimeOut = 10;
    UseMetadata = false;
    Multicast = false;
    UseVideo = true;

    VideoWidth = 0;
    VideoHeight = 0;
    RTSP = true;
    VideoFPS = 0;
    VideoCodecName[0] = 0;
    AudioCodecName[0] = 0;
    MulticastAddress[0] = 0;
    MulticastRtpPortVideo = 1234;
    MulticastRtpPortAudio = 1236;
    MulticastTTL = 1;

    CustomSetupFields[0] = '\0';
    CustomPlayFields[0] = '\0';
    DoSetupOnReplay = false;
    ReplayMode = false;
    ReplayMaxDuration = 0;
    ReplayPauseWait = 0;
    ReplayReverse = false;
  }
  void Load(const WCHAR* FileName);
};

struct RtpReplayExtension
{
  u_int32_t dateTime;
  u_int32_t secFrac;
  u_int8_t bitC;
  u_int8_t bitE;
  u_int8_t bitD;
  u_int8_t CSeq;
  u_short seq;
  u_int8_t startsFrame;
  u_int8_t endsFrame;
  u_int8_t validLength;
  u_int32_t timestamp;
  bool sent;
  RtpReplayExtension() : sent(false) {}
  void ToString(MessageLog* log, bool canSkip = true)
  {
    // RTP=DateTimeUnix.SecondsFraction.C.E.D.CSeq.SequenceNumber.CanSkip.StartsFrame.EndsFrame.ValidLength
    log->Drop("RTP=%u.%u.%u.%u.%u.%u.%u.%u.%u.%u.%u.%u", 
              dateTime, secFrac, bitC, bitE, bitD, CSeq, seq, canSkip ? 1 : 0, 
              startsFrame, endsFrame, validLength, timestamp);
    sent = true;
  }
};

class CapsuleMediaSubsession : public IMediaSubsession
{
  MediaSubsession *mSub;
  MessageLog *mLog;
public:
  CapsuleMediaSubsession(MediaSubsession *sub, MessageLog *log) : mSub(sub), mLog(log), mCheckingPackets(0), mFirstPacket(NULL) {}
public: // IMediaSubsession
  virtual unsigned short   videoWidth()  const { return mSub->videoWidth(); }
  virtual unsigned short   videoHeight() const { return mSub->videoHeight(); }
  virtual unsigned         videoFPS()    const { return mSub->videoFPS(); }
  virtual unsigned         numChannels() const { return mSub->numChannels(); }
  virtual char const*      mediumName()  const { return mSub->mediumName(); }
  virtual char const*      codecName()   const { return mSub->codecName(); }
  virtual char const*      fmtp_config() const { return mSub->fmtp_config(); }
  virtual char const*      fmtp_spropparametersets() const { return mSub->fmtp_spropparametersets(); }
  virtual unsigned         rtpTimestampFrequency()   const { return mSub->rtpTimestampFrequency(); }
  virtual RTPSource*       rtpSource()                     { return mSub->rtpSource(); }
  virtual Boolean          initiate(int useSpecialRTPoffset = -1);
  virtual MediaSubsession* mediaSub() { return mSub; }

public:
  MessageLog* Log() { return mLog; }
public:
  RtpReplayExtension packets[2];
  RtpReplayExtension *mFirstPacket;
  u_int8_t mCheckingPackets;
};

class CapsuleRtpSubsession : public IMediaSubsession
{
  UsageEnvironment *mEnv;
  CapsuleSettings *mSettings;
  Groupsock *mRtpGroupsock;
  Groupsock *mRtcpGroupsock;
  RTPSource *mRtpSource; 
  RTCPInstance *mRtcpInstance;
  char *mMediumName;
  char *mCodecName;
  int mRtpPort;
  unsigned mRtpTimestampFrequency;
public:
  CapsuleRtpSubsession(UsageEnvironment *env, CapsuleSettings *settings, bool video);
  ~CapsuleRtpSubsession();
public: // IMediaSubsession
  virtual unsigned short   videoWidth()  const { return mSettings->VideoWidth; }
  virtual unsigned short   videoHeight() const { return mSettings->VideoHeight; }
  virtual unsigned         videoFPS()    const { return mSettings->VideoFPS; }
  virtual char const*      mediumName()  const { return mMediumName; }
  virtual char const*      codecName()   const { return mCodecName; }
  virtual RTPSource*       rtpSource()         { return mRtpSource; }
  virtual Boolean          initiate(int useSpecialRTPoffset = -1);
  virtual unsigned         rtpTimestampFrequency()   const { return mRtpTimestampFrequency; }
  // unsupported
  virtual unsigned         numChannels()             const { return 2; }
  virtual char const*      fmtp_config()             const { return ""; }
  virtual char const*      fmtp_spropparametersets() const { return ""; }
  virtual MediaSubsession* mediaSub()                      { return NULL; }
};

class TrackBuffer
{
  typedef std::vector<unsigned char> ContainerType;
  ContainerType Container;
public:
  TrackBuffer() {};
  TrackBuffer(int size)
  {
    SetSize(size);
  }
  void SetSize(int size) { Container.resize(size); };
  int GetSize() { return Container.size(); };
  unsigned char* GetBuffer() { return const_cast<unsigned char*>(&(Container.front())); };
};

#define FRAMES_POOL 150
#define MAX_FRAMES  100
#define MIN_FRAMES  10

class CapsuleTrack
{
public:
  typedef void (afterGettingFunc)(class LiveCapsule *pCapsule, struct timeval presentationTime);
  afterGettingFunc *afterGetting;
private:
  friend class LiveCapsule;
  class LiveCapsule *Parent;
  IMediaSubsession  *sub;
  GeneralFormatSupporter *Supporter;
  TrackBuffer       Buffer;
  CQueue<FrameInfo*> Frames;
  long FramesCount;
  REFERENCE_TIME Start;
  FrameInfo *Prefix;
  long KeyInterval;
  long KeyIntervalBack;
  long Keys[4];
  int KeyIndex;
  double KeyAverage;
  double KeyAverageDistortion;
  double KeyLastDistortion;
  
  static void AfterGetting(
    void* clientData, 
    unsigned frameSize,
    unsigned numTruncatedBytes,
    struct timeval presentationTime,
    unsigned durationInMicroseconds);
  static void OnClose(void* clientData);

  void PutBottom(FrameInfo* Info);
  void CalcKeyTime();

public:
  CapsuleTrack(LiveCapsule *P, 
    IMediaSubsession   *s,
    GeneralFormatSupporter *f) :
      Frames(FRAMES_POOL)
  {
    Parent = P;
    sub = s;
    Supporter = f;
    Buffer.SetSize((Supporter->AsType() == CODEC_TYPE_VIDEO) ? 65536*8 : 4096*4);
    Prefix = NULL;
    Clear();
    afterGetting = NULL;
  };
  void Clear()
  {
    FramesCount = 0;
    Start = 0;
    KeyInterval = 0;
    KeyIntervalBack = 0;
    KeyAverage = 0;
    KeyAverageDistortion = 0;
    KeyLastDistortion = 0;
    Keys[0] = Keys[1] = Keys[2] = Keys[3] = 0;
    KeyIndex = 0;
  }
  ~CapsuleTrack()
  {
    if (sub) delete sub; sub = NULL;
    FrameInfo::Release(Prefix);
    Prefix = NULL;
  }
  void ReadSource();
  FrameInfo* GetTop();
  long GetCount() const { return FramesCount; };
  long GetKeyLen() const { return KeyInterval; };
  long GetKeyLenBack() const { return KeyIntervalBack; };
  double GetKeyLenAv() const { return KeyAverage; };
  bool StableKey() const;
  bool WasSyncPoint() const;
  IMediaSubsession* mediasub() { return sub; }
};

class LiveCapsule : protected CAMThread
{
  MediaSession     *ms;
  TaskScheduler    *scheduler;
  UsageEnvironment *env ;
  RTSPClient       *rtsp;	
//public:
  CapsuleTrack     *Audio;
  CapsuleTrack     *Video;
  CapsuleTrack     *Meta;
//private:
  RTSPClientState   State;
  char              StopFlag;
  char              StopFlagStop;
  bool InitEnvironment();
  void HaltEnvironment();
  char* DoOptions(const char* filename);
  char* DescribeStream(const char* filename);
  bool UseSubSource(IMediaSubsession *sub, GeneralFormatSupporter *supporter);
  virtual DWORD ThreadProc();
  bool RunThread();
  void StopThread();
  bool CheckOptions(const char *Options);
  void CheckConnectionState();
  static void PutFakeVideoFrame(LiveCapsule *pCapsule, struct timeval presentationTime);

  CapsuleSettings Settings;
  MessageLog* Log;

  bool StartingPlay;
  int LastParameterTick;
  int Step;
  int PassBits;
  int FailBits;
public:
  LiveCapsule(MessageLog* l) : Log(l)
  {
    ms = NULL;
    scheduler = NULL;
    env = NULL;
    rtsp = NULL;	
    Audio = NULL;
    Video = NULL;
    Meta = NULL;
    State = RTSP_STATE_IDLE;
    StopFlag = 0;
    StopFlagStop = 0;
    StartingPlay = false;
    LastParameterTick = 0;

    Step = 0;
    PassBits = 0;
    FailBits = 0;
    //SetStepOK(step_Constructed);
  }
  ~LiveCapsule()
  {
    CloseStream();
    //HaltEnvironment();
  }
  void RunStep(int Step);
  void SetStepOK(int Step);
  void SetStepFail(int Step);

  bool OpenSettings(const WCHAR* filename);
  bool OpenStream(const char* filename);
  bool OpenRtpStream();
  bool PlayStream();

  bool RtspSetup();

  bool PauseStream();
  bool StopStream();
  bool CloseStream();

  bool NeedRePlay();
  void BalanceLoad();
  bool IsAlive() const { return !StopFlag; };

  void CheckResponse(Boolean &Ret);

  bool TrySetParameter();
  void GetCurrentStep(int& Step, int& PassBits, int& FailBits)
  {
    Step = this->Step;
    PassBits = this->PassBits;
    FailBits = this->FailBits;
  }

  bool HasAudio() const { return Audio != NULL; };

  RTSPClientState GetState() const { return State; };

  bool GetVideoFormat(CMediaType *pmt) const
  {
    if (!Video) return false;
    if (Settings.UseVideo)
      if (!Video->sub) return false;
    if (!Video->Supporter) return false;
    return Video->Supporter->GetFormat(pmt, Video->sub, &Video->Prefix);
  }
  bool CheckVideoFormat(const CMediaType *pmt) const
  {
    if (!Video) return false;
    if (Settings.UseVideo)
      if (!Video->sub) return false;
    if (!Video->Supporter) return false;
    return Video->Supporter->CheckFormat(pmt, Video->sub);
  }
  bool GetAudioFormat(CMediaType *pmt) const
  {
    if (!Audio) return false;
    if (!Audio->sub) return false;
    if (!Audio->Supporter) return false;
    return Audio->Supporter->GetFormat(pmt, Audio->sub, &Audio->Prefix);
  }
  bool CheckAudioFormat(const CMediaType *pmt) const
  {
    if (!Audio) return false;
    if (!Audio->sub) return false;
    if (!Audio->Supporter) return false;
    return Audio->Supporter->CheckFormat(pmt, Audio->sub);
  }

  FrameInfo* GetTop(bool audio)
  {
    if (audio)
    {
      //if (Audio->GetCount() < 1) return NULL;
      return Audio->GetTop();
    }
    else
    {
      return Video->GetTop();
    }
  }
  int GetCount(bool audio)
  {
    if (audio)
    {
      return Audio->GetCount();
    }
    else
    {
      return Video->GetCount();
    }
  }

  const CapsuleTrack* GetVideo() const { return Video; };
  //void DropLogInfo(int level, const char* Format, ...);
  //void DropError(const char* Where);
  //void DropOK(const char* Where);
  void UnlockTracks();

  bool ReplayReverse() { return Settings.ReplayReverse; };

  char              LastEvent[2048]; // hack
};

class TaskScheduler2: public BasicTaskScheduler
{
public:
  static TaskScheduler2* createNew()
  {
    return new TaskScheduler2();
  }

  virtual ~TaskScheduler2() {}

protected:
  TaskScheduler2() {}

public:
  virtual void doEventLoop(char* watchVariable)
  {
    while (1) {
      if (watchVariable != NULL && *watchVariable != 0) break;
      SingleStep(2000000);
    }
  }
};

void DropDebug(const char* Format, ...);

class BasicUsageEnvironmentDbg: public BasicUsageEnvironment
{
public:
  BasicUsageEnvironmentDbg::BasicUsageEnvironmentDbg(TaskScheduler& taskScheduler)
    : BasicUsageEnvironment(taskScheduler) 
  {
  }

  virtual ~BasicUsageEnvironmentDbg() {}

  static BasicUsageEnvironmentDbg* createNew(TaskScheduler& taskScheduler)
  {
    return new BasicUsageEnvironmentDbg(taskScheduler);
  }

  void DropDebug(const char* Format, ...)
  {
    va_list ap;
    char Buffer[2048];
    int size = 0;
    va_start(ap, Format);
    size += _vsnprintf(Buffer + size, sizeof(Buffer), Format, ap);
    va_end(ap);
    OutputDebugStringA(Buffer);
  }

  virtual UsageEnvironment& operator<<(char const* str) 
  {
    DropDebug("ENV: %s", str);
    return *this;
  }
  virtual UsageEnvironment& operator<<(int i)
  {
    DropDebug("ENV: %d", i);
    return *this;
  }
  virtual UsageEnvironment& operator<<(unsigned u)
  {
    DropDebug("ENV: %u", u);
    return *this;
  }
  virtual UsageEnvironment& operator<<(double d)
  {
    DropDebug("ENV: %f", d);
    return *this;
  }
  virtual UsageEnvironment& operator<<(void* p)
  {
    DropDebug("ENV: %p", p);
    return *this;
  }

  virtual void appendToResultMsg(MsgString msg)
  {
    BasicUsageEnvironment::appendToResultMsg(msg);
    DropDebug("ENV: %s", getResultMsg());
  }

  virtual void setResultErrMsg(MsgString msg) 
  {
    DropDebug("ENV_ERR: %s %i", msg, getErrno());
  }
};
