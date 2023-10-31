using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WikiGame
{
    public class WikiGameTests
    {
        IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            /*
            // funktioniert irgendwie nid, falschi ChromeDriver Version ??
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            this.driver = new ChromeDriver(path + @"\drivers\");
            */

            this.driver = new ChromeDriver();
        }

        [Test]
        public void StartGame()
        {
            // Seite um Klammern zu testen
            //this.driver.Navigate().GoToUrl("https://de.wikipedia.org/wiki/Mabe_(Solukhumbu)");

            int linksClicked = 0;

            this.NavigateToRandomPage();
            
            while (!this.IsOnPagePhilosophy())
            {
                this.ClickNextLink();
                linksClicked++;
            }

            int test = linksClicked;

            Assert.True(this.IsOnPagePhilosophy());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        private void NavigateToRandomPage()
        {
            this.driver.Navigate().GoToUrl("https://de.wikipedia.org/wiki/Wikipedia:Hauptseite/");
            IWebElement randomPageButton = driver.FindElement(By.Id("n-randompage"));
            randomPageButton.Click();
        }

        private void ClickNextLink()
        {
            IWebElement pageText = driver.FindElement(By.CssSelector("#mw-content-text > .mw-parser-output"));

            List<IWebElement> paragraphs = pageText.FindElements(By.CssSelector(".mw-parser-output > p")).ToList();
            List<IWebElement> links = new List<IWebElement>();

            foreach (IWebElement paragraph in paragraphs)
            {
                List<IWebElement> linksInParagraph = paragraph.FindElements(By.TagName("a")).ToList();

                // auf klammern überprüfen
                if (paragraph.Text.Contains("(") && paragraph.Text.Contains(")"))
                {

                }

                foreach (IWebElement link in linksInParagraph)
                {
                    int startposition = paragraph.Text.IndexOf(link.Text);
                    int endposition = startposition + link.Text.Length - 1;
                }

                links.AddRange(linksInParagraph);
            }

            links[0].Click();
        }

        private bool IsOnPagePhilosophy()
        {
            return this.driver.Url == "https://de.wikipedia.org/wiki/Philosophie";
        }
    }
}
