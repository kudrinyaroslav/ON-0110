///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global.SingletonInitializer.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.TestCaseSet.Base;

namespace ClientTestTool.Data.Global
{
  /// <summary>
  /// Contains information about each Test Case class
  /// </summary>
  [InitOnLoad]
  public class TestCaseSet : BaseTestCaseSet
  {
    #region Singleton

    private static TestCaseSet mInstance;

    public static TestCaseSet Instance
    {
      get
      {
        return mInstance ?? (mInstance = new TestCaseSet());
      }
    }

    static TestCaseSet()
    {}

    #endregion

    private TestCaseSet() : base()
    {
    }

    public void ClearTestResults()
    {
      mTests.Value.ForEach(item => item.Test.ClearTestResults());
    }

    #region Properties

    public List<TestInfo> Tests
    {
      get
      {
        return mTests.Value.ToList();
      }
    }

    public bool IsTestingDone
    {
      get
      {
        var profiles = UnitSet.GetSupportedProfiles();
        var tests = profiles.Any()
          ? new HashSet<TestInfo>(profiles.SelectMany(item => item.GetMandatoryTests())).ToList()
          : Tests;

        return tests.All(item => ((BaseTest) item.Test).IsCompleted);
      }
    }

    #endregion

    //TODO IReporter and global Reporter
    public static void SaveTestsResult(String filename)
    {
      var testLogs = Instance.Tests.SelectMany(item => ((BaseTest)item.Test).Log.ConversationLogs).ToList();
      using (var writer = XmlWriter.Create(filename))
      {
        writer.WriteStartDocument();

        writer.WriteStartElement("TestList");
        writer.WriteAttributeString("Count", testLogs.Count.ToString(CultureInfo.InvariantCulture));

        testLogs.ToList().ForEach(item => item.WriteXml(writer));

        writer.WriteEndElement();

        writer.WriteEndDocument();
      }
    }
  }
}
