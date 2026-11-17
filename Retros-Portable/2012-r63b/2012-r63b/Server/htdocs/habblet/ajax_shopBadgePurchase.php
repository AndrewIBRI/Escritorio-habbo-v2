<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

$id = FilterText($_POST['optionNumber']);

$sql = mysql_query("SELECT * FROM cms_marktplatz WHERE id = '".$id."' LIMIT 1");
$row = mysql_fetch_assoc($sql);

if($myrow['credits'] < $row['credits']){ $error = "Cr&eacute;ditos"; }
if($myrow['activity_points'] < $row['pixels']){ $error = "P&iacute;xeles"; }
if($myrow['vip_points'] < $row['schneeflocken']){ $error = "Puntos VIP"; }

if(isset($error)){
	$msg ="<img src=\"".$path."/web-gallery/v2/images/frank_welcome.png\" style=\"margin: 10px;\" align=\"left\">
		<p>&iexcl;No tienes lo suficiente para comprar esta Placa!</p>
 		<p>No tienes suficientes <b>".$error."</b> para comprar esta Placa<br></p>
 		<p>Si no tienes suficientes Cr&eacute;ditos/P&iacute;xeles/Puntos VIP, haz clic <a href=\"".$path."/credits\">aqu&iacute;</a><br></p>";
}elseif($vip > 0 or $user_rank > 4){
		$check2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$my_id."' AND badge_id = '".$row['image']."'");
		$check = mysql_num_rows($check2);
                if($check < 1){
			mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','".$row['image']."','0')");
			mysql_query("UPDATE users SET credits = credits - '".$row['credits']."', activity_points = activity_points - '".$row['pixels']."', vip_points = vip_points - '".$row['vip_points']."' WHERE id = '".$my_id."'") or die(mysql_error());
			mysql_query("INSERT INTO cms_transactions (userid,amount,date,descr) VALUES ('".$my_id."',-'".$row['credits']."','".$date_full."','Badge ".$row['image']." gekauft')") or die(mysql_error());
			$msg = "&iexcl;Has comprado la Placa <b>".$row['image']."</b> con &eacute;xito!";
		}else{
			$msg = "&iexcl;Ya tienes esta Placa!";
		}
} else {
$msg = "<img src=\"".$path."/web-gallery/v2/images/frank_welcome.png\" align=\"right\"> La tienda de placas es para miembros VIP.<br><br>Si quieres unirte, clic <a href=\"./credits\">aqu&iacute; &raquo;</a><br><br><br>";
}

?>

<div id="hc_confirm_box">


<p><?php echo $msg; ?></p>
<p>
<a href="#" class="new-button" onclick="habboclub.closeSubscriptionWindow(); return false;">
<b>Continuar</b><i></i></a>
</p>

</div>

<div class="clear"></div>