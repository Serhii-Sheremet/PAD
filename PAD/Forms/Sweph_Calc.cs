using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PAD
{
    public partial class Sweph_Calc : Form
    {
        public Sweph_Calc()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EpheCalculation eCalc = new EpheCalculation();

            double[] calcRes = new double[6];
            double longitude = -0.17, latitude = 51.5, altitude = 0;

            int year = 2021, yearTo = year + 1;
            DateTime curDate = new DateTime(year, 1, 1, 0, 0, 0);

            calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, curDate.AddSeconds(-1), longitude, latitude, altitude);
            int currentZnak = GetCurrentZnak(calcRes[0]);
            int currentNakshatra = GetCurrentNakshatra(calcRes[0]);
            int currentPada = GetCurrentPada(calcRes[0]);
            string currentRetro = string.Empty;
            
            List<PlanetData> planetDataList = new List<PlanetData>();
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            int monthChange = 0, dayChange = 0, hourChange = 0, minuteChange = 0, secondChange = 0;

            while (curDate.Year < yearTo)
            {
                monthChange = CheckChangeOfMonth(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                if (monthChange != curDate.Month)
                {
                    curDate = new DateTime(curDate.Year, (monthChange - 1), curDate.Day, curDate.Hour, curDate.Minute, curDate.Second);
                    dayChange = CheckChangeOfDay(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                    if (dayChange == 0)
                    {
                        curDate = new DateTime(curDate.Year, monthChange, curDate.Day, curDate.Hour, curDate.Minute, curDate.Second);
                        dayChange = CheckChangeOfDay(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                    }
                    else
                    {
                        if (dayChange != curDate.Day)
                        {
                            curDate = new DateTime(curDate.Year, curDate.Month, (dayChange - 1), curDate.Hour, curDate.Minute, curDate.Second);
                            hourChange = CheckChangeOfHour(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                            if (hourChange == 0)
                            {
                                curDate = new DateTime(curDate.Year, curDate.Month, dayChange, curDate.Hour, curDate.Minute, curDate.Second);
                                hourChange = CheckChangeOfHour(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                            }
                            else
                            {
                                if (hourChange != curDate.Hour)
                                {
                                    curDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, (hourChange - 1), curDate.Minute, curDate.Second);
                                    minuteChange = CheckChangeOfMinute(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                                    if (minuteChange == 0)
                                    {
                                        curDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, hourChange, curDate.Minute, curDate.Second);
                                        minuteChange = CheckChangeOfMinute(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                                    }
                                    else
                                    {
                                        if (minuteChange != curDate.Minute)
                                        {
                                            curDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, curDate.Hour, (minuteChange - 1), curDate.Second);
                                            secondChange = CheckChangeOfSecond(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                                            if (secondChange == 0)
                                            {
                                                curDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, curDate.Hour, minuteChange, curDate.Second);
                                                secondChange = CheckChangeOfSecond(eCalc, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, out calcRes);
                                            }
                                            else
                                            {
                                                if (secondChange != curDate.Second)
                                                {
                                                    curDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, curDate.Hour, curDate.Minute, secondChange);

                                                    currentZnak = GetCurrentZnak(calcRes[0]);
                                                    currentNakshatra = GetCurrentNakshatra(calcRes[0]);
                                                    currentPada = GetCurrentPada(calcRes[0]);
                                                    currentRetro = cRetro;

                                                    PlanetData pdTemp = new PlanetData
                                                    {
                                                        Date = curDate,
                                                        Longitude = calcRes[0],
                                                        Latitude = calcRes[1],
                                                        SpeedInLongitude = calcRes[3],
                                                        SpedInLatitude = calcRes[4],
                                                        Retro = cRetro,
                                                        ZodiakId = cZnak,
                                                        NakshatraId = cNakshatra,
                                                        PadaId = cPada
                                                    };

                                                    planetDataList.Add(pdTemp);

                                                    
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                curDate = curDate.AddSeconds(+1);
            }

            label1.Text = "Count in a list: " + planetDataList.Count;
        }

        private int CheckChangeOfMonth(EpheCalculation eCalc, double longitude, double latitude, double altitude, int currentZnak, int currentNakshatra, int currentPada, string currentRetro, DateTime curDate, out double[] calcRes)
        {
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            calcRes = new double[6];
            int monthChange = 0;

            for (DateTime date = curDate; date < curDate.AddYears(+1);)
            {
                calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, date, longitude, latitude, altitude);

                cZnak = GetCurrentZnak(calcRes[0]);
                cNakshatra = GetCurrentNakshatra(calcRes[0]);
                cPada = GetCurrentPada(calcRes[0]);
                cRetro = string.Empty;

                if (cZnak != currentZnak)
                {
                    monthChange = date.Month;
                    break;
                }

                date = date.AddMonths(+1);
            }
            return monthChange;
        }

        private int CheckChangeOfDay(EpheCalculation eCalc, double longitude, double latitude, double altitude, int currentZnak, int currentNakshatra, int currentPada, string currentRetro, DateTime curDate, out double[] calcRes)
        {
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            calcRes = new double[6];
            int dayChange = 0;

            for (DateTime date = curDate; date < curDate.AddMonths(+1);)
            {
                calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, date, longitude, latitude, altitude);

                cZnak = GetCurrentZnak(calcRes[0]);
                cNakshatra = GetCurrentNakshatra(calcRes[0]);
                cPada = GetCurrentPada(calcRes[0]);
                cRetro = string.Empty;

                if (cZnak != currentZnak)
                {
                    dayChange = date.Day;
                    break;
                }

                date = date.AddDays(+1);
            }
            return dayChange;
        }

        private int CheckChangeOfHour(EpheCalculation eCalc, double longitude, double latitude, double altitude, int currentZnak, int currentNakshatra, int currentPada, string currentRetro, DateTime curDate, out double[] calcRes)
        {
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            calcRes = new double[6];
            int hourChange = 0;

            for (DateTime date = curDate; date < curDate.AddDays(+1);)
            {
                calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, date, longitude, latitude, altitude);

                cZnak = GetCurrentZnak(calcRes[0]);
                cNakshatra = GetCurrentNakshatra(calcRes[0]);
                cPada = GetCurrentPada(calcRes[0]);
                cRetro = string.Empty;

                if (cZnak != currentZnak)
                {
                    hourChange = date.Hour;
                    break;
                }

                date = date.AddHours(+1);
            }
            return hourChange;
        }

        private int CheckChangeOfMinute(EpheCalculation eCalc, double longitude, double latitude, double altitude, int currentZnak, int currentNakshatra, int currentPada, string currentRetro, DateTime curDate, out double[] calcRes)
        {
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            calcRes = new double[6];
            int minuteChange = 0;

            for (DateTime date = curDate; date < curDate.AddHours(+1);)
            {
                calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, date, longitude, latitude, altitude);

                cZnak = GetCurrentZnak(calcRes[0]);
                cNakshatra = GetCurrentNakshatra(calcRes[0]);
                cPada = GetCurrentPada(calcRes[0]);
                cRetro = string.Empty;

                if (cZnak != currentZnak)
                {
                    minuteChange = date.Minute;
                    break;
                }

                date = date.AddMinutes(+1);
            }
            return minuteChange;
        }

        private int CheckChangeOfSecond(EpheCalculation eCalc, double longitude, double latitude, double altitude, int currentZnak, int currentNakshatra, int currentPada, string currentRetro, DateTime curDate, out double[] calcRes)
        {
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            calcRes = new double[6];
            int secondChange = 0;

            for (DateTime date = curDate; date < curDate.AddMinutes(+1);)
            {
                calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, date, longitude, latitude, altitude);

                cZnak = GetCurrentZnak(calcRes[0]);
                cNakshatra = GetCurrentNakshatra(calcRes[0]);
                cPada = GetCurrentPada(calcRes[0]);
                cRetro = string.Empty;

                if (cZnak != currentZnak)
                {
                    secondChange = date.Second;
                    break;
                }

                date = date.AddSeconds(+1);
            }
            return secondChange;
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
