using System;

namespace PAD
{
    public class TithiDescription
    {
        public int Id { get; set; }
        public int TithiId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Upravitel { get; set; }
        public string Type { get; set; }
        public string GoodFor { get; set; }
        public string BadFor { get; set; }
        public string LanguageCode { get; set; }

        public TithiDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new TithiDescription() { TithiId = Convert.ToInt32(row[0]), Name = row[1], ShortName = row[2], Upravitel = row[3], Type = row[4], GoodFor = row[5], BadFor = row[6], LanguageCode = row[7] };
        }
    }
}
