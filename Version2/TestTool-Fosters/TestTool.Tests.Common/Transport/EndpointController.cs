///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.ServiceModel;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.Tests.Common.Transport
{
    /// <summary>
    /// Holds endpoint information
    /// </summary>
    public class EndpointController : IEndpointController
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

        private bool _wsaEnabled;
        public bool WsaEnabled
        {
            get { return _wsaEnabled; }
            set { _wsaEnabled = value; }
        }

        #endregion


    }
}
