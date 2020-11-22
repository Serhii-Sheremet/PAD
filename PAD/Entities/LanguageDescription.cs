using System;

namespace PAD
{
    public class LanguageDescription
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public LanguageDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new LanguageDescription() { LanguageId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
