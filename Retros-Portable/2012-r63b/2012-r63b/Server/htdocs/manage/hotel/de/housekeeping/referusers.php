<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Revisar Referidos";
$pageid = "referusers";

if($do == "search" && $_POST['uname']){


		$tmp = mysql_query("SELECT id FROM users WHERE username = '".$_POST['uname']."' LIMIT 1") or die(mysql_error());
		$tmp = mysql_num_rows($tmp);

	if($tmp > 0){

                                $usuario = $_POST['uname'];
				$usuario= mysql_real_escape_string($usuario);
			
                                $datau = mysql_evaluate("SELECT COUNT(*) AS aantalleden FROM users_referidos WHERE usuario ='".$usuario."' ORDER BY ID");
			        $msg = "<div class='rounded rounded-green'><center>Revisa os referidos ".$_POST['uname']." abaixo. <img src=\"./images/check.gif\"></center></div><br>";
	}else{
		$msg = "<div class='rounded rounded-red'><center>Error - Não foi possível localizar o usuário <img src=\"./images/del.gif\"></center></div>";


}
    }


@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>

<form action='<?php echo $adminpath; ?>/index/p/referusers&do=search' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Buscar Referidos</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nome de usuario</strong><div class='graytext'>Por favor, digite o nome do usuário ao qual você deseja Pesquisar.</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='uname' value="<?php echo $_POST['uname']; ?>" size='30' class='textinput'></td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Aceitar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Usuário Referido Por Você</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='20%' align='center'>Usuario</td>
  <td class='tablesubheader' width='15%' align='center'>Referidos</td>
 </tr>


<tr>
  <td class='tablerow1' align='left'><center><b><?php echo $usuario; ?></b></center></td>
  <td class='tablerow1' align='left'><center><b><?php echo $datau; ?></b></center></td>
</tr>

 
 </table>
</div>
	 </div>
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>