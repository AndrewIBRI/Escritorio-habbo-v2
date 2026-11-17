<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Top's";
$pageid = "96";
$body_id = "credits";

include('./templates/community_subheader.php');
include('./templates/community_header.php');

?>

<div id="container">

<!-- / Table left - Créditos -->
<div id="content" style="position: relative" class="clearfix">
<div id="column" class="column">
<div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Moedas</span> </h2>
<div align="left"> 
<table width="100%">
<tr>
	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 4 ORDER BY credits DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>

<tr><td width="5px"></td>
<td width="20px">
<img src="<?php echo $avatar; echo $sql['look']; ?>&action=wav&direction=3&head_direction=3&gesture=sml&size=s" align="left"></td>
<td width="195px"><?php echo $sql['username']; ?><br />
<span style="color: blue" size:="" 2=""><?php echo $sql['credits']; ?> Moedas</span></td>
	  <?php } ?>
</tr>
</table>
</div> </div>
</div> </div>



<!-- / Table center - Duckets -->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Duckets</span> </h2> 
<div align="left"> 
<table width="100%">
<tr>
	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 4 ORDER BY activity_points DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>
	  <tr><td width="5px"> </td>
            <td width="20px"><img src="<?php echo $avatar; echo $sql['look']; ?>&action=wav&direction=3&head_direction=3&gesture=sml&size=s" align="left"></td> <td width="195px"><?php echo $sql['username']; ?><br />
<span style="color: blue" size:="" 2=""><?php echo $sql['activity_points']; ?> Duckets</span></td>
	  <?php } ?>
</tr>
</table>
</div> </div>
</div> </div>

<!-- / Table center - Pontos de Conquista-->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Pontos de Conquista</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 4 ORDER BY achievement_points DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>
	  <tr><td width="5px"> </td>
            <td width="20px"><img src="<?php echo $avatar; echo $sql['look']; ?>&action=wav&direction=3&head_direction=3&gesture=sml&size=s" align="left"></td> 
<td width="195px"><?php echo $sql['username']; ?><br />
<span style="color: blue" size:="" 2=""><?php echo $sql['achievement_points']; ?> Conquistas</span></td>
	  <?php } ?>
</table>
</div> </div>
</div> </div>

<!-- / Table center - Pontos online-->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Pontos online</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 4 ORDER BY points_online DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>
	  <tr><td width="5px"> </td>
            <td width="20px"><img src="<?php echo $avatar; echo $sql['look']; ?>&action=wav&direction=3&head_direction=3&gesture=sml&size=s" align="left"></td> 
<td width="195px"><?php echo $sql['username']; ?><br />
<span style="color: blue" size:="" 2=""><?php echo $sql['points_online']; ?> Pontos</span></td>
	  <?php } ?>
</table>
</div> </div>
</div> </div>


<!-- / Table center - Respeitos-->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Respeitos</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 5 ORDER BY respect DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>
	  <tr><td width="5px"> </td>
            <td width="20px"><img src="<?php echo $avatar; echo $sql['look']; ?>&action=wav&direction=3&head_direction=3&gesture=sml&size=s" align="left"></td> 
<td width="195px"><?php echo $sql['username']; ?><br />
<span style="color: blue" size:="" 2=""><?php echo $sql['respect']; ?> Respeitos</span></td>
	  <?php } ?>
</table>
</div> </div>
</div> </div>


<!-- / Table center - Diamantes-->
<div id="column" class="column"><div class="habblet-container ">
<div class="cbb clearfix blue ">  
<h2 class="title"><span style="float: left;">Diamantes</span> </h2> 
<div align="left"> 
<table width="100%">

<tr>	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 5 ORDER BY vip_points DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>
	  <tr><td width="5px"> </td>
            <td width="20px"><img src="<?php echo $avatar; echo $sql['look']; ?>&action=wav&direction=3&head_direction=3&gesture=sml&size=s" align="left"></td> 
<td width="195px"><?php echo $sql['username']; ?><br />
<span style="color: blue" size:="" 2=""><?php echo $sql['vip_points']; ?> Diamantes</span></td>
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
  <td class='tablerow1' align='left'><a href="./client?forwardId=2&roomId=<?php echo $row2['id']; ?>"><img src="<?php echo $path; ?>/entrar.png" border="0" 
                                onMouseOver="this.src='<?php echo $path; ?>/entrar.png';" onMouseOut="this.src='<?php echo $path; ?>/entrar.png';"/></a>
                                </td>

</td>
</tr>

<?php }?></table>

</div></div></div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>


</div>

<?php include('./templates/community_footer.php'); ?>