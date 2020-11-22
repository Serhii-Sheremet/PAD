using System.Linq;

namespace PAD
{
    public class TithiCalendar: Calendar
    {
        public int TithiId { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
           return TithiId + "." +  CacheLoad._tithiDescList.Where(i => i.TithiId == TithiId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty;
        }

        public override string GetFullName(ELanguage langCode)
        {
            return string.Empty;
        }

        public override string GetNumber(ELanguage langCode)
        {
            return TithiId.ToString();
        }

    }
}
