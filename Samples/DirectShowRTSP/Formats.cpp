#include "Formats.h"

class MJPEGFormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(MediaSubsession *sub) { return !strcmp( sub->codecName(), "JPEG"); }
  CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  GUID AsGUID() const { return MEDIASUBTYPE_MJPG; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'M', 'J', 'P', 'G' ); };
  bool GetFormat(CMediaType *pmt, MediaSubsession *sub) const 
  { 
    VIDEOINFO *pvi = (VIDEOINFO *) pmt->AllocFormatBuffer(sizeof(VIDEOINFO));
    if(NULL == pvi) return false;
    ZeroMemory(pvi, sizeof(VIDEOINFO));

    int fps = sub->videoFPS();
    if (fps) fps = 1000 / fps;

    pvi->rcSource.left = 0;
    pvi->rcSource.top  = 0;
    pvi->rcSource.right  = sub->videoWidth();
    if (pvi->rcSource.right <= 0) pvi->rcSource.right = 320;
    pvi->rcSource.bottom = sub->videoHeight();
    if (pvi->rcSource.bottom <= 0) pvi->rcSource.bottom  = 240;
    pvi->rcTarget = pvi->rcSource;

    pvi->AvgTimePerFrame = fps;

    pvi->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
    pvi->bmiHeader.biWidth = pvi->rcSource.right;
    pvi->bmiHeader.biHeight = pvi->rcSource.bottom;
    pvi->bmiHeader.biPlanes = 1;
    pvi->bmiHeader.biBitCount = 24;
    pvi->bmiHeader.biCompression = AsFOURCC();
    pvi->bmiHeader.biSizeImage = pvi->bmiHeader.biWidth * pvi->bmiHeader.biHeight;

    pmt->SetType(&MEDIATYPE_Video);
    pmt->SetFormatType(&FORMAT_VideoInfo);
    pmt->SetTemporalCompression(TRUE);

    // Work out the GUID for the subtype from the header info.
    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);

    return true;
  };

  bool CheckFormat(const CMediaType *pmt, MediaSubsession *sub) const 
  { 
    if (*(pmt->Type()) != MEDIATYPE_Video)   // we only output video
    {                                                  
      return false;
    }

    const GUID *SubType = pmt->Subtype();
    if (SubType == NULL) return false;
    if (*SubType != AsGUID()) return false;
    return true;
  };

  FrameInfo* NextChunk(MediaSubsession *sub, unsigned char* buffer, int size) const
  {
    FrameInfo* Info = FrameInfo::Init(size);
    memcpy( Info->pdata, buffer, size );
    return Info;
  }

};

static MJPEGFormatSupporter mjpeg;


class MPEG4FormatSupporter : public GeneralFormatSupporter
{
public:
  virtual bool ProbeCodec(MediaSubsession *sub) { return !strcmp( sub->codecName(), "JPEG"); }
  virtual CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  virtual GUID AsGUID() const { return GUID_NULL; };
  virtual FOURCC AsFOURCC() const { return MAKEFOURCC( 'M', 'J', 'P', 'G' ); };
};

static MPEG4FormatSupporter mpeg4;




class H264FormatSupporter : public GeneralFormatSupporter
{
public:
  virtual bool ProbeCodec(MediaSubsession *sub) { return !strcmp( sub->codecName(), "H264"); }
  virtual CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  virtual GUID AsGUID() const { return GUID_NULL; };
  virtual FOURCC AsFOURCC() const { return MAKEFOURCC( 'h', '2', '6', '4' ); };
};

static H264FormatSupporter h264;




class G711FormatSupporter : public GeneralFormatSupporter
{
public:
  virtual bool ProbeCodec(MediaSubsession *sub) { return !strcmp( sub->codecName(), "JPEG"); }
  virtual CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  virtual GUID AsGUID() const { return GUID_NULL; };
  virtual FOURCC AsFOURCC() const { return MAKEFOURCC( 'M', 'J', 'P', 'G' ); };
};

static G711FormatSupporter g711;



class G726FormatSupporter : public GeneralFormatSupporter
{
public:
  virtual bool ProbeCodec(MediaSubsession *sub) { return !strcmp( sub->codecName(), "G726"); }
  virtual CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  virtual GUID AsGUID() const { return GUID_NULL; };
  virtual FOURCC AsFOURCC() const { return MAKEFOURCC( 'g', '7', '2', '6' ); };
};

static G726FormatSupporter g726;



class AACFormatSupporter : public GeneralFormatSupporter
{
public:
  virtual bool ProbeCodec(MediaSubsession *sub) { return !strcmp( sub->codecName(), "JPEG"); }
  virtual CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  virtual GUID AsGUID() const { return GUID_NULL; };
  virtual FOURCC AsFOURCC() const { return MAKEFOURCC( 'M', 'J', 'P', 'G' ); };
};

static AACFormatSupporter aac;












GeneralFormatSupporter *SupportedFormats[] =
{
  // core ONVIF formats
  &mjpeg,
  &mpeg4,
  &h264,
  &g711,
  &g726,
  &aac
  // maybe others
};

GeneralFormatSupporter *ProbeByCodec(MediaSubsession *sub)
{
  for (int i = 0 ; i < sizeof(SupportedFormats) / sizeof(SupportedFormats[0]); i++)
  {
    if (SupportedFormats[i]->ProbeCodec(sub))
    {
      return SupportedFormats[i];
    }
  }
  return NULL;
}

