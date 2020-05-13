using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Imaging;
using System.Xml;

namespace CameraWebService
{
    /// <summary>
    /// Summary description for ImagingService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ImagingService : Imaging.ImagingBinding
    {

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetImagingSettings", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ImagingSettings")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetImagingSettingsResponse xmlns=\"http://www.onvif.org/ver20/imaging/wsdl\">      <INCORRECT_TAG_ImagingSettings>        <BacklightCompensation xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>OFF</Mode>          <Level>1</Level>        </BacklightCompensation>        <Brightness xmlns=\"http://www.onvif.org/ver10/schema\">1</Brightness>        <ColorSaturation xmlns=\"http://www.onvif.org/ver10/schema\">1</ColorSaturation>        <Contrast xmlns=\"http://www.onvif.org/ver10/schema\">1</Contrast>        <Exposure xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>AUTO</Mode>          <Priority>LowNoise</Priority>          <Window bottom=\"1\" top=\"1\" right=\"1\" left=\"1\" />          <MinExposureTime>1</MinExposureTime>          <MaxExposureTime>1</MaxExposureTime>          <MinGain>1</MinGain>          <MaxGain>1</MaxGain>          <MinIris>1</MinIris>          <MaxIris>1</MaxIris>          <ExposureTime>1</ExposureTime>          <Gain>1</Gain>          <Iris>1</Iris>        </Exposure>        <Focus xmlns=\"http://www.onvif.org/ver10/schema\">          <AutoFocusMode>AUTO</AutoFocusMode>          <DefaultSpeed>1</DefaultSpeed>          <NearLimit>1</NearLimit>          <FarLimit>1</FarLimit>          <Extension />        </Focus>        <IrCutFilter xmlns=\"http://www.onvif.org/ver10/schema\">ON</IrCutFilter>        <Sharpness xmlns=\"http://www.onvif.org/ver10/schema\">1</Sharpness>        <WideDynamicRange xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>OFF</Mode>          <Level>1</Level>        </WideDynamicRange>        <WhiteBalance xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>AUTO</Mode>          <CrGain>1</CrGain>          <CbGain>1</CbGain>          <Extension />        </WhiteBalance>        <Extension xmlns=\"http://www.onvif.org/ver10/schema\" />      </INCORRECT_TAG_ImagingSettings>    </GetImagingSettingsResponse>  </soap:Body></soap:Envelope>")]
        public override Imaging.ImagingSettings20 GetImagingSettings(string VideoSourceToken)
        {
            CheckActionSupported();
            CheckSourceExists(VideoSourceToken);

            Imaging.ImagingSettings20 settings = new Imaging.ImagingSettings20();
            //settings.BacklightCompensation = new BacklightCompensation20();
            //settings.BacklightCompensation.Mode = Imaging.BacklightCompensationMode.OFF;
            
            settings.Brightness = 0;
            settings.ColorSaturation = 0;
            settings.Contrast = 255;

            return settings;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/SetImagingSettings", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetImagingSettings(string VideoSourceToken, Imaging.ImagingSettings20 ImagingSettings, bool ForcePersistence, bool ForcePersistenceSpecified)
        {
            CheckActionSupported();
            CheckSourceExists(VideoSourceToken);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetOptions", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ImagingOptions")]
        public override ImagingOptions20 GetOptions(string VideoSourceToken)
        {
            CheckActionSupported();
            CheckSourceExists(VideoSourceToken);

            ImagingOptions20 options = new ImagingOptions20();

            //options.BacklightCompensation = new BacklightCompensationOptions20();
            //options.BacklightCompensation.Mode = new Imaging.BacklightCompensationMode[]{Imaging.BacklightCompensationMode.OFF, Imaging.BacklightCompensationMode.ON};
            //options.BacklightCompensation.Level = new Imaging.FloatRange() { Min = 2, Max = 5 };

            options.Brightness = new Imaging.FloatRange() {Min = 0, Max = 255};
            options.ColorSaturation = new Imaging.FloatRange() { Min = 0, Max = 255 };
            options.Contrast = new Imaging.FloatRange() { Min = 0, Max =255 };
            
            //options.IrCutFilterModes = new Imaging.IrCutFilterMode[]{Imaging.IrCutFilterMode.ON};
            
            //options.Exposure = new ExposureOptions20();
            //options.Exposure.Mode = new Imaging.ExposureMode[]{Imaging.ExposureMode.AUTO, Imaging.ExposureMode.MANUAL };
            //options.Exposure.ExposureTime = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.Gain = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.Iris = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.MaxExposureTime = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.MaxGain = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.MaxIris = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.MinExposureTime = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.MinGain = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.MinIris = new Imaging.FloatRange() { Min = 2, Max = 5 };
            //options.Exposure.Priority = new Imaging.ExposurePriority[]{Imaging.ExposurePriority.FrameRate};
            
            //options.Focus = new FocusOptions20();
            //options.Focus.AutoFocusModes = new Imaging.AutoFocusMode[] {Imaging.AutoFocusMode.AUTO, Imaging.AutoFocusMode.MANUAL};

            //options.Focus.DefaultSpeed = new Imaging.FloatRange();
            //options.Focus.FarLimit = new Imaging.FloatRange();
            //options.Focus.NearLimit = new Imaging.FloatRange();
            
            //options.Sharpness = new Imaging.FloatRange();
            //options.WideDynamicRange = new WideDynamicRangeOptions20();
            //options.WideDynamicRange.Mode = new Imaging.WideDynamicMode[]{Imaging.WideDynamicMode.OFF};

            //options.WhiteBalance = new WhiteBalanceOptions20();
            //options.WhiteBalance.Mode = new Imaging.WhiteBalanceMode[]{Imaging.WhiteBalanceMode.AUTO};
 
            return options;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/Move", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Move(string VideoSourceToken, Imaging.FocusMove Focus)
        {
            CheckActionSupported();
            CheckSourceExists(VideoSourceToken);

            MoveOptions20 options = Storage.Instance.MoveOptions[VideoSourceToken];

            if ((Focus.Absolute != null && Focus.Continuous != null) || 
                (Focus.Absolute != null && Focus.Relative != null) || 
                (Focus.Continuous!= null && Focus.Relative != null))
            {
                ThrowInvalidArgVal();
            }
            
            if (Focus.Absolute != null)
            {
                if (Focus.Absolute.Position.NotIn(options.Absolute.Position))
                {
                    ThrowInvalidArgVal();
                }

                if (Focus.Absolute.SpeedSpecified && Focus.Absolute.Speed.NotIn(options.Absolute.Speed))
                {
                    ThrowInvalidArgVal();

                }
            }

            if (Focus.Continuous != null)
            {
                if (Focus.Continuous.Speed.NotIn(options.Continuous.Speed))
                {
                    ThrowInvalidArgVal();
                }
            }

            if (Focus.Relative != null)
            {
                if (Focus.Relative.Distance.NotIn(options.Relative.Distance))
                {
                    ThrowInvalidArgVal();
                }
                if (Focus.Relative.SpeedSpecified && Focus.Relative.Speed.NotIn(options.Relative.Speed))
                {
                    ThrowInvalidArgVal();

                }

            }
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetMoveOptions", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MoveOptions")]
        public override MoveOptions20 GetMoveOptions(string VideoSourceToken)
        {
            CheckActionSupported();
            CheckSourceExists(VideoSourceToken);

            return Storage.Instance.MoveOptions[VideoSourceToken];

        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public ImagingServiceCapabilities GetServiceCapabilities()
        {
            ImagingServiceCapabilities capabilities = new ImagingServiceCapabilities();
            capabilities.ImageStabilizationSpecified = true;
            capabilities.ImageStabilization = true;
            return capabilities;
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/FocusStop", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Stop(string VideoSourceToken)
        {
            CheckActionSupported();
            CheckSourceExists(VideoSourceToken);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetStatus", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Status")]
        public override ImagingStatus20 GetStatus(string VideoSourceToken)
        {
            CheckActionSupported();
            CheckSourceExists(VideoSourceToken);
            return new ImagingStatus20();
        }

        void CheckSourceExists(string token)
        {
            Media.VideoSource[] sources = Storage.Instance.Sources;
            if (sources.Where(s => s.token == token).FirstOrDefault() == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "NoSource" });
            }
        }
        
        void CheckActionSupported()
        {
            if (false)
            {
                ReturnFault(new string[]{"Receiver", "ActionNotSupported","NoImagingForSource"});
            }
        }

        void ThrowInvalidArgVal()
        {
            ReturnFault(new string[] { "Sender", "ActionNotSupported", "InvalidArgValVal" });
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
