using System;
using System.Globalization;

namespace PAD
{
    public class PersonEvent
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int ARGBValue { get; set; }

        public PersonEvent ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new PersonEvent
            {
                ProfileId = int.Parse(row[0]),
                DateStart = DateTime.ParseExact(row[1], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo),
                DateEnd = DateTime.ParseExact(row[2], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo),
                Name = row[3],
                Message = row[4],
                ARGBValue = Convert.ToInt32(row[5])
            };
        }
    }
}
