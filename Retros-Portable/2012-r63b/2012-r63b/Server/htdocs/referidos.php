<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Referidos";
$pageid = "1000";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');
	

?>

<div id="container">
<div id="content"> 
<div id="column1" class="column">
<div class="habblet-container ">        


<div class="cbb clearfix pixellightblue ">
<h2 class="title"><span style="float: left;">Sistema de Referidos</span> <span style="float: right; font-weight: normal; font-size: 75%;"><?php echo $name; ?></span></h2>
<center><img src=""></center>
<center><br>Quer ser um VIP, <b><?php echo $name; ?></b>? <br><br>Agora <b><?php echo $shortname; ?></b> da VIP.<br> 
<br>Então nós criamos um sistema de referências! Você pode ganhar VIP, 60 indicações você pode entrar na página VIP para resgatar Lince [desde que você tenha as referências necessárias].<br><br>

<b>O que eu faço?</b><br><br>

Basta ir a qualquer chat ou colocar o link a seguir em um fórum.<br />
<br><br><b>Você deve ter o próximo elo na publicação:<b><br><br>
<textarea style="height: 15px; width: 350px; "><?php echo $path; ?>/refer.php?r=<?php echo $name; ?></textarea><br><br>
<h4>Note-se que:</h4>
<ul type="square">
<li>• Se duas pessoas, convidá-lo, basta dizer que como referindo-se o primeiro a fazê-lo.</li>
<li>• O IP do seu encaminhamento é salvo, então não tente enganar. </li>
</ul>
<br></div></center>


</div></tbody>
</table>

</div>
	
<div id="column2" class="column">

<div class="habblet-container">        
<div class="cbb clearfix green">
<h2 class="title">Suas referências</h2>


<div class="habblet box-content">
	
<center><span style=": 0px 0px 7px "><span style="color: black" size:="" 2="">Você tem atualmente:</span></center>
<center><span style=": 0px 0px 7px black"><span style="color: red" size:="" 2="">
<h2>
<b>
<?php if(!isset($_GET['n'])) {
				$usuario = $name;
			} else {
				$usuario = mysql_real_escape_string($_GET['n']);
			}
							$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='". $usuario ."' ORDER BY ID") or die(mysql_error()); 
								$data = mysql_fetch_assoc($query); echo $data['aantalleden'];?></span></center> <center><span style=": 0px 0px 7px blue"><span style="color: black" size:="" 2="">Referidos</span></center>


</div>
</div></div>
<div class="habblet-container">        
<div class="cbb clearfix orange">
<h2 class="title">Para o que serve os referidos?</h2>


<div class="habblet box-content">
<img src="<?php echo $path; ?>/web-gallery/v2/images/xler/habbox_2.gif" align="right">

<span style=": 0px 0px 9px blue"><span style="color: black" size:="" 2=""><b>Agora você pode resgatar o seu 60 encaminhados por vip [1 mês]. Como você pode resgatar seus 60 referências. Nota: Não há nenhuma maneira de retornar o encaminhamento porque este é um sistema automático.</b></span>

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
			        mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Felicidades<br><br>- Agora eu pertenço ao Clube VIP <?php echo $sitename; ?> por 30 dias, o difrútalo máximo dentro do Hotel.')");;
			}
				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 60");
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Pronto, agora pertence ao Clube VIP <?php echo $sitename; ?>. Obrigado pelo convite de seu amigo!</div>';
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

		<h2 class="title">Você tem seus 60 referências?</h2>

		<div class="box-content">

			<div align="center">
			<img src="http://images.wikia.com/habboworldteam/es/images/9/9e/Frank_welcome1.gif" style="float: left;"><span style="text-: 0px 0px 7px black"><span style="color: black" size:="" 2=""> Pode trocar seus <b>60 Referidos</b> por 1 mes de VIP no <?php echo $sitename; ?>.</span></span> <br />

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



<div class="habblet-container ">		
<div class="cbb clearfix white "> 

<h2 class="title">Revisão do usuario </h2>

<div class="box-content">
	<p>

 <form action="" method="post">
            
            Usuario: <input name="user" type="text"maxlength="50" /><input name="" type="submit" value="Ver" /></form>
            
            <?php 
			if(isset($_POST['user'])) {
				$usuario=$_POST['user'];
				$usuario= mysql_real_escape_string($usuario);
				?> 
				<hr /> <br />Busca Estatísticas:<br /><br />Usuario: <b><?php echo htmlentities($_POST['user']) ?></b><br />
                Referidos: <b><?php $query = mysql_query("SELECT COUNT(*) AS aantalleden FROM users_referidos WHERE usuario ='".$usuario."' ORDER BY ID") or die(mysql_error()); 
        $data = mysql_fetch_assoc($query); echo $data['aantalleden']; ?></b>
				<?php
				}
			?>
            </p>
			
</div>
	
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</i><br>
<br><br><br></div></div></div>


    <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<?php require_once('./templates/community_footer.php'); ?>