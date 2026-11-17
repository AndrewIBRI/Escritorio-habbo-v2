           
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />

<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(empty($body_id)){
$body_id = "home";
}
?>

<body id="<?php echo $body_id; ?>" class=""<?php if($pageid == "4"){ ?> onLoad="NaviFenster()"<?php } ?>>

<div id="overlay"></div>


<div id="header-container">
	<div id="header" class="clearfix">
		<h1><a href="index"></a></h1>
       <div id="subnavi">
                           

                                                        


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
                            <label for="login-remember-me" class="left">Mantenha-me conectado</label>
                        </li>
                    </ul>
                </form>
                <div id="subnavi-login-help" class="clearfix">
                    <ul>
                        <li class="register"><a href="<?php echo $path; ?>/forgot" id="forgot-password"><span>Esqueceu a senha?</span></a></li>
                    	<li><a href="<?php echo $path; ?>/register"><span>Registre-se agora!</span></a></li>
                    </ul>
                </div>
<div id="remember-me-notification" class="bottom-bubble" style="display:none;">
	<div class="bottom-bubble-t"><div></div></div>
	<div class="bottom-bubble-c">
					Selecionando esta opção permancerá conectado no <?php echo $shortname; ?> até clicar em "Sair".
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
				
                </div>

<div id="to-hotel">

					    <?php if($user_rank > 6 && $logged_in == true){ ?> ~
				<a href="<?php echo $adminpath; ?>" class="new-button red-button" id="signout"><b>Controles</b><i></i></a><?php } ?>

					   <a href="/client" class="new-button green-button" target="client" onclick="HabboClient.openOrFocus(this); return false;"><b>Entrar no Hotel</b><i></i></a><!-- <div id="hotel-closed-medium"><class="new-button red-button" ><b>iHabi Hotel Offline</b><i></i></a> -->


	</div>   
</div>


        </div>
        <script type="text/javascript">
		L10N.put("purchase.group.title", "Criar um grupo");
        </script>
        <?php } ?>
        <ul id="navi">
        <?php if($pageid > 0 && $pageid < 5 || $pageid == "myprofile" && $logged_in == true){ ?>
        <li class="metab selected"><strong>
	<?php echo $name; ?>
	<?php if($_SESSION['FACEBOOK'] != "YES"){ ?> <?php } 
	else{
	 ?> <?php } 
		?>
	</strong><span></span></li>
        <?php } elseif($logged_in == true){ ?>
        <li class="metab"><a href="<?php echo $path; ?>/me">
	<?php echo $name; ?>
	<?php if($_SESSION['FACEBOOK'] != "YES"){ ?> <?php } 
	else{
	 ?> <?php } 
		?>
	</a><span></span></li>
        <?php } elseif($logged_in !== true){ ?>
        <li id="tab-register-now"><a href="quickregister/start" target="_self">Registre-se agora!</a><span></span></li>
        <?php } ?>
		
<?php if($pageid >= 10 && $pageid <= 20 || $pageid == "profile" || $pageid == "32" || $pageid == "33"){ ?>
        <li class="selected"><strong>Comunidade</strong><span></span></li>
        <?php } else { ?>
        <li class=" "><a href="<?php echo $path; ?>/community">Comunidade</a><span></span></li>
        <?php } ?>

       <?php if($pageid >= 1000 && $pageid <= 1100){ ?>
        <li class="selected"><strong>Equipe</strong><span></span></li>
        <?php } else { ?>
        <li class=""><a href="<?php echo $path; ?>/community/staff">Equipe</a><span></span></li>
        <?php } ?>   
         


</ul>
<div id="habbos-online" style="border-radius: 3px; margin-left: 3px; margin-top: -2px;"><span style="border: 0px solid #00596B; border-radius: 10px; height: 46px; width: 80px; padding-top: 19px;"><?php echo $online_count; ?></br> usuários online</span></div>

		</div>
</div>

<div id="content-container">


<div id="navi2-container" class="pngbg">

    <div id="navi2" class="pngbg clearfix">

	
	<ul>
       <?php if($pageid > 0 && $pageid < 5 || $pageid == "myprofile"){ ?>
                <?php if($pageid == "1"){ ?>
                <li class="selected">
    		Principal

                <?php } else { ?>
                <li class="">
    		<a href="<?php echo $path; ?>/me">Principal</a>
                <?php } ?>
        		</li>

<?php if($pageid == "2"){ ?>
                <li class="selected">
    				Minha Página
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/home/<?php echo $name; ?>">Minha Página</a>
                <?php } ?>
        		</li>

                <?php if($pageid == "3"){ ?>
                <li class="selected">
    				Preferências
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/profile">Preferências</a>
                <?php } ?>
        		</li>

   <?php if($pageid == "5"){ ?>
                <li class="selected">
    				Regras
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/">Regras</a>
                <?php } ?>
        		</li>


   <?php if($pageid == "5"){ ?>
                <li class="selected">
    				Hall da Fama
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/community/topstats">Hall da Fama</a>
                <?php } ?>
        		</li>

              

<?php if($pageid == "8"){ ?>
                <li class="selected">
    				Sair
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/account/logout">Sair</a>
                <?php } ?>
        		</li>


</ul><li class="stats-visited">&nbsp;&nbsp;&nbsp;<span class="stats-fig">10</span> <font color="#0094FF">onlines</font></li>

                <li class="">
               
                


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
    				Notícias
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/articles">Noticias</a>
                <?php } ?>

                <?php if($pageid == "11b"){ ?>
                <li class="selected">
				Referências
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/community/referidos">Referências</a>
                <?php } ?>


              

 <?php } else if($pageid >= 1000  && $pageid <= 1100){ ?>
                <?php if($pageid == "1000"){ ?>
                <li class="selected ">
				Equipe Staff
		<?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/community/staff">Equipe Staff</a>
                <?php } ?>
                
                <?php if($pageid == "1100"){ ?>
                <li class="selected ">
				Estágiarios
		<?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/community/esta">Estágiarios</a>
                <?php } ?>  

                <li class="">
	    			 
                


                
    	    	</li>
	    			
                

<?php } ?>

    	    	</li>
</ul>

	</div>
</div>