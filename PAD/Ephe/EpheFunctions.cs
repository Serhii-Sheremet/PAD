using System;
using System.Runtime.InteropServices;

namespace PAD
{
    public static class EpheFunctions
    {
        [DllImport("swedll32.dll")]
        public static extern void swe_set_ephe_path(string path);

        [DllImport("swedll32.dll")]
        public static extern void swe_set_jpl_file(string fName);

        [DllImport("swedll32.dll")]
        public static extern IntPtr swe_version(IntPtr sver);

        /// <param name="gregFlag"></param>  // SE_JUL_CAL = 0, SE_GREG_CAL = 1,  --- 1 should be used
        /// <returns></returns>
        [DllImport("swedll32.dll")]
        public static extern double swe_julday(int year, int month, int day, double hour, int gregFlag);

        [DllImport("swedll32.dll")]
        public static extern void swe_revjul(double tjd, int gregflag, IntPtr year, IntPtr month, IntPtr day, IntPtr hours);

        [DllImport("swedll32.dll")]
        public static extern int swe_utc_to_jd(int iyear, int imonth, int iday, int ihour, int imin, double dsec, int gregflag, double[] dret, IntPtr serr); /* note : second is a decimal */

        [DllImport("swedll32.dll")]
        public static extern void swe_utc_time_zone(int iyear, int imonth, int iday, int ihour, int imin, double dsec, double d_timezone, IntPtr iyear_out, IntPtr imonth_out, IntPtr iday_out, IntPtr ihour_out, IntPtr imin_out, IntPtr dsec_out);

        [DllImport("swedll32.dll")]
        public static extern double swe_deltat(double tjd);

        [DllImport("swedll32.dll")]
        public static extern void swe_get_planet_name(int ipl, IntPtr spname);

        //double tjd_ut, /* Julian day number, UT */
        //double geolat, /* geographic latitude, in degrees */
        //double geolon, /* geographic longitude, in degrees
        //                * eastern longitude is positive,
        //                * western longitude is negative,
        //                * northern latitude is positive,
        //                * southern latitude is negative */
        //int hsys, /* house method, ascii code of one of the letters documented below */
        //double* cusps, /* array for 13 (or 37 for system G) doubles */
        //double* ascmc); /* array for 10 doubles */
        [DllImport("swedll32.dll")]
        public static extern double swe_houses(double tjd_ut, double geolat, double geolon, int hsys, IntPtr cusps, IntPtr ascmc);

        //armc      /* ARMC */
        //geolat    /* geographic latitude, in degrees */
        //eps       /* ecliptic obliquity, in degrees */
        //hsys      /* house method, one of the letters PKRCAV */
        //xpin 	    /* array of 2 doubles: ecl. longitude and latitude of the planet */
        //serr      /* return area for error or warning message */
        [DllImport("swedll32.dll")]
        public static extern double swe_house_pos(double armc, double geolat, double eps, int hsys, IntPtr xpin, IntPtr serr);

        //double tjd_ut, /* Julian day number, UT */
        //int32 iflag,   /* 0 or SEFLG_SIDEREAL or SEFLG_RADIANS or SEFLG_NONUT */
        //double geolat, /* geographic latitude, in degrees */
        //double geolon, /* geographic longitude, in degrees
        //                * eastern longitude is positive,
        //                * western longitude is negative,
        //                * northern latitude is positive,
        //                * southern latitude is negative */
        //int hsys, /* house method, one-letter case sensitive code (list, see further below) */
        //double* cusps, /* array for 13 (or 37 for system G) doubles */
        //double* ascmc); /* array for 10 doubles */
        [DllImport("swedll32.dll")]
        public static extern int swe_houses_ex(double tjd_ut, int iflag, double latitude, double longitude, int hsys, IntPtr cusps, IntPtr ascmc);

        /// <param name="tjd_et">Julian day, Ephemeris time</param>
        /// <param name="ipl">Nody number</param>
        /// <param name="iflag">a 32 bit integer containing bit flags that indicate what kind of computation is wanted</param>
        /// <param name="xx">array of 6 doubles for longitude, latitude, distance, speed in long., speed in lat., and speed in dist</param>
        /// <param name="serr">character string to return error messages in case of error</param>
        /// <returns></returns>
        [DllImport("swedll32.dll")]
        public static extern int swe_calc(double tjd_et, int ipl, int iflag, IntPtr xx, IntPtr serr);

        [DllImport("swedll32.dll")]
        public static extern int swe_calc_ut(double tjd_ut, int ipl, int iflag, IntPtr xx, IntPtr serr);

        //double tjd_start, 	/* start date for search, Jul. day UT */
        //int32 ifl, 		    /* ephemeris flag */
        //int32 ifltype, 	    /* eclipse type wanted: SE_ECL_TOTAL etc. or 0, if any eclipse type */
        //double* tret, 	    /* return array, 10 doubles, see below */
        //AS_BOOL backward, 	/* TRUE, if backward search */
        //char* serr);		    /* return error string */
        [DllImport("swedll32.dll")]
        public static extern int swe_sol_eclipse_when_glob(double tjd_start, int ifl, int ifltype, IntPtr tret, bool backward, IntPtr serr);

        //This function can be used to specify the mode for sidereal computations.
        //swe_calc() or swe_fixstar() has then to be called with the bit SEFLG_SIDEREAL.
        //If swe_set_sid_mode() is not called, the default ayanamsha(Fagan/Bradley) is used.
        //If a predefined mode is wanted, the variable sid_mode has to be set, while t0 and ayan_t0 are not considered, i.e.can be 0. 
        [DllImport("swedll32.dll")]
        public static extern void swe_set_sid_mode(int sid_mode, double t0, double ayan_t0);

        [DllImport("swedll32.dll")]
        public static extern void swe_set_topo(double geolon, double geolat, double altitude);

        [DllImport("swedll32.dll")]
        public static extern double swe_sidtime(double tjd_ut);

        [DllImport("swedll32.dll")]
        public static extern long swe_fixstar(IntPtr star, double tjd_et, long iflag, IntPtr xx, IntPtr serr);


    }
}
