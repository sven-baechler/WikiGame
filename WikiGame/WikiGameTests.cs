using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestPlatform.Utilities;

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
            //this.NavigateToRandomPage();

            this.driver.Navigate().GoToUrl("https://de.wikipedia.org/wiki/Mabe_(Solukhumbu)");

            int linksClicked = 0;

            while (linksClicked <= 10)
            {
                this.ClickNextLink();
            }

            Assert.True(linksClicked == 10, "10 Links geklickt");
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
        }
    }
}
