///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#pragma once

#ifndef __PUSHGUIDS_DEFINED
#define __PUSHGUIDS_DEFINED

// {547D37D7-1451-414e-BF20-706F38963B16}
DEFINE_GUID(CLSID_SourceFilter, 
0x547d37d7, 0x1451, 0x414e, 0xbf, 0x20, 0x70, 0x6f, 0x38, 0x96, 0x3b, 0x16);


// {669BB843-E7C6-4bff-86A6-501BAA2D6A16}
DEFINE_GUID(IID_TestControl, 
0x669bb843, 0xe7c6, 0x4bff, 0x86, 0xa6, 0x50, 0x1b, 0xaa, 0x2d, 0x6a, 0x16);


DECLARE_INTERFACE_(ITestControl, IUnknown)
{
  // Logs
  //STDMETHOD (PollLog) (LPOLESTR *_filename) = 0;
  //STDMETHOD (TrySetParameter) (int* Ok) = 0;

  STDMETHOD (ControlStream) (int Control) = 0;

  //STDMETHOD (GetCurrentStep) (int* Step, int* PassBits, int* FailBits) = 0;
  //STDMETHOD (GetStepName)(int Step, LPOLESTR *StepName) = 0;

  STDMETHOD (WaitForStableKey) (double *Length) = 0;
  STDMETHOD (WaitForSync) (int *Length) = 0;

  STDMETHOD (GetEvents)(LPOLESTR *StepName) = 0;
  STDMETHOD (GetStreamHealth)(int *State) = 0;
};


#endif


