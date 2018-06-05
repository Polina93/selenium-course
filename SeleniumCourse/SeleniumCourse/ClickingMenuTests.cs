using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    public class ClickingMenuTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void ClickAllItemsOfMenu_Test()
        {
            driver.Url = "http://localhost:8080/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            var menu = driver.FindElements(By.Id("app-"));
            for (var i = 0; i < menu.Count; i++)
            {
                menu[i].Click();
                var submenu = driver.FindElements(By.ClassName("docs"));
                if (submenu.Count==0)
                    Assert.AreEqual(driver.FindElement(By.CssSelector("#content > h1")).Displayed, true);
                foreach (var item in submenu)
                {
                    item.Click();
                    Assert.AreEqual(driver.FindElement(By.CssSelector("#content > h1")).Displayed, true);
                }
                menu = driver.FindElements(By.Id("app-"));
            }
        }
        
        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
