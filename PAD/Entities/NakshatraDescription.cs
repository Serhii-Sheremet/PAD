using System;

namespace PAD
{
    public class NakshatraDescription
    {
        public int Id { get; set; }
        public int NakshatraId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Upravitel { get; set; }
        public string Nature { get; set; }
        public string Description { get; set; }
        public string GoodFor { get; set; }
        public string BadFor { get; set; }
        public string LanguageCode { get; set; }

        public NakshatraDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new NakshatraDescription() { NakshatraId = Convert.ToInt32(row[0]), Name = row[1], ShortName = row[2], Upravitel = row[3], Nature = row[4], Description = row[5], GoodFor = row[6], BadFor = row[7], LanguageCode = row[8] };
        }
    }
}
