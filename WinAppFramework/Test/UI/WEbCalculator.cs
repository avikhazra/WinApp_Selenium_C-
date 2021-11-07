
using NUnit.Framework;
using WinAppFramework.Common;
using WinAppFramework.PageObjects.UI;

namespace WinAppFramework.Test
{
    [TestFixture]
    class WEbCalculator: UIDriverInit
    {
        guruwebpage guruwebpage;
   
          [OneTimeSetUp]
        public void Before()
        {

         
            guruwebpage = new guruwebpage(driver);
           
        }


        [Test]
        public void _1()
        {
            Open("https://www.youtube.com/");


        }


        [OneTimeTearDown]
        public void AfterAll()
        {
            KillDriver();
        }
    }
}
