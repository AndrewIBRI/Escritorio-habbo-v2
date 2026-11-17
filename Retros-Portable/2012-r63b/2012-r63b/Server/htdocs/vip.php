<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "VIP";
$pageid = "12";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content"> 
<div id="column1" class="column">

<div class="habblet-container">        
<div class="cbb clearfix blue">
<h2 class="title"><?php echo $shortname; ?> VIP's</h2>

<?php 

$i = 0;

$get_vip = mysql_query("SELECT * FROM vip ORDER BY rand() LIMIT 100"); 
while($row = mysql_fetch_assoc($get_vip)){ $userdata = mysql_fetch_assoc($userdata = mysql_query("SELECT * FROM users WHERE id = '".$row['id_user']."'"));

$i++;


        if(IsEven($i)){
            $even = "odd";
        } else {
            $even = "even";
        }


$badge = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_slot = '1'");
$badge2 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_slot = '2'");
$badge3 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_slot = '3'");
$badge4 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_slot = '4'");
$badge5 = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$userdata['id']."' AND badge_slot = '5'");

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

<p><img align="left" src="<?php echo $avatar; echo $userdata['look']; ?>&size=b&direction=3&head_direction=3&gesture=sml" width="64" height="110" /><br>
<span style=": 0px 0px 7px red"><span style="color: red" size:="" 2=""><b>Usuario:</b></span> <?php echo $userdata['username']; ?><br>
<span style=": 0px 0px 7px green"><span style="color: green" size:="" 2=""><b>Missão:</b></span> <?php echo $userdata['motto']; ?><br>
<span style=": 0px 0px 7px blue"><span style="color: blue" size:="" 2=""><b>Última vez conectado:</b></span> <?php echo date('d.m.Y - H:i:s', $userdata['last_online']); ?><br>
<span style=": 0px 0px 7px orange"><span style="color: orange" size:="" 2=""><b>O VIP está se esgotando em:</b></span> <?php echo getVIPDays($userdata['id']); ?> Dias<br>
<span style=": 0px 0px 7px black"><span style="color: black" size:="" 2=""><b>Pontos VIP:</b></span> <?php echo $userdata['vip_points']; ?><br />
<span style="font-weight: normal; font-size: 95%;"><img src="<?php echo $path; ?>/Premiun_Images/sc.gif" /> <?php echo $userdata['credits']; ?> <img src="<?php echo $path; ?>/Premiun_Images/sp.gif" /> <?php echo $userdata['activity_points']; ?> <img src="<?php echo $path; ?>/Premiun_Images/dolares.png" /><?php echo $userdata['dolares']; ?></span><br />
<?php echo $badge; ?> <?php echo $badge2; ?> <?php echo $badge3; ?> <?php echo $badge4; ?> <?php echo $badge5; ?></p>
<hr><?php } ?>
</div>
</div>
</div>

<div id="column2" class="column">


<div class="habblet-container ">        
<div class="cbb clearfix blue">
<h2 class="title"><?php echo $sitename; ?> - VIP</h2>
<div class="habblet box-content">
<img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=lg-3023-110.hr-831-61.ha-1012-110.hd-190-1.ca-3175-110.ch-3050-92-92&size=b&direction=2&head_direction=3&action=wlk&gesture=sml&size=s" align='left'><span style=": 0px 0px 0px #000000"><span style=" size:="" 2=""><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=ch-3014-92.hd-600-1.lg-3023-110.hr-3012-61&size=b&direction=4&head_direction=3&action=wlk&gesture=sml&size=s" align='right'><span style=" size:="" 2="">
<br><center>O VIP é um clube especial que tem mais vantagens. Para filiar-se você pode conseguir isto conseguindo <a href=/referidos />Referidos</a> .</center>
</div>
</div>
</div>

<?php

	// $timened = time() + 15552000;
	$timeend2 = time() + 1296000 * 2;

$status = NULL;
if(isset($_POST['canjear'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhuma referência.</div>';
		}

		elseif($data['aantalleden'] >= '60') {

			$row = mysql_fetch_array(mysql_query("SELECT rank FROM users WHERE id = '".$my_id."' LIMIT 1"));
			if($row['rank'] == 1 || $row['rank'] == 1)
			{
				mysql_query("UPDATE users SET vip_points = vip_points + 70000, rank = '2', vip = '1' WHERE id = '".$my_id."'") or die(mysql_error());
			        mysql_query("INSERT INTO vip (id_user,timestamp,timestampend) VALUES ('".$my_id."','".time()."','".$timeend2."')") or die(mysql_error());
			        mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Parabéns<br><br>- Agora eu pertenço ao Habbe VIP  <?php echo $shortname; ?> por 30 dias, aproveite o melhor do Hotel.')");;
			}
				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 60");
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Pronto, agora pertence ao Habbe VIP  <?php echo $shortname; ?>. Obrigado pelo convite de seu amigo!</div>';
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
		<div class="cbb clearfix blue "> 

		<h2 class="title">Tem seus 60 Referidos?</h2>

		<div class="box-content">

			<div align="center">
			<img src="http://images.wikia.com/habboworldteam/es/images/9/9e/Frank_welcome1.gif" style="float: left;"><span style=": 0px 0px 7px black"><span style=" size:="" 2=""> Você pode trocar o seu <b>60 Referidos</b> durante 1 mês de VIP  <?php echo $shortname; ?></span></span> <br />

			  <br />
			<?php echo $status; ?>
			<br />
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Trocar" name="canjear" />'; } ?></center>
			</form>
			  </div>
		</div>
			
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>
<div id="column2" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix blue">
<h2 class="title">Benefícios VIP</h2>
<div class="habblet box-content">
<center><img src="./Premiun_Images/anunciosvip.png"></center>
<br>
* Mais acessórios para sua casa.<br>
* Um emblema VIP exclusivo.<br>
<h3>Como faço para entrar para o Habbe VIP?</h3>
<p>Você pode conseguir isso em <a href="/referidos" />Referidos</a>.</p></font>
<br><br><br>
</div>
</div>
</div>
<div class="habblet-container ">        
<div class="cbb clearfix blue">
<h2 class="title">Comandos VIP</h2>
<div class="habblet box-content">
<p><b>Os benefícios abaixo :</b></p>
<h3>Benefícios VIP Club:</h3>
* Comando: <b>:push (x)</b> <a onclick="alert('Empurrar outro usuário.');return false">INFO</a><br>
* Comando: <b>:pull (x)</b> <a onclick="alert('Puxar o outro usuário.');return false">INFO</a><br>
* Comando: <b>:copy (x)</b> <a onclick="alert('Copiar o look de outros usuarios.');return false">INFO</a><br>
* Comando: <b>:follow (x)</b> <a onclick="alert('Seguir o usuário para a sala onde ele esta.');return false">INFO</a><br>
* Comando: <b>:moonwalk</b> <a onclick="alert('Andar para trás como Michael Jackson.');return false">INFO</a><br>
<img src="<?php echo $cimagesurl.$badgesurl; ?>VIP.gif" align="right">
</div></div></div></div>

<div id="column7" class="column">

<div class="habblet-container">        


 

</div></div>
</div></div>
                <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>


<?php require_once('./templates/community_footer.php'); ?>