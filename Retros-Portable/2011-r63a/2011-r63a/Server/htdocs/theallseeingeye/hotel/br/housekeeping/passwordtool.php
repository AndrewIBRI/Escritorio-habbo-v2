<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Configurações de senha";
$pageid = "passwordtool";

if(isset($_POST['password'])){

	$check = mysql_query("SELECT * FROM users WHERE username = '".mysql_real_escape_string($_POST['name'])."' LIMIT 1") or die(mysql_error());
	if(mysql_num_rows($check) > 0){
		$drow = mysql_fetch_assoc($check);

		mysql_query("UPDATE users SET password  = '".HoloHashMD5(mysql_real_escape_string($_POST['password']), mysql_real_escape_string($_POST['name']))."' WHERE username = '".mysql_real_escape_string($_POST['name'])."' LIMIT 1") or die(mysql_error()); 
		$msg = "<div class='rounded rounded-green'><center>La contrase&ntilde;a de  ".mysql_real_escape_string($_POST['name'])." se ha cambiado correctamente. <img src=\"./images/check.gif\"></center></div>";

	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha encontrado el usuario <img src=\"./images/del.gif\"></center></div>";
	}

}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/passwordtool' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editar senha</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nome do usuário</strong><div class='graytext'>Nome do usuário que irá alterar a senha</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Senha</strong><div class='graytext'>Nova senha do usuário</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='password' name='password' value="" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Salvar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>