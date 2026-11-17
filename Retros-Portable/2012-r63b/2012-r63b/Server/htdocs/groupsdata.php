<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

if(isset($_POST['searchString'])){
	$searchString = FilterText($_POST['searchString']);
	$check = mysql_query("SELECT id FROM group_details WHERE name LIKE '".$searchString."' LIMIT 1") or die(mysql_error());

	if(mysql_num_rows($check) > 0){
		$tmp = mysql_fetch_assoc($check);
		header("Location: ".$path."/groups/".$tmp['id']."");
		exit;
	}
}

if(isset($_GET['web-group-id']) && is_numeric($_GET['web-group-id'])){

	$check = mysql_query("SELECT * FROM group_details WHERE id = '".FilterText($_GET['web-group-id'])."' LIMIT 1");
	$exists = mysql_num_rows($check);

	if($exists > 0){

		$groupid = FilterText($_GET['web-group-id']);

		$error = false;
		$groupdata = mysql_fetch_assoc($check);
		$ownerid = $groupdata['ownerid'];
		$pagename = "Grupo: ".$groupdata['name']."";

		$members = mysql_evaluate("SELECT COUNT(*) FROM group_members WHERE id_group = '".$groupid."' AND is_pending = '0'");

		$check = mysql_query("SELECT * FROM group_members WHERE id_user = '".$my_id."' AND id_group = '".$groupid."' AND is_pending = '0' LIMIT 1");

		if(mysql_num_rows($check) > 0 && $logged_in){

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


if(isset($_GET['do']) && $_GET['do'] == "edit" && $logged_in){

	if($is_member && $member_rank > 1){

		$edit_mode = true;

		$check = mysql_query("SELECT * FROM homes_group_linker WHERE userid = '".$my_id."' LIMIT 1") or die(mysql_error());
		$linkers = mysql_num_rows($check);

		if($linkers > 0){
			mysql_query("UPDATE homes_group_linker SET active = '1', groupid = '".$groupid."' WHERE userid = '".$my_id."' LIMIT 1") or die(mysql_error());
		} else {
			mysql_query("INSERT INTO homes_group_linker (userid,groupid,active) VALUES ('".$my_id."','".$groupid."','1')") or die(mysql_error());
		}

	} else {
		header("location: ".$path."/groups/".$groupid."do=bounce");
		$edit_mode = false;
	}

} else {
	$edit_mode = false;
}

if(!$error){

	$body_id = "viewmode";

	if($edit_mode){
		$body_id = "editmode";
	}

}else{
	$body_id = "home";
}

$pageid = "profile";

if($groupdata['type'] !== "1" && $is_member !== true){
	$remove_pending = mysql_query("DELETE FROM group_members WHERE is_pending = '1' AND id_user = '".$my_id."' AND id_group = '".$groupid."' LIMIT 1") or die(mysql_error());
}

$viewtools = "	<div class=\"myhabbo-view-tools\">\n";

if($logged_in && !$is_member && $groupdata['type'] !== "2" && $my_membership['is_pending'] !== "1"){ $viewtools = $viewtools . "<a href=\"joingroup.php?groupId=".$groupid."\" id=\"join-group-button\">"; if($groupdata['type'] == "0" || $groupdata['type'] == "3"){ $viewtools = $viewtools . "Junte-se ao grupo"; } else { $viewtools = $viewtools . "Petição para a adesão"; } $viewtools = $viewtools . "</a>"; }
if($logged_in && $my_membership['is_current'] !== "1" && $is_member){ $viewtools = $viewtools . "<a href=\"#\" id=\"select-favorite-button\">Faça um Favorito</a>\n"; }
if($logged_in && $my_membership['is_current'] == "1" && $is_member){ $viewtools = $viewtools . "<a href=\"#\" id=\"deselect-favorite-button\">Remover de favorito</a>"; }
if($logged_in && $is_member && $my_id !== $ownerid){ $viewtools = $viewtools . "<a href=\"leavegroup.php?groupId=".$groupid."\" id=\"leave-group-button\">Deixar o Grupo</a>\n"; }

$viewtools = $viewtools . "	</div>\n";

$bg_fetch = mysql_query("SELECT data FROM homes_stickers WHERE type = '4' AND groupid = '".$groupid."' LIMIT 1");
$bg_exists = mysql_num_rows($bg_fetch);

	if($bg_exists < 1){ // if there's no background override for this user set it to the standard
		$bg = "b_bg_pattern_abstract2";
	} else {
		$bg = mysql_fetch_array($bg_fetch);
		$bg = "b_" . $bg[0];
	}

if(!$error){

require_once('./templates/community_hsubheader.php');
require_once('./templates/community_header.php');

mysql_query("UPDATE group_details SET views = views+ '1' WHERE id='".$groupid."' LIMIT 1");

$check_stickers = mysql_query("SELECT * FROM homes_stickers WHERE userid = '-1' AND groupid = '".$groupid."' AND type = '2' AND subtype = '1'");
if(mysql_num_rows($check_stickers) < 1){ 
	mysql_query("INSERT INTO homes_stickers (userid,groupid,data,type,subtype,x,y,z,skin) VALUES ('-1','".$groupid."','0','2','1','25','25','5','defaultskin')") or die(mysql_error());
}

?>

<div id="container">
	<div id="content" style="position: relative" class="clearfix">
    <div id="mypage-wrapper" class="cbb blue">
<div class="box-tabs-container box-tabs-left clearfix">
	<?php if($member_rank > 1 && !$edit_mode){ ?><a href="#" id="myhabbo-group-tools-button" class="new-button dark-button edit-icon" style="float:left"><b><span></span>Editar</b><i></i></a><?php } ?>
	<?php if(!$edit_mode){ echo $viewtools; } ?>
    <h2 class="page-owner">
<?php echo HoloText($groupdata['name']); ?>&nbsp;
<?php if($groupdata['type'] == "2"){ ?><img src='./web-gallery/images/status_closed_big.gif' alt='Closed Group' title='Closed Group'><?php } ?>
<?php if($groupdata['type'] == "1"){ ?><img src='./web-gallery/images/status_exclusive_big.gif' alt='Moderated Group' title='Moderated Group'><?php } ?></h2>
</h2>
    <ul class="box-tabs">
        <li class="selected"><a href="<?php echo $path; ?>/groups/<?php echo $groupid; ?>">P&aacute;gina Principal</a><span class="tab-spacer"></span></li>
        <li><a href="groupsdata_forum.php?web-discussions-id=<?php echo $groupid; ?>"'" onClick="">Fórum<?php if($groupdata['forum'] > 0) { ?><img src="<?php echo $path; ?>/web-gallery/images/groups/privatekey.png"><?php } ?></a><span class="tab-spacer"></span></li>
    </ul>
</div>
	<div id="mypage-content">
<?php 	if($edit_mode == true){ ?>
<div id="top-toolbar" class="clearfix">
	<ul>
		<li><a href="#" id="inventory-button">Inventario</a></li>
		<li><a href="#" id="webstore-button">Catalogo</a></li>
	</ul>

	<form action="#" method="get" style="width: 50%;">
		<a id="cancel-button" class="new-button red-button cancel-icon" href="#"><b><span></span>Cancelar</b><i></i></a>
		<a id="save-button" class="new-button green-button save-icon" href="#"><b><span></span>Salvar</b><i></i></a>
	</form>
</div>
<?php 	} ?>
		<div id="mypage-bg" class="<?php echo $bg; ?>">
			<div id="playground-outer">
				<div id="playground">

<?php

$get_em = mysql_query("SELECT * FROM homes_stickers WHERE groupid = '".$groupid."' and type < 4 LIMIT 200") or die(mysql_error());
while ($row = mysql_fetch_assoc($get_em)) {

	switch($row['type']){
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

?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($type == "stickie"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<div class="movable stickie n_skin_<?php echo $row['skin']; ?>-c" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;" id="stickie-<?php echo $row['id']; ?>">
	<div class="n_skin_<?php echo $row['skin']; ?>" >
		<div class="stickie-header">
			<h3><?php echo $edit; ?></h3>
			<div class="clear"></div>
		</div>
		<div class="stickie-body">
			<div class="stickie-content">
				<div class="stickie-markup"><?php echo bbcode_format(nl2br(HoloText($row['data']))); ?></div>
				<div class="stickie-footer">
				</div>
			</div>
		</div>
	</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($type == "sticker"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<div class="movable sticker s_<?php echo $row[data]; ?>" style="left: <?php echo $row[x]; ?>px; top: <?php echo $row[y]; ?>px; z-index: <?php echo $row[z]; ?>" id="sticker-<?php echo $row[id]; ?>"><?php echo $edit; ?></div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($type == "widget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<?php

switch($row['subtype']){
case "1": $subtype = "Profilewidget"; break;
case "3": $subtype = "MemberWidget"; break;
case "4": $subtype = "GuestbookWidget"; break;
case "5": $subtype = "TraxPlayerWidget";
}

?>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($subtype == "Profilewidget"){ $found_profile = true; ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<div class="movable widget GroupInfoWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline"><h3><span class="header-left">&nbsp;</span><span class="header-middle">Informaci&oacute;n del grupo</span><span class="header-right"><?php echo $edit; ?></span></h3>
		</div>
	</div>
	<div class="widget-body">
		<div class="widget-content">

<div class="group-info-icon"><img src='<?php echo $path; ?>/habbo-imaging/badge.php?badge=<?php echo $groupdata['badge']; ?>' align='right' /></div>

<h4><?php echo HoloText($groupdata['name']); ?></h4>

<p>
Creado el: <strong><?php echo $groupdata['created']; ?></strong>
</p>

<p>
<strong><?php echo $members; ?></strong> membros
</p>

<?php 

if($groupdata['roomid'] != 0 OR $groupdata['roomid'] != "" OR $groupdata['roomid'] != " ") {

	$sql = mysql_query("SELECT caption FROM rooms WHERE id='".$groupdata['roomid']."' LIMIT 1");
	$roominfo = mysql_fetch_assoc($sql);

if($groupdata['roomid'] <> 0){

?>

<p><a href="<?php echo $path; ?>/client?forwardId=2&amp;roomId=<?php echo $groupdata['roomid']; ?>" onclick="HabboClient.roomForward(this, '<?php echo $groupdata['roomid']; ?>', 'private'); return false;" target="client" class="group-info-room"><?php echo HoloText($roominfo['name']); ?></a></p><?php } ?>

<?php } ?>

<div class="group-info-description"><?php echo HoloText($groupdata['description']); ?></div>

<script type="text/javascript">
    document.observe("dom:loaded", function() {
        new GroupInfoWidget('<?php echo $groupid; ?>', '<?php echo $row['id']; ?>');
    });
</script>

		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "MemberWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
		
<div class="movable widget MemberWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline"><h3><span class="header-left">&nbsp;</span><span class="header-middle">Membros (<span id="avatar-list-size"><?php echo $members; ?></span>)</span><span class="header-right"><?php echo $edit; ?></span></h3>
		</div>
	</div>
	<div class="widget-body">
		<div class="widget-content">

<div id="avatar-list-search">
<input type="text" style="float:left;" id="avatarlist-search-string"/>
<a class="new-button" style="float:left;" id="avatarlist-search-button"><b>Buscar</b><i></i></a>
</div>
<br clear="all">

<div id="avatarlist-content">

<?php

$bypass = true;
$widgetid = $row['id'];
include('./myhabbo/avatarlist_membersearchpaging.php');

?>

<script type="text/javascript">
document.observe("dom:loaded", function() {
window.widget<?php echo $row['id']; ?> = new MemberWidget('<?php echo $groupid; ?>', '<?php echo $row['id']; ?>');
});
</script>

</div>
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

if($row['10'] == "0"){;
	$status = "public";
}else{
	$status = "private";
}

?>

<div class="movable widget GuestbookWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
		<div class="widget-headline"><h3>
		<?php echo $edit; ?>
		<span class="header-left">&nbsp;</span><span class="header-middle">Meu Livro de Visitas(<span id="guestbook-size"><?php echo $count; ?></span>) <span id="guestbook-type" class="<?php echo $status; ?>"><img src="./web-gallery/images/groups/status_exclusive.gif" title="Friends only" alt="Friends only"/></span></span><span class="header-right">&nbsp;</span></h3>
		</div>	
	</div>
	<div class="widget-body">
		<div class="widget-content">
<div id="guestbook-wrapper" class="gb-public">
<ul class="guestbook-entries" id="guestbook-entry-container">
	
<?php if($count == 0){ ?>
	<div id="guestbook-empty-notes">Este livro está vazio.</div>
<?php } else {

$sql123 = mysql_query("SELECT * FROM group_details WHERE id = '".FilterText($_GET['id'])."' LIMIT 1");
$grouprrow = mysql_fetch_assoc($sql123);
$i = 0;
while ($row1 = mysql_fetch_assoc($sql)) {
$i++;
$userrow = mysql_fetch_assoc(mysql_query("SELECT * FROM users WHERE id = '".$row1['userid']."' LIMIT 1"));

if($my_id == $row1['userid']){
	$owneronly = "<img src=\"./web-gallery/images/myhabbo/buttons/delete_entry_button.gif\" id=\"gbentry-delete-".$row1['id']."\" class=\"gbentry-delete\" style=\"cursor:pointer\" alt=\"\"/><br/>";
} elseif($grouprrow['ownerid'] == $my_id) {
	$owneronly = "<img src=\"./web-gallery/images/myhabbo/buttons/delete_entry_button.gif\" id=\"gbentry-delete-".$row1['id']."\" class=\"gbentry-delete\" style=\"cursor:pointer\" alt=\"\"/><br/>";
} else {
	$owneronly = "";
}

if($row1['online'] == "1"){ $useronline = "online"; } else { $useronline = "offline"; } 

?>

<li id="guestbook-entry-<?php echo $row1['id']; ?>" class="guestbook-entry">
		<div class="guestbook-author">
			<img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=<?php echo $userrow['look']; ?>&direction=2&head_direction=2&gesture=sml&size=s" alt="<?php echo $userrow['username']; ?>" title="<?php echo $userrow['username']; ?>"/>
		</div>
			<div class="guestbook-actions">
					<?php echo $owneronly; ?>
			</div>
		<div class="guestbook-message">
			<div class="<?php echo $useronline; ?>">
				<a href="<?php echo $path; ?>/home/<?php echo $userrow['username']; ?>"><?php echo $userrow['username']; ?></a>
			</div>
			<p><?php echo HoloText($row1['message'],false ,true); ?></p>
		</div>
		<div class="guestbook-cleaner">&nbsp;</div>
		<div class="guestbook-entry-footer metadata"><?php echo $userrow['time']; ?></div>
	</li>

<?php } } ?>

</ul></div>

<?php if($edit_mode == false){ ?>
	<div class="guestbook-toolbar clearfix">
	<a href="#" class="new-button envelope-icon" id="guestbook-open-dialog">
	<b><span></span>Postar uma nova mensagem</b><i></i>
	</a>
	</div>
<?php } ?>
<script type="text/javascript">	
	document.observe("dom:loaded", function() {
		var gb<?php echo $row['id']; ?> = new GuestbookWidget('17570', '<?php echo $row['id']; ?>', 500);
		var editMenuSection = $('guestbook-privacy-options');
		if (editMenuSection) {
			gb<?php echo $row['id']; ?>.updateOptionsList('public');
		}
	});
</script>
		<div class="clear"></div>
		</div>
	</div>
</div>
</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php } elseif($subtype == "TraxPlayerWidget"){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

<?php 

$sql123 = mysql_query("SELECT * FROM groups_details WHERE id = '".FilterText($_GET['id'])."' LIMIT 1");
$grouprrow = mysql_fetch_assoc($sql123);

?>

<div class="movable widget TraxPlayerWidget" id="widget-<?php echo $row['id']; ?>" style=" left: <?php echo $row['x']; ?>px; top: <?php echo $row['y']; ?>px; z-index: <?php echo $row['z']; ?>;">
<div class="w_skin_<?php echo $row['skin']; ?>">
	<div class="widget-corner" id="widget-<?php echo $row['id']; ?>-handle">
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
	<img src="<?php echo $path; ?>/web-gallery/images/traxplayer/player.png"/>
</div>

<div id="edit-menu-trax-select-temp" style="display:none">
    <select id="trax-select-options-temp">
    <option value="">- Seleccionar canciones -</option>

<?php

$mysql = mysql_query("SELECT * FROM furniture WHERE ownerid = '".$grouprrow['ownerid']."'");
$i = 0;
while($machinerow = mysql_fetch_assoc($mysql)){
$i++;
$sql = mysql_query("SELECT * FROM soundmachine_songs WHERE machineid = '".$machinerow['id']."'");
$n = 0;
while($songrow = mysql_fetch_assoc($sql)){
$n++;
if($songrow['id'] <> ""){
	echo "		<option value=\"".$songrow['id']."\">".trim(nl2br(HoloText($songrow['title'])))."</option>\n"; }
}

} ?>
</select>

</div>
<?php }elseif($songselected == false){ ?>
No hay canciones seleccionadas.
<?php 

}else{
	$sql1 = mysql_query("SELECT * FROM soundmachine_songs WHERE id = '".$row['8']."' LIMIT 1");
	$songrow1 = mysql_fetch_assoc($sql);

?>
<div id="traxplayer-content" style="text-align:center;"></div>
<embed type="application/x-shockwave-flash"
src="<?php echo $path; ?>/web-gallery/flash/traxplayer/traxplayer.swf" name="traxplayer" quality="high"
base="<?php echo $path; ?>/web-gallery/flash/traxplayer/" allowscriptaccess="always" menu="false"
wmode="transparent" flashvars="songUrl=<?php echo $path; ?>/myhabbo/trax_song.php?songId=<?php echo $row['var']; ?>&amp;sampleUrl=http://images.habbohotel.com/dcr/hof_furni//mp3/" height="66" width="210" />
<?php } ?>

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
&nbsp;
</div>
    </div>
			</div>

		</div>
	</div>
</div>

<script language="JavaScript" type="text/javascript">
initEditToolbar();
initMovableItems();
document.observe("dom:loaded", initDraggableDialogs);
</script>


<div id="edit-save" style="display:none;"></div>
    </div>

<?php require_once('./templates/login_footer.php'); ?>
</div>

</div>

<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>
<?php if($edit_mode){ ?>
<?php //////////////////////////////////////////////////////////////////////////////////////////////////// ?>

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
			<option value="8" id="edit-menu-skins-select-hc_pillowskin">HC para boys</option>
			<option value="7" id="edit-menu-skins-select-hc_machineskin">HC para girls</option>
		<?php } ?>
		<?php if($user_rank > 4){ ?>
			<option value="9" id="edit-menu-skins-select-nakedskin">Staff</option>
		<?php } ?>
			<option value="3" id="edit-menu-skins-select-metalskin">Metal</option>
			<option value="5" id="edit-menu-skins-select-notepadskin">Nota-etiqueta</option>
			<option value="2" id="edit-menu-skins-select-speechbubbleskin">Bocadillo de di&aacute;logo</option>
			<option value="4" id="edit-menu-skins-select-noteitskin">Bloco de notas</option>
	</select>
				</div>
				<div id="edit-menu-stickie">
					<p>Atenção! Se você clicar em Apagar esta nota será apagada para sempre.</p>
				</div>
				<div id="rating-edit-menu">
					<input type="button" id="ratings-reset-link"
						value="Wertung Zur&uuml;cksetzen" />
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
					<p>O item é de outro usuário e retornar ao seu inventário.</p>
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
					<input type="button" id="edit-menu-remove-button" value="L&ouml;schen" />
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
<?php } else { ?>
<div class="cbb topdialog" id="guestbook-form-dialog">
	<h2 class="title dialog-handle">Editar entrada</h2>

	<a class="topdialog-exit" href="#" id="guestbook-form-dialog-exit">X</a>
	<div class="topdialog-body" id="guestbook-form-dialog-body">
<div id="guestbook-form-tab">
<form method="post" id="guestbook-form">
    <p>
        Nota: comprimento máximo da mensagem: 200 caracteres
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
            "cyan" : ["#00c6c4", "Ciano"],
            "blue" : ["#0070d7", "Azul"],
            "gray" : ["#828282", "Cinza"],
            "black" : ["#000000", "Preto"]
        };
        bbcodeToolbar.addColorSelect("Farben", colors, true);
    </script>
    </div>

	<div class="guestbook-toolbar clearfix">
		<a href="#" class="new-button" id="guestbook-form-cancel"><b>Cancelar</b><i></i></a>
		<a href="#" class="new-button" id="guestbook-form-preview"><b>Visualização</b><i></i></a>
	</div>
</form>
</div>

<div id="guestbook-preview-tab">&nbsp;</div>
	</div>
</div>

<div class="cbb topdialog" id="guestbook-delete-dialog">
	<h2 class="title dialog-handle">Eliminar entrada</h2>

	<a class="topdialog-exit" href="#" id="guestbook-delete-dialog-exit">X</a>
	<div class="topdialog-body" id="guestbook-delete-dialog-body">
	<form method="post" id="guestbook-delete-form">
	<input type="hidden" name="entryId" id="guestbook-delete-id" value="" />
	<p>Tem certeza de que deseja excluir a entrada?</p>
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
	<li><a href="<?php echo $path; ?>/groups/<?php echo $groupid; ?>&do=edit" id="group-tools-style">Editar Página</a></li>
	<?php if($ownerid == $my_id){ ?><li><a href="#" id="group-tools-settings">Configuração</a></li><?php } ?>
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
	<li id="group-settings-link-forum"><a href="#">Ajustes do fórum</a><span class="tab-spacer"></span></li>
	<li id="group-settings-link-room"><a href="#">Ajustes da sala</a><span class="tab-spacer"></span></li>
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
</script><div class="cbb topdialog black" id="group-memberlist">

	<div class="box-tabs-container">
<ul class="box-tabs">
	<li class="selected" id="group-memberlist-link-members"><a href="#">Membros</a><span class="tab-spacer"></span></li>
	<li id="group-memberlist-link-pending"><a href="#">Membos atuais</a><span class="tab-spacer"></span></li>
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
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-give-rights"><b>Dê permissões</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-revoke-rights"><b>Remover Permissões</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-remove"><b>Remover</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button" id="group-memberlist-button-close"><b>Fechar</b><i></i></a>
</div>
<div id="group-memberlist-pending" style="clear: both"></div>
<div id="group-memberlist-pending-buttons" class="clearfix">
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-accept"><b>Aceitar</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button-disabled" id="group-memberlist-button-decline"><b>Rejeitar</b><i></i></a>
	<a href="#" class="new-button group-memberlist-button" id="group-memberlist-button-close2"><b>Fechar</b><i></i></a>
</div>
	</div>
</div>
<?php } ?>

<script type="text/javascript">
HabboView.run();
</script>

<?php echo $analytics; ?>
</body>
</html>


<?php 

} else {

$pagename = "Página do grupo não localizado";
require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');
?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix red "><h2 class="title">Página do grupo não localizado</h2>
<div id="notfound-content" class="box-content">
    <p class="error-text">A página solicitada não pôde ser encontrado.</p> <img id="error-image" src="./web-gallery/v2/images/error.gif" />
    <p class="error-text">Clique <a href="javascript:history.back()">aqu&iacute;</a> para voltar à página anterior.</p>
    <p class="error-text"><b>Buscar grupo</b></p>
    <?php if(isset($searchString)){ echo "<p class=\"error-text\">Não existe nenhum grupo chamado <strong>'".$searchString."'.</strong></p>"; } ?>
    <p class="error-text">
	<form method='post'>
		Nome do grupo:<br />
		<input type='text' name='searchString' maxlength='25' size='25' value='<?php echo FilterText($_POST['searchString']); ?>'>
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