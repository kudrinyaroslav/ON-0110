using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using CameraWebService.FileServer;
using DUT.CameraWebService;
using CameraWebService.Discovery;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Device10
{
    /// <summary>
    /// Summary description for DeviceService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeviceServiceFake : DeviceServiceBinding
    {
        //TestSuit
        public delegate void SendHelloDelegate(bool multicast, System.Net.EndPoint target,
                                               string[] scopes, string types, XmlQualifiedName[] typesNamespaces,
                                               string[] xAddrs, uint metadataVersion);
        private const int WS_DISCOVER_PORT = 3702;

        //TestSuit
        DeviceServiceTest DeviceServiceTest
        {
            get
            {
                if (Application[Base.AppVars.DEVICESERVICE] != null)
                {
                    return (DeviceServiceTest)Application[Base.AppVars.DEVICESERVICE];
                }
                else
                {
                    DeviceServiceTest serviceTest = new DeviceServiceTest(TestCommon);
                    Application[Base.AppVars.DEVICESERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        //protected TestCommon TestCommon
        //{
        //    get
        //    {
        //        if (Application[Base.AppVars.TESTCOMMON] != null)
        //        {
        //            return (TestCommon)Application[Base.AppVars.TESTCOMMON];
        //        }
        //        else
        //        {
        //            TestCommon testCommon = new TestCommon();
        //            testCommon.LoadTestSuit();
        //            Application[Base.AppVars.TESTCOMMON] = testCommon;
        //            return testCommon;
        //        }
        //    }
        //}


        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("Manufacturer")]
        #region XmlReplySubstituteExtension for testing
        //Bug
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:aa=\"http://www.axis.com/vapix/ws/action1\" xmlns:aev=\"http://www.axis.com/vapix/ws/event1\" xmlns:tan1=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan2=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\"><SOAP-ENV:Header><wsse:Security SOAP-ENV:mustUnderstand=\"true\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"><wsse:UsernameToken><wsse:Username>root</wsse:Username><wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">T47nb8advoejLFN8kXV7dFUtefU=</wsse:Password><wsse:Nonce>DVGaBtMDbvzLUUh0846BHQ==</wsse:Nonce><wsu:Created>2011-04-28T07:50:02Z</wsu:Created></wsse:UsernameToken></wsse:Security></SOAP-ENV:Header><SOAP-ENV:Body><tds:GetDeviceInformationResponse><tds:Manufacturer>AXIS</tds:Manufacturer><tds:Model>AXIS P3344</tds:Model><tds:FirmwareVersion>LFP-3_1 beta1</tds:FirmwareVersion><tds:SerialNumber>00408CA681CB</tds:SerialNumber><tds:HardwareId>16B.3</tds:HardwareId></tds:GetDeviceInformationResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:aa=\"http://www.axis.com/vapix/ws/action1\" xmlns:aev=\"http://www.axis.com/vapix/ws/event1\" xmlns:tan1=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan2=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\"><SOAP-ENV:Header><wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"><wsse:UsernameToken><wsse:Username>root</wsse:Username><wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">T47nb8advoejLFN8kXV7dFUtefU=</wsse:Password><wsse:Nonce>DVGaBtMDbvzLUUh0846BHQ==</wsse:Nonce><wsu:Created>2011-04-28T07:50:02Z</wsu:Created></wsse:UsernameToken></wsse:Security></SOAP-ENV:Header><SOAP-ENV:Body><tds:GetDeviceInformationResponse><tds:Manufacturer>AXIS</tds:Manufacturer><tds:Model>AXIS P3344</tds:Model><tds:FirmwareVersion>LFP-3_1 beta1</tds:FirmwareVersion><tds:SerialNumber>00408CA681CB</tds:SerialNumber><tds:HardwareId>16B.3</tds:HardwareId></tds:GetDeviceInformationResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        //[XmlReplySubstituteExtension(Common.ResponsesConst.GetDeviceInformation_NamespaceTest1)]
        #endregion //XmlReplySubstituteExtension for testing
        public override string GetDeviceInformation(out string Model, out string FirmwareVersion, out string SerialNumber, out string HardwareId)
        {
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetDeviceInformationTest(out res, out Model, out FirmwareVersion, out SerialNumber, out HardwareId, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        public override void SetSystemDateAndTime(SetDateTimeType DateTimeType, bool DaylightSavings, TimeZone TimeZone, DateTime UTCDateTime)
        {
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetSystemDateAndTimeTest(out ex, out timeOut, DateTimeType, DaylightSavings, TimeZone, UTCDateTime);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("SystemDateAndTime")]        
        public override SystemDateTime GetSystemDateAndTime()
        {
            SystemDateTime res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetSystemDateAndTimeTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetSystemFactoryDefault", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSystemFactoryDefault(FactoryDefaultType FactoryDefault)
        {
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetSystemFactoryDefaultTest(out ex, out timeOut, FactoryDefault);
            StepTypeProcessing(stepType, ex, timeOut);

            if (true)
            {
                string[] scopes = { "onvif://www.onvif.org/Network_Video_Transmitter",
                                "onvif://www.onvif.org/type/video_encoder",
                                "onvif://www.onvif.org/name/DUT",
                                "onvif://www.onvif.org/hardware/VG4_AutoDome_IVA",
                                "onvif://www.onvif.org/type/ptz",
                                "onvif://www.onvif.org/location/scope2",
                                "onvif://www.onvif.org/location/scope1" };
                XmlQualifiedName typesNamespace = new XmlQualifiedName("dn", " http://sdasd/asdasd");
                XmlQualifiedName[] typesNamespaces = { typesNamespace };
                Uri uri = HttpContext.Current.Request.Url;
                string xAddr = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + "/ServiceDevice10/DeviceServiceFake.asmx";
                IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                System.Net.IPAddress ipAddr = null;
                foreach (System.Net.IPAddress ipAddress in ipHostEntry.AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddr = ipAddress;

                        var ep = new IPEndPoint(ipAddr, WS_DISCOVER_PORT);

                        SendHelloDelegate shd = SendHelloDelay;
                        shd.BeginInvoke(true, ep, scopes, "dn:NetworkVideoTransmitter",
                            typesNamespaces, new[] { xAddr }, 1, null, null);
                    }
                }
                if (ipAddr == null)
                {
                    ipAddr = System.Net.IPAddress.Any;

                    var ep = new IPEndPoint(ipAddr, WS_DISCOVER_PORT);

                    SendHelloDelegate shd = SendHelloDelay;
                    shd.BeginInvoke(true, ep, scopes, "dn:NetworkVideoTransmitter",
                        typesNamespaces, new[] { xAddr }, 1, null, null);
                }
            }
        }

        public override string UpgradeSystemFirmware(AttachmentData Firmware)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SystemReboot", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Message")]
        public override string SystemReboot()
        {
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SystemRebootTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            string[] scopes = { "onvif://www.onvif.org/Network_Video_Transmitter",
                                "onvif://www.onvif.org/type/video_encoder",
                                "onvif://www.onvif.org/name/DUT",
                                "onvif://www.onvif.org/hardware/VG4_AutoDome_IVA",
                                "onvif://www.onvif.org/type/ptz",
                                "onvif://www.onvif.org/location/scope2",
                                "onvif://www.onvif.org/location/scope1" };
                XmlQualifiedName typesNamespace = new XmlQualifiedName("dn", " http://sdasd/asdasd");
                XmlQualifiedName[] typesNamespaces = { typesNamespace };
                Uri uri = HttpContext.Current.Request.Url;
                string xAddr = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + "/ServiceDevice10/DeviceServiceFake.asmx";
                IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                System.Net.IPAddress ipAddr = null;
                foreach (System.Net.IPAddress ipAddress in ipHostEntry.AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddr = ipAddress;

                        var ep = new IPEndPoint(ipAddr, WS_DISCOVER_PORT);

                        SendHelloDelegate shd = SendHelloDelay;
                        shd.BeginInvoke(true, ep, scopes, "dn:NetworkVideoTransmitter",
                            typesNamespaces, new[] { xAddr }, 1, null, null);
                    }
                }
                if (ipAddr == null)
                {
                    ipAddr = System.Net.IPAddress.Any;

                    var ep = new IPEndPoint(ipAddr, WS_DISCOVER_PORT);

                    SendHelloDelegate shd = SendHelloDelay;
                    shd.BeginInvoke(true, ep, scopes, "dn:NetworkVideoTransmitter",
                        typesNamespaces, new[] { xAddr }, 1, null, null);
                }


            return res;
        }

        private void SendHelloDelay(bool multicast, System.Net.EndPoint target,
           string[] scopes, string types, XmlQualifiedName[] typesNamespaces, string[] xAddrs, uint metadataVersion)
        {
            Thread.Sleep(1000);
            Discovery discovery = (Discovery)Application["Discovery"];
            if (discovery != null)
            {
                System.Diagnostics.Debug.WriteLine("SendHelloDelay called for " + target.ToString());
                System.Diagnostics.Debug.Flush();

                discovery.SendHello(multicast, target,
                    scopes, types, typesNamespaces, xAddrs, metadataVersion);
            }
        }

        public override void RestoreSystem(BackupFile[] BackupFiles)
        {
            throw new NotImplementedException();
        }

        public override BackupFile[] GetSystemBackup()
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemLog", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //MTOM
        //        [XmlReplySubstituteExtension("--==Hr5qBwn7N66Cwrtq128ZSPCj3b7Cm6+tm3Ynn/i15c3pHkT8aPVseYQWzLYZ==" + "\r\n" +
        //"Content-Type: application/xop+xml; charset=utf-8; type=application/soap+xml" + "\r\n" +
        //"Content-Transfer-Encoding: binary" + "\r\n" +
        //"Content-ID: <SOAP-ENV:Envelope>" + "\r\n" +
        //"" + "\r\n" +
        //"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" +
        //"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:Device=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:xmime5=\"http://www.w3.org/2005/05/xmlmime\"><SOAP-ENV:Body><Device:GetSystemLogResponse><Device:SystemLog><tt:Binary xmime5:contentType=\"application/octet-stream\"><xop:Include href=\"cid:id3\"/></tt:Binary></Device:SystemLog></Device:GetSystemLogResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>" + "\r\n" +
        //"--==Hr5qBwn7N66Cwrtq128ZSPCj3b7Cm6+tm3Ynn/i15c3pHkT8aPVseYQWzLYZ==" + "\r\n" +
        //"Content-Type: application/octet-stream" + "\r\n" +
        //"Content-Transfer-Encoding: binary" + "\r\n" +
        //"Content-ID: <id3>" + "\r\n" +
        //"\r\n" +
        //"-]olm$O{qQm46/Im]93LG$<^u-@t]W\",~2\"z'0J q#jMV dE\"\"N'3V1znxS}cv[GDnE@Y*l'[vkDF<E2fh)MA1g!z]z;lUW@3A	NSohE4'j03/xiH35:!jTU.rV$[C@gPTbh-rqAZZG\\\\.Dt0~5yfz" + "\r\n" +
        //"(604aB&-.T\\dOeGwB}tX3V6TAXB=.*4T%h" + "\r\n" +
        //"l*U+@s\\ NN{XL]^$#PcpJHKQxH@+\"@e/	?e	(" + "\r\n" +
        //"~A" + "\r\n" +
        //"FGZ~4])`d9S*HR<BA9tb4+TGRiX}JRo/`zPa=W,cm}	*kCuPu~}U:F$U%xwu>O	F.&h)PKH}[GBG2BtK[GQHaA" + "\r\n" +
        //"iX7!ibO-P\\!6AEKz#_7LlbJS>t\\>*qr9QSc4" + "\r\n" +
        //"#0a<8\"?yJ)EMRFN Pv@,M5F8FI[.,[)12U48Is/Jl9^%\"i)IRm+@swI$L_2/;KsU15HAjwGaA/Ndz5]M(c" + "\r\n" +
        //")k\\{\"G7/_7y%eD%u." + "\r\n" +
        //"lE7\";Km\\p[bH{,!WhE=(U:i@Qjd}w>fXIb_|EqJ+HUZI^]j\\U#[5dRa<I;u:8&O	{:\"C{qU/cP[o/r4i{g-O$|" + "\r\n" +
        //".z@/Ly[]';5g,B1-1+O<VYnq%h[R%fd-*w0{nz\"OA_:4-{MZUy% Bu?wOhF#I%]7\"EPY&awK_`un~=9;=OB=Ww]`.*dGaQc," + "\r\n" +
        //"tr$b3wC5~3k pxPb0S&	j0hQrI12(;nML_DR!EjwK" + "\r\n" +
        //" ^zC1c< " + "\r\n" +
        //"+0+" + "\r\n" +
        //"(W_{NN (6\"3@` " + "\r\n" +
        //"8UcpXNluCG7/W)Ggx?r8'>y4ras[?td,!~l:C0[s\"[rbCzL-~S]Q+D\\MHaYd28CoOR}f\\#fTYMaRkM*C4K\"tvwg^S:-k99" + "\r\n" +
        //"^S}1|O#x;vz@Z_>?n}M?g!}HM~?y?	Y?r4~tw+Qw{RuP{Bh'gaz_p1vG$0}mc4	(8$Sp'DB/	)Kp>-$pc}m	04h!dZ FiR[fC?0J74^hXD3R!fq\"z*'8#NX'C8it	w}I[xV4wy*jQX-j E.-*-j_V%,?D3ana7-BbVC5<tQK:l_kbY>v}N5-z#:b,WK:z6I/Ep'QuJN3i#1EugyfQQ12}LhO)g[96<IFQ1SgTf{f)J5&^)jyM^}V^1y3Z/JnVKSLKF&A_" + "\r\n" +
        //"',5?Y}~YbMY	V;|aO;{!^v[|/I_LNZCqy//{RXQR8[i^&KfU?K0|RGkbyWhqKiSVY|F5L$O33" + "\r\n" +
        //"K/=/oj97;3/FcCj ]<>H9.0)}`g[4';xP_5@JVwD,C*]nj^62Gu=uA_Ft3]W:NMd)zc`F^|R8*vW(sXoe[" + "\r\n" +
        //"la4!evXz\\G%DtV)y:0Ay=n=ouv@*i	{_fz[.1+(r?6r=ynJ+oSX)F!40(~h0c^nI" + "\r\n" +
        //" A.N1J(&kGEC)P:F	UOGQ]k~>UGZ5oc&(9k?Q7X!JBM/5EV	=0/yKvZ2?W?j*H)z:*uwP#*rk4%di& ;fd7^vm?hVb2=6\"7r*r_6r@U-LbB	,!Gvm0nwAv d\\Ekb7v^0K1qr5-zS\\+TO-}4BZq ?T?	teWim` Kc9'/ia:J&B*4o-hy+~9p_|8qr~EE:W;]kpWW_nax3a7`Udd2i[xyiV)(V'7=FWM&W6o7P2S7Z&=fM~vt s$'Nd'N{Z" + "\r\n" +
        //"oyE@1W1tqS>7r#vM7oK6g^_wG7]0wb*G11HTHAsckqS!B.S|?_D	g2\"W}p2.\\<Pg2B`6S'p}xnsxX]^F$>ty~by(P=,Ez4AI)O3D5yii664f\"MWh+65kMl!n!i!m!kjIaxAaM5-z^\\zrq3[<La6zMwbXhifRcOH+<GzffJH2nc%eU?N\"'5bX z->C" + "\r\n" +
        //"eM^6CdB#CyTsnX!!ar\\z{qN|Ztvi>kgEu~_%`v3%89=MooSA&<XA'>}oq@N4;?%VhgA,,[%BP[3HfHfZ$DT$Dv,Hv,~$';{YNV1E;Wj{d;H;" + "\r\n" +
        //"dO	rysw\"sJ4cF~PCyZuzX{@oy?!'BNZJ~*9)" + "\r\n" +
        //"nu#tE;1JWW(1aF_A\"mA0)mt5hV4Se7Idy~ClL :tr|evU} )&k|!%2o&\"jTT]4o@1&wlvg|$?F@GO|'O;U:L+W	aCT#e139x3QMIMKdh>(EP$+KQ9@GAF6" + "\r\n" +
        //"~1,z\" MX Wov;6TFTh E9kWt_?-_vt[0cEjB{A{c7CDtiK*TzElB?)$xCrBE1?D%isU\"-GX.S'K]l6mEI6@Q.dNZ-xk{o*_?{LU'_=FhfO5OQQQ)D)6d4$={{Tf`jG'7~TOTRk[@!7VrZse'6[t?H+_yK/_glNy!3M." + "\r\n" +
        //"<rwS$^SYxB,X`^$ntRb|/UI\\zX=vn+Cis?	^c7xzEe*t>=3{<n	rUSJ3++k>rqWgpq(_!xZ~V{E" + "\r\n" +
        //"@B_v+u9\\*,yj4,Y=7YnS*TT6!	.,?KuT1{7;z@,VFmyom2{V{Iah[W-J=C_YDU2]KS|.o&0ozo>jFv`-P~ff']??4}zK>'#T2Q%ezVXqEXwdC.&EX$Er\\^z34}=\"in8:$V=vS:t.M*P}*}EiQj" + "\r\n" +
        //"u]U&KXcn2GK$>*<eFz8x^D	F\\s?W'xbzR~pC&O:d(]0U?WeeV $3tB>]-^YEw'Od<yq<~Z56ckM" + "\r\n" +
        //"k2g\"Ww%DR#Wq.G!sg16WIFY@ )(m+^$v_F	YP49|}To~Wz" + "\r\n" +
        //"jriqT *s tUz!\"	d6>}^ZV=a|" + "\r\n" +
        //"TxZOx0\\Tz(wm)9&Izm8'f^s$`$a#UV7jNC'bo_gYiQm2$OCfn|6Q-Ttf>k-{|?z<(h%~>|4NdT S*?J}}_lkn??y?T?r?B,|_QU?:!|0w8:4j54r]KB+;	n4})z9[*V~A#x1A6vp[5" + "\r\n" +
        //"H|7[8S~'y$D_9WoFE&z` <A=P!myI~VQ=YE/9$F7/X+/iNPn|Swifbl'U]XP7e`&se@:y=OVpbp@j#p^A80AhH-Em&e*}:ZET*pKC(*7'\"WD8i9\"2B;EWJ{EA2#$2[[EsE9m	yfr8cg\"l7La[|B)GRE~85Z0=I}:c-*Q}7W#,g]t`O6dTPI8O(v)7ejgwLo?d~4t`q3rS@4lNL1Xz3)HfBO4R<q,1ox7r]Gf" + "\r\n" +
        //"B[WR@;jH$X9N~g{LB+IHI?Oeo cAi0@/z@A)hY" + "\r\n" +
        //"bOI`ToFLq/+5:sdAS(Tp~JgDs*s]bfVf{#-NP!aBBPBVQ$L85WZ^g!eE]3)zS'}z_9r[<n`x>ugc;%&q^nAMcEY#KRk+dc)5n(Cv0l5bP!UnpT}2nxKbfi<v$:(A)X?.E{5Qk7Tfr2VMV!tXClAA%*d-MER4yYU*U3S" + "\r\n" +
        //"-(9(hhRw@h2bH|N&E=YWfA\\v5B(&;<Z)~O>n16W[61_A[$3|)C&myg*eesn-t&ak*iS6{y\"7oK3(I[nU2L[igrtr*Li\\+" + "\r\n" +
        //"DijoEn>*$b;Fp(M&p\\ } }lnR3SVCB~PGk" + "\r\n" +
        //"}<	[M:$wTJ@r@RT|`RYI\\h&JJ1&EebKo1crp$D2Qi" + "\r\n" +
        //"P<84Yce@L^FHiaKfvf#*s|_|i9${\"\"u8WlMJ=gR8YO)Z,:4c6z[JKIjV&&q0 (0F:`4E^+???8A?8y/=na1B8?C2gW|N>&J:)FGWn<V~>?uQ@[Wer" + "\r\n" +
        //"." + "\r\n" +
        //"\"Y9_oV (^	\"h92;+aNM3N[SclMXM0-){%kk#A<.'6.tpvA<Qs&:J%Nc3Mm;rId9xb0jZ\"{jbxPq'}bZm[$r)fEi$0OcvK%8\"X0hP3KL]~2|-] L%Jm,q~Z^H^v*}Gn$TaV@" + "\r\n" +
        //"cqEq.*otc!\\{((#V`3k< ]fqmfN2/nsxDPjo&`7;rDilt,2\\q-Bd43cmMm0s3b~W,)~w]w#exixaW4BdfiN%X7t,p[xu1kXqbXnv\\w\"zLNvNFu\"'Q	B@g1z^RL,3{ef=lmQB [HDlos-X53" + "\r\n" +
        //"Y1rs|z-FE?E_,?[~	+~n&Q|C-V3d|VsLp]UZSo(>D6=7n	&vO!azcjsJw-q6]qV8_?>?.oT-s_OV^}|+\\t.>E[|V@J9s{BQ>Fp1	Gr*a]a'ni4_|2*4'+Yw/Jk<F:v4?~2nO]yP4=gf2l&l[j" + "\r\n" +
        //"'Z~<nm!Zp;D}\\,-7CW	_U-G3~WfGO9kgWAP??%oRIx,J@51gD`|OJv]Lx;1^#r[`8;7?Wpd	H4N,<%U!-uQU/#K>+R%AIv@S *JKEE9zvM$W|'+Og.l7zLP)Ea0; 0S(=ucD)?0i~c$79" + "\r\n" +
        //"erB1',UuL	C2``\"]]km%>0KTjWom|fn^7g:ij+7{PmK[I%AgzhpA*K:2`D$}q" + "\r\n" +
        //"G .Tj\\.~C:UKE*x9hVC2O(:1zL/\\;PwfJ" + "\r\n" +
        //"zj^!4Pv z,T59fvMeav&?f``71?J;	sp\"@DxikE," + "\r\n" +
        //"O004p" + "\r\n" +
        //"TLC.t~LL:=)9l?F?*=D>Tjgt&o&<`%Vz>K- m+53\\?Y}Xk:=D00Vb'ZnH4{|5Z>Izh\"tlaQXVWG<L4" + "\r\n" +
        //"B*zTj)GkWd1vXt^W9#TdZ' `sFS#xvJ)/a^ugvwZTDBoJz,09ItFXpI0AVm}&$~MY4qH&d/hGZVRcHW4ZcyKBGvh',R" + "\r\n" +
        //",;F t" + "\r\n" +
        //"m~c4!K`I*T}ukI5ZoTiLRNtScaK;Cp95}CC(x	hrZJK[s@E3|`#" + "\r\n" +
        //"1SQPinJ'\\	$~lbnbiP7R5B07#/&R?W*=[WJ`>-,?bKJl@C8viuyB[4:MBJA|E@wenH4T/FxkH-$VN@Tju+#@[C5XS!mLq/i\"TN32% yT:uXhc`/0@6gzydKXBPq0=o gI\"\\:I=E2vm|9\"\"? $*-&HaD<tEw\"HBneJ/Qt2K%^70+%fEvuIG&\"dNzXam2k\"ByQBm!.@J'sc'de(0k}xN(d%>4u.32X$6A|q`RXj?0" + "\r\n" +
        //"zwa`0q? ,FbJ,k1}`cd'#icn5vH3V,dd`KPSHg>mzZQimc~jmU*nh	(vC_9*ia8LI}&5&.lqpAo~2 Ru7aqK/\\!w.5iRZ(K(+,%_r@Z.Ve	%!^sb" + "\r\n" +
        //"WXF9[MyT/I#k1oWFN:I7KKX)\\ZSPV@E)(_IWIN41CDo@Obzzy%D,CA t$<gkOA%\\4W`k,gV	9*K6k!!Q0gF2`/9_(9&|uz{Dz5On=C,rLg/-xHc:6AW2z-D8VJ[Xh=`&:}yx%K']" + "\r\n" +
        //"(`rval(Lr9'f0yiI98o pdo8kq=[.ZsBWpy0k@j1Ye4E;%MT55R{s* -i_t;&L`3,PzbS}}	-0Sg(g)r,8<48joW?@LcO&2 0lvg]M}PVBBvbm=#YjY'Dl5y:Z(cDz.W(UbmL[-xVK,`=j8(XBF$\">H#WQXm-b=(" + "\r\n" +
        //"&_|<ru^i~Z[=E-w/A5TW@R^I" + "\r\n" +
        //"H)FH+!vLc){7ac,}R{nue1caw" + "\r\n" +
        //"T97rt8Gh:Mp4Fx$`YVgUU;(<sz3|&;RV*WzKnVu\"2)]k5q/[4U(;Xj,g5sC(VOO=1F;ChDI,/mi9*^tT" + "\r\n" +
        //"+A`=lx|IE\\%=)^k~yqzuO+?WffF5?E\\[FW" + "\r\n" +
        //"RSo[ST@=u;g3LF=xi,X#.ktmgz1y2.$Ch<hS0}|||7zz(#7C6{n!9j`xN!CM^;7Xp`y<!Z<m)=rB+(F%&i" + "\r\n" +
        //"Xf	@O0uh!lMYVsB,X;a7!	 8Y\\g&rX'=+d8HA0)VG=gb3kSpihl;_>JX!Va\"NA	q|	'~D.WF'Mj`SKW#*Vhw\" %cdB<f:C|;!Ao}ZqWj	x`7Zm39T-S&+rI7J<5\"+?\"$d'<%;a+Nu[N*LtqDwIH>CsO>6YCXNU|jR6*y)f[e9IvQp M)?|LS.*,XQtt%?+.6Jv\"}tp:(u@Pc(k~0k;XC(f5Wzdb`" + "\r\n" +
        //"xEm_(ZF(K,Mng7}),b/uSRig7iMH>u}i!P~v=Z{T+aa?TKIA~~Ok_?N|Mln-Vjv>Gm0ECL~vqy}Fs}q$~\"2!^cX6e x%VwOM'[O7O9w9l/#eb6R7&yR&9\"9$i}akHcYk7|iz7J,#y:k{*bY8TsoQWwlls}qC}$ov\"5~rKNrh(-ZEPsPX#L,VcP&DU6Sq=P`o=$P)6=B$]2>NOXskz)TPV?$^`~T4F'K1itFa{28GSU`8	rg]Yy,Xv>vJeUkeM$/k{iq3Aj{}K-Mk>{to_R$E6qg3vf3= 03NDD,~%BbHr4mv^	;4x	{rw bYR3c" + "\r\n" +
        //"{1:rwwbt,~	Z{]-	=kiz7.`L2]IreHsF!?Q@UI_DdkuO/39c])Zm8T5_I:" + "\r\n" +
        //"a=v&x\\D\\z/_nBO	*t^I_/SG>:xB.1/@H;>2Q6g#xZuO		!9BXgaA8:I0!{GG	;$)xAgZ+4Vz,zNii!p9(rpv,k4vm DR+H1' (qmKa4&" + "\r\n" +
        //"KQ$G;5q}A/((	fB17,$+F61Y	3UF<8+'pyKd6C\">.wselXx:3\\/d8l}O}fb~ug|&('mM6h\"~'J=7<'p>{yo.I++]E:r>:>[[z32=1sRu	E9:};ixw4~s?cO:s'O=t9wYSg{yFy+#/a/>gj_GK/}Q7WfvqM{:q{G'?s?K+k[;__???<Tg}|(?{y)#[O~>|WiOz#+/}i^NO/zw8h/in7{wwl?[HwyZSoxZgO#t99N:C1:z" + "\r\n" +
        //" 8`zCpMJ 9MUSVE9Pt~g[a\",uU@Vlt6o,A!,Sc+M'|xpog;T@!Kez[Zh#\"VU{D-(]F'F>kZe1]j ]@s~Z`4_(7$),CMX +*z4@di,0*8)_x ,`IQBYjFpXmQ3:;uUQ&B+xAOj_EW @A:A@=XiPpDWuF\"hQ)uz-=\\hGjd~]0V\\hG,90/R.v(2A Yw{4F^r\\]v@1gv9TB;v)Bx}k$&HMp$MhoD	i$$n}qg?avB_HNjhsf`S#/nZky+.Zy4ZkzI2,[q^.K*M>XjBYm+V@AI	$Y	Bj(z[6b59zFDNW2B! Q@KC	e!P$TBd2&" + "\r\n" +
        //"MH.u|d	mB$iaIp! OlkS	{A_[g3${6#)SBoOg3hf=a3xg0`f=uWf=e7+8{hO:o(PWZ5mvLz>Wq][C@uaWT}>k" + "\r\n" +
        //"JyqRb:|d_-z(~R!T^krCR+4_T6" + "\r\n" +
        //"M[R~@R|J2>~]*1FkE?Jqr|PKW8!~m2Kw~8p]S{R_oVws`-k\"tXki+eB4`LrQK~d/W" + "\r\n" +
        //"LR_<()1JzI):,rV).K7p\\o|#{:cgXaV.gM#{1Z}:f{{^m!l?4kpG::Jiir:MI`NX 0HiC*vtCND'a&\"a}Xp!N*)0}$@:@=xCSHG}\"LC@V>GP]@TEyWN9dUIkzW?>Nyz5:zy7D4opXl|qo/1XU	tB!d	Fj)f ^:^T<z-G*/500Rdr@6	^7J'1iV5GqG!dhF\"j4.z^ y3\\>#@.N  4M;^w&t_[Z86}io d@ OZPr@VYe*$m5@[9I4]22.jZl.R<X(}TV OMGjBHJ43$%J$M$$|($b$+t0L.,uA.$II" + "\r\n" +
        //"(R[Wk!z'['j$%$y%VU&TinbgD	!lN89L$$x9	7r lq/[Rp;fvdF14{iC JERe8^d7n)o~PN:7_LIw3Vx[.`O8oeN&tf8<rxNy^Y	kC^K	^;ChcD'}:c$9jov,A2k2A7u'4?NeteY:a	:e%Y]NZNK$C/A)tvOfa2`wV#UUS?L/PRZgp?FZMuu^6JVLylZ#0DAl{pbhl+[1f:7'xRMA.S+jQ(7*8r5J/b^#,_jgBc|C&lPf$`l%` 0!Z^n	X+)a1kj?u~l~}MG>8" + "\r\n" +
        //"&+L;Sn5z#wmRDD2TQ7NuMzXCyH/<' G ;:vF?:zo0.h6A_<vX]9?" + "\r\n" +
        //"`1@c>P8:BE" + "\r\n" +
        //"P2;GbhL:)U#entrP^)ri\"tR>x>f[_rGSTYjd!'to" + "\r\n" +
        //";a$pV6[0E3cN5~qx9(ap	$Z^xp" + "\r\n" +
        //"p^}y<W7aMpNwrQLsZCa%^$S7%" + "\r\n" +
        //"Xt2,C8 F7$ p'!S" + "\r\n" +
        //"2MiXe60.gzV\"nnXFK(]A6D7R|YEf=+=|oyIF3X079H31J8l%NQS;sn6-3X`dr%uYHZ\"x7FPw=?K+*4c4D`Eh[]JhhS(C" + "\r\n" +
        //"%7^hN&ZG~&:9\\ nVuY*{,+d}l7,85a@lWe,b	3J<M,T[ Z3c9Q=bf7@[	umi#khMkg&cWiV3gMunV~3`&'\"{fny\"h:XzS{p|}<vo`xZj)N_&7I>3MhM!8.zf&rH+Rcb=shMo&38AN8" + "\r\n" +
        //"]j)xcYiBH':aV fM$dYDCLFH$#BYxG^26cH(mC`fl'kwv1@hIG ^dMUvedA2tf&z2:m1e" + "\r\n" +
        //"tt[YQYx7}NsY)Qhe`P]	81iF$_;G] $xn{,3!_beVA!gu2^2*Lo8V9K.<j\\[jyK,sV4UOLvl-R6i09Dzv, N_bwzK" + "\r\n" +
        //"H=" + "\r\n" +
        //"$` }:le77xc2oC]z^`xYrXHU)r=_yo!a8mq^)?[h--9V~m!p!^vB#|xuAb7R<&qwR`z/_)xgBn1'sdbt4DDWlk,Y3YvTs2vts(L=\\fp@ oO263M>wSm\\g; ,{n2[RCkLs*6qlMMgfa41eGQtjM7bXx:(Z mgOfsMsskbk<Xf9.cUgKN%EW'9c>e>V'tX<t<3ShjHnJ-be*`+s\"jNjjs*cT%CLktKDfhn9WM=fLUs.kSG-ulL[ER|27d@{fM=W,Mq[mX" + "\r\n" +
        //">)&$Ontnzc=7[,@-uuX)aFy''TQtZ46O{VQ8Z&M|NM`Z8" + "\r\n" +
        //"htZ@w&QJ)K0i4%)7?D8Y0Sl4JEY!0j;XQ#%d	~{4=/w#`<e~T<}uuF'o>bK!v#OXAl4@B  z'\\VR%=mr(" + "\r\n" +
        //"0vdFNRCm52h:]GxS3yQ1eAZ$sK5XC~v36>J6]@_v~8I]b8U'p]c8^)$)<G6-S#xK8v2wj>>3c>a22HV*^#O	:1mZW11:0*IOz	(	]x2ns_\"[H-.7,-cimJam9	ll;Pzl;8`eMfYn7Ziwg0XV&/4#\\eC6ZZcitjWmp8[tS!H^u	]pWJ}-C=CC=W=`*-bTZUz!TZ,r\\LiV7rE:[=i]pC.Ipo4Y^m.\"uMk:x:I_jYK1S><lz9{/9C$XdT<R\"B-dXH/tt/b$V|AyM]YytU+8._:\\$A>:D.G~%{K{2@(rVr#2!tm/:];0%b(E+o?`@mm\"" + "\r\n" +
        //".G?%|qsAzgeZ`EY" + "\r\n" +
        //"cO<4WB<,P	=B5 T|D9po07PK|aR3,Ld`x\"T$t$3#<AnWe)yS;*KaqC|X0av	uUA		]L<E~y)v.~!S0JgT	ozpAU" + "\r\n" +
        //"7$$>J%=rF4EpEi{I{7f.S7@VXX@+G;sn5bm0f^}tFx{DV{=wHh+" + "\r\n" +
        //"19xi<@el}M'RSa%GeJZ`G#WA,4i1F u&-0Fxl@\\p(l1 arbCl`e,=`\"d&Q/N4}Bsz?fp7!^7Bmp)T-|o||oYe" + "\r\n" +
        //"!T=jBxPqVPz_ pBxmy|-yAk+c)A|L`g" + "\r\n" +
        //"YVZ.y;aBx5*eDw'1Il[4-N[Z4Rw*_ME'TP6`, !K+5=0f_?P6ic4/	?I0Fr|WKaK&D4w('3@]`)Y<!9,n#f]>H:J$" + "\r\n" +
        //"UU;\"D?K;tqo[o{4}=6!a[aUHL}XDR	,9f[8dr*zX$SiW-fJ6SLymm6<u69;_7k7FUQq'@BiKU.mA_Fe2I2B^maCv2<mCixN,&:b?}yyWi]Ikj2_2Y1\"\"m:74bdw@-sIjZS|jKeh@\\e!U;TPY\\oAq/25S-{wq/,[,#Q30Y eYFQ>q/,[,#Q3A?,;azlA|F|Ex:dTAfV7d{}cxuPduqh2cIWwasuH*,Jp#K-slFwMa3J1<,WSI;OO%t>;u+)1IQ1%iZ#u4TIFtnM\"u!qyqN_J/hO{xD{I4-.1QiJyj,gn-7XJaf'Y4:7$U$2pH'Clt[XEqQ];qb<,z+%0\\40{NMI,GH~Szbk`~\\%.%Un?} !.}ywW.HzJa.! w~&gE!qE	Q:Y2tO8P_V]%O\"KpAHpS@;m9\"C-II}CuAv" + "\r\n" +
        //"LYqR!/iV3X#D}.G,\"=F\".T]W!K^lFxojERA5 " + "\r\n" +
        //"c-1I" + "\r\n" +
        //"aD{JyPpw&+jb;` G\\K`n4DyPyi>e`i]v6;~cTpK8.[A*#bh9\"%<kBX`Gg.x$$^e|x$nZDJS1utZlTVs\"k]m=Nza^ $N	03?2c`Xmd~|w6/?tEgbj`@8R%wGyXn &>C,2sdX" + "\r\n" +
        //"bM:-(l|W1Bl\"<*f#:%8qz2=|J`F[1d$[>~h.o=.*O$PA]tC%i!9CA5\"/g`^Dy?q\"V^ed}V" + "\r\n" +
        //"/L!S%g}'jTV.z8DEu	6I[qmv3$GhhDEUpr%CO]^SSJ\\3:K!EHY1>~@n:HH}Is\"P~xQ:RDd(b#" + "\r\n" +
        //" b=MTkW>==+f6Hpsa&C\"nuGr6'EvXt4hk#J4_[j)>7(=+h\"rVP{JS%BT<km.8)NI1q{!lcOZq0Le5Tu{k=Ift{J:[J=f94GOwj_Y]VV$Tz!Aqx5ZiWn=Z^%!5OZGd#'nz/ah2VV=zHBOOjoAqsu878`-X%O<u]=.vF,1waU#a8Y(E)SF	ic*z[:	z~pm7hVtMQu h[Wy1rMNm,-iAeehm-~a'1z1Ja)+6BXq}?_IIp<L=M}ynzt2O3+?yqsIa{~	4H?L?O~8~2Ow?l/To.4:;|:rKzzL_\\f>qwqm,R9#s#YqPg4=MfBtES2x,,r3'w#((yUlF>!VUjbCZX+V5]bLoPt#~^?y~^?G$m ty6Fzflz^@;KFSIN?q:I(*9eL6$%1HHHx:|h]@7h\\Xc!}xVrD}Hl(*By)@J{)]8b=.%v'o>L7IRl//Rt@VM5x&t(qpA&54/Bw@M-bvq_Y/kbO=j7D9k`kNG$@3*qO-w\\omhr<uu2dzhZb>+g$zJ[`r<&1_<,\\/j/oc	y1tWNh=M]I2@&ZJOWUrdj	i Rp?'~*_UU" + "\r\n" +
        //"fR}g{'~(FJ'9.R7	!!UDBe>s#M2sR}_wG'>l%JsBZZ7%JQrS]BDnZ@E<	~5im1YF!hhP]fw0xcGbLz.+Lhhk[|,XEaDnP9HSo	I#2WMvM>iGQ37qwV;+~C;#Fmxr+v1fB5Pt+:1	a'd'	l::jF" + "\r\n" +
        //"|7n&7:0'$s2W34|Dx{{_`?d(.UTg %J" + "\r\n" +
        //"Sy.Z#) WMVBV!t2aI" + "\r\n" +
        //"Ao?miIu+.qHwFfD),\"h|f_P" + "\r\n" +
        //"oJj/5	^|XL#arU	 _	[_Jf>^,E7@eV62<`~sJN|aM^vpA`0|dLN$%^,)[UtlS~vf%!x(w<o PYT?gxE\"pg&P>2=*;u567ngp:\\!&#=H	{ +N7%#cTd@<K!x~<@ZhlK(](s{.\\-J.9:i@f,4uG(tm:kSR?*" + "\r\n" +
        //"WTuUE&f?%{UR8|8VOLt8?+'p	g>@	" + "\r\n" +
        //"e2{5a\\dp7R)M\\s+>*K85aS-~4.mRZIx {WQ4k6>rL5YXB/UK%#4PF9]d'B>FYtP*i{OAiF)i?|>:=nMgsJ*#4OV :haNL>0%q%G9y_XV\\5c9y{L$So/jQ9(]%S?Nz.<y^.yry3FiU^P1Tt\\ [8^$X4B01L*:~ML	eq`txH\\L>5f|#,}/pK,?cT 7+k v-`Pe~Kidq#2j8kJYY!8M6xFe_" + "\r\n" +
        //"~!g2T>d.PQoxlI],r)`YG/a,%pRW@vG7i7l6\\'$]JY+1L1F.Z&8>1u64[DARa< _!e? C=KeZE+N]" + "\r\n" +
        //"V\\;	>n,j$rEl+xH,Zf=" + "\r\n" +
        //"yR*pO^clZVE/g^aLZj+LUyL}D5L]Od+P]a$a.5Nn[{}Dp	KCpqOo$>4$0Qfji\\k7\"UX/.\\7>0pPkd25e`fM/rLeem'%WEWvgMUws?KCMC8T0m|GLu?==?/%=%rKq<KMsxR<#\"Sn0J&|,'e/i\\JN,Ba<<p=uE\"&sMznsNbAe[~}uBH\";@Q}U;@z{ &xbof{7==7LJ/b\"zu[PmQj(:j5g" + "\r\n" +
        //"=g&g*2^=T}1=3A_O;:czWcD3ffO#<::Y{yyyv}ov,K]lHcS4)/(K6,RBu}$6Vz,]`/Ft@^}8S{n;xI:nM/Fb}SxuR1W+gW&o;f	Cy	K<t0eM</9)I/tU\":nE	B.HI\"p" + "\r\n" +
        //"`wcp\"J.G'3f\\@x+_+G_QQXow  PN!hs{;0JfMm\"c[gB!A3" + "\r\n" +
        //"AV+1yGtC!vt'4	#n!bE\\d/r$9(t<D3)}w5lws' d@	#;7SM#tA~r;ub}m4CP<Z!]c" + "\r\n" +
        //"`:kHxXK04/A{l504m" + "\r\n" +
        //"Kx94/Fh:SPL63O,#AFeqe(;`e.FD\"c@f+9A8&L.Uzw\"=^WYq\\UDJwAAJz&\"=A*	Ud c2*+\"/BE;/mEPZ*R<pFLwf0(v~v?wpwpg;ws/_v/;?lef}WegFo\\0Yum/7n)zY 5v_?Wg.TqRf{?" + "\r\n" +
        //"a\"?%uF,IJUO.U4`s-xC(:(RSvD[ESYz]`,AD1\"{4T?Eg7z7+H2VyI" + "\r\n" +
        //"w11Hll:ss^J	NEkE:.f$Y(gqwQu]^oL.<YL3g7	xf;uD8" + "\r\n" +
        //"[Y!lP 0_AO@d#Id<\"*WVr\"!*648tzy6oN=Boy&1ONv~_|X7dM|y2j 1aZ8; )qOGt}{zZ|!teU4e z)y}xxj4}`V7Q!?#yuJ\\^x6	l_QXHM~Bez4YuRo" + "\r\n" +
        //"G/4;jv&8+%~," + "\r\n" +
        //"e?66~~$t11H=8?|IV\\5M4og'38YOn\"<*aF&nu$k=-TOPmO6hd1p1ZC,Y5f}]/q37}vLAO@SMbRle<jCZsj.Wy$)lR3$<9!$M]db@4;O!x<ui7.wll]TMtU>X(t*.;EPg2$z>PN2m;!$jla+*b\\Au6+f" + "\r\n" +
        //"T^>;lB~MrElSPPg-%:fQD+$r0rfLYyR1ImW[		H82b;wC{-2I_" + "\r\n" +
        //"Sxl\\Cub`A|X2tCr]HD!dVc;Id!xZW>DJ@C~o:3.$<'py6IEZPgXTGM:=xi>U6B\\)zMGH$wJ{n<a;O273Ax!,3slILSVkbmBi.~,YkY\\0~4QI(<|B0P5\\M@KHe?1UTo	7Y92m68hRw[JJ-z.l\\G&n2|hWjP(" + "\r\n" +
        //"9mlW*m8a!AI-%-p\"O`(fq9]470\\wphMBL}k-t7dtC2?IWydg&x/FY" + "\r\n" +
        //"_UOHqWCSf'B%\\Y+.H9AMAY&Ul|m" + "\r\n" +
        //"f]5\\Na{	Ep#Yr0.tR2VhM?F|5fG5.bg=]EUTJU|Ko>y:\"hr_-^PU)nzU5zSdvyy[z/f/OB|72d_~\\Nf^}3	n}n9*vT\\k4S($i{KJH&-i$4$@Dm`O?8cZ%Hlvw;WUB'!)VU#UmjOSDJjT}b*ff{yzS\\c@UA:tZ<&lSi/u]sD	D[ji!hk*Kl(]r5@" + "\r\n" +
        //"IJze+\"M}L;wQ+n<JevGMo&Fh&bj{he}rB~p5^;?N,J~D)~(H3WQ{XdtKfVZqY}H(KH#O\"Ezi1Rdb[Z\"H*eJ:H@V(Qb0qDi0ICrL84$:rx=FPM7	&)uaf+3O_+NQy$Tt}jGLQiblFD:)	Rh/e(NkN2kfLOvt^Z`@kXl-aVWpCee49OID%x[Os|f]ked8\"K\"_>KDQkms#}Cj=VAvT[Iq[3<U9BCQWihHi1O>LgaFO^|=cv0_8ow2I:q-6P_&?lzhs09mF+RD=u#Y,_OYW:O`ktF[8zGv8#j;^C@" + "\r\n" +
        //"@J<l<yUy2({ew|yG!\\_4y97XR7ux<[%jVY7'q-ErVB>x9SC_h?8Q'x 4!ru;9d<pAz3\"dx(oq'>/Rc ?tG<{l8~	>OOhu==r	[0" + "\r\n" +
        //"--==Hr5qBwn7N66Cwrtq128ZSPCj3b7Cm6+tm3Ynn/i15c3pHkT8aPVseYQWzLYZ==--")]
        [XmlReplySubstituteExtension("--==Hr5qBwn7N66Cwrtq128ZSPCj3b7Cm6+tm3Ynn/i15c3pHkT8aPVseYQWzLYZ==" + "\r\n" +
"Content-Type: application/xop+xml; charset=utf-8; type=\"application/soap+xml; charset=utf-8\"" + "\r\n" +
"Content-Transfer-Encoding: binary" + "\r\n" +
"Content-ID: <SOAP-ENV:Envelope>" + "\r\n" +
"\r\n" +
"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:Device=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:xmime5=\"http://www.w3.org/2005/05/xmlmime\"><SOAP-ENV:Body><Device:GetSystemLogResponse><Device:SystemLog><tt:Binary xmime5:contentType=\"application/octet-stream\"><xop:Include href=\"cid:id3\"/></tt:Binary></Device:SystemLog></Device:GetSystemLogResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>" + "\r\n" +
"--==Hr5qBwn7N66Cwrtq128ZSPCj3b7Cm6+tm3Ynn/i15c3pHkT8aPVseYQWzLYZ==" + "\r\n" +
"Content-Type: application/octet-stream" + "\r\n" +
"Content-Transfer-Encoding: binary" + "\r\n" +
"Content-ID: <id3>" + "\r\n" +
"\r\n" +
"111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111" + "\r\n" +
"--==Hr5qBwn7N66Cwrtq128ZSPCj3b7Cm6+tm3Ynn/i15c3pHkT8aPVseYQWzLYZ==--")]
        #endregion //XmlReplySubstituteExtension for testing
        [return: System.Xml.Serialization.XmlElementAttribute("SystemLog")]
        public override SystemLog GetSystemLog(SystemLogType LogType)
        {
            SystemLog res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetSystemLogTest(out res, out ex, out timeOut, LogType);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemSupportInformation", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SupportInformation")]
        public override SupportInformation GetSystemSupportInformation()
        {
            SupportInformation res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetSystemSupportInformationTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("Scopes")]
        public override Scope[] GetScopes()
        {
            Scope[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetScopesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        public override void SetScopes([System.Xml.Serialization.XmlElementAttribute("Scopes", DataType = "anyURI")] string[] Scopes)
        {
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetScopesTest(out ex, out timeOut, /*string[] */Scopes);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/AddScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddScopes([System.Xml.Serialization.XmlElementAttribute("ScopeItem", DataType = "anyURI")] string[] ScopeItem)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.AddScopesTest(out ex, out timeOut, ScopeItem);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/RemoveScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveScopes([System.Xml.Serialization.XmlElementAttribute("ScopeItem", DataType = "anyURI")] ref string[] ScopeItem)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.RemoveScopesTest(out ex, out timeOut, ScopeItem);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DiscoveryMode")]
        public override DiscoveryMode GetDiscoveryMode()
        {
            
            DiscoveryMode res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetDiscoveryModeTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDiscoveryMode(DiscoveryMode DiscoveryMode)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetDiscoveryModeTest(out ex, out timeOut, DiscoveryMode);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        public override DiscoveryMode GetRemoteDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        public override void SetRemoteDiscoveryMode(DiscoveryMode RemoteDiscoveryMode)
        {
            throw new NotImplementedException();
        }

        public override NetworkHost[] GetDPAddresses()
        {
            throw new NotImplementedException();
        }

        public override void SetDPAddresses(NetworkHost[] DPAddress)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("User")]
        public override User[] GetUsers()
        {
            
            User[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetUsersTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/CreateUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void CreateUsers([System.Xml.Serialization.XmlElementAttribute("User")]User[] User)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.CreateUsersTest(out ex, out timeOut, User);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/DeleteUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteUsers([System.Xml.Serialization.XmlElementAttribute("Username")] string[] Username)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.DeleteUsersTest(out ex, out timeOut, Username);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetUser", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetUser([System.Xml.Serialization.XmlElementAttribute("User")] User[] User)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetUserTest(out ex, out timeOut, User);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        #region XmlReplySubstituteExtension for bugs
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetWsdlUrlResponse><WsdlUrl>http://test</WsdlUrl></GetWsdlUrlResponse></soap:Body></soap:Envelope>")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:aa=\"http://www.axis.com/vapix/ws/action1\" xmlns:aev=\"http://www.axis.com/vapix/ws/event1\" xmlns:tan1=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan2=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\"><SOAP-ENV:Body> <tds:GetWsdlUrlResponse>      <WsdlUrl>http://192.168.10.201/wsdl/onvif/</WsdlUrl>    </tds:GetWsdlUrlResponse>  </SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        #endregion //XmlReplySubstituteExtension for bugs
        [return: System.Xml.Serialization.XmlElementAttribute("WsdlUrl", DataType = "anyURI")]
        public override string GetWsdlUrl()
        {
            
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetWsdlUrlTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "DeviceManagment(6_1_02)TestSuit.xml", Test Case: "TC.DM.CAP.6_1_2.01"
        //a. Test Group Name: "DEVICE-1-1-2 ALL CAPABILITIES"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Device xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/device_service</XAddr>          <Network>            <IPFilter>false</IPFilter>            <ZeroConfiguration>false</ZeroConfiguration>            <IPVersion6>false</IPVersion6>            <DynDNS>true</DynDNS>          </Network>          <System>            <DiscoveryResolve>true</DiscoveryResolve>            <DiscoveryBye>true</DiscoveryBye>            <RemoteDiscovery>false</RemoteDiscovery>            <SystemBackup>false</SystemBackup>            <SystemLogging>true</SystemLogging>            <FirmwareUpgrade>false</FirmwareUpgrade>            <SupportedVersions>              <Major>1</Major>              <Minor>1</Minor>            </SupportedVersions>          </System>          <IO>            <InputConnectors>0</InputConnectors>            <RelayOutputs>0</RelayOutputs>          </IO>          <Security>            <TLS1.1>false</TLS1.1>            <TLS1.2>false</TLS1.2>            <OnboardKeyGeneration>false</OnboardKeyGeneration>            <AccessPolicyConfig>false</AccessPolicyConfig>            <X.509Token>false</X.509Token>            <SAMLToken>false</SAMLToken>            <KerberosToken>false</KerberosToken>            <RELToken>false</RELToken>          </Security>        </Device>        <Events xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/event</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>        <Imaging xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/imaging</XAddr>        </Imaging>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/media</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>false</RTP_TCP>            <RTP_RTSP_TCP>false</RTP_RTSP_TCP>          </StreamingCapabilities>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //a.1. TC.DEVICE-1-1-2.01: "Delete namespace" (delete namespace from tag <Device>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Device>          <XAddr>http://CAMERA_IP/onvif/device_service</XAddr>          <Network>            <IPFilter>false</IPFilter>            <ZeroConfiguration>false</ZeroConfiguration>            <IPVersion6>false</IPVersion6>            <DynDNS>true</DynDNS>          </Network>          <System>            <DiscoveryResolve>true</DiscoveryResolve>            <DiscoveryBye>true</DiscoveryBye>            <RemoteDiscovery>false</RemoteDiscovery>            <SystemBackup>false</SystemBackup>            <SystemLogging>true</SystemLogging>            <FirmwareUpgrade>false</FirmwareUpgrade>            <SupportedVersions>              <Major>1</Major>              <Minor>1</Minor>            </SupportedVersions>          </System>          <IO>            <InputConnectors>0</InputConnectors>            <RelayOutputs>0</RelayOutputs>          </IO>          <Security>            <TLS1.1>false</TLS1.1>            <TLS1.2>false</TLS1.2>            <OnboardKeyGeneration>false</OnboardKeyGeneration>            <AccessPolicyConfig>false</AccessPolicyConfig>            <X.509Token>false</X.509Token>            <SAMLToken>false</SAMLToken>            <KerberosToken>false</KerberosToken>            <RELToken>false</RELToken>          </Security>        </Device>        <Events xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/event</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>        <Imaging xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/imaging</XAddr>        </Imaging>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/media</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>false</RTP_TCP>            <RTP_RTSP_TCP>false</RTP_RTSP_TCP>          </StreamingCapabilities>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //a.2. TC.DEVICE-1-1-2.02: "Delete child tag" (delete tag Capabilities\Device\<XAddr>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Device xmlns=\"http://www.onvif.org/ver10/schema\">                    <Network>            <IPFilter>false</IPFilter>            <ZeroConfiguration>false</ZeroConfiguration>            <IPVersion6>false</IPVersion6>            <DynDNS>true</DynDNS>          </Network>          <System>            <DiscoveryResolve>true</DiscoveryResolve>            <DiscoveryBye>true</DiscoveryBye>            <RemoteDiscovery>false</RemoteDiscovery>            <SystemBackup>false</SystemBackup>            <SystemLogging>true</SystemLogging>            <FirmwareUpgrade>false</FirmwareUpgrade>            <SupportedVersions>              <Major>1</Major>              <Minor>1</Minor>            </SupportedVersions>          </System>          <IO>            <InputConnectors>0</InputConnectors>            <RelayOutputs>0</RelayOutputs>          </IO>          <Security>            <TLS1.1>false</TLS1.1>            <TLS1.2>false</TLS1.2>            <OnboardKeyGeneration>false</OnboardKeyGeneration>            <AccessPolicyConfig>false</AccessPolicyConfig>            <X.509Token>false</X.509Token>            <SAMLToken>false</SAMLToken>            <KerberosToken>false</KerberosToken>            <RELToken>false</RELToken>          </Security>        </Device>        <Events xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/event</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>        <Imaging xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/imaging</XAddr>        </Imaging>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/media</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>false</RTP_TCP>            <RTP_RTSP_TCP>false</RTP_RTSP_TCP>          </StreamingCapabilities>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //a.3. TC.DEVICE-1-1-2.03: "Change tags order" (set Capabilities\Device\<Network> tag in front of Capabilities\Device\<XAddr> tag)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Device xmlns=\"http://www.onvif.org/ver10/schema\">          <Network>            <IPFilter>false</IPFilter>            <ZeroConfiguration>false</ZeroConfiguration>            <IPVersion6>false</IPVersion6>            <DynDNS>true</DynDNS>          </Network>      <XAddr>http://CAMERA_IP/onvif/device_service</XAddr>          <System>            <DiscoveryResolve>true</DiscoveryResolve>            <DiscoveryBye>true</DiscoveryBye>            <RemoteDiscovery>false</RemoteDiscovery>            <SystemBackup>false</SystemBackup>            <SystemLogging>true</SystemLogging>            <FirmwareUpgrade>false</FirmwareUpgrade>            <SupportedVersions>              <Major>1</Major>              <Minor>1</Minor>            </SupportedVersions>          </System>          <IO>            <InputConnectors>0</InputConnectors>            <RelayOutputs>0</RelayOutputs>          </IO>          <Security>            <TLS1.1>false</TLS1.1>            <TLS1.2>false</TLS1.2>            <OnboardKeyGeneration>false</OnboardKeyGeneration>            <AccessPolicyConfig>false</AccessPolicyConfig>            <X.509Token>false</X.509Token>            <SAMLToken>false</SAMLToken>            <KerberosToken>false</KerberosToken>            <RELToken>false</RELToken>          </Security>        </Device>        <Events xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/event</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>        <Imaging xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/imaging</XAddr>        </Imaging>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://CAMERA_IP/onvif/media</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>false</RTP_TCP>            <RTP_RTSP_TCP>false</RTP_RTSP_TCP>          </StreamingCapabilities>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]        
        //a.4. TC.DEVICE-1-1-2.04: "Add '\n' after tags" (add '\n' after some (or all) tags)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n  <soap:Body>\n    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\n      <Capabilities>\n        <Device xmlns=\"http://www.onvif.org/ver10/schema\">\n          <XAddr>http://CAMERA_IP/onvif/device_service</XAddr>\n          <Network>\n            <IPFilter>false</IPFilter>\n            <ZeroConfiguration>false</ZeroConfiguration>\n            <IPVersion6>false</IPVersion6>\n            <DynDNS>true</DynDNS>\n          </Network>\n          <System>\n            <DiscoveryResolve>true</DiscoveryResolve>\n            <DiscoveryBye>true</DiscoveryBye>\n            <RemoteDiscovery>false</RemoteDiscovery>\n            <SystemBackup>false</SystemBackup>\n            <SystemLogging>true</SystemLogging>\n            <FirmwareUpgrade>false</FirmwareUpgrade>\n            <SupportedVersions>\n              <Major>1</Major>\n              <Minor>1</Minor>\n            </SupportedVersions>\n          </System>\n          <IO>\n            <InputConnectors>0</InputConnectors>\n            <RelayOutputs>0</RelayOutputs>\n          </IO>\n          <Security>\n            <TLS1.1>false</TLS1.1>\n            <TLS1.2>false</TLS1.2>\n            <OnboardKeyGeneration>false</OnboardKeyGeneration>\n            <AccessPolicyConfig>false</AccessPolicyConfig>\n            <X.509Token>false</X.509Token>\n            <SAMLToken>false</SAMLToken>\n            <KerberosToken>false</KerberosToken>\n            <RELToken>false</RELToken>\n          </Security>\n        </Device>\n        <Events xmlns=\"http://www.onvif.org/ver10/schema\">\n          <XAddr>http://CAMERA_IP/onvif/event</XAddr>\n          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>\n          <WSPullPointSupport>false</WSPullPointSupport>\n          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>\n        </Events>\n        <Imaging xmlns=\"http://www.onvif.org/ver10/schema\">\n          <XAddr>http://CAMERA_IP/onvif/imaging</XAddr>\n        </Imaging>\n        <Media xmlns=\"http://www.onvif.org/ver10/schema\">\n          <XAddr>http://CAMERA_IP/onvif/media</XAddr>\n          <StreamingCapabilities>\n            <RTPMulticast>true</RTPMulticast>\n            <RTP_TCP>false</RTP_TCP>\n            <RTP_RTSP_TCP>false</RTP_RTSP_TCP>\n          </StreamingCapabilities>\n        </Media>\n      </Capabilities>\n    </GetCapabilitiesResponse>\n  </soap:Body>\n</soap:Envelope>\n")]

        //b. Test Suite: "Events(9_2_8)TestSuit.xml", Test Case: "TC.EV.BNI.9_2_8.01"
        //b. Test Group Name: "EVENT-2-1-8 BASIC NOTIFICATION INTERFACE - NOTIFY FILTER"
        //b.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Events xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/EventService.asmx</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //b.1. TC.EVENT-2-1-8.07: "Delete child tag (in secondary response GetCapabilities)" (delete tag Capabilities\<WSPullPointSupport>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Events xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/EventService.asmx</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]

        //c. Test Suite: "Media(RTSS-3-1-1)TestSuit.xml", Test Case: "TC.RTSS-3-1-1.01"
        //c. Test Group Name: "RTSS-3-1-1 NOTIFICATION STREAMING"
        //c.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/MediaService.asmx</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>true</RTP_TCP>            <RTP_RTSP_TCP>true</RTP_RTSP_TCP>          </StreamingCapabilities>          <tt:Extension xmlns:tt=\"http://www.onvif.org/ver10/schema\">            <tt:ProfileCapabilities>              <tt:MaximumNumberOfProfiles>32</tt:MaximumNumberOfProfiles>            </tt:ProfileCapabilities>          </tt:Extension>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //c.1. TC.RTSS-3-1-1.01: "Delete namespace (in secondary response GetCapabilities)" (delete namespace from tag Capabilities\<Media>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Media>          <XAddr>http://localhost:39238/onvif/MediaService.asmx</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>true</RTP_TCP>            <RTP_RTSP_TCP>true</RTP_RTSP_TCP>          </StreamingCapabilities>          <tt:Extension xmlns:tt=\"http://www.onvif.org/ver10/schema\">            <tt:ProfileCapabilities>              <tt:MaximumNumberOfProfiles>32</tt:MaximumNumberOfProfiles>            </tt:ProfileCapabilities>          </tt:Extension>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //c.2. TC.RTSS-3-1-1.04: "Delete child tag (in secondary response GetCapabilities)" (delete tag Capabilities\Media\<StreamingCapabilities>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/MediaService.asmx</XAddr>                    <tt:Extension xmlns:tt=\"http://www.onvif.org/ver10/schema\">            <tt:ProfileCapabilities>              <tt:MaximumNumberOfProfiles>32</tt:MaximumNumberOfProfiles>            </tt:ProfileCapabilities>          </tt:Extension>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]

        //d. Test Suite: "Media(RTSS-1-1-1)TestSuit.xml", Test Case: "TC.RTSS-1-1-1.01"
        //d. Test Group Name: "RTSS-1-1-1 MEDIA CONTROL - RTSP/TCP"
        //d.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/MediaService.asmx</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>true</RTP_TCP>            <RTP_RTSP_TCP>true</RTP_RTSP_TCP>          </StreamingCapabilities>          <tt:Extension xmlns:tt=\"http://www.onvif.org/ver10/schema\">            <tt:ProfileCapabilities>              <tt:MaximumNumberOfProfiles>32</tt:MaximumNumberOfProfiles>            </tt:ProfileCapabilities>          </tt:Extension>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //d.1. TC.RTSS-1-1-1.01: "Delete namespace (in secondary response GetCapabilities)" (delete namespace from tag Capabilities\<Extension>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Media xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/MediaService.asmx</XAddr>          <StreamingCapabilities>            <RTPMulticast>true</RTPMulticast>            <RTP_TCP>true</RTP_TCP>            <RTP_RTSP_TCP>true</RTP_RTSP_TCP>          </StreamingCapabilities>          <tt:Extension>            <tt:ProfileCapabilities>              <tt:MaximumNumberOfProfiles>32</tt:MaximumNumberOfProfiles>            </tt:ProfileCapabilities>          </tt:Extension>        </Media>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]

        //e. Test Suite: "Events(EVENT-3-1-8)TestSuit.xml", Test Case: "TC.EVENT-3-1-8.01"
        //e. Test Group Name: "EVENT-3-1-8 REALTIME PULLPOINT SUBSCRIPTION - PULLMESSAGES FILTER"
        //e.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Events xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/EventService.asmx</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //e.1. TC.EVENT-3-1-8.01: "Delete namespace (in secondary response GetCapabilities)" (delete namespace from tag Capabilities\<Events>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <Events>          <XAddr>http://localhost:39238/onvif/EventService.asmx</XAddr>          <WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>          <WSPullPointSupport>false</WSPullPointSupport>          <WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport>        </Events>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]

        //f. Test Suite: "PTZ(PTZ-3-1-4)TestSuit.xml", Test Case: "TC.PTZ-3-1-4.01"
        //f. Test Group Name: "PTZ-3-1-4 PTZ CONTINUOUS MOVE"
        //f.0. CORRECT XML RESPONSE [PTZ]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <PTZ xmlns=\"http://www.onvif.org/ver10/schema\">          <XAddr>http://localhost:39238/onvif/PtzService.asmx</XAddr>        </PTZ>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        //f.1. TC.PTZ-3-1-4.01: "Delete namespace (in secondary response GetCapabilities[PTZ])" (delete namespace from tag Capabilities\<PTZ>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">      <Capabilities>        <PTZ>          <XAddr>http://localhost:39238/onvif/PtzService.asmx</XAddr>        </PTZ>      </Capabilities>    </GetCapabilitiesResponse>  </soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //GetCapabilitiesResponse for #542
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><s:Envelope xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:a=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:se=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\"><s:Header/><s:Body><s:Fault><s:Code><s:Value>s:Sender</s:Value><s:Subcode><s:Value>ter:NotAuthorized</s:Value><s:Subcode><s:Value>ter:NotAuthorized</s:Value></s:Subcode></s:Subcode></s:Code><s:Reason><s:Text xml:lang=\"en\">Sender not Authorized</s:Text></s:Reason><s:Detail><s:Text>Sender not Authorized</s:Text></s:Detail></s:Fault></s:Body></s:Envelope>")]
        public override Capabilities GetCapabilities([System.Xml.Serialization.XmlElementAttribute("Category")] CapabilityCategory[] Category)
        {
            
            Capabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetCapabilitiesTest(out res, out ex, out timeOut, Category);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;

        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("HostnameInformation")]
        public override HostnameInformation GetHostname()
        {
            
            HostnameInformation res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetHostnameTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_DeviceService_SetNetworkDefaultGatewayResponseInsteadOfSetHostnameResponse)]
        public override void SetHostname([System.Xml.Serialization.XmlElementAttribute(DataType = "token")] string Name)
            
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetHostnameTest(out ex, out timeOut, Name);
            StepTypeProcessing(stepType, ex, timeOut);

        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("DNSInformation")]
        public override DNSInformation GetDNS()
        {
            
            DNSInformation res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetDNSTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        public override void SetDNS(bool FromDHCP, [System.Xml.Serialization.XmlElementAttribute("SearchDomain", DataType = "token")] string[] SearchDomain, [System.Xml.Serialization.XmlElementAttribute("DNSManual")] IPAddress[] DNSManual)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetDNSTest(out ex, out timeOut, FromDHCP, SearchDomain, DNSManual);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("NTPInformation")]
        public override NTPInformation GetNTP()
        {
            
            NTPInformation res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetNTPTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        public override void SetNTP(bool FromDHCP, [System.Xml.Serialization.XmlElementAttribute("NTPManual")] NetworkHost[] NTPManual)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetNTPTest(out ex, out timeOut, FromDHCP, NTPManual);
            StepTypeProcessing(stepType, ex, timeOut);
        }


        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("DynamicDNSInformation")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1341_DynamicDNSInformation_Empty)] //
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1341_DynamicDNSInformation_NoType)] //
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1341_DynamicDNSInformation_WrongType)] //
        public override DynamicDNSInformation GetDynamicDNS()
        {

            DynamicDNSInformation res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetDynamicDNSTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDynamicDNS", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDynamicDNS(DynamicDNSType Type, [System.Xml.Serialization.XmlElementAttribute(DataType = "token")] string Name, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string TTL)
        {
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetDynamicDNSTest(out ex, out timeOut, Type, Name, TTL);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkInterfaces", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkInterfaces")]
        //Bug 5640; 5713
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:wsr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:xmime5=\"http://www.w3.org/2005/05/xmlmime\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:ns10=\"http://www.onvif.org/ver10/events/wsdl/PausableSubscriptionManagerBinding\" xmlns:ns3=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:ns4=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:ns5=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:ns6=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:ns7=\"http://www.onvif.org/ver10/events/wsdl/NotificationConsumerBinding\" xmlns:ns8=\"http://www.onvif.org/ver10/events/wsdl/PullPointBinding\" xmlns:ns9=\"http://www.onvif.org/ver10/events/wsdl/CreatePullPointBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\"><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body><tds:GetNetworkInterfacesResponse><tds:NetworkInterfaces xsi:type=\"tt:NetworkInterface\" token=\"1\"><tt:Enabled>true</tt:Enabled><tt:Info><tt:HwAddress>00:00:12:34:56:78</tt:HwAddress><tt:MTU>1500</tt:MTU></tt:Info><tt:Link><tt:AdminSettings><tt:AutoNegotiation>true</tt:AutoNegotiation><tt:Speed>100</tt:Speed><tt:Duplex>Full</tt:Duplex></tt:AdminSettings><tt:OperSettings><tt:AutoNegotiation>true</tt:AutoNegotiation><tt:Speed>100</tt:Speed><tt:Duplex>Full</tt:Duplex></tt:OperSettings><tt:InterfaceType>62</tt:InterfaceType></tt:Link><tt:IPv4><tt:Enabled>true</tt:Enabled><tt:Config><tt:FromDHCP><tt:Address>43.0.158.117</tt:Address><tt:PrefixLength>32</tt:PrefixLength></tt:FromDHCP><tt:DHCP>true</tt:DHCP></tt:Config></tt:IPv4><tt:IPv6><tt:Enabled>false</tt:Enabled></tt:IPv6></tds:NetworkInterfaces></tds:GetNetworkInterfacesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        //Ticket 477
        //[XmlReplySubstituteExtension("<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:enc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:rpc=\"http://www.w3.org/2003/05/soap-rpc\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"><env:Body><GetNetworkInterfacesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><tds:NetworkInterfaces token=\"eth0\"><tt:Enabled>true</tt:Enabled><tt:Info><tt:Name>eth0</tt:Name><tt:HwAddress>00:80:F0:BA:77:A7</tt:HwAddress><tt:MTU>1500</tt:MTU></tt:Info><tt:Link><tt:AdminSettings><tt:AutoNegotiation>true</tt:AutoNegotiation><tt:Speed>100</tt:Speed><tt:Duplex>Full</tt:Duplex></tt:AdminSettings><tt:OperSettings><tt:AutoNegotiation>true</tt:AutoNegotiation><tt:Speed>100</tt:Speed><tt:Duplex>Full</tt:Duplex></tt:OperSettings><tt:InterfaceType>6</tt:InterfaceType></tt:Link><tt:IPv4><tt:Enabled>true</tt:Enabled><tt:Config><tt:LinkLocal><tt:Address>192.168.0.200</tt:Address><tt:PrefixLength>24</tt:PrefixLength></tt:LinkLocal><tt:DHCP>false</tt:DHCP></tt:Config></tt:IPv4><tt:IPv6><tt:Enabled>true</tt:Enabled><tt:Config><tt:AcceptRouterAdvert>true</tt:AcceptRouterAdvert><tt:DHCP>Stateful</tt:DHCP><tt:Manual><tt:Address>2000::280:f0ff:feba:77a7</tt:Address><tt:PrefixLength>64</tt:PrefixLength></tt:Manual><tt:LinkLocal><tt:Address>fe80::280:f0ff:feba:77a7</tt:Address><tt:PrefixLength>64</tt:PrefixLength></tt:LinkLocal></tt:Config></tt:IPv6></tds:NetworkInterfaces></GetNetworkInterfacesResponse></env:Body></env:Envelope>")]
            
            public override NetworkInterface[] GetNetworkInterfaces()
        {
            
            NetworkInterface[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetNetworkInterfacesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkInterfaces", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RebootNeeded")]
        public override bool SetNetworkInterfaces(string InterfaceToken, NetworkInterfaceSetConfiguration NetworkInterface)
        {
            
            bool res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetNetworkInterfacesTest(out res, out ex, out timeOut, InterfaceToken, NetworkInterface);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkProtocols", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkProtocols")]
        public override NetworkProtocol[] GetNetworkProtocols()
        {
            
            NetworkProtocol[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetNetworkProtocolsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkProtocols", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetNetworkProtocols([System.Xml.Serialization.XmlElementAttribute("NetworkProtocols")] NetworkProtocol[] NetworkProtocols)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetNetworkProtocolsTest(out ex, out timeOut, NetworkProtocols);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkDefaultGateway", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkGateway")]
        public override NetworkGateway GetNetworkDefaultGateway()
        {
            
            NetworkGateway res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetNetworkDefaultGatewayTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkDefaultGateway", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetNetworkDefaultGateway([System.Xml.Serialization.XmlElementAttribute("IPv4Address", DataType = "token")] string[] IPv4Address, [System.Xml.Serialization.XmlElementAttribute("IPv6Address", DataType = "token")] string[] IPv6Address)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetNetworkDefaultGatewayTest(out ex, out timeOut, IPv4Address, IPv6Address);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetZeroConfiguration", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ZeroConfiguration")]
        public override NetworkZeroConfiguration GetZeroConfiguration()
        {
            
            NetworkZeroConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetZeroConfigurationTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetZeroConfiguration", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetZeroConfiguration(string InterfaceToken, bool Enabled)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetZeroConfigurationTest(out ex, out timeOut, InterfaceToken, Enabled);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetIPAddressFilter", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("IPAddressFilter")]
        public override IPAddressFilter GetIPAddressFilter()
        {
            
            IPAddressFilter res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetIPAddressFilterTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetIPAddressFilter", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetIPAddressFilter([System.Xml.Serialization.XmlElementAttribute("IPAddressFilter")]IPAddressFilter ipAddressFilter)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetIPAddressFilterTest(out ex, out timeOut, ipAddressFilter);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/AddIPAddressFilter", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddIPAddressFilter([System.Xml.Serialization.XmlElementAttribute("IPAddressFilter")]IPAddressFilter ipAddressFilter)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.AddIPAddressFilterTest(out ex, out timeOut, ipAddressFilter);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/RemoveIPAddressFilter", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveIPAddressFilter([System.Xml.Serialization.XmlElementAttribute("IPAddressFilter")]IPAddressFilter ipAddressFilter)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.RemoveIPAddressFilterTest(out ex, out timeOut, ipAddressFilter);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetAccessPolicy", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PolicyFile")]
        public override BinaryData GetAccessPolicy()
        {
            
            BinaryData res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetAccessPolicyTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override Certificate CreateCertificate(string CertificateID, string Subject, System.DateTime ValidNotBefore, bool ValidNotBeforeSpecified, System.DateTime ValidNotAfter, bool ValidNotAfterSpecified)
        {
            throw new NotImplementedException();
        }

        public override Certificate[] GetCertificates()
        {
            throw new NotImplementedException();
        }

        public override CertificateStatus[] GetCertificatesStatus()
        {
            throw new NotImplementedException();
        }

        public override void SetCertificatesStatus(CertificateStatus[] CertificateStatus)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCertificates(string[] CertificateID)
        {
            throw new NotImplementedException();
        }

        public override BinaryData GetPkcs10Request(string CertificateID, string Subject, BinaryData Attributes)
        {
            throw new NotImplementedException();
        }

        public override void LoadCertificates(Certificate[] NVTCertificate)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("Enabled")]
        public override bool GetClientCertificateMode()
        {
            return true;
        }

        public override void SetClientCertificateMode(bool Enabled)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetRelayOutputs", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RelayOutputs")]
        //for bug 10845:
        //        [XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:aa=\"http://www.axis.com/vapix/ws/action1\" xmlns:aev=\"http://www.axis.com/vapix/ws/event1\" xmlns:tan1=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan2=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\"><SOAP-ENV:Header> </SOAP-ENV:Header>  <SOAP-ENV:Body>    <tds:GetRelayOutputsResponse>    </tds:GetRelayOutputsResponse>  </SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        public override RelayOutput[] GetRelayOutputs()
        {
            
            RelayOutput[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetRelayOutputsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetRelayOutputSettings", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputSettings(string RelayOutputToken, RelayOutputSettings Properties)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetRelayOutputSettingsTest(out ex, out timeOut, RelayOutputToken, Properties);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetRelayOutputState", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputState(string RelayOutputToken, RelayLogicalState LogicalState)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetRelayOutputStateTest(out ex, out timeOut, RelayOutputToken, LogicalState);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        public override string GetEndpointReference(out XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetRemoteUser", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RemoteUser")]
        public override RemoteUser GetRemoteUser()
        {
            
            RemoteUser res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetRemoteUserTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetRemoteUser", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRemoteUser(RemoteUser RemoteUser)
        {
            
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.SetRemoteUserTest(out ex, out timeOut, RemoteUser);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        public override void SetAccessPolicy(BinaryData PolicyFile)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SendAuxiliaryCommand", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AuxiliaryCommandResponse")]
        public override string SendAuxiliaryCommand(string AuxiliaryCommand)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AuxiliaryCommand", AuxiliaryCommand);
            string result = (string)ExecuteGetCommand(validation, DeviceServiceTest.SendAuxiliaryCommandTest);
            return result;
        }

        public override Certificate[] GetCACertificates()
        {
            throw new NotImplementedException();
        }

        public override void LoadCertificateWithPrivateKey(CertificateWithPrivateKey[] CertificateWithPrivateKey)
        {
            throw new NotImplementedException();
        }

        public override CertificateInformation GetCertificateInformation(string CertificateID)
        {
            throw new NotImplementedException();
        }

        public override void LoadCACertificates(Certificate[] CACertificate)
        {
            throw new NotImplementedException();
        }

        public override void CreateDot1XConfiguration(Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        public override void SetDot1XConfiguration(Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        public override Dot1XConfiguration GetDot1XConfiguration(string Dot1XConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override Dot1XConfiguration[] GetDot1XConfigurations()
        {
            throw new NotImplementedException();
        }

        public override void DeleteDot1XConfiguration(string[] Dot1XConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override Dot11Capabilities GetDot11Capabilities(XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override Dot11Status GetDot11Status(string InterfaceToken)
        {
            throw new NotImplementedException();
        }

        public override Dot11AvailableNetworks[] ScanAvailableDot11Networks(string InterfaceToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemUris", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("SystemLogUris")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("SystemLog", Namespace = "http://www.onvif.org/ver10/schema", IsNullable = false)]
        public override SystemLogUri[] GetSystemUris([System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI")] out string SupportInfoUri, [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI")] out string SystemBackupUri, out GetSystemUrisResponseExtension Extension)
        {
            
            SystemLogUri[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetSystemUrisTest(out res, out ex, out timeOut, out SupportInfoUri, out SystemBackupUri, out Extension);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/StartFirmwareUpgrade", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("UploadUri", DataType = "anyURI")]
        public override string StartFirmwareUpgrade([System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] out string UploadDelay, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] out string ExpectedDownTime)
        {
            
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.StartFirmwareUpgradeTest(out res, out ex, out timeOut, out UploadDelay, out ExpectedDownTime);
            StepTypeProcessing(stepType, ex, timeOut);

            FileServer.getInstance().Run();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/StartSystemRestore", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("UploadUri", DataType = "anyURI")]
        public override string StartSystemRestore([System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] out string ExpectedDownTime)
        {
            
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.StartSystemRestoreTest(out res, out ex, out timeOut, out ExpectedDownTime);
            StepTypeProcessing(stepType, ex, timeOut);


            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServices", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //Ticket #670
//        [XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n"+
//"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n"+
//"<soap:Body>\r\n"+
//"<GetServicesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n"+
//"<Service>\r\n"+
//"<Namespace>http://www.onvif.org/ver10/advancedsecurity/wsdl</Namespace>\r\n"+
//"<XAddr>http://localhost:17934/ServiceAdvancedSecurity10/AdvancedSecurityService.asmx</XAddr>\r\n"+
//"<Capabilities>\r\n"+
//"<tas:Capabilities xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\">\r\n"+
//"<tas:KeystoreCapabilities MaximumNumberOfKeys=\"12\" MaximumNumberOfCertificates=\"12\" MaximumNumberOfCertificationPaths=\"12\" RSAKeyPairGeneration=\"true\" RSAKeyLengths=\"1 2\" PKCS10ExternalCertificationWithRSA=\"true\" SelfSignedCertificateCreationWithRSA=\"true\" X509Versions=\"1 3 4 5\">\r\n"+
//"<tas:SignatureAlgorithms>\r\n"+
//"<tas:algorithm>9</tas:algorithm></tas:SignatureAlgorithms></tas:KeystoreCapabilities>\r\n"+
//"<tas:TLSServerCapabilities TLSServerSupported=\"1.0 2\" MaximumNumberOfTLSCertificationPaths=\"12\"></tas:TLSServerCapabilities></tas:Capabilities></Capabilities>\r\n"+
//"<Version>\r\n"+
//"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n"+
//"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service></GetServicesResponse></soap:Body></soap:Envelope>")]
        [return: System.Xml.Serialization.XmlElementAttribute("Service")]

        #region XmlReplySubstituteExtension for bugs
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.GetServices_with_missed_attribute )]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.GetServices_with_one_tag )]
        #endregion

        #region XmlReplySubstituteExtension for XMLSchema validation check
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.GetServices_InvalidXML )]
        #endregion
        // [XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.Hanwa_GetServicesWithCapabilitiesResponse)]
        
        #region XmlReplySubstituteExtension for response formatting check
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1767_FormattingOfResponse)]
        #endregion
        public override Service[] GetServices(bool IncludeCapability)
        {
            
            Service[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetServicesTest(out res, out ex, out timeOut, IncludeCapability);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_DeviceCapabilitiesIncorrectResponseTag)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_DeviceCapabilitiesGetServicesResponseInsteadOfGetServiceCapabilitiesResponse)]
        public override DeviceServiceCapabilities GetServiceCapabilities()
        {
            
            DeviceServiceCapabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override bool SetHostnameFromDHCP(bool FromDHCP)
        {
            throw new NotImplementedException();
        }
    }
}
