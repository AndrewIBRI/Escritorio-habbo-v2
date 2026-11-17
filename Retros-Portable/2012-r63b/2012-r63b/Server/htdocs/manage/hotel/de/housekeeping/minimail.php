<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Minimail";
$pageid = "minimail";

if(isset($_POST['body'])){

        $userdata = mysql_query("SELECT id FROM users WHERE username = '".$_POST['name']."'");

	if(mysql_num_rows($userdata) > 0){

        	$userdata = mysql_fetch_assoc($userdata);

		mysql_query("INSERT INTO cms_minimail (sender_id,receiver_id,folder,is_read,subject,date,isodate,timestamp,body) VALUES ('".$my_id."','".$userdata['id']."','inbox','0','".$_POST['subject']."','".$date_full."','0','0','".$_POST['body']."')") or die(mysql_error());
		mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Minimail an ".$row['name']." gesendet','minimail.php','".$my_id."','".$recipientIds."','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Minimail enviado a ".$_POST['name']." enviado correctamente. <img src=\"./images/check.gif\"></center></div>";
	}else{
		$msg = "<div class='rounded rounded-red'><center>Error - Usuario no encontrado <img src=\"./images/del.gif\"></center></div>";
	}

}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>
 
<form action='./index/p/minimail&do=placeAlert' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Env&iacute;o de Minimail</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre de usuario</b><div class='graytext'>&iquest;A qui&eacute;n quieres envi&aacute;rselo?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' size='50' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Asunto</b><div class='graytext'>Title del Minimail.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='subject' size='50' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Descripci&oacute;n</b><div class='graytext'>Texto del minimail</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='body' cols='60' rows='5' wrap='soft' id='sub_desc' class='multitext'></textarea></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Enviar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>