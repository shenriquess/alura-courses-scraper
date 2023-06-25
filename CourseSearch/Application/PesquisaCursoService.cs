using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using ProjetoPesquisaAlura.Infrastructure;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace ProjetoPesquisaAlura.Domain.PesquisaCursos
{
    public class PesquisaCursoService : IPesquisaCursoService
    {
        private readonly IWebDriverFactory _webDriverFactory;
        private readonly ICursoRepository _cursoRepository;
        // Declaração de constantes para os elementos da página
        private const string UrlInicial = "https://www.alura.com.br/";
        private const string BotaoVerMaisCaminho = "//*[@id='busca']/nav/a[2]";
        private const string InputPesquisar = "#header-barraBusca-form-campoBusca";
        private const string CorpoResultado = "#busca-resultados";
        private const string Filtro = "#busca-form span";
        private const string CheckboxCursos = "#type-filter--0";
        private const string CheckboxFormacoes = "#type-filter--2";
        private const string BotaoFiltrarResultado = "//*[@id='busca--filtrar-resultados']";
        private const string DivCompleta = "//li[contains(@class, 'busca-resultado')]";
        private const string TituloCaminho = ".//*[contains(@class,'busca-resultado-nome')]";
        private const string DescricaoCaminho = ".//*[contains(@class,'busca-resultado-descricao')]";
        private const string BuscaResultadoLink = ".//*[contains(@class,'busca-resultado-link')]";
        private const string ProfessorCurso = "//*[@id='section-icon']/div[1]/section/div/div/div/h3";
        private const string ProfessorFormacao = "//*[@id='instrutores']/div/ul/li[2]/div/h3";
        private const string CargaHorariaCurso = "/html/body/section[1]/div/div[2]/div[1]/div/div[1]/div/p[1]";
        private const string CargaHorariaFormacao = "/html/body/main/section[1]/article[2]/div/div[1]/div[2]/div";

        public PesquisaCursoService(IWebDriverFactory webDriverFactory, ICursoRepository cursoRepository)
        {
            _webDriverFactory = webDriverFactory;
            _cursoRepository = cursoRepository;
        }

        public void PesquisarCursos(string termoPesquisa)
        {
            using (var webDriver = _webDriverFactory.CreateWebDriver())
            {
                var divExiste = false;
                var resultados = new List<string>();

                IWebDriver driver = webDriver;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                try
                {
                    // Navegação para a página inicial do Alura
                    driver.Navigate().GoToUrl(UrlInicial);

                    IWebElement campoPesquisar = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(InputPesquisar)));
                    campoPesquisar.SendKeys(termoPesquisa);
                    campoPesquisar.SendKeys(Keys.Enter);

                    // Aplicação de filtros de busca
                    IWebElement filtroLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(Filtro)));
                    filtroLink.Click();

                    IWebElement checkboxCursosElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(CheckboxCursos)));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkboxCursosElement);

                    IWebElement checkboxFormacoesElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(CheckboxFormacoes)));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkboxFormacoesElement);

                    IWebElement botaoFiltrarResultado = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(BotaoFiltrarResultado)));
                    botaoFiltrarResultado.Click();

                    // Verificação da existência da div de resultados
                    IWebElement divElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div[2]/section/div")));
                    string classAttributeValue = divElement.GetAttribute("class");
                    bool hasDesiredClass = classAttributeValue.Contains("search-noResult search-noResult--visible");

                    if (hasDesiredClass)
                    {
                        divExiste = true;
                    }
                    else
                    {
                        // Loop para clicar no botão "Ver mais" e obter os resultados adicionais
                        while (true)
                        {
                            try
                            {
                                IWebElement corpoResultado = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(CorpoResultado)));
                                resultados.Add(corpoResultado.GetAttribute("innerHTML"));

                                IWebElement botaoVerMais = driver.FindElement(By.XPath(BotaoVerMaisCaminho));
                                string classAttributeValue2 = botaoVerMais.GetAttribute("class");
                                bool hasDesiredClass2 = classAttributeValue2.Contains("busca-paginacao-prevNext busca-paginacao-linksProximos busca-paginacao-prevNext--disabled");

                                if (hasDesiredClass2)
                                {
                                    break;
                                }

                                botaoVerMais.Click();
                            }
                            catch
                            {
                                break;
                            }
                        }
                    }

                    // Verificação dos resultados obtidos
                    if (resultados.Count == 0 || divExiste)
                    {
                        Console.WriteLine("Não foi possível obter resultados para pesquisa");
                        driver?.Quit();
                        Console.WriteLine("Pressione qualquer tecla para sair...");
                        Console.ReadKey();
                        return;
                    }

                    // Processamento dos resultados e inserção no banco de dados
                    foreach (var resultado in resultados)
                    {
                        var camposExtraidos = new List<Dictionary<string, object>>();
                        try
                        {
                            camposExtraidos = BuscarInformacoes(resultado, driver, wait);

                            var cursos = new List<Curso>();
                            foreach (var campos in camposExtraidos)
                            {
                                var curso = new Curso();
                                // Mapeie os campos do dicionário para as propriedades do objeto 'curso'
                                if (campos.TryGetValue("Titulo", out var tituloValue))
                                    curso.Titulo = tituloValue?.ToString();
                                if (campos.TryGetValue("Professor", out var professorValue))
                                    curso.Professor = professorValue?.ToString();
                                if (campos.TryGetValue("Carga_Horaria", out var cargaHorariaValue))
                                    curso.Carga_Horaria = cargaHorariaValue?.ToString();
                                if (campos.TryGetValue("Descricao", out var descricaoValue))
                                    curso.Descricao = descricaoValue?.ToString();

                                cursos.Add(curso);
                            }

                            InserirCursos(cursos);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Ocorreu um erro em ExtrairInformacoesHtml: " + e.Message);
                            continue;
                        }
                    }
                }
                finally
                {
                    driver?.Quit();
                }
            }
        }

        public void InserirCursos(List<Curso> cursos)
        {
            // Lógica para inserir os cursos no banco de dados
            foreach (var curso in cursos)
            {
                _cursoRepository.InserirCurso(curso);
            }
        }

        public static List<Dictionary<string, object>> BuscarInformacoes(string html, IWebDriver driver, WebDriverWait wait)
        {
            // Carregamento do HTML em um objeto HtmlDocument
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes(DivCompleta);
            var resultados = new List<Dictionary<string, object>>();

            // Iteração pelos nós do HTML para extrair as informações desejadas
            foreach (var node in nodes)
            {
                try
                {
                    var resultado = new Dictionary<string, object>();
                    var linkElement = node.SelectSingleNode(BuscaResultadoLink);
                    var linkUrl = linkElement.GetAttributeValue("href", "");

                    if (!linkUrl.StartsWith("https://www.alura.com.br"))
                    {
                        linkUrl = "https://www.alura.com.br" + linkUrl;
                    }

                    driver.Navigate().GoToUrl(linkUrl);

                    IWebElement professorName = null;
                    IWebElement cargaHoraria = null;

                    // Verificação do tipo de página (curso ou formação) para obter as informações corretas
                    try
                    {
                        professorName = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(ProfessorCurso)));
                        cargaHoraria = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(CargaHorariaCurso)));
                    }
                    catch
                    {
                        professorName = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(ProfessorFormacao)));
                        cargaHoraria = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(CargaHorariaFormacao)));
                    }

                    var tituloNode = node.SelectSingleNode(TituloCaminho);
                    if (tituloNode != null)
                        resultado["Titulo"] = tituloNode.InnerText.Trim();

                    var publicacaoNode = node.SelectSingleNode(DescricaoCaminho);
                    if (publicacaoNode != null)
                        resultado["Descricao"] = publicacaoNode.InnerText.Trim();

                    resultado["Professor"] = professorName.GetAttribute("innerText");
                    resultado["Carga_Horaria"] = cargaHoraria.GetAttribute("innerText");

                    resultados.Add(resultado);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ocorreu um erro ao extrair as informações: " + e.Message);
                }
            }

            return resultados;
        }
    }
}
