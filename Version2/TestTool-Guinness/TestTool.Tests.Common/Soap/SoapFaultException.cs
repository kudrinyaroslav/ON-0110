﻿///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;

namespace TestTool.Tests.Common.Soap
{
    public class SoapFaultException : Exception
    {
        public SoapMessage<Fault> FaultMessage { get; protected set; }
        public Fault Fault { get; protected set; }

        public SoapFaultException(SoapMessage<Fault> message)
        {
            Fault = message.Object;
            FaultMessage = message;
        }
        public SoapFaultException(Fault fault)
        {
            Fault = fault;
            FaultMessage = null;
        }
    }
}