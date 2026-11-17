<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<script type="text/javascript" src="js/jquery.js"></script>
	<title>{hotelName}: Where the fun begins!</title>
	<link rel="stylesheet" type="text/css" media="screen" href="app/tpl/skins/{skin}/css/style.css">
	<link rel="stylesheet" type="text/css" media="screen" href="app/tpl/skins/{skin}/css/index.css">    
	<script type="text/javascript" src="js/jquery.js"></script>
	<link rel="shortcut icon" href="{url}/app/tpl/skins/Habbo/web-gallery/favicon.ico" type="image/vnd.microsoft.icon" />

		<!-- Index structure by Louis (Lazarus) -->
		<!-- Index image & further edits by Tyler of Habboon -->
		
	<script type="text/javascript">
			if (navigator.userAgent.toLowerCase().indexOf("chrome") >= 0)
			{
			var _interval = window.setInterval(function ()
			{
			var autofills = $('input:-webkit-autofill');
			if (autofills.length > 0)
			{
			window.clearInterval(_interval); // stop polling
			autofills.each(function()
			{
			var clone = $(this).clone(true, true);
			$(this).after(clone).remove();
			});
			}
			}, 20);
			}
	</script>
</head>
<body>
    <div class="container index">
        <div class="index-top-bar"> 
            <div class="container">
                <div class="online">
                    <span class="sprite alert"></span>
                    <p>Notice: Welcome to {hotelName}. Make sure you invite your friends!</p>
                </div>
                <div class="stats">
                <div class="index-online">
                        <p>{online} User(s) Online!</p>
                    </div>
                    <span class="sprite star"></span>
                </div>
            </div>
        </div>
        <a href="#">
            <div class="logo"></div>
        </a>
        <div class="index-bg">
            <div class="login">
                <form id="loginformitem" name="loginformitem" method="post">
					<button type="button" name="register" class="submit red" onclick="location.href='{url}/register'">Register</button>
                    <input type="text" name="log_username" class="login-form"  id="username" placeholder="Username">
                    <input type="password" name="log_password" class="login-form" id="password" placeholder="Password">
                    <button type="submit" class="submit green" name="login" >Login</button>
                </form>
            </div>
        </div>
			<script type="text/javascript">
                    HabboView.add(function() {
                        LandingPage.init();
                        if (!LandingPage.focusForced) {
                        LandingPage.fieldFocus('login-username');
                    }
                });
            </script>
		</div>
    <footer>
				<hr />
					<?php include('footer.php'); ?>
            </footer>
	</body>
</html>