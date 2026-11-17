<?php
#####################################################################
#||||| 		Project MyHobba desenvolvido por @rtiag0		   |||||#
#||||| Criado por @rtiag0 - HTML, PHP, CSS & Data_classes      |||||#
# ----------------------------------------------------------------- #
#||||| © Copyright 2012. Hobba 2011 - Open Source Software     |||||#
#||||| © Copyright 2012. LavvosCMS - DATA_CLASSES Estrutura	   |||||#
#||||| © Copyright 2012. Sulake Corporation - Habbo Arquivos   |||||#
#####################################################################

define('APP_ID', '580146118667846');
define('APP_SECRET', 'a3672497f470a9b2088d54aa1b11655c');
require_once('./data_classes/server-data.php_data_classes-core.php.php');
require('./data_classes/facebook.php');
require('./data_classes/fbconfig.php');

$_SESSION['FACEBOOK'] = "YES";
$Facebook = new Facebook(Array(
	'appId'  => $config['appId'],
  'secret' => $config['secret'],
));
$users = $Facebook->getUser();
if($users):
	try {
		$my = $Facebook->api('/me');
	} catch(FacebookApiException $e) {
		$my = null;
		die("ERROR FB: ".$e);
	}
else:
	header("Location: ".$path."/");
	exit();
endif;
if(is_null($my)):
	header("Location: $path");
else:
	$facebook_id = $my['id'];
	$check = mysql_query("SELECT * FROM users WHERE facebook_id = '".$facebook_id."'") or die(mysql_error());
	if(mysql_num_rows($check) != 0):
		$rows = mysql_fetch_array($check);
		$_SESSION['username'] = $rows['username'];
		$_SESSION['password'] = $rows['password'];
		header("Location: $path/me?facebook=true&frame=false");
	else:
		$nombre = strtolower($my['last_name']) . rand(0, 983923);
		$dob = null;
		$mail = $my['email'];
		$password = substr(md5(rand(0, 999). strtolower($mail). rand(0,100000)), 0, 12);
		$password2 = HoloHashMD5($password);
		$gender = substr($my['gender'],0,1);
		$look = null;
		mysql_query("INSERT INTO `users` (username,real_name,password,auth_ticket,motto,mail,rank,look,gender,account_created,last_online,online,ip_last,ip_reg,working,secretcode,mymusik,birth,facebook_id) VALUES ('".$nombre."','Habol','".$password2."','Default','Bem vindo ao".$sitename."!','".$mail."','1','hd-180-1.ch-210-66.lg-270-82.sh-290-91.hr-100-','".$gender."','".time()."','".time()."','0','".$_SERVER['REMOTE_ADDR']."','".$_SERVER['REMOTE_ADDR']."','','','','".$dob."','".$facebook_id."')") or die(mysql_error());
			$_SESSION['username'] = $nombre;
			$_SESSION['password'] = $password2;
		header("Location: $path/me?facebook=true&frame=false");
	endif;
endif;

?>