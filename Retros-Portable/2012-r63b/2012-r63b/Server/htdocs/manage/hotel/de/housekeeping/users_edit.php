<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Editar usuarios";
$pageid = "users_edit";

if(isset($_GET['username'])){
	$sql = mysql_fetch_assoc($sql = mysql_query("SELECT * FROM users WHERE username = '".FilterText($_GET['username'])."' LIMIT 1"));
	header("location: ".$adminpath."/index/p/users_edit&key=".$sql['id']."");
}

if(isset($key)){
	if(is_numeric($key)){
		$usercheck = mysql_query("SELECT * FROM users WHERE id = '".$key."' LIMIT 1");
		$usercheck_a = mysql_query("SELECT * FROM user_stats WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

		if(mysql_num_rows($usercheck) > 0){
			$usereditor = true;
			$userdata = mysql_fetch_assoc($usercheck);
			$userdata_a = mysql_fetch_assoc($usercheck_a);
		} else {
			$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar al usuario <img src=\"./images/del.gif\"></center></div>";
		}
	}
}

if($do == "save" && is_numeric($key)){

if($_POST['credits']){

	$query = mysql_query("SELECT * FROM users WHERE id = '".$key."' LIMIT 1");

	if(mysql_num_rows($query) > 0){

		$usereditor = true;
		$userdata = mysql_fetch_assoc($query);

		if($user_rank < 7){
			$working = $userdata['working'];
			$secretcode = $userdata['secretcode'];
			$vip_points = $userdata['vip_points'];
			$online = $userdata['online'];
			$look = $userdata['look'];
			$vip = $userdata['vip'];
		}else{
			$working = $_POST['working'];
			$secretcode = $_POST['secretcode'];
			$vip_points = $_POST['vip_points'];
			$online = $_POST['online'];
			$look = $_POST['look'];

			if($_POST['vip'] == "true"){ $vip = "1"; } else { $vip = "0"; }
		}

		mysql_query("UPDATE users SET real_name = '".$_POST['real_name']."', motto = '".$_POST['motto']."', look = '".$look."', gender = '".$_POST['gender']."', mail = '".$_POST['mail']."', credits = '".$_POST['credits']."', activity_points = '".$_POST['activity_points']."', birth = '".$_POST['birth']."', ip_last = '".$_POST['ip_last']."', ip_reg = '".$_POST['ip_reg']."', working = '".$working."', secretcode = '".$secretcode."', vip_points = '".$vip_points."', online = '".$online."', vip = '".$vip."' WHERE id = '".$userdata['id']."'");
		mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp,details) VALUES ('Housekeeping','User Bearbeitet','users_edit.php','".$my_id."','".$userdata['id']."','".$date_full."','
<b>Motto Vorher</b> ".$userdata['motto']." || <b>Motto Nachher</b> ".$_POST['motto']."<br>
<b>Email Vorher</b> ".$userdata['mail']." || <b>Motto Nachher</b> ".$_POST['mail']."<br>
<b>Taler Vorher</b> ".$userdata['credits']." || <b>Taler Nachher</b> ".$_POST['credits']."<br>
<b>Pixel Vorher</b> ".$userdata['activity_points']." || <b>Pixel Nachher</b> ".$_POST['activity_points']."<br>
<b>VIP-Points Vorher</b> ".$userdata['vip_points']." || <b>VIP-Points Nachher</b> ".$avip_points."<br>
<b>Aufgabe Vorher</b> ".$userdata['working']." || <b>Aufgabe Nachher</b> ".$working."<br>
<b>Security-Code Vorher</b> ".$userdata['secretcode']." || <b>Security-Code Nachher</b> ".$secretcode."')") or die(mysql_error());
		
		$msg = "<div class='rounded rounded-green'><center>".$userdata['username']." (ID: ".$userdata['id'].") editado correctamente. <img src=\"./images/check.gif\"></center></div>";
		$query = mysql_query("SELECT * FROM users WHERE id = '".$key."' LIMIT 1");
		$userdata = mysql_fetch_assoc($query);

	}else{
	$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar al usuario <img src=\"./images/del.gif\"></center></div>";
	}

}else{
	$msg = "<div class='rounded rounded-red'><center>Error - Debe rellenar todos los campos <img src=\"./images/del.gif\"></center></div>";
}

}

$check_bann = mysql_query("SELECT * FROM bans WHERE value = '".$userdata['username']."' AND bantype = 'user'");

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><br><?php }
if($usereditor == "true"){ ?>

<form action='<?php echo $adminpath; ?>/index/p/users_edit&key=<?php echo $userdata['id']; ?>&do=save' method='post' name='theAdminForm' id='theAdminForm'>
<input type='hidden' name='hiddenHook' value='valid'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editando usuario - <?php echo $userdata['username']; ?> (ID: <?php echo $userid; ?>)</b> <?php if($userdata['online'] > 0){ ?><img src="<?php echo $path; ?>/web-gallery/v2/images/online.gif"><?php }else{ ?><img src="<?php echo $path; ?>/web-gallery/v2/images/offline.gif"><?php } ?> // Baneado: <?php if(mysql_num_rows($check_bann) > 0){ echo'Si'; }else{ echo'No'; } ?></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre de usuario</b><div class='graytext'>Nombre del usuario a editar</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $userdata['username']; ?>" disabled="disabled" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Pa&iacute;s</b><div class='graytext'>Pa&iacute;s del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='real_name' value="<?php echo $userdata['real_name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tiempo online</b><div class='graytext'>Tiempo total en linea</div></td>
<td class='tablerow2'  width='60%'  valign='middle'>
<input type='text' name='onlinetimer' value="<?php echo round($userdata_a['OnlineTime'] / 3600) ; ?>" disabled='disabled' size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Mision</b><div class='graytext'>Misi&oacute;n del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='motto' value="<?php echo $userdata['motto']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>&Uacute;ltima vez conectado</b><div class='graytext'>&Uacute;ltima vez que se conect&oacute; el usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='last_online' value="<?php echo date('d.m.Y - H:i:s', $userdata['last_online']); ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Creado el</b><div class='graytext'>Fecha en la que se registro</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='account_created' value="<?php echo date('d.m.Y - H:i:s', $userdata['account_created']); ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Figure_Data</b><div class='graytext'>Look del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='look' value="<?php echo $userdata['look']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Genero</b><div class='graytext'>Masculino o femenino</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="gender"><option value="M">Masculino</option><option value="F" <?php if($userdata['gender'] == "F"){ echo'selected="selected"'; } ?>>Femenino</option></select></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>E-Mail</b><div class='graytext'>Correo electr&oacute;nico del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='mail' value="<?php echo $userdata['mail']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Cr&eacute;ditos</b><div class='graytext'>Cr&eacute;ditos del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='credits' value="<?php echo $userdata['credits']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Pixeles</b><div class='graytext'>P&iacute;xeles del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='activity_points' value="<?php echo $userdata['activity_points']; ?>" size='30' class='textinput'></td>
</tr>

<?php if($user_rank > 6){ // Entwickler ?>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Puntos VIP</b><div class='graytext'>Puntos VIP del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='vip_points' value="<?php echo $userdata['vip_points']; ?>" size='30' class='textinput'>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>VIP</b><div class='graytext'>Solo para > Usuarios con rango 7</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input name="vip" <?php if($userdata['vip'] == 1){ ?>checked="checked"<?php } ?> value="true" type="checkbox"></td>
</tr>

<?php } ?>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Cumplea&ntilde;os</b><div class='graytext'>Fecha de nacimiento del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='birth' value="<?php echo $userdata['birth']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>IP ~ &Uacute;ltima</b><div class='graytext'>&Uacute;ltima ip usada por el usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='ip_last' value="<?php echo $userdata['ip_last']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>IP ~ Registro</b><div class='graytext'>IP con la que se registro el usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='ip_reg' value="<?php echo $userdata['ip_reg']; ?>" size='30' class='textinput'></td>
</tr>

<?php if($user_rank > 6){  // Entwickler ?>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tarea</b><div class='graytext'>Responsabilidad que tiene el usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='working' value="<?php echo $userdata['working']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>C&oacute;digo secreto</b><div class='graytext'>C&oacute;digo secreto del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='secretcode' value="<?php echo $userdata['secretcode']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Estado</b><div class='graytext'>Si el usuario est&aacute; desconectado o no</div>
<td class='tablerow2'  width='60%'  valign='middle'><select name="online" class="dateselector"><option value="0">Desconectado</option><option value="1"<?php if($userdata['online'] == 1){ ?>selected="selected"<?php } ?>>Online</option></select>
</tr>

<?php } if($userdata['rank'] > 4){ ?>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Llamadas de ayuda</b><div class='graytext'>Llamadas de ayuda que ha resuelto</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='helper' value="<?php echo mysql_evaluate("SELECT COUNT(*) FROM moderation_tickets WHERE moderator_id = '".$userdata['id']."'"); ?>x" disabled="disabled" size='30'class='textinput'></td>
</tr>

<?php } ?>


<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Guardar' class='realbutton' accesskey='s'></td></tr>

</form>
</table>
</div>

<td width='10%' valign='top' id='rightblock'>

<center><img src="<?php echo $avatar; echo $userdata['look']; ?>&size=b&direction=2&action=wlk,wav,&head_direction=3&gesture=sml"></center>

<div class='tableborder'>
<div class='tableheaderalt'><center>PLacas (<?php echo mysql_evaluate("SELECT COUNT(*) FROM user_badges WHERE user_id = '".$key."'"); ?>) </div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

 <tr>
  <td class='tablesubheader' width='1%' align='center'>Codigo</td>
  <td class='tablesubheader' width='14%' align='center'>Placa</td>
 </tr>

<?php 

$sql = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$key."' ORDER BY badge_id"); 
while($row = mysql_fetch_assoc($sql)){

?>

 <tr>
  <td class='tablerow1' align='center'><?php echo $row['badge_id']; ?></td>
  <td class='tablerow2'><img src="<?php echo $cimagesurl.$badgesurl.$row['badge_id']; ?>.gif"></div></td>
</tr>

<?php } ?>

</form>
</table>
<div><div>

<?php } else { ?>

<form action='<?php echo $adminpath; ?>/index/p/users_edit' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editar</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre de usuario</b><div class='graytext'>Escribe el nombre de usuario del cual quieres editar.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='searchname' value="<?php echo $_POST['query']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Guardar cambios' class='realbutton' accesskey='s'></td>
</tr>

</form>
</table>
</div>

<?php } ?>

</div>
</td></tr>
</table>
</div>

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>




            <li><a href="index.php?_cmd=bots"><b>Crear BOT</b></li>