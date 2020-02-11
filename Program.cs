using System;
using System.IO;
using System.Net;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace selBot
{
    class Program
    {
        static FirefoxDriver driver;

        static void Main(string[] args)
        {
            timer();
        }

        static void timer()
        {
            int i = 0;
            while (true)
            {
                if (i == 1800)
                {
                    Startup();
                    savePicture();
                    i = 0;
                }
                else
                {
                    i++;
                    if (i % 100 == 0)
                    {
                        Console.WriteLine(3600 - i + " Second left.");
                    }                   
                    Thread.Sleep(1000);
                }
            }
        }

        static void Startup()
        {
            FirefoxOptions options = new FirefoxOptions();
            //For make browser hide
            //options.AddArgument("--headless");
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            driver = new FirefoxDriver(service, options);
        }


        static void savePicture()
        {
            WebClient webc = new WebClient();

            driver.Navigate().GoToUrl("https://source.unsplash.com/random/1920x1080");
            Thread.Sleep(6000);

            //Picture src
            string link = driver.FindElement(By.XPath("/html/body/img")).GetAttribute("src");

            webc.DownloadFile(link, Path.GetTempPath() + "\\img.png");
           
            ConsoleLog("Picture saved.");

            sharePicture();
        }

        static void sharePicture()
        {
            //Login
            driver.Navigate().GoToUrl("https://www.twitter.com/login");

            Thread.Sleep(5000);

            ConsoleLog("Twitter page ready...");
            //Username and password textbox
            IWebElement userInput = driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/form/div/div[1]/label/div[2]/div/input");
            IWebElement passInput = driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/form/div/div[2]/label/div[2]/div/input");

            ConsoleLog("User&Pass inputs located.");

            //Click to mail input
            userInput.Click();
            //Write mail to mail input
            userInput.SendKeys("mail");

            Thread.Sleep(250);

            //Write mail to mail input
            passInput.Click();
            //Write pass to pass input
            passInput.SendKeys("password");

            Thread.Sleep(250);

            //Login button
            driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/form/div/div[3]/div/div").Click();

            ConsoleLog("Logged in redirecting main page...");

            Thread.Sleep(4000);

            //Select upload input.
            IWebElement file = driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/div/div[1]/div/div[2]/div[2]/div[1]/div/div/div[2]/div[2]/div/div/div[1]/input");

            //Send picture to input.
            file.SendKeys(Path.GetTempPath() + "\\img.png");

            ConsoleLog("File attached.");

            Thread.Sleep(3000);

            //Click to tweet button.
            driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/div/div[1]/div/div[2]/div[2]/div[1]/div/div/div[2]/div[2]/div/div/div[2]/div[3]/div").Click();

            ConsoleLog("Share finished.");

            Thread.Sleep(6000);

            //Delete picture after tweet it.
            File.Delete(Path.GetTempPath() + "\\img.png");

            Thread.Sleep(1000);

            driver.Quit();

            timer();
        }

        static void ConsoleLog(string s)
        {
            Console.WriteLine($"[{DateTime.Now}] {s}");
        }
    }
}
