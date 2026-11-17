# Habbo 6-in-1 Suite - Production Deployment Guide
*(Guia de Implantação em Produção - Português & English)*

---

## 🇧🇷 Português

Este guia detalha como implantar qualquer um ou todos os 6 retros Habbo (2007-v14 até 2017-r63c) em ambientes de produção: VPS Linux (Ubuntu/Debian), Easypanel, e Google Cloud Run / GCP VM.

---

### 1. Requisitos de Infraestrutura

- **Sistema Operacional**: Ubuntu 22.04 LTS ou Debian 12 (Recomendado).
- **Recursos Mínimos**:
  - Para rodar **1 retro individual**: 1 vCPU, 1 GB RAM, 10 GB disco.
  - Para rodar **todos os 6 retros simultaneamente**: 4 vCPU, 8 GB RAM, 40 GB disco.
- **Docker & Docker Compose**:
  ```bash
  curl -fsSL https://get.docker.com | sh
  sudo usermod -aG docker $USER
  newgrp docker
  docker compose version
  ```

---

### 2. Liberação de Portas no Firewall (UFW / Cloud Provider)

Libere as portas necessárias no firewall da sua nuvem (AWS Security Group, Oracle Cloud VCN Ingress, GCP Firewall Rules, etc.) ou no UFW:

```bash
# HTTP Web CMS (Cliente / Avatares / DCR / SWF)
sudo ufw allow 8014/tcp   # v14 (2007)
sudo ufw allow 8031/tcp   # v31 (2009)
sudo ufw allow 8061/tcp   # r63a (2011)
sudo ufw allow 8062/tcp   # r63b (2012)
sudo ufw allow 8063/tcp   # r63b Swift (2013-14)
sudo ufw allow 8064/tcp   # r63c PlusEMU (2015-17)

# Game TCP Sockets (Conexão do cliente com o Emulador)
sudo ufw allow 12321/tcp  # v14 Game
sudo ufw allow 12331/tcp  # v31 Game
sudo ufw allow 30000/tcp  # r63a Game
sudo ufw allow 30100/tcp  # r63b Game
sudo ufw allow 30200/tcp  # r63b Swift Game
sudo ufw allow 30300/tcp  # r63c PlusEMU Game

# Ativar firewall
sudo ufw enable
```

> **Aviso de Segurança**: As portas dos bancos de dados (`33061` a `33066`) e as portas MUS/RCON não devem ser expostas para a internet pública a menos que você precise de acesso remoto direto via Navicat/HeidiSQL.

---

### 3. Deploy em VPS Linux com Docker Compose

1. Clone o repositório na sua VPS:
   ```bash
   git clone https://github.com/SEU_USUARIO/Escritorio-habbo-v2.git
   cd Escritorio-habbo-v2
   ```

2. Crie e configure o arquivo `.env`:
   ```bash
   cp .env.example .env
   nano .env
   ```
   **Passo essencial**: Altere `HOST_IP` para o IP público da sua VPS ou para o seu domínio (ex: `HOST_IP=203.0.113.50` ou `HOST_IP=habbo.meusite.com`).

3. Inicie o retro desejado usando profiles:
   ```bash
   # Iniciar apenas o v14 (2007 Kepler):
   docker compose --profile v14 up -d --build

   # Iniciar apenas o r63a (2011 Gold Tree):
   docker compose --profile r63a up -d --build

   # Iniciar apenas o r63c (2015-2017 PlusEMU):
   docker compose --profile r63c up -d --build

   # Ou iniciar todos os 6 simultaneamente:
   docker compose --profile all up -d --build
   ```

4. Verifique o status e logs:
   ```bash
   docker compose ps
   docker compose logs -f v14-emulator
   docker compose logs -f r63a-emulator
   ```

---

### 4. Deploy no Easypanel

O [Easypanel](https://easypanel.io) permite gerenciar serviços Docker via interface web moderna:

1. Acesse o Easypanel na porta `:3000` da sua VPS.
2. Crie um novo Projeto (ex: `habbo`).
3. Adicione um serviço **App** -> Selecione **Docker Compose**.
4. Configure a URL do Git do repositório e o arquivo `docker-compose.yml` (ou `docker-compose.v14.yml` para um retro específico).
5. Na aba **Environment**, insira:
   - `HOST_IP`: Seu IP público ou domínio.
   - `MYSQL_ROOT_PASSWORD`: Senha do banco.
6. Configure as portas no painel do Easypanel mapeando as portas públicas para as portas dos containers.
7. Clique em **Deploy**. Veja o guia completo em [EASYPANEL.md](file:///c:/Users/Usuario/Documents/GitHub/Escritorio-habbo-v2/EASYPANEL.md).

---

### 5. Deploy no Google Cloud Run & GCP

O Google Cloud Run é ideal para containers stateless HTTP, mas os emuladores Habbo operam com **conexões TCP binárias de longa duração (sockets raw)**.

#### Arquitetura Recomendada no Google Cloud:
1. **Google Compute Engine (GCE) com Container-Optimized OS (Recomendado)**:
   - Crie uma VM e2-standard-2 (2 vCPU, 8 GB RAM) na GCP.
   - Instale Docker e execute o `docker compose` conforme a seção 3 deste guia.
2. **Cloud Run + VM Híbrida**:
   - Os containers Web/CMS (`v14-web`, `r63a-web`, etc.) podem ser implantados no Google Cloud Run para escalabilidade automática de requisições HTTP e entrega rápida de assets SWF/DCR.
   - Os containers de Banco de Dados (Cloud SQL ou container MariaDB) e os Emuladores TCP rodam em uma VM Compute Engine com IP estático reservado.
   - No Cloud Run, defina a variável `HOST_IP` apontando para o IP público estático da VM Compute Engine onde o emulador TCP está ouvindo.

---

### 6. Configuração de Nginx Reverso com SSL / HTTPS

Para colocar seu retro sob domínio com SSL (Let's Encrypt):

```nginx
# Exemplo de VHost Nginx para Retro 2011 r63a
server {
    listen 80;
    server_name hotel.seusite.com;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl http2;
    server_name hotel.seusite.com;

    ssl_certificate /etc/letsencrypt/live/hotel.seusite.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/hotel.seusite.com/privkey.pem;

    client_max_body_size 50M;

    location / {
        proxy_pass http://127.0.0.1:8061;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto https;
    }
}
```

> **Atenção sobre Flash/Shockwave com HTTPS**: Clientes Flash clássicos conectam via socket TCP direto (`Security.loadPolicyFile` e `Socket.connect`). A porta TCP do jogo (ex: 30000) não utiliza TLS/SSL nativo, portanto a porta TCP deve ser acessível diretamente via IP público ou proxy TCP stream do Nginx.

---

## 🇺🇸 English

This guide describes how to deploy any or all 6 Habbo retros (2007-v14 through 2017-r63c) into production environments: Linux VPS, Easypanel, and Google Cloud.

---

### 1. Infrastructure Requirements

- **OS**: Ubuntu 22.04 LTS or Debian 12.
- **Minimum Sizing**:
  - **Single retro**: 1 vCPU, 1 GB RAM, 10 GB disk.
  - **All 6 retros concurrently**: 4 vCPU, 8 GB RAM, 40 GB disk.
- **Docker Setup**:
  ```bash
  curl -fsSL https://get.docker.com | sh
  sudo usermod -aG docker $USER
  newgrp docker
  docker compose version
  ```

---

### 2. Firewall Rules

Open the required HTTP and TCP game ports in your cloud provider's firewall:

- **Web Ports**: `8014`, `8031`, `8061`, `8062`, `8063`, `8064`
- **Game TCP Ports**: `12321`, `12331`, `30000`, `30100`, `30200`, `30300`

---

### 3. Deploying via Docker Compose

```bash
git clone https://github.com/YOUR_USER/Escritorio-habbo-v2.git
cd Escritorio-habbo-v2
cp .env.example .env
nano .env  # Set HOST_IP to your server public IP or domain
```

Run specific retro profiles:
```bash
docker compose --profile v14 up -d --build
docker compose --profile r63a up -d --build
docker compose --profile all up -d --build
```

---

### 4. Deploying to Easypanel

1. Open your Easypanel console.
2. Create project -> New App -> Docker Compose.
3. Link Git repository, choose `docker-compose.yml`, set `HOST_IP` in Environment, and click Deploy.
See [EASYPANEL.md](file:///c:/Users/Usuario/Documents/GitHub/Escritorio-habbo-v2/EASYPANEL.md) for full instructions.

---

### 5. Google Cloud Deployment

Because Habbo emulators require persistent raw TCP connections, use **Google Compute Engine (GCE)** with Docker Compose for the emulator stack. For hybrid setups, deploy the web CMS onto Cloud Run while the game server runs on a GCE instance with a static external IP.
