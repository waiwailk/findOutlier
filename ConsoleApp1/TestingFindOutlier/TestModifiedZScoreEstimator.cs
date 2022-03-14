using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using FindOutlierApp;
using AlgoLibrary;

namespace TestingFindOutlier
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestModifiedZScoreEstimator
    {
        [TestMethod] // abnormal case
        public void TestIsHighDeltaWhenTicklistHasOneOnly()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "100.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);
            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator.TestIsHighDelta(dummpyTick1));
        }

        [TestMethod]
        public void TestIsHighDeltaWhenTicklistOnlyHasTwoOnly()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "100.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator.TestIsHighDelta(dummpyTick1));

            ModifiedZScoreEstimator estimator1 = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator1.TestIsHighDelta(dummpyTick2));
        }

        [TestMethod]
        public void TestIsHighDeltaWhenTicklistHasThreeOnly()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "150.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator.TestIsHighDelta(dummpyTick1));

            ModifiedZScoreEstimator estimator1 = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator1.TestIsHighDelta(dummpyTick2));
            
            ModifiedZScoreEstimator estimator2 = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator2.TestIsHighDelta(dummpyTick3));
        }

        [TestMethod] // boundary case
        public void TestIsHighDeltaWhenTicklistHasFour()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "101.4577362");
            dummpyTick3.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);
            
            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator.TestIsHighDelta(dummpyTick1));

            mockDailyTick[0].PriceClosing = 150.4577362; // mark as uplift
            ModifiedZScoreEstimator estimator_a = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsTrue(estimator_a.TestIsHighDelta(dummpyTick1));
            mockDailyTick[0].PriceClosing = 101.4577362; // reset
            
            ModifiedZScoreEstimator estimator1 = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator1.TestIsHighDelta(dummpyTick2));

            mockDailyTick[1].PriceClosing = 150.4577362; // mark as uplift
            ModifiedZScoreEstimator estimator1_b = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator1_b.TestIsHighDelta(dummpyTick2));
            mockDailyTick[1].PriceClosing = 102.4577362;
            
            ModifiedZScoreEstimator estimator2 = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator2.TestIsHighDelta(dummpyTick3));

            mockDailyTick[0].PriceClosing = 150.4577362; // mark as uplift
            ModifiedZScoreEstimator estimator2_b = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator2_b.TestIsHighDelta(dummpyTick3));
            mockDailyTick[0].PriceClosing = 102.4577362; // reset

            ModifiedZScoreEstimator estimator3 = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator3.TestIsHighDelta(dummpyTick3));

            
            mockDailyTick[3].PriceClosing = 150.4577362; // mark as uplift
            ModifiedZScoreEstimator estimator3_b = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsTrue(estimator3_b.TestIsHighDelta(dummpyTick3));
            mockDailyTick[3].PriceClosing = 101.4577362; // reset
        }
        [TestMethod] // normal case
        public void TestIsHighDeltaWhenTicklistInNormal()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "170.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "104.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "101.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "101.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsTrue(estimator.TestIsHighDelta(dummpyTick4));
        }

        [TestMethod] // normal case
        public void TestIsHighDeltaWhenTicklistInNormal1()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "102.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "104.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "170.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "101.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsTrue(estimator.TestIsHighDelta(dummpyTick5));
        }


        [TestMethod] // normal case
        public void TestIsHighDeltaWhenTicklistInNormal2()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "102.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "104.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "170.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "101.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsTrue(estimator.TestIsHighDelta(dummpyTick6));
        }

        [TestMethod] // normal case
        public void TestIsHighDeltaWhenTicklistInNormal3()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "102.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "104.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "171.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsTrue(estimator.TestIsHighDelta(dummpyTick7));
        }

        [TestMethod] // normal case
        public void TestIsHighDeltaWhenTicklistInSwiftJump()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "170.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "102.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "104.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "101.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            Assert.IsFalse(estimator.TestIsHighDelta(dummpyTick4));
        }

        [TestMethod]
        public void TestOnSelectedOutliersIfNoExistingInOutliers()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "174.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "101.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "102.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            var outlierNumber = new List<DailyTick>();
            outlierNumber.Add(dummpyTick4);
            estimator.TestNeighbourCheck(outlierNumber, filterIn );

            DateTime myKey = new DateTime(1990, 01, 12);
            Assert.IsTrue(filterIn.Count == 1);
            Assert.IsTrue(filterIn.ContainsKey(myKey));
            filterIn[myKey].DateIndex = 3;
            filterIn[myKey].PriceClosing = 174.4577362;
        }

        [TestMethod]
        public void TestOnSelectedOutliersIfDuplicateSubmit()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();

            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "102.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "174.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "101.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "102.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            var outlierNumber = new List<DailyTick>();
            outlierNumber.Add(dummpyTick4);
            estimator.TestNeighbourCheck(outlierNumber, filterIn);

            DateTime myKey = new DateTime(1990, 01, 12);
            Assert.IsTrue(filterIn.Count == 1);
            Assert.IsTrue(filterIn.ContainsKey(myKey));
            filterIn[myKey].DateIndex = 3;
            filterIn[myKey].PriceClosing = 174.4577362;

            outlierNumber.Clear();
            outlierNumber.Add(dummpyTick4);
            Assert.IsTrue(filterIn.Count == 1);
            estimator.TestNeighbourCheck(outlierNumber, filterIn);
            Assert.IsTrue(filterIn.Count == 1);
        }

        [TestMethod]
        public void TestEstimator()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();


            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "172.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "104.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "101.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "102.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            estimator.Request(0, mockDailyTick.Count);

            DateTime myKey = new DateTime(1990, 01, 11);
            Assert.IsTrue(filterIn.Count == 1);
            Assert.IsTrue(filterIn.ContainsKey(myKey));
            filterIn[myKey].DateIndex = 2;
            filterIn[myKey].PriceClosing = 172.4577362;

        }

        [TestMethod]
        public void TestEstimatorAtlast()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();


            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "102.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "109.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "104.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "101.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "190.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            estimator.Request(0, mockDailyTick.Count);

            DateTime myKey = new DateTime(1990, 01, 15);
            Assert.IsTrue(filterIn.Count == 1);
            Assert.IsTrue(filterIn.ContainsKey(myKey));
            filterIn[myKey].DateIndex = 6;
            filterIn[myKey].PriceClosing = 190.4577362;

        }

        [TestMethod]
        public void TestEstimatorAtFirst()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();


            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "192.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "109.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "104.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "101.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "101.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            estimator.Request(0, mockDailyTick.Count);

            DateTime myKey = new DateTime(1990, 01, 09);
            Assert.IsTrue(filterIn.Count == 1);
            Assert.IsTrue(filterIn.ContainsKey(myKey));
            filterIn[myKey].DateIndex = 0;
            filterIn[myKey].PriceClosing = 192.4577362;
        }

        [TestMethod]
        public void TestEstimatorOccurredMore()
        {
            var filterIn = new Dictionary<DateTime, DailyTick>();
            var mockDailyTick = new List<DailyTick>();


            DailyTick dummpyTick1 = new DailyTick("09/01/1990", "192.4577362");
            dummpyTick1.DateIndex = 0;
            mockDailyTick.Add(dummpyTick1);

            DailyTick dummpyTick2 = new DailyTick("10/01/1990", "101.4577362");
            dummpyTick2.DateIndex = 1;
            mockDailyTick.Add(dummpyTick2);

            DailyTick dummpyTick3 = new DailyTick("11/01/1990", "109.4577362");
            dummpyTick3.DateIndex = 2;
            mockDailyTick.Add(dummpyTick3);

            DailyTick dummpyTick4 = new DailyTick("12/01/1990", "104.4577362");
            dummpyTick4.DateIndex = 3;
            mockDailyTick.Add(dummpyTick4);

            DailyTick dummpyTick5 = new DailyTick("13/01/1990", "101.4577362");
            dummpyTick5.DateIndex = 4;
            mockDailyTick.Add(dummpyTick5);

            DailyTick dummpyTick6 = new DailyTick("14/01/1990", "100.4577362");
            dummpyTick6.DateIndex = 5;
            mockDailyTick.Add(dummpyTick6);

            DailyTick dummpyTick7 = new DailyTick("15/01/1990", "180.4577362");
            dummpyTick7.DateIndex = 6;
            mockDailyTick.Add(dummpyTick7);

            ModifiedZScoreEstimator estimator = new ModifiedZScoreEstimator(mockDailyTick, filterIn);
            estimator.Request(0, mockDailyTick.Count);

            // Due to >1 days tick prices scale up the avg mean in the same window screening,
            // by check with modified Z-Score both are not treated as Outlier.
            // No outlier point can be distinct
            Assert.IsTrue(filterIn.Count == 0);
        }
    }
}
