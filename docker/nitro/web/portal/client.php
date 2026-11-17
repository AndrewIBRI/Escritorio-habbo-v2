<?php
require_once __DIR__ . '/db.php';

// 1. If user is already logged in, generate new ticket and redirect to /hotel/
if (!empty($_SESSION['username'])) {
    try {
        $stmt = $pdo->prepare("SELECT * FROM users WHERE username = ? LIMIT 1");
        $stmt->execute([$_SESSION['username']]);
        $user = $stmt->fetch();
        if ($user) {
            $ticket = 'FLOW-SSO-' . bin2hex(random_bytes(16));
            $pdo->prepare("UPDATE users SET auth_ticket = ?, ip_current = ?, last_login = ? WHERE id = ?")
                ->execute([$ticket, $_SERVER['REMOTE_ADDR'] ?? '127.0.0.1', time(), $user['id']]);
            header("Location: /hotel/?sso=" . urlencode($ticket));
            exit;
        }
    } catch (Exception $e) {
        // Fallback to guest creation
    }
}

// 2. If not logged in, auto-create a guest session for seamless 1-click play from /client
$guestSuffix = rand(1000, 9999);
$guestName = "Visitante_" . $guestSuffix;
$ticket = 'FLOW-SSO-' . bin2hex(random_bytes(16));
$now = time();
$ip = $_SERVER['REMOTE_ADDR'] ?? '127.0.0.1';
$hashed = password_hash(bin2hex(random_bytes(8)), PASSWORD_BCRYPT);

try {
    $stmt = $pdo->prepare("
        INSERT INTO users (
            username, real_name, password, mail, mail_verified,
            account_created, account_day_of_birth, last_login, last_online,
            motto, look, gender, rank, credits, pixels, points,
            online, auth_ticket, ip_register, ip_current
        ) VALUES (
            ?, 'Convidado', ?, ?, '1',
            ?, 1, ?, ?,
            'Visitando o Flow Hotel HTML5', 'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64', 'M', 1, 50000, 10000, 500,
            '0', ?, ?, ?
        )
    ");
    $stmt->execute([$guestName, $hashed, "{$guestName}@flowhotel.local", $now, $now, $now, $ticket, $ip, $ip]);
    $_SESSION['username'] = $guestName;
    header("Location: /hotel/?sso=" . urlencode($ticket));
    exit;
} catch (Exception $e) {
    // If DB error, redirect to root portal with error message
    header("Location: /?error=" . urlencode($e->getMessage()));
    exit;
}
