using System;
using System.Collections.Generic;
using System.Globalization;

namespace PAD
{
    public class TithiData
    {
        public DateTime Date { get; set; }
        public double MoonSunDifference { get; set; }
        public int TithiId { get; set; }

        public List<TithiData> ParseUpdateFile(List<string> nList)
        {
            List<TithiData> parsedList = new List<TithiData>();
            double msdifferenece;
            foreach (string str in nList)
            {
                string temp = Utility.Decrypt(str, true);
                var arg = temp.Split(new char[] { '|' });
                DateTime date = DateTime.ParseExact(arg[0], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo);
                if (double.TryParse(arg[1], NumberStyles.Any, CultureInfo.InvariantCulture, out msdifferenece))
                {
                    parsedList.Add(new TithiData
                    {
                        Date = date,
                        MoonSunDifference = msdifferenece,
                        TithiId = Convert.ToInt32(arg[2])
                    });
                }
            }
            return parsedList;
        }
    }
}
