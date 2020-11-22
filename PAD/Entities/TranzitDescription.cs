using System;

namespace PAD
{
    public class TranzitDescription
    {
        public int Id { get; set; }
        public int TranzitId { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

        public TranzitDescription ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new TranzitDescription() { TranzitId = Convert.ToInt32(row[0]), Description = row[1], LanguageCode = row[2] };
        }
    }
}
