rem Open TCP Port 500 inbound and outbound
netsh advfirewall firewall add rule name="Test TCP Port 500" dir=in action=allow protocol=TCP localport=500
netsh advfirewall firewall add rule name="Test TCP Port 500" dir=out action=allow protocol=TCP localport=500

netsh advfirewall firewall add rule name="Allow from 192.168.0.101" dir=in action=allow protocol=ANY remoteip=192.168.0.101

