using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;
using DUT.CameraWebService.Common;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System.Security.Cryptography.X509Certificates;
using CameraWebService.Servers;
using Org.BouncyCastle.X509;

namespace DUT.CameraWebService.AdvancedSecurity10
{

    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class AdvancedSecurityServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName
        {
            get
            {
                //Used in DUT to define service name for command
                return "AdvancedSecurity10";
            }
        }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int CreateRSAKeyPair = 1;
        private const int GetKeyStatus = 2;
        private const int GetPrivateKeyStatus = 3;
        private const int GetAllKeys = 4;
        private const int DeleteKey = 5;
        private const int CreatePKCS10CSR = 6;
        private const int CreateSelfSignedCertificate = 7;
        private const int UploadCertificate = 8;
        private const int GetCertificate = 9;
        private const int GetAllCertificates = 10;
        private const int DeleteCertificate = 11;
        private const int DeleteCertificationPath = 12;
        private const int AddServerCertificateAssignment = 13;
        private const int RemoveServerCertificateAssignment = 14;
        private const int ReplaceServerCertificateAssignment = 15;
        private const int CreateCertificationPath = 16;
        private const int GetAllCertificationPaths = 17;
        private const int GetAssignedServerCertificates = 18;
        private const int GetCertificationPath = 19;
        private const int UploadPassphrase = 20;
        private const int DeletePassphrase = 21;
        private const int GetAllPassphrases = 22;
        private const int UploadKeyPairInPKCS8 = 23;
        private const int UploadCertificateWithPrivateKeyInPKCS12 = 24;
        private const int GetCRL = 25;
        private const int GetAllCRLs = 26;
        private const int DeleteCRL = 27;
        private const int CreateCertPathValidationPolicy = 28;
        private const int GetCertPathValidationPolicy = 29;
        private const int GetAllCertPathValidationPolicies = 30;
        private const int DeleteCertPathValidationPolicy = 31;
        private const int DeleteUnreferencedCertPathValidationPolicies = 32;
        private const int SetClientAuthenticationRequired = 33;
        private const int GetClientAuthenticationRequired = 34;
        private const int AddCertPathValidationPolicyAssignment = 35;
        private const int RemoveCertPathValidationPolicyAssignment = 36;
        private const int ReplaceCertPathValidationPolicyAssignment = 37;
        private const int GetAssignedCertPathValidationPolicies = 38;
        private const int AddDot1XConfiguration = 39;
        private const int GetAllDot1XConfigurations = 40;
        private const int GetDot1XConfiguration = 41;
        private const int DeleteDot1XConfiguration = 42;
        private const int SetNetworkInterfaceDot1XConfiguration = 43;
        private const int GetNetworkInterfaceDot1XConfiguration = 44;
        private const int DeleteNetworkInterfaceDot1XConfiguration = 45;
        private const int UploadCRL = 46;
        private const int MaxCommands = 47;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AdvancedSecurityServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

        private string m_X509CertificateFromUploadPKCS12Alias = null;
        private byte[] m_X509CertificateFromUploadPKCS12 = null;
        private byte[] m_X509CertificateFromGet = null;
        private byte[] m_UploadPKCS12 = null;
        private byte[] m_X509CertificateFromUpload = null;
        private string m_X509CertificateFromUploadAlias = null;
        private Org.BouncyCastle.X509.X509Certificate m_X509CertificateSS = null;
        private AsymmetricCipherKeyPair m_RSAKeyPair = null;
        private string m_CRLFromUploadAlias = null;
        private byte[] m_CRLFromUpload = null;

        #region General

        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            int special = 0;
            Capabilities result = GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out timeout, out special);

            switch (special)
            {
                case 1:
                    result.KeystoreCapabilities.RSAKeyLengths = new string[0];
                    break;
                case 2:
                    result.TLSServerCapabilities.TLSServerSupported = new string[0];
                    break;
                case 3:
                    result.KeystoreCapabilities.RSAKeyLengths = new string[0];
                    result.TLSServerCapabilities.TLSServerSupported = new string[0];
                    break;
                case 4:
                    result.KeystoreCapabilities.X509Versions = new int[0];
                    break;
                case 5:
                    result.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms = new string[0];
                    break;
                case 6:
                    result.KeystoreCapabilities.PasswordBasedMACAlgorithms = new string[0];
                    break;
            }

            return result;

        }

        #endregion //General

        internal string CreateRSAKeyPairTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;
            var res = GetCommand<string>("CreateRSAKeyPair", CreateRSAKeyPair, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (special == 1)
            {
                var generator = new RsaKeyPairGenerator();
                generator.Init(new KeyGenerationParameters(new SecureRandom(), 1024));

                m_RSAKeyPair = generator.GenerateKeyPair();
            }

            return res;
        }

        internal string TakeEstimatedCreationTime()
        {
            return TakeSpecialParameterSimple<string>("CreateRSAKeyPair", CreateRSAKeyPair, "EstimatedCreationTime");
        }

        internal string TakeKeyIDFromPKCS12()
        {
            return TakeSpecialParameterSimple<string>("UploadCertificateWithPrivateKeyInPKCS12", UploadCertificateWithPrivateKeyInPKCS12, "KeyID");
        }

        internal string GetKeyStatusTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetKeyStatus", GetKeyStatus, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal bool GetPrivateKeyStatusTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            //bool result = GetCommand<bool>("GetPrivateKeyStatus", GetPrivateKeyStatus, validationRequest, true, out stepType, out exc, out timeout);
            //return (object)result;
            return GetCommand<bool>("GetPrivateKeyStatus", GetPrivateKeyStatus, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal KeyAttribute[] GetAllKeysTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<KeyAttribute[]>("GetAllKeys", GetAllKeys, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeleteKeyTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteKey", DeleteKey, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal byte[] CreatePKCS10CSRTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout, DistinguishedName Subject, string KeyID, CSRAttribute[] CSRAttribute, AlgorithmIdentifier SignatureAlgorithm)
        {
            int special;


            VoidCommand("CreatePKCS10CSR", CreatePKCS10CSR, validationRequest, true, out stepType, out exc, out timeout, out special);

            byte[] result;


            switch (special)
            {
                case 1:
                    //Correct response
                    var subject = "";
                    if (null != Subject)
                    {
                        var r = new StringBuilder();
                        if (null != Subject.CommonName)
                            r.Append(string.Format("CN={0},", Subject.CommonName));
                        if (null != Subject.Country)
                            r.Append(string.Format("C={0},", Subject.Country));
                        if (null != Subject.Locality)
                            r.Append(string.Format("L={0},", Subject.Locality));
                        if (null != Subject.Organization)
                            r.Append(string.Format("O={0},", Subject.Organization));
                        if (null != Subject.OrganizationalUnit)
                            r.Append(string.Format("OU={0},", Subject.OrganizationalUnit));
                        if (null != Subject.StateOrProvinceName)
                            r.Append(string.Format("ST={0},", Subject.StateOrProvinceName));

                        subject = r.ToString().TrimEnd(',');
                    }

                    var generator = new RsaKeyPairGenerator();
                    generator.Init(new KeyGenerationParameters(new SecureRandom(), 1024));

                    var keyPair = generator.GenerateKeyPair();

                    var signatureAlg = "SHA1WithRSAEncryption";
                    if (null != SignatureAlgorithm && !string.IsNullOrEmpty(SignatureAlgorithm.algorithm))
                        signatureAlg = SignatureAlgorithm.algorithm;

                    var csr = new Pkcs10CertificationRequest(signatureAlg, new X509Name(subject), keyPair.Public, null, keyPair.Private);
                    TestCommon.writeToLogInfo("Public Key: " + csr.GetCertificationRequestInfo().SubjectPublicKeyInfo.PublicKeyData.ToString());
                    TestCommon.writeToLogInfo("Signature: " + csr.Signature.ToString());
                    TestCommon.writeToLogInfo("SignatureAlgorithm: " + csr.SignatureAlgorithm.ObjectID.ToString());
                    TestCommon.writeToLogInfo("Subject: " + csr.GetCertificationRequestInfo().Subject.ToString());

                    result = csr.GetEncoded();
                    break;
                case 2:
                    //with sign error, sign lenght 1024
                    result = TestCommon.ReadBinary(TestCommon.PCS10Binary3Uri);
                    break;
                case 3:
                    //without error, sign lenght 3072
                    result = new byte[1];
                    break;
                case 4:
                    //without error, sign lenght 3072
                    result = new byte[0];
                    break;
                case 5:
                    //without sign error, sign lenght 1024, with wrong subject
                    result = TestCommon.ReadBinary(TestCommon.PCS10Binary2Uri);
                    break;
                default:
                    result = null;
                    break;
            }

            return result;

        }

        internal string CreateSelfSignedCertificateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;
            var res = GetCommand<string>("CreateSelfSignedCertificate", CreateSelfSignedCertificate, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (special == 1)
            {
                var generator = new X509V3CertificateGenerator();
                generator.SetSerialNumber(new Org.BouncyCastle.Math.BigInteger( DateTime.Now.Ticks.ToString()));
                generator.SetIssuerDN(new X509Name("CN=ONVIF TT,C=US"));
                generator.SetPublicKey(m_RSAKeyPair.Public);
                generator.SetNotBefore(DateTime.Now.AddYears(-1));
                generator.SetNotAfter(DateTime.Now.AddYears(1));
                generator.SetSubjectDN(new X509Name("CN=ONVIF TT,C=US"));

                generator.SetSignatureAlgorithm("SHA1WithRSAEncryption");

                m_X509CertificateSS = generator.Generate(m_RSAKeyPair.Private);
            }
            return res;

        }

        internal string TakeKeyID()
        {
            return TakeSpecialParameterSimple<string>("UploadCertificate", UploadCertificate, "KeyID");
        }

        internal string UploadCertificateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            var r = GetCommand<string>("UploadCertificate", UploadCertificate, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (0 != special)
            {
                m_X509CertificateFromUpload = (byte[])validationRequest.ValidationRules.First(rule => rule.ParameterName == "Certificate").Value;
                m_X509CertificateFromUploadAlias = (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Alias").Value;
            }

            return r;
        }

        internal X509Certificate GetCertificateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;
            
            var r = GetCommand<X509Certificate>("GetCertificate", GetCertificate, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (1 == special && null != m_X509CertificateFromUploadPKCS12Alias)
            {
                if (null != r && m_X509CertificateFromUploadPKCS12Alias == r.Alias)
                {
                    r.CertificateContent = m_X509CertificateFromUploadPKCS12;
                }
            }

            if (2 == special && null != m_X509CertificateFromUploadAlias)
            {
                if (null != r && m_X509CertificateFromUploadAlias == r.Alias)
                {
                    r.CertificateContent = m_X509CertificateFromUpload;
                }
            }

            if (3 == special && null != m_X509CertificateFromUploadAlias)
            {
                if (null != r && m_X509CertificateFromUploadAlias == r.Alias)
                {
                    r.CertificateContent = TestCommon.ReadBinary(TestCommon.TLSCertificate1Uri);
                }
            }
            else
            {
                if (3 == special && null != r && "ONVIF_Certificate_Test" == r.Alias)
                {
                    r.CertificateContent = TestCommon.ReadBinary(TestCommon.TLSCertificate1Uri);
                }
            }

            if (4 == special)
            {
                r.CertificateContent = m_X509CertificateSS.GetEncoded();
            }

            if (r != null)
            {
                m_X509CertificateFromGet = r.CertificateContent;
            }

            return r;
        }

        internal X509Certificate[] GetAllCertificatesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;
            var r = GetCommand<X509Certificate[]>("GetAllCertificates", GetAllCertificates, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (1 == special && null != m_X509CertificateFromUploadPKCS12Alias)
            {
                var certificate = r.FirstOrDefault(c => c.Alias == m_X509CertificateFromUploadPKCS12Alias);
                if (null != certificate)
                {
                    certificate.CertificateContent = m_X509CertificateFromUploadPKCS12;
                }
                
                certificate = r.FirstOrDefault(c => c.Alias == m_X509CertificateFromUploadPKCS12Alias + "Invalid");
                if (null != certificate)
                {
                    certificate.CertificateContent = m_X509CertificateFromUploadPKCS12;
                }
            }

            if (2 == special && null != m_X509CertificateFromUploadAlias)
            {

                var certificate = r.FirstOrDefault(c => c.Alias == m_X509CertificateFromUploadAlias);
                if (null != certificate)
                {
                    certificate.CertificateContent = m_X509CertificateFromUpload;
                }

                certificate = r.FirstOrDefault(c => c.Alias == m_X509CertificateFromUploadAlias + "Invalid");
                if (null != certificate)
                {
                    certificate.CertificateContent = m_X509CertificateFromUpload;
                }
            }

            return r;
        }

        internal void DeleteCertificateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteCertificate", DeleteCertificate, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeleteCertificationPathTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteCertificationPath", DeleteCertificationPath, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void AddServerCertificateAssignmentTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;
            VoidCommand("AddServerCertificateAssignment", AddServerCertificateAssignment, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (1 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(m_UploadPKCS12), 1000);
            }
            if (2 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(TestCommon.ReadBinary(TestCommon.TLSCertificate1Uri), "1234"), 1000);
            }
            if (3 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(m_X509CertificateFromGet), 1000);
            }
            if (4 == special)
            {
                var keyStore = new Pkcs12StoreBuilder().SetUseDerEncoding(true).Build();
                keyStore.SetKeyEntry("KeyAlias",
                                   new AsymmetricKeyEntry(m_RSAKeyPair.Private),
                                   new[] { new X509CertificateEntry(m_X509CertificateSS) });
                using (var stream = new MemoryStream())
                {
                    keyStore.Save(stream, "".ToCharArray(), new SecureRandom());

                    HTTPSServer.getInstance(true).Run(new X509Certificate2(stream.ToArray(), ""), 1000);
                }
            }
            if (-1 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(m_UploadPKCS12), -1);
            }
        }

        internal void RemoveServerCertificateAssignmentTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            VoidCommand("RemoveServerCertificateAssignment", RemoveServerCertificateAssignment, validationRequest, true, out stepType, out exc, out timeout, out special);
            if (1 == special)
            {
                //HTTPSServer.getInstance().Stop();
            }
        }

        internal void ReplaceServerCertificateAssignmentTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            VoidCommand("ReplaceServerCertificateAssignment", ReplaceServerCertificateAssignment, validationRequest, true, out stepType, out exc, out timeout, out special);
            if (1 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(m_UploadPKCS12), 1000);
            }
            if (2 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(TestCommon.ReadBinary(TestCommon.TLSCertificate1Uri), "1234"), 1000);
            }
            if (3 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(m_X509CertificateFromGet), 1000);
            }
            if (4 == special)
            {
                var keyStore = new Pkcs12StoreBuilder().SetUseDerEncoding(true).Build();
                keyStore.SetKeyEntry("KeyAlias",
                                   new AsymmetricKeyEntry(m_RSAKeyPair.Private),
                                   new[] { new X509CertificateEntry(m_X509CertificateSS) });
                using (var stream = new MemoryStream())
                {
                    keyStore.Save(stream, "".ToCharArray(), new SecureRandom());

                    HTTPSServer.getInstance(true).Run(new X509Certificate2(stream.ToArray(), ""), 1000);
                }
            }
            if (-1 == special)
            {
                HTTPSServer.getInstance(true).Run(new X509Certificate2(m_UploadPKCS12), -1);
            }
        }

        internal object CreateCertificationPathTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("CreateCertificationPath", CreateCertificationPath, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal CertificationPath GetCertificationPathTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<CertificationPath>("GetCertificationPath", GetCertificationPath, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string[] GetAllCertificationPathsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string[]>("GetAllCertificationPaths", GetAllCertificationPaths, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string[] GetAssignedServerCertificatesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string[]>("GetAssignedServerCertificates", GetAssignedServerCertificates, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string UploadPassphraseTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("UploadPassphrase", UploadPassphrase, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeletePassphraseTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeletePassphrase", DeletePassphrase, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal PassphraseAttribute[] GetAllPassphrasesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<PassphraseAttribute[]>("GetAllPassphrases", GetAllPassphrases, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string UploadKeyPairInPKCS8Test(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("UploadKeyPairInPKCS8", UploadKeyPairInPKCS8, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string UploadCertificateWithPrivateKeyInPKCS12Test(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            var r = GetCommand<string>("UploadCertificateWithPrivateKeyInPKCS12", UploadCertificateWithPrivateKeyInPKCS12, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (0 != special)
            {
                var pkcs12Binary = (byte[])validationRequest.ValidationRules.First(rule => rule.ParameterName == "CertWithPrivateKey").Value;
                var passphraseID = validationRequest.ValidationRules.First(rule => rule.ParameterName == "EncryptionPassphraseID").Value;

                var pkcs12Store = new Org.BouncyCastle.Pkcs.Pkcs12Store();
                pkcs12Store.Load(new MemoryStream(pkcs12Binary), ((null != passphraseID) ? "DefaultPassword" : "").ToArray());

                m_X509CertificateFromUploadPKCS12 = pkcs12Store.GetCertificate(pkcs12Store.Aliases.OfType<string>().First()).Certificate.GetEncoded();

                m_X509CertificateFromUploadPKCS12Alias = (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "CertificationPathAlias").Value;

                m_UploadPKCS12 = pkcs12Binary;
            }

            return r;
        }

        internal CRL GetCRLTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            var res = GetCommand<CRL>("GetCRL", GetCRL, validationRequest, true, out stepType, out exc, out timeout, out special);


            if (1 == special && null != m_CRLFromUploadAlias)
            {

                if (null != res)
                {
                    res.CRLContent = m_CRLFromUpload;
                }
            }

            return res;
        }

        internal CRL[] GetAllCRLsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            var res = GetCommand<CRL[]>("GetAllCRLs", GetAllCRLs, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (1 == special && null != m_CRLFromUploadAlias)
            {

                var crl = res.FirstOrDefault(c => c.Alias == m_CRLFromUploadAlias);
                if (null != crl)
                {
                    crl.CRLContent = m_CRLFromUpload;
                }

                crl = res.FirstOrDefault(c => c.Alias == m_CRLFromUploadAlias + "Invalid");
                if (null != crl)
                {
                    crl.CRLContent = m_CRLFromUpload;
                }
            }

            return res;
        }

        internal void DeleteCRLTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteCRL", DeleteCRL, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string CreateCertPathValidationPolicyTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("CreateCertPathValidationPolicy", CreateCertPathValidationPolicy, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal CertPathValidationPolicy GetCertPathValidationPolicyTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<CertPathValidationPolicy>("GetCertPathValidationPolicy", GetCertPathValidationPolicy, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal CertPathValidationPolicy[] GetAllCertPathValidationPoliciesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<CertPathValidationPolicy[]>("GetAllCertPathValidationPolicies", GetAllCertPathValidationPolicies, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeleteCertPathValidationPolicyTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteCertPathValidationPolicy", DeleteCertPathValidationPolicy, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeleteUnreferencedCertPathValidationPoliciesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteUnreferencedCertPathValidationPolicies", DeleteUnreferencedCertPathValidationPolicies, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetClientAuthenticationRequiredTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetClientAuthenticationRequired", SetClientAuthenticationRequired, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal bool GetClientAuthenticationRequiredTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<bool>("GetClientAuthenticationRequired", GetClientAuthenticationRequired, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void AddCertPathValidationPolicyAssignmentTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("AddCertPathValidationPolicyAssignment", AddCertPathValidationPolicyAssignment, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void RemoveCertPathValidationPolicyAssignmentTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("RemoveCertPathValidationPolicyAssignment", RemoveCertPathValidationPolicyAssignment, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void ReplaceCertPathValidationPolicyAssignmentTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("ReplaceCertPathValidationPolicyAssignment", ReplaceCertPathValidationPolicyAssignment, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string[] GetAssignedCertPathValidationPoliciesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string[]>("GetAssignedCertPathValidationPolicies", GetAssignedCertPathValidationPolicies, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string AddDot1XConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("AddDot1XConfiguration", AddDot1XConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal Dot1XConfiguration[] GetAllDot1XConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Dot1XConfiguration[]>("GetAllDot1XConfigurations", GetAllDot1XConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal Dot1XConfiguration GetDot1XConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Dot1XConfiguration>("GetDot1XConfiguration", GetDot1XConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeleteDot1XConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteDot1XConfiguration", DeleteDot1XConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal bool SetNetworkInterfaceDot1XConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<bool>("SetNetworkInterfaceDot1XConfiguration", SetNetworkInterfaceDot1XConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string GetNetworkInterfaceDot1XConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetNetworkInterfaceDot1XConfiguration", GetNetworkInterfaceDot1XConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal bool DeleteNetworkInterfaceDot1XConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<bool>("DeleteNetworkInterfaceDot1XConfiguration", DeleteNetworkInterfaceDot1XConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string UploadCRLTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            var r = GetCommand<string>("UploadCRL", UploadCRL, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (0 != special)
            {
                m_CRLFromUpload = (byte[])validationRequest.ValidationRules.First(rule => rule.ParameterName == "Crl").Value;
                m_CRLFromUploadAlias = (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Alias").Value;
            }

            return r;
        }
    }
}