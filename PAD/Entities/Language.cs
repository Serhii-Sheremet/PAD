using System;

namespace PAD
{
    public class Language
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string CultureCode { get; set; }

        public Language ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Language() { Id = Convert.ToInt32(row[0]), LanguageCode = row[1], CultureCode = row[2] };
        }

    }
}
