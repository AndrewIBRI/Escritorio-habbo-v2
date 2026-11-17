<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');

$pagename = "Duckets";
$pageid = "8";
$body_id = "home";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">

<div class="habblet-container ">		
	<div class="cbb clearfix pixelblue ">
		<h2 class="title">Aprenda como conseguir Duckets e divirta-se!</h2>
	<div class="pixels-infobox-container">
<div class="pixels-infobox-text">

<h3>Você pode ganhar Duckets de várias formas:</h3>
	<ul>
		<li><p>1. Entrando no <?php echo $shortname; ?> uma vez por dia</p></li>
                <li><p>2. Passando o tempo no <?php echo $shortname; ?>  quando mais tempo você ficar no Hotel, mais Duckets ganha!</p></li>
                <li><p>3. Conseguindo Emblemas de Conquistas, sendo <?php echo $shortname; ?> Guia ou dando Respeito a outros <?php echo $shortname; ?>s</p></li>
                <li><p>4. Juntando-se ao <?php echo $shortname; ?> VIP!</p></li>
	</ul>
    </div>
</div></div></div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
			 
</div>

<div id="column2" class="column">
	<div class="habblet-container ">		
		<div class="cbb clearfix pixelgreen ">
		<h2 class="title">Alugue algo!</h2>
	<div id="pixels-info" class="box-content pixels-info">
<div class="pixels-info-text clearfix">

<img class="pixels-image" src="<?php echo $path; ?>/web-gallery/v2/images/activehomes/pixelpage_effectmachine.png" alt=""/>
<p class="pixels-text">Crie um Quarto mais legal com esses novos Efeitos Especiais</p>
</div>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<div class="habblet-container ">		
	<div class="cbb clearfix pixellightblue ">
		<h2 class="title">Efeitos?</h2>
	<div id="pixels-info" class="box-content pixels-info">
<div class="pixels-info-text clearfix">

<img class="pixels-image" src="<?php echo $path; ?>/web-gallery/v2/images/activehomes/pixelpage_personaleffect.png" alt=""/>
<p class="pixels-text">Incremente seu <?php echo $shortname; ?> com Efeitos super divertidos para todas as ocasiões.</p>

</div>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
			 
<div class="habblet-container ">		
	<div class="cbb clearfix pixeldarkblue ">
		<h2 class="title">Ofertas?</h2>
	<div id="pixels-info" class="box-content pixels-info">
<div class="pixels-info-text clearfix">

<img class="pixels-image" src="<?php echo $path; ?>/web-gallery/v2/images/activehomes/pixelpage_discounts.png" alt=""/>
<p class="pixels-text">Os Duckets que você ganha dão descontos em Mobis e Efeitos Especiais. Confira o que você pode fazer com eles!</p>
</div>
</div>
</div>
</div>

<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<script type="text/javascript">
HabboView.run();
</script>
<div id="column3" class="column">

<?php require_once('./templates/community_ads.php'); ?>

<?php require_once('./templates/community_footer.php'); ?>