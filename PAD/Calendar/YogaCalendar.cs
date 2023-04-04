using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class YogaCalendar: Calendar
    {
        public int Type { get; set; }
        public EYoga YogaCode { get; set; }
        public DayOfWeek Vara { get; set; }
        public ENakshatra NakshatraCode { get; set; }
        public int TithiId { get; set; }

        public YogaCalendar() { }

        public YogaCalendar(EYoga jCode, DateTime startDate, DateTime endDate, DayOfWeek vara, ENakshatra nakshatraCode, int tithiId)
        {
            int type = 0;
            if (jCode == EYoga.DWIPUSHKAR  || jCode == EYoga.TRIPUSHKAR || jCode == EYoga.AMRITASIDDHA || jCode == EYoga.SARVARTHA || jCode == EYoga.SIDDHA)
            {
                type = 1;
            }

            DateStart = startDate;
            DateEnd = endDate;
            ColorCode = (EColor)(CacheLoad._yogaList.Where(i => i.Code.Equals(jCode.ToString())).FirstOrDefault()?.ColorId ?? 0);
            Type = type;
            YogaCode = jCode;
            Vara = vara;
            NakshatraCode = nakshatraCode;
            TithiId = tithiId;
        }

        public override string GetShortName(ELanguage langCode)
        {
            return CacheLoad._yogaDescList.Where(i => i.YogaId == (int)YogaCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ShortName ?? string.Empty;
        }

        private DateTime CalculateJogaStartDate(DateTime startPeriod, DateTime endPeriod, DateTime nakDateStar, DateTime tiDateStart)
        {
            DateTime startDate = startPeriod;
            int compareNT = DateTime.Compare(nakDateStar, tiDateStart);
            if (compareNT < 0)
            {
                int comparePeriod = DateTime.Compare(tiDateStart, startPeriod);
                if (comparePeriod < 0)
                    startDate = startPeriod;
                else if (comparePeriod == 0)
                    startDate = startPeriod;
                else
                    startDate = tiDateStart;
            }
            else if (compareNT == 0)
            {
                int comparePeriod = DateTime.Compare(tiDateStart, startPeriod);
                if (comparePeriod < 0)
                    startDate = startPeriod;
                else if (comparePeriod == 0)
                    startDate = startPeriod;
                else
                    startDate = tiDateStart;
            }
            else
            {
                int comparePeriod = DateTime.Compare(nakDateStar, startPeriod);
                if (comparePeriod < 0)
                    startDate = startPeriod;
                else if (comparePeriod == 0)
                    startDate = startPeriod;
                else
                    startDate = nakDateStar;
            }
            return startDate;
        }

        private DateTime CalculateJogaEndDate(DateTime startPeriod, DateTime endPeriod, DateTime nakDateEnd, DateTime tiDateEnd)
        {
            DateTime endDate = startPeriod;
            int compareNT = DateTime.Compare(nakDateEnd, tiDateEnd);
            if (compareNT < 0)
            {
                int comparePeriod = DateTime.Compare(nakDateEnd, endPeriod);
                if (comparePeriod < 0)
                    endDate = nakDateEnd;
                else if (comparePeriod == 0)
                    endDate = nakDateEnd;
                else
                    endDate = endPeriod;
            }
            else if (compareNT == 0)
            {
                int comparePeriod = DateTime.Compare(nakDateEnd, endPeriod);
                if (comparePeriod < 0)
                    endDate = nakDateEnd;
                else if (comparePeriod == 0)
                    endDate = nakDateEnd;
                else
                    endDate = endPeriod;
            }
            else
            {
                int comparePeriod = DateTime.Compare(tiDateEnd, endPeriod);
                if (comparePeriod < 0)
                    endDate = tiDateEnd;
                else if (comparePeriod == 0)
                    endDate = tiDateEnd;
                else
                    endDate = endPeriod;
            }
            return endDate;
        }

        private DateTime CalculateNakshatraJogaStartDate(DateTime startPeriod, DateTime endPeriod, DateTime nakDateStart)
        {
            DateTime startDate = startPeriod;
            int comparePeriod = DateTime.Compare(nakDateStart, startPeriod);
            if (comparePeriod < 0)
            {
                startDate = startPeriod;
            }
            else if (comparePeriod == 0)
                startDate = startPeriod;
            else
                startDate = nakDateStart;
            return startDate;
        }

        private DateTime CalculateNakshatraJogaEndDate(DateTime startPeriod, DateTime endPeriod, DateTime nakDateEnd)
        {
            DateTime endDate = startPeriod;
            int comparePeriod = DateTime.Compare(nakDateEnd, endPeriod);
            if (comparePeriod < 0)
                endDate = nakDateEnd;
            else if (comparePeriod == 0)
                endDate = nakDateEnd;
            else
                endDate = endPeriod;
            return endDate;
        }

        private DateTime CalculateTithiJogaStartDate(DateTime startPeriod, DateTime endPeriod, DateTime tiDateStart)
        {
            DateTime startDate = startPeriod;
            int comparePeriod = DateTime.Compare(tiDateStart, startPeriod);
            if (comparePeriod < 0)
                startDate = startPeriod;
            else if (comparePeriod == 0)
                startDate = startPeriod;
            else
                startDate = tiDateStart;
            return startDate;
        }

        private DateTime CalculateTithiJogaEndDate(DateTime startPeriod, DateTime endPeriod, DateTime tiDateEnd)
        {
            DateTime endDate = startPeriod;
            int comparePeriod = DateTime.Compare(tiDateEnd, endPeriod);
            if (comparePeriod < 0)
                endDate = tiDateEnd;
            else if (comparePeriod == 0)
                endDate = tiDateEnd;
            else
                endDate = endPeriod;
            return endDate;
        }

        public List<YogaCalendar> CheckDvipushkarJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> dvipushkarJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<NakshatraCalendar> nakshatraList = new List<NakshatraCalendar>();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                if (jogaPeriod[0].Date.DayOfWeek == DayOfWeek.Tuesday || jogaPeriod[0].Date.DayOfWeek == DayOfWeek.Saturday || jogaPeriod[0].Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                    {
                        TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                        if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 17 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                        {
                            if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                tithiList.Add(tempTithi);
                        }
                    }
                    for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                    {
                        NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                        if ((tempNakshatra.NakshatraCode == ENakshatra.MRIGASHIRA || tempNakshatra.NakshatraCode == ENakshatra.CHITRA || tempNakshatra.NakshatraCode == ENakshatra.DHANISHTA) && tempNakshatra.DateStart < endPeriod)
                        {
                            if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                nakshatraList.Add(tempNakshatra);
                        }
                    }
                }
            }

            if (nakshatraList.Count > 0 && tithiList.Count > 0)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(i => i.DateEnd > startPeriod).ToList();
                List<TithiCalendar> dateT = tithiList.Where(i => i.DateEnd > startPeriod).ToList();
                if ((dateN != null && dateN.Count > 0) && (dateT != null && dateT.Count > 0))
                {
                    foreach (NakshatraCalendar nc in dateN)
                    {
                        foreach (TithiCalendar tc in dateT)
                        {
                            // add check: bool overlap = a.start < b.end && b.start < a.end; only after this chek if true create joga
                            bool overlap = nc.DateStart < tc.DateEnd && tc.DateStart < nc.DateEnd;
                            if (overlap)
                            {
                                dvipushkarJoga.Add(new YogaCalendar(
                                                            EYoga.DWIPUSHKAR,
                                                            CalculateJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart, tc.DateStart),
                                                            CalculateJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd, tc.DateEnd),
                                                            jogaPeriod[0].Date.DayOfWeek,
                                                            nc.NakshatraCode,
                                                            tc.TithiId));
                            }
                        }
                    }
                }
            }

            return dvipushkarJoga;
        }

        public List<YogaCalendar> CheckTripushkarJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> tripushkarJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<NakshatraCalendar> nakshatraList = new List<NakshatraCalendar>();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                if (jogaPeriod[0].Date.DayOfWeek == DayOfWeek.Tuesday || jogaPeriod[0].Date.DayOfWeek == DayOfWeek.Saturday || jogaPeriod[0].Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                    {
                        TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                        if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 17 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                        {
                            if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                tithiList.Add(tempTithi);
                        }
                    }
                    for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                    {
                        NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                        if ((tempNakshatra.NakshatraCode == ENakshatra.KRITTIKA || tempNakshatra.NakshatraCode == ENakshatra.PUNARVASU || tempNakshatra.NakshatraCode == ENakshatra.UTTARAPHALGUNI || tempNakshatra.NakshatraCode == ENakshatra.VISAKHA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAASHADHA || tempNakshatra.NakshatraCode == ENakshatra.PURVABHADRAPADA) && tempNakshatra.DateStart < endPeriod)
                        {
                            if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                nakshatraList.Add(tempNakshatra);
                        }
                    }
                }
            }

            if (nakshatraList.Count > 0 && tithiList.Count > 0)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(s => s.DateEnd > startPeriod).ToList();
                List<TithiCalendar> dateT = tithiList.Where(s => s.DateEnd > startPeriod).ToList();
                if ((dateN != null && dateN.Count > 0) && (dateT != null && dateT.Count > 0))
                {
                    foreach (NakshatraCalendar nc in dateN)
                    {
                        foreach (TithiCalendar tc in dateT)
                        {
                            bool overlap = nc.DateStart < tc.DateEnd && tc.DateStart < nc.DateEnd;
                            if (overlap)
                                tripushkarJoga.Add(new YogaCalendar(
                                                            EYoga.TRIPUSHKAR, 
                                                            CalculateJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart, tc.DateStart), 
                                                            CalculateJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd, tc.DateEnd), 
                                                            jogaPeriod[0].Date.DayOfWeek, 
                                                            nc.NakshatraCode, 
                                                            tc.TithiId));
                        }
                    }
                }
            }
            return tripushkarJoga;
        }

        public List<YogaCalendar> CheckAmritaSiddhaJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> amritaSiddhaJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<NakshatraCalendar> nakshatraList = new List<NakshatraCalendar>();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.MRIGASHIRA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if (tempTithi.TithiId != 6 && tempTithi.TithiId != 21 && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.ASHWINI && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if (tempTithi.TithiId != 7 && tempTithi.TithiId != 22 && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.ANURADHA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if (tempTithi.TithiId != 8 && tempTithi.TithiId != 23 && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.PUSHYA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if (tempTithi.TithiId != 9 && tempTithi.TithiId != 24 && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.REVATI && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if (tempTithi.TithiId != 10 && tempTithi.TithiId != 25 && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.ROHINI && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if (tempTithi.TithiId != 11 && tempTithi.TithiId != 26 && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.HASTA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if (tempTithi.TithiId != 5 && tempTithi.TithiId != 20 && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (nakshatraList.Count > 0 && tithiList.Count > 0)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(s => s.DateEnd > startPeriod).ToList();
                List<TithiCalendar> dateT = tithiList.Where(s => s.DateEnd > startPeriod).ToList();
                if ((dateN != null && dateN.Count > 0) && (dateT != null && dateT.Count > 0))
                {
                    foreach (NakshatraCalendar nc in dateN)
                    {
                        foreach (TithiCalendar tc in dateT)
                        {
                            // add check: bool overlap = a.start < b.end && b.start < a.end; only after this chek if true create joga
                            bool overlap = nc.DateStart < tc.DateEnd && tc.DateStart < nc.DateEnd;
                            if (overlap)
                                amritaSiddhaJoga.Add(new YogaCalendar(
                                                            EYoga.AMRITASIDDHA, 
                                                            CalculateJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart, tc.DateStart), 
                                                            CalculateJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd, tc.DateEnd), 
                                                            jogaPeriod[0].Date.DayOfWeek, 
                                                            nc.NakshatraCode, 
                                                            tc.TithiId));
                        }
                    }
                }
            }
            return amritaSiddhaJoga;
        }

        public List<YogaCalendar> CheckSarvarthaSiddhaJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> sarvarthaSiddhaJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<NakshatraCalendar> nakshatraList = new List<NakshatraCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.SHRAVANA || tempNakshatra.NakshatraCode == ENakshatra.ROHINI || tempNakshatra.NakshatraCode == ENakshatra.PUSHYA || tempNakshatra.NakshatraCode == ENakshatra.ANURADHA || tempNakshatra.NakshatraCode == ENakshatra.MRIGASHIRA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.UTTARABHADRAPADA || tempNakshatra.NakshatraCode == ENakshatra.KRITTIKA || tempNakshatra.NakshatraCode == ENakshatra.ASHLESHA || tempNakshatra.NakshatraCode == ENakshatra.ASHWINI) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ROHINI || tempNakshatra.NakshatraCode == ENakshatra.HASTA || tempNakshatra.NakshatraCode == ENakshatra.KRITTIKA || tempNakshatra.NakshatraCode == ENakshatra.MRIGASHIRA || tempNakshatra.NakshatraCode == ENakshatra.ANURADHA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.REVATI || tempNakshatra.NakshatraCode == ENakshatra.ANURADHA || tempNakshatra.NakshatraCode == ENakshatra.ASHWINI || tempNakshatra.NakshatraCode == ENakshatra.PUNARVASU || tempNakshatra.NakshatraCode == ENakshatra.PUSHYA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ANURADHA || tempNakshatra.NakshatraCode == ENakshatra.ASHWINI || tempNakshatra.NakshatraCode == ENakshatra.PUNARVASU || tempNakshatra.NakshatraCode == ENakshatra.SHRAVANA || tempNakshatra.NakshatraCode == ENakshatra.REVATI) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.SHRAVANA || tempNakshatra.NakshatraCode == ENakshatra.SWATI || tempNakshatra.NakshatraCode == ENakshatra.ROHINI) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.MULA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAPHALGUNI || tempNakshatra.NakshatraCode == ENakshatra.UTTARAASHADHA || tempNakshatra.NakshatraCode == ENakshatra.UTTARABHADRAPADA || tempNakshatra.NakshatraCode == ENakshatra.PUSHYA || tempNakshatra.NakshatraCode == ENakshatra.ASHWINI || tempNakshatra.NakshatraCode == ENakshatra.HASTA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (nakshatraList.Count > 0)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateN != null && dateN.Count > 0)
                {
                    foreach (NakshatraCalendar nc in dateN)
                        sarvarthaSiddhaJoga.Add(new YogaCalendar(
                                                        EYoga.SARVARTHA,
                                                        CalculateNakshatraJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart),
                                                        CalculateNakshatraJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd),
                                                        jogaPeriod[0].Date.DayOfWeek,
                                                        nc.NakshatraCode,
                                                        0));
                }
            }

            return sarvarthaSiddhaJoga;
        }

        public List<YogaCalendar> CheckSiddhaJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> siddhaJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        break;

                    case DayOfWeek.Tuesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 3 || tempTithi.TithiId == 8 || tempTithi.TithiId == 13 || tempTithi.TithiId == 18 || tempTithi.TithiId == 23 || tempTithi.TithiId == 28) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 17 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 5 || tempTithi.TithiId == 10 || tempTithi.TithiId == 15 || tempTithi.TithiId == 20 || tempTithi.TithiId == 25 || tempTithi.TithiId == 30) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 1 || tempTithi.TithiId == 6 || tempTithi.TithiId == 11 || tempTithi.TithiId == 16 || tempTithi.TithiId == 21 || tempTithi.TithiId == 26) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 4 || tempTithi.TithiId == 9 || tempTithi.TithiId == 14 || tempTithi.TithiId == 19 || tempTithi.TithiId == 24 || tempTithi.TithiId == 29) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        break;

                    default:
                        break;
                }
            }

            if (tithiList.Count > 0)
            {
                List<TithiCalendar> dateT = tithiList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateT != null && dateT.Count > 0)
                {
                    foreach (TithiCalendar tc in dateT)
                        siddhaJoga.Add(new YogaCalendar(
                                                EYoga.SIDDHA, 
                                                CalculateTithiJogaStartDate(startPeriod.Value, endPeriod.Value, tc.DateStart), 
                                                CalculateTithiJogaEndDate(startPeriod.Value, endPeriod.Value, tc.DateEnd), 
                                                jogaPeriod[0].Date.DayOfWeek, 
                                                ENakshatra.NULL, 
                                                tc.TithiId));
                }
            }

            return siddhaJoga;
        }

        public List<YogaCalendar> CheckLargeSiddhaJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> largeSiddhaJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<NakshatraCalendar> nakshatraList = new List<NakshatraCalendar>();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ROHINI || tempNakshatra.NakshatraCode == ENakshatra.MRIGASHIRA || tempNakshatra.NakshatraCode == ENakshatra.PUNARVASU || tempNakshatra.NakshatraCode == ENakshatra.CHITRA || tempNakshatra.NakshatraCode == ENakshatra.SHRAVANA || tempNakshatra.NakshatraCode == ENakshatra.DHANISHTA || tempNakshatra.NakshatraCode == ENakshatra.SHATABHISHA || tempNakshatra.NakshatraCode == ENakshatra.PURVABHADRAPADA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 17 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ASHWINI || tempNakshatra.NakshatraCode == ENakshatra.MRIGASHIRA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAPHALGUNI || tempNakshatra.NakshatraCode == ENakshatra.CHITRA || tempNakshatra.NakshatraCode == ENakshatra.ANURADHA || tempNakshatra.NakshatraCode == ENakshatra.MULA || tempNakshatra.NakshatraCode == ENakshatra.DHANISHTA || tempNakshatra.NakshatraCode == ENakshatra.PURVABHADRAPADA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ROHINI || tempNakshatra.NakshatraCode == ENakshatra.MRIGASHIRA || tempNakshatra.NakshatraCode == ENakshatra.ARDRA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAPHALGUNI || tempNakshatra.NakshatraCode == ENakshatra.ANURADHA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAASHADHA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 3 || tempTithi.TithiId == 7 || tempTithi.TithiId == 8 || tempTithi.TithiId == 12 || tempTithi.TithiId == 13 || tempTithi.TithiId == 17 || tempTithi.TithiId == 18 || tempTithi.TithiId == 22 || tempTithi.TithiId == 23 || tempTithi.TithiId == 27 || tempTithi.TithiId == 28) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ASHWINI || tempNakshatra.NakshatraCode == ENakshatra.PUNARVASU || tempNakshatra.NakshatraCode == ENakshatra.PUSHYA || tempNakshatra.NakshatraCode == ENakshatra.MAGHA || tempNakshatra.NakshatraCode == ENakshatra.SWATI || tempNakshatra.NakshatraCode == ENakshatra.PURVAASHADHA || tempNakshatra.NakshatraCode == ENakshatra.PURVABHADRAPADA || tempNakshatra.NakshatraCode == ENakshatra.REVATI) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 4 || tempTithi.TithiId == 5 || tempTithi.TithiId == 7 || tempTithi.TithiId == 9 || tempTithi.TithiId == 13 || tempTithi.TithiId == 14 || tempTithi.TithiId == 19 || tempTithi.TithiId == 20 || tempTithi.TithiId == 22 || tempTithi.TithiId == 24 || tempTithi.TithiId == 28 || tempTithi.TithiId == 29) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ASHWINI || tempNakshatra.NakshatraCode == ENakshatra.BHARANI || tempNakshatra.NakshatraCode == ENakshatra.ARDRA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAPHALGUNI || tempNakshatra.NakshatraCode == ENakshatra.CHITRA || tempNakshatra.NakshatraCode == ENakshatra.SWATI || tempNakshatra.NakshatraCode == ENakshatra.PURVAASHADHA || tempNakshatra.NakshatraCode == ENakshatra.REVATI) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 1 || tempTithi.TithiId == 2 || tempTithi.TithiId == 6 || tempTithi.TithiId == 7 || tempTithi.TithiId == 11 || tempTithi.TithiId == 12 || tempTithi.TithiId == 16 || tempTithi.TithiId == 17 || tempTithi.TithiId == 21 || tempTithi.TithiId == 22 || tempTithi.TithiId == 26 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.ROHINI || tempNakshatra.NakshatraCode == ENakshatra.SWATI || tempNakshatra.NakshatraCode == ENakshatra.VISAKHA || tempNakshatra.NakshatraCode == ENakshatra.ANURADHA || tempNakshatra.NakshatraCode == ENakshatra.DHANISHTA || tempNakshatra.NakshatraCode == ENakshatra.SHATABHISHA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 4 || tempTithi.TithiId == 7 || tempTithi.TithiId == 9 || tempTithi.TithiId == 12 || tempTithi.TithiId == 14 || tempTithi.TithiId == 17 || tempTithi.TithiId == 19 || tempTithi.TithiId == 22 || tempTithi.TithiId == 24 || tempTithi.TithiId == 27 || tempTithi.TithiId == 29) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if ((tempNakshatra.NakshatraCode == ENakshatra.PUSHYA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAPHALGUNI || tempNakshatra.NakshatraCode == ENakshatra.HASTA || tempNakshatra.NakshatraCode == ENakshatra.MULA || tempNakshatra.NakshatraCode == ENakshatra.UTTARAASHADHA || tempNakshatra.NakshatraCode == ENakshatra.SHRAVANA || tempNakshatra.NakshatraCode == ENakshatra.UTTARABHADRAPADA) && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 1 || tempTithi.TithiId == 4 || tempTithi.TithiId == 6 || tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 16 || tempTithi.TithiId == 19 || tempTithi.TithiId == 21 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (nakshatraList.Count > 0 && tithiList.Count > 0 && dayOfWeek != DayOfWeek.Tuesday)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(s => s.DateEnd > startPeriod).ToList();
                List<TithiCalendar> dateT = tithiList.Where(s => s.DateEnd > startPeriod).ToList();
                if ((dateN != null && dateN.Count > 0) && (dateT != null && dateT.Count > 0))
                {
                    foreach (NakshatraCalendar nc in dateN)
                    {
                        foreach (TithiCalendar tc in dateT)
                        {
                            // add check: bool overlap = a.start < b.end && b.start < a.end; only after this chek if true create joga
                            bool overlap = nc.DateStart < tc.DateEnd && tc.DateStart < nc.DateEnd;
                            if (overlap)
                                largeSiddhaJoga.Add(new YogaCalendar(
                                                            EYoga.SIDDHA, 
                                                            CalculateJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart, tc.DateStart), 
                                                            CalculateJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd, tc.DateEnd), 
                                                            jogaPeriod[0].Date.DayOfWeek, 
                                                            nc.NakshatraCode, 
                                                            tc.TithiId));
                        }
                    }
                }
            }
            else if (nakshatraList.Count > 0 && dayOfWeek == DayOfWeek.Tuesday)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateN != null && dateN.Count > 0)
                {
                    foreach (NakshatraCalendar nc in dateN)
                        largeSiddhaJoga.Add(new YogaCalendar(
                                                    EYoga.SIDDHA, 
                                                    CalculateNakshatraJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart), 
                                                    CalculateNakshatraJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd), 
                                                    jogaPeriod[0].Date.DayOfWeek, 
                                                    nc.NakshatraCode, 
                                                    0));
                }
            }

            return largeSiddhaJoga;
        }

        public List<YogaCalendar> CheckMritjuJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> mritjuJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 17 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 1 || tempTithi.TithiId == 6 || tempTithi.TithiId == 11 || tempTithi.TithiId == 16 || tempTithi.TithiId == 21 || tempTithi.TithiId == 26) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 3 || tempTithi.TithiId == 8 || tempTithi.TithiId == 13 || tempTithi.TithiId == 18 || tempTithi.TithiId == 23 || tempTithi.TithiId == 28) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 4 || tempTithi.TithiId == 9 || tempTithi.TithiId == 14 || tempTithi.TithiId == 19 || tempTithi.TithiId == 24 || tempTithi.TithiId == 29) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 2 || tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 17 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 5 || tempTithi.TithiId == 10 || tempTithi.TithiId == 15 || tempTithi.TithiId == 20 || tempTithi.TithiId == 25 || tempTithi.TithiId == 30) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 1 || tempTithi.TithiId == 6 || tempTithi.TithiId == 11 || tempTithi.TithiId == 16 || tempTithi.TithiId == 21 || tempTithi.TithiId == 26) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (tithiList.Count > 0)
            {
                List<TithiCalendar> dateT = tithiList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateT != null && dateT.Count > 0)
                {
                    foreach (TithiCalendar tc in dateT)
                        mritjuJoga.Add(new YogaCalendar(
                                                EYoga.MRITYU, 
                                                CalculateTithiJogaStartDate(startPeriod.Value, endPeriod.Value, tc.DateStart), 
                                                CalculateTithiJogaEndDate(startPeriod.Value, endPeriod.Value, tc.DateEnd), 
                                                jogaPeriod[0].Date.DayOfWeek, 
                                                ENakshatra.NULL, 
                                                tc.TithiId));
                }
            }

            return mritjuJoga;
        }

        public List<YogaCalendar> CheckAdhamJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> adhamJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 11 || tempTithi.TithiId == 21) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 10 || tempTithi.TithiId == 25) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 1 || tempTithi.TithiId == 9 || tempTithi.TithiId == 16 || tempTithi.TithiId == 24) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 8 || tempTithi.TithiId == 23) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 7 || tempTithi.TithiId == 22) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 6 || tempTithi.TithiId == 21) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 7 || tempTithi.TithiId == 12 || tempTithi.TithiId == 22 || tempTithi.TithiId == 27) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (tithiList.Count > 0)
            {
                List<TithiCalendar> dateT = tithiList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateT != null && dateT.Count > 0)
                {
                    foreach (TithiCalendar tc in dateT)
                        adhamJoga.Add(new YogaCalendar(
                                                EYoga.ADHAM, 
                                                CalculateTithiJogaStartDate(startPeriod.Value, endPeriod.Value, tc.DateStart), 
                                                CalculateTithiJogaEndDate(startPeriod.Value, endPeriod.Value, tc.DateEnd), 
                                                jogaPeriod[0].Date.DayOfWeek, 
                                                ENakshatra.NULL, 
                                                tc.TithiId));
                }
            }

            return adhamJoga;
        }

        public List<YogaCalendar> CheckYamaghataJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> yamaghataJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<NakshatraCalendar> nakshatraList = new List<NakshatraCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.VISAKHA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.ARDRA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.MULA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.KRITTIKA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.ROHINI && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.HASTA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.MAGHA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (nakshatraList.Count > 0)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateN != null && dateN.Count > 0)
                {
                    foreach (NakshatraCalendar nc in dateN)
                        yamaghataJoga.Add(new YogaCalendar(
                                                    EYoga.YAMAGHATA, 
                                                    CalculateNakshatraJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart), 
                                                    CalculateNakshatraJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd), 
                                                    jogaPeriod[0].Date.DayOfWeek, 
                                                    nc.NakshatraCode, 
                                                    0));
                }
            }

            return yamaghataJoga;
        }

        public List<YogaCalendar> CheckDagdhaJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> dagdhaJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<NakshatraCalendar> nakshatraList = new List<NakshatraCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.CHITRA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.UTTARAASHADHA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.DHANISHTA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.UTTARAPHALGUNI && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.JYESHTHA && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.REVATI && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int nak = 0; nak < jogaPeriod[i].NakshatraDayList.Count; nak++)
                        {
                            NakshatraCalendar tempNakshatra = (NakshatraCalendar)jogaPeriod[i].NakshatraDayList[nak];
                            if (tempNakshatra.NakshatraCode == ENakshatra.BHARANI && tempNakshatra.DateStart < endPeriod)
                            {
                                if (nakshatraList.Where(s => s.NakshatraCode == tempNakshatra.NakshatraCode).FirstOrDefault() == null)
                                    nakshatraList.Add(tempNakshatra);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (nakshatraList.Count > 0)
            {
                List<NakshatraCalendar> dateN = nakshatraList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateN != null && dateN.Count > 0)
                {
                    foreach (NakshatraCalendar nc in dateN)
                        dagdhaJoga.Add(new YogaCalendar(
                                                EYoga.DAGDHA, 
                                                CalculateNakshatraJogaStartDate(startPeriod.Value, endPeriod.Value, nc.DateStart), 
                                                CalculateNakshatraJogaEndDate(startPeriod.Value, endPeriod.Value, nc.DateEnd), 
                                                jogaPeriod[0].Date.DayOfWeek, 
                                                nc.NakshatraCode, 
                                                0));
                }
            }

            return dagdhaJoga;
        }

        public List<YogaCalendar> CheckUnfarobaleJoga(DateTime? startPeriod, DateTime? endPeriod, List<Day> daysList)
        {
            List<YogaCalendar> unfarobaleJoga = new List<YogaCalendar>();
            List<Day> jogaPeriod = daysList.Where(i => i.Date >= startPeriod.Value.Date && i.Date <= endPeriod.Value.Date).ToList();
            List<TithiCalendar> tithiList = new List<TithiCalendar>();

            DayOfWeek dayOfWeek = jogaPeriod[0].Date.DayOfWeek;
            for (int i = 0; i < jogaPeriod.Count; i++)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 6 || tempTithi.TithiId == 26) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 5 || tempTithi.TithiId == 7 || tempTithi.TithiId == 20 || tempTithi.TithiId == 22) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 3 || tempTithi.TithiId == 8 || tempTithi.TithiId == 18 || tempTithi.TithiId == 23) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 6 || tempTithi.TithiId == 9 || tempTithi.TithiId == 21 || tempTithi.TithiId == 24) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 8 || tempTithi.TithiId == 9 || tempTithi.TithiId == 10 || tempTithi.TithiId == 23 || tempTithi.TithiId == 24 || tempTithi.TithiId == 25) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Saturday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 7 || tempTithi.TithiId == 9 || tempTithi.TithiId == 11 || tempTithi.TithiId == 22 || tempTithi.TithiId == 24 || tempTithi.TithiId == 26) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    case DayOfWeek.Sunday:
                        for (int thi = 0; thi < jogaPeriod[i].TithiDayList.Count; thi++)
                        {
                            TithiCalendar tempTithi = (TithiCalendar)jogaPeriod[i].TithiDayList[thi];
                            if ((tempTithi.TithiId == 4 || tempTithi.TithiId == 9) && tempTithi.DateStart < endPeriod)
                            {
                                if (tithiList.Where(s => s.TithiId == tempTithi.TithiId).FirstOrDefault() == null)
                                    tithiList.Add(tempTithi);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (tithiList.Count > 0)
            {
                List<TithiCalendar> dateT = tithiList.Where(s => s.DateEnd > startPeriod).ToList();
                if (dateT != null && dateT.Count > 0)
                {
                    foreach (TithiCalendar tc in dateT)
                        unfarobaleJoga.Add(new YogaCalendar(
                                                EYoga.UNFAVORABLE, 
                                                CalculateTithiJogaStartDate(startPeriod.Value, endPeriod.Value, tc.DateStart), 
                                                CalculateTithiJogaEndDate(startPeriod.Value, endPeriod.Value, tc.DateEnd), 
                                                jogaPeriod[0].Date.DayOfWeek, 
                                                ENakshatra.NULL, 
                                                tc.TithiId));
                }
            }

            return unfarobaleJoga;
        }



    }
}
