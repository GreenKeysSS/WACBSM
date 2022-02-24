using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Keys = OpenQA.Selenium.Keys;


namespace Presentation
{
  
    public  class WA 
    {
        

       
        public static IWebDriver driver;
        public static IWebDriver driver2;
        
        public bool driverstate;
        public bool driverstate2;
        public bool clickstate = false;
        public int preventblocktiming = 0;
        public int preventblocktiming2 = 0;



        private static string actualuser = Environment.UserName;
        private static string AttachXPath = "//body[1]/div[1]/div[1]/div[1]/div[4]/div[1]/footer[1]/div[1]/div[1]/span[2]/div[1]/div[1]/div[2]/div[1]/div[1]/span[1]";

        public async Task LaunchBrowser()
        {
            
            

            await Task.Run(() =>
            {
                try
                {
                    string userProfile = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome WA Profile\\Default\\";

                    var service = ChromeDriverService.CreateDefaultService("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\webdriver\\");
                    service.HideCommandPromptWindow = true;
                    ChromeOptions options = new ChromeOptions();

                    options.AddArguments("user-data-dir=" + userProfile);
                  


                    driver = new ChromeDriver(service, options)
                    {
                        Url = ("https://web.whatsapp.com/")
                    };

                    
                        driverstate = true;




                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }

                

        

            });







        }
       
        public async Task LaunchBrowser2()
        {

            await Task.Run(() =>
            {


                string userProfile = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome SMS Profile\\Default\\";

                var service = ChromeDriverService.CreateDefaultService("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\webdriver\\");
                service.HideCommandPromptWindow = true;
                ChromeOptions options = new ChromeOptions();

               options.AddArguments("user-data-dir=" + userProfile);


                driver2 = new ChromeDriver(service, options)
                {
                    Url = ("https://messages.google.com/web/conversations")
                    //Url = ("https://google.com")



                };
               

                driverstate2 = true;



            });







        }

        
        public void ContactSearch(string tosearch)
        {

            try
            {
                

                if (FindElement(driver, By.XPath("//body//label//div[2]"), 20))
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;


                    string _script1 = "document.evaluate" +
                        "('//body//label//div[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML='" + tosearch + "'";
                    string _script2 = "document.evaluate" +
                        "('//body//label//div[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML=''";


                    string title2 = (string)js.ExecuteScript(_script2);
                    string title = (string)js.ExecuteScript(_script1);

                }

               


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }







        }
        public void ContactSearch2(string tosearch)
        {

            try
            {
               

                /*
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver2;


                string _script1 = "document.evaluate" +
                    "('//mw-new-conversation-sub-header/div[1]/div[2]/mw-contact-chips-input[1]/div[1]/mat-chip-list[1]/div[1]/input[1]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML='" + tosearch + "'";
                string _script2 = "document.evaluate" +
                    "('//mw-new-conversation-sub-header/div[1]/div[2]/mw-contact-chips-input[1]/div[1]/mat-chip-list[1]/div[1]/input[1]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML=''";


                string title2 = (string)js.ExecuteScript(_script2);
                string title = (string)js.ExecuteScript(_script1);
                */
                if (FindElement(driver, By.XPath("//mw-new-conversation-sub-header/div[1]/div[2]/" +
                                  "mw-contact-chips-input[1]/div[1]/mat-chip-list[1]/div[1]/input[1]"), 20))
                {
                    driver2.FindElement(By.XPath("//mw-new-conversation-sub-header/div[1]/div[2]/" +
                                   "mw-contact-chips-input[1]/div[1]/mat-chip-list[1]/div[1]/input[1]")).SendKeys(tosearch);

                }

               


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }







        }

        public void CloseWDriver()
        {

          
            if (driverstate)
            {
                try
                {
                    
                    driver.Quit();
                   
                    /*
                    Process[] _proceses = null;
                    _proceses = Process.GetProcessesByName("chromedriver");

                    if (_proceses.Length != 0)
                    {
                        foreach (Process proces in _proceses)
                        {
                            proces.Kill();
                        }
                    }*/
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }





            }



        }
        public void CloseWDriver2()
        {
            

            if (driverstate2)
            {
                try
                {


                    driver2.Quit();
                    /*
                    Process[] _proceses = null;
                    _proceses = Process.GetProcessesByName("chromedriver");

                    if (_proceses.Length != 0)
                    {
                        foreach (Process proces in _proceses)
                        {
                            proces.Kill();
                        }
                    }*/
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }





            }



        }
      
       


        
        public static bool FindElement(IWebDriver driver, By by, int timeoutInSeconds)
        {
            
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();
            
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement SearchResult = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
            


        }

        public void ContactClick()
        {

            

            Actions action = new Actions(driver);


            if (FindElement(driver, By.XPath("//div[contains(text(),'Chats')]"),20) || FindElement(driver,By.XPath("//div[contains(text(),'Contactos')]"), 20)  )
            {
                clickstate = true;
                action.SendKeys(Keys.Enter).Build().Perform();
                Console.WriteLine("contact or group founded");
              
            }
            else
            {

                clickstate = false;
                Console.WriteLine("not founded contact o group");
            }



            //action.MoveToElement(driver.FindElement(By.XPath("//body//div[@id='pane-side']//div//div//div//div[1]//div[1]//div[1]//div[1]//div[1]//div[1]//span[1]"))).Click().Perform();



        }
        public void ContactClick2()
        {

           

            try
            {
                Actions action = new Actions(driver2);
                action.SendKeys(Keys.Enter).Build().Perform();
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            
            

        }
        public void ContactMessage(string totype)
        {
            

            try
            {
               

                if (FindElement(driver,By.XPath("//body//footer//div//div//div//div[2]"),20))
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                    string _script2 = "document.evaluate" +
                    "('//body[1]/div[1]/div[1]/div[1]/div[4]/div[1]/footer[1]/div[1]/div[1]/span[2]/div[1]/div[2]/div[1]/div[1]/div[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML='" + totype + "'";


                    string title = (string)js.ExecuteScript(_script2);
                }

               

                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public void ContactMessage2(string totype)
        {

            try
            {
                

                if (FindElement(driver, By.XPath("//body/mw-app[1]/mw-bootstrap[1]/div[1]/main[1]/mw-main-container[1]/div[1]/mw-conversation-container[1]/div[1]/div[1]" +
                    "/mws-message-compose[1]/div[1]/div[2]/div[1]/mws-autosize-textarea[1]/textarea[1]"), 20))
                {
                    driver2.FindElement(By.XPath("//body/mw-app[1]/mw-bootstrap[1]/div[1]/main[1]/mw-main-container[1]/div[1]/mw-conversation-container[1]/div[1]/div[1]" +
                   "/mws-message-compose[1]/div[1]/div[2]/div[1]/mws-autosize-textarea[1]/textarea[1]")).SendKeys(totype);
                }
                   

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public void ContactFileImage(string imagedir,string totype)
        {



            try
            {

          

                if (FindElement(driver, By.XPath(AttachXPath), 10))
                {
                    driver.FindElement(By.XPath(AttachXPath)).Click();
                    Task.Delay(1000).Wait();


                    IWebElement uploadElement = driver.FindElement(By.XPath("//body[1]/div[1]/div[1]/div[1]/div[4]/div[1]/footer[1]/div[1]/div[1]/span[2]/div[1]/div[1]/div[2]/div[1]/span[1]/div[1]/div[1]/ul[1]/li[1]/button[1]/input[1]"));
                    uploadElement.SendKeys(imagedir);




                    Task.Delay(1000 + preventblocktiming).Wait();


                    if (FindElement(driver, By.XPath("//body[1]/div[1]/div[1]/div[1]/div[2]/div[2]/span[1]/div[1]/span[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]"), 10))
                    {

                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                        string _script2 = "document.evaluate" +
                        "('//body[1]/div[1]/div[1]/div[1]/div[2]/div[2]/span[1]/div[1]/span[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML='" + totype + "'";



                        string title = (string)js.ExecuteScript(_script2);

                    }


                   


                }


             



            }
            catch (Exception ex)
            {


                Console.WriteLine(ex.Message);
            }

        }
        public void ContactFile(string filedir)
        {



            try
            {

           

                if (FindElement(driver, By.XPath("//body/div[@id='app']/div/div/div/div[@id='main']/footer/div/div/div/div/div/span[1]"), 10))
                {
                    driver.FindElement(By.XPath("//body/div[@id='app']/div/div/div/div[@id='main']/footer/div/div/div/div/div/span[1]")).Click();

                    Task.Delay(2000).Wait();

                    
                    IWebElement uploadElement = driver.FindElement(By.XPath("//li[3]//button[1]//input[1]"));
                    uploadElement.SendKeys(filedir);
                    
                    Task.Delay(3000 + preventblocktiming).Wait();
                }
               

                if (FindElement(driver, By.XPath("//div[contains(text(),'1 documento que intentaste añadir es mayor al lími')]"),10))
                {
             
                    driver.FindElement(By.XPath("//div[@id='app']//div//div//div//div//div//span//div//div//header//div//div//span")).Click();

                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public void ContactFileAudio(string audiodir)
        {



            try
            {

               

                if (FindElement(driver, By.XPath(AttachXPath), 10))
                {
                    driver.FindElement(By.XPath(AttachXPath)).Click();
                    Task.Delay(2000).Wait();


                    IWebElement uploadElement = driver.FindElement(By.XPath("//body[1]/div[1]/div[1]/div[1]/div[4]/div[1]/footer[1]/div[1]/div[1]/span[2]/div[1]/div[1]/div[2]/div[1]/span[1]/div[1]/div[1]/ul[1]/li[1]/button[1]/input[1]"));

                    uploadElement.SendKeys(audiodir);

                    Task.Delay(3000 + preventblocktiming).Wait();

                }

               

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

        }
        public void ContactSend()
        {
            try
            {
                new Actions(driver).SendKeys(Keys.Enter).Build().Perform();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
           

        }
        public void ContactSend2()
        {
            try
            {
                new Actions(driver2).SendKeys(Keys.Enter).Build().Perform();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
            }


        }
        public  StringBuilder GetContactsFromGroup(string tosearch)
        {
            StringBuilder str = new StringBuilder();

            Actions action = new Actions(driver);


            driver.FindElement(By.XPath("//div[@id='side']//div//div//label")).Click();


            ContactSearch(tosearch);

            Task.Delay(1000).Wait();

            action.SendKeys(Keys.Space).Build().Perform();
            
            ContactClick();

      

            if (clickstate)
            {

                Task.Delay(5000).Wait();


                if (FindElement(driver,By.XPath("//header/div[2]/div[2]"),10))
                {
                    IList<IWebElement> selectElements = driver.FindElements(By.XPath("//header/div[2]/div[2]"));



                    string[] allText = new string[selectElements.Count];

                    int a = 0;

                    foreach (IWebElement element in selectElements)
                    {
                        allText[a++] = element.Text;

                    }


                    str.Append("First Name,Mobile Phone");
                    str.AppendLine();
                    str.Append(",");
                    foreach (var item in allText)
                    {

                        str.Append(item.ToString());
                    }
                }
               
            }



            return str;

            

        }
        private bool IfSizeError(By by)
        {

            try
            {

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                driver.FindElement(by);

                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public bool IfConnected(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public bool IfConnected2(By by)
        {
            try
            {
                driver2.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        public bool IsBrowserClosed(IWebDriver driver)
        {
            bool isClosed = false;

            try
            {
                string myTitle = driver.Title;
            }
            catch ( WebDriverException)
            {
                isClosed = true;
            }

            return isClosed;
        }
        public void  LogoutWA()
        {
          
            driver.FindElement(By.XPath("//header//div[3]//div[1]//span[1]")).Click();
            

            if (FindElement(driver, By.XPath("//body//li[7]"), 10))
            { driver.FindElement(By.XPath("//body//li[7]")).Click(); }

              


        }
    }
}
