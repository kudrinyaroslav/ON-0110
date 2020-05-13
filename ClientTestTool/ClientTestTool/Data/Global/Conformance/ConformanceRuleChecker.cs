///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Definitions.Worker;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global.SingletonInitializer.Attributes;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Global.Conformance
{
  [InitOnLoad]
  public class ConformanceRuleChecker : Worker
  {
    #region Singleton

    private static ConformanceRuleChecker mInstance;

    public static ConformanceRuleChecker Instance
    {
      get
      {
        return mInstance ?? (mInstance = new ConformanceRuleChecker());
      }
    }

    static ConformanceRuleChecker()
    {}

    #endregion

    private ConformanceRuleChecker() : base(ApplicationState.ConformanceRunning)
    {
    }

    public override void Run()
    {
      Run(Check);
    }

    private void Check()
    {
      ConformanceLog.Instance.Clear();

      foreach (Profile profile in Enum.GetValues(typeof(Profile)))
      {
        var devices        = UnitSet.GetDevices(profile);

        bool isProfileSupported = profile.IsSupported();
        if (!isProfileSupported && !devices.Any())
        {
          ConformanceLog.Instance.AddWarning(profile, "0 devices found");
          continue;
        }

        Check3Devices(profile, devices);

        if (isProfileSupported && !devices.Any())
          ConformanceLog.Instance.AddError(profile, "At least one Device Feature List is missing");

        if (Profile.C == profile)
          if (FeatureStatus.Undefined == Feature.EventHandling.GetInfo().Status)
            ConformanceLog.Instance.AddError(profile, "At least one Event Handling Feature Required");

        CheckMandatoryTests(profile);
        CheckConditionalTests(profile);

        foreach (var device in devices)
          CheckDeviceSpecificTests(profile, device);
      }
    }

    private void Check3Devices(Profile profile, List<Device> devices)
    {
      if (devices.Count < 3)
        ConformanceLog.Instance.AddError(profile, "Min 3 conformant Devices not found");
      else
      {
        var manufacturersSet = new HashSet<String>(devices.Select(item => item.Info.Manufacturer.ToUpper()));

        if (manufacturersSet.Count < 3)
          ConformanceLog.Instance.AddError(profile, "Min 3 conformant Devices from 3 different manufacturers not found");
      }
    }

    private void CheckMandatoryTests(Profile profile)
    {
      var mandatoryTests = profile.GetMandatoryTests();
      var notPassedTests = mandatoryTests.Where(item => TestStatus.Passed != item.Status).ToList();

      foreach (var test in notPassedTests)
        if (FeatureStatus.Supported != test.FeatureUnderTest.GetParentFeature().GetInfo().Status)
          ConformanceLog.Instance.AddError(profile, String.Format("Mandatory test {0} has not passed", test.GetNameString()));
    }

    private void CheckConditionalTests(Profile profile)
    {
      var conditionalTests = profile.GetConditionalTests();
      var notPassedTests = conditionalTests.Where(item => TestStatus.Passed != item.Status).ToList();

      foreach (var test in notPassedTests)
        ConformanceLog.Instance.AddWarning(profile, String.Format("Conditional test {0} has not passed", test.GetNameString()));
    }

    private void CheckDeviceSpecificTests(Profile profile, Device device)
    {
      if (!device.IsFeatureListAttached)
        return;

      var mandatoryTests = profile.GetMandatoryTests();
      var optionalTests  = profile.GetOptionalTests();

      var deviceSpecificTests = device.GetSupportedFeatures()
          .Select(feature => feature.GetDependingTest())
          .Except(mandatoryTests)
          .Except(optionalTests)
          .ToList();

      foreach (var test in deviceSpecificTests)
      {
        var featureInfo = test.FeatureUnderTest.GetInfo();

        if (TestStatus.Failed == test.Status)
          ConformanceLog.Instance.AddError(profile, String.Format("Conditional feature {0} which supported by Device {1} has failed",
              featureInfo.Name,
              device.Name));

        if (TestStatus.NotDetected == test.Status)
          ConformanceLog.Instance.AddWarning(profile, String.Format("Conditional feature {0} which supported by Device {1} was not detected",
              featureInfo.Name,
              device.Name));
      }
    }
  }
}
