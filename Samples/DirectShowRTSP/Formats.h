#include <vector>
#include <streams.h>

#include "UsageEnvironment.hh"
#include "BasicUsageEnvironment.hh"
#include "GroupsockHelper.hh"
#include "liveMedia.hh"


#define AppName "ONVIF Filter"

#ifndef elementsof
#define elementsof(a) (sizeof(a) / sizeof((a)[0]))
#endif

#ifndef err
#define err (void)
//#define err(text) {MessageBox(NULL, TEXT(text), L"Info O2", MB_OK | MB_TOPMOST);}
#endif


enum CodecType {
  CODEC_TYPE_UNKNOWN = -1,
  CODEC_TYPE_VIDEO,
  CODEC_TYPE_AUDIO,
  CODEC_TYPE_DATA,
  CODEC_TYPE_SUBTITLE,
  CODEC_TYPE_ATTACHMENT,
  CODEC_TYPE_NB
};

enum RTSPClientState {
  RTSP_STATE_IDLE,    /**< not initialized */
  RTSP_STATE_OPENED,
  RTSP_STATE_PLAYING, /**< initialized and receiving data */
  RTSP_STATE_PAUSED,  /**< initialized, but not receiving data */
};


struct FrameHeader
{
  //long TimeStamp;
  REFERENCE_TIME Start;
  long FrameType;
  long FrameLen;
};


struct FrameInfo
{
  FrameHeader frameHead;
  unsigned char* pdata;
  static FrameInfo* Init(int size)
  {
    FrameInfo *p = (FrameInfo *)malloc(offsetof(FrameInfo, pdata) + sizeof(unsigned char*) + size);
    p->pdata   = ((unsigned char*)p) + offsetof(FrameInfo, pdata) + sizeof(unsigned char*);
    p->frameHead.FrameLen = size;
    return p;
  }
  static void Release(FrameInfo *p)
  {
    if (p->pdata != ((unsigned char*)p) + offsetof(FrameInfo, pdata) + sizeof(unsigned char*))
    {
      free(p->pdata);
    }
    free(p);
  }
};



class GeneralFormatSupporter
{
public:
  virtual bool ProbeCodec(MediaSubsession *sub) { return false; }
  virtual CodecType AsType() const { return CODEC_TYPE_UNKNOWN; }
  virtual GUID AsGUID() const { return GUID_NULL; };
  virtual FOURCC AsFOURCC() const { return MAKEFOURCC('u','n','d','f'); };
  virtual bool GetFormat(CMediaType *pmt, MediaSubsession *sub) const { return false; };
  virtual bool CheckFormat(const CMediaType *pmt, MediaSubsession *sub) const { return false; };
  virtual FrameInfo* NextChunk(MediaSubsession *sub, unsigned char* buffer, int size) const { return NULL; };
};

GeneralFormatSupporter *ProbeByCodec(MediaSubsession *sub);
