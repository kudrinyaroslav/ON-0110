using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CameraWebService
{
    public class Script
    {
        Dictionary<int, string> _responses = new Dictionary<int, string>();

        public void LoadScript(XmlDocument doc)
        {
            _responses.Clear();
            foreach (XmlElement element in doc.DocumentElement.ChildNodes)
            {
                if (element.LocalName == "Step")
                {
                    XmlAttribute attrStepId = element.Attributes["id"];
                    int stepId = int.Parse(attrStepId.Value);

                    _responses.Add(stepId, element.InnerXml);
                }
            }
        }

        public string GetResponse(int step)
        {
            if (_responses.ContainsKey(step))
            {
                return _responses[step];
            }

            return null;
        }

    }
}
