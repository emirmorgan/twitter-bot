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
                if (i == 3600)
                {
                    Startup();
                    savePicture();                 
                }
                else
                {
                    i++;
                    if(i % 120 == 0)
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
            //options.AddArgument("--headless");
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            driver = new FirefoxDriver(service, options);
        }
  
        static void savePicture()
        {
            WebClient webc = new WebClient();

            //Go to site
            driver.Navigate().GoToUrl("https://unsplash.com/");
            Thread.Sleep(5000);

            string link = driver.FindElement(By.XPath("/html/body/div/div/div[4]/div[3]/div[1]/div/div/div[3]/div[1]/figure/div/a/div/img")).GetAttribute("src");
            webc.DownloadFile(link, @"C:\Users\Anonim\Desktop\img.png");
            ConsoleLog("Picture saved in desktop");

            sharePicture();
        }

        static void sharePicture()
        {          
            driver.Navigate().GoToUrl("https://www.twitter.com/login");
            Thread.Sleep(5000);

            ConsoleLog("Twitter page ready...");

            IWebElement userInput = driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/form/div/div[1]/label/div[2]/div/input");
            IWebElement passInput = driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/form/div/div[2]/label/div[2]/div/input");

            ConsoleLog("User&Pass inputs located.");

            userInput.Click();
            //Username or email - You must fill inside of userInput with your username or email.
            userInput.SendKeys("www.github.com/aemirdnr");

            Thread.Sleep(250);

            passInput.Click();
            //Password - You must fill inside of passInput with your password.
            passInput.SendKeys("www.github.com/aemirdnr");

            Thread.Sleep(250);
            driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/form/div/div[3]/div/div").Click();
            ConsoleLog("Logged in redirecting main page...");

            Thread.Sleep(4000);
            IWebElement file = driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/div/div[1]/div/div[2]/div[2]/div[1]/div/div/div[2]/div[2]/div/div/div[1]/input");
            file.SendKeys(@"C:\Users\Anonim\Desktop\img.png");

            ConsoleLog("File attached.");

            Thread.Sleep(2000);
            driver.FindElementByXPath("/html/body/div/div/div/div/main/div/div/div/div[1]/div/div[2]/div[2]/div[1]/div/div/div[2]/div[2]/div/div/div[2]/div[3]/div").Click();

            ConsoleLog("Share finished.");

            Thread.Sleep(3000);

            File.Delete(@"C:\Users\Anonim\Desktop\img.png");

            timer();
        }

        static void ConsoleLog(string s)
        {
            Console.WriteLine($"[{DateTime.Now}] {s}");
        }
    }
}
