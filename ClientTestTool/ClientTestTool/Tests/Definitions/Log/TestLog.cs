///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.Tests.Definitions.Log
{
  /// <summary>
  /// Single test log.
  /// </summary>
  public sealed class TestLog : IXmlSerializable
  {
    public TestLog(BaseTest test)
    {
      Test     = test;
      Steps    = new List<StepResult>();
      TestName = test.TestInfo.GetNameString();
    }

    public BaseTest Test
    {
      get;
      private set;
    }

    public String TestName
    {
      get;
      private set; 
    }

    public void Clear()
    {
      Steps.Clear();
      ErrorMessage = null;
    }

    /// <summary>
    /// Test steps
    /// </summary>
    public List<StepResult> Steps
    {
      get;
      private set;
    }

    /// <summary>
    /// Test error message (if an error occurred outside a step)
    /// </summary>
    public String ErrorMessage
    {
      get;
      set;
    }

    /// <summary>
    /// Test status
    /// </summary>
    public TestStatus TestStatus
    {
      get
      {
        if (0 == Steps.Count)
          return TestStatus.NotDetected;

        if (null == Test)
          return TestStatus.Failed;

        return Test.IsCompleted && Steps.All(item => StepStatus.Passed == item.Status) ? TestStatus.Passed : TestStatus.Failed;
      }
    }

    #region IXmlSerializable implementation

    public XmlSchema GetSchema()
    {
      return null;
    }

    public void ReadXml(XmlReader reader)
    {
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("TestLog");
      writer.WriteAttributeString("Name", TestName);

      if (0 == Steps.Count && null != ErrorMessage)
      {
        writer.WriteElementString("NotSupported", ErrorMessage);
      }
      else
      {
        writer.WriteStartElement("Steps");

        foreach (var step in Steps)
        {
          writer.WriteStartElement("Step");
          writer.WriteAttributeString("Number", step.Number.ToString(CultureInfo.InvariantCulture));
          writer.WriteAttributeString("Status", step.Status.ToString());

          writer.WriteElementString("Name", step.StepName);

          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    #endregion
  }
}
