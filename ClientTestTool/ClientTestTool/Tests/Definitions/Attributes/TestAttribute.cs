///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Attributes
  {
    /// <summary>
    /// Attribute to mark Test Case class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TestAttribute : Attribute
    {
      public TestAttribute()
      {
        FeatureUnderTest = Feature.Unknown;
      }

      /// <summary>
      /// Path to the test in the tree
      /// </summary>
      public String Path
      {
        get
        {
          return String.Format("{0}\\{1}", Category.GetDisplayName(), FeatureUnderTest.GetParentFeature().GetDisplayName());
        }
      }

      /// <summary>
      /// Test name
      /// </summary>
      public String Name
      {
        get;
        set;
      }

      public String Id
      {
        get;
        set;
      }

      public Category Category
      {
        get;
        set;
      }

      public Feature FeatureUnderTest
      {
        get;
        set;
      }
    }
}
