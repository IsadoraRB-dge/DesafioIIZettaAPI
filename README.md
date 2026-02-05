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

* **Framework:** .NET 10.0
* **Banco de Dados:** SQL Server via Entity Framework Core 10
* **Segurança:** Autenticação JWT (Bearer) e criptografia de senhas com BCrypt.Net
* **Mapeamento:** AutoMapper para conversão entre Entidades e DTOs
* **Testes:** Entity Framework InMemory

---

## 🟢 Endpoints da API

### 🔑 Acesso e Identidade (`/api/Acesso`)

Responsável por criar a conta do "Comandante da Biblioteca" e gerar o acesso seguro.

* **POST /api/Acesso/registrar**
  * Cria a conta do bibliotecário com senha criptografada.
  * **Body (JSON):**
    ```json
    {
        "nomeUsuario": "Isadora Rocha",
        "emailUsuario": "Isadora@biblioteca.com",
        "senhaUsuario": "Minhagataelinda"
    }
    ```

* **POST /api/Acesso/login**
  * Autentica e libera o Token JWT.
  * **Body (JSON):**
    ```json
    {
        "nomeUsuario": "Isadora Rocha",
        "emailUsuario": "Isadora@biblioteca.com"
    }
    ```

---

### 🚻 Clientes (`/api/Cliente`)

Gestão completa dos clientes do bibliotecário.

* **GET /api/Cliente | GET /api/Cliente/{id}**
  * Lista todos os seus clientes ou busca um detalhado por ID.
* **GET /api/Cliente/buscar-por-cpf/{cpf}**
  * Busca rápida por documento (CPF).
* **POST /api/Cliente**
  * **Body (JSON):**
    ```json
    {
        "nome": "Isabela Rocha",
        "email": "isabela@email.com",
        "cpf": "123.456.789-00",
        "telefone": "35999999999",
        "endereco": "Rua Central, 10"
    }
    ```
* **PUT /api/Cliente/{id}**
  * **Body (JSON):**
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
* **DELETE /api/Cliente/{id}**
  * Remove o cliente se não houver restrições de vínculo (empréstimos ativos).

---

### 📖 Livros (`/api/Livro`)

Controle do acervo e disponibilidade física.

* **GET /api/Livro**
  * Lista os livros cadastrados na biblioteca.
* **POST /api/Livro**
  * **Body (JSON):**
    ```json
    {
        "titulo": "Animais Marinhos",
        "autor": "Familia Rocha",
        "ano": 2024,
        "estoque": 12
    }
    ```
* **PUT /api/Livro/{id}**
  * **Body (JSON):**
    ```json
    {
        "id": 6,
        "titulo": "Animais das Praias",
        "autor": "Familia Rocha",
        "ano": 2024,
        "estoque": 12
    }
    ```
* **DELETE /api/Livro/{id}**
  * Remove o livro do acervo se não houver restrições de vínculo.

---

### 📑 Empréstimos (`/api/Emprestimo`)

A inteligência do sistema e controle de estoque.

* **POST /api/Emprestimo**
  * Registra saída e reduz estoque automaticamente.
  * **Body (JSON):**
    ```json
    {
        "idCliente": 9, 
        "idLivro": 6,
        "diasEmprestimo": 15
    }
    ```
* **PATCH /api/Emprestimo/renovar/{id}**
  * Aumenta o prazo de entrega para o cliente.
  * **Body (JSON):**
    ```json
    {
        "diasAdicionais": 7
    }
    ```
* **PATCH /api/Emprestimo/devolver/{id}**
  * Finaliza um empréstimo e processa o retorno do livro ao estoque (+1).
  * Verifica se houve atraso através do serviço de multas (`IMultaService`).
  * **Body (JSON):**
    ```json
    {
        "dataDevolucaoManual": "2024-05-20T14:30:00"
    }
    ```

---

### 🧹 Gestão de Tarefas (`/api/Tarefas`)

Organização pessoal da rotina administrativa do bibliotecário.

* **GET /api/Tarefas**
  * Suporta filtros via Query String (Ex: `/api/Tarefas?status=Pendente&prioridade=Alta`).
* **POST /api/Tarefas**
  * **Body (JSON):**
    ```json
    {
        "nomeTarefa": "Limpeza",
        "descricaoTarefa": "Realizar a manutenção semanal: Tirar pó, limpar espelhos, limpar o sanitário",
        "statusTarefa": "Concluida",
        "prioridade": "Média"
    }
    ```
* **PUT /api/Tarefas/{id}**
  * **Body (JSON):**
    ```json
    {	
        "idTarefa": 3,
        "nomeTarefa": "Limpeza Banheiro",
        "descricaoTarefa": "Realizar a manutenção semanal",
        "statusTarefa": "Concluida",
        "prioridade": "Média"
    }
    ```
* **DELETE /api/Tarefas/{id}**
  * Remove permanentemente uma tarefa da lista do bibliotecário.
