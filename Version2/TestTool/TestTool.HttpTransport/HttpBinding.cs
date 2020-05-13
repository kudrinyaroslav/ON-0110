///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using System.Linq;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    /// <summary>
    /// Binding for custom Http. 
    /// </summary>
    public class HttpBinding: Binding
    {
        // private BindingElements

        /// <summary>
        ///  "Our" transport layer
        /// </summary>
        TransportBindingElement _transport;
        public virtual TransportBindingElement Transport
        {
            get
            {
                if (null == _transport)
                    _transport = new HttpTransportBindingElement(_controllers.Where(c => c is ITransportController));

                return _transport;
            }
        }
        /// <summary>
        /// Standard message encoder.
        /// </summary>
        ControlledTextMessageBindingElement _encoding;
        public virtual ControlledTextMessageBindingElement Encoding
        {
            get
            {
                if (null == _encoding)
                {
                    _encoding = new ControlledTextMessageBindingElement();
                    _encoding.AddControllers(_controllers.Where(c => c is IEncodingController));
                }

                return _encoding;
            }
        }

        /// <summary>
        /// Channel controllers
        /// </summary>
        private List<IChannelController> _controllers;
        public List<IChannelController> Controllers
        {
            get { return _controllers; }
        }

        public HttpBinding(): this(null)
        {}

        /// <summary>
        /// Creates binding with controllers added.
        /// </summary>
        /// <param name="elements"></param>
        public HttpBinding(IEnumerable<IChannelController> elements)
        {
            _controllers = new List<IChannelController>(elements);
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
            var bindingElements = new BindingElementCollection();

            // only two binding elements are used.
            bindingElements.Add(Encoding);
            bindingElements.Add(Transport);

            return bindingElements.Clone();
        }
    }
}
