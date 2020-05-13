using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport
{
    public class BindingFactory
    {
        public static HttpBinding CreateBinding(string deviceEntryPoint, IEnumerable<IChannelController> controllers)
        {
            if (deviceEntryPoint.StartsWith("https://"))
                return new HttpsBinding(controllers);

            return new HttpBinding(controllers);
        }
    }
}
