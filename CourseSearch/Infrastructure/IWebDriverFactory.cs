using OpenQA.Selenium;

namespace ProjetoPesquisaAlura.Infrastructure
{
    public interface IWebDriverFactory
    {
        IWebDriver CreateWebDriver(bool visible = false);
    }
}
