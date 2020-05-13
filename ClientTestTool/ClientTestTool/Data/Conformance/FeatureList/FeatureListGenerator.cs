///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Xml;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.Data.Conformance.FeatureList
{
  public class FeatureListGenerator
  {
    private readonly ConformanceInfo mConformanceInfo;

    public FeatureListGenerator(ConformanceInfo info)
    {
      mConformanceInfo = info;
    }

    public void Generate(String filename)
    {
      using (XmlWriter writer = XmlWriter.Create(filename))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("Datasheet");

        WriteResponsibleMemberInfo(writer);
        WriteTestExecutionInformation(writer);
        WriteClientInfo(writer);
        WriteConformanceInformation(writer);
        WriteSupportedFeaturesList(writer);
        WriteDevicesList(writer);
        WriteSupportInformation(writer);

        writer.WriteEndElement();
        writer.WriteEndDocument();
      }
    }

    private void WriteResponsibleMemberInfo(XmlWriter writer)
    {
      writer.WriteStartElement("ResponsibleMember");

      writer.WriteElementString("MemberName"   , mConformanceInfo.MemberName);
      writer.WriteElementString("MemberAddress", mConformanceInfo.MemberAddress);
      
      writer.WriteEndElement();
    }

    private void WriteTestExecutionInformation(XmlWriter writer)
    {
      writer.WriteStartElement("TestExecutionInformation");

      writer.WriteElementString("TestOperatorName"            , mConformanceInfo.TestOperatorName);
      writer.WriteElementString("ExecutingOrganizationName"   , mConformanceInfo.OrganizationName);
      writer.WriteElementString("ExecutingOrganizationAddress", mConformanceInfo.OrganizationAddress);

      writer.WriteEndElement();
    }

    private void WriteClientInfo(XmlWriter writer)
    {
      writer.WriteStartElement("ClientInformation");

      writer.WriteElementString("ProductName", mConformanceInfo.ProductName);
      writer.WriteElementString("Brand"      , mConformanceInfo.Brand);
      writer.WriteElementString("Model"      , mConformanceInfo.Model);
      writer.WriteElementString("Version"    , mConformanceInfo.Version);
      writer.WriteElementString("ProductType", mConformanceInfo.ProductType);

      writer.WriteEndElement();
    }

    private void WriteConformanceInformation(XmlWriter writer)
    {
      writer.WriteStartElement("ConformanceInformation");

      writer.WriteElementString("TestDate"   , DateTime.Now.ToLongDateString());
      writer.WriteElementString("ToolVersion", Properties.Settings.Default.Version);

      WriteClientSupportedProfiles(mConformanceInfo.ClientUnderTest, writer);

      writer.WriteEndElement();
    }

    private void WriteClientSupportedProfiles(Client client, XmlWriter writer)
    {
      writer.WriteStartElement("ClientSupportedProfiles");

      var supportedProfiles = client.GetSupportedProfiles();
      
      foreach (var profile in supportedProfiles)
        writer.WriteElementString("Profile", profile.ToString());

      writer.WriteEndElement();
    }

    private void WriteDevicesList(XmlWriter writer) // TODO remove ???
    {
      writer.WriteStartElement("DeviceUsed");

      foreach (var device in UnitSet.GetDevices())
      {
        writer.WriteStartElement("DeviceInformation");
        writer.WriteElementString("MemberName"     , device.Info.Manufacturer);
        writer.WriteElementString("ProductName"    , device.GetConformanceName());
        writer.WriteElementString("FirmwareVersion", device.Info.FirmwareVersion);

        WriteDeviceSupportedProfiles(device, writer);

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    private void WriteDeviceSupportedProfiles(Device device, XmlWriter writer)
    {
      writer.WriteStartElement("DeviceSupportedProfiles");

      var supportedProfiles = device.GetSupportedProfiles();

      foreach (var profile in supportedProfiles)
        writer.WriteElementString("Profile", profile.ToString());

      writer.WriteEndElement();
    }

    private void WriteSupportedFeaturesList(XmlWriter writer)
    {
      writer.WriteStartElement("ClientSupportedFeatures");

      var supportedFeatures =
        TestCaseSet.Instance.Tests.Where(item => TestStatus.Passed == item.Status)
          .Select(item => item.FeatureUnderTest);

      foreach (var feature in supportedFeatures)
        writer.WriteElementString("Feature", feature.GetFullName());
      
      writer.WriteEndElement();
    }

    private void WriteSupportInformation(XmlWriter writer)
    {
      writer.WriteStartElement("SupportInformation");

      writer.WriteElementString("SupportUrl"          , mConformanceInfo.TechSupportWebsite);
      writer.WriteElementString("SupportEmail"        , mConformanceInfo.TechSupportEmail);
      writer.WriteElementString("SupportPhone"        , mConformanceInfo.TechSupportPhone);
      writer.WriteElementString("InternationalAddress", mConformanceInfo.InternationalSupportAddress);
      writer.WriteElementString("RegionalAddress"     , mConformanceInfo.RegionalSupportAddress);
                                                      
      writer.WriteEndElement();
    }
  }
}
