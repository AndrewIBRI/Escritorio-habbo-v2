<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Comunidade";
$pageid = "10";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
	<div id="content" style="position: relative" class="clearfix">

<div id="column1" class="column">
			     		
<div class="habblet-container ">		
<div class="cbb clearfix blue ">
<div class="box-tabs-container clearfix">
    <h2>Grupos</h2>
    <ul class="box-tabs">
        <li id="tab-0-1-1"><a href="#">Top Groups</a><span class="tab-spacer"></span></li>
        <li id="tab-0-1-2" class="selected"><a href="#">Recent Threads</a><span class="tab-spacer"></span></li>
    </ul>
</div>

<div id="tab-0-1-1-content"  style="display: none">
	<div class="progressbar"><img src="./web-gallery/v2/images/page_loader.gif" alt="" width="29" height="6" /></div>
	<a href="./habblet/proxy.php?hid=h122" class="tab-ajax"></a>
</div>

    <div id="tab-0-1-2-content" >

<ul class="active-discussions-toplist">
<?php
$i = 0;
$getem = mysql_query("SELECT * FROM cms_forum_threads ORDER BY unix DESC LIMIT 10") or die(mysql_error());

while ($row = mysql_fetch_assoc($getem)) {
    if($row['owner'] !== ""){
        $i++;

        if(IsEven($i)){
            $even = "even";
        } else {
            $even = "odd";
        }

	$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_forum_posts WHERE threadid = '".$row['id']."'");
	$pages = ceil($posts / 10);
	$pagelink = "<a href=\"".$path."/thread/".$row['id']."\" class=\"topiclist-page-link secondary\">1</a>";

	if($pages > 4){
		$pageat = $pages - 2;
		$pagelink .= "\n...";
		while($pageat <= $pages){
		$pagelink .= " <a href=\"".$path."/thread/".$row['id']."&page=".$pageat."\" class=\"topiclist-page-link secondary\">".$pageat."</a>";
		$pageat++;
		}
	}elseif($pages != 1){
		$pageat = 2;
		while($pageat <= $pages){
		$pagelink .= " <a href=\"".$path."/thread/".$row['id']."&page=".$pageat."\" class=\"topiclist-page-link secondary\">".$pageat."</a>";
		$pageat++;
		}
	}
?>

<li class="<?php echo $even; ?>" >
<a href="<?php echo $path; ?>/thread/<?php echo $row['id']; ?>" class="topic">
<span><?php echo HoloText($row['title']); ?></span>
</a>

<div class="topic-info post-icon">
<span class="grey">(</span><?php echo $pagelink; ?><span class="grey">)</span>
</div>
</li>

<?php } } ?>

</ul>
<div id="active-discussions-toplist-hidden-h121" style="display: none">
<ul class="active-discussions-toplist" style="padding-top:0;">

<?php
$i = 0;
$getem = mysql_query("SELECT * FROM cms_forum_threads ORDER BY unix DESC LIMIT 40 OFFSET 10") or die(mysql_error());

while ($row = mysql_fetch_assoc($getem)) {
    if($row['owner'] !== ""){
        $i++;

        if(IsEven($i)){
            $even = "even";
        } else {
            $even = "odd";
        }

	$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_forum_posts WHERE threadid = '".$row['id']."'");
	$pages = ceil($posts / 10);
	$pagelink = "<a href=\"".$path."/thread/".$row['id']."\" class=\"topiclist-page-link secondary\">1</a>";

	if($pages > 4){
		$pageat = $pages - 2;
		$pagelink .= "\n...";
		while($pageat <= $pages){
		$pagelink .= " <a href=\"".$path."/thread/".$row['id']."&page=".$pageat."\" class=\"topiclist-page-link secondary\">".$pageat."</a>";
		$pageat++;
		}
	}elseif($pages != 1){
		$pageat = 2;
		while($pageat <= $pages){
		$pagelink .= " <a href=\"".$path."/thread/".$row['id']."&page=".$pageat."\" class=\"topiclist-page-link secondary\">".$pageat."</a>";
		$pageat++;
		}
	}
?>

<li class="<?php echo $even; ?>" >
<a href="<?php echo $path; ?>/thread/<?php echo $row['id']; ?>" class="topic">
<span><?php echo HoloText($row['title']); ?></span>
</a>

<div class="topic-info post-icon">
<span class="grey">(</span><?php echo $pagelink; ?><span class="grey">)</span>
</div>
</li>

<?php } } ?>

</ul>

</div>
<div class="clearfix">
    <a href="#" class="discussions-toggle-more-data secondary" id="discussions-toggle-more-data-h121">Show more threads</a>
</div>

<script type="text/javascript">
L10N.put("show.more.discussions", "Show more threads");
L10N.put("show.less.discussions", "Show less threads");
var discussionMoreDataHelper = new MoreDataHelper("discussions-toggle-more-data-h121", "active-discussions-toplist-hidden-h121","discussions");
</script>

</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
     		
<div class="habblet-container ">		
<div class="cbb clearfix activehomes ">
    <center>
<div id="contentTitle" class="lightblue" style="width:440px;">Usuários aleatórios - Clique Us!</div></center>
<div id="homes-habblet-list-container" class="habblet-list-container">
<img class="active-habbo-imagemap" src="./web-gallery/v2/images/activehomes/transparent_area.gif" width="435px" height="230px" usemap="#habbomap" />

<?php
$i = 0;
$getem = mysql_query("SELECT * FROM users ORDER BY RAND() LIMIT 18") or die(mysql_error());

while ($row = mysql_fetch_assoc($getem)) {
$i++;
$list_id = $i - 1;

if($row['online'] >= 1){
	$status = "online";
} else {
	$status = "offline";
}

?>
<div id="active-habbo-data-<?php echo $list_id; ?>" class="active-habbo-data">
<div class="active-habbo-data-container">
<div class="active-name <?php echo $status; ?>"><?php echo $row['username']; // (echo $row['real_name']; ) ?></div>
                       Joined on: <?php echo date('F d, Y', $row['account_created']); ?>
                            <p class="moto"><?php echo HoloText($row['motto']); ?></p>
                    </div>
                </div>
                <input type="hidden" id="active-habbo-url-<?php echo $list_id; ?>" value="<?php echo $path; ?>/home/<?php echo $row['username']; ?>"/>
                <input type="hidden" id="active-habbo-image-<?php echo $list_id; ?>" class="active-habbo-image" value="<?php echo $avatar; echo $row['look']; ?>&size=b&direction=2&action=std,wav,&head_direction=3&gesture=smln" />

<?php } ?>
                                                        
            <div id="placeholder-container">
                    <div id="active-habbo-image-placeholder-0" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-1" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-2" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-3" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-4" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-5" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-6" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-7" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-8" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-9" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-10" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-11" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-12" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-13" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-14" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-15" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-16" class="active-habbo-image-placeholder"></div>
                    <div id="active-habbo-image-placeholder-17" class="active-habbo-image-placeholder"></div>
            </div>
    </div>

    <map id="habbomap" name="habbomap">
            <area id="imagemap-area-0" shape="rect" coords="55,53,95,103" href="#" alt=""/>
            <area id="imagemap-area-1" shape="rect" coords="120,53,160,103" href="#" alt=""/>
            <area id="imagemap-area-2" shape="rect" coords="185,53,225,103" href="#" alt=""/>
            <area id="imagemap-area-3" shape="rect" coords="250,53,290,103" href="#" alt=""/>
            <area id="imagemap-area-4" shape="rect" coords="315,53,355,103" href="#" alt=""/>
            <area id="imagemap-area-5" shape="rect" coords="380,53,420,103" href="#" alt=""/>
            <area id="imagemap-area-6" shape="rect" coords="28,103,68,153" href="#" alt=""/>
            <area id="imagemap-area-7" shape="rect" coords="93,103,133,153" href="#" alt=""/>
            <area id="imagemap-area-8" shape="rect" coords="158,103,198,153" href="#" alt=""/>
            <area id="imagemap-area-9" shape="rect" coords="223,103,263,153" href="#" alt=""/>
            <area id="imagemap-area-10" shape="rect" coords="288,103,328,153" href="#" alt=""/>
            <area id="imagemap-area-11" shape="rect" coords="353,103,393,153" href="#" alt=""/>
            <area id="imagemap-area-12" shape="rect" coords="55,153,95,203" href="#" alt=""/>
            <area id="imagemap-area-13" shape="rect" coords="120,153,160,203" href="#" alt=""/>
            <area id="imagemap-area-14" shape="rect" coords="185,153,225,203" href="#" alt=""/>
            <area id="imagemap-area-15" shape="rect" coords="250,153,290,203" href="#" alt=""/>
            <area id="imagemap-area-16" shape="rect" coords="315,153,355,203" href="#" alt=""/>
            <area id="imagemap-area-17" shape="rect" coords="380,153,420,203" href="#" alt=""/>
    </map>
<script type="text/javascript">
    var activeHabbosHabblet = new ActiveHabbosHabblet();
    document.observe("dom:loaded", function() { activeHabbosHabblet.generateRandomImages(); });
</script>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>
    <div id="column2" class="column">


<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>



<?php require_once('./data_classes/server-data.php_data_classes-news.php.php'); ?>


</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>
<?php if(!$vip_a['vip'] == '1'){?>
<br />
<script type="text/javascript"><!--
google_ad_client = "ca-pub-3468458975227842";
/* Paradise Medium */
google_ad_slot = "5681714024";
google_ad_width = 300;
google_ad_height = 250;
//-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>
<?php } ?>

<script type="text/javascript">
	HabboView.add(LoginFormUI.init);
</script>

<?php require_once('./templates/community_footer.php'); ?>