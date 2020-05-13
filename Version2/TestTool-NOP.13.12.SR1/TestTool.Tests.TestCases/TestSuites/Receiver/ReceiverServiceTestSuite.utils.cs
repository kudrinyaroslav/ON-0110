using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Tests.Definitions.Exceptions;


namespace TestTool.Tests.TestCases.TestSuites
{
    public partial class ReceiverServiceTestSuite : ReceiverTest
    {
        protected ReceiverServiceCapabilities ParseReceiverCapabilities(XmlElement element)
        {
            return ExtractCapabilities<ReceiverServiceCapabilities>(element,
                                                                   Definitions.Onvif.OnvifService.RECEIVER);
        }

        void CompareCapabilities(ReceiverServiceCapabilities serviceCapabilities, ReceiverServiceCapabilities capabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            equal &= CheckField(serviceCapabilities, capabilities, C => C.RTP_Multicast,
                                              C => C.RTP_MulticastSpecified, "RTP_Multicast", "GetServices", "GetServiceCapabilities", dump);
            equal &= CheckField(serviceCapabilities, capabilities, C => C.RTP_TCP,
                                              C => C.RTP_TCPSpecified, "RTP_TCP", "GetServices", "GetServiceCapabilities", dump);
            equal &= CheckField(serviceCapabilities, capabilities, C => C.RTP_RTSP_TCP,
                                              C => C.RTP_RTSP_TCPSpecified, "RTP_RTSP_TCP", "GetServices", "GetServiceCapabilities", dump);
            equal &= CheckField(serviceCapabilities, capabilities, C => C.MaximumRTSPURILength,
                                              C => C.MaximumRTSPURILengthSpecified, "MaximumRTSPURILength", "GetServices", "GetServiceCapabilities", dump);

            if (serviceCapabilities.SupportedReceivers != capabilities.SupportedReceivers)
            {
                equal &= false;
                dump.Append(string.Format("   Supported recievers values don't match{0}", Environment.NewLine));
            }

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }

            StepPassed();
        }

        public bool CheckField<T1, T2>(T1 s1, T1 s2,
            Func<T1, T2> valueSelector, Func<T1, bool> specifiedSelector,
            string fieldName, string descr1, string descr2, StringBuilder dump)
            where T2 : IEquatable<T2>
        {
            bool ok = false;
            if (specifiedSelector(s1) && specifiedSelector(s2))
            {
                if (!valueSelector(s1).Equals(valueSelector(s2)))
                {
                    dump.AppendLine(
                        string.Format("   {0} values are different{1}", fieldName, Environment.NewLine));
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
                        dump.AppendLine(
                            string.Format("   {0} field is not present in the structure received via {1}{2}",
                                                        fieldName, descr1, Environment.NewLine));
                    }
                    if (!specifiedSelector(s2))
                    {
                        dump.AppendLine(
                            string.Format("   {0} field is not present in the structure received via {1}{2}",
                                                        fieldName, descr2, Environment.NewLine));
                    }
                }
            }
            return ok;
        }
    }
}