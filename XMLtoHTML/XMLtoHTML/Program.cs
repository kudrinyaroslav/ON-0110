using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl; 

namespace XMLtoHTML
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = @".";
            var outFolder = Path.Combine(folder, "Specifications");

            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            XslCompiledTransform myXslTransform = new XslCompiledTransform();

            using (XmlReader reader = XmlReader.Create(Path.Combine(folder, "docbook-css", "docbook.xsl"), new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
            {
                myXslTransform.Load(reader, XsltSettings.TrustedXslt, new XmlUrlResolver());
            }

            for (int i = 0; i < args.Count(); i += 2)
            {
                TransformFile(myXslTransform, Path.Combine(folder, args[i]), Path.Combine(outFolder, args[i + 1]));
            }

            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Core_Client_Test_Specification_v16.12.xml"), Path.Combine(outFolder, "Core.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Profile_S_Client_Test_Specification_v16.12.xml"), Path.Combine(outFolder, "ProfileS.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Advanced_Security_Client_Test_Specification_v16.12.xml"), Path.Combine(outFolder, "AdvancedSecurity.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Audio_Backchannel_Client_Test_Specification_v16.07.xml"), Path.Combine(outFolder, "AudioBackchannel.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Imaging_Client_Test_Specification_v16.07.xml"), Path.Combine(outFolder, "Imaging.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_OSD_Client_Test_Specification_v16.07.xml"), Path.Combine(outFolder, "OSD.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Profile_Q_Client_Test_Specification_v16.07.xml"), Path.Combine(outFolder, "ProfileQ.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Profile_A_Client_Test_Specification_v16.12.xml"), Path.Combine(outFolder, "ProfileA.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Profile_C_Client_Test_Specification_v16.12.xml"), Path.Combine(outFolder, "ProfileC.html"));
            //myXslTransform.Transform(Path.Combine(folder, "ONVIF_Profile_G_Client_Test_Specification_v16.12.xml"), Path.Combine(outFolder, "ProfileG.html"));
        }

        private static void TransformFile(XslCompiledTransform myXslTransform, string inFilePath, string outFilePath)
        {
            Console.WriteLine("Input file: {0}", new FileInfo(inFilePath).Name);

            Console.WriteLine("Output file: {0}", new FileInfo(outFilePath).Name);

            Console.Write("Processing... ");
            using (TextWriter writer = new StreamWriter(outFilePath, false, Encoding.UTF8))
            {
                using (XmlReader reader = XmlReader.Create(inFilePath, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
                {
                    myXslTransform.Transform(reader, new XsltArgumentList(), writer);
                }
            }
            Console.WriteLine("Ok");
        }
    }
}
