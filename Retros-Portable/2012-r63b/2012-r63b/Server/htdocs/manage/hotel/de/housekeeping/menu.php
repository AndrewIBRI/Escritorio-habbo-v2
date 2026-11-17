<?php if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }

if($tab == "3"){ ?>

<div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Configura&ccedil;&atilde;o</div></div>
  
<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/banners' style='text-decoration:none'><?php if($pageid == "banners"){ ?><b>Banners</b><?php }else{ ?>Banners<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/settings' style='text-decoration:none'><?php if($pageid == "settings"){ ?><b>Configura&ccedil;&atilde;o</b><?php }else{ ?>Configura&ccedil;&atilde;o<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/groups' style='text-decoration:none'><?php if($pageid == "groups"){ ?><b>Grupos</b><?php }else{ ?>Grupos<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/news' style='text-decoration:none'><?php if($pageid == "news"){ ?><b>Not&iacute;cias</b><?php }else{ ?>Not&iacute;cias<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/campaigns' style='text-decoration:none'><?php if($pageid == "campaigns"){ ?><b>Campanhas</b><?php }else{ ?>Campanhas<?php } ?></a>
</div>


<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/recommended' style='text-decoration:none'><?php if($pageid == "recommended"){ ?><b>Recomendado pelos staffs</b><?php }else{ ?>Recomendado pelos staffs<?php } ?></a>
</div>

</div>
<br />

 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Mercado</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/marktplatz' style='text-decoration:none'><?php if($pageid == "marktplatz"){ ?><b>Editar</b><?php }else{ ?>Editar<?php } ?></a>
</div>

</div>
<br>

 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Loja (Homes e grupos)</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/webstore_item' style='text-decoration:none'><?php if($pageid == "webstore_item"){ ?><b>Editar itens</b><?php }else{ ?>Editar itens<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/webstore_catalog' style='text-decoration:none'><?php if($pageid == "webstore_catalog"){ ?><b>Editar cat&aacute;logo</b><?php }else{ ?>Editar cat&aacute;logo<?php } ?></a>
</div>

</div>
<br />

<?php } if($tab == "4"){ ?>

 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Quartos</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/room_guestrooms' style='text-decoration:none'>Salas <img src="./images/<?php if($pageid == "room_guestroom"){ echo'check'; }else{ echo'del'; } ?>.gif" align="right"></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/room_officialrooms' style='text-decoration:none'>Quartos oficiais <img src="./images/<?php if($pageid == "room_officialroom"){ echo'check'; }else{ echo'del'; } ?>.gif" align="right"></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/room_categories' style='text-decoration:none'>Categorias <img src="./images/<?php if($pageid == "room_categories"){ echo'check'; }else{ echo'del'; } ?>.gif" align="right"></a>
</div>

</div>
<br>

<?php } if($tab == "5"){ ?>

 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Gest&atilde; de usu&aacute;rios</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/badgetool' style='text-decoration:none'><?php if($pageid == "badgetool"){ ?><b>Placas</b><?php }else{ ?>Placas<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/cloner' style='text-decoration:none'><?php if($pageid == "cloner"){ ?><b>Revisara clones</b><?php }else{ ?>Revisar clones<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/massa' style='text-decoration:none'><?php if($pageid == "massa"){ ?><b>Enviar masa</b><?php }else{ ?>Enviar masa<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/users' style='text-decoration:none'><?php if($pageid == "users"){ ?><b>Usuarios</b><?php }else{ ?>Usuarios<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/users_edit' style='text-decoration:none'><?php if($pageid == "users_edit"){ ?><b>Editar usuarios</b><?php }else{ ?>Editar usuarios<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/passwordtool' style='text-decoration:none'><?php if($pageid == "passwordtool"){ ?><b>Contrase&ntilde;a</b><?php }else{ ?>Contrase&ntilde;a<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/ranktool' style='text-decoration:none'><?php if($pageid == "ranktool"){ ?><b>Rangos</b><?php }else{ ?>Rangos<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/vouchers' style='text-decoration:none'><?php if($pageid == "vouchers"){ ?><b>Vouchers</b><?php }else{ ?>Vouchers<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/transactions' style='text-decoration:none'><?php if($pageid == "transactions"){ ?><b>Transacciones</b><?php }else{ ?>Transacciones<?php } ?></a>
</div>

</div>
<br />


 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Editar VIP</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/viptool' style='text-decoration:none'><?php if($pageid == "viptool"){ ?><b>Herramienta VIP</b><?php }else{ ?>Herramienta VIP<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/viptool_bestellung' style='text-decoration:none'><?php if($pageid == "viptool_bestellung"){ ?><b>Pedidos VIP</b><?php }else{ ?>Pedidos VIP<?php } ?></a>
</div>

</div>
<br />


<div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> MOD-Tool</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/ban' style='text-decoration:none'><?php if($pageid == "ban"){ ?><b>Baneos</b><?php }else{ ?>Baneos<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/unban' style='text-decoration:none'><?php if($pageid == "unban"){ ?><b>Desbanear</b><?php }else{ ?>Desbanear<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/helper' style='text-decoration:none'><?php if($pageid == "helper"){ ?><b>Llamadas de ayuda</b><?php }else{ ?>Llamadas de ayuda<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/alert' style='text-decoration:none'><?php if($pageid == "alert"){ ?><b>Alertas de la CMS</b><?php }else{ ?>Alertas de la CMS<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/minimail' style='text-decoration:none'><?php if($pageid == "minimail"){ ?><b>Minimail</b><?php }else{ ?>Minimail<?php } ?></a>
</div>

</div>
<br />


 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Registros y listas</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/stafflogs' style='text-decoration:none'><?php if($pageid == "stafflogs"){ ?><b>HK-Stafflogs</b><?php }else{ ?>HK-Stafflogs<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/banlogs' style='text-decoration:none'><?php if($pageid == "banlogs"){ ?><b>Logs de baneos</b><?php }else{ ?>Logs de baneos<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/alertlogs' style='text-decoration:none'><?php if($pageid == "alertlogs"){ ?><b>CMS Alert-Logs</b><?php }else{ ?>CMS Alert-Logs<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/chatlogs' style='text-decoration:none'><?php if($pageid == "chatlogs"){ ?><b>Chatlogs</b><?php }else{ ?>Chatlogs<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/onlinelogs' style='text-decoration:none'><?php if($pageid == "onlinelogs"){ ?><b>Onlinelogs</b><?php }else{ ?>OnlineLogs<?php } ?></a>
</div>

</div>
<br />


<?php } ?>
