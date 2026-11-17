# Deploying Habbo Suite on Easypanel
*(Guia de Deploy no Easypanel - Português & English)*

---

## 🇧🇷 Português

O [Easypanel](https://easypanel.io) é um painel moderno baseado em Docker que simplifica o gerenciamento de servidores e aplicações. Todos os retros deste repositório foram preparados para rodar perfeitamente no Easypanel com suporte a proxy reverso e portas TCP/WebSocket.

### 🌟 Recomendado: Stack Moderna 100% HTML5 (Nitro + Arcturus) — Sem Flash!
Se você quer que o hotel funcione diretamente nos navegadores modernos (Brave, Google Chrome, Edge, Safari, Celular) **sem precisar de Adobe Flash Player nem FlashBrowser**, utilize o stack **Nitro HTML5**:

1. No Easypanel, selecione o seu App Compose.
2. Na aba **Source**:
   - Compose File Path: `docker-compose.nitro.yml`
3. Na aba **Environment**:
   ```env
   PRIMARY_DOMAIN=server-flow-hotel.00behz.easypanel.host
   WS_PROTOCOL=wss
   HTTP_PROTOCOL=https
   PORT_NITRO_WEB=8064
   ```
4. Na aba **Domains**:
   - `https://server-flow-hotel.00behz.easypanel.host/` -> `http://server_flow-hotel:8064/`
5. Clique em **Deploy**!
6. Pronto! Ao acessar seu domínio:
   - Página inicial moderna com contagem de jogadores online.
   - Opção de login, registro rápido ou **"Jogar Agora como Convidado"** com 1 clique.
   - O hotel abre direto na tela em canvas HTML5/PixiJS com WebSockets seguros (`wss://`).

---

### Opção: Retros Clássicos com Flash / Shockwave

Para rodar as versões nostálgicas antigas (que necessitam de FlashBrowser ou Shockwave):
Aponte o **Compose File Path** para:
- `docker-compose.nitro.yml` (Moderno HTML5 - Nitro + Arcturus)
- `docker-compose.r63c.yml` (2015-2017 - PlusEMU + RevCMS - requer FlashBrowser)
- `docker-compose.r63b-swift.yml` (2013-2014 - Swift)
- `docker-compose.r63b.yml` (2012 - Atom/ButterStorm)
- `docker-compose.r63a.yml` (2011 - Gold Tree)
- `docker-compose.v31.yml` (2009 - Havana)
- `docker-compose.v14.yml` (2007 - Kepler)

---

## 🇺🇸 English

Easypanel is a modern Docker-powered server manager. All retro versions in this repository are optimized to run on Easypanel.

### 🌟 Recommended: Modern 100% HTML5 Stack (Nitro + Arcturus) — No Flash!
To play directly in modern browsers (Brave, Chrome, Edge, Safari, Mobile) with **no Adobe Flash Player needed**:

1. In Easypanel, select your Compose App.
2. Set **Compose File Path**: `docker-compose.nitro.yml`
3. Under **Environment**:
   ```env
   PRIMARY_DOMAIN=your-domain.easypanel.host
   WS_PROTOCOL=wss
   HTTP_PROTOCOL=https
   PORT_NITRO_WEB=8064
   ```
4. Under **Domains**, map your domain to `http://server_flow-hotel:8064/`
5. Click **Deploy**!
