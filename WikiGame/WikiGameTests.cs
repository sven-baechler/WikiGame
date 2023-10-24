using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
                linksClicked++;

                if (driver.Title == "Philosophie – Wikipedia")
                {
                    break;
                }
            }

            Assert.True(linksClicked == 9, "9 Links geklickt");
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

            foreach (IWebElement paragraph in paragraphs)
            {
                int indexOfFirstValidLink = this.getIndexOfFirstValidLink(paragraph.GetAttribute("innerHTML"));

                if (indexOfFirstValidLink > -1)
                {
                    List<IWebElement> linksInParagraph = paragraph.FindElements(By.TagName("a")).ToList();

                    linksInParagraph[indexOfFirstValidLink].Click();
                    return;
                }
            }

            throw new Exception("No Link found on page " + driver.Url);
        }

        private int getIndexOfFirstValidLink(string html)
        {
            int insideCursive = 0;
            int insideParenthesis = 0;
            int insideBrackets = 0;

            int linkStart = -1;
            int linkEnd = -1;

            int linkIndex = -1;

            for (int i = 0; i < html.Length; i++)
            {
                char current = html[i];

                if (current.Equals('<'))
                {
                    if (html[i + 1].Equals('i') && html[i + 2].Equals('>'))
                    {
                        insideCursive += 1;
                        i += 2;
                    }
                    else if (html[i + 1].Equals('/') && html[i + 2].Equals('i') && html[i + 3].Equals('>'))
                    {
                        insideCursive -= 1;
                        i += 3;
                    } 
                    else if (html[i + 1].Equals('a'))
                    {
                        linkStart = i;
                        i += 1;
                    }
                    else if (html[i + 1].Equals('/') && html[i + 2].Equals('a') && html[i + 3].Equals('>'))
                    {
                        linkEnd = i + 3;
                        i += 3;
                    }
                } 
                else if (current.Equals('('))
                {
                    insideParenthesis += 1;
                } 
                else if (current.Equals(')'))
                {
                    insideParenthesis -= 1;
                } 
                else if (current.Equals('['))
                {
                    insideBrackets += 1;
                } 
                else if (current.Equals(']'))
                {
                    insideBrackets -= 1;
                }


                if (linkStart > -1 && linkEnd > -1)
                {
                    linkIndex += 1;

                    if (insideCursive == 0 && insideParenthesis == 0 && insideBrackets == 0)
                    {
                        String link = html.Substring(linkStart, linkEnd - linkStart + 1);

                        if (Regex.IsMatch(link, "<a.*?>\\w*<\\/a>"))
                        {
                            return linkIndex;
                        }
                    }

                    linkStart = -1;
                    linkEnd = -1;
                }
            }

          

            return -1;
        }
    }
}
