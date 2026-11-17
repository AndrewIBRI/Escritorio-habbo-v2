<?php

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

if($user_rank > 6){

$pagename = "Editar Emblemas";
$pageid = "badgetool";

if(isset($_POST['badge']) && ($_POST['name'])){

	$check_name = mysql_query("SELECT * FROM users WHERE username = '".mysql_real_escape_string($_POST['name'])."'") or die(mysql_error());
	if(mysql_num_rows($check_name) > 0){

		$userdata = mysql_fetch_assoc($check_name);

		$check_badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_id = '".mysql_real_escape_string($_POST['badge'])."' LIMIT 1") or die(mysql_error());
			
		if($_POST['action'] == "give"){
	
			if(mysql_num_rows($check_badge) < 1){
				mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$userdata['id']."','".mysql_real_escape_string($_POST['badge'])."','0')") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center>Você acabou de dar à <b>".mysql_real_escape_string($_POST['name'])."</b> o emblema " . mysql_real_escape_string($_POST['badge']) . " con &eacute;xito. <img src=\"./images/check.gif\"></center></div>";
			} else {
				$msg = "<div class='rounded rounded-red'><center>".$_POST['name']." Já têm o Emblema " . $_POST['badge'] . ". <img src=\"./images/del.gif\"></center></div>";
			}

		} else {

		if(mysql_num_rows($check_badge) > 0){
				mysql_query("DELETE FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_id = '".mysql_real_escape_string($_POST['badge'])."'") or die(mysql_error());
				$msg = "<div class='rounded rounded-green'><center>Você removeu o Emblema " . mysql_real_escape_string($_POST['badge']) . " . <img src=\"./images/check.gif\"></center></div>";
			} else {
				$msg = "<div class='rounded rounded-red'><center>".mysql_real_escape_string($_POST['name'])." no tiene la placa " . mysql_real_escape_string($_POST['badge']) . " <img src=\"./images/del.gif\"></center></div>";
			}

		}

	} else {
		$msg = "<div class='rounded rounded-red'><center>Não foi possível encontrar o usuário! <img src=\"./images/del.gif\"></center></div>";
	}
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/badgetool&do=something' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Administrar Emblemas</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong><?php echo $shortname; ?> Nome</strong><div class='graytext'>Nome do usuário que irá dar o Emblema</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo $_POST['name']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Código ou ID do Emblema</strong><div class='graytext'>Exemplo: ADM, NWB, HBA<br></div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='badge' value="<?php echo $_POST['badge']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><b>Opção</b></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='action' class='dropdown'><option value='give'>Dar</option><option value='delete'>Remover</option></select>
</td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Continuar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); }else{ require_once('error.php'); exit; } ?>