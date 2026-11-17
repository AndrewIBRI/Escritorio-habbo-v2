<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Loja de Raros";
$pageid = "9";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<script src="<?php echo $path; ?>/web-gallery/static/js/shop_furni.js" type="text/javascript"></script>

<div id="container">
<div id="content" style="position: relative; left: 0px; top: 0px;" class="clearfix">

<div id="column7" class="column">

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
		echo '<h2>Bem feito.</h2>';
		echo 'Detectamos que estão fora do hotel, você pode continuar com a compra de itens raros.</font>';
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
<div class="cbb clearfix orange "> 
<h2 class="title"><?php echo $sitename; ?> - Loja de Raros</h2> 

<center><br><img src="./web-gallery/images/catalogue/headers/Frontpage.gif"></center>

<div id="credits-safety" class="box-content credits-info">
<div class="credit-info-text clearfix">
<center>

<?php

$i = 0;
$sql = mysql_query("SELECT * FROM cms_marktplatzfurni ORDER BY id ASC LIMIT 20 OFFSET ".$page*20);
while($row = mysql_fetch_assoc($sql)) { 

$i++;
        if(IsEven($i)){
            $even = "even";
        } else {
            $even = "odd";
        }

?>

<li class="<?php echo $even; ?>">
 <table cellpadding='4' cellspacing='0' width='100%'>

 <tr>
  <td class='tablesubheader' width='20%' align='left'><b>Raro</b></td>
  <td class='tablesubheader' width='15%' align='left'><b>Título</b></td>
  <td class='tablesubheader' width='30%' align='left'><b>Descrição</b></td> 
  <td class='tablesubheader' width='10%' align='left'><b>Creditos</b></td>
  <td class='tablesubheader' width='10%' align='left'><b>Pixels</b></td>
 </tr>
<tr>
  <td class='tablerow1' align='left'><img src="images/rares/<?php echo $row['image']; ?>.gif"></td>
  <td class='tablerow1' align='left'><?php echo $row['name']; ?></td>
  <td class='tablerow1' align='left'><?php echo $row['description']; ?></td>
  <td class='tablerow1' align='left'><?php echo $row['credits']; ?></td>
  <td class='tablerow1' align='left'><?php echo $row['pixels']; ?></td>
  <td class='tablerow1'><a class="new-button" onclick="habboclub.buttonClick(<?php echo $row['id']; ?>,'Confirmar compra'); return false; "href="#" id="webstore-purchase-disabled"><b>Comprar</b><i></i></a></td>

</td>
</tr>
</table>
</li>


<?php } ?>
							
</div>
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