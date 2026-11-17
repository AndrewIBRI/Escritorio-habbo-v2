<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Transacciones";
$pageid = "transactions";

if(isset($_POST['query'])){

	$get_cc = mysql_query("SELECT * FROM users WHERE username = '".$_POST['query']."'");
	$usercheck = mysql_num_rows($get_cc);
	if(mysql_num_rows($get_cc) > 0){

	$userdata = mysql_fetch_assoc($get_cc);

	$get_logs = mysql_query("SELECT * FROM cms_transactions WHERE userid = '".$userdata['id']."' ORDER BY date DESC LIMIT 100");
	$check = mysql_num_rows($get_logs);

		if($check > 0){
			$msg = "<div class='rounded rounded-green'><center>Transaktionen von ".$_POST['query']." (ID: ".$userdata['id']."). <img src=\"./images/check.gif\"></center></div>";
                	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Transaktionen','transactions.php','".$my_id."','".$userdata['id']."','".$date_full."')") or die(mysql_error());
		} else {
			$msg = "<div class='rounded rounded-red'><center>Error - No se han encontrado las transacciones <img src=\"./images/del.gif\"></center></div>";
		}
	} else {
		$msg = "<div class='rounded rounded-red'><center>Error - No se ha enconttrado el usuario <img src=\"./images/del.gif\"></center></div>";
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>

<form action='<?php echo $adminpath; ?>/index/p/transactions&do=search' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Transacciones</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nombre de usuario</strong><div class='graytext'>Escribe el nombre del usuario</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='query' value="<?php echo $_POST['query']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Suchen' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

<?php if($usercheck > 0){ ?>

 <div class='tableborder'>
 <div class='tableheaderalt'><center>100 &Uacute;ltimas transacciones</div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='25%'>Fecha</td>
  <td class='tablesubheader' width='50%'>Descripci&oacute;n</td>
  <td class='tablesubheader' width='25%'>Cantidad</td>
 </tr>

<?php while($row = mysql_fetch_assoc($get_logs)){ ?>

<tr>
  <td class='tablerow1'><?php echo $row['date']; ?></td>
  <td class='tablerow2'><?php echo $row['descr']; ?></td>
  <td class='tablerow2'><?php echo $row['amount']; ?></td>
</tr>

<?php } ?>

 </table>
</div>
<?php } ?>
	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>