<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Bots";
$pageid = "70";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>
<div id="container">
   <div id="content" style="position: relative; left: 0px; top: 0px;" class="clearfix">
<?php 
$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");
$data = mysql_fetch_array($query);

if ($data['rank'] == 1){ 
?>
<script>
alert('Você deve ser Premium para comprar ou EDITAR BOTS.');
</script>
<?php 
} ?>

<?php
$error = "";
$precio = "9";

$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");

$saldo = 0;
while ($data = mysql_fetch_array($query)){

$saldo = $data['dolares'];
}

if(isset($_POST['buy-bot']))
{
	$user_info = @mysql_query("SELECT * FROM users WHERE username='". $myrow['username'] ."'");
	if($row = mysql_fetch_assoc($user_info))
	{
		$id = $row['id'];
		$owner = $row['id'];
		$pixeles = $row['activity_points'];
		$creditos2 = $row['dolares'];
		$tranferencia = 'Compra de um BOT';
		$nombre = $_POST['nombre'];
$mision = $_POST['mision'];
$ropa = $_POST['look'];
$posicionx = $_POST['posicionx'];
$posiciony = $_POST['posiciony'];
$sala = $_POST['sala'];
$estado = $_POST['estado'];
$rotacion = $_POST['rotacion'];
		
	
		$check_bot = mysql_query("SELECT * FROM bots WHERE owner='". $id ."'");
		$check_user = mysql_query("SELECT * FROM users WHERE id='". $id ."'");
		$check_stats = mysql_query("SELECT * FROM user_stats WHERE id='". $id ."'");

		if(mysql_num_rows($check_bot) > 20)
		{
			$error.= "Por agora só permite 20 Usuário BOT!<br /><br />";
		}
		else
		{
			if($mostrar = mysql_fetch_assoc($check_stats))
			{
				$respetos = $mostrar['Respect'];
				if($creditos2 >= $precio)

				{
					$creditos_totales = $creditos2 - $precio;
					
				
					mysql_query("UPDATE users SET dolares = '".$creditos_totales."' WHERE id = '".$id."'");
					mysql_query("INSERT INTO `bots`(`room_id`, `name`, `motto`, `look`, `x`, `y`, `rotation`, `walk_mode`, `owner`) VALUES ('$sala', '$nombre', '$mision', '$ropa', '$posicionx', '$posiciony', '$rotacion', '$estado', '$owner')");

									
					$error.= "<font color='#009900'>Parabéns! Você acaba de receber um BOT Agora tem " . $creditos_totales . " Dolares<br> PARA VER VOCÊ ESCREVER BOT :update_bots  ENTÃO :unload PARA RECARREGAR O QUARTO.<br /></font>";
				}
				
				else
				{
					$error.= "<font color='#FF0000'>Desculpe, não tem Dólares suficiente<br /><br /></font>";
				}
			}
		}
	}
}

?>
   <div class="habblet-container ">
<div class="cbb clearfix orange ">
<div class="rounded-container"><div style="background-color: rgb(255, 255, 255); "><div style="margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(249, 150, 85); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(247, 108, 16); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(247, 108, 16); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(249, 150, 85); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(247, 108, 16); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div></div><h2 class="title rounded-done"><span style="float: left;">Criar um BOT por <?php echo $precio;?> Dólares</span> <span style="float: right; font-weight: normal; font-size: 75%;">Ou quem são notificados.</span></h2><div style="background-color: rgb(255, 255, 255); "><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(247, 108, 16); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(249, 150, 85); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(247, 108, 16); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div></div><div style="margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(249, 150, 85); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(247, 108, 16); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(246, 98, 0); "></div></div></div></div></div></div>
<div class="habblet box-content"><span style="float: left;"><div style=" : #808080 1px 1px 0px;padding-top:5px;padding-left:5px;color:#000;font-weight:bold;font-size:11px;float:center;">
<?php echo $error; ?>
<hr style="color:#9596AB">
<b>Criar um BOT</b>

<form method="post">
<b>Nome: </b><input name="nombre" type="text" id="habbo" value="<?php echo $myrow['username']; ?>" /> Digite um nome para o BOT<br>
<b>Missão: </b><input name="mision" type="text" id="habbo" value="BOT de <?php echo $myrow['username']; ?>" /> Escreva uma Missão para o BOT<br>
<b>Sala: </b><select name="sala">
<?php $query = mysql_query("SELECT * FROM rooms WHERE owner ='".$myrow['username']."'");

while ($data = mysql_fetch_array($query)){ ?>
<option value="<?php echo $data['id'] ?>"><?php echo $data['caption'] ?></option>
<?php } ?>
</select> Selecione a sala onde você quer colocar um BOT.<br>
Modo Pedestre: <select name="estado">
<option value="stand">Quieto</option>
<option value="freeroam">Livremente</option></select> Selecione se você pode mover site BOT.<br>
<hr>
Escolha um Look para o seu BOT
<?php
$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'") or die(mysql_error()); 
while ($data2 = mysql_fetch_array($query)){
$look=$data2['look'];
$id = $data2['id']; }
?><br>
<input type="radio" name="look" value="<?php echo $look; ?>" checked><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=<?php echo $look; ?>&amp;direction=2&amp;head_direction=2&amp;size=s"></img> |
<input type="radio" name="look" value="ch-3049-1336.hd-600-8.lg-3088-1341-1340.sh-3180-110-1336.hr-678-37"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=ch-3049-1336.hd-600-8.lg-3088-1341-1340.sh-3180-110-1336.hr-678-37&amp;direction=2&amp;head_direction=2&amp;size=s"></img> |
<input type="radio" name="look" value="hr-828-45.lg-285-64.ch-215-110.hd-180-2.sh-290-62.he-1607-"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-45.lg-285-64.ch-215-110.hd-180-2.sh-290-62.he-1607-&amp;direction=2&amp;head_direction=2&amp;size=s"></img> |
<input type="radio" name="look" value="hd-180-7.sh-290-110.lg-270-91.ch-809-62.hr-828-45"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hd-180-7.sh-290-110.lg-270-91.ch-809-62.hr-828-45&amp;direction=2&amp;head_direction=2&amp;size=s"></img>Recuerda Que El primer Look es el de Tu Habbo.<br>
<hr>
Rotação e Posição<br>
<input type="radio" name="rotacion" value="0" checked><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=0&head_direction=0&size=s"></img> |
<input type="radio" name="rotacion" value="1"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=1&head_direction=1&size=s"></img> |
<input type="radio" name="rotacion" value="2"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=2&head_direction=2&size=s"></img> |
<input type="radio" name="rotacion" value="3"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=3&head_direction=3&size=s"></img> |
<input type="radio" name="rotacion" value="4"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=4&head_direction=4&size=s"></img> |
<input type="radio" name="rotacion" value="5"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=5&head_direction=5&size=s"></img> |
<input type="radio" name="rotacion" value="6"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=6&head_direction=6&size=s"></img> |
<input type="radio" name="rotacion" value="7"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=7&head_direction=7&size=s"></img> |<br><br>
<hr>
Posição X <input name="posicionx" type="text" id="habbo" value="4" size="2" onkeypress="return onlyNumbersDano(event)" />
Posição Y <input name="posiciony" type="text" id="habbo" value="1" size="2" onkeypress="return onlyNumbersDano(event)" /><br>Para encontrar a posição X e Y Vá para o seu quarto onde você pretende visitar e escrever Local BOT<br> :Coordenadas e eu virei a informação que você precisa para preencher esta.<br>

<br>
<br>
<br>


<input name="habbo" type="hidden" id="habbo" value="<?php echo $myrow['username']?>" />


<?php
$me = mysql_real_escape_string($myrow['username']);
$query = mysql_query("SELECT * FROM users WHERE username = '".$me."'");
while ($data = mysql_fetch_array($query)){
$rango = $data['rank'];
$vip = $data['vip'];
}
if ($rango >= 2){ ?>
                    <center><input type="submit" value="Comprar BOT agora" name="buy-bot" /></center><?php } ?>
					<?php
$me = mysql_real_escape_string($myrow['username']);
$query = mysql_query("SELECT * FROM users WHERE username = '".$me."'");
while ($data = mysql_fetch_array($query)){
$rango = $data['rank'];
$vip = $data['vip'];
}
if ($rango == 1){ ?><center>
<input name="button2" type="button" 
onclick='alert("Você deve ser Premium para Comprar BOTS.")' value="Comprar BOT Agora" /></center>
<?php } ?>



</form></div></span>
<span style="float: right;" >
Quais são os BOT?:

<br><br />
 <img src="/images/bot.png" align="right" hspace="2" />

Os membros, Falar Robô<br> Responda, andar etc.<br> Máquinas são programadas <br>por você ou qualquer usuário que própria.<br><br>Você pode colocá-los em seu<br> Imperio, Sala, MacDonals.<br> ou em qualquer outro lugar que você quiser.<br><br>

</span>
</div>
</div>
</div>



<?php require_once('./templates/community_footer.php'); ?>
