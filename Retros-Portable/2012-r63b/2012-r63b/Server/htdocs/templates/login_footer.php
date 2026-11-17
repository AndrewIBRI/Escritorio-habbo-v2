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

</div></div>
<center>
</center>
        </div>
    </div>
</div>

<script type="text/javascript">
HabboView.run();
</script>

<?php echo $analytics; ?>
</body>
</html>