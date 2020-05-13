///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport
{
    using System.Collections.Generic;
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
        ControlledTextMessageBindingElement _encoding;

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
            List<IChannelController> transportControllers = new List<IChannelController>();
            List<IChannelController> encoderControllers = new List<IChannelController>();

            foreach (IChannelController controller in _controllers)
            {
                ITransportController transportController = controller as ITransportController;
                IEncodingController encodingController = controller as IEncodingController;

                if (transportController != null)
                {
                    transportControllers.Add(transportController);
                }
                if (encodingController != null)
                {
                    encoderControllers.Add(encodingController);
                }
            }

            _transport = new HttpTransportBindingElement(transportControllers);
            _encoding = new ControlledTextMessageBindingElement();
            _encoding.AddControllers(encoderControllers);
        }

    }
}
