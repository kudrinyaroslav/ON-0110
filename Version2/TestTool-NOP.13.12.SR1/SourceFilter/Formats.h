///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include <vector>
#include <streams.h>
#include <strsafe.h>

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
  CODEC_TYPE_META
  /*CODEC_TYPE_DATA,
  CODEC_TYPE_SUBTITLE,
  CODEC_TYPE_ATTACHMENT,
  CODEC_TYPE_NB*/
};

enum RTSPClientState {
  RTSP_STATE_IDLE,    /**< not initialized */
  RTSP_STATE_OPENED,
  RTSP_STATE_PLAYING, /**< initialized and receiving data */
  RTSP_STATE_PAUSED,  /**< initialized, but not receiving data */
  RTSP_STATE_STOPPED, // after TEARDOWN
};


struct FrameHeader
{
  //long TimeStamp;
  REFERENCE_TIME Start;
  long FrameType;
  long FrameLen;
  bool InterFrame;
};


struct FrameInfo
{
  FrameHeader frameHead;
  long UseCounter;
  unsigned char* pdata;
  static FrameInfo* Init(int size)
  {
    int asize = offsetof(FrameInfo, pdata) + sizeof(unsigned char*) + size + 3;

    //FrameInfo *p = (FrameInfo *)VirtualAlloc(NULL, asize, MEM_COMMIT, PAGE_READWRITE);
    FrameInfo *p = (FrameInfo *)malloc(asize);
    p->pdata   = ((unsigned char*)p) + offsetof(FrameInfo, pdata) + sizeof(unsigned char*);
    if (((size_t)p->pdata) & 3) {
      OutputDebugStringA("Unaligned memory! why?\n");
      p->pdata = (unsigned char*)((((size_t)p->pdata) + 3) & (~3));
    }
    p->frameHead.FrameLen = size;
    p->frameHead.FrameType = 0;
    p->frameHead.InterFrame = false;
    p->UseCounter = 1;
    return p;
  }
  static void Release(FrameInfo *p)
  {
    if (!p) return;
    unsigned char *pp = ((unsigned char*)p) + offsetof(FrameInfo, pdata) + sizeof(unsigned char*);
    pp = (unsigned char *)((((size_t)pp) + 3) & (~3));
    if (p->pdata != pp)
    {
      OutputDebugStringA("Releasing external memory! why?\n");
      free(p->pdata);
    }
    free(p);
    //VirtualFree(p, 0, MEM_RELEASE);
  }
};

// better to use boost::intrusiveptr later
class FrameInfoPtr
{
  FrameInfo *pInfo;
public:
  FrameInfoPtr() { pInfo = NULL;};
  FrameInfoPtr(FrameInfo *p);
  FrameInfoPtr(const FrameInfoPtr &p);
  ~FrameInfoPtr();
  operator FrameInfo *();
  void Release();
  void Unlink();
};

class IMediaSubsession
{
public:
  virtual ~IMediaSubsession() {}
  virtual unsigned short   videoWidth()  const = 0;
  virtual unsigned short   videoHeight() const = 0;
  virtual unsigned         videoFPS()    const = 0;
  virtual unsigned         numChannels() const = 0;
  virtual char const*      mediumName()  const = 0;
  virtual char const*      codecName()   const = 0;
  virtual char const*      fmtp_config() const = 0;
  virtual char const*      fmtp_spropparametersets() const = 0;
  virtual unsigned         rtpTimestampFrequency()   const = 0;
  virtual RTPSource*       rtpSource() = 0;
  virtual Boolean          initiate(int useSpecialRTPoffset = -1) = 0;
  virtual MediaSubsession* mediaSub() = 0;
  virtual RTPSource*       readSource() { return rtpSource(); }
};

class GeneralFormatSupporter
{
  virtual BYTE* CreatePrefixAndInfo(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const;
  bool GetFormatVideo(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const;

public:
  virtual bool ProbeCodec(IMediaSubsession *sub) { return false; }
  virtual CodecType AsType() const { return CODEC_TYPE_UNKNOWN; }
  virtual GUID AsGUID() const { return GUID_NULL; };
  virtual FOURCC AsFOURCC() const { return MAKEFOURCC('u','n','d','f'); };
  virtual bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const;
  virtual bool CheckFormat(const CMediaType *pmt, IMediaSubsession *sub) const;
  virtual FrameInfo* NextChunk(IMediaSubsession *sub, unsigned char* buffer, int size, FrameInfo *Prefix) const;
};

GeneralFormatSupporter *ProbeByCodec(IMediaSubsession *sub);

void ParseH264SPS(unsigned char *buffer, unsigned size, FramedSource *readSource);
void ParseMPEG4VOL(unsigned char *buffer, unsigned size, FramedSource *readSource);

