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
            SwephCalculation swCalc = new SwephCalculation();

            double[] calcRes = new double[6];
            double longitude = -0.1257400, latitude = 51.5085300, altitude = 0;

            calcRes = swCalc.SWE_Calculation(EpheConstants.SE_SUN, DateTime.Now, longitude, latitude, altitude);

        }
    }
}
