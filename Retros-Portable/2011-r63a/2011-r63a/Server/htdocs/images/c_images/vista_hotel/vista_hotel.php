<?php


$connessione = mysql_connect('localhost','root','SUA SENHA');
mysql_select_db('SUA DB', $connessione);


$image = "normal.png";
putenv('GDFONTPATH=' . realpath('.')); 
$font = 'font'; 
$username = $_GET['username'];

$get = mysql_fetch_array(mysql_query("SELECT username, look FROM users WHERE username = '" . $username . "'"));
$getstaff = mysql_fetch_array(mysql_query("SELECT username, look FROM users WHERE rank >= 2 ORDER BY RAND()"));
$d1 = date("d-m-y");
$adapt = $get[0]-7200;
$d2 = date("H:i:s");

$im = imagecreatefrompng($image);
$src_to_copy = imagecreatefrompng($src);
$pg = imagecreatefrompng('http://www.habbo.it/habbo-imaging/avatarimage?figure=' . $get[1] . '&action=crr=667&direction=2&head_direction=2&gesture=sml&size=b');

imagealphablending($im, true);
imagesavealpha($im, true);

imagealphablending($src_to_copy, true);
imagesavealpha($src_to_copy, true);

imagealphablending($pg, true);
imagesavealpha($pg, true);

$wc = ImageColorAllocate ($im, 255, 255, 255);
$red = ImageColorAllocate ($im, 255, 0, 0);
$blk = imagecolorallocate($im, 0, 0, 0);


imagecopy($im, $src_to_copy, 1300, 100, 0, 0, 201, 146);
imagecopy($im, $pg, 105, 510, 0, 0, 64, 110);

imagettftext($im, 10, 0, 105, 317, $blk, $font , "Olá, " . $username . "!");
if($get[1] != 1)
{
	imagettftext($im, 10, 0, 105, 334, $blk, $font , "Bem vindo ao Habbo!");
	imagettftext($im, 10, 0, 105, 346, $blk, $font , "Último acesso: " . $d1 . "");
	imagettftext($im, 10, 0, 105, 358, $blk, $font , "ás " . $d2 . "! Sentimos sua falta ;)");
	imagettftext($im, 10, 0, 105, 369, $blk, $font , "Bom divertimento!");
}
else
{
	imagettftext($im, 12, 0, 785, 310, $blk, $font , "Erro na conexão com o MySQL");
	imagettftext($im, 12, 0, 785, 330, $blk, $font , "Veja se configurou corretamente!");
}

imagettftext($im, 12, 0, 1370, 220, $blk, $font , $getstaff[0]);

header("Content-Type: image/png");
Imagepng($im);
ImageDestroy ($im);
?>