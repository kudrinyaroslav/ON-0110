///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
namespace TestTool.HttpTransport
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    /// <summary>
    /// Binding for custom Http. 
    /// </summary>
    public class HttpBinding : Binding
    {
        // private BindingElements

        /// <summary>
        ///  "Our" transport layer
        /// </summary>
        HttpTransportBindingElement _transport;
        /// <summary>
        /// Standard message encoder.
        /// </summary>
        MessageEncodingBindingElement _encoding;

        /// <summary>
        /// Channel controllers
        /// </summary>
        private List<IChannelController> _controllers;

        public HttpBinding()
            : this(null)
        {
        }

        /// <summary>
        /// Creates binding with controllers added.
        /// </summary>
        /// <param name="elements"></param>
        public HttpBinding(IEnumerable<IChannelController> elements)
        {
            _controllers = new List<IChannelController>(elements);
            Initialize();
        }

        /// <summary>
        /// Scheme
        /// </summary>
        public override string Scheme { get { return "http"; } }

        /// <summary>
        /// Soap version
        /// </summary>
        public EnvelopeVersion SoapVersion
        {
            get { return EnvelopeVersion.Soap12; }
        }

        /// <summary>
        /// Create the set of binding elements that make up this binding. 
        /// NOTE: order of binding elements is important.
        /// </summary>
        /// <returns></returns>
        public override BindingElementCollection CreateBindingElements()
        {   
            BindingElementCollection bindingElements = new BindingElementCollection();

            // only two binding elements are used.
            bindingElements.Add(_encoding);
            bindingElements.Add(_transport);

            return bindingElements.Clone();
        }
        
        /// <summary>
        /// Initializes binding elements.
        /// </summary>
        void Initialize()
        {
            _transport = new HttpTransportBindingElement(_controllers);
            _encoding = new TextMessageEncodingBindingElement();
        }

    }
}
