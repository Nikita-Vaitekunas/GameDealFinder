using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GameDealFinder.Tests
{
    [TestClass]
    public class HomePageTests
    {
        [TestMethod]
        public void HomePageLoads()
        {
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://localhost:7299/");

            Assert.IsTrue(driver.Title.Contains("GameDealFinder"));

            driver.Quit();
        }
        [TestMethod]
        public void SearchInputExists()
        {
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://localhost:7299/");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(d => d.PageSource.Contains("Search Games"));

            Assert.IsTrue(driver.PageSource.Contains("Search Games"));

            driver.Quit();
        }
        [TestMethod]
        public void SearchButtonExists()
        {
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://localhost:7299/");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(d => d.PageSource.Contains("Search Games"));

            IWebElement button = driver.FindElement(By.XPath("//button[text()='Search']"));

            Assert.IsTrue(button.Displayed);

            driver.Quit();
        }
    }
}