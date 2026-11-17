<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "LLamadas de ayuda";
$pageid = "users";

$posts = mysql_evaluate("SELECT COUNT(*) FROM moderation_tickets");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

if($_POST['site']){
	header("location: ".$adminpath."/index/p/helper&page=".$_POST['page']."");
}

if($_POST['site2']){
	header("location: ".$adminpath."/index/p/helper&page=".$_POST['page2']."");
}

$today = mysql_evaluate("SELECT count(*) FROM moderation_tickets WHERE timestamp > '".strtotime("today")."'");

@include('subheader.php');

?>

<form action='<?php echo $adminpath; ?>/index/p/helper&do=save' method='post' name='theAdminForm' id='theAdminForm'>

<div class='tableborder'>
<div class='tableheaderalt'><center><?php echo mysql_evaluate("SELECT COUNT(*) FROM moderation_tickets"); ?> Llamadas de ayuda // <?php echo mysql_evaluate("SELECT COUNT(*) FROM moderation_tickets WHERE moderator_id = '0'"); ?> sin ver // Hoy: <?php echo $today; ?>

<select name="page">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
	<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site" value="Weiter »">
</div>

<table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='5%'>Estado</td>	
  <td class='tablesubheader' width='5%'>De</td>
  <td class='tablesubheader' width='5%'>Informado</td>
  <td class='tablesubheader' width='5%'>Staff</td>
  <td class='tablesubheader' width='20%'>Mensaje</td>
  <td class='tablesubheader' width='10%'>Sala</td>
  <td class='tablesubheader' width='11%'>Fecha</td>
  <td class='tablesubheader' width='1%'>Total</td>
 </tr>

<?php

$query_min = ($page * 50) - 50;

if($query_min < 0){ 
	$query_min = 0;
}

if($do == "cautions" && $_GET['name']){
	$get_id = mysql_query("SELECT id FROM users WHERE username = '".$_GET['name']."'");
	if(mysql_num_rows($get_id) > 0){
		$get = mysql_fetch_assoc($get_id);
		$get_tickets = mysql_query("SELECT * FROM moderation_tickets WHERE reported_id = '".$get['id']."' ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
	}
} else { 
	$get_tickets = mysql_query("SELECT * FROM moderation_tickets ORDER BY id DESC LIMIT ".$query_min.", 50") or die(mysql_error());
}

while($row = mysql_fetch_assoc($get_tickets)){

$get_sender_id = mysql_fetch_assoc($get_reporter_id = mysql_query("SELECT username FROM users WHERE id = '".$row['sender_id']."'"));
$get_reported_id = mysql_fetch_assoc($get_reported_id = mysql_query("SELECT username FROM users WHERE id = '".$row['reported_id']."'"));
$get_moderator_id = mysql_fetch_assoc($get_reporter_id = mysql_query("SELECT username FROM users WHERE id = '".$row['moderator_id']."'"));

$sender_id = $get_sender_id['username'];

if($row['reported_id'] == "0"){
	$reported_id = "-/-";
} else {
	$reported_id = $get_reported_id['username'];
}

if($row['sender_id'] == "0"){
	$sender_id = "-/-";
} else {
	$sender_id = $get_sender_id['username'];
}

if($row['moderator_id'] == "0"){
	$moderator_id = "<font color='red'><b>ABIERTA</b></font>";
} else {
	$moderator_id = $get_moderator_id['username'];
}

?>
     
 <tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><?php echo $row['status']; ?></td>
  <td class='tablerow2'><?php echo $sender_id; ?></td>
  <td class='tablerow2'><?php echo $reported_id; ?></td>
  <td class='tablerow2'><?php echo $moderator_id; ?></td>
  <td class='tablerow2'><?php echo $row['message']; ?></td>
  <td class='tablerow2'><?php echo $row['room_name']; ?> (ID: <?php echo $row['room_id']; ?>)</td>
  <td class='tablerow2'><?php echo date('d.m.Y - H:i:s', $row['timestamp']); ?></td>
  <td class='tablerow2' align='center'><?php echo mysql_evaluate("SELECT COUNT(*) FROM moderation_tickets WHERE moderator_id = '".$row['moderator_id']."'"); ?></td>
 </tr>

<?php } ?>
</table>

<div class='tableborder'>
<div class='tableheaderalt'><center><?php echo mysql_evaluate("SELECT COUNT(*) FROM moderation_tickets"); ?> Llamdas de ayuda // <?php echo mysql_evaluate("SELECT COUNT(*) FROM moderation_tickets WHERE moderator_id = '0'"); ?> sin ver // Hoy: <?php echo $today; ?>

<select name="page2">
<?php for ($i = 1; $i <= $pages; $i++){ ?>
	<option value="<?php echo $i; ?>"<?php if($i == $page){ echo' selected="selected"'; } ?>><?php echo $i; ?></option>
<?php } ?>
</select>

<input type="submit" name="site2" value="Weiter »">
</div>

</div>
</div>

</table></div></div></div></div>

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>