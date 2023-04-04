using System;

namespace PAD
{
    public class YogaDescription
    {
        public int Id { get; set; }
        public int YogaId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

        public YogaDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new YogaDescription() { YogaId = Convert.ToInt32(row[0]), Name = row[1], ShortName = row[2], Description = row[3], LanguageCode = row[4] };
        }
    }
}
