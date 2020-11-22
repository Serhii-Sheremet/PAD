using System;

namespace PAD
{
    public class FontListDescription
    {
        public int Id { get; set; }
        public int FontListId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public FontListDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new FontListDescription() { FontListId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
