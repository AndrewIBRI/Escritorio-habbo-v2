<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Da gerência";
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
		$msg = "<div class='rounded rounded-green'><center>Removido com sucesso <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Não foi possível realizar a ação desejada, tente novamente<img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_recommended WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$recommended = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Não foi possível realizar a ação desejada, tente novamente <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['id_rec'])){

	$check = mysql_query("SELECT id FROM cms_recommended WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

		if($_POST['comments'] == true){ $checked = '1'; }else{ $checked = '0'; }

		mysql_query("UPDATE cms_recommended SET type = '".mysql_real_escape_string($_POST['type'])."', id_rec = '".mysql_real_escape_string($_POST['id_rec'])."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>As preferências foram salvos<img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {

		$msg = "<div class='rounded rounded-red'><center>Isto não existe<img src=\"./images/check.gif\"></center></div>";

	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){
	mysql_query("INSERT INTO cms_recommended (id_rec,type) VALUES ('".mysql_real_escape_string($_POST['id_rec'])."','".mysql_real_escape_string($_POST['type'])."')") or die(mysql_error());

	$msg = "<div class='rounded rounded-green'><center>A atualização foi feita</center></div>";
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
 <div class='tableheaderalt'><center>Quartos/Grupos recomendados

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Próximo »"> <span class='fauxbutton'><a href='./index/p/recommended&do=add'>Adicionar</a></span>
</div>

 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='20%'>Tipo</td>
  <td class='tablesubheader' width='20%' align='left'>ID da ação</td>
  <td class='tablesubheader' width='20%' align='center'>Editar</td>
  <td class='tablesubheader' width='1%' align='center'>Remover</td>
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
 <div class='tableheaderalt'><center>Quartos/Grupos recomendados

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Próximo »"> <span class='fauxbutton'><a href='./index/p/recommended&do=add'>Adicionar recomendação</a></span>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/recommended&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Quartos/Grupos recomendados</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Quartos recomendados</b><div class='graytext'></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>
									<option value='room'>Quarto</option>
									<option value='group' selected='selected'>Grupo</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>ID</b><div class='graytext'>ID da sala/Grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type="text" name="id_rec" value=""></td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' name='submit' value='Adicionar' class='realbutton' accesskey='s'></td>
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
<div class='tableheaderalt'><center>Editar (ID: <?php echo $recommended['id']; ?>) <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo</b><div class='graytext'>Quarto do Grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>
									<option value='room' <?php if($recommended['type'] == "room"){ echo" selected='selected'"; } ?>>Sala</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>ID</b><div class='graytext'>ID</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type="text" name="id_rec" value="<?php echo $recommended['id_rec']; ?>"></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Salvar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>