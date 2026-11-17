<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(empty($pagename) && isset($searchname)){
$pagename = $searchname;
} elseif(empty($pagename)){
$pagename = "Home";
}

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
	<base href="<?php echo $path; ?>/">
	<meta http-equiv="content-type" content="text/html; charset=iso-8859-1" />
	<title><?php echo $shortname; ?> <?php echo HoloText($pagename); ?></title>

<script type="text/javascript">
var andSoItBegins = (new Date()).getTime();
</script>



<link rel="shortcut icon" href="<?php echo $path; ?>/web-gallery/v2/images/favicon.ico" type="image/vnd.microsoft.icon" />
<script src="<?php echo $path; ?>/web-gallery/static/js/visual.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/jav.min.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/common.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/fullcontent.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs2.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/style.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/tooltips.css" type="text/css" />
<link rel="alternate" type="application/rss+xml" title="<?php $shortname; ?>: RSS" href="./rss.php"/>

<script type="text/javascript">
document.habboLoggedIn = true;
var habboName = "<?php echo $name; ?>";
var habboReqPath = "";
var habboStaticFilePath = "<?php echo $path; ?>/web-gallery";
var habboImagerUrl = "http://www.habbo.es/habbo-imaging/";
var habboPartner = "Meth0d.org";
window.name = "habboMain";

</script>

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/myhabbo.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/skins.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/dialogs.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/control.textarea.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/myhabbo.css" type="text/css" />
<link href="<?php echo $path; ?>/web-gallery/styles/myhabbo/assets.css" type="text/css" rel="stylesheet" />
<script src="<?php echo $path; ?>/web-gallery/static/js/homeview.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/homeauth.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/group.css" type="text/css" />
<style type="text/css">

<?php if(isset($groupid)){ ?>

    #playground, #playground-outer {
	    width: 922px;
	    height: 1360px;
    }

<?php } elseif($hc > 0){ ?>

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




<script src="<?php echo $path; ?>/web-gallery/static/js/homeedit.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
<?php if(isset($groupid)){ $xid = $groupid; } else { $xid = $user_row['id']; } ?>
document.observe("dom:loaded", function() { initView(<?php echo $xid . "," . $xid; ?>); });
function isElementLimitReached() {
	if (getElementCount() >= 200) {
		showHabboHomeMessageBox("Error", "Você colocou o número máximo de items na página, portanto não foi possível salvar.", "Fechar");
		return true;
	}
	return false;
}

function cancelEditing(expired) {
	<?php if(!isset($groupid)){ ?>
	location.replace("<?php echo $path; ?>/home/<?php echo $searchname; ?>" + (expired ? "?expired=true" : ""));
	<?php } else { ?>
	location.replace("<?php echo $path; ?>/groups/<?php echo $groupid; ?>" + (expired ? "?expired=true" : ""));
	<?php } ?>
}

function getSaveEditingActionName(){
	return '../myhabbo/savehome.php';
}

function showEditErrorDialog() {
	var closeEditErrorDialog = function(e) { if (e) { Event.stop(e); } Element.remove($("myhabbo-error")); Overlay.hide(); }
	var dialog = Dialog.createDialog("myhabbo-error", "", false, false, false, closeEditErrorDialog);
	Dialog.setDialogBody(dialog, '<p>Oopa! ocorreu um erro, por favor tente novamente</p><p><a href="#" class="new-button" id="myhabbo-error-close"><b>Fechar</b><i></i></a></p><div class="clear"></div>');
	Event.observe($("myhabbo-error-close"), "click", closeEditErrorDialog);
	Dialog.moveDialogToCenter(dialog);
	Dialog.makeDialogDraggable(dialog);
}


function showSaveOverlay() {
	var invalidPos = getElementsInInvalidPositions();
	if (invalidPos.length > 0) {
		showHabboHomeMessageBox("Ocorreu um erro!", "Opa! você não pode colocar nada aqui!", "Fechar");
		$A(invalidPos).each(function(el) { Effect.Pulsate(el); });
		return false;
	} else {
		Overlay.show(null,'Salvar');
		return true;
	}
}
</script>

<meta name="description" content="<?php echo $shortname; ?> Hotel: haz amig@s, únete a la diversión y date a conocer." />
<meta name="keywords" content="<?php echo $shortname; ?> hotel, mundo, virtual, red social, gratis, comunidad, personaje, chat, online, adolescente, roleplaying, unirse, social, grupos, forums, seguro, jugar, juegos, amigos, adolescentes, raros, furni raros, coleccionable, crear, coleccionar, conectar, furni, muebles, mascotas, diseño de salas, compartir, expresión, placas, pasar el rato, música, celebridad, visitas de famosos, celebridades, juegos en línea, juegos multijugador, multijugador masivo" />

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
body { behavior: url(<?php echo $path; ?>/web-gallery/csshover.htc); }
</style>
<![endif]-->
<meta name="build" content="63 Build 1821 MyHobba Public Beta" />
</head>