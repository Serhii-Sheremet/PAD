using System;
using System.Collections.Generic;

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

        public virtual string GetNumberForYear()
        {
            return string.Empty;
        }

        public virtual string GetTranzitNakshatra(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual int GetTranzitNakshatraForYear()
        {
            return 0;
        }

        public virtual string GetTranzitPada()
        {
            return string.Empty;
        }

        public virtual string GetTranzitTaraBala(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetMasaName(ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual string GetMasaFullName(List<PlanetCalendar> pList, Profile person, ELanguage langCode)
        {
            return string.Empty;
        }

        public virtual EShunya GetShunyaCode()
        {
            return EShunya.NOSHUNYA;
        }

    }
}
