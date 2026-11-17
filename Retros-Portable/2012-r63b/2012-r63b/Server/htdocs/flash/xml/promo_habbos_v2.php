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

header("Content-Type: text/xml");

if($settings->find("site_generate_promo_habbos") == 0){
	if(!file_exists('./xml/promo_habbos_v2.xml') || ($user->user("rank") > 4 && (isset($_GET['recache']) && $_GET['recache'] == "true"))){
		$fh = fopen('./xml/promo_habbos_v2.xml', 'w');
		fwrite($fh, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<habbos url=\"".PATH."/habbo-imaging/avatar/\">\n");
		$generator = new HoloFigureCheck();
		$i = 0;
		while ($i < 30) {
			if($input->IsEven($i)){ $gender = "M"; }else{ $gender = "F"; }
			$figure = $generator->generateFigure(false,$gender);
			fwrite($fh, "<habbo gender=\"".strtolower($figure[1])."\" figure=\"".$figure[0]."\" hash=\"".$user->avatarURL($figure[0],"b,3,3,sml,1,0",1)."\" />\n");
			$i++;
		}
		fwrite($fh, "</habbos>");
		echo file_get_contents('./xml/promo_habbos_v2.xml');
	}else{
		echo file_get_contents('./xml/promo_habbos_v2.xml');
	}
}else{
echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
?>
<habbos url="<?php echo PATH; ?>/habbo-imaging/avatar/">
<?php
$generator = new HoloFigureCheck();
$i = 0;
while ($i < $settings->find("site_generate_promo_habbos")) {
$i++;
$figure = $generator->generateFigure(false);
?>

<habbo gender="<?php echo strtolower($figure[1]); ?>" figure="<?php echo $figure[0]; ?>" hash="<?php echo $user->avatarURL($figure[0],"b,3,3,sml,1,0",1); ?>" />
<?php } ?>
</habbos>
<?php } ?>