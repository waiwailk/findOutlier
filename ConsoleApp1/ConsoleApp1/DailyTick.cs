using System;
using System.Globalization;

namespace FindOutlierApp
{
    public class DailyTick
    {
        public double AdjacentMean { get; set; }
        public int DateIndex { get; set; }
        public double Delta
        {
            get
            {
                return Math.Abs(PriceClosing - AdjacentMean);
            }
        }

        public double PriceClosing { get; set; }
        public DateTime TradeDate { get; set; }

        public DailyTick()
        {
            TradeDate = DateTime.Now;
            PriceClosing = 0.0;
        }

        public DailyTick(string tradeDate, string pricetick)
        {
            TradeDate = DateTime.ParseExact(tradeDate, "dd/M/yyyy",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None);

            PriceClosing = Convert.ToDouble(pricetick);
        }

        public override string ToString()
        {
            return string.Format("TradeDate: {0} Price: {1} \t (Neigbour Mean: {2}, Price Delta: {3})",
                                 TradeDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),
                                 Math.Round(PriceClosing,6),
                                 Math.Round(AdjacentMean,3),
                                 Math.Round(Delta,3));
        }
        public static DailyTick ReadFrmCSV(string tradeline)
        {
            string[] values = tradeline.Split(',');
            return new DailyTick(values[0], values[1]);
        }


    }
}
