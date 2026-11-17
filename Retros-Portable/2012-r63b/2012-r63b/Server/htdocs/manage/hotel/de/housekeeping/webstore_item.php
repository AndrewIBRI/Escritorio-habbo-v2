<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Items";
$pageid = "webstore_item";

$posts = mysql_evaluate("SELECT COUNT(*) FROM homes_catalouge");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/webstore_item&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/webstore_item&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM homes_catalouge WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM homes_catalouge WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Item gelöscht','webstore_item.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Item eliminado con &eacute;xito.<img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar el item <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM homes_catalouge WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		$item = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar el item. <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM homes_catalouge WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	$exists = mysql_num_rows($check);

        if($exists > 0){
		$id = $key;
		$name = $_POST['name'];
		$type = $_POST['type'];
		$subtype = $_POST['subtype'];
		$data = $_POST['data'];
		$price = $_POST['price'];
		$amount = $_POST['amount'];
		$category = $_POST['category'];

		mysql_query("UPDATE homes_catalouge SET name = '".$name."', type = '".$type."', data = '".$data."', price = '".$price."', amount = '".$amount."', category = '".$category."' WHERE id = '".$id."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Homes Item wurde Bearbeitet','webstore_item.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Articulo actualizado correctamente. <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;
        } else {
                $msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar el item <img src=\"./images/del.gif\"></center></div>";
        }

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

	mysql_query("INSERT homes_catalouge (name,type,subtype,data,price,amount,category) VALUES ('".$name."', '".$type."', '0', '".$data."', '".$price."', '".$amount."', '".$category."')") or die(mysql_error());
	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Homes Item wurde Hinzugefügt','webstore_item.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
	$msg = "<div class='rounded rounded-green'><center>Item a&ntilde;adido correctamente. <img src=\"./images/check.gif\"></center></div>";
	$editor_mode = false;

	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><br><?php } ?>

<form action='<?php echo $adminpath; ?>/index/p/webstore_item' method='post' name='theAdminForm' id='theAdminForm'>

<div class='tableborder'>
<div class='tableheaderalt'><center><?php echo mysql_evaluate("SELECT COUNT(*) FROM homes_catalouge"); ?> Items

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/webstore_item&do=add'>A&ntilde;adir item</a></span>
</div>
</form>

<?php if($do == "edit" && is_numeric($key)){ ?>
<table cellpadding='4' cellspacing='0' width='100%'>
<form action='index.php?p=webstore_item&do=save&key=<?php echo $item['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre</b><div class='graytext'>Nombre del item</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $item['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo</b><div class='graytext'>Tipo del item</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>

									<option value='4' <?php if($type == "4"){ echo "selected='selected'"; } ?>>Fondo</option>
									<option value='3' <?php if($type == "3"){ echo "selected='selected'"; } ?>>Nota</option>
									<option value='2' <?php if($type == "2"){ echo "selected='selected'"; } ?>>UNKNOWN</option>
									<option value='1' <?php if($type == "1"){ echo "selected='selected'"; } ?>>Sticker</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Subtipo</b><div class='graytext'><font color="red">Por defecto: 0</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='subtype' value="<?php echo $item['subtype']; ?>" size='30' disabled='disabled' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Datos</b><div class='graytext'>Datos del item</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='data' value="<?php echo $item['data']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Precio</b><div class='graytext'>Precio que tendr&aacute;</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='price' value="<?php echo $item['price']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Cantidad</b><div class='graytext'>&iquest;Cuantos quieres a&ntilde;adir?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='amount' value="<?php echo $item['amount']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Categoria</b><div class='graytext'>Categoria en la que aparecera</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='category' value="<?php echo $item['category']; ?>" size='30' class='textinput'></td>
</tr>
<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Item Bearbeiten' class='realbutton' accesskey='s'></td></tr></table>
<?php } elseif($do == "add"){ ?>

<table cellpadding='4' cellspacing='0' width='100%'>
<form action='index.php?p=webstore_item&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre</b><div class='graytext'>Nombre del item</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $item['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo</b><div class='graytext'>Tipo del item</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type'  class='dropdown'>

									<option value='4' <?php if($type == "4"){ echo "selected='selected'"; } ?>>Fondo</option>
									<option value='3' <?php if($type == "3"){ echo "selected='selected'"; } ?>>Nota</option>
									<option value='2' <?php if($type == "2"){ echo "selected='selected'"; } ?>>UNKNOWN</option>
									<option value='1' <?php if($type == "1"){ echo "selected='selected'"; } ?>>Sticker</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Subtipo</b><div class='graytext'><font color="red">Por defecto: 0</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='subtype' value="<?php echo $_POST['subtype']; ?>" size='30' disabled='disabled' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Datos</b><div class='graytext'>Datos del item</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='data' value="<?php echo $_POST['data']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Precio</b><div class='graytext'>Precio que tendr&aacute;</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='price' value="<?php echo $_POST['price']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Cantidad</b><div class='graytext'>&iquest;Cuantos quieres a&ntilde;adir?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='amount' value="<?php echo $_POST['amount']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Categoria</b><div class='graytext'>&iquest;En  que categor&iacute;a aparecer&aacute;?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='category'  class='dropdown'><?php $get_category = mysql_query("SELECT * FROM homes_categories ORDER BY name"); while($category = mysql_fetch_assoc($get_category)){ ?>
<option value='<?php echo $category['id']; ?>' <?php if($_POST['category'] == "".$category['id'].""){ echo "selected='selected'"; } ?>><?php echo $category['name']; ?></option>
<?php } ?>
</select>
</td>
</tr>
<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Item Hinzufügen' class='realbutton' accesskey='s'></td></tr></table>
<?php } else { ?>
<table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='15%' align='left'>ID</td>
  <td class='tablesubheader' width='25%' align='left'>Nombre</td>
  <td class='tablesubheader' width='10%' align='left'>Tipo</td>
  <td class='tablesubheader' width='5%' align='left'>Subtipo</td>
  <td class='tablesubheader' width='15%'>Datos</td>
  <td class='tablesubheader' width='5%'>Precio</td>
  <td class='tablesubheader' width='5%' align='left'>Cantidad</td>
  <td class='tablesubheader' width='5%' align='left'>ID de la categoria</td>
  <td class='tablesubheader' width='5%'>Editar</td>
 </tr>

<?php

$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_catalouge = mysql_query("SELECT * FROM homes_catalouge ORDER BY name LIMIT ".$query_min.", 50") or die(mysql_error());

while($row = mysql_fetch_assoc($get_catalouge)){
if($row['type'] == 1){
$type = "Sticker";
} elseif($row['type'] == 4){
$type = "Hintergrund";
} elseif($row['type'] == 3){
$type = "Notiz";
} else {
$type = "Unknown";
}

?>

 <tr>
  <td class='tablerow1' align='left'><?php echo $row['id']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['name']; ?></td>
  <td class='tablerow2' align='left'><?php echo $type; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['subtype']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['data']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['price']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['amount']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['category']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/webstore_item&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif'></a>
				       <a href='<?php echo $adminpath; ?>/index/p/webstore_item&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif'></a>
  </td>	
 </tr>

<?php } ?>

 </table>

<form action='<?php echo $adminpath; ?>/index/p/webstore_item' method='post' name='theAdminForm' id='theAdminForm'>

<div class='tableborder'>
<div class='tableheaderalt'><center><?php echo mysql_evaluate("SELECT COUNT(*) FROM homes_catalouge"); ?> Items

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »"> <span class='fauxbutton'><a href='./index/p/webstore_item&do=add'>A&ntilde;adir item</a></span>
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