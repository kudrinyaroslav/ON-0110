///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Definitions.Worker;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Parsers.Interfaces;
using ClientTestTool.Parsers.NetworkTraceParser.Extensions;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Parsers.NetworkTraceParser.Base
{
  public abstract class BaseNTParser : Worker, ITraceParser
  {
    protected BaseNTParser(NetworkTraceInfo networkTrace, ApplicationState state) : base(state)
    {
      NetworkTrace = networkTrace;
      FrameList = new NTFrameList();
    }

    #region Parser Logic

    protected abstract void ParseFrameList();
    protected abstract void Parse();

    public override void Run()
    {
      Run(() =>
      {
        Parse();
        CheckConversations();
        RemoveUnusedUnits();
      });
    }

    #endregion

    #region Properties

    /// <summary>
    /// Network Trace
    /// </summary>
    public NetworkTraceInfo NetworkTrace
    {
      get;
      private set;
    }

    /// <summary>
    /// Frames found in the network trace
    /// </summary>
    public NTFrameList FrameList
    {
      get;
      private set;
    }

    /// <summary>
    /// The output folder.
    /// </summary>
    public String OutputFolder
    {
      get;
      private set;
    }

    /// <summary>
    /// The conversations folder.
    /// </summary>
    public String ConversationsFolder
    {
      get;
      private set;
    }

    #endregion

    #region Logging

    private String mBaseLogMessage;

    protected override void SetStatusMessage(String status)
    {
      if (null == mBaseLogMessage)
        mBaseLogMessage = String.Format("Processing {0}:", NetworkTrace.Filename);

      base.SetStatusMessage(String.Join(" ", mBaseLogMessage, status));
    }

    #endregion

    #region Helpers

    //TODO
    private static void RemoveUnusedUnits()
    {
      UnitSet.RemoveAll(unit => !ConversationList.Any(conversation => conversation.Client == unit ||
                                                                      conversation.Device == unit));
    }

    //TODO
    private void CheckConversations()
    {
      if (0 == ConversationList.GetConversations(NetworkTrace).Count)
        DialogHelper.ShowError(NetworkTrace.Filename, "Communication was not detected between Client and Device using ONVIF standard.\r\nConversation list cannot be generated.");
    }

    #region Frame List

    private String GetFrameListFilename()
    {
      return Path.Combine(OutputFolder, String.Join(".", FILE_FRAMELIST, CTTSettings.EXTENSION_TXT));
    }

    protected void CreateFrameList(IEnumerable<String> lines)
    {
      foreach (String line in lines)
      {
        String[] values = line.Split(TSharkHelper.OUTPUT_SEPARATOR);
        int frameNumber = int.Parse(values[0]);

        Frame frame = new Frame(frameNumber,
                                NetworkTrace,
                                values[1],
                                values[2],
                                values[3],
                                values[4],
                                values[5]);

        FrameList.Add(frame);
      }
    }

    protected void SaveFrameList()
    {
      var frameList = FrameList.Select(item =>
      String.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}{1}{6}", item.Number, TSharkHelper.OUTPUT_SEPARATOR,
                                                         item.SourceMac, item.DestinationMac,
                                                         item.SourceIp, item.DestinationIp, item.Protocol));

      File.WriteAllLines(GetFrameListFilename(), frameList);
    }

    protected void LoadFrameList(String filename)
    {
      if (String.IsNullOrEmpty(filename))
        throw new ArgumentNullException("filename");

      var reader = new StreamReader(filename);
      CreateFrameList(reader.ReadAllLines());
    }

    #endregion 

    protected void CreateOutputDirectory()
    {
      String path = Path.Combine(CTTSettings.GetOutputDir(), NetworkTrace.Filename);

      if (Directory.Exists(path))
      {
        String[] directories = Directory.GetDirectories(Directory.GetParent(path).FullName);
        int nDir = 1;
        directories.ToList().ForEach(directory =>
        {
          if (Regex.IsMatch(directory, String.Format(@"{0}_\d", NetworkTrace.Filename)))
          {
            int nCurDir = int.Parse(directory.Substring(directory.LastIndexOf("_", StringComparison.Ordinal) + 1));
            if (nCurDir == nDir)
              ++nDir;
          }
        });

        path = String.Join("_", path, nDir);
      }

      OutputFolder        = Directory.CreateDirectory(path).FullName;
      ConversationsFolder = Directory.CreateDirectory(Path.Combine(OutputFolder, "conversations")).FullName;
    }

    private const String FILE_FRAMELIST = @"framelist";

    #endregion
  }
}
