using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;

namespace TestTool.GUI.Utils.ServiceProviders
{
    class CredentialServiceProvider : BaseServiceProvider<CredentialPortClient, CredentialPort>
    {
        public CredentialServiceProvider(string serviceAddress, int messageTimeout): base(serviceAddress, messageTimeout)
        {
            EnableLogResponse = false;
        }

        public override CredentialPortClient CreateClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress address)
        {
            return new CredentialPortClient(binding, address);
        }

        public List<string> GetSupportedCredentialIdentifierTypes()
        {
            CredentialServiceCapabilities serviceCapabilities = null;

            if (Security == Security.None)
            {
                var action = ConstructSecurityTolerantAction(() => serviceCapabilities = Client.GetServiceCapabilities());

                action();
            }
            else
                serviceCapabilities = Client.GetServiceCapabilities();

            if (null != serviceCapabilities)
                return serviceCapabilities.SupportedIdentifierType.ToList();

            return new List<string>();
        }

        public List<string> GetSupportedCredentialIdentifierFormatTypes(string typeName)
        {
            CredentialIdentifierFormatTypeInfo[] types = null;

            if (Security == Security.None)
            {
                var action = ConstructSecurityTolerantAction(() => types = Client.GetSupportedFormatTypes(typeName));

                action();
            }
            else
                types = Client.GetSupportedFormatTypes(typeName);

            if (null != types)
                return types.Select(e => e.FormatType).ToList();

            return new List<string>();
        }
    }
}
