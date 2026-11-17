<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = $shortname . " Club";
$pageid = "6";
$body_id = "home";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>
<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column7" class="column">
<div class="habblet-container ">		
<div class="cbb clearfix hcred ">
	<h2 class="title">Junte-se ao <?php echo $shortname; ?> Club</h2>

<div id="hc-habblet box-content">
<div id="hc-buy-container">
<div id="hc-buy-buttons" class="hc-buy-buttons">

<form class="subscribe-form" method="post">

<div style="float: left; margin: 0px;">
<div class="cbb habboclub-buyentry">

	<h2 class="title" style="background-color: #90a7b7;">HC ou VIP?</h2>

<div style="padding: 10px 0px 0px 10px;">
<?php if($hc > 0){ ?>
       HC: <b><?php echo getHCdays($my_id); ?></b> d&iacute;as
<?php }else{ ?>
Sem inscri&ccedil;&otilde;es ativas.
<?php } ?><br>
</div>

<div style="height: 113px; padding: 10px">HC or VIP? HC d&aacute; a voc&ecirc; mais visuais, novas cores e Mobi todo m&ecirc;s. VIP d&aacute; a voc&ecirc; tudo o que voc&ecirc; sempre sonhou num clube. </div>
</div> 
</div> 
   
<div style="float: left;">
<div class="cbb habboclub-buyentry hcbasic">
     
<h2 class="title" style="background-color: #9b9448;"><img style="float: left;" alt="hc" src="./web-gallery/v2/images/habboclub/habboclub_basic_small.png" />1 m&ecirc;s</h2>   
            
<div style="height: 55px;">
 <img style="position: relative; left: 10px; top: 10px;" alt="credits" src="../web-gallery/v2/images/newcredits/credit_in_white_bg.png" />   
 <span style="position: relative; left: -32px; top: -3px; font-size:1.5em; font-weight:bold; color: #000000;" class="habboclub-offerprice">
 -/-</span>           
 <a class="new-button oversize" style="position: relative; left: -10px; top: 10px; margin-left: 0px" href="#" onclick="habboclub.buttonClick(1,"Confirmar suscripci&oacute;n"); return false;"'><b>Comprar</b><i></i></a>
 <div style="width: 10px;"></div>
</div> 
</div> 

<div class="cbb habboclub-buyentry hcbasic">

<h2 class="title" style="background-color: #9b9448;"><img style="float: left;" alt="hc" src="./web-gallery/v2/images/habboclub/habboclub_basic_small.png" />3 meses</h2>   

<div style="height: 55px;">
<img style="position: relative; left: 10px; top: 10px;" alt="credits" src="../web-gallery/v2/images/newcredits/credit_in_white_bg.png" />   
<span style="position: relative; left: -32px; top: -3px; font-size:1.5em; font-weight:bold; color: #000000;" class="habboclub-offerprice">
-/-</span>           
<a class="new-button oversize" style="position: relative; left: -10px; top: 10px; margin-left: 0px" id="subscribe2" href="#" onclick='habboclub.buttonClick(2, "Confirmar suscripción"); return false;'><b>Comprar</b><i></i></a>
<div style="width: 10px;"></div>
</div> 
</div> 
  
</div>   
  
<div style="float: left;"> 
<div class="cbb habboclub-buyentry hcvip">

     
      <h2 class="title" style="background-color: #969696;">      
        <img style="float: left;" alt="vip" src="./web-gallery/v2/images/habboclub/habboclub_vip_small.png" />
        1 m&ecirc;s   
      </h2>
            
    <div style="height: 55px;">
     <img style="position: relative; left: 10px; top: 10px;" alt="credits" src="../web-gallery/v2/images/newcredits/credit_in_white_bg.png" />   
     <span style="position: relative; left: -32px; top: -3px; font-size:1.5em; font-weight:bold; color: #000000;" class="habboclub-offerprice">
	-/-</span>           
     <a class="new-button oversize" style="position: relative; left: -10px; top: 10px; margin-left: 0px" id="subscribe3" href="#" onclick='habboclub.buttonClick(3, "Confirmar suscripci&oacute;n"); return false;'><b>Comprar</b><i></i></a>
     <div style="width: 10px;"></div>
    </div> 
    
   </div> 
  
       
     <div class="cbb habboclub-buyentry hcvip">

     
      <h2 class="title" style="background-color: #969696;">      
        <img style="float: left;" alt="vip" src="./web-gallery/v2/images/habboclub/habboclub_vip_small.png" />
        3 meses   
      </h2>
            
    <div style="height: 55px;">
     <img style="position: relative; left: 10px; top: 10px;" alt="credits" src="../web-gallery/v2/images/newcredits/credit_in_white_bg.png" />   
     <span style="position: relative; left: -32px; top: -3px; font-size:1.5em; font-weight:bold; color: #000000;" class="habboclub-offerprice">-/-</span>           
     <a class="new-button oversize" style="position: relative; left: -10px; top: 10px; margin-left: 0px" id="subscribe4" href="#" onclick='habboclub.buttonClick(4, "Confirmar suscripci&oacute;n"); return false;'><b>Comprar</b><i></i></a>
     <div style="width: 10px;"></div>
    </div> 
    
   </div> 
  
  </div> 
 
</form>

</div> 
</div>
</div>
	
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
			 

			     		
<div class="habblet-container ">		
	<div class="cbb clearfix hcred ">
	
	<h2 class="title">Compare os benef&iacute;cios</h2>
	<div id="habboclub-info" class="box-content" style="position: relative; top: 3px; left: -11px">
 <table cellspacing="0" cellpadding="0">

  <tr>
   <td valign="top">
  <div class="cbb hcnone habboclub-infoentry" style="height: 214px;">
   <h2 class="title" style="height: 53px; background-color: #90a7b7;">
    <span style="position: relative; top: 18px; font-weight: bold">QUALQUER &Eacute;GRATIS</span>
   </h2> 
   <div style="height: 3px"></div>
   <div class="rounded" style="background-color: #ffffff;">
    Look B&aacute;sico
   </div>

   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/clothes_b.png" />
  </div> 
  
  <div class="cbb hcnone habboclub-infoentry" style="height: 83px;">
   <div class="rounded" style="background-color: #ffffff;">
    Cores b&aacute;sicas
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/colors_b.png" />
  </div> 
  
  <div class="cbb hcnone habboclub-infoentry" style="height: 101px;">
   <div class="rounded" style="background-color: #ffffff;"><span id="result_box3" lang="pt" xml:lang="pt">Vestu&aacute;rio e / ou cores de cabelo que voc&ecirc; pode misturar duas</span></div>
   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/multicolor_b.png" />
  </div> 
  
  <div class="cbb hcnone habboclub-infoentry" style="height: 185px;">   
  </div> 
  
  <div class="cbb hcnone habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">
    Lista de Amigos con capacidad para 200
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/friends_b.png" />

  </div>
  
  <div class="cbb hcnone habboclub-infoentry" style="height: 136px;">   
  </div>  
  
  <div class="cbb hcnone habboclub-infoentry" style="height: 75px;">   
   <div class="rounded" style="background-color: #ffffff;">
    12 desenhos de Quarto
   </div>
  </div> 
  
  <div class="cbb hcnone habboclub-infoentry" style="height: 65px;">   
  </div>  
  
  <div class="cbb hcnone habboclub-infoentry" >   
   <div class="rounded" style="background-color: #ffffff;">
    1 baile
   </div>

  </div>  
  
  <div class="cbb hcnone habboclub-infoentry" >   
   <div class="rounded" style="background-color: #ffffff;">
    Ofertas na feira livre
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/coin_offers.png" />
   <div style="position: relative; top: 13px; left: -2px">
    = 5 ofertas x 1 Cr&eacute;dito
   </div>
  </div>  
  
  </td><td valign="top">

  
  <div class="cbb hcbasic habboclub-infoentry">
   <h2 class="title" style="height: 53px; background-color: #9b9448;">
    <img style="position: relative; top: 5px" alt="xx" src="./web-gallery/v2/images/habboclub/habboclub_basic_big.png" />
   </h2> 
   <div style="height: 3px"></div>
   <div class="rounded" style="background-color: #ffffff;">
    Looks HC
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/clothes_hc.png" />
  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry" style="height: 83px;">

   <div class="rounded" style="background-color: #ffffff;">
    Cores HC
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/colors_hc.png" />
  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry" style="height: 101px;">
   <div class="rounded" style="background-color: #ffffff;"><span id="result_box" lang="pt" xml:lang="pt">Vestu&aacute;rio e / ou cores de cabelo que voc&ecirc; pode misturar duas</span></div>
   <img style="float: left; padding: 10px;" alt="xx" src="./web-gallery/v2/images/habboclub/multicolor_hc.png" />

  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">Arm&aacute;rio para memorizar 5 Visuais </div>
  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry" style="height: 135px;">
   <div class="rounded" style="background-color: #ffffff;">
    Um Mobi HC por m&ecirc;s, HC exclusivos</div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/furni_hc.png" />

  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">
    Lista de Amigos con capacidade para 600
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/friends_hc.png" />
  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">
    Emblema HC
   </div>

   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/badge_hc.png" />
  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry" >
   <div class="rounded" style="background-color: #ffffff;"><span class="rounded" style="background-color: #ffffff;">Prioridade para entrar em Quartos</span></div>
  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry" style="height: 75px;">   
   <div class="rounded" style="background-color: #ffffff;">
    + 8 dise&ntilde;os HC
   </div>

   <div style="padding: 10px"><span id="result_box2" lang="pt" xml:lang="pt">Quartos com escadas</span></div>
  </div>  
  
  <div class="cbb hcbasic habboclub-infoentry">   
   <div class="rounded" style="background-color: #ffffff;">
    Comandos
   no chat:</div>
   <div style="padding: 5px;">
    <b>:furni</b> - mostra os mobis<br/>

    <b>:chooser</b> - mostra os usu&aacute;rios
   </div>
  </div> 
  
  <div class="cbb hcbasic habboclub-infoentry" >   
   <div class="rounded" style="background-color: #ffffff;">
    4 Bailes HC
   </div>
  </div>   
  
  <div class="cbb hcbasic habboclub-infoentry" >   
   <div class="rounded" style="background-color: #ffffff;">
    Ofertas na feira livre
   </div>

   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/coin_offers.png" />
   <div style="position: relative; top: 13px; left: -2px">
    = 5 ofertas x 1 Cr&eacute;dito
   </div>
  </div>   
  
  </td><td valign="top">
 
  <div class="cbb hcvip habboclub-infoentry">
   <h2 class="title" style="height: 53px; background-color: #969696;">
    <img style="position: relative; top: 5px" alt="xx" src="../web-gallery/v2/images/habboclub/habboclub_vip_big.png" />
   </h2> 
   <div style="height: 3px"></div>

   <div class="rounded" style="background-color: #ffffff;">
    Looks HC + VIP
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/clothes_vip.png" />
  </div> 
  
  <div class="cbb hcvip habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">
    Cores HC + VIP
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/colors_vip.png" />

  </div> 
  
  <div class="cbb hcvip habboclub-infoentry" style="height: 101px;">
   <div class="rounded" style="background-color: #ffffff;">
     <div id="gt-src-c">
       <div id="gt-src-p">
         <div id="gt-src-wrap">
           <div></div>
           <div id="gt-src-tools">
             <div id="gt-src-tools-l"></div>
             <div id="gt-src-tools-r"></div>
           </div>
         </div>
       </div>
     </div>
     <div id="gt-res-c">
       <div id="gt-res-p">
         <div id="gt-res-data">
           <div id="gt-tgt-lang-sugg"></div>
           <div id="gt-res-wrap">
             <div id="gt-res-content">
               <div dir="ltr">
                 <div id="tts_button">
                   <object type="application/x-shockwave-flash" data="http://www.gstatic.com/translate/sound_player2.swf" id="tts" height="18" width="18">
                   </object>
                 </div>
                 Roupas coloridas para combinar e muito mais! </div>
             </div>
           </div>
         </div>
       </div>
     </div>
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/multicolor.png" />
  </div> 
  
  <div class="cbb hcvip habboclub-infoentry" >
   <div class="rounded" style="background-color: #ffffff;">Arm&aacute;rio para memorizar 10 Visuais</div>

  </div> 
  
  <div class="cbb hcvip habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">1 Presente por m&ecirc;s das edi&ccedil;&otilde;es exclusivas HC + VIP</div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/furni_vip.png" />
  </div> 
  
   <div class="cbb hcvip habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">1.100 amigos no Console</div>

   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/friends_vip.png" />
  </div> 
  
  <div class="cbb hcvip habboclub-infoentry">
   <div class="rounded" style="background-color: #ffffff;">HC + Emblema VIP </div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/badge_vip.png" />
  </div>
  
  <div class="cbb hcvip habboclub-infoentry" >
   <div class="rounded" style="background-color: #ffffff;">

    Prioridade para entrar em Quartos
   </div>
  </div> 
  
  <div class="cbb hcvip habboclub-infoentry">   
   <div class="rounded" style="background-color: #ffffff;">
    + 8 desenhos HC + 6 desenhos VIP
   </div>
   <div style="padding: 10px">
     Quartos com escadas<br/>
     Quartos sem paredes
   </div>

  </div>   
  
  <div class="cbb hcvip habboclub-infoentry">   
   <div class="rounded" style="background-color: #ffffff;">
    Comandos
   </div>
   <div style="padding: 5px;">
    <b>:furni</b> - mostra os mobis<br/>
    <b>:chooser</b> - mostra os usu&aacute;rios
   </div>

  </div>   
  
  <div class="cbb hcvip habboclub-infoentry" >   
   <div class="rounded" style="background-color: #ffffff;">
    4 Bailes HC
   </div>
  </div>  
  
  <div class="cbb hcvip habboclub-infoentry" >   
   <div class="rounded" style="background-color: #ffffff;">
    Ofertas na feira livre
   </div>
   <img style="float: left; padding: 10px;" alt="xx" src="../web-gallery/v2/images/habboclub/coin_offers.png" />
   <div style="position: relative; top: 13px; left: -2px">

    = 10 ofertas x 1 Cr&eacute;dito
   </div>
  </div>  
  
   </td> 
  </tr>
 </table>
</div>
	
						
</div>
				</div>
				<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>
</div>
</div>
<div id="container2">
  <div id="content2" style="position: relative" class="clearfix">
    <?php require_once('./templates/community_footer.php'); ?>
  </div>
</div>
