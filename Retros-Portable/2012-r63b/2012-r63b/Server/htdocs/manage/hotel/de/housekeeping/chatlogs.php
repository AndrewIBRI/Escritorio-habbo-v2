<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Chatlogs";
$pageid = "chatlogs";

if(isset($_POST['query'])){

$limit = $_POST['limit'];

	if($_POST['type'] == "user_id"){

			$userdata = mysql_query("SELECT * FROM users WHERE username = '".$_POST['query']."'");
			if(mysql_num_rows($userdata) > 0){

			$userdata = mysql_fetch_assoc($userdata);
			$get_logs = mysql_query("SELECT * FROM chatlogs WHERE user_id = '".$userdata['id']."' ORDER BY timestamp DESC LIMIT ".$limit."");
			$results = mysql_num_rows($get_logs);

			if($results > 0){

				$chatlogs = mysql_fetch_assoc($get_logs);

				$msg = "<div class='rounded rounded-green'><center>Reegistros de <u>".$_POST['query']." (ID: ".$userdata['id'].")</u> <img src=\"./images/check.gif\"></center></div>";
		} else {
			$msg = "<div class='rounded rounded-red'><center>Error - Log no encontrado <img src=\"./images/del.gif\"></center></div>";
		}

			}else{
				$msg = "<div class='rounded rounded-red'><center>Error - El usuario no existe <img src=\"./images/del.gif\"></center></div>";
			}

	} else {

		$get_logs = mysql_query("SELECT * FROM chatlogs WHERE room_id = '".$_POST['query']."' ORDER BY timestamp LIMIT ".$limit."");
		$results = mysql_num_rows($get_logs);

		if($results > 0){

			$chatlogs = mysql_fetch_assoc($get_logs);
			$roomdata = mysql_query("SELECT * FROM rooms WHERE id = '".$_POST['query']."'");
			$data = mysql_fetch_assoc($roomdata);

			$msg = "<div class='rounded rounded-green'><center>Log de la sala <u>".$data['caption']." (ID: ".$data['id'].")</u> <img src=\"./images/check.gif\"></center></div>";
		} else {
			$msg = "<div class='rounded rounded-red'><center>Error - Log no encontrado <img src=\"./images/del.gif\"></center></div>";
		}
	}

}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/chatlogs&do=search' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Chatlogs</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nombre de usuario o ID de la sala</strong><div class='graytext'>Si quieres buscar por una sala pon la ID, si quieres buscar por un usuario pon su nombre.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='query' value="<?php echo $_POST['query']; ?>" size='35' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Tipo</strong></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="type">
<option value='room_id'>ID de la sala</option>
<option value="user_id"<?php if($_POST['type'] == "user_id"){ ?> selected="selected"<?php } ?>>Nombre de usuario</option></select></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Limite</strong><div class='graytext'>&iquest;Cuantas entradas desea ver? <font color="red"><br>Contra m&aacute;s entradas, m&aacute;s tendr&aacute;s que esperar.</font></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name="limit">
<option value='10'>10</option>
<option value='25'>25</option>
<option value='50'>50</option>
<option value='75'>75</option>
<option value='100'>100</option>
<option value='150'>150</option>
<option value='200'>200</option>
<option value='300'>300</option>
<option value='400'>400</option>
<option value='500'>500</option>
<option value='1000'>1000</option>
</select></td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Suchen' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<?php if($results > 0 && $_POST['type'] == "user_id"){ ?>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Chatlog del usuario <?php echo $userdata['username']; ?> - Limite <?php echo $limit; ?></center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>Usuario</td>
  <td class='tablesubheader' width='20%' align='center'>Sala</td>
  <td class='tablesubheader' width='21%' align='center'>Fecha</td>
  <td class='tablesubheader' width='59%' align='center'>Mensaje</td>
 </tr>
<?php

$logs = mysql_query("SELECT * FROM chatlogs WHERE user_id = '".$userdata['id']."' ORDER BY timestamp DESC LIMIT ".$limit."");
while($rowlogs = mysql_fetch_assoc($logs)){

$get_room = mysql_query("SELECT * FROM rooms WHERE id = '".$rowlogs['room_id']."'") or die(mysql_error());
$rooms = mysql_fetch_assoc($get_room);

?>

<tr>
  <td class='tablerow1'><?php echo $userdata['username']; ?></td>
  <td class='tablerow2'><?php echo $rooms['caption']; ?> (ID: <?php echo $rowlogs['room_id']; ?>)</td>
  <td class='tablerow2''><?php echo date('d.m.Y - H:i:s', $rowlogs['timestamp']); ?></td>
  <td class='tablerow2'><?php echo FilterText($rowlogs['message']); ?></td>
</tr>

<?php } }elseif($results > 0 && $_POST['type'] == "room_id"){ ?>


 <div class='tableborder'>
 <div class='tableheaderalt'><center>Chatlogs de la sala <?php echo $roomdata['caption']; ?> - Limite <?php echo $limit; ?></center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>Usuario</td>
  <td class='tablesubheader' width='20%' align='center'>Sala</td>
  <td class='tablesubheader' width='21%' align='center'>Fecha</td>
  <td class='tablesubheader' width='59%' align='center'>Mensaje</td>
 </tr>
<?php

$logs = mysql_query("SELECT * FROM chatlogs WHERE room_id = '".$_POST['query']."' ORDER BY timestamp DESC LIMIT ".$limit."");
while($rowlogs = mysql_fetch_assoc($logs)){

$get_user = mysql_query("SELECT * FROM users WHERE id = '".$rowlogs['user_id']."'") or die(mysql_error());
$users = mysql_fetch_assoc($get_user);

$get_room = mysql_query("SELECT * FROM rooms WHERE id = '".$rowlogs['room_id']."'") or die(mysql_error());
$rooms = mysql_fetch_assoc($get_room);

?>

<tr>
  <td class='tablerow1'><?php echo $users['username']; ?></td>
  <td class='tablerow2'><?php echo $rooms['caption']; ?> (ID: <?php echo $rowlogs['room_id']; ?>)</td>
  <td class='tablerow2''><?php echo date('d.m.Y - H:i:s', $rowlogs['timestamp']); ?> Uhr</td>
  <td class='tablerow2'><?php echo FilterText($rowlogs['message']); ?></td>
</tr>

<?php } } ?>

</table>
</div>
</div>

<!-- / RIGHT CONTENT BLOCK -->
 </td>
</tr>
</table>
</div>

<!-- / OUTERDIV -->
<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>