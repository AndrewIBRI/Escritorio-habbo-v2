<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Buscador de 'Tags'";
$pageid = "5";
$body_id = "tags";

$search = FilterText($_GET['tag']);
$pagenum = FilterText($_GET['pageNumber']);

if(!isset($_GET['pageNumber']) || $pagenum < 1){
	$pagenum = 1;
}

$correct = HoloText($search);

if(strlen($search) > 20 || strlen($search) < 1 || $search != $correct){
	$search = "";
}

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">
<div class="habblet-container ">		
<div class="cbb clearfix default ">
	
<h2 class="title">Tags relacionados</h2>
<div id="tag-related-habblet-container" class="habblet box-contents">

<?php

$sql = mysql_query("SELECT tag, COUNT(id) AS quantity FROM user_tags GROUP BY tag ORDER BY quantity DESC LIMIT 20") or die(mysql_error());
if(mysql_num_rows($sql) < 1){
	echo"No hay Tags para Mostrar";
}else{
	echo "<ul class=\"tag-list\">";
	for($i=0;($array[$i] = @    mysql_fetch_array($sql,1))!="";$i++)
        {
            $row[] = $array[$i];
        }
	sort($row);
	$i = -1;
	while($i <> mysql_num_rows($sql)){
		$i++;
		$tag = $row[$i]['tag'];
		$count = $row[$i]['quantity'];
		$tags[$tag] = $count;
	}
		
		$max_qty = max(array_values($tags));
		$min_qty = min(array_values($tags));
		$spread = $max_qty - $min_qty;

		if($spread == 0){ $spread = 1; }

		$step = (200 - 100)/($spread);

		foreach($tags as $key => $value){
		    $size = 100 + (($value - $min_qty) * $step);
		    $size = ceil($size);
		    echo "<li><a href=\"".$path."/tag/".strtolower(HoloText($key))."\" class=\"tag\" style=\"font-size:".$size."%\">".trim(strtolower($key))."</a> </li>\n";
		}

echo "</ul>";
}
?>
</div>
	
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
			 
<div class="habblet-container ">		
<div class="cbb clearfix default ">
<h2 class="title">Luta de Tags</h2>
<div id="tag-fight-habblet-container">
<div class="fight-process" id="fight-process">Combate</div>
<div id="fightForm" class="fight-form">
    <div class="tag-field-container">Tag 1<br /><input maxlength="30" type="text" class="tag-input" name="tag1" id="tag1"/></div>

    <div class="tag-field-container">Tag 2<br /><input maxlength="30" type="text" class="tag-input" name="tag2" id="tag2"/></div>
</div>
<div id="fightResults" class="fight-results">
    <div class="fight-image">
    	<img src="<?php echo $path; ?>/web-gallery/images/tagfight/tagfight_start.gif" alt="" name="fightanimation" id="fightanimation" />
        <a id="tag-fight-button" href="#" class="new-button" onclick="TagFight.init(); return false;"><b>Lutar</b><i></i></a>
    </div>
</div>
<div class="tagfight-preload">
	<img src="<?php echo $path; ?>/web-gallery/images/tagfight/tagfight_end_0.gif" width="1" height="1"/>
	<img src="<?php echo $path; ?>/web-gallery/images/tagfight/tagfight_end_1.gif" width="1" height="1"/>
	<img src="<?php echo $path; ?>/web-gallery/images/tagfight/tagfight_end_2.gif" width="1" height="1"/>
	<img src="<?php echo $path; ?>/web-gallery/images/tagfight/tagfight_loop.gif" width="1" height="1"/>
	<img src="<?php echo $path; ?>/web-gallery/images/tagfight/tagfight_start.gif" width="1" height="1"/>
</div>
</div>
	
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<?php if($logged_in){ ?>
<div class="habblet-container ">		
<div class="cbb clearfix default ">
<h2 class="title">Lords Famosos</h2>
<div id="tag-match-habblet-container">
<div id="tag-match-result">
<?php echo $sitename; ?> Nome&nbsp;do Senhor, para saber se&nbsp;voc&ecirc; tem que&nbsp;introduzir&nbsp;&nbsp;muitas Tags</div>

<div id="tag-match-form">
<input type="text" id="tag-match-friend" value=""/>
<a class="new-button" id="tag-match-send" href="#"><b>Confira</b><i></i></a>
</div>
<script type="text/javascript">
document.observe("dom:loaded", function() {
    Event.observe($('tag-match-friend'), "keypress", function(e) {
        if (e.keyCode == Event.KEY_RETURN) {
            TagHelper.matchFriend();
        }
    });
    Event.observe($('tag-match-send'), "click", function(e) { Event.stop(e); TagHelper.matchFriend(); });
});
</script>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<?php } ?>

</div>


<div id="column2" class="column">
<div class="habblet-container ">		
<div class="cbb clearfix default ">
<h2 class="title">Buscador de Tags</h2>
<?php $bypass = true; require_once('./habblet/ajax_tagsearch.php'); ?>
	
</div>
</div>

<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>
<script type="text/javascript">
HabboView.run();
</script>

<?php include('./templates/community_footer.php'); ?>