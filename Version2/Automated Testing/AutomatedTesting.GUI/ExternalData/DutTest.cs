using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace AutomatedTesting.GUI.ExternalData
{
    [Serializable]
    [XmlRoot("TestSuit")]
    public class DutTest
    {
        [System.Xml.Serialization.XmlElement("Test")]
        public List<DutTestCase> Tests { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public string FileName { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public TestTool.Tests.Definitions.Enums.Category Category { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public string TestID { get; set; }
        
        public static DutTest Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DutTest));
                Stream stream = File.OpenRead(fileName);
                DutTest ts = (DutTest)(serializer.Deserialize(stream));
                stream.Close();
                
                ts.FileName = fileName;

                if (ts.Tests.Count > 0)
                {
                    DutTestCase tc = ts.Tests[0];
                    string testId = tc.ONVIFTestID;
                    int delimeter = testId.IndexOf('-');
                    string category = testId.Substring(0, delimeter);
                    ts.Category = (TestTool.Tests.Definitions.Enums.Category)Enum.Parse(typeof(TestTool.Tests.Definitions.Enums.Category), category);
                    ts.TestID = testId.Substring(delimeter + 1, testId.Length - delimeter - 1);
                }


                return ts;
            }
            return null;
        }

        [NonSerialized]
        public const string ROOT = "TestSuit";
    }

    [Serializable]
    [XmlRoot("Test")]
    public class DutTestCase
    {
        [System.Xml.Serialization.XmlAttribute("ID")]
        public string TestCaseID { get; set; }

        [System.Xml.Serialization.XmlAttribute("ONVIFTestID")]
        public string ONVIFTestID { get; set; }
               

        public string Name { get; set; }
    }

}
