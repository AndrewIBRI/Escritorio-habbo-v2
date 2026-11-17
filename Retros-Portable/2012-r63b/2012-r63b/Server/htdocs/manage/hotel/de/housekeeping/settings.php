<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Configura&ccedil;&atilde;o";

if($do == "save"){
if($_POST['url']){

	mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Einstellungen geändert','settings.php','".$my_id."','0','".$date_full."')") or die(mysql_error());
	mysql_query("UPDATE cms_settings SET value = '".$_POST['url']."' WHERE variable = 'cms_url'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['ip']."' WHERE variable = 'client_ip'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['port']."' WHERE variable = 'client_port'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['mus']."' WHERE variable = 'client_mus'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['variables']."' WHERE variable = 'client_variables'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['texts']."' WHERE variable = 'client_texts'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['rts_uname']."' WHERE variable = 'rts_uname'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['cms_name']."' WHERE variable = 'cms_name'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['maintenance']."' WHERE variable = 'cms_maintenance'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['flashclient']."' WHERE variable = 'cms_flashclient'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['register']."' WHERE variable = 'cms_register'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['clientlimit']."' WHERE variable = 'cms_clientlimit'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['swf_path']."' WHERE variable = 'client_swf_path'");
	mysql_query("UPDATE cms_settings SET value = '".$_POST['habbo_swf']."' WHERE variable = 'client_habbo_swf'");

	$msg = "<div class='rounded rounded-green'><center>Configura&ccedil;&atilde;o guardada com sucesso! <img src=\"./images/check.gif\"></center></div>";
}else{
	$msg = "<div class='rounded rounded-red'><center>Não foi possível salvar as alterações. <img src=\"./images/del.gif\"></center></div>";
}
}

$client_port = mysql_fetch_assoc($client_port = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_port'"));
$client_mus = mysql_fetch_assoc($client_mus = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_mus'"));
$client_ip = mysql_fetch_assoc($client_ip = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_ip'"));
$client_variables = mysql_fetch_assoc($client_variables = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_variables'"));
$client_texts = mysql_fetch_assoc($client_texts = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_texts'"));
$client_swf_path = mysql_fetch_assoc($client_swf_path = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_swf_path'"));
$client_habbo_swf = mysql_fetch_assoc($client_habbo_swf = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_habbo_swf'"));

$cms_url = mysql_fetch_assoc($cms_url = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_url'"));
$cms_name = mysql_fetch_assoc($cms_name = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_name'"));
$cms_maintenance = mysql_fetch_assoc($cms_maintenance = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_maintenance'"));
$cms_flashclient = mysql_fetch_assoc($cms_flashclient = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_flashclient'"));
$cms_register = mysql_fetch_assoc($cms_register = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_register'"));
$cms_clientlimit = mysql_fetch_assoc($cms_clientlimit = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_clientlimit'"));

$rts_uname = mysql_fetch_assoc($rts_uname = mysql_query("SELECT * FROM cms_settings WHERE variable = 'rts_uname'"));

$pageid = "settings";

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<form action='<?php echo $adminpath; ?>/index/p/settings&do=save' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Configuraci&oacute;n del sitio</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>URL de la CMS(Hotel)</b><div class='graytext'><?php echo $cms_url['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='url' value="<?php echo $cms_url['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>IP del client</b><div class='graytext'><?php echo $client_ip['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='ip' value="<?php echo $client_ip['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Puerto de conexi&oacute;n</b><div class='graytext'><?php echo $client_port['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='port' value="<?php echo $client_port['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Puerto de conexi&oacute;n MUS</b><div class='graytext'><?php echo $client_mus['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='mus' value="<?php echo $client_mus['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>External variables</b><div class='graytext'><?php echo $client_variables['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='variables' value="<?php echo $client_variables['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>External texts</b><div class='graytext'><?php echo $client_texts['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='texts' value="<?php echo $client_texts['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Url del client</b><div class='graytext'><?php echo $client_swf_path['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='swf_path' value="<?php echo $client_swf_path['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Habbo.SWF</b><div class='graytext'><?php echo $client_habbo_swf['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='habbo_swf' value="<?php echo $client_habbo_swf['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre de usuario de Otaku</b><div class='graytext'><?php echo $rts_uname['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='rts_uname' value="<?php echo $rts_uname['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Nombre de la CMS</b><div class='graytext'><?php echo $cms_name['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='cms_name' value="<?php echo $cms_name['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Clientlimit</b><div class='graytext'><?php echo $cms_clientlimit['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='clientlimit' value="<?php echo $cms_clientlimit['value']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Mantenimiento</b><div class='graytext'><?php echo $cms_maintenance['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='maintenance'  class='dropdown'>
									<option value='1'>Activar</option>
									<option value='0' <?php if($cms_maintenance['value'] == "0"){ echo "selected='selected'"; } ?>>Desactivar</option>
								    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Flashclient</b><div class='graytext'><?php echo $cms_flashclient['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='flashclient'  class='dropdown'>
									<option value='1'>Activar</option>
									<option value='0' <?php if($cms_flashclient['value'] == "0"){ echo "selected='selected'"; } ?>>Desactivar</option>
						    </select>
</td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Registro</b><div class='graytext'><?php echo $cms_register['description']; ?></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='register'  class='dropdown'>
									<option value='1'>Desactivado</option>
									<option value='0' <?php if($cms_register['value'] == "0"){ echo "selected='selected'"; } ?>>Activado</option>
								    </select>
</td>
</tr>

<tr>
<tr>
<td align='center' class='tablesubheader' colspan='2' >
<input type='submit' value='Guardar' class='realbutton' accesskey='s'>
</td>
</tr>

</form></table></div><br />	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>