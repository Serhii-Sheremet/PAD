using System;
using System.Collections.Generic;
using System.Globalization;

namespace PAD
{
    public class PlanetData
    {
        public DateTime Date { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double SpeedInLongitude { get; set; }
        public double SpedInLatitude { get; set; }
        public string Retro { get; set; } // "R" or "D"
        public int ZodiakId { get; set; } // 1 - 12
        public int NakshatraId { get; set; } // 1- 27
        public int PadaId { get; set; } // 1 - 108

        public List<PlanetData> ParseUpdateFile(List<string> nList)
        {
            List<PlanetData> parsedList = new List<PlanetData>();
            double longitude, latitude, speedinlongitude, speedinlatitude;
            foreach (string str in nList)
            {
                string temp = Utility.Decrypt(str, true);
                var arg = temp.Split(new char[] { '|' });
                DateTime date = DateTime.ParseExact(arg[0], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo);
                if (double.TryParse(arg[1], NumberStyles.Any, CultureInfo.InvariantCulture, out longitude) &&
                    double.TryParse(arg[2], NumberStyles.Any, CultureInfo.InvariantCulture, out latitude) &&
                    double.TryParse(arg[3], NumberStyles.Any, CultureInfo.InvariantCulture, out speedinlongitude) &&
                    double.TryParse(arg[4], NumberStyles.Any, CultureInfo.InvariantCulture, out speedinlatitude))
                {
                    parsedList.Add(new PlanetData{
                        Date = date,
                        Longitude = longitude,
                        Latitude = latitude,
                        SpeedInLongitude = speedinlongitude,
                        SpedInLatitude = speedinlatitude,
                        Retro = arg[5],
                        ZodiakId = Convert.ToInt32(arg[6]),
                        NakshatraId = Convert.ToInt32(arg[7]),
                        PadaId = Convert.ToInt32(arg[8])
                    });
                }
            }
            return parsedList;
        }
        
    }
}
