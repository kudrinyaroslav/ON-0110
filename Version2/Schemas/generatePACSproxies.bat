
REM DoorControl, Access

svcutil.exe doorcontrol_api.txt.wsdl pacs_api.txt.wsdl pacstypes.xsd  /noconfig /directory:all /out:pacs.cs

REM doorcontrol_api.txt.xsd pacs_api.txt.xsd