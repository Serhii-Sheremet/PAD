using System;

namespace PAD
{
    public class ZodiakDescription
    {
        public int Id { get; set; }
        public int ZodiakId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public ZodiakDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new ZodiakDescription() { ZodiakId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
