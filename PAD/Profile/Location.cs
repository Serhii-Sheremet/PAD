using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class Location
    {
        public int Id { get; set; }
        public string Locality { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string LanguageCode { get; set; }

        public List<Location> ParseFile(string[] lList)
        {
            List<Location> parsedList = new List<Location>();
            foreach (string str in lList)
            {
                var row = str.Split(new char[] { '|' });
                Location temp = new Location
                {
                    Locality = row[1],
                    Latitude = row[2],
                    Longitude = row[3],
                    Region = row[4],
                    State = row[5],
                    Country = row[6],
                    CountryCode = row[7],
                    LanguageCode = row[8]
                };
                parsedList.Add(temp);
            }
            return parsedList;
        }
    }
}
