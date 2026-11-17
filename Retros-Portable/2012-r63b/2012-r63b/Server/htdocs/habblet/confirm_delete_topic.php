<?php

require_once('../data_classes/server-data.php_data_classes-core.php.php');
require_once('../data_classes/server-data.php_data_classes-session.php.php'); 

if($user_rank < 6){ exit; }

?>

<p>Vas a eliminar este tema. &iquest;Estas seguro?</p>

<p>
<a href="#" class="new-button" id="discussion-action-cancel"><b>Cancelar</b><i></i></a>
<a href="#" class="new-button" id="discussion-action-ok"><b>Continuar</b><i></i></a>
</p>

<div class="clear"></div>