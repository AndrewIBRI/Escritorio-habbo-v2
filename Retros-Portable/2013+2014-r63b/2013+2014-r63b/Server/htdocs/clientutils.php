<?php
#####################################################################
#||||| 		Project MyHobba desenvolvido por @rtiag0		   |||||#
#||||| Criado por @rtiag0 - HTML, PHP, CSS & Data_classes      |||||#
# ----------------------------------------------------------------- #
#||||| © Copyright 2012. Hobba 2011 - Open Source Software     |||||#
#||||| © Copyright 2012. LavvosCMS - DATA_CLASSES Estrutura	   |||||#
#||||| © Copyright 2012. Sulake Corporation - Habbo Arquivos   |||||#
#####################################################################

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename ="Error";
require_once('./templates/client_subheader.php'); 

?>

<body id="popup" class="process-template client_error">
<div id="container">
<div id="content">
<div id="process-content" class="centered-client-error">

<div id="column1" class="column">
<div class="habblet-container ">		
<div class="habblet-container ">		
						<div class="cbb clearfix orange ">
	
							<h2 class="title">Opa!!
							</h2>
						<script type="text/javascript">
    if (typeof HabboClient != "") {
        HabboClient.forceReload = true;
    }
</script>

<div class="box-content">
    <div class="info-client_error-text">
       <p>Opa, foi encontrado um problema técnico, mas não se preocupe. Este erro será investigado por nossa equipe de suporte.</p>
       <p>Entre novamente no <a onclick="openOrFocusHabbo(this); return true;" target="" href="/client">Hotel</a> para continuar. Desculpe-nos o incoveniente.</p>
    </div>
    <div class="retry-enter-hotel">
    <div class="hotel-open">
        <a id="enter-hotel-open-image" class="open" href="client" target="" onclick="HabboClient.openOrFocus(this); return true;">
        <div class="hotel-open-image-splash"></div>
        <div class="hotel-image hotel-open-image"></div>
        </a>
        <div class="hotel-open-button-content">
          <a class="open" href="client" target="" onclick="HabboClient.openOrFocus(this); return true;">Entre no Hotel</a>
            <span class="open"></span>
        </div>
    </div>
    </div>
</div>


<script type="text/javascript">
document.observe("dom:loaded", function() {
    var titles = $$("h2.title");
    if (titles.length > 0) {
        Element.insert(titles[0], "(#01087836) ");
    }
});
</script>

</div>
<script type="text/javascript">
  document.observe("dom:loaded", function() {
    ClientMessageHandler.googleEvent("client_error", "unknown");
  });
</script>
	
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

</div>
<script type="text/javascript">
HabboView.run();
</script>
<div id="column2" class="column">
</div>
<!--[if lt IE 7]>
<script type="text/javascript">
Pngfix.doPngImageFix();
</script>
<![endif]-->
</div></div>

</body>
</html>