 <?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Equipe";
$pageid = "11b";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content">
				 
<style>
.ssh {
float:left;
width:70px;
height: 100px;
-moz-border-radius:5px;
-webkit-border-radius:5px;
margin-left:5px;
margin-right:5px;
border-radius:5px;
text-align:center;
border:3px solid #000;
background: #ffffff; /* Old browsers */
background: -moz-linear-gradient(top, #ffffff 0%, #f1f1f1 50%, #e1e1e1 51%, #f6f6f6 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#ffffff), color-stop(50%,#f1f1f1), color-stop(51%,#e1e1e1), color-stop(100%,#f6f6f6)); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top, #ffffff 0%,#f1f1f1 50%,#e1e1e1 51%,#f6f6f6 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top, #ffffff 0%,#f1f1f1 50%,#e1e1e1 51%,#f6f6f6 100%); /* Opera 11.10+ */
background: -ms-linear-gradient(top, #ffffff 0%,#f1f1f1 50%,#e1e1e1 51%,#f6f6f6 100%); /* IE10+ */
background: linear-gradient(top, #ffffff 0%,#f1f1f1 50%,#e1e1e1 51%,#f6f6f6 100%); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#f6f6f6',GradientType=0 ); /* IE6-9 */
}
.ssh:hover {
background: #ffffff; /* Old browsers */
background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#ffffff), color-stop(50%,#f3f3f3), color-stop(51%,#ededed), color-stop(100%,#ffffff)); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%); /* Opera 11.10+ */
background: -ms-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%); /* IE10+ */
background: linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 ); /* IE6-9 */
}
</style>




<?php

$i = 0;

$getem = mysql_query("SELECT * FROM ranks WHERE id > 9 ORDER BY id DESC") or die(mysql_error());
while ($rank = mysql_fetch_assoc($getem)) {


?>

<div id="column1" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue">
<img align="right" style="position:absolute; margin-left:400px; z-index:2; float:right" src="http://i.imgur.com/r6GVFFA.gif"><h2 class="title"><span style="float: center;">Gerentes</span> </h2>

<?php 

$userdata = mysql_query("SELECT * FROM users WHERE rank = '9' ORDER BY last_online DESC");
while($row = mysql_fetch_assoc($userdata)){

$i++;

$badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '1'");
$badge2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '2'");
$badge3 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '3'");
$badge4 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '4'");
$badge5 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '5'");

$get_groups = mysql_query("SELECT * FROM group_members WHERE id_user = '".$row['id']."' AND is_current = '1'");
$members_row = mysql_fetch_assoc($get_groups);


if(mysql_num_rows($badge) > 0){
	$badgerow = mysql_fetch_assoc($badge);
	$badge = "<img src=\"".$cimagesurl.$badgesurl.$badgerow['badge_id'].".gif\">";
}else{
	$badge = "<br>";
}

if(mysql_num_rows($badge2) > 0){
	$badgerow2 = mysql_fetch_assoc($badge2);
        $badge2 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow2['badge_id'].".gif\">";
}else{
	$badge2 = "<br>";
}

if(mysql_num_rows($badge3) > 0){
	$badgerow3 = mysql_fetch_assoc($badge3);
        $badge3 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow3['badge_id'].".gif\">";
}else{
	$badge3 = "";
}


if(mysql_num_rows($badge4) > 0){
	$badgerow4 = mysql_fetch_assoc($badge4);
        $badge4 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow4['badge_id'].".gif\">";
}else{
	$badge4 = "";
}


if(mysql_num_rows($badge5) > 0){
	$badgerow5 = mysql_fetch_assoc($badge5);
        $badge5 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow5['badge_id'].".gif\">";
}else{
	$badge5 = "";
}


if(mysql_num_rows($get_groups) > 0){
	$grouprow = mysql_query("SELECT * FROM group_details WHERE id = '".$members_row['id_group']."' LIMIT 1");
	$group = mysql_fetch_assoc($grouprow);

	$groupbadge = "<a href=\"".$path."/groups/".$group['id']."\"><img src=\"".$path."/habbo-imaging/badge.php?badge=".$group['badge'].".gif\"></a>";
}else{
	$groupbadge = "<br>";
}

$timeon = mysql_query("SELECT * FROM user_stats WHERE id = '".$row['id']."'");
$timeon = mysql_fetch_assoc($timeon);
$fintime = $timeon['OnlineTime'];

 $timeE = 500000;
$timeMB = 450000;
 $timeB = 400000;
 $timeR = 350000;
 $timeM = 300000;

if(IsEven($i)){
	$even = "even";
} else {
	$even = "odd";
}

if($row['online'] == "1"){
	$image = "online_anim_big";
} else {
	$image = "offline_big";
}

?>

<li class="odd">
<p><div class="ssh"><center><img style="margin-top: -10px;" src="<?php echo $avatar; echo $row['look']; ?>&action=drk=6&direction=2&head_direction=3&gesture=sml&size=b"><br/><br></div> 
<b><?php echo $row['username']; ?></b></span>&nbsp<br />
<br>
<img src='./web-gallery/images/myhabbo/habbo_<?php echo $image; ?>.gif' border='0'><br></br>
<b>Miss&atilde;o:</b> <i><?php echo $row['motto']; ?></i><br>
<b>Último login: </b><?php echo $row['last_online']; ?><br>
<b>Função:</b> <?php echo $row['working']; ?><br />
 <br> <br> </td> </p></table>
</li>
<?php if($row['rank'] > 3){ ?><?php } ?>

<?php } ?>
</div></div>
<?php } ?>





<?php

$i = 0;

$getem = mysql_query("SELECT * FROM ranks WHERE id > 9 ORDER BY id DESC") or die(mysql_error());
while ($rank = mysql_fetch_assoc($getem)) {


?>

<div id="column1" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue">
<img align="right" style="position:absolute; margin-left:400px; z-index:2; float:right" src="http://i.imgur.com/hV8f51R.gif"><h2 class="title"><span style="float: center;">Administradores</span> </h2>

<?php 

$userdata = mysql_query("SELECT * FROM users WHERE rank = '8' ORDER BY last_online DESC");
while($row = mysql_fetch_assoc($userdata)){

$i++;

$badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '1'");
$badge2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '2'");
$badge3 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '3'");
$badge4 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '4'");
$badge5 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '5'");

$get_groups = mysql_query("SELECT * FROM group_members WHERE id_user = '".$row['id']."' AND is_current = '1'");
$members_row = mysql_fetch_assoc($get_groups);


if(mysql_num_rows($badge) > 0){
	$badgerow = mysql_fetch_assoc($badge);
	$badge = "<img src=\"".$cimagesurl.$badgesurl.$badgerow['badge_id'].".gif\">";
}else{
	$badge = "<br>";
}

if(mysql_num_rows($badge2) > 0){
	$badgerow2 = mysql_fetch_assoc($badge2);
        $badge2 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow2['badge_id'].".gif\">";
}else{
	$badge2 = "<br>";
}

if(mysql_num_rows($badge3) > 0){
	$badgerow3 = mysql_fetch_assoc($badge3);
        $badge3 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow3['badge_id'].".gif\">";
}else{
	$badge3 = "";
}


if(mysql_num_rows($badge4) > 0){
	$badgerow4 = mysql_fetch_assoc($badge4);
        $badge4 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow4['badge_id'].".gif\">";
}else{
	$badge4 = "";
}


if(mysql_num_rows($badge5) > 0){
	$badgerow5 = mysql_fetch_assoc($badge5);
        $badge5 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow5['badge_id'].".gif\">";
}else{
	$badge5 = "";
}


if(mysql_num_rows($get_groups) > 0){
	$grouprow = mysql_query("SELECT * FROM group_details WHERE id = '".$members_row['id_group']."' LIMIT 1");
	$group = mysql_fetch_assoc($grouprow);

	$groupbadge = "<a href=\"".$path."/groups/".$group['id']."\"><img src=\"".$path."/habbo-imaging/badge.php?badge=".$group['badge'].".gif\"></a>";
}else{
	$groupbadge = "<br>";
}

$timeon = mysql_query("SELECT * FROM user_stats WHERE id = '".$row['id']."'");
$timeon = mysql_fetch_assoc($timeon);
$fintime = $timeon['OnlineTime'];

 $timeE = 500000;
$timeMB = 450000;
 $timeB = 400000;
 $timeR = 350000;
 $timeM = 300000;

if(IsEven($i)){
	$even = "even";
} else {
	$even = "odd";
}

if($row['online'] == "1"){
	$image = "online_anim_big";
} else {
	$image = "offline_big";
}

?>

<li class="odd">
<p><div class="ssh"><center><img style="margin-top: -10px;" src="<?php echo $avatar; echo $row['look']; ?>&action=drk=6&direction=2&head_direction=3&gesture=sml&size=b"><br/><br></div> 
<b><?php echo $row['username']; ?></b></span>&nbsp<br />
<br><img src='./web-gallery/images/myhabbo/habbo_<?php echo $image; ?>.gif' border='0'><br></br>
<b>Miss&atilde;o:</b> <i><?php echo $row['motto']; ?></i><br>
<b>Último login: </b><?php echo $row['last_online']; ?><br>
<b>Função:</b> <?php echo $row['working']; ?><br />
 <br> <br> </td> </p></table>
</li>
<?php if($row['rank'] > 3){ ?><?php } ?>

<?php } ?>
</div></div>
<?php } ?>





<?php

$i = 0;

$getem = mysql_query("SELECT * FROM ranks WHERE id > 9 ORDER BY id DESC") or die(mysql_error());
while ($rank = mysql_fetch_assoc($getem)) {


?>

<div id="column1" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue">
<img align="right" style="position:absolute; margin-left:400px; z-index:2; float:right" src="http://i.imgur.com/AyUtwga.gif"><h2 class="title"><span style="float: center;">Moderadores</span> </h2>

<?php 

$userdata = mysql_query("SELECT * FROM users WHERE rank = '7' ORDER BY last_online DESC");
while($row = mysql_fetch_assoc($userdata)){

$i++;

$badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '1'");
$badge2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '2'");
$badge3 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '3'");
$badge4 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '4'");
$badge5 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '5'");

$get_groups = mysql_query("SELECT * FROM group_members WHERE id_user = '".$row['id']."' AND is_current = '1'");
$members_row = mysql_fetch_assoc($get_groups);


if(mysql_num_rows($badge) > 0){
	$badgerow = mysql_fetch_assoc($badge);
	$badge = "<img src=\"".$cimagesurl.$badgesurl.$badgerow['badge_id'].".gif\">";
}else{
	$badge = "<br>";
}

if(mysql_num_rows($badge2) > 0){
	$badgerow2 = mysql_fetch_assoc($badge2);
        $badge2 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow2['badge_id'].".gif\">";
}else{
	$badge2 = "<br>";
}

if(mysql_num_rows($badge3) > 0){
	$badgerow3 = mysql_fetch_assoc($badge3);
        $badge3 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow3['badge_id'].".gif\">";
}else{
	$badge3 = "";
}


if(mysql_num_rows($badge4) > 0){
	$badgerow4 = mysql_fetch_assoc($badge4);
        $badge4 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow4['badge_id'].".gif\">";
}else{
	$badge4 = "";
}


if(mysql_num_rows($badge5) > 0){
	$badgerow5 = mysql_fetch_assoc($badge5);
        $badge5 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow5['badge_id'].".gif\">";
}else{
	$badge5 = "";
}


if(mysql_num_rows($get_groups) > 0){
	$grouprow = mysql_query("SELECT * FROM group_details WHERE id = '".$members_row['id_group']."' LIMIT 1");
	$group = mysql_fetch_assoc($grouprow);

	$groupbadge = "<a href=\"".$path."/groups/".$group['id']."\"><img src=\"".$path."/habbo-imaging/badge.php?badge=".$group['badge'].".gif\"></a>";
}else{
	$groupbadge = "<br>";
}

$timeon = mysql_query("SELECT * FROM user_stats WHERE id = '".$row['id']."'");
$timeon = mysql_fetch_assoc($timeon);
$fintime = $timeon['OnlineTime'];

 $timeE = 500000;
$timeMB = 450000;
 $timeB = 400000;
 $timeR = 350000;
 $timeM = 300000;

if(IsEven($i)){
	$even = "even";
} else {
	$even = "odd";
}

if($row['online'] == "1"){
	$image = "online_anim_big";
} else {
	$image = "offline_big";
}

?>

<li class="odd">
<p><div class="ssh"><center><img style="margin-top: -10px;" src="<?php echo $avatar; echo $row['look']; ?>&action=drk=6&direction=2&head_direction=3&gesture=sml&size=b"><br/><br></div>
<b><?php echo $row['username']; ?></b></span>&nbsp<br />
<br><img src='./web-gallery/images/myhabbo/habbo_<?php echo $image; ?>.gif' border='0'><br></br>
<b>Miss&atilde;o:</b> <i><?php echo $row['motto']; ?></i><br>
<b>Último login: </b><?php echo $row['last_online']; ?><br>
<b>Função:</b> <?php echo $row['working']; ?><br />
 <br> <br> </td> </p></table>
</li>
<?php if($row['rank'] > 3){ ?><?php } ?>

<?php } ?>
</div></div>
<?php } ?>



</div></div></div>







<?php

$i = 0;

$getem = mysql_query("SELECT * FROM ranks WHERE id > 9 ORDER BY id ASC") or die(mysql_error());
while ($rank = mysql_fetch_assoc($getem)) {


?>

<div id="column2" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue ">
<img align="right" style="position:absolute; margin-left:250px; z-index:2; float:right" src="http://i.imgur.com/Ynt0Ziu.gif"><h2 class="title"><span style="float: center;">CEO</span> </h2>

<?php 

$userdata = mysql_query("SELECT * FROM users WHERE rank = '10' ORDER BY last_online DESC");
while($row = mysql_fetch_assoc($userdata)){

$i++;

$badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '1'");
$badge2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '2'");
$badge3 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '3'");
$badge4 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '4'");
$badge5 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = '5'");

$get_groups = mysql_query("SELECT * FROM group_members WHERE id_user = '".$row['id']."' AND is_current = '1'");
$members_row = mysql_fetch_assoc($get_groups);


if(mysql_num_rows($badge) > 0){
	$badgerow = mysql_fetch_assoc($badge);
	$badge = "<img src=\"".$cimagesurl.$badgesurl.$badgerow['badge_id'].".gif\">";
}else{
	$badge = "<br>";
}

if(mysql_num_rows($badge2) > 0){
	$badgerow2 = mysql_fetch_assoc($badge2);
        $badge2 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow2['badge_id'].".gif\">";
}else{
	$badge2 = "<br>";
}

if(mysql_num_rows($badge3) > 0){
	$badgerow3 = mysql_fetch_assoc($badge3);
        $badge3 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow3['badge_id'].".gif\">";
}else{
	$badge3 = "";
}


if(mysql_num_rows($badge4) > 0){
	$badgerow4 = mysql_fetch_assoc($badge4);
        $badge4 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow4['badge_id'].".gif\">";
}else{
	$badge4 = "";
}


if(mysql_num_rows($badge5) > 0){
	$badgerow5 = mysql_fetch_assoc($badge5);
        $badge5 = "<img src=\"".$cimagesurl.$badgesurl.$badgerow5['badge_id'].".gif\">";
}else{
	$badge5 = "";
}


if(mysql_num_rows($get_groups) > 0){
	$grouprow = mysql_query("SELECT * FROM group_details WHERE id = '".$members_row['id_group']."' LIMIT 1");
	$group = mysql_fetch_assoc($grouprow);

	$groupbadge = "<a href=\"".$path."/groups/".$group['id']."\"><img src=\"".$path."/habbo-imaging/badge.php?badge=".$group['badge'].".gif\"></a>";
}else{
	$groupbadge = "<br>";
}

$timeon = mysql_query("SELECT * FROM user_stats WHERE id = '".$row['id']."'");
$timeon = mysql_fetch_assoc($timeon);
$fintime = $timeon['OnlineTime'];

 $timeE = 500000;
$timeMB = 450000;
 $timeB = 400000;
 $timeR = 350000;
 $timeM = 300000;

if(IsEven($i)){
	$even = "even";
} else {
	$even = "odd";
}

if($row['online'] == "1"){
	$image = "online_anim_big";
} else {
	$image = "offline_big";
}

?>



<li class="odd">				<p><div class="ssh"><center><img style="margin-top: -10px;" src="<?php echo $avatar; echo $row['look']; ?>&action=drk=6&direction=2&head_direction=3&gesture=sml&size=b"><br/><br></div> 
<b><?php echo $row['username']; ?></b></span>&nbsp<br />
<br><img src='./web-gallery/images/myhabbo/habbo_<?php echo $image; ?>.gif' border='0'><br></br>
<b>Miss&atilde;o:</b> <i><?php echo $row['motto']; ?></i><br>
<b>Último Login:</b> <?php echo $row['last_online']; ?><br />
<b>Função:</b> <?php echo $row['working']; ?><br />
 <br> <br> </td> </p></table>
</li>
<?php } ?></div><?php } ?>







<div id="column2" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix red ">
<h2 class="title">Sobre a Equipe</h2>
<div class="habblet box-content">
<p>O Habbie é formado por uma equipe bastante competente e disponível para lhe ajudar e certificar que tudo está funcionando corretamente, para ajudar ajudar nossos usuários e mantê-los seguros.

<br></br><img src="http://i.imgur.com/D4vvKK9.gif" align="right">Os membros da Equipe Habbie Hotel possuem um emblema especial <b>"Habbie Staff"</b> para mostrar claramente que eles são funcionários legítimos do Hotel. É impossível os usuários obterem este emblema. Caso um jogador do Hotel afirme ser um membro da Equipe e não puder mostrar o seu emblema de identificação, pedimos a você que reporte a Equipe para que possamos tomar as medidas cabíveis.

<br></br>Caso você tenha alguma dúvida, procure de imediato um membro da Equipe para que ele possa lhe esclarer o que você quiser.</div>
</div></div></div></div>


<?php require_once('./templates/community_footer.php'); ?>