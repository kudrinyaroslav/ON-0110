using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.CameraWebService
{
    public class Script
    {
        Dictionary<string, Dictionary<int, string>> _responses = new Dictionary<string, Dictionary<int, string>>();

        private bool _initialized = false;

        void InitResponses()
        {
            Dictionary<int, string> capabilities = new Dictionary<int, string>();
            _responses.Add("capabilities", capabilities);

            capabilities.Add(1, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><Capabilities><Device xmlns=\"http://www.onvif.org/ver10/schema\" /><Events xmlns=\"http://www.onvif.org/ver10/schema\"><WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport><WSPullPointSupport>false</WSPullPointSupport><WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport></Events><Media xmlns=\"http://www.onvif.org/ver10/schema\" /></Capabilities></GetCapabilitiesResponse></soap:Body></soap:Envelope>");
            capabilities.Add(2, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><Capabilities><Events xmlns=\"http://www.onvif.org/ver10/schema\"><WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport><WSPullPointSupport>false</WSPullPointSupport><WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport></Events><Media xmlns=\"http://www.onvif.org/ver10/schema\" /></Capabilities></GetCapabilitiesResponse></soap:Body></soap:Envelope>");
            capabilities.Add(3, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><soap:Fault><soap:Code><soap:Value>soap:Sender</soap:Value></soap:Code><soap:Reason><soap:Text xml:lang=\"en\"></soap:Text></soap:Reason><soap:Detail /></soap:Fault></soap:Body></soap:Envelope>");
        
            Dictionary<int, string> hostname = new Dictionary<int, string>();
            _responses.Add("hostname", hostname);
            hostname.Add(1, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHostnameResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><HostnameInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">true</FromDHCP><Name xmlns=\"http://www.onvif.org/ver10/schema\">Hostname</Name></HostnameInformation></GetHostnameResponse></soap:Body></soap:Envelope>");
            hostname.Add(2, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHostnameResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><HostnameInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">true</FromDHCP><Name xmlns=\"http://www.onvif.org/ver10/schema\">-Hostname</Name></HostnameInformation></GetHostnameResponse></soap:Body></soap:Envelope>");
            hostname.Add(3, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHostnameResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><HostnameInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">true</FromDHCP><Name xmlns=\"http://www.onvif.org/ver10/schema\">Hostname-</Name></HostnameInformation></GetHostnameResponse></soap:Body></soap:Envelope>");
            hostname.Add(4, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHostnameResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><HostnameInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">true</FromDHCP><Name xmlns=\"http://www.onvif.org/ver10/schema\">Host-name</Name></HostnameInformation></GetHostnameResponse></soap:Body></soap:Envelope>");
            hostname.Add(5, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHostnameResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><HostnameInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">true</FromDHCP><Name xmlns=\"http://www.onvif.org/ver10/schema\">Host.name</Name></HostnameInformation></GetHostnameResponse></soap:Body></soap:Envelope>");
            hostname.Add(6, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHostnameResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><HostnameInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">true</FromDHCP><Name xmlns=\"http://www.onvif.org/ver10/schema\">1Hostname</Name></HostnameInformation></GetHostnameResponse></soap:Body></soap:Envelope>");
            hostname.Add(7, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHostnameResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><HostnameInformation><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">true</FromDHCP><Name xmlns=\"http://www.onvif.org/ver10/schema\">12345678</Name></HostnameInformation></GetHostnameResponse></soap:Body></soap:Envelope>");
        
            Dictionary<int, string> getDnsInformation = new Dictionary<int, string>();
            getDnsInformation.Add(1, "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetDNSResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><DNSInformation info=\"some data\"><FromDHCP xmlns=\"http://www.onvif.org/ver10/schema\">false</FromDHCP><DNSManual xmlns=\"http://www.onvif.org/ver10/schema\"><Type>IPv4</Type><IPv4Address>10.1.1.10</IPv4Address></DNSManual></DNSInformation></GetDNSResponse></soap:Body></soap:Envelope>");

            _responses.Add("getDnsInformation", getDnsInformation);
        }

        public string GetResponse(string script, int step)
        {
            if (!_initialized)
            {
                InitResponses();
                _initialized = true;
            }

            if (_responses.ContainsKey(script))
            {
                if (_responses[script].ContainsKey(step))
                {
                    return _responses[script][step];
                }
            }
            return null;
        }

    }
}
