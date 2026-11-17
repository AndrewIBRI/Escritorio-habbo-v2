<?php
#####################################################################
#||||| 		Project MyHobba desenvolvido por @rtiag0		   |||||#
#||||| Criado por @rtiag0 - HTML, PHP, CSS & Data_classes      |||||#
# ----------------------------------------------------------------- #
#||||| © Copyright 2012. Hobba 2011 - Open Source Software     |||||#
#||||| © Copyright 2012. LavvosCMS - DATA_CLASSES Estrutura	   |||||#
#||||| © Copyright 2012. Sulake Corporation - Habbo Arquivos   |||||#
#####################################################################

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');


if($_GET['do'] == "RemoveFeedItem" && is_numeric($_GET['key'])){
    mysql_query("DELETE FROM cms_alerts WHERE userid = '".$my_id."' AND id = '".FilterText($_GET['key'])."' ORDER BY id ASC LIMIT 1") or die(mysql_error());
}

$pagename = Principal;
$pageid = "1";


include('templates/community_subheader.php');
include('templates/community_header.php');

$fetch_tags = mysql_query("SELECT tag,id FROM user_tags WHERE user_id = '".$my_id."' LIMIT 20") or die(mysql_error());
$tags_num = mysql_num_rows($fetch_tags);

$randomq[] = "Quem é o seu ator favorito?";
$randomq[] = "Qual é o esporte que você gosta?";
$randomq[] = "Qual é o seu sobrenome?";
$randomq[] = "Como se chama sua canção favorita?";
$randomq[] = "Qual foi seu ano favorito?";
$randomq[] = "Qual é a sua banda favorita?";
$randomq[] = "Qual é o programa de televisão que você mais gosta?";
$randomq[] = "Qual é o instrumento que gostaria de tocar?";
$randomq[] = "Escolha: Pizza ou Hamburguer";
$randomq[] = "Qual é o nome do esporte que você gosta?";
$randomq[] = "Quando você vai a alguma festa, com qual roupa vai?";
$randomq[] = "Qual o nome do seu namorado (a)?";
$randomq[] = "Qual o animal que você gosta?";
$randomq[] = "Qual é a sua cor preferida?";
$randomq[] = "Qual você prefere: Jonas Brothers ou Justin Bieber";
$randomq[] = "Qual é o estilo de música que você gosta?";
$randomq[] = "Qual é a palavra que te define?";
$randomq[] = "Qual é o melhor: Habbo ou ".$shortname."?";
$randomq[] = "Qual é o seu staff favorito?";
$randomq[] = "Qual jornal você assiste?";
$randomq[] = "Quais idiomas você fala?";
$randomq[] = "O Melhor do mundo é...?";
$randomq[] = "Qual é o mobi que você mais gosta?";
$randomq[] = "Oque você faz quando está na frente do PC?";

srand ((double) microtime() * 1000000);
$chosen = rand(0,count($randomq)-1);

$tag_question = $randomq[$chosen];

$get_flashclient = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_flashclient' AND value = '1'");

?>

<div id="container"> 
	<div id="content"> 
<div style="float:right">

</div>

</div>
 
<img src="<?php echo $path; ?>/web-gallery/v2/images/personal_info/bg_enter_hotel.png" style="margin-bottom: 9px; margin-left: 1px; float: left;"> 
 
<div id="column1" class="column"> 
<div class="habblet-container ">
					<div id="new-personal-info" style="height: 0px; margin-bottom: -143px;"> 
 
	<div class="enter-hotel-btn" style="margin-top: -120px; margin-right: 170px;"> 
				<div class="open enter-btn"> 
				<a href="<?php echo $path; ?>/client" target="client" onclick="openOrFocusHabbo(this); return false;">Entrar no Hotel<i></i></a> 
				
				
			<b></b> 
		

		</div> 
	</div> 
 

    </div>
</div>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/lightweightmepage.css" type="text/css" />

<script src="<?php echo $path; ?>/web-gallery/static/js/lightweightmepage.js" type="text/javascript"></script>




<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column"><div class="habblet-container ">	
	
<div id="new-personal-info" style="background-image:url(<?php echo $path; ?>/web-gallery/v2/images/personal_info/menu_view.png)">
<div style="position:absolute; top:11px; left:375px;">	
<a style="color:#fff;"><strong><?php echo $myrow['credits']; ?></strong></a></div>
<div style="position:absolute; top:43px; left:375px;">	
<a style="color:#fff;"><strong><?php echo $myrow['activity_points']; ?></strong></a></div>
<div style="position:absolute; top:75px; left:375px;">	
<a style="color:#fff;"><strong><?php echo getHCDays($my_id); ?></strong></div>
<div style="position:absolute; top:104px; left:375px;">	

</div>
<div  style="position:absolute; margin-left:104px; top:30px;"> 
<a href="<?php echo $path; ?>/profile"><img alt="<?php echo $name; ?>" src="http://www.habbo.fr/habbo-imaging/avatarimage?figure=<?php echo $myrow['look']; ?>&size=b&direction=2&action=stand&head_direction=3&gesture=sml"></a>
</div>
<div id="habbo-info">
<div id="motto-container" class="clearfix">
	<strong><?php echo $name; ?>:</strong>
	<div>
	<a href="<?php echo $path; ?>/profile"><span title="Clique para alterar sua missão/status"><?php echo $myrow['motto']; ?></span></a>
	<p style="display: none"><input type="text" length="30" name="motto" value="<?php echo $myrow['motto']; ?>"/></p>
</div>
</div>
		
<div id="motto-links" style="display: none"><a href="#" id="motto-cancel">Cancel.</a></div>

</div>

<div id="habbo-feed">
<ul id="feed-items">

<li class="small" id="feed-lastlogin">
Último Login: <?php echo $myrow['last_online']; ?>
</li>

<li class="small" id="feed-item-hcgift">
Caso você tenha alguma dúvida, entre em contato conosco pelo e-mail: habbiesuporte@outlook.com, resposta dentro de 24hrs.
</li>

<li class="small" id="">
No Habbie você adquiri moedas e duckets gratuitamente a cada quinze minutos, pondendo comprar mobílias e raros exclusivos disponíveis em nossa loja.
</li>

</ul>
</div>

<p class="last"></p>
</div>
<script type="text/javascript"> 
	HabboView.add(function() {
		L10N.put("personal_info.motto_editor.spamming", "M&uuml;ll mich nicht voll!");
		PersonalInfo.init("");
	});
</script>

<?php

require_once('./noticiasmobiles.php');

?>

</div>

<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>


<?php
// #########################################################################
// Notícias
// #########################################################################

require_once('./data_classes/server-data.php_data_classes-news.php.php');

// #########################################################################
// Da Gerência
// #########################################################################
?>

                <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

        

       <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>

<script type="text/javascript">
	HabboView.add(LoginFormUI.init);
</script>

<?php require_once('./templates/community_ads.php'); ?>


<?php require_once('./templates/community_footer.php'); ?>