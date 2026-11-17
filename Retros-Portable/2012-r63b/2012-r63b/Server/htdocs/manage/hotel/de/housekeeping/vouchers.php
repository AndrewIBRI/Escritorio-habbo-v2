<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; } 
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; } 

$pagename = "Vouchers"; 
$pageid = "vouchers";

if(isset($_POST['submit'])){ 

	if(!empty($_POST['voucher']) && !empty($_POST['credits'])){ 

		mysql_query("INSERT INTO credit_vouchers (code,value) VALUES ('".$_POST['voucher']."','".$_POST['credits']."')") or die(mysql_error()); 
		mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Talercode erstellt: ".FilterText($_POST['voucher']).", Wert: ".FilterText($_POST['credits'])." Taler','vouchers.php','".$my_id."','0','".$date_full."')") or die(mysql_error()); 
		$msg = "<div class='rounded rounded-green'><center>Codigo creado correctamente <img src=\"./images/check.gif\"></center></div>"; 
            
	} else { 
        	$msg = "<div class='rounded rounded-red'><center>No has rellenado todos los campos. <img src=\"./images/del.gif\"></center></div>"; 
	} 
}

function randomVoucher($code) {
	$characters = "1234567890abdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	$key = $characters{rand(0,71)};
	for($i=1;$i<$code;$i++){
		$key .= $characters{rand(0,71)};
	}
	return $key;
}

$get_vouchers = mysql_query("SELECT * FROM credit_vouchers") or die(mysql_error());

@include('subheader.php'); 

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?> 
  
<form action='<?php echo $adminpath; ?>/index/p/vouchers&do=create' method='post' name='theAdminForm' id='theAdminForm'> 
<div class='tableborder'> 
<div class='tableheaderalt'><center>Crerar c&oacute;digos vouchers</center></div> 

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'> 
<tr> 
<td class='tablerow1'  width='40%'  valign='middle'><strong>C&oacute;digo</strong><div class='graytext'>C&oacute;digo clave</div></td> 
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='voucher' value="<?php echo randomVoucher(8); ?>" size='30' class='textinput'></td> 
</tr> 

<tr> 
<td class='tablerow1'  width='40%'  valign='middle'><strong>Cantidad</strong><div class='graytext'>Cantidad de creditos</div></td> 
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='credits' value="" size='30' class='textinput'></td> 
</tr> 

<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Guardar' name="submit" class='realbutton' accesskey='s'></td></tr> 
</form></table></div><br /> 

<?php 
?> 

 <div class='tableborder'> 
 <div class='tableheaderalt'><center>C&oacute;digos actuales</center></div> 
 <table cellpadding='4' cellspacing='0' width='100%'> 

 <tr> 
  <td class='tablesubheader' width='60%' align='center'>C&oacute;digo</td> 
  <td class='tablesubheader' width='40%' align='center'>Cr&eacute;ditos</td> 
 </tr> 

<?php while($row = mysql_fetch_assoc($get_vouchers)){ ?>
<tr> 
  <td class='tablerow1' align='center'><?php echo $row['code']; ?></td> 
  <td class='tablerow2' align='center'><?php echo $row['value']; ?></td> 
</tr>
<?php } ?>
  
 </table> 
</div> 
     </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>