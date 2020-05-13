using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.WithLogic.Proxy;

namespace DUT.WithLogic.Engine
{
    public static class DUTDataTime
    {
        public static Proxy.DateTime GetCurrentTime(long offset)
        {
            System.DateTime internalTime = System.DateTime.UtcNow.AddTicks(offset);

            Proxy.DateTime res = new Proxy.DateTime();
            res.Time = new Proxy.Time();
            res.Time.Hour = internalTime.Hour;
            res.Time.Minute = internalTime.Minute;
            res.Time.Second = internalTime.Second;
            
            res.Date = new Proxy.Date();
            res.Date.Day = internalTime.Day;
            res.Date.Month = internalTime.Month;
            res.Date.Year = internalTime.Year;

            return res;

        }

        public static long GetOffset(Proxy.DateTime dataTime)
        {
            System.DateTime internalTime = new System.DateTime(dataTime.Date.Year, dataTime.Date.Month, dataTime.Date.Day, dataTime.Time.Hour, dataTime.Time.Minute, dataTime.Time.Second);
            return internalTime.Ticks - System.DateTime.UtcNow.Ticks;

        }
            
    }
}