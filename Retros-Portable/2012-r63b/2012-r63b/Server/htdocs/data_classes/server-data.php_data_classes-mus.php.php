<?php

$ip = "javvox.com";
$port = "30001";

function Mus($header, $data = ''){
		
	$musData = $header . chr(1) . $data;
		
	$sock = @socket_create(AF_INET, SOCK_STREAM, getprotobyname('tcp'));
	@socket_connect($sock, $ip, $port);
	@socket_send($sock, $musData, strlen($musData), MSG_DONTROUTE);	
	@socket_close($sock);

}

?>