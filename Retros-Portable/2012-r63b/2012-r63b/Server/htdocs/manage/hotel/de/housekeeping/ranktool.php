<?php

if($user_rank > 6){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Rank Manager";
$pageid = "ranktool";

if(isset($_POST['rank'])){

	$check = mysql_query("SELECT * FROM users WHERE username = '".$_POST['name']."' LIMIT 1") or die(mysql_error());
	$row = mysql_fetch_assoc($check);

	if($row['rank'] !== '7'){
		if(mysql_num_rows($check) > 0){
			mysql_query("UPDATE users SET rank = '".$_POST['rank']."' WHERE username = '".$_POST['name']."' LIMIT 1") or die(mysql_error()); 
			mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','".$_POST['name']." bekommt Rank ".$_POST['rank']."','ranktool.php','".$my_id."','".$row['id']."','".$date_full."')") or die(mysql_error());
			$msg = "<div class='rounded rounded-green'><center>Der Rank von ".$_POST['name']." wurde Erfolgreich geändert. <img src=\"./images/check.gif\"></center></div>";
		} else {
				$msg = "<div class='rounded rounded-red'><center>Error - No se ha podido encontrar al usuario <img src=\"./images/del.gif\"></center></div>";
		}
	} else {
		mysql_query("INSERT INTO stafflogs (action,message,note,userid,targetid,timestamp) VALUES ('Housekeeping','Access Denied to Rank Tool; Only the system administrator may give out rank 7!','ranktool.php','".$my_id."','".$row['id']."','".$date_full."')") or die(mysql_error());
		$msg = "<div class='rounded rounded-red'><center>Error - No se puede cambiar el rango de un administrador <img src=\"./images/del.gif\"></center></div>";
	}
}

$get_rank = mysql_query("SELECT * FROM ranks ORDER BY ID");
$get_users = mysql_query("SELECT * FROM users WHERE rank > 2 ORDER BY rank DESC");

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></p></strong><?php } ?>
 
<form action='<?php echo $adminpath; ?>/index/p/ranktool' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Editar rango</center></div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nombre de usuario</strong><div class='graytext'>Nombre de usuario al que deseas acmbiar el rango</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='name' value="<?php echo FilterText($_POST['name']); ?>" size='30' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Rango</strong><div class='graytext'>Rango que deseas darle</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><select name='rank'  class='dropdown' size='1'><?php while($rank = mysql_fetch_assoc($get_rank)){ ?>
<option value='<?php echo $rank['id']; ?>'><?php echo $rank['name']; ?></option><?php } ?>
</select>
</td>
</tr>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Cambiar' class='realbutton' accesskey='s'></td></tr>
</form></table></div><br />

 <div class='tableborder'>
 <div class='tableheaderalt'><center>Lavvo ~ Staff</center></div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='20%'>Nombre de usuario</td>
  <td class='tablesubheader' width='30%'>Mail</td>
  <td class='tablesubheader' width='20%'>IP</td>
  <td class='tablesubheader' width='20%'>Rango</td>
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