<?php
require_once __DIR__ . '/db.php';

$error = '';
$success = '';

// Check online count
$onlineCount = 0;
try {
    $stmt = $pdo->query("SELECT COUNT(*) AS total FROM users WHERE online = '1'");
    $onlineCount = (int) $stmt->fetchColumn();
} catch (Exception $e) {
    $onlineCount = 0;
}

// Handle Guest Play
if (isset($_POST['action']) && $_POST['action'] === 'guest') {
    $guestName = 'Guest_' . mt_rand(1000, 9999);
    $guestPass = bin2hex(random_bytes(8));
    $hashed = password_hash($guestPass, PASSWORD_BCRYPT);
    $ticket = 'FLOW-GUEST-' . bin2hex(random_bytes(16));
    $now = time();
    $ip = $_SERVER['REMOTE_ADDR'] ?? '127.0.0.1';

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
                'Visitando o Flow Hotel', 'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64', 'M', 1, 50000, 10000, 500,
                '0', ?, ?, ?
            )
        ");
        $stmt->execute([$guestName, $hashed, "{$guestName}@flowhotel.local", $now, $now, $now, $ticket, $ip, $ip]);
        $guestId = $pdo->lastInsertId();

        $_SESSION['user_id'] = $guestId;
        $_SESSION['username'] = $guestName;

        header("Location: /hotel/?sso=" . urlencode($ticket));
        exit;
    } catch (Exception $e) {
        $error = 'Erro ao criar conta de convidado: ' . $e->getMessage();
    }
}

// Handle Registration
if (isset($_POST['action']) && $_POST['action'] === 'register') {
    $username = trim($_POST['username'] ?? '');
    $email = trim($_POST['email'] ?? '');
    $password = $_POST['password'] ?? '';
    $gender = in_array($_POST['gender'] ?? '', ['M', 'F']) ? $_POST['gender'] : 'M';

    if (empty($username) || empty($password)) {
        $error = 'Por favor, preencha o usuário e a senha.';
    } elseif (strlen($username) < 3 || strlen($username) > 20) {
        $error = 'O usuário deve ter entre 3 e 20 caracteres.';
    } elseif (!preg_match('/^[a-zA-Z0-9_\-]+$/', $username)) {
        $error = 'O usuário pode conter apenas letras, números, hífen e underline.';
    } else {
        try {
            $stmt = $pdo->prepare("SELECT id FROM users WHERE username = ?");
            $stmt->execute([$username]);
            if ($stmt->fetch()) {
                $error = 'Este nome de usuário já está em uso.';
            } else {
                $hashed = password_hash($password, PASSWORD_BCRYPT);
                $ticket = 'FLOW-REG-' . bin2hex(random_bytes(16));
                $now = time();
                $ip = $_SERVER['REMOTE_ADDR'] ?? '127.0.0.1';

                $look = ($gender === 'F')
                    ? 'hr-828-42.hd-600-1.ch-665-64.lg-700-64.sh-725-64'
                    : 'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64';

                $stmt = $pdo->prepare("
                    INSERT INTO users (
                        username, real_name, password, mail, mail_verified,
                        account_created, account_day_of_birth, last_login, last_online,
                        motto, look, gender, rank, credits, pixels, points,
                        online, auth_ticket, ip_register, ip_current
                    ) VALUES (
                        ?, ?, ?, ?, '1',
                        ?, 1, ?, ?,
                        'Novo no Flow Hotel HTML5!', ?, ?, 1, 50000, 10000, 500,
                        '0', ?, ?, ?
                    )
                ");
                $mail = !empty($email) ? $email : "{$username}@flowhotel.local";
                $stmt->execute([$username, $username, $hashed, $mail, $now, $now, $now, $look, $gender, $ticket, $ip, $ip]);
                $userId = $pdo->lastInsertId();

                $_SESSION['user_id'] = $userId;
                $_SESSION['username'] = $username;

                header("Location: /hotel/?sso=" . urlencode($ticket));
                exit;
            }
        } catch (Exception $e) {
            $error = 'Erro no registro: ' . $e->getMessage();
        }
    }
}

// Handle Login
if (isset($_POST['action']) && $_POST['action'] === 'login') {
    $username = trim($_POST['username'] ?? '');
    $password = $_POST['password'] ?? '';

    if (empty($username) || empty($password)) {
        $error = 'Por favor, informe seu usuário e senha.';
    } else {
        try {
            $stmt = $pdo->prepare("SELECT id, username, password, rank FROM users WHERE username = ?");
            $stmt->execute([$username]);
            $user = $stmt->fetch();

            $valid = false;
            if ($user) {
                if (password_verify($password, $user['password'])) {
                    $valid = true;
                } elseif (hash('sha256', $password) === $user['password'] || sha1($password) === $user['password'] || md5($password) === $user['password']) {
                    $valid = true;
                } elseif ($password === $user['password'] || ($username === 'admin' && $password === 'admin')) {
                    $valid = true;
                }
            }

            if ($valid) {
                $ticket = 'FLOW-SSO-' . bin2hex(random_bytes(16));
                $ip = $_SERVER['REMOTE_ADDR'] ?? '127.0.0.1';

                $update = $pdo->prepare("UPDATE users SET auth_ticket = ?, ip_current = ?, last_login = ? WHERE id = ?");
                $update->execute([$ticket, $ip, time(), $user['id']]);

                $_SESSION['user_id'] = $user['id'];
                $_SESSION['username'] = $user['username'];

                header("Location: /hotel/?sso=" . urlencode($ticket));
                exit;
            } else {
                $error = 'Usuário ou senha incorretos.';
            }
        } catch (Exception $e) {
            $error = 'Erro no login: ' . $e->getMessage();
        }
    }
}

// User session info
$currentUser = null;
if (!empty($_SESSION['user_id'])) {
    try {
        $stmt = $pdo->prepare("SELECT id, username, motto, look, credits, pixels, points, rank, auth_ticket FROM users WHERE id = ?");
        $stmt->execute([$_SESSION['user_id']]);
        $currentUser = $stmt->fetch();
    } catch (Exception $e) {
        $currentUser = null;
    }
}

// Launch hotel for already logged in user
if (isset($_GET['play']) && $currentUser) {
    $ticket = 'FLOW-SSO-' . bin2hex(random_bytes(16));
    $pdo->prepare("UPDATE users SET auth_ticket = ?, ip_current = ?, last_login = ? WHERE id = ?")
        ->execute([$ticket, $_SERVER['REMOTE_ADDR'] ?? '127.0.0.1', time(), $currentUser['id']]);
    header("Location: /hotel/?sso=" . urlencode($ticket));
    exit;
}
?>
<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Flow Hotel - Habbo HTML5</title>
    <link rel="stylesheet" href="/style.css">
</head>
<body>
    <div class="stars"></div>
    <main class="page-container">
        <header class="header">
            <div class="brand">
                <div class="brand-logo">FLOW</div>
                <div class="brand-subtitle">Habbo Hotel HTML5 Nitro</div>
            </div>
            <div class="online-indicator">
                <span class="online-dot"></span>
                <span class="online-text"><strong><?= $onlineCount ?></strong> Habbos online</span>
            </div>
        </header>

        <?php if (!empty($error)): ?>
            <div class="alert alert-error">
                <?= htmlspecialchars($error) ?>
            </div>
        <?php endif; ?>

        <?php if (!empty($success)): ?>
            <div class="alert alert-success">
                <?= htmlspecialchars($success) ?>
            </div>
        <?php endif; ?>

        <?php if ($currentUser): ?>
            <!-- User is logged in -->
            <section class="card user-card">
                <div class="user-profile">
                    <div class="avatar-preview">
                        <img src="https://www.habbo.com/habbo-imaging/avatarimage?figure=<?= urlencode($currentUser['look']) ?>&direction=2&head_direction=3&gesture=sml&action=wav" alt="Avatar">
                    </div>
                    <div class="user-details">
                        <h2>Olá, <?= htmlspecialchars($currentUser['username']) ?>!</h2>
                        <p class="user-motto"><?= htmlspecialchars($currentUser['motto'] ?: 'Sem missão') ?></p>
                        <div class="user-wallets">
                            <span class="wallet credits">🪙 <?= number_format($currentUser['credits']) ?> Moedas</span>
                            <span class="wallet pixels">💎 <?= number_format($currentUser['pixels']) ?> Duckets</span>
                            <span class="wallet points">⭐ <?= number_format($currentUser['points']) ?> Diamantes</span>
                        </div>
                    </div>
                </div>

                <div class="actions">
                    <a href="/?play=1" class="btn btn-primary btn-large">
                        <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor"><path d="M8 5v14l11-7z"/></svg>
                        Entrar no Hotel (HTML5)
                    </a>
                    <a href="/logout.php" class="btn btn-secondary">Sair</a>
                </div>
            </section>
        <?php else: ?>
            <!-- Login / Register Form -->
            <div class="auth-grid">
                <!-- Login Box -->
                <section class="card">
                    <h2 class="card-title">Entrar na sua conta</h2>
                    <form method="POST" action="/" class="auth-form">
                        <input type="hidden" name="action" value="login">
                        
                        <div class="form-group">
                            <label for="login-username">Nome de Usuário</label>
                            <input type="text" id="login-username" name="username" placeholder="Seu nome no hotel" required autocomplete="username">
                        </div>

                        <div class="form-group">
                            <label for="login-password">Senha</label>
                            <input type="password" id="login-password" name="password" placeholder="Sua senha secreta" required autocomplete="current-password">
                        </div>

                        <button type="submit" class="btn btn-primary btn-full">
                            Entrar no Hotel
                        </button>
                    </form>

                    <div class="separator"><span>ou</span></div>

                    <form method="POST" action="/">
                        <input type="hidden" name="action" value="guest">
                        <button type="submit" class="btn btn-secondary btn-full">
                            🎮 Jogar Agora como Convidado
                        </button>
                    </form>
                </section>

                <!-- Register Box -->
                <section class="card">
                    <h2 class="card-title">Criar nova conta</h2>
                    <form method="POST" action="/" class="auth-form">
                        <input type="hidden" name="action" value="register">

                        <div class="form-group">
                            <label for="reg-username">Escolha seu Nome</label>
                            <input type="text" id="reg-username" name="username" placeholder="Ex: Andrew, Felipe..." required autocomplete="off">
                        </div>

                        <div class="form-group">
                            <label for="reg-email">E-mail (opcional)</label>
                            <input type="email" id="reg-email" name="email" placeholder="seuemail@exemplo.com" autocomplete="email">
                        </div>

                        <div class="form-group">
                            <label for="reg-password">Crie uma Senha</label>
                            <input type="password" id="reg-password" name="password" placeholder="Mínimo 6 caracteres" required autocomplete="new-password">
                        </div>

                        <div class="form-group">
                            <label>Gênero do Avatar</label>
                            <div class="gender-selector">
                                <label class="radio-label">
                                    <input type="radio" name="gender" value="M" checked>
                                    <span>Masculino</span>
                                </label>
                                <label class="radio-label">
                                    <input type="radio" name="gender" value="F">
                                    <span>Feminino</span>
                                </label>
                            </div>
                        </div>

                        <button type="submit" class="btn btn-accent btn-full">
                            Registrar e Jogar (Grátis)
                        </button>
                    </form>
                </section>
            </div>
        <?php endif; ?>

        <footer class="footer">
            <p>Flow Hotel HTML5 • Powered by Arcturus Morningstar &amp; Nitro Client • 100% Nativo sem Flash</p>
            <p class="admin-hint">Conta Administrador padrão: <code>admin</code> / <code>admin</code></p>
        </footer>
    </main>
</body>
</html>
