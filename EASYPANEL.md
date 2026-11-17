# Deploying Habbo 6-in-1 Suite on Easypanel
*(Guia de Deploy no Easypanel - Português & English)*

---

## 🇧🇷 Português

O [Easypanel](https://easypanel.io) é um painel moderno baseado em Docker que simplifica o gerenciamento de servidores e aplicações. Todos os retros deste repositório foram preparados para rodar perfeitamente no Easypanel com suporte a proxy reverso e portas TCP diretas.

### Pré-requisitos
1. Uma VPS com Ubuntu 22.04+ e o Easypanel instalado:
   ```bash
   curl -sSL https://get.easypanel.io | sh
   ```
2. Portas TCP liberadas no Firewall da sua VPS / Cloud Provider (Oracle Cloud, DigitalOcean, AWS, GCP, Hetzner, etc.):
   - Portas HTTP Web: `8014`, `8031`, `8061`, `8062`, `8063`, `8064`
   - Portas TCP Game: `12321`, `12331`, `30000`, `30100`, `30200`, `30300`

---

### Opção 1: Deploy Rápido via Docker Compose no Easypanel (Recomendado)

1. Abra o seu painel do Easypanel.
2. Crie um novo Projeto (ex: `habbo-retros`).
3. Clique em **+ Service** -> Selecione **App** -> Escolha **Docker Compose**.
4. Defina a origem do código como **Git Repository**:
   - Repository URL: a URL do seu repositório no GitHub.
   - Branch: `main` (ou a branch desejada).
   - Compose File Path: `docker-compose.yml`
5. Na aba **Environment**, adicione as variáveis de ambiente necessárias (consulte `.env.example`):
   ```env
   HOST_IP=SEU_IP_PUBLICO_DA_VPS
   MYSQL_ROOT_PASSWORD=sua_senha_segura
   MYSQL_PASSWORD=sua_senha_segura
   ```
   > **Importante**: O `HOST_IP` DEVE ser o IP público da sua VPS ou o seu domínio apontado para a VPS. O cliente Flash e Shockwave usam este IP para conectar no emulador.
6. Se quiser subir apenas uma versão específica (ex: v14 ou r63a), passe o comando ou profile no Easypanel:
   - Compose profiles: `v14` (para 2007) ou `r63a` (para 2011) ou `all` (para todos).
7. Clique em **Deploy**!

---

### Opção 2: Deploy de um Retro Individual

Para rodar apenas um retro específico sem subir containers adicionais:
1. No Easypanel, crie um novo **App (Docker Compose)**.
2. Aponte o Compose File Path para o compose da versão desejada:
   - `docker-compose.v14.yml` (2007 - Kepler)
   - `docker-compose.v31.yml` (2009 - Havana)
   - `docker-compose.r63a.yml` (2011 - Gold Tree)
   - `docker-compose.r63b.yml` (2012 - Atom/ButterStorm)
   - `docker-compose.r63b-swift.yml` (2013-2014 - Swift)
   - `docker-compose.r63c.yml` (2015-2017 - PlusEMU)
3. Configure `HOST_IP=SEU_IP_PUBLICO` nas variáveis de ambiente.
4. Clique em **Deploy**.

---

## 🇺🇸 English

[Easypanel](https://easypanel.io) is a modern Docker-based server management panel. All retros in this repository are pre-configured to run seamlessly on Easypanel with zero port collisions and automated database initialization.

### Prerequisites
1. A Linux VPS with Ubuntu 22.04+ running Easypanel:
   ```bash
   curl -sSL https://get.easypanel.io | sh
   ```
2. Firewall open for HTTP and raw TCP emulator ports on your cloud provider:
   - HTTP Web Ports: `8014`, `8031`, `8061`, `8062`, `8063`, `8064`
   - TCP Game Ports: `12321`, `12331`, `30000`, `30100`, `30200`, `30300`

---

### Step-by-Step Deployment

1. Open your Easypanel dashboard.
2. Create a new Project (e.g. `habbo-retros`).
3. Click **+ Service** -> Select **App** -> Select **Docker Compose**.
4. Set source to **Git Repository**:
   - URL: Your GitHub repository URL
   - Branch: `main`
   - Compose path: `docker-compose.yml` (or `docker-compose.v14.yml` for solo v14)
5. Under **Environment**, configure:
   ```env
   HOST_IP=YOUR_VPS_PUBLIC_IP_OR_DOMAIN
   MYSQL_ROOT_PASSWORD=your_strong_password
   MYSQL_PASSWORD=your_strong_password
   ```
6. Set the Docker Compose profile:
   - `v14` (2007)
   - `v31` (2009)
   - `r63a` (2011)
   - `r63b` (2012)
   - `r63b-swift` (2013-2014)
   - `r63c` (2015-2017)
   - `all` (All retros concurrently)
7. Click **Deploy**. The containers will build, import SQL schemas automatically, and start up healthy!
