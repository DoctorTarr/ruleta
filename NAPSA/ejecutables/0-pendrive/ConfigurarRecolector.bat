rmdir /s /q "c:\program files (x86)\napsa\Colector III"
rmdir /s /q "c:\program files (x86)\napsa\Collector IV"
pause
xcopy /s /i "d:\Collector IV" "c:\program files (x86)\napsa\Collector IV\"
del /q  "%userprofile%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\Recolector*.lnk"
xcopy "d:\Recolector 4-boot.lnk" "%userprofile%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup"
pause
