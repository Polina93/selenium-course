using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    public class LinkOpenNewPageTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        }

        [Test]
        public void Test()
        {
            driver.Url = "http://localhost:8080/litecart/admin/";
            Login();

            driver.Url = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";

            var countryRow = driver.FindElements(By.CssSelector("td#content .dataTable tr"))[1];
            countryRow.FindElements(By.TagName("td"))[4].FindElement(By.CssSelector("a")).Click();

            var fields = driver.FindElements(By.CssSelector("td#content table tr"));
            foreach (var field in fields)
            {
                if (field.FindElements(By.CssSelector("a[target=_blank]")).Count > 0)
                {
                    var originalWindow = driver.CurrentWindowHandle;
                    var link = field.FindElement(By.CssSelector("a[target=_blank]"));
                    link.Click();
                    var newWindow = wait.Until(IsWindowsMoreOne());
                    driver.SwitchTo().Window(newWindow);
                    driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                };
            }

            Func<IWebDriver, string> IsWindowsMoreOne()
            {
                return (d) =>
                {
                    ICollection<string> handles = driver.WindowHandles;
                    return handles.Count > 1 ? handles.ElementAt(1) : null;
                };
            }
        }

        public void Login()
        {
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
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
