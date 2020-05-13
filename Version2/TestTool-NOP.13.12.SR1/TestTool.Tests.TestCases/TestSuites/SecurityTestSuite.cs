///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.MessageModification;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.Reflection;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class SecurityTestSuite : Base.DeviceManagementTest
    {
        private const string _testUserName = "OnvifTest1";

        protected string _secureOperation;

        public SecurityTestSuite(TestLaunchParam param)
            : base(param)
        {
            _secureOperation = param.SecureMethod;

        }

        protected void InvokeOperation(string operation)
        {
            MethodInfo method = Client.GetType().GetMethod(operation);
            object[] parameters = null;
            if (operation == "GetDeviceInformation")
            {
                parameters = new string[4];
            }
            else if (operation == "GetCapabilities")
            {
                parameters = new object[] { new CapabilityCategory[] { CapabilityCategory.All } };
            }
            else if (operation == "CreateUsers")
            {
                User user = new User() { Username = _testUserName, Password = "OnvifTest123", UserLevel = UserLevel.Operator };
                parameters = new object[] { new User[] { user } };
            }

            try
            {
                method.Invoke(Client, parameters);
                DoRequestDelay();
            }
            catch (System.Exception ex)
            {
                if(ex.InnerException != null)            	
                {
                    throw ex.InnerException;
                }
                throw;
            }
        }
        protected void DeleteTestUser()
        {
            RunStep(() =>
            {
                try
                {
                    Client.DeleteUsers(new string[] { _testUserName });
                    DoRequestDelay();
                }
                catch(Exception e)
                {
                    LogStepEvent("Error: " + e.Message);
                }
            }, "Removing test users");
        }
        [Test(Name = "USER TOKEN PROFILE",
            Path = "Security Test Cases",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.SECURITY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures =  new Feature[]{ Feature.WSU },
            FunctionalityUnderTest = new Functionality[]{Functionality.WsSecurity})]
        public void UserTokenProfileTest()
        {
            bool createUser = false;
            RunTest(() =>
            {
                Assert(!(string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_username)), 
                    "This test cannot be performed without credentials supplied", 
                    "Check if credentials were defined");

                if (_credentialsProvider != null)
                {
                    _credentialsProvider.Security = Security.WS;
                }

                MessageSpoiler spoiler = new MessageSpoiler();

                Dictionary<string, string> namespaces = new Dictionary<string, string>();
                namespaces.Add("s", "http://www.w3.org/2003/05/soap-envelope");
                namespaces.Add("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                namespaces.Add("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

                string operationName = _secureOperation; 
                createUser = operationName == "CreateUsers";

                spoiler.Namespaces = namespaces;
                spoiler.NodesToDelete = new List<string>();
                SetBreakingBehaviour(spoiler);

                spoiler.NodesToDelete.Add("/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsse:Nonce");
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to the DUT with omitted Nonce",
                "Sender/NotAuthorized",
                true);

                spoiler.NodesToDelete.Clear();
                spoiler.NodesToDelete.Add("/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsu:Created");
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to the DUT with omitted Created",
                "Sender/NotAuthorized",
                true);

                spoiler.NodesToDelete.Clear();
                spoiler.AttributesToDelete = new Dictionary<string, string>();
                spoiler.AttributesToDelete["/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsse:Password"] = "Type";
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to the DUT with omitted Password/Type",
                "Sender/NotAuthorized",
                true);

                ResetBreakingBehaviour();
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending valid request to the DUT");

                if (createUser)
                {
                    DeleteTestUser();
                }
            },
            () =>
            {
                if (createUser)
                {
                    DeleteTestUser();
                }
            });
        }

        [Test(Name = "DIGEST AUTHENTICATION",
            Path = "Security Test Cases",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.SECURITY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.Digest },
            FunctionalityUnderTest = new Functionality[] { Functionality.DigestAuthentication })]
        public void DigestAuthenticationTest()
        {
            bool createUser = false;
            RunTest(() =>
            {
                Assert(!(string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_username)),
                    "This test cannot be performed without credentials supplied",
                    "Check if credentials were defined");

                if (_credentialsProvider != null)
                {
                    _credentialsProvider.Security = Security.None;
                }

                string operationName = _secureOperation;
                createUser = operationName == "CreateUsers";

                // try-catch ?

                bool exception = false;
                try
                {
                    RunStep(
                        () => { InvokeOperation(operationName); },
                        string.Format("Invoke {0} without credentials supplied", _secureOperation));
                }
                catch (Exception exc)
                {
                    StepPassed();
                    if (exc is HttpTransport.Interfaces.Exceptions.AccessDeniedException)
                    {
                        // digest 
                        exception = true;
                    }
                }
                finally
                {
                    DoRequestDelay();
                }

                Assert(exception,
                    "No HTTP 401 response received when operation is invoked without credentials",
                    "Check response");

                if (_credentialsProvider != null)
                {
                    _credentialsProvider.Security = Security.Digest;
                }

                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending valid request to the DUT");

                if (createUser)
                {
                    DeleteTestUser();
                }
            },
            () =>
            {
                if (createUser)
                {
                    DeleteTestUser();
                }
            });
        }

        // POSTPONED to projects following Erdinger 

        /*[Test(Name = "DIGEST AUTHENTICATION – INVALID AUTHENTICATION",
            Path = "Security Test Cases",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.SECURITY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.Digest },
            FunctionalityUnderTest = new Functionality[] { Functionality.DigestAuthentication })]*/
        public void DigestAuthenticationParametersTest()
        {
            RunTest(() =>
            {
                Assert(!(string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_username)),
                    "This test cannot be performed without credentials supplied",
                    "Check if credentials were defined");
                

                string operationName = _secureOperation;

                string requestDescription = string.Empty;

                Action checkDigest = 
                    new Action(
                        () =>
                            {
                                bool exception = false;
                                try
                                {
                                    RunStep(
                                        () => { InvokeOperation(operationName); },
                                        string.Format("Invoke {0} {1}", _secureOperation, requestDescription));
                                }
                                catch (Exception exc)
                                {
                                    StepPassed();
                                    if (exc is HttpTransport.Interfaces.Exceptions.AccessDeniedException)
                                    {
                                        // digest 
                                        exception = true;
                                    }
                                }
                                finally
                                {
                                    DoRequestDelay();
                                }

                                Assert(exception,
                                    string.Format("No HTTP 401 response received when operation is invoked {0}", requestDescription),
                                    "Check response");
                            });

                requestDescription = "without credentials supplied";
                _credentialsProvider.Security = Security.None;

                // without credentials at all
                checkDigest();
                
                // with some fields missing
                DigestTestingSettings settings = new DigestTestingSettings();
                _credentialsProvider.DigestTestingSettings = settings;
                _credentialsProvider.Security = Security.DigestTesting;

                settings.UserNameMissing = true;
                requestDescription = "without username included in request";

                checkDigest();

                // realm
                settings.RealmMissing = true;
                settings.UserNameMissing = false;
                requestDescription = "without realm included in request";

                checkDigest();
                
                // nonce

                settings.NonceMissing = true;
                settings.RealmMissing = false;
                requestDescription = "without nonce included in request";

                checkDigest();

                // uri

                settings.UriMissing = true;
                settings.NonceMissing = false;
                requestDescription = "without URI included in request";

                checkDigest();

                // response

                settings.ResponseMissing = true;
                settings.UriMissing = false;
                requestDescription = "without response included in request";

                checkDigest();

            },
            () =>
            {
            });
        }


    }
}
