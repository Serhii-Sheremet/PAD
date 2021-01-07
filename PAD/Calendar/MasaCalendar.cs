using System.Linq;

namespace PAD
{
    public class MasaCalendar : Calendar
    {
        public int MasaId { get; set; }

        public override string GetFullName(ELanguage langCode)
        {
            return CacheLoad._masaDescList.Where(i => i.MasaId == MasaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }
    }
}
