///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.ServiceModel;

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Holds endpoint information
    /// </summary>
    public class EndpointController : HttpTransport.IEndpointController
    {
        public EndpointController()
        {

        }

        public EndpointController(EndpointAddress address)
        {
            _address = address;
        }

        public void UpdateAddress(EndpointAddress address)
        {
            _address = address;
        }

        private EndpointAddress _address;        
        
        #region IAddressProvider Members
        
        public EndpointAddress Address
        {
            get { return _address; }
        }

        #endregion

        #region IAddressProvider Members

        private bool _wsaEnabled;
        public bool WsaEnabled
        {
            get { return _wsaEnabled; }
            set { _wsaEnabled = value; }
        }

        #endregion
    }
}
