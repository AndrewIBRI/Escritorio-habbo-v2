<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');

session_start();

if(isset($_GET['registerCancel']) && $_GET['registerCancel'] == "true"){
session_unset();
}

if(!session_is_registered(username)){

	if(isset($_POST['username']) && isset($_POST['password'])){
	$username = FilterText($_POST['username']);
	$password = HoloHash($_POST['password'], $username);
	$password2 = HoloHashMD5($_POST['password'], $username);
		
		if(empty($username) || empty($_POST['password'])){
			$login_error = "Entre com o seu nome de usuário e senha!";
		} else {
			$sql = mysql_query("SELECT id FROM users WHERE username = '".$username."' AND password = '".$password."' OR username = '".$username."' AND password = '".$password2."' LIMIT 1") or die(mysql_error());
			if(mysql_num_rows($sql) < 1){
				$login_error = "Sua senha e seu nome não conferem.";
			} else {
				$userdata = mysql_fetch_assoc($sql);
				$userid = $userdata['id'];
				$ban = mysql_query("SELECT * FROM bans WHERE value = '".$username."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1") or die(mysql_error());
				if(mysql_num_rows($ban) < 1){

					mysql_query("UPDATE users SET password = '".$password2."', ip_last = '".$remote_ip."' WHERE username = '".$username."'");

					$_SESSION['username'] = $username;
					$_SESSION['password'] = $password2;
						if($_POST['_login_remember_me'] == "true"){
							setcookie("remember", "remember", time()+60*60*24*100, "/");
							setcookie("rusername", $_SESSION['username'], time()+60*60*24*100, "/");
							setcookie("rpassword", $_SESSION['password'], time()+60*60*24*100, "/");
						}
					header("location: $path/me"); exit;
				} else {
					$bandata = mysql_fetch_assoc($ban);
					$timestamp = time();
					if($bandata['expire'] > $timestamp){
						$login_error = "Você foi banido do Hotel por: \"" . $bandata['reason'] . "\" seu banimento acabará em: " . date('d/m/Y H:i:s', $bandata['expire']) . "."; //Tempo e duração do Banimento
					} else {
						mysql_query("DELETE FROM bans WHERE value = '".$username."' AND bantype = 'user' OR value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1") or die(mysql_error());
						mysql_query("UPDATE users SET password = '".$password2."', ip_last = '".$remote_ip."' WHERE username = '".$username."'");

					$_SESSION['username'] = $username;
					$_SESSION['password'] = $password2;
							if($_POST['_login_remember_me'] == "true"){
								setcookie("remember", "remember", time()+60*60*24*100, "/");
								setcookie("rusername", $_SESSION['username'], time()+60*60*24*100, "/");
								setcookie("rpassword", $_SESSION['password'], time()+60*60*24*100, "/");
							}
						header("location: $path/me"); exit;
					}
				}
			}
		}
	}



$body_id = "index";
$pagename = "Crie seu ".$shortname.", construa seu quarto, converse, faça novos amigos e divirta-se!";

include('./templates/login_subheader.php');

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="content-type" content="text/html;charset=ISO-8859-1">
<head>

 

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link rel="stylesheet" href="css/indexdc6a.css?676" />
<link type="text/css" rel="stylesheet" href="Public/Styles/Default/CSS/website_background.html" />
<link href='http://fonts.googleapis.com/css?family=Ubuntu:400,700' rel='stylesheet' type='text/css'>

<meta name="apple-mobile-web-app-capable" content="yes">
<meta name="apple-mobile-web-app-status-bar-style" content="black">

<meta name="SKYPE_TOOLBAR" content="SKYPE_TOOLBAR_PARSER_COMPATIBLE" />

<meta property="fb:app_id" content="155102069619" />

<meta property="og:site_name" content="<?php echo $shortname; ?> Hotel" />
<meta property="og:title" content="<?php echo $shortname; ?>: Crie seu <?php echo $shortname; ?>, construa seu Quarto, converse e faça novos amigos." />
<meta property="og:url" content="http://www.<?php echo $shortname; ?>hotel.com.br" />
<meta property="og:image" content="http://i46.tinypic.com/e5pilj.png" />
<meta property="og:locale" content="pt_BR" />

<link rel="shortcut icon" href="favicon.gif" type="image/vnd.microsoft.icon" />
</head>
<body>

		<div id="container" style="height:105px;">
    <div id="logo"></div>
    <div id="login_place">
<br/>

   <form id="loginformitem" name="loginformitem" action="<?php echo $path; ?>?name=<?php echo FilterText($_POST['username']); ?>&password=true"
          method="post">
<?php if(isset($login_error)){ ?>
        <div id="loginerrorfieldwrapper">
            <div id="loginerrorfield">
            <div><?php echo $login_error; ?></div>
            </div> </div> <?php } ?>


      <div id="login_username">

            <div id="title">Nome <?php echo $shortname; ?>:</div>
    <input type="text" name="username" x-webkit-speech speech lang="pt-br" style="width: 180px"; placeholder="Digite ou fale seu nome..." required autofocus>
            <div id="ledenlijstpb_box" style="display:none;" >
                <div id="ledenlijstpb_mid">
                    <table width='100% border='0' cellpadding='0' cellspacing='0'>
                               </table>                </div>
            </div><br />
            <input type="checkbox" /> <strong>Manter-me conectado</strong>
        </div>
        <div id="login_password">
            <div id="title">Senha:</div>
     <input type="password" name="password" style="width: 180px"; placeholder="Digite sua senha..." required autofocus><br />
            </div>
        <div id="login_submit" style="margin-top:2px;"><input type="submit" value="Entrar" style="border: 0;cursor: pointer;"/></div>
    </form>
    <div style="width:100%; float:left; padding-top:5px; text-align:center;"><small><br /></small></div>
    </div>
    <a href="<?php echo $path; ?>/quickregister/start"><div id="register_button"><div style="padding-top:93px; color:#FFF; text-shadow:none; text-align:center;"></div></div></a>    
</div> 
<div id="container" style="height:475px;">


	<div id="information"><div id="text_20px" style="color:#FFF; text-shadow: 1px 1px 0px #000;">

<font color="blue"> Você não imagina o que acontece no <font color="red"> Ha</font>bbie ! - <font color="gray"> Mobis Pirata</font>, <font color="red"> Diamantes</font>, <font color="green"> Vagas</font> e Muito mais! </font>



</div></div>
	<div id="container_image" class="xmas12outdoor" style="margin-top:20px;"></div>
    <table width="920px" border="0" cellpadding="0" cellspacing="0" style="margin:-380px 10px 0px 10px;">
    	<tr>
        	
            <br><br><br><br>
        </tr>
    </table>
    <div style="float:right; position: relative; overflow: hidden; margin:55px 2px 0px 0px; width:220px;">
    <div id="infobox_top"></div>
        <div id="infobox_mid"><img src="./img/online.png" align="left" style="padding-right:3px;" /><div id="text_15px">Usuários Onlines</div>
        	<div style="width:200px;">            No momento há <br><strong><?php echo $online_count; ?></strong> <?php echo $shortname; ?>'s Onlines<br />
            <div style="padding:2px;"></div>
            
			</div>
        </div>
    <div id="infobox_bot"></div>
    </div>
    
    
    <table width="920px" border="0" cellpadding="0" cellspacing="0" style="margin:248px 10px 0px 10px;">
    	<tr>
        	<td>
            	<div id="infobox_top"></div>
            	<div id="infobox_mid"><img src="./img/furni.png" align="left" style="padding-right:3px;" /><div id="text_15px">Mobíliario </div>Catálogo atualizado, cheio de maravilhosas mobílias novas e inteligentes para seus quartos.</div>
            	<div id="infobox_bot"></div>
            </td>
            <td width="10px"></td>
        	<td>
            	<div id="infobox_top"></div>
            	<div id="infobox_mid"><img src="./img/chat.png" align="left" style="padding-right:3px;" /><div id="text_15px">Bate-Papo</div>Convide seus amigos ou faça novas amizades, experimente de tudo mas antes proteja-se!</div>
            	<div id="infobox_bot"></div>
            </td>
            <td width="10px"></td>
            <td>
            	<div id="infobox_top"></div>
            	<div id="infobox_mid"><img src="./img/games.png" align="left" style="padding-right:3px;" /><div id="text_15px">Atividades Diárias</div>Diariamente você encontra eventos e notícias que poderão garantir-lhe prêmios incríveis.</div>
            	<div id="infobox_bot"></div>
            </td>
        </tr>
    </table>
    
    <div id="container_stats"></div>
    </div>
</div>

<div id="container">
	<div id="copyricht_top"></div>
    <div id="copyricht_mid">    
    <div id="text" style="text-align:center">
    <br>
	© Habbie Hotel 2013 - Todos os direitos reservados. Development by Ribom
    
</div></div>
    <div id="copyricht_bot"></div>
</div>
</script>
 
</body>
</html>

<?php
} else {
header("location:../me");
}
?>

