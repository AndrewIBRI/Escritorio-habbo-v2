<?php if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }

if(empty($pagename)){
	$pnme = "Housekeeping Login";
} else {
	$pnme = "".$shortname." - Housekeeping: " . $pagename . " ";
}

$search = FilterText($_POST['search']);
$searchheader = FilterText($_POST['searchheader']);

if(isset($_POST['searchname'])){
	if($check = mysql_num_rows($sql = mysql_query("SELECT * FROM users WHERE username = '".$_POST['searchname']."' LIMIT 1")) > 0){
		$rowid = mysql_fetch_assoc($sql);
		header("location: ".$adminpath."/index/p/users_edit&key=".$rowid['id']."");
	}else{
		echo"<script>alert(\"Der Username ".$_POST['searchname']." konnte nicht gefunden werden!\")</script>";
	}
}

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xml:lang="en" lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
<base href="<?php echo $adminpath; ?>/">
<meta http-equiv="content-type" content="text/html; charset=iso-8859-1" /> 
<title><?php echo $pnme; ?></title>
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Cache-Control" content="no-cache" />
<link rel="stylesheet" href="<?php echo $path; ?>/manage/hotel/de/housekeeping/css/hk_style.css" type="text/css" />


<link rel="shortcut icon" href="<?php echo $path; ?>/manage/hotel/de/housekeeping/favicon.ico" />

<script src="<?php echo $path; ?>/web-gallery/static/js/visual.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/fullcontent.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/common.js" type="text/javascript"></script>
<script src="<?php echo $adminpath; ?>/js/hk_js.js" type="text/javascript" ></script>
<!-- TinyMCE -->
<script type="text/javascript" src="<?php echo $path; ?>/manage/hotel/de/housekeeping//jscripts/tiny_mce/tiny_mce.js"></script>
<script type="text/javascript">
	tinyMCE.init({
		// General options
		mode : "textareas",
		theme : "advanced",
		skin : "o2k7",
		plugins : "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,inlinepopups,autosave",

		// Theme options
		theme_advanced_buttons1 : "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
		theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
		theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
		theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft",
		theme_advanced_toolbar_location : "top",
		theme_advanced_toolbar_align : "left",
		theme_advanced_statusbar_location : "bottom",
		theme_advanced_resizing : true,

		// Example word content CSS (should be your site CSS) this one removes paragraph margins
		content_css : "css/word.css",

		// Drop lists for link/image/media/template dialogs
		template_external_list_url : "<?php echo $path; ?>/manage/hotel/de/housekeeping/lists/template_list.js",
		external_link_list_url : "<?php echo $path; ?>/manage/hotel/de/housekeeping/lists/link_list.js",
		external_image_list_url : "<?php echo $path; ?>/manage/hotel/de/housekeeping/<?php echo $path; ?>/manage/hotel/de/housekeeping/lists/image_list.js",
		media_external_list_url : "<?php echo $path; ?>/manage/hotel/de/housekeeping/lists/media_list.js",

		// Replace values for the template plugin
		template_replace_values : {
			username : "Some User",
			staffid : "991234"
		}
	});
</script>
<!-- /TinyMCE -->

<?php if($pageid !== "login"){ ?>

<body>
<!-- IPDWRAPPER -->
<div id='ipdwrapper'>
<!-- TOP TABS -->

<div class='tabwrap-main'>
<div class='tab<?php if($tab == 1){ echo "on"; } else { echo "off"; } ?>-main'><img src='images/system.png' style='vertical-align:middle' /> <a href='index/p/home'>Inicio</a></div>
<div class='tab<?php if($tab == 3){ echo "on"; } else { echo "off"; } ?>-main'><img src='images/tools.png' style='vertical-align:middle' /> <a href='index/p/news'>Ajustes</a></div>
<div class='tab<?php if($tab == 5){ echo "on"; } else { echo "off"; } ?>-main'><img src='images/members.png' style='vertical-align:middle' /> <a href='index/p/users'>Usu&aacute;rios</a></div>
<div class='taboff-main'><img src='images/members.png' style='vertical-align:middle' /><form id='loginform' action='<?php echo $_SERVER["REQUEST_URI"]; ?>' method='post'>Usu&aacute;rio: <input type='text' name="searchname" value='<?php echo $_POST['searchname']; ?>' /><input type='submit' style='border:1px solid #AAA' value='Buscar' /></form></div>


</div>
<!-- / TOP TABS -->

<div class='sub-tab-strip'>
    <div class='global-memberbar'>
 Conectado como: <?php echo $admin_username; echo" (ID: #"; echo $my_id; ?>)</strong> [
 <a href='<?php echo $path; ?>'>Hotel</a> / | \ <a href="<?php echo $adminpath; ?>/index/p/logout">Sair</a> ]</div>
<div class='navwrap'><img src='<?php echo $adminpath; ?>/images/clock.png' style='vertical-align:middle' /> <?php echo $date_full; ?></div>
</div>



<div class='outerdiv' id='global-outerdiv'>
<?php } if($pageid !== "login" && $pageid !== "home"){ ?>
<table cellpadding='0' cellspacing='8' width='100%' id='tablewrap'>
<tr> <td width='16%' valign='top' id='leftblock'>
 <div>
 <!-- LEFT CONTEXT SENSITIVE MENU -->
<?php @include('menu.php'); ?>
 <!-- / LEFT CONTEXT SENSITIVE MENU -->
 </div>
 </td>
 <td width='78%' valign='top' id='rightblock'>
 <div><!-- RIGHT CONTENT BLOCK -->
<?php } ?>