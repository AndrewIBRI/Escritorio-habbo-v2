<?php
require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./templates/community_subheader.php');

?>

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/common.css" type="text/css" />
<div style="width:500px; margin: 0 auto; text-align: left" id="content">

<div class="habblet-container">		

	<div class="cb clearfix blue"><div class="bt"><div></div></div><div class="i1"><div class="i2"><div class="i3">

		<div class="rounded-container"><div style="background-color: rgb(255, 255, 255); "><div style="margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(111, 153, 196); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(53, 113, 173); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(53, 113, 173); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(111, 153, 196); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(53, 113, 173); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div></div><h2 class="title rounded-done">Bem-vindo ao <?php echo $shortname ?> Hotel!</h2><div style="background-color: rgb(255, 255, 255); "><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(53, 113, 173); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(111, 153, 196); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div><div style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(53, 113, 173); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div></div><div style="margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; height: 1px; overflow-x: hidden; overflow-y: hidden; background-color: rgb(255, 255, 255); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(111, 153, 196); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(53, 113, 173); "><div style="height: 1px; overflow-x: hidden; overflow-y: hidden; margin-top: 0px; margin-right: 1px; margin-bottom: 0px; margin-left: 1px; background-color: rgb(39, 103, 167); "></div></div></div></div></div></div>
<?php
if(!empty($_GET['roomId'])){
$ROOMSQL = mysql_query("SELECT * FROM rooms WHERE id = '".$_GET['roomId']."'");
$ROOMSQL_RESULT = mysql_fetch_assoc($ROOMSQL);
$ROOMSQL_RESULTNUM = mysql_num_rows($ROOMSQL);
}
if($ROOMSQL_RESULTNUM > 0){
?>
<div class="habblet box-content">
<img src="<?php echo $path; ?>/web-gallery/v2/images/facebook/app_habbo_hotel_image.png" style="float: left; margin-right: 20px; width: 77px; height: 77px">
<div id="fb-like-room-details" style="float: left; width: 300px">
<h4 style="margin: 0; padding: 0">Nome do Quarto</h4><?php echo $ROOMSQL_RESULT['caption']; ?>
<h4 style="margin: 10px 0 0 0; padding: 0">Proprietário</h4><?php echo $ROOMSQL_RESULT['owner']; ?>
<h4 style="margin: 10px 0 0 0; padding: 0">Descrição</h4>
<em style="color: #999"><?php if(!empty($ROOMSQL_RESULT['description'])){echo $ROOMSQL_RESULT['description']; }else{echo"Sem descrisao";} ?></em>
<p style="margin-top: 20px" class="clearfix">
<a href="<?php echo $path; ?>/client.php?forwardId=1&roomId=<?php echo $ROOMSQL_RESULT['id']; ?>" id="enter-room" style="" class="btn-gloss left" onclick="HabboClient.openOrFocus(this); return false;"><b>Clique aqui para entrar no Quarto!</b><i></i></a>
</p><div id="room-opened-notice" style="display: none; padding: 10px; border: #5b5 2px solid; ">Hey <?php echo $shortname ?>, você já esta nesta sala!</div>
<p></p>
</div>			
</div>
<?php }else{ ?>
							
<?php } ?>
	</div></div></div><div class="bb"><div></div></div></div>

</div>

</div>