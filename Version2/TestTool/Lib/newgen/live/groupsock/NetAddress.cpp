/**********
This library is free software; you can redistribute it and/or modify it under
the terms of the GNU Lesser General Public License as published by the
Free Software Foundation; either version 2.1 of the License, or (at your
option) any later version. (See <http://www.gnu.org/copyleft/lesser.html>.)

This library is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for
more details.

You should have received a copy of the GNU Lesser General Public License
along with this library; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA
**********/
// "mTunnel" multicast access service
// Copyright (c) 1996-2015 Live Networks, Inc.  All rights reserved.
// Network Addresses
// Implementation

#include "NetAddress.hh"
#include "GroupsockHelper.hh"

#include <stddef.h>
#include <stdio.h>

////////// NetAddress //////////

NetAddress::NetAddress()
{
  netAddressBits adr = 0;
  assign((u_int8_t const*)&adr, 4);
  fAF = AF_INET;
}

NetAddress::NetAddress(netAddressBits ipv4)
{
  assign((u_int8_t const*)&ipv4, 4);
  fAF = AF_INET;
}

NetAddress::NetAddress(int AF, u_int8_t mask)
{
  switch (AF) {
  case AF_INET:
    fLength = 4;
    fAF = AF_INET;
    break;
  case AF_INET6:
    fLength = 16;
    fAF = AF_INET6;
    break;
  default:
    fLength = 0;
    fAF = 0;
  }
  if (fData) {
    for (unsigned i = 0; i < fLength; ++i) 
      fData[i] = mask;
  } else {
    fLength = 0;
  }
}

NetAddress::NetAddress(struct in_addr const& adr)
{
  assign((u_int8_t const*)&adr, 4);
  fAF = AF_INET;
}

NetAddress::NetAddress(struct in6_addr const& adr)
{
  assign((u_int8_t const*)&adr, 16);
  fAF = AF_INET6;
}

NetAddress::NetAddress(ADDRINFO const& ai)
{
  if (ai.ai_family == PF_INET) {
    assign((u_int8_t*)(ai.ai_addr->sa_data + 2), 4);
    fAF = AF_INET;
    return;
  }
  if (ai.ai_family == PF_INET6) {
    assign((u_int8_t*)(ai.ai_addr->sa_data + 6), 16);
    fAF = AF_INET6;
    return;
  }
  fLength = 0;
  fAF = 0;
}

NetAddress::NetAddress(struct sockaddr_in const& adr)
{
  if (adr.sin_family == PF_INET) {
    assign((u_int8_t*)&(adr.sin_addr), 4);
    fAF = AF_INET;
    return;
  }
  if (adr.sin_family == PF_INET6) {
    assign((u_int8_t*)&(((sockaddr_in6*)&adr)->sin6_addr), 16);
    fAF = AF_INET6;
    return;
  }
  fLength = 0;
  fAF = 0;
}

NetAddress::NetAddress(struct sockaddr_in6 const& adr)
{
  if (adr.sin6_family == PF_INET) {
    assign((u_int8_t*)&(((sockaddr_in*)&adr)->sin_addr), 4);
    fAF = AF_INET;
    return;
  }
  if (adr.sin6_family == PF_INET6) {
    assign((u_int8_t*)&(adr.sin6_addr), 16);
    fAF = AF_INET6;
    return;
  }
  fLength = 0;
  fAF = 0;
}


NetAddress::NetAddress(NetAddress const& orig) {
  assign(orig.data(), orig.length());
  fAF = orig.fAF;
}

NetAddress& NetAddress::operator=(NetAddress const& rightSide) {
  if (&rightSide != this) {
    clean();
    assign(rightSide.data(), rightSide.length());
    fAF = rightSide.fAF;
  }
  return *this;
}

NetAddress::~NetAddress() {
  clean();
}

void NetAddress::init(u_int8_t mask)
{
  for (unsigned i = 0; i < fLength; ++i)	fData[i] = mask;
}

void NetAddress::assign(u_int8_t const* data, unsigned length) {
  if (length > 16) {
    fLength = 0;
    return;
  }

  for (unsigned i = 0; i < length; ++i)	fData[i] = data[i];
  fLength = length;
}

void NetAddress::clean() {
  fLength = 0;
}

bool NetAddress::operator==(const NetAddress& addr) const
{
  bool equal = (length() == addr.length());
  equal &= (af() == addr.af());
  if (equal) {
    for (unsigned i = 0; i < length(); ++i)	{
      equal &= (data()[i] == addr.data()[i]);
    }
  }
  return equal;
}

bool NetAddress::operator!=(const NetAddress& addr) const
{
  return !(*this == addr);
}

bool NetAddress::isNotZero() const
{
  for (unsigned int i = 0; i < fLength; i++) {
    if (fData[i]) return true;
  }
  return false;
}

bool NetAddress::isNotOne() const
{
  for (unsigned int i = 0; i < fLength; i++) {
    if (fData[i] != 0xff) return true;
  }
  return false;
}

int NetAddress::getSocket(int Type) const
{
  return socket(af(), Type, 0);
}

#ifdef HAVE_SOCKADDR_LEN
#define SET_SOCKADDR_SIN_LEN4(var) var.sin_len = sizeof var
#else
#define SET_SOCKADDR_SIN_LEN4(var)
#endif

#define MAKE_SOCKADDR_IN4(var,adr,prt) /*adr,prt must be in network order*/\
struct sockaddr_in var;             \
  var.sin_family = AF_INET;         \
  memcpy(&var.sin_addr, (adr), 4);  \
  var.sin_port = (prt);             \
  SET_SOCKADDR_SIN_LEN4(var);


#ifdef HAVE_SOCKADDR_LEN
#define SET_SOCKADDR_SIN_LEN6(var) var.sin_len = sizeof var
#else
#define SET_SOCKADDR_SIN_LEN6(var)
#endif

#define MAKE_SOCKADDR_IN6(var, adr, prt) /*adr,prt must be in network order*/\
struct sockaddr_in6 var;            \
  var.sin6_family = AF_INET6;       \
  var.sin6_flowinfo = 0;            \
  var.sin6_scope_id = 0;            \
  memcpy(&var.sin6_addr, (adr), 16);\
  var.sin6_port = (prt);            \
  SET_SOCKADDR_SIN_LEN6(var);

/*
int NetAddress::connect(int Socket, int Port) const
{
  if (fAF == AF_INET) {
    MAKE_SOCKADDR_IN4(sin, data(), Port);
    return ::connect(Socket, (struct sockaddr*) &sin, sizeof(sin));
  }
  if (fAF == AF_INET6) {
    MAKE_SOCKADDR_IN6(sin, data(), Port);
    return ::connect(Socket, (struct sockaddr*) &sin, sizeof(sin));
  }
  return -1;
}

int NetAddress::bindSocket(int Socket, int Port) const
{
  if (fAF == AF_INET) {
    MAKE_SOCKADDR_IN4(sin, data(), Port);
    return bind(Socket, (struct sockaddr*) &sin, sizeof(sin));
  }
  if (fAF == AF_INET6) {
    MAKE_SOCKADDR_IN6(sin, data(), Port);
    return bind(Socket, (struct sockaddr*) &sin, sizeof(sin));
  }
  return -1;
}
*/

////////// NetAddressList //////////

NetAddressList::NetAddressList(char const* hostname)
: fNumAddresses(0), fAddressArray(NULL) {

  ADDRINFO *aiResult, *aiIter;
  if (getaddrinfo(hostname, "", NULL, &aiResult) != 0) return;
  fNumAddresses = 0;
  aiIter = aiResult;
  while (aiIter) {
    if ((aiIter->ai_family == PF_INET) || (aiIter->ai_family == PF_INET6))
      fNumAddresses++;
    aiIter = aiIter->ai_next;
  };
  if (!fNumAddresses) {
    freeaddrinfo(aiResult);
    return;
  }
  fAddressArray = new NetAddress*[fNumAddresses];
  if (fAddressArray == NULL) {
    freeaddrinfo(aiResult);
    return;
  }

  aiIter = aiResult;
  int i = 0;
  while (aiIter) {
    if ((aiIter->ai_family == PF_INET) || (aiIter->ai_family == PF_INET6)) {
      fAddressArray[i] = new NetAddress(*aiIter);
      i++;
    }
    aiIter = aiIter->ai_next;
  };
  freeaddrinfo(aiResult);
}

NetAddressList::NetAddressList(NetAddressList const& orig) {
  assign(orig.numAddresses(), orig.fAddressArray);
}

NetAddressList& NetAddressList::operator=(NetAddressList const& rightSide) {
  if (&rightSide != this) {
    clean();
    assign(rightSide.numAddresses(), rightSide.fAddressArray);
  }
  return *this;
}

NetAddressList::~NetAddressList() {
  clean();
}

void NetAddressList::assign(unsigned numAddresses, NetAddress** addressArray) {
  fAddressArray = new NetAddress*[numAddresses];
  if (fAddressArray == NULL) {
    fNumAddresses = 0;
    return;
  }

  for (unsigned i = 0; i < numAddresses; ++i) {
    fAddressArray[i] = new NetAddress(*addressArray[i]);
  }
  fNumAddresses = numAddresses;
}

void NetAddressList::clean() {
  while (fNumAddresses-- > 0) {
    delete fAddressArray[fNumAddresses];
  }
  delete[] fAddressArray; fAddressArray = NULL;
}

NetAddress const* NetAddressList::firstAddress() const {
  if (fNumAddresses == 0) return NULL;

  return fAddressArray[0];
}

////////// NetAddressList::Iterator //////////
NetAddressList::Iterator::Iterator(NetAddressList const& addressList)
  : fAddressList(addressList), fNextIndex(0) {}

NetAddress const* NetAddressList::Iterator::nextAddress() {
  if (fNextIndex >= fAddressList.numAddresses()) return NULL; // no more
  return fAddressList.fAddressArray[fNextIndex++];
}


////////// Port //////////

Port::Port(portNumBits num /* in host byte order */) {
  fPortNum = htons(num);
}

UsageEnvironment& operator<<(UsageEnvironment& s, const Port& p) {
  return s << ntohs(p.num());
}


////////// AddressPortLookupTable //////////

AddressPortLookupTable::AddressPortLookupTable()
  : fTable(HashTable::create(3)) { // three-word keys are used
}

AddressPortLookupTable::~AddressPortLookupTable() {
  delete fTable;
}

void* AddressPortLookupTable::Add(netAddressBits address1,
				  netAddressBits address2,
				  Port port, void* value) {
  int key[3];
  key[0] = (int)address1;
  key[1] = (int)address2;
  key[2] = (int)port.num();
  return fTable->Add((char*)key, value);
}

void* AddressPortLookupTable::Lookup(netAddressBits address1,
				     netAddressBits address2,
				     Port port) {
  int key[3];
  key[0] = (int)address1;
  key[1] = (int)address2;
  key[2] = (int)port.num();
  return fTable->Lookup((char*)key);
}

Boolean AddressPortLookupTable::Remove(netAddressBits address1,
				       netAddressBits address2,
				       Port port) {
  int key[3];
  key[0] = (int)address1;
  key[1] = (int)address2;
  key[2] = (int)port.num();
  return fTable->Remove((char*)key);
}

AddressPortLookupTable::Iterator::Iterator(AddressPortLookupTable& table)
  : fIter(HashTable::Iterator::create(*(table.fTable))) {
}

AddressPortLookupTable::Iterator::~Iterator() {
  delete fIter;
}

void* AddressPortLookupTable::Iterator::next() {
  char const* key; // dummy
  return fIter->next(key);
}


////////// isMulticastAddress() implementation //////////

Boolean IsMulticastAddress(netAddressBits address) {
  // Note: We return False for addresses in the range 224.0.0.0
  // through 224.0.0.255, because these are non-routable
  // Note: IPv4-specific #####
  netAddressBits addressInNetworkOrder = htonl(address);
  return addressInNetworkOrder >  0xE00000FF &&
         addressInNetworkOrder <= 0xEFFFFFFF;
}


////////// AddressString implementation //////////

AddressString::AddressString(struct sockaddr_in const& addr) {
  init(addr.sin_addr.s_addr);
}

AddressString::AddressString(struct in_addr const& addr) {
  init(addr.s_addr);
}

AddressString::AddressString(netAddressBits addr) {
  init(addr);
}

void AddressString::init(netAddressBits addr) {
  fVal = new char[16]; // large enough for "abc.def.ghi.jkl"
  netAddressBits addrNBO = htonl(addr); // make sure we have a value in a known byte order: big endian
  sprintf(fVal, "%u.%u.%u.%u", (addrNBO>>24)&0xFF, (addrNBO>>16)&0xFF, (addrNBO>>8)&0xFF, addrNBO&0xFF);
}

AddressString::~AddressString() {
  delete[] fVal;
}


Boolean IsMulticastAddress(NetAddress address) {
  if (address.af() == AF_INET6) {
    const u_int8_t dataLow = address.data()[1] & 0x0F;
    return ((address.data()[0] == 0xFF) 
            && (address.data()[1] > 0x0F) // ff00::/16 - ff0f::/16 Reserved
            && ((dataLow == 1)            // ffx1::/16 Interface-local
                 || (dataLow == 0x2)      // ffx2::/16 Link-local
                 || (dataLow == 0x3)      // ffx3::/16 IPv4 local scope
                 || (dataLow == 0x4)      // ffx4::/16 Admin-local
                 || (dataLow == 0x5)      // ffx5::/16 Site-local
                 || (dataLow == 0x6)      // ffx8::/16 Organization-local
                 || (dataLow == 0xE)));   // ffxe::/16 Global scope
  } else if (address.af() == AF_INET) {
    netAddressBits addressIPv4;
    memcpy(&addressIPv4, address.data(), address.length());
    netAddressBits addressInHostOrder = ntohl(addressIPv4);
    return ((addressInHostOrder > 0xE00000FF) && (addressInHostOrder <= 0xEFFFFFFF));
  } else {
    return False;
  }
}

Endpoint::Endpoint(NetAddress const& adr, Port port)
{
  if (adr.af() == AF_INET) {
    struct sockaddr_in &var = *(struct sockaddr_in*)&fVar;
    var.sin_family = AF_INET;
    memcpy(&var.sin_addr, adr.data(), 4);
    var.sin_port = port.num();
    fSize = sizeof(sockaddr_in);
#ifdef HAVE_SOCKADDR_LEN
    fVar.sin_len = fSize;
#endif
    return;
  }
  if (adr.af() == AF_INET6) {
    fVar.sin6_family = AF_INET6;
    fVar.sin6_flowinfo = 0;
    fVar.sin6_scope_id = 0;
    memcpy(&fVar.sin6_addr, adr.data(), 16);
    fVar.sin6_port = port.num();
    fSize = sizeof(sockaddr_in6);
#ifdef HAVE_SOCKADDR_LEN
    fVar.sin_len = fSize;
#endif
    return;
  }
  fSize = 0;
}