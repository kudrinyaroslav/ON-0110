///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ClientTestTool.Data.Conformance.DoC.Base.Pdf;
using ClientTestTool.Data.Conformance.Extensions;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Utils;

namespace ClientTestTool.Data.Conformance.DoC.Base
{
  public abstract class BaseDoCGenerator : PdfRenderer
  {
    protected BaseDoCGenerator(ConformanceInfo info, String templatePath) : base(templatePath)
    {
      Info = info;
    }

    public ConformanceInfo Info
    {
      get;
      private set;
    }

    public abstract void Generate(String filename);

    #region Helpers

    protected void ReplaceIds(XElement doc, ConformanceInfo info)
    {
      ReplaceElementValue(doc, ID_MEMBER_NAME                  , info.MemberName);
      ReplaceElementValue(doc, ID_MEMBER_ADDRESS               , info.MemberAddress);
      ReplaceElementValue(doc, ID_SUPPORTED_PROFILES           , info.GetSupportedProfilesString());
      ReplaceElementValue(doc, ID_CTT_VERSION                  , Properties.Settings.Default.Version);
      ReplaceElementValue(doc, ID_PRODUCT_NAME                 , info.ProductName);
      ReplaceElementValue(doc, ID_CLIENT_FEATURES              , info.GetSupportedFeaturesString());
      ReplaceElementValue(doc, ID_BRAND                        , info.Brand);
      ReplaceElementValue(doc, ID_MODEL                        , info.Model);
      ReplaceElementValue(doc, ID_VERSION                      , info.Version);
      ReplaceElementValue(doc, ID_PRODUCT_TYPE                 , info.ProductType);
      ReplaceElementValue(doc, ID_OTHER_INFO                   , info.OtherInformation);
      ReplaceElementValue(doc, ID_GENERAL_INFO                 , info.InternationalSupportAddress);
      ReplaceElementValue(doc, ID_TEST_OPERATOR                , info.TestOperatorName);
      ReplaceElementValue(doc, ID_EXECUTING_ORG_NAME           , info.OrganizationName);
      ReplaceElementValue(doc, ID_EXECUTING_ORG_ADDRESS        , info.MemberAddress);
      ReplaceElementValue(doc, ID_TECH_SUPPORT_WEBSITE         , info.TechSupportWebsite);
      ReplaceElementValue(doc, ID_TECH_SUPPORT_EMAIL           , info.TechSupportEmail);
      ReplaceElementValue(doc, ID_TECH_SUPPORT_PHONE           , info.TechSupportPhone);
      ReplaceElementValue(doc, ID_INTERNATIONAL_SUPPORT_ADDRESS, info.InternationalSupportAddress);
      ReplaceElementValue(doc, ID_REGIONAL_SUPPORT_ADDRESS     , info.RegionalSupportAddress);
    }

    protected void AddDevicesList(XElement doc)
    {
      String headerId = GetIdWithPostfix(ID_DEVICE_TABLE, 1);

      var deviceTableHeaderTemplate = XmlUtil.GetElementById(doc, headerId);
      var deviceTableTemplate = deviceTableHeaderTemplate.NextNode as XElement;

      XElement lastElement = deviceTableTemplate;

      if (null == deviceTableTemplate)
        throw new XmlException("device table does not exist");

      var devices = UnitSet.GetDevices();
      int count = 3 > devices.Count ? 3 : devices.Count;

      for (int i = 1; i < count; ++i)
      {
        var newHeader = new XElement(deviceTableHeaderTemplate);
        var newTable = new XElement(deviceTableTemplate);

        ProcessDeviceHeader(newHeader, i + 1);
        ProcessDeviceTable(newTable, i + 1);

        lastElement.AddAfterSelf(newHeader);
        newHeader.AddAfterSelf(newTable);

        lastElement = newTable;
      }
    }

    protected void FillDevicesList(XElement doc)
    {
      var devices = UnitSet.GetDevices();

      for (int i = 0; i < devices.Count; ++i)
      {
        var device = devices[i];
        int deviceNumber = i + 1;

        ReplaceElementValue(doc, GetIdWithPostfix(ID_DEVICE_PRODUCT , deviceNumber), device.GetConformanceName());
        ReplaceElementValue(doc, GetIdWithPostfix(ID_DEVICE_MEMBER  , deviceNumber), device.Info.Manufacturer    ?? String.Empty);
        ReplaceElementValue(doc, GetIdWithPostfix(ID_DEVICE_FIRMWARE, deviceNumber), device.Info.FirmwareVersion ?? String.Empty);
        ReplaceElementValue(doc, GetIdWithPostfix(ID_DEVICE_PROFILE , deviceNumber), String.Join(", ", device.GetSupportedProfiles()));
      }
    }

    private void ProcessDeviceHeader(XElement headerElement, int deviceNumber)
    {
      XmlUtil.ReplaceId(headerElement, GetIdWithPostfix(ID_DEVICE_TABLE, 1), GetIdWithPostfix(ID_DEVICE_TABLE, deviceNumber));

      String newHeaderValue = headerElement.Value; //Reference ONVIF Device used (Device 1)
      headerElement.Value = newHeaderValue.Replace("Device 1", String.Format("Device {0}", deviceNumber));
    }

    private void ProcessDeviceTable(XElement tableElement, int deviceNumber)
    {
      String[] ids = { ID_DEVICE_MEMBER, ID_DEVICE_PRODUCT, ID_DEVICE_PROFILE, ID_DEVICE_FIRMWARE };

      foreach (String id in ids)
        XmlUtil.ReplaceId(tableElement, GetIdWithPostfix(id, 1), GetIdWithPostfix(id, deviceNumber));
    }

    protected void ReplaceElementValue(XElement doc, String elementId, String replacement)
    {
      var element = XmlUtil.GetElementById(doc, elementId);

      if (null == element)
        return;

      element.Value = replacement;
    }

    protected void ReplaceAttributeValue(XElement doc, String elementId, String attributeName, String replacement)
    {
      var element = XmlUtil.GetElementById(doc, elementId);

      if (null == element)
        return;

      var attribute = element.Attribute(attributeName);

      if (null == attribute)
        return;

      attribute.Value = replacement;
    }

    protected String GetIdWithPostfix(String id, int number)
    {
      return String.Format("{0}_{1}", id, number.ToString("D2"));
    }

    #endregion

    #region ID's

    protected const String ID_ONVIF_LOGO                  = "onvifLogo";

    private const String ID_DEVICE_TABLE                  = "tableTitle_deviceUsed";
    private const String ID_DEVICE_MEMBER                 = "deviceMemberName";
    private const String ID_DEVICE_PRODUCT                = "deviceProductName";
    private const String ID_DEVICE_PROFILE                = "deviceProfileSupported";
    private const String ID_DEVICE_FIRMWARE               = "deviceProductFirmwareVersion";    
    private const String ID_MEMBER_NAME                   = "memberName";
    private const String ID_MEMBER_ADDRESS                = "memberAddress";
    private const String ID_SUPPORTED_PROFILES            = "supportedProfiles";
    private const String ID_CTT_VERSION                   = "cttVersion";
    private const String ID_PRODUCT_NAME                  = "clientProductName";
    private const String ID_BRAND_ROW                     = "clientBrandRow";
    private const String ID_BRAND                         = "clientBrand";
    private const String ID_MODEL_ROW                     = "clientModelRow";
    private const String ID_MODEL                         = "clientModel";
    private const String ID_VERSION                       = "clientVersion";
    private const String ID_PRODUCT_TYPE                  = "clientProductType";
    private const String ID_CLIENT_FEATURES               = "clientClientFeatures";
    private const String ID_OTHER_INFO_ROW                = "clientOtherInformationRow";
    private const String ID_OTHER_INFO                    = "clientOtherInformation";
    private const String ID_GENERAL_INFO                  = "generalInternationalSupportContactAddress";
    private const String ID_TEST_OPERATOR                 = "testOperatorName";
    private const String ID_EXECUTING_ORG_NAME            = "executingOrganizationName";
    private const String ID_EXECUTING_ORG_ADDRESS         = "executingOrganizationAddress";
    private const String ID_TECH_SUPPORT_WEBSITE          = "technicalSupportWebsiteURL";
    private const String ID_TECH_SUPPORT_EMAIL            = "technicalSupportEmail";
    private const String ID_TECH_SUPPORT_PHONE            = "technicalSupportPhone";
    private const String ID_INTERNATIONAL_SUPPORT_ADDRESS = "internationalSupportContactAddress";
    private const String ID_REGIONAL_SUPPORT_ADDRESS      = "regionalSupportContactAddress";

    #endregion


    #region Debuging

    private static void PrintUniqueIds(XElement doc) // TODO
    {
      var elements = XmlUtil.GetElementsWithTag(doc, "id");
      String[] ids = new String[elements.Count];

      int i = 0;
      elements.ToList().ForEach(item =>
      {
        ids[i] = item.Attribute("id").Value;
        ++i;
      });

      File.WriteAllLines(@"d:\ids.txt", ids); // TODO
    }

    #endregion
  }
}
