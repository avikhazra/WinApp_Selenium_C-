using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace WinAppFramework.Common
{
    public class BaseFixture
    {
        [OneTimeSetUp]
        public void Setup()
        {
            ExtentManager.CreateParentTest(GetType().Name);
        }

        [OneTimeTearDown]
        protected void TearDown()
        {

            ExtentManager.Instance.Flush();
        }

        [SetUp]
        public void BeforeTest()
        {
            ExtentManager.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void AfterTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
           var msg = string.IsNullOrEmpty(TestContext.CurrentContext.Result.Message)
        ? ""
        : string.Format("<pre>{0}</pre>", "  Message: "+TestContext.CurrentContext.Result.Message);

   
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }

            ExtentManager.GetTest().Log(logstatus, "Test ended with " + logstatus + stacktrace+ msg);
        }
    }
}
