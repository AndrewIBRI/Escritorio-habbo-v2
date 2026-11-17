<?php
#############################################################################
#|-| |-| Index desenvolvida por Daniel Tenorio e por Matheus Silva   |-| |-|#
#|-| |-| Index desenvolvida por Daniel Tenorio e por Matheus Silva   |-| |-|#
#|-| |-| Index desenvolvida por Daniel Tenorio e por Matheus Silva   |-| |-|#
# ------------------------------------------------------------------------- #
#|-| |-| Index desenvolvida por Daniel Tenorio e por Matheus Silva   |-| |-|#
#|-| |-| Index desenvolvida por Daniel Tenorio e por Matheus Silva   |-| |-|#
#|-| |-| Index desenvolvida por Daniel Tenorio e por Matheus Silva   |-| |-|#
#############################################################################

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
			$login_error = "UsuÃ¡rio ou senha incorretos!";
		} else {
			$sql = mysql_query("SELECT id FROM users WHERE username = '".$username."' AND password = '".$password."' OR username = '".$username."' AND password = '".$password2."' LIMIT 1") or die(mysql_error());
			if(mysql_num_rows($sql) < 1){
				$login_error = "UsuÃ¡rio ou senha incorretos!";
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
						$login_error = "VocÃª foi banido por: \"" . $bandata['reason'] . "\" e acaba em: " . date('d.m.Y - H:i:s', $bandata['expire']) . ".";
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
	

<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />

<title><?php echo $shortname; ?> -  </title>

<script type="text/javascript">
var andSoItBegins = (new Date()).getTime();
</script>



<link rel="stylesheet" href="/css/style.css" type="text/css" />


<div id="overlay"></div>


<div id="header-container">
	<div id="header" class="clearfix">
		<h1><a href="index"></a></h1>

<a href="<?php echo $path;?>/me">
			<div class="logo">
			</div>
		</a>
			
<ul>

</ul>



<p>
<p>

		</div>
</div>


		

</head>
<body>
	<center>
		<div id="container-border">
			<div id="container">
				<div id="logo-container">
					<a class="logo" style="width: 123px;height: 80px;margin: 5px;float: left;"></a>
					<div class="count">
						

					</div>
				</div>
				<div class="content_box" style="float:right;margin-right:20px;">
					<div class="content_padding smaller">
						
<div id="contentTitle" class="blue">Faça seu Login </div>

						<div class="content_padding">
														<form action="" method="post" autocomplete="off">
								<div class="input_div">
									<label>Nome de usuário</label><br>
									<input type="text" class="input_form" name="username" value="">
								</div>
								<div class="input_div" style="position:relative;">
									<label>Senha</label><br>
									<input type="password" class="input_form" name="password" value="">
									<input type="submit" class="button black index" name="iLogin" value="Entrar">
								</div>
<?php

				} else {
				header("location:me.php");
				}

				?>

							</form>
							<div class="input_div">
								<center>
																		<b>Cadastre-se</b> e junte-se aos <b><?php echo $online_count; ?></b> usuários online! 
								</center>
							</div>
								<a href="../quickregister/start" style="text-decoration:none;">
									<div class="content_title green index">Crie uma conta! Gratuitamente;</div>
								</a>
							</div>
						</div>
					</div>
				</div>
			</div>

<p>
<div style="text-shadow:0 1px 1px #CCC;font-size:16px;font-family:Arial, Helvetica, sans-serif;margin-top:15px;font-size:11px;text-align:center;"><p class="copyright">© <?php echo $shortname; ?>  2013 - 2014 Todos os direitos reservados.  Este site não possui fins comerciais oy. <br> <b>mani0nsCMS</b> v2 Criada e Programa por: <b>nadi0s</b> e <b>Houshed</b>. <br></a></p></div>

		</center>
	</body>
</html>


