using System;

namespace PAD
{
    public class Muhurta30Description
    {
        public int Id { get; set; }
        public int Muhurta30Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

        public Muhurta30Description ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Muhurta30Description() { Muhurta30Id = Convert.ToInt32(row[0]), ShortName = row[1], Name = row[2], Description = row[3], LanguageCode = row[4] };
        }
    }
}
