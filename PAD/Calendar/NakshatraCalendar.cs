using System.Linq;

namespace PAD
{
    public class NakshatraCalendar: Calendar
    {
        public ENakshatra NakshatraCode { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            int nId = CacheLoad._nakshatraList.Where(i => i.Code.Equals(NakshatraCode.ToString())).FirstOrDefault()?.Id ?? 0;
            return (int)NakshatraCode + "." +  CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == nId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty;
        }

        public override string GetFullName(ELanguage langCode)
        {
            return string.Empty;
        }

        public override string GetNumber(ELanguage langCode)
        {
            return (CacheLoad._nakshatraList.Where(i => i.Code.Equals(NakshatraCode.ToString())).FirstOrDefault()?.Id ?? 0).ToString();
        }
    }
}
