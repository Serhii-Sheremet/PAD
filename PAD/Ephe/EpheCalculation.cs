using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace PAD
{
    public class EpheCalculation
    {
        private const Int64 SEFLG_EPHMASK = (EpheConstants.SEFLG_JPLEPH | EpheConstants.SEFLG_SWIEPH | EpheConstants.SEFLG_MOSEPH);
        private Int64 iflag;
        private Int64 whicheph;
        private int gregflag;

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private int DateTimeToEPOCH(DateTime date)
        {
            TimeSpan t = date - epoch;
            return (int)t.TotalSeconds;
        }

        private DateTime EPOCHToDateTime(int time)
        {
            return epoch.AddSeconds(time);
        }

        private EpheResults GetMrityuBhagaTimeFromEpoch(int currentZnak, DateTime curDate, EpheParameters eParameters, MrityuBhagaParameters mbParameters, Func<EpheParameters, MrityuBhagaParameters, int, int, int, EpheResults> calcFunc)
        {
            EpheResults mbResults = new EpheResults() {DateInSeconds = DateTimeToEPOCH(curDate) };
            int timeUnit = 100000;
            while (timeUnit != 0)
            {
                if (mbResults.DateNotFound)
                    return mbResults;
                mbResults = calcFunc(eParameters, mbParameters, currentZnak, mbResults.DateInSeconds, timeUnit);
                mbResults.DateInSeconds -= timeUnit;
                timeUnit = timeUnit == 1 ? 0 : timeUnit / 10;
            }
            return mbResults;
        }

        public List<MrityuBhagaData> CalculateMrityuBhagaDataList_London(List<MrityuBhaga> mbList, EPlanet planetId, DateTime fromDate, DateTime toDate)
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            double[] calcRes = new double[6];
            double longitude = -0.17, latitude = 51.5, altitude = 0; // London
            double longitudeFrom = 0.00, longitudeTo = 0.00;
            double degree = 0, degreeFrom = 0, degreeTo = 0;
            DateTime curDate = fromDate;
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            DateTime newPeriodDate = fromDate;
            int planetConstant = -1, currentZnak = 0, newZnak = 0;
            bool currentRetrogradeStatus = false, newRetrogradeStatus = false;

            EAppSetting mbSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.MRITYUBHAGA.ToString()) && i.Active == 1).FirstOrDefault().Id;
            List<MrityuBhagaData> mbDataList = new List<MrityuBhagaData>();

            if (planetId == EPlanet.KETUMEAN)
            {
                planetConstant = EpheConstants.SE_MEAN_NODE;
            }
            else if (planetId == EPlanet.KETUTRUE)
            {
                planetConstant = EpheConstants.SE_TRUE_NODE;
            }
            else
            {
                planetConstant = Utility.GetPlanetSWEConstByPlanetId((int)planetId);
            }
            EpheParameters eParameters = new EpheParameters() { PalnetId = planetId, PlanetConst = planetConstant, Longitude = longitude, Latitude = latitude, Altitude = altitude };

            EpheResults mbResults = new EpheResults() { CalcResults = SWEPH_Calculation(planetConstant, curDate, longitude, latitude, altitude) };
            if (eParameters.PalnetId == EPlanet.KETUMEAN || eParameters.PalnetId == EPlanet.KETUTRUE)
            {
                mbResults.CalcResults[0] = CalculateKetuDegree(mbResults.CalcResults[0]);
            }
            currentZnak = Utility.GetZodiakIdFromDegree(mbResults.CalcResults[0]);
            currentRetrogradeStatus = GetCurrentRetrogradeStatus(mbResults.CalcResults[3]);
            newZnak = currentZnak;
            newRetrogradeStatus = currentRetrogradeStatus;
            degree = GetMBDegreeForPlanet_Znak(mbList, (int)planetId, currentZnak);
            GetMrityuBhagaDegreeFromAndTo(mbSettings, degree, out degreeFrom, out degreeTo);

            for (curDate = fromDate; curDate < toDate;)
            {
                if (curDate > toDate)
                    break;

                if (newZnak != currentZnak || newRetrogradeStatus != currentRetrogradeStatus)
                {
                    currentZnak = newZnak;
                    currentRetrogradeStatus = newRetrogradeStatus;
                    degree = GetMBDegreeForPlanet_Znak(mbList, (int)planetId, currentZnak);
                    GetMrityuBhagaDegreeFromAndTo(mbSettings, degree, out degreeFrom, out degreeTo);
                }
                MrityuBhagaParameters mbParameters = new MrityuBhagaParameters() { Degree = degree, DegreeFrom = degreeFrom, DegreeTo = degreeTo };

                if (mbResults.CalcResults[0] > degreeFrom && mbResults.CalcResults[0] < degreeTo)
                {
                    mbParameters.IsCalcFrom = true;
                    mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreInTimePeriodBackward);
                    longitudeFrom = mbResults.CalcResults[0];
                    dateFrom = EPOCHToDateTime(mbResults.DateInSeconds);

                    curDate = newPeriodDate;
                    mbParameters.IsCalcFrom = false;

                    mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreeToInTimePeriod);
                    longitudeTo = mbResults.CalcResults[0];
                    dateTo = EPOCHToDateTime(mbResults.DateInSeconds);
                    curDate = dateTo;

                    MrityuBhagaData mbd = new MrityuBhagaData
                    {
                        PlanetId = (int)planetId,
                        ZodiakId = currentZnak,
                        Degree = degree,
                        MrityuBhagaSetting = mbSettings,
                        LongitudeFrom = longitudeFrom,
                        LongitudeTo = longitudeTo,
                        DateFrom = dateFrom,
                        DateTo = dateTo
                    };
                    mbDataList.Add(mbd);
                }

                if (mbResults.CalcResults[0] > degreeTo && mbResults.CalcResults[3] < 0)
                {
                    mbParameters.DegreeFrom = degreeTo;
                    mbParameters.DegreeTo = degreeTo;
                    mbParameters.IsCalcFrom = true;
                    mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreToInTimePeriodRetrograde);

                    if (!mbResults.DateNotFound)
                    {
                        longitudeFrom = mbResults.CalcResults[0];
                        dateFrom = EPOCHToDateTime(mbResults.DateInSeconds);
                        curDate = dateFrom;

                        curDate = curDate.AddDays(+1);

                        mbParameters.DegreeFrom = degreeFrom;
                        mbParameters.DegreeTo = degreeTo;
                        mbParameters.IsCalcFrom = false;
                        mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreeToInTimePeriod);
                        longitudeTo = mbResults.CalcResults[0];
                        dateTo = EPOCHToDateTime(mbResults.DateInSeconds);
                        curDate = dateTo;

                        MrityuBhagaData mbd = new MrityuBhagaData
                        {
                            PlanetId = (int)planetId,
                            ZodiakId = currentZnak,
                            Degree = degree,
                            MrityuBhagaSetting = mbSettings,
                            LongitudeFrom = longitudeFrom,
                            LongitudeTo = longitudeTo,
                            DateFrom = dateFrom,
                            DateTo = dateTo
                        };
                        mbDataList.Add(mbd);
                    }
                }

                if (mbResults.CalcResults[0] < degreeFrom && mbResults.CalcResults[3] > 0)
                {
                    mbParameters.IsCalcFrom = true;
                    mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreeToInTimePeriod);

                    if (!mbResults.DateNotFound)
                    {
                        longitudeFrom = mbResults.CalcResults[0];
                        dateFrom = EPOCHToDateTime(mbResults.DateInSeconds);
                        curDate = dateFrom;

                        mbParameters.IsCalcFrom = false;
                        mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreeToInTimePeriod);
                        longitudeTo = mbResults.CalcResults[0];
                        dateTo = EPOCHToDateTime(mbResults.DateInSeconds);
                        curDate = dateTo;

                        MrityuBhagaData mbd = new MrityuBhagaData
                        {
                            PlanetId = (int)planetId,
                            ZodiakId = currentZnak,
                            Degree = degree,
                            MrityuBhagaSetting = mbSettings,
                            LongitudeFrom = longitudeFrom,
                            LongitudeTo = longitudeTo,
                            DateFrom = dateFrom,
                            DateTo = dateTo
                        };
                        mbDataList.Add(mbd);
                    }
                }

                if (longitudeTo <= longitudeFrom && longitudeFrom != 0 && longitudeTo != 0 && mbResults.CalcResults[3] > 0)
                {
                    curDate = curDate.AddDays(+1);

                    mbParameters.DegreeTo = degreeFrom;
                    mbParameters.IsCalcFrom = true;
                    mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreeToInTimePeriod);

                    if (!mbResults.DateNotFound)
                    {
                        longitudeFrom = mbResults.CalcResults[0];
                        dateFrom = EPOCHToDateTime(mbResults.DateInSeconds);
                        curDate = dateFrom;

                        mbParameters.DegreeTo = degreeTo;
                        mbParameters.IsCalcFrom = false;
                        mbResults = GetMrityuBhagaTimeFromEpoch(currentZnak, curDate, eParameters, mbParameters, CheckDegreeToInTimePeriod);
                        longitudeTo = mbResults.CalcResults[0];
                        dateTo = EPOCHToDateTime(mbResults.DateInSeconds);
                        curDate = dateTo;

                        MrityuBhagaData mbd = new MrityuBhagaData
                        {
                            PlanetId = (int)planetId,
                            ZodiakId = currentZnak,
                            Degree = degree,
                            MrityuBhagaSetting = mbSettings,
                            LongitudeFrom = longitudeFrom,
                            LongitudeTo = longitudeTo,
                            DateFrom = dateFrom,
                            DateTo = dateTo
                        };
                        mbDataList.Add(mbd);
                        newPeriodDate = curDate;
                    }
                }
                else
                {
                    curDate = curDate.AddHours(+1);
                    mbResults = GetTimeOfNextZnakFromEpoch(curDate, currentZnak, currentRetrogradeStatus, eParameters, GetTimeOfNextZnak);
                    newPeriodDate = EPOCHToDateTime(mbResults.DateInSeconds);
                    newZnak = mbResults.Znak;
                    newRetrogradeStatus = mbResults.RetrogradeStatus;
                }

                curDate = newPeriodDate;
            }
            return mbDataList;
        }

        private void GetMrityuBhagaDegreeFromAndTo(EAppSetting mbSettings, double degree, out double degreeFrom, out double degreeTo)
        {
            degreeFrom = 0;
            degreeTo = 0;
            switch (mbSettings)
            {
                case EAppSetting.MRITYUBHAGANEQUAL:
                    degreeFrom = degree - 0.5;
                    degreeTo = degree + 0.5;
                    break;

                case EAppSetting.MRITYUBHAGANLESS:
                    degreeFrom = degree - 1;
                    degreeTo = degree;
                    break;

                case EAppSetting.MRITYUBHAGANMORE:
                    degreeFrom = degree;
                    degreeTo = degree + 1;
                    break;

                case EAppSetting.MRITYUBHAGAERNST:
                    degreeFrom = degree - 1;
                    degreeTo = degree + 1;
                    break;
            }
        }

        private EpheResults CheckDegreeToInTimePeriod(EpheParameters eParameters, MrityuBhagaParameters mbParameters, int currentZnak, int curDate, int tsStep)
        {
            EpheResults mbResults = new EpheResults() { Znak = 0, CalcResults = new double[6], DateInSeconds = curDate };

            for (int date = curDate; date < (curDate + TimeSpan.FromDays(800).TotalSeconds); )
            {
                mbResults.CalcResults = SWEPH_Calculation(eParameters.PlanetConst, EPOCHToDateTime(date), eParameters.Longitude, eParameters.Latitude, eParameters.Altitude);
                if (eParameters.PalnetId == EPlanet.KETUMEAN || eParameters.PalnetId == EPlanet.KETUTRUE)
                {
                    mbResults.CalcResults[0] = CalculateKetuDegree(mbResults.CalcResults[0]);
                }
                mbResults.Znak = Utility.GetZodiakIdFromDegree(mbResults.CalcResults[0]);

                if(mbResults.Znak != currentZnak && mbResults.CalcResults[3] < 0)
                {
                    mbResults.DateNotFound = true;
                    return mbResults;
                }

                if ((mbResults.CalcResults[0] - mbParameters.Degree) <= 30 && mbResults.CalcResults[3] < 0 && mbResults.CalcResults[0] <= mbParameters.DegreeFrom && mbParameters.DegreeFrom == mbParameters.DegreeTo)
                {
                    date += tsStep;
                    continue;
                }
                if (((mbResults.CalcResults[0] - mbParameters.Degree) <= 30 && mbResults.CalcResults[0] >= mbParameters.DegreeFrom && mbResults.CalcResults[3] > 0 && mbParameters.IsCalcFrom) || ((mbResults.CalcResults[0] - mbParameters.Degree) <= 30 && mbResults.CalcResults[0] >= mbParameters.DegreeTo) || ((mbResults.CalcResults[0] - mbParameters.Degree) <= 30 && mbResults.CalcResults[3] < 0 && mbResults.CalcResults[0] <= mbParameters.DegreeFrom && !mbParameters.IsCalcFrom) )
                {
                    mbResults.DateInSeconds = date;
                    return mbResults;
                }
                date += tsStep;
            }
            return mbResults;
        }

        private EpheResults CheckDegreToInTimePeriodRetrograde(EpheParameters eParameters, MrityuBhagaParameters mbParameters, int currentZnak, int curDate, int tsStep)
        {
            EpheResults mbResults = new EpheResults() { Znak = 0, CalcResults = new double[6], DateInSeconds = curDate };

            for (int date = curDate; date < (curDate + TimeSpan.FromDays(800).TotalSeconds);)
            {
                mbResults.CalcResults = SWEPH_Calculation(eParameters.PlanetConst, EPOCHToDateTime(date), eParameters.Longitude, eParameters.Latitude, eParameters.Altitude);
                if (eParameters.PalnetId == EPlanet.KETUMEAN || eParameters.PalnetId == EPlanet.KETUTRUE)
                {
                    mbResults.CalcResults[0] = CalculateKetuDegree(mbResults.CalcResults[0]);
                }
                mbResults.Znak = Utility.GetZodiakIdFromDegree(mbResults.CalcResults[0]);

                if (mbResults.CalcResults[3] >= 0 )
                {
                    mbResults.DateNotFound = true;
                    return mbResults;
                }

                if ((mbParameters.Degree - mbResults.CalcResults[0]) <= 30 && mbResults.CalcResults[0] <= mbParameters.DegreeTo)
                {
                    mbResults.DateInSeconds = date;
                    return mbResults;
                }
                date += tsStep;
            }
            return mbResults;
        }

        private EpheResults CheckDegreInTimePeriodBackward(EpheParameters eParameters, MrityuBhagaParameters mbParameters, int currentZnak, int curDate, int tsStep)
        {
            EpheResults mbResults = new EpheResults() { Znak = 0, CalcResults = new double[6], DateInSeconds = curDate };

            for (int date = curDate; date > (curDate - TimeSpan.FromDays(400).TotalSeconds);)
            {
                mbResults.CalcResults = SWEPH_Calculation(eParameters.PlanetConst, EPOCHToDateTime(date), eParameters.Longitude, eParameters.Latitude, eParameters.Altitude);
                if (eParameters.PalnetId == EPlanet.KETUMEAN || eParameters.PalnetId == EPlanet.KETUTRUE)
                {
                    mbResults.CalcResults[0] = CalculateKetuDegree(mbResults.CalcResults[0]);
                }
                mbResults.Znak = Utility.GetZodiakIdFromDegree(mbResults.CalcResults[0]);

                if ((mbParameters.Degree - mbResults.CalcResults[0]) <= 30 && (mbResults.CalcResults[0] <= mbParameters.DegreeFrom || (mbResults.CalcResults[0] >= mbParameters.DegreeTo) ))
                {
                    mbResults.DateInSeconds = date;
                    return mbResults;
                }
                date -= tsStep;
            }
            return mbResults;
        }

        private EpheResults GetTimeOfNextZnakFromEpoch(DateTime curDate, int currentZnak, bool currentRetrogradeStatus, EpheParameters eParameters, Func<int, bool, EpheParameters, int, int, EpheResults> calcFunc)
        {
            EpheResults mbResults = new EpheResults() { DateInSeconds = DateTimeToEPOCH(curDate) };
            int timeUnit = 100000;
            while (timeUnit != 0)
            {
                mbResults = calcFunc(currentZnak, currentRetrogradeStatus, eParameters, mbResults.DateInSeconds, timeUnit);
                mbResults.DateInSeconds -= timeUnit;
                timeUnit = timeUnit == 1 ? 0 : timeUnit / 10;
            }
            return mbResults;
        }

        private EpheResults GetTimeOfNextZnak(int currentZnak, bool currentRetrogradeStatus, EpheParameters eParameters, int curDate, int tsStep)
        {
            EpheResults mbResults = new EpheResults() { Znak = 0, CalcResults = new double[6], DateInSeconds = curDate };

            for (int date = curDate; date < (curDate + TimeSpan.FromDays(800).TotalSeconds);)
            {
                mbResults.CalcResults = SWEPH_Calculation(eParameters.PlanetConst, EPOCHToDateTime(date), eParameters.Longitude, eParameters.Latitude, eParameters.Altitude);
                if (eParameters.PalnetId == EPlanet.KETUMEAN || eParameters.PalnetId == EPlanet.KETUTRUE)
                {
                    mbResults.CalcResults[0] = CalculateKetuDegree(mbResults.CalcResults[0]);
                }
                mbResults.Znak = Utility.GetZodiakIdFromDegree(mbResults.CalcResults[0]);
                mbResults.RetrogradeStatus = GetCurrentRetrogradeStatus(mbResults.CalcResults[3]);

                if (mbResults.Znak != currentZnak || mbResults.RetrogradeStatus != currentRetrogradeStatus)
                {
                    mbResults.DateInSeconds = date;
                    return mbResults;
                }
                date += tsStep;
            }
            return mbResults;
        }

        private double GetMBDegreeForPlanet_Znak(List<MrityuBhaga> mbList, int planetId, int znakId)
        {
            if (planetId == 10)
                planetId = 8;
            if (planetId == 11)
                planetId = 9;
            return mbList.Where(i => i.PlanetId == planetId && i.ZodiakId == znakId).FirstOrDefault()?.Degree ?? 0;
        }

        private double CalculateKetuDegree(double rahuDegree)
        {
            double ketuDegree = rahuDegree + 180;
            if (ketuDegree > 360)
                ketuDegree = ketuDegree - 360;
            return ketuDegree;
        }

        public List<EclipseData> CalculateEclipse_London(DateTime fromDate, DateTime toDate)
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            double[] calcRes = new double[6];
            double longitude = -0.17, latitude = 51.5, altitude = 0; // London
            DateTime curDate = fromDate;
            DateTime dateChange;

            List<EclipseData> ecDataList = new List<EclipseData>();

            while (curDate < toDate.AddSeconds(+1))
            {
                dateChange = SWEPH_SolarEclipse_Calculation(curDate, longitude, latitude, altitude);
                curDate = dateChange;
                EclipseData ecTemp = new EclipseData
                {
                    Date = curDate,
                    EclipseId = 2
                };
                ecDataList.Add(ecTemp);
                curDate = curDate.AddDays(+1);
            }

            curDate = fromDate;
            while (curDate < toDate.AddSeconds(+1))
            {
                dateChange = SWEPH_OccultationGlobal_Calculation(curDate, EpheConstants.SE_SUN, EpheConstants.SE_ECL_TOTAL, longitude, latitude, altitude);
                curDate = dateChange;
                EclipseData ecTemp = new EclipseData
                {
                    Date = curDate,
                    EclipseId = 2
                };
                ecDataList.Add(ecTemp);
                curDate = curDate.AddDays(+1);
            }

            curDate = fromDate;
            while (curDate < toDate.AddSeconds(+1))
            {
                dateChange = SWEPH_MoonEclipse_Calculation(curDate, longitude, latitude, altitude);
                curDate = dateChange;
                EclipseData ecTemp = new EclipseData
                {
                    Date = curDate,
                    EclipseId = 1
                };
                ecDataList.Add(ecTemp);
                curDate = curDate.AddDays(+1);
            }

            ecDataList.OrderBy(i => i.Date);
            return ecDataList;
        }

        private DateTime SWEPH_SolarEclipse_Calculation(DateTime calcDate, double lon, double lat, double alt)
        {
            iflag = 0;
            whicheph = EpheConstants.SEFLG_SWIEPH;
            gregflag = EpheConstants.SE_GREG_CAL;
            iflag |= EpheConstants.SEFLG_SIDEREAL;
            EpheFunctions.swe_set_sid_mode(EpheConstants.SE_SIDM_LAHIRI, 0, 0);
            EpheFunctions.swe_set_topo(lon, lat, alt);

            iflag = (iflag & ~SEFLG_EPHMASK) | whicheph;
            iflag |= EpheConstants.SEFLG_SPEED;
            iflag |= EpheConstants.SE_ECL_ONE_TRY;

            Int64 iflgret;
            double jut = 0.0;
            double tjd_ut = 2415020.5;
            int jday = calcDate.Day;
            int jmonth = calcDate.Month;
            int jyear = calcDate.Year;
            int jhour = calcDate.Hour;
            int jmin = calcDate.Minute;
            int jsec = calcDate.Second;

            jut = jhour + jmin / 60.0 + jsec / 3600.0;
            tjd_ut = EpheFunctions.swe_julday(jyear, jmonth, jday, jut, gregflag);

            double[] tret = new double[10];

            IntPtr ptrTretDouble = Marshal.AllocHGlobal(Marshal.SizeOf(tret[0]) * tret.Length);
            Marshal.Copy(tret, 0, ptrTretDouble, 10);

            string serr = new string('*', 256);
            IntPtr ptrErr = (IntPtr)Marshal.StringToHGlobalAnsi(serr);

            int ifltype = ~(EpheConstants.SE_ECL_TOTAL | EpheConstants.SE_ECL_PARTIAL);
            iflgret = EpheFunctions.swe_sol_eclipse_when_glob(tjd_ut, (int)iflag, ifltype, ptrTretDouble, false, ptrErr);

            Marshal.Copy(ptrTretDouble, tret, 0, 10);

            Marshal.FreeHGlobal(ptrTretDouble);
            Marshal.FreeHGlobal(ptrErr);

            DateTime date = DateTime.FromOADate(tret[0] - 2415018.5);
            return date;
        }

        private DateTime SWEPH_MoonEclipse_Calculation(DateTime calcDate, double lon, double lat, double alt)
        {
            iflag = 0;
            whicheph = EpheConstants.SEFLG_SWIEPH;
            gregflag = EpheConstants.SE_GREG_CAL;
            iflag |= EpheConstants.SEFLG_SIDEREAL;
            EpheFunctions.swe_set_sid_mode(EpheConstants.SE_SIDM_LAHIRI, 0, 0);
            EpheFunctions.swe_set_topo(lon, lat, alt);

            iflag = (iflag & ~SEFLG_EPHMASK) | whicheph;
            iflag |= EpheConstants.SEFLG_SPEED;
            iflag |= EpheConstants.SE_ECL_ONE_TRY;

            Int64 iflgret;
            double jut = 0.0;
            double tjd_ut = 2415020.5;
            int jday = calcDate.Day;
            int jmonth = calcDate.Month;
            int jyear = calcDate.Year;
            int jhour = calcDate.Hour;
            int jmin = calcDate.Minute;
            int jsec = calcDate.Second;

            jut = jhour + jmin / 60.0 + jsec / 3600.0;
            tjd_ut = EpheFunctions.swe_julday(jyear, jmonth, jday, jut, gregflag);

            double[] tret = new double[10];

            IntPtr ptrTretDouble = Marshal.AllocHGlobal(Marshal.SizeOf(tret[0]) * tret.Length);
            Marshal.Copy(tret, 0, ptrTretDouble, 10);

            string serr = new string('*', 256);
            IntPtr ptrErr = (IntPtr)Marshal.StringToHGlobalAnsi(serr);

            int ifltype = ~(EpheConstants.SE_ECL_CENTRAL | EpheConstants.SE_ECL_NONCENTRAL);
            iflgret = EpheFunctions.swe_lun_eclipse_when(tjd_ut, (int)iflag, ifltype, ptrTretDouble, false, ptrErr);

            Marshal.Copy(ptrTretDouble, tret, 0, 10);

            Marshal.FreeHGlobal(ptrTretDouble);
            Marshal.FreeHGlobal(ptrErr);

            DateTime date = DateTime.FromOADate(tret[0] - 2415018.5);
            return date;
        }

        private DateTime SWEPH_OccultationGlobal_Calculation(DateTime calcDate, int planetConst, int eclipseType, double lon, double lat, double alt)
        {
            iflag = 0;
            whicheph = EpheConstants.SEFLG_SWIEPH;
            gregflag = EpheConstants.SE_GREG_CAL;
            iflag |= EpheConstants.SEFLG_SIDEREAL;
            EpheFunctions.swe_set_sid_mode(EpheConstants.SE_SIDM_LAHIRI, 0, 0);
            EpheFunctions.swe_set_topo(lon, lat, alt);

            iflag = (iflag & ~SEFLG_EPHMASK) | whicheph;
            iflag |= EpheConstants.SEFLG_SPEED;
            iflag |= EpheConstants.SE_ECL_ONE_TRY;

            Int64 iflgret;
            double jut = 0.0;
            double tjd_ut = 2415020.5;
            int jday = calcDate.Day;
            int jmonth = calcDate.Month;
            int jyear = calcDate.Year;
            int jhour = calcDate.Hour;
            int jmin = calcDate.Minute;
            int jsec = calcDate.Second;

            jut = jhour + jmin / 60.0 + jsec / 3600.0;
            tjd_ut = EpheFunctions.swe_julday(jyear, jmonth, jday, jut, gregflag);

            double[] tret = new double[10];

            IntPtr ptrTretDouble = Marshal.AllocHGlobal(Marshal.SizeOf(tret[0]) * tret.Length);
            Marshal.Copy(tret, 0, ptrTretDouble, 10);

            string serr = new string('*', 256);
            IntPtr ptrErr = (IntPtr)Marshal.StringToHGlobalAnsi(serr);

            string starname = string.Empty;
            IntPtr ptrStar = (IntPtr)Marshal.StringToHGlobalAnsi(starname);

            iflgret = EpheFunctions.swe_lun_occult_when_glob(tjd_ut, planetConst, ptrStar, (int)iflag, eclipseType, ptrTretDouble, false, ptrErr);
            Marshal.Copy(ptrTretDouble, tret, 0, 10);

            Marshal.FreeHGlobal(ptrTretDouble);
            Marshal.FreeHGlobal(ptrErr);
            Marshal.FreeHGlobal(ptrStar);

            DateTime date = DateTime.FromOADate(tret[0] - 2415018.5);
            return date;
        }

        private double[] SWEPH_Calculation(int planetConst, DateTime calcDate, double lon, double lat, double alt)  // without TZ shift
        {
            iflag = 0;
            whicheph = EpheConstants.SEFLG_SWIEPH;
            gregflag = EpheConstants.SE_GREG_CAL;
            iflag |= EpheConstants.SEFLG_SIDEREAL;
            EpheFunctions.swe_set_sid_mode(EpheConstants.SE_SIDM_LAHIRI, 0, 0);
            EpheFunctions.swe_set_topo(lon, lat, alt);

            iflag = (iflag & ~SEFLG_EPHMASK) | whicheph;
            //iflag |= EpheConstants.SEFLG_TOPOCTR; // looks like not necessary
            iflag |= EpheConstants.SEFLG_SPEED;

            Int64 iflgret;
            double jut = 0.0;
            double tjd_ut = 2415020.5;
            double[] calcRes = new double[6];

            int jday = calcDate.Day;
            int jmonth = calcDate.Month;
            int jyear = calcDate.Year;
            int jhour = calcDate.Hour;
            int jmin = calcDate.Minute;
            int jsec = calcDate.Second;

            jut = jhour + jmin / 60.0 + jsec / 3600.0;
            tjd_ut = EpheFunctions.swe_julday(jyear, jmonth, jday, jut, gregflag);

            IntPtr ptrXDouble = Marshal.AllocHGlobal(Marshal.SizeOf(calcRes[0]) * calcRes.Length);
            Marshal.Copy(calcRes, 0, ptrXDouble, 6);

            string serr = new string('*', 256);
            IntPtr ptrErr = (IntPtr)Marshal.StringToHGlobalAnsi(serr);

            iflgret = EpheFunctions.swe_calc_ut(tjd_ut, planetConst, (int)iflag, ptrXDouble, ptrErr);

            Marshal.Copy(ptrXDouble, calcRes, 0, 6);

            Marshal.FreeHGlobal(ptrXDouble);
            Marshal.FreeHGlobal(ptrErr);

            return calcRes;
        }

        private double[] SWEPH_Calculation_withTZshift(int planetConst, DateTime calcDate, double lon, double lat, double alt, double tzone) // with TZ shift for Map drawing
        {
            iflag = 0;
            whicheph = EpheConstants.SEFLG_SWIEPH;
            gregflag = EpheConstants.SE_GREG_CAL;
            iflag |= EpheConstants.SEFLG_SIDEREAL;
            EpheFunctions.swe_set_sid_mode(EpheConstants.SE_SIDM_LAHIRI, 0, 0);
            EpheFunctions.swe_set_topo(lon, lat, alt);

            iflag = (iflag & ~SEFLG_EPHMASK) | whicheph;
            iflag |= EpheConstants.SEFLG_SPEED;

            Int64 iflgret;
            double jut = 0.0;
            double tjd_ut = 2415020.5;
            double[] calcRes = new double[6];

            int jday = calcDate.Day;
            int jmonth = calcDate.Month;
            int jyear = calcDate.Year;
            int jhour = calcDate.Hour;
            int jmin = calcDate.Minute;
            int jsec = calcDate.Second;

            int iyear = 0;
            int imonth = 0;
            int iday = 0;
            int ihour = 0;
            int imin = 0;
            int isec = 0;

            IntPtr iyear_out = Marshal.AllocHGlobal(Marshal.SizeOf(iyear));
            IntPtr imonth_out = Marshal.AllocHGlobal(Marshal.SizeOf(imonth));
            IntPtr iday_out = Marshal.AllocHGlobal(Marshal.SizeOf(iday));
            IntPtr ihour_out = Marshal.AllocHGlobal(Marshal.SizeOf(ihour));
            IntPtr imin_out = Marshal.AllocHGlobal(Marshal.SizeOf(imin));
            IntPtr isec_out = Marshal.AllocHGlobal(Marshal.SizeOf(isec));

            EpheFunctions.swe_utc_time_zone(jyear, jmonth, jday, jhour, jmin, jsec, tzone, iyear_out, imonth_out, iday_out, ihour_out, imin_out, isec_out);

            iyear = Marshal.ReadInt32(iyear_out);
            imonth = Marshal.ReadInt32(imonth_out);
            iday = Marshal.ReadInt32(iday_out);
            ihour = Marshal.ReadInt32(ihour_out);
            imin = Marshal.ReadInt32(imin_out);
            isec = Marshal.ReadInt32(isec_out);

            Marshal.FreeHGlobal(iyear_out);
            Marshal.FreeHGlobal(imonth_out);
            Marshal.FreeHGlobal(iday_out);
            Marshal.FreeHGlobal(ihour_out);
            Marshal.FreeHGlobal(imin_out);
            Marshal.FreeHGlobal(isec_out);

            jut = ihour + imin / 60.0 + isec / 3600.0;
            tjd_ut = EpheFunctions.swe_julday(iyear, imonth, iday, jut, gregflag);

            IntPtr ptrXDouble = Marshal.AllocHGlobal(Marshal.SizeOf(calcRes[0]) * calcRes.Length);
            Marshal.Copy(calcRes, 0, ptrXDouble, 6);

            string serr = new string('*', 256);
            IntPtr ptrErr = (IntPtr)Marshal.StringToHGlobalAnsi(serr);

            iflgret = EpheFunctions.swe_calc_ut(tjd_ut, planetConst, (int)iflag, ptrXDouble, ptrErr);

            Marshal.Copy(ptrXDouble, calcRes, 0, 6);

            Marshal.FreeHGlobal(ptrXDouble);
            Marshal.FreeHGlobal(ptrErr);

            return calcRes;
        }


        private NityaYogaTithiResults GetNityaYogaTimeFromEpoch(DateTime curDate, EpheParameters sunParameters, EpheParameters moonParameters, int currentYogaNakshatra, Func<EpheParameters, EpheParameters, int, int, int, NityaYogaTithiResults> calcFunc)
        {
            NityaYogaTithiResults nyResults = new NityaYogaTithiResults() { DateInSeconds = DateTimeToEPOCH(curDate) };
            int timeUnit = 100000;
            while (timeUnit != 0)
            {
                nyResults = calcFunc(sunParameters, moonParameters, currentYogaNakshatra, nyResults.DateInSeconds, timeUnit);
                nyResults.DateInSeconds -= timeUnit;
                timeUnit = timeUnit == 1 ? 0 : timeUnit / 10;
            }
            return nyResults;
        }

        public List<NityaYogaData> CalculateNityaYogaDataList_London(DateTime fromDate, DateTime toDate)
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            double longitude = -0.17, latitude = 51.5, altitude = 0; // London
            double nakshatraPart = 360.0000 / 27;
            DateTime curDate = fromDate;

            EpheParameters sunParameters = new EpheParameters() { PlanetConst = EpheConstants.SE_SUN, Longitude = longitude, Latitude = latitude, Altitude = altitude };
            EpheParameters moonParameters = new EpheParameters() { PlanetConst = EpheConstants.SE_MOON, Longitude = longitude, Latitude = latitude, Altitude = altitude };

            NityaYogaTithiResults nyResults = new NityaYogaTithiResults() { SunResults = SWEPH_Calculation(EpheConstants.SE_SUN, curDate.AddSeconds(-1), longitude, latitude, altitude),
                                                                    MoonResults = SWEPH_Calculation(EpheConstants.SE_MOON, curDate.AddSeconds(-1), longitude, latitude, altitude)};

            double yogaLongitude = GetYoga360Longitude(nyResults.SunResults[0] + nyResults.MoonResults[0] + (7 * nakshatraPart));
            int currentYogaNakshatra = Utility.GetNakshatraIdFromDegree(yogaLongitude);

            List<NityaYogaData> nyDataList = new List<NityaYogaData>();
            while (curDate < toDate.AddSeconds(+1))
            {
                nyResults = GetNityaYogaTimeFromEpoch(curDate, sunParameters, moonParameters, currentYogaNakshatra, CheckYogaNakshatraChangeInTimePeriod);
                curDate = EPOCHToDateTime(nyResults.DateInSeconds);
                yogaLongitude = GetYoga360Longitude(nyResults.SunResults[0] + nyResults.MoonResults[0] + (7 * nakshatraPart));
                currentYogaNakshatra = Utility.GetNakshatraIdFromDegree(yogaLongitude);

                NityaYogaData nyTemp = new NityaYogaData
                {
                    Date = curDate,
                    Longitude = yogaLongitude,
                    NakshatraId = currentYogaNakshatra
                };
                nyDataList.Add(nyTemp);

                curDate = curDate.AddSeconds(+1);
            }

            return nyDataList;
        }

        private NityaYogaTithiResults CheckYogaNakshatraChangeInTimePeriod(EpheParameters sunParameters, EpheParameters moonParameters, int currentYogaNakshatra, int curDate, int tsStep)
        {
            NityaYogaTithiResults nyResults = new NityaYogaTithiResults() {SunResults = new double[6], MoonResults = new double[6], DateInSeconds = curDate };
            double nakshatraPart = 360.0000 / 27;

            for (int date = curDate; date < (curDate + TimeSpan.FromDays(400).TotalSeconds);)
            {
                nyResults.SunResults = SWEPH_Calculation(sunParameters.PlanetConst, EPOCHToDateTime(date), sunParameters.Longitude, sunParameters.Latitude, sunParameters.Altitude);
                nyResults.MoonResults = SWEPH_Calculation(moonParameters.PlanetConst, EPOCHToDateTime(date), moonParameters.Longitude, moonParameters.Latitude, moonParameters.Altitude);
                double yogaLongitude = GetYoga360Longitude(nyResults.SunResults[0] + nyResults.MoonResults[0] + (7 * nakshatraPart));
                int cYogaNakshatra = Utility.GetNakshatraIdFromDegree(yogaLongitude);

                if (cYogaNakshatra != currentYogaNakshatra)
                {
                    nyResults.DateInSeconds = date;
                    return nyResults;
                }
                date += tsStep;
            }
            return nyResults;
        }

        public List<TithiData> CalculateTithiDataList_London(DateTime fromDate, DateTime toDate)
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            double longitude = -0.17, latitude = 51.5, altitude = 0; // London
            DateTime curDate = fromDate;

            EpheParameters sunParameters = new EpheParameters() { PlanetConst = EpheConstants.SE_SUN, Longitude = longitude, Latitude = latitude, Altitude = altitude };
            EpheParameters moonParameters = new EpheParameters() { PlanetConst = EpheConstants.SE_MOON, Longitude = longitude, Latitude = latitude, Altitude = altitude };

            double moonSunDifference = 0;
            NityaYogaTithiResults tResults = new NityaYogaTithiResults()
            {
                SunResults = SWEPH_Calculation(EpheConstants.SE_SUN, curDate.AddSeconds(-1), longitude, latitude, altitude),
                MoonResults = SWEPH_Calculation(EpheConstants.SE_MOON, curDate.AddSeconds(-1), longitude, latitude, altitude)
            };
            
            if ((tResults.MoonResults[0] - tResults.SunResults[0]) < 0)
            {
                moonSunDifference = (tResults.MoonResults[0] + 360.0000) - tResults.SunResults[0];
            }
            else
            {
                moonSunDifference = tResults.MoonResults[0] - tResults.SunResults[0];
            }
            int currentTithi = GetCurrentTithi(moonSunDifference);

            List<TithiData> tithiDataList = new List<TithiData>();
            while (curDate < toDate.AddSeconds(+1))
            {
                tResults = GetTithiTimeFromEpoch(curDate, sunParameters, moonParameters, currentTithi, CheckTithiChangeInTimePeriod);
                curDate = EPOCHToDateTime(tResults.DateInSeconds);
                if ((tResults.MoonResults[0] - tResults.SunResults[0]) < 0)
                {
                    moonSunDifference = (tResults.MoonResults[0] + 360.0000) - tResults.SunResults[0];
                }
                else
                {
                    moonSunDifference = tResults.MoonResults[0] - tResults.SunResults[0];
                }
                currentTithi = GetCurrentTithi(moonSunDifference);

                TithiData tTemp = new TithiData
                {
                    Date = curDate,
                    MoonSunDifference = moonSunDifference,
                    TithiId = currentTithi
                };
                tithiDataList.Add(tTemp);

                curDate = curDate.AddSeconds(+1);
            }

            return tithiDataList;
        }

        private NityaYogaTithiResults GetTithiTimeFromEpoch(DateTime curDate, EpheParameters sunParameters, EpheParameters moonParameters, int currentTithi, Func<EpheParameters, EpheParameters, int, int, int, NityaYogaTithiResults> calcFunc)
        {
            NityaYogaTithiResults tResults = new NityaYogaTithiResults() { DateInSeconds = DateTimeToEPOCH(curDate) };
            int timeUnit = 100000;
            while (timeUnit != 0)
            {
                tResults = calcFunc(sunParameters, moonParameters, currentTithi, tResults.DateInSeconds, timeUnit);
                tResults.DateInSeconds -= timeUnit;
                timeUnit = timeUnit == 1 ? 0 : timeUnit / 10;
            }
            return tResults;
        }

        private NityaYogaTithiResults CheckTithiChangeInTimePeriod(EpheParameters sunParameters, EpheParameters moonParameters, int currentTithi, int curDate, int tsStep)
        {
            NityaYogaTithiResults tResults = new NityaYogaTithiResults() { SunResults = new double[6], MoonResults = new double[6], DateInSeconds = curDate };
            double msDifference = 0;

            for (int date = curDate; date < (curDate + TimeSpan.FromDays(400).TotalSeconds);)
            {
                tResults.SunResults = SWEPH_Calculation(sunParameters.PlanetConst, EPOCHToDateTime(date), sunParameters.Longitude, sunParameters.Latitude, sunParameters.Altitude);
                tResults.MoonResults = SWEPH_Calculation(moonParameters.PlanetConst, EPOCHToDateTime(date), moonParameters.Longitude, moonParameters.Latitude, moonParameters.Altitude);
                if ((tResults.MoonResults[0] - tResults.SunResults[0]) < 0)
                {
                    msDifference = (tResults.MoonResults[0] + 360.0000) - tResults.SunResults[0];
                }
                else
                {
                    msDifference = tResults.MoonResults[0] - tResults.SunResults[0];
                }
                int cTithi = GetCurrentTithi(msDifference);

                if (cTithi != currentTithi)
                {
                    tResults.DateInSeconds = date;
                    return tResults;
                }
                date += tsStep;
            }
            return tResults;
        }

        public PlanetData CalculatePlanetData_London(int planetConstant, DateTime date)
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            double longitude = -0.17, latitude = 51.5, altitude = 0; // London
            EpheParameters eParameters = new EpheParameters() { PlanetConst = planetConstant, Longitude = longitude, Latitude = latitude, Altitude = altitude };
            EpheResults pResults = new EpheResults() { CalcResults = SWEPH_Calculation(planetConstant, date, longitude, latitude, altitude) };
            PlanetParameters pParameters = new PlanetParameters()
            {
                CurrentZnak = Utility.GetZodiakIdFromDegree(pResults.CalcResults[0]),
                CurrentNakshatra = Utility.GetNakshatraIdFromDegree(pResults.CalcResults[0]),
                CurrentPada = Utility.GetPadaIdFromDegree(pResults.CalcResults[0]),
                CurrentRetro = string.Empty
            };
            if (planetConstant != EpheConstants.SE_SUN && planetConstant != EpheConstants.SE_MOON)
            {
                pParameters.CurrentRetro = GetRetroInfo(pResults.CalcResults[3]);
            }
            PlanetData pdData = new PlanetData
            {
                PlanetId = Utility.GetPlanetIdBySWEConst(planetConstant),
                Date = date,
                Longitude = pResults.CalcResults[0],
                Latitude = pResults.CalcResults[1],
                SpeedInLongitude = pResults.CalcResults[3],
                SpedInLatitude = pResults.CalcResults[4],
                Retro = pParameters.CurrentRetro,
                ZodiakId = pParameters.CurrentZnak,
                NakshatraId = pParameters.CurrentNakshatra,
                PadaId = pParameters.CurrentPada
            };
            return pdData;
        }

        public List<PlanetData> CalculatePlanetDataList_London(int planetConstant, DateTime fromDate, DateTime toDate)
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            double longitude = -0.17, latitude = 51.5, altitude = 0; // London
            DateTime curDate = fromDate;

            EpheParameters eParameters = new EpheParameters() { PlanetConst = planetConstant, Longitude = longitude, Latitude = latitude, Altitude = altitude };
            EpheResults pResults = new EpheResults() { CalcResults = SWEPH_Calculation(planetConstant, curDate.AddSeconds(-1), longitude, latitude, altitude) };
            PlanetParameters pParameters = new PlanetParameters() { CurrentZnak = Utility.GetZodiakIdFromDegree(pResults.CalcResults[0]),
                                                                    CurrentNakshatra = Utility.GetNakshatraIdFromDegree(pResults.CalcResults[0]),
                                                                    CurrentPada = Utility.GetPadaIdFromDegree(pResults.CalcResults[0]),
                                                                    CurrentRetro = string.Empty};
            
            List<PlanetData> planetDataList = new List<PlanetData>();

            if (planetConstant != EpheConstants.SE_SUN && planetConstant != EpheConstants.SE_MOON)
            {
                pParameters.CurrentRetro = GetRetroInfo(pResults.CalcResults[3]);
            }
            
            PlanetData pdTemp = new PlanetData
            {
                Date = curDate.AddSeconds(-1),
                Longitude = pResults.CalcResults[0],
                Latitude = pResults.CalcResults[1],
                SpeedInLongitude = pResults.CalcResults[3],
                SpedInLatitude = pResults.CalcResults[4],
                Retro = pParameters.CurrentRetro,
                ZodiakId = pParameters.CurrentZnak,
                NakshatraId = pParameters.CurrentNakshatra,
                PadaId = pParameters.CurrentPada
            };
            planetDataList.Add(pdTemp);

            while (curDate < toDate.AddSeconds(+1))
            {
                pResults = GetPlanetTimeFromEpoch(curDate, eParameters, pParameters, CheckPlanetChangeInTimePeriod);
                curDate = EPOCHToDateTime(pResults.DateInSeconds);

                pParameters.CurrentZnak = Utility.GetZodiakIdFromDegree(pResults.CalcResults[0]);
                pParameters.CurrentNakshatra = Utility.GetNakshatraIdFromDegree(pResults.CalcResults[0]);
                pParameters.CurrentPada = Utility.GetPadaIdFromDegree(pResults.CalcResults[0]);
                pParameters.CurrentRetro = string.Empty;
                if (planetConstant != EpheConstants.SE_SUN && planetConstant != EpheConstants.SE_MOON)
                {
                    pParameters.CurrentRetro = GetRetroInfo(pResults.CalcResults[3]);
                }

                pdTemp = new PlanetData
                {
                    Date = curDate,
                    Longitude = pResults.CalcResults[0],
                    Latitude = pResults.CalcResults[1],
                    SpeedInLongitude = pResults.CalcResults[3],
                    SpedInLatitude = pResults.CalcResults[4],
                    Retro = pParameters.CurrentRetro,
                    ZodiakId = pParameters.CurrentZnak,
                    NakshatraId = pParameters.CurrentNakshatra,
                    PadaId = pParameters.CurrentPada
                };
                planetDataList.Add(pdTemp);

                curDate = curDate.AddSeconds(+1);
            }
            return planetDataList;
        }

        private EpheResults GetPlanetTimeFromEpoch(DateTime curDate, EpheParameters eParameters, PlanetParameters pParameters, Func<EpheParameters, PlanetParameters, int, int, EpheResults> calcFunc)
        {
            EpheResults pResults = new EpheResults() { DateInSeconds = DateTimeToEPOCH(curDate) };
            int timeUnit = 100000;
            while (timeUnit != 0)
            {
                pResults = calcFunc(eParameters, pParameters, pResults.DateInSeconds, timeUnit);
                pResults.DateInSeconds -= timeUnit;
                timeUnit = timeUnit == 1 ? 0 : timeUnit / 10;
            }
            return pResults;
        }

        private EpheResults CheckPlanetChangeInTimePeriod(EpheParameters eParameters, PlanetParameters pParameters, int curDate, int tsStep)
        {
            EpheResults pResults = new EpheResults() { CalcResults = new double[6], DateInSeconds = curDate };

            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;

            for (int date = curDate; date < (curDate + TimeSpan.FromDays(400).TotalSeconds);)
            {
                pResults.CalcResults = SWEPH_Calculation(eParameters.PlanetConst, EPOCHToDateTime(date), eParameters.Longitude, eParameters.Latitude, eParameters.Altitude);

                cZnak = Utility.GetZodiakIdFromDegree(pResults.CalcResults[0]);
                cNakshatra = Utility.GetNakshatraIdFromDegree(pResults.CalcResults[0]);
                cPada = Utility.GetPadaIdFromDegree(pResults.CalcResults[0]);
                cRetro = string.Empty;
                if (eParameters.PlanetConst != EpheConstants.SE_SUN && eParameters.PlanetConst != EpheConstants.SE_MOON)
                {
                    cRetro = GetRetroInfo(pResults.CalcResults[3]);
                }

                if (cZnak != pParameters.CurrentZnak)
                {
                    pResults.DateInSeconds = date;
                    return pResults;
                }
                if (cNakshatra != pParameters.CurrentNakshatra)
                {
                    pResults.DateInSeconds = date;
                    return pResults;
                }
                if (cPada != pParameters.CurrentPada)
                {
                    pResults.DateInSeconds = date;
                    return pResults;
                }
                if (!cRetro.Equals(pParameters.CurrentRetro))
                {
                    pResults.DateInSeconds = date;
                    return pResults;
                }
                date += tsStep;
            }
            return pResults;
        }

        private bool GetCurrentRetrogradeStatus(double velocity)
        {
            if (velocity < 0)
                return true;
            else
                return false;
        }

        private string GetRetroInfo(double speed)
        {
            string retro = string.Empty;
            if (speed < 0)
                retro = "R";
            else if (speed > 0)
                retro = "D";
            return retro;
        }

        private int GetCurrentTithi(double msDifference)
        {
            double tithiPart = 360.0000 / 30;

            double currentDTithi = msDifference / tithiPart;
            double intDTithi = (int)currentDTithi;

            int currentTithi = 0;
            if (currentDTithi > intDTithi)
            {
                currentTithi = Convert.ToInt32(intDTithi) + 1;
            }
            else if (currentDTithi == intDTithi)
            {
                currentTithi = Convert.ToInt32(intDTithi);
            }
            return currentTithi;
        }

        public PlanetData CalculateKetu(PlanetData rahuData)
        {
            double kLongitude = CalculateKetuDegree(rahuData.Longitude);
            int planetId = 0;
            if (rahuData.PlanetId == 8)
            {
                planetId = 9;
            }
            if (rahuData.PlanetId == 10)
            {
                planetId = 11;
            }

            PlanetData pdData = new PlanetData
            {
                PlanetId = planetId,
                Date = rahuData.Date,
                Longitude = kLongitude,
                Latitude = rahuData.Latitude,
                SpeedInLongitude = rahuData.SpeedInLongitude,
                SpedInLatitude = rahuData.SpedInLatitude,
                Retro = rahuData.Retro,
                ZodiakId = Utility.GetZodiakIdFromDegree(kLongitude),
                NakshatraId = Utility.GetNakshatraIdFromDegree(kLongitude),
                PadaId = Utility.GetPadaIdFromDegree(kLongitude)
            };
            return pdData;
        }

        public List<PlanetData> PrepareKetuList(List<PlanetData> rahuList)
        {
            List<PlanetData> ketuList = new List<PlanetData>();
            foreach (PlanetData pd in rahuList)
            {
                double kLongitude = CalculateKetuDegree(pd.Longitude);
                PlanetData pdTemp = new PlanetData
                {
                    Date = pd.Date,
                    Longitude = kLongitude,
                    Latitude = pd.Latitude,
                    SpeedInLongitude = pd.SpeedInLongitude,
                    SpedInLatitude = pd.SpedInLatitude,
                    Retro = pd.Retro,
                    ZodiakId = Utility.GetZodiakIdFromDegree(kLongitude),
                    NakshatraId = Utility.GetNakshatraIdFromDegree(kLongitude),
                    PadaId = Utility.GetPadaIdFromDegree(kLongitude)
                };
                ketuList.Add(pdTemp);
            }
            return ketuList;
        }

        private double GetYoga360Longitude(double longitude)
        {
            double calcLongitude = 0;
            double count = (int)longitude / 360;
            if (count == 0)
            {
                calcLongitude = longitude;
            }
            else
            {
                calcLongitude = longitude - (count * 360);
            }
            return calcLongitude;
        }

        public double[] AscendanceCalculation(DateTime calcDate, double lat, double lon, double alt , int hsys)
        {
            //double lon = -0.17, lat = 51.5, alt = 0; // London
            //double lon = 26.764657, lat = 49.506984, alt = 0;  //Chorny Ostriv
            //int hsys = 'O';

            double[] ascmc = new double[10];
            double[] cusps = new double[13];
            
            EpheFunctions.swe_set_sid_mode(EpheConstants.SE_SIDM_LAHIRI, 0, 0);
            EpheFunctions.swe_set_topo(lon, lat, alt);

            double jut = 0.0;
            double tjd_ut = 2415020.5;
            int jday = calcDate.Day;
            int jmonth = calcDate.Month;
            int jyear = calcDate.Year;
            int jhour = calcDate.Hour;
            int jmin = calcDate.Minute;
            int jsec = calcDate.Second;

            jut = jhour + jmin / 60.0 + jsec / 3600.0;
            tjd_ut = EpheFunctions.swe_julday(jyear, jmonth, jday, jut, EpheConstants.SE_GREG_CAL);

            IntPtr ptrDouble_ascmc = Marshal.AllocHGlobal(Marshal.SizeOf(ascmc[0]) * ascmc.Length);
            Marshal.Copy(ascmc, 0, ptrDouble_ascmc, 10);

            IntPtr ptrDouble_cusps = Marshal.AllocHGlobal(Marshal.SizeOf(cusps[0]) * cusps.Length);
            Marshal.Copy(cusps, 0, ptrDouble_cusps, 13);

            double iflgret = EpheFunctions.swe_houses_ex(tjd_ut, EpheConstants.SEFLG_SIDEREAL, lat, lon, hsys, ptrDouble_cusps, ptrDouble_ascmc);
            //iflgret = EpheFunctions.swe_houses(tjd_ut, lat, lon, hsys, ptrDouble_cusps, ptrDouble_ascmc);

            Marshal.Copy(ptrDouble_ascmc, ascmc, 0, 10);

            Marshal.FreeHGlobal(ptrDouble_ascmc);
            Marshal.FreeHGlobal(ptrDouble_cusps);

            return ascmc;
        }

        //char serr[AS_MAXCH];
        //double epheflag = SEFLG_SWIEPH;
        //int gregflag = SE_GREG_CAL;
        //int year = 2017;
        //int month = 4;
        //int day = 12;
        //int geo_longitude = 76.5; // positive for east, negative for west of Greenwich
        //int geo_latitude = 30.0;
        //int geo_altitude = 0.0;
        //double hour;
        //// array for atmospheric conditions
        //double datm[2];
        //datm[0] = 1013.25; // atmospheric pressure;
        //// irrelevant with Hindu method, can be set to 0
        //datm[1] = 15; // atmospheric temperature;
        //// irrelevant with Hindu method, can be set to 0
        //// array for geographic position
        //double geopos[3];
        //        geopos[0] = geo_longitude;
        //geopos[1] = geo_latitude;
        //geopos[2] = geo_altitude; // height above sea level in meters;
        //// irrelevant with Hindu method, can be set to 0
        //swe_set_topo(geopos[0], geopos[1], geopos[2]);
        //int ipl = SE_SUN; // object whose rising is wanted
        //char starname[255]; // name of star, if a star's rising is wanted
        //                    // is "" or NULL, if Sun, Moon, or planet is calculated
        //double trise; // for rising time
        //double tset; // for setting time
        //             // calculate the Julian day number of the date at 0:00 UT:
        //double tjd = swe_julday(year, month, day, 0, gregflag);
        //// convert geographic longitude to time (day fraction) and subtract it from tjd
        //// this method should be good for all geographic latitudes except near in
        //// polar regions
        //double dt = geo_longitude / 360.0;
        //tjd = tjd - dt;
        //// calculation flag for Hindu risings/settings
        //int rsmi = SE_CALC_RISE | SE_BIT_HINDU_RISING;
        //// or SE_CALC_RISE + SE_BIT_HINDU_RISING;
        //// or SE_CALC_RISE | SE_BIT_DISC_CENTER | SE_BIT_NO_REFRACTION | SE_BIT_GEOCTR_NO_ECL_LAT;
        //int return_code = swe_rise_trans(tjd, ipl, starname, epheflag, rsmi, geopos, datm[0], datm[1], &trise, serr);
        //if (return_code == ERR) {
        //// error action
        //printf("%s\n", serr);
        //}
        //// conversion to local time zone must be made by the user. The Swiss Ephemeris
        //// does not have a function for that.
        //// After that, the Julian day number of the rising time can be converted into
        //// date and time:
        //swe_revjul(trise, gregflag, &year, &month, &day, &hour);
        //printf("sunrise: date=%d/%d/%d, hour=%.6f UT\n", year, month, day, hour);
        //// To calculate the time of the sunset, you can either use the same
        //// tjd increased or trise as start date for the search.
        //rsmi = SE_CALC_SET | SE_BIT_DISC_CENTER | SE_BIT_NO_REFRACTION;
        //return_code = swe_rise_trans(tjd, ipl, starname, epheflag, rsmi, geopos, datm[0], datm[1], &tset, serr);
        //if (return_code == ERR) {
        //// error action
        //printf("%s\n", serr);
        //}
        //printf("sunset : date=%d/%d/%d, hour=%.6f UT\n", year, month, day, hour);
        //}

        //It says(given in Red & Blue colour fonts) "/ convert geographic longitude to time (day fraction) and subtract it from tjd
        //// this method should be good for all geographic latitudes except near in
        //// polar regions
        //double dt = geo_longitude / 360.0;
        //tjd = tjd - dt; ".



    }
}
