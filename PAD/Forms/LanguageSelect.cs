using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PAD
{
    public partial class LanguageSelect : Form
    {
        private ELanguage _selectedLanguage;
        public ELanguage SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { _selectedLanguage = value; }
        }

        private List<Language> _lanList;

        public LanguageSelect()
        {
            InitializeComponent();
        }

        public LanguageSelect(List<Language> lList)
        {
            InitializeComponent();

            _lanList = lList;
        }

        private void LanguageSelect_Shown(object sender, EventArgs e)
        {
            _lanList.ForEach(i => comboBoxLanguage.Items.Add(i.LanguageCode));
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (!comboBoxLanguage.Text.Equals(string.Empty))
            {
                SelectedLanguage = (ELanguage)(comboBoxLanguage.SelectedIndex + 1);
                Close();
            }
        }
        
    }
}
