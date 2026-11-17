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
<h2>Moedas Grátis!</h2><br> 

<div class="summary clearfix">    
Se você entrar no <?php echo $shortname; ?> todos os dias, você ganha 1000 Moedas e 500 Pixels!
			
</div> 
</div> 
   
 
</div></div>



<script type="text/javascript"> 
document.observe("dom:loaded", function() { new CreditsList(); });
</script> 


<div class="disclaimer" style="border: 0 !important;"> 
    <center><h3><span>Atenção, as moedas são totalmente grátis, você não precisa pagar absolutamente nada!</span></h3> 
  As moedas são dadas em um intervalo de geralmente 15 a 30 minutos!
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
 
<div class="redeem-redeeming-text"><p class="redeeming-text">Coloque um código de Moedas aqui:</p></div> 
<div class="redeem-form-container clearfix"> 

<form method="post" action="credits" id="voucher-form">
<div class="redeem-redeeming"> 
    <div><input type="text" name="voucherCode" value="" id="purse-habblet-redeemcode-string" class="redeemcode" /></div> 
    <div class="redeem-redeeming-button"><a href="<?php echo $path; ?>/credits/#"<?php echo $target; ?> id="purse-redeemcode-button" class="new-button green-button redeem-submit"d="purse-redeemcode-button" class="new-button purse-icon"><b><span></span>Gerar</b><i></i></a></div> 
</form>

<script type="text/javascript">
new PurseHabblet();
</script>

</div> 
</div> 

</div> 

<script type="text/javascript">
L10N.put("credits.navi.read_more", "Leia mais »");
L10N.put("credits.navi.close_fulltext", "Fechar instruções");
PaymentMethodHabblet.init();
</script>

<script type="text/javascript">
$("method-show-1").show();
$("method-full-2").hide();
</script>
	
</div> 
</div> 
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script></div>