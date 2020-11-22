using System;

namespace PAD
{
    public class EclipseDescription
    {
        public int Id { get; set; }
        public int EclipseId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public EclipseDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new EclipseDescription() { EclipseId = Convert.ToInt32(row[0]), Name = row[1], LanguageCode = row[2] };
        }
    }
}
