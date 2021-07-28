using System;
using System.Runtime.InteropServices;

namespace PAD
{
    public class SwephCalculation
    {
        private const Int64 SEFLG_EPHMASK = (EpheConstants.SEFLG_JPLEPH | EpheConstants.SEFLG_SWIEPH | EpheConstants.SEFLG_MOSEPH);
        private Int64 iflag;
        private Int64 whicheph;
        private int gregflag;

        public SwephCalculation()
        {
            EpheFunctions.swe_set_ephe_path(@".\ephe");
            iflag = 0;
            whicheph = EpheConstants.SEFLG_SWIEPH;
            gregflag = EpheConstants.SE_GREG_CAL;
        }

        public double[] SWE_Calculation(int planetConst, DateTime calcDate, double lon, double lat, double alt)
        {
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
            int jmon = calcDate.Month;
            int jyear = calcDate.Year;
            int jhour = calcDate.Hour;
            int jmin = calcDate.Minute;
            int jsec = calcDate.Second;

            jut = jhour + jmin / 60.0 + jsec / 3600.0;
            tjd_ut = EpheFunctions.swe_julday(jyear, jmon, jday, jut, gregflag);

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


    }
}
