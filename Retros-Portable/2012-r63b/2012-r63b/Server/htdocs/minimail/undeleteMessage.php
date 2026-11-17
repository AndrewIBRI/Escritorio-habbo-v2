<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$id = FilterText($_POST['messageId']);
$start = FilterText($_POST['start']);
$label = FilterText($_POST['label']);

mysql_query("UPDATE cms_minimail SET folder = 'inbox' WHERE id = '".$id."'");

$bypass = "true";
$page = "inbox";
$message = "El mensaje se ha movido a la bandeja de entrada.";
include('loadMessage.php');
?>