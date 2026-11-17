<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$id =  FilterText($_POST['accountId']);

$sql = mysql_query("SELECT * FROM messenger_friendships WHERE user_one_id = '".$my_id."' AND user_two_id = '".$id."'");
if(mysql_num_rows($sql) > 0){
	$error = 1;
	$message = "Esta persona ya es tu amigo.";
}

$sql = mysql_query("SELECT * FROM messenger_requests WHERE from_id = '".$my_id."' AND to_id = '".$id."'");
if(mysql_num_rows($sql) > 0){
	$error = 1;
	$message = "Voc&ecirc; j&aacute;&nbsp;enviou&nbsp;um pedido de amizade&nbsp;com essa pessoa.&nbsp;Espere at&eacute; que&nbsp;voc&ecirc; &eacute; aceito&nbsp;ou rejeitado.";
}

if($id == $my_id){
	$error = 1;
	$message = "Voc&ecirc; n&atilde;o pode enviar&nbsp;um pedido de amizade&nbsp;para voc&ecirc; mesmo.";
}

if($error < 1){
	mysql_query("INSERT INTO `messenger_requests` (from_id,to_id) VALUES ('".$my_id."','".$id."')");
	$message = "Solicitud enviada."; 
}

?>

<div id="avatar-habblet-dialog-body" class="topdialog-body"><ul>
	<li><?php echo $message; ?></li>
</ul>

<p>
<a href="#" class="new-button done"><b>Ok</b><i></i></a>
</p></div>    