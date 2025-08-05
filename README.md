# Teste Técnico - Sistema de Gestão de Cadastros

Este projeto é uma aplicação de desktop desenvolvida como parte de um desafio técnico. O objetivo é demonstrar habilidades na construção de uma aplicação WPF utilizando o padrão MVVM, com foco em organização de código, manipulação de dados e boas práticas de desenvolvimento.

## 📜 Descrição

A aplicação permite o cadastro e a manipulação de dados de pessoas, produtos e pedidos. Os dados são persistidos localmente em arquivos JSON, e a interface oferece funcionalidades de CRUD (Criar, Ler, Atualizar, Excluir) e filtragem para todas as entidades principais.

## ✨ Funcionalidades

- **Cadastro de Pessoas**:
  - Inclusão, edição e exclusão de clientes.
  - Filtros por Nome e CPF.
  - Visualização de todos os pedidos associados a um cliente selecionado.
  - Ações para alterar o status de cada pedido (Pendente, Pago, Enviado, Recebido).
  - Filtros para visualizar pedidos por status.

- **Cadastro de Produtos**:
  - Inclusão, edição e exclusão de produtos.
  - Filtros por Nome, Código e faixa de Valor.

- **Cadastro de Pedidos**:
  - Criação de novos pedidos associados a um cliente.
  - Adição de múltiplos produtos com quantidade.
  - Cálculo automático do valor total do pedido.
  - Seleção da forma de pagamento.

## 🛠️ Tecnologias Utilizadas

- **C#**
- **WPF (Windows Presentation Foundation)**
- **.NET Framework 4.6**
- **Padrão de Arquitetura MVVM (Model-View-ViewModel)**
- **Newtonsoft.Json** para serialização e desserialização de dados.

## ⚙️ Pré-requisitos

Para compilar e executar este projeto, você precisará de:

- **Visual Studio 2022** (ou versão compatível)
- **.NET Framework 4.6 Developer Pack**

## 🚀 Como Executar

1.  **Clone o repositório:**
    ```sh
    git clone [<URL_DO_SEU_REPOSITORIO>](https://github.com/jpmmaion498/teste-tecnico.git)
    ```
2.  **Abra a solução:**
    - Navegue até a pasta do projeto e abra o arquivo `teste-tecnico.sln` com o Visual Studio.

3.  **Restaure os pacotes NuGet:**
    - O Visual Studio deve restaurar os pacotes automaticamente ao abrir a solução. Caso contrário, clique com o botão direito na solução no "Gerenciador de Soluções" e selecione "Restaurar Pacotes NuGet".

4.  **Execute o projeto:**
    - Pressione `F5` ou clique no botão "Iniciar" (com o ícone de play verde) para compilar e executar a aplicação.

## 📁 Estrutura do Projeto

O código está organizado seguindo as melhores práticas do padrão MVVM:

- `Models/`: Contém as classes de domínio (Pessoa, Produto, Pedido).
- `ViewModels/`: Contém a lógica de apresentação e o estado da interface.
- `Views/`: Contém as telas da aplicação (arquivos XAML e code-behind).
- `Services/`: Contém a lógica de negócio e os serviços de persistência de dados.
- `Data/`: Pasta onde os arquivos `pessoas.json`, `produtos.json` e `pedidos.json` são criados e armazenados em tempo de execução.
