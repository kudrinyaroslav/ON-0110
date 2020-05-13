@echo %1
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2
rem md C:\Builds\3\ON-0110\SampleApp\src\Version2\SampleApp
del /S /F /Q "%~1\\*"
rem del /S /F /Q %1\Version2\External\*
rem cd E:\onvifSVN\Source\SampleApp
rem svn revert -R E:\onvifSVN\Source\SampleApp\
rem svn checkout --force -R E:\onvifSVN\Source\SampleApp\
rem svn revert -R E:\onvifSVN\Source\DTT_Src\
svn update E:\onvifSVN3\
xcopy /i /R /E "E:\\onvifSVN3\\Project Cuervo\\trunk" "%~1"