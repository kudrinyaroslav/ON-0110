///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;
using ClientTestTool.Parsers.HeaderParser;
using ClientTestTool.Tests.SoapValidation;
using ClientTestTool.Tests.SoapValidation.SchemaSets;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Http
{
  public sealed class HttpRequest : HttpMessage
  {
    public HttpRequest(Frame frame, Conversation conversation)
      : base(frame, conversation, MessageType.Request)
    {
    }

    #region Properties

    public String RequestMethod
    {
      get
      {
        return new HttpHeaderParser(new StringReader(GetHttpString())).RequestMethod;
      }
    }

    public String UserAgent
    {
      get
      {
        return new HttpHeaderParser(new StringReader(GetHttpString())).UserAgent;
      }
    }

    #endregion

    #region Validation

    /// <summary>
    /// SOAP validation 
    /// </summary>
    public override void Validate()
    {
      try
      {
        SoapValidator validator = new SoapValidator(OnvifSchemaSet.Instance);
        validator.Validate(new StringReader(GetXmlString())); // TODO Validation of String.Empty
        ValidationStatus = ValidationStatus.Passed;
      }
      catch (XmlSchemaValidationException e)
      {
        ValidationError += e.Message;
        ValidationStatus = ValidationStatus.Failed;
      }
      catch (XmlException e)
      {
        ValidationError += e.Message;
        ValidationStatus = ValidationStatus.Failed;
      }

      Validated = true;
    }

    #endregion
  }
}
