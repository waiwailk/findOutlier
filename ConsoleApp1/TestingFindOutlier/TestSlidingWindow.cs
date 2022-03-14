using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Methodologies;
using AlgoLibrary;
using FindOutlierApp;
using Utils;
using System;

namespace TestingFindOutlier
{
    class MockLogger : Logger
    {
        public MockLogger(string fileName) : base(fileName)
        {
        }
    }
    class MockProcessor : IOutlierProcessor
    {
        private List<String> checkResult = new List<string>();
        public void Request(int movingIdx, int lookupRng)
        {
            checkResult.Add(String.Format("{0},{1}", movingIdx, lookupRng));
        }
        
        public List<string> GetTestResult()
        {
            return checkResult;
        }

    }


    [TestClass]
    public class TestSlidingWindow
    {
        [TestMethod]
        public void TestPassNullPtrIntoConstructor()
        {
            List<DailyTick> mockDailyTick = new List<DailyTick>();
            IOutlierProcessor mockProc = new MockProcessor();
            Logger mockLogger = new MockLogger("");

            bool hasRtn = false;
            try
            {
                SlidingWindow sw = new SlidingWindow(null, mockProc, mockLogger, 100, 10);
                sw.MoveAndCompare();
            }
            catch
            {
                hasRtn = true;
            }
            Assert.IsTrue(hasRtn);

           
            mockDailyTick.Add(null);
            mockDailyTick.Add(null);

            hasRtn = false;
            try
            {
                SlidingWindow sw = new SlidingWindow(mockDailyTick, mockProc, mockLogger, 100, 10);
                sw.MoveAndCompare();
            }
            catch
            {
                hasRtn = true;
            }
            Assert.IsTrue(!hasRtn); // no exception coz not touch elements
            
            hasRtn = false;
            try
            {
                SlidingWindow sw = new SlidingWindow(mockDailyTick, null, mockLogger, 100, 10);
                sw.MoveAndCompare();
            }
            catch
            {
                hasRtn = true;
            }
            Assert.IsTrue(hasRtn);

            hasRtn = false;
            try
            {
                SlidingWindow sw = new SlidingWindow(mockDailyTick, mockProc, null, 100, 10);
                sw.MoveAndCompare();
            }
            catch
            {
                hasRtn = true;
            }
            Assert.IsTrue(hasRtn);
        }

        [TestMethod]
        // positive case
        public void TestVirtualInterface()
        {
            List<DailyTick> mockDailyTick = new List<DailyTick>();
            mockDailyTick.Add(new DailyTick("09/01/1990", "100.4577362"));

            IOutlierProcessor mockProc = new MockProcessor();
            Logger mockLogger = new MockLogger("");

            ITraversalMethod sw = new MockSlidingWindow(mockDailyTick, mockProc, mockLogger, 3, 1);
            sw.MoveAndCompare();
            Assert.IsTrue(sw.GetTestResult().Equals("MockSlidingWindow"));


            ITraversalMethod sw1 = new MockBinarySearch(mockDailyTick, mockProc, mockLogger, 3, 1);
            sw1.MoveAndCompare();
            Assert.IsTrue(sw1.GetTestResult().Equals("MockBinarySearch"));

        }

        [TestMethod]
        // positive case
        public void TestGivenDataSetHaveBeenCheckedFully()
        {
            List<DailyTick> mockDailyTick = new List<DailyTick>();
            // assign 10 items
            mockDailyTick.Add(new DailyTick("09/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("10/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("11/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("12/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("13/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("14/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("15/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("16/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("17/01/1990", "100.4577362"));
            mockDailyTick.Add(new DailyTick("18/01/1990", "100.4577362"));

            IOutlierProcessor mockProc = new MockProcessor();
            Logger mockLogger = new MockLogger("");

            SlidingWindow sw = new SlidingWindow(mockDailyTick, mockProc, mockLogger, 3, 1);
            sw.MoveAndCompare();

            var verifiedSet = mockProc.GetTestResult();
            Assert.IsTrue(verifiedSet[0].Equals("0,3"));
            Assert.IsTrue(verifiedSet[1].Equals("1,3"));
            Assert.IsTrue(verifiedSet[2].Equals("2,3"));
            Assert.IsTrue(verifiedSet[3].Equals("3,3"));
            Assert.IsTrue(verifiedSet[4].Equals("4,3"));
            Assert.IsTrue(verifiedSet[5].Equals("5,3"));
            Assert.IsTrue(verifiedSet[6].Equals("6,3"));
            Assert.IsTrue(verifiedSet[7].Equals("7,3"));
        }

    }
}
