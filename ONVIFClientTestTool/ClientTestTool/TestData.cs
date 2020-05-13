using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace ClientTestTool
{
    /// <summary>
    /// Класс для xml-сериализации тестовых данных
    /// </summary>
    /// 
    public class TestSet
    {
        public string reportPrefix { get; set; }
        public List<Test> testList { get; set; }
        public void SerializeData(TestSet data, string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(TestSet));
            StreamWriter writer = new StreamWriter(path);
            s.Serialize(writer, data);
        }
        public TestSet DeSerializeData(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestSet));
            FileStream fs = new FileStream(path, FileMode.Open);
            TestSet data;
            data = (TestSet)serializer.Deserialize(fs);
            return data;
        }
    }

    public class Test
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> relatedItems { get; set; }
        public string description { get; set; }
        public List<string> pcapng { get; set; }        
        public List<SupportedEvent> eventList { get; set; }
        public SerializableDictionary<string, string> featureDevice { get; set; }
        public List<DeviceExpectedResult> expectedResults { get; set; }
        public SerializableDictionary<string, string> featuresExpectedResult { get; set; }
        public SerializableDictionary<string, string> profilesExpectedResult { get; set; }
    }

    public class DeviceExpectedResult
    {
        public string deviceMAC { get; set; }
        public List<ExpectedResult> expectedResults { get; set; }
    }

    public class ExpectedResult
    {
        public string parentNode0 { get; set; }
        public string parentNode1 { get; set; }
        public SerializableDictionary<string, string> expectedResult { get; set; }
    }

    /// <summary>
    /// Класс для xml-сериализации путей файлов с тестовыми данными
    /// </summary>
    public class TestSetList
    {
        public string reportPrefix { get; set; }
        public List<string> testSetFilePaths { get; set; }

        public void SerializeData(TestSetList data, string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(TestSetList));
            StreamWriter writer = new StreamWriter(path);
            s.Serialize(writer, data);
        }
        public TestSetList DeSerializeData(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestSetList));
            FileStream fs = new FileStream(path, FileMode.Open);
            TestSetList data;
            data = (TestSetList)serializer.Deserialize(fs);
            return data;
        }
    }
    public class LogData
    {
        public string testPath { get; set; }
        public string deviceMAC { get; set; }        
        public string expectedResult { get; set; }
        public string currentResult { get; set; }
        public string result { get; set; }
        public List<TestStep> testSteps { get; set; }
        public LogData()
        {
            this.testSteps = new List<TestStep>();
        }
    }

    public class TestStep
    {
        public string results { get; set; }   
    }

    public class LogTest
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> relatedItems { get; set; }
        public string description { get; set; }
        public string relatedBugs { get; set; }
        public List<string> pcapng { get; set; }
        public List<SupportedEvent> eventList { get; set; }      
        public SerializableDictionary<string, string> featureDevice { get; set; }
        public List<LogData> check { get; set; }
        public List<LogData> checkFeatures { get; set; }       
        public List<LogData> checkProfiles { get; set; }
        public LogTest()
        {
            this.id = "";
            this.name = "";
            this.relatedItems = new List<string>();
            this.description = "";
            this.pcapng = new List<string>();
            this.eventList = new List<SupportedEvent>();
            this.featureDevice = new SerializableDictionary<string, string>();
            this.check = new List<LogData>();
        }

        public LogTest(Test test)
        {
            this.id = test.id;
            this.name = test.name;
            this.relatedItems = test.relatedItems;
            this.description = test.description;
            this.pcapng = test.pcapng;
            this.eventList = test.eventList;
            this.featureDevice = test.featureDevice;
            this.check = new List<LogData>();
            this.checkFeatures = new List<LogData>();           
            this.checkProfiles = new List<LogData>();
        }

    }

    public class LogFile
    {
        public List<LogTest> test { get; set; }

        public LogFile()
        {
            this.test = new List<LogTest>();
        }

        public void SerializeData(LogFile data, string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            XmlSerializer s = new XmlSerializer(typeof(LogFile));
            using (StreamWriter writer = new StreamWriter(path))
            {
                s.Serialize(writer, data);
            }
        }
        public LogFile DeSerializeData(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LogFile));
            LogFile data;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                data = (LogFile)serializer.Deserialize(fs);
            }
            return data;
        }
    }

    public class SupportedEvent
    {
        public string parentNode0 { get; set; }
        public string parentNode1 { get; set; }
        public string parentNode2 { get; set; }
        public List<string> topicList { get; set; }        
    }

    /// <summary>
    /// Аналог стандартного Dictionary, с возможность xml-сериализации
    /// </summary>
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty) return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                var key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                var value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}