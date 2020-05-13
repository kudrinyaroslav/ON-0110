///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Devices.Definitions;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Enums;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Proxies;
using ClientTestTool.Tests.SoapValidation;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils
{
  internal static class UnitAnalyzer
  {
    private const String TAG_DEVICE_INFO = "GetDeviceInformationResponse";

    public static void ParseUnitInfo(Unit unit)
    {
      Conversation conversation = ConversationList.GetConversations(item => Equals(item.Client, unit) ||
                                                                            Equals(item.Device, unit))
          .LastOrDefault();  // last, not first :\

      if (null == conversation)
        return;

      switch (unit.Type)
      {
        case UnitType.Client:
          ParseClientInfo(conversation, unit as Client);
          break;
        case UnitType.Device:
          ParseDeviceInfo(conversation, unit as Device);
          break;
      }
    }

    private static void ParseClientInfo(Conversation conversation, Client client)
    {
      if (null == client)
        return;

      var pair = conversation.GetMessages(ContentType.Http).FirstOrDefault();

      if (null == pair)
        return;

      String name = pair.GetRequest<HttpRequest>().UserAgent;

      if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
        return;

      client.Name = name;
    }

    private static void ParseDeviceInfo(Conversation conversation, Device device)
    {
      if (null == device)
        return;

      int pairNumber;

      if (!DeviceInfoExists(conversation, out pairNumber))
        return;

      var pair = conversation.GetMessages(ContentType.Http)[pairNumber];

      try
      {
        var message =
          SoapBuilder.ParseMessage<GetDeviceInformationResponse>(
            new StringReader(pair.GetResponse<HttpMessage>().GetXmlString()), null);

        DeviceInformation deviceInfo = DeviceInformation.Create(message);

        device.SetInformation(deviceInfo);
      }
      catch (Exception e)
      {
        Logger.WriteLine(String.Format(e.Message));
      }
    }

    private static bool DeviceInfoExists(Conversation conversation, out int pairNumber)
    {
      pairNumber = -1;

      var pairs = conversation.GetMessages(ContentType.Http);

      for (int i = 0; i < pairs.Count; ++i)
        if (pairs[i].GetResponse<HttpMessage>().ContainsTag(TAG_DEVICE_INFO))
        {
          pairNumber = i;
          break;
        }

      return -1 != pairNumber;
    }
  }
}
