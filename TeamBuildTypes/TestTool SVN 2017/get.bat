@echo %1
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2\SampleApp
del /S /F /Q "%~1\TestTool\*"
del /S /F /Q "%~1\External\*"
rem cd E:\onvifSVN\Source\SampleApp
rem svn revert -R E:\onvifSVN\Source\SampleApp\
rem svn checkout --force -R E:\onvifSVN\Source\SampleApp\

svn cleanup E:\onvifSVN\Src\External
svn cleanup E:\onvifSVN\Src\TestTool
svn cleanup "E:\\onvifSVN\\Test Specifications"

svn revert -R E:\onvifSVN\Src\External
svn revert -R E:\onvifSVN\Src\TestTool
svn revert -R "E:\\onvifSVN\\Test Specifications"

svn update --force E:\onvifSVN\Src\External
svn update --force E:\onvifSVN\Src\TestTool
svn update --force "E:\\onvifSVN\\Test Specifications"

xcopy /R /E /Y E:\onvifSVN\Src\External "%~1\External\"
xcopy /R /E /Y E:\onvifSVN\Src\TestTool "%~1\TestTool\"
xcopy /R /E /Y "E:\\onvifSVN\\Test Specifications" "%~1\Test Specifications\Main\"