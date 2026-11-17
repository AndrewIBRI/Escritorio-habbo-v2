<?php

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index/p/login"); exit; }

$pagename = "Home";
$pageid = "home";

@include('subheader.php');
@include('header.php');

?>

<table cellpadding='0' cellspacing='8' width='100%' id='tablewrap'>
<tr>

<td width='100%' valign='top' id='rightblock'>

<div><div style='font-size:30px; padding-left:7px; letter-spacing:-2px; border-bottom:1px solid #EDEDED'>
<center>
  <img src="<?php echo $path; ?>/manage/hotel/de/housekeeping/images/paineldegerenciamento.png" />
</center>
</div>
<table cellpadding='0' cellspacing='8' width='100%' id='tablewrap'>
<tr>	<td width='100%' valign='top' id='rightblock'>
	 <div><!-- RIGHT CONTENT BLOCK -->
<div id='ipb-get-members' style='border:1px solid #000; background:#FFF; padding:2px;position:absolute;width:120px;display:none;z-index:100'></div>
<!--in_dev_notes-->
<!--in_dev_check-->
<table border='0' width='100%' cellpadding='0' cellspacing='4'>
<tr>
<td valign='top' width='75%'>
<table border='0' width='100%' cellpadding='0' cellspacing='0'>
<tr>
<td>

<table width='100%' cellspacing='0' cellpadding='4'>
<tr>
			  
<?php //////////// System/Statistik Liste //////////// ?>
<td width='15%' valign='top'>
<div class='tableborder'>
<div class='tableheaderalt'><center>Sistema ~ Estat&iacute;sticas</center></div>
<table width='100%' cellpadding='4' cellspacing='0'>
<tr>
  <td class='tablesubheader' width='50%'>Nome</td>
  <td class='tablesubheader' width='50%'>Estat&iacute;stica</td>
</tr>

<tr>
  <td class='tablerow1'>Emblemas</td>
  <td class='tablerow1'><?php echo mysql_evaluate("SELECT COUNT(*) FROM user_badges"); ?></td>
</tr>
<tr>
  <td class='tablerow2'>Bans</td>
  <td class='tablerow2'><a href="<?php echo $adminpath; ?>/index/p/banlogs"><?php echo mysql_evaluate("SELECT COUNT(*) FROM bans"); ?></a></td>
</tr>
<tr>
  <td class='tablerow1'>Banners</td>
  <td class='tablerow1'><a href="<?php echo $adminpath; ?>/index/p/banners"><?php echo mysql_evaluate("SELECT COUNT(*) FROM cms_banners"); ?></a></td>
</tr>
<tr>
  <td class='tablerow2'>FAQs</td>
  <td class='tablerow2'><a href="<?php echo $adminpath; ?>/index/p/faqs"><?php echo mysql_evaluate("SELECT COUNT(*) FROM help_topics"); ?></a></td>
</tr>
<tr>
  <td class='tablerow1'>Grupos</td>
  <td class='tablerow1'><a href="<?php echo $adminpath; ?>/index/p/groups"><?php echo mysql_evaluate("SELECT COUNT(*) FROM group_details"); ?></a></td>
</tr>
<tr>
  <td class='tablerow2'>Campanhas</td>
  <td class='tablerow2'><a href="<?php echo $adminpath; ?>/index/p/campaigns"><?php echo mysql_evaluate("SELECT COUNT(*) FROM cms_hotcampaigns"); ?></a></td>
</tr>
<tr>
  <td class='tablerow1'>Feira Livre</td>
  <td class='tablerow1'><a href="<?php echo $adminpath; ?>/index/p/marktplatz"><?php echo mysql_evaluate("SELECT COUNT(*) FROM cms_marktplatz"); ?></a></td>
</tr>
<tr>
  <td class='tablerow2'>Minimails</td>
  <td class='tablerow2'><?php echo mysql_evaluate("SELECT COUNT(*) FROM cms_minimail"); ?></a></td>
</tr>
<tr>
  <td class='tablerow1'>Usu&aacute;rios</td>
  <td class='tablerow1'><a href="<?php echo $adminpath; ?>/index/p/users"><?php echo mysql_evaluate("SELECT COUNT(*) FROM users"); ?></a></td>
</tr>
<tr>
  <td class='tablerow2'>Usu&aacute;rios conectados</td>
  <td class='tablerow2'><a href="<?php echo $adminpath; ?>/index/p/onlinelogs"><?php echo mysql_evaluate("SELECT COUNT(*) FROM users WHERE online = '1'"); ?></a></td>
</tr>
<tr>
  <td class='tablerow1'>Mobis</td>
  <td class='tablerow1'><?php echo mysql_evaluate("SELECT COUNT(*) FROM items"); ?></td>
</tr>
<tr>
  <td class='tablerow2'>Not&iacute;cias</td>
  <td class='tablerow2'><a href="<?php echo $adminpath; ?>/index/p/news"><?php echo mysql_evaluate("SELECT COUNT(*) FROM cms_news"); ?></a></td>
</tr>
<tr>
  <td class='tablerow1'>Quartos</td>
  <td class='tablerow1'><?php echo mysql_evaluate("SELECT COUNT(*) FROM rooms"); ?></td>
</tr>
<tr>
  <td class='tablerow2'>Quartos oficiais</td>
  <td class='tablerow2'><?php echo mysql_evaluate("SELECT COUNT(*) FROM rooms WHERE owner =''"); ?></td>
</tr>
<tr>
  <td class='tablerow1'>Stafflogs</td>
  <td class='tablerow1'><a href="<?php echo $adminpath; ?>/index/p/stafflogs"><?php echo mysql_evaluate("SELECT COUNT(*) FROM stafflogs"); ?></a></td>
</tr>
<tr>
  <td class='tablerow2'>Tags</td>
  <td class='tablerow2'><?php echo mysql_evaluate("SELECT COUNT(*) FROM user_tags"); ?></td>
</tr>
<tr>
  <td class='tablerow1'>C&oacute;digos de Moedas</td>
  <td class='tablerow1'><a href="<?php echo $adminpath; ?>/index/p/vouchers"><?php echo mysql_evaluate("SELECT COUNT(*) FROM credit_vouchers"); ?></a></td>
</tr>
<tr>
  <td class='tablerow2'>Transa&ccedil;&otilde;es</td>
  <td class='tablerow2'><a href="<?php echo $adminpath; ?>/index/p/transactions"><?php echo mysql_evaluate("SELECT COUNT(*) FROM cms_transactions"); ?></a></td>
</tr>

</table>
</div></div>

<?php //////////// Staff Liste //////////// ?>
<td width='42%' valign='top'>

<div class='tableborder'>
<div class='tableheaderalt'><center><?php echo $sitename; ?> ~ Staffs</center></div>
<table width='100%' cellpadding='4' cellspacing='0'>
<tr>
  <td class='tablesubheader' width='10%'>Nome</td>
  <td class='tablesubheader' width='10%'>Cargo</td>
  <td class='tablesubheader' width='10%'>E-mail</td>
  <td class='tablesubheader' width='1%' align='center'>Status</td>
  <td class='tablesubheader' width='1%' align='center'>Editar</td>
</tr>

<?php

$get_em = mysql_query("SELECT * FROM users WHERE rank > 3 && rank < 8 ORDER BY username ASC") or die(mysql_error());
while($row = mysql_fetch_assoc($get_em)){

$rank = mysql_query("SELECT * FROM ranks WHERE id = '".$row['rank']."'");
$rank = mysql_fetch_assoc($rank);

?>

<tr>
 <td class='tablerow<?php if($row['username'] == $name){ echo'3'; }else{ echo'2'; } ?>'>
<a href='../../../../home/<?php echo $row['username']; ?>' target='_blank'><?php echo $row['username']; ?></a></strong> (ID: <?php echo $row['id']; ?>)
</td>

<td class='tablerow<?php if($row['username'] == $name){ echo'3'; }else{ echo'2'; } ?>'>
<?php echo $rank['name']; ?>
</td>

<td class='tablerow<?php if($row['username'] == $name){ echo'3'; }else{ echo'2'; } ?>'>
<a href='mailto:<?php echo $row['mail']; ?>'><?php echo $row['mail']; ?></a>
</td>

<td class='tablerow<?php if($row['username'] == $name){ echo'3'; }else{ echo'2'; } ?>' align='center'>
<?php if($row['online'] >= "1"){ ?>
	<img src="<?php echo $path; ?>/web-gallery/v2/images/online.gif">
<?php } else { ?>
	<img src="<?php echo $path; ?>/web-gallery/v2/images/offline.gif">
<?php } ?>

</td>

<td class='tablerow<?php if($row['username'] == $name){ echo'3'; }else{ echo'2'; } ?>' align='center'>
<a href='<?php echo $adminpath; ?>/index/p/users_edit&key=<?php echo $row['id']; ?>'><img src='./images/edit.gif' alt='Editar datos del usuario'></a>
</td>

</tr>
<?php } ?>

</table>
</div></div>

</div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table></table>
<?php require_once('footer.php'); ?>