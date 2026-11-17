<?php
 
require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');
 
$select = "SELECT * FROM cms_settings WHERE variable='client_ip'";
$result = mysql_query($select);
while($l = mysql_fetch_array($result)){
$cliendfgtip = $l['value'];
}
 
$select = "SELECT * FROM cms_settings WHERE variable='client_port'";
$result = mysql_query($select);
 while($l = mysql_fetch_array($result)){
$clienghjport = $l['value'];
}
 
mysql_query("UPDATE users SET auth_ticket = '', auth_ticket = '".GenerateTicket()."', ip_last = '', ip_last = '".$remote_ip."' WHERE id = '".$my_id."'") or die(mysql_error());
 
$ticketsql = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'") or die(mysql_error());
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
$caca="deltoro";
if($caca=="delavaca"){
 
  echo"<script>alert(\"Edita la Configuraci�n de tu Hotel en el Housekeeping\");</script>
 <meta http-equiv=\"refresh\" content=\"0;url=".$adminpath."/index/p/login&do=secretcode\">";
 
} elseif($user_rank < 0 && $online_count >= $client_limit['value']){
 
require_once('./templates/login_subheader.php');
require_once('./templates/login_header.php');
 
?>
 
<br>
<div id="intermediate">
<h2><center>L�mite de Clientes</center></h2>
<div id="enter-hotel">
<div class="open enter-btn">
<a href="http://servidor2.lavvos.com" target="client" onclick="return onClientOpen(this)">Ir al Servidor 2<i></i></a><b></b>
</div>
</div>
 
<div id="info">
Actualmente tenemos <b><?php echo $online_count; ?> <?php echo $shortname; ?>s </b> conectados en el Servidor 1 y el l�mite es de <b><?php echo $client_limit['value']; ?> Onlines.</b> Puedes intentar entrar en el <strong>Servidor 2</strong>.</div>
 
<?php require_once('./templates/login_footer.php'); }else{ require_once('./templates/client_subheader.php'); ?>
<script type="text/javascript">
    FlashExternalInterface.loginLogEnabled = true;
 
   FlashExternalInterface.logLoginStep("web.view.start");
 
   if (top == self) {
        FlashHabboClient.cacheCheck();
    }
    var flashvars = {
    "client.allow.cross.domain" : "1",
        "client.notify.cross.domain" : "0",
        "connection.info.host" : "localhost",
        "connection.info.port" : "30000",
        "site.url" : "localhost",
        "url.prefix" : "http://localhost",
        "client.reload.url" : "127.0.0.1/client.php",
        "client.fatal.error.url" : "http://localhost/client.php",
        "client.connection.failed.url" : "http://localhost/client.php",
        "external.variables.txt" : "http://localhost/phx/vari.php?username=<?php echo $name; ?>",
        "use.sso.ticket" : "1",
        "sso.ticket" : "<?php echo $ticketrow['auth_ticket']; ?>",
        "processlog.enabled" : "1",
        "account_id" : "1",
        "client.starting" : "Ol� <?php echo $name; ?>, aguarde, est� carregando.",
        "flash.client.url" : "http://localhost/phx/dcr/hof_furni/",
        "has.identity" : "1",
        "flash.client.origin" : "popup"
 
 
    };
    var params = {
        "base" : "http://localhost/phx/",
        "allowScriptAccess" : "always",
        "menu" : "false"              
   };
 
   if (!(HabbletLoader.needsFlashKbWorkaround())) {
      params["wmode"] = "opaque";
    }
 
    FlashExternalInterface.signoutUrl = "<?php echo $path; ?>/account/logout?token=<?php echo sha1($myrow['password']); ?>";
 
   var clientUrl = "http://localhost/phx/Habbo.swf";
 
     swfobject.embedSWF(clientUrl, "flash-container", "100%", "100%", "10.0.0", "http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/424/web-gallery/flash/expressInstall.swf", flashvars, params);
 
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
 
 
<meta name="description" content="Lavvo Hotel: haz amig@s, �nete a la diversi�n y date a conocer." />
<meta name="keywords" content="lavvo hotel, mundo, virtual, red social, gratis, comunidad, personaje, chat, online, adolescente, roleplaying, unirse, social, grupos, forums, seguro, jugar, juegos, amigos, adolescentes, raros, furni raros, coleccionable, crear, coleccionar, conectar, furni, muebles, mascotas, dise�o de salas, compartir, expresi�n, placas, pasar el rato, m�sica, celebridad, visitas de famosos, celebridades, juegos en l�nea, juegos multijugador, multijugador masivo" />
 
 
 
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
    <h2 class="title">Por favor, actualiza tu Flash Player a la �ltima versi�n</h2>
 
    <div class="box-content">
            <p>Puedes instalar y descargar Adobe Flash Player aqu�: <a href="http://get.adobe.com/flashplayer/">Instala Flash player</a>. M�s instrucciones para su instalaci�n aqu�: <a href="http://www.adobe.com/products/flashplayer/productinfo/instructions/">M�s informaci�n</a></p>
            <p><a href="http://www.adobe.com/go/getflashplayer"><img src="http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/424/web-gallery/v2/images/client/get_flash_player.gif" alt="Get Adobe Flash player" /></a></p>
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
  <div id="content" class="client-content"></div>  
<?PHP
$rand =rand(0,100);
if($rand < 85){
//PUBLI EDU
?>
 
 
<?PHP
} else {
//PUBLICIDAD HECTOR
?>
 
 
<?PHP
}
?>
     
</div>
    <div style="display: none">
<div id="habboCountUpdateTarget">
1,323 Mitglieder online
</div>
  <script language="JavaScript" type="text/javascript">
      setTimeout(function() {
        HabboCounter.init(600);
      }, 20000);
  </script>
 
    </div>
    <script type="text/javascript">
        RightClick.init("flash-wrapper", "flash-container");
        if (window.opener && window.opener != window && window.opener.location.href == "/") {
            window.opener.location.replace("/me");
        }
        $(document.body).addClassName("js");
        HabboClient.startPingListener();
    </script>
<div id="fb-root"></div>
<script type="text/javascript">
    (function() {
        var e = document.createElement('script');
        e.async = true;
        e.src = 'https://connect.facebook.net/de_DE/all.js';
        document.getElementById('fb-root').appendChild(e);
    }());
</script>
 
 
        <iframe name="logframe" src="/bc/logframe?" width="0" height="0" frameborder="0" scrolling="no" marginwidth="0" marginheight="0" style="position: absolute; top:0; left:0"></iframe>
 
 
<iframe name="conversion-tracking" src="/conversion_tracking_frame" width="0" height="0" frameborder="0" scrolling="no" marginwidth="0" marginheight="0" style="position: absolute; top:0; left:0"></iframe>
 
 
 
<script type="text/javascript">
    HabboView.run();
</script>
<script type="text/javascript" src="anuncio.js"></script>
 
</body>
</html>
<?php } ?>