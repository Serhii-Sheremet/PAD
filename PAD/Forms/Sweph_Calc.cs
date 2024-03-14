using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PAD
{
    public partial class Sweph_Calc : Form
    {
        private List<MrityuBhaga> _mbList;

        public Sweph_Calc(List<MrityuBhaga> mbList)
        {
            InitializeComponent();

            _mbList = mbList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EpheCalculation eCalc = new EpheCalculation();
            //DateTime fromDate = new DateTime(2022, 1, 1), toDate = new DateTime(2023, 1, 1);

            //List<PlanetData> planetDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MOON, fromDate, toDate);
            //List<TithiData> tithiDataList = eCalc.CalculateTithiDataList_London(fromDate, toDate);
            //List<NityaYogaData> nyDataList = eCalc.CalculateNityaYogaDataList_London(fromDate, toDate);
            //List<EclipseData> ecDataList = eCalc.CalculateEclipse_London(fromDate, toDate);
            //List<MrityuBhagaData> mbDataList = eCalc.CalculateMrityuBhagaDataList_London(_mbList, EPlanet.KETUMEAN, fromDate, toDate);

            // 05.12.1971 00:40 -3
            /*DateTime calcDate = new DateTime(1971, 12, 4, 21, 40, 0);

            double[] houses = eCalc.AscendanceCalculation(calcDate, 49.506984, 26.764657, 0 , 'O');
            int Znak = Utility.GetZodiakIdFromDegree(houses[0]);
            label1.Text = "House: " + houses[0] + "     " + Znak;*/

            DateTime calcDate = new DateTime(2024, 3, 13, 0, 0, 0);

            double risetime = eCalc.SunRiseCalculation(calcDate, 52.25, 21, 0, 0, 0, EpheConstants.SE_CALC_RISE);
            //double risetime = eCalc.SunRiseCalculation(calcDate, 52.25, 21, 0, 0, 0, EpheConstants.SE_CALC_SET | EpheConstants.SE_BIT_DISC_CENTER | EpheConstants.SE_BIT_NO_REFRACTION); //| EpheConstants.SE_BIT_HINDU_RISING
            DateTime sunriseTime = eCalc.DateTimeFromJulday(risetime);
            label1.Text = "Sunrise: " + risetime + "      " + sunriseTime;
        }

        
    }
}
