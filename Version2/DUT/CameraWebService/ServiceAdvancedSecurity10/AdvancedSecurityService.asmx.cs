using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using CameraWebService.Servers;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.AdvancedSecurity10
{

    /// <summary>
    /// Summary description for Access Control Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "AdvancedSecurityServiceBinding", Namespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl")]
    public class AdvancedSecurityService : AdvancedSecurityServiceBinding
    {
        //TestSuit
        AdvancedSecurityServiceTest AdvancedSecurityServiceTest
        {
            get
            {
                if (Application[Base.AppVars.ADVSECSERVICE] != null)
                {
                    return (AdvancedSecurityServiceTest)Application[Base.AppVars.ADVSECSERVICE];
                }
                else
                {
                    AdvancedSecurityServiceTest serviceTest = new AdvancedSecurityServiceTest(TestCommon);
                    Application[Base.AppVars.ADVSECSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AdvancedSecurityCapabilitiesIncorrectResponseTag)]
        public override Capabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            Capabilities result = (Capabilities)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetServiceCapabilitiesTest);
            //result.KeystoreCapabilities.RSAKeyLengths = new int[0];

            return result;
        }
    
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/CreateRSAKeyPair", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><CreateRSAKeyPairResponse xmlns=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\"><EstimatedCreationTime>P0Y0M0DT0H0M0.6S</EstimatedCreationTime><KeyID>ID:1</KeyID></CreateRSAKeyPairResponse></soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing        
        [return: System.Xml.Serialization.XmlElementAttribute("KeyID", DataType = "NCName")]
        public override string CreateRSAKeyPair([System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")] string KeyLength, string Alias, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] out string EstimatedCreationTime)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "KeyLength", KeyLength);
            validation.Add(ParameterType.String, "Alias", Alias);
            EstimatedCreationTime = AdvancedSecurityServiceTest.TakeEstimatedCreationTime();
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.CreateRSAKeyPairTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
            "http://www.onvif.org/ver10/advancedsecurity/wsdl/UploadKeyPairInPKCS8",
            RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("KeyID", DataType = "NCName")]
        public override string UploadKeyPairInPKCS8([System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")] byte[] KeyPair, string Alias, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string EncryptionPassphraseID, string EncryptionPassphrase)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.Log, "KeyPair", KeyPair);
            validation.Add(ParameterType.String, "Alias", Alias);
            validation.Add(ParameterType.String, "EncryptionPassphraseID", EncryptionPassphraseID);

            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.UploadKeyPairInPKCS8Test);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/UploadCertificateWithPrivateKeyI" +
            "nPKCS12", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertificationPathID", DataType = "NCName")]
        public override string UploadCertificateWithPrivateKeyInPKCS12([System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")] byte[] CertWithPrivateKey, string CertificationPathAlias, string KeyAlias, [System.ComponentModel.DefaultValueAttribute(false)] bool IgnoreAdditionalCertificates, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string IntegrityPassphraseID, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string EncryptionPassphraseID, string Passphrase, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] out string KeyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.PKCS12WithoutPassphrase, "CertWithPrivateKey", CertWithPrivateKey);
            validation.Add(ParameterType.String, "CertificationPathAlias", CertificationPathAlias);
            validation.Add(ParameterType.String, "KeyAlias", KeyAlias);
            validation.Add(ParameterType.String, "IgnoreAdditionalCertificates", IgnoreAdditionalCertificates.ToString().ToLower());
            validation.Add(ParameterType.String, "IntegrityPassphraseID", IntegrityPassphraseID);
            validation.Add(ParameterType.String, "EncryptionPassphraseID", EncryptionPassphraseID);

            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.UploadCertificateWithPrivateKeyInPKCS12Test);
            KeyID = AdvancedSecurityServiceTest.TakeKeyIDFromPKCS12();
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetKeyStatus", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("KeyStatus")]
        public override string GetKeyStatus([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string KeyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "KeyID", KeyID);
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetKeyStatusTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetPrivateKeyStatus", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><hasPrivateKey>TrUe</hasPrivateKey></soap:Body></soap:Envelope>")]
        [return: System.Xml.Serialization.XmlElementAttribute("hasPrivateKey")]
        public override bool GetPrivateKeyStatus([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string KeyID)
        {
            var validation = new ParametersValidation();
            validation.Add(ParameterType.String, "KeyID", KeyID);
            return (bool)ExecuteGetCommand(validation, (ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout) 
                                                       => AdvancedSecurityServiceTest.GetPrivateKeyStatusTest(validationRequest, out stepType, out exc, out timeout));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAllKeys", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("KeyAttribute")]
        public override KeyAttribute[] GetAllKeys()
        {
            ParametersValidation validation = new ParametersValidation();
            KeyAttribute[] result = (KeyAttribute[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAllKeysTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteKey", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AdvancedSecurity_DeleteKeyIncorrectResponseTag)]
        public override void DeleteKey([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string KeyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "KeyID", KeyID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeleteKeyTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/CreatePKCS10CSR", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PKCS10CSR", DataType = "base64Binary")]
        public override byte[] CreatePKCS10CSR(DistinguishedName Subject, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string KeyID, [System.Xml.Serialization.XmlElementAttribute("CSRAttribute")] CSRAttribute[] CSRAttribute, AlgorithmIdentifier SignatureAlgorithm)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Subject.CommonName", Subject.CommonName);
            validation.Add(ParameterType.StringArray, "Subject.Country", Subject.Country);
            validation.Add(ParameterType.StringArray, "Subject.DistinguishedNameQualifier", Subject.DistinguishedNameQualifier);
            validation.Add(ParameterType.StringArray, "Subject.GenerationQualifier", Subject.GenerationQualifier);
            //TODO GenericAttribute
            validation.Add(ParameterType.StringArray, "Subject.Initials", Subject.Initials);
            validation.Add(ParameterType.StringArray, "Subject.GivenName", Subject.GivenName);
            validation.Add(ParameterType.StringArray, "Subject.Locality", Subject.Locality);
            validation.Add(ParameterType.StringArray, "Subject.Organization", Subject.Organization);
            validation.Add(ParameterType.StringArray, "Subject.OrganizationalUnit", Subject.OrganizationalUnit);
            validation.Add(ParameterType.StringArray, "Subject.Pseudonym", Subject.Pseudonym);
            validation.Add(ParameterType.StringArray, "Subject.SerialNumber", Subject.SerialNumber);
            validation.Add(ParameterType.StringArray, "Subject.StateOrProvinceName", Subject.StateOrProvinceName);
            validation.Add(ParameterType.StringArray, "Subject.Surname", Subject.Surname);
            validation.Add(ParameterType.StringArray, "Subject.Title", Subject.Title);

            validation.Add(ParameterType.String, "KeyID", KeyID);

            //TODO CSRAttribute
            if (SignatureAlgorithm != null)
            {
                validation.Add(ParameterType.String, "SignatureAlgorithm.algorithm", SignatureAlgorithm.algorithm);
                if (SignatureAlgorithm.parameters != null)
                {
                    validation.Add(ParameterType.String, "SignatureAlgorithm.algorithm", SignatureAlgorithm.parameters.ToString());
                }
            }

            int timeOut;
            SoapException ex;
            StepType stepType;

            byte[] response = AdvancedSecurityServiceTest.CreatePKCS10CSRTest(validation, out stepType, out ex, out timeOut, Subject, KeyID, CSRAttribute, SignatureAlgorithm);
            StepTypeProcessing(stepType, ex, timeOut);
        
            return response;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/CreateSelfSignedCertificate", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertificateID", DataType = "NCName")]
        public override string CreateSelfSignedCertificate([System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")] string X509Version, DistinguishedName Subject, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string KeyID, string Alias, System.DateTime notValidBefore, [System.Xml.Serialization.XmlIgnoreAttribute()] bool notValidBeforeSpecified, System.DateTime notValidAfter, [System.Xml.Serialization.XmlIgnoreAttribute()] bool notValidAfterSpecified, AlgorithmIdentifier SignatureAlgorithm, [System.Xml.Serialization.XmlElementAttribute("Extension")] X509v3Extension[] Extension)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "X509Version", X509Version);

            validation.Add(ParameterType.StringArray, "Subject.CommonName", Subject.CommonName);
            validation.Add(ParameterType.StringArray, "Subject.Country", Subject.Country);
            validation.Add(ParameterType.StringArray, "Subject.DistinguishedNameQualifier", Subject.DistinguishedNameQualifier);
            validation.Add(ParameterType.StringArray, "Subject.GenerationQualifier", Subject.GenerationQualifier);
            //TODO GenericAttribute
            validation.Add(ParameterType.StringArray, "Subject.Initials", Subject.Initials);
            validation.Add(ParameterType.StringArray, "Subject.GivenName", Subject.GivenName);
            validation.Add(ParameterType.StringArray, "Subject.Locality", Subject.Locality);
            validation.Add(ParameterType.StringArray, "Subject.Organization", Subject.Organization);
            validation.Add(ParameterType.StringArray, "Subject.OrganizationalUnit", Subject.OrganizationalUnit);
            validation.Add(ParameterType.StringArray, "Subject.Pseudonym", Subject.Pseudonym);
            validation.Add(ParameterType.StringArray, "Subject.SerialNumber", Subject.SerialNumber);
            validation.Add(ParameterType.StringArray, "Subject.StateOrProvinceName", Subject.StateOrProvinceName);
            validation.Add(ParameterType.StringArray, "Subject.Surname", Subject.Surname);
            validation.Add(ParameterType.StringArray, "Subject.Title", Subject.Title);

            validation.Add(ParameterType.String, "KeyID", KeyID);

            validation.Add(ParameterType.String, "Alias", Alias);

            //TODO notValidBefore
            //TODO notValidAfter
            //TODO notValidBeforeSpecified
            //TODO notValidAfterSpecified

            if (SignatureAlgorithm != null)
            {
                validation.Add(ParameterType.String, "SignatureAlgorithm.algorithm", SignatureAlgorithm.algorithm);
                if (SignatureAlgorithm.parameters != null)
                {
                    validation.Add(ParameterType.String, "SignatureAlgorithm.algorithm", SignatureAlgorithm.parameters.ToString());
                }
            }

            //TODO Extension
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.CreateSelfSignedCertificateTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/UploadCertificate", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertificateID", DataType = "NCName")]

        public override string UploadCertificate([System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")] byte[] Certificate, string Alias, string KeyAlias, [System.ComponentModel.DefaultValueAttribute(false)] bool PrivateKeyRequired, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] out string KeyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.X509Cert, "Certificate", Certificate);
            validation.Add(ParameterType.String, "Alias", Alias);
            validation.Add(ParameterType.String, "KeyAlias", KeyAlias);
            validation.Add(ParameterType.String, "PrivateKeyRequired", PrivateKeyRequired.ToString());

            string keyID = AdvancedSecurityServiceTest.TakeKeyID();
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.UploadCertificateTest);
            KeyID = keyID;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetCertificate", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetCertificateResponse xmlns=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\"><Certificate><CertificateID>certificateID1</CertificateID><Alias>Alias</Alias><CertificateContent>MIICvjCCAaagAwIBAgICA64wDQYJKoZIhvcNAQELBQAwITESMBAGA1UEAwwJMTI3LjAuMC4xMQswCQYDVQQGEwJVUzAgFw0xMzA5MTIxNDIxMTBaGA8zMDEzMDkxMzE0MjExMFowITESMBAGA1UEAwwJMTI3LjAuMC4xMQswCQYDVQQGEwJVUzCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKt1aK/wnJuQEYRfn53ji+F1DQpcX6rgZefGI2DeYdDw1YvyuVEnhk9lXESXYf2c1Kj9hYooMTNd0dwXdeBRWkbVop6+ojY03vfcRV3VlWRZ34mTmdyzuqRC0V3UrNko6CdHMhJ64UxpzzdsAsluszmasZBTxK0eP57qiFwXMPW+rREF++V1WfQ6BmrJBphzTHjAF4dZAuJ9SXv90PSFyuvg0UKI6S7WpW4LU8DixlOb4f/Keb4CYIAe73rO5hbLLgT5TrNvqhmMoZll1mu5HmWRDD/N76AWr6j+AjTrIRQnUwHaCi4C1HhLIBesvfRqiXUjFbgmRtjQgW5txKYSencCAwEAATANBgkqhkiG9w0BAQsFAAOCAQEAb7JeA7H4ZgV208ElhklbopvrAf2Z5dRptJS0/DAub4yoL+b3Asx1tR2/169YTaYiI1hzZ4yY+eeda67Z0rcdJ7/dSUOyk5c7dMZCKEzcX6HP0zXAHy8huQB5y6FBznCmcUTTbJgOiOFNjkLzsKBXVCJ8P1cqJiXcnX4kxDg+RPATHQogdjXLt4VclZJ2Sd8oUlAqbVv3WxjDg5c88PjR05qWo12+7x4dVmhx3MlVQjprsvi0GzO1V20HnzHNJA77+UqP9ENe+IIRWebrIs6xpGRmdpz7oQAWW6KT85P8GuwnPeWXglNPXoPvlFIyRkCbkxZxfb63Ayjgopwc0uueqQ==</CertificateContent><KeyID>keyID1</KeyID></Certificate></GetCertificateResponse></soap:Body></soap:Envelope>")]
        [return: System.Xml.Serialization.XmlElementAttribute("Certificate")]
        public override X509Certificate GetCertificate([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertificateID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertificateID", CertificateID);
            X509Certificate result = (X509Certificate)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetCertificateTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAllCertificates", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetAllCertificatesResponse xmlns=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\"><Certificate><CertificateID>certificateID1</CertificateID><Alias>Alias</Alias><CertificateContent>MIICvjCCAaagAwIBAgICA64wDQYJKoZIhvcNAQELBQAwITESMBAGA1UEAwwJMTI3LjAuMC4xMQswCQYDVQQGEwJVUzAgFw0xMzA5MTIxNDIxMTBaGA8zMDEzMDkxMzE0MjExMFowITESMBAGA1UEAwwJMTI3LjAuMC4xMQswCQYDVQQGEwJVUzCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKt1aK/wnJuQEYRfn53ji+F1DQpcX6rgZefGI2DeYdDw1YvyuVEnhk9lXESXYf2c1Kj9hYooMTNd0dwXdeBRWkbVop6+ojY03vfcRV3VlWRZ34mTmdyzuqRC0V3UrNko6CdHMhJ64UxpzzdsAsluszmasZBTxK0eP57qiFwXMPW+rREF++V1WfQ6BmrJBphzTHjAF4dZAuJ9SXv90PSFyuvg0UKI6S7WpW4LU8DixlOb4f/Keb4CYIAe73rO5hbLLgT5TrNvqhmMoZll1mu5HmWRDD/N76AWr6j+AjTrIRQnUwHaCi4C1HhLIBesvfRqiXUjFbgmRtjQgW5txKYSencCAwEAATANBgkqhkiG9w0BAQsFAAOCAQEAb7JeA7H4ZgV208ElhklbopvrAf2Z5dRptJS0/DAub4yoL+b3Asx1tR2/169YTaYiI1hzZ4yY+eeda67Z0rcdJ7/dSUOyk5c7dMZCKEzcX6HP0zXAHy8huQB5y6FBznCmcUTTbJgOiOFNjkLzsKBXVCJ8P1cqJiXcnX4kxDg+RPATHQogdjXLt4VclZJ2Sd8oUlAqbVv3WxjDg5c88PjR05qWo12+7x4dVmhx3MlVQjprsvi0GzO1V20HnzHNJA77+UqP9ENe+IIRWebrIs6xpGRmdpz7oQAWW6KT85P8GuwnPeWXglNPXoPvlFIyRkCbkxZxfb63Ayjgopwc0uueqQ==</CertificateContent><KeyID>keyID1</KeyID></Certificate></GetAllCertificatesResponse></soap:Body></soap:Envelope>")]        
        [return: System.Xml.Serialization.XmlElementAttribute("Certificate")]
        public override X509Certificate[] GetAllCertificates()
        {
            ParametersValidation validation = new ParametersValidation();
            X509Certificate[] result = (X509Certificate[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAllCertificatesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteCertificate", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCertificate([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertificateID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertificateID", CertificateID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeleteCertificateTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/CreateCertificationPath", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertificationPathID", DataType = "NCName")]
        public override string CreateCertificationPath(CertificateIDs CertificateIDs, string Alias)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Alias", Alias);
            validation.Add(ParameterType.StringArray, "CertificateIDs.CertificateID", CertificateIDs.CertificateID);

            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.CreateCertificationPathTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetCertificationPath", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertificationPath")]
        public override CertificationPath GetCertificationPath([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertificationPathID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertificationPathID", CertificationPathID);

            CertificationPath result = (CertificationPath)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetCertificationPathTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAllCertificationPaths", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertificationPathID", DataType = "NCName")]
        public override string[] GetAllCertificationPaths()
        {
            ParametersValidation validation = new ParametersValidation();
            string[] result = (string[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAllCertificationPathsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteCertificationPath", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCertificationPath([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertificationPathID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertificationPathID", CertificationPathID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeleteCertificationPathTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
            "http://www.onvif.org/ver10/advancedsecurity/wsdl/UploadPassphrase",
            RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PassphraseID", DataType = "NCName")]
        public override string UploadPassphrase(string Passphrase, string PassphraseAlias)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Passphrase", Passphrase);
            validation.Add(ParameterType.String, "PassphraseAlias", PassphraseAlias);

            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.UploadPassphraseTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
            "http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAllPassphrases",
            RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PassphraseAttribute")]
        public override PassphraseAttribute[] GetAllPassphrases()
        {
            ParametersValidation validation = new ParametersValidation();

            PassphraseAttribute[] result = (PassphraseAttribute[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAllPassphrasesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
            "http://www.onvif.org/ver10/advancedsecurity/wsdl/DeletePassphrase",
            RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeletePassphrase([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string PassphraseID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "PassphraseID", PassphraseID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeletePassphraseTest);
        }

       
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/UploadCRL", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CrlID", DataType = "ID")]
        public override string UploadCRL([System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")] byte[] Crl, string Alias, UploadCRLAnyParameters anyParameters)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "Alias", Alias);
            validation.Add(ParameterType.CRL, "Crl", Crl);
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.UploadCRLTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetCRL", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Crl")]
        public override CRL GetCRL([System.Xml.Serialization.XmlElementAttribute(DataType = "ID")] string CrlID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CrlID", CrlID);
            CRL result = (CRL)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetCRLTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAllCRLs", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Crl")]
        public override CRL[] GetAllCRLs()
        {
            ParametersValidation validation = new ParametersValidation();
            CRL[] result = (CRL[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAllCRLsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteCRL", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCRL([System.Xml.Serialization.XmlElementAttribute(DataType = "ID")] string CrlID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CrlID", CrlID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeleteCRLTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/CreateCertPathValidationPolicy", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertPathValidationPolicyID", DataType = "ID")]
        public override string CreateCertPathValidationPolicy(string Alias, CertPathValidationParameters Parameters, [System.Xml.Serialization.XmlElementAttribute("TrustAnchor")] TrustAnchor[] TrustAnchor, CreateCertPathValidationPolicyAnyParameters anyParameters)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Alias", Alias);
            //validation.Add(ParameterType.OptionalBool, "Parameters/UseDeltaCRLs", Parameters.UseDeltaCRLs, Parameters.UseDeltaCRLsSpecified);
            validation.Add(ParameterType.OptionalBool, "Parameters/RequireTLSWWWClientAuthExtendedKeyUsage", Parameters.RequireTLSWWWClientAuthExtendedKeyUsage, Parameters.RequireTLSWWWClientAuthExtendedKeyUsage);
            if (TrustAnchor != null)
            {
            validation.Add(ParameterType.OptionalString, "TrustAnchor/CertificateID", TrustAnchor[0].CertificateID);
            }
            //TODO Parameters
            //TODO TrustAnchor
            //TODO anyParameters
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.CreateCertPathValidationPolicyTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetCertPathValidationPolicy", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertPathValidationPolicy")]
        public override CertPathValidationPolicy GetCertPathValidationPolicy([System.Xml.Serialization.XmlElementAttribute(DataType = "ID")] string CertPathValidationPolicyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertPathValidationPolicyID", CertPathValidationPolicyID);
            CertPathValidationPolicy result = (CertPathValidationPolicy)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetCertPathValidationPolicyTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAllCertPathValidationPolicies" +
            "", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertPathValidationPolicy")]
        public override CertPathValidationPolicy[] GetAllCertPathValidationPolicies()
        {
            ParametersValidation validation = new ParametersValidation();
            CertPathValidationPolicy[] result = (CertPathValidationPolicy[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAllCertPathValidationPoliciesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteCertPathValidationPolicy", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCertPathValidationPolicy([System.Xml.Serialization.XmlElementAttribute(DataType = "ID")] string CertPathValidationPolicyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertPathValidationPolicyID", CertPathValidationPolicyID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeleteCertPathValidationPolicyTest);
        }
    

    

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/AddServerCertificateAssignment", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AdvancedSecurity_AddServerCertificateAssignmentResponseTag)]
        public override void AddServerCertificateAssignment([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertificationPathID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertificationPathID", CertificationPathID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.AddServerCertificateAssignmentTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/RemoveServerCertificateAssignmen" +
            "t", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveServerCertificateAssignment([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertificationPathID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertificationPathID", CertificationPathID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.RemoveServerCertificateAssignmentTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/ReplaceServerCertificateAssignme" +
            "nt", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ReplaceServerCertificateAssignment([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string OldCertificationPathID, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string NewCertificationPathID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OldCertificationPathID", OldCertificationPathID);
            validation.Add(ParameterType.String, "NewCertificationPathID", NewCertificationPathID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.ReplaceServerCertificateAssignmentTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAssignedServerCertificates", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertificationPathID", DataType = "NCName")]
        public override string[] GetAssignedServerCertificates()
        {
            ParametersValidation validation = new ParametersValidation();
            string[] result = (string[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAssignedServerCertificatesTest);
            return result;
        }

        //[System.Web.Services.WebMethodAttribute()]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteUnreferencedCertPathValida" +
        //    "tionPolicies", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //public override void DeleteUnreferencedCertPathValidationPolicies()
        //{
        //    ParametersValidation validation = new ParametersValidation();
        //    ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeleteUnreferencedCertPathValidationPoliciesTest);
        //}

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/SetClientAuthenticationRequired", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetClientAuthenticationRequired(bool clientAuthenticationRequired)
        {
            ParametersValidation validation = new ParametersValidation();
            //TODO clientAuthenticationRequired
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.SetClientAuthenticationRequiredTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetClientAuthenticationRequired", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("clientAuthenticationRequired")]
        public override bool GetClientAuthenticationRequired()
        {
            ParametersValidation validation = new ParametersValidation();
            bool result = (bool)ExecuteGetCommand(validation, (ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
                                                       => AdvancedSecurityServiceTest.GetClientAuthenticationRequiredTest(validationRequest, out stepType, out exc, out timeout));
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/AddCertPathValidationPolicyAssig" +
            "nment", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddCertPathValidationPolicyAssignment([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertPathValidationPolicyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertPathValidationPolicyID", CertPathValidationPolicyID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.AddCertPathValidationPolicyAssignmentTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/RemoveCertPathValidationPolicyAs" +
            "signment", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveCertPathValidationPolicyAssignment([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string CertPathValidationPolicyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CertPathValidationPolicyID", CertPathValidationPolicyID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.RemoveCertPathValidationPolicyAssignmentTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/ReplaceCertPathValidationPolicyA" +
            "ssignment", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ReplaceCertPathValidationPolicyAssignment([System.Xml.Serialization.XmlElementAttribute(DataType = "ID")] string OldCertPathValidationPolicyID, [System.Xml.Serialization.XmlElementAttribute(DataType = "ID")] string NewCertPathValidationPolicyID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OldCertPathValidationPolicyID", OldCertPathValidationPolicyID);
            validation.Add(ParameterType.String, "NewCertPathValidationPolicyID", NewCertPathValidationPolicyID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.ReplaceCertPathValidationPolicyAssignmentTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAssignedCertPathValidationPol" +
            "icies", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CertPathValidationPolicyID", DataType = "ID")]
        public override string[] GetAssignedCertPathValidationPolicies()
        {
            ParametersValidation validation = new ParametersValidation();
            string[] result = (string[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAssignedCertPathValidationPoliciesTest);
            return result;
        }
   

    
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/AddDot1XConfiguration", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Dot1XID", DataType = "NCName")]
        public override string AddDot1XConfiguration(Dot1XConfiguration Dot1XConfiguration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Dot1XConfiguration/Outer/@Method", Dot1XConfiguration.Outer.Method);
            validation.Add(ParameterType.OptionalString, "Dot1XConfiguration/Outer/Identity", Dot1XConfiguration.Outer.Identity);
            //validation.Add(ParameterType.OptionalString, "Dot1XConfiguration/Outer/AnonymousID", Dot1XConfiguration.Outer.AnonymousID);
            validation.Add(ParameterType.String, "Dot1XConfiguration/Outer/CertificationPathID", Dot1XConfiguration.Outer.CertificationPathID);
            validation.Add(ParameterType.OptionalString, "Dot1XConfiguration/Outer/PassphraseID", Dot1XConfiguration.Outer.PassphraseID);
            validation.Add(ParameterType.String, "Dot1XConfiguration/Outer/Inner/@Method", Dot1XConfiguration.Outer.Inner.Method);
            validation.Add(ParameterType.String, "Dot1XConfiguration/Outer/Inner/Identity", Dot1XConfiguration.Outer.Inner.Identity);
            //validation.Add(ParameterType.OptionalString, "Dot1XConfiguration/Outer/Inner/AnonymousID", Dot1XConfiguration.Outer.Inner.AnonymousID);
            validation.Add(ParameterType.OptionalString, "Dot1XConfiguration/Outer/Inner/CertificationPathID", Dot1XConfiguration.Outer.Inner.CertificationPathID);
            validation.Add(ParameterType.String, "Dot1XConfiguration/Outer/Inner/PassphraseID", Dot1XConfiguration.Outer.Inner.PassphraseID);
            validation.Add(ParameterType.OptionalString, "Alias", Dot1XConfiguration.Alias);
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.AddDot1XConfigurationTest);
            return result;
        }

         [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetAllDot1XConfigurations", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Dot1XConfiguration[] GetAllDot1XConfigurations()
        {
            ParametersValidation validation = new ParametersValidation();
            Dot1XConfiguration[] result = (Dot1XConfiguration[])ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetAllDot1XConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetDot1XConfiguration", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Dot1XConfiguration")]
        public override Dot1XConfiguration GetDot1XConfiguration([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string Dot1XID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Dot1XID", Dot1XID);
            Dot1XConfiguration result = (Dot1XConfiguration)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetDot1XConfigurationTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteDot1XConfiguration", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteDot1XConfiguration([System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string Dot1XID)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Dot1XID", Dot1XID);
            ExecuteVoidCommand(validation, AdvancedSecurityServiceTest.DeleteDot1XConfigurationTest);
        }

       [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/SetNetworkInterfaceDot1XConfigur" +
            "ation", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RebootNeeded")]
        public override bool SetNetworkInterfaceDot1XConfiguration(string token, [System.Xml.Serialization.XmlElementAttribute(DataType = "NCName")] string Dot1XID)
       {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", token);
            validation.Add(ParameterType.String, "Dot1XID", Dot1XID);
            bool result = (bool) ExecuteGetCommand(validation, (ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
                                                       => AdvancedSecurityServiceTest.SetNetworkInterfaceDot1XConfigurationTest(validationRequest, out stepType, out exc, out timeout));
            return result;
        }

        

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/GetNetworkInterfaceDot1XConfigur" +
            "ation", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Dot1XID", DataType = "NCName")]
        public override string GetNetworkInterfaceDot1XConfiguration(string token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", token);
            string result = (string)ExecuteGetCommand(validation, AdvancedSecurityServiceTest.GetNetworkInterfaceDot1XConfigurationTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/advancedsecurity/wsdl/DeleteNetworkInterfaceDot1XConfi" +
            "guration", RequestNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/advancedsecurity/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RebootNeeded")]
        public override bool DeleteNetworkInterfaceDot1XConfiguration(string token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", token);
            bool result = (bool)ExecuteGetCommand(validation, (ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
                                                      => AdvancedSecurityServiceTest.DeleteNetworkInterfaceDot1XConfigurationTest(validationRequest, out stepType, out exc, out timeout));
            return result;
        }

    }
}
