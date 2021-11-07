using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium.Windows;
using WinAppFramework.Common;

namespace WinAppFramework.PageObjects
{
    class CalculatorPageLocators : WinDriverInit
    {
        public CalculatorPageLocators(WindowsDriver<WindowsElement> driver) : base(driver) { }

        public WindowsElement One() => WIN_FindElementByName("One", 1);
          public WindowsElement Two() => WIN_FindElementByName("Two", 1);
        public WindowsElement Backspace() => WIN_FindElementByName("Backspace", 1);
    }
    class Calculator: CalculatorPageLocators
    {
        public Calculator(WindowsDriver<WindowsElement> driver) : base(driver) { }



    }
}
