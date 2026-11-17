<?php
#####################################################################
#||||| 		Project Hey desenvolvido por Pixel Ação & Poko Design		   |||||#
#||||| Criado por Pixel Ação & Poko Design	- HTML, PHP, CSS & Data_classes      |||||#
# ----------------------------------------------------------------- #
#||||| © Copyright 2013.Hey CMS - Softwares PokoDesigns   |||||#
#||||| © Copyright 2012. LavvosCMS - DATA_CLASSES Estrutura	   |||||#
#||||| © Copyright 2012. Sulake Corporation - Habbo Arquivos   |||||#
#####################################################################

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
 <div style="position:absolute; top:7px; left:320px;">	
<div id="habbo" class="drag"></div>
</div>

<script type="text/javascript" src="../styles/drag.js"></script>
<div id="container">
<div id="content">

<div id="column2" class="column">
<div id="conteudoBox" class="right" style="width:540px;margin-bottom:3px;">


<div id="article-wrapper">
	<h2><?php echo HoloText($news['title']); ?></h2>
	<div class="article-meta" style="margin-top:-4px;">Publicado em: <b><?php echo date('d/m/Y', $news['published']); ?></b> por <b><?php echo $news['author']; ?></b>.	</div>
	<p class="summary"><?php echo $news['longstory']; ?></p>
	<div class="article-body">
	<p><?php echo (HoloText($news['shortstory'], true)); ?></p>

</div>
</div>
</div>
</div>
</div>

<div id="column1" class="column">
<div id="contentBox" class="right" style="width:190px;">
<div id="contentTitle" class="orange">Notícias</div><div style="padding:5px">
	
</div>
<div id="article-archive">
<?php

if ($news_id > 0)
{
	for ($i = 0; $i < 7; $i++)
	{
		$sectionName = "";
		$sectionCutoffMax = 0;
		$sectionCutoffMin = 0;
		
		switch ($i)
		{
			case 0:
			
				$sectionName = "Hoje";
				$sectionCutoffMax = time();
				$sectionCutoffMin = time() - 86400;
				break;
				
			case 1:
			
				$sectionName = "Ontem";
				$sectionCutoffMax = time() - 86400;
				$sectionCutoffMin = time() - 172800;
				break;
				
			case 2: 
			
				$sectionName = "Essa semana";
				$sectionCutoffMax = time() - 172800;
				$sectionCutoffMin = time() - 604800;
				break;
				
			case 3:
			
				$sectionName = "Semana passada";
				$sectionCutoffMax = time() - 604800;
				$sectionCutoffMin = time() - 1209600;
				break;
				
			case 4:
			
				$sectionName = "Esse mês";
				$sectionCutoffMax = time() - 1209600;
				$sectionCutoffMin = time() - 2592000;
				break;
				
			case 5:
			
				$sectionName = "Mês passado";
				$sectionCutoffMax = time() - 2592000;
				$sectionCutoffMin = time() - 5184000;
				break;
                	

                         /*case 6:
			
				$sectionName = "Meses anteriores";
				$sectionCutoffMax = time();
				$sectionCutoffMin = time();
				break;*/

		}

	$sql = mysql_query("SELECT * FROM cms_news WHERE published >= " . $sectionCutoffMin . " AND published <= " . $sectionCutoffMax .  " ORDER BY published DESC"); 
	if(mysql_num_rows($sql) > 0){
?>

	<?php echo "<h2>"; echo $sectionName; echo "</h2><ul>"; ?>
<?php while($row = mysql_fetch_assoc($sql)){ ?>
	<li><?php if($news_id !== $row['id']){ echo"<a href=\"".$path."/articles/".$row['id']."\">"; } ?>
	<?php echo $row['title']; ?> &raquo;</a> 
	<?php if($news_id !== $row['id']){ echo"</a>"; } ?></li>
	
<?php } ?>

</ul>

<?php } } } ?>

<ul>
	<li>
	<a href="<?php echo $path; ?>/noticias" class="article">Mais notícias »</a>
	</li>
</ul>
</div>

</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>





<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>

<?php require_once('./templates/community_footer.php'); ?>