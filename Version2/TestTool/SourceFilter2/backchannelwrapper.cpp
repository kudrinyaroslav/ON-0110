// wrapper.cpp : Defines the entry point for the console application.
//
#include "backchannelwrapper.h"

Boolean BackMediaSubsession::CreateBackSink(std::string const& Codec, std::string const& Address)
{
  do {
    if (Codec == "G711") {
      if (!CreateBackSink711(Address))
        break;
    }
    if (Codec == "G726") {
      if (!CreateBackSink726(Address))
        break;
    }
    if (Codec == "AAC") {
      if (!CreateBackSinkAAC(Address))
        break;
    }

    if (sink != NULL && fRTCPSocket != NULL) {
      // If bandwidth is specified, use it and add 5% for RTCP overhead.
      // Otherwise make a guess at 500 kbps.
      unsigned totSessionBandwidth
        = fBandwidth ? fBandwidth + fBandwidth / 20 : 500;
      fRTCPInstance = RTCPInstance::createNew(env(), fRTCPSocket,
        totSessionBandwidth,
        (unsigned char const*)
        fParent.CNAME(),
        NULL /* we're a client */,
        fRTPSource);
      if (fRTCPInstance == NULL) {
        env().setResultMsg("Failed to create RTCP instance");
        break;
      }
    }
    return True;
  } while (0);

  return False; // an error occurred
}

Boolean BackMediaSubsession::CreateBackSink711(std::string const& Address)
{
  WAVAudioFileSource* wavSource = WAVAudioFileSource::createNew(fParent.envir(), Address.c_str());
  if (!wavSource) {
    return False;
  }
  fReadSource = wavSource;

  char const* mimeType = "PCMU";
  unsigned char payloadFormatCode = 0;
  int sampleFrequency = 8000;
  unsigned int numChannels = 1;
  
  sink = SimpleRTPSink::createNew(fParent.envir(), fRTPSocket,
									  payloadFormatCode, sampleFrequency,
									  "audio", mimeType, numChannels);
  if (!sink) {
    Medium::close(wavSource);
    return False;
  }
  return True;
}

Boolean BackMediaSubsession::CreateBackSink726(std::string const& Address)
{

  WAVAudioFileSource* wavSource = WAVAudioFileSource::createNew(fParent.envir(), Address.c_str());
  fReadSource = wavSource;

  char const* mimeType = "G726-16";
  unsigned char payloadFormatCode = 0;
  int sampleFrequency = 8000;
  unsigned int numChannels = 1;
  
  sink = SimpleRTPSink::createNew(fParent.envir(), fRTPSocket,
									  payloadFormatCode, sampleFrequency,
									  "audio", mimeType, numChannels);
  return True;
}

Boolean BackMediaSubsession::CreateBackSinkAAC(std::string const& Address)
{
  do {
    ADTSAudioFileSource* adtsSource = ADTSAudioFileSource::createNew(fParent.envir(), Address.c_str());
    fReadSource = adtsSource;
    if(!fReadSource) {
      env().setResultMsg("BackMediaSubsession::createSinkObjects()  fReadSource==NULL\n");
      break;
    }
    sink = MPEG4GenericRTPSink::createNew(fParent.envir(), fRTPSocket,
                                                   fRTPPayloadFormat,
                                                   adtsSource->samplingFrequency(),
                                                   "audio", "aac-hbr", adtsSource->configStr(),
                                                   adtsSource->numChannels());
    return True;
  } while (0);

  return False; // an error occurred
}