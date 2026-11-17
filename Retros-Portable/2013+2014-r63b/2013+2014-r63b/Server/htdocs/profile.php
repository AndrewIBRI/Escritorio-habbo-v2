<?php
require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-core.php.php');

$pagename = "Meus Detalhes";
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
	$motto = $_POST['motto'];
	$chr = chr(1);
	$chr2 = chr(2);
	$motto2 = str_replace($chr, "", $motto);
	$motto3 = str_replace($chr2, "", $motto);

		if(strlen($motto3) > 24){
			$result = "Sua missão é muito larga!";
			$error = "1";
		} else {
			if($_POST['block_newfriends'] == "true"){ $block_newfriends = '0'; }else{ $block_newfriends = '1'; }

			mysql_query("UPDATE users SET motto = '".mysql_real_escape_string($motto3)."', visibility = '".mysql_real_escape_string($_POST['visibility'])."', block_newfriends = '".mysql_real_escape_string($block_newfriends)."', mymusik = '".mysql_real_escape_string($_POST['mymusik'])."' WHERE id = '".$my_id."' LIMIT 1") or die(mysql_error());
			$result = "Seu perfil foi atualizado corretamente!";
		}
	}
} else if($tab == "3"){
	if(isset($_POST['save'])){
	$pass1 = FilterText($_POST['password']);
	$pass1_hash = HoloHashMD5($pass1, $myrow['username']);
	$mail1 = FilterText($_POST['email']);
	$themail = $mail1;
		//checks password --encryption--
		if($pass1_hash == $myrow['password'] ){
		$email_check = preg_match("/^[a-z0-9_\.-]+@([a-z0-9]+([\-]+[a-z0-9]+)*\.)+[a-z]{2,7}$/i", $mail1);
			if($email_check == "1"){
			mysql_query("UPDATE users SET mail = '".$mail1."' WHERE username = '".$rawname."' and password = '".$rawpass."'") or die(mysql_error());

			$result = "A atualização do e-mail \"".$mail1."\" foi realizada corretamente!";
			} else {
			$result = "A atualização de e-mail está incorreta!";
			$error = "1";
			}
		} else {
		$result = "Seus dados estão incorretos!";
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
	$newpass = FilterText($_POST['pass']);
	//Hashes and salts the new password with the user id (in lowercase) --encryption--
	$newpass_hash = HoloHashMD5($newpass, $rawname);
	$newpass_conf = FilterText($_POST['confpass']);
		if($pass1_hash == $myrow['password'] ){
			if($newpass == $newpass_conf){
				if(strlen($newpass) < 6){
				$result = "A senha fornecida é muito curta, digite uma de pelo menos 6 carácteres!";
				$error = "1";
				} else {
					if(strlen($newpass) > 51){
					$result = "A senha é muito longa, forneça uma senha com menos de 50 carácteres!";
					$error = "1";
					} else {
					//Atualização sobre encriptação de senha*
					mysql_query("UPDATE users SET password = '".$newpass_hash."' WHERE username = '".$rawname."' and password = '".$rawpass."'") or die(mysql_error());
					$result = "A senha foi alterada. Por favor faça login novamente.";
					}
				}
			} else {
			$result = "As senhas não coincidem.";
			$error = "1";
			}
		} else {
		$result = "Preencha todos os campos requeridos!";
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

<h2 class="title">Preferências</h2>
<div class="box-content">
            <div id="settingsNavigation">
            <ul>
		<?php
		if($tab == "2"){
                echo "<li class='selected'>Meus detalhes
                </li>";
		} else {
                echo "<li><a href='".$path."/profile/2'>Meus detalhes
                </li>";
		}

		if($tab == "3"){
                echo "<li class='selected'>Meu Email
                </li>";
		} else {
                echo "<li><a href='".$path."/profile/3'>Meu Email</a>
                </li>";
		}

		if($tab == "4"){
                echo "<li class='selected'>Minha senha
                </li>";
		} else {
                echo "<li><a href='".$path."/profile/4'>Minha senha</a>
                </li>";
		}
		
		?>
            </ul>
            </div>
</div></div>

</div>

<?php if($tab == "2"){ ?>
    <div class="habblet-container " style="float:left; width: 560px;">
        <div class="cbb clearfix settings">

            <h2 class="title">Mudar Perfil</h2>
            <div class="box-content">



<form action="profile/2" method="post">
<input type="hidden" name="tab" value="2" />
<input type="hidden" name="__app_key" value="MyHobba" />

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

<h3>Missão</h3>

<p>
Sua missão poderá ser vista por todos os usuários!
</p>

<p>
<span class="label">Missão</span>
<input type="text" name="motto" size="32" maxlength="32" value="<?php echo HoloText($user_row['motto']); ?>" id="avatarmotto" />
</p>

<h3>Sua Página</h3>

<p>
Quem verá sua <?php echo $shortname; ?> Home:<br>
<label><input name="visibility" value="EVERYONE" <?php if($user_row['visibility'] == "EVERYONE"){ ?>checked="checked" <?php } ?>type="radio">Todos</label>
<label><input name="visibility" value="FRIENDS" <?php if($user_row['visibility'] == "FRIENDS"){ ?>checked="checked" <?php } ?>type="radio">Meus amigos</label>
<label><input name="visibility" value="NOBODY" <?php if($user_row['visibility'] == "NOBODY"){ ?>checked="checked" <?php } ?>type="radio">Ninguém</label>
</p>

<h3>Filtro de Palavrões</h3>
<p>
<label><input type="checkbox" name="wordFilterDisabled" <?php if($wordFilterDisabled == "true"){ echo "checked='checked'"; } ?> value="true"/>
Ativar filtro de palavrões</label>
</p>

<h3>Pedidos de amizade</h3>
<p>

<label>
<input name="block_newfriends" <?php if($user_row['block_newfriends'] == 0){ ?>checked="checked"<?php } ?> value="true" type="checkbox">
Pedidos de amizade ativado
</label></p>

<h3>Online Status</h3>
<p>

<label>
<input name="hide_online" <?php if($user_row['hide_online'] == 0){ ?>checked="checked"<?php } ?> value="true" type="checkbox">
Todos podem me ver online
</label></p>

<h3>Preferências "Siga-me"</h3>
<p>

<label>
<input name="hide_inroom" <?php if($user_row['hide_inroom'] == 0){ ?>checked="checked"<?php } ?> value="true" type="checkbox">
Todos podem me seguir
</label></p>

<div class="settings-buttons" align="right">
<input type="submit" value="Salvar" name="save" class="submit" />
</div>

</form>

</div></div>
    </div>
<?php } else if($tab == "3"){ ?>
    <div class="habblet-container " style="float:left; width: 560px;">
        <div class="cbb clearfix settings">

            <h2 class="title">Alterar seu endereço de Email</h2>
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
ATENÇÃO! Nunca mude o email se alguém pedir, prometendo mobis, raros ou moedas! Para sua segurança use um email que exista!
<div class="settings-step">

 <label for="currentpassword">Senha atual</label><br />
 <input type="password" size="32" maxlength="32" name="password" id="currentpassword" class="currentpassword " />
</p>

<p>
<label for="mail">Endereço de email atual:</label><br />
<input type="text" name="mail" id="mail" READONLY size="32" maxlength="48" value="<?php echo $myrow['mail']; ?>" />
</p>

<div>


</div></div>

<div class="settings-step">

<label for="email">Novo endereço de email</label><br />
<input type="text" name="email" id="email" size="32" maxlength="48" value="<?php echo $themail; ?>" />
</p>

	</div>
</div>

<div class="settings-buttons">
<input type="submit" value="Mudar email" name="save" class="submit" />
</div>

</form>

</div></div></div></div>


</div>
</div>

<?php } else if($tab == "4"){ ?>

<div class="habblet-container " style="float:left; width: 560px;">
        <div class="cbb clearfix settings">

            <h2 class="title">Mudar senha</h2>
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

ATENÇÃO! Nunca mude a senha se alguém pedir, prometendo mobis, raros ou moedas! Para sua segurança use um email que exista!

<form action="profile/4" method="post" id="passwordform">
<input type="hidden" name="tab" value="4" />
<input type="hidden" name="__app_key" value="HoloCMS" />

<div class="settings-step">

<p>
 <label for="currentpassword">Senha atual</label><br />
 <input type="password" size="32" maxlength="32" name="password" id="currentpassword" class="currentpassword " />
</p>

<div>


	</div>
</div>
<div class="settings-step">

<p>
<label for="pass">Nova senha</label><br />
<input type="password" name="pass" id="password" size="32" maxlength="48" value="" />
</p>

<p>
<label for="confpass">Re-digite a nova senha</label><br/>
<input type="password" name="confpass" id="password" size="32" maxlength="48" value="" />
</p>

	</div>
</div>

<div class="settings-buttons">
<input type="submit" value="Mudar senha" name="save" class="submit" />
</div>

</form>

</div></div></div></div>

<?php } else if($tab == "5"){ ?>

<div class="habblet-container " style="float:left; width: 560px;">
        <div class="cbb clearfix settings">

            <h2 class="title">Convide o seus amigos!</h2>
            <div class="box-content">

Você sabia que divulgando a sua URL para seus amigos você pode ganhar diversos prêmios no Hotel? O <b><?php echo $shortname; ?></b> está dando a oportunidade de conseguir raros e emblemas mais 
fáceis dentro do Hotel, oque acha?!
<p>Vamos lá... Copie a URL abaixo e envie para seus amigos</p>

<br><input style="height: 15px; width: 350px;" type="text" name="username" maxlength="5" value="<?php echo $path; ?>/refer/<?php echo $name; ?>" disabled="disabled"/></br>
<br>Convide o seus amigos do Facebook!</br>
<br><a href="https://www.facebook.com/sharer/sharer.php?u=<?php echo $path; ?>/refer/<?php echo $name; ?>"><img title="Convide teus amigos!" src="http://images.maquinadevendas.com.br/site/es/button/btn_compartilhar_facebook.jpg"></a></br>

<br>Você possui atualmente <b><?php if(!isset($_GET['n'])) {
				$usuario = $name;
			} else {
				$usuario = mysql_real_escape_string($_GET['n']);
			}
							$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='". $usuario ."' ORDER BY ID") or die(mysql_error()); 

								$data = mysql_fetch_assoc($query); echo $data['aantalleden'];?></b> referidos</br>

								<p>__________________________________________________________________________</p>
								<br>» Lembre-se que nós temos todos os dados do usuário e do amigo que você chamou, caso tenha algo idêntico como endereço de IP você poderá ser desclassificado.</br>

</div></div></div></div>


</div>
</div>


    </div>
    </div>
<?php } else { header("location: ".$path.""); exit; } require_once('./templates/community_footer.php'); ?>