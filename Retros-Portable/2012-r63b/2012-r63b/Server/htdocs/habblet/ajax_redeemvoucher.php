<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

$credits = FilterText($myrow['credits']); 
$voucher = FilterText($_POST['voucherCode']); 

$check = mysql_query("SELECT value FROM credit_vouchers WHERE code = '" . $voucher . "' LIMIT 1") or die(mysql_error()); 
$valid = mysql_num_rows($check); 

if($valid > 0){ 
    $tmp = mysql_fetch_assoc($check); 
    $amount = $tmp[value]; 
    $resultcode = "green";  
    $result = "<div class='rounded rounded-green'>Du hast ".$credits." bekommen!</div>";
    $credits = $credits + $amount; 
    mysql_query("UPDATE users SET credits = '".$credits."' WHERE username = '" . FilterText($name) . "' LIMIT 1") or die(mysql_error()); 
    mysql_query("DELETE FROM credit_vouchers WHERE code = '" . $voucher . "' LIMIT 1") or die(mysql_error()); 
    mysql_query("INSERT INTO `cms_transactions` (`date`, `amount`, `descr`, `userid`) VALUES ('".$date_full."', '".$amount."', 'Talercode', '".$my_id."');") or die(mysql_error()); // Appearently ' is not good enough, has to be fancy ` =/ 
    $result = "<div class='rounded rounded-green'>Acabas de recibir " . $amount . " creditos.</div>"; 

    $resultcode = "green"; 
    $result = "<div class='rounded rounded-green'>&iexcl;Has recibido ".$amount." cr&eacute;ditos!</div>"; 
} else { 
    $resultcode = "red"; 
    $result = "<div class='rounded rounded-red'>El c&oacute;digo introducido es incorrecto. Int&eacute;ntelo de nuevo.</div></div>"; 
} 

?>

<div class="redeem-result">
<div class="rounded-container">

<form method="post" action="credits" id="voucher-form">
<div class="redeem-redeeming"> 
<div><input type="text" name="voucherCode" value="" id="purse-habblet-redeemcode-string" class="redeemcode" /></div>
<div class="redeem-redeeming-button">
<a href="#" id="purse-redeemcode-button" class="new-button green-button redeem-submit"d="purse-redeemcode-button" class="new-button purse-icon">
<b><span></span>Canjear</b><i></i></a></div> 
</form> <div style="height: 15px;"></div><br><br>
	<?php echo $result; ?>
<br>

<div class="redeem-past-transactions"><a href="<?php echo $path; ?>/credits/transactions"><b>Ver mis transacciones</b></a></div>
<script type="text/javascript">
new PurseHabblet();
</script>