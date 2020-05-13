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
                "Sending request to NVT with omitted Nonce",
                "Sender/NotAuthorized",
                true);

                spoiler.NodesToDelete.Clear();
                spoiler.NodesToDelete.Add("/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsu:Created");
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to NVT with omitted Created",
                "Sender/NotAuthorized",
                true);

                spoiler.NodesToDelete.Clear();
                spoiler.AttributesToDelete = new Dictionary<string, string>();
                spoiler.AttributesToDelete["/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsse:Password"] = "Type";
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to NVT with omitted Password/Type",
                "Sender/NotAuthorized",
                true);

                ResetBreakingBehaviour();
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending valid request to NVT");

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
    }
}
