using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class KaranaCalendar: TithiCalendar
    {
        public int KaranaId { get; set; }

        public List<KaranaCalendar> MakeKaranaCalendarFromTithi(List<TithiCalendar> tList)
        {
            List<KaranaCalendar> kList = new List<KaranaCalendar>();
            foreach (TithiCalendar tc in tList)
            {
                TimeSpan periodHalf = new TimeSpan((tc.DateEnd - tc.DateStart).Ticks / 2);
                DateTime middleDate = tc.DateStart.AddTicks(periodHalf.Ticks);
                kList.Add(new KaranaCalendar { DateStart = tc.DateStart, DateEnd = middleDate, TithiId = tc.TithiId, KaranaId = GetKaranaIdByTithiAndSequence(tc.TithiId, 1) });
                kList.Add(new KaranaCalendar { DateStart = middleDate, DateEnd = tc.DateEnd, TithiId = tc.TithiId, KaranaId = GetKaranaIdByTithiAndSequence(tc.TithiId, 2) });
            }
            return kList;
        }

        public int GetKaranaIdByTithiAndSequence(int tithi, int sequence)
        {
            return CacheLoad._karanaList.Where(i => i.TithiId == tithi && i.Position == sequence).FirstOrDefault().Id;
        }

        public override string GetShortName(ELanguage langCode)
        {
            return CacheLoad._karanaDescList.Where(i => i.KaranaId == KaranaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

    }
}
