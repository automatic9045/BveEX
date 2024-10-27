xcopy "_out\AtsEx\" "bin\Input Devices\AtsEx\" /e /s /y
move "bin\Input Devices\AtsEx\Debug" "bin\Input Devices\AtsEx\1.0"
xcopy "_out\Scenarios\" "bin\Scenarios\" /e /s /y

xcopy "AtsEx.Caller.InputDevice\bin\Release\*.dll" "bin\Input Devices\" /s /y
xcopy "AtsEx.Caller.InputDevice\bin\Release\*.xml" "bin\Input Devices\" /s /y

xcopy "*.svg" "bin\Input Devices\AtsEx\" /y
xcopy "*.md" "bin\Input Devices\AtsEx\" /y
xcopy "LICENSE" "bin\Input Devices\AtsEx\" /y

xcopy "*.svg" "bin\" /y
xcopy "*.md" "bin\" /y
xcopy "LICENSE" "bin\" /y

cd "bin\"
del "*.pdb" /s
del "*.tmp" /s
