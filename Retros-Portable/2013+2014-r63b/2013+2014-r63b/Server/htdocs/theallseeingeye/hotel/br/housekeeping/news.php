<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Notícias";
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
		$msg = "<div class='rounded rounded-green'><center>A notícia foi removida com êxito!. <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Você não tem permissão para remover notícias!<img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_news WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$article = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Você não tem permissão para editar notícias.<img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['image'])){

	$check = mysql_query("SELECT id FROM cms_news WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

	$newsdata = mysql_fetch_assoc($check);

		if($_POST['comments'] == true){ $checked = '1'; }else{ $checked = '0'; }

		mysql_query("UPDATE cms_news SET title = '".$_POST['title']."', image = '".$_POST['image']."', shortstory = '".$_POST['shortstory']."', longstory = '".$_POST['longstory']."', author = '".$_POST['author']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>A notícia foi publicada corretamente!. <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {

		$msg = "<div class='rounded rounded-red'><center>Esta notícia não existe<img src=\"./images/check.gif\"></center></div>";

	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

	mysql_query("INSERT INTO cms_news (title,longstory,shortstory,published,image,campaign,campaignimg,author) VALUES ('".$_POST['title']."','".$_POST['shortstory']."','".$_POST['longstory']."','".time()."','".$_POST['image']."','0','0','".$_POST['author']."')") or die(mysql_error());
	$msg = "<div class='rounded rounded-green'><center>A notícia foi publicada!</center></div>";
	$editor_mode = false;
	}
}

@include('subheader.php');

if(isset($msg)){ ?>
<style type="text/css">
#theAdminForm .tableborder table tr .tablerow2 #story {
	 width: 250px;
	 overflow: auto;
}
</style>
<p><strong><?php echo $msg; ?></strong></p><?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($editor_mode !== true){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/news&do=save' method='post' name='theAdminForm' id='theAdminForm'>
 <div class='tableborder'>
 <div class='tableheaderalt'><center>Todas as notícias do Hotel

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Próxima página >>"> <span class='fauxbutton'><a href='./index/p/news&do=add'>Publicar Notícia</a></span>
</div>

</center>

 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='20%'>Título</td>
  <td class='tablesubheader' width='12%' align='left'>Data</td>
  <td class='tablesubheader' width='10%' align='left'>Autor</td>
  <td class='tablesubheader' width='1%' align='center'>Editar</td>
  <td class='tablesubheader' width='1%' align='center'>Remover</td>
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
  <td class='tablerow2' align='left'><?php echo date('d/m/Y, H:i', $row['published']); ?></td>
  <td class='tablerow2' align='left'><?php echo $row['author']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/news&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Editar'></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/news&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Eliminar'></a></td>															
</tr>

<?php } ?>
 
 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Todas as notícias do Hotel

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Próxima página »"> <span class='fauxbutton'><a href='./index/p/news&do=add'>Publicar Notícia</a></span>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php }elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/news&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Publicar Notícias</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Título</b><div class='graytext'>Título da Notícia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='title' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Resumo</b><div class='graytext'>Resumo da Notícia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='shortstory' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>História</b><div class='graytext'>Notícia<br /><font color="green"></font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea id="story" name="longstory" cols='60' rows='8' style="width: 15%"></textarea></td>
</tr>
<script type="text/javascript" src="tiny_mce/tiny_mce.js"></script>
<script type="text/javascript">
	tinyMCE.init({
		language : "en",
		// General options
		mode : "exact",
		elements : "story",
		theme : "advanced",
		plugins : "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave",

		// Theme options
		theme_advanced_buttons1 : "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
		theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
		theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
		theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft",
		theme_advanced_toolbar_location : "top",
		theme_advanced_toolbar_align : "left",
		theme_advanced_statusbar_location : "bottom",
		theme_advanced_resizing : true,

		// Example content CSS (should be your site CSS)
		content_css : "<?php echo HPATH; ?>tiny_mce/css/content.css",

		// Drop lists for link/image/media/template dialogs
		template_external_list_url : "<?php echo HPATH; ?>tiny_mce/lists/template_list.js",
		external_link_list_url : "<?php echo HPATH; ?>tiny_mce/lists/link_list.js",
		external_image_list_url : "<?php echo HPATH; ?>tiny_mce/lists/image_list.js",
		media_external_list_url : "<?php echo HPATH; ?>tiny_mce/lists/media_list.js",

		// Style formats
		style_formats : [
			{title : 'Bold text', inline : 'b'},
			{title : 'Red text', inline : 'span', styles : {color : '#ff0000'}},
			{title : 'Red header', block : 'h1', styles : {color : '#ff0000'}},
			{title : 'Example 1', inline : 'span', classes : 'example1'},
			{title : 'Example 2', inline : 'span', classes : 'example2'},
			{title : 'Table styles'},
			{title : 'Table row 1', selector : 'tr', classes : 'tablerow1'}
		],

		// Replace values for the template plugin
		template_replace_values : {
			username : "Some User",
			staffid : "991234"
		}
	});
</script>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Topstory</b><div class='graytext'>Imagem TopStory <a href="http://www.habbighotel.com.br/swfs/c_images/topstory_files/" target="_new">Clique aqui</a> para escolher</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Autor</b><div class='graytext'>Nome de quem escreveu a notícia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='author' value="" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' name='submit' value='Publicar Notícia' class='realbutton' accesskey='s'></td></tr>

</table>

</div>
</div>
</form>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)) { ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/news&do=save&key=<?php echo $article['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Notícia <u><?php echo $articlen['title']; ?></u> (ID: <?php echo $article['id']; ?>) <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Título</b><div class='graytext'>Título da Notícia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='title' value="<?php echo $article['title']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Resumo</b><div class='graytext'>Resumo da Notícia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='longstory' value="<?php echo $article['longstory']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>História</b><div class='graytext'>Notícia<br /><font color="green"></font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea id="story" name="shortstory" cols='60' rows='8' style="width: 8%"><?php echo $article['shortstory']; ?></textarea></td>
</tr>
<script type="text/javascript" src="./tiny_mce/tiny_mce.js"></script>
<script type="text/javascript">
	tinyMCE.init({
		language : "es",
		// General options
		mode : "exact",
		elements : "story",
		theme : "advanced",
		plugins : "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave",

		// Theme options
		theme_advanced_buttons1 : "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
		theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,code,|,forecolor,backcolor",
		theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,ltr,rtl,|,fullscreen",
		theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft,|,insertdate,inserttime,preview",
		theme_advanced_toolbar_location : "top",
		theme_advanced_toolbar_align : "left",
		theme_advanced_statusbar_location : "bottom",
		theme_advanced_resizing : true,
		width: "750px",

		// Example content CSS (should be your site CSS)
		content_css : "./tiny_mce/css/content.css",

		// Drop lists for link/image/media/template dialogs
		template_external_list_url : "./tiny_mce/lists/template_list.js",
		external_link_list_url : "./>tiny_mce/lists/link_list.js",
		external_image_list_url : "./tiny_mce/lists/image_list.js",
		media_external_list_url : "./tiny_mce/lists/media_list.js",

		// Style formats
		style_formats : [
			{title : 'Bold text', inline : 'b'},
			{title : 'Red text', inline : 'span', styles : {color : '#ff0000'}},
			{title : 'Red header', block : 'h1', styles : {color : '#ff0000'}},
			{title : 'Example 1', inline : 'span', classes : 'example1'},
			{title : 'Example 2', inline : 'span', classes : 'example2'},
			{title : 'Table styles'},
			{title : 'Table row 1', selector : 'tr', classes : 'tablerow1'}
		],

		// Replace values for the template plugin
		template_replace_values : {
			username : "Some User",
			staffid : "991234"
		}
	});
</script>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Topstory</b><div class='graytext'>TopStory <a href="http://www.habbighotel.com.br/swfs/c_images/topstory_files/" target="_new">Clique aqui</a> para ver</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image' value="<?php echo $article['image']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Autor</b><div class='graytext'>Nome de quem escreveu a Notícia</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='author' value="<?php echo $article['author']; ?>" size='30' class='textinput'></td>
</tr>


<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Atualizar Notícia' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>