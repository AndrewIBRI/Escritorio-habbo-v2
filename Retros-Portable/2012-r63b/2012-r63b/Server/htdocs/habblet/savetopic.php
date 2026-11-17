<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

$message = FilterText($_POST['message']);
$topicTitle = FilterText($_POST['topicName']);

if(empty($topicTitle)){ echo "El t&iacute;tulo no puede estar en blanco"; exit; }

mysql_query("INSERT INTO cms_forum_threads (forumid,type,title,author,date,lastpost_author,lastpost_date,views,posts,unix) VALUES ('".FilterText($_POST['groupId'])."','1','".$topicTitle."','".$name."','".$date_full."','".$name."','".$date_full."','0','0','".strtotime('now')."')") or die(mysql_error());
mysql_query("UPDATE users SET postcount = postcount + 1 WHERE id = '".$my_id."' LIMIT 1");

$check = mysql_query("SELECT id FROM cms_forum_threads WHERE forumid='".FilterText($_POST['groupId'])."' ORDER BY id DESC LIMIT 1") or die(mysql_error());
$row = mysql_fetch_assoc($check);

$threadid = $row['id'];

mysql_query("INSERT INTO cms_forum_posts (forumid,threadid,message,author,date) VALUES ('".FilterText($_POST['groupId'])."','".$threadid."','".$message."','".$name."','".$date_full."')");

echo "viewthread.php?thread=".$threadid."&page=1";

?>