using System;

namespace PAD
{
    public class NityaYogaDescription
    {
        public int Id { get; set; }
        public int NityaYogaId { get; set; }
        public string Name { get; set; }
        public string Deity { get; set; }
        public string Meaning { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

        public NityaYogaDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new NityaYogaDescription() { NityaYogaId = Convert.ToInt32(row[0]), Name = row[1], Deity = row[2], Meaning = row[3], Description = row[4], LanguageCode = row[5] };
        }
    }
}
