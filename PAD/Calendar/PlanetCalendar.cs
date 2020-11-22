using System.Linq;

namespace PAD
{
    public class PlanetCalendar: Calendar
    {
        public EPlanet PlanetCode { get; set; }
        public int Dom { get; set; }
        public string Retro { get; set; }
        public EZodiak ZodiakCode { get; set; }
        public ENakshatra NakshatraCode { get; set; }
        public int PadaId { get; set; }
        public int LagnaDom { get; set; }
        public int TaraBalaId { get; set; }
        public int TaraBalaPercent { get; set; }

        
        public override string GetShortName(ELanguage langCode)
        {
            string planetAddon = string.Empty;
            EExaltation exaltation = Utility.GetExaltationByPlanetAndZnak(PlanetCode, ZodiakCode);
            string planetName = Utility.GetPlanetNameByCode(PlanetCode, langCode);
            if (Retro.Equals("R") && PlanetCode != EPlanet.RAHUMEAN && PlanetCode != EPlanet.KETUMEAN && PlanetCode != EPlanet.RAHUTRUE && PlanetCode != EPlanet.KETUTRUE)
            {
                planetAddon = "." + Retro;
            }
            else if (Retro.Equals("D") && (PlanetCode == EPlanet.RAHUMEAN || PlanetCode == EPlanet.KETUMEAN || PlanetCode == EPlanet.RAHUTRUE || PlanetCode == EPlanet.KETUTRUE))
            {
                planetAddon = "." + Retro;
            }
            else
            {
                if (exaltation == EExaltation.EXALTATION)
                    planetAddon = "↑";
                else if (exaltation == EExaltation.DEBILITATION)
                    planetAddon = "↓";
            }
            return planetName.Substring(0, 2) + planetAddon + ", " + (CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)ZodiakCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty);
        }

        public override string GetPlanetDayViewShortName(ELanguage langCode)
        {
            string pada = "P";
            if (langCode == ELanguage.ru)
                pada = "П";
            int padaId = CacheLoad._padaList.Where(i => i.Id == PadaId).FirstOrDefault().PadaNumber;
            return (CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)NakshatraCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty) + " " + padaId + pada;
        }

        public override string GetFullName(ELanguage langCode)
        {
            return string.Empty;
        }

        public override string GetNumber(ELanguage langCode)
        {
            string planetAddon = string.Empty;
            EExaltation exaltation = Utility.GetExaltationByPlanetAndZnak(PlanetCode, ZodiakCode);
            if (Retro.Equals("R") && PlanetCode != EPlanet.RAHUMEAN && PlanetCode != EPlanet.KETUMEAN && PlanetCode != EPlanet.RAHUTRUE && PlanetCode != EPlanet.KETUTRUE)
            {
                planetAddon = "." + Retro;
            }
            else if (Retro.Equals("D") && (PlanetCode == EPlanet.RAHUMEAN || PlanetCode == EPlanet.KETUMEAN || PlanetCode == EPlanet.RAHUTRUE || PlanetCode == EPlanet.KETUTRUE))
            {
                planetAddon = "." + Retro;
            }
            else
            {
                if (exaltation == EExaltation.EXALTATION)
                    planetAddon = "↑";
                else if (exaltation == EExaltation.DEBILITATION)
                    planetAddon = "↓";
            }
            return (CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)ZodiakCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name??string.Empty) + planetAddon;
        }

        public override string GetTranzitNakshatra(ELanguage langCode)
        {
            int nId = CacheLoad._nakshatraList.Where(i => i.Code.Equals(NakshatraCode.ToString())).FirstOrDefault()?.Id ?? 0;
            return (int)NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == nId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty;
        }

        public override string GetTranzitPada(ELanguage langCode)
        {
            return (CacheLoad._padaList.Where(i => i.Id == PadaId).FirstOrDefault()?.PadaNumber ?? 0).ToString();
        }

        public override string GetTranzitTaraBala(ELanguage langCode)
        {
            return TaraBalaId + "." + (CacheLoad._taraBalaDescList.Where(i => i.TaraBalaId == TaraBalaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty) + " " + TaraBalaPercent + "%"; 
        }

    }
}
