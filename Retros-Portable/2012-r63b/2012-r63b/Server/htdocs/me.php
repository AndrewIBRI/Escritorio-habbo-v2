<?php 

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

if($_GET['do'] == "RemoveFeedItem" && is_numeric($_GET['key'])){
    mysql_query("DELETE FROM cms_alerts WHERE userid = '".$my_id."' AND id = '".FilterText($_GET['key'])."' ORDER BY id ASC LIMIT 1") or die(mysql_error());
}

$pagename = $name;
$pageid = "1";

$messages = mysql_query("SELECT COUNT(*) FROM cms_minimail WHERE receiver_id = '".$my_id."'") or $messages = 0;
header("X-JSON: {\"totalMessages\":".$messages."}");

include('templates/community_subheader.php');
include('templates/community_header.php');

$fetch_tags = mysql_query("SELECT tag,id FROM user_tags WHERE user_id = '".$my_id."' LIMIT 20") or die(mysql_error());
$tags_num = mysql_num_rows($fetch_tags);

$randomq[] = "&iquest;Qui&eacute;n es tu actor favorito?";
$randomq[] = "&iquest;Prefieres deporte o siesta?";
$randomq[] = "&iquest;Tienes un sobrenombre?";
$randomq[] = "&iquest;Cu&aacute;l es tu canci&oacute;n favorita?";
$randomq[] = "&iquest;Cu&aacute;l es tu &eacute;poca del a&ntilde;o favorita?";
$randomq[] = "&iquest;Cu&aacute;l es tu banda favorita?";
$randomq[] = "&iquest;No puedes vivir sin qu&eacute; programa de televisi&oacute;n? ";
$randomq[] = "&iquest;Qu&eacute; instrumento te gustar&iacute;a tocar?";
$randomq[] = "Elige: &iquest;Pizza? &iquest;Hamburguesa? &iquest;Hot-Dog?";
$randomq[] = "El Deporte que m&aacute;s te gusta";
$randomq[] = "Para salir de fiesta vistes con...";
$randomq[] = "&iquest;Cu&aacute;l es tu pel&iacute;cula favorita?";
$randomq[] = "Perros, Gatos o algo m&aacute;s ex&oacute;tico.";
$randomq[] = "&iquest;Rojo? &iquest;Azul? &iquest;Rosa? u otro color.";
$randomq[] = "Elige: &iquest;MCR? &iquest;Panda? &iquest;Jonas Brothers?";
$randomq[] = "Elige: &iquest;Rock? &iquest;Pop? &iquest;Rap?";
$randomq[] = "&iquest;Qu&eacute; palabra te define?";
$randomq[] = "Elige &iquest;Habbo? &iquest;".$shortname."?";
$randomq[] = "&iquest;Cu&aacute;l es tu Staff favorito?";
$randomq[] = "&iquest;Postre o Jam&oacute;n?";
$randomq[] = "&iquest;Qu&eacute; otro idioma hablas?";
$randomq[] = "Lo mejor del mundo es...";
$randomq[] = "&iquest;Qu&eacute; furni te gusta m&aacute;s?";
$randomq[] = "Elige: &iquest;Laptop? &iquest;Escritorio?";

srand ((double) microtime() * 1000000);
$chosen = rand(0,count($randomq)-1);

$tag_question = $randomq[$chosen];

$get_flashclient = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_flashclient' AND value = '1'");

///////////////////////////////////////////////////// ALEJO.MASTER97 ////////////////////////////////////////////////////////
if($vip == 1){

//Sacar ID de Usuario
$sql   = "SELECT id FROM users WHERE username='".$name."'";
$result= mysql_query($sql);
$row 	 = mysql_fetch_assoc($result); 

//Coger ID
$userid = $row[id];

//Sacar tiempo de expiración Vip
$sql   = "SELECT timestampend FROM vip WHERE id_user=$userid ORDER BY timestampend DESC LIMIT 1";
$result= mysql_query($sql);
$row 	 = mysql_fetch_assoc($result); 

//Tiempo para finalizar
$expiretime = $row[timestampend];

//Sacar días restantes
$daysLeft = 0;
if ( $expiretime>time() ){
  $daysLeft = $expiretime - time();
  $daysLeft = round($daysLeft/86400);
} 

//Si Expira el tiempo el vip será quitado
if($expiretime < time()){
  $sql   = "UPDATE users SET vip='0' WHERE id='$my_id'";
  mysql_query($sql);

//Degradamos rango
  $sql   = "UPDATE users SET rank='1' WHERE id='$my_id'";
  mysql_query($sql);

//Alertamos al Usuario
  $sql   = "INSERT INTO cms_alerts (userid,alert) VALUES ('".$my_id."','Por favor note que a sua subscrição terminou Vip.')";
  mysql_query($sql);

//Eliminar de tabla vip
  $sql   = "DELETE FROM vip WHERE id_user=$userid";
  mysql_query($sql);
} } 
///////////////////////////////////////////////////// ALEJO.MASTER97 ////////////////////////////////////////////////////////

?>

<link rel="stylesheet" href="./web-gallery/static/styles/lightweightmepage.css" type="text/css" />
<script src="./web-gallery/static/js/lightweightmepage.js" type="text/javascript"></script>

<div id="container">
<div id="content" style="position: relative" class="clearfix">

<?PHP
$rand =rand(0,100);
if($rand < 85){
//PUBLI EDU
?>


<?PHP
} else {
//PUBLI HECTOR
?>


<?PHP
}
?>

<div id="wide-personal-info">

    <div id="habbo-plate">
	<a href="<?php echo $path; ?>/profile">
	<img alt="<?php echo $name; ?>" src="<?php echo $avatar; echo $myrow['look']; ?>&size=b&action=wav&direction=2&head_direction=3&gesture=sml&size=s" width="64" height="110"/>
</a>
    </div>

    <div id="name-box" class="info-box">
        <div class="label">Nome</div>
        <div class="content"><?php echo $name; ?></div>
    </div>
    <div id="motto-box" class="info-box">
        <div class="label">Missão:</div>
        <div class="content"><?php echo $myrow['motto']; ?></div>
    </div>
    <div id="last-logged-in-box" class="info-box">
        <div class="label">Última conexão:</div>
        <div class="content"><?php echo date('d-m-Y H:i:s', $myrow['last_online']); echo""; ?> </div>
    </div>

<div class="enter-hotel-btn">
    <div class="open enter-btn">
            <a href="/client" target="01080cec2ad93526df3c8baaeb130bf04025261d" onclick="HabboClient.openOrFocus(this); return false;">Entrar no <?php echo $shortname; ?> Hotel<i></i></a>
        <b></b>
    </div>
</div>

</div>



<div id="column1" class="column">

				<div class="habblet-container ">		
	    
						<div id="twitterfeed-habblet-container">
    <div id="twitterfeed-habblet-content"></div>
        <script>new TWTR.Widget({
                    version: 2,
                    id: 'twitterfeed-habblet-content',
                    type: 'profile',
                    rpp: 5,
                    interval: 30000,
                    width: 453,
                    height: 163,
                    theme: {
                        shell: {
                            background: '#4a4d4f',
                            color: '#ffffff'
                        },
                        tweets: {
                            background: '#ffffff',
                            color: '#4a4d4f',
                            links: '#fe6301'
                        }
                    },
                    features: {
                        scrollbar: true,
                        loop: false,
                        live: false,
                        behavior: 'default'
                    } }).render().setUser('twitter').start();
        </script>
</div>					


    <div class="habblet-container ">
<div class=" "> 


                     
                                                           

    </div>
                                           
                                            </div>

<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

					
				</div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<?php
// #########################################################################
// Minimail
// #########################################################################
?>
			     		
<div class="habblet-container minimail" id="mail">		
<div class="cbb clearfix blue ">


<div id="minimail">
<div class="minimail-contents">



</div>

<div id="message-compose-wait"></div>
<form style="display: none" id="message-compose">
<div>Para</div>
<div id="message-recipients-container" class="input-text" style="width: 426px; margin-bottom: 1em">
<input type="text" value="" id="message-recipients" />
<div class="autocomplete" id="message-recipients-auto">
<div class="default" style="display: none;">Escreva o nome do seu amigo</div>
<ul class="feed" style="display: none;"></ul>
</div>
</div>

	<div>Asunto<br/>
	        <input type="text" style="margin: 5px 0" id="message-subject" class="message-text" maxlength="100" tabindex="2" />
	        </div>
	        <div>Mensaje<br/>
	        <textarea style="margin: 5px 0" rows="5" cols="10" id="message-body" class="message-text" tabindex="3"></textarea>

	        </div>
	        <div class="new-buttons clearfix">
	            <a href="#" class="new-button preview"><b>Previa</b><i></i></a>
	            <a href="#" class="new-button send"><b>Enviar</b><i></i></a>
	        </div>
	    </form>
	</div>
		
	</div>
    <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>



</br>

</center>

					
                  

</div>

</div>


<?php
// #########################################################################
// News
// #########################################################################

require_once('./data_classes/server-data.php_data_classes-news.php.php');

// #########################################################################
// Seleção Staff
// #########################################################################
?>

							
<div class="habblet-container ">		
<div class="cbb clearfix red ">
<div class="box-tabs-container clearfix">

<h2>Seleção Staff</h2>

<ul class="box-tabs">
        <li id="tab-1-3-1"><a href="#">Salas</a><span class="tab-spacer"></span></li>
        <li id="tab-1-3-2" class="selected"><a href="#">Grupos</a><span class="tab-spacer"></span></li>
</ul>

</div>

<div id="tab-1-3-1-content"  style="display: none">
<div id="staffpicks-rooms-habblet-list-container" class="habblet-list-container groups-list">

<ul class="habblet-list">

<?php $sql = mysql_query("SELECT * FROM cms_recommended WHERE type = 'room' ORDER BY id ASC") or die(mysql_error());
	while($row = mysql_fetch_assoc($sql)) {
	$i++;
		$roomsql = mysql_query("SELECT * FROM rooms WHERE id = '".$row['id_rec']."' LIMIT 1");
		$roomrow = mysql_fetch_assoc($roomsql);

	if(IsEven($i)){
            $even = "even left";
        } else {
            $even = "even2";
        }


	$roomcount = $roomrow['users_now'] / $roomrow['users_max'] * 100;
	if($roomcount == 99 || $roomcount > 99){
		$rl = 5;
	} elseif($roomcount > 65){
		$rl = 4;
	} elseif($roomcount > 32){
		$rl = 3;
	} elseif($roomcount > 0){
		$rl = 2;
	} elseif($roomcount < 1){
		$rl = 1;
	}

	$getOwner = mysql_query("SELECT * FROM users WHERE id = '".$roomrow['owner']."'");
	$owner = mysql_fetch_assoc($getOwner);

	?>

	<li class="<?php echo $even; ?> room-occupancy-<?php echo $rl; ?>" roomid="<?php echo $roomrow['id']; ?>">
            <div>
                <span class="room-name"><a href="../client?forwardId=2&amp;roomId=<?php echo $roomrow['id']; ?>" onclick="HabboClient.roomForward(this, '<?php echo $roomrow['id']; ?>', '<?php echo $roomrow['roomtype']; ?>'); return false;" target="85d327955e5fc804ac33675ef8d05e3c9d3ef7ed"><?php echo $roomrow['caption']; ?></a></span>
                <span class="room-owner"><a href="/home/<?php echo $owner['username']; ?>"><?php echo $owner['username']; ?></a></span>
                <p><?php echo $roomrow['description']; ?></p>

            </div>
        </li>

<?php } ?>
    </ul>
</div>
</div>


<div id="tab-1-3-2-content" >
<div id="staffpicks-groups-habblet-list-container" class="habblet-list-container groups-list">

    <ul class="habblet-list two-cols">

	<?php $sql = mysql_query("SELECT * FROM cms_recommended WHERE type = 'group' ORDER BY id ASC") or die(mysql_error());
	while($row = mysql_fetch_assoc($sql)) {

	$i++;
	
		$groupsql = mysql_query("SELECT * FROM group_details WHERE id = '".$row['id_rec']."' LIMIT 1");
		$grouprow = mysql_fetch_assoc($groupsql);

        if(IsEven($i)){
            $even = "even left";
        } else {
            $even = "even right";
        }
    ?>
 <li class="<?php echo $even; ?>" style="background-image: url(<?php echo $path; ?>/habbo-imaging/badge.php?badge=<?php echo $grouprow['badge']; ?>)">
        <?php if($grouprow['roomid'] !== 0) { ?><a href="<?php echo $path; ?>/client?forwardId=2&amp;roomId=<?php echo $grouprow['roomid']; ?>" onclick="HabboClient.roomForward(this, '<?php echo $grouprow['roomid']; ?>', 'private'); return false;" target="9cf76b1ff4b99b02656456ee94e0271ab56272ee" class="group-room"></a><?php } ?>
            <a class="item" href="./groups/<?php echo $grouprow['id']; ?>"><?php echo HoloText($grouprow['name']); ?></a>
            </li>
            <?php } ?>
    </ul>
</div>
</div>
</div>
</div>
    
                        
                    
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>




<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<script type="text/javascript">
    HabboView.add(LoginFormUI.init);
</script>
<div id="column3" class="column">
</div>
</div>
<?php require_once('./templates/community_footer2.php'); ?>