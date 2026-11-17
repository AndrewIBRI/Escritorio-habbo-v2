<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Loja de Dólares";
$pageid = "40";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content"> 
<div id="column1" class="column">
<div class="habblet-container ">        


<div class="cbb clearfix orange ">
<h2 class="title">Loja</h2>
<div class="habblet box-content">
<center>
<br><img src="./web-gallery/images/catalogue/headers/tiendavip.png">
</center>

<br>
Bem vindo <?php echo $name; ?>, esta loja foi criada para que você pode comprar <strong>Mega Raros</strong>, <strong>Emblemas Exclusivos</strong>, <strong>Respeitos e Petting</strong>, <strong>Efeitos</strong>, <strong>Cavalos Exclusivos</strong>, <strong>Bots</strong> y <strong>Vip</strong>, para se parecer com uma celebridade em <?php echo $shortname; ?>.<br> 
<br>
Nossa loja trabalha com um sistema de moeda única chamada <strong>Dólares</strong>, esses você pode comprar em qualquer página de nossa loja a um preço acessível.<br>
<br>
A melhor coisa sobre isso é que através da compra de dólares, os resiviras automaticamente à sua conta, enquanto você paga corretamente.<br>
<br>
<span style="color: red" size:="" 2="">NOTA:</span> Se você tiver qualquer problema ou dúvida sobre este sistema ou a loja do dólar, envie um email para <u></u> Obrigado e divirta-se.<br>
<br>

</center>
</div></div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script> 		     		
</div>

<div class="habblet-container ">		
<div class="cbb clearfix brown ">
<h2 class="title">Histórico de compras</h2>
<div id="tx-log">


<div id="Layer1" style="width:444px; height:430px; overflow: scroll;">
<table class="tx-history">
<thead>
<tr>
 <th class="tx-date">Data</th>
 <th class="tx-amount">Dólares</th>
 <th class="tx-description">Descrição</th>
</tr>
</thead>

<tbody>

<?php
$tick = 0;
$get_em = mysql_query("SELECT date,amount,descr FROM cms_transactionsvip WHERE userid = '".$my_id."' ORDER BY id DESC LIMIT 100") or die(mysql_error());
while($row = mysql_fetch_assoc($get_em)){
$tick++;

if(IsEven($tick)){ $even = "even"; } else { $even = "odd"; }

?>

<tr class="<?php echo $even; ?>">
 <td class="tx-date"><?php echo $row['date']; ?></td>
 <td class="tx-amount"><?php echo $row['amount']; ?></td>
 <td class="tx-description"><?php echo $row['descr']; ?></td>
</tr>

<?php } ?>

</tbody>
</table>
	
</div> 
</div> 
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script> 		     		
</div></div></div>




<div id="column2" class="column">

<div class="habblet-container">        
<div class="cbb clearfix blue">
<h2 class="title">Teus Dólares</h2>

<div class="habblet box-content">
	
<center>Olá <?php echo $name; ?>, Você tem um total de:</center>
<center><br><b style="font-size:15px;"><?php echo $myrow['dolares']; ?> Dólares.</b></br>

<center><br>Os dólares que você pode começar comprar em qualquer página da nossa loja.</br></center>


</div>
</div></div>

<div class="habblet-container">        
<div class="cbb clearfix red">
<h2 class="title">Compra Dólares</h2>

<div class="habblet box-content">

<br>
<center>
<b>Codificar o seu método de venda de moedas</b>
</center>
</br>


</div></div>



<div class="habblet-container ">
<div class="cbb clearfix white ">
<h2 class="title">Transferir Dólares</h2>
<div class="habblet box-content">

<?php
if(isset($_POST['tousgive']) && '.$name.' != $_POST['tousgive']){
$getCaracolas = mysql_query("SELECT * FROM users WHERE username = '".$name."'");
					$b = mysql_fetch_assoc($getCaracolas);
					$Caracolas1 = $b['dolares'];
					$user2 = addslashes(trim($_POST['tousgive']));
					$getCaracolas2 = mysql_query("SELECT * FROM users WHERE username = '".$user2."'");
					$b2 = mysql_fetch_assoc($getCaracolas2);
					$Caracolas2 = $b2['dolares'];

					$amount = preg_replace("/[^0-9]/", "", addslashes(trim(abs($_POST['amount']))));
					if($Caracolas1 && $Caracolas2 >= "0" && isset($amount)){
								
					if($Caracolas1 >= $amount){
						$final = $Caracolas1-$amount;
						$finalnew = $Caracolas2+$amount;
					        mysql_query("UPDATE users SET dolares='".$finalnew."' WHERE username = '".$user2."'");
						mysql_query("UPDATE users SET dolares='".$final."' WHERE username = '".$name."'");
						$error = "<center>Transferência realizada com sucesso.";
						}else{$error = "<center>Não tem dólares suficientes";};						
					
					
						
					}else{$error = "<center>Ocorreu um erro, certifique-se de ter preenchido corretamente os dois campos.";};
echo $error;

	}else{
		echo 'Olá '.$name.', Com esta ferramenta você pode transferir dólares para outros usuários.<br><br><form action="" method="post" name="tranfer">
<strong style="margin-left:10px;">Usuario: <input name="tousgive" type="text"><br></strong>
<strong style="margin-left:10px;">Quantidade: <input name="amount" type="text"><input name="me" type="hidden" value="'.$name.'" /></strong>
<center><input name="enviar" value="Enviar" type="submit"></center></br>
</form>';};
					
?></div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>



<div class="habblet-container">        
<div class="cbb clearfix green">
<h2 class="title">Usuarios com mais Dólares</h2>


<div class="habblet box-content"> 
<table width="100%">
<tr>
	
<?php

$row = mysql_query("SELECT * FROM users WHERE rank < 5 ORDER BY dolares DESC LIMIT 5");
while($sql = mysql_fetch_assoc($row)){

?>

<tr><td width="5px"></td>
<td width="20px">
<img src="<?php echo $avatar; echo $sql['look']; ?>&size=s&direction=2&head_direction=2&gesture=sml&size=m" align="left"></td>
<td width="195px"><a href="<?php echo $path; ?>/home/<?php echo $sql['username']; ?>"><b><?php echo $sql['username']; ?></b></a><br />
<span style=" : 0px 0px 7px black"><span style="color: black" size:="" 2=""><?php echo $sql['dolares']; ?> Dólares</span></td>
	  <?php } ?>
</tr>
</table>
</div> </div> </div> </div>
</div>

<div id="column3" class="column">


<?php
$status = NULL;
if(isset($_POST['canjear'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhuma referência.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");



	//dar creditos
	$sql = "UPDATE users SET dolares = dolares  + '1' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você já resgatou suas 5 referências para 1 dólar.</div>';
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
		<div class="cbb clearfix red "> 

		<h2 class="title">Trocar Dólares</h2>

		<div class="box-content">

			<div align="center">
<div style=" : #808080 1px 1px 0px;padding-top:5px;color:#000000;font-weight:bold;font-size:10.4px;float:center;margin-left: 5px;"> 
			Você pode Trocar 5 Referências por 1 dólar em <?php echo $shortname; ?>.<br />
			 <br /> 
			<center><img src="Premiun_Images/dolares.gif"></center>
                         <br />
                         <br />
			<?php echo $status; ?>
                        
			
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Trocar" name="canjear" />'; } ?></center>
			</form>
			  </div>
                      </div>
			
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>
</div>

<?php
$status = NULL;
if(isset($_POST['cambear'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhuma referência.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");



	//dar creditos
	$sql = "UPDATE users SET credits  = credits  + '10000' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você tem seus 5 referências trocado por 10000 créditos.</div>';
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
		<div class="cbb clearfix green "> 

		<h2 class="title">Trocar Créditos</h2>

		<div class="box-content">

			<div align="center">
<div style=" : #808080 1px 1px 0px;padding-top:5px;color:#000000;font-weight:bold;font-size:10.4px;float:center;margin-left: 5px;"> 
			 Você pode Trocar 5 Referências por 10000 Creditos em <?php echo $shortname; ?>. <br />
			  <br /> 
			<center><img src="Premiun_Images/creditos.gif"></center>
                         <br />
                         <br />
			<?php echo $status; ?>
                        
			
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Trocar" name="cambear" />'; } ?></center>
			</form>
			  </div>
		</div>
			
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>
</div>

<?php
$status = NULL;
if(isset($_POST['cambioss'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhuma referência.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");



	//dar pixeles
	$sql = "UPDATE users SET activity_points  = activity_points  + '60000' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você tem seus 5 referências trocadas por 60000 pixels.</div>';
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
		<div class="cbb clearfix orange "> 

		<h2 class="title">Trocar Píxeles</h2>

		<div class="box-content">

			<div align="center">
<div style=" : #808080 1px 1px 0px;padding-top:5px;color:#000000;font-weight:bold;font-size:10.4px;float:center;margin-left: 5px;"> 
			Você Pode Trocar 5 Referências por 60000 pixels em <?php echo $shortname; ?>. <br />
			 <br /> 
			<center><img src="Premiun_Images/pixeles.gif"></center>
                         <br />
                         <br />
			<?php echo $status; ?>
                        
			
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Trocar" name="cambioss" />'; } ?></center>
			</form>
			  </div>
		</div>
			
		</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>
</div>
</div>


<script type="text/javascript">
HabboView.run();
</script>
<div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<?php require_once('./templates/community_footer.php'); ?>