<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');



?>
<?php

$threadid = FilterText($_GET['thread']);

if(!empty($threadid) && is_numeric($threadid)){
	$check = mysql_query("SELECT * FROM cms_forum_threads WHERE id = '".$threadid."' LIMIT 1") or die(mysql_error());
	$exists = mysql_num_rows($check);
	if($exists > 0){
		$thread = mysql_fetch_assoc($check);
		$valid_thread = true;
		mysql_query("UPDATE cms_forum_threads SET views = views + 1 WHERE id = '".$threadid."' LIMIT 1") or die(mysql_error());

	} else {
		    echo "Forum id nicht  mehr Online";
		//	header("Location: me.php");
		exit;
	}
} else {
 echo "Comentario no encontrado o borrado";
 //header("Location: me.php");
	exit;
}
$pagename = "Foro de Discucion " . $thread['title'];
$pageid = "forum";
$body_id = "viewmode";
$page = FilterText (isset($_GET['page'])) ? (int)$_GET['page'] : 0;

$posts = mysql_evaluate("SELECT COUNT(*) FROM cms_forum_posts WHERE threadid = '".$threadid."'");
$pages = ceil(($posts + 0) / 10);

if($page > $pages || $page < 1){
	$page = 1;
}


$group = mysql_query("SELECT forumid FROM cms_forum_posts WHERE threadid='".$_GET['thread']."'");
$info = mysql_fetch_assoc($group);
if(isset($info['forumid']) && is_numeric($info['forumid'])){

	$check = mysql_query("SELECT * FROM group_details WHERE id = '".$info['forumid']."' LIMIT 1");
	$exists = mysql_num_rows($check);

	if($exists > 0){

		$groupid = $info['forumid'];

		$error = false;
		$groupdata = mysql_fetch_assoc($check);

		$pagename = $groupdata['name'];
		$ownerid = $groupdata['ownerid'];

		$members = mysql_evaluate("SELECT COUNT(*) FROM group_members WHERE id_group = '".$groupid."' AND is_pending = '0'");

		$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' AND is_pending = '0' LIMIT 1");
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


//if($posts < 1){
//	echo "<b>Internal Error</b><br />No posts found for this thread. Please double-check / repair your cms_forum_posts database table.";
//	exit;
//}

$key = 0;

if(!empty($_GET["sp"]) && $_GET['sp'] == "JumpToLast"){
header("Location: viewthread.php?thread=".$threadid."&page=".$pages."#page-bottom");
exit;
}

switch($thread['type']){
	case 1: $topic_open = true; break;
	case 2: $topic_open = false; break;
	case 3: $topic_open = true; break;
	case 4: $topic_open = false; break;
}

if(!isset($topic_open)){
	exit;
}

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
	<div id="content" style="position: relative" class="clearfix">
    <div id="mypage-wrapper" class="cbb blue">
<div class="box-tabs-container box-tabs-left clearfix">
	<?php
	$sql = mysql_query("SELECT forumid FROM cms_forum_posts WHERE threadid='".$_GET['thread']."'");
	$row = mysql_fetch_assoc($sql);
	if($row['forumid'] != 0) {
	 if($member_rank > 1 && !$edit_mode){ ?><a href="groups/<?php echo $groupid; ?>&do=edit" class="new-button dark-button edit-icon" style="float:left"><b><span></span>Edit</b><i></i></a><?php } ?>
	<?php if(!$edit_mode){ echo $viewtools; }
	} ?>
	<div class="myhabbo-view-tools">
	</div>
    <h2 class="page-owner">
	<?php
	$sql = mysql_query("SELECT forumid FROM cms_forum_posts WHERE threadid='".$_GET['thread']."'");
	$row = mysql_fetch_assoc($sql);
	if($row['forumid'] != 0) {
 echo HoloText($groupdata['name']); ?>&nbsp;<?php
 if($groupdata['type'] == "2"){ ?><img src='./web-gallery/images/status_closed_big.gif' alt='Closed Group' title='Gruppe Chiuso'><?php } ?>
<?php if($groupdata['type'] == "1"){ ?><img src='./web-gallery/images/status_exclusive_big.gif' alt='Moderated Group' title='Gruppe Moderatori'><?php } ?></h2>
<?php }else{ ?>
    	<li><a href="groupsdata_forum.php?web-discussions-id=<?php echo $groupid; ?>">Foro de discusion<?php if($groupdata['pane'] > 0) { ?><?php } ?></a><span class="tab-spacer"></span></li>
	<?php } ?>
    </h2>
    <ul class="box-tabs">
		<?php if($row['forumid'] != 0) { ?>
		<li><a href="javascript:history.back();">Discuciones</a></span></li>
<?php	} ?>
        <li class="selected"><a href="javascript:location.reload()">Foro de discusion</a><span class="tab-spacer"></span></li>
    </ul>
</div>
	<div id="mypage-content">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="content-1col">
            <tr>
                <td valign="top" style="width: 750px;" class="habboPage-col rightmost">
                    <div id="discussionbox">
<div id="group-postlist-container">

    <div class="postlist-header clearfix">

				<?php
				$sql = mysql_query("SELECT * FROM group_details WHERE id='".$row['forumid']."' LIMIT 1");
				if (mysql_errno()) die ("MySQL-Error: " . mysql_error());
				$row = mysql_fetch_assoc($sql);
				$asdf = "";
				$zxcv = "";
				
				if($row['topics'] == 0) {
					$asdf = "<a href=\"viewthread.php?thread=".$topicid."\" id=\"create-post-message\" class=\"create-post-link verify-email\">Responder</a>";
					$zxcv = "<a href=\"#\" class=\"quote-post-link verify-email\" id=\"quote-post-".$row['id']."-message\">Quote</a>";
					
				}elseif($row['topics'] == 1) {
					$check = mysql_query("SELECT * FROM group_members WHERE id_user='".$my_id."' AND id_group='".$_GET['id']."' AND is_pending <> '1' LIMIT 1");
					if(mysql_num_rows($check) > 0) { 
						$asdf = "<a href=\"viewthread.php?thread=".$topicid."\" id=\"create-post-message\" class=\"create-post-link verify-email\">Responder</a>";
						$zxcv = "<a href=\"#\" class=\"quote-post-link verify-email\" id=\"quote-post-".$row['id']."-message\">Quote</a>";
					}
					
				}elseif($row['topics'] == 2) {
					$check = mysql_query("SELECT * FROM group_members WHERE id_user='".$my_id."' AND id_group='".$_GET['id']."' AND rank='2' AND is_pending <> '1' LIMIT 1");
					if(mysql_num_rows($check) > 0) { 
						$asdf = "<a href=\"viewthread.php?thread=".$topicid."\" id=\"create-post-message\" class=\"create-post-link verify-email\">Responder</a>";
						$zxcv = "<a href=\"#\" class=\"quote-post-link verify-email\" id=\"quote-post-".$row['id']."-message\">Quote</a>";
					}
				}
				?>
                <?php if($topic_open && $logged_in){ ?><?php echo $asdf ?><?php } elseif($logged_in) { ?><span class="topic-closed"><img src="./web-gallery/images/groups/status_closed.gif" title="Closed Thread"> Tema Cerrado</span><?php } ?>
                <input type="hidden" id="email-verfication-ok" value="1"/>
				<?php
				$hid = mysql_query("SELECT * FROM group_members WHERE id_user='".$my_id."' AND id_group='".$my_id_group."' AND rank='1' LIMIT 1");
				if (mysql_errno()) die ("MySQL-Error: " . mysql_error());
				if(mysql_num_rows($hid) > 0) { ?><a href="viewthread.php?thread=".$topicid."" id="edit-topic-settings" class="edit-topic-settings-link">Moderacion;</a>
                <input type="hidden" id="settings_dialog_header" value="Herramientas"/>
				<?php
				}elseif($user_rank > 5){ ?><a href="viewthread.php?thread=".$topicid."" id="edit-topic-settings" class="edit-topic-settings-link">Moderacion &raquo;</a>
                <input type="hidden" id="settings_dialog_header" value="Herramientas"/><?php } ?>
        <div class="page-num-list">
	<input type="hidden" id="current-page" value="<?php echo $page; ?>"/>
    Veces Vista:
<?php
	for ($i = 1; $i <= $pages; $i++){
		if($page == $i){
			echo $i . "\n";
		} else {
			echo "<a href=\"viewthread.php?thread=".$threadid."&page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
		}
	} 
?>
    </div>
</div>

<table border="0" cellpadding="0" cellspacing="0" width="100%" class="group-postlist-list" id="group-postlist-list">


<?php 

// Post view handler & echoer

$query_min = ($page * 10) - 10;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_em = mysql_query("SELECT * FROM cms_forum_posts WHERE threadid = '".$threadid."' ORDER BY id ASC LIMIT ".$query_min.", 10") or die(mysql_error());
$dynamic_id = 0;

while($row = mysql_fetch_assoc($get_em)){

	$dynamic_id++;

	if(IsEven($dynamic_id)){
		$oddeven = "odd";
	} else {
		$oddeven = "even";
	}

	$usersql = mysql_query("SELECT * FROM users WHERE username = '".$row['author']."' LIMIT 1");
	$userdata = mysql_fetch_assoc($usersql);

	$userid = $userdata['id'];
	$useridon = $userdata['online'];

//
	$get_cc = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' and badge_slot = '1'") or die(mysql_error());
	$get_cc_badge = mysql_fetch_assoc($get_cc);

	$groupbadge = mysql_query("SELECT * FROM group_members WHERE id_user = '".$userdata['id']."' AND is_current = '1'");
	$badge = mysql_fetch_assoc($groupbadge);
	$groupbadge_check = mysql_num_rows($groupbadge);

		$badgedetails_a = mysql_query("SELECT * FROM group_details WHERE id = '".$badge['id_group']."'");
		$badgedetails = mysql_fetch_assoc($badgedetails_a);



	echo "<tr class=\"post-list-index-".$oddeven."\">
	<a id='post-".$row['id']."'>
	<td class=\"post-list-row-container\">
		<a href=\"home/".$userdata['username']."\" class=\"post-list-creator-link post-list-creator-info\">".$userdata['username']."</a>&nbsp;\n";
            if($useridon){ echo "<img alt=\"Online\" src=\"./web-gallery/images/myhabbo/habbo_online_anim.gif\" />"; } else { echo "<img alt=\"Offline\" src=\"./web-gallery/images/myhabbo/habbo_offline.gif\" />"; }
		echo "<div class=\"post-list-posts post-list-creator-info\">Mensajes: ".$userdata['postcount']."</div>
		<div class=\"clearfix\">
            <div class=\"post-list-creator-avatar\"><img src=\"http://www.habbo.es/habbo-imaging/avatarimage?figure=".$userdata['look']."&size=b&direction=2&head_direction=2&gesture=sml\" alt=\"".$userdata['username']."\" /></div><div class=\"post-list-group-badge\">";

	if($groupbadge_check > 0){
		echo"<a href='".$path."/groups/".$badgedetails['id']."'><img src='".$path."/habbo-imaging/badge.php?badge=".$badgedetails['badge'].".gif'></a>";
	}
		echo "</div>

		<div class=\"post-list-avatar-badge\">";
	if(mysql_num_rows($get_cc) > 0){
		echo "<img src=\"".$cimagesurl.$badgesurl.$get_cc_badge['badge_id'].".gif\"/></a>";
	}
		echo "</div>
        </div>
        <div class=\"post-list-motto post-list-creator-info\">".trim(HoloText($userdata['motto']))."</div>
	</td>
	<td class=\"post-list-message\" valign=\"top\" colspan=\"2\">
                <a href=\"#\" class=\"quote-post-link verify-email\" id=\"quote-post-".$row['id']."-message\">Citar</a>";
			if($user_rank > 5 || $my_id == $userdata['id']){
	                echo "<a href=\"#\" class=\"edit-post-link verify-email\" id=\"edit-post-".$row['id']."-message\">Editar</a>";
			}
        echo "<span class=\"post-list-message-header\">"; 
	if($dynamic_id !== 1 || $page > 1){
		echo "RE: ";
 	}
	echo HoloText($thread['title'])."</span><br />
        <span class=\"post-list-message-time\">".$row['date']."</span>
        <div class=\"post-list-report-element\">";
			if($user_rank > 5 || $my_id == $userdata['id'] && $logged_in){
                		echo "<a href=\"viewthread.php?thread=".$row['id']."\" id=\"delete-post-".$row['id']."\" class=\"delete-button delete-post\"></a>";
			}
			if($my_id !== $userdata['id'] && $logged_in){
				echo "        <div class=\"post-list-report-element\">\n                <a href=\"./iot/go.php?do=report&post=".$row['id']."&page=".$page."\" class=\"create-report-button\" title=\"Report this post\" target=\"habbohelp\" onclick=\"openOrFocusHelp(this); return false\"></a>\n        </div>";
			}
echo "        </div>";

		if(!empty($row['edit_date']) && !empty($row['edit_author'])){
		echo "\n<br /><br /><font size='1'><strong>Ultima Edicion ".$row['edit_date']." by ".$row['edit_author']."</strong></font>";
		}

echo "        <div class=\"post-list-content-element\">";

            echo bbcode_format(trim(nl2br(HoloText($row['message']))))."
                <input type=\"hidden\" id=\"".$row['id']."-message\" value=\"".HoloText($row['message'])."\" />
        </div>
        <div>
        </div>
	</td>
</tr>";
}


?>



<tr id="new-post-entry-message" style="display:none;">
	<td class="new-post-entry-label"><div class="new-post-entry-label" id="new-post-entry-label">Text:</div></td>
	<td colspan="2">
		<table border="0" cellpadding="0" cellspacing="0" style="margin: 5px; width: 98%;">
		<tr>
		<td>
		<input type="hidden" id="edit-type"/>
		<input type="hidden" id="post-id"/>
        <a href="#" class="preview-post-link" id="post-form-preview">Previsualizar &raquo;</a>
        <input type="hidden" id="spam-message" value="Spam-alarm!"/>
		<textarea id="post-message" class="new-post-entry-message" rows="5" name="message" ></textarea>
    <script type="text/javascript">
        bbcodeToolbar = new Control.TextArea.ToolBar.BBCode("post-message");
        bbcodeToolbar.toolbar.toolbar.id = "bbcode_toolbar";
        var colors = { "red" : ["#d80000", "Red"],
            "orange" : ["#fe6301", "Orange"],
            "yellow" : ["#ffce00", "Yellow"],
            "green" : ["#6cc800", "Green"],
            "cyan" : ["#00c6c4", "Cyan"],
            "blue" : ["#0070d7", "Blue"],
            "gray" : ["#828282", "Grey"],
            "black" : ["#000000", "Black"]
        };
        bbcodeToolbar.addColorSelect("Color", colors, false);
    </script>
	    <br /><br />
        <a id="post-form-cancel" class="new-button red-button cancel-icon" href="#"><b><span></span>Cancelar</b><i></i></a>
        <a id="post-form-save" class="new-button green-button save-icon" href="#"><b><span></span>Aceptar</b><i></i></a>
        </td>
        </tr>
        </table>
	</td>
</tr></table>
<div id="new-post-preview" style="display:none;">
</div>

    <div class="postlist-footer clearfix">
		<?php if($topic_open && $logged_in){ ?><?php echo $asdf ?></a><?php } elseif($logged_in){ ?><span class="topic-closed"><img src="./web-gallery/images/groups/status_closed.gif" title="Closed Thread"> Tema Cerrado</span><?php } else { echo "</a>Devi effettuare l'accesso per visualizzare/postare I post del forum."; } ?>
    </a><div class="page-num-list">
   Veces Vista:
<?php
	for ($i = 1; $i <= $pages; $i++){
		if($page == $i){
			echo $i . "\n";
		} else {
			echo "<a href=\"viewthread.php?thread=".$threadid."&page=" . $i . "\" class=\"topiclist-page-link\">" . $i . "</a>\n";
		}
	} 
?>
    </div>
</div>
</div>

<a id='page-bottom'>

<script type="text/javascript" language="JavaScript">
L10N.put("myhabbo.discussion.error.topic_name_empty", "Topic title may not be empty");
Discussions.initialize("DiscussionBoard", "forum.php", "<?php echo $threadid; ?>");
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
<div id="footer">
<?php require_once('./templates/community_footer.php'); ?>
</div></div>

</div>

	 
<div id="group-tools" class="bottom-bubble">
	<div class="bottom-bubble-t"><div></div></div>
	<div class="bottom-bubble-c">
<h3>Cambia Gruppe</h3>

<ul>
	<li><a href="/groups/actions/startEditingSession/55918" id="group-tools-style">roflcopter</a></li>
	<li><a href="#" id="group-tools-settings">e</a></li>
	<li><a href="#" id="group-tools-badge">Spegni</a></li>
	<li><a href="#" id="group-tools-members">Limite</a></li>
</ul>

	</div>
	<div class="bottom-bubble-b"><div></div></div>
</div>

<div class="cbb topdialog black" id="dialog-group-settings">
	
	<div class="box-tabs-container">
<ul class="box-tabs">
	<li class="selected" id="group-settings-link-group"><a href="#">Impostazioni Gruppe</a><span class="tab-spacer"></span></li>
	<li id="group-settings-link-forum"><a href="#">Forum Impostazioni</a><span class="tab-spacer"></span></li>
	<li id="group-settings-link-room"><a href="#">Impostazioni Stanze</a><span class="tab-spacer"></span></li>
</ul>
</div>

	<a class="topdialog-exit" href="#" id="dialog-group-settings-exit">X</a>
	<div class="topdialog-body" id="dialog-group-settings-body">
<p style="text-align:center"><img src="./web-gallery/images/progress_bubbles.gif" alt="" width="29" height="6" /></p>
	</div>
</div>	

<script language="JavaScript" type="text/javascript">
Event.observe("dialog-group-settings-exit", "click", function(e) {
    Event.stop(e);
    closeGroupSettings();
}, false);
</script><div class="cbb topdialog" id="postentry-verifyemail-dialog">
	<h2 class="title dialog-handle">Verifica</h2>
	
	<a class="topdialog-exit" href="#" id="postentry-verifyemail-dialog-exit">X</a>
	<div class="topdialog-body" id="postentry-verifyemail-dialog-body">
	<p>Si prega di verificare la vostra email prima di postare.</p>
	<p class="clearfix">
		<a href="#" id="postentry-verifyemail-ok" class="new-button"><b>Cancelar</b><i></i></a>
	</p>
	</div>
</div>	

<div class="cbb topdialog" id="postentry-delete-dialog">
	<h2 class="title dialog-handle">Eliminar Post</h2>

	<a class="topdialog-exit" href="" id="postentry-delete-dialog-exit">X</a>
	<div class="topdialog-body" id="postentry-delete-dialog-body">
<form method="post" id="postentry-delete-form">
	<input type="hidden" name="entryId" id="postentry-delete-id" value="" />
	<p>Usted está a punto de eliminar el Tema Completo. ¿Está seguro?</p>
	<p class="clearfix">
		<a href="" id="postentry-delete-cancel" class="new-button"><b>Cancelar</b><i></i></a>
		<a href="" id="postentry-delete" class="new-button"><b>Aceptar</b><i></i></a>
	</p>
</form>
	</div>
</div>	
					
<script type="text/javascript">
HabboView.run();
</script>

</body>
</html>