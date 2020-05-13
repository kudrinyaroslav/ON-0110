
REM without events
REM Media, Device, DeviceIO, Imaging, PTZ
REM direction for future improvement: analytics etc.

svcutil.exe /async /serializer:XmlSerializer replay.wsdl receiver.wsdl search.wsdl recording.wsdl onvif.xsd b-2.xsd xmlmime.xsd include.xsd ws-addr.xsd bf-2.xsd envelope2003.xsd t-1.xsd /noconfig /directory:all /out:storage.cs
rem svcutil.exe /async replay.wsdl receiver.wsdl search.wsdl recording.wsdl onvif.xsd b-2.xsd xmlmime.xsd include.xsd ws-addr.xsd bf-2.xsd envelope2003.xsd t-1.xsd /noconfig /directory:all /out:storage1.cs

