# Minimal API
## Sistema de Veículos
<img align="right" height="200" src="https://github.com/user-attachments/assets/ae80de2a-d443-4e64-ba73-686526d72815">


Este projeto foi desenvolvido como parte do desafio "Trabalhando com ASP.NET Minimals APIs" da DIO em parceria com a Avanade.  O desafio consistia em replicar um projeto prático, assim recriei o projeto do zero (sem fork) acrescentando atualizações para que o projeto rodasse em ambiente de produção real, utilizando versões atualizadas: .NET 9.0.111, MySQL 8.0.43, Ubuntu 24.04 LTS, Nginx 1.24.0. Adicionei testes unitários tais como: VeiculoTest, VeiculoServicoTest, VeiculoServicoMock, VeiculoRequestsTest e configurei todo o ambiente de produção.
 
### O QUE É ESTE PROJETO:
Este é um sistema de gerenciamento de veículos desenvolvido com ASP.NET Core Minimal API, implementando autenticação JWT, autorização por perfis e operações CRUD completas.

#### Funcionalidades:

- ✅ Autenticação JWT com múltiplos perfis (Admin/Editor)
- ✅ CRUD completo de veículos
- ✅ Documentação interativa com Swagger
- ✅ Testes unitários abrangentes
- ✅ Deploy em produção com Nginx
- ✅ CORS configurado para front-end

____________________________________________________________
## ENTENDENDO O PROJETO

## 1. COMEÇANDO:
Comando: `dotnet new web`
Imagina construir uma casa. Antes de começar, você precisa do terreno e da estrutura inicial. O comando dotnet new web é exatamente isso para o mundo .NET. Quando você digita dotnet new web no terminal, é como encomendar a planta baixa mais simples para uma aplicação web. Ele cria uma pasta com tudo essencial para começar a programar imediatamente.
Deste modo, ele prepara um projeto configurado para rodar na web. O coração é o arquivo Program.cs, com código enxuto, tendo como arquivo principal gerado o Program.cs com código básico:
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Olá, Mundo!");
app.Run();
Assim, cria uma aplicação web que responde "Olá, Mundo!" quando acessada. É nosso "Hello World" para web.
A grande vantagem de utilizaar este comando é a simplicidade total. APIs Mínimas são perfeitas para iniciantes porque você não precisa se preocupar com muitas pastas ou conceitos complexos. Toda a lógica fica em um lugar só no início.

## 2. ADICIONANDO FUNCIONALIDADES: NuGet.org

NuGet é o gerenciador de pacotes oficial do .NET. Pense como uma "loja de aplicativos" ou "biblioteca de peças prontas".
Assim, quando precisa de uma funcionalidade (banco de dados, emails, etc.), em vez de programar do zero, procura um pacote NuGet que já faça isso.
##### Comandos de Instalação:
No terminal, dentro da pasta do projeto, deve utilizar o comando:
- `dotnet add package NomeDoPacote`

##### Pacotes que usei neste projeto:

| Pacote                                      | Versão      | Finalidade                  |
|:-------------------------------------------|:-----------:|:----------------------------|
| Microsoft.EntityFrameworkCore              | 9.0.9       | ORM para banco de dados     |
| Pomelo.EntityFrameworkCore.MySql           | 9.0.0       | Provedor MySQL              |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.9       | Autenticação JWT            |
| Swashbuckle.AspNetCore                     | 9.0.6       | Documentação Swagger        |
| Microsoft.EntityFrameworkCore.Design       | 9.0.9       | Ferramentas EF              |
| Microsoft.EntityFrameworkCore.Tools        | 10.0.0-rc.1 | Comandos EF                 |

Para entender melhor, quando cria o projeto utilizando o dotnet new web já vem com pacotes básicos e conforme precisa de mais coisas, adiciona mais pacotes via NuGet.

## 3. ESTRUTURA DO PROJETO
```
minimal-api/
├── 📂 API/                         # Projeto principal
│   ├── 📂 Dominio/                 # Camada de domínio
│   │   ├── 📂 DTOs/               # Data Transfer Objects
│   │   ├── 📂 Entidades/          # Entidades do banco
│   │   ├── 📂 Enums/              # Enumerações
│   │   ├── 📂 Interfaces/         # Contratos de serviços
│   │   └── 📂 ModelViews/         # Modelos para respostas
│   ├── 📂 Servicos/               # Camada de serviços
│   │   ├── AdministradorServico.cs
│   │   └── VeiculoServico.cs
│   ├── 📂 Infraestrutura/         # Camada de infraestrutura
│   │   └── 📂 Db/
│   │       └── DbContexto.cs
│   ├── 📄 Program.cs                      # Ponto de entrada
│   ├── 📄 Startup.cs                      # Configuração principal ⭐
│   └── 📄 README-API.md                   # Guia técnico
├── 📂 Test/                       # Projeto de testes
│   ├── 📂 Mocks/                  # Serviços mockados
│   │   ├── AdministradorServicoMock.cs
│   │   └── VeiculoServicoMock.cs
│   ├── 📂 Helpers/                # Configuração de testes
│   │   └── Setup.cs
│   ├── 📂 Requests/               # Testes de endpoints
│   │   ├── AdministradorRequestsTest.cs
│   │   └── VeiculoRequestsTest.cs
│   └── 📂 Domain/                 # Testes de domínio
│       ├── AdministradorServicoTest.cs
│       └── VeiculoServicoTest.cs
└── 📄 README.md                   # Documentação principal
```
### Arquitetura da Solução

#### Princípios Arquiteturais Aplicados
- **Separation of Concerns**: Camadas bem definidas (Domínio, Serviços, Infraestrutura)
- **Dependency Injection**: Inversão de controle para testabilidade
- **Repository Pattern**: Abstração do acesso a dados via Entity Framework
- **DTO Pattern**: Segurança na transferência de dados

#### Sistema de Autenticação JWT - Explicação Técnica

O JWT (JSON Web Token) foi implementado seguindo o padrão Bearer Token:
1. **Login**: Cliente envia credenciais para `/administradores/login`
2. **Validação**: Servidor verifica no banco de dados
3. **Geração**: Se válido, cria token com claims (email, perfil)
4. **Retorno**: Token é devolvido ao cliente
5. **Uso**: Cliente envia token no header `Authorization: Bearer {token}`
6. **Validação**: Middleware verifica assinatura e expiração

**Estrutura do Token:**
- Header: Algoritmo (HS256) e tipo (JWT)
- Payload: Claims (email, perfil, exp)
- Signature: Assinatura com chave secreta


### PASTA API

#### Dominio/DTOs (Data Transfer Objects)
Esta pasta contém os objetos de transferência de dados que são usados para receber e enviar informações entre o cliente e a API. Eles servem como uma camada de proteção para suas entidades principais.
- **AdministradorDTO** - Usado para criar novos administradores, contendo email, senha e perfil.
- **LoginDTO** - Especializado apenas para operações de login, com email e senha.
- **VeiculoDTO** - Usado para operações com veículos, contendo nome, marca e ano.

A existência dos DTOs é importante porque evita que você exponha diretamente suas entidades de domínio, permitindo controle sobre quais campos são recebidos e enviados.

#### Dominio/Entidades
Aqui estão as classes que representam as tabelas do banco de dados. Elas são mapeadas diretamente para o Entity Framework.
- **Administrador**: Representa a tabela de administradores no banco, com ID, email, senha e perfil. Os atributos como Key, Required e StringLength são validações e configurações do Entity Framework.
- **Veiculo**: Representa a tabela de veículos, com ID, nome, marca e ano.

Estas entidades são essenciais pois definem a estrutura de dados que será persistida no banco.

#### Dominio/Enums 
Contém definições de tipos enumerados que restringem os valores possíveis para certas propriedades.
- **Perfil**: Define os tipos de perfis disponíveis no sistema (Adm e Editor). 

Isso garante consistência nos dados.

#### Dominio/Interfaces
Aqui estão as interfaces que definem os contratos que os serviços devem implementar. Isso é fundamental para o princípio de inversão de dependência e para permitir mocking nos testes.
- **IAdministradorServico**: Define todas as operações possíveis com administradores.
- **IVeiculoServico**: Define todas as operações possíveis com veículos.

As interfaces permitem que você altere a implementação dos serviços sem afetar o código que os consome.

#### Dominio/ModelViews
Contém classes especializadas para representar dados em cenários específicos, especialmente para respostas da API.
- **AdministradorLogado**: Usado na resposta de login, contendo email, perfil e token JWT.
- **AdministradorModelView**: Versão simplificada do administrador para listagem, sem a senha.
- **Home**: Modelo para a página inicial da API.
- **ErrosDeValidacao**: Estrutura para padronizar respostas de erro de validação.

Estes modelos são importantes porque controlam exatamente quais dados são retornados ao cliente.

#### Dominio/Servicos
Contém a implementação concreta das interfaces de serviço. Aqui está a lógica de negócio real da aplicação.
- **AdministradorServico**: Implementa as operações de administrador usando o Entity Framework.
- **VeiculoServico**: Implementa as operações de veículos com suporte a filtros e paginação.

Estes serviços encapsulam toda a lógica de acesso a dados e regras de negócio.

#### Infraestrutura/Db
- **DbContexto**: Esta é a classe principal do Entity Framework que representa a sessão com o banco de dados. Ela configura as entidades, o mapeamento e fornece acesso às tabelas. O método OnModelCreating é usado para configurar dados iniciais (seed data).

### ARQUIVOS PRINCIPAIS

#### Program.cs
Ponto de entrada da aplicação. Configura e executa o host da aplicação web.

#### Startup.cs
Classe central que configura todos os serviços e o pipeline da aplicação. Ela é dividida em:

- **ConfigureServices**: Registra todos os serviços necessários (autenticação JWT, autorização, CORS, Entity Framework, Swagger) e configura a injeção de dependência.
- **Configure**: Define o pipeline de execução (middlewares) e mapeia todos os endpoints da API.

A Startup configura a autenticação JWT, que é essencial para proteger os endpoints, e define as políticas de autorização baseadas em roles (Adm e Editor). Também configura o Swagger para documentação automática da API.
Os endpoints são organizados por funcionalidade (Home, Administradores, Veículos) e cada um tem suas regras de autorização específicas. Por exemplo, apenas administradores podem deletar veículos, mas editores podem criá-los.
Esta estrutura segue boas práticas de separação de concerns, facilitando a manutenção, teste e evolução do código.

### PASTA TEST 

#### Helpers/Setup
 Classe responsável por configurar o ambiente de testes. Ela inicializa o WebApplicationFactory, configura serviços mockados e cria o HttpClient para os testes. Isso é essencial para ter um ambiente isolado de testes.

#### Mocks
**AdministradorServicoMock e VeiculoServicoMock**: Implementações mockadas dos serviços que usam listas em memória ao invés do banco de dados real. Estes mocks permitem que os testes sejam executados de forma rápida e independente do banco de dados.

#### Requests
**AdministradorRequestsTest e VeiculoRequestsTest**: Classes de teste que verificam se os endpoints da API estão funcionando corretamente. Eles testam diversos cenários como login, CRUD, validações, autorizações e tratamento de erros.


## 4. CONFIGURANDO BANCO DE DADOS
Esta fase é crucial porque transforma nossas classes C# em tabelas reais no banco de dados. É aqui que a mágica do Entity Framework acontece, convertendo objetos em estruturas SQL.

A criação das migrations funciona como um sistema de controle de versão para o banco de dados. Quando executamos o comando dotnet ef migrations add NomeDaMigration, estamos criando um snapshot da estrutura desejada do banco. No exemplo, usamos CriacaoTabelas, que seria a migration inicial criando as tabelas Administradores e Veículos.

Cada migration gerada contém dois métodos essenciais: Up e Down. O método Up define as alterações a serem aplicadas no banco, como criar novas tabelas ou adicionar colunas. O método Down faz exatamente o oposto, permitindo reverter as mudanças se necessário. Isso garante que possamos evoluir nosso esquema de banco de dados de forma controlada e reversível.

O comando dotnet ef database update é onde a teoria vira prática. Ele pega todas as migrations pendentes e as executa sequencialmente no banco de dados real. Se for a primeira execução, ele criará toda a estrutura desde o início. Se houver migrations mais recentes, ele aplicará apenas as alterações necessárias, mantendo os dados existentes intactos.

A estrutura resultante do banco inclui duas tabelas principais. A tabela Administradores armazena as informações de acesso dos usuários, com campos para identificação única, email para login, senha para autenticação e perfil para controle de acesso. A tabela Veículos gerencia o cadastro dos veículos, contendo identificador, nome do modelo, marca fabricante e ano de fabricação.

O acesso ao MySQL via linha de comando é feito através do comando `mysql -h localhost -u root -p`, que solicitará a senha do usuário root. Dentro do ambiente MySQL, temos comandos úteis para inspecionar o banco. O SHOW TABLES lista todas as tabelas existentes no banco selecionado, permitindo verificar se nossas migrations foram aplicadas corretamente. O DESC NomeTabela mostra a estrutura detalhada de uma tabela específica, incluindo tipos de dados, chaves e restrições. Já o SELECT * FROM NomeTabela exibe todos os registros contidos na tabela, útil para verificar se os dados estão sendo persistidos corretamente.

Esta configuração é fundamental para o projeto porque estabelece a base de persistência de dados. Sem ela, nossa API não teria onde armazenar as informações de administradores e veículos, tornando-se apenas uma aplicação em memória que perde todos os dados ao ser reiniciada. Com o banco configurado, garantimos que os dados persistam entre execuções da aplicação e estejam disponíveis para consultas e operações CRUD.

##### Comandos Práticos:
- `dotnet ef migrations add NomeDoArquivoMigration`
O nome do arquivo é o que você quiser para identificar

##### Para aplicar as migrations no banco:
- `dotnet ef database update`

##### Acessando MySQL via terminal:
- `mysql -h localhost -u root -p minimal_api`
Digite a senha quando pedir

##### Comandos úteis dentro do MySQL:
- `SHOW TABLES;` (mostra todas tabelas)
- `DESC Administradores;` (mostra estrutura da tabela)
- `SELECT * FROM administradores;` (mostra dados da tabela)

## 5. ORGANIZANDO CÓDIGO: 
#region é um recurso do C# que funciona como "pastinhas virtuais" dentro do seu código. Ele permite agrupar seções relacionadas de código, criando blocos que podem ser expandidos ou recolhidos no editor Visual Studio.

## 6. DOCUMENTAÇÃO COM SWAGGER
O Swagger foi implementado no projeto para fornecer documentação interativa e automática da API. Ele serve como uma interface web que mostra todos os endpoints disponíveis, como usá-los e quais parâmetros são necessários.

Quando você acessa o Swagger, seja na versão de produção ou local, ele apresenta uma página organizada com todas as rotas da API agrupadas por funcionalidade. Cada endpoint mostra claramente quais métodos HTTP são suportados, quais parâmetros precisa enviar, qual a estrutura do corpo da requisição e quais respostas pode esperar.

A grande vantagem do Swagger é que ele permite testar a API diretamente pela interface, sem precisar usar ferramentas externas como Postman ou Insomnia. Você pode clicar em um endpoint, preencher os dados necessários e executar a requisição para ver a resposta real da API. Isso é extremamente útil durante o desenvolvimento para testar rapidamente se tudo está funcionando corretamente.

No contexto deste projeto de curso, o Swagger foi essencial porque documenta automaticamente todo o trabalho realizado. Ele mostra os endpoints de administradores com o sistema de login JWT, os endpoints de veículos com as operações CRUD completas, e demonstra visualmente como o sistema de autenticação e autorização funciona. Qualquer desenvolvedor que queira usar esta API pode consultar o Swagger para entender rapidamente como integrar com o sistema.

Além disso, o Swagger ajuda a garantir que a API segue padrões consistentes e fornece feedback imediato sobre a estrutura de dados esperada. Ele também serve como uma ferramenta de validação durante o desenvolvimento, pois mostra claramente se algum endpoint não está seguindo as convenções ou se falta documentação em algum parâmetro.

Para usuários finais ou outros desenvolvedores, o Swagger funciona como um manual vivo da API, sempre atualizado com as últimas mudanças e pronto para ser consultado a qualquer momento, tanto no ambiente de desenvolvimento quanto em produção.

### Métodos HTTP suportados:
- GET = Buscar informações (ex: listar veículos)
- POST = Criar coisas novas (ex: adicionar veículo)
- PUT = Atualizar tudo (ex: editar veículo completo)
- PATCH = Atualizar só um pedaço (ex: mudar só preço)
- DELETE = Apagar coisas (ex: excluir veículo)
- HEAD = Ver se existe (ex: checar se veículo existe)
- OPTIONS = Ver o que pode fazer (ex: consultar permissões)

### Dicas para Utilização do Swagger
- Cada combinação de caminho e método HTTP é única no Swagger
- Utilize a anotação @Tag para agrupar endpoints relacionados por funcionalidade
- Documente todas as respostas possíveis para cada endpoint (200 para sucesso, 404 para não encontrado, etc.)
- Mantenha as descrições dos parâmetros claras e objetivas
- Utilize exemplos práticos para ilustrar o uso de cada endpoint
Acesse a API em produção:

### Acesso aos Ambientes
**Produção**:
- API: http://13.51.250.207
- Swagger: http://13.51.250.207/swagger

**Desenvolvimento Local**:
- API: http://localhost:5237
- Swagger: http://localhost:5237/swagger

O Swagger representa a porta de entrada ideal para explorar todas as capacidades desta API, oferecendo uma experiência interativa que facilita tanto o desenvolvimento quanto a integração com outros sistemas.

## 7. TESTES IMPLATADOS
Foi implementada uma suíte abrangente de testes unitários e de integração para garantir a qualidade e confiabilidade do sistema. Os testes foram desenvolvidos para validar tanto a lógica de negócio quanto os endpoints da API, assegurando que todas as funcionalidades funcionem conforme esperado.

**Testes de Serviço - Administrador**
No serviço de Administrador, foram testadas as operações essenciais incluindo a criação de novos administradores, busca por identificador, processo de login com credenciais e listagem com suporte a paginação. Os testes validam o comportamento do sistema em diferentes cenários, garantindo que as regras de negócio sejam aplicadas corretamente e que o sistema de autenticação funcione de maneira segura e eficiente.

**Testes de Serviço - Veículos**
Para o serviço de Veículos, os testes cobriram todo o ciclo de vida dos dados, desde a inclusão e busca por identificador até atualização, exclusão e listagem paginada. Implementou-se também testes específicos para os sistemas de filtro, verificando a funcionalidade de busca por nome, por marca e a combinação de ambos os filtros. Estes testes garantem que a paginação funcione corretamente e que os filtros retornem os resultados esperados de acordo com os parâmetros fornecidos.

**Testes de Entidades**
As entidades principais do sistema, Administrador e Veículo, foram igualmente validadas para garantir que suas propriedades funcionem corretamente. Os testes verificam a integridade dos dados, as validações de campos obrigatórios e as restrições de tamanho e formato, assegurando que apenas dados válidos sejam persistidos no sistema.

**Testes de Integração - Endpoints**
A camada de API foi rigorosamente testada através de testes de integração que simulam requisições HTTP reais. Estes testes verificam o comportamento dos endpoints sob diferentes condições, incluindo cenários de sucesso, falhas de validação, tentativas de acesso não autorizado e operações com dados inexistentes. Os testes de integração garantem que toda a stack da aplicação funcione harmonicamente.

**Testes de Segurança e Autorização**
Foram implementados testes específicos para validar o sistema de segurança, incluindo a verificação de tokens JWT, controle de acesso baseado em perfis e proteção contra acessos não autorizados. Estes testes asseguram que apenas usuários autenticados e autorizados possam acessar os recursos protegidos do sistema.

#### Cobertura de Testes
- **AdministradorServico**: Incluir, BuscaPorId, Login, Todos com paginação
- **VeiculoServico**: Incluir, BuscarPorId, Atualizar, Apagar, Todos com paginação, Filtro por nome, Filtro por marca, Filtros combinados
- **Entidades**: Administrador (propriedades get/set), Veiculo (propriedades get/set)
- **Requests**: Testes de endpoints para Administradores e Veículos
- **Autorização**: Validação de perfis de acesso (Admin vs Editor)

#### Estrutura de Testes
A suíte de testes utiliza o padrão WebApplicationFactory para criar um ambiente de testes isolado, com serviços mockados que simulam o comportamento das camadas de negócio e persistência. Esta abordagem permite a execução de testes rápidos e confiáveis, independentemente de infraestrutura externa como banco de dados ou serviços web.

Os testes foram organizados em categorias específicas para facilitar a manutenção e execução seletiva, permitindo validar componentes individuais ou o sistema como um todo de maneira eficiente e organizada.


## 8. CONFIGURAÇÃO DE PRODUÇÃO E DEPLOY
### Ambiente de Produção na AWS
O deploy foi realizado na Amazon Web Services (AWS) para disponibilizar a API em um ambiente de produção real, acessível publicamente pela internet. A AWS foi escolhida por sua confiabilidade, escalabilidade e conjunto abrangente de serviços em nuvem que garantem a disponibilidade constante da aplicação.
### Configuração do Servidor EC2
Foi provisionada uma instância EC2 (Elastic Compute Cloud) executando Ubuntu 24.04 LTS, que serve como ambiente host para a aplicação. O EC2 fornece capacidade computacional escalável na nuvem, permitindo que a API tenha recursos consistentes e possa ser dimensionada conforme a demanda.

### Passo a Passo do Deploy
#### I. Acesso ao Servidor
```
bash
`ssh -i minimal-api.pem ubuntu@13.51.250.207`
```
O acesso seguro é realizado via SSH utilizando par de chaves, garantindo autenticação criptografada com o servidor.

#### II. Atualização do Sistema
```
bash
`sudo apt update`
`sudo apt upgrade`
```
Atualização completa do sistema operacional para garantir que todos os pacotes estejam nas versões mais recentes e seguras.

#### III. Instalação do .NET 9
```
bash
`sudo add-apt-repository ppa:dotnet/backports`
`sudo apt install dotnet-sdk-9.0`
`dotnet --version`
 ```
Instalação do runtime e SDK do .NET 9, necessário para executar aplicações desenvolvidas com esta versão do framework.

#### IV. Configuração do MySQL
```
bash
`sudo apt install mysql-server`
`sudo systemctl start mysql`
`sudo systemctl enable mysql`
```
Instalação e configuração do MySQL Server como banco de dados relacional para persistência dos dados da aplicação.

#### V. Configuração do Nginx como Proxy Reverso

```
bash
`sudo apt install nginx`
`sudo systemctl start nginx`
`sudo systemctl enable nginx`
```
Instalação do Nginx para atuar como proxy reverso, direcionando requisições da porta 80 (HTTP padrão) para a porta da aplicação.

- **Configuração do proxy no Nginx**:
```
nginx
server {
    listen 80;
    server_name 13.51.250.207;
    
    location / {
        proxy_pass http://localhost:5237;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```
#### VI. Deploy da Aplicação
```
bash
`git clone https://github.com/Gracielibr/minimal-api.git`
`cd minimal-api/API`
`dotnet restore`
`dotnet run`
```
Clone do repositório, restauração de dependências e execução da aplicação no servidor.

#### Configurações de Segurança
**Grupos de Segurança AWS**
Foram configurados grupos de segurança para permitir tráfego apenas nas portas necessárias:
  - Porta 22 (SSH) para administração
  - Porta 80 (HTTP) para acesso à API
  - Porta 443 (HTTPS) para tráfego criptografado

**Firewall do Sistema**
Configuração do UFW (Uncomplicated Firewall) para restringir acesso não autorizado aos serviços do servidor.

#### Monitoramento e Logs
**Systemd Service** 

Criação de serviço systemd para garantir que a aplicação execute continuamente e reinicie automaticamente em caso de falhas:
```
ini
[Unit]
Description=Minimal API Veiculos
After=network.target

[Service]
Type=simple
User=ubuntu
WorkingDirectory=/home/ubuntu/minimal-api/API
ExecStart=/usr/bin/dotnet /home/ubuntu/minimal-api/API/MinimalApi.dll
Restart=always
RestartSec=10

[Install]
WantedBy=multi-user.target
```
**Logs da Aplicação**

Configuração de rotação de logs para monitoramento da saúde da aplicação e detecção de problemas.

#### Processo de Atualização
Para atualizar a aplicação em produção:
```
bash
`cd minimal-api/API`
`git pull origin main`
`dotnet restore`
`dotnet build`
`sudo systemctl restart minimal-api`
```
#### Resultado do Deploy
**API em Produção**
  - URL Principal: http://13.51.250.207
  - Swagger: http://13.51.250.207/swagger
  - Disponibilidade: 24/7
  - Performance: Resposta em milissegundos

**Benefícios do Deploy na AWS**
  - Alta Disponibilidade: Infrastructure designed for uptime
  - Escalabilidade: Capacidade de expandir recursos conforme demanda
  - Segurança: Ambiente protegido com políticas de segurança AWS
  - Backup Automático: Snapshots regulares da instância
  - Monitoramento: Métricas de performance via CloudWatch

#### Validação do Deploy
**Testes Pós-Implantação**
  - Verificação de conectividade com o banco de dados
  - Teste de todos os endpoints via Swagger
  - Validação do sistema de autenticação JWT
  - Confirmação do funcionamento do CORS
  - Verificação de logs por erros ou warnings

**Métricas de Performance**
  - Tempo de resposta das requisições
  - Uso de CPU e memória
  - Conexões simultâneas suportadas
  - Latência de rede

O deploy bem-sucedido na AWS resultou em uma API totalmente funcional e acessível globalmente, demonstrando a maturidade do projeto e sua prontidão para uso em ambiente produtivo. A arquitetura implementada garante confiabilidade, segurança e performance adequadas para suportar usuários reais e integrações com sistemas front-end.

____________________________________________
##  Desafios Técnicos e Soluções

### Desafio 1: Configuração do Proxy Reverso
**Problema**: API não respondia na porta 80
**Solução**: Configuração do Nginx como proxy reverso

### Desafio 2: Migrations Assíncronas
**Problema**: Conflito de versões do Entity Framework
**Solução**: Atualização para versões compatíveis do .NET 9

### Desafio 3: Testes de Integração
**Problema**: Dependência do banco de dados real
**Solução**: Implementação de serviços mockados

### Desafio 4: Autenticação JWT
**Problema**: Configuração correta do middleware de autenticação
**Solução**: Estudo detalhado da documentação Microsoft e implementação passo a passo do fluxo JWT

##  Fluxo de Desenvolvimento Adotado

1. **Análise de Requisitos**: Identificação de entidades e operações
2. **Modelagem de Dados**: Definição das entidades e relacionamentos
3. **Implementação por Camadas**:
   - Domínio (entidades, DTOs, interfaces)
   - Infraestrutura (DbContext, configurações)
   - Serviços (regras de negócio)
   - API (endpoints, autenticação)
4. **Testes**: Desenvolvimento guiado por testes
5. **Documentação**: Swagger e README
6. **Deploy**: Configuração de ambiente de produção

## Lições Aprendidas

### Técnicas
- **APIs Mínimas**: Compreensão das vantagens em simplicidade, performance e redução de boilerplate code
- **JWT**: Domínio da implementação segura de autenticação stateless e gestão de tokens
- **Entity Framework**: Experiência prática com migrations para versionamento controlado do esquema de banco
- **Testes**: Importância dos mocks para testes isolados e confiáveis

### Processo
- **Documentação**: Swagger como ferramenta essencial para documentação automática e sempre atualizada
- **Deploy**: Experiência prática com configuração de servidores e implantação em ambiente AWS
- **Versionamento**: Controle preciso de dependências através do gerenciamento de pacotes NuGet
- **Metodologia Iterativa**: Valor do desenvolvimento incremental com validação contínua

## Conclusão do projeto

### Metodologia Aplicada
Este projeto seguiu uma abordagem de desenvolvimento incremental e iterativa, onde cada funcionalidade foi implementada, testada e validada antes da próxima. A metodologia permitiu:

1. **Aprendizado Progressivo**: Conceitos complexos como autenticação JWT e Entity Framework foram assimilados gradualmente
2. **Validação Contínua**: Feedback imediato através de testes automatizados em cada etapa
3. **Qualidade Garantida**: Cada componente testado individualmente antes da integração
4. **Documentação Paralela**: Swagger atualizado durante o desenvolvimento como documentação viva

### Competências Desenvolvidas
- **Backend .NET**: Domínio completo do ecossistema ASP.NET Core com APIs Mínimas
- **Segurança**: Implementação robusta de autenticação JWT e autorização baseada em perfis
- **Banco de Dados**: Modelagem relacional e ORM com Entity Framework Core e MySQL
- **DevOps**: Experiência prática com deploy, configuração de servidores e gestão de ambiente cloud AWS
- **Qualidade**: Cultura de testes automatizados com xUnit e WebApplicationFactory
- **Arquitetura de Software**: Aplicação de princípios SOLID e padrões arquiteturais em camadas

### Resultados Alcançados
O projeto Minimal API para gerenciamento de veículos representa a conclusão bem-sucedida de um ciclo completo de desenvolvimento de software, desde a concepção até a implantação em ambiente de produção. A implementação demonstra domínio dos conceitos fundamentais de desenvolvimento backend com .NET, incluindo arquitetura em camadas, segurança com autenticação JWT, operações CRUD e integração com banco de dados relacional.

O sistema desenvolvido atende plenamente aos requisitos funcionais propostos, oferecendo uma solução robusta para administração de veículos com controle de acesso baseado em perfis. A arquitetura adotada, seguindo os princípios de APIs Mínimas, resultou em uma aplicação enxuta mas poderosa, com código limpo e de fácil manutenção. A escolha do Entity Framework Core como ORM facilitou a interação com o banco de dados MySQL, enquanto a implementação de migrações garantiu o versionamento controlado do schema.

A validação do projeto vai além do funcionamento básico, com a implementação de uma suíte abrangente de testes que assegura a qualidade do código e a confiabilidade do sistema. A documentação interativa via Swagger fornece uma interface intuitiva para desenvolvedores, facilitando a compreensão e utilização da API. A configuração de CORS adequada permite a integração segura com aplicações front-end, tornando a API verdadeiramente preparada para consumo em cenários reais.

O deploy realizado na AWS coroa o projeto, demonstrando competência em configuração de ambientes de produção e administração de servidores. A aplicação está disponível publicamente, funcionando de forma estável e performática, o que comprova sua maturidade e prontidão para uso em produção.

Este trabalho consolida o aprendizado em desenvolvimento backend com .NET, mostrando proficiência em conceitos avançados como autenticação, autorização, testes automatizados, deploy em cloud e boas práticas de desenvolvimento. O projeto serve como base sólida para evoluções futuras e demonstra capacidade técnica para desenvolver soluções completas e profissionais na plataforma .NET.
