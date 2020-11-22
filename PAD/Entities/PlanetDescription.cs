using System;

namespace PAD
{
    public class PlanetDescription
    {
        public int Id { get; set; }
        public int PlanetId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public PlanetDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new PlanetDescription() { PlanetId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
