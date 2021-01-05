using System;

namespace PAD
{
    public class MasaDescription
    {
        public int Id { get; set; }
        public int MasaId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public MasaDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new MasaDescription() { MasaId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
