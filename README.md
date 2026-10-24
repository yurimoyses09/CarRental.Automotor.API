# CarRental.Automotor.API

API para gerenciar uma locadora de automóveis: cadastro de clientes e automóveis, aluguéis e pagamentos. Ao registrar um pagamento de aluguel, a API publica um evento no RabbitMQ para que outros sistemas possam reagir a ele.

## Status
- Status do projeto: Em desenvolvimento (protótipo com pagamento simulado)
- Última atualização: 2026-10-23

## Tecnologias
- Linguagem: C# (.NET 8)
- Frameworks / bibliotecas principais:
  - ASP.NET Core Web API
  - Entity Framework Core 8 com Npgsql (PostgreSQL)
  - RabbitMQ.Client 7 (mensageria)
  - Swashbuckle (Swagger)
  - xUnit e Moq (testes)
- Banco de dados: PostgreSQL 15
- Mensageria: RabbitMQ 3 (com painel de gerenciamento)
- Outras ferramentas: Docker, Docker Compose, GitHub Actions

## Funcionalidades
- [x] Cadastro, consulta, atualização e remoção lógica de clientes
- [x] Cadastro, consulta, atualização e remoção lógica de automóveis
- [x] Criação, consulta, atualização e remoção lógica de aluguéis
- [x] Simulação de pagamento de aluguel, com consulta por ID
- [x] Publicação de evento no RabbitMQ a cada pagamento processado
- [ ] Integração com gateway de pagamento real

## Como funciona

### Entidades
- **Client**: nome, idade e documento.
- **Automobile**: modelo, ano de fabricação, placa, tipo de combustível e cor.
- **Rental**: cliente, automóvel, datas de início e previsão de devolução, diária, valor total e status (`Active`, `Completed`, `Canceled`, `Pending`).
- **Payment**: aluguel associado, valor, status (`Pending`, `Approved`, `Declined`) e data do pagamento.

A exclusão é lógica: o registro recebe uma data em `delete_at` e não é removido do banco.

### Fluxo de pagamento
1. O cliente envia `POST /Payment` com o `rental_id` e o `amount`.
2. A API valida o valor (precisa ser maior que zero), verifica se o aluguel existe e confere se o valor é igual ao `total_amount` do aluguel. Se o valor for diferente, o pagamento é recusado com 422 e a mensagem de erro é retornada no campo `title`. Nada é salvo nem publicado.
3. O pagamento é simulado como aprovado: o status vira `Approved` e `paid_at` recebe a data atual.
4. O pagamento é salvo no PostgreSQL.
5. Um evento é publicado na fila `payment.processed` do RabbitMQ.
6. A API responde com `201 Created` e os dados do pagamento.

Exemplo de mensagem publicada na fila:

```json
{
  "PaymentId": 1,
  "RentalId": 1,
  "Amount": 300.00,
  "Status": 2,
  "PaidAt": "2026-10-23T21:00:00+00:00"
}
```

O campo `Status` é numérico (1 = Pending, 2 = Approved, 3 = Declined).

### Endpoints

| Método | Rota | Descrição | Resposta |
|---|---|---|---|
| POST | `/Client` | Cria um cliente | 201 |
| GET | `/Client/{id}` | Consulta um cliente | 200 / 404 |
| PUT | `/{id}` | Atualiza um cliente | 204 / 404 |
| DELETE | `/Client/{id}` | Remove um cliente (lógico) | 204 / 404 |
| POST | `/Automobile` | Cria um automóvel | 201 |
| GET | `/Automobile/{id}` | Consulta um automóvel | 200 / 404 |
| PUT | `/{id}` | Atualiza um automóvel | 204 / 404 |
| DELETE | `/Automobile/{id}` | Remove um automóvel (lógico) | 204 / 404 |
| POST | `/Rental` | Cria um aluguel | 201 |
| GET | `/{id}` | Consulta um aluguel | 200 / 404 |
| PUT | `/{id}` | Atualiza um aluguel | 204 / 404 |
| DELETE | `/Rental/{id}` | Remove um aluguel (lógico) | 204 / 404 |
| POST | `/Payment` | Simula o pagamento de um aluguel e publica o evento | 201 / 400 / 404 / 422 |
| GET | `/Payment/{id}` | Consulta um pagamento | 200 / 404 |

Exemplo de pagamento:

```bash
curl -X POST http://localhost:5000/Payment \
  -H "Content-Type: application/json" \
  -d '{"rental_id": 1, "amount": 300.00}'
```

Resposta:

```json
{
  "id": 1,
  "rental_id": 1,
  "amount": 300.00,
  "status": 2,
  "paid_at": "2026-10-23T21:00:00+00:00"
}
```

Consulta do pagamento:

```bash
curl http://localhost:5000/Payment/1
```

Os erros seguem o formato `ProblemDetails`, retornado pelo `GlobalExceptionMiddleware`: valor inválido gera 400, aluguel inexistente gera 404 e valor diferente do total do aluguel gera 422 com a mensagem no campo `title`.

## Pré-requisitos
- Docker e Docker Compose (para rodar tudo em containers)
- Para rodar a API fora do Docker: .NET SDK 8.0

## Como executar

### Opção 1: tudo com Docker Compose
1. Clone o repositório e entre na pasta:
   ```bash
   git clone https://github.com/yurimoyses09/CarRental.Automotor.API.git
   cd CarRental.Automotor.API
   ```
2. Suba os containers (API, PostgreSQL e RabbitMQ):
   ```bash
   docker compose up --build
   ```
3. Acesse:
   - API e Swagger: http://localhost:5000/swagger
   - Painel do RabbitMQ: http://localhost:15672 (usuário `guest`, senha `guest`)
   - PostgreSQL: `localhost:5432`, banco `AutoMotorsDB`, usuário `admin`, senha `admin`

As migrations são aplicadas automaticamente quando a API inicia.

### Opção 2: API local com banco e RabbitMQ no Docker
1. Suba apenas o banco e o RabbitMQ:
   ```bash
   docker compose up -d database rabbitmq
   ```
2. Rode a API:
   ```bash
   dotnet run --project src/CarRental.Automotor.API
   ```
3. Acesse o Swagger em http://localhost:5194/swagger.

As configurações de desenvolvimento (`appsettings.Development.json`) já apontam para `localhost`.

Para acompanhar os eventos: no painel do RabbitMQ, abra a aba **Queues** e a fila `payment.processed`. Depois de um pagamento, a mensagem aparece em **Get messages**.

## Configuração

| Chave | Onde | Padrão | Descrição |
|---|---|---|---|
| `ConnectionStrings:DefaultConnection` | `appsettings.Development.json` | `localhost:5432` | Conexão com o PostgreSQL |
| `RabbitMq:HostName` | `appsettings.json` | `localhost` | Host do RabbitMQ |
| `RabbitMq:Port` | `appsettings.json` | `5672` | Porta AMQP |
| `RabbitMq:UserName` / `Password` | `appsettings.json` | `guest` / `guest` | Credenciais do RabbitMQ |
| `RabbitMq:PaymentQueueName` | `appsettings.json` | `payment.processed` | Nome da fila de pagamentos |

No Docker Compose, essas chaves são sobrescritas por variáveis de ambiente (por exemplo, `RabbitMq__HostName=rabbitmq`). As credenciais do `compose.yaml` são apenas para desenvolvimento.

## Testes
Os testes unitários ficam em `src/CarRental.Automotor.Tests` e cobrem:
- regras da entidade `Payment` (estado inicial e aprovação);
- `PaymentService` (aprovação, publicação do evento, aluguel inexistente, valor inválido e consulta);
- `PaymentMapper` (conversão entre DTOs e entidade).

Para executar:

```bash
dotnet test src/CarRental.Automotor.API.sln
```

O pipeline de CI executa os testes em cada push para branches `feature/**` e `develop`.

## Estrutura do projeto

```
src/
├── CarRental.Automotor.API/             # Controllers, DTOs, mappers e Program.cs
├── CarRental.Automotor.Application/     # Interfaces, serviços e eventos da aplicação
├── CarRental.Automotor.Domain/          # Entidades e enums
├── CarRental.Automotor.Infrastructure/  # EF Core, repositórios, migrations e RabbitMQ
└── CarRental.Automotor.Tests/           # Testes unitários (xUnit e Moq)
.github/workflows/                       # Pipelines de build, teste e publicação da imagem
Dockerfile                               # Imagem da API
compose.yaml                             # API, PostgreSQL e RabbitMQ
```

## Problemas conhecidos
- As rotas de `GET` e `PUT` de `Rental`, `Client` e `Automobile` usam caminho absoluto (`/{id}`) em vez de `/Rental/{id}`, `/Client/{id}` e `/Automobile/{id}`. Isso não foi alterado neste projeto para não quebrar quem já usa essas rotas.
- O pagamento é salvo antes da publicação do evento e não há outbox. Se o RabbitMQ estiver indisponível, o pagamento fica salvo, mas a requisição retorna erro 500.
- O pagamento é apenas simulado: todo pagamento é aprovado e não há integração com gateway.
- Não há consumidor da fila; a API só publica os eventos.
- Uma conexão com o RabbitMQ é aberta a cada publicação. Isso é simples, mas não é o mais eficiente para produção.

## Licença
A definir.

## Contato
- Autor / Maintainer: Yuri Moysés (yurimoyses09)
- Repositório: https://github.com/yurimoyses09/CarRental.Automotor.API

## Links úteis
- [Documentação do ASP.NET Core](https://learn.microsoft.com/pt-br/aspnet/core/)
- [Documentação do Entity Framework Core](https://learn.microsoft.com/pt-br/ef/core/)
- [Tutoriais do RabbitMQ](https://www.rabbitmq.com/getstarted.html)
- [Documentação do Docker Compose](https://docs.docker.com/compose/)
