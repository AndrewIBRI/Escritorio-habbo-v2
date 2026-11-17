<?php
#####################################################################
#||||| zCMS - [Private BETA] build 1.1.5 - Desenvolvido por nadi0s |#
#||||| Criado por nadi0s - HTML, PHP, CSS                      |||||#     
#||||| Criado por @rTiago - DATA_CLASSES, Housekeeping         |||||#
# ----------------------------------------------------------------- #
#||||| © Copyright 2012. LavvosCMS -  Estrutura                |||||#
#||||| © Copyright 2012. Sulake Corporation - Habbo Arquivos   |||||#
#####################################################################

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Equipe";
$pageid = "12";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content" style="position: relative; left: 0px; top: 0px;" class="clearfix">




<?php

$i = 0;

$getem = mysql_query("SELECT * FROM ranks WHERE id > 6 ORDER BY id DESC") or die(mysql_error());
while ($rank = mysql_fetch_assoc($getem)) {
$num = mysql_num_rows(mysql_query("SELECT * FROM users WHERE rank = '".$rank['id']."' ORDER BY id"));

?>

<div id="column1" class="column">
<div id="container">
                
                    
							<div id="contentBox" class="right" style="width:443px;">
<div id="contentTitle" class="blue"><?php echo $rank['name']; if(($num) > 1){ echo ''; } ?></div><div style="padding:0px">

<?php 

$userdata = mysql_query("SELECT * FROM users WHERE rank = '".$rank['id']."' ORDER BY id");
while($row = mysql_fetch_assoc($userdata)){

$i++;

$badge = mysql_query("SELECT badge_id FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = 1");
$badge2 = mysql_query("SELECT badge_id FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = 2");
$badge3 = mysql_query("SELECT badge_id FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = 3");
$badge4 = mysql_query("SELECT badge_id FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = 4");
$badge5 = mysql_query("SELECT badge_id FROM user_badges WHERE user_id = '".$row['id']."' AND badge_slot = 5");

$get_groups = mysql_query("SELECT * FROM group_memberships WHERE userid = '".$row['id']."'");
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
	$grouprow = mysql_query("SELECT * FROM groups_details WHERE id = '".$members_row['groupid']."' LIMIT 1");
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
	$even = "odd";
} else {
	$even = "even";
}

?>




<li class="<?php echo $even; ?>">

<p><div class="ssh"><center><img style="margin-top: -15px;" src="<?php echo $avatar; echo $row['look']; ?>&size=b&direction=2&head_direction=3&action=std&gesture=sml&size=1" align='left'><br></center></div>
									
								<div id="slickTitle" class="newsAuthor">
										<a href="<?php echo $path; ?>/home/<?php echo $row['username']; ?>" style="text-decoration:none;color:#666;"><?php echo $row['username']; ?></a> 
										<small></small>
										<span>
											Missão: </b><?php echo $row['motto']; ?>											
										</span>
									</div><br clear="all">

</li>

<?php  }?>

</div></div><br>
<?php } ?>

</div></div></div></div></div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<div id="column2" class="column">
<div id="contentBox" class="right" style="width:290px;">
<div id="contentTitle" class="blue">BOX DIARE</div><div style="padding:5px">
                                <div style="padding:5px">
                                    Não sei o que vou colocar aqui, talvez <strong>informações</strong> sobre a equipe staff, essa CMS não é de <strong>minha
                                   autoria</strong> é uma CMS baseada em <strong>lavvos</strong>, CMS privada para uso de hotéis escolhido por nossos staffs. 
<strong><h4> JE4N, POWER PIXEL FÓRUM, HIRBO HOTEL, KILER, HOUSHED, PAUL WALKER</h4></strong> 

                                    <br><br>
                                    <center><img src="<?php echo $path; ?>/client/r63/c_images/album1584/ADM.gif"></center>
                                </div>
                            </div>
							

</div>

</div></div></div></div></div></div>




<?php require_once('./templates/community_footer.php'); ?>