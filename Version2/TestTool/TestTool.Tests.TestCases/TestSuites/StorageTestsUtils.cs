using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    class StorageTestsUtils
    {
        public static void ValidateFullRecordingsList(GetRecordingsResponseItem[] fullList, AssertDelegate assert)
        {
            // check that tokens are unique
            bool tokensOk = true;
            StringBuilder logger = new StringBuilder();

            List<string> logged = new List<string>();

            foreach (GetRecordingsResponseItem item in fullList)
            {
                string token = item.RecordingToken;
                int count = fullList.Count(RI => RI.RecordingToken == token);
                if (count != 1)
                {
                    tokensOk = false;
                    if (!logged.Contains(token))
                    {
                        logged.Add(token);
                        logger.AppendFormat("Token '{0}' is not unique{1}", token, Environment.NewLine);
                    }
                    //break; // ?
                }

                if (item.Configuration == null)
                    logger.AppendFormat("Configuration element is missing for item with token '{0}'{1}", token, Environment.NewLine);
            }

            assert(tokensOk, logger.ToStringTrimNewLine(),
                   "Validate recordings list got from GetRecordings", null);
        }
             

        public static bool CompareSourceInformation(RecordingSourceInformation source1,
                RecordingSourceInformation source2, string descr1, string descr2,
                string itemToken, StringBuilder logger)
        {
            if (source1 == null && source2 == null)
            {
                return true;
            }

            bool specifyToken = !string.IsNullOrEmpty(itemToken);

            string infoMissingFormatString = specifyToken ?
                string.Format("Source Information is missing for recording with token '{0}' (when got via {{0}}){1}", itemToken, Environment.NewLine) :
                string.Format("Source Information is missing (when got via {{0}}){0}", Environment.NewLine);

            if (source1 == null)
            {
                logger.AppendFormat(
                    infoMissingFormatString,
                    descr1, Environment.NewLine);
                return false;
            }
            if (source2 == null)
            {
                logger.AppendFormat(
                    infoMissingFormatString,
                    descr2, Environment.NewLine);
                return false;
            }

            bool ok = true;
            StringBuilder dump = specifyToken ?
                new StringBuilder(string.Format("Source information is different for Recording with token '{0}'{1}", itemToken, Environment.NewLine)) :
                new StringBuilder(string.Format("Source information is different {0}", Environment.NewLine));

            Action<string, Func<RecordingSourceInformation, string>> checkStringAction =
                new Action<string, Func<RecordingSourceInformation, string>>((name, fieldSelector) =>
                {
                    string value1 = fieldSelector(source1);
                    string value2 = fieldSelector(source2);
                    if (value1 != value2)
                    {
                        ok = false;
                        dump.AppendFormat("   {0} field is different ('{1}' when got via {2}, '{3}' when got via {4}){5}",
                            name, value1, descr1, value2, descr2, Environment.NewLine);
                    }

                });

            checkStringAction("Address", S => S.Address);
            checkStringAction("Description", S => S.Description);
            checkStringAction("Location", S => S.Location);
            checkStringAction("Name", S => S.Name);
            checkStringAction("SourceId", S => S.SourceId);

            if (!ok)
            {
                logger.Append(dump.ToString());
            }

            return ok;
        }

        public static bool CompareConfigurations(RecordingConfiguration configuration1,
            RecordingConfiguration configuration2, StringBuilder dump, string descr1, string descr2)
        {
            bool equal = true;
            bool local;

            if (configuration1.Content != configuration2.Content)
            {
                equal = false;
                dump.AppendFormat("Content field is different: '{0}' in {1}, '{2}' in {3}{4}",
                    configuration1.Content, descr1, configuration2.Content, descr2, Environment.NewLine);
            }

            // convert configuration1.MaximumRetentionTime
            var MaxRetTime1 = new TimeSpan();
            try
            {
                MaxRetTime1 = System.Xml.XmlConvert.ToTimeSpan(configuration1.MaximumRetentionTime);
            }
            catch (Exception e)
            {
                dump.AppendFormat("Invalid format for Maximum Retention Time: {0} {1}", e.Message, Environment.NewLine);
                equal = false;

                return equal;
            }

            // convert configuration2.MaximumRetentionTime
            var MaxRetTime2 = new TimeSpan();
            try
            {
                MaxRetTime2 = System.Xml.XmlConvert.ToTimeSpan(configuration2.MaximumRetentionTime);
            }
            catch (Exception e)
            {
                dump.AppendFormat("Invalid format for Maximum Retention Time: {0} {1}", e.Message, Environment.NewLine);
                equal = false;

                return equal;
            }

            // compare  them
            if (MaxRetTime1 != MaxRetTime2)
            {
                equal = false;
                dump.AppendFormat("MaximumRetentionTime field is different: '{0}' ({1}) in {2}, '{3}' ({4}) in {5}{6}",
                    configuration1.MaximumRetentionTime, MaxRetTime1.ToString(), descr1, 
                    configuration2.MaximumRetentionTime, MaxRetTime2.ToString(), descr2, 
                    Environment.NewLine);
            }

            RecordingSourceInformation source1 = configuration1.Source;
            RecordingSourceInformation source2 = configuration2.Source;

            local = StorageTestsUtils.CompareSourceInformation(source1, source2, descr1, descr2, null, dump);

            equal = equal && local;

            
            return equal;
        }

    }
}
