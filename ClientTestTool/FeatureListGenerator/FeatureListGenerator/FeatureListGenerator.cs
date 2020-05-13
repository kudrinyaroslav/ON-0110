using System;
using System.Xml;
using ClientTestTool.Data.Devices;
using ClientTestTool.Data.Global;
using DateTime = System.DateTime;

namespace ClientTestTool.Data.Conformance
{
  public static class FeatureListGenerator // TODO conformance values as argument
  {
    public static void Generate(String filename)
    {
      using (XmlWriter writer = XmlWriter.Create(filename))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("Datasheet");
       
        WriteClientInfo(writer);
        WriteSupportedProfilesList(writer);
        WriteDevicesList(writer);
        WriteSupportInformation(writer);

        writer.WriteEndElement();
        writer.WriteEndDocument();
      }
    }

    private static void WriteClientInfo(XmlWriter writer)
    {
      writer.WriteStartElement("ClientInformation");

      Client client = UnitSet.GetClient();

      writer.WriteElementString("ProductName" , client.Name);
      writer.WriteElementString("Brand"       , String.Empty);
      writer.WriteElementString("Model"       , String.Empty);
      writer.WriteElementString("Version"     , String.Empty);
      writer.WriteElementString("ProductType" , String.Empty);

      writer.WriteEndElement();
    }

    private static void WriteSupportedProfilesList(XmlWriter writer)
    {
      writer.WriteStartElement ("ConformanceInformation");

      writer.WriteElementString("TestDate"   , DateTime.Now.ToShortDateString());
      writer.WriteElementString("ToolVersion", Properties.Settings.Default.Version);

      writer.WriteStartElement("ClientSupportedProfile");

      writer.WriteElementString("S", String.Empty); // TODO
      writer.WriteElementString("G", String.Empty);
      writer.WriteElementString("C", String.Empty);
      writer.WriteEndElement();
      writer.WriteStartElement("ClientSupportedFeatures");
      for (int i = 0; i < 7; i++)
      {
        writer.WriteElementString("Feature", String.Empty);
      }
      writer.WriteEndElement();
    }

    private static void WriteDevicesList(XmlWriter writer) // TODO remove ???
    {
      writer.WriteStartElement("DeviceUsed");


      foreach (var device in UnitSet.GetDevices())
      {
        writer.WriteStartElement("DeviceInformation");
        writer.WriteElementString("MemberName"     , device.Manufacturer);
        writer.WriteElementString("ProductName"    , device.Name);
        writer.WriteElementString("FirmwareVersion", device.FirmwareVersion);

        writer.WriteStartElement("DeviceSupportedProfile");
        for (int i = 0; i < 3; i++)
          writer.WriteElementString("Profile", String.Empty);
        writer.WriteEndElement();

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private static void WriteSupportInformation(XmlWriter writer)
    {
      writer.WriteStartElement("SupportInformation");

      writer.WriteElementString("InternationalAddress", String.Empty);
      writer.WriteElementString("SupportUrl"  , String.Empty);
      writer.WriteElementString("SupportEmail", String.Empty);
      writer.WriteElementString("SupportPhone", String.Empty);

      writer.WriteEndElement();
    }
  }
}
