using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;


namespace PAD
{
    public partial class About : Form
    {
        private ELanguage _activeLang;

        public About()
        {
            InitializeComponent();
        }

        public About(ELanguage langCode)
        {
            InitializeComponent();
            _activeLang = langCode;
        }

        private void About_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);

            labelName.Left = tabPage1.Width / 2 - labelName.Width / 2;

            labelDescription.MaximumSize = new Size(250, 0);
            labelDescription.AutoSize = true;

            labelConcept.Left = (260 - labelConcept.Width);
            labelProgramming.Left = (260 - labelProgramming.Width);

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            labelVersion.Text = version.ToString();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://vk.com/id263300332");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/halyna.sheremet");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:hsheremet@ukr.net");
        }

        
        
    }
}
