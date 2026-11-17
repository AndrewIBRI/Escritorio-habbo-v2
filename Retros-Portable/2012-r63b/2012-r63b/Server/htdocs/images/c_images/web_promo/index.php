<?php

$ruta = "../web_promo/"; // Indicar ruta
$filehandle = opendir($ruta); // Abrir archivos
while ($file = readdir($filehandle)) {
        if ($file != "." && $file != "..") {
                $tamanyo = GetImageSize($ruta . $file);
                echo "<p><img src='$ruta$file' $tamanyo[3]><br>/swfs/c_images/web_promo/$file<br></p>\n";
        } 
} 
closedir($filehandle);

?>