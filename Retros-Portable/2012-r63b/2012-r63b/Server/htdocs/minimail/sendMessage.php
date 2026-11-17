<?php

if($bypass1 != "true"){
require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$messageid = FilterText($_POST['messageId']);
$recipientids = FilterText($_POST['recipientIds']);
$subject = FilterText($_POST['subject']);
$body = addslashes(htmlspecialchars(($_POST["body"])));
}
$body = FilterText($body);

$ids = explode(",", $recipientids);
$numofids = count($ids);

$sql = mysql_query("SELECT NOW()");
$date = mysql_result($sql, 0);

if(isset($_POST['messageId'])){
	$sql = mysql_query("SELECT * FROM cms_minimail WHERE id = '".$messageid."'");
	$row = mysql_fetch_assoc($sql);
	if($row['conversationid'] == "0"){
		$sql = mysql_query("SELECT MAX(conversationid) FROM cms_minimail");
		$conid = mysql_result($sql, 0);
		$conid = $conid + 1;
		mysql_query("UPDATE cms_minimail SET conversationid = '".$conid."' WHERE id = '".$row['id']."'");
	} else {
		$conid = $row['conversationid'];
	}
	$subject = "Re: ".$row['subject'];
	$ids[0] = $row['senderid'];
} else {
	$conid = "0";
}

$elements = count($ids);
$elements = $elements - 1;
$i = -1;
while ($elements <> $i){
	$i++;
    mysql_query("INSERT INTO cms_minimail (sender_id,receiver_id,folder,is_read,subject,date,isodate,timestamp,body,conversationid) VALUES ('".$my_id."','".$ids[$i]."','inbox','0','".$subject."','".$date."','0','0','".$body."','".$conid."')") or die(mysql_error());
}
$bypass = "true";
$page = "inbox";
$message = "El mensaje ha sido enviado con &eacute;xito.";
if($bypass1 != "true"){ include('loadMessage.php'); }
?>