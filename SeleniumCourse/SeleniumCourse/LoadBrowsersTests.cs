using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    public class LoadBrowsersTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            //firefox with geckodriver
            /*    FirefoxOptions options = new FirefoxOptions();
                options.UseLegacyImplementation = false;
                driver = new FirefoxDriver(options);*/
            FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = false;
            options.BrowserExecutableLocation = @"c:\Program Files\Firefox Nightly\firefox.exe";
            driver = new FirefoxDriver(options);
            // chrome
            //    driver = new ChromeDriver();
            //IE
            /*    InternetExplorerOptions options = new InternetExplorerOptions();
            options.RequireWindowFocus = true;
            driver = new InternetExplorerDriver(options);*/

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test()
        {
            driver.Url = "http://localhost:8080/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElement(By.Name("login")).Click();
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
