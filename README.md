# Projeto Pesquisa de Cursos

Este projeto é uma aplicação de exemplo que realiza a pesquisa de cursos em uma plataforma educacional e armazena os resultados em um banco de dados.

## Estrutura do Projeto

O projeto está estruturado da seguinte forma:

- Pasta `Application`
  - `PesquisaCursoService.cs`: Classe responsável por executar a lógica de pesquisa de cursos.
  - `IPesquisaCursoService.cs`: Interface que define o contrato para o serviço de pesquisa de cursos.

- Pasta `Domain`
  - Pasta `PesquisaCursos`
    - `Curso.cs`: Classe que representa um curso.
    - `CursoRepository.cs`: Classe responsável pela persistência dos cursos no banco de dados.
    - `ICursoRepository.cs`: Interface que define o contrato para o repositório de cursos.

- Pasta `Infrastructure`
  - `IBancoDados.cs`: Interface que define o contrato para a conexão com o banco de dados.
  - `BancoDadosInfraestrutura.cs`: Classe responsável pela configuração da infraestrutura de banco de dados.
  - `IWebDriverFactory.cs`: Interface que define o contrato para a criação de instâncias do WebDriver.
  - `SeleniumWebDriver.cs`: Classe responsável por gerenciar uma instância do WebDriver do Selenium. 
  - `SeleniumWebDriverFactory.cs`: Classe que implementa a interface `IWebDriverFactory` e atua como uma fábrica para criar instâncias do WebDriver do Selenium.

- `Program.cs`: Arquivo contendo o ponto de entrada da aplicação.
- `Startup.cs`: Arquivo contendo a configuração inicial da aplicação.

## Decisões Técnicas

- O projeto utiliza a biblioteca Selenium WebDriver para automatizar a interação com a plataforma educacional e obter os resultados da pesquisa de cursos.
- A lógica de pesquisa de cursos é implementada na classe `PesquisaCursoService.cs`, que utiliza o WebDriver para realizar a pesquisa e obter os resultados.
- Os cursos encontrados são armazenados em um banco de dados por meio da classe `CursoRepository.cs`.
- A separação em camadas (Application, Domain e Infrastructure) visa promover a modularidade e a separação de responsabilidades no projeto.

## Requisitos

- .NET Core SDK 3.1 ou superior
- Selenium WebDriver

## Bibliotecas e Ferramentas

O projeto faz uso das seguintes bibliotecas e ferramentas:

- **OpenQA.Selenium**: Uma biblioteca que fornece uma API para interagir com navegadores web. Utilizamos o Selenium WebDriver em conjunto com o Chrome para automatizar a navegação e extração de informações da plataforma Alura.
- **HtmlAgilityPack**: Uma biblioteca que facilita a manipulação e extração de informações de documentos HTML. Utilizamos o HtmlAgilityPack para extrair informações específicas dos cursos encontrados na página de resultados da pesquisa.
- **Dapper**: Uma biblioteca de mapeamento objeto-relacional (ORM) que simplifica o acesso e a manipulação do banco de dados. Utilizamos o Dapper para executar as consultas SQL e inserir os cursos no banco de dados.
- **NuGet**: O gerenciador de pacotes NuGet é utilizado para gerenciar as dependências do projeto e garantir que todas as bibliotecas necessárias sejam instaladas corretamente.
- **Visual Studio**: O Visual Studio é a IDE utilizada para desenvolver e executar o projeto. No entanto, você pode usar o ambiente de desenvolvimento de sua escolha.
- **System.Data.SQLite**: Uma biblioteca que fornece acesso ao banco de dados SQLite. Utilizamos o System.Data.SQLite para trabalhar com o SQLite em nosso projeto.
## Como executar

1. Clone o repositório: `git clone <URL_DO_REPOSITORIO>`
2. Abra o projeto no Visual Studio ou em um editor de texto de sua preferência.
3. Execute a aplicação.

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir uma issue para relatar problemas, sugestões ou enviar um pull request com melhorias.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
