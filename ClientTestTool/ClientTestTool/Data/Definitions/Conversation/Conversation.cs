///
/// @Author Matthew Tuusberg
///

﻿  using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ClientTestTool.Data.Definitions.Conversation.Base;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Global;

namespace ClientTestTool.Data.Definitions.Conversation
{
  public sealed class Conversation : BaseConversation, IXmlSerializable
  {
    public Conversation(NetworkTraceInfo trace, Unit client, Unit device) : base (client, device)
    {
      FoundInTraces = new HashSet<NetworkTraceInfo>
      {
        trace
      };
    }

    #region Properties

    public String Name
    {
      get
      {
        if (!UnitSet.Contains(mClient))
        {
          var system = UnitSet.GetSystems().First(item => item.Clients.Contains(mClient));
          return String.Format("{0} - {1}", system.Name, mDevice.Name);
        }

        return String.Format("{0} - {1}", mClient.Name, mDevice.Name);
      }
    }

    public HashSet<NetworkTraceInfo> FoundInTraces
    {
      get;
      private set;
    }

    public bool IsEmpty
    {
      get
      {
        return !mRequestResponseList.Any();
      }
    }

    public bool IsIgnored
    {
      get
      {
        return mClient.IsIgnored || mDevice.IsIgnored;
      }
    }

    public Unit Client
    {
      get
      {
        return mClient;
      }
    }

    public Unit Device
    {
      get
      {
        return mDevice;
      }
    }

    #endregion

    #region List Manipulation

    public List<RequestResponsePair> GetMessages()
    {
      return mRequestResponseList.ToList();
    }

    private List<RequestResponsePair> GetMessages(Func<RequestResponsePair, bool> predicate)
    {
      return mRequestResponseList.Where(predicate).ToList();
    }

    public List<RequestResponsePair> GetMessages(ContentType type)
    {
      return GetMessages(item => item.ContentType == type);
    }

    public BaseMessage GetRequest(int index)
    {
      if (index < 0 || index >= mRequestResponseList.Count)
        throw new ArgumentOutOfRangeException();

      return mRequestResponseList[index].Request;
    }

    public T GetRequest<T>(int index) where T : BaseMessage
    {
      if (index < 0 || index >= mRequestResponseList.Count)
        throw new ArgumentOutOfRangeException();

      return mRequestResponseList[index].GetRequest<T>();
    }

    public BaseMessage GetResponse(int index)
    {
      if (index < 0 || index >= mRequestResponseList.Count)
        throw new ArgumentOutOfRangeException();

      return mRequestResponseList[index].Response;
    }

    public T GetResponse<T>(int index) where T : BaseMessage
    {
      if (index < 0 || index >= mRequestResponseList.Count)
        throw new ArgumentOutOfRangeException();

      return mRequestResponseList[index].GetResponse<T>();
    }

    public int IndexOf(RequestResponsePair pair)
    {
      return mRequestResponseList.IndexOf(pair);
    }

    public void RemoveConnectedMessages(NetworkTraceInfo trace)
    {
      mRequestResponseList.RemoveAll(item => item.FoundInTrace == trace);
    }

    #endregion 

    #region IXmlSerializable implementation

    public XmlSchema GetSchema()
    {
      return null;
    }

    public void ReadXml(XmlReader reader)
    {}

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("Conversation");
      writer.WriteAttributeString("Name", Name);
      //writer.WriteAttributeString("FoundInTrace", FoundInTrace.Filename);

      foreach (var pair in mRequestResponseList)
      {
        writer.WriteStartElement("Pair");
        writer.WriteAttributeString("Number", mRequestResponseList.IndexOf(pair).ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("ContentType", pair.ContentType.ToString());

        writer.WriteStartElement("Request");
        writer.WriteElementString("Frame", pair.Request.FrameNumber.ToString(CultureInfo.InvariantCulture));
        writer.WriteElementString("Details", pair.Request.GetDetails());
        writer.WriteEndElement();

        writer.WriteStartElement("Response");
        writer.WriteElementString("Frame", pair.Response.FrameNumber.ToString(CultureInfo.InvariantCulture));
        writer.WriteElementString("Details", pair.Response.GetDetails());
        writer.WriteEndElement();

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    #endregion
  }
}
