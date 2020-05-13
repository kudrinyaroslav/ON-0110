
REM without events
REM Media, Device, DeviceIO, Imaging, PTZ
REM direction for future improvement: analytics etc.

svcutil.exe /async devicemgmt.wsdl media.wsdl deviceio.wsdl imaging.wsdl ptz.wsdl onvif.xsd ws-addr.xsd xmlmime.xsd  b-2.xsd bf-2.xsd t-1.xsd include.xsd /noconfig /directory:all /out:onvif.cs

