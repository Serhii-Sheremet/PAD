using System;

namespace PAD
{
    public class Masa
    {
        public int Id { get; set; }
        public int ZodiakId { get; set; }
        public int ColorId { get; set; }
        public string ShunyaNakshatra { get; set; }
        public string ShunyaTithi { get; set; }
        public int[] ShunyaNakshatraIdArray { get; set; }
        public int[] ShunyaTithiIdArray { get; set; }

        public Masa ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Masa() { Id = Convert.ToInt32(row[0]), ZodiakId = Convert.ToInt32(row[1]), ColorId = Convert.ToInt32(row[2]), ShunyaNakshatra = row[3], ShunyaTithi = row[4] };
        }
    }
}
