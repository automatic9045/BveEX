
mkdir "bin\Input Devices\1.0"
mkdir "bin\Input Devices\1.0\Resources"
mkdir "bin\Input Devices\1.0\Extensions"
set "dst=bin\Input Devices\"

xcopy "AtsEx\bin\Release\*.dll" "%dst%\1.0" /s /y
xcopy "AtsEx\bin\Release\*.xml" "%dst%\1.0" /s /y
xcopy "AtsEx\Resources\*.resx" "%dst%\1.0\Resources" /s /y
xcopy "AtsEx.PluginHost\bin\Release\*.dll" "%dst%\1.0" /s /y
xcopy "AtsEx.PluginHost\bin\Release\*.xml" "%dst%\1.0" /s /y
xcopy "AtsEx.PluginHost\Resources\*.resx" "%dst%\1.0\Resources" /s /y
xcopy "AtsEx.Scripting\bin\Release\*.dll" "%dst%\1.0" /s /y
xcopy "AtsEx.Scripting\bin\Release\*.xml" "%dst%\1.0" /s /y
xcopy "AtsEx.Scripting\Resources\*.resx" "%dst%\1.0\Resources" /s /y
xcopy "AtsEx.Launcher\bin\Release\*.dll" "%dst%\" /s /y
xcopy "AtsEx.Launcher\bin\Release\*.xml" "%dst%\" /s /y
xcopy "AtsEx.Caller.InputDevice\bin\Release\*.dll" "bin\" /s /y
xcopy "AtsEx.Caller.InputDevice\bin\Release\*.xml" "bin\" /s /y

for /d %%i in ("Extensions\*") do (
    xcopy "%%i\bin\Release\*.dll" "%dst%\1.0\Extensions" /s /y
)
