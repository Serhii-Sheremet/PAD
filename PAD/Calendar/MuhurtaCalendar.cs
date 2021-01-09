using System;
using System.Linq;

namespace PAD
{
    public class MuhurtaCalendar: Calendar
    {
        public EMuhurta MuhurtaCode { get; set; }
        public EMuhurta OverlapedMuhurtaCode { get; set; }

        public MuhurtaCalendar() { }

        public MuhurtaCalendar(EMuhurta mCode, DateTime startDate, DateTime endDate)
        {
            DateStart = startDate;
            DateEnd = endDate;
            ColorCode = (EColor)(CacheLoad._muhurtaList.Where(i => i.Code.Equals(mCode.ToString())).FirstOrDefault()?.ColorId ?? 0);
            MuhurtaCode = mCode;
            OverlapedMuhurtaCode = EMuhurta.NOMUHURTA;
        }

        public MuhurtaCalendar(EMuhurta mCode, EMuhurta overMCode, DateTime startDate, DateTime endDate)
        {
            DateStart = startDate;
            DateEnd = endDate;
            ColorCode = EColor.PINK;
            MuhurtaCode = mCode;
            OverlapedMuhurtaCode = overMCode;
        }

        public override string GetShortName(ELanguage langCode)
        {
            return CacheLoad._muhurtaDescList.Where(i => i.MuhurtaId == (int)MuhurtaCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty;
        }

        public override string GetFullName(ELanguage langCode)
        {
            return CacheLoad._muhurtaDescList.Where(i => i.MuhurtaId == (int)MuhurtaCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public override string GetOverlapedShortName(ELanguage langCode)
        {
            return CacheLoad._muhurtaDescList.Where(i => i.MuhurtaId == (int)OverlapedMuhurtaCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public MuhurtaCalendar CalculateteAbhijitMuhurta(DateTime? sunrise, TimeSpan time, DayOfWeek dayOfWeek)
        {
            MuhurtaCalendar abhijit = null; 
            TimeSpan muhurtaLong = new TimeSpan(time.Ticks / 15);
            DateTime muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 7));
            DateTime muhurtaEnd = muhurtaStart.Add(muhurtaLong);
            if (dayOfWeek != DayOfWeek.Wednesday)
            {
                abhijit =  new MuhurtaCalendar(EMuhurta.ABHIJIT, muhurtaStart, muhurtaEnd);
            }
            return abhijit;
        }

        public MuhurtaCalendar CalculateteRahuKala(DateTime? sunrise, TimeSpan time, DayOfWeek dayOfWeek)
        {
            MuhurtaCalendar rahuKala = null;
            DateTime muhurtaStart, muhurtaEnd;
            TimeSpan muhurtaLong = new TimeSpan(time.Ticks / 8);
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    muhurtaStart = sunrise.Value.Add(muhurtaLong);
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    rahuKala = new MuhurtaCalendar(EMuhurta.RAHUKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Tuesday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 6));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    rahuKala = new MuhurtaCalendar(EMuhurta.RAHUKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Wednesday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 4));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    rahuKala = new MuhurtaCalendar(EMuhurta.RAHUKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Thursday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 5));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    rahuKala = new MuhurtaCalendar(EMuhurta.RAHUKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Friday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 3));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    rahuKala = new MuhurtaCalendar(EMuhurta.RAHUKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Saturday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 2));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    rahuKala = new MuhurtaCalendar(EMuhurta.RAHUKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Sunday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 7));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    rahuKala = new MuhurtaCalendar(EMuhurta.RAHUKALA, muhurtaStart, muhurtaEnd);
                    break;

                default:
                    break;
            }
            return rahuKala;
        }

        public MuhurtaCalendar CalculateBrahmaMuhurta(DateTime? sunrise, DateTime? sunrisePrevious)
        {
            var diff = sunrise.Value.Subtract(sunrisePrevious.Value);
            TimeSpan muhurtaLong = new TimeSpan(diff.Ticks / 30);
            DateTime muhurtaStart = sunrisePrevious.Value.Add(new TimeSpan(muhurtaLong.Ticks * 28));
            DateTime muhurtaEnd = muhurtaStart.Add(muhurtaLong);
            return new MuhurtaCalendar(EMuhurta.BRAHMA, muhurtaStart, muhurtaEnd);
        }

        public MuhurtaCalendar CalculateGulikaKala(DateTime? sunrise, TimeSpan time, DayOfWeek dayOfWeek)
        {
            MuhurtaCalendar gulikaKala = null;
            DateTime muhurtaStart, muhurtaEnd;
            TimeSpan muhurtaLong = new TimeSpan(time.Ticks / 8);
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 5));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    gulikaKala = new MuhurtaCalendar(EMuhurta.GULIKAKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Tuesday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 4));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    gulikaKala = new MuhurtaCalendar(EMuhurta.GULIKAKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Wednesday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 3));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    gulikaKala = new MuhurtaCalendar(EMuhurta.GULIKAKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Thursday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 2));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    gulikaKala = new MuhurtaCalendar(EMuhurta.GULIKAKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Friday:
                    muhurtaStart = sunrise.Value.Add(muhurtaLong);
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    gulikaKala = new MuhurtaCalendar(EMuhurta.GULIKAKALA, muhurtaStart, muhurtaEnd);
                    break;

                case DayOfWeek.Saturday:
                    gulikaKala = new MuhurtaCalendar(EMuhurta.GULIKAKALA, sunrise.Value, sunrise.Value.Add(muhurtaLong));

                    break;

                case DayOfWeek.Sunday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 6));
                    muhurtaEnd = muhurtaStart.Add(muhurtaLong);
                    gulikaKala = new MuhurtaCalendar(EMuhurta.GULIKAKALA, muhurtaStart, muhurtaEnd);
                    break;

                default:
                    break;
            }
            return gulikaKala;
        }

        public MuhurtaCalendar CalculateYamagandaMuhurta(DateTime? sunrise, TimeSpan time, DayOfWeek dayOfWeek)
        {
            MuhurtaCalendar jamaganda = null;
            DateTime muhurtaStart;
            TimeSpan muhurtaLong = new TimeSpan(time.Ticks / 8);
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 3));
                    jamaganda = new MuhurtaCalendar(EMuhurta.YAMAGANDA, muhurtaStart, muhurtaStart.Add(muhurtaLong));
                    break;

                case DayOfWeek.Tuesday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 2));
                    jamaganda = new MuhurtaCalendar(EMuhurta.YAMAGANDA, muhurtaStart, muhurtaStart.Add(muhurtaLong));
                    break;

                case DayOfWeek.Wednesday:
                    muhurtaStart = sunrise.Value.Add(muhurtaLong);
                    jamaganda = new MuhurtaCalendar(EMuhurta.YAMAGANDA, muhurtaStart, muhurtaStart.Add(muhurtaLong));
                    break;

                case DayOfWeek.Thursday:
                    jamaganda = new MuhurtaCalendar(EMuhurta.YAMAGANDA, sunrise.Value, sunrise.Value.Add(muhurtaLong));
                    break;

                case DayOfWeek.Friday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 6));
                    jamaganda = new MuhurtaCalendar(EMuhurta.YAMAGANDA, muhurtaStart, muhurtaStart.Add(muhurtaLong));
                    break;

                case DayOfWeek.Saturday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 5));
                    jamaganda = new MuhurtaCalendar(EMuhurta.YAMAGANDA, muhurtaStart, muhurtaStart.Add(muhurtaLong));
                    break;

                case DayOfWeek.Sunday:
                    muhurtaStart = sunrise.Value.Add(new TimeSpan(muhurtaLong.Ticks * 4));
                    jamaganda = new MuhurtaCalendar(EMuhurta.YAMAGANDA, muhurtaStart, muhurtaStart.Add(muhurtaLong));
                    break;

                default:
                    break;
            }
            return jamaganda;
        }
    }
}
