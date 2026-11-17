<!DOCTYPE html>
    <html>
        <head>
            <meta charset="utf-8">
            <title>{hotelName}: Home</title>
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
				
				<div class="top left-large">
                    <div class="user-stats">
                        <div class="content">
                            <div class="content-container">
                                <div class="av-img-plate">
								    <img src="http://www.habbo.nl/habbo-imaging/avatarimage?figure={figure}.gif" alt="{username}" class="big-av" alt="img" />
                                </div>
                                <div class="bout-container">
                                    <div class="bout">
                                        <p><b>Username:</b></p>
                                        <p class="right">{username}</p>
                                    </div>
                                    <div class="bout">
                                        <p><b>Motto:</b></p>
                                        <p class="right">{motto}</p>
                                    </div>
                                    <div class="bout">
                                        <p><b>Last Visited:</b></p>
                                        <p class="right">{lastSignedIn}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="enter-hotel">
                        <div class="content">
                            <div class="content-container">
                                <div class="enter-hotel-plate">
								<a href="{url}/client" onclick="return popitup('{url}/client')">
                                   <button name="enter" class="enter-hotel">Enter {hotelName} Hotel</button>
                                </a>
                                <a href="{url}/logout">
                                    <button name="enter" class="log-out">Logout</button>
                                </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
				

                <!--<div class="top right-small">
                    <div class="content">
                        <div class="content-container">
                            <img src="app/tpl/skins/{skin}/images/news1.png" class="news-img" />
                            <a href="#">
                                <div class="news-title"> 
                                    <p><b>He Strikes Again!</b> &#187 Stealing a Modem.</p>
                                </div>
                            </a>
                            <a href="#">
                                <div class="news-title"> 
                                    <p><b>He Strikes Again!</b> &#187 Stealing a Modem.</p>
                                </div>
                            </a>
                            <a href="#">
                                <div class="news-title"> 
                                    <p><b>He Strikes Again!</b> &#187 Stealing a Modem.</p>
                                </div>
                            </a>
							<a href="#">
                            <div class="news-more"></div>
							</a>
                        </div>   
                     </div>
                </div>-->
				

				
                <div class="clear-fix"></div>
                <div class="content-box half left">
                    <div class="content">
                        <div class="content-container">
                            <div class="content-header red">
                                <p>What's New</p>
                            </div>
								<div class="content-content">
                                 <p>lol</p>
								</div> 
                            </div>
                        </div>  
                    </div>
				</div>
                <div class="content-box half right">
                    <div class="content">
					<div class="content-container">
							<div class="content-header blue">
								<p>{hotelName} Radio</p>
							 </div>
							 <div class="content-content">
								<center>
									<p><script type="text/javascript" src="http://player.wavestreamer.com/cgi-bin/swf.js?id=YJDXJDVWLNRAZ9UG"></script>
									<script type="text/javascript" src=" http://player.wavestreaming.com/?id=YJDXJDVWLNRAZ9UG"></script></p>
								</center>
							 </div>
						</div>
					</div>
				</div>
				
				
                <div class="clear-fix"></div>
		
		
				<!--
				<div class="habblet-container news-promo " style="width: 300px;">		
                            <div class="cbb clearfix notitle ">
                                <div id="newspromo">
                                    <div id="topstories">
                                        <div class="topstory" style="background-image: url({url}/r63/c_images/Top_Story_Images/{newsIMG-1})">
                                            <h4>Latest news</h4>
                                            <h3><a href="{url}/index.php?url=news&id={newsID-1}">{newsTitle-1}</a></h3>
                                            <p class="summary">
                                                {newsCaption-1}
                                            </p>
                                            <p>
                                                <a href="{url}/index.php?url=news&id={newsID-1}">Read more &raquo;</a>
                                            </p>
                                        </div>
                                        <div class="topstory" style="background-image: url({url}/r63/c_images/Top_Story_Images/{newsIMG-2}); display: none">
                                            <h4>Latest news</h4>
                                            <h3><a href="{url}/index.php?url=news&id={newsID-2}">{newsTitle-2}</a></h3>
                                            <p class="summary">
                                                {newsCaption-2}
                                            </p>
                                            <p>
                                                <a href="{url}/index.php?url=news&id={newsID-2}">Read more &raquo;</a>
                                            </p>
                                        </div>
                                        <div id="topstories-nav" style="display: none"><a href="#" class="prev">&laquo; Previous</a><span>1</span> / 3<a href="#" class="next">Next &raquo;</a></div>
                                    </div>
                                    <ul class="widelist">
                                        <li class="even"><a href="{url}/index.php?url=news&id={newsID-1}">{newsTitle-1} &raquo;</a><div class="newsitem-date">{newsDate-1}</div></li>            
                                        <li class="odd"><a href="{url}/index.php?url=news&id={newsID-2}">{newsTitle-2} &raquo;</a><div class="newsitem-date">{newsDate-2}</div></li>            
                                        <li class="last"><a href="/news">More news &raquo;</a></li>            
                                    </ul>
                                </div>
                                <script type="text/javascript">
                                    document.observe("dom:loaded", function() { NewsPromo.init(); });
                                </script>
                            </div>
                        </div>
						-->
            <footer>
			<br />
				<hr />
					<?php include('footer.php'); ?>
            </footer>
        </body>
    </html>