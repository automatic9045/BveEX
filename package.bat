if "%1"=="" exit
set dest=%1

xcopy "_out\BveEx\" "%dest%\Input Devices\BveEx\" /e /s /y
move "%dest%\Input Devices\BveEx\Debug" "%dest%\Input Devices\BveEx\1.0"
if "%2"=="/s" xcopy "_out\Scenarios\" "%dest%\Scenarios\" /e /s /y

xcopy "BveEx.Caller.InputDevice\bin\Release\*.dll" "%dest%\Input Devices\" /s /y
xcopy "BveEx.Caller.InputDevice\bin\Release\*.xml" "%dest%\Input Devices\" /s /y

xcopy "*.svg" "%dest%\Input Devices\BveEx\" /y
xcopy "*.md" "%dest%\Input Devices\BveEx\" /y
xcopy "LICENSE" "%dest%\Input Devices\BveEx\" /y

xcopy "*.svg" "%dest%\" /y
xcopy "*.md" "%dest%\" /y
xcopy "LICENSE" "%dest%\" /y

cd "%dest%\"
del "*.pdb" /s
del "*.tmp" /s
