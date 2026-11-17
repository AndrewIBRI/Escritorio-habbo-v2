<?php if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }

if($tab == "3"){ ?>

<div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'>Configurações</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/groups' style='text-decoration:none'><?php if($pageid == "groups"){ ?><b>Grupos</b><?php }else{ ?>Grupos<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/news' style='text-decoration:none'><?php if($pageid == "news"){ ?><b>Notícias</b><?php }else{ ?>Notícias<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/campaigns' style='text-decoration:none'><?php if($pageid == "campaigns"){ ?><b>Campanhas</b><?php }else{ ?>Campanhas<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/recommended' style='text-decoration:none'><?php if($pageid == "recommended"){ ?><b>Recomendado pela Equipe</b><?php }else{ ?>Recomendado pela Equipe<?php } ?></a>
</div>

</div>
<br />

</div>
<br>

 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Loja (Homes e Grupos)</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/webstore_item' style='text-decoration:none'><?php if($pageid == "webstore_item"){ ?><b>Editar Items</b><?php }else{ ?>Editar Items<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/webstore_catalog' style='text-decoration:none'><?php if($pageid == "webstore_catalog"){ ?><b>Editar Cat&aacute;logo</b><?php }else{ ?>Editar Cat&aacute;logo<?php } ?></a>
</div>

</div>
<br />

<?php } if($tab == "4"){ ?>

 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Quartos</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/room_guestrooms' style='text-decoration:none'>Quartos <img src="./images/<?php if($pageid == "room_guestroom"){ echo'check'; }else{ echo'del'; } ?>.gif" align="right"></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/room_officialrooms' style='text-decoration:none'>Quatos oficiais <img src="./images/<?php if($pageid == "room_officialroom"){ echo'check'; }else{ echo'del'; } ?>.gif" align="right"></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/room_categories' style='text-decoration:none'>Categorias <img src="./images/<?php if($pageid == "room_categories"){ echo'check'; }else{ echo'del'; } ?>.gif" align="right"></a>
</div>

</div>
<br>

</div>
<br>

<?php } if($tab == "5"){ ?>

 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Gestão de Usuários</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/badgetool' style='text-decoration:none'><?php if($pageid == "badgetool"){ ?><b>Emblemas</b><?php }else{ ?>Emblemas<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/cloner' style='text-decoration:none'><?php if($pageid == "cloner"){ ?><b>Fakes</b><?php }else{ ?>Fakes<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/massa' style='text-decoration:none'><?php if($pageid == "massa"){ ?><b>Moedas/Emblemas</b><?php }else{ ?>Moedas/Emblemas<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/users' style='text-decoration:none'><?php if($pageid == "users"){ ?><b>Usuários do Hotel</b><?php }else{ ?>Usuários do Hotel<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/users_edit' style='text-decoration:none'><?php if($pageid == "users_edit"){ ?><b>Procurar usuários</b><?php }else{ ?>Procurar usuários<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/passwordtool' style='text-decoration:none'><?php if($pageid == "passwordtool"){ ?><b>Senha</b><?php }else{ ?>Senha<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/ranktool' style='text-decoration:none'><?php if($pageid == "ranktool"){ ?><b>Cargos</b><?php }else{ ?>Cargos<?php } ?></a>
</div>

<br />


 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Ferramentas VIP</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/viptool' style='text-decoration:none'><?php if($pageid == "viptool"){ ?><b>Usuários VIP</b><?php }else{ ?>Usuários VIP<?php } ?></a>
</div>
</div>
<br />


<div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Moderação</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/ban' style='text-decoration:none'><?php if($pageid == "ban"){ ?><b>Banimento</b><?php }else{ ?>Banimento<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/unban' style='text-decoration:none'><?php if($pageid == "unban"){ ?><b>Desbanir</b><?php }else{ ?>Desbanir<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/helper' style='text-decoration:none'><?php if($pageid == "helper"){ ?><b>Pedidos de ajuda</b><?php }else{ ?>Pedidos de ajuda<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/alert' style='text-decoration:none'><?php if($pageid == "alert"){ ?><b>Alertar Usuário (Site)</b><?php }else{ ?>Alertar Usuário (Site)<?php } ?></a>
</div>

</div>
<br />


 <div class='menuouterwrap'>
  <div class='menucatwrap'><div id="content_kat"><img src='./images/menu_title_bullet.gif'> Dados Salvos</div></div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/stafflogs' style='text-decoration:none'><?php if($pageid == "stafflogs"){ ?><b>Dados da Moderação</b><?php }else{ ?>Dados da Moderação<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/banlogs' style='text-decoration:none'><?php if($pageid == "banlogs"){ ?><b>Lista de usuários banidos</b><?php }else{ ?>Lista de usuários banidos<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/alertlogs' style='text-decoration:none'><?php if($pageid == "alertlogs"){ ?><b>Dados de Alertas</b><?php }else{ ?>Dados de Alertas<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/chatlogs' style='text-decoration:none'><?php if($pageid == "chatlogs"){ ?><b>Conversas</b><?php }else{ ?>Conversas<?php } ?></a>
</div>

<div class='menulinkwrap'>&nbsp;
<img src='./images/item_bullet.gif' border='0' alt='' valign='absmiddle'>&nbsp;
<a href='./index/p/onlinelogs' style='text-decoration:none'><?php if($pageid == "onlinelogs"){ ?><b>Pessoas online</b><?php }else{ ?>Pessoas online<?php } ?></a>
</div>

</div>
<br />


<?php } ?>
