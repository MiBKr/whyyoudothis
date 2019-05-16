using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Alma
{

    public class AlmaPrinter
    {
        ChromeOptions chromeOptions = new ChromeOptions();
        public string bookPath  { get; set; }
        public string outputPath { get; set; }
        public string BookName { get; set; }
        public AlmaPrinter(string folPath)
        {
            chromeOptions.AddArguments("--disable-print-preview");
            SetDefaultPrinter("Microsoft Print to PDF");
            bookPath = folPath;
            outputPath = folPath;
        }

        public bool GetBook(string id)
        {

            string username = "user";
            string password = "pass";

            using (var chromeDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options: chromeOptions))
            {
                chromeDriver.Navigate().GoToUrl("https://fokus-almatalent-fi.libproxy.aalto.fi/teos/" + id);
                var wait = new WebDriverWait(chromeDriver, TimeSpan.FromMinutes(1));
                IWebElement submitbtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("_eventId_proceed")));
                IWebElement user = chromeDriver.FindElementById("username");
                IWebElement passw = chromeDriver.FindElementById("password");
                user.Clear();
                passw.Clear();
                user.SendKeys(username);
                passw.SendKeys(password);
                submitbtn.Click();
                IWebElement fuckingpopup = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("alma-data-policy-banner__accept-cookies-only")));
                System.Threading.Thread.Sleep(500);
                 
                fuckingpopup.Click();
                LoopBook(chromeDriver);
            }
            return true;
        }
        private void PrintPart(string partName)
        {
            SendKeys.SendWait("^p");
            System.Threading.Thread.Sleep(3000);
            SendKeys.SendWait("~");
            // give it a minute to spool onto the printer
            System.Threading.Thread.Sleep(3500);
            SendKeys.SendWait(bookPath + @"\" + partName + ".pdf");
            System.Threading.Thread.Sleep(3000);
            SendKeys.SendWait(@"{Enter}");
            System.Threading.Thread.Sleep(3000);


        }
        
        private void LoopBook(ChromeDriver chromeDriver)
        {
            IWebElement toc = chromeDriver.FindElementByCssSelector("#toc > ul");
            BookName = chromeDriver.FindElementByCssSelector("#displayname").Text;
            bookPath = Path.Combine(bookPath, BookName);
            outputPath = Path.Combine(outputPath, BookName + ".pdf");
            if (Directory.Exists(bookPath))            
                Directory.Delete(bookPath, true);

            System.Threading.Thread.Sleep(1000);
            System.IO.Directory.CreateDirectory(bookPath);
            Console.WriteLine(toc);
            int ensureOrder = 1;
            string lastUrl = "";
            try { chromeDriver.ExecuteScript("document.getElementById('_hj_poll_container').remove();"); } catch { }

            IWebElement nextBtn = chromeDriver.FindElementByCssSelector("#naviForward");
            while (lastUrl != chromeDriver.Url)
            {
                System.Threading.Thread.Sleep(3000);
                try { chromeDriver.ExecuteScript("document.getElementById('_hj_poll_container').remove();"); } catch { }
                System.Threading.Thread.Sleep(3000);
                lastUrl = chromeDriver.Url;
                PrintPart(ensureOrder.ToString("D4"));

                System.Threading.Thread.Sleep(3000);
                nextBtn.Click();
                System.Threading.Thread.Sleep(3000);
                ensureOrder += 1;
            }
        }

        [DllImport("winspool.drv",
           CharSet = CharSet.Auto,
           SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetDefaultPrinter(String name);
    }
}
