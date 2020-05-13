using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DeviceIo;
using System.Web.Services.Protocols;
using System.Xml;

namespace CameraWebService
{
    /// <summary>
    /// Summary description for IoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class IoService : DeviceIOBinding
    {


        public override DeviceIo.AudioSource[] GetAudioSources()
        {
            throw new NotImplementedException();
        }

        public override DeviceIo.AudioOutput[] GetAudioOutputs()
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetVideoSources", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("VideoSources")]
        public override DeviceIo.VideoSource[] GetVideoSources()
        {
            List<DeviceIo.VideoSource> lst = new List<DeviceIo.VideoSource>();

            for (int i = 1; i < 5; i++)
            {

                DeviceIo.VideoSource source = new DeviceIo.VideoSource();

                source.token = "source" + i;
                source.Resolution = new DeviceIo.VideoResolution();
                source.Resolution.Height = 480;
                source.Resolution.Width = 640;

                source.Imaging = new DeviceIo.ImagingSettings();
                source.Imaging.BacklightCompensation = new DeviceIo.BacklightCompensation();
                source.Imaging.BacklightCompensation.Mode = DeviceIo.BacklightCompensationMode.OFF;

                lst.Add(source);
            }
            return lst.ToArray();
        }

        public override DeviceIo.VideoOutput[] GetVideoOutputs()
        {
            throw new NotImplementedException();
        }

        public override DeviceIo.VideoSourceConfiguration GetVideoSourceConfiguration(string VideoSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override VideoOutputConfiguration GetVideoOutputConfiguration(string VideoOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override DeviceIo.AudioSourceConfiguration GetAudioSourceConfiguration(string AudioSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override AudioOutputConfiguration GetAudioOutputConfiguration(string AudioOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetVideoSourceConfiguration(DeviceIo.VideoSourceConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetVideoOutputConfiguration(VideoOutputConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioSourceConfiguration(DeviceIo.AudioSourceConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioOutputConfiguration(AudioOutputConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override DeviceIo.VideoSourceConfigurationOptions GetVideoSourceConfigurationOptions(string VideoSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override VideoOutputConfigurationOptions GetVideoOutputConfigurationOptions(string VideoOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override DeviceIo.AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string AudioSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string AudioOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetRelayOutputs", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RelayOutputs")]
        public override DeviceIo.RelayOutput[] GetRelayOutputs()
        {
            return IoStorage.Instance.RelayOutputs.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/SetRelayOutputSettings", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputSettings(DeviceIo.RelayOutput RelayOutput)
        {
            bool found = false;

            foreach (DeviceIo.RelayOutput output in IoStorage.Instance.RelayOutputs)
            {
                if (output.token == RelayOutput.token)
                {
                    output.Properties = RelayOutput.Properties;
                    output.Properties.Mode = DeviceIo.RelayMode.Monostable;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "RelayToken" });
            }

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/SetRelayOutputState", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputState(string RelayOutputToken, DeviceIo.RelayLogicalState LogicalState)
        {
            bool found = false;

            foreach (DeviceIo.RelayOutput output in IoStorage.Instance.RelayOutputs)
            {
                if (output.token == RelayOutputToken)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "RelayToken" });
            }

        }

        void ReturnFault(string[] codes)
        {
            SoapFaultSubCode subCode = null;
            for (int i = codes.Length - 1; i > 0; i--)
            {
                SoapFaultSubCode currentSubCode = new SoapFaultSubCode(new XmlQualifiedName(codes[i], "http://www.onvif.org/ver10/error"), subCode);
                subCode = currentSubCode;
            }
            throw new SoapException("Error", new XmlQualifiedName(codes[0], "http://www.w3.org/2003/05/soap-envelope"), subCode);
        }
    }
}
