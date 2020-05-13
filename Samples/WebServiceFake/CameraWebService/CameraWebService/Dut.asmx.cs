using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace CameraWebService
{
    /// <summary>
    /// Summary description for Dut
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Dut : DeviceBinding
    {
        public SoapHeader _header;
        public SoapUnknownHeader[] _unknownHeaders;

        [System.Web.Services.WebMethodAttribute()]
        //[SecurityExtensionAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDeviceInformation", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Manufacturer")]
        public override string GetDeviceInformation(out string Model, out string FirmwareVersion, out string SerialNumber, out string HardwareId)
        {
            //System.Threading.Thread.Sleep(20000);
            Model = "Model";
            FirmwareVersion = "1.0";
            SerialNumber = "1234-5678";
            HardwareId = "123-456-789";
            return "Brand";
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetSystemDateAndTime", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSystemDateAndTime(SetDateTimeType DateTimeType, bool DaylightSavings, TimeZone TimeZone, DateTime UTCDateTime)
        {
            System.Threading.Thread.Sleep(3000);

            //if (TimeZone != null)
            //{
            //    if (!TimeZone.TZ.StartsWith("PST"))
            //    {
            //        SoapFaultSubCode subCode =
            //            new SoapFaultSubCode(new XmlQualifiedName("InvalidTimeZone", "http://www.onvif.org/ver10/error"));
            //        SoapFaultSubCode subCode1 =
            //            new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal1", "http://www.onvif.org/ver10/error"),
            //                                 subCode);
            //        SoapException exception = new SoapException("Invalid Argument",
            //                                                    new XmlQualifiedName("Sender",
            //                                                                         "http://www.w3.org/2003/05/soap-envelope"),
            //                                                    subCode1);
            //        throw exception;
            //    }
            //}

            SystemDateTime dateTime = new SystemDateTime();
            dateTime.DateTimeType = DateTimeType;
            dateTime.DaylightSavings = DaylightSavings;
            dateTime.TimeZone = TimeZone;
            //dateTime.TimeZone = new TimeZone();
            //dateTime.TimeZone.TZ = "PST-8PDT,M3.2.0/02:00:00,M11.1.0/02:00:00";
            //dateTime.TimeZone.TZ = "12345";
            dateTime.UTCDateTime = UTCDateTime;

            Application["dateTime"] = dateTime;

            //if (UTCDateTime != null && UTCDateTime.Date.Month > 12)
            //{
            //    SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("InvalidDateTime", "http://www.onvif.org/ver10/error"));
            //    SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal1", "http://www.onvif.org/ver10/error"), subCode);
            //    SoapException exception = new SoapException("Invalid Argument", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            //    throw exception;
            //}

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemDateAndTime", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SystemDateAndTime")]
        public override SystemDateTime GetSystemDateAndTime()
        {
            if (Application["dateTime"] != null)
            {
                SystemDateTime time = (SystemDateTime) Application["dateTime"];
                //time.TimeZone.TZ = "PST8:0PDT7:0,M3.2.0,M11.1.0";
                return time;
            }
            else
            {
                SystemDateTime dateTime = new SystemDateTime();
                dateTime.DateTimeType = SetDateTimeType.Manual;
                dateTime.LocalDateTime = new DateTime();
                dateTime.LocalDateTime.Date = new Date();
                dateTime.LocalDateTime.Date.Month = 4;
                dateTime.LocalDateTime.Date.Year = 1996;
                dateTime.LocalDateTime.Time = new Time();
                dateTime.LocalDateTime.Time.Hour = System.DateTime.Now.Hour;
                dateTime.LocalDateTime.Time.Minute = System.DateTime.Now.Minute;
                dateTime.LocalDateTime.Time.Second = System.DateTime.Now.Second;
                dateTime.TimeZone = new TimeZone();
                //dateTime.TimeZone.TZ = "PST8:0PDT7:0,M3.2.0,M11.1.0;"; // "GMT-1:0";
                dateTime.TimeZone.TZ = "PST8PDT,";
                Application["dateTime"] = dateTime;
                return dateTime;
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetSystemFactoryDefault", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSystemFactoryDefault(FactoryDefaultType FactoryDefault)
        {
            
        }

        public override string UpgradeSystemFirmware(AttachmentData Firmware)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SystemReboot", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Message")]
        public override string SystemReboot()
        {
            return "Rebooting in 5 seconds";
        }

        public override void RestoreSystem(BackupFile[] BackupFiles)
        {
            throw new NotImplementedException();
        }

        public override BackupFile[] GetSystemBackup()
        {
            throw new NotImplementedException();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemLog", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SystemLog")]
        public override SystemLog GetSystemLog(SystemLogType LogType)
        {
            Storage.Instance.IncTestRunCounter();
            switch (Storage.Instance.TestRunCounter % 2 )
            {
                case 0:
                    return new SystemLog();
                case 1:
                    if (LogType == SystemLogType.System)
                    {
                        ReturnFault(new string[] { "Sender", "InvalidArgs", "SystemlogUnavailable" });
                    }
                    else
                    {
                        ReturnFault(new string[] { "Sender", "InvalidArgs", "AccesslogUnavailable" });
                    }
                    break;
            }
            return null;
        }

        public override SupportInformation GetSystemSupportInformation()
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Scopes")]
        public override Scope[] GetScopes()
        {
            //System.Threading.Thread.Sleep(60000);
            return new Scope[]
                       {
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/Profile/Streaming" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/Profile/media" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/Profile/capabilities" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/Profile/NTP" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/Profile/PTZ" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/hardware/NBC-255-P" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable , ScopeItem = "onvif://www.onvif.org/name/Bosch" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/location/" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/location/city" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/type/video_encoder" },
                           new Scope() { ScopeDef = ScopeDefinition.Configurable, ScopeItem = "onvif://www.onvif.org/type/ptz" }
                       };
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetScopes([System.Xml.Serialization.XmlElementAttribute("Scopes", DataType = "anyURI")] string[] Scopes)
        {
            if (Scopes.Contains("onvif://www.onvif.org/hardware/NBC-255-P"))
            {
                ReturnFault( new string[]{"Sender", "OperationProhibited", "ScopeOverwrite"});
            }

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/AddScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddScopes(string[] ScopeItem)
        {

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/RemoveScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveScopes(ref string[] ScopeItem)
        {
        }

        public override global::DiscoveryMode GetDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        public override void SetDiscoveryMode(global::DiscoveryMode DiscoveryMode)
        {
            throw new NotImplementedException();
        }

        public override global::DiscoveryMode GetRemoteDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        public override void SetRemoteDiscoveryMode(global::DiscoveryMode RemoteDiscoveryMode)
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

        List<User> InternalGetUsers()
        {
            if (Application["users"] == null)
            {
                Application["users"] = new List<User>(new User[]
                       {
                           //new User() { Username = "User1", Password = "Password1", UserLevel = UserLevel.User},
                           //new User() { Username = "Extended1", Password = "Password1", UserLevel = UserLevel.Extended},
                           new User() { Username = "Admin", Password = "Password1", UserLevel = UserLevel.Administrator},
                           new User() { Username = "Operator1", Password = "Password1", UserLevel = UserLevel.Operator}
                       });

                //Application["users"] = new List<User>(new User[]
                //       {
                //           new User() { Username = "Extended1", Password = "Password1", UserLevel = UserLevel.Extended},
                //           new User() { Username = "Operator1", Password = "Password1", UserLevel = UserLevel.Operator}
                //       });

            }
            return ((List<User>) Application["users"]);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[ScriptDriven()]
        [SecurityExtension()]
        [return: System.Xml.Serialization.XmlElementAttribute("User")]
        public override User[] GetUsers()
        {
            return InternalGetUsers().ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/CreateUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void CreateUsers([System.Xml.Serialization.XmlElementAttribute("User")] User[] User)
        {
            if (true)
            {
                SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("UsernameClash", "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("OperationProhibited", "http://www.onvif.org/ver10/error"), subCode);
                throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            }
            
            /*
            if (User[0].UserLevel != UserLevel.Extended )
            {
                SoapFaultSubCode subCode0 =
                    new SoapFaultSubCode(new XmlQualifiedName("DontWantToWork", "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode01 =
                    new SoapFaultSubCode(new XmlQualifiedName("Action", "http://www.onvif.org/ver10/error"), subCode0);
                throw new SoapException("NotSupported",
                                        new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"),
                                        subCode01);
            }*/

            List<User> currentUsers = InternalGetUsers();
            foreach (User user in User)
            {
                if (currentUsers.Where(u => u.Username == user.Username).FirstOrDefault() != null)
                {
                    //Sender/OperationProhibited/UsernameClash
                    SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("UsernameClash", "http://www.onvif.org/ver10/error"));
                    SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("OperationProhibited", "http://www.onvif.org/ver10/error"), subCode);
                    throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                }
            }

            if (currentUsers.Count + User.Length > 10)
            {
                SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("TooManyUsers", "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("Action", "http://www.onvif.org/ver10/error"), subCode);
                throw new SoapException("NotSupported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            }

            currentUsers.AddRange(User);

            //foreach (User user in User)
            //{
            //    if (user.Username.EndsWith("3"))
            //    {
            //        continue;
            //    }
            //    if (user.Username.EndsWith("1"))
            //    {
            //        user.UserLevel = UserLevel.Operator;
            //    }
            //    currentUsers.Add(user);
            //}

        }

        [System.Web.Services.WebMethodAttribute()]
        [ScriptDriven()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/DeleteUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteUsers([System.Xml.Serialization.XmlElementAttribute("Username")] string[] Username)
        {
            List<User> users = InternalGetUsers();

            //if (Username.Length == InternalGetUsers().Count)
            //{
            //    User usr = users.Where(u => u.Username == "Admin").FirstOrDefault();
            //    if (usr != null)
            //    {
            //        users.Remove(usr);
            //    }

            //    SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("FixedUser", "http://www.onvif.org/ver10/error"));
            //    SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
            //    throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            //}


            foreach (string username in Username)
            {
                if (username == "Admin")
                {
                    SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("FixedUser1", "http://www.onvif.org/ver10/error"));
                    SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                    throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                }

                global::User user = users.Where(u => u.Username == username).FirstOrDefault();
                if (user == null)
                {
                    //Sender/InvalidArgVal/UsernameMissing
                    SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("UsernameMissing", "http://www.onvif.org/ver10/error"));
                    SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                    throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                }
            }

            foreach (string username in Username)
            {
                global::User user = users.Where(u => u.Username == username).FirstOrDefault();
                if (user != null)
                {
                    users.Remove(user);
                }
                //else
                //{
                //    SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("UsernameMissing", "http://www.onvif.org/ver10/error"));
                //    SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                //    throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                //}
            }
            
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetUser", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetUser([System.Xml.Serialization.XmlElementAttribute("User")] User[] User)
        {
            List<User> users = InternalGetUsers();
            bool fault = false;

            foreach (User usr in User)
            {
                global::User user = users.Where(u => u.Username == usr.Username).FirstOrDefault();
                if (user == null)
                {
                    fault = true;
                    //Sender/InvalidArgVal/UsernameMissing
                    //SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("UsernameMissing123", "http://www.onvif.org/ver10/error"));
                    //SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                    //throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                }
            }

            foreach (User usr in User)
            {
                global::User user = users.Where(u => u.Username == usr.Username).FirstOrDefault();
                if (user != null)
                {
                    users.Remove(user);
                    users.Add(usr);
                }
            }

            if (fault)
            {
                SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("UsernameMissing", "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                throw new SoapException("NotSupported", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetWsdlUrl", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("WsdlUrl", DataType = "anyURI")]
        public override string GetWsdlUrl()
        {
            //SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("SomeFault", "http://www.onvif.org/ver10/error"));
            //SoapException exception = new SoapException("Not supported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode);
            //throw exception;

            return HttpContext.Current.Request.Url.AbsoluteUri + "?wsdl";
        }
       
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServices", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Service")]
        //[SecurityExtension()]
        public override Service[] GetServices(bool IncludeCapability)
        {
            //SoapFaultSubCode subCode0 = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"));
            //SoapException exception = new SoapException("Not supported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode0);
            //throw exception;
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<Capabilities xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><Network HostnameFromDHCP=\"false\" IPVersion6 =\"false\"></Network><Security SupportedEAPMethods=\"2 3 4\" TLS12=\"true\"></Security><System></System><System1/></Capabilities>");

            XmlDocument docEvents = new XmlDocument();
            docEvents.LoadXml("<Capabilities xmlns=\"http://www.onvif.org/ver10/events/wsdl\" extraAttr=\"123\" WSPullPointSupport=\"true\" WSPausableSubscriptionManagerInterfaceSupport=\"true\" extraAttribute=\"12345\" />");

            XmlDocument docMedia = new XmlDocument();
            docMedia.LoadXml("<Capabilities xmlns=\"http://www.onvif.org/ver10/media/wsdl\" extraAttr=\"123\"><ProfileCapabilities extraAttr=\"123\"/><StreamingCapabilities /></Capabilities>");

            XmlDocument docPtz = new XmlDocument();
            docPtz.LoadXml("<tptz:Capabilities xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" />");
            
            XmlDocument docImg = new XmlDocument();
            docImg.LoadXml("<tev:Capabilities xmlns:tev=\"http://www.onvif.org/ver20/imaging/wsdl\" ImageStabilization=\"false\"></tev:Capabilities>");

            XmlDocument docDoorControl = new XmlDocument();
            docDoorControl.LoadXml("<tdc:Capabilities xmlns:tdc=\"http://www.onvif.org/v3/DoorControl/wsdl\" ></tdc:Capabilities>");

            XmlDocument docPacs = new XmlDocument();
            docPacs.LoadXml("<tdc:Capabilities xmlns:tdc=\"http://www.onvif.org/v3/AccessControl/wsdl\" ></tdc:Capabilities>");

            XmlDocument docUser = new XmlDocument();
            docUser.LoadXml("<tdc:Capabilities xmlns:tdc=\"http://www.onvif.org/v3/User/wsdl\" ></tdc:Capabilities>");

            XmlDocument docReceiver = new XmlDocument();
            docReceiver.LoadXml(" <trv:Capabilities xmlns:trv=\"http://www.onvif.org/ver10/receiver/wsdl\" RTP_Multicast=\"true\" RTP_RTSP_TCP=\"true\" SupportedReceivers=\"15\" MaximumRTSPURILength=\"256\" />");

            XmlDocument docRecording  =new XmlDocument();
            docRecording.LoadXml("<trc:Capabilities xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" DynamicTracks=\"true\" Encoding=\"JPEG H264\" MaxRate=\"2\" MaxRecordings=\"1\"></trc:Capabilities>");

            XmlDocument docReplay = new XmlDocument();
            docReplay.LoadXml("<trp:Capabilities xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" ReversePlayback=\"true\"></trp:Capabilities>");

            return new Service[]
                       {
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver20/imaging/wsdl",
                                   XAddr =
                                       string.Format("http://{0}/Imaging/ImagingService.asmx",
                                                     HttpContext.Current.Request.Url.Authority),
                                   Capabilities = IncludeCapability ? docImg.DocumentElement : null

                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver10/device/wsdl",
                                   XAddr = HttpContext.Current.Request.Url.AbsolutePath,
                                   Capabilities = IncludeCapability ? doc.DocumentElement : null
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver20/ptz/wsdl",
                                   XAddr =
                                       string.Format("http://{0}/PTZ/PtzService.asmx",
                                                     HttpContext.Current.Request.Url.Authority),
                                   Capabilities = IncludeCapability ? docPtz.DocumentElement : null
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver10/media/wsdl",
                                   XAddr = string.Format("http://{0}/Media/MediaServiceFake.asmx",
                                                         HttpContext.Current.Request.Url.Authority),
                                   Capabilities = IncludeCapability ? docMedia.DocumentElement : null
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver10/events/wsdl",
                                   Capabilities = IncludeCapability ? docEvents.DocumentElement : null,
                                   XAddr =
                                       string.Format("http://{0}/Events/EventsServiceFake.asmx",
                                                     HttpContext.Current.Request.Url.Authority)
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver10/search/wsdl",
                                   XAddr = string.Format("http://{0}/Search/SearchService.asmx",
                                                         HttpContext.Current.Request.Url.Authority)
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver10/replay/wsdl",
                                   Capabilities = IncludeCapability ? docReplay.DocumentElement : null,
                                   XAddr = string.Format("http://{0}/Replay/ReplayService.asmx",
                                                         HttpContext.Current.Request.Url.Authority)
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver10/recording/wsdl",
                                   Capabilities = IncludeCapability ? docRecording.DocumentElement : null,
                                   XAddr = string.Format("http://{0}/Recording/RecordingService.asmx",
                                                         HttpContext.Current.Request.Url.Authority)
                               },

                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/ver10/receiver/wsdl",
                                   Capabilities = IncludeCapability ? docReceiver.DocumentElement: null,
                                   XAddr = string.Format("http://{0}/Receiver/ReceiverService.asmx",
                                                         HttpContext.Current.Request.Url.Authority)
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/v3/AccessControl/wsdl",
                                   Capabilities = IncludeCapability ? docPacs.DocumentElement : null,
                                   XAddr = string.Format("http://{0}/PACS/AccessControl.asmx",
                                                         HttpContext.Current.Request.Url.Authority)
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/v3/User/wsdl",
                                   Capabilities = IncludeCapability ? docUser.DocumentElement : null,
                                   XAddr = string.Format("http://{0}/PACS/User.asmx",
                                                         HttpContext.Current.Request.Url.Authority)
                               },
                           new Service()
                               {
                                   Version = new OnvifVersion() {Major = 2, Minor = 1},
                                   Namespace = "http://www.onvif.org/v3/DoorControl/wsdl",
                                   Capabilities = IncludeCapability ? docDoorControl.DocumentElement : null,
                                   XAddr = string.Format("http://{0}/PACS/DoorControl.asmx", HttpContext.Current.Request.Url.Authority)
                               },

                       };
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [SecurityExtension()]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override DeviceServiceCapabilities GetServiceCapabilities()
        {
            //SoapFaultSubCode subCode0 = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"));
            //SoapException exception = new SoapException("Not supported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode0);
            //throw exception;

            DeviceServiceCapabilities cap = new DeviceServiceCapabilities();

            cap.Security = new SecurityCapabilities();
            cap.Security.HttpDigest = true;
            cap.Security.HttpDigestSpecified = true;
            cap.Security.UsernameToken = true;
            cap.Security.UsernameTokenSpecified = true;
            cap.Security.SupportedEAPMethods = new int[]{1,2,3};
 
            cap.Network = new NetworkCapabilities();
            cap.Network.DynDNSSpecified = true;
            cap.Network.DynDNS = false;
            cap.Network.IPVersion6Specified = true;
            cap.Network.IPVersion6 = true;
            cap.Network.ZeroConfiguration = true;
            cap.Network.ZeroConfigurationSpecified = true;

            cap.System = new SystemCapabilities();
            cap.System.DiscoveryBye = true;
            cap.System.SystemBackup = true;
            cap.System.FirmwareUpgradeSpecified = true;
            cap.System.FirmwareUpgrade = false;

            return cap;
        }


        [WebMethod]
        [ScriptDriven()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[SecurityExtension()]
        public override Capabilities GetCapabilities(
            [System.Xml.Serialization.XmlElementAttribute("Category")] CapabilityCategory[] Category)
        {

            //SoapFaultSubCode subCode0 = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"));
            //SoapException exception = new SoapException("Not supported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode0);
            //throw exception;
            
            Capabilities result = new Capabilities();

            if (Category == null || Category.Length == 0)
            {
                result = GenerateAllCapabilities();
            }
            else
            {
                if (Category.Length == 1)
                {
                    CapabilityCategory category = Category[0];
                    switch (category)
                    {
                        case CapabilityCategory.All:
                            {
                                result = GenerateAllCapabilities();
                                //result.Device = new DeviceCapabilities();
                                //result.Device.XAddr = HttpContext.Current.Request.Url.AbsoluteUri;
                                //result.Device.Network = new NetworkCapabilities1();
                                ////result.Device.IO = new IOCapabilities();
                                ////result.Device.IO.RelayOutputsSpecified = true;
                                ////result.Device.IO.RelayOutputs = Storage.Instance.RelayOutputs.Count;
                                ////result.Device.IO.Extension = new IOCapabilitiesExtension();
                                //result.Device.System = new SystemCapabilities1();
                                //result.Device.Security = new SecurityCapabilities1();
                                ////result.Device.Security.TLS11 = true;
                                ////result.Device.Security.TLS12 = false;
                                //result.Device.System.SupportedVersions = new OnvifVersion[] { new OnvifVersion() { Major = 1, Minor = 3 }, new OnvifVersion() { Major = 1, Minor = 2 } };

                                //result.Events = new EventCapabilities();
                                //result.Events.XAddr = string.Format("http://{0}/Events/EventsServiceFake.asmx", HttpContext.Current.Request.Url.Authority);

                                //result.Media = new MediaCapabilities();
                                //result.Media.XAddr = string.Format("http://{0}/Media/MediaServiceFake.asmx", HttpContext.Current.Request.Url.Authority);
                                //result.Media.StreamingCapabilities = new RealTimeStreamingCapabilities();
                                //result.Media.StreamingCapabilities.RTP_RTSP_TCP = true;
                                //result.Media.StreamingCapabilities.RTP_RTSP_TCPSpecified = true;

                                ////result.PTZ = new PTZCapabilities();
                                ////result.PTZ.XAddr = string.Format("http://{0}/PTZ/PtzService.asmx", HttpContext.Current.Request.Url.Authority);

                                //result.Imaging = new ImagingCapabilities();
                                //result.Imaging.XAddr = string.Format("http://{0}/Imaging/Imaging.asmx", HttpContext.Current.Request.Url.Authority);
                
                                //result.Extension = new CapabilitiesExtension();
                                //result.Extension.DeviceIO = new DeviceIOCapabilities();
                                //result.Extension.DeviceIO.XAddr = string.Format("http://{0}/IO/IoService.asmx", HttpContext.Current.Request.Url.Authority);

                                //result.Extension.Search = new SearchCapabilities();
                                //result.Extension.Search.XAddr = string.Format("http://{0}/Search/SearchService.asmx",
                                //                                              HttpContext.Current.Request.Url.Authority);

                                //result.Extension.Search.MetadataSearch = true;

                            }
                            break;
                        case CapabilityCategory.Device:
                            {
                                result.Device = new DeviceCapabilities();
                                result.Device.XAddr = HttpContext.Current.Request.Url.AbsoluteUri;
                                result.Device.Network = new NetworkCapabilities1();
                                result.Device.Network.IPFilter = true;
                                result.Device.Network.IPFilterSpecified = true;
                                result.Device.IO = new IOCapabilities();
                                result.Device.IO.RelayOutputsSpecified = false;
                                result.Device.IO.RelayOutputs = Storage.Instance.RelayOutputs.Count;
                                result.Device.System = new SystemCapabilities1();
                                result.Device.System.FirmwareUpgrade = true;
                                result.Device.System.RemoteDiscovery = true;
                                result.Device.Security = new SecurityCapabilities1();
                                result.Device.Security.TLS11 = true;
                                result.Device.Security.TLS12 = false;
                                result.Device.System.SupportedVersions = new OnvifVersion[] { new OnvifVersion() { Major = 1, Minor = 3 }, new OnvifVersion() { Major = 1, Minor = 2 } };
                            }
                            break;
                        case CapabilityCategory.Media:
                            {
                                //System.Threading.Thread.Sleep(10000);
                                result.Media = new MediaCapabilities();
                                result.Media.XAddr = string.Format("http://{0}/Media/MediaServiceFake.asmx", HttpContext.Current.Request.Url.Authority);

                                result.Media.StreamingCapabilities = new RealTimeStreamingCapabilities();
                                result.Media.StreamingCapabilities.RTP_RTSP_TCP = true;
                                result.Media.StreamingCapabilities.RTP_RTSP_TCPSpecified = true;

                                //SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("NoSuchService1", "http://www.onvif.org/ver10/error"));
                                //SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"), subCode);
                                //throw new SoapException("NotSupported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                            };
                            break;
                        case CapabilityCategory.Events:
                            {
                                result.Events = new EventCapabilities();
                                result.Events.XAddr = string.Format("http://{0}/Events/EventsServiceFake.asmx", HttpContext.Current.Request.Url.Authority);
                                result.Events.WSSubscriptionPolicySupport = true;
                                
                            };
                            break;
                        case CapabilityCategory.Analytics:
                            {
                                //result.Analytics = new AnalyticsCapabilities();
                                //result.Analytics.XAddr = HttpContext.Current.Request.Url.AbsoluteUri;
                                //SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("NoSuchService", "http://www.onvif.org/ver10/error"));
                                //SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"), subCode);
                                //throw new SoapException("NotSupported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                            }
                            break;
                        case CapabilityCategory.Imaging:
                            {
                                result.Imaging = new ImagingCapabilities();
                                result.Imaging.XAddr = string.Format("http://{0}/Imaging/ImagingService.asmx", HttpContext.Current.Request.Url.Authority);
                                //SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("NoSuchService", "http://www.onvif.org/ver10/error"));
                                //SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"), subCode);
                                //throw new SoapException("NotSupported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode1);

                            }
                            break;
                        case CapabilityCategory.PTZ:
                            {
                                result.PTZ = new PTZCapabilities();
                                result.PTZ.XAddr = string.Format("http://{0}/PTZ/PtzService.asmx", HttpContext.Current.Request.Url.Authority);

                                //SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("NoSuchService", "http://www.onvif.org/ver10/error"));
                                //SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"), subCode);
                                //throw new SoapException("NotSupported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                            }
                            break;
                    }
                }
                else
                {
                    result = GenerateAllCapabilities();
                    //result.Device = new DeviceCapabilities();
                    //result.Device.XAddr = HttpContext.Current.Request.Url.AbsoluteUri;
                    //result.Device.Network = new NetworkCapabilities1();
                    //result.Device.Network.IPVersion6Specified = true;
                    //result.Device.Network.IPVersion6 = false;
                    //result.Device.Network.ZeroConfigurationSpecified = true;
                    //result.Device.Network.ZeroConfiguration = true;
                    //result.Device.IO = new IOCapabilities();
                    //result.Device.IO.RelayOutputsSpecified = true;
                    //result.Device.IO.RelayOutputs = Storage.Instance.RelayOutputs.Count;
                    ////result.Device.IO.Extension = new IOCapabilitiesExtension();

                    //result.Device.System = new SystemCapabilities1();
                    //result.Device.Security = new SecurityCapabilities1();
                    ////result.Device.Security.TLS11 = true;
                    ////result.Device.Security.TLS12 = false;
                    //result.Device.System.SupportedVersions = new OnvifVersion[] { new OnvifVersion() { Major = 1, Minor = 3 }, new OnvifVersion() { Major = 1, Minor = 2 } };

                    //result.Events = new EventCapabilities();
                    //result.Events.XAddr = string.Format("http://{0}/Events/EventsServiceFake.asmx", HttpContext.Current.Request.Url.Authority);

                    //result.Media = new MediaCapabilities();
                    //result.Media.XAddr = string.Format("http://{0}/Media/MediaServiceFake.asmx", HttpContext.Current.Request.Url.Authority);
                    //result.Media.StreamingCapabilities = new RealTimeStreamingCapabilities();
                    //result.Media.StreamingCapabilities.RTP_RTSP_TCP = true;
                    //result.Media.StreamingCapabilities.RTP_RTSP_TCPSpecified = true;

                    //result.PTZ = new PTZCapabilities();
                    //result.PTZ.XAddr = string.Format("http://{0}/PTZ/PtzService.asmx", HttpContext.Current.Request.Url.Authority);

                }
            }

            return result;
        }

        Capabilities GenerateAllCapabilities()
        {
            Capabilities result = new Capabilities();
            result.Device = new DeviceCapabilities();
            result.Device.XAddr = HttpContext.Current.Request.Url.AbsoluteUri;
            result.Device.Network = new NetworkCapabilities1();
            result.Device.Network.IPVersion6Specified = true;
            result.Device.Network.IPVersion6 = false;
            result.Device.Network.ZeroConfigurationSpecified = true;
            result.Device.Network.ZeroConfiguration = true;
            //result.Device.IO = new IOCapabilities();
            //result.Device.IO.RelayOutputsSpecified = true;
            //result.Device.IO.RelayOutputs = Storage.Instance.RelayOutputs.Count;
            //result.Device.IO.Extension = new IOCapabilitiesExtension();

            result.Device.System = new SystemCapabilities1();
            result.Device.Security = new SecurityCapabilities1();
            result.Device.Security.TLS11 = true;
            result.Device.Security.TLS12 = false;
            result.Device.System.SupportedVersions = new OnvifVersion[] { new OnvifVersion() { Major = 1, Minor = 3 }, new OnvifVersion() { Major = 1, Minor = 2 } };

            result.Events = new EventCapabilities();
            result.Events.XAddr = string.Format("http://{0}/Events/EventsServiceFake.asmx", HttpContext.Current.Request.Url.Authority);

            //result.Media = new MediaCapabilities();
            //result.Media.XAddr = string.Format("http://{0}/Media/MediaServiceFake.asmx", HttpContext.Current.Request.Url.Authority);
            //result.Media.StreamingCapabilities = new RealTimeStreamingCapabilities();
            //result.Media.StreamingCapabilities.RTP_RTSP_TCP = true;
            //result.Media.StreamingCapabilities.RTP_RTSP_TCPSpecified = true;

            result.PTZ = new PTZCapabilities();
            result.PTZ.XAddr = string.Format("http://{0}/PTZ/PtzService.asmx", HttpContext.Current.Request.Url.Authority);

            result.Imaging = new ImagingCapabilities();
            result.Imaging.XAddr = string.Format("http://{0}/Imaging/ImagingService.asmx", HttpContext.Current.Request.Url.Authority);

            result.Extension = new CapabilitiesExtension();
            result.Extension.DeviceIO = new DeviceIOCapabilities();
            // IoService
            result.Extension.DeviceIO.XAddr = string.Format("http://{0}/IO/IoService.asmx", HttpContext.Current.Request.Url.Authority);
            result.Extension.DeviceIO.AudioOutputs = 1;

            result.Extension.Search = new SearchCapabilities();
            result.Extension.Search.XAddr = string.Format("http://{0}/Search/SearchService.asmx",
                                                          HttpContext.Current.Request.Url.Authority);
            result.Extension.Search.MetadataSearch = true;

            result.Extension.Recording = new RecordingCapabilities();
            result.Extension.Recording.XAddr = string.Format("http://{0}/Recording/RecordingService.asmx",
                                                          HttpContext.Current.Request.Url.Authority);
            
            result.Extension.Replay = new ReplayCapabilities();
            result.Extension.Replay.XAddr = string.Format("http://{0}/Replay/ReplayService.asmx",
                                                          HttpContext.Current.Request.Url.Authority);


            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetHostname", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("HostnameInformation")]
        public override HostnameInformation GetHostname()
        {
            //System.Threading.Thread.Sleep(15000);
            string name = "Hostname";
            if (Application["hostname"] != null)
            {
                name = (string) Application["hostname"];
            }
            return new HostnameInformation() { FromDHCP =  false, Name = name};
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetHostname", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetHostname([System.Xml.Serialization.XmlElementAttribute(DataType = "token")] string Name)
        {
            if (Name.Length > 100 || Name.Contains("_"))
            {
                SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("InvalidHostname", "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                throw new SoapException("Hostname too long", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            }
            
            Application["hostname"] = Name;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDNS", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><s:Header xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\" /><soap:Body><GetDNSResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><ExtraNode/><DNSInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">false</FromDHCP><SearchDomain xmlns=\"http://www.onvif.org/ver10/schema\">domain.name</SearchDomain><DNSManual xmlns=\"http://www.onvif.org/ver10/schema\"><Type>IPv6</Type></DNSManual></DNSInformation></GetDNSResponse></soap:Body></soap:Envelope>")]
        [return: System.Xml.Serialization.XmlElementAttribute("DNSInformation")]
        public override DNSInformation GetDNS()
        {
            DNSInformation dnsInformation = null;

            if (Application["dnsInformation"] == null)
            {
                dnsInformation = new DNSInformation();
                dnsInformation.FromDHCP = false;
                dnsInformation.DNSManual = new IPAddress[]
                        {
                            new IPAddress(){IPv4Address = "10.1.1.10", Type = IPType.IPv4}
                        };

                dnsInformation.SearchDomain = new string[] { "domain.name" };
                Application["dnsInformation"] = dnsInformation;
            }
            else
            {
                dnsInformation = (DNSInformation)Application["dnsInformation"];
            }

            return dnsInformation;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDNS", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDNS(bool FromDHCP, 
            [System.Xml.Serialization.XmlElementAttribute("SearchDomain", DataType = "token")] string[] SearchDomain,
            [System.Xml.Serialization.XmlElementAttribute("DNSManual")] IPAddress[] DNSManual)
        {
            //throw new SoapException("Internal error", new XmlQualifiedName("Sender",
            //                                                         "http://www.w3.org/2003/05/soap-envelope"));
            
            if (DNSManual != null)
            {
                foreach (IPAddress address in DNSManual.Where( a => a.Type == IPType.IPv4))
                {
                    string[] parts = address.IPv4Address.Split(new char[] { '.' });
                    if (parts.Length != 4)
                    {
                        SoapFaultSubCode subCode =
                            new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv4Address",
                                                                      "http://www.onvif.org/ver10/error"));
                        SoapFaultSubCode subCode1 =
                            new SoapFaultSubCode(
                                new XmlQualifiedName("InvalidArgVal1", "http://www.onvif.org/ver10/error"), subCode);
                        throw new SoapException("Sender",
                                                new XmlQualifiedName("Sender",
                                                                     "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                    }
                }

                foreach (IPAddress address in DNSManual.Where(a => a.Type == IPType.IPv6))
                {
                    string[] parts = address.IPv6Address.Split(new char[] { ':' });
                    if (parts.Length != 8)
                    {
                        SoapFaultSubCode subCode =
                            new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv6Address",
                                                                      "http://www.onvif.org/ver10/error"));
                        SoapFaultSubCode subCode1 =
                            new SoapFaultSubCode(
                                new XmlQualifiedName("InvalidArgVal1", "http://www.onvif.org/ver10/error"), subCode);
                        throw new SoapException("Sender",
                                                new XmlQualifiedName("Sender",
                                                                     "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                    }
                }

            }


            DNSInformation dnsInformation1 = new DNSInformation();
            dnsInformation1.FromDHCP = FromDHCP;
            if (FromDHCP)
            {
                dnsInformation1.DNSFromDHCP = new IPAddress[] { new IPAddress() { IPv4Address = "10.1.1.2" } };
            }
            dnsInformation1.SearchDomain = SearchDomain;
            dnsInformation1.DNSManual = DNSManual;
            Application["dnsInformation"] = dnsInformation1;


            //DNSInformation dnsInformation = new DNSInformation();
            //dnsInformation.FromDHCP = FromDHCP;
            //if (FromDHCP)
            //{
            //    dnsInformation.DNSFromDHCP = new IPAddress[] { new IPAddress() { IPv4Address = "10.1.1.1" } };
            //}
            //dnsInformation.SearchDomain =  SearchDomain;
            //dnsInformation.SearchDomain = new string[] { "domain1", "domain2" };
            //dnsInformation.DNSManual = DNSManual;
            //dnsInformation.DNSManual = new IPAddress[]{new IPAddress(){IPv6Address = "FF02:1", Type = IPType.IPv4}, new IPAddress(){IPv4Address = "10.1.1.2"}, new IPAddress(){IPv4Address = "192.168.3.10"} };
            //Application["dnsInformation"] = dnsInformation;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNTP", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NTPInformation")]
        public override NTPInformation GetNTP()
        {
            //SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"));
            //SoapException exception = new SoapException("Not supported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), subCode);
            //throw exception;
            
            if (Application["ntpInformation"] != null)
            {
                return (NTPInformation) Application["ntpInformation"];
            }
            else
            {
                NTPInformation ntpInformation = new NTPInformation();
                ntpInformation.FromDHCP = true;
                //ntpInformation.NTPManual = new NetworkHost[]{new NetworkHost(){ Type =  NetworkHostType.DNS, DNSname = "www.google.ru"}};
                Application["ntpInformation"] = ntpInformation;
                return ntpInformation;
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNTP", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetNTP(bool FromDHCP, [System.Xml.Serialization.XmlElementAttribute("NTPManual")] NetworkHost[] NTPManual)
        {
            //SoapFaultSubCode notSupportedSubCode = new SoapFaultSubCode(new XmlQualifiedName("ActionNotSupported", "http://www.onvif.org/ver10/error"));
            //SoapException notSupportedException = new SoapException("Not supported", new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope"), notSupportedSubCode);
            //throw notSupportedException;

            if (NTPManual != null)
            {
                foreach (NetworkHost host in NTPManual.Where( a => a.Type == NetworkHostType.IPv4))
                {
                    string[] parts = host.IPv4Address.Split(new char[] { '.' });
                    if (parts.Length != 4)
                    {
                        SoapFaultSubCode subCode =
                            new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv4Address",
                                                                      "http://www.onvif.org/ver10/error"));
                        SoapFaultSubCode subCode1 =
                            new SoapFaultSubCode(
                                new XmlQualifiedName("InvalidArgVal1", "http://www.onvif.org/ver10/error"), subCode);
                        SoapException exception = new SoapException("Invalid Argument",
                                                                    new XmlQualifiedName("Sender",
                                                                                         "http://www.w3.org/2003/05/soap-envelope"),
                                                                    subCode1);
                        throw exception;
                    }
                }

                foreach (NetworkHost address in NTPManual.Where(a => a.Type == NetworkHostType.IPv6))
                {
                    string[] parts = address.IPv6Address.Split(new char[] { ':' });
                    if (parts.Length != 8)
                    {
                        SoapFaultSubCode subCode =
                            new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv6Address",
                                                                      "http://www.onvif.org/ver10/error"));
                        SoapFaultSubCode subCode1 =
                            new SoapFaultSubCode(
                                new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                        throw new SoapException("Sender",
                                                new XmlQualifiedName("Sender",
                                                                     "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                    }
                }


            }

            NTPInformation ntpInformation = new NTPInformation();
            ntpInformation.FromDHCP = FromDHCP;
            if (!FromDHCP)
            {
                List<NetworkHost> hosts = new List<NetworkHost>(NTPManual);
                //hosts.AddRange(new NetworkHost[]
                //                               {
                //                                   new NetworkHost() {DNSname = "somehost", Type = NetworkHostType.DNS},
                //                                   new NetworkHost()
                //                                       {Type = NetworkHostType.IPv4, IPv4Address = "10.1.1.2"}
                //                               });
                ntpInformation.NTPManual = hosts.ToArray();
            }
            else
            {
                ntpInformation.NTPFromDHCP = new NetworkHost[]
                                               {
                                                   new NetworkHost() {DNSname = "somehost", Type = NetworkHostType.DNS},
                                                   new NetworkHost()
                                                       {Type = NetworkHostType.IPv4, IPv4Address = "10.1.1.2"}
                                               };
            }
            Application["ntpInformation"] = ntpInformation;
        }

        public override DynamicDNSInformation GetDynamicDNS()
        {
            throw new NotImplementedException();
        }

        public override void SetDynamicDNS(DynamicDNSType Type, string Name, string TTL)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkInterfaces", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkInterfaces")]
        public override NetworkInterface[] GetNetworkInterfaces()
        {
            //throw new SoapException();

            NetworkInterface ni4 = new NetworkInterface();
            ni4.token = "ipv4";
            ni4.Enabled = true;
            ni4.Info = new NetworkInterfaceInfo();
            ni4.Info.HwAddress = "00:00:00:00:00:00";
            ni4.Info.Name = "name";
            ni4.Info.MTU = 100;
            ni4.IPv4 = new IPv4NetworkInterface();
            ni4.IPv4.Enabled = true;
            ni4.IPv4.Config = new IPv4Configuration();
            ni4.IPv4.Config.Manual = new PrefixedIPv4Address[]{new PrefixedIPv4Address(){PrefixLength = 0, Address = "192.168.23.77"}};
            ni4.IPv4.Config.DHCP = false;
            //ni4.IPv4.Config.FromDHCP = new PrefixedIPv4Address();
            //ni4.IPv4.Config.FromDHCP.Address = "192.168.3.33";
            //ni4.IPv4.Config.FromDHCP.PrefixLength = 2;

            NetworkInterface ni6 = new NetworkInterface();
            ni6.token = "ipv6";
            ni6.Enabled = true;
            ni6.Info = new NetworkInterfaceInfo();
            ni6.Info.MTU = 100;
            ni6.Info.HwAddress = "00:00:00:00:00:00";
            ni6.Info.Name = "Name";
            ni6.IPv6 = new IPv6NetworkInterface(); 
            ni6.IPv6.Enabled = true;
            ni6.IPv6.Config = new IPv6Configuration();
            ni6.IPv6.Config.DHCP = IPv6DHCPConfiguration.Stateful;
            ni6.IPv6.Config.FromDHCP = new PrefixedIPv6Address[]
                                           {
                                               new PrefixedIPv6Address() { Address = "2001:1:1:1:1:1:1:255", PrefixLength = 2 }
                                           };

            
            return new NetworkInterface[]{ni4, ni6};
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkInterfaces", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RebootNeeded")]
        public override bool SetNetworkInterfaces(string InterfaceToken, NetworkInterfaceSetConfiguration NetworkInterface)
        {
            if (NetworkInterface != null)
            {
                if (NetworkInterface.IPv4 != null && NetworkInterface.IPv4.Manual != null)
                {
                    foreach (PrefixedIPv4Address address in NetworkInterface.IPv4.Manual)
                    {
                        string[] parts = address.Address.Split(new char[] { '.' });
                        if (parts.Length != 4)
                        {
                            SoapFaultSubCode subCode =
                                new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv4Address",
                                                                          "http://www.onvif.org/ver10/error"));
                            SoapFaultSubCode subCode1 =
                                new SoapFaultSubCode(
                                    new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                            SoapException exception = new SoapException("Invalid Argument",
                                                                        new XmlQualifiedName("Sender",
                                                                                             "http://www.w3.org/2003/05/soap-envelope"),
                                                                        subCode1);
                            throw exception;
                        }
                    }
                }

                if (NetworkInterface.IPv6 != null && NetworkInterface.IPv6.Manual != null)
                {
                    foreach (PrefixedIPv6Address address in NetworkInterface.IPv6.Manual)
                    {
                        string[] parts = address.Address.Split(new char[] { ':' });
                        if (parts.Length != 8)
                        {
                            SoapFaultSubCode subCode =
                                new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv6Address",
                                                                          "http://www.onvif.org/ver10/error"));
                            SoapFaultSubCode subCode1 =
                                new SoapFaultSubCode(
                                    new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                            throw new SoapException("Sender",
                                                    new XmlQualifiedName("Sender",
                                                                         "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                        }
                    }
                }
            }

            if (InterfaceToken != "qwerty")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkProtocols", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkProtocols")]
        public override NetworkProtocol[] GetNetworkProtocols()
        {
            NetworkProtocol http = new NetworkProtocol() {Enabled = true, Name = NetworkProtocolType.HTTP};
            NetworkProtocol rtsp = new NetworkProtocol() { Enabled = true, Name = NetworkProtocolType.RTSP};

            http.Port = new int[]{8080, 8181};
            rtsp.Port = new int[]{9090, 10554};

            //return new NetworkProtocol[]{http, rtsp};
            return new NetworkProtocol[] { http, rtsp};
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkProtocols", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetNetworkProtocols([System.Xml.Serialization.XmlElementAttribute("NetworkProtocols")] NetworkProtocol[] NetworkProtocols)
        {
            if (NetworkProtocols.Where( p => p.Name == NetworkProtocolType.HTTPS).FirstOrDefault() != null)
            {
                SoapFaultSubCode subCode =
                    new SoapFaultSubCode(new XmlQualifiedName("ServiceNotSupported",
                                                              "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode1 =
                    new SoapFaultSubCode(
                        new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
                SoapException exception = new SoapException("Invalid Argument",
                                                            new XmlQualifiedName("Sender",
                                                                                 "http://www.w3.org/2003/05/soap-envelope"),
                                                            subCode1);
                throw exception;
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkDefaultGateway", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkGateway")]
        public override NetworkGateway GetNetworkDefaultGateway()
        {
            if (Application["gateway"] == null)
            {
                NetworkGateway gateway = new NetworkGateway();

                gateway.IPv4Address = new string[] {"192.168.1.1"};
                gateway.IPv6Address = new string[] {"2001:1:1:1:1:1:1:1"};
            
                Application["gateway"] = gateway;
            }
            return (NetworkGateway)Application["gateway"];
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkDefaultGateway", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetNetworkDefaultGateway(
            [System.Xml.Serialization.XmlElementAttribute("IPv4Address", DataType = "token")] string[] IPv4Address,
            [System.Xml.Serialization.XmlElementAttribute("IPv6Address", DataType = "token")] string[] IPv6Address)
        {

            if (IPv4Address != null)
            {
                foreach (string address in IPv4Address)
                {
                    string[] parts = address.Split(new char[] { '.' });
                    if (parts.Length != 4)
                    {
                        SoapFaultSubCode subCode =
                            new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv4Address1",
                                                                      "http://www.onvif.org/ver10/error"));
                        SoapFaultSubCode subCode1 =
                            new SoapFaultSubCode(
                                new XmlQualifiedName("InvalidArgVal1", "http://www.onvif.org/ver10/error"), subCode);
                        SoapException exception = new SoapException("Invalid Argument",
                                                                    new XmlQualifiedName("Sender",
                                                                                         "http://www.w3.org/2003/05/soap-envelope"),
                                                                    subCode1);
                        throw exception;
                    }
                }
            }

            if (IPv6Address != null)
            {
                foreach (string address in IPv6Address)
                {
                    string[] parts = address.Split(new char[] { ':' });
                    if (parts.Length != 8)
                    {
                        SoapFaultSubCode subCode =
                            new SoapFaultSubCode(new XmlQualifiedName("InvalidIPv6Address1",
                                                                      "http://www.onvif.org/ver10/error"));
                        SoapFaultSubCode subCode1 =
                            new SoapFaultSubCode(
                                new XmlQualifiedName("InvalidArgVal1", "http://www.onvif.org/ver10/error"), subCode);
                        throw new SoapException("Sender",
                                                new XmlQualifiedName("Sender",
                                                                     "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                    }
                }
            }


            NetworkGateway gateway = new NetworkGateway();

            gateway.IPv4Address = IPv4Address;
            gateway.IPv6Address = IPv6Address;

            Application["gateway"] = gateway;
        }

        public override NetworkZeroConfiguration GetZeroConfiguration()
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetZeroConfiguration", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetZeroConfiguration(string InterfaceToken, bool Enabled)
        {
            
        }

        public override IPAddressFilter GetIPAddressFilter()
        {
            throw new NotImplementedException();
        }

        public override void SetIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public override void AddIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public override void RemoveIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public override BinaryData GetAccessPolicy()
        {
            throw new NotImplementedException();
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

        public override bool GetClientCertificateMode()
        {
            throw new NotImplementedException();
        }

        public override void SetClientCertificateMode(bool Enabled)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetRelayOutputs", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RelayOutputs")]
        public override RelayOutput[] GetRelayOutputs()
        {
            return Storage.Instance.RelayOutputs.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetRelayOutputSettings", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputSettings(string RelayOutputToken, RelayOutputSettings Properties)
        {
            if (Properties.IdleState == RelayIdleState.open && Properties.Mode == RelayMode.Monostable)
            {
                ReturnFault(new string[] { "Receiver", "ActionNotSupported" });
            }
            
            bool found = false;

            foreach (RelayOutput output in Storage.Instance.RelayOutputs)
            {
                if (output.token == RelayOutputToken)
                {
                    output.Properties = Properties;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "RelayToken" });
            }
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetRelayOutputState", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputState(string RelayOutputToken, RelayLogicalState LogicalState)
        {
            bool found = false;

            foreach (RelayOutput output in Storage.Instance.RelayOutputs)
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

        public override string GetEndpointReference(out XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override RemoteUser GetRemoteUser()
        {
            throw new NotImplementedException();
        }

        public override void SetRemoteUser(RemoteUser RemoteUser)
        {
            throw new NotImplementedException();
        }

        public override bool SetHostnameFromDHCP(bool FromDHCP)
        {
            throw new NotImplementedException();
        }

        public override void SetAccessPolicy(BinaryData PolicyFile)
        {
            throw new NotImplementedException();
        }

        public override string SendAuxiliaryCommand(string AuxiliaryCommand)
        {
            throw new NotImplementedException();
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

        public override SystemLogUri[] GetSystemUris(out string SupportInfoUri, out string SystemBackupUri, out GetSystemUrisResponseExtension Extension)
        {
            throw new NotImplementedException();
        }

        public override string StartFirmwareUpgrade(out string UploadDelay, out string ExpectedDownTime)
        {
            throw new NotImplementedException();
        }

        public override string StartSystemRestore(out string ExpectedDownTime)
        {
            throw new NotImplementedException();
        }
    }
}
