using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DUT.PACS.Simulator.ServiceUser03
{
    /// <summary>
    /// Summary description for UserService1
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/v3/User/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "UserBinding", Namespace = "http://www.onvif.org/v3/User/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserService : UserServiceBinding
    {

        #region Members

        /// <summary>
        /// Storage
        /// </summary>
        ConfStorage m_ConfStorage = null;

        #endregion //Members

        /***************************************************************************************/

        #region General

        /// <summary>
        /// Storage load if this is not done yet
        /// </summary>
        public void ConfStorageLoad()
        {
            if (Application["m_ConfStorage"] != null)
            {
                m_ConfStorage = (ConfStorage)Application["m_ConfStorage"];
            }
            else
            {
                m_ConfStorage = new ConfStorage();
                Application["m_ConfStorage"] = m_ConfStorage;
            }
        }

        /// <summary>
        /// Storage save
        /// </summary>
        public void ConfStorageSave()
        {
            Application["m_ConfStorage"] = m_ConfStorage;
        }

        #endregion //General

        /***************************************************************************************/

        #region WebMethods

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/User/wsdl/GetUserInfoList", RequestNamespace = "http://www.onvif.org/v3/User/wsdl", ResponseNamespace = "http://www.onvif.org/v3/User/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("UserInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override UserInfo[] GetUserInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            UserInfo[] res;
            List<UserInfo> tempRes = new List<UserInfo>();
            bool tokenFaund = false;

            ConfStorageLoad();

            res = m_ConfStorage.UserInfoList;

            if ((TokenList == null) || (TokenList.Count() == 0))
            {
                res = m_ConfStorage.UserInfoList;
            }
            else
            {
                foreach (string token in TokenList)
                {
                    tokenFaund = false;
                    foreach (UserInfo userInfo in m_ConfStorage.UserInfoList)
                    {
                        if (userInfo.token == token)
                        {
                            if (!tempRes.Contains(userInfo))
                            {
                                tempRes.Add(userInfo);
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/User/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/User/wsdl", ResponseNamespace = "http://www.onvif.org/v3/User/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override UserServiceCapabilities GetServiceCapabilities()
        {
            UserServiceCapabilities res = new UserServiceCapabilities();

            return res;
        }

        public override UserInfo[] GetUserInfoByCredentialToken(string CredentialToken)
        {
            throw new NotImplementedException();
        }

        #endregion //WebMethods

        /***************************************************************************************/

        #region Depricated
        //[WebMethod]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/User/wsdl/GetUserInfoByCredentialToken", RequestNamespace = "http://www.onvif.org/v3/User/wsdl", ResponseNamespace = "http://www.onvif.org/v3/User/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[return: System.Xml.Serialization.XmlArrayAttribute("UserInfoList")]
        //[return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        //public override UserInfo[] GetUserInfoByCredentialToken(string CredentialToken)
        //{
        //    UserInfo[] res;
        //    ServiceAccessControl03.CredentialInfo credentialInfo;
        //    List<UserInfo> tempRes = new List<UserInfo>();

        //    ConfStorageLoad();

        //    if (m_ConfStorage.CredentialInfoList.Count(credential => credential.token == CredentialToken) != 0)
        //    {

        //    credentialInfo = m_ConfStorage.CredentialInfoList.Single(credential => credential.token == CredentialToken);


        //        if (credentialInfo.UserToken == null)
        //        {
        //            res = null;
        //        }
        //        else
        //        {
        //            tempRes.Add(m_ConfStorage.UserInfoList.Single(user => user.token == credentialInfo.UserToken));
        //            res = tempRes.ToArray();
        //        }
        //    }
        //    else
        //    {
        //        throw FaultLib.GetSoapException(FaultType.General, "Token " + CredentialToken + " does not exist.");
        //    }

        //    ConfStorageSave();

        //    return res;
        //}
        #endregion Depricated
    }
}
