<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Mercadillo";
$pageid = "marktplatz";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_marktplatz");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/marktplatz&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/marktplatz&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM cms_marktplatz WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM cms_marktplatz WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Placa eliminda correctamente <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - Plca no econtrada <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_marktplatz WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$marktplatz = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - La placa no se ha podido encontrar <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_marktplatz WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

		mysql_query("UPDATE cms_marktplatz SET name = '".$_POST['name']."', image = '".$_POST['image']."', description = '".$_POST['description']."', credits = '".$_POST['credits']."', pixels = '".$_POST['pixels']."', vip_points = '".$_POST['vip_points']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

		$msg = "<div class='rounded rounded-green'><center>Placa actualizada correctamente. <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar la placa. <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

	mysql_query("INSERT INTO cms_marktplatz (image,name,description,credits,pixels,vip_points) VALUES ('".$_POST['image']."','".$_POST['name']."','".$_POST['description']."','".$_POST['credits']."','".$_POST['pixels']."','".$_POST['vip_points']."')") or die(mysql_error());
	$msg = "<div class='rounded rounded-green'><center>Placa introducida correctamente <img src=\"./images/check.gif\"></center></div>";
	$editor_mode = false;

	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($editor_mode !== true){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/marktplatz&do=save' method='post' name='theAdminForm' id='theAdminForm'>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Mercadillo - Informaci&oacute;n general

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/marktplatz&do=add'>A&ntilde;adir</a></span>
</div>

 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='10%' align='center'>Codigo de placa</td>
  <td class='tablesubheader' width='10%' align='center'>Nombre</td>
  <td class='tablesubheader' width='10%' align='center'>Descripc&oacute;n</td>
  <td class='tablesubheader' width='10%' align='center'>Creditos</td>
  <td class='tablesubheader' width='10%' align='center'>Pixeles</td>
  <td class='tablesubheader' width='10%' align='center'>Puntos VIP</td>
  <td class='tablesubheader' width='10%' align='center'>Editar</td>
  <td class='tablesubheader' width='12%' align='center'>Borrar</td>
 </tr>
<?php


$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_marktplatz = mysql_query("SELECT * FROM cms_marktplatz ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_marktplatz)){

?>

<tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2' align='center'><img src="<?php echo $path; ?>/r63/c_images/album1584/<?php echo $row['image']; ?>.gif" alt="<?php echo $row['image']; ?>"></td>
  <td class='tablerow2' align='center'><?php echo $row['name']; ?></td>
  <td class='tablerow2' align='center'><?php echo $row['description']; ?></td>
  <td class='tablerow2' align='center'><?php echo $row['credits']; ?></td>
  <td class='tablerow2' align='center'><?php echo $row['pixels']; ?></td>
  <td class='tablerow2' align='center'><?php echo $row['vip_points']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/marktplatz&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Edit'></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/marktplatz&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Delete'></a></td>
</tr>
<?php } ?>

 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Mercadillo - Resumen

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/marktplatz&do=add'>A&ntilde;adir al merecadillo</a></span>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/marktplatz&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Crear placa</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>C&oacute;digo de la placa</b><div class='graytext'>Introduce el c&oacute;digo de la placa</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="<?php echo $marktplatz['image']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre</b><div class='graytext'>Nombre que tendr&aacute; la placa</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $marktplatz['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descripci&oacute;n</b><div class='graytext'>Descripci&oacute;n de la placa</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='description' value="<?php echo $marktplatz['description']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Cr&eacute;ditos</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='credits' value="<?php echo $marktplatz['credits']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Pixeles</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='pixels' value="<?php echo $marktplatz['pixels']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Puntos VIP</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='vip_points' value="<?php echo $marktplatz['vip_points']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' name='submit' value='Badge Hinzufügen' class='realbutton' accesskey='s'></td>
</tr>

</table>

</div>
</div>
</form>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)) { ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/marktplatz&do=save&key=<?php echo $marktplatz['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Placa: <b>(<?php echo $marktplatz['image']; ?>, ID: <?php echo $marktplatz['id']; ?>)</b></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>C&oacute;digo de la placa</b><div class='graytext'>Introduce el c&oacute;digo de la placa</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="<?php echo $marktplatz['image']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Cr&eacute;ditos</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='credits' value="<?php echo $marktplatz['credits']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Pixeles</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='pixels' value="<?php echo $marktplatz['pixels']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Puntos VIP</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='vip_points' value="<?php echo $marktplatz['vip_points']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre</b><div class='graytext'>Nombre que tendr&aacute; la placa</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $marktplatz['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descripci&oacute;n</b><div class='graytext'>Descripci&oacute;n de la placa</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='description' value="<?php echo $marktplatz['description']; ?>" size='30' class='textinput'></td>
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