using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;

namespace DUT.WithLogic.Engine
{
    public static class ONVIFFault
    {
        public static SoapException GetException(string message, System.Xml.XmlQualifiedName code, System.Xml.XmlNode detail, SoapFaultSubCode subCode, Exception innerException)
        {
            string actor = "http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver";
            string role = "http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver";
            string lang = "en";
            return new SoapException(message, code, actor, role, lang, detail, subCode, innerException);
        }

        #region SubCodes
        const string ErrorNamespace = "http://www.onvif.org/ver10/error";

        static XmlQualifiedName InvalidDateTime = new XmlQualifiedName("InvalidDateTime", ErrorNamespace);
        static XmlQualifiedName InvalidArgVal = new XmlQualifiedName("InvalidArgVal", ErrorNamespace);
        static XmlQualifiedName ActionNotSupported = new XmlQualifiedName("ActionNotSupported", ErrorNamespace);
        static XmlQualifiedName OperationProhibited = new XmlQualifiedName("OperationProhibited", ErrorNamespace);
        static XmlQualifiedName FixedScope = new XmlQualifiedName("FixedScope", ErrorNamespace);
        static XmlQualifiedName NoScope = new XmlQualifiedName("NoScope", ErrorNamespace);
        static XmlQualifiedName NoSuchService = new XmlQualifiedName("NoSuchService", ErrorNamespace);
        static XmlQualifiedName UsernameClash = new XmlQualifiedName("UsernameClash", ErrorNamespace);
        static XmlQualifiedName TooManyUsers = new XmlQualifiedName("TooManyUsers", ErrorNamespace);
        static XmlQualifiedName Action = new XmlQualifiedName("Action", ErrorNamespace);
        static XmlQualifiedName PasswordTooLong = new XmlQualifiedName("PasswordTooLong", ErrorNamespace);
        static XmlQualifiedName UsernameTooLong = new XmlQualifiedName("UsernameTooLong", ErrorNamespace);
        static XmlQualifiedName Password = new XmlQualifiedName("Password", ErrorNamespace);
        static XmlQualifiedName UsernameTooShort = new XmlQualifiedName("UsernameTooShort", ErrorNamespace);
        static XmlQualifiedName FixedUser = new XmlQualifiedName("FixedUser", ErrorNamespace);
        static XmlQualifiedName UsernameMissing = new XmlQualifiedName("UsernameMissing", ErrorNamespace);
        static XmlQualifiedName InvalidHostname = new XmlQualifiedName("InvalidHostname", ErrorNamespace);
        static XmlQualifiedName InvalidIPv4Address = new XmlQualifiedName("InvalidIPv4Address", ErrorNamespace);
        static XmlQualifiedName InvalidIPv6Address = new XmlQualifiedName("InvalidIPv6Address", ErrorNamespace);
        static XmlQualifiedName TimeSyncedToNtp = new XmlQualifiedName("TimeSyncedToNtp", ErrorNamespace);
        static XmlQualifiedName InvalidNetworkInterface = new XmlQualifiedName("InvalidNetworkInterface", ErrorNamespace);
        static XmlQualifiedName NoConfig = new XmlQualifiedName("NoConfig", ErrorNamespace);
        static XmlQualifiedName MaxOSDs = new XmlQualifiedName("MaxOSDs", ErrorNamespace);

        #endregion //SubCodes

        #region GeneralExceptions

        public static SoapException GetGeneralException_ActionNotSupported()
        {
            return GetException("Action not supported.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(ActionNotSupported), null);
        }

        public static SoapException GetGeneralException_InvalidArgVal(string Message)
        {
            return GetException(Message, SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal), null);
        }

        #endregion //GeneralExceptions

        #region DeviceManagementExceptions

        public static SoapException GetDeviceManagementException_InvalidArgVal_InvalidDateTime(Exception innerException)
        { 
            return GetException(innerException.Message, SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(InvalidDateTime)), null);
        }

        public static SoapException GetDeviceManagementException_OperationProhibited_FixedScope()
        {
            return GetException("Trying to Remove fixed scope parameter, command rejected.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(OperationProhibited, new SoapFaultSubCode(FixedScope)), null);
        }

        public static SoapException GetDeviceManagementException_InvalidArgVal_NoScope()
        {
            return GetException("Trying to Remove scope which does not exist.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(NoScope)), null);
        }

        public static SoapException GetDeviceManagementException_ActionNotSupported_NoSuchService()
        {
            return GetException("The requested WSDL service category is not supported by the device.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(ActionNotSupported, new SoapFaultSubCode(NoSuchService)), null);
        }

        public static SoapException GetDeviceManagementException_InvalidArgVal_InvalidHostname()
        {
            return GetException("The requested hostname cannot be accepted by the device.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(InvalidHostname)), null);
        }

        public static SoapException GetDeviceManagementException_OperationProhibited_UsernameClash()
        {
            return GetException("Username already exists.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(OperationProhibited, new SoapFaultSubCode(UsernameClash)), null);
        }

        public static SoapException GetDeviceManagementException_Action_TooManyUsers()
        {
            return GetException("Maximum number of supported users exceeded.", SoapException.ServerFaultCode, null, new SoapFaultSubCode(Action, new SoapFaultSubCode(TooManyUsers)), null);
        }

        public static SoapException GetDeviceManagementException_OperationProhibited_PasswordTooLong()
        {
            return GetException("The password is too long.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(OperationProhibited, new SoapFaultSubCode(PasswordTooLong)), null);
        }

        public static SoapException GetDeviceManagementException_OperationProhibited_UsernameTooLong()
        {
            return GetException("The username is too long.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(OperationProhibited, new SoapFaultSubCode(UsernameTooLong)), null);
        }

        public static SoapException GetDeviceManagementException_OperationProhibited_Password()
        {
            return GetException("Too weak password.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(OperationProhibited, new SoapFaultSubCode(Password)), null);
        }

        public static SoapException GetDeviceManagementException_OperationProhibited_UsernameTooShort()
        {
            return GetException("The username is too short.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(OperationProhibited, new SoapFaultSubCode(UsernameTooShort)), null);
        }

        public static SoapException GetDeviceManagementException_InvalidArgVal_FixedUser()
        {
            return GetException("Username may not be deleted.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(FixedUser)), null);
        }

        public static SoapException GetDeviceManagementException_InvalidArgVal_UsernameMissing()
        {
            return GetException("Username not recognized.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(UsernameMissing)), null);
        }
        public static SoapException GetDeviceManagementException_InvalidArgVal_InvalidIPv4Address()
        {
            return GetException("The suggested IPv4 address is invalid.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(InvalidIPv4Address)), null);
        }

        public static SoapException GetDeviceManagementException_InvalidArgVal_InvalidIPv6Address()
        {
            return GetException("The suggested IPv6 address is invalid.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(InvalidIPv6Address)), null);
        }

        public static SoapException GetDeviceManagementException_InvalidArgVal_TimeSyncedToNtp()
        {
            return GetException("Current DateTimeType requires an NTP server.", SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(TimeSyncedToNtp)), null);
        }

        public static SoapException GetDeviceManagementException_InvalidArgVal_InvalidNetworkInterface(string token)
        {
            return GetException(String.Format("The supplied network interface token does not exist: {0}.", token), SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(InvalidNetworkInterface)), null);
        }

        #endregion //DeviceManagementExceptions

        #region Media2Exceptions

        internal static Exception GetMedia2Exception_InvalidArgVal_NoConfig(string token)
        {
            return GetException(String.Format("The requested video source configuration or OSD indicated with {0} does not exist.", token), SoapException.ClientFaultCode, null, new SoapFaultSubCode(InvalidArgVal, new SoapFaultSubCode(NoConfig)), null);
        }

        internal static Exception GetMedia2Exception_Action_MaxOSDs(string token)
        {
            return GetException(String.Format("The maximum number of supported OSDs by the specific {0} has been reached. ", token), SoapException.ClientFaultCode, null, new SoapFaultSubCode(Action, new SoapFaultSubCode(MaxOSDs)), null);
        }
        #endregion //Media2Exceptions


    }
}