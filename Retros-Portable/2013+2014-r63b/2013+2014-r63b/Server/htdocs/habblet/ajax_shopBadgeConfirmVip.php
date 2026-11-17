<?php
header("Content-Type: text/html; charset=ISO-8859-1", true);
require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

$id = FilterText($_POST['optionNumber']);
$row = mysql_fetch_assoc(mysql_query("SELECT * FROM cms_marktplatzvip WHERE id = '".$id."' LIMIT 1"));

?>

<div id="hc_confirm_box">

    <img src="<?php echo $cimagesurl.$badgesurl.$row['image'].".gif"; ?>" alt="" align="right" style="margin:10px;" />

<p>Se você deseja comprar este emblema aperte em comprar logo abaixo!</p>
<p>Preço: <b><?php echo $row['dolares']; ?> Dólares.</b></p>
<p>Teus dólares: <b><?php echo $myrow['dolares']; ?> Dólares.</b></p>
<p>
<a href="#" class="new-button" onclick="habboclub.closeSubscriptionWindow(); return false;">
<b>Cancelar</b><i></i></a>
<a href="#" ondblclick="habboclub.showSubscriptionResultWindow(<?php echo $id; ?>,'Emblema comprado com sucesso!'); return false;" onclick="habboclub.showSubscriptionResultWindow(<?php echo $id; ?>,'Compra de Placa'); return false;" class="new-button">
<b>Comprar</b><i></i></a>
</p>

</div>

<div class="clear"></div>