using System;
using System.Windows.Forms;

namespace PAD
{
    public partial class DBPassword : Form
    {
        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }

        private bool _password;  
        public bool IsPasswordOK
        {
            get { return _password; }
            set { _password = value; }   
        }

        public DBPassword()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            IsPasswordOK = false;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Equals("pad2020"))
            {
                IsPasswordOK = true;
            }
            else
            {
                MessageBox.Show("Введен неверный пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Close();
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonOK_Click(sender, e);
        }
    }
}
