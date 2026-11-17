<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Staffs";
$pageid = "13";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content">
<?php

$i = 0;

$getem = mysql_query("SELECT * FROM ranks WHERE id > 3 ORDER BY id DESC") or die(mysql_error());
while ($rank = mysql_fetch_assoc($getem)) {


?>
				 
<div id="column1" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue">
<h2 class="title"><?php echo $rank['name']; ?></h2>

<?php 

$userdata = mysql_query("SELECT * FROM users WHERE rank = '".$rank['id']."' ORDER BY last_online DESC");
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

if($row['online'] == "1"){
	$image = "online_anim_big";
} else {
	$image = "offline_big";
}

if(IsEven($i)){
	$even = "even";
} else {
	$even = "odd";
}



?>

<li class="<?php echo $even; ?>">
<p><img src="<?php echo $avatar; echo $row['look']; ?>&size=b&direction=4&head_direction=3&action=wlk&gesture=sml&size=s" align='left'><br>
<b><a href='../home/<?php echo $row['username']; ?>'><?php echo $row['username']; ?></a></b>&nbsp;<span style="font-weight: normal; font-size: 80%;">(ID: <b><?php echo $row['id']; ?></b>)</span><span style="float: right;"><img src='./web-gallery/images/myhabbo/habbo_<?php echo $image; ?>.gif' border='0'></span><br />
<i><span style=": 0px 0px 7px blue"><span style="color: black" size:="" 2=""><?php echo $row['motto']; ?></span></i><br />
 <span style=": 0px 0px 7px purple"><span style="color: brown " size:="" 2=""><?php echo $timerp; ?></span></span><br />
<?php if($row['rank'] > 3){ ?><span style=": 0px 0px 7px red"><?php } ?>
<span><?php echo $badge; ?> <?php echo $badge2; ?> <?php echo $badge3; ?> <?php echo $badge4; ?> <?php echo $badge5; ?></p><hr>
</li>
<?php } ?>
</div></div>
<?php } ?>

</div></div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div></div>
</div></div>
<div id="column2" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue ">
<h2 class="title">Quem são os Staffs?</h2>
<img src="http://i.imgur.com/C5Mnw.png" align='left'><img src="http://i.imgur.com/Ieo3y.png" align='right'><span style=": 0px 0px 0px #000000"><span style="color: #198CA3" size:="" 2="">
<br><i><center>O pessoal do hotel tem placas especiais que você pode reconhecê-los. Eles estão aqui para ajudar você precisa e se precisar de ajuda basta olhar em qualquer sala ou contatá-los através do console ou uma ferramenta para ajudar o hotel.</a>.</center></i>

</div>
</div>
</div>
                                               
<div id="column2" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue ">
<h2 class="title">Qual é o seu trabalho?</h2>

<div class="habblet box-content">

<table width="107%" style="padding: 5px; margin-left: -15px; background-color: #fff">
<tbody>
<tr>

<span style=": 0px 0px 7px black"><span style="color: #357DD5" size:="" 2=""><b>Donos</b></span><br>
<span style=": 0px 0px 7px #31455e">Você está em carga <?php echo $shortname; ?> Hotel trabalho diário, e é responsável por aconselhar sobre os desenvolvimentos e tomar grandes decisões</span><br><br>

<span style=": 0px 0px 7px black"><span style="color: #357DD5" size:="" 2=""><b>Gerentes</b></span><br>
<span style=": 0px 0px 7px #31455e">Pronto para criar concursos e eventos! Eles são também para responder a quaisquer perguntas que fazer ou para ajudar com qualquer problema</span><br><br>

<span style=": 0px 0px 7px black"><span style="color: #357DD5" size:="" 2=""><b>Administradores</b></span><br>
<span style=": 0px 0px 7px #31455e">Eles cuidam do hotel está em perfeito estado! , Sem eles, o hotel é nada tão respeitá-los </span><br><br>

<span style=": 0px 0px 7px black"><span style="color: #357DD5" size:="" 2=""><b>Moderadores Supremos</b></span><br>
<span style=": 0px 0px 7px #31455e">Os  Moderadores Supremos têm controle total-los no hotel, você pode puni-los por violar as Regas, então cuidado com eles!  </span><br><br>

<span style=": 0px 0px 7px black"><span style="color: #357DD5" size:="" 2=""><b>Moderadores</b></span><br>
<span style=": 0px 0px 7px #31455e">Moderadores são especialmente aqueles que podem ajudar, porque eles são muito capazitados, ouedes preguntrales Então o que você quer </span><br><br>

<span style=": 0px 0px 7px black"><span style="color: #357DD5" size:="" 2=""><b>Guias</b></span><br>
<span style=": 0px 0px 7px #31455e">Eles são testados na equipe, mas toda a administração como tomadores de decisão e que merecem o mesmo respeito dos outros </span><br><br>
</tr>
</tbody>
</table>
	
</div>
</div>



</div></div>

</div></div>


<?php require_once('./templates/community_footer.php'); ?>