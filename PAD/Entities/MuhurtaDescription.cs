using System;

namespace PAD
{
    public class MuhurtaDescription
    {
        public int Id { get; set; }
        public int MuhurtaId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LanguageCode { get; set; }

        public MuhurtaDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new MuhurtaDescription() { MuhurtaId = Convert.ToInt32(row[0]), Name = row[1], ShortName = row[2], LanguageCode = row[3] };
        }
    }
}
