<?php $is_maintenance = "true"; require_once('../../../../data_classes/data_classes-core.php'); ?>
<link rel="shortcut icon" href="<?php echo $path; ?>/manage/hotel/de/housekeeping/favicon.ico" />
<html>
<head>
<title><?php echo $shortname ;?></title>
<link href="<?php echo $path; ?>/manage/hotel/de/housekeeping/css/error.css" rel="stylesheet" type="text/css">
</head>
<body>
<div class="wrapper">
<div class="contentwrapper">
<div class="content">

<h3>Acceso denegado</h3>

<p><?php echo $remote_ip; ?> - &iexcl;No tienes acceso a esta p&aacute;gina!</p>

<p>
Para m&aacute;s informaci&oacute;n mira el <a href="<?php echo $path; ?>/help">panel de ayuda</a> <br><br><a href="javascript:history.back()"><u>&laquo; Atr&aacute;s</u></a>
</p>
</div></div></div></body></html>