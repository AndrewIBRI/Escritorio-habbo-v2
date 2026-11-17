<body id="client" class="background-accountdetails-male">
<div id="overlay"></div>
<img src="/web-gallery/v2/images/page_loader.gif" style="position:absolute; margin: -1500px;" />


<div id="change-password-form" style="display: none;">

    <div id="change-password-form-container" class="clearfix">

        <div id="change-password-form-title" class="bottom-border">Esqueceu a senha?</div>

        <div id="change-password-form-content" style="display: none;">

            <form method="post" action="<?php echo $path; ?>/account/password/identityResetForm" id="forgotten-pw-form">

                <input type="hidden" name="page" value="/quickregister/start?changePwd=true" />

                <span>Introduza seu e-mail</span>

                <div id="email" class="center bottom-border">

                    <input type="text" id="change-password-email-address" name="emailAddress" value="" class="email-address" maxlength="48"/>

                    <div id="change-password-error-container" class="error" style="display: none;">Introduza seu e-mail</div>

                </div>

            </form>

            <div class="change-password-buttons">

                <a href="#" id="change-password-cancel-link">Cancelar</a>

                <a href="#" id="change-password-submit-button" class="new-button"><b>Enviar email</b><i></i></a>

            </div>

        </div>

        <div id="change-password-email-sent-notice" style="display: none;">

            <div class="bottom-border">

              <span>Nós enviamos um email para você para alterar a senha.<br>
<br>

ATENÇÃO: Lembre-se de ohar a lixeira!</span>

                <div id="email-sent-container"></div>

            </div>

            <div class="change-password-buttons">

                <a href="#" id="change-password-change-link">Atrás</a>

                <a href="#" id="change-password-success-button" class="new-button"><b>Fechar</b><i></i></a>

            </div>

        </div>

    </div>

    <div id="change-password-form-container-bottom"></div>

</div>



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


<?php if(isset($errors)){ ?>

<div id="error-messages-container" class="cbb"> 
          <div class="rounded" style="background-color: #cb2121;"> 
          <div id="error-title" class="error"><?php echo $errors; ?>
	  </div></div></div>
<?php } else {?>
<?php } ?>


    <form method="post" action="<?php echo $path; ?>/quickregister/email_password_submit" id="quickregister-form">



        <div id="title">Informações da sua Conta</div>

      <div id="inner-container">

        <div class="inner-content bottom-border">
		  <div class="field">

                <label for="user-address">Nome de Usuário</label>

                <input type="text" id="email-address" name="bean.name" value="<?php echo FilterText($_POST['bean_name']); ?>" <?php if(isset($errors['name'])) { echo 'class="error"'; } else { } ?>/>

            </div>
                    <div class="help">Este é o nome que você usará para entrar no Hotel, lembre-se dele!</div>

            <div class="field">

                <label for="email-address">E-mail</label>

                <input type="text" id="email-address" name="bean.email" value="<?php echo FilterText($_POST['bean_email']); ?>" <?php if(isset($errors['email'])) { echo 'class="error"'; } else { } ?>/>

            </div>

            <div class="help">Você pode usar para fazer o Login no Hotel também. </div>

            <div id="password-field" class="field">

                <label for="register-password">Sua Senha</label>

                <input type="password" name="bean.password" id="register-password" maxlength="32" value="" <?php if(isset($errors['password'])) { echo 'class="error"'; } else { } ?>/>
<div class="help">Escolha uma senha bem forte e que você não esqueça. </div>

            </div>
			 <div id="password-field" class="field">

                <label for="register-password">Repita a Senha</label>

                <input type="password" name="bean.retypedPassword" id="register-password" maxlength="32" value="" <?php if(isset($errors['retypedpassword'])) { echo 'class="error"'; } else { } ?>/>

            </div>

            <div class="help">Apenas para ter certeza. </b> para garantir maior segurança dentro do Hotel.</div>

        </div>



        <div class="inner-content top-margin">

			<div class="field-content checkbox <?php if(isset($errors['terms'])) { echo 'error';} else { } ?>">

			  <label>

			    <input type="checkbox" name="bean.termsOfServiceSelection" id="terms" value="true" class="checkbox-field"/>

			    Aceito os <a href="/papers/disclaimer" target="_blank" onclick="window.open('/papers/disclaimer'); return false;">Termos e condições</a> do Hotel

			  </label>

			</div>            

			

			

			

        </div>

      </div>

    </form>





    <div id="select">

        <div class="button">

            <a id="proceed-button" href="#" class="area">Próximo!</a>

            <span class="close"></span>

        </div>

        <a href="<?php echo $path; ?>/quickregister/back" id="back-link">Cancelar</a>

   </div>

</div>



<script type="text/javascript">

    document.observe("dom:loaded", function() {

        Event.observe($("back-link"), "click", function() {

            Overlay.show(null,'Cargando...');

        });

        Event.observe($("proceed-button"), "click", function() {

            Overlay.show(null,'Cargando...');            

            $("quickregister-form").submit();

        });

            $("email-address").focus();

    });

</script>

<script type="text/javascript">
    HabboView.run();
</script>

</body>
</html>