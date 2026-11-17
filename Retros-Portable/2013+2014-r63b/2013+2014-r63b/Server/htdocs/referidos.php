<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Referidos";

$pageid = "39";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');
	

?>

<div id="container">
<div id="content" style="position: relative" class="clearfix"><div id="column1" class="column"><div class="cbb clearfix blue ">
<h2 class="title"><span style="float: left;">Referidos no <?php echo $shortname; ?> Hotel</span></h2>
<div class="box-content">
<img src="http://www.foroswebgratis.com/imagenes_foros/1/3/9/1/0/595199arbol_habbos.gif" style="float: right;">
<?php echo $name;  ?>, no <?php echo $shortname; ?> nós adoramos ter muitos usuários on-lines no Hotel, com isso, a Equipe estará sempre disponível proporcionando eventos à vocês. Assim, nós usamos o Sistema de Referências para que os usuários divulguem colocando o link de seus referidos em chats, fóruns, redes sociais, etc, assim, mais usuários novatos se registrando no Hotel!<br><br>
Quanto mais Referidos você possuir, poderá trocar por alguma coisa em nossa Loja de emblemas/raros e outras mobílias que estarão disponíveis para trocar por Referências. Também estará disponível enables incríveis e exclusivo só para aqueles que conseguirem muitas Referências.<br><br>
<b>O que eu devo fazer para conseguir referidos?</b><br><br>
É fácil e básico, você poderá fazer isso muito rápido. Basta ir em algum chat, fórum, rede social, algo do gênero e colar o link da sua referências no local!<br/><br>
<b>Copie este link e cole onde você quiser, assim conseguindo mais referências!</b><br><br>
<textarea onClick="this.select()" readonly style="height: 15px; width: 350px; "><?php echo $path; ?>/refer/<?php echo $myrow['username']; ?></textarea>
<br><br>

<b>Convide o seus amigos do Facebook!</b></br>
<br><a href="https://www.facebook.com/sharer/sharer.php?u=<?php echo $path; ?>/refer/<?php echo $name; ?>"><img title="Convide teus amigos!" src="http://images.maquinadevendas.com.br/site/es/button/btn_compartilhar_facebook.jpg"></a><br></br>

<h4>Vale ressaltar que:</h4>
<ul type="square">
<li>• Se duas pessoas convidam a mesma, só contará a primeira após o registro.</li></br>
<li>• O IP do teu referido ficará guardado, assim que não tentes fazer clones.</li>
</ul>
</br></div></div></div>
	
<div id="column2" class="column">
        
<div class="cbb clearfix white">
<h2 class="title">Contador de referidos</h2>
<div class="habblet box-content">
<img src="http://images.wikia.com/habboworldteam/es/images/9/9e/Frank_welcome1.gif" style="float: left;">Nesta área aqui você poderá ver quantos referidos você conseguirá até hoje, lembre-se que juntando Referências você poderá adquirir algumas mobílias, enables e outras coisas que estarão disponíveis em nossa loja!
</br>
<b><span>Tens atualmente:</b></span>
<center><br>
<b><?php if(!isset($_GET['n'])) {
				$usuario = $name;
			} else {
				$usuario = mysql_real_escape_string($_GET['n']);
			}
							$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='". $usuario ."' ORDER BY ID") or die(mysql_error()); 
								$data = mysql_fetch_assoc($query); echo '<center><font color="red">'; echo $data['aantalleden'];?></font></span></center> </br><center>Referidos</span></center>

</b>
</div></center>

			
</div>	


<?php

	// $timened = time() + 999999999992000;
	$timeend2 = time() + 999999999992000 * 2;

$status = NULL;
if(isset($_POST['canjear'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você não tem nenhuma referência.</div>';
		}

		elseif($data['aantalleden'] >= '5') {

			$row = mysql_fetch_array(mysql_query("SELECT rank FROM users WHERE id = '".$my_id."' LIMIT 1"));
			if($row['rank'] == 1 || $row['rank'] == 1)
			{
				mysql_query("UPDATE users SET vip_points = vip_points + 0, rank = '2', vip = '1' WHERE id = '".$my_id."'") or die(mysql_error());
			        mysql_query("INSERT INTO vip (id_user,timestamp,timestampend) VALUES ('".$my_id."','".time()."','".$timeend2."')") or die(mysql_error());
			        mysql_query("INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Você trocou 5 referidos pelo enable. Obrigado pelo convite ao seu amigo!')");;
			}
				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 5");
		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Você trocou 5 referidos pelo enable. Obrigado pelo convite ao seu amigo!</div>';
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


<div class="habblet-container ">
		<?php if($myrow['online'] == '1')
{  ?>		
		<div class="cbb clearfix gray "> 
		<?php } ?>
		<?php if($myrow['online'] == '0')
{  ?>		
		<div class="cbb clearfix gray "> 
		<?php } ?>

		<h2 class="title">Prêmios</h2>
<div align="center">
<h2><a href="<?php echo $path; ?>/premio">Prêmios</a></h2>

		<div class="box-content">

			<div align="center">





</div></div>
			

                <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>



    <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
