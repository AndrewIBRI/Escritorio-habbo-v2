<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Loja VIP";
$pageid = "47";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>


<div id="container">
<div id="content" style="position: relative; left: 0px; top: 0px;" class="clearfix">

<div id="column1" class="column">
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


		<style type="text/css">
		<!--
		.Estilo1 {
			font-family: Verdana, Arial, Helvetica, sans-serif;
			font-size: 12px;
		}
		-->
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
		echo '<h2>Bem Feito.</h2>';
		echo 'Detectamos que estão fora do hotel, você pode continuar com a compra de VIP.</font>';
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


<?php
	// $timened = time() + 15552000;
	$timeend15 = time() + 648000 * 2;

    error_reporting(0);
    define('CMS', 'LAVVOCMS'); // Sólo se puede usar en Uber  && LAVVOCMS
    define('id', (CMS == 'uber' ? USER_ID : $my_id));
    define('moneda', 'dolares');
    define('nombre_moneda', 'Dólares');
    define('price15', '6');
    if(CMS == 'uber')
    $online = $users->GetUserVar(USER_ID, 'online');
    else
    $online = $myrow['online'];
    if(isset($_POST['comprar15'])):
            $moneda = (CMS == 'uber' ? $users->GetUserVar(USER_ID, moneda) : $myrow[moneda]);
            $error15 = array();
            if($myrow['online'] == 1)
                    $error[] = "Desculpe, mas você deve estar offline no hotel.";
            if($myrow['vip'] == '1')
                    $error15[] = "Desculpe, mas para adquirir uma assinatura mais VIP deve completar o anterior.";
            if($moneda < price15)
                    $error15[] = "Desculpe, não têm o suficiente ".nombre_moneda;;
            if(empty($error15)):
                    $sucess15 = "Você Comprou 15 Dias VIP Com Sucesso";
                    $cambio15 = $moneda-price15;
                    mysql_query("UPDATE users SET ".moneda." = '".$cambio15."' WHERE id = '".$my_id."'");
	            mysql_query("UPDATE users SET vip_points = vip_points + 70000, rank = '2', vip = '1' WHERE id = '".$my_id."'") or die(mysql_error());
		    mysql_query("INSERT INTO vip (id_user,timestamp,timestampend) VALUES ('".$my_id."','".time()."','".$timeend15."')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Parabéns<br><br>- Você tem 15 dias de VIP comprado com sucesso no <?php echo $shortname; ?>.')");
                    mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','VIP','0')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_transactionsvip (userid,amount,date,descr) VALUES ('".$my_id."',-'".price15."','".$date_full."','Compre 15 dias de VIP.')");
            endif;
            foreach($error15 as $errors)
                    $errorz15 = $errors;
    endif;
     
    ?>

    <div class="habblet-container ">
<div class="cbb clearfix orange "> 
<h2 class="title">Compra VIP</h2> 
    <div class="habblet box-content">

               <center><img src="./web-gallery/images/catalogue/headers/compravip.png"></center><br>

                <center>    <?php if(!empty($error15)): echo '<span style="color: red" size:="" 2=""><b>'.$errorz15.'</b></span>'; endif;
    if(!empty($sucess15)): echo '<span style="color: green" size:="" 2=""><b>'.$sucess15.'</b></span>';  endif; ?> </center>

<table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='15%' align='left'></td>
  <td class='tablesubheader' width='25%' align='left'></td> 
  <td class='tablesubheader' width='10%' align='left'></td>

 </tr>
<tr>
  <td class='tablerow1' align='left'><img src="<?php echo $path; ?>/Premiun_Images/vip15.png"></td>
  <td class='tablerow1' align='left'><center><b style="font-size:10px;"><img src="./Premiun_Images/dolares.gif">6 Dólares</b></center></td>
  <td class='tablerow1' align='left'><form method="post" action=""><input type="submit" name="comprar15" value="Comprar"/></form></td>

</td>
</tr>
</table>
                                                            <br><br>
<?php
	// $timened = time() + 15552000;
	$timeend1 = time() + 1296000 * 2;

    error_reporting(0);
    define('CMS', 'LAVVOCMS'); // Sólo se puede usar en Uber  && LAVVOCMS
    define('id', (CMS == 'uber' ? USER_ID : $my_id));
    define('moneda', 'dolares');
    define('nombre_moneda', 'Dólares');
    define('price1', '11');
    if(CMS == 'uber')
    $online = $users->GetUserVar(USER_ID, 'online');
    else
    $online = $myrow['online'];
    if(isset($_POST['comprar1'])):
            $moneda = (CMS == 'uber' ? $users->GetUserVar(USER_ID, moneda) : $myrow[moneda]);
            $error1 = array();
            if($myrow['online'] == 1)
                    $error[] = "Desculpe, mas você deve estar offline no hotel.";
            if($myrow['vip'] == '1')
                    $error1[] = "Desculpe, mas para adquirir uma assinatura mais VIP deve completar o anterior.";
            if($moneda < price1)
                    $error1[] = "Desculpe, não têm o suficiente ".nombre_moneda;;
            if(empty($error1)):
                    $sucess1 = "Você Comprou 1 Mês de VIP Com Sucesso";
                    $cambio1 = $moneda-price1;
                    mysql_query("UPDATE users SET ".moneda." = '".$cambio1."' WHERE id = '".id."'");
	            mysql_query("UPDATE users SET vip_points = vip_points + 70000, rank = '2', vip = '1' WHERE id = '".$my_id."'") or die(mysql_error());
		    mysql_query("INSERT INTO vip (id_user,timestamp,timestampend) VALUES ('".$my_id."','".time()."','".$timeend1."')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Parabéns<br><br>- Você tem 1 mês de VIP comprado com sucesso no <?php echo $shortname; ?>.')");
                    mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','VIP','0')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_transactionsvip (userid,amount,date,descr) VALUES ('".$my_id."',-'".price1."','".$date_full."','Comprar 1 mês de vip.')");
            endif;
            foreach($error1 as $errors)
                    $errorz1 = $errors;
    endif;
     
    ?>

                <center>    <?php if(!empty($error1)): echo '<span style="color: red" size:="" 2=""><b>'.$errorz1.'</b></span>'; endif;
    if(!empty($sucess1)): echo '<span style="color: green" size:="" 2=""><b>'.$sucess1.'</b></span>';  endif; ?> </center>

<table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='15%' align='left'></td>
  <td class='tablesubheader' width='25%' align='left'></td> 
  <td class='tablesubheader' width='10%' align='left'></td>

 </tr>
<tr>
  <td class='tablerow1' align='left'><img src="<?php echo $path; ?>/Premiun_Images/vip1.png"></td>
  <td class='tablerow1' align='left'><center><b style="font-size:10px;"><img src="./Premiun_Images/dolares.gif">11 Dólares</b></center></td>
  <td class='tablerow1' align='left'><form method="post" action=""><input type="submit" name="comprar1" value="Comprar"/></form></td>

</td>
</tr>
</table>
                                                            <br><br>




<?php
	// $timened = time() + 15552000;
	$timeend3 = time() + 3888000 * 2;

    error_reporting(0);
    define('CMS', 'LAVVOCMS'); // Sólo se puede usar en Uber  && LAVVOCMS
    define('id', (CMS == 'uber' ? USER_ID : $my_id));
    define('moneda', 'dolares');
    define('nombre_moneda', 'Dólares');
    define('price3', '26');
    if(CMS == 'uber')
    $online = $users->GetUserVar(USER_ID, 'online');
    else
    $online = $myrow['online'];
    if(isset($_POST['comprar3'])):
            $moneda = (CMS == 'uber' ? $users->GetUserVar(USER_ID, moneda) : $myrow[moneda]);
            $error3 = array();
            if($myrow['online'] == 1)
                    $error[] = "Desculpe, mas você deve estar offline no hotel.";
            if($myrow['vip'] == '1')
                    $error3[] = "Desculpe, mas para adquirir uma assinatura mais vip deve completar o anterior.";
            if($moneda < price3)
                    $error3[] = "Desculpe, não têm o suficiente ".nombre_moneda;;
            if(empty($error3)):
                    $sucess3 = "Você comprou 3 Meses Sucesso VIP";
                    $cambio3 = $moneda-price3;
                    mysql_query("UPDATE users SET ".moneda." = '".$cambio3."' WHERE id = '".id."'");
	            mysql_query("UPDATE users SET vip_points = vip_points + 70000, rank = '2', vip = '1' WHERE id = '".$my_id."'") or die(mysql_error());
		    mysql_query("INSERT INTO vip (id_user,timestamp,timestampend) VALUES ('".$my_id."','".time()."','".$timeend3."')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Parabéns<br><br>- Você tem 3 meses de VIP comprado com sucesso no <?php echo $shortname; ?>.')");
                    mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','VIP','0')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_transactionsvip (userid,amount,date,descr) VALUES ('".$my_id."',-'".price3."','".$date_full."','Compra de 3 meses de VIP.')");
            endif;
            foreach($error3 as $errors)
                    $errorz3 = $errors;
    endif;
     
    ?>

                  <center><?php if(!empty($error3)): echo '<span style="color: red" size:="" 2=""><b>'.$errorz3.'</b></span>'; endif;
    if(!empty($sucess3)): echo '<span style="color: green" size:="" 2=""><b>'.$sucess3.'</b></span>';  endif; ?></center>

  <table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='15%' align='left'></td>
  <td class='tablesubheader' width='25%' align='left'></td> 
  <td class='tablesubheader' width='10%' align='left'></td>

 </tr>
<tr>
  <td class='tablerow1' align='left'><img src="<?php echo $path; ?>/Premiun_Images/vip3.png"></td>
  <td class='tablerow1' align='left'><center><b style="font-size:10px;"><img src="./Premiun_Images/dolares.gif">26 Dólares</b></center></td>
  <td class='tablerow1' align='left'><form method="post" action=""><input type="submit" name="comprar3" value="Comprar"/></form></td>

</td>
</tr>
</table>
                                                            <br><br>


<?php
	// $timened = time() + 15552000;
	$timeend5 = time() + 6480000 * 2;

    error_reporting(0);
    define('CMS', 'LAVVOCMS'); // Sólo se puede usar en Uber  && LAVVOCMS
    define('id', (CMS == 'uber' ? USER_ID : $my_id));
    define('moneda', 'dolares');
    define('nombre_moneda', 'Dólares');
    define('price5', '46');
    if(CMS == 'uber')
    $online = $users->GetUserVar(USER_ID, 'online');
    else
    $online = $myrow['online'];
    if(isset($_POST['comprar5'])):
            $moneda = (CMS == 'uber' ? $users->GetUserVar(USER_ID, moneda) : $myrow[moneda]);
            $error5 = array();
            if($myrow['online'] == 1)
                    $error[] = "Desculpe, mas você deve estar offline no hotel.";
            if($myrow['vip'] == '1')
                    $error5[] = "Desculpe, mas para adquirir uma assinatura mais vip deve completar o anterior.";
            if($moneda < price5)
                    $error5[] = "Desculpe, não têm o suficiente ".nombre_moneda;;
            if(empty($error5)):
                    $sucess5 = "Você comprou 5 meses de VIP Sucesso";
                    $cambio5 = $moneda-price5;
                    mysql_query("UPDATE users SET ".moneda." = '".$cambio5."' WHERE id = '".id."'");
	            mysql_query("UPDATE users SET vip_points = vip_points + 70000, rank = '2', vip = '1' WHERE id = '".$my_id."'") or die(mysql_error());
		    mysql_query("INSERT INTO vip (id_user,timestamp,timestampend) VALUES ('".$my_id."','".time()."','".$timeend5."')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Parabéns<br><br>- Você tem 5 meses de VIP comprado com sucesso no <?php echo $shortname; ?>.')");
                    mysql_query("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('".$my_id."','VIP','0')") or die(mysql_error());
		    mysql_query("INSERT INTO cms_transactionsvip (userid,amount,date,descr) VALUES ('".$my_id."',-'".price5."','".$date_full."','Compre 5 meses VIP.')");
            endif;
            foreach($error5 as $errors)
                    $errorz5 = $errors;
    endif;
     
    ?>

                  <center><?php if(!empty($error5)): echo '<span style="color: red" size:="" 2=""><b>'.$errorz5.'</b></span>'; endif;
    if(!empty($sucess5)): echo '<span style="color: green" size:="" 2=""><b>'.$sucess5.'</b></span>';  endif; ?></center>

<table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='15%' align='left'></td>
  <td class='tablesubheader' width='25%' align='left'></td> 
  <td class='tablesubheader' width='10%' align='left'></td>

 </tr>
<tr>
  <td class='tablerow1' align='left'><img src="<?php echo $path; ?>/Premiun_Images/vip5.png"></td>
  <td class='tablerow1' align='left'><center><b style="font-size:10px;"><img src="./Premiun_Images/dolares.gif">46 Dólares</b></center></td>
  <td class='tablerow1' align='left'><form method="post" action=""><input type="submit" name="comprar5" value="Comprar"/></form></td>

</td>
</tr>
</table>
                                                            <br><br>

    </div></div>
                                           
                                            </div>
     


      </div>


<div id="column2" class="column">

<div class="habblet-container">        
<div class="cbb clearfix blue">
<h2 class="title">Teus Dólares</h2>

<div class="habblet box-content">
	
<center>Olá <?php echo $name; ?>, Temos um total de:</center>
<center><br><b style="font-size:15px;"><?php echo $myrow['dolares']; ?> Dólares.</b></br>

<center><br>Os dólares que você pode começar comprar em qualquer página da nossa loja.</br></center>


</div>
</div></div>

<div class="habblet-container">        
<div class="cbb clearfix red">
<h2 class="title">Comprar dólares</h2>

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
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">No tienes ningún referido.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");



	//dar creditos
	$sql = "UPDATE users SET credits  = credits  + '10000' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você tem seus 5 referências trocado por créditos 10000.</div>';
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

		<h2 class="title">Trocar Píxels</h2>

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
</div>

<script type="text/javascript">
HabboView.run();
</script>
<div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<?php require_once('./templates/community_footer.php'); ?>