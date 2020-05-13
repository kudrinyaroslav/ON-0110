using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using DUT.WithLogic.Base;

namespace DUT.WithLogic.Services.Media2
{
    public class ONVIFMedia2Capabilities
    {
        Proxy.Capabilities2 m_Capabilities;


        public Proxy.Capabilities2 Capabilities
        {
            get { return m_Capabilities; }
            set { m_Capabilities = value; }
        }

        public static void Serialize()
        {
            ONVIFMedia2Capabilities temp = new ONVIFMedia2Capabilities();

            using (XmlWriter writer = XmlWriter.Create(@"D:\2.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFMedia2Capabilities));
                serializer.Serialize(writer, temp);
            }
        }

        public override string ToString()
        {
            StringWriter stringWriter;
            using (stringWriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFMedia2Capabilities));
                serializer.Serialize(stringWriter, this);
            }
            return stringWriter.ToString();
        }

        public static ONVIFMedia2Capabilities Load()
        {
            using (XmlReader reader = XmlReader.Create(Engine.ONVIFServiceList.FullUri(Base.AppPaths.PATH_MEDAI2CAPABILITIES)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFMedia2Capabilities));
                return (ONVIFMedia2Capabilities)serializer.Deserialize(reader);
            }

        }

    }
}