using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
    }
}