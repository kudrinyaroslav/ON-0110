using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace AutomatedTesting.GUI.ExternalData
{
    public class FeatureDefinition
    {
        public string DefaultFileName { get; set; }
        public List<TestCase> TestCases { get; set; }

        public static FeatureDefinition Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FeatureDefinition));
                Stream stream = File.OpenRead(fileName);
                FeatureDefinition ts = (FeatureDefinition)(serializer.Deserialize(stream));
                stream.Close();
                return ts;
            }
            return null;
        }

        [NonSerialized]
        public const string ROOT = "FeatureDefinition";

    }
}
