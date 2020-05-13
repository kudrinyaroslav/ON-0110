// playbacksupport.cpp
//

#include "playbacksupport.h"

void PlaybackFrameLog::RtpExtHdrCallback(u_int16_t profile, u_int16_t seq, u_int16_t len, u_int8_t* pHdrData, void* pPriv)
{
  if (pPriv) {
    ((PlaybackFrameLog*)pPriv)->RtpExtHdrCallback1(profile, seq, len, pHdrData);
  }
}

void PlaybackFrameLog::RtpExtHdrCallback1(u_int16_t profile, u_int16_t seq, u_int16_t len, u_int8_t* pHdrData)
{
  RtpReplayExtension packet;

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

  Frames.push_back(packet);

  char Buffer[256];
  packet.ToString(Buffer);
  Log.AddLogText(Buffer);
}

void RtpReplayExtension::ToString(char* Buffer, bool canSkip)
{
  sprintf(Buffer, "%u:%u.%u.%u.%u.%u.%u.%u.%u.%u.%u.%u.%u.%llu", 
              payload, dateTime, secFrac, bitC, bitE, bitD, CSeq, seq, canSkip ? 1 : 0, 
              startsFrame, endsFrame, validLength, timestamp, creationTime);
}


bool PlaybackFrameLog::GetText(char* Buffer, int Record)
{
  if (!Buffer) {
    return false;
  }
  if ((Record < 0) || Record >= Frames.size()) {
    Buffer[0] = 0;
    return false;
  };
  Frames[Record].ToString(Buffer);
}
