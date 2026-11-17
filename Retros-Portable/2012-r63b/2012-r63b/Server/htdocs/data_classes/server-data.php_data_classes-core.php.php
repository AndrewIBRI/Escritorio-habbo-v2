<?php 
ini_set("display_errors",false);
define("IN_HOLOCMS", TRUE);
session_start();
ini_set('default_charset', 'iso-8859-1');
header("Content-Type: text/html; charset=ISO-8859-1", true);
// #########################################################################
// MySQL CONNECTOR
// #########################################################################

@require_once('server-data.php_data_classes-config.php.php');
mysql_connect("$MySQLhostname", "$MySQLusername", "$MySQLpassword") or die(""); 
mysql_select_db("$MySQLdb") or die("");

// #########################################################################
// Kurz Befehle
// #########################################################################

$cms_name = mysql_fetch_assoc($cms_name = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_name'"));
$cms_url = mysql_fetch_assoc($cms_url = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_url'"));

$remote_ip = $_SERVER[REMOTE_ADDR];
$sitename = "".$cms_name['value']."";
$shortname = "".$cms_name['value']."";

if(@ini_get('date.timezone') == null && function_exists("date_default_timezone_get")){ @date_default_timezone_set("Europe/Berlin"); }

$H = date('H');
$i = date('i');
$s = date('s');
$m = date('m');
$d = date('d');
$Y = date('Y');
$j = date('j');
$n = date('n');
$today = $d;
$month = $m;
$year = $Y;
$getmoney_date = date('d.m.Y',mktime($m,$d,$Y));
$birthday_date = date('d.m', mktime($m,$d));
$date_normal = date('d.m.Y',mktime($m,$d,$Y));
$date_full = date('d.m.Y H:i:s',mktime($H,$i,$s,$m,$d,$Y));
$path = "".$cms_url['value']."";
$adminpath = "".$path."/manage/hotel/de/housekeeping";
$clientpath = "http://lavvos.com";
$cimagesurl = "http://localhost/images/c_images/";
$badgesurl = "/album1584/";
$hash_secret = "";

$cms_settings = mysql_query("SELECT * FROM cms_settings LIMIT 1");
$config = mysql_fetch_assoc($cms_settings);

$maintenance = mysql_num_rows($maintenance = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_maintenance' AND value = '1'"));

$server = mysql_fetch_assoc($server_status = mysql_query("SELECT * FROM server_status"));
$online_count = $server['users_online'];

if(isset($_POST) || isset($_GET) || isset($_REQUEST) || isset($_COOKIE)){
		foreach($_POST as $key => $p)
		{
		$_POST[$key] = htmlentities($p);
			$_POST[$key] = mysql_real_escape_string($p);
			$_POST[$key] = html_entity_decode($p);
		}
		
		//Filtro las entradas v�a GET
		foreach($_GET as $key => $g)
		{	
			$_GET[$key] = mysql_real_escape_string($g);
		}
	foreach($_GET as $key => $s)
		{	
			$COOKIE[$key] = mysql_real_escape_string($s);
		}
		//Filtro las entradas v�a REQUEST
		foreach($_REQUEST as $key => $k)
		{
			$_REQUEST[$key] = mysql_real_escape_string($k);
		}
	}
if(isset($_GET)){
		
		//Filtro las entradas v�a GET
		foreach($_GET as $key => $f)
		{	
			$_GET[$key] = strip_tags(htmlentities($f));
		}
	}

// #########################################################################
// MySQL TABLE SITE_CONFIG
// #########################################################################

function FetchSITESetting($strSetting){

	$tmp = mysql_query("SELECT ".$strSetting." FROM cms_settings LIMIT 1") or die(mysql_error());
	$tmp = mysql_fetch_assoc($tmp);
	return $tmp[$strSetting];

}

// #########################################################################
// CMS LOGIN FUNKTION
// #########################################################################

function HoloHash($password){
	//$hash_secret = "xCg532%@%gdvf^5DGaa6&*rFTfg^FD4\$OIFThrR_gh(ugf*/";
	$string = sha1($password);
	return $string;
}

function HoloHashMD5($password){
	//$hash_secret = "xCg532%@%gdvf^5DGaa6&*rFTfg^FD4\$OIFThrR_gh(ugf*/";
	$string = md5($password);
	return $string;
}

// #########################################################################
// "EINGELOGGT BLEIBEN" FUNKTION
// #########################################################################

if(!session_is_registered(username) && $_COOKIE['remember'] == "remember"){

	$cname = FilterText($_COOKIE['rusername']);
	$cpass_hash = $_COOKIE['rpassword'];

	$csql = mysql_query("SELECT password,id FROM users WHERE username = '".$cname."' LIMIT 1") or die(mysql_error());
	$cnum = mysql_num_rows($csql);

		if($cnum < 1){
			setcookie("remember", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
			setcookie("rusername", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
			setcookie("rpassword", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
		} else {

			$crow = mysql_fetch_assoc($csql);
			$correct_pass = $crow['password'];

			if($cpass_hash == $correct_pass){
				$_SESSION['username'] = $cname;
				$_SESSION['password'] = $crow['password'];
				$sql3 = mysql_query("UPDATE users SET ip_last = '".$remote_ip."' WHERE username = '".$cname."'");
				header("location: me"); exit;
			} else {

				setcookie("remember", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
				setcookie("rusername", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
				setcookie("rpassword", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
			}
		}
}

// #########################################################################
// IS-EVEN FUNKTION
// #########################################################################

function IsEven($intNumber)
{
	if($intNumber % 2 == 0){
		return true;
	} else {
		return false;
	}
}

// #########################################################################
// SMILIES FOR GRUPPEN/FORUM
// #########################################################################

function bbcode_format($str){

	$str = str_replace(":)", " <img src='./web-gallery/smilies/smile.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(";)", " <img src='./web-gallery/smilies/wink.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":P", " <img src='./web-gallery/smilies/tongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(";P", " <img src='./web-gallery/smilies/winktongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":p", " <img src='./web-gallery/smilies/tongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(";p", " <img src='./web-gallery/smilies/winktongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace("(L)", " <img src='./web-gallery/smilies/heart.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace("(l)", " <img src='./web-gallery/smilies/heart.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":o", " <img src='./web-gallery/smilies/shocked.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":O", " <img src='./web-gallery/smilies/shocked.gif' alt='Smiley' title='Smiley' border='0'> ", $str);

        $simple_search = array(
                                '/\[b\](.*?)\[\/b\]/is',
                                '/\[i\](.*?)\[\/i\]/is',
                                '/\[u\](.*?)\[\/u\]/is',
                                '/\[s\](.*?)\[\/s\]/is',
                                '/\[quote\](.*?)\[\/quote\]/is',
                                '/\[link\=(.*?)\](.*?)\[\/link\]/is',
                                '/\[url\=(.*?)\](.*?)\[\/url\]/is',
                                '/\[color\=(.*?)\](.*?)\[\/color\]/is',
                                '/\[size=small\](.*?)\[\/size\]/is',
                                '/\[size=large\](.*?)\[\/size\]/is',
                                '/\[code\](.*?)\[\/code\]/is',
                                '/\[habbo\=(.*?)\](.*?)\[\/habbo\]/is',
                                '/\[room\=(.*?)\](.*?)\[\/room\]/is',
                                '/\[group\=(.*?)\](.*?)\[\/group\]/is'
	);

        $simple_replace = array(
                                '<strong>$1</strong>',
                                '<em>$1</em>',
                                '<u>$1</u>',
                                '<s>$1</s>',
                                "<div class='bbcode-quote'>$1</div>",
                                "<a href='$1'>$2</a>",
                                "<a href='$1'>$2</a>",
                                "<font color='$1'>$2</font>",
                                "<font size='1'>$1</font>",
                                "<font size='3'>$1</font>",
                                '<pre>$1</pre>',
                                "<a href='./user_profile.php?id=$1'>$2</a>",
                                "<a onclick=\"roomForward(this, '$1', 'private'); return false;\" target=\"client\" href=\"./client.php?forwardId=2&roomId=$1\">$2</a>",
                                "<a href='./group_profile.php?id=$1'>$2</a>"
	);

        $str = preg_replace ($simple_search, $simple_replace, $str);

        return $str;
}

// #########################################################################
// F�R LOGIN_TICKET
// #########################################################################

function GenerateTicket(){

	$data = "ST-";

	for ($i=1; $i<=6; $i++){
		$data = $data . rand(0,9);
	}

	$data = $data . "-";

	for ($i=1; $i<=20; $i++){
		$data = $data . rand(0,9);
	}

	$data = $data . "-lavvo-beta-fe";
	$data = $data . rand(0,5);

	return $data;
}

// #########################################################################

if(session_is_registered('username')){

	$rawname = $_SESSION['username'];
	$rawpass = $_SESSION['password'];

	$usersql = mysql_query("SELECT * FROM users WHERE username = '".$rawname."' AND password = '".$rawpass."' LIMIT 1");
	$myrow = mysql_fetch_assoc($usersql);

	$userinfo = mysql_query("SELECT * FROM user_stats WHERE id = '".$myrow['id']."'");
	$userinfo = mysql_fetch_assoc($userinfo);

	$password_correct = mysql_num_rows($usersql);

	$my_id = $myrow['id'];
	$user_rank = $myrow['rank'];

	$ban = mysql_query("SELECT * FROM bans WHERE value = '".$myrow['username']."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1");
	$bancheck = mysql_num_rows($ban);

	if($myrow['ip_reg'] == "0"){
		mysql_query("UPDATE users SET ip_reg = '".$remote_ip."' WHERE id = '".$myrow['id']."'");

	}elseif($password_correct !== 1){

	session_destroy();
	header("location: ".$path."1");
	exit;

	}elseif($bancheck > 0){

	$bandata = mysql_fetch_assoc($ban);

	$timestamp = time();
	if($bandata['expire'] > $timestamp){
	$login_error = "Du bist gebannt! Der Grund f�r deinen Bann lautet \"".$bandata['reason']."\" und dauert bis ".date('d.m.Y - H:i:s', $bandata['expire'])."";			
	include('logout.php');
	session_destroy(); exit;

	} else{ 
		mysql_query("DELETE FROM bans WHERE value = '".$name."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1"); }  
	}

	$logged_in = true;
	$name = HoloText($myrow['username']);

	} else {

	$user_rank = 0;
	$name = "No-Name";
	$my_id = "No-ID";
	$myticket = "ST-No-Name-habbore-fe";
	$logged_in = false;

}

// #########################################################################
// HC CHECK
// #########################################################################

	$hc_a = mysql_query("SELECT * FROM user_subscriptions WHERE user_id = '".$my_id."' and timestamp_expire > '".time()."'");
	$hc = mysql_num_rows($hc_a);

	function getHCDays($my_id){

		$sql = mysql_query("SELECT timestamp_activated,timestamp_expire FROM user_subscriptions WHERE user_id = '".$my_id."' LIMIT 1") or die(mysql_error());
		
		if (mysql_num_rows($sql) == 0){
			return 0;
		}
		
		$data = mysql_fetch_assoc($sql);
		$diff = $data['timestamp_expire'] - time();
		
		if ($diff <= 0){
			return 0;
		}
		
		return ceil($diff / 86400);
	}


// #########################################################################
// VIP CHECK
// #########################################################################

	$vip_a = mysql_query("SELECT * FROM vip WHERE id_user = '".$my_id."'");
	$vip = mysql_num_rows($vip_a);

	function getVIPDays($my_id){

		$sql = mysql_query("SELECT timestamp,timestampend FROM vip WHERE id_user = '".$my_id."' LIMIT 1") or die(mysql_error());
		
		if (mysql_num_rows($sql) == 0){
			return 0;
		}
		
		$data = mysql_fetch_assoc($sql);
		$diff = $data['timestampend'] - time();
		
		if ($diff <= 0){
			return 0;
		}
		
		return ceil($diff / 86400);
	}


// #########################################################################
// HK/IS_MAINTENANCE CHECK
// #########################################################################

if($user_rank > 4){

	if(session_is_registered(hkusername) && session_is_registered(hkpassword)){ 
		$rank['iAdmin'] = "1";
	} else {
		$rank['iAdmin'] = "0"; 
	}

} else { 
	$rank['iAdmin'] = "0";
}

if($maintenance == '1' && !$is_maintenance && $rank['iAdmin'] < 1){
	header("Location: ".$path."/maintenance");
	exit;
} elseif($rank['iAdmin'] == 1 && $config['variable'] == "cms_maintenance" && $config['value'] == '1'){
	$notify_maintenance = true;
}

// #########################################################################

function IsUserBanned($name){

	$check = mysql_query("SELECT * FROM bans WHERE value = '".$my_id."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip'") or die(mysql_error());
	$is_banned = mysql_num_rows($check);

	if($is_banned > 0){
		$bandata = mysql_fetch_assoc($check);
		$reason = $bandata['reason'];
		$expire = $bandata['expire'];

		$stamp_now = time();

		if($stamp_now < $bandata['expire']){
			return true;
		} else { // ban expired
			mysql_query("DELETE FROM bans WHERE value = '".$my_id."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1") or die(mysql_error());
			return false;
		}
	} else {
		return false;
	}
}

// #########################################################################

function mysql_evaluate($query, $default_value="undefined") {
	$result = mysql_query($query) or die(mysql_error());

	if(mysql_num_rows($result) < 1){
		return $default_value;
	} else {
		return mysql_result($result, 0);
	}
}

// #########################################################################

function FilterText($str, $advanced=false) {
	if($advanced == true){ return mysql_real_escape_string($str); }
	$str = mysql_real_escape_string(htmlspecialchars($str));
	return $str;
}

function HoloText($str, $advanced=false, $bbcode=false) {
	if($advanced == true){ return stripslashes($str); }
	$str = stripslashes(nl2br(htmlspecialchars($str)));
	if($bbcode == true){$str = bbcode_format($str); }
	return $str;
}


?>
