<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Bots";
$pageid = "72";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>
<div id="container">
<div id="content">
<div id="column1" class="column">
<?php
$query = mysql_query("SELECT * FROM cms_transactions WHERE userid = '".$my_id."'");

$saldo = mysql_fetch_assoc(mysql_query("SELECT dolares FROM users WHERE id = '".$my_id."'"));

if (isset($_POST['rare'])){

$rare = mysql_real_escape_string(htmlentities($_POST['rare']));

if (is_numeric($rare)){

$me = mysql_real_escape_string($myrow['username']);
$query = mysql_query("SELECT * FROM users WHERE username = '".$me."'");
while ($data = mysql_fetch_array($query)){
$vip = $data['vip'];
}
$query = mysql_query("SELECT * FROM site_tienda WHERE tipo = 'rare' AND valor = '".$rare."'");

if (mysql_num_rows($query) > 0){

$data = mysql_fetch_array($query);
$id = $my_id;
$tranferencia = 'Efeito Comprado BOT';
$valor_rare = $data['precio'];
$imagen = $data['imagen'];
$nombre = $data['nombre'];
$id_item = $data['valor'];

if ($valor_rare <= $saldo) {

mysql_query("INSERT INTO cms_transactions (userid, descr, amount, date) VALUES ('$id', 'restar', '$tranferencia', '$valor_rare', '".time()."')");


$query55 = mysql_query("SELECT comprados FROM site_tienda WHERE tipo='rare' AND valor ='".$rare."'") or die(mysql_error()); 
$dataj = mysql_fetch_assoc($query55); 

$id2 = $dataj['comprados'];
$cantidad = "1";

$autoid = $id2 + $cantidad;


mysql_query("UPDATE site_tienda SET comprados = '".$autoid."' WHERE valor = '.$rare.' AND tipo = 'rare'");
mysql_query("INSERT INTO `bots_effects`(`user_id`, `effect`, `nombre`, `imagen`) values ('.$id.', '.$rare,','.$nombre.','.$imagen.')") or die(mysql_error());

header("Location: /bots/efectos?exito");

}else{
header ("Location: /bots/efectos?error=monto");
die;
}
}else{
header ("Location: /bots/efectos?error=db");
die;	
	}
}
}

?>
<?php
if (isset($_GET['error']) && $_GET['error'] == 'db'){ ?>
<script>
alert('O artigo que você está tentando comprar, não foi encontrado');
</script>
<?php }
if (isset($_GET['exito'])){ ?>
<script>
alert('Compra real, Obrigado pela sua compra.');
</script>
<?php } ?>
<?php
if (isset($_GET['error']) && $_GET['error'] == 'monto'){ ?>
<script>
alert('Não é Premium ou o item que você está tentando comprar, o seu saldo atual excede. ');
</script>
<?php } ?>


<?php
$total = mysql_query("SELECT * FROM site_tienda WHERE tipo ='rare' AND otro ='bot_effect'");
$total = mysql_num_rows($total);
?>
<div class="cbb clearfix red ">
<h2 class="title"><span style="float: left;">Efeitos para BOTS</span> <span style="float: right; font-weight: normal; font-size: 75%;">(<?php echo $total ?> disponibles)</span></h2>
<div style="height:400px;overflow:auto;width:99.5%;">


<?php
$color = '#FFFFFF';
 $query=mysql_query("SELECT * FROM site_tienda WHERE tipo ='rare' AND otro ='bot_effect' ORDER BY ID DESC");
$cuantos2 =$data['valor'];

while ($data=mysql_fetch_array($query)){ 

if ($color == '#FFFFFF'){
	
	$color = '#C2FFD5';
	}else{
	$color = '#FFFFFF';	
	}?>

<div style="width:100%;height:100px;padding-top:10px;background-color:<?php echo $color ?>;">
<table cellspacing="0" cellpadding="0" style="width:100%;">
<tr>
<td style="padding-left:20px;"><img src="<?php echo $data['imagen'] ?>"></td>
<td><b style="font-size:15px; color:#0EA800;"><?php echo $data['nombre'] ?></b><br><b style="font-size:15px;"><?php echo $data['precio'] ?> PeSos </b></td>
<td><div style="margin-top:15px;margin-right:20px;" align="right" class="redeem-redeeming-button"><form method="post" action=""><input type="hidden" name="rare" value="<?php echo $data['valor'] ?>">

<input type="submit" value="Comprar Agora" class="new-button fill">

</form></div>
</td>
</tr>
</table>
</div>
<?php } ?>
</div>
</div>
</div>
<div id="column2" class="column">
<div class="habblet-container ">		
<div class="cbb clearfix green "> 
<h2 class="title">Efeitos para o seu BOT</h2>
<div class="box-content">
Lembre-se que você só pode usar BOT `S.
<table width="250" border="0" cellpadding="0" cellspacing="1" align="left">

<tr>         
<td><b>Efeito</b></td>
            <td><b>Nome</b></td>
            <td><b>ID do Efeito</b></td>
        </tr>
<?php $query = mysql_query("SELECT * FROM bots_effects WHERE user_id ='".$my_id."'");
while ($data = mysql_fetch_array($query)){ ?>
<tr>
            <td><img src="<?php echo $data['imagen'] ?>"></td> 
            <td><?php echo $data['nombre'] ?></td>
            <td><?php echo $data['effect'] ?></td>
        </tr>

<?php } ?></table>

</div></div></div>



<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>
<?php require_once('./templates/community_footer.php'); ?>