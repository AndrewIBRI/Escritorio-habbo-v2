<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; } ?>

<!--[if lt IE 7]>
<script type="text/javascript">
Pngfix.doPngImageFix();
</script>
<![endif]-->
<?php if($body_id == "faq"){ ?>
<div id="faq-footer" class="clearfix"><p>
<?php } else { ?>
<div id="footer">
<?php } ?>
	<p class="footer-links"><a href="<?php echo $path; ?>">Início</a> | <a href="mailto:<?php echo $Email_admin; ?>">Contate-nos</a> | <a href="http://twitter.com/<?php echo $Twitter_id; ?>">Twitter</a> |<a href="<?php echo $path; ?>/papers/disclaimer" target="_self">Termos e condições</a> | <a href="<?php echo $path; ?>/papers/privacy" target="_self">Politica de Privacidade</a></p>
	<p class="copyright">© 2012 - 2013 <?php echo $shortname; ?> Hotel <b>(Hotel Virtual)</b>. Todos os direitos reservados.<br />Este WebSite não é de propriedade ou operado pela Sulake Corporation Oy | Desenvolved by Orium<br>“Tudo posso naquele que creio”</p>
</div></div>
        </div>
    </div>
</div>

<script type="text/javascript">
HabboView.run();
</script>
</body>
</html>