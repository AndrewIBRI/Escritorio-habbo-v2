<?php
header('Content-Type: text/plain; charset=utf-8');
header('Access-Control-Allow-Origin: *');

$proto = (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on') ? 'https' : 'http';
$hostHeader = $_SERVER['HTTP_HOST'] ?? 'localhost';
$baseUrl = $proto . '://' . $hostHeader;

$filePath = __DIR__ . '/dcr/14.1_b8/external_variables.txt';
if (file_exists($filePath)) {
    $content = file_get_contents($filePath);
    echo str_replace('http://localhost', $baseUrl, $content);
} else {
    echo "# external_variables.txt not found\n";
}
