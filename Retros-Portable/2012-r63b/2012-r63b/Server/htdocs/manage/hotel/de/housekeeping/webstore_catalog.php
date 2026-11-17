<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$page = FilterText($_GET['page']);

$pagename = "Catalogo de la tienda";
$pageid = "webstore_catalog";

$posts = mysql_evaluate("SELECT COUNT(*) FROM homes_categories");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/webstore_catalog&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/webstore_catalog&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM homes_categories WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM homes_categories WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Kategorie gelöscht','webstore_catalog.php','".$my_id."','0','".$date_full."')");
		$msg = "<div class='rounded rounded-green'><center>Categoria eliminada con exito <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar la categor&iacute;a <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM homes_categories WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		$categorie = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar la categoria <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM homes_categories WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

        if(mysql_num_rows($check) > 0){

		mysql_query("UPDATE homes_categories SET name = '".$_POST['name']."', minrank = '".$_POST['minrank']."', type = '".$_POST['type']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Webstore Kategorie wurde Bearbeitet','webstore_catalog.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Categoria actualizada correctamente <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;
        } else {
                $msg = "<div class='rounded rounded-red'><center>Error - No se ha podido actualizar la categoria <img src=\"./images/del.gif\"></center></div>";
        }

} elseif($do == "add" && isset($_POST['name'])){

                mysql_query("INSERT homes_categories (name,minrank,type) VALUES ('".$_POST['name']."', '".$_POST['minrank']."', '".$_POST['type']."')") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Webstore Kategorie wurde Hinzugefügt','webstore_catalog.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Nueva categoria a&ntilde;adida <img src=\"./images/check.gif\"></b></center></div>";

}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><br><?php } ?>

<form action='<?php echo $adminpath; ?>/index/p/webstore_catalog' method='post' name='theAdminForm' id='theAdminForm'>

<div class='tableborder'>
<div class='tableheaderalt'><center><?php echo mysql_evaluate("SELECT COUNT(*) FROM homes_categories"); ?> Categoriass

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/webstore_catalog&do=add'>A&ntilde;adir categoria</a></span>
</div>
</form>

<?php if($do == "edit" && is_numeric($key)){ ?>

<table cellpadding='4' cellspacing='0' width='100%'>
<form action='<?php echo $adminpath; ?>/index/p/webstore_catalog&do=save&key=<?php echo $categorie['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre</b><div class='graytext'>Nombre de la categoria</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $categorie['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Rango minimo</b><div class='graytext'>Rango minimo para poder verla</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='minrank' value="<?php echo $categorie['minrank']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo</b><div class='graytext'>Tipo de categoria</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>

									<option value='6' <?php if($categorie['type'] == "6"){ echo "selected='selected'"; } ?>>Notas</option>
									<option value='2' <?php if($categorie['type'] == "2"){ echo "selected='selected'"; } ?>>Fondos</option>
									<option value='1' <?php if($categorie['type'] == "1"){ echo "selected='selected'"; } ?>>Stickers</option>
								    </select>
</td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Editar categoria' class='realbutton' accesskey='s'></td>
</tr>

</form>
</table>

<?php } elseif($do == "add"){ ?>

<table cellpadding='4' cellspacing='0' width='100%'>
<form action='<?php echo $adminpath; ?>/index/p/webstore_catalog&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre</b><div class='graytext'>Nombre de la categor&iacute;a</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $_POST['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Rango m&iacute;nimo</b><div class='graytext'>1 Para que todos puedan verlo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='minrank' value="<?php echo $_POST['minrank']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo</b><div class='graytext'>Tipo de categoria</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>
									<option value='6' <?php if($type == "6"){ echo "selected='selected'"; } ?>>Notas</option>
									<option value='2' <?php if($type == "2"){ echo "selected='selected'"; } ?>>Fondos</option>
									<option value='1' <?php if($type == "1"){ echo "selected='selected'"; } ?>>Stickers</option>
						     </select>
</td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Agregar' class='realbutton' accesskey='s'></td>
</tr>

</form>
</table>

<?php } else { ?>

<table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='15%' align='left'>ID</td>
  <td class='tablesubheader' width='25%' align='left'>Nombre</td>
  <td class='tablesubheader' width='10%' align='left'>Rango m&iacute;nimo</td>
  <td class='tablesubheader' width='10%' align='left'>Tipo</td>
  <td class='tablesubheader' width='1%'>Editar</td>
</tr>


<?php

$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_cc = mysql_query("SELECT * FROM homes_categories ORDER BY name LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_cc)){

if($row['type'] == 1){
	$typ = "Stickers";
} elseif($row['type'] == 2){
	$typ = "Fondos";
} elseif($row['type'] == 6){
	$typ = "Notas";
} else{
	$typ = "<b><font color=\"red\">UNKNOWN</font></b>";
}

?>

 <tr>
  <td class='tablerow1' align='left'><?php echo $row['id']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['name']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['minrank']; ?></td>
  <td class='tablerow2' align='left'><?php echo $typ; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/webstore_catalog&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif'></a>
				<a href='<?php echo $adminpath; ?>/index/p/webstore_catalog&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif'></a></td>
 </tr>

<?php } ?>

 </table>

<form action='<?php echo $adminpath; ?>/index/p/webstore_catalog' method='post' name='theAdminForm' id='theAdminForm'>

<div class='tableborder'>
<div class='tableheaderalt'><center><?php echo mysql_evaluate("SELECT COUNT(*) FROM homes_categories"); ?> categor&iacute;as

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/webstore_catalog&do=add'>A&ntilde;adir categor&iacute;a</a></span>
</div>
</form>

<?php } ?>

</div>



<!-- / RIGHT CONTENT BLOCK -->
</td>
</tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>