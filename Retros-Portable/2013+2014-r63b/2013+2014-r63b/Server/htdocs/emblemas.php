<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Loja de Emblemas";
$pageid = "699";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<script src="<?php echo $path; ?>/web-gallery/static/js/shop_badgevip.js" type="text/javascript"></script>

<div id="container">
<div id="content" style="position: relative; left: 0px; top: 0px;" class="clearfix">

<div id="column1" class="column">


<div class="habblet-container ">
<div class="cbb clearfix red "> 
<h2 class="title">Lista de Emblemas <span style="float: right; font-weight: normal; font-size: 75%;">(<?php echo mysql_evaluate("SELECT COUNT(*) FROM cms_marktplatzvip"); ?> Emblemas dispon�veis)</span></h2> 

<div id="Layer1" style="width:444px; height:400px; overflow: scroll;">
<div id="credits-safety" class="box-content credits-info">
<div class="credit-info-text clearfix">
<center>

<?php

$i = 0;
$sql = mysql_query("SELECT * FROM cms_marktplatzvip ORDER BY id ASC LIMIT 100 OFFSET ".$page*20);
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
  <td class='tablesubheader' width='15%' align='left'><b><b/></td>
  <td class='tablesubheader' width='30%' align='left'><b></b></td> 
  <td class='tablesubheader' width='30%' align='left'></td>
 </tr>
<tr>
  <td class='tablerow1' align='left'><img src="http://localhost/swfs/c_images/album1584/<?php echo $row['image']; ?>.gif"></td>
  <td class='tablerow1' align='left'><span style="text-shadow: 0px 2px 5px black"><span style="color: black" size:="" 2=""><b><?php echo $row['name']; ?></b></span></td>
  <td class='tablerow1' align='left'><img src="<?php echo $path; ?>/creditos.gif"> <b><?php echo $row['dolares']; ?> D�lares</b></td>
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
</div></div>

<div id="column2" class="column">

<div class="habblet-container">        
<div class="cbb clearfix green">
<h2 class="title">Teus D�lares</h2>


<div class="habblet box-content">
	
<center>Ol� <?php echo $name; ?>, voc� possui:</center>
<center><br><b style="font-size:15px;"><?php echo $myrow['dolares']; ?> D�lares.</b></br>

<center><br>Voc� pode conseguir d�lares trocando 1 Referido por 5 D�lares.</br></center>


</div>
</div></div>

<?php
$status = NULL;
if(isset($_POST['canjear'])){
	$query = mysql_query("SELECT COUNT(id) AS aantalleden FROM users_referidos WHERE usuario ='".$name."' ORDER BY ID") or die(mysql_error());
			$data = mysql_fetch_assoc($query);


		if($data['aantalleden'] <= '0')
		{	
			$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Voc� n�o possui referidos suficientes :(</div>';
		}

		elseif($data['aantalleden'] >= '1') {

				
			mysql_query("UPDATE users_referidos SET usuario = '' WHERE usuario = '".$name."' LIMIT 1");



	//dar creditos
	$sql = "UPDATE users SET dolares = dolares  + '5' WHERE id = '".$my_id."'";
	//echo $sql;
	//exit;
	mysql_query($sql);		
						
							$status = '<div class="goodmsg" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Voc� trocou seu 1 referido por 5 d�lares! Atualize a p�gina para que eles apare�am!</div>';
							}

		else
	{
		$status = '<div class="error" style="-moz-border-radius: 7px; -webkit-border-radius: 7px; padding: 10px;">Voc� n�o possui referidos suficientes! :(
(</div>';
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
		font-weight: Arial;
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
		font-weight: Arial;
		margin: 0px;
		padding: 0px;
		font-size: 13px;
	}
	div.display_none {
		display: none;
	}
	</style>

<div class="habblet-container">        
<div class="cbb clearfix blue">
<h2 class="title">Consiga D�lares</h2>

</br>
			<div align="center">

			<b>Trocar 1 Referido por 5 D�lares</b><br />
			 <br /> 
			<center><img src="http://i114.piczo.com/view/4/8/l/3/0/1/n/k/x/p/m/1/img/t285725236_35679.gif"></center>
                         <br />
                         <br />
			<?php echo $status; ?>
                        </br>
			
			<form method="post" action="">
			<center><?php if(isset($my_id)) { echo '<input type="submit" value="Trocar" name="canjear" />'; } ?></center>
			</form>
			  </div>
                      </div>
					  <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>

<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>


</div>
</div>

<script type="text/javascript">
HabboView.run();
</script>
<div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<?php require_once('./templates/community_footer.php'); ?>