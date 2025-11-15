# Minimal API - Sistema de Veículos
<img align="right" height="200" src="https://github.com/user-attachments/assets/ae80de2a-d443-4e64-ba73-686526d72815">


Projeto desenvolvido para o desafio "Trabalhando com ASP.NET Minimals APIs" da DIO em parceria com Avanade  
Curso: Avanade - Back-end com .NET e IA

Uma API moderna para gerenciamento de veículos com autenticação JWT, desenvolvida em .NET 9 seguindo o padrão de APIs Mínimas.

## Funcionalidades

- ✅ Autenticação JWT com múltiplos perfis (Admin/Editor)
- ✅ CRUD completo de veículos
- ✅ Documentação interativa com Swagger
- ✅ Testes unitários abrangentes
- ✅ Deploy em produção com Nginx
- ✅ CORS configurado para front-end

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/mysql/)
- [Git](https://git-scm.com/)
- Navegador web para acessar o Swagger

## Como Usar

### Acesse a API em produção:
**URL:** http://13.51.250.207  
**Swagger:** http://13.51.250.207/swagger

### Credenciais para teste:
- **Admin:** administrador@teste.com / 123456
- **Editor:** editor@teste.com / 123456

### Exemplo de uso:
```bash
# Login
curl -X POST http://13.51.250.207/administradores/login \
  -H "Content-Type: application/json" \
  -d '{"email":"administrador@teste.com","senha":"123456"}'

# Listar veículos (use o token retornado)
curl -X GET http://13.51.250.207/veiculos \
  -H "Authorization: Bearer seu-token-jwt"
```


## Tecnologias e Versões

- .NET 9.0.111 - Framework principal
- Entity Framework Core 9.0.9 - ORM para banco de dados
- Pomelo.EntityFrameworkCore.MySql 9.0.0 - Provedor MySQL para EF Core
- MySQL 8.0.43 - Banco de dados relacional
- JWT Bearer Authentication 9.0.9 - Autenticação e autorização
- Swagger/OpenAPI 9.0.6 - Documentação interativa
- Nginx 1.24.0 - Proxy reverso
- Ubuntu 24.04 LTS - Sistema operacional do servidor

## Estrutura do Projeto
```
minimal-api/
├── 📂 API/                 # Projeto principal
│   ├── 📂 Dominio/         # Entidades, DTOs, Interfaces
│   ├── 📂 Servicos/        # Regras de negócio
│   ├── 📂 Infraestrutura/  # DbContext e Migrations
│   ├── 📄 Program.cs                      # Ponto de entrada
│   ├── 📄 Startup.cs                      # Configuração principal ⭐
│   └── 📄 README-API.md                   # Guia técnico
├── 📂 Test/                # Testes unitários
│   ├── 📂 Mocks/           # Implementações mockadas
│   ├── 📂 Helpers/         # Configuração de testes
│   ├── 📂 Requests/        # Testes de endpoints
│   └── 📂 Domain/          # Testes de serviços
└── 📄 README.md           # Documentação principal

```
## Arquitetura em Camadas

```
┌─────────────────────────────────────────────────┐
│ API (Startup.cs)                                │ ← Apresentação
├─────────────────────────────────────────────────┤
│ Servicos (AdministradorServico, VeiculoServico) │ ← Regras de Negócio
├─────────────────────────────────────────────────┤
│ Dominio (Entidades, DTOs, Interfaces)           │ ← Domínio
├─────────────────────────────────────────────────┤
│ Infraestrutura (DbContext, Migrations)          │ ← Acesso a Dados
└─────────────────────────────────────────────────┘
```
## Desenvolvimento
```bash
# Clone o repositório
git clone https://github.com/Gracielibr/minimal-api.git

# Entre na pasta do projeto
cd minimal-api/API

# Restaure as dependências
dotnet restore

# Execute as migrations do banco
dotnet ef database update

# Execute a aplicação
dotnet run
```

### Acesse: http://localhost:5237/swagger

## Documentação Completa

Para um guia técnico detalhado com explicações de arquitetura, 
soluções de problemas e passo a passo educacional, acesse:

[**Guia Técnico Detalhado**](./API/README.md)
