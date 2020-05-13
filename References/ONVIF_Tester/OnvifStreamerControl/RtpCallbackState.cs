/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Onvif
{

    public class RtpStats
    {
        const int MAX_DROPOUT = 3000;
        const int MAX_MISORDER = 100;
        const int MIN_SEQUENTIAL = 2;
        const UInt16 One = 1;
        const UInt32 SEQ_MOD = 0x10000;
        public UInt16 BaseSeq;
        public UInt16 MaxSeq;
        public UInt32 BadSeq;
        public UInt32 Cycles;
        public UInt32 Probation;
        public UInt32 Received;
        private Int32 _ssrc;
        private UInt32 _receivedPrior;
        private UInt32 _expectedPrior;
        private long _lostSinceLastCheckFraction;
        public byte RecentPacketLostFraction
        {
            //this must be checked after a call to PacketsLost for accurate value
            get
            {
                return (byte)_lostSinceLastCheckFraction;
            }
        }
        public Int32 SSRC
        {
            get
            {
                return _ssrc;
            }
        }

        public long PacketsLost
        {
            get
            {
                long maxSeq = Cycles + MaxSeq;
                long expected = maxSeq - BaseSeq + 1;

                long lost = expected - Received;

                long expectedSinceLastCheck = expected - _expectedPrior;
                _expectedPrior = (UInt32)expected;
                long receivedSinceLastCheck = Received - _receivedPrior;
                _receivedPrior = Received;
                long lostSinceLastCheck = expectedSinceLastCheck - receivedSinceLastCheck;

                if (expectedSinceLastCheck == 0 || lostSinceLastCheck <= 0)
                    _lostSinceLastCheckFraction = 0;
                else
                    _lostSinceLastCheckFraction = (lostSinceLastCheck << 8) / expectedSinceLastCheck;

                //clamp value to 24 bits no rolloever per RFC 3550
                if (lost < -8388608) lost = -8388608;
                if (lost > 8388607) lost = 8388607;
                return lost;
            }
        }

        public UInt32 ExtendedMaxSequence
        {
            get
            {
                return (Cycles | MaxSeq);
            }
        }

        public void InitSequence(Int32 ssrc, ushort seq)
        {
            _ssrc = ssrc;
            ResetSequence(seq);
            MaxSeq = (UInt16)(seq - 1);
            Probation = MIN_SEQUENTIAL;
        }

        public void ResetSequence(UInt16 seq)
        {
            BaseSeq = seq;
            MaxSeq = seq;
            BadSeq = SEQ_MOD + 1; /* so seq == bad_seq is false */
            Cycles = 0;
            Received = 0;
            _receivedPrior = 0;
            _expectedPrior = 0;
        }

        public int UpdateSequence(UInt16 seq)
        {
            UInt16 udelta = (UInt16)(seq - MaxSeq);

            /*
             * Source is not valid until MIN_SEQUENTIAL packets with
             * sequential sequence numbers have been received.
             */
            if (Probation > 0)
            {
                /* packet is in sequence */
                if (seq == MaxSeq + 1)
                {
                    Probation--;
                    MaxSeq = seq;
                    if (Probation == 0)
                    {
                        ResetSequence(seq);
                        Received++;
                        return 1;
                    }
                }
                else
                {
                    Probation = MIN_SEQUENTIAL - 1;
                    MaxSeq = seq;
                }
                return 0;
            }
            else if (udelta < MAX_DROPOUT)
            {
                /* in order, with permissible gap */
                if (seq < MaxSeq)
                {
                    /*
                     * Sequence number wrapped - count another 64K cycle.
                     */
                    Cycles += SEQ_MOD;
                }
                MaxSeq = seq;
            }
            else if (udelta <= SEQ_MOD - MAX_MISORDER)
            {
                /* the sequence number made a very large jump */
                if (seq == BadSeq)
                {
                    /*
                     * Two sequential packets -- assume that the other side
                     * restarted without telling us so just re-sync
                     * (i.e., pretend this was the first packet).
                     */
                    ResetSequence(seq);
                }
                else
                {
                    BadSeq = (UInt32)((seq + 1) & (SEQ_MOD - 1));
                    return 0;
                }
            }
            else
            {
                /* duplicate or reordered packet */
            }
            Received++;
            return 1;
        }


    }

    class RtpCallbackState
    {
        public UdpClient RtpClient;
        public UdpClient RtcpClient;
        public ushort LastSequence;
        public RtpStats RtpStats = new RtpStats();
        public Guid ClientSSRC = Guid.NewGuid();
        public double TransitJitter;
        public long LastTransit;
    }
}
