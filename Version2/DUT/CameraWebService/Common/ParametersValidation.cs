using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.CameraWebService.Common
{
    public enum ParameterType
    { 
        String,
        OptionalElement,
        OptionalElementBoolFlag,
        OptionalString,
        OptionalBool,
        Int,
        OptionalInt,
        StringArray,
        OptionalQName,
        X509Cert,
        PKCS12WithoutPassphrase,
        PKCS10,
        CRL,
        Log,
        Float,
        OptionalFloat
    }

    public class ParametersValidation
    {
        private List<ValidationRule> _rules = new List<ValidationRule>();

        public List<ValidationRule> ValidationRules
        {
            get { return _rules; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type to specify comparison</param>
        /// <param name="name">Parameter Name for log. Will be used also as parameter path.</param>
        /// <param name="value">Value for comparison.</param>
        public void Add(ParameterType type, string name, object value)
        {
            ValidationRule rule = new ValidationRule() { Type = type, ParameterName = name, ParameterPath = name, Value = value };
            _rules.Add(rule);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type to specify comparison</param>
        /// <param name="path"></param>
        /// <param name="name">Parameter Name for log. Will be used also as parameter path.</param>
        /// <param name="value">Value for comparison.</param>
        public void Add(ParameterType type, string path, string name, object value)
        {
            ValidationRule rule = new ValidationRule() { Type = type, ParameterName = name, ParameterPath = path, Value = value };
            _rules.Add(rule);       
        }

        public void Add(ParameterType type, string name, object value, bool valueSpecified)
        {
            ValidationRule rule = new ValidationRule() { Type = type, ParameterName = name, ParameterPath = name, Value = value, ValueSpecified = valueSpecified };
            _rules.Add(rule);
        }

    }

    public class ValidationRule
    {
        public ParameterType Type { get; set; }
        public string ParameterName { get; set; }
        public string ParameterPath { get; set; }
        public object Value { get; set; }
        public bool ValueSpecified { get; set; }
    }
}
