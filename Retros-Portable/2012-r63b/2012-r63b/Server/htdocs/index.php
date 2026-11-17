<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

require_once('./data_classes/server-data.php_data_classes-core.php.php');

session_start();

if(!session_is_registered(username)){

	if(isset($_POST['username']) && isset($_POST['password'])){
	$username = FilterText($_POST['username']);
	$password = FilterText(HoloHash($_POST['password'], $username));
	$password2 = FilterText(HoloHashMD5($_POST['password'], $username));
		
		if(empty($username) || empty($password)){
			$login_error = "Digite seu e-mail e senha para entrar.";
		} else {
			$sql = mysql_query("SELECT id FROM users WHERE username = '".$username."' AND password = '".$password."' OR username = '".$username."' AND password = '".$password2."' LIMIT 1") or die(mysql_error());
			if(mysql_num_rows($sql) < 1){
				$login_error = "Sua senha e <?php echo $shortname; ?> Nome não correspondem.";
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
						$login_error = "Você foi banido! A razão para a proibição foi \"" . $bandata['reason'] . "\" e durou até " . date('d.m.Y - H:i:s', $bandata['expire']) . ".";
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
$pagename = " Crie seu Habbo, construa seu Quarto, converse e faça novos amigos.";

include('./templates/login_subheader.php');
?>

<style type="text/css">
	body {
             background-color: #000000;
        }
        #footer {
            color: #595959;
        }

        #footer .footer-links a {
            color: #333333;
        }

        #footer .copyright {
            color: #595959;
        }
    </style>

<body id="frontpage">

<div id="overlay"></div>

<div id="site-header">


    <form id="loginformitem" name="loginformitem" action="<?php echo $path; ?>?name=<?php echo FilterText($_POST['username']); ?>&password=true"
          method="post">
<?php if(isset($login_error)){ ?>
        <div id="loginerrorfieldwrapper">
            <div id="loginerrorfield">
            <div><?php echo $login_error; ?></div>
            </div> </div> <?php } ?>

        <div style="clear: both;"></div>

        <div id="site-header-content">

            <div id="habbo-logo"></div>

            <div id="login-form">


                <div id="login-form-email">
                    <label for="login-username"
                           class="login-text"><?php echo $shortname; ?> Nome</label>
                    <input tabindex="3" type="text" class="login-field" name="username" id="login-username"
                           value="" maxlength="48"/>
                    <input tabindex="6" type="checkbox" name="_login_remember_me" id="login-remember-me"
                           value="true"/>
                    <label for="login-remember-me">Manter-me conectado</label>

<div id="landing-remember-me-notification" class="bottom-bubble" style="display:none;">
	<div class="bottom-bubble-t"><div></div></div>
	<div class="bottom-bubble-c">
                Selecionar esta opção permanecer conectado ao <?php echo $shortname; ?> até que a opção des 'Desconectar' 
	</div>
	<div class="bottom-bubble-b"><div></div></div>
</div>

                </div>

                <div id="login-form-password">
                    <label for="login-password" class="login-text">Senha</label>
                    <input tabindex="4" type="password" class="login-field" name="password"
                           id="login-password" maxlength="32"/>

                    <div id="login-forgot-password">
                        <a href="./forgot"
                           id="forgot-password"><span>Esqueceu sua senha?</span></a>
                    </div>
                </div>

                <div id="login-form-submit">
                    <input type="submit" value="Login" class="login-top-button"
                           id="login-submit-button"/>
                    <a href="#" tabindex="5"f
                       id="login-submit-new-button"><span>Login</span></a>

                </div>      </div><br><br><table background=/Premiun_Images/png.png height=39 width=166> <tr> <td><div align="center"><b><font color="black"><?php echo $online_count; ?> <?php echo $shortname; ?>'s Online</h1></font></b></div></td> </tr> </table>
            

	 <script type="text/javascript">
                HabboView.add(function() {
                    fieldFocus('login-username');
                    popdownSetupRun();
                });
                HabboView.add(function() {
                    LandingPage.init();
                });
            </script>

        </div>

    </form>

</div>

<div id="fp-container">
    <div id="content">
    <div id="column1" class="column">
			     		
				<div class="habblet-container ">		
	
						<div style="width: 900px; margin: 0 auto">
<div id="tagline">Você não imagina o que acontece neste Hotel Maluco!</div>

		
 </div>

</div>

<div id="frontpage-image-container">


    <div id="join-now-button-container">
        <div id="join-now-button-wrapper-fb">
            <div class="join-now-alternative">

            </div>
            <div class="join-now-button">
                <a class="join-now-link" href="#" onclick="assistedLogin(FB); return false;"> 
                    <div class="join-now-text-big">Usa <?php echo $shortname; ?></div>
                    <div class="join-now-text-small">com<span
                            class="fbword">Facebook</span></div>

                </a>
                <span class="close"></span>
            </div>
            <div class="join-now-alternative">
                <a id="register-link-fb" href="/quickregister/start" onclick="startRegistration(); return false;">
                o crea una <?php echo $shortname; ?> cuenta
                </a>
            </div>
        </div>

        <div id="join-now-button-wrapper">
            <div class="join-now-alternative">
                <a href="./quickregister/start" class="newusers" onclick="startRegistration(); return false;"><b>Caro visitente,<span style="color: #8f8f8f;">clique aqui</span></a>
            </div>
            <div class="join-now-button">
                <a class="join-now-link" id="register-link" href="/quickregister/start"
                   onclick="startRegistration(); return false;"> 
                    <div class="join-now-text-big">Entre agora</div>

                    <div class="join-now-text-small">de graça</div>
                </a>
                <span class="close"></span>
            </div>

        </div>
    </div>
    <script type="text/javascript">

        function startRegistration() {
    window.location.replace("/quickregister/start");
        }

    </script>


</div><br>
<a href="./quickregister/start" id="frontpage-image"
   style="background-image: url('/web-gallery/v2/images/frontpage/horse_building_bigbox.png')"></a>
</div>
         </div>

<div id="tags-main-container">

    

</div>

<script src="http://px.smowtion.com/validate?sid=150305" type="text/javascript"></script>
	
<?php require_once('./templates/login_footer.php');?>
							
					
				</div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
			 

</div>


<?php



} else {
header("location:../me");
}

?>