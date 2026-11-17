<?php

$ar=fopen("../templates/comentarios.php","a") or
 die("Problemas en la creacion");
 fputs($ar,$_REQUEST['username']);
 fputs($ar,"\n");
 fputs($ar,$_REQUEST['title']);
 fputs($ar,"\n");
 fputs($ar,$_REQUEST['idea']);
 fputs($ar,"\n");
 fputs($ar,"--------------------------------------------------------");
 fputs($ar,"\n");
 fputs($ar,"</br>");
  fputs($ar,"</br>");
 fclose($ar);
 echo "Tu idea fue publicada correctamente.";
 ?>
 
 </br>
 
 <form method="post" action="">
 <input type="button" onclick="window.location='../uservoice.php'" href="../uservoice.php" value="Click aquí para ver tú idea">
 </form>