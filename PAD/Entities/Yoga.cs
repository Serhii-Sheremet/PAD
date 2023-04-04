using System;

namespace PAD
{
    public class Yoga
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ColorId { get; set; }

        public Yoga ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Yoga() { Id = Convert.ToInt32(row[0]), ColorId = Convert.ToInt32(row[1]), Code = row[2] };
        }
    }
}
