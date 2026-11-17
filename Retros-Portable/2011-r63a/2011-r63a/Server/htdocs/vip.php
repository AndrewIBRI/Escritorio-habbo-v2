<?php
#####################################################################
#||||| 		Project MyHobba desenvolvido por @rtiag0		   |||||#
#||||| Criado por @rtiag0 - HTML, PHP, CSS & Data_classes      |||||#
# ----------------------------------------------------------------- #
#||||| © Copyright 2012. Hobba 2011 - Open Source Software     |||||#
#||||| © Copyright 2012. LavvosCMS - DATA_CLASSES Estrutura	   |||||#
#||||| © Copyright 2012. Sulake Corporation - Habbo Arquivos   |||||#
#####################################################################

require_once('./data_classes/server-data.php_data_classes-core.php.php');

$pagename = "VIP";
$pageid = "9";
$body_id = "home";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

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
<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">

<div id="container">
<div id="content">
<div id="column1" class="column">
<div class="cbb clearfix red"> 

<h2 class="title"><?php echo $shortname; ?> VIP!</h2>
<div id="habboclub-info" class="box-content">

<h3>Oque é <?php echo $shortname; ?> VIP?</h3>
	<ul>
<b><?php echo $shortname; ?> VIP</b> é um privilégio dentro do Hotel que é totalmente grátis, porém é necessário ter mais de <b>7</b> referidos para adquirí-lo.
		<p>Você ficar totalmente entre os usuários destacados do Hotel, você recebe 5.000 Moedas a cada 15 minutos, Emblema, e um catálogo com raros limitados à seu dispor!<br><img src="http://2.bp.blogspot.com/-UByYPnwJueE/TaR7PAmzNmI/AAAAAAAAAGo/JM147NKGDs0/s200/vip.gif"></br>
		<p>Não perca tempo mais, você pode adquirir VIP convidando o seus amigos! <a href="profile/5">Clique aqui para ir ao sistema de referidos</a>
		
	</ul>
    </div>
</div>

<div class="habblet-container ">        
<div class="cbb clearfix purple ">
<h2 class="title">Usuários VIP</h2>


<?php 

$i = 0;

$getXlers = mysql_query("SELECT * FROM users WHERE rank = '2
' ORDER BY last_online DESC"); 
while($Xler = mysql_fetch_assoc($getXlers)){

$i++;
?>


<li class="<?php echo $even; ?>">
<p><img align="left" src="<?php echo $avatar; echo $Xler['look']; ?>&action=std,wav,&head_direction=3&gesture=sml"><br>
Nome Hobba: <a href="../home/<?php echo $Xler['username']; ?>"><b><?php echo $Xler['username']; ?></b></a>

<br><i><?php echo $Xler['motto']; ?></i></br>
<br><img src="http://2.bp.blogspot.com/-UByYPnwJueE/TaR7PAmzNmI/AAAAAAAAAGo/JM147NKGDs0/s200/vip.gif">
<?php } ?><br>
</table>
	

</div></div></div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
			 
</div>

<div id="column2" class="column">
	<div class="habblet-container ">		
		<div class="cbb clearfix blue ">
		<h2 class="title">Privilégios de ser VIP!</h2>
	<div id="pixels-info" class="box-content pixels-info">
<div class="pixels-info-text clearfix">

<li>Comprando <b><?php echo $shortname; ?> VIP</b> você passa a ter comandos e um catálogo cheinho de novidades!</li>
<b>:push x</b> - Para empurrar um usuário
<br><b>:pull x</b> - Para puxar um usuário</br>
<b>:flagme x</b> - Para mudar o nome de usuário
<br><b>:mimic x</b> - Para puxar um usuário</br>
<b>:moonwalk </b> - Para andar de costas
<br><b>:follow x</b> - Para seguir um usuário</br>
<b>:enable </b> - Ativar efeitos especiais
<br><b>:fly</b> - Permite voar dentro do quarto</br>
</div>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

			 <?php

$status = NULL;
if(isset($_POST['canjear'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não possui nenhuma referência ainda!</div>';
		}

		elseif($data['aantalleden'] >= '15') {

			$row = mysql_fetch_array(mysql_query("SELECT rank FROM users WHERE id = '".$my_id."' LIMIT 1"));
			if($row['rank'] == 1 || $row['rank'] == 2)
			{
				mysql_query("UPDATE users SET rank = '2' WHERE id = '".$my_id."' LIMIT 1");
			}
				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 7");
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Parabéns! Você acabou de juntar-se ao usuários VIP! faça um ótimo aproveito.</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Oops! você não em referências suficientes para tornar-se VIP!</div>';
	}

	}

?>
			 
<div class="habblet-container ">		
	<div class="cbb clearfix pixeldarkblue ">
		<h2 class="title">Troque o seus referidos!</h2>
	<div id="pixels-info" class="box-content pixels-info">
<div class="pixels-info-text clearfix">
Você pode trocar suas referências por um mês de VIP no Hotel, quando você trocar as suas referências voltarão desde o zero.

			<br><?php echo $status; ?>
			<br />
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Adquirir VIP!" name="canjear" />'; } ?></center>
			</form></br>

</div>
</div>
</div>
</div>

<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<script type="text/javascript">
HabboView.run();
</script>
<div id="column3" class="column">

<?php require_once('./templates/community_ads.php'); ?>

<?php require_once('./templates/community_footer.php'); ?>