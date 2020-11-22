using System;

namespace PAD
{
    public class Calendar
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public EColor ColorCode { get; set; }

        public virtual string GetShortName(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetPlanetDayViewShortName(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetOverlapedShortName(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetFullName(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetNumber(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetTranzitNakshatra(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetTranzitPada(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetTranzitTaraBala(ELanguage langCode)
        {
            return string.Empty;
        }

    }
}
