<!DOCTYPE html>
    <html>
        <head>
            <meta charset="utf-8">
            <title>{hotelName}: VIP</title>
				<meta name="description" content="iMG">
				<link rel="stylesheet" type="text/css" href="app/tpl/skins/{skin}/css/style.css" />
				<link rel="stylesheet" type="text/css" href="app/tpl/skins/{skin}/css/fonts.css" />
				<link rel="shortcut icon" href="/app/tpl/skins/{skin}/images/favicon.ico" type="image/x-icon"> 
				
			<!-- Javascript Includes -->
			<script type="text/javascript">
            document.habboLoggedIn = true;
            var habboName = "{username}";
            var habboId = 0;
            var habboReqPath = "";
            var habboStaticFilePath = "{url}/app/tpl/skins/Habbo";
            var habboImagerUrl = "http://www.habbo.com/habbo-imaging/";
            var habboPartner = "";
            var habboDefaultClientPopupUrl = "{url}/client";
            window.name = "habboMain";
            if (typeof HabboClient != "undefined") {
                HabboClient.windowName = "eac955c8dbc88172421193892a3e98fc7402021a";
                HabboClient.maximizeWindow = true;
            }
        </script>
		
		<script language="javascript" type="text/javascript">
			function popitup(url) {
				newwindow=window.open(url,'Client','height=760,width=1200');
				if (window.focus) {newwindow.focus()}
			return false;
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
                            <p>{coins}</p>
                            <span class="sprite credits"></span>
                        </div>
                        <div class="coins-pix">
                            <p>{activitypoints}</p>
                            <span class="sprite pixels"></span>
                        </div>
                    </div>
                </div>
            </header>
            <div class="container body">
			
			<!-- Navigation -->
                <div class="nav-bar">
                    <nav>
                        <ul>
                            <li><a href="#">Home</a>
								<ul>
									<li><a href="{url}/me">Home</a></li>
									<li><a href="{url}/account">Account Settings</a></li>
								</ul>
							</li>
                            <li><a href="#">Community</a>
                                <ul>
                                    <li><a href="{url}/news">News</a></li>
                                    <li><a href="{url}/staff">Staff</a></li>
                                </ul>
                            </li>
                            <li><a href="#">Store</a>
                                <ul>
                                    <li><a href="{url}/vip">Buy VIP</a></li>
                                </ul>
                            </li>
                            <li><a href="#">Forums</a>
								<ul>
									<li><a href="http://www.google.com" target="_blank">Visit</a></li>
									<li><a href="http://www.google.com" target="_blank">Register</a></li>
								</ul>
							</li>
							<li><a href="http://www.google.com" target="_blank">Facebook</a></li>
							<li class="right"><a href="{url}/client" onclick="return popitup('{url}/client')">Enter {hotelname} Hotel</a></li>
							<?php if(mysql_result(mysql_query("SELECT rank FROM users WHERE id = '" . $_SESSION['user']['id'] . "'"), 0) >= 4) { ?>
							<li class="right"><a href="#">Housekeeping</a>
                                <ul>
                                    <li><a href="{url}/securelogin/login">Secure Login</a></li>
                                </ul>
                            </li>
								<?php
							} ?>				
                        </ul>
                    </nav>
                </div>
				<!-- End Nav -->
				
				<div class="content-box half-half">
                    <div class="content">
						<div class="content-container">
							<div class="content-header orange">
								<p>Platinum VIP - $10</p>
							 </div>
							 <div class="content-content">
								<p>
									By purchasing Platinum VIP, you will receive the following:
										<br /><br />
									&nbsp; &nbsp; &nbsp; &raquo; :push command.
										<br />
									&nbsp; &nbsp; &nbsp; &raquo; :pull command.
										<br />
									&nbsp; &nbsp; &nbsp; &raquo; ::setspeed command.
										<br />
									&nbsp; &nbsp; &nbsp; &raquo; Enter full and locked rooms.
										<br />
									&nbsp; &nbsp; &nbsp; &raquo; 5 second flood time.
										<br />
									&nbsp; &nbsp; &nbsp; &raquo; Exclusive Platinum VIP catalogue.
										<br />
									&nbsp; &nbsp; &nbsp; &raquo; 25,000 coins.
										<br />
									&nbsp; &nbsp; &nbsp; &raquo; 15,000 pixels.
										<br />
									</p>
							 </div>
						</div>
					</div>
                    <div class="content">
					   <div class="content-container">
							<div class="content-header orange">
								<p>Events Staff - $15</p>
							 </div>
							 <div class="content-content">
									<p>
										By purchasing Events Staff, you will receive the following:
											<br /><br />
										&nbsp; &nbsp; &nbsp; &raquo; :push command.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; :pull command.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; ::setspeed command.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; :spull command.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; Enter full and locked rooms.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; 0 second flood time.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; Exclusive Events Staff catalogue.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; 75,000 coins.
											<br />
										&nbsp; &nbsp; &nbsp; &raquo; 50,000 pixels.
											<br />
									</p>
							 </div>
						</div>
					</div>
				</div>
			</div>
			<div class="content-box half-half right">
                <div class="content">
				   <div class="content-container">
                       <div class="content-header green">
						  <p>Purchase via PayPal</p>
				        </div>
                    <div class="content-content">
								<p>PayPal code goes here.</p>
						 </div>
					</div>
				</div>
                <div class="content">
					   <div class="content-container">
							<div class="content-header green">
								<p>Purchase via PayGol</p>
							 </div>
							 <div class="content-content">
									<p>PayGol code goes here.</p>
							 </div>
						</div>
					</div>
				</div>
			<footer>
			<br />
				<hr />
					<?php include('footer.php'); ?>
            </footer>
        </body>
    </html>