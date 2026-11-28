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
                driver, TimeSpan.FromSeconds(15));

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

        // Test 6: Check that deals can be sorted by cheapest price
        [TestMethod]
        public void SortDealsByPrice()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            SelectElement sortSelect = new SelectElement(
                driver.FindElement(By.Id("sortSelect")));

            sortSelect.SelectByValue("Price");

            driver.FindElement(By.Id("sortButton")).Click();

            wait.Until(d => d.FindElements(
                By.CssSelector(".deal .sale-price")).Count > 1);

            var prices = driver.FindElements(
                By.CssSelector(".deal .sale-price"));

            decimal firstPrice = decimal.Parse(
                prices[0].Text.Replace("$", ""));

            decimal secondPrice = decimal.Parse(
                prices[1].Text.Replace("$", ""));

            Assert.IsTrue(firstPrice <= secondPrice);
        }

        // Test 7: Check that deals can be filtered by store
        [TestMethod]
        public void FilterDealsByStore()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            SelectElement storeSelect = new SelectElement(
                driver.FindElement(By.Id("storeSelect")));

            wait.Until(d => new SelectElement(d.FindElement(By.Id("storeSelect"))).Options.Count > 1);

            storeSelect = new SelectElement(
                driver.FindElement(By.Id("storeSelect")));

            storeSelect.SelectByIndex(1);

            string selectedStore = storeSelect.SelectedOption.Text;

            driver.FindElement(By.Id("storeFilterButton")).Click();

            wait.Until(d => d.FindElements(
                By.CssSelector(".deal")).Count > 0);

            var deals = driver.FindElements(By.CssSelector(".deal"));

            foreach (var deal in deals)
            {
                Assert.IsTrue(deal.Text.Contains(selectedStore));
            }
        }
        // Test 8: Check that deals can be filtered by minimum discount
        [TestMethod]
        public void FilterDealsByDiscount()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            driver.FindElement(By.Id("discountInput")).SendKeys("50");
            driver.FindElement(By.Id("discountFilterButton")).Click();

            wait.Until(d => d.FindElements(
                By.CssSelector(".deal")).Count > 0);

            var deals = driver.FindElements(By.CssSelector(".deal"));

            foreach (var deal in deals)
            {
                string discountText = deal.FindElement(
                    By.CssSelector(".discount")).Text;

                double discount = double.Parse(
                    discountText.Replace("% OFF", ""));

                Assert.IsTrue(discount >= 50);
            }
        }
        // Test 9: Check that deals can be filtered by Metacritic score
        [TestMethod]
        public void FilterDealsByMetacritic()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            driver.FindElement(By.Id("metacriticInput")).SendKeys("80");
            driver.FindElement(By.Id("metacriticFilterButton")).Click();

            wait.Until(d => d.FindElements(
                By.CssSelector(".deal")).Count > 0);

            var deals = driver.FindElements(By.CssSelector(".deal"));

            foreach (var deal in deals)
            {
                string dealText = deal.Text;

                int score = 0;

                foreach (var line in dealText.Split('\n'))
                {
                    if (line.Contains("Metacritic:"))
                    {
                        score = int.Parse(line.Replace("Metacritic:", "").Trim());
                    }
                }

                Assert.IsTrue(score >= 80);
            }
        }

        // Test 10: Check that resetting filters clears the inputs
        [TestMethod]
        public void ResetFilters()
        {
            using IWebDriver driver = new ChromeDriver();

            WebDriverWait wait = OpenHomePage(driver);

            driver.FindElement(By.Id("searchInput")).SendKeys("Elden Ring");
            driver.FindElement(By.Id("discountInput")).SendKeys("50");

            driver.FindElement(By.Id("resetFiltersButton")).Click();

            wait.Until(d => d.FindElement(
                By.Id("searchInput")).GetAttribute("value") == "");

            string searchValue = driver.FindElement(
                By.Id("searchInput")).GetAttribute("value");

            string discountValue = driver.FindElement(
                By.Id("discountInput")).GetAttribute("value");

            Assert.AreEqual("", searchValue);
            Assert.AreEqual("", discountValue);
        }
    }
}
