using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.ServiceSchedule10
{

    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/schedule/help")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ScheduleBinding", Namespace = "http://www.onvif.org/ver10/schedule/help")]
    public class ServiceScheduleHelp : System.Web.Services.WebService
    {

        public delegate void GetRequestParameters(ref XmlDocument requestParametersXml);

        [System.Web.Services.WebMethodAttribute()]        
        public XmlDocument GetServiceCapabilities()
        {
            string commandResult = "Token";

            return GetHelp("ServiceSchedule10", "GetServiceCapabilities", commandResult, GetServiceCapabilitiesRequestParameters);
        }


        void GetServiceCapabilitiesRequestParameters(ref XmlDocument requestParametersXml)
        {
            AddElement(ref requestParametersXml, requestParametersXml.DocumentElement, "token", "expected token from request");
            AddElement(ref requestParametersXml, requestParametersXml.DocumentElement, "Name", "expected Name from request");
            AddElement(ref requestParametersXml, requestParametersXml.DocumentElement, "Standard", "expected Standard from request");
            AddElement(ref requestParametersXml, requestParametersXml.DocumentElement, "Description", "expected Description from request");


            XmlElement SpecialDays = requestParametersXml.CreateElement(string.Empty, "SpecialDays", string.Empty);
            requestParametersXml.DocumentElement.AppendChild(SpecialDays);

            AddElement(ref requestParametersXml, SpecialDays, "GroupToken", "expected GroupToken from request");

            XmlElement TimeRange = requestParametersXml.CreateElement(string.Empty, "TimeRange", string.Empty);
            SpecialDays.AppendChild(TimeRange);

            AddElement(ref requestParametersXml, TimeRange, "From", "expected From from request");
            AddElement(ref requestParametersXml, TimeRange, "Until", "expected Until from request");
        }

        void AddElement(ref XmlDocument requestParametersXml, XmlElement parent, string elementName, string elementText)
        {
            XmlElement element = requestParametersXml.CreateElement(string.Empty, elementName, string.Empty);
            parent.AppendChild(element);
            XmlText elementInnerText = requestParametersXml.CreateTextNode(elementText);
            element.AppendChild(elementInnerText);
        }

        XmlDocument GetHelp(string serviceName, string commandName, object commandResult, GetRequestParameters getRequestParameters)
        {
            XmlDocument requestParametersXml = new XmlDocument();
            requestParametersXml.AppendChild(requestParametersXml.CreateElement(string.Empty, "RequestParameters", string.Empty));

            getRequestParameters(ref requestParametersXml);

            XmlDocument result = new XmlDocument();


            XmlElement elementHelp = result.CreateElement(string.Empty, "Help", string.Empty);
            result.AppendChild(elementHelp);

            XmlElement elementStep = result.CreateElement(string.Empty, "Step", string.Empty);
            elementHelp.AppendChild(elementStep);

            XmlAttribute attributeStepId = result.CreateAttribute(string.Empty, "id", string.Empty);
            elementStep.Attributes.Append(attributeStepId);

            XmlText attributeStepIdText = result.CreateTextNode("<id>");
            attributeStepId.AppendChild(attributeStepIdText);

            XmlElement elementCommand = result.CreateElement(string.Empty, "Command", string.Empty);
            XmlText elementCommandText = result.CreateTextNode(string.Format("{0}.{1}", serviceName, commandName));
            elementStep.AppendChild(elementCommand);
            elementCommand.AppendChild(elementCommandText);

            XmlElement elementResponse = result.CreateElement(string.Empty, "Response", string.Empty);
            XmlText elementResponseText = result.CreateTextNode(string.Format("Normal"));
            elementStep.AppendChild(elementResponse);
            elementResponse.AppendChild(elementResponseText);

            elementStep.AppendChild(result.ImportNode(requestParametersXml.DocumentElement, true));

            XmlElement elementResponseParameters = result.CreateElement(string.Empty, "ResponseParameters", string.Empty);
            elementStep.AppendChild(elementResponseParameters);

            XmlSerializer serializer = new XmlSerializer(commandResult.GetType());
            using (StringWriter sw = new StringWriter())
            {
                XmlDocument resultResponseParameters = new XmlDocument();
                serializer.Serialize(sw, commandResult);
                resultResponseParameters.LoadXml(sw.ToString());
                elementResponseParameters.AppendChild(result.ImportNode(resultResponseParameters.DocumentElement, true));
            }


            return result;
        }

    }
}
