using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using WinAppFramework.Common;

namespace WinAppFramework.PageObjects.UI
{
    class guruwebpageLocator: UIDriverInit
    {
        public guruwebpageLocator(IWebDriver driver) : base(driver) { }

        public IWebElement one() => element(By.XPath(""));
    }
    class guruwebpage : guruwebpageLocator
    {
        public guruwebpage(IWebDriver driver) : base(driver) { }

       
    }
}
