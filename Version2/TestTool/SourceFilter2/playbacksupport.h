// playbacksupport.h
//

#include "RTPSource.hh"
#include "LiveCapsule.h"
#include <vector>

struct RtpReplayExtension
{
  u_int64_t creationTime;
  u_int32_t dateTime;
  u_int32_t secFrac;
  u_int8_t bitC;
  u_int8_t bitE;
  u_int8_t bitD;
  u_int8_t CSeq;
  u_short seq;
  u_int8_t startsFrame;
  u_int8_t endsFrame;
  u_int8_t validLength;
  u_int32_t timestamp;
  u_int8_t payload;

  void ToString(char* Buffer, bool canSkip = true);
};

class PlaybackFrameLog {
  std::vector<RtpReplayExtension> Frames;
  CStepLog& Log;
public:
  PlaybackFrameLog(CStepLog& Log) : Log(Log) {};
  static void RtpExtHdrCallback(u_int16_t profile, u_int16_t seq, u_int16_t len, u_int8_t* pHdrData, void* pPriv);
  int GetRecords() const { return Frames.size(); };
  bool GetText(char* Buffer, int Record);
private:
  void RtpExtHdrCallback1(u_int16_t profile, u_int16_t seq, u_int16_t len, u_int8_t* pHdrData);
};
