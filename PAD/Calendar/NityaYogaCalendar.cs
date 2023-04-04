using System.Linq;

namespace PAD
{
    public class NityaYogaCalendar: Calendar
    {
        public ENityaYoga NYCode { get; set; }
        public int NakshatraId { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            return (int)NYCode + "." + CacheLoad._nityaYogaDescList.Where(i => i.NityaYogaId == (int)NYCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public override string GetFullName(ELanguage langCode)
        {
            return string.Empty;
        }

        public override string GetNumber(ELanguage langCode)
        {
            return ((int)NYCode).ToString();
        }

    }
}
