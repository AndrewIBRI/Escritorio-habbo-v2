<?php
/*=========================================================+
|| # -RemiX CMS - Sistema de administração de conteúdo Habbo.
|+=========================================================+
|| # Copyright © 2012 RemiX CMS. Todos os direitos reservados.
|| # Email: macyelgomez@hotmail.com
|+=========================================================+
|| # Todas as imagens, scripts e temas
|| # Copyright (C) 2011 Sulake Ltd. All rights reserved.
|+=========================================================*/

$page['dir'] = '\xml';
require_once('../includes/core.php');
$data = new xml_sql;

header("Content-Type: text/xml");
echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
echo "<habbos>\n";

$sql = $data->select1();
while ($row = $db->fetch_row($sql)) {

	$form_badge = $user->GetUserBadge($row[1]);
	$form_group_badge = $user->GetUserGroupBadge($row[0]);

	if($form_badge != false){
		$form_badge = $settings->find("site_c_images_path").$settings->find("site_badges_path").$form_badge.".gif";
	} else {
		$form_badge = "";	
	}

	if($form_group_badge != false){
		$form_group_badge = "groupBadge=\"".PATH."/habbo-imaging/badge/".$form_group_badge.".gif\"";
	} else {
		$form_group_badge = "";
	}

	if($user->IsUserOnline($row[0]) == true){
	$status = "1";
	} else {
	$status = "0";
	}

	printf("<habbo id=\"%s\" name=\"%s\" motto=\"%s\" url=\"".PATH."/home/%s\" image=\"".$user->avatarURL($row[3],"b,4,3,sml,1,0")."\" badge=\"%s\" status=\"%s\" %s />\n", $row[0], $row[1], $input->HoloText($row[2]), $row[1], $form_badge, $status, $form_group_badge);

}

echo "</habbos>";
?>
