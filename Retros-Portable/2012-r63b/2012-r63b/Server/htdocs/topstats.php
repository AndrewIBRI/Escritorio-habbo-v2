<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Top's";
$pageid = "14";
$body_id = "credits";

include('./templates/community_subheader.php');
include('./templates/community_header.php');

?>

<div id="container">

<!-- / Table left - Créditos -->
<div id="content" style="position: relative" class="clearfix">
<div id="column" class="column">
<div class="habblet-container ">
<div class="cbb clearfix orange ">  
<h2 class="title"><span style="float: left;">Créditos</span> </h2>
<div align="left"> 
<table width="100%">
<tr>
	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 5 ORDER BY credits DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>

<tr><td width="5px"></td>
<td width="20px">
<img src="<?php echo $avatar; echo $sql['look']; ?>&size=s&direction=2&head_direction=2&gesture=sml&size=m" align="left"></td>
<td width="195px"><a href="<?php echo $path; ?>/home/<?php echo $sql['username']; ?>"><b><?php echo $sql['username']; ?></b></a><br />
<span style=" : 0px 0px 7px black"><span style="color: black" size:="" 2=""><?php echo $sql['credits']; ?> Créditos</span></td>
	  <?php } ?>
</tr>
</table>
</div> </div>

</div> 

<!-- / Table center - Online -->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix orange ">  
<h2 class="title"><span style="float: left;">Online</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$userstats_a = mysql_query("SELECT * FROM user_stats ORDER BY OnlineTime DESC LIMIT 5");
while($userstats = mysql_fetch_assoc($userstats_a)){
$row = mysql_fetch_assoc($row = mysql_query("SELECT * FROM users WHERE id = '".$userstats['id']."' LIMIT 5"));

$time = 3600*24; // 60*60*24

?>

<tr><td width="5px"> </td>
<td width="20px"><img src="<?php echo $avatar; echo $row['look']; ?>&size=s&direction=2&head_direction=2&gesture=sml&size=m" align="left"></td>
<td width="195px"><a href="../home/<?php echo $row['username']; ?>"><b><?php echo $row['username']; ?></b></a><br />
<span style=" : 0px 0px 7px purple"><span style="color: purple" size:="" 2=""><?php echo round($userstats['OnlineTime'] / $time); ?> Online</span></td>
</tr>

<?php } ?>
</table>
</div> </div>

</div> 
</div> </div> 

<!-- / Table center - Pixels -->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Pixels</span> </h2> 
<div align="left"> 
<table width="100%">
<tr>
	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 5 ORDER BY activity_points DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>
	  <tr><td width="5px"> </td>
            <td width="20px"><img src="<?php echo $avatar; echo $sql['look']; ?>&size=s&direction=2&head_direction=2&gesture=sml&size=m" align="left"></td> <td width="195px"><a href="../home/<?php echo $sql['username']; ?>"><b><?php echo $sql['username']; ?></b></a><br />
<span style=" : 0px 0px 7px green"><span style="color: green" size:="" 2=""><?php echo $sql['activity_points']; ?> Pixels</span></td>
	  <?php } ?>
</tr>
</table>
</div> </div>

</div> 

<!-- / Table center - Presentes enviados-->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Presentes enviados</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$userstats_a = mysql_query("SELECT * FROM user_stats ORDER BY GiftsGiven DESC LIMIT 5");
while($userstats = mysql_fetch_assoc($userstats_a)){
$row = mysql_fetch_assoc($row = mysql_query("SELECT * FROM users WHERE id = '".$userstats['id']."' LIMIT 5"));

?>

<tr><td width="5px"> </td>
<td width="20px"><img src="<?php echo $avatar; echo $row['look']; ?>&size=s&direction=2&head_direction=2&gesture=sml&size=m" align="left"></td>
<td width="195px"><a href="../home/<?php echo $row['username']; ?>"><b><?php echo $row['username']; ?></b></a><br />
<span style=" : 0px 0px 7px gray"><span style="color: gray" size:="" 2=""><?php echo $userstats['GiftsGiven']; ?> Presentes enviados</span></td>
</tr>

<?php } ?>
</table>
</div> </div>

</div> 
</div> </div> 



<!-- / Table right - Respeitos -->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix green ">  
<h2 class="title"><span style="float: left;">Respeitos</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$userstats_a = mysql_query("SELECT * FROM user_stats ORDER BY Respect DESC LIMIT 5");
while($userstats = mysql_fetch_assoc($userstats_a)){
$row = mysql_fetch_assoc($row = mysql_query("SELECT * FROM users WHERE id = '".$userstats['id']."' LIMIT 5"));

?>

<tr><td width="5px"> </td>
<td width="20px"><img src="<?php echo $avatar; echo $row['look']; ?>&size=s&direction=2&head_direction=2&gesture=sml&size=m" align="left"></td>
<td width="195px"><a href="../home/<?php echo $row['username']; ?>"><b><?php echo $row['username']; ?></b></a><br />
<span style=" : 0px 0px 7px red"><span style="color: red" size:="" 2=""><?php echo $userstats['Respect']; ?> Respeitos</span></td>
</tr>

<?php } ?>
</table>
</div> </div>

</div> 

<!-- / Table right - Presentes recebidos -->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix green ">  
<h2 class="title"><span style="float: left;">Presentes recebidos</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$i = 0;
$userstats_a = mysql_query("SELECT * FROM user_stats ORDER BY GiftsReceived DESC LIMIT 5");
while($userstats = mysql_fetch_assoc($userstats_a)){

$row = mysql_fetch_assoc($row = mysql_query("SELECT * FROM users WHERE id = '".$userstats['id']."' LIMIT 5"));

$i++;

?>

<tr><td width="5px"> </td>
<td width="20px"><img src="<?php echo $avatar; echo $row['look']; ?>&size=s&direction=2&head_direction=2&gesture=sml&size=m" align="left"></td>
<td width="195px"><a href="../home/<?php echo $row['username']; ?>"><b><?php echo $row['username']; ?></b></a><br />
<span style=" : 0px 0px 7px blue"><span style="color: blue" size:="" 2=""><?php echo $userstats['GiftsReceived']; ?> Presentes recebidos</span></td>
</tr>

<?php } ?>
</table>
</div> </div>

</div> 
</div> </div> 

<script type="text/javascript">
		HabboView.run();
	</script>


<div id="column7" class="column">

<div class="habblet-container">        
<div class="cbb clearfix red">
<h2 class="title">Top - Quartos Famosos</h2>
<div class="habblet box-content">

<table width="700" border="0" cellpadding="0" cellspacing="1" align="left">

 <tr>
  <td class='tablesubheader' width='5%' align='left'><span style="color: red" size:="" 2=""><b>Avatar<b></span></td>
  <td class='tablesubheader' width='5%' align='left'><span style="color: red" size:="" 2=""><b>Usuario<b></span></td> 
  <td class='tablesubheader' width='5%' align='left'><span style="color: red" size:="" 2=""><b>Contagem<b></span></td>
  <td class='tablesubheader' width='20%' align='left'><span style="color: red" size:="" 2=""><b>Sala<b></span></td>
  <td class='tablesubheader' width='10%' align='left'></td>

 </tr>

<?php

$toprooms_a = mysql_query("SELECT * FROM rooms ORDER BY score DESC LIMIT 5");
while($toprooms = mysql_fetch_assoc($toprooms_a)){
$row = mysql_fetch_assoc($row = mysql_query("SELECT * FROM users WHERE username = '".$toprooms['owner']."' LIMIT 5"));
$row2 = mysql_fetch_assoc($row2 = mysql_query("SELECT * FROM rooms WHERE caption = '".$toprooms['caption']."' LIMIT 5"));

?>

<tr>
  <td class='tablerow1' align='left'><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=<?php echo $row['look']; ?>&action=sit&size=b&direction=4&head_direction=3&gesture=sml" align="left"></td>
  <td class='tablerow1' align='left'><a href="../home/<?php echo $row['username']; ?>"><b><?php echo $row['username']; ?></td>
  <td class='tablerow1' align='left'><center><span style="color: green" size:="" 2=""><b><?php echo $row2['score']; ?></b></span></center></td>
  <td class='tablerow1' align='left'><span style="color: blue" size:="" 2=""><b><?php echo $row2['caption']; ?></b></a></td>
  <td class='tablerow1' align='left'><a href="./client?forwardId=2&roomId=<?php echo $row2['id']; ?>"><img src="<?php echo $path; ?>/Premiun_images/entrar-a-la-sala.png" border="0" 
                                onMouseOver="this.src='<?php echo $path; ?>/Premiun_images/entrar-a-la-sala-over.png';" onMouseOut="this.src='<?php echo $path; ?>/Premiun_images/entrar-a-la-sala.png';"/></a>
                                </td>

</td>
</tr>

<?php }?></table>

</div></div></div>

				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>


</div>

<?php include('./templates/community_footer.php'); ?>