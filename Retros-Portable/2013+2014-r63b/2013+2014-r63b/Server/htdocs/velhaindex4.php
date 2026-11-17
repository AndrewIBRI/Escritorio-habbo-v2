<?php
header("Content-Type: text/html; charset=ISO-8859-1", true);
require_once('./data_classes/server-data.php_data_classes-core.php.php');

session_start();

if(isset($_GET['registerCancel']) && FilterText($_GET['registerCancel'] == "true")){
session_unset();
}

if(!session_is_registered(username)){

	if(isset($_POST['username']) && isset($_POST['password'])){
	$username = FilterText($_POST['username']);
	$password = HoloHash($_POST['password'], $username);
	$password2 = HoloHashMD5($_POST['password'], $username);

		if(empty($username) || empty($_POST['password'])){
			$login_error = "Usuário ou senha incorretos!";
		} else {
			$sql = mysql_query("SELECT id FROM users WHERE username = '".$username."' AND password = '".$password."' OR username = '".$username."' AND password = '".$password2."' LIMIT 1") or die(mysql_error());
			if(mysql_num_rows($sql) < 1){
				$login_error = "Usuário ou senha incorretos!";
			} else {
				$userdata = mysql_fetch_assoc($sql);
				$userid = $userdata['id'];
				$ban = mysql_query("SELECT * FROM bans WHERE value = '".$username."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1") or die(mysql_error());
				if(mysql_num_rows($ban) < 1){

					mysql_query("UPDATE users SET password = '".$password2."', ip_last = '".$remote_ip."' WHERE username = '".$username."'");

					$_SESSION['username'] = $username;
					$_SESSION['password'] = $password2;
						if ($_POST['_login_remember_me'] == "true"){
							setcookie("remember", "remember", time()+60*60*24*100, "/");
							setcookie("rusername", $_SESSION['username'], time()+60*60*24*100, "/");
							setcookie("rpassword", $_SESSION['password'], time()+60*60*24*100, "/");
						}
					header("location: $path/me"); exit;
				} else {
					$bandata = mysql_fetch_assoc($ban);
					$timestamp = time();
					if($bandata['expire'] > $timestamp){
						$login_error = "Você foi banido por: \"" . $bandata['reason'] . "\" e acaba em: " . date('d.m.Y - H:i:s', $bandata['expire']) . ".";
					} else {
						mysql_query("DELETE FROM bans WHERE value = '".$username."' AND bantype = 'user' OR value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1") or die(mysql_error());
						mysql_query("UPDATE users SET password = '".$password2."', ip_last = '".$remote_ip."' WHERE username = '".$username."'");

					$_SESSION['username'] = $username;
					$_SESSION['password'] = $password2;
							if ($_POST['_login_remember_me'] == "true"){
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


?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
<script src="js/script.js"></script>
<link href="css-index.css" rel="stylesheet" type="text/css" />
<link rel="shortcut icon" href="/web-gallery/v2/images/favicon.ico" />
<title>Habbie: Crie seu Habbie, construa seu quarto, converse, faça novos amigos e divirta-se!</title>
<meta name="robots" content="index,follow,all" />
<meta name="description" content="Habbo, Hotel, Avatar, Jogos, Habbie, Divers&atilde;o" />
</head>
<body>


<div id="wrapper">
  <div id="topo"></div>
<div id="box_login">
            <form action="" method="POST">
      <input type="text" name="username" placeholder="Usuário" maxlength="50" required />
      <input type="password" name="password" class="tbox_style" placeholder="Senha" maxlength="40" required />
      <input name="iLogin" type="image" src="_img/bt_entrar.png">
      </form> 
      
               
          
      <a href="/quickregister/start"><img style="margin-top:2px;" src="_img/bt_registrar.png"/></a>
    </div>
  <div id="conteudo">
    <div id="box_texto">
    <h1>H&aacute; <b><?php echo $online_count; ?></b> Habbies onlines, junte-se a eles!</h1>
    <div id="divisor"></div>

    Habbie Hotel é uma comunidade virtual que te permite criar amizades, quartos e participar de várias<br /> promoções que são feitas durante o dia.<br /><br />

Nosso catálogo é repleto de novidades e com certeza vai agradar o seu gosto, afinal, o hotel foi feito<br /> com intuito de divertir você e seus amigos.<br /><br />

Junte-se a nós agora mesmo, é de graça e não dura nem um minuto.<br />

    </div>
    <div style=" margin:auto;margin-bottom:3px;" id="divisor"></div>
    <div id="mini_fotos"></div>
  </div>
    <div id="creditos">
    &copy; 2013 Habbie Hotel.<br />Recomendação: Use o <a href="http://www.google.com/intl/pt-BR/chrome/"><b>Google Chrome</b></a> para acessar | CMS by <b>Ribom</b>.
                                       <br />Veja Sempre se os seus dados estão <b>corretos!</b>
    </div>
</div>


</body>
</html>

<?php


} else {
header("location:me");
}

?> 