<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Passwort Manager";
$pageid = "passwordtool";

if(isset($_POST['password'])){

	$check = mysql_query("SELECT * FROM users WHERE username = '".$_POST['name']."' LIMIT 1") or die(mysql_error());
	if(mysql_num_rows($check) > 0){
		$drow = mysql_fetch_assoc($check);

		mysql_query("UPDATE users SET password  = '".HoloHash($_POST['password'], $_POST['name'])."' WHERE username = '".$_POST['name']."' LIMIT 1") or die(mysql_error()); 
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Passwort geändert','passwordtool.php','".$my_id."','".$drow['id']."','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>La contrase&ntilde;a de  ".$_POST['name']." se ha cambiado correctamente. <img src=\"./images/check.gif\"></center></div>";

	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha encontrado el usuario <img src=\"./images/del.gif\"></center></div>";
	}

}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/passwordtool' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editar contrase&ntilde;a</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nombre de usuario</strong><div class='graytext'>Usuario al que deseas cambiar la contrase&ntilde;a</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Contrase&ntilde;a</strong><div class='graytext'>Nueva contrase&ntilde;a para el usuario </div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='password' name='password' value="" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Cambiar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>