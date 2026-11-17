<?php
header("Content-Type: text/html; charset=ISO-8859-1", true);
require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

$id = FilterText($_POST['optionNumber']);

$sql = mysql_query("SELECT * FROM cms_marktplatz WHERE id = '".$id."' LIMIT 1");
$row = mysql_fetch_assoc($sql);

if($myrow['credits'] < $row['credits']){ $error = "Cr&eacute;ditos"; }
if($myrow['activity_points'] < $row['pixels']){ $error = "P&iacute;xeles"; }

if(isset($error)){
	$msg ="<img src=\"".$path."/web-gallery/v2/images/frank.png\" style=\"margin: 10px;\" align=\"left\">
		<p>&iexcl;Não possui o suficiente para comprar este emblema!</p>
 		<p>Você não possui<b>".$error."</b> para comprar este emblema!<br></p>
 		<p>Si no tienes suficientes Cr&eacute;ditos/P&iacute;xeles, haz clic <a href=\"".$path."/credits\">aqu&iacute;</a><br></p>";
}elseif($myrow['online'] == '0'){
		$check2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$my_id."' AND badge_id = '".$row['image']."'");
		$check = mysql_num_rows($check2);
                if($check < 1){
			mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','".$row['image']."','0')");
			mysql_query("UPDATE users SET credits = credits - '".$row['credits']."', activity_points = activity_points - '".$row['pixels']."' WHERE id = '".$my_id."'") or die(mysql_error());
			mysql_query("INSERT INTO cms_transactions (userid,amount,date,descr) VALUES ('".$my_id."',-'".$row['credits']."','".$date_full."','Compra de la Placa ".$row['image'].".')") or die(mysql_error());
			$msg = "Você comprou o emblema <b>".$row['image']."</b> com sucesso!";
		}else{
			$msg = "Haha espertinho! Você já possui este emblema!";
		}
} else {
$msg = "<img src=\"".$path."/web-gallery/v2/images/frank_welcome.png\" align=\"right\">Você tem que desconectar do Habbie para comprar o emblema!<br><br><br>";
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