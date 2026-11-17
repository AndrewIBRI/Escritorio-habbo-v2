<?php

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(session_is_registered(acp)){ header("Location: index.php?loginThrowBack=true"); exit; }

$pagename = "Login";
$pageid = "login";

if(isset($_POST['username'])){

	$form_name = addslashes($_POST['username']);
	$form_pass = HoloHash($_POST['password']);
	$form_pass2 = HoloHashMD5($_POST['password']);
	$form_code = $_POST['codeword'];

	$check = mysql_query("SELECT * FROM users WHERE username = '" . $form_name . "' AND password = '" . $form_pass . "' AND secretcode = '".$form_code."' AND rank > 3 or username = '" . $form_name . "' AND password = '" . $form_pass2 . "' AND secretcode = '".$form_code."' AND rank > 3 LIMIT 1") or die(mysql_error());
	$valid = mysql_num_rows($check);

	if(!empty($form_name) && !empty($form_pass)){
		if($valid > 0){
			$row = mysql_fetch_assoc($check);

			$_SESSION['acp'] = true;
			$_SESSION['hkusername'] = $row['username'];
			$_SESSION['hkpassword'] = $form_pass2;
			$_SESSION['hkcode'] = $form_code;

			$my_id = $row['id'];

				if(!session_is_registered(username)){
					$_SESSION['username'] = $row['username'];
					$_SESSION['password'] = $form_pass2;
					$_SESSION['code'] = $form_code;
				}

			mysql_query("UPDATE users SET ip_last = '".$remote_ip."' WHERE id = '".$row['id']."' LIMIT 1");
			mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Login (IP: ".$remote_ip.")','login.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
			if($_POST['headerclient'] == true){
				header("location: $path/client"); exit;
			}else{
				header("location: ".$adminpath."/index/p/home"); exit;
			}
		
		} else {
			$msg = "Nome de usuário ou senha incorreto.";
			header("location: ".$adminpath."/index/p/login");
		}
	} else {
		$msg = "Você deve preencher todos os campos!";
	}

} elseif($notify_logout == true){
        mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Logout','notify_logout','".$my_id."','0','".$date_full."')") or die(mysql_error());
	$msg = "<font color='green'>Você foi desconectado corretamente.</font>";
} else {
	$msg = "Faça o login";
}

include('subheader.php');

?>
<style type="text/css">
body {
	background-image: url('./images/bg-login.png')

}
</style>

<div id='ipdwrapper'>
<div align='center'>
<br><img src="./images/logo.png">
<br>
<div class="header_right"><img src="./images/header_tm1.gif"></div>
<div style='width:500px'>
<div class='outerdiv' id='global-outerdiv'><!-- OUTERDIV -->
<table cellpadding='0' cellspacing='8' width='100%' id='tablewrap'>
<tr>
 <td id='rightblock'>
 <div>
 <form id='loginform' action='<?php echo $adminpath; ?>/index/p/login&do=submit' method='post'>
 <input type='hidden' name='qstring' value='' />
  <table width='100%' cellpadding='0' cellspacing='0' border='0'>
  <tr>
   <td width='200' class='tablerow1' valign='top' style='border:0px;width:200px'>
   <div style='text-align:center;padding-top:20px'>
   	<img src='./images/frank_waving_dbl_sml.gif' alt='Housekeeping' border='0' />
   </div>
   <br />
   <div class='desctext' style='font-size:10px'>
   <div align='center'><strong>Bem-vindo à administração do <?php echo $shortname; ?></strong></div>
   <br />
  	<div style='font-size:9px;color:gray'>A partir daí você pode gerenciar todo o hotel. Gestão está disponível 24 horas.<br /><br /><b>Sobre o "Habbo ID"</b>:<br>Sua HabboID pode encontrar em mim (Home) com créditos pixels, etc.</div>
   </div>
   </td>
   <td width='300' style='width:300px' valign='top'>
	 <table width='100%' cellpadding='5' cellspacing='0' border='0'>
	 <tr>
	  <td colspan='2' align='center'>
		 <br />
		 <div style='font-weight:bold;color:red'><?php echo $msg; ?></div><br />
	  </td>
	 </tr>
	<?php if($notify_login !== "login"){ ?>
	 <tr>		
	  <td align='right'><strong>Nombre de usuario</strong></td>	  
	  <td><input style='border:1px solid #AAA' type='text' size='20' name='username' id='namefield' value='' /></td>
	 </tr>
	 <tr>
	  <td align='right'><strong>Senha</strong></td>
	  <td><input style='border:1px solid #AAA' type='password' size='20' name='password' value='' /></td>
	 </tr>
	 <tr>		
	  	  
	
	 </tr>
	 <tr>		
	 </tr>
	 <tr>
	  <td colspan='2' align='center'><input type='submit' style='border:1px solid #AAA' value='Conectar' /></td>
	 </tr>
	<?php } ?>
	 <tr>
	  <td colspan='2'><br /><center><img src="./images/workman_habbo_down.gif"></center></td>
	 </tr>
	</table>
   </td>
  </tr>
  </table>
 </form>
 
 </div>
 </td>
</tr>
</table>
</div><!-- / OUTERDIV -->

</div>
</div>
<script type='text/javascript'>
  if (top.location != self.location) { top.location = self.location }
  
  try
  {
  	window.onload = function() { document.getElementById('namefield').focus(); }
  }
  catch(error)
  {
  	alert(error);
  }
  
</script>
