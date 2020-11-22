using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class HoraCalendar: Calendar
    {
        public EHoraPlanet PlanetCode { get; set; }
        public bool IsDayLightHora { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            int pId = CacheLoad._planetList.Where(i => i.Code.Equals(PlanetCode.ToString())).FirstOrDefault()?.Id ?? 0;
            return CacheLoad._planetDescList.Where(i => i.PlanetId == pId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public List<HoraCalendar> Calculate12Plus12Hora(List<HoraPlanet> hpList, DateTime? sunrisePrevious, DateTime? sunsetPrevious, DateTime? sunrise, DateTime? sunset, DateTime? sunriseNext, DateTime date)
        {
            List<HoraCalendar> hcList = new List<HoraCalendar>();

            // calculate the longivety of 12 pars for each of 4 periods
            var dayLongBefore = sunsetPrevious.Value.Subtract(sunrisePrevious.Value);
            var nightLongBefore = sunrise.Value.Subtract(sunsetPrevious.Value);
            var dayLong = sunset.Value.Subtract(sunrise.Value);
            var nightLongAfter = sunriseNext.Value.Subtract(sunset.Value);

            // calculate the timespan of 1 of 12 part of each 4 periods
            TimeSpan dayBefore12Part = new TimeSpan(dayLongBefore.Ticks / 12);
            TimeSpan nightBefore12Part = new TimeSpan(nightLongBefore.Ticks / 12);
            TimeSpan day12Part = new TimeSpan(dayLong.Ticks / 12);
            TimeSpan nightAfter12Part = new TimeSpan(nightLongAfter.Ticks / 12);

            // preparing swapped planet list for current astro day and previous astro day containing 24 part
            List<HoraPlanet> before24PlanetList = GetHora24PlanetList(hpList, date.AddDays(-1));
            List<HoraPlanet> current24PlanetList = GetHora24PlanetList(hpList, date);

            // preparing list with 48 parts of time frames for 2 astro days
            int index = 0;
            DateTime startDate = sunrisePrevious.Value;
            DateTime endDate = startDate.Add(dayBefore12Part);
            for (int i = 0; i < 12; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = before24PlanetList[index].ColorCode,
                    PlanetCode = before24PlanetList[index].PlanetCode,
                    IsDayLightHora = true
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(dayBefore12Part);
                endDate = endDate.Add(dayBefore12Part);
                index++;
            }

            startDate = sunsetPrevious.Value;
            endDate = startDate.Add(nightBefore12Part);
            for (int i = 0; i < 12; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = before24PlanetList[index].ColorCode,
                    PlanetCode = before24PlanetList[index].PlanetCode,
                    IsDayLightHora = false,
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(nightBefore12Part);
                endDate = endDate.Add(nightBefore12Part);
                index++;
            }

            index = 0;
            startDate = sunrise.Value;
            endDate = startDate.Add(day12Part);
            for (int i = 0; i < 12; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = current24PlanetList[index].ColorCode,
                    PlanetCode = current24PlanetList[index].PlanetCode,
                    IsDayLightHora = true
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(day12Part);
                endDate = endDate.Add(day12Part);
                index++;
            }

            startDate = sunset.Value;
            endDate = startDate.Add(nightAfter12Part);
            for (int i = 0; i < 12; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = current24PlanetList[index].ColorCode,
                    PlanetCode = current24PlanetList[index].PlanetCode,
                    IsDayLightHora = false
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(nightAfter12Part);
                endDate = endDate.Add(nightAfter12Part);
                index++;
            }

            // selecting into htList only parts for the current day
            List<HoraCalendar> selectedHoraTimeList = hcList.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();
            return selectedHoraTimeList;
        }

        List<HoraPlanet> GetHora24PlanetList(List<HoraPlanet> hpList, DateTime date)
        {
            List<HoraPlanet> hp24List = new List<HoraPlanet>();

            EHoraPlanet dayPlanet = GetMainHoraPlanetByDayOfWeek(date.DayOfWeek);
            List<HoraPlanet> swappedHoraPlanetList = SwappingHoraPlanetList(hpList, dayPlanet);
            int index = 0;
            for (int i = 0; i < 24; i++)
            {
                if (index == 7)
                    index = 0;
                hp24List.Add(swappedHoraPlanetList[index]);
                index++;
            }
            return hp24List;
        }

        private EHoraPlanet GetMainHoraPlanetByDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return EHoraPlanet.MOON;

                case DayOfWeek.Tuesday:
                    return EHoraPlanet.MARS;

                case DayOfWeek.Wednesday:
                    return EHoraPlanet.MERCURY;

                case DayOfWeek.Thursday:
                    return EHoraPlanet.JUPITER;

                case DayOfWeek.Friday:
                    return EHoraPlanet.VENUS;

                case DayOfWeek.Saturday:
                    return EHoraPlanet.SATURN;

                case DayOfWeek.Sunday:
                    return EHoraPlanet.SUN;
            }
            return EHoraPlanet.NULL;
        }

        private List<HoraPlanet> SwappingHoraPlanetList(List<HoraPlanet> hpList, EHoraPlanet planeCode)
        {
            List<HoraPlanet> newList = new List<HoraPlanet>();
            int id = hpList.Where(i => i.PlanetCode == planeCode).FirstOrDefault()?.Id ?? 0;
            newList.AddRange(hpList.Where(i => i.Id >= id).ToList());
            newList.AddRange(hpList.Where(i => i.Id < id).ToList());
            return newList;
        }

        public List<HoraCalendar> CalculateEqualHora(List<HoraPlanet> hpList, DateTime? sunrisePrevious, DateTime? sunrise, DateTime? sunriseNext, DateTime date)
        {
            List<HoraCalendar> hcList = new List<HoraCalendar>();

            // calculate the longivety of periods
            var longivetyPrev = sunrise.Value.Subtract(sunrisePrevious.Value);
            var longivetyCurr = sunriseNext.Value.Subtract(sunrise.Value);

            // calculate the hora timespan of 1/24 part in period
            TimeSpan horaTimespanPrev = new TimeSpan(longivetyPrev.Ticks / 24);
            TimeSpan horaTimespanCurr = new TimeSpan(longivetyCurr.Ticks / 24);

            // preparing swapped planet list for previous astro day containing 24 part
            List<HoraPlanet> previous24PlanetList = GetHora24PlanetList(hpList, date.AddDays(-1));

            // preparing swapped planet list for current astro day containing 24 part
            List<HoraPlanet> current24PlanetList = GetHora24PlanetList(hpList, date);

            int index = 0;
            DateTime startDate = sunrisePrevious.Value;
            DateTime endDate = startDate.Add(horaTimespanPrev);
            for (int i = 0; i < 24; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = previous24PlanetList[index].ColorCode,
                    PlanetCode = previous24PlanetList[index].PlanetCode
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(horaTimespanPrev);
                endDate = endDate.Add(horaTimespanPrev);
                index++;
            }

            index = 0;
            startDate = sunrise.Value;
            endDate = startDate.Add(horaTimespanCurr);
            for (int i = 0; i < 24; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = current24PlanetList[index].ColorCode,
                    PlanetCode = current24PlanetList[index].PlanetCode
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(horaTimespanCurr);
                endDate = endDate.Add(horaTimespanCurr);
                index++;
            }

            // selecting into htList only parts for the current day
            List<HoraCalendar> selectedHoraTimeList = hcList.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();
            return selectedHoraTimeList;
        }

        public List<HoraCalendar> CalculateHoraFrom6(List<HoraPlanet> hpList, DateTime date)
        {
            List<HoraCalendar> hcList = new List<HoraCalendar>();

            // calculate the longivety of periods
            DateTime prevStartDate = new DateTime(date.AddDays(-1).Year, date.AddDays(-1).Month, date.AddDays(-1).Day, 6, 0, 0);
            DateTime periodStartDate = new DateTime(date.Year, date.Month, date.Day, 6, 0, 0);
            DateTime periodEndDate = new DateTime(date.AddDays(+1).Year, date.AddDays(+1).Month, date.AddDays(+1).Day, 6, 0, 0);

            var prevLongivety = periodStartDate.Subtract(prevStartDate);
            var curLongivety = periodEndDate.Subtract(periodStartDate);

            // calculate the hora timespan of 1/24 part in period
            TimeSpan prevHoraTimespan = new TimeSpan(prevLongivety.Ticks / 24);
            TimeSpan currHoraTimespan = new TimeSpan(curLongivety.Ticks / 24);

            // preparing swapped planet list for previous astro day containing 24 part
            List<HoraPlanet> previous24PlanetList = GetHora24PlanetList(hpList, date.AddDays(-1));

            // preparing swapped planet list for current astro day containing 24 part
            List<HoraPlanet> current24PlanetList = GetHora24PlanetList(hpList, date);

            int index = 0;
            DateTime startDate = prevStartDate;
            DateTime endDate = startDate.Add(prevHoraTimespan);
            for (int i = 0; i < 24; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = previous24PlanetList[index].ColorCode,
                    PlanetCode = previous24PlanetList[index].PlanetCode
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(prevHoraTimespan);
                endDate = endDate.Add(prevHoraTimespan);
                index++;
            }

            index = 0;
            startDate = periodStartDate;
            endDate = startDate.Add(currHoraTimespan);
            for (int i = 0; i < 24; i++)
            {
                HoraCalendar hcTemp = new HoraCalendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = current24PlanetList[index].ColorCode,
                    PlanetCode = current24PlanetList[index].PlanetCode
                };
                hcList.Add(hcTemp);
                startDate = startDate.Add(currHoraTimespan);
                endDate = endDate.Add(currHoraTimespan);
                index++;
            }

            // selecting into htList only parts for the current day
            List<HoraCalendar> selectedHoraTimeList = hcList.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();
            return selectedHoraTimeList;
        }

    }
}
