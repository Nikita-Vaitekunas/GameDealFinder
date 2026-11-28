
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GameDealFinder.Tests
{
    [TestClass]
    public class HomePageTests
    {
        // Open the homepage and wait for it to load
        private static WebDriverWait OpenHomePage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://localhost:7299/");

            WebDriverWait wait = new WebDriverWait(
                driver, TimeSpan.FromSeconds(10));

            wait.Until(d => d.PageSource.Contains("Search Games"));

            return wait;
        }

        // Test 1: Check that the homepage loads
        [TestMethod]
        public void HomepageLoads()
        {
            using IWebDriver driver = new ChromeDriver();

            OpenHomePage(driver);

            Assert.IsTrue(driver.Title.Contains("GameDealFinder"));
        }

        // Test 2: Search for a game
        [TestMethod]
        public void SearchReturnsResults()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            driver.FindElement(By.Id("searchInput")).SendKeys("Elden Ring");
            driver.FindElement(By.Id("searchButton")).Click();

            wait.Until(d => d.FindElements(
                By.CssSelector(".deal h3")).Count > 0);

            var results = driver.FindElements(By.CssSelector(".deal h3"));

            bool foundGame = false;

            foreach (var result in results)
            {
                if (result.Text.ToLower().Contains("elden ring"))
                {
                    foundGame = true;
                }
            }

            Assert.IsTrue(foundGame, "Elden Ring was not found in the results.");
        }

        // Test 3: Check what happens when no games are found
        [TestMethod]
        public void SearchWithNoResults()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            driver.FindElement(By.Id("searchInput"))
                .SendKeys("zzzz_no_matching_game_12345");

            driver.FindElement(By.Id("searchButton")).Click();

            wait.Until(d => d.PageSource.Contains("No deals found."));

            Assert.IsTrue(driver.PageSource.Contains("No deals found."));
        }

        // Test 4: Check that clearing a search restores the deals
        [TestMethod]
        public void ClearSearch()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            driver.FindElement(By.Id("searchInput"))
                .SendKeys("zzzz_no_matching_game_12345");

            driver.FindElement(By.Id("searchButton")).Click();

            wait.Until(d => d.PageSource.Contains("No deals found."));

            driver.FindElement(By.Id("clearButton")).Click();

            wait.Until(d => d.FindElements(
                By.CssSelector(".deal")).Count > 0);

            Assert.IsTrue(driver.FindElements(
                By.CssSelector(".deal")).Count > 0);
        }

        // Test 5: Check that invalid price ranges show an error
        [TestMethod]
        public void InvalidPriceRangeShowsError()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            driver.FindElement(By.Id("minPriceInput")).SendKeys("50");
            driver.FindElement(By.Id("maxPriceInput")).SendKeys("10");

            driver.FindElement(By.Id("priceFilterButton")).Click();

            wait.Until(d => d.PageSource.Contains(
                "Minimum price cannot be higher than maximum price."));

            Assert.IsTrue(driver.PageSource.Contains(
                "Minimum price cannot be higher than maximum price."));
        }
    }
}
