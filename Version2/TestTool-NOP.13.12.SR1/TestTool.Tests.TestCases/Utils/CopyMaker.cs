using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

// we need a true deep copy here, instead of serialization
// see for example https://raw.github.com/Burtsev-Alexey/net-object-deep-copy/master/ObjectExtensions.cs
// http://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-an-object-in-net-c-specifically
// http://www.c-sharpcorner.com/UploadFile/ff2f08/deep-copy-of-object-in-C-Sharp/


namespace TestTool.Tests.TestCases.Utils
{
    internal class CopyMaker
    {
        public static T CreateCopy<T>(T obj)
            where T: class
        {
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("tt", "http://www.onvif.org/ver10/schema");
            
            XmlSerializer ser = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);

            stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream); 
            string text = reader.ReadToEnd();
            int posBegin;
            int posEnd = 0;
            while ((posBegin = text.IndexOf("xsi:type")) >= 0)
            {
              posEnd = text.IndexOf("\"", posBegin);
              if (posEnd < 0)
              {
                break;
              }
              posEnd = text.IndexOf("\"", posEnd + 1);
              text = text.Remove(posBegin, posEnd - posBegin + 1);
            };
            if (posEnd > 0)
            {
              byte[] byteArray = Encoding.ASCII.GetBytes(text); 
              stream = new MemoryStream(byteArray);
            }

            stream.Seek(0, SeekOrigin.Begin);
            object copy = ser.Deserialize(stream);
            stream.Close();

            return copy as T;
        }

    }
}
