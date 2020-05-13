using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.CameraWebService.Common;
using System.Web.Services.Protocols;


namespace DUT.CameraWebService.Provisioning10
{
    public class ProvisioningServiceTest : Base.BaseServiceTest
    {
        #region Const

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int PanMove = 1;
        private const int TiltMove = 2;
        private const int ZoomMove = 3;
        private const int RollMove = 4;
        private const int FocusMove = 5;
        private const int Stop = 6;
        private const int GetUsage = 7;

        private const int MaxCommands = 8;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ProvisioningServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors


        protected override string ServiceName
        {
            get { return "ProvisioningService"; }
        }

        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void PanMoveTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("PanMove", PanMove, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void TiltMoveTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("TiltMove", TiltMove, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void ZoomMoveTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("ZoomMove", ZoomMove, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void RollMoveTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("RollMove", RollMove, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void FocusMoveTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("FocusMove", FocusMove, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void StopTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("Stop", Stop, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal Usage GetUsageTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Usage>("GetUsage", GetUsage, validationRequest, true, out stepType, out exc, out timeout);
        }
    }
}