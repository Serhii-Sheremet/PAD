using System.Linq;

namespace PAD
{
    public class TaraBalaCalendar: NakshatraCalendar
    {
        public int TaraBalaId { get; set; }
        public int Percent { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            string tbText = CacheLoad._taraBalaDescList.Where(i => i.TaraBalaId == TaraBalaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName??string.Empty;
            return tbText + " " + Percent + "%";
        }

        public override string GetFullName(ELanguage langCode)
        {
            return string.Empty;
        }

        public override string GetNumber(ELanguage langCode)
        {
            return (CacheLoad._taraBalaDescList.Where(i => i.TaraBalaId == TaraBalaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.TaraBalaId ?? 0).ToString();
        }

    }
}
