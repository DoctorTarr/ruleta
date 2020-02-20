rmdir /s /q \"program files (x86)\napsa\Colector III"
xcopy /s /i ".\Collector IV" \"program files (x86)\napsa\Collector IV\"
del /q  "%userprofile%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\Col*.lnk"
xcopy ".\Recolector 4-boot.lnk" "%userprofile%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup"
@start iexplore http://192.168.1.64/
