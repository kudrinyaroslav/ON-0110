///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ClientTestTool.GUI.Utils
{
  public static class FileHelper
  {
    public static String TrimFilename(String filename)
    {
      return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), String.Empty));
    }

    /// <summary>
    /// Gets human-readable file size
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static String GetSizeString(String filename)
    {
      long byteCount = GetSize(filename);
      String[] suffixes = { "B", "KB", "MB", "GB", "TB" };

      if (0 == byteCount)
        return "0" + suffixes[0];

      long bytes = Math.Abs(byteCount);
      int  place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
      double num = Math.Round(bytes / Math.Pow(1024, place), 1);

      return (Math.Sign(byteCount) * num).ToString("0.### ") + suffixes[place];
    }

    /// <summary>
    /// Gets file size in bytes
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static long GetSize(String filename)
    {
      if (!File.Exists(filename))
        throw new FileNotFoundException();

      return new FileInfo(filename).Length;
    }
  }
}
