<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Peticiones VIP - Lista";
$pageid = "viptool_bestellung";

if($do == "accept" && is_numeric($key)){

$get_code = mysql_query("SELECT * FROM vip_bestellung WHERE id = '".$key."'");

if(mysql_num_rows($get_code) > 0){
	$row = mysql_fetch_assoc($get_code);

	$premium_check = mysql_query("SELECT * FROM vip WHERE id_user = '".$row['id_user']."'");
	if(mysql_num_rows($premium_check) > 0){

		mysql_query("UPDATE vip_bestellung SET finish = '1' WHERE id = '".$key."'");
		$msg = "<div class='rounded rounded-green'><center>C&oacute;digo aceptado con &eacute;xito. <img src=\"./images/check.gif\"></center></div>";
	} else {
		$msg = "<div class='rounded rounded-red'><center>El c&oacute;digo no puede ser aceptado. <img src=\"./images/del.gif\"></center></div>";
	}

} else {
	$msg = "<div class='rounded rounded-red'><center>No se ha podido encontrar el c&oacute;digo <img src=\"./images/del.gif\"></center></div>";
}

}

if($do == "decline" && is_numeric($key)){

$get_code = mysql_query("SELECT * FROM vip_bestellung WHERE id = '".$key."'");

if(mysql_num_rows($get_code) > 0){
	$row = mysql_fetch_assoc($get_code);

	if($row['finish'] !== '2'){

		mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$row['id_user']."','Du hast uns am <b>".date('d.m.Y - H:i:s', $row['time'])."</b> Uhr einen PaySafeCard Code zugesendet. Leider scheint dieser Code nicht zu funktionieren. <br><br>".$row['code_1']." - ".$row['code_2']." - ".$row['code_3']." - ".$row['code_4']."<br><br>Bitte überprüfe deinen Code nochmal und schicke ihn uns dann <a href=\"./credits/shop/vip\"><font color=\"red\">hier</font></a> erneut zu!<br><br>-Mit freundlichen Grüßen: HabboRE Management')");
		mysql_query("UPDATE vip_bestellung SET finish = '2' WHERE id = '".$key."'");

		$msg = "<div class='rounded rounded-green'><center>Eror en el PaysafeCode <img src=\"./images/check.gif\"></center></div>";
	} else {
	$msg = "<div class='rounded rounded-red'><center>C&oacute;digo rechazado <img src=\"./images/del.gif\"></center></div>";
	}
	
} else {
	$msg = "<div class='rounded rounded-red'><center>No se ha podido encontrar el c&oacute;digo <img src=\"./images/del.gif\"></center></div>";
}

}

@include('subheader.php');

?>

<?php if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Actualmente <?php echo mysql_evaluate("SELECT COUNT(*) FROM vip_bestellung"); ?> usuarios VIP || Con subscripci&oacute;n finalizada: <?php echo mysql_evaluate("SELECT COUNT(*) FROM vip_bestellung WHERE finish = '2'"); ?> || <?php echo mysql_evaluate("SELECT COUNT(*) FROM vip_bestellung WHERE finish = '1'"); ?> sin finalizar || <a href="https://customer.cc.at.paysafecard.com/psccustomer/GetWelcomePanelServlet?&language=es" target="_new">Reviar c&oacute;digos</a>
</div>
 <table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='1%' align='left'>ID</td>
  <td class='tablesubheader' width='15%'>Usuario</td>
  <td class='tablesubheader' width='15%'>Codigo</td>
  <td class='tablesubheader' width='35%'>Estadisticas</td> 
  <td class='tablesubheader' width='15%'>Fecha</td>
  <td class='tablesubheader' width='10%'>Editar</td>
  <td class='tablesubheader' width='1%'>Estado</td>
 </tr>

<?php

$get_users = mysql_query("SELECT * FROM vip_bestellung ORDER BY time DESC") or die(mysql_error());
while($row = mysql_fetch_assoc($get_users)){
	
$get_cc = mysql_query("SELECT * FROM users WHERE id = '".$row['id_user']."'") or die(mysql_error());
$users = mysql_fetch_assoc($get_cc);

if($row['finish'] == '0' or $row['finish'] == '2'){ $img = "del"; }else{ $img = "check"; }

?>

<tr>
  <td class='tablerow1' align='left'><?php echo $row['id']; ?>)</td>
  <td class='tablerow1' align='left'><?php echo $users['username']; echo' (ID: #'; echo $users['id']; ?>)</td>
  <td class='tablerow1' align='left'><?php echo $row['code_1']; ?> - <?php echo $row['code_2']; ?> - <?php echo $row['code_3']; ?> - <?php echo $row['code_4']; ?></td>
  <td class='tablerow1' align='left'>Total de <b><?php echo mysql_evaluate("SELECT COUNT(*) FROM vip_bestellung WHERE id_user = '".$row['id_user']."'"); ?></b>x || Davon <b><?php echo mysql_evaluate("SELECT COUNT(*) FROM vip_bestellung WHERE id_user = '".$row['id_user']."' AND finish = '1'"); ?></b>x aceptados || De los que <b><?php echo mysql_evaluate("SELECT COUNT(*) FROM vip_bestellung WHERE id_user = '".$row['id_user']."' AND finish = '2'"); ?></b>x finalizados</td>
  <td class='tablerow1' align='left'><?php echo date('d.m.Y - H:i:s', $row['time']); ?> Uhr</td>
  <td class='tablerow1' align='center'><?php if($row['finish'] == '0'){ ?><a href='<?php echo $adminpath; ?>/index/p/viptool_bestellung&do=accept&key=<?php echo $row['id']; ?>'><img src='./images/check.gif'></a> <a href='<?php echo $adminpath; ?>/index/p/viptool_bestellung&do=decline&key=<?php echo $row['id']; ?>'><img src='<?php echo $adminpath; ?>/images/del.gif'></a><?php }elseif($row['finish'] == '2'){ ?>Code Fehlerhaft!<?php }else{ ?>-/-<?php } ?></td>
  <td class='tablerow1' align='left'><center><img src="<?php echo $adminpath; ?>/images/<?php echo $img; ?>.gif"></center></td>
</tr>

<?php } ?>
 
</table>
</div>

</div>
</td></tr>
</table>
</div>

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>