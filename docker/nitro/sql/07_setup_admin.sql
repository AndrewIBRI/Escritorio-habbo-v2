-- Admin and Nitro Configuration Seed for Arcturus Morningstar
-- Ensures admin user and websocket settings are properly populated

-- Update default user 1 or insert admin user
INSERT INTO `users` (
    `id`, `username`, `real_name`, `password`, `mail`, `mail_verified`,
    `account_created`, `account_day_of_birth`, `last_login`, `last_online`,
    `motto`, `look`, `gender`, `rank`, `credits`, `pixels`, `points`,
    `online`, `auth_ticket`, `ip_register`, `ip_current`, `machine_id`,
    `home_room`, `secret_key`, `pincode`
) VALUES (
    2, 'admin', 'Administrator', '$2y$10$e0MYzXyjpJS7Pd0RVvHwHeF.LgG9Hsh99w1gHwG.X7OqE09Wz4s/y', 'admin@flowhotel.local', '1',
    UNIX_TIMESTAMP(), 1, UNIX_TIMESTAMP(), UNIX_TIMESTAMP(),
    'Hotel Manager', 'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64', 'M', 7, 100000, 50000, 25000,
    '0', 'ADMIN-SSO-TICKET', '127.0.0.1', '127.0.0.1', '',
    0, NULL, NULL
) ON DUPLICATE KEY UPDATE
    `rank` = 7,
    `credits` = 100000,
    `pixels` = 50000,
    `points` = 25000,
    `auth_ticket` = 'ADMIN-SSO-TICKET';

-- Update user 1 (Systemaccount) as well
UPDATE `users` SET
    `rank` = 7,
    `credits` = 100000,
    `pixels` = 50000,
    `points` = 25000,
    `auth_ticket` = '123'
WHERE `id` = 1;

-- Ensure Nitro WebSocket and asset settings in emulator_settings
INSERT INTO `emulator_settings` (`key`, `value`) VALUES
    ('websockets.whitelist', '*'),
    ('ws.nitro.host', '0.0.0.0'),
    ('ws.nitro.port', '2096'),
    ('ws.nitro.ip.header', 'X-Forwarded-For'),
    ('hotel.view.snow', '0'),
    ('gamedata.figuredata.url', 'http://nitro-web/gamedata/FigureData.json')
ON DUPLICATE KEY UPDATE `value` = VALUES(`value`);
