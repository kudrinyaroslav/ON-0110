///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Data.Utils;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Parsers.Frames.Enums;
using ClientTestTool.Parsers.Interfaces;

namespace ClientTestTool.Parsers.Frames
{
  public sealed class FrameLoader : IFrameProvider
  {
    public FrameLoader(Conversation conversation, Frame frame, MessageType type)
    {
      if (null == conversation)
        throw new ArgumentNullException("conversation");

      if (null == frame)
        throw new ArgumentNullException("frame");

      mFolder   = FrameHelper.GetFrameFolder(conversation, frame, type);
      mFilename = Path.GetFileName(new DirectoryInfo(mFolder).Name);
    }

    public String GetContent(ContentType type)
    {
      switch (type)
      {
        case ContentType.Http:
          return LoadString(GetHttpFilename());
        case ContentType.Rtsp:
          return LoadString(GetRtspFilename());
        default:
          throw new ArgumentOutOfRangeException("type");
      }
    }

    public String GetXmlString(XmlNamespaceOption option)
    {
      try
      {
        XElement doc = XElement.Load(GetXmlFilename());

        switch (option)
        {
          case XmlNamespaceOption.None:
            return doc.ToString();

          case XmlNamespaceOption.IgnoreNamespaces:
            return XmlUtil.RemoveAllNamespaces(doc).ToString();

          default:
            return doc.ToString();
        }
      }
      catch (XmlException e)
      {
        Logger.WriteLine(e.Message);
        return String.Empty;
      }
    }

    #region Helpers

    public bool ContainsXml()
    {
      String[] xmlFiles = Directory.GetFiles(mFolder, String.Format("*.{0}", CTTSettings.EXTENSION_XML));

      return 0 != xmlFiles.Length && xmlFiles.All(item => 0 != new FileInfo(item).Length);
    }

    private String LoadString(String filename)
    {
      if (!File.Exists(filename))
        return String.Empty;

      return File.ReadAllText(filename);
    }

    private String GetHttpFilename()
    {
      return GetFilename(CTTSettings.PROTOCOL_HTTP);
    }

    private String GetXmlFilename()
    {
      return GetFilename(CTTSettings.EXTENSION_XML);
    }

    private String GetRtspFilename()
    {
      return GetFilename(CTTSettings.PROTOCOL_RTSP);
    }

    private String GetFilename(String extension)
    {
      return Path.Combine(mFolder, String.Join(".", mFilename, extension));
    }

    #endregion

    private readonly String mFolder;
    private readonly String mFilename;

  }
}
