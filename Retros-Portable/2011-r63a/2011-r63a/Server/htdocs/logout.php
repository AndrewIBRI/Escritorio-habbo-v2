<?php
@session_start();
if(isset($login_error) && $ban_check > 0){
	session_start();
} else {
	@require_once('./data_classes/server-data.php_data_classes-core.php.php');
	@require_once('./data_classes/server-data.php_data_classes-session.php.php');
	
}
	@setcookie("remember", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
	@setcookie("rusername", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
	@setcookie("rpassword", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");

require_once('./templates/login_subheader.php');
require_once('./templates/login_header.php');

if(isset($login_error) && $ban > 0){

?>

<div id="process-content">

<div class="action-error flash-message">
<div class="rounded">

	<b>Você foi banido. O motivo é: "<?php echo $bandata['reason'] ?>" seu banimento acabará em: <?php echo date('d/m/Y H:i:s', $bandata['expire']) ?></b>

</div>
</div>

<div style="text-align: center">

<div style="width:100px; margin: 10px auto"><a href="<?php echo $path; ?>" id="logout-ok" class="new-button fill"><b>OK</b><i></i></a></div>

<div id="column1" class="column"></div>
<div id="column2" class="column"></div>

</div>

<?php }elseif(@session_is_registered(username)){

@session_destroy();

		if(@$_COOKIE['remember'] == "remember"){
		@setcookie("remember", "", time()-60*60*24*100, "/"); @setcookie("cookpass", "", time()-60*60*24*100, "/");
		@setcookie("rusername", "", time()-60*60*24*100, "/"); @setcookie("cookpass", "", time()-60*60*24*100, "/");
		@setcookie("rpassword", "", time()-60*60*24*100, "/"); @setcookie("cookpass", "", time()-60*60*24*100, "/");
		}

?>

<div id="process-content">
<div class="action-confirmation flash-message">
<div class="rounded">

	<b>Você saiu do <?php echo $shortname; ?> Hotel corretamente. Nós esperamos que você tenha se divertido!</b>

</div>
</div>

<div style="text-align: center">
<div style="width:100px; margin: 10px auto"><a href="<?php echo $path; ?>" id="logout-ok" class="new-button fill"><b>OK</b><i></i></a></div>

<div id="column1" class="column"></div>
<div id="column2" class="column"></div>

</div>

<?php } require_once('./templates/login_footer.php'); ?>