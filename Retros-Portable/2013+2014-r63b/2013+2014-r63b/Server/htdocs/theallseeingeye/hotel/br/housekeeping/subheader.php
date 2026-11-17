<?php if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }

if(empty($pagename)){
	$pnme = "Painel de Controle";
} else {
	$pnme = "".$shortname." Painel de Controle - " . $pagename . " ";
}

$search = FilterText($_POST['search']);
$searchheader = FilterText($_POST['searchheader']);

if(isset($_POST['searchname'])){
	if($check = mysql_num_rows($sql = mysql_query("SELECT * FROM users WHERE username = '".mysql_real_escape_string($_POST['searchname'])."' LIMIT 1")) > 0){
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
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Cache-Control" content="no-cache" />
<link rel="shortcut icon" href="<?php echo $path; ?>/theallseeingeye/hotel/br/housekeeping/favicon.ico" />

<script src="<?php echo $path; ?>/web-gallery/static/js/visual.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/fullcontent.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/common.js" type="text/javascript"></script>
<script src="<?php echo $adminpath; ?>/js/hk_js.js" type="text/javascript" ></script>

 <link href="css/modern.css" rel="stylesheet">
    <link href="css/modern-responsive.css" rel="stylesheet">
    <link href="css/site.css" rel="stylesheet" type="text/css">
    <link href="js/google-code-prettify/prettify.css" rel="stylesheet" type="text/css">


<?php if($pageid !== "login"){ ?>

<body>

   <div class="nav-bar bg-color-darken">
        <div class="nav-bar-inner">
 
            <span class="element">Logado como: <?php echo $admin_username; echo" (ID: "; echo $my_id; ?>)</span>
 
            <span class="divider"></span>
 
            <ul class="menu">
                <li data-role="dropdown">
				<li><a href="index/p/home">Principal</a></li>
                    <li><a href="index/p/alert">Alertas</a></li>
			<li><a href="index/p/badgetool">Emblemas</a></li>
            <li><a href="index/p/news">Noticias do Hotel</a></li>
			<li><a href="index/p/campaigns">Novidades</a></li>
			<li><a href="index/p/users">Usuarios do Hotel</a></li>
			<li><a href="index/p/groups">Grupos</a></li>
			<li><a href="index/p/ban">Gerenciar banimentos</a></li>
			<li><a href="index/p/recommended">Da Gerencia</a></li>
			<li><a href="index/p/ranktool">Cargos</a></li>
			<li><a href="index/p/viptool">Ferramentas VIP</a></li>
                    
                </li>
                <li><a /></li>
                <li class="divider"></li>
                <li><a /></li>
            </ul>
        </div>
    </div>

  <div class="horizontal-menu">
        <ul>
            <li class="sub-menu"><a href=""></a></li>
                <ul>
    </div>
	

	
	

<p><p><p>
<div class='outerdiv' id='global-outerdiv'>
<?php } if($pageid !== "login" && $pageid !== "home"){ ?>
<table cellpadding='0' cellspacing='8' width='100%' id='tablewrap'>
<tr> <td width='16%' valign='top' id='leftblock'>
 <div>
<?php @include('menu.php'); ?>
 </div>
 </td>
 <td width='78%' valign='top' id='rightblock'>
 <div>
  </p> </p> </p>
<?php } ?>