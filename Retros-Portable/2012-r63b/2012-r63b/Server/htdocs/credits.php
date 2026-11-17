<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Créditos";
$pageid = "5";
$body_id = "newcredits";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php'); 

?> 

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column"><div class="habblet-container ">		
<div class="cbb clearfix orange "> 
	
<h2 class="title">Obter créditos-prêmio!</h2>

<style type="text/css">
.method-group.online.premium {
 background-image: url(<?php echo $path; ?>/web-gallery/v2/images/newcredits/de_strike_top_angebot.png);
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

<?php include('./templates/community_footer.php'); ?>
<?php require_once('./templates/community_footer.php'); ?>