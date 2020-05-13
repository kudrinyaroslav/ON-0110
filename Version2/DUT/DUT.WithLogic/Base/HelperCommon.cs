using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace DUT.WithLogic.Base
{
    public static class HelperCommon
    {
        public static string RandomStr()
        {
            string rStr = Path.GetRandomFileName();
            rStr = rStr.Replace(".", ""); // For Removing the .
            return rStr;
        }
    }
}