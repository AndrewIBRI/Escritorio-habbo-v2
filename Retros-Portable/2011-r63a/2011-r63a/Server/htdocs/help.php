<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');

?>
<link rel="StyleSheet" href="../css/voice.css" type="text/css">
</style>
<body>
<div id="header">
</br></br><div class="logo">
</div></div>
<div class="contenido">
<h1>Tens uma ideia?</h1>
BEM VINDOS AO USER VOICE!</br>
Com este novo sistema, agora podemos ver os teus comentarios, tuas ideias, como objetivo de melhorar o <?php echo $shortname; ?> hotel.</br>
<strong>A cima de tudo,temos um par de regras:</strong></br>
1- Nao copies as ideias dos outros.</br>
2- No pessas cargo, creditos, raros, ect...</br>
3- Seja construtivo, explique as tuas ideias, com sentido.</br>
<strong>Nada mais, diverte-te no <?php echo $shortname; ?> hotel e compartilha todas as tuas ideias.</strong></br></br>
<h2>Compartilha a tua ideia!</h2>
<form action="../templates/coment.php" method="post">
<strong>Nome <?php echo $shortname; ?>:</strong></br>
<input type="text" name="username" value="<?php echo $name; ?>"></br></br>
<strong>Titulo da tua ideia:</strong></br>
<input type="text" name="title"></br></br>
<strong>Conteudo da tua ideia:</strong></br>
<textarea name="idea" rows="10" cols="40">
</textarea></br></br>
<input type="submit" value="Publicar ideia">
</form>
<br>
<br>
<br>
<h2>Ideias recentes:</h2>
</br></br>

<?php 

 $ar=fopen("./templates/comentarios.php","r") or
    die("No se pudo abrir el archivo");
  while (!feof($ar))
  {
    $linea=fgets($ar);
    $lineasalto=nl2br($linea);
    echo $lineasalto;
  }
  fclose($ar);
?>
<center><div class="footer">  
tavaresCMS by Tavares CSHP/PPF |.</div>
</div>
</body><center>