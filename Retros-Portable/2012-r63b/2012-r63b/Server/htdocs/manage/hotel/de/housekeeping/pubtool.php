<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Anunciantes";
$pageid = "pubtool";

if($do == "delete" && $_GET['key']){

	$check = mysql_query("SELECT * FROM publicistas WHERE user_name = '".$key."'");
	if(mysql_num_rows($check) > 0){

		$user_check = mysql_query("SELECT * FROM users WHERE username = '".$key."' LIMIT 1");
		$userdata = mysql_fetch_assoc($user_check);

		mysql_query("DELETE FROM publicistas WHERE user_name = '".$key."'");
		mysql_query("DELETE FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_id = 'PUB1'");
		mysql_query("DELETE FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_id = 'PUB2'");
		$msg = "<div class='rounded rounded-green'><center>".$userdata['username']." foi removido a partir da lista de Anunciantes <img src=\"./images/check.gif\"></center></div>";

	}else{
		$msg = "<div class='rounded rounded-red'><center>Error - Não foi possível localizar o usuário <img src=\"./images/del.gif\"></center></div>";
}

}elseif($do == "add" && $_POST['vip']){

	$user_check = mysql_query("SELECT * FROM users WHERE username = '".$_POST['vip']."' LIMIT 1");
	$userdata = mysql_fetch_assoc($user_check);

	if(mysql_num_rows($user_check) > 0){

			mysql_query("INSERT INTO publicistas (id, user_name) VALUES ('".$userdata['id']."','".$userdata['username']."')");
                        mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$userdata['id']."','PUB1','0')");
                        mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$userdata['id']."','PUB2','0')");
			$msg = "<div class='rounded rounded-green'><center>Você fez publicitário ".$_POST['vip']." corretamente. <img src=\"./images/check.gif\"></center></div><br>";

	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - Não Foi Possivel Encontrar O Usuário <img src=\"./images/del.gif\"></center></div><br>";
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>

<form action='<?php echo $adminpath; ?>/index/p/pubtool&do=add' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editar editor</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nome de usuario</strong><div class='graytext'>Por favor, digite o nome do usuário ao qual você deseja fazer o publicitário.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='vip' value="<?php echo $_POST['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Aceitar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Anunciantes atuais</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='20%' align='center'>Nome de usuario</td>
  <td class='tablesubheader' width='15%' align='center'>Email</td>
  <td class='tablesubheader' width='15%' align='center'>IP</td>
  <td class='tablesubheader' width='10%' align='center'>Estado</td>
  <td class='tablesubheader' width='5%' align='center'>Editar</td>
 </tr>

<?php

$get_pb = mysql_query("SELECT * FROM publicistas LIMIT 100"); while($row = mysql_fetch_assoc($get_pb)){ $userdata = mysql_fetch_assoc($userdata = mysql_query("SELECT * FROM users WHERE id = '".$row['id']."'"));

if($row['online'] >= 1){ $online = "online"; } else { $online = "offline";  

?>

<tr>
  <td class='tablerow1' align='left'><?php echo $userdata['username']; ?> (ID: <?php echo $userdata['id']; ?>)</td>
  <td class='tablerow1' align='left'><?php echo $userdata['mail']; ?></td>
  <td class='tablerow1' align='left'><?php echo $userdata['ip_last']; ?></td>
  <td class='tablerow2' align='center'><img src="<?php echo $path; ?>/web-gallery/v2/images/<?php echo $online; ?>.gif"></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/users_edit&key=<?php echo $userdata['id']; ?>'><img src='./images/edit.gif' alt='Editar dados de usuário'></a> <a href='<?php echo $adminpath; ?>/index/p/pubtool&do=delete&key=<?php echo $userdata['username']; ?>'><img src='./images/del.gif' alt='Editar dados de usuário'></a></td>
</tr>

<?php } } ?>
 
 </table>
</div>
	 </div>
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>