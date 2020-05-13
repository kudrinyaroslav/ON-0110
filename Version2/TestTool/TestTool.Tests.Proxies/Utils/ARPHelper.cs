using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace TestTool.Proxies.Utils
{
    public class IpNetTable
    {
        public IpNetTable(IntPtr tableBuffer)
        {
            m_TableBuffer = tableBuffer;
            Parse();
        }

        ~IpNetTable()
        {
            Marshal.FreeCoTaskMem(m_TableBuffer);
        }

        private void Parse()
        {
            if (0 == m_TableBuffer.ToInt64())
            {
                Count = 0;
                return;
            }
            // Get the length of the buffer
            Count = Marshal.ReadInt32(m_TableBuffer);
        }

        private IpNetRow GetEntryFromBuffer(int i)
        {
            if (0 <= i && i < Count)
                return (IpNetRow)Marshal.PtrToStructure(GetPtrToTableEntry(i), typeof(IpNetRow));

            return new IpNetRow();
        }

        public IpNetRow this[int index]
        {
            get { return GetEntryFromBuffer(index); }
        }

        public int Count { get; private set; }

        public IntPtr GetPtrToTableEntry(int index)
        {
            if (0 <= index && index < Count)
                return new IntPtr(m_TableBuffer.ToInt64() + sizeof(UInt32) + (index * Marshal.SizeOf(typeof(IpNetRow))));

            return IntPtr.Zero;
        }

        private IntPtr m_TableBuffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IpNetRow
    {
        [MarshalAs(UnmanagedType.U4)]
        [DefaultValue(0)]
        public UInt32 dwIndex;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dwPhysAddrLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] bPhysAddr;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dwAddr;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dwType;
    }

    public static class ARPHelper
    {
        //#region Singleton implementation

        //private static ARPHelper m_Instance;

        //static public ARPHelper Instance()
        //{ return m_Instance ?? (m_Instance = new ARPHelper()); }

        //#endregion

        //private ARPHelper()
        //{}

        private const uint ERROR_SUCCESS = 0,
                           ERROR_NO_DATA = 232;

        [DllImport("iphlpapi.dll", EntryPoint = "GetIpNetTable", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint GetIpNetTable(IntPtr pIpNetTable, [MarshalAs(UnmanagedType.U4)] ref Int32 pdwSize, Boolean bOrder);

        private static IpNetTable GetIpNetTable()
        {
            var pIpNetTable = IntPtr.Zero;
            Int32 tableSize = 0;
            var r = GetIpNetTable(IntPtr.Zero, ref tableSize, false);

            //if (ERROR_SUCCESS != r && ERROR_NO_DATA != r)
            //    throw new Exception(string.Format("GetIpNetTable call returned an error! Error code: {0}.", r));

            // Allocate new memory, make sure we free this at the end.
            pIpNetTable = Marshal.AllocCoTaskMem(tableSize);
            r = GetIpNetTable(pIpNetTable, ref tableSize, false);

            if (ERROR_NO_DATA == r)
                return new IpNetTable(IntPtr.Zero);
            if (ERROR_SUCCESS != r)
                throw new Exception(string.Format("GetIpNetTable call returned an error! Error code: {0}.", r));

            return new IpNetTable(pIpNetTable);
        }

        [DllImport("iphlpapi.dll", EntryPoint = "DeleteIpNetEntry", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint DeleteIpNetEntry(IntPtr pIpNetTable);

        public static void DeleteIpNetEntry(IPAddress addr)
        {
            var ipNetTable = GetIpNetTable();
            for (int i = 0; i < ipNetTable.Count; i++)
            {
                var ipAddr = BitConverter.ToInt32(addr.GetAddressBytes(), 0);
                if (ipNetTable[i].dwAddr == ipAddr)
                {
                    DeleteIpNetEntry(ipNetTable.GetPtrToTableEntry(i));
                    break;
                }
            }
        }

        public static void DeleteIpNetEntries(IEnumerable<IPAddress> addrs)
        {
            var ipNetTable = GetIpNetTable();
            foreach (var addr in addrs)
            {
                for (int i = 0; i < ipNetTable.Count; i++)
                {
                    var ipAddr = BitConverter.ToInt32(addr.GetAddressBytes(), 0);
                    if (ipNetTable[i].dwAddr == ipAddr)
                    {
                        DeleteIpNetEntry(ipNetTable.GetPtrToTableEntry(i));
                        break;
                    }
                }
            }
        }
    }
}
