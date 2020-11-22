using System;

namespace PAD
{
    public class TaraBalaDescription
    {
        public int Id { get; set; }
        public int TaraBalaId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

        public TaraBalaDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new TaraBalaDescription() { TaraBalaId = Convert.ToInt32(row[0]), Name = row[1], ShortName = row[2], Description = row[3], LanguageCode = row[4] };
        }
    }
}
