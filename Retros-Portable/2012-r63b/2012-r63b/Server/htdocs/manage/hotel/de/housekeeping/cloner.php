<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Comprobar clones";
$pageid = "cloner";

if(isset($_POST['query']) && ($_POST['type'])){

	if($_POST['query']){

		if($_POST['type'] == "name"){

			$get_users_a = mysql_query("SELECT * FROM users WHERE username = '".FilterText($_POST['query'])."' ORDER BY username");
			$check_a = mysql_num_rows($get_users_a);

			if($check_a > 0){
		
				$userdata_a = mysql_fetch_assoc($get_users_a);
				$get_users = mysql_query("SELECT * FROM users WHERE ip_last = '".$userdata_a['ip_last']."' or ip_reg = '".$userdata_a['ip_reg']."' ORDER BY username");
				$check = mysql_num_rows($get_users);
				mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Klon-Check benutzt','cloner.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center><b>Es wurde/n ". mysql_evaluate("SELECT COUNT(*) From users WHERE username = '" . FilterText($_POST['query']) . "'")."  Mitglied/er gefunden</b> <img src=\"./images/check.gif\"></center></div>";
			} else {
				$msg = "<div class='rounded rounded-red'><center><b>No se han encontrado usurios</b> <img src=\"./images/del.gif\"></center></div>";
			}

		} else {

			$get_users = mysql_query("SELECT * FROM users WHERE ip_last = '".FilterText($_POST['query'])."' ORDER BY username");
			$check = mysql_num_rows($get_users);

			if($check > 0){
                mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Klon-Check benutzt','cloner.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center><b>Es wurde/n ". mysql_evaluate("SELECT COUNT(*) From users WHERE ip_last = '" . FilterText($_POST['query']) . "'")."  Mitglied/er gefunden</b> <img src=\"./images/check.gif\"></center></div>";
			} else {
				$msg = "<div class='rounded rounded-red'><center><b>Usuario no encontrado.</b> <img src=\"./images/del.gif\"></center></div>";
			}
		}
	} else {
		$msg = "<div class='rounded rounded-red'><center><b>Por favor, introduce una IP o Nombre de usuario</b> <img src=\"./images/del.gif\"></center></div>";
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/cloner&do=search' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Comprobar clones</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nombre de usuario o IP</strong><div class='graytext'>Por favor escribe el nombre de usuario o la IP para ver si hay clones.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='query' value="<?php echo $_POST['query']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Tipo</strong></td>
<td class='tablerow2'  width='60%'  valign='middle'>
<input name="type" value="name" <?php if($_POST['type'] == "name"){ ?>checked="checked" <?php } ?>type="radio">Usuario
<input name="type" value="ip" <?php if($_POST['type'] == "ip"){ ?>checked="checked" <?php } ?>type="radio">IP</td>
</tr>

<tr>
<td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Check' class='realbutton' accesskey='s'></td>
</tr>

</table>

</div>
</div>
</form>

<?php if(isset($_POST['query']) && ($_POST['type']) && $check > 0 or $check_a > 0){ ?>

<br>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Usuarios encontrados <font color="red">(M&aacute;ximo 3 cuentas por IP!)</font></center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='10%'>Nombre</td>
  <td class='tablesubheader' width='15%' align='left'>E-Mail</td>
  <td class='tablesubheader' width='10%' align='left'>IP(Registro)</td>
  <td class='tablesubheader' width='10%' align='left'>IP(&Uacute;ltima vez)</td>
  <td class='tablesubheader' width='20%' align='left'>&Uacute;ltima vez conectado</td>
  <td class='tablesubheader' width='20%' align='left'>Fecha de registro</td>
  <td class='tablesubheader' width='10%' align='left'>Estado</td>
  <td class='tablesubheader' width='10%' align='left'>Baneado</td>
  <td class='tablesubheader' width='10%' align='left'>Editar</td>
 </tr>
<?php

while($row = mysql_fetch_assoc($get_users)){
$get_banns = mysql_query("SELECT * FROM bans WHERE value = '".$row['id']."' AND bantype = 'user' OR value = '".$row['ip_last']."' AND bantype = 'ip'") or die(mysql_error());

if($row['online'] > 0){ 
	$status = "Conectado";
}else{
	$status = "Desconectado";
}

if(mysql_num_rows($get_banns) > 0){ 
	$color="green"; 
	$text="Si";
}else{
	$color="red";
	$text="No";
}

?>

 <tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><a href='<?php echo $path; ?>/home/<?php echo $row['username']; ?>'><strong><?php echo $row['username']; ?></strong></a></td>
  <td class='tablerow2' align='left'><a href='mailto:<?php echo $row['mail']; ?>'><?php echo $row['mail']; ?></a></td>
  <td class='tablerow2' align='left'><?php echo $row['ip_reg']; ?></td>
  <td class='tablerow2' align='left'><?php echo $row['ip_last']; ?></td>
  <td class='tablerow2' align='left'><?php echo date('d.m.Y - H:i:s', $row['last_online']); ?></td>
  <td class='tablerow2' align='left'><?php echo date('d.m.Y - H:i:s', $row['account_created']); ?></td>
  <td class='tablerow2' align='left'><img src="<?php echo $path; ?>/web-gallery/v2/images/<?php echo $status; ?>.gif"></td>
  <td class='tablerow2' align='left'><font color="<?php echo $color; ?>"><?php echo $text; ?></font></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/users_edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Edit User Data'></a></td>
</tr>

<?php } ?>
 
 </table>
</div>
<?php } ?>
	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>