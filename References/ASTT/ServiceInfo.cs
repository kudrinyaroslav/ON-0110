using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ONVIFTestTool
{
    /// <summary>
    /// Simple class for service info
    /// </summary>
    class ServiceInfo
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">Service IP or DNS address</param>
        /// <param name="port">Service port</param>
        /// <param name="path">Exact path to service</param>
        public ServiceInfo(string address, int port, string path)
        {
            m_adress = address;
            m_port = port;
            m_path = path;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">Service IP or DNS address</param>
        /// <param name="port">Service port</param>
        /// <param name="user">User name</param>
        /// <param name="password">User password</param>
        /// <param name="path">Exact path to service</param>
        public ServiceInfo(string address, int port, string user, string password, string path) :
            this(address, port, path)
        {
            m_user = user;
            m_password = password;
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// Service IP or DNS address
        /// </summary>
        public string Adress { get { return m_adress; } }

        /// <summary>
        /// Service port
        /// </summary>
        public int Port { get { return m_port; } }

        /// <summary>
        /// User name
        /// </summary>
        public string User { get { return m_user; } }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get { return m_password; } }

        /// <summary>
        /// Exact path to service
        /// </summary>
        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        /// <summary>
        /// Service XMLNode
        /// </summary>
        public static XmlNode Service
        {
            get { return ServiceInfo.m_service; }
            set { ServiceInfo.m_service = value; }
        }

        #endregion // Properties

        #region Fields

        private string m_adress;
        private int m_port;
        private string m_user;
        private string m_password;
        private string m_path;
        static private XmlNode m_service;

        #endregion // Fields
    }
}
