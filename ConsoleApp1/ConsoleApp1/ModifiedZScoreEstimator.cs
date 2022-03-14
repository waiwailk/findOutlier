using System;
using System.Collections.Generic;
using FindOutlierApp;

namespace AlgoLibrary
{
    public class ModifiedZScoreEstimator : IOutlierProcessor
    {
        private List<DailyTick> srcDailyTick;
        private Dictionary<DateTime, DailyTick> result;

        // some algo config and assumption
        private const int monitoringRange = 3;  // +/- 3 ticks * * * T * * *
        private const int slope = 10;

        private const double defaultMADScale = 3.5;
        private const double defaultNorminatorScale = 0.6745;
        public ModifiedZScoreEstimator(in List<DailyTick> srcDailyTickList, 
                               Dictionary<DateTime, DailyTick> filterIn )
        {
            srcDailyTick = srcDailyTickList;
            result = filterIn;
        }
        private void Apply(Dictionary<DateTime, DailyTick> filterIn,
                                         int movingIdx,
                                         int windowSize)

        {
            var subTicks = srcDailyTick.GetRange(movingIdx, windowSize);
            List<DailyTick> outlierNumber = new List<DailyTick>();
            List<DailyTick> normalNumber = new List<DailyTick>();


        double lumpSum = 0.0;
            foreach (var value in subTicks)
                lumpSum += value.PriceClosing;

            double std_mean = lumpSum / subTicks.Count;

            double totalranges = 0.0, stdDev = 0.0;
            foreach (var value in subTicks)
                totalranges += Math.Pow(value.PriceClosing - std_mean, 2);

            stdDev = Math.Sqrt(totalranges / (subTicks.Count - 1));

            // z =  0.6745(x - u) / MAD , where MAD = scalefactor * sigma
            int begin = 0;
            int end = subTicks.Count;

            double mad_scale = defaultMADScale;
            while (mad_scale >= 1 && outlierNumber.Count <= 0)
            {
                for (int i = begin; i < end; i++)
                {
                    subTicks[i].AdjacentMean = std_mean;
                    // loop to check,
                    // dynamic to adjust the mad_scale if no explicit outlier found
                    if ((defaultNorminatorScale * Math.Abs(subTicks[i].PriceClosing - std_mean)) > (mad_scale * stdDev) &&  // Z-Score value
                         Math.Abs(subTicks[i].PriceClosing - std_mean) > slope )                           //Conditions 2: delta between price and mean
                        outlierNumber.Add(subTicks[i]);
                    else
                        normalNumber.Add(subTicks[i]);
                }
                mad_scale -= 0.1;
            }

            // per windows price, estimate the local outlier
            OnSelectedOutliers(outlierNumber, filterIn);

        }

        private void OnSelectedOutliers(in List<DailyTick> outlierNumber,
                                        Dictionary<DateTime, DailyTick> filterIn)
        {
            foreach (var value in outlierNumber)
            {
                // filter out selected day tick from previous screening
                // distinct the explicit outlier price with great delta in compare with neigbours
                if (!filterIn.ContainsKey(value.TradeDate) &&
                    IsHighDelta(value))
                {
                    filterIn.Add(value.TradeDate, value);
                }
                else 
                {
                    // ignore
                }

            }
        }
        private bool IsHighDelta(DailyTick target)
        {
            int targetIdx = target.DateIndex;

            if (srcDailyTick.Count <= monitoringRange)
            {
                // return status as false if length < check range (LHS/RHS = 3 steps) as unpossible to check
                return false;
            }
            else if (srcDailyTick.Count > monitoringRange &&
                     targetIdx - monitoringRange < 0 &&
                     targetIdx + monitoringRange < srcDailyTick.Count ) // target at most LHS, check right only
            {
                // handle the list size > check range
                // the check position LHS -3 steps < 0
                // the check position RHS +3 steps < list size
                // the list size > check position RHS 3 steps

                return Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx + 1].PriceClosing) > 1.0 &&
                       Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx + 2].PriceClosing) > 1.0 &&
                       Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx + 3].PriceClosing) > 1.0;
            }
            else if (srcDailyTick.Count > monitoringRange &&
                     targetIdx + monitoringRange >= srcDailyTick.Count &&
                     targetIdx - monitoringRange >= 0) // target at most RHS, check left only
            {
                // only check LHS
                // handle the list size > check range
                // the check position LHS -3 steps > 0
                // the check position RHS +3 steps < list size
                // the list size > check position RHS 3 steps

                return
                    Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx - 1].PriceClosing) > 1.0 &&
                    Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx - 2].PriceClosing) > 1.0 &&
                    Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx - 3].PriceClosing) > 1.0;
            }
            else
            {
                return (targetIdx >= monitoringRange &&
                           Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx - 1].PriceClosing) > 1.0 &&
                           Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx - 2].PriceClosing) > 1.0 &&
                           Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx - 3].PriceClosing) > 1.0) ||
                       (targetIdx < srcDailyTick.Count - monitoringRange &&
                           Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx + 1].PriceClosing) > 1.0 &&
                           Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx + 2].PriceClosing) > 1.0 &&
                           Math.Abs(srcDailyTick[targetIdx].PriceClosing - srcDailyTick[targetIdx + 3].PriceClosing) > 1.0);
                
            }
        }

        public void Request(int movingIdx,int lookupRng)
        {
            Apply(result, movingIdx, lookupRng);
        }

        // prepare for unit test
        
        public bool TestIsHighDelta( DailyTick mocktarget )
        {
            return IsHighDelta(mocktarget);
        }

        public void TestNeighbourCheck(List<DailyTick> outlierNumber, 
                                       Dictionary<DateTime, DailyTick> filterIn)
        {
            OnSelectedOutliers(outlierNumber, filterIn);
        }

        public List<string> GetTestResult()
        {
            return new List<string>();
        }
    }

    // alternative Estimator 
    public class DummpyEstimator : IOutlierProcessor
    {
        public void Request(int movingIdx, int lookupRng)
        {
            Console.WriteLine("Do requesting");
        }

        public List<string> GetTestResult()
        {
            return new List<string>();
        }
    }

}
