using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class Muhurta30Calendar: Calendar
    {
        public EMuhurta30 Muhurta30Code { get; set; }
        public bool IsDayLightMuhurta { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            return (int)Muhurta30Code + "." +  CacheLoad._muhurta30DescList.Where(i => i.Muhurta30Id == (int)Muhurta30Code && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty;
        }

        public List<Muhurta30Calendar> Calculate15Plus15Muhurta30(List<Muhurta30> mu30List, DateTime? sunsetPrevious, DateTime? sunrise, DateTime? sunset, DateTime? sunriseNext, DateTime date)
        {
            List<Muhurta30Calendar> mc30List = new List<Muhurta30Calendar>();

            // calculate the longivety of 15 pars for each of 3 periods
            var nightLongBefore = sunrise.Value.Subtract(sunsetPrevious.Value);
            var dayLong = sunset.Value.Subtract(sunrise.Value);
            var nightLongAfter = sunriseNext.Value.Subtract(sunset.Value);

            // calculate the timespan of 1 of 15 part of each 3 periods
            TimeSpan nightBefore15Part = new TimeSpan(nightLongBefore.Ticks / 15);
            TimeSpan day15Part = new TimeSpan(dayLong.Ticks / 15);
            TimeSpan nightAfter15Part = new TimeSpan(nightLongAfter.Ticks / 15);

            // preparing list with 45 parts of time frames for 2 astro days
            int index = 0;
            DateTime startDate = sunsetPrevious.Value;
            DateTime endDate = startDate.Add(nightBefore15Part);
            for (int i = 0; i < 15; i++)
            {
                Muhurta30Calendar m30tTemp = new Muhurta30Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == mu30List[index + 15].ColorId).FirstOrDefault()?.Id ?? 0),
                    Muhurta30Code = (EMuhurta30)mu30List[index + 15].Id,
                    IsDayLightMuhurta = false
                };
                mc30List.Add(m30tTemp);
                startDate = startDate.Add(nightBefore15Part);
                endDate = endDate.Add(nightBefore15Part);
                index++;
            }

            index = 0;
            startDate = sunrise.Value;
            endDate = startDate.Add(day15Part);
            for (int i = 0; i < 15; i++)
            {
                Muhurta30Calendar m30tTemp = new Muhurta30Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == mu30List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Muhurta30Code = (EMuhurta30)mu30List[index].Id,
                    IsDayLightMuhurta = true
                };
                mc30List.Add(m30tTemp);
                startDate = startDate.Add(day15Part);
                endDate = endDate.Add(day15Part);
                index++;
            }

            index = 0;
            startDate = sunset.Value;
            endDate = startDate.Add(nightAfter15Part);
            for (int i = 0; i < 15; i++)
            {
                Muhurta30Calendar m30tTemp = new Muhurta30Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == mu30List[index + 15].ColorId).FirstOrDefault()?.Id ?? 0),
                    Muhurta30Code = (EMuhurta30)mu30List[index + 15].Id,
                    IsDayLightMuhurta = false
                };
                mc30List.Add(m30tTemp);
                startDate = startDate.Add(nightAfter15Part);
                endDate = endDate.Add(nightAfter15Part);
                index++;
            }

            // selecting mc30List only parts for the current day
            List<Muhurta30Calendar> selectedMuhurta30List = mc30List.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();

            return selectedMuhurta30List;
        }

        public List<Muhurta30Calendar> CalculateEqualMuhurta30(List<Muhurta30> mu30List, DateTime? sunrisePrev, DateTime? sunrise, DateTime? sunriseNext, DateTime date)
        {
            List<Muhurta30Calendar> mc30List = new List<Muhurta30Calendar>();

            // calculate the longivety of periods
            var longivetyPrev = sunrise.Value.Subtract(sunrisePrev.Value);
            var longivetyCurr = sunriseNext.Value.Subtract(sunrise.Value);

            // calculate the muhurta30 timespan of 1/30 part in period
            TimeSpan muhurtaPrevTimespan = new TimeSpan(longivetyPrev.Ticks / 30);
            TimeSpan muhurtaCurrTimespan = new TimeSpan(longivetyCurr.Ticks / 30);

            // preparing list with 30 parts of time frames
            int index = 0;
            DateTime startDate = sunrisePrev.Value;
            DateTime endDate = startDate.Add(muhurtaPrevTimespan);
            for (int i = 0; i < 30; i++)
            {
                Muhurta30Calendar m30tTemp = new Muhurta30Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == mu30List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Muhurta30Code = (EMuhurta30)mu30List[index].Id
                };
                mc30List.Add(m30tTemp);
                startDate = startDate.Add(muhurtaPrevTimespan);
                endDate = endDate.Add(muhurtaPrevTimespan);
                index++;
            }

            index = 0;
            startDate = sunrise.Value;
            endDate = startDate.Add(muhurtaCurrTimespan);
            for (int i = 0; i < 30; i++)
            {
                Muhurta30Calendar m30tTemp = new Muhurta30Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == mu30List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Muhurta30Code = (EMuhurta30)mu30List[index].Id
                };
                mc30List.Add(m30tTemp);
                startDate = startDate.Add(muhurtaCurrTimespan);
                endDate = endDate.Add(muhurtaCurrTimespan);
                index++;
            }

            // selecting mc30List only parts for the current day
            List<Muhurta30Calendar> selectedMuhurta30List = mc30List.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();

            return selectedMuhurta30List;
        }

        public List<Muhurta30Calendar> CalculateMuhurta30From6(List<Muhurta30> mu30List, DateTime date)
        {
            List<Muhurta30Calendar> mc30List = new List<Muhurta30Calendar>();

            // calculate the longivety of periods
            DateTime prevStartDate = new DateTime(date.AddDays(-1).Year, date.AddDays(-1).Month, date.AddDays(-1).Day, 6, 0, 0);
            DateTime periodStartDate = new DateTime(date.Year, date.Month, date.Day, 6, 0, 0);
            DateTime periodEndDate = new DateTime(date.AddDays(+1).Year, date.AddDays(+1).Month, date.AddDays(+1).Day, 6, 0, 0);

            var prevlongivety = periodStartDate.Subtract(prevStartDate);
            var curlongivety = periodEndDate.Subtract(periodStartDate);

            // calculate the muhurta30 timespan of 1/30 part in period
            TimeSpan prevMuhurtaTimespan = new TimeSpan(prevlongivety.Ticks / 30);
            TimeSpan currMuhurtaTimespan = new TimeSpan(curlongivety.Ticks / 30);

            // preparing list with 30 parts of time frames
            int index = 0;
            DateTime startDate = prevStartDate;
            DateTime endDate = startDate.Add(prevMuhurtaTimespan);
            for (int i = 0; i < 30; i++)
            {
                Muhurta30Calendar m30tTemp = new Muhurta30Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == mu30List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Muhurta30Code = (EMuhurta30)mu30List[index].Id
                };
                mc30List.Add(m30tTemp);
                startDate = startDate.Add(prevMuhurtaTimespan);
                endDate = endDate.Add(prevMuhurtaTimespan);
                index++;
            }

            index = 0;
            startDate = periodStartDate;
            endDate = startDate.Add(currMuhurtaTimespan);
            for (int i = 0; i < 30; i++)
            {
                Muhurta30Calendar m30tTemp = new Muhurta30Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == mu30List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Muhurta30Code = (EMuhurta30)mu30List[index].Id
                };
                mc30List.Add(m30tTemp);
                startDate = startDate.Add(currMuhurtaTimespan);
                endDate = endDate.Add(currMuhurtaTimespan);
                index++;
            }

            // selecting mc30List only parts for the current day
            List<Muhurta30Calendar> selectedMuhurta30List = mc30List.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();

            return selectedMuhurta30List;
        }
    }
}
