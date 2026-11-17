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

$pagename = "Perguntas Frequentes";
$body_id = "faq";
include('./templates/community_subheader.php');
include('./templates/faq_header.php');

$id = FilterText($_GET['web-faq-id']);
$query = FilterText($_POST['query']);

?>

<div id="faq-category-content" class="clearfix" >

<?php
if(isset($_GET['web-faq-id'])){
	$sql = mysql_query("SELECT * FROM help_topics WHERE subject = '".$id."' LIMIT 1");
	$row = mysql_fetch_assoc($sql);
	if($row['title'] != ""){
?>
		<p class="faq-category-description">Aqui está uma lista de dúvidas frequentes entre os usuários do Hotel.</p>
		<?php
		$sql = mysql_query("SELECT * FROM help_topics WHERE subject = '".$id."'") or die(mysql_error());
		$i = 0;
	while($itemrow = mysql_fetch_assoc($sql)){
	$i++;
	if($i == 1){ $selected = "selected"; }
	if($i != 1){ $faqitem = "$(\"faq-item-content-".$itemrow['id']."\").hide();"; }
?>

<h4 id="faq-item-header-<?php echo $itemrow['id']; ?>" class="faq-item-header faq-toggle "><span class="faq-toggle <?php echo $selected; ?>" id="faq-header-text-<?php echo $itemrow['id']; ?>"><?php echo $itemrow['title']; ?></span></h4>
	<div id="faq-item-content-<?php echo $itemrow['id']; ?>" class="faq-item-content clearfix">
	    <div class="faq-item-content clearfix"><?php echo $itemrow['body']; ?></div>
	<div class="faq-close-container">
	<div id="faq-close-button-<?php echo $itemrow['id']; ?>" class="faq-close-button clearfix faq-toggle" style="display:none">Fechar FAQ <img id="faq-close-image-<?php echo $itemrow['id']; ?>" class="faq-toggle" src="<?php echo $path; ?>/web-gallery/v2/images/faq/close_btn.png"></div>
	</div>
	</div>

	<script type="text/javascript">
	    <?php echo $faqitem; ?>
	    $("faq-close-button-<?php echo $itemrow['id']; ?>").show();
	</script>

<?php } } }elseif(isset($_POST['query'])){
	$sql = mysql_query("SELECT * FROM help_topics WHERE title LIKE '%".$query."%' OR body LIKE '%".$query."%'") or die(mysql_error());
	$count = mysql_num_rows($sql);
	if($count == 0){;
		echo "Inexistente, tente novamente...";
	}else{
		while($itemrow = mysql_fetch_assoc($sql)){
		$sql2 = mysql_query("SELECT * FROM help_topics WHERE id = '".$itemrow['catid']."' LIMIT 1");
		$catrow = mysql_fetch_assoc($sql2);
		$cat = $catrow['title'];
?>

<h4 id="faq-item-header-<?php echo $itemrow['id']; ?>" class="faq-item-header faq-toggle "><span class="faq-toggle " id="faq-header-text-<?php echo $itemrow['id']; ?>"><?php echo $itemrow['title']; ?></span><span class="item-category"> - <?php echo $cat; ?></span></h4>
	<div id="faq-item-content-<?php echo $itemrow['id']; ?>" class="faq-item-content clearfix">
	    <div class="faq-item-content clearfix"><?php echo HoloText($itemrow['body'], true); ?></div>
	<div class="faq-close-container">
	<div id="faq-close-button-<?php echo $itemrow['id']; ?>" class="faq-close-button clearfix faq-toggle" style="display:none">Fechar FAQ<img id="faq-close-image-<?php echo $itemrow['id']; ?>" class="faq-toggle" src="<?php echo $path; ?>/web-gallery/v2/images/faq/close_btn.png"/></div>
	</div>
	</div>

	<script type="text/javascript">
	    $("faq-item-content-<?php echo $itemrow['id']; ?>").hide();
	    $("faq-close-button-<?php echo $itemrow['id']; ?>").show();
	</script>

<?php } } } ?>
<script type="text/javascript">
    FaqItems.init();
    SearchBoxHelper.init();
</script>
</div>

</div>
<?php require_once('./templates/login_footer.php'); ?>