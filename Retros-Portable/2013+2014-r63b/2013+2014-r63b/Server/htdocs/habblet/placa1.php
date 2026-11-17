<?php
require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 
$check2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$my_id."' AND badge_id = 'Monk1'");
		$check = mysql_num_rows($check2);
                if($check < 1){
$muestra = "si";
mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','Monk1','0')");
}else{
$muestra = "no";
}
echo "muestra=" . $muestra;
?>