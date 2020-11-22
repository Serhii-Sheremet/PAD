using System;

namespace PAD
{
    public class Muhurta30
    {
        public int Id { get; set; }
        public int ColorId { get; set; }
        public string Muhurta30Code { get; set; }

        public Muhurta30 ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Muhurta30() { ColorId = Convert.ToInt32(row[1]), Muhurta30Code = row[2] };
        }
    }
}
