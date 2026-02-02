# Desafio II Zetta - API de Gestão de Biblioteca

Este projeto é uma API desenvolvida para o controle de empréstimos de livros, clientes e estoque.

## Tecnologias Utilizadas
- **.NET 8** (ASP.NET Core)
- **Entity Framework Core** (Database First)
- **SQL Server**
- **Padrão Repository** com métodos assíncronos (`async/await`)

## Arquitetura
O projeto segue uma estrutura organizada para facilitar a manutenção:
- **Models**: Classes que representam as tabelas do banco de dados.
- **Interfaces**: Contratos que definem as operações dos repositórios.
- **Repositories**: Implementação da lógica de acesso a dados.

## Como rodar o projeto
1. Clone o repositório.
2. Certifique-se de que o SQL Server está rodando.
3. Atualize a `ConnectionString` no arquivo `appsettings.json`.
4. Pressione `F5` no Visual Studio para rodar.