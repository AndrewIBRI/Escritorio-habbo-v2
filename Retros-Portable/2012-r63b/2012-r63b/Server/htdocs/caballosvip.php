<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Loja de Cavalos";
$pageid = "44";

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
		echo 'Detectamos que estão fora do hotel, você pode continuar com a compra de cavalos.</font>';
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
    error_reporting(0);
    define('CMS', 'LAVVOCMS'); // Sólo se puede usar en Uber  && LAVVOCMS
    define('id', (CMS == 'uber' ? USER_ID : $my_id));
    define('moneda', 'dolares');
    define('nombre_moneda', 'Dólares');
    define('price', '4');
    if(CMS == 'uber')
    $online = $users->GetUserVar(USER_ID, 'online');
    else
    $online = $myrow['online'];
    if(isset($_POST['caballo'])):
            $moneda = (CMS == 'uber' ? $users->GetUserVar(USER_ID, moneda) : $myrow[moneda]);
            $error = array();
            if($myrow['online'] == 1)
                    $error[] = "Desculpe, mas você deve estar offline no hotel.";
            if($moneda < price)
                    $error[] = "Desculpe, não têm o suficiente ".nombre_moneda;
            if(empty($_POST['caballo_name']))
                    $error[] = "Desculpe, você precisa nomear o seu cavalo";
            if(empty($error)):
                    $sucess = "Cavalo adquirido com sucesso !";
                    $cambio = $moneda-price;
                    mysql_query("UPDATE users SET ".moneda." = '".$cambio."' WHERE id = '".id."'");
                    mysql_query("INSERT INTO `user_pets` (`user_id`, `room_id`, `name`, `race`, `color`, `type`, `expirience`, `energy`, `nutrition`, `respect`, `createstamp`, `x`, `y`, `z`) VALUES ('".id."', '0', '".mysql_real_escape_string($_POST['caballo_name'])."', '".mysql_real_escape_string($_POST['caballo_race'])."', 'FFFFFF', '13', '20', '100', '100', '0', '".time()."', '0', '0', '0')");
		    mysql_query("INSERT INTO cms_transactionsvip (userid,amount,date,descr) VALUES ('".$my_id."',-'".price."','".$date_full."','Comprando um cavalo.')");
            endif;
            foreach($error as $errors)
                    $errorz = $errors;
    endif;
     
    ?>

    <div class="habblet-container ">
<div class="cbb clearfix orange "> 
<h2 class="title">Compra Cavalos</h2> 
    <div class="habblet box-content">

                                                     <center><img src="./web-gallery/images/catalogue/headers/caballosvip.png"></center><br>

                      <center><p><?php if(!empty($error)): echo '<span style="color: red" size:="" 2=""><b>'.$errorz.'</b></span>'; endif;
    if(!empty($sucess)): echo '<span style="color: green" size:="" 2=""><b>'.$sucess.'</b></span>';  endif; ?></p></center>
     
                                                    	

                                                           Comecei a procurar cavalos! Agora você pode comprar um muito original e difente aos encontrados no catálogo do hotel
                                                           Coloque o nome e escolhe uma raça. O cavalo custa a <b><?php echo number_format(price) . ' '.nombre_moneda ; ?></b>.</div>

<script type="text/javascript">								function previewTS(el)								{									document.getElementById('ts-preview').innerHTML = '<div style="margin-top:-10px; float: right; text-align: center; vertical-align: middle;"><b>Vista Previa:</b><br><br><img src="/images/horse/' + el + '.png" /></div>';								}							</script>							<div id="ts-preview" style="margin-left:10px; padding: 10px; float: left; text-align: center; vertical-align: middle;">								<div style="margin-top:-10px; float: right; text-align: center; vertical-align: middle;">								<b>Vista Previa:</b><br><br>								<img src="./images/horse/2.png" />								</div>									</div>
</p></p>

                							<form method="post"><br>
								<strong style="margin-left:0px;">Nome:</strong>
								<input type="text" name="caballo.name"/><br>
								<strong style="margin-left:0px;">Raça:  </strong>
								<select name="caballo.race" onchange="previewTS(this.value);">	

								<option value="2">Normal</option>
								<option value="3">Normal, Pontos</option>
								<option value="4">Normal, Manchas</option>
                                                                <option value="5">Normal, Manchas e Pontos</option>
								<option value="6">Marrom Claro</option>
								<option value="7">Marrom Claro, Pontos</option>
								<option value="8">Marrom Claro, Manchas</option>
								<option value="9">Marrom Claro, Manchas e Pontos</option>
								<option value="10">Marrom</option>
								<option value="11">Marrom, Pontos</option>
								<option value="12">Marrom, Manchas</option>
								<option value="13">Marrom, Manchas e Pontos</option>
								<option value="14">Cinza Palido</option>
								<option value="15">Cinza Palido, Pontos</option>
								<option value="16">Cinza Palido, Manchas</option>
								<option value="17">Cinza Palido, Manchas e Pontos</option>
								<option value="18">Naránja</option>
								<option value="19">Laranja, Pontos</option>
								<option value="20">Laranja, Manchas</option>
								<option value="21">Laranja, Manchas e Pontos</option>
								<option value="22">Rosa</option>
								<option value="23">Rosa, Pontos</option>
								<option value="24">Rosa, Manchas</option>
								<option value="25">Rosa, Manchas e Pontos</option>
								<option value="26">Azul Claro</option>
								<option value="27">Azul Claro, Pontos</option>
								<option value="28">Azul Claro, Manchas</option>
								<option value="29">Azul Claro, Manchas e Pontos</option>
								<option value="30">Branco Palido</option>
								<option value="31">Branco Palido, Pontos</option>
								<option value="32">Branco Palido, Manchas</option>
								<option value="33">Branco Palido, Manchas e Pontos</option>
								<option value="34">Preto</option>
								<option value="35">Preto, Pontos</option>
								<option value="36">Preto, Manchas</option>
								<option value="37">Preto, Manchas e Pontos</option>
								<option value="38">Branco</option>
								<option value="39">Branco, Pontos</option>
								<option value="40">Branco, Manchas</option>
								<option value="41">Branco, Manchas e Pontos</option>
								<option value="42">Verde</option>
								<option value="43">Verde, Pontos</option>
								<option value="44">Verde, Manchas</option>
								<option value="45">Verde, Manchas e Pontos</option>
								<option value="46">Laranja com Cafe</option>
								<option value="47">Laranja com Cafe, Pontos</option>
								<option value="48">Laranja com Cafe, Manchas</option>
								<option value="49">Laranja com Cafe, Manchas e Pontos</option>
								<option value="50">Preto, Manchas Laranja</option>
								<option value="51">Preto, Manchas Cafés</option>
								<option value="52">Preto, Manchas Cinza</option>
								<option value="53">Preto, Manchas Amarelo</option>
								<option value="54">Preto, Manchas Rosa</option>
								<option value="55">Preto, Manchas Eelestial</option>
								<option value="56">Preto, Manchas Amarelo Escuro</option>
								<option value="57">Preto, Manchas Azul</option>
								<option value="58">Preto, Manchas Pálidas</option>
								<option value="59">Preto, Manchas Laranja Escuro</option>
								<option value="60">Preto, Manchas Verdes</option>
								<option value="61">Azul</option>
								<option value="62">Azul, Pontos</option>
								<option value="63">Azul, Manchas</option>
								<option value="64">Azul, Manchas e Pontos</option>
								<option value="65">Rosa Claro</option>
								<option value="66">Rosa Claro, Pontos</option>
								<option value="67">Rosa Claro, Manchas</option>
								<option value="68">Rosa Claro, Manchas e Pontos</option>
								<option value="69">Amarelo</option>
								<option value="70">Amarelo, Pontos</option>
								<option value="71">Amarelo, Manchas</option>
								<option value="72">Amarelo, Manchas e Pontos</option>
								<option value="73">Vermelho</option>
								<option value="74">Vermelho, Pontos</option>
								<option value="75">Vermelho, Manchas</option>
								<option value="76">Vermelho, Manchas e Pontos</option>
								<option value="77">Variado [Unica]</option>


								</select>
                                                                    <input type="submit" name="caballo" value="Comprar"/>
                                                            </form>
                                                            <br><br>

    </div>
                                           
                                            </div>
     


      </div>


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
						$error = "<center>Tranferencia realizada con éxito.";
						}else{$error = "<center>Nem dólares suficientes";};						
					
					
						
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
<h2 class="title">Usuários com mais dólares</h2>


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
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Troca" name="canjear" />'; } ?></center>
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

		<h2 class="title">Trocar Pixels</h2>

		<div class="box-content">

			<div align="center">
<div style=" : #808080 1px 1px 0px;padding-top:5px;color:#000000;font-weight:bold;font-size:10.4px;float:center;margin-left: 5px;"> 
			Você Pode Trocar 5 Referências por 60000 pixels em  <?php echo $shortname; ?>. <br />
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