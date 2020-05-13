REM wsdl.exe  imaging.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:serverImaging
rem wsdl.exe  devicemgmt.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:server/DeviceService.cs

rem wsdl.exe  ptz.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:serverPTZ

rem wsdl.exe  media.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:serverMedia

wsdl.exe advancedsecurity.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd envelope.xsd /server /out:server/advancedsecurity.cs
wsdl.exe accessrules.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd envelope.xsd types.xsd /server /out:server/accessrules.cs
wsdl.exe credential.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd envelope.xsd types.xsd /server /out:server/credential.cs
wsdl.exe schedule.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd envelope.xsd types.xsd /server /out:server/schedule.cs
wsdl.exe event.wsdl bw-2.wsdl b-2.xsd ws-addr.xsd t-1.xsd bf-2.xsd rw-2.wsdl r-2.xsd /server /out:server/event.cs 
