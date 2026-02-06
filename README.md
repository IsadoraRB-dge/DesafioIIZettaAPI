# 📚 Documentação da API - Gestão de Biblioteca

Esta API foi desenvolvida para auxiliar o bibliotecário, oferecendo controle total e centralizado sobre a operação da biblioteca. Através dela, o profissional gerencia pessoalmente todo o fluxo: desde o cadastro detalhado de seus clientes e a organização do acervo de livros, até o registro de cada empréstimo e devolução.

O sistema garante que o bibliotecário tenha domínio total sobre o inventário, atualizando o estoque automaticamente em tempo real, além de fornecer uma ferramenta de tarefas exclusiva para organizar sua rotina administrativa. Tudo isso é operado sob uma camada de segurança que isola os dados, garantindo que o bibliotecário seja o único responsável e gestor de suas informações.

---

## 🔐 Autenticação e Segurança

O acesso à API é restrito e protegido. A segurança é baseada em dois pilares fundamentais:

* **Criptografia de Senhas:** Utilizamos o **BCrypt**. Isso significa que as senhas nunca são salvas em texto puro no banco de dados. Elas passam por um processo de hashing seguro, tornando-as ilegíveis para qualquer pessoa.
* **Tokens JWT (JSON Web Token):** Após o login, a API gera um token de acesso. Esse token garante que apenas você acesse seus dados e deve ser enviado no cabeçalho das requisições.

---

## ⌨️ Tecnologias e Dependências

O projeto foi construído utilizando as versões mais recentes do ecossistema .NET, focando em performance e segurança:

* **Framework:** .NET 8.0
* **Banco de Dados:** SQL Server via Entity Framework Core 8.0
* **Segurança:** Autenticação JWT (Bearer) e criptografia de senhas com BCrypt.Net
* **Mapeamento:** AutoMapper para conversão entre Entidades e DTOs
* **Testes:** Entity Framework InMemory

---

## 🚀 Como Testar a API (Postman)

Para facilitar a sua experiência de teste, disponibilizamos uma coleção completa do **Postman** com todas as rotas e payloads já configurados.

📍 **Onde encontrar:** Vá até a pasta `Docs/` na raiz deste projeto.
📄 **Arquivo:** `Biblioteca.postman_collection.json`

**Como usar:**
1. Abra o seu Postman.
2. Clique no botão **Import**.
3. Arraste o arquivo acima para o Postman.
4. Pronto! Todas as rotas estarão prontas para uso, bastando apenas configurar a URL base do seu ambiente.

---

## 🟢 Endpoints da API

## 🔐 1. GRUPO: ACESSO 

### 1.1 Cadastrar Usuário
- **POST** `/api/Acesso/registrar`
- **Request (JSON):**
```json
{
  "nomeUsuario": "Henrique",
  "emailUsuario": "Henrique@amorzinho.com",
  "senhaUsuario": "1234556"
}
```
- **Response (200 OK):**
```json
{
  "message": "Usuário (Comandante da Biblioteca) registrado com sucesso!"
}
```

### 1.2 Login (Acesso Usuário)
- **POST** `/api/Acesso/login`
- **Request (JSON):**
```json
{
  "nomeUsuario": "Rafaela@gmail",
  "emailUsuario": "Rafaela@gmail.com"
}
```
- **Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": "Rafaela"
}
```

---

## 👥 2. GRUPO: CLIENTES 

### 2.1 Listar Todos os Clientes
- **GET** `/api/Cliente`
- **Response (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "Genis",
    "email": "Genis@gmail.com",
    "cpf": "12345678901",
    "telefone": "35999999999",
    "endereco": "Rua da Biblioteca, 123"
  }
]
```

### 2.2 Cadastrar Novo Cliente
- **POST** `/api/Cliente`
- **Request (JSON):**
```json
{
  "nome": "Genis",
  "email": "Genis@gmail.com",
  "cpf": "12345678901",
  "telefone": "35999999999",
  "endereco": "Rua da Biblioteca, 123"
}
```
- **Response (201 Created):**
```json
{
  "id": 4,
  "nome": "Genis",
  "cpf": "12345678901",
  "status": "Cliente criado com sucesso"
}
```

### 2.3 Listar Cliente por ID
- **GET** `/api/Cliente/4`
- **Response (200 OK):**
```json
{
  "id": 4,
  "nome": "Genis",
  "cpf": "12345678901",
  "email": "Genis@gmail.com",
  "endereco": "Rua da Biblioteca, 123"
}
```

### 2.4 Listar Cliente por CPF
- **GET** `/api/Cliente/buscar-por-cpf/12345679234`
- **Response (200 OK):**
```json
{
  "id": 5,
  "nome": "Exemplo",
  "cpf": "12345679234",
  "email": "exemplo@gmail.com"
}
```

### 2.5 Atualizar Dados do Cliente
- **PUT** `/api/Cliente/9`
- **Request (JSON):**
```json
{
  "id": 9,
  "nome": "Isabela Rocha",
  "email": "Isabela@gmail.com",
  "cpf": "12345679237",
  "telefone": "35994444444",
  "endereco": "Rua das Margaridas, 423"
}
```
- **Response (200 OK):**
```json
{
  "id": 9,
  "status": "Atualizado com sucesso",
  "updatedAt": "2024-05-10T14:30:00Z"
}
```

### 2.6 Remover Cliente por ID
- **DELETE** `/api/Cliente/10`
- **Response (204 No Content):** `{}`

---

## 📚 3. GRUPO: LIVROS

### 3.1 Listar Todos os Livros
- **GET** `/api/Livro`
- **Response (200 OK):**
```json
[
  {
    "idLivro": 1,
    "titulo": "Elizabeth Linda",
    "autor": "Isadora",
    "ano": 2022,
    "estoque": 1000
  }
]
```

### 3.2 Cadastrar Livro
- **POST** `/api/Livro`
- **Request (JSON):**
```json
{
  "titulo": "Elizabeth Linda",
  "autor": "Isadora",
  "ano": 2022,
  "estoque": 1000
}
```
- **Response (201 Created):**
```json
{
  "idLivro": 10,
  "titulo": "Elizabeth Linda",
  "status": "Cadastrado"
}
```

### 3.3 Listar Livro por ID
- **GET** `/api/Livro/3`
- **Response (200 OK):**
```json
{
  "idLivro": 3,
  "titulo": "O Hobbit",
  "autor": "J.R.R. Tolkien",
  "ano": 1937,
  "estoque": 5
}
```

### 3.4 Atualizar Dados do Livro
- **PUT** `/api/Livro/6`
- **Request (JSON):**
```json
{
  "id": 6,
  "titulo": "Animais das Praias",
  "autor": "Familia Rocha",
  "ano": 2024,
  "estoque": 12
}
```
- **Response (200 OK):**
```json
{
  "id": 6,
  "message": "Livro atualizado com sucesso"
}
```

### 3.5 Remover Livro por ID
- **DELETE** `/api/Livro/8`
- **Response (204 No Content):** `{}`

---

## 🔄 4. GRUPO: EMPRÉSTIMOS 

### 4.1 Realizar Empréstimo
- **POST** `/api/Emprestimo`
- **Request (JSON):**
```json
{
  "idCliente": 2,
  "idLivro": 3,
  "diasEmprestimo": 10
}
```
- **Response (200 OK):**
```json
{
  "idEmprestimo": 15,
  "mensagem": "Empréstimo realizado com sucesso!",
  "dataDevolucao": "2024-05-20T10:00:00Z"
}
```

### 4.2 Listar Meus Empréstimos
- **GET** `/api/Emprestimo`
- **Response (200 OK):**
```json
[
  {
    "idEmprestimo": 15,
    "idLivro": 3,
    "status": "Ativo",
    "dataEmprestimo": "2024-05-10T10:00:00Z"
  }
]
```

### 4.3 Devolução de Livro (Finalizar)
- **PATCH** `/api/emprestimo/devolver/3`
- **Response (200 OK):**
```json
{
  "mensagem": "Livro devolvido com sucesso!",
  "dataDevolucaoReal": "2024-05-15T09:00:00Z"
}
```

### 4.4 Adiar/Renovar Empréstimo
- **PATCH** `/api/emprestimo/renovar/7`
- **Request (JSON):**
```json
{
  "diasAdicionais": 15
}
```
- **Response (200 OK):**
```json
{
  "idEmprestimo": 7,
  "novaDataDevolucao": "2024-06-04T10:00:00Z",
  "status": "Prazo renovado"
}
```

---

## 📝 5. GRUPO: TAREFAS 

### 5.1 Listar Todas as Tarefas
- **GET** `/api/tarefas`
- **Response (200 OK):**
```json
[
  {
    "id": 1,
    "nomeTarefa": "Limpeza Geral",
    "statusTarefa": "Pendente",
    "prioridade": "Média"
  }
]
```

### 5.2 Cadastrar Tarefa
- **POST** `/api/tarefas`
- **Request (JSON):**
```json
{
  "nomeTarefa": "Organizar Devoluções",
  "descricaoTarefa": "Verificar os livros que foram entregues na caixa de coleta",
  "statusTarefa": "Pendente",
  "prioridade": "Alta"
}
```
- **Response (201 Created):**
```json
{
  "id": 5,
  "nomeTarefa": "Organizar Devoluções",
  "status": "Tarefa criada com sucesso"
}
```

### 5.3 Listar Tarefas com Filtro (Status e Prioridade)
- **GET** `/api/tarefas?status=Pendente&prioridade=Alta`
- **Response (200 OK):**
```json
[
  {
    "id": 5,
    "nomeTarefa": "Organizar Devoluções",
    "statusTarefa": "Pendente",
    "prioridade": "Alta"
  }
]
```

### 5.4 Atualizar Tarefa
- **PUT** `/api/Tarefas/5`
- **Request (JSON):**
```json
{
  "nomeTarefa": "Limpeza banheiros",
  "descricaoTarefa": "Realizar manutenção completa semanal",
  "statusTarefa": "Concluida",
  "prioridade": "Média"
}
```
- **Response (200 OK):**
```json
{
  "id": 5,
  "status": "Tarefa atualizada com sucesso"
}
```

### 5.5 Remover Tarefa por ID
- **DELETE** `/api/tarefas/6`
- **Response (204 No Content):** `{}`


