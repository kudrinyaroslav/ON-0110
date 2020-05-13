using System.Collections.Generic;
using System.Linq;

namespace DUT.PACS.Simulator.ServiceAccessControl03
{
    /// <summary>
    /// Summary description for PACSService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/v3/AccessControl/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PACSBinding", Namespace = "http://www.onvif.org/v3/AccessControl/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PACSService : PACSServiceBinding
    {

        public PACSService()
        {
            ConfStorageLoad();
            EventServerLoad();
           
            EventServer.PACSService = this;

            EventServerSave();
            ConfStorageSave();
        }

        /***************************************************************************************/

        #region WebMethods

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetAreaInfoList", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AreaInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AreaInfo[] GetAreaInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            AreaInfo[] res;
            List<AreaInfo> tempRes = new List<AreaInfo>();
            bool tokenFaund = false;

            ConfStorageLoad();

            if ((TokenList == null) || (TokenList.Count() == 0))
            {
                res = ConfStorage.AreaInfoList;
            }
            else
            {
                foreach (string token in TokenList)
                {
                    tokenFaund = false;
                    foreach (AreaInfo areaInfo in ConfStorage.AreaInfoList)
                    {
                        if (areaInfo.token == token)
                        {
                            if (!tempRes.Contains(areaInfo))
                            {
                                tempRes.Add(areaInfo);
                            }
                            tokenFaund = true;
                            break;
                        }
                    }

                    if (!tokenFaund)
                    {
                        throw FaultLib.GetSoapException(FaultType.General, "Token " + token + " does not exist.");
                    }
                }
                res = tempRes.ToArray();
            }

            ConfStorageSave();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetAccessPointInfoList", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AccessPointInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AccessPointInfo[] GetAccessPointInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            AccessPointInfo[] res;
            List<AccessPointInfo> tempRes = new List<AccessPointInfo>();
            bool tokenFaund = false;

            ConfStorageLoad();

            res = ConfStorage.AccessPointInfoList;

            if ((TokenList == null) || (TokenList.Count() == 0))
            {
                res = ConfStorage.AccessPointInfoList;
            }
            else
            {
                foreach (string token in TokenList)
                {
                    tokenFaund = false;
                    foreach (AccessPointInfo accessPointInfo in ConfStorage.AccessPointInfoList)
                    {
                        if (accessPointInfo.token == token)
                        {
                            if (!tempRes.Contains(accessPointInfo))
                            {
                                tempRes.Add(accessPointInfo);
                            }
                            tokenFaund = true;
                            break;
                        }
                    }

                    if (!tokenFaund)
                    {
                        throw FaultLib.GetSoapException(FaultType.General, "Token " + token + " does not exist.");
                    }
                }
                res = tempRes.ToArray();
            }

            ConfStorageSave();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetAccessControllerInfoList", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AccessControllerInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AccessControllerInfo[] GetAccessControllerInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            AccessControllerInfo[] res;
            List<AccessControllerInfo> tempRes = new List<AccessControllerInfo>();
            bool tokenFaund = false;

            ConfStorageLoad();

            res = ConfStorage.AccessControllerInfoList;

            if ((TokenList == null) || (TokenList.Count() == 0))
            {
                res = ConfStorage.AccessControllerInfoList;
            }
            else
            {
                foreach (string token in TokenList)
                {
                    tokenFaund = false;
                    foreach (AccessControllerInfo accessControllerInfo in ConfStorage.AccessControllerInfoList)
                    {
                        if (accessControllerInfo.token == token)
                        {
                            if (!tempRes.Contains(accessControllerInfo))
                            {
                                tempRes.Add(accessControllerInfo);
                            }
                            tokenFaund = true;
                            break;
                        }
                    }

                    if (!tokenFaund)
                    {
                        throw FaultLib.GetSoapException(FaultType.General, "Token " + token + " does not exist.");
                    }
                }
                res = tempRes.ToArray();
            }

            ConfStorageSave();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override AccessControlServiceCapabilities GetServiceCapabilities()
        {
            AccessControlServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessControlCapabilities;
            return capabilities;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/EnableAccessPoint", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void EnableAccessPoint(string AccessPointToken)
        {
            ServiceAccessControl03.AccessPointInfo accessPointInfo;

            ConfStorageLoad();

            if (ConfStorage.AccessPointInfoList.Count(accessPoint => accessPoint.token == AccessPointToken) != 0)
            {
                accessPointInfo = ConfStorage.AccessPointInfoList.Single(accessPoint => accessPoint.token == AccessPointToken);
                accessPointInfo.Enabled = true;
            }
            else
            {
                throw FaultLib.GetSoapException(FaultType.General, "Token " + AccessPointToken + " does not exist.");
            }

            ConfStorageSave();

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/DisableAccessPoint", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DisableAccessPoint(string AccessPointToken)
        {
            ServiceAccessControl03.AccessPointInfo accessPointInfo;

            ConfStorageLoad();

            if (ConfStorage.AccessPointInfoList.Count(accessPoint => accessPoint.token == AccessPointToken) != 0)
            {
                accessPointInfo = ConfStorage.AccessPointInfoList.Single(accessPoint => accessPoint.token == AccessPointToken);
                accessPointInfo.Enabled = false;
            }
            else
            {
                throw FaultLib.GetSoapException(FaultType.General, "Token " + AccessPointToken + " does not exist.");
            }

            ConfStorageSave();
        }

        #endregion //WebMethods

        /***************************************************************************************/

        public void SynchronizationPoint()
        {
            ConfStorageLoad();
            EventServerLoad();
            
            foreach (AccessControllerInfo info in ConfStorage.AccessControllerInfoList)
            {
                //EventServer.TamperingEvent(this, "Initialized", info.token, info.);
            }

            EventServerSave();
            ConfStorageSave();
        }


    }
}
