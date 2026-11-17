<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Loja de emblemas";
$pageid = "marktplatzdo";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_marktplatzvip");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/marktplatzdo&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/marktplatzdo&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM cms_marktplatzvip WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM cms_marktplatzvip WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Emblema Apagado Com Sucesso. <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - Emblema Não Encontrado <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_marktplatzvip WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$marktplatz = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error -Não Foi Possivel Encontrar O Emblema <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_marktplatzvip WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

		mysql_query("UPDATE cms_marktplatzvip SET name = '".$_POST['name']."', image = '".$_POST['image']."', dolares = '".$_POST['dolares']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

		$msg = "<div class='rounded rounded-green'><center>Emblema Atualizado Com Sucesso. <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - A placa não pôde ser encontrada. <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

	mysql_query("INSERT INTO cms_marktplatzvip (image,name,dolares) VALUES ('".$_POST['image']."','".$_POST['name']."','".$_POST['dolares']."')") or die(mysql_error());
	$msg = "<div class='rounded rounded-green'><center>Emblema adicionado com sucesso <img src=\"./images/check.gif\"></center></div>";
	$editor_mode = false;

	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($editor_mode !== true){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/marktplatzdo&do=save' method='post' name='theAdminForm' id='theAdminForm'>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Infomação Geral

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Más »"> <span class='fauxbutton'><a href='./index/p/marktplatzdo&do=add'>A&ntilde;adir</a></span>
</div>

 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='10%' align='center'>Codigo do emblema</td>
  <td class='tablesubheader' width='10%' align='center'>Nome</td>
  <td class='tablesubheader' width='10%' align='center'>Dólares</td>
  <td class='tablesubheader' width='10%' align='center'>Editar</td>
  <td class='tablesubheader' width='12%' align='center'>Excluir</td>
 </tr>
<?php


$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_marktplatz = mysql_query("SELECT * FROM cms_marktplatzvip ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_marktplatz)){

?>

<tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2' align='center'><img src="<?php echo $cimagesurl.$badgesurl.$row['image']; ?>.gif" alt="<?php echo $row['image']; ?>"></td>
  <td class='tablerow2' align='center'><?php echo $row['name']; ?></td>
  <td class='tablerow2' align='center'><?php echo $row['dolares']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/marktplatzdo&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Edit'></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/marktplatzdo&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Delete'></a></td>
</tr>
<?php } ?>

 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Resumen

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Miss »"> <span class='fauxbutton'><a href='./index/p/marktplatzdo&do=add'>Adicionar ao mercado</a></span>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/marktplatzdo&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Criar O Emblema</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Codigo Do Emblema</b><div class='graytext'>Digite o código do Emblema</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="<?php echo $marktplatz['image']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nome</b><div class='graytext'>O Nome Do Emblema Será</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $marktplatz['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Dólares</b><div class='graytext'>O Preço Que Tem O Emblema Não Pode Passar De 3. Dólares</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='dolares' value="<?php echo $marktplatz['dolares']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' name='submit' value='Adicionar Emblema' class='realbutton' accesskey='s'></td>
</tr>

</table>

</div>
</div>
</form>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)) { ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/marktplatzdo&do=save&key=<?php echo $marktplatz['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Placa: <b>(<?php echo $marktplatz['image']; ?>, ID: <?php echo $marktplatz['id']; ?>)</b></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Codigo Do Emblema</b><div class='graytext'>Digite o código do Emblema</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="<?php echo $marktplatz['image']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nome</b><div class='graytext'>O Nome Do Emblema Será</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $marktplatz['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Dólares</b><div class='graytext'>O Preço Que Tem O Emblema Não Pode Passar De 3. Dólares</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='dolares' value="<?php echo $marktplatz['dolares']; ?>" size='30' class='textinput'></td>
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