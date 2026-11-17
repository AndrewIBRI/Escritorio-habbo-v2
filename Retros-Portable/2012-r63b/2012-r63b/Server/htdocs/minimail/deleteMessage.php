<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$label = FilterText($_POST['label']);
$id = FilterText($_POST['messageId']);
$start = FilterText($_POST['start']);
$conversation = FilterText($_POST['conversationId']);

$sql = mysql_query("SELECT * FROM cms_minimail WHERE id = '".$id."' LIMIT 1");
$row = mysql_fetch_assoc($sql);

if($row['folder'] == "trash"){
mysql_query("DELETE FROM cms_minimail WHERE id = '".$id."' LIMIT 1");
$bypass = "true";
$page = "trash";
$message = "El mensaje ha sido borrado.";
include('loadMessage.php');
} else {
mysql_query("UPDATE cms_minimail SET folder = 'trash' WHERE id = '".$id."' LIMIT 1");
$bypass = "true";
$page = "inbox";
$message = "El mensaje ha sido enviado a la papelera.";
include('loadMessage.php');
} ?>