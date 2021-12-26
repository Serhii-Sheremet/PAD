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
            DateTime fromDate = new DateTime(2021, 1, 1), toDate = new DateTime(2022, 1, 1);
            //List<PlanetData> planetDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MOON, fromDate, toDate);
            //List<TithiData> tithiDataList = eCalc.CalculateTithiDataList_London(fromDate, toDate);
            //List<NityaJogaData> njDataList = eCalc.CalculateNityaJogaDataList_London(fromDate, toDate);
            //List<EclipseData> ecDataList = eCalc.CalculateSolarEclipse_London(fromDate, toDate);
            List<MrityuBhagaData> mbDataList = eCalc.CalculateMrityuBhagaDataList(fromDate, toDate);

            label1.Text = "Count: " + mbDataList.Count;
        }

        

        

    }
}
