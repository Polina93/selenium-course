using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    public class StickersTest
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
        public void Test()
        {
            driver.Url = "http://localhost:8080/litecart/";

            var boxes = driver.FindElement(By.CssSelector("div.middle>.content")).FindElements(By.ClassName("box"));
            foreach (var box in boxes)
            {
                var products = box.FindElements(By.TagName("li"));
                foreach (var product in products)
                {
                    var stickersCount = product.FindElements(By.ClassName("sticker")).Count;
                    Assert.AreEqual(1,stickersCount);
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
