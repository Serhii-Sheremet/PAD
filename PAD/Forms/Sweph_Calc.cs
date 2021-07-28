using System;
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

            calcRes = eCalc.SWE_Calculation(EpheConstants.SE_SUN, DateTime.Now, longitude, latitude, altitude);

        }
    }
}
