using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoLibrary;
using FindOutlierApp;
using Utils;

namespace Methodologies
{
    public class SlidingWindow : ITraversalMethod
    {
        private IOutlierProcessor processor;
        private List<DailyTick> ticklist;
        private Logger logger;
        private int windowSize = 100;
        private int slidingMove = 10;
        
        public SlidingWindow(List<DailyTick> ticks, 
                             IOutlierProcessor proc,
                             Logger log,
                             int cfgWindowSize,
                             int cfgSlidingStep)
        {
            processor = proc;
            ticklist = ticks;
            logger = log;
            windowSize = cfgWindowSize;
            slidingMove = cfgSlidingStep;

        }

        public void MoveAndCompare()
        {
            ArgumentsCheck();

            // screen the max/min price by sliding window
            // set offset > 1 to reduce numbers of iterration and avoid over checking.
            for (int i = 0; i <= ticklist.Count; i += slidingMove)
            {
                if (i < ticklist.Count - windowSize)
                    processor.Request(i, windowSize);
                else
                {
                    // last range i - n-1
                    processor.Request(i, ticklist.Count - i);
                    break;
                }
            }
        }

        private void ArgumentsCheck()
        {
            if (ticklist == null)
                throw new NullReferenceException("ticklist has not been initialized");
            if (processor == null)
                throw new NullReferenceException("Outlier Processor has not been initialized");
            if (logger == null)
                throw new NullReferenceException("Logger has not been initialized");
        }

        public string GetTestResult()
        {
            return "";
        }
    }

    public class MockSlidingWindow : ITraversalMethod
    {
        private string result;

        public MockSlidingWindow(List<DailyTick> ticks,
                            IOutlierProcessor proc,
                            Logger log,
                            int cfgWindowSize,
                            int cfgSlidingStep)
        {
        }

        public string GetTestResult()
        {
            return result;
        }

        public void MoveAndCompare()
        {
            result = "MockSlidingWindow";
        }
    }

    public class MockBinarySearch : ITraversalMethod
    {
        private string result;
        public MockBinarySearch(List<DailyTick> ticks,
                           IOutlierProcessor proc,
                           Logger log,
                           int cfgWindowSize,
                           int cfgSlidingStep)
        {
        }
        public string GetTestResult()
        {
            return result;
        }

        public void MoveAndCompare()
        {
            result = "MockBinarySearch";
        }
    }
}
