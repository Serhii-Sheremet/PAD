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

        public override string GetMasaFullName(List<PlanetCalendar> pList, Profile_old person, ELanguage langCode)
        {
            string masaName = CacheLoad._masaDescList.Where(i => i.MasaId == MasaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
            int nakId = Utility.GetNakshatraFullMoonId(this, pList);
            if (nakId != 0)
            {
                return masaName + " (" + Utility.GetNakshatraUprvitel(nakId, langCode) + ")";
            }
            else
            {
                return masaName;
            }
        }
    }
}
