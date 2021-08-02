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

        //double tjd_et, /* Julian day number in ET (TT) */
        //gregflag, /* Gregorian calendar: 1, Julian calendar: 0 */
        //int32 *iyear, int32* imonth, int32 *iday,
        //int32* ihour, int32 *imin, double* dsec); /* NOTE: second is a decimal */
        [DllImport("swedll32.dll")]
        public static extern void swe_jdet_to_utc(double tjd_et, int gregflag, IntPtr iyear, IntPtr imonth, IntPtr iday, IntPtr ihours, IntPtr imin, IntPtr isec);

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


        //int retc, y, m, d;
        //double tjdr;
        //double tjd = swe_julday(2020, 5, 22, 0, SE_GREG_CAL);
        //double dhour;
        //char serr[AS_MAXCH];
        //double geopos[6], datm[2];
        //geopos[0] = 80.25;
        //geopos[1] = 13;
        //geopos[2] = 0; // altitude above see, not used
        //retc = swe_rise_trans(tjd, SE_SUN, NULL, SEFLG_SWIEPH, SE_CALC_RISE + SE_BIT_CIVIL_TWILIGHT, geopos, 1013.25, 20, &tjdr, serr);
        //swe_revjul(tjdr, SE_GREG_CAL, &y, &m, &d, &dhour);
        //printf("%04d/%02d/%02d: %f\n", y, m, d, dhour);
        //retc = swe_rise_trans(tjd, SE_SUN, NULL, SEFLG_SWIEPH, SE_CALC_RISE + SE_BIT_NAUTIC_TWILIGHT, geopos, 1013.25, 20, &tjdr, serr);
        //swe_revjul(tjdr, SE_GREG_CAL, &y, &m, &d, &dhour);
        //printf("%04d/%02d/%02d: %f\n", y, m, d, dhour);
        //retc = swe_rise_trans(tjd, SE_SUN, NULL, SEFLG_SWIEPH, SE_CALC_RISE + SE_BIT_ASTRO_TWILIGHT, geopos, 1013.25, 20, &tjdr, serr);
        //swe_revjul(tjdr, SE_GREG_CAL, &y, &m, &d, &dhour);

        //double tjd_ut, /* search after this time (UT) */
        //int32 ipl, /* planet number, if planet or moon */
        //char* starname, /* star name, if star; must be NULL or empty, if ipl is used */
        //int32 epheflag, /* ephemeris flag */
        //int32 rsmi, /* integer specifying that rise, set, or one of the two meridian transits is wanted. see definition below */
        //double* geopos, /* array of three doubles containing
        //* geograph. long., lat., height of observer */
        //double atpress /* atmospheric pressure in mbar/hPa */
        //double attemp, /* atmospheric temperature in deg. C */
        //double* tret, /* return address (double) for rise time etc. */
        //char* serr); /* return address for error message */
        [DllImport("swedll32.dll")]
        public static extern int swe_rise_trans(double tjd_ut, int ipl, IntPtr starname, int epheflag, int rsmi, IntPtr geopos, double atpress, double attemp, IntPtr tret, IntPtr serr);

        //double tjd_start, 	/* start date for search, Jul. day UT */
        //int32 ifl, 		    /* ephemeris flag */
        //int32 ifltype, 	    /* eclipse type wanted: SE_ECL_TOTAL etc. or 0, if any eclipse type */
        //double* tret, 	    /* return array, 10 doubles, see below */
        //AS_BOOL backward, 	/* TRUE, if backward search */
        //char* serr);		    /* return error string */
        [DllImport("swedll32.dll")]
        public static extern int swe_sol_eclipse_when_glob(double tjd_start, int ifl, int ifltype, IntPtr tret, bool backward, IntPtr serr);

        //double tjd_start, /* start date for search, Jul. day UT */
        //int32 ifl, /* ephemeris flag */
        //double* geopos, /* 3 doubles for geographic lon, lat, height.
        //* eastern longitude is positive,
        //* western longitude is negative,
        //* northern latitude is positive,
        //* southern latitude is negative */
        //double* tret, /* return array, 10 doubles, see below */
        //double* attr, /* return array, 20 doubles, see below */
        //AS_BOOL backward, /* TRUE, if backward search */
        //char* serr); /* return error string */
        //double tret[10], attr[20], geopos[10];
        //char serr[255];
        //int32 whicheph = 0; /* default ephemeris */
        //double tjd_start = 2451545; /* Julian day number for 1 Jan 2000 */
        //int32 ifltype = SE_ECL_TOTAL ¦ SE_ECL_CENTRAL ¦ SE_ECL_NONCENTRAL;
        ///* find next eclipse anywhere on Earth */
        //eclflag = swe_sol_eclipse_when_glob(tjd_start, whicheph, ifltype, tret, 0, serr);
        //if (eclflag == ERR)
        //return ERR;
        ///* the time of the greatest eclipse has been returned in tret[0];
        //* now we can find geographical position of the eclipse maximum */
        //tjd_start = tret[0];
        //eclflag = swe_sol_eclipse_where(tjd_start, whicheph, geopos, attr, serr);
        //if (eclflag == ERR)
        //return ERR;
        ///* the geographical position of the eclipse maximum is in geopos[0] and geopos[1];
        //* now we can calculate the four contacts for this place. The start time is chosen
        //* a day before the maximum eclipse: */
        //tjd_start = tret[0] - 1;
        //eclflag = swe_sol_eclipse_when_loc(tjd_start, whicheph, geopos, tret, attr, 0, serr);
        //if (eclflag == ERR)
        //return ERR;
        ///* now tret[] contains the following values:
        //* tret[0] = time of greatest eclipse (Julian day number)
        //* tret[1] = first contact
        //* tret[2] = second contact
        //* tret[3] = third contact
        //* tret[4] = fourth contact */
        [DllImport("swedll32.dll")]
        public static extern int swe_sol_eclipse_when_loc(double tjd_start, int ifl, IntPtr geopos, IntPtr tret, IntPtr attr, bool backward, IntPtr serr);

        //double tjd_start, /* start date for search, Jul. day UT */
        //int32 ifl, /* ephemeris flag */
        //int32 ifltype, /* eclipse type wanted: SE_ECL_TOTAL etc. or 0, if any eclipse type */
        //double* tret, /* return array, 10 doubles, see below */
        //AS_BOOL backward, /* TRUE, if backward search */
        //char* serr); /* return error string */
        [DllImport("swedll32.dll")]
        public static extern int swe_lun_eclipse_when(double tjd_start, int ifl, int ifltype, IntPtr tret, bool backward, IntPtr serr);

        //double tjd_start, /* start date for search, Jul. day UT */
        //int32 ifl, /* ephemeris flag */
        //double* geopos, /* 3 doubles for geogr. longitude, latitude, height above sea.
        //* eastern longitude is positive,
        //* western longitude is negative,
        //* northern latitude is positive,
        //* southern latitude is negative */
        //double* tret, /* return array, 10 doubles, see below */
        //double* attr, /* return array, 20 doubles, see below */
        //AS_BOOL backward, /* TRUE, if backward search */
        //char* serr); /* return error string */
        [DllImport("swedll32.dll")]
        public static extern int swe_lun_eclipse_when_loc(double tjd_start, int ifl, IntPtr geopos, IntPtr tret, IntPtr attr, bool backward, IntPtr serr);

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
