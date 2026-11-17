<?php 
                                       
$FACT['1'] = "blue";
$FACT['2'] = "red";
$FACT['3'] = "green";
$FACT['4'] = "yellow";
$FACT['5'] = "purple";
                                       
$i = $FACT[rand(1,5)];
$i .= "_logo.gif";
                                       
$image = imagecreatefromgif($i); 
header('Content-type: image/gif'); 
imagegif($image); 
imagedestroy($image); 
?> 