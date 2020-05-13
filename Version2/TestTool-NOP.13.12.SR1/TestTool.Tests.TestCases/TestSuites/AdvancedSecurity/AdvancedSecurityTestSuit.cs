///////////////////////////////////////////////////////////////////////////
//!  @author        Alexei Soloview
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.TestSuites.Events;
using DateTime = System.DateTime;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using TestTool.Tests.Common.TestBase;


namespace TestTool.Tests.TestCases.TestSuites.AdvancedSecurity
{
    [TestClass]
    partial class AdvancedSecurityTestSuit: NotificationsTestSuite
    {
        private const string PATH_GENERAL = "Advanced Security";
        private const string PATH_KEYSTORE = PATH_GENERAL + @"\Keystore";
        private const string PATH_CERTIFICATE_MANAGEMENT = PATH_GENERAL + @"\Certificate Management";
        private const string PATH_TLS_SERVER = PATH_GENERAL + @"\TLS Server";
        private const string PATH_REFERENTIAL_INTEGRITY = PATH_GENERAL + @"\Referential Integrity";
        private const string PATH_CAPABILITY = PATH_GENERAL + @"\Capabilities";

        public AdvancedSecurityTestSuit(TestLaunchParam param): base(param) {}

        [Test(Name = "Create RSA Key Pair - Status Using Polling",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_KEYSTORE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateRSAKeyPair, Functionality.GetKeyStatus })]
        public void CreateRSAKeyPairStatusThroughPolling()
        {
            string keyID = null;
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var capabilities = GetCapabilities();

                        Assert(null != capabilities.KeystoreCapabilities && null != capabilities.KeystoreCapabilities.RSAKeyLengths,
                               "The set of supported RSA key's length is empty",
                               "Check the set of supported RSA key's length isn't empty");

                        foreach (var keyLength in SelectKeyLengthsForTest(capabilities.KeystoreCapabilities.RSAKeyLengths))
                        {
                            CreateTestRSAKeyPairOfSpecifiedLength(out keyID, keyLength);

                            var local = keyID;
                            keyID = "";
                            DeleteRSAKeyPair(local);
                        }
                    },
                    () =>
                        {
                            if (!string.IsNullOrEmpty(keyID)) DeleteRSAKeyPair(keyID);

                            FinishRestoreSettings();
                        });
        }

        [Test(Name = "Create RSA Key Pair - Status Using Event",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_KEYSTORE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateRSAKeyPair })]
        public void CreateRSAKeyPairStatusThroughEvent()
        {
            SubscriptionHandler subscription = null;

            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var capabilities = GetCapabilities();

                        Assert(null != capabilities.KeystoreCapabilities && null != capabilities.KeystoreCapabilities.RSAKeyLengths && capabilities.KeystoreCapabilities.RSAKeyLengths.Any(),
                               "The set of supported RSA key's length is empty",
                               "Check the set of supported RSA key's length isn't empty");

                        subscription = new SubscriptionHandler(this, false, GetEventServiceAddress()){ PullTimeout = "PT60S" };

                        const string eventTopic = "tns1:Advancedsecurity/Keystore/KeyStatus";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var timeout = _operationDelay / 1000;

                        var filter = CreateFilter(topicInfo);
                        subscription.Subscribe(filter, timeout);

                        foreach (var keyLength in SelectKeyLengthsForTest(capabilities.KeystoreCapabilities.RSAKeyLengths))
                        {
                            string estimatedCreationTime = null;
                            var keyID = CreateRSAKeyPair(keyLength.ToString(), null, out estimatedCreationTime);

                            try
                            {
                                var deadline = DateTime.Now.AddSeconds(timeout);

                                var pollingCondition = new WaitNotificationForKeyPollingCondition(deadline, keyID)
                                                       {
                                                           //Filter = msg =>
                                                           //         {
                                                           //             var expected = topicInfo;
                                                           //             var actual = TopicInfo.ExtractTopicInfoPACS(eventTopic, msg.Message);
                                                           //             return TopicInfo.TopicsMatch(actual, expected);
                                                           //         }
                                                       };

                                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();

                                Assert(subscription.WaitMessages(1, pollingCondition, out notifications),
                                       "Timeout for key pair creation is expired",
                                       "Check timeout is not expired");

                                Assert(ksOK == pollingCondition.KeyStatus,
                                       string.Format("Key status is other than '{0}'", ksOK),
                                       string.Format("Check key status of key pair is '{0}'", ksOK));
                            }
                            finally 
                            {
                                DeleteRSAKeyPair(keyID);
                            }
                        }
                    },
                    () =>
                    {
                        if (null != subscription)
                            SubscriptionHandler.Unsubscribe(subscription);

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Check private key status for an RSA private key",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_KEYSTORE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPrivateKeyStatus })]
        public void CheckPrivateKeyStatusForRSAPrivateKey()
        {
            var rsaKeyID = "";

            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        CreateTestRSAKeyPairA7(out rsaKeyID);

                        Assert(GetPrivateKeyStatus(rsaKeyID),
                               "Private key status of created key pair is 'false'.",
                               "Check private key status of created key pair is 'true'");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Keys",
            Order = "01.01.04",
            Id = "1-1-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_KEYSTORE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllKeys })]
        public void GetAllKeysTest()
        {
            var newKey = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var allKeysBefore = GetAllKeys().Select(e => e.KeyID);

                        UpdateRSAKeyList(allKeysBefore, out newKey);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(newKey))
                                DeleteRSAKeyPair(newKey);
                        });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Key",
            Order = "01.01.05",
            Id = "1-1-5",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_KEYSTORE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteKey })]
        public void DeleteKey()
        {
            var newKey = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var allKeysBefore = GetAllKeys().Select(e => e.KeyID);

                        UpdateRSAKeyList(allKeysBefore, out newKey);

                        DeleteRSAKeyPair(newKey);
                        newKey = "";

                        var allKeysFinal = GetAllKeys().Select(e => e.KeyID);

                        Assert(allKeysBefore.All(allKeysFinal.Contains) && allKeysFinal.Count() == allKeysBefore.Count(),
                               "Key list received before new key's creation isn't the same as after removing created key",
                               "Check that key list received before new key's creation is the same as after removing created key");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(newKey))
                                               DeleteRSAKeyPair(newKey);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create PKCS#10",
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateSelfSignedCertificate })]
        public void CreatePKCS10()
        {
            var rsaKeyID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        CreateTestRSAKeyPairA7(out rsaKeyID);

                        ValidateCertificateSigningRequest(CreatePKCS10CSR(DefaultSubject, rsaKeyID, null, null), DefaultSubjectString);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create Self-Signed Certificate",
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateSelfSignedCertificate })]
        public void CreateSelfSignedCertificate()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certID = CreateTestSelfSignedCertificateA8(out rsaKeyID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – Keystore contains private key",
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateKeystoreContainsPrivateKey()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        RSAKeyPair caKeyPair = null;
                        CreateTestRSAKeyPairA7(out rsaKeyID);

                        var certificate = CreateTestCertificateSignedByCACertificateA14(CreateTestSelfSignedCACertificateA4(out caKeyPair), caKeyPair, rsaKeyID);

                        UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", true, out certID, rsaKeyID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – Keystore contains private key (negative test)",
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateKeystoreContainsPrivateKeyNegativeTest()
        {
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        try
                        {
                            var certificate = CreateTestSelfSignedCACertificateA4();
                            UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", true, out certID, null, true, false);

                            Assert(false, "The DUT did not send the env:Receiver/ter:Action/ter:NoMatchingPrivateKey SOAP 1.2 fault message.", "Fail the test");
                        }
                        catch (FaultException e)
                        {
                            if (e.IsValidOnvifFault("Receiver/Action/NoMatchingPrivateKey"))
                                StepPassed();
                            else
                            {
                                LogStepEvent("MESSAGE: The DUT did not send the env:Receiver/ter:Action/ter:NoMatchingPrivateKey SOAP 1.2 fault message.");
                                throw;
                            }
                        }
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – Keystore does not contain private key",
            Order = "02.01.05",
            Id = "2-1-5",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateKeystoreDoesNotContainsPrivateKey()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var certificate = CreateTestSelfSignedCACertificateA4();
                        UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", false, out rsaKeyID, out certID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certificate - Self-Signed",
            Order = "02.01.06",
            Id = "2-1-6",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificate })]
        public void GetCertificateSelfSigned()
        {
            var keyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certID = CreateTestSelfSignedCertificateA8(out keyID);

                        var cert = GetCertificate(certID);

                        Assert(ValidateDEREncoding(new MemoryStream(cert.CertificateContent)),
                               "X509Cert is wrongly DER encoded",
                               "Check certificate is correctly DER encoded");

                        //var expectedSubject = new X509Name(new List<DerObjectIdentifier>() { X509Name.CN, X509Name.C },
                        //                                   new List<string>() { _cameraIp.ToString(), "US" });

                        var expectedSubject = string.Format("CN={0},C=US", _cameraIp);

                        var certificate = new X509CertificateBC(new MemoryStream(cert.CertificateContent));

                        Assert(X509NamesAreEqual(certificate.SubjectDN, expectedSubject),
                               "Certificate's subject is invalid",
                               "Validating subject");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certID))
                                DeleteCertificate(certID);
                        });

                        AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(keyID))
                                DeleteRSAKeyPair(keyID);
                        });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certificate - CA",
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificate })]
        public void GetCertificateCA()
        {
            var keyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        UploadCertificateWithoutPrivateKeyA15(CreateTestSelfSignedCACertificateA4(), out keyID, out certID);

                        var cert = GetCertificate(certID);

                        Assert(ValidateDEREncoding(new MemoryStream(cert.CertificateContent)),
                               "X509Cert is wrongly DER encoded",
                               "Check certificate is correctly DER encoded");

                        var expectedSubject = string.Format("CN={0},C=US", _cameraIp);

                        var certificate = new X509CertificateBC(new MemoryStream(cert.CertificateContent));

                        Assert(X509NamesAreEqual(certificate.SubjectDN, expectedSubject),
                               "Certificate's subject is invalid",
                               "Validating subject");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(keyID))
                                               DeleteRSAKeyPair(keyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certificates - Self-Signed",
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllCertificates })]
        public void GetAllCertificatesSelfSigned()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialCertsList = GetAllCertificates().Select(e => e.CertificateID);

                        UpdateCertificateListBySelfSignedCertificate(initialCertsList, out rsaKeyID, out certID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certID))
                                DeleteCertificate(certID);
                        });

                        AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(rsaKeyID))
                                DeleteRSAKeyPair(rsaKeyID);
                        });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certificates – CA",
            Order = "02.01.09",
            Id = "2-1-9",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllCertificates })]
        public void GetAllCertificatesCA()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialCertsList = GetAllCertificates().Select(e => e.CertificateID);

                        UpdateCertificateListByCASignedCertificate(initialCertsList, out rsaKeyID, out certID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certificate – self signed",
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificate })]
        public void DeleteSelfSignedCertificate()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialCertsList = GetAllCertificates().Select(e => e.CertificateID);

                        UpdateCertificateListBySelfSignedCertificate(initialCertsList, out rsaKeyID, out certID);

                        var local = certID;
                        certID = "";
                        DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        DeleteRSAKeyPair(local);

                        var finalCertsList = GetAllCertificates().Select(e => e.CertificateID);

                        Assert(finalCertsList.All(initialCertsList.Contains) && finalCertsList.Count() == initialCertsList.Count(),
                               "Certificate's list received after deletion of test certificate is different from initial certificate's list",
                               "Check certificate's list received after deletion of test certificate");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certificate – CA",
            Order = "02.01.11",
            Id = "2-1-11",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificate })]
        public void DeleteCertificateCA()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialCertsList = GetAllCertificates().Select(e => e.CertificateID);

                        UpdateCertificateListByCASignedCertificate(initialCertsList, out rsaKeyID, out certID);

                        var local = certID;
                        certID = "";
                        DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        DeleteRSAKeyPair(local);

                        var finalCertsList = GetAllCertificates().Select(e => e.CertificateID);

                        Assert(finalCertsList.All(initialCertsList.Contains) && finalCertsList.Count() == initialCertsList.Count(),
                               "Certificate's list received after deletion of test certificate is different from initial certificate's list",
                               "Check certificate's list received after deletion of test certificate");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create Certification Path – self-signed",
            Order = "02.01.12",
            Id = "2-1-12",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCertificationPath })]
        public void CreateSelfSignedCertificationPath()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var certPathID = CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        DeleteCertificationPath(certPathID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create Certification Path – CA",
            Order = "02.01.13",
            Id = "2-1-13",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCertificationPath })]
        public void CreateCertificationPathCA()
        {
            var caKeyID = "";
            var caCertID = "";
            var certKeyID = "";
            var certID = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certPathID = CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out caKeyID, out caCertID, out certKeyID, out certID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });
                        
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID))
                                               DeleteCertificate(caCertID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID))
                                               DeleteRSAKeyPair(caKeyID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certKeyID))
                                               DeleteRSAKeyPair(certKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certifications Path - Self-Signed",
            Order = "02.01.14",
            Id = "2-1-14",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificationPath })]
        public void GetSelfSignedCertificationPath()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certPathID = CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        var certificationPath = GetCertificationPath(certPathID);

                        var log = new StringBuilder();
                        Assert(ValidateReceivedCertificationPath(certificationPath, new []{ certID }, log),
                               log.ToString(),
                               "Checking received Certification Path");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certification Path - CA",
            Order = "02.01.15",
            Id = "2-1-15",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificationPath })]
        public void GetCertificationPathCA()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var rsaKeyID2 = "";
            var certID2 = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certPathID = CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out rsaKeyID1, out certID1, out rsaKeyID2, out certID2);

                        var certificationPath = GetCertificationPath(certPathID);

                        var log = new StringBuilder();
                        Assert(ValidateReceivedCertificationPath(certificationPath, new []{ certID1, certID2 }, log),
                               log.ToString(),
                               "Checking received Certification Path");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certification Paths – self-signed",
            Order = "02.01.16",
            Id = "2-1-16",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllCertificationPaths })]
        public void GetAllCertificationPathsSelfSigned()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var certPathID1 = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var startCertPathIDs = GetAllCertificationPaths();

                        UpdateCertificationPathList(startCertPathIDs, out rsaKeyID1, out certID1, out certPathID1);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID1))
                                               DeleteCertificationPath(certPathID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certification Paths – CA",
            Order = "02.01.17",
            Id = "2-1-17",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllCertificationPaths })]
        public void GetAllCertificationPathsCA()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var rsaKeyID2 = "";
            var certID2 = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var startCertPathIDs = GetAllCertificationPaths();

                        UpdateCertificationPathListByPathBasedOnCAAndCASignedCertificates(startCertPathIDs, 
                                                                                        out rsaKeyID1, out certID1,
                                                                                        out rsaKeyID2, out certID2,
                                                                                        out certPathID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certifications Path - Self-Signed",
            Order = "02.01.18",
            Id = "2-1-18",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificationPath })]
        public void DeleteSelfSignedCertificationPath()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialCertPathsList = GetAllCertificationPaths();

                        UpdateCertificationPathList(initialCertPathsList, out rsaKeyID, out certID, out certPathID);

                        var local = certPathID;
                        certPathID = "";
                        DeleteCertificationPath(local);
                        local = certID;
                        certID = "";
                        DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        DeleteRSAKeyPair(local);

                        var finalCertsList = GetAllCertificationPaths();

                        Assert(finalCertsList.All(initialCertPathsList.Contains) && finalCertsList.Count() == initialCertPathsList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });
                        
                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certifications Path - CA",
            Order = "02.01.19",
            Id = "2-1-19",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificationPath })]
        public void DeleteCertificationPathCA()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var rsaKeyID2 = "";
            var certID2 = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialCertPathsList = GetAllCertificationPaths();

                        UpdateCertificationPathListByPathBasedOnCAAndCASignedCertificates(initialCertPathsList, 
                                                                                          out rsaKeyID1, out certID1,
                                                                                          out rsaKeyID2, out certID2, 
                                                                                          out certPathID);

                        var local = certPathID;
                        certPathID = "";
                        DeleteCertificationPath(local);
                        
                        local = certID2;
                        certID2 = "";
                        DeleteCertificate(local);
                        local = rsaKeyID2;
                        rsaKeyID2 = "";
                        DeleteRSAKeyPair(local);

                        local = certID1;
                        certID1 = "";
                        DeleteCertificate(local);
                        local = rsaKeyID1;
                        rsaKeyID1 = "";
                        DeleteRSAKeyPair(local);

                        var finalCertsList = GetAllCertificationPaths();

                        Assert(finalCertsList.All(initialCertPathsList.Contains) && finalCertsList.Count() == initialCertPathsList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Add Server Certificate Assignment – self-signed",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.AddServerCertificateAssignment })]
        public void AddServerSelfSignedCertificatateAssignment()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certPathID = CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        AddServerCertificateAssignment(certPathID);

                        RemoveServerCertificateAssignment(certPathID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Add Server Certificate Assignment – CA",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.AddServerCertificateAssignment })]
        public void AddServerCertificatateAssignmentCA()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var rsaKeyID2 = "";
            var certID2 = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certPathID = CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out rsaKeyID1, out certID1, out rsaKeyID2, out certID2);

                        AddServerCertificateAssignment(certPathID);

                        RemoveServerCertificateAssignment(certPathID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Replace Server Certificate Assignment - Self-Signed",
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.ReplaceServerCertificateAssignment })]
        public void ReplaceServerCertificateAssignmentSelfSigned()
        {
            var rsaKeyID = "";
            var certID1 = ""; 
            var certID2 = "";
            var certPathID1 = "";
            var certPathID2 = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certPathID1 = CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID1);

                        AddServerCertificateAssignment(certPathID1);

                        certID2 = CreateSelfSignedCertificate(null, DefaultSubject, rsaKeyID, null, new System.DateTime(), new System.DateTime(), null, null);

                        certPathID2 = CreateCertificationPath(new CertificateIDs() {CertificateID = new[] {certID2}}, "ONVIF_Test");

                        ReplaceServerCertificateAssignment(certPathID1, certPathID2);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID1))
                                               DeleteCertificationPath(certPathID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID2))
                                               DeleteCertificationPath(certPathID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Replace Server Certificate Assignment - CA",
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.ReplaceServerCertificateAssignment })]
        public void ReplaceServerCertificateAssignmentCA()
        {
            var caKeyID = "";
            var testKeyID = "";
            var caCertID = "";
            var certID1 = "";
            var certID2 = "";
            var certPathID1 = "";
            var certPathID2 = "";
            var lastCertificateAssignment = "";
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        RSAKeyPair caKeyPair = null;
                        X509CertificateBase caCert = CreateTestSelfSignedCACertificateA4(out caKeyPair);

                        UploadCertificateWithoutPrivateKeyA15(caCert, out caKeyID, out caCertID);

                        CreateAndUploadCASignedCertificateA16(caCert, caKeyPair, out testKeyID, out certID1);

                        Assert(caKeyID != testKeyID, "The DUT returned the same RSA key pair's IDs for CA certificate and CA-signed certificate", "Verifying recieved RSA key pair's IDs");

                        Assert(caCertID != certID1, "The DUT returned the same certificate's IDs for CA certificate and CA-signed certificate", "Verifying recieved certificate's IDs");

                        certPathID1 = CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { caCertID, certID1 } }, "ONVIF_TestPath1");

                        AddServerCertificateAssignment(certPathID1);
                        lastCertificateAssignment = certPathID1;

                        var cert2 = CreateTestCertificateSignedByCACertificateA14(caCert, caKeyPair, testKeyID);
                        UploadCertificate(cert2.GetEncoded(), "ONVIF_Test2", true, out certID2, testKeyID);

                        Assert(caCertID != certID2 && certID1 != certID2,
                               string.Format("The DUT returned the same certificate's IDs for {0}", caCertID == certID2 ? "CA certificate and CA-signed certificate" : "both CA-signed certificates"), 
                               "Verifying recieved certificate's ID");

                        certPathID2 = CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { caCertID, certID2 } }, "ONVIF_TestPath2");
                        Assert(certPathID2 != certPathID1, "The DUT returned the same certification path's IDs for both test certification paths", "Verifying recieved certification path's IDs");

                        ReplaceServerCertificateAssignment(certPathID1, certPathID2);
                        lastCertificateAssignment = certPathID2;
                    },
                    () =>
                    {

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(lastCertificateAssignment))
                                               RemoveServerCertificateAssignment(lastCertificateAssignment);
                                       }); 
                        
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID1))
                                               DeleteCertificationPath(certPathID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID2) && certPathID2 != certPathID1)
                                               DeleteCertificationPath(certPathID2);
                                       });


                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2) && certID2 != certID1)
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(testKeyID))
                                               DeleteRSAKeyPair(testKeyID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID) && caCertID != certID1 && caCertID != certID2)
                                               DeleteCertificate(caCertID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID) && caKeyID != testKeyID)
                                               DeleteRSAKeyPair(caKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Assigned Server Certificate - Self-Signed",
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAssignedServerCertificates })]
        public void GetAssignedServerCertificateSelfSigned()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialList = GetAssignedServerCertificates();

                        UpdateAssignedServerCertificateListByPathBaseOnSelfSignedCertificate(initialList, out rsaKeyID, out certID, out certPathID, out removeFlag);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               RemoveServerCertificateAssignment(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Assigned Server Certificate - CA",
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAssignedServerCertificates })]
        public void GetAssignedServerCertificateCA()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var rsaKeyID2 = "";
            var certID2 = "";
            var certPathID = "";
            var removeFlag = false;
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialList = GetAssignedServerCertificates();

                        UpdateAssignedCertificateListByPathBasedOnCAAndCASignedCertificatesA18(initialList, out rsaKeyID1, out certID1, out rsaKeyID2, out certID2, out certPathID, out removeFlag);

                        removeFlag = false;
                        RemoveServerCertificateAssignment(certPathID);
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               RemoveServerCertificateAssignment(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Remove Server Certificate Assignment – self-signed",
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveServerCertificateAssignment })]
        public void RemoveSelfSignedServerCertificateAssignment()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialCertPathsList = GetAssignedServerCertificates();

                        UpdateAssignedServerCertificateListByPathBaseOnSelfSignedCertificate(initialCertPathsList, out rsaKeyID, out certID, out certPathID, out removeFlag);

                        removeFlag = false;
                        RemoveServerCertificateAssignment(certPathID);

                        var local = certPathID;
                        certPathID = "";
                        DeleteCertificationPath(local);
                        local = certID;
                        certID = "";
                        DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        DeleteRSAKeyPair(local);

                        var finalCertsList = GetAssignedServerCertificates();

                        Assert(finalCertsList.All(initialCertPathsList.Contains) && finalCertsList.Count() == initialCertPathsList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        if (removeFlag)
                            AllowFaultStep(() => RemoveServerCertificateAssignment(certPathID));

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Remove Server Certificate Assignment - CA",
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveServerCertificateAssignment })]
        public void RemoveServerCertificateAssignmentCA()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var rsaKeyID2 = "";
            var certID2 = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        var initialList = GetAssignedServerCertificates();

                        UpdateAssignedCertificateListByPathBasedOnCAAndCASignedCertificatesA18(initialList, out rsaKeyID1, out certID1, out rsaKeyID2, out certID2, out certPathID, out removeFlag);

                        removeFlag = false;
                        RemoveServerCertificateAssignment(certPathID);

                        var local = certPathID;
                        certPathID = "";
                        DeleteCertificationPath(local);
                        
                        local = certID1;
                        certID1 = "";
                        DeleteCertificate(local);
                        local = rsaKeyID1;
                        rsaKeyID1 = "";
                        DeleteRSAKeyPair(local);

                        local = certID2;
                        certID2 = "";
                        DeleteCertificate(local);
                        local = rsaKeyID2;
                        rsaKeyID2 = "";
                        DeleteRSAKeyPair(local);

                        var finalCertsList = GetAssignedServerCertificates();

                        Assert(finalCertsList.All(initialList.Contains) && finalCertsList.Count() == initialList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               RemoveServerCertificateAssignment(certPathID);
                                       }); 
                        
                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               DeleteCertificate(certID2);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "TLS Server Certificate - Self-Signed",
            Order = "04.01.01",
            Id = "4-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_REFERENTIAL_INTEGRITY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteKey, Functionality.DeleteCertificate, Functionality.DeleteCertificationPath })]
        public void TLSServerCertificateSelfSigned()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        AddSelfSignedCertificateAsAssignedServerCertificateA13(out rsaKeyID1, out certID1, out certPathID, out removeFlag);

                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(rsaKeyID1); rsaKeyID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificate(certID1); certID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificationPath(certPathID); certPathID = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        removeFlag = false;
                        RemoveServerCertificateAssignment(certPathID);

                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(rsaKeyID1); rsaKeyID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificate(certID1); certID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        var local = certPathID;
                        certPathID = "";
                        DeleteCertificationPath(local);

                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(rsaKeyID1); rsaKeyID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               RemoveServerCertificateAssignment(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               DeleteCertificate(certID1);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "TLS Server Certificate - CA",
            Order = "04.01.02",
            Id = "4-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_REFERENTIAL_INTEGRITY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteKey, Functionality.DeleteCertificate, Functionality.DeleteCertificationPath })]
        public void TLSServerCertificateCA()
        {
            var caKeyID = "";
            var caCertID = "";
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        GeneralPrerequisites();

                        certPathID = CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out caKeyID, out caCertID, out rsaKeyID, out certID);
                        
                        AddServerCertificateAssignment(certPathID);
                        removeFlag = true;

                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(caKeyID); caKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(rsaKeyID); rsaKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificate(caCertID); caCertID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificate(certID); certID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificationPath(certPathID); certPathID = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        removeFlag = false;
                        RemoveServerCertificateAssignment(certPathID);

                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(caKeyID); caKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(rsaKeyID); rsaKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificate(caCertID); caCertID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteCertificate(certID); certID = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        var local = certPathID;
                        certPathID = "";
                        DeleteCertificationPath(local);

                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(caKeyID); caKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        DoActionWithSOAPFault(() => { DeleteRSAKeyPair(rsaKeyID); rsaKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                    },
                    () =>
                    {
                        AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               RemoveServerCertificateAssignment(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               DeleteCertificationPath(certPathID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               DeleteCertificate(certID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               DeleteRSAKeyPair(rsaKeyID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID))
                                               DeleteCertificate(caCertID);
                                       });

                        AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID))
                                               DeleteRSAKeyPair(caKeyID);
                                       });

                        FinishRestoreSettings();
                    });
        }

        [Test(Name = "Advanced Security Service Capabilities",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CAPABILITY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAdvancedSecurityServiceCapabilities })]
        public void AdvancedSecurityCapabilities()
        {
            RunTest(() =>
            {
                Assert(null != Client,
                    "Can't connect to Advance Security Service",
                    "Check that Advance Security Service is accessible");

                var capabilities = GetCapabilities();

                // 5. MaximumNumberOfCertificates
                if (!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates))
                {
                    int MaximumNumberOfCertificates;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates, out MaximumNumberOfCertificates);
                    Assert(result, "MaximumNumberOfCertificates value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificates value");

                    if (MaximumNumberOfCertificates > 0)
                    {
                        // 5.1. If cap.KeystoreCapabilities.MaximumNumberOfKeys <= 0 or skipped, FAIL the test.
                        Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfKeys),
                            "MaximumNumberOfKeys attribute skipped when MaximumNumberOfCertificates > 0",
                            "Check for existing of MaximumNumberOfKeys attribute");

                        int MaximumNumberOfKeys;
                        result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfKeys, out MaximumNumberOfKeys);
                        Assert(result, "MaximumNumberOfKeys value could not be parsed as integer",
                            "Validating format of MaximumNumberOfKeys value");

                        Assert(MaximumNumberOfKeys > 0,
                            "Wrong value: MaximumNumberOfKeys <= 0 when MaximumNumberOfCertificates > 0",
                            "Validating of MaximumNumberOfKeys value");
                    }
                }

                // 6. MaximumNumberOfCertificationPaths
                if (!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths))
                {
                    int MaximumNumberOfCertificationPaths;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths, out MaximumNumberOfCertificationPaths);
                    Assert(result, "MaximumNumberOfCertificationPaths value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificationPaths value");

                    if (MaximumNumberOfCertificationPaths > 0)
                    {
                        // 6.1. If cap.KeystoreCapabilities.MaximumNumberOfCertificates < 2 or skipped, FAIL the test.
                        Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates),
                            "MaximumNumberOfCertificates attribute skipped when MaximumNumberOfCertificationPaths > 0",
                            "Check for existing of MaximumNumberOfCertificates attribute");

                        int MaximumNumberOfCertificates;
                        result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates, out MaximumNumberOfCertificates);
                        Assert(result, "MaximumNumberOfCertificates value could not be parsed as integer",
                            "Validating format of MaximumNumberOfCertificates value");

                        Assert(MaximumNumberOfCertificates >= 2,
                            "Wrong value: MaximumNumberOfCertificates < 2 when MaximumNumberOfCertificationPaths > 0",
                            "Validating of MaximumNumberOfCertificates value");
                    }
                }

                // 7. RSAKeyPairGeneration
                if (capabilities.KeystoreCapabilities.RSAKeyPairGeneration &&
                    capabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified)
                {
                    //7.1. If cap.KeystoreCapabilities.RSAKeyLenghts is empty or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.RSAKeyLengths != null && 
                        capabilities.KeystoreCapabilities.RSAKeyLengths.Length != 0,
                        "Wrong value: RSAKeyLenghts is empty or skipped when RSAKeyPairGeneration = true",
                        "Validating of RSAKeyLenghts value");

                    //7.2. If cap.KeystoreCapabilities.MaximumNumberOfKeys <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfKeys),
                         "MaximumNumberOfKeys attribute skipped when RSAKeyPairGeneration = true",
                         "Check for existing of MaximumNumberOfKeys attribute");

                    int MaximumNumberOfKeys;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfKeys, out MaximumNumberOfKeys);
                    Assert(result, "MaximumNumberOfKeys value could not be parsed as integer",
                        "Validating format of MaximumNumberOfKeys value");

                    Assert(MaximumNumberOfKeys > 0,
                        "Wrong value: MaximumNumberOfKeys <= 0 when RSAKeyPairGeneration = true",
                        "Validating of MaximumNumberOfKeys value");
                }

                // 8. PKCS10ExternalCertificationWithRSA
                if (capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA &&
                    capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified)
                {
                    //8.1. If cap.KeystoreCapabilities.RSAKeyPairGeneration = false or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.RSAKeyPairGeneration &&
                        capabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified,
                        "Wrong value: RSAKeyPairGeneration is false or skipped when PKCS10ExternalCertificationWithRSA = true",
                        "Validating of RSAKeyPairGeneration value");

                    //8.2. If cap.KeystoreCapabilities.SignatureAlgorithms list is empty, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Length != 0,
                        "Wrong value: SignatureAlgorithms is empty or skipped when PKCS10ExternalCertificationWithRSA = true",
                        "Validating of SignatureAlgorithms value");

                    //8.3. If cap.KeystoreCapabilities.MaximumNumberOfKeys < 2 or skipped, FAIL the test.
                     Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfKeys),
                            "MaximumNumberOfKeys attribute skipped when PKCS10ExternalCertificationWithRSA = true",
                            "Check for existing of MaximumNumberOfKeys attribute");

                    int MaximumNumberOfKeys;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfKeys, out MaximumNumberOfKeys);
                    Assert(result, "MaximumNumberOfKeys value could not be parsed as integer",
                        "Validating format of MaximumNumberOfKeys value");

                    Assert(MaximumNumberOfKeys >= 2,
                        "Wrong value: MaximumNumberOfKeys < 2 when PKCS10ExternalCertificationWithRSA = true",
                        "Validating of MaximumNumberOfKeys value");

                    //8.4. If cap.KeystoreCapabilities.MaximumNumberOfCertificates < 2 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates),
                            "MaximumNumberOfCertificates attribute skipped when PKCS10ExternalCertificationWithRSA = true",
                            "Check for existing of MaximumNumberOfCertificates attribute");

                    int MaximumNumberOfCertificates;
                    result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates, out MaximumNumberOfCertificates);
                    Assert(result, "MaximumNumberOfCertificates value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificates value");

                    Assert(MaximumNumberOfCertificates >= 2,
                        "Wrong value: MaximumNumberOfCertificates < 2 when PKCS10ExternalCertificationWithRSA = true",
                        "Validating of MaximumNumberOfCertificates value");

                     //8.5. If cap.KeystoreCapabilities.MaximumNumberOfCertificationPaths <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths),
                        "MaximumNumberOfCertificationPaths attribute skipped when PKCS10ExternalCertificationWithRSA = true",
                        "Check for existing of MaximumNumberOfCertificationPaths attribute");

                    int MaximumNumberOfCertificationPaths;
                    result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths, out MaximumNumberOfCertificationPaths);
                    Assert(result, "MaximumNumberOfCertificationPaths value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificationPaths value");

                    Assert(MaximumNumberOfCertificationPaths > 0,
                        "Wrong value: MaximumNumberOfCertificationPaths <= 0 when PKCS10ExternalCertificationWithRSA = true",
                        "Validating of MaximumNumberOfCertificationPaths value");
                }

                // 9. SelfSignedCertificateCreationWithRSA
                if (capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA &&
                    capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified)
                {
                    //9.1. If cap.KeystoreCapabilities.RSAKeyPairGeneration = false or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.RSAKeyPairGeneration &&
                        capabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified,
                        "Wrong value: RSAKeyPairGeneration is false or skipped when SelfSignedCertificateCreationWithRSA = true",
                        "Validating of RSAKeyPairGeneration value");

                    //9.2. If cap.KeystoreCapabilities.MaximumNumberOfCertificates <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates),
                        "MaximumNumberOfCertificates attribute skipped when SelfSignedCertificateCreationWithRSA = true",
                        "Check for existing of MaximumNumberOfCertificates attribute");

                    int MaximumNumberOfCertificates;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates, out MaximumNumberOfCertificates);
                    Assert(result, "MaximumNumberOfCertificates value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificates value");

                    Assert(MaximumNumberOfCertificates > 0,
                        "Wrong value: MaximumNumberOfCertificates <= 0 when SelfSignedCertificateCreationWithRSA = true",
                        "Validating of MaximumNumberOfCertificates value");

                    //9.3. If cap.KeystoreCapabilities.SignatureAlgorithms list is empty, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Length != 0,
                        "Wrong value: SignatureAlgorithms is empty or skipped when SelfSignedCertificateCreationWithRSA = true",
                        "Validating of SignatureAlgorithms value");
                }

                // 10. TLSServerSupported
                if (capabilities.TLSServerCapabilities.TLSServerSupported != null &&
                    capabilities.TLSServerCapabilities.TLSServerSupported.Length != 0)
                {
                    //10.1. If cap.TLSServerCapabilities.TLSServerSupported does not contain the value 1.0, FAIL the test.
                    string tlsSupported = capabilities.TLSServerCapabilities.TLSServerSupported.FirstOrDefault(tls => tls == "1.0");
                    Assert(tlsSupported != null,
                        "Wrong value: TLSServerSupported doesn't contain value \"1.0\" when TLSServerSupported is not empty",
                        "Validating of TLSServerSupported value");

                    //10.2. If cap.KeystoreCapabilities.MaximumNumberOfCertificationPaths < 2 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths),
                        "MaximumNumberOfCertificationPaths attribute skipped when TLSServerSupported is not empty",
                        "Check for existing of MaximumNumberOfCertificationPaths attribute");

                    int MaximumNumberOfCertificationPaths;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths, out MaximumNumberOfCertificationPaths);
                    Assert(result, "MaximumNumberOfCertificationPaths value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificationPaths value");

                    Assert(MaximumNumberOfCertificationPaths >= 2,
                        "Wrong value: MaximumNumberOfCertificationPaths < 2 when TLSServerSupported is not empty",
                        "Validating of MaximumNumberOfCertificationPaths value");

                    //10.3. If cap.KeystoreCapabilities.MaximumNumberOfTLSCertificationPaths <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths),
                        "MaximumNumberOfTLSCertificationPaths attribute skipped when TLSServerSupported is not empty",
                        "Check for existing of MaximumNumberOfTLSCertificationPaths attribute");

                    int MaximumNumberOfTLSCertificationPaths;
                    result = Int32.TryParse(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths, out MaximumNumberOfTLSCertificationPaths);
                    Assert(result, "MaximumNumberOfTLSCertificationPaths value could not be parsed as integer",
                        "Validating format of MaximumNumberOfTLSCertificationPaths value");

                    Assert(MaximumNumberOfTLSCertificationPaths > 0,
                        "Wrong value: MaximumNumberOfTLSCertificationPaths <= 0 when TLSServerSupported is not empty",
                        "Validating of MaximumNumberOfTLSCertificationPaths value");
                }

                // 11. TLSServerSupported and PKCS10ExternalCertificationWithRSA
                if (capabilities.TLSServerCapabilities.TLSServerSupported  != null &&
                    capabilities.TLSServerCapabilities.TLSServerSupported.Length != 0 &&
                    capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA &&
                    capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified)
                {
                    //11.1. If cap.KeystoreCapabilities.MaximumNumberOfCertificates < 3 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates),
                        "MaximumNumberOfCertificates attribute skipped when TLSServerSupported is not empty and PKCS10ExternalCertificationWithRSA = true",
                        "Check for existing of MaximumNumberOfCertificates attribute");

                    int MaximumNumberOfCertificates;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates, out MaximumNumberOfCertificates);
                    Assert(result, "MaximumNumberOfCertificates value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificates value");

                    Assert(MaximumNumberOfCertificates >= 3,
                        "Wrong value: MaximumNumberOfCertificates < 3 when TLSServerSupported is not empty and PKCS10ExternalCertificationWithRSA = true",
                        "Validating of MaximumNumberOfCertificates value");
                }

                // 12. MaximumNumberOfTLSCertificationPaths
                if (!string.IsNullOrEmpty(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths))
                {
                    int MaximumNumberOfTLSCertificationPaths;
                    bool result = Int32.TryParse(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths, out MaximumNumberOfTLSCertificationPaths);
                    Assert(result, "MaximumNumberOfTLSCertificationPaths value could not be parsed as integer",
                        "Validating format of MaximumNumberOfTLSCertificationPaths value");

                    Assert(MaximumNumberOfTLSCertificationPaths > 0,
                        "Wrong value: MaximumNumberOfTLSCertificationPaths <= 0 when TLSServerSupported is not empty",
                        "Validating of MaximumNumberOfTLSCertificationPaths value");

                    if (MaximumNumberOfTLSCertificationPaths > 0)
                    {
                        //12.1. If cap.KeystoreCapabilities.MaximumNumberOfCertificationPaths <= 0 or skipped, FAIL the test.
                        Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths),
                            "MaximumNumberOfCertificationPaths attribute skipped when MaximumNumberOfTLSCertificationPaths > 0",
                            "Check for existing of MaximumNumberOfCertificationPaths attribute");

                        int MaximumNumberOfCertificationPaths;
                        result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths, out MaximumNumberOfCertificationPaths);
                        Assert(result, "MaximumNumberOfCertificationPaths value could not be parsed as integer",
                            "Validating format of MaximumNumberOfCertificationPaths value");

                        Assert(MaximumNumberOfCertificationPaths > 0,
                            "Wrong value: MaximumNumberOfCertificationPaths <= 0 when MaximumNumberOfTLSCertificationPaths > 0",
                            "Validating of MaximumNumberOfCertificationPaths value");
                    }
                }

            },
            () =>
            {

            });
        }

        [Test(Name = "Get Services and Get Advanced Security Service Capabilities Consistency",
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CAPABILITY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetAdvancedSecurityServiceCapabilities })]
        public void CapabilitiesConsistency()
        {
            RunTest(() =>
            {
                Assert(null != Client,
                    "Can't connect to Advance Security Service",
                    "Check that Advance Security Service is accessible");

                // GetServices
                Service[] services =  GetServices(true);

                Service advancedSecurityService = services.FindService(OnvifService.ADVANCED_SECURITY);
                Assert(advancedSecurityService != null, 
                    "No Advanced Security service information returned",
                    "Check that the DUT returned Advanced Security service information");

                string version = advancedSecurityService.Version == null  
                    ? "missing"
                    : string.Format("{0}.{1}", advancedSecurityService.Version.Major, advancedSecurityService.Version.Minor);

                Assert(advancedSecurityService.Capabilities != null,
                    string.Format("Capabilities are not included in entry for Advanced Security service version {0}", version),
                    "Check that the DUT returned Capabilities element");

                AdvancedSecurityCapabilities advancedSecurityCapabilities = 
                    ExtractAdvancedSecurityCapabilities(advancedSecurityService.Capabilities);

                // GetServiceCapabilities
                var capabilities = GetCapabilities();

                CompareCapabilities(advancedSecurityCapabilities, capabilities);
            },
            () =>
            {

            });
        }
    }
}
