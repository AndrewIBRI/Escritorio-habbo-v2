<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Ajudantes";
$pageid = "13a";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content"> 
<div id="column1" class="column">

<div class="habblet-container ">        
<div class="cbb clearfix red ">
<h2 class="title"><?php echo $shortname; ?> Ajudantes</h2>


<?php 

$i = 0;

$getXlers = mysql_query("SELECT * FROM users WHERE rank = '3' ORDER BY last_online DESC"); 
while($Xler = mysql_fetch_assoc($getXlers)){

$i++;

        if(IsEven($i)){
            $even = "odd";
        } else {
            $even = "even";
        }


$badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$Xler['id']."' AND badge_slot = '1'");
$badge2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$Xler['id']."' AND badge_slot = '2'");
$badge3 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$Xler['id']."' AND badge_slot = '3'");
$badge4 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$Xler['id']."' AND badge_slot = '4'");
$badge5 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$Xler['id']."' AND badge_slot = '5'");

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


?>
<li class="<?php echo $even; ?>">
<p><img align="left" src="<?php echo $avatar; echo $Xler['look']; ?>&action=std,wav,&head_direction=3&gesture=sml"><br>
<a href="../home/<?php echo $Xler['username']; ?>"><b><?php echo $Xler['username']; ?></b></a> &nbsp;<span style="font-weight: normal; font-size: 80%;">(ID: <b><?php echo $Xler['id']; ?></b>)</span>
<b style="font-size: 110%;"><?php if($Xler['online'] >= 1){ ?><img src="../web-gallery/v2/images/online.gif"><?php }else{ ?><img src="../web-gallery/v2/images/offline.gif"><?php } ?></b><br />
<i><span style=": 0px 0px 7px blue"><span style="color: black" size:="" 2=""><?php echo $Xler['motto']; ?></span></i><br><br>
<span style=": 0px 0px 7px green"><span style="color: black" size:="" 2=""><b>&Uacute;ltima conexi&oacute;n: </b></span></span><i><span style=": 0px 0px 7px red"><span style="color: blue" size:="" 2=""><?php echo date('d-m-Y - H:i:s', $Xler['last_online']); ?></span></i><br>
<span style="font-weight: normal; font-size: 95%;"><img src="http://javvox.com/Premiun_Images/sc.gif" /> <?php echo $Xler['credits']; ?> <img src="http://javvox.com/Premiun_Images/sp.gif" /> <?php echo $Xler['activity_points']; ?></span><br />
<?php echo $badge; ?> <?php echo $badge2; ?> <?php echo $badge3; ?> <?php echo $badge4; ?> <?php echo $badge5; ?></p>
<?php } ?><br>
</table>
	

</div></div></div>

<div id="column2" class="column">

<div class="habblet-container">        
<div class="cbb clearfix blue">
<h2 class="title">Quais são os Ajudantes?</h2>


<div class="habblet box-content">

<img src="<?php echo $path; ?>/web-gallery/v2/images/xler/habbox_3.gif" align='left'><span style=": 0px 0px 7px orange"><span style="color: black" size:="" 2="">Os Ajudantes estão as pessoas que ajudam os outros. Tenho a resposta para suas perguntas, você pode encontrar no <b>Central de Ajuda</b> e identificados pelos seus 2 placas distintas.</span> <br>

</div>
</div>
</div>


<?php

$status = NULL;
if(isset($_POST['canjear'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhuma referência.</div>';
		}

		elseif($data['aantalleden'] >= '100') {

			$row = mysql_fetch_array(mysql_query("SELECT rank FROM users WHERE id = '".$my_id."' LIMIT 1"));
			if($row['rank'] == 1 || $row['rank'] == 2)
			{
				mysql_query("UPDATE users SET rank = '3' WHERE id = '".$my_id."' LIMIT 1");
			}
				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 100");
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Pronto, agora pertencem ao grupo dos Ajudantes de <?php echo $shortname; ?>. Obrigado por convidar seus amigos para o hotel!</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Desculpe, você não tem referências suficientes.</div>';
	}

	}

?>
<style>
	.error {
		padding: 7px;
		background-color: #fff4f2;
		border: 1px solid #a63c29;
		color: #E2001A;
		margin-top: 5px;
	}

	.error > h3 {
		font-weight: bold;
		margin: 0px;
		padding: 0px;
		font-size: 13px;
	}
	div.goodmsg {
		padding: 7px;
		background-color: #d8f3d8;
		border: 1px solid #4da04d;
		color: #205220;
		margin-top: 5px;
	}

	div.goodmsg > h3 {
		font-weight: bold;
		margin: 0px;
		padding: 0px;
		font-size: 13px;
	}
	div.display_none {
		display: none;
	}
	</style>

<div class="habblet-container ">		
		<div class="cbb clearfix white "> 

		<h2 class="title">Junte-se aos Ajudantes</h2>

		<div class="box-content">

			<div align="center">
			<img src="http://images.wikia.com/habboworldteam/es/images/9/9e/Frank_welcome1.gif" style="float: left;"><span style=": 0px 0px 7px black"><span style="color: gray" size:="" 2=""> Agora você pode resgatar o seus <b>100 Referidos</b> por Ajudante em <?php echo $shortname; ?>.</span></span> <br />
			  <br />
			  
			<?php echo $status; ?>
			<br />
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Ser Ajudante" name="canjear" />'; } ?></center>
			</form>
			  </div>
		</div>
			
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>


<?php require_once('./templates/community_footer.php'); ?>