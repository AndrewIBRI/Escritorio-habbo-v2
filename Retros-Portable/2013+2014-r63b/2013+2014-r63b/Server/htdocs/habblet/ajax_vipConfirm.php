<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

$id = FilterText($_POST['optionNumber']);
$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");
$data = mysql_fetch_assoc($query); 

?>

<div id="hc_confirm_box">

    <img src="<?php echo $cimagesurl.$badgesurl."VIP.gif"; ?>" alt="" align="right" style="margin:10px;" />

<p>Es&aacute;s apunto de hacerte VIP. &iexcl;Genial!</p>
<p>Duraci&oacute;n: <b>1 mes</b></p>
<p>Precio: <b>50000 P&iacute;xeles</b></p>
<p>Dispones de: <b><?php echo $data['activity_points']; ?> P&iacute;xeles</b></p>
<p>
<a href="#" class="new-button" onclick="habboclub.closeSubscriptionWindow(); return false;">
<b>Cancelar</b><i></i></a>
<a href="#" ondblclick="habboclub.showSubscriptionResultWindow(1,'¡Placa Comprada!'); return false;" onclick="habboclub.showSubscriptionResultWindow(1,'Compra de Emblemas'); return false;" class="new-button">
<b>Comprar</b><i></i></a>
</p>

</i><font size="1pt">By TitoP</font></i>
</div>

<div class="clear"></div>