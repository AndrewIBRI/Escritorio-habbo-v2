<?php
$proto = (isset($_SERVER['HTTP_X_FORWARDED_PROTO']) && $_SERVER['HTTP_X_FORWARDED_PROTO'] === 'https') || (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on') ? 'https' : 'http';
$hostHeader = $_SERVER['HTTP_HOST'] ?? 'localhost';
$hostParts = explode(':', $hostHeader);

$primaryDomain = getenv('PRIMARY_DOMAIN') ?: '';
$siteUrl = getenv('SITE_URL') ?: ($primaryDomain ? 'https://' . $primaryDomain : $proto . '://' . $hostHeader);

$rawClientHost = getenv('HOST_IP') ?: ($primaryDomain ?: $hostParts[0]);
// Strip http:// or https:// if present
$cleanClientHost = preg_replace('#^https?://#', '', $rawClientHost);
$cleanClientHost = explode('/', $cleanClientHost)[0];
$cleanClientHost = explode(':', $cleanClientHost)[0];
if ($cleanClientHost === '0.0.0.0' || empty($cleanClientHost)) {
    $cleanClientHost = $hostParts[0];
}
$clientHost = $cleanClientHost;
$gamePort = getenv('PORT_V14_GAME') ?: '12321';
$musPort = getenv('PORT_V14_MUS') ?: '12322';
$sso = isset($_GET['sso']) ? htmlspecialchars($_GET['sso'], ENT_QUOTES, 'UTF-8') : '';
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Habbo 2007 (v14) - Kepler</title>
<style type="text/css">
body { margin: 0; padding: 0; background-color: #000; color: #fff; font-family: Ubuntu, Verdana, Arial, sans-serif; text-align: center; }
#client-container { margin: 20px auto; width: 720px; }
.info-bar { margin: 10px auto; font-size: 13px; color: #aaa; background: #111; padding: 6px 12px; border-radius: 4px; display: inline-block; }
.info-bar span { color: #00c3ff; font-weight: bold; }
</style>
</head>
<body>
<div id="client-container">
<div class="info-bar">Habbo Hotel v14 (2007) | Emulator: <span><?= htmlspecialchars($clientHost) ?>:<?= htmlspecialchars($gamePort) ?></span></div>
<object classid="clsid:166B1BCA-3F9C-11CF-8075-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/director/sw.cab#version=10,8,5,1,0" id="habbo" width="720" height="540">
<param name="src" value="<?= $siteUrl ?>/dcr/14.1_b8/habbo.dcr">
<param name="swRemote" value="swSaveEnabled='true' swVolume='true' swRestart='false' swPausePlay='false' swFastForward='false' swTitle='Habbo Hotel' swContextMenu='true' ">
<param name="swStretchStyle" value="none">
<param name="swText" value="">
<param name="bgColor" value="#000000">
<?php if (!empty($sso)): ?>
<param name="sw6" value="use.sso.ticket=1;sso.ticket=<?= $sso ?>">
<?php endif; ?>
<param name="sw2" value="connection.info.host=<?= htmlspecialchars($clientHost) ?>;connection.info.port=<?= htmlspecialchars($gamePort) ?>">
<param name="sw4" value="connection.mus.host=<?= htmlspecialchars($clientHost) ?>;connection.mus.port=<?= htmlspecialchars($musPort) ?>">
<param name="sw3" value="client.reload.url=<?= $siteUrl ?>/">
<param name="sw1" value="site.url=<?= $siteUrl ?>;url.prefix=<?= $siteUrl ?>">
<param name="sw5" value="external.variables.txt=<?= $siteUrl ?>/external_variables.php;external.texts.txt=<?= $siteUrl ?>/dcr/14.1_b8/external_texts.txt">
<embed src="<?= $siteUrl ?>/dcr/14.1_b8/habbo.dcr" bgColor="#000000" width="720" height="540" swRemote="swSaveEnabled='true' swVolume='true' swRestart='false' swPausePlay='false' swFastForward='false' swTitle='Habbo Hotel' swContextMenu='true'" swStretchStyle="none" swText="" type="application/x-director" pluginspage="http://www.macromedia.com/shockwave/download/"
<?php if (!empty($sso)): ?>
sw6="use.sso.ticket=1;sso.ticket=<?= $sso ?>"
<?php endif; ?>
sw2="connection.info.host=<?= htmlspecialchars($clientHost) ?>;connection.info.port=<?= htmlspecialchars($gamePort) ?>"
sw4="connection.mus.host=<?= htmlspecialchars($clientHost) ?>;connection.mus.port=<?= htmlspecialchars($musPort) ?>"
sw3="client.reload.url=<?= $siteUrl ?>/"
sw1="site.url=<?= $siteUrl ?>;url.prefix=<?= $siteUrl ?>"
sw5="external.variables.txt=<?= $siteUrl ?>/external_variables.php;external.texts.txt=<?= $siteUrl ?>/dcr/14.1_b8/external_texts.txt"></embed>
</object>
</div>
</body>
</html>
