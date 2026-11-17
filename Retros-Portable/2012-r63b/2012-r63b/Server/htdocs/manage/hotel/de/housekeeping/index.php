<?php

$is_maintenance = true;

require_once('../../../../data_classes/server-data.php_data_classes-core.php.php');
if($user_rank > 3 && $logged_in or !$logged_in){

session_start();

$hkzone = true;
$p = FilterText($_GET['p']);
$do = FilterText($_GET['do']);
$page = FilterText($_GET['page']);
$key = FilterText($_GET['key']);
$search = FilterText($_POST['search']);
			
	if(session_is_registered(acp)){

		$session = $_SESSION['acp'];
		$admin_username = $_SESSION['hkusername'];
		$admin_password = $_SESSION['hkpassword'];

		$check = mysql_query("SELECT * FROM users WHERE username = '" . $admin_username . "' AND password = '" . $admin_password . "' AND rank > 3 LIMIT 1") or die(mysql_error());
		$valid = mysql_num_rows($check);

		if($valid > 0){

			$tmp = mysql_fetch_assoc($check);

                        if($p == "logout"){
				session_destroy();
				$notify_logout = true;
				include('login.php');
			} elseif($p == "home"){
				$tab = 1;
				require_once('home.php');
			} elseif($p == "test"){
				$tab = 1;
				require_once('test.php');
			} elseif($p == "banners"){
				$tab = 3;
				require_once('banners.php');
			} elseif($p == "campaigns"){
				$tab = 3;
				require_once('campaigns.php');
			} elseif($p == "groups"){
				$tab = 3;
				require_once('groups.php');
			} elseif($p == "news"){
				$tab = 3;
				require_once('news.php');
			} elseif($p == "marktplatz"){
				$tab = 3;
				require_once('marktplatz.php');
			} elseif($p == "recommended"){
				$tab = 3;
				require_once('recommended.php');
			} elseif($p == "settings"){
				$tab = 3;
				require_once('settings.php');
			} elseif($p == "sitealert"){
				$tab = 3;
				require_once('sitealert.php');
			} elseif($p == "webstore_item"){
				$tab = 3;
				require_once('webstore_item.php');
			} elseif($p == "webstore_catalog"){
				$tab = 3;
				require_once('webstore_catalog.php');
			} elseif($p == "banlogs"){
				$tab = 5;
				require_once('banlogs.php');
			} elseif($p == "alert"){
				$tab = 5;
				require_once('alert.php');
			} elseif($p == "alertlogs"){
				$tab = 5;
				require_once('alertlogs.php');
			} elseif($p == "chatlogs"){
				$tab = 5;
				require_once('chatlogs.php');
			} elseif($p == "onlinelogs"){
				$tab = 5;
				require_once('onlinelogs.php');
			} elseif($p == "viptool"){
				$tab = 5;
				require_once('viptool.php');
			} elseif($p == "viptool_bestellung"){
				$tab = 5;
				require_once('viptool_bestellung.php');
			} elseif($p == "ban"){
				$tab = 5;
				require_once('bantool.php');
			} elseif($p == "unban"){
				$tab = 5;
				require_once('unbantool.php');
			} elseif($p == "cloner"){
				$tab = 5;
				require_once('cloner.php');
			} elseif($p == "badgetool"){
				$tab = 5;
				require_once('badgetool.php');
			} elseif($p == "massa"){
				$tab = 5;
				require_once('massa.php');
			} elseif($p == "helper"){
				$tab = 5;
				require_once('helper.php');
			} elseif($p == "stafflogs"){
				$tab = 5;
				require_once('stafflogs.php');
			} elseif($p == "users"){
				$tab = 5;
				require_once('users.php');
			} elseif($p == "users_edit"){
				$tab = 5;
				require_once('users_edit.php');
			} elseif($p == "ranktool"){
				$tab = 5;
				require_once('ranktool.php');
			} elseif($p == "passwordtool"){
				$tab = 5;
				require_once('passwordtool.php');
			} elseif($p == "transactions"){
				$tab = 5;
				require_once('transactions.php');
			} elseif($p == "vouchers"){
				$tab = 5;
				require_once('vouchers.php');
			} else {
				$tab = 0;
				header("Location: ".$adminpath."/index/p/home");
				exit;
			} 

		} else {
			session_destroy();
			header("Location: ".$path.""); exit;
		}
	} else {
		include('login.php');
	}
} else {
	require_once('error.php');
}
exit;

?>