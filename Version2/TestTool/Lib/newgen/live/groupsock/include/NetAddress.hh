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
// C++ header

#ifndef _NET_ADDRESS_HH
#define _NET_ADDRESS_HH

#ifndef _HASH_TABLE_HH
#include "HashTable.hh"
#endif

#ifndef _NET_COMMON_H
#include "NetCommon.h"
#endif

#ifndef _USAGE_ENVIRONMENT_HH
#include "UsageEnvironment.hh"
#endif

#include <assert.h> 

// Definition of a type representing a low-level network address.
typedef u_int32_t netAddressBits;

class NetAddress {
public:
  NetAddress();
  NetAddress(netAddressBits ipv4); // outdated
  NetAddress(int AF, u_int8_t mask);
  NetAddress(NetAddress const& orig);
  NetAddress& operator=(NetAddress const& rightSide);
  virtual ~NetAddress();
  
  // AS update for IPv6
  NetAddress(ADDRINFO const& ai);
  NetAddress(struct in_addr const& adr);
  NetAddress(struct in6_addr const& adr);
  NetAddress(struct sockaddr_in const& adr);
  NetAddress(struct sockaddr_in6 const& adr);
  operator struct in_addr&() const { assert(af() == AF_INET); return *(struct in_addr*)data(); };
  operator struct in6_addr&() const { assert(af() == AF_INET6); return *(struct in6_addr*)data(); };
  bool operator==(NetAddress const & addr) const;
  bool operator!=(NetAddress const& addr) const;
  void init(u_int8_t mask);
  bool isNotZero() const;
  bool isNotOne() const;
  int af() const { return fAF;};
  int getSocket(int Type) const;
  //int connect(int Socket, int Port) const;
  //int bindSocket(int Socket, int Port = 0) const;
  // AS update for IPv6 end

  unsigned length() const { return fLength; }
  u_int8_t const* data() const // always in network byte order
  { return fData; }
  
private:
  void assign(u_int8_t const* data, unsigned length);
  void clean();
  
  unsigned fLength;
  u_int8_t fData[16];
  int fAF;
};

class NetAddressList {
public:
  NetAddressList(char const* hostname);
  NetAddressList(NetAddressList const& orig);
  NetAddressList& operator=(NetAddressList const& rightSide);
  virtual ~NetAddressList();
  
  unsigned numAddresses() const { return fNumAddresses; }
  
  NetAddress const* firstAddress() const;
  
  // Used to iterate through the addresses in a list:
  class Iterator {
  public:
    Iterator(NetAddressList const& addressList);
    NetAddress const* nextAddress(); // NULL iff none
  private:
    NetAddressList const& fAddressList;
    unsigned fNextIndex;
  };
  
private:
  void assign(u_int32_t numAddresses, NetAddress** addressArray);
  void clean();
  
  friend class Iterator;
  unsigned fNumAddresses;
  NetAddress** fAddressArray;
};

typedef u_int16_t portNumBits;

class Port {
public:
  Port(portNumBits num /* in host byte order */);
  
  portNumBits num() const { return fPortNum; } // in network byte order
  
private:
  portNumBits fPortNum; // stored in network byte order
#ifdef IRIX
  portNumBits filler; // hack to overcome a bug in IRIX C++ compiler
#endif
};

UsageEnvironment& operator<<(UsageEnvironment& s, const Port& p);


// A generic table for looking up objects by (address1, address2, port)
class AddressPortLookupTable {
public:
  AddressPortLookupTable();
  virtual ~AddressPortLookupTable();
  
  void* Add(netAddressBits address1, netAddressBits address2, Port port, void* value);
      // Returns the old value if different, otherwise 0
  Boolean Remove(netAddressBits address1, netAddressBits address2, Port port);
  void* Lookup(netAddressBits address1, netAddressBits address2, Port port);
      // Returns 0 if not found
  void* RemoveNext() { return fTable->RemoveNext(); }

  // Used to iterate through the entries in the table
  class Iterator {
  public:
    Iterator(AddressPortLookupTable& table);
    virtual ~Iterator();
    
    void* next(); // NULL iff none
    
  private:
    HashTable::Iterator* fIter;
  };
  
private:
  friend class Iterator;
  HashTable* fTable;
};


Boolean IsMulticastAddress(netAddressBits address);
Boolean IsMulticastAddress(NetAddress address);

// A mechanism for displaying an IPv4 address in ASCII.  This is intended to replace "inet_ntoa()", which is not thread-safe.
class AddressString {
public:
  AddressString(struct sockaddr_in const& addr);
  AddressString(struct in_addr const& addr);
  AddressString(netAddressBits addr); // "addr" is assumed to be in host byte order here

  virtual ~AddressString();

  char const* val() const { return fVal; }

private:
  void init(netAddressBits addr); // used to implement each of the constructors

private:
  char* fVal; // The result ASCII string: allocated by the constructor; deleted by the destructor
};

class Endpoint {
public:
  Endpoint(NetAddress const& adr, Port port);
  struct sockaddr const* adr() const { return (struct sockaddr const*)&fVar; };
  int size() { return fSize;};
private:
  sockaddr_in6 fVar;
  int fSize;
};

#endif
