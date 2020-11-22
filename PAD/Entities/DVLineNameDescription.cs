using System;

namespace PAD
{
    public class DVLineNameDescription
    {
        public int Id { get; set; }
        public int DVLineNameId { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public DVLineNameDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new DVLineNameDescription() { DVLineNameId = Convert.ToInt32(row[0]), ShortName = row[1], Name = row[2], LanguageCode = row[3] };
        }

    }
}
