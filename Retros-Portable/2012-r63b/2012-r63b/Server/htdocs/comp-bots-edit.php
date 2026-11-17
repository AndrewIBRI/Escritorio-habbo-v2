<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Editar Bots";
$pageid = "71";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>
<div id="container">
<div id="content"> 
<div id="column1" class="column">
<?php
$error = "";
$precio = "3";

$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");

$saldo = 0;
while ($data = mysql_fetch_array($query)){

$saldo = $data['dolares'];
}


$bot = "";
$error = "";
if(isset($_POST['select-bot']))
{

$bot = $_POST['select-bot'];
		
	
}

$bot_info = @mysql_query("SELECT * FROM bots WHERE id='$bot'");
if($row = mysql_fetch_assoc($bot_info))

?>

<div class="cbb clearfix blue ">
<h2 class="title"><span style="float: left;">Editar BOTS</span> <span style="float: right; font-weight: normal; font-size: 75%;">;D</span></h2>
<script type="text/javascript">
function c(op){
var d = document.getElementById("add");
if (op == "select-bot"){
d.innerHTML = '<textarea name="textarea" id="textarea" cols="45" rows="5"></textarea>';
}
}
</script> 
<div style=" : #808080 1px 1px 0px;padding-top:5px;padding-left:5px;color:#000;font-weight:bold;font-size:11px;float:center;">
<?php echo $error; ?>
<?php $query = mysql_query("SELECT * FROM bots WHERE id ='$bot'");
while ($data = mysql_fetch_array($query)){
$bot_nombre =$data['name'];
}
 ?>
<b>BOTS:</b><?php
if (isset($_POST['select-bot'])) {

echo "<font color='#009900'>Eu estou editando o BOT $bot_nombre</font>";
}

 else {
?>
<img src="/images/bot.png" align="right" hspace="2" />
Olá, aqui você pode editar sua BOTS, coloque-os Efeitos Editar Your Clothes Seu Câmara, Crearles Respostas para Quando um usuário diz uma palavra, Ou expressões para a Random Diga Não há necessidade de um usuário para Discussão.<br><br>NOTAS:<br><br>* Você pode ter um máximo de 20 BOTS<br><br>* Máximo de 20 respostas BOT<br><br>* Máximo de 20 recados por BOT<br><br>* Cada BOT tem um custo de 3 pesos<br><br>* Você pode comprar os efeitos que deseja<br><br>* Mas é claro que você pode colocar apenas uma vez Asa<br><br>* Cada efeito tem um custo de 1 euro<br><br>* Ao comprar um efeito que você pode usar o que poucos querem BOTS<br><br><font color='#FF1000'>* Um olhar Aparecendo Ao editar o seu bot é o que você está vestindo</font><br> Então você pode colocar as roupas que eles querem com apenas Colocartela seu.<br><br>* Ao comprar um BOT Você será capaz Remover mim, mas não Suas frases e Respostas Se.

<?php } ?>


<?php 
$bot_room = $row['room_id'];
$query = mysql_query("SELECT * FROM rooms WHERE id ='$bot_room'");
while ($rooms2 = mysql_fetch_array($query)){
$room_name = $rooms2['caption'];
} ?>
<?php
if (isset($_POST['select-bot'])) {
?>

<form method="post">
<b>Nome:</b><input name="nombre" type="text" id="habbo" value="<?php echo $row['name'] ?>" /> Digite um nome para o BOT<br>
<b>Missão:</b><input name="mision" type="text" id="habbo" value="<?php echo $row['motto'] ?>" /> Escreva uma Missão para o BOT<br>
<b>Sala:</b><select name="sala">
<option value="<?php echo $row['room_id'] ?>"><?php echo $room_name; ?> (Actual)</option>
<?php $query = mysql_query("SELECT * FROM rooms WHERE owner ='".$myrow['username']."'");
while ($data = mysql_fetch_array($query)){ ?>
<option value="<?php echo $data['id'] ?>"><?php echo $data['caption'] ?></option>
<?php } ?>
</select>Para o Conselho de BOT<br>
Modo Pedestre:<select name="estado">
<option value="<?php echo $row['walk_mode'] ?>">(Actual)</option>
<option value="stand">Quieto</option>
<option value="freeroam">Livremente</option></select>Site é possível mover ?<br>
<hr>
Escolha um olhar para o seu BOT
<?php
$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'") or die(mysql_error()); 
while ($data2 = mysql_fetch_array($query)){
$look=$data2['look'];
$id = $data2['id']; }
?><br>
<input type="radio" name="look" value="<?php echo $row[look] ?>" checked><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=<?php echo $row[look] ?>&amp;direction=2&amp;head_direction=2&amp;size=s"></img>
<input type="radio" name="look" value="<?php echo $look; ?>"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=<?php echo $look; ?>&amp;direction=2&amp;head_direction=2&amp;size=s"></img> |
<input type="radio" name="look" value="ch-3049-1336.hd-600-8.lg-3088-1341-1340.sh-3180-110-1336.hr-678-37"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=ch-3049-1336.hd-600-8.lg-3088-1341-1340.sh-3180-110-1336.hr-678-37&amp;direction=2&amp;head_direction=2&amp;size=s"></img> |
<input type="radio" name="look" value="hr-828-45.lg-285-64.ch-215-110.hd-180-2.sh-290-62.he-1607-"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-45.lg-285-64.ch-215-110.hd-180-2.sh-290-62.he-1607-&amp;direction=2&amp;head_direction=2&amp;size=s"></img> |
<input type="radio" name="look" value="hd-180-7.sh-290-110.lg-270-91.ch-809-62.hr-828-45"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hd-180-7.sh-290-110.lg-270-91.ch-809-62.hr-828-45&amp;direction=2&amp;head_direction=2&amp;size=s"></img>Seleccion un Look.<br>
<hr>
<center> Rotação e Posição </center>
<input type="radio" name="rotacion" value="<?php echo $row[rotation] ?>" checked><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=<?php echo $row[rotation] ?>&head_direction=<?php echo $row[rotation] ?>&size=s"></img> |
<input type="radio" name="rotacion" value="0"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=0&head_direction=0&size=s"></img> |
<input type="radio" name="rotacion" value="1"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=1&head_direction=1&size=s"></img> |
<input type="radio" name="rotacion" value="2"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=2&head_direction=2&size=s"></img> |
<input type="radio" name="rotacion" value="3"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=3&head_direction=3&size=s"></img> |
<input type="radio" name="rotacion" value="4"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=4&head_direction=4&size=s"></img> |
<input type="radio" name="rotacion" value="5"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=5&head_direction=5&size=s"></img> |
<input type="radio" name="rotacion" value="6"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=6&head_direction=6&size=s"></img> |
<input type="radio" name="rotacion" value="7"><img src="http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=hr-828-42&direction=7&head_direction=7&size=s"></img> |<br><br>
<hr>
Posição X<input name="posicionx" type="text" id="habbo" value="<?php echo $row[x] ?>" size="2" onkeypress="return onlyNumbersDano(event)" />
Posição Y<input name="posiciony" type="text" id="habbo" value="<?php echo $row[y] ?>" size="2" onkeypress="return onlyNumbersDano(event)" /><br>Para encontrar a posição X e Y Vá para seu quarto onde você quer colocar-se e escrever o BOT<br> :Coordenadas e eu virei a informação que você precisa para preencher esta.<br>
<hr>

<b><center>Efeito do BOT</CENTER></B>

<select name="efecto">
<option value="<?php echo $row['effect'] ?>"><?php $effect=$row['effect'];if ($effect == 0){$efecto_nombre= Ninguno; }if ($effect >= 1){$efecto_nombre= $row[effect]; } ?><?php echo $efecto_nombre; ?> (Actual)</option>
<?php $query = mysql_query("SELECT * FROM bots_effects WHERE user_id ='".$my_id."'");

while ($data11 = mysql_fetch_array($query)){ ?>
<option value="<?php echo $data11['effect'] ?>"><?php echo $data11['nombre'] ?></option>
<?php } ?>
<option value="0">Sem efeito</option>
</select>
<br>

Que você fez? Clique no botão ao lado, certifique-se todos os campos estão corretos.<br><br>
<input name="habbo" type="hidden" id="habbo" value="<?php echo $myrow['username'];?>" />
<input name="editarbot" type="hidden" id="habbo" value="<?php echo $bot; ?>" />

<?php
$me = mysql_real_escape_string($myrow['username']);
$query = mysql_query("SELECT * FROM users WHERE username = '".$me."'");
while ($data = mysql_fetch_array($query)){
$rango = $data['rank'];
$vip = $data['vip'];
}
if ($rango >= 2){ ?>
                    <center><input type="submit" value="Atualizar BOT" name="edit-bot" /></center><?php } ?>
					<?php
$me = mysql_real_escape_string($myrow['username']);
$query = mysql_query("SELECT * FROM users WHERE username = '".$me."'");
while ($data = mysql_fetch_array($query)){
$rango = $data['rank'];
$vip = $data['vip'];
}
if ($rango == 1){ ?><center>
<input name="button2" type="button" 
onclick='alert("Você deve ser Premium Comprar ou Editar BOTS.")' value="Atualizar BOT" /></center>
<?php } ?>

<?php } ?>
</form>




<?php

		
if(isset($_POST['edit-bot']))
{
	$user_info = @mysql_query("SELECT * FROM users WHERE username='". $myrow['username'] ."'");
	if($row = mysql_fetch_assoc($user_info))
	{
		$id = $row['id'];
		$owner = $row['id'];
		$pixeles = $row['activity_points'];
		$creditos2 = $row['credits'];
		$tranferencia = 'Creditos y Pixeles';
		$nombre = $_POST['nombre'];
$mision = $_POST['mision'];
$ropa = $_POST['look'];
$posicionx = $_POST['posicionx'];
$posiciony = $_POST['posiciony'];
$sala = $_POST['sala'];
$estado = $_POST['estado'];
$rotacion = $_POST['rotacion'];
$effect = $_POST['efecto'];
$editarbot = $_POST['editarbot'];


		
		$check_bot = mysql_query("SELECT * FROM bots WHERE owner='". $id ."'");
		$check_user = mysql_query("SELECT * FROM users WHERE id='". $id ."'");
		$check_stats = mysql_query("SELECT * FROM user_stats WHERE id='". $id ."'");

		
		if(mysql_num_rows($check_bot) > 20)
		{
			$error.= "Por agora só permite Usuário 20 BOT!<br /><br />";
		}
		else
		{
			if($mostrar = mysql_fetch_assoc($check_stats))
			{
				$respetos = $mostrar['Respect'];
				if($saldo >= 0)

				{
					
					mysql_query("UPDATE bots SET room_id = '$sala', name= '$nombre', motto = '$mision', look = '$ropa', x ='$posicionx', y ='$posiciony', rotation = '$rotacion', walk_mode = '$estado', effect = '$effect', owner = '$owner' WHERE id = '$editarbot'");
					$error.= "<font color='#009900'>BOT Atualizado<br> BOT para atualizá-lo ESCREVER :update_bots  ENTÃO :unload Para recarregar a QUARTO.<br /></font>";
				}
				else
				{
					$error.= "<font color='#FF0000'>Não medidas suficientes para fazer a compra!<br /><br /></font>";
				}
			}
		}
	}
}

?>





</div>
</div>
<?php
$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");

$saldo = 0;
while ($data = mysql_fetch_array($query)){

$saldo = $data['credits'];
}

if (isset($_POST['rare'])){

$bot_res = $_POST['bot_res'];
$rare = $_POST['rare'];
$contesta = $_POST['contesta'];



$query = mysql_query("SELECT * FROM bots_responses WHERE bot_id = '$bot_res'");
if (mysql_num_rows($query) < 30){
$data = mysql_fetch_array($query);

$response_id = $_POST['response_id'];
$eliminar = $_POST['eliminar'];
$modo = $_POST['modo'];

if (empty($rare)) {
echo "<b><font color='#009900'>Desculpe, não podem ter campos vazios.</font><b>";
}

else 

if (empty($contesta)) {
echo "<b><font color='#009900'>Desculpe, não podem ter campos vazios.</font><b>";
}


else {

mysql_query("UPDATE bots_responses SET keywords = '$rare', response_text = '$contesta', mode = '$modo' WHERE id = '$response_id'");

if ($eliminar == 1){
mysql_query("DELETE FROM bots_responses WHERE id = '$response_id'");
 }
header("Location: /bots/editar?exito");
}

}else{
header ("Location: /bots/editar?error=monto");
die;
}
}


?>

<?php 
if (isset($_GET['exito'])){ ?>
<script>
alert('BOT Atualizado Recorda Escreva no Hotel :update_bots e, em seguida :unload O quarto no BOT.');
</script>
<?php } ?>
<?php
if (isset($_GET['error']) && $_GET['error'] == 'monto'){ ?>
<script>
alert('OPPS¡¡ Houve um erro de re Desculpe. ');
</script>
<?php } ?>


<?php
$total = mysql_query("SELECT * FROM bots_responses WHERE bot_id = '$bot'");
$total = mysql_num_rows($total);
?>
<?php

$error = "";

if(isset($_POST['agregar']))
{

$bot_res = $_POST['bot_res'];
$dice = $_POST['dice'];
$contesta = $_POST['contesta'];
$modo = $_POST['modo'];


	$user_info = @mysql_query("SELECT * FROM users WHERE id='".$_POST["habbo"]."'");
	if($rew = mysql_fetch_assoc($user_info))
	
		$name = $rew['username'];


if (empty($dice)) {
echo "<b><font color='#009900'>Eu sinto não pode haver campos vazios.</font><b>";
}

else 

if (empty($contesta)) {
echo "<b><font color='#009900'>Desculpe, não podem ter campos vazios.</font><b>";
}


else {
		
		$check_bot = mysql_query("SELECT * FROM bots_responses WHERE bot_id='$bot_res'");
				if(mysql_num_rows($check_bot) > 20)
		{
			$error.= "Permite apenas 20 Responses para BOT!<br /><br />";
		}
		else {
		
		

mysql_query("INSERT INTO bots_responses (bot_id, keywords, response_text, mode) VALUES ('$bot_res', '$dice', '$contesta', '$modo')");



}
}
}
?>
<?php
if (isset($_POST['select-bot'])) {
?>
<div class="cbb clearfix red ">
<h2 class="title"><span style="float: left;">Respostas BOT</span> <span style="float: right; font-weight: normal; font-size: 75%;">(<?php echo $total ?> Respuestas)</span></h2>
<div style="height:600px;overflow:auto;width:99.5%;">
<center><b style="font-size:15px; color:#000000;">Adicionar Respostas</b></center><br />
<div class="box-content">
<form method="post">
<b>Quando um usuário diz:<input name="dice" type="text" value="Olá;bom;ola"/><br></b>Você pode separar as palavras por <b>Ponto e vírgula</b>.<br><br>
<b>O BOT irá responder:<input name="contesta" type="text"   value="Olá Como Elas Bem-vindo ao Conselho" size="40"/><br>
<input type="hidden" name="bot_res" value="<?php echo $bot; ?>">
<select name="modo">
<option value="say">Dizer</option>
<option value="shout">Gritar</option>
<option value="whisper">Sussurrar</option>
</select>
                    <input type="submit" value="Adicionar" name="agregar"  class="new-button fill" />
 </form></div>

<hr><br><center><b style="font-size:15px; color:#000000;">Edite Respostas</b></center><br /><br />
<?php
$color = '#FFFFFF';
 $query=mysql_query("SELECT * FROM bots_responses WHERE bot_id = '$bot' ORDER BY ID DESC");
$cuantos2 =$data['valor'];

while ($data=mysql_fetch_array($query)){ 

if ($color == '#FFFFFF'){
	
	$color = '#CAFF70';
	}else{
	$color = '#FFFFFF';	
	}?>

<div style="width:100%;height:150px;padding-top:10px;background-color:<?php echo $color ?>;">
<div class="box-content">
<table cellspacing="0" cellpadding="0" style="width:100%;">

<form method="post" action="">
<tr>

Quando um usuário diz:<input type="text" name="rare" size="30" value="<?php echo $data['keywords'] ?>"><br>
A resposta BOT:<input type="text" name="contesta" size="37" value="<?php echo $data['response_text'] ?>"><br>
Modo:<select name="modo">
<option value="say">Dizer</option>
<option value="shout">Gritar</option>
<option value="whisper">Sussurrar</option>
</select><br><br>Eliminar: <input type="radio" name="eliminar" value="0" checked>Não | <input type="radio" name="eliminar" value="1" onchange="this.form.submit()">Sim, Eliminar.<br></td>
<td><div style="margin-top:15px;margin-right:20px;" align="right" class="redeem-redeeming-button"><input type="hidden" name="response_id" value="<?php echo $data['id'] ?>"><input type="hidden" name="bot_res" value="<?php echo $bot; ?>">

<input type="submit" value="Atualizar" class="new-button fill">

</form></div>
</td>
</tr>
</table>
</div></div>
<?php } ?>

</div>
</div><?php } ?>
</div>
<div id="column2" class="column">
<?php 
$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");

$saldo = 0;
while ($data = mysql_fetch_array($query)){

$saldo = $data['credits'];
}

?>
<div class="habblet-container ">		
<div class="cbb clearfix green "> 
<h2 class="title">Editar um BOT</h2>
<div class="box-content">
<table width="250" border="1"  cellpadding="2" cellspacing="1" align="left">

<tr>           <td><b>BOT</b></td>
<td><b>Editar</b></td>
            <td><b>Nome</b></td>
            <td><b>Sala</b></td>
            <td><b>Efeito</b></td>
        </tr>
		<form method="post" action="/bots/editar">
<?php 

$query2 = mysql_query("SELECT * FROM bots_effects WHERE user_id ='".$my_id."'");
while ($data2 = mysql_fetch_array($query2))
$effect_name=$data2['nombre'];

$query = mysql_query("SELECT * FROM bots WHERE owner ='".$my_id."'");
while ($data = mysql_fetch_array($query))


{ ?>


       <tr>
            <td><img src="http://www.habbo.es/habbo-imaging/avatarimage?figure=<?php echo $data['look'] ?>&amp;direction=2&amp;head_direction=2&amp;size=s"></td>
            <td><input type="radio" name="select-bot" value="<?php echo $data['id'] ?>" onchange="this.form.submit()">
<input name="habbo" type="hidden" id="habbo" value="<?php echo $myrow['username'];?>" /></td>
			<td><?php echo $data['name'] ?></td>
            <td><?php $room=$data['room_id'];$room_info = @mysql_query("SELECT * FROM rooms WHERE id='$room'");if($row = mysql_fetch_assoc($room_info)){$sala = $row['caption'];}?><?php echo $sala; ?></td>
            <td><?php $effect=$data['effect'];if ($effect == 0){$efecto= Ninguno; }if ($effect >= 1){$efecto= $effect_name; }?> <?php echo $efecto; ?></td>
        </tr><br>
<?php } ?></form>
    
</table>

</div></div></div>


<div class="habblet-container ">		
<div class="cbb clearfix green "> 
<h2 class="title">Efeitos para o seu BOT</h2>
<div class="box-content">
Lembre-se que você só pode usar BOT `S.
<table width="250" border="0" cellpadding="0" cellspacing="1" align="left">

<tr>         
            <td><b>Nome</b></td>
            <td><b>ID</b></td>
        </tr>
<?php $query = mysql_query("SELECT * FROM bots_effects WHERE user_id ='".$my_id."'");
while ($data = mysql_fetch_array($query)){ ?>
<tr>
            <td><img src="<?php echo $data['imagen'] ?>"></td> 
            <td><?php echo $data['nombre'] ?></td>
            <td><?php echo $data['effect'] ?></td>
        </tr>

<?php } ?></table>

</div></div></div>



<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>


<?php

if(isset($_POST['select-bot']))
{

$bot = $_POST['select-bot'];
		
	
}

$total = mysql_query("SELECT * FROM bots_speech WHERE bot_id = '$bot'");
$total = mysql_num_rows($total);
?>

<?php
$query = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");

$saldo = 0;
while ($data = mysql_fetch_array($query)){

$saldo = $data['credits'];
}

if (isset($_POST['dice'])){

$bot_res = $_POST['bot_res'];
$dice = $_POST['dice'];
$modo = $_POST['modo'];



$query = mysql_query("SELECT * FROM bots_speech WHERE bot_id = '$bot_res'");
if (mysql_num_rows($query) < 30){
$data = mysql_fetch_array($query);

$response_id = $_POST['response_id'];
$eliminar = $_POST['eliminar'];
$modo = $_POST['modo'];

if (empty($dice)) {
echo "<b><font color='#009900'>Desculpe, não podem ter campos vazios.</font><b>";
}

else {

mysql_query("UPDATE bots_speech SET text = '$dice', shout = '$modo' WHERE id = '$response_id'");

if ($eliminar == 1){
mysql_query("DELETE FROM bots_speech WHERE id = '$response_id'");
 }
header("Location: /bots/editar?exito");
}

}else{
header ("Location: /bots/editar?error=monto");
die;
}
}


?>


<?php

$error = "";

if(isset($_POST['agregarfrase']))
{

$bot_res = $_POST['bot_res'];
$dice = $_POST['dice'];
$modo = $_POST['modo'];


	$user_info = @mysql_query("SELECT * FROM users WHERE id='".$_POST["habbo"]."'");
	if($rew = mysql_fetch_assoc($user_info))
	
		$name = $rew['username'];

		if (empty($dice)) {
echo "<b><font color='#009900'>Desculpe, não podem ter campos vazios.</font><b>";
}

else {
		
		$check_bot = mysql_query("SELECT * FROM bots_speech WHERE bot_id='$bot_res'");
				if(mysql_num_rows($check_bot) > 20)
		{
			$error.= "Permite apenas 20 Scraps por BOT!<br /><br />";
		}
		else {
		
		

mysql_query("INSERT INTO bots_speech (bot_id, text, shout) VALUES ('$bot_res', '$dice', '$modo')");



}
}
}
?>
<?php
if (isset($_POST['select-bot'])) {
?>
<div class="cbb clearfix red ">
<h2 class="title"><span style="float: left;">Frases aleatórias</span> <span style="float: right; font-weight: normal; font-size: 75%;">(<?php echo $total ?> Frases)</span></h2>
<div style="height:600px;overflow:auto;width:99.5%;">
<center><b style="font-size:15px; color:#000000;">Adicionar Scraps</b></center><br />
<div class="box-content">
<form method="post">
<b>O BOT irá dizer uma frase:<input name="dice" type="text" value="Bem-vindo ao meu quarto" size="35"/></b><br>
<input type="hidden" name="bot_res" value="<?php echo $bot; ?>">
<select name="modo">
<option value="0">Dizer</option>
<option value="1">Gritar</option>
</select>
                    <input type="submit" value="Adicionar" name="agregarfrase"  class="new-button fill" />
 </form></div>

<hr><br><center><b style="font-size:15px; color:#000000;">Editar Frases</b></center><br /><br />
<?php
$color = '#FFFFFF';
 $query=mysql_query("SELECT * FROM bots_speech WHERE bot_id = '$bot' ORDER BY ID DESC");
$cuantos2 =$data['valor'];

while ($data=mysql_fetch_array($query)){ 

if ($color == '#FFFFFF'){
	
	$color = '#FFE4C4';
	}else{
	$color = '#FFFFFF';	
	}?>

<div style="width:100%;height:150px;padding-top:10px;background-color:<?php echo $color ?>;">
<div class="box-content">
<table cellspacing="0" cellpadding="0" style="width:100%;">

<form method="post" action="">
<tr>

Frase que o BOT vai dizer:<input type="text" name="dice" size="37" value="<?php echo $data['text'] ?>"><br>
En Modo:<select name="modo">
<option value="0">Dizer</option>
<option value="1">Gritar</option>
</select><br><br>Eliminar: <input type="radio" name="eliminar" value="0" checked>Não | <input type="radio" name="eliminar" value="1" onchange="this.form.submit()">Sim, Eliminar.<br></td>
<td><div style="margin-top:15px;margin-right:20px;" align="right" class="redeem-redeeming-button"><input type="hidden" name="response_id" value="<?php echo $data['id'] ?>"><input type="hidden" name="bot_res" value="<?php echo $bot; ?>">

<input type="submit" value="Atualizar" class="new-button fill">

</form></div>
</td>
</tr>
</table>
</div></div>
<?php } ?>

</div>
</div><?php } ?>
</div>
</div>
</div>
<?php require_once('./templates/community_footer.php'); ?>