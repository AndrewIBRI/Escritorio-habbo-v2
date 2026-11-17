<?php

if($user_rank > 5){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Noticiones";
$pageid = "campaigns";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_hotcampaigns");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/campaigns&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/campaigns&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM cms_hotcampaigns WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM cms_hotcampaigns WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Campaign gelöscht','campaigns.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Notici&oacute;n eliminado correctamente <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido eliminar <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_hotcampaigns WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$campaign = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>>Error - No se puede eliminar el notici&oacute;n <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['image_url'])){

	$check = mysql_query("SELECT id FROM cms_hotcampaigns WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

	$campaigndata = mysql_fetch_assoc($check);

		mysql_query("UPDATE cms_hotcampaigns SET image_url = '".$_POST['image_url']."', caption = '".$_POST['caption']."', descr = '".$_POST['descr']."', url = '".$_POST['url']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Campaign wurde Bearbeitet','campaigns.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Campa&ntilde;a a&ntilde;adida correctamente <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {

		$msg = "<div class='rounded rounded-red'><center>Error - Este notici&oacute;n no existe <img src=\"./images/check.gif\"></center></div>";

	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Campaign wurde hinzugefügt','campaigns_add.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
	mysql_query("INSERT INTO cms_hotcampaigns (image_url,caption,descr,url) VALUES ('".$_POST['image_url']."','".$_POST['caption']."','".$_POST['descr']."','".$_POST['url']."')") or die(mysql_error());

	$msg = "<div class='rounded rounded-green'><center>Notici&oacute;n a&ntilde;adido correctamente</center></div>";

	$editor_mode = false;

	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($editor_mode !== true){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/campaigns&do=save' method='post' name='theAdminForm' id='theAdminForm'>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Notici&oacute;n - Informaci&oacute;n general

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/campaigns&do=add'>A&ntilde;adir notici&oacute;n</a></span>
</div>


 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='10%'>Titulo</td>
  <td class='tablesubheader' width='20%' align='left'>Descripci&oacute;n</td>
  <td class='tablesubheader' width='1%' align='left'>Imagen</td>
  <td class='tablesubheader' width='10%' align='left'>URL</td>
  <td class='tablesubheader' width='1%' align='center'>Editar</td>
 </tr>

<?php


$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_hotcampaigns = mysql_query("SELECT * FROM cms_hotcampaigns ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_hotcampaigns)){

?>

<tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><?php echo $row['caption']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['descr']; ?></td>
  <td class='tablerow2' align='left'><img src="<?php echo $row['image_url']; ?>"></td>
  <td class='tablerow2' align='left'><a href="<?php echo $row['url']; ?>" target="_new"><?php echo $row['url']; ?></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/campaigns&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Edit'></a> <a href='<?php echo $adminpath; ?>/index/p/campaigns&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Delete'></a></td>															
</tr>

<?php } ?>
 
 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Campaign - Informaci&oacute;n general

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="M&aacute;s »"> <span class='fauxbutton'><a href='./index/p/campaigns&do=add'>A&ntilde;adir notici&oacute;n</a></span>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/campaigns&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Crear notici&oacute;n</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Titulo</b><div class='graytext'>T&iacute;tulo del notici&oacute;n</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='caption' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descripci&oacute;n</b><div class='graytext'>Descripci&oacute;n, resumen<br /><font color="green">HTML activado.</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='descr' cols='60' rows='5' wrap='soft' id='sub_desc' class='multitext'></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Imagen</b><div class='graytext'>Puedes verlos <a href="http://www.habborator.org/archive/hotcampaigns.php" target="_new">aqui</a></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image_url' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>URL</b><div class='graytext'>Normalmente se pone la url de una noticia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='url' value="" size='30' class='textinput'></td>
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

<form action='<?php echo $adminpath; ?>/index/p/campaigns&do=save&key=<?php echo $campaign['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Notici&oacute;n <u><?php echo $campaign['caption']; ?></u> (ID: <?php echo $campaign['id']; ?>) <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Titulo</b><div class='graytext'>T&iacute;tulo del notici&oacute;n</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='caption' value="<?php echo $campaign['caption']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descripci&oacute;n</b><div class='graytext'>Descripci&oacute;n, resumen<br /><font color="green">HTML activado.</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='descr' cols='60' rows='5' wrap='soft' id='sub_desc' class='multitext'><?php echo $campaign['descr']; ?></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Imagen</b><div class='graytext'>Puedes verlos <a href="http://www.habborator.org/archive/hotcampaigns.php" target="_new">aqui</a></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image_url' value="<?php echo $campaign['image_url']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>URL</b><div class='graytext'>Normalmente se pone la url de una noticia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='url' value="<?php echo $campaign['url']; ?>" size='30' class='textinput'></td>
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