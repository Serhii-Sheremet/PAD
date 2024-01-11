using System;
using System.Collections.Generic;
using System.Globalization;

namespace PAD
{
    public class MrityuBhagaData
    {
        public int PlanetId { get; set; }
        public int ZodiakId { get; set; }
        public double Degree { get; set; }
        public EAppSetting MrityuBhagaSetting { get; set; }
        public double LongitudeFrom { get; set; }
        public double LongitudeTo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public List<MrityuBhagaData> ParseUpdateFile(List<string> nList)
        {
            List<MrityuBhagaData> parsedList = new List<MrityuBhagaData>();
            double longitudeFrom, longitudeTo;
            EAppSetting mbSettings;
            foreach (string str in nList)
            {
                string temp = Utility.Decrypt(str, true);
                var arg = temp.Split(new char[] { '|' });
                DateTime dateFrom = DateTime.ParseExact(arg[6], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo);
                DateTime dateTo = DateTime.ParseExact(arg[7], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo);
                Enum.TryParse(arg[3], out mbSettings);
                if (double.TryParse(arg[4], NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitudeFrom) &&
                    double.TryParse(arg[5], NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitudeTo))
                {
                    parsedList.Add(new MrityuBhagaData
                    {
                        PlanetId = Convert.ToInt32(arg[0]),
                        ZodiakId = Convert.ToInt32(arg[1]),
                        Degree = Convert.ToInt32(arg[2]),
                        MrityuBhagaSetting = mbSettings,
                        LongitudeFrom = longitudeFrom,
                        LongitudeTo = longitudeTo,
                        DateFrom = dateFrom,
                        DateTo = dateTo
                    });
                }
            }
            return parsedList;
        }

    }
}
