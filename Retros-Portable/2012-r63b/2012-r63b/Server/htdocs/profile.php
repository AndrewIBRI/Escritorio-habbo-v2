<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Meu perfil";
$body_id = "profile";
$pageid = "2";

if(isset($_GET['web-profile-tab'])){
	if($_GET['web-profile-tab'] < 2 || $_GET['web-profile-tab'] > 5 || !$_GET['web-profile-tab']){
		header("Location: ".$path."/profile/2");
		$tab = 0;
		exit;
	} else {
		$tab = FilterText($_GET['web-profile-tab']);
	}
} else {
	$tab = "2";
}

if($tab == "2"){
	if(isset($_POST['save'])){
		if(strlen($_POST['motto']) > 32){
			$result = "&iexcl;Tu misi&oacute;n es demasiado larga!";
			$error = "1";
		} else {
			if($_POST['block_newfriends'] == "true"){ $block_newfriends = '0'; }else{ $block_newfriends = '1'; }
			if($_POST['hide_online'] == "true"){ $hide_online = '0'; }else{ $hide_online = '1'; }
			if($_POST['hide_inroom'] == "true"){ $hide_inroom = '0'; }else{ $hide_inroom = '1'; }

			mysql_query("UPDATE users SET motto = '".FilterText($_POST['motto'])."', visibility = '".FilterText($_POST['visibility'])."', block_newfriends = '".$block_newfriends."', hide_online = '".$hide_online."', hide_inroom = '".$hide_inroom."', mymusik = '".FilterText($_POST['mymusik'])."' WHERE id = '".$my_id."' LIMIT 1") or die(mysql_error());
			$result = "Ajustes atualizados";
		}
	}
} else if($tab == "3"){
	if(isset($_POST['save'])){
	$pass1 = FilterText($_POST['password']);
	$pass1_hash = HoloHashMD5($pass1, $myrow['username']);
	$day1 = FilterText($_POST['day']);
	$month1 = FilterText($_POST['month']);
	$year1 = FilterText($_POST['year']);
		$formatted_dob = "".$day1.".".$month1.".".$year1."";
	$mail1 = FilterText($_POST['email']);
	$themail = $mail1;
		//checks password --encryption--
		if($pass1_hash == $myrow['password'] && $formatted_dob == $myrow['birth']){
		$email_check = preg_match("/^[a-z0-9_\.-]+@([a-z0-9]+([\-]+[a-z0-9]+)*\.)+[a-z]{2,7}$/i", $mail1);
			if($email_check == "1"){
			mysql_query("UPDATE users SET mail = '".$mail1."' WHERE username = '".$rawname."' and password = '".$rawpass."'") or die(mysql_error());

			$result = "Tu email se ha cambiado con &eacute;xito a \"".$mail1."\".";
			} else {
			$result = "Direcci&oacute;n de correo electr&oacute;nico incorrecto.";
			$error = "1";
			}
		} else {
		$result = "&iexcl;Los datos son incorrectos!";
		$error = "1";
		}
	} else {
	$themail = $myrow['mail'];
	}

} else if($tab == "4"){
	if(isset($_POST['save'])){
	$pass1 = FilterText($_POST['password']);
	//Hashes and salts the old password with the user id (in lowercase) --encryption--
	$pass1_hash = HoloHashMD5($pass1, $myrow['name']);
	$day1 = FilterText($_POST['day']);
	$month1 = FilterText($_POST['month']);
	$year1 = FilterText($_POST['year']);
	$formatted_dob = "".$day1.".".$month1.".".$year1."";
	$newpass = FilterText($_POST['pass']);
	//Hashes and salts the new password with the user id (in lowercase) --encryption--
	$newpass_hash = HoloHashMD5($newpass, $rawname);
	$newpass_conf = FilterText($_POST['confpass']);
		if($pass1_hash == $myrow['password'] && $formatted_dob == $myrow['birth']){
			if($newpass == $newpass_conf){
				if(strlen($newpass) < 6){
				$result = "La contrase&ntilde;a debe contener al menos 6 car&aacute;cteres.";
				$error = "1";
				} else {
					if(strlen($newpass) > 51){
					$result = "&iexcl;Tu contrase&ntilde;a es demasiado larga! Debe contener 50 car&aacute;cteres como m&aacute;ximo.";
					$error = "1";
					} else {
					//Updates password --encryption--
					mysql_query("UPDATE users SET password = '".$newpass_hash."' WHERE username = '".$rawname."' and password = '".$rawpass."'") or die(mysql_error());
					$result = "Contrase&ntilde;a cambiada correctamente. Por favor, entra de nuevo.";
					}
				}
			} else {
			$result = "Las contrase&nitlde;as no coinciden.";
			$error = "1";
			}
		} else {
		$result = "Los datos no coinciden con los de nuestra base de datos.";
		$error = "1";
		}
	}

}

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
	<div id="content">
    <div>
<div class="content">
<div class="habblet-container" style="float:left; width:210px;">
<div class="cbb settings">

<h2 class="title">Ajustes</h2>
<div class="box-content">
            <div id="settingsNavigation">
            <ul>
		<?php
		if($tab == "2"){
                echo "<li class='selected'>Meu Estado
                </li>";
		} else {
                echo "<li><a href='".$path."/profile/2'>Meu Estado
                </li>";
		}

		if($tab == "3"){
                echo "<li class='selected'>Email e Verificação
                </li>";
		} else {
                echo "<li><a href='".$path."/profile/3'>Email e Verificação</a>
                </li>";
		}

		if($tab == "4"){
                echo "<li class='selected'>Minha Senha
                </li>";
		} else {
                echo "<li><a href='".$path."/profile/4'>Minha Senha</a>
                </li>";
		}
		?>
            </ul>
            </div>
</div></div>
<?php if($hc < 1){ ?>
    <div class="cbb habboclub-tryout">
        <h2 class="title">&Uacute;nete al <?php echo $shortname; ?> Club</h2>
        <div class="box-content">
            <div class="habboclub-banner-container habboclub-clothes-banner"></div>
            <p class="habboclub-header"><?php echo $shortname; ?> Club es el Club de los <?php echo $shortname; ?>s VIP. Sus miembros cuentan con una larga lista de beneficios: desde ropa exclusiva a regalos gratis, pasando por un l&iacute;mite para Amigos en la Consola mucho mayor</p>
            <p class="habboclub-link"><a href="../credits/habboclub">Ir al <?php echo $shortname; ?> Club &gt;&gt;</a></p>
        </div>
    </div>
<?php } ?>
</div>

<?php if($tab == "2"){ ?>
    <div class="habblet-container " style="float:left; width: 560px;">
        <div class="cbb clearfix settings">

            <h2 class="title">Edite seu perfil</h2>
            <div class="box-content">



<form action="profile/2" method="post">
<input type="hidden" name="tab" value="2" />
<input type="hidden" name="__app_key" value="HoloCMS" />

<?php

if(!empty($result)){
	if($error == "1"){
		echo "<div class='rounded rounded-red'>";
	} else {
		echo "<div class='rounded rounded-green'>";
	}

	echo $result . "<br />
	</div><br />";
}

$user_sql = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");
$user_row = mysql_fetch_assoc($user_sql);

?>

<h3>Seu pensamento</h3>

<p> Seu pensamento pode&nbsp;ser visto por todos, pense novamente! </p>

<p>
<span class="label">Pensamento:</span>
<input type="text" name="motto" size="32" maxlength="32" value="<?php echo HoloText($user_row['motto']); ?>" id="avatarmotto" />
</p>

<h3>Sua P&aacute;gina</h3>

<p> Permitir que outros Habbos&nbsp;para ver&nbsp;sua p&aacute;gina pessoal: <br>
<label><input name="visibility" value="EVERYONE" <?php if($user_row['visibility'] == "EVERYONE"){ ?>checked="checked" <?php } ?>type="radio">Todos</label>
<label><input name="visibility" value="FRIENDS" <?php if($user_row['visibility'] == "FRIENDS"){ ?>checked="checked" <?php } ?>type="radio">
Apenas meus amigos</label><label><input name="visibility" value="NOBODY" <?php if($user_row['visibility'] == "NOBODY"){ ?>checked="checked" <?php } ?>type="radio">
N&atilde;o</label>
</p>

<h3>Pedidos de amizade</h3>
<p>

<label>
<input name="block_newfriends" <?php if($user_row['block_newfriends'] == 0){ ?>checked="checked"<?php } ?> value="true" type="checkbox">
Pedidos de amigos ativado</label>
</p>

<h3>Estado da conex&atilde;o</h3>
<p>

<label>
<input name="hide_online" <?php if($user_row['hide_online'] == 0){ ?>checked="checked"<?php } ?> value="true" type="checkbox">
Estado de ativa&ccedil;&atilde;o&nbsp;online para todos</label>
</p>

<h3>Ajustes 'Siga-me'</h3>
<p>

<label>
<input name="hide_inroom" <?php if($user_row['hide_inroom'] == 0){ ?>checked="checked"<?php } ?> value="true" type="checkbox">
Seguimento de Amigos ativado</label>
</p>

<h3><? $shortname; ?> M&uacute;sica</h3>
<p> Escreva&nbsp;um link&nbsp;do youtube com&nbsp;sua m&uacute;sica favorita&nbsp;para ouvi-la em sua Home.<br><br>
  <b>Nota:</b> O link deve ser diferente exemplo:<br><font color="green">http://youtube.com/</font><font color="red">watch?</font><font color="green">v</font><font color="red">=</font><font color="green">nciFjxOYe74</font> = <font color="green">http://youtube.com/v/nciFjxOYe74</font><br><br>
<label>
Link: <input type="text" name="mymusik" size="32" value="<?php echo FilterText($user_row['mymusik']); ?>">
</label></p>

<?php if($hc > 0){ ?>
<h3><?php echo $shortname; ?> Club</h3>

<p>Ja &eacute; membro <?php echo $shortname; ?> Club. Seu Lord club expira em <b>
<?php echo getHCdays($my_id); ?></b> dias. Se quizer renovar com o Lord Club click <a href='./client'>aqui</a>.</p>
<?php }?>

<?php if($vip > 0){ ?>
<h3>Usuario VIP</h3>

<p>Voc&ecirc; &eacute; um usu&aacute;rio VIP. Seu VIP expira em<b><?php echo getVIPDays($my_id); ?></b> dias. Se deseja renovar com o Lord VIP click <a href='../credits'>aqui</a>.</p>
<?php }?>
<div class="settings-buttons">
<input type="submit" value="Salvar altera&ccedil;&otilde;es" name="save" class="submit" />
</div>

</form>

</div>
</div>
</div>
</div></div>
    </div>
<?php } else if($tab == "3"){ ?>
    <div class="habblet-container " style="float:left; width: 560px;">
        <div class="cbb clearfix settings">

            <h2 class="title">Ajustes de E-mail</h2>
            <div class="box-content">

<?php
if(!empty($result)){
	if($error == "1"){
	echo "<div class='rounded rounded-red'>";
	} else {
	echo "<div class='rounded rounded-green'>";
	}
	echo "".$result."<br />
	</div><br />";
}
?>

<form action="profile/3" method="post" id="emailform">
<input type="hidden" name="tab" value="3" />
<input type="hidden" name="__app_key" value="HoloCMS" />
ATEN&Ccedil;&Atilde;O: N&atilde;o d&ecirc;&nbsp;o seu endere&ccedil;o de&nbsp;e-mail&nbsp;para&nbsp;Ningu&eacute;m.&nbsp;Voc&ecirc; pode perder tudo!
<div class="settings-step">

	<h4>1.</h4>
	<div class="settings-step-content">

<h3>Introduza seus dados atuais.</h3>

<p>
 <label for="currentpassword">Senha atual:</label><br />
 <input type="password" size="32" maxlength="32" name="password" id="currentpassword" class="currentpassword " />
</p>

<p>
<label for="mail">E-mail actual:</label><br />
<input type="text" name="mail" id="mail" READONLY size="32" maxlength="48" value="<?php echo $myrow['mail']; ?>" />
</p>

<div>
<div><label for="birthdate">Fecha de nacimiento:</label></div>
<div id="required-birthday" ><select name="day" id="day" class="dateselector"><option value="">Dia</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option><option value="11">11</option><option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option><option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option><option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option><option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option><option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option></select> <select name="month" id="month" class="dateselector"><option value="" selected="selected">Mes</option><option value="1">Janeiro</option><option value="2">Fevereiro</option><option value="3">Março</option><option value="4">Abril</option><option value="5">Maio</option><option value="6">Junho</option><option value="7">Julho</option><option value="8">Agosto</option><option value="9">Setembro</option><option value="10">Outubro</option><option value="11">Novembro</option><option value="12">Dezembro</option>
</select> <select name="year" id="year" class="dateselector"><option value="" selected="selected">Ano</option><option value="2010">2010</option><option value="2009">2009</option><option value="2008">2008</option><option value="2007">2007</option><option value="2006">2006</option><option value="2005">2005</option><option value="2004">2004</option><option value="2003">2003</option><option value="2002">2002</option><option value="2001">2001</option><option value="2000">2000</option><option value="1999">1999</option><option value="1998">1998</option><option value="1997">1997</option><option value="1996">1996</option><option value="1995">1995</option><option value="1994">1994</option><option value="1993">1993</option><option value="1992">1992</option><option value="1991">1991</option><option value="1990">1990</option><option value="1989">1989</option><option value="1988">1988</option><option value="1987">1987</option><option value="1986">1986</option><option value="1985">1985</option><option value="1984">1984</option><option value="1983">1983</option><option value="1982">1982</option><option value="1981">1981</option><option value="1980">1980</option><option value="1979">1979</option><option value="1978">1978</option><option value="1977">1977</option><option value="1976">1976</option><option value="1975">1975</option><option value="1974">1974</option><option value="1973">1973</option><option value="1972">1972</option><option value="1971">1971</option><option value="1970">1970</option><option value="1969">1969</option><option value="1968">1968</option><option value="1967">1967</option><option value="1966">1966</option><option value="1965">1965</option><option value="1964">1964</option><option value="1963">1963</option><option value="1962">1962</option><option value="1961">1961</option><option value="1960">1960</option><option value="1959">1959</option><option value="1958">1958</option><option value="1957">1957</option><option value="1956">1956</option><option value="1955">1955</option><option value="1954">1954</option><option value="1953">1953</option><option value="1952">1952</option><option value="1951">1951</option><option value="1950">1950</option><option value="1949">1949</option><option value="1948">1948</option><option value="1947">1947</option><option value="1946">1946</option><option value="1945">1945</option><option value="1944">1944</option><option value="1943">1943</option><option value="1942">1942</option><option value="1941">1941</option><option value="1940">1940</option><option value="1939">1939</option><option value="1938">1938</option><option value="1937">1937</option><option value="1936">1936</option><option value="1935">1935</option><option value="1934">1934</option><option value="1933">1933</option><option value="1932">1932</option><option value="1931">1931</option><option value="1930">1930</option><option value="1929">1929</option><option value="1928">1928</option><option value="1927">1927</option><option value="1926">1926</option><option value="1925">1925</option><option value="1924">1924</option><option value="1923">1923</option><option value="1922">1922</option><option value="1921">1921</option><option value="1920">1920</option><option value="1919">1919</option><option value="1918">1918</option><option value="1917">1917</option><option value="1916">1916</option><option value="1915">1915</option><option value="1914">1914</option><option value="1913">1913</option><option value="1912">1912</option><option value="1911">1911</option><option value="1910">1910</option><option value="1909">1909</option><option value="1908">1908</option><option value="1907">1907</option><option value="1906">1906</option><option value="1905">1905</option><option value="1904">1904</option><option value="1903">1903</option><option value="1902">1902</option><option value="1901">1901</option><option value="1900">1900</option>
</select> </div>
</div>

</div></div>

<div class="settings-step">

	<h4>2.</h4>
	<div class="settings-step-content">

<h3>Escreva um novo e-mail</h3>

<p>Tenha certeza de que introduzir&aacute; corretamente</p>

<p>
<label for="email">E-mail:</label><br />
<input type="text" name="email" id="email" size="32" maxlength="48" value="<?php echo $themail; ?>" />
</p>

	</div>
</div>

<div class="settings-buttons">
<input type="submit" value="Salvar Modifica&ccedil;&otilde;es" name="save" class="submit" />
</div>

</form>

</div></div></div></div>


</div>
</div>

<?php } else if($tab == "4"){ ?>

<div class="habblet-container " style="float:left; width: 560px;">
        <div class="cbb clearfix settings">

            <h2 class="title">Troca de senha</h2>
            <div class="box-content">

<?php
if(!empty($result)){
	if($error == "1"){
	echo "<div class='rounded rounded-red'>";
	} else {
	echo "<div class='rounded rounded-green'>";
	}
	echo "".$result."<br />
	</div><br />";
}
?>

Aten&ccedil;&atilde;o nunca diga sua senha a ningu&eacute;m

<form action="profile/4" method="post" id="passwordform">
<input type="hidden" name="tab" value="4" />
<input type="hidden" name="__app_key" value="HoloCMS" />

<div class="settings-step">

	<h4>1.</h4>
	<div class="settings-step-content">

<h3>Introduza seus dados atuais.</h3>

<p>
 <label for="currentpassword">Senha atual:</label><br />
 <input type="password" size="32" maxlength="32" name="password" id="currentpassword" class="currentpassword " />
</p>

<div>
<div>
  <label for="birthdate">Data de nascimento</label>
</div>
<div id="required-birthday" ><select name="day" id="day" class="dateselector"><option value="">Dia</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option><option value="11">11</option><option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option><option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option><option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option><option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option><option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option></select> <select name="month" id="month" class="dateselector"><option value="" selected="selected">Mes</option><option value="1">Janeiro</option><option value="2">Fevereiro</option><option value="3">Março</option><option value="4">Abril</option><option value="5">Maio</option><option value="6">Junho</option><option value="7">Julho</option><option value="8">Agosto</option><option value="9">Setembro</option><option value="10">Outubro</option><option value="11">Novembro</option><option value="12">Dezembro</option>
</select> <select name="year" id="year" class="dateselector"><option value="" selected="selected">Ano</option><option value="2010">2010</option><option value="2009">2009</option><option value="2008">2008</option><option value="2007">2007</option><option value="2006">2006</option><option value="2005">2005</option><option value="2004">2004</option><option value="2003">2003</option><option value="2002">2002</option><option value="2001">2001</option><option value="2000">2000</option><option value="1999">1999</option><option value="1998">1998</option><option value="1997">1997</option><option value="1996">1996</option><option value="1995">1995</option><option value="1994">1994</option><option value="1993">1993</option><option value="1992">1992</option><option value="1991">1991</option><option value="1990">1990</option><option value="1989">1989</option><option value="1988">1988</option><option value="1987">1987</option><option value="1986">1986</option><option value="1985">1985</option><option value="1984">1984</option><option value="1983">1983</option><option value="1982">1982</option><option value="1981">1981</option><option value="1980">1980</option><option value="1979">1979</option><option value="1978">1978</option><option value="1977">1977</option><option value="1976">1976</option><option value="1975">1975</option><option value="1974">1974</option><option value="1973">1973</option><option value="1972">1972</option><option value="1971">1971</option><option value="1970">1970</option><option value="1969">1969</option><option value="1968">1968</option><option value="1967">1967</option><option value="1966">1966</option><option value="1965">1965</option><option value="1964">1964</option><option value="1963">1963</option><option value="1962">1962</option><option value="1961">1961</option><option value="1960">1960</option><option value="1959">1959</option><option value="1958">1958</option><option value="1957">1957</option><option value="1956">1956</option><option value="1955">1955</option><option value="1954">1954</option><option value="1953">1953</option><option value="1952">1952</option><option value="1951">1951</option><option value="1950">1950</option><option value="1949">1949</option><option value="1948">1948</option><option value="1947">1947</option><option value="1946">1946</option><option value="1945">1945</option><option value="1944">1944</option><option value="1943">1943</option><option value="1942">1942</option><option value="1941">1941</option><option value="1940">1940</option><option value="1939">1939</option><option value="1938">1938</option><option value="1937">1937</option><option value="1936">1936</option><option value="1935">1935</option><option value="1934">1934</option><option value="1933">1933</option><option value="1932">1932</option><option value="1931">1931</option><option value="1930">1930</option><option value="1929">1929</option><option value="1928">1928</option><option value="1927">1927</option><option value="1926">1926</option><option value="1925">1925</option><option value="1924">1924</option><option value="1923">1923</option><option value="1922">1922</option><option value="1921">1921</option><option value="1920">1920</option><option value="1919">1919</option><option value="1918">1918</option><option value="1917">1917</option><option value="1916">1916</option><option value="1915">1915</option><option value="1914">1914</option><option value="1913">1913</option><option value="1912">1912</option><option value="1911">1911</option><option value="1910">1910</option><option value="1909">1909</option><option value="1908">1908</option><option value="1907">1907</option><option value="1906">1906</option><option value="1905">1905</option><option value="1904">1904</option><option value="1903">1903</option><option value="1902">1902</option><option value="1901">1901</option><option value="1900">1900</option>
</select> </div>
</div>

	</div>
</div>
<div class="settings-step">

	<h4>2.</h4>
	<div class="settings-step-content">

<h3>Nova senha</h3>

<p>A nova senha deve ter no m&iacute;nimo 6 caracteres.</p>

<p>
<label for="pass">Nova senha</label><br />
<input type="password" name="pass" id="password" size="32" maxlength="48" value="" />
</p>

<p>
<label for="confpass">Nova senha (repetir)</label><br/>
<input type="password" name="confpass" id="password" size="32" maxlength="48" value="" />
</p>

	</div>
</div>

<div class="settings-buttons">
<input type="submit" value="Salvar Modifica&ccedil;&otilde;es" name="save" class="submit" />
</div>

</form>

</div></div></div></div>


</div>
</div>

<?php } elseif($tab == "5"){ ?>

<div id="friend-management" class="habblet-container">
                <div class="cbb clearfix settings">
                    <h2 class="title">AJUSTES DE AMIGOS</h2>
                    <div id="friend-management-container" class="box-content">
                        <div id="category-view" class="clearfix">
                            <div id="search-view">
                                Escreva o nome de seu amigo
                                  <div id="friend-search" class="friendlist-search">
					                <input type="text" maxlength="32" id="friend_query" class="friend-search-query" />
					                <a class="friendlist-search new-button search-icon" id="friend-search-button"><b><span></span></b><i></i></a>
					            </div>
                            </div>
                            <div id="category-list">
<div id="friends-category-title">
    Categorias de amigos
</div>

<div class="category-default category-item selected-category" id="category-item-0">Amigos</div>

    <input type="text" maxlength="32" id="category-name" class="create-category" /><div id="add-category-button" class="friendmanagement-small-icons add-category-item add-category"></div>
                            </div>
                        </div>
                        <div id="friend-list" class="clearfix">
<div id="friend-list-header-container" class="clearfix">
    <div id="friend-list-header">
        <div class="page-limit">
            <div class="big-icons friend-header-icon">Amigos
                <br />Mostrar
                30 |
                <a class="category-limit" id="pagelimit-50">50</a> |
                <a class="category-limit" id="pagelimit-100">100</a>
            </div>
        </div>
    </div>
	<div id="friend-list-paging">
			 1 |
    <?php
		$afriendscount = mysql_query("SELECT COUNT(*) FROM messenger_friendships  WHERE user_one_id = '".$my_id."'") or die(mysql_error());
		$friendscount = mysql_result($afriendscount, 0);

		$pages = ceil($friendscount / 30);

		$n = 1;

		while ($n < $pages) {
			$n++;
			echo "<a href=\"#\" class=\"friend-list-page\" id=\"page-".$n."\">".$n."</a> |";
		}

		echo "<a href=\"#\" class=\"friend-list-page\" id=\"page-2\">&gt;&gt;</a>"
	?>

        </div>
    </div>


<form id="friend-list-form">
    <table id="friend-list-table" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr class="friend-list-header">
                <td class="friend-select" />
                <td class="friend-name table-heading">Nome</td>
                <td class="friend-login table-heading">&Uacute;ltima vez online</td>
                <td class="friend-remove table-heading">Apagar</td>
            </tr>
           <?php
		   $i = 0;
		   $getem = mysql_query("SELECT * FROM messenger_friendships WHERE user_one_id = '".$my_id."' LIMIT 30") or die(mysql_error());

		   while ($row = mysql_fetch_assoc($getem)) {
		           $i++;

		           if(IsEven($i)){
		               $even = "odd";
		           } else {
		               $even = "even";
		           }

		           if($row['user_two_id'] == '".$my_id."'){
		           		$friendsql = mysql_query("SELECT * FROM users WHERE id = '".$row['user_one_id']."'");
		           } else {
		           		$friendsql = mysql_query("SELECT * FROM users WHERE id = '".$row['user_two_id']."'");
		           }

		           $friendrow = mysql_fetch_assoc($friendsql);



printf("   <tr class=\"%s\">
               <td><input type=\"checkbox\" name=\"friendList[]\" value=\"%s\" /></td>
               <td class=\"friend-name\">
                %s
               </td>
               <td class=\"friend-login\" title=\"%s\">%s</td>
               <td class=\"friend-remove\"><div id=\"remove-friend-button-%s\" class=\"friendmanagement-small-icons friendmanagement-remove remove-friend\"></div></td>
           </tr>", $even, $friendrow['id'], $friendrow['username'], date('d.m.Y  H:i:s', $friendrow['last_online']), date('d.m.Y  H:i:s', $friendrow['last_online']), $friendrow['id']);
		   }
		?>
        </tbody>
    </table>
    <a class="select-all" id="friends-select-all" href="#">Selecionar todos</a> |
    <a class="deselect-all" href="#" id="friends-deselect-all">Cancelar sele&ccedil;&atilde;o</a>
</form>                        </div>
    <script type="text/javascript">
        new FriendManagement({ currentCategoryId: 0, pageListLimit: 30, pageNumber: 1});
    </script>
                        <div id="category-options" class="clearfix">
<select id="category-list-select" name="category-list">
    <option value="0">Amigos</option>
    <option value="1">Teste de amigos</option>
</select>
<div class="friend-del"><a class="new-button red-button cancel-icon" href="#" id="delete-friends"><b><span></span>Apagar selecionados</b><i></i></a></div>
<div class="friend-move"><a class="new-button" href="#" id="move-friend-button"><b><span></span>Mover</b><i></i></a></div>                        </div>
                    </div>
                </div>
            </div>
        </div>

<script type="text/javascript">
  L10N.put("friendmanagement.tooltip.deletefriends", "¿Seguro que deseas eliminar a estos Amigos?\n<div class=\"friendmanagement-small-icons friendmanagement-save friendmanagement-tip-delete\"\>\n    <a class=\"friends-delete-button\" id=\"delete-friends-button\"\>Borrar</a\>\n</div\>\n<div class=\"friendmanagement-small-icons friendmanagement-remove friendmanagement-tip-cancel\"\>\n    <a id=\"cancel-delete-friends\"\>Cancelar</a\>\n</div\>\n\n");
  L10N.put("friendmanagement.tooltip.deletefriend", "¿Seguro que deseas eliminar a este/a Amig@?\n<div class=\"friendmanagement-small-icons friendmanagement-save friendmanagement-tip-delete\"\>\n    <a id=\"delete-friend-%friend_id%\"\>Borrar</a\>\n</div\>\n<div class=\"friendmanagement-small-icons friendmanagement-remove friendmanagement-tip-cancel\"\>\n    <a id=\"remove-friend-can-%friend_id%\"\>Cancelar</a\>\n</div\>");
  L10N.put("friendmanagement.tooltip.deletecategory", "¿Seguro que quieres borrar esta categoría?\n<div class=\"friendmanagement-small-icons friendmanagement-save friendmanagement-tip-delete\"\>\n    <a class=\"delete-category-button\" id=\"delete-category-%category_id%\"\>Borrar</a\>\n</div\>\n<div class=\"friendmanagement-small-icons friendmanagement-remove friendmanagement-tip-cancel\"\>\n    <a id=\"cancel-cat-delete-%category_id%\"\>Cancelar</a\>\n</div\>");
  new FriendManagement({ currentCategoryId: 0, pageListLimit: 30, pageNumber: 1});
</script>

    </div>
    </div>
<?php } else { header("location: ".$path.""); exit; } require_once('./templates/community_footer.php'); ?>