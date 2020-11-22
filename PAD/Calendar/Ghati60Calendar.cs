using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class Ghati60Calendar: Calendar
    {
        public EGhati60 Ghati60Code { get; set; }
        public bool IsDayLightGhati { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            return (int)Ghati60Code + "." + CacheLoad._ghati60DescList.Where(i => i.Ghati60Id == (int)Ghati60Code && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty;
        }

        public List<Ghati60Calendar> Calculate30Plus30Ghati60(List<Ghati60> g60List, DateTime? sunsetPrevious, DateTime? sunrise, DateTime? sunset, DateTime? sunriseNext, DateTime date)
        {
            List<Ghati60Calendar> gc60List = new List<Ghati60Calendar>();

            // calculate the longivety of 30 pars for each of 3 periods
            var nightLongBefore = sunrise.Value.Subtract(sunsetPrevious.Value);
            var dayLong = sunset.Value.Subtract(sunrise.Value);
            var nightLongAfter = sunriseNext.Value.Subtract(sunset.Value);

            // calculate the timespan of 1 of 30 part of each 3 periods
            TimeSpan nightBefore30Part = new TimeSpan(nightLongBefore.Ticks / 30);
            TimeSpan day30Part = new TimeSpan(dayLong.Ticks / 30);
            TimeSpan nightAfter30Part = new TimeSpan(nightLongAfter.Ticks / 30);

            // preparing list with 90 parts of time frames for 2 astro days
            int index = 0;
            DateTime startDate = sunsetPrevious.Value;
            DateTime endDate = startDate.Add(nightBefore30Part);
            for (int i = 0; i < 30; i++)
            {
                Ghati60Calendar gt60Temp = new Ghati60Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == g60List[index + 30].ColorId).FirstOrDefault()?.Id ?? 0),
                    Ghati60Code = (EGhati60)g60List[index + 30].Id,
                    IsDayLightGhati = false
                };
                gc60List.Add(gt60Temp);
                startDate = startDate.Add(nightBefore30Part);
                endDate = endDate.Add(nightBefore30Part);
                index++;
            }

            index = 0;
            startDate = sunrise.Value;
            endDate = startDate.Add(day30Part);
            for (int i = 0; i < 30; i++)
            {
                Ghati60Calendar gt60Temp = new Ghati60Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == g60List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Ghati60Code = (EGhati60)g60List[index].Id,
                    IsDayLightGhati = true
                };
                gc60List.Add(gt60Temp);
                startDate = startDate.Add(day30Part);
                endDate = endDate.Add(day30Part);
                index++;
            }

            index = 0;
            startDate = sunset.Value;
            endDate = startDate.Add(nightAfter30Part);
            for (int i = 0; i < 30; i++)
            {
                Ghati60Calendar gt60Temp = new Ghati60Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == g60List[index + 30].ColorId).FirstOrDefault()?.Id ?? 0),
                    Ghati60Code = (EGhati60)g60List[index + 30].Id,
                    IsDayLightGhati = false
                };
                gc60List.Add(gt60Temp);
                startDate = startDate.Add(nightAfter30Part);
                endDate = endDate.Add(nightAfter30Part);
                index++;
            }

            List<Ghati60Calendar> selectedGhati60List = gc60List.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();
            return selectedGhati60List;
        }

        public List<Ghati60Calendar> CalculateEqualGhati60(List<Ghati60> g60List, DateTime? sunrisePrevious, DateTime? sunrise, DateTime? sunriseNext, DateTime date)
        {
            List<Ghati60Calendar> gc60List = new List<Ghati60Calendar>();

            // calculate the longivety of periods
            var prevlongivety = sunrise.Value.Subtract(sunrisePrevious.Value);
            var currlongivety = sunriseNext.Value.Subtract(sunrise.Value);

            // calculate the muhurta30 timespan of 1/60 part in period
            TimeSpan prevGhatiTimespan = new TimeSpan(prevlongivety.Ticks / 60);
            TimeSpan currGhatiTimespan = new TimeSpan(currlongivety.Ticks / 60);

            // preparing list with 60 parts of time frames 
            int index = 0;
            DateTime startDate = sunrisePrevious.Value;
            DateTime endDate = startDate.Add(prevGhatiTimespan);
            for (int i = 0; i < 60; i++)
            {
                Ghati60Calendar gt60Temp = new Ghati60Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == g60List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Ghati60Code = (EGhati60)g60List[index].Id
                };
                gc60List.Add(gt60Temp);
                startDate = startDate.Add(prevGhatiTimespan);
                endDate = endDate.Add(prevGhatiTimespan);
                index++;
            }

            index = 0;
            startDate = sunrise.Value;
            endDate = startDate.Add(currGhatiTimespan);
            for (int i = 0; i < 60; i++)
            {
                Ghati60Calendar gt60Temp = new Ghati60Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == g60List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Ghati60Code = (EGhati60)g60List[index].Id
                };
                gc60List.Add(gt60Temp);
                startDate = startDate.Add(currGhatiTimespan);
                endDate = endDate.Add(currGhatiTimespan);
                index++;
            }

            List<Ghati60Calendar> selectedGhati60List = gc60List.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();
            return selectedGhati60List;
        }

        public List<Ghati60Calendar> CalculateGhati60From6(List<Ghati60> g60List, DateTime date)
        {
            List<Ghati60Calendar> gc60List = new List<Ghati60Calendar>();

            // calculate the longivety of periods
            DateTime prevStartDate = new DateTime(date.AddDays(-1).Year, date.AddDays(-1).Month, date.AddDays(-1).Day, 6, 0, 0);
            DateTime periodStartDate = new DateTime(date.Year, date.Month, date.Day, 6, 0, 0);
            DateTime periodEndDate = new DateTime(date.AddDays(+1).Year, date.AddDays(+1).Month, date.AddDays(+1).Day, 6, 0, 0);

            var prevlongivety = periodStartDate.Subtract(prevStartDate);
            var currlongivety = periodEndDate.Subtract(periodStartDate);

            // calculate the muhurta30 timespan of 1/60 part in period
            TimeSpan prevGhatiTimespan = new TimeSpan(prevlongivety.Ticks / 60);
            TimeSpan currGhatiTimespan = new TimeSpan(currlongivety.Ticks / 60);

            // preparing list with 60 parts of time frames 
            int index = 0;
            DateTime startDate = prevStartDate;
            DateTime endDate = startDate.Add(prevGhatiTimespan);
            for (int i = 0; i < 60; i++)
            {
                Ghati60Calendar gt60Temp = new Ghati60Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == g60List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Ghati60Code = (EGhati60)g60List[index].Id
                };
                gc60List.Add(gt60Temp);
                startDate = startDate.Add(prevGhatiTimespan);
                endDate = endDate.Add(prevGhatiTimespan);
                index++;
            }

            index = 0;
            startDate = periodStartDate;
            endDate = startDate.Add(currGhatiTimespan);
            for (int i = 0; i < 60; i++)
            {
                Ghati60Calendar gt60Temp = new Ghati60Calendar
                {
                    DateStart = startDate,
                    DateEnd = endDate,
                    ColorCode = (EColor)(CacheLoad._colorList.Where(c => c.Id == g60List[index].ColorId).FirstOrDefault()?.Id ?? 0),
                    Ghati60Code = (EGhati60)g60List[index].Id
                };
                gc60List.Add(gt60Temp);
                startDate = startDate.Add(currGhatiTimespan);
                endDate = endDate.Add(currGhatiTimespan);
                index++;
            }

            List<Ghati60Calendar> selectedGhati60List = gc60List.Where(s => s.DateEnd > date && s.DateStart < date.AddDays(+1)).ToList();
            return selectedGhati60List;
        }
    }
}
