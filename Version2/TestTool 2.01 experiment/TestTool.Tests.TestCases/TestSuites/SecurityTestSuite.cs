///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Device;
using TestTool.Tests.TestCases;
using System.Reflection;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class SecurityTestSuite : Base.DeviceManagementTest
    {
        private const string _testUserName = "OnvifTest1";

        public SecurityTestSuite(TestLaunchParam param)
            : base(param)
        {
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
                }
                catch(Exception e)
                {
                    LogStepEvent("Error: " + e.Message);
                }
            }, "Removing test users");
        }
        [Test(Name = "USER TOKEN PROFILE",
            Interactive = true,
            Path = "Security Test Cases",
            Order = "11.01",
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void UserTokenProfileTest()
        {
            bool createUser = false;
            RunTest(() =>
            {
                if (string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_username))
                {
                    throw new AssertException("Please enter valid credentials on management tab");
                }

                MessageSpoiler spoiler = new MessageSpoiler();

                Dictionary<string, string> namespaces = new Dictionary<string, string>();
                namespaces.Add("s", "http://www.w3.org/2003/05/soap-envelope");
                namespaces.Add("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                namespaces.Add("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

                string operationName = _operator.GetSecureAPI();
                createUser = operationName == "CreateUsers";

                spoiler.Namespaces = namespaces;
                spoiler.NodesToDelete = new List<string>();
                SetBreakingBehaviour(spoiler);

                spoiler.NodesToDelete.Add("/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsse:Nonce");
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to the Device with omitted Nonce",
                "Sender/NotAuthorized",
                true);

                spoiler.NodesToDelete.Clear();
                spoiler.NodesToDelete.Add("/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsu:Created");
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to the Device with omitted Created",
                "Sender/NotAuthorized",
                true);

                spoiler.NodesToDelete.Clear();
                spoiler.AttributesToDelete = new Dictionary<string, string>();
                spoiler.AttributesToDelete["/s:Envelope/s:Header/wsse:Security/wsse:UsernameToken/wsse:Password"] = "Type";
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending request to the Device with omitted Password/Type",
                "Sender/NotAuthorized",
                true);

                ResetBreakingBehaviour();
                RunStep(() =>
                {
                    InvokeOperation(operationName);
                },
                "Sending valid request to the Device");

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
