<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');

$pagename = "Prêmios por referidos";
$pageid = "42";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
   <div id="content" style="position: relative; left: 0px; top: 0px; width: 770px;" class="clearfix">

  

<div class="habblet box-content">
<div class="habblet-container ">



<?php
$status = NULL;
if(isset($_POST['cambiosss'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;"><?php echo $name;  ?> Você não possui referidos suficiente para a compra!</div>';
		}

		elseif($data['aantalleden'] >= '20') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 30");



	//dar pixeles
	$sql = "UPDATE users SET vip = '1' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;"><?php echo $name;  ?>, Parabéns! Você se tornou VIP!</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;"><?php echo $name;  ?> Você não possui referidos suficiente para a compra!</div>';
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
		font-weight: Arial;
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
		font-weight: Arial;
		margin: 0px;
		padding: 0px;
		font-size: 13px;
	}
	div.display_none {
		display: none;
	}
	</style>


<div class="habblet-container ">
		<?php if($myrow['online'] == '1')
{  ?>		
		<div class="cbb clearfix red "> 
		<?php } ?>
		<?php if($myrow['online'] == '0')
{  ?>		
		<div class="cbb clearfix green "> 
		<?php } ?>

		<h2 class="title">Verificar Online/Offline</h2>

		<div class="box-content">

			<div align="center">
			<?php
			if(isset($my_id)){
			$sql = mysql_query("SELECT online FROM users WHERE id = '".$my_id."' LIMIT 1;");
		$online = mysql_fetch_array($sql);
		if($online['online'] == '1')
		{
		echo '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">';
		echo '<h2>ALERTA</h2>';
		echo 'O nosso sistema detectou que você está online. Por favor, logoff do hotel antes de comprar para evitar problemas com a compra';
		echo '</div>';
		}
		else
		{
		echo '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">';
		echo '<h2>Tudo Ok.</h2>';
		echo 'Detectamos que você está fora do hotel, você pode continuar com a compra.</font>';
		echo '</div>';
		} 
		}
		?>
		</div>
		</div>
		</div>
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		
		
		<style type="text/css">
		<!--
		.Estilo1 {
			font-family: Verdana, Arial, Helvetica, sans-serif;
			font-size: 12px;
		}
		-->
		</style>








</div></div>

<div class="habblet-container ">		
		 <div class="cbb clearfix blue ">

		<h2 class="title" >VIP</h2>

		<div class="box-content">

			<div align="center">

			<b>Trocar 30 Referidos por VIP no Habbie Hotel.</b> <br />
			 <br /> 
			<center><img src="http://www.HabbieHotel.com.br/swfs/c_images/album1584/VIP.gif"></center>
                         <br />
                         <br />
			<?php echo $status; ?>
                        
						</br>
			
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Trocar" name="cambiosss" />'; } ?></center>
			</form>
			
		</div>
			
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>
</div>

<?php
$status = NULL;
if(isset($_POST['cambiosss'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;"><?php echo $name;  ?> Você não possui referidos suficiente para a compra!</div>';
		}

		elseif($data['aantalleden'] >= '100') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 100");



	//dar pixeles
	$sql = "UPDATE users SET rank = '4' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Parabéns, agora você faz parte de nossa equipe! Bom Proveito!</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;"><?php echo $name;  ?> Você não possui referidos suficiente para a compra!</div>';
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
		font-weight: Arial;
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
		font-weight: Arial;
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

		<h2 class="title" ><?php echo $shortname; ?> X</h2>

		<div class="box-content">

			<div align="center">

			<b>Trocar 100 Referidos por <?php echo $shortname; ?> X no Habbie Hotel.</b> 
<br />
<center><img src="/web-gallery/v2/images/habbox_1.gif"></center>
                         <br />	

                         <br />
                         <br />
			<?php echo $status; ?>
                        
						</br>
			
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Trocar" name="cambiosss" />'; } ?></center>
			</form>
			
		</div>
			
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>

</div>
</div>
		
<?php
$status = NULL;
if(isset($_POST['cambiosss'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;"><php echo $name>Não possui referidos suficientes</div>';
		}

		elseif($data['aantalleden'] >= '120') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 120");



	//dar pixeles
	$sql = "UPDATE users SET rank = '6' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">¡Felicidades, ahora eres Vip!</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;"><?php echo $name;  ?> Você não possui referidos suficiente para a compra!</div>';
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
		font-weight: Arial;
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
		font-weight: Arial;
		margin: 0px;
		padding: 0px;
		font-size: 13px;
	}
	div.display_none {
		display: none;
	}
	</style>

</div>
</div>
</div>

				
<?php require_once('./templates/community_footer.php');

?>