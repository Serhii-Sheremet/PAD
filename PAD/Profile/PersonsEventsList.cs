using System;
using System.Globalization;

namespace PAD
{
    public class PersonsEventsList
    {
        public int Id;
        public DateTime DateStart;
        public DateTime DateEnd;
        public string Name;
        public string Message;
        public int ARGBValue;
        public string GUID;

        public PersonsEventsList ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new PersonsEventsList
            {
                DateStart = DateTime.ParseExact(row[0], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo),
                DateEnd = DateTime.ParseExact(row[1], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo),
                Name = row[2],
                Message = row[3],
                ARGBValue = Convert.ToInt32(row[4]),
                GUID = row[5]
            };
        }
    }
}
