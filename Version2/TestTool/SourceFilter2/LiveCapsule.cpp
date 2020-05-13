///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include "LiveCapsule.h"
#include "playbacksupport.h"
#include <time.h>
#include <map>

char* ToMulti(const WCHAR* Name)
{
  size_t size = wcstombs(0, Name, wcslen(Name)) + 1;
  if (!size) return NULL;

  char *char_str = new char[size];
  if (!char_str) return NULL;

  WideCharToMultiByte(CP_ACP, 0, Name, -1, char_str, (int)size, 0, 0);
  char_str[size-1] = 0; // make sure that string is properly ended

  return char_str;
}

void LoadIniString(std::string &str, const char* Default, const char* File, const char* Key, const char* Elem)
{
  char Buffer[2048]; Buffer[0];
  GetPrivateProfileStringA(Key,
    Elem,
    Default,
    Buffer,
    2047,
    File);
  str = Buffer;
}

#define LOAD_INI_STRING(Key, Elem, Default) \
  LoadIniString(Key.Elem, Default, FileName, #Key, #Elem)

#define LOAD_INI_INTEGER(Key, Elem, Default) \
  Key.Elem = GetPrivateProfileIntA(#Key, #Elem, Default, FileName)

#define LOAD_INI_BOOLEAN(Key, Elem, Default) \
  Key.Elem = !!GetPrivateProfileIntA(#Key, #Elem, Default, FileName)

#define LOAD_INI_ENUM(Key, Elem, Default, Type) \
  Key.Elem = (Type)GetPrivateProfileIntA(#Key, #Elem, Default, FileName)

#define RESTORE_CLRF(var)                \
  while (char *clrf = strstr((char*)TestSequence.var.c_str(), "$$")) \
  {                                      \
    *clrf++ = '\r';                      \
    *clrf = '\n';                        \
  }

void SFullConfig::Load(const char* FileName)
{
  LOAD_INI_STRING(ControlConnection, Uri, "rtsp://127.0.0.1");
  if (ControlConnection.Uri.substr(0, 4) == "http") {
    ControlConnection.Uri = "rtsp" + ControlConnection.Uri.substr(4);
  }
  LOAD_INI_STRING(ControlConnection, User, "");//
  LOAD_INI_STRING(ControlConnection, Password, "");//
  LOAD_INI_ENUM(ControlConnection, Transport, ELT_UDP, ELiveTransport);
  LOAD_INI_INTEGER(ControlConnection, Port, 0);
  LOAD_INI_INTEGER(ControlConnection, NICIndex, 0);
  LOAD_INI_INTEGER(ControlConnection, Timeout, 60000);

  LOAD_INI_INTEGER(TestSequence, ObjectId, 0);
  LOAD_INI_INTEGER(TestSequence, SequenceNumber, 0);
  LOAD_INI_INTEGER(TestSequence, Timeout, 60000);
  LOAD_INI_STRING(TestSequence, CustomSetupFields, "");
  LOAD_INI_STRING(TestSequence, CustomPlayFields, "");
  LOAD_INI_STRING(TestSequence, CustomPauseFields, "");
  RESTORE_CLRF(CustomSetupFields);
  RESTORE_CLRF(CustomPlayFields);
  RESTORE_CLRF(CustomPauseFields);
  LOAD_INI_BOOLEAN(TestSequence, UseVideo, true);
  LOAD_INI_BOOLEAN(TestSequence, UseAudio, false);
  LOAD_INI_BOOLEAN(TestSequence, UseMetadata, false);
  LOAD_INI_BOOLEAN(TestSequence, UseBackchannel, false);
  LOAD_INI_BOOLEAN(TestSequence, CheckOptions, false);
  LOAD_INI_BOOLEAN(TestSequence, CheckActualResolution, false);
  LOAD_INI_BOOLEAN(TestSequence, CheckJPEGExtension, false);

  LOAD_INI_STRING(Video, MulticastAddress, "");
  LOAD_INI_INTEGER(Video, MulticastRtpPort, 0);
  LOAD_INI_INTEGER(Video, MulticastTTL, 0);
  LOAD_INI_STRING(Video, Codec, "JPEG");
  LOAD_INI_INTEGER(Video, Width, 320);
  LOAD_INI_INTEGER(Video, Height, 240);
  LOAD_INI_INTEGER(Video, FPS, 30);

  LOAD_INI_STRING(Audio, MulticastAddress, "");
  LOAD_INI_INTEGER(Audio, MulticastRtpPort, 0);
  LOAD_INI_INTEGER(Audio, MulticastTTL, 0);
  LOAD_INI_STRING(Audio, Codec, "G711");
  LOAD_INI_INTEGER(Audio, Channels, 1);
  LOAD_INI_INTEGER(Audio, Frequency, 8);
  Audio.Frequency *= 1000;

  LOAD_INI_STRING(Metadata, MulticastAddress, "");
  LOAD_INI_INTEGER(Metadata, MulticastRtpPort, 0);
  LOAD_INI_INTEGER(Metadata, MulticastTTL, 0);

  LOAD_INI_STRING(Backchannel, MulticastAddress, "");
  LOAD_INI_STRING(Backchannel, Codec, "PCMU");

}

CFrameQueue::~CFrameQueue()
{
  CLocker l(Door);
  while (!Frames.empty()) {
    FrameInfo::Release(Frames.front());
    Frames.pop_front();
  };
}

void CFrameQueue::Push(FrameInfo* f)
{
  CLocker l(Door);
  PassedCount++;
  if (Frames.size() < MaxSize) {
    Frames.push_back(f);
  } else {
    FrameInfo::Release(f);
  }
}

FrameInfo* CFrameQueue::Peek()
{
  FrameInfo* f = NULL;
  CLocker l(Door);
  if (!Frames.empty()) {
    f = Frames.front();
  };
  return f;
}

FrameInfo* CFrameQueue::Pop()
{
  FrameInfo* f = NULL;
  CLocker l(Door);
  if (!Frames.empty()) {
    f = Frames.front();
    Frames.pop_front();
    PlayedCount++;
  };
  return f;
}

int CFrameQueue::GetCount()
{
  CLocker l(Door);
  return Frames.size();
}

int CStepLog::CreateNewStep(const char* Name)
{
  CLocker l(Door);
  LastStep++;
  Log.resize(LastStep + 1);
  int Step = GetCurrentStep();
  Log[Step].Name = Name;
  if (StepCallback) {
    StepCallback(ObjectId, Step, ESDT_Begin, Name);
  }
  return Step;
}

void CStepLog::SetRequest(const char* Text, int Step)
{
  if (Step < 0) {
    Step = LastStep;
  }
  CLocker l(Door);
  Step = VerifyStep(Step);
  Log[Step].Request = Text;
  if (StepCallback) {
    StepCallback(ObjectId, Step, ESDT_Send, Text);
  }
}

void CStepLog::SetResponse(const char* Text, int Step)
{
  if (Step < 0) {
    Step = LastStep;
  }
  CLocker l(Door);
  Step = VerifyStep(Step);
  Log[Step].Response = Text;
  if (StepCallback) {
    StepCallback(ObjectId, Step, ESDT_Receive, Text);
  }
}

void CStepLog::AddLogText(const char* Text, int Step)
{
  if (Step < 0) {
    Step = LastStep;
  }
  CLocker l(Door);
  Step = VerifyStep(Step);
  Log[Step].Log += Text;
  if (StepCallback) {
    StepCallback(ObjectId, Step, ESDT_Log, Text);
  }
}

void CStepLog::SetStepResult(int Result, const char* Text, int Step)
{
  if (Step < 0) {
    Step = LastStep;
  }
  CLocker l(Door);
  Step = VerifyStep(Step);
  Log[Step].Result = Result;
  if (StepCallback) {
    StepCallback(ObjectId, Step, Result ? ESDT_ResultError : ESDT_ResultOK, Text);
  }
}

int CStepLog::GetCurrentStep()
{
  return LastStep;
  //CLocker l(Door);
  //return Log.size() - 1;
}

int CStepLog::VerifyStep(int Step)
{
  if (Step < 0)
    Step = Log.size() - 1;
  if (Step < 0) {
    Log.resize(1);
    Step = 0;
  }
  if (Step >= Log.size()) {
    Step = Log.size() - 1;
  };
  return Step;
}


class CapsuleMediaSubsession : public IMediaSubsession
{
  SChannelSettings &Settings;
  bool Audio;
public:
  MediaSubsession* MediaSub;
  CapsuleMediaSubsession(SChannelSettings &Settings, bool Audio) : Settings(Settings), Audio(Audio), MediaSub(NULL) {}
  virtual unsigned short   videoWidth()  const { return Settings.Width; }
  virtual unsigned short   videoHeight() const { return Settings.Height; }
  virtual unsigned         videoFPS()    const { return Settings.FPS; }
  virtual unsigned         numChannels() const { return Settings.Channels; }
  virtual char const*      mediumName()  const { return Audio ? "audio" : "video"; }
  virtual char const*      codecName()   const { return Settings.Codec.c_str(); }
  virtual char const*      fmtp_config() const { return ""; }
  virtual char const*      fmtp_spropparametersets() const { return ""; }
  virtual unsigned         rtpTimestampFrequency()   const { return Audio ? 8000 : 90000; }
  virtual MediaSubsession* mediaSub() { return MediaSub; }
};


void CLiveCapsule::ConfigureWrapper()
{
  StepLog.SetObjectId(FullConfig.TestSequence.ObjectId);

  Live555Wrapper.AttachNotify(NotifyFunctionSend, NotifyFunctionReceive, NotifyFunctionLog, this);

  if (FullConfig.TestSequence.UseVideo) {
    Live555Wrapper.AttachChannel(ELC_Video, FullConfig.Video.Codec.c_str(), VideoDataFunction, this, 
      FullConfig.Video.MulticastAddress.c_str(), FullConfig.Video.MulticastRtpPort, FullConfig.Video.MulticastTTL);
    FramesVideo.Subsession = new CapsuleMediaSubsession(FullConfig.Video, false);
    FramesVideo.Format = ProbeByCodec(FramesVideo.Subsession);
  };
  if (FullConfig.TestSequence.UseAudio) {
    Live555Wrapper.AttachChannel(ELC_Audio, FullConfig.Audio.Codec.c_str(), AudioDataFunction, this,
      FullConfig.Audio.MulticastAddress.c_str(), FullConfig.Audio.MulticastRtpPort, FullConfig.Audio.MulticastTTL);
    FramesAudio.Subsession = new CapsuleMediaSubsession(FullConfig.Audio, true);
    FramesAudio.Format = ProbeByCodec(FramesAudio.Subsession);
  };
  if (FullConfig.TestSequence.UseMetadata) {
    Live555Wrapper.AttachChannel(ELC_Metadata, "", MetadataDataFunction, this);
  };

  if (FullConfig.TestSequence.UseBackchannel) {
    Live555Wrapper.AttachChannel(ELC_AudioBack, FullConfig.Backchannel.Codec.c_str(),
		NULL, this, FullConfig.Backchannel.MulticastAddress.c_str());
  };

  Live555Wrapper.AttachCompletion(CompleteFunction, this);

  Live555Wrapper.AttachCredentials(FullConfig.ControlConnection.User.c_str(), FullConfig.ControlConnection.Password.c_str());
  Live555Wrapper.Open(FullConfig.ControlConnection.Uri.c_str(), FullConfig.ControlConnection.Transport, FullConfig.ControlConnection.Port);

  PlayType = FullConfig.TestSequence.SequenceNumber;
  if (PlayType < 0) PlayType = 0;
  if (PlayType > SequenceCount) PlayType = 0;
}

void CLiveCapsule::NotifyFunctionSend(const void* UserData, ELiveStep Step, const char* MessageData)
{
  CLiveCapsule* pL = ((CLiveCapsule*)UserData);
  pL->StepLog.SetRequest(MessageData);
}

void CLiveCapsule::NotifyFunctionReceive(const void* UserData, ELiveStep Step, const char* MessageData)
{
  CLiveCapsule* pL = ((CLiveCapsule*)UserData);
  pL->StepLog.SetResponse(MessageData);
}

void CLiveCapsule::NotifyFunctionLog(const void* UserData, ELiveStep Step, const char* MessageData)
{
  CLiveCapsule* pL = ((CLiveCapsule*)UserData);
  pL->StepLog.AddLogText(MessageData);
}

void CLiveCapsule::VideoDataFunction(
  const void* UserData, 
  unsigned char* ReceiveBuffer, 
  unsigned FrameSize, 
  unsigned NumTruncatedBytes,
  struct timeval PresentationTime, 
  unsigned DurationInMicroseconds
  )
{
  CLiveCapsule* pL = ((CLiveCapsule*)UserData);
  FrameInfo *Info = pL->FramesVideo.Format->NextChunk(
    pL->FramesVideo.Subsession, ReceiveBuffer, FrameSize, pL->FramesVideo.Prefix);
  Info->frameHead.FrameType = pL->FramesVideo.Format->AsFOURCC();
  Info->frameHead.Start = 
    ((REFERENCE_TIME)PresentationTime.tv_sec) * 10000000 +
    ((REFERENCE_TIME)PresentationTime.tv_usec) * 10;
  pL->FramesVideo.Push(Info);
}

void CLiveCapsule::AudioDataFunction(
  const void* UserData, 
  unsigned char* ReceiveBuffer, 
  unsigned FrameSize, 
  unsigned NumTruncatedBytes,
  struct timeval PresentationTime, 
  unsigned DurationInMicroseconds
  )
{
  CLiveCapsule* pL = ((CLiveCapsule*)UserData);
  FrameInfo *Info = pL->FramesAudio.Format->NextChunk(
    pL->FramesAudio.Subsession, ReceiveBuffer, FrameSize, pL->FramesAudio.Prefix);
  Info->frameHead.FrameType = pL->FramesAudio.Format->AsFOURCC();
  Info->frameHead.Start = 
    ((REFERENCE_TIME)PresentationTime.tv_sec) * 10000000 +
    ((REFERENCE_TIME)PresentationTime.tv_usec) * 10;
  pL->FramesAudio.Push(Info);
}

void CLiveCapsule::MetadataDataFunction(
  const void* UserData, 
  unsigned char* ReceiveBuffer, 
  unsigned FrameSize, 
  unsigned NumTruncatedBytes,
  struct timeval PresentationTime, 
  unsigned DurationInMicroseconds
  )
{
  FrameInfo* Info = FrameInfo::Init(FrameSize + 1);
  memcpy(Info->pdata, ReceiveBuffer, FrameSize);
  Info->pdata[FrameSize] = 0;
  ((CLiveCapsule*)UserData)->FramesMeta.Push(Info);
}

bool GetErrorString(std::string &str, int Error)
{
    LPVOID lpMsgBuf;
    FormatMessageW( 
        FORMAT_MESSAGE_ALLOCATE_BUFFER | 
        FORMAT_MESSAGE_FROM_SYSTEM | 
        FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL,
        Error,
        MAKELANGID(LANG_ENGLISH, SUBLANG_DEFAULT), // Default language
        (LPWSTR) &lpMsgBuf,
        0,
        NULL 
        );
    if (lpMsgBuf) {
      char buffer[1024];
      WideCharToMultiByte(CP_UTF8, 0, (LPCWSTR)lpMsgBuf, -1, buffer, sizeof(buffer) - 1, NULL, NULL);
      str = buffer;
      LocalFree( lpMsgBuf );
      return true;
    } else {
      str.clear();
      return false;
    }
}

bool CLiveCapsule::GetSpecificErrorDescription(char *Buffer, ELiveStep Step, int &Result)
{
  if (Result == -10057) {
    if (((Step == ELS_Describe) || (Step == ELS_Options)) && (FullConfig.ControlConnection.Transport == ELT_HTTP)) {
      sprintf(Buffer, "Network error 10057 - probably HTTP tunnel is not working");
      return true;
    }
    if (Step == ELS_Teardown) {
      sprintf(Buffer, "Warning: Network error 10057 - probably device close connection before sending reply");
      StepLog.AddLogText(Buffer);
      Result = 0;
      return true;
    }
  }
  if (Result == RPC_E_TIMEOUT) {
    sprintf(Buffer, "Network error - timeout");
    return true;
  }
  if (Result == 401) {
    sprintf(Buffer, "RTSP error 401 - probably device is not accepting our authentication data");
    return true;
  }
  return false;
}

void CLiveCapsule::CompleteFunction(const void* UserData, ELiveStep Step, int Result, const char* Body)
{
  char Buffer[1024];
  Buffer[0] = 0;
  if (Result) {
    if (!((CLiveCapsule*)UserData)->GetSpecificErrorDescription(Buffer, Step, Result)) {
      if (Result < 0) {
        std::string ErrorText;
        if (GetErrorString(ErrorText, -Result)) {
          sprintf_s(Buffer, sizeof(Buffer) - 1, "Network error %i: %s", -Result, ErrorText.c_str());
        } else {
          sprintf_s(Buffer, sizeof(Buffer) - 1, "Network error %i", -Result);
        }
      } else {
        sprintf_s(Buffer, sizeof(Buffer) - 1, "Protocol error %i in %s", Result, Body);
      }
    }
  }
  ((CLiveCapsule*)UserData)->StepLog.SetStepResult(Result, Buffer);
}

bool CLiveCapsule::Play()
{
  if (PlayType) {
    return RunSequence(PlayType);
  } else {
    return PlayAsFilter();
  }
}

void CLiveCapsule::Pause()
{
  if (!PlayType) {
    if (GetCurrentStep() == ELS_Play) {
      StepLog.CreateNewStep("Pause");
      Live555Wrapper.Pause();
      Live555Wrapper.WaitResult();
    }
  }
}

void CLiveCapsule::Stop()
{
  if ((GetCurrentStep() >= ELS_Play) && (GetCurrentStep() < ELS_Teardown)) {
    StepLog.CreateNewStep("Teardown");
    Live555Wrapper.Teardown();
    Live555Wrapper.WaitResult();
  }
}

bool CLiveCapsule::PlayAsFilter()
{
  if (!Prepare()) {
    return false;
  }
  StepLog.CreateNewStep("Play");
  Live555Wrapper.Play();
  Live555Wrapper.WaitResult();
  return true;
}

void CLiveCapsule::FullStop()
{
  StepLog.CreateNewStep("Teardown");
  Live555Wrapper.Teardown();
  Live555Wrapper.WaitResult(FullConfig.ControlConnection.Timeout);
}

bool CLiveCapsule::PlayAsTestBase()
{
  //if (!Prepare(50)){
  if (!Prepare(FullConfig.ControlConnection.Timeout)) {
    return false;
  }
  StepLog.CreateNewStep("Play");
  Live555Wrapper.Play();
  if (Live555Wrapper.WaitResult(FullConfig.ControlConnection.Timeout))
    return false;
  return true;
}

bool CLiveCapsule::Configure(const char* FileName)
{
  FullConfig.Load(FileName);
  ConfigureWrapper();
  return true;
}

bool CLiveCapsule::Configure(const WCHAR* FileName)
{
  char *filename = ToMulti(FileName);
  FullConfig.Load(filename);
  delete[] filename;
  ConfigureWrapper();
  return true;
}

bool CLiveCapsule::HasAudio() const
{
  return FullConfig.TestSequence.UseAudio;
}

bool CLiveCapsule::HasVideo() const
{
  return FullConfig.TestSequence.UseVideo;
}

bool CLiveCapsule::HasMeta() const
{
  return FullConfig.TestSequence.UseMetadata;
}

bool CLiveCapsule::IsFakeVideo() const
{
  return FullConfig.TestSequence.UseMetadata && !FullConfig.TestSequence.UseVideo;
}

RTSPClientState CLiveCapsule::GetState() const
{
  return RTSP_STATE_IDLE;
}

bool CLiveCapsule::GetVideoFormat(CMediaType *pmt)
{
  if (!FramesVideo.Format || !FramesVideo.Subsession)
    return false;
  return FramesVideo.Format->GetFormat(pmt, FramesVideo.Subsession, &FramesVideo.Prefix);
}

bool CLiveCapsule::CheckVideoFormat(const CMediaType *pmt) const
{
  if (!FramesVideo.Format || !FramesVideo.Subsession)
    return false;
  return FramesVideo.Format->CheckFormat(pmt, FramesVideo.Subsession);
}

bool CLiveCapsule::GetAudioFormat(CMediaType *pmt)
{
  if (!FramesAudio.Format || !FramesAudio.Subsession)
    return false;
  return FramesAudio.Format->GetFormat(pmt, FramesAudio.Subsession, &FramesAudio.Prefix);
}

bool CLiveCapsule::CheckAudioFormat(const CMediaType *pmt) const
{
  if (!FramesAudio.Format || !FramesAudio.Subsession)
    return false;
  return FramesAudio.Format->CheckFormat(pmt, FramesAudio.Subsession);
}

FrameInfo* CLiveCapsule::GetTop(bool Audio)
{
  if (Audio) {
    return FramesAudio.Pop();
  } else {
    return FramesVideo.Pop();
  };
}

int CLiveCapsule::GetCount(bool Audio)
{
  if (Audio) {
    return FramesAudio.GetCount();
  } else {
    return FramesVideo.GetCount();
  };
}

FrameInfo* CLiveCapsule::GetMeta()
{
  return FramesMeta.Pop();
}

void CLiveCapsule::InterruptableSleep(int Timeout)
{
  InSleep = true;
  int Count = Timeout / 100;
  while (InSleep && !Terminating && (Count --> 0)) {
    Sleep(100);
  };
  InSleep = false;
}

void CLiveCapsule::ExtractResolutionFromSdp()
{
  if (strcmp(FramesVideo.Subsession->mediaSub()->codecName(), "MP4V-ES") == 0) {
    if (const char *fmtp_config = FramesVideo.Subsession->mediaSub()->fmtp_config()) {
      unsigned char buffer[128];
      unsigned size = 0;
      unsigned i = 0;
      if (const char *vol_header = strstr(fmtp_config, "00000120")) {
        size = strlen(vol_header) / 2;
        for (i = 0; i < size; ++i)
          if (1 != sscanf(&vol_header[2 * i], "%2X", &buffer[i])) break;
      }
      if (i == size && size) {
        ParseMPEG4VOL(buffer, size, FramesVideo.Subsession->mediaSub()->readSource());
      }
    }
  }
  else if (strcmp(FramesVideo.Subsession->mediaSub()->codecName(), "H264") == 0) {
    unsigned int num = 0;
    SPropRecord *SPropRecords = parseSPropParameterSets(FramesVideo.Subsession->mediaSub()->fmtp_spropparametersets(), num);
    if (SPropRecords && num > 0) {
      ParseH264SPS(SPropRecords[0].sPropBytes, SPropRecords[0].sPropLength, FramesVideo.Subsession->mediaSub()->readSource());
    }
  }
}

bool CLiveCapsule::CheckResolution()
{
  char Buffer[100];
  StepLog.CreateNewStep("Checking actual resolution");
  sprintf(Buffer, "Requested resolution: %u x %u", FullConfig.Video.Width, FullConfig.Video.Height);
  StepLog.AddLogText(Buffer);
  int Width = 0;
  int Height = 0;

  if (FramesVideo.Subsession->mediaSub()) {
    if (FramesVideo.Subsession->mediaSub()->readSource()->IsFrameDimensionsParsed()) {
      Width = FramesVideo.Subsession->mediaSub()->readSource()->fFrameWidth;
      Height = FramesVideo.Subsession->mediaSub()->readSource()->fFrameHeight;
    }

	  if (Width <= 0 || Height <= 0)
	  {
	    ExtractResolutionFromSdp();
      Width = FramesVideo.Subsession->mediaSub()->readSource()->fFrameWidth;
      Height = FramesVideo.Subsession->mediaSub()->readSource()->fFrameHeight;
	  }
  }
  sprintf(Buffer, "Detected resolution: %u x %u", Width, Height);
  StepLog.AddLogText(Buffer);
  int Tolerance = 1;
  if (FullConfig.Video.Codec == "JPEG") {
    Tolerance = 8;
  }
  if (FullConfig.Video.Width != Width || FullConfig.Video.Height != Height) {
    StepLog.AddLogText("Warning: Resolution not matching exactly!");
  }
  if (!(Width  >= FullConfig.Video.Width-Tolerance  && Width  <= FullConfig.Video.Width+Tolerance) || 
      !(Height >= FullConfig.Video.Height-Tolerance && Height <= FullConfig.Video.Height+Tolerance)
      ) {
    StepLog.SetStepResult(1, "Verification failed!");
    return false;
  }
  StepLog.SetStepResult();
  return true;
}

bool GetChannelPackets(CLive555Wrapper& wrap, ELiveChannels Channel, int& Packets)
{
  Packets = 0;

  RTPSource* source = wrap.GetChannelSource(Channel);
  if (!source)
    return false;

  RTPReceptionStatsDB& db = source->receptionStatsDB();
  Packets = db.totNumPacketsReceived();
  return true;
}

bool CLiveCapsule::CheckFrames(bool MakeStep, int Time)
{
  char Buffer[100];
  bool Ret = true;
  if (MakeStep) {
    StepLog.CreateNewStep("Checking media frames count");
  }
  if (Time < 1) Time = 1;
  if (FullConfig.TestSequence.UseVideo) {
    sprintf(Buffer, "%i video frames captured (%.1f FPS), %i played", FramesVideo.GetPassed(), double(FramesVideo.GetPassed()) * 1000 / Time, FramesVideo.GetPlayed());
    StepLog.AddLogText(Buffer);
    if (FramesVideo.GetPassed() <= 0) {
      Ret = false;
      int Packets;
      if (GetChannelPackets(Live555Wrapper, ELC_Video, Packets)) {
        if (Packets > 0) {
          sprintf(Buffer, "%i RTP packet(s) captured, maybe they are wrong formatted or network is noisy", Packets);
          StepLog.AddLogText(Buffer);
        }
      }
    };
    if ((FramesVideo.GetPlayed() <= 0) && (FramesVideo.GetPassed() > 0)) {
      StepLog.AddLogText("Warning: maybe video decoding filters are not active");
    };
  }
  if (FullConfig.TestSequence.UseAudio) {
    sprintf(Buffer, "%i audio frames captured, %i played", FramesAudio.GetPassed(), FramesAudio.GetPlayed());
    StepLog.AddLogText(Buffer);
    if (FramesAudio.GetPassed() <= 0) {
      Ret = false;
      int Packets;
      if (GetChannelPackets(Live555Wrapper, ELC_Audio, Packets)) {
        if (Packets > 0) {
          sprintf(Buffer, "%i RTP packet(s) captured, maybe they are wrong formatted or network is noisy", Packets);
          StepLog.AddLogText(Buffer);
        }
      }
    };
    if ((FramesAudio.GetPlayed() <= 0) && (FramesAudio.GetPassed() > 0)){
      StepLog.AddLogText("Warning: maybe audio decoding filters are not active");
    };
  }
  if (MakeStep) {
    if (Ret) {
      StepLog.SetStepResult();
    } else {
      StepLog.SetStepResult(1, "No frames captured");
    }
  }
  return Ret;
}

bool CLiveCapsule::CheckOptions(unsigned int Timeout)
{
  const char* Body;
  if (FullConfig.TestSequence.CheckOptions) {
    StepLog.CreateNewStep("Options");
    Live555Wrapper.Options();
    if (Live555Wrapper.WaitResult(Body, Timeout))
      return false;

  char Buffer[512];
  sprintf_s(Buffer, 511, "Options [%s]", Body);
  StepLog.CreateNewStep("Checking Options");
  StepLog.AddLogText(Buffer);
  bool Ret = true;
#define CheckOptionsParam(param)              \
    if (!strstr(Body, param)) {               \
      sprintf(Buffer, "Missed [%s]", param);  \
      StepLog.AddLogText(Buffer);             \
      Ret = false; }
    CheckOptionsParam("SET_PARAMETER");
    CheckOptionsParam("DESCRIBE");
    CheckOptionsParam("SETUP");
    CheckOptionsParam("PLAY");
    CheckOptionsParam("TEARDOWN");

    if (Ret) {
      StepLog.SetStepResult();
    } else {
      StepLog.SetStepResult(1, "Some options missed");
    }
    return Ret;
  } else {
    return true;
  }
}

bool CLiveCapsule::Prepare(unsigned int Timeout)
{
  if ((GetCurrentStep() < ELS_Play) || (GetCurrentStep() >= ELS_Teardown)) {

    if (!CheckOptions(Timeout))
      return false;

    const char* Body;

    StepLog.CreateNewStep("Describe");
    Live555Wrapper.Describe();
    if (Live555Wrapper.WaitResult(Body, Timeout))
      return false;

    StepLog.CreateNewStep("Create Media Session");
    if (!Live555Wrapper.CreateMediaSessionBySdp(Body)) {
      StepLog.SetStepResult(1, "Error creating media session");
      return false;
    }
    StepLog.SetStepResult();

    int Channels = 0;
    if (FullConfig.TestSequence.UseVideo) {
      ((CapsuleMediaSubsession*)FramesVideo.Subsession)->MediaSub = Live555Wrapper.GetChannelSession(ELC_Video);
      Channels++;
    }
    if (FullConfig.TestSequence.UseAudio) Channels++;
    if (FullConfig.TestSequence.UseMetadata) Channels++;
    if (FullConfig.TestSequence.UseBackchannel) Channels++;
    for (int i = 0; i < Channels; i++) {
      StepLog.CreateNewStep("Setup");
      Live555Wrapper.SetupSub(i);
      if (Live555Wrapper.WaitResult(Timeout))
        return false;
    }

    StepLog.CreateNewStep("Create Sinks");
    if (!Live555Wrapper.CreateSinks()) {
      StepLog.SetStepResult(1, "Error creating sinks");
      return false;
    }
    StepLog.SetStepResult();
  }
  return true;
}

bool CLiveCapsule::PrepareRtp()
{
  if ((GetCurrentStep() < ELS_Play) || (GetCurrentStep() >= ELS_Teardown)) {
    StepLog.CreateNewStep("Create Media Session");
    if (!Live555Wrapper.CreateRtpSubsessionsByChannels()) {
      StepLog.SetStepResult(1, "Error creating media session");
      return false;
    }
    StepLog.SetStepResult();

    StepLog.CreateNewStep("Create Sinks");
    if (!Live555Wrapper.CreateSinks()) {
      StepLog.SetStepResult(1, "Error creating sinks");
      return false;
    }
    StepLog.SetStepResult();
  }
  return true;
}

bool CLiveCapsule::CheckTermination()
{
  if (Terminating) {
    if (Halt) {
      StepLog.SetStepResult(0, "Forced stop - halt button pressed");
    } else {
      StepLog.SetStepResult(0, "Forced stop - rolling back");
    }
    return false;
  } else {
    StepLog.SetStepResult();
    return true;
  }
}

bool CLiveCapsule::PlayAsTest1TimeWait()
{
  if (!PlayAsTestBase())
    return false;

  int Start = GetTickCount();
  StepLog.CreateNewStep("Waiting for 10 seconds");
  InterruptableSleep(10000);
  if (!CheckTermination())
    return false;

  if (!CheckFrames(true, GetTickCount() - Start)) {
    return false;
  }

  FullStop();
  return true;
}

bool CLiveCapsule::PlayAsTest2FramesWait()
{
  if (!PlayAsTestBase())
    return false;

  int MaxFrames = 100;
  int Frames = MaxFrames;
  int WaitingTimeout = FullConfig.TestSequence.Timeout;
  int CriticalFrames = WaitingTimeout * FullConfig.Video.FPS / 2000;
  if (Frames > CriticalFrames) {
    if (CriticalFrames < 3) {
      CriticalFrames = 3;
      if (FullConfig.Video.FPS >= 1) {
        WaitingTimeout = CriticalFrames * 2000 / FullConfig.Video.FPS;
      } else {
        WaitingTimeout = CriticalFrames * 2000;
      }
    }
    Frames = CriticalFrames;
  }
  char Buffer[128];
  sprintf(Buffer, "Waiting for %i frames up to %i ms ", Frames, WaitingTimeout);
  StepLog.CreateNewStep(Buffer);
  int Start = GetTickCount();
  int End = Start + WaitingTimeout;
  while (!Terminating && (FramesVideo.GetPassed() < Frames)) {
    if (GetTickCount() > End) {
      int Time = FullConfig.TestSequence.Timeout;
      if (Time < 1) Time = 1;
      double FPS = double(FramesVideo.GetPassed()) * 1000 /Time;
      sprintf(Buffer, "Only %i frames captured (%.1f FPS)", FramesVideo.GetPassed(), FPS);
      StepLog.AddLogText(Buffer);
      if (FPS > 0.001) {
        int TimeoutHint = MaxFrames / FPS + 2;
        sprintf(Buffer, "Please increase Operation Delay to %i ms in case device is working properly!", TimeoutHint * 1000);
        StepLog.AddLogText(Buffer);
      } else {
        StepLog.AddLogText("Please increase Operation Delay in case device is working properly!");
      }
      int Packets;
      if (GetChannelPackets(Live555Wrapper, ELC_Video, Packets)) {
        if (Packets > 0) {
          sprintf(Buffer, "%i RTP packet(s) captured, maybe they are wrong formatted or network is noisy", Packets);
          StepLog.AddLogText(Buffer);
        }
      }
      StepLog.SetStepResult(1, "Frames waiting timeout.");
      return false;
    }
    Sleep(100);
  };
  CheckFrames(false, GetTickCount() - Start);
  if (!CheckTermination())
    return false;

  if (FullConfig.TestSequence.CheckActualResolution) {
    if (!CheckResolution()) {
      return false;
    }
  }

  FullStop();
  return true;
}

bool CLiveCapsule::PlayAsTest3TimeWaitRtp()
{
  if (!PrepareRtp())
    return false;

  int Start = GetTickCount();
  StepLog.CreateNewStep("Waiting for 10 seconds");
  InterruptableSleep(10000);
  if (!CheckTermination())
    return false;

  if (!CheckFrames(true, GetTickCount() - Start)) {
    return false;
  }

  //FullStop();
  return true;
}

bool CLiveCapsule::PlayAsTest4Backchannel()
{
  Live555Wrapper.SetAdditionalFields("Require: www.onvif.org/ver20/backchannel\r\n");
  if (!PlayAsTestBase())
    return false;

  int Start = GetTickCount();
  StepLog.CreateNewStep("Waiting for 10 seconds");
  InterruptableSleep(10000);
  if (!CheckTermination())
    return false;

  FullStop();
  return true;
}

bool CLiveCapsule::PlayAsTest5Playback()
{
  int Timeout = FullConfig.ControlConnection.Timeout;

  ELiveChannels MasterChannel = ELC_Error;
  if (FullConfig.TestSequence.UseVideo) {
    MasterChannel = ELC_Video;
  }
  if (FullConfig.TestSequence.UseAudio && (MasterChannel == ELC_Error)) {
    MasterChannel = ELC_Audio;
  }
  if (FullConfig.TestSequence.UseMetadata && (MasterChannel == ELC_Error)) {
    MasterChannel = ELC_Metadata;
  }

  if ((GetCurrentStep() < ELS_Play) || (GetCurrentStep() >= ELS_Teardown)) {

    if (!CheckOptions(Timeout))
      return false;

    const char* Body;

    StepLog.CreateNewStep("Describe");
    Live555Wrapper.Describe();
    if (Live555Wrapper.WaitResult(Body, Timeout))
      return false;

    StepLog.CreateNewStep("Create Media Session");
    if (!Live555Wrapper.CreateMediaSessionBySdp(Body)) {
      StepLog.SetStepResult(1, "Error creating media session");
      return false;
    }
    StepLog.SetStepResult();


    Live555Wrapper.SetAdditionalFields(FullConfig.TestSequence.CustomSetupFields.c_str());

    int Channels = 0;
    if (FullConfig.TestSequence.UseVideo) {
      ((CapsuleMediaSubsession*)FramesVideo.Subsession)->MediaSub = Live555Wrapper.GetChannelSession(ELC_Video);
      Channels++;
    }
    if (FullConfig.TestSequence.UseAudio) Channels++;
    if (FullConfig.TestSequence.UseMetadata) Channels++;
    if (FullConfig.TestSequence.UseBackchannel) Channels++;
    for (int i = 0; i < Channels; i++) {
      StepLog.CreateNewStep("Setup");
      Live555Wrapper.SetupSub(i);
      if (Live555Wrapper.WaitResult(Timeout))
        return false;
    }

    StepLog.CreateNewStep("Create Sinks");
    if (!Live555Wrapper.CreateSinks()) {
      StepLog.SetStepResult(1, "Error creating sinks");
      return false;
    }
    StepLog.SetStepResult();
  }

  std::string &CustomPlayFields = FullConfig.TestSequence.CustomPlayFields;
  char Delim = CustomPlayFields.empty() ? 0 : CustomPlayFields[0];
  int pos = CustomPlayFields.find(Delim, 1);
  std::string CustomPlayFieldsLoc;
  if (pos > 0) {
    CustomPlayFieldsLoc = CustomPlayFields.substr(1, pos - 1);
    CustomPlayFields = CustomPlayFields.substr(pos);
  } else {
    if (!CustomPlayFields.empty()) {
      CustomPlayFieldsLoc = CustomPlayFields.substr(1);
      CustomPlayFields.clear();
    }
  }

  Live555Wrapper.SetAdditionalFields(CustomPlayFieldsLoc.c_str());

  PlaybackFrameLog fl(StepLog);
  Live555Wrapper.AttachRtpCallback(MasterChannel, PlaybackFrameLog::RtpExtHdrCallback, &fl);

  StepLog.CreateNewStep("Play");
  Live555Wrapper.Play(-1.0);
  Live555Wrapper.SetAdditionalFields(NULL);
  if (Live555Wrapper.WaitResult(Timeout))
    return false;

  int Start = GetTickCount();
  StepLog.CreateNewStep("Wait Stream");
//  StepLog.CreateNewStep("Waiting for 10 seconds");
  InterruptableSleep(10000);
  Live555Wrapper.AttachRtpCallback(MasterChannel, NULL, NULL);

  char Buffer[128];
  sprintf(Buffer, "Frames received: %d", fl.GetRecords());
  StepLog.AddLogText(Buffer);
  if (!fl.GetRecords()) {
    StepLog.SetStepResult(1, "No frames captured");
    return false;
  }
  if (!CheckTermination())
    return false;

  return true;
}

bool CLiveCapsule::RunStep(int Step) 
{ 
  StepLog.CreateNewStep(GetLiveStepName(ELiveStep(Step)));
  return Live555Wrapper.RunCommand(ELiveStep(Step));
};

bool CLiveCapsule::RunStepSync(int Step)
{
  if (!RunStep(Step))
    return false;
  if (Live555Wrapper.WaitResult(FullConfig.ControlConnection.Timeout))
    return false;
  return true;
}

bool CLiveCapsule::RunSequence(int Sequence)
{
  if (IsInSequence())
    return false;
  if (Sequence < 0)
    return false;
  if (Sequence > SequenceCount)
    return false;
  InSequence = true;
  this->Sequence = Sequence;
  return true;
}

void CLiveCapsule::RunSequenceInternal(int Sequence)
{
  InSequence = true;
  switch (Sequence) {
  case 0: PlayAsFilter(); break;
  case 1: PlayAsTest1TimeWait(); break;
  case 2: PlayAsTest2FramesWait(); break;
  case 3: PlayAsTest3TimeWaitRtp(); break;
  case 4: PlayAsTest4Backchannel(); break;
  case 5: PlayAsTest5Playback(); break;
  default:  break;
  };
  InSequence = false;
}

bool CLiveCapsule::IsInSequence()
{
  return InSequence;
}

void CLiveCapsule::Thread()
{
  while (!Terminating) {
    if (Sequence) {
      RunSequenceInternal(Sequence);
      Sequence = 0;
    };
    Sleep(100);
  }
}

void CLiveCapsule::ClearMutators()
{
  InSleep = false;
  Live555Wrapper.AttachRtpCallback(ELC_Video, NULL, NULL);
  Live555Wrapper.SetAdditionalFields(NULL);
}


TSharpStepCallback GlobalStepCallback;
extern "C"   __declspec(dllexport) 
int   
__cdecl   
SetGlobalStepCallback(TSharpStepCallback StepCallback)
{
  GlobalStepCallback = StepCallback;
  return 0;
}


extern "C"   __declspec(dllexport) 
int   
__cdecl   
RunTestSession(const char* Filepath)
{
  CLiveCapsule test;
  test.SetNotificationCallback(GlobalStepCallback);
  if (!test.Configure(Filepath)) {
    return 1;
  };
  if (!test.Play()) {
    return 2;
  }

  do {
    Sleep(1000);
  } while(test.IsInSequence());
  Sleep(1000);

  return 0;
}

extern "C"   __declspec(dllexport) 
int   
__cdecl   
StartTestSession(const char* Filepath)
{
  CLiveCapsule* p = new CLiveCapsule();
  p->SetNotificationCallback(GlobalStepCallback);
  if (!p->Configure(Filepath)) {
    return 0;
  };
  if (!p->Play()) {
    return 0;
  }
  /*do {
    Sleep(1000);
  } while(p->IsInSequence());*/
  Sleep(1000);

  return int(p);
}

extern "C"   __declspec(dllexport) 
int   
__cdecl   
IsTestSequence(int Session)
{
  if (!Session) {
    return 0;
  }
  CLiveCapsule* p = (CLiveCapsule*)Session;
  return p->IsInSequence();
}

extern "C"   __declspec(dllexport) 
int   
__cdecl   
ClearTestSequence(int Session)
{
  if (!Session) {
    return 0;
  }
  CLiveCapsule* p = (CLiveCapsule*)Session;
  p->ClearMutators();
  return 1;
}

extern "C"   __declspec(dllexport) 
int   
__cdecl   
CallTestSequence(int Session, int Sequence)
{
  if (!Session) {
    return 0;
  }
  CLiveCapsule* p = (CLiveCapsule*)Session;
  p->RunSequence(Sequence);
 /* do {
    Sleep(1000);
  } while(p->IsInSequence());*/
  Sleep(1000);
  return 1;
}

extern "C"   __declspec(dllexport) 
int   
__cdecl   
CallTestCommand(int Session, int Command)
{
  if (!Session) {
    return 0;
  }
  CLiveCapsule* p = (CLiveCapsule*)Session;
  return !p->RunStepSync(Command);
}

extern "C"   __declspec(dllexport) 
int   
__cdecl   
CloseTestSession(int Session)
{
  if (!Session) {
    return 0;
  }
  CLiveCapsule* p = (CLiveCapsule*)Session;
  if (p->IsNeedTeardown()) {
    p->RunStepSync(ELS_Teardown);
  }
  delete p;
  return 0;
}

