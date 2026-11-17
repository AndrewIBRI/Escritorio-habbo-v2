<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$SQL=mysql_query("SELECT * FROM user_tickets WHERE userid = '".$my_id."'");
echo mysql_error();
$N=mysql_num_rows($SQL);
if($N==0){
mysql_query("INSERT INTO `user_tickets` SET `user_tickets`.`userid` = '".$my_id."', `user_tickets`.`sessionticket` = '".GenerateTicket()."', `user_tickets`.`ipaddress` = '".$myrealip."'") or die(mysql_error());
}else{
mysql_query("UPDATE `user_tickets` SET `user_tickets`.`sessionticket` = '".GenerateTicket()."', `user_tickets`.`ipaddress` = '".$myrealip."' WHERE `userid` = '".$my_id."'") or die(mysql_error());
}

$ticketsql = mysql_query("SELECT * FROM user_tickets WHERE userid = '".$my_id."'") or die(mysql_error());
$ticketrow = mysql_fetch_assoc($ticketsql);

$client_port = mysql_fetch_assoc($client_port = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_port'"));
$client_mus = mysql_fetch_assoc($client_mus = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_mus'"));
$client_ip = mysql_fetch_assoc($client_ip = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_ip'"));
$client_variables = mysql_fetch_assoc($client_variables = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_variables'"));
$client_variables_night = mysql_fetch_assoc($client_variables_night = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_variables_night'"));
$client_texts = mysql_fetch_assoc($client_texts = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_texts'"));
$client_swf_path = mysql_fetch_assoc($client_swf_path = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_swf_path'"));
$client_habbo_swf = mysql_fetch_assoc($client_habbo_swf = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_habbo_swf'"));
$client_limit = mysql_fetch_assoc($client_limit = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_clientlimit'"));
if($user_rank < 1 && $online_count >= $client_limit['value']){ 

require_once('./templates/login_subheader.php');
require_once('./templates/login_header.php'); 

?>
<br>
<div id="intermediate">
<h2><center>Límite de Usuarios</center></h2>
<div id="enter-hotel">
<div class="open enter-btn">
<a href="/me" target="client" onClick="return onClientOpen(this)">Vuelve al ME<i></i></a><b></b>
</div>
</div>

<div id="info">
Actualmente tenemos <b><?php echo $online_count; ?> <?php echo $shortname; ?>s </b> conectados en el Servidor 1 y el límite es de <b><?php echo $client_limit['value']; ?> Onlines.</div>

<?php require_once('./templates/login_footer.php'); }else{ require_once('./templates/client_subheader.php'); ?>

<script type="text/javascript">
    FlashExternalInterface.loginLogEnabled = true;
    
    FlashExternalInterface.logLoginStep("web.view.start");
    
    if (top == self) {
        FlashHabboClient.cacheCheck();
    }

<?php
$pathd="test.jebbo.net";
?>
   var flashvars = {
     "client.allow.cross.domain" : "1", 
			"client.notify.cross.domain" : "0", 
			"connection.info.host" : "localhost", 
			"connection.info.port" : "<?php echo $client_port ['value']; ?>", 
			"site.url" : "<?php echo $path; ?>", 
			"url.prefix" : "<?php echo $path; ?>", 
			"client.reload.url" : "<?php echo $path; ?>/client.php", 
			"client.fatal.error.url" : "<?php echo $path; ?>/client.php", 
			"client.connection.failed.url" : "<?php echo $path; ?>/client.php", 
			"external.variables.txt" : "http://localhost/images/external_variables.txt?7",
			"external.texts.txt" : "http://localhost/images/external_flash_texts.txt?1",  
			"productdata.load.url" : "http://localhost/images/productdata.txt",  
			"furnidata.load.url" : "http://localhost/images/furnidata.txt?2",  
			"use.sso.ticket" : "1", 
			"sso.ticket" : "<?php echo $ticketrow['sessionticket']; ?>", 
			"processlog.enabled" : "1", 
			"account_id" : "1", 
			"client.starting" : "Por favor aguarde! carregando", 
			"flash.client.url" : "http://localhost/images/", 
			"user.hash" : "31385693ae558a03d28fc720be6b41cb1ccfec02", 
			"has.identity" : "1", 
			"flash.client.origin" : "popup" 


    };
    var params = {
        "base" : "http://localhost/images/",
        "allowScriptAccess" : "always",
        "menu" : "false"           
    };
    
    if (!(HabbletLoader.needsFlashKbWorkaround())) {
    	params["wmode"] = "opaque";
    }

    FlashExternalInterface.signoutUrl = "<?php echo $path; ?>/account/logout?token=<?php echo sha1($myrow['password']); ?>";
    
    var clientUrl = "http://localhost/images/Habbo.swf";

    swfobject.embedSWF(clientUrl, "flash-container", "100%", "100%", "10.0.0", "<?php echo $path; ?>/web-gallery/flash/expressInstall.swf", flashvars, params);
 
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

<meta name="description" content="Habbo Hotel : faça amigos, divirta-se e fique conhecido!" />
<meta name="keywords" content="habbo hotel ,Habbo2 , virtual, mundo, comunidade virtual, grátis, comunidade, avatar, bate papo, online, jovem, rpg, entre, social, grupos, fóruns, seguro, jogue, jogos, online, amigos, jovens, raros, mobis raros, colecionar, expressão, emblemas, diversão, música, celebridade, visita de famosos, celebridades, mmo, mmorpg, rpg online" />



<!--[if IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/ie8.css" type="text/css" />
<![endif]-->
<!--[if lt IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/ie.css" type="text/css" />
<![endif]-->
<!--[if lt IE 7]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/ie6.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/pngfix.js" type="text/javascript"></script>
<script type="text/javascript">
try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}
</script>

<style type="text/css">
body { behavior: url(/js/csshover.htc); }
</style>
<![endif]-->

<meta name="build" content="63-BUILD406 - 09.05.2011 23:04 - de" />
</head>
<body id="client" class="flashclient">
<div id="overlay"></div>
<img src="<?php echo $path; ?>/web-gallery/v2/images/page_loader.gif" style="position:absolute; margin: -1500px;" />

<div id="overlay"></div>
<div id="client-ui" >
    <div id="flash-wrapper">
    <div id="flash-container">
        <div id="content" style="width: 400px; margin: 20px auto 0 auto; display: none">
<div class="cbb clearfix">
    <h2 class="title">Por favor, actualiza tu Flash Player a la última versión</h2>

    <div class="box-content">
            <p>Puedes instalar y descargar Adobe Flash Player aquí: <a href="http://get.adobe.com/flashplayer/">Instala Flash player</a>. Más instrucciones para su instalación aquí: <a href="http://www.adobe.com/products/flashplayer/productinfo/instructions/">Más información</a></p>
            <p><a href="http://www.adobe.com/go/getflashplayer"><img src="<?php echo $path; ?>/web-gallery/v2/images/client/get_flash_player.gif" alt="Get Adobe Flash player" /></a></p>
    </div>
</div>

        </div>
        <script type="text/javascript">
            $('content').show();
        </script>

        <noscript>
            <div style="width: 400px; margin: 20px auto 0 auto; text-align: center">
                <p>If you are not automatically redirected, please <a href="/client">click here</a></p>
            </div>
        </noscript>
    </div>

</div>
<div id="fb-root"></div>
<script type="text/javascript">
    (function() {
        var e = document.createElement('script');
        e.async = true;
        e.src = 'https://connect.facebook.net/es_ES/all.js';
        document.getElementById('fb-root').appendChild(e);
    }());
</script>

</body>
</html>

<?php } ?>