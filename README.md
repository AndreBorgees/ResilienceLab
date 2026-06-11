# 🧪 Pedido Resilience Lab

## Objetivo

Laboratório técnico em .NET para estudar resiliência em integrações externas.

O projeto simula uma API de pedidos que precisa se comunicar com uma API externa de pagamentos.

O foco não é construir um sistema produtivo completo, mas entender como uma aplicação se comporta diante de falhas, lentidão, limites de chamada, repetição de requisições e indisponibilidade externa.

---

## Arquitetura

```plaintext
Pedido.API
   ↓
Pedido.Application
   ↓
Pedido.Integrations.Sdk
   ↓
External.Payment.MockApi
```

---

## Estrutura da Solution

```plaintext
src/
├── Services/
│   └── Pedido/
│       ├── Pedido.API
│       ├── Pedido.Application
│       ├── Pedido.Domain
│       └── Pedido.Infra
│
└── Integrations/
    ├── Pedido.Integrations.Sdk
    └── External.Payment.MockApi
```

---

## Projetos

### Pedido.API

Responsável por expor os endpoints HTTP da aplicação.

Também contém recursos ligados à entrada da API, como:

* Idempotência via Attribute/Filter
* Integração com Redis
* Configurações de entrada HTTP

---

### Pedido.Application

Responsável pelos casos de uso da aplicação.

A controller chama a Application, e a Application chama o SDK.

---

### Pedido.Integrations.Sdk

SDK manual responsável por encapsular a comunicação com a API externa de pagamento.

Implementa:

* Client HTTP genérico
* Endpoints tipados
* AuthHandler
* RateLimitHandler
* Retry
* Circuit Breaker
* Tratamento de erro HTTP
* Cache de token

---

### External.Payment.MockApi

API externa simulada.

Usada para testar cenários controlados de falha, sucesso, lentidão, timeout e rate limit.

---

## Funcionalidades implementadas

### Retry

Implementado com Polly.

Usado para repetir chamadas externas em falhas transitórias.

Exemplo:

```plaintext
1ª tentativa falha
↓
aguarda um tempo
↓
tenta novamente
↓
aplica backoff exponencial
```

---

### Circuit Breaker

Implementado com Polly.

Quando a API externa falha repetidamente, o circuito abre por um período configurado.

Enquanto estiver aberto, novas chamadas são bloqueadas rapidamente sem chegar na API externa.

---

### Rate Limit Client-Side

Implementado no SDK com `System.Threading.RateLimiting`.

O objetivo é controlar quantas chamadas o SDK pode fazer para a API externa.

Foi usado um `DelegatingHandler` para interceptar a saída HTTP antes da chamada externa.

---

### Timeout

Configurado para evitar que chamadas externas fiquem aguardando indefinidamente.

---

### CancellationToken

Propagado pelas chamadas para permitir cancelamento controlado das operações.

---

### Cache de Token

Implementado com `IMemoryCache`.

O token da API externa é cacheado usando o tempo de expiração retornado pela autenticação, com margem de segurança.

---

### Tratamento de erro HTTP

O SDK converte erros HTTP externos em exceções específicas.

Exemplos:

```plaintext
429 → ExternalRateLimitException
500 → ExternalServerErrorException
401 → ExternalUnauthorizedException
503 → ExternalServiceUnavailableException
```

---

### Idempotência

Implementada na `Pedido.API` com Attribute/Filter e Redis.

Objetivo:

Evitar que a mesma operação crítica seja processada mais de uma vez.

Exemplo:

```plaintext
Autorizar pagamento
↓
Repetir a mesma requisição
↓
Retornar o mesmo resultado salvo
↓
Evitar duplicidade de processamento
```

A chave usada é enviada no header:

```http
Idempotency-Key: pedido-123-autorizar-pagamento
```

---

## Redis

O Redis é usado para armazenar o estado da idempotência.

Estados possíveis:

```plaintext
Processing
Completed
Failed
```

Também é usado para impedir duas requisições simultâneas com a mesma chave de idempotência.

---

## Docker Compose

O projeto usa Docker Compose para subir o Redis localmente.

Serviços:

* Redis
* Redis Commander

---

## Cenários simulados na Mock API

### Success

Retorna sucesso.

---

### Failure

Retorna erro interno, como HTTP 500.

---

### RateLimit

Retorna HTTP 429 Too Many Requests.

---

### Timeout

Simula uma chamada que demora mais que o tempo máximo permitido.

---

## Fluxo principal

```plaintext
1. A controller recebe a requisição.
2. Se o endpoint for idempotente, o filtro valida a Idempotency-Key.
3. A Application executa o caso de uso.
4. A Application chama o SDK.
5. O SDK passa por handlers e policies.
6. O SDK chama a Mock API externa.
7. A resposta é tratada.
8. Em caso de erro HTTP, o SDK lança exceções específicas.
9. Em caso de sucesso idempotente, a resposta é salva no Redis.
```

Pipeline principal do SDK:

```plaintext
RateLimitHandler
   ↓
AuthHandler
   ↓
Retry
   ↓
Circuit Breaker
   ↓
External.Payment.MockApi
```

---

## O que este laboratório cobre

* Resiliência em chamadas HTTP externas
* Retry com backoff exponencial
* Circuit Breaker
* Rate Limit do lado cliente
* Timeout
* Cancelamento com CancellationToken
* Cache de token
* Idempotência com Redis
* Tratamento de erro HTTP
* Mock de API externa
* Separação entre API, Application e SDK

---

## Observação

Este projeto é um laboratório técnico.

O foco principal é entender comportamento de integrações externas sob falha.

Não é um sistema produtivo completo.
