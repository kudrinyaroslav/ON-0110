///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include "LiveCapsule.h"
#include <time.h>
#include <map>

static const WCHAR ApplicationW[] = L"Test Options";
static const  char ApplicationA[] =  "Test Options";
static const  char ReplyA[]       =  "Test State";

void DropDebug(const char* Format, ...)
{
  return;
  va_list ap;
  char Buffer[2048];
  int size = 0;

  va_start(ap, Format);
  size += _vsnprintf(Buffer + size, sizeof(Buffer) - 1, Format, ap);
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

void MessageLog::SetStepEnd(int PassBits, int FailBits, const char *LastError)
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

  DropDebug("********** STEP ENDED: %i", (FailBits & (1 << Step)) ? -Step : Step);
  sprintf(val, "%i", (FailBits & (1 << Step)) ? -Step : Step);
  WritePrivateProfileStringA(ReplyA,
    "StepEnded",
    val,
    IniName);

  if (LastError)
  {
    WritePrivateProfileStringA(ReplyA,
      "LastError",
      LastError,
      IniName);
  }
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

#define RESTORE_CLRF(var)                \
  while (char *clrf = strstr(var, "$$")) \
  {                                      \
    *clrf++ = '\r';                      \
    *clrf = '\n';                        \
  }


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
  AssumeWidth = VideoWidth = GetPrivateProfileIntW(ApplicationW,
    L"VideoWidth",
    0,
    FileName);
  AssumeHeight = VideoHeight = GetPrivateProfileIntW(ApplicationW,
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

  UseKeepAliveOptions = !!GetPrivateProfileIntW(ApplicationW,
    L"UseKeepAliveOptions",
    0,
    FileName);

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

  Multicast = !!GetPrivateProfileIntW(ApplicationW,
    L"Multicast",
    0,
    FileName);

  UseVideo = !!GetPrivateProfileIntW(ApplicationW,
    L"UseVideo",
    1,
    FileName);

  RTSP = !!GetPrivateProfileIntW(ApplicationW,
    L"RTSP",
    1,
    FileName);

  VideoFPS = GetPrivateProfileIntW(ApplicationW,
    L"VideoFPS",
    VideoFPS,
    FileName);

  GetPrivateProfileStringA(ApplicationA,
    "VideoCodecName",
    "",
    VideoCodecName,
    elementsof(VideoCodecName),
    filename);

  GetPrivateProfileStringA(ApplicationA,
    "AudioCodecName",
    "",
    AudioCodecName,
    elementsof(AudioCodecName),
    filename);

  GetPrivateProfileStringA(ApplicationA,
    "MulticastAddress",
    "",
    MulticastAddress,
    elementsof(MulticastAddress),
    filename);

  GetPrivateProfileStringA(ApplicationA,
    "MulticastAddressAudio",
    "",
    MulticastAddressAudio,
    elementsof(MulticastAddressAudio),
    filename);

  MulticastRtpPortVideo = GetPrivateProfileIntW(ApplicationW,
    L"MulticastRtpPortVideo",
    1234,
    FileName);

  MulticastRtpPortAudio = GetPrivateProfileIntW(ApplicationW,
    L"MulticastRtpPortAudio",
    1236,
    FileName);

  MulticastTTL = GetPrivateProfileIntW(ApplicationW,
    L"MulticastTTL",
    1,
    FileName);

  Base64LineBreaks = !!GetPrivateProfileIntW(ApplicationW,
    L"Base64LineBreaks",
    1,
    FileName);

  NICIndex = GetPrivateProfileIntW(ApplicationW,
    L"NICIndex",
    0,
    FileName);

  GetPrivateProfileStringA(ApplicationA,
    "CustomSetupFields",
    "",
    CustomSetupFields,
    elementsof(CustomSetupFields),
    filename);

  GetPrivateProfileStringA(ApplicationA,
    "CustomPlayFields",
    "",
    CustomPlayFields,
    elementsof(CustomPlayFields),
    filename);

  DoSetupOnReplay = !!GetPrivateProfileIntW(ApplicationW,
    L"DoSetupOnReplay",
    1,
    FileName);

  ReplayMode = !!GetPrivateProfileIntW(ApplicationW,
    L"ReplayMode",
    0,
    FileName);

  GetPrivateProfileStringA(ApplicationA,
    "CustomPauseFields",
    "",
    CustomPauseFields,
    elementsof(CustomPauseFields),
    filename);

  ReplayMaxDuration = GetPrivateProfileIntW(ApplicationW,
    L"ReplayMaxDuration",
    0,
    FileName);

  ReplayPauseWait = GetPrivateProfileIntW(ApplicationW,
    L"ReplayPauseWait",
    0,
    FileName);

  ReplayReverse = !!GetPrivateProfileIntW(ApplicationW,
    L"ReplayReverse",
    0,
    FileName);

  GetPrivateProfileStringA(ApplicationA,
    "MulticastMultipleSetup",
    "",
    MulticastMultipleSetup,
    elementsof(MulticastMultipleSetup),
    filename);

  CheckActualResolution = !!GetPrivateProfileIntW(ApplicationW,
    L"CheckActualResolution",
    0,
    FileName);

  CheckJPEGExtension = !!GetPrivateProfileIntW(ApplicationW,
    L"CheckJPEGExtension",
    0,
    FileName);

  RESTORE_CLRF(CustomSetupFields);
  RESTORE_CLRF(CustomPlayFields);
  RESTORE_CLRF(CustomPauseFields);

  delete[] filename;

  if ((TunnelPortNumber != 0) && UseTCPTunnel)
  {
    //UseKeepAlive = false;
  }

  memcpy(Address, "rtsp", 4);
}

extern unsigned int readSocketTimeout;

bool LiveCapsule::InitEnvironment()
{
  RunStep(step_InitEnvironment);
  StopFlag = 1;
  scheduler = Settings.ReplayMode ? TaskScheduler2::createNew() : BasicTaskScheduler::createNew();
  if (scheduler ==	NULL)
  {
    Log->Drop("ERROR in BasicTaskScheduler");
    goto fail;
  }
#ifdef DEBUG
  env = BasicUsageEnvironment::createNew(*scheduler);
#else
  env = BasicUsageEnvironment::createNew(*scheduler);
#endif
  if (env == NULL)
  {
    Log->Drop("BasicUsageEnvironment fail");
    goto fail;
  }
  if (Settings.RTSP)
  {
    rtsp = RTSPClient::createNew(*env, 0, AppName, Settings.TunnelPortNumber);
    if (rtsp == NULL)
    {		 
      Log->Drop("create rtsp client fail");
      goto fail;
    }
    rtsp->setBase64LineBreaks(Settings.Base64LineBreaks);
    rtsp->setSetupDestination("");
  }

  Groupsock::NIC = Settings.NICIndex;

  readSocketTimeout = Settings.ReplayMode ? 2 : 0;
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
  if (rtsp) 
  {
    RTSPClient::close(rtsp);			
    rtsp = NULL;
  }

  if (Video) delete Video;
  Video = NULL;
  if (Audio) delete Audio;
  Audio = NULL;

  if (env)
  {
    env->reclaim();
    env = NULL;
  }
  if (scheduler)
  {
    delete scheduler;
    scheduler = NULL;
  }
  //SetStepOK(step_HaltEnvironment);
}

bool LiveCapsule::RunThread()
{
  StopFlag = 0;
  if (!ThreadExists())
    
    ThreadStarted = false;
    ThreadTerminated = false;

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

  //(IsFakeVideo() ? Audio : Video)->sub->rtpSource()->stopGettingFrames();
  (IsFakeVideo() ? Audio : Video)->StopGettingFrames();
  StopFlag = 1;

  // Disable warning: Conversion from LONG to PVOID of greater size
#pragma warning(push)
#pragma warning(disable: 4312)
  HANDLE hThread = (HANDLE)InterlockedExchangePointer(&m_hThread, 0);
#pragma warning(pop)
 try {
  if (hThread) {
    if (WaitForSingleObject(hThread, 5000) == WAIT_TIMEOUT)
    {
      Log->Drop("Forcing thread to close");
      TerminateThread(hThread, 1);
    }
    OutputDebugStringA("CloseHandle(hThread)");
    CloseHandle(hThread);
  }
 } catch (...)
 {
   OutputDebugStringA("Exception in terminating hThread");
 };

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
  static struct RTSPErrorCodesMap {
    std::map<int, char*> m;
    RTSPErrorCodesMap()
    {
      m[400] = "Bad Request";
      m[401] = "Unauthorized";
      m[402] = "Payment Required";
      m[403] = "Forbidden";
      m[404] = "Not Found";
      m[405] = "Method Not Allowed";
      m[406] = "Not Acceptable";
      m[407] = "Proxy Authentication Required";
      m[408] = "Request Time-out";
      m[410] = "Gone";
      m[411] = "Length Required";
      m[412] = "Precondition Failed";
      m[413] = "Request Entity Too Large";
      m[414] = "Request-URI Too Large";
      m[415] = "Unsupported Media Type";
      m[451] = "Parameter Not Understood";
      m[452] = "Conference Not Found";
      m[453] = "Not Enough Bandwidth";
      m[454] = "Session Not Found";
      m[455] = "Method Not Valid in This State";
      m[456] = "Header Field Not Valid for Resource";
      m[457] = "Invalid Range";
      m[458] = "Parameter Is Read-Only";
      m[459] = "Aggregate operation not allowed";
      m[460] = "Only aggregate operation allowed";
      m[461] = "Unsupported transport";
      m[462] = "Destination unreachable";
      m[500] = "Internal Server Error";
      m[501] = "Not Implemented";
      m[502] = "Bad Gateway";
      m[503] = "Service Unavailable";
      m[504] = "Gateway Time-out";
      m[505] = "RTSP Version not supported";
      m[551] = "Option not supported";
    }
    const char* get(int code)
    {
      std::map<int, char*>::const_iterator it = m.find(lastResponseCode);
      return it == m.end() ? "" : it->second;
    }
  } RTSPErrorCodes;

  if ((lastResponseCode != 0) && ((lastResponseCode < 100) || (lastResponseCode >= 600)) && (lastResponseCode < 1000))
  {
    Log->Drop("Unknown HTTP / RTSP Status-Code %i", lastResponseCode);
  }
  else if ((lastResponseCode >= 400) && (lastResponseCode < 600))
  {
    Log->Drop("Connection error, HTTP / RTSP code %i %s", lastResponseCode, RTSPErrorCodes.get(lastResponseCode));
  }
  else
  {
    Log->Drop("Incorrect HTTP / RTSP code %i. Probably there was no response.", lastResponseCode);
  }
}

char* LiveCapsule::DoOptions(const char* filename)
{
  if (Settings.RTSP && Settings.UseOptions)
  {
    RunStep(step_OPTIONS);
    rtsp->setSocketReadAllMode(true);
    char* Options = rtsp->sendOptionsCmd(filename, Settings.User, Settings.Password, NULL, Settings.TimeOut);
    rtsp->setSocketReadAllMode(false);

    LogRtspRequestAndResponse("OPTIONS", NULL);

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

  LogRtspRequestAndResponse("DESCRIBE", Ret);

  bool ipv6 = false;
  char *bracket = strchr((char*)filename, '[');
  if (bracket)
	  ipv6 = true;

  if ((Ret == NULL) || (*Ret == 0))
  {
    CheckConnectionState();
    SetStepFail(step_DESCRIBE);
  }
  else
  {
	    char username[50] = {'\0'};
		char sessid[50] = {'\0'};
		char sessversion[50] = {'\0'};
		char nettype[5] = {'\0'};
		char addrtype[5] = {'\0'};
		char unicastaddress[200] = {'\0'};
		int num = sscanf(Ret, "%*s\r\no=%s %s %s %s %s %s\r\n%*s", username, sessid, sessversion, nettype, addrtype, unicastaddress);

		int internet = strcmp(nettype, "IN");
		int ipv4 = strcmp(addrtype, "IP4");

		if (ipv6 &&  internet == 0 && ipv4 == 0)
		{
			Log->Drop("IPv4 address (%s) is detected in origin section of SDP (o=%s %s %s %s %s %s) as <unicast-address> instead of IPv6", unicastaddress, 
				username, sessid, sessversion, nettype, addrtype, unicastaddress);
			SetStepFail(step_DESCRIBE);
		}

    SetStepOK(step_DESCRIBE);
  }

  return Ret;
}

bool LiveCapsule::UseSubSource(IMediaSubsession *sub, GeneralFormatSupporter *supporter)
{
  if (supporter->AsType() == CODEC_TYPE_VIDEO)
  {
    if (Video) return false;
    Video = new CapsuleTrack(this, sub, supporter);
  }
  else if (supporter->AsType() == CODEC_TYPE_AUDIO)
  {
    if (Audio) return false;
    Audio = new CapsuleTrack(this, sub, supporter);
  }
  else if (( supporter->AsType() == CODEC_TYPE_META) && Settings.UseMetadata)
  {
    if (Meta) return false;
    Meta = new CapsuleTrack(this, sub, supporter);
  }
  else
  {
     return false;
  }
  return true;
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

  MediaSubsession         *mediaSub;
  MediaSubsessionIterator *iter = new MediaSubsessionIterator(*ms);
  IMediaSubsession *sub = NULL;
  while((mediaSub = iter->next()) != NULL)
  {
    // delete unused instance
    if (sub)
      delete sub;

    sub = new CapsuleMediaSubsession(mediaSub, Log, Settings.ReplayMode);
    
    GeneralFormatSupporter *supporter = ProbeByCodec(sub);
    if (!supporter) continue;

    if ((!Settings.UseVideo && (supporter->AsType() == CODEC_TYPE_VIDEO))
        || (!Settings.UseAudio && (supporter->AsType() == CODEC_TYPE_AUDIO))
        || (!Settings.UseMetadata && (supporter->AsType() == CODEC_TYPE_META))) {
      continue;
    }

    if (Settings.ReplayMode) {
      const char *sdpLines = mediaSub->savedSDPLines();
      char trackRef[128] = {'\0'};
      const char *trackRefStart = NULL;
      if ((trackRefStart = strstr(sdpLines, "a=x-onvif-track:")) 
          && (sscanf(trackRefStart, "a=x-onvif-track:%[^\r\n^/ ]", trackRef) == 1)) {
        Log->Drop("Found TrackReference: %s", trackRef);
      } else {
        Log->Drop("No TrackReference in media subsession '%s/%s'", mediaSub->mediumName(), mediaSub->codecName());
        SetStepFail(step_OpenStream);
        HaltEnvironment();
        return false;
      }
    }

    if (!sub->initiate())
    {
      Log->Drop("RTP subsession '%s/%s' failed (%s)",
                sub->mediumName(), sub->codecName(), env->getResultMsg() ); 
      continue;
    }

    if (sub->rtpSource() != NULL)
    {
      int fd = sub->rtpSource()->RTPgs()->socketNum();
      int size = 0;
      if (supporter->AsType() == CODEC_TYPE_VIDEO) size = 10000000;
      if (supporter->AsType() == CODEC_TYPE_AUDIO) size =  100000;
      /* Increase the buffer size */
      if( size > 0 )
        increaseReceiveBufferTo(*env, fd, size );
      /* Increase the RTP reorder time buffer just a bit */
      sub->rtpSource()->setPacketReorderingThresholdTime(200000);
    }

    if (sub->readSource() == NULL)
      continue;

    if (UseSubSource(sub, supporter))
      sub = NULL;
  }
  delete iter;
  if ((Settings.UseVideo && !Video) || (Settings.UseAudio && !Audio) || (Settings.UseMetadata && !Meta))
  {
    if (Settings.UseVideo && !Video) 
      Log->Drop("Video track not found");
    if (Settings.UseAudio && !Audio) 
      Log->Drop("Audio track not found");
    if (Settings.UseMetadata && !Meta) 
      Log->Drop("Metadata track not found");

    SetStepFail(step_OpenStream);
    HaltEnvironment();
    return false;
  }
  if (Meta && !Video)
  {
    Video = Meta;
    Meta = NULL;
    Settings.UseVideo = true;
  }
  // use fake video with audio-only session
  if (Audio && !Video) 
  {
    Video = new CapsuleTrack(this, NULL, ProbeByCodec(NULL));
  }
  SetStepOK(step_OpenStream);
  State = RTSP_STATE_OPENED;
  return true;
}

// Get the first set of custom command fields
#define GET_NEXT_FIELD_START(var, fields) \
  char *var = NULL;                       \
  char dlmr;                              \
  char *next = NULL;                      \
  if (strlen(fields) > 0)                 \
  {                                       \
    dlmr = fields[0];                     \
    var = &fields[1];                     \
    if (next = strchr(var, dlmr))         \
      *next++ = '\0';                     \
  }

// Shift the next set of custom command fields to beginning
#define GET_NEXT_FIELD_END(var, fields)  \
  if (next)                              \
    memcpy(var, next, strlen(next) + 1); \
  else                                   \
    fields[0] = '\0';

// hack
// AS Fix for something unknown
extern int InPlayMode;

u_int8_t NewFramesArrived = 0;

bool LiveCapsule::PlayStream()
{
  if (StartingPlay) return true;

  StartingPlay = true;
  
  time_t timeoutEnd = time(NULL) + (int)(0.8 * Settings.TimeOut);
  
  if (Settings.RTSP && ((State == RTSP_STATE_PAUSED) || (State == RTSP_STATE_PLAYING)))
  {
    (IsFakeVideo() ? Audio : Video)->ReadSource();

    RunStep(step_PLAY);

    if (Settings.ReplayMode)
      ((CapsuleMediaSubsession*)(IsFakeVideo() ? Audio : Video)->sub)->mPlayCommandCount++;

    // get the first set of custom PLAY fields
    GET_NEXT_FIELD_START(CustomPlayField, Settings.CustomPlayFields);

    rtsp->setCustomFields(CustomPlayField);
    rtsp->setSocketReadAllMode(true);
    Boolean Ret = rtsp->playMediaSession(*ms, -1);
    rtsp->setSocketReadAllMode(false);

    LogRtspRequestAndResponse("PLAY", NULL);

    if (Step != step_PLAY) return (StartingPlay = false);

    // shift the next set of custom PLAY fields to beginning
    GET_NEXT_FIELD_END(CustomPlayField, Settings.CustomPlayFields);

    CheckResponse(Ret);

    if (!Ret)
    {
      CheckConnectionState();
      SetStepFail(step_PLAY);
      return (StartingPlay = false);
    }
  } 
  else if (State != RTSP_STATE_OPENED && State != RTSP_STATE_STOPPED)
  {
    StartingPlay = false;
    return false;
  }
  else
  { // from RTSP_STATE_OPENED

    if (Settings.RTSP && !RtspSetup())
    {
      Sleep(100);
      return (StartingPlay = false);
    }

    if (!RunThread())
    {
      StartingPlay = false;
      return false;
    }

    // RTSP PLAY
    if (Settings.RTSP)
    {
      RunStep(step_PLAY);

      if (!Settings.CheckActualResolution && Settings.UseVideo && Video)
      {
        // hack: prevent parsing frames for resolution info
        Video->sub->rtpSource()->fFrameWidth = 1;
        Video->sub->rtpSource()->fFrameHeight = 1;
      }

      if (Settings.ReplayMode)
        ((CapsuleMediaSubsession*)(IsFakeVideo() ? Audio : Video)->sub)->mPlayCommandCount++;

      // get the first set of custom PLAY fields
      GET_NEXT_FIELD_START(CustomPlayField, Settings.CustomPlayFields);

      rtsp->setCustomFields(CustomPlayField);
      rtsp->setSocketReadAllMode(true);
      Boolean Ret = rtsp->playMediaSession(*ms, Settings.ReplayMode ? -1.0 : 0.0);
      rtsp->setSocketReadAllMode(false);

      LogRtspRequestAndResponse("PLAY", NULL);

      if (Step != step_PLAY) return (StartingPlay = false);

      // shift the next set of custom PLAY fields to beginning
      GET_NEXT_FIELD_END(CustomPlayField, Settings.CustomPlayFields);

      CheckResponse(Ret);

      if (!Ret)
      {
        CheckConnectionState();
        if (Settings.ReplayMode)
        {
          SetStepFail(step_PLAY);
          return (StartingPlay = false);
        }
        //SetStepFail(step_PLAY);
        //StartingPlay = false;
        //return false;
      }
    }
  }

  SetStepOK(step_PLAY);
  InPlayMode = 1;
  //State  = RTSP_STATE_PLAYING;

  RunStep(step_WaitStream);

  NewFramesArrived = 0;

  
  if (Settings.CheckActualResolution)
  {
    if (strcmp(Video->sub->codecName(), "MP4V-ES") == 0)
    {
      if (const char *fmtp_config = Video->sub->mediaSub()->fmtp_config())
      {
        unsigned char buffer[128];
        unsigned size = 0;
        unsigned i = 0;
        if (const char *vol_header = strstr(fmtp_config, "00000120"))
        {
          size = strlen(vol_header) / 2;
          for (i = 0; i < size; ++i)
            if (1 != sscanf(&vol_header[2 * i], "%2X", &buffer[i])) break;
        }
        if (i == size && size)
        {
          ParseMPEG4VOL(buffer, size, Video->sub->mediaSub()->readSource());
        }
      }
    }
    else if (strcmp(Video->sub->codecName(), "H264") == 0)
    {
      unsigned int num = 0;
      SPropRecord *SPropRecords = parseSPropParameterSets(Video->sub->mediaSub()->fmtp_spropparametersets(), num);
      if (SPropRecords && num > 0)
      {
        ParseH264SPS(SPropRecords[0].sPropBytes, SPropRecords[0].sPropLength, Video->sub->mediaSub()->readSource());
      }
    }
  }

  if (!Settings.RTSP && Settings.UseVideo && !Settings.UseAudio)
  {
    CapsuleRtpSubsession *subVideo = (CapsuleRtpSubsession*)Video->mediasub();
    if (!subVideo->GetWarningFlag())
      subVideo->SetWarningFlag(true);
  }

  if (!Settings.RTSP && Settings.UseAudio && !Settings.UseVideo)
  {
    CapsuleRtpSubsession *subAudio = (CapsuleRtpSubsession*)Audio->mediasub();
    if (!subAudio->GetWarningFlag())
      subAudio->SetWarningFlag(true);
  }

  // start checking packets for replay
  CapsuleMediaSubsession *sub = NULL;
  if (Settings.ReplayMode)
  {
    sub = (CapsuleMediaSubsession*)(Settings.UseAudio ? Audio : Video)->mediasub();
    sub->StartCheckingPackets();
    timeoutEnd = Settings.ReplayMaxDuration ? min(timeoutEnd, time(NULL) + Settings.ReplayMaxDuration) : timeoutEnd;
  }

  int msAwaiting = 0;
  bool WaitSuccess = false;
  for(int i = 0; i< Settings.TimeOut * 5; i++)
  {
    Sleep(200);
    // check frames from both streams!
    if (Settings.UseVideo && Settings.UseAudio)
    {
      DropDebug("awaiting %i V:%i A:%i", i, Video->GetCount(), Audio->GetCount());
      WaitSuccess = (Video->StreamReceived() && Audio->StreamReceived());
    }
    else if (Settings.UseVideo)
    {
      DropDebug("awaiting %i %i", i, Video->GetCount());
      WaitSuccess = (Settings.ReplayMode ? NewFramesArrived > 0 : Video->StreamReceived());
    }
    else if (Settings.UseAudio)
    {
      DropDebug("awaiting %i A:%i", i, Audio->GetCount());
      WaitSuccess = Audio->StreamReceived();
    }

    if (Settings.CheckActualResolution) 
    {
      WaitSuccess = WaitSuccess && Video->sub->mediaSub()->readSource()->IsFrameDimensionsParsed();
    }
    if (WaitSuccess) break;
    msAwaiting += 200;
  }

  if (Settings.UseVideo && Settings.UseAudio && !WaitSuccess)
  {
    if (!Video->StreamReceived())
    {
      Log->Drop("No video stream from the NVT");
    }

    if (!Audio->StreamReceived())
    {
      Log->Drop("No audio stream from the NVT");
    }
  }

  if (Step != step_WaitStream) return (StartingPlay = false);

  StartingPlay = false;
  if (WaitSuccess && !UnlockingTracks)
  {
    State = RTSP_STATE_PLAYING;

    if (Settings.CheckActualResolution)
    {
      unsigned FrameWidth = Video->sub->rtpSource()->fFrameWidth;
      unsigned FrameHeight = Video->sub->rtpSource()->fFrameHeight;

      Log->Drop("Verifying resolution...");
      Log->Drop("Requested resolution: %u x %u", Settings.VideoWidth, Settings.VideoHeight);
      Log->Drop("Detected resolution: %u x %u", FrameWidth, FrameHeight);

      // Ticket 278: JPEG resolution over RTP should be multiple of 8
      if (strcmp(Settings.VideoCodecName, "JPEG") == 0)
      {
        if (!(FrameWidth >= Settings.VideoWidth-8 && FrameWidth <= Settings.VideoWidth+8) || 
          !(FrameHeight >= Settings.VideoHeight-8 && FrameHeight <= Settings.VideoHeight+8)
          )
        {
          SetStepFail(step_WaitStream, "Verification failed!");
          return false;
        }
        else
          Log->Drop("OK");
      }
      // for H264 and MPEG4 exact matching is required
      else
      {
        if (Settings.VideoWidth != FrameWidth || Settings.VideoHeight != FrameHeight)
        {
          SetStepFail(step_WaitStream, "Verification failed!");
          return false;
        }
        else
          Log->Drop("OK");
      }
    }

    if (Settings.CheckJPEGExtension)
    {
      Log->Drop("Verifying JPEG header extension...");
      CapsuleMediaSubsession *sub = (CapsuleMediaSubsession*)Video->mediasub();
      sub->mPacketsReceived = 0;
      sub->mFailedPacketsReceived = 0;
      sub->mCheckingPackets = 1;
      int i = 0;
      timeoutEnd = min(timeoutEnd, time(NULL) + 2); // 2 sec
      while ((time(NULL) < timeoutEnd) && sub->mCheckingPackets) 
      {
        Sleep(100);
        sub->DropMsg = true;
        ++i;
      }
      sub->mCheckingPackets = 0;

      if (Settings.VideoHeight > 2040 || Settings.VideoWidth > 2040)
      {
        if (sub->mPacketsReceived == 0) 
        {
          Log->Drop("No JPEG header extensions found in packets for resolution %u x %u", Settings.VideoWidth, Settings.VideoHeight);
          SetStepFail(step_WaitStream, "Verification failed!");
          return false;
        }
        else if (sub->mFailedPacketsReceived > 10)
        {
          Log->Drop("Packets without JPEG header extensions found for resolution %u x %u", Settings.VideoWidth, Settings.VideoHeight);
          SetStepFail(step_WaitStream, "Verification failed!");
          return false;
        }
      }
      else
      {
        Log->Drop("Low resolution");
      }
      Log->Drop("OK");
    }

    if (Settings.ReplayMode)
    {
      timeoutEnd += msAwaiting / 1000;
      do 
      {
        // write first packet to log
        /*if (sub->mFirstPacket && !sub->mFirstPacket->sent) {
  OutputDebugStringA("ToString 123\n");
          sub->mFirstPacket->ToString(sub->Log(), false);
        } else {
  OutputDebugStringA("ToString 123 missed\n");
        }*/
        sub->PutPacketToLog();
        Sleep(100);
      }
      while (time(NULL) < timeoutEnd && sub->mCheckingPackets);
      sub->mCheckingPackets = 0;

      //(IsFakeVideo() ? Audio : Video)->sub->rtpSource()->stopGettingFrames();
	  (IsFakeVideo() ? Audio : Video)->StopGettingFrames();

      if (sub->mPacketsReceived == 0)
      //if ((sub->mPacketsReceived == 0) && !(sub->mFirstPacket && sub->mFirstPacket->sent))
      {
        SetStepFail(step_WaitStream, "No packets with correct RTP header extension were received!");
        return false;
      }
      Log->Drop("Packets received: %d", sub->mPacketsReceived + 1);
    }
    SetStepOK(step_WaitStream);
    return true;
  }
  else
  {
    SetStepFail(step_WaitStream);
    return false;
  }
}

bool LiveCapsule::RtspSetup()
{
  if (!(Settings.RTSP && !(State == RTSP_STATE_STOPPED && !Settings.DoSetupOnReplay)))
    return false;

  RunStep(step_SETUP);

  // check multicast address in SDP 
  if (Settings.Multicast)
  {
    bool addressValid = false;
    if (Settings.UseVideo && Video)
    {
      addressValid = Video->sub->mediaSub()->IsValidMulticastEndpointAddress();
    }
    else if (Settings.UseAudio && Audio)
    {  
      addressValid = Audio->sub->mediaSub()->IsValidMulticastEndpointAddress();
    }
    if (!addressValid)
    {
      Log->Drop("Invalid multicast address in SDP content returned in reply to a DESCRIBE request");
      SetStepFail(step_SETUP);
      StartingPlay = false;
      return false;
    }
  }

  Boolean Ret = False;
  if (strlen(Settings.MulticastMultipleSetup) == 0)
  {

    Ret = !Settings.UseVideo;
    if (Settings.UseVideo)
    {
      Log->Drop("SETUP %s", Video->sub->codecName());
      rtsp->setCustomFields(Settings.CustomSetupFields);
      rtsp->setSocketReadAllMode(true);
      Ret = rtsp->setupMediaSubsession(*Video->sub->mediaSub(), False, Settings.UseTCPTunnel);
      rtsp->setSocketReadAllMode(false);
    }
    if (Ret && Audio) 
    {
      Log->Drop("SETUP %s", Audio->sub->codecName());
      rtsp->setCustomFields(Settings.CustomSetupFields);
      rtsp->setSocketReadAllMode(true);
      Ret = rtsp->setupMediaSubsession(*Audio->sub->mediaSub(), False, Settings.UseTCPTunnel);
      rtsp->setSocketReadAllMode(false);
    }
    if (Ret && Settings.UseMetadata && Meta) 
    {
      Log->Drop("SETUP %s", Meta->sub->codecName());
      rtsp->setCustomFields(Settings.CustomSetupFields);
      rtsp->setSocketReadAllMode(true);
      Ret = rtsp->setupMediaSubsession(*Meta->sub->mediaSub(), False, Settings.UseTCPTunnel);
      rtsp->setSocketReadAllMode(false);
    }

    LogRtspRequestAndResponse("SETUP", NULL);
  }
  else
  {
    char *str = Settings.MulticastMultipleSetup;
    char *next = NULL;
    while (strlen(str) > 0) 
    {
      next = strchr(str, ';');
      *next++ = '\0';
      char *addr = str;
      str = next;

      next = strchr(str, ';');
      *next++ = '\0';
      unsigned short port = (unsigned short)atoi(str);
      str = next;

      CapsuleTrack *Media = (Settings.UseVideo ? Video : Audio);
      MediaSubsession *mediaSub = Media->sub->mediaSub();

      // reinitiate subsession if port is changed
      if (mediaSub->clientPortNum() != port)
      {
        mediaSub->deInitiate();
        mediaSub->setClientPortNum(port);
        Media->sub->initiate();
      }

      char dest[64];
      sprintf(dest, ";destination=%s", addr);
      rtsp->setSetupDestination(dest);

      Log->Drop("SETUP %s (Address: %s; Port: %u)", Media->sub->codecName(), addr, port);
      rtsp->setSocketReadAllMode(true);
      Ret = rtsp->setupMediaSubsession(*Media->sub->mediaSub(), False, Settings.UseTCPTunnel);
      rtsp->setSocketReadAllMode(false);

      rtsp->setSetupDestination("");

      LogRtspRequestAndResponse("SETUP", NULL);

      if (Ret)
      {
        // verify requested settings with those actualy set
        if (mediaSub->serverPortNum != port)
        {
          Log->Drop("Returned port: %u", mediaSub->serverPortNum);
          Ret = False;
        }
        if (strcmp(mediaSub->connectionEndpointName(), addr) != 0)
        {
          Log->Drop("Returned address: %s", mediaSub->connectionEndpointName());
          Ret = False;
        }
      }
      if (!Ret)
      {
        SetStepFail(step_SETUP, "Verification failed!");
        StartingPlay = false;

        return false;
      }
      else
      {
        Log->Drop("OK");
      }
    }    
  }

  if (!Ret)
  {
    CheckConnectionState();
    SetStepFail(step_SETUP);
    StartingPlay = false;
    return false;
  }

  SetStepOK(step_SETUP);

  SetupDone = true;
  return true;
}

void LiveCapsule::CheckResponse(Boolean &Ret)
{
  const char *Response = rtsp->ResponseBuffer();
  DropDebug(Response);
  if (!Ret) {
    unsigned Code = 0;
    sscanf(Response, "%*s %u OK", &Code);
    Ret = (Code == 200);
  }
}


bool LiveCapsule::PauseStream()
{
  if (State != RTSP_STATE_PLAYING)
  {
    return false;
  }
  
  RunStep(step_PAUSE);
  CapsuleMediaSubsession *sub = (CapsuleMediaSubsession*)(IsFakeVideo() ? Audio : Video)->mediasub();
  //CapsuleMediaSubsession *sub = (CapsuleMediaSubsession*)Video->mediasub();
  if (!sub)
  {
    SetStepFail(step_PAUSE);
    return false;
  }
  sub->mCheckingPackets = 1;
  Sleep(100);

  // get the first set of custom PAUSE fields
  GET_NEXT_FIELD_START(CustomPauseField, Settings.CustomPauseFields);

  rtsp->setCustomFields(CustomPauseField);
  rtsp->setSocketReadAllMode(true);
  Boolean Ret = rtsp->pauseMediaSession(*ms);
  rtsp->setSocketReadAllMode(false);

  LogRtspRequestAndResponse("PAUSE", NULL);

  // shift the next set of custom PAUSE fields to beginning
  GET_NEXT_FIELD_END(CustomPauseField, Settings.CustomPauseFields);

  // check if we are still in this step (it can be skipped with timeout by caller)
  if (Step != step_PAUSE) return Ret;

  CheckResponse(Ret);
       
  if (!Ret)
  {
    CheckConnectionState();
    SetStepFail(step_PAUSE);
    return false;
  } 

  for (int i = 0; i < Settings.ReplayPauseWait * 10; ++i)
  {
    Sleep(100);
  }
  sub->mCheckingPackets = 0;
  //OutputDebugStringA("ToString 456\n");
  //sub->packets[0].ToString(Log, false);

  const char *Response = rtsp->ResponseBuffer();
  //CSeq: 4
  unsigned CSeq = 0;
  if (const char *strCSeq = strstr(Response, "CSeq: ")) {
    sscanf(strCSeq + 6, "%u", &CSeq);
  }
  //Date: Tue, 15 Nov 1994 08:12:31 GMT
  char Date[40] = {'\0'};
  if (const char *strDate = strstr(Response, "Date: ")) {
    sscanf(strDate + 6, "%[^\r\n]", Date);
  }
  Log->Drop("PAUSE CSeq=%u NTP=%u NTPSec=%u Date=%s", CSeq, sub->lastPacket.dateTime, sub->lastPacket.secFrac, Date);

  SetStepOK(step_PAUSE);
  State = RTSP_STATE_PAUSED;

  return true;
}

bool LiveCapsule::StopStream()
{
  InPlayMode = 0;

  if (StopFlagStop)
  {
    DropDebug("LiveCapsule::StopStream(): StopFlagStop=%d", StopFlagStop);
    return true;
  }
  StopFlagStop = 1;

  //if (Settings.RTSP && SetupDone)
  if (IsRunning())
  {
    StopThread();
  }

  if (State == RTSP_STATE_IDLE || State == RTSP_STATE_STOPPED)
  {
    StopFlagStop = 0;
    return true;
  }

  if ((State == RTSP_STATE_PLAYING) || (State == RTSP_STATE_OPENED) || (State == RTSP_STATE_PAUSED))
  {
    // RTSP TEARDOWN
    if (Settings.RTSP && rtsp && ms)
    {
      RunStep(step_TEARDOWN);
      
      rtsp->setSocketReadAllMode(true);
      Boolean Ret = rtsp->teardownMediaSession(*ms);
      rtsp->setSocketReadAllMode(false);

      LogRtspRequestAndResponse("TEARDOWN", NULL);

      if (Ret)
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

  State = RTSP_STATE_STOPPED;
  StopFlagStop = 0;

  return true;
}

LiveCapsule::~LiveCapsule()
{
  DropDebug("~LiveCapsule() StopFlag=%d", StopFlag);
  CloseStream();
  //HaltEnvironment();
}

bool LiveCapsule::CloseStream()
{
  bool ret = StopStream();
  HaltEnvironment();
  return ret;
}

void LiveCapsule::PutFakeVideoFrame(LiveCapsule *pCapsule, struct timeval presentationTime)
{
  char *msg = "Playing Audio...";

  FrameInfo *Info = pCapsule->Video->Supporter->NextChunk(
    NULL, (unsigned char*)msg, strlen(msg), pCapsule->Video->Prefix);

  Info->frameHead.FrameType = pCapsule->Video->Supporter->AsFOURCC();

  Info->frameHead.Start = 
    ((REFERENCE_TIME)presentationTime.tv_sec) * 10000000 +
    ((REFERENCE_TIME)presentationTime.tv_usec) * 10;

  pCapsule->Video->PutBottom(Info);
}

DWORD LiveCapsule::ThreadProc()
{
 try {
  if (Settings.UseVideo && Video) 
  {
      Video->ReadSource();
  };
  if (Settings.UseMetadata && Meta) 
  {
    Meta->ReadSource();
  }
  if (Audio)
  {
    if (!Settings.UseVideo)
      // callback for putting empty video frame for each audio frame
      Audio->afterGetting = PutFakeVideoFrame;
    Audio->ReadSource();
  };
  OutputDebugStringA("ThreadProc start");
  ThreadStarted = true;
  env->taskScheduler().doEventLoop(&StopFlag);
  ThreadTerminated = true;
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

void LiveCapsule::LogRtspRequestAndResponse(char* rtspCommand, char* Ret)
{

  strset(RtspRequestBuffer, '\0');
  strset(RtspResponseBuffer, '\0');

  // Prepare RTSP request for sending to Test Tool
  //unsigned int reqSize = rtsp->RequestBufferSize();
  const char* pReqBuffer = rtsp->GetRtspRequestBuffer();

  char* newLine = NULL;
  char* pReqBufferCopy = new char[20000];
  char* pReqBufferCopy2 = pReqBufferCopy;

  strcpy(pReqBufferCopy, pReqBuffer);

  // replace endline (\r\n) with '!!' symbols
  if (pReqBufferCopy2)
  {
    while ((newLine = strstr(pReqBufferCopy2, "\r\n")) != NULL)
    {
      *newLine = '!';
      *(++newLine) = '!';
      pReqBufferCopy2 = newLine;
    }

    strcpy(RtspRequestBuffer, pReqBufferCopy);
    delete[] pReqBufferCopy;
    pReqBufferCopy = pReqBufferCopy2 = NULL;

    Log->Drop("%s_REQUEST=%s", rtspCommand, RtspRequestBuffer);
    // Preparing RTSP request finished
  }

  // Prepare RTSP response for sending to Test Tool
  //unsigned int respSize = rtsp->ResponseBufferSize();
  const char* pRespBuffer = rtsp->GetRtspResponseBuffer();

  if (pRespBuffer && Ret)
  {
    strcat((char*)pRespBuffer, Ret);
  }

  char* pRespBufferCopy = new char[20000];
  char* pRespBufferCopy2 = pRespBufferCopy;
  strcpy(pRespBufferCopy, pRespBuffer);

  // replace endline (\r\n) with '!!' symbols
  if (pRespBufferCopy2)
  {
    while ((newLine = strstr(pRespBufferCopy2, "\r\n")) != NULL)
    {
      *newLine = '!';
      *(++newLine) = '!';
      pRespBufferCopy2 = newLine;
    }

    strcpy(RtspResponseBuffer, pRespBufferCopy);
    delete[] pRespBufferCopy;
    pRespBufferCopy = pRespBufferCopy2 = NULL;

    Log->Drop("%s_RESPONSE=%s", rtspCommand, RtspResponseBuffer);
    // Preparing RTSP response finished
  }

}
void CapsuleTrack::ReadSourceCallback(void *params)
{
	CapsuleTrack *capsuleTrack = (CapsuleTrack *)params;
	if (capsuleTrack->sub->readSource()->isCurrentlyAwaitingData()) return;
	capsuleTrack->sub->readSource()->getNextFrame(capsuleTrack->Buffer.GetBuffer(),  capsuleTrack->Buffer.GetSize(),
												  capsuleTrack->AfterGetting, capsuleTrack, 
												  capsuleTrack->OnClose, capsuleTrack);
}

void CapsuleTrack::ReadSource()
{
	sub->readSource()->envir().taskScheduler().scheduleDelayedTask(0, ReadSourceCallback, this);
	//if (sub->readSource()->isCurrentlyAwaitingData()) return;
	//sub->readSource()->getNextFrame(Buffer.GetBuffer(), Buffer.GetSize(),
	//								AfterGetting, this, 
	//								OnClose, this);
}

void CapsuleTrack::StopGettingFramesCallback(void *params)
{
	CapsuleTrack *capsuleTrack = (CapsuleTrack *)params;
	capsuleTrack->sub->rtpSource()->stopGettingFrames();
}

void CapsuleTrack::StopGettingFrames()
{
	sub->readSource()->envir().taskScheduler().scheduleDelayedTask(0, StopGettingFramesCallback, this);
}

void CapsuleTrack::AfterGetting(
                          void* clientData, 
                          unsigned frameSize,
                          unsigned numTruncatedBytes,
                          struct timeval presentationTime,
                          unsigned durationInMicroseconds)
{
  CapsuleTrack *Track = (CapsuleTrack*)clientData;

  if (Track->Supporter->AsType() == CODEC_TYPE_META)
  {
    if (frameSize > 20)
      memcpy(Track->Parent->LastEvent, Track->Buffer.GetBuffer(), frameSize+1);
    //DropDebug("Event occurs [%s]\n", Track->Buffer.GetBuffer());
    if (Track->Parent->HasVideo() && Track->Parent->HasMeta())
      goto next; // skip metadata rendering if we have real video
  }

  FrameInfo *Info = Track->Supporter->NextChunk(
    Track->sub, Track->Buffer.GetBuffer(), frameSize, Track->Prefix);
  if (!Info) goto next; // unsupported frame type


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

  if (Track->afterGetting != NULL)
    (*(Track->afterGetting))(Track->Parent, presentationTime);

next:
  if (Track->Parent->IsAlive())
    Track->ReadSource();
}

int LastTick;
void CapsuleTrack::PutBottom(FrameInfo* Info)
{
  LastTick = GetTickCount();
  NewFramesArrived = 1;
  Parent->BalanceLoad();

  //DropDebug("Frame: %u, %u", Info->frameHead.Start, Info->frameHead.InterFrame ? 1 : 0);
 bool isInterFrame = Info->frameHead.InterFrame;

  Frames.PutQueueObject(Info);
  InterlockedIncrement(&FramesCount);
  InterlockedIncrement(&KeyInterval);
  if (!Parent->IsFakeVideo())
  {
    if (!isInterFrame)
    {
      CalcKeyTime();
    }
  }

  InterlockedIncrement(&ReceivedFramesCount);
}

void LiveCapsule::UnlockTracks()
{
  UnlockingTracks = true;
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
#ifdef USE_MANAGED_MEMORY
  FrameInfoPtr p = Frames.GetQueueObject();
  p.Unlink();
  return p;
#else // USE_MANAGED_MEMORY
  return Frames.GetQueueObject();
#endif // USE_MANAGED_MEMORY
}

CapsuleTrack::~CapsuleTrack()
{
  DropDebug("Rest Tracks %d\n", FramesCount);
  if (sub) delete sub; sub = NULL;
  FrameInfo::Release(Prefix);
  Prefix = NULL;
  while (FramesCount--)
  {
    FrameInfo *fi = Frames.GetQueueObject();
    FrameInfo::Release(fi);
  }
}

bool LiveCapsule::OpenSettings(const WCHAR* FileName)
{
  Reset();

  Settings.Load(FileName);
  char *filename = ToMulti(FileName);
  Log->AttachIni(filename);
  delete[] filename;
  if (Settings.RTSP)
  {
    return OpenStream(Settings.Address);
  }
  else
  {
    return OpenRtpStream();
  }
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
    if (LastParameterTick < GetTickCount() - Settings.TimeOut * 1000)
    {
      LastParameterTick = GetTickCount();
      DropDebug("TrySetParameter %i", LastParameterTick);

      if (Settings.UseKeepAliveOptions)
      {
        rtsp->setSocketReadAllMode(true);
        char* Options = rtsp->sendOptionsCmd(Settings.Address, Settings.User, Settings.Password, NULL, Settings.TimeOut);
        rtsp->setSocketReadAllMode(false);
        LogRtspRequestAndResponse("OPTIONS", NULL);
      }
      else
      {
        TrySetParameter();
      }
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
      if (!Settings.ReplayMode && !Settings.CheckJPEGExtension)
      {
        StopFlag = 1;
      }
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
  DropDebug("********** STEP STARTED: %d", Step);
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
void LiveCapsule::SetStepFail(int Step, const char *LastError)
{
  if (this->Step != Step)
  {
    RunStep(Step);
  }
  FailBits |= 1 << Step;
  Log->Drop("");
  //Log->Drop("Step %i Fail", Step);
  Log->SetStepEnd(PassBits, FailBits, LastError);
};

bool LiveCapsule::OpenRtpStream()
{ 
  if (!InitEnvironment()) return false;

  RunStep(step_OpenStream);

  bool Result = false;
  if (Settings.UseVideo)
  {
    IMediaSubsession *sub = new CapsuleRtpSubsession(env, &Settings, true, Log);
    GeneralFormatSupporter *supporter = ProbeByCodec(sub);
    Result = sub->initiate();
    if (Result) Result = UseSubSource(sub, supporter);
  }
  if (Settings.UseAudio)
  {
    IMediaSubsession *sub = new CapsuleRtpSubsession(env, &Settings, false, Log);
    GeneralFormatSupporter *supporter = ProbeByCodec(sub);
    Result = sub->initiate();
    if (Result) Result = UseSubSource(sub, supporter);
  }
  // use fake video with audio-only session
  if (Audio && !Settings.UseVideo) 
  {
    if (Video)
    {
      delete Video;
    }
    Video = new CapsuleTrack(this, NULL, ProbeByCodec(NULL));
  }

  if ((Settings.UseVideo && !Video) || (Settings.UseAudio && !Audio))
  {
    if (Settings.UseVideo && !Video) 
      Log->Drop("Video track not found");
    if (Settings.UseAudio && !Audio) 
      Log->Drop("Audio track not found");
    Result = false;
  }

  if (!Result)
  {
    SetStepFail(step_OpenStream);
    HaltEnvironment();
    return false;
  }

  SetStepOK(step_OpenStream);
  State = RTSP_STATE_OPENED;
  return true;
}

CapsuleRtpSubsession::CapsuleRtpSubsession(UsageEnvironment *env, CapsuleSettings *settings, bool video, MessageLog* log)
  : mEnv(env), 
    mSettings(settings),
    mRtpGroupsock(NULL),
    mRtcpGroupsock(NULL),
    mRtpSource(NULL),
    mRtcpInstance(NULL),
    mAddress(NULL),
    mWarning(false),
    mLog1(log)
{ 
  mMediumName = (video ? "video" : "audio");
  mCodecName = (video
                ? mSettings->VideoCodecName 
                : mSettings->AudioCodecName);
  mRtpPort = (video
              ? mSettings->MulticastRtpPortVideo 
              : mSettings->MulticastRtpPortAudio);
  mRtpTimestampFrequency = (video ? 90000 : 8000);
  mAddress = (video 
              ? mSettings->MulticastAddress
              : (strlen(mSettings->MulticastAddressAudio) == 0
                 ? mSettings->MulticastAddress
                 : mSettings->MulticastAddressAudio));
}

CapsuleRtpSubsession::~CapsuleRtpSubsession()
{
  if (mRtcpInstance)
    Medium::close(mRtcpInstance);
  if (mRtcpGroupsock)
    delete mRtcpGroupsock;
  if (mRtpSource)
    Medium::close(mRtpSource);
  if (mRtpGroupsock)
    delete mRtpGroupsock; 
  mRtcpInstance = NULL;
  mRtpSource = NULL;
  mRtpGroupsock = mRtcpGroupsock = NULL;
}

bool CheckPayloadTypeCallback(u_int8_t type, unsigned char *packetData, unsigned int packetSize, void* pPriv)
{
  CapsuleRtpSubsession *sub = (CapsuleRtpSubsession*)pPriv;
  return sub->CheckPayload(type, packetData, packetSize);
}

bool CapsuleRtpSubsession::CheckPayload(unsigned char type, unsigned char *packetData, unsigned int packetSize)
{

  if (strcmp(mCodecName, "H264") == 0 && (type == 96 || type == 98))
    return true;

  // AAC
  if (strcmp(mCodecName, "MPEG4-GENERIC") == 0 && (type == 96 || type == 97))
    return true;

  // if we expect JPEG, but NVT sends incorrect stream
  if (strcmp(mCodecName, "JPEG") == 0 && type != 26)
  {
    if (mWarning)
    {
      mLog1->Drop("There was not JPEG stream received");
      mWarning = false; 
    }
    return false;
  }

  // if we expect G711, but NVT sends incorrect stream
  if (strcmp(mCodecName, "PCMU") == 0 && type != 0)
  {
    if (mWarning)
    {
      mLog1->Drop("There was not G.711 stream received");
      mWarning = false; 
    }
    return false;
  }

  // Get JPEG, but expect other
  if (type == 26 && strcmp(mCodecName, "JPEG") != 0)
  {
    if (mWarning)
    {
      mLog1->Drop("There was JPEG stream received. Expected: %s", mCodecName);
      mWarning = false; 
    }
    return false;
  }

  // Get G711, but expect other
  if (type == 0 && strcmp(mCodecName, "PCMU") != 0)
  {
    if (mWarning)
    {
      mLog1->Drop("There was G.711 stream received. Expected: %s", mCodecName);
      mWarning = false; 
    }
    return false;
  }

  return true;
}

Boolean CapsuleRtpSubsession::initiate(int useSpecialRTPoffset)
{
  const unsigned short rtpPortNum  = mRtpPort;
  const unsigned short rtcpPortNum = rtpPortNum + 1;
  const unsigned char  ttl         = mSettings->MulticastTTL;

  NetAddressList addresses(mAddress);
  if (addresses.numAddresses() == 0) return False;
  NetAddress sessionAddress = *addresses.firstAddress();
  const Port rtpPort(rtpPortNum);
  const Port rtcpPort(rtcpPortNum);

  mRtpGroupsock = new Groupsock(*mEnv, sessionAddress, rtpPort, ttl);
  mRtcpGroupsock = new Groupsock(*mEnv, sessionAddress, rtcpPort, ttl);

  if (!mRtpGroupsock || !mRtcpGroupsock)
    return False;

  if (strcmp(mCodecName, "JPEG") == 0)
  {
	  mRtpSource = JPEGVideoRTPSource::createNew(*mEnv, mRtpGroupsock);
  }
  else if (strcmp(mCodecName, "PCMU") == 0)
  {
  	char* mimeType = new char[strlen(mediumName()) + strlen(codecName()) + 2];
	  sprintf(mimeType, "%s/%s", mediumName(), codecName());
	  mRtpSource = SimpleRTPSource::createNew(*mEnv, mRtpGroupsock, 0,
				                                    rtpTimestampFrequency(), mimeType,
				                                    0, False);
	  delete[] mimeType;
  }
  else if (strcmp(mCodecName, "G726") == 0)
  {
    mRtpSource = SimpleRTPSource::createNew(*mEnv, mRtpGroupsock, 0,
      rtpTimestampFrequency(), "audio/G726-32",
      0, False);
    mRtpSource->setCheckPldTypeCallback(CheckPayloadTypeCallback, this);
  }
  else if (strcmp(mCodecName, "MPEG4-GENERIC") == 0) // AAC
  {
    mRtpSource = MPEG4GenericRTPSource::createNew(
      *mEnv, mRtpGroupsock, 0, rtpTimestampFrequency(), "audio/MPEG4-GENERIC", "aac-hbr", 13, 3, 3);
    mRtpSource->setCheckPldTypeCallback(CheckPayloadTypeCallback, this);
  }
  else if (strcmp(mCodecName, "H264") == 0)
  {
    mRtpSource = H264VideoRTPSource::createNew(*mEnv, mRtpGroupsock, 0, rtpTimestampFrequency());
    mRtpSource->setCheckPldTypeCallback(CheckPayloadTypeCallback, this);
  }     
  else if (strcmp(mCodecName, "MP4V-ES") == 0)
  {
    mRtpSource = MPEG4ESVideoRTPSource::createNew(*mEnv, mRtpGroupsock, 0, rtpTimestampFrequency());
    mRtpSource->setCheckPldTypeCallback(CheckPayloadTypeCallback, this);
  }     
  else
    return False;

  // Create (and start) a 'RTCP instance' for the RTP source:
  const unsigned estimatedSessionBandwidth = 160; // in kbps; for RTCP b/w share
  const unsigned maxCNAMElen = 100;
  unsigned char CNAME[maxCNAMElen + 1];
  gethostname((char*)CNAME, maxCNAMElen);
  CNAME[maxCNAMElen] = '\0'; // just in case
  mRtcpInstance = RTCPInstance::createNew(*mEnv, mRtcpGroupsock,
                                          estimatedSessionBandwidth, CNAME,
                                          NULL, // we're a client 
                                          mRtpSource);

  if (!mRtcpInstance)
    return False;

  return True;
}

void RtpExtHdrCallback(u_int16_t profile, u_int16_t seq, u_int16_t len, u_int8_t* pHdrData, void* pPriv)
{
  CapsuleMediaSubsession *sub = (CapsuleMediaSubsession*)pPriv;
  OutputDebugStringA("RtpExtHdrCallback1\n");
  // JPEG extension checking
  if (!sub->IsReplay() && (profile == 0xFFD8 || profile == 0xFFFF)) {
    if (!sub->mCheckingPackets) return;
  OutputDebugStringA("RtpExtHdrCallback2\n");

    // 1: processed; 2: failed; 3: not processed in this step
    u_int32_t SpecialHeaderStatus = *((u_int32_t*)pHdrData);

    unsigned int FrameWidth = sub->rtpSource()->fFrameWidth;
    unsigned int FrameHeight = sub->rtpSource()->fFrameHeight;

    if (SpecialHeaderStatus == 2 && *((u_int32_t*)pHdrData) == 1)
    {
      ++sub->mFailedPacketsReceived;
      if (sub->DropMsg)
      {
        sub->Log()->Drop("SeqNum=%u Incorrecr header!", htons(seq));
      }
    }
    if (SpecialHeaderStatus == 1) 
    {
      ++sub->mPacketsReceived;
      if (sub->DropMsg)
      {
        sub->Log()->Drop("SeqNum=%u Resolution: %u x %u", htons(seq), FrameWidth, FrameHeight);
      }
    }
    sub->rtpSource()->fFrameWidth = 0;
    sub->rtpSource()->fFrameHeight = 0;
    sub->DropMsg = false;
  }

  // replay header extension checking
  if (sub->IsReplay() && (profile == 0xABAC)) {
    OutputDebugStringA("RtpExtHdrCallback3\n");

    // we need to remember first packet for each CSeq set
/*    bool parseFirst = (!sub->mFirstPacket 
                       || ((sub->mPlayCommandCount > 1) // skip any check for the first PLAY
                           && sub->mFirstPacket
                           && sub->mFirstPacket->CSeq != (RTSPClient::CSeq() - 1)));
*/
    bool parseFirst = (!sub->HaveFirst
                       || ((sub->mPlayCommandCount > 1) // skip any check for the first PLAY
                           && sub->HaveFirst
                           && sub->firstPacket.CSeq != (RTSPClient::CSeq() - 1)));

//    RtpReplayExtension &packet = sub->packets[parseFirst ? 1 : 0];

    if (parseFirst) {
    OutputDebugStringA("RtpExtHdrCallback3 - first packet\n");
    }

    RtpReplayExtension packet;
    // garbage for compatibility
    packet.canSkip = !parseFirst;

    // Packet receiving time
    packet.creationTime = GetTickCount();

    // NTP time
    packet.dateTime = ntohl(*((u_int32_t*)pHdrData)) - 2208988800UL;
    pHdrData += 4;
    packet.secFrac = ntohl(*((u_int32_t*)pHdrData));
    pHdrData += 4;

    packet.bitC = (*pHdrData >> 7) & 1;
    packet.bitE = (*pHdrData >> 6) & 1;
    packet.bitD = (*pHdrData >> 5) & 1;
    packet.CSeq = *(++pHdrData);

    if ((sub->mPlayCommandCount > 1) && (packet.CSeq != (RTSPClient::CSeq() - 1))) {
  OutputDebugStringA("RtpExtHdrCallback4\n");
      return;
    }

    packet.seq = htons(seq);
    pHdrData += 1;
    packet.validLength = *pHdrData & 1;
    packet.startsFrame = (*pHdrData >> 1) & 1;
    packet.endsFrame   = (*pHdrData >> 2) & 1;
    pHdrData += 2;
    // RTP time
    packet.timestamp = *((u_int32_t*)pHdrData);
    pHdrData += 4;
    packet.payload = *((u_int8_t*)pHdrData);

/*    if (parseFirst)
    {
  OutputDebugStringA("RtpExtHdrCallback5\n");
      sub->mFirstPacket = &packet;
      sub->mFirstPacket->sent = false;
    }
    else if (sub->mCheckingPackets)*/
    {
  OutputDebugStringA("RtpExtHdrCallback6\n");
      // skip packets from previous command
      if ((sub->mPlayCommandCount > 1) // skip any check for the first PLAY
          && (packet.CSeq != (RTSPClient::CSeq() - 1))) {
  OutputDebugStringA("RtpExtHdrCallback7\n");
        return;
      }
/*
      // write first packet to log before any other packet
      if (sub->mFirstPacket && !sub->mFirstPacket->sent) {
  OutputDebugStringA("RtpExtHdrCallback8\n");
        sub->mFirstPacket->ToString(sub->Log(), false);
      }

      packet.ToString(sub->Log());*/
      sub->DropLastPacket(packet);
      if (!parseFirst) {
  //OutputDebugStringA("RtpExtHdrCallback - inc\n");
        ++sub->mPacketsReceived;
      }
  OutputDebugStringA("RtpExtHdrCallback9\n");
    }
  }
}

Boolean CapsuleMediaSubsession::initiate(int useSpecialRTPoffset)
{ 
  return mSub->initiate(useSpecialRTPoffset, RtpExtHdrCallback, this); 
}
