
using NUnit.Framework;
using WinAppFramework.Common;
using WinAppFramework.PageObjects;

namespace WinAppFramework.Test
{
    [TestFixture]
    class Calculator2: WinDriverInit
    {
        Calculator Cal;
   
          [OneTimeSetUp]
        public void BeforeALL()
        {

            driverStart("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");


            Cal = new Calculator(driver);
           
        }
     

        [Test]
        public void _1()
        {

            Click(Cal.One);
            Click(Cal.One);

           Click(Cal.Backspace);
           
        }

        [Test]
        public void _2()
        {

           Click(Cal.Two());
           Click(Cal.Two());
         
        }

        [OneTimeTearDown]
        public void AfterAll()
        {
            KillDriver();
        }
    }
}
