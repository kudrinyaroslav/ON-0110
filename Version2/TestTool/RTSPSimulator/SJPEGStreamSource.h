/*
 * Copyright (C) 2005-2006 WIS Technologies International Ltd.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and the associated README documentation file (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
// A class that encapsulates a 'FramedSource' object that
// returns JPEG video frames.
// C++ header

#ifndef  _WIS_JPEG_STREAM_SOURCE
#define _WIS_JPEG_STREAM_SOURCE

#include <JPEGVideoSource.hh>

#include <MediaSink.hh>

#define AUDIO_MAX_FRAME_SIZE 20480
#define VIDEO_MAX_FRAME_SIZE 250000
inline void setVideoRTPSinkBufferSize() { OutPacketBuffer::maxSize = VIDEO_MAX_FRAME_SIZE; }


class WISJPEGStreamSource: public JPEGVideoSource {
public:
  static WISJPEGStreamSource* createNew(UsageEnvironment& env, char const* fFileName); 

private:
  WISJPEGStreamSource(UsageEnvironment& env, char const* fFileName);
      // called only by createNew()

  virtual ~WISJPEGStreamSource();

private: // redefined virtual functions
  virtual void doGetNextFrame();

  virtual u_int8_t type();
  virtual u_int8_t qFactor();
  virtual u_int8_t width();
  virtual u_int8_t height();
  virtual u_int8_t const* quantizationTables(u_int8_t& precision,
                                             u_int16_t& length);
private:
  static void afterGettingFrame(void* clientData, unsigned frameSize,
                                unsigned numTruncatedBytes,
                                struct timeval presentationTime,
                                unsigned durationInMicroseconds);
  void afterGettingFrame1(unsigned frameSize, unsigned numTruncatedBytes,
                          struct timeval presentationTime,
                          unsigned durationInMicroseconds);

private:
  char const* fFileName;
  int FrameNumber;
  u_int8_t fLastWidth, fLastHeight; // actual dimensions /8
  u_int8_t fLastQuantizationTable[128];
  u_int16_t fLastQuantizationTableSize;
  unsigned char fBuffer[VIDEO_MAX_FRAME_SIZE];
};

#endif
