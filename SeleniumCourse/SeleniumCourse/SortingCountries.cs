using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    public class SortingCountries
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void Login()
        {
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        [Test]
        public void TestSortingCountries()
        {
            driver.Url = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
            Login();

            var cellCountryName = driver.FindElement(By.CssSelector("td#content .dataTable")).FindElements(By.CssSelector("tr.row>td:nth-of-type(5)"));

            var prevName = "A";
            foreach (var cell in cellCountryName)
            {
                var name = cell.Text;
                Assert.LessOrEqual(prevName.CompareTo(name),0);
                prevName = name;
            }
        }

        [Test]
        public void TestSortingZones()
        {
            driver.Url = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
            Login();

            var tableCountries = driver.FindElement(By.CssSelector("td#content .dataTable")).FindElements(By.CssSelector("tr.row"));

            for (var j=0; j < tableCountries.Count; j++)
            {
                var row = tableCountries[j];
                var columns = row.FindElements(By.TagName("td"));
                var zoneCounts = columns[5].Text;
                if (zoneCounts != "0")
                {
                    columns[4].FindElement(By.CssSelector("a")).Click();
                    var tableZones = driver.FindElement(By.CssSelector("td#content .dataTable")).FindElements(By.CssSelector("tr"));

                    var prevName = "A";
                    for (var i = 1; i < tableZones.Count - 1; i++)
                    {
                        var cells = tableZones[i].FindElements(By.TagName("td"));
                        var name = cells[2].Text;
                        Assert.LessOrEqual(prevName.CompareTo(name), 0);
                        prevName = name;
                    }
                    driver.Url = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
                    tableCountries = driver.FindElement(By.CssSelector("td#content .dataTable")).FindElements(By.CssSelector("tr.row"));
                }
            }
        }

        [Test]
        public void TestSortingGeoZones()
        {
            driver.Url = "http://localhost:8080/litecart/admin/?app=geo_zones&doc=geo_zones";
            Login();

            var countryCount = driver.FindElement(By.CssSelector("td#content .dataTable")).FindElements(By.CssSelector("tr.row")).Count;
            for (var i = 0; i < countryCount; i++)
            {
                var cellName = driver.FindElement(By.CssSelector("td#content .dataTable")).FindElements(By.CssSelector("tr.row"))[i];
                cellName.FindElement(By.CssSelector("td:nth-of-type(3)")).FindElement(By.CssSelector("a")).Click();

                var tableZones = driver.FindElement(By.CssSelector("td#content .dataTable")).FindElements(By.CssSelector("tr"));
                var prevName = "A";
                for (var j = 1; j < tableZones.Count - 1; j++)
                {
                    var cells = tableZones[j].FindElements(By.TagName("td"));
                    var name = cells[2].FindElement(By.CssSelector("option[selected]")).Text;
                    Assert.LessOrEqual(prevName.CompareTo(name), 0);
                    prevName = name;
                }
                driver.Url = "http://localhost:8080/litecart/admin/?app=geo_zones&doc=geo_zones";
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
