set dest=%1

xcopy "_out\AtsEx\" "%dest%\Input Devices\AtsEx\" /e /s /y
move "%dest%\Input Devices\AtsEx\Debug" "%dest%\Input Devices\AtsEx\1.0"
if "%2"=="/s" xcopy "_out\Scenarios\" "%dest%\Scenarios\" /e /s /y

xcopy "AtsEx.Caller.InputDevice\bin\Release\*.dll" "%dest%\Input Devices\" /s /y
xcopy "AtsEx.Caller.InputDevice\bin\Release\*.xml" "%dest%\Input Devices\" /s /y

xcopy "*.svg" "%dest%\Input Devices\AtsEx\" /y
xcopy "*.md" "%dest%\Input Devices\AtsEx\" /y
xcopy "LICENSE" "%dest%\Input Devices\AtsEx\" /y

xcopy "*.svg" "%dest%\" /y
xcopy "*.md" "%dest%\" /y
xcopy "LICENSE" "%dest%\" /y

cd "%dest%\"
del "*.pdb" /s
del "*.tmp" /s
