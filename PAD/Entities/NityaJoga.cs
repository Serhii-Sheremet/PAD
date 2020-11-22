using System;

namespace PAD
{
    public class NityaJoga
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ColorId { get; set; }
        public int NakshatraId { get; set; }
        public int JogiPlanetId { get; set; }
        public int AvaJogiPlanetId { get; set; }

        public NityaJoga ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new NityaJoga() { Id = Convert.ToInt32(row[0]), Code = row[1], ColorId = Convert.ToInt32(row[2]), NakshatraId = Convert.ToInt32(row[3]), JogiPlanetId = Convert.ToInt32(row[4]), AvaJogiPlanetId = Convert.ToInt32(row[5]) };
        }
    }
}
