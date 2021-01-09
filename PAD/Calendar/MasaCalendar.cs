using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class MasaCalendar : Calendar
    {
        public int MasaId { get; set; }
        public DateTime FullMoonDate { get; set; }

        public override string GetMasaName(ELanguage langCode)
        {
            return CacheLoad._masaDescList.Where(i => i.MasaId == MasaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public override string GetMasaFullName(List<PlanetCalendar> pList, Profile person, ELanguage langCode)
        {
            string masaName = CacheLoad._masaDescList.Where(i => i.MasaId == MasaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
            int nakId = Utility.GetNakshatraFullMoonId(this, pList, person);
            return masaName + " (" + Utility.GetNakshatraUprvitel(nakId, langCode) + ")";
        }
    }
}
