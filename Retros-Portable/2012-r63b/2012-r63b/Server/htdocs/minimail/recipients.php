<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$i = 0;
$output = "[";
$getem = mysql_query("SELECT * FROM messenger_friendships WHERE user_one_id = '".$my_id."'") or die(mysql_error());

while ($row = mysql_fetch_assoc($getem)) {
	$i++;

	if($row['user_two_id'] == $my_id){
		$friendsql = mysql_query("SELECT * FROM users WHERE id = '".$row['user_one_id']."'");
	} else {
		$friendsql = mysql_query("SELECT * FROM users WHERE id = '".$row['user_two_id']."'");
	}

	$friendrow = mysql_fetch_assoc($friendsql);

	$name = $friendrow['username'];
	$id = $friendrow['id'];

	$output = $output."{\"id\":".$id.",\"name\":\"".$name."\"},";
}
$output = substr_replace($output,"",-1);
$output = $output."]";
?>
/*-secure-
<?php echo $output; ?>
 */