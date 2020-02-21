netsh interface ipv4 set address name="Ethernet" static 192.168.1.128 255.255.255.0 192.168.1.1
pause
netsh int tcp set global autotuninglevel=disabled
netsh interface teredo set state disabled
netsh interface ipv6 6to4 set state state=disabled undoonstop=disabled
netsh interface ipv6 isatap set state state=disabled
pause
ping 192.168.1.64
pause **** User: admin / Pass: Qwer1234 ****
@start iexplore http://192.168.1.64/
