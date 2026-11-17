<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

if($do == "cleanlogs"){
	mysql_query("TRUNCATE TABLE stafflogs") or die(mysql_error());
	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Staff Logs wurde geleert','logs.php','".$my_id."','','".$date_full."')") or die(mysql_error());
}

$pagename = "Stafflogs";
$pageid = "stafflogs";

@include('subheader.php');

?>
 
 <div class='tableborder'>
 <div class='tableheaderalt'><center><?php echo $sitename; ?> ~ Housekeeping Staff Logs</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>

<?php 

if($do == "reading" && is_numeric($key)){

$get_details = mysql_query("SELECT * FROM stafflogs WHERE id = '".$key."' && details IS NOT NULL");
if(mysql_num_rows($get_details) > 0){ 

?>

 <tr>
  <td class='tablesubheader' width='20%' align='left'>Usuario</td>
  <td class='tablesubheader' width='80%' align='left'>Detalles</td>
 </tr>

<?php 

$details = mysql_fetch_assoc($get_details); 
$user = mysql_query("SELECT * FROM users WHERE id = '".$details['targetid']."'"); 
$user = mysql_fetch_assoc($user); 

?>
       
 <tr>
  <td class='tablerow1' align='left'><?php echo $user['name']; ?> (ID: <?php echo $user['id']; ?>)</td>
  <td class='tablerow2' align='left'><?php echo $details['details']; ?></td>
 </tr>

 <?php }else{ echo"<p><strong><div class='rounded rounded-red'><center><b>Error</b></center></div></p>"; } }else{ ?>

 <tr>
  <td class='tablesubheader' width='5%' align='left'>Sitio</td>
  <td class='tablesubheader' width='15%' align='left'>Staff</td>
  <td class='tablesubheader' width='15%' align='left'>Usuario</td>
  <td class='tablesubheader' width='20%'>Detalle</td>
  <td class='tablesubheader' width='20%'>Datos</td>
  <td class='tablesubheader' width='20%' align='left'>Fecha</td>
 </tr>
<?php

$get_users = mysql_query("SELECT * FROM stafflogs ORDER BY id DESC") or die(mysql_error());
while($row = mysql_fetch_assoc($get_users)){ 

?>

<script language="JavaScript" type="text/javascript">
function openWin(){
var newWin=window.open('', '', 'height=330, width=560');
newWin.document.close();
}
</script>

<?php

$userdata = mysql_query("SELECT * FROM users WHERE id = '".$row['userid']."' LIMIT 1");
$userdata = mysql_fetch_assoc($userdata);

if(!empty($row['targetid'])){
	$targetdata = mysql_query("SELECT * FROM users WHERE id = '".$row['targetid']."' LIMIT 1");
	$targetdata = mysql_fetch_assoc($targetdata);
} else {
	$targetdata['username'] = "-/-";
}
	
if(!empty($row['note'])){ 
	$note = $row['note'];
} else {
	$note = "<i>None given</i>";
}

?>

<tr>
	<td class='tablerow1' align='left'><?php echo $row['action']; ?></td>
	<td class='tablerow2' align='left'><?php echo $userdata['username']?> (ID: <?php echo $row['userid']; ?>)</td>
	<td class='tablerow2' align='left'><?php echo $targetdata['username']; if(!empty($row['targetid'])){ echo" (ID: ".$row['targetid'].")"; } ?></td>
	<td class='tablerow2'><?php echo $row['message']; ?> <?php if($note == "users_edit.php"){ ?> <a href="<?php echo $adminpath; ?>/index/p/stafflogs&do=reading&key=<?php echo $row['id']; ?>" target="2941aeb24b02737dbd8720de2ebc95e8e240c39b" onclick="HabboClient.openOrFocus(this); return false;">[DETAILS]</a><?php } ?></td>
	<td class='tablerow2'><?php echo $note; ?></td>
	<td class='tablerow2' align='left'><?php echo $row['timestamp']; ?></td>
</tr>

<?php } ?>
 
</table>
</div>
<br />
<div align='center'>(<a href='<?php echo $adminpath; ?>/index/p/stafflogs&do=cleanlogs'><b>Limpiar log</b></a>)</div>

</div>
<?php } ?>

<!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); }else{ include('error.php'); exit; } ?>