<?php

if($user_rank > 8){

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
		$msg = "<div class='rounded rounded-green'><center>Grupo removido com sucesso!<img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Não foi possível remover este grupo, tente novamente<img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM group_details WHERE id = '".$key."'");

	if(mysql_num_rows($check) > 0){
	
		$group = mysql_fetch_assoc($check);
		$editor_mode = true;

		$owner = mysql_fetch_assoc($owner = mysql_query("SELECT username FROM users WHERE id = '".$group['ownerid']."'"));
	} else {
		$msg = "<div class='rounded rounded-red'><center>Não é possível editar este grupo. <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['name'])){

	$check = mysql_query("SELECT id FROM group_details WHERE id = '".$key."'");

	if(mysql_num_rows($check) > 0){

	$groupdata = mysql_fetch_assoc($check);

	$owner = mysql_fetch_assoc($owner = mysql_query("SELECT id FROM users WHERE username = '".mysql_real_escape_string($_POST['owner'])."'"));

		mysql_query("UPDATE group_details SET name = '".mysql_real_escape_string($_POST['name'])."', description = '".mysql_real_escape_string($_POST['description'])."', ownerid = '".$owner['id']."', roomid = '".mysql_real_escape_string($_POST['roomid'])."', created = '".mysql_real_escape_string($_POST['created'])."', badge = '".mysql_real_escape_string($_POST['badge'])."', type = '".mysql_real_escape_string($_POST['type'])."', views = '".mysql_real_escape_string($_POST['views'])."', forum = '".mysql_real_escape_string($_POST['forum'])."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Suas preferências foram salvas <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {
		$msg = "<div class='rounded rounded-red'><center>Este grupo não existe <img src=\"./images/check.gif\"></center></div>";
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
 <div class='tableheaderalt'><center>Informações dos Grupos

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>
<input type="submit" name="site" value="Próximo »">
</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%'>ID</td>
  <td class='tablesubheader' width='10%'>Grupo</td>
  <td class='tablesubheader' width='5%'>Proprietário</td>
  <td class='tablesubheader' width='12%'>Criado em</td>
  <td class='tablesubheader' width='1%'>Emblema</td>
  <td class='tablesubheader' width='1%'>Tipo</td>
  <td class='tablesubheader' width='1%'>Acessos</td>
  <td class='tablesubheader' width='1%'>Fórum</td>
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
 <div class='tableheaderalt'><center>Informações dos Grupos

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Próxima »">
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/groups&do=save&key=<?php echo $group['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editar grupo <u><?php echo $group['name']; ?> (ID: <?php echo $group['id']; ?>)</u> <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>


<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nome</b><div class='graytext'>O Nome do grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $group['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descrisão</b><div class='graytext'>A descrisão do grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea type='text' name='description' cols='60' rows='5'><?php echo $group['description']; ?></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Dono</b><div class='graytext'>Proprietário do Grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='owner' value="<?php echo $owner['username']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Quarto do Grupo</b><div class='graytext'>ID do quarto do grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='roomid' value="<?php echo $group['roomid']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Data</b><div class='graytext'>Data que o grupo foi criado</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='created' value="<?php echo $group['created']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Emblema</b><div class='graytext'>Emblema do Grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='badge' value="<?php echo $group['badge']; ?>" size='30' class='textinput'> &nbsp;&nbsp;<img src="<?php echo $path; ?>/habbo-imaging/badge.php?badge=<?php echo $group['badge']; ?>"></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo de grupo</b><div class='graytext'>Tipo de grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="type"><option value="0"<?php if($group['type'] == "0"){ ?> selected="selected"<?php } ?>>Limitado</option><option value="1"<?php if($group['type'] == "1"){ ?> selected="selected"<?php } ?>>Administrado</option><option value="2"<?php if($group['type'] == "2"){ ?> selected="selected"<?php } ?>>Aberto</option><option value="3"<?php if($group['type'] == "3"){ ?> selected="selected"<?php } ?>>Fechado</option></select></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Acessos</b><div class='graytext'>Acessos ao Grupo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='views' value="<?php echo $group['views']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo de Fórum</b><div class='graytext'>Acesso ao Fórum</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="forum"><option value="0"<?php if($group['forum'] == "0"){ ?> selected="selected"<?php } ?>>Jeder</option><option value="1"<?php if($group['forum'] == "1"){ ?> selected="selected"<?php } ?>>Administrado</option></select></td>
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