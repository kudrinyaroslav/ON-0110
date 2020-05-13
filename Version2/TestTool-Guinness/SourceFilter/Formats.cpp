///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include "Formats.h"
#include <initguid.h>
#include <Mmreg.h>

void DropDebug(const char* Format, ...);

// HACK begin
extern int AssumeWidth;
extern int AssumeHeight;
// HACK end

//7634706D-0000-0010-8000-00AA00389B71 mp4v
DEFINE_GUID(MEDIASUBTYPE_mpg4SUB ,
0x7634706D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

//77616c75-0000-0010-8000-00AA00389B71 ulaw
DEFINE_GUID(MEDIASUBTYPE_g711uSUB ,
0x77616c75, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

//77616c75-0000-0010-8000-00AA00389B71 alaw
DEFINE_GUID(MEDIASUBTYPE_g711aSUB ,
0x77616c61, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);


//00000045-0000-0010-8000-00AA00389B71  g726
DEFINE_GUID(MEDIASUBTYPE_g726SUB ,
0x00000045, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
//0x36323767, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

//7432706D-0000-0010-8000-00AA00389B71  mp2t
DEFINE_GUID(MEDIASUBTYPE_mp2tSUB ,
0x7432706D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

DEFINE_GUID(MEDIASUBTYPE_AAC ,
0x000000FF, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

// e06d8026-db46-11cf-b4d1-00805f6cbbea
DEFINE_GUID(MEDIASUBTYPE_MPEG2_VIDEO,
0xe06d8026, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

// 00000050-0000-0010-8000-00AA00389B71
DEFINE_GUID(MEDIASUBTYPE_MPEG1AudioPayload,
0x00000050, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);

BYTE* GeneralFormatSupporter::CreatePrefixAndInfo(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const
{
  int Size = 0;
  switch (AsType())
  {
  case CODEC_TYPE_VIDEO:
    Size = sizeof(VIDEOINFO);
    break;
  case CODEC_TYPE_AUDIO:
    Size = sizeof(WAVEFORMATEX);
    break;
  default: return NULL;
  }
  //if (!Size) return NULL;
  BYTE* Buffer = pmt->AllocFormatBuffer(Size);
  if (NULL != Buffer) ZeroMemory(Buffer, Size);
  return Buffer;
}

bool GeneralFormatSupporter::GetFormatVideo(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
{ 
  VIDEOINFO *pvi = (VIDEOINFO *) CreatePrefixAndInfo(pmt, sub, Prefix);
  if (NULL == pvi) return false;

  int fps = sub->videoFPS();
  if (!fps) fps = 25;

  pvi->rcSource.left = 0;
  pvi->rcSource.top  = 0;
  pvi->rcSource.right  = sub->videoWidth();
  if (pvi->rcSource.right <= 0) pvi->rcSource.right = AssumeWidth;
  if (pvi->rcSource.right <= 0) pvi->rcSource.right = 320;
  AssumeWidth = pvi->rcSource.right;
  pvi->rcSource.bottom = sub->videoHeight();
  if (pvi->rcSource.bottom <= 0) pvi->rcSource.bottom  = AssumeHeight;
  if (pvi->rcSource.bottom <= 0) pvi->rcSource.bottom  = 240;
  AssumeHeight = pvi->rcSource.bottom;
  pvi->rcTarget = pvi->rcSource;

  DropDebug("detected fps(%i), width(%i), height(%i)", sub->videoFPS(), sub->videoWidth(), sub->videoHeight());
  DropDebug("used     fps(%i), width(%i), height(%i)", fps, pvi->rcSource.right, pvi->rcSource.bottom);

  pvi->AvgTimePerFrame = UNITS / fps;

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

  const GUID SubTypeGUID = AsGUID();
  pmt->SetSubtype(&SubTypeGUID);
  pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);

  return true;
};

bool GeneralFormatSupporter::GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
{ 
  if (AsType() != CODEC_TYPE_VIDEO) return false;
  return GetFormatVideo(pmt, sub, Prefix);
}

bool GeneralFormatSupporter::CheckFormat(const CMediaType *pmt, IMediaSubsession *sub) const 
{ 
  switch (AsType())
  {
  case CODEC_TYPE_VIDEO:
    if (*(pmt->Type()) != MEDIATYPE_Video) return false;
    break;
  case CODEC_TYPE_AUDIO:
    if (*(pmt->Type()) != MEDIATYPE_Audio) return false;
    break;
  case CODEC_TYPE_META:
    if (*(pmt->Type()) != MEDIATYPE_Video) return false;
    break;
  default: return false;
  }

  const GUID *SubType = pmt->Subtype();
  if (SubType == NULL) return false;
  if (*SubType != AsGUID()) return false;
  return true;
};

FrameInfo* GeneralFormatSupporter::NextChunk(IMediaSubsession *sub, unsigned char* buffer, int size, FrameInfo *Prefix) const
{
  FrameInfo* Info = FrameInfo::Init(size);
  memcpy( Info->pdata, buffer, size );
  return Info;
}


class MJPEGFormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) {   OutputDebugStringA(sub->codecName());
return !strcmp( sub->codecName(), "JPEG"); }
  CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  GUID AsGUID() const { return MEDIASUBTYPE_MJPG; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'M', 'J', 'P', 'G' ); };
  FrameInfo* NextChunk(IMediaSubsession *sub, unsigned char* buffer, int size, FrameInfo *Prefix) const
  {
    FrameInfo* Info = FrameInfo::Init(size);
    memcpy( Info->pdata, buffer, size );
    if (1) // we will use it if there is a bug on ffdshow side
    {
      if (*((unsigned*)(Info->pdata + 6)) == 0x4649464a)
      {
        unsigned char c = Info->pdata[15];
        Info->pdata[15] = Info->pdata[17];
        Info->pdata[17] = c;
      }
    }
    return Info;
  }
};

static MJPEGFormatSupporter mjpeg;


class MPEG4FormatSupporter : public GeneralFormatSupporter
{
  BYTE* CreatePrefixAndInfo(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const
  {
    unsigned int ExtraSize;
    unsigned char *Extra = parseGeneralConfigStr( 
      sub->fmtp_config(), 
      ExtraSize);

    if (Prefix)
    {
      *Prefix = FrameInfo::Init(ExtraSize);
      memcpy((*Prefix)->pdata, Extra, ExtraSize);
      (*Prefix)->frameHead.FrameType = 0;
    }

    BYTE *pvi = pmt->AllocFormatBuffer(sizeof(VIDEOINFO) + ExtraSize);
    if (NULL == pvi) return NULL;
    ZeroMemory(pvi, sizeof(VIDEOINFO));
    memcpy(pvi + sizeof(VIDEOINFO), Extra, ExtraSize);
    delete[] Extra;
    
    return pvi;
  }
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strncmp( sub->codecName(), "MP4V-ES", 7); }
  CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  GUID AsGUID() const { return MEDIASUBTYPE_mpg4SUB; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'm', 'p', '4', 'v' ); };

  FrameInfo* NextChunk(IMediaSubsession *sub, unsigned char* buffer, int size, FrameInfo *Prefix) const
  {
    FrameInfo* Info;

    if (Prefix->frameHead.FrameType)
    {
      Info = FrameInfo::Init(size);
      memcpy( Info->pdata, buffer, size );
    }
    else
    {
      Info = FrameInfo::Init(size + Prefix->frameHead.FrameLen);
      memcpy( Info->pdata, Prefix->pdata, Prefix->frameHead.FrameLen );
      memcpy( Info->pdata + Prefix->frameHead.FrameLen, buffer, size );
      Prefix->frameHead.FrameType = 1;
    }
    if ((buffer[3] & 0x0f) != 0)
    {
      Info->frameHead.InterFrame = true;
    }
    return Info;
  }
};

static MPEG4FormatSupporter mpeg4;




class H264FormatSupporter : public GeneralFormatSupporter
{
  static int b64_decode( char *dest, char *src );
  static unsigned char* parseH264ConfigStr( char const* configStr,
    unsigned int& configSize );
  BYTE* CreatePrefixAndInfo(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const
  {
    unsigned int ExtraSize;
    unsigned char *Extra = parseH264ConfigStr( 
      sub->fmtp_spropparametersets(), 
      ExtraSize);

    if (Prefix)
    {
      *Prefix = FrameInfo::Init(ExtraSize);
      memcpy((*Prefix)->pdata, Extra, ExtraSize);
      //(*Prefix)->frameHead.FrameType = 0;
    }

    BYTE *pvi = pmt->AllocFormatBuffer(sizeof(VIDEOINFO) + ExtraSize);
    if (NULL == pvi) return NULL;
    ZeroMemory(pvi, sizeof(VIDEOINFO));
    memcpy(pvi + sizeof(VIDEOINFO), Extra, ExtraSize);
    delete[] Extra;

    return pvi;
  }
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strcmp( sub->codecName(), "H264"); }
  CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  GUID AsGUID() const { return MEDIASUBTYPE_H264; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'h', '2', '6', '4' ); };

  FrameInfo* NextChunk(IMediaSubsession *sub, unsigned char* buffer, int size, FrameInfo *Prefix) const
  {
    FrameInfo* Info;
    /*if ((buffer[0] & 31) == 6) // SEI nal
    {
      OutputDebugStringA("Skipping SEI NAL type\n");
      return NULL;
    }*/
    if (Prefix->frameHead.FrameType)
    {
      Info = FrameInfo::Init(size + 4);
      Info->pdata[0] = 0x00;
      Info->pdata[1] = 0x00;
      Info->pdata[2] = 0x00;
      Info->pdata[3] = 0x01;
      memcpy( Info->pdata + 4, buffer, size );
    }
    else
    {
      Info = FrameInfo::Init(size + 4 + Prefix->frameHead.FrameLen);
      memcpy( Info->pdata, Prefix->pdata, Prefix->frameHead.FrameLen );
      Info->pdata[Prefix->frameHead.FrameLen + 0] = 0x00;
      Info->pdata[Prefix->frameHead.FrameLen + 1] = 0x00;
      Info->pdata[Prefix->frameHead.FrameLen + 2] = 0x00;
      Info->pdata[Prefix->frameHead.FrameLen + 3] = 0x01;
      memcpy( Info->pdata + 4 + Prefix->frameHead.FrameLen, buffer, size );
      Prefix->frameHead.FrameType = 1;
    }
    if ((buffer[0] & 31) != 5)
    {
      Info->frameHead.InterFrame = true;
    }
    return Info;
  }

};

/*char b64[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";*/
int H264FormatSupporter::b64_decode( char *dest, char *src )
{
  const char *dest_start = dest;
  int  i_level;
  int  last = 0;
  int  b64[256] = {
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* 00-0F */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* 10-1F */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,62,-1,-1,-1,63,  /* 20-2F */
    52,53,54,55,56,57,58,59,60,61,-1,-1,-1,-1,-1,-1,  /* 30-3F */
    -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,  /* 40-4F */
    15,16,17,18,19,20,21,22,23,24,25,-1,-1,-1,-1,-1,  /* 50-5F */
    -1,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,  /* 60-6F */
    41,42,43,44,45,46,47,48,49,50,51,-1,-1,-1,-1,-1,  /* 70-7F */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* 80-8F */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* 90-9F */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* A0-AF */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* B0-BF */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* C0-CF */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* D0-DF */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,  /* E0-EF */
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1   /* F0-FF */
  };

  for( i_level = 0; *src != '\0'; src++ )
  {
    int  c;

    c = b64[(unsigned int)*src];
    if( c == -1 )
    {
      continue;
    }

    switch( i_level )
    {
    case 0:
      i_level++;
      break;
    case 1:
      *dest++ = ( last << 2 ) | ( ( c >> 4)&0x03 );
      i_level++;
      break;
    case 2:
      *dest++ = ( ( last << 4 )&0xf0 ) | ( ( c >> 2 )&0x0f );
      i_level++;
      break;
    case 3:
      *dest++ = ( ( last &0x03 ) << 6 ) | c;
      i_level = 0;
    }
    last = c;
  }

  *dest = '\0';

  return dest - dest_start;
}



unsigned char* H264FormatSupporter::parseH264ConfigStr( char const* configStr,
                                         unsigned int& configSize )
{
  char *dup, *psz;
  int i, i_records = 1;

  if( configSize )
    configSize = 0;

  if( configStr == NULL || *configStr == '\0' )
    return NULL;

  psz = dup = strdup( configStr );

  /* Count the number of comma's */
  for( psz = dup; *psz != '\0'; ++psz )
  {
    if( *psz == ',')
    {
      ++i_records;
      *psz = '\0';
    }
  }

  unsigned char *cfg = new unsigned char[5 * strlen(dup)];
  psz = dup;
  for( i = 0; i < i_records; i++ )
  {
    cfg[configSize++] = 0x00;
    cfg[configSize++] = 0x00;
    cfg[configSize++] = 0x00;
    cfg[configSize++] = 0x01;

    configSize += b64_decode( (char*)&cfg[configSize], psz );
    psz += strlen(psz)+1;
  }

  free( dup );
  return cfg;
}



static H264FormatSupporter h264;



class G711uFormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strcmp( sub->codecName(), "PCMU"); }
  CodecType AsType() const { return CODEC_TYPE_AUDIO; }
  GUID AsGUID() const { return MEDIASUBTYPE_g711uSUB; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'u', 'l', 'a', 'w' ); };
  bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
  { 
    //AUDIODESCRIPTION
    WAVEFORMATEX *wfe = (WAVEFORMATEX *) pmt->AllocFormatBuffer(sizeof(WAVEFORMATEX));
    if(NULL == wfe) return false;
    ZeroMemory(wfe, sizeof(WAVEFORMATEX));

    wfe->nSamplesPerSec = 8000; 
    wfe->wFormatTag = 0x07;

    wfe->nChannels = 1; 
    wfe->wBitsPerSample = 8; 
    wfe->cbSize	= 0;
    wfe->nBlockAlign = 1;
    wfe->nAvgBytesPerSec = wfe->nSamplesPerSec;

    pmt->SetType(&MEDIATYPE_Audio);
    pmt->SetFormatType(&FORMAT_WaveFormatEx);
    pmt->SetTemporalCompression(FALSE);

    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(1);

    return true;
  };
};

static G711uFormatSupporter g711u;

class G711aFormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strcmp( sub->codecName(), "PCMA"); }
  CodecType AsType() const { return CODEC_TYPE_AUDIO; }
  GUID AsGUID() const { return MEDIASUBTYPE_g711aSUB; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'a', 'l', 'a', 'w' ); };
  bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
  { 
    //AUDIODESCRIPTION
    WAVEFORMATEX *wfe = (WAVEFORMATEX *) pmt->AllocFormatBuffer(sizeof(WAVEFORMATEX));
    if(NULL == wfe) return false;
    ZeroMemory(wfe, sizeof(WAVEFORMATEX));

    wfe->nSamplesPerSec = 8000; 
    wfe->wFormatTag = 0x06;

    wfe->nChannels = 1; 
    wfe->wBitsPerSample = 8; 
    wfe->cbSize	= 0;
    wfe->nBlockAlign = 1;
    wfe->nAvgBytesPerSec = wfe->nSamplesPerSec;

    pmt->SetType(&MEDIATYPE_Audio);
    pmt->SetFormatType(&FORMAT_WaveFormatEx);
    pmt->SetTemporalCompression(FALSE);

    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(1);

    return true;
  };
};

static G711aFormatSupporter g711a;



class G726FormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strncmp( sub->codecName(), "G726", 4); }
  CodecType AsType() const { return CODEC_TYPE_AUDIO; }
  GUID AsGUID() const { return MEDIASUBTYPE_g726SUB; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'g', '7', '2', '6' ); };
  bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
  { 
    //AUDIODESCRIPTION
    WAVEFORMATEX *wfe = (WAVEFORMATEX *) pmt->AllocFormatBuffer(sizeof(WAVEFORMATEX));
    if(NULL == wfe) return false;
    ZeroMemory(wfe, sizeof(WAVEFORMATEX));

    wfe->nSamplesPerSec = 8000; 
    wfe->nAvgBytesPerSec = 32000 / 8;
    if( !strcmp( sub->codecName()+5, "40" ) )
      wfe->nAvgBytesPerSec = 40000 / 8;
    else if( !strcmp( sub->codecName()+5, "32" ) )
      wfe->nAvgBytesPerSec = 32000 / 8;
    else if( !strcmp( sub->codecName()+5, "24" ) )
      wfe->nAvgBytesPerSec = 24000 / 8;
    else if( !strcmp( sub->codecName()+5, "16" ) )
      wfe->nAvgBytesPerSec = 16000 / 8;

    wfe->wFormatTag = 0x45;
    wfe->nChannels = 1; 
    wfe->wBitsPerSample = 4; 
    wfe->cbSize	= 0;
    wfe->nBlockAlign = 1;
    //wfe->nAvgBytesPerSec = wfe->nSamplesPerSec * 2;

    pmt->SetType(&MEDIATYPE_Audio);
    pmt->SetFormatType(&FORMAT_WaveFormatEx);
    pmt->SetTemporalCompression(FALSE);

    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(1);

    return true;
  };
};

static G726FormatSupporter g726;



class AACFormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strcmp( sub->codecName(), "MPEG4-GENERIC"); }
  CodecType AsType() const { return CODEC_TYPE_AUDIO; }
  GUID AsGUID() const { return MEDIASUBTYPE_AAC; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'm', 'p', '4', 'a' ); };
  bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
  { 
    //AUDIODESCRIPTION
    unsigned int ExtraSize;
    unsigned char *Extra = parseGeneralConfigStr( 
      sub->fmtp_config(), 
      ExtraSize);

    WAVEFORMATEX *wfe = (WAVEFORMATEX*)pmt->AllocFormatBuffer(sizeof(WAVEFORMATEX) + ExtraSize);
    if(NULL == wfe) return false;
    ZeroMemory(wfe, sizeof(WAVEFORMATEX));
    memcpy((char*)(wfe) + sizeof(WAVEFORMATEX), Extra, ExtraSize);
    delete[] Extra;

    wfe->wFormatTag = 0x00ff;
    wfe->nSamplesPerSec = sub->rtpTimestampFrequency(); 
    if (wfe->nSamplesPerSec <  8000) wfe->nSamplesPerSec = 16000;
    if (wfe->nSamplesPerSec > 48000) wfe->nSamplesPerSec = 16000;

    wfe->nChannels = sub->numChannels(); 
    wfe->nAvgBytesPerSec = 16000;

    wfe->wBitsPerSample = 0; 
    //wfe->cbSize	= 0;
    wfe->nBlockAlign = 0;
    wfe->cbSize  = ExtraSize;

    pmt->SetType(&MEDIATYPE_Audio);
    pmt->SetFormatType(&FORMAT_WaveFormatEx);
    pmt->SetTemporalCompression(FALSE);

    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(1);

    return true;
  };
};

static AACFormatSupporter aac;


// NON ONVIF FORMATS

class MP2TFormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strcmp( sub->codecName(), "MP2T"); }
  CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  GUID AsGUID() const { return MEDIASUBTYPE_mp2tSUB; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'm', 'p', '2', 't' ); };
};

static MP2TFormatSupporter mp2t;


class MPEG2FormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !strcmp( sub->codecName(), "MPV"); }
  CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  GUID AsGUID() const { return MEDIASUBTYPE_MPEG2_VIDEO; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'm', 'p', 'e', 'g' ); };
};

static MPEG2FormatSupporter mpeg2;


class MPAFormatSupporter : public GeneralFormatSupporter
{
public:
  bool ProbeCodec(IMediaSubsession *sub) 
  { 
    return 
      !strcmp( sub->codecName(), "MPA") ||
      !strcmp( sub->codecName(), "MPA-ROBUST") ||
      !strcmp( sub->codecName(), "X-MP3-DRAFT-00"); 
  }
  CodecType AsType() const { return CODEC_TYPE_AUDIO; }
  GUID AsGUID() const { return MEDIASUBTYPE_MPEG1AudioPayload; };
  FOURCC AsFOURCC() const { return MAKEFOURCC( 'm', 'p', 'g', 'a' ); };
  bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
  { 
    //AUDIODESCRIPTION
    MPEG1WAVEFORMAT *mpga = (MPEG1WAVEFORMAT *) pmt->AllocFormatBuffer(sizeof(MPEG1WAVEFORMAT));
    WAVEFORMATEX *wfe = (WAVEFORMATEX*)mpga;
    if(NULL == wfe) return false;
    ZeroMemory(wfe, sizeof(MPEG1WAVEFORMAT));

    wfe->wFormatTag = 0x0050;
    wfe->nSamplesPerSec = 16000;//sub->rtpTimestampFrequency(); 

    wfe->nChannels = 2;//sub->numChannels(); 
    wfe->nAvgBytesPerSec = 16000;

    wfe->wBitsPerSample = 0; 
    wfe->cbSize	= 0;
    wfe->nBlockAlign = 1;
    wfe->cbSize  = 22;

    mpga->fwHeadLayer = ACM_MPEG_LAYER3;				
    mpga->dwHeadBitrate = wfe->nAvgBytesPerSec*8;
    if (wfe->nChannels >= 2)
    {
      mpga->fwHeadMode = ACM_MPEG_STEREO; 
    }
    else
    {
      mpga->fwHeadMode = ACM_MPEG_SINGLECHANNEL;
    }
    mpga->fwHeadModeExt  = 0x00;
    mpga->wHeadEmphasis  = 1;
    mpga->fwHeadFlags = ACM_MPEG_ID_MPEG1;
    mpga->dwPTSLow  =  0;
    mpga->dwPTSHigh = 0;	


    pmt->SetType(&MEDIATYPE_Audio);
    pmt->SetFormatType(&FORMAT_WaveFormatEx);
    pmt->SetTemporalCompression(FALSE);

    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(1);

    return true;
  };
};

static MPAFormatSupporter mpa;


class MetaDataFormatSupporter : public GeneralFormatSupporter
{
  static BITMAPINFOHEADER bmih;
public:
  bool ProbeCodec(IMediaSubsession *sub) { return !stricmp( sub->codecName(), "VND.ONVIF.METADATA"); }
  CodecType AsType() const { return CODEC_TYPE_META; }
  GUID AsGUID() const { return GetBitmapSubtype(&bmih); };
  FOURCC AsFOURCC() const { return BI_RGB; };

  bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
  {
    //OutputDebugStringA("Meta");
    VIDEOINFO *pvi = (VIDEOINFO *) pmt->AllocFormatBuffer(sizeof(VIDEOINFO));
    if (NULL == pvi) return false;
    ZeroMemory(pvi, sizeof(VIDEOINFO));

    int fps = 5;

    SetRectEmpty(&(pvi->rcSource)); // we want the whole image area rendered
    SetRectEmpty(&(pvi->rcTarget)); // no particular destination rectangle

    pvi->AvgTimePerFrame = UNITS / fps;

    bmih.biSizeImage  = GetBitmapSize(&bmih);
    memcpy(&(pvi->bmiHeader), &bmih, sizeof(BITMAPINFOHEADER));
    
    pmt->SetType(&MEDIATYPE_Video);
    pmt->SetFormatType(&FORMAT_VideoInfo);
    pmt->SetTemporalCompression(FALSE);

    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);

    return true;
  };

  FrameInfo* NextChunk(IMediaSubsession *sub, unsigned char* buffer, int size, FrameInfo *Prefix) const
  {
    buffer[size] = 0;
    //OutputDebugStringA((char*)buffer);


    FrameInfo* Info = FrameInfo::Init(bmih.biSizeImage);
    //memcpy( Info->pdata, buffer, size );

    HDC hdcScreen = GetDC(NULL);
    HDC dc = CreateCompatibleDC(hdcScreen);
    HBITMAP bm = CreateCompatibleBitmap(dc, bmih.biWidth, bmih.biHeight);
    HBITMAP bmo = SelectBitmap(dc, bm);
    
    RECT rc = {0, 0, bmih.biWidth, bmih.biHeight};
    DrawTextA(dc, (char*)buffer, size, &rc, DT_TOP | DT_WORDBREAK);

    //TextOutA(dc, 20, 20, (char*)buffer, size);
    GetDIBits(dc, bm, 0, bmih.biHeight, Info->pdata, (BITMAPINFO*)&bmih, DIB_RGB_COLORS);

    SelectBitmap(dc, bmo);

    DeleteBitmap(bm);
    DeleteDC(dc);
    ReleaseDC(NULL, hdcScreen);      

    return Info;
  }
};

BITMAPINFOHEADER MetaDataFormatSupporter::bmih = {
  sizeof(BITMAPINFOHEADER),
  320*2,
  240*2,
  1,
  24,
  BI_RGB,
  320*240*3*4
};

static MetaDataFormatSupporter meta;



class FakeVideoFormatSupporter : public GeneralFormatSupporter
{
  static BITMAPINFOHEADER bmih;
public:
  bool ProbeCodec(IMediaSubsession *sub) { return (sub == NULL); }
  CodecType AsType() const { return CODEC_TYPE_VIDEO; }
  GUID AsGUID() const { return GetBitmapSubtype(&bmih); };
  FOURCC AsFOURCC() const { return BI_RGB; };

  bool GetFormat(CMediaType *pmt, IMediaSubsession *sub, FrameInfo **Prefix) const 
  {
    VIDEOINFO *pvi = (VIDEOINFO *) pmt->AllocFormatBuffer(sizeof(VIDEOINFO));
    if (NULL == pvi) return false;
    ZeroMemory(pvi, sizeof(VIDEOINFO));

    SetRectEmpty(&(pvi->rcSource)); // we want the whole image area rendered
    SetRectEmpty(&(pvi->rcTarget)); // no particular destination rectangle

    pvi->AvgTimePerFrame = 0;

    bmih.biSizeImage  = GetBitmapSize(&bmih);
    memcpy(&(pvi->bmiHeader), &bmih, sizeof(BITMAPINFOHEADER));
    
    pmt->SetType(&MEDIATYPE_Video);
    pmt->SetFormatType(&FORMAT_VideoInfo);
    pmt->SetTemporalCompression(FALSE);

    const GUID SubTypeGUID = AsGUID();
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);

    return true;
  };

  FrameInfo* NextChunk(IMediaSubsession *sub, unsigned char* buffer, int size, FrameInfo *Prefix) const
  {
    FrameInfo* Info = FrameInfo::Init(bmih.biSizeImage);
    if (size > 0)
    {
      HDC hdcScreen = GetDC(NULL);
      HDC dc = CreateCompatibleDC(hdcScreen);
      HBITMAP bm = CreateCompatibleBitmap(dc, bmih.biWidth, bmih.biHeight);
      HBITMAP bmo = SelectBitmap(dc, bm);

      RECT rc = {0, 0, bmih.biWidth, bmih.biHeight};
      SetBkMode(dc, TRANSPARENT);
      SetTextColor(dc, RGB(255,255,255));
      SelectObject(dc, GetStockObject(SYSTEM_FIXED_FONT));
      DrawTextA(dc, (char*)buffer, size, &rc, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

      GetDIBits(dc, bm, 0, bmih.biHeight, Info->pdata, (BITMAPINFO*)&bmih, DIB_RGB_COLORS);

      SelectBitmap(dc, bmo);

      DeleteBitmap(bm);
      DeleteDC(dc);
      ReleaseDC(NULL, hdcScreen);      
    }
    return Info;
  }
};

BITMAPINFOHEADER FakeVideoFormatSupporter::bmih = {
  sizeof(BITMAPINFOHEADER),
  320,
  240,
  1,
  24,
  BI_RGB
};

static FakeVideoFormatSupporter fakeVideo;


GeneralFormatSupporter *SupportedFormats[] =
{
  // core ONVIF formats
  &mjpeg,
  &mpeg4,
  &h264,
  &g711u,
  &g711a,
  &g726,
  &aac,
  &mp2t,
  &mpeg2,
  &mpa,
  &meta
  // maybe others
};

GeneralFormatSupporter *ProbeByCodec(IMediaSubsession *sub)
{
  if (!sub)
    return &fakeVideo;
  for (int i = 0 ; i < sizeof(SupportedFormats) / sizeof(SupportedFormats[0]); i++)
  {
    if (SupportedFormats[i]->ProbeCodec(sub))
    {
      return SupportedFormats[i];
    }
  }
  return NULL;
}
