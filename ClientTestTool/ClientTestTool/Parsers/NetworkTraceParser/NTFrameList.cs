///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Parsers.NetworkTraceParser
{
  /// <summary>
  /// List of Frames
  /// </summary>
  public sealed class NTFrameList : IEnumerable<Frame>
  {
    public const String DEVICE_SEPARATOR = ",";

    /// <summary>
    /// ctor
    /// </summary>
    public NTFrameList()
    {
      mFrames = new List<Frame>();
    }

    #region Properties


    public int Count
    {
      get
      {
        return mFrames.Count;
      }
    }

    #endregion

    /// <summary>
    /// Loads frames from file.
    /// </summary>
    public void Load(String filename)
    {
      
    }

    /// <summary>
    /// Get Frame with specified number
    /// </summary>
    /// <param name="frameNumber"></param>
    /// <returns></returns>
    public Frame GetFrame(int frameNumber)
    {
      if (frameNumber < 0)
        throw new ArgumentOutOfRangeException("frameNumber");

      return mFrames.FirstOrDefault(item => item.Number == frameNumber);
    }

    /// <summary>
    /// Adds Frame to FrameList
    /// </summary>
    /// <param name="frame"></param>
    public void Add(Frame frame)
    {
      if (null == frame)
        throw new ArgumentNullException("frame");

      mFrames.Add(frame);
    }

    /// <summary>
    /// Removes frame from FrameList
    /// </summary>
    /// <param name="frame"></param>
    public void Remove(Frame frame)
    {
      if (null == frame)
        throw new ArgumentNullException("frame");

      mFrames.Remove(frame);
    }

    #region IEnumerable implementation

    public IEnumerator<Frame> GetEnumerator()
    {
      return mFrames.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    #endregion

    private readonly List<Frame> mFrames;

  }
}
