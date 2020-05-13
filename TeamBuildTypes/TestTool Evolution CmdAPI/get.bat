@echo %1
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2\SampleApp
del /S /F /Q "%~1\TestTool\*"
del /S /F /Q "%~1\External\*"
rem cd E:\onvifSVN\Source\SampleApp
rem svn revert -R E:\onvifSVN\Source\SampleApp\
rem svn checkout --force -R E:\onvifSVN\Source\SampleApp\

svn cleanup "E:\onvifSVN\Src\dev branches\Evolution\TestTool_API"
svn cleanup "E:\\onvifSVN\\Test Specifications"

svn revert -R "E:\onvifSVN\Src\dev branches\Evolution\TestTool_API"
svn revert -R "E:\\onvifSVN\\Test Specifications"

svn update --force "E:\onvifSVN\Src\dev branches\Evolution\TestTool_API"
svn update --force "E:\\onvifSVN\\Test Specifications"

xcopy /R /E /Y "E:\onvifSVN\Src\dev branches\Evolution\TestTool_API" "%~1\"
xcopy /R /E /Y "E:\\onvifSVN\\Test Specifications" "%~1\Test Specifications\Main\"
