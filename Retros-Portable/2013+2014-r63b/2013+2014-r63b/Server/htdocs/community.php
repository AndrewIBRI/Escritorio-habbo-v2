<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');

$pagename = "Comunidade";
$pageid = "10";


require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/lightweightmepage.css" type="text/css" />

<script src="<?php echo $path; ?>/web-gallery/static/js/lightweightmepage.js" type="text/javascript"></script>

<div id="container">
	<div id="content" style="position: relative" class="clearfix">
   

<?php

require_once('./noticiasmobiles.php');

?>

<div id="column3" class="column">
</div>
</div>

<?php require_once('./templates/community_ads.php'); ?>

<?php require_once('./templates/community_footer.php'); ?>
