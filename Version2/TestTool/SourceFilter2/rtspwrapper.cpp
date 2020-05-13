// wrapper.cpp : Defines the entry point for the console application.
//

#include <stdio.h>
#include <tchar.h>

#include <iostream>
#include <vector>
#include <process.h>

#include "liveMedia.hh"
#include "BasicUsageEnvironment.hh"
#include "rtspwrapper.h"
#include "DigestAuthentication.hh"
#include "backchannelwrapper.h"

#define APPLICATION_NAME "ONVIF RTSP Client 15.06"

class CWrapRTSPClient: public RTSPClient {
public:
  static CWrapRTSPClient* createNew(CLive555Wrapper *pWrapper)
  {
    return new CWrapRTSPClient(pWrapper);
  };

protected:
  CWrapRTSPClient(CLive555Wrapper *pWrapper);
    // called only by createNew();
  //virtual ~CWrapRTSPClient();

  virtual void ReflectSend(const char* Data);
  virtual void ReflectReceive(const char* Data);

public:
  CLive555Wrapper *pWrapper;
  void Reset() { reset(); setBaseURL(pWrapper->RtspAddress.c_str()); };
};

CWrapRTSPClient::CWrapRTSPClient(CLive555Wrapper *pWrapper)
  : RTSPClient(*pWrapper->Environment, pWrapper->RtspAddress.c_str(), pWrapper->VerbosityLevel, APPLICATION_NAME, (pWrapper->Transport == ELT_HTTP) ? pWrapper->TunnelPort : 0, -1),
  pWrapper(pWrapper)
{
}

void CWrapRTSPClient::ReflectSend(const char* Data)
{
  if (pWrapper->SendNotify) {
    pWrapper->SendNotify(pWrapper->NotifyData, pWrapper->CurrentStep, Data);
  }
}

void CWrapRTSPClient::ReflectReceive(const char* Data)
{
  if (pWrapper->ReceiveNotify) {
    pWrapper->ReceiveNotify(pWrapper->NotifyData, pWrapper->CurrentStep, Data);
  }
}

class CWrapSink: public MediaSink {
public:
  static CWrapSink* createNew(UsageEnvironment& env, CWrapRTSPClient* RtspClient, ELiveChannels Channel);

  CWrapRTSPClient* rtspClient() const { return RtspClient; };

private:
  CWrapSink(UsageEnvironment& env, CWrapRTSPClient* RtspClient, ELiveChannels Channel);
    // called only by "createNew()"
  virtual ~CWrapSink();

  static void afterGettingFrame(void* clientData, 
    unsigned frameSize, unsigned numTruncatedBytes,
    struct timeval presentationTime, unsigned durationInMicroseconds);
  void afterGettingFrame(unsigned frameSize, unsigned numTruncatedBytes,
    struct timeval presentationTime, unsigned durationInMicroseconds);
private:
  // redefined virtual functions:
  virtual Boolean continuePlaying();

private:
  u_int8_t* fReceiveBuffer;
  int BufferSize;
  ELiveChannels Channel;
  CWrapRTSPClient* RtspClient;
};


CWrapSink* CWrapSink::createNew(UsageEnvironment& env, CWrapRTSPClient* RtspClient, ELiveChannels Channel) 
{
  return new CWrapSink(env, RtspClient, Channel);
}

CWrapSink::CWrapSink(UsageEnvironment& env, CWrapRTSPClient* RtspClient, ELiveChannels Channel)
  : MediaSink(env), RtspClient(RtspClient), Channel(Channel) 
{
  BufferSize = 64*1024*3;
  fReceiveBuffer = new u_int8_t[BufferSize];
}

CWrapSink::~CWrapSink() 
{
  delete[] fReceiveBuffer;
}

void CWrapSink::afterGettingFrame(void* clientData, unsigned frameSize, unsigned numTruncatedBytes,
  struct timeval presentationTime, unsigned durationInMicroseconds) 
{
  CWrapSink* sink = (CWrapSink*)clientData;
  sink->afterGettingFrame(frameSize, numTruncatedBytes, presentationTime, durationInMicroseconds);
}

// If you don't want to see debugging output for each received frame, then comment out the following line:
//#define DEBUG_PRINT_EACH_RECEIVED_FRAME 1

void CWrapSink::afterGettingFrame(unsigned frameSize, unsigned numTruncatedBytes,
  struct timeval presentationTime, unsigned durationInMicroseconds) 
{
  //CWrapRTSPClient* RtspClient = (CWrapRTSPClient*)(fSubsession.miscPtr);
  RtspClient->pWrapper->AfterGettingFrame(Channel, fReceiveBuffer, frameSize, numTruncatedBytes,
				   presentationTime, durationInMicroseconds);
  // Then continue, to request the next frame of data:
  if (numTruncatedBytes > 0) {
    BufferSize += 64*1024;
    delete[] fReceiveBuffer;
    fReceiveBuffer = new u_int8_t[BufferSize];
  }
  continuePlaying();
}

Boolean CWrapSink::continuePlaying() 
{
  if (fSource == NULL) return False; // sanity check (should not happen)

  // Request the next frame of data from our input source.  "afterGettingFrame()" will get called later, when it arrives:
  fSource->getNextFrame(fReceiveBuffer, BufferSize,
                        afterGettingFrame, this,
                        onSourceClosure, this);
  return True;
}

void ShutdownStream(RTSPClient* rtspClient, int exitCode = 1);

void SubsessionAfterPlaying(void* ClientData) 
{
  MediaSubsession* Subsession = (MediaSubsession*)ClientData;
  RTSPClient* RtspClient = (RTSPClient*)(Subsession->miscPtr);

  // Begin by closing this subsession's stream:
  Medium::close(Subsession->sink);
  Subsession->sink = NULL;

  // Next, check whether *all* subsessions' streams have now been closed:
  MediaSession& session = Subsession->parentSession();
  MediaSubsessionIterator iter(session);
  while ((Subsession = iter.next()) != NULL) {
    if (Subsession->sink != NULL) return; // this subsession is still active
  }

  // All subsessions' streams have now been closed, so shutdown the client:
  ShutdownStream(RtspClient);
}

void SubsessionAfterPlayingRtp(void* ClientData) 
{
  CWrapSink *Sink = (CWrapSink*)ClientData;
  RTSPClient* RtspClient = Sink->rtspClient();

  // Begin by closing this subsession's stream:
  Medium::close(Sink);

  // All subsessions' streams have now been closed, so shutdown the client:
  ShutdownStream(RtspClient);
}

void SubsessionByeHandler(void* ClientData) {
  MediaSubsession* Subsession = (MediaSubsession*)ClientData;
  RTSPClient* RtspClient = (RTSPClient*)Subsession->miscPtr;
  UsageEnvironment& env = RtspClient->envir(); // alias

  //env << "Received RTCP \"BYE\" on \"" << *subsession << "\" subsession\n";

  // Now act as if the subsession had closed:
  SubsessionAfterPlaying(Subsession);
}

void StreamTimerHandler(void* ClientData) {
  CWrapRTSPClient* RtspClient = (CWrapRTSPClient*)ClientData;
  //StreamClientState& scs = rtspClient->scs; // alias

  //scs.streamTimerTask = NULL;
  //
  // Shut down the stream:
  //shutdownStream(rtspClient);
}

void ShutdownStream(RTSPClient* rtspClient, int exitCode) 
{
/*  UsageEnvironment& env = rtspClient->envir(); // alias
  StreamClientState& scs = ((ourRTSPClient*)rtspClient)->scs; // alias

  // First, check whether any subsessions have still to be closed:
  if (scs.session != NULL) { 
    Boolean someSubsessionsWereActive = False;
    MediaSubsessionIterator iter(*scs.session);
    MediaSubsession* subsession;

    while ((subsession = iter.next()) != NULL) {
      if (subsession->sink != NULL) {
	Medium::close(subsession->sink);
	subsession->sink = NULL;

	if (subsession->rtcpInstance() != NULL) {
	  subsession->rtcpInstance()->setByeHandler(NULL, NULL); // in case the server sends a RTCP "BYE" while handling "TEARDOWN"
	}

	someSubsessionsWereActive = True;
      }
    }

    if (someSubsessionsWereActive) {
      // Send a RTSP "TEARDOWN" command, to tell the server to shutdown the stream.
      // Don't bother handling the response to the "TEARDOWN".
      rtspClient->sendTeardownCommand(*scs.session, NULL);
    }
  }

  env << *rtspClient << "Closing the stream.\n";
  Medium::close(rtspClient);
    // Note that this will also cause this stream's "StreamClientState" structure to get reclaimed.

  if (--rtspClientCount == 0) {
    // The final stream has ended, so exit the application now.
    // (Of course, if you're embedding this code into your own application, you might want to comment this out,
    // and replace it with "eventLoopWatchVariable = 1;", so that we leave the LIVE555 event loop, and continue running "main()".)
    exit(exitCode);
  }*/
}

void CThread::RunThread()
{
  ThreadStarted = false;
  ThreadStopped = false;
  unsigned ThreadID;
  hThread = (HANDLE)_beginthreadex(NULL, 0, &ThreadProc, this, 0, &ThreadID);
}

void CThread::StopThread()
{
  if (ThreadStarted) {
    //while (!ThreadStopped) Sleep(100);
    WaitForSingleObject(hThread, INFINITE);
    CloseHandle(hThread);
    hThread = NULL;
  }
}

unsigned __stdcall CThread::ThreadProc(void* Ptr)
{
  CThread* pThread = (CThread*)Ptr;
  pThread->ThreadStarted = true;
  pThread->Thread();
  pThread->ThreadStopped = true;
  _endthreadex(0);
  return 0;
}



CLive555Wrapper::CLive555Wrapper() :
  CurrentStep(ELS_Begin),
  NeedTeardown(false),
  CurrentSubStep(0),
  InProgress(false),
  Result(0),
  ResultString(NULL),
  SendNotify(NULL),
  ReceiveNotify(NULL),
  LogNotify(NULL),
  NotifyData(NULL),
  Complete(NULL),
  CompleteData(NULL),
  EventLoopWatchVariable(0),
  Transport(ELT_UDP),
  RtspClient(NULL),
  Scheduler(NULL),
  Environment(NULL),
  Session(NULL),
  Credentials(NULL),
  VerbosityLevel(0),
  TunnelPort(0)
  //CurrentChannel(0)
{
  {
    CLocker l(ConfigurationDoor);
    Scheduler = BasicTaskScheduler::createNew();
    Environment = BasicUsageEnvironment::createNew(*Scheduler);
  }
  RunThread();
}

CLive555Wrapper::~CLive555Wrapper()
{
  StopThread();
  CLocker l(ConfigurationDoor);
  CleanupChannels();
  Medium::close(RtspClient);
  Environment->reclaim(); Environment = NULL;
  delete Scheduler; Scheduler = NULL;
  delete[] ResultString;
  delete Credentials;
}

void CLive555Wrapper::RunThread()
{
  EventLoopWatchVariable = 0;
  if (VerbosityLevel >= 1) {
    *Environment << "Starting notification thread\n";
  };
  CThread::RunThread();
}

void CLive555Wrapper::StopThread()
{
  if (!EventLoopWatchVariable) {
    if (VerbosityLevel >= 1) {
     *Environment << "Stopping notification thread\n";
    };
    EventLoopWatchVariable = 1;
  } else {
    if (VerbosityLevel >= 1) {
     *Environment << "Notification thread already stopped\n";
    }
  }
  CThread::StopThread();
}

void CLive555Wrapper::Thread()
{
  if (!EventLoopWatchVariable) {
    if (VerbosityLevel >= 1) {
      *Environment << "Starting event loop\n";
    }
    Environment->taskScheduler().doEventLoop((char*)&EventLoopWatchVariable);
  }
  if (VerbosityLevel >= 1) {
    *Environment << "Event loop stopped\n";
  }
}

void CLive555Wrapper::AttachNotify(NotifyFunction Send, NotifyFunction Receive, NotifyFunction Log, void* NotifyData)
{
  CLocker l(ConfigurationDoor);

  this->SendNotify = Send;
  this->ReceiveNotify = Receive;
  this->LogNotify = Log;
  this->NotifyData = NotifyData;
}

void CLive555Wrapper::AttachCompletion(CompleteFunction Complete, void* CompleteData)
{
  CLocker l(ConfigurationDoor);

  this->Complete = Complete;
  this->CompleteData = CompleteData;
}

void CLive555Wrapper::AttachChannel(ELiveChannels Channel, const char* Codec, DataFunction Func, void *UserData, const char *Address, int Port, int TTL)
{
  CLocker l(ConfigurationDoor);

  SDataChannel ChannelDesc;
  ChannelDesc.Channel = Channel;
  ChannelDesc.Codec = Codec;
  ChannelDesc.Func = Func;
  ChannelDesc.UserData = UserData;
  if (Address) {
    ChannelDesc.Address = Address;
  }
  ChannelDesc.Port = Port;
  ChannelDesc.TTL = TTL;
  ChannelDesc.Subsession = NULL;
  ChannelDesc.Sink = NULL;
  ChannelDesc.RtpSource = NULL;
  ChannelDesc.RtcpInstance = NULL;
  Channels.push_back(ChannelDesc);
}

bool CLive555Wrapper::AttachRtpCallback(ELiveChannels Channel, void (*RtpExtHdrCallback)(u_int16_t profile, u_int16_t seq, u_int16_t len, u_int8_t* pHdrData, void* pPriv), void *UserData)
{
  CLocker l(ConfigurationDoor);

  MediaSubsession* sub = GetChannelSession(Channel);
  if (!sub) {
    return false;
  }
  RTPSource *source = sub->rtpSource();
  if (!source) {
    return false;
  };
  source->setRtpExtHdrCallback(RtpExtHdrCallback, UserData);
  return true;
}

MediaSubsession* CLive555Wrapper::GetChannelSession(ELiveChannels Channel)
{
  CLocker l(ConfigurationDoor);

  for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
    if (i->Channel == Channel) {
      return i->Subsession;
    }
  };
  return NULL;
}

RTPSource* CLive555Wrapper::GetChannelSource(ELiveChannels Channel)
{
  CLocker l(ConfigurationDoor);

  for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
    if (i->Channel == Channel) {
      return (i->RtpSource) ? i->RtpSource : (i->Subsession) ? i->Subsession->rtpSource() : NULL;
    }
  };
  return NULL;
}

void CLive555Wrapper::AfterGettingFrame(ELiveChannels Channel, u_int8_t* fReceiveBuffer, unsigned frameSize, unsigned numTruncatedBytes,
				  struct timeval presentationTime, unsigned durationInMicroseconds)
{
  CLocker l(ConfigurationDoor);

  for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
    if (i->Channel == Channel) {
      if (i->Func) {
        i->Func(i->UserData, fReceiveBuffer, frameSize, numTruncatedBytes, presentationTime, durationInMicroseconds);
      };
      break;
    }
  };
}

void CLive555Wrapper::WriteLog(const char* Text, ...)
{
  if (!LogNotify || !NotifyData || !Text)
    return;

  va_list ap;
  char Buffer[2048];
  va_start(ap, Text);
  _vsnprintf(Buffer, sizeof(Buffer) - 1, Text, ap);
  va_end(ap);

  if (VerbosityLevel >= 1) {
    *Environment << Buffer << "\n";
  };
  LogNotify(NotifyData, CurrentStep, Buffer);
}

void CLive555Wrapper::AttachCredentials(const char* User, const char* Password)
{
  CLocker l(ConfigurationDoor);

  Credentials = new Authenticator(User, Password);
}

bool CLive555Wrapper::Open(const char* Rtsp, ELiveTransport Transport, int TunnelPort)
{
  CLocker l(ConfigurationDoor);

  if (TunnelPort <= 0) {
    TunnelPort = 80;
  }
  this->TunnelPort = TunnelPort;
  this->Transport = Transport;
  this->RtspAddress = Rtsp;
  RtspClient = CWrapRTSPClient::createNew(this);
  if (RtspClient == NULL) {
    if (VerbosityLevel >= 1) {
      *Environment << "Failed to create a RTSP client for URL \"" << Rtsp << "\": " << Environment->getResultMsg() << "\n";
    }
    return false;
  } else {
    RtspClient->setUserAgentString(APPLICATION_NAME);
  }
  return true;
}

bool CLive555Wrapper::Prologue(ELiveStep Step)
{
  // TODO use signals here
  if (InProgress)
    return false;
  InProgress = true;
  CurrentStep = Step;
  return true;
}

void CLive555Wrapper::SignalComplete(int Result, const char* Body)
{
  // TODO use signals here
  this->Result = Result;
  InProgress = false;
  if (ResultString) {
    delete[] ResultString;
    ResultString = NULL;
  }
  ResultString = Body;
  if (Complete && !EventLoopWatchVariable) {
    Complete(CompleteData, CurrentStep, Result, Body);
  };
  if (CurrentStep == ELS_Teardown) {
    NeedTeardown = false;
    CLocker l(ConfigurationDoor);
    RtspClient->Reset();
  }
  if (CurrentStep == ELS_Play) {
    NeedTeardown = !Result;
  }
}

int CLive555Wrapper::WaitResult(unsigned int Timeout)
{
  // TODO use signals here
  Timeout /= 100;
  while (IsActive() && !EventLoopWatchVariable && (Timeout --> 0)) Sleep(100);
  if (IsActive()) {
    Terminate();
    SignalComplete(RPC_E_TIMEOUT, strDup("Timeout"));
    return RPC_E_TIMEOUT;
  }
  return GetResult();
}

int CLive555Wrapper::WaitResult(const char* &Body, unsigned int Timeout)
{
  int Ret = WaitResult(Timeout);
  Body = ResultString;
  return Ret;
}

void CLive555Wrapper::RtspHandler(RTSPClient* rtspClient, int resultCode, char* resultString)
{
  CLive555Wrapper* pWrapper = ((CWrapRTSPClient*)rtspClient)->pWrapper;
  pWrapper->SignalComplete(resultCode, resultString);
}


const char* GetLiveStepName(ELiveStep Step)
{
  switch (Step) {
  case ELS_Begin:         return "Begin";
  case ELS_Init:          return "Init";
  case ELS_Options:       return "Options";
  case ELS_Announce:      return "Announce";
  case ELS_Describe:      return "Describe";
  case ELS_Setup:         return "Setup";
  case ELS_SetupSub:      return "Setup";
  case ELS_Play:          return "Play";
  case ELS_Pause:         return "Pause";
  case ELS_Record:        return "Record";
  case ELS_Teardown:      return "Teardown";
  case ELS_SetParameter:  return "SetParameter";
  case ELS_GetParameter:  return "GetParameter";
  default: return "Error";
  };
}

ELiveChannels GetChannelFromString(const char* ChannelType)
{
  if (!ChannelType)
    return ELC_Error;
  if (!_strcmpi(ChannelType, "video"))
    return ELC_Video;
  if (!_strcmpi(ChannelType, "audio"))
    return ELC_Audio;
  if (!_strcmpi(ChannelType, "application"))
    return ELC_Metadata;
  if (!_strcmpi(ChannelType, "backchannel"))
    return ELC_AudioBack;
  return ELC_Error;
}

const char* GetStringFromChannel(ELiveChannels Channel)
{
  switch (Channel) {
  case ELC_Video: return "video";
  case ELC_Audio: return "audio";
  case ELC_Metadata: return "metadata";
  case ELC_AudioBack: return "backchannel";
  default: return "Error";
  };
}

void CLive555Wrapper::CleanupChannels()
{
  if (Session) {
    // RTSP case
    Medium::close(Session);
  } else {
    // possible RTP case
    for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
      Medium::close(i->RtcpInstance);
      Medium::close(i->RtpSource);
    };
  }
}

bool CLive555Wrapper::CreateMediaSessionBySdp(const char* sdpDescription)
{
  CLocker l(ConfigurationDoor);

  Session = MediaSession::createNew(*Environment, sdpDescription);
  if (Session == NULL) {
    WriteLog("Failed to create a MediaSession object from the SDP description: %s", Environment->getResultMsg());
    return false;
  } else if (!Session->hasSubsessions()) {
    WriteLog("This session has no media subsessions (i.e., no \"m=\" lines)");
    return false;
  };
  
  MediaSubsessionIterator iter(*Session);
  MediaSubsession* Subsession;
  while ((Subsession = iter.next()) != NULL) {
    ELiveChannels Type = GetChannelFromString(Subsession->mediumName());
    if (Type == ELC_Error)
      continue;
    if ((Type == ELC_Audio) && Subsession->getIsBackchannel()) {
      Type = ELC_AudioBack;
    };
    TChannels::iterator i = Channels.begin();
    for (; i != Channels.end(); i++) {
      if (i->Channel == Type)
        break;
    };
    if (i == Channels.end())
      continue;
    i->Subsession = Subsession;
  }
  
  for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
    if (!i->Subsession) {
      WriteLog("This session has no media subsession for %s", GetStringFromChannel(i->Channel));
      return false;
    };
  };

  for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
    if (!i->Subsession->initiate()) {
      WriteLog("Failed to initiate the %s subsession: %s", GetStringFromChannel(i->Channel), Environment->getResultMsg());
      return false;
    } else {
      const char* Text = i->Subsession->GetWarningMessage();
      if (Text && *Text) {
        WriteLog("Warning: %s", Text);
      };
      WriteLog("Initiated the %s subsession", GetStringFromChannel(i->Channel));
    };
  };

  return true;
}

bool CLive555Wrapper::CreateRtpSubsessionsByChannels()
{
  CLocker l(ConfigurationDoor);

  for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
    NetAddressList Addresses(i->Address.c_str());
    if (Addresses.numAddresses() == 0) 
      return false;
    NetAddress SessionAddress = *Addresses.firstAddress();
    const Port rtpPort(i->Port);
    const Port rtcpPort(i->Port + 1);
    Groupsock* RtpGroupsock  = new Groupsock(*Environment, SessionAddress, rtpPort,  i->TTL);
    Groupsock* RtcpGroupsock = new Groupsock(*Environment, SessionAddress, rtcpPort, i->TTL);
    if (!RtpGroupsock || !RtcpGroupsock)
      return false;

    if (i->Codec == "JPEG") {
	    i->RtpSource = JPEGVideoRTPSource::createNew(*Environment, RtpGroupsock);
    } else if (i->Codec == "MP4V-ES") {
      i->RtpSource = MPEG4ESVideoRTPSource::createNew(*Environment, RtpGroupsock, 0, 90000);
    } else if (i->Codec == "H264") {
      i->RtpSource = H264VideoRTPSource::createNew(*Environment, RtpGroupsock, 0, 90000);
    } else if (i->Codec == "PCMU") {
	    i->RtpSource = SimpleRTPSource::createNew(*Environment, RtpGroupsock, 0, 8000, "audio/PCMU", 0, False);
    } else if (i->Codec == "PCMA") {
	    i->RtpSource = SimpleRTPSource::createNew(*Environment, RtpGroupsock, 0, 8000, "audio/PCMA", 0, False);
    } else if (i->Codec == "G726") {
      i->RtpSource = SimpleRTPSource::createNew(*Environment, RtpGroupsock, 0, 8000, "audio/G726-32", 0, False);
    } else if (i->Codec == "MPEG4-GENERIC") { // AAC
      i->RtpSource = MPEG4GenericRTPSource::createNew(*Environment, RtpGroupsock, 0, 8000, "audio/MPEG4-GENERIC", "aac-hbr", 13, 3, 3);
    } else {
      return false;
    }

    unsigned char CNAME[100 + 1];
    gethostname((char*)CNAME, 100);
    CNAME[100] = '\0'; // just in case
    i->RtcpInstance = RTCPInstance::createNew(*Environment, RtcpGroupsock, 160, CNAME, NULL, i->RtpSource);
  }
  return true;
}

bool CLive555Wrapper::CreateSinks()
{
  CLocker l(ConfigurationDoor);

  for (TChannels::iterator i = Channels.begin(); i != Channels.end(); i++) {
    MediaSink* Sink = NULL;
    if (i->Subsession && i->Subsession->getIsBackchannel()) {
      BackMediaSubsession* back = (BackMediaSubsession*)i->Subsession;
      back->CreateBackSink(i->Codec, i->Address);
      Sink = back->sink;
    } else {
      Sink = CWrapSink::createNew(*Environment, RtspClient, i->Channel);
    }
    if (!Sink) {
      WriteLog("Failed to create a data sink for the %s subsession: %s", GetStringFromChannel(i->Channel), Environment->getResultMsg());
      return false;
    } else {
      WriteLog("Created a data sink for the %s subsession", GetStringFromChannel(i->Channel));
    };
    if (i->Subsession) {
      // RTSP connection
      i->Subsession->miscPtr = RtspClient; // a hack to let subsession handle functions get the "RTSPClient" from the subsession 
      // Also set a handler to be called if a RTCP "BYE" arrives for this subsession:
      if (i->Subsession->rtcpInstance() != NULL) {
        i->Subsession->rtcpInstance()->setByeHandler(SubsessionByeHandler, i->Subsession);
      }
      i->Subsession->sink = Sink;
      Sink->startPlaying(*(i->Subsession->readSource()), SubsessionAfterPlaying, i->Subsession);
    } else {
      // Pure RTP
      i->Sink = Sink;
      Sink->startPlaying(*i->RtpSource, SubsessionAfterPlayingRtp, Sink);
    }
  };
  return true;
}

bool CLive555Wrapper::SetAdditionalFields(const char* Text)
{
  CLocker l(ConfigurationDoor);

  RtspClient->setCustomFields(Text);
  return true;
}

bool CLive555Wrapper::RunCommand(ELiveStep Step)
{
  switch (Step) {
  case ELS_Options: return Options();
  //case ELS_Announce: return Announce();
  case ELS_Describe: return Describe();
  case ELS_Setup: return Setup();
  case ELS_Play: return Play();
  case ELS_Pause: return Pause();
  //case ELS_Record: return Record();
  case ELS_Teardown: return Teardown();
  //case ELS_SetParameter: return SetParameter();
  //case ELS_GetParameter: return GetParameter();
  default: return false;
  };
}

bool CLive555Wrapper::Options()
{
  if (!Prologue(ELS_Options))
    return false;

  return !!RtspClient->sendOptionsCommand(RtspHandler, Credentials); 
}

bool CLive555Wrapper::Describe()
{
  if (!Prologue(ELS_Describe))
    return false;

  return !!RtspClient->sendDescribeCommand(RtspHandler, Credentials); 
}

bool CLive555Wrapper::Setup()
{
  if (Channels.empty())
    return false;
  if (!Prologue(ELS_Setup))
    return false;
  CurrentSubStep = 0;
  return !!RtspClient->sendSetupCommand(*Channels[0].Subsession, RtspHandler, False, 
    (Transport >= ELT_TCP) && (Transport <= ELT_HTTP) ? True : False, 
    Transport == ELT_MULTICAST ? True : False, 
    Credentials);
}

bool CLive555Wrapper::SetupSub(int Channel)
{
  if (Channels.empty())
    return false;
  if ((Channel < 0) || (Channel >= Channels.size()))
    return false;
  if (!Prologue(ELS_Setup))
    return false;
  CurrentSubStep = Channel;
  return !!RtspClient->sendSetupCommand(*Channels[CurrentSubStep].Subsession, RtspHandler, False, 
    (Transport >= ELT_TCP) && (Transport <= ELT_HTTP) ? True : False, 
    Transport == ELT_MULTICAST ? True : False, 
    Credentials);
}

bool CLive555Wrapper::Play(double start)
{
  if (!Prologue(ELS_Play))
    return false;

  return !!RtspClient->sendPlayCommand(*Session, RtspHandler, start, -1.0, 1.0f, Credentials); 
}

bool CLive555Wrapper::Pause()
{
  if (!Prologue(ELS_Pause))
    return false;

  return !!RtspClient->sendPauseCommand(*Session, RtspHandler, Credentials); 
}

bool CLive555Wrapper::Teardown()
{
  if (!Prologue(ELS_Teardown))
    return false;

  return !!RtspClient->sendTeardownCommand(*Session, RtspHandler, Credentials); 
}
