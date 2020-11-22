using System;

namespace PAD
{
    public class AppSettingList
    {
        public int Id { get; set; }
        public string GroupCode { get; set; }
        public string SettingCode { get; set; }
        public int Active { get; set; }

        public AppSettingList ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new AppSettingList() { GroupCode = row[0], SettingCode = row[1], Active = Convert.ToInt32(row[2]) };
        }

    }
}
