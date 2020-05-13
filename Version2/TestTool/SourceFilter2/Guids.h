///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#pragma once

#ifndef __PUSHGUIDS_DEFINED
#define __PUSHGUIDS_DEFINED

// {F37C8D57-E7F1-41b9-98D3-D98FAE3618D8}
DEFINE_GUID(CLSID_SourceFilter, 
0xf37c8d57, 0xe7f1, 0x41b9, 0x98, 0xd3, 0xd9, 0x8f, 0xae, 0x36, 0x18, 0xd8);

// {6B2DF008-ADE8-47a9-8EAB-AEAB48F631CB}
DEFINE_GUID(IID_TestControl, 
0x6b2df008, 0xade8, 0x47a9, 0x8e, 0xab, 0xae, 0xab, 0x48, 0xf6, 0x31, 0xcb);

DECLARE_INTERFACE_(ITestControl, IUnknown)
{
  STDMETHOD (RunSequence) (int Sequence) = 0;
  STDMETHOD (RunStep) (int Step) = 0;
  STDMETHOD (GetActivity) (int *Activity) = 0;
  STDMETHOD (StopActivity) () = 0;
  STDMETHOD (GetEvents)(LPOLESTR *EventText) = 0;
};


#endif


