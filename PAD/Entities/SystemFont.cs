using System;

namespace PAD
{
    public class SystemFont
    {
        public int Id { get; set; }
        public int AppMain { get; set; }
        public string SystemName { get; set; }

        public SystemFont ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new SystemFont() { AppMain = Convert.ToInt32(row[0]), SystemName = row[1] };
        }
    }
}
