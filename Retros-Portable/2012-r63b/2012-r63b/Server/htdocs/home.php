<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

if(isset($_GET['web-home-tag']) || isset($_GET['web-home-name']) || isset($_POST['web-home-name'])){
	if(isset($_GET['web-home-tag'])){
	$searchname = FilterText($_GET['web-home-tag']);
	} else if(isset($_GET['web-home-name'])){
	$searchname = FilterText($_GET['web-home-name']);
	} else if(isset($_POST['web-home-name'])){
	$searchname = FilterText($_POST['web-home-sname']);
	} else {
	$error = true;
	}

	$user_sql = mysql_query("SELECT * FROM users WHERE username = '".$searchname."' LIMIT 1") or die(mysql_error());
	$user_exists = mysql_num_rows($user_sql);

	if($user_exists == "1"){
	$error = false;
	$user_row = mysql_fetch_assoc($user_sql);

	$pagename = "".$user_row['username']."";

	} else { $error = true; }
	
	} else if(isset($_GET['tagid']) || isset($_GET['id']) || isset($_POST['id'])){
	if(isset($_GET['tagid'])){
	$searchid = FilterText($_GET['tagid']);
	} else if(isset($_GET['id'])){
	$searchid = FilterText($_GET['id']);
	} else if(isset($_POST['id'])){
	$searchid = FilterText($_POST['id']);
	} else {
	$error = true;
	}

	$user_sql = mysql_query("SELECT * FROM users WHERE id = '".$searchid."' LIMIT 1") or die(mysql_error());
	$user_exists = mysql_num_rows($user_sql);

	if($user_exists == "1"){
	$error = false;
	$user_row = mysql_fetch_assoc($user_sql);
	$pagename = "Home - ".$user_row['username']."";
	} else {
	$error = true;
	}

	} else { $error = true; }

	if(isset($_GET['do']) && FilterText($_GET['do']) == "edit" && $logged_in){
	if($user_row['username'] == $name){
	$edit_mode = true;
	}else{
	header("location:home?do=bounce&name=".$user_row['username'].""); exit;
	$edit_mode = false;
	}

	} else { $edit_mode = false; }

	if(!$error && !IsUserBanned($user_row['username'])){
	$body_id = "viewmode";
	if($edit_mode){
	$body_id = "editmode";
	}

	} else { $body_id = "home"; }

	if($searchname == $rawname && $logged_in){
	$pageid = "myprofile";
	} else {
	$pageid = "profile";
	}

$bg_fetch = mysql_query("SELECT data FROM homes_stickers WHERE type = '4' AND userid = '".$user_row['id']."' AND groupid = '-1' LIMIT 1");
$bg_exists = mysql_num_rows($bg_fetch);

	if($bg_exists < 1){ // if there's no background override for this user set it to the standard
		$bg = "b_bg_pattern_abstract2";
	} else {
		$bg = mysql_fetch_array($bg_fetch);
		$bg = "b_" . $bg[0];
	}

	if($searchname !== $name){
	mysql_query("INSERT INTO logs_visitedhomes (id_user,id_target,timestamp) VALUES ('".$my_id."','".$user_row['id']."','".time()."')");
	}

	$defaultskin_check = mysql_query("SELECT * FROM homes_stickers WHERE userid = '".$user_row['id']."' AND type = '2' AND subtype = '1'");
	if(mysql_num_rows($defaultskin_check) < 1){
		mysql_query("INSERT INTO homes_stickers (userid,type,data,subtype,x,y,z,skin) VALUES ('".$user_row['id']."','2','0','1','25','25','5','defaultskin')") or die(mysql_error());
	}

mysql_fetch_assoc($get_friends = mysql_query("SELECT * FROM messenger_friendships WHERE user_two_id = '".$my_id."' and user_one_id = '".$user_row['id']."' or user_one_id = '".$my_id."' and user_two_id = '".$user_row['id']."'"));
$friend = mysql_fetch_assoc($get_friends);

if(!$error){
if($user_row['visibility'] == "NOBODY" && $user_row['username'] == $name or $user_row['visibility'] == "FRIENDS" && $friend['user_two_id'] == $my_id or $user_row['visibility'] == "FRIENDS" && $friend['user_one_id'] == $my_id or $user_row['visibility'] == "EVERYONE" or $myrow['rank'] > 6){

require_once('./templates/community_hsubheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="mypage-wrapper" class="cbb blue">
<div class="box-tabs-container box-tabs-left clearfix">

<?php if($user_row['username'] == $name && $edit_mode !== true){ ?><a href="<?php echo $path; ?>/home/<?php echo $user_row['username']; ?>&do=edit" id="edit-button" class="new-button dark-button edit-icon" style="float:left"><b><span></span>Editar</b><i></i></a><?php } ?>
    <h2 class="page-owner"><?php echo $user_row['username']; ?></h2>
    <ul class="box-tabs"></ul>
</div>

<div id="mypage-content">
<?php if($edit_mode == true){ ?>
<div id="top-toolbar" class="clearfix">
	<ul>
		<li><a href="#" id="inventory-button">Inventario</a></li>
		<li><a href="#" id="webstore-button">Catálogo</a></li>
	</ul>

	<form action="#" method="get" style="width: 50%;">
		<a id="cancel-button" class="new-button red-button cancel-icon" href="#"><b><span></span>Cancelar Edição</b><i></i></a>
		<a id="save-button" class="new-button green-button save-icon" href="#"><b><span></span>Salvar Modificações</b><i></i></a>
	</form>
</div>
<?php } ?>
		<div id="mypage-bg" class="<?php echo $bg; ?>">
			<div id="playground-outer">
				<div id="playground">

<?php

$get_em = mysql_query("SELECT * FROM homes_stickers WHERE userid = '".$user_row['id']."' AND groupid = '-1' AND type < 4 LIMIT 200") or die(mysql_error());
while ($row = mysql_fetch_assoc($get_em)) {

	switch($row['type']){
	default: $type = "sticker"; break;
	case 1: $type = "sticker"; break;
	case 2: $type = "widget"; break;
	case 3: $type = "stickie"; break;
	case 4: $type = "ignore"; break;
	}

	if($edit_mode == true){
	$edit = "\n<img src=\"./web-gallery/images/myhabbo/icon_edit.gif\" width=\"19\" height=\"18\" class=\"edit-button\" id=\"" . $type . "-" . $row['id'] . "-edit\" />
<script language=\"JavaScript\" type=\"text/javascript\">
Event.observe(\"".$type."-".$row['id']."-edit\", \"click\", function(e) { openEditMenu(e, ".$row['id'].", \"".$type."\", \"".$type."-".$row['id']."-edit\"); }, false);
</script>\n";
	} else {
	$edit = " ";
	}

	$content = bbcode_format(nl2br(HoloText($row['data'])));

	if($type == "stickie"){
	printf("<div class=\"movable stickie n_skin_%s-c\" style=\" left: %spx; top: %spx; z-index: %s;\" id=\"stickie-%s\">
	<div class=\"n_skin_%s\" >
		<div class=\"stickie-header\">
			<h3>%s</h3>
			<div class=\"clear\"></div>
		</div>
		<div class=\"stickie-body\">
			<div class=\"stickie-content\">
				<div class=\"stickie-markup\">%s</div>
				<div class=\"stickie-footer\">
				</div>
			</div>
		</div>
	</div>
</div>",$row['skin'],$row['x'],$row['y'],$row['z'],$row['id'],$row['skin'],$edit,$content);
	} elseif($type == "sticker"){
	printf("<div class=\"movable sticker s_%s\" style=\"left: %spx; top: %spx; z-index: %s\" id=\"sticker-%s\">\n%s\n</div>", $row['data'], $row['x'], $row['y'], $row['z'], $row['id'], $edit);
	} elseif($type == "widget"){

		switch($row['subtype']){
		case 1: $subtype = "Profilewidget"; break;
		case 2: $subtype = "GroupsWidget"; break;
		case 3: $subtype = "RoomsWidget"; break;
		case 4: $subtype = "GuestbookWidget"; break;
		case 5: $subtype = "FriendsWidget"; break;
		case 6: $subtype = "TraxPlayerWidget"; break;
		case 7: $subtype = "HighScoresWidget"; break;
		case 8: $subtype = "BadgesWidget"; break;
		case 9: $subtype = "RatingWidget";
		}
?>


<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($subtype == "GroupsWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<?php $groups = mysql_evaluate("SELECT COUNT(*) FROM group_members WHERE id_user = '".$user_row['id']."' LIMIT 1"); ?>

<div class="movable widget GroupsWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline">
		  <h3><span class="header-left">&nbsp;</span><span class="header-middle">Meus grupos (<span id="groups-list-size"><?php echo $groups; ?></span>)</span><span class="header-right"><?php echo $edit; ?></span></h3>
		</div>
	</div>
	<div class="widget-body">
		<div class="widget-content">

<div class="groups-list-container">
<ul class="groups-list">

<?php

$get_groups = mysql_query("SELECT * FROM group_members WHERE id_user = '".$user_row['id']."'") or die(mysql_error());

if(mysql_num_rows($get_groups) > 0){
while($members_row = mysql_fetch_assoc($get_groups)){

$get_groupdata = mysql_query("SELECT * FROM group_details WHERE id = '".$members_row['id_group']."' LIMIT 1") or die(mysql_error());
$grouprow = mysql_fetch_assoc($get_groupdata);

?>

	<li title="<?php echo $grouprow['name']; ?>" id="groups-list-<?php echo $row['id']; ?>-<?php echo $grouprow['id']; ?>">
	<div class="groups-list-icon"><a href="<?php echo $path; ?>/groups/<?php echo $$grouprow['id']; ?>"><img src='./habbo-imaging/badge-fill/<?php echo $grouprow['badge']; ?>.gif'></a></div>
	<div class="groups-list-open"></div>
	<h4><a href="<?php echo $path; ?>/groups/<?php echo $grouprow['id']; ?>"><?php echo $grouprow['name']; ?></a></h4>
	<p>
	Fundado:<br />
	<?php if($members_row['is_current'] == 1){ ?><div class="favourite-group" title="Favorito"></div><?php } ?>
	<?php if($members_row['rank'] > 1 && $grouprow['ownerid'] !== $user_row['id']){ ?><div class="admin-group" title="Admin"></div><?php } ?>
	<?php if($grouprow['ownerid'] == $user_row['id'] && $members_row['rank'] > 1){ ?><div class="owned-group" title="Proprietário"></div><?php } ?>
	<b><?php echo $grouprow['created']; ?></b>
	</p>
	<div class=\"clear\"></div>
	</li>

<?php } }else { echo" Tem nenhum grupo"; } ?>

</ul></div>

<div class="groups-list-loading"><div><a href="#" class="groups-loading-close"></a></div><div class="clear"></div><p style="text-align:center"><img src="./web-gallery/images/progress_bubbles.gif" alt="" width="29" height="6"></p></div>
<div class="groups-list-info"></div>

		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<script type="text/javascript">
document.observe("dom:loaded", function() {
	new GroupsWidget('<?php echo $user_row['id']; ?>', '<?php echo $row['id']; ?>');
});
</script>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "Profilewidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<?php

$found_profile = true;
$info = mysql_query("SELECT * FROM users WHERE username = '".$searchname."' LIMIT 1") or die(mysql_error());
$userdata = mysql_fetch_assoc($info);

if(mysql_num_rows($info) > 0){

?>

<div class="movable widget ProfileWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline">
		  <h3><?php echo $edit; ?>
<span class="header-left">&nbsp;</span><span class="header-middle">MEU PERFIL</span><span class="header-right">&nbsp;</span></h3>
		</div>
	</div>

	<div class="widget-body">
		<div class="widget-content">
	<div class="profile-info">

		<div class="name" style="float: left">
			<span class="name-text"><?php echo $userdata['username']; ?></span>
		</div>

		<br class="clear">

<?php if($userdata['online'] == "1"){ ?>
	<img alt="online" src="<?php echo $path; ?>/web-gallery/images/myhabbo/habbo_online_anim_big.gif"> 
<?php } else { ?>
	<img alt="offline" src="<?php echo $path; ?>/web-gallery/images/myhabbo/habbo_offline_big.gif">
<?php } ?>

		<div class="birthday text">
			<?php echo $shortname; ?> criado em:
		</div>
		<div class="birthday date">
			<?php echo date('d-m-Y', $userdata['account_created']); ?>
		</div>
		<div>

<?php
	$get_cc = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' and badge_slot = '1'") or die(mysql_error());
	$get_cc_badge = mysql_fetch_assoc($get_cc);

	$groupbadge = mysql_query("SELECT * FROM group_members WHERE id_user = '".$userdata['id']."' AND is_current = '1'");
	$badge = mysql_fetch_assoc($groupbadge);
	$groupbadge_check = mysql_num_rows($groupbadge);

		$badgedetails_a = mysql_query("SELECT * FROM group_details WHERE id = '".$badge['id_group']."'");
		$badgedetails = mysql_fetch_assoc($badgedetails_a);

	if($groupbadge_check > 0){
		echo"<a href='".$path."/groups/".$badgedetails['id']."'><img src='".$path."/habbo-imaging/badge-fill/".$badgedetails['badge'].".gif'></a>";
	}

	if(mysql_num_rows($get_cc) > 0){
		echo "<img src=\"".$cimagesurl.$badgesurl.$get_cc_badge['badge_id'].".gif\"/></a>";
	}
?>
        </div>
	</div>
	<div class="profile-figure">
			<img alt="<?php echo $userdata['username']; ?>" src="<?php echo $avatar; echo $userdata['look']; ?>&size=b&direction=4&head_direction=4&gesture=sml">
	</div>
	<?php if($userdata['motto'] != null){ ?>
		<div class="profile-motto">
			<?php echo HoloText($userdata['motto']); ?>
			<div class="clear"></div>
		</div>
	<?php }
	if($userdata['id'] != $my_id && $logged_in == true){ ?>
		<div class="profile-friend-request clearfix">
			<a href="./myhabbo/friends_add.php?id=<?php echo $userdata['id']; ?>" class="new-button" id="add-friend" style="float: left"><b>Adicionar como amigo</b><i></i></a>
		</div>
	<?php } ?>
	<br clear="all" style="display: block; height: 1px">
    <div id="profile-tags-panel">
    <div id="profile-tag-list">
<div id="profile-tags-container">

<?php

$get_tags = mysql_query("SELECT * FROM user_tags WHERE user_id = '".$userdata['id']."' ORDER BY id LIMIT 20") or die(mysql_error());
if(mysql_num_rows($get_tags) > 0){

if($userdata['id'] == $my_id && $logged_in){
while ($row1 = mysql_fetch_assoc($get_tags)){

?>

<span class="tag-search-rowholder">
<a href="./tag/<?php echo $row1['tag']; ?>" class="tag-search-link tag-search-link-<?php echo $row1['tag']; ?>"><?php echo $row1['tag']; ?></a>
<img border="0" class="tag-delete-link tag-delete-link-<?php echo $row1['tag']; ?>" onMouseOver="this.src='./web-gallery/images/buttons/tags/tag_button_delete_hi.gif'" onMouseOut="this.src='./web-gallery/images/buttons/tags/tag_button_delete.gif'" src="./web-gallery/images/buttons/tags/tag_button_delete.gif"/></span>

<?php } } elseif($logged_in){ while ($row1 = mysql_fetch_assoc($get_tags)){ ?>


<span class="tag-search-rowholder">
<a href="./tag/<?php echo $row1['tag']; ?>" class="tag-search-link tag-search-link-<?php echo $row1['tag']; ?>"><?php echo $row1['tag']; ?></a>
<img border="0" class="tag-add-link tag-add-link-<?php echo $row1['tag']; ?>" onMouseOver="this.src='./web-gallery/images/buttons/tags/tag_button_add_hi.gif'" onMouseOut="this.src='./web-gallery/images/buttons/tags/tag_button_add.gif'" src="./web-gallery/images/buttons/tags/tag_button_add.gif"/></span>

<?php } } else { while ($row1 = mysql_fetch_assoc($get_tags)){ ?>

<span class="tag-search-rowholder">
<a href="./tag/<?php echo $row1['tag']; ?>" class="tag-search-link tag-search-link-<?php echo $row1['tag']; ?>"><?php echo $row1['tag']; ?></a></span>

<?php } } } else { echo "Nenhum grupo quaisquer'"; } ?>

<img id="tag-img-added" border="0" src="./web-gallery/images/buttons/tags/tag_button_added.gif" style="display:none"/>
</div>

<script type="text/javascript">
    document.observe("dom:loaded", function() {
        TagHelper.setTexts({
            buttonText: "OK",
            tagLimitText: "Você atingiu o limite de \ "Tag \ '! Remove um antes, se você quiser adicionar um novo."
        });
    });
</script>
    </div>

<div id="profile-tags-status-field">
 <div style="display: block;">
  <div class="content-red">
   <div class="content-red-body">
    <span id="tag-limit-message"><img src="./web-gallery/images/register/icon_error.gif"/> Limite de Tags.</span>
    <span id="tag-invalid-message"><img src="./web-gallery/images/register/icon_error.gif"/> Tag Inválido</span>
   </div>
  </div>
 <div class="content-red-bottom">
  <div class="content-red-bottom-body"></div>
 </div>
 </div>
</div>

<?php if($userdata['id'] == $my_id){ ?>

<div class="profile-add-tag">
<input type="text" id="profile-add-tag-input" maxlength="30"/><br clear="all"/>
<a href="#" class="new-button" style="float:left;margin:5px 0 0 0;" id="profile-add-tag"><b>Adicionar Tag</b><i></i></a>
</div>

<?php } ?>

</div>
    <script type="text/javascript">
		document.observe("dom:loaded", function() {
			new ProfileWidget('<?php echo $userdata['id']; ?>', '<?php echo $userdata['id']; ?>', {
				headerText: "Você tem certeza?",
				messageText: "Tem certeza de que quer <strong\><?php echo $userdata['username']; ?></strong\> ser seu amigo? Pense duas vezes antes de dar o OK!",
				buttonText: "OK",
				cancelButtonText: "Cancelar"
			});
		});
	</script>
		<div class="clear"></div>
		</div>
	</div>
</div></div>

<?php } ?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "RoomsWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<div class="movable widget RoomsWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">

<div class="widget-headline">
  <h3>
<?php echo $edit; ?>
</script>

<span class="header-left">&nbsp;</span><span class="header-middle">Meus Quartos</span><span class="header-right">&nbsp;</span></h3>

</div>	
</div>

<div class="widget-body">
<div class="widget-content">

<?php 			

$roomsql = mysql_query("SELECT * FROM rooms WHERE owner = '".$user_row['username']."'");
if(mysql_num_rows($roomsql) >= 1){ 

?>

<div id="room_wrapper">
<table border="0" cellpadding="0" cellspacing="0">

<?php 

$i = 0;
while ($room = mysql_fetch_assoc($roomsql)) {
$i++;

if(mysql_num_rows($roomsql) == $i){
	$asdf = " ";
} else {
	$asdf = "\"class=\"dotted-line\"";
}

if($room['state'] == "open"){
	$icon = "open";
	$text = "Entrar";
}elseif($room['state'] == "password"){
	$icon = "password";
	$text = "Protegido por senha";
} elseif($room['state'] == "locked"){
	$icon = "locked";
	$text = "Fechado";
}

?>

<tr>

<td valign="top">
<div class="room_image">
<img src="<?php echo $path; ?>/web-gallery/images/myhabbo/rooms/room_icon_<?php echo $icon; ?>.gif" alt="" align="middle"/>
</div>
</td>

<td <?php echo $asdf; ?>>
<div class="room_info">
<div class="room_name"><?php echo $room['caption']; ?></div>
<img id="room-<?php echo $room['id']; ?>-report" class="report-button report-r"alt="report" src="<?php echo $path; ?>/web-gallery/images/myhabbo/buttons/report_button.gif" style="display: none;" />

<div class="clear"></div>
<div><?php echo $room['description']; ?></div>

<a href="/client?forwardId=2&amp;roomId=<?php echo $room['id']; ?>" target="" id="room-navigation-link_<?php echo $room['id']; ?>" onclick="HabboClient.roomForward(this, '<?php echo $room['id']; ?>', 'private', true); return false;">
<?php echo $text; ?>
</a>
 
</div>
<br class="clear" />

</td>
</tr>

<?php } ?>

<br class="clear" />
</td>
</tr>
</table>
</div> 

<?php } else { echo "Não tem nenhum Quarto"; } ?>

<div class="clear"></div>
</div>
</div>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "GuestbookWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
	
<?php

$sql = mysql_query("SELECT * FROM homes_guestbook WHERE widget_id = '".$row['id']."' ORDER BY id DESC");
$count = mysql_num_rows($sql);

if($row['10'] == "0"){
	$status = "public";
}else{
	$status = "private";
}

if($searchname == $name){
	mysql_query("UPDATE homes_guestbook SET pickup = '0' WHERE widget_id = '".$row['id']."'");
}

?>
	
<div class="movable widget GuestbookWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline">
		  <h3>
		<?php echo $edit; ?>
		<span class="header-left">&nbsp;</span><span class="header-middle">Livro de visitas(<span id="guestbook-size"><?php echo $count; ?></span>) <span id="guestbook-type" class="<?php echo $status; ?>"><?php if($row['10'] == "0"){ ?><img src="./web-gallery/images/groups/status_exclusive.gif" title="Solo amigos" alt="Apenas amigos"/><?php } ?></span></span><span class="header-right">&nbsp;</span></h3>
		</div>	
	</div>

<div class="widget-body">
<div class="widget-content">
<div id="guestbook-wrapper" class="gb-public">
<ul class="guestbook-entries" id="guestbook-entry-container">

<?php if($count == 0){ ?>
	<div id="guestbook-empty-notes">Livro vazio</div>
<?php } else {

	$i = 0;
	while ($guestbook = mysql_fetch_assoc($sql)) {
	$i++;
				
	$userdata = mysql_fetch_assoc(mysql_query("SELECT * FROM users WHERE id = '".$guestbook['userid']."' LIMIT 1"));
	if($my_id == $guestbook['userid']){
		$owneronly = "<img src=\"./web-gallery/images/myhabbo/buttons/delete_entry_button.gif\" id=\"gbentry-delete-".$guestbook['id']."\" class=\"gbentry-delete\" style=\"cursor:pointer\" alt=\"\"/><br/>";
	} elseif($user_row['id'] == $my_id) {
		$owneronly = "<img src=\"./web-gallery/images/myhabbo/buttons/delete_entry_button.gif\" id=\"gbentry-delete-".$guestbook['id']."\" class=\"gbentry-delete\" style=\"cursor:pointer\" alt=\"\"/><br/>";
	} else {
		$owneronly = "";
	}
	
	if($userdata['online'] == '1'){ 
		$useronline = "online";
	} else {
		$useronline = "offline";
	}
?>
	<li id="guestbook-entry-<?php echo $guestbook['id']; ?>" class="guestbook-entry">
		<div class="guestbook-author">
			<img src="<?php echo $avatar; echo $userdata['look']; ?>&direction=2&head_direction=2&gesture=sml&size=s" alt="<?php echo $userdata['username']; ?>" title="<?php echo $userdata['username']; ?>"/>
		</div>
			<div class="guestbook-actions">
					<?php echo $owneronly; ?>
			</div>

		<div class="guestbook-message">
			<div class="<?php echo $useronline; ?>">
				<a href="<?php echo $path; ?>/home/<?php echo $userdata['username']; ?>"><?php echo $userdata['username']; ?></a>
			</div>
			<p><?php echo HoloText($guestbook["message"],false,true); ?></p>
		</div>
		<div class="guestbook-cleaner">&nbsp;</div>
		<div class="guestbook-entry-footer metadata"><?php echo $guestbook['time']; ?></div>
	</li>

<?php }	} ?>

</ul>
</div>

<?php if($edit_mode == false){ ?>
	<div class="guestbook-toolbar clearfix">
	<a href="<?php echo $path; ?>/home/<?php echo $searchname; ?>#" class="new-button envelope-icon" id="guestbook-open-dialog">
	<b><span></span>Nova mensagem</b><i></i>
	</a>
	</div>
<?php } ?>
<script type="text/javascript">	
	document.observe("dom:loaded", function() {
		var gb<?php echo $row['id']; ?> = new GuestbookWidget('17570', '<?php echo $row['id']; ?>', 500);
		var editMenuSection = $('guestbook-privacy-options');
		if (editMenuSection) {
			gb<?php echo $row['0']; ?>.updateOptionsList('public');
		}
	});
</script>
		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "HighScoresWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<div class="movable widget HighScoresWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline">
		  <h3><?php echo $edit; ?><span class="header-left">&nbsp;</span><span class="header-middle">Meus pontos</span><span class="header-right">&nbsp;</span></h3>
		</div>	
	</div>

<div class="widget-body">
<div class="widget-content">

<table>
	<tr colspan="2">
		<th>Battle Ball</a></th>
	</tr>
	<tr>
		<td>Jogadas</td>
		<td>-/-</td>
	</tr>

	<tr>
		<td>Pontos</td>
		<td>-/-</td>
	</tr>
</table>

		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "FriendsWidget"){  ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<?php
	
$sql = mysql_query("SELECT * FROM messenger_friendships WHERE user_two_id = '".$user_row['id']."'");
$count = mysql_num_rows($sql);

?>
<div class="movable widget FriendsWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline">
		  <h3><?php echo $edit; ?><span class="header-left">&nbsp;</span><span class="header-middle">Meus amigos (<?php echo $count; ?>)</span><span class="header-right">&nbsp;</span></h3>
		</div>	
	</div>
	<div class="widget-body">
		<div class="widget-content">

<div id="avatar-list-search">
<input type="text" style="float:left;" id="avatarlist-search-string"/>
<a class="new-button" style="float:left;" id="avatarlist-search-button"><b>Buscar</b><i></i></a>
</div>
<br clear="all"/>

<div id="avatarlist-content">

<?php
$bypass = true;
$widgetid = $row['id'];
include('./myhabbo/avatarlist_friendsearchpaging.php');
?>

<script type="text/javascript">
document.observe("dom:loaded", function() {
	window.widget<?php echo $row['id']; ?> = new FriendsWidget('<?php echo $user_row['id']; ?>', '<?php echo $row['id']; ?>');
});
</script>

</div>
		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "TraxPlayerWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<div class="movable widget TraxPlayerWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['0']; ?>-handle">
		<div class="widget-headline"><h3><?php echo $edit; ?><span class="header-left">&nbsp;</span><span class="header-middle">TRAXPLAYER</span><span class="header-right">&nbsp;</span></h3>
		</div>	
	</div>
<div class="widget-body">
<div class="widget-content">

<?php 

if($row['var'] == ""){
	$songselected = false;
}else{
	$songselected = true;
}

if($edit_mode == true){

?>

<div id="traxplayer-content" style="text-align: center;">
<img src="./web-gallery/images/traxplayer/player.png"/>
</div>

<div id="edit-menu-trax-select-temp" style="display:none">
<select id="trax-select-options-temp">
<option value="">- Escolha música -</option>

<?php

$mysql = mysql_query("SELECT * FROM items WHERE user_id = '".$user_row['id']."'");
$i = 0;
while($machinerow = mysql_fetch_assoc($mysql)){
$i++;

$sql = mysql_query("SELECT * FROM soundmachine_songs WHERE machineid = '".$machinerow['id']."'");
$n = 0;
while($songrow = mysql_fetch_assoc($sql)){
$n++;
if($songrow['id'] <> ""){ echo "<option value=\"".$songrow['id']."\">".trim(nl2br(HoloText($songrow['title'])))."</option>\n"; }

} } ?>

</select>
</div>
<?php }elseif($songselected == false){ ?>
Nao temperatura selecionada pode cair.

<?php 

}else{
	$sql1 = mysql_query("SELECT * FROM soundmachine_songs WHERE id = '".$row['8']."' LIMIT 1");
	$songrow1 = mysql_fetch_assoc($sql); 

?>
<div id="traxplayer-content" style="text-align:center;"></div>
<embed type="application/x-shockwave-flash"
src="<?php echo $path; ?>web-gallery/flash/traxplayer/traxplayer.swf" name="traxplayer" quality="high"
base="<?php echo $path; ?>web-gallery/flash/traxplayer/" allowscriptaccess="always" menu="false"
wmode="transparent" flashvars="songUrl=<?php echo $path; ?>myhabbo/trax_song.php?songId=<?php echo $row['8']; ?>&amp;sampleUrl=http://images.habbohotel.com/dcr/hof_furni/mp3/" height="66" width="210" />

<?php } ?>

		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "BadgesWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<?php	
	$pagenum = "1";
	$sql = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$user_row['id']."' ORDER BY badge_id ASC");
	$count = mysql_num_rows($sql);
?>

<div class="movable widget BadgesWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline">
		  <h3><?php echo $edit; ?><span class="header-left">&nbsp;</span><span class="header-middle">Emblemas</span><span class="header-right">&nbsp;</span></h3>
		</div>	
	</div>
	<div class="widget-body">
		<div class="widget-content">
    <div id="badgelist-content">
	<?php if($count == 0){
		echo "Este usuário não tem placas.";
	}else{
		$bypass1 = true;
		include('./myhabbo/badgelist_badgepaging.php');
	?>
	<?php } ?>
        <script type="text/javascript">
        document.observe("dom:loaded", function() {
            window.badgesWidget<?php echo $row['id']; ?> = new BadgesWidget('<?php echo $count; ?>', '<?php echo $row['id']; ?>');
        });
        </script>
    </div>
		<div class="clear"></div>
		</div>
	</div>

</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "RatingWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<?php	
	$pagenum = "1";
	$sql = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$user_row['id']."' ORDER BY badge_id ASC");
	$count = mysql_num_rows($sql);
?>

<div class="movable widget RatingWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline">
		  <h3><?php echo $edit; ?><span class="header-left">&nbsp;</span><span class="header-middle">Minha Qualificação</span><span class="header-right">&nbsp;</span></h3>
		</div>	
	</div>

	<div class="widget-body">
		<div class="widget-content">
	<div id="rating-main">
<?php
$myvote = mysql_evaluate("SELECT COUNT(*) FROM homes_ratings WHERE raterid = '".$my_id."' AND userid = '".$user_row['id']."'");
$totalvotes = mysql_evaluate("SELECT COUNT(*) FROM homes_ratings WHERE userid = '".$user_row['id']."'");
$highvotes = mysql_evaluate("SELECT COUNT(*) FROM homes_ratings WHERE userid = '".$user_row['id']."' AND rating > 3");
$votestally = mysql_evaluate("SELECT SUM(rating) FROM homes_ratings WHERE userid = '".$user_row['id']."'");

$x = $totalvotes;
if($x == 0){ $x = 1; }
$average = round($votestally / $x, 1);
$px = ceil(($average * 150) / 5);

if($user_row['id'] == $my_id|| $myvote > 0){ $bypass = true; $ownerid = $user_row['id']; $widgetid = $row['id']; $rate = 0; require_once('./myhabbo/rating_rate.php'); }else{ ?>
<script type="text/javascript">	
	var ratingWidget;
	document.observe("dom:loaded", function() { 
		ratingWidget = new RatingWidget(<?php echo $user_row['id']; ?>, <?php echo $row['id']; ?>);
	}); 
</script>
<div class="rating-average">
		<b>Média de votos: <?php echo $average; ?></b>
	    <div id="rating-stars" class="rating-stars" >
				<ul id="rating-unit_ul1" class="rating-unit-rating">
				<li class="rating-current-rating" style="width:<?php echo $px; ?>px;" /><?php if($logged_in){ ?>
					<li><a href="#"   class="r1-unit rater">1</a></li>
					<li><a href="#"   class="r2-unit rater">2</a></li>
					<li><a href="#"   class="r3-unit rater">3</a></li>
					<li><a href="#"   class="r4-unit rater">4</a></li>
					<li><a href="#"   class="r5-unit rater">5</a></li>
	
			<?php } ?></ul>	
	</div>
	<?php echo $totalvotes; ?> votos no total
	
	<br/>
	(<?php echo $highvotes; ?> <?php echo $shortname; ?>s  â votaram 4 mais )</div>
<?php } ?>

	</div>
		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<?php } } } ?>
				</div>
			</div>
			<div id="mypage-ad">
    <div class="habblet ">
<div class="ad-container">
<?php if($hc > 0){ ?>
&nbsp;
<?php } else { ?>
<?php } ?>
</div>
    
    </div>
				</div>
			</div>
	</div>

</div>

<script type="text/javascript">
	Event.observe(window, "load", observeAnim);
	document.observe("dom:loaded", function() {
		initDraggableDialogs();
        repositionInvalidItems();
	});
</script>
    </div>

<?php if($user_row['mymusik'] !== '0'){ ?>
<embed src="<?php echo htmlspecialchars($user_row['mymusik']); ?>&autoplay=1&volume=50" type="application/x-shockwave-flash" wmode="transparent" width="1" height="1"></embed> 
<?php } ?>


<script language="JavaScript" type="text/javascript">
initEditToolbar();
initMovableItems();
document.observe("dom:loaded", initDraggableDialogs);
</script>


<div id="edit-save" style="display:none;"></div>
    </div>
</div>

</div>

<div id="edit-menu" class="menu">
	<div class="menu-header">
		<div class="menu-exit" id="edit-menu-exit"><img src="./web-gallery/images/dialogs/menu-exit.gif" alt="" width="11" height="11" /></div>
		<h3>Editar</h3>
	</div>
	<div class="menu-body">
		<div class="menu-content">
			<form action="#" onsubmit="return false;">
				<div id="edit-menu-skins">
	<select id="edit-menu-skins-select">
			<option value="1" id="edit-menu-skins-select-defaultskin">Por padrão</option>
			<option value="6" id="edit-menu-skins-select-goldenskin">Dourado</option>
		<?php if($hc > 0){ ?>
			<option value="8" id="edit-menu-skins-select-hc_pillowskin">HC para meninas</option>
			<option value="7" id="edit-menu-skins-select-hc_machineskin">HC para meninos</option>
		<?php } ?>
		<?php if($user_rank > 4){ ?>
			<option value="9" id="edit-menu-skins-select-nakedskin">Staff</option>
		<?php } ?>
			<option value="3" id="edit-menu-skins-select-metalskin">Metal</option>
			<option value="5" id="edit-menu-skins-select-notepadskin">Nota-etiqueta</option>
			<option value="2" id="edit-menu-skins-select-speechbubbleskin">Caixa de diálogo</option>
			<option value="4" id="edit-menu-skins-select-noteitskin">Bloco de notas</option>
	</select>
				</div>
				<div id="edit-menu-stickie">
					<p> Atenção! Em é voce clique em Excluir esta nota será desligada para Sempre. </p>
			  </div>
		  <div id="rating-edit-menu">
					<input type="button" id="ratings-reset-link"
						value="Classificação da" />
				</div>
				<div id="highscorelist-edit-menu" style="display:none">
					<select id="highscorelist-game">
						<option value="">Selecionar jogo</option>
						<option value="1">Battle Ball</option>
						<option value="2">SnowStorm</option>
						<option value="0">Wobble Squabble</option>
					</select>
				</div>
				<div id="edit-menu-remove-group-warning">
					<p>O item é de outro usuário, retorne a seu inventário.</p>
				</div>
				<div id="edit-menu-gb-availability">
					<select id="guestbook-privacy-options">
						<option value="private">Membros</option>
						<option value="public">Público</option>
					</select>
				</div>
				<div id="edit-menu-trax-select">
					<select id="trax-select-options"></select>
				</div>
				<div id="edit-menu-remove">
					<input type="button" id="edit-menu-remove-button" value="Excluir" />
				</div>
			</form>
			<div class="clear"></div>
		</div>
	</div>
	<div class="menu-bottom"></div>
</div>

<script language="JavaScript" type="text/javascript">
Event.observe(window, "resize", function() { if (editMenuOpen) closeEditMenu(); }, false);
Event.observe(document, "click", function() { if (editMenuOpen) closeEditMenu(); }, false);
Event.observe("edit-menu", "click", Event.stop, false);
Event.observe("edit-menu-exit", "click", function() { closeEditMenu(); }, false);
Event.observe("edit-menu-remove-button", "click", handleEditRemove, false);
Event.observe("edit-menu-skins-select", "click", Event.stop, false);
Event.observe("edit-menu-skins-select", "change", handleEditSkinChange, false);
Event.observe("guestbook-privacy-options", "click", Event.stop, false);
Event.observe("guestbook-privacy-options", "change", handleGuestbookPrivacySettings, false);
Event.observe("trax-select-options", "click", Event.stop, false);
Event.observe("trax-select-options", "change", handleTraxplayerTrackChange, false);
</script>

<div class="cbb topdialog" id="guestbook-form-dialog">
	<h2 class="title dialog-handle">Editar entrada</h2>

	<a class="topdialog-exit" href="#" id="guestbook-form-dialog-exit">X</a>
	<div class="topdialog-body" id="guestbook-form-dialog-body">
<div id="guestbook-form-tab">
<form method="post" id="guestbook-form">
    <p>
        Nota: O comprimento máximo da mensagem: 200 caracteres
        <input type="hidden" name="ownerId" value="441794" />
	</p>
	<div>
	    <textarea cols="15" rows="5" name="message" id="guestbook-message"></textarea>
    <script type="text/javascript">
        bbcodeToolbar = new Control.TextArea.ToolBar.BBCode("guestbook-message");
        bbcodeToolbar.toolbar.toolbar.id = "bbcode_toolbar";
         var colors = { "red" : ["#d80000", "Vermelho"],
            "orange" : ["#fe6301", "Laranja"],
            "yellow" : ["#ffce00", "Amarelo"],
            "green" : ["#6cc800", "Verde"],
            "cyan" : ["#00c6c4", "Azul bebe"],
            "blue" : ["#0070d7", "Azul"],
            "gray" : ["#828282", "Cinza"],
            "black" : ["#000000", "Preto"]
        };
        bbcodeToolbar.addColorSelect("Farben", colors, true);
    </script>
    </div>

	<div class="guestbook-toolbar clearfix">
		<a href="#" class="new-button" id="guestbook-form-cancel"><b>Cancelar</b><i></i></a>
		<a href="#" class="new-button" id="guestbook-form-preview"><b>Previzualizar</b><i></i></a>
	</div>
</form>
</div>
<div id="guestbook-preview-tab">&nbsp;</div>
	</div>
</div>
<div class="cbb topdialog" id="guestbook-delete-dialog">
	<h2 class="title dialog-handle">Excluir entrada</h2>

	<a class="topdialog-exit" href="#" id="guestbook-delete-dialog-exit">X</a>
	<div class="topdialog-body" id="guestbook-delete-dialog-body">
<form method="post" id="guestbook-delete-form">
	<input type="hidden" name="entryId" id="guestbook-delete-id" value="" />
	<p>Tem certeza que quer apagar sua entrada?</p>
	<p>
		<a href="#" id="guestbook-delete-cancel" class="new-button"><b>Cancelar</b><i></i></a>
		<a href="#" id="guestbook-delete" class="new-button"><b>Apagar</b><i></i></a>
	</p>
</form>
	</div>
</div>
<div id="group-tools" class="bottom-bubble">
	<div class="bottom-bubble-t"><div></div></div>
	<div class="bottom-bubble-c">
<h3>Editar grupo</h3>

<ul>
	<li><a href="group_profile.php?id=<?php echo $groupid; ?>&do=edit" id="group-tools-style">Editar Página</a></li>
	<?php if($ownerid == $my_id){ ?>
	<li><a href="#" id="group-tools-settings">Configuração</a></li><?php } ?>
	<li><a href="#" id="group-tools-badge">Emblema</a></li>
	<li><a href="#" id="group-tools-members">Membros</a></li>
</ul>

	</div>
	<div class="bottom-bubble-b"><div></div></div>
</div>

<div class="cbb topdialog black" id="dialog-group-settings">

	<div class="box-tabs-container">
<ul class="box-tabs">
	<li class="selected" id="group-settings-link-group"><a href="#">Ajustes do grupo</a><span class="tab-spacer"></span></li>
	<li id="group-settings-link-forum"><a href="#">Ajustes do fórum</a></li>
	<li id="group-settings-link-room"><a href="#">Ajustes da sala</a><span class="tab-spacer"></span></li>
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
</script><div class="cbb topdialog black" id="group-memberlist">

	<div class="box-tabs-container">
<ul class="box-tabs">
	<li class="selected" id="group-memberlist-link-members"><a href="#">Membros</a><span class="tab-spacer"></span></li>
	<li id="group-memberlist-link-pending"><a href="#">Memrbos atuais</a><span class="tab-spacer"></span></li>
</ul>
</div>

	<a class="topdialog-exit" href="#" id="group-memberlist-exit">X</a>
	<div class="topdialog-body" id="group-memberlist-body">
<div id="group-memberlist-members-search" class="clearfix" style="display:none">

    <a id="group-memberlist-members-search-button" href="#" class="new-button"><b>Buscar</b><i></i></a>
    <input type="text" id="group-memberlist-members-search-string"/>
</div>
<div id="group-memberlist-members" style="clear: both"></div>
<div id="group-memberlist-members-buttons" class="clearfix">
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-give-rights"><b>Dar permissões</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-revoke-rights"><b>Remover permissões</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-remove"><b>Remover</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button" id="group-memberlist-button-close"><b>Fechar</b><i></i></a>
</div>
<div id="group-memberlist-pending" style="clear: both"></div>
<div id="group-memberlist-pending-buttons" class="clearfix">
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-accept"><b>Aceitar</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-decline"><b>Rejeitar</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button" id="group-memberlist-button-close2"><b>Fechar</b><i></i></a>
</div>
	</div>	</div>



<script type="text/javascript">
HabboView.run();
</script>

<script type="text/javascriscript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
<?php include('./templates/login_footer.php'); ?>

<?php  }else { ?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix red ">
	<h2 class="title">Uma página Deste Privado e Usuário.</h2>

<div id="notfound-content" class="box-content">
<p class="error-text">A página que você estava procurando nao existe</p>
<img id="error-image" src="../web-gallery/v2/images/activehomes/habbo_skeleton.gif" />
</div>

</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix green ">
	<h2 class="title">Estava buscando...</h2>
<div id="notfound-looking-for" class="box-content">

<p><b>Grupo de Amigos, o una página personal?</b><br/>
Verifique se&nbsp;ele aparece&nbsp;na p&aacute;gina <a href="<?php echo $path; ?>/community">Comunidade</a>.</p>

    <p><b>Quartos?</b><br/>
    Vizite os <a href="<?php echo $path; ?>/community">quartos  recomendados</a>.</p>

    <p><b>Como comprar VIP?</b><br/>
    Vizite a p&aacute;gina <a href="<?php echo $path; ?>/credits">cr&eacute;ditos</a>.</p>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<?php } }elseif($error){ $cored = true; require_once('./error.php'); } ?>