
REM without events
REM Media, Device, DeviceIO, Imaging, PTZ
REM direction for future improvement: analytics etc.

svcutil.exe /async /serializer:XmlSerializer devicemgmt.wsdl media.wsdl deviceio.wsdl advancedsecurity.wsdl accessrules.wsdl credential.wsdl schedule.wsdl thermal.wsdl common.xsd onvif.xsd ws-addr.xsd xmlmime.xsd b-2.xsd bf-2.xsd t-1.xsd include.xsd envelope2003.xsd types.xsd /noconfig /directory:all /out:onvif1.cs
svcutil.exe /async /serializer:XmlSerializer imaging.wsdl ptz.wsdl media2.wsdl analytics.wsdl common.xsd onvif.xsd ws-addr.xsd xmlmime.xsd  b-2.xsd bf-2.xsd t-1.xsd include.xsd envelope2003.xsd types.xsd rules.xsd radiometry.xsd /noconfig /directory:all /out:onvif2.cs
svcutil.exe /async /serializer:XmlSerializer replay.wsdl receiver.wsdl search.wsdl recording.wsdl common.xsd onvif.xsd b-2.xsd xmlmime.xsd include.xsd ws-addr.xsd bf-2.xsd envelope2003.xsd t-1.xsd /noconfig /directory:all /out:storage.cs
svcutil.exe event.wsdl bw-2.wsdl b-2.xsd ws-addr.xsd t-1.xsd bf-2.xsd rw-2.wsdl r-2.xsd /UseSerializerForFaults /config:"event.config" /directory:all  

rem svcutil.exe /async imaging.wsdl onvif.xsd ws-addr.xsd xmlmime.xsd  b-2.xsd bf-2.xsd t-1.xsd include.xsd envelope2003.xsd types.xsd /noconfig /directory:all /out:imaging.cs
rem xsd /c onvif.xsd b-2.xsd include.xsd envelope2003.xsd xmlmime.xsd t-1.xsd bf-2.xsd ws-addr.xsd /out:all


