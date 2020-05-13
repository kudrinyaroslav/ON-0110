

#ifndef RTSP_WRAPPER
#define RTSP_WRAPPER

#include <string>

class TaskScheduler;
class UsageEnvironment;
class MediaSession;
class MediaSubsession;
class MediaSink;


enum ELiveChannels {
  ELC_Error,
  ELC_Video,
  ELC_Audio,
  ELC_Metadata,
  ELC_AudioBack
};

enum ELiveTransport {
  ELT_UDP,
  ELT_TCP,
  ELT_HTTP,
  ELT_MULTICAST
};

enum ELiveStep {
  ELS_Begin,
  ELS_Init,
  ELS_Options,
  ELS_Announce,
  ELS_Describe,
  ELS_Setup,
  ELS_SetupSub,
  ELS_Play,
  ELS_Pause,
  ELS_Record,
  ELS_Teardown,
  ELS_SetParameter,
  ELS_GetParameter
};

const char* GetLiveStepName(ELiveStep Step);

typedef void (*DataFunction)(
  const void* UserData, 
  unsigned char* fReceiveBuffer, 
  unsigned frameSize, 
  unsigned numTruncatedBytes,
  struct timeval presentationTime, 
  unsigned durationInMicroseconds
  );
typedef void (*NotifyFunction)(const void* UserData, ELiveStep Step, const char* MessageData);
typedef void (*CompleteFunction)(const void* UserData, ELiveStep Step, int Result, const char* Body);

struct SMediaProperties {
  std::string CodecName;
  int Payload;
  MediaSubsession* Subsession;
  RTPSource* RtpSource;
};

struct SVideoProperties : public SMediaProperties {
  int Width;
  int Height;
  int FPS;
};

struct SAudioProperties : public SMediaProperties {
  int Channels;
  int Frequency;
};

class CCriticalSection {
public:
  CCriticalSection()
  {
    InitializeCriticalSection(&Door);
  };
  ~CCriticalSection()
  {
    DeleteCriticalSection(&Door);
  };
  void Enter()
  {
    EnterCriticalSection(&Door);
  };
  void Leave()
  {
    LeaveCriticalSection(&Door);
  };
private:
  CRITICAL_SECTION Door;
};
class CLocker {
public:
  CLocker(CCriticalSection &Door) : Door(Door)
  {
    Door.Enter();
  };
  ~CLocker()
  {
    Door.Leave();
  };
private:
  CCriticalSection &Door;
};

class CThread {
public:
  CThread() : ThreadStarted(false), ThreadStopped(false), hThread(NULL) {};
  void RunThread();
  void StopThread();
  virtual void Thread() {};
protected:
  static unsigned __stdcall ThreadProc(void* Ptr);
  bool ThreadStarted;
  bool ThreadStopped;
  HANDLE hThread;
};

class CWrapRTSPClient;
//class CWrapSink;
class CLive555Wrapper : CThread {
public:
  CLive555Wrapper();
  ~CLive555Wrapper();
  void AttachCredentials(const char* User, const char* Password);
  bool Open(const char* Rtsp, ELiveTransport Transport, int TunnelPort = 80);
  void AttachNotify(NotifyFunction Send, NotifyFunction Receive, NotifyFunction Log, void* NotifyData = NULL);
  void AttachCompletion(CompleteFunction Complete, void* CompleteData = NULL);
  void AttachChannel(ELiveChannels Channel, const char* Codec, DataFunction Func, void *UserData, const char *Address = NULL, int Port = 0, int TTL = 0);
  bool AttachRtpCallback(ELiveChannels Channel, void (*RtpExtHdrCallback)(u_int16_t profile, u_int16_t seq, u_int16_t len, u_int8_t* pHdrData, void* pPriv), void *UserData);
  bool GetVideoProperties(SVideoProperties &Properties);
  bool GetAudioProperties(SAudioProperties &Properties);
  MediaSubsession* GetChannelSession(ELiveChannels Channel);
  RTPSource* GetChannelSource(ELiveChannels Channel);

  ELiveStep GetCurrentStep() const { return CurrentStep; };
  bool IsNeedTeardown() const { return NeedTeardown; };
  bool IsActive() const { return InProgress; };
  int GetResult() const { return Result; };
  int WaitResult(unsigned int Timeout = (unsigned)-1);
  int WaitResult(const char* &Body, unsigned int Timeout = (unsigned)-1);
  void Terminate() { EventLoopWatchVariable = 1; };

  bool CreateMediaSessionBySdp(const char* sdp);
  bool CreateRtpSubsessionsByChannels();
  bool CreateSinks();
  
  bool SetAdditionalFields(const char* Text);
  bool RunCommand(ELiveStep Step);
  bool Options();
  bool Describe();
  bool Setup();
  bool SetupSub(int Channel);
  bool Play(double start = 0.0);
  bool Playback(const char* From, const char* To = NULL);
  bool Pause();
  bool Teardown();
private:
  friend class CWrapRTSPClient;
  friend class CWrapSink;

  static void RtspHandler(RTSPClient* rtspClient, int resultCode, char* resultString);
  ELiveStep CurrentStep;
  int CurrentSubStep;
  bool NeedTeardown;
  volatile bool InProgress;
  volatile int Result;
  const char* ResultString;
  bool Prologue(ELiveStep Step);
  void SignalComplete(int Result, const char* Body);
  NotifyFunction SendNotify;
  NotifyFunction ReceiveNotify;
  NotifyFunction LogNotify;
  void* NotifyData;
  CompleteFunction Complete;
  void* CompleteData;

  void RunThread();
  void StopThread();
  void Thread();
  volatile char EventLoopWatchVariable;

  ELiveTransport Transport;
  CWrapRTSPClient* RtspClient;
  TaskScheduler* Scheduler;
  UsageEnvironment* Environment;
  MediaSession* Session;
  Authenticator* Credentials;
  int VerbosityLevel;
  std::string RtspAddress;
  int TunnelPort;

  struct SDataChannel {
    ELiveChannels Channel;
    std::string Codec;
    DataFunction Func;
    void *UserData;
    std::string Address;
    int Port;
    int TTL;
    MediaSubsession* Subsession;
    MediaSink *Sink;
    RTPSource *RtpSource; 
    RTCPInstance *RtcpInstance;
  };
  typedef std::vector<SDataChannel> TChannels;
  TChannels Channels;
  void CleanupChannels();
  CCriticalSection ConfigurationDoor;

  void AfterGettingFrame(ELiveChannels Channel, unsigned char* fReceiveBuffer, unsigned frameSize, unsigned numTruncatedBytes,
				  struct timeval presentationTime, unsigned durationInMicroseconds);
  void WriteLog(const char* Text, ...);
};

#endif // RTSP_WRAPPER