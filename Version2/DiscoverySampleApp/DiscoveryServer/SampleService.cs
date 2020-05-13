using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace DiscoveryServer
{
    [ServiceContract(Namespace = "http://Microsoft.Samples.Discovery")]
    public interface ISampleService
    {
        [OperationContract]
        void HelloWorld();
    }

    class SampleService  : ISampleService
    {
        public void HelloWorld()
        { 
        }
    }

}
