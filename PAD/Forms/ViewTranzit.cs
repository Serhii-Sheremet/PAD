using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;


namespace PAD
{
    public partial class ViewTranzit : Form
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

        private ELanguage _activeLang;
        private DateTime _curDate;

        public ViewTranzit()
        {
            InitializeComponent();
        }

        public ViewTranzit(ELanguage lanCode)
        {
            InitializeComponent();

            _activeLang = lanCode;
            _curDate = DateTime.Now;
        }

        private void ViewTranzit_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);

            textBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
            textBoxSeconds.Text = _curDate.Second.ToString();
            textBoxMinutes.Text = _curDate.Minute.ToString();
            textBoxHours.Text = _curDate.Hour.ToString();
            textBoxDay.Text = _curDate.Day.ToString();
            textBoxMonth.Text = _curDate.Month.ToString();
            textBoxYear.Text = _curDate.Year.ToString();

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxSeconds_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxMinutes_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxSecondsStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxMinutesStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxHoursStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxDayStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxMonthStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBoxYearStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void arrowButtonSecondsLeft_Click(object sender, EventArgs e)
        {
            textBoxSeconds.Text = (Convert.ToInt32(textBoxSeconds.Text) - Convert.ToInt32(textBoxSecondsStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonSecondsRight_Click(object sender, EventArgs e)
        {
            textBoxSeconds.Text = (Convert.ToInt32(textBoxSeconds.Text) + Convert.ToInt32(textBoxSecondsStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonMinutesLeft_Click(object sender, EventArgs e)
        {
            textBoxMinutes.Text = (Convert.ToInt32(textBoxMinutes.Text) - Convert.ToInt32(textBoxMinutesStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonMinutesRight_Click(object sender, EventArgs e)
        {
            textBoxMinutes.Text = (Convert.ToInt32(textBoxMinutes.Text) + Convert.ToInt32(textBoxMinutesStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonHoursLeft_Click(object sender, EventArgs e)
        {
            textBoxHours.Text = (Convert.ToInt32(textBoxHours.Text) - Convert.ToInt32(textBoxHoursStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonHoursRight_Click(object sender, EventArgs e)
        {
            textBoxHours.Text = (Convert.ToInt32(textBoxHours.Text) + Convert.ToInt32(textBoxHoursStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonDayLeft_Click(object sender, EventArgs e)
        {
            textBoxDay.Text = (Convert.ToInt32(textBoxDay.Text) - Convert.ToInt32(textBoxDayStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonDayRight_Click(object sender, EventArgs e)
        {
            textBoxDay.Text = (Convert.ToInt32(textBoxDay.Text) + Convert.ToInt32(textBoxDayStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonMonthLeft_Click(object sender, EventArgs e)
        {
            textBoxMonth.Text = (Convert.ToInt32(textBoxMonth.Text) - Convert.ToInt32(textBoxMonthStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonMonthRight_Click(object sender, EventArgs e)
        {
            textBoxMonth.Text = (Convert.ToInt32(textBoxMonth.Text) + Convert.ToInt32(textBoxMonthStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonYearLeft_Click(object sender, EventArgs e)
        {
            textBoxYear.Text = (Convert.ToInt32(textBoxYear.Text) - Convert.ToInt32(textBoxYearStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void arrowButtonYearRight_Click(object sender, EventArgs e)
        {
            textBoxYear.Text = (Convert.ToInt32(textBoxYear.Text) + Convert.ToInt32(textBoxYearStep.Text)).ToString();
            CurrentDateRefresh();
        }

        private void textBoxSeconds_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxSeconds.Text.Equals(string.Empty))
            {
                int newValue = 0;
                int curValue = Convert.ToInt32(textBoxSeconds.Text);
                if (curValue < 0)
                {
                    newValue = 60 + curValue;
                    textBoxSeconds.Text = newValue.ToString();
                    textBoxMinutes.Text = (Convert.ToInt32(textBoxMinutes.Text) - 1).ToString();
                }
                else if (curValue >= 60)
                {
                    newValue = curValue - 60;
                    textBoxSeconds.Text = newValue.ToString();
                    textBoxMinutes.Text = (Convert.ToInt32(textBoxMinutes.Text) + 1).ToString();
                }
            }
        }

        private void textBoxMinutes_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxMinutes.Text.Equals(string.Empty))
            {
                int newValue = 0;
                int curValue = Convert.ToInt32(textBoxMinutes.Text);
                if (curValue < 0)
                {
                    newValue = 60 + curValue;
                    textBoxMinutes.Text = newValue.ToString();
                    textBoxHours.Text = (Convert.ToInt32(textBoxHours.Text) - 1).ToString();
                }
                else if (curValue >= 60)
                {
                    newValue = curValue - 60;
                    textBoxMinutes.Text = newValue.ToString();
                    textBoxHours.Text = (Convert.ToInt32(textBoxHours.Text) + 1).ToString();
                }
            }
        }

        private void textBoxHours_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxHours.Text.Equals(string.Empty))
            {
                int newValue = 0;
                int curValue = Convert.ToInt32(textBoxHours.Text);
                if (curValue < 0)
                {
                    newValue = 24 + curValue;
                    textBoxHours.Text = newValue.ToString();
                    textBoxDay.Text = (Convert.ToInt32(textBoxDay.Text) - 1).ToString();
                }
                else if (curValue >= 24)
                {
                    newValue = curValue - 24;
                    textBoxHours.Text = newValue.ToString();
                    textBoxDay.Text = (Convert.ToInt32(textBoxDay.Text) + 1).ToString();
                }
            }
        }

        private void textBoxDay_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxDay.Text.Equals(string.Empty))
            {
                int newValue = 0, yearValue = 0, monthValue = 0, daysAmount = 0;
                int curValue = Convert.ToInt32(textBoxDay.Text);

                int daysAmoutCurrent = DateTime.DaysInMonth(_curDate.Year, _curDate.Month);
                if (curValue <= 0)
                {
                    textBoxMonth.Text = (Convert.ToInt32(textBoxMonth.Text) - 1).ToString();
                    yearValue = Convert.ToInt32(textBoxYear.Text);
                    monthValue = Convert.ToInt32(textBoxMonth.Text);
                    daysAmount = DateTime.DaysInMonth(yearValue, monthValue);

                    newValue = daysAmount + curValue;
                    textBoxDay.Text = newValue.ToString();

                }
                else if (curValue > daysAmoutCurrent)
                {
                    textBoxMonth.Text = (Convert.ToInt32(textBoxMonth.Text) + 1).ToString();
                    newValue = curValue - daysAmoutCurrent;
                    textBoxDay.Text = newValue.ToString();
                }
            }
        }

        private void textBoxMonth_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxMonth.Text.Equals(string.Empty))
            {
                int newValue = 0;
                int curValue = Convert.ToInt32(textBoxMonth.Text);
                if (curValue <= 0)
                {
                    newValue = 12 + curValue;
                    textBoxMonth.Text = newValue.ToString();
                    textBoxYear.Text = (Convert.ToInt32(textBoxYear.Text) - 1).ToString();
                }
                else if (curValue > 12)
                {
                    newValue = curValue - 12;
                    textBoxMonth.Text = newValue.ToString();
                    textBoxYear.Text = (Convert.ToInt32(textBoxYear.Text) + 1).ToString();
                }
            }
        }

        private void CurrentDateRefresh()
        {
            if (textBoxSeconds.Text.Equals(string.Empty))
                return;
            if (textBoxMinutes.Text.Equals(string.Empty))
                return;
            if (textBoxHours.Text.Equals(string.Empty))
                return;
            if (textBoxDay.Text.Equals(string.Empty))
                return;
            if (textBoxMonth.Text.Equals(string.Empty))
                return;
            if (textBoxYear.Text.Equals(string.Empty))
                return;

            int year = Convert.ToInt32(textBoxYear.Text);
            int month = Convert.ToInt32(textBoxMonth.Text);
            int day = Convert.ToInt32(textBoxDay.Text);
            int hour = Convert.ToInt32(textBoxHours.Text);
            int min = Convert.ToInt32(textBoxMinutes.Text);
            int sec = Convert.ToInt32(textBoxSeconds.Text);
            _curDate = new DateTime(year, month, day, hour, min, sec);
            textBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
            
        }

    }
}
