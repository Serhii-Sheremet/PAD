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
            DateTime fromDate = new DateTime(2026, 1, 1), toDate = new DateTime(2027, 1, 1);
            //List<PlanetData> planetDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MOON, fromDate, toDate);
            //List<TithiData> tithiDataList = eCalc.CalculateTithiDataList_London(fromDate, toDate);
            //List<NityaYogaData> nyDataList = eCalc.CalculateNityaYogaDataList_London(fromDate, toDate);
            //List<EclipseData> ecDataList = eCalc.CalculateEclipse_London(fromDate, toDate);
            List<MrityuBhagaData> mbDataList = eCalc.CalculateMrityuBhagaDataList_London(_mbList, EPlanet.SATURN, fromDate, toDate);

            label1.Text = "Count: " + mbDataList.Count;
        }

        

        

    }
}
