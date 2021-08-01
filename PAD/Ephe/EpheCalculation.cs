using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PAD
{
    public class EpheCalculation
    {
        private const Int64 SEFLG_EPHMASK = (EpheConstants.SEFLG_JPLEPH | EpheConstants.SEFLG_SWIEPH | EpheConstants.SEFLG_MOSEPH);
        private Int64 iflag;
        private Int64 whicheph;
        private int gregflag;

        private double[] SWEPH_Calculation(int planetConst, DateTime calcDate, double lon, double lat, double alt)
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

            /*
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

            EpheFunctions.swe_utc_time_zone(jyear, jmon, jday, jhour, jmin, jsec, tzone, iyear_out, imonth_out, iday_out, ihour_out, imin_out, isec_out);

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
            */

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

        public List<PlanetData> CalculatePlanetDataList_London(int planetConstant, DateTime fromDate, DateTime toDate)
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            double[] calcRes = new double[6];
            double longitude = -0.17, latitude = 51.5, altitude = 0; // London
            DateTime curDate = fromDate;
            DateTime dateChange;

            calcRes = SWEPH_Calculation(planetConstant, curDate.AddSeconds(-1), longitude, latitude, altitude);
            int currentZnak = GetCurrentZnak(calcRes[0]);
            int currentNakshatra = GetCurrentNakshatra(calcRes[0]);
            int currentPada = GetCurrentPada(calcRes[0]);
            string currentRetro = string.Empty;
            if (planetConstant != EpheConstants.SE_SUN && planetConstant != EpheConstants.SE_MOON)
            {
                currentRetro = GetRetroInfo(calcRes[3]);
            }

            List<PlanetData> planetDataList = new List<PlanetData>();
            while (curDate < toDate.AddSeconds(+1))
            {
                TimeSpan tsStep = curDate.AddMonths(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep);
                tsStep = curDate.AddDays(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep);
                tsStep = curDate.AddHours(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep);
                tsStep = curDate.AddMinutes(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep);
                tsStep = curDate.AddSeconds(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange;
                currentZnak = GetCurrentZnak(calcRes[0]);
                currentNakshatra = GetCurrentNakshatra(calcRes[0]);
                currentPada = GetCurrentPada(calcRes[0]);
                currentRetro = string.Empty;
                if (planetConstant != EpheConstants.SE_SUN && planetConstant != EpheConstants.SE_MOON)
                {
                    currentRetro = GetRetroInfo(calcRes[3]);
                }

                PlanetData pdTemp = new PlanetData
                {
                    Date = curDate,
                    Longitude = calcRes[0],
                    Latitude = calcRes[1],
                    SpeedInLongitude = calcRes[3],
                    SpedInLatitude = calcRes[4],
                    Retro = currentRetro,
                    ZodiakId = currentZnak,
                    NakshatraId = currentNakshatra,
                    PadaId = currentPada
                };
                planetDataList.Add(pdTemp);

                curDate = curDate.AddSeconds(+1);
            }

            return planetDataList;
        }

        private DateTime CheckChangeInTimePeriod(int planetConst, double longitude, double latitude, double altitude, int currentZnak, int currentNakshatra, int currentPada, string currentRetro, DateTime curDate, TimeSpan tsStep, out double[] calcRes)
        {
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            calcRes = new double[6];

            for (DateTime date = curDate; date < curDate.AddYears(+1);)
            {
                calcRes = SWEPH_Calculation(planetConst, date, longitude, latitude, altitude);

                cZnak = GetCurrentZnak(calcRes[0]);
                cNakshatra = GetCurrentNakshatra(calcRes[0]);
                cPada = GetCurrentPada(calcRes[0]);
                cRetro = string.Empty;
                if (planetConst != EpheConstants.SE_SUN && planetConst != EpheConstants.SE_MOON)
                {
                    cRetro = GetRetroInfo(calcRes[3]);
                }

                if (cZnak != currentZnak)
                {
                    return date;
                }

                if (cNakshatra != currentNakshatra)
                {
                    return date;
                }

                if (cPada != currentPada)
                {
                    return date;
                }

                if (!cRetro.Equals(currentRetro))
                {
                    return date;
                }

                date = date.Add(tsStep);
            }
            return curDate;
        }

        private int GetCurrentZnak(double longitude)
        {
            double znakPart = 360.0000 / 12;

            double currentDZnak = longitude / znakPart;
            double intDZnak = (int)currentDZnak;

            int currentZnak = 0;
            if (currentDZnak > intDZnak)
            {
                currentZnak = Convert.ToInt32(intDZnak) + 1;
            }
            else if (currentDZnak == intDZnak)
            {
                currentZnak = Convert.ToInt32(intDZnak);
            }
            return currentZnak;
        }

        private int GetCurrentNakshatra(double longitude)
        {
            double nakshatraPart = 360.0000 / 27;

            double currentDNakshatra = longitude / nakshatraPart;
            double intDNakshatra = (int)currentDNakshatra;

            int currentNakshatra = 0;
            if (currentDNakshatra > intDNakshatra)
            {
                currentNakshatra = Convert.ToInt32(intDNakshatra) + 1;
            }
            else if (currentDNakshatra == intDNakshatra)
            {
                currentNakshatra = Convert.ToInt32(intDNakshatra);
            }
            return currentNakshatra;
        }

        private int GetCurrentPada(double longitude)
        {
            double padaPart = 360.0000 / 108;

            double currentDPada = longitude / padaPart;
            double intDPada = (int)currentDPada;

            int currentPada = 0;
            if (currentDPada > intDPada)
            {
                currentPada = Convert.ToInt32(intDPada) + 1;
            }
            else if (currentDPada == intDPada)
            {
                currentPada = Convert.ToInt32(intDPada);
            }
            return currentPada;
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



    }
}
