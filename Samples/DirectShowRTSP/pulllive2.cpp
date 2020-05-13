// pulllive.cpp : Defines the entry point for the console application.
//
//#include "LiveCapsule.h"
#include "../../Version2/TestTool/SourceFilter/LiveCapsule.h"
#include <iostream>

#ifdef DEBUG
//#pragma comment(lib,"../../Version2/TestTool/lib/debug/live.lib") 
#pragma comment(lib,"../../Version2/TestTool/lib/debug/strmbasd.lib") 
#else
//#pragma comment(lib,"../../Version2/TestTool/lib/live.lib") 
#pragma comment(lib,"../../Version2/TestTool/lib/strmbase.lib") 
#endif


int main(int argc, WCHAR* argv[])
{
  MessageLog Log;
  LiveCapsule Capsule(&Log);
  Capsule.OpenSettings(L"c:\\a.omsd");
  Capsule.PlayStream();
  
  while(1)
  {
#if 0
  FrameInfo *fi = Capsule.Video->GetTop();
  std::cout << fi->frameHead.Start << std::endl;
  if (Capsule.Audio && (Capsule.Audio->Poll() > 0))
  {
    FrameInfo *fi = Capsule.Audio->GetTop();
    std::cout << "A " << fi->frameHead.Start << std::endl;
  }
#else
    Sleep(100);
#endif
  }
  Capsule.StopStream();

	return 0;
}

