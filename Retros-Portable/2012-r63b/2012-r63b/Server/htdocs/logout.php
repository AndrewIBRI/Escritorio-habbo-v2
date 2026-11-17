<?php

if(isset($login_error) && $ban_check > 0){
	session_start();
} else {
	require_once('./data_classes/server-data.php_data_classes-core.php.php');
	require_once('./data_classes/server-data.php_data_classes-session.php.php');
	session_start();
}
	setcookie("remember", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
	setcookie("rusername", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
	setcookie("rpassword", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");

require_once('./templates/login_subheader.php');
require_once('./templates/login_header.php');

if(isset($login_error) && $ban > 0){

?>

<div id="process-content">

<div class="action-error flash-message">
<div class="rounded">

	<b><?php echo $login_error; ?></b>

</div>
</div>

<div style="text-align: center">
	
<div style="width:100px; margin: 10px auto"><a href="<?php echo $path; ?>" id="logout-ok" class="new-button fill"><b>OK</b><i></i></a></div>

<div id="column1" class="column"></div>
<div id="column2" class="column"></div>

</div>

<?php }elseif(session_is_registered(username)){

session_destroy();

		if($_COOKIE['remember'] == "remember"){
		setcookie("remember", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
		setcookie("rusername", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
		setcookie("rpassword", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
		}

?>

<div id="process-content">
<div class="action-confirmation flash-message">
<div class="rounded">

	<b>Você vai tão cedo? Fique mais tempo com seus amigos em <?php echo $shortname; ?>! Você não pode?
Volte em breve, estaremos esperando aqui!
</b>

</div>
</div>

<div style="text-align: center">
	
<div style="width:100px; margin: 10px auto"><a href="<?php echo $path; ?>" id="logout-ok" class="new-button fill"><b>OK</b><i></i></a></div>

<div id="column1" class="column"></div>
<div id="column2" class="column"></div>


</div>

<?php } require_once('./templates/logout.php'); ?>