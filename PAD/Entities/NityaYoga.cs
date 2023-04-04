using System;

namespace PAD
{
    public class NityaYoga
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ColorId { get; set; }
        public int NakshatraId { get; set; }
        public int YogiPlanetId { get; set; }
        public int AvaYogiPlanetId { get; set; }

        public NityaYoga ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new NityaYoga() { Id = Convert.ToInt32(row[0]), Code = row[1], ColorId = Convert.ToInt32(row[2]), NakshatraId = Convert.ToInt32(row[3]), YogiPlanetId = Convert.ToInt32(row[4]), AvaYogiPlanetId = Convert.ToInt32(row[5]) };
        }
    }
}
