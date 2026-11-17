<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; } ?>

<?php if($noads == true){ ?>
<?php } elseif($pageid == "10b"){ ?>
<?php } else { ?>
<div id="column3" class="column">

<script type="text/javascript">
HabboView.run();
<script type="text/javascript">
HabboView.run();
</script>
<div class="habblet-container ">
<?php $sql = mysql_query("SELECT * FROM cms_banners ORDER BY id DESC LIMIT 5");
while($row = mysql_fetch_assoc($sql)) { ?>
<?php echo $row['html']; ?>
<?php if($row['banner'] == "" or $row['banner'] == " "){ ?><?php } else { ?><a target="blank" href="<?php echo $row['url']; ?>"><img src="<?php echo $row['banner']; ?>"></a><br /><?php } ?>
<a target="blank" href="<?php echo $row['url']; ?>"><?php echo HoloText($row['text']); ?></a><br />
<?php } ?>
</div>

<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>
<?php } ?>

<!--[if lt IE 7]>
<script type="text/javascript">
Pngfix.doPngImageFix();
</script>
<![endif]-->
    </div>
<div id="footer" class="new_and_improved">
	<p class="footer-links"><a href="mailto:marcoscbarbosa@hotmail.com.br<?php echo $shortname; ?>hotel.com" taget="_new">Anuncie</a> | <a href="<?php echo $path; ?>" target="_new">Contate-nos</a> | <a href="<?php echo $path; ?>/help" target="_new">FAQs</a> | <a href="<?php echo $path; ?>/papers/disclaimer" target="_new">Termos e Condições</a> | <a href="<?php echo $path; ?>/papers/privacy" target="_new">Política de Privacidade</a> | <a href="<?php echo $path; ?>/safety/habbo_way" target="_new"><?php echo $shortname; ?> Etiqueta</a> | <a href="<?php echo $path; ?>/safety/safety_tips">Segurança</a></p>
	<p class="copyright">iHabbo Hotel Copyright © 2012 à 2013 - Todos os Direitos Reservados ao <?php echo $shortname; ?> Hotel | Este Hotel não Pertenece a Sulake Corporation Oy.</a></p>

<script type="text/javascript">
HabboView.run();
</script>