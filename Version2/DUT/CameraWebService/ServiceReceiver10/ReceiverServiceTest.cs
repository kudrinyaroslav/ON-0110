using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Receiver10
{
    public class ReceiverServiceTest : Base.BaseServiceTest
    {

        protected override string ServiceName { get {return  "Receiver10";}}

        private const int MaxCommands = 10;
        private const int GetServiceCapabilities = 1;
        private const int GetReceiver = 2;
        private const int GetReceivers = 3;
        private const int GetReceiverState = 4;
        private const int CreateReceiver = 5;
        private const int DeleteReceiver = 6;
        private const int ConfigureReceiver = 7;
        private const int SetReceiverMode = 8;

        public ReceiverServiceTest(TestCommon testCommon)
            :base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out Timeout);
        }

        internal Receiver GetReceiverTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<Receiver>("GetReceiver", GetReceiver, validationRequest, out stepType, out ex, out Timeout);
        }

        internal Receiver[] GetReceiversTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<Receiver[]>("GetReceivers", GetReceivers, validationRequest, out stepType, out ex, out Timeout);
        }

        internal Receiver CreateReceiverTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<Receiver>("CreateReceiver", CreateReceiver, validationRequest, out stepType, out ex, out Timeout);
        }

        internal ReceiverStateInformation GetReceiverStateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<ReceiverStateInformation>("GetReceiverState", GetReceiverState, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DeleteReceiverTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteReceiver", DeleteReceiver,  validationRequest, out stepType,out ex, out Timeout);
        }

        internal void ConfigureReceiverTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("ConfigureReceiver", ConfigureReceiver, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void SetReceiverModeTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetReceiverMode", SetReceiverMode, validationRequest, out stepType, out ex, out Timeout);
        }
    }
}
