

        [Test (Name ="5.1 NVT ",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void Test()
        {
            try{
            {
                BeginTest()

                SystemReboot();
                HelloType Hello = GetHello();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.2 NVT ",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void Test()
        {
            try{
            {
                BeginTest()

                SystemReboot();
                HelloType Hello = GetHello();
                Assert(Hello.Types == "dn:NetworkVideoTransmitter", "Wrong device", "Hello Message validation");
                Assert(Hello.Scopes != null, "No Scopes", "Hello Message validation");
                string[] Scopes = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Location,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Type};
                Assert(Hello.Scopes.Text == Scopes, "Wrong Scopes", "Hello Message validation");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.3 NVT ",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void Test()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = GetScopes();
                Assert(Scopes != null, "No Scopes", "GetScopes validation");
                ProbeMatchesType match = Probe("dn:NetworkVideoTransmitter", Scope);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.4 NVT WITH DEVICE INFORMATION OMMITED",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void WithDeviceInformationOmmitedTest()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { "" };
                ProbeMatchesType match = Probe("", Scope);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.5 NVT ",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void Test()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { "blah" };
                try { 
                ProbeMatchesType match = Probe("wrongTYPE", Scope);
                }
                catch (FaultException exc) {
                    StepPassed(); };
                catch (TimeoutException exc) {
                    StepPassed(); };

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.6 NVT ",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void Test()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = GetScopes();
                Assert(Scopes != null, "No Scopes", "GetScopes validation");
                ProbeMatchesType match = Probe("dn:NetworkVideoTransmitter", Scope);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.7 NVT DEICE INFORMATION OMITTED",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeiceInformationOmittedTest()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { "" };
                ProbeMatchesType match = Probe("", Scope);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.8 NVT INVALID DEICE INFORMATION",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void InvalidDeiceInformationTest()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { "blah" };
                try { 
                ProbeMatchesType match = Probe("wrongTYPE", Scope);
                }
                catch (FaultException exc) {
                    StepPassed(); };
                catch (TimeoutException exc) {
                    StepPassed(); };

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.9 NVT SCOPES CONFIGURATION",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void ScopesConfigurationTest()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = GetScopes();
                Assert(Scopes != null, "No Scopes", "GetScopes validation");
                SetScopes(newScopes);
                AddScopes(new string[] { RemoteDiscovery.Constants.ScopeTypePrefix_Name + TMP_SCOPE_STRING } );
                HelloType Hello = GetHello();
                Assert(Hello.Types == "dn:NetworkVideoTransmitter", "Wrong device", "Hello Message validation");
                Assert(Hello.Scopes != null, "No Scopes", "Hello Message validation");
                string[] Scopes = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Location,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Type};
                Assert(Hello.Scopes.Text == Scopes, "Wrong Scopes", "Hello Message validation");
                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { Parameters.Temporary_String };
                ProbeMatchesType match = Probe("dn:NetworkVideoTransmitter", Scope);
                DeleteScopes(new string[] { RemoteDiscovery.Constants.ScopeTypePrefix_Name + TMP_SCOPE_STRING });
                HelloType Hello = GetHello();
                Assert(Hello.Types == "dn:NetworkVideoTransmitter", "Wrong device", "Hello Message validation");
                Assert(Hello.Scopes != null, "No Scopes", "Hello Message validation");
                string[] Scopes = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Location,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Type};
                Assert(Hello.Scopes.Text == Scopes, "Wrong Scopes", "Hello Message validation");
                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { Parameters.Temporary_String };
                ProbeMatchesType match = Probe("dn:NetworkVideoTransmitter", Scope);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.10 NVT BYE MESSAGE",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void ByeMessageTest()
        {
            try{
            {
                BeginTest()

                SystemReboot();
                ByeType Bye = GetBye();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="5.11 NVT SOAP FAULT DETECTION",
            Path="Device Discovery",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void SoapFaultDetectionTest()
        {
            try{
            {
                BeginTest()

                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { "" };
                        Scope.MatchBy = "BADTYPE";
                try { 
                ProbeMatchesType match = Probe("dn:NetworkVideoTransmitter", Scope);
                }
                catch (FaultException exc) {
                    StepPassed(); };
                catch (TimeoutException exc) {
                    StepPassed(); };

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.1 NVT WSDL URL",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void WSDLURLTest()
        {
            try{
            {
                BeginTest()

                GetWsdlUrl();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.2 NVT ALL CAPABILITIES",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void AllCapabilitiesTest()
        {
            try{
            {
                BeginTest()

                Capabilities capabilities = GetCapabilities( new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.All } );
                Assert(!(capabilities == null), "Required capabilities not found", "Check  capabilities");
                Assert(!(capabilities.Media == null), "Media capabilities not found", "Check Media capabilities");
                Assert(!(capabilities.Device == null), "Device capabilities not found", "Check Device capabilities");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.3 NVT DEVICE CAPABILITIES",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceCapabilitiesTest()
        {
            try{
            {
                BeginTest()

                Capabilities capabilities = GetCapabilities(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Device });
                Assert(!(capabilities == null), "Required capabilities not found", "Check  capabilities");
                Assert(!(capabilities.Device == null), "Device capabilities not found", "Check Device capabilities");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.4 NVT MEDIA CAPABILITIES",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void MediaCapabilitiesTest()
        {
            try{
            {
                BeginTest()

                Capabilities capabilities = GetCapabilities(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Media });
                Assert(!(capabilities == null), "Required capabilities not found", "Check  capabilities");
                Assert(!(capabilities.Media == null), "Media capabilities not found", "Check Media capabilities");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.5 NVT SERVICE CAPABILITIES",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void ServiceCapabilitiesTest()
        {
            try{
            {
                BeginTest()

                Capabilities capabilities = GetCapabilities(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Analytics });
                Assert(!(capabilities == null), "capabilities not found", "Check  capabilities");
                Assert(!(capabilities.Analytics == null), "Analytics not capabilities found", "Check Analytics capabilities");
                Capabilities capabilities = GetCapabilities(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Events });
                Assert(!(capabilities == null), "capabilities not found", "Check  capabilities");
                Assert(!(capabilities.Events != null), "Events capabilities found", "Check  capabilities");
                Capabilities capabilities = GetCapabilities(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Imaging });
                Assert(!(capabilities == null), "capabilities not found", "Check  capabilities");
                Assert(!(capabilities.Imaging != null), "Imaging capabilities found", "Check  capabilities");
                Capabilities capabilities = GetCapabilities(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.PTZ });
                Assert(!(capabilities == null), "capabilities not found", "Check  capabilities");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.6 NVT SOAP FAULT DETECTION",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void SoapFaultDetectionTest()
        {
            try{
            {
                BeginTest()

                Capabilities capabilities = GetCapabilities(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.PTZ });

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.7 NVT DEVICE HOSTNAME",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceHostnameTest()
        {
            try{
            {
                BeginTest()

                HostnameInformation hostnameInformation = GetHostname();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.8 NVT DEVICE HOSTNAME SETUP",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceHostnameSetupTest()
        {
            try{
            {
                BeginTest()

                SetHostname("Test");
                HostnameInformation hostnameInformation = GetHostname();
                Assert(HostnameInformation.Name.Equals("Test"), "Name not set", "Host name validation");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.9 NVT DEVICE HOSTNAME SETUP WITH INVALID NAME",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceHostnameSetupWithInvalidNameTest()
        {
            try{
            {
                BeginTest()

                try { 
                SetHostname("Test#$%");
                }
                catch (FaultException exc) {
                    StepPassed(); };
                catch (TimeoutException exc) {
                    StepPassed(); };
                HostnameInformation hostnameInformation = GetHostname();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.10 NVT DEVICE DNS",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceDNSTest()
        {
            try{
            {
                BeginTest()

                DNSInformation dnsInformation = GetDNS();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.11 NVT DEVICE DNS SETUP",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceDNSSetupTest()
        {
            try{
            {
                BeginTest()

                    DeviceManagement.IPAddress[] DNS_Server = new DeviceManagement.IPAddress[1];
                    DNS_Server[0] = new DeviceManagement.IPAddress();
                    DNS_Server[0].IPv4Address = "10.1.1.1";
                    DNS_Server[0].Type = DeviceManagement.IPType.IPv4;
                SetDNS(DNS_Server, null);
                DNSInformation dnsInformation = GetDNS();
                Assert(!(dnsInformation.FromDHCP == null), "FromDHCP is null", "Check FromDHCP settings");
                Assert(!(dnsInformation.DNSManual == null), "DNSManual is null", "Check DNSManual settings");
                bool foundEntry = false;
                for (int x = 0; x < DNSInformation.DNSManual.Length; x++)
                {
                    if ((dnsInformation.DNSManual[x].IPv4Address.Equals("10.1.1.1")) &&
                        (dnsInformation.DNSManual[x].Type == DeviceManagement.IPType.IPv4))
                         foundEntry = true;
                }
                Assert(foundEntry, "IP Address", "Check DNSManual settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.12 NVT DEVICE DNS SETUP WITH INVALID DNS",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceDNSSetupWithInvalidDNSTest()
        {
            try{
            {
                BeginTest()

                    DeviceManagement.IPAddress[] DNS_Server = new DeviceManagement.IPAddress[1];
                    DNS_Server[0] = new DeviceManagement.IPAddress();
                try { 
                    DNS_Server[0].IPv4Address = "10.1.1.255";
                    DNS_Server[0].Type = DeviceManagement.IPType.IPv4;
                SetDNS(DNS_Server, null);
                }
                catch (FaultException exc) {
                    StepPassed(); };
                catch (TimeoutException exc) {
                    StepPassed(); };
                DNSInformation dnsInformation = GetDNS();
                Assert(!(dnsInformation.DNSManual != null)), "DNSManual", "Check DNSManual settings");
                bool foundEntry = false;
                for (int x = 0; x < DNSInformation.DNSManual.Length; x++)
                {
                    if ((dnsInformation.DNSManual[x].IPv4Address.Equals("10.1.1.255")) &&
                        (dnsInformation.DNSManual[x].Type == DeviceManagement.IPType.IPv4))
                         foundEntry = true;
                }
                Assert(!foundEntry, "Wrong IP Address set", "Check DNSManual settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.13 NVT DEVICE NTP",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceNTPTest()
        {
            try{
            {
                BeginTest()

                NTPInformation ntpInformation = GetNTP();
                Assert((ntpInformation != null), " != null)", "Check  settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.14 NVT DEVICE NTP SETUP",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceNTPSetupTest()
        {
            try{
            {
                BeginTest()

                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.IPv4;
                    NTP[0].IPv4Address = "10.1.1.1";
                    //NTP[0].DNSname = "test";  removed in test version 1.01
                SetNTP(false, NTP);
                NTPInformation ntpInformation = GetNTP();
                Assert((ntpInformation != null), " != null)", "Check  settings");
                                NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;
                Assert((ntpInformation.FromDHCP != false), "FromDHCP not set", "Check  settings");
                Assert((ntpInformation.NTPManual == null), "NTPManual not correct", "Check NTPManual settings");
                                        if (GNTPR.NTPInformation.NTPManual[x].IPv4Address == NTP[0].IPv4Address)
                Assert((ntpInformation.NTPManual[x].IPv4Address == NTP[0].IPv4Address), "NTPManual[x].IPv4Address", "Check NTPManual[x].IPv4Address settings");
                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.DNS;                    
                    NTP[0].DNSname = "test";  
                SetNTP(false, NTP);
                NTPInformation ntpInformation = GetNTP();
                Assert((ntpInformation != null), " != null)", "Check  settings");
                                NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;
                Assert((ntpInformation.FromDHCP != false), "FromDHCP not set", "Check  settings");
                Assert((ntpInformation.NTPManual == null), "NTPManual not correct", "Check NTPManual settings");
                                        if (GNTPR.NTPInformation.NTPManual[x].DNSname == NTP[0].DNSname)
                Assert((ntpInformation.NTPManual[x].DNSname == NTP[0].DNSname), "NTPManual[x].DNSname", "Check NTPManual[x].DNSname settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.15 NVT DEVICE NTP SETUP WITH INVALID IP ADDRESS",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceNTPSetupWithInvalidIPAddressTest()
        {
            try{
            {
                BeginTest()

                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.IPv4;
                try { 
                    NTP[0].IPv4Address = "10.1.1.255";
                    //NTP[0].DNSname = "test"; //removed in test version 1.01
                SetNTP(false, NTP);
                }
                catch (FaultException exc) {
                    StepPassed(); };
                catch (TimeoutException exc) {
                    StepPassed(); };
                NTPInformation ntpInformation = GetNTP();
                Assert((ntpInformation != null), " != null)", "Check  settings");
                Assert((ntpInformation == null), "NTPInformation not complete", "Check  settings");
                Assert((ntpInformation.NTPManual == null), "No NTP servers manually set - OK", "Check NTPManual settings");
                                        NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;
                                                if ((GNTPR.NTPInformation.NTPManual[x].IPv4Address != null) && (GNTPR.NTPInformation.NTPManual[x].IPv4Address.Equals(NTP[0].IPv4Address)))
                Assert((ntpInformation.NTPManual[x].IPv4Address == null), "NTPManual[x].IPv4Address", "Check NTPManual[x].IPv4Address settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.16 NVT DEVICE NTP SETUP WITH INVALID NAME",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceNTPSetupWithInvalidNameTest()
        {
            try{
            {
                BeginTest()

                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.DNS;
                    //NTP[0].IPv4Address = "10.1.1.1"; // removed in test version 1.01
                try { 
                    NTP[0].DNSname = "test#$%";
                SetNTP(false, NTP);
                }
                catch (FaultException exc) {
                    StepPassed(); };
                catch (TimeoutException exc) {
                    StepPassed(); };
                NTPInformation ntpInformation = GetNTP();
                Assert((ntpInformation != null), " != null)", "Check  settings");
                Assert((ntpInformation == null), "NTPInformation not complete", "Check  settings");
                Assert((ntpInformation.NTPManual == null), "No NTP servers manually set - OK", "Check NTPManual settings");
                                        NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;
                                                    (GNTPR.NTPInformation.NTPManual[x].DNSname.Equals(NTP[0].DNSname)))

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.17 NVT DEVICE INFORMATION REQUEST",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceInformationRequestTest()
        {
            try{
            {
                BeginTest()

                GetDeviceInformation();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.18 NVT DEVICE SYSTEM DATE AND TIME REQUEST",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceSystemDateAndTimeRequestTest()
        {
            try{
            {
                BeginTest()

                SystemDateTime SystemDateAndTime = GetSystemDateAndTime();
                Assert((SystemDateAndTime == null), "System Date and Time not received", "Check  settings");
                Assert((SystemDateAndTime.DateTimeType == null), "DateTimeType not received", "Check DateTimeType settings");
                Assert((SystemDateAndTime.DaylightSavings == null), "DaylightSavings not received", "Check DaylightSavings settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.19 NVT DEVICE SYSTEM DATE AND TIME SETUP",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceSystemDateAndTimeSetupTest()
        {
            try{
            {
                BeginTest()

                SetSystemDateAndTime(SetSystemDateAndTimeRequest);
                SystemDateTime SystemDateAndTime = GetSystemDateAndTime();
                Assert((SystemDateAndTime == null), "System Date and Time not received", "Check  settings");
                Assert((SystemDateAndTime.DateTimeType == null), "DateTimeType", "Check DateTimeType settings");
                Assert((SystemDateAndTime.DaylightSavings == null), "DaylightSavings", "Check DaylightSavings settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.20 NVT DEVICE SYSTEM DATE AND TIME SET WITH INVALID TIMEZONE",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceSystemDateAndTimeSetWithInvalidTimezoneTest()
        {
            try{
            {
                BeginTest()

                SetSystemDateAndTime(SetSystemDateAndTimeRequest);
                SystemDateTime SystemDateAndTime = GetSystemDateAndTime();
                Assert((SystemDateAndTime == null), "System Date and Time not received", "Check  settings");
                Assert((SystemDateAndTime.DateTimeType == null), "DateTimeType not received", "Check DateTimeType settings");
                Assert((SystemDateAndTime.DaylightSavings == null), "DaylightSavings not received", "Check DaylightSavings settings");
                Assert((SystemDateAndTime.TimeZone == null), "TimeZone not received", "Check TimeZone settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.21 NVT DEVICE SYSTEM DATE AND TIME SETUP WITH INVALID DATE",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceSystemDateAndTimeSetupWithInvalidDateTest()
        {
            try{
            {
                BeginTest()

                SetSystemDateAndTime(SetSystemDateAndTimeRequest);
                SystemDateTime SystemDateAndTime = GetSystemDateAndTime();
                Assert((SystemDateAndTime == null), "System Date and Time not received", "Check  settings");
                Assert((SystemDateAndTime.DateTimeType == null), "DateTimeType not received", "Check DateTimeType settings");
                Assert((SystemDateAndTime.DaylightSavings == null), "DaylightSavings not received", "Check DaylightSavings settings");
                Assert((SystemDateAndTime.TimeZone == null), "TimeZone not received", "Check TimeZone settings");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.22 NVT RESTORE DEVICE TO FACTORY DEFAULT",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void RestoreDeviceToFactoryDefaultTest()
        {
            try{
            {
                BeginTest()

                SetSystemFactoryDefault(DeviceManagement.FactoryDefaultType.Hard);
                HelloType Hello = GetHello();
                Assert(Hello.Types == "dn:NetworkVideoTransmitter", "Wrong device", "Hello Message validation");
                Assert(Hello.Scopes != null, "No Scopes", "Hello Message validation");
                string[] Scopes = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Location,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Type};
                Assert(Hello.Scopes.Text == Scopes, "Wrong Scopes", "Hello Message validation");

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.23 NVT RESTORE DEVICE TO FACTORY DEFAULT (SOFT)",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void RestoreDeviceToFactoryDefault(Soft)Test()
        {
            try{
            {
                BeginTest()

                SetSystemFactoryDefault(DeviceManagement.FactoryDefaultType.Soft);
                ScopesType Scope = new ScopesType;
                            Scope.Text = new string[] { "" };
                ProbeMatchesType match = Probe("dn:NetworkVideoTransmitter", Scope);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="6.24 NVT DEVICE RESET",
            Path="Device Management",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceResetTest()
        {
            try{
            {
                BeginTest()

                SystemReboot();
                HelloType Hello = GetHello();
                Assert(Hello.Types == "dn:NetworkVideoTransmitter", "Wrong device", "Hello Message validation");
                Assert(Hello.Scopes != null, "No Scopes", "Hello Message validation");
                string[] Scopes = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Location,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                RemoteDiscovery.Constants.ScopeTypePrefix_Type};
                Assert(Hello.Scopes.Text == Scopes, "Wrong Scopes", "Hello Message validation");
                ScopesType Scope = new ScopesType;
                        Scope.Text = new string[] { "" };
                ProbeMatchesType match = Probe("dn:NetworkVideoTransmitter", Scope);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="7.1 NVT PROFILE CONFIGURATION",
            Path="Media",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void ProfileConfigurationTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="7.2 NVT DYNAMIC PROFILE CONFIGURATION",
            Path="Media",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void DynamicProfileConfigurationTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                GetVideoEncoderConfiguration
                Media_CreateProfile("testprofile" + (rand.Next() % 100).ToString(), "testprofile" + (rand.Next() % 100).ToString());
                Media_AddVideoSourceConfiguration(tmpProfile.VideoSourceConfiguration.token, "testprofile" + (rand.Next() % 100).ToString());
                Media_AddVideoEncoderConfiguration(tmpProfile.VideoEncoderConfiguration.token, "testprofile" + (rand.Next() % 100).ToString());
                Media_GetProfile("testprofile" + (rand.Next() % 100).ToString());
                Media_RemoveVideoEncoderConfiguration("testprofile" + (rand.Next() % 100).ToString());
                Media_RemoveVideoSourceConfiguration ("testprofile" + (rand.Next() % 100).ToString());
                Media_DeleteProfile("testprofile" + (rand.Next() % 100).ToString());
                Media_GetProfile("testprofile" + (rand.Next() % 100).ToString());

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="7.3 NVT JPEG VIDEO ENCODER CONFIGURATION",
            Path="Media",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void JPEGVideoEncoderConfigurationTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_GetVideoEncoderConfiguration(Parameters.Profile_Token);
                Media_SetVideoEncoderConfiguration(SVEC);
                Media_GetVideoEncoderConfiguration(SVEC.Configuration.token);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="7.4 NVT MEDIA STREAM URI CONFIGURATION RTP UDP UNICAST",
            Path="Media",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void MediaStreamURIConfigurationRTPUDPUnicastTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_SetVideoEncoderConfiguration(SVEC);
                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="7.5 NVT MEDIA STREAM URI CONFIGURATION RTP RTSP HTTP",
            Path="Media",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void MediaStreamURIConfigurationRTPRTSPHTTPTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_SetVideoEncoderConfiguration(SVEC);
                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="7.6 NVT MEDIA STREAM URI INVLAID CONFIGURATION",
            Path="Media",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void MediaStreamURIInvlaidConfigurationTest()
        {
            try{
            {
                BeginTest()

                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="7.7 NVT MEDIA STREAM INVALID TRANSPORT",
            Path="Media",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void MediaStreamInvalidTransportTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="8.1 NVT RTSP TCP",
            Path="Real Time Streaming",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void RTSPTCPTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_SetVideoEncoderConfiguration(SVEC);
                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="8.2 NVT RTP UDP UNICAST",
            Path="Real Time Streaming",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void RTPUDPUnicastTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_SetVideoEncoderConfiguration(SVEC);
                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="8.3 NVT RTP RTSP HTTP",
            Path="Real Time Streaming",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void RTPRTSPHTTPTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_SetVideoEncoderConfiguration(SVEC);
                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }


        [Test (Name ="8.4 NVT KEEPALIVE",
            Path="Real Time Streaming",
            Version=1.01,
            RequirementLevel = RequirementLevel.Must)]
        public void KeepaliveTest()
        {
            try{
            {
                BeginTest()

                Media_GetProfiles();
                Media_SetVideoEncoderConfiguration(SVEC);
                Media_GetStreamUri(GSUri);

                EndTest(Tests.Common.Trace.TestStatus.Passed);
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }

        }
