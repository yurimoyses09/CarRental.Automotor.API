# CarRental.Automotor.API

API para gerenciar uma locadora de automóveis: cadastro de clientes e automóveis, aluguéis e pagamentos. Ao registrar um pagamento de aluguel, a API publica um evento no RabbitMQ para que outros sistemas possam reagir a ele.

## Status
- Status do projeto: Em desenvolvimento (protótipo com pagamento simulado)
- Última atualização: 2026-10-27

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
- [x] Regras de unicidade: documento de cliente, placa de automóvel e pagamento por aluguel
- [ ] Integração com gateway de pagamento real

## Como funciona

### Entidades
- **Client**: nome, idade e documento. O documento é único entre clientes não excluídos.
- **Automobile**: modelo, ano de fabricação, placa, tipo de combustível e cor. A placa é única entre automóveis não excluídos.
- **Rental**: cliente, automóvel, datas de início e previsão de devolução, diária e status (`Active`, `Completed`, `Canceled`, `Pending`). O valor total é calculado pela API: `diária × dias`, em que dias parciais contam como dia inteiro e o mínimo é 1 dia. Se a devolução prevista for anterior à retirada, o aluguel é recusado com 422. O cálculo não considera atrasos nem `actual_end_date`.
- **Payment**: aluguel associado, valor, status (`Pending`, `Approved`, `Declined`) e data do pagamento.

A exclusão é lógica: o registro recebe uma data em `delete_at` e não é removido do banco. Registros excluídos não bloqueiam novos cadastros com o mesmo documento ou placa.

## Regras de negócio

| Regra | Onde é verificada | Resposta se violada |
|---|---|---|
| Valor do pagamento precisa ser maior que zero | `PaymentService` | 400 |
| O aluguel do pagamento precisa existir | `PaymentService` | 404 |
| O valor do pagamento precisa ser igual ao total do aluguel | `PaymentService` | 422 |
| Um aluguel só pode ter um pagamento aprovado | `PaymentService` e índice único no banco | 422 |
| Não pode haver dois clientes com o mesmo documento | `ClientService` e índice único no banco | 422 |
| Não pode haver dois automóveis com a mesma placa | `AutomobileService` e índice único no banco | 422 |
| Devolução prevista não pode ser anterior à retirada | `Rental.CalculateTotalAmount` | 422 |

Cada regra de unicidade é verificada em duas camadas. A aplicação faz a checagem antes de salvar e devolve uma mensagem clara. O banco tem um índice único parcial, que garante a regra mesmo quando duas requisições chegam ao mesmo tempo. Se o banco rejeitar o registro, a violação é convertida no mesmo erro 422.

### Fluxo de pagamento
1. O cliente envia `POST /Payment` com o `rental_id` e o `amount`.
2. A API valida o valor (precisa ser maior que zero), verifica se o aluguel existe, confere se o valor é igual ao `total_amount` do aluguel e se o aluguel ainda não tem pagamento aprovado. Se alguma validação falhar, nada é salvo nem publicado.
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
| POST | `/Client` | Cria um cliente | 201 / 422 |
| GET | `/Client/{id}` | Consulta um cliente | 200 / 404 |
| PUT | `/Client/{id}` | Atualiza um cliente | 204 / 404 / 422 |
| DELETE | `/Client/{id}` | Remove um cliente (lógico) | 204 / 404 |
| POST | `/Automobile` | Cria um automóvel | 201 / 400 / 422 |
| GET | `/Automobile/{id}` | Consulta um automóvel | 200 / 404 |
| PUT | `/Automobile/{id}` | Atualiza um automóvel | 204 / 404 / 422 |
| DELETE | `/Automobile/{id}` | Remove um automóvel (lógico) | 204 / 404 |
| POST | `/Rental` | Cria um aluguel (o total é calculado pela API) | 201 / 422 |
| GET | `/Rental/{id}` | Consulta um aluguel | 200 / 404 |
| PUT | `/Rental/{id}` | Atualiza um aluguel | 204 / 404 |
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

### Erros

Os erros seguem o formato `ProblemDetails`, retornado pelo `GlobalExceptionMiddleware`. O campo `title` traz a mensagem para o cliente.

- **400**: dados inválidos (`ArgumentException`), por exemplo, valor de pagamento não positivo.
- **404**: recurso não encontrado (`KeyNotFoundException`).
- **422**: regra de negócio violada (`BusinessException`). Exemplos: documento ou placa já cadastrados, aluguel já pago, valor diferente do total.
- **500**: erro inesperado. A resposta traz uma mensagem genérica e um `traceId`, que permite localizar a exceção nos logs. O detalhe técnico não é exposto ao cliente.

Exemplo de erro de regra de negócio:

```json
{
  "title": "A client with this document already exists.",
  "status": 422,
  "instance": "/Client"
}
```

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
Os testes unitários ficam em `src/CarRental.Automotor.Tests` e são divididos por área:

- **Domínio** (`Domain/`): regras das entidades. `RentalCalculateTotalAmountTests` cobre o cálculo do valor do aluguel (dias exatos, dia parcial, mínimo de 1 dia e devolução antes da retirada). `PaymentTests` e `RentalTests` cobrem o estado inicial e as transições das entidades.
- **Serviços** (`Services/`): regras da camada de aplicação, com repositórios mockados. `PaymentServiceTests` cobre aprovação, publicação de evento, aluguel inexistente, valor inválido, valor diferente do total e pagamento duplicado. `ClientServiceTests` e `AutomobileServiceTests` cobrem a bloqueio de documento e placa repetidos e o cadastro com dados novos.
- **Mapeamento** (`Mappers/`): conversão entre DTOs e entidades (`PaymentMapperTests`).
- **Middleware** (`Middlewares/`): `GlobalExceptionMiddlewareTests` verifica qual status HTTP e qual mensagem cada tipo de exceção gera, e que o detalhe técnico de um erro 500 não aparece para o cliente.

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
├── CarRental.Automotor.Domain/          # Entidades, enums e exceções de negócio
├── CarRental.Automotor.Infrastructure/  # EF Core, repositórios, migrations, middleware e RabbitMQ
└── CarRental.Automotor.Tests/           # Testes unitários (xUnit e Moq)
.github/workflows/                       # Pipelines de build, teste e publicação da imagem
Dockerfile                               # Imagem da API
compose.yaml                             # API, PostgreSQL e RabbitMQ
```

## Problemas conhecidos
- O pagamento é salvo antes da publicação do evento e não há outbox. Se o RabbitMQ estiver indisponível, o pagamento fica salvo, mas a requisição retorna erro 500.
- O pagamento é apenas simulado: todo pagamento é aprovado e não há integração com gateway.
- Não há consumidor da fila; a API só publica os eventos.
- Uma conexão com o RabbitMQ é aberta a cada publicação. Isso é simples, mas não é o mais eficiente para produção.
- A placa é comparada exatamente como foi digitada. `abc1d23` e `ABC1D23` são aceitas como placas diferentes. A normalização para maiúsculas e sem hífen ainda não foi implementada.
- Um cliente excluído mantém o documento livre, então um novo cadastro com o mesmo documento é permitido. Ainda não existe reativação de registros excluídos.
- As rotas de `Rental`, `Client` e `Automobile` mudaram para o padrão `/{Controller}/{id}`. Clientes que usavam `/{id}` na raiz para PUT ou GET de aluguel precisam ser atualizados.
- Não há autenticação nem autorização nos endpoints.

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
