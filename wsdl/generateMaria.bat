
REM without events
REM Media, Device, DeviceIO, Imaging, PTZ
REM direction for future improvement: analytics etc.

svcutil.exe /async accesscontrol.wsdl doorcontrol.wsdl accessrules.wsdl credential.wsdl onvif.xsd ws-addr.xsd xmlmime.xsd  b-2.xsd bf-2.xsd t-1.xsd include.xsd envelope.xsd types.xsd /noconfig /directory:maria /out:onvif.cs
svcutil.exe /async ConfigurationService.asmx.wsdl /directory:maria /out:ConfigurationService.cs

