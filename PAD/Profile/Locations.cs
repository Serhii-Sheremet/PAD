using System.Collections.Generic;

namespace PAD
{
    public class Locations
    {
        public int Id { get; set; }
        public string CityEN { get; set; }
        public string CityRU { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AreaEN { get; set; }
        public string AreaRU { get; set; }
        public string StateEN { get; set; }
        public string StateRU { get; set; }
        public string CountryEN { get; set; }
        public string CountryRU { get; set; }
        public string CountryCode { get; set; }

        public List<Locations> ParseFile(string[] lList)
        {
            List<Locations> parsedList = new List<Locations>();
            foreach (string str in lList)
            {
                var row = str.Split(new char[] { '|' });
                Locations temp = new Locations
                {
                    CityEN = row[0],
                    CityRU = row[1],
                    Latitude = row[2],
                    Longitude = row[3],
                    AreaEN = row[4],
                    AreaRU = row[5],
                    StateEN = row[6],
                    StateRU = row[7],
                    CountryEN = row[8],
                    CountryRU = row[9],
                    CountryCode = row[10]
                };
                parsedList.Add(temp);
            }
            return parsedList;
        }

    }
}
