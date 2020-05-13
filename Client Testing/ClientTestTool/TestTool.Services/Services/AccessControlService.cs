using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Onvif;

namespace TestTool.Services.Services
{
    [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    public class AccessControlService : BasePacsService, Onvif.PACSPort
    {
        public override string GetServiceName()
        {
            return "Access Control";
        }

        public override string GetLocalAddress()
        {
            return Definitions.LocalAddress.ACCESSCONTROL;
        }

        protected override Type GetContractType()
        {
            return typeof(Onvif.PACSPort);
        }


        #region PACSPort Members

        public void DoNothing()
        {
            
        }

        public AreaInfo GetAreaInfo(string Token)
        {
            BeginMethod("GetAreaInfo");
            EndMethod();
            return GetInfo(Token, I => I.token, S => S.PacsConfiguration.AreaInfoList);
        }

        public GetAreaInfoListResponse GetAreaInfoList(GetAreaInfoListRequest request)
        {
            BeginMethod("GetAreaInfoList");
            AreaInfo[] list = GetList<AreaInfo>(request.Offset.GetValueOrDefault(), request.Offset.HasValue,
                                                request.Limit.GetValueOrDefault(), request.Limit.HasValue, A => A.token,
                                                C => C.PacsConfiguration.AreaInfoList);
            EndMethod();
            return new GetAreaInfoListResponse(list);
        }

        public GetAreaInfoListByTokenListResponse GetAreaInfoListByTokenList(GetAreaInfoListByTokenListRequest request)
        {
            BeginMethod("GetAreaInfoListByTokenList");

            AreaInfo[] list = GetListByTokenList<AreaInfo>(request.TokenList, A => A.token,
                                                C => C.PacsConfiguration.AreaInfoList);
            EndMethod();
            return new GetAreaInfoListByTokenListResponse(list);
        }

        public AccessPointInfo GetAccessPointInfo(string Token)
        {
            BeginMethod("GetAccessPointInfo");

            AccessPointInfo info = GetInfo(Token, I => I.token, S => S.PacsConfiguration.AccessPointInfoList);
            
            EndMethod();

            return info;
        }

        public GetAccessPointInfoListResponse GetAccessPointInfoList(GetAccessPointInfoListRequest request)
        {
            BeginMethod("GetAccessPointInfoList");

            AccessPointInfo[] list = GetList<AccessPointInfo>(request.Offset.GetValueOrDefault(),
                                                              request.Offset.HasValue,
                                                              request.Limit.GetValueOrDefault(), request.Limit.HasValue,
                                                              A => A.token,
                                                              C => C.PacsConfiguration.AccessPointInfoList);
            EndMethod();

            return new GetAccessPointInfoListResponse(list);
        }

        public TestTool.Onvif.GetAccessPointInfoListByTokenListResponse GetAccessPointInfoListByTokenList(TestTool.Onvif.GetAccessPointInfoListByTokenListRequest request)
        {
            BeginMethod("GetAccessPointInfoListByTokenList");

            AccessPointInfo[] list = GetListByTokenList<AccessPointInfo>(request.TokenList, A => A.token,
                                                C => C.PacsConfiguration.AccessPointInfoList);

            EndMethod();

            return new GetAccessPointInfoListByTokenListResponse(list);
        }

        public void EnableAccessPoint(string Token)
        {
            BeginMethod("EnableAccessPoint");

            AccessPointInfo info = GetInfo(Token, I => I.token, S => S.PacsConfiguration.AccessPointInfoList);
            if (info.Capabilities.DisableAccessPoint)
            {
                info.Enabled = true;
            }
            else
            {
                Transport.CommonUtils.ReturnFault("Receiver", "ActionNotSupported");
            }
            EndMethod();

        }

        public void DisableAccessPoint(string Token)
        {
            BeginMethod("DisableAccessPoint");

            AccessPointInfo info = GetInfo(Token, I => I.token, S => S.PacsConfiguration.AccessPointInfoList);
            if (info.Capabilities.DisableAccessPoint)
            {
                info.Enabled = false;
            }
            else
            {
                Transport.CommonUtils.ReturnFault("Receiver", "ActionNotSupported");
            }
            EndMethod();
        }

        public TestTool.Onvif.AccessControlServiceCapabilities GetServiceCapabilities()
        {
            BeginMethod("GetServiceCapabilities");
            AccessControlServiceCapabilities capabilities =
                SimulatorConfiguration.ServicesConfiguration.AccessControlCapabilities; 

            EndMethod();
            return capabilities;
        }

        #endregion
    }
}
