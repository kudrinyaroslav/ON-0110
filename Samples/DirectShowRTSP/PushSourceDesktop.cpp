//------------------------------------------------------------------------------
// File: PushSourceDesktop.cpp
//
// Desc: DirectShow sample code - In-memory push mode source filter
//       Provides an image of the user's desktop as a continuously updating stream.
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

#include <streams.h>

#include "PushSource.h"
#include "PushGuids.h"
#include "DibHelper.h"

#define USE_NEW_DATA

//#define MyInfo(text) {MessageBox(NULL, TEXT(text), L"Info O", MB_OK | MB_TOPMOST);}
#define MyInfo(text) {}


/**********************************************
 *
 *  CPushPinDesktop Class
 *  
 *
 **********************************************/

CRTSPPinVideo::CRTSPPinVideo(HRESULT *phr, CSource *pFilter)
        : CSourceStream(NAME("ONVIF RTSP Source"), phr, pFilter, L"Out"),
        m_FramesWritten(0),
        m_bZeroMemory(0),
        m_iFrameNumber(0),
        m_rtFrameLength(FPS_5), // Capture and display desktop 5 times per second
        m_nCurrentBitDepth(32)
{
	// The main point of this sample is to demonstrate how to take a DIB
	// in host memory and insert it into a video stream. 

	// To keep this sample as simple as possible, we just read the desktop image
	// from a file and copy it into every frame that we send downstream.
    //
	// In the filter graph, we connect this filter to the AVI Mux, which creates 
    // the AVI file with the video frames we pass to it. In this case, 
    // the end result is a screen capture video (GDI images only, with no
    // support for overlay surfaces).

    // Get the device context of the main display
    HDC hDC;
    hDC = CreateDC(TEXT("DISPLAY"), NULL, NULL, NULL);

    // Get the dimensions of the main desktop window
    m_rScreen.left   = m_rScreen.top = 0;
    m_rScreen.right  = GetDeviceCaps(hDC, HORZRES);
    m_rScreen.bottom = GetDeviceCaps(hDC, VERTRES);

    // Save dimensions for later use in FillBuffer()
    m_iImageWidth  = m_rScreen.right  - m_rScreen.left;
    m_iImageHeight = m_rScreen.bottom - m_rScreen.top;

    // Release the device context
    DeleteDC(hDC);
}

CRTSPPinVideo::~CRTSPPinVideo()
{   
    DbgLog((LOG_TRACE, 3, TEXT("Frames written %d"), m_iFrameNumber));
}


//
// GetMediaType
//
// Prefer 5 formats - 8, 16 (*2), 24 or 32 bits per pixel
//
// Prefered types should be ordered by quality, with zero as highest quality.
// Therefore, iPosition =
//      0    Return a 32bit mediatype
//      1    Return a 24bit mediatype
//      2    Return 16bit RGB565
//      3    Return a 16bit mediatype (rgb555)
//      4    Return 8 bit palettised format
//      >4   Invalid
//
HRESULT CRTSPPinVideo::GetMediaType(int iPosition, CMediaType *pmt)
{
  MyInfo("GetMediaType");

#ifdef USE_NEW_DATA
  if(iPosition < 0)
    return E_INVALIDARG;
  if(iPosition > 0)
    return VFW_S_NO_MORE_ITEMS;
  LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
  if (Capsule->GetVideoFormat(pmt))
  {
    return NOERROR;
  }
  return E_INVALIDARG;
#endif

    CheckPointer(pmt,E_POINTER);
    CAutoLock cAutoLock(m_pFilter->pStateLock());

    if(iPosition < 0)
        return E_INVALIDARG;

    // Have we run off the end of types?
    if(iPosition > 4)
        return VFW_S_NO_MORE_ITEMS;

    VIDEOINFO *pvi = (VIDEOINFO *) pmt->AllocFormatBuffer(sizeof(VIDEOINFO));
    if(NULL == pvi)
        return(E_OUTOFMEMORY);

    // Initialize the VideoInfo structure before configuring its members
    ZeroMemory(pvi, sizeof(VIDEOINFO));

    switch(iPosition)
    {
        case 0:
        {    
            // Return our highest quality 32bit format

            // Since we use RGB888 (the default for 32 bit), there is
            // no reason to use BI_BITFIELDS to specify the RGB
            // masks. Also, not everything supports BI_BITFIELDS
            pvi->bmiHeader.biCompression = BI_RGB;
            pvi->bmiHeader.biBitCount    = 32;
            break;
        }

        case 1:
        {   // Return our 24bit format
            pvi->bmiHeader.biCompression = BI_RGB;
            pvi->bmiHeader.biBitCount    = 24;
            break;
        }

        case 2:
        {       
            // 16 bit per pixel RGB565

            // Place the RGB masks as the first 3 doublewords in the palette area
            for(int i = 0; i < 3; i++)
                pvi->TrueColorInfo.dwBitMasks[i] = bits565[i];

            pvi->bmiHeader.biCompression = BI_BITFIELDS;
            pvi->bmiHeader.biBitCount    = 16;
            break;
        }

        case 3:
        {   // 16 bits per pixel RGB555

            // Place the RGB masks as the first 3 doublewords in the palette area
            for(int i = 0; i < 3; i++)
                pvi->TrueColorInfo.dwBitMasks[i] = bits555[i];

            pvi->bmiHeader.biCompression = BI_BITFIELDS;
            pvi->bmiHeader.biBitCount    = 16;
            break;
        }

        case 4:
        {   // 8 bit palettised

            pvi->bmiHeader.biCompression = BI_RGB;
            pvi->bmiHeader.biBitCount    = 8;
            pvi->bmiHeader.biClrUsed     = iPALETTE_COLORS;
            break;
        }
    }

    // Adjust the parameters common to all formats
    pvi->bmiHeader.biSize       = sizeof(BITMAPINFOHEADER);
    pvi->bmiHeader.biWidth      = m_iImageWidth;
    pvi->bmiHeader.biHeight     = m_iImageHeight;
    pvi->bmiHeader.biPlanes     = 1;
    pvi->bmiHeader.biSizeImage  = GetBitmapSize(&pvi->bmiHeader);
    pvi->bmiHeader.biClrImportant = 0;

    SetRectEmpty(&(pvi->rcSource)); // we want the whole image area rendered.
    SetRectEmpty(&(pvi->rcTarget)); // no particular destination rectangle

    pmt->SetType(&MEDIATYPE_Video);
    pmt->SetFormatType(&FORMAT_VideoInfo);
    pmt->SetTemporalCompression(FALSE);

    // Work out the GUID for the subtype from the header info.
    const GUID SubTypeGUID = GetBitmapSubtype(&pvi->bmiHeader);
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);

    return NOERROR;

} // GetMediaType


//
// CheckMediaType
//
// We will accept 8, 16, 24 or 32 bit video formats, in any
// image size that gives room to bounce.
// Returns E_INVALIDARG if the mediatype is not acceptable
//
HRESULT CRTSPPinVideo::CheckMediaType(const CMediaType *pMediaType)
{
  MyInfo("CheckMediaType");

#ifdef USE_NEW_DATA
  LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
  if (Capsule->CheckVideoFormat(pMediaType))
  {
    return S_OK;
  }
  return E_INVALIDARG;
#endif

    CheckPointer(pMediaType,E_POINTER);

    if((*(pMediaType->Type()) != MEDIATYPE_Video) ||   // we only output video
        !(pMediaType->IsFixedSize()))                  // in fixed size samples
    {                                                  
        return E_INVALIDARG;
    }

    // Check for the subtypes we support
    const GUID *SubType = pMediaType->Subtype();
    if (SubType == NULL)
        return E_INVALIDARG;

    if(    (*SubType != MEDIASUBTYPE_RGB8)
        && (*SubType != MEDIASUBTYPE_RGB565)
        && (*SubType != MEDIASUBTYPE_RGB555)
        && (*SubType != MEDIASUBTYPE_RGB24)
        && (*SubType != MEDIASUBTYPE_RGB32))
    {
        return E_INVALIDARG;
    }

    // Get the format area of the media type
    VIDEOINFO *pvi = (VIDEOINFO *) pMediaType->Format();

    if(pvi == NULL)
        return E_INVALIDARG;

    // Check if the image width & height have changed
    if(    pvi->bmiHeader.biWidth   != m_iImageWidth || 
       abs(pvi->bmiHeader.biHeight) != m_iImageHeight)
    {
        // If the image width/height is changed, fail CheckMediaType() to force
        // the renderer to resize the image.
        return E_INVALIDARG;
    }

    // Don't accept formats with negative height, which would cause the desktop
    // image to be displayed upside down.
    if (pvi->bmiHeader.biHeight < 0)
        return E_INVALIDARG;

    return S_OK;  // This format is acceptable.

} // CheckMediaType


//
// DecideBufferSize
//
// This will always be called after the format has been sucessfully
// negotiated. So we have a look at m_mt to see what size image we agreed.
// Then we can ask for buffers of the correct size to contain them.
//
HRESULT CRTSPPinVideo::DecideBufferSize(IMemAllocator *pAlloc,
                                      ALLOCATOR_PROPERTIES *pProperties)
{
  MyInfo("DecideBufferSize");

    CheckPointer(pAlloc,E_POINTER);
    CheckPointer(pProperties,E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());
    HRESULT hr = NOERROR;

    VIDEOINFO *pvi = (VIDEOINFO *) m_mt.Format();
    pProperties->cBuffers = 1;
    pProperties->cbBuffer = pvi->bmiHeader.biSizeImage;
    if (pvi->bmiHeader.biSizeImage > 0)
    {
      MyInfo("Buffer not zero");
    }
    else
    {
      MyInfo("Buffer zero - bad");
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
HRESULT CRTSPPinVideo::SetMediaType(const CMediaType *pMediaType)
{
  MyInfo("SetMediaType");

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // Pass the call up to my base class
    HRESULT hr = CSourceStream::SetMediaType(pMediaType);

    if(SUCCEEDED(hr))
    {
        VIDEOINFO * pvi = (VIDEOINFO *) m_mt.Format();
        if (pvi == NULL)
            return E_UNEXPECTED;

#ifdef USE_NEW_DATA
        LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
        if (Capsule->CheckVideoFormat(pMediaType))
        {
          return S_OK;
        }
        return E_INVALIDARG;
#endif
        switch(pvi->bmiHeader.biBitCount)
        {
            case 8:     // 8-bit palettized
            case 16:    // RGB565, RGB555
            case 24:    // RGB24
            case 32:    // RGB32
                // Save the current media type and bit depth
                m_MediaType = *pMediaType;
                m_nCurrentBitDepth = pvi->bmiHeader.biBitCount;
                hr = S_OK;
                break;

            default:
                // We should never agree any other media types
                ASSERT(FALSE);
                hr = E_INVALIDARG;
                break;
        }
    } 

    return hr;

} // SetMediaType


// This is where we insert the DIB bits into the video stream.
// FillBuffer is called once for every sample in the stream.
HRESULT CRTSPPinVideo::FillBuffer(IMediaSample *pSample)
{
	BYTE *pData;
    long cbData;

    CheckPointer(pSample, E_POINTER);

    CAutoLock cAutoLockShared(&m_cSharedState);

    // Access the sample's data buffer
    pSample->GetPointer(&pData);
    cbData = pSample->GetSize();


#ifdef USE_NEW_DATA
    LiveCapsule *Capsule = ((CRTSPSource*)m_pFilter)->GetCapsule();
    if (!Capsule->PlayStream())
    {
      MyInfo("Stream NOT run");
    }
    FrameInfo *fi = Capsule->Video->GetTop();
    
    int nSize = min(fi->frameHead.FrameLen, (DWORD) cbData);
    memcpy(pData, fi->pdata, fi->frameHead.FrameLen);

    REFERENCE_TIME rtStart = fi->frameHead.Start;
    REFERENCE_TIME rtStop  = rtStart + m_rtFrameLength;

#else


    // Check that we're still using video
    ASSERT(m_mt.formattype == FORMAT_VideoInfo);

    VIDEOINFOHEADER *pVih = (VIDEOINFOHEADER*)m_mt.pbFormat;

	// Copy the DIB bits over into our filter's output buffer.
    // Since sample size may be larger than the image size, bound the copy size.
    int nSize = min(pVih->bmiHeader.biSizeImage, (DWORD) cbData);
    HDIB hDib = CopyScreenToBitmap(&m_rScreen, pData, (BITMAPINFO *) &(pVih->bmiHeader));

    if (hDib)
        DeleteObject(hDib);

	// Set the timestamps that will govern playback frame rate.
	// If this file is getting written out as an AVI,
	// then you'll also need to configure the AVI Mux filter to 
	// set the Average Time Per Frame for the AVI Header.
    // The current time is the sample's start.
    REFERENCE_TIME rtStart = m_iFrameNumber * m_rtFrameLength;
    REFERENCE_TIME rtStop  = rtStart + m_rtFrameLength;

#endif

    pSample->SetTime(&rtStart, &rtStop);
    m_iFrameNumber++;
	// Set TRUE on every sample for uncompressed frames
    pSample->SetSyncPoint(TRUE);

    return S_OK;
}



/**********************************************
 *
 *  CPushSourceDesktop Class
 *
 **********************************************/

CRTSPSource::CRTSPSource(IUnknown *pUnk, HRESULT *phr)
           : CSource(NAME("ONVIF RTSP Source"), pUnk, CLSID_PushSourceDesktop)
{
    // The pin magically adds itself to our pin array.
    m_pPin = new CRTSPPinVideo(phr, this);
/*
#ifdef USE_NEW_DATA
    if (!Capsule.OpenStream("rtsp://192.168.3.243"))
    {
      MyInfo("Stream NOT opened");
    }
    else
    {
      MyInfo("Stream opened OK");
    }
#endif
*/
	if (phr)
	{
		if (m_pPin == NULL)
			*phr = E_OUTOFMEMORY;
		else
			*phr = S_OK;
	}  
}


CRTSPSource::~CRTSPSource()
{
    delete m_pPin;
}


CUnknown * WINAPI CRTSPSource::CreateInstance(IUnknown *pUnk, HRESULT *phr)
{
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
  MyInfo("Load");

  wcscpy(FileName, inFileName);

  Capsule.OpenSettings(FileName);


  return S_OK;
  //if (mFileName[0] != 0 && m_RtspState == StateStopped)
  {	
    //char filename[1024] = {0};		
    //WideCharToMultiByte (CP_OEMCP,NULL,inFileName,-1,(LPSTR)filename,1024,NULL,FALSE);
    //int ret = m_streamMedia->rtspClientOpenStream((const char *)filename, 0);
    //if (ret < 0)
    {
      //return E_FAIL;	
    }
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

STDMETHODIMP CRTSPSource::Run(REFERENCE_TIME tStart)
{
  MyInfo("Run");
  if (!Capsule.PlayStream())
  {
    MyInfo("Stream NOT run");
  }
  else
  {
    MyInfo("Stream runned OK");
  }
  return S_OK;
}

STDMETHODIMP 
CRTSPSource::NonDelegatingQueryInterface(REFIID _riid, void **_ppv)
{
  MyInfo("NonDelegatingQueryInterface");

  CheckPointer(_ppv, E_POINTER);
/*
  if (_riid == IID_IAC3File)
    return GetInterface((IAC3File *) this, _ppv);
*/
  if (_riid == IID_IFileSourceFilter)
    return GetInterface((IFileSourceFilter *) this, _ppv);
/*
  if (_riid == IID_IAMFilterMiscFlags)
    return GetInterface((IAMFilterMiscFlags *) this, _ppv);

  if (_riid == IID_ISpecifyPropertyPages)
    return GetInterface((ISpecifyPropertyPages *) this, _ppv);
*/
  return CSource::NonDelegatingQueryInterface(_riid, _ppv);
}
