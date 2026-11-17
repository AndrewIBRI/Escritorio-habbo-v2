<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "New Slides";
$pageid = "campaigns";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_news_slider");
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

	$check = mysql_query("SELECT id FROM cms_news_slider WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM cms_news_slider WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Campaign gelöscht','campaigns.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Notícias removido com sucesso <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Erro - não é capaz de apagar <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM cms_news_slider WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$campaign = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>>Erro - não é possível excluir new <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['image_url'])){

	$check = mysql_query("SELECT id FROM cms_news_slider WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

	$campaigndata = mysql_fetch_assoc($check);

		mysql_query("UPDATE cms_news_slider SET image_url = '".$_POST['image_url']."', title = '".$_POST['title']."', shortstory = '".$_POST['shortstory']."', link = '".$_POST['link']."', enter_button = '".$_POST['enter_button']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','New foi editada','campaigns.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>New adicionado com sucesso <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {

		$msg = "<div class='rounded rounded-red'><center>Error - Essa New Não Existe <img src=\"./images/check.gif\"></center></div>";

	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','New foi adicionada','campaigns_add.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
	mysql_query("INSERT INTO cms_news_slider (image_url,id,title,shortstory,link,enter_button) VALUES ('".$_POST['image_url']."','".$_POST['id']."','".$_POST['title']."','".$_POST['shortstory']."','".$_POST['link']."','".$_POST['enter_button']."')") or die(mysql_error());

	$msg = "<div class='rounded rounded-green'><center>Notícias adicionada corretamente</center></div>";

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
 <div class='tableheaderalt'><center>New - Informação Geral

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Mais »"> <span class='fauxbutton'><a href='./index/p/campaigns&do=add'>Adicionar New</a></span>
</div>


 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='10%'>Titulo</td>
  <td class='tablesubheader' width='20%' align='left'>Descrição</td>
  <td class='tablesubheader' width='1%' align='left'>Imagem</td>
  <td class='tablesubheader' width='10%' align='left'>Link</td>
  <td class='tablesubheader' width='1%' align='left'>Nome do Botão</td>
  <td class='tablesubheader' width='1%' align='center'>Editar</td>
 </tr>

<?php


$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_hotcampaigns = mysql_query("SELECT * FROM cms_news_slider ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_hotcampaigns)){

?>

<tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><?php echo $row['title']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['shortstory']; ?></td>
  <td class='tablerow2' align='left'><img src="<?php echo $row['image_url']; ?>"></td>
  <td class='tablerow2' align='left'><a href="<?php echo $row['link']; ?>" target="_new"><?php echo $row['link']; ?></a></td>
  <td class='tablerow2' align='left'><a href="<?php echo $row['enter_button']; ?>" target="_new"><?php echo $row['enter_button']; ?></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/campaigns&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Edita'></a> <a href='<?php echo $adminpath; ?>/index/p/campaigns&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Delete'></a></td>															
</tr>

<?php } ?>
 
 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>New - Informação Geral

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Mais »"> <span class='fauxbutton'><a href='./index/p/campaigns&do=add'>Adicionar New</a></span>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/campaigns&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Criar New</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Titulo</b><div class='graytext'>Titulo Da New</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='title' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descrição</b><div class='graytext'>Resumo de descrição<br /><font color="green">HTML activado.</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='shortstory' cols='60' rows='5' wrap='soft' id='sub_desc' class='multitext'></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Imagem</b><div class='graytext'>Você pode obtelas <a href="http://brteamgroup.hospedagemdesite.com/swf_habbig/c_images/NewSlide/" target="_new">aqui</a></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image_url' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Link</b><div class='graytext'>Link para onde será redirecionado ao clicar no botão. Ex: www.habbighotel.com.br/client</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='link' value="" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nome do Botão</b><div class='graytext'>Nome do botão na New. Ex: Saiba mais</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='enter_button' value="" size='30' class='textinput'></td>
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

<form action='<?php echo $adminpath; ?>/index/p/campaigns&do=save&key=<?php echo $campaign['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Notícias <u><?php echo $campaign['title']; ?></u> (ID: <?php echo $campaign['id']; ?>) <img src="./images/edit.gif"></center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Titulo</b><div class='graytext'>Titulo Da New</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='title' value="<?php echo $campaign['title']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descrição</b><div class='graytext'>Resumo de descrição<br /><font color="green">HTML activado.</font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='shortstory' cols='60' rows='5' wrap='soft' id='sub_desc' class='multitext'><?php echo $campaign['shortstory']; ?></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Imagem</b><div class='graytext'>Você pode obtelas <a href="http://brteamgroup.hospedagemdesite.com/swf_habbig/c_images/NewSlide/" target="_new">aqui</a></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='image_url' value="<?php echo $campaign['image_url']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Link</b><div class='graytext'>Link para onde será redirecionado ao clicar no botão. Ex: www.habbighotel.com.br/client</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='link' value="<?php echo $campaign['link']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nome do Botão</b><div class='graytext'>Nome do botão na New. Ex: Saiba mais</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='enter_button' value="<?php echo $campaign['enter_button']; ?>" size='30' class='textinput'></td>
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