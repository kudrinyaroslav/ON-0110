///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include <strsafe.h>
#include "LiveCapsule.h"
#include "Guids.h"

// UNITS = 10 ^ 7  
// UNITS / 30 = 30 fps;
// UNITS / 20 = 20 fps, etc
const REFERENCE_TIME FPS_30 = UNITS / 30;
const REFERENCE_TIME FPS_25 = UNITS / 25;
const REFERENCE_TIME FPS_20 = UNITS / 20;
const REFERENCE_TIME FPS_10 = UNITS / 10;
const REFERENCE_TIME FPS_5  = UNITS / 5;
const REFERENCE_TIME FPS_4  = UNITS / 4;
const REFERENCE_TIME FPS_3  = UNITS / 3;
const REFERENCE_TIME FPS_2  = UNITS / 2;
const REFERENCE_TIME FPS_1  = UNITS / 1;

const REFERENCE_TIME rtDefaultFrameLength = FPS_10;

#define g_wszONVIFRTSP    L"ONVIF RTSP Source Filter"

class CRTSPPinMedia : public CSourceStream
{
  REFERENCE_TIME CurrentOut;
  REFERENCE_TIME CurrentIn;
  REFERENCE_TIME BackIn;
  REFERENCE_TIME Slice;
  REFERENCE_TIME SliceAverage;
  int Tick;
  void UpdateSlice(REFERENCE_TIME Current);
protected:
  bool Audio;
public:

    CRTSPPinMedia(HRESULT *phr, CSource *pFilter, bool a);
    ~CRTSPPinMedia();

    // Override the version that offers exactly one media type
    HRESULT DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pRequest);
    HRESULT FillBuffer(IMediaSample *pSample);
    
    // Set the agreed media type and set up the necessary parameters
    HRESULT SetMediaType(const CMediaType *pMediaType);

    // Support multiple display formats
    HRESULT CheckMediaType(const CMediaType *pMediaType);
    HRESULT GetMediaType(int iPosition, CMediaType *pmt);
};


class CRTSPSource : public CSource, IFileSourceFilter, ITestControl
{

private:
    // Constructor is private because you have to use CreateInstance
    CRTSPSource(IUnknown *pUnk, HRESULT *phr);
    ~CRTSPSource();

    CRTSPPinMedia *PinVideo;
    CRTSPPinMedia *PinAudio;

    LiveCapsule Capsule;
    WCHAR FileName[260];
    MessageLog Log;
    bool Terminating;
public:

  bool IsTerminating() { return Terminating; };
    static CUnknown * WINAPI CreateInstance(IUnknown *pUnk, HRESULT *phr);  

    // IUnknown
    DECLARE_IUNKNOWN;
    STDMETHODIMP NonDelegatingQueryInterface(REFIID _riid, void **_ppv);

    // IFileSourceFilter
    STDMETHODIMP Load(LPCOLESTR inFileName, const AM_MEDIA_TYPE* inMediaType);
    STDMETHODIMP GetCurFile(LPOLESTR *_filename, AM_MEDIA_TYPE *_pmt);

    // ITestControl
    //STDMETHODIMP PollLog(LPOLESTR *_filename);
    //STDMETHODIMP TrySetParameter(int* Ok);
    STDMETHODIMP ControlStream(int Control);
    //STDMETHODIMP GetCurrentStep(int* Step, int* PassBits, int* FailBits);
    //STDMETHODIMP GetStepName(int Step, LPOLESTR *StepName);

    STDMETHODIMP WaitForStableKey(double *Length);
    STDMETHODIMP WaitForSync(int *Length);
    STDMETHODIMP GetEvents(LPOLESTR *StepName);

    STDMETHODIMP GetStreamHealth(int *State);


    LiveCapsule* GetCapsule() { return &Capsule; };
};


