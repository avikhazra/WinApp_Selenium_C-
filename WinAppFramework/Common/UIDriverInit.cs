using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace WinAppFramework.Common
{

    class UIDriverInit: BaseFixture
    {
       public IWebDriver driver;

        public object Envireonment { get; private set; }

        public UIDriverInit(IWebDriver driver = null) { this.driver = driver; }

        protected IWebDriver driverStart(string drivername = "chrome")
        {

            if (drivername.ToLower().Equals("chrome"))
            {
                var options = new ChromeOptions();
                options.AddAdditionalCapability("useAutomationExtension", false);
                options.AddExcludedArgument("enable-automation");
                options.AddArgument("--disable-save-password-bubble");
                options.AddArgument("ignore-certificate-errors");
                options.AddArgument("start-maximized");
                driver = new ChromeDriver(options);
            }
            else if (drivername.ToLower().Equals("firefox"))
            {
                var options = new FirefoxOptions { AcceptInsecureCertificates = true };
                driver = new FirefoxDriver();
            }
            else if (drivername.ToLower().Equals("internetexplore"))
            {
                var options = new InternetExplorerOptions()
                {
                    IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                    IgnoreZoomLevel = true,
                    EnsureCleanSession = true
                };
                driver = new InternetExplorerDriver();
            }
            else if (drivername.ToLower().Equals("edge"))
            {

                var options = new EdgeOptions()
                {
                    UseInPrivateBrowsing = true,

                };

                driver = new EdgeDriver(options); ;
            }
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return driver;
        }
        public void sleep(double time = 1)
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

        protected IWebElement Retry(Func<IWebElement> func, int attempt = 5)
        {
            IWebElement ele = null;

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
        protected IList<IWebElement> Retry(Func<IList<IWebElement>> func, int attempt)
        {
            IList<IWebElement> ele = null;

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
        public void waitFor(Func<IWebElement> func, int attempt = 4)
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
                    try { waiting().Until(pred => func().Displayed); } catch { }

                    if (func().Displayed) return;
                }
                else
                {
                    throw e;
                }
            }
        }
        public IWebElement element(By locator, int attempt = 5)
        {
            waiting().Until(ExpectedConditions.ElementExists(locator));
            return Retry(() => driver.FindElement(locator), attempt);
        }
        public IList<IWebElement> elements(By locator, int attempt = 5)
        {
            waiting().Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
            return Retry(() => driver.FindElements(locator), attempt);
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
        public void click(Func<IWebElement> func)
        {
            try
            {
                var x = func();
                try { waiting().Until(ExpectedConditions.ElementToBeClickable(x)); } catch { }
                x.Click();
                ExtentManager.StepTest("Click : " + func.Method.Name, true, "");

            }
            catch (Exception e)
            {
                try
                {
                }
                catch (Exception x)
                {
                    ExtentManager.StepTest("Click : " + func.Method.Name, false, x.ToString(), TakeScreenShot());
                }
            }
        }
        public void click(IWebElement x)
        {
            try
            {
                try { waiting().Until(ExpectedConditions.ElementToBeClickable(x)); } catch { }
                x.Click();
                ExtentManager.StepTest("Click : on Element ", true, "");

            }
            catch (Exception e)
            {
                ExtentManager.StepTest("Click : on Element", false, e.ToString(), TakeScreenShot());
            }
        }
        public void Write(Func<IWebElement> func, string text, bool forceClear = false)
        {
            try

            {
                var ele = func();

                try { waiting().Until(ExpectedConditions.ElementToBeClickable(ele)); } catch { }
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
                ExtentManager.StepTest("Write: " + func.Method.Name, true, "");
            }
            catch (Exception e)
            {

                ExtentManager.StepTest("Write: " + func.Method.Name, true, "");
                throw e;

            }
        }
        public void Write(IWebElement ele, string text, bool forceClear = false)
        {
            try

            {
                try { waiting().Until(ExpectedConditions.ElementToBeClickable(ele)); } catch { }
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
                ExtentManager.StepTest("Write: on Element", true, "");
            }
            catch (Exception e)
            {

                ExtentManager.StepTest("Write: on Element", false, e.ToString(), TakeScreenShot());
                throw e;

            }
        }
        public void ClickJS(Func<IWebElement> func)
        {
            try
            {
                var ele = func();
                ClickJS(ele);
            }
            catch { }
        }
        public void ClickJS(IWebElement ele)
        {
            try
            {
                IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                executor.ExecuteScript("arguments[0].click();", ele);
            }
            catch { }
        }

        public void SelectByText(IWebElement ele, string text)
        {
            try
            {
                var selectElement = new SelectElement(ele);
                selectElement.SelectByText(text);
                ExtentManager.StepTest("SelectByText: on Element", true, "");
            }
            catch (Exception e)
            {
                ExtentManager.StepTest("SelectByText: on Element", false, "", TakeScreenShot());
            }
        }
        public string SelectedItem(Func<IWebElement> func, string text)
        {
            try
            {
                var ele = func();
                var selectElement = new SelectElement(ele);

                ExtentManager.StepTest("SelectedItem:  of" + func.Method.Name, true, "");
                return selectElement.SelectedOption.Text;


            }
            catch (Exception e)
            {

                ExtentManager.StepTest("SelectedItem:  of" + func.Method.Name, false, e.ToString(), TakeScreenShot());
                throw e;
            }
        }

        public void SelectByText(Func<IWebElement> func, string text)
        {
            try
            {
                var ele = func();
                var selectElement = new SelectElement(ele);
                selectElement.SelectByText(text);
                ExtentManager.StepTest("SelectByText:  of" + func.Method.Name, true, "");

            }
            catch (Exception e)
            {
                ExtentManager.StepTest("SelectByText:  of" + func.Method.Name, false, e.ToString(), TakeScreenShot());
                throw e;
            }
        }
        public void SelectRadio(Func<IWebElement> func, bool value)
        {
            try
            {
                var ele = func();
                if (value)
                {
                    ele.Click();
                    ExtentManager.StepTest("SelectRadio:  on" + func.Method.Name, true, "");
                }

            }
            catch (Exception e)
            {
                ExtentManager.StepTest("SelectRadio:  on" + func.Method.Name, false, e.ToString(), TakeScreenShot());
                throw e;
            }
        }
        public void SelectRadio(IWebElement ele, bool value)
        {
            try
            {

                if (value)
                {
                    ele.Click();
                    ExtentManager.StepTest("SelectRadio:  on Element", true, "");
                }

            }
            catch (Exception e)
            {
                ExtentManager.StepTest("SelectRadio:  on Element", false, e.ToString(), TakeScreenShot());
                throw e;
            }
        }
        public void ActionOnCheckBox(IWebElement ele, string value)
        {
            try
            {
                bool checkbox = ele.Selected;
                String Checkattribute = ele.GetAttribute("checked");

                if (Checkattribute == null || checkbox == false)
                {
                    if (value.ToLower().Equals("check"))
                    {
                        ele.Click();

                    }

                }
                else if (Checkattribute.ToLower().Contains("checked") || checkbox == true)
                {
                    if (value.ToLower().Equals("uncheck"))
                    {
                        ele.Click();

                    }
                }
                ExtentManager.StepTest("ActionOnCheckBox:" + value + " on Element", true, "");
            }
            catch (Exception e)
            {
                ExtentManager.StepTest("ActionOnCheckBox:" + value + " on Element", false, e.ToString(), TakeScreenShot());
                throw e;
            }
        }
        public void KillDriver()
        {
            driver.Close();
            driver.Quit();

        }
        public string TakeScreenShot()
        {

            string fileName = TestContext.CurrentContext.TestDirectory.Replace("bin\\Debug", "") + "Reports\\"+ "fileName_" + DateTime.Now.ToString("MMddyyyyhhmmsstt") + ".png";
            ((ITakesScreenshot)driver)
          .GetScreenshot().SaveAsFile(fileName, ScreenshotImageFormat.Png);
            return fileName;
        }
        [SetUp]
        public void BeforeALL()
        {
            driver = driverStart();
        }
       public void Open(String Url)
        {
            try
            {
                driver.Navigate().GoToUrl(Url);
                ExtentManager.StepTest("URL:  " + Url, true, "");
            }
            catch (Exception e)
            {
                ExtentManager.StepTest("URL:  " + Url, false, e.ToString(), TakeScreenShot());
            }     
        }
    }
}
