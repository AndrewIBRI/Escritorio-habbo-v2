<script src="<?php echo $path; ?>/web-gallery/static/js/regcommon.js" type="text/javascript"></script>
	
<body id="client" class="background-captcha">
<div id="overlay"></div>
<img src="<?php echo $path; ?>/web-gallery/v2/images/page_loader.gif" style="position:absolute; margin: -1500px;" />

<script type="text/javascript">
HabboView.add( function() {
     ChangePassword.init();


});
</script>

<div id="stepnumbers">
    <div class="stepdone">Aniversário &amp; Gênero</div>
    <div class="step2">Detalhes</div>
    <div class="step3focus">Segurança</div>
    
</div>

<div id="main-container"> 

<div id="error-placeholder">
<?php if(isset($errors)){ ?>

<div id="error-messages-container" class="cbb"> 
          <div class="rounded" style="background-color: #cb2121;"> 
          <div id="error-title" class="error"><?php echo $errors; ?>
	  </div></div></div>
<?php } ?>
</div>


    <h2>Entre no Hotel</h2>        <div id="avatar-choices">            <h3>Escolha um visual para sua primeira visita:</h3>        

    <ul id="avatars">        <li>

        <span class="bgtop"></span>

        <span class="bgbottom"></span>

        <img alt="hd-185-4.lg-3116-63-62.hr-893-32.ch-210-110" src="<?php echo $avatar; echo $row['look']; ?>/habbo-imaging/avatarimage?figure=hd-185-4.lg-3116-63-62.hr-893-32.ch-210-110" width="64" height="110"/>

    </li>

     <li>

        <span class="bgtop"></span>

        <span class="bgbottom"></span>

        <img alt="hd-180-2.lg-270-82.hr-115-37.ch-255-100" src="<?php echo $avatar; echo $row['look']; ?>/habbo-imaging/avatarimage?figure=hd-180-2.lg-270-82.hr-115-37.ch-255-100" width="64" height="110"/>



    </li>

     <li>

        <span class="bgtop"></span>

        <span class="bgbottom"></span>

        <img alt="hd-200-3.lg-285-96.hr-165-37.ch-3030-85" src="<?php echo $avatar; echo $row['look']; ?>/habbo-imaging/avatarimage?figure=hd-200-3.lg-285-96.hr-165-37.ch-3030-85" width="64" height="110"/>

    </li>

    <li>

        <span class="bgtop"></span>

        <span class="bgbottom"></span>



        <img alt="hr-890-36.hd-627-1.ch-680-72.lg-715-72.sh-907-62.ca-1805-6" src="<?php echo $avatar; echo $row['look']; ?>/habbo-imaging/avatarimage?figure=hr-890-36.hd-627-1.ch-680-72.lg-715-72.sh-907-62.ca-1805-6" width="64" height="110"/>

    </li>

    <li>

        <span class="bgtop"></span>

        <span class="bgbottom"></span>

        <img alt="hr-890-45.hd-629-1.ch-665-62.lg-720-71.sh-725-62.he-1602-71.ca-1805-71" src="<?php echo $avatar; echo $row['look']; ?>/habbo-imaging/avatarimage?figure=hr-890-45.hd-629-1.ch-665-62.lg-720-71.sh-725-62.he-1602-71.ca-1805-71" width="64" height="110"/>

    </li>

     <li>

        <span class="bgtop"></span>

        <span class="bgbottom"></span>

        <img alt="hr-890-39.hd-629-1.ch-665-62.lg-720-81.sh-725-62.he-1605-77.ca-1805-81" src="<?php echo $avatar; echo $row['look']; ?>/habbo-imaging/avatarimage?figure=hr-890-39.hd-629-1.ch-665-62.lg-720-81.sh-725-62.he-1605-77.ca-1805-81" width="64" height="110"/>

    </li>            </ul>   

<p style="clear: left;">
                Não gosta de nenhum desses?
                <br/><span class="help">Não se preocupe - dá pra mudar seu visual depois.</span></p>
</div>


<h3>Apenas mais uma coisinha de segurança antes:</h3>

            <div id="captcha-image-container">
                <div id="recaptcha_image"><img src="<?php echo $path; ?>/captcha/captcha.php"></div>
            </div>

    <div class="delimiter_smooth">
        <div class="flat">&nbsp;</div>
        <div class="arrow">&nbsp;</div>
        <div class="flat">&nbsp;</div>

    </div>

  <div id="inner-container">
        <form id="captcha-form" method="post" action="<?php echo $path; ?>/quickregister/captcha_submit" onsubmit="Overlay.show(null,'Carregando...');">
            <div id="recaptcha-input-title">Digite oque você vê abaixo:</div>
            <div id="recaptcha-input">
                <input type="text" tabindex="2" name="captchaResponse" id="recaptcha_response_field">
            </div>

			                <input type="hidden" id="avatarFigure" name="bean.figure" value=""/>
        </form>

    </div>

    <div id="select">
        <a href="<?php echo $path; ?>/quickregister/back" id="back-link">Voltar</a>
        <div class="button">
            <a id="proceed-button" href="#" class="area">Pronto!</a>
            <span class="close"></span>
        </div>

   </div>
<script type="text/javascript">
    document.observe("dom:loaded", function() {
        Event.observe($("back-link"), "click", function() {
            Overlay.show(null,'Carregando...');
        });
        Event.observe($("proceed-button"), "click", function() {
            Overlay.show(null,'Carregando...');            
            $("captcha-form").submit();
        });
            $("captcha-form").focus();
			            QuickRegister.initAvatarChooser();
    });
</script>

	
	<script type="text/javascript"> 
    HabboView.run();
</script> 
 
</body> 
</html>