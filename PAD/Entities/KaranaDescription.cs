using System;

namespace PAD
{
    public class KaranaDescription
    {
        public int Id { get; set; }
        public int KaranaId { get; set; }
        public string Name { get; set; }
        public string Upravitel { get; set; }
        public string GoodFor { get; set; }
        public string BadFor { get; set; }
        public string LanguageCode { get; set; }

        public KaranaDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new KaranaDescription() { KaranaId = Convert.ToInt32(row[0]), Name = row[1], Upravitel = row[2], GoodFor = row[3], BadFor = row[4], LanguageCode = row[5] };
        }
    }
}
