<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(empty($body_id)){
$body_id = "home";
}

?>
<body id="<?php echo $body_id; ?>" class=""<?php if($pageid == "4"){ ?> onload="NaviFenster()"<?php } ?>>
<link rel="stylesheet" href="/web-gallery/v2/styles/top.css" type="text/css" /> 
<div id="top">
<p class="alignleft"><b> Bem vindo <?php echo $name; ?>  | <img src="/web-gallery/v2/images/test/new_04.png"/> <?php echo $myrow['credits']; ?> Créditos		</li>  |  <img src="/web-gallery/v2/images/test/cat_1.png"/> <?php echo $myrow['activity_points']; ?>  Pixels  | <img src="/Premiun_Images/dolares.png" /> <?php echo $myrow['dolares']; ?> Dólares <b></b>   
<p class="alignright"> <img src="/web-gallery/v2/images/test/new_17.gif"/> <?php echo $online_count; ?> <?php echo $shortname; ?>'s Online </li>  </b> |  <a href="/account/logout"><img src="/web-gallery/v2/images/test/salir.gif"/></a></p>     
<div style="clear: both;"></div></div> 
<div id="overlay"></div>

<body id="<?php echo $body_id; ?>" class=""<?php if($pageid == "4"){ ?> onload="NaviFenster()"<?php } ?>>
<div id="overlay"></div>

<div id="header-container">
	<div id="header" class="clearfix">
		<h1><a href="index"></a></h1>
       <div id="subnavi">
			<div id="subnavi-user">
                            <?php if($logged_in){ ?>
				<ul>
					<li id="myfriends"><a href="#"><span>Meus Amigos</span></a><span class="r"></span></li>
					<li id="mygroups"><a href="#"><span>Meus Grupos</span></a><span class="r"></span></li>
					<li id="myrooms"><a href="#"><span>Minhas Salas</span></a><span class="r"></span></li>
				</ul>

                            <?php } elseif(!$logged_in){ ?>
                                <div class="clearfix">&nbsp;</div>
        <p>
                <a href="/client" id="enter-hotel-open-medium-link" target="client" onclick="HabboClient.openOrFocus(this); return false;">Entrar no <?php echo $shortname; ?> Hotel</a>
        </p>

                            <?php } ?>

			</div>
        <?php if(!$logged_in){ ?>
            <div id="subnavi-login">
                <form action="<?php echo $path; ?>/index?page=&username=<?php echo $username; ?>&rememberme=false" method="post" id="login-form">
            		<input type="hidden" name="page" value="<?php echo $pageid; ?>" />
                    <ul>
                        <li>
                            <label for="login-username" class="login-text"><b><?php echo $shortname; ?> Nome</b></label>
                            <input tabindex="1" type="text" class="login-field" name="username" id="login-username" />
		                    <a href="#" id="login-submit-new-button" class="new-button" style="float: left; display:none"><b>Login</b><i></i></a>
                            <input type="submit" id="login-submit-button" value="Login" class="submit"/>
                        </li>
                        <li>
                            <label for="login-password" class="login-text"><b>Senha</b></label>
                            <input tabindex="2" type="password" class="login-field" name="password" id="login-password" />
                            <input tabindex="3" type="checkbox" name="_login_remember_me" value="true" id="login-remember-me" />
                            <label for="login-remember-me" class="left">Manter-me conectado</label>
                        </li>
                    </ul>
                </form>
                <div id="subnavi-login-help" class="clearfix">
                    <ul>
                        <li class="register"><a href="<?php echo $path; ?>/forgot" id="forgot-password"><span>Esqueceu sua senha?</span></a></li>
                    	<li><a href="<?php echo $path; ?>/register"><span>Registe-se agora!</span></a></li>
                    </ul>
                </div>
<div id="remember-me-notification" class="bottom-bubble" style="display:none;">
	<div class="bottom-bubble-t"><div></div></div>
	<div class="bottom-bubble-c">
					 Selecionar esta opção permanecer conectado ao <?php echo $shortname; ?> até que você dê a opção em "Disconnect"
	</div>
	<div class="bottom-bubble-b"><div></div></div>
</div>
            </div>
        </div>
		<script type="text/javascript">
			LoginFormUI.init();
			RememberMeUI.init("right");
		</script>
        <?php } else { ?>
            <div id="subnavi-search">
                <div id="subnavi-search-upper">
                <ul id="subnavi-search-links">
                    <li><a href="<?php echo $path; ?>/help" target="habbohelp" onclick="openOrFocusHelp(this); return false">Ajuda</a></li>
					<li><a href="<?php echo $path; ?>/account/logout?token=<?php echo md5($myrow['last_online']); ?>" class="userlink">Sair</a></li>
				</ul>
                </div>


<div id="to-hotel">
<?php 

$get_flashclient = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_flashclient' AND example = '1'");
if(mysql_num_rows($get_flashclient) > 0 && $server['status'] == "1"){ 

?>



<a href="/client" class="new-button green-button" target="2941aeb24b02737dbd8720de2ebc95e8e240c39b" onclick="HabboClient.openOrFocus(this); return false;"><b>Entrar no <?php echo $shortname; ?> Hotel</b><i></i></a>
<?php } else { ?>

<?php } ?></div>


	   
</div>


        </div>
        <script type="text/javascript">
		L10N.put("purchase.group.title", "Criar um grupo");
        </script>
        <?php } ?>
        <ul id="navi">
        <?php if($pageid > 0 && $pageid < 5 || $pageid == "myprofile" && $logged_in == true){ ?>
        <li class="selected"><strong>
	<?php echo $name; ?>( <img src="<?php echo $path; ?>/web-gallery/v2/images/icon_habbo_small.png"> )
		</strong><span></span></li>
        <?php } elseif($logged_in == true){ ?>
        <li class=" "><a href="<?php echo $path; ?>/me">
	<?php echo $name; ?>( <img src="<?php echo $path; ?>/web-gallery/v2/images/icon_habbo_small.png"> )
		</a><span></span></li>
        <?php } elseif($logged_in !== true){ ?>
        <li id="tab-register-now"><a href="<?php echo $path; ?>/register" target="_self">Registe-se agora!</a><span></span></li>
        <?php } ?>

        <?php if($pageid >= 10 && $pageid <= 20 || $pageid == "profile"){ ?>
        <li class="selected"><strong>Comunidade</strong><span></span></li>
        <?php } else { ?>
        <li class=" "><a href="<?php echo $path; ?>/community">Comunidade</a><span></span></li>
        <?php } ?>

        <?php if($pageid >= 5 && $pageid <= 9){ ?>
        <li class="selected"><strong>Créditos</strong><span></span></li>
        <?php } else { ?>
        <li class=" "><a href="<?php echo $path; ?>/credits">Créditos</a><span></span></li>
        
        <?php } ?>

		 <?php if($pageid >= 70 && $pageid <= 80){ ?>
        <li class="selected"><strong>Bots <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></strong><span></span></li>
        <?php } else { ?>
        <li class=" "><li id="tab-register-now"><a href="<?php echo $path; ?>/bots">Bots <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a><span></span></li>
        <?php } ?>
		
        <?php if($pageid >= 1000 && $pageid <= 1000){ ?>
        <li class="selected"><strong>Referidos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></strong><span></span></li>
        <?php } else { ?>
        <li class=" "><li id="tab-register-now"><a href="<?php echo $path; ?>/referidos">Referidos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a><span></span></li>
        <?php } ?>

        <?php if($user_rank > 4 && $logged_in == true){ ?>
        <li id="tab-register-now"><a href="<?php echo $adminpath; ?>"><b>Administração</b></a><span></span></li>
        <?php } ?>
</ul>

	</div>
</div>

<div id="content-container">

<div id="navi2-container" class="pngbg">
    <div id="navi2" class="pngbg clearfix">

	<ul>
        <?php if($pageid > 0 && $pageid < 5 || $pageid == "myprofile"){ ?>
                <?php if($pageid == "1"){ ?>
                <li class="selected">
    		Home 

                <?php } else { ?>
                <li class="">
    		<a href="<?php echo $path; ?>/me">Home</a>
                <?php } ?>
        		</li>

                <?php if($pageid == "myprofile"){ ?>
                <li class="selected">
    				Minha Página
                <?php } else { ?>
                <li class="">
    				<a href="<?php echo $path; ?>/home/<?php echo $rawname; ?>">Minha Página</a>
                <?php } ?>
        		</li>

                <?php if($pageid == "2"){ ?>
                <li class="selected">
    				Ajustes
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/profile">Ajustes</a>
                <?php } ?>
    	    	</li>


                <?php if($pageid == "3"){ ?>
                <li class="selected">
    				
                <?php } elseif($logged_in){ ?>
                <li class="">

                <?php } ?>
    	    	</li>

                
        <?php } else if($pageid >= 10 && $pageid <= 20 || $pageid == "profile"){ ?>
                <?php if($pageid == "10"){ ?>
                <li class="selected">
    				Comunidade
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/community">Comunidade</a>
                <?php } ?>

                <?php if($pageid == "11"){ ?>
                <li class="selected">
    				Noticias
                <?php } else { ?>
                <li class=" ">
	    	<a href="<?php echo $path; ?>/articles">Noticias</a>
                <?php } ?>

                <?php if($pageid == "13a"){ ?>
               
                <li class="selected">
    				VIP
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/community/vip">VIP</a>
                <?php } ?>
                <?php if($pageid == "13"){ ?>
                <li class="selected">
				Staff
		<?php } else { ?>
                <li class=" ">
	    	<a href="<?php echo $path; ?>/community/staff">Staff</a>

                <?php } ?>
                <?php if($pageid == "15"){ ?>
                <li class="selected ">
    				Créditos
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/credits">Créditos</a>
                <?php } ?>
                <?php if($pageid == "6"){ ?>
                <li class="selected">
    				
                <?php } else { ?>
                <li class=" ">

                <?php } ?>
                <?php if($pageid == "7"){ ?>
                <li class="selected">
    				
                <?php } else { ?>
                <li class=" ">

                <?php } ?>
                <?php if($pageid == "8"){ ?>
                <li class="selected">
    				Pixels
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/credits/pixels">Pixels</a>
                <?php } ?>
                <?php if($pageid == "6b"){ ?>
     
                <?php } ?>
				<?php } else if($pageid >= 70  && $pageid <= 80){ ?>
                <?php if($pageid == "70"){ ?>
                <li class="selected ">
    				Bots <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/bots">Bots <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
				<?php if($pageid == "71"){ ?>
                <li class="selected ">
    				Editar Bots <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/bots/editar">Editar Bots <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
				<?php if($pageid == "72"){ ?>
                <li class="selected ">
    				Efeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/bots/efectos">Efeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php } else if($pageid >= 40  && $pageid <= 47){ ?>
                <?php if($pageid == "40"){ ?>
                <li class="selected ">
    				Principal <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda">Principal <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "41"){ ?>
                <li class="selected ">
    				Emblemas <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/placas">Emblemas <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "42"){ ?>
                <li class="selected ">
    				Raros <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/rares">Raros <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "43"){ ?>
                <li class="selected ">
    				Respeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/respetos">Respeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "44"){ ?>
                <li class="selected ">
    				Cavalos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/caballos">Cavalos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "45"){ ?>
                <li class="selected ">
    				Efeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/efectos">Efeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "47"){ ?>
                <li class="selected ">
    				Vip <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/vip">Vip <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
				


<?php } ?>
    	    	</li>
</ul>


	</div>
</div>

<?php
$get_sitealert = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_sitealert' AND value = '1'"); 
if(mysql_num_rows($get_sitealert) == "1"){
$get_sitealert_text = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_sitealert_text'");  
$get_sitealert_text = mysql_fetch_assoc($get_sitealert_text);

?>
<div align="center" style="color: red; background-color: white; border: 1px solid black; padding:2px; width: 100%">
<b>Atenci&oacute;n:</b> <?php echo $get_sitealert_text['value']; ?></div>


<?php } if($maintenance){ ?>
<div style="background-color: #FBEFEF; padding: 5px; margin: 5px; border: 1px solid #DF0101;">&iexcl;<?php echo $shortname; ?>Hotel está em manutenção! Em breve estaremos Online, Espere por nós!</div>
<?php } ?>