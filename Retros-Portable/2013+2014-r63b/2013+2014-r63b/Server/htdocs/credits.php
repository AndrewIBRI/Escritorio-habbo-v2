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

$pagename = "Moedas";
$pageid = "5";
$body_id = "newcredits";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php'); 

?> 

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column"><div class="habblet-container ">		
<div class="cbb clearfix orange "> 
	
<h2 class="title">Moedas grátis!</h2>

<style type="text/css">
.method-group.online.premium {
 background-image: url(http://images.habbo.com/c_images/credits/credit_promo_pumpkins.png);
 background-repeat: no-repeat;
}

</style>


<?php require_once('./templates/pages/credits.php'); ?>

<!--[if lt IE 7]>
<script type="text/javascript">
Pngfix.doPngImageFix();
</script>
<![endif]--> 
<script type="text/javascript"> 
HabboView.run(); 
</script>

<?php require_once('./templates/community_ads.php'); ?>
<?php include('./templates/community_footer.php'); ?>
<?php require_once('./templates/community_footer.php'); ?>