using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse
{
    [TestFixture]
    public class RegistrationTest
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

            var loginForm = driver.FindElement(By.CssSelector("div#box-account-login form[name = login_form]"));
            var linkRegistation = loginForm.FindElement(By.CssSelector("table > tbody > tr:nth-of-type(5) a"));
            linkRegistation.Click();

            var emailPart1 = RandomString(7);
            var emailPart2 = RandomString(5);
            var fullEmail = emailPart1 + "@" + emailPart2 + ".ru";
            var password = "password";

            Registration(fullEmail, password);
            Logout();
            Login(fullEmail, password);
            Logout();
        }

        public void Registration(string email, string password)
        {
            var registrationForm = driver.FindElements(By.CssSelector("div.content > form[name=customer_form] > table > tbody > tr > td"));
            registrationForm[2].FindElement(By.Name("firstname")).SendKeys("firstname");
            registrationForm[3].FindElement(By.Name("lastname")).SendKeys("lastname");
            registrationForm[4].FindElement(By.Name("address1")).SendKeys("address1");
            registrationForm[6].FindElement(By.Name("postcode")).SendKeys("12345");
            registrationForm[7].FindElement(By.Name("city")).SendKeys("city");

            var selectCountry = new SelectElement(registrationForm[8].FindElement(By.Name("country_code")));
            selectCountry.SelectByValue("US");
            
            registrationForm[10].FindElement(By.Name("email")).SendKeys(email);
            registrationForm[11].FindElement(By.Name("phone")).SendKeys("123456789");
            registrationForm[14].FindElement(By.Name("password")).SendKeys(password);
            registrationForm[15].FindElement(By.Name("confirmed_password")).SendKeys(password);

            registrationForm[16].FindElement(By.Name("create_account")).Click();
        }

        public void Login(string email, string password)
        {
            var loginFormElements = driver.FindElements(By.CssSelector("div#box-account-login form[name = login_form] table > tbody > tr > td"));
            loginFormElements[0].FindElement(By.Name("email")).SendKeys(email);
            loginFormElements[1].FindElement(By.Name("password")).SendKeys("password");
            loginFormElements[3].FindElement(By.CssSelector("button[name=login]")).Click();
        }

        public void Logout()
        {
            var loginInfoForm = driver.FindElement(By.CssSelector("div#box-account"));
            var linkLogout = loginInfoForm.FindElement(By.CssSelector("div.content > .list-vertical > li:nth-child(4) a"));
            linkLogout.Click();
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
