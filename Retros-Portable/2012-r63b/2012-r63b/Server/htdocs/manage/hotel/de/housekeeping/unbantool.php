<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Desbanear";
$pageid = "unban";

if(isset($_POST['query'])){

	if($_POST['type'] == 'ip'){
		$check_ip = mysql_query("SELECT * FROM bans WHERE value = '".$_POST['query']."' AND bantype = 'ip'");
		if(mysql_num_rows($check_ip) > 0){
			mysql_query("DELETE FROM bans WHERE value = '".$_POST['query']."' AND bantype = 'ip'");
			mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Unbann / Typ: IP: [".FilterText($_POST['query'])."]','unbantool.php','".$my_id."','','".$date_full."')") or die(mysql_error());
			$msg = "<div class='rounded rounded-green'><center> (" . mysql_num_rows($check_ip). ") Desbaneado correctamente. <img src=\"./images/check.gif\"></center></div>";
		}else{
			$msg = "<div class='rounded rounded-red'><center>No se han podido encontrar baneos <img src=\"./images/del.gif\"></center></div>";
		}
	}else{
		$check_user = mysql_query("SELECT * FROM bans WHERE value = '".$_POST['query']."' AND bantype = 'user'");
		if(mysql_num_rows($check_user) > 0){

			mysql_query("DELETE FROM bans WHERE value = '".$_POST['query']."' AND bantype = 'user'");
			mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Unbann / Typ: User: [".FilterText($_POST['query'])."]','unbantool.php','".$my_id."','','".$date_full."')") or die(mysql_error());
			$msg = "<div class='rounded rounded-green'><center>(" . mysql_num_rows($check_user). ") Desbaneado correctamente. <img src=\"./images/check.gif\"></center></div>";
		}else{
			$msg = "<div class='rounded rounded-red'><center>No se ha podido encontrar el baneo. <img src=\"./images/del.gif\"></center></div>";
		}
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/unban&do=unban' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Herramienta desbanear</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
    
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Usuario/IP</b><div class='graytext'>Escribe el nombre de usuario o IP que deseas desbanear</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='query' value="<?php echo $_POST['query']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Tipo:</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='type' class='dropdown'><option value='user'>Usuario</option><option value='ip'>IP</option></select>
</td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Unbannen' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>