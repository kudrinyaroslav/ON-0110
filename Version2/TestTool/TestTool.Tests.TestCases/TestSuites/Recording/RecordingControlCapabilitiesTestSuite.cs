using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites.Recording
{
    [TestClass]
    public class RecordingControlCapabilitiesTestSuite : RecordingTest
    {
        public RecordingControlCapabilitiesTestSuite(TestLaunchParam param) : base(param)
        {
        }

        private const string PATH = "Recording Control\\Capabilities";


        [Test(Name = "RECORDING CONTROL SERVICE CAPABILITIES",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.RECORDING,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingServiceCapabilities })]
        public void RecordingServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client

                Proxies.Onvif.RecordingServiceCapabilities  capabilities = GetServiceCapabilities();


                BeginStep("Validate Service Capabilities");

                bool ok = true;
                StringBuilder dump = new StringBuilder("Capabilities are incorrect" + Environment.NewLine);
                if (capabilities.MaxRateSpecified && capabilities.MaxRate <= 0)
                {
                    dump.Append(string.Format("   MaxRate is incorrect ({0}){1}", capabilities.MaxRate, Environment.NewLine));
                    ok = false;
                }
                if (capabilities.MaxTotalRateSpecified && capabilities.MaxTotalRate <= 0)
                {
                    dump.Append(string.Format("   MaxTotalRate is incorrect ({0}){1}", capabilities.MaxTotalRate, Environment.NewLine));
                    ok = false;
                }

                List<string> validEncodings = new List<string>();
                validEncodings.AddRange(new string[] { "G711", "G726", "AAC", "JPEG", "MPEG4", "H264" });

                List<string> encodings = new List<string>();
                List<string> incorrect = new List<string>();
                if (capabilities.Encoding != null)
                { 
                    foreach (string encoding in capabilities.Encoding)
                    {
                        if (!string.IsNullOrEmpty(encoding.Trim()))
                        {
                            encodings.Add(encoding);
                            if (!validEncodings.Contains(encoding))
                            {
                                incorrect.Add(encoding);
                            }
                        }
                    }                
                }

                if (encodings.Count == 0)
                {
                    dump.AppendLine("   Encoding list is empty");
                    ok = false;
                }

                if (incorrect.Count > 0)
                {
                    dump.AppendLine(string.Format("   The following encoding values are incorrect: {0}", 
                        string.Join(",", incorrect.ToArray())));
                    ok = false;
                }

                if (capabilities.MaxRecordingsSpecified && capabilities.MaxRecordings <1)
                {
                    dump.Append(string.Format("   MaxRecordings is incorrect ({0}){1}", capabilities.MaxRecordings, Environment.NewLine));
                    ok = false;
                }

                if (!ok)
                {
                    throw new AssertException(dump.ToStringTrimNewLine());
                }

                StepPassed();
            });
        }

        [Test(Name = "GET SERVICES AND GET RECORDING CONTROL SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.RECORDING,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetRecordingServiceCapabilities })]
        public void CapabilitiesAndRecordingServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service recordingService = services.FindService(Definitions.Onvif.OnvifService.RECORIDING);

                Assert(recordingService != null, "No Recording service information returned", "Check that the DUT returned Search service information");

                Assert((recordingService.Capabilities != null), "No Capabilities information included",
                       "Check that Capabilities element is included in Services element");

                RecordingServiceCapabilities serviceCapabilities = ExtractCapabilities<RecordingServiceCapabilities>(recordingService.Capabilities, OnvifService.RECORIDING);

                RecordingServiceCapabilities capabilities = GetServiceCapabilities();

                CompareCapabilities(serviceCapabilities, capabilities);
            });
        }

        #region Validation

        void CompareCapabilities(RecordingServiceCapabilities serviceCapabilities, RecordingServiceCapabilities capabilities)
        {
            // serviceCapabilities - from GetServices
            // capabilities - from GetServiceCapabilities

            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder("Capabilities are different"+Environment.NewLine);
            bool equal = true;
            bool local;

            Action<Func<RecordingServiceCapabilities, bool>, Func<RecordingServiceCapabilities, bool>, string> checkField =
                new Action<Func<RecordingServiceCapabilities, bool>, Func<RecordingServiceCapabilities, bool>, string>(
                    (specifiedSelector, valueSelector, fieldName) =>
                        {
                            if (specifiedSelector(serviceCapabilities) && specifiedSelector(capabilities))
                            {
                                if (valueSelector(serviceCapabilities) != valueSelector(capabilities))
                                {
                                    equal = false;
                                    dump.Append(string.Format("   {0} values don't match{1}", fieldName, Environment.NewLine));
                                }
                            }
                            else
                            {
                                if (specifiedSelector(serviceCapabilities))
                                {
                                    equal = false;
                                    dump.Append(string.Format("   {0} is not present in structure received from GetServiceCapabilities{1}", fieldName, Environment.NewLine));
                                }
                                if (specifiedSelector(capabilities))
                                {
                                    equal = false;
                                    dump.Append(string.Format("   {0} is not present in structure received from GetServices{1}", fieldName, Environment.NewLine));
                                }
                            }
                        });

            checkField(C => C.DynamicRecordingsSpecified, C => C.DynamicRecordings, "DynamicRecordings");

            checkField(C => C.DynamicTracksSpecified, C => C.DynamicTracks, "DynamicTracks");
            
            // Encoding

            local = true;
            if (serviceCapabilities.Encoding != null  && capabilities.Encoding != null)
            {
                foreach (string item in serviceCapabilities.Encoding)
                {
                    if (!capabilities.Encoding.Contains(item) )
                    {
                        dump.AppendFormat(
                                "   Encoding {0} not found in Encodings list received via GetServices{1}",
                                item, Environment.NewLine);
                        local = false;
                    }
                }

                foreach (string item in capabilities.Encoding)
                {
                    if (!serviceCapabilities.Encoding.Contains(item))
                    {
                        dump.AppendFormat(
                                "   Encoding {0} not found in Encodings list received via GetCapabilities{1}",
                                item, Environment.NewLine);
                        local = false;
                    }
                }
            }
            else
            {
                if (capabilities.Encoding != null)
                {
                    dump.AppendLine("   Encoding list is empty in structure received via GetCapabilities");
                    local = false;
                }
                if (serviceCapabilities.Encoding != null)
                {
                    dump.AppendLine("   Encoding list is empty in structure received via GetServices");
                    local = false;
                }              

            }

            equal = equal && local;

            // 
            local = CheckFloatField(serviceCapabilities, capabilities, C => C.MaxRate, C => C.MaxRateSpecified, "MaxRate", "GetServices", "GetServiceCapabilities", dump);

            equal = equal && local;

            local = CheckFloatField(serviceCapabilities, capabilities, C => C.MaxRecordings, C => C.MaxRecordingsSpecified, "MaxRecordings", "GetServices", "GetServiceCapabilities", dump);

            equal = equal && local;

            local = CheckFloatField(serviceCapabilities, capabilities, C => C.MaxTotalRate, C => C.MaxTotalRateSpecified, "MaxTotalRate", "GetServices", "GetServiceCapabilities", dump);

            equal = equal && local;

            checkField(C => C.MetadataRecordingSpecified, C => C.MetadataRecording, "MetadataRecording");

            checkField(C => C.OptionsSpecified, C => C.Options, "Options");

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }

            StepPassed();
        }



        /// <summary>
        /// Checks int field.
        /// </summary>
        /// <typeparam name="T">Structure type</typeparam>
        /// <param name="s1">First structure</param>
        /// <param name="s2">Second structure</param>
        /// <param name="valueSelector">Value selector</param>
        /// <param name="specifiedSelector">Selector for corresponding "specified" field.</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="descr1">First structure description.</param>
        /// <param name="descr2">Second structure description.</param>
        /// <param name="dump">Log</param>
        /// <returns></returns>
        public bool CheckFloatField<T>(T s1, T s2,
            Func<T, float> valueSelector, Func<T, bool> specifiedSelector,
            string fieldName, string descr1, string descr2, StringBuilder dump)
        {
            bool ok = false;

            if (specifiedSelector(s1) && specifiedSelector(s2))
            {
                if (valueSelector(s1) != valueSelector(s2))
                {
                    dump.AppendLine(string.Format("   {0} values are different", fieldName));
                }
                else
                {
                    ok = true;
                }
            }
            else
            {
                if (!specifiedSelector(s1) && !specifiedSelector(s2))
                {
                    ok = true;
                }
                else
                {
                    if (!specifiedSelector(s1))
                    {
                        dump.AppendLine(string.Format("   {0} field is not present in the structure received via {1}", fieldName, descr1));
                    }
                    if (!specifiedSelector(s2))
                    {
                        dump.AppendLine(string.Format("   {0} field is not present in the structure received via {1}", fieldName, descr2));
                    }            
                }
            }
            return ok;
        }


        #endregion


  
    }
}
