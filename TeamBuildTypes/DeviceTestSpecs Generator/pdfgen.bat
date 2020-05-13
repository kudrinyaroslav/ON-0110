@echo %1
@echo %2
del /S /F /Q "%~1\\*"
svn update "E:\\onvifSVN\\Test Specifications"
xcopy /i /R /E "E:\\onvifSVN\\Test Specifications" "%~1"
set current_dir=%cd%
cd %FOP%
for %%f in (%1\*.xml) do (
	echo "%%f"
	echo "%~1\ONVIFNew-stylesheets\onvif-specification-fo-us.xsl"
	echo "%~1\%%~nf.pdf"
	java.exe -jar fop.jar -c "%~1\ONVIFNew-stylesheets\fop.xconf" -xml "%%f" -xsl "%~1\ONVIFNew-stylesheets\onvif-specification-fo-us.xsl" -pdf "%~1\%%~nf.pdf"
)
cd %current_dir%
xcopy /i /R "%~1\*.pdf" "%~2"
exit 0
