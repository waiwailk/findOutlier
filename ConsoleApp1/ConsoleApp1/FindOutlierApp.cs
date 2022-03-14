using System;
using System.Collections.Generic;
using AlgoLibrary;
using Methodologies;
using Utils;

namespace FindOutlierApp
{   
    internal class ProgramMain
    {
        private static Logger logger = new Logger(@"c:\worksplace\FindOutlierApp.log");
        private const int windowSize  = 100;
        private const int slidingMove = 10;

        static void Main(string[] args)
        {
            try
            {
                DataReader dr = new CSVReader(@"c:\worksplace\Outliers.csv");
                var tickList = dr.Read();
                var resultDict = new Dictionary<DateTime, DailyTick>();

                // Select particular estimator 
                IOutlierProcessor processor = new ModifiedZScoreEstimator(tickList, resultDict);

                // Select Lookup method
                ITraversalMethod traversalFunctor = new SlidingWindow(tickList, processor, logger, windowSize, slidingMove);

                // process the dataset
                traversalFunctor.MoveAndCompare();

                // print result
                Console.WriteLine("The selected outlier pts are as following:");
                foreach (var item in resultDict)
                    Console.WriteLine(item.Value.ToString());

                Logger writer = new Logger(@"c:\worksplace\Outliers_clean.csv");
                writer.Write("Date,Price");
                foreach (var item in tickList)
                {
                    if (!resultDict.ContainsKey(item.TradeDate))
                        writer.Write(  String.Format("{0},{1}",item.TradeDate.ToString("dd/M/yyyy"),item.PriceClosing));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
        }
    }
}
