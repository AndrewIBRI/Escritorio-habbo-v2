<?php
define('USERNAME_REQUIRED', TRUE);
define('ACCOUNT_REQUIRED', TRUE);
include('../global.php');

$rand1 = rand(100000, 999999);
$rand2 = rand(10000, 99999);
$rand3 = rand(10000, 99999);
$rand4 = rand(10000, 99999);
$rand5 = rand(10000, 99999);
$rand6 = rand(1, 9);
$ticket = "ST-".$rand1."-".$rand2.$rand3."-".$rand4.$rand5."-otaku-".$rand6;
$username = $_SESSION['username'];
if(@$_GET['ticket'] && @$_GET['username'] && $users->UserPermission('hk_ext_login', $_SESSION['username']))
{
$username = $_GET['username'];
$ticket = $_GET['ticket'];
}
$query = mysql_query("UPDATE users SET auth_ticket = '$ticket' WHERE username = '$username'");
$query = mysql_query("UPDATE users SET ip_last = '".$_SERVER['REMOTE_ADDR']."' WHERE username = '$username'");
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en" xmlns:og="http://opengraphprotocol.org/schema/" xmlns:fb="http://www.facebook.com/2008/fbml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<title><?php echo $sitename; ?></title>

<script type="text/javascript">
var andSoItBegins = (new Date()).getTime();
</script>
<link rel="shortcut icon" href="http://cotendo.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1064/web-gallery/v2/favicon.ico" type="image/vnd.microsoft.icon" />
<link rel="alternate" type="application/rss+xml" title="Habbo RSS" href="http://www.habbo.com.br/articles/rss.xml" />
<meta name="csrf-token" content="cd2bd2ca75"/>
<link rel="stylesheet" href="http://localhost/swf/css.js/common.css" type="text/css" />
<script src="http://localhost/swf/css.js/libs2.js" type="text/javascript"></script>

<script src="http://localhost/swf/css.js/visual.js" type="text/javascript"></script>
<script src="http://localhost/swf/css.js/libs.js" type="text/javascript"></script>
<script src="http://localhost/swf/css.js/common.js" type="text/javascript"></script>


<script type="text/javascript"> 
document.habboLoggedIn = true;
var habboName = "<?php echo $_SESSION['username']; ?>";
var habboReqPath = "http://habbe.sytes.net/";
var habboStaticFilePath = "http://cotendo.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1064/web-gallery";
var habboImagerUrl = "http://www.habbo.com/habbo-imaging/";
var habboPartner = "";
var habboDefaultClientPopupUrl = "http://localhost/client";
window.name = "ClientWndw";
if (typeof HabboClient != "undefined") { HabboClient.windowName = "ClientWndw"; }
</script> 

<link rel="stylesheet" href="http://localhost/swf/css.js/habboflashclient.css" type="text/css" />
<script src="http://localhost/swf/css.js/habboflashclient.js" type="text/javascript"></script>
<script src="http://localhost/swf/css.js/habboflashclient.js" type="text/javascript"></script>

<script type="text/javascript"> 
FlashExternalInterface.loginLogEnabled = false;
FlashExternalInterface.logLoginStep("web.view.start");

if (top == self) {
FlashHabboClient.cacheCheck();
}
var flashvars = {
"client.allow.cross.domain" : "1", 
"client.notify.cross.domain" : "1", 
"connection.info.host" : "<?php echo $core->CmsSetting('client_ip'); ?>", 
"connection.info.port" : "<?php echo $core->CmsSetting('client_port'); ?>", 
"site.url" : "%www%", 
"url.prefix" : "%www%", 
"client.reload.url" : "%www%/index.php", 
"client.fatal.error.url" : "%www%/client/disconnected.php?flash_client_error", 
"client.connection.failed.url" : "%www%/client/disconnected.php?client_connection_failed", 
"external.variables.txt" : "http://localhost/swf/gamedata/(variaveis)/external_variables2.txt", 
"external.texts.txt" : "http://localhost/swf/gamedata/(textos)/external_flash_texts.txt", 
"use.sso.ticket" : "1",
"sso.ticket" : "<?php echo $ticket ?>", 
"processlog.enabled" : "0", 
"account_id" : "1", 
"client.starting" : "Ei <?php echo $users->UserInfo($username, 'username'); ?> Aguarde está carregando!", 
"flash.client.url" : "http://localhost/swf/gordon/RELEASE63(base)/", 
"user.hash" : "31385693ae558a03d28fc720be6b41cb1ccfec02", 
"has.identity" : "0", 
"flash.client.origin" : "popup",
"token" : "<?php echo $ticket ?>"
};
var params = {
"base" : "http://localhost/swf/gordon/RELEASE63(base)/",
"allowScriptAccess" : "always",
"menu" : "false" 
};

if (!(HabbletLoader.needsFlashKbWorkaround())) {
params["wmode"] = "opaque";
}

var clientUrl = "http://localhost/swf/gordon/RELEASE63(base)/Habbo.swf";

swfobject.embedSWF(clientUrl, "flash-container", "100%", "100%", "10.0.0", "http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/763/web-gallery/flash/expressInstall.swf", flashvars, params);

window.onbeforeunload = unloading;
function unloading() {
var clientObject;
if (navigator.appName.indexOf("Microsoft") != -1) {
clientObject = window["flash-container"];
} else {
clientObject = document["flash-container"];
}
try {
clientObject.unloading();
} catch (e) {}
}
</script>

<!--[if IE 8]>
<link rel="stylesheet" href="http://localhost/swf/css.js/ie8.css" type="text/css" />
<![endif]-->

<!--[if lt IE 8]>
<link rel="stylesheet" href="http://localhost/swf/css.js/ie.css" type="text/css" />
<![endif]-->
<!--[if lt IE 7]>
<link rel="stylesheet" href="http://localhost/swf/css.js/ie6.css" type="text/css" />
<script src="http://localhost/swf/css.js/pngfix.js" type="text/javascript"></script>
<script type="text/javascript">
try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}
</script>

<style type="text/css">
body { behavior: url(/js/csshover.htc); }
</style>
<![endif]-->

</head> 

<body id="client" class="flashclient"> 

<div id="overlay"></div> 
<div id="overlay"></div> 
<div id="client-ui" > 
<div id="flash-wrapper"> 
<div id="flash-container"> 
<div id="content" style="width: 400px; margin: 20px auto 0 auto; display: none"> 
<div class="cbb clearfix"> 
<h2 class="title">Please install Adobe Flash Player.</h2> 
<div class="box-content"> 
<p>You can install and download Adobe Flash Player here: <a href="http://get.adobe.com/flashplayer/">
Install flash player</a>. More instructions for installation can be 
found here: <a href="http://www.adobe.com/products/flashplayer/productinfo/instructions/">
More information</a></p> 
<p><a href="http://www.adobe.com/go/getflashplayer"><img src="http://cotendo.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1064/web-gallery/v2/images/client/get_flash_player.gif" alt="Get Adobe Flash player" /></a></p> 
</div> 
</div> 
</div> 
<script type="text/javascript"> 
$('content').show();
</script> 
<noscript> 
<div style="width: 400px; margin: 20px auto 0 auto; text-align: center">
<p>If you are not automatically redirected, please <a href="/client/nojs">click here</a></p>
</div>
</noscript> 
</div> 

</div> 
<div id="content" class="client-content"></div>
</div> 

</body> 
</html>