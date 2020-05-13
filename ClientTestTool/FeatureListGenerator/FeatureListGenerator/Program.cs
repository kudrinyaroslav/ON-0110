using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FeatureListGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create("feature_list_template.xml", settings))
            {
                FeatureListGenerator fListGenerator = new FeatureListGenerator();
                fListGenerator.FeatureList(writer);
            }
        }
    }
}
