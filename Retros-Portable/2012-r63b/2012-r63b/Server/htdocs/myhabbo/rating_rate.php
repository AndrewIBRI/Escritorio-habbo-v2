<?php

if($bypass !== true){

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php');

$ownerid = FilterText($_GET['ownerId']);
$widgetid = FilterText($_GET['ratingId']);
$rate = FilterText($_GET['givenRate']);
}

if(is_numeric($ownerid) && is_numeric($widgetid) && is_numeric($rate)){
	$myvote = mysql_evaluate("SELECT COUNT(*) FROM homes_ratings WHERE raterid = '".$my_id."' AND userid = '".$ownerid."'");
	if($myvote < 1 && $ownerid != $user->id && $rate > 0 && $rate < 6){
		mysql_query("INSERT INTO homes_ratings (userid,rating,raterid) VALUES ('".$ownerid."','".$rate."','".$my_id."')");
	}
}

$totalvotes = mysql_evaluate("SELECT COUNT(*) FROM homes_ratings WHERE userid = '".$ownerid."'");
$highvotes = mysql_evaluate("SELECT COUNT(*) FROM homes_ratings WHERE userid = '".$ownerid."' AND rating > 3");
$votestally = mysql_evaluate("SELECT SUM(rating) FROM homes_ratings WHERE userid = '".$ownerid."'");

$x = $totalvotes;
if($x == 0){ $x = 1; }
$average = round($votestally / $x, 1);
$px = ceil(($average * 150) / 5);

?>
<script type="text/javascript">	
	var ratingWidget;
	 
		ratingWidget = new RatingWidget(<?php echo HoloText($ownerid); ?>, <?php echo HoloText($widgetid); ?>);
	 
</script><div class="rating-average">
		<b>Media de votos: <?php echo $average; ?></b><br/>
	<div id="rating-stars" class="rating-stars" >
				<ul id="rating-unit_ul1" class="rating-unit-rating">
				<li class="rating-current-rating" style="width:<?php echo $px; ?>px;" />
	
			</ul>	
	</div>
	<?php echo $totalvotes; ?> votos em total
	
	<br/>
	(<?php echo $highvotes; ?> <?php echo $shortname; ?>'s votaram 4 ou mais)
</div>