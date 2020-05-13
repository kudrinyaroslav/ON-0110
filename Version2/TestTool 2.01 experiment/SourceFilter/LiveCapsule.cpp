///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include "LiveCapsule.h"

static const WCHAR ApplicationW[] = L"Test Options";
static const  char ApplicationA[] =  "Test Options";
static const  char ReplyA[]       =  "Test State";


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

#if 0
static FILE* file = NULL;
void DropDebug1(const char* Format, ...)
{
  va_list ap;
  char Buffer[2048];
  int size = 0;
  va_start(ap, Format);
  size += _vsnprintf(Buffer + size, sizeof(Buffer), Format, ap);
  va_end(ap);
  OutputDebugStringA(Buffer);
  /*if (!file)
  {
    file = fopen("c:\\onvifrtsp.txt", "wb");
  }
  if (file)
  {
    fwrite(Buffer, size, 1, file);
  }*/
}
void CloseLog()
{
  if (file)
  {
    fclose(file);
    file = NULL;
  }
}
#endif


void MessageLog::AttachIni(const char* Name)
{
  if (!Name) return;
  strncpy(IniName, Name, sizeof(IniName) - 1);
  WritePrivateProfileStringA(ReplyA,
    "MessageCount",
    "0",
    IniName);
  ErrorReport = 0;
}

void MessageLog::DropIni(const char* Message)
{
  if (!IniName[0]) return;
  char key[32];
  ErrorReport++;
  sprintf(key, "Log%03i", ErrorReport);
  WritePrivateProfileStringA(ReplyA,
    key,
    Message,
    IniName);
  sprintf(key, "%i", ErrorReport);
  WritePrivateProfileStringA(ReplyA,
    "MessageCount",
    key,
    IniName);
}

void MessageLog::Drop(const char* Format, ...)
{
#if 0
  LPOLESTR _filename = (LPOLESTR)CoTaskMemAlloc(30);
  if (!_filename) return;
  static int i = 0;

  wsprintf(_filename, L"Call %i", i++);
  Lines.PutQueueObject(_filename);
#else
  va_list ap;
  char Buffer[2048];
  int size = 0;
  va_start(ap, Format);
  size += _vsnprintf(Buffer + size, sizeof(Buffer), Format, ap);
  va_end(ap);
  size++;

  DropIni(Buffer);
  OutputDebugStringA(Buffer);
/*
  //int size2 = MultiByteToWideChar(CP_ACP, 0, Buffer, size, NULL, 0);

  LPOLESTR _filename = (LPOLESTR)CoTaskMemAlloc(size * 2);
  if (!_filename) return;

  MultiByteToWideChar(CP_ACP, 0, Buffer, size, _filename, size);
  
  Lines.PutQueueObject(_filename);
  */
#endif
  //InterlockedIncrement(&Count);
}
/*
void MessageLog::Drop(const WCHAR* Format, ...)
{
  va_list ap;
  WCHAR Buffer[2048];
  int size = 0;
  va_start(ap, Format);
  size += _snwprintf(Buffer + size, sizeof(Buffer), Format, ap);
  va_end(ap);
  size++;

  LPOLESTR _filename = (LPOLESTR)CoTaskMemAlloc(size);
  if (!*_filename) return;

  wcscpy(_filename, Buffer);

  Lines.PutQueueObject(_filename);
  InterlockedIncrement(&Count);
}
*/
LPOLESTR MessageLog::GetTop()
{
  InterlockedDecrement(&Count);
  return Lines.GetQueueObject();
}

void MessageLog::SetStep(int Step)
{
  this->Step = Step;

  if (!IniName[0]) return;
  char key[32];
  char val[32];
  sprintf(key, "Step%03i", Step);
  sprintf(val, "%03i", ErrorReport+1);
  WritePrivateProfileStringA(ReplyA,
    key,
    val,
    IniName);

  sprintf(val, "%i", Step);
  WritePrivateProfileStringA(ReplyA,
    "CurrentStep",
    val,
    IniName);
}

void MessageLog::SetStepEnd(int PassBits, int FailBits)
{
  if (!IniName[0]) return;
  char key[32];
  char val[32];
  sprintf(key, "Step%03i", Step+1);
  sprintf(val, "%03i", ErrorReport+1);
  WritePrivateProfileStringA(ReplyA,
    key,
    val,
    IniName);

  sprintf(val, "%i", PassBits);
  WritePrivateProfileStringA(ReplyA,
    "PassBits",
    val,
    IniName);

  sprintf(val, "%i", FailBits);
  WritePrivateProfileStringA(ReplyA,
    "FailBits",
    val,
    IniName);
}


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

// HACK begin
int AssumeWidth = 0;
int AssumeHeight = 0;
// HACK end


void CapsuleSettings::Load(const WCHAR* FileName)
{
  char *filename = ToMulti(FileName);

  GetPrivateProfileStringA(ApplicationA,
    "Address",
    "rtsp://127.0.0.1",
    Address,
    elementsof(Address),
    filename);

  OutputDebugStringA(Address);
  TunnelPortNumber = 0;
  if (!strncmp(Address, "http", 4))
  {
    TunnelPortNumber = 80;
  }

  // HACK begin
  AssumeWidth = GetPrivateProfileIntW(ApplicationW,
    L"VideoWidth",
    0,
    FileName);
  AssumeHeight = GetPrivateProfileIntW(ApplicationW,
    L"VideoHeight",
    0,
    FileName);

  UseAudio = !!GetPrivateProfileIntW(ApplicationW,
    L"UseAudio",
    0,
    FileName);
  // HACK end

  TimeOut = GetPrivateProfileIntW(ApplicationW,
    L"Timeout",
    TimeOut,
    FileName);
  TimeOut = (TimeOut+500) / 1000;
  if (TimeOut <   2) TimeOut =   2;
  if (TimeOut > 600) TimeOut = 600;

  TunnelPortNumber = GetPrivateProfileIntW(ApplicationW,
    L"TunnelPortNumber",
    TunnelPortNumber,
    FileName);

  UseTCPTunnel = !!GetPrivateProfileIntW(ApplicationW,
    L"UseTCPTunnel",
    TunnelPortNumber != 0,
    FileName);
  
  UseOptions = !!GetPrivateProfileIntW(ApplicationW,
    L"UseOptions",
    0,
    FileName);

  UseKeepAlive = !!GetPrivateProfileIntW(ApplicationW,
    L"UseKeepAlive",
    0,
    FileName);
  // hack
  //UseKeepAlive = true;

  UseMetadata = !!GetPrivateProfileIntW(ApplicationW,
    L"UseMetadata",
    0,
    FileName);

  GetPrivateProfileStringA(ApplicationA,
    "User",
    "",
    User,
    elementsof(User),
    filename);

  GetPrivateProfileStringA(ApplicationA,
    "Password",
    "",
    Password,
    elementsof(Password),
    filename);

  delete[] filename;

  if ((TunnelPortNumber != 0) && UseTCPTunnel)
  {
    //UseKeepAlive = false;
  }

  memcpy(Address, "rtsp", 4);
}

bool LiveCapsule::InitEnvironment()
{
  RunStep(step_InitEnvironment);
  StopFlag = 0;
  scheduler =  BasicTaskScheduler::createNew();
  if (scheduler ==	NULL)
  {
    Log->Drop("ERROR in BasicTaskScheduler");
    goto fail;
  }
  env = BasicUsageEnvironment::createNew(*scheduler);
  if (env == NULL)
  {
    Log->Drop("BasicUsageEnvironment fail");
    goto fail;
  }
  rtsp = RTSPClient::createNew(*env, 0, AppName, Settings.TunnelPortNumber);
  if (rtsp == NULL)
  {		 
    Log->Drop("create rtsp client fail");
    goto fail;
  }
  SetStepOK(step_InitEnvironment);
  return true;
fail:
  SetStepFail(step_InitEnvironment);
  return false;
}

void LiveCapsule::HaltEnvironment()
{
  //RunStep(step_HaltEnvironment);
  //CloseLog();
  State = RTSP_STATE_IDLE;

  if (ms)
  {
    Medium::close(ms);
    ms = NULL;
  }
  if(rtsp) 
  {
    RTSPClient::close(rtsp);			
    rtsp = NULL;
  }
  if(env)
  {
    env->reclaim();
    env = NULL;
  }
  if (scheduler)
  {
    delete scheduler;
    scheduler = NULL;
  }
  if (Video) delete Video;
  Video = NULL;
  if (Audio) delete Audio;
  Audio = NULL;
  //SetStepOK(step_HaltEnvironment);
}

bool LiveCapsule::RunThread()
{
  StopFlag = 0;
  if (!ThreadExists())
    if (!Create())
    {
      Log->Drop("rtspClientPlayStream receive thread failed");
      return false;
    }
    return true;
}

void LiveCapsule::StopThread()
{
  if (StopFlag) return; // thread already stopped

  RunStep(step_StopThread);

  StopFlag = 1;

  // Disable warning: Conversion from LONG to PVOID of greater size
#pragma warning(push)
#pragma warning(disable: 4312)
  HANDLE hThread = (HANDLE)InterlockedExchangePointer(&m_hThread, 0);
#pragma warning(pop)
 try {
  if (hThread) {
    if (WaitForSingleObject(hThread, 2000) == WAIT_TIMEOUT)
    {
      Log->Drop("Forcing thread to close");
      TerminateThread(hThread, 1);
    }
    CloseHandle(hThread);
  }
 } catch (...) {};

  SetStepOK(step_StopThread);
}

bool LiveCapsule::CheckOptions(const char *Options)
{
  if (!Options) 
  {
    return false;
  }
#if 1
  bool Ret = true;
#define CheckOptionsParam(param)      \
  if (!strstr(Options, param)) {      \
    Log->Drop("Missed [%s]", param); \
    Ret = false; }
  CheckOptionsParam("SET_PARAMETER");
  //CheckOptionsParam("OPTIONS");
  CheckOptionsParam("DESCRIBE");
  CheckOptionsParam("SETUP");
  CheckOptionsParam("PLAY");
  CheckOptionsParam("TEARDOWN");
#else
  bool Ret = 
    !!strstr(Options, "SET_PARAMETER") &&
    //!!strstr(Options, "OPTIONS") &&
    !!strstr(Options, "DESCRIBE") &&
    !!strstr(Options, "SETUP") &&
    !!strstr(Options, "PLAY") &&
    !!strstr(Options, "TEARDOWN");
#endif
  return Ret;
}

extern unsigned lastResponseCode;
void LiveCapsule::CheckConnectionState()
{
  if ((lastResponseCode != 200) && (lastResponseCode != 0) && (lastResponseCode < 1000))
  {
    Log->Drop("Connection error, HTTP / RTSP code %i", lastResponseCode);
  }
}

char* LiveCapsule::DoOptions(const char* filename)
{
  if (Settings.UseOptions)
  {
    RunStep(step_OPTIONS);
    char* Options = rtsp->sendOptionsCmd(filename, Settings.User, Settings.Password, NULL, Settings.TimeOut);
    if (!Options)
    {
      CheckConnectionState();
      SetStepFail(step_OPTIONS);
      return NULL;
    }
    Log->Drop("Supported options are [%s]", Options);
    SetStepOK(step_OPTIONS);

    RunStep(step_CheckOptions);
    if (!CheckOptions(Options))
    {
      SetStepFail(step_CheckOptions);
      return NULL;
    }
    SetStepOK(step_CheckOptions);
    return Options;
  }
  return "";
}


char* LiveCapsule::DescribeStream(const char* filename)
{
  //DoOptions(filename);
  if (!DoOptions(filename)) return NULL;
  //OutputDebugStringA("Running Describe");
  char *Ret = NULL;
  RunStep(step_DESCRIBE);
  if (Settings.User[0] && Settings.Password[0])
  {
    Ret = rtsp->describeWithPassword(filename, Settings.User, Settings.Password, false, Settings.TimeOut);
  }
  else
  {
    Ret = rtsp->describeURL(filename, NULL, false, Settings.TimeOut);
  }
  if ((Ret == NULL) || (*Ret == 0))
  {
    CheckConnectionState();
    SetStepFail(step_DESCRIBE);
  }
  else
  {
    SetStepOK(step_DESCRIBE);
  }
  return Ret;
}

void LiveCapsule::UseSubSource(MediaSubsession *sub, GeneralFormatSupporter *supporter)
{
  if (supporter->AsType() == CODEC_TYPE_VIDEO)
  {
    if (Video) return;
    Video = new CapsuleTrack(this, sub, supporter);
  }
  if (supporter->AsType() == CODEC_TYPE_AUDIO)
  {
    if (Audio) return;
    Audio = new CapsuleTrack(this, sub, supporter);
  }
  if (( supporter->AsType() == CODEC_TYPE_META) && Settings.UseMetadata)
  {
    if (Meta) return;
    Meta = new CapsuleTrack(this, sub, supporter);
  }
}


bool LiveCapsule::OpenStream(const char* filename)
{
  if (!InitEnvironment()) return false;
  char* sdpDescription = DescribeStream(filename);
#if 0
  if (!sdpDescription)
  {
    HaltEnvironment();
    if (!InitEnvironment()) return false;
    sdpDescription = DescribeStream(filename);
  }
#endif
  if (!sdpDescription) 
  {
    HaltEnvironment();
    return false;
  }

  RunStep(step_OpenStream);
  ms = MediaSession::createNew(*env, sdpDescription);
  delete[] sdpDescription;

  MediaSubsession         *sub;
  MediaSubsessionIterator *iter = new MediaSubsessionIterator(*ms);
  while( ( sub = iter->next() ) != NULL )
  {
    GeneralFormatSupporter *supporter = ProbeByCodec(sub);
    if (!supporter) continue;
    if (!Settings.UseAudio)
    {
      if (supporter->AsType() == CODEC_TYPE_AUDIO) continue;
    }
    if (!sub->initiate())
    {
      Log->Drop("RTP subsession '%s/%s' failed (%s)",
      sub->mediumName(), sub->codecName(), env->getResultMsg() ); 
      continue;
    }

    if( sub->rtpSource() != NULL )
    {
      int fd = sub->rtpSource()->RTPgs()->socketNum();
      int size = 0;
      if (supporter->AsType() == CODEC_TYPE_VIDEO) size = 2000000;
      if (supporter->AsType() == CODEC_TYPE_AUDIO) size =  100000;
      /* Increase the buffer size */
      if( size > 0 )
        increaseReceiveBufferTo(*env, fd, size );
      /* Increase the RTP reorder time buffer just a bit */
      sub->rtpSource()->setPacketReorderingThresholdTime(200000);
    }

    if( sub->readSource() == NULL )
      continue;

    UseSubSource(sub, supporter);
  }
  delete iter;
  if (!Audio && !Video && !Meta)
  {
    SetStepFail(step_OpenStream);
    HaltEnvironment();
    return false;
  }
  if (Meta)
  {
    if (Video)
    {
      delete Video;
    }
    Video = Meta;
    Meta = NULL;
  }
  SetStepOK(step_OpenStream);
  State = RTSP_STATE_OPENED;
  return true;
}

// hack
// AS Fix for something unknown
extern int InPlayMode;
bool LiveCapsule::PlayStream()
{
  if (StartingPlay) return true;

  if (State  == RTSP_STATE_PLAYING) return true;
  
  StartingPlay = true;
  
  if (State == RTSP_STATE_PAUSED)
  {
    RunStep(step_PLAY);
    if (!rtsp->playMediaSession(*ms, -1))
    {
      SetStepFail(step_PLAY);
      return false;
    }
    InPlayMode = 1;
    SetStepOK(step_PLAY);
    State  = RTSP_STATE_PLAYING;
    StartingPlay = false;
    return true;
  }

  if (State != RTSP_STATE_OPENED)
  {
    StartingPlay = false;
    return false;
  }

  // from RTSP_STATE_OPENED

  RunStep(step_SETUP);
  Log->Drop("SETUP %s", Video->sub->codecName());
  bool Ret = rtsp->setupMediaSubsession(*Video->sub, False, Settings.UseTCPTunnel);
  if (Ret && Audio) 
  {
    Log->Drop("SETUP %s", Audio->sub->codecName());
    Ret = rtsp->setupMediaSubsession(*Audio->sub, False, Settings.UseTCPTunnel);
  }
  if (!Ret)
  {
    CheckConnectionState();
    SetStepFail(step_SETUP);
    StartingPlay = false;
    return false;
  }
  SetStepOK(step_SETUP);

  if (!RunThread())
  {
    StartingPlay = false;
    return false;
  }

  RunStep(step_PLAY);
  if (!rtsp->playMediaSession(*ms, 0.0 , -1, 1 ))
  {
    CheckConnectionState();
    //SetStepFail(step_PLAY);
    //StartingPlay = false;
    //return false;
  }
  SetStepOK(step_PLAY);
  InPlayMode = 1;
//  State  = RTSP_STATE_PLAYING;


  RunStep(step_WaitStream);
  for(int i = 0; i< Settings.TimeOut * 5; i++)
  {
    Sleep(200);
    DropDebug("awaiting %i %i", i, Video->GetCount());
    if (Video->GetCount() > 0) break;
  }
  StartingPlay = false;
  if (Video->GetCount() > 0)
  {
    State  = RTSP_STATE_PLAYING;
    SetStepOK(step_WaitStream);
    return true;
  }
  else
  {
    SetStepFail(step_WaitStream);
    return false;
  }
}

bool LiveCapsule::PauseStream()
{
  if (State  != RTSP_STATE_PLAYING)
  {
    return false;
  }
  State  = RTSP_STATE_PAUSED;
  
  if (!rtsp->pauseMediaSession(*ms))
  {
    //char *errmsg;
    //errmsg = (char *)env->getResultMsg();
    return false;
  }
  return true;
}

bool LiveCapsule::StopStream()
{
  InPlayMode = 0;
  if (StopFlagStop) return true;
  StopFlagStop = 1;
  StopThread();

  if (State == RTSP_STATE_IDLE)
  {
    StopFlagStop = 0;
    return true;
  }

  if (State == RTSP_STATE_PLAYING)
  {
    if (rtsp && ms)
    {
      RunStep(step_TEARDOWN);
      if (rtsp->teardownMediaSession(*ms))
      {
        SetStepOK(step_TEARDOWN);
      }
      else
      {
        CheckConnectionState();
        SetStepFail(step_TEARDOWN);
        State = RTSP_STATE_IDLE;
        StopFlagStop = 0;
        return false;
      }
    }
  }

  State = RTSP_STATE_IDLE;
  StopFlagStop = 0;
  return true;
}

bool LiveCapsule::CloseStream()
{
  bool ret = StopStream();
  HaltEnvironment();
  return ret;
}

DWORD LiveCapsule::ThreadProc()
{
 try {
  if (Video) 
  {
    Video->ReadSource();
  };
  if (Audio)
  {
    Audio->ReadSource();
  };
  env->taskScheduler().doEventLoop(&StopFlag);
  OutputDebugStringA("ThreadProc end");
  
  // verify that stream is not playing
  if (State == RTSP_STATE_PLAYING)
  {
    StopStream();
  }
 } catch (...) {
   return 2;
 };
  return 0;
}

void CapsuleTrack::ReadSource()
{
  if (sub->readSource()->isCurrentlyAwaitingData()) return;
  sub->readSource()->getNextFrame(
    Buffer.GetBuffer(), 
    Buffer.GetSize(),
    AfterGetting, 
    this, 
    OnClose,
    this);
}

void CapsuleTrack::AfterGetting(
                          void* clientData, 
                          unsigned frameSize,
                          unsigned numTruncatedBytes,
                          struct timeval presentationTime,
                          unsigned durationInMicroseconds)
{
  CapsuleTrack *Track = (CapsuleTrack*)clientData;
  FrameInfo *Info = Track->Supporter->NextChunk(
    Track->sub, Track->Buffer.GetBuffer(), frameSize, Track->Prefix);
  if (!Info) goto next; // unsupported frame type


  if (Track->Supporter->AsType() == CODEC_TYPE_META)
  {
    if (frameSize > 20)
      memcpy(Track->Parent->LastEvent, Track->Buffer.GetBuffer(), frameSize+1);
    //DropDebug("Event occurs [%s]\n", Track->Buffer.GetBuffer());
  }


  Info->frameHead.FrameType = Track->Supporter->AsFOURCC();


  if (numTruncatedBytes)
  {
    DropDebug("Truncated frame by %i\n", numTruncatedBytes);
  }

  Info->frameHead.Start = 
    ((REFERENCE_TIME)presentationTime.tv_sec) * 10000000 +
    ((REFERENCE_TIME)presentationTime.tv_usec) * 10;

 /*
  if (Track->Supporter->AsType() == CODEC_TYPE_VIDEO)
  {
    static bool first = false;
    if (!first)
    {
      first = true;
      DropDebug1("FrameSize\tTrunc\tNAL\tSec\tms\tQSize\n"); 
    }
    DropDebug1("%i\t%i\t%i\t%i\t%i\t%i\n", 
      frameSize, 
      numTruncatedBytes, 
      *Track->Buffer.GetBuffer() & 0x1f,
      presentationTime.tv_sec, 
      presentationTime.tv_usec,
      Track->FramesCount);
  }
*/
#if 1
  Track->PutBottom(Info);
#else
  Track->Parent->BalanceLoad();
  Track->Frames.PutQueueObject(Info);
  InterlockedIncrement(&Track->FramesCount);
#endif
next:
  if (Track->Parent->IsAlive())
    Track->ReadSource();
}

int LastTick;
void CapsuleTrack::PutBottom(FrameInfo* Info)
{
  LastTick = GetTickCount();
  Parent->BalanceLoad();
  Frames.PutQueueObject(Info);
  InterlockedIncrement(&FramesCount);
  InterlockedIncrement(&KeyInterval);
  if (!Info->frameHead.InterFrame)
  {
    CalcKeyTime();
  }
}

void LiveCapsule::UnlockTracks()
{
  if (Video)
  {
    OutputDebugStringA("Drop Object to video\n");
    FrameInfo *Info = FrameInfo::Init(4);
    Video->PutBottom(Info);
  }
  if (Audio)
  {
    OutputDebugStringA("Drop Object to audio\n");
    FrameInfo *Info = FrameInfo::Init(4);
    Audio->PutBottom(Info);
  }
}


void CapsuleTrack::CalcKeyTime()
{
  if (KeyAverage <= 0)
  {
    KeyAverage = KeyInterval;
  }
  else
  {
    KeyLastDistortion = KeyAverage - KeyInterval;
    KeyLastDistortion *= KeyLastDistortion;
    if (KeyAverageDistortion <= 0)
    {
      KeyAverageDistortion = KeyLastDistortion;
    }
    else
    {
      KeyAverageDistortion = (KeyAverageDistortion * 3 + KeyLastDistortion) / 4;
    }
    KeyAverage = (KeyAverage * 3 + KeyInterval) / 4;
  }
  DropDebug("LastKey(%i), KeyAverage(%g), LastD(%g), AD(%g)\n", KeyInterval, KeyAverage, KeyLastDistortion, KeyAverageDistortion);
  KeyIntervalBack = KeyInterval;
  Keys[KeyIndex] = KeyInterval; KeyIndex++; KeyIndex &= 3;
  KeyInterval = 0;
}

#if 1

bool CapsuleTrack::StableKey() const 
{ 
  return 
    (Keys[0] == Keys[1]) &&
    (Keys[0] == Keys[2]) &&
    (Keys[0] == Keys[3]);
};
bool CapsuleTrack::WasSyncPoint() const 
{ 
  int i = KeyIndex + 2; i &= 3;
  int j = KeyIndex + 3; j &= 3;
  return Keys[i] != Keys[j]; 
};

#else

bool CapsuleTrack::StableKey() const { return KeyAverageDistortion < 4; };
bool CapsuleTrack::WasSyncPoint() const { return KeyAverageDistortion * 2 < KeyLastDistortion; };

#endif


void CapsuleTrack::OnClose(void* clientData)
{

}

FrameInfo* CapsuleTrack::GetTop()
{
  InterlockedDecrement(&FramesCount);
  return Frames.GetQueueObject();
}

bool LiveCapsule::OpenSettings(const WCHAR* FileName)
{
  Settings.Load(FileName);
  char *filename = ToMulti(FileName);
  Log->AttachIni(filename);
  delete[] filename;
  return OpenStream(Settings.Address);
}

bool LiveCapsule::TrySetParameter()
{
  if (!rtsp || !ms) return false;
  return rtsp->setMediaSessionParameter(*ms, "", "");
  //return rtsp->setMediaSessionParameter(*ms, "a", "1");
}

#if 1
void LiveCapsule::BalanceLoad()
{
  if (Settings.UseKeepAlive)
  {
    if (LastParameterTick < GetTickCount() - 6000)
    {
      LastParameterTick = GetTickCount();
      DropDebug("TrySetParameter %i", LastParameterTick);
      TrySetParameter();
    }
  }
  if (Video)
  {
    DropDebug("Video count %i", Video->GetCount());
    if (Video->GetCount() > MAX_FRAMES) 
    {
      FrameInfo *fi = Video->GetTop();
      FrameInfo::Release(fi);
      // drop stream - seems there are no client
      StopFlag = 1;
    }
  }
  if (Audio)
  {
    DropDebug("Audio count %i", Audio->GetCount());
    if (Audio->GetCount() > MAX_FRAMES)
    {
      FrameInfo *fi = Audio->GetTop();
      FrameInfo::Release(fi);
    }
  }
}
#else
void LiveCapsule::BalanceLoad()
{
  bool ToStop = false;
  if (Video)
  {
    if (Video->GetCount() > MAX_FRAMES) ToStop = true;
    DropDebug("Video count %i", Video->GetCount());
  }
  if (Audio)
  {
    if (Audio->GetCount() > MAX_FRAMES) ToStop = true;
    DropDebug("Audio count %i", Audio->GetCount());
  }
  if (ToStop)
  {
    //OutputDebugStringA("GoPause");
    //PauseStream();
  }
}
#endif


bool LiveCapsule::NeedRePlay()
{
  int Count = 0;
  if (Video)
  {
    Count = Video->GetCount();
  }
  if (Audio)
  {
    if (Count > Audio->GetCount()) Count = Audio->GetCount();
  }
  return (Count < MIN_FRAMES) && (State != RTSP_STATE_PLAYING);
}

void LiveCapsule::RunStep(int Step)
{
  if (this->Step == Step) return;
  if (!((PassBits|FailBits) & (1 << this->Step)))
  {
    if (this->Step) SetStepOK(this->Step);
  }
  for (int i = this->Step+1; i < Step; i++)
    Log->SetStep(i);
  Log->SetStep(Step);
  Log->Drop("");
  //Log->Drop("RunStep %i", Step);
  this->Step = Step;
}
void LiveCapsule::SetStepOK(int Step)
{
  if (this->Step != Step)
  {
    RunStep(Step);
  }
  PassBits |= 1 << Step;
  Log->Drop("");
  //Log->Drop("Step %i OK", Step);
  Log->SetStepEnd(PassBits, FailBits);
};
void LiveCapsule::SetStepFail(int Step)
{
  if (this->Step != Step)
  {
    RunStep(Step);
  }
  FailBits |= 1 << Step;
  Log->Drop("");
  //Log->Drop("Step %i Fail", Step);
  Log->SetStepEnd(PassBits, FailBits);
};
