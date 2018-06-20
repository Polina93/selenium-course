using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static System.Int32;

namespace SeleniumCourse
{
    [TestFixture]
    public class WorkWithBasketTest
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
            driver.Url = "http://localhost:8080/litecart/";

            var products = driver.FindElement(By.CssSelector("div.middle>.content>#box-most-popular")).FindElements(By.CssSelector(".product"));
            var count = 0;
            for (var i = 0; i < 3; i++)
            {
                var product = products[0];
                product.FindElement(By.CssSelector("a.link")).Click();
                
                var basketCount = driver.FindElement(By.CssSelector("div#cart a.content > span.quantity"));
                Assert.AreEqual(count, Parse(basketCount.Text));

                var productCard = driver.FindElement(By.CssSelector("div#box-product"));
                var productInfo = productCard.FindElement(By.CssSelector("div.information"));
                if (productInfo.FindElements(By.CssSelector(".buy_now select[name='options[Size]']")).Count > 0)
                {
                    var requiredOption = productInfo.FindElement(By.CssSelector(".buy_now select[name='options[Size]']"));
                    new SelectElement(requiredOption).SelectByValue("Small");
                }
                var buttonAddBasket = productInfo.FindElement(By.CssSelector(".buy_now  td.quantity > button[name=add_cart_product]"));
                buttonAddBasket.Click();
                count++;
                
                wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(By.CssSelector("div#cart > a.content > span.quantity")), count.ToString()));

                driver.Url = "http://localhost:8080/litecart/";
                products = driver.FindElement(By.CssSelector("div.middle>.content>#box-most-popular")).FindElements(By.CssSelector(".product"));
            }

            var goToBasket = driver.FindElement(By.CssSelector("div#cart a.link"));
            goToBasket.Click();

            var countInBasket = driver.FindElements(By.CssSelector("div#box-checkout-cart ul.items li")).Count;
            var currentCount = countInBasket;
            for (var i = 0; i < countInBasket; i++)
            {
                var removeButton = driver.FindElement(By.CssSelector("div#box-checkout-cart ul.items li:first-child")).FindElement(By.CssSelector("button[name=remove_cart_item]"));
                removeButton.Click();
                currentCount--;
                wait.Until(ExpectedConditions.StalenessOf(driver.FindElement(By.CssSelector("div#order_confirmation-wrapper table.dataTable"))));
                if (currentCount == 0)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                    Assert.AreEqual(0,driver.FindElements(By.CssSelector("div#order_confirmation-wrapper table.dataTable")).Count);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                }
                else
                {
                    Assert.AreEqual(currentCount + 5, driver.FindElements(By.CssSelector("div#order_confirmation-wrapper table.dataTable tr")).Count);
                }
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
