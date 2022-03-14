using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FindOutlierApp;

namespace Utils
{
    public abstract class DataReader
    {
        protected List<DailyTick> dailyTicks;
        public abstract List<DailyTick> Read();

        public DataReader()
        {
            dailyTicks = null;
        }
        protected void MarkItemIndex()
        {
            // assign seq order for O(1) look up
            for (int idx = 0; idx < dailyTicks.Count; idx++)
                dailyTicks[idx].DateIndex = idx;
        }
    }

    public class CSVReader : DataReader
    {
        private string fileSrc;
        public CSVReader(in string fileSrc)
        {
            this.fileSrc = fileSrc;
        }

        public override List<DailyTick> Read()
        {
            try
            {
                dailyTicks = File.ReadAllLines(this.fileSrc)
                        .Skip(1)
                        .Select(v => DailyTick.ReadFrmCSV(v))
                        .ToList();

                MarkItemIndex();     
                return dailyTicks;

            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message );
            }
        }
    }

    // Alternative Reader
    public class DBReader : DataReader
    {
        public override List<DailyTick> Read()
        {
            throw new NotImplementedException();
        }
    }
}
