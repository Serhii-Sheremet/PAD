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
            int planetConstant = EpheConstants.SE_MOON;

            int year = 2021, yearTo = year + 1;
            DateTime curDate = new DateTime(year, 1, 1, 0, 0, 0);

            calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, curDate.AddSeconds(-1), longitude, latitude, altitude);
            int currentZnak = GetCurrentZnak(calcRes[0]);
            int currentNakshatra = GetCurrentNakshatra(calcRes[0]);
            int currentPada = GetCurrentPada(calcRes[0]);
            string currentRetro = string.Empty;
            
            List<PlanetData> planetDataList = new List<PlanetData>();
            DateTime dateChange;

            while (curDate.Year < yearTo)
            {
                TimeSpan tsStep = curDate.AddMonths(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(eCalc, planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep); 
                tsStep = curDate.AddDays(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(eCalc, planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep); 
                tsStep = curDate.AddHours(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(eCalc, planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep); 
                tsStep = curDate.AddMinutes(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(eCalc, planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange.Add(-tsStep); 
                tsStep = curDate.AddSeconds(+1).Subtract(curDate);
                dateChange = CheckChangeInTimePeriod(eCalc, planetConstant, longitude, latitude, altitude, currentZnak, currentNakshatra, currentPada, currentRetro, curDate, tsStep, out calcRes);

                curDate = dateChange;
                currentZnak = GetCurrentZnak(calcRes[0]);
                currentNakshatra = GetCurrentNakshatra(calcRes[0]);
                currentPada = GetCurrentPada(calcRes[0]);
                currentRetro = string.Empty;

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
            label1.Text = "Count in a list: " + planetDataList.Count;
        }

        private DateTime CheckChangeInTimePeriod(EpheCalculation eCalc, int planetConst, double longitude, double latitude, double altitude, int currentZnak, int currentNakshatra, int currentPada, string currentRetro, DateTime curDate, TimeSpan tsStep, out double[] calcRes)
        {
            int cZnak = 0, cNakshatra = 0, cPada = 0;
            string cRetro = string.Empty;
            calcRes = new double[6];

            for (DateTime date = curDate; date < curDate.AddYears(+1);)
            {
                calcRes = eCalc.SWE_Calculation(planetConst, date, longitude, latitude, altitude);

                cZnak = GetCurrentZnak(calcRes[0]);
                cNakshatra = GetCurrentNakshatra(calcRes[0]);
                cPada = GetCurrentPada(calcRes[0]);
                cRetro = string.Empty;
                
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
