<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Grupos";
$pageid = "groups";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM group_details");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/groups&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/groups&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM group_details WHERE id = '".$key."'");

	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM group_details WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("DELETE FROM group_members WHERE id_group = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Gruppe gelöscht','groups.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Grupo eliminado con &eacute;xito <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido eliminar el grupo <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM group_details WHERE id = '".$key."'");

	if(mysql_num_rows($check) > 0){
	
		$group = mysql_fetch_assoc($check);
		$editor_mode = true;

		$owner = mysql_fetch_assoc($owner = mysql_query("SELECT username FROM users WHERE id = '".$group['ownerid']."'"));
	} else {
		$msg = "<div class='rounded rounded-red'><center>Erro -r no se puede editar el grupo <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['name'])){

	$check = mysql_query("SELECT id FROM group_details WHERE id = '".$key."'");

	if(mysql_num_rows($check) > 0){

	$groupdata = mysql_fetch_assoc($check);

	$owner = mysql_fetch_assoc($owner = mysql_query("SELECT id FROM users WHERE username = '".$_POST['owner']."'"));

		mysql_query("UPDATE group_details SET name = '".$_POST['name']."', description = '".$_POST['description']."', ownerid = '".$owner['id']."', roomid = '".$_POST['roomid']."', created = '".$_POST['created']."', badge = '".$_POST['badge']."', type = '".$_POST['type']."', views = '".$_POST['views']."', forum = '".$_POST['forum']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Gruppe wurde Bearbeitet','groups_manage.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Grupo editado correctamente. <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - Este grupo no existe  <img src=\"./images/check.gif\"></center></div>";
	}

}
}

@include('subheader.php');

if($editor_mode !== true){
if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($editor_mode !== true){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
 
<form action='<?php echo $adminpath; ?>/index/p/groups&do=save' method='post' name='theAdminForm' id='theAdminForm'>
 <div class='tableborder'>
 <div class='tableheaderalt'><center>Grupos - Informaci&oacute;n general

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>
<input type="submit" name="site" value="Weiter »">
</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%'>ID</td>
  <td class='tablesubheader' width='10%'>Grupo</td>
  <td class='tablesubheader' width='5%'>Due&ntilde;o</td>
  <td class='tablesubheader' width='12%'>Creado el</td>
  <td class='tablesubheader' width='1%'>Placa</td>
  <td class='tablesubheader' width='1%'>Tipo</td>
  <td class='tablesubheader' width='1%'>Clicks</td>
  <td class='tablesubheader' width='1%'>Foro</td>
  <td class='tablesubheader' width='1%'>Editar</td>
 </tr>
<?php

$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_groups = mysql_query("SELECT * FROM group_details ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_groups)){

$members = mysql_evaluate("SELECT COUNT(*) FROM group_members WHERE id_group = '".$row['id']."'");

$owner = mysql_fetch_assoc($owner = mysql_query("SELECT username FROM users WHERE id = '".$row['ownerid']."'"));

?>

<tr>
  <td class='tablerow1'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><strong><?php echo HoloText($row['name']); ?></strong></td>
  <td class='tablerow2'><?php echo $owner['username']; ?></td>
  <td class='tablerow2'><?php echo $row['created']; ?></td>
  <td class='tablerow2'><img src='<?php echo $path; ?>/habbo-imaging/badge.php?badge=<?php echo $row['badge']; ?>.gif'></td>
  <td class='tablerow2'><?php echo $row['type']; ?></td>
  <td class='tablerow2'><?php echo $row['views']; ?></td>
  <td class='tablerow2'><?php echo $row['forum']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/groups&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif'></a> <a href='<?php echo $adminpath; ?>/index/p/groups&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif'></a></td>															
</tr>

<?php } ?>
 
 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Gruppen - Informci&ocute;n general

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Weiter »">
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/groups&do=save&key=<?php echo $group['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editando grupo <u><?php echo $group['name']; ?> (ID: <?php echo $group['id']; ?>)</u> <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>


<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre</b><div class='graytext'>El nombre del grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $group['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descripci&oacute;n</b><div class='graytext'>La descripci&oacute;n del grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea type='text' name='description' cols='60' rows='5'><?php echo $group['description']; ?></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Due&ntilde;o</b><div class='graytext'>Due&ntilde;o del grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='owner' value="<?php echo $owner['username']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>ID de la sala</b><div class='graytext'>ID de la sala del grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='roomid' value="<?php echo $group['roomid']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Fecha</b><div class='graytext'>Fecha en la que se creo el grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='created' value="<?php echo $group['created']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Placa</b><div class='graytext'>Badgecode del grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='badge' value="<?php echo $group['badge']; ?>" size='30' class='textinput'> &nbsp;&nbsp;<img src="<?php echo $path; ?>/habbo-imaging/badge.php?badge=<?php echo $group['badge']; ?>"></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo de grupo</b><div class='graytext'>Acceso que se tiene al grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="type"><option value="0"<?php if($group['type'] == "0"){ ?> selected="selected"<?php } ?>>Limit 500</option><option value="1"<?php if($group['type'] == "1"){ ?> selected="selected"<?php } ?>>Administratoren</option><option value="2"<?php if($group['type'] == "2"){ ?> selected="selected"<?php } ?>>Keinen</option><option value="3"<?php if($group['type'] == "3"){ ?> selected="selected"<?php } ?>>Jeder</option></select></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Clicks</b><div class='graytext'>Visitas al grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='views' value="<?php echo $group['views']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo de foro</b><div class='graytext'>Acceso al foro</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="forum"><option value="0"<?php if($group['forum'] == "0"){ ?> selected="selected"<?php } ?>>Jeder</option><option value="1"<?php if($group['forum'] == "1"){ ?> selected="selected"<?php } ?>>Administratoren</option></select></td>
</tr>


<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Speichern' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>