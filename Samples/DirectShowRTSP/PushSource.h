//------------------------------------------------------------------------------
// File: PushSource.H
//
// Desc: DirectShow sample code - In-memory push mode source filter
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------


#include <strsafe.h>
#include "LiveCapsule.h"

// UNITS = 10 ^ 7  
// UNITS / 30 = 30 fps;
// UNITS / 20 = 20 fps, etc
const REFERENCE_TIME FPS_30 = UNITS / 30;
const REFERENCE_TIME FPS_20 = UNITS / 20;
const REFERENCE_TIME FPS_10 = UNITS / 10;
const REFERENCE_TIME FPS_5  = UNITS / 5;
const REFERENCE_TIME FPS_4  = UNITS / 4;
const REFERENCE_TIME FPS_3  = UNITS / 3;
const REFERENCE_TIME FPS_2  = UNITS / 2;
const REFERENCE_TIME FPS_1  = UNITS / 1;

const REFERENCE_TIME rtDefaultFrameLength = FPS_10;

#define g_wszONVIFRTSP    L"ONVIF RTSP Source Filter"

class CRTSPPinVideo : public CSourceStream
{
protected:

    int m_FramesWritten;				// To track where we are in the file
    BOOL m_bZeroMemory;                 // Do we need to clear the buffer?
    CRefTime m_rtSampleTime;	        // The time stamp for each sample

    int m_iFrameNumber;
    const REFERENCE_TIME m_rtFrameLength;

    RECT m_rScreen;                     // Rect containing entire screen coordinates

    int m_iImageHeight;                 // The current image height
    int m_iImageWidth;                  // And current image width
    int m_iRepeatTime;                  // Time in msec between frames
    int m_nCurrentBitDepth;             // Screen bit depth

    CMediaType m_MediaType;
    CCritSec m_cSharedState;            // Protects our internal state
    CImageDisplay m_Display;            // Figures out our media type for us

public:

    CRTSPPinVideo(HRESULT *phr, CSource *pFilter);
    ~CRTSPPinVideo();

    // Override the version that offers exactly one media type
    HRESULT DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pRequest);
    HRESULT FillBuffer(IMediaSample *pSample);
    
    // Set the agreed media type and set up the necessary parameters
    HRESULT SetMediaType(const CMediaType *pMediaType);

    // Support multiple display formats
    HRESULT CheckMediaType(const CMediaType *pMediaType);
    HRESULT GetMediaType(int iPosition, CMediaType *pmt);

    // Quality control
	// Not implemented because we aren't going in real time.
	// If the file-writing filter slows the graph down, we just do nothing, which means
	// wait until we're unblocked. No frames are ever dropped.
    STDMETHODIMP Notify(IBaseFilter *pSelf, Quality q)
    {
        return E_FAIL;
    }

};


class CRTSPSource : public CSource, IFileSourceFilter
{

private:
    // Constructor is private because you have to use CreateInstance
    CRTSPSource(IUnknown *pUnk, HRESULT *phr);
    ~CRTSPSource();

    CRTSPPinVideo *m_pPin;

    LiveCapsule Capsule;
    WCHAR FileName[260];
public:
    static CUnknown * WINAPI CreateInstance(IUnknown *pUnk, HRESULT *phr);  

    // IUnknown
    DECLARE_IUNKNOWN;
    STDMETHODIMP NonDelegatingQueryInterface(REFIID _riid, void **_ppv);

    // IFileSourceFilter
    STDMETHODIMP Load(LPCOLESTR inFileName, const AM_MEDIA_TYPE* inMediaType);
    STDMETHODIMP GetCurFile(LPOLESTR *_filename, AM_MEDIA_TYPE *_pmt);

    STDMETHODIMP Run(REFERENCE_TIME tStart);

    LiveCapsule* GetCapsule() { return &Capsule; };
};


