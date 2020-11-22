using System;
using System.Windows.Forms;

namespace PAD
{
    public partial class YearSelect : Form
    {
        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; }
        }

        public YearSelect()
        {
            InitializeComponent();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (!textBoxYear.Text.Equals(string.Empty))
            {
                SelectedYear = Convert.ToInt32(textBoxYear.Text);
                Close();
            }
            else
            {
                MessageBox.Show("Введите год", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
    }
}
