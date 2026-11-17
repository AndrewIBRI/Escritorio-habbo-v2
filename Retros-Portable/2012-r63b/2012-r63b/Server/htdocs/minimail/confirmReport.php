<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$id = FilterText($_POST['messageId']);

$sql = mysql_query("SELECT * FROM cms_minimail WHERE id = '".$id."' LIMIT 1");
$row = mysql_fetch_assoc($sql);

if($row['sender_id'] == $my_id){
	$error = 1;
	$message = "No puedes reportar tus propios mensajes.";
}

if($error == 1){
?>
<ul class="error">
	<li><?php echo $message; ?></li>
</ul>

<p>
<a href="#" class="new-button cancel-report"><b>Cancelar</b><i></i></a>
</p>
<?php
} else {
$sql = mysql_query("SELECT * FROM users WHERE id = '".$row['sender_id']."' LIMIT 1");
$senderrow = mysql_fetch_assoc($sql);
?>
<p>
&iquest;Est&aacute;s seguro que deseas reportar el mensaje <b><?php echo $row['subject']; ?></b> a los moderadores y borrar a <b><?php echo $senderrow['username']; ?></b> de tu lista de amigos? No podr&aacute;s volver atr&aacute;s
</p>

<p>
<a href="#" class="new-button cancel-report"><b>Cancelar</b><i></i></a>
<a href="#" class="new-button send-report"><b>Enviar reporte</b><i></i></a>
</p>
<?php } ?>