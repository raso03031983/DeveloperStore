# 🛒 DeveloperStore API

Projeto de avaliação técnica para desenvolvedor C# .NET Backend, com foco em arquitetura DDD, autenticação JWT, testes e boas práticas de design de APIs RESTful.

---

## 📦 Estrutura do Projeto

| Projeto                        | Descrição                                      |
|-------------------------------|------------------------------------------------|
| `DeveloperStore.Sales.API`    | API principal (WebAPI com controllers REST)    |
| `DeveloperStore.Sales.Application` | DTOs, Services, Validations e Interfaces  |
| `DeveloperStore.Sales.Domain` | Entidades de domínio e eventos                 |
| `DeveloperStore.Sales.Infrastructure` | DbContext, Mapeamentos EF Core     |
| `DeveloperStore.Sales.Tests`  | Testes unitários e integração com xUnit        |

---

## 🚀 Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- xUnit (testes)
- Docker

---

## 🔐 Autenticação

A API utiliza autenticação baseada em JWT com registro e login persistidos no banco de dados.

### Exemplo de fluxo:

1. **Registrar usuário**
   ```
   POST /api/Auth/register
   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. **Login**
   ```
   POST /api/Auth/login
   {
     "username": "admin",
     "password": "admin123"
   }
   ```

3. **Header de autenticação:**
   ```
   Authorization: Bearer {token}
   ```

---

## 📚 Endpoints Principais

- `POST /api/Sales` - Cria uma nova venda
- `GET /api/Sales/{id}` - Busca venda por ID
- `GET /api/Sales` - Lista vendas com filtros e paginação
- `PUT /api/Sales/{id}` - Atualiza venda
- `DELETE /api/Sales/{id}` - Cancela venda

---

## 🔍 Filtros disponíveis

- `startDate`, `endDate` (Data)
- `customerName` (Cliente)
- `branchName` (Filial)
- `cancelled` (bool)
- `pageNumber`, `pageSize`
- `orderBy`, `sortDirection`

---

## 🧪 Rodando os Testes

Os testes estão localizados em `DeveloperStore.Sales.Tests`:

```bash
dotnet test
```

---

## 🐳 Rodando com Docker

### Pré-requisitos
- Docker
- SQL Server disponível em `localhost`, ou ajuste no `appsettings.json`

### Build e run
```bash
docker build -t developerstore-api -f DeveloperStore.Sales.API/Dockerfile .
docker run -d -p 8080:80 --name devstore developerstore-api
```

Acesse em: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## ⚙️ Configurações

Veja o arquivo `DeveloperStore.Sales.API/appsettings.json`:

```json
"Jwt": {
  "Key": "sua-chave-super-secreta"
},
"ConnectionStrings": {
  "SalesConnection": "Server=localhost;Database=DeveloperStoreDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 🧠 Diferenciais implementados

- Autenticação JWT persistente (registro e login)
- Validações via FluentValidation
- Eventos de domínio: `SaleCreated`, `SaleCancelled`, etc
- Geração automática de número de venda incremental: `SALE-2025-0001`
- Testes unitários e de integração
- Paginação e ordenação nos endpoints

---

## 📝 Como rodar localmente

```bash
git clone https://github.com/raso03031983/DeveloperStore.git
cd DeveloperStore/DeveloperStore
dotnet build
dotnet ef database update --project DeveloperStore.Sales.Infrastructure
dotnet run --project DeveloperStore.Sales.API
```

Acesse: [https://localhost:7265/swagger](https://localhost:7265/swagger)

---

## 📄 Licença

Projeto de uso educacional e avaliativo.
