///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#include <streams.h>

#include "SourceFilter.h"
#include "Guids.h"



#ifdef DEBUG
#pragma comment(lib,"../lib/debug/live.lib") 
#pragma comment(lib,"../lib/debug/strmbasd.lib") 
#else
#pragma comment(lib,"../lib/live.lib") 
#pragma comment(lib,"../lib/strmbase.lib") 
#endif

//#define MyInfo(text) {MessageBox(NULL, TEXT(text), L"Info O", MB_OK | MB_TOPMOST);}
#define MyInfo(text) {}
//#define MyInfo(text) { OutputDebugStringA(text); }


CRTSPPinMedia::CRTSPPinMedia(HRESULT *phr, CSource *pFilter, bool a)
: CSourceStream(NAME("ONVIF RTSP Source"), phr, pFilter, a ? L"AudioOut" : L"VideoOut"), Audio (a)
{
  CurrentOut = 0;
  CurrentIn = 0;
  BackIn = 0;
  Slice = 0;
  SliceAverage = 0;
  Tick = 0;
}

CRTSPPinMedia::~CRTSPPinMedia()
{   
}


HRESULT CRTSPPinMedia::GetMediaType(int iPosition, CMediaType *pmt)
{
  MyInfo("GetMediaType");

  if(iPosition < 0)
    return E_INVALIDARG;
  if(iPosition > 0)
    return VFW_S_NO_MORE_ITEMS;
  LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
  if (Audio ? Capsule->GetAudioFormat(pmt) : Capsule->GetVideoFormat(pmt))
    return NOERROR;
  return E_INVALIDARG;
}


HRESULT CRTSPPinMedia::CheckMediaType(const CMediaType *pMediaType)
{
  MyInfo("CheckMediaType");

  LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
  if (Audio ? Capsule->CheckAudioFormat(pMediaType) : Capsule->CheckVideoFormat(pMediaType))
  {
    return S_OK;
  }
  return E_INVALIDARG;
}


HRESULT CRTSPPinMedia::DecideBufferSize(IMemAllocator *pAlloc,
                                      ALLOCATOR_PROPERTIES *pProperties)
{
  MyInfo("DecideBufferSize");

    CheckPointer(pAlloc,E_POINTER);
    CheckPointer(pProperties,E_POINTER);

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
  MyInfo("SetMediaType");

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

        LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
        if (Capsule->CheckVideoFormat(pMediaType))
        {
          return S_OK;
        }
        return E_INVALIDARG;
      }
      else
      {
//TODO Audio
        BYTE * pvi = m_mt.Format();
        if (pvi == NULL)
          return E_UNEXPECTED;

        LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
        if (Capsule->CheckAudioFormat(pMediaType))
        {
          return S_OK;
        }
        return E_INVALIDARG;
      }
    } 

    return hr;

}

void DropDebug(const char* Format, ...);

void CRTSPPinMedia::UpdateSlice(REFERENCE_TIME Current)
{
  Tick++;
  if (!BackIn)
  {
    BackIn = CurrentIn;
    CurrentIn = Current;
    return;
  };
  BackIn = CurrentIn;
  CurrentIn = Current;
  if (CurrentIn >= BackIn)
  {
    //if ((Slice <= 0) || (Slice > CurrentIn - BackIn))
      Slice = CurrentIn - BackIn;
  }
  else
  {
    DropDebug("Leap back detected [%i], old step [%i] used", (int)((BackIn - CurrentIn + 5000) / 10000), (int)((Slice + 5000) / 10000));
  }
  if (SliceAverage) 
  {
    if ((Slice > SliceAverage * 4) && (Tick > 10))
    {
      DropDebug("Leap forward detected [%i], average step [%i] used", (int)((Slice + 5000) / 10000), (int)((SliceAverage + 5000) / 10000));
      Slice = SliceAverage;
    }
    if (Slice)
    {
      SliceAverage = (SliceAverage * 15 + Slice + 15) / 16;
    }
  }
  else
  {
    SliceAverage = Slice;
  };
  if (Slice < 0)
  {
    DropDebug("Strange slice detected [%i], 33ms used", (int)((Slice + 5000) / 10000));
    Slice = 330000;
  }
  //if (Slice < SliceAverage) Slice = SliceAverage + SliceAverage / 8;
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
      OutputDebugStringA("Providing empty buffer");
      return S_FALSE;
    }

    LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();

    //DropDebug("Ask %i - %08x, %i\n", Audio, GetCurrentThreadId(), Capsule->Audio->GetCount());
#if 0
    if (Capsule->NeedRePlay())
    //if (Capsule->GetState() != RTSP_STATE_PLAYING)
    {
      OutputDebugStringA("Need to Run");
      //if (Audio) return S_FALSE;
      if (!Audio)
      {
        OutputDebugStringA("Run in Video");
        if (!Capsule->PlayStream())
        {
          OutputDebugStringA("Stream NOT run");
          MyInfo("Stream NOT run");
        }
      }
    }
#endif
    FrameInfo *fi = Capsule->GetTop(Audio);
    //DropDebug("Top %i - %08x\n", Audio, GetCurrentThreadId());
    if (!fi) return S_FALSE;
    
    int nSize = min(fi->frameHead.FrameLen, (DWORD) cbData);
    if (nSize != fi->frameHead.FrameLen)
    {
      DropDebug("Trimmed frame from %i to %i\n", fi->frameHead.FrameLen, nSize);
    }
    memcpy(pData, fi->pdata, fi->frameHead.FrameLen);
    pSample->SetActualDataLength(nSize);
    
    UpdateSlice(fi->frameHead.Start);

    int Count = Capsule->GetCount(Audio);
    if (Count > 10)
    {
      Slice = Slice * (120 - Count) / 100;
    }
    if (Count < 3)
    {
      Slice = Slice * 2;
    }
#if 1
    CurrentOut += Slice;
    REFERENCE_TIME rtStart = CurrentOut;
    REFERENCE_TIME rtStop  = rtStart + Slice;
    if (!Slice) rtStop  = rtStart + SliceAverage;
#else
    REFERENCE_TIME rtStart = CurrentOut;
    REFERENCE_TIME rtStop  = rtStart + Slice;
    CurrentOut = rtStop;
#endif

    DropDebug("Get %i, size(%i), time(%i), slice(%i)\n",
    Audio,
    nSize,
    int((rtStart + 5000)/10000),
    int((rtStop - rtStart + 5000)/10000));

    pSample->SetTime(&rtStart, &rtStop);
	// Set TRUE on every sample for uncompressed frames
    //if (Audio || (fi->frameHead.FrameLen > 40000)) 
      //pSample->SetSyncPoint(TRUE);

    pSample->SetSyncPoint(TRUE);
    //pSample->SetSyncPoint(!fi->frameHead.InterFrame);

    FrameInfo::Release(fi);

    return S_OK;
}



CRTSPSource::CRTSPSource(IUnknown *pUnk, HRESULT *phr)
           : CSource(NAME("ONVIF RTSP Source"), pUnk, CLSID_SourceFilter), 
             Capsule(&Log)
{
    // The pin magically adds itself to our pin array.
  *phr = E_OUTOFMEMORY;
  PinAudio = NULL;
  PinVideo = new CRTSPPinMedia(phr, this, false);
  if (phr)
  {
    if (PinVideo == NULL) return;
  }  
#if 0
  PinAudio = new CRTSPPinMedia(phr, this, true);
  if (phr)
  {
    if (PinAudio == NULL) return;
  }  
#else
  PinAudio = NULL;
#endif
  *phr = S_OK;
  FileName[0] = 0;
  OutputDebugStringA("Constructor");
  Terminating = false;
}


CRTSPSource::~CRTSPSource()
{
  OutputDebugStringA("Destructor");
  if (PinAudio) delete PinAudio;
  delete PinVideo;
}


CUnknown * WINAPI CRTSPSource::CreateInstance(IUnknown *pUnk, HRESULT *phr)
{
  OutputDebugStringA("CreateInstance");
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

STDMETHODIMP CRTSPSource::Load(LPCOLESTR inFileName, const AM_MEDIA_TYPE* inMediaType)
{
 try {
  MyInfo("Load");

  if (FileName[0])
  {
    if (wcscmp(FileName, inFileName))
    {
      return E_FAIL;
    }
    return S_OK;
  }
  wcscpy(FileName, inFileName);

  if (!Capsule.OpenSettings(FileName))
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

STDMETHODIMP 
CRTSPSource::GetCurFile(LPOLESTR *_filename, AM_MEDIA_TYPE *_pmt)
{
  MyInfo("GetCurFile");

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
/*
STDMETHODIMP CRTSPSource::PollLog(LPOLESTR *_filename)
{
  CheckPointer(_filename, E_POINTER);
#if 1
  if (Log.CanGet())
    *_filename = Log.GetTop();
  else
    *_filename = NULL;
#else
  *_filename = (LPOLESTR)CoTaskMemAlloc(30);
  if (!*_filename) return E_OUTOFMEMORY;
static int i = 0;

  wsprintf(*_filename, L"Call %i", i++);
//wcscpy(*_filename, L"54647");
#endif
  return S_OK;
}

STDMETHODIMP CRTSPSource::TrySetParameter(int* Ok)
{
  CheckPointer(Ok, E_POINTER);
  *Ok = Capsule.TrySetParameter();
  return S_OK;
}
*/
STDMETHODIMP CRTSPSource::ControlStream(int Control)
{
 try {
  switch (Control)
  {
  case 1:
    {
      Terminating = false;
      OutputDebugStringA("CRTSPSource::StartStream");
      bool Ret = Capsule.PlayStream();
      if (Capsule.GetState() != RTSP_STATE_PLAYING)
      {
        if (Ret) Capsule.StopStream();
        return E_FAIL;
      }
      return S_OK;
    }
  case 0:
    {
      OutputDebugStringA("CRTSPSource::StopStream");
      return Capsule.StopStream() ? S_OK : E_FAIL;
    }
  case 2:
    {
      OutputDebugStringA("CRTSPSource::GoTerminate");
      Terminating = true;
      Capsule.UnlockTracks();
      return S_OK;
    }
  }
 } catch (...) {
   return E_FAIL;
 }
}
/*
STDMETHODIMP CRTSPSource::GetCurrentStep(int* Step, int* PassBits, int* FailBits)
{
  CheckPointer(Step, E_POINTER);
  CheckPointer(PassBits, E_POINTER);
  CheckPointer(FailBits, E_POINTER);
  Capsule.GetCurrentStep(*Step, *PassBits, *FailBits);
  return S_OK;
}

STDMETHODIMP CRTSPSource::GetStepName(int Step, LPOLESTR *StepName)
{
  CheckPointer(StepName, E_POINTER);

  WCHAR rawStepName[40];
  wsprintf(rawStepName, L"Step%i", Step);
  size_t size = wcslen(rawStepName) + 1;

  *StepName = (LPOLESTR)CoTaskMemAlloc(size * sizeof(wchar_t));
  if (!*StepName) return E_OUTOFMEMORY;
  wcscpy(*StepName, rawStepName);
  (*StepName)[size-1] = 0; // make sure that string is properly ended

  return S_OK;
}
*/
STDMETHODIMP CRTSPSource::WaitForStableKey(double *Length)
{
  CheckPointer(Length, E_POINTER);
  const CapsuleTrack* Video = Capsule.GetVideo();
  if (!Video) return E_POINTER;

  for (int i = 0; i < 300; i++)
  {
    if (Video->StableKey()) 
    {
      //*Length = Video->GetKeyLenAv();
      *Length = Video->GetKeyLenBack();
      return S_OK;
    }
    Sleep(50);
  }
  *Length = Video->GetKeyLenAv();
  //*Length = Video->GetKeyLenBack();
  return E_FAIL;
}

STDMETHODIMP CRTSPSource::WaitForSync(int *Length)
{
  CheckPointer(Length, E_POINTER);
  const CapsuleTrack* Video = Capsule.GetVideo();
  if (!Video) return E_POINTER;

  for (int i = 0; i < 300; i++)
  {
    if (Video->WasSyncPoint()) 
    {
      *Length = Video->GetKeyLenBack();
      return S_OK;
    }
    Sleep(10);
  }
  *Length = Video->GetKeyLenBack();
  return E_FAIL;
}

STDMETHODIMP CRTSPSource::GetEvents(LPOLESTR *StepName)
{
  CheckPointer(StepName, E_POINTER);

  size_t size = strlen(Capsule.LastEvent) + 1;

  *StepName = (LPOLESTR)CoTaskMemAlloc(size * sizeof(wchar_t));
  if (!*StepName) return E_OUTOFMEMORY;
  mbstowcs(*StepName, Capsule.LastEvent, size);
  (*StepName)[size-1] = 0; // make sure that string is properly ended

  return S_OK;
}

extern int LastTick;
STDMETHODIMP CRTSPSource::GetStreamHealth(int *State)
{
  CheckPointer(State, E_POINTER);
  //*Size = Capsule.GetCount(false);
  *State = (GetTickCount() - LastTick) < 2000;
  return S_OK;
}


STDMETHODIMP 
CRTSPSource::NonDelegatingQueryInterface(REFIID _riid, void **_ppv)
{
  //MyInfo("NonDelegatingQueryInterface");

  CheckPointer(_ppv, E_POINTER);
  if (_riid == IID_IFileSourceFilter)
    return GetInterface((IFileSourceFilter *) this, _ppv);
  if (_riid == IID_TestControl)
    return GetInterface((ITestControl *) this, _ppv);
  return CSource::NonDelegatingQueryInterface(_riid, _ppv);
}
