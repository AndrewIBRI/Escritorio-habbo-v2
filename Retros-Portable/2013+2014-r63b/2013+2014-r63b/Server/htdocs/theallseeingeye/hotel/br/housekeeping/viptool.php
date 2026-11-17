<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Usuários VIP";
$pageid = "viptool";

if($do == "delete" && $_GET['key']){

	$check = mysql_query("SELECT * FROM vip WHERE id_user = '".$key."'");
	if(mysql_num_rows($check) > 0){

		$user_check = mysql_query("SELECT * FROM users WHERE id = '".$key."' LIMIT 1");
		$userdata = mysql_fetch_assoc($user_check);

		mysql_query("DELETE FROM vip WHERE id_user = '".$key."'") or die(mysql_error());
		mysql_query("DELETE FROM user_badges WHERE user_id = '".$key."' AND badge_id = 'VIP'") or die(mysql_error());
		mysql_query("UPDATE users SET rank = '1', vip = '0' WHERE id = '".$key."'") or die(mysql_error());
                mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$userdata['id']."','Você agora é parte do Hobba VIP!')");
		$msg = "<div class='rounded rounded-green'><center>".$userdata['username']." Foi removido da lista de usuários VIP <img src=\"./images/check.gif\"></center></div>";

	}else{
		$msg = "<div class='rounded rounded-red'><center>Não foi possível encontrar este usuário.<img src=\"./images/del.gif\"></center></div>";
	}

}elseif($do == "add" && $_POST['vip']){

	// $timened = time() + 15552000;
	$timeend2 = time() + 1296000 * 2;

	$user_check = mysql_query("SELECT * FROM users WHERE username = '".$_POST['vip']."' LIMIT 1");
	$userdata = mysql_fetch_assoc($user_check);

	$badge_check = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' and badge_id = 'VIP'");

	$vip_check = mysql_query("SELECT * FROM vip WHERE id_user = '".$userdata['id']."' LIMIT 1");
	$vip = mysql_fetch_assoc($vip_check);

	if(mysql_num_rows($user_check) > 0){

		if(mysql_num_rows($vip_check) > 0){

			mysql_query("UPDATE vip SET timestampend = '".$vip['timestampend']."' + 15552000 WHERE id_user = '".$userdata['id']."'") or die(mysql_error());
			$msg = "<div class='rounded rounded-green'><center>".$_POST['vip']." a&ntilde;adir seis meses m&aacute;s <img src=\"./images/check.gif\"></center></div>";
	
		} else {

			if(mysql_num_rows($badge_check) < 1){
				mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$userdata['id']."','VIP','0')") or die(mysql_error());
			}

			mysql_query("UPDATE users SET vip_points = vip_points + 70000, rank = '2', vip = '1' WHERE id = '".$userdata['id']."'") or die(mysql_error());
			mysql_query("INSERT INTO vip (id_user,timestamp,timestampend) VALUES ('".$userdata['id']."','".time()."','".$timeend2."')") or die(mysql_error());
			mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$userdata['id']."','Você agora faz parte do Hobba VIP!')");
	
	
			$msg = "<div class='rounded rounded-green'><center>".$_POST['vip']." Recebeu o VIP corretamente! <img src=\"./images/check.gif\"></center></div><br>";
		}
	} else {
		$msg = "<div class='rounded rounded-red'><center>Não foi possível encontrar este usuário! <img src=\"./images/del.gif\"></center></div><br>";
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/viptool&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Usuários VIP</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nome de usuário</strong><div class='graytext'>Digite o nome do usuário que tornará VIP</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='vip' value="<?php echo $_POST['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Torná-lo VIP' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Usuários VIP atualmente</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='20%' align='center'>Nome de usuário</td>
  <td class='tablesubheader' width='15%' align='center'>E-mail</td>
  <td class='tablesubheader' width='15%' align='center'>IP</td>
  <td class='tablesubheader' width='10%' align='center'>VIP desde</td>
  <td class='tablesubheader' width='10%' align='center'>Termina em</td>
  <td class='tablesubheader' width='5%' align='center'>Editar usuário</td>
 </tr>

<?php

$get_vip = mysql_query("SELECT * FROM vip ORDER BY timestampend") or die(mysql_error());
while($vip = mysql_fetch_assoc($get_vip)){

$get_user = mysql_query("SELECT * FROM users WHERE id = '".$vip['id_user']."'");
while($row = mysql_fetch_assoc($get_user)){

if($row['online'] >= 1){ $online = "online"; } else { $online = "offline"; } 

?>

<tr>
  <td class='tablerow1' align='left'><?php echo $row['username']; ?> (ID: <?php echo $row['id']; ?>)</td>
  <td class='tablerow1' align='left'><?php echo $row['mail']; ?></td>
  <td class='tablerow1' align='left'><?php echo $row['ip_last']; ?></td>
  <td class='tablerow1' align='left'><?php echo date('d/m/Y', $vip['timestamp']); ?></td>
  <td class='tablerow1' align='left'><?php echo date('d/m/Y', $vip['timestampend']); ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/users_edit&key=<?php echo $vip['id_user']; ?>'><img src='./images/edit.gif' alt='Edit User Data'></a> <a href='<?php echo $adminpath; ?>/index/p/viptool&do=delete&key=<?php echo $vip['id_user']; ?>'><img src='./images/del.gif' alt='Edit User Data'></a></td>
</tr>

<?php } } ?>
 
 </table>
</div>
	 </div>
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>