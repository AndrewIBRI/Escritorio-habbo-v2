<?php
if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

$db_host = getenv('DB_HOST') ?: 'nitro-db';
$db_port = getenv('DB_PORT') ?: 3306;
$db_name = getenv('DB_NAME') ?: 'arcturus';
$db_user = getenv('DB_USER') ?: 'arcturus_user';
$db_pass = getenv('DB_PASS') ?: 'HabboUser_0kh9qhJkhYI0Klk444U45VjA';

$pdo = null;
$maxRetries = 10;
$retry = 0;

while ($retry < $maxRetries && $pdo === null) {
    try {
        $pdo = new PDO(
            "mysql:host={$db_host};port={$db_port};dbname={$db_name};charset=utf8mb4",
            $db_user,
            $db_pass,
            [
                PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
                PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
                PDO::ATTR_EMULATE_PREPARES => false
            ]
        );
    } catch (PDOException $e) {
        $retry++;
        if ($retry >= $maxRetries) {
            die("Erro ao conectar ao banco de dados: " . htmlspecialchars($e->getMessage()));
        }
        sleep(1);
    }
}
