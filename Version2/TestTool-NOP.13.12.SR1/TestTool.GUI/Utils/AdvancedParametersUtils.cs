using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace TestTool.GUI.Utils
{
    class AdvancedParametersUtils
    {
        public static List<object> Deserialize(IEnumerable<XmlElement> elements, IEnumerable<Type> types)
        {
            List<object> advancedSettings = new List<object>();
            Dictionary<string, Type> namedTypes= new Dictionary<string, Type>();
            foreach (Type t in types)
            {
                namedTypes.Add(t.Name, t);
            }
            foreach (XmlElement element in elements)
            {
                string name = element.LocalName;

                if (namedTypes.ContainsKey(name))
                {
                    Type t = namedTypes[name];

                    XmlNodeReader rdr = new XmlNodeReader(element);

                    System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(t);
                    object parameters = serializer.Deserialize(rdr);
                    advancedSettings.Add(parameters);
                }
            }
            return advancedSettings;

        }
        
        public static List<XmlElement> Serialize(IEnumerable<object> settings)
        {
            MemoryStream memStream = new MemoryStream();
            XmlFragmentWriter xmlWriter = new XmlFragmentWriter(memStream, new UTF8Encoding(false));
            {
                int notNull = 0;
                foreach (object parameters in settings)
                {
                    if (parameters != null)
                    {
                        notNull++;
                        System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(parameters.GetType());
                        serializer.Serialize(xmlWriter, parameters);
                    }
                }
                if (notNull > 0)
                {
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.Close();

            string str = Encoding.UTF8.GetString(memStream.GetBuffer());

            XmlDocument doc = new XmlDocument();
            if (!string.IsNullOrEmpty(str))
            {
                doc.LoadXml(str);
            }

            // Serialize AdvancedSettings
            List<XmlElement> elements = new List<XmlElement>();
            if (doc.DocumentElement != null)
            {
                foreach (XmlElement element in doc.DocumentElement.ChildNodes)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

    }
}
