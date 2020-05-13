using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using DUT.CameraWebService.Common;
using System.Xml.Serialization;
using System.IO;
using System.Web.Services;

namespace DUT.CameraWebService.Base
{

    public abstract class BaseServiceTest
    {

        protected abstract string ServiceName { get; }
            

        private int[] _commandCount;

        protected int[] CommandCount
        {
            get { return _commandCount; }
        }
        
        /// <summary>
        /// Test suit description
        /// </summary>
        private TestCommon _testCommon = null;


        protected TestCommon TestCommon
        {
            get { return _testCommon; }
        }

        public BaseServiceTest(TestCommon testCommon)
        {
            _testCommon = testCommon;
        }

        protected void InitCommandsCount(int maxCommands)
        {
            _commandCount = new int[maxCommands];
            for (int i = 0; i < _commandCount.Length; i++)
            {
                _commandCount[i] = 0;
            }
        }

        #region General

        /// <summary>
        /// Return incremented target if it is not more than maxValue. Return 0 in other case.
        /// </summary>
        /// <param name="maxValue">Max value for target</param>
        /// <param name="teaget">Incremented value</param>
        /// <returns>Changed target</returns>
        protected void Increment(int maxValue, int index)
        {
            if (maxValue - 1 <= _commandCount[index])
            {
                _commandCount[index] = 0;
            }
            else
            {
                _commandCount[index]++;
            }
        }

        public void ResetTestSuit()
        {
            for (int i = 0; i < _commandCount.Length; i++)
            {
                _commandCount[i] = 0;
            }
        }

        #endregion //General


        #region Common commands

        protected T GetCommand<T>(string commandName,
            int commandNumber,
            ParametersValidation validationRequest,
            out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<T>(commandName, commandNumber, validationRequest, false, out stepType, out ex, out Timeout);
        }

        protected T GetCommand<T>(string commandName,
            int commandNumber,
            ParametersValidation validationRequest, bool sendNotifications,
            out StepType stepType, out SoapException ex, out int Timeout)
        {
            return CommonGetCommand<T>(commandName, commandNumber, validationRequest, sendNotifications, out stepType, out ex, out Timeout);
        }

        protected T GetCommand<T>(string commandName,
    int commandNumber,
    ParametersValidation validationRequest,
    out StepType stepType, out SoapException ex, out int Timeout, out int special)
        {
            return GetCommand<T>(commandName, commandNumber, validationRequest, false, out stepType, out ex, out Timeout, out special);
        }

        protected T GetCommand<T>(string commandName,
            int commandNumber,
            ParametersValidation validationRequest, bool sendNotifications,
            out StepType stepType, out SoapException ex, out int Timeout, out int special)
        {
            return CommonGetCommand<T>(commandName, commandNumber, validationRequest, sendNotifications, out stepType, out ex, out Timeout, out special);
        }

        protected void VoidCommand(string commandName,
            int commandNumber,
            ParametersValidation validationRequest,
            out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand(commandName, commandNumber, validationRequest, false, out stepType, out ex, out Timeout);
        }

        protected void VoidCommand(string commandName,
            int commandNumber,
            ParametersValidation validationRequest,
            out StepType stepType, out SoapException ex, out int Timeout, out int special)
        {
            VoidCommand(commandName, commandNumber, validationRequest, false, out stepType, out ex, out Timeout, out special);
        }

        protected void VoidCommand(string commandName,
            int commandNumber,
            ParametersValidation validationRequest, bool sendNotifications,
            out StepType stepType, out SoapException ex, out int Timeout)
        {
            CommonVoidCommand(commandName, commandNumber, validationRequest, sendNotifications, out stepType, out ex, out Timeout);
        }

        protected void VoidCommand(string commandName,
            int commandNumber,
            ParametersValidation validationRequest, bool sendNotifications,
            out StepType stepType, out SoapException ex, out int Timeout, out int special)
        {
            CommonVoidCommand(commandName, commandNumber, validationRequest, sendNotifications, out stepType, out ex, out Timeout, out special);
        }

        private T CommonGetCommand<T>(string commandName,
            int commandNumber,
            ParametersValidation validationRequest, bool sendNotifications,
            out StepType stepType, out SoapException ex, out int Timeout)
        {
            StepType st;
            SoapException exception;
            int timeout;
            T target = default(T);
            object targetObj = null;

            ExecuteCommand(commandName, commandNumber, validationRequest,  sendNotifications, out st, out exception, out timeout,
                new Func<XmlNode, StepType>((test) => { return TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out exception, out timeout, typeof(T)); }));

            stepType = st;
            ex = exception;
            Timeout = timeout;
            if ((st != StepType.Normal) && (targetObj == null))
            {
                target = default(T);
            }
            else
            {
                target = (T)targetObj;
            }


            return target;
        }

        private T CommonGetCommand<T>(string commandName,
           int commandNumber,
           ParametersValidation validationRequest, bool sendNotifications,
           out StepType stepType, out SoapException ex, out int Timeout, out int special)
        {
            StepType st;
            SoapException exception;
            int timeout;
            T target = default(T);
            object targetObj = null;
            int localSpecial = 0;

            ExecuteCommand(commandName, commandNumber, validationRequest, sendNotifications, out st, out exception, out timeout,
                new Func<XmlNode, StepType>((test) => { return TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out exception, out timeout, typeof(T), out localSpecial); }));

            special = localSpecial;
            stepType = st;
            ex = exception;
            Timeout = timeout;
            if ((st != StepType.Normal) && (targetObj == null))
            {
                target = default(T);
            }
            else
            {
                target = (T)targetObj;
            }


            return target;
        }

        private void CommonVoidCommand(string commandName,
            int commandNumber,
            ParametersValidation validationRequest, bool sendNotifications,
            out StepType stepType, out SoapException ex, out int Timeout)
        {
            StepType st;
            SoapException exception;
            int timeout;

            ExecuteCommand(commandName, commandNumber, validationRequest, sendNotifications, out st, out exception, out timeout,
                new Func<XmlNode, StepType>((test) => { return TestCommon.GenerateResponseStepTypeVoid(test, out exception, out timeout); }));

            stepType = st;
            ex = exception;
            Timeout = timeout;
        }

        private void CommonVoidCommand(string commandName,
            int commandNumber,
            ParametersValidation validationRequest, bool sendNotifications,
            out StepType stepType, out SoapException ex, out int Timeout, out int special)
        {
            StepType st;
            SoapException exception;
            int timeout;
            int localSpecial = 0;

            ExecuteCommand(commandName, commandNumber, validationRequest, sendNotifications, out st, out exception, out timeout,
                new Func<XmlNode, StepType>((test) => { return TestCommon.GenerateResponseStepTypeVoid(test, out exception, out timeout, out localSpecial); }));
            
            special = localSpecial;
            stepType = st;
            ex = exception;
            Timeout = timeout;
        }

        private void ExecuteCommand(string commandName,
            int commandNumber,
            ParametersValidation validationRequest,
            bool sendNotifications,
            out StepType stepType, out SoapException ex, out int timeout,
            Func<XmlNode, StepType> parseStep)
        {
            StepType res = StepType.None;
            timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandNumber]];

                #region Analyze request

                //Parameters
                if (validationRequest != null)
                {
                    ValidateParameters(validationRequest, ref logMessage, ref passed, test);
                }

                #endregion //Analyze request
                //Generate response
                res = parseStep(test);

                if (sendNotifications)
                {
                    TestCommon.SendNotifications(test, ref logMessage, ref passed, TestCommon.LastNotificationAddress);
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            stepType = res;
        }

        #endregion

        void ValidateParameters(Common.ParametersValidation validationRequest, ref string logMessage, ref bool passed, XmlNode test)
        {
            foreach (Common.ValidationRule rule in validationRequest.ValidationRules)
            {
                string parameterPath = string.Format("RequestParameters/{0}", rule.ParameterPath);

                switch (rule.Type)
                { 
                    case ParameterType.String:
                        CommonCompare.StringCompare(parameterPath, rule.ParameterName, (string)rule.Value, 
                            ref logMessage, ref passed, test);
                        break;
                    case ParameterType.Int:
                        CommonCompare.IntCompare(parameterPath, rule.ParameterName, (int)rule.Value,
                            ref logMessage, ref passed, test);
                        break;
                    case ParameterType.OptionalInt:
                        int? value = (int?)rule.Value;
                        if (CommonCompare.Exist2(parameterPath, rule.ParameterName, value.HasValue, ref logMessage, ref passed, test))
                        {
                            CommonCompare.IntCompare(parameterPath, rule.ParameterName, value.Value, ref logMessage, ref passed, test);
                        }
                        break;
                    case ParameterType.OptionalBool:
                        if (rule.ValueSpecified)
                        {
                            CommonCompare.StringCompare(parameterPath, rule.ParameterName, ((bool)rule.Value).ToString(),
                            ref logMessage, ref passed, test);
                        }
                        else
                        {
                            CommonCompare.Exist2(parameterPath, rule.ParameterName, rule.ValueSpecified,
                            ref logMessage, ref passed, test);
                        }
                        break;
                    case ParameterType.OptionalString:
                        {
                            string stringValue = (string)rule.Value;
                            if (CommonCompare.Exist2(parameterPath, rule.ParameterName, stringValue != null, ref logMessage, ref passed, test))
                            {
                                CommonCompare.StringCompare(parameterPath, rule.ParameterName, (string)rule.Value,
                                    ref logMessage, ref passed, test);
                            }
                        }
                        break;
                    case ParameterType.OptionalElement:
                        {
                            CommonCompare.Exist2(parameterPath, rule.ParameterName, rule.Value != null, ref logMessage, ref passed, test);
                        }
                        break;
                    case ParameterType.OptionalElementBoolFlag:
                        {
                            CommonCompare.Exist2(parameterPath, rule.ParameterName, (bool)rule.Value, ref logMessage, ref passed, test);
                        }
                        break;
                    case ParameterType.StringArray:
                        {
                            CommonCompare.StringArrayCompare(parameterPath, rule.ParameterName, (string[])rule.Value,
                                ref logMessage, ref passed, test);
                        }
                        break;
                    case ParameterType.Log:
                        {
                            if (rule.Value.GetType().ToString() == "System.Byte[]")
                            {
                                logMessage = logMessage + rule.ParameterName + " = [Check it manually!]";
                            }
                            else
                            {
                                logMessage = logMessage + rule.ParameterName + " = " + rule.Value.ToString();
                            }
                        }
                        break;
                    case ParameterType.OptionalQName:
                        {
                            XmlQualifiedName QNameValue = (XmlQualifiedName)rule.Value;
                            if (CommonCompare.Exist2(parameterPath, rule.ParameterName, QNameValue != null, ref logMessage, ref passed, test))
                            {
                                CommonCompare.StringCompare(parameterPath + "/Namespace", rule.ParameterName + "/Namespace", QNameValue.Namespace,
                                    ref logMessage, ref passed, test);
                                CommonCompare.StringCompare(parameterPath + "/Name", rule.ParameterName + "/Name", QNameValue.Name,
                                    ref logMessage, ref passed, test);
                            }
                        }
                        break;
                    case ParameterType.X509Cert:
                        {
                            Org.BouncyCastle.X509.X509Certificate cert = (new Org.BouncyCastle.X509.X509CertificateParser()).ReadCertificate((byte[])rule.Value);
                            logMessage = logMessage + "\r\n";
                            logMessage = logMessage + rule.ParameterName + ": " + "\r\n" + cert.ToString();
                            logMessage = logMessage + "\r\n";
                            //TextWriter textWriter = new StringWriter();
                            //Org.BouncyCastle.Utilities.IO.Pem.PemWriter pemWriter = new Org.BouncyCastle.Utilities.IO.Pem.PemWriter(textWriter);
                            //pemWriter.WriteObject(cert.CertificateStructure.SubjectPublicKeyInfo.PublicKeyData);
                            //pemWriter.Writer.Flush();

                            //string privateKey = textWriter.ToString();
                            logMessage = logMessage + rule.ParameterName + "(PubK): " + "\r\n" + cert.CertificateStructure.SubjectPublicKeyInfo.PublicKeyData.ToString();
                            logMessage = logMessage + "\r\n";
                            logMessage = logMessage + rule.ParameterName + "(SignatureAlgorithm): " + "\r\n" + cert.CertificateStructure.SignatureAlgorithm.ObjectID.ToString();
                            logMessage = logMessage + "\r\n";
                        }
                        break;
                    case ParameterType.PKCS10:
                        {
                            Org.BouncyCastle.Pkcs.Pkcs10CertificationRequest pkcs10 = new Org.BouncyCastle.Pkcs.Pkcs10CertificationRequest((byte[])rule.Value);
                            logMessage = logMessage + "\r\n";
                            logMessage = logMessage + rule.ParameterName + ": " + "\r\n" + pkcs10.ToString();
                            logMessage = logMessage + "\r\n";
                        }
                        break;
                    case ParameterType.PKCS12WithoutPassphrase:
                        {
                            Org.BouncyCastle.Pkcs.Pkcs12Store pkcs12Store = new Org.BouncyCastle.Pkcs.Pkcs12Store();
                            pkcs12Store.Load(new MemoryStream((byte[])rule.Value), ("").ToArray());

                            logMessage = logMessage + rule.ParameterName + ": " + "\r\n";


                            foreach (string alias in pkcs12Store.Aliases)
                            {
                                logMessage = logMessage + "\r\n";
                                logMessage = logMessage + "Alias = " + alias + "\r\n";
                                logMessage = logMessage + "Certificate = " + pkcs12Store.GetCertificate(alias).Certificate.ToString();
                            }

                            logMessage = logMessage + "\r\n";
                        }
                        break;
                    case ParameterType.CRL:
                        {
                            Org.BouncyCastle.X509.X509Crl crl = (new Org.BouncyCastle.X509.X509CrlParser()).ReadCrl((byte[])rule.Value);
                            logMessage = logMessage + "\r\n";
                            logMessage = logMessage + rule.ParameterName + ": " + "\r\n" + crl.ToString();
                            logMessage = logMessage + "\r\n";
                        }
                        break;
                    case ParameterType.Float:
                        {
                            CommonCompare.FloatCompare(parameterPath, rule.ParameterName, (float)rule.Value,
                            ref logMessage, ref passed, test);
                        }
                        break;
                    case ParameterType.OptionalFloat:
                        float? fvalue = (float?)rule.Value;
                        if (CommonCompare.Exist2(parameterPath, rule.ParameterName, fvalue.HasValue, ref logMessage, ref passed, test))
                        {
                            CommonCompare.FloatCompare(parameterPath, rule.ParameterName, (float)rule.Value,
                            ref logMessage, ref passed, test);
                        }
                        break;
                }
            
            }
        
        }

        public T TakeSpecialParameter<T>(string commandName, int commandNumber, string name)
            where T : class
        {
            Type t = typeof(T);

            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);
            int lastCommandNumber = CommandCount[commandNumber];

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[lastCommandNumber];
                XmlNodeList nodes = test.SelectNodes(string.Format("ResponseParametersAdditional/{0}", name));

                foreach (XmlNode node in nodes)
                {
                    XmlElement elem = node as XmlElement;
                    if (elem != null)
                    {
                        try
                        {
                            XmlSerializer serializer = new XmlSerializer(t);
                            XmlReader sr = new  XmlTextReader(new StringReader(elem.OuterXml));
                            object target = serializer.Deserialize(sr);
                            return target as T;
                        }
                        catch (Exception exc)
                        {
                            throw exc;
                        }
                    }
                }
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            
            return null;
        }

        public T TakeSpecialParameter<T>(string commandName, int commandNumber, string name, string flagName, out string flagValue)
    where T : class
        {
            Type t = typeof(T);

            flagValue = "";

            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);
            int lastCommandNumber = CommandCount[commandNumber];

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[lastCommandNumber];
                XmlNodeList nodes = test.SelectNodes(string.Format("ResponseParametersAdditional/{0}", name));

                foreach (XmlNode node in nodes)
                {
                    XmlElement elem = node as XmlElement;
                    if (elem != null)
                    {
                        try
                        {
                            if (test.SelectNodes(string.Format("ResponseParametersAdditional/@{0}", flagName)).Count != 0)
                            {
                                flagValue = test.SelectSingleNode(string.Format("ResponseParametersAdditional/@{0}", flagName)).InnerText;
                            }
                            

                            XmlSerializer serializer = new XmlSerializer(t);
                            XmlReader sr = new XmlTextReader(new StringReader(elem.OuterXml));
                            object target = serializer.Deserialize(sr);
                            return target as T;
                        }
                        catch (Exception exc)
                        {
                            throw exc;
                        }
                    }
                }
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }

            return null;
        }

        public void TakeSpecialParameterFlag(string commandName, int commandNumber, string name, string flagName, out string flagValue)
        {
            flagValue = "";

            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);
            int lastCommandNumber = CommandCount[commandNumber];

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[lastCommandNumber];
                XmlNodeList nodes = test.SelectNodes(string.Format("ResponseParametersAdditional/{0}", name));

                foreach (XmlNode node in nodes)
                {
                    XmlElement elem = node as XmlElement;
                    if (elem != null)
                    {
                        try
                        {
                            if (test.SelectNodes(string.Format("ResponseParametersAdditional/{0}/@{1}", name, flagName)).Count != 0)
                            {
                                flagValue = test.SelectSingleNode(string.Format("ResponseParametersAdditional/{0}/@{1}", name, flagName)).InnerText;
                            }

                        }
                        catch (Exception exc)
                        {
                            throw exc;
                        }
                    }
                }
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }

        }


        public T TakeSpecialParameterSimple<T>(string commandName, int commandNumber, string name)
    where T : class
        {
            Type t = typeof(T);

            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);
            int lastCommandNumber = CommandCount[commandNumber];

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[lastCommandNumber];
                XmlNodeList nodes = test.SelectSingleNode(string.Format("ResponseParametersAdditional/{0}", name)).ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    if (node != null)
                    {
                        try
                        {
                            return node.Value as T;
                        }
                        catch (Exception exc)
                        {
                            throw exc;
                        }
                    }
                }
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }

            return null;
        }

        public Dictionary<string, string> TakePrefixes(string commandName, int commandNumber)
        {
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);
            int lastCommandNumber = CommandCount[commandNumber];
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[lastCommandNumber];
                XmlNode prefixNode = test.SelectSingleNode(string.Format("ResponseParametersAdditional/{0}", "Prefixes"));

                if (prefixNode != null)
                {
                    foreach (XmlNode node in prefixNode.ChildNodes)
                    {
                        XmlElement elem = node as XmlElement;
                        if (elem != null)
                        {
                            try
                            {
                                result.Add(elem.Name, elem.InnerText);
                            }
                            catch (Exception exc)
                            {
                                throw exc;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }

            return result;
        }
    }
}
