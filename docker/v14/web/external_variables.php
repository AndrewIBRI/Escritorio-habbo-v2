<?php
header('Content-Type: text/plain; charset=utf-8');
header('Access-Control-Allow-Origin: *');

$proto = (isset($_SERVER['HTTP_X_FORWARDED_PROTO']) && $_SERVER['HTTP_X_FORWARDED_PROTO'] === 'https') || (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on') ? 'https' : 'http';
$hostHeader = $_SERVER['HTTP_HOST'] ?? 'localhost';
$primaryDomain = getenv('PRIMARY_DOMAIN') ?: '';
$baseUrl = getenv('SITE_URL') ?: ($primaryDomain ? 'https://' . $primaryDomain : $proto . '://' . $hostHeader);

$filePath = __DIR__ . '/dcr/14.1_b8/external_variables.txt';
if (file_exists($filePath)) {
    $content = file_get_contents($filePath);
    echo str_replace('http://localhost', $baseUrl, $content);
} else {
    echo "# external_variables.txt not found\n";
}
