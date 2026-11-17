<?php
header("Content-Type: text/html; charset=ISO-8859-1", true);
require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

$id = FilterText($_POST['optionNumber']);

$sql = mysql_query("SELECT * FROM cms_marktplatzvip WHERE id = '".$id."' LIMIT 1");
$row = mysql_fetch_assoc($sql);

if($myrow['dolares'] < $row['dolares']){ $error = "Dólares"; }


if(isset($error)){
	$msg ="<img src=\"".$path."/web-gallery/v2/images/frank.png\" style=\"margin: 10px;\" align=\"left\">
		<p>Você não possui dólares suficiente para comprar este emblema!</p>
 		<p>Não possui <b>".$error."</b> para comprar este emblema<br></p>
 		<p>Você não possui dólares suficiente para comprar esse emblema :( !</a><br></p>";
}elseif($myrow['online'] == '0'){
		$check2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$my_id."' AND badge_id = '".$row['image']."'");
		$check = mysql_num_rows($check2);
                if($check < 1){
			mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','".$row['image']."','0')");
			mysql_query("UPDATE users SET dolares = dolares - '".$row['dolares']."' WHERE id = '".$my_id."'") or die(mysql_error());
mysql_query("INSERT INTO cms_transactionsvip (userid,amount,date,descr) VALUES ('".$my_id."',-'".$row['dolares']."','".$date_full."','Compra de una Placa.')") or die(mysql_error());
			$msg = "&iexcl;Você comprou o emblema <b>".$row['image']."</b> com sucesso!";
		}else{
			$msg = "Haha espertinho!, você já comprou esse emblema!";
		}
} else {
$msg = "<img src=\"".$path."/web-gallery/v2/images/frank.png\" align=\"right\">Porfavor nosso sistema detectou que você está online, desconecte-se do hotel e atualize essa página com a tecla F5 e faça sua compra!<br><br><br>";
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