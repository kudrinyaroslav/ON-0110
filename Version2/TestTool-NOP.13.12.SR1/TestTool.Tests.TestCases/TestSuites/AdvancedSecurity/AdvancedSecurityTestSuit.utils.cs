///////////////////////////////////////////////////////////////////////////
//!  @author        Alexei Soloview
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils.Events;
using AlgorithmIdentifier = TestTool.Proxies.Onvif.AlgorithmIdentifier;
using DateTime = System.DateTime;

namespace TestTool.Tests.TestCases.TestSuites.AdvancedSecurity
{
    /// <summary>
    /// This file contains various utils used in implementation of testcase's bodies.
    /// </summary>
    partial class AdvancedSecurityTestSuit
    {
        /// <summary>
        /// Constants for KeyStatus in GetKeyStatus command
        /// </summary>
        private const string ksOK = "ok";
        private const string ksCORRUPT = "corrupt";

        private DistinguishedName DefaultSubject { get { return new DistinguishedName() { Country = new[] { "US" }, CommonName = new[] { _cameraIp.ToString() } }; } }
        private string DefaultSubjectString 
        { 
            get
            {
                if (null != DefaultSubject)
                {
                    var r = new StringBuilder();

                    if (null != DefaultSubject.CommonName)
                        r.Append(string.Format("CN={0},", DefaultSubject.CommonName));
                    if (null != DefaultSubject.Country)
                        r.Append(string.Format("C={0},", DefaultSubject.Country));
                    if (null != DefaultSubject.Locality)
                        r.Append(string.Format("L={0},", DefaultSubject.Locality));
                    if (null != DefaultSubject.Organization)
                        r.Append(string.Format("O={0},", DefaultSubject.Organization));
                    if (null != DefaultSubject.OrganizationalUnit)
                        r.Append(string.Format("OU={0},", DefaultSubject.OrganizationalUnit));
                    if (null != DefaultSubject.StateOrProvinceName)
                        r.Append(string.Format("ST={0},", DefaultSubject.StateOrProvinceName));

                    return r.ToString().TrimEnd(',');
                }

                return "";
            }
        }

        #region Fault Messages

        private const string fpMsgRSAKeyPair = "WARNING: Key storage pre-requisite was not fullfilled. The DUT shall have sufficient free storage for one more additional RSA key pair. Please see test description to find more information.";
        private const string fpMsgCertificate = "WARNING: Certificate storage pre-requisite was not fullfilled. The DUT shall have sufficient free storage for one more additional certificate. Please see test description to find more information.";
        private const string fpMsgCertificationPath = "WARNING: Certificate storage pre-requisite was not fullfilled. The DUT shall have sufficient free storage for one more additional certification path. Please see test description to find more information.";
        private const string fpMsgCertificateAssignment = "WARNING: TLS server pre-requisite was not fullfilled. The DUT shall have sufficient free storage for one more additional server certificate assignment. Please see test description to find more information.";

        private const string wMsgUnspecifiedSOAPFault = "WARNING: received SOAP fault message is not specified for this operation. Please see operation description to find more information.";
        private const string wMsgKeyLength = "WARNING: the specified key length is not supported by the DUT.";
        private const string wMsgKeyID = "WARNING: no key is stored under the requested KeyID.";
        private const string wMsgKeyDeletingFailed = "WARNING: deleting the key with the requested KeyID failed.";
        private const string wMsgReferenceExists = "WARNING: a reference exists for the specified key. ";

        #endregion

        #region Clients
        private DeviceClient m_DeviceClient;
        DeviceClient DeviceClient
        {
            get
            {
                if (null == m_DeviceClient)
                {
                    var binding = CreateBinding(true, new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    m_DeviceClient = new DeviceClient(binding, new EndpointAddress(_cameraAddress));
                    AttachSecurity(m_DeviceClient.Endpoint);
                    SetupChannel(m_DeviceClient.InnerChannel);
                }
                return m_DeviceClient;
            }

        }

        private ServiceHolder<AdvancedSecurityServiceClient, AdvancedSecurityService> m_Client;
        private AdvancedSecurityServiceClient Client
        {
            get
            {
                if (null == m_Client)
                {
                    m_Client = new ServiceHolder<AdvancedSecurityServiceClient, AdvancedSecurityService>(features => GetAdvancedSecurityServiceAddress(),
                                                                                                         (binding, address) => new AdvancedSecurityServiceClient(binding, address),
                                                                                                         "Advanced Security");

                    if (null == m_Client.Client)
                    {
                        InitServiceClient(m_Client, new IChannelController[] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) });
                    }
                }

                return m_Client.Client;
            }
        }

        private ServiceHolder<KeystoreClient, Keystore> m_KeystoreClient;
        private KeystoreClient KeystoreClient
        {
            get
            {
                if (null == m_KeystoreClient)
                {
                    m_KeystoreClient = new ServiceHolder<KeystoreClient, Keystore>(features => GetAdvancedSecurityServiceAddress(),
                                                                                   (binding, address) => new KeystoreClient(binding, address),
                                                                                   "Keystore Client");

                    if (null == m_KeystoreClient.Client)
                    {
                        InitServiceClient(m_KeystoreClient, new IChannelController[] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) });
                    }
                }

                return m_KeystoreClient.Client;
            }
        }

        private ServiceHolder<TLSServerClient, TLSServer> m_TLSServer;
        private TLSServerClient TLSServer
        {
            get
            {
                if (null == m_TLSServer)
                {
                    m_TLSServer = new ServiceHolder<TLSServerClient, TLSServer>(features => GetAdvancedSecurityServiceAddress(),
                                                                                (binding, address) => new TLSServerClient(binding, address),
                                                                                "TLSServer Client");

                    if (null == m_TLSServer.Client)
                    {
                        InitServiceClient(m_TLSServer, new IChannelController[] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) });
                    }
                }

                return m_TLSServer.Client;
            }
        }

        /// <summary>
        /// Initializes specified client: determines service's address, attaches security and controllers
        /// </summary>
        /// <param name="serviceHolder">Client to be initialized</param>
        /// <param name="controllers">Controllers that should be attached to client</param>
        void InitServiceClient(ServiceHolder serviceHolder, IEnumerable<IChannelController> controllers)
        {
            bool found = false;
            if (!serviceHolder.HasAddress)
            {
                RunStep(() =>
                        {
                            serviceHolder.Retrieve(Features);
                            if (!serviceHolder.HasAddress)
                            {
                                throw new AssertException(string.Format("{0} service not found", serviceHolder.ServiceName));
                            }
                            else
                            {
                                found = true;
                                LogStepEvent(serviceHolder.Address);
                            }
                        },
                        string.Format("Get {0} service address", serviceHolder.ServiceName));
                DoRequestDelay();
            }

            Assert(found,
                   string.Format("{0} service address not found", serviceHolder.ServiceName),
                   string.Format("Check that the DUT returned {0} service address", serviceHolder.ServiceName));

            if (found)
            {
                EndpointController controller = new EndpointController(new EndpointAddress(serviceHolder.Address));

                List<IChannelController> ctrls = new List<IChannelController>();
                ctrls.Add(controller);
                ctrls.AddRange(controllers);

                Binding binding = CreateBinding(false, ctrls);

                serviceHolder.CreateClient(binding, AttachSecurity, SetupChannel);
            }
        }

        private string m_AdvancedSecurityServiceAddress;

        /// <summary>
        /// Retrieves and stores address of Advanced Security Service
        /// </summary>
        /// <returns>Address of Advanced Security Service</returns>
        private string GetAdvancedSecurityServiceAddress()
        {
            return m_AdvancedSecurityServiceAddress ?? (m_AdvancedSecurityServiceAddress = DeviceClient.GetServiceAddress(OnvifService.ADVANCED_SECURITY));
        }

        #endregion

        #region Service commands

        private AdvancedSecurityCapabilities GetCapabilities()
        {
            AdvancedSecurityCapabilities r = null;

            RunStep(() =>  r = Client.GetServiceCapabilities(),
                    "Get Advanced Security Capabilities");

            return r;
        }

        private string CreateRSAKeyPair(string keyLength, string alias, out string estimatedCreationTime)
        {
            string keyID = null;
            string localEstimatedCreationTime = null;
            var aliasPart = (null == alias) ? "without alias" : string.Format("with alias = '{0}'", alias);

            try
            {
                RunStep(() => keyID = KeystoreClient.CreateRSAKeyPair(keyLength, alias, out localEstimatedCreationTime),
                        string.Format("Create RSA Key Pair of size '{0}' and {1}", keyLength, aliasPart));

                estimatedCreationTime = localEstimatedCreationTime;
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfKeysReached"))
                    LogStepEvent(fpMsgRSAKeyPair);
                else if (e.IsValidOnvifFault("Sender/InvalidArgVal/KeyLength"))
                    LogStepEvent(wMsgKeyLength);
                else
                    LogStepEvent(wMsgUnspecifiedSOAPFault);

                throw;
            }

            return keyID;
        }

        private void DeleteRSAKeyPair(string keyID)
        {
            try
            {
                RunStep(() => KeystoreClient.DeleteKey(keyID),
                        string.Format("Delete RSA Key Pair with ID = '{0}'", keyID));
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/KeyDeletionFailed"))
                    LogStepEvent(wMsgKeyDeletingFailed);
                else if (e.IsValidOnvifFault("Sender/InvalidArgVal/KeyID"))
                    LogStepEvent(wMsgKeyID);
                else if (e.IsValidOnvifFault("Sender/InvalidArgVal/ReferenceExists"))
                    LogStepEvent(wMsgReferenceExists);
                else
                    LogStepEvent(wMsgUnspecifiedSOAPFault);

                throw;
            }
        }

        private IEnumerable<KeyAttribute> GetAllKeys()
        {
            IEnumerable<KeyAttribute> r = null;

            RunStep(() => r = KeystoreClient.GetAllKeys(),
                    "Get All Keys");

            if (null != r)
                Assert(r.Distinct().Count() == r.Count(),
                       "There are duplicates in received list of key's IDs",
                       "Check that received key's IDs are unique");

            return r ?? new List<KeyAttribute>();
        }

        private string CreateSelfSignedCertificate(string X509Version, DistinguishedName Subject, string KeyID, string Alias, System.DateTime notValidBefore, System.DateTime notValidAfter, AlgorithmIdentifier SignatureAlgorithm, X509v3Extension[] Extension)
        {
            string r = null;

            try
            {
                RunStep(() => r = KeystoreClient.CreateSelfSignedCertificate(X509Version, Subject, KeyID, Alias, notValidBefore, notValidAfter, SignatureAlgorithm, Extension),
                        "Create Self Signed Certificate");
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertificatesReached"))
                    LogStepEvent(fpMsgCertificate);

                throw;
            }


            return r;
        }

        private void DeleteCertificate(string certID)
        {
            RunStep(() => KeystoreClient.DeleteCertificate(certID),
                    string.Format("Delete Certificate with ID = '{0}'", certID));
        }

        private Proxies.Onvif.X509Certificate GetCertificate(string certificateID)
        {
            Proxies.Onvif.X509Certificate r = null;

            RunStep(() => r = KeystoreClient.GetCertificate(certificateID),
                    string.Format("Get Certificate with ID = '{0}'", certificateID));

            return r;
        }

        private IEnumerable<Proxies.Onvif.X509Certificate> GetAllCertificates()
        {
            IEnumerable<Proxies.Onvif.X509Certificate> r = null;

            RunStep(() => r = KeystoreClient.GetAllCertificates(),
                    "Get All Certificates");

            return r ?? new List<Proxies.Onvif.X509Certificate>();
        }

        private string CreateCertificationPath(CertificateIDs CertificateIDs, string Alias)
        {
            string r = null;

            try
            {
                RunStep(() => r = KeystoreClient.CreateCertificationPath(CertificateIDs, Alias),
                        "Create Certification Path");
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertificationPathsReached"))
                    LogStepEvent(fpMsgCertificationPath);

                throw;
            }

            return r;
        }
        
        private void DeleteCertificationPath(string certificationPathID)
        {
            RunStep(() => KeystoreClient.DeleteCertificationPath(certificationPathID),
                    string.Format("Delete Certification Path with ID = '{0}'", certificationPathID));
        }

        private CertificationPath GetCertificationPath(string CertificationPathID)
        {
            CertificationPath r = null;

            RunStep(() => r = KeystoreClient.GetCertificationPath(CertificationPathID),
                    string.Format("Get Certification Path with ID = '{0}'", CertificationPathID));

            return r;
        }

        private IEnumerable<string> GetAllCertificationPaths()
        {
            IEnumerable<string> r = null;

            RunStep(() => r = KeystoreClient.GetAllCertificationPaths(),
                    "Get All Certification Paths");

            if (null != r)
                Assert(r.Distinct().Count() == r.Count(),
                       "There are duplicates in received list of certification path's IDs",
                       "Check that received certification path's IDs are unique");


            return r ?? new List<string>();
        }

        private void AddServerCertificateAssignment(string CertificationPathID)
        {
            try
            {
                RunStep(() => TLSServer.AddServerCertificateAssignment(CertificationPathID),
                        string.Format("Add Server Certificate Assignment(CertificationPathID = '{0}')", CertificationPathID));
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertPathsReached"))
                    LogStepEvent(fpMsgCertificateAssignment);

                throw;
            }
        }

        private void RemoveServerCertificateAssignment(string CertificationPathID)
        {
            RunStep(() => TLSServer.RemoveServerCertificateAssignment(CertificationPathID),
                    string.Format("Remove Server Certificate Assignment(CertificationPathID = '{0}')", CertificationPathID));
        }

        private void ReplaceServerCertificateAssignment(string oldCertificationPathID, string newCertificationPathID)
        {
            RunStep(() => TLSServer.ReplaceServerCertificateAssignment(oldCertificationPathID, newCertificationPathID),
                    string.Format("Replace Server Certificate Assignment from '{0}' to '{1}'", oldCertificationPathID, newCertificationPathID));
        }

        private IEnumerable<string> GetAssignedServerCertificates()
        {
            IEnumerable<string> r = null;

            RunStep(() => r = TLSServer.GetAssignedServerCertificates(),
                    "Get Assigned Server Certificates");

            if (null != r)
                Assert(r.Distinct().Count() == r.Count(),
                       "There are duplicates in received list of assigned server certificate's IDs",
                       "Check that received assigned server certificate's IDs are unique");

            return r ?? new List<string>();
        }

        private string GetKeyStatus(string keyID)
        {
            string r = null;

            RunStep(() => r = KeystoreClient.GetKeyStatus(keyID),
                    string.Format("Get Key Status with ID = '{0}'", keyID));

            return r;
        }

        private bool GetPrivateKeyStatus(string keyID)
        {
            bool r = false;

            RunStep(() => r = KeystoreClient.GetPrivateKeyStatus(keyID),
                    string.Format("Get Private Key Status with ID = '{0}'", keyID));

            return r;
        }

        private byte[] CreatePKCS10CSR(DistinguishedName Subject, string KeyID, CSRAttribute[] CSRAttribute, AlgorithmIdentifier SignatureAlgorithm)
        {
            byte[] r = null;

            RunStep(() => r = KeystoreClient.CreatePKCS10CSR(Subject, KeyID, CSRAttribute, SignatureAlgorithm),
                    "CreatePKCS10CSR");

            return r;

        }

        private void UploadCertificate(byte[] Certificate, string Alias, bool PrivateKeyRequired,
                                       out string KeyID, out string CertID, 
                                       string expectedKeyID = null, bool preconditionCertNumber = true, bool preconditionKeysNumber = true)
        {


            try
            {
                string localKeyID = null;
                string r = "";
                RunStep(() => r = KeystoreClient.UploadCertificate(Certificate, Alias, PrivateKeyRequired, out localKeyID),
                        "Upload Certificate");
                CertID = r;
                KeyID = localKeyID;
            }
            catch (FaultException e)
            {
                if (preconditionCertNumber && e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertificatesReached"))
                    LogStepEvent(fpMsgCertificate);

                if (preconditionKeysNumber && e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfKeysReached"))
                    LogStepEvent(fpMsgRSAKeyPair);

                throw;
            }


            if (null != expectedKeyID)
                Assert(expectedKeyID == KeyID,
                       string.Format("Certificate is uploaded for RSA key pair with ID = '{0}'. But actually it is linked to RSA key pair with ID = '{1}'", expectedKeyID, KeyID), 
                       "Checking returned KeyID");
        }

        private void UploadCertificate(byte[] Certificate, string Alias, bool PrivateKeyRequired,
                                       out string CertID, 
                                       string expectedKeyID = null, bool preconditionCertNumber = true, bool preconditionKeysNumber = true)
        {
            string keyID;
            UploadCertificate(Certificate, Alias, PrivateKeyRequired, out keyID, out CertID, expectedKeyID, preconditionCertNumber, preconditionKeysNumber);
        }

        #endregion

        #region Device Management commands

        protected Service[] GetServices(bool includeCapabilities)
        {
            //Service[] services = null;
            //RunStep(() => { services = Client.GetServices(includeCapabilities); }, "Get Services");
            //DoRequestDelay();
            //return services;
            return CommonMethodsProvider.GetServices(this, DeviceClient, includeCapabilities);
        }

        #endregion

        #region Utils

        /// <summary>
        /// Called at start of all tests.
        /// Performs check that Bouncy Castle crypto library is available and Advanced Security Service is accessible.
        /// </summary>
        private void GeneralPrerequisites()
        {
            bool cryptoIsAvailable = true;
            var msg = new StringBuilder();
            try
            { AppDomain.CurrentDomain.Load(new AssemblyName("BouncyCastle.Crypto")); }
            catch (Exception e)
            {
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                msg.AppendLine("ONVIF TT unable to load Bouncy Castle Crypto library.");
                msg.AppendLine("Please install it using the following instruction:");
                msg.AppendLine(string.Format(@"{0}\{1}", directory, "BouncyCastle.rtf"));
                cryptoIsAvailable = false;
            }

            Assert(cryptoIsAvailable,
                   msg.ToString(),
                   "Check crypto library is available");

            Assert(null != Client && null != KeystoreClient && null != TLSServer,
                   "Can't connect to Advance Security Service",
                   "Check that Advance Security Service is accessible");
        }

        private static IEnumerable<uint> SelectKeyLengthsForTest(IEnumerable<uint> keyLengths, uint limit = 4096)
        {
            var r = keyLengths.Where(e => e <= limit);

            return r.Any() ? r : new [] { keyLengths.Min() };
        }

        /// <summary>
        /// Annex A.8: helper procedure to create a self-signed certificate.
        /// </summary>
        /// <param name="testRSAKeyID">ID of created RSA keypair</param>
        /// <param name="notBefore">start of certificate's validity period</param>
        /// <param name="notAfter">end of certificate's validity period</param>
        /// <returns>ID of created certificate</returns>
        private string CreateTestSelfSignedCertificateA8(out string testRSAKeyID, DateTime notBefore = new DateTime(), DateTime notAfter = new DateTime())
        {
            CreateTestRSAKeyPairA7(out testRSAKeyID);

            return CreateSelfSignedCertificate(null, DefaultSubject, testRSAKeyID, null, notBefore, notAfter, null, null);
        }

        /// <summary>
        /// Annex A.4: helper procedure to create an X.509 CA certificate.
        /// </summary>
        /// <param name="key">keypair used for certificate's creation</param>
        /// <returns>X509 certificate</returns>
        private X509CertificateBase CreateTestSelfSignedCACertificateA4(out RSAKeyPair key)
        {
            var keyPairGenerator = new RSAKeyPairGeneratorBC(1024);

            key = keyPairGenerator.Generate();
            
            var dn = "CN=ONVIF TT,C=US";
            var generator = new X509CertificateGeneratorBC();
            generator.SetSerialNumber(DateTime.Now.Ticks.ToString());
            generator.SetIssuerDN(dn);
            generator.SetPublicKey(key.PublicKey);
            generator.SetNotValidBefore(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            generator.SetNotValidAfter(new DateTime(9999, 12, 31, 23, 59, 59, 0));
            generator.SetSubjectDN(dn);

            generator.SetSignatureAlgorithm("SHA1WithRSAEncryption");

            return generator.Generate(key.PrivateKey);
        }

        private X509CertificateBase CreateTestSelfSignedCACertificateA4()
        {
            RSAKeyPair q = null;
            return CreateTestSelfSignedCACertificateA4(out q);
        }

        /// <summary>
        /// Annex A.14: helper procedure to create a CA-signed certificate for RSA key pair.
        /// </summary>
        /// <param name="caCertificate">CA certificate by which generated certificate should be signed</param>
        /// <param name="caKeyPair">RSA keypair corresponding to CA certificate</param>
        /// <param name="testRSAKeyID">ID of RSA keypair for that certificate is created</param>
        /// <returns></returns>
        private X509CertificateBase CreateTestCertificateSignedByCACertificateA14(X509CertificateBase caCertificate, RSAKeyPair caKeyPair, string testRSAKeyID)
        {
            var r = CreatePKCS10CSR(DefaultSubject, testRSAKeyID, null, null);

            var request = ValidateCertificateSigningRequest(r);

            var generator = new X509CertificateGeneratorBC();
            generator.SetSerialNumber(DateTime.Now.Ticks.ToString());
            generator.SetIssuerDN(caCertificate.SubjectDN);
            generator.SetSubjectDN(request.SubjectDN);
            generator.SetPublicKey(request.PublicKey);
            generator.SetNotValidBefore(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            generator.SetNotValidAfter(new DateTime(9999, 12, 31, 23, 59, 59, 0));
            generator.SetSignatureAlgorithm("SHA1WithRSAEncryption");

            try
            { generator.CopyExtensions(request.Extensions); }
            catch (Exception)
            {}

            return generator.Generate(caKeyPair.PrivateKey);
        }

        private CertificateSigningRequestBC ValidateCertificateSigningRequest(byte[] r, string expectedSubject = null)
        {
            CertificateSigningRequestBC request = null;

            var msg = "";
            var reqFlag = true;
            try
            {
                request = new CertificateSigningRequestBC(new MemoryStream(r));
                
                if (null != expectedSubject)
                    reqFlag = X509NamesAreEqual (request.SubjectDN, expectedSubject);
                if (!reqFlag)
                    msg = string.Format("Received PKCS10 Certificate Signing Request has invalid Subject: {0}. Expected: {1}", request.SubjectDN, DefaultSubjectString);
            }
            catch (Exception e)
            {
                msg = string.Format("Received PKCS10 Certificate Signing Request is invalid: {0}", !r.Any() ? "empty response" : e.Message);
                reqFlag = false;
            }

            Assert(reqFlag,
                   msg,
                   "Validating received PKCS10 Certificate Signing Request");

            Assert(request.Verify(),
                   "Received PKCS10 Certificate Signing Request has invalid signature",
                   "Verifying signature on PKCS10 Certificate Signing Request");
            return request;
        }

        private X509CertificateBase CreateCASignedCertificateA14(X509CertificateBase caCertificate, RSAKeyPair caKeyPair, out string rsaKeyPairID)
        {
            CreateTestRSAKeyPairA7(out rsaKeyPairID);

            return CreateTestCertificateSignedByCACertificateA14(caCertificate, caKeyPair, rsaKeyPairID);
        }

        /// <summary>
        /// Annex A.15: helper procedure to upload a certificate without private key assignment.
        /// </summary>
        /// <param name="certificate">Certificate to be loaded</param>
        /// <param name="keyID">ID of RSA keypair for that certificate is created</param>
        /// <returns></returns>
        private void UploadCertificateWithoutPrivateKeyA15(X509CertificateBase certificate, out string keyID, out string certID)
        {
            UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", false, out keyID, out certID);
        }

        /// <summary>
        /// Annex A.11: helper procedure to create a certification path based on self-signed certificate.
        /// </summary>
        /// <param name="testRSAKeyID"></param>
        /// <param name="testCertID"></param>
        /// <param name="alias"></param>
        /// <returns>ID of created test certification path</returns>
        private string CreateTestSelfSignedCertificationPathA11(out string testRSAKeyID, out string testCertID, string alias = "ONVIF_TEST")
        {
            testCertID = CreateTestSelfSignedCertificateA8(out testRSAKeyID);

            return CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { testCertID } }, alias);
        }

        /// <summary>
        /// Annex A.18: helper procedure to create a certification path based on CA-signed certificate and associated CA certificate.
        /// </summary>
        /// <param name="CARSAKeyID"></param>
        /// <param name="CACertID"></param>
        /// <param name="RSAKeyID"></param>
        /// <param name="certID"></param>
        /// <param name="alias"></param>
        /// <returns>ID of created test certification path</returns>
        private string CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out string CARSAKeyID, out string CACertID,
                                                                                  out string RSAKeyID, out string certID,
                                                                                  string alias = "ONVIF_Test2")
        {
            RSAKeyPair keyPair = null;
            var caCertificate = CreateTestSelfSignedCACertificateA4(out keyPair);
            CreateAndUploadCASignedCertificateA16(caCertificate, keyPair, out RSAKeyID, out certID);

            string localCARSAKeyID;
            var localCACertID = "";
            UploadCertificateWithoutPrivateKeyA15(caCertificate, out localCARSAKeyID, out localCACertID);

            var flagKey = RSAKeyID != localCARSAKeyID;
            var flagCert = certID != localCACertID;
            var msg = new StringBuilder();
            var flag = true;
            if (!flagKey)
            {
                flag = false;
                msg.Append("The DUT has returned the same IDs for Test certificate and CA certificate");
                CARSAKeyID = null;
            }
            else
                CARSAKeyID = localCARSAKeyID;

            if (!flagCert)
            {
                flag = false;
                if (0 != msg.Length) msg.AppendLine();
                msg.Append("The DUT has returned the same IDs for Test keypair and CA's keypair");
                CACertID = null;
            }
            else
                CACertID = localCACertID;

            Assert(flag, msg.ToString(), "Checking returned IDs");

            return CreateCertificationPath(new CertificateIDs() { CertificateID = new string[] { CACertID, certID } }, alias);
        }

        private void CreateAndUploadCASignedCertificateA16(X509CertificateBase caCertificate, RSAKeyPair caKeyPair, out string certKeyID, out string certID)
        {
            var certificate = CreateCASignedCertificateA14(caCertificate, caKeyPair, out certKeyID);

            UploadCertificate(certificate.GetEncoded(), "ONVIF_Test1", true, out certID, certKeyID);
        }

        /// <summary>
        /// Checks if received certification path is as expected.
        /// </summary>
        /// <param name="certificationPath">received certification path</param>
        /// <param name="expectedCertificateSequence">certificate's sequence supposed to be in certificationPath</param>
        /// <param name="log">to store log messages</param>
        /// <returns>true if received certification path is as expected</returns>
        private bool ValidateReceivedCertificationPath(CertificationPath certificationPath,
                                                       string[] expectedCertificateSequence,
                                                       StringBuilder log)
		{
            if (null == certificationPath)
            {
                log.Append("Unable to receive Certification Path");
                return false;
            }

            if (null == certificationPath.CertificateID)
            {
                log.Append("Received Certification Path contains no certificate's ID");
                return false;
            }

            if (expectedCertificateSequence.Count() != certificationPath.CertificateID.Count())
            {
                log.AppendFormat("Received Certification Path contains unexpected number of elements: {0} instead of {1}",
                                 certificationPath.CertificateID.Count(), expectedCertificateSequence.Count());
                return false;
            }
            
            if (!Enumerable.Range(0, certificationPath.CertificateID.Count()).All(i => certificationPath.CertificateID[i] == expectedCertificateSequence[i]))
            {
                log.Append("Received Certification Path is not the same as expected");
                return false;
            }

            return true;
		}

        /// <summary>
        /// Annex A.6: helper procedure to determine the RSA key length to use during testing.
        /// </summary>
        /// <returns>Size of RSA keypair to be used during test</returns>
        private uint GetKeyLengthForTestRSAKeyPairA6()
        {
            var r = GetCapabilities();

            Assert(null != r.KeystoreCapabilities.RSAKeyLengths && r.KeystoreCapabilities.RSAKeyLengths.Any(),
                   "Determination of key length for test key pair is failed: the DUT doesn't support any key length",
                   "Determine key length for test key pair");

            return r.KeystoreCapabilities.RSAKeyLengths.Min();
        }

        /// <summary>
        /// Annex A.7: helper procedure to create a RSA key pair.
        /// </summary>
        /// <param name="keyID"></param>
        private void CreateTestRSAKeyPairA7(out string keyID)
        {
            var keyLength = GetKeyLengthForTestRSAKeyPairA6();

            CreateTestRSAKeyPairOfSpecifiedLength(out keyID, keyLength);
        }

        private string ConvertTimeSpanToHumanReadableFormat(TimeSpan T)
        {
            if (0 != T.Hours)
                return string.Format("{0} day(s), {1} hour(s), {2} minute(s) and {3} seconds", 
                                     T.Days, T.Hours, T.Minutes, T.Seconds);

            if (0 != T.Hours)
                return string.Format("{0} hour(s), {1} minute(s) and {2} seconds", T.Hours, T.Minutes, T.Seconds);

            if (0 != T.Minutes)
                return string.Format("{0} minute(s) and {1} seconds", T.Minutes, T.Seconds);

            if (0 != T.Seconds)
                return string.Format("{0} seconds", T.Seconds);

            return "less than a second";
        }

        /// <summary>
        /// The same as Annex A.8 but the length of created key is specified manually.
        /// </summary>
        /// <param name="keyID"></param>
        /// <param name="keyLength"></param>
        private void CreateTestRSAKeyPairOfSpecifiedLength(out string keyID, uint keyLength)
        {
            string estimatedCreationTime;
            keyID = CreateRSAKeyPair(keyLength.ToString(), null, out estimatedCreationTime);

            var duration = new TimeSpan();
            try
            {
                duration = XmlConvert.ToTimeSpan(estimatedCreationTime);
            }
            catch (Exception e)
            {
                Assert(false,
                       string.Format("Invalid format for Estimated Creation Time: {0}", e.Message),
                       "Parsing Estimated Creation Time");
            }

            var timeout = _operationDelay/1000;
            var deadline = DateTime.Now.AddSeconds(timeout);

            LogStepEvent(string.Format("The DUT has reported key pair creation takes about {0}.", ConvertTimeSpanToHumanReadableFormat(duration)));

            bool first = true;
            var keyStatus = "";
            while (DateTime.Now <= deadline && ksOK != keyStatus && ksCORRUPT != keyStatus)
            {
                if (first)
                {
                    LogStepEvent("The status of key pair will be checked after specified amount of time.");
                    first = false;
                }
                else
                    LogStepEvent(string.Format("The status of key pair will be checked once again after {0}.", ConvertTimeSpanToHumanReadableFormat(duration)));
                LogStepEvent("");

                if (this.Semaphore.StopEvent.WaitOne(duration))
                    this.Semaphore.ReportStop();

                keyStatus = GetKeyStatus(keyID);
            }

            var msg = DateTime.Now > deadline
                          ? string.Format("Timeout for key pair creation is expired{0}Increase Operation Delay on Management tab to increase timeout.", Environment.NewLine)
                          : string.Format("Key status is other than '{0}'", ksOK);
            Assert(ksOK == keyStatus,
                   msg,
                   string.Format("Check key status of key pair is '{0}'", ksOK));
        }

        /// <summary>
        /// Utility to create topic filter for event's subscription.
        /// </summary>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        TestTool.Proxies.Event.FilterType CreateFilter(TopicInfo topicInfo)
        {
            var filter = new Proxies.Event.FilterType();

            var filterDoc = new XmlDocument();
            var filterTopicElement = filterDoc.CreateTopicElement();

            var topicPath = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

            filterTopicElement.InnerText = topicPath;

            filter.Any = new [] { filterTopicElement };

            return filter;
        }

        /// <summary>
        /// Utility to insert new RSA keypair and check result of this operation.
        /// </summary>
        /// <param name="keyListBefore"></param>
        /// <param name="rsaKeyID"></param>
        /// <returns>List of presented RSA keypairs after creation of new one</returns>
        IEnumerable<KeyAttribute> UpdateRSAKeyList(IEnumerable<string> keyListBefore, out string rsaKeyID)
        {
            CreateTestRSAKeyPairA7(out rsaKeyID);
            Assert(!keyListBefore.Contains(rsaKeyID),
                   "Initial key's list already contains ID of just created key",
                   "Check just created key's ID");

            var updatedKeysList = GetAllKeys();
            var updatedKeyIDsList = updatedKeysList.Select(e => e.KeyID);

            bool flag = keyListBefore.All(updatedKeyIDsList.Contains) && updatedKeyIDsList.Contains(rsaKeyID) && keyListBefore.Count() + 1 == updatedKeyIDsList.Count();
            var msg = "";
            if (!flag)
            {
                if (!keyListBefore.All(updatedKeyIDsList.Contains))
                    msg = "Key's list received before test key's creation contains entries that isn't present in key's list received later";
                else if (!updatedKeyIDsList.Contains(rsaKeyID))
                    msg = "Key's list received after test key's creation doesn't contain just created key";
                else if (keyListBefore.Count() + 1 != updatedKeyIDsList.Count())
                    msg = "Key's list received after test key's creation contains entry another than just created or from initial key's list";
            }

            Assert(flag,
                   msg,
                   "Check key's list received after test key's creation");

            return updatedKeysList;
        }

        /// <summary>
        /// Utility to check that creation of new certificate is performed successfully.
        /// </summary>
        /// <param name="certListBefore"></param>
        /// <param name="certID"></param>
        /// <returns>List of presented certificates</returns>
        IEnumerable<Proxies.Onvif.X509Certificate> ValidationAfterCertificateListUpdate(IEnumerable<string> certListBefore, string certID)
        {
            Assert(!certListBefore.Contains(certID),
                   "Initial certificate's list already contains ID of just created certificate",
                   "Check just created certificate's ID");

            var updatedCertsList = GetAllCertificates();
            var updatedCertsIDList = updatedCertsList.Select(e => e.CertificateID);

            bool flag = certListBefore.All(updatedCertsIDList.Contains) && updatedCertsIDList.Contains(certID) && certListBefore.Count() + 1 == updatedCertsList.Count();
            var msg = "";
            if (!flag)
            {
                if (!certListBefore.All(updatedCertsIDList.Contains))
                    msg = "Certificate's list received before test certificate's creation contains entries that isn't present in certificate's list received later";
                else if (!updatedCertsIDList.Contains(certID))
                    msg = "Certificate's list received after test certificate's creation doesn't contain just created certificate";
                else if (certListBefore.Count() + 1 != updatedCertsIDList.Count())
                    msg = "Certificate's list received after test certificate's creation contains entry another than just created or from initial certificate's list";
            }

            Assert(flag,
                   msg,
                   "Check certificate's list received after test certificate's creation");

            return updatedCertsList;
        }

        /// <summary>
        ///  Utility to create new self-signed certificate and check if this operation is performed successfully.
        /// </summary>
        IEnumerable<Proxies.Onvif.X509Certificate> UpdateCertificateListBySelfSignedCertificate(IEnumerable<string> certListBefore, out string rsaKeyID, out string certID)
        {
            certID = CreateTestSelfSignedCertificateA8(out rsaKeyID);

            return ValidationAfterCertificateListUpdate(certListBefore, certID);
        }

        /// <summary>
        ///   Utility to create new certificate signed by CA certificate and check if this operation is performed successfully.
        /// </summary>
        IEnumerable<Proxies.Onvif.X509Certificate> UpdateCertificateListByCASignedCertificate(IEnumerable<string> certListBefore, out string rsaKeyID, out string certID)
        {
            var caCertificate = CreateTestSelfSignedCACertificateA4();
            UploadCertificateWithoutPrivateKeyA15(caCertificate, out rsaKeyID, out certID);

            return ValidationAfterCertificateListUpdate(certListBefore, certID);
        }

        /// <summary>
        /// Utility to check that creation of new certification path is performed successfully.
        /// </summary>
        /// <param name="certPathListBefore"></param>
        /// <param name="certPathID"></param>
        /// <returns>List of presented certification paths</returns>
        private IEnumerable<string> ValidationAfterCertificationPathListUpdate(IEnumerable<string> certPathListBefore, string certPathID)
        {
            Assert(!certPathListBefore.Contains(certPathID),
                   "Initial certification path's list already contains ID of just created path",
                   "Check just created certification path's ID");

            var updatedCertPathsList = GetAllCertificationPaths();

            bool flag = certPathListBefore.All(updatedCertPathsList.Contains) && updatedCertPathsList.Contains(certPathID) && certPathListBefore.Count() + 1 == updatedCertPathsList.Count();
            var msg = "";
            if (!flag)
            {
                if (!certPathListBefore.All(updatedCertPathsList.Contains))
                    msg = "Certification path's list received before test certification path's creation contains entries that isn't present in certification path's list received later";
                else if (!updatedCertPathsList.Contains(certPathID))
                    msg = "Certification path's list received after test certification path's creation doesn't contain just created certification path";
                else if (certPathListBefore.Count() + 1 != updatedCertPathsList.Count())
                    msg = "Certification path's list received after test certification path's creation contains entry another than just created or from initial certification path's list";
            }

            Assert(flag,
                   msg,
                   "Check certification path's list received after test certification path's creation");

            return updatedCertPathsList;
        }

        /// <summary>
        ///   Utility to create new certification path and check if this operation is performed successfully.
        /// </summary>
        IEnumerable<string> UpdateCertificationPathList(IEnumerable<string> certPathListBefore, out string rsaKeyID, out string certID, out string certPathID)
        {
            certPathID = CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);
            
            return ValidationAfterCertificationPathListUpdate(certPathListBefore, certPathID);
        }

        /// <summary>
        ///   Utility to create new certification path and check if this operation is performed successfully.
        /// </summary>
        IEnumerable<string> UpdateCertificationPathListByPathBasedOnCAAndCASignedCertificates(IEnumerable<string> certPathListBefore,
                                                                                              out string CARSAKeyID, out string CACertID,
                                                                                              out string RSAKeyID, out string certID,
                                                                                              out string certPathID)
        {
            certPathID = CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out CARSAKeyID, out CACertID, out RSAKeyID, out certID);
            
            return ValidationAfterCertificationPathListUpdate(certPathListBefore, certPathID);
        }

        /// <summary>
        /// Utility to check that AddServerCertificateAssignment command is performed successfully.
        /// </summary>
        /// <param name="certPathListBefore"></param>
        /// <param name="certPathID"></param>
        /// <returns>List of presented assigned server certificates</returns>
        private IEnumerable<string> ValidationAfterAssignedServerCertificateListUpdate(IEnumerable<string> certPathListBefore, string certPathID)
        {
            var updatedCertsList = GetAssignedServerCertificates();

            bool flag = certPathListBefore.All(updatedCertsList.Contains) && updatedCertsList.Contains(certPathID) && certPathListBefore.Count() + 1 == updatedCertsList.Count();
            var msg = "";
            if (!flag)
            {
                if (!certPathListBefore.All(updatedCertsList.Contains))
                    msg = "Certificate's list received before test certificate's creation contains entries that isn't present in certificate's list received later";
                else if (!updatedCertsList.Contains(certPathID))
                    msg = "Certificate's list received after test certificate's creation doesn't contain just created certificate";
                else if (certPathListBefore.Count() + 1 != updatedCertsList.Count())
                    msg = "Certificate's list received after test certificate's creation contains entry another than just created or from initial certificate's list";
            }

            Assert(flag,
                   msg,
                   "Check certificate's list received after test certificate's creation");

            return updatedCertsList;
        }

        /// <summary>
        /// Annex A.13: helper procedure to add server certificate assignment with corresponding certification path, self-signed certificate and RSA key pair.
        /// </summary>
        /// <param name="rsaKeyID"></param>
        /// <param name="certID"></param>
        /// <param name="certificateAssignmentAdded"></param>
        /// <returns>ID of created test certification path</returns>
        void AddSelfSignedCertificateAsAssignedServerCertificateA13(out string rsaKeyID, out string certID, out string certPathID, out bool certificateAssignmentAdded)
        {
            certPathID = CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

            AddServerCertificateAssignment(certPathID);
            certificateAssignmentAdded = true;
        }

        /// <summary>
        ///   Utility to add new assigned server certificate and check if this operation is performed successfully.
        /// </summary>
        IEnumerable<string> UpdateAssignedServerCertificateListByPathBaseOnSelfSignedCertificate(IEnumerable<string> certListBefore, 
                                                                                                 out string rsaKeyID, out string certID,
                                                                                                 out string certPathID, out bool certificateAssignmentAdded)
        {
            certPathID = CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);
            Assert(!certListBefore.Contains(certPathID),
                   "Initial certification path's list already contains ID of just created path",
                   "Check just created certification path's ID");

            AddServerCertificateAssignment(certPathID);
            certificateAssignmentAdded = true;

            return ValidationAfterAssignedServerCertificateListUpdate(certListBefore, certPathID);
        }

        /// <summary>
        ///   Utility to add new assigned server certificate and check if this operation is performed successfully.
        /// </summary>
        IEnumerable<string> UpdateAssignedCertificateListByPathBasedOnCAAndCASignedCertificatesA18(IEnumerable<string> certPathListBefore,
                                                                                                   out string caKeyID, out string caCertID,
                                                                                                   out string rsaKeyID, out string certID,
                                                                                                   out string certPathID, out bool certificateAssignmentAdded)
        {
            certPathID = CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out caKeyID, out caCertID, out rsaKeyID, out certID);
            Assert(!certPathListBefore.Contains(certPathID),
                   "Initial certification path's list already contains ID of just created path",
                   "Check just created certification path's ID");

            AddServerCertificateAssignment(certPathID);
            certificateAssignmentAdded = true;

            return ValidationAfterAssignedServerCertificateListUpdate(certPathListBefore, certPathID);
        }

        protected T ExtractCapabilities<T>(XmlElement element, string ns)
        {
            BeginStep("Parse Capabilities element in GetServices response");

            System.Xml.Serialization.XmlRootAttribute xRoot = new System.Xml.Serialization.XmlRootAttribute();
            xRoot.ElementName = "Capabilities";
            xRoot.IsNullable = true;
            xRoot.Namespace = ns;

            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);

            XmlReader reader = new XmlNodeReader(element);

            T capabilities;
            try
            {
                capabilities = (T)serializer.Deserialize(reader);
            }
            catch (Exception exc)
            {
                string message;
                if (exc.InnerException != null)
                {
                    message = string.Format("{0} {1}", exc.Message, exc.InnerException.Message);
                }
                else
                {
                    message = exc.Message;
                }
                throw new ApplicationException(message);
            }
            StepPassed();
            return capabilities;
        }

        protected AdvancedSecurityCapabilities ExtractAdvancedSecurityCapabilities(XmlElement element)
        {
            return ExtractCapabilities<AdvancedSecurityCapabilities>(element, OnvifService.ADVANCED_SECURITY);
        }

        void ValidateCapabilitiesAttribute(string attributeName,
                                                                       bool attributeSpecified1, bool attributeSpecified2,
                                                                       string attributeValue1, string attributeValue2, 
                                                                       ref StringBuilder sb, ref bool ok)
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1 != attributeValue2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
                else if (attributeSpecified1 &&
                    !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped", attributeName));
                }
            }
        }

        void ValidateCapabilitiesAttribute(string attributeName,
                                                                       bool attributeSpecified1, bool attributeSpecified2,
                                                                       bool attributeValue1, bool attributeValue2,
                                                                       ref StringBuilder sb, ref bool ok)
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1 != attributeValue2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2 && attributeValue2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
                else if (attributeSpecified1 && attributeValue1 &&
                    !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped", attributeName));
                }
            }
        }

        void ValidateCapabilitiesAttribute(string attributeName,
                                                                       bool attributeSpecified1, bool attributeSpecified2,
                                                                       int[] attributeValue1, int[] attributeValue2,
                                                                       ref StringBuilder sb, ref bool ok)
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1.Length != attributeValue2.Length)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));
                }
                else
                {
                    List<int> foundItems = new List<int>();
                    for (int i = 0; i < attributeValue1.Length; i++ )
                    {
                        int index = Array.IndexOf(attributeValue2, attributeValue1[i]);

                        if (attributeValue2.Contains(attributeValue1[i]))
                        {
                            if (!foundItems.Contains(index))
                            {
                                foundItems.Add(index);
                            }
                            else
                            {
                                while (foundItems.Contains(index))
                                {
                                    index = Array.IndexOf(attributeValue2, attributeValue1[i], index + 1);
                                    if (index == -1)
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("{0} values are different.", attributeName));
                                        sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                                        sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));

                                        break;
                                    }
                                }

                                if (index != -1)
                                    foundItems.Add(index);
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine(string.Format("{0} values are different.", attributeName));
                            sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                            sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));

                            break;
                        }
                    }
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped or empty", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));
                }
                else if (attributeSpecified1 &&
                    !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped or empty", attributeName));
                }
            }
        }

        void ValidateCapabilitiesAttribute(string attributeName,
                                                                       bool attributeSpecified1, bool attributeSpecified2,
                                                                       object[] attributeValue1, object[] attributeValue2,
                                                                       ref StringBuilder sb, ref bool ok)
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1.Length != attributeValue2.Length)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));
                }
                else
                {
                    List<int> foundItems = new List<int>();
                    for (int i = 0; i < attributeValue1.Length; i++)
                    {
                        int index = Array.IndexOf(attributeValue2, attributeValue1[i]);

                        if (attributeValue2.Contains(attributeValue1[i]))
                        {
                            if (!foundItems.Contains(index))
                            {
                                foundItems.Add(index);
                            }
                            else
                            {
                                while (foundItems.Contains(index))
                                {
                                    index = Array.IndexOf(attributeValue2, attributeValue1[i], index + 1);
                                    if (index == -1)
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("{0} values are different.", attributeName));
                                        sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                                        sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));

                                        break;
                                    }
                                }

                                if (index != -1)
                                    foundItems.Add(index);
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine(string.Format("{0} values are different.", attributeName));
                            sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                            sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));

                            break;
                        }
                    }
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped or empty", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue2)));
                }
                else if (attributeSpecified1 &&
                    !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", attributeName, string.Join(" ", attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped or empty", attributeName));
                }
            }
        }

        bool CompareAlgorithmIdentifier(AlgorithmIdentifier algId1, AlgorithmIdentifier algId2)
        {
            bool ok = true;

            if (algId1.algorithm != algId2.algorithm)
            {
                ok = false;
            }
            else
            {
                byte[] parameters1 = null;
                byte[] parameters2 = null;

                bool parametersSpecified1= algId1.parameters != null && algId1.parameters.Length != 0;
                bool parametersSpecified2= algId2.parameters != null && algId2.parameters.Length != 0;

                if (parametersSpecified1)
                    parameters1 = algId1.parameters;

                if (parametersSpecified2)
                    parameters2 = algId2.parameters;

                if (parametersSpecified1 && parametersSpecified2)
                {
                    if (parameters1.Length != parameters2.Length)
                    {
                        ok = false;
                    }
                    else
                    {
                        for (int i = 0; i < parameters1.Length; i++)
                            if (parameters1[i] != parameters2[i])
                                ok = false;
                    }
                }
                else if (!parametersSpecified1 && parametersSpecified2)
                {
                    ok = false;
                }
                else if (parametersSpecified1 && !parametersSpecified2)
                {
                    ok = false;
                }
            }

            return ok;
        }

        void ValidateCapabilitiesElement(string elementName,
                                                                     bool elementSpecified1, bool elementSpecified2,
                                                                     AlgorithmIdentifier[] elementValue1, AlgorithmIdentifier[] elementValue2,
                                                                     ref StringBuilder sb, ref bool ok)
        {
            if (elementSpecified1 &&
                elementSpecified2)
            {
                List<string> algorithms1 = new List<string>();
                foreach (var item in elementValue1)
                    algorithms1.Add(item.algorithm);

                List<string> algorithms2 = new List<string>();
                foreach (var item in elementValue2)
                    algorithms2.Add(item.algorithm);

                if (elementValue1.Length != elementValue2.Length)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", elementName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"", elementName, string.Join(" ", algorithms1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"", elementName, string.Join(" ", algorithms2)));
                }
                else
                {
                    List<int> foundItems = new List<int>();
                    for (int i = 0; i < elementValue1.Length; i++)
                    {
                        AlgorithmIdentifier algId = elementValue2.FirstOrDefault(algid => CompareAlgorithmIdentifier(elementValue1[i], algid));
                        if (algId != null)
                        {
                            int index = Array.IndexOf(elementValue2, algId);
                            if (!foundItems.Contains(index))
                            {
                                foundItems.Add(index);
                            }
                            else
                            {
                                while (foundItems.Contains(index))
                                {
                                    index = Array.IndexOf(elementValue2, algId, index + 1);
                                    if (index == -1)
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("{0} values are different.", elementName));

                                        string parameters = null;
                                        if (algId.parameters != null && algId.parameters.Length != 0)
                                            parameters = Convert.ToBase64String(algId.parameters, Base64FormattingOptions.InsertLineBreaks);

                                        sb.AppendLine(string.Format("{0} value = {{{1}{2}}} in capabilities from GetServices is missed in capabilities from GetServiceCapabilities.",
                                        elementName,
                                        algId.algorithm,
                                        !string.IsNullOrEmpty(parameters) ? (", \"" + parameters + "\"") : ""));

                                        break;
                                    }
                                }

                                if (index != -1)
                                    foundItems.Add(index);
                            }
                        }
                        else 
                        {
                            ok = false;
                            sb.AppendLine(string.Format("{0} values are different.", elementName));

                            string parameters = null;
                            if (elementValue1[i].parameters != null && elementValue1[i].parameters.Length != 0)
                                parameters = Convert.ToBase64String(elementValue1[i].parameters, Base64FormattingOptions.InsertLineBreaks);

                            sb.AppendLine(string.Format("{0} value = {{{1}{2}}} in capabilities from GetServices is missed in capabilities from GetServiceCapabilities.",
                            elementName,
                            elementValue1[i].algorithm,
                            !string.IsNullOrEmpty(parameters) ? (", \"" + parameters + "\"") : ""));

                            break;
                        }
                    }
                }
            }
            else
            {
                if (!elementSpecified1 &&
                    elementSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", elementName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped or empty", elementName));
                }
                else if (elementSpecified1 &&
                    !elementSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", elementName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped or empty", elementName));
                }
            }
        }

        protected void CompareCapabilities(AdvancedSecurityCapabilities fromGetServices,
            AdvancedSecurityCapabilities fromGetServiceCapabilities)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = true;

            // SignatureAlgorithms
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool SignatureAlgorithmsSpecified1 = fromGetServices.KeystoreCapabilities.SignatureAlgorithms != null &&
                                                                                fromGetServices.KeystoreCapabilities.SignatureAlgorithms.Length != 0;

            bool SignatureAlgorithmsSpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                                                                                fromGetServiceCapabilities.KeystoreCapabilities.SignatureAlgorithms.Length != 0;

            AlgorithmIdentifier[] SignatureAlgorithms1 = fromGetServices.KeystoreCapabilities.SignatureAlgorithms;
            AlgorithmIdentifier[] SignatureAlgorithms2 = fromGetServiceCapabilities.KeystoreCapabilities.SignatureAlgorithms;

            ValidateCapabilitiesElement("SignatureAlgorithms",
                                                                 SignatureAlgorithmsSpecified1, SignatureAlgorithmsSpecified2,
                                                                 SignatureAlgorithms1, SignatureAlgorithms2,
                                                                 ref sb, ref ok);

            // MaximumNumberOfKeys
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfKeysSpecified1 = !string.IsNullOrEmpty(fromGetServices.KeystoreCapabilities.MaximumNumberOfKeys);
            bool MaximumNumberOfKeysSpecified2 = !string.IsNullOrEmpty(fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfKeys);
            string MaximumNumberOfKeys1 = fromGetServices.KeystoreCapabilities.MaximumNumberOfKeys;
            string MaximumNumberOfKeys2 = fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfKeys;

            ValidateCapabilitiesAttribute("MaximumNumberOfKeys",
                                                                  MaximumNumberOfKeysSpecified1, MaximumNumberOfKeysSpecified2,
                                                                  MaximumNumberOfKeys1, MaximumNumberOfKeys2,
                                                                  ref sb, ref ok);

            // MaximumNumberOfCertificates 
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfCertificatesSpecified1 = !string.IsNullOrEmpty(fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificates);
            bool MaximumNumberOfCertificatesSpecified2 = !string.IsNullOrEmpty(fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificates);
            string MaximumNumberOfCertificates1 = fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificates;
            string MaximumNumberOfCertificates2 = fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificates;

            ValidateCapabilitiesAttribute("MaximumNumberOfCertificates",
                                                                  MaximumNumberOfCertificatesSpecified1, MaximumNumberOfCertificatesSpecified2,
                                                                  MaximumNumberOfCertificates1, MaximumNumberOfCertificates2,
                                                                  ref sb, ref ok);

            // MaximumNumberOfCertificationPaths  
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfCertificationPathsSpecified1 = !string.IsNullOrEmpty(fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificationPaths);
            bool MaximumNumberOfCertificationPathsSpecified2 = !string.IsNullOrEmpty(fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths);
            string MaximumNumberOfCertificationPaths1 = fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificationPaths;
            string MaximumNumberOfCertificationPaths2 = fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths;

            ValidateCapabilitiesAttribute("MaximumNumberOfCertificationPaths",
                                                                  MaximumNumberOfCertificationPathsSpecified1, MaximumNumberOfCertificationPathsSpecified2,
                                                                  MaximumNumberOfCertificationPaths1, MaximumNumberOfCertificationPaths2,
                                                                  ref sb, ref ok);

            // RSAKeyPairGeneration 
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool RSAKeyPairGenerationSpecified1 = fromGetServices.KeystoreCapabilities.RSAKeyPairGenerationSpecified;
            bool RSAKeyPairGenerationSpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified;
            bool RSAKeyPairGeneration1 = fromGetServices.KeystoreCapabilities.RSAKeyPairGeneration;
            bool RSAKeyPairGeneration2 = fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyPairGeneration;

            ValidateCapabilitiesAttribute("RSAKeyPairGeneration",
                                                                  RSAKeyPairGenerationSpecified1, RSAKeyPairGenerationSpecified2,
                                                                  RSAKeyPairGeneration1, RSAKeyPairGeneration2,
                                                                  ref sb, ref ok);


            // RSAKeyLengths 
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool RSAKeyLengthsSpecified1 = fromGetServices.KeystoreCapabilities.RSAKeyLengths != null &&
                                                       fromGetServices.KeystoreCapabilities.RSAKeyLengths.Length != 0;

            bool RSAKeyLengthsSpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyLengths != null &&
                                                                   fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyLengths.Length != 0;

            uint[] RSAKeyLengths1 = fromGetServices.KeystoreCapabilities.RSAKeyLengths;
            uint[] RSAKeyLengths2 = fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyLengths;

            ValidateCapabilitiesAttribute("RSAKeyLengths",
                                                                  RSAKeyLengthsSpecified1, RSAKeyLengthsSpecified2,
                                                                  (int[])(object)RSAKeyLengths1, (int[])(object)RSAKeyLengths2,
                                                                  ref sb, ref ok);

            // PKCS10ExternalCertificationWithRSA
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool PKCS10ExternalCertificationWithRSASpecified1 = fromGetServices.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified;
            bool PKCS10ExternalCertificationWithRSASpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified;
            bool PKCS10ExternalCertificationWithRSA1 = fromGetServices.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA;
            bool PKCS10ExternalCertificationWithRSA2 = fromGetServiceCapabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA;

            ValidateCapabilitiesAttribute("PKCS10ExternalCertificationWithRSA",
                                                                  PKCS10ExternalCertificationWithRSASpecified1, PKCS10ExternalCertificationWithRSASpecified2,
                                                                  PKCS10ExternalCertificationWithRSA1, PKCS10ExternalCertificationWithRSA2,
                                                                  ref sb, ref ok);

            // SelfSignedCertificateCreationWithRSA
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool SelfSignedCertificateCreationWithRSASpecified1 = fromGetServices.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified;
            bool SelfSignedCertificateCreationWithRSASpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified;
            bool SelfSignedCertificateCreationWithRSA1 = fromGetServices.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA;
            bool SelfSignedCertificateCreationWithRSA2 = fromGetServiceCapabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA;

            ValidateCapabilitiesAttribute("SelfSignedCertificateCreationWithRSA",
                                                                  SelfSignedCertificateCreationWithRSASpecified1, SelfSignedCertificateCreationWithRSASpecified2,
                                                                  SelfSignedCertificateCreationWithRSA1, SelfSignedCertificateCreationWithRSA2,
                                                                  ref sb, ref ok);

            // X509Versions
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool X509VersionsSpecified1 = fromGetServices.KeystoreCapabilities.X509Versions != null && 
                                                                   fromGetServices.KeystoreCapabilities.X509Versions.Length != 0;

            bool X509VersionsSpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.X509Versions != null &&
                                                                   fromGetServiceCapabilities.KeystoreCapabilities.X509Versions.Length != 0;

            int[] X509Versions1 = fromGetServices.KeystoreCapabilities.X509Versions;
            int[] X509Versions2 = fromGetServiceCapabilities.KeystoreCapabilities.X509Versions;

            ValidateCapabilitiesAttribute("X509Versions",
                                                                  X509VersionsSpecified1, X509VersionsSpecified2,
                                                                  X509Versions1, X509Versions2,
                                                                  ref sb, ref ok);

            // TLSServerSupported
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool TLSServerSupportedSpecified1 = fromGetServices.TLSServerCapabilities != null &&
                                                                                fromGetServices.TLSServerCapabilities.TLSServerSupported != null &&
                                                                                fromGetServices.TLSServerCapabilities.TLSServerSupported.Length != 0;

            bool TLSServerSupportedSpecified2 = fromGetServiceCapabilities.TLSServerCapabilities != null &&
                                                                                fromGetServiceCapabilities.TLSServerCapabilities.TLSServerSupported != null &&
                                                                                fromGetServiceCapabilities.TLSServerCapabilities.TLSServerSupported.Length != 0;

            string[] TLSServerSupported1 = null;

            if (TLSServerSupportedSpecified1)
                TLSServerSupported1 =  fromGetServices.TLSServerCapabilities.TLSServerSupported;

            string[] TLSServerSupported2 = null;
            if (TLSServerSupportedSpecified2)
                TLSServerSupported2 = fromGetServiceCapabilities.TLSServerCapabilities.TLSServerSupported;

            ValidateCapabilitiesAttribute("TLSServerSupported",
                                                                   TLSServerSupportedSpecified1, TLSServerSupportedSpecified2,
                                                                   (object[])TLSServerSupported1, (object[])TLSServerSupported2,
                                                                   ref sb, ref ok);

            // MaximumNumberOfTLSCertificationPaths
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfTLSCertificationPathsSpecified1 = fromGetServices.TLSServerCapabilities != null &&
                !string.IsNullOrEmpty(fromGetServices.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths);

            bool MaximumNumberOfTLSCertificationPathsSpecified2 = fromGetServiceCapabilities.TLSServerCapabilities != null &&
                !string.IsNullOrEmpty(fromGetServiceCapabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths);

            string MaximumNumberOfTLSCertificationPaths1 = null;
            if (MaximumNumberOfTLSCertificationPathsSpecified1)
                MaximumNumberOfTLSCertificationPaths1= fromGetServices.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths;

            string MaximumNumberOfTLSCertificationPaths2 = null;
            if (MaximumNumberOfTLSCertificationPathsSpecified2)
                MaximumNumberOfTLSCertificationPaths2 = fromGetServiceCapabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths;

            ValidateCapabilitiesAttribute("MaximumNumberOfTLSCertificationPaths",
                                                                  MaximumNumberOfTLSCertificationPathsSpecified1, MaximumNumberOfTLSCertificationPathsSpecified2,
                                                                  MaximumNumberOfTLSCertificationPaths1, MaximumNumberOfTLSCertificationPaths2,
                                                                  ref sb, ref ok);

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump, "Check that capabilities from GetServices and GetServiceCapabilities is equal to each other");
        }

        #endregion

        #region Settings Recovery

        /// <summary>
        /// Flag to indicate that during settings restoration unexpected fault is occured
        /// </summary>
        [DefaultValue(false)]
        private bool RestoreSettingsFailed { get; set; }

        /// <summary>
        /// Performs action and doesn't break test's sequence if fault is occured.
        /// </summary>
        /// <param name="action"></param>
        void AllowFaultStep(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                RestoreSettingsFailed = true;
                StepFailed(e);
            }
        }

        /// <summary>
        ///  Performs action and passes test's step if expected fault is received.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exception"></param>
        void DoActionWithSOAPFault(Action action, string exception)
        {
            try
            {
                action();
                Assert(false, string.Format("The previous step was performed successfully while SOAP Fault '{0}' is expected.", exception), "Fail the test");
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault(exception))
                    StepPassed();
                else
                    throw;
            }
        }

        /// <summary>
        /// Called at the end of all tests: breaks test's sequence if any unexpected fault are received during settings restoration.
        /// </summary>
        void FinishRestoreSettings()
        {
            if (RestoreSettingsFailed)
                Assert(false, "One of steps during setting's recovery is failed", "Check setting's recovery is successfull");
        }

        #endregion

        #region Polling Condition
        /// <summary>
        /// Implements condition to wait until notification for specified keyID is received or timeout is expired.
        /// </summary>
        public class WaitNotificationForKeyPollingCondition : SubscriptionHandler.PollingConditionBase
        {
            public WaitNotificationForKeyPollingCondition(System.DateTime deadline, string waitingNotificationsFor): base(deadline)
            {
                m_WaitingNotificationsFor = waitingNotificationsFor;
            }

            public string KeyStatus { get; protected set; }

            private bool m_StopPolling = false;
            public override bool StopPulling
            {
                get { return m_StopPolling; }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        log.AppendFormat("No notification for key with ID {0}", m_WaitingNotificationsFor);

                        return log.ToString();
                    }
                    else
                        return string.Format("Notification for key with ID '{0}' is received", m_WaitingNotificationsFor);
                }
            }

            public override void Update(Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (var msg in messages.Keys)
                    {
                        var sourceSI = msg.Message.GetMessageSourceSimpleItems();

                        string keyID = null;
                        if (null != sourceSI && sourceSI.ContainsKey("KeyID"))
                            keyID = sourceSI["KeyID"];

                        var dataSI = msg.Message.GetMessageDataSimpleItems();
                        if (null != keyID && m_WaitingNotificationsFor == keyID
                            && dataSI.ContainsKey("NewStatus") && (dataSI["NewStatus"] == ksOK || dataSI["NewStatus"] == ksCORRUPT))
                        {
                            m_StopPolling = true;
                            KeyStatus = dataSI["NewStatus"];
                        }
                    }
            }

            private readonly string m_WaitingNotificationsFor;
        }

        #endregion
    }
}
