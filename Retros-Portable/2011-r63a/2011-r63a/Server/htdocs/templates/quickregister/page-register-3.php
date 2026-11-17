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
   <div class="step1">Aniversário & Gênero</div>
    <div class="step2focus">Detalhes</div>
    <div class="step3">Segurança</div>

    
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
        <a href="<?php echo $path; ?>/quickregister/back" id="back-link">Cancelar</a>
        <div class="button">
            <a id="proceed-button" href="#" class="area">Concluír</a>
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