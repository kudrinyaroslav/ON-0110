// backchannelwrapper.h
//

#include "liveMedia.hh"
#include "BasicUsageEnvironment.hh"
#include "DigestAuthentication.hh"
#include <string>

class BackMediaSubsession : public MediaSubsession
{
public:
  Boolean CreateBackSink(std::string const& Codec, std::string const& Address);
private:
  Boolean CreateBackSink711(std::string const& Address);
  Boolean CreateBackSink726(std::string const& Address);
  Boolean CreateBackSinkAAC(std::string const& Address);
};
