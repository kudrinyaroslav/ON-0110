using System.Xml;
using System.Web.Services;
using System.IO;
using DUT.PACS.Simulator.Configuration;
using System.Xml.Serialization;
using DUT.PACS.Simulator.ServiceAccessControl10;
using DUT.PACS.Simulator;
using DUT.PACS.Simulator.Common;
using System.Collections.Generic;



namespace DUT.PACS.Simulator.BackDoorServices
{
    /// <summary>
    /// Summary description for ConfigurationService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/simulator/configuration")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ConfigurationService : BaseDutService
    {
        /// <summary>
        /// Gets current configuration.
        /// </summary>
        /// <returns>Serialized configuration.</returns>
        [WebMethod]
        public XmlDocument GetCurrentConfiguration()
        {
            ConfStorage storage = null;
            if (Application["m_ConfStorage"] != null)
            {
                storage = (ConfStorage)Application["m_ConfStorage"];
            }
            else
            {
                storage = new ConfStorage();
                Application["m_ConfStorage"] = storage;
            }

            MemoryStream ms = new MemoryStream();
            SerializableConfiguration config = storage.GetSerializableConfiguration();
            System.Xml.Serialization.XmlSerializer ser = new XmlSerializer(typeof(SerializableConfiguration));
            ser.Serialize(ms, config);
            ms.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            return doc;
        }

        /// <summary>
        /// Reconfigures DUT.
        /// </summary>
        /// <param name="configuration"></param>
        [WebMethod]
        public void LoadConfiguration(XmlDocument configuration)
        {
            XmlReader reader = new XmlNodeReader(configuration.DocumentElement);
            System.Xml.Serialization.XmlSerializer ser = new XmlSerializer(typeof(SerializableConfiguration));
            SerializableConfiguration config = (SerializableConfiguration)ser.Deserialize(reader);

            

            ConfStorage storage = ConfStorage.Load(config);
            Application["m_ConfStorage"] = storage;
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("CredentialInfo")]
        public ServiceCredential10.CredentialInfo[] GetCredentialInfo()
        {
            ConfStorageLoad();
            List<ServiceCredential10.CredentialInfo> credentiaInfoList = new List<ServiceCredential10.CredentialInfo>();

            foreach (ServiceCredential10.Credential credential in ConfStorage.CredentialList.Values)
            {
                credentiaInfoList.Add(ServiceCredential10.CredentialService.ToCredentialInfo(credential));
            }
            return credentiaInfoList.ToArray();
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessProfileInfo")]
        public ServiceAccessRules10.AccessProfileInfo[] GetAccessProfileInfo()
        {
            ConfStorageLoad();
            List<ServiceAccessRules10.AccessProfileInfo> accessProfileInfoList = new List<ServiceAccessRules10.AccessProfileInfo>();

            foreach (ServiceAccessRules10.AccessProfile accessProfile in ConfStorage.AccessProfileList.Values)
            {
                accessProfileInfoList.Add(ServiceAccessRules10.AccessRulesService.ToAccessProfileInfo(accessProfile));
            }
            return accessProfileInfoList.ToArray();
        }

    }
}
