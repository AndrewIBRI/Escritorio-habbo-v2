<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$check = mysql_query("SELECT groupid,active FROM homes_group_linker WHERE userid = '".$my_id."' AND active = '1' LIMIT 1") or die(mysql_error());
$linked = mysql_num_rows($check);

$refer = $_SERVER['HTTP_REFERER'];

if($linked > 0){
	$linkdata = mysql_fetch_assoc($check);
	$groupid = $linkdata['groupid'];
	$pos = strrpos($refer, "groups");
} else {
	$pos = strrpos($refer, "home");
}

/** Quick function to insert or update the user's inventory
*
* UpdateOrInsert(type,amount,data,userid);
* always returns true or cuts the script off with a mysql error
*
*/

function formatThing($type,$data,$pre)
{
	$str = "";

	switch($type){
		case 1: $str = $str . "s_"; break;
		case 2: $str = $str . "w_"; break;
		case 3: $str = $str . "commodity_"; break; // =S
		case 4: $str = $str . "b_"; break;
	}

	$str = $str . $data;

	if($pre == true){ $str = $str . "_pre"; }

	return $str;
}

function UpdateOrInsert($type,$amount,$data,$my_id)
{
	$data = FilterText($data);
	$type = FilterText($type);
	$amount = FilterText($amount);

	$check = mysql_query("SELECT id FROM homes_inventory WHERE data = '".$data."' AND userid = '".$my_id."' AND type = '".$type."' LIMIT 1") or die(mysql_error());
	$exists = mysql_num_rows($check);

	if($exists > 0){
		mysql_query("UPDATE homes_inventory SET amount = amount + ".$amount." WHERE userid = '".$my_id."' AND type = '".$type."' AND data = '".$data."' LIMIT 1") or die(mysql_error());
	} else {
		mysql_query("INSERT INTO homes_inventory (userid,type,subtype,data,amount) VALUES ('".$my_id."','".$type."','0','".$data."','".$amount."')") or die(mysql_error());
	}

	return true;
}

/** Quick function to delete or update something from the user's inventory
*
* always returns true or cuts the script off with a mysql error
*
*/

function UpdateOrDelete($id,$my_id)
{
	$id = FilterText($id);
	$type = FilterText($type);

	$check = mysql_query("SELECT amount FROM homes_inventory WHERE id = '".$id."' AND userid = '".$my_id."' LIMIT 1") or die(mysql_error());
	$exists = mysql_num_rows($check);

	if($exists > 0){
	$row = mysql_fetch_assoc($check);

		if($row['amount'] > 1){
			mysql_query("UPDATE homes_inventory SET amount = amount - 1 WHERE id = '".$id."' AND userid = '".$my_id."' LIMIT 1") or die(mysql_error());
		} else {
			mysql_query("DELETE FROM homes_inventory WHERE id = '".$id."' AND userid = '".$my_id."' LIMIT 1") or die(mysql_error());
		}

	}

	return true;
}

$mode = FilterText($_GET['key']);

if($mode == "inventory"){

// Look for the first inventory sticker in the DB for the header
$tmp = mysql_query("SELECT data FROM homes_inventory WHERE type = '1' AND userid = '".$my_id."' LIMIT 1");
$valid = mysql_num_rows($tmp);

if($valid > 0){
	$row = mysql_fetch_assoc($tmp);
	header("X-JSON: [[\"Inventario\",\"Catalogo\"],[\"" . formatThing(1,$row['data'],true) . "\",\"" . formatThing(1,$row['data'],false) . "\",\"".$row['data']."\",\"Sticker\",null,1]]");
} else {
	header("X-JSON: [[\"Inventario\",\"Catalogo\"],[]]");
}

?>
<div style="position: relative;">
<div id="webstore-categories-container">
	<h4>Categoria:</h4>
	<div id="webstore-categories">
<ul class="purchase-main-category">
		<li id="maincategory-1-stickers" class="selected-main-category webstore-selected-main">
			<div>Sticker</div>
			<ul class="purchase-subcategory-list" id="main-category-items-1">
				<?php $get_cc = mysql_query("SELECT * FROM homes_categories WHERE type = 1 ORDER BY minrank DESC,name") or die(mysql_error());
                                      while($row = mysql_fetch_assoc($get_cc)){
                           if($myrow['rank'] >= $row['minrank']){
			   echo"<li id=\"subcategory-".$row['type']."-".$row['id']."-stickers\" class=\"subcategory\">
					<div>".$row['name']."</div>
				</li>";
				} } ?>
				
			</ul>
		</li>
		<li id="maincategory-2-backgrounds" class="main-category">
			<div>Fondos</div>
			<ul class="purchase-subcategory-list" id="main-category-items-2">
				<?php $get_cc = mysql_query("SELECT * FROM homes_categories WHERE type = 2 ORDER BY name") or die(mysql_error());
                                      while($row = mysql_fetch_assoc($get_cc)){
                           if($myrow['rank'] >= $row['minrank']){
			   echo"<li id=\"subcategory-".$row['type']."-".$row['id']."-stickers\" class=\"subcategory\">
					<div>".$row['name']."</div>
				</li>";
				} } ?>
				
			</ul>
		</li>
		<li id="maincategory-6-stickie_notes" class="main-category-no-subcategories">
			<div>Noticia</div>
			<ul class="purchase-subcategory-list" id="main-category-items-6">
			<?php $get_cc = mysql_query("SELECT * FROM homes_categories WHERE type = 6 ORDER BY name") or die(mysql_error());
                                      while($row = mysql_fetch_assoc($get_cc)){
                           if($myrow['rank'] >= $row['minrank']){
			   echo"<li id=\"subcategory-".$row['type']."-".$row['id']."-stickie_notes\" class=\"subcategory\">
					<div>".$row['name']."</div>
				</li>";
                                } } ?>
			</ul>
		</li>
</ul>
	</div>
</div>

<div id="webstore-content-container">
	<div id="webstore-items-container">
		<h4>Selecciona un elemento haciendo click en el.</h4>
		<div id="webstore-items"><ul id="webstore-item-list">
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
</ul></div>
	</div>
	<div id="webstore-preview-container">
		<div id="webstore-preview-default"></div>
		<div id="webstore-preview"></div>
	</div>
</div>

<div id="inventory-categories-container">
	<h4>Categoria:</h4>
	<div id="inventory-categories">
<ul class="purchase-main-category">
	<li id="inv-cat-stickers" class="selected-main-category-no-subcategories">
		<div>Stickers</div>
	</li>
	<li id="inv-cat-backgrounds" class="main-category-no-subcategories">
		<div>Fondos</div>
	</li>
	<li id="inv-cat-widgets" class="main-category-no-subcategories">
		<div>Widgets</div>
	</li>
	<li id="inv-cat-notes" class="main-category-no-subcategories">
		<div>Notas</div>
	</li>
</ul>

	</div>
</div>

<div id="inventory-content-container">
	<div id="inventory-items-container">
		<h4>Selecciona un elemento haciendo click en el.</h4>
		<div id="inventory-items"><ul id="inventory-item-list">
<?php
	$get_em = mysql_query("SELECT * FROM homes_inventory WHERE userid = '".$my_id."' AND type = '1'") or die(mysql_error());
	$typ = "sticker";
	$number = mysql_num_rows($get_em);

	if($number < 1){
	echo "<div class=\"webstore-frank\">
	<div class=\"blackbubble\">
		<div class=\"blackbubble-body\">

<p><b>&iexcl;Esta categoria esta vacia!</b></p>
<p>Para comprar sticks, fondos, o notas, haga clic en la tienda y elege una categor&iacute;a y un producto. Luego haga clic en 'Comprar'.</p>
		
<div class=\"clear\"></div>
		</div>
	</div>
	<div class=\"blackbubble-bottom\">
		<div class=\"blackbubble-bottom-body\">
			<img src=\"./web-gallery/images/box-scale/bubble_tail_small.gif\" alt=\"\" width=\"12\" height=\"21\" class=\"invitation-tail\" />
		</div>
	</div>
	<div class=\"webstore-frank-image\"><img src=\"./web-gallery/images/frank/sorry.gif\" alt=\"\" width=\"57\" height=\"88\" /></div>
</div>";
	}

	while ($row = mysql_fetch_assoc($get_em)) {

	if($row['amount'] > 1){
		$specialcount = "<div class=\"webstore-item-count\"><div>x".$row['amount']."</div></div>";
	} else {
		$specialcount = "";
	}

	printf("	<li id=\"inventory-item-%s\" title=\"%s\">
		<div class=\"webstore-item-preview %s\">

			<div class=\"webstore-item-mask\">
				%s
			</div>
		</div>
	</li>", $row['id'], $row['data'], FormatThing($row['type'],$row['data'],true), $specialcount);
	}

	// We want at least 20 empty slots. If the user has less than 20 items for this type
	// we echo the necessary empty slots to fill it up to 20 slots using a for() loop.
	if($number < 20){
	$empty_slots = 20 - $number;
		for ($i = 1; $i <= $empty_slots; $i++) {
		echo "<li class=\"webstore-item-empty\"></li>";
		}
	}

?>
</ul></div>
	</div>
	<div id="inventory-preview-container">
		<div id="inventory-preview-default"></div>
		<div id="inventory-preview"><h4>&nbsp;</h4>

<div id="inventory-preview-box"></div>

<div id="inventory-preview-place" class="clearfix">
	<div class="clearfix">
		<a href="#" class="new-button" id="inventory-place"><b>Colocar</b><i></i></a>
	</div>
</div>

</div>
	</div>
</div>

<div id="webstore-close-container">
	<div class="clearfix"><a href="#" id="webstore-close" class="new-button"><b>Cerrar</b><i></i></a></div>
</div>
<?php } else if($mode == "inventory_items"){
$type = FilterText($_POST['type']);

	if($type == "stickers"){
	$get_em = mysql_query("SELECT * FROM homes_inventory WHERE userid = '".$my_id."' AND type = '1'") or die(mysql_error());
	$typ = "sticker";
	} else if($type == "notes"){
	$get_em = mysql_query("SELECT * FROM homes_inventory WHERE userid = '".$my_id."' AND type = '3'") or die(mysql_error());
	$typ = "note";
	} else if($type == "widgets"){
	$typ = "widget";
	} else if($type == "backgrounds"){
	$get_em = mysql_query("SELECT * FROM homes_inventory WHERE userid = '".$my_id."' AND type = '4'") or die(mysql_error());
	$typ = "background";
	} else {
	$get_em = mysql_query("SELECT * FROM homes_inventory WHERE userid = '".$my_id."' AND type = '1'") or die(mysql_error());
	$typ = "sticker";
	}

	if($typ !== "widget"){
		$number = mysql_num_rows($get_em);
		echo "		<ul id=\"webstore-item-list\">";

		if($number < 1){
			echo "<div class=\"webstore-frank\">
	<div class=\"blackbubble\">
		<div class=\"blackbubble-body\">

<p><b>&iexcl;Esta categoria esta vacia!</b></p>
<p>Para comprar sticks, fondos, o notas, haga clic en la tienda y elege una categor&iacute;a y un producto. Luego haga clic en 'Comprar'.</p>

		<div class=\"clear\"></div>
		</div>
	</div>
	<div class=\"blackbubble-bottom\">
		<div class=\"blackbubble-bottom-body\">
			<img src=\"./web-gallery/images/box-scale/bubble_tail_small.gif\" alt=\"\" width=\"12\" height=\"21\" class=\"invitation-tail\" />
		</div>
	</div>
	<div class=\"webstore-frank-image\"><img src=\"./web-gallery/images/frank/sorry.gif\" alt=\"\" width=\"57\" height=\"88\" /></div>
</div>";
	}

		while ($row = mysql_fetch_assoc($get_em)) {

		if($row['amount'] > 1){
			$specialcount = "<div class=\"webstore-item-count\"><div>x".$row['amount']."</div></div>";
		} else {
			$specialcount = "";
		}

		printf("	<li id=\"inventory-item-%s\" title=\"%s\">
		<div class=\"webstore-item-preview %s\">

			<div class=\"webstore-item-mask\">
				%s
			</div>
		</div>
	</li>", $row['id'], $row['data'], FormatThing($row['type'],$row['data'],true), $specialcount);
	}

	// We want at least 20 empty slots. If the user has less than 20 items for this type
	// we echo the necessary empty slots to fill it up to 20 slots using a for loop.
		if($number < 20){
		$empty_slots = 20 - $number;
			for ($i = 1; $i <= $empty_slots; $i++) {
			echo "<li class=\"webstore-item-empty\"></li>";
			}
		}

		echo "</ul>";
	} elseif($typ == "widget"){
		if($linked > 0){ // Group Mode
			$check = mysql_query("SELECT id FROM homes_stickers WHERE groupid = '".$groupid."' AND type = '2' AND subtype = '3' LIMIT 1") or die(mysql_error());
			$placed_memberwidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE groupid = '".$groupid."' AND type = '2' AND subtype = '4' LIMIT 1") or die(mysql_error());
			$placed_guestbookwidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE groupid = '".$groupid."' AND type = '2' AND subtype = '5' LIMIT 1") or die(mysql_error());
			$placed_traxwidget = mysql_num_rows($check);

			echo "<ul id=\"inventory-item-list\">";
			echo "<li id=\"inventory-item-p-3\"
		title=\"My Groups\" class=\"webstore-widget-item"; if($placed_memberwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_memberwidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Miembros del grupo</h3>
			<p>Muestra los miembros del grupo</p>
		</div>
	</li>";
	echo "<li id=\"inventory-item-p-4\"
		title=\"Guestbook\" class=\"webstore-widget-item"; if($placed_guestbookwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_guestbookwidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Libro de visitas</h3>
			<p>Para que tus colegas te dejen recuerdos</p>
		</div>
	</li>
	<li id=\"inventory-item-p-5\"
		title=\"Traxplayer\" class=\"webstore-widget-item" ; if($placed_traxwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_traxplayerwidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Traxplayer</h3>
			<p>Escucha tus canciones</p>
		</div>
	</li>";
			echo "</ul>";

		} else { // User profile
		$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '2' LIMIT 1") or die(mysql_error());
			$placed_groupwidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '3' LIMIT 1") or die(mysql_error());
			$placed_roomwidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '4' LIMIT 1") or die(mysql_error());
			$placed_guestbookwidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '5' LIMIT 1") or die(mysql_error());
			$placed_friendswidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '6' LIMIT 1") or die(mysql_error());
			$placed_traxwidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '7' LIMIT 1") or die(mysql_error());
			$placed_scoreswidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '8' LIMIT 1") or die(mysql_error());
			$placed_badgeswidget = mysql_num_rows($check);
			$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '2' AND subtype = '9' LIMIT 1") or die(mysql_error());
			$placed_ratingwidget = mysql_num_rows($check);

			echo "<ul id=\"inventory-item-list\">";
	echo "<li id=\"inventory-item-p-7\"
		title=\"Mis votos\" class=\"webstore-widget-item"; if($placed_scoreswidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_highscoreswidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Mis votos</h3>
			<p>&iquest;Tan popular eres?</p>
		</div>
	</li>
	<li id=\"inventory-item-p-2\"
		title=\"Mis grupos\" class=\"webstore-widget-item"; if($placed_groupwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_groupswidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Mis grupos</h3>
			<p>Haz que todos conozcan tus grupo</p>
		</div>
	</li>
	<li id=\"inventory-item-p-3\"
		title=\"Mis salas\" class=\"webstore-widget-item"; if($placed_roomwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_roomswidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Mis salas</h3>
			<p>Apareceran todas tus salas/p>
		</div>
	</li>
	<li id=\"inventory-item-p-4\"
		title=\"Libro de invitados\" class=\"webstore-widget-item"; if($placed_guestbookwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_guestbookwidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Libro de invitados</h3>
			<p>Todos podran escribir en el</p>
		</div>
	</li>
	<li id=\"inventory-item-p-5\"
		title=\"Mis amigos\" class=\"webstore-widget-item"; if($placed_friendswidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_friendswidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Mis amigos</h3>
			<p>Lista de todos tus amigos</p>
		</div>
	</li>
	<li id=\"inventory-item-p-6\"
		title=\"Traxplayer\" class=\"webstore-widget-item" ; if($placed_traxwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_traxplayerwidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Traxplayer</h3>
			<p>Para escuchar tu musica</p>
		</div>
	</li>
                  <li id=\"inventory-item-p-9\"
		title=\"Voting\" class=\"webstore-widget-item" ; if($placed_ratingwidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_ratingwidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Votos</h3>
			<p>&iquest;Cual es tu calificaci&oacute;n?</p>
		</div>
	</li>
	<li id=\"inventory-item-p-8\"
		title=\"Mis placas\" class=\"webstore-widget-item" ; if($placed_badgeswidget > 0){ echo " webstore-widget-disabled"; } echo "\">
		<div class=\"webstore-item-preview w_badgeswidget_pre\" >
			<div class=\"webstore-item-mask\">

			</div>
		</div>
		<div class=\"webstore-widget-description\">
			<h3>Mis placas</h3>
			<p>Muestra todas tus placas.</p>
		</div>
	</li>"; }
echo "</ul>";
		}
} elseif($mode == "main"){

// Look for the first thing in this category
$tmp = mysql_query("SELECT * FROM homes_catalouge WHERE category = '156' ORDER BY id ASC LIMIT 1");
$valid = mysql_num_rows($tmp);

if($valid > 0){
	$row = mysql_fetch_assoc($tmp);
	header("X-JSON: [[\"Inventario\",\"Catalogo\"],[{\"itemCount\":1,\"titleKey\":\"".$row['name']."\",\"previewCssClass\":\"".formatThing($row['type'],$row['data'],true)."\"}]]");
} else {
	header("X-JSON: [[\"Inventario\",\"Catalogo\"],[]]");
}

?>
<div style="position: relative;">
<div id="webstore-categories-container">
	<h4>Categoria:</h4>
	<div id="webstore-categories">
<ul class="purchase-main-category">
		<li id="maincategory-1-stickers" class="selected-main-category webstore-selected-main">
			<div>Sticker</div>
			<ul class="purchase-subcategory-list" id="main-category-items-1">
				<?php $get_cc = mysql_query("SELECT * FROM homes_categories WHERE type = 1 ORDER BY name") or die(mysql_error());
                                      while($row = mysql_fetch_assoc($get_cc)){
                           if($myrow['rank'] >= $row['minrank']){
			   echo"<li id=\"subcategory-".$row['type']."-".$row['id']."-stickers\" class=\"subcategory\">
					<div>".$row['name']."</div>
				</li>";
				} } ?>
				
			</ul>
		</li>
		<li id="maincategory-2-backgrounds" class="main-category">
			<div>Fondos</div>
			<ul class="purchase-subcategory-list" id="main-category-items-2">
				<?php $get_cc = mysql_query("SELECT * FROM homes_categories WHERE type = 2 ORDER BY name") or die(mysql_error());
                                      while($row = mysql_fetch_assoc($get_cc)){
                           if($myrow['rank'] >= $row['minrank']){
			   echo"<li id=\"subcategory-".$row['type']."-".$row['id']."-stickers\" class=\"subcategory\">
					<div>".$row['name']."</div>
				</li>";
				} } ?>
				
			</ul>
		</li>
		<li id="maincategory-6-stickie_notes" class="main-category-no-subcategories">
			<div>Notas</div>
			<ul class="purchase-subcategory-list" id="main-category-items-6">
			<?php $get_cc = mysql_query("SELECT * FROM homes_categories WHERE type = 6 ORDER BY name") or die(mysql_error());
                                      while($row = mysql_fetch_assoc($get_cc)){
                           if($myrow['rank'] >= $row['minrank']){
			   echo"<li id=\"subcategory-".$row['type']."-".$row['id']."-stickie_notes\" class=\"subcategory\">
					<div>".$row['name']."</div>
				</li>";
                                } } ?>
			</ul>
		</li>
</ul>

	</div>
</div>

<div id="webstore-content-container">
	<div id="webstore-items-container">
		<h4>Selecciona un elemento</h4>
		<div id="webstore-items">

<?php
	$category = "19";

	$get_em = mysql_query("SELECT * FROM homes_catalouge WHERE category = ".$category."") or die(mysql_error());
	$number = mysql_num_rows($get_em);

	echo "		<ul id=\"webstore-item-list\">";

	if($number < 1){
	echo "<div class=\"webstore-frank\">
	<div class=\"blackbubble\">
		<div class=\"blackbubble-body\">

<p><b>&iexcl;Esta categoria esta vac&iacute;a!</b></p>
<p>Lo sentimos, actualmente no hay elementos.</p>

		<div class=\"clear\"></div>
		</div>
	</div>
	<div class=\"blackbubble-bottom\">
		<div class=\"blackbubble-bottom-body\">
			<img src=\"./web-gallery/images/box-scale/bubble_tail_small.gif\" alt=\"\" width=\"12\" height=\"21\" class=\"invitation-tail\" />
		</div>
	</div>
	<div class=\"webstore-frank-image\"><img src=\"./web-gallery/images/frank/sorry.gif\" alt=\"\" width=\"57\" height=\"88\" /></div>
</div>";
	}

	while ($row = mysql_fetch_assoc($get_em)) {

	if($row['amount'] > 1){
		$specialcount = "<div class=\"webstore-item-count\"><div>x".$row['amount']."</div></div>";
	} else {
		$specialcount = "";
	}

	printf("	<li id=\"inventory-item-%s\" title=\"%s\">
		<div class=\"webstore-item-preview %s\">

			<div class=\"webstore-item-mask\">
				%s
			</div>
		</div>
	</li>", $row['id'], $row['data'], FormatThing($row['type'],$row['data'],true), $specialcount);
	}

	// We want at least 20 empty slots. If the user has less than 20 items for this type
	// we echo the necessary empty slots to fill it up to 20 slots using a for() loop.
	if($number < 20){
	$empty_slots = 20 - $number;
		for ($i = 1; $i <= $empty_slots; $i++) {
		echo "<li class=\"webstore-item-empty\"></li>";
		}
	}

	echo "</ul>";
?>

		</div>
	</div>
	<div id="webstore-preview-container">
		<div id="webstore-preview-default"></div>
		<div id="webstore-preview"><?php
$tmp = mysql_query("SELECT * FROM homes_catalouge WHERE category = '207'");
$row = mysql_fetch_assoc($tmp);

?>
<h4 title="<?php echo $row['name']; ?>"><?php echo $row['name']; ?></h4>

<div id="webstore-preview-box"></div>

<div id="webstore-preview-price">
Precio:<br /><b>
	<?php echo $row['price']; ?> creditos
</b>
</div>

<div id="webstore-preview-purse">
Tienes:<br /><b><?php echo $myrow['credits']; ?> creditos</b><br />
<?php if($myrow['credits'] < $row['cost']){ ?><span class="webstore-preview-error">&iexcl;No tienes suficientes creditos!</span><br />
<a href="credits.php" target=_blank>Comprar m&aacute;s</a><?php } ?>
</div>

<div id="webstore-preview-purchase" class="clearfix">
	<div class="clearfix">
		<a href="#" class="new-button <?php if($myrow['credits'] < $row['price']){ ?>disabled-button<?php } ?>" <?php if($myrow['credits'] < $row['price']){ ?>disabled="disabled"<?php } ?> id="webstore-purchase<?php if($myrow['credits'] < $row['price']){ ?>-disabled<?php } ?>"><b>Comprar</b><i></i></a>
	</div>
</div>

<span id="webstore-preview-bg-text" style="display: none">Vista previa</span>
</div>
	</div>
</div>

<div id="inventory-categories-container">
	<h4>Categoria:</h4>
	<div id="inventory-categories">
<ul class="purchase-main-category">
	<li id="inv-cat-stickers" class="selected-main-category-no-subcategories">
		<div>Stickers</div>
	</li>
	<li id="inv-cat-backgrounds" class="main-category-no-subcategories">
		<div>Fondos</div>
	</li>
	<li id="inv-cat-widgets" class="main-category-no-subcategories">
		<div>Widgets</div>
	</li>
	<li id="inv-cat-notes" class="main-category-no-subcategories">
		<div>Notas</div>
	</li>
</ul>

	</div>
</div>

<div id="inventory-content-container">
	<div id="inventory-items-container">
		<h4>Haz clic en un item:</h4>
		<div id="inventory-items"><ul id="inventory-item-list">
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
	<li class="webstore-item-empty"></li>
</ul></div>
	</div>
	<div id="inventory-preview-container">
		<div id="inventory-preview-default"></div>
		<div id="inventory-preview"><h4>&nbsp;</h4>

<div id="inventory-preview-box"></div>

<div id="inventory-preview-place" class="clearfix">
	<div class="clearfix">
		<a href="#" class="new-button" id="inventory-place"><b>Colocar</b><i></i></a>
	</div>
</div>

</div>
	</div>
</div>

<div id="webstore-close-container">
	<div class="clearfix"><a href="#" id="webstore-close" class="new-button"><b>Cerrar</b><i></i></a></div>
</div>
</div>
<?php
} elseif($mode == "preview"){

$productId = FilterText($_POST['productId']);
$subCategoryId = FilterText($_POST['subCategoryId']);

$tmp = mysql_query("SELECT * FROM homes_catalouge WHERE id = '".$productId."' AND category = '".$subCategoryId."' LIMIT 1");
$exists = mysql_num_rows($tmp);
$row = mysql_fetch_assoc($tmp);

if($row['type'] == "4"){
	$bg_pre = "\"bgCssClass\":\"" . FormatThing($row['type'],$row['data'],false) . "\",";
}

header("X-JSON: [{\"itemCount\":1,\"titleKey\":\"".$row['name']."\"," . $bg_pre . "\"previewCssClass\":\"" . FormatThing($row['type'],$row['data'],true) . "\"}]");

?>
<h4 title="<?php echo $row['name']; ?>"><?php echo $row['name']; ?></h4>

<div id="webstore-preview-box"></div>

<?php if($exists > 0){ ?><div id="webstore-preview-price">
Precio:<br /><b>
	<?php echo $row['price']; ?> creditos
</b>
</div>

<div id="webstore-preview-purse">
Tienes:<br /><b><?php echo $myrow['credits']; ?> creditos</b><br />
<?php if($myrow['credits'] < $row['price']){ ?><span class="webstore-preview-error">&iexcl;No tienes suficientes cr&eacute;ditos</span><br />
<a href="credits" target=_blank>Comprar m&aacute;s</a><?php } ?>
</div>

<div id="webstore-preview-purchase" class="clearfix">
	<div class="clearfix">
		<a href="#" class="new-button <?php if($myrow['credits'] < $row['price']){ ?>disabled-button<?php } ?>" <?php if($myrow['credits'] < $row['price']){ ?>disabled="disabled"<?php } ?> id="webstore-purchase<?php if($myrow['credits'] < $row['price']){ ?>-disabled<?php } ?>"><b>Comprar</b><i></i></a>
	</div>
</div><?php } ?>

<span id="webstore-preview-bg-text" style="display: none">Vista previa</span>
<?php
} elseif($mode == "purchase_confirm"){
$productId = FilterText($_POST['productId']);
$subCategoryId = FilterText($_POST['subCategoryId']);

$tmp = mysql_query("SELECT * FROM homes_catalouge WHERE id = '".$productId."' AND category = '".$subCategoryId."' LIMIT 1");
$exists = mysql_num_rows($tmp);
$row = mysql_fetch_assoc($tmp);

	if($exists > 0){
?>
		<div class="webstore-item-preview <?php echo formatThing($row['type'],$row['data'],true); ?>">
			<div class="webstore-item-mask">
			</div>
		</div>
		<p>&iquest;Estas seguro que quieres comprar un <b><?php echo $row['name']; ?></b>? Cuesta <b><?php echo $row['price']; ?></b> cr&eacute;ditos!</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>Cancelar</b><i></i></a>
		<a href="#" class="new-button" id="webstore-confirm-submit"><b>Comprar</b><i></i></a>
	</p>
	<div class="clear"></div>
<?php
	} else {
?>
		<p>&iexcl;No puedes comprar este articulo!</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
	<div class="clear"></div>
<?php
	}
} elseif($mode == "purchase_stickers"){
$productId = FilterText($_POST['selectedId']);

$tmp = mysql_query("SELECT * FROM homes_catalouge WHERE id = '".$productId."' LIMIT 1");
$exists = mysql_num_rows($tmp);
$row = mysql_fetch_assoc($tmp);

if($row['category'] == "50" && $user_rank < 6){ exit; }

	if($exists > 0){
		if($myrow['credits'] < $row['price']){
		?>
		<p>No tienes suficientes cr&eacute;ditos para comprar el item.</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
	<div class="clear"></div>
		<?php
		} else {
			mysql_query("UPDATE users SET credits = credits - ".$row['price']." WHERE id = '".$my_id."' LIMIT 1") or die(mysql_error());
			UpdateOrInsert($row['type'],$row['amount'],$row['data'],$my_id);
			mysql_query("INSERT INTO cms_transactions (userid,amount,date,descr) VALUES ('".$my_id."','-".$row['price']."','".$date_full."','Compra de Sticker para tu home o Grupo.')");
			echo "OK";
		}
	} else {
?>
		<p>No puedes comprar este item.</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
	<div class="clear"></div>
<?php
	}
} elseif($mode == "items"){

	$category = FilterText($_POST['subCategoryId']);

	if($category == "50" && $user_rank < 6){ exit; }

	$get_em = mysql_query("SELECT * FROM homes_catalouge WHERE category = ".$category."") or die(mysql_error());
	$number = mysql_num_rows($get_em);

	echo "		<ul id=\"webstore-item-list\">";

	if($number < 1){
	echo "<div class=\"webstore-frank\">
	<div class=\"blackbubble\">
		<div class=\"blackbubble-body\">

<p><b>Esta categoria esta vacia</b></p>
<p>Actualmente no hay ningun item</p>

		<div class=\"clear\"></div>
		</div>
	</div>
	<div class=\"blackbubble-bottom\">
		<div class=\"blackbubble-bottom-body\">
			<img src=\"./web-gallery/images/box-scale/bubble_tail_small.gif\" alt=\"\" width=\"12\" height=\"21\" class=\"invitation-tail\" />
		</div>
	</div>
	<div class=\"webstore-frank-image\"><img src=\"./web-gallery/images/frank/sorry.gif\" alt=\"\" width=\"57\" height=\"88\" /></div>
</div>";
	}

	while ($row = mysql_fetch_assoc($get_em)) {

	if($row['amount'] > 1){
		$specialcount = "<div class=\"webstore-item-count\"><div>x".$row['amount']."</div></div>";
	} else {
		$specialcount = "";
	}

	printf("	<li id=\"inventory-item-%s\" title=\"%s\">
		<div class=\"webstore-item-preview %s\">

			<div class=\"webstore-item-mask\">
				%s
			</div>
		</div>
	</li>", $row['id'], $row['data'], FormatThing($row['type'],$row['data'],true), $specialcount);
	}

	// We want at least 20 empty slots. If the user has less than 20 items for this type
	// we echo the necessary empty slots to fill it up to 20 slots using a for() loop.
	if($number < 20){
	$empty_slots = 20 - $number;
		for ($i = 1; $i <= $empty_slots; $i++) {
		echo "<li class=\"webstore-item-empty\"></li>";
		}
	}

	echo "</ul>";

} elseif($mode == "purchase_backgrounds"){
$productId = FilterText($_POST['selectedId']);

$tmp = mysql_query("SELECT * FROM homes_catalouge WHERE id = '".$productId."' LIMIT 1");
$exists = mysql_num_rows($tmp);
$row = mysql_fetch_assoc($tmp);

if($row['category'] == "50" && $user_rank < 6){ exit; }

	if($exists > 0){
		if($myrow['credits'] < $row['price']){
		?>
		<p>No tienes suficientes creditos para comprar este item.</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
	<div class="clear"></div>
		<?php
		} else {
			$tcheck = mysql_query("SELECT id FROM homes_inventory WHERE userid = '".$my_id."' AND type = '4' AND data = '".$row['data']."' LIMIT 1") or die(mysql_error());
			$tnum = mysql_num_rows($tcheck);
			if($tnum > 0){ ?>
		<p>&iexcl;Ya tienes esto en tu inventario!</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
			<?php } else {
				mysql_query("UPDATE users SET credits = credits - ".$row['price']." WHERE id = '".$my_id."' LIMIT 1") or die(mysql_error());
				UpdateOrInsert($row['type'],$row['amount'],$row['data'],$my_id);
				mysql_query("INSERT INTO cms_transactions (userid,amount,date,descr) VALUES ('".$my_id."','-".$row['price']."','".$date_full."','Webstore purchase')");
				echo "OK";
			}
		}
	} else {
?>
		<p>No puedes comprar este articulo</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
	<div class="clear"></div>
<?php
	}
} elseif($mode == "purchase_stickie_notes"){
$productId = FilterText($_POST['selectedId']); if(!is_numeric($productId)){ exit; }

$tmp = mysql_query("SELECT * FROM homes_catalouge WHERE id = '".$productId."' LIMIT 1");
$exists = mysql_num_rows($tmp);
$row = mysql_fetch_assoc($tmp);

if($row['category'] == "50" && $user_rank < 6){ exit; }

	if($exists > 0){
		if($myrow['credits'] < $row['price']){
		?>
		<p>No tienes suficientes creditos para comprar esto.</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
	<div class="clear"></div>
		<?php
		} else {
			mysql_query("UPDATE users SET credits = credits - ".$row['price']." WHERE id = '".$my_id."' LIMIT 1") or die(mysql_error());
			UpdateOrInsert($row['type'],$row['amount'],$row['data'],$my_id);
			mysql_query("INSERT INTO cms_transactions (userid,amount,date,descr) VALUES ('".$my_id."','-".$row['price']."','".$date_full."','Webstore')");
			echo "OK";
		}
	} else {
?>
		<p>No puedes comprar esto.</p>

	<p class="new-buttons">
		<a href="#" class="new-button" id="webstore-confirm-cancel"><b>OK</b><i></i></a>
	</p>
	<div class="clear"></div>
<?php
	}
} elseif($mode == "inventory_preview"){

if($_POST['type'] == "widgets"){
	$widget = FilterText($_POST['itemId']);
	if($widget == "2"){
		$row['data'] = "groupswidget";
	} elseif($widget == "3"){
		$row['data'] = "friends_preview";
	} else {
		$row['data'] = "profilewidget";
	}
	$row['type'] = 2;
	$exists = 1;
} else {
	$productId = FilterText($_POST['itemId']); if(!is_numeric($productId)){ exit; }
	$tmp = mysql_query("SELECT * FROM homes_inventory WHERE id = '".$productId."' AND userid = '".$my_id."' LIMIT 1");
	$exists = mysql_num_rows($tmp);
	$row = mysql_fetch_assoc($tmp);
}

header("X-JSON: [\"" . FormatThing($row['type'],$row['data'],true) . "\",\"" . FormatThing($row['type'],$row['data'],false) . "\",\"8\",\"".FilterText($_POST['type'])."\",null,".$row['amount']."]");

?>
<h4>&nbsp;</h4>

<div id="inventory-preview-box"></div>

<div id="inventory-preview-place" class="clearfix">
	<div class="clearfix">
		<a href="#" class="new-button" id="inventory-place"><b>Colocar</b><i></i></a>
	</div>
<?php if($row['amount'] > 1 && $row['type'] == "1"){ ?>
	<div class="clearfix">
		<a href="#" class="new-button" id="inventory-place-all"><b>Todos</b><i></i></a>
	</div>
<?php } ?>
</div>
<?php
} elseif($mode == "noteeditor"){
?>
<form action="#" method="post" id="webstore-notes-form">

<input type="hidden" name="maxlength" id="webstore-notes-maxlength" value="500" />

<div id="webstore-notes-counter"><?php echo 500 - strlen(HoloText($_POST['noteText'])); ?></div>

<p>
	<select id="webstore-notes-skin" name="skin">
			<option value="1" id="webstore-notes-skin-defaultskin">Default</option>
			<option value="6" id="webstore-notes-skin-goldenskin">Dorado</option>
	<?php if($myrow['hc_days'] > 0 or $myrow['hc2_days'] > 0){ ?>
			<option value="8" id="webstore-notes-skin-hc_pillowskin">HC para chicas</option>
			<option value="7" id="webstore-notes-skin-hc_machineskin">HC para chicos</option>
	<?php } if($user_rank > 0){ ?>
			<option value="9" id="webstore-notes-skin-nakedskin">Staff</option>
	<?php } ?>
			<option value="3" id="webstore-notes-skin-metalskin">Metalico</option>
			<option value="5" id="webstore-notes-skin-notepadskin">Notas</option>
			<option value="2" id="webstore-notes-skin-speechbubbleskin">Bocadillo</option>
			<option value="4" id="webstore-notes-skin-noteitskin">Bloc de  notas</option>
	</select>
</p>

<p class="warning">Una vez escrita no se puede editar.</p>

<div id="webstore-notes-edit-container">
<textarea id="webstore-notes-text" rows="7" cols="42" name="noteText"><?php echo HoloText($_POST['noteText']); ?></textarea>
    <script type="text/javascript">
        bbcodeToolbar = new Control.TextArea.ToolBar.BBCode("webstore-notes-text");
        bbcodeToolbar.toolbar.toolbar.id = "bbcode_toolbar";
        var colors = { "red" : ["#d80000", "Rojo"],
            "orange" : ["#fe6301", "Naranja"],
            "yellow" : ["#ffce00", "Amarillo"],
            "green" : ["#6cc800", "Verde"],
            "cyan" : ["#00c6c4", "Cyan"],
            "blue" : ["#0070d7", "Azul"],
            "gray" : ["#828282", "Gris"],
            "black" : ["#000000", "Negro"]
        };
        bbcodeToolbar.addColorSelect("Colores", colors, true);
    </script>


</form>

<p>
<a href="#" class="new-button" id="webstore-confirm-cancel"><b>Cancelar</b><i></i></a>
<a href="#" class="new-button" id="webstore-notes-continue"><b>Continuar</b><i></i></a>
</p>

<div class="clear"></div>
<?php
} elseif($mode == "noteeditor-preview"){
?>
<div id="webstore-notes-container">
<?php
if($user_rank < 1){ $text = FilterText($_POST['noteText']); } else { $text = FilterText($_POST['noteText']); }
$newskin = FilterText($_POST['skin']);

	if($newskin == 1){ $skin = "defaultskin"; }
	else if($newskin == 2){ $skin = "speechbubbleskin"; }
	else if($newskin == 3){ $skin = "metalskin"; }
	else if($newskin == 4){ $skin = "noteitskin"; }
	else if($newskin == 5){ $skin = "notepadskin"; }
	else if($newskin == 6){ $skin = "goldenskin"; }
	else { $skin = "defaultskin"; }

	echo "<div class=\"movable stickie n_skin_".$skin."-c\" style=\" left: 0px; top: 0px; z-index: 1;\" id=\"stickie--1\">
	<div class=\"n_skin_".$skin."\" >
		<div class=\"stickie-header\">
			<h3></h3>
			<div class=\"clear\"></div>
		</div>
		<div class=\"stickie-body\">
			<div class=\"stickie-content\">
				<div class=\"stickie-markup\">" . bbcode_format(nl2br(HoloText($text))) . "</div>
				<div class=\"stickie-footer\">
				</div>
			</div>
		</div>
	</div>
</div></div>";
?>
<p class="warning">Despues no puedes editar el texto.</p>

<p>
<a href="#" class="new-button" id="webstore-notes-edit"><b>Editar</b><i></i></a>
<a href="#" class="new-button" id="webstore-notes-add"><b>A&ntilde;adir</b><i></i></a>
</p>

<div class="clear"></div>
<?php
} elseif($mode == "noteeditor-place"){

	$data = FilterText($_POST['noteText']);
	$newskin = FilterText($_POST['skin']);

	if($newskin == 1){ $skin = "defaultskin"; }
	else if($newskin == 2){ $skin = "speechbubbleskin"; }
	else if($newskin == 3){ $skin = "metalskin"; }
	else if($newskin == 4){ $skin = "noteitskin"; }
	else if($newskin == 5){ $skin = "notepadskin"; }
	else if($newskin == 6){ $skin = "goldenskin"; }
	else { $skin = "defaultskin"; }

	if(strlen($data) < 501 && strlen($data) > 0){

	if($linked > 0){
		mysql_query("INSERT INTO homes_stickers (userid,groupid,x,y,z,data,type,subtype,skin) VALUES ('".$my_id."','".$groupid."','10','10','18','".FilterText($data)."','3','0','".$skin."')") or die(mysql_error());
		$sql = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '".$groupid."' AND type = '3' AND data = '".FilterText($data)."' ORDER BY id DESC LIMIT 1") or die(mysql_error());
		$sql2 = mysql_query("SELECT id FROM homes_inventory WHERE userid = '".$my_id."' AND type = '3' LIMIT 1") or die(mysql_error());
	} else {
		mysql_query("INSERT INTO homes_stickers (userid,groupid,x,y,z,data,type,subtype,skin) VALUES ('".$my_id."','-1','10','10','18','".FilterText($data)."','3','0','".$skin."')") or die(mysql_error());
		$sql = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '3' AND data = '".FilterText($data)."' ORDER BY id DESC LIMIT 1") or die(mysql_error());
		$sql2 = mysql_query("SELECT id FROM homes_inventory WHERE userid = '".$my_id."' AND type = '3' LIMIT 1") or die(mysql_error());
		}

		$row = mysql_fetch_assoc($sql);
		$row2 = mysql_fetch_assoc($sql2);

		UpdateOrDelete($row2['id'],$my_id);

		$id = $row['id'];
		header("X-JSON: " . $id );

		$edit = "\n<img src=\"./web-gallery/images/myhabbo/icon_edit.gif\" width=\"19\" height=\"18\" class=\"edit-button\" id=\"stickie-" . $id . "-edit\" />
<script language=\"JavaScript\" type=\"text/javascript\">
Event.observe(\"stickie-" . $id . "-edit\", \"click\", function(e) { openEditMenu(e, " . $id . ", \"stickie\", \"stickie-" . $id . "-edit\"); }, false);
</script>\n";

		echo "<div class=\"movable stickie n_skin_".$skin."-c\" style=\" left: 0px; top: 0px; z-index: 1;\" id=\"stickie-" . $id . "\">
	<div class=\"n_skin_".$skin."\" >
		<div class=\"stickie-header\">
			<h3>".$edit."</h3>
			<div class=\"clear\"></div>
		</div>
		<div class=\"stickie-body\">
			<div class=\"stickie-content\">
				<div class=\"stickie-markup\">" . bbcode_format(nl2br(HoloText($data))) . "</div>
				<div class=\"stickie-footer\">
				</div>
			</div>
		</div>
	</div>
</div></div>";

	}
} elseif($mode == "place_sticker"){

$id = FilterText($_POST['selectedStickerId']);
$z = FilterText($_POST['zindex']);
$placeAll = FilterText($_POST['placeAll']);

$check = mysql_query("SELECT data,amount FROM homes_inventory WHERE userid = '".$my_id."' AND type = '1' AND id = '".$id."' LIMIT 1") or die(mysql_error());
$exists = mysql_num_rows($check);

	if($exists > 0){
		$row = mysql_fetch_assoc($check);

		if($placeAll == "true"){
			$amount = $row['amount'];
		} else {
			$amount = 1;
		}

		$header_pack = "X-JSON: [";

		for ($i = 1; $i <= $amount; $i++) {

			if($linked > 0){
				mysql_query("INSERT INTO homes_stickers (userid,groupid,x,y,z,type,subtype,data,skin) VALUES ('".$my_id."','".$groupid."','10','10','".$z."','1','0','".$row['data']."','')") or die(mysql_error());
				$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '".$groupid."' AND type = '1' AND data = '".$row['data']."' ORDER BY id DESC LIMIT 1") or die(mysql_error());
			} else {
				mysql_query("INSERT INTO homes_stickers (userid,groupid,x,y,z,type,subtype,data,skin) VALUES ('".$my_id."','-1','10','10','".$z."','1','0','".$row['data']."','')") or die(mysql_error());
				$check = mysql_query("SELECT id FROM homes_stickers WHERE userid = '".$my_id."' AND groupid = '-1' AND type = '1' AND data = '".$row['data']."' ORDER BY id DESC LIMIT 1") or die(mysql_error());
			}
			
			$assoc = mysql_fetch_assoc($check);
			$edit = "\n<img src=\"./web-gallery/images/myhabbo/icon_edit.gif\" width=\"19\" height=\"18\" class=\"edit-button\" id=\"sticker-" . $assoc['id'] . "-edit\" />
<script language=\"JavaScript\" type=\"text/javascript\">
Event.observe(\"sticker-" . $assoc['id'] . "-edit\", \"click\", function(e) { openEditMenu(e, " . $assoc['id'] . ", \"sticker\", \"sticker-" . $assoc['id'] . "-edit\"); }, false);
</script>\n";
			$sticker_pack = $sticker_pack . "<div class=\"movable sticker s_" . $row['data'] . "\" style=\"left: 10px; top: 10px; z-index: " . $z . "\" id=\"sticker-" . $assoc['id'] . "\">\n" . $edit . "\n</div>\n";
			if($i == 1){ // X-JSON: [1
				$header_pack = $header_pack . $assoc['id'];
			} else { // X-JSON [1,2
				$header_pack = $header_pack . "," . $assoc['id'];
			}
		}

		$header_pack = $header_pack . "]";

		if($placeAll == "true"){
			mysql_query("DELETE FROM homes_inventory WHERE userid = '".$my_id."' AND id = '".$id."' AND type = '1' LIMIT 1");
		} else {
			UpdateOrDelete($id,$my_id);
		}

		header($header_pack);
		echo $sticker_pack;

 	}

} elseif($mode == "background_warning"){
?>
<p>
La imagen que has seleccionado permanecer&aacute; como Fondo de tu P&aacute;gina hasta que selecciones otra imagen o cierres el Cat&aacute;logo. Si quieres conservarla como Fondo, tendr&aacute;s que comprarla y seleccionarla en tu Inventario.
</p>

<p>
<a href="#" class="new-button" id="webstore-warning-ok"><b>OK</b><i></i></a>
</p>

<div class="clear"></div>
<?php
} else {
//echo "<b>Error:</b> Unknown mode " . $mode . ".";
header("Location: index.php");
}
?>