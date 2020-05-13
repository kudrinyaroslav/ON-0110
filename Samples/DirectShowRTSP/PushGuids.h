//------------------------------------------------------------------------------
// File: PushGuids.h
//
// Desc: DirectShow sample code - GUID definitions for PushSource filter set
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

#pragma once

#ifndef __PUSHGUIDS_DEFINED
#define __PUSHGUIDS_DEFINED

#if 1

// {547D37D7-1451-414e-BF20-706F38963B16}
DEFINE_GUID(CLSID_PushSourceDesktop, 
0x547d37d7, 0x1451, 0x414e, 0xbf, 0x20, 0x70, 0x6f, 0x38, 0x96, 0x3b, 0x16);

#else
// {4EA6930A-2C8A-4ae6-A561-56E4B5044437}
DEFINE_GUID(CLSID_PushSourceDesktop, 
0x4ea6930a, 0x2c8a, 0x4ae6, 0xa5, 0x61, 0x56, 0xe4, 0xb5, 0x4, 0x44, 0x37);
#endif
#endif


