using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Door12
{
    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class DoorServiceTest : Base.BaseServiceTest
    {
        protected override string ServiceName { get { return "Door12"; } }

        #region Const

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetDoorState = 1;
        private const int GetDoorInfoList = 2;
        private const int AccessDoor = 3;
        private const int LockDoor = 4;
        private const int UnlockDoor = 5;
        private const int BlockDoor = 6;
        private const int LockDownDoor = 7;
        private const int LockDownReleaseDoor = 8;
        private const int LockOpenDoor = 9;
        private const int LockOpenReleaseDoor = 10;
        private const int DoubleLockDoor = 11;
        private const int GetDoorInfo = 12;
        private const int GetDoorList = 13;
        private const int GetDoors = 14;
        private const int CreateDoor = 15;
        private const int SetDoor = 16;
        private const int ModifyDoor = 17;
        private const int DeleteDoor = 18;
        private const int MaxCommands = 19;
        

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DoorServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

        

        #region General

        internal ServiceCapabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<ServiceCapabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //General

        //***************************************************************************************

        #region Door

        internal DoorState GetDoorStateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<DoorState>("GetDoorState", GetDoorState, validationRequest, out stepType, out ex, out Timeout);
        }
        
        internal DoorInfo[] GetDoorInfoTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<DoorInfo[]>("GetDoorInfo", GetDoorInfo, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string GetDoorInfoListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetDoorInfoList", GetDoorInfoList, validationRequest, out stepType, out ex, out Timeout);
        }

        internal DoorInfo[] TakeDoorInfoList()
        {
            return TakeSpecialParameter<DoorInfo[]>("GetDoorInfoList", GetDoorInfoList, "ArrayOfDoorInfo");
        }

        internal Door[] GetDoorsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<Door[]>("GetDoors", GetDoors, validationRequest, out stepType, out ex, out timeout);
        }


        internal Door[] TakeDoorList()
        {
            return TakeSpecialParameter<Door[]>("GetDoorList", GetDoorList, "ArrayOfDoor");
        }
        internal string GetDoorListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetDoorList", GetDoorList, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string CreateDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("CreateDoor", CreateDoor, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void SetDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetDoor", SetDoor, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void ModifyDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("ModifyDoor", ModifyDoor, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DeleteDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteDoor", DeleteDoor, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //Door

        //***************************************************************************************

        #region DoorControl

        internal void AccessDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("AccessDoor", AccessDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void LockDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("LockDoor", LockDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void UnlockDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("UnlockDoor", UnlockDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void BlockDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("BlockDoor", BlockDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void LockDownDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("LockDownDoor", LockDownDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void LockDownReleaseDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("LockDownReleaseDoor", LockDownReleaseDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void LockOpenDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("LockOpenDoor", LockOpenDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void LockOpenReleaseDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("LockOpenReleaseDoor", LockOpenReleaseDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        internal void DoubleLockDoorTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DoubleLockDoor", DoubleLockDoor, validationRequest, true, out stepType, out ex, out Timeout);
        }

        #endregion //DoorControl

    }
}
