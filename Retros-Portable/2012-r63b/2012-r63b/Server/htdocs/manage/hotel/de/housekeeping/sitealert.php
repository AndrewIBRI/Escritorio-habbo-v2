<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Alertas";

if($_POST['submit']){
        	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Seitenalert','sitealert.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
		mysql_query("UPDATE cms_settings SET value = '".$_POST['status']."' WHERE variable = 'cms_sitealert'");
		mysql_query("UPDATE cms_settings SET value = '".$_POST['text']."' WHERE variable = 'cms_sitealert_text' LIMIT 1") or die(mysql_error());
		$msg = "<div class='rounded rounded-green'><center>Alertas atualizados corretamente. <img src=\"./images/check.gif\"></center></div>";
}

$get_cms_sitealert = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_sitealert'");
$cms_sitealert = mysql_fetch_assoc($get_cms_sitealert);

$get_cms_sitealert_text = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_sitealert_text'");
$cms_sitealert_text = mysql_fetch_assoc($get_cms_sitealert_text);

$pageid = "sitealert"; 

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<form action='<?php echo $adminpath; ?>/index/p/sitealert' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Alertas</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Estado</b>
<div class='graytext'>Deseja manter o alerta ativo?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='status'  class='dropdown'>
									<option value='0'>Ativar</option>
									<option value='1' <?php if($cms_sitealert['value'] == "1"){ echo "selected='selected'"; } ?>>Desativar</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Alerta</b>
<div class='graytext'>Texto de alerta</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><textarea name='text' cols='60' rows='5' wrap='soft' id='sub_desc'   class='multitext'><?php echo $cms_sitealert_text['value']; ?></textarea></td>
</tr>

<tr>
<tr>
<td align='center' class='tablesubheader' colspan='2' >
<input type='submit' name='submit' value='Atertar' class='realbutton' accesskey='s'>
</td>
</tr>

</form></table></div><br />	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>