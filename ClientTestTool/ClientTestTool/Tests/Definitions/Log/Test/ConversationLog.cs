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
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Log.Steps;

namespace ClientTestTool.Tests.Definitions.Log.Test
{
  /// <summary>
  /// Single test log.
  /// </summary>
  public sealed class ConversationLog : IXmlSerializable
  {
    public ConversationLog(BaseTest test, Conversation conversation)
    {
      Test         = test;
      Steps        = new List<StepResult>();
      TestName     = test.TestInfo.GetNameString();
      Conversation = conversation;
    }

    #region Properties

    /// <summary>
    /// Test
    /// </summary>
    public BaseTest Test
    {
      get;
      private set;
    }

    /// <summary>
    /// Name of the Test
    /// </summary>
    public String TestName
    {
      get;
      private set; 
    }

    /// <summary>
    /// Conversation Under Test
    /// </summary>
    public Conversation Conversation
    {
      get;
      private set;
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
        StepResult lastStep = Steps.LastOrDefault();
        if (null == lastStep || StepStatus.NotDetected == lastStep.Status)
          return TestStatus.NotDetected;

        if (null == Test)
          return TestStatus.Failed;

        return Steps.All(item => StepStatus.Passed == item.Status) ? TestStatus.Passed : TestStatus.Failed;
      }
    }

    #endregion

    public void Clear()
    {
      Steps.Clear();
      ErrorMessage = null;
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
      writer.WriteStartElement("ConversationLog");
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

          writer.WriteElementString("Name", step.Name);

          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    #endregion
  }
}
