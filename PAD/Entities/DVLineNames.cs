using System;

namespace PAD
{
    public class DVLineNames
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public DVLineNames ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new DVLineNames() { Id = Convert.ToInt32(row[0]), Code = row[1] };
        }
    }
}
