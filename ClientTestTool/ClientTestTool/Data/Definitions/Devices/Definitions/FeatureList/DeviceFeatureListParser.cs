///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Definitions.Devices.Definitions.FeatureList
{
  public class DeviceFeatureListParser
  {
    public DeviceFeatureListParser(Device device)
    {
      if (null == device)
        throw new ArgumentNullException();

      if (!File.Exists(device.FeatureList))
        throw new ArgumentException(String.Format("{0} does not exists", device.FeatureList));

      mDevice = device;
    }

    public String GetTestProductName()
    {
      try
      {
        XElement doc = LoadFeatureList();

        var testInfo = doc.Descendants(TAG_TEST_INFORMATION).SingleOrDefault();

        if (null == testInfo)
          return mDevice.Name;

        var productNameElement = testInfo.Descendants("ProductName").Single();
        return productNameElement.Value;
      }
      catch (XmlException e)
      {
        Logger.LogException("", e);
        throw;
      }
    }

    public DeviceInformation GetDeviceInformation()
    {
      try
      {
        XElement doc = LoadFeatureList();

        var deviceInfo = doc.Descendants(TAG_DEVICE_INFO).SingleOrDefault();

        if (null == deviceInfo)
          return null;

        return DeviceInformation.Create(ParseDeviceInfo(deviceInfo));
      }
      catch (XmlException e)
      {
        Logger.LogException("Error while obtaining device information", e);
        return null;
      }
    }

    public List<Profile> GetSupportedProfiles()
    {
      var result = new List<Profile>();

      try
      {
        XElement doc = LoadFeatureList();

        var supportedProfiles = doc.Descendants(TAG_SUPPORTED_PROFILE).FirstOrDefault();

        if (null != supportedProfiles)
        {
          var profiles = supportedProfiles.Descendants(TAG_PROFILE).ToList();

          profiles.ForEach(element => result.Add(ProfileFromString(element.Value.Trim())));
        }

      }
      finally
      {
        if (!result.Any())
          result.Add(Profile.S);
      }

      return result;
    }

    public List<Feature> GetSupportedFeatures()
    {
      var translatedFeatures = GetFeatures().SelectMany(FeatureMapper.GetTranslatedFeatures).ToList();
      translatedFeatures.RemoveAll(feature => Feature.Unknown == feature);

      return translatedFeatures;
    }

    #region Helpers

    private IEnumerable<String> GetFeatures()
    {
      var result = new List<String>();

      try
      {
        XElement doc = LoadFeatureList();

        var features = doc.Descendants(TAG_FEATURES).SingleOrDefault();

        if (null != features)
          result.AddRange(features.Elements().Select(element => element.Value));
      }
      catch (XmlException e)
      {
        Logger.LogException("Error while obtaining features", e);
      }

      return result;
    }

    private Dictionary<String, String> ParseDeviceInfo(XElement deviceInfo)
    {
      var values = new Dictionary<String, String>();

      String[] tags =
      {
        DeviceInformation.TAG_MODEL,
        DeviceInformation.TAG_MANUFACTURER, 
        DeviceInformation.TAG_FIRMWARE,
        DeviceInformation.TAG_SERIAL_NUMBER
      };

      foreach (String tag in tags)
      {
        var element = deviceInfo.Element(tag);

        if (null != element)
          values.Add(tag, element.Value);
      }

      return values;
    }

    private Profile ProfileFromString(String s)
    {
      switch (s.ToUpper())
      {
        case "S":
          return Profile.S;

        case "G":
          return Profile.G;

        case "C":
          return Profile.C;

        default:
          throw new FormatException(); // TODO
      }
    }

    private XElement LoadFeatureList()
    {
      return XElement.Load(mDevice.FeatureList);
    }

    private readonly Device mDevice;

    private const String TAG_SUPPORTED_PROFILE = "SupportedProfile";
    private const String TAG_DEVICE_INFO       = "DeviceInformation";
    private const String TAG_TEST_INFORMATION  = "TestInformation";
    private const String TAG_FEATURES          = "Features";
    private const String TAG_PROFILE           = "Profile";

    #endregion
  }
}
