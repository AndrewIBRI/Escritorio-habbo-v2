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

$pageid = " ";
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
<p class="error-text">Desculpe, não encontramos a página que você procura.</p> <img id="error-image" src="./web-gallery/v2/images/error.gif" />
<p class="error-text">Tente digitar a URL novamente. Se vier para aqui de novo utilize o botão "Voltar" para retornar onde você estava.</p>

</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>

<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix green ">
	<h2 class="title">O que você procura?</h2>
<div id="notfound-looking-for" class="box-content">

<p><b>Um Grupo ou Página Pessoal?</b><br/>
    Veja se está na página <a href="<?php echo $path; ?>/community">Comunidade</a></p>

    <p><b>Quartos estonteantes?</b><br/>
    Procure em <a href="<?php echo $path; ?>/community">Quartos recomendados</a>.</p>

    <p><b>Oque outros <?php echo $shortname; ?>s estão fazendo?</b><br/>
   Veja na lista de <a href="<?php echo $path; ?>/tag">Etiquetas</a>.</p>

     <p><b>Como conseguir Moedas</b><br/>
    Veja na página de <a href="<?php echo $path; ?>/credits">Moedas</a>.</p>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<?php require_once('./templates/community_footer.php'); ?>