<?php

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

if($user_rank > 5){

$pagename = "Editar placas";
$pageid = "badgetool";

if(isset($_POST['badge']) && ($_POST['name'])){

	$check_name = mysql_query("SELECT * FROM users WHERE username = '".$_POST['name']."'") or die(mysql_error());
	if(mysql_num_rows($check_name) > 0){

		$userdata = mysql_fetch_assoc($check_name);

		$check_badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_id = '".$_POST['badge']."' LIMIT 1") or die(mysql_error());
			
		if($_POST['action'] == "give"){
	
			if(mysql_num_rows($check_badge) < 1){
				mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$userdata['id']."','".$_POST['badge']."','0')") or die(mysql_error());
				mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Badge ".$_POST['badge']." vergeben','badgetool.php','".$my_id."','".$userdata['id']."','".$date_full."')") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center>Le has dado a <b>".$_POST['name']."</b> la placa " . $_POST['badge'] . " con &eacute;xito. <img src=\"./images/check.gif\"></center></div>";
			} else {
				$msg = "<div class='rounded rounded-red'><center>".$_POST['name']." ya tiene la placa " . $_POST['badge'] . ". <img src=\"./images/del.gif\"></center></div>";
			}

		} else {

		if(mysql_num_rows($check_badge) > 0){
				mysql_query("DELETE FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_id = '".$_POST['badge']."'") or die(mysql_error());
				mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Badge ".$_POST['badge']." entfernt','badgetool.php','".$my_id."','".$userdata['id']."','".$date_full."')") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center>Le has quitado a <b>".$_POST['name']."</b> la placa " . $_POST['badge'] . " . <img src=\"./images/check.gif\"></center></div>";
			} else {
				$msg = "<div class='rounded rounded-red'><center>".$_POST['name']." no tiene la placa " . $_POST['badge'] . " <img src=\"./images/del.gif\"></center></div>";
			}

		}

	} else {
		$msg = "<div class='rounded rounded-red'><center>No se ha podido encontrar al usuario. <img src=\"./images/del.gif\"></center></div>";
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/badgetool&do=something' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Administrar placas</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nombre de usuario</strong><div class='graytext'>El miebro al que le quieres dar la placa.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $_POST['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>C&oacute;digo de la placa</strong><div class='graytext'>Ejemplo: ADM,HBL,DE5 usw...<br></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='badge' value="<?php echo $_POST['badge']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Opci&oacute;n:</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='action' class='dropdown'><option value='give'>Dar</option><option value='delete'>Quitar</option></select>
</td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Continuar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); }else{ require_once('error.php'); exit; } ?>