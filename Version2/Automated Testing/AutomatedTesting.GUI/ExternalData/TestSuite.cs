using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace AutomatedTesting.GUI.ExternalData
{
    [Serializable]
    public class TestSuite
    {
        [System.Xml.Serialization.XmlArray("Tests")]
        [System.Xml.Serialization.XmlArrayItem("Test")]
        public List<Test> Tests { get; set; }

        public TestParameters Parameters { get; set; }

        public static TestSuite Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestSuite));
                Stream stream = File.OpenRead(fileName);
                TestSuite ts = (TestSuite)(serializer.Deserialize(stream));
                stream.Close();
                return ts;
            }
            return null;
        }

        [NonSerialized]
        public const string ROOT = "TestSuite";

    }
}
