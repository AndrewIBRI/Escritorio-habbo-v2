<?php
require_once('./data_classes/server-data.php_data_classes-core.php.php');
if(session_is_registered(username)) {
	header("Location: ".$path); 
	exit; 
}

$sql = mysql_query("SELECT * FROM users WHERE ip_last='".$_SERVER['REMOTE_ADDR']."'");
$sql1 = mysql_query("SELECT * FROM users_referidos WHERE ip_referida='".$_SERVER['REMOTE_ADDR']."'");

if(mysql_num_rows($sql) > 0) {
	header("Location: " . $path . "/quickregister/start");
	exit;
} elseif(mysql_num_rows($sql1) > 0) {
	 header("Location: " . $path . "/quickregister/start");
	 exit;
    } else {

	$r = mysql_real_escape_string($_GET['r']);
	$ip = $_SERVER['REMOTE_ADDR'];
	mysql_query("INSERT INTO users_referidos (usuario, ip_referida, fecha) VALUES ('".$r."','".$remote_ip."','".time()."')");
	header("Location: " . $path . "/quickregister/start");
}

?>