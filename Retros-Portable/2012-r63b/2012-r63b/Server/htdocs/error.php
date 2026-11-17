<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');


$pageid = "10a";
$pagename = "Página não encontrada!";
$title = "Página não encontrada!";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');
?>
<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix darkred ">

<h2 class="title">Página não encontrada!</h2>

<div id="notfound-content" class="box-content">
<p class="error-text">Desculpe, mas nenhuma página for solicitada.</p> <img id="error-image" src="./web-gallery/v2/images/error.gif" />
<p class="error-text">Por favor, re-introduzir a URL. Se você retornar a esta página, use o botão 'Voltar' para voltar ao topo.</p>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>

<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix green ">
	<h2 class="title">Você estava procurando... </h2>
<div id="notfound-looking-for" class="box-content">

<p><b>Grupo de Amigos ou página principal?</b><br/>
    Verifica se a página aparece em <a href="<?php echo $path; ?>/community">Comunidade</a>.</p>

    <p><b>Salas em greve?</b><br/>
    Confira o <a href="<?php echo $path; ?>/community">Quartos recomendados</a>.</p>

    <p><b>Que outros <?php echo $shortname; ?>s está conectado?</b><br/>
    Confira o Top <a href="<?php echo $path; ?>/tag">Etiqueta</a>.</p>

     <p><b>Como posso comprar créditos?</b><br/>
    Confira página <a href="<?php echo $path; ?>/credits">Créditos</a>.</p>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<?php require_once('./templates/community_footer.php'); ?>