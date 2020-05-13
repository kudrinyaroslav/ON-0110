rd /S /Q External\build
del /Q External\live\build\*.user

del /S /Q TestTool\*.user
del /S /Q TestTool\*.vspscc
del /S /Q TestTool\*.vssscc
del /S /Q TestTool\*.suo
del /S /Q TestTool\*.aps

rd /S /Q TestTool\Debug
rd /S /Q TestTool\Release
rd /S /Q TestTool\SourceFilter\Debug
rd /S /Q TestTool\SourceFilter\Release
del /Q TestTool\SourceFilter\*.c
del /Q TestTool\SourceFilter\*_h.h

rd /S /Q TestTool\TestTool.Tests.Common\bin
rd /S /Q TestTool\TestTool.Tests.Common\obj

rd /S /Q TestTool\TestTool.GUI\bin
rd /S /Q TestTool\TestTool.GUI\obj

rd /S /Q TestTool\TestTool.HttpTransport\bin
rd /S /Q TestTool\TestTool.HttpTransport\obj

rd /S /Q TestTool\TestTool.Tests.Proxies\bin
rd /S /Q TestTool\TestTool.Tests.Proxies\obj

rd /S /Q TestTool\TestTool.Tests.TestCases\bin
rd /S /Q TestTool\TestTool.Tests.TestCases\obj

rd /S /Q TestTool\TestTool.Setup\Debug
rd /S /Q TestTool\TestTool.Setup\Release 
rd /S /Q "TestTool\TestTool.Setup\Release - all"
rd /S /Q "TestTool\TestTool.Setup\Release - Full"

rd /S /Q External\live
del /Q TestTool\Lib\live.lib
del /Q TestTool\Lib\debug\live.lib
