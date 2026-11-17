<?php
$user = "root";
$pass = "1034@nb";
$db = "habbo";
$host = "localhost";
 
$link = mysqli_connect($host, $user, $pass, $db) or die("Error " . mysqli_error($link));
$query = "SELECT * FROM furniture";
$result = $link->query($query);
 
$appendstring = "INSERT IGNORE INTO `items_base` (`item_id`, `sprite_id`, `public_name`, `item_name`, `type`, `width`, `length`, `height`, `allow_stack`, `allow_walk`, `allow_sit`, `allow_recycle`, `allow_trade`, `allow_marketplace_sell`, `allow_gift`, `allow_inventory_stack`, `interaction_type`, `cycle_count`, `vending_ids`) VALUES ";
 
while($row = mysqli_fetch_array($result)) {
  $itemid =  str_replace("'","", $row["id"]);
  $name = str_replace("'","", $row["public_name"]);
  $itemname = str_replace("'","", $row["item_name"]);
  $type = str_replace("'","", $row["type"]);
  $spriteid = str_replace("'","", $row["sprite_id"]);
  $width = str_replace("'","", $row["width"]);
  $length = str_replace("'","", $row["length"]);
  $height = str_replace("'","", $row["stack_height"]);
  $stack = str_replace("'","", $row["can_stack"]);
  $walk = str_replace("'","", $row["is_walkable"]);
  $sit = str_replace("'","", $row["can_sit"]);
  $recycle = str_replace("'","", $row["allow_recycle"]);
  $trade = str_replace("'","", $row["allow_trade"]);
  $sell = str_replace("'","", $row["allow_marketplace_sell"]);
  $gift = str_replace("'","", $row["allow_gift"]);
  $stack = str_replace("'","", $row["allow_inventory_stack"]);
  $cycle = str_replace("'","", $row["interaction_modes_count"]);
  $interaction = str_replace("'","", $row["interaction_type"]);
  $vendid = str_replace("'","", $row["vending_ids"]);
 
  $appendstring = $appendstring ."('" . $itemid . "', '" . $spriteid . "', '" . $name . "', '" . $itemname . "', '" . $type . "','" . $width . "','" . $length . "','" . $height . "','" . $stack . "','" . $walk . "','" . $sit ."','" . $recycle . "','" . $trade . "','" . $marketplace . "','" . $gift . "','" . $stack . "','" . $interaction . "','" . $cycle . "','" . $vendid . "'),</br>";
}
 
echo substr_replace($appendstring, ";", -6);
?>