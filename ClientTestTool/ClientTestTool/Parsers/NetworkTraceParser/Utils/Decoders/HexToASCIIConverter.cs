///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Text;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.Decoders
{
  public static class HexToASCIIConverter
  {
    public static String Convert(String hex)
    {
      var sb = new StringBuilder();
      var parts = hex.Split(':');

      foreach (String hs in parts)
      {
        uint @decimal = System.Convert.ToUInt32(hs, 16);
        char @char    = System.Convert.ToChar(@decimal);
        sb.Append(@char);
      }

      return sb.ToString();
    }
  }
}
