<?php
#####################################################################
#||||| mani0nsCMS - 2.0 [Public BETA] - Desenvolvido por nadi0s|||||#
#||||| Criado por nadi0s - HTML, PHP, CSS                      |||||#     
#||||| Criado por @rTiago - DATA_CLASSES, Housekeeping         |||||#
# ----------------------------------------------------------------- #
#||||| © Copyright 2012. LavvosCMS -  Estrutura                |||||#
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

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/lightweightmepage.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/lightweightmepage.js" type="text/javascript"></script>

		
	
<style>
#error{
	border: 1px solid #cd0a0a;
	background: #fef1ec; 
	color: #cd0a0a;
	padding: 10px;
	border-radius: 5px;
	margin-bottom: 10px;
	width:739px;
}
</style>
<style>

</style>


<div id="container">
<div id="content" style="position: relative" class="clearfix">

<div id="wide-personal-info">

    <div id="habbo-plate">
    <a href="<?php echo $path; ?>/profile">
    <img alt="<?php echo $name; ?>" src="<?php echo $avatar; echo $myrow['look']; ?>&size=b&direction=2&head_direction=3&gesture=sml&action=wav&size=l" width="64" height="110"/>
</a>
    </div>

    <div id="name-box" class="info-box">
        <div class="label">Nome:</div>
        <div class="content"><?php echo $name; ?></div>
    </div><div style="position:absolute; top:13px; left:380px;">	<a style="color:#fff;">
</div></a>
    <div id="motto-box" class="info-box">
        <div class="label">Missão: </div>
        <div class="content"><?php echo $myrow['motto']; ?></div>  
</div><div style="position:absolute; top:55px; left:380px;">	<a style="color:#fff;"></a></div>
    <div id="last-logged-in-box" class="info-box">
        <div class="label">Último Visita:</div>
        <div class="content"> <?php echo $myrow['last_online']; ?></div>
    </div>

<div class="enter-hotel-btn">
    <div class="open enter-btn">
            <a href="client" target="new">Entre no Hotel<i></i></a>
        <b></b>
    </div>
		

</div></div>
  





<div id="column1" class="column">
<div style="margin-bottom: 20px;">

<div id="contentBox" class="right" style="width:440px;">
<div id="contentTitle" class="blue">Convide seus Amigos!</div>
<div style="padding:5px">
<img src="<?php echo $path; ?>/web-gallery/credits/credits_poor.png" align="left">
Não adianta nada você estar no melhor jogo online do mundo se não estiver com seus amigos!
<b>Chame-os agora mesmo</b> e ajude o Hotel a ficar cada vez maior. Quem sabe as surpresas que vem por aí? :-)<br><br>
<input type="text" style="width:240px;" value="http://www.hirbohotel.com" disabled="disabled"/><br><br><br><br>

</div></div>

<p>

<div style="margin-bottom: px;">
<div id="contentBox" class="right" style="width:440px;">
<div id="contentTitle" class="red">Como consigo moedas no <?php echo $shortname; ?>?</div>
<div style="padding:9px">
<img src="http://cocahotel.biz/web-gallery/credits/credits_rich.png" align="left">
<p style="margin-top:7px;">Basta você ficar <b>bastante tempo online</b>! Você as recebe a cada 15 minutos de jogo. Não é demais?<br><br>

Você pode usá-las para comprar, vender ou fazer trocas de mobílias. Está esperando o quê? Entre agora!</p><br><br>

</div></div></div>







    </div>


<div style="position:absolute; top:142px; left:461px;">
<?php
// #########################################################################
// Notícias
// #########################################################################

require_once('./data_classes/server-data.php_data_classes-news.php.php');

// #########################################################################
// App Facebook
// #########################################################################
?>
</div>
<br/><br/>
<br/><br/>
<br/><br/>
<br/><br/>
<br/><br/>
<br/><br/>
<br/>





    </div>
</div>
</div>

</div>


<p class="last"></p>
</div>
<script type="text/javascript"> 
	HabboView.add(function() {
		L10N.put("personal_info.motto_editor.spamming", "Hey, não há spam!");
		PersonalInfo.init("");
	});
</script>

</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>


<div>
<div>
<div>

	</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>





</div>



<script type="text/javascript">
	HabboView.add(LoginFormUI.init);
</script>

</div>

<?php require_once('./templates/community_footer.php'); ?>