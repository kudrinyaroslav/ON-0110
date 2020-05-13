///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Interfaces;

namespace ClientTestTool.Tests.Definitions.Data
{
  /// <summary>
  /// Test information
  /// </summary>
  public class TestInfo
  {

    #region Builder

    internal static class Builder
    {
      public static TestInfo Build(ITest test)
      {
        var testAttribute = GetAttribute(test.GetType());

        if (null == testAttribute)
          throw new NullReferenceException();

        return new TestInfo(test, testAttribute);
      }

      private static TestAttribute GetAttribute(Type t)
      {
        return Attribute.GetCustomAttribute(t, typeof(TestAttribute)) as TestAttribute;
      }
    }

    #endregion

    #region TestInfo

    private TestInfo(ITest test, TestAttribute infoAttribute)
    {
      Test = test;
      Name             = infoAttribute.Name;
      Category         = infoAttribute.Category;
      Id               = infoAttribute.Id;
      Path             = infoAttribute.Path;
      FeatureUnderTest = infoAttribute.FeatureUnderTest;
    }

    public String GetNameString()
    {
      return String.Format("{0}-{1} {2}", FeatureUnderTest.GetParentFeature(), Id, Name).ToUpper();
    }

    #region Properties

    public String Name
    {
      get;
      private set;
    }

    public String Id
    {
      get;
      private set;
    }

    public ITest Test
    {
      get;
      private set;
    }

    public TestStatus Status
    {
      get
      {
        return ((BaseTest)Test).Log.ConversationLogs.GetAverageTestStatus();
      }
    }

    public String Path
    {
      get;
      private set;
    }

    public Category Category
    {
      get;
      private set;
    }

    public Feature FeatureUnderTest
    {
      get;
      private set;
    }

    #endregion

    #endregion
  }
}