#include "Formats.h"

//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow
//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow whitelist
//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow_audio
//HKEY_USERS\S-1-5-21-620922525-2556621756-4136132698-2049\Software\GNU\ffdshow_enc

struct CapsuleSettings
{
  char Address[260];
  int   TunnelPortNumber;
  bool  UseTCPTunnel;
  bool  UseHTTPTunnel;
  char User[64];
  char Password[64];
  WCHAR Pipe[260];
  CapsuleSettings()
  {
    Address[0] = 0;
    TunnelPortNumber = 0;
    UseTCPTunnel = false;
    UseHTTPTunnel = false;
    User[0] = 0;
    Password[0] = 0;
    Pipe[0] = 0;
  }
  void Load(const WCHAR* FileName);
};

class TrackBuffer
{
  typedef std::vector<unsigned char> ContainerType;
  ContainerType Container;
public:
  TrackBuffer() {};
  TrackBuffer(int size)
  {
    SetSize(size);
  }
  void SetSize(int size) { Container.resize(size); };
  int GetSize() { return Container.size(); };
  unsigned char* GetBuffer() { return const_cast<unsigned char*>(&(Container.front())); };
};

class CapsuleTrack
{
  friend class LiveCapsule;
  class LiveCapsule *Parent;
  MediaSubsession   *sub;
  GeneralFormatSupporter *Supporter;
  TrackBuffer       Buffer;
  CQueue<FrameInfo*> Frames;
  REFERENCE_TIME Start;
  
  static void AfterGetting(
    void* clientData, 
    unsigned frameSize,
    unsigned numTruncatedBytes,
    struct timeval presentationTime,
    unsigned durationInMicroseconds);
  static void OnClose(void* clientData);

public:
  CapsuleTrack(LiveCapsule *P, 
    MediaSubsession   *s,
    GeneralFormatSupporter *f) :
      Frames(400)
  {
    Parent = P;
    sub = s;
    Supporter = f;
    Buffer.SetSize((Supporter->AsType() == CODEC_TYPE_VIDEO) ? 65536 : 4096);
    Start = 0;
  };
  void ReadSource();
  FrameInfo* GetTop();
};

class LiveCapsule : protected CAMThread
{
  MediaSession     *ms;
  TaskScheduler    *scheduler;
  UsageEnvironment *env ;
  RTSPClient       *rtsp;	
public:
  CapsuleTrack     *Audio;
  CapsuleTrack     *Video;
private:
  RTSPClientState   State;
  char              StopFlag;
  bool InitEnvironment();
  void HaltEnvironment();
  char* DescribeStream(const char* filename);
  void UseSubSource(MediaSubsession *sub, GeneralFormatSupporter *supporter);
  virtual DWORD ThreadProc();

  CapsuleSettings Settings;
public:
  LiveCapsule()
  {
    ms = NULL;
    scheduler = NULL;
    env = NULL;
    rtsp = NULL;	
    Audio = NULL;
    Video = NULL;
    State = RTSP_STATE_IDLE;
    StopFlag = 0;
  }
  ~LiveCapsule()
  {
    HaltEnvironment();
  }
  bool OpenSettings(const WCHAR* filename);
  bool OpenStream(const char* filename);
  bool PlayStream();
  bool PauseStream();
  bool StopStream();
  bool CloseStream();

  bool GetVideoFormat(CMediaType *pmt) const
  {
    return Video->Supporter->GetFormat(pmt, Video->sub);
  }
  bool CheckVideoFormat(const CMediaType *pmt) const
  {
    return Video->Supporter->CheckFormat(pmt, Video->sub);
  }
};
