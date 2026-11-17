<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = " VIP";
$pageid = "5a";
$body_id = "home";

if(isset($_POST['submit'])){

	if($_POST['mainPagePart:rn1']){

		if(mysql_num_rows($sql = mysql_query("SELECT * FROM vip_bestellung WHERE id_user = '".$my_id."' AND finish = '0'")) < 1){

			if(is_numeric($_POST['mainPagePart:rn1'])){
				if(strlen($_POST['mainPagePart:rn1']) == '4'){

	mysql_query("INSERT INTO vip_bestellung (`id_user`, `time`, `finish`,`code_1`) VALUES ('".$my_id."','".time()."','0','".FilterText($_POST	['mainPagePart:rn1'])."')") or die(mysql_error());
	$msg = "Su pedido ha sido recibido con &eacute;xito. Se le notificar&aacute; en las pr&oacute;ximas 24 horas con una alerta.";
	$color = "green";

				} else {
					$msg = "&iexcl;El c&oacute;digo debe contener 4 n&uacute;meros! Ejemplo: 0060, 0100";
					$color = "red";
				}
			} else {
				$msg = "El c&oacute;digo debe contener n&uacote;meros del 0 al 9";
				$color = "red";
			}
		} else {
			$msg = "Ya has pedido tu subscripci&oacute;n VIP. &iexcl;Espera a que tu pedido sea procesado!";
			$color = "red";
		}

	} else {
		$msg = "No has ingresado el c&oacute;digo completo.";
		$color = "red";
	}
}

include('./templates/community_subheader.php');
include('./templates/community_header.php');

?>

<script src="https://customer.cc.at.paysafecard.com/psccustomer/js/auto_tab.js" type="text/javascript"></script>
<form name="mainPagePart" method="post">

<div id="container">
<div id="content">
<div id="column1" class="column">
<div class="cbb clearfix orange"> 
 
<h2 class="title"><?php echo $sitename; ?> - Tienda VIP </h2> 
<div class="habblet box-content"> 
<?php if(isset($msg)){ ?>
<td colspan="2" class="main" width="100%" height="16"><span class="font-small"><font color="<?php echo $color; ?>"><b><?php echo $msg; ?></b></font></span></td><br><br>
<?php } ?>
<img src="<?php echo $path; ?>/web-gallery/v2/images/frank_welcome.png" align="right">Debes introducir tu Cantidad de Referidos que tienes en el Hotel<br><br>Si introduces una cantidad que no es igual a la que tienes, seras expulsado del hotel.<br><br><br><br><br>

Referidos:
<td class="main">
<span class="text text-small3">
<input id="mainPagePart:rn1" type="text" name="mainPagePart:rn1" value="<?php echo FilterText($_POST['mainPagePart:rn1']); ?>" autocomplete="off" class="text" style="width: 80px" maxlength="4" onkeyup="return autoTab(this, 4, event);" size="4" style="text-align:center" />
</span>

<br><br>
<?php if(mysql_num_rows($sql = mysql_query("SELECT * FROM vip_bestellung WHERE id_user = '".$my_id."' AND finish = '0'")) < 1){ ?>
<input type="submit" name="submit" class="save" value="Enviar" style="float: right">
<?php } else { ?>
<input type="submit" name="submit" class="cancel2" disabled="disabled" value="In bearbeitung..." style="float: right">
<?php } ?><i></i></a>
</td></tr>

<br><br><br>

<center><i><font color="gray">Powered by:</i></font><br><img src="<?php echo $path; ?>/Premiun_Images/logo.png" border="0"></center>


</tbody>					
							
</div> 
</div></div>


<div id="column2" class="column">
<div class="cbb clearfix orange"> 
 
<h2 class="title">Tu estado</h2> 
<div class="habblet box-content"> 
    
 <?php if($vip < 1){ ?><img src="<?php echo $path; ?>/web-gallery/v2/images/games_illustration.png" align="right"><b>No</b> eres VIP.<br><br>Si quieres ser VIP entonces compra una <a href="http://www.paysafecard.com/es/es-paysafecard/" target="_new">PaySafeCard</a> de <b>10 €</b> e introduce el c&oacute;digo en el box de la izquierda.<br><br>Su solicitud VIP ser&aacute; procesada en unas 48 horas.<br> Ser&aacute;s notificado con un aviso en la p&aacute;gina principal de <?php echo $sitename; ?>.<br><br>Si no recibe el VIP en 48 horas env&iacute;a una llamada de ayuda al hotel.<br><hr>
<?php } elseif($vip > 0){ $get_status = mysql_fetch_assoc($get_status = mysql_query("SELECT * FROM vip WHERE id_user = '".$my_id."'")); ?>
<img src="<?php echo $path; ?>/web-gallery/v2/images/facebook/app_habbo_hotel_image.gif" align="right"><b>Si</b> eres miembro del club VIP.<br><br>ID: <?php echo $get_status['id']; ?><br>UserID: <?php echo $get_status['id_user']; ?><br>Miembro desde: <?php echo date('d.m.Y', $get_status['timestamp']); ?><br>Finaliza el: <?php echo date('d.m.Y', $get_status['timestampend']); ?><br><hr>
<?php } ?>
<br>Pedidos Enviados:<br><br><?php $code_a = mysql_query("SELECT * FROM vip_bestellung WHERE id_user = '".$my_id."'"); while($code = mysql_fetch_assoc($code_a)){ echo $code['code_1']; echo' Referidos '; if($code['finish'] == "1"){ echo" <img src=\"".$adminpath."/images/check.gif\">"; } elseif($code['finish'] == "2") { echo" <img src=\"".$adminpath."/images/del.gif\">"; } else { echo' <i>En proceso...</i><br>'; } } ?>
<br><br><br><i><font color="red">Recuerda que debe ser la cantidad de referidos correcta que tengas, caso contrario revisaremos tus referidos y si mientes seras baneado o simplemente no sera entregado tu VIP!</font></i>

</div></div></div>

<div id="column7" class="column">
<div class="cbb clearfix orange"> 
 
<h2 class="title">- Informaci&oacute;n -</h2>

<div class="disclaimer" style="border: 0 !important;"> 
    <center><h3><span>Atenci&oacute;n: Es algo autom&aacute;tico, &iexcl;no hay forma de devolver el dinero!</span></h3> 
  Hazlo siempre con el permiso de tus padres o tutor.
</center></div> 

	
						
							
					</div> 
</div></div></div>
<?php include('./templates/community_footer.php'); ?>