using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PAD
{
    public partial class YearTranzitSelect : Form
    {
        private ELanguage _activeLang;
        private List<int> _yList;
        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; }
        }

        public YearTranzitSelect()
        {
            InitializeComponent();
        }

        public YearTranzitSelect(ELanguage aLang)
        {
            InitializeComponent();

            _activeLang = aLang;
            SelectedYear = 0;
            int currYear = DateTime.Now.Year;
            _yList = new List<int>();
            for (int i = currYear - 2; i <= currYear + 10; i++)
            {
                _yList.Add(i);
            }
        }

        private void YearTranzitSelect_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);
            _yList.ForEach(i => comboBoxYear.Items.Add(i));

            int curYear = DateTime.Now.Year;
            comboBoxYear.SelectedIndex = comboBoxYear.Items.IndexOf(curYear);
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (!comboBoxYear.Text.Equals(string.Empty))
            {
                try {

                    int sYear = Convert.ToInt32(comboBoxYear.Text);
                    bool isPresent = false;
                    foreach (int curr in _yList)
                    {
                        if (curr == sYear)
                            isPresent = true;
                    }
                    if(isPresent)
                        SelectedYear = sYear;
                    
                    Close();
                }
                catch {
                    frmShowMessage.Show("Proper year have to be provided", "Error", enumMessageIcon.Error, enumMessageButton.OK);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
