<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

if(isset($_GET['do'])){ $do = FilterText($_GET['do']); }
$groups_owned = mysql_query("SELECT * FROM group_details WHERE ownerid = '".$my_id."'");

if($myrow['credits'] < 20){ ?>
	<p id="purchase-result-error">Erro!</p><div id="purchase-group-errors">
	  <p><img src="<?php echo $path; ?>/web-gallery/images/frank/sorry.gif" align="right">N&atilde;o h&aacute; cr&eacute;ditos suficientes<a href="<?php echo $path; ?>/credits"></a><br><br><br><br><br></p></div><p><a href="#" class="new-button" onclick="GroupPurchase.close(); return false;"><b>Cancelar</b><i></i></a></p><div class="clear"></div>
<?php }elseif(mysql_num_rows($groups_owned) > 10){ ?>
	<p id="purchase-result-error">Erro!</p><div id="purchase-group-errors">
	  <p><img src="<?php echo $path; ?>/web-gallery/images/frank/sorry.gif" align="right">J&aacute; alcan&ccedil;ou o limite de grupos (10).<br>
      <br><br><br><br></p></div><p><a href="#" class="new-button" onclick="GroupPurchase.close(); return false;"><b>Cancelar</b><i></i></a></p><div class="clear"></div>
<?php }


if(empty($do) || $do !== "purchase_confirmation"){ ?>

<p><img src="<?php echo $path; ?>/web-gallery/images/groups/group_icon.gif"><br><br>
Pre&ccedil;o: <b>20</b> moedas.<br>
Atualmente voc&ecirc; tem: <b><?php echo $myrow['credits']; ?></b> moedas.</p>

<p><b>Nome do grupo:</b><br />
<input type='text' name='name' id='group_name' value='' length='10' maxlength='25'></p>
<p><b>Descri&ccedil;&atilde;o:</b><br /><span id="description_chars_left"><label for="characters_left">Car&aacute;cteres restantes:</label>
<input id="group_description-counter" value="255" size="3" readonly="readonly" class="amount" type="text"></span><br>
<textarea name="group_description" id="group_description" onkeyup="GroupUtils.validateGroupElements('group_description', 255, 'Has alcanzado el l&iacute;mite de car&aacute;cteres');"></textarea>
</div></p>
<p>Pode mudar a descri&ccedil;&atilde;o do grupo em qualquer momento.</p>
<p><a href="#" class="new-button" onclick="GroupPurchase.confirm(); return false;"><b>Comprar</b><i></i></a><a href="#" class="new-button" onclick="GroupPurchase.close(); return false;"><b>Cancelar</b><i></i></a></p>

<?php } elseif($do == "purchase_confirmation"){

$group_name = trim($_POST['name']);
$group_desc = trim($_POST['description']);

if(empty($group_name) || empty($group_desc)){

?>

<p>Por favor, Preencha todos oscampos!</p>
<p><a href="#" class="new-button" onclick="GroupPurchase.close(); GroupPurchase.open(); return false;"><b>Retornar</b><i></i></a></p>

<?php 

} else { 
if(strlen($group_name > 25) && !is_numeric($group_name)){ 

?>

<p>O nome do grupo &eacute; largo. (Max. 25)</p>
<p><a href="#" class="new-button" onclick="GroupPurchase.close(); GroupPurchase.open(); return false;"><b>Retornar</b><i></i></a></p>

<?php } elseif(strlen($group_desc > 255) && !is_numeric($group_desc)){ ?>

<p>A descri&ccedil;&atilde;o do grupo &eacute; muito larga. (Max. 255)</p>
<p><a href="#" class="new-button" onclick="GroupPurchase.close(); GroupPurchase.open(); return false;"><b>Cancelar</b><i></i></a></p>

<?php } else {

	$check = mysql_query("SELECT id FROM group_details WHERE name = '".$group_name."' LIMIT 1") or die(mysql_error());
	$already_exists = mysql_num_rows($check);

	if($already_exists > 0){
?>

<p>O nome do grupo ja existe, tente outro.</p>
<p><a href="#" class="new-button" onclick="GroupPurchase.close(); GroupPurchase.open(); return false;"><b>Tentar</b><i></i></a></p>

<?php } else {

	mysql_query("INSERT INTO group_details (name,description,ownerid,roomid,created,badge,type,views) VALUES ('".FilterText($group_name)."','".FilterText($group_desc)."','".$my_id."','0','".$date_full."','b0503Xs09114s05013s05015','0','0')") or die(mysql_error());

	$check = mysql_query("SELECT id FROM group_details WHERE ownerid = '".$my_id."' ORDER BY id DESC LIMIT 1") or die(mysql_error());
	$row = mysql_fetch_assoc($check);

		mysql_query("INSERT INTO group_members (id_user,id_group,rank,is_current) VALUES ('".$my_id."','".$row['id']."','2','0')") or die(mysql_error());
		mysql_query("UPDATE users SET credits = credits - 20 WHERE id = '".$my_id."' LIMIT 1") or die(mysql_error());
		mysql_query("INSERT INTO cms_transactions (userid,descr,date,amount) VALUES ('".$my_id."','Gruppe gekauft','".$date_full."','-20')") or die(mysql_error());

?>

<p><b>Grupo criado!</b><br /><br />
<img src='./habbo-imaging/badge-fill/b0503Xs09114s05013s05015.gif' border='0' align='left'>Parab&eacute;ns voc&ecirc; &eacute; dono do <b><?php echo HoloText($group_name); ?></b>.<br><br><br>
Click <a href='<?php echo $path; ?>/groups/<?php echo $row['id']; ?>'>aqui</a> para ir ao grupo.</p>
<p><a href="#" class="new-button" onclick="GroupPurchase.close(); return false;"><b>Mais &raquo;</b><i></i></a></p>

<?php } } }} else { ?>

<p>Erro de sistema - Por favor, tente novamente mais tarde</p>
<p><a href="#" class="new-button" onclick="GroupPurchase.close(); return false;"><b>OK</b><i></i></a></p>

<?php } ?>