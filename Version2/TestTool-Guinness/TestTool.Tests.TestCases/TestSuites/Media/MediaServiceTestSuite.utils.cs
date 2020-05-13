#define ERRATA

///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.TestCases.TestSuites
{
    /// <summary>
    /// Utility methods.
    /// </summary>
    partial class MediaServiceTestSuite
    {

        #region UTILS

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
        
        /// <summary>
        /// Compares multicast configurations.
        /// </summary>
        /// <param name="multicast1">First configuration</param>
        /// <param name="multicast2">Second configuration</param>
        /// <param name="sb">StringBuilder to receive error description, if occurred.</param>
        /// <returns></returns>
        bool AreEqual(MulticastConfiguration multicast1,
                MulticastConfiguration multicast2,
                StringBuilder sb)
        {
            bool ok = true;

            // Multicast
            if (multicast1 != null && multicast2 != null)
            {

                if (!EqualIPAddresses(multicast1.Address, multicast2.Address))
                {
                    ok = false;
                    sb.AppendLine("IP addresses in multicast configuration are different");
                }

                if (multicast1.AutoStart != multicast2.AutoStart)
                {
                    ok = false;
                    sb.AppendLine("'AutoStart' properties in multicast configuration are different");
                }

                if (multicast1.Port != multicast2.Port)
                {
                    ok = false;
                    sb.AppendLine("'Port' properties in multicast configuration are different");
                }

                if (multicast1.TTL != multicast2.TTL)
                {
                    ok = false;
                    sb.AppendLine("'TTL' properties in multicast configuration are different");
                }

            }
            else
            {
                if (!(multicast1 == null && multicast2 == null))
                {
                    ok = false;
                    sb.AppendLine("Multicast settings are defined only for one configuration");
                }
            }

            return ok;
        }

        /// <summary>
        /// Compares configs from VideoAnalyticsConfiguration
        /// </summary>
        /// <param name="configs1">First configuration</param>
        /// <param name="configs2">Second configuration</param>
        /// <param name="sb">StringBuilder to receive error description, if occurred.</param>
        /// <returns></returns>
        bool AreEqual(Config[] configs1, Config[] configs2, StringBuilder sb)
        {
            bool ok = true;

            foreach (Config config1 in configs1)
            {
                Config config2 = configs2.Where(C => C.Name == config1.Name).FirstOrDefault();
                if (config2 == null)
                {
                    ok = false;
                    sb.AppendLine(string.Format("Record with name '{0}' is presented in only one list", config1.Name));
                }
                else
                {
                    // Type
                    if (config1.Type != null && config2.Type != null)
                    {
                        if (!config1.Type.IsEmpty && !config2.Type.IsEmpty)
                        {
                            if (config1.Type.Name != config2.Type.Name && config1.Type.Namespace != config2.Type.Namespace)
                            {
                                ok = false;
                                sb.AppendLine(string.Format("In records with name '{0}' Type is different", config1.Name));
                            }
                        }
                        else
                        {
                            if (!(config1.Type.IsEmpty && config2.Type.IsEmpty))
                            {
                                ok = false;
                                sb.AppendLine(string.Format("In records with name '{0}' Type is empty only for one", config1.Name));
                            }
                        }
                    }
                    else
                    {
                        if (!(config1.Type == null && config2.Type == null))
                        {
                            ok = false;
                            sb.AppendLine(string.Format("In records with name '{0}' Type is defined only for one", config1.Name));
                        }
                    }
                    
                    // Parameters

                    ItemList list1 = config1.Parameters;
                    ItemList list2 = config2.Parameters;

                    if (list1 != null && list2 != null)
                    {
                        sb.AppendLine("Compare parameters lists");
                        bool listsOk = AreEqual(list1, list2, sb);
                        ok = ok && listsOk;
                    }
                    else
                    {
                        if (! (list1 == null && list2 == null))
                        {
                            ok = false;
                            sb.AppendLine(string.Format("In records with name '{0}' Parameters list is defined only for one", config1.Name));
                        }
                    }

                }
            }

            foreach (Config config2 in configs2)
            {
                Config config1 = configs1.Where(C => C.Name == config2.Name).FirstOrDefault();
                if (config1 == null)
                {
                    ok = false;
                    sb.AppendLine(string.Format("Record with name '{0}' is presented in only one list", config2.Name));
                }
            }

            return ok;
        }
        
        bool AreEqual(ItemList list1, ItemList list2, StringBuilder sb)
        {
            bool ok = true;
            bool currentOk = true;

            ok = AllElementItemsPresented(list1.ElementItem, list2.ElementItem, sb);

            currentOk = AllElementItemsPresented(list2.ElementItem, list1.ElementItem, sb);
            ok = ok && currentOk;

            currentOk = AllSimpleItemsPresented(list1.SimpleItem, list2.SimpleItem, sb);
            ok = ok && currentOk;

            currentOk = AllSimpleItemsPresented(list2.SimpleItem, list1.SimpleItem, sb);
            ok = ok && currentOk;

            return ok;
        }

        bool AllElementItemsPresented(ItemListElementItem[] list1, ItemListElementItem[] list2, StringBuilder sb)
        {
            bool ok = true;
            if (list1 != null)
            {
                if (list2 != null)
                {
                    foreach (ItemListElementItem item1 in list1)
                    {
                        ItemListElementItem item2 = list2.Where(I => I.Name == item1.Name).FirstOrDefault();
                        if (item2 == null)
                        {
                            ok = false;
                            sb.AppendLine(string.Format("Parameter with name '{0}' is presented in only one list",
                                                        item1.Name));
                        }
                    }
                }
                else
                {
                    ok = false;
                    foreach (ItemListElementItem item1 in list1)
                    {
                        sb.AppendLine(string.Format("Parameter with name '{0}' is presented in only one list",
                                                    item1.Name));
                    }
                }
            }
            return ok;
        }

        bool AllSimpleItemsPresented(ItemListSimpleItem[] list1, ItemListSimpleItem[] list2, StringBuilder sb)
        {
            bool ok = true;

            if (list1 != null)
            {
                if (list2 != null)
                {
                    foreach (ItemListSimpleItem item1 in list1)
                    {
                        ItemListSimpleItem item2 = list2.Where(I => I.Name == item1.Name).FirstOrDefault();
                        if (item2 == null)
                        {
                            ok = false;
                            sb.AppendLine(string.Format("Parameter with name '{0}' is presented in only one list", item1.Name));
                        }
                    }
                }
                else
                {
                    ok = false;
                    foreach (ItemListSimpleItem item1 in list1)
                    {
                        sb.AppendLine(string.Format("Parameter with name '{0}' is presented in only one list", item1.Name));
                    }
                }
            }

            return ok;
        }

        bool AreEqual(AudioOutputConfiguration configuration1,
            AudioOutputConfiguration configuration2, 
            StringBuilder sb)
        {
            bool ok = true;
            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }
            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            if (configuration1.OutputToken != configuration2.OutputToken)
            {
                ok = false;
                sb.AppendLine("'OutputToken' properties are different");
            }

            if (configuration1.SendPrimacy != configuration2.SendPrimacy)
            {
                ok = false;
                sb.AppendLine("'SendPrimacy' properties are different");
            }

            if (configuration1.OutputLevel != configuration2.OutputLevel)
            {
                ok = false;
                sb.AppendLine("'OutputLevel' properties are different");
            }

            return ok;
        }

        bool AreEqual(AudioDecoderConfiguration configuration1,
            AudioDecoderConfiguration configuration2,
            StringBuilder sb)
        {
            bool ok = true;
            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }

            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }

            return ok;
        }

        
        bool AreEqual(AudioSourceConfiguration configuration1, AudioSourceConfiguration configuration2, StringBuilder sb)
        {
            bool ok = true;

            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }

            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            if (configuration1.SourceToken != configuration2.SourceToken)
            {
                ok = false;
                sb.AppendLine("'SourceToken' properties are different");
            }

            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }

            return ok;
        }

        void CompareConfigurations(AudioSourceConfiguration configuration1, AudioSourceConfiguration configuration2)
        {
            CompareConfigurations( configuration1, configuration2, false);
        }

        void CompareConfigurations(AudioSourceConfiguration configuration1, 
            AudioSourceConfiguration configuration2, bool displayToken)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = AreEqual(configuration1, configuration2, sb);

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump, 
                displayToken ? 
                string.Format("Check that configurations [token = '{0}'] are the same", configuration1.token) :
                "Check that configurations are the same");
        }


        bool AreEqual(AudioEncoderConfiguration configuration1, AudioEncoderConfiguration configuration2, StringBuilder sb)
        {
            bool ok = true;
            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }
            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }
            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }

            if (configuration1.Bitrate != configuration2.Bitrate)
            {
                ok = false;
                sb.AppendLine("'Bitrate' properties are different");
            }

            if (configuration1.Encoding != configuration2.Encoding)
            {
                ok = false;
                sb.AppendLine("'Encoding' properties are different");
            }

            bool multicastOk = AreEqual(configuration1.Multicast, configuration2.Multicast, sb);
            ok = ok && multicastOk;
            
            if (configuration1.SampleRate != configuration2.SampleRate)
            {
                ok = false;
                sb.AppendLine("'SampleRate' properties are different");
            }

            bool currentOk = TimeoutsAreEqual(configuration1.SessionTimeout, 
                configuration2.SessionTimeout,
                "SessionTimeout", 
                sb);
            ok = ok && currentOk;

            return ok;
        }

        bool TimeoutsAreEqual(string value1, string value2, string propertyName, StringBuilder sb)
        {
            bool ok = true;

            if (StringComparer.InvariantCultureIgnoreCase.Compare(value1, value2) != 0)
            {
                if (!string.IsNullOrEmpty(value1) &&
                    !string.IsNullOrEmpty(value2))
                {
                    double timeout1 = value1.DurationToSeconds();
                    double timeout2 = value2.DurationToSeconds();

                    if (double.IsNaN(timeout1) || double.IsNaN(timeout2))
                    {
                        ok = true;
                        sb.AppendLine(string.Format(
                                          "WARNING: at least one of {0} values contains date part. Comparison will be omitted",
                                          propertyName));
                    }
                    else
                    {
                        if (timeout1 != timeout2)
                        {
                            ok = false;
                            sb.AppendLine(string.Format("{0} values are different ({1}, {2})", propertyName, value1,
                                                        value2));
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value1) || !string.IsNullOrEmpty(value2))
                    {
                        ok = false;
                        sb.AppendLine(string.Format("{0} is defined for only one configuration", propertyName));
                    }
                }
            }
            return ok;
        }

        void CompareConfigurations(AudioEncoderConfiguration configuration1, AudioEncoderConfiguration configuration2)
        {
            CompareConfigurations(configuration1, configuration2, false);
        }

        void CompareConfigurations(AudioEncoderConfiguration configuration1, 
            AudioEncoderConfiguration configuration2,
            bool displayToken)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = AreEqual(configuration1, configuration2, sb);

            string stepName = displayToken
                                  ? string.Format("Check that configurations [token = '{0}'] are the same",
                                                  configuration1.token)
                                  : "Check that configurations are the same";

            ProcessResults(stepName, ok, sb);
        }


        bool AreEqual(VideoSourceConfiguration configuration1, VideoSourceConfiguration configuration2, StringBuilder sb)
        {
            bool ok = true;

            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }
            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }
            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }

            if (configuration1.SourceToken != configuration2.SourceToken)
            {
                ok = false;
                sb.AppendLine("'SourceToken' properties are different");
            }

            // Bounds
            if (configuration1.Bounds != null && configuration2.Bounds != null)
            {
                IntRectangle rectangle1 = configuration1.Bounds;
                IntRectangle rectangle2 = configuration2.Bounds;

                if (!(rectangle1.height == rectangle2.height && rectangle1.width == rectangle2.width &&
                    rectangle1.x == rectangle2.x && rectangle1.y == rectangle2.y))
                {
                    ok = false;
                    sb.AppendLine("'Bounds' are different");
                }
            }
            else
            {
                if (!(configuration1.Bounds == null && configuration2.Bounds == null))
                {
                    ok = false;
                    sb.AppendLine("Bounds are defined only for one configuration");
                }
            }


            return ok;
        }

        void CompareConfigurations(VideoSourceConfiguration configuration1, VideoSourceConfiguration configuration2)
        {
            CompareConfigurations(configuration1, configuration2, false);
        }

        void CompareConfigurations(VideoSourceConfiguration configuration1, 
            VideoSourceConfiguration configuration2, bool displayToken)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = AreEqual(configuration1, configuration2, sb);

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump,
                displayToken ?
                string.Format("Check that configurations [token = '{0}'] are the same", configuration1.token) :
                "Check that configurations are the same");

        }


        bool AreEqual(VideoEncoderConfiguration configuration1, VideoEncoderConfiguration configuration2, StringBuilder sb)
        {
            bool ok = true;
            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }
            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }

            if (configuration1.Encoding != configuration2.Encoding)
            {
                ok = false;
                sb.AppendLine("'Encoding' properties are different");
            }

            if (configuration1.Quality != configuration2.Quality)
            {
                ok = false;
                sb.AppendLine("'Quality' properties are different");
            }

            // RateControl

            if (configuration1.RateControl != null && configuration2.RateControl != null)
            {
                VideoRateControl vrc1 = configuration1.RateControl;
                VideoRateControl vrc2 = configuration2.RateControl;

                if (vrc1.BitrateLimit != vrc2.BitrateLimit)
                {
                    ok = false;
                    sb.AppendLine("BitrateLimit values in RateControl settings are different");
                }

                if (vrc1.EncodingInterval != vrc2.EncodingInterval)
                {
                    ok = false;
                    sb.AppendLine("EncodingInterval values in RateControl settings are different");
                }

                if (vrc1.FrameRateLimit != vrc2.FrameRateLimit)
                {
                    ok = false;
                    sb.AppendLine("FrameRateLimit values in RateControl settings are different");
                }
            }
            else if (!(configuration1.RateControl == null && configuration2.RateControl == null))
            {
                ok = false;
                sb.AppendLine("RateControl settings are defined only for one configuration");
            }

            // Resolution

            if (configuration1.Resolution != null && configuration2.Resolution != null)
            {
                VideoResolution r1 = configuration1.Resolution;
                VideoResolution r2 = configuration2.Resolution;

                if (r1.Width != r2.Width)
                {
                    ok = false;
                    sb.AppendLine("Width values in Resolution settings are different");
                }
                if (r1.Height != r2.Height)
                {
                    ok = false;
                    sb.AppendLine("Height values in Resolution settings are different");
                }
            }
            else if (!(configuration1.Resolution == null && configuration2.Resolution == null))
            {
                ok = false;
                sb.AppendLine("Resolution settings are defined only for one configuration");
            }
            // SessionTimeout
            bool currentOk = TimeoutsAreEqual(configuration1.SessionTimeout,
                configuration2.SessionTimeout,
                "SessionTimeout",
                sb);

            ok = ok && currentOk;


            // Multicast
            if (configuration1.Multicast != null && configuration2.Multicast != null)
            {
                MulticastConfiguration multicast1 = configuration1.Multicast;
                MulticastConfiguration multicast2 = configuration2.Multicast;

                if (!EqualIPAddresses(multicast1.Address, multicast2.Address))
                {
                    ok = false;
                    sb.AppendLine("IP addresses in multicast configuration are different");
                }

                if (multicast1.AutoStart != multicast2.AutoStart)
                {
                    ok = false;
                    sb.AppendLine("'AutoStart' properties in multicast configuration are different");
                }

                if (multicast1.Port != multicast2.Port)
                {
                    ok = false;
                    sb.AppendLine("'Port' properties in multicast configuration are different");
                }

                if (multicast1.TTL != multicast2.TTL)
                {
                    ok = false;
                    sb.AppendLine("'TTL' properties in multicast configuration are different");
                }

            }
            else
            {
                if (!(configuration1.Multicast == null && configuration2.Multicast == null))
                {
                    ok = false;
                    sb.AppendLine("Multicast settings are defined only for one configuration");
                }
            }

            // H264

            if (configuration1.H264 != null && configuration2.H264 != null)
            {
                H264Configuration h1 = configuration1.H264;
                H264Configuration h2 = configuration2.H264;

                if (h1.GovLength != h2.GovLength)
                {
                    ok = false;
                    sb.AppendLine("GovLength values in H264 settings are different");
                }

                if (h1.H264Profile != h2.H264Profile)
                {
                    ok = false;
                    sb.AppendLine("H264Profile values in H264 settings are different");
                }
            }
            else if (!(configuration1.H264 == null && configuration2.H264 == null))
            {
                ok = false;
                sb.AppendLine("H264 settings are defined only for one configuration");
            }

            // MPEG4

            if (configuration1.MPEG4 != null && configuration2.MPEG4 != null)
            {
                Mpeg4Configuration m1 = configuration1.MPEG4;
                Mpeg4Configuration m2 = configuration2.MPEG4;

                if (m1.GovLength != m2.GovLength)
                {
                    ok = false;
                    sb.AppendLine("GovLength values in MPEG4 settings are different");
                }

                if (m1.Mpeg4Profile != m2.Mpeg4Profile)
                {
                    ok = false;
                    sb.AppendLine("Mpeg4Profile values in MPEG4 settings are different");
                }
            }
            else if (!(configuration1.MPEG4 == null && configuration2.MPEG4 == null))
            {
                ok = false;
                sb.AppendLine("MPEG4 settings are defined only for one configuration");
            }


            return ok;
        }

        void CompareConfigurations(VideoEncoderConfiguration configuration1, VideoEncoderConfiguration configuration2)
        {
            CompareConfigurations(configuration1, configuration2, false);
        }

        void CompareConfigurations(VideoEncoderConfiguration configuration1, 
            VideoEncoderConfiguration configuration2, bool displayToken)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = AreEqual(configuration1, configuration2, sb);

            string stepName =
                displayToken
                    ? string.Format("Check that configurations [token = '{0}'] are the same", configuration1.token)
                    : "Check that configurations are the same";

            ProcessResults(stepName, ok, sb);
        }
        
        void CompareConfigurations(Proxies.Onvif.PTZConfiguration configuration1,
            Proxies.Onvif.PTZConfiguration configuration2)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = AreEqual(configuration1, configuration2, sb);
            
            ProcessResults("Check that configurations are the same", ok, sb);
            
        }
        
        bool AreSetCorrectly(AudioOutputConfiguration expected,
            AudioOutputConfiguration actual, StringBuilder sb)
        {
            bool ok = true;
            if (expected.Name != actual.Name)
            {
                ok = false;
                sb.AppendLine(string.Format("Name does not match. Expected: {0}, actual: {1}", expected.Name, actual.Name));
            }

            if (expected.OutputToken != actual.OutputToken)
            {
                ok = false;
                sb.AppendLine(string.Format("OutputToken does not match. Expected: {0}, actual: {1}", expected.OutputToken, actual.OutputToken));
            }

            if (expected.SendPrimacy != actual.SendPrimacy)
            {
                ok = false;
                sb.AppendLine(string.Format("SendPrimacy does not match. Expected: {0}, actual: {1}", expected.SendPrimacy, actual.SendPrimacy));
            }

            if (expected.OutputLevel != actual.OutputLevel)
            {
                ok = false;
                sb.AppendLine(string.Format("OutputLevel does not match. Expected: {0}, actual: {1}", expected.OutputLevel, actual.OutputLevel));
            }

            return ok;
        }

        void CheckConfiguration(AudioOutputConfiguration expected,
            AudioOutputConfiguration actual, string stepName)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = AreSetCorrectly(expected, actual, sb);

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump, stepName);
        }


        bool AreEqual(MetadataConfiguration configuration1, 
            MetadataConfiguration configuration2, 
            StringBuilder sb)
        {
            bool ok = true;

            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }
            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }

            if (configuration1.AnalyticsSpecified != configuration1.AnalyticsSpecified)
            {
                ok = false;
                sb.AppendLine("'AnalyticsSpecified' properties are different");
            }

            if ((configuration1.AnalyticsSpecified && configuration2.AnalyticsSpecified))
            {
                if (configuration1.Analytics != configuration1.Analytics)
                {
                    ok = false;
                    sb.AppendLine("'Analytics' properties are different");
                }
            }

            if (configuration1.Events != null && configuration2.Events != null)
            {
                if ( (configuration1.Events.Filter == null) != (configuration2.Events.Filter == null))
                {
                    ok = false;
                    sb.AppendLine("'Filter' properties in Events are defined only for one configuration");
                }

                if ((configuration1.Events.SubscriptionPolicy == null) != (configuration2.Events.SubscriptionPolicy == null))
                {
                    ok = false;
                    sb.AppendLine("'SubscriptionPolicy' properties in Events are defined only for one configuration");
                }
            }
            else
            {
                if (!(configuration1.Events == null && configuration2.Events == null))
                {
                    ok = false;
                    sb.AppendLine("Events for configuration is returned by only one command");
                }
            }

            bool multicastOk = AreEqual(configuration1.Multicast, configuration2.Multicast, sb);
            ok = ok && multicastOk;

            if (configuration1.PTZStatus != null && configuration2.PTZStatus != null)
            {
                if (configuration1.PTZStatus.Position != configuration2.PTZStatus.Position)
                {
                    ok = false;
                    sb.AppendLine("'Position' properties in PTZStatus are different");
                }

                if (configuration1.PTZStatus.Status != configuration2.PTZStatus.Status)
                {
                    ok = false;
                    sb.AppendLine("'Status' properties in PTZStatus are different");
                }
            }
            else
            {
                if (!(configuration1.PTZStatus == null && configuration2.PTZStatus == null))
                {
                    ok = false;
                    sb.AppendLine("PTZStatus for configuration is returned by only one command");
                }
            }

            if (!TimeoutsAreEqual(configuration1.SessionTimeout, 
                configuration2.SessionTimeout, 
                "SessionTimeout", sb))
            {
                ok = false;
            }


            return ok;
        }

        bool AreEqual(PTZConfiguration configuration1, 
            PTZConfiguration configuration2, 
            StringBuilder sb)
        {
            bool ok = true;
            bool currentOk = true;

            //Name
            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }
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
                                             configuration2.DefaultAbsolutePantTiltPositionSpace, "DefaultAbsolutePantTiltPositionSpace", sb);
            ok = ok && currentOk;

            //DefaultAbsoluteZoomPositionSpace
            currentOk = StringsAreEqual(configuration1.DefaultAbsoluteZoomPositionSpace,
                                 configuration2.DefaultAbsoluteZoomPositionSpace,
                                 "DefaultAbsoluteZoomPositionSpace",
                                 sb);
            ok = ok && currentOk;

            //DefaultContinuousPanTiltVelocitySpace
            currentOk = StringsAreEqual(configuration1.DefaultContinuousPanTiltVelocitySpace,
                     configuration2.DefaultContinuousPanTiltVelocitySpace,
                     "DefaultContinuousPanTiltVelocitySpace",
                     sb);
            ok = ok && currentOk;

            //DefaultContinuousZoomVelocitySpace
            currentOk = StringsAreEqual(configuration1.DefaultContinuousZoomVelocitySpace,
                 configuration2.DefaultContinuousZoomVelocitySpace,
                 "DefaultContinuousZoomVelocitySpace",
                 sb);
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
                    sb.AppendLine("DefaultPTZSpeed is defined for only one configuration");
                }
            }


            //DefaultPTZTimeout
            currentOk = TimeoutsAreEqual(configuration1.DefaultPTZTimeout,
                configuration2.DefaultPTZTimeout,
                "DefaultPTZTimeout",
                sb); 
            ok = ok && currentOk;


            //DefaultRelativePanTiltTranslationSpace
            currentOk = StringsAreEqual(configuration1.DefaultRelativePanTiltTranslationSpace,
                 configuration2.DefaultRelativePanTiltTranslationSpace,
                 "DefaultRelativePanTiltTranslationSpace",
                 sb);
            ok = ok && currentOk;

            //DefaultRelativeZoomTranslationSpace
            currentOk = StringsAreEqual(configuration1.DefaultRelativeZoomTranslationSpace,
                 configuration2.DefaultRelativeZoomTranslationSpace,
                 "DefaultRelativeZoomTranslationSpace",
                 sb);
            ok = ok && currentOk;

            //NodeToken
            currentOk = StringsAreEqual(configuration1.NodeToken,
                 configuration2.NodeToken,
                 "NodeToken",
                 sb);
            ok = ok && currentOk;

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

            return ok;
        }
        
        bool AreEqual(VideoAnalyticsConfiguration configuration1, 
            VideoAnalyticsConfiguration configuration2, 
            StringBuilder sb)
        {
            bool ok = true;

            if (configuration1.Name != configuration2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }

            if (configuration1.token != configuration2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            if (configuration1.UseCount != configuration2.UseCount)
            {
                ok = false;
                sb.AppendLine("'UseCount' properties are different");
            }
            
            if (configuration1.AnalyticsEngineConfiguration != null && configuration2.AnalyticsEngineConfiguration != null)
            {
                if (configuration1.AnalyticsEngineConfiguration.AnalyticsModule != null && 
                    configuration2.AnalyticsEngineConfiguration.AnalyticsModule != null)
                {
                    sb.AppendLine("Compare AnalyticsModule in AnalyticsEngineConfiguration");
                    bool localOk = AreEqual(configuration1.AnalyticsEngineConfiguration.AnalyticsModule,
                                        configuration2.AnalyticsEngineConfiguration.AnalyticsModule, 
                                        sb);
                    ok = ok && localOk;
                }
                else if (!(configuration1.AnalyticsEngineConfiguration.AnalyticsModule == null && 
                    configuration2.AnalyticsEngineConfiguration.AnalyticsModule == null))
                {
                    ok = false;
                    sb.AppendLine("AnalyticsModule in AnalyticsEngineConfiguration is defined only for one configuration");
                }
            }
            else if (!(configuration1.AnalyticsEngineConfiguration == null && configuration2.AnalyticsEngineConfiguration == null))
            {
                ok = false;
                sb.AppendLine("AnalyticsEngineConfiguration is defined only for one configuration");
            }

            if (configuration1.RuleEngineConfiguration != null && configuration2.RuleEngineConfiguration != null)
            {
                if (configuration1.RuleEngineConfiguration.Rule != null &&
                    configuration2.RuleEngineConfiguration.Rule != null)
                {
                    sb.AppendLine("Compare Rule in RuleEngineConfiguration");
                    bool localOk = AreEqual(configuration1.RuleEngineConfiguration.Rule,
                                        configuration2.RuleEngineConfiguration.Rule,
                                        sb);
                    ok = ok && localOk;
                }
                else if (!(configuration1.RuleEngineConfiguration.Rule == null &&
                    configuration2.RuleEngineConfiguration.Rule == null))
                {
                    ok = false;
                    sb.AppendLine("Rule in RuleEngineConfiguration is defined only for one configuration");
                }


            }
            else if (!(configuration1.RuleEngineConfiguration == null && configuration2.RuleEngineConfiguration == null))
            {
                ok = false;
                sb.AppendLine("RuleEngineConfiguration is defined only for one configuration");
            }

            return ok;
        }

        bool AreEqual(ProfileExtension extension1, 
            ProfileExtension extension2, 
            StringBuilder sb)
        {
            bool ok = true;

            if (extension1.AudioOutputConfiguration != null && extension2.AudioOutputConfiguration != null)
            {
                 sb.AppendLine("Compare AudioOutputConfiguration in Extension");
                 ok = ok && AreEqual(extension1.AudioOutputConfiguration, extension2.AudioOutputConfiguration, sb);
            }
            else if (!(extension1.AudioOutputConfiguration == null && extension2.AudioOutputConfiguration == null))
            {
                ok = false;
                sb.AppendLine("AudioOutputConfiguration is defined only for one configuration");
            }  



            return ok;
        }

        void CompareProfiles(Profile profile1, Profile profile2)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = true;
            bool currentOk = true;

            if (profile1.Name != profile2.Name)
            {
                ok = false;
                sb.AppendLine("'Name' properties are different");
            }
            if (profile1.token != profile2.token)
            {
                ok = false;
                sb.AppendLine("'token' properties are different");
            }

            if (profile1.fixedSpecified != profile2.fixedSpecified)
            {
                ok = false;
                sb.AppendLine("'fixedSpecified' properties are different");
            }

            if (profile1.@fixed != profile2.@fixed)
            {
                ok = false;
                sb.AppendLine("'fixed' properties are different");
            }

            if (profile1.AudioEncoderConfiguration != null &&
                profile2.AudioEncoderConfiguration != null)
            {
                sb.AppendLine("Compare AudioEncoderConfiguration");
                currentOk =
                     AreEqual(profile1.AudioEncoderConfiguration, profile2.AudioEncoderConfiguration, sb);
                ok = ok && currentOk;
            }
            else
            {
                if (! (profile1.AudioEncoderConfiguration == null && profile2.AudioEncoderConfiguration == null))
                {
                    ok = false;
                    sb.AppendLine("AudioEncoderConfiguration for profile is returned by only one command");
                }
            }

            if (profile1.AudioSourceConfiguration != null && profile2.AudioSourceConfiguration != null)
            {
                sb.AppendLine("Compare AudioSourceConfiguration");

                currentOk =
                     AreEqual(profile1.AudioSourceConfiguration, profile2.AudioSourceConfiguration, sb);
                ok = ok && currentOk;
            }
            else
            {
                if (!(profile1.AudioSourceConfiguration == null && profile2.AudioSourceConfiguration == null))
                {
                    ok = false;
                    sb.AppendLine("AudioSourceConfiguration for profile is returned by only one command");
                }
            }

            // Extension
            if (profile1.Extension != null && profile2.Extension != null)
            {
                sb.AppendLine("Compare Extension");

                currentOk =
                     AreEqual(profile1.Extension, profile2.Extension, sb);
                ok = ok && currentOk;

            }
            else
            {
                if (!(profile1.Extension == null && profile2.Extension == null))
                {
                    ok = false;
                    sb.AppendLine("Extension for profile is returned by only one command");
                }
            }
            
            // Metadata

            if (profile1.MetadataConfiguration != null && profile2.MetadataConfiguration != null)
            {
                sb.AppendLine("Compare MetadataConfiguration");

                currentOk =
                     AreEqual(profile1.MetadataConfiguration, profile2.MetadataConfiguration, sb);
                ok = ok && currentOk;
            }
            else
            {
                if (!(profile1.MetadataConfiguration == null && profile2.MetadataConfiguration == null))
                {
                    ok = false;
                    sb.AppendLine("MetadataConfiguration for profile is returned by only one command");
                }
            }


            // PTZ
            if (profile1.PTZConfiguration != null && profile2.PTZConfiguration != null)
            {
                sb.AppendLine("Compare PTZConfiguration ");

                currentOk =
                     AreEqual(profile1.PTZConfiguration, profile2.PTZConfiguration, sb);
                ok = ok && currentOk;
            }
            else
            {
                if (!(profile1.PTZConfiguration == null && profile2.PTZConfiguration == null))
                {
                    ok = false;
                    sb.AppendLine("PTZConfiguration for profile is returned by only one command");
                }
            }


            // VideoAnalytics
            if (profile1.VideoAnalyticsConfiguration != null && profile2.VideoAnalyticsConfiguration != null)
            {
                sb.AppendLine("Compare VideoAnalyticsConfiguration");

                currentOk =
                     AreEqual(profile1.VideoAnalyticsConfiguration, profile2.VideoAnalyticsConfiguration, sb);
                ok = ok && currentOk;
            }
            else
            {
                if (!(profile1.VideoAnalyticsConfiguration == null && profile2.VideoAnalyticsConfiguration == null))
                {
                    ok = false;
                    sb.AppendLine("VideoAnalyticsConfiguration for profile is returned by only one command");
                }
            }

            if (profile1.VideoEncoderConfiguration != null && profile2.VideoEncoderConfiguration != null)
            {
                sb.AppendLine("Compare VideoEncoderConfiguration");

                currentOk =
                     AreEqual(profile1.VideoEncoderConfiguration, profile2.VideoEncoderConfiguration, sb);
                ok = ok && currentOk;
            }
            else
            {
                if (!(profile1.VideoEncoderConfiguration == null && profile2.VideoEncoderConfiguration == null))
                {
                    ok = false;
                    sb.AppendLine("VideoEncoderConfiguration for profile is returned by only one command");
                }
            }

            if (profile1.VideoSourceConfiguration != null && profile2.VideoSourceConfiguration != null)
            {
                sb.AppendLine("Compare VideoSourceConfiguration");

                currentOk =
                     AreEqual(profile1.VideoSourceConfiguration, profile2.VideoSourceConfiguration, sb);
                ok = ok && currentOk;
            }
            else
            {
                if (!(profile1.VideoSourceConfiguration == null && profile2.VideoSourceConfiguration == null))
                {
                    ok = false;
                    sb.AppendLine("VideoSourceConfiguration for profile is returned by only one command");
                }
            }

            string dump = sb.ToStringTrimNewLine();

            BeginStep(string.Format("Check that profiles [token = '{0}'] are the same", profile1.token));
            if (!ok)
            {
                throw new AssertException(dump);
            }
            else
            {
                if (!string.IsNullOrEmpty(dump))
                {
                    LogStepEvent(dump);
                }
            }
            StepPassed();
        }

        void ProcessResults(string stepName, bool ok, StringBuilder sb)
        {
            string dump = sb.ToStringTrimNewLine();

            BeginStep(stepName);
            if (!ok)
            {
                throw new AssertException(dump);
            }
            else
            {
                if (!string.IsNullOrEmpty(dump))
                {
                    LogStepEvent(dump);
                }
            }
            StepPassed();
        }
        
        #region VideoSource

        void ValidateOptions(VideoSourceConfigurationOptions options)
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();

            if (options.VideoSourceTokensAvailable == null)
            {
                ok = false;
                sb.AppendLine("No video source tokens available found in video source configuration options");
            }

            if (options.BoundsRange == null)
            {
                ok = false;
                sb.AppendLine("Bounds range not found in video source configuration options");
            }
            else
            {
                if (options.BoundsRange.HeightRange == null)
                {
                    ok = false;
                    sb.AppendLine("Height range not found in video source configuration options");
                }
                if (options.BoundsRange.WidthRange == null)
                {
                    ok = false;
                    sb.AppendLine("Width range not found in video source configuration options");
                }
                if (options.BoundsRange.XRange == null)
                {
                    ok = false;
                    sb.AppendLine("X range not found in video source configuration options");
                }
                if (options.BoundsRange.YRange == null)
                {
                    ok = false;
                    sb.AppendLine("Y range not found in video source configuration options");
                }
            }

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump,
                string.Format("Check if video source configuration options are valid"));

        }

        void ValidateConfiguration(VideoSourceConfiguration configuration)
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();
            if (configuration.Bounds == null)
            {
                ok = false;
                sb.AppendLine("Bounds not defined");
            }

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump, string.Format("Check if video source configuration is valid"));

        }

        void CheckOptions(VideoSourceConfigurationOptions options, VideoSourceConfiguration configuration)
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();

            if (!options.VideoSourceTokensAvailable.Contains(configuration.SourceToken))
            {
                ok = false;
                sb.AppendLine(string.Format("Video source token '{0}' not present in list of tokes available",
                                            configuration.SourceToken));
            }

            if (!options.BoundsRange.HeightRange.Contains(configuration.Bounds.height))
            {
                ok = false;
                sb.AppendLine(string.Format("Height ({0}) is out of range ([{1}, {2}])",
                    configuration.Bounds.height,
                    options.BoundsRange.HeightRange.Min,
                    options.BoundsRange.HeightRange.Max));
            }

            if (!options.BoundsRange.WidthRange.Contains(configuration.Bounds.width))
            {
                ok = false;
                sb.AppendLine(string.Format("Width ({0}) is out of range ([{1}, {2}])",
                    configuration.Bounds.width,
                    options.BoundsRange.WidthRange.Min,
                    options.BoundsRange.WidthRange.Max));
            }

            if (!options.BoundsRange.XRange.Contains(configuration.Bounds.x))
            {
                ok = false;
                sb.AppendLine(string.Format("X ({0}) is out of range ([{1}, {2}])",
                    configuration.Bounds.x,
                    options.BoundsRange.XRange.Min,
                    options.BoundsRange.XRange.Max));
            }

            if (!options.BoundsRange.YRange.Contains(configuration.Bounds.y))
            {
                ok = false;
                sb.AppendLine(string.Format("Y ({0}) is out of range ([{1}, {2}])",
                    configuration.Bounds.y,
                    options.BoundsRange.YRange.Min,
                    options.BoundsRange.YRange.Max));
            }

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump,
                string.Format("Check if video source configuration [token='{0}'] and options are consistent", configuration.token));

        }

        #endregion

        #region Video encoder

        void ValidateConfiguration(VideoEncoderConfiguration configuration)
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();

            if (configuration.RateControl == null)
            {
                ok = false;
                sb.AppendLine("RateControl in JPEG options not defined");
            }

            switch (configuration.Encoding)
            {
                case VideoEncoding.JPEG:
                    {

                    }
                    break;
                case VideoEncoding.MPEG4:
                    {
                        if (configuration.MPEG4 == null)
                        {
                            ok = false;
                            sb.AppendLine("MPEG4 configuration not found");
                        }
                    }
                    break;
                case VideoEncoding.H264:
                    {
                        if (configuration.H264 == null)
                        {
                            ok = false;
                            sb.AppendLine("H264 configuration not found");
                        }
                    }
                    break;
            }

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump,
                string.Format("Check if video encoder configuration is valid"));


        }

        void CheckOptions(VideoEncoderConfigurationOptions options, VideoEncoderConfiguration configuration)
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();

            if (options.QualityRange != null)
            {
                if (!options.QualityRange.Contains(configuration.Quality))
                {
                    ok = false;
                    sb.AppendLine(string.Format("Quality ({0}) is out of range ([{1}, {2}])",
                                                configuration.Quality,
                                                options.QualityRange.Min,
                                                options.QualityRange.Max));
                }
            }

            switch (configuration.Encoding)
            {
                case VideoEncoding.JPEG:
                    {
                        if (options.JPEG != null)
                        {
                            if (!options.JPEG.ResolutionsAvailable.ContainsResolution(configuration.Resolution))
                            {
                                ok = false;
                                sb.AppendLine("Resolution not found in options");
                            }

                            if (configuration.RateControl != null)
                            {
                                if (options.JPEG.FrameRateRange != null)
                                {
                                    if (!options.JPEG.FrameRateRange.Contains(configuration.RateControl.FrameRateLimit))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("FrameRateLimit ({0}) is out of range ([{1}, {2}])",
                                                                    configuration.RateControl.FrameRateLimit,
                                                                    options.JPEG.FrameRateRange.Min,
                                                                    options.JPEG.FrameRateRange.Max));
                                    }
                                }

                                if (options.JPEG.EncodingIntervalRange != null)
                                {
                                    if (!options.JPEG.EncodingIntervalRange.Contains(
                                             configuration.RateControl.EncodingInterval))
                                    {
                                        ok = false;
                                        sb.AppendLine(
                                            string.Format("EncodingInterval ({0}) is out of range ([{1}, {2}])",
                                                          configuration.RateControl.EncodingInterval,
                                                          options.JPEG.EncodingIntervalRange.Min,
                                                          options.JPEG.EncodingIntervalRange.Max));
                                    }
                                }

                                if (options.Extension != null && 
                                    options.Extension.JPEG != null &&
                                    options.Extension.JPEG.BitrateRange != null)
                                {
                                    if (!options.Extension.JPEG.BitrateRange.Contains(
                                             configuration.RateControl.BitrateLimit))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("BitrateLimit ({0}) is out of range ([{1}, {2}])",
                                                                    configuration.RateControl.BitrateLimit,
                                                                    options.Extension.JPEG.BitrateRange.Min,
                                                                    options.Extension.JPEG.BitrateRange.Max));
                                    }
                                }
                            }
                            else
                            {
                                if ((options.JPEG.FrameRateRange != null) || 
                                    (options.JPEG.EncodingIntervalRange != null) || 
                                    (options.Extension != null && 
                                    options.Extension.JPEG != null &&
                                    options.Extension.JPEG.BitrateRange != null))
                                {
                                    ok = false;
                                    sb.AppendLine(
                                        "RateControl values ranges are defined in options, but not found in JPEG configuration.");
                                }
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine("JPEG options not defined");
                        }
                    }
                    break;
                case VideoEncoding.MPEG4:
                    {
                        if (options.MPEG4 != null)
                        {
                            if (options.MPEG4.ResolutionsAvailable != null)
                            {
                                if (!options.MPEG4.ResolutionsAvailable.ContainsResolution(configuration.Resolution))
                                {
                                    ok = false;
                                    sb.AppendLine("Resolution not found in options");
                                }
                            }

                            if (configuration.RateControl != null)
                            {
                                if (options.MPEG4.FrameRateRange != null)
                                {
                                    if (!options.MPEG4.FrameRateRange.Contains(configuration.RateControl.FrameRateLimit))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("FrameRateLimit ({0}) is out of range ([{1}, {2}])",
                                            configuration.RateControl.FrameRateLimit,
                                            options.MPEG4.FrameRateRange.Min,
                                            options.MPEG4.FrameRateRange.Max));
                                    }                            
                                }

                                if (options.MPEG4.EncodingIntervalRange != null)
                                {
                                    if (!options.MPEG4.EncodingIntervalRange.Contains(
                                            configuration.RateControl.EncodingInterval))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("EncodingInterval ({0}) is out of range ([{1}, {2}])",
                                            configuration.RateControl.EncodingInterval,
                                            options.MPEG4.EncodingIntervalRange.Min,
                                            options.MPEG4.EncodingIntervalRange.Max));
                                    }
                                }

                                if (options.Extension != null && 
                                    options.Extension.MPEG4 != null && 
                                    options.Extension.MPEG4.BitrateRange != null)
                                {
                                    if (
                                        !options.Extension.MPEG4.BitrateRange.Contains(
                                             configuration.RateControl.BitrateLimit))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("BitrateLimit ({0}) is out of range ([{1}, {2}])",
                                                                    configuration.RateControl.BitrateLimit,
                                                                    options.Extension.MPEG4.BitrateRange.Min,
                                                                    options.Extension.MPEG4.BitrateRange.Max));
                                    }
                                }
                            }
                            else
                            {
                                if ((options.MPEG4.FrameRateRange != null) || 
                                    (options.MPEG4.EncodingIntervalRange != null) || 
                                    (options.Extension != null && 
                                    options.Extension.MPEG4 != null &&
                                    options.Extension.MPEG4.BitrateRange != null))
                                {
                                    ok = false;
                                    sb.AppendLine(
                                        "RateControl values ranges are defined in options, but not found in MPEG4 configuration.");
                                }

                            }

                            if (configuration.MPEG4 != null)
                            {
                                if (options.MPEG4.Mpeg4ProfilesSupported != null)
                                {
                                    if (!options.MPEG4.Mpeg4ProfilesSupported.Contains(configuration.MPEG4.Mpeg4Profile))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("MPEG4 profile {0} not found in supported profiles",
                                                                    configuration.MPEG4.Mpeg4Profile));
                                    }
                                }

                                if (options.MPEG4.GovLengthRange != null)
                                {
                                    if (!options.MPEG4.GovLengthRange.Contains(configuration.MPEG4.GovLength))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("GovLength ({0}) is out of range ([{1}, {2}])",
                                                                    configuration.MPEG4.GovLength,
                                                                    options.MPEG4.GovLengthRange.Min,
                                                                    options.MPEG4.GovLengthRange.Max));
                                    }
                                }
                            }
                            else
                            {
                                ok = false;
                                sb.AppendLine("MPEG4 configuration not defined");
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine("MPEG4 options not defined");
                        }
                    }
                    break;
                case VideoEncoding.H264:
                    {
                        if (options.H264 != null)
                        {
                            if (options.H264.ResolutionsAvailable != null)
                            {
                                if (!options.H264.ResolutionsAvailable.ContainsResolution(configuration.Resolution))
                                {
                                    ok = false;
                                    sb.AppendLine("Resolution not found in options");
                                }                                
                            }
                            else
                            {
                                ok = false;
                                sb.AppendLine("Resolution not found in options");
                            }

                            if (configuration.RateControl != null)
                            {
                                if (options.H264.FrameRateRange != null)
                                {
                                    if (!options.H264.FrameRateRange.Contains(configuration.RateControl.FrameRateLimit))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("FrameRateLimit ({0}) is out of range ([{1}, {2}])",
                                            configuration.RateControl.FrameRateLimit,
                                            options.H264.FrameRateRange.Min,
                                            options.H264.FrameRateRange.Max));
                                    }
                                }

                                if (options.H264.EncodingIntervalRange != null)
                                {
                                    if (!options.H264.EncodingIntervalRange.Contains(
                                        configuration.RateControl.EncodingInterval))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("EncodingInterval ({0}) is out of range ([{1}, {2}])",
                                            configuration.RateControl.EncodingInterval,
                                            options.H264.EncodingIntervalRange.Min,
                                            options.H264.EncodingIntervalRange.Max));
                                    }
                                }
                                else
                                {
                                    
                                }

                                if (options.Extension != null && 
                                    options.Extension.H264 != null && 
                                    options.Extension.H264.BitrateRange != null )
                                {
                                    if (
                                        !options.Extension.H264.BitrateRange.Contains(configuration.RateControl.BitrateLimit))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("BitrateLimit ({0}) is out of range ([{1}, {2}])",
                                                                    configuration.RateControl.BitrateLimit,
                                                                    options.Extension.H264.BitrateRange.Min,
                                                                    options.Extension.H264.BitrateRange.Max));
                                    }
                                }                            
                            }
                            else
                            {
                                if ((options.H264.FrameRateRange != null) || 
                                    (options.H264.EncodingIntervalRange != null) || 
                                    (options.Extension != null && 
                                    options.Extension.H264 != null &&
                                    options.Extension.H264.BitrateRange != null))
                                {
                                    ok = false;
                                    sb.AppendLine(
                                        "RateControl values ranges are defined in options, but not found in H264 configuration.");
                                }
                            }
                            if (configuration.H264 != null)
                            {
                                if (options.H264.H264ProfilesSupported != null)
                                {
                                    if (!options.H264.H264ProfilesSupported.Contains(configuration.H264.H264Profile))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("H264 profile {0} not found in supported profiles",
                                                                    configuration.H264.H264Profile));
                                    }                                
                                }
                                
                                if (options.H264.GovLengthRange != null)
                                {
                                    if (!options.H264.GovLengthRange.Contains(configuration.H264.GovLength))
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("GovLength ({0}) is out of range ([{1}, {2}])",
                                            configuration.H264.GovLength,
                                            options.H264.GovLengthRange.Min,
                                            options.H264.GovLengthRange.Max));
                                    }
                                }
                            }
                            else
                            {
                                ok = false;
                                sb.AppendLine("H264 configuration not defined");
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine("H264 options not defined");
                        }
                    }
                    break;
            }

            string dump = sb.ToStringTrimNewLine();
            
            Assert(ok, dump,
                string.Format("Check if video encoder configuration [token='{0}'] and options are consistent", configuration.token));

        }

        #endregion

        #region Audio Encoder

        void CheckOptionsExist(AudioEncoderConfigurationOptions options, AudioEncoderConfiguration configuration)
        {
            bool found = false;
            if (options.Options != null)
            {
                foreach (AudioEncoderConfigurationOption option in options.Options)
                {
                    if (option.Encoding == configuration.Encoding)
                    {
                        if (option.BitrateList != null && option.SampleRateList != null)
                        {
                            if (option.BitrateList.Contains(configuration.Bitrate) &&
                                option.SampleRateList.Contains(configuration.SampleRate))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
            Assert(found, "Suitable audio encoder configuration option not found",
                string.Format("Check if options list contain option for configuration with token '{0}'", configuration.token));

        }

        #endregion

        #region Profiles

        Profile CreateTestProfile(out bool existing)
        {
            Profile newProfile = null;

            Profile[] profiles = GetProfiles();

            List<string> names;
            List<string> tokens;
            if (profiles != null)
            {
                names = profiles.Select(P => P.Name).ToList();
                tokens = profiles.Select(P => P.token).ToList();
            }
            else
            {
                names = new List<string>();
                tokens = new List<string>();
            }

            string name = names.GetNonMatchingString();
            string token = tokens.GetNonMatchingString();

            existing = false;
            RunStep(() => { newProfile = Client.CreateProfile(name, token); },
                string.Format(Resources.StepCreateMediaProfile_Format, name),
                "Sender/Action/MaxNVTProfiles", true, true);

            DoRequestDelay();

            if (newProfile == null)
            {
                // try to update some existing profile

                BeginStep("Select existing profile for test");

                if (profiles != null)
                {
                    foreach (Profile p in profiles)
                    {
                        if (!(p.fixedSpecified && p.@fixed))
                        {
                            newProfile = p;
                            existing = true;
                            break;
                        }
                    }
                }

                if (newProfile == null)
                {
                    LogStepEvent("No possibility to use existing profile - skip this test");
                }

                StepPassed();

            }
            return newProfile;
        }
 
        void ResetProfile(Profile profile)
        {
            LogTestEvent("Restore profile used for test" + Environment.NewLine);

            Profile actual = GetProfile(profile.token, "Get actual profile");
            
#if ERRATA

            if (profile.VideoEncoderConfiguration == null)
            {
                if (actual.VideoEncoderConfiguration != null)
                {
                    RemoveVideoEncoderConfiguration(profile.token);
                }
            }   
         
            if (profile.AudioEncoderConfiguration == null)
            {
                if (actual.AudioEncoderConfiguration != null)
                {
                    RemoveAudioEncoderConfiguration(profile.token);
                }
            }

            if (profile.VideoSourceConfiguration != null)
            {
                if (actual.VideoSourceConfiguration == null ||
                    actual.VideoSourceConfiguration.token != profile.VideoSourceConfiguration.token)
                {
                    AddVideoSourceConfiguration(profile.token, profile.VideoSourceConfiguration.token);
                }
            }   
            else 
            {
                if (actual.VideoSourceConfiguration != null)
                {
                    RemoveVideoSourceConfiguration(profile.token);
                }
            }

            if (profile.AudioSourceConfiguration != null)
            {
                if (actual.AudioSourceConfiguration == null ||
                    actual.AudioSourceConfiguration.token != profile.AudioSourceConfiguration.token)
                {
                    AddAudioSourceConfiguration(profile.token, profile.AudioSourceConfiguration.token);
                }
            }
            else
            {
                if (actual.AudioSourceConfiguration != null)
                {
                    RemoveAudioSourceConfiguration(profile.token);
                }
            }
            
            if (profile.VideoEncoderConfiguration != null)
            {
                if (actual.VideoEncoderConfiguration == null ||
                    actual.VideoEncoderConfiguration.token != profile.VideoEncoderConfiguration.token)
                {
                    AddVideoEncoderConfiguration(profile.token, profile.VideoEncoderConfiguration.token);
                }
            }            
            
            if (profile.AudioEncoderConfiguration != null)
            {
                if (actual.AudioEncoderConfiguration == null || 
                    actual.AudioEncoderConfiguration.token != profile.AudioEncoderConfiguration.token)
                {
                    AddAudioEncoderConfiguration(profile.token, profile.AudioEncoderConfiguration.token);
                }
            }

#else
            if (profile.AudioSourceConfiguration == null)
            {
                if (actual.AudioSourceConfiguration != null)
                {
                    RemoveAudioSourceConfiguration(profile.token);
                }
            }
            else
            {
                if (actual.AudioSourceConfiguration == null ||
                    actual.AudioSourceConfiguration.token != profile.AudioSourceConfiguration.token)
                {
                    AddAudioSourceConfiguration(profile.token, profile.AudioSourceConfiguration.token);
                }
            }

            if (profile.AudioEncoderConfiguration == null)
            {
                if (actual.AudioEncoderConfiguration != null)
                {
                    RemoveAudioEncoderConfiguration(profile.token);
                }
            }
            else
            {
                if (actual.AudioEncoderConfiguration == null ||
                    actual.AudioEncoderConfiguration.token != profile.AudioEncoderConfiguration.token)
                {
                    AddAudioEncoderConfiguration(profile.token, profile.AudioEncoderConfiguration.token);
                }
            }

            if (profile.VideoSourceConfiguration == null)
            {
                if (actual.VideoSourceConfiguration != null)
                {
                    RemoveVideoSourceConfiguration(profile.token);
                }
            }
            else
            {
                if (actual.VideoSourceConfiguration == null ||
                    actual.VideoSourceConfiguration.token != profile.VideoSourceConfiguration.token)
                {
                    AddVideoSourceConfiguration(profile.token, profile.VideoSourceConfiguration.token);
                }
            }

            if (profile.VideoEncoderConfiguration == null)
            {
                if (actual.VideoEncoderConfiguration != null)
                {
                    RemoveVideoEncoderConfiguration(profile.token);
                }
            }
            else
            {
                if (actual.VideoEncoderConfiguration == null ||
                    actual.VideoEncoderConfiguration.token != profile.VideoEncoderConfiguration.token)
                {
                    AddVideoEncoderConfiguration(profile.token, profile.VideoEncoderConfiguration.token);
                }
            }

#endif

        }


        void RestoreProfile(Profile profile)
        {
            LogTestEvent("Restore profile used for test" + Environment.NewLine);

            CreateProfile(profile.Name, profile.token);

            if (profile.AudioSourceConfiguration != null)
            {
                AddAudioSourceConfiguration(profile.token, profile.AudioSourceConfiguration.token);
            }

            if (profile.AudioEncoderConfiguration != null)
            {
                AddAudioEncoderConfiguration(profile.token, profile.AudioEncoderConfiguration.token);
            }

            if (profile.VideoSourceConfiguration != null)
            {
                AddVideoSourceConfiguration(profile.token, profile.VideoSourceConfiguration.token);
            }

            if (profile.VideoEncoderConfiguration != null)
            {
                AddVideoEncoderConfiguration(profile.token, profile.VideoEncoderConfiguration.token);
            }
            
            if (profile.MetadataConfiguration != null)
            {
                AddMetadataConfiguration(profile.token, profile.MetadataConfiguration.token);
            }

            if (profile.PTZConfiguration != null)
            {
                AddPTZConfiguration(profile.token, profile.PTZConfiguration.token);
            }
            if (profile.VideoAnalyticsConfiguration != null)
            {
                AddVideoAnalyticsConfiguration(profile.token, profile.VideoAnalyticsConfiguration.token);
            }
        }

        #endregion

        #endregion

    }
}