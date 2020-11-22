using System.Linq;

namespace PAD
{
    public class NityaJogaCalendar: Calendar
    {
        public ENityaJoga NJCode { get; set; }
        public int NakshatraId { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            return (int)NJCode + "." + CacheLoad._nityaJogaDescList.Where(i => i.NityaJogaId == (int)NJCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public override string GetFullName(ELanguage langCode)
        {
            return string.Empty;
        }

        public override string GetNumber(ELanguage langCode)
        {
            return ((int)NJCode).ToString();
        }

    }
}
