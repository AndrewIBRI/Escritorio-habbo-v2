<?php

if($user_rank > 5){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "News bearbeiten";
$pageid = "news";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_news");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/news&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/news&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM cms_news WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM cms_news WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','News gelöscht','news.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>News wurde erfolgreich gelöscht. <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No puedes eliminar noticias <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_news WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$article = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No puedes editar noticias <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['image'])){

	$check = mysql_query("SELECT id FROM cms_news WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

	$newsdata = mysql_fetch_assoc($check);

		if($_POST['comments'] == true){ $checked = '1'; }else{ $checked = '0'; }

		mysql_query("UPDATE cms_news SET title = '".$_POST['title']."', image = '".$_POST['image']."', shortstory = '".$_POST['shortstory']."', longstory = '".$_POST['longstory']."', author = '".$_POST['author']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','News wurde Bearbeitet','news.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>News wurde erfolgreich bearbeitet. <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {

		$msg = "<div class='rounded rounded-red'><center>Error - &iexcl;La noticia no existe!  <img src=\"./images/check.gif\"></center></div>";

	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

        mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','News wurde hinzugefügt','news_add.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
	mysql_query("INSERT INTO cms_news (title,longstory,shortstory,published,image,campaign,campaignimg,author) VALUES ('".$_POST['title']."','".$_POST['shortstory']."','".$_POST['longstory']."','".time()."','".$_POST['image']."','0','0','".$_POST['author']."')") or die(mysql_error());
	$msg = "<div class='rounded rounded-green'><center>Noticia a&nitlde;adida correctamente</center></div>";
	$editor_mode = false;
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($editor_mode !== true){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/news&do=save' method='post' name='theAdminForm' id='theAdminForm'>
 <div class='tableborder'>
 <div class='tableheaderalt'><center>Not&iacute;cias - Informac&atilde;o geral

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Mais »"> <span class='fauxbutton'><a href='./index/p/news&do=add'>Criar not&iacute;cia</a></span>
</div>

</center>

 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='20%'>Titulo</td>
  <td class='tablesubheader' width='12%' align='left'>Fecha</td>
  <td class='tablesubheader' width='10%' align='left'>Autor</td>
  <td class='tablesubheader' width='1%' align='center'>EDitar</td>
  <td class='tablesubheader' width='1%' align='center'>Borrar</td>
 </tr>
<?php

$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_articles = mysql_query("SELECT * FROM cms_news ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_articles)){

?>

<tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><strong><?php echo HoloText($row['title']); ?></strong><div class='desctext'><?php echo $row['longstory']; ?></div></td>
  <td class='tablerow2' align='left'><?php echo date('d.m.Y, H:i', $row['published']); ?></td>
  <td class='tablerow2' align='left'><?php echo $row['author']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/news&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Editar'></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/news&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Eliminar'></a></td>															
</tr>

<?php } ?>
 
 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Not&iacute;cias - Informac&atilde;o geral

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Mais »"> <span class='fauxbutton'><a href='./index/p/news&do=add'>Criar not&iacute;cia</a></span>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php }elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/news&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Criar Not&iacute;cia</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Titulo</b><div class='graytext'>T&iacute;tulo da not&iacute;cia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='title' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Texto curto</b><div class='graytext'>Breve resumo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='shortstory' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Conte&uacute;do</b><div class='graytext'>Conte&uacute;do da not&iacute;cia<br /><font color="green">HTML ativado.</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='longstory' cols='100' rows='5' wrap='soft' id='sub_desc' class='multitext'></textarea></td>
<!-- Gets replaced with TinyMCE, remember HTML in a textarea should be encoded -->
</form>
<script type="text/javascript">
if (document.location.protocol == 'file:') {
	alert("The examples might not work properly on the local file system due to security settings in your browser. Please use a real webserver.");
}
</script>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Topstory</b><div class='graytext'>Imagen da not&iacute;cia. Clique <a href="http://www.habbotimes.net/pixel/news" target="_new">aqui</a> para verlas.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Autor</b><div class='graytext'>Quem escreveu a not&iacute;cia?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='author' value="" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' name='submit' value='Publicar Not&iacute;cia' class='realbutton' accesskey='s'></td></tr>

</table>

</div>
</div>
</form>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)) { ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/news&do=save&key=<?php echo $article['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Noticia <u><?php echo $articlen['title']; ?></u> (ID: <?php echo $article['id']; ?>) <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Titulo</b><div class='graytext'>T&iacute;tulo de la noticia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='title' value="<?php echo $article['title']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Texto corto</b><div class='graytext'>Resumen breve</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='longstory' value="<?php echo $article['longstory']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Contenido</b><div class='graytext'>Contenido de la noticia<br /><font color="green">HTML activado.</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='shortstory' cols='60' rows='5' wrap='soft' id='sub_desc' class='multitext'><?php echo $article['shortstory']; ?></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Topstory</b><div class='graytext'>Imagen de la noticia. Clic <a href="http://www.habbotimes.net/fancenter/pixxelsearch/topstory_gallery" target="_new">aqui</a> para verlos.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="<?php echo $article['image']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Autor</b><div class='graytext'>&iquest;Qui&eacute;n escribe la noticia?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='author' value="<?php echo $article['author']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Comentarios</b><div class='graytext'>Clic en la casilla si deseas activarlo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input tabindex="6" type="checkbox" name="comments" id="comments" <?php if($article['comments'] == 1){ ?>checked="checked" <?php } ?>value="true"/></td>
</tr>


<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='News Bearbeiten' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>