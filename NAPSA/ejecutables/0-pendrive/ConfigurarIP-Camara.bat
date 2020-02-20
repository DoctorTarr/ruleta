netsh interface ipv4 set address name="Ethernet" static 192.168.1.128 255.255.255.0 192.168.1.1
pause
ping 192.168.1.64
pause **** User: admin / Pass: Qwer1234 ****
@start iexplore http://192.168.1.64/
