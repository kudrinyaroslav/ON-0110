using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Media10
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MediaService : Media10ServiceBinding
    {

        //TestSuit
        MediaServiceTest MediaServiceTest
        {
            get
            {
                if (Application[Base.AppVars.MEDIASERVICE] != null)
                {
                    return (MediaServiceTest)Application[Base.AppVars.MEDIASERVICE];
                }
                else
                {
                    MediaServiceTest serviceTest = new MediaServiceTest(TestCommon);
                    Application[Base.AppVars.MEDIASERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetVideoSources/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("VideoSources")]
        #region XmlReplySubstituteExtension for testing
        //TC.IMAGING-1-1-1.10
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetVideoSourcesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\"></GetVideoSourcesResponse>  </soap:Body></soap:Envelope>")]
        #endregion XmlReplySubstituteExtension for testing
        public override VideoSource[] GetVideoSources()
        {
            
            VideoSource[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetVideoSourcesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioSources", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AudioSources")]
        public override AudioSource[] GetAudioSources()
        {
            
            AudioSource[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioSourcesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/CreateProfile", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "Media(RTSS-3-1-1)TestSuit.xml", Test Case: "TC.RTSS-3-1-1.01"
        //a. Test Group Name: "RTSS-3-1-1 NOTIFICATION STREAMING"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <CreateProfileResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profile token=\"Test\" fixed=\"false\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">Test</Name>      </Profile>    </CreateProfileResponse>  </soap:Body></soap:Envelope>")]
        //a.1. TC.RTSS-3-1-1.02: "Delete namespace (in main response CreateProfile)" (delete namespace from tag ProfileToken\<Name>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <CreateProfileResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profile token=\"Test\" fixed=\"false\">        <Name>Test</Name>      </Profile>    </CreateProfileResponse>  </soap:Body></soap:Envelope>")]        
        //a.2. TC.RTSS-3-1-1.03: "Delete child tag (in main response CreateProfile)" (delete tag ProfileToken\<Name>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <CreateProfileResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profile token=\"Test\" fixed=\"false\">        </Profile>    </CreateProfileResponse>  </soap:Body></soap:Envelope>")]        
        //a.3. TC.RTSS-3-1-1.05: "Add '\n' after tags (in main response CreateProfile)" (add '\n' after some (or all) tags)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n  <soap:Body>\n    <CreateProfileResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">\n      <Profile token=\"Test\" fixed=\"false\">\n        <Name xmlns=\"http://www.onvif.org/ver10/schema\">\nTest</Name>\n      </Profile>\n    </CreateProfileResponse>\n  </soap:Body>\n</soap:Envelope>\n")]

        #endregion XmlReplySubstituteExtension for testing
        [return: System.Xml.Serialization.XmlElementAttribute("Profile")]
        public override Profile CreateProfile(string Name, string Token)
        {
            //
            //Profile res;
            //int timeOut;
            //SoapException ex;

            //StepType stepType = MediaServiceTest.CreateProfileTest(out res, out ex, out timeOut, Name, Token);
            //StepTypeProcessing(stepType, ex, timeOut);

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "Name", Name);
            validation.Add(ParameterType.OptionalString, "Token", Token);
            Profile result = (Profile)ExecuteGetCommand(validation, MediaServiceTest.CreateProfileTest);
            return result;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetProfile/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Profile")]
        public override Profile GetProfile(string ProfileToken)
        {
            
            Profile res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetProfileTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetProfiles", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "Media(RTSS-1-1-1)TestSuit.xml", Test Case: "TC.RTSS-1-1-1.01"
        //a. Test Group Name: "RTSS-1-1-1 MEDIA CONTROL - RTSP/TCP"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetProfilesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profiles token=\"quality_h264\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">media_profile1</Name>        <VideoSourceConfiguration token=\"quality_h264\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>3</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"720\" height=\"576\" />        </VideoSourceConfiguration>        <VideoEncoderConfiguration token=\"quality_h264\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>MPEG4</Encoding>          <Resolution>            <Width>720</Width>            <Height>576</Height>          </Resolution>          <Quality>6</Quality>          <RateControl>            <FrameRateLimit>25</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>1024</BitrateLimit>          </RateControl>          <MPEG4>            <GovLength>1</GovLength>            <Mpeg4Profile>SP</Mpeg4Profile>          </MPEG4>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>      </Profiles>    </GetProfilesResponse>  </soap:Body></soap:Envelope>")]
        //a.1. TC.RTSS-1-1-1.02: "Delete namespace (in main response GetProfiles)" (delete namespace from tag Profiles\<VideoEncoderConfiguration>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetProfilesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profiles token=\"quality_h264\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">media_profile1</Name>        <VideoSourceConfiguration token=\"quality_h264\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>3</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"720\" height=\"576\" />        </VideoSourceConfiguration>        <VideoEncoderConfiguration token=\"quality_h264\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>MPEG4</Encoding>          <Resolution>            <Width>720</Width>            <Height>576</Height>          </Resolution>          <Quality>6</Quality>          <RateControl>            <FrameRateLimit>25</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>1024</BitrateLimit>          </RateControl>          <MPEG4>            <GovLength>1</GovLength>            <Mpeg4Profile>SP</Mpeg4Profile>          </MPEG4>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>      </Profiles>    </GetProfilesResponse>  </soap:Body></soap:Envelope>")]
        //a.2. TC.RTSS-1-1-1.04: "Change tags order (in main response GetProfiles)" (set Profiles\<VideoSourceConfiguration> tag in front of Profiles\<Name> tag)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetProfilesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profiles token=\"quality_h264\">                <VideoSourceConfiguration token=\"quality_h264\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>3</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"720\" height=\"576\" />        </VideoSourceConfiguration>    <Name xmlns=\"http://www.onvif.org/ver10/schema\">media_profile1</Name>    <VideoEncoderConfiguration token=\"quality_h264\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>MPEG4</Encoding>          <Resolution>            <Width>720</Width>            <Height>576</Height>          </Resolution>          <Quality>6</Quality>          <RateControl>            <FrameRateLimit>25</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>1024</BitrateLimit>          </RateControl>          <MPEG4>            <GovLength>1</GovLength>            <Mpeg4Profile>SP</Mpeg4Profile>          </MPEG4>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>      </Profiles>    </GetProfilesResponse>  </soap:Body></soap:Envelope>")]
        //a.3. TC.RTSS-1-1-1.06: "Change tag name (in main response GetProfiles)" (change name of tag <Profiles> to <Profile>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetProfilesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profile token=\"quality_h264\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">media_profile1</Name>        <VideoSourceConfiguration token=\"quality_h264\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>3</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"720\" height=\"576\" />        </VideoSourceConfiguration>        <VideoEncoderConfiguration token=\"quality_h264\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>MPEG4</Encoding>          <Resolution>            <Width>720</Width>            <Height>576</Height>          </Resolution>          <Quality>6</Quality>          <RateControl>            <FrameRateLimit>25</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>1024</BitrateLimit>          </RateControl>          <MPEG4>            <GovLength>1</GovLength>            <Mpeg4Profile>SP</Mpeg4Profile>          </MPEG4>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>      </Profile>    </GetProfilesResponse>  </soap:Body></soap:Envelope>")]

        //b. Test Suite: "PTZ(PTZ-3-1-4)TestSuit.xml", Test Case: "TC.PTZ-3-1-4.01"
        //b. Test Group Name: "PTZ-3-1-4 PTZ CONTINUOUS MOVE"
        //b.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetProfilesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profiles token=\"media_profile2\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">media_profile1</Name>        <VideoSourceConfiguration token=\"video_source_config\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>1</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"1920\" height=\"1080\" />        </VideoSourceConfiguration>        <AudioSourceConfiguration token=\"audio_source_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_source_config1</Name>          <UseCount>1</UseCount>          <SourceToken>audio_source</SourceToken>        </AudioSourceConfiguration>        <VideoEncoderConfiguration token=\"video_encoder_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>H264</Encoding>          <Resolution>            <Width>320</Width>            <Height>192</Height>          </Resolution>          <Quality>7</Quality>          <RateControl>            <FrameRateLimit>1</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>2048</BitrateLimit>          </RateControl>          <H264>            <GovLength>32</GovLength>            <H264Profile>Baseline</H264Profile>          </H264>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>        <AudioEncoderConfiguration token=\"audio_encoder1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_encoder</Name>          <UseCount>1</UseCount>          <Encoding>G711</Encoding>          <Bitrate>64</Bitrate>          <SampleRate>8000</SampleRate>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </AudioEncoderConfiguration>        <PTZConfiguration token=\"Wrong-Token\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>ptz6Name</Name>          <UseCount>0</UseCount>          <NodeToken>ptzWrong</NodeToken>          <DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</DefaultAbsolutePantTiltPositionSpace>          <DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</DefaultAbsoluteZoomPositionSpace>          <DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</DefaultRelativePanTiltTranslationSpace>          <DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</DefaultRelativeZoomTranslationSpace>          <DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</DefaultContinuousPanTiltVelocitySpace>          <DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</DefaultContinuousZoomVelocitySpace>          <PanTiltLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</URI>              <XRange>                <Min>-1</Min>                <Max>1</Max>              </XRange>              <YRange>                <Min>-1</Min>                <Max>1</Max>              </YRange>            </Range>          </PanTiltLimits>          <ZoomLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</URI>              <XRange>                <Min>0</Min>                <Max>1</Max>              </XRange>            </Range>          </ZoomLimits>        </PTZConfiguration>      </Profiles>      <Profiles token=\"media_profile1\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">media_profile1</Name>        <VideoSourceConfiguration token=\"video_source_config\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>1</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"1920\" height=\"1080\" />        </VideoSourceConfiguration>        <AudioSourceConfiguration token=\"audio_source_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_source_config1</Name>          <UseCount>1</UseCount>          <SourceToken>audio_source</SourceToken>        </AudioSourceConfiguration>        <VideoEncoderConfiguration token=\"video_encoder_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>H264</Encoding>          <Resolution>            <Width>320</Width>            <Height>192</Height>          </Resolution>          <Quality>7</Quality>          <RateControl>            <FrameRateLimit>1</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>2048</BitrateLimit>          </RateControl>          <H264>            <GovLength>32</GovLength>            <H264Profile>Baseline</H264Profile>          </H264>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>        <AudioEncoderConfiguration token=\"audio_encoder1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_encoder</Name>          <UseCount>1</UseCount>          <Encoding>G711</Encoding>          <Bitrate>64</Bitrate>          <SampleRate>8000</SampleRate>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </AudioEncoderConfiguration>        <PTZConfiguration token=\"PTZ-Token\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>ptz0Name</Name>          <UseCount>0</UseCount>          <NodeToken>ptz0</NodeToken>          <DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</DefaultAbsolutePantTiltPositionSpace>          <DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</DefaultAbsoluteZoomPositionSpace>          <DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</DefaultRelativePanTiltTranslationSpace>          <DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</DefaultRelativeZoomTranslationSpace>          <DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</DefaultContinuousPanTiltVelocitySpace>          <DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</DefaultContinuousZoomVelocitySpace>          <PanTiltLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</URI>              <XRange>                <Min>-1</Min>                <Max>1</Max>              </XRange>              <YRange>                <Min>-1</Min>                <Max>1</Max>              </YRange>            </Range>          </PanTiltLimits>          <ZoomLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</URI>              <XRange>                <Min>0</Min>                <Max>1</Max>              </XRange>            </Range>          </ZoomLimits>        </PTZConfiguration>      </Profiles>    </GetProfilesResponse>  </soap:Body></soap:Envelope>")]
        //b.1. TC.MC.8-1-11.12, TC.PTZ-3-1-4.07: "Delete child tag (in secondary response GetProfiles)" (delete tag Profiles\<Name>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetProfilesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Profiles token=\"media_profile2\">         <VideoSourceConfiguration token=\"video_source_config\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>1</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"1920\" height=\"1080\" />        </VideoSourceConfiguration>        <AudioSourceConfiguration token=\"audio_source_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_source_config1</Name>          <UseCount>1</UseCount>          <SourceToken>audio_source</SourceToken>        </AudioSourceConfiguration>        <VideoEncoderConfiguration token=\"video_encoder_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>H264</Encoding>          <Resolution>            <Width>320</Width>            <Height>192</Height>          </Resolution>          <Quality>7</Quality>          <RateControl>            <FrameRateLimit>1</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>2048</BitrateLimit>          </RateControl>          <H264>            <GovLength>32</GovLength>            <H264Profile>Baseline</H264Profile>          </H264>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>        <AudioEncoderConfiguration token=\"audio_encoder1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_encoder</Name>          <UseCount>1</UseCount>          <Encoding>G711</Encoding>          <Bitrate>64</Bitrate>          <SampleRate>8000</SampleRate>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </AudioEncoderConfiguration>        <PTZConfiguration token=\"Wrong-Token\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>ptz6Name</Name>          <UseCount>0</UseCount>          <NodeToken>ptzWrong</NodeToken>          <DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</DefaultAbsolutePantTiltPositionSpace>          <DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</DefaultAbsoluteZoomPositionSpace>          <DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</DefaultRelativePanTiltTranslationSpace>          <DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</DefaultRelativeZoomTranslationSpace>          <DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</DefaultContinuousPanTiltVelocitySpace>          <DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</DefaultContinuousZoomVelocitySpace>          <PanTiltLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</URI>              <XRange>                <Min>-1</Min>                <Max>1</Max>              </XRange>              <YRange>                <Min>-1</Min>                <Max>1</Max>              </YRange>            </Range>          </PanTiltLimits>          <ZoomLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</URI>              <XRange>                <Min>0</Min>                <Max>1</Max>              </XRange>            </Range>          </ZoomLimits>        </PTZConfiguration>      </Profiles>      <Profiles token=\"media_profile1\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">media_profile1</Name>        <VideoSourceConfiguration token=\"video_source_config\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_source_config</Name>          <UseCount>1</UseCount>          <SourceToken>video_source</SourceToken>          <Bounds x=\"1\" y=\"1\" width=\"1920\" height=\"1080\" />        </VideoSourceConfiguration>        <AudioSourceConfiguration token=\"audio_source_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_source_config1</Name>          <UseCount>1</UseCount>          <SourceToken>audio_source</SourceToken>        </AudioSourceConfiguration>        <VideoEncoderConfiguration token=\"video_encoder_config1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>video_encoder_config1</Name>          <UseCount>1</UseCount>          <Encoding>H264</Encoding>          <Resolution>            <Width>320</Width>            <Height>192</Height>          </Resolution>          <Quality>7</Quality>          <RateControl>            <FrameRateLimit>1</FrameRateLimit>            <EncodingInterval>0</EncodingInterval>            <BitrateLimit>2048</BitrateLimit>          </RateControl>          <H264>            <GovLength>32</GovLength>            <H264Profile>Baseline</H264Profile>          </H264>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </VideoEncoderConfiguration>        <AudioEncoderConfiguration token=\"audio_encoder1\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>audio_encoder</Name>          <UseCount>1</UseCount>          <Encoding>G711</Encoding>          <Bitrate>64</Bitrate>          <SampleRate>8000</SampleRate>          <Multicast>            <Address>              <Type>IPv4</Type>              <IPv4Address>0.0.0.0</IPv4Address>            </Address>            <Port>0</Port>            <TTL>3</TTL>            <AutoStart>false</AutoStart>          </Multicast>          <SessionTimeout>PT0S</SessionTimeout>        </AudioEncoderConfiguration>        <PTZConfiguration token=\"PTZ-Token\" xmlns=\"http://www.onvif.org/ver10/schema\">          <Name>ptz0Name</Name>          <UseCount>0</UseCount>          <NodeToken>ptz0</NodeToken>          <DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</DefaultAbsolutePantTiltPositionSpace>          <DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</DefaultAbsoluteZoomPositionSpace>          <DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</DefaultRelativePanTiltTranslationSpace>          <DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</DefaultRelativeZoomTranslationSpace>          <DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</DefaultContinuousPanTiltVelocitySpace>          <DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</DefaultContinuousZoomVelocitySpace>          <PanTiltLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</URI>              <XRange>                <Min>-1</Min>                <Max>1</Max>              </XRange>              <YRange>                <Min>-1</Min>                <Max>1</Max>              </YRange>            </Range>          </PanTiltLimits>          <ZoomLimits>            <Range>              <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</URI>              <XRange>                <Min>0</Min>                <Max>1</Max>              </XRange>            </Range>          </ZoomLimits>        </PTZConfiguration>      </Profiles>    </GetProfilesResponse>  </soap:Body></soap:Envelope>")]         
        #endregion //XmlReplySubstituteExtension for testing        
        [return: System.Xml.Serialization.XmlElementAttribute("Profiles")]
       //GetProfiles response for #489
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><s:Envelope xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:a=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:se=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\"><s:Header/><s:Body><trt:GetProfilesResponse><trt:Profiles token=\"fixed_profile_0\" fixed=\"true\"><tt:Name>fixed_profile_0</tt:Name><tt:VideoSourceConfiguration xsi:type=\"tt:VideoSourceConfiguration\" token=\"video_source_config_0\"><tt:Name>video_source_config_0</tt:Name><tt:UseCount>2</tt:UseCount><tt:SourceToken>video_source_0</tt:SourceToken><tt:Bounds height=\"1080\" width=\"1920\" y=\"0\" x=\"0\"></tt:Bounds></tt:VideoSourceConfiguration><tt:VideoEncoderConfiguration xsi:type=\"tt:VideoEncoderConfiguration\" token=\"video_encoder_config_0\"><tt:Name>video_encoder_config_0</tt:Name><tt:UseCount>1</tt:UseCount><tt:Encoding>JPEG</tt:Encoding><tt:Resolution><tt:Width>1920</tt:Width><tt:Height>1080</tt:Height></tt:Resolution><tt:Quality>50</tt:Quality><tt:RateControl><tt:FrameRateLimit>30</tt:FrameRateLimit><tt:EncodingInterval>1</tt:EncodingInterval><tt:BitrateLimit>0</tt:BitrateLimit></tt:RateControl><tt:Multicast><tt:Address><tt:Type>IPv4</tt:Type><tt:IPv4Address>232.0.0.2</tt:IPv4Address></tt:Address><tt:Port>1234</tt:Port><tt:TTL>5</tt:TTL><tt:AutoStart>false</tt:AutoStart></tt:Multicast><tt:SessionTimeout>PT60S</tt:SessionTimeout></tt:VideoEncoderConfiguration></trt:Profiles><trt:Profiles token=\"profile_1\" fixed=\"false\"><tt:Name>profile 1</tt:Name><tt:VideoSourceConfiguration xsi:type=\"tt:VideoSourceConfiguration\" token=\"video_source_config_0\"><tt:Name>video_source_config_0</tt:Name><tt:UseCount>2</tt:UseCount><tt:SourceToken>video_source_0</tt:SourceToken><tt:Bounds height=\"1080\" width=\"1920\" y=\"0\" x=\"0\"></tt:Bounds></tt:VideoSourceConfiguration><tt:VideoEncoderConfiguration xsi:type=\"tt:VideoEncoderConfiguration\" token=\"video_encoder_config_1\"><tt:Name>video_encoder_config_1</tt:Name><tt:UseCount>1</tt:UseCount><tt:Encoding>H264</tt:Encoding><tt:Resolution><tt:Width>1920</tt:Width><tt:Height>1080</tt:Height></tt:Resolution><tt:Quality>50</tt:Quality><tt:RateControl><tt:FrameRateLimit>30</tt:FrameRateLimit><tt:EncodingInterval>1</tt:EncodingInterval><tt:BitrateLimit>0</tt:BitrateLimit></tt:RateControl><tt:H264><tt:GovLength>30</tt:GovLength><tt:H264Profile>Baseline</tt:H264Profile></tt:H264><tt:Multicast><tt:Address><tt:Type>IPv4</tt:Type><tt:IPv4Address>232.0.0.2</tt:IPv4Address></tt:Address><tt:Port>1234</tt:Port><tt:TTL>5</tt:TTL><tt:AutoStart>false</tt:AutoStart></tt:Multicast><tt:SessionTimeout>PT60S</tt:SessionTimeout></tt:VideoEncoderConfiguration></trt:Profiles></trt:GetProfilesResponse></s:Body></s:Envelope>")]
        public override Profile[] GetProfiles()
        {
            
            Profile[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetProfilesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddVideoEncoderConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddVideoEncoderConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveVideoEncoderConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveVideoEncoderConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddVideoSourceConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddVideoSourceConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveVideoSourceConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveVideoSourceConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddAudioEncoderConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddAudioEncoderConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveAudioEncoderConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveAudioEncoderConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddAudioSourceConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddAudioSourceConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveAudioSourceConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveAudioSourceConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddPTZConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddPTZConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddPTZConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemovePTZConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemovePTZConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemovePTZConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddVideoAnalyticsConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddVideoAnalyticsConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddVideoAnalyticsConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveVideoAnalyticsConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveVideoAnalyticsConfiguration(string ProfileToken)
        {
            //throw new NotImplementedException();
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveVideoAnalyticsConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddMetadataConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddMetadataConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddMetadataConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveMetadataConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveMetadataConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveMetadataConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/DeleteProfile", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteProfile(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.DeleteProfileTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override VideoSourceConfiguration[] GetVideoSourceConfigurations()
        {
            
            VideoSourceConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetVideoSourceConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "Media(MEDIA-8-1-21)TestSuit.xml", Test Case: "TC.MC-8-1-21.10"
        //a.1. TC.MC-8-1-21.10: "Delete namespace (delete namespace for RateControl)"
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetVideoEncoderConfigurationsResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Configurations token=\"VEC1\">    <Name xmlns=\"http://www.onvif.org/ver10/schema\">VideoEncConf1</Name>        <UseCount xmlns=\"http://www.onvif.org/ver10/schema\">1</UseCount>        <Encoding xmlns=\"http://www.onvif.org/ver10/schema\">H264</Encoding>        <Resolution xmlns=\"http://www.onvif.org/ver10/schema\"><Width>320</Width>          <Height>192</Height>        </Resolution>        <Quality xmlns=\"http://www.onvif.org/ver10/schema\">7</Quality>        <RateControl>          <FrameRateLimit>1</FrameRateLimit>          <EncodingInterval>0</EncodingInterval>          <BitrateLimit>2048</BitrateLimit>        </RateControl>        <H264 xmlns=\"http://www.onvif.org/ver10/schema\">          <GovLength>32</GovLength>        <H264Profile>Baseline</H264Profile>        </H264>        <Multicast xmlns=\"http://www.onvif.org/ver10/schema\">          <Address>            <Type>IPv4</Type>            <IPv4Address>0.0.0.0</IPv4Address>          </Address>          <Port>0</Port>          <TTL>3</TTL>   <AutoStart>false</AutoStart>        </Multicast>        <SessionTimeout xmlns=\"http://www.onvif.org/ver10/schema\">PT0S</SessionTimeout>      </Configurations>      <Configurations token=\"VEC2\"><Name xmlns=\"http://www.onvif.org/ver10/schema\">VideoEncConf2</Name><UseCount xmlns=\"http://www.onvif.org/ver10/schema\">1</UseCount><Encoding xmlns=\"http://www.onvif.org/ver10/schema\">MPEG4</Encoding> <Resolution xmlns=\"http://www.onvif.org/ver10/schema\">          <Width>320</Width>          <Height>192</Height>        </Resolution>        <Quality xmlns=\"http://www.onvif.org/ver10/schema\">7</Quality>        <RateControl xmlns=\"http://www.onvif.org/ver10/schema\">          <FrameRateLimit>1</FrameRateLimit>          <EncodingInterval>0</EncodingInterval>          <BitrateLimit>2048</BitrateLimit>        </RateControl>        <Multicast xmlns=\"http://www.onvif.org/ver10/schema\">          <Address>            <Type>IPv6</Type><IPv6Address>0.0.0.0</IPv6Address>          </Address>          <Port>0</Port>          <TTL>3</TTL>         <AutoStart>false</AutoStart>        </Multicast>        <SessionTimeout xmlns=\"http://www.onvif.org/ver10/schema\">PT0S</SessionTimeout>      </Configurations></GetVideoEncoderConfigurationsResponse>  </soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing        
        public override VideoEncoderConfiguration[] GetVideoEncoderConfigurations()
        {
            
            VideoEncoderConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetVideoEncoderConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetAudioSourceConfigurations/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        
        public override AudioSourceConfiguration[] GetAudioSourceConfigurations()
        {
            
            AudioSourceConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioSourceConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        #region XmlReplySubstituteExtension for testing
        //TC.MC.8-1-11.12 "Delete name space"(delete namespace for UseCount)
       // [XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetAudioEncoderConfigurationsResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Configurations token=\"AEC1\">     <Name xmlns=\"http://www.onvif.org/ver10/schema\">AudioEncConf1</Name>        <UseCount>1</UseCount>        <Encoding xmlns=\"http://www.onvif.org/ver10/schema\">G711</Encoding>        <Bitrate xmlns=\"http://www.onvif.org/ver10/schema\">64</Bitrate>        <SampleRate xmlns=\"http://www.onvif.org/ver10/schema\">8000</SampleRate>        <Multicast xmlns=\"http://www.onvif.org/ver10/schema\">  <Address>            <Type>IPv4</Type>            <IPv4Address>0.0.0.0</IPv4Address>          </Address>          <Port>0</Port><TTL>3</TTL>          <AutoStart>false</AutoStart>        </Multicast>        <SessionTimeout xmlns=\"http://www.onvif.org/ver10/schema\">PT0S</SessionTimeout>      </Configurations> </GetAudioEncoderConfigurationsResponse>  </soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing    
        public override AudioEncoderConfiguration[] GetAudioEncoderConfigurations()
        {
            
            AudioEncoderConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioEncoderConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override VideoAnalyticsConfiguration[] GetVideoAnalyticsConfigurations()
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetMetadataConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override MetadataConfiguration[] GetMetadataConfigurations()
        {
            
            MetadataConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetMetadataConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override VideoSourceConfiguration GetVideoSourceConfiguration(string ConfigurationToken)
        {
            
            VideoSourceConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetVideoSourceConfigurationTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "Media(MEDIA-8-1-21)TestSuit.xml", Test Case: "TC.MC-8-1-21.10"
        //a.1. TC.MC-8-1-21.10: "Delete namespace (delete namespace for RateControl)"
       // [XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body><GetVideoEncoderConfigurationResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\"><Configuration token=\"VEC1\">        <Name xmlns=\"http://www.onvif.org/ver10/schema\">VideoEncConf1</Name>        <UseCount xmlns=\"http://www.onvif.org/ver10/schema\">1</UseCount>        <Encoding xmlns=\"http://www.onvif.org/ver10/schema\">H264</Encoding>        <Resolution xmlns=\"http://www.onvif.org/ver10/schema\">          <Width>320</Width>          <Height>192</Height>        </Resolution>        <Quality xmlns=\"http://www.onvif.org/ver10/schema\">7</Quality>        <RateControl xmlns=\"http://www.onvif.org/ver10/schema\">          <FrameRateLimit>1</FrameRateLimit>          <EncodingInterval>0</EncodingInterval>          <BitrateLimit>2048</BitrateLimit>        </RateControl>        <H264>          <GovLength>32</GovLength>          <H264Profile>Baseline</H264Profile>        </H264>        <Multicast xmlns=\"http://www.onvif.org/ver10/schema\">          <Address>            <Type>IPv4</Type><IPv4Address>0.0.0.0</IPv4Address>          </Address>          <Port>0</Port>          <TTL>3</TTL><AutoStart>false</AutoStart>        </Multicast>        <SessionTimeout xmlns=\"http://www.onvif.org/ver10/schema\">PT0S</SessionTimeout> </Configuration>   </GetVideoEncoderConfigurationResponse></soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing        
        public override VideoEncoderConfiguration GetVideoEncoderConfiguration(string ConfigurationToken)
        {
            
            VideoEncoderConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetVideoEncoderConfigurationTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override AudioSourceConfiguration GetAudioSourceConfiguration(string ConfigurationToken)
        {
            
            AudioSourceConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioSourceConfigurationTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override AudioEncoderConfiguration GetAudioEncoderConfiguration(string ConfigurationToken)
        {
            
            AudioEncoderConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioEncoderConfigurationTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override VideoAnalyticsConfiguration GetVideoAnalyticsConfiguration(string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetMetadataConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override MetadataConfiguration GetMetadataConfiguration(string ConfigurationToken)
        {
            
            MetadataConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetMetadataConfigurationTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleVideoEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override VideoEncoderConfiguration[] GetCompatibleVideoEncoderConfigurations(string ProfileToken)
        {
            
            VideoEncoderConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetCompatibleVideoEncoderConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleVideoSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override VideoSourceConfiguration[] GetCompatibleVideoSourceConfigurations(string ProfileToken)
        {
            
            VideoSourceConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetCompatibleVideoSourceConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleAudioEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioEncoderConfiguration[] GetCompatibleAudioEncoderConfigurations(string ProfileToken)
        {
            
            AudioEncoderConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetCompatibleAudioEncoderConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleAudioSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioSourceConfiguration[] GetCompatibleAudioSourceConfigurations(string ProfileToken)
        {
            
            AudioSourceConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetCompatibleAudioSourceConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override VideoAnalyticsConfiguration[] GetCompatibleVideoAnalyticsConfigurations(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleMetadataSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override MetadataConfiguration[] GetCompatibleMetadataConfigurations(string ProfileToken)
        {
            
            MetadataConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetCompatibleMetadataConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetVideoSourceConfiguration(VideoSourceConfiguration Configuration, bool ForcePersistence)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetVideoSourceConfigurationTest(out ex, out timeOut, Configuration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "Media(RTSS-1-1-1)TestSuit.xml", Test Case: "TC.RTSS-1-1-1.01"
        //a. Test Group Name: "RTSS-1-1-1 MEDIA CONTROL - RTSP/TCP"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <SetVideoEncoderConfigurationResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\" />  </soap:Body></soap:Envelope>")]
        //a.1. TC.RTSS-1-1-1.05: "Add '\n' after tags (in main response SetVideoEncoderConfiguration)" (add '\n' after some (or all) tags)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  \n<soap:Body>\n    <SetVideoEncoderConfigurationResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\" /> \n </soap:Body>\n</soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing            
        public override void SetVideoEncoderConfiguration(VideoEncoderConfiguration Configuration, bool ForcePersistence)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetVideoEncoderConfigurationTest(out ex, out timeOut, Configuration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioSourceConfiguration(AudioSourceConfiguration Configuration, bool ForcePersistence)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetAudioSourceConfigurationTest(out ex, out timeOut, Configuration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioEncoderConfiguration(AudioEncoderConfiguration Configuration, bool ForcePersistence)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetAudioEncoderConfigurationTest(out ex, out timeOut, Configuration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        public override void SetVideoAnalyticsConfiguration(VideoAnalyticsConfiguration Configuration, bool ForcePersistence)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetMetadataConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetMetadataConfiguration(MetadataConfiguration Configuration, bool ForcePersistence)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetMetadataConfigurationTest(out ex, out timeOut, Configuration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetVideoSourceConfigurationOptions/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override VideoSourceConfigurationOptions GetVideoSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            
            VideoSourceConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetVideoSourceConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoEncoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "Media(RTSS-1-1-1)TestSuit.xml", Test Case: "TC.RTSS-1-1-1.01"
        //a. Test Group Name: "RTSS-1-1-1 MEDIA CONTROL - RTSP/TCP"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetVideoEncoderConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Options>        <QualityRange xmlns=\"http://www.onvif.org/ver10/schema\">          <Min>1</Min>          <Max>10</Max>        </QualityRange>        <JPEG xmlns=\"http://www.onvif.org/ver10/schema\">          <ResolutionsAvailable>            <Width>320</Width>            <Height>240</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>384</Width>            <Height>288</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>640</Width>            <Height>480</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>720</Width>            <Height>576</Height>          </ResolutionsAvailable>          <FrameRateRange>            <Min>1</Min>            <Max>25</Max>          </FrameRateRange>          <EncodingIntervalRange>            <Min>1</Min>            <Max>1</Max>          </EncodingIntervalRange>        </JPEG>        <MPEG4 xmlns=\"http://www.onvif.org/ver10/schema\">          <ResolutionsAvailable>            <Width>320</Width>            <Height>240</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>384</Width>            <Height>288</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>640</Width>            <Height>480</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>720</Width>            <Height>576</Height>          </ResolutionsAvailable>          <GovLengthRange>            <Min>1</Min>            <Max>150</Max>          </GovLengthRange>          <FrameRateRange>            <Min>1</Min>            <Max>25</Max>          </FrameRateRange>          <EncodingIntervalRange>            <Min>0</Min>            <Max>0</Max>          </EncodingIntervalRange>          <Mpeg4ProfilesSupported>SP</Mpeg4ProfilesSupported>        </MPEG4>        <H264 xmlns=\"http://www.onvif.org/ver10/schema\">          <ResolutionsAvailable>            <Width>320</Width>            <Height>240</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>384</Width>            <Height>288</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>640</Width>            <Height>480</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>720</Width>            <Height>576</Height>          </ResolutionsAvailable>          <GovLengthRange>            <Min>1</Min>            <Max>150</Max>          </GovLengthRange>          <FrameRateRange>            <Min>1</Min>            <Max>25</Max>          </FrameRateRange>          <EncodingIntervalRange>            <Min>0</Min>            <Max>0</Max>          </EncodingIntervalRange>          <H264ProfilesSupported>Baseline</H264ProfilesSupported>        </H264>        <Extension xmlns=\"http://www.onvif.org/ver10/schema\">          <JPEG>            <ResolutionsAvailable>              <Width>320</Width>              <Height>240</Height>            </ResolutionsAvailable>            <FrameRateRange>              <Min>1</Min>              <Max>25</Max>            </FrameRateRange>            <EncodingIntervalRange>              <Min>1</Min>              <Max>1</Max>            </EncodingIntervalRange>            <BitrateRange>              <Min>1500</Min>              <Max>2000</Max>            </BitrateRange>          </JPEG>          <MPEG4>            <ResolutionsAvailable>              <Width>320</Width>              <Height>240</Height>            </ResolutionsAvailable>            <GovLengthRange>              <Min>1</Min>              <Max>1</Max>            </GovLengthRange>            <FrameRateRange>              <Min>1</Min>              <Max>25</Max>            </FrameRateRange>            <EncodingIntervalRange>              <Min>1</Min>              <Max>1</Max>            </EncodingIntervalRange>            <Mpeg4ProfilesSupported>SP</Mpeg4ProfilesSupported>            <BitrateRange>              <Min>2500</Min>              <Max>3000</Max>            </BitrateRange>          </MPEG4>          <H264>            <ResolutionsAvailable>              <Width>320</Width>              <Height>240</Height>            </ResolutionsAvailable>            <ResolutionsAvailable>              <Width>384</Width>              <Height>288</Height>            </ResolutionsAvailable>            <ResolutionsAvailable>              <Width>640</Width>              <Height>480</Height>            </ResolutionsAvailable>            <ResolutionsAvailable>              <Width>720</Width>              <Height>576</Height>            </ResolutionsAvailable>            <GovLengthRange>              <Min>1</Min>              <Max>150</Max>            </GovLengthRange>            <FrameRateRange>              <Min>1</Min>              <Max>25</Max>            </FrameRateRange>            <EncodingIntervalRange>              <Min>0</Min>              <Max>0</Max>            </EncodingIntervalRange>            <H264ProfilesSupported>Baseline</H264ProfilesSupported>            <BitrateRange>              <Min>3500</Min>              <Max>4000</Max>            </BitrateRange>          </H264>        </Extension>      </Options>    </GetVideoEncoderConfigurationOptionsResponse>  </soap:Body></soap:Envelope>")]
        //a.1. TC.RTSS-1-1-1.03: "Delete child tag (in main response GetVideoEncoderConfigurationOptions)" (delete tag Options\<QualityRange)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetVideoEncoderConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">      <Options>                <JPEG xmlns=\"http://www.onvif.org/ver10/schema\">          <ResolutionsAvailable>            <Width>320</Width>            <Height>240</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>384</Width>            <Height>288</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>640</Width>            <Height>480</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>720</Width>            <Height>576</Height>          </ResolutionsAvailable>          <FrameRateRange>            <Min>1</Min>            <Max>25</Max>          </FrameRateRange>          <EncodingIntervalRange>            <Min>1</Min>            <Max>1</Max>          </EncodingIntervalRange>        </JPEG>        <MPEG4 xmlns=\"http://www.onvif.org/ver10/schema\">          <ResolutionsAvailable>            <Width>320</Width>            <Height>240</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>384</Width>            <Height>288</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>640</Width>            <Height>480</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>720</Width>            <Height>576</Height>          </ResolutionsAvailable>          <GovLengthRange>            <Min>1</Min>            <Max>150</Max>          </GovLengthRange>          <FrameRateRange>            <Min>1</Min>            <Max>25</Max>          </FrameRateRange>          <EncodingIntervalRange>            <Min>0</Min>            <Max>0</Max>          </EncodingIntervalRange>          <Mpeg4ProfilesSupported>SP</Mpeg4ProfilesSupported>        </MPEG4>        <H264 xmlns=\"http://www.onvif.org/ver10/schema\">          <ResolutionsAvailable>            <Width>320</Width>            <Height>240</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>384</Width>            <Height>288</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>640</Width>            <Height>480</Height>          </ResolutionsAvailable>          <ResolutionsAvailable>            <Width>720</Width>            <Height>576</Height>          </ResolutionsAvailable>          <GovLengthRange>            <Min>1</Min>            <Max>150</Max>          </GovLengthRange>          <FrameRateRange>            <Min>1</Min>            <Max>25</Max>          </FrameRateRange>          <EncodingIntervalRange>            <Min>0</Min>            <Max>0</Max>          </EncodingIntervalRange>          <H264ProfilesSupported>Baseline</H264ProfilesSupported>        </H264>        <Extension xmlns=\"http://www.onvif.org/ver10/schema\">          <JPEG>            <ResolutionsAvailable>              <Width>320</Width>              <Height>240</Height>            </ResolutionsAvailable>            <FrameRateRange>              <Min>1</Min>              <Max>25</Max>            </FrameRateRange>            <EncodingIntervalRange>              <Min>1</Min>              <Max>1</Max>            </EncodingIntervalRange>            <BitrateRange>              <Min>1500</Min>              <Max>2000</Max>            </BitrateRange>          </JPEG>          <MPEG4>            <ResolutionsAvailable>              <Width>320</Width>              <Height>240</Height>            </ResolutionsAvailable>            <GovLengthRange>              <Min>1</Min>              <Max>1</Max>            </GovLengthRange>            <FrameRateRange>              <Min>1</Min>              <Max>25</Max>            </FrameRateRange>            <EncodingIntervalRange>              <Min>1</Min>              <Max>1</Max>            </EncodingIntervalRange>            <Mpeg4ProfilesSupported>SP</Mpeg4ProfilesSupported>            <BitrateRange>              <Min>2500</Min>              <Max>3000</Max>            </BitrateRange>          </MPEG4>          <H264>            <ResolutionsAvailable>              <Width>320</Width>              <Height>240</Height>            </ResolutionsAvailable>            <ResolutionsAvailable>              <Width>384</Width>              <Height>288</Height>            </ResolutionsAvailable>            <ResolutionsAvailable>              <Width>640</Width>              <Height>480</Height>            </ResolutionsAvailable>            <ResolutionsAvailable>              <Width>720</Width>              <Height>576</Height>            </ResolutionsAvailable>            <GovLengthRange>              <Min>1</Min>              <Max>150</Max>            </GovLengthRange>            <FrameRateRange>              <Min>1</Min>              <Max>25</Max>            </FrameRateRange>            <EncodingIntervalRange>              <Min>0</Min>              <Max>0</Max>            </EncodingIntervalRange>            <H264ProfilesSupported>Baseline</H264ProfilesSupported>            <BitrateRange>              <Min>3500</Min>              <Max>4000</Max>            </BitrateRange>          </H264>        </Extension>      </Options>    </GetVideoEncoderConfigurationOptionsResponse>  </soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing          
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        #region XmlReplySubstituteExtension for bugs
        //response for 489 ticket
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket489_GetVideoEncoderConfigurationOptionsResponse)]

#endregion

        public override VideoEncoderConfigurationOptions GetVideoEncoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            
            VideoEncoderConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetVideoEncoderConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioSourceConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            
            AudioSourceConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioSourceConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioEncoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioEncoderConfigurationOptions GetAudioEncoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            
            AudioEncoderConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioEncoderConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetMetadataConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override MetadataConfigurationOptions GetMetadataConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            
            MetadataConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetMetadataConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetStreamUri", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MediaUri")]
        public override MediaUri GetStreamUri(StreamSetup StreamSetup, string ProfileToken)
        {
            
            MediaUri res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetStreamUriTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/StartMulticastStreaming", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void StartMulticastStreaming(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.StartMulticastStreamingTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/StopMulticastStreaming", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void StopMulticastStreaming(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.StopMulticastStreamingTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSynchronizationPoint(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetSynchronizationPointTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetSnapshotUri", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MediaUri")]
        public override MediaUri GetSnapshotUri(string ProfileToken)
        {
            
            MediaUri res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetSnapshotUriTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioOutputs", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AudioOutputs")]
        public override AudioOutput[] GetAudioOutputs()
        {
            
            AudioOutput[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioOutputsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddAudioOutputConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddAudioOutputConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddAudioOutputConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveAudioOutputConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveAudioOutputConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveAudioOutputConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddAudioDecoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddAudioDecoderConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.AddAudioDecoderConfigurationTest(out ex, out timeOut, ProfileToken, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveAudioDecoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveAudioDecoderConfiguration(string ProfileToken)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.RemoveAudioDecoderConfigurationTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetAudioOutputConfigurations/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioOutputConfiguration[] GetAudioOutputConfigurations()
        {
            
            AudioOutputConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioOutputConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioDecoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioDecoderConfiguration[] GetAudioDecoderConfigurations()
        {
            
            AudioDecoderConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioDecoderConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }


        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioOutputConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override AudioOutputConfiguration GetAudioOutputConfiguration(string ConfigurationToken)
        {
            
            AudioOutputConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioOutputConfigurationTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioDecoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override AudioDecoderConfiguration GetAudioDecoderConfiguration(string ConfigurationToken)
        {
            
            AudioDecoderConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioDecoderConfigurationTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleAudioOutputConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioOutputConfiguration[] GetCompatibleAudioOutputConfigurations(string ProfileToken)
        {
            
            AudioOutputConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetCompatibleAudioOutputConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleAudioDecoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioDecoderConfiguration[] GetCompatibleAudioDecoderConfigurations(string ProfileToken)
        {
            
            AudioDecoderConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetCompatibleAudioDecoderConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetAudioOutputConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioOutputConfiguration(AudioOutputConfiguration Configuration, bool ForcePersistence)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetAudioOutputConfigurationTest(out ex, out timeOut, Configuration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetAudioDecoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioDecoderConfiguration(AudioDecoderConfiguration Configuration, bool ForcePersistence)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetAudioDecoderConfigurationTest(out ex, out timeOut, Configuration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioOutputConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            
            AudioOutputConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioOutputConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioDecoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioDecoderConfigurationOptions GetAudioDecoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            
            AudioDecoderConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetAudioDecoderConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/ GetGuaranteedNumberOfVideoEncoderInstances" +
            "", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("TotalNumber")]
        //response for 489 ticket
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><s:Envelope xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:a=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:se=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\"><s:Header/><s:Body><trt:GetGuaranteedNumberOfVideoEncoderInstancesResponse><trt:TotalNumber>2</trt:TotalNumber></trt:GetGuaranteedNumberOfVideoEncoderInstancesResponse></s:Body></s:Envelope>")]
        public override int GetGuaranteedNumberOfVideoEncoderInstances(string ConfigurationToken, out int JPEG, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool JPEGSpecified, out int H264, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool H264Specified, out int MPEG4, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool MPEG4Specified)
        {
            
            int res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetGuaranteedNumberOfVideoEncoderInstancesTest(out res, out ex, out timeOut, ConfigurationToken, out JPEG, out JPEGSpecified, out H264, out H264Specified, out MPEG4, out MPEG4Specified);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_MediaCapabilitiesIncorrectResponseTag)]
        public override Capabilities GetServiceCapabilities()
        {
            
            Capabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override VideoSourceMode[] GetVideoSourceModes(string VideoSourceToken)
        {
            throw new NotImplementedException();
        }

        public override bool SetVideoSourceMode(string VideoSourceToken, string VideoSourceModeToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetOSDs", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDs")]
        public override OSDConfiguration[] GetOSDs(string ConfigurationToken)
        {
            
            OSDConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetOSDsTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetOSD", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSD")]
        public override OSDConfiguration GetOSD(string OSDToken, [System.Xml.Serialization.XmlAnyElementAttribute()] ref System.Xml.XmlElement[] Any)
        {
            
            OSDConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetOSDTest(out res, out ex, out timeOut, OSDToken, ref Any);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetOSDOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDOptions")]
        public override OSDConfigurationOptions GetOSDOptions(string ConfigurationToken, [System.Xml.Serialization.XmlAnyElementAttribute()] ref System.Xml.XmlElement[] Any)
        {
            
            OSDConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.GetOSDOptionsTest(out res, out ex, out timeOut, ConfigurationToken, ref Any);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetOSD", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetOSD(OSDConfiguration OSD, [System.Xml.Serialization.XmlAnyElementAttribute()] ref System.Xml.XmlElement[] Any)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.SetOSDTest(out ex, out timeOut, OSD, ref Any);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/CreateOSD", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDToken")]
        public override string CreateOSD(OSDConfiguration OSD, [System.Xml.Serialization.XmlAnyElementAttribute()] ref System.Xml.XmlElement[] Any)
        {
            
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.CreateOSDTest(out res, out ex, out timeOut, OSD, ref Any);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/DeleteOSD", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteOSD(string OSDToken, [System.Xml.Serialization.XmlAnyElementAttribute()] ref System.Xml.XmlElement[] Any)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = MediaServiceTest.DeleteOSDTest(out ex, out timeOut, OSDToken, ref Any);
            StepTypeProcessing(stepType, ex, timeOut);
        }
    }
}
