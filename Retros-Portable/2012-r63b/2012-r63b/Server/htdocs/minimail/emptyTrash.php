<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

mysql_query("DELETE FROM cms_minimail WHERE folder = 'trash' AND receiver_id = '".$my_id."'");
$bypass = "true";
$page = "trash";
$message = "&iexcl;Vaciar&aacute;s la papelera!";
include('loadMessage.php');

?>