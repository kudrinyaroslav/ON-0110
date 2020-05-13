///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class PTZConfigurationTestSuite : Base.PTZTest
    {
        public PTZConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "PTZ NODES",
            Path = "PTZ\\PTZ Node",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures =  new Feature[]{Feature.PTZService},
            FunctionalityUnderTest = new Functionality[]{Functionality.PtzGetNodes})]
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
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzGetNodes, Functionality.PtzGetNode })]
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
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService })]
        public void NodeSoapFaultMessage()
        {
            RunTest(() =>
            {
                GetInvalidNode("InvalidNode01234Token");
            });
        }

        [Test(Name = "PTZ CONFIGURATIONS",
            Path = PATH_2_1,
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPtzConfigurations })]
        public void PtzConfigurations()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configuarions = GetConfigurations();
                Assert((configuarions != null)&& (configuarions.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);
            });
        }

        [Test(Name = "PTZ CONFIGURATION",
            Path = PATH_2_1,
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPtzConfigurations, Functionality.GetPtzConfiguration })]
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

        [Test(Name = "PTZ CONFIGURATION OPTIONS",
            Path = PATH_2_1,
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPtzConfigurations, Functionality.GetPtzConfigurationOptions })]
        public void PtzConfigurationOptions()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configuarions = GetConfigurations();
                Assert((configuarions != null)&&(configuarions.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);
                PTZConfiguration configuration = configuarions[0];
                PTZConfigurationOptions options = GetConfigurationOptions(configuration.token);
                string reason = null;
                Assert(ValidatePTZConfigurationOptions(options, configuration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);
            });
        }

        [Test(Name = "PTZ SET CONFIGURATION",
            Path = PATH_2_1,
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPtzConfigurationOptions, Functionality.SetPtzConfiguration })]
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
            Path = PATH_2_1,
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetPtzConfiguration })]
        public void PtzConfigurationFault()
        {
            RunTest(() =>
            {
                SetInvalidConfiguration("InvalidConfigToken@#$");
            });
        }


        #region 2.0

        private const string PATH_2_1 = "PTZ\\PTZ Configuration";

        [Test(Name = "PTZ CONFIGURATIONS AND PTZ CONFIGURATION CONSISTENCY",
            Path = PATH_2_1,
            Order = "02.01.05",
            Id = "2-1-5",
            Category = Category.PTZ,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPtzConfigurations, Functionality.GetPtzConfiguration })]
        public void PtzConfigurationConsistencyTest()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configurations = GetConfigurations();
                Assert((configurations != null) && (configurations.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);

                foreach (PTZConfiguration configuration in configurations)
                {
                    PTZConfiguration config = GetConfiguration(configuration.token);
                    CompareConfigurations(configuration, config);
                }

            });
        }


        [Test(Name = "PTZ CONFIGURATIONS AND PTZ NODES CONSISTENCY",
            Path = PATH_2_1,
            Order = "02.01.06",
            Id = "2-1-6",
            Category = Category.PTZ,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzGetNodes, Functionality.GetPtzConfigurations })]
        public void PtzConfigurationAndNodesConsistencyTest()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configurations = GetConfigurations();
                Assert((configurations != null) && (configurations.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);

                PTZNode[] nodes = GetNodes();
                Assert((nodes != null) && (nodes.Length > 0), Resources.ErrorNoPTZNode_Text, Resources.StepValidateGetNodes_Title);
                
                foreach (PTZConfiguration configuration in configurations)
                {
                    string nodeToken = configuration.NodeToken;

                    int cnt = nodes.Where(N => N.token == nodeToken).Count();

                    Assert(cnt > 0, 
                        string.Format("PTZ node with token '{0}' not found", nodeToken), 
                        "Check if PTZ Node exists");

                    Assert(cnt == 1,
                        string.Format("More than one PTZ node with token '{0}' found", nodeToken),
                        "Check if PTZ Node with token specified is unique");

                    PTZNode node = nodes.Where(N => N.token == nodeToken).FirstOrDefault();

                    Assert(node.SupportedPTZSpaces != null, 
                        "SupportedPTZSpaces not defined", 
                        string.Format("Check if SupportedPTZSpaces settings are defined for PTZ node '{0}'", nodeToken));

                    CheckNodeAndConfiguration(configuration, node);
                }
            });
        }


        [Test(Name = "PTZ CONFIGURATIONS AND PTZ CONFIGURATION OPTIONS CONSISTENCY",
            Path = PATH_2_1,
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.PTZ,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPtzConfigurations, Functionality.GetPtzConfigurationOptions })]
        public void PtzConfigurationAndOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                PTZConfiguration[] configurations = GetConfigurations();
                Assert((configurations != null) && (configurations.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);

                foreach (PTZConfiguration configuration in configurations)
                {
                    string token = configuration.token;

                    PTZConfigurationOptions options = GetConfigurationOptions(token);

                    string reason = null;
                    Assert(ValidatePTZConfigurationOptions(options, configuration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                    CheckOptionsAndConfiguration(configuration, options);
                  
                }
            });
        }

        #endregion


        #region UTILS

        void CompareConfigurations(PTZConfiguration configuration1, PTZConfiguration configuration2)
        {
            bool ok = true;
            bool currentOk;
            StringBuilder sb = new StringBuilder();

            //Name
            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }

            //Name
            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            //UseCount
            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }

            //DefaultAbsolutePantTiltPositionSpace
            currentOk = StringsAreEqual(configuration1.DefaultAbsolutePantTiltPositionSpace,
                                 configuration2.DefaultAbsolutePantTiltPositionSpace,
                                 "DefaultAbsolutePantTiltPositionSpace", sb);
            ok = ok && currentOk;
            
            //DefaultAbsoluteZoomPositionSpace
            currentOk = StringsAreEqual(configuration1.DefaultAbsoluteZoomPositionSpace,
                     configuration2.DefaultAbsoluteZoomPositionSpace,
                     "DefaultAbsoluteZoomPositionSpace", sb);
            ok = ok && currentOk;

            //DefaultContinuousPanTiltVelocitySpace
            currentOk = StringsAreEqual(configuration1.DefaultContinuousPanTiltVelocitySpace,
                     configuration2.DefaultContinuousPanTiltVelocitySpace,
                     "DefaultContinuousPanTiltVelocitySpace", sb);
            ok = ok && currentOk;


            //DefaultContinuousZoomVelocitySpace
            currentOk = StringsAreEqual(configuration1.DefaultContinuousZoomVelocitySpace,
                     configuration2.DefaultContinuousZoomVelocitySpace,
                     "DefaultContinuousZoomVelocitySpace", sb);
            ok = ok && currentOk;


            //DefaultPTZSpeed
            if (configuration1.DefaultPTZSpeed != null && configuration2.DefaultPTZSpeed != null)
            {
                PTZSpeed speed1 = configuration1.DefaultPTZSpeed;
                PTZSpeed speed2 = configuration2.DefaultPTZSpeed;

                if (speed1.PanTilt != null && speed2.PanTilt != null)
                {
                    if (!(speed1.PanTilt.space == speed2.PanTilt.space && speed1.PanTilt.x == speed2.PanTilt.x && speed1.PanTilt.y == speed2.PanTilt.y))
                    {
                        ok = false;
                        sb.AppendLine("PanTilt settings in DefaultPTZSpeed are different");
                    }
                }
                else if (!(speed1.PanTilt == null && speed2.PanTilt == null))
                {
                    ok = false;
                    sb.AppendLine("PanTilt settings in DefaultPTZSpeed are defined for only one configuration");
                }

                if (speed1.Zoom != null && speed2.Zoom != null)
                {
                    if (!(speed1.Zoom.space == speed2.Zoom.space && speed1.Zoom.x == speed2.Zoom.x))
                    {
                        ok = false;
                        sb.AppendLine("Zoom settings in DefaultPTZSpeed are different");
                    }
                }
                else if (!(speed1.Zoom == null && speed2.Zoom == null))
                {
                    ok = false;
                    sb.AppendLine("Zoom settings in DefaultPTZSpeed are defined for only one configuration");
                }
            }
            else
            {
                if (!(configuration1.DefaultPTZSpeed == null && configuration2.DefaultPTZSpeed == null))
                {
                    ok = false;
                    sb.AppendLine("DefaultPTZSpeed are defined for only one configuration");
                }
            }


            //DefaultPTZTimeout
            //currentOk = StringsAreEqual(configuration1.DefaultPTZTimeout,
            //     configuration2.DefaultPTZTimeout,
            //     "DefaultPTZTimeout", sb);
            //ok = ok && currentOk;

            //DefaultRelativePanTiltTranslationSpace
            currentOk = StringsAreEqual(configuration1.DefaultRelativePanTiltTranslationSpace,
                  configuration2.DefaultRelativePanTiltTranslationSpace,
                  "DefaultRelativePanTiltTranslationSpace", sb);
            ok = ok && currentOk;

            //DefaultRelativeZoomTranslationSpace
            currentOk = StringsAreEqual(configuration1.DefaultRelativeZoomTranslationSpace,
                  configuration2.DefaultRelativeZoomTranslationSpace,
                  "DefaultRelativeZoomTranslationSpace", sb);
            ok = ok && currentOk;
            
            //NodeToken
            if (configuration1.NodeToken != configuration2.NodeToken)
            {
                ok = false;
                sb.AppendLine("'NodeToken' properties are different");
            }

            //PanTiltLimits
            if (configuration1.PanTiltLimits != null && configuration2.PanTiltLimits != null)
            {
                Space2DDescription range1 = configuration1.PanTiltLimits.Range;
                Space2DDescription range2 = configuration2.PanTiltLimits.Range;

                if (range1 != null && range2 != null)
                {
                    if (range1.URI != range2.URI)
                    {
                        ok = false;
                        sb.AppendLine("URI in 'PanTiltLimits' ranges are different");
                    }

                    FloatRange r1 = range1.XRange;
                    FloatRange r2 = range2.XRange;

                    if (r1 != null && r2 != null)
                    {
                        if (r1.Max != r2.Max || r1.Min != r2.Min)
                        {
                            ok = false;
                            sb.AppendLine("XRange in PanTiltLimits range are different");
                        }
                    }
                    else
                    {
                        if (!(r1 == null && r2 == null))
                        {
                            ok = false;
                            sb.AppendLine("XRange in PanTiltLimits range is defined for only one configuration");
                        }
                    }

                    r1 = range1.YRange;
                    r2 = range2.YRange;

                    if (r1 != null && r2 != null)
                    {
                        if (r1.Max != r2.Max || r1.Min != r2.Min)
                        {
                            ok = false;
                            sb.AppendLine("YRange in PanTiltLimits range are different");
                        }
                    }
                    else
                    {
                        if (!(r1 == null && r2 == null))
                        {
                            ok = false;
                            sb.AppendLine("YRange in PanTiltLimits range is defined for only one configuration");
                        }
                    }
                }
                else
                {
                    ok = false;
                    sb.AppendLine("'PanTiltLimits' range is defined for only one configuration");
                }
            }
            else
            {
                if (!(configuration1.PanTiltLimits == null && configuration2.PanTiltLimits == null))
                {
                    ok = false;
                    sb.AppendLine("PanTiltLimits are defined for only one configuration");
                }
            }


            //ZoomLimits
            if (configuration1.ZoomLimits != null && configuration2.ZoomLimits != null)
            {
                Space1DDescription range1 = configuration1.ZoomLimits.Range;
                Space1DDescription range2 = configuration2.ZoomLimits.Range;

                if (range1 != null && range2 != null)
                {
                    if (range1.URI != range2.URI)
                    {
                        ok = false;
                        sb.AppendLine("URI in ZoomLimits ranges are different");
                    }

                    FloatRange r1 = range1.XRange;
                    FloatRange r2 = range2.XRange;

                    if (r1 != null && r2 != null)
                    {
                        if (r1.Max != r2.Max || r1.Min != r2.Min)
                        {
                            ok = false;
                            sb.AppendLine("XRange in ZoomLimits range are different");
                        }
                    }
                    else
                    {
                        if (!(r1 == null && r2 == null))
                        {
                            ok = false;
                            sb.AppendLine("XRange in ZoomLimits range is defined for only one configuration");
                        }
                    }
                }
                else
                {
                    ok = false;
                    sb.AppendLine("'ZoomLimits' range is defined for only one configuration");
                }
            }
            else
            {
                if (!(configuration1.ZoomLimits == null && configuration2.ZoomLimits == null))
                {
                    ok = false;
                    sb.AppendLine("ZoomLimits are defined for only one configuration");
                }
            }

            Assert(ok, sb.ToStringTrimNewLine(), "Check that configurations are the same");

        }

        void CheckNodeAndConfiguration(PTZConfiguration configuration, PTZNode node)
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();

            ok = CheckConfigurationAndSpaces(configuration, node.SupportedPTZSpaces, sb);
            
            Assert(ok, sb.ToStringTrimNewLine(), "Check that PTZ configuration are correct accordingly to PTZ node settings");
        }

        void CheckOptionsAndConfiguration(PTZConfiguration configuration, PTZConfigurationOptions options)
        {
            bool ok = true;
            bool warning = false;
            StringBuilder sb = new StringBuilder();

            ok = CheckConfigurationAndSpaces(configuration, options.Spaces, sb);

            if (options.PTZTimeout != null && !string.IsNullOrEmpty(configuration.DefaultPTZTimeout))
            {
                double minTimeout = options.PTZTimeout.Min.DurationToSeconds();
                double maxTimeout = options.PTZTimeout.Max.DurationToSeconds();
                double timeout = configuration.DefaultPTZTimeout.DurationToSeconds();

                if (double.IsNaN(minTimeout) || double.IsNaN(maxTimeout) || double.IsNaN(timeout))
                {
                    warning = true;
                    sb.AppendLine("WARNING: some of timeout values contain date part. Comparison will be omitted.");
                }
                else
                {
                    if (!(minTimeout <= timeout && maxTimeout >= timeout))
                    {
                        ok = false;
                        sb.AppendLine(string.Format("DefaultPTZTimeout ({0}) is out of range ([{1}, {2}])",
                            configuration.DefaultPTZTimeout, options.PTZTimeout.Min, options.PTZTimeout.Max));
                    }                    
                }
            }

            BeginStep("Check that PTZ configuration and configuration options are consistent");
            if (!ok)
            {
                throw new AssertException(sb.ToStringTrimNewLine());
            }
            else
            {
                if (warning)
                {
                    LogStepEvent(sb.ToStringTrimNewLine());
                }
            }
            StepPassed();
        }

        bool CheckConfigurationAndSpaces(PTZConfiguration configuration, PTZSpaces spaces, StringBuilder sb)
        {
            bool ok = true;
            if (!string.IsNullOrEmpty(configuration.DefaultAbsolutePantTiltPositionSpace))
            {
                bool supported = true;
                if (spaces.AbsolutePanTiltPositionSpace == null)
                {
                    supported = false;
                }
                else
                {
                    //DefaultAbsolutePanTiltPositionSpace from PTZConfiguration is not included in one of SupportedPTZSpaces.AbsolutePanTiltPositionSpace from GetNodesResponse.
                    if (spaces.AbsolutePanTiltPositionSpace.Where(
                        S => S.URI == configuration.DefaultAbsolutePantTiltPositionSpace).FirstOrDefault() == null)
                    {
                        supported = false;
                    }
                }

                if (!supported)
                {
                    ok = false;
                    sb.AppendLine(
                        string.Format("PanTiltPositionSpace with URI = '{0}' not found in SupportedPTZSpaces",
                        configuration.DefaultAbsolutePantTiltPositionSpace));
                }
            }

            //	DefaultAbsoluteZoomPositionSpace from PTZConfiguration is not included in one of SupportedPTZSpaces.AbsoluteZoomPositionSpace from GetNodesResponse.
            if (!string.IsNullOrEmpty(configuration.DefaultAbsoluteZoomPositionSpace))
            {
                bool supported = true;
                if (spaces.AbsoluteZoomPositionSpace == null)
                {
                    supported = false;
                }
                else
                {
                    if (spaces.AbsoluteZoomPositionSpace.Where(
                        S => S.URI == configuration.DefaultAbsoluteZoomPositionSpace).FirstOrDefault() == null)
                    {
                        supported = false;
                    }
                }

                if (!supported)
                {
                    ok = false;
                    sb.AppendLine(
                        string.Format("AbsoluteZoomPositionSpace with URI = '{0}' not found in SupportedPTZSpaces",
                        configuration.DefaultAbsoluteZoomPositionSpace));
                }
            }

            //	DefaultRelativePanTiltTranslationSpace from PTZConfiguration is not included in one of SupportedPTZSpaces.RelativePanTiltTranslationSpace from GetNodesResponse.
            if (!string.IsNullOrEmpty(configuration.DefaultRelativePanTiltTranslationSpace))
            {
                bool supported = true;
                if (spaces.RelativePanTiltTranslationSpace == null)
                {
                    supported = false;
                }
                else
                {
                    if (spaces.RelativePanTiltTranslationSpace.Where(
                        S => S.URI == configuration.DefaultRelativePanTiltTranslationSpace).FirstOrDefault() == null)
                    {
                        supported = false;
                    }
                }

                if (!supported)
                {
                    ok = false;
                    sb.AppendLine(
                        string.Format("RelativePanTiltTranslationSpace with URI = '{0}' not found in SupportedPTZSpaces",
                        configuration.DefaultRelativePanTiltTranslationSpace));
                }
            }

            //	DefaultRelativeZoomTranslationSpace from PTZConfiguration is not included in one of SupportedPTZSpaces.RelativeZoomTranslationSpace from GetNodesResponse.
            if (!string.IsNullOrEmpty(configuration.DefaultRelativeZoomTranslationSpace))
            {
                bool supported = true;
                if (spaces.RelativeZoomTranslationSpace == null)
                {
                    supported = false;
                }
                else
                {
                    if (spaces.RelativeZoomTranslationSpace.Where(
                        S => S.URI == configuration.DefaultRelativeZoomTranslationSpace).FirstOrDefault() == null)
                    {
                        supported = false;
                    }
                }

                if (!supported)
                {
                    ok = false;
                    sb.AppendLine(
                        string.Format("RelativeZoomTranslationSpace with URI = '{0}' not found in SupportedPTZSpaces",
                        configuration.DefaultRelativeZoomTranslationSpace));
                }
            }

            //	DefaultContinuousPanTiltVelocitySpace from PTZConfiguration is not included in one of SupportedPTZSpaces.ContinuousPanTiltVelocitySpacefrom GetNodesResponse.
            if (!string.IsNullOrEmpty(configuration.DefaultContinuousPanTiltVelocitySpace))
            {
                bool supported = true;
                if (spaces.ContinuousPanTiltVelocitySpace == null)
                {
                    supported = false;
                }
                else
                {
                    if (spaces.ContinuousPanTiltVelocitySpace.Where(
                        S => S.URI == configuration.DefaultContinuousPanTiltVelocitySpace).FirstOrDefault() == null)
                    {
                        supported = false;
                    }
                }

                if (!supported)
                {
                    ok = false;
                    sb.AppendLine(
                        string.Format("ContinuousPanTiltVelocitySpace with URI = '{0}' not found in SupportedPTZSpaces",
                        configuration.DefaultContinuousPanTiltVelocitySpace));
                }
            }

            //	DefaultContinuousZoomVelocitySpace from PTZConfiguration is not included in one of SupportedPTZSpaces.ContinuousZoomVelocitySpace from GetNodesResponse.
            if (!string.IsNullOrEmpty(configuration.DefaultContinuousZoomVelocitySpace))
            {
                bool supported = true;
                if (spaces.ContinuousZoomVelocitySpace == null)
                {
                    supported = false;
                }
                else
                {
                    if (spaces.ContinuousZoomVelocitySpace.Where(
                            S => S.URI == configuration.DefaultContinuousZoomVelocitySpace).FirstOrDefault() == null)
                    {
                        supported = false;
                    }
                }

                if (!supported)
                {
                    ok = false;
                    sb.AppendLine(
                        string.Format("ContinuousZoomVelocitySpace with URI = '{0}' not found in SupportedPTZSpaces",
                        configuration.DefaultContinuousZoomVelocitySpace));

                }
            }

            if (configuration.DefaultPTZSpeed != null)
            {
                if (configuration.DefaultPTZSpeed.PanTilt != null)
                {
                    bool supported = true;

                    //	DefaultPTZSpeed.PanTilt.space from PTZConfiguration is not included in one of PanTiltSpeedSpace.URI section from GetNodesResponse
                    Space1DDescription space = null;
                    if (spaces.PanTiltSpeedSpace == null)
                    {
                        supported = false;
                    }
                    else
                    {
                        space = spaces.PanTiltSpeedSpace.Where(
                            S => S.URI == configuration.DefaultPTZSpeed.PanTilt.space).FirstOrDefault();
                        if (space == null)
                        {
                            supported = false;
                        }
                    }
                    if (!supported)
                    {
                        ok = false;
                        sb.AppendLine(
                            string.Format(
                                "PanTiltSpeedSpace with URI = '{0}' not found in SupportedPTZSpaces",
                                configuration.DefaultPTZSpeed.PanTilt.space));
                    }

                    //	DefaultPTZSpeed.PanTilt.x is not between SupportedPTZSpaces.PanTiltSpeedSpace.XRange.Min and SupportedPTZSpaces.PanTiltSpeedSpace.XRange.Max for appropriate PanTiltSpeedSpace.URI from GetNodesResponse.
                    //	DefaultPTZSpeed.PanTilt.y is not between SupportedPTZSpaces.PanTiltSpeedSpace.YRange.Min and SupportedPTZSpaces.PanTiltSpeedSpace.YRange.Max for appropriate PanTiltSpeedSpace.URI from GetNodesResponse. (see Questions)
                    // see questions = PanTile.y not defined =(
                    if (space != null && space.XRange != null)
                    {
                        if (configuration.DefaultPTZSpeed.PanTilt.x < space.XRange.Min ||
                            configuration.DefaultPTZSpeed.PanTilt.x > space.XRange.Max)
                        {
                            ok = false;
                            sb.AppendLine(string.Format("DefaultPTZSpeed.PanTilt.x ({0}) is out of range ([{1},{2}])",
                                configuration.DefaultPTZSpeed.PanTilt.x, space.XRange.Min, space.XRange.Max));
                        }

                        if (configuration.DefaultPTZSpeed.PanTilt.y < space.XRange.Min ||
                            configuration.DefaultPTZSpeed.PanTilt.y > space.XRange.Max)
                        {
                            ok = false;
                            sb.AppendLine(string.Format("DefaultPTZSpeed.PanTilt.y ({0}) is out of range ([{1},{2}])",
                                configuration.DefaultPTZSpeed.PanTilt.y, space.XRange.Min, space.XRange.Max));
                        }
                    }
                }

                if (configuration.DefaultPTZSpeed.Zoom != null)
                {
                    bool supported = true;

                    //	DefaultPTZSpeed.Zoom.Space is not included in one of ZoomSpeedSpace.URI section of GetNodesResponse
                    Space1DDescription space = null;
                    if (spaces.ZoomSpeedSpace == null)
                    {
                        supported = false;
                    }
                    else
                    {
                        space = spaces.ZoomSpeedSpace.Where(
                            S => S.URI == configuration.DefaultPTZSpeed.Zoom.space).FirstOrDefault();
                        if (space == null)
                        {
                            supported = false;
                        }
                    }
                    if (!supported)
                    {
                        ok = false;
                        sb.AppendLine(
                            string.Format(
                                "PanTiltZoomSpace with URI = '{0}' not found in SupportedPTZSpaces",
                                configuration.DefaultPTZSpeed.Zoom.space));
                    }

                    //	DefaultPTZSpeed.Zoom.x is not between SupportedPTZSpaces.ZoomSpeedSpace.XRange.Min and SupportedPTZSpaces.ZoomSpeedSpace.XRange.Max for appropriate ZoomSpeedSpace.URI from GetNodesResponse.
                    if (space != null && space.XRange != null)
                    {
                        if (configuration.DefaultPTZSpeed.Zoom.x < space.XRange.Min ||
                            configuration.DefaultPTZSpeed.Zoom.x > space.XRange.Max)
                        {
                            ok = false;
                            sb.AppendLine(string.Format("DefaultPTZSpeed.Zoom.x ({0}) is out of range ([{1}, {2}])", 
                                configuration.DefaultPTZSpeed.Zoom.x, space.XRange.Min, space.XRange.Max));
                        }
                    }
                }
            }

            return ok;
        }

        /// <summary>
        /// Compares strings.
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="sb">StringBuilder to receive error description, if occurred.</param>
        /// <returns></returns>
        bool StringsAreEqual(string value1, string value2, string propertyName, StringBuilder sb)
        {
            bool ok = true;
            if (!(string.IsNullOrEmpty(value1) && string.IsNullOrEmpty(value2)))
            {
                if (StringComparer.InvariantCultureIgnoreCase.Compare(value1, value2) != 0 )
                {
                    ok = false;
                    if (string.IsNullOrEmpty(value1) || string.IsNullOrEmpty(value2))
                    {
                        sb.AppendFormat("'{0}' is defined only in one configuration{1}", propertyName, Environment.NewLine);
                    }
                    else
                    {
                        sb.AppendFormat("'{0}' properties are different{1}", propertyName, Environment.NewLine);
                    }
                }
            }
            return ok;
        }

        #endregion
    }
}
