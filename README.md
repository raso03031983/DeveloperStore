
# DeveloperStore API

A **DeveloperStore** é uma API RESTful desenvolvida em **.NET 8**, usando **Entity Framework Core**, arquitetura **Domain-Driven Design (DDD)**, autenticação via **JWT** e persistência em **SQL Server**. O objetivo é servir como uma solução de backend para gerenciamento de vendas com funcionalidades de autenticação, criação e manutenção de registros de vendas.

---

## 🔧 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- Entity Framework Core
- SQL Server (Docker)
- JWT Authentication
- FluentValidation
- Swagger (OpenAPI)
- Docker & Docker Compose
- xUnit & Moq (para testes)

---

## 📦 Estrutura do Projeto

```
DeveloperStore/
├── DeveloperStore.Sales.API            → API ASP.NET Core
├── DeveloperStore.Sales.Application   → Camada de aplicação (DTOs, serviços, filtros, validações)
├── DeveloperStore.Sales.Domain        → Entidades e interfaces de domínio
├── DeveloperStore.Sales.Infrastructure→ Persistência de dados (EF Core, Context, Mappings)
├── DeveloperStore.Sales.Tests         → Testes unitários e de controller
└── docker-compose.yml                 → Composição dos containers (API + SQL Server)
```

---

## 🚀 Executando com Docker

> A aplicação e o banco de dados podem ser iniciados juntos com Docker Compose.

### 1. Pré-requisitos

- Docker instalado e funcionando corretamente
- Docker Desktop com WSL2 ativo (em Windows)

### 2. Build e execução

```bash
docker-compose up --build
```

A API estará disponível em:

```
http://localhost:8080/swagger
```

---

## 🔧 Executando Localmente (sem Docker)

### 1. Pré-requisitos

- .NET 8 SDK
- SQL Server Local ou Azure Data Studio

### 2. Banco de dados

Configure a string de conexão no `appsettings.json`:

```json
"ConnectionStrings": {
  "SalesConnection": "Server=(localdb)\\mssqllocaldb;Database=DeveloperStoreDb;Trusted_Connection=True;"
}
```

Ao rodar a aplicação com `dotnet run`, o banco será criado automaticamente via `EnsureCreated()`.

### 3. Executar a aplicação

```bash
cd DeveloperStore.Sales.API
dotnet run
```

---

## 🔑 Autenticação JWT

### Registro de usuário

**Endpoint:** `POST /api/Auth/register`

```json
{
  "username": "admin",
  "password": "Admin@123"
}
```

### Login

**Endpoint:** `POST /api/Auth/login`

```json
{
  "username": "admin",
  "password": "Admin@123"
}
```

**Resposta:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp..."
}
```

Use esse token no Swagger clicando em **Authorize** e inserindo:

```
Bearer <seu_token>
```

---

## 📘 Endpoints Principais

### Vendas

| Verbo  | Rota               | Descrição                       |
|--------|--------------------|----------------------------------|
| POST   | `/api/sales`       | Cria uma nova venda              |
| GET    | `/api/sales/{id}`  | Retorna venda por ID             |
| GET    | `/api/sales`       | Lista vendas com filtros         |
| PUT    | `/api/sales/{id}`  | Atualiza uma venda               |
| DELETE | `/api/sales/{id}`  | Cancela uma venda                |

> ⚠️ Todos os endpoints de vendas requerem autenticação JWT.

---

## 🧪 Executando os Testes

```bash
dotnet test
```

---

## 🗃️ Banco de Dados

O banco de dados é criado automaticamente na inicialização da aplicação via `EnsureCreated`. Ele será hospedado no container SQL Server com:

- **Server:** `localhost,1433`
- **User:** `sa`
- **Password:** `YourStrong@Password1`
- **Database:** `DeveloperStoreDb`

---

## 📌 Observações

- O projeto segue o padrão DDD, com separação clara de responsabilidades.
- Os testes cobrem os controllers principais.
- O Swagger está habilitado apenas em ambiente de desenvolvimento.

---

## 🧑‍💻 Autor

**Rodrigo Alexander**  
Projeto técnico para processo seletivo  
GitHub: [raso03031983](https://github.com/raso03031983)
