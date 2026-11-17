<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index.php?p=login"); exit; }

$pagename = "Lista de baneos";
$pageid = "banlogs";

$page = FilterText($_GET['page']);
$do = FilterText($_GET['do']);

$posts = mysql_evaluate("SELECT COUNT(*) FROM bans");
$pages = ceil(($posts + 0) / 50);

if($page > $pages || $page < 1){
	$page = 1;
}

@include('subheader.php');

?>
 <div class='tableborder'>
 <div class='tableheaderalt'><center>Bans actuales:  <?php echo mysql_evaluate("SELECT COUNT(*) FROM bans"); ?> / <?php echo mysql_evaluate("SELECT COUNT(*) FROM bans WHERE bantype = 'user'"); ?> usuarios baneados / <?php echo mysql_evaluate("SELECT COUNT(*) FROM bans WHERE bantype = 'ip'"); ?> IP's baneadas<br><br> P&aacute;ginas:
<?php
	for ($i = 1; $i <= $pages; $i++){
		if($page == $i){
			echo"<blink><b><font color=\"#000000\">".$i . "</font></b></blink>\n";
		} else {
			echo"<a href=\"./index.php?p=banlogs&page=" . $i . "\"><font color=\"#FFFFFF\">" . $i . "</a></font>\n ";
		}
	} 
?></div>
 <table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='1%' align='left'>ID</td>
  <td class='tablesubheader' width='8%' align='left'>Estado</td>
  <td class='tablesubheader' width='10%'>Valor</td> 
  <td class='tablesubheader' width='10%'>Raz&oacute;n</td>
  <td class='tablesubheader' width='10%' align='left'>Baneado por</td>
  <td class='tablesubheader' width='10%' align='left'>IP</td>
  <td class='tablesubheader' width='10%' align='left'>Desde</td>
  <td class='tablesubheader' width='10%' align='left'>Hasta</td>
  <td class='tablesubheader' width='1%' align='left'>IP-Bann</td>
 </tr>

<?php

$query_min = ($page * 50) - 50;

if($query_min < 0){ // Page 1
$query_min = 0;
}

$get_bans = mysql_query("SELECT * FROM bans WHERE expire + 3600 > '".time()."' ORDER BY expire LIMIT ".$query_min.", 50") or die(mysql_error());
while($row = mysql_fetch_assoc($get_bans)){

if($row['bantype'] == 'user'){

	$userdata = mysql_query("SELECT * FROM users WHERE username = '".$row['value']."'");
	$users = mysql_fetch_assoc($userdata);

	$ip_last = $users['ip_last'];
}else{
	$ip_last = '-/-';
}

$minuten = $row['expire'] - time();

if(time() >= $row['expire']){
$stat = "Expira el";
$color="green";
}elseif(time() + 3600 >= $row['expire']){
if(date('i', $minuten) > 0){
$stat = "(Hace ".date('i', $minuten)." minutos)";
$color="orange";
}else{
$stat = "(Hace ".date('s', $minuten)." segundos)";
$color="orange";
} }else{
$stat = "Activado";
$color="red";
}

if($row['bantype'] == 'user'){ $img = "del"; }else{ $img = "check"; } ?>

  <tr>
  <td class='tablerow1' align='left'><?php echo $row['id']; ?></td>
  <td class='tablerow1' align='left'><font color="<?php echo $color; ?>"><?php echo $stat; ?></font></td>
  <td class='tablerow1' align='left'><font color="<?php echo $color; ?>"><?php echo $row['value']; ?></font></td>
  <td class='tablerow1' align='left'><font color="<?php echo $color; ?>"><?php echo $row['reason']; ?></font></td>
  <td class='tablerow1' align='left'><font color="<?php echo $color; ?>"><?php echo $row['added_by']; ?></font></td>
  <td class='tablerow1' align='left'><font color="<?php echo $color; ?>"><?php echo $ip_last; ?></font></td>
  <td class='tablerow1' align='left'><font color="<?php echo $color; ?>"><?php echo date('d.m.Y - H:i:s', $row['added_date']); ?></font></td>
  <td class='tablerow1' align='left'><font color="<?php echo $color; ?>"><?php echo date('d.m.Y - H:i:s', $row['expire']); ?></font></td>
  <td class='tablerow1' align='left'><center><img src="<?php echo $path; ?>/manage/hotel/de/housekeeping/images/<?php echo $img; ?>.gif"></center></td>
  </tr>

<?php } ?>
 
 </table>
</div>
	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div>

<!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>