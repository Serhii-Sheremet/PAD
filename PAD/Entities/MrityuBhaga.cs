using System;

namespace PAD
{
    public class MrityuBhaga
    {
        public int Id { get; set; }
        public int PlanetId { get; set; }
        public int ZodiakId { get; set; }
        public int Degree { get; set; }

        public MrityuBhaga ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new MrityuBhaga() { PlanetId = Convert.ToInt32(row[0]), ZodiakId = Convert.ToInt32(row[1]), Degree = Convert.ToInt32(row[2]) };
        }
    }
}
