using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    class AddProductTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
     //       driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void LoginAdmin()
        {
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElement(By.Name("login")).Click();
        }

        [Test]
        public void Test()
        {
            driver.Url = "http://localhost:8080/litecart/admin/";
            LoginAdmin();

            var menuCatalog = driver.FindElement(By.CssSelector("ul#box-apps-menu li:nth-child(2)"));
            menuCatalog.Click();

            var buttonAdd = driver.FindElement(By.CssSelector("#content a:nth-of-type(2)"));
            buttonAdd.Click();

            var productName = RandomString(10);
            var filePath = TestContext.CurrentContext.TestDirectory + @"\new_product.jpg";
            

            var generalElements = driver.FindElements(By.CssSelector("#tab-general > table > tbody > tr > td"));
            generalElements[1].FindElement(By.Name("name[en]")).SendKeys(productName);
            generalElements[2].FindElement(By.Name("code")).SendKeys("123");
            var categories = generalElements[5].FindElements(By.CssSelector("table > tbody > tr > td"));
            categories[1].FindElement(By.CssSelector("input[type=checkbox]")).Click();
            categories[3].FindElement(By.CssSelector("input[type=checkbox]")).Click();
            var quantityStr = generalElements[6].FindElements(By.CssSelector("table > tbody > tr > td"));
            quantityStr[0].FindElement(By.Name("quantity")).SendKeys("1");
            new SelectElement(quantityStr[3].FindElement(By.Name("sold_out_status_id"))).SelectByValue("2");

            var uploadImages = generalElements[7].FindElements(By.CssSelector("table > tbody > tr > td"));
            uploadImages[0].FindElement(By.CssSelector("input[type=file]")).SendKeys(filePath);

            driver.FindElement(By.CssSelector(".tabs > ul > li:nth-child(2) > a")).Click();
            var infoElements = driver.FindElements(By.CssSelector("#tab-information > table > tbody > tr > td"));
            new SelectElement(infoElements[0].FindElement(By.Name("manufacturer_id"))).SelectByValue("1");

            driver.FindElement(By.CssSelector(".tabs > ul > li:nth-child(4) > a")).Click();
            var pricesElements = driver.FindElements(By.CssSelector("#tab-prices > table > tbody > tr > td"));
            pricesElements[0].FindElement(By.Name("purchase_price")).SendKeys("100");
            new SelectElement(pricesElements[0].FindElement(By.Name("purchase_price_currency_code"))).SelectByValue("EUR");

            driver.FindElement(By.CssSelector("td#content > form > p")).FindElement(By.CssSelector("button[name=save]")).Click();
            
            Assert.IsTrue(driver.FindElement(By.XPath($"//td[@id='content']//form[@name='catalog_form']//table[@class='dataTable']//tr//td//a[.='{productName}']")).Displayed, "No new product");
        }

        private static readonly Random random = new Random((int)DateTime.Now.Ticks);

        public string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
