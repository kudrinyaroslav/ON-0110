using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using TestTool.Crypto;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using TestTool.Tests.Common.TestBase;
using AlgorithmIdentifier = TestTool.Proxies.Onvif.AlgorithmIdentifier;
using DateTime = System.DateTime;

namespace TestTool.Tests.TestCases.OnvifServices
{
    public interface IKeyStoreService: IBaseOnvifService2<Keystore, KeystoreClient>
    {}

    public interface ITLSServerService: IBaseOnvifService2<TLSServer, TLSServerClient>
    {}

    public interface IAdvancedSecurityBaseService: IBaseOnvifService2<AdvancedSecurityService, AdvancedSecurityServiceClient>
    {}

    public interface IDot1XService: IBaseOnvifService2<Dot1X, Dot1XClient>
    {}

    public interface IAdvancedSecurityService: IAdvancedSecurityBaseService, IKeyStoreService, ITLSServerService, IDot1XService
    {
        DistinguishedName DefaultSubject { get; }
    }

    public static class AdvancedSecurityExtensions
    {
        #region Utility values
        public const string ksOK = "ok";
        public const string ksCORRUPT = "corrupt";

        public static string DefaultSubjectString(this IAdvancedSecurityService s)
        {
            if (null != s.DefaultSubject)
            {
                var r = new StringBuilder();

                if (null != s.DefaultSubject.CommonName)
                    r.Append(string.Format("CN={0},", s.DefaultSubject.CommonName));
                if (null != s.DefaultSubject.Country)
                    r.Append(string.Format("C={0},", s.DefaultSubject.Country));
                if (null != s.DefaultSubject.Locality)
                    r.Append(string.Format("L={0},", s.DefaultSubject.Locality));
                if (null != s.DefaultSubject.Organization)
                    r.Append(string.Format("O={0},", s.DefaultSubject.Organization));
                if (null != s.DefaultSubject.OrganizationalUnit)
                    r.Append(string.Format("OU={0},", s.DefaultSubject.OrganizationalUnit));
                if (null != s.DefaultSubject.StateOrProvinceName)
                    r.Append(string.Format("ST={0},", s.DefaultSubject.StateOrProvinceName));

                return r.ToString().TrimEnd(',');
            }

            return "";
        }

        public static string DistinguishedNameToString(this IAdvancedSecurityService s, DistinguishedName distinguishedName)
        {
                if (null != distinguishedName)
                {
                    var r = new StringBuilder();

                    if (null != distinguishedName.CommonName)
                        r.Append(string.Format("CN={0},", distinguishedName.CommonName));
                    if (null != distinguishedName.Country)
                        r.Append(string.Format("C={0},", distinguishedName.Country));
                    if (null != distinguishedName.Locality)
                        r.Append(string.Format("L={0},", distinguishedName.Locality));
                    if (null != distinguishedName.Organization)
                        r.Append(string.Format("O={0},", distinguishedName.Organization));
                    if (null != distinguishedName.OrganizationalUnit)
                        r.Append(string.Format("OU={0},", distinguishedName.OrganizationalUnit));
                    if (null != distinguishedName.StateOrProvinceName)
                        r.Append(string.Format("ST={0},", distinguishedName.StateOrProvinceName));
                    if (null != distinguishedName.Title)
                        r.Append(string.Format("T={0},", distinguishedName.Title));
                    if (null != distinguishedName.DistinguishedNameQualifier)
                        r.Append(string.Format("DN={0},", distinguishedName.DistinguishedNameQualifier));
                    if (null != distinguishedName.SerialNumber)
                        r.Append(string.Format("SERIALNUMBER={0},", distinguishedName.SerialNumber));
                    if (null != distinguishedName.Surname)
                        r.Append(string.Format("SURNAME={0},", distinguishedName.Surname));
                    if (null != distinguishedName.GivenName)
                        r.Append(string.Format("GIVENNAME={0},", distinguishedName.GivenName));
                    if (null != distinguishedName.Initials)
                        r.Append(string.Format("INITIALS={0},", distinguishedName.Initials));
                    if (null != distinguishedName.Pseudonym)
                        r.Append(string.Format("Pseudonym={0},", distinguishedName.Pseudonym));
                    if (null != distinguishedName.GenerationQualifier)
                        r.Append(string.Format("GENERATION={0},", distinguishedName.GenerationQualifier));
                    //if (null != distinguishedName.Title)
                    //    r.Append(string.Format("T={0},", distinguishedName.Title));
                    //if (null != distinguishedName.Title)
                    //    r.Append(string.Format("T={0},", distinguishedName.Title));

                    return r.ToString().TrimEnd(',');
                }

                return "";           
        }

        public static AlgorithmIdentifier DefaultSignatureAlgorithmIdentifier = new AlgorithmIdentifier()
                                                                                {
                                                                                    algorithm = "1.2.840.113549.1.1.5",//SHA-1 with RSA Encryption
                                                                                    parameters = null,
                                                                                    anyParameters = null,
                                                                                    AnyAttr = null
                                                                                };
        public static AlgorithmIdentifier OIDSignatureAlgorithmIdentifier = new AlgorithmIdentifier()
                                                                            {
                                                                                algorithm = "1.2.840.113549.1.1.11",//OID of SHA-256 with RSA Encryption
                                                                                parameters = null,
                                                                                anyParameters = null,
                                                                                AnyAttr = null
                                                                            };

        #region Default aliases
        public const string defaultAlias = "ONVIF_Test_Alias";
        public const string defaultDot1XAlias = "ONVIF_Dot1X_Test";
        public const string defaultPassphraseAlias = "ONVIF_Passphrase_Test";
        public const string defaultCertificateAlias = "ONVIF_Certificate_Test";
        public const string defaultCertificationPathAlias = "ONVIF_Certification_Path_Test";
        public const string defaultKeyAlias = "ONVIF_Key_Test";
        public const string defaultCRLAlias = "ONVIF_CRL_Test";
        public const string defaultCertificationPathValidationPolicyAlias = "ONVIF_CertPathValidationPolicy_Test";
        #endregion


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
        #endregion

        #region Initialize utils
        public static void InitializeService(this IAdvancedSecurityService s)
        {
            s.CheckCryptoLibary();
        }
        
        private static void InitializeGuard(this IAdvancedSecurityBaseService s)
        {
            if (!s.ServiceClient.IsInitialized())
                s.Test.Assert(false,
                              "Can't connect to Advanced Security Service",
                              "Check that Advanced Security Service is accessible");
        }

        private static void InitializeGuard(this IKeyStoreService s)
        {
            if (!s.ServiceClient.IsInitialized())
                s.Test.Assert(false,
                              "Can't connect to Key Store Service",
                              "Check that Key Store Service is accessible");
        }

        private static void InitializeGuard(this ITLSServerService s)
        {
            if (!s.ServiceClient.IsInitialized())
                s.Test.Assert(false,
                              "Can't connect to TLS Server Service",
                              "Check that TLS Server Service is accessible");
        }

        private static void InitializeGuard(this IDot1XService s)
        {
            if (!s.ServiceClient.IsInitialized())
                s.Test.Assert(false,
                              "Can't connect to Dot1X Service",
                              "Check that Dot1X Service is accessible");
        }
        #endregion

        #region Service commands

        #region IAdvancedSecurityBaseService

        public static AdvancedSecurityCapabilities GetServiceCapabilities(this IAdvancedSecurityBaseService s)
        {
            s.InitializeGuard();

            AdvancedSecurityCapabilities r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetServiceCapabilities(), "Get Service Capabilities(Advanced Security)");

            return r;
        }

        #endregion 

        #region IKeyStoreService

        public static string CreateRSAKeyPair(this IKeyStoreService s, string keyLength, string alias, out string estimatedCreationTime)
        {
            s.InitializeGuard();

            string keyID = null;
            string localEstimatedCreationTime = null;
            var aliasPart = (null == alias) ? "without alias" : string.Format("with alias = '{0}'", alias);

            try
            {
                s.Test.RunStep(() => keyID = s.ServiceClient.Port.CreateRSAKeyPair(keyLength, alias, out localEstimatedCreationTime),
                               string.Format("Create RSA Key Pair of size '{0}' and {1}", keyLength, aliasPart));

                estimatedCreationTime = localEstimatedCreationTime;
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfKeysReached"))
                    s.Test.LogStepEvent(fpMsgRSAKeyPair);
                else if (e.IsValidOnvifFault("Sender/InvalidArgVal/KeyLength"))
                    s.Test.LogStepEvent(wMsgKeyLength);
                else
                    s.Test.LogStepEvent(wMsgUnspecifiedSOAPFault);

                throw;
            }

            return keyID;
        }

        public static void CreateRSAKeyPair(this IKeyStoreService s, string keyLength, string alias, out TimeSpan estimatedCreationTime, out string keyID)
        {
            s.InitializeGuard();

            string est;
            keyID = s.CreateRSAKeyPair(keyLength, null, out est);

            estimatedCreationTime = new TimeSpan();
            try
            {
                estimatedCreationTime = XmlConvert.ToTimeSpan(est);
            }
            catch (Exception e)
            {
                s.Test.Assert(false,
                              string.Format("Invalid format for Estimated Creation Time: {0}", e.Message),
                              "Parsing Estimated Creation Time");
            }
        }

        public static void DeleteRSAKeyPair(this IKeyStoreService s, string keyID)
        {
            s.InitializeGuard();

            try
            {
                s.Test.RunStep(() => s.ServiceClient.Port.DeleteKey(keyID), string.Format("Delete RSA Key Pair with ID = '{0}'", keyID));
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/KeyDeletionFailed"))
                    s.Test.LogStepEvent(wMsgKeyDeletingFailed);
                else if (e.IsValidOnvifFault("Sender/InvalidArgVal/KeyID"))
                    s.Test.LogStepEvent(wMsgKeyID);
                else if (e.IsValidOnvifFault("Sender/InvalidArgVal/ReferenceExists"))
                    s.Test.LogStepEvent(wMsgReferenceExists);
                else
                    s.Test.LogStepEvent(wMsgUnspecifiedSOAPFault);

                throw;
            }
        }

        public static IEnumerable<KeyAttribute> GetAllKeys(this IKeyStoreService s)
        {
            s.InitializeGuard();

            IEnumerable<KeyAttribute> r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAllKeys(), "Get All Keys");

            if (null != r)
                s.Test.Assert(r.Distinct().Count() == r.Count(),
                              "There are duplicates in received list of key's IDs",
                              "Check that received key's IDs are unique");

            return r ?? new List<KeyAttribute>();
        }

        public static string CreateSelfSignedCertificate(this IKeyStoreService s, string X509Version, DistinguishedName Subject, string KeyID, string Alias, 
                                                         System.DateTime notValidBefore, System.DateTime notValidAfter, 
                                                         AlgorithmIdentifier SignatureAlgorithm, X509v3Extension[] Extension = null)
        {
            s.InitializeGuard();

            string r = null;

            try
            {
                s.Test.RunStep(() => r = s.ServiceClient.Port.CreateSelfSignedCertificate(X509Version, Subject, KeyID, Alias, notValidBefore, notValidAfter, SignatureAlgorithm, Extension),
                               "Create Self Signed Certificate");
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertificatesReached"))
                    s.Test.LogStepEvent(fpMsgCertificate);

                throw;
            }


            return r;
        }

        public static string CreateSelfSignedCertificate(this IKeyStoreService s, string X509Version, DistinguishedName Subject, string KeyID, string Alias,
                                                         System.DateTime notValidBefore, System.DateTime notValidAfter,
                                                         X509v3Extension[] Extension = null)
        {
            return s.CreateSelfSignedCertificate(X509Version, Subject, KeyID, Alias, notValidBefore, notValidAfter, DefaultSignatureAlgorithmIdentifier, Extension);
        }

        public static void DeleteCertificate(this IKeyStoreService s, string certID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteCertificate(certID), string.Format("Delete Certificate with ID = '{0}'", certID));
        }

        public static Proxies.Onvif.X509Certificate GetCertificate(this IKeyStoreService s, string certificateID)
        {
            s.InitializeGuard();

            Proxies.Onvif.X509Certificate r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCertificate(certificateID), string.Format("Get Certificate with ID = '{0}'", certificateID));

            return r;
        }

        public static IEnumerable<Proxies.Onvif.X509Certificate> GetAllCertificates(this IKeyStoreService s)
        {
            s.InitializeGuard();

            IEnumerable<X509Certificate> r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAllCertificates(), "Get All Certificates");

            return r ?? new List<X509Certificate>();
        }

        public static string CreateCertificationPath(this IKeyStoreService s, CertificateIDs CertificateIDs, string Alias)
        {
            s.InitializeGuard();

            string r = null;

            try
            {
                s.Test.RunStep(() => r = s.ServiceClient.Port.CreateCertificationPath(CertificateIDs, Alias), "Create Certification Path");
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertificationPathsReached"))
                    s.Test.LogStepEvent(fpMsgCertificationPath);

                throw;
            }

            return r;
        }
        
        public static void DeleteCertificationPath(this IKeyStoreService s, string certificationPathID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteCertificationPath(certificationPathID), string.Format("Delete Certification Path with ID = '{0}'", certificationPathID));
        }

        public static CertificationPath GetCertificationPath(this IKeyStoreService s, string CertificationPathID)
        {
            s.InitializeGuard();

            CertificationPath r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCertificationPath(CertificationPathID), string.Format("Get Certification Path with ID = '{0}'", CertificationPathID));

            return r;
        }

        public static IEnumerable<string> GetAllCertificationPaths(this IKeyStoreService s)
        {
            s.InitializeGuard();

            IEnumerable<string> r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAllCertificationPaths(), "Get All Certification Paths");

            if (null != r)
                s.Test.Assert(r.Distinct().Count() == r.Count(),
                              "There are duplicates in received list of certification path's IDs",
                              "Check that received certification path's IDs are unique");


            return r ?? new List<string>();
        }

        public static string UploadCRL(this IKeyStoreService s, byte[] Crl, string Alias, UploadCRLAnyParameters anyParameters = null)
        {
            s.InitializeGuard();

            var r = string.Empty;

            s.Test.RunStep(() => r = s.ServiceClient.Port.UploadCRL(Crl, Alias, anyParameters),
                           "Upload CRL");

            return r;
        }

        public static void DeleteCRL(this IKeyStoreService s, string CrlID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteCRL(CrlID),
                           "Delete CRL");
        }

        public static CRL GetCRL(this IKeyStoreService s, string CrlID)
        {
            s.InitializeGuard();

            CRL r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCRL(CrlID),
                           "Get CRL");

            return r;
        }

        public static CRL[] GetAllCRLs(this IKeyStoreService s)
        {
            s.InitializeGuard();

            CRL[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAllCRLs(),
                           "Get All CRLs");

            return r ?? new CRL[0];
        }

        public static string CreateCertPathValidationPolicy(this IKeyStoreService s,
                                                            string Alias,
                                                            CertPathValidationParameters Parameters,
                                                            TrustAnchor[] TrustAnchor,
                                                            CreateCertPathValidationPolicyAnyParameters anyParameter)
        {
            s.InitializeGuard();

            var r = string.Empty;

            s.Test.RunStep(() => r = s.ServiceClient.Port.CreateCertPathValidationPolicy(Alias, Parameters, TrustAnchor, anyParameter),
                           "Create Certification Path Validation Policy");

            return r;            
        }

        public static void DeleteCertPathValidationPolicy(this IKeyStoreService s,
                                                          string CertPathValidationPolicyID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteCertPathValidationPolicy(CertPathValidationPolicyID),
                           "Delete Certification Path Validation Policy");
        }

        public static CertPathValidationPolicy GetCertPathValidationPolicy(this IKeyStoreService s,
                                                                           string CertPathValidationPolicyID)
        {
            s.InitializeGuard();

            CertPathValidationPolicy r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCertPathValidationPolicy(CertPathValidationPolicyID),
                           "Get Certification Path Validation Policy");

            return r;            
        }

        public static CertPathValidationPolicy[] GetAllCertPathValidationPolicies(this IKeyStoreService s)
        {
            s.InitializeGuard();

            CertPathValidationPolicy[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAllCertPathValidationPolicies(),
                           "Get All Certification Path Validation Policies");

            return r ?? new CertPathValidationPolicy[0];            
        }

        public static string GetKeyStatus(this IKeyStoreService s, string keyID)
        {
            s.InitializeGuard();

            string r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetKeyStatus(keyID),
                           string.Format("Get Key Status with ID = '{0}'", keyID));

            return r;
        }

        public static bool GetPrivateKeyStatus(this IKeyStoreService s, string keyID)
        {
            s.InitializeGuard();

            bool r = false;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetPrivateKeyStatus(keyID),
                           string.Format("Get Private Key Status with ID = '{0}'", keyID));

            return r;
        }

        public static byte[] CreatePKCS10CSR(this IKeyStoreService s, DistinguishedName Subject, string KeyID, CSRAttribute[] CSRAttribute, AlgorithmIdentifier SignatureAlgorithm)
        {
            s.InitializeGuard();

            byte[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.CreatePKCS10CSR(Subject, KeyID, CSRAttribute, SignatureAlgorithm),
                           "CreatePKCS10CSR");

            return r;

        }

        public static byte[] CreatePKCS10CSR(this IKeyStoreService s, DistinguishedName Subject, string KeyID, CSRAttribute[] CSRAttribute = null)
        {
            return s.CreatePKCS10CSR(Subject, KeyID, CSRAttribute, DefaultSignatureAlgorithmIdentifier);
        }

        public static void UploadCertificate(this IKeyStoreService s, byte[] Certificate, string Alias, string KeyAlias, bool PrivateKeyRequired,
                                             out string KeyID, out string CertID, 
                                             string expectedKeyID = null, bool preconditionCertNumber = true, bool preconditionKeysNumber = true)
        {
            s.InitializeGuard();

            try
            {
                string localKeyID = null;
                string r = "";
                s.Test.RunStep(() => r = s.ServiceClient.Port.UploadCertificate(Certificate, Alias, KeyAlias, PrivateKeyRequired, out localKeyID),
                               "Upload Certificate");
                CertID = r;
                KeyID = localKeyID;
            }
            catch (FaultException e)
            {
                if (preconditionCertNumber && e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertificatesReached"))
                    s.Test.LogStepEvent(fpMsgCertificate);

                if (preconditionKeysNumber && e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfKeysReached"))
                    s.Test.LogStepEvent(fpMsgRSAKeyPair);

                throw;
            }


            if (null != expectedKeyID)
                s.Test.Assert(expectedKeyID == KeyID,
                              string.Format("Certificate is uploaded for RSA key pair with ID = '{0}'. But actually it is linked to RSA key pair with ID = '{1}'", expectedKeyID, KeyID), 
                              "Checking returned KeyID");
        }

        public static void UploadCertificate(this IKeyStoreService s, byte[] Certificate, string Alias, string KeyAlias, bool PrivateKeyRequired,
                                             out string CertID, 
                                             string expectedKeyID = null, bool preconditionCertNumber = true, bool preconditionKeysNumber = true)
        {
            string keyID;
            s.UploadCertificate(Certificate, Alias, KeyAlias, PrivateKeyRequired, out keyID, out CertID, expectedKeyID, preconditionCertNumber, preconditionKeysNumber);
        }

        public static string UploadPassphrase(this IKeyStoreService s, string Passphrase, string PassphraseAlias = null)
        {
            s.InitializeGuard();

            string r = "";
            s.Test.RunStep(() => r = s.ServiceClient.Port.UploadPassphrase(Passphrase, PassphraseAlias),
                           "Upload Passphrase");

            return r;
        }

        public static void DeletePassphrase(this IKeyStoreService s, string PassphraseID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeletePassphrase(PassphraseID),
                           "Delete Passphrase");
        }

        public static string UploadKeyPairInPKCS8(this IKeyStoreService s, byte[] KeyPair, string Alias, string EncryptionPassphraseID)
        {
            s.InitializeGuard();

            string r = "";
            s.Test.RunStep(() => r = s.ServiceClient.Port.UploadKeyPairInPKCS8(KeyPair, Alias, EncryptionPassphraseID),
                           "Upload Key Pair In PKCS8");

            return r;
        }

        public static string UploadCertificateWithPrivateKeyInPKCS12(this IKeyStoreService s, byte[] CertWithPrivateKey, string CertificationPathAlias, string KeyAlias, bool IgnoreAdditionalCertificates, string IntegrityPassphraseID, string EncryptionPassphraseID, out string KeyID)
        {
            s.InitializeGuard();

            string r = string.Empty, localKeyID = string.Empty;
            s.Test.RunStep(() => r = s.ServiceClient.Port.UploadCertificateWithPrivateKeyInPKCS12(CertWithPrivateKey, CertificationPathAlias, KeyAlias, IgnoreAdditionalCertificates, IntegrityPassphraseID, EncryptionPassphraseID, out localKeyID),
                           "Upload Certificate With Private Key In PKCS12");

            KeyID = localKeyID;

            return r;
        }

        public static IEnumerable<PassphraseAttribute> GetAllPassphrases(this IKeyStoreService s)
        {
            s.InitializeGuard();

            IEnumerable<PassphraseAttribute> r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAllPassphrases(),
                           "Get All Passphrases");

            if (null != r)
                s.Test.Assert(r.Distinct().Count() == r.Count(),
                              "There are duplicates in received list of passphrase's IDs",
                              "Check that received passphrase's IDs are unique");

            return r ?? new List<PassphraseAttribute>();
        }
        #endregion

        #region ITLSServerService

        public static void AddServerCertificateAssignment(this ITLSServerService s, string CertificationPathID)
        {
            s.InitializeGuard();

            try
            {
                s.Test.RunStep(() => s.ServiceClient.Port.AddServerCertificateAssignment(CertificationPathID), 
                               string.Format("Add Server Certificate Assignment(CertificationPathID = '{0}')", CertificationPathID));
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaximumNumberOfCertPathsReached"))
                    s.Test.LogStepEvent(fpMsgCertificateAssignment);

                throw;
            }
        }

        public static void RemoveServerCertificateAssignment(this ITLSServerService s, string CertificationPathID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.RemoveServerCertificateAssignment(CertificationPathID),
                           string.Format("Remove Server Certificate Assignment(CertificationPathID = '{0}')", CertificationPathID));
        }

        public static void ReplaceServerCertificateAssignment(this ITLSServerService s, string oldCertificationPathID, string newCertificationPathID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.ReplaceServerCertificateAssignment(oldCertificationPathID, newCertificationPathID),
                           string.Format("Replace Server Certificate Assignment from '{0}' to '{1}'", oldCertificationPathID, newCertificationPathID));
        }

        public static List<string> GetAssignedServerCertificates(this ITLSServerService s)
        {
            s.InitializeGuard();

            string[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAssignedServerCertificates(), "Get Assigned Server Certificates");

            if (null != r)
                s.Test.Assert(r.Distinct().Count() == r.Count(),
                              "There are duplicates in received list of assigned server certificate's IDs",
                              "Check that received assigned server certificate's IDs are unique");

            return null != r ? r.ToList() : new List<string>();
        }

        public static bool GetClientAuthenticationRequired(this ITLSServerService s)
        {
            s.InitializeGuard();

            var r = false;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetClientAuthenticationRequired(),
                           "Get Client Authentication Required");

            return r;
        }

        public static void SetClientAuthenticationRequired(this ITLSServerService s, bool clientAuthenticationRequired)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetClientAuthenticationRequired(clientAuthenticationRequired),
                           "Set Client Authentication Required");
        }

        public static string[] GetAssignedCertPathValidationPolicies(this ITLSServerService s)
        {
            s.InitializeGuard();

            string[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAssignedCertPathValidationPolicies(),
                           "Get Assigned Certification Path Validation Policies");

            return r ?? new string[0];
        }

        public static void AddCertPathValidationPolicyAssignment(this ITLSServerService s, string CertPathValidationPolicyID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.AddCertPathValidationPolicyAssignment(CertPathValidationPolicyID),
                           "Add Certification Path Validation Policies");
        }

        public static void ReplaceCertPathValidationPolicyAssignment(this ITLSServerService s, string oldCertPathValidationPolicyID, string newCertPathValidationPolicyID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.ReplaceCertPathValidationPolicyAssignment(oldCertPathValidationPolicyID, newCertPathValidationPolicyID),
                           "Replace Assigned Certification Path Validation Policies");
        }

        public static void RemoveCertPathValidationPolicyAssignment(this ITLSServerService s, string CertPathValidationPolicyID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.RemoveCertPathValidationPolicyAssignment(CertPathValidationPolicyID),
                           "Remove Assigned Certification Path Validation Policies");
        }
        #endregion

        #region IDot1XService

        public static string AddDot1XConfiguration(this IDot1XService s, Dot1XConfiguration1 Dot1XConfiguration, string Alias)
        {
            s.InitializeGuard();

            string r = string.Empty;

            s.Test.RunStep(() => r = s.ServiceClient.Port.AddDot1XConfiguration(Dot1XConfiguration, Alias),
                           "Add Dot1X Configuration");

            return r;
        }

        public static Dot1XSummary[] GetAllDot1XConfigurations(this IDot1XService s)
        {
            s.InitializeGuard();

            Dot1XSummary[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAllDot1XConfigurations(),
                           "Get All Dot1X Configurations");

            return r ?? new Dot1XSummary[0];
        }

        public static Dot1XConfiguration1 GetDot1XConfiguration(this IDot1XService s, string Dot1XID)
        {
            s.InitializeGuard();

            Dot1XConfiguration1 r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetDot1XConfiguration(Dot1XID),
                           "Get Dot1X Configuration");

            return r;
        }

        public static void DeleteDot1XConfiguration(this IDot1XService s, string Dot1XID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteDot1XConfiguration(Dot1XID),
                           "Delete Dot1X Configuration");
        }

        public static void SetNetworkInterfaceDot1XConfiguration(this IDot1XService s, string token, string Dot1XID)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetNetworkInterfaceDot1XConfiguration(token, Dot1XID),
                           "Set Network Interface Dot1X Configuration");
        }

        public static string GetNetworkInterfaceDot1XConfiguration(this IDot1XService s, string token)
        {
            s.InitializeGuard();

            string r = string.Empty;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetNetworkInterfaceDot1XConfiguration(token),
                           "Get Network Interface Dot1X Configuration");

            return r;
        }

        public static void DeleteNetworkInterfaceDot1XConfiguration(this IDot1XService s, string token)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteNetworkInterfaceDot1XConfiguration(token),
                           "Delete Network Interface Dot1X Configuration");
        }

        #endregion

        #endregion

        #region Annexes
        public static IEnumerable<uint> SelectKeyLengthsForTest(this IAdvancedSecurityService s, IEnumerable<uint> keyLengths, uint limit = 4096)
        {
            var r = keyLengths.Where(e => e <= limit);

            return r.Any() ? r : new [] { keyLengths.Min() };
        }

        public static  X509CertificateBase HelperCreateCertificateFromPKCS10CSRA3(this IAdvancedSecurityService s, byte[] pkcs10CSR, RSAKeyPair caKeyPair, X509CertificateBase caCertificate)
        {
            var request = s.ValidateCertificateSigningRequest(pkcs10CSR);

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

        /// <summary>
        /// Annex A.8: helper procedure to create a self-signed certificate.
        /// </summary>
        /// <param name="testRSAKeyID">ID of created RSA keypair</param>
        /// <param name="notBefore">start of certificate's validity period</param>
        /// <param name="notAfter">end of certificate's validity period</param>
        /// <returns>ID of created certificate</returns>
        public static string CreateTestSelfSignedCertificateA8(this IAdvancedSecurityService s, out string testRSAKeyID, DateTime notBefore = new DateTime(), DateTime notAfter = new DateTime())
        {
            s.CreateTestRSAKeyPairA7(out testRSAKeyID);

            return s.CreateSelfSignedCertificate(null, s.DefaultSubject, testRSAKeyID, null, notBefore, notAfter);
        }

        public static X509CertificateBase GenerateSignedCertificate(byte[] issuerPrivateKey,
                                                                    byte[] ownerPublicKey,
                                                                    DateTime? notBefore = null, DateTime? notAfter = null,
                                                                    string issuerDN = "CN=ONVIF TT,C=US",
                                                                    string subjectDN = "CN=ONVIF TT,C=US",
                                                                    byte[] extensions = null)
        {
            if (null == notBefore)
                notBefore = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            if (null == notAfter)
                notAfter = new DateTime(9999, 12, 31, 23, 59, 59, 0);

            var generator = new X509CertificateGeneratorBC();
            generator.SetSerialNumber(DateTime.Now.Ticks.ToString());
            generator.SetIssuerDN(issuerDN);
            generator.SetPublicKey(ownerPublicKey);
            generator.SetNotValidBefore(notBefore.Value);
            generator.SetNotValidAfter(notAfter.Value);
            generator.SetSubjectDN(subjectDN);

            generator.SetSignatureAlgorithm("SHA1WithRSAEncryption");

            generator.CopyExtensions(extensions);

            return generator.Generate(issuerPrivateKey);
        }
        /// <summary>
        /// Annex A.4: helper procedure to create an X.509 CA certificate.
        /// </summary>
        /// <param name="key">keypair used for certificate's creation</param>
        /// <returns>X509 certificate</returns>
        public static X509CertificateBase CreateTestSelfSignedCACertificateA4(this IAdvancedSecurityService s, out RSAKeyPair key, DateTime? notBefore = null, DateTime? notAfter = null, string subjectDN = "CN=ONVIF TT,C=US")
        {
            var keyPairGenerator = new RSAKeyPairGeneratorBC(1024);

            key = keyPairGenerator.Generate();

            return GenerateSignedCertificate(key.PrivateKey, key.PublicKey, notBefore, notAfter, subjectDN, subjectDN);
        }

        public static X509CertificateBase CreateTestSelfSignedCACertificateA4(this IAdvancedSecurityService s, DateTime? notBefore = null, DateTime? notAfter = null)
        {
            RSAKeyPair q = null;
            return s.CreateTestSelfSignedCACertificateA4(out q, notBefore, notAfter);
        }

        public static X509CertificateBase CreateTestSelfSignedCACertificateA4(this IAdvancedSecurityService s, out RSAKeyPair key, string subjectDN = "CN=ONVIF TT,C=US")
        {
            return s.CreateTestSelfSignedCACertificateA4(out key, null, null, subjectDN);
        }

        public static X509CertificateBase CreateTestSelfSignedCACertificateA4(this IAdvancedSecurityService s, out RSAKeyPair key)
        {
            return s.CreateTestSelfSignedCACertificateA4(out key, "CN=ONVIF TT,C=US");
        }

        /// <summary>
        /// Annex A.14: helper procedure to create a CA-signed certificate for RSA key pair.
        /// </summary>
        /// <param name="caCertificate">CA certificate by which generated certificate should be signed</param>
        /// <param name="caKeyPair">RSA keypair corresponding to CA certificate</param>
        /// <param name="testRSAKeyID">ID of RSA keypair for that certificate is created</param>
        /// <returns></returns>
        public static X509CertificateBase CreateTestCertificateSignedByCACertificateA14(this IAdvancedSecurityService s, X509CertificateBase caCertificate, RSAKeyPair caKeyPair, string testRSAKeyID)
        {
            var r = s.CreatePKCS10CSR(s.DefaultSubject, testRSAKeyID);

            var request = s.ValidateCertificateSigningRequest(r);

            return GenerateSignedCertificate(caKeyPair.PrivateKey,
                                             request.PublicKey,
                                             null,
                                             null,
                                             caCertificate.SubjectDN,
                                             request.SubjectDN,
                                             request.Extensions);
        }

        public static CertificateSigningRequestBC ValidateCertificateSigningRequest(this IAdvancedSecurityService s, byte[] r, string expectedSubject = null)
        {
            CertificateSigningRequestBC request = null;

            var msg = "";
            var reqFlag = true;
            try
            {
                request = new CertificateSigningRequestBC(new MemoryStream(r));
                
                if (null != expectedSubject)
                    reqFlag = s.X509NamesAreEqual(request.SubjectDN, expectedSubject);
                if (!reqFlag)
                    msg = string.Format("Received PKCS10 Certificate Signing Request has invalid Subject: {0}. Expected: {1}", request.SubjectDN, s.DefaultSubjectString());
            }
            catch (Exception e)
            {
                msg = string.Format("Received PKCS10 Certificate Signing Request is invalid: {0}", !r.Any() ? "empty response" : e.Message);
                reqFlag = false;
            }

            s.Test.Assert(reqFlag,
                          msg,
                          "Validating received PKCS10 Certificate Signing Request");

            s.Test.Assert(request.Verify(),
                          "Received PKCS10 Certificate Signing Request has invalid signature",
                          "Verifying signature on PKCS10 Certificate Signing Request");
            return request;
        }

        public static bool ValidateDEREncoding(this IAdvancedSecurityService s, Stream stream)
        {
            try
            {
                var asn1Stream = new Asn1InputStream(stream);
                Asn1Object certificate = asn1Stream.ReadObject();

                var derEncodedStream = new MemoryStream();
                var encoder = new DerOutputStream(derEncodedStream);
                encoder.WriteObject(certificate);
                encoder.Flush();

                if (stream.Length != derEncodedStream.Length)
                    return false;

                stream.Seek(0, SeekOrigin.Begin);
                derEncodedStream.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < stream.Length; i++)
                {
                    if (stream.ReadByte() != derEncodedStream.ReadByte())
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool X509NamesAreEqual(this IAdvancedSecurityService s, string l, string r)
        {
            return new X509Name(l).Equivalent(new X509Name(r), false);
        }

        public static X509CertificateBase CreateCASignedCertificateA14(this IAdvancedSecurityService s, X509CertificateBase caCertificate, RSAKeyPair caKeyPair, out string rsaKeyPairID)
        {
            s.CreateTestRSAKeyPairA7(out rsaKeyPairID);

            return s.CreateTestCertificateSignedByCACertificateA14(caCertificate, caKeyPair, rsaKeyPairID);
        }

        /// <summary>
        /// Annex A.15: helper procedure to upload a certificate without public static key assignment.
        /// </summary>
        /// <param name="certificate">Certificate to be loaded</param>
        /// <param name="keyID">ID of RSA keypair for that certificate is created</param>
        /// <returns></returns>
        public static void UploadCertificateWithoutPrivateKeyA15(this IAdvancedSecurityService s, X509CertificateBase certificate, out string keyID, out string certID)
        {
            s.UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", null, false, out keyID, out certID);
        }

        /// <summary>
        /// Annex A.11: helper procedure to create a certification path based on self-signed certificate.
        /// </summary>
        /// <param name="testRSAKeyID"></param>
        /// <param name="testCertID"></param>
        /// <param name="alias"></param>
        /// <returns>ID of created test certification path</returns>
        public static string CreateTestSelfSignedCertificationPathA11(this IAdvancedSecurityService s, out string testRSAKeyID, out string testCertID, string alias = "ONVIF_TEST")
        {
            testCertID = s.CreateTestSelfSignedCertificateA8(out testRSAKeyID);

            return s.CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { testCertID } }, alias);
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
        public static string CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(this IAdvancedSecurityService s, out string CARSAKeyID, out string CACertID,
                                                                                        out string RSAKeyID, out string certID,
                                                                                        out X509CertificateBase certificate,
                                                                                        string alias = "ONVIF_Test2")
        {
            RSAKeyPair keyPair = null;
            var caCertificate = s.CreateTestSelfSignedCACertificateA4(out keyPair);
            s.CreateAndUploadCASignedCertificateA16(caCertificate, keyPair, out RSAKeyID, out certID, out certificate);

            string localCARSAKeyID;
            var localCACertID = "";
            s.UploadCertificateWithoutPrivateKeyA15(caCertificate, out localCARSAKeyID, out localCACertID);

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

            s.Test.Assert(flag, msg.ToString(), "Checking returned IDs");

            return s.CreateCertificationPath(new CertificateIDs() { CertificateID = new string[] { certID, CACertID } }, alias);
        }

        public static string CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(this IAdvancedSecurityService s, out string CARSAKeyID,
                                                                                        out string CACertID,
                                                                                        out string RSAKeyID, out string certID,
                                                                                        string alias = "ONVIF_Test2")
        {
            X509CertificateBase certificate = null;
            return s.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out CARSAKeyID, out CACertID, out RSAKeyID, out certID, out certificate);
        }


        public static void CreateAndUploadCASignedCertificateA16(this IAdvancedSecurityService s, 
                                                                 X509CertificateBase caCertificate, RSAKeyPair caKeyPair, 
                                                                 out string certKeyID, out string certID, out X509CertificateBase certificate)
        {
            certificate = s.CreateCASignedCertificateA14(caCertificate, caKeyPair, out certKeyID);

            s.UploadCertificate(certificate.GetEncoded(), "ONVIF_Test1", null, true, out certID, certKeyID);
        }

        public static void CreateAndUploadCASignedCertificateA16(this IAdvancedSecurityService s,
                                                                 X509CertificateBase caCertificate, RSAKeyPair caKeyPair,
                                                                 out string certKeyID, out string certID)
        {
            X509CertificateBase certificate = null;
            s.CreateAndUploadCASignedCertificateA16(caCertificate, caKeyPair, out certKeyID, out certID, out certificate);
        }

        /// <summary>
        /// Checks if received certification path is as expected.
        /// </summary>
        /// <param name="certificationPath">received certification path</param>
        /// <param name="expectedCertificateSequence">certificate's sequence supposed to be in certificationPath</param>
        /// <param name="log">to store log messages</param>
        /// <returns>true if received certification path is as expected</returns>
        public static bool ValidateReceivedCertificationPath(this IAdvancedSecurityService s, 
                                                             CertificationPath certificationPath,
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
        public static uint GetKeyLengthForTestRSAKeyPairA6(this IAdvancedSecurityService s)
        {
            var r = s.GetServiceCapabilities();

            s.Test.Assert(null != r.KeystoreCapabilities.RSAKeyLengths && r.KeystoreCapabilities.RSAKeyLengths.Any(),
                          "Determination of key length for test key pair is failed: the DUT doesn't support any key length",
                          "Determine key length for test key pair");

            return r.KeystoreCapabilities.RSAKeyLengths.Min();
        }

        /// <summary>
        /// Annex A.7: helper procedure to create a RSA key pair.
        /// </summary>
        /// <param name="keyID"></param>
        public static void CreateTestRSAKeyPairA7(this IAdvancedSecurityService s, out string keyID)
        {
            var keyLength = s.GetKeyLengthForTestRSAKeyPairA6();

            s.CreateTestRSAKeyPairOfSpecifiedLength(out keyID, keyLength);
        }

        public static string ConvertTimeSpanToHumanReadableFormat(TimeSpan T)
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
        public static void CreateTestRSAKeyPairOfSpecifiedLength(this IAdvancedSecurityService s, out string keyID, uint keyLength)
        {
            var duration = new TimeSpan();
            s.CreateRSAKeyPair(keyLength.ToString(), null, out duration, out keyID);

            var timeout = s.Test.OperationDelay / 1000;
            var deadline = DateTime.Now.AddSeconds(timeout);

            s.Test.LogStepEvent(string.Format("The DUT has reported key pair creation takes about {0}.", ConvertTimeSpanToHumanReadableFormat(duration)));

            bool first = true;
            var keyStatus = "";
            while (DateTime.Now <= deadline && ksOK != keyStatus && ksCORRUPT != keyStatus)
            {
                if (first)
                {
                    s.Test.LogStepEvent("The status of key pair will be checked after specified amount of time.");
                    first = false;
                }
                else
                    s.Test.LogStepEvent(string.Format("The status of key pair will be checked once again after {0}.", ConvertTimeSpanToHumanReadableFormat(duration)));
                s.Test.LogStepEvent("");

                if (s.Test.Semaphore.StopEvent.WaitOne(duration))
                    s.Test.Semaphore.ReportStop();

                keyStatus = s.GetKeyStatus(keyID);
            }

            var msg = DateTime.Now > deadline
                          ? string.Format("Timeout for key pair creation is expired{0}Increase Operation Delay on Management tab to increase timeout.", Environment.NewLine)
                          : string.Format("Key status is other than '{0}'", ksOK);
            s.Test.Assert(ksOK == keyStatus,
                          msg,
                          string.Format("Check key status of key pair is '{0}'", ksOK));
        }

        /// <summary>
        /// Utility to insert new RSA keypair and check result of this operation.
        /// </summary>
        /// <param name="keyListBefore"></param>
        /// <param name="rsaKeyID"></param>
        /// <returns>List of presented RSA keypairs after creation of new one</returns>
        public static IEnumerable<KeyAttribute> UpdateRSAKeyList(this IAdvancedSecurityService s, IEnumerable<string> keyListBefore, out string rsaKeyID)
        {
            s.CreateTestRSAKeyPairA7(out rsaKeyID);
            s.Test.Assert(!keyListBefore.Contains(rsaKeyID),
                          "Initial key's list already contains ID of just created key",
                          "Check just created key's ID");

            var updatedKeysList = s.GetAllKeys();
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

            s.Test.Assert(flag,
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
        public static IEnumerable<Proxies.Onvif.X509Certificate> ValidationAfterCertificateListUpdate(this IAdvancedSecurityService s, IEnumerable<string> certListBefore, string certID)
        {
            s.Test.Assert(!certListBefore.Contains(certID),
                          "Initial certificate's list already contains ID of just created certificate",
                          "Check just created certificate's ID");

            var updatedCertsList = s.GetAllCertificates();
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

            s.Test.Assert(flag,
                          msg,
                          "Check certificate's list received after test certificate's creation");

            return updatedCertsList;
        }

        /// <summary>
        ///  Utility to create new self-signed certificate and check if this operation is performed successfully.
        /// </summary>
        public static IEnumerable<Proxies.Onvif.X509Certificate> UpdateCertificateListBySelfSignedCertificate(this IAdvancedSecurityService s, IEnumerable<string> certListBefore, out string rsaKeyID, out string certID)
        {
            certID = s.CreateTestSelfSignedCertificateA8(out rsaKeyID);

            return s.ValidationAfterCertificateListUpdate(certListBefore, certID);
        }

        /// <summary>
        ///   Utility to create new certificate signed by CA certificate and check if this operation is performed successfully.
        /// </summary>
        public static IEnumerable<Proxies.Onvif.X509Certificate> UpdateCertificateListByCASignedCertificate(this IAdvancedSecurityService s, IEnumerable<string> certListBefore, out string rsaKeyID, out string certID)
        {
            var caCertificate = s.CreateTestSelfSignedCACertificateA4();
            s.UploadCertificateWithoutPrivateKeyA15(caCertificate, out rsaKeyID, out certID);

            return s.ValidationAfterCertificateListUpdate(certListBefore, certID);
        }

        /// <summary>
        /// Utility to check that creation of new certification path is performed successfully.
        /// </summary>
        /// <param name="certPathListBefore"></param>
        /// <param name="certPathID"></param>
        /// <returns>List of presented certification paths</returns>
        public static IEnumerable<string> ValidationAfterCertificationPathListUpdate(this IAdvancedSecurityService s, IEnumerable<string> certPathListBefore, string certPathID)
        {
            s.Test.Assert(!certPathListBefore.Contains(certPathID),
                          "Initial certification path's list already contains ID of just created path",
                          "Check just created certification path's ID");

            var updatedCertPathsList = s.GetAllCertificationPaths();

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

            s.Test.Assert(flag,
                          msg,
                          "Check certification path's list received after test certification path's creation");

            return updatedCertPathsList;
        }

        /// <summary>
        ///   Utility to create new certification path and check if this operation is performed successfully.
        /// </summary>
        public static IEnumerable<string> UpdateCertificationPathList(this IAdvancedSecurityService s, IEnumerable<string> certPathListBefore, out string rsaKeyID, out string certID, out string certPathID)
        {
            certPathID = s.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);
            
            return s.ValidationAfterCertificationPathListUpdate(certPathListBefore, certPathID);
        }

        /// <summary>
        ///   Utility to create new certification path and check if this operation is performed successfully.
        /// </summary>
        public static IEnumerable<string> UpdateCertificationPathListByPathBasedOnCAAndCASignedCertificates(this IAdvancedSecurityService s, IEnumerable<string> certPathListBefore,
                                                                                                            out string CARSAKeyID, out string CACertID,
                                                                                                            out string RSAKeyID, out string certID,
                                                                                                            out string certPathID)
        {
            certPathID = s.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out CARSAKeyID, out CACertID, out RSAKeyID, out certID);
            
            return s.ValidationAfterCertificationPathListUpdate(certPathListBefore, certPathID);
        }

        /// <summary>
        /// Utility to check that AddServerCertificateAssignment command is performed successfully.
        /// </summary>
        /// <param name="certPathListBefore"></param>
        /// <param name="certPathID"></param>
        /// <returns>List of presented assigned server certificates</returns>
        public static IEnumerable<string> ValidationAfterAssignedServerCertificateListUpdate(this IAdvancedSecurityService s, IEnumerable<string> certPathListBefore, string certPathID)
        {
            var updatedCertsList = s.GetAssignedServerCertificates();

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

            s.Test.Assert(flag,
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
        public static void AddSelfSignedCertificateAsAssignedServerCertificateA13(this IAdvancedSecurityService s, 
                                                                                  out string rsaKeyID, out string certID, out string certPathID, out bool certificateAssignmentAdded)
        {
            certPathID = s.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

            s.AddServerCertificateAssignment(certPathID);
            certificateAssignmentAdded = true;
        }

        /// <summary>
        ///   Utility to add new assigned server certificate and check if this operation is performed successfully.
        /// </summary>
        public static IEnumerable<string> UpdateAssignedServerCertificateListByPathBaseOnSelfSignedCertificate(this IAdvancedSecurityService s, 
                                                                                                               IEnumerable<string> certListBefore, 
                                                                                                               out string rsaKeyID, out string certID,
                                                                                                               out string certPathID, out bool certificateAssignmentAdded)
        {
            certPathID = s.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);
            s.Test.Assert(!certListBefore.Contains(certPathID),
                          "Initial certification path's list already contains ID of just created path",
                          "Check just created certification path's ID");

            s.AddServerCertificateAssignment(certPathID);
            certificateAssignmentAdded = true;

            return s.ValidationAfterAssignedServerCertificateListUpdate(certListBefore, certPathID);
        }

        /// <summary>
        ///   Utility to add new assigned server certificate and check if this operation is performed successfully.
        /// </summary>
        public static IEnumerable<string> UpdateAssignedCertificateListByPathBasedOnCAAndCASignedCertificatesA18(this IAdvancedSecurityService s, 
                                                                                                                 IEnumerable<string> certPathListBefore,
                                                                                                                 out string caKeyID, out string caCertID,
                                                                                                                 out string rsaKeyID, out string certID,
                                                                                                                 out string certPathID, out bool certificateAssignmentAdded)
        {
            certPathID = s.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out caKeyID, out caCertID, out rsaKeyID, out certID);
            s.Test.Assert(!certPathListBefore.Contains(certPathID),
                          "Initial certification path's list already contains ID of just created path",
                          "Check just created certification path's ID");

            s.AddServerCertificateAssignment(certPathID);
            certificateAssignmentAdded = true;

            return s.ValidationAfterAssignedServerCertificateListUpdate(certPathListBefore, certPathID);
        }

        //public static IEnumerable<NetworkProtocol> checkHTTPSAccess(this IAdvancedSecurityService s)
        //{
        //    var protocols = GetNetworkProtocols();
        //    var https = protocols.FirstOrDefault(e => NetworkProtocolType.HTTPS == e.Name); 

        //    Assert(null != https,
        //           "Acess over HTTPS is not supported",
        //           "Check access over HTTPS is supported");

        //    return protocols;
        //}

        public static X509CertificateBase CreateExpiredCertificateA22(this IAdvancedSecurityService s)
        {
            var deviceClient = (s as IDeviceService);

            if (null != deviceClient)
            {
                var date = deviceClient.GetSystemDateAndTime();

                var notAfter = new DateTime(date.UTCDateTime.Date.Year,
                                            date.UTCDateTime.Date.Month,
                                            date.UTCDateTime.Date.Day);

                return s.CreateTestSelfSignedCACertificateA4(notAfter: notAfter.Subtract(new TimeSpan(1, 0, 0, 0)));
            }

            s.Test.Assert(false, "The Device Service is not implemented in this TestSuit", "Implementation issue!");
            return null;
        }

        public static string GenerateTestPassphraseA24(this IAdvancedSecurityService s, uint v = 0)
        {
            switch (v)
            {
                case 0:
                    return "Passphrase for ONVIF";
                case 1:
                    return "AdditionalPassphrase";
                default:
                    return "";
            }
        }

        public static OneAsymmetricKeyInfo GenerateRSAKeyPairInPKCS8WithoutPassphraseA25(this IAdvancedSecurityService s)
        {
            return s.GenerateRSAKeyPairWithoutPassphraseA27(s.GenerateRSAKeyPairA26());
        }

        public static byte[] GenerateRSAKeyPairInPKCS8WithoutPassphraseBinaryA25(this IAdvancedSecurityService s)
        {
            return s.GenerateRSAKeyPairInPKCS8WithoutPassphraseA25().GetDerEncoded();
        }

        public static RSAKeyPair GenerateRSAKeyPairA26(this IAdvancedSecurityService s)
        {
            RSAKeyPair r = null;
            var keyLength = s.GetKeyLengthForTestRSAKeyPairA6();
            s.Test.RunStep(() =>
                           {
                               try
                               {
                                   r = new RSAKeyPairGeneratorBC((int)keyLength).Generate();
                               }
                               catch (Exception e)
                               {
                                   var q = new AssertException(string.Format("Crypto engine has reported an error during RSA keypair generation: {0}", e.Message), e);
                                   s.Test.StepFailed(q);
                                   throw q;
                               }
                           }, 
                           "Generating RSA keypair");

            return r;
        }

        public static OneAsymmetricKeyInfo GenerateRSAKeyPairWithoutPassphraseA27(this IAdvancedSecurityService s, RSAKeyPair keyPair)
        {
            return OneAsymmetricKeyInfoFactory.CreateOneAsymmetricKeyInfo(keyPair);
        }

        public static RSAKeyPair GenerateTestKeyPairWithPassphraseA28(this IAdvancedSecurityService s, string passphrase)
        {
            return s.EncryptKeyPairWithPassphraseA29(passphrase, s.GenerateRSAKeyPairA26());
        }

        public static RSAKeyPair EncryptKeyPairWithPassphraseA29(this IAdvancedSecurityService s, string passphrase, RSAKeyPair keyPair)
        {
            return RSAKeyPair.Encrypt(passphrase, keyPair);
        }

        public static byte[] PackRSAKeyPairInPKCS12A32(this IAdvancedSecurityService s, RSAKeyPair keyPair, X509CertificateBase certificate)
        { return s.PackRSAKeyPairInPKCS12A33(keyPair, certificate, null, null); }

        public static byte[] PackRSAKeyPairInPKCS12A33(this IAdvancedSecurityService s, RSAKeyPair keyPair, X509CertificateBase certificate, string encryptionPassphrase, string integrityPassphrase)
        {
            var keyStore = new PKCS12KeyStore(defaultAlias, keyPair, certificate);

            using (var stream = new MemoryStream())
            {
                keyStore.Save(stream, encryptionPassphrase, integrityPassphrase);

                return stream.ToArray();
            }
        }

        public static byte[] CreateRSAKeyPairInPKCS12EncryptionIntegrityA31(this IAdvancedSecurityService s, string passphraseEncryption, string passphraseIntegrity)
        {
            RSAKeyPair keyPair;
            var certificate = s.CreateTestSelfSignedCACertificateA4(out keyPair);

            return s.PackRSAKeyPairInPKCS12A33(keyPair, certificate, passphraseEncryption, passphraseIntegrity);
        }

        public static byte[] CreateRSAKeyPairInPKCS12EncryptionIntegrityA31(this IAdvancedSecurityService s, string passphrase)
        {
            return s.CreateRSAKeyPairInPKCS12EncryptionIntegrityA31(passphrase, passphrase);
        }

        public static byte[] CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(this IAdvancedSecurityService s, out RSAKeyPair keyPair, out X509CertificateBase certificate, string subjectDN = "CN=ONVIF TT,C=US")
        {
            certificate = s.CreateTestSelfSignedCACertificateA4(out keyPair, subjectDN);

            return s.PackRSAKeyPairInPKCS12A32(keyPair, certificate);
        }

        public static byte[] CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(this IAdvancedSecurityService s, out X509CertificateBase certificate, string subjectDN = "CN=ONVIF TT,C=US")
        {
            RSAKeyPair keyPair;
            return s.CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(out keyPair, out certificate, subjectDN);
        }

        public static byte[] CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(this IAdvancedSecurityService s, string subjectDN = "CN=ONVIF TT,C=US")
        {
            RSAKeyPair keyPair;
            X509CertificateBase certificate;
            return s.CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(out keyPair, out certificate, subjectDN);
        }

        public static DistinguishedName CertificateSubjectA34(this IAdvancedSecurityService s)
        {
            return new DistinguishedName()
                       {
                           Country = new[] { "US" },
                           Organization = new[] { "ONVIF Test" },
                           OrganizationalUnit = new[] { "Unit test" },
                           DistinguishedNameQualifier = new[] { "Unit test" },
                           StateOrProvinceName = new[] { "State Name Test" },
                           CommonName = new[] { "Common Name Test" },
                           SerialNumber = new[] { "000000000000" },
                           Locality = new[] { "LA" },
                           Title = new[] { "Mr" },
                           Surname = new[] { "SurnameTest" },
                           GivenName = new[] { "GivenNameTest" },
                           Initials = new[] { "AS" },
                           Pseudonym = new[] { "Pseudonym Test" },
                           //GenerationQualifier = new [] { "GenerationQualifier Test" },
                           //GenericAttribute = new [] { new DNAttributeTypeAndValue() { Type = "string", Value = "Test GenericAttribute" } },
                           //MultiValuedRDN = new [] { new MultiValuedRDN() { Attribute = new [] { new DNAttributeTypeAndValue() { Type = "string", Value = "Test MultyValueRDN" } } } },
                       };
        }

        /// <summary>
        /// Annex A.36: Helper procedure to create and upload PKCS#12 data structure with new public key and public static key.
        /// </summary>
        /// <param name="certificate">Certificate to be loaded</param>
        /// <param name="keyID">ID of RSA keypair for that certificate is created</param>
        /// <returns></returns>
        public static void HelperUploadPKCS12A36(this IAdvancedSecurityService s, out string keyID, out RSAKeyPair keyPair, out X509CertificateBase certificate, out string certificationPathID)
        {
            var pkcs12 = s.CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(out keyPair, out certificate);
            certificationPathID = s.UploadCertificateWithPrivateKeyInPKCS12(pkcs12, defaultCertificationPathAlias, defaultKeyAlias, false, null, null, out keyID);
        }

        public static void DeleteCertificationPathA35(this IAdvancedSecurityService s, string keyID, string certificationPathID)
        {
            var deleteKey = true;
            if (!string.IsNullOrEmpty(certificationPathID))
            {
                CertificationPath certificationPath = null;
                s.Test.AllowFaultStep(() => certificationPath = s.GetCertificationPath(certificationPathID));

                if (null != certificationPath)
                {
                    s.Test.AllowFaultStep(() => s.DeleteCertificationPath(certificationPathID));

                    foreach (var certificateID in certificationPath.CertificateID ?? new string[0])
                    {
                        s.Test.AllowFaultStep(() =>
                                              {
                                                  var certificate = s.GetCertificate(certificateID);
                                                  s.DeleteCertificate(certificateID);
                                                  s.DeleteRSAKeyPair(certificate.KeyID);
                                              });
                    }

                    //RSA keypair is already deleted in the cycle above
                    deleteKey = false;
                }
            }

            if (deleteKey && !string.IsNullOrEmpty(keyID))
                s.Test.AllowFaultStep(() => s.DeleteRSAKeyPair(keyID));
        }

        public static CertificateRevocationListBC HelperCreateCRLA37(this IAdvancedSecurityService s)
        {
            var generator = new CertificateRevocationListGeneratorBC();

            generator.SetIssuerDN("CN=ONVIF TT,C=US");
            generator.SetThisUpdate(DateTime.UtcNow);
            generator.SetSignatureAlgorithm(DefaultSignatureAlgorithmIdentifier.algorithm);
            generator.AddCRLEntry("1234567890", DateTime.UtcNow);

            return generator.Generate(new RSAKeyPairGeneratorBC(1024).Generate()) as CertificateRevocationListBC;
        }

        public static void HelperPrepareCertificateA41(this IAdvancedSecurityService s,
                                                       out string keyPairID,
                                                       out string certificateID,
                                                       out string certificationPathID)
        {
            var serviceCapabilities = s.GetServiceCapabilities();

            if (null != serviceCapabilities.KeystoreCapabilities)
            {
                if (serviceCapabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified &&
                    serviceCapabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA)
                {
                    var certificate = s.CreateTestSelfSignedCACertificateA4();
                    s.UploadCertificateWithoutPrivateKeyA15(certificate, out keyPairID, out certificateID);
                    certificationPathID = string.Empty;

                    return;
                }

                if (serviceCapabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified &&
                    serviceCapabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA &&
                    serviceCapabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified &&
                    serviceCapabilities.KeystoreCapabilities.RSAKeyPairGeneration)
                {
                    certificateID = s.CreateTestSelfSignedCertificateA8(out keyPairID);
                    certificationPathID = string.Empty;

                    return;
                }

                if (serviceCapabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUploadSpecified &&
                    serviceCapabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload)
                {
                    RSAKeyPair keyPair;
                    X509CertificateBase certificate;
                    s.HelperUploadPKCS12A36(out keyPairID, out keyPair, out certificate, out certificationPathID);

                    var certificationPath = s.GetCertificationPath(certificationPathID);
                    s.Test.Assert(null != certificationPath && null != certificationPath.CertificateID && certificationPath.CertificateID.Any(),
                                  "The returned CertificationPath is empty",
                                  "Checking returned Certification Path");

                    certificateID = certificationPath.CertificateID.First();

                    return;
                }
            }

            keyPairID = certificateID = certificationPathID = string.Empty;

            s.Test.Assert(false,
                          "The DUT can neither upload nor generate a certificate on-site",
                          "Checking KeyStore Capabilities of the DUT");
        }

        public static void HelperCreateCertPathValidationPolicyA42(this IAdvancedSecurityService s,
                                                                   out string keyPairID,
                                                                   out string certificateID,
                                                                   out string certificationPathID,
                                                                   out CertPathValidationPolicy validationPolicy,
                                                                   string alias = defaultCertificationPathValidationPolicyAlias)
        {
            s.HelperPrepareCertificateA41(out keyPairID, out certificateID, out certificationPathID);

            s.HelperCreateCertPathValidationPolicyWithCertIDA44(out validationPolicy, certificateID, alias);
        }

        public static X509CertificateBase HelperCreateSignedCertificateA43(this IAdvancedSecurityService s, 
                                                                           out RSAKeyPair keyPair,
                                                                           RSAKeyPair caKeyPair, X509CertificateBase issuerCertificate, string subjectDN)
        {
            keyPair = new RSAKeyPairGeneratorBC(1024).Generate();
            return GenerateSignedCertificate(caKeyPair.PrivateKey,
                                             keyPair.PublicKey,
                                             issuerCertificate.NotValidBefore,
                                             issuerCertificate.NotValidAfter,
                                             issuerCertificate.IssuerDN,
                                             subjectDN);
        }

        public static void HelperCreateCertPathValidationPolicyWithCertIDA44(this IAdvancedSecurityService s,
                                                                             out CertPathValidationPolicy validationPolicy,
                                                                             string certificateID,
                                                                             string alias = defaultCertificationPathValidationPolicyAlias)
        {
            var parameters = new CertPathValidationParameters()
                             {
                                 RequireTLSWWWClientAuthExtendedKeyUsageSpecified = false,
                                 UseDeltaCRLs = true,
                                 anyParameters = null
                             };

            validationPolicy = new CertPathValidationPolicy()
                               {
                                   Alias = alias,
                                   TrustAnchor = new []{ new TrustAnchor() { CertificateID = certificateID } },
                                   Parameters = parameters,
                                   anyParameters = null
                               };

            validationPolicy.CertPathValidationPolicyID = s.CreateCertPathValidationPolicy(validationPolicy.Alias, validationPolicy.Parameters, validationPolicy.TrustAnchor, null);
        }        

        public static CertificateRevocationListBase HelperCreateCRLForCertificateA45(this IAdvancedSecurityService s,
                                                                                     X509CertificateBase certificateToRevoce,
                                                                                     RSAKeyPair issuerKeyPair,
                                                                                     string issuerDN)
        {
            var generator = new CertificateRevocationListGeneratorBC();

            generator.SetIssuerDN(issuerDN);
            generator.SetThisUpdate(DateTime.UtcNow);
            generator.SetSignatureAlgorithm(DefaultSignatureAlgorithmIdentifier.algorithm);
            generator.AddCRLEntry(certificateToRevoce.SerialNumber, DateTime.UtcNow);

            return generator.Generate(issuerKeyPair) as CertificateRevocationListBC;
        }        
        #endregion

        #region Equals

        public static bool EqualCertPathValidationParameters(CertPathValidationParameters original,
                                                             CertPathValidationParameters received,
                                                             TabulatedStringBuilder logger,
                                                             string headerFirst,
                                                             string headerSecond)
        {
            try
            {
                logger.IncreaseTabSize();

                var flag = true;

                string msgHeader = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);
                string msgHeader1 = string.Format("The field '{{0}}' is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);

                //'FieldSpecified is false' == 'FieldSpecified is true and FieldValue is false'
                var originalValue = original.RequireTLSWWWClientAuthExtendedKeyUsageSpecified ? original.RequireTLSWWWClientAuthExtendedKeyUsage : false;
                var receivedValue = received.RequireTLSWWWClientAuthExtendedKeyUsageSpecified ? received.RequireTLSWWWClientAuthExtendedKeyUsage : false;
                if (originalValue != receivedValue)
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader1, "RequireTLSWWWClientAuthExtendedKeyUsage",
                                                    original.RequireTLSWWWClientAuthExtendedKeyUsageSpecified 
                                                    ? 
                                                    string.Format("present and equals to '{0}'", original.RequireTLSWWWClientAuthExtendedKeyUsage) : "absent",
                                                    received.RequireTLSWWWClientAuthExtendedKeyUsageSpecified 
                                                    ? 
                                                    string.Format("present and equals to '{0}'", received.RequireTLSWWWClientAuthExtendedKeyUsage) : "absent"));
                }

                //if (original.RequireTLSWWWClientAuthExtendedKeyUsageSpecified !=
                //    received.RequireTLSWWWClientAuthExtendedKeyUsageSpecified)
                //{
                //    flag = false;
                //    logger.AppendLine(string.Format(msgHeader1, "RequireTLSWWWClientAuthExtendedKeyUsage",
                //                                    original.RequireTLSWWWClientAuthExtendedKeyUsageSpecified ? "present" : "absent",
                //                                    received.RequireTLSWWWClientAuthExtendedKeyUsageSpecified ? "present" : "absent"));
                //}
                //else if (original.RequireTLSWWWClientAuthExtendedKeyUsageSpecified &&
                //         original.RequireTLSWWWClientAuthExtendedKeyUsage != received.RequireTLSWWWClientAuthExtendedKeyUsage)
                //{
                //    flag = false;
                //    logger.AppendLine(string.Format(msgHeader, "RequireTLSWWWClientAuthExtendedKeyUsage",
                //                                    original.RequireTLSWWWClientAuthExtendedKeyUsage,
                //                                    received.RequireTLSWWWClientAuthExtendedKeyUsage));
                //}

                if (original.UseDeltaCRLsSpecified != received.UseDeltaCRLsSpecified)
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader1, "UseDeltaCRLs",
                                                    original.UseDeltaCRLsSpecified ? "present" : "absent",
                                                    received.UseDeltaCRLsSpecified ? "present" : "absent"));
                }
                else if (original.UseDeltaCRLsSpecified && original.UseDeltaCRLs != received.UseDeltaCRLs)
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader, "UseDeltaCRLs", original.UseDeltaCRLs, received.UseDeltaCRLs));
                }

                return flag;
            }
            finally
            {
                logger.DecreaseTabSize();
            }
        }

        public static bool EqualCertPathValidationPolicy(CertPathValidationPolicy original,
                                                         CertPathValidationPolicy received,
                                                         TabulatedStringBuilder logger,
                                                         string headerFirst,
                                                         string headerSecond)
        {
            try
            {
                logger.IncreaseTabSize();

                var flag = true;

                string msgHeader = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);

                if (original.CertPathValidationPolicyID != received.CertPathValidationPolicyID)
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader, "CertPathValidationPolicyID", original.CertPathValidationPolicyID, received.CertPathValidationPolicyID));
                }

                if (original.Alias != received.Alias)
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader, "Alias", original.Alias, received.Alias));
                }

                var internalLogger = new TabulatedStringBuilder(logger.TabStep, logger.StepCount);
                internalLogger.AppendLine("'Parameters' fields are inconsistent.");
                if (!EqualCertPathValidationParameters(original.Parameters, received.Parameters, internalLogger, headerFirst, headerSecond))
                {
                    flag = false;
                    logger.Append(internalLogger);
                }

                internalLogger.Clear();
                internalLogger.AppendLine("TrustAnchor lists are inconsistent.");
                if (!EqualTrustAnchorLists(original.TrustAnchor, received.TrustAnchor, internalLogger, headerFirst, headerSecond))
                {
                    flag = false;
                    logger.Append(internalLogger);
                }

                return flag;
            }
            finally 
            {
                logger.DecreaseTabSize();
            }
        }

        public static bool EqualTrustAnchorLists(IEnumerable<TrustAnchor> original,
                                                 IEnumerable<TrustAnchor> received,
                                                 TabulatedStringBuilder logger,
                                                 string headerFirst,
                                                 string headerSecond)
        {
            try
            {
                logger.IncreaseTabSize();

                if (null == original)
                    original = new TrustAnchor[0];

                if (null == received)
                    received = new TrustAnchor[0];

                if (original.Count() != received.Count())
                {
                    logger.AppendLine("TrustAnchor lists has different number of items");
                    return false;
                }

                var originalCertificateIDs = original.Select(e => e.CertificateID);
                if (originalCertificateIDs.Count() != originalCertificateIDs.Distinct().Count())
                {
                    logger.AppendLine(string.Format("TrustAnchor list from {0} has items with non-unique CertificateID", headerFirst));
                    return false;
                }

                var flag = true;

                foreach (var anchor in original)
                {
                    var twins = received.Where(e => e.CertificateID == anchor.CertificateID);

                    if (1 != twins.Count())
                    {
                        if (twins.Count() >= 2)
                            logger.AppendLine(string.Format("There are many corresponding TrustAnchor items for item with CertificateID = '{0}' while only one is expected",
                                                            anchor.CertificateID));
                        else
                            logger.AppendLine(string.Format("There is no corresponding TrustAnchor item for item with CertificateID = '{0}'",
                                                            anchor.CertificateID));
                        flag = false;
                    }
                    else
                    {
                        //Ok
                    }
                }

                return flag;
            }
            finally
            {
                logger.DecreaseTabSize();
            }
        }
        #endregion
    }
}
