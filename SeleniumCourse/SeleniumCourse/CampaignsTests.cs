using System;
using System.Globalization;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    public class CampaignsTets
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
       /*     FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = false;
            options.BrowserExecutableLocation = @"c:\Program Files\Firefox Nightly\firefox.exe";
            driver = new FirefoxDriver(options);*/
            // chrome
                driver = new ChromeDriver();
            //IE
     /*           InternetExplorerOptions options = new InternetExplorerOptions();
            options.RequireWindowFocus = true;
            driver = new InternetExplorerDriver(options);
            */
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void StyleOnMainTest()
        {
            driver.Url = "http://localhost:8080/litecart/";

            var campaignsItems = driver.FindElement(By.CssSelector("div.middle>.content")).FindElement(By.CssSelector("#box-campaigns")).FindElements(By.CssSelector(".product"));
            foreach (var productOnMain in campaignsItems)
            {
                var regPriceOnMain = productOnMain.FindElement(By.CssSelector(".price-wrapper>.regular-price"));
                var camPriceOnMain = productOnMain.FindElement(By.CssSelector(".price-wrapper>.campaign-price"));
                
                var regPriceOnMainColor = regPriceOnMain.GetCssValue("color");
                var regPriceOnMainStyle = regPriceOnMain.TagName == "s" && regPriceOnMain.GetCssValue("text-decoration-line") == "" ? "line-through" : regPriceOnMain.GetCssValue("text-decoration-line");
                var regPriceOnMainSize = regPriceOnMain.GetCssValue("font-size");
                var rgbRegPrice = Regex.Matches(regPriceOnMainColor, @"(\d+)");
                Assert.That(rgbRegPrice[0].Value == rgbRegPrice[1].Value && rgbRegPrice[1].Value == rgbRegPrice[2].Value);
                Assert.AreEqual("line-through", regPriceOnMainStyle);
                
                var camPriceOnMainColor = camPriceOnMain.GetCssValue("color");
                var camPriceOnMainStyle = int.Parse(camPriceOnMain.GetCssValue("font-weight"));
                var camPriceOnMainSize = camPriceOnMain.GetCssValue("font-size");
                var rgbCamPrice = Regex.Matches(camPriceOnMainColor, @"(\d+)");
                Assert.That(rgbCamPrice[1].Value == "0" && rgbCamPrice[2].Value == "0");
                Assert.LessOrEqual(700, camPriceOnMainStyle);

                var sizeRegPrice = double.Parse(Regex.Matches(regPriceOnMainSize, @"(\d+.*\d+)")[0].Value, CultureInfo.InvariantCulture);
                var sizeCamPrice = double.Parse(Regex.Matches(camPriceOnMainSize, @"(\d+.*\d+)")[0].Value, CultureInfo.InvariantCulture);
                Assert.Less(sizeRegPrice, sizeCamPrice);
            }
        }

        [Test]
        public void StyleOnCardTest()
        {
            driver.Url = "http://localhost:8080/litecart/";

            var campaignsItems = driver.FindElement(By.CssSelector("div.middle>.content")).FindElement(By.CssSelector("#box-campaigns")).FindElements(By.CssSelector(".product"));
            for (var i = 0; i < campaignsItems.Count; i++)
            {
                var productOnMain = campaignsItems[i];
                productOnMain.FindElement(By.CssSelector("a.link")).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var product = driver.FindElement(By.CssSelector("div#box-product"));
                var productInfo = product.FindElement(By.CssSelector("div.information"));
                var regPrice = productInfo.FindElement(By.CssSelector(".price-wrapper>.regular-price"));
                var camPrice = productInfo.FindElement(By.CssSelector(".price-wrapper>.campaign-price"));
                
                var regPriceColor = regPrice.GetCssValue("color");
                var regPriceStyle = regPrice.TagName == "s" && regPrice.GetCssValue("text-decoration-line") == "" ? "line-through" : regPrice.GetCssValue("text-decoration-line");
                var regPriceSize = regPrice.GetCssValue("font-size");
                var rgbRegPrice = Regex.Matches(regPriceColor, @"(\d+)");
                Assert.That(rgbRegPrice[0].Value == rgbRegPrice[1].Value && rgbRegPrice[1].Value == rgbRegPrice[2].Value);
                Assert.AreEqual("line-through", regPriceStyle);
                
                var camPriceColor = camPrice.GetCssValue("color");
                var camPriceStyle = int.Parse(camPrice.GetCssValue("font-weight"));
                var camPriceSize = camPrice.GetCssValue("font-size");
                var rgbCamPrice = Regex.Matches(camPriceColor, @"(\d+)");
                Assert.That(rgbCamPrice[1].Value == "0" && rgbCamPrice[2].Value == "0");
                Assert.LessOrEqual(700, camPriceStyle);

                var sizeRegPrice = double.Parse(Regex.Matches(regPriceSize, @"(\d+.*\d+)")[0].Value, CultureInfo.InvariantCulture);
                var sizeCamPrice = double.Parse(Regex.Matches(camPriceSize, @"(\d+.*\d+)")[0].Value, CultureInfo.InvariantCulture);
                Assert.Less(sizeRegPrice, sizeCamPrice);
                
                driver.Url = "http://localhost:8080/litecart/";
                campaignsItems = driver.FindElement(By.CssSelector("div.middle>.content")).FindElement(By.CssSelector("#box-campaigns")).FindElements(By.CssSelector(".product"));
            }
        }

        [Test]
        public void InfoOnMainAndCardEqualsTest()
        {
            driver.Url = "http://localhost:8080/litecart/";

            var campaignsItems = driver.FindElement(By.CssSelector("div.middle>.content")).FindElement(By.CssSelector("#box-campaigns")).FindElements(By.CssSelector(".product"));
            for (var i = 0; i < campaignsItems.Count; i++)
            {
                var productOnMain = campaignsItems[i];

                var nameOnMain = productOnMain.FindElement(By.CssSelector(".name")).Text;
                var regPriceOnMain = productOnMain.FindElement(By.CssSelector(".price-wrapper>.regular-price")).Text;
                var camPriceOnMain = productOnMain.FindElement(By.CssSelector(".price-wrapper>.campaign-price")).Text;
                
                productOnMain.FindElement(By.CssSelector("a.link")).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var product = driver.FindElement(By.CssSelector("div#box-product"));
                var name = product.FindElement(By.CssSelector(".title")).Text;
                var productInfo = product.FindElement(By.CssSelector("div.information"));
                var regPrice = productInfo.FindElement(By.CssSelector(".price-wrapper>.regular-price")).Text;
                var camPrice = productInfo.FindElement(By.CssSelector(".price-wrapper>.campaign-price")).Text;
                
                Assert.AreEqual(name,nameOnMain);
                Assert.AreEqual(regPrice, regPriceOnMain);
                Assert.AreEqual(camPrice, camPriceOnMain);

                driver.Url = "http://localhost:8080/litecart/";
                campaignsItems = driver.FindElement(By.CssSelector("div.middle>.content")).FindElement(By.CssSelector("#box-campaigns")).FindElements(By.CssSelector(".product"));
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
