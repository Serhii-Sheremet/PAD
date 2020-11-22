using System;

namespace PAD
{
    public class JogaDescription
    {
        public int Id { get; set; }
        public int JogaId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

        public JogaDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new JogaDescription() { JogaId = Convert.ToInt32(row[0]), Name = row[1], ShortName = row[2], Description = row[3], LanguageCode = row[4] };
        }
    }
}
