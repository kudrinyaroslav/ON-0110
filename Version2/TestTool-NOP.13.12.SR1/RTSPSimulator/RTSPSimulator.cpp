#include "liveMedia.hh"
#include "BasicUsageEnvironment.hh"
#include "SJPEGVideoServerMediaSubsession.h"

#define NUM_CODECS 6
#define NUM_STREAMS_PER_CODECS 10

struct SStream {
  char cURL[MAX_PATH];
  char cName[MAX_PATH];
  char cFile[MAX_PATH];
};

struct SStreams {
  SStream Streams[NUM_STREAMS_PER_CODECS];
  int StreamCnt;
  SStreams() { StreamCnt = 0; }
};

static SStreams sampleFiles[NUM_CODECS];
static char lastError[256] = {'\0'};

static int StreamCnt = 0;
static char StopFlag = 1;
static char Started = 0;
static char RtspOverHttp = 0;

// To make the second and subsequent client for each stream reuse the same
// input stream as the first client (rather than playing the file from the
// start for each client), change the following "False" to "True":
Boolean reuseFirstSource = False;

enum ECodecs
{
    EC_JPEG  = 0,
    EC_MPEG4 = 1,
    EC_H264  = 2,
    EC_G711  = 3,
    EC_G726  = 4,
    EC_AAC   = 5
};

BOOL __stdcall DllMain(HINSTANCE hInst, DWORD dwReason, LPVOID lpReserved) {
  return TRUE;
}

static void announceStream(RTSPServer *rtspServer, ServerMediaSession *sms, char const *streamName, char const *inputFileName, ECodecs codec)
{
  char *url = rtspServer->rtspURL(sms);

  for (int i = 0; i < sampleFiles[codec].StreamCnt; ++i) {
    if (0 == strcmp(sampleFiles[codec].Streams[i].cName, streamName)) {
      memcpy(sampleFiles[codec].Streams[i].cURL, url, strlen(url));
      sampleFiles[codec].Streams[i].cURL[strlen(url)] = '\0'; 
      break;
    }
  }

  UsageEnvironment& env = rtspServer->envir();
  env << "\n\"" << streamName << "\" stream, from the file \"" << inputFileName << "\"\n";
  env << "Play this stream using the URL \"" << url << "\"\n";
  delete[] url;
}

extern "C" __declspec(dllexport) char const* __stdcall GetRtspUrl(int codec, char const *name) 
{
  if (codec >= 0 && codec < NUM_CODECS) {
    for (int i = 0; i < sampleFiles[codec].StreamCnt; ++i) {
      if (0 == strcmp(sampleFiles[codec].Streams[i].cName, name)) {
        return sampleFiles[codec].Streams[i].cURL;
      }
    }
  }
  return "";
}

extern "C" __declspec(dllexport) int __stdcall IsStarted()
{
  return (int)Started;
}

extern "C" __declspec(dllexport) int __stdcall IsRtspOverHttp()
{
  return (int)RtspOverHttp;
}

extern "C" __declspec(dllexport) void __stdcall Stop()
{
  StopFlag = 1;
}

extern "C" __declspec(dllexport) void __stdcall AddSampleFile(int codec, char const *file, char const *name)
{
  if (codec >= 0 && codec < NUM_CODECS) {
    if (sampleFiles[codec].StreamCnt < NUM_STREAMS_PER_CODECS) {
      SStream &Stream = sampleFiles[codec].Streams[++(sampleFiles[codec].StreamCnt) - 1];

      memcpy(Stream.cFile, file, strlen(file));
      Stream.cFile[strlen(file)] = '\0'; 

      memcpy(Stream.cName, name, strlen(name));
      Stream.cName[strlen(name)] = '\0'; 

      Stream.cURL[0] = '\0';
    }
  }
}

extern "C" __declspec(dllexport) char* __stdcall GetLasErrorMsg()
{
  return lastError;
}

extern "C" __declspec(dllexport) void __stdcall Start(unsigned int codecs, int rtspPort, int httpPort)
{
  StopFlag = 0;
  Started = 0;
  RtspOverHttp = 0;

  for (int j = 0; j < NUM_CODECS; ++j) {
    for (int i = 0; i < sampleFiles[j].StreamCnt; ++i) {
      sampleFiles[j].Streams[i].cURL[0] = '\0';
    }
  }

  // Begin by setting up our usage environment:
  TaskScheduler *scheduler = BasicTaskScheduler::createNew();
  UsageEnvironment *env = BasicUsageEnvironment::createNew(*scheduler);

  UserAuthenticationDatabase *authDB = NULL;
#ifdef ACCESS_CONTROL
  // To implement client access control to the RTSP server, do the following:
  authDB = new UserAuthenticationDatabase;
  authDB->addUserRecord("username1", "password1"); // replace these with real strings
  // Repeat the above with each <username>, <password> that you wish to allow
  // access to the server.
#endif

  // Create the RTSP server:
  RTSPServer *rtspServer = RTSPServer::createNew(*env, rtspPort, authDB);
  if (rtspServer == NULL) {
    *env << "Failed to create RTSP server: " << env->getResultMsg() << "\n";
    exit(1);
  }

  char const *descriptionString = "Session streamed by \"testOnDemandRTSPServer\"";

  // Set up each of the possible streams that can be served by the
  // RTSP server.  Each such stream is implemented using a
  // "ServerMediaSession" object, plus one or more
  // "ServerMediaSubsession" objects for each audio/video substream.

  int codecCheck;
  
  // A JPEG video elementary stream:
  codecCheck = EC_JPEG;
  if (((1 << codecCheck) & codecs) && (sampleFiles[codecCheck].StreamCnt > 0)) {
    for (int i = 0; i < sampleFiles[codecCheck].StreamCnt; ++i) {
      char const *streamName = sampleFiles[codecCheck].Streams[i].cName;
      char const *inputFileName = sampleFiles[codecCheck].Streams[i].cFile;
      ServerMediaSession* sms = ServerMediaSession::createNew(*env, streamName, streamName, descriptionString);
      sms->addSubsession(WISJPEGVideoServerMediaSubsession::createNew(*env, inputFileName, reuseFirstSource));
      rtspServer->addServerMediaSession(sms);

      announceStream(rtspServer, sms, streamName, inputFileName, EC_JPEG);
    }
  }//*/

  // A MPEG-4 video elementary stream:
  codecCheck = EC_MPEG4;
  if (((1 << codecCheck) & codecs) && (sampleFiles[codecCheck].StreamCnt > 0)) {
    for (int i = 0; i < sampleFiles[codecCheck].StreamCnt; ++i) {
      char const *streamName = sampleFiles[codecCheck].Streams[i].cName;
      char const *inputFileName = sampleFiles[codecCheck].Streams[i].cFile;
      ServerMediaSession* sms = ServerMediaSession::createNew(*env, streamName, streamName, descriptionString);
      sms->addSubsession(MPEG4VideoFileServerMediaSubsession::createNew(*env, inputFileName, reuseFirstSource));
      rtspServer->addServerMediaSession(sms);

      announceStream(rtspServer, sms, streamName, inputFileName, EC_MPEG4);
    }
  }//*/

  // A H.264 video elementary stream:
  codecCheck = EC_H264;
  if (((1 << codecCheck) & codecs) && (sampleFiles[codecCheck].StreamCnt > 0)) {
    for (int i = 0; i < sampleFiles[codecCheck].StreamCnt; ++i) {
      char const *streamName = sampleFiles[codecCheck].Streams[i].cName;
      char const *inputFileName = sampleFiles[codecCheck].Streams[i].cFile;
      ServerMediaSession* sms = ServerMediaSession::createNew(*env, streamName, streamName, descriptionString);
      sms->addSubsession(H264VideoFileServerMediaSubsession::createNew(*env, inputFileName, reuseFirstSource));
      rtspServer->addServerMediaSession(sms);

      announceStream(rtspServer, sms, streamName, inputFileName, EC_H264);
    }
  }//*/

  // A WAV (G711) audio stream:
  codecCheck = EC_G711;
  if (((1 << codecCheck) & codecs) && (sampleFiles[codecCheck].StreamCnt > 0)) {
    for (int i = 0; i < sampleFiles[codecCheck].StreamCnt; ++i) {
      char const *streamName = sampleFiles[codecCheck].Streams[i].cName;
      char const *inputFileName = sampleFiles[codecCheck].Streams[i].cFile;
      ServerMediaSession* sms = ServerMediaSession::createNew(*env, streamName, streamName, descriptionString);
      // To convert 16-bit PCM data to 8-bit u-law, prior to streaming,
      // change the following to True:
      Boolean convertToULaw = False;
      sms->addSubsession(WAVAudioFileServerMediaSubsession::createNew(*env, inputFileName, reuseFirstSource, convertToULaw));
      rtspServer->addServerMediaSession(sms);

      announceStream(rtspServer, sms, streamName, inputFileName, EC_G711);
    }
  }//*/

  // A WAV (G726) audio stream:
  codecCheck = EC_G726;
  if (((1 << codecCheck) & codecs) && (sampleFiles[codecCheck].StreamCnt > 0)) {
    for (int i = 0; i < sampleFiles[codecCheck].StreamCnt; ++i) {
      char const *streamName = sampleFiles[codecCheck].Streams[i].cName;
      char const *inputFileName = sampleFiles[codecCheck].Streams[i].cFile;
      ServerMediaSession* sms = ServerMediaSession::createNew(*env, streamName, streamName, descriptionString);
      // To convert 16-bit PCM data to 8-bit u-law, prior to streaming,
      // change the following to True:
      Boolean convertToULaw = False;
      sms->addSubsession(WAVAudioFileServerMediaSubsession::createNew(*env, inputFileName, reuseFirstSource, convertToULaw));
      rtspServer->addServerMediaSession(sms);

      announceStream(rtspServer, sms, streamName, inputFileName, EC_G726);
    }
  }//*/

  // An AAC audio stream (ADTS-format file):
  codecCheck = EC_AAC;
  if (((1 << codecCheck) & codecs) && (sampleFiles[codecCheck].StreamCnt > 0)) {
    for (int i = 0; i < sampleFiles[codecCheck].StreamCnt; ++i) {
      char const *streamName = sampleFiles[codecCheck].Streams[i].cName;
      char const *inputFileName = sampleFiles[codecCheck].Streams[i].cFile;
      ServerMediaSession *sms = ServerMediaSession::createNew(*env, streamName, streamName, descriptionString);
      sms->addSubsession(ADTSAudioFileServerMediaSubsession::createNew(*env, inputFileName, reuseFirstSource));
      rtspServer->addServerMediaSession(sms);

      announceStream(rtspServer, sms, streamName, inputFileName, EC_AAC);
    }
  }//*/

  // Create a HTTP server for RTSP-over-HTTP tunneling.
  if (httpPort > 0) {
    if (rtspServer->setUpTunnelingOverHTTP(httpPort)) {
      *env << "\n(We use port " << rtspServer->httpServerPortNum() << " for optional RTSP-over-HTTP tunneling.)\n";
      RtspOverHttp = 1;
    } else {
      *env << "\n(RTSP-over-HTTP tunneling is not available.)\n";
    }
  }

  Started = 1;

  env->taskScheduler().doEventLoop(&StopFlag); // does not return

  RTSPServer::close(rtspServer);
}
