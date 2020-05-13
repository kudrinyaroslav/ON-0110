///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Xml.Serialization;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.Tests.Definitions.Log
{
  /// <summary>
  /// Step results.
  /// </summary>
  [Serializable]
  [XmlType("Step")]
  public class StepResult
  {
    /// <summary>
    /// Step number
    /// </summary>
    public int Number
    {
      get;
      set;
    }

    /// <summary>
    /// Step name
    /// </summary>
    [XmlAttribute("Name")]
    public string StepName
    {
      get;
      set;
    }

    /// <summary>
    /// Message
    /// </summary>
    public string Message
    {
      get;
      set;
    }

    [XmlIgnore]
    public RequestResponsePair IssuedPair
    {
      get;
      set;
    }


    public StepStatus Status
    {
      get;
      set;
    }

    [XmlIgnore]
    public Exception Exception
    {
      get;
      set;
    }
  }
}
