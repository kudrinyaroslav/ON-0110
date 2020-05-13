///////////////////////////////////////////////////////////////////////////
//!  @author        Alexei Soloview
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Xml;
using TestTool.Crypto;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using DateTime = System.DateTime;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using TestTool.Tests.Common.TestBase;


namespace TestTool.Tests.TestCases.TestSuites.AdvancedSecurity
{
    [TestClass]
    partial class AdvancedSecurityTestSuit : BaseOnvifTest, IAdvancedSecurityService, IEventService, IDeviceService
    {
        private const string PATH_GENERAL = "Advanced Security";
        private const string PATH_KEYSTORE = PATH_GENERAL + @"\Keystore";
        private const string PATH_CERTIFICATE_MANAGEMENT = PATH_GENERAL + @"\Certificate Management";
        private const string PATH_TLS_SERVER = PATH_GENERAL + @"\TLS Server";
        private const string    PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT = PATH_TLS_SERVER + @"\TLS Certificate Management";
        private const string    PATH_TLS_SERVER_HANDSHAKING = PATH_TLS_SERVER + @"\TLS Handshaking";
        private const string    PATH_TLS_CLIENT_AUTHENTICATION = PATH_TLS_SERVER + @"\TLS Client Authentication";
        private const string PATH_REFERENTIAL_INTEGRITY = PATH_GENERAL + @"\Referential Integrity";
        private const string PATH_CAPABILITY = PATH_GENERAL + @"\Capabilities";
        private const string PATH_PASSPHRASE_MANAGEMENT = PATH_GENERAL + @"\Passphrase Management";
        private const string PATH_PKCS8 = PATH_GENERAL + @"\PKCS#8";
        private const string PATH_PKCS12 = PATH_GENERAL + @"\PKCS#12";
        private const string PATH_IEEE_8021X_CONFIGURATION = PATH_GENERAL + @"\IEEE 802.1X Configuration";
        private const string PATH_CERTIFICATE_BASED_CLIENT_AUTHENTICATION = PATH_GENERAL + @"\Certificate-based Client Authentication";

        public AdvancedSecurityTestSuit(TestLaunchParam param) : base(param) { }

        #region Branch 1-*
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
                        var capabilities = (this as IAdvancedSecurityService).GetServiceCapabilities();

                        Assert(null != capabilities.KeystoreCapabilities && null != capabilities.KeystoreCapabilities.RSAKeyLengths && capabilities.KeystoreCapabilities.RSAKeyLengths.Any(),
                               "The set of supported RSA key's length is empty",
                               "Check the set of supported RSA key's length isn't empty");

                        foreach (var keyLength in this.SelectKeyLengthsForTest(capabilities.KeystoreCapabilities.RSAKeyLengths))
                        {
                            this.CreateTestRSAKeyPairOfSpecifiedLength(out keyID, keyLength);

                            var local = keyID;
                            keyID = "";
                            this.DeleteRSAKeyPair(local);
                        }
                    },
                    () =>
                    {
                        if (!string.IsNullOrEmpty(keyID)) this.DeleteRSAKeyPair(keyID);

                        this.FinishRestoreSettings();
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
                        var capabilities = (this as IAdvancedSecurityService).GetServiceCapabilities();

                        Assert(null != capabilities.KeystoreCapabilities && null != capabilities.KeystoreCapabilities.RSAKeyLengths && capabilities.KeystoreCapabilities.RSAKeyLengths.Any(),
                               "The set of supported RSA key's length is empty",
                               "Check the set of supported RSA key's length isn't empty");

                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        const string eventTopic = "tns1:Advancedsecurity/Keystore/KeyStatus";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var timeout = _operationDelay / 1000;

                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);
                        subscription.Subscribe(filter, timeout);

                        foreach (var keyLength in this.SelectKeyLengthsForTest(capabilities.KeystoreCapabilities.RSAKeyLengths))
                        {
                            string estimatedCreationTime = null;
                            var keyID = this.CreateRSAKeyPair(keyLength.ToString(), null, out estimatedCreationTime);

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

                                Assert(AdvancedSecurityExtensions.ksOK == pollingCondition.KeyStatus,
                                       string.Format("Key status is other than '{0}'", AdvancedSecurityExtensions.ksOK),
                                       string.Format("Check key status of key pair is '{0}'", AdvancedSecurityExtensions.ksOK));
                            }
                            finally
                            {
                                this.DeleteRSAKeyPair(keyID);
                            }
                        }
                    },
                    () =>
                    {
                        if (null != subscription)
                            SubscriptionHandler.Unsubscribe(subscription);

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Check private key status for an RSA private key",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_KEYSTORE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllKeys })]
        public void CheckPrivateKeyStatusForRSAPrivateKey()
        {
            var rsaKeyID = "";

            RunTest(() =>
                    {
                        this.CreateTestRSAKeyPairA7(out rsaKeyID);

                        var keys = this.GetAllKeys();
                        var keyFromDevice = keys.FirstOrDefault(c => c.KeyID == rsaKeyID);

                        Assert(null != keyFromDevice,
                               string.Format("The results received via GetAllKeys are inconsistent with CreateRSAKeyPair: GetAllKeys returned no key with KeyID = '{0}'", rsaKeyID),
                               string.Format("Check that key pair was received (KeyID = {0})", rsaKeyID));

                        Assert((keyFromDevice.hasPrivateKeySpecified) && (keyFromDevice.hasPrivateKey),
                               "Private key status of created key pair is 'false'.",
                               string.Format("Check private key status of created key pair is 'true' (KeyID = {0})", rsaKeyID));

                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
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
                        var allKeysBefore = this.GetAllKeys().Select(e => e.KeyID);

                        this.UpdateRSAKeyList(allKeysBefore, out newKey);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(newKey))
                                this.DeleteRSAKeyPair(newKey);
                        });

                        this.FinishRestoreSettings();
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
                        var allKeysBefore = this.GetAllKeys().Select(e => e.KeyID);

                        this.UpdateRSAKeyList(allKeysBefore, out newKey);

                        this.DeleteRSAKeyPair(newKey);
                        newKey = "";

                        var allKeysFinal = this.GetAllKeys().Select(e => e.KeyID);

                        Assert(allKeysBefore.All(allKeysFinal.Contains) && allKeysFinal.Count() == allKeysBefore.Count(),
                               "Key list received before new key's creation isn't the same as after removing created key",
                               "Check that key list received before new key's creation is the same as after removing created key");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(newKey))
                                               this.DeleteRSAKeyPair(newKey);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        #endregion

        #region Branch 2-*
        [Test(Name = "Create PKCS#10",
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePKCS10CSR })]
        public void CreatePKCS10()
        {
            var rsaKeyID = "";
            RunTest(() =>
                    {
                        this.CreateTestRSAKeyPairA7(out rsaKeyID);

                        this.ValidateCertificateSigningRequest(this.CreatePKCS10CSR(DefaultSubject, rsaKeyID), this.DefaultSubjectString());
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create PKCS#10 (negative test)",
            Id = "2-1-20",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePKCS10CSR })]
        public void CreatePKCS10NegativeTest()
        {
            var rsaKeyID = "";
            RunTest(() =>
                    {
                        var capabilities = (this as IAdvancedSecurityService).GetServiceCapabilities();

                        Assert(null != capabilities.KeystoreCapabilities && null != capabilities.KeystoreCapabilities.RSAKeyLengths && capabilities.KeystoreCapabilities.RSAKeyLengths.Any(),
                               "The set of supported RSA key's length is empty",
                               "Check the set of supported RSA key's length isn't empty");

                        var keyLengths = capabilities.KeystoreCapabilities.RSAKeyLengths.Where(e => e >= 1024).OrderBy(e => e).ToArray();
                        if (!keyLengths.Any())
                            keyLengths = new[] { capabilities.KeystoreCapabilities.RSAKeyLengths.Max() };

                        var performedFlag = false;
                        foreach (var keyLength in keyLengths)
                        {
                            var duration = new TimeSpan();
                            this.CreateRSAKeyPair(keyLength.ToString(), null, out duration, out rsaKeyID);

                            if (duration.TotalSeconds < 2)
                                LogStepEvent(string.Format("Estimated Creation time for RSA keypair of size {0} is less than 2 seconds. Skip this key length.", keyLength));
                            else
                            {
                                try
                                {
                                    this.CreatePKCS10CSR(DefaultSubject, rsaKeyID);
                                }
                                catch (FaultException e)
                                {
                                    performedFlag = true;
                                    if (e.IsValidOnvifFault("Sender/InvalidArgVal/InvalidKeyStatus"))
                                        StepPassed();
                                    else
                                        StepFailed(e);
                                }
                            }

                            var local = rsaKeyID;
                            rsaKeyID = "";
                            this.DeleteRSAKeyPair(local);
                            if (performedFlag)
                                break;
                        }

                        if (!performedFlag)
                            LogStepEvent("WARNING: there is no more supported key length. Pass the test.");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create Self-Signed Certificate",
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateSelfSignedCertificate })]
        public void CreateSelfSignedCertificateTest()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        certID = this.CreateTestSelfSignedCertificateA8(out rsaKeyID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – Keystore contains private key",
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateKeystoreContainsPrivateKey()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        RSAKeyPair caKeyPair = null;
                        this.CreateTestRSAKeyPairA7(out rsaKeyID);

                        var certificate = this.CreateTestCertificateSignedByCACertificateA14(this.CreateTestSelfSignedCACertificateA4(out caKeyPair), caKeyPair, rsaKeyID);

                        this.UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", null, true, out certID, rsaKeyID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – Keystore contains private key (negative test)",
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateKeystoreContainsPrivateKeyNegativeTest()
        {
            var certID = "";
            RunTest(() =>
                    {
                        try
                        {
                            var certificate = this.CreateTestSelfSignedCACertificateA4();
                            this.UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", null, true, out certID, null, true, false);

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
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – delete linked key (negative test)",
            Order = "02.01.22",
            Id = "2-1-22",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateDeleteLinkedKeyNegativeTest()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        try
                        {
                            RSAKeyPair caKeyPair = null;
                            this.CreateTestRSAKeyPairA7(out rsaKeyID);

                            var certificate = this.CreateTestCertificateSignedByCACertificateA14(this.CreateTestSelfSignedCACertificateA4(out caKeyPair), caKeyPair, rsaKeyID);

                            this.UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", null, true, out certID, rsaKeyID);

                            this.DeleteRSAKeyPair(rsaKeyID);

                            Assert(false, "The DUT did not send the env:Sender/ter:InvalidArgVal/ter:ReferenceExists SOAP 1.2 fault message.", "Fail the test");
                        }
                        catch (FaultException e)
                        {
                            if (e.IsValidOnvifFault("Sender/InvalidArgVal/ReferenceExists"))
                                StepPassed();
                            else
                            {
                                LogStepEvent("MESSAGE: The DUT did not send the env:Sender/ter:InvalidArgVal/ter:ReferenceExists SOAP 1.2 fault message.");
                                throw;
                            }
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – Keystore does not contain private key",
            Order = "02.01.05",
            Id = "2-1-5",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateKeystoreDoesNotContainsPrivateKey()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        var certificate = this.CreateTestSelfSignedCACertificateA4();
                        this.UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", null, false, out rsaKeyID, out certID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload certificate – Upload malformed certificate (negative test)",
            Id = "2-1-23",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateMalformedCertificateNegativeTest()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        try
                        {
                            var certificate = this.CreateTestSelfSignedCACertificateA4();

                            var certificateBinary = certificate.GetEncoded();

                            for (int i = 0; i < certificateBinary.Count() && this.ValidateDEREncoding(new MemoryStream(certificateBinary)); ++i)
                                certificateBinary[i] = 0xFF;

                            this.UploadCertificate(certificateBinary, "ONVIF_Test", null, false, out rsaKeyID, out certID);

                            Assert(false, "The DUT did not send the env:Sender/ter:InvalidArgVal/ter:BadCertificate SOAP 1.2 fault message.", "Fail the test");
                        }
                        catch (FaultException e)
                        {
                            if (e.IsValidOnvifFault("Sender/InvalidArgVal/BadCertificate"))
                                StepPassed();
                            else
                            {
                                LogStepEvent("MESSAGE: The DUT did not send the env:Sender/ter:InvalidArgVal/ter:BadCertificate SOAP 1.2 fault message.");
                                throw;
                            }
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }


        [Test(Name = "Upload certificate – Upload expired certificate",
            Id = "2-1-24",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificate })]
        public void UploadCertificateExpiredCertificate()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        try
                        {
                            var expiredCertificate = this.CreateExpiredCertificateA22();

                            this.UploadCertificate(expiredCertificate.GetEncoded(), "ONVIF_Test", null, false, out rsaKeyID, out certID);
                        }
                        catch (FaultException e)
                        {
                            if (e.IsValidOnvifFault("Sender/InvalidArgVal/BadCertificate"))
                                throw;

                            LogStepEvent("MESSAGE: The DUT sent the SOAP 1.2 fault message different from env:Sender/ter:InvalidArgVal/ter:BadCertificate.");

                            StepPassed();
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certificate - Self-Signed",
            Order = "02.01.06",
            Id = "2-1-6",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificate })]
        public void GetCertificateSelfSigned()
        {
            var keyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        certID = this.CreateTestSelfSignedCertificateA8(out keyID);

                        var cert = this.GetCertificate(certID);

                        Assert(this.ValidateDEREncoding(new MemoryStream(cert.CertificateContent)),
                               "X509Cert is wrongly DER encoded",
                               "Check certificate is correctly DER encoded");

                        //var expectedSubject = new X509Name(new List<DerObjectIdentifier>() { X509Name.CN, X509Name.C },
                        //                                   new List<string>() { _cameraIp.ToString(), "US" });

                        var expectedSubject = string.Format("CN={0},C=US", _cameraIp);

                        var certificate = new X509CertificateBC(new MemoryStream(cert.CertificateContent));

                        Assert(this.X509NamesAreEqual(certificate.SubjectDN, expectedSubject),
                               "Certificate's subject is invalid",
                               "Validating subject");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certID))
                                this.DeleteCertificate(certID);
                        });

                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(keyID))
                                this.DeleteRSAKeyPair(keyID);
                        });

                        this.FinishRestoreSettings();
                    });
        }

        //[Test(Name = "Create PKCS#10 – Subject Test",
        //    Id = "2-1-25",
        //    Category = Category.ADVANCED_SECURITY,
        //    Path = PATH_CERTIFICATE_MANAGEMENT,
        //    Version = 1.0,
        //    RequirementLevel = RequirementLevel.Optional,
        //    LastChangedIn = "v15.06",
        //    RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.RSAKeyPairGeneration },
        //    FunctionalityUnderTest = new Functionality[] { Functionality.CreatePKCS10CSR })]
        public void CreatePKCS10SubjectTestTest()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        this.CreateTestRSAKeyPairA7(out rsaKeyID);

                        var subject = this.CertificateSubjectA34();

                        var requestBinary = this.CreatePKCS10CSR(subject, rsaKeyID);

                        this.ValidateCertificateSigningRequest(requestBinary, this.DistinguishedNameToString(subject));
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        //[Test(Name = "Create self-signed certificate – Subject Test",
        //    Id = "2-1-26",
        //    Category = Category.ADVANCED_SECURITY,
        //    Path = PATH_CERTIFICATE_MANAGEMENT,
        //    Version = 1.0,
        //    RequirementLevel = RequirementLevel.Optional,
        //    RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration },
        //    LastChangedIn = "v15.06",
        //    FunctionalityUnderTest = new Functionality[] { Functionality.CreateSelfSignedCertificate })]
        public void CreateSelfSignedCertificateSubjectTestTest()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        this.CreateTestRSAKeyPairA7(out rsaKeyID);

                        var subject = this.CertificateSubjectA34();

                        certID = this.CreateSelfSignedCertificate(null, subject, rsaKeyID, null, new DateTime(), new DateTime());

                        var certificateBinary = this.GetCertificate(certID);

                        Assert(this.ValidateDEREncoding(new MemoryStream(certificateBinary.CertificateContent)),
                               "X509Cert is wrongly DER encoded",
                               "Check certificate is correctly DER encoded");

                        var certificate = new X509CertificateBC(new MemoryStream(certificateBinary.CertificateContent));

                        Assert(this.X509NamesAreEqual(certificate.SubjectDN, this.DistinguishedNameToString(subject)),
                               "Subject of received recertificate is not as expected",
                               "Validating subject");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certificate - CA",
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificate })]
        public void GetCertificateCA()
        {
            var keyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        this.UploadCertificateWithoutPrivateKeyA15(this.CreateTestSelfSignedCACertificateA4(), out keyID, out certID);

                        var cert = this.GetCertificate(certID);

                        Assert(this.ValidateDEREncoding(new MemoryStream(cert.CertificateContent)),
                               "X509Cert is wrongly DER encoded",
                               "Check certificate is correctly DER encoded");

                        //var expectedSubject = string.Format("CN={0},C=US", _cameraIp);

                        var certificate = new X509CertificateBC(new MemoryStream(cert.CertificateContent));

                        Assert(this.X509NamesAreEqual(certificate.SubjectDN, "CN=ONVIF TT,C=US"),
                               "Certificate's subject is invalid",
                               "Validating subject");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(keyID))
                                               this.DeleteRSAKeyPair(keyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certificates - Self-Signed",
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllCertificates })]
        public void GetAllCertificatesSelfSigned()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        var initialCertsList = this.GetAllCertificates().Select(e => e.CertificateID);

                        this.UpdateCertificateListBySelfSignedCertificate(initialCertsList, out rsaKeyID, out certID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certID))
                                this.DeleteCertificate(certID);
                        });

                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(rsaKeyID))
                                this.DeleteRSAKeyPair(rsaKeyID);
                        });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certificates – CA",
            Order = "02.01.09",
            Id = "2-1-9",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllCertificates })]
        public void GetAllCertificatesCA()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        var initialCertsList = this.GetAllCertificates().Select(e => e.CertificateID);

                        this.UpdateCertificateListByCASignedCertificate(initialCertsList, out rsaKeyID, out certID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certificate – self signed",
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificate })]
        public void DeleteSelfSignedCertificate()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        var initialCertsList = this.GetAllCertificates().Select(e => e.CertificateID);

                        this.UpdateCertificateListBySelfSignedCertificate(initialCertsList, out rsaKeyID, out certID);

                        var local = certID;
                        certID = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        this.DeleteRSAKeyPair(local);

                        var finalCertsList = this.GetAllCertificates().Select(e => e.CertificateID);

                        Assert(finalCertsList.All(initialCertsList.Contains) && finalCertsList.Count() == initialCertsList.Count(),
                               "Certificate's list received after deletion of test certificate is different from initial certificate's list",
                               "Check certificate's list received after deletion of test certificate");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certificate – CA",
            Order = "02.01.11",
            Id = "2-1-11",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificate })]
        public void DeleteCertificateCA()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        var initialCertsList = this.GetAllCertificates().Select(e => e.CertificateID);

                        this.UpdateCertificateListByCASignedCertificate(initialCertsList, out rsaKeyID, out certID);

                        var local = certID;
                        certID = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        this.DeleteRSAKeyPair(local);

                        var finalCertsList = this.GetAllCertificates().Select(e => e.CertificateID);

                        Assert(finalCertsList.All(initialCertsList.Contains) && finalCertsList.Count() == initialCertsList.Count(),
                               "Certificate's list received after deletion of test certificate is different from initial certificate's list",
                               "Check certificate's list received after deletion of test certificate");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certificate – CA – Preserve Public Key",
            Order = "02.01.21",
            Id = "2-1-21",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificate })]
        public void DeleteCertificateWithLinkedUploadedPublicKey()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        var certificate = this.CreateTestSelfSignedCACertificateA4();
                        this.UploadCertificate(certificate.GetEncoded(), "ONVIF_Test", null, false, out rsaKeyID, out certID);

                        this.DeleteCertificate(certID);

                        Assert(this.GetAllKeys().Any(e => e.KeyID == rsaKeyID),
                               "The public key from deleted certificate is removed with certificate!",
                               "Check public key from deleted certificate is still on the device");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }


        [Test(Name = "Create Certification Path – self-signed",
            Order = "02.01.12",
            Id = "2-1-12",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCertificationPath })]
        public void CreateSelfSignedCertificationPath()
        {
            var rsaKeyID = "";
            var certID = "";
            RunTest(() =>
                    {
                        var certPathID = this.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        this.DeleteCertificationPath(certPathID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create Certification Path – CA",
            Order = "02.01.13",
            Id = "2-1-13",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
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
                        certPathID = this.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out caKeyID, out caCertID, out certKeyID, out certID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID))
                                               this.DeleteCertificate(caCertID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID))
                                               this.DeleteRSAKeyPair(caKeyID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certKeyID))
                                               this.DeleteRSAKeyPair(certKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certifications Path - Self-Signed",
            Order = "02.01.14",
            Id = "2-1-14",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificationPath })]
        public void GetSelfSignedCertificationPath()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        certPathID = this.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        var certificationPath = this.GetCertificationPath(certPathID);

                        var log = new StringBuilder();
                        Assert(this.ValidateReceivedCertificationPath(certificationPath, new[] { certID }, log),
                               log.ToString(),
                               "Checking received Certification Path");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get Certification Path - CA",
            Order = "02.01.15",
            Id = "2-1-15",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        certPathID = this.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out rsaKeyID1, out certID1, out rsaKeyID2, out certID2);

                        var certificationPath = this.GetCertificationPath(certPathID);

                        var log = new StringBuilder();
                        Assert(this.ValidateReceivedCertificationPath(certificationPath, new[] { certID2, certID1 }, log),
                               log.ToString(),
                               "Checking received Certification Path");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                              this. DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               this.DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certification Paths – self-signed",
            Order = "02.01.16",
            Id = "2-1-16",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAllCertificationPaths })]
        public void GetAllCertificationPathsSelfSigned()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var certPathID1 = "";
            RunTest(() =>
                    {
                        var startCertPathIDs = this.GetAllCertificationPaths();

                        this.UpdateCertificationPathList(startCertPathIDs, out rsaKeyID1, out certID1, out certPathID1);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID1))
                                               this.DeleteCertificationPath(certPathID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               this.DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Get All Certification Paths – CA",
            Order = "02.01.17",
            Id = "2-1-17",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        var startCertPathIDs = this.GetAllCertificationPaths();

                        this.UpdateCertificationPathListByPathBasedOnCAAndCASignedCertificates(startCertPathIDs,
                                                                                        out rsaKeyID1, out certID1,
                                                                                        out rsaKeyID2, out certID2,
                                                                                        out certPathID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               this.DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               this.DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certifications Path - Self-Signed",
            Order = "02.01.18",
            Id = "2-1-18",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertificationPath })]
        public void DeleteSelfSignedCertificationPath()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        var initialCertPathsList = this.GetAllCertificationPaths();

                        this.UpdateCertificationPathList(initialCertPathsList, out rsaKeyID, out certID, out certPathID);

                        var local = certPathID;
                        certPathID = "";
                        this.DeleteCertificationPath(local);
                        local = certID;
                        certID = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        this.DeleteRSAKeyPair(local);

                        var finalCertsList = this.GetAllCertificationPaths();

                        Assert(finalCertsList.All(initialCertPathsList.Contains) && finalCertsList.Count() == initialCertPathsList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Certifications Path - CA",
            Order = "02.01.19",
            Id = "2-1-19",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        var initialCertPathsList = this.GetAllCertificationPaths();

                        this.UpdateCertificationPathListByPathBasedOnCAAndCASignedCertificates(initialCertPathsList,
                                                                                          out rsaKeyID1, out certID1,
                                                                                          out rsaKeyID2, out certID2,
                                                                                          out certPathID);

                        var local = certPathID;
                        certPathID = "";
                        this.DeleteCertificationPath(local);

                        local = certID2;
                        certID2 = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID2;
                        rsaKeyID2 = "";
                        this.DeleteRSAKeyPair(local);

                        local = certID1;
                        certID1 = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID1;
                        rsaKeyID1 = "";
                        this.DeleteRSAKeyPair(local);

                        var finalCertsList = this.GetAllCertificationPaths();

                        Assert(finalCertsList.All(initialCertPathsList.Contains) && finalCertsList.Count() == initialCertPathsList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               this.DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               this.DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "CreateSelfSignedCertificate with PKCS#12",
            Id = "2-1-27",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.PKCS12CertificateWithRSAPrivateKeyUpload },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificate })]
        public void CreateSelfSignedCertificateWithPKCS12Test()
        {
            string certificationPathID = string.Empty, certID = string.Empty, keyID1 = string.Empty;
            RunTest(() =>
                    {
                        RSAKeyPair caKeyPair1;
                        X509CertificateBase caCertificate1;
                        this.HelperUploadPKCS12A36(out keyID1, out caKeyPair1, out caCertificate1, out certificationPathID);

                        certID = this.CreateSelfSignedCertificate(null, DefaultSubject, keyID1, null, DateTime.MinValue, DateTime.MinValue, AdvancedSecurityExtensions.DefaultSignatureAlgorithmIdentifier);

                        var certificateFromDevice = this.GetCertificate(certID);
                        Assert(certID == certificateFromDevice.CertificateID,
                               string.Format("The ODTT requested certificate with CertificateID = '{0}', but the DUT returned certificate with CertificateID = '{1}'", certID, certificateFromDevice.CertificateID),
                               "Check CertificateID of returned certificate");

                        Assert(certificateFromDevice.KeyID == keyID1,
                               string.Format("The certificate returned by the DUT has KeyID = '{0}' while expected '{1}'", certificateFromDevice.KeyID, keyID1),
                               "Check consistency of KeyID");

                        Assert(this.ValidateDEREncoding(new MemoryStream(certificateFromDevice.CertificateContent)),
                                                   "X509Cert is wrongly DER encoded",
                                                   "Check certificate is correctly DER encoded");

                        X509CertificateBC certificate = null;
                        try
                        { certificate = new X509CertificateBC(new MemoryStream(certificateFromDevice.CertificateContent)); }
                        catch
                        {
                            Assert(false, "The certificate is corrupt", "Check integrity of the certificate");
                        }

                        Assert(this.X509NamesAreEqual(certificate.SubjectDN, this.DefaultSubjectString()),
                                                 "Subject of received recertificate is not as expected",
                                                 "Validating subject");

                        var certificates = this.GetAllCertificates();
                        var certificateFromDevice2 = certificates.FirstOrDefault(c => c.CertificateID == certID);
                        Assert(null != certificateFromDevice2,
                               string.Format("The results received via GetAllCertificates are inconsistent with CreateSelfSignedCertificate: GetAllCertificates returned no certificate with CertificateID = '{0}'", certID),
                               "Check consistency of CertificateID");

                        Assert(certificateFromDevice2.KeyID == certificateFromDevice.KeyID,
                               string.Format("The certificate received via GetAllCertificates is inconsistent with GetCertificate: it has KeyID = '{0}' while expected '{1}'",
                                             certificateFromDevice2.KeyID, certificateFromDevice.KeyID),
                               "Check consistency of KeyID");

                        Assert(certificateFromDevice2.CertificateID == certificateFromDevice.CertificateID,
                               string.Format("The certificate received via GetAllCertificates is inconsistent with GetCertificate: it has CertificateID = '{0}' while expected '{1}'",
                                             certificateFromDevice2.CertificateID, certificateFromDevice.CertificateID),
                               "Check consistency of CertificateID");

                        Assert(certificateFromDevice2.Alias == certificateFromDevice.Alias,
                               string.Format("The certificate received via GetAllCertificates is inconsistent with GetCertificate: it has Alias = '{0}' while expected '{1}'",
                                             certificateFromDevice2.Alias, certificateFromDevice.Alias),
                               "Check consistency of Alias");

                        Assert(certificateFromDevice.CertificateContent.SequenceEqual(certificateFromDevice2.CertificateContent),
                               "The content of the certificate received via GetCertificate is different from the one received via GetAllCertificates",
                               "Check consistency of certificates received from the DUT");

                        var localСertID = certID;
                        certID = null;
                        this.DeleteCertificate(localСertID);

                        certificates = this.GetAllCertificates();
                        var certificateFromDevice3 = certificates.FirstOrDefault(c => c.CertificateID == localСertID);
                        Assert(null == certificateFromDevice3,
                               string.Format("The results received via GetAllCertificates are inconsistent with DeleteCertificate: GetAllCertificates returned certificate with CertificateID = '{0}'", localСertID),
                               "Check consistency of CertificateID");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certID))
                                this.DeleteCertificate(certID);
                        });

                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certificationPathID))
                                this.DeleteCertificationPathA35(keyID1, certificationPathID);
                        });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Create PKCS#10 request with PKCS#12",
            Order = "02.01.28",
            Id = "2-1-28",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.PKCS12CertificateWithRSAPrivateKeyUpload },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCertificate })]
        public void CreatePKCS10RequestWithPKCS12Test()
        {
            string certificationPathID = string.Empty, certID = string.Empty, keyID1 = string.Empty, keyID2 = string.Empty;
            RunTest(() =>
                    {
                        RSAKeyPair caKeyPair1;
                        X509CertificateBase caCertificate1;
                        this.HelperUploadPKCS12A36(out keyID1, out caKeyPair1, out caCertificate1, out certificationPathID);

                        var pkcs10 = this.CreatePKCS10CSR(DefaultSubject, keyID1, null, AdvancedSecurityExtensions.DefaultSignatureAlgorithmIdentifier);

                        RSAKeyPair caKeyPair2;
                        X509CertificateBase caCertificate2 = this.CreateTestSelfSignedCACertificateA4(out caKeyPair2, "CN=ONVIF TT2,C=US");

                        var certificateOriginal = this.HelperCreateCertificateFromPKCS10CSRA3(pkcs10, caKeyPair2, caCertificate2);

                        this.UploadCertificate(certificateOriginal.GetEncoded(), AdvancedSecurityExtensions.defaultCertificateAlias, null, false, out keyID2, out certID, keyID1);
                        //No exception, the key will be deleted by A.35
                        keyID2 = string.Empty;

                        var certificateFromDevice = this.GetCertificate(certID);
                        Assert(certID == certificateFromDevice.CertificateID,
                               string.Format("The ODTT requested certificate with CertificateID = '{0}', but the DUT returned certificate with CertificateID = '{1}'", certID, certificateFromDevice.CertificateID),
                               "Check CertificateID of returned certificate");
                        Assert(certificateFromDevice.KeyID == keyID1,
                               string.Format("The certificate returned by the DUT has KeyID = '{0}' while expected '{1}'", certificateFromDevice.KeyID, keyID1),
                               "Check consistency of KeyID");

                        Assert(this.ValidateDEREncoding(new MemoryStream(certificateFromDevice.CertificateContent)),
                                                   "X509Cert is wrongly DER encoded",
                                                   "Check certificate is correctly DER encoded");

                        X509CertificateBC certificate = null;
                        try
                        { certificate = new X509CertificateBC(new MemoryStream(certificateFromDevice.CertificateContent)); }
                        catch
                        {
                            Assert(false, "The certificate is corrupt", "Check integrity of the certificate");
                        }
                        

                        Assert(this.X509NamesAreEqual(certificate.SubjectDN, certificateOriginal.SubjectDN),
                                                 "Subject of received recertificate is not as expected",
                                                 "Validating subject");

                        var certificates = this.GetAllCertificates();
                        var certificateFromDevice2 = certificates.FirstOrDefault(c => c.CertificateID == certID);
                        Assert(null != certificateFromDevice2,
                               string.Format("The results received via GetAllCertificates are inconsistent with UploadCertificate: GetAllCertificates returned no certificate with CertificateID = '{0}'", certID),
                               "Check consistency of CertificateID");

                        Assert(certificateFromDevice2.KeyID == keyID1,
                               string.Format("The certificate received via GetAllCertificates is inconsistent with UploadCertificateWithPrivateKeyInPKCS12: it has KeyID = '{0}' while expected '{1}'",
                                             certificateFromDevice2.KeyID, keyID1),
                               "Check consistency of KeyID");

                        Assert(certificateFromDevice2.CertificateID == certID,
                               string.Format("The certificate received via GetAllCertificates is inconsistent with UploadCertificate: it has CertificateID = '{0}' while expected '{1}'",
                                             certificateFromDevice2.CertificateID, certID),
                               "Check consistency of CertificateID");

                        Assert(certificateFromDevice2.Alias == AdvancedSecurityExtensions.defaultCertificateAlias,
                               string.Format("The certificate received via GetAllCertificates is inconsistent with UploadCertificateWithPrivateKeyInPKCS12: it has Alias = '{0}' while expected '{1}'",
                                             certificateFromDevice2.Alias, AdvancedSecurityExtensions.defaultCertificateAlias),
                               "Check consistency of Alias");

                        Assert(certificateFromDevice.CertificateContent.SequenceEqual(certificateFromDevice2.CertificateContent),
                               "The content of the certificate received via GetCertificate is different from the one received via GetAllCertificates",
                               "Check consistency of certificates received from the DUT");

                        var localСertID = certID;
                        certID = null;
                        this.DeleteCertificate(localСertID);

                        certificates = this.GetAllCertificates();
                        var certificateFromDevice3 = certificates.FirstOrDefault(c => c.CertificateID == localСertID);
                        Assert(null == certificateFromDevice3,
                               string.Format("The results received via GetAllCertificates are inconsistent with DeleteCertificate: GetAllCertificates returned certificate with CertificateID = '{0}'", localСertID),
                               "Check consistency of CertificateID");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certID))
                                this.DeleteCertificate(certID);
                        });

                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(keyID2))
                                this.DeleteRSAKeyPair(keyID2);
                        });

                        this.AllowFaultStep(() =>
                        {
                            if (!string.IsNullOrEmpty(certificationPathID))
                                this.DeleteCertificationPathA35(keyID1, certificationPathID);
                        });

                        this.FinishRestoreSettings();
                    });
        }

        #endregion

        #region Branch 3-*

        #region 3-1-1
        [Test(Name = "Add Server Certificate Assignment – self-signed",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.AddServerCertificateAssignment })]
        public void AddServerSelfSignedCertificatateAssignment()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            RunTest(() =>
                    {
                        certPathID = this.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        this.AddServerCertificateAssignment(certPathID);

                        this.RemoveServerCertificateAssignment(certPathID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-1-2
        [Test(Name = "Add Server Certificate Assignment – CA",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        certPathID = this.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out rsaKeyID1, out certID1, out rsaKeyID2, out certID2);

                        this.AddServerCertificateAssignment(certPathID);

                        this.RemoveServerCertificateAssignment(certPathID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               this.DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               this.DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-1-3
        [Test(Name = "Replace Server Certificate Assignment - Self-Signed",
            Id = "3-1-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.ReplaceServerCertificateAssignment })]
        public void ReplaceServerCertificateAssignmentSelfSigned()
        {
            var rsaKeyID = "";
            var certID1 = "";
            var certID2 = "";
            var certPathID1 = "";
            var certPathID2 = "";
            var lastServerCertificateAssignement = "";
            RunTest(() =>
                    {
                        certPathID1 = this.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID1);

                        this.AddServerCertificateAssignment(certPathID1);
                        lastServerCertificateAssignement = certPathID1;

                        certID2 = this.CreateSelfSignedCertificate(null, DefaultSubject, rsaKeyID, null, new System.DateTime(), new System.DateTime());

                        certPathID2 = this.CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { certID2 } }, "ONVIF_Test");

                        this.ReplaceServerCertificateAssignment(certPathID1, certPathID2);
                        lastServerCertificateAssignement = certPathID2;
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(lastServerCertificateAssignement))
                                               this.RemoveServerCertificateAssignment(lastServerCertificateAssignement);
                                       });


                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID1))
                                               this.DeleteCertificationPath(certPathID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID2))
                                               this.DeleteCertificationPath(certPathID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-1-4
        [Test(Name = "Replace Server Certificate Assignment - CA",
            Id = "3-1-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        RSAKeyPair caKeyPair = null;
                        X509CertificateBase caCert = this.CreateTestSelfSignedCACertificateA4(out caKeyPair);

                        this.UploadCertificateWithoutPrivateKeyA15(caCert, out caKeyID, out caCertID);

                        this.CreateAndUploadCASignedCertificateA16(caCert, caKeyPair, out testKeyID, out certID1);

                        Assert(caKeyID != testKeyID, "The DUT returned the same RSA key pair's IDs for CA certificate and CA-signed certificate", "Verifying recieved RSA key pair's IDs");

                        Assert(caCertID != certID1, "The DUT returned the same certificate's IDs for CA certificate and CA-signed certificate", "Verifying recieved certificate's IDs");

                        certPathID1 = this.CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { certID1, caCertID } }, "ONVIF_TestPath1");

                        this.AddServerCertificateAssignment(certPathID1);
                        lastCertificateAssignment = certPathID1;

                        var cert2 = this.CreateTestCertificateSignedByCACertificateA14(caCert, caKeyPair, testKeyID);
                        this.UploadCertificate(cert2.GetEncoded(), "ONVIF_Test2", null, true, out certID2, testKeyID);

                        Assert(caCertID != certID2 && certID1 != certID2,
                               string.Format("The DUT returned the same certificate's IDs for {0}", caCertID == certID2 ? "CA certificate and CA-signed certificate" : "both CA-signed certificates"),
                               "Verifying recieved certificate's ID");

                        certPathID2 = this.CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { certID2, caCertID } }, "ONVIF_TestPath2");
                        Assert(certPathID2 != certPathID1, "The DUT returned the same certification path's IDs for both test certification paths", "Verifying recieved certification path's IDs");

                        this.ReplaceServerCertificateAssignment(certPathID1, certPathID2);
                        lastCertificateAssignment = certPathID2;
                    },
                    () =>
                    {

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(lastCertificateAssignment))
                                               this.RemoveServerCertificateAssignment(lastCertificateAssignment);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID1))
                                               this.DeleteCertificationPath(certPathID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID2) && certPathID2 != certPathID1)
                                               this.DeleteCertificationPath(certPathID2);
                                       });


                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2) && certID2 != certID1)
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(testKeyID))
                                               this.DeleteRSAKeyPair(testKeyID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID) && caCertID != certID1 && caCertID != certID2)
                                               this.DeleteCertificate(caCertID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID) && caKeyID != testKeyID)
                                               this.DeleteRSAKeyPair(caKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-1-5
        [Test(Name = "Get Assigned Server Certificate - Self-Signed",
            Id = "3-1-5",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAssignedServerCertificates })]
        public void GetAssignedServerCertificateSelfSigned()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        var initialList = this.GetAssignedServerCertificates();

                        this.UpdateAssignedServerCertificateListByPathBaseOnSelfSignedCertificate(initialList, out rsaKeyID, out certID, out certPathID, out removeFlag);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               this.RemoveServerCertificateAssignment(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-1-6
        [Test(Name = "Get Assigned Server Certificate - CA",
            Id = "3-1-6",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        var initialList = this.GetAssignedServerCertificates();

                        this.UpdateAssignedCertificateListByPathBasedOnCAAndCASignedCertificatesA18(initialList, out rsaKeyID1, out certID1, out rsaKeyID2, out certID2, out certPathID, out removeFlag);

                        removeFlag = false;
                        this.RemoveServerCertificateAssignment(certPathID);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               this.RemoveServerCertificateAssignment(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               this.DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               this.DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-1-7
        [Test(Name = "Remove Server Certificate Assignment – self-signed",
            Id = "3-1-7",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveServerCertificateAssignment })]
        public void RemoveSelfSignedServerCertificateAssignment()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        var initialCertPathsList = this.GetAssignedServerCertificates();

                        this.UpdateAssignedServerCertificateListByPathBaseOnSelfSignedCertificate(initialCertPathsList, out rsaKeyID, out certID, out certPathID, out removeFlag);

                        removeFlag = false;
                        this.RemoveServerCertificateAssignment(certPathID);

                        var local = certPathID;
                        certPathID = "";
                        this.DeleteCertificationPath(local);
                        local = certID;
                        certID = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID;
                        rsaKeyID = "";
                        this.DeleteRSAKeyPair(local);

                        var finalCertsList = this.GetAssignedServerCertificates();

                        Assert(finalCertsList.All(initialCertPathsList.Contains) && finalCertsList.Count() == initialCertPathsList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        if (removeFlag)
                            this.AllowFaultStep(() => this.RemoveServerCertificateAssignment(certPathID));

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-1-8
        [Test(Name = "Remove Server Certificate Assignment - CA",
            Id = "3-1-8",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_CERTIFICATE_MANAGEMENT,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        var initialList = this.GetAssignedServerCertificates();

                        this.UpdateAssignedCertificateListByPathBasedOnCAAndCASignedCertificatesA18(initialList, out rsaKeyID1, out certID1, out rsaKeyID2, out certID2, out certPathID, out removeFlag);

                        removeFlag = false;
                        this.RemoveServerCertificateAssignment(certPathID);

                        var local = certPathID;
                        certPathID = "";
                        this.DeleteCertificationPath(local);

                        local = certID1;
                        certID1 = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID1;
                        rsaKeyID1 = "";
                        this.DeleteRSAKeyPair(local);

                        local = certID2;
                        certID2 = "";
                        this.DeleteCertificate(local);
                        local = rsaKeyID2;
                        rsaKeyID2 = "";
                        this.DeleteRSAKeyPair(local);

                        var finalCertsList = this.GetAssignedServerCertificates();

                        Assert(finalCertsList.All(initialList.Contains) && finalCertsList.Count() == initialList.Count(),
                               "Certification path's list received after deletion of test certification path is different from initial certification path's list",
                               "Check certification path's list received after deletion of test certification path");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               this.RemoveServerCertificateAssignment(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               this.DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID2))
                                               this.DeleteCertificate(certID2);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID2))
                                               this.DeleteRSAKeyPair(rsaKeyID2);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-2-3
        [Test(Name = "Basic TLS Handshake",
            Id = "3-2-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_HANDSHAKING,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { })]
        public void TLSServerBasicHandshake()
        {
            var caKeyID = "";
            var caCertID = "";
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            var serverCertificateAssignment = "";
            NetworkProtocol[] initialNetworkProtocols = null;
            var restoreHTTPS = false;
            RunTest(() =>
                    {
                        initialNetworkProtocols = checkHTTPSAccess();

                        certPathID = this.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        var expectedServerCertificate = this.GetCertificate(certID);

                        this.AddServerCertificateAssignment(certPathID);
                        serverCertificateAssignment = certPathID;

                        var httpsProtocol = initialNetworkProtocols.FirstOrDefault(p => NetworkProtocolType.HTTPS == p.Name && p.Enabled && p.Port.Any());
                        if (null == httpsProtocol)
                        {
                            this.SetNetworkProtocols(new[] { httpsProtocol = new NetworkProtocol() { Name = NetworkProtocolType.HTTPS, Enabled = true, Port = new[] { 443 }, Extension = null } });
                            restoreHTTPS = true;
                            DelayAfterSetNetworkProtocols();
                        }

                        var httpsPort = httpsProtocol.Port.First();

                        var client = establishTCPConnection(httpsPort);
                        performTLSHandshake(client.GetStream(), null, expectedServerCertificate.CertificateContent);
                        //performTLSHandshake(client.GetStream(), null, null);
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (restoreHTTPS)
                                           {
                                               this.SetNetworkProtocols(initialNetworkProtocols);
                                               DelayAfterSetNetworkProtocols();
                                           }
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(serverCertificateAssignment))
                                               this.RemoveServerCertificateAssignment(serverCertificateAssignment);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID))
                                               this.DeleteCertificate(caCertID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID))
                                               this.DeleteRSAKeyPair(caKeyID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-2-4
        [Test(Name = "Basic TLS Handshake after Replace Server Certificate Assignment",
            Id = "3-2-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_HANDSHAKING,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { })]
        public void TLSServerReplaceServerCertificateAssignment()
        {
            var rsaKeyID = "";
            var certID = "";
            var certPathID = "";
            var newCertID = "";
            var newCertPathID = "";
            bool flagRemoveInitialAssignement = false;
            bool flagRemoveReplacedAssignement = false;
            NetworkProtocol[] initialNetworkProtocols = null;
            bool restoreNetworkProtocols = false;
            RunTest(() =>
                    {
                        initialNetworkProtocols = checkHTTPSAccess();

                        certPathID = this.CreateTestSelfSignedCertificationPathA11(out rsaKeyID, out certID);

                        var expectedServerCertificate = this.GetCertificate(certID);

                        this.AddServerCertificateAssignment(certPathID);
                        flagRemoveInitialAssignement = true;

                        var httpsProtocol = initialNetworkProtocols.FirstOrDefault(p => NetworkProtocolType.HTTPS == p.Name && p.Enabled && p.Port.Any());
                        if (null == httpsProtocol)
                        {
                            this.SetNetworkProtocols(new[] { httpsProtocol = new NetworkProtocol() { Name = NetworkProtocolType.HTTPS, Enabled = true, Port = new[] { 443 }, Extension = null } });
                            restoreNetworkProtocols = true;
                            DelayAfterSetNetworkProtocols();
                        }

                        var httpsPort = httpsProtocol.Port.First();

                        using (var client = establishTCPConnection(httpsPort))
                        {
                            performTLSHandshake(client.GetStream(), null, expectedServerCertificate.CertificateContent);
                        }

                        var newSubject = new DistinguishedName()
                            {
                                Country = new[] { "US" },
                                CommonName = new[] { _cameraIp.ToString() + "-New" }
                            };
                        newCertID = this.CreateSelfSignedCertificate(null, newSubject, rsaKeyID, null, new DateTime(), new DateTime());

                        newCertPathID = this.CreateCertificationPath(new CertificateIDs() { CertificateID = new[] { newCertID } }, "ONVIF_TEST2");

                        this.ReplaceServerCertificateAssignment(certPathID, newCertPathID);
                        flagRemoveInitialAssignement = false;
                        flagRemoveReplacedAssignement = true;

                        expectedServerCertificate = this.GetCertificate(newCertID);

                        using (var client = establishTCPConnection(httpsPort))
                        {
                            performTLSHandshake(client.GetStream(), null, expectedServerCertificate.CertificateContent);
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (restoreNetworkProtocols)
                                           {
                                               this.SetNetworkProtocols(initialNetworkProtocols);
                                               DelayAfterSetNetworkProtocols();
                                           }
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID) && flagRemoveInitialAssignement)
                                               this.RemoveServerCertificateAssignment(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(newCertPathID) && flagRemoveReplacedAssignement)
                                               this.RemoveServerCertificateAssignment(newCertPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(newCertPathID))
                                               this.DeleteCertificationPath(newCertPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(newCertID))
                                               this.DeleteCertificate(newCertID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-2-5
        [Test(Name = "Basic TLS Handshake with Replace Server Certification Path and PKCS#12",
            Id = "3-2-5",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_SERVER_HANDSHAKING,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS12CertificateWithRSAPrivateKeyUpload, Feature.TLSServerSupport },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new[] { Functionality.UploadCertificateWithPrivateKeyInPKCS12, Functionality.AddServerCertificateAssignment, Functionality.ReplaceServerCertificateAssignment, Functionality.GetAssignedServerCertificates })]
        public void TLSServerReplaceServerCertificateAssignmentAndPKCS12Test()
        {
            var keyIDFirst = "";
            var certPathIDFirst = "";

            var keyIDSecond = "";
            var certPathIDSecond = "";
            var lastServerCertificateAssignment = "";
            NetworkProtocol[] initialNetworkProtocols = null;
            bool restoreNetworkProtocols = false;
            RunTest(() =>
                    {
                        initialNetworkProtocols = checkHTTPSAccess();

                        X509CertificateBase certificateFirst;
                        var pkcs12 = this.CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(out certificateFirst);

                        certPathIDFirst = this.UploadCertificateWithPrivateKeyInPKCS12(pkcs12, AdvancedSecurityExtensions.defaultCertificationPathAlias, AdvancedSecurityExtensions.defaultKeyAlias,
                                                                                       false, null, null, out keyIDFirst);

                        this.AddServerCertificateAssignment(certPathIDFirst);
                        lastServerCertificateAssignment = certPathIDFirst;

                        var httpsProtocol = initialNetworkProtocols.FirstOrDefault(p => NetworkProtocolType.HTTPS == p.Name && p.Enabled && p.Port.Any());
                        if (null == httpsProtocol)
                        {
                            this.SetNetworkProtocols(new[] { httpsProtocol = new NetworkProtocol() { Name = NetworkProtocolType.HTTPS, Enabled = true, Port = new[] { 443 }, Extension = null } });
                            restoreNetworkProtocols = true;
                            DelayAfterSetNetworkProtocols();
                        }

                        var httpsPort = httpsProtocol.Port.First();

                        using (var client = establishTCPConnection(httpsPort))
                        {
                            performTLSHandshake(client.GetStream(), null, certificateFirst.GetEncoded());
                        }

                        X509CertificateBase certificateSecond;
                        pkcs12 = this.CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(out certificateSecond, "CN=ONVIF TT2,C=US");

                        certPathIDSecond = this.UploadCertificateWithPrivateKeyInPKCS12(pkcs12, "ONVIF_CertificationPath_Test2", "ONVIF_Key_Test2", false, null, null, out keyIDSecond);

                        this.ReplaceServerCertificateAssignment(certPathIDFirst, certPathIDSecond);
                        lastServerCertificateAssignment = certPathIDSecond;


                        var assignedServerCertificates = this.GetAssignedServerCertificates();

                        Assert(!assignedServerCertificates.Contains(certPathIDFirst),
                               string.Format("ReplaceServerCertificateAssignment is called, but CertificationPath with ID = '{0}' is still assigned to the TLS server", certPathIDFirst),
                               string.Format("Checking CertificationPath with ID = '{0}' is not assigned to the TLS server on the device", certPathIDFirst));

                        Assert(assignedServerCertificates.Contains(certPathIDSecond),
                               string.Format("ReplaceServerCertificateAssignment is called, but CertificationPath with ID = '{0}' is not assigned to the TLS server", certPathIDSecond),
                               string.Format("Checking CertificationPath with ID = '{0}' is assigned to the TLS server on the device", certPathIDSecond));

                        using (var client = establishTCPConnection(httpsPort))
                        {
                            performTLSHandshake(client.GetStream(), null, certificateSecond.GetEncoded());
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(lastServerCertificateAssignment))
                                               this.RemoveServerCertificateAssignment(lastServerCertificateAssignment);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathIDSecond))
                                               this.DeleteCertificationPathA35(keyIDSecond, certPathIDSecond);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathIDFirst))
                                               this.DeleteCertificationPathA35(keyIDFirst, certPathIDFirst);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (restoreNetworkProtocols)
                                           {
                                               this.SetNetworkProtocols(initialNetworkProtocols);
                                               DelayAfterSetNetworkProtocols();
                                           }
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-3-1
        [Test(Name = "TLS client authentication – self-signed TLS server certificate with on-device RSA key pair",
            Id = "3-3-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSClientAuthentication },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new[] { Functionality.GetClientAuthenticationRequired, Functionality.SetClientAuthenticationRequired, Functionality.AddCertPathValidationPolicyAssignment })]
        public void TLSClientAuthenticationSelfSignedTLSServerCertificateWithOnDeviceRSAKeyPairTest()
        {
            var keyIDFirst = string.Empty;
            var certIDFirst = string.Empty;
            var certPathIDFirst = string.Empty;
            var certificateAssignmentFirstAdded = false;

            var keyIDSecond = string.Empty;
            var certIDSecond = string.Empty;

            NetworkProtocol[] initialNetworkProtocols = null;
            bool restoreNetworkProtocols = false;

            var certPathValidationPolicyID = string.Empty;

            bool clientAuthenticationRequired = false;
            bool restoreClientAuthenticationRequired = false;

            var initialEntryPoint = CameraAddress;

            RunTest(() =>
                    {
                        //{
                        //    RSAKeyPair caKeyPair;
                        //    var ca = this.CreateTestSelfSignedCACertificateA4(out caKeyPair, "CN=onvif.org");

                        //    RSAKeyPair sKeyPair = new RSAKeyPairGeneratorBC(1024).Generate();
                        //    var s  = AdvancedSecurityExtensions.GenerateSignedCertificate(caKeyPair.PrivateKey, 
                        //                                                                  sKeyPair.PublicKey,
                        //                                                                  ca.NotValidBefore,
                        //                                                                  ca.NotValidAfter,
                        //                                                                  ca.SubjectDN, 
                        //                                                                  "CN=thedut.org");

                        //    var cKeyPair = new RSAKeyPairGeneratorBC(1024).Generate();
                        //    var c  = AdvancedSecurityExtensions.GenerateSignedCertificate(caKeyPair.PrivateKey, 
                        //                                                                  cKeyPair.PublicKey,
                        //                                                                  ca.NotValidBefore,
                        //                                                                  ca.NotValidAfter,
                        //                                                                  ca.SubjectDN, 
                        //                                                                  "CN=ONVIF TT Client,C=US");

                        //    using (var file = new FileStream(@"C:\1\root.pfx", FileMode.OpenOrCreate))
                        //    { new BinaryWriter(file).Write(this.PackRSAKeyPairInPKCS12A32(caKeyPair, ca)); }

                        //    using (var file = new FileStream(@"C:\1\server.pfx", FileMode.OpenOrCreate))
                        //    { new BinaryWriter(file).Write(this.PackRSAKeyPairInPKCS12A32(sKeyPair, s)); }

                        //    using (var file = new FileStream(@"C:\1\client.pfx", FileMode.OpenOrCreate))
                        //    { new BinaryWriter(file).Write(this.PackRSAKeyPairInPKCS12A32(cKeyPair, c)); }
                        //}

                        initialNetworkProtocols = checkHTTPSAccess();

                        clientAuthenticationRequired = this.GetClientAuthenticationRequired();

                        this.AddSelfSignedCertificateAsAssignedServerCertificateA13(out keyIDFirst, out certIDFirst, out certPathIDFirst, out certificateAssignmentFirstAdded);

                        RSAKeyPair keyPair2;
                        var certificate2 = this.CreateTestSelfSignedCACertificateA4(out keyPair2, "CN=ONVIF1 TT,C=US");
                        this.UploadCertificateWithoutPrivateKeyA15(certificate2, out keyIDSecond, out certIDSecond);

                        RSAKeyPair keyPair3;
                        var certificate3 = this.HelperCreateSignedCertificateA43(out keyPair3, keyPair2, certificate2, "CN=ONVIF2 TT,C=US");

                        RSAKeyPair keyPair4;
                        var certificate4 = this.CreateTestSelfSignedCACertificateA4(out keyPair4, "CN=ONVIF3 TT,C=US");

                        RSAKeyPair keyPair5;
                        var certificate5 = this.HelperCreateSignedCertificateA43(out keyPair5, keyPair4, certificate4, "CN=ONVIF4 TT,C=US");

                        CertPathValidationPolicy validationPolicy;
                        this.HelperCreateCertPathValidationPolicyWithCertIDA44(out validationPolicy, certIDSecond);
                        certPathValidationPolicyID = validationPolicy.CertPathValidationPolicyID;

                        this.AddCertPathValidationPolicyAssignment(certPathValidationPolicyID);

                        var httpsProtocol = initialNetworkProtocols.FirstOrDefault(p => NetworkProtocolType.HTTPS == p.Name && p.Enabled && p.Port.Any());
                        if (null == httpsProtocol)
                        {
                            this.SetNetworkProtocols(new[] { httpsProtocol = new NetworkProtocol() { Name = NetworkProtocolType.HTTPS, Enabled = true, Port = new[] { 443 }, Extension = null } });
                            restoreNetworkProtocols = true;
                            DelayAfterSetNetworkProtocols();
                        }

                        var httpsPort = httpsProtocol.Port.First();

                        if (!clientAuthenticationRequired)
                        {
                            this.SetClientAuthenticationRequired(true);
                            restoreClientAuthenticationRequired = true;
                        }

                        RaiseNetworkSettingsChangedEvent(SecuredCameraAddress(httpsPort));

                        //switch to https with certificate3
                        Credentials.HTTPS = true;
                        Credentials.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate(certificate2.GetEncoded());

                        var pkcs12 = this.PackRSAKeyPairInPKCS12A32(keyPair3, certificate3);
                        Credentials.ClientCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(pkcs12);

                        UpdateSecurity();

                        //try
                        //{
                            this.GetClientAuthenticationRequired();
                        //}
                        //catch(IOException e)
                        //{
                        //    StepFailed(e);
                        //}
                        
                        //switch to https with certificate5
                        Credentials.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate(certificate4.GetEncoded());

                        pkcs12 = this.PackRSAKeyPairInPKCS12A32(keyPair5, certificate5);
                        Credentials.ClientCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(pkcs12);
                        UpdateSecurity();

                        var failFlag = false;
                        try
                        {
                            this.GetClientAuthenticationRequired();
                            failFlag = true;
                        }
                        catch
                        {
                            StepPassed();
                        }

                        if (failFlag)
                            Assert(failFlag,
                                   "The DUT successfully processed GetClientAuthenticationRequired request via TLS connection with unknown client certificate from ODTT",
                                   "Fail the test!");
                    },
                    () =>
                    {
                        Credentials.HTTPS = false;
                        Credentials.ClientCertificate = null;
                        Credentials.ServerCertificate = null;
                        UpdateSecurity();

                        RaiseNetworkSettingsChangedEvent(initialEntryPoint);

                        this.AllowFaultStep(() =>
                                            {
                                                if (restoreClientAuthenticationRequired)
                                                    this.SetClientAuthenticationRequired(clientAuthenticationRequired);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (restoreNetworkProtocols)
                                                {
                                                    this.SetNetworkProtocols(initialNetworkProtocols);
                                                    DelayAfterSetNetworkProtocols();
                                                }
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certPathValidationPolicyID))
                                                    this.RemoveCertPathValidationPolicyAssignment(certPathValidationPolicyID);

                                                if (!string.IsNullOrEmpty(certPathValidationPolicyID))
                                                    this.DeleteCertPathValidationPolicy(certPathValidationPolicyID);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certIDSecond))
                                                    this.DeleteCertificate(certIDSecond);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(keyIDSecond))
                                                    this.DeleteRSAKeyPair(keyIDSecond);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (certificateAssignmentFirstAdded)
                                                    this.RemoveServerCertificateAssignment(certPathIDFirst);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certPathIDFirst))
                                                    this.DeleteCertificationPath(certPathIDFirst);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certIDFirst))
                                                    this.DeleteCertificate(certIDFirst);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(keyIDFirst))
                                                    this.DeleteRSAKeyPair(keyIDFirst);
                                            });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-3-2
        [Test(Name = "Verify CRL Processing with On-Device RSA Key Pair",
            Id = "3-3-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.PKCS10ExternalCertificationWithRSA, Feature.CRLs, Feature.TLSClientAuthentication },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new[] { Functionality.GetClientAuthenticationRequired, Functionality.SetClientAuthenticationRequired })]
        public void VerifyCRLProcessingWithOnDeviceRSAKeyPairTest()
        {
            var keyIDFirst = string.Empty;
            var certIDFirst = string.Empty;
            var certPathIDFirst = string.Empty;
            var certificateAssignmentFirstAdded = false;

            var keyIDSecond = string.Empty;
            var certIDSecond = string.Empty;

            NetworkProtocol[] initialNetworkProtocols = null;
            bool restoreNetworkProtocols = false;

            var certPathValidationPolicyID = string.Empty;
            var certPathValidationPolicyAssigned = false;

            var crlID = string.Empty;

            bool clientAuthenticationRequired = false;
            bool restoreClientAuthenticationRequired = false;

            var initialEntryPoint = CameraAddress;

            RunTest(() =>
                    {
                        initialNetworkProtocols = checkHTTPSAccess();

                        clientAuthenticationRequired = this.GetClientAuthenticationRequired();

                        this.AddSelfSignedCertificateAsAssignedServerCertificateA13(out keyIDFirst, out certIDFirst, out certPathIDFirst, out certificateAssignmentFirstAdded);

                        RSAKeyPair keyPair2;
                        var certificate2 = this.CreateTestSelfSignedCACertificateA4(out keyPair2, "CN=ONVIF1 TT,C=US");
                        this.UploadCertificateWithoutPrivateKeyA15(certificate2, out keyIDSecond, out certIDSecond);

                        RSAKeyPair keyPair3;
                        var certificate3 = this.HelperCreateSignedCertificateA43(out keyPair3, keyPair2, certificate2, "CN=ONVIF2 TT,C=US");

                        RSAKeyPair keyPair4;
                        var certificate4 = this.CreateTestSelfSignedCACertificateA4(out keyPair4, "CN=ONVIF3 TT,C=US");

                        RSAKeyPair keyPair5;
                        var certificate5 = this.HelperCreateSignedCertificateA43(out keyPair5, keyPair4, certificate4, "CN=ONVIF4 TT,C=US");

                        var crl = this.HelperCreateCRLForCertificateA45(certificate5, keyPair4, certificate5.IssuerDN);

                        crlID = this.UploadCRL(crl.GetEncoded(), AdvancedSecurityExtensions.defaultCRLAlias);

                        CertPathValidationPolicy validationPolicy;
                        this.HelperCreateCertPathValidationPolicyWithCertIDA44(out validationPolicy, certIDSecond);
                        certPathValidationPolicyID = validationPolicy.CertPathValidationPolicyID;

                        this.AddCertPathValidationPolicyAssignment(certPathValidationPolicyID);
                        certPathValidationPolicyAssigned = true;

                        var httpsProtocol = initialNetworkProtocols.FirstOrDefault(p => NetworkProtocolType.HTTPS == p.Name && p.Enabled && p.Port.Any());
                        if (null == httpsProtocol)
                        {
                            this.SetNetworkProtocols(new[] { httpsProtocol = new NetworkProtocol() { Name = NetworkProtocolType.HTTPS, Enabled = true, Port = new[] { 443 }, Extension = null } });
                            restoreNetworkProtocols = true;
                            DelayAfterSetNetworkProtocols();
                        }

                        var httpsPort = httpsProtocol.Port.First();

                        if (!clientAuthenticationRequired)
                        {
                            this.SetClientAuthenticationRequired(true);
                            restoreClientAuthenticationRequired = true;
                        }

                        RaiseNetworkSettingsChangedEvent(SecuredCameraAddress(httpsPort));

                        //switch to https with certificate3
                        Credentials.HTTPS = true;
                        Credentials.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate(certificate2.GetEncoded());

                        var pkcs12 = this.PackRSAKeyPairInPKCS12A32(keyPair3, certificate3);
                        Credentials.ClientCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(pkcs12);

                        UpdateSecurity();

                        //try
                        //{
                            this.GetClientAuthenticationRequired();
                        //}
                        //catch(IOException e)
                        //{
                        //    StepFailed(e);
                        //}
                        
                        //switch to https with certificate5
                        Credentials.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate(certificate4.GetEncoded());

                        pkcs12 = this.PackRSAKeyPairInPKCS12A32(keyPair5, certificate5);
                        Credentials.ClientCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(pkcs12);
                        UpdateSecurity();

                        var failFlag = false;
                        try
                        {
                            this.GetClientAuthenticationRequired();
                            failFlag = true;
                        }
                        catch
                        {
                            StepPassed();
                        }

                        if (failFlag)
                            Assert(failFlag,
                                   "The DUT successfully processed GetClientAuthenticationRequired request via TLS connection with unknown client certificate from ODTT",
                                   "Fail the test!");
                    },
                    () =>
                    {
                        Credentials.HTTPS = false;
                        Credentials.ClientCertificate = null;
                        Credentials.ServerCertificate = null;
                        UpdateSecurity();

                        RaiseNetworkSettingsChangedEvent(initialEntryPoint);

                        this.AllowFaultStep(() =>
                                            {
                                                if (restoreClientAuthenticationRequired)
                                                    this.SetClientAuthenticationRequired(clientAuthenticationRequired);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (restoreNetworkProtocols)
                                                {
                                                    this.SetNetworkProtocols(initialNetworkProtocols);
                                                    DelayAfterSetNetworkProtocols();
                                                }
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (certPathValidationPolicyAssigned)
                                                    this.RemoveCertPathValidationPolicyAssignment(certPathValidationPolicyID);

                                                if (!string.IsNullOrEmpty(certPathValidationPolicyID))
                                                    this.DeleteCertPathValidationPolicy(certPathValidationPolicyID);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(crlID))
                                                    this.DeleteCRL(crlID);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certIDSecond))
                                                    this.DeleteCertificate(certIDSecond);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(keyIDSecond))
                                                    this.DeleteRSAKeyPair(keyIDSecond);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (certificateAssignmentFirstAdded)
                                                    this.RemoveServerCertificateAssignment(certPathIDFirst);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certPathIDFirst))
                                                    this.DeleteCertificationPath(certPathIDFirst);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certIDFirst))
                                                    this.DeleteCertificate(certIDFirst);
                                            });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(keyIDFirst))
                                                    this.DeleteRSAKeyPair(keyIDFirst);
                                            });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 3-3-3
        [Test(Name = "Replace certification path validation policy assignment",
            Id = "3-3-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_TLS_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.CertificationPathValidationPolicies, Feature.TLSClientAuthentication },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new[] { Functionality.ReplaceCertPathValidationPolicyAssignment, Functionality.GetAssignedCertPathValidationPolicies })]
        public void ReplaceCertificationPathValidationPolicyAssignmentTest()
        {
            var keyIDFirst = string.Empty;
            var certIDFirst = string.Empty;
            var certPathIDFirst = string.Empty;
            var certPathValidationPolicyIDFirst = string.Empty;

            var certPathValidationPolicyIDSecond = string.Empty;

            var lastAssignedValidationPolicyID = string.Empty;
            IEnumerable<string> validationPolicies = null;

            RunTest(() =>
                    {
                        CertPathValidationPolicy validationPolicyFirst;
                        this.HelperCreateCertPathValidationPolicyA42(out keyIDFirst, out certIDFirst, out certPathIDFirst, out validationPolicyFirst, "Test CertPathValidationPolicy1 Alias");
                        certPathValidationPolicyIDFirst = validationPolicyFirst.CertPathValidationPolicyID;

                        CertPathValidationPolicy validationPolicySecond;
                        this.HelperCreateCertPathValidationPolicyWithCertIDA44(out validationPolicySecond, certIDFirst, "Test CertPathValidationPolicy2 Alias");
                        certPathValidationPolicyIDSecond = validationPolicySecond.CertPathValidationPolicyID;

                        this.AddCertPathValidationPolicyAssignment(certPathValidationPolicyIDFirst);
                        lastAssignedValidationPolicyID = certPathValidationPolicyIDFirst;

                        this.ReplaceCertPathValidationPolicyAssignment(certPathValidationPolicyIDFirst, certPathValidationPolicyIDSecond);
                        lastAssignedValidationPolicyID = certPathValidationPolicyIDSecond;

                        validationPolicies = this.GetAssignedCertPathValidationPolicies().Where(p => p == certPathValidationPolicyIDFirst || p == certPathValidationPolicyIDSecond);

                        Assert(!validationPolicies.Contains(certPathValidationPolicyIDFirst),
                               string.Format("The Certification Path Validation Policy with CertPathValidationPolicyID = '{0}' is present in GetAssignedCertPathValidationPolicies but it was replaced before",
                                             certPathValidationPolicyIDFirst),
                               string.Format("Checking Certification Path Validation Policy with CertPathValidationPolicyID = '{0}' is successfully replaced", 
                                             certPathValidationPolicyIDFirst));

                        Assert(validationPolicies.Contains(certPathValidationPolicyIDSecond),
                               string.Format("The Certification Path Validation Policy with CertPathValidationPolicyID = '{0}' is absent in GetAssignedCertPathValidationPolicies but it was assigned before",
                                             certPathValidationPolicyIDSecond),
                               string.Format("Checking Certification Path Validation Policy with CertPathValidationPolicyID = '{0}' is successfully replaced", 
                                             certPathValidationPolicyIDFirst));
                    },
                    () =>
                    {
                        if (null != validationPolicies)
                            foreach (var validationPolicyID in validationPolicies)
                            {
                                this.AllowFaultStep(() =>
                                                    {
                                                        if (!string.IsNullOrEmpty(validationPolicyID))
                                                            this.RemoveCertPathValidationPolicyAssignment(validationPolicyID);
                                                    });
                            }
                        else
                            this.AllowFaultStep(() =>
                                                {
                                                    if (!string.IsNullOrEmpty(lastAssignedValidationPolicyID))
                                                        this.RemoveCertPathValidationPolicyAssignment(lastAssignedValidationPolicyID);
                                                });

                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certPathValidationPolicyIDFirst))
                                                    this.DeleteCertPathValidationPolicy(certPathValidationPolicyIDFirst);
                                            });
                        
                        this.AllowFaultStep(() =>
                                            {
                                                if (!string.IsNullOrEmpty(certPathValidationPolicyIDSecond))
                                                    this.DeleteCertPathValidationPolicy(certPathValidationPolicyIDSecond);
                                            });

                        if (!string.IsNullOrEmpty(certPathIDFirst))
                            this.AllowFaultStep(() =>
                                                {
                                                    if (!string.IsNullOrEmpty(certPathIDFirst))
                                                        this.DeleteCertificationPathA35(keyIDFirst, certPathIDFirst);
                                                });
                        else
                        { 
                            this.AllowFaultStep(() =>
                                                {
                                                    if (!string.IsNullOrEmpty(certIDFirst))
                                                        this.DeleteCertificate(certIDFirst);
                                                });

                            this.AllowFaultStep(() =>
                                                {
                                                    if (!string.IsNullOrEmpty(keyIDFirst))
                                                        this.DeleteRSAKeyPair(keyIDFirst);
                                                });
                        }

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #endregion

        #region Branch 4-*
        [Test(Name = "TLS Server Certificate - Self-Signed",
            Order = "04.01.01",
            Id = "4-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_REFERENTIAL_INTEGRITY,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.SelfSignedCertificateCreationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteKey, Functionality.DeleteCertificate, Functionality.DeleteCertificationPath })]
        public void TLSServerCertificateSelfSigned()
        {
            var rsaKeyID1 = "";
            var certID1 = "";
            var certPathID = "";
            bool removeFlag = false;
            RunTest(() =>
                    {
                        this.AddSelfSignedCertificateAsAssignedServerCertificateA13(out rsaKeyID1, out certID1, out certPathID, out removeFlag);

                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(rsaKeyID1); rsaKeyID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificate(certID1); certID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificationPath(certPathID); certPathID = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        removeFlag = false;
                        this.RemoveServerCertificateAssignment(certPathID);

                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(rsaKeyID1); rsaKeyID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificate(certID1); certID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        var local = certPathID;
                        certPathID = "";
                        this.DeleteCertificationPath(local);

                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(rsaKeyID1); rsaKeyID1 = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               this.RemoveServerCertificateAssignment(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID1))
                                               this.DeleteCertificate(certID1);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID1))
                                               this.DeleteRSAKeyPair(rsaKeyID1);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "TLS Server Certificate - CA",
            Order = "04.01.02",
            Id = "4-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_REFERENTIAL_INTEGRITY,
            Version = 1.0,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS10ExternalCertificationWithRSA, Feature.TLSServerSupport, Feature.RSAKeyPairGeneration },
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
                        certPathID = this.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out caKeyID, out caCertID, out rsaKeyID, out certID);

                        this.AddServerCertificateAssignment(certPathID);
                        removeFlag = true;

                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(caKeyID); caKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(rsaKeyID); rsaKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificate(caCertID); caCertID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificate(certID); certID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificationPath(certPathID); certPathID = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        removeFlag = false;
                        this.RemoveServerCertificateAssignment(certPathID);

                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(caKeyID); caKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(rsaKeyID); rsaKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificate(caCertID); caCertID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteCertificate(certID); certID = ""; }, "Sender/InvalidArgVal/ReferenceExists");

                        var local = certPathID;
                        certPathID = "";
                        this.DeleteCertificationPath(local);

                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(caKeyID); caKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                        this.DoActionWithSOAPFault(() => { this.DeleteRSAKeyPair(rsaKeyID); rsaKeyID = ""; }, "Sender/InvalidArgVal/ReferenceExists");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (removeFlag)
                                               this.RemoveServerCertificateAssignment(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certPathID))
                                               this.DeleteCertificationPath(certPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(rsaKeyID))
                                               this.DeleteRSAKeyPair(rsaKeyID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID))
                                               this.DeleteCertificate(caCertID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID))
                                               this.DeleteRSAKeyPair(caKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        #endregion

        #region Branch 5-*

        #region 5-1-1
        [Test(Name = "Advanced Security Service Capabilities",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CAPABILITY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAdvancedSecurityServiceCapabilities })]
        public void AdvancedSecurityCapabilities()
        {
            RunTest(() =>
            {
                var capabilities = (this as IAdvancedSecurityService).GetServiceCapabilities();

                // 5. MaximumNumberOfCertificates
                if (capabilities.KeystoreCapabilities.MaximumNumberOfCertificatesSpecified)
                {
                    int MaximumNumberOfCertificates = capabilities.KeystoreCapabilities.MaximumNumberOfCertificates;

                    if (MaximumNumberOfCertificates > 0)
                    {
                        // 5.1. If cap.KeystoreCapabilities.MaximumNumberOfKeys <= 0 or skipped, FAIL the test.
                        Assert(capabilities.KeystoreCapabilities.MaximumNumberOfKeysSpecified,
                               "MaximumNumberOfKeys attribute skipped when MaximumNumberOfCertificates > 0",
                               "Check for existing of MaximumNumberOfKeys attribute");

                        int MaximumNumberOfKeys = capabilities.KeystoreCapabilities.MaximumNumberOfKeys;

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
                        Assert(capabilities.KeystoreCapabilities.MaximumNumberOfCertificatesSpecified,
                               "MaximumNumberOfCertificates attribute skipped when MaximumNumberOfCertificationPaths > 0",
                               "Check for existing of MaximumNumberOfCertificates attribute");

                        int MaximumNumberOfCertificates = capabilities.KeystoreCapabilities.MaximumNumberOfCertificates;

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
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfKeysSpecified,
                           "MaximumNumberOfKeys attribute skipped when RSAKeyPairGeneration = true",
                           "Check for existing of MaximumNumberOfKeys attribute");

                    int MaximumNumberOfKeys = capabilities.KeystoreCapabilities.MaximumNumberOfKeys;

                    Assert(MaximumNumberOfKeys > 0,
                           "Wrong value: MaximumNumberOfKeys <= 0 when RSAKeyPairGeneration = true",
                           "Validating of MaximumNumberOfKeys value");
                }

                // 8. If cap.KeystoreCapabilities.PKCS8RSAKeyPairUpload = true
                if (capabilities.KeystoreCapabilities.PKCS8RSAKeyPairUpload &&
                    capabilities.KeystoreCapabilities.PKCS8RSAKeyPairUploadSpecified)
                {
                    //8.1. cap.KeystoreCapabilities.MaximumNumberOfPassphrases< 1 or skipped, FAIL the test.

                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfPassphrases),
                           "MaximumNumberOfPassphrases attribute skipped or empty when PKCS8RSAKeyPairUpload = true",
                           "Check for existing of MaximumNumberOfPassphrases attribute");

                    int MaximumNumberOfPassphrases;
                    bool isMaximumNumberOfPassphrasesParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfPassphrases, out MaximumNumberOfPassphrases);

                    Assert(isMaximumNumberOfPassphrasesParse && MaximumNumberOfPassphrases > 0,
                           "MaximumNumberOfPassphrases attribute < 1 when PKCS8RSAKeyPairUpload = true",
                           "Check for existing of MaximumNumberOfPassphrases attribute");

                    //8.2. If cap.KeystoreCapabilities.MaximumNumberOfKeys < 1 or skipped, FAIL the test
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfKeysSpecified,
                           "MaximumNumberOfKeys attribute skipped or empty when PKCS8RSAKeyPairUpload = true",
                           "Check for existing of MaximumNumberOfKeys attribute");

                    int MaximumNumberOfKeys = capabilities.KeystoreCapabilities.MaximumNumberOfKeys;

                    Assert(MaximumNumberOfKeys >= 1,
                           "MaximumNumberOfKeys < 1 when PKCS8RSAKeyPairUpload = true",
                           "Validating of MaximumNumberOfKeys value");

                    //8.3. If cap.KeystoreCapabilities.RSAKeyLenghts is empty or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.RSAKeyLengths != null &&
                        capabilities.KeystoreCapabilities.RSAKeyLengths.Length != 0,
                           "RSAKeyLenghts attribute skipped or empty when PKCS8RSAKeyPairUpload = true",
                           "Check for existing of RSAKeyLenghts attribute");

                    //8.4. If cap.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms is empty or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms != null &&
                        capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms.Length > 0,
                           "PasswordBasedEncryptionAlgorithms attribute skipped or empty when PKCS8RSAKeyPairUpload = true",
                           "Check for existing of PasswordBasedEncryptionAlgorithms attribute");

                    //8.5. If cap.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms does not contain "pbeWithSHAAnd3-KeyTripleDES-CBC" item, FAIL the test.

                    Assert(capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms != null &&
                        capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms.Contains("pbeWithSHAAnd3-KeyTripleDES-CBC"),
                           "PasswordBasedEncryptionAlgorithms attribute does not contain 'pbeWithSHAAnd3-KeyTripleDES-CBC' when PKCS8RSAKeyPairUpload = true",
                           "Validating of PasswordBasedEncryptionAlgorithms");

                }

                // 9. SelfSignedCertificateCreationWithRSA
                if (capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload &&
                    capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUploadSpecified)
                {
                    //9.1. If cap.KeystoreCapabilities.MaximumNumberOfPassphrases< 1 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfPassphrases),
                           "MaximumNumberOfPassphrases attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of MaximumNumberOfPassphrases attribute");

                    int MaximumNumberOfPassphrases;
                    bool isMaximumNumberOfPassphrasesParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfPassphrases, out MaximumNumberOfPassphrases);

                    Assert(isMaximumNumberOfPassphrasesParse && MaximumNumberOfPassphrases >= 1,
                           "MaximumNumberOfPassphrases attribute < 1 when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of MaximumNumberOfPassphrases attribute");

                    //9.2. If cap.KeystoreCapabilities.MaximumNumberOfKeys < 2 or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfKeysSpecified,
                           "MaximumNumberOfKeys attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of MaximumNumberOfKeys attribute");

                    int MaximumNumberOfKeys = capabilities.KeystoreCapabilities.MaximumNumberOfKeys;

                    Assert(MaximumNumberOfKeys >= 2,
                           "MaximumNumberOfKeys < 2 when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Validating of MaximumNumberOfKeys value");

                    //9.3. If cap.KeystoreCapabilities.MaximumNumberOfCertificates < 2 or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfCertificatesSpecified,
                           "MaximumNumberOfCertificatesSpecified attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of MaximumNumberOfCertificatesSpecified attribute");

                    int MaximumNumberOfCertificates = capabilities.KeystoreCapabilities.MaximumNumberOfCertificates;

                    Assert(MaximumNumberOfCertificates >= 2,
                           "MaximumNumberOfCertificates < 2 when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Validating of MaximumNumberOfCertificates value");

                    //9.4. If cap.KeystoreCapabilities.MaximumNumberOfCertificattionPaths <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths),
                        "MaximumNumberOfCertificationPaths attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                        "Check for existing of MaximumNumberOfCertificationPaths attribute");

                    int MaximumNumberOfCertificationPaths;
                    bool isMaximumNumberOfCertificationPathsParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths, out MaximumNumberOfCertificationPaths);

                    Assert(isMaximumNumberOfCertificationPathsParse && MaximumNumberOfCertificationPaths > 0,
                               "MaximumNumberOfCertificationPaths attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                               "Check for existing of MaximumNumberOfCertificationPaths attribute");

                    //9.5. If cap.KeystoreCapabilities.SignatureAlgorithms list is empty, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                                capabilities.KeystoreCapabilities.SignatureAlgorithms.Length != 0,
                                "SignatureAlgorithms is empty or skipped when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                                "Validating of SignatureAlgorithms value");

                    //9.6. If cap.KeystoreCapabilities.RSAKeyLenghts is empty or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.RSAKeyLengths != null &&
                        capabilities.KeystoreCapabilities.RSAKeyLengths.Length != 0,
                           "RSAKeyLenghts attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of RSAKeyLenghts attribute");

                    //9.7. If cap.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms is empty or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms != null &&
                        capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms.Length > 0,
                           "PasswordBasedEncryptionAlgorithms attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of PasswordBasedEncryptionAlgorithms attribute");

                    //9.8. If cap.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms does not contain “pbeWithSHAAnd3-KeyTripleDES-CBC” item, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms != null &&
                        capabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms.Contains("pbeWithSHAAnd3-KeyTripleDES-CBC"),
                           "PasswordBasedEncryptionAlgorithms attribute does not contain 'pbeWithSHAAnd3-KeyTripleDES-CBC' when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Validating of PasswordBasedEncryptionAlgorithms");

                    //9.9. If cap.KeystoreCapabilities.PasswordBasedMACAlgorithms is empty or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.PasswordBasedMACAlgorithms != null &&
                        capabilities.KeystoreCapabilities.PasswordBasedMACAlgorithms.Length > 0,
                           "PasswordBasedMACAlgorithms attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of PasswordBasedMACAlgorithms attribute");

                    //9.10. If cap.KeystoreCapabilities.PasswordBasedMACAlgorithms does not contain “hmacWithSHA256” item, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.PasswordBasedMACAlgorithms != null &&
                        capabilities.KeystoreCapabilities.PasswordBasedMACAlgorithms.Contains("hmacWithSHA256"),
                           "PasswordBasedMACAlgorithms attribute does not contain 'hmacWithSHA256' when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Validating of PasswordBasedMACAlgorithms");

                    //9.11. If cap.KeystoreCapabilities.X509Versions is empty or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.X509Versions != null &&
                        capabilities.KeystoreCapabilities.X509Versions.Length > 0,
                           "X509Versions attribute skipped or empty when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of X509Versions attribute");

                    //9.12. If cap.KeystoreCapabilities.X509Versions does not contain “3” item, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.X509Versions != null &&
                        capabilities.KeystoreCapabilities.X509Versions.Contains(3),
                           "X509Versions attribute does not contain '3' when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Validating of X509Versions");

                    //9.13. If cap.KeystoreCapabilities.SignatureAlgorithms list does not contain item with algorithm = “1.2.840.113549.1.1.5” (OID of SHA-1 with RSA Encryption algorithm), FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Any(e => CompareAlgorithmIdentifier(e, AdvancedSecurityExtensions.DefaultSignatureAlgorithmIdentifier)),
                           "SignatureAlgorithms attribute does not contain '1.2.840.113549.1.1.5' algorithm when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of SignatureAlgorithms attribute");

                    //9.14. If cap.KeystoreCapabilities.SignatureAlgorithms list does not contain item with algorithm = “1.2.840.113549.1.1.11” (OID of SHA-256 with RSA Encryption algorithm), FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Any(e => CompareAlgorithmIdentifier(e, AdvancedSecurityExtensions.OIDSignatureAlgorithmIdentifier)),
                           "SignatureAlgorithms attribute does not contain '1.2.840.113549.1.1.11' algorithm when PKCS12CertificateWithRSAPrivateKeyUpload = true",
                           "Check for existing of SignatureAlgorithms attribute");
                }

                // 10. If cap.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA = true
                if (capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA &&
                    capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified)
                {
                    //10.1. If (cap.KeystoreCapabilities.RSAKeyPairGeneration = false or skipped) and (cap.KeystoreCapabilities.PKCS8RSAKeyPairUpload = false or skipped) and (cap.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload = false or skipped), FAIL the test.
                    Assert((capabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified && capabilities.KeystoreCapabilities.RSAKeyPairGeneration) ||
                    (capabilities.KeystoreCapabilities.PKCS8RSAKeyPairUploadSpecified && capabilities.KeystoreCapabilities.PKCS8RSAKeyPairUpload) ||
                    (capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUploadSpecified && capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload),
                        "RSAKeyPairGeneration = false or skipped and PKCS8RSAKeyPairUpload = false or skipped and PKCS12CertificateWithRSAPrivateKeyUpload = false or skipped when PKCS10ExternalCertificationWithRSA = true",
                        "Check for existing of RSAKeyPairGeneration, PKCS8RSAKeyPairUpload, PKCS12CertificateWithRSAPrivateKeyUpload attribute");

                    //10.2. If cap.KeystoreCapabilities.SignatureAlgorithms list is empty, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                                capabilities.KeystoreCapabilities.SignatureAlgorithms.Length != 0,
                                "SignatureAlgorithms is empty or skipped when PKCS10ExternalCertificationWithRSA = true",
                                "Validating of SignatureAlgorithms value");

                    //10.3. If cap.KeystoreCapabilities.MaximumNumberOfKeys < 2 or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfKeysSpecified,
                           "MaximumNumberOfKeys attribute skipped or empty when PKCS10ExternalCertificationWithRSA = true",
                           "Check for existing of MaximumNumberOfKeys attribute");

                    int MaximumNumberOfKeys = capabilities.KeystoreCapabilities.MaximumNumberOfKeys;

                    Assert(MaximumNumberOfKeys >= 2,
                           "MaximumNumberOfKeys < 2 when PKCS10ExternalCertificationWithRSA = true",
                           "Validating of MaximumNumberOfKeys value");

                    //10.4. If cap.KeystoreCapabilities.MaximumNumberOfCertificates < 2 or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfCertificatesSpecified,
                           "MaximumNumberOfCertificatesSpecified attribute skipped or empty when PKCS10ExternalCertificationWithRSA = true",
                           "Check for existing of MaximumNumberOfCertificatesSpecified attribute");

                    int MaximumNumberOfCertificates = capabilities.KeystoreCapabilities.MaximumNumberOfCertificates;

                    Assert(MaximumNumberOfCertificates >= 2,
                           "MaximumNumberOfCertificates < 2 when PKCS10ExternalCertificationWithRSA = true",
                           "Validating of MaximumNumberOfCertificates value");

                    //10.5. If cap.KeystoreCapabilities.MaximumNumberOfCertificationPaths <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths),
                        "MaximumNumberOfCertificationPaths attribute skipped or empty when PKCS10ExternalCertificationWithRSA = true",
                        "Check for existing of MaximumNumberOfCertificationPaths attribute");

                    int MaximumNumberOfCertificationPaths;
                    bool isMaximumNumberOfCertificationPathsParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths, out MaximumNumberOfCertificationPaths);

                    Assert(isMaximumNumberOfCertificationPathsParse && MaximumNumberOfCertificationPaths > 0,
                               "MaximumNumberOfCertificationPaths attribute skipped or empty when PKCS10ExternalCertificationWithRSA = true",
                               "Check for existing of MaximumNumberOfCertificationPaths attribute");

                    //10.6. If cap.KeystoreCapabilities.SignatureAlgorithms list does not contain item with algorithm = “1.2.840.113549.1.1.5” (OID of SHA-1 with RSA Encryption algorithm), FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Any(e => CompareAlgorithmIdentifier(e, AdvancedSecurityExtensions.DefaultSignatureAlgorithmIdentifier)),
                           "SignatureAlgorithms attribute does not contain '1.2.840.113549.1.1.5' algorithm when PKCS10ExternalCertificationWithRSA = true",
                           "Check for existing of SignatureAlgorithms attribute");

                    //10.7. If cap.KeystoreCapabilities.SignatureAlgorithms list does not contain item with algorithm = “1.2.840.113549.1.1.11” (OID of SHA-256 with RSA Encryption algorithm), FAIL the test. 
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Any(e => CompareAlgorithmIdentifier(e, AdvancedSecurityExtensions.OIDSignatureAlgorithmIdentifier)),
                           "SignatureAlgorithms attribute does not contain '1.2.840.113549.1.1.11' algorithm when PKCS10ExternalCertificationWithRSA = true",
                           "Check for existing of SignatureAlgorithms attribute");
                }

                // 11. If cap.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA = true
                if (capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA &&
                    capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified)
                {
                    //11.1 If (cap.KeystoreCapabilities.RSAKeyPairGeneration = false or skipped) and (cap.KeystoreCapabilities.PKCS8RSAKeyPairUpload = false or skipped) and (cap.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload = false or skipped), FAIL the test.
                    Assert((capabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified && capabilities.KeystoreCapabilities.RSAKeyPairGeneration) ||
                    (capabilities.KeystoreCapabilities.PKCS8RSAKeyPairUploadSpecified && capabilities.KeystoreCapabilities.PKCS8RSAKeyPairUpload) ||
                    (capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUploadSpecified && capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload),
                        "RSAKeyPairGeneration = false or skipped and PKCS8RSAKeyPairUpload = false or skipped and PKCS12CertificateWithRSAPrivateKeyUpload = false or skipped when SelfSignedCertificateCreationWithRSA = true",
                        "Check for existing of RSAKeyPairGeneration, PKCS8RSAKeyPairUpload, PKCS12CertificateWithRSAPrivateKeyUpload attribute");

                    //11.2. If cap.KeystoreCapabilities.MaximumNumberOfCertificates <= 0 or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfCertificatesSpecified,
                           "MaximumNumberOfCertificatesSpecified attribute skipped or empty when SelfSignedCertificateCreationWithRSA = true",
                           "Check for existing of MaximumNumberOfCertificatesSpecified attribute");

                    int MaximumNumberOfCertificates = capabilities.KeystoreCapabilities.MaximumNumberOfCertificates;

                    Assert(MaximumNumberOfCertificates > 0,
                           "MaximumNumberOfCertificates <= 0 when SelfSignedCertificateCreationWithRSA = true",
                           "Validating of MaximumNumberOfCertificates value");

                    //11.3. If cap.KeystoreCapabilities.SignatureAlgorithms list is empty, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                                capabilities.KeystoreCapabilities.SignatureAlgorithms.Length != 0,
                                "SignatureAlgorithms is empty or skipped when SelfSignedCertificateCreationWithRSA = true",
                                "Validating of SignatureAlgorithms value");

                    //11.4. If cap.KeystoreCapabilities.SignatureAlgorithms list does not contain item with algorithm = “1.2.840.113549.1.1.5” (OID of SHA-1 with RSA Encryption algorithm), FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Any(e => CompareAlgorithmIdentifier(e, AdvancedSecurityExtensions.DefaultSignatureAlgorithmIdentifier)),
                           "SignatureAlgorithms attribute does not contain '1.2.840.113549.1.1.5' algorithm when SelfSignedCertificateCreationWithRSA = true",
                           "Check for existing of SignatureAlgorithms attribute");

                    //11.5. If cap.KeystoreCapabilities.SignatureAlgorithms list does not contain item with algorithm = “1.2.840.113549.1.1.11” (OID of SHA-256 with RSA Encryption algorithm), FAIL the test. 
                    Assert(capabilities.KeystoreCapabilities.SignatureAlgorithms != null &&
                        capabilities.KeystoreCapabilities.SignatureAlgorithms.Any(e => CompareAlgorithmIdentifier(e, AdvancedSecurityExtensions.OIDSignatureAlgorithmIdentifier)),
                           "SignatureAlgorithms attribute does not contain '1.2.840.113549.1.1.11' algorithm when SelfSignedCertificateCreationWithRSA = true",
                           "Check for existing of SignatureAlgorithms attribute");
                }

                //12.	If cap.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies > 0
                int MaximumNumberOfCertificationPathValidationPolicies;
                bool isMaximumNumberOfCertificationPathValidationPoliciesParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies, out MaximumNumberOfCertificationPathValidationPolicies);
                if (MaximumNumberOfCertificationPathValidationPolicies > 0)
                {
                    //12.1. If (cap.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA = false or skipped) and (cap.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA = false or skipped) and (cap.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload = false or skipped), FAIL the test.
                    Assert((capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified && capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA) ||
                    (capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified && capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA) ||
                    (capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUploadSpecified && capabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload),
                        "SelfSignedCertificateCreationWithRSA = false or skipped and PKCS10ExternalCertificationWithRSA = false or skipped and PKCS12CertificateWithRSAPrivateKeyUpload = false or skipped when MaximumNumberOfCertificationPathValidationPolicies > 0",
                        "Check for existing of SelfSignedCertificateCreationWithRSA, PKCS10ExternalCertificationWithRSA, PKCS12CertificateWithRSAPrivateKeyUpload attribute");
                }

                // 13. TLSServerSupported
                if (capabilities.TLSServerCapabilities.TLSServerSupported != null &&
                    capabilities.TLSServerCapabilities.TLSServerSupported.Length != 0)
                {
                    //13.1. If cap.TLSServerCapabilities.TLSServerSupported does not contain the value 1.0, FAIL the test.
                    string tlsSupported = capabilities.TLSServerCapabilities.TLSServerSupported.FirstOrDefault(tls => tls == "1.0");
                    Assert(tlsSupported != null,
                        "Wrong value: TLSServerSupported doesn't contain value \"1.0\" when TLSServerSupported is not empty",
                        "Validating of TLSServerSupported value");

                    //13.2. If cap.KeystoreCapabilities.MaximumNumberOfCertificationPaths < 2 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths),
                        "MaximumNumberOfCertificationPaths attribute skipped or empty when TLSServerSupported is not empty",
                        "Check for existing of MaximumNumberOfCertificationPaths attribute");

                    int MaximumNumberOfCertificationPaths;
                    bool result = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths, out MaximumNumberOfCertificationPaths);
                    Assert(result, "MaximumNumberOfCertificationPaths value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificationPaths value");

                    Assert(MaximumNumberOfCertificationPaths >= 2,
                        "Wrong value: MaximumNumberOfCertificationPaths < 2 when TLSServerSupported is not empty",
                        "Validating of MaximumNumberOfCertificationPaths value");

                    //13.3. If cap.KeystoreCapabilities.MaximumNumberOfTLSCertificationPaths <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths),
                        "MaximumNumberOfTLSCertificationPaths attribute skipped or empty when TLSServerSupported is not empty",
                        "Check for existing of MaximumNumberOfTLSCertificationPaths attribute");

                    int MaximumNumberOfTLSCertificationPaths;
                    result = Int32.TryParse(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths, out MaximumNumberOfTLSCertificationPaths);
                    Assert(result, "MaximumNumberOfTLSCertificationPaths value could not be parsed as integer",
                        "Validating format of MaximumNumberOfTLSCertificationPaths value");

                    Assert(MaximumNumberOfTLSCertificationPaths > 0,
                        "Wrong value: MaximumNumberOfTLSCertificationPaths <= 0 when TLSServerSupported is not empty",
                        "Validating of MaximumNumberOfTLSCertificationPaths value");
                    //13.4. If (cap.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA = false or skipped) and (cap.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA = false or skipped), FAIL the test. 
                    Assert((capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified && capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA) ||
                    (capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified && capabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA),
                        "SelfSignedCertificateCreationWithRSA = false or skipped and PKCS10ExternalCertificationWithRSA = false or skipped when TLSServerSupported is not empty",
                        "Check for existing of SelfSignedCertificateCreationWithRSA, PKCS10ExternalCertificationWithRSA, PKCS12CertificateWithRSAPrivateKeyUpload attribute");
                }

                //14.	If cap.TLSServerCapabilities.TLSClientAuthSupported = true
                if (capabilities.TLSServerCapabilities.TLSClientAuthSupportedSpecified &&
                    capabilities.TLSServerCapabilities.TLSClientAuthSupported)
                {
                    //14.1. If cap.TLSServerCapabilities.TLSServerSupported is empty, FAIL the test.
                    Assert(capabilities.TLSServerCapabilities.TLSServerSupported != null &&
                        capabilities.TLSServerCapabilities.TLSServerSupported.Length > 0,
                        "TLSServerSupported attribute is empty when TLSClientAuthSupported = true",
                        "Check for existing of TLSServerSupported attribute");

                    //14.2. If cap.TLSServerCapabilities.MaximumNumberOfTLSCertificationPathValidationPolicies < 2 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies),
                        "MaximumNumberOfCertificationPathValidationPolicies attribute skipped or empty when TLSClientAuthSupported = true",
                        "Check for existing of MaximumNumberOfCertificationPathValidationPolicies attribute");

                    int KeystoreMaximumNumberOfCertificationPathValidationPolicies;
                    bool isKeystoreMaximumNumberOfCertificationPathValidationPoliciesParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies, out KeystoreMaximumNumberOfCertificationPathValidationPolicies);
                    Assert(isKeystoreMaximumNumberOfCertificationPathValidationPoliciesParse, "KeystoreMaximumNumberOfCertificationPathValidationPolicies value could not be parsed as integer",
                        "Validating format of KeystoreMaximumNumberOfCertificationPathValidationPolicies value");

                    Assert(KeystoreMaximumNumberOfCertificationPathValidationPolicies >= 2,
                        "Wrong value: KeystoreMaximumNumberOfCertificationPathValidationPolicies < 2 when TLSClientAuthSupported = true",
                        "Validating of KeystoreMaximumNumberOfCertificationPathValidationPolicies value");

                    //14.3. If cap.TLSServerCapabilities.MaximumNumberOfCertificationPathValidationPolicies <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPathValidationPolicies),
                        "MaximumNumberOfTLSCertificationPathValidationPolicies attribute skipped or empty when TLSClientAuthSupported = true",
                        "Check for existing of MaximumNumberOfTLSCertificationPathValidationPolicies attribute");

                    int MaximumNumberOfTLSCertificationPathValidationPolicies;
                    bool isMaximumNumberOfTLSCertificationPathValidationPoliciesParse = Int32.TryParse(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPathValidationPolicies, out MaximumNumberOfTLSCertificationPathValidationPolicies);
                    Assert(isMaximumNumberOfTLSCertificationPathValidationPoliciesParse, "MaximumNumberOfTLSCertificationPathValidationPolicies value could not be parsed as integer",
                        "Validating format of MaximumNumberOfTLSCertificationPathValidationPolicies value");

                    Assert(MaximumNumberOfTLSCertificationPathValidationPolicies > 0,
                        "Wrong value: MaximumNumberOfTLSCertificationPathValidationPolicies <= 0 when TLSClientAuthSupported = true",
                        "Validating of MaximumNumberOfTLSCertificationPathValidationPolicies value");

                    

                    //14.4. If cap.KeystoreCapabilities.MaximumNumberOfCRLs <= 0 or skipped, FAIL the test.
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCRLs),
                        "MaximumNumberOfCRLs attribute skipped or empty when TLSClientAuthSupported = true",
                        "Check for existing of MaximumNumberOfCRLs attribute");

                    int MaximumNumberOfCRLs;
                    bool isMaximumNumberOfCRLsParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCRLs, out MaximumNumberOfCRLs);
                    Assert(isMaximumNumberOfCRLsParse, "MaximumNumberOfCRLs value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCRLs value");

                    Assert(MaximumNumberOfCRLs > 0,
                        "Wrong value: MaximumNumberOfCRLs <= 0 when TLSClientAuthSupported = true",
                        "Validating of MaximumNumberOfCRLs value");


                }

                //15.	If cap.TLSServerCapabilities.MaximumNumberOfTLSCertificationPathValidationPolicies > 0
                int TLSMaximumNumberOfTLSCertificationPathValidationPolicies;
                bool isTLSMaximumNumberOfTLSCertificationPathValidationPoliciesParse = Int32.TryParse(capabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPathValidationPolicies, out TLSMaximumNumberOfTLSCertificationPathValidationPolicies);
                if (TLSMaximumNumberOfTLSCertificationPathValidationPolicies > 0)
                {
                    //15.1. If cap.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies <= 0 or skipped, FAIL the test. 
                    Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies),
                           "MaximumNumberOfCertificationPathValidationPolicies attribute skipped or empty when MaximumNumberOfTLSCertificationPathValidationPolicies > 0",
                           "Check for existing of MaximumNumberOfCertificationPathValidationPolicies attribute");

                    int KeystoreMaximumNumberOfCertificationPathValidationPolicies;
                    bool isKeystoreMaximumNumberOfCertificationPathValidationPoliciesParse = Int32.TryParse(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies, out KeystoreMaximumNumberOfCertificationPathValidationPolicies);
                    Assert(isKeystoreMaximumNumberOfCertificationPathValidationPoliciesParse, "MaximumNumberOfCertificationPathValidationPolicies value could not be parsed as integer",
                        "Validating format of MaximumNumberOfCertificationPathValidationPolicies value");

                    Assert(MaximumNumberOfCertificationPathValidationPolicies > 0,
                        "Wrong value: MaximumNumberOfCertificationPathValidationPolicies <= 0 when MaximumNumberOfTLSCertificationPathValidationPolicies > 0",
                        "Validating of MaximumNumberOfCertificationPathValidationPolicies value");

                }

                //16.	If cap.TLSServerCapabilities.TLSServerSupported is not empty and cap.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA = true
                if (capabilities.TLSServerCapabilities.TLSServerSupported != null &&
                    capabilities.TLSServerCapabilities.TLSServerSupported.Length > 0 &&
                    capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified &&
                    capabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA)
                {
                    //16.1. If cap.KeystoreCapabilities.MaximumNumberOfCertificates < 3 or skipped, FAIL the test.
                    Assert(capabilities.KeystoreCapabilities.MaximumNumberOfCertificates >= 3,
                        "Wrong value: MaximumNumberOfCertificates < 3 when TLSServerSupported is not empty and PKCS10ExternalCertificationWithRSA = true",
                        "Validating of MaximumNumberOfCertificates value");
                }

                // 17. MaximumNumberOfTLSCertificationPaths
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
                        //17.1. If cap.KeystoreCapabilities.MaximumNumberOfCertificationPaths <= 0 or skipped, FAIL the test.
                        Assert(!string.IsNullOrEmpty(capabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths),
                            "MaximumNumberOfCertificationPaths attribute skipped or empty when MaximumNumberOfTLSCertificationPaths > 0",
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

                // Temprary commented according ticket #971
                /*
                //18.	If cap.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations > 0:
                if (capabilities.Dot1XCapabilities !=null && !string.IsNullOrEmpty(capabilities.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations))
                {
                    int MaximumNumberOfDot1XConfigurations;
                    bool result = Int32.TryParse(capabilities.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations, out MaximumNumberOfDot1XConfigurations);
                    Assert(result, "MaximumNumberOfDot1XConfigurations value could not be parsed as integer",
                        "Validating format of MaximumNumberOfDot1XConfigurations value");

                    if (MaximumNumberOfDot1XConfigurations > 0)
                    {
                        //18.1. If cap.Dot1XCapabilities.Dot1XMethods list is empty, FAIL the test.
                        Assert(capabilities.Dot1XCapabilities.Dot1XMethods != null &&
                            capabilities.Dot1XCapabilities.Dot1XMethods.Length > 0,
                            "Dot1XMethods element list is empty when MaximumNumberOfDot1XConfigurations > 0",
                            "Check for existing of Dot1XMethods element list");
                    }
                }
                
                //19.	If cap.Dot1XCapabilities.Dot1XMethods list is not empty
                if (capabilities.Dot1XCapabilities != null && capabilities.Dot1XCapabilities.Dot1XMethods != null &&
                    capabilities.Dot1XCapabilities.Dot1XMethods.Length > 0)
                {
                    //19.1. If cap.Dot1XCapabilities.Dot1XMethods list does not contain “EAP-PEAP MSCHAPv2”, FAIL the test.
                    Assert(capabilities.Dot1XCapabilities.Dot1XMethods != null &&
                        capabilities.Dot1XCapabilities.Dot1XMethods.Contains("EAP-PEAP MSCHAPv2"),
                           "Dot1XMethods element list does not contain 'EAP-PEAP MSCHAPv2' when Dot1XMethods is not empty",
                           "Check for existing of Dot1XMethods element list");

                    //19.2. If cap.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations <= 0 or skipped, FAIL the test. 
                    Assert(!string.IsNullOrEmpty(capabilities.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations),
                            "MaximumNumberOfDot1XConfigurations attribute skipped or empty when Dot1XMethods is not empty",
                            "Check for existing of MaximumNumberOfDot1XConfigurations attribute");

                    int MaximumNumberOfDot1XConfigurations;
                    bool result = Int32.TryParse(capabilities.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations, out MaximumNumberOfDot1XConfigurations);
                    Assert(result, "MaximumNumberOfDot1XConfigurations value could not be parsed as integer",
                        "Validating format of MaximumNumberOfDot1XConfigurations value");

                    Assert(MaximumNumberOfDot1XConfigurations > 0,
                        "Wrong value: MaximumNumberOfDot1XConfigurations <= 0 when Dot1XMethods is not empty",
                        "Validating of MaximumNumberOfDot1XConfigurations value");
                }
                */
            },
            () =>
            {

            });
        }
        #endregion

        #region 5-1-2
        [Test(Name = "Get Services and Get Advanced Security Service Capabilities Consistency",
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CAPABILITY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetAdvancedSecurityServiceCapabilities })]
        public void CapabilitiesConsistency()
        {
            RunTest(() =>
            {
                // GetServices
                Service[] services = this.GetServices(true);

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

                AdvancedSecurityCapabilities advancedSecurityCapabilities = ExtractAdvancedSecurityCapabilities(advancedSecurityService.Capabilities);

                // GetServiceCapabilities
                var capabilities = (this as IAdvancedSecurityService).GetServiceCapabilities();

                CompareCapabilities(advancedSecurityCapabilities, capabilities);
            },
            () =>
            {

            });
        }
        #endregion

        #endregion

        #region Branch 6-*

        [Test(Name = "Upload Passphrase Verification",
            Id = "6-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_PASSPHRASE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PassphraseManagement },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadPassphrase })]
        public void UploadPassphraseVerificationTest()
        {
            var passPhraseID = string.Empty;
            RunTest(() =>
                    {
                        var passPhrase = this.GenerateTestPassphraseA24();

                        passPhraseID = this.UploadPassphrase(passPhrase, AdvancedSecurityExtensions.defaultPassphraseAlias);

                        var justUploadedPassphrase = this.GetAllPassphrases().FirstOrDefault(e => e.PassphraseID == passPhraseID);
                        Assert(null != justUploadedPassphrase,
                               "Passphrase's list received after test passphrase's creation doesn't contain just created passphrase",
                               "Check passphrase's list received after test passphrase's creation");

                        Assert(AdvancedSecurityExtensions.defaultPassphraseAlias == justUploadedPassphrase.Alias,
                               string.Format("The alias of passphrase received from the DUT {0}",
                                             null == justUploadedPassphrase.Alias
                                             ?
                                             "isn't specified" : string.Format("has unexpected value '{0}'. Expected: {1}", justUploadedPassphrase.Alias, AdvancedSecurityExtensions.defaultPassphraseAlias)),
                               "Check passphrase's alias");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(passPhraseID))
                                               this.DeletePassphrase(passPhraseID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Delete Passphrase Verification",
            Id = "6-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_PASSPHRASE_MANAGEMENT,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PassphraseManagement },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeletePassphrase })]
        public void DeletePassphraseVerificationTest()
        {
            RunTest(() =>
                    {
                        var passPhrase = this.GenerateTestPassphraseA24();

                        var passPhraseID = this.UploadPassphrase(passPhrase, AdvancedSecurityExtensions.defaultPassphraseAlias);

                        this.DeletePassphrase(passPhraseID);

                        Assert(!this.GetAllPassphrases().Select(e => e.PassphraseID).Contains(passPhraseID),
                               "Passphrase's list received after test passphrase's deletion contains just deleted passphrase",
                               "Check passphrase's list received after test passphrase's deletion");
                    });
        }

        [Test(Name = "Upload PKCS8 - no key pair exists",
            Id = "6-2-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_PKCS8,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS8RSAKeyPairUpload },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadKeyPairInPKCS8 })]
        public void UploadPKCS8NoKeyPairExistsTest()
        {
            var keyID = string.Empty;
            RunTest(() =>
                    {
                        keyID = this.UploadKeyPairInPKCS8(this.GenerateRSAKeyPairInPKCS8WithoutPassphraseBinaryA25(), "ONVIF_Test", null);

                        var allKeys = this.GetAllKeys();

                        var key = allKeys.FirstOrDefault(k => keyID == k.KeyID);
                        Assert(null != key,
                               string.Format("The DUT has no key pair with KeyID = '{0}'", keyID),
                               "Check the key pair is successfully uploaded");
                        Assert(key.hasPrivateKeySpecified && key.hasPrivateKey,
                               string.Format("Uploading of the private key for key pair with KeyID = '{0}' is failed", keyID),
                               "Check the private key is successully uploaded");
                        Assert(key.KeyStatus == AdvancedSecurityExtensions.ksOK,
                               string.Format("The uploaded key pair with KeyID = '{0}' has unexpected KeyStatus '{1}'. Expected: '{2}'.", keyID, key.KeyStatus, AdvancedSecurityExtensions.ksOK),
                               "Check KeyStatus of uploaded key pair");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(keyID))
                                               this.DeleteRSAKeyPair(keyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload PKCS8 – decryption fails",
            Id = "6-2-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_PKCS8,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS8RSAKeyPairUpload },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadKeyPairInPKCS8 })]
        public void UploadPKCS8DecryptionFailsTest()
        {
            var keyID = string.Empty;
            var passphraseID = string.Empty;
            RunTest(() =>
                    {
                        var passphraseFirst = this.GenerateTestPassphraseA24();
                        var key = this.GenerateTestKeyPairWithPassphraseA28(passphraseFirst);
                        var passphraseSecond = this.GenerateTestPassphraseA24(1);

                        passphraseID = this.UploadPassphrase(passphraseSecond, AdvancedSecurityExtensions.defaultPassphraseAlias);

                        try
                        {
                            keyID = this.UploadKeyPairInPKCS8(key.PrivateKey, "ONVIF_Test", passphraseID);

                            Assert(false, "The DUT did not send the env:Sender/ter:InvalidArgVal/ter:DecryptionFailed SOAP 1.2 fault message.", "Fail the test");
                        }
                        catch (FaultException e)
                        {
                            if (!e.IsValidOnvifFault("Sender/InvalidArgVal/DecryptionFailed"))
                                throw;

                            StepPassed();
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(passphraseID))
                                               this.DeletePassphrase(passphraseID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(keyID))
                                               this.DeleteRSAKeyPair(keyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload PKCS12 - no key pair exists",
            Id = "6-3-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_PKCS12,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS12CertificateWithRSAPrivateKeyUpload },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificateWithPrivateKeyInPKCS12 })]
        public void UploadPKCS12NoKeyPairExistsTest()
        {
            string certificationPathID = string.Empty, certID = string.Empty, keyID = string.Empty;
            RunTest(() =>
                    {
                        certificationPathID = this.UploadCertificateWithPrivateKeyInPKCS12(this.CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(), AdvancedSecurityExtensions.defaultCertificationPathAlias, AdvancedSecurityExtensions.defaultKeyAlias, true, null, null, out keyID);

                        var keys = this.GetAllKeys();
                        var justUploadedKey = keys.FirstOrDefault(k => k.KeyID == keyID);
                        Assert(null != justUploadedKey,
                               string.Format("The DUT has no key pair with KeyID = '{0}'", keyID),
                               "Check the key pair is successfully uploaded");
                        Assert(justUploadedKey.hasPrivateKeySpecified && justUploadedKey.hasPrivateKey,
                               string.Format("Uploading of the private key for key pair with KeyID = '{0}' is failed", keyID),
                               "Check the private key is successully uploaded");
                        Assert(justUploadedKey.Alias == AdvancedSecurityExtensions.defaultKeyAlias,
                               string.Format("The alias of uploaded key pair with KeyID = '{0}' has unexpected value '{1}'. Expected: '{2}'.", keyID, justUploadedKey.Alias, AdvancedSecurityExtensions.defaultKeyAlias),
                               "Check alias of uploaded key pair");
                        Assert(justUploadedKey.KeyStatus == AdvancedSecurityExtensions.ksOK,
                               string.Format("The uploaded key pair with KeyID = '{0}' has unexpected KeyStatus '{1}'. Expected: '{2}'.", keyID, justUploadedKey.KeyStatus, AdvancedSecurityExtensions.ksOK),
                               "Check KeyStatus of uploaded key pair");

                        var certificationPaths = this.GetAllCertificationPaths();
                        Assert(certificationPaths.Contains(certificationPathID),
                               string.Format("The DUT has no certification path with CertificationPathID = '{0}'", certificationPathID),
                               "Check the certification path is successfully uploaded");

                        var justCertificationPath = this.GetCertificationPath(certificationPathID);
                        Assert(justCertificationPath.Alias == AdvancedSecurityExtensions.defaultCertificationPathAlias,
                               string.Format("The alias of uploaded certification path with CertificationPathID = '{0}' has unexpected value '{1}'. Expected: '{2}'.", certificationPathID, justCertificationPath.Alias, AdvancedSecurityExtensions.defaultCertificationPathAlias),
                               "Check alias of uploaded certification path");

                        Assert(null != justCertificationPath.CertificateID && 1 == justCertificationPath.CertificateID.Count(),
                               string.Format("The uploaded certification path with CertificationPathID = '{0}' has {1}",
                                             certificationPathID,
                                             null == justCertificationPath.CertificateID || !justCertificationPath.CertificateID.Any() ? "no certificates" : "more than one certificate"),
                               "Check certificates of uploaded certification path");
                        certID = justCertificationPath.CertificateID.First();
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certificationPathID))
                                               this.DeleteCertificationPathA35(keyID, certificationPathID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload PKCS12 – decryption fails",
            Id = "6-3-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_PKCS12,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS12CertificateWithRSAPrivateKeyUpload },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificateWithPrivateKeyInPKCS12, Functionality.GetKeyStatus, Functionality.GetCertificate, Functionality.GetAllCertificates })]
        public void UploadPKCS12DecryptionFailsTest()
        {
            var keyID = string.Empty;
            var passphraseID = string.Empty;
            var certPathID = string.Empty;
            RunTest(() =>
                    {
                        var passphraseFirst = this.GenerateTestPassphraseA24();
                        var passphraseSecond = this.GenerateTestPassphraseA24(1);
                        var key = this.CreateRSAKeyPairInPKCS12EncryptionIntegrityA31(passphraseFirst);

                        passphraseID = this.UploadPassphrase(passphraseSecond, AdvancedSecurityExtensions.defaultPassphraseAlias);

                        try
                        {
                            certPathID = this.UploadCertificateWithPrivateKeyInPKCS12(key, AdvancedSecurityExtensions.defaultCertificationPathAlias, AdvancedSecurityExtensions.defaultKeyAlias, false, null, passphraseID, out keyID);

                            Assert(false, "The DUT did not send the env:Sender/ter:InvalidArgVal/ter:DecryptionFailed SOAP 1.2 fault message.", "Fail the test");
                        }
                        catch (FaultException e)
                        {
                            if (!e.IsValidOnvifFault("Sender/InvalidArgVal/DecryptionFailed"))
                                throw;

                            StepPassed();
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(passphraseID))
                                               this.DeletePassphrase(passphraseID);
                                       });

                        this.DeleteCertificationPathA35(keyID, certPathID);

                        this.FinishRestoreSettings();
                    });
        }

        [Test(Name = "Upload PKCS12 - Verify Key and Certificate",
            Id = "6-3-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_PKCS12,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.PKCS12CertificateWithRSAPrivateKeyUpload },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.UploadCertificateWithPrivateKeyInPKCS12 })]
        public void UploadPKCS12VerifyKeyAndCertificateTest()
        {
            string certificationPathID = string.Empty, keyID = string.Empty;
            RunTest(() =>
                    {
                        X509CertificateBase certificate;
                        certificationPathID = this.UploadCertificateWithPrivateKeyInPKCS12(this.CreateRSAKeyPairInPKCS12NoEncryptionNoIntegrityA30(out certificate), 
                                                                                           AdvancedSecurityExtensions.defaultCertificationPathAlias, AdvancedSecurityExtensions.defaultKeyAlias, 
                                                                                           false, null, null, out keyID);

                        var keyStatus = this.GetKeyStatus(keyID);

                        Assert(AdvancedSecurityExtensions.ksOK == keyStatus,
                               string.Format("The PKCS#12 container is not successfully uploaded: the key pair has status '{0}' while expected '{1}'", keyStatus, AdvancedSecurityExtensions.ksOK),
                               "Check status of the uploaded key pair");

                        var certificationPath = this.GetCertificationPath(certificationPathID);
                        Assert(null != certificationPath.CertificateID && 1 == certificationPath.CertificateID.Count(),
                               null == certificationPath.CertificateID
                               ?
                               string.Format("The DUT has no CertificationPath with ID = '{0}'", certificationPathID)
                               :
                               string.Format("The DUT has CertificationPath with ID = '{0}', however, it consists of {1} certificates while only 1 is expected", certificationPathID, certificationPath.CertificateID.Count()),
                               "Check uploaded CertificationPath");

                        var certificates = this.GetAllCertificates();

                        var justUploadedCertificateID = certificationPath.CertificateID.First();
                        var justUploadedCertificate = certificates.FirstOrDefault(k => k.CertificateID == justUploadedCertificateID);
                        Assert(null != justUploadedCertificate,
                               string.Format("The results received via GetCertificationPath and GetAllCertificates are inconsistent: GetAllCertificates returned no certificate with CertificateID = '{0}', but CertificationPath with ID = '{1}' has it", justUploadedCertificateID, certificationPathID),
                               "Check consistency of CertificateID");
                        Assert(justUploadedCertificate.KeyID == keyID,
                               string.Format("The certificate is successfully uploaded, however, it has KeyID = '{0}' while expected '{1}'", justUploadedCertificate.KeyID, keyID),
                               "Check consistency of KeyID");
                        Assert(justUploadedCertificate.CertificateContent.SequenceEqual(certificate.GetEncoded()),
                               "The content of certificate returned by the DUT is different from the one uploaded in PKCS#12 container",
                               "Check consistency of certificate in binary form");

                        var certificateFromDevice = this.GetCertificate(justUploadedCertificateID);
                        Assert(justUploadedCertificateID == certificateFromDevice.CertificateID,
                               string.Format("The ODTT requested certificate with CertificateID = '{0}', but the DUT returned certificate with CertificateID = '{1}'", justUploadedCertificateID, certificateFromDevice.CertificateID),
                               "Check CertificateID of returned certificate");
                        Assert(certificateFromDevice.KeyID == keyID,
                               string.Format("The certificate returned by the DUT has KeyID = '{0}' while expected '{1}'", certificateFromDevice.KeyID, keyID),
                               "Check consistency of KeyID");

                        Assert(certificateFromDevice.CertificateContent.SequenceEqual(certificate.GetEncoded()),
                               "The content of certificate returned by the DUT is different from the one uploaded in PKCS#12 container",
                               "Check consistency of certificate in binary form");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certificationPathID))
                                               this.DeleteCertificationPathA35(keyID, certificationPathID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region Branch 7-*

        #region 7-1-1
        //[Test(Name = "Configure IEEE 802.1X Configuration",
        //    Id = "7-1-1",
        //    Category = Category.ADVANCED_SECURITY,
        //    Path = PATH_IEEE_8021X_CONFIGURATION,
        //    Version = 1.0,
        //    RequirementLevel = RequirementLevel.Optional,
        //    RequiredFeatures = new [] { Feature.AdvancedSecurity, Feature.Configurations802Dot1X, Feature.PassphraseManagement,  Feature.PKCS10ExternalCertificationWithRSA, Feature.RSAKeyPairGeneration, Feature.TLSServerSupport },
        //    LastChangedIn = "v15.06",
        //    FunctionalityUnderTest = new [] { Functionality.AddDot1XConfiguration, Functionality.GetAllDot1XConfigurations })]
        public void ConfigureIEEE8021XConfigurationTest()
        {
            var passPhraseID = string.Empty;
            var certificationPathID = string.Empty;
            var dot1XID = string.Empty;
            var caKeyID = string.Empty;
            var caCertID = string.Empty;
            var keyID = string.Empty;
            var certID = string.Empty;

            RunTest(() =>
                    {
                        var passPhrase = this.GenerateTestPassphraseA24();

                        passPhraseID = this.UploadPassphrase(passPhrase, AdvancedSecurityExtensions.defaultPassphraseAlias);
                        
                        certificationPathID = this.CreateCertificationPathBasedOnCAAndCASignedCertificatesA18(out caKeyID, out caCertID, out keyID, out certID);

                        var dot1XConfig = new Dot1XConfiguration1()
                                          {
                                              Outer = new Dot1XStage()
                                                      {
                                                          Method = "EAP-PEAP",
                                                          Identity = null,
                                                          AnonymousID = null,
                                                          CertificationPathID = certificationPathID,
                                                          PassphraseID = null,
                                                          Inner = new Dot1XStage()
                                                                  {
                                                                      Method = "MSCHAPv2",
                                                                      Identity = "ONVIF Test",
                                                                      AnonymousID = null,
                                                                      CertificationPathID = null,
                                                                      PassphraseID = passPhraseID,
                                                                      Inner = null,
                                                                      Extension = null
                                                                  },
                                                          Extension = null
                                                      }
                                          };

                        dot1XID = this.AddDot1XConfiguration(dot1XConfig, AdvancedSecurityExtensions.defaultDot1XAlias);

                        var dot1xConfigs = this.GetAllDot1XConfigurations();

                        var justUploadedConfig = dot1xConfigs.FirstOrDefault(c => c.Dot1XID == dot1XID);
                        Assert(null != justUploadedConfig, 
                               string.Format("The results received via AddDot1XConfiguration and GetAllDot1XConfigurations are inconsistent: GetAllDot1XConfigurations returned no Dot1X Configuration with Dot1XID = '{0}'", dot1XID),
                               "Check consistency of Dot1XID");
                        Assert(justUploadedConfig.Alias == AdvancedSecurityExtensions.defaultDot1XAlias,
                               string.Format("The alias of uploaded Dot1X Configuration with Dot1XID = '{0}' has unexpected value '{1}'. Expected: '{2}'.", dot1XID, justUploadedConfig.Alias, AdvancedSecurityExtensions.defaultDot1XAlias),
                               "Check alias of received Dot1X Configuration");

                        try
                        {
                            this.DeletePassphrase(passPhraseID);
                            passPhraseID = string.Empty;

                            Assert(false, "The DUT did not send the env:Sender/ter:InvalidArgVal/ter:ReferenceExists SOAP 1.2 fault message.", "Fail the test");
                        }
                        catch (FaultException e)
                        {
                            if (!e.IsValidOnvifFault("Sender/InvalidArgVal/ReferenceExists"))
                            {
                                LogStepEvent("The env:Sender/ter:InvalidArgVal/ter:ReferenceExists SOAP 1.2 fault message is expected.");
                                throw;
                            }

                            StepPassed();
                        }

                        try
                        {
                            this.DeleteCertificationPath(certificationPathID);
                            certificationPathID = string.Empty;

                            Assert(false, "The DUT did not send the env:Sender/ter:InvalidArgVal/ter:ReferenceExists SOAP 1.2 fault message.", "Fail the test");
                        }
                        catch (FaultException e)
                        {
                            if (!e.IsValidOnvifFault("Sender/InvalidArgVal/ReferenceExists"))
                            {
                                LogStepEvent("The env:Sender/ter:InvalidArgVal/ter:ReferenceExists SOAP 1.2 fault message is expected.");
                                throw;
                            }

                            StepPassed();
                        }
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(dot1XID))
                                               this.DeleteDot1XConfiguration(dot1XID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(passPhraseID))
                                               this.DeletePassphrase(passPhraseID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certificationPathID))
                                               this.DeleteCertificationPath(certificationPathID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certID))
                                               this.DeleteCertificate(certID);
                                       });
                        
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(keyID))
                                               this.DeleteRSAKeyPair(keyID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caCertID))
                                               this.DeleteCertificate(caCertID);
                                       });

                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(caKeyID))
                                               this.DeleteRSAKeyPair(caKeyID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #endregion

        #region Branch 8-*

        #region 8-1-1
        [Test(Name = "Upload CRL",
            Id = "8-1-1",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_BASED_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new [] { Feature.AdvancedSecurity, Feature.CRLs },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new [] { Functionality.UploadCRL, Functionality.GetAllCRLs })]
        public void UploadCRLTest()
        {
            var crlID = string.Empty;

            RunTest(() =>
                    {
                        var crl = this.HelperCreateCRLA37();

                        crlID = this.UploadCRL(crl.GetEncoded(), AdvancedSecurityExtensions.defaultCRLAlias);

                        var crls = this.GetAllCRLs();

                        var justUploadedCrl = crls.FirstOrDefault(c => c.CRLID == crlID);
                        Assert(null != justUploadedCrl,
                               string.Format("The results received via UploadCRL and GetAllCRLs are inconsistent: GetAllCRLs returned no CRL with CRLID = '{0}'", crlID),
                               "Check consistency of CRLID");

                        Assert(justUploadedCrl.Alias == AdvancedSecurityExtensions.defaultCRLAlias,
                               string.Format("The alias of received CRL with CRLID = '{0}' has unexpected value '{1}'. Expected: '{2}'.", crlID, justUploadedCrl.Alias, AdvancedSecurityExtensions.defaultCRLAlias),
                               "Check alias of received CRL");


                        Assert(crl.GetEncoded().SequenceEqual(justUploadedCrl.CRLContent),
                               "The CRL received via GetAllCRLs is different from uploaded via UploadCRL",
                               "Check consistency of CRLs");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(crlID))
                                               this.DeleteCRL(crlID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }

        #endregion

        #region 8-1-2
        [Test(Name = "Delete CRL",
            Id = "8-1-2",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_BASED_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new [] { Feature.AdvancedSecurity, Feature.CRLs },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new [] { Functionality.DeleteCRL, Functionality.GetAllCRLs })]
        public void DeleteCRLTest()
        {
            RunTest(() =>
                    {
                        var crl = this.HelperCreateCRLA37();

                        var crlID = this.UploadCRL(crl.GetEncoded(), AdvancedSecurityExtensions.defaultCRLAlias);

                        this.DeleteCRL(crlID);

                        var crls = this.GetAllCRLs();
                        Assert(!crls.Any(c => c.CRLID == crlID),
                               string.Format("The CRL with CRLID = '{0}' is present in GetAllCRLs but it was removed before", crlID),
                               string.Format("Checking CRL with CRLID = '{0}' is successfully removed", crlID));
                    });
        }
        #endregion

        #region 8-1-3
        [Test(Name = "Get CRL",
            Id = "8-1-3",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_BASED_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.CRLs },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCRL })]
        public void GetCRLTest()
        {
            var crlID = string.Empty;

            RunTest(() =>
                    {
                        var crl = this.HelperCreateCRLA37();

                        crlID = this.UploadCRL(crl.GetEncoded(), AdvancedSecurityExtensions.defaultCRLAlias);

                        var justUploadedCrl = this.GetCRL(crlID);
                        Assert(null != justUploadedCrl,
                               string.Format("The results received via UploadCRL and GetAllCRLs are inconsistent: GetAllCRLs returned no CRL with CRLID = '{0}'", crlID),
                               "Check consistency of CRLID");

                        Assert(justUploadedCrl.CRLID == crlID,
                               string.Format("The result received via GetCRL is inconsistent: CRL with CRLID = '{0}' was requested but the returned one has CRLID = '{1}'", crlID, justUploadedCrl.CRLID),
                               "Check consistency of CRLID");

                        Assert(justUploadedCrl.Alias == AdvancedSecurityExtensions.defaultCRLAlias,
                               string.Format("The alias of received CRL with CRLID = '{0}' has unexpected value '{1}'. Expected: '{2}'.", crlID, justUploadedCrl.Alias, AdvancedSecurityExtensions.defaultCRLAlias),
                               "Check alias of received CRL");


                        Assert(crl.GetEncoded().SequenceEqual(justUploadedCrl.CRLContent),
                               "The CRL received via GetCRL is different from uploaded via UploadCRL",
                               "Check consistency of CRLs");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(crlID))
                                               this.DeleteCRL(crlID);
                                       });

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 8-1-4
        [Test(Name = "Create certification path validation policy",
            Id = "8-1-4",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_BASED_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new [] { Feature.AdvancedSecurity, Feature.CertificationPathValidationPolicies },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new [] { Functionality.CreateCertPathValidationPolicy, Functionality.GetAllCertPathValidationPolicies })]
        public void CreateCertificationPathValidationPolicyTest()
        {
            var keyPairID = string.Empty;
            var certificateID = string.Empty;
            var certificationPathID = string.Empty;
            var certificationPathValidationPolicyID = string.Empty;

            RunTest(() =>
                    {
                        CertPathValidationPolicy certPathValidationPolicy;
                        this.HelperCreateCertPathValidationPolicyA42(out keyPairID, out certificateID, out certificationPathID, out certPathValidationPolicy);
                        certificationPathValidationPolicyID = certPathValidationPolicy.CertPathValidationPolicyID;


                        var validationPolicies = this.GetAllCertPathValidationPolicies();
                        var justUploadedValidationPolicy = validationPolicies.FirstOrDefault(p => p.CertPathValidationPolicyID == certificationPathValidationPolicyID);

                        Assert(null != justUploadedValidationPolicy,
                               string.Format("The results received via CreateCertPathValidationPolicy and GetAllCertPathValidationPolicies are inconsistent: GetAllCertPathValidationPolicies returned no Validation Policy with CertPathValidationPolicyID = '{0}'", 
                                             certificationPathValidationPolicyID),
                               "Check consistency of CertPathValidationPolicyID");

                        var logger = new TabulatedStringBuilder();
                        Assert(AdvancedSecurityExtensions.EqualCertPathValidationPolicy(certPathValidationPolicy, justUploadedValidationPolicy, logger,
                                                                                        "CreateCertPathValidationPolicy", "GetAllCertPathValidationPolicies"),
                               logger.ToStringTrimNewLine(),
                               "Checking received Validation Policy item");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certificationPathValidationPolicyID))
                                               this.DeleteCertPathValidationPolicy(certificationPathValidationPolicyID);
                                       });

                        if (string.IsNullOrEmpty(certificationPathID))
                        {
                            this.AllowFaultStep(() =>
                                {
                                    if (!string.IsNullOrEmpty(certificateID))
                                        this.DeleteCertificate(certificateID);
                                });

                            this.AllowFaultStep(() =>
                                {
                                    if (!string.IsNullOrEmpty(keyPairID))
                                        this.DeleteRSAKeyPair(keyPairID);
                                });
                        }
                        else
                            this.AllowFaultStep(() => this.DeleteCertificationPathA35(keyPairID, certificationPathID));

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 8-1-5
        [Test(Name = "Get Certification Path Validation Policy",
            Id = "8-1-5",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_BASED_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.CertificationPathValidationPolicies },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCertPathValidationPolicy, Functionality.GetCertPathValidationPolicy })]
        public void GetCertificationPathValidationPolicyTest()
        {
            var keyPairID = string.Empty;
            var certificateID = string.Empty;
            var certificationPathID = string.Empty;
            var certificationPathValidationPolicyID = string.Empty;

            RunTest(() =>
                    {
                        CertPathValidationPolicy originalValidationPolicy;
                        this.HelperCreateCertPathValidationPolicyA42(out keyPairID, out certificateID, out certificationPathID, out originalValidationPolicy);
                        certificationPathValidationPolicyID = originalValidationPolicy.CertPathValidationPolicyID;

                        var receivedValidationPolicy = this.GetCertPathValidationPolicy(certificationPathValidationPolicyID);

                        var logger = new TabulatedStringBuilder();
                        Assert(AdvancedSecurityExtensions.EqualCertPathValidationPolicy(originalValidationPolicy, receivedValidationPolicy, logger,
                                                                                        "CreateCertPathValidationPolicy", "GetCertPathValidationPolicy"),
                               logger.ToStringTrimNewLine(),
                               "Checking received Validation Policy item");
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certificationPathValidationPolicyID))
                                               this.DeleteCertPathValidationPolicy(certificationPathValidationPolicyID);
                                       });

                        if (string.IsNullOrEmpty(certificationPathID))
                        {
                            this.AllowFaultStep(() =>
                                {
                                    if (!string.IsNullOrEmpty(certificateID))
                                        this.DeleteCertificate(certificateID);
                                });

                            this.AllowFaultStep(() =>
                                {
                                    if (!string.IsNullOrEmpty(keyPairID))
                                        this.DeleteRSAKeyPair(keyPairID);
                                });
                        }
                        else
                            this.AllowFaultStep(() => this.DeleteCertificationPathA35(keyPairID, certificationPathID));

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #region 8-1-6
        [Test(Name = "Delete Certification Path Validation Policy",
            Id = "8-1-6",
            Category = Category.ADVANCED_SECURITY,
            Path = PATH_CERTIFICATE_BASED_CLIENT_AUTHENTICATION,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AdvancedSecurity, Feature.CertificationPathValidationPolicies },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCertPathValidationPolicy, Functionality.GetAllCertPathValidationPolicies })]
        public void DeleteCertificationPathValidationPolicyTest()
        {
            var keyPairID = string.Empty;
            var certificateID = string.Empty;
            var certificationPathID = string.Empty;
            var certificationPathValidationPolicyID = string.Empty;

            RunTest(() =>
                    {
                        CertPathValidationPolicy originalValidationPolicy;
                        this.HelperCreateCertPathValidationPolicyA42(out keyPairID, out certificateID, out certificationPathID, out originalValidationPolicy);
                        certificationPathValidationPolicyID = originalValidationPolicy.CertPathValidationPolicyID;

                        this.DeleteCertPathValidationPolicy(originalValidationPolicy.CertPathValidationPolicyID);
                        certificationPathValidationPolicyID = string.Empty;

                        var validationPolicies = this.GetAllCertPathValidationPolicies();
                        Assert(!validationPolicies.Any(p => p.CertPathValidationPolicyID == originalValidationPolicy.CertPathValidationPolicyID),
                               string.Format("The Certification Path Validation Policy with CertPathValidationPolicyID = '{0}' is present in GetAllCertPathValidationPolicies but it was removed before", originalValidationPolicy.CertPathValidationPolicyID),
                               string.Format("Checking Certification Path Validation Policy with CertPathValidationPolicyID = '{0}' is successfully removed", originalValidationPolicy.CertPathValidationPolicyID));
                    },
                    () =>
                    {
                        this.AllowFaultStep(() =>
                                       {
                                           if (!string.IsNullOrEmpty(certificationPathValidationPolicyID))
                                               this.DeleteCertPathValidationPolicy(certificationPathValidationPolicyID);
                                       });

                        if (string.IsNullOrEmpty(certificationPathID))
                        {
                            this.AllowFaultStep(() =>
                                {
                                    if (!string.IsNullOrEmpty(certificateID))
                                        this.DeleteCertificate(certificateID);
                                });

                            this.AllowFaultStep(() =>
                                {
                                    if (!string.IsNullOrEmpty(keyPairID))
                                        this.DeleteRSAKeyPair(keyPairID);
                                });
                        }
                        else
                            this.AllowFaultStep(() => this.DeleteCertificationPathA35(keyPairID, certificationPathID));

                        this.FinishRestoreSettings();
                    });
        }
        #endregion

        #endregion
    }
}
