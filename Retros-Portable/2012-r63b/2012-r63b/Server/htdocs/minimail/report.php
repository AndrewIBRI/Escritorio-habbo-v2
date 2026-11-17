<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$id = FilterText($_POST['messageId']);
$start = FilterText($_POST['start']);
$label = FilterText($_POST['label']);

$sql = mysql_query("SELECT NOW()");
$date = mysql_result($sql, 0);

$sql = mysql_query("SELECT * FROM cms_minimail WHERE id = '".$id."' LIMIT 1");
$row = mysql_fetch_assoc($sql);

mysql_query("DELETE FROM messenger_friendships WHERE user_one_id = '".$my_id."' AND user_two_id = '".$row['sender_id']."' LIMIT 1");
mysql_query("DELETE FROM messenger_friendships WHERE user_two_id = '".$row['sender_id']."' AND user_one_id = '".$my_id."' LIMIT 1");
mysql_query("DELETE FROM cms_minimail WHERE id = '".$id."' LIMIT 1");

$bypass = "true";
$page = $label;
$startpage = $start;
$message = "Mensaje reportado y amigo borrado correctamente.";
include('loadMessage.php');
?>