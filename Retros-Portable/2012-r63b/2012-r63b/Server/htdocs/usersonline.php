<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');


$pagename = "Usuarios Online";
$pageid = "600";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="content"> 
<div id="column1" class="column">

<div class="habblet-container ">        
<div class="cbb clearfix blue ">
<h2 class="title"><?php echo $shortname; ?>'s Online <span style="float: right; font-weight: normal; font-size: 75%;">(<?php echo $online_count; ?> Online)</h2>


<?php 

$i = 0;

$onlineu = mysql_query("SELECT * FROM users WHERE online = '1' ORDER BY last_online DESC"); 
while($usero = mysql_fetch_assoc($onlineu)){

$i++;

        if(IsEven($i)){
            $even = "odd";
        } else {
            $even = "even";
        }

?>

<li class="<?php echo $even; ?>">
<p><img align="left" src="<?php echo $avatar; echo $usero['look']; ?>&action=std,wav,&head_direction=3&gesture=sml"><br>
<p><a href="../home/<?php echo $usero['username']; ?>"><b><?php echo $usero['username']; ?></b></a> &nbsp;
<b style="font-size: 110%;"><?php if($usero['online'] >= 1){ ?><img src="../web-gallery/v2/images/online.gif"><?php }else{ ?><img src="../web-gallery/v2/images/offline.gif"><?php } ?></b><br />
<i><span style=" : 0px 0px 7px blue"><span style="color: black" size:="" 2=""><?php echo $usero['motto']; ?></span></i><br><br>
<span style=" : 0px 0px 7px green"><span style="color: black" size:="" 2=""><b>&Uacute;ltima conexi&oacute;n: </b></span></span><i><span style=" : 0px 0px 7px red"><span style="color: blue" size:="" 2=""><?php echo date('d-m-Y - H:i:s', $usero['last_online']); ?></span></i><br>
<br><br><br>
<?php } ?><br>
</table>
	

</div></div>

                <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>