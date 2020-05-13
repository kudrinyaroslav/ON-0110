///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Engine.Interfaces;

namespace ClientTestTool.Tests.Engine.Builder
{
  internal class TestInfoListBuilder : IBuilder<List<TestInfo>>
  {
    public List<TestInfo> Build()
    {
      var tests = new List<TestInfo>();

      Assembly currentAssembly = Assembly.GetExecutingAssembly();

      foreach (Type t in currentAssembly.GetTypes())
      {
        // Load test, if this is a test class
        object[] attrs = t.GetCustomAttributes(typeof(TestAttribute), true);
        if (attrs.Length > 0)
          tests.Add(LoadTest(t, tests));
      }

      return tests;
    }

    /// <summary>
    /// Loads test info from test class attribute
    /// </summary>
    /// <param name="t">Type of a Test class</param>
    /// <param name="tests">Array to keep values</param>
    private TestInfo LoadTest(Type t, ICollection<TestInfo> tests)
    {
      if (!t.GetInterfaces().Contains(typeof (ITest)))
        throw new ArgumentException();

      ConstructorInfo constructorInfo = t.GetConstructor(Type.EmptyTypes);

      if (constructorInfo == null)
        throw new ArgumentException();

      var testInstance = (BaseTest)constructorInfo.Invoke(new object[] { });

      return testInstance.TestInfo;
    }
  }
}
