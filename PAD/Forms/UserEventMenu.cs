using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class UserEventMenu : Form
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

        private ELanguage _langCode;
        private Profile _currentProfle;
        private List<string> _timesList;
        private PersonsEventsList _peItem;
        private int _slotsPerHour; // configurable from dayView

        private bool _toCreate = false; // true - create appointment, false - not create appointment
        public bool ToCreate
        {
            get { return _toCreate; }
            set { _toCreate = value; }
        }

        public string EventName
        {
            set { textBoxName.Text = value; }
            get { return textBoxName.Text; }
        }

        private DateTime _evStartDate;
        public DateTime EVStartDate
        {
            set { _evStartDate = value; }
            get { return _evStartDate; }
        }

        private DateTime _evEndDate;
        public DateTime EVEndDate
        {
            set { _evEndDate = value; }
            get { return _evEndDate; }
        }

        public string EventMessage
        {
            set { richTextBoxMessage.Text = value; }
            get { return richTextBoxMessage.Text; }
        }

        public Color controlColor
        {
            get { return pictureBoxColor.BackColor; }
        }
        
        public UserEventMenu()
        {
            InitializeComponent();
        }

        public UserEventMenu(Profile profile, int perHour, ELanguage lCode)
        {
            InitializeComponent();

            _langCode = lCode;
            _currentProfle = profile;
            _slotsPerHour = perHour;
            _peItem = null;

            FillingTimeComboboxes();
            pictureBoxColor.BackColor = Color.LightSalmon;
        }

        public UserEventMenu(Profile profile, PersonsEventsList peItem, int perHour, ELanguage lCode)
        {
            InitializeComponent();

            _langCode = lCode;
            _currentProfle = profile;
            _peItem = peItem;
            _slotsPerHour = perHour;

            FillingTimeComboboxes();
            textBoxName.Text = _peItem.Name;
            richTextBoxMessage.Text = _peItem.Message;
            pictureBoxColor.BackColor = Color.FromArgb(_peItem.ARGBValue);
        }

        private void FillingTimeComboboxes()
        {
            _timesList = new List<string>();
            List<string> hourList;
            for (int i = 0; i < 24; i++)
            {
                hourList = PrepareTimeFramesListforHour(i, _slotsPerHour);
                foreach(string timeStr in hourList)
                    _timesList.Add(timeStr);
            }
            for (int i = 0; i < _timesList.Count; i++)
            {
                comboBoxTimeFrom.Items.Add(_timesList[i]);
                comboBoxTimeTo.Items.Add(_timesList[i]);
            }
        }

        private void UserEventMenu_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _langCode);

            //Configuring DatePickers culture and other settings
            EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
            datePickerFrom.Culture = new CultureInfo(Utility.GetActiveCultureCode(_langCode));
            datePickerTo.Culture = new CultureInfo(Utility.GetActiveCultureCode(_langCode));
            switch (weekSetting)
            {
                case EAppSetting.WEEKSUNDAY:
                    datePickerFrom.FormatProvider.FirstDayOfWeek = DayOfWeek.Sunday;
                    datePickerTo.FormatProvider.FirstDayOfWeek = DayOfWeek.Sunday;
                    break;
                case EAppSetting.WEEKMONDAY:
                    datePickerFrom.FormatProvider.FirstDayOfWeek = DayOfWeek.Monday;
                    datePickerTo.FormatProvider.FirstDayOfWeek = DayOfWeek.Monday;
                    break;
            }
            datePickerFrom.FormatProvider.ShortDatePattern = "dd MMMM yyyy";
            datePickerFrom.PickerDayTextAlignment = ContentAlignment.MiddleCenter;
            datePickerFrom.Value = EVStartDate;
            datePickerTo.FormatProvider.ShortDatePattern = "dd MMMM yyyy";
            datePickerTo.PickerDayTextAlignment = ContentAlignment.MiddleCenter;
            datePickerTo.Value = EVEndDate;

            int cbFromIndex = comboBoxTimeFrom.FindString(EVStartDate.ToString("HH:mm"));
            int cbToIndex = comboBoxTimeTo.FindString(EVEndDate.ToString("HH:mm"));

            if (cbFromIndex != -1)
            {
                comboBoxTimeFrom.SelectedIndex = cbFromIndex;
            }
            else
            {
                comboBoxTimeFrom.SelectedIndex = comboBoxTimeFrom.FindString(FindSettlingTime(EVStartDate, _slotsPerHour).ToString("HH:mm"));
                comboBoxTimeFrom.Text = EVStartDate.ToString("HH:mm");
            }

            if (cbToIndex != -1)
            {
                comboBoxTimeTo.SelectedIndex = cbToIndex;
            }
            else
            {
                comboBoxTimeTo.SelectedIndex = comboBoxTimeTo.FindString(FindSettlingTime(EVEndDate, _slotsPerHour).ToString("HH:mm"));
                comboBoxTimeTo.Text = EVEndDate.ToString("HH:mm");
            }
        }

        private DateTime FindSettlingTime(DateTime originalDate, int slotsPerHour)
        {
            int minutesInSlot = 60 / slotsPerHour;
            int slots = originalDate.Minute / minutesInSlot;
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, (slots * minutesInSlot), 0);
        }

        private List<string> PrepareTimeFramesListforHour(int hour, int perHour)
        {
            List<string> hourList = new List<string>();
            int min = 0;
            int timeFrame = 60 / perHour;
            for (int i = 0; i < perHour; i++)
            {
                string minValue = min.ToString();
                if (min == 0)
                    minValue = "00";
                hourList.Add(hour.ToString() + ":" + minValue);
                min += timeFrame;
            }
            return hourList;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ToCreate = false;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!textBoxName.Text.Equals(string.Empty))
            {
                ToCreate = true;
                if (_peItem == null)
                {
                    InsertEventIntoDatabase();
                }
                else
                {
                    UpdateEventInDatabase();
                }
                this.Close();
            }
            else
            {
                frmShowMessage.Show(Utility.GetLocalizedText("At least title have to be present.", _langCode), Utility.GetLocalizedText("Error", _langCode), enumMessageIcon.Error, enumMessageButton.OK);
            }
        }

        private DateTime GetDateFromDatePickerAndComboBox(DateTime date, string cbText)
        {
            var arr = cbText.Split(new char[] { ':' });
            int hour = Convert.ToInt32(arr[0]);
            int min = Convert.ToInt32(arr[1]);
            return new DateTime(date.Year, date.Month, date.Day, hour, min, 0);
        }

        private void InsertEventIntoDatabase()
        {
            string startDate = GetDateFromDatePickerAndComboBox(datePickerFrom.Value, comboBoxTimeFrom.Text).ToString("yyyy-MM-dd HH:mm:ss");
            string endDate = GetDateFromDatePickerAndComboBox(datePickerTo.Value, comboBoxTimeTo.Text).ToString("yyyy-MM-dd HH:mm:ss");

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("insert into USER_EVENTS (DATESTART, DATEEND, NAME, MESSAGE, ARGBVALUE, GUID) values (@DATESTART, @DATEEND, @NAME, @MESSAGE, @ARGBVALUE, @GUID)", dbCon);
                    command.Parameters.AddWithValue("@DATESTART", startDate);
                    command.Parameters.AddWithValue("@DATEEND", endDate);
                    command.Parameters.AddWithValue("@NAME", textBoxName.Text);
                    command.Parameters.AddWithValue("@MESSAGE", richTextBoxMessage.Text);
                    command.Parameters.AddWithValue("@ARGBVALUE", pictureBoxColor.BackColor.ToArgb());
                    command.Parameters.AddWithValue("@GUID", _currentProfle.GUID);
                    
                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                dbCon.Close();
            }
        }

        private void UpdateEventInDatabase()
        {
            string startDate = GetDateFromDatePickerAndComboBox(datePickerFrom.Value, comboBoxTimeFrom.Text).ToString("yyyy-MM-dd HH:mm:ss");
            string endDate = GetDateFromDatePickerAndComboBox(datePickerTo.Value, comboBoxTimeTo.Text).ToString("yyyy-MM-dd HH:mm:ss");

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update USER_EVENTS set DATESTART = @DATESTART, DATEEND = @DATEEND, NAME = @NAME, MESSAGE = @MESSAGE, ARGBVALUE = @ARGBVALUE where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@DATESTART", startDate);
                    command.Parameters.AddWithValue("@DATEEND", endDate);
                    command.Parameters.AddWithValue("@NAME", textBoxName.Text);
                    command.Parameters.AddWithValue("@MESSAGE", richTextBoxMessage.Text);
                    command.Parameters.AddWithValue("@ARGBVALUE", pictureBoxColor.BackColor.ToArgb());
                    command.Parameters.AddWithValue("@ID", _peItem.Id);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                dbCon.Close();
            }
        }
        
        private void pictureBoxColor_Click(object sender, EventArgs e)
        {
            if (colorDialogMessage.ShowDialog() == DialogResult.Cancel)
                return;

            pictureBoxColor.BackColor = colorDialogMessage.Color;
        }

        
    }
}
