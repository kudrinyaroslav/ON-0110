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
// Implementation

#include "SJPEGStreamSource.h"
#include <GroupsockHelper.hh>

WISJPEGStreamSource*
WISJPEGStreamSource::createNew(UsageEnvironment& env, char const* fFileName) {
  return new WISJPEGStreamSource(env, fFileName);
}

WISJPEGStreamSource::WISJPEGStreamSource(UsageEnvironment& env, char const* fFileName)
  : JPEGVideoSource(env),
    fLastWidth(0), fLastHeight(0), FrameNumber(0) {
  this->fFileName = fFileName;
}

WISJPEGStreamSource::~WISJPEGStreamSource() {
}

void WISJPEGStreamSource::doGetNextFrame() {
  char Name[256];
  FrameNumber++;
  sprintf(Name, fFileName, FrameNumber);
  FILE* f = fopen(Name, "rb");
  if (f == NULL)
    return;
  int size = fread(fBuffer, 1, sizeof(fBuffer), f);
  fclose(f);
  timeval tm;
  gettimeofday(&fPresentationTime, NULL);
  afterGettingFrame(this, size, 0, tm, 0);
}

u_int8_t WISJPEGStreamSource::type() {
  return 1;
};

u_int8_t WISJPEGStreamSource::qFactor() {
  return 255; // indicates that quantization tables are dynamic
};

u_int8_t WISJPEGStreamSource::width() {
  return fLastWidth;
}

u_int8_t WISJPEGStreamSource::height() {
  return fLastHeight;
}

u_int8_t const* WISJPEGStreamSource::quantizationTables(u_int8_t& precision,
							   u_int16_t& length) {
  precision = 0;
  length = fLastQuantizationTableSize;
  return fLastQuantizationTable;
}
 
void WISJPEGStreamSource
::afterGettingFrame(void* clientData, unsigned frameSize,
		    unsigned numTruncatedBytes,
		    struct timeval presentationTime,
		    unsigned durationInMicroseconds) {
  WISJPEGStreamSource* source = (WISJPEGStreamSource*)clientData;
  source->afterGettingFrame1(frameSize, numTruncatedBytes,
			     presentationTime, durationInMicroseconds);
}

void WISJPEGStreamSource
::afterGettingFrame1(unsigned frameSize, unsigned numTruncatedBytes,
		     struct timeval presentationTime,
		     unsigned durationInMicroseconds) {
  fFrameSize = frameSize < fMaxSize ? frameSize : fMaxSize;
  // NOTE: Change the following if the size of the encoder's JPEG hdr changes
  unsigned const JPEGHeaderSize = 524;
  
  // Look for the "SOF0" marker (0xFF 0xC0) in the header, to get the frame
  // width and height.  Also, look for the "DQT" marker(s) (0xFF 0xDB), to
  // get the quantization table(s):
  Boolean foundSOF0 = False;
  fLastQuantizationTableSize = 0;
  for (unsigned i = 0; i < JPEGHeaderSize-8; ++i) {
    if (fBuffer[i] == 0xFF) {
      if (fBuffer[i+1] == 0xDB) { // DQT
	u_int16_t length = (fBuffer[i+2]<<8) | fBuffer[i+3];
	if (i+2 + length < JPEGHeaderSize) { // sanity check
	  u_int16_t tableSize = length - 3;
	  if (fLastQuantizationTableSize + tableSize > 128) { // sanity check
	    tableSize = 128 - fLastQuantizationTableSize;
	  }
	  memmove(&fLastQuantizationTable[fLastQuantizationTableSize],
		  &fBuffer[i+5], tableSize);
	  fLastQuantizationTableSize += tableSize;
	  if (fLastQuantizationTableSize == 128 && foundSOF0) break;
	      // we've found everything that we want
	  i += length; // skip over table
	}
      } else if (fBuffer[i+1] == 0xC0) { // SOF0
	fLastHeight = (fBuffer[i+5]<<5)|(fBuffer[i+6]>>3);
	fLastWidth = (fBuffer[i+7]<<5)|(fBuffer[i+8]>>3);
	foundSOF0 = True;
	if (fLastQuantizationTableSize == 128) break;
	    // we've found everything that we want
	i += 8;
      }
    }
  }
  if (fLastQuantizationTableSize == 64) {
    // Hack: We apparently saw only one quantization table.  Unfortunately,
    // media players seem to be unhappy if we don't send two (luma+chroma).
    // So, duplicate the existing table data:
    memmove(&fLastQuantizationTable[64], fLastQuantizationTable, 64);
    fLastQuantizationTableSize = 128;
  }
  if (!foundSOF0) envir() << "Failed to find SOF0 marker in JPEG header!\n";

  // Complete delivery to the client:
  fFrameSize = fFrameSize > JPEGHeaderSize ? fFrameSize - JPEGHeaderSize : 0;
  memmove(fTo, &fBuffer[JPEGHeaderSize], fFrameSize);
  fNumTruncatedBytes = numTruncatedBytes;
  fPresentationTime = presentationTime;
  fDurationInMicroseconds = fDurationInMicroseconds;
  FramedSource::afterGetting(this);
}
