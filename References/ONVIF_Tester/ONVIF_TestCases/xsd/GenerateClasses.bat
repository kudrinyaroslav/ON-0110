rem use this batch file to generate the xml classes required for version 1.0 test specification.

set XML_CONVERTER="C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\XSD.exe"

%XML_CONVERTER% import/addressing.xsd import/ws-discovery.xsd import/xmlmime.xsd import/b-2.xsd import/bf-2.xsd import/ws-addr.xsd import/t-1.xsd import/xop.xsd onvif.xsd devicemgmt.xsd /c /n:DeviceManagement

%XML_CONVERTER% import/addressing.xsd import/ws-discovery.xsd import/xmlmime.xsd import/b-2.xsd import/bf-2.xsd import/ws-addr.xsd import/t-1.xsd import/xop.xsd onvif.xsd remotediscovery.xsd /c /n:RemoteDiscovery

%XML_CONVERTER% import/addressing.xsd import/ws-discovery.xsd import/xmlmime.xsd import/b-2.xsd import/bf-2.xsd import/ws-addr.xsd import/t-1.xsd import/xop.xsd onvif.xsd media.xsd /c /n:Media

%XML_CONVERTER% ../../Tests/Test.xsd /c /n:TestSchema_Class
