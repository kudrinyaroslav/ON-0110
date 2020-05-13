#include "LiveCapsule.h"

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

void CapsuleSettings::Load(const WCHAR* FileName)
{
  char *filename = ToMulti(FileName);

  const WCHAR ApplicationW[] = L"Test Options";
  const  char ApplicationA[] =  "Test Options";
  GetPrivateProfileStringA(ApplicationA,
    "Address",
    "rtsp://127.0.0.1",
    Address,
    elementsof(Address),
    filename);

  TunnelPortNumber = GetPrivateProfileIntW(ApplicationW,
    L"TunnelPortNumber",
    0,
    FileName);

  UseTCPTunnel = !!GetPrivateProfileIntW(ApplicationW,
    L"UseTCPTunnel",
    0,
    FileName);

  UseHTTPTunnel = !!GetPrivateProfileIntW(ApplicationW,
    L"UseHTTPTunnel",
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

  GetPrivateProfileStringW(ApplicationW,
    L"Pipe",
    L"c:\\onviftestpipe",
    Pipe,
    elementsof(Pipe),
    FileName);

  delete[] filename;

  if (!UseTCPTunnel && !UseHTTPTunnel) TunnelPortNumber = 0;
}

bool LiveCapsule::InitEnvironment()
{
  StopFlag = 0;
  scheduler =  BasicTaskScheduler::createNew();
  if (scheduler ==	NULL)
  {
    err("BasicTaskScheduler fail \n");
    goto fail;
  }
  env = BasicUsageEnvironment::createNew(*scheduler);
  if (env == NULL)
  {
    err("BasicUsageEnvironment fail \n");
    goto fail;
  }
  rtsp = RTSPClient::createNew(*env, 0, AppName, Settings.TunnelPortNumber);
  if (rtsp == NULL)
  {		 
    err("create rtsp client fail \n");

    goto fail;
  }
  return true;
fail:
  return false;
}

void LiveCapsule::HaltEnvironment()
{
  State = RTSP_STATE_IDLE;

  StopFlag = 1;
  Close();

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
}


char* LiveCapsule::DescribeStream(const char* filename)
{
  if (Settings.User[0] && Settings.Password[0])
  {
    if (rtsp->sendOptionsCmd(filename, Settings.User, Settings.Password) == NULL)
    {
      return NULL;
    }
    return rtsp->describeWithPassword(filename, Settings.User, Settings.Password);
  }
  else
  {
    if (rtsp->sendOptionsCmd(filename) == NULL)
    {
      return NULL;
    }
    return rtsp->describeURL(filename);
  }
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
}


bool LiveCapsule::OpenStream(const char* filename)
{
  if (!InitEnvironment()) return false;
  char* sdpDescription = DescribeStream(filename);
  if (!sdpDescription) return false;

  ms = MediaSession::createNew(*env, sdpDescription);
  delete[] sdpDescription;

  MediaSubsession         *sub;
  MediaSubsessionIterator *iter = new MediaSubsessionIterator(*ms);
  while( ( sub = iter->next() ) != NULL )
  {
    GeneralFormatSupporter *supporter = ProbeByCodec(sub);
    if (!supporter) continue;
    if (!sub->initiate())
    {
      err("RTP subsession '%s/%s' failed (%s)",
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
  if (!Audio && !Video)
  {
    HaltEnvironment();
    return false;
  }
  State = RTSP_STATE_OPENED;
  return true;
}

bool LiveCapsule::PlayStream()
{
  if (State == RTSP_STATE_PAUSED)
  {
    if (!rtsp->playMediaSession(*ms, -1))
    {
      err("playMediaSession fail \n");
      return false;
    }
  }
  if (State != RTSP_STATE_OPENED)
  {
    return false;
  }

  // from RTSP_STATE_OPENED

  if (!ThreadExists())
    if (!Create())
    {
      err("rtspClientPlayStream receive thread failed \n");
      return false;
    }

  rtsp->setupMediaSubsession(*Video->sub);
  if (!rtsp->playMediaSession(*ms, 0.0 , -1, 1 ))
  {
    err("playMediaSession fail \n");
    return false;
  }
  State  = RTSP_STATE_PLAYING;
  return true;
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
  return CloseStream();
}

bool LiveCapsule::CloseStream()
{
  if (State == RTSP_STATE_IDLE)
  {
    return true;
  }

  if (rtsp && ms)
  {
    rtsp->teardownMediaSession(*ms);
  }

  State = RTSP_STATE_IDLE;

  if (Video) delete Video;
  if (Audio) delete Audio;

  HaltEnvironment();

  return true;
}

DWORD LiveCapsule::ThreadProc()
{
  if (Video) 
  {
    Video->ReadSource();
  };
  if (Audio)
  {
    Audio->ReadSource();
  };
  env->taskScheduler().doEventLoop(&StopFlag);
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
  FrameInfo *Info = Track->Supporter->NextChunk(Track->sub, Track->Buffer.GetBuffer(), frameSize);
  Info->frameHead.FrameType = Track->Supporter->AsFOURCC();
#if 1
  Info->frameHead.Start = 
    ((REFERENCE_TIME)presentationTime.tv_sec) * 10000000 +
    ((REFERENCE_TIME)presentationTime.tv_usec) * 10;
  if (!Track->Start) Track->Start = Info->frameHead.Start;
  Info->frameHead.Start -= Track->Start;
#else
  unsigned long i_pts = presentationTime.tv_sec*1000 + presentationTime.tv_usec/1000;
  Info->frameHead.TimeStamp = i_pts;
#endif
  Track->Frames.PutQueueObject(Info);
  Track->ReadSource();
}

void CapsuleTrack::OnClose(void* clientData)
{

}

FrameInfo* CapsuleTrack::GetTop()
{
  return Frames.GetQueueObject();
}

bool LiveCapsule::OpenSettings(const WCHAR* FileName)
{
  Settings.Load(FileName);
  return OpenStream(Settings.Address);
}
