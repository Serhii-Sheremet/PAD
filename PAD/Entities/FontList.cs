using System;

namespace PAD
{
    public class FontList
    {
        public int Id { get; set; }
        public int FontId { get; set; }
        public string Code { get; set; }
        public int FontStyleId { get; set; }

        public FontList ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new FontList() { FontId = Convert.ToInt32(row[0]), Code = row[1], FontStyleId = Convert.ToInt32(row[2]) };
        }
    }
}
