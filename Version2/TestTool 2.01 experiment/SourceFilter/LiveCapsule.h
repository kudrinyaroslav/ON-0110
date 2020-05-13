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
  //int VideoWidth;
  //int VideoHeight;
  int TimeOut;
  bool UseMetadata;
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
    //VideoWidth = 0;
    //VideoHeight = 0;
    TimeOut = 10;
    UseMetadata = false;
  }
  void Load(const WCHAR* FileName);
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
  friend class LiveCapsule;
  class LiveCapsule *Parent;
  MediaSubsession   *sub;
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
    MediaSubsession   *s,
    GeneralFormatSupporter *f) :
      Frames(FRAMES_POOL)
  {
    Parent = P;
    sub = s;
    Supporter = f;
    Buffer.SetSize((Supporter->AsType() == CODEC_TYPE_VIDEO) ? 65536*8 : 4096*4);
    Start = 0;
    Prefix = NULL;
    FramesCount = 0;
    KeyInterval = 0;
    KeyIntervalBack = 0;
    KeyAverage = 0;
    KeyAverageDistortion = 0;
    KeyLastDistortion = 0;
    Keys[0] = Keys[1] = Keys[2] = Keys[3] = 0;
    KeyIndex = 0;
  };
  ~CapsuleTrack()
  {
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
  void UseSubSource(MediaSubsession *sub, GeneralFormatSupporter *supporter);
  virtual DWORD ThreadProc();
  bool RunThread();
  void StopThread();
  bool CheckOptions(const char *Options);
  void CheckConnectionState();

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
  bool PlayStream();
  bool PauseStream();
  bool StopStream();
  bool CloseStream();

  bool NeedRePlay();
  void BalanceLoad();
  bool IsAlive() const { return !StopFlag; };

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
    if (!Video->sub) return false;
    if (!Video->Supporter) return false;
    return Video->Supporter->GetFormat(pmt, Video->sub, &Video->Prefix);
  }
  bool CheckVideoFormat(const CMediaType *pmt) const
  {
    if (!Video) return false;
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

  char              LastEvent[2048]; // hack
};
