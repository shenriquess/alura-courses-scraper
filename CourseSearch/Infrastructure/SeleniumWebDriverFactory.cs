using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ProjetoPesquisaAlura.Infrastructure
{
    public class SeleniumWebDriverFactory : IWebDriverFactory
    {
        public IWebDriver CreateWebDriver(bool visible = false)
        {
            ChromeOptions options = new ChromeOptions();
            if (!visible)
            {
                options.AddArguments("--start-maximized");
            }
            options.AddArguments("--disable-notifications");
            options.AddArguments("--disable-popup-blocking");
            options.AddArguments("--disable-infobars");
            //options.AddArguments("--headless");

            new DriverManager().SetUpDriver(new ChromeConfig());

            IWebDriver driver = new ChromeDriver(options);
            return driver;
        }
    }
}
