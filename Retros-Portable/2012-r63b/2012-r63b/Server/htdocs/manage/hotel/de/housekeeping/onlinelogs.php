<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Mitglieder Online";
$pageid = "onlinelogs";

$onlineCutOff = (time() - 601);
$onlineUsers = mysql_evaluate("SELECT COUNT(*) FROM users WHERE online > '0'");

$onlineUsers1 = mysql_evaluate("SELECT COUNT(*) FROM users WHERE gender = 'm' and online > '0'");
$onlineUsers2 = mysql_evaluate("SELECT COUNT(*) FROM users WHERE gender = 'f' and online > '0'");

@include('subheader.php');

?>
 <div class='tableborder'>
 <div class='tableheaderalt'>Usuarios conectados ahora(<?php echo $onlineUsers; ?>) || <?php echo $onlineUsers1; ?> Chicos & <?php echo $onlineUsers2; ?> chicas</div>
 <table cellpadding='4' cellspacing='0' width='100%'>
 <tr>
  <td class='tablesubheader' width='1%' align='center'>ID</td>
  <td class='tablesubheader' width='15%'>Nombre de usuario</td>
  <td class='tablesubheader' width='18%' align='left'>Correo electr&oacute;nico</td>
  <td class='tablesubheader' width='10%' align='left'>Fecha de registro</td>
  <td class='tablesubheader' width='10%' align='left'>&Uacute;ltima vez conectado</td>
  <td class='tablesubheader' width='1%' align='left'>Editar</td>
 </tr>
<?php

$get_users = mysql_query("SELECT * FROM users WHERE online > '0' ORDER BY username LIMIT " . $onlineUsers) or die(mysql_error());
while($row = mysql_fetch_assoc($get_users)){

?>

 <tr>
  <td class='tablerow1' align='center'><?php echo $row['id']; ?></td>
  <td class='tablerow2'><strong><?php echo $row['username']; ?> </strong><div class='desctext'><?php echo $row['ip_last']; ?> [<a href='http://who.is/whois-ip/ip-address/<?php echo $row['ip_last']; ?>/' target='_blank'>WHOIS</a>]</div></td>
  <td class='tablerow2' align='left'><a href='mailto:<?php echo $row['mail']; ?>'><?php echo $row['mail']; ?></a></td>
  <td class='tablerow2' align='left'><?php echo date('d.m.Y - H:i:s', $row['account_created']); ?></td>
  <td class='tablerow2' align='left'><?php echo date('d.m.Y - H:i:s', $row['last_online']); ?></td>
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