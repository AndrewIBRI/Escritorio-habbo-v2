<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index/?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index/p/login"); exit; }

$pagename = "Alertas";
$pageid = "alert";

if(isset($_POST['alert'])){

	$check = mysql_query("SELECT * FROM users WHERE username = '".$_POST['name']."' LIMIT 1");
	if(mysql_num_rows($check) > 0){
		$userdata = mysql_fetch_assoc($check);
		mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$userdata['id']."','".$_POST['alert']."')");
		mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Alert gesendet','alert.php','".$my_id."','".$userdata['id']."','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Alerta enviada a  ".$_POST['name']." (ID: ".$userdata['id'].") <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - Usuario no encontrado <img src=\"./images/del.gif\"></center></div>";
	}

}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/alert' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Enviar alertas</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Usuario</b><div class='graytext'>&iexcl;Aseg&uacute;rate de que lo escribes bien!</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $_POST['name']; ?>" size='50' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Alerta</b><div class='graytext'>&iquest;Que dir&aacute; en la alerta?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='alert' cols='60' rows='5' wrap='soft' id='sub_desc' class='multitext'><?php echo $_POST['alert']; ?></textarea></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Enviar alerta' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br>	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>
