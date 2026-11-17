<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Bots ~ Respostas";
$pageid = "bots_responses";

$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM bots_responses");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/bots_responses&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/bots_responses&page=".$_POST['page2']."");
}

if($do == "delete" && is_numeric($key)){

	$check = mysql_query("SELECT id FROM bots_responses WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		mysql_query("DELETE FROM bots_responses WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','News gelöscht','bots_responses.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>As respostas estão no excluída correta <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - Eu não posso apagar notícias <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "edit" && is_numeric($key)){

	$check = mysql_query("SELECT * FROM bots_responses WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
	
	if(mysql_num_rows($check) > 0){
		$article = mysql_fetch_assoc($check);
		$editor_mode = true;
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - Você não pode editar Respostas <img src=\"./images/del.gif\"></center></div>";
	}

} elseif($do == "save" && is_numeric($key) && isset($_POST['response_text'])){

	$check = mysql_query("SELECT id FROM bots_responses WHERE id = '".$key."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){

	$bots_responses = mysql_fetch_assoc($check);


		mysql_query("UPDATE bots_responses SET bot_id = '".$_POST['bot_id']."', keywords = '".$_POST['keywords']."', response_text = '".$_POST['response_text']."', mode = '".$_POST['mode']."' WHERE id = '".$key."' LIMIT 1") or die(mysql_error());
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Atualize Respostas','news.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Respostas atualizadas corretamente <img src=\"./images/check.gif\"></center></div>";
		$editor_mode = false;

	} else {

		$msg = "<div class='rounded rounded-red'><center>Error -  As respostas não existem!  <img src=\"./images/check.gif\"></center></div>";

	}

} elseif($do == "add"){

	$editor_mode = true;

	if($_POST['submit']){

        mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Añadir Contestaciones','news_add.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
	mysql_query("INSERT INTO bots_responses (bot_id,keywords,response_text,mode,serve_id) VALUES ('".$_POST['bot_id']."','".$_POST['keywords']."','".$_POST['response_text']."','".$_POST['mode']."','0')") or die(mysql_error());
	$msg = "<div class='rounded rounded-green'><center>Adicionado respostas corretas</center></div>";
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

<form action='<?php echo $adminpath; ?>/index/p/bots_responses&do=save' method='post' name='theAdminForm' id='theAdminForm'>
 <div class='tableborder'>
 <div class='tableheaderalt'><center>BOTS Respostas - Visão geral

   <select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »"> 
<span class='fauxbutton'><a href='./index/p/bots_responses&do=add'>Adicionar respostas</a></span>
</div>

</center>

 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
   <td class='tablesubheader' width='7%' align='center'>ID</td>
  <td class='tablesubheader' width='8%'>Bot ID</td>
  <td class='tablesubheader' width='57%'>Texto</td>
  <td class='tablesubheader' width='10%' align='center'>Editar</td>
  <td class='tablesubheader' width='10%' align='center'>Apagar</td>
 </tr>
<?php

$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_articles = mysql_query("SELECT * FROM bots_responses ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_articles)){
?>

<tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><strong><?php echo ($row['bot_id']); ?></strong></td>
  <td class='tablerow2'><?php echo $row['response_text']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/bots_responses&do=edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Editar'></a></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/bots_responses&do=delete&key=<?php echo $row['id']; ?>'><img src='./images/del.gif' alt='Eliminar'></a></td>															
</tr>

<?php } ?>
 
 </table>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>
   BOTS Respostas - Visão geral

   <select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Weiter »"> 
<span class='fauxbutton'><a href='./index/p/bots_responses&amp;do=add'>Adicionar respostas</a></span>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php }elseif($do == "add"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/bots_responses&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>
Criar respostas
</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>ID Bot</b>
  <div class='graytext'>Coloque o IP do bot que por trás dessa</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='bot_id' value="" size='30' class='textinput' id="bot_id"></td>
</tr>
<tr>
  <td class='tablerow1'  valign='middle'><b>Palavras-chave</b>
    <div class='graytext'>Escreva as palavras seguidas de :exemplo: Olá, oi, Olá</div></td>
  <td class='tablerow2'  valign='middle'><label for="ai_type2">
    <textarea name="keywords" cols="30" rows="4" class="textinput" id="keywords"></textarea>
  </label></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>O que ele dirá?</b>
  <div class='graytext'>Anote tudo que você quer que eu diga o bot, mas não escrever um discurso.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><label for="ai_type">
  <textarea name="response_text" cols="30" rows="4" class="textinput" id="response_text"></textarea>
</label></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Como eu dize</b>
  <div class='graytext'>Selecione se você chorar, dizer normal ou sussurrando</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><label for="mode"></label>
  <select name="mode" id="mode">
    <option value="say" selected="selected">Dizer</option>
    <option value="shout">Gritar</option>
    <option value="whisper">Susurrar</option>
  </select></td>
</tr>
<tr>
  <td align='center' class='tablerow1' colspan='2' >Com as respostas, o bot por trás do que você tem que ouvir uma das palavras-chave que você postar ..</td>
</tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' name='submit' value='Adicionar Resposta' class='realbutton' accesskey='s'></td></tr>

</table>

</div>
</div>
</form>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($do == "edit" && is_numeric($key)) { ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<form action='<?php echo $adminpath; ?>/index/p/bots_responses&do=save&key=<?php echo $article['id']; ?>' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>
    BOT Responder<u><?php echo $article['title']; ?></u> (ID: <?php echo $article['id']; ?>) <img src="./images/edit.gif">
</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>ID</b>  <div class='graytext'>Coloque o IP do bot que por trás dessa.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='bot_id' value="<?php echo $article['bot_id']; ?>" size='30' class='textinput' id="bot_id" /></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>O que dizer?</b><div class='graytext'>Anote tudo que você quer que eu diga o bot, mas não escrever um discurso.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name="keywords" cols="30" rows="4" class="textinput" id="keywords"><?php echo $article['keywords']; ?></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>O que dizer?</b><div class='graytext'>Anote tudo que você quer que eu diga o bot, mas não escrever um discurso inflamado.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name="response_text" cols="30" rows="4" class="textinput" id="response_text"><?php echo $article['response_text']; ?></textarea></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Gritar?</b>
  <div class='graytext'>Selecciona si gritara.<br /><font color="green"><a href="http://fibbox.org">Edições FibboX.</a></font></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="mode" id="mode">
  <option value="say" selected="selected" <?php if (!(strcmp("say", $article['mode']))) {echo "selected=\"selected\"";} ?>>Dizer</option>
  <option value="shout" <?php if (!(strcmp("shout", $article['mode']))) {echo "selected=\"selected\"";} ?>>Gritar</option>
  <option value="whisper" <?php if (!(strcmp("whisper", $article['mode']))) {echo "selected=\"selected\"";} ?>>Susurrar</option>
</select></td>
</tr>

<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Atualizar resposta 'class =' realbutton 'accesskey='s'></td></tr>
</form></table></div><br />

<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ?>

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>