using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System;

namespace TestingFindOutlier
{
    [TestClass]
    public class TestLogger
    {
        [TestMethod]
        public void TestEmptyFileNameIsRtnException()
        {
            bool check = false;
            bool isRtnFalse = false;
            try
            {
                Logger logger = new Logger("");
                isRtnFalse = logger.Write("write something");
            }
            catch
            {
                check = true;
            }
            Assert.IsTrue(check && !isRtnFalse);
        }
        [TestMethod]
        public void TestInvalidFileNameIsRtnException()
        {
            bool isRtnFalse = false;
            try
            {
                Logger logger = new Logger(@"c:\rqwejlfj\test.log");
                isRtnFalse = logger.Write("write something");
            }
            catch 
            {
                
            }
            Assert.IsTrue(!isRtnFalse);
        }

    }
}
