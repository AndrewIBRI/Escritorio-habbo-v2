<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');

$body_id = "news";
$pageid = "11";

$news_id = FilterText($_GET['web-articles-id']);
$main_sql = mysql_query("SELECT * FROM cms_news WHERE id = '".$news_id."'") or die(mysql_error());
$article_exists = mysql_num_rows($main_sql);

if($article_exists == "1"){
	$news = mysql_fetch_assoc($main_sql);
	$pagename = "Notícias - ".HoloText($news['title'])."";
} else {
	$main_sql = mysql_query("SELECT * FROM cms_news ORDER BY ID DESC") or die(mysql_error());
	$news = mysql_fetch_assoc($main_sql);
	$news_id = $news['id'];
	$pagename = "Notícias - ".HoloText($news['title'])."";
}


require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content">
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix red">
	<h2 class="title">Notícias</h2>
<div id="article-archive">

<?php
	$sql = mysql_query("SELECT * FROM cms_news ORDER BY id DESC"); 
	if(mysql_num_rows($sql) > 0){
?>
<h2>Últimas Notícias</h2>
<ul>

<?php while($row = mysql_fetch_assoc($sql)){ ?>

	<li><?php if($news_id !== $row['id']){ echo"<a href=\"".$path."/articles/".$row['id']."\">"; } ?>
	<font color="red"><?php echo $row['title']; ?></a> &raquo;</font>
	<?php if($news_id !== $row['id']){ echo"</a>"; } ?></li>
	
<?php } ?>
</ul>

<?php } ?>


</div>

</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix notitle ">

<div id="article-wrapper">
	<h2><font color="red"><?php echo HoloText($news['title']); ?></font></h2><font color="grey"><?php echo date('d/m/Y', $news['published']); ?> por <b><?php echo $news['author']; ?></b></i></font><br><hr />
	
	<p class="summary"><?php echo (HoloText($news['longstory'])); ?></p>
	<div class="article-body">
	<p><?php echo (HoloText($news['shortstory'], true)); ?></p>
	
       	<div class="article-body">
</iframe>









	
</div></div></div>
</div>


<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>






</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<div id="column3" class="column">
</div></div>

<?php require_once('./templates/community_footer.php'); ?>