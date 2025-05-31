# Loja do Seu Manoel - Desafio Técnico .NET

## Descrição
API para automatizar o empacotamento de pedidos da Loja do Seu Manoel. Recebe uma lista de pedidos com produtos e dimensões, calcula o empacotamento em caixas pré-definidas e retorna a distribuição dos produtos nas caixas. Utiliza ASP.NET Core, Entity Framework Core, SQL Server e Docker.

---

## Pré-requisitos
- [Docker](https://www.docker.com/get-started) instalado
- [Docker Compose](https://docs.docker.com/compose/) instalado

---

## Como rodar o projeto

1. **Clone o repositório:**
   ```sh
   git clone https://github.com/RafaSantana/loja-seu-manoel-api
   cd loja-seu-manoel-api
   ```

2. **Suba os containers:**
   ```sh
   docker-compose up --build -d
   ```
   Isso irá subir a API e o banco SQL Server. As migrations são aplicadas automaticamente ao iniciar a API.

3. **Acesse o Swagger:**
   - Abra o navegador em: [http://localhost:5000/swagger](http://localhost:5000/swagger)
   - Você verá a documentação e poderá testar os endpoints.

---

## Cadastro das caixas padrão
Antes de testar o endpoint de empacotamento, cadastre as caixas padrão no banco. Use um cliente SQL (Azure Data Studio, DBeaver, etc) conectado ao container do banco e execute:

```sql
INSERT INTO Caixas (Nome, Altura, Largura, Comprimento) VALUES
('Caixa 1', 30, 40, 80),
('Caixa 2', 80, 50, 40),
('Caixa 3', 50, 80, 60);
```

---

## Testando o endpoint principal

- Endpoint: `POST /api/pedido/empacotar`
- Exemplo de entrada (JSON):

```json
{
  "pedidos": [
    {
      "produtos": [
        { "nome": "Jogo 1", "altura": 10, "largura": 20, "comprimento": 30 },
        { "nome": "Jogo 2", "altura": 15, "largura": 25, "comprimento": 35 }
      ]
    },
    {
      "produtos": [
        { "nome": "Jogo 3", "altura": 40, "largura": 30, "comprimento": 20 }
      ]
    }
  ]
}
```

- Exemplo de saída (JSON):
```json
{
  "pedidos": [
    {
      "pedidoId": 1,
      "caixas": [
        {
          "nomeCaixa": "Caixa 1",
          "produtos": ["Jogo 1", "Jogo 2"]
        }
      ]
    },
    {
      "pedidoId": 2,
      "caixas": [
        {
          "nomeCaixa": "Caixa 1",
          "produtos": ["Jogo 3"]
        }
      ]
    }
  ]
}
```

---

## Estrutura do projeto
- ASP.NET Core Web API
- Entity Framework Core (migrations automáticas)
- SQL Server rodando em container
- Docker e Docker Compose
- Swagger para documentação e testes

---

## Observações
- O endpoint aceita múltiplos pedidos por requisição.
- As caixas devem ser cadastradas manualmente no banco antes do primeiro uso.
- O empacotamento é feito de forma simples: cada produto é colocado na menor caixa possível.
- O código está pronto para evoluir com autenticação, testes e melhorias na lógica de empacotamento.

---

## Comandos úteis
- Parar os containers:
  ```sh
  docker-compose down
  ```
- Remover volumes (resetar banco):
  ```sh
  docker-compose down -v
  ```

---

## Autor
Desafio técnico realizado por Rafael Santana para a vaga .NET Junior.
