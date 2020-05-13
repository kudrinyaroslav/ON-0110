///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Proxies;
using ClientTestTool.Tests.SoapValidation;

namespace ClientTestTool.Data.Definitions.Devices.Definitions
{
  /// <summary>
  /// Device Info container
  /// </summary>
  public sealed class DeviceInformation
  {
    public const String TAG_MODEL         = "Model";
    public const String TAG_FIRMWARE      = "FirmwareVersion";
    public const String TAG_MANUFACTURER  = "Manufacturer";
    public const String TAG_SERIAL_NUMBER = "SerialNumber";

    #region Building

    public static DeviceInformation Create()
    {
      return new DeviceInformation();  
    }

    public static DeviceInformation Create(String manufacturer, String model, String firmware, String serial)
    {
      return new DeviceInformation(manufacturer, model, firmware, serial);
    }

    public static DeviceInformation Create(SoapMessage<GetDeviceInformationResponse> message)
    {
      if (null == message)
        throw new ArgumentNullException("message");

      var info = message.Object;
      TrimDeviceInformation(info);

      return Create(info.Manufacturer, info.Model, info.FirmwareVersion, info.SerialNumber);
    }

    public static DeviceInformation Create(Dictionary<String, String> values)
    {
      return new DeviceInformation()
      {
        Manufacturer    = values.ContainsKey(TAG_MANUFACTURER)  ? values[TAG_MANUFACTURER]  : String.Empty,
        Model           = values.ContainsKey(TAG_MODEL)         ? values[TAG_MODEL]         : String.Empty,
        FirmwareVersion = values.ContainsKey(TAG_FIRMWARE)      ? values[TAG_FIRMWARE]      : String.Empty,
        SerialNumber    = values.ContainsKey(TAG_SERIAL_NUMBER) ? values[TAG_SERIAL_NUMBER] : String.Empty,
      };
    }

    #endregion

    #region Properties

    public String Manufacturer
    {
      get;
      set;
    }

    public String Model
    {
      get;
      set;
    }

    public String FirmwareVersion
    {
      get;
      set;
    }

    public String SerialNumber
    {
      get;
      set;
    }

    #endregion

    #region Ctors

    private DeviceInformation()
    {
      Manufacturer = String.Empty;
      Model = String.Empty;
      FirmwareVersion = String.Empty;
      SerialNumber = String.Empty;
    }

    private DeviceInformation(String manufacturer, String model, String firmware, String serial)
    {
      Manufacturer = manufacturer;
      Model = model;
      FirmwareVersion = firmware;
      SerialNumber = serial;
    }

    #endregion

    #region Helpers

    private static void TrimDeviceInformation(GetDeviceInformationResponse deviceInfo)
    {
      foreach (var property in deviceInfo.GetType().GetProperties())
      {
        String processedValue = property.GetValue(deviceInfo).ToString().Trim();

        property.SetValue(deviceInfo, processedValue);
      }
    }

    #endregion
  }
}
