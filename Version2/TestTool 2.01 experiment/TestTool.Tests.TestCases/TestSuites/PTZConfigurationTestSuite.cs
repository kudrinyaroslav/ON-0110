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
using TestTool.Proxies.PTZ;
using TestTool.Tests.TestCases;

namespace TestTool.Tests.TestCases.TestSuites
{
//#if FULL
    [TestClass]
//#endif
    public class PTZConfigurationTestSuite : Base.PTZTest
    {
        public PTZConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "PTZ NODES",
            Path = "PTZ\\PTZ Node",
            Order = "10.01.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[]{Feature.PTZ})]
        public void NodesTest()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                Assert((nodes != null) && (nodes.Length > 0), Resources.ErrorNoPTZNode_Text, Resources.StepValidateGetNodes_Title);
                bool valid = true;
                string reason = string.Empty;
                foreach (PTZNode node in nodes)
                {
                    if (!ValidatePTZNode(node, out reason))
                    {
                        valid = false;
                        break;
                    }
                }
                Assert(valid, reason, Resources.StepValidatePTZNodes_Title);
            });
        }

        [Test(Name = "PTZ NODE",
            Path = "PTZ\\PTZ Node",
            Order = "10.01.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void NodeTest()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                Assert((nodes != null)&&(nodes.Length > 0), Resources.ErrorNoPTZNode_Text, Resources.StepValidateGetNodes_Title);

                string token = !string.IsNullOrEmpty(_ptzNodeToken) ? _ptzNodeToken : nodes[0].token;
                PTZNode node = GetNode(token);
                string reason = null;
                Assert(
                    (node.token == token) && ValidatePTZNode(node, out reason),
                    node.token != token ? string.Format("PTZ node has invalid token. Expected: {0} - actual: {1}", token, node.token) : reason,
                    Resources.StepValidatePTZNode_Title);
            });
        }

        [Test(Name = "SOAP FAULT MESSAGE",
            Path = "PTZ\\PTZ Node",
            Order = "10.01.03",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void NodeSoapFaultMessage()
        {
            RunTest(() =>
            {
                GetInvalidNode("InvalidNodeToken@#$");
            });
        }

        [Test(Name = "PTZ CONFIGURATIONS",
            Path = "PTZ\\PTZ Configuration",
            Order = "10.02.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void PtzConfigurations()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configuarions = GetConfigurations();
                Assert((configuarions != null)&& (configuarions.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);
            });
        }

        [Test(Name = "PTZ CONFIGURATION",
            Path = "PTZ\\PTZ Configuration",
            Order = "10.02.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void PtzConfiguration()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configuarions = GetConfigurations();
                Assert((configuarions != null)&&(configuarions.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);

                string token = configuarions[0].token;
                if(!string.IsNullOrEmpty(_ptzNodeToken))
                {
                    PTZConfiguration config = configuarions.FirstOrDefault((c) => c.NodeToken == _ptzNodeToken);
                    Assert(config != null, string.Format("No PTZ configuration for node [token={0}]", _ptzNodeToken));
                    token = config.token;
                }
                PTZConfiguration configuration = GetConfiguration(token);
                string reason = null;

                Assert(
                    (configuration.token == token)&&ValidatePTZConfiguration(configuration, out reason),
                    configuration.token != token ? string.Format("PTZ configuration has invalid token. Expected: {0} - actual: {1}", token, configuration.token) : reason,
                    Resources.StepValidatePTZConfig_Title);
            });
        }

        [Test(Name = "PTZ CONFIGURTATION OPTIONS",
            Path = "PTZ\\PTZ Configuration",
            Order = "10.02.03",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void PtzConfigurationOptions()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configuarions = GetConfigurations();
                Assert((configuarions != null)&&(configuarions.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);
                PTZConfiguration configuration = configuarions[0];
                PTZConfigurationOptions options = GetConfigurationOptions(configuration.token);
                string reason = null;
                Assert(ValidatePTZConfigurationOptions(options, configuration.token, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);
            });
        }

        [Test(Name = "PTZ SET CONFIGURTATION",
            Path = "PTZ\\PTZ Configuration",
            Order = "10.02.04",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void PtzSetConfiguration()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configuarions = GetConfigurations();
                Assert((configuarions != null)&&(configuarions.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);
                PTZConfiguration configuration = configuarions[0];
                string reason = null;
                Assert(ValidatePTZConfiguration(configuration, out reason), reason, Resources.StepValidatePTZConfig_Title);

                //according to Tests interpretation.xls 61
                PTZConfigurationOptions options = GetConfigurationOptions(configuration.token);

                Assert(ValidatePTZConfigurationOptions(options, configuration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);


                configuration.DefaultPTZTimeout = GetModifiedTimeout(configuration.DefaultPTZTimeout, options);

                SetConfiguration(configuration, false, string.Format("Default timeout = {0}", configuration.DefaultPTZTimeout));

                PTZConfiguration newConfig = GetConfiguration(configuration.token);

                Assert(
                    EqualConfigs(configuration, newConfig, out reason), 
                    string.Format(Resources.PTZConfigsNotEqual_Format, reason), 
                    Resources.StepValidateNewPTZConfig_Title);
            });
        }

        [Test(Name = "SOAP FAULT MESSAGE",
            Path = "PTZ\\PTZ Configuration",
            Order = "10.02.05",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void PtzConfigurationFault()
        {
            RunTest(() =>
            {
                SetInvalidConfiguration("InvalidConfigToken@#$");
            });
        }
    }
}
