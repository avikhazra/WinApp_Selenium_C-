using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace WinAppFramework.Common
{
  
    class WinDriverInit: BaseFixture
    {

        public  WindowsDriver<WindowsElement> driver;
      public WinDriverInit(WindowsDriver<WindowsElement> driver = null)
        {
            this.driver = driver;
        }
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        public WindowsDriver<WindowsElement> driverStart(string AppName_Address)
        {
            Process.Start(@"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe");
            if (driver == null)
            {
                AppiumOptions appCapabilities = new AppiumOptions();
                appCapabilities.AddAdditionalCapability("app", AppName_Address);
                appCapabilities.AddAdditionalCapability("deviceName", "WindowsPC");
                appCapabilities.AddAdditionalCapability("ms:waitForAppLaunch", "25");
                driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                //driver.Manage().Window.Maximize();
            }
            return driver;
        }

        public void KillDriver()
        {
            driver.Close();
           
            driver.Quit();

            driver = null;
            Process.GetCurrentProcess().Close();
            sleep();

        }
        protected WindowsElement Retry(Func<WindowsElement> func, int attempt)
        {
            WindowsElement ele = null;
           
            try
            {
                ele = func();
                return ele;
            }catch(Exception e)
            {
                if (attempt > 0)
                {
                    attempt--;
                    sleep();
                    Retry(func, attempt);

                }
                else
                {
                    throw e;
                }
            }

            return ele;
        }
      
        protected ReadOnlyCollection<WindowsElement> Retry(Func<ReadOnlyCollection<WindowsElement>> func, int attempt )
        {
            ReadOnlyCollection<WindowsElement> ele = null;
        
            try
            {
                ele = func();
                return ele;
            }
            catch (Exception e)
            {
                if (attempt > 0)
                {
                    attempt--;
                    sleep();
                    Retry(func, attempt);


                }
                else
                {
                    throw e;
                }
            }

            return ele;
        }
 
        public WindowsElement WIN_FindElementByXPath(string locator, int attempt=5)
        {
            try
            {

                WindowsElement x = Retry(() => driver.FindElementByXPath(locator), attempt);
              
                return x;
            }
            catch(Exception r)
            {
               // Step("WIN_FindElementByXPath", false, r.ToString(),true);
                throw r;
            }

        }
        public WindowsElement WIN_FindElementByTagName(string locator, int attempt=5)
        {
            try
            {
                WindowsElement e= Retry(() => driver.FindElementByTagName(locator), attempt);
               
                return e;
            }
            catch(Exception x)
            {
               // Step("WIN_FindElementByTagName", false, x.ToString(), true);
                throw x;
            }
        }
        public WindowsElement WIN_FindElementByName(string locator, int attempt = 5)
        {
            try
            {
                WindowsElement e = Retry(() => driver.FindElementByName(locator), attempt);
           
                return e;
            }
            catch(Exception x)
            {
                //Step("WIN_FindElementByTagName", false, x.ToString(), true);
                throw x;
            }
        }
        public WindowsElement WIN_FindElementById(string locator, int attempt = 5)
        {
            try
            {
                WindowsElement e = Retry(() => driver.FindElementById(locator), attempt);
               
                return e;
            }
            catch (Exception x)
            {
               // Step("WIN_FindElementById", false, x.ToString(), true);
                throw x;
            }
        }
        public WindowsElement WIN_FindElementByClassName(string locator, int attempt = 5)
        {
            try
            {
                WindowsElement e = Retry(() => driver.FindElementById(locator), attempt);
                
                return e;
            }
            catch(Exception x)
            {
                //Step("WIN_FindElementByClassName", false, x.ToString(), true);
                throw x;
            }
        }
        public WindowsElement WIN_FindElementByAccessibilityId(string locator, int attempt = 5)
        {
            try
            {
                WindowsElement e = Retry(() => driver.FindElementByAccessibilityId(locator), attempt);
     
                return e;
            }
            
             catch (Exception x)
            {
                //Step("WIN_FindElementByAccessibilityId", false, x.ToString(), true);
                throw x;
            }
        }
        public ReadOnlyCollection<WindowsElement> WIN_WindowsElementSByXpath(string locator, int attempt = 5)
        {
            try
            {
                ReadOnlyCollection<WindowsElement> e = Retry(() => driver.FindElementsByXPath(locator), attempt);
                
                return e;
            }
            catch(Exception x)
            {
                //Report.Step("WIN_WindowsElementSByXpath", false, x.ToString(), true);
                throw x;
            }
        }
        public void sleep(double time=1)
        {
            if (time > 0)
            {
                double actualtime = time / 2;
                try { Task delay = Task.Delay(Convert.ToInt32(actualtime * 1000)); } catch { }
                try { Thread.Sleep(Convert.ToInt32(actualtime * 1000)); } catch { }
            }
            else
            {
                throw new Exception("time cant be zero");
            }
        }
        public WebDriverWait waiting(int time = 10)
        {
            
            if (time > 0)
            {
                WebDriverWait waitForMe = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
                {
                    var PollingInterval = TimeSpan.FromSeconds(5);
                };
                return waitForMe;
            }
            else
            {
                throw new Exception("It cant be zero");
            }
            
        }
        public void waitFor(Func<WindowsElement> func, int attempt = 4)
        {
            
            try
            {
                var ele = func();
                if (ele.Displayed)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                if (attempt > 0)
                {
                    attempt--;
                    sleep();
                    try { waiting().Until(pred => func().Displayed); } catch { }
                     waitFor(func, attempt);
                }
                else
                {
                    throw e;
                }
            }
        }
        public void waitFor(WindowsElement e, int attempt = 2)
        {

            try
            {
              
                if (e.Displayed)
                {
                    return;
                }
            }
            catch (Exception x)
            {
                if (attempt > 0)
                {
                    attempt--;
                    try { waiting().Until(pred => e.Displayed); } catch { }
                    waitFor(e, attempt);
                }
                else
                {
                    throw x;
                }
            }
        }
        public void Click(Func<WindowsElement> func)
        {
            try
            {
                var ele = func();
                ele.Click();
            
                ExtentManager.StepTest("Click : "+ func.Method.Name, true, "");
                return;
            }
            catch(Exception e)
            {

                ExtentManager.StepTest("Click : " + func.Method.Name, false, e.ToString(), TakeScreenShot());
                    throw e;
                
            }
        }
        public void Click(WindowsElement ele)
        {
            try
            {
               
                ele.Click();
              
                ExtentManager.StepTest("Click : on Element", true, "");
                return;
            }
            catch (Exception e)
            {

                ExtentManager.StepTest("Click : on Element", false, e.ToString(), TakeScreenShot());
                throw e;

            }
        }


        public void Write(Func<WindowsElement> func, string text,bool forceClear=false)
        {
            try
                
            {
                var ele = func();
                if (forceClear)
                {
                    ele.SendKeys(Keys.Control + "a");
                    ele.SendKeys(Keys.Delete);

                }
                else
                {
                    ele.Clear();
                }
                if (!text.Equals(""))
                {
                    ele.SendKeys(text);
                }
                ExtentManager.StepTest("Write: "+func.Method.Name, true, "");
            }
            catch (Exception e)
            {

                ExtentManager.StepTest("Write: " + func.Method.Name, true, "");
                throw e;
              
            }
        }
        public void Write(WindowsElement ele, string text, bool forceClear = false)
        {
            try

            {
              
                if (forceClear)
                {
                    ele.SendKeys(Keys.Control + "a");
                    ele.SendKeys(Keys.Delete);

                }
                else
                {
                    ele.Clear();
                }
                if (!text.Equals(""))
                {
                    ele.SendKeys(text);
                }
                ExtentManager.StepTest("Write: on Element" , true, "");
            }
            catch (Exception e)
            {

                ExtentManager.StepTest("Write: on Element" , false, e.ToString(), TakeScreenShot());
                throw e;

            }
        }
        public void ClickJS(Func<WindowsElement> func)
        {
            try
            {
                var ele = func();
                ele.Click();
            }
            catch { }
          
        }
        public string TakeScreenShot()
        {

            string fileName = TestContext.CurrentContext.TestDirectory.Replace("bin\\Debug", "") + "Reports\\" + "fileName_" + DateTime.Now.ToString("MMddyyyyhhmmsstt") + ".png";
            ((ITakesScreenshot)driver)
          .GetScreenshot().SaveAsFile(fileName, ScreenshotImageFormat.Png);
            return fileName;
        }



    }
}
