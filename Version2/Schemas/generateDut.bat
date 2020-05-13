REM wsdl.exe  imaging.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:serverImaging
REM wsdl.exe  devicemgmt.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:serverDM

wsdl.exe  ptz.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:serverPTZ

wsdl.exe  media.wsdl ws-addr.xsd onvif.xsd b-2.xsd bf-2.xsd t-1.xsd xmlmime.xsd include.xsd /server /out:serverMedia
