<?php

if($user_rank > 5){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Recomendados por los staffs";
$pageid = "recommended";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_recommended");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/recommended&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/recommended&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM cms_recommended WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM cms_recommended WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Staffempfehlung gelöscht','recommended.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Recomendaci&oacute;n eliminada con &eacute;xito <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido eliminar la recomendaci&oacute;n <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_recommended WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$recommended = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido eliminar. <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['id_rec'])){

	$check = mysql_query("SELECT id FROM cms_recommended WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

		if($_POST['comments'] == true){ $checked = '1'; }else{ $checked = '0'; }

		mysql_query("UPDATE cms_recommended SET type = '".$_POST['type']."', id_rec = '".$_POST['id_rec']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Staffempfehlung wurde Bearbeitet','news_manage.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Recomendaci&oacute;n actualizada correctamente. <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {

		$msg = "<div class='rounded rounded-red'><center>Error - Esta recomendaci&oacute;n no existe  <img src=\"./images/check.gif\"></center></div>";

	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){
	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Staffempfehlung wurde hinzugefügt','recommended_add.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
	mysql_query("INSERT INTO cms_recommended (id_rec,type) VALUES ('".$_POST['id_rec']."','".$_POST['type']."')") or die(mysql_error());

	$msg = "<div class='rounded rounded-green'><center>Recomendaci&oacute;n staff a&ntilde;adida correctamente.</center></div>";
	$editor_mode = false;

	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($editor_mode !== true){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/recommended&do=save' method='post' name='theAdminForm' id='theAdminForm'>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Recomendados por los staffs

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/recommended&do=add'>Staffempfehlung Hinzufügen</a></span>
</div>

 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='20%'>Tipo</td>
  <td class='tablesubheader' width='20%' align='left'>ID de Recomendaci&oacute;n</td>
  <td class='tablesubheader' width='20%' align='center'>Editar</td>
  <td class='tablesubheader' width='1%' align='center'>Borrar</td>
 </tr>
<?php


$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_recommended = mysql_query("SELECT * FROM cms_recommended ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_recommended)){

?>

<tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><?php if($row['type'] == "room"){ echo'Sala'; }else{ echo'Grupo'; } ?></td>
  <td class='tablerow2' align='left'><?php echo $row['id_rec']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/recommended&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Editar'></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/recommended&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Eliminar'></a></td>															
</tr>

<?php } ?>
 
 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Recomendados por los staffs

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/recommended&do=add'>A&ntilde;adir recomendaci&oacute;n</a></span>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/recommended&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>A&ntilde;adir recomendaci&oacute;n</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo</b><div class='graytext'>Sala o grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>
									<option value='room'>Sala</option>
									<option value='group' selected='selected'>Grupo</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>ID</b><div class='graytext'>ID de la recomendaci&oacute;n</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type="text" name="id_rec" value=""></td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' name='submit' value='A&ntilde;adir' class='realbutton' accesskey='s'></td>
</tr>

</table>

</div>
</div>
</form>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)) { ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/recommended&do=save&key=<?php echo $recommended['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editando recomendaci&oacute;n (ID: <?php echo $recommended['id']; ?>) <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo</b><div class='graytext'>Sala o grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>
									<option value='room' <?php if($recommended['type'] == "room"){ echo" selected='selected'"; } ?>>Sala</option>
									<option value='group'<?php if($recommended['type'] == "group"){ echo" selected='selected'"; } ?>>Grupo</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>ID</b><div class='graytext'>ID de la recomendaci&oacute;n</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type="text" name="id_rec" value="<?php echo $recommended['id_rec']; ?>"></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Guardar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>