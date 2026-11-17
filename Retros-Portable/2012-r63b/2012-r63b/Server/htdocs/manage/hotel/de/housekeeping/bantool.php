<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Baneos";
$pageid = "ban";

if(isset($_POST['value']) && ($_POST['length']) && ($_POST['reason'])){

$check_exists = mysql_query("SELECT * FROM bans WHERE value = '".$_POST['value']."' AND bantype = '".$_POST['type']."'");

$get_cc = mysql_query("SELECT * FROM users WHERE username = '".$_POST['value']."'");
$userdata = mysql_fetch_assoc($get_cc);

	if(mysql_num_rows($check_exists) > 0){
		mysql_query("UPDATE bans SET expire = expire + '".$_POST['length']."' WHERE value = '".$_POST['value']."' AND bantype = '".$_POST['type']."'");
		mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Bann geändert','bantool.php','".$my_id."','".$userdata['id']."','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>El ban (".$_POST['type']." - ".$_POST['value'].") ha sido actualizado. <img src=\"./images/check.gif\"></center></div>";
	}else{


			if(mysql_num_rows($get_cc) > 0 && $_POST['type'] == "user"){
				mysql_query("INSERT INTO bans (bantype,value,reason,expire,added_by,added_date) VALUES ('".$_POST['type']."','".$_POST['value']."','".$_POST['reason']."','".time()."' + '".$_POST['length']."','".$name."','".time()."')") or die(mysql_error());
				mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Bann','bantool.php','".$my_id."','".$userdata['id']."','".$date_full."')") or die(mysql_error());
				mysql_query("UPDATE users SET auth_ticket = '' WHERE username = '".$_POST['value']."' LIMIT 1") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center>".$_POST['value']." ha sido a&ntilde;dido <img src=\"./images/check.gif\"></center></div>";

			}elseif(mysql_num_rows($get_cc) < 1 && $_POST['type'] == "user"){
				$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar al usuario <img src=\"./images/del.gif\"></center></div>";
			}elseif($_POST['type'] == "ip"){
				mysql_query("INSERT INTO bans (bantype,value,reason,expire,added_by,added_date) VALUES ('".$_POST['type']."','".$_POST['value']."','".$_POST['reason']."','".time()."' + '".$_POST['length']."','".$name."','".time()."')") or die(mysql_error());
				mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Bann','bantool.php','".$my_id."','".$userdata['id']."','".$date_full."')") or die(mysql_error());
				mysql_query("UPDATE users SET auth_ticket = '' WHERE username = '".$_POST['value']."' LIMIT 1") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center>La IP ".$_POST['value']." ha sido baneada. <img src=\"./images/check.gif\"></center></div>";

			}
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/ban&do=banthebastard' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Banear usuario</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<td class='tablerow1'  width='30%'  valign='middle'><b>Nombre</b></td>
<td class='tablerow2'  width='70%'  valign='middle'><input type='text' name='value' value="<?php echo $_POST['value']; ?>" size='50' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='30%'  valign='middle'><b>Raz&oacute;n</b></td>
<td class='tablerow2'  width='70%'  valign='middle'><input type='text' name='reason' value="<?php echo $_POST['reason']; ?>" size='50' class='textinput'></td>
</tr>

<script type="text/javascript">
function banPreset(val)
{
	document.getElementById('banlength').value = val;
}
</script>


<tr>
<td class='tablerow1'  width='30%'  valign='middle'><b>Duraci&oacute;n</b></td>
<td class='tablerow2'  width='70%'  valign='middle'>
<input type="text" name="length" size='50' id="banlength" value="<?php echo $_POST['length']; ?>"> (segundos)<br /><br>
<small>
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(3600);">1 hora,</a>
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(7200);">2 horas,</a>
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(10800);">3 horas,</a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(14400);">4 horas,</a>
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(43200);">12 horas,<br><br></a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(86400);">1 dia,</a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(259200);">3 dias,<br><br></a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(604800);">1 semana,</a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(1209600);">2 semanas,<br><br></a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(2592000);">1 mes,</a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(7776000);">3 meses,<br><br></a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(1314000);">1 a&ntilde;o,</a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(2628000);">2 a&ntilde;os,</a> 
<a href="<?php echo $adminpath; ?>/index/p/ban#" onclick="banPreset(360000000);">> 10 a&ntilde;os</a></small><br />

</td>
</tr>

<tr>
<td class='tablerow1'  width='30%'  valign='middle'><b>A&ntilde;adido por:</b></td>
<td class='tablerow2'  width='70%'  valign='middle'><input type='text'disabled='disabled' name='added_by' value="<?php echo $name; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo:</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type' class='dropdown'><option value='user'>Usuario</option><option value='ip' <?php if($_POST['type'] == "ip"){ echo"selected='selected'"; } ?>>IP</option></select>
</td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Banear' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>