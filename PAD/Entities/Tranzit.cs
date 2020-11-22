using System;

namespace PAD
{
    public class Tranzit
    {
        public int Id { get; set; }
        public int PlanetId { get; set; }
        public int Dom { get; set; }
        public int ColorId { get; set; }
        public string Vedha { get; set; }

        public Tranzit ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Tranzit() {Id = Convert.ToInt32(row[0]), PlanetId = Convert.ToInt32(row[1]), Dom = Convert.ToInt32(row[2]), ColorId = Convert.ToInt32(row[3]), Vedha = row[4] };
        }
    }
}
