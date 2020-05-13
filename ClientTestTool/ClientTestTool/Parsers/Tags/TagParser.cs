///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClientTestTool.Parsers.Tags
{
  public sealed class TagParser
  {
    private readonly String mFilename;

    public TagParser(String filename)
    {
      mFilename = filename;
    } 

    public List<String> TagsForFrame(int frameNumber)
    {
      List<String> tags = File.ReadAllLines(mFilename).ToList();
      int indexOfFrame = tags.FindIndex(item => frameNumber.ToString(CultureInfo.InvariantCulture) == item.Trim());

      if (-1 == indexOfFrame)
        return null;

      tags.RemoveRange(0, indexOfFrame + 1);

      int indexOfNextFrame = tags.FindIndex(item =>
                                            {
                                              int n;
                                              return int.TryParse(item, out n);
                                            });

      if (-1 != indexOfNextFrame)
      tags.RemoveRange(indexOfNextFrame, tags.Count - indexOfNextFrame);

      return tags;
    }

    public int FrameIndexOf(String xmlTag)
    {
      String[] tags = File.ReadAllLines(mFilename);

      int currentFrameNumber = -1;
      foreach (String tag in tags)
      {
        if (Regex.IsMatch(tag, @"\d") && !tag.Contains("<"))
          currentFrameNumber = int.Parse(tag.Trim(':'));
        if (tag.Contains(xmlTag))
          return currentFrameNumber;
      }

      return -1;
    }
  }
}
