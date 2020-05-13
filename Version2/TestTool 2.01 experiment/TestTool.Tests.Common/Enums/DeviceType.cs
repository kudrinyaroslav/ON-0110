using System;

namespace TestTool.Tests.Common.Enums
{
    /// <summary>
    /// Devices types enumeration 
    /// </summary>
    [FlagsAttribute]
    public enum DeviceType
    {
        None = 0,
        NVT = 1,
        NVS = 2,
        NVD = 4,
        NVA = 8

    }


}