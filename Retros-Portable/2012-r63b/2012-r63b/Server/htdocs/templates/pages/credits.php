<?php

$refer = $_SERVER['HTTP_REFERER']; 
$pos = strrpos($refer, "client"); 
if($pos == true){
	$target = " target=\"_new\"";
}

?>

<div class="method-group online clearfix"> 
<div class="method idx0"> 
	
<div class="method-content"> 		
<h2>Cr&eacute;ditos gratis!</h2>
<br> 
<div class="summary clearfix">    
Toda vez que voc&ecirc; iniciar uma sess&atilde;o no hotel receber&aacute; 200 cr&eacute;ditos<br>
<br>
<br>
Mais informa&ccedil;&otilde;es sobre os<i> <?php echo $sitename; ?> </i>cr&eacute;ditos <a href="<?php echo $path; ?>/help/2" target="_new">aqui</a>.</div> 
</div> 

		</div> 
		 
        <div class="smallprint"> 
			<style type="text/css">.method-group.online .method.idx0 h2 {font-size:26px !important;}</style> 
        </div> 
        
</div> 
<div class="method-group online premium clearfix"> 
<div class="method idx0"> 
	
<div class="method-content"> 		
<h2><b> VIP!</b></h2><br> 
<div class="summary clearfix">    
Deseja juntar-se ao club VIP?<br>
Divulgue bastante o <?php echo $shortname; ?> Hotel e consiga referidos para trocar por VIP.<br>
<br>
<b></div>	
</div>
 
</div></div>



<script type="text/javascript"> 
document.observe("dom:loaded", function() { new CreditsList(); });
</script> 


<div class="disclaimer" style="border: 0 !important;"> 
    <center>
  <h3><span>Como conseguir mais moedas?</span></h3> 
  <p>Participando de promoções e eventos realizados pela Equipe do Habbe!</p>
</center></div> 

	
</div> 
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script> 
			 
<div class="habblet-container ">		
<div class="cbb clearfix darkgray "> 
<div id="redeem-habblet"> 

<div class="redeem-balance"> 
 <p class="redeem-balance-username"><?php echo $name; ?></p>
 <p class="redeem-balance-text">Saldo:</p> 
 <p><span class="redeem-balance-amount"><?php echo $myrow['credits']; ?></span></p> 
</div> 
 
<div class="redeem-redeeming-text">
  <p class="redeeming-text">Introduza o c&oacute;digo para conseguir moedas</p>
</div> 
<div class="redeem-form-container clearfix"> 

<form method="post" action="credits" id="voucher-form">
<div class="redeem-redeeming"> 
    <div><input type="text" name="voucherCode" value="" id="purse-habblet-redeemcode-string" class="redeemcode" /></div> 
    <div class="redeem-redeeming-button"><a href="<?php echo $path; ?>/credits/#"<?php echo $target; ?> id="purse-redeemcode-button" class="new-button green-button redeem-submit"d="purse-redeemcode-button" class="new-button purse-icon"><b><span></span>Trocas</b><i></i></a></div> 
</form>

<div class="redeem-past-transactions"><a href="<?php echo $path; ?>/credits/transactions"<?php echo $target; ?>><b>Hist&oacute;rico de transa&ccedil;&otilde;es</b></a></div>

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