<div id="promo-box">

    <div id="promo-bullets"></div>

<?php
$news1_5 = mysql_query("SELECT * FROM cms_news_slider ORDER BY ID DESC LIMIT 5") or die(mysql_error());
?>

<?php $i = 0; while($news = mysql_fetch_assoc($news1_5)){ $i++; ?>

        <div class="promo-container" style="background-image: url(<?php echo $news['image_url']; ?>)<?php if($i != '1'){ ?>; display: none<?php } ?>">
            <div class="promo-content-container">
                <div class="promo-content">
                    <div class="title"><?php echo $news['title']; ?></div>
                    <div class="body"><?php echo $news['shortstory']; ?></div>
                </div>
            </div>
            <div class="promo-link-container">

<a href="#" target="_blank" class="twitter-link" onclick="recordOutboundLink('Promo','Niko Twitter Button');"></a>
                <a href="https://www.facebook.com/HabbirOficial" target="_blank" class="facebook-link" onclick="recordOutboundLink('Promo','Niko Facebook Button');"></a>

<div class="enter-hotel-btn">
    <div class="open enter-btn">
            <a href="<?php echo $news['link']; ?>"><?php echo $news['enter_button']; ?><i></i></a>
        <b></b>
    </div>
</div>
            </div>
        </div><?php } ?>

</div>

<script type="text/javascript">    document.observe("dom:loaded", function() { PromoSlideShow.init(); });</script>
