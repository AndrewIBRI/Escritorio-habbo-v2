<!DOCTYPE html>
    <html>
        <head>
            <meta charset="utf-8">
            <title>{hotelName}: Registration</title>
				<meta name="description" content="iMG">
				<link rel="stylesheet" type="text/css" href="app/tpl/skins/{skin}/css/style.css" />
				<link rel="stylesheet" type="text/css" href="app/tpl/skins/{skin}/css/fonts.css" />
				<link rel="shortcut icon" href="/app/tpl/skins/{skin}/images/favicon.ico" type="image/x-icon"> 
				
			<script type="text/javascript">
            document.habboLoggedIn = true;
            var habboName = null;
            var habboId = null;
            var habboReqPath = "";
            var habboStaticFilePath = "{url}/app/tpl/skins/Habbo";
            var habboImagerUrl = "http://www.habbo.com/habbo-imaging/";
            var habboPartner = "";
            var habboDefaultClientPopupUrl = "{url}/client";
            window.name = "habboMain";
            if (typeof HabboClient != "undefined") {
                HabboClient.windowName = "client";
                HabboClient.maximizeWindow = true;
            }
        </script>
		
    </head>
		
        <!-- BODY -->
        <body>
            <header>
                <div class="container head">
                    <div class="online">
                        <span class="sprite online"></span>
                        <p>There are currently {online} user(s) online!</p>
                    </div>
                    <div class="stats">
                        <div class="coins-pix">
							<a href="/index">
								<p>Sign In</p>
							</a>
                        </div>
                    </div>
                </div>
            </header>
            <div class="container body">
			<div class="content-box medium left">
                    <div class="content">
					<div class="content-container">
							<div class="content-header blue">
								<p>Registration</p>
							 </div>
							 <div class="content-content"></div>
								<form method="post" id="phase-0-form"> 
								<?php if(isset($template->form->error)) { echo '<div class="content-header red"><p>OOPS! '.$template->form->error.'.</p></div>'; } ?>
									<div class="content-content">
									<p>
										<strong>Desired Username</strong>
										<br />
										<input type="text" id="habbo-name" placeHolder="Username" size="35" value="<?php echo $template->form->reg_username; ?>" name="reg_username" class="text-field" maxlength="32"> 
										<br /><br />
										<strong>Desired Password</strong>
										<br />
										<input type="password" id="password" placeHolder="Password" size="35" name="reg_password" value="" class="password-field" maxlength="32"> 
										<br /><br />
										<strong>Confirm Password</strong>
										<br />
										<input type="password" id="password2" placeHolder="Confirm Password" size="35" name="reg_rep_password" value="" class="password-field" maxlength="32"> 
										<br /><br />
										<strong>Email Address</strong>
										<br />
										<input type="text" id="email" size="35" placeHolder="Email" name="reg_email" value="<?php echo $template->form->reg_email; ?>" class="text-field" maxlength="48"> 
										<br /><br />
										<strong>Security Key (Must be 4 numbers)</strong>
										<br />
										<input type="password" id="seckey" size="35" placeHolder="Security Key" name="reg_seckey" value="" class="text-field" maxlength="4">
										<br /><br />
										<button type="submit" name="register" class="submit green">Register</button>
										<button type="button" name="cancel" class="submit red" onclick="location.href='{url}/'" style="float:right;margin-right:35px;">Cancel</button>
									</p>
								</form>
							 </div>
						</div>
					</div>
				</div>
			<div class="content-box small right">
                    <div class="content">
					<div class="content-container">
							<div class="content-header blue">
								<p>Facebook</p>
							 </div>
							 <div class="content-content">
								<p>
									<div class="facebook"></div>
								</p>
							 </div>
						</div>
					</div>
				<div class="content-box small right">
                    <div class="content">
					<div class="content-container">
							<div class="content-header blue">
								<p>Twitter</p>
							 </div>
							 <div class="content-content">
								<p>
									<div class="twitter"></div>
								</p>
							 </div>
						</div>
					</div>
				</div>
				<div class="content-box small right">
                    <div class="content">
					<div class="content-container">
							<div class="content-header blue">
								<p>Google+</p>
							 </div>
							 <div class="content-content">
								<p>
									<div class="google"></div>
								</p>
							 </div>
						</div>
					</div>
				</div>
			</div>