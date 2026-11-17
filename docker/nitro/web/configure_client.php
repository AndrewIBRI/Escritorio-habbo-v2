<?php
$domain = getenv('PRIMARY_DOMAIN') ?: '127.0.0.1';
$isSsl = (!in_array($domain, ['127.0.0.1', 'localhost']) && !filter_var($domain, FILTER_VALIDATE_IP));
$wsProto = getenv('WS_PROTOCOL') ?: ($isSsl ? 'wss' : 'ws');
$httpProto = getenv('HTTP_PROTOCOL') ?: ($isSsl ? 'https' : 'http');

$rendererPath = '/var/www/html/hotel/renderer-config.json';
if (file_exists($rendererPath)) {
    $cfg = json_decode(file_get_contents($rendererPath), true);
    if (!is_array($cfg)) {
        $cfg = [];
    }
    $cfg['socket.url'] = "{$wsProto}://{$domain}/ws";
    $cfg['asset.url'] = "{$httpProto}://{$domain}/hotel";
    $cfg['image.library.url'] = "{$httpProto}://{$domain}/hotel/c_images/";
    $cfg['hof.furni.url'] = "{$httpProto}://{$domain}/hotel/dcr/hof_furni";
    $cfg['images.url'] = '${asset.url}/images';
    $cfg['gamedata.url'] = '${asset.url}/gamedata';
    $cfg['sounds.url'] = '${asset.url}/sounds/%sample%.mp3';
    $cfg['external.texts.url'] = [
        '${gamedata.url}/ExternalTexts.json',
        '${gamedata.url}/UITexts.json'
    ];
    $cfg['furnidata.url'] = '${gamedata.url}/FurnitureData.json';
    $cfg['productdata.url'] = '${gamedata.url}/ProductData.json';
    $cfg['avatar.actions.url'] = '${gamedata.url}/HabboAvatarActions.json';
    $cfg['avatar.figuredata.url'] = '${gamedata.url}/FigureData.json';
    $cfg['avatar.figuremap.url'] = '${gamedata.url}/FigureMap.json';
    $cfg['avatar.effectmap.url'] = '${gamedata.url}/EffectMap.json';
    $cfg['avatar.asset.url'] = '${asset.url}/bundled/figure/%libname%.nitro';
    $cfg['avatar.asset.effect.url'] = '${asset.url}/bundled/effect/%libname%.nitro';
    $cfg['furni.asset.url'] = '${asset.url}/bundled/furniture/%libname%.nitro';
    $cfg['pet.asset.url'] = '${asset.url}/bundled/pet/%libname%.nitro';
    $cfg['generic.asset.url'] = '${asset.url}/bundled/generic/%libname%.nitro';

    file_put_contents($rendererPath, json_encode($cfg, JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES));
    echo "[+] Configured renderer-config.json with socket.url={$wsProto}://{$domain}/ws\n";
}

$uiPath = '/var/www/html/hotel/ui-config.json';
if (file_exists($uiPath)) {
    $cfg = json_decode(file_get_contents($uiPath), true);
    if (!is_array($cfg)) {
        $cfg = [];
    }
    $cfg['url.prefix'] = "{$httpProto}://{$domain}";
    file_put_contents($uiPath, json_encode($cfg, JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES));
    echo "[+] Configured ui-config.json with url.prefix={$httpProto}://{$domain}\n";
}
