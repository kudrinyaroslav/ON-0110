using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.TestSuites.ProfileQFake
{
    [TestClass]
    class ProfileQFakeTestSuit: BaseOnvifTest, IDeviceService
    {
        private const string ROOT_PATH = "Profile Q testing preparation";

        public ProfileQFakeTestSuit(TestLaunchParam param) : base(param)
        {}

        #region Onvif Clients

        private ServiceHolder<DeviceClient, Device> m_DeviceClient;

        private OnvifServiceClient<Device, DeviceClient> m_DeviceServiceClient;
        public OnvifServiceClient<Device, DeviceClient> ServiceClient        
        {
            get
            {
                if (!m_DeviceServiceClient.IsInitialized())
                {
                    m_DeviceServiceClient = new OnvifServiceClient<Device, DeviceClient>(this, "Device", this.GetDeviceServiceAddress);
                    m_DeviceServiceClient.InitServiceClient(new [] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });
                }

                return m_DeviceServiceClient;
            }
        }

        public BaseOnvifTest Test { get { return this; } }

        #endregion

        #region Fake Tests
        [Test(Name = "Hard Factory Reset",
              Id = "1-1-1",
              Category = Category.PROFILEQ_TESTING_PREPARATION,
              Path = ROOT_PATH,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              LastChangedIn = "v15.06",
              RequiredFeatures = new [] { Feature.ProfileQSupported })]
        public void HardFactoryResetTest()
        {
            RunTest(() =>
                    {
                        this.SetSystemFactoryDefault(FactoryDefaultType.Hard);

                        this.ReceiveHelloWithNonLinkLocalIPv4AddressFromClientSubnetwork();

                        HelperTimeSynchronizationA6();

                        var adminUser = CreateUserA1(UserLevel.Administrator);

                        Credentials.Username = adminUser.Username;
                        Credentials.Password = adminUser.Password;

                        UpdateSecurity();
                    });
        }
        #endregion

        protected User CreateUserA1(UserLevel lvl, DeviceServiceCapabilities serviceCapabilities = null)
        {
            if (null == serviceCapabilities)
                serviceCapabilities = this.GetServiceCapabilities();

            Assert(serviceCapabilities.Security.MaxUserNameLengthSpecified && serviceCapabilities.Security.MaxPasswordLengthSpecified,
                   "The DUT didn't send Security.MaxUserNameLength and/or Security.MaxPasswordLength capabilitie(s)",
                   "Checking service capabilities Security.MaxUserNameLength and Security.MaxPasswordLength are received");

            var users = this.GetUsers();

            var existedUser = users.FirstOrDefault(u => u.UserLevel == lvl);
            User user = null;

            while (true)
            {
                try
                {
                    if (null != existedUser)
                    {
                        var passwordLength = serviceCapabilities.Security.MaxPasswordLength;
                        user = new User() { Username = existedUser.Username, Password = Extensions.RandomString(passwordLength), UserLevel = lvl, Extension = null };
                        this.SetUser(new[] { user });
                    }
                    else
                    {
                        var usernameLength = serviceCapabilities.Security.MaxUserNameLength;
                        var userName = users.Select(u => u.Username).GetNonMatchingAlphabeticalString(usernameLength);
                        var passwordLength = serviceCapabilities.Security.MaxPasswordLength;
                        user = new User() { Username = userName, Password = Extensions.RandomString(passwordLength), UserLevel = lvl, Extension = null };
                        this.CreateUsers(new[] { user });
                    }
                    break;
                }
                catch (FaultException e)
                {
                    if (!e.IsValidOnvifFault("Sender/OperationProhibited/Password"))
                        //Break cycle if received fault is not "Sender/OperationProhibited/Password"
                        throw;
                    else
                    {
                        LogFault(e);
                        StepPassed();

                        StopRequested();
                    }
                }
            }

            return user;
        }

        protected void HelperTimeSynchronizationA6()
        {
            _security = Security.None;
            _username = _password = null;
            UpdateSecurity();

            this.SetNTP(new NTPInformation() { FromDHCP = true, NTPManual = null });

            var dateTime = this.GetSystemDateAndTime();
            this.SetSystemDateAndTime(new SystemDateTime() { DateTimeType = SetDateTimeType.NTP, 
                                                             DaylightSavings = dateTime.DaylightSavings, 
                                                             TimeZone = dateTime.TimeZone,
                                                             UTCDateTime = null });
        }
    }
}
