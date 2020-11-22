using System;

namespace PAD
{
    public class ColorDescription
    {
        public int Id { get; set; }
        public int ColorId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public ColorDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new ColorDescription() { ColorId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
