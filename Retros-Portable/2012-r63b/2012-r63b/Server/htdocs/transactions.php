<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Movimentos da conta";
$pageid = "6b";
$body_id = "home";

include('./templates/community_subheader.php');
include('./templates/community_header.php');

?>

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column7" class="column">
<div class="habblet-container ">		
<div class="cbb clearfix brown ">
<h2 class="title">Movimentos da conta</h2>
<div id="tx-log">

<div class="box-content">
Bem vindo <b><?php echo $name; ?></b>! Aqui estão as últimas 50 transações feitas em sua conta.
</div>

<table class="tx-history">
<thead>
<tr>
 <th class="tx-date">Data</th>
 <th class="tx-amount">Créditos</th>
 <th class="tx-description">Descrição</th>
</tr>
</thead>
<tbody>

<?php
$tick = 0;
$get_em = mysql_query("SELECT date,amount,descr FROM cms_transactions WHERE userid = '".$my_id."' ORDER BY id DESC LIMIT 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_em)){
$tick++;

if(IsEven($tick)){ $even = "even"; } else { $even = "odd"; }

?>

<tr class="<?php echo $even; ?>">
 <td class="tx-date"><?php echo $row['date']; ?></td>
 <td class="tx-amount"><?php echo $row['amount']; ?></td>
 <td class="tx-description"><?php echo $row['descr']; ?></td>
</tr>

<?php } ?>

</tbody>
</table>

</div>
	
</div> 
</div> 
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script> 		     		

<div class="habblet-container ">		
<div class="cbb clearfix darkgray "> 
<div id="redeem-habblet"> 

<div class="redeem-balance"> 
 <p class="redeem-balance-username"><?php echo $name; ?></p>
 <p class="redeem-balance-text">Saldo:</p> 
 <p><span class="redeem-balance-amount"><?php echo $myrow['dolares']; ?></b></span></p> 
</div> 
 
<div class="redeem-redeeming-text"><p class="redeeming-text">Digite o código PUK.</p></div> 
<div class="redeem-form-container clearfix"> 

<form method="post" action="credits" id="voucher-form">
<div class="redeem-redeeming"> 
    <div><input type="text" name="voucherCode" value="" id="purse-habblet-redeemcode-string" class="redeemcode" /></div> 
    <div class="redeem-redeeming-button"><a href="<?php echo $path; ?>/me/#"<?php echo $target; ?> id="purse-redeemcode-button" class="new-button green-button redeem-submit"d="purse-redeemcode-button" class="new-button purse-icon"><b><span></span>Trocar</b><i></i></a></div> 
</form><br><br>
<script type="text/javascript">
new PurseHabblet();
</script>

</div> 
</div>

</div> 

<script type="text/javascript">
L10N.put("credits.navi.read_more", "Leer m&aacute;s");
L10N.put("credits.navi.close_fulltext", "Cerrar Instrucciones");
PaymentMethodHabblet.init();
</script>

<script type="text/javascript">
$("method-show-1").show();
$("method-full-2").hide();
</script>
	
</div> 
</div> 
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script></div>

<!--[if lt IE 7]>
<script type="text/javascript">
Pngfix.doPngImageFix();
</script>
<![endif]--> 
<script type="text/javascript"> 
HabboView.run(); 
</script>  

<?php include('./templates/community_footer.php'); ?>