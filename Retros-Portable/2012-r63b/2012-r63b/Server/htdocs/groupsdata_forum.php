<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

if(isset($_POST['searchString'])){
	$searchString = FilterText($_POST['searchString']);
	$check = mysql_query("SELECT id FROM group_details WHERE name LIKE '".$searchString."' LIMIT 1") or die(mysql_error());
	if(mysql_num_rows($check) > 0){
		$tmp = mysql_fetch_assoc($check);
		header("Location: ".$path."/groups/" . $tmp['id']);
		exit;
	}
}

$threadid = FilterText($_GET['web-thread-id']);
$pageid = "10b";

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////// THREAD ///////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if(!empty($threadid) && $_GET['web-discussions-id'] && is_numeric($threadid)){
	$check = mysql_query("SELECT * FROM cms_forum_threads WHERE id = '".$threadid."' LIMIT 1") or die(mysql_error());
	$exists = mysql_num_rows($check);
	if($exists > 0){
		$thread = mysql_fetch_assoc($check);
		$valid_thread = true;
		mysql_query("UPDATE cms_forum_threads SET views = views + 1 WHERE id = '".$threadid."' LIMIT 1") or die(mysql_error());
	} else {
		header("Location: ".$path."/community");
		exit;
	}
} elseif(!$_GET['web-discussions-id']) {
	header("Location: ".$path."/community");
	exit;
}

$pagename = "Discussão - ".$thread['title']."";
$body_id = "viewmode";
$page = FilterText($_GET['page']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_forum_posts WHERE threadid = '".$threadid."'");
$pages = ceil(($posts + 0) / 10);

if($page > $pages || $page < 1){
	$page = 1;
}


$group = mysql_query("SELECT forumid FROM cms_forum_posts WHERE threadid='".$_GET['thread']."'");
$info = mysql_fetch_assoc($group);
if(isset($info['forumid']) && is_numeric($info['forumid'])){

	$check = mysql_query("SELECT * FROM group_details WHERE id = '".$info['forumid']."' LIMIT 1") or die(mysql_error());
	$exists = mysql_num_rows($check);

	if($exists > 0){

		$groupid = $info['forumid'];

		$error = false;
		$groupdata = mysql_fetch_assoc($check);

		$pagename = $groupdata['name'];
		$ownerid = $groupdata['ownerid'];

		$members = mysql_evaluate("SELECT COUNT(*) FROM group_members WHERE id_group = '".$groupid."' AND is_pending = '0'") or die(mysql_error());

		$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' AND is_pending = '0' LIMIT 1") or die(mysql_error());
		$is_member = mysql_num_rows($check);

		if($is_member > 0 && $logged_in){

			$is_member = true;
			$my_membership = mysql_fetch_assoc($check);
			$member_rank = $my_membership['rank'];

		} else {
			$is_member = false;
		}
	} else {
		$error = true;
	}
} else {
	$error = true;
}

$key = 0;

if($_GET['sp'] == "JumpToLast"){
header("Location: viewthread.php?thread=".$threadid."&page=".$pages."#page-bottom");
exit;
}

switch($thread['type']){
	case 1: $topic_open = true; break;
	case 2: $topic_open = false; break;
	case 3: $topic_open = true; break;
	case 4: $topic_open = false; break;
}

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

if(isset($threadid) && $valid_thread == "true"){

echo"xD";
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////// FORUM ///////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if(isset($_GET['web-discussions-id']) && !$_GET['web-thread-id'] && is_numeric($_GET['web-discussions-id'])){

	$check = mysql_query("SELECT * FROM group_details WHERE id = '".FilterText($_GET['web-discussions-id'])."' LIMIT 1");
	if(mysql_num_rows($check) > 0){

		$groupid = FilterText($_GET['web-discussions-id']);

		$error = false;
		$groupdata = mysql_fetch_assoc($check);

		$pagename = $groupdata['name'];
		$ownerid = $groupdata['ownerid'];

		$members = mysql_evaluate("SELECT COUNT(*) FROM group_members WHERE id_group = '".$groupid."' AND is_pending = '0'");

		$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' AND is_pending = '0' LIMIT 1") or die(mysql_error());
		$is_member = mysql_num_rows($check);

		if($is_member > 0 && $logged_in){

			$is_member = true;
			$my_membership = mysql_fetch_assoc($check);
			$member_rank = $my_membership['rank'];

		} else {
			$is_member = false;
		}
	} else {
		$error = true;
	}
} else {
	$error = true;
}

if($error != true){
require_once('./templates/community_hsubheader.php');
require_once('./templates/community_header.php');

	mysql_query("UPDATE group_details SET views = views+'1' WHERE id='".$groupid."' LIMIT 1");

	$viewtools = "	<div class=\"myhabbo-view-tools\">\n";

	$page = FilterText($_GET['page']);

	$threads = mysql_evaluate("SELECT COUNT(*) FROM cms_forum_threads WHERE forumid='".$groupid."'");
	$pages = ceil(($threads + 0) / 10);

	if($page > $pages || $page < 1){
		$page = 1;
	}

	$key = 0;

	mysql_query("UPDATE group_details SET views = views+'1' WHERE id='".$groupid."' LIMIT 1");

?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="mypage-wrapper" class="cbb blue">
<div class="box-tabs-container box-tabs-left clearfix">

<?php if($member_rank > 1 && !$edit_mode){ ?>
	
<?php } ?>

<h2 class="page-owner">
<?php echo HoloText($groupdata['name']); ?>&nbsp;
<?php if($groupdata['type'] == "2"){ ?><img src='<?php echo $path; ?>/web-gallery/images/status_closed_big.gif' alt='Closed Group' title='Closed Group'><?php } ?>
<?php if($groupdata['type'] == "1"){ ?><img src='<?php echo $path; ?>/web-gallery/images/status_exclusive_big.gif' alt='Moderated Group' title='Moderated Group'><?php } ?></h2>
</h2>

<ul class="box-tabs">
	<li><a href="<?php echo $path; ?>/groups/<?php echo $groupid; ?>">Pagina Principal</a><span class="tab-spacer"></span></li>
	<li class="selected"><a href="<?php echo $path; ?>/groups/<?php echo $groupid; ?>/discussions">Fórum 
<?php if($groupdata['forum'] > 0){ ?>
	<img src="<?php echo $path; ?>/web-gallery/images/groups/privatekey.png">
<?php } ?>
</a><span class="tab-spacer"></span></li>

<?php 

$viewtools = "	<div class=\"myhabbo-view-tools\">\n";

if($logged_in && !$is_member && $groupdata['type'] !== "2" && $my_membership['is_pending'] !== "1"){

	$viewtools = $viewtools . "<a href=\"joingroup.php?groupId=".$groupid."\" id=\"join-group-button\">";

	if($groupdata['type'] == "0" || $groupdata['type'] == "3"){
		$viewtools = $viewtools . "Join";
	} else {
		$viewtools = $viewtools . "Pedir registro"; }
	$viewtools = $viewtools . "</a>";
	}

if($logged_in && $my_membership['is_current'] !== "1" && $is_member){
	$viewtools = $viewtools . "<a href=\"#\" id=\"select-favorite-button\">Escolha um favorito</a>\n";}
if($logged_in && $my_membership['is_current'] == "1" && $is_member){
	$viewtools = $viewtools . "<a href=\"#\" id=\"deselect-favorite-button\">Remover favorito
</a>";}
if($logged_in && $is_member && $my_id !== $ownerid){
	$viewtools = $viewtools . "<a href=\"leavegroup.php?groupId=".$groupid."\" id=\"leave-group-button\">Sair do grupo</a>\n";}

$viewtools = $viewtools . "	</div>\n"; 

?>

</ul>
</div>	

<div id="mypage-content">
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="content-1col">
<tr>
<td valign="top" style="width: 750px;" class="habboPage-col rightmost">
<div id="discussionbox">

<?php

if($groupdata['forum'] > 0) {
	$sql = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND is_pending <> '1' AND id_group='".$groupid."'") or die(mysql_error());
	$member = mysql_fetch_assoc($sql);
	if(mysql_num_rows($sql) > 0){

?>
<div id="group-topiclist-container">
<div class="topiclist-header clearfix">
		
<?php

$sql = mysql_query("SELECT * FROM group_details WHERE id='".$groupid."' LIMIT 1") or die(mysql_error());
$row = mysql_fetch_assoc($sql);

if($row['topics'] == 0){

?>
		
<input type="hidden" id="email-verfication-ok" value="1"/>
 
<?php if($logged_in){ ?>
	<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
	<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }

	}elseif($row['topics'] == 1) {
		$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND groupid = '".groupid."' AND is_pending <> '1' LIMIT 1");
		if(mysql_num_rows($check) > 0) { ?>
			<input type="hidden" id="email-verfication-ok" value="1"/>

			<?php if($logged_in){ ?>
			<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
			<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
		}
	}elseif($row['topics'] == 2) {
		$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group='".$groupid."' AND rank='2' AND is_pending <> '1' LIMIT 1");
		if(mysql_num_rows($check) > 0) { ?>
        		<input type="hidden" id="email-verfication-ok" value="1"/>
			<?php if($logged_in){ ?>
				<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
			<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
	}
}
?>
		
<div class="page-num-list">
Seiten:

<?php
	for ($i = 1; $i <= $pages; $i++){
		if($page == $i){
			echo $i . "\n";
		} else {
			echo "<a href=\"forum.php?page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
		}
	} 
?>

</div>
</div>

<table class="group-topiclist" border="0" cellpadding="0" cellspacing="0" id="group-topiclist-list">
	<tr class="topiclist-columncaption">
		<td class="topiclist-columncaption-topic">Assunto</td>
		<td class="topiclist-columncaption-lastpost">Ultima mensagem</td>
		<td class="topiclist-columncaption-replies">Respostas</td>
		<td class="topiclist-columncaption-views">Comentários</td>
	</tr>
	
<?php if($threads == 0){ ?>

<tr class="topiclist-row-1">
<td class="topiclist-rowtopic\" valign="top">
	Atualmente não há tópicos.
</td>
</tr>

<?php

}

$sql = mysql_query("SELECT * FROM cms_forum_threads WHERE type > 2 AND forumid='".$groupid."' ORDER BY unix DESC") or die(mysql_error());
$stickies = mysql_num_rows($sql);

$query_min = ($page * 10) - 10;
$query_max = 10;

$query_max = $query_max - $stickies;
$query_min = $query_min - $stickies;

if($query_min < 0){ // Page 1
$query_min = 0;
}

while($row = mysql_fetch_assoc($sql)){

$key++;

if(IsEven($key)){
	$x = "odd";
} else {
	$x = "even";
}

?>

<tr class="topiclist-row-<?php echo $x; ?>">
<td class="topiclist-rowtopic" valign="top">
<div class="topiclist-row-content">
<a class="topiclist-link icon icon-sticky" href="viewthread.php?thread=<?php echo $row['id']; ?>"><?php echo HoloText($row['title']); ?></a>

<?php if($row['type'] == 4){ ?>
&nbsp;<span class="topiclist-row-topicsticky"><img src="<?php echo $path; ?>/web-gallery/images/groups/status_closed.gif" title="Closed Thread" alt="Closed Thread"></span>
<?php }

	echo "&nbsp;(seiten ";

			$thread_pages = ceil(($row['posts'] + 1) / 10);

			for ($i = 1; $i <= $thread_pages; $i++){
				echo "<a href=\"viewthread.php?thread=" . $row['id'] . "&page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
			} 

            echo ")
			<br />
			<span><a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['author'] . "\">" . $row['author'] . "</a></span>";

			$date_bits = explode(" ", $row['date']);
			$date = $date_bits[0];
			$time = $date_bits[1];
			
			echo "&nbsp;<span class=\"latestpost\">" . $date . "</span>
			<span class=\"latestpost\">(" . $time . ")</span>
			</div>
		</td>
		<td class=\"topiclist-lastpost\" valign=\"top\">
		    <a class=\"lastpost-page-link\" href=\"viwthread.php?thread=" . $row['id'] . "&sp=JumpToLast\">";

			$date_bits = explode(" ", $row['lastpost_date']);
			$date = $date_bits[0];
			$time = $date_bits[1];

				echo "<span class=\"lastpost\">" . $date . "</span>
            <span class=\"lastpost\">(" . $time . ")</span></a><br />
			<span class=\"topiclist-row-writtenby\">von:</span> <a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['lastpost_author'] . "\">" . $row['lastpost_author'] . "</a>&nbsp;
		</td>
 		<td class=\"topiclist-replies\" valign=\"top\">" . $row['posts'] . "</td>
 		<td class=\"topiclist-views\" valign=\"top\">" . $row['views'] . "</td>
	</tr>";

}

$sql = mysql_query("SELECT * FROM cms_forum_threads WHERE type < 3 AND forumid='".$groupid."' ORDER BY unix DESC LIMIT ".$query_min.", ".$query_max."") or die(mysql_error());

while($row = mysql_fetch_assoc($sql)){

	$key++;

	if(IsEven($key)){
		$x = "odd";
	} else {
		$x = "even";
	}

	echo "<tr class=\"topiclist-row-" . $x . "\">
		<td class=\"topiclist-rowtopic\" valign=\"top\">
			<div class=\"topiclist-row-content\">
			<a class=\"topiclist-link \" href=\"".$path."/groups/".$groupid."/discussions/".$row['id']."\">".HoloText($row['title'])."</a>";

			if($row['type'] == 2){
			echo "&nbsp;<span class=\"topiclist-row-topicsticky\"><img src=\"".$path."/web-gallery/images/groups/status_closed.gif\" title=\"Closed Thread\" alt=\"Closed Thread\"></span>";
			}

			echo "&nbsp;(Seiten ";

			$thread_pages = ceil(($row['posts'] + 1) / 10);

			for ($i = 1; $i <= $thread_pages; $i++){
				echo "<a href=\"viewthread.php?thread=" . $row['id'] . "&page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
			} 

            echo ")
			<br />
			<span><a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['author'] . "\">" . $row['author'] . "</a></span>";

			$date_bits = explode(" ", $row['date']);
			$date = $date_bits[0];
			$time = $date_bits[1];
			
				echo "&nbsp;<span class=\"latestpost\">" . $date . "</span>
			<span class=\"latestpost\">(" . $time . ")</span>
			</div>
		</td>
		<td class=\"topiclist-lastpost\" valign=\"top\">
		    <a class=\"lastpost-page-link\" href=\"viewthread.php?thread=" . $row['id'] . "&sp=JumpToLast\">";

			$date_bits = explode(" ", $row['lastpost_date']);
			$date = $date_bits[0];
			$time = $date_bits[1];

				echo "<span class=\"lastpost\">" . $date . "</span>
            <span class=\"lastpost\">(" . $time . ")</span></a><br />
			<span class=\"topiclist-row-writtenby\">von:</span> <a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['lastpost_author'] . "\">" . $row['lastpost_author'] . "</a>&nbsp;
		</td>
 		<td class=\"topiclist-replies\" valign=\"top\">" . $row['posts'] . "</td>
 		<td class=\"topiclist-views\" valign=\"top\">" . $row['views'] . "</td>
	</tr>";

}

?>	

</table>
<div class="topiclist-footer clearfix">

<?php

$sql = mysql_query("SELECT * FROM group_details WHERE id='".$groupid."' LIMIT 1");
$row = mysql_fetch_assoc($sql);
if($row['topics'] == 0) {

?>
		
<input type="hidden" id="email-verfication-ok" value="1"/>
<?php if($logged_in){ ?>
	<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
	<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
}elseif($row['topics'] == 1) {
	$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' LIMIT 1");
	if(mysql_num_rows($check) > 0) { ?>
        	<input type="hidden" id="email-verfication-ok" value="1"/>
        	<?php if($logged_in){ ?>
			<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
		<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
	}
}elseif($row['topics'] == 2) {
	$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$id_group."' AND rank='2' LIMIT 1");
	if(mysql_num_rows($check) > 0) { ?>
        	<input type="hidden" id="email-verfication-ok" value="1"/>
        	<?php if($logged_in){ ?>
			<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
		<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
	}
}
?>

<div class="page-num-list">
Seiten:
<?php
	for ($i = 1; $i <= $pages; $i++){
		if($page == $i){
			echo $i . "\n";
		} else {
			echo "<a href=\"forum.php?page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
		}
	}
?>

</div>
<?php }else{ ?>
	<h1>Oops...</h1>
	<p>Você deve ser um membro deste grupo para postar aqui o que pode <br /></p>
<?php } }else{ ?>
<div id="group-topiclist-container">
<div class="topiclist-header clearfix">
        <input type="hidden" id="email-verfication-ok" value="1"/>
		
<?php

$sql = mysql_query("SELECT * FROM group_details WHERE id='".groupid."' LIMIT 1");
$row = mysql_fetch_assoc($sql);

if($row['topics'] == 0) {

?>
		
<input type="hidden" id="email-verfication-ok" value="1"/>
<?php if($logged_in){ ?>
	<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }

}elseif($row['topics'] == 1) {
	$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' LIMIT 1");
	if(mysql_num_rows($check) > 0) { ?>
        	<input type="hidden" id="email-verfication-ok" value="1"/>
        	<?php if($logged_in){ ?>
			<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
		<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
	}
}elseif($row['topics'] == 2) {
	$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' AND rank='2' LIMIT 1");
	if(mysql_num_rows($check) > 0) { ?>
        	<input type="hidden" id="email-verfication-ok" value="1"/>
        	<?php if($logged_in){ ?>
			<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
		<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
	}
} ?>
    <div class="page-num-list">
    Ver pagina:
<?php
	for ($i = 1; $i <= $pages; $i++){
		if($page == $i){
			echo $i . "\n";
		} else {
			echo "<a href=\"forum.php?page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
		}
	} 
?>
    </div>
</div>
<table class="group-topiclist" border="0" cellpadding="0" cellspacing="0" id="group-topiclist-list">
	<tr class="topiclist-columncaption">
		<td class="topiclist-columncaption-topic">Assunto</td>
		<td class="topiclist-columncaption-lastpost">Ultima mensagem</td>
		<td class="topiclist-columncaption-replies">Respostas</td>
		<td class="topiclist-columncaption-views">Comentarios</td>
	</tr>
	
<?php if($threads == 0){ ?>
<tr class="topiclist-row-1">
<td class="topiclist-rowtopic" valign="top">
	Atualmente não há tópico.
</td>
</tr>

<?php }

$sql = mysql_query("SELECT * FROM cms_forum_threads WHERE type > 2 AND forumid='".$groupid."' ORDER BY unix DESC") or die(mysql_error());
$stickies = mysql_num_rows($sql);

$query_min = ($page * 10) - 10;
$query_max = 10;

$query_max = $query_max - $stickies;
$query_min = $query_min - $stickies;

if($query_min < 0){ // Page 1
$query_min = 0;
}

while($row = mysql_fetch_assoc($sql)){

	$key++;

	if(IsEven($key)){
		$x = "odd";
	} else {
		$x = "even";
	}

	echo "<tr class=\"topiclist-row-" . $x . "\">
		<td class=\"topiclist-rowtopic\" valign=\"top\">
			<div class=\"topiclist-row-content\">
			<a class=\"topiclist-link icon icon-sticky\" href=\"viewthread.php?thread=".$row['id']."\">".HoloText($row['title'])."</a>";

			if($row['type'] == 4){
			echo "&nbsp;<span class=\"topiclist-row-topicsticky\"><img src=\"".$path."/web-gallery/images/groups/status_closed.gif\" title=\"Closed Thread\" alt=\"Closed Thread\"></span>";
			}

			echo "&nbsp;(Seiten ";

			$thread_pages = ceil(($row['posts'] + 1) / 10);

			for ($i = 1; $i <= $thread_pages; $i++){
				echo "<a href=\"viewthread.php?thread=" . $row['id'] . "&page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
			} 

            echo ")
			<br />
			<span><a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['author'] . "\">" . $row['author'] . "</a></span>";

			$date_bits = explode(" ", $row['date']);
			$date = $date_bits[0];
			$time = $date_bits[1];
			
				echo "&nbsp;<span class=\"latestpost\">" . $date . "</span>
			<span class=\"latestpost\">(" . $time . ")</span>
			</div>
		</td>
		<td class=\"topiclist-lastpost\" valign=\"top\">
		    <a class=\"lastpost-page-link\" href=\"viewthread.php?thread=" . $row['id'] . "&sp=JumpToLast\">";

			$date_bits = explode(" ", $row['lastpost_date']);
			$date = $date_bits[0];
			$time = $date_bits[1];

				echo "<span class=\"lastpost\">" . $date . "</span>
            <span class=\"lastpost\">(" . $time . ")</span></a><br />
			<span class=\"topiclist-row-writtenby\">von:</span> <a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['lastpost_author'] . "\">" . $row['lastpost_author'] . "</a>&nbsp;
		</td>
 		<td class=\"topiclist-replies\" valign=\"top\">" . $row['posts'] . "</td>
 		<td class=\"topiclist-views\" valign=\"top\">" . $row['views'] . "</td>
	</tr>";

}

$sql = mysql_query("SELECT * FROM cms_forum_threads WHERE type < 3 AND forumid='".$groupid."' ORDER BY unix DESC LIMIT ".$query_min.", ".$query_max."") or die(mysql_error());

while($row = mysql_fetch_assoc($sql)){

$key++;

if(IsEven($key)){
	$x = "odd";
} else {
	$x = "even";
}

?>

<tr class="topiclist-row-<?php echo $x; ?>">
<td class="topiclist-rowtopic" valign="top">
<div class="topiclist-row-content">
<a class="topiclist-link " href="viewthread.php?thread=<?php echo $row['id']; ?>"><?php echo HoloText($row['title']); ?></a>

<?php
			if($row['type'] == 2){
			echo "&nbsp;<span class=\"topiclist-row-topicsticky\"><img src=\"".$path."/web-gallery/images/groups/status_closed.gif\" title=\"Closed Thread\" alt=\"Closed Thread\"></span>";
			}

			echo "&nbsp;(Seiten ";

			$thread_pages = ceil(($row['posts'] + 1) / 10);

			for ($i = 1; $i <= $thread_pages; $i++){
				echo "<a href=\"viewthread.php?thread=" . $row['id'] . "&page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
			} 

            echo ")
			<br />
			<span><a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['author'] . "\">" . $row['author'] . "</a></span>";

			$date_bits = explode(" ", $row['date']);
			$date = $date_bits[0];
			$time = $date_bits[1];
			
				echo "&nbsp;<span class=\"latestpost\">" . $date . "</span>
			<span class=\"latestpost\">(" . $time . ")</span>
			</div>
		</td>
		<td class=\"topiclist-lastpost\" valign=\"top\">
		    <a class=\"lastpost-page-link\" href=\"viewthread.php?thread=" . $row['id'] . "&sp=JumpToLast\">";

			$date_bits = explode(" ", $row['lastpost_date']);
			$date = $date_bits[0];
			$time = $date_bits[1];

				echo "<span class=\"lastpost\">" . $date . "</span>
            <span class=\"lastpost\">(" . $time . ")</span></a><br />
			<span class=\"topiclist-row-writtenby\">von:</span> <a class=\"topiclist-row-openername\" href=\"".$path."/home/" . $row['lastpost_author'] . "\">" . $row['lastpost_author'] . "</a>&nbsp;
		</td>
 		<td class=\"topiclist-replies\" valign=\"top\">" . $row['posts'] . "</td>
 		<td class=\"topiclist-views\" valign=\"top\">" . $row['views'] . "</td>
	</tr>";

}

?>	

	</table>
<div class="topiclist-footer clearfix">

<?php
$sql = mysql_query("SELECT * FROM group_details WHERE id='".$groupid."' LIMIT 1");
$row = mysql_fetch_assoc($sql);
if($row['topics'] == 0) {

?>
		
<input type="hidden" id="email-verfication-ok" value="1"/>
<?php if($logged_in){ ?>
	<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
	<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
}elseif($row['topics'] == 1) {
	$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' LIMIT 1");
	if(mysql_num_rows($check) > 0) { ?>
        	<input type="hidden" id="email-verfication-ok" value="1"/>
        	<?php if($logged_in){ ?>
			<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
		<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
	}
}elseif($row['topics'] == 2) {
	$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$id_group."' AND rank='2' LIMIT 1");
	if(mysql_num_rows($check) > 0) { ?>
        	<input type="hidden" id="email-verfication-ok" value="1"/>
        	<?php if($logged_in){ ?>
			<a href="#" id="newtopic-upper" class="new-button verify-email newtopic-icon" style="float:left"><b><span></span>Novo tópico</b><i></i></a>
		<?php } else { echo "Você precisa fazer login para escrever um novo segmento pode."; }
	}
}
?>

<div class="page-num-list">
Ver pagina:
<?php
	for ($i = 1; $i <= $pages; $i++){
		if($page == $i){
			echo $i . "\n";
		} else {
			echo "<a href=\"forum.php?page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
		}
	}
?>
    </div>
<?php }
?>
</div>
</div>

<script type="text/javascript" language="JavaScript">
L10N.put("myhabbo.discussion.error.topic_name_empty", "Title darf nicht leer sein!");
Discussions.initialize("<?php echo $groupid; ?>", "forum.php", null);
</script>
                    </div>
					
                </td>
                <td style="width: 4px;"></td>
                <td valign="top" style="width: 164px;">
    <div class="habblet ">
    
    </div>
                </td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
	Event.observe(window, "load", observeAnim);
	document.observe("dom:loaded", initDraggableDialogs);
</script>
    </div>
</div>

</div>

<div class="cbb topdialog black" id="dialog-group-settings">
	
	<div class="box-tabs-container">
<ul class="box-tabs">
	<li class="selected" id="group-settings-link-group"><a href="#">Definir grupo</a><span class="tab-spacer"></span></li>
	<li id="group-settings-link-forum"><a href="#">Configurações Forum</a><span class="tab-spacer"></span></li>
	<li id="group-settings-link-room"><a href="#">Omissão</a><span class="tab-spacer"></span></li>
</ul>
</div>
	<a class="topdialog-exit" href="#" id="dialog-group-settings-exit">X</a>
	<div class="topdialog-body" id="dialog-group-settings-body">
<p style="text-align:center"><img src="<?php echo $path; ?>/web-gallery/images/progress_bubbles.gif" alt="" width="29" height="6" /></p>
	</div>
</div>	

<script language="JavaScript" type="text/javascript">
Event.observe("dialog-group-settings-exit", "click", function(e) {
    Event.stop(e);
    closeGroupSettings();
}, false);
</script><div class="cbb topdialog" id="postentry-verifyemail-dialog">
	<h2 class="title dialog-handle">Bevestig endereços de e-mail</h2>
	
	<a class="topdialog-exit" href="#" id="postentry-verifyemail-dialog-exit">X</a>
	<div class="topdialog-body" id="postentry-verifyemail-dialog-body">
	<p>Você deve preencher seu endereço de e-mail antes que você pode postar um comentário.</p>
	<p><a href="/profile?tab=3">Digite seu endereço de e-mail</a></p>
	<p class="clearfix">
		<a href="#" id="postentry-verifyemail-ok" class="new-button"><b>OK</b><i></i></a>
	</p>
	</div>
</div>	
					
<script type="text/javascript">
HabboView.run();
</script>

<?php require_once('./templates/community_footer.php'); ?>

</body>
</html>
<?php

} else {
$pagename = "Página não encontrada";
require_once('./templates/community_subheader.php');
?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix red "><h2 class="title">Grupo não encontrado!</h2>
<div id="notfound-content" class="box-content">
    <p class="error-text">A página solicitada não pôde ser encontrado.</p> <img id="error-image" src="./web-gallery/v2/images/error.gif" />
    <p class="error-text">Por favor, clique <a href="javascript:history.back()">hier</a> para retornar à página anterior.</p>
    <p class="error-text"><b>Olhando para o Grupo</b></p>
    <?php if(isset($searchString)){ echo "<p class=\"error-text\">Não houve grupo chamado <strong>'".$searchString."'.</strong> ser encontrados.</p>"; } ?>
    <p class="error-text">
	<form method='post'>
		Nome do grupo:<br />
		<input type='text' name='searchString' maxlength='25' size='25' value='<?php echo $_POST['searchString']; ?>'>
		<input type='submit' class='submit' value='Suchen'>
	</form>
    </p>
</div>


					</div>
				</div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>
<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix green ">
	<h2 class="title">Você estava procurando ...</h2>
<div id="notfound-looking-for" class="box-content">

<p><b>Grupo de Amigos ou página principal?</b><br/>
    Verifica se a página aparece em <a href="<?php echo $path; ?>/community">Comunidade</a>.</p>

    <p><b>Salas chocante?</b><br/>
    Confira o <a href="<?php echo $path; ?>/community">salas recomendadas</a>.</p>

    <p><b>O que outros membros estão ligados? </b><br/>
    Confira o Top <a href="<?php echo $path; ?>/tag">Tags</a>.</p>

     <p><b>Como posso comprar créditos?</b><br/>
    Confira página <a href="<?php echo $path; ?>/credits">Creditos</a>.</p>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<?php require_once('./templates/community_footer.php'); } ?>