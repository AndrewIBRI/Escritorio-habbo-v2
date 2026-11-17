<?php

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(session_is_registered(acp)){ header("Location: index.php?loginThrowBack=true"); exit; }

$pagename = "Login";
$pageid = "login";

if(isset($_POST['username'])){

	$form_name =  mysql_real_escape_string (addslashes($_POST['username']));
	$form_pass =  mysql_real_escape_string(HoloHash($_POST['password']));
	$form_pass2 =  mysql_real_escape_string(HoloHashMD5($_POST['password']));
	$form_code = mysql_real_escape_string($_POST['codeword']); 

	$check = mysql_query("SELECT * FROM users WHERE username = '" . $form_name . "' AND password = '" . $form_pass . "' AND secretcode = '".$form_code."' AND rank > 4 or username = '" . $form_name . "' AND password = '" . $form_pass2 . "' AND secretcode = '".$form_code."' AND rank > 4 LIMIT 1") or die(mysql_error());
	$valid = mysql_num_rows($check);

	if(!empty($form_name) && !empty($form_pass)){
		if($valid > 0){
			$row = mysql_fetch_assoc($check);

			$_SESSION['acp'] = true;
			$_SESSION['hkusername'] = $row['username'];
			$_SESSION['hkpassword'] = $form_pass2;
			$_SESSION['secretcode'] = $form_code;

			$my_id = $row['id'];

				if(!session_is_registered(username)){
					$_SESSION['username'] = $row['username'];
					$_SESSION['password'] = $form_pass2;
					$_SESSION['code'] = $form_code;
				}

			mysql_query("UPDATE users SET ip_last = '".$remote_ip."' WHERE id = '".$row['id']."' LIMIT 1");
			//mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Login (IP: ".$remote_ip.")','login.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
			if($_POST['headerclient'] == true){
				header("location: $path/client"); exit;
			}else{
				header("location: ".$adminpath."/index/p/home"); exit;
			}
		
		} else {
			$msg = "Username, Passwort, Habbo ID oder Security Code Falsch";
			header("location: ".$adminpath."/index/p/login");
		}
	} else {
		$msg = "Você deve preencher todos os campos!";
	}

} elseif($notify_logout == true){
	$msg = "<font color='green'>Você saiu corretamente.</font>";
} else {
	$msg = "Painel de Controle Habbo";
}

include('subheader.php');

?>


<div id='ipdwrapper'>
<div align='center'>
<h2>Painel de Controle</h2>
<br>
<div style='width:500px'>
<div class='outerdiv' id='global-outerdiv'><!-- OUTERDIV -->
<table cellpadding='0' cellspacing='8' width='100%' id='tablewrap'>
<tr>
 <td id='rightblock'>
 <div>
 <form id='loginform' action='<?php echo $adminpath; ?>/index/p/login&do=submit' method='post'>

	<?php if($notify_login !== "login"){ ?>
<table class="striped"><span>Nome de usuário</span>	  
	  <td><input style='border:2px solid #0' type='text' size='25' name='username' id='namefield' value='' /></td>
	 </table>
	 
	 
	<table class="striped"><span>Senha</span>	  
	  <td> <input style='border:2px solid #0' type='password' size='25' name='password' value='' /></td>
	 </table>
	 Nunca dê o seus dados para ninguém, sempre que entrar em sites idêntico, coloque senhas diferentes.
	  
	  <td colspan='8' align='center'><input type='submit' style='border:1px solid #0' value='Entrar' /></td>
	<?php } ?>