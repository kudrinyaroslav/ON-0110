// pulllive.cpp : Defines the entry point for the console application.
//
#include "LiveCapsule.h"
#include <iostream>

int main(int argc, WCHAR* argv[])
{
  LiveCapsule Capsule;
  Capsule.OpenStream("rtsp://192.168.3.243");
  Capsule.PlayStream();
  
  while(1)
  {
#if 1
  FrameInfo *fi = Capsule.Video->GetTop();
  std::cout << fi->frameHead.TimeStamp << std::endl;
#else
    Sleep(100);
#endif
  }
  Capsule.StopStream();

	return 0;
}

