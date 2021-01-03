using System;

namespace PAD
{
    public class SpecialNavamsha
    {
        public int Id { get; set; }
        public int SpeciaNavamshaId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public SpecialNavamsha ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new SpecialNavamsha() { SpeciaNavamshaId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
