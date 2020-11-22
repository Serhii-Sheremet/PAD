using System;

namespace PAD
{
    public class Ghati60
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public int ColorId { get; set; }
        public string Ghati60Code { get; set; }

        public Ghati60 ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Ghati60() { Position = Convert.ToInt32(row[1]), ColorId = Convert.ToInt32(row[2]), Ghati60Code = row[3] };
        }

    }
}
