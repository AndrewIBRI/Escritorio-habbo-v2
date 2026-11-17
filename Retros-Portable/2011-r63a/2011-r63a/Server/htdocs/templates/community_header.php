           
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="<?php echo $path; ?>/images/square.css" type="text/css"/>
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

      
<a href="<?php echo $path;?>/me">
			<div class="logo">
			</div>
		</a>
			
<ul>

                                                        


        <?php if(!$logged_in){ ?>
            
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

					    <?php if($user_rank > 6 && $logged_in == true){ ?> 
				<a href="<?php echo $adminpath; ?>" class="new-button red-button" id="signout"><b>Controles</b><i></i></a><?php } ?>

					   
	</div>   
</div>

</div>
        
        <?php } ?>

       
         


</ul>

		</div>
</div>


<div style='width:100%;'>
<div class="navigation">
<div style="margin-left:22%;">
<a href='<?php echo $path; ?>/me'><li class="community" style='margin-top:5px;'><?php echo $name; ?></li></a>

<a href='<?php echo $path; ?>/community'><li class="community" style='margin-top:5px;'>Comunidade</li></a>



<a href=''><li class="" style='margin-top:5px;'>Segurança</li></a>



<a href='<?php echo $path; ?>/habbo_way.php'><li class="community" style='margin-top:5px;'>Loja de Emblema</li></a>


<a href='<?php echo $path; ?>/account/logout?token=<?php echo md5($myrow['last_online']); ?>'><li class="community" style='margin-top:5px;'>Sair</li></a>

	
<a href='<?php echo $path; ?>/client' target="_blank"><li class="community" style='margin-top:5px;'>Entrar no Hotel</li></a>

<div style="margin-left:660px; position:absolute"><li class="" style='margin-top:5px;'><font color=red><b> <?php echo $online_count; ?> usuários online</b></font></li></div>

			
</div>
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
    				Preferências
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/profile">Preferências</a>
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
    				Notícias
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/articles">Notícias</a>
                <?php } ?>

                <?php if($pageid == "12"){ ?>
                <li class="selected">
				Equipe 
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/community/staff">Equipe </a>
                <?php } ?>
                
                <?php } else if($pageid >= 1000  && $pageid <= 1100){ ?>
                <?php if($pageid == "1000"){ ?>
                <li class="selected ">
				Dicas de Segurança
		<?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/safety/safety_tips">Dicas de Segurança</a>
                <?php } ?>
                
                <?php if($pageid == "1100"){ ?>
                <li class="selected ">
				Etiqueta <?php echo $shortname; ?>
		<?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/safety/habbo_way">Etiqueta <?php echo $shortname; ?></a>
                <?php } ?>              
                
                <?php } else if($pageid >= 5  && $pageid <= 9){ ?>
                <?php if($pageid == "5"){ ?>
                <li class="selected ">
    				Moedas
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/credits">Moedas</a>

                <?php } ?>
                <?php if($pageid == "9"){ ?>
                <li class="selected">
				<?php echo $shortname; ?> VIP
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/credits/vip"><?php echo $shortname; ?> VIP</a>
                <?php } ?>
                <?php if($pageid == "7b"){ ?>
                <li class="selected">
    			
                <?php } else { ?>
                <li class=" ">
				
				<?php } ?>
                <?php if($pageid == "8"){ ?>
                <li class="selected">
    				Loja de Emblemas
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/credits/pixels">Loja de Emblemas</a>
                <?php } ?>
                <?php if($pageid == "6b"){ ?>
                <li class="selected">
    				
                <?php } else { ?>
                <li class=" ">
	    			<?php if($pageid == "14"){ ?>
                <li class="selected">
    				Hall da Fama
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/topstats.php">Hall da Fama</a>
                <?php } ?>
        		</li>
                <?php } ?>

<?php } ?>

    	    	</li>
</ul>

<span class="navi_right">&nbsp;</span>
</ul>
</div>

					
</ul>
</div>
    	    	</li>
</ul>

	</div>
</div>