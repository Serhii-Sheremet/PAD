using System;

namespace PAD
{
    public class AppSettingDescList
    {
        public int Id { get; set; }
        public int AppSettingId { get; set; }
        public string SettingName { get; set; }
        public string LanguageCode { get; set; }

        public AppSettingDescList ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new AppSettingDescList() { AppSettingId = Convert.ToInt32(row[0]), SettingName = row[1], LanguageCode = row[2] };
        }
    }
}
