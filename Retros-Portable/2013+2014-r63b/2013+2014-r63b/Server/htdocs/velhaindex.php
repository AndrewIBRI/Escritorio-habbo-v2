<?php 
#####################################################################
# ----------------------------------------------------------------- #
#||||| ProjectHabbo - Desenvolvimendo da CMS                   |||||#
#||||| Baseada em HabboCMS - HTML, PHP, CSS                    |||||#
#||||| © Copyright 2012 Sulake Corporation - Habbo Arquivos    |||||#
#|||||                                                         |||||#
#|||||                                 www.ProjectHabbbo.com.br|||||#
# ----------------------------------------------------------------- #
#####################################################################

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
						$login_error = "Você foi banido do Hotel por: \"" . $bandata['reason'] . "\" seu banimento acabará em: " . date('d/m/Y H:i:s', $bandata['expire']) . "."; #@DURAÇÃO BANIMENTO@#
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
$pagename = "- Crie seu $shortname, Construa seu quarto, Converse e faça novos amigos.";

?>


<?php
} else {
header("location:../me");
}
?>


<!DOCTYPE html>
<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7"> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8"> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9"> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js"> <!--<![endif]-->
<head>
    <meta http-equiv="content-type" content="text/html; charset=ISO-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title><?php echo $shortname; ?> - Crie seu <?php echo $shortname; ?>, construa seu Quarto, converse e faça novos amigos. </title>

    <script>
        var andSoItBegins = (new Date()).getTime();
        var habboPageInitQueue = [];
        var habboStaticFilePath = "<?php echo $path; ?>/web-gallery";
    </script>
    <link rel="shortcut icon" href="<?php echo $path; ?>/web-gallery/v2/favicon.ico" type="image/vnd.microsoft.icon" />

    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Ubuntu:400,700,400italic,700italic">

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/v3_landing.css" type="text/css" /><script src="<?php echo $path; ?>/web-gallery/js/v3_landing_top.js" type="text/javascript"></script>
        <meta name="description" content="<?php echo $shortname; ?> Hotel: faça amigos, divirta-se e fique conhecido!" />
        <meta name="keywords" content="habbo hotel, virtual, mundo, comunidade virtual, grátis, comunidade, avatar, bate papo, online, jovem, rpg, entre, social, grupos, fóruns, seguro, jogue, jogos, online, amigos, jovens, raros, mobis raros, colecionar, expressão, emblemas, diversão, música, celebridade, visita de famosos, celebridades, mmo, mmorpg, rpg online" />
        <meta property="og:image" content="<?php echo $path; ?>/web-gallery/v2/images/facebook/app_habbo_hotel_image.png" />

    <meta name="build" content="63-BUILD2104 - 29.04.2013 11:11 - br" />
    
</head>
<body>

<div id="overlay"></div>


<div id="change-password-form" class="overlay-dialog" style="display: none;">
    <div id="change-password-form-container" class="clearfix form-container">
        <h2 id="change-password-form-title" class="bottom-border">Esqueceu sua senha?</h2>
        <div id="change-password-form-content" style="display: none;">
            <form method="post" action="https://www.habbo.com.br/account/password/identityResetForm" id="forgotten-pw-form">
                <input type="hidden" name="page" value="/?changePwd=true" />
                <span>Escreva na sua conta Habbo o endereço de email:</span>
                <div id="email" class="center bottom-border">
                    <input type="text" id="change-password-email-address" name="emailAddress" value="" class="email-address" maxlength="48"/>
                    <div id="change-password-error-container" class="error" style="display: none;">Por favor, coloque um e-mail válido</div>
                </div>
            </form>
            <div class="change-password-buttons">
                <a href="#" id="change-password-cancel-link">Cancelar</a>
                <a href="#" id="change-password-submit-button" class="new-button"><b>Enviar email</b><i></i></a>
            </div>
        </div>
        <div id="change-password-email-sent-notice" style="display: none;">
            <div class="bottom-border">
                <span>Ei, acabamos de enviar um email com um link para que você altere sua senha<br>
<br>

ATENÇÃO! Lembre-se de checar a pasta "Lixo" também!</span>
                <div id="email-sent-container"></div>
            </div>
            <div class="change-password-buttons">
                <a href="#" id="change-password-change-link">Voltar</a>
                <a href="#" id="change-password-success-button" class="new-button"><b>Fechar</b><i></i></a>
            </div>
        </div>
    </div>
    <div id="change-password-form-container-bottom" class="form-container-bottom"></div>
</div>

<script type="text/javascript">
    function initChangePasswordForm() {
        ChangePassword.init();
    }
    if (window.HabboView) {
        HabboView.add(initChangePasswordForm);
    } else if (window.habboPageInitQueue) {
        habboPageInitQueue.push(initChangePasswordForm);
    }
</script>



<header>
    <div id="border-left"></div>
    <div id="border-right"></div>

<?php if(isset($login_error)){ ?>
        <div id="loginerrorfieldwrapper">
            <div id="loginerrorfield">
            <div><?php echo $login_error; ?></div>
            </div> </div> <?php } ?>

<div id="login-form-container">
    <a href="<?php echo $path; ?>" id="habbo-logo"></a>

    <form name="login" action="?name=&password=true" method="post">


    
    <div id="login_username">
        <div id="login-column-1">
            <label for="login-username">Usuário</label>
            <input type="text" name="username" style="width:197px"; onclick="toggle('ledenlijstpb_box');" autocomplete="on" />
            <input tabindex="5" type="checkbox" name="_login_remember_me" id="credentials-remember-me">
            <label for="credentials-remember-me" class="sub-label">Mantenha-me conectado</label>
        </div>

        <div id="login-column-2">
            <label for="login-password">Senha</label>
            <input type="password" name="password" style="width: 126px"; />
            <a href="#" class="sub-label">Esqueceu a senha?</a>
        </div>

        <div id="login-column-3">
            <input type="submit" value="Login" style="margin: -10000px; position: absolute;">
            <a href="#" tabindex="4" class="button" id="credentials-submit"><b></b><span>Login</span></a>
        </div>

                <div id="login-column-4">
<div id="fb-root"></div>


<a class="fb_button fb_button_large" onclick="assistedLogin(FB); return false;">
    <span class="fb_button_text"><?php echo $online_count; ?> Usuários Onlines</span>
</a>

<div id="rpx-signin">
    <a href="https://www.facebook.com/HabbieBR" class="sub-label" target="_new">Curta no Facebook</a>
</div>  
       </div>
    </div>
</form>
</div>

<script>
    habboPageInitQueue.push(function() {
        if (!LandingPage.focusForced) {
            LandingPage.fieldFocus('credentials-email');
        }
    });
</script>
    <div id="alerts">
<noscript>
<div id="alert-javascript-container">
    <div id="alert-javascript-title">
        Javascript é necessário!
    </div>
    <div id="alert-javascript-text">
        O plugin Javascript está desabilitado em seu navegador. Por favor, ative o Javascript ou utilize um navegador que tenha suporte ao Javascript para entrar no Habbo Hotel :)
    </div>
</div>
</noscript>

<div id="alert-cookies-container" style="display:none">
    <div id="alert-cookies-title">
        Não há suporte para cookies.
    </div>
    <div id="alert-cookies-text">
        Cookies não são habilitados no seu navegador. Por favor, habilite os cookies para poder usar o Habbo.
    </div>
</div>
<script type="text/javascript">
    document.cookie = "habbotestcookie=supported";
    var cookiesEnabled = document.cookie.indexOf("habbotestcookie") != -1;
    if (cookiesEnabled) {
        var date = new Date();
        date.setTime(date.getTime()-24*60*60*1000);
        document.cookie="habbotestcookie=supported; expires="+date.toGMTString();
    } else {
        if (window.habboPageInitQueue) {
            // jquery might not be loaded yet
            habboPageInitQueue.push(function() {
                $('#alert-cookies-container').show();
            });
        } else {
            $('alert-cookies-container').show();
        }
    }
</script>
    </div>
    <div id="top-bar-triangle"></div>
    <div id="top-bar-triangle-border"></div>
</header>


<div id="content">
    <ul>
        <li id="home-anchor">
            <div id="welcome">
                <a href="/quickregister/start" class="button large" id="join-now-button"><b></b><span>Entre Hoje</span><span class="sub">(É grátis)</span></a>
                <div id="slogan">
                    <h1>Bem-vindo ao <?php echo $shortname; ?>,</h1>
                    <p>um lugar divertido com gente incrível.</p>
                    <p><a id="tell-me-more-link" href="#">Saiba Mais!</a></p>
                </div>
            </div>
            <div id="carousel">
                <div id="image1"></div>
                <div id="image2"></div>
                <div id="image3"></div>
                <div id="tell-me-more">O <?php echo $shortname; ?> Hotel é um mundo virtual para jogadores maiores de 13 anos, onde você pode criar seu próprio personagem e projetar seu Quarto do jeito que quiser. Você poderá conhecer novos amigos, organizar festas, cuidar de mascotes virtuais, jogar e criar seus próprios jogos e completar Tarefas. Clique em "Entre Hoje" para começar!</div>
            </div>
            <div id="floaters"></div>
        </li>

        <li id="registration-anchor">

<div id="registration-form" class="hide-captcha">
    <div id="registration-form-header">
        <h2>ID do Usuário</h2>
        <p>Preencha estes detalhes para começar:</p>
    </div>
    <div id="registration-form-main">
        <form id="register-new-user" autocomplete="off">
        <input type="hidden" name="next" value="">
        <div id="registration-form-main-left">
            <label for="registration-birthday">Data de nascimento</label>
            <label for="registration-birthday" class="details">Usaremos isso para restaurar sua conta caso você perca o acesso. Sua data de nascimento nunca será compartilhada publicamente.</label>
            <div id="registration-birthday">
<select name="registrationBean.day" id="registrationBean_day" class="dateselector"><option value="">Dia</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option><option value="11">11</option><option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option><option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option><option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option><option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option><option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option></select> <select name="registrationBean.month" id="registrationBean_month" class="dateselector"><option value="">Mês</option><option value="1">Janeiro</option><option value="2">Fevereiro</option><option value="3">Março</option><option value="4">Abril</option><option value="5">Maio</option><option value="6">Junho</option><option value="7">Julho</option><option value="8">Agosto</option><option value="9">Setembro</option><option value="10">Outubro</option><option value="11">Novembro</option><option value="12">Dezembro</option></select> <select name="registrationBean.year" id="registrationBean_year" class="dateselector"><option value="">Ano</option><option value="2005">2005</option><option value="2004">2004</option><option value="2003">2003</option><option value="2002">2002</option><option value="2001">2001</option><option value="2000">2000</option><option value="1999">1999</option><option value="1998">1998</option><option value="1997">1997</option><option value="1996">1996</option><option value="1995">1995</option><option value="1994">1994</option><option value="1993">1993</option><option value="1992">1992</option><option value="1991">1991</option><option value="1990">1990</option><option value="1989">1989</option><option value="1988">1988</option><option value="1987">1987</option><option value="1986">1986</option><option value="1985">1985</option><option value="1984">1984</option><option value="1983">1983</option><option value="1982">1982</option><option value="1981">1981</option><option value="1980">1980</option><option value="1979">1979</option><option value="1978">1978</option><option value="1977">1977</option><option value="1976">1976</option><option value="1975">1975</option><option value="1974">1974</option><option value="1973">1973</option><option value="1972">1972</option><option value="1971">1971</option><option value="1970">1970</option><option value="1969">1969</option><option value="1968">1968</option><option value="1967">1967</option><option value="1966">1966</option><option value="1965">1965</option><option value="1964">1964</option><option value="1963">1963</option><option value="1962">1962</option><option value="1961">1961</option><option value="1960">1960</option><option value="1959">1959</option><option value="1958">1958</option><option value="1957">1957</option><option value="1956">1956</option><option value="1955">1955</option><option value="1954">1954</option><option value="1953">1953</option><option value="1952">1952</option><option value="1951">1951</option><option value="1950">1950</option><option value="1949">1949</option><option value="1948">1948</option><option value="1947">1947</option><option value="1946">1946</option><option value="1945">1945</option><option value="1944">1944</option><option value="1943">1943</option><option value="1942">1942</option><option value="1941">1941</option><option value="1940">1940</option><option value="1939">1939</option><option value="1938">1938</option><option value="1937">1937</option><option value="1936">1936</option><option value="1935">1935</option><option value="1934">1934</option><option value="1933">1933</option><option value="1932">1932</option><option value="1931">1931</option><option value="1930">1930</option><option value="1929">1929</option><option value="1928">1928</option><option value="1927">1927</option><option value="1926">1926</option><option value="1925">1925</option><option value="1924">1924</option><option value="1923">1923</option><option value="1922">1922</option><option value="1921">1921</option><option value="1920">1920</option><option value="1919">1919</option><option value="1918">1918</option><option value="1917">1917</option><option value="1916">1916</option><option value="1915">1915</option><option value="1914">1914</option><option value="1913">1913</option><option value="1912">1912</option><option value="1911">1911</option><option value="1910">1910</option><option value="1909">1909</option><option value="1908">1908</option><option value="1907">1907</option><option value="1906">1906</option><option value="1905">1905</option><option value="1904">1904</option><option value="1903">1903</option><option value="1902">1902</option><option value="1901">1901</option><option value="1900">1900</option></select>             </div>
            <label for="registration-email">Email</label>
            <label for="registration-email" class="details">Você precisará utilizar este <b>endereço de email para entrar</b> no Habbo no futuro. Por favor utilize um email válido.</label>
            <input type="email" name="registrationBean.email" id="registration-email" value="">


            

        </div>
        <div id="registration-form-main-right">

            <span id="password-field-container">
                <label for="registration-password">Nova senha</label>
                <label for="registration-password" class="details">Senhas precisam ter no mínimo <b>6 caracteres </b>, incluíndo <b>letras e números</b></label>
                <input type="password" name="registrationBean.password" id="registration-password" maxlength="32" value="">
            </span>

            <div id="captcha-container">

                <label for="recaptcha_response_field">Captcha</label>
                <label for="recaptcha_response_field" class="details">Digite as duas palavras (separadas com um espaço):</label>

                    <script src="https://www.google.com/recaptcha/api/js/recaptcha_ajax.js"></script>
                    <script>
                        habboPageInitQueue.push(function() {
                            Registration.setRecaptchaKey("6LeMMQYAAAAAAEawUZsMUSdErdmxdUk0C3snHg9U");
                        });
                    </script>
                    <div id="captcha-image-container">
                        <div id="recaptcha_image"></div>
                        <div id="captcha-overlay"></div>
                    </div>
                    <p id="captcha-new" class="details"><a class="recaptcha-reload" href="#">Tente um novo código</a></p>
                    <input type="text" name="recaptcha_response_field" id="recaptcha_response_field">

            </div>
            <p class="checkbox-container" id="registration-tos">
                <input type="checkbox" id="tos" name="registrationBean.termsOfServiceSelection" value="true">
                <label for="tos" class="details checkbox">
                    Eu aceito o <a href="http://help.habbo.com.br/entries/20477982-termos-e-condicoes" target="_blank" onclick="window.open('http://help.habbo.com.br/entries/20477982-termos-e-condicoes'); return false;">Termos de Serviço</a>
                </label>
            </p>
            <p class="checkbox-container">

                <input type="checkbox" id="registration-marketing" value="true" name="registrationBean.marketing">
                <label for="registration-marketing" class="details checkbox">Envie-me novidades do Hotel, incluindo o Boletim Habbo semanal.</label>
            </p>
            <div class="submit-button-wrapper">
                <a href="#" class="button large not-so-large register-submit"><b></b><span>Pronto!</span></a>
            </div>
        </div>

        <div id="parent-email-container" style="display: none;">
            <label for="parent-email">Email dos pais</label>
            <label for="parent-email" class="details">Se você tem menos de 13 anos de idade, nós precisaremos entrar em contato com seus pais ou responsáveis para avisá-los que você entrou no Habbo.</label>
            <input type="email" id="parent-email" name="registrationBean.parentEmail" value="">
            <div class="submit-button-wrapper">
                <a href="#" class="button large not-so-large register-submit"><b></b><span>Pronto!</span></a>
            </div>
        </div>
        </form>
    </div>
</div>
<div id="magnifying-glass"></div>
            <div id="sail"></div>
        </li>
    </ul>
</div>

<footer>
        <div id="partner-logo"><a href="" style="background-image: url('//habboo-a.akamaihd.net/c_images/Landingpage_partner_logos/recomendado.png')"></a></div>
    <div id="age-recommendation"></div>

    <div id="footer-content" class="partner-logo-present">
        <div id="footer"><a href="<?php echo $path; ?>">Início</a> / <a href="<?php echo $path; ?>/contato" target="_new">Contate-nos</a> / <a href="<?php echo $path; ?>/safety/habbo_way"><?php echo $shortname; ?> Etiqueta</a> / <a href="<?php echo $path; ?>/safety/safety_tips">Segurança</a></div>
        <div id="copyright">© 2012 - 2013 <?php echo $shortname; ?> Hotel <b>(Hotel Virtual)</b>. Todos os direitos reservados.<br />Este WebSite não é de propriedade ou operado pela Sulake Corporation Oy / Desenvolved by Orium
    </div>
    
</footer>


<script src="<?php echo $path; ?>/web-gallery/js/v3_landing_bottom.js" type="text/javascript"></script><!--[if IE]><script src="https://habboo-a.akamaihd.net/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1699/web-gallery/static/js/v3_ie_fixes.js" type="text/javascript"></script><![endif]-->



<script type="text/javascript">
    var rpxJsHost = (("https:" == document.location.protocol) ? "https://" : "http://static.");
    document.write(unescape("%3Cscript src='" + rpxJsHost +
            "rpxnow.com/js/lib/rpx.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
    RPXNOW.overlay = false;
    RPXNOW.language_preference = 'pt'; 
    RPXNOW.flags = 'show_provider_list';
</script>

<script type="text/javascript">
  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-448325-21']);
  _gaq.push(['_trackPageview']);
  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();
</script>    
    

    


</body>
</html>