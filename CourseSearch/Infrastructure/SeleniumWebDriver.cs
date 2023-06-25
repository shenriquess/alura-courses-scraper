using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


namespace ProjetoPesquisaAlura.Infrastructure
{
    public class Selenium_WebDriver
    {
        private IWebDriver driver;

        public IWebDriver GetWebDriver(bool visible = false)
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

            driver = new ChromeDriver(options);
            return driver;
        }
        public WebDriverWait WebDriver_Wait(int waitTimeSeconds = 10)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(waitTimeSeconds));
        }
        
    }
}
