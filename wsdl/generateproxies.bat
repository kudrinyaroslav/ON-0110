
svcutil.exe /async devicemgmt.wsdl onvif.xsd xmlmime.xsd include.xsd ws-addr.xsd b-2.xsd bf-2.xsd t-1.xsd envelope.xsd   /config:devicemgmt\devicemgmt.config /directory:devicemgmt

REM -----------------

svcutil.exe /async media.wsdl onvif.xsd ws-addr.xsd xmlmime.xsd b-2.xsd include.xsd bf-2.xsd t-1.xsd envelope.xsd /config:media\media.config /directory:media

REM -----------------

svcutil.exe /async ptz.wsdl onvif.xsd ws-addr.xsd xmlmime.xsd b-2.xsd include.xsd bf-2.xsd t-1.xsd envelope.xsd /config:ptz\ptz.config /directory:ptz