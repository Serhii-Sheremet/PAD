using System;
using System.Collections.Generic;
using System.Globalization;

namespace PAD
{
    public class NityaJogaData
    {
        public DateTime Date { get; set; }
        public double Longitude { get; set; }
        public int NakshatraId { get; set; }

        public List<NityaJogaData> ParseUpdateFile(List<string> nList)
        {
            List<NityaJogaData> parsedList = new List<NityaJogaData>();
            double longitude;
            foreach (string str in nList)
            {
                string temp = Utility.Decrypt(str, true);
                var arg = temp.Split(new char[] { '|' });
                DateTime date = DateTime.ParseExact(arg[0], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo);
                if (double.TryParse(arg[1], NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
                {
                    parsedList.Add(new NityaJogaData
                    {
                        Date = date,
                        Longitude = longitude,
                        NakshatraId = Convert.ToInt32(arg[2])
                    });
                }
            }
            return parsedList;
        }
    }
}
