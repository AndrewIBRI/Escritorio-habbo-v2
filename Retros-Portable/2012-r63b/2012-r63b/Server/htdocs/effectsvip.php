<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Loja de Efeitos";
$pageid = "45";

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
		echo 'Detectamos que você está fora do hotel, você pode continuar com a compra de suprimentoss.</font>';
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
    define('price', '1');
    if(CMS == 'uber')
    $online = $users->GetUserVar(USER_ID, 'online');
    else
    $online = $myrow['online'];
    if(isset($_POST['effect'])):
            $moneda = (CMS == 'uber' ? $users->GetUserVar(USER_ID, moneda) : $myrow[moneda]);
            $error = array();
            if($myrow['online'] == 1)
                    $error[] = "Desculpe, mas você deve estar offline no hotel.";
            if($moneda < price)
                    $error[] = "Desculpe, não têm o suficiente ".nombre_moneda;
            if(empty($error)):
                    $sucess = "Efeito adquirido com sucesso!";
                    $cambio = $moneda-price;
                    mysql_query("UPDATE users SET ".moneda." = '".$cambio."' WHERE id = '".id."'");
                    mysql_query("INSERT INTO `user_effects` (`user_id`, `effect_id`, `total_duration`, `is_activated`, `activated_stamp`) VALUES ('".id."', '".mysql_real_escape_string($_POST['effect_num'])."', '100000000', '0', '0')");
		    mysql_query("INSERT INTO cms_transactionsvip (userid,amount,date,descr) VALUES ('".$my_id."',-'".price."','".$date_full."','Compra um efeito.')");
            endif;
            foreach($error as $errors)
                    $errorz = $errors;
    endif;
     
    ?>

    <div class="habblet-container ">
<div class="cbb clearfix orange "> 
<h2 class="title">Compra Efeitos</h2> 
    <div class="habblet box-content">

                                                     <center><img src="./web-gallery/images/catalogue/headers/efectosvip.png"></center><br>

                      <center><p><?php if(!empty($error)): echo '<span style="color: red" size:="" 2=""><b>'.$errorz.'</b></span>'; endif;
    if(!empty($sucess)): echo '<span style="color: green" size:="" 2=""><b>'.$sucess.'</b></span>';  endif; ?></p></center>
     
                                                    	

                                                            Efeitos Compre e escolhe seu favorito e comprar por apenas <b><?php echo number_format(price) ?> Dólar</b>. 
                                                            A diferença em relação aos encontrados no catálogo é que estes são tempo ilimitado.</div>

<script type="text/javascript">								function previewTS(el)								{									document.getElementById('ts-preview').innerHTML = '<div style="margin-top:-10px; float: right; text-align: center; vertical-align: middle;"><b>Vista Previa:</b><br><br><img src="/images/effects/' + el + '.png" /></div>';								}							</script>							<div id="ts-preview" style="margin-left:10px; padding: 10px; float: left; text-align: center; vertical-align: middle;">								<div style="margin-top:-10px; float: right; text-align: center; vertical-align: middle;">								<b>Vista Previa:</b><br><br>								<img src="./images/effects/1.png" />								</div>									</div>
</p></p>

                							<form method="post"><br>
								<strong style="margin-left:0px;">Efectos:  </strong>
								<select name="effect.num" onchange="previewTS(this.value);">	

								<option value="1">Luz de Foco</option>
								<option value="2">Holopatín Azul</option>
								<option value="3">Ovni Azul</option>
                                                                <option value="4">Brillo</option>
								<option value="5">Antorcha</option>
								<option value="6">Jetpack</option>
								<option value="7">Mariposas</option>
								<option value="8">Libélulas</option>
								<option value="9">Corazones</option>
								<option value="10">Moscas</option>
								<option value="11">Habbo Verde</option>
								<option value="12">Congelado</option>
								<option value="13">Transparente</option>
								<option value="14">Holopatín Rosa</option>
								<option value="15">Holopatín Amarillo</option>
								<option value="16">Micrófono</option>
								<option value="17">Ovni Rosa</option>
								<option value="18">Ovni Amarillo</option>
								<option value="19">Policía</option>
								<option value="20">Ambulancia</option>
								<option value="21">Coche Azul</option>
								<option value="22">Coche Negro</option>
								<option value="23">Radioactivo</option>
								<option value="24">Lluvia</option>
								<option value="25">Fuego</option>
								<option value="26">Baston con Bola de Fuego</option>
								<option value="27">Vikingo</option>
								<option value="28">En el Agua</option>
								<option value="30">Agua por los Pies</option>
								<option value="31">Cheetos</option>
								<option value="32">Ojo Grú</option>
								<option value="33">Botador BB Rojo</option>
								<option value="34">Botador BB Verde</option>
								<option value="35">Botador BB Azul</option>
								<option value="36">Botador BB Amarillo</option>
								<option value="37">Agua Turbia</option>
								<option value="38">Patines Marrones</option>
								<option value="39">Patines Blancos</option>
								<option value="40">Casco Freeze Rojo</option>
								<option value="41">Casco Freeze Verde</option>
								<option value="42">Casco Freeze Azul</option>
								<option value="43">Casco Freeze Amarillo</option>
								<option value="44">Rombo Sims</option>
								<option value="45">Brillo con Patines Marrones</option>
								<option value="46">Brillo con Patines Blancos</option>
								<option value="47">Cabeza Gigante - Azul</option>
								<option value="48">Coche de Perro</option>
								<option value="49">Casco Rojo con Brillo</option>
								<option value="50">Casco Verde con Brillo</option>
								<option value="51">Casco Azul con Brillo</option>
								<option value="52">Casco Amarillo con Brillo</option>
								<option value="53">Pollitos Volando</option>
								<option value="54">Coche de Conejo</option>
								<option value="55">Patines Azules</option>
								<option value="56">Patines Rosa</option>
								<option value="57">Patines Azules con Brillo</option>
								<option value="58">Patines Roda con Brillo</option>
								<option value="59">Luz Resplandeciente</option>
								<option value="60">Cabeza de Lagartija</option>
							        <option value="61">Estrellas Amarillas</option>
								<option value="62">Estrellas Moradas</option>
								<option value="63">Estrellas Rosa</option>
								<option value="64">Bela</option>
								<option value="65">Teléfono Celular</option>
								<option value="66">Vara de la suerte</option>
								<option value="67">Pájaro</option>
								<option value="68">Conejo</option>
								<option value="69">Carro Rojo</option>
								<option value="70">Panda</option>
							        <option value="71">Patineta</option>
								<option value="73">Chupa Chups</option>
								<option value="74">Flores Rosas</option>
								<option value="75">Flores Rojas</option>
								<option value="76">Flores Moradas</option>
								<option value="77">Montura de Caballo</option>
								<option value="78">Perry</option>
								<option value="79">Extra-Terrestre</option>
								<option value="80">Mandíbula</option>
								<option value="81">Mascara Alien</option>
								<option value="82">Mascara Baleada</option>
								<option value="83">Mascara Mujer</option>
								<option value="84">Máscara Momia</option>
								<option value="85">Calabaza</option>
								<option value="86">Máscara Hombre del Saco</option>
								<option value="87">Máscara Hombre Lobo</option>
								<option value="88">Máscara Zombie</option>
								<option value="89">Cuchillo Sangriento</option>
								<option value="90">Alas Violetas</option>
								<option value="91">Alas Transparentes</option>
								<option value="92">Capa de Fantasma</option>
								<option value="93">Sin Cabeza</option>
								<option value="94">Camión</option>
								<option value="95">Flecha Roja</option>
								<option value="96">Flecha Azul</option>
								<option value="97">Tabla de SnowBoard</option>
								<option value="98">Francotirador</option>
                                                                <option value="99">Niko</option>
                                                                <option value="100">Dragon Chino</option>


								</select>
                                                                    <input type="submit" name="effect" value="Comprar"/>
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
	
<center>Olá <?php echo $name; ?>, Possui um total de:</center>
<center><br><b style="font-size:15px;"><?php echo $myrow['dolares']; ?> Dólares.</b></br>

<center><br>Dólares você pode começar comprar em qualquer página de nossa loja.</br></center>


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
						$error = "<center>Transferir feita com Sucesso.";
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
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhum referido.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");



	//dar creditos
	$sql = "UPDATE users SET dolares = dolares  + '1' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você já resgatou suas 5 referidos para 1 dólar.</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Desculpe, você não tem referidos suficientes.</div>';
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
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhum referido.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");



	//dar creditos
	$sql = "UPDATE users SET credits  = credits  + '10000' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você tem seus 5 referidos trocado por 10000 Dólare.</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Desculpe, você não tem referidos suficientes.</div>';
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
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhum referido.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");



	//dar pixeles
	$sql = "UPDATE users SET activity_points  = activity_points  + '60000' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você tem seus 5 referidos trocado por 60000 Píxeles.</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Desculpe, você não tem referidos suficientes</div>';
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