using System;
using System.Diagnostics;
using BourseInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BourseInfoTests
{
    [TestClass]
    public class MainFormTests
    {
        [TestMethod]
        public void HttpRequestsOnApiUrlsReturnsNotNull()
        {
            MainForm mf = new MainForm();
            Stopwatch stopWatch = new Stopwatch();
            

            for (int i = 0 ; i < mf.JsonUrls.Count ; i++)
            {
                stopWatch.Restart();
                var res = WebController.GetString(mf.JsonUrls[i]);
                stopWatch.Stop();
                Assert.IsNotNull(res);

                Trace.WriteLine("Url " + i + ": " + (stopWatch.ElapsedMilliseconds / 1000.0) + "s (" + mf.JsonUrls[i] + ")");
            }
            
        }
    }
}
