# Habbo Hotel 6-in-1 Retro Suite (2007 - 2017)
### Self-Hosted, Containerized & Portable Habbo Archive Engine
*(Ambiente Completo Auto-Hospedado, Containerizado e Portátil de Retros Habbo)*

[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![Java](https://img.shields.io/badge/Java-17%2B-ED8B00?logo=openjdk&logoColor=white)](https://openjdk.org/)
[![Mono](https://img.shields.io/badge/Mono-.NET_4.5%2F4.6-white?logo=mono&logoColor=black)](https://www.mono-project.com/)
[![MariaDB](https://img.shields.io/badge/MariaDB-10.5%2F11-003545?logo=mariadb&logoColor=white)](https://mariadb.org/)
[![PHP](https://img.shields.io/badge/PHP-5.6%20%7C%208.2-777BB4?logo=php&logoColor=white)](https://www.php.net/)
[![Easypanel](https://img.shields.io/badge/Easypanel-Supported-10B981)](https://easypanel.io/)

---

## 🇧🇷 Visão Geral (Português)

O **Habbo 6-in-1 Retro Suite** é uma plataforma completa e auto-suficiente que preserva e executa todas as eras de ouro do Habbo Hotel (de 2007 a 2017). O projeto foi totalmente desacoplado e containerizado para rodar em **qualquer servidor Linux via Docker**, no **Easypanel**, ou localmente no **Windows em modo Portátil nativo** através de scripts de 1 clique.

### Principais Características
- **6 Retros Clássicos Prontos para Jogar**:
  1. **2007 - v14 (Kepler)**: Emulador Java de alto desempenho criado por Quackster, banco MariaDB, client Shockwave DCR clássico.
  2. **2009 - v31 (Havana)**: Emulador Java Havana + Servidor Web Java embutido, client Flash e Shockwave híbrido.
  3. **2011 - r63a (Gold Tree / Phoenix)**: Emulador .NET 4.5 rodando sob Mono 6.8 Linux, HobbaCMS e client Flash r63a.
  4. **2012 - r63b (Atom / ButterStorm)**: Emulador .NET 4.0 rodando sob Mono 6.8 Linux, UberCMS e client Flash r63b.
  5. **2013-2014 - r63b (SwiftEmulator)**: Emulador .NET 4.0 (Butterfly Rev 5 Alpha 3), ProjectHabbo CMS e client Flash 2013-2014.
  6. **2015-2017 - r63c (PlusEMU)**: Emulador .NET 4.6 (PlusEMU) adaptado para Linux headless, RevCMS e client Flash moderno PRODUCTION-2017.
- **Zero Conflito de Portas**: Cada retro possui sua faixa exclusiva de portas Web, Game TCP, MUS e Banco de Dados. Você pode executar **1 retro isolado** ou **todos os 6 simultaneamente** no mesmo servidor sem colisões!
- **Seeds de Banco Automatizados**: Todos os schemas SQL e dados iniciais foram extraídos e sanitizados em arquivos `.sql` portáteis, importados automaticamente na inicialização dos containers.
- **Injeção Dinâmica de IP**: Basta configurar `HOST_IP` no `.env` com seu IP local ou domínio da VPS; o sistema injeta as configurações dinamicamente no CMS, FlashVars e emuladores.

---

## 🇺🇸 Overview (English)

The **Habbo 6-in-1 Retro Suite** is an all-in-one archive and execution platform that preserves and runs all golden eras of Habbo Hotel (from 2007 to 2017). It has been completely decoupled and containerized to run on **any Linux server via Docker**, on **Easypanel**, or locally on **Windows in native Portable mode** using one-click launch scripts.

---

## 🌐 Tabela de Portas / Port Mapping Matrix

| Retro Era | Nome / Versão | Emulador | CMS / Web Port | Game TCP Port | MUS / RCON Port | DB Host Port |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **2007** | **v14 (Kepler)** | Java 17 | `8014` | `12321` | `12322` / `12309` | `33061` |
| **2009** | **v31 (Havana)** | Java 17 | `8031` | `12331` | `12332` / `12319` | `33062` |
| **2011** | **r63a (Gold Tree)** | .NET (Mono) | `8061` | `30000` | `30001` | `33063` |
| **2012** | **r63b (Atom/Butter)** | .NET (Mono) | `8062` | `30100` | `30101` | `33064` |
| **2013-14** | **r63b (Swift)** | .NET (Mono) | `8063` | `30200` | `30201` | `33065` |
| **2015-17** | **r63c (PlusEMU)** | .NET (Mono) | `8064` | `30300` | `30301` | `33066` |

---

## 🚀 Como Iniciar / Quick Start

### Pré-requisito Global: Configurar `.env`
Antes de iniciar, crie seu arquivo `.env` a partir do modelo:
```bash
cp .env.example .env
```
Abra o `.env` e defina `HOST_IP`:
- Para testes locais no mesmo computador: `HOST_IP=127.0.0.1`
- Para rede local (LAN): `HOST_IP=192.168.1.X`
- Para VPS ou Nuvem pública: `HOST_IP=SEU_IP_PUBLICO_OU_DOMINIO`

---

### Opção A: Executar via Docker Compose (Recomendado)

#### 1. Iniciar um Retro Individual usando Profiles:
```bash
# Retro 2007 (v14 Kepler):
docker compose --profile v14 up -d

# Retro 2009 (v31 Havana):
docker compose --profile v31 up -d

# Retro 2011 (r63a Gold Tree):
docker compose --profile r63a up -d

# Retro 2012 (r63b Atom/Butter):
docker compose --profile r63b up -d

# Retro 2013-2014 (r63b Swift):
docker compose --profile r63b-swift up -d

# Retro 2015-2017 (r63c PlusEMU):
docker compose --profile r63c up -d
```

#### 2. Iniciar Todos os 6 Retros Concorrentemente:
```bash
docker compose --profile all up -d
```

#### 3. Executar via Arquivos Compose Standalone:
Você também pode rodar cada retro de forma totalmente independente:
```bash
docker compose -f docker-compose.v14.yml up -d
docker compose -f docker-compose.v31.yml up -d
docker compose -f docker-compose.r63a.yml up -d
docker compose -f docker-compose.r63b.yml up -d
docker compose -f docker-compose.r63b-swift.yml up -d
docker compose -f docker-compose.r63c.yml up -d
```

---

### Opção B: Scripts de 1 Clique no Windows (.bat)

Na raiz do repositório, basta dar duplo clique no script da versão desejada:
- `start-v14.bat`: Abre o menu do Habbo 2007 (opção de rodar via Docker ou Windows Nativo Portátil).
- `start-v31.bat`: Abre o menu do Habbo 2009 Havana.
- `start-r63a.bat`: Abre o menu do Habbo 2011 Gold Tree.
- `start-r63b.bat`: Abre o menu do Habbo 2012 Atom / ButterStorm.
- `start-r63b-swift.bat`: Abre o menu do Habbo 2013-2014 Swift.
- `start-r63c.bat`: Abre o menu do Habbo 2015-2017 PlusEMU.
- `scripts/start-all-docker.bat`: Inicia todas as 6 versões no Docker de uma vez só!
- `scripts/stop-all-docker.bat`: Encerra todos os containers Docker.

---

## 🎮 Acessando os Jogos (Navegadores com Flash e Shockwave)

Como o Flash Player e o Adobe Shockwave foram descontinuados nos navegadores modernos, utilize navegadores portáteis compatíveis:
1. **Navegador Portátil Incluído**:
   - Em `Retros-Portable/.Files/` você encontra o `BasiliskPortable[Browser].7z`, um navegador já pré-configurado com os plugins Shockwave e Flash habilitados.
2. **Navegadores Alternativos**:
   - [Pale Moon](https://www.palemoon.org/) com plugin Flash NPAPI / Adobe Shockwave 10/11/12.
   - [Waterfox Classic](https://classic.waterfox.net/).

Ao abrir o navegador, acesse a URL da versão correspondente:
- **v14 (2007)**: `http://localhost:8014`
- **v31 (2009)**: `http://localhost:8031`
- **r63a (2011)**: `http://localhost:8061`
- **r63b (2012)**: `http://localhost:8062`
- **r63b-swift (2013-14)**: `http://localhost:8063`
- **r63c (2015-17)**: `http://localhost:8064`

---

## ☁️ Deploy em Produção (VPS, Easypanel, GCP)

Consulte o guia completo de implantação em [DEPLOYMENT.md](file:///c:/Users/Usuario/Documents/GitHub/Escritorio-habbo-v2/DEPLOYMENT.md) e o guia do Easypanel em [EASYPANEL.md](file:///c:/Users/Usuario/Documents/GitHub/Escritorio-habbo-v2/EASYPANEL.md).

---

## 📂 Estrutura do Repositório / Repository Layout

```
.
├── .env.example                  # Modelo de variáveis de ambiente com portas padrão
├── .gitignore                    # Regras limpas para ignorar logs e binários >100MB
├── docker-compose.yml            # Compose mestre unificado com profiles (v14..r63c, all)
├── docker-compose.v14.yml        # Compose standalone 2007 (v14)
├── docker-compose.v31.yml        # Compose standalone 2009 (v31)
├── docker-compose.r63a.yml       # Compose standalone 2011 (r63a)
├── docker-compose.r63b.yml       # Compose standalone 2012 (r63b)
├── docker-compose.r63b-swift.yml # Compose standalone 2013-14 (r63b Swift)
├── docker-compose.r63c.yml       # Compose standalone 2015-17 (r63c PlusEMU)
├── docker/                       # Dockerfiles, scripts de entrypoint e configs dinâmicas
│   ├── v14/                      # Kepler Java + Apache PHP Shockwave CMS
│   ├── v31/                      # Havana Java Server + Havana Web
│   ├── r63a/                     # Mono 6.8 + PHP 5.6 Apache HobbaCMS
│   ├── r63b/                     # Mono 6.8 + PHP 5.6 Apache UberCMS
│   ├── r63b-swift/               # Mono 6.8 + PHP 5.6 Apache ProjectHabbo CMS
│   └── r63c/                     # Mono 6.8 + PHP 5.6 Apache RevCMS
├── extracted_sql/                # Dumps SQL sanitizados prontos para importação
├── easypanel/                    # Template e especificações de 1 clique para Easypanel
├── scripts/                      # Scripts de inicialização multiplataforma (.bat e .sh)
├── Retros-Portable/              # Arquivos originais portáteis do pacote Windows
├── EASYPANEL.md                  # Guia de implantação no Easypanel
├── DEPLOYMENT.md                 # Guia de implantação em VPS Linux e Nuvem
└── README.md                     # Documentação geral do projeto
```

---

## 📜 Licença e Créditos
- Os emuladores e CMSs clássicos contidos neste repositório são softwares criados por desenvolvedores da comunidade de preservação do Habbo (Quackster, Phoenix Team, Butterfly/Swift Team, PlusEMU Team, etc.).
- Todo o trabalho de desacoplamento, dockerização, injeção dinâmica de variáveis, automação de banco de dados e padronização foi realizado com foco em preservação e facilidade de deploy.
