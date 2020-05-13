///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include <streams.h>

#include "SourceFilter.h"
#include "Guids.h"
#include "time.h"



#ifdef DEBUG
//#pragma comment(lib,"../lib/newgen/Debug/live.lib") 
#pragma comment(lib,"../lib/debug/strmbasd.lib") 
#else
//#pragma comment(lib,"../lib/newgen/Release/live.lib") 
#pragma comment(lib,"../lib/strmbase.lib")  
#endif

//#define MyInfo(text) {MessageBox(NULL, TEXT(text), L"Info O", MB_OK | MB_TOPMOST);}
//#define MyInfo(text) {}
#define MyInfo(text) { OutputDebugStringA("####FILTER " ## text ## "\n"); }

#define MyInfoFunction MyInfo(__FUNCTION__)

CRTSPPinMedia::CRTSPPinMedia(HRESULT *phr, CSource *pFilter, bool a)
: CSourceStream(NAME("ONVIF RTSP Source"), phr, pFilter, a ? L"AudioOut" : L"VideoOut"), Audio (a)
{
  Slice = 10000000 / 5;
}

CRTSPPinMedia::~CRTSPPinMedia()
{   
}


HRESULT CRTSPPinMedia::GetMediaType(int iPosition, CMediaType *pmt)
{
  MyInfoFunction;

  if(iPosition < 0)
    return E_INVALIDARG;
  if(iPosition > 0)
    return VFW_S_NO_MORE_ITEMS;
  CLiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
  if (Audio ? Capsule->GetAudioFormat(pmt) : Capsule->GetVideoFormat(pmt))
    return NOERROR;
  return E_INVALIDARG;
}


HRESULT CRTSPPinMedia::CheckMediaType(const CMediaType *pMediaType)
{
  MyInfoFunction;

  CLiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
  if (Audio ? Capsule->CheckAudioFormat(pMediaType) : Capsule->CheckVideoFormat(pMediaType))
  {
    return S_OK;
  }
  return E_INVALIDARG;
}


HRESULT CRTSPPinMedia::DecideBufferSize(IMemAllocator *pAlloc,
                                      ALLOCATOR_PROPERTIES *pProperties)
{
  MyInfoFunction;

    CheckPointer(pAlloc, E_POINTER);
    CheckPointer(pProperties, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());
    HRESULT hr = NOERROR;
    if (Audio)
    {
      pProperties->cBuffers = 1;
      pProperties->cbBuffer = 25000;
    }
    else
    {

      VIDEOINFO *pvi = (VIDEOINFO *) m_mt.Format();
      pProperties->cBuffers = 1;
      pProperties->cbBuffer = pvi->bmiHeader.biSizeImage*16;
      if (pvi->bmiHeader.biSizeImage > 0)
      {
        MyInfo("Buffer not zero");
      }
      else
      {
        MyInfo("Buffer zero - bad");
      }
    }

    ASSERT(pProperties->cbBuffer);

    // Ask the allocator to reserve us some sample memory. NOTE: the function
    // can succeed (return NOERROR) but still not have allocated the
    // memory that we requested, so we must check we got whatever we wanted.
    ALLOCATOR_PROPERTIES Actual;
    hr = pAlloc->SetProperties(pProperties,&Actual);
    if(FAILED(hr))
    {
        return hr;
    }

    // Is this allocator unsuitable?
    if(Actual.cbBuffer < pProperties->cbBuffer)
    {
        return E_FAIL;
    }

    // Make sure that we have only 1 buffer (we erase the ball in the
    // old buffer to save having to zero a 200k+ buffer every time
    // we draw a frame)
    ASSERT(Actual.cBuffers == 1);
    return NOERROR;

} // DecideBufferSize


//
// SetMediaType
//
// Called when a media type is agreed between filters
//
HRESULT CRTSPPinMedia::SetMediaType(const CMediaType *pMediaType)
{
  MyInfoFunction;

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // Pass the call up to my base class
    HRESULT hr = CSourceStream::SetMediaType(pMediaType);

    if(SUCCEEDED(hr))
    {
      if (!Audio)
      {
        BYTE * pvi = m_mt.Format();
        if (pvi == NULL)
            return E_UNEXPECTED;

        CLiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
        if (Capsule->CheckVideoFormat(pMediaType))
        {
          return S_OK;
        }
        return E_INVALIDARG;
      }
      else
      {
        BYTE * pvi = m_mt.Format();
        if (pvi == NULL)
          return E_UNEXPECTED;

        CLiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
        if (Capsule->CheckAudioFormat(pMediaType))
        {
          return S_OK;
        }
        return E_INVALIDARG;
      }
    } 

    return hr;
}



// This is where we insert the DIB bits into the video stream.
// FillBuffer is called once for every sample in the stream.
HRESULT CRTSPPinMedia::FillBuffer(IMediaSample *pSample)
{
	  BYTE *pData;
    long cbData;

    CheckPointer(pSample, E_POINTER);

    // Access the sample's data buffer
    pSample->GetPointer(&pData);
    cbData = pSample->GetSize();

    if (((CRTSPSource*)m_pFilter)->IsTerminating())
    {
      OutputDebugStringA("FillBuffer - in terminating step\n");
      return S_FALSE;
    }

    CLiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();

    FrameInfo *fi = NULL;
    do {
      fi = Capsule->GetTop(Audio);
      if (!fi)
        Sleep(10);
    } while (!fi && !((CRTSPSource*)m_pFilter)->IsTerminating());
    if (!fi) return S_FALSE;
    
    int nSize = min(fi->frameHead.FrameLen, (DWORD) cbData);
    if (nSize != fi->frameHead.FrameLen)
    {
//      DropDebug("Trimmed frame from %i to %i\n", fi->frameHead.FrameLen, nSize);
    }
    memcpy(pData, fi->pdata, nSize);
    pSample->SetActualDataLength(nSize);

    int Count = Capsule->GetCount(Audio);
    REFERENCE_TIME UseSlice = Slice;
    if (Count > 10)
    {
      UseSlice = UseSlice * (120 - Count) / 100;
    }
    if (Count < 2)
    {
      UseSlice = UseSlice * 2;
    }
    REFERENCE_TIME CurrentOut = (REFERENCE_TIME)(time(NULL)) * 1e7 + UseSlice;
    REFERENCE_TIME rtStart = CurrentOut;
    REFERENCE_TIME rtStop  = rtStart + UseSlice;
    pSample->SetTime(NULL, NULL);
//    pSample->SetTime(&rtStart, &rtStop);

    //pSample->SetSyncPoint(TRUE);
    //pSample->SetSyncPoint(!fi->frameHead.InterFrame);

    FrameInfo::Release(fi);

    return S_OK;
}



CRTSPSource::CRTSPSource(IUnknown *pUnk, HRESULT *phr)
           : CSource(NAME("ONVIF RTSP Source"), pUnk, CLSID_SourceFilter)
{
  MyInfoFunction;
    // The pin magically adds itself to our pin array.
  *phr = E_OUTOFMEMORY;
  PinAudio = NULL;
  PinVideo = new CRTSPPinMedia(phr, this, false);
  if (phr)
  {
    if (PinVideo == NULL) return;
  }  
  PinAudio = NULL;
  *phr = S_OK;
  FileName[0] = 0;
  Terminating = false;
}


CRTSPSource::~CRTSPSource()
{
  MyInfoFunction;

  Terminating = true;
  GetCapsule()->SetTerminating(false);
  delete PinAudio;
  delete PinVideo;
}


CUnknown * WINAPI CRTSPSource::CreateInstance(IUnknown *pUnk, HRESULT *phr)
{
  MyInfoFunction;

  CRTSPSource *pNewFilter = new CRTSPSource(pUnk, phr );

	if (phr)
	{
		if (pNewFilter == NULL) 
			*phr = E_OUTOFMEMORY;
		else
			*phr = S_OK;
	}
    return pNewFilter;

}

extern TSharpStepCallback GlobalStepCallback;

STDMETHODIMP CRTSPSource::Load(LPCOLESTR inFileName, const AM_MEDIA_TYPE* inMediaType)
{
  MyInfoFunction;
  try {
    Capsule.SetNotificationCallback(GlobalStepCallback);
    if (FileName[0])
    {
      if (wcscmp(FileName, inFileName))
      {
        return E_FAIL;
      }
      return S_OK;
    }
    wcscpy(FileName, inFileName);

    if (!Capsule.Configure(FileName))
      return E_FAIL;

    if (Capsule.HasAudio())
    {
      HRESULT hr;
      PinAudio = new CRTSPPinMedia(&hr, this, true);
    }

    return S_OK;
  } catch (...) {
    return E_FAIL;
  }
}


STDMETHODIMP CRTSPSource::GetCurFile(LPOLESTR *_filename, AM_MEDIA_TYPE *_pmt)
{
  MyInfoFunction;

  if (_filename)
  {
    size_t size = wcslen(FileName) + 1;

    *_filename = (LPOLESTR)CoTaskMemAlloc(size * sizeof(wchar_t));
    if (!*_filename) return E_OUTOFMEMORY;
    wcscpy(*_filename, FileName);
    (*_filename)[size-1] = 0; // make sure that string is properly ended
  }
  else
    return E_POINTER;

  return S_OK;
}

STDMETHODIMP CRTSPSource::Run(REFERENCE_TIME tStart)
{
  MyInfoFunction;

  Terminating = false;
  HRESULT r = CSource::Run(tStart);
  if (r != S_OK)
    return r;
  if (!GetCapsule()->Play())
    return S_FALSE;
  return S_OK;
  //return CSource::Run(tStart);
}

STDMETHODIMP CRTSPSource::Pause()
{
  MyInfoFunction;

  GetCapsule()->Pause();
  return CSource::Pause();
}

STDMETHODIMP CRTSPSource::Stop()
{
  MyInfoFunction;

  Terminating = true;
  GetCapsule()->SetTerminating(false);
  GetCapsule()->Stop();
  return CSource::Stop();
}

STDMETHODIMP CRTSPSource::GetState(DWORD dwMilliSecsTimeout, FILTER_STATE *State)
{
  CSource::GetState(dwMilliSecsTimeout, State);
  CheckPointer(State, E_POINTER);
  ELiveStep Step = GetCapsule()->GetCurrentStep();
  if (Step == ELS_Play) {
    *State = State_Running;
  } else if (Step == ELS_Pause) {
    *State = State_Paused;
    return VFW_S_CANT_CUE;
  } else {
    *State = State_Stopped;
  }
  return S_OK;
}


STDMETHODIMP CRTSPSource::RunSequence(int Sequence)
{
  MyInfoFunction;

  if (GetCapsule()->RunSequence(Sequence)) {
    return S_OK;
  }
  return E_FAIL;
}

STDMETHODIMP CRTSPSource::RunStep(int Step)
{
  MyInfoFunction;

  if (GetCapsule()->RunStep(Step)) {
    return S_OK;
  }
  return E_FAIL;
}

STDMETHODIMP CRTSPSource::GetActivity(int *Activity)
{
  MyInfoFunction;

  CheckPointer(Activity, E_POINTER);
  *Activity = GetCapsule()->IsInSequence();
  return S_OK;
}

STDMETHODIMP CRTSPSource::StopActivity()
{
  MyInfoFunction;

  Terminating = true;
  GetCapsule()->SetTerminating(true);
  return S_OK;
}



STDMETHODIMP CRTSPSource::GetEvents(LPOLESTR *EventText)
{
  MyInfoFunction;

  CheckPointer(EventText, E_POINTER);

  FrameInfo* fi = Capsule.GetMeta();
  if (!fi) {
    return S_FALSE;
  }
  size_t size = fi->frameHead.FrameLen + 1;

  *EventText = (LPOLESTR)CoTaskMemAlloc(size * sizeof(wchar_t));
  if (!*EventText) return E_OUTOFMEMORY;
  mbstowcs(*EventText, (const char*)fi->pdata, size);
  (*EventText)[size-1] = 0; // make sure that string is properly ended

  return S_OK;
}



STDMETHODIMP 
CRTSPSource::NonDelegatingQueryInterface(REFIID _riid, void **_ppv)
{
  MyInfoFunction;

  CheckPointer(_ppv, E_POINTER);
  if (_riid == IID_IFileSourceFilter)
    return GetInterface((IFileSourceFilter *) this, _ppv);
  if (_riid == IID_TestControl)
    return GetInterface((ITestControl *) this, _ppv);
  return CSource::NonDelegatingQueryInterface(_riid, _ppv);
}




HRESULT CRTSPPinMedia::OnThreadCreate(void)
{
  MyInfoFunction;
  return NOERROR;
}

HRESULT CRTSPPinMedia::OnThreadDestroy(void)
{
  MyInfoFunction;
  return NOERROR;
}

HRESULT CRTSPPinMedia::OnThreadStartPlay(void)
{
  MyInfoFunction;
  return NOERROR;
}

#if 1

HRESULT CRTSPPinMedia::DoBufferProcessingLoop()
{ 
  MyInfo("DoBufferProcessingLoop begin");
  HRESULT Res = CSourceStream::DoBufferProcessingLoop();
  MyInfo("DoBufferProcessingLoop end");
  return Res;
}

#else

HRESULT CRTSPPinMedia::DoBufferProcessingLoop(void) 
{

  Command com;

  OnThreadStartPlay();

  do {
    while (!CheckRequest(&com)) {

      IMediaSample *pSample;

      HRESULT hr = GetDeliveryBuffer(&pSample,NULL,NULL,0);
      if (FAILED(hr)) {
        Sleep(1);
        continue;	// go round again. Perhaps the error will go away
        // or the allocator is decommited & we will be asked to
        // exit soon.
      }

      // Virtual function user will override.
      hr = FillBuffer(pSample);

      if (hr == S_OK) {
        hr = Deliver(pSample);
        pSample->Release();

        // downstream filter returns S_FALSE if it wants us to
        // stop or an error if it's reporting an error.
        if(hr != S_OK)
        {
          DbgLog((LOG_TRACE, 2, TEXT("Deliver() returned %08x; stopping"), hr));
          return S_OK;
        }

      } else if (hr == S_FALSE) {
        // derived class wants us to stop pushing data
        pSample->Release();
        DeliverEndOfStream();
        return S_OK;
      } else {
        // derived class encountered an error
        pSample->Release();
        DbgLog((LOG_ERROR, 1, TEXT("Error %08lX from FillBuffer!!!"), hr));
        DeliverEndOfStream();
        m_pFilter->NotifyEvent(EC_ERRORABORT, hr, 0);
        return hr;
      }

      // all paths release the sample
    }

    // For all commands sent to us there must be a Reply call!

    if (com == CMD_RUN || com == CMD_PAUSE) {
      Reply(NOERROR);
    } else if (com != CMD_STOP) {
      Reply((DWORD) E_UNEXPECTED);
      DbgLog((LOG_ERROR, 1, TEXT("Unexpected command!!!")));
    }
  } while (com != CMD_STOP);

  return S_FALSE;
}

#endif