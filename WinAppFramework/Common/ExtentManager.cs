using System;
using System.Runtime.CompilerServices;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace WinAppFramework.Common
{
    class ExtentManager
    {
        private static readonly Lazy<ExtentReports> _lazy = new Lazy<ExtentReports>(() => new ExtentReports());

        public static ExtentReports Instance { get { return _lazy.Value; } }

        static ExtentManager()
        {
            var adress = TestContext.CurrentContext.TestDirectory.Replace("bin\\Debug", "") + "Reports\\Extent.html";
            var htmlReporter = new ExtentHtmlReporter(adress);
           

            Instance.AttachReporter(htmlReporter);
        }
        private ExtentManager()
        {
        }
        [ThreadStatic]
        private static ExtentTest _parentTest;

        [ThreadStatic]
        private static ExtentTest _childTest;

        [ThreadStatic]
        private static ExtentTest _Step;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateParentTest(string testName, string description = null)
        {
            _parentTest = ExtentManager.Instance.CreateTest(testName, description);
            return _parentTest;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateTest(string testName, string description = null)
        {
            _childTest = _parentTest.CreateNode("Test case: "+testName, description);
            return _childTest;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest StepTest(String method, bool status, string details, string Screenshot = "")
        {
            

            if (method != "")
            {
                _Step = _childTest.CreateNode(method);
                if (status)
                {
                    _Step.Log(Status.Pass, MarkupHelper.CreateLabel("Passed", ExtentColor.Green));
                    if (!Screenshot.Equals(""))
                    {
                        _Step.Pass("Passed").AddScreenCaptureFromPath(Screenshot);
                    }
                }
                else
                {
                    _Step.Log(Status.Fail, MarkupHelper.CreateLabel("Failed", ExtentColor.Red));
                    _Step.Fail(details).AddScreenCaptureFromPath(Screenshot);

                }
            }
            else
            {
                _Step.Fail(MarkupHelper.CreateLabel(details, ExtentColor.Grey));
            }
            return _Step;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest GetTest()
        {
            return _childTest;
        }
    }



}
