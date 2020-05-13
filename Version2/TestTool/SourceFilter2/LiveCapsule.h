///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#ifndef LIVE_CAPSULE_H
#define LIVE_CAPSULE_H

#include "Formats.h"
#include <deque>
#include <string>
#include "rtspwrapper.h"

enum EStepDataType {
  ESDT_Begin,
  ESDT_Send,
  ESDT_Receive,
  ESDT_Log,
  ESDT_ResultOK,
  ESDT_ResultError
};

typedef void (__stdcall * TSharpStepCallback)(int ObjectId, int StepNumber, int Type, const char* Text);

struct SControlConnection {
  std::string Uri;
  std::string User;
  std::string Password;
  ELiveTransport Transport;
  int Port;
  int NICIndex;
  int Timeout;
};

struct SChannelSettings {
  std::string MulticastAddress;
  int MulticastRtpPort;
  int MulticastTTL;
  std::string Codec;
  int Width;
  int Height;
  int FPS;
  int Channels;
  int Frequency;
};

struct STestSequence {
  int ObjectId;
  int SequenceNumber;
  int Timeout;
  std::string CustomSetupFields;
  std::string CustomPlayFields;
  std::string CustomPauseFields;
  bool UseVideo;
  bool UseAudio;
  bool UseMetadata;
  bool UseBackchannel;
  bool CheckOptions;
  bool CheckActualResolution;
  bool CheckJPEGExtension;
};

struct SFullConfig {
  SControlConnection ControlConnection;
  STestSequence TestSequence;
  SChannelSettings Video;
  SChannelSettings Audio;
  SChannelSettings Metadata;
  SChannelSettings Backchannel;
  void Load(const char* FileName);
};

class CFrameQueue {
public:
  CFrameQueue(int MaxSize = 20) : MaxSize(MaxSize), PassedCount(0), PlayedCount(0) {};
  ~CFrameQueue();
  void Push(FrameInfo*);
  FrameInfo* Peek();
  FrameInfo* Pop();
  int GetCount();
  int GetPassed() const { return PassedCount; };
  int GetPlayed() const { return PlayedCount; };
private:
  CCriticalSection Door;
  std::deque<FrameInfo*> Frames;
  int MaxSize;
  int PassedCount;
  int PlayedCount;
};

class CFramePipe : public CFrameQueue {
public:
  CFramePipe() : Subsession(NULL), Format(NULL), Prefix(NULL) {};
  ~CFramePipe() { delete Subsession; delete Prefix; };
  IMediaSubsession *Subsession;
  GeneralFormatSupporter* Format;
  FrameInfo *Prefix;
};

struct SStepLogRecord {
  std::string Name;
  std::string Request;
  std::string Response;
  std::string Log;
  int Result;
};

class CStepLog {
public:
  CStepLog() : StepCallback(NULL), LastStep(-1), ObjectId(0) {};
  void SetNotificationCallback(TSharpStepCallback StepCallback) { this->StepCallback = StepCallback; };
  void SetObjectId(int ObjectId) { this->ObjectId = ObjectId; };

  int CreateNewStep(const char* Name);
  void SetRequest(const char* Text, int Step = -1);
  void SetResponse(const char* Text, int Step = -1);
  void AddLogText(const char* Text, int Step = -1);
  void SetStepResult(int Result = 0, const char* Text = "", int Step = -1);
  int GetCurrentStep();
private:
  CCriticalSection Door;
  std::vector<SStepLogRecord> Log;
  int VerifyStep(int Step);
  TSharpStepCallback StepCallback;
  long LastStep;
  int ObjectId;
};

class CLiveCapsule : CThread {
public:
  CLiveCapsule () : PlayType(0), Terminating(false), Halt(false), InSequence(false), InSleep(false), Sequence(0) { RunThread(); };
  ~CLiveCapsule() { SetTerminating(false); StopThread(); };
  void SetTerminating(bool Halt) { Terminating = true; this->Halt = Halt; Live555Wrapper.Terminate(); };
  void ClearMutators();
  void SetNotificationCallback(TSharpStepCallback StepCallback) { StepLog.SetNotificationCallback(StepCallback); };
  bool Play();
  void Pause();
  void Stop();
  bool RunSequence(int Sequence);
  bool RunStep(int Step);
  bool RunStepSync(int Step);
  ELiveStep GetCurrentStep() const { return Live555Wrapper.GetCurrentStep(); };
  bool IsNeedTeardown() const { return Live555Wrapper.IsNeedTeardown(); };
  bool IsInSequence();

  // for OTT usage
  bool Configure(const WCHAR* FileName);
  bool Configure(const char* FileName);
  bool HasAudio() const;
  bool HasVideo() const;
  bool HasMeta() const;
  bool IsFakeVideo() const;
  RTSPClientState GetState() const;
  bool GetVideoFormat(CMediaType *pmt);
  bool CheckVideoFormat(const CMediaType *pmt) const;
  bool GetAudioFormat(CMediaType *pmt);
  bool CheckAudioFormat(const CMediaType *pmt) const;

  FrameInfo* GetTop(bool Audio);
  int GetCount(bool Audio);
  FrameInfo* GetMeta();
private:
  CFramePipe FramesVideo;
  CFramePipe FramesAudio;
  CFrameQueue FramesMeta;
  CLive555Wrapper Live555Wrapper;
  SFullConfig FullConfig;
  CStepLog StepLog;
  int PlayType;
  bool Terminating;
  bool Halt;
  bool InSequence;
  bool InSleep;
  int Sequence;
  void RunSequenceInternal(int Sequence);
  void Thread();
  void InterruptableSleep(int Timeout);

  void ConfigureWrapper();
  bool GetSpecificErrorDescription(char *Buffer, ELiveStep Step, int &Result);
static void NotifyFunctionSend(const void* UserData, ELiveStep Step, const char* MessageData);
static void NotifyFunctionReceive(const void* UserData, ELiveStep Step, const char* MessageData);
static void NotifyFunctionLog(const void* UserData, ELiveStep Step, const char* MessageData);
static void VideoDataFunction(
  const void* UserData, 
  unsigned char* ReceiveBuffer, 
  unsigned FrameSize, 
  unsigned NumTruncatedBytes,
  struct timeval PresentationTime, 
  unsigned DurationInMicroseconds
  );
static void AudioDataFunction(
  const void* UserData, 
  unsigned char* ReceiveBuffer, 
  unsigned FrameSize, 
  unsigned NumTruncatedBytes,
  struct timeval PresentationTime, 
  unsigned DurationInMicroseconds
  );
static void MetadataDataFunction(
  const void* UserData, 
  unsigned char* ReceiveBuffer, 
  unsigned FrameSize, 
  unsigned NumTruncatedBytes,
  struct timeval PresentationTime, 
  unsigned DurationInMicroseconds
  );

static void CompleteFunction(const void* UserData, ELiveStep Step, int Result, const char* Body);

  // for general usage
  bool Prepare(unsigned int Timeout = (unsigned)-1);
  bool PrepareRtp();

  bool PlayAsFilter();

  void FullStop();
  bool PlayAsTestBase();
  bool PlayAsTest1TimeWait();
  bool PlayAsTest2FramesWait();
  bool PlayAsTest3TimeWaitRtp();
  bool PlayAsTest4Backchannel();
  bool PlayAsTest5Playback();
  static const int SequenceCount = 5;

  void ExtractResolutionFromSdp();
  bool CheckResolution();
  bool CheckFrames(bool MakeStep, int Time);
  bool CheckTermination();
  bool CheckOptions(unsigned int Timeout);
};

#endif // LIVE_CAPSULE_H

