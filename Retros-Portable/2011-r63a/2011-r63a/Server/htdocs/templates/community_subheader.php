<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(!isset($pagename) || empty($pagename)){
$pagename = $shortname;
}

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="de" lang="de">
<head>
<base href="<?php echo $path; ?>">
<?php if($pageid == "profile" || $pageid == "com"){ ?>
<?php } ?>
<title><?php echo $shortname; ?> <?php echo $pagename; ?></title>

<script type="text/javascript">
var andSoItBegins = (new Date()).getTime();
</script>

<link rel="shortcut icon" href="<?php echo $path; ?>/web-gallery/v2/images/favicon.ico" type="image/vnd.microsoft.icon" />

<script src="<?php echo $path; ?>/web-gallery/static/js/libs2.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/visual.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/jav.min.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs.js" type="text/javascript"></script>
<?php if($pageid == "5"){ ?>
<script src="<?php echo $path; ?>/habblet/common.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/habblet/fullcontent.js" type="text/javascript"></script>
<?php } else { ?>
<script src="<?php echo $path; ?>/web-gallery/static/js/common.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/fullcontent.js" type="text/javascript"></script>
<?php } ?>
<?php if($pageid == "10"){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/rooms.css" type="text/css">
<?php } ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/style.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/tooltips.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/changepassword.css" type="text/css" />
<?php if($pageid == "5"){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/newcredits.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/newcredits.js" type="text/javascript"></script>
<?php } ?>
<?php if($pageid == "33"){ ?>
<script src="http://promo.contenidopago.com/js/cnt_origi.js" type=text/javascript></script>
<link rel=stylesheet href="http://promo.contenidopago.com/css/cnt_pre.css" type=text/css media=screen />
<?php } ?>


<link rel="alternate" type="application/rss+xml" title="<?php $shortname; ?>: RSS" href="<?php echo $path; ?>/rss.php"/>

<script type="text/javascript">
document.habboLoggedIn = true;
var habboName = "<?php echo $name; ?>";
var habboReqPath = "";
var habboStaticFilePath = "web-gallery/";
var habboImagerUrl = "http://www.habbo.es/habbo-imaging/";
var habboPartner = "Meth0d.org";
window.name = "habboMain";
if (typeof HabboClient != "undefined") { HabboClient.windowName = "uberClientWnd"; }
</script>

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/newcredits.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/newcredits.js" type="text/javascript"></script>

<?php if($pageid == "10b" or $pageid == "forum"){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/myhabbo.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/skins.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/dialogs.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/control.textarea.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/myhabbo.css" type="text/css" />

<script src="<?php echo $path; ?>/web-gallery/static/js/homeview.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/homeauth.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/group.css" type="text/css" />
<style type="text/css">

    #playground, #playground-outer {
	    width: 752px;
	    height: 1360px;
    }

<?php if(isset($groupid)){ ?>

    #playground, #playground-outer {
	    width: 922px;
	    height: 1360px;
    }

<?php } elseif(getHCDays($user_row['id'])){ ?>

    #playground, #playground-outer {
	    width: 922px;
	    height: 1360px;
    }

<?php } else { ?>

    #playground, #playground-outer {
	    width: 752px;
	    height: 1360px;
    }

<?php } ?>


</style>

<script type="text/javascript">
document.observe("dom:loaded", function() { initView(55918, 1478728); });
</script>

<link href="<?php echo $path; ?>/web-gallery/styles/discussions.css" type="text/css" rel="stylesheet"/>
<?php } ?>

<?php if($body_id == "home" || empty($body_id)){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/welcome.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/personal.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/rooms.css" type="text/css" />

<script src="<?php echo $path; ?>/web-gallery/static/js/group.js" type="text/javascript"></script>

<script src="<?php echo $path; ?>/web-gallery/static/js/rooms.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/moredata.js" type="text/javascript"></script>
<?php } ?>

<?php if($pageid == "1" || $pageid == "7"){ ?>
<script src="<?php echo $path; ?>/web-gallery/static/js/habboclub.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/shop_badge.js" type="text/javascript"></script>
<?php } if($pageid == "7.32") {?>
<script src="<?php echo $path; ?>/web-gallery/static/js/habboclub.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/buy_vip.js" type="text/javascript"></script>
<?php } if($pageid == "7.2"){ ?>
<script src="<?php echo $path; ?>/jquery-1.4.3.min.js"></script>
<?php } ?>

<?php if($body_id == "profile"){ ?>
<script src="<?php echo $path; ?>/web-gallery/static/js/settings.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/settings.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/friendmanagement.css" type="text/css" />
<?php } ?>

<?php if($pageid == 1){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/minimail.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/control.textarea.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/minimail.js" type="text/javascript"></script>
<?php } ?>


<!--[if lt IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/ie.css" type="text/css" />
<![endif]-->
<!--[if lt IE 7]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/ie6.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/pngfix.js" type="text/javascript"></script>
<script type="text/javascript">
try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}
</script>

<style type="text/css">
body { behavior: url(web-gallery/csshover.htc); }
</style>
<![endif]-->
<meta name="build" content="63 Build 1821 MyHobba Public Beta" />
</head>