using System;
using System.Runtime.InteropServices;

namespace PAD
{
    public class EpheCalculation
    {
        private const Int64 SEFLG_EPHMASK = (EpheConstants.SEFLG_JPLEPH | EpheConstants.SEFLG_SWIEPH | EpheConstants.SEFLG_MOSEPH);
        private Int64 iflag;
        private Int64 whicheph;
        private int gregflag;

        public EpheCalculation()
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


    }
}
