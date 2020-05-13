
REM DoorControl, Access

svcutil.exe doorcontrol_api.txt.wsdl pacs_api.txt.wsdl pacstypes.xsd   /wrapped /noconfig /directory:wrapped /out:pacs.cs

