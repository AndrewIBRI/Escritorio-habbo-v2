<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(!isset($body_id)){ $body_id = "landing"; }

?>

<body id="<?php echo $body_id; ?>" class="process-template">

<div id="overlay"></div>

<div id="container">
	<div class="cbb process-template-box clearfix">
		<div id="content">
			<div id="header" class="clearfix">
				<h1><a href="<?php echo $path; ?>"></a></h1>
				<ul class="stats">
					    <li class="stats-online"><span class="stats-fig"><?php echo $online_count; ?></span> <?php echo $shortname; ?>s no Hotel</li> 
				</ul>
			</div>