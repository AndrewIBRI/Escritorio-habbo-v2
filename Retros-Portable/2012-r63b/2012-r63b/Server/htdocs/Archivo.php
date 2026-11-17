<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Top Salas";
$pageid = "100";

include('./templates/community_subheader.php');
include('./templates/community_header.php');

?>


<div id="container">
<div id="content">

<div id="column7" class="column">

<div class="habblet-container">        
<div class="cbb clearfix blue">
<h2 class="title">Top - Salas Famosas</h2>
<div class="habblet box-content">

<table width="700" border="0" cellpadding="0" cellspacing="1" align="left">

 <tr>
  <td class='tablesubheader' width='5%' align='left'><span style="color: red" size:="" 2=""><b>Keko<b></span></td>
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
  <td class='tablerow1' align='left'><a href="./client?forwardId=2&roomId=<?php echo $row2['id']; ?>"><img src="http://javvox.com/Premiun_images/entrar-a-la-sala.png" border="0" 
                                onMouseOver="this.src='http://javvox.com/Premiun_images/entrar-a-la-sala-over.png';" onMouseOut="this.src='http://javvox.com/Premiun_images/entrar-a-la-sala.png';"/></a>
                                </td>

</td>
</tr>

<?php }?></table>

</div></div></div>


<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<?php include('./templates/community_footer.php'); ?>