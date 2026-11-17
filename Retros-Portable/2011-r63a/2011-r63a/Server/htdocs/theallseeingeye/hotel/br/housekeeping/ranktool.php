<?php
session_start();
if($user_rank > 7){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Cargos administrativos";
$pageid = "ranktool";

if(isset($_POST['rank'])){

	$check = mysql_query("SELECT * FROM users WHERE username = '".$_POST['name']."' LIMIT 1") or die(mysql_error());
	$row = mysql_fetch_assoc($check);

	if($row['rank'] !== '5'){
		if(mysql_num_rows($check) > 0){
			mysql_query("UPDATE users SET rank = '".$_POST['rank']."' WHERE username = '".$_POST['name']."' LIMIT 1") or die(mysql_error()); 
			$msg = "<div class='rounded rounded-green'><center>Der Rank von ".$_POST['name']." wurde Erfolgreich geändert. <img src=\"./images/check.gif\"></center></div>";
		} else {
				$msg = "<div class='rounded rounded-red'><center>Não foi possível encontrar o usuário <img src=\"./images/del.gif\"></center></div>";
		}
	} else {
		$msg = "<div class='rounded rounded-red'><center>Não é possível alterar este cargo <img src=\"./images/del.gif\"></center></div>";
	}
}

$get_rank = mysql_query("SELECT * FROM ranks ORDER BY ID");
$get_users = mysql_query("SELECT * FROM users WHERE rank > 2 ORDER BY rank DESC");

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/ranktool' method='post' name='theAdminForm' id='theAdminForm'>
<input type="hidden" value="<?=md5(session_id());?>" name="csrf">
<div class='tableborder'>
<div class='tableheaderalt'><center>Editar Cargos</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nome do usuário</strong><div class='graytext'>Nome do usuário que deseja dar cargo</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo FilterText($_POST['name']); ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Cargo</strong><div class='graytext'>Cargo a dar ao usuário</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='rank'  class='dropdown' size='1'><?php while($rank = mysql_fetch_assoc($get_rank)){ ?>
<option value='<?php echo $rank['id']; ?>'><?php echo $rank['name']; ?></option><?php } ?>
</select>
</td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Salvar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Equipe staff</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='20%'>Nome</td>
  <td class='tablesubheader' width='30%'>Email</td>
  <td class='tablesubheader' width='20%'>IP</td>
  <td class='tablesubheader' width='20%'>Cargo</td>
  <td class='tablesubheader' width='1%' align='center'>Editar</td>
 </tr>
<?php

while($row = mysql_fetch_assoc($get_users)){
$rank = mysql_query("SELECT * FROM ranks WHERE id = '".$row['rank']."' ");
$rank = mysql_fetch_assoc($rank);

?>
<tr>
  <td class='tablerow1' align='left'><?php echo $row['username']; ?> (ID: <?php echo $row['id']; ?>)</td>
  <td class='tablerow1' align='left'><?php echo $row['mail']; ?></td>
  <td class='tablerow1' align='left'><?php echo $row['ip_last']; ?></td>
  <td class='tablerow2'><?php echo $rank['name']; ?></td>
  <td class='tablerow2' align='center'><a href='<?php echo $adminpath; ?>/index/p/users_edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Edit User Data'></a></td>
</tr>

<?php } ?>
 
 </table>
</div>
	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>