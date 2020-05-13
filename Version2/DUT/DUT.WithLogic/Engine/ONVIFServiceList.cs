using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Xml;
using DUT.WithLogic.Base;
using System.Web.Services;
using DUT.WithLogic.Services.DeviceManagement;
using DUT.WithLogic.Services.Media2;
using System.Xml.Serialization;

namespace DUT.WithLogic.Engine
{
    public class ONVIFServiceList
    {

        public ONVIFServiceList(ONVIFDeviceManagementCapabilities ONVIFDeviceManagementCapabilities, ONVIFMedia2Capabilities ONVIFMedia2Capabilities)
        {
            m_ONVIFDeviceManagementCapabilities = ONVIFDeviceManagementCapabilities;
            m_ONVIFMedia2Capabilities = ONVIFMedia2Capabilities;
        }

        #region Device Management

        ONVIFDeviceManagementCapabilities m_ONVIFDeviceManagementCapabilities;

        public ONVIFDeviceManagementCapabilities ONVIFDeviceManagementCapabilities
        {
            get { return m_ONVIFDeviceManagementCapabilities; }
            set { m_ONVIFDeviceManagementCapabilities = value; }
        }

        public string DeviceManagementUri
        {
            get
            {
                return FullUri(Base.AppPaths.PATH_DEVICEMANAGEMENT); ;
            }
        }

        #endregion //Device Management

        #region Media2

        ONVIFMedia2Capabilities m_ONVIFMedia2Capabilities;

        public ONVIFMedia2Capabilities ONVIFMedia2Capabilities
        {
            get { return m_ONVIFMedia2Capabilities; }
            set { m_ONVIFMedia2Capabilities = value; }
        }

        public string Media2Uri
        {
            get
            {
                return FullUri(Base.AppPaths.PATH_MEDAI2);
            }
        }

        #endregion //Media2

        public Proxy.Service[] ServicesWithoutCapabilities
        {
            get
            {
                Proxy.Service service;
                Proxy.Service[] res = new Proxy.Service[2];

                //Device Management
                service = new Proxy.Service();
                service.Version = new Proxy.OnvifVersion();
                service.Version.Major = 1;
                service.Version.Minor = 0;
                service.Namespace = "http://www.onvif.org/ver10/device/wsdl";
                service.XAddr = DeviceManagementUri;

                res[0] = service;

                //Media 2
                service = new Proxy.Service();
                service.Version = new Proxy.OnvifVersion();
                service.Version.Major = 1;
                service.Version.Minor = 0;
                service.Namespace = "http://www.onvif.org/ver20/media/wsdl";
                service.XAddr = Media2Uri;

                res[1] = service;

                return res;
            }
        }

        public Proxy.Service[] ServicesWithCapabilities
        {
            get
            {
                Proxy.Service service;
                Proxy.Service[] res = new Proxy.Service[2];

                //Device Management
                service = new Proxy.Service();
                service.Version = new Proxy.OnvifVersion();
                service.Version.Major = 1;
                service.Version.Minor = 0;
                service.Namespace = "http://www.onvif.org/ver10/device/wsdl";
                service.XAddr = DeviceManagementUri;
                service.Capabilities = SerializeToXmlElement(ONVIFDeviceManagementCapabilities.Capabilities);

                res[0] = service;

                //Media2
                service = new Proxy.Service();
                service.Version = new Proxy.OnvifVersion();
                service.Version.Major = 1;
                service.Version.Minor = 0;
                service.Namespace = "http://www.onvif.org/ver20/media/wsdl";
                service.XAddr = Media2Uri;
                service.Capabilities = SerializeToXmlElement(ONVIFMedia2Capabilities.Capabilities);

                res[1] = service;

                return res;
            }
        }



        public static XmlElement SerializeToXmlElement(object o)
        {
            XmlDocument doc = new XmlDocument();

            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                new XmlSerializer(o.GetType()).Serialize(writer, o);
            }

            XmlElement res = doc.CreateElement("Capabilities", "http://www.onvif.org/ver10/device/wsdl");
            foreach (XmlNode child in doc.DocumentElement.ChildNodes)
            {
                res.AppendChild(child.CloneNode(true));
            }

            return res;
        }

        public static string FullUri(string urlPart)
        {
            return String.Format("http://{0}/{1}", HttpContext.Current.Request.Url.Authority, urlPart);
        }
    }
}