@echo %1
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2\SampleApp
del /S /F /Q "%~1\\*"
rem del /S /F /Q %1\Version2\External\*
rem cd E:\onvifSVN\Source\SampleApp
rem svn revert -R E:\onvifSVN\Source\SampleApp\
rem svn checkout --force -R E:\onvifSVN\Source\SampleApp\
rem svn revert -R E:\onvifSVN\Source\DTT_Src\

svn cleanup E:\onvifSVN3

svn update E:\onvifSVN3

md "%~1\CTT_Src"
md "%~1\CTT_Src\ClientTestTool"

xcopy /i /R /E "E:\onvifSVN3\CTT_Src\branches\17.06 - Frangelico" "%~1\CTT_Src\ClientTestTool"

md "%~1\Test Specifications"
md "%~1\Test Specifications\Main"

xcopy /i /R /E "E:\onvifSVN3\Test Specifications\Main" "%~1\Test Specifications\Main"

