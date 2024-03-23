using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class TransitsMap : Form
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

        private EAppSetting _nodesSetting;
        private ELanguage _activeLang;
        private DateTime _curDate;

        private List<NakshatraDescription> _ndList;
        private List<ZodiakDescription> _zdList;
        private List<PlanetDescription> _plDescList;
        private Location _location;

        private List<PlanetData> _pdBirthList;
        private List<PlanetData> _pdList;
        private double _birthAscendant;
        private double _curAscendant;
        private Profile _profile;

        private int _spaceLenght = 5;
        private double _chartHeightCoeficient = 0.72;
        private int _chartWidth = 0;
        private int _chartHeight = 0;

        public TransitsMap()
        {
            InitializeComponent();
        }

        public TransitsMap(Profile selProfile, ELanguage langCode)
        {
            InitializeComponent();
            //this.Shown += new EventHandler(this.TransitsMap_Shown);

            _nodesSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            _activeLang = langCode;
            _curDate = DateTime.Now;

            _ndList = CacheLoad._nakshatraDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
            _zdList = CacheLoad._zodiakDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
            _plDescList = CacheLoad._planetDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();

            
            if (_nodesSetting == EAppSetting.NODEMEAN)
            {
                var planetToRemove = new[] { 10, 11 };
                _plDescList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
            }
            if (_nodesSetting == EAppSetting.NODETRUE)
            {
                var planetToRemove = new[] { 8, 9 };
                _plDescList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
            }

            _profile = selProfile;
            _location = CacheLoad._locationList.Where(i => i.Id == _profile.PlaceOfLivingId).FirstOrDefault();

            PrepareDataGridInfoNatal(_activeLang);
            PrepareDataGridInfoTranzit(_activeLang);

            Location birthLocation = CacheLoad._locationList.Where(i => i.Id == _profile.PlaceOfBirthId).FirstOrDefault();
            double latitude, longitude;
            if (double.TryParse(birthLocation.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                double.TryParse(birthLocation.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
            {
                _pdBirthList = Utility.CalculatePlanetsPositionForDate(_profile.DateOfBirth, latitude, longitude);
                _birthAscendant = Utility.CalculateAscendantForDate(_profile.DateOfBirth, latitude, longitude, 0, 'O');
            }

            foreach (PlanetDescription pd in _plDescList)
            {
                comboBoxRuler.Items.Add(new KeyValueData(pd.Name, pd.PlanetId));
            }
        }

        private void TransitsMap_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);

            labelLagna.Top = toolStripProfileMenu.Height + _spaceLenght;
            labelLagna.Left = _spaceLenght;
            
            int topSpace = labelLagna.Top + labelLagna.Height + _spaceLenght;
            int panelWidth = this.Width - groupBoxSeconds.Width;

            panelTranzits.Width = panelWidth;
            _chartWidth = (panelWidth / 3) - 10;
            _chartHeight = (int)(_chartWidth * _chartHeightCoeficient);
            
            pictureBoxMapLagna.Width = _chartWidth;
            pictureBoxMapLagna.Height = _chartHeight;
            pictureBoxMapLagna.Top = topSpace;
            pictureBoxMapLagna.Left = _spaceLenght;

            pictureBoxMapMoon.Width = _chartWidth;
            pictureBoxMapMoon.Height = _chartHeight;
            pictureBoxMapMoon.Top = topSpace;
            pictureBoxMapMoon.Left = pictureBoxMapLagna.Width + 2 * _spaceLenght;

            labelNatalMoon.Top = toolStripProfileMenu.Height + _spaceLenght;
            labelNatalMoon.Left = pictureBoxMapMoon.Left;

            pictureBoxMap.Width = _chartWidth;
            pictureBoxMap.Height = _chartHeight;
            pictureBoxMap.Top = topSpace;
            pictureBoxMap.Left = pictureBoxMapLagna.Width + pictureBoxMapMoon.Width + 3 * _spaceLenght;

            labelCurrent.Top = toolStripProfileMenu.Height + _spaceLenght;
            labelCurrent.Left = pictureBoxMap.Left;

            int topGridSpace = topSpace + pictureBoxMapLagna.Height + 2 * _spaceLenght;
            labelPeriodRuler.Top = topGridSpace;
            labelPeriodRuler.Left = pictureBoxMapLagna.Left;
            comboBoxRuler.Top = topGridSpace - _spaceLenght;
            comboBoxRuler.Left = labelPeriodRuler.Width + 4;
            comboBoxRuler.SelectedIndex = 0;

            labelNatal.Top = topGridSpace;
            labelNatal.Left = pictureBoxMapMoon.Left;
            
            labelTranzit.Top = topGridSpace;
            labelTranzit.Left = pictureBoxMap.Left;

            pictureBoxPeriodRuler.Width = _chartWidth;
            pictureBoxPeriodRuler.Height = _chartHeight;
            pictureBoxPeriodRuler.Top = topGridSpace + labelPeriodRuler.Height + _spaceLenght;
            pictureBoxPeriodRuler.Left = _spaceLenght;

            dataGridViewInfoNatal.Top = topGridSpace + labelNatal.Height + _spaceLenght;
            dataGridViewInfoNatal.Left = pictureBoxMapMoon.Left;
            dataGridViewInfoTranzit.Top = topGridSpace + labelTranzit.Height + _spaceLenght;
            dataGridViewInfoTranzit.Left = pictureBoxMap.Left;

            ProfileInfoDataGridViewNatalFillByRow(_activeLang);

            int navHeight = _chartHeight - dataGridViewInfoNatal.Height - labelNatalNavamsa.Height - 2 * _spaceLenght;
            int navWidth = (int)(navHeight * 1.5);
            int navTop = topGridSpace + labelNatal.Height + dataGridViewInfoNatal.Height + 2 * _spaceLenght;

            labelNatalNavamsa.Top = navTop;
            labelNatalNavamsa.Left = pictureBoxMapMoon.Left;
            labelTransitNavamsa.Top = navTop;
            labelTransitNavamsa.Left = pictureBoxMap.Left;

            pictureBoxNatalNavamsa.Width = navWidth;
            pictureBoxNatalNavamsa.Height = navHeight;
            pictureBoxNatalNavamsa.Top = navTop + labelNatalNavamsa.Height + _spaceLenght;
            pictureBoxNatalNavamsa.Left = pictureBoxMapMoon.Left;

            pictureBoxTransitNavamsa.Width = navWidth;
            pictureBoxTransitNavamsa.Height = navHeight;
            pictureBoxTransitNavamsa.Top = navTop + labelTransitNavamsa.Height + _spaceLenght;
            pictureBoxTransitNavamsa.Left = pictureBoxMap.Left;

            int groupBoxTop = pictureBoxMap.Top;
            int groupBoxHeight = groupBoxSeconds.Height;
            groupBoxSeconds.Top = groupBoxTop;
            groupBoxMinutes.Top = groupBoxTop + groupBoxHeight + 2;
            groupBoxHours.Top = groupBoxTop + 2 * groupBoxHeight + 2;
            groupBoxDay.Top = groupBoxTop + 3 * groupBoxHeight + 2;
            groupBoxMonth.Top = groupBoxTop + 4 * groupBoxHeight + 2;
            groupBoxYear.Top = groupBoxTop + 5 * groupBoxHeight + 2;

            buttonRefresh.Top = pictureBoxMap.Bottom - buttonRefresh.Height;
            buttonRefresh.Left = groupBoxYear.Left;

            groupBoxAspects.Top = dataGridViewInfoTranzit.Top;
            groupBoxAspects.Left = groupBoxSeconds.Left;

            toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
            textBoxSeconds.Text = _curDate.Second.ToString();
            textBoxMinutes.Text = _curDate.Minute.ToString();
            textBoxHours.Text = _curDate.Hour.ToString();
            textBoxDay.Text = _curDate.Day.ToString();
            textBoxMonth.Text = _curDate.Month.ToString();
            textBoxYear.Text = _curDate.Year.ToString();
            textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == _location.Id).FirstOrDefault()?.Locality ?? string.Empty;

            groupBoxEventInfo.Top = dataGridViewInfoTranzit.Top + dataGridViewInfoTranzit.Height + _spaceLenght;  
            groupBoxEventInfo.Left = groupBoxAspects.Left;
            groupBoxEventInfo.Height = this.Bottom - dataGridViewInfoTranzit.Bottom - _spaceLenght - 8;;
            richTextBoxEventDesc.Height = groupBoxEventInfo.Height - richTextBoxEventDesc.Top - 5;
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

            ConstructNewDate();
        }

        private void ConstructNewDate()
        {
            int year = Convert.ToInt32(textBoxYear.Text);
            int month = Convert.ToInt32(textBoxMonth.Text);
            int day = Convert.ToInt32(textBoxDay.Text);
            int hour = Convert.ToInt32(textBoxHours.Text);
            int min = Convert.ToInt32(textBoxMinutes.Text);
            int sec = Convert.ToInt32(textBoxSeconds.Text);
            _curDate = new DateTime(year, month, day, hour, min, sec);
            toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void PrepareDataGridInfoTranzit(ELanguage langCode)
        {
            dataGridViewInfoTranzit.Width = 380;

            dataGridViewInfoTranzit.AutoGenerateColumns = false;
            dataGridViewInfoTranzit.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
            dataGridViewInfoTranzit.DefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.DWTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.DWTOOLTIPTEXT));

            DataGridViewColumn column = new DataGridViewColumn();
            column.DataPropertyName = "Planet";
            column.Name = Utility.GetLocalizedText("", langCode);
            column.Width = 30;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoTranzit.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Degree";
            column.Name = Utility.GetLocalizedText("Degree", langCode);
            column.Width = 60;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfoTranzit.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Znak";
            column.Name = Utility.GetLocalizedText("Rasi", langCode);
            column.Width = 90;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfoTranzit.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Nakshatra";
            column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
            column.Width = 150;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfoTranzit.Columns.Add(column);

            int lastColWidth = (dataGridViewInfoTranzit.Width - 330);
            column = new DataGridViewColumn();
            column.DataPropertyName = "Pada";
            column.Name = Utility.GetLocalizedText("Pada", langCode);
            column.Width = lastColWidth;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoTranzit.Columns.Add(column);

            dataGridViewInfoTranzit.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewInfoTranzit.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            foreach (DataGridViewColumn col in dataGridViewInfoTranzit.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridViewInfoTranzit.EnableHeadersVisualStyles = false;
            dataGridViewInfoTranzit.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            dataGridViewInfoTranzit.ScrollBars = ScrollBars.None;
            dataGridViewInfoTranzit.Height = dataGridViewInfoTranzit.ColumnHeadersHeight;
        }

        private void PrepareDataGridInfoNatal(ELanguage langCode)
        {
            dataGridViewInfoNatal.Width = 380;

            dataGridViewInfoNatal.AutoGenerateColumns = false;
            dataGridViewInfoNatal.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
            dataGridViewInfoNatal.DefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.DWTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.DWTOOLTIPTEXT));

            DataGridViewColumn column = new DataGridViewColumn();
            column.DataPropertyName = "Planet";
            column.Name = Utility.GetLocalizedText("", langCode);
            column.Width = 30;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoNatal.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Degree";
            column.Name = Utility.GetLocalizedText("Degree", langCode);
            column.Width = 60;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfoNatal.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Znak";
            column.Name = Utility.GetLocalizedText("Rasi", langCode);
            column.Width = 90;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfoNatal.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Nakshatra";
            column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
            column.Width = 150;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfoNatal.Columns.Add(column);

            int lastColWidth = (dataGridViewInfoNatal.Width - 330);
            column = new DataGridViewColumn();
            column.DataPropertyName = "Pada";
            column.Name = Utility.GetLocalizedText("Pada", langCode);
            column.Width = lastColWidth;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoNatal.Columns.Add(column);

            dataGridViewInfoNatal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewInfoNatal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            foreach (DataGridViewColumn col in dataGridViewInfoNatal.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridViewInfoNatal.EnableHeadersVisualStyles = false;
            dataGridViewInfoNatal.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            dataGridViewInfoNatal.ScrollBars = ScrollBars.None;
            dataGridViewInfoNatal.Height = dataGridViewInfoTranzit.ColumnHeadersHeight;
        }

        public struct dgvRowObj
        {
            public string Planet { get; set; }
            public string Degree { get; set; }
            public string Zodiak { get; set; }
            public string Nakshatra { get; set; }
            public string Pada { get; set; }
        }

        private void ProfileInfoDataGridViewNatalFillByRow(ELanguage langCode)
        {
            dgvRowObj rowTemp;
            List<dgvRowObj> rowList = new List<dgvRowObj>();
            string degree = string.Empty, zodiak = string.Empty, nakshatra = string.Empty, pada = string.Empty;

            int nakshatraId = Utility.GetNakshatraIdFromDegree(_birthAscendant);
            int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_birthAscendant));
            degree = Utility.ConvertDecimalToDegree(_birthAscendant);
            nakshatra = GetNakshatraNameById(nakshatraId);
            pada = padaNum.ToString();
            zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedText("Lg", langCode),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);


            int nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().NakshatraId;
            int padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().PadaId);
            double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SUN, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().PadaId);
            pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MOON, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().PadaId);
            pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MARS, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().PadaId);
            pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MERCURY, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().PadaId);
            pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.JUPITER, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().PadaId);
            pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.VENUS, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().PadaId);
            pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SATURN, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (_nodesSetting == EAppSetting.NODEMEAN)
            {
                nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().PadaId);
                pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.RAHUMEAN, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);

                nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().PadaId);
                pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.KETUMEAN, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);
            }

            if (_nodesSetting == EAppSetting.NODETRUE)
            {
                nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().PadaId);
                pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.RAHUTRUE, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);

                nakId = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().PadaId);
                pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.KETUTRUE, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);
            }

            dataGridViewInfoNatal.Rows.Clear();
            for (int i = 0; i < rowList.Count; i++)
            {
                string[] row = new string[] {
                        rowList[i].Planet,
                        rowList[i].Degree,
                        rowList[i].Zodiak,
                        rowList[i].Nakshatra,
                        rowList[i].Pada
                };
                dataGridViewInfoNatal.Rows.Add(row);
            }

            int heihgt = dataGridViewInfoNatal.ColumnHeadersHeight;
            for (int i = 0; i < dataGridViewInfoNatal.RowCount; i++)
            {
                int rowHeight = dataGridViewInfoNatal.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                heihgt += rowHeight;
            }
            dataGridViewInfoNatal.Height = heihgt;
            dataGridViewInfoNatal.ClearSelection();
        }

        private void ProfileInfoDataGridViewTranzitFillByRow(ELanguage langCode)
        {
            dgvRowObj rowTemp;
            List<dgvRowObj> rowList = new List<dgvRowObj>();
            string degree = string.Empty, zodiak = string.Empty, nakshatra = string.Empty, pada = string.Empty;

            int nakshatraId = Utility.GetNakshatraIdFromDegree(_curAscendant);
            int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_curAscendant));
            degree = Utility.ConvertDecimalToDegree(_curAscendant);
            nakshatra = GetNakshatraNameById(nakshatraId);
            pada = padaNum.ToString();
            zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedText("Lg", langCode),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            
            int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().NakshatraId;
            int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().PadaId);
            double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SUN, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().PadaId);
            pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MOON, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().PadaId);
            pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MARS, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().PadaId);
            pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MERCURY, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().PadaId);
            pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.JUPITER, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().PadaId);
            pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.VENUS, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().NakshatraId;
            padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().PadaId);
            pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().Longitude;
            degree = Utility.ConvertDecimalToDegree(pLong);
            zodiak = GetZodiakNameByIds(nakId, padaId);
            nakshatra = GetNakshatraNameById(nakId);
            pada = padaId.ToString();
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SATURN, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (_nodesSetting == EAppSetting.NODEMEAN)
            {
                nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().PadaId);
                pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.RAHUMEAN, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);

                nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().PadaId);
                pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.KETUMEAN, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);
            }

            if (_nodesSetting == EAppSetting.NODETRUE)
            {
                nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().PadaId);
                pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.RAHUTRUE, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);

                nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().NakshatraId;
                padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().PadaId);
                pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.KETUTRUE, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);
            }

            dataGridViewInfoTranzit.Rows.Clear();
            for (int i = 0; i < rowList.Count; i++)
            {
                string[] row = new string[] {
                        rowList[i].Planet,
                        rowList[i].Degree,
                        rowList[i].Zodiak,
                        rowList[i].Nakshatra,
                        rowList[i].Pada
                };
                dataGridViewInfoTranzit.Rows.Add(row);
            }

            int heihgt = dataGridViewInfoTranzit.ColumnHeadersHeight;
            for (int i = 0; i < dataGridViewInfoTranzit.RowCount; i++)
            {
                int rowHeight = dataGridViewInfoTranzit.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                heihgt += rowHeight;
            }
            dataGridViewInfoTranzit.Height = heihgt;
            dataGridViewInfoTranzit.ClearSelection();
        }

        private string GetZodiakNameByIds(int nakshatraId, int pada)
        {
            int id = Utility.GetZodiakIdFromNakshatraIdandPada(nakshatraId, pada);
            return id + ". " + _zdList.Where(i => i.ZodiakId == id).FirstOrDefault().Name;
        }
        private string GetNakshatraNameById(int id)
        {
            return id + ". " + _ndList.Where(i => i.NakshatraId == id).FirstOrDefault().Name;
        }

        private void buttonLivingPlace_Click(object sender, EventArgs e)
        {
            LocationForm lForm = new LocationForm(CacheLoad._locationList.ToList(), _activeLang, false);
            lForm.ShowDialog(this);
            if (lForm.SelectedLocation != null)
            {
                _location = lForm.SelectedLocation;
                textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == _location.Id).FirstOrDefault()?.Locality ?? string.Empty;
                PrepareTransitMapMoon();
                PrepareTransitMapLagna();
                PrepareGeneralTransitMap();
                KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
                PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);
                ProfileInfoDataGridViewTranzitFillByRow(_activeLang);
                PrepareNatalNavamsa();
                PrepareTransitNavamsa();
            }
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            _curDate = DateTime.Now;
            _location = CacheLoad._locationList.Where(i => i.Id == _profile.PlaceOfLivingId).FirstOrDefault();
            textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == _location.Id).FirstOrDefault()?.Locality ?? string.Empty;

            toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
            textBoxSeconds.Text = _curDate.Second.ToString();
            textBoxMinutes.Text = _curDate.Minute.ToString();
            textBoxHours.Text = _curDate.Hour.ToString();
            textBoxDay.Text = _curDate.Day.ToString();
            textBoxMonth.Text = _curDate.Month.ToString();
            textBoxYear.Text = _curDate.Year.ToString();

            textBoxEvent.Text = string.Empty;
            richTextBoxEventDesc.Text = string.Empty;
        }

        private void toolStripTextBoxDate_TextChanged(object sender, EventArgs e)
        {
            //this.Shown += new EventHandler(this.TransitsMap_Shown);
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();
            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);
            ProfileInfoDataGridViewTranzitFillByRow(_activeLang);
            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void PreparePeriodRulerMap(EPlanet planet)
        {
            Bitmap canvas = new Bitmap(pictureBoxPeriodRuler.Width, pictureBoxPeriodRuler.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxPeriodRuler.Width - 1, height = pictureBoxPeriodRuler.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            PrepareRulerTransits(g, width, height, planet);
            pictureBoxPeriodRuler.Image = canvas;
        }

        private void PrepareTransitMapMoon()
        {
            Bitmap canvas = new Bitmap(pictureBoxMapMoon.Width, pictureBoxMapMoon.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxMapMoon.Width - 1, height = pictureBoxMapMoon.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            PrepareNatalMoonTransits(g, width, height);
            pictureBoxMapMoon.Image = canvas;
        }

        private void PrepareNatalMoonTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0, nakshatraId = -1, pada = -1;
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();
            double latitude, longitude;
            if (double.TryParse(_location.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                double.TryParse(_location.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
            {
                _pdList = Utility.CalculatePlanetsPositionForDate(_curDate, latitude, longitude);
                _curAscendant = Utility.CalculateAscendantForDate(_curDate, latitude, longitude, 0, 'O');

                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().Longitude;
                nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                int znak = Utility.GetZodiakIdFromNakshatraIdandPada(nakshatraId, pada);

                List<Zodiak> swapZodiakList = Utility.SwappingZodiakList(zList, znak);
                TransitChart.SettingNumberInDom(g, posX, posY, width, height, swapZodiakList);

                //Get list of planets per dom
                List<DomPlanet>[] planetsList = GetPlanetsListWithAspects(swapZodiakList);

                Font textFont = new Font("Times New Roman", 14, FontStyle.Regular);
                Font aspectFont = new Font("Times New Roman", 14, FontStyle.Regular);
                Size textSize = TextRenderer.MeasureText("СоR", textFont);

                for (int i = 0; i < 12; i++)
                {
                    TransitChart.DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i, _activeLang);
                }
            }
        }

        private void PrepareTransitMapLagna()
        {
            Bitmap canvas = new Bitmap(pictureBoxMapLagna.Width, pictureBoxMapLagna.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxMapLagna.Width - 1, height = pictureBoxMapLagna.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            // drawing content
            PrepareLagnaTransits(g, width, height);
            pictureBoxMapLagna.Image = canvas;
        }

        private void PrepareLagnaTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0, lagnaId = -1, nakshatraId = -1, pada = -1;
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();
            double latitude, longitude;
            if (double.TryParse(_location.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                double.TryParse(_location.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
            {
                _pdList = Utility.CalculatePlanetsPositionForDate(_curDate, latitude, longitude);
                _curAscendant = Utility.CalculateAscendantForDate(_curDate, latitude, longitude, 0, 'O');

                nakshatraId = Utility.GetNakshatraIdFromDegree(_birthAscendant);
                pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_birthAscendant));
                lagnaId = Utility.GetZodiakIdFromDegree(_birthAscendant);

                List<Zodiak> swapZodiakList = Utility.SwappingZodiakList(zList, lagnaId);
                TransitChart.SettingNumberInDom(g, posX, posY, width, height, swapZodiakList);

                //Get list of planets per dom
                List<DomPlanet>[] planetsList = GetPlanetsListWithAspects(swapZodiakList);

                Font textFont = new Font("Times New Roman", 14, FontStyle.Regular);
                Font aspectFont = new Font("Times New Roman", 14, FontStyle.Regular);
                Size textSize = TextRenderer.MeasureText("СоR", textFont);

                for (int i = 0; i < 12; i++)
                {
                    TransitChart.DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i, _activeLang);
                }
            }
        }
        
        private void PrepareGeneralTransitMap()
        {
            Bitmap canvas = new Bitmap(pictureBoxMap.Width, pictureBoxMap.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxMap.Width - 1, height = pictureBoxMap.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            // drawing content
            PrepareGeneralTransits(g, width, height);
            pictureBoxMap.Image = canvas;
        }

        private void PrepareGeneralTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0, lagnaId = -1, nakshatraId = -1, pada = -1;
            // placing dom numbers based on lagna
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();

            nakshatraId = Utility.GetNakshatraIdFromDegree(_curAscendant);
            pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_curAscendant));
            lagnaId = Utility.GetZodiakIdFromDegree(_curAscendant);

            List<Zodiak> swapZodiakList = Utility.SwappingZodiakList(zList, lagnaId);
            TransitChart.SettingNumberInDom(g, posX, posY, width, height, swapZodiakList);

            //Get list of planets per dom
            List<DomPlanet>[] planetsList = GetGeneralPlanetsListWithAspects(swapZodiakList);

            Font textFont = new Font("Times New Roman", 14, FontStyle.Regular);
            Font aspectFont = new Font("Times New Roman", 14, FontStyle.Regular);
            Size textSize = TextRenderer.MeasureText("СоR", textFont);

            for (int i = 0; i < 12; i++)
            {
                TransitChart.DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i, _activeLang);
            }
        }

        private void PrepareRulerTransits(Graphics g, int width, int height, EPlanet planet)
        {
            int posX = 0, posY = 0, nakshatraId = -1, pada = -1;
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();
            double latitude, longitude;
            if (double.TryParse(_location.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                double.TryParse(_location.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
            {
                _pdList = Utility.CalculatePlanetsPositionForDate(_curDate, latitude, longitude);
                _curAscendant = Utility.CalculateAscendantForDate(_curDate, latitude, longitude, 0, 'O');

                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)planet).FirstOrDefault().Longitude;
                nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                int znak = Utility.GetZodiakIdFromNakshatraIdandPada(nakshatraId, pada);

                List<Zodiak> swapZodiakList = Utility.SwappingZodiakList(zList, znak);
                TransitChart.SettingNumberInDom(g, posX, posY, width, height, swapZodiakList);

                //Get list of planets per dom
                List<DomPlanet>[] planetsList = GetPlanetsListWithAspects(swapZodiakList);

                Font textFont = new Font("Times New Roman", 14, FontStyle.Regular);
                Font aspectFont = new Font("Times New Roman", 14, FontStyle.Regular);
                Size textSize = TextRenderer.MeasureText("СоR", textFont);

                for (int i = 0; i < 12; i++)
                {
                    TransitChart.DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i, _activeLang);
                }
            }
        }

        private void PrepareNatalNavamsa()
        {
            Bitmap canvas = new Bitmap(pictureBoxNatalNavamsa.Width, pictureBoxNatalNavamsa.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxNatalNavamsa.Width - 1, height = pictureBoxNatalNavamsa.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            PrepareNatalNavamsaTransits(g, width, height);
            pictureBoxNatalNavamsa.Image = canvas;
        }

        private void PrepareNatalNavamsaTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0, lagnaId = -1, nakshatraId = -1, pada = -1;
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();
            double latitude, longitude;
            if (double.TryParse(_location.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                double.TryParse(_location.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
            {
                _pdList = Utility.CalculatePlanetsPositionForDate(_curDate, latitude, longitude);
                _curAscendant = Utility.CalculateAscendantForDate(_curDate, latitude, longitude, 0, 'O');

                nakshatraId = Utility.GetNakshatraIdFromDegree(_birthAscendant);
                pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_birthAscendant));
                lagnaId = Utility.GetZodiakIdFromDegree(_birthAscendant);
                int navamsa = Utility.GetNavamsaByNakshatraAndPada(nakshatraId, pada);

                List<int> swapNawamshaList = Utility.SwappingNavamsaArray(navamsa);
                TransitChart.SettingNavamsaNumberInDom(g, posX, posY, width, height, swapNawamshaList);

                //Get list of planets per dom
                List<DomPlanet>[] planetsList = GetNatalNavamsaPlanetsListWithAspects(swapNawamshaList);

                Font textFont = new Font("Times New Roman", 8, FontStyle.Regular);
                Font aspectFont = new Font("Times New Roman", 8, FontStyle.Regular);
                Size textSize = TextRenderer.MeasureText("СоR", textFont);

                for (int i = 0; i < 12; i++)
                {
                    TransitChart.DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i, _activeLang);
                }
            }
        }

        private void PrepareTransitNavamsa()
        {
            Bitmap canvas = new Bitmap(pictureBoxTransitNavamsa.Width, pictureBoxTransitNavamsa.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxTransitNavamsa.Width - 1, height = pictureBoxTransitNavamsa.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            PrepareTransitNavamsaTransits(g, width, height);
            pictureBoxTransitNavamsa.Image = canvas;
        }

        private void PrepareTransitNavamsaTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0, lagnaId = -1, nakshatraId = -1, pada = -1;
            // placing dom numbers based on lagna
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();

            nakshatraId = Utility.GetNakshatraIdFromDegree(_curAscendant);
            pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_curAscendant));
            lagnaId = Utility.GetZodiakIdFromDegree(_curAscendant);
            int navamsa = Utility.GetNavamsaByNakshatraAndPada(nakshatraId, pada);

            List<int> swapNawamshaList = Utility.SwappingNavamsaArray(navamsa);
            TransitChart.SettingNavamsaNumberInDom(g, posX, posY, width, height, swapNawamshaList);

            //Get list of planets per dom
            List<DomPlanet>[] planetsList = GetTransitNavamsaPlanetsListWithAspects(swapNawamshaList);

            Font textFont = new Font("Times New Roman", 8, FontStyle.Regular);
            Font aspectFont = new Font("Times New Roman", 8, FontStyle.Regular);
            Size textSize = TextRenderer.MeasureText("СоR", textFont);

            for (int i = 0; i < 12; i++)
            {
                TransitChart.DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i, _activeLang);
            }
        }

        private List<DomPlanet>[] GetGeneralPlanetsListWithAspects(List<Zodiak> zList)
        {
            List<DomPlanet>[] fullList = new List<DomPlanet>[12];

            for (int i = 0; i < zList.Count; i++)
            {
                fullList[i] = TransitChart.GetGeneralPlanetsListByZnak(_pdList, zList[i].Id, (i + 1));
            }

            if (CheckIfAspectsActive())
            {
                fullList = AddAspects(fullList);
            }

            return fullList;
        }

        private bool CheckIfAspectsActive()
        {
            bool active = false;
            if (checkBoxAll.Checked || checkBoxMoon.Checked || checkBoxSun.Checked || checkBoxVenus.Checked || checkBoxJupiter.Checked ||
                checkBoxMercury.Checked || checkBoxMars.Checked || checkBoxSaturn.Checked || checkBoxRahu.Checked)
                active = true;
            return active;
        }

        private List<DomPlanet>[] AddAspects(List<DomPlanet>[] planetsList)
        {
            for (int planetListCount = 0; planetListCount < planetsList.Length; planetListCount++)
            {
                for (int planetCount = 0; planetCount < planetsList[planetListCount].Count; planetCount++)
                {
                    if (!planetsList[planetListCount][planetCount].IsActiveAspect)
                    {
                        List<int> aspectList = GetAspectDomsListByPlanet(planetsList[planetListCount][planetCount].PlanetCode);
                        for (int aspectCount = 0; aspectCount < aspectList.Count; aspectCount++)
                        {
                            if (planetsList[planetListCount][planetCount].TransitType != ETransitType.TRANSITBIRTH && planetsList[planetListCount][planetCount].TransitType != ETransitType.NATALNAVAMSA && planetsList[planetListCount][planetCount].TransitType != ETransitType.TRANSITNAVAMSA)
                            {
                                DomPlanet newDomPlanet = new DomPlanet
                                {
                                    PlanetCode = planetsList[planetListCount][planetCount].PlanetCode,
                                    Longitude = planetsList[planetListCount][planetCount].Longitude,
                                    TransitType = planetsList[planetListCount][planetCount].TransitType,
                                    Retro = string.Empty,
                                    Exaltation = string.Empty,
                                    IsActiveAspect = true,
                                    ColorCode = EColor.GRAY
                                };

                                if (planetListCount + aspectList[aspectCount] - 1 < 12)
                                {
                                    planetsList[planetListCount + aspectList[aspectCount] - 1].Add(newDomPlanet);
                                }
                                else
                                {
                                    planetsList[(planetListCount + aspectList[aspectCount] - 1) - 12].Add(newDomPlanet);
                                }
                            }
                        }
                    }
                }
            }
            return planetsList;
        }

        private List<int> GetAspectDomsListByPlanet(EPlanet planetCode)
        {
            List<int> aspecstList = new List<int>();
            if (checkBoxMoon.Checked && planetCode == EPlanet.MOON)
            {
                aspecstList.Add(7);
            }
            if (checkBoxSun.Checked && planetCode == EPlanet.SUN)
            {
                aspecstList.Add(7);
            }
            if (checkBoxVenus.Checked && planetCode == EPlanet.VENUS)
            {
                aspecstList.Add(7);
            }
            if (checkBoxJupiter.Checked && planetCode == EPlanet.JUPITER)
            {
                aspecstList.Add(5);
                aspecstList.Add(7);
                aspecstList.Add(9);
            }
            if (checkBoxMercury.Checked && planetCode == EPlanet.MERCURY)
            {
                aspecstList.Add(7);
            }
            if (checkBoxMars.Checked && planetCode == EPlanet.MARS)
            {
                aspecstList.Add(4);
                aspecstList.Add(7);
                aspecstList.Add(8);
            }
            if (checkBoxSaturn.Checked && planetCode == EPlanet.SATURN)
            {
                aspecstList.Add(3);
                aspecstList.Add(7);
                aspecstList.Add(10);
            }
            if ((checkBoxRahu.Checked && planetCode == EPlanet.RAHUMEAN) || (checkBoxRahu.Checked && planetCode == EPlanet.RAHUTRUE))
            {
                aspecstList.Add(5);
                aspecstList.Add(7);
                aspecstList.Add(9);
            }
            return aspecstList;
        }

        private List<DomPlanet>[] GetPlanetsListWithAspects(List<Zodiak> zList)
        {
            List<DomPlanet>[] fullList = new List<DomPlanet>[12];

            for (int i = 0; i < zList.Count; i++)
            {
                fullList[i] = TransitChart.GetPlanetsListByZnak(_pdBirthList, _pdList, zList[i].Id, (i + 1));
            }

            if (CheckIfAspectsActive())
            {
                fullList = AddAspects(fullList);
            }

            return fullList;
        }

        private List<DomPlanet>[] GetNatalNavamsaPlanetsListWithAspects(List<int> navamsaList)
        {
            List<DomPlanet>[] fullList = new List<DomPlanet>[12];

            for (int i = 0; i < navamsaList.Count; i++)
            {
                fullList[i] = TransitChart.GetNatalNavamsaPlanetsListByNavamsaZnak(_pdBirthList, navamsaList[i], (i + 1));
            }

            return fullList;
        }

        private List<DomPlanet>[] GetTransitNavamsaPlanetsListWithAspects(List<int> navamsaList)
        {
            List<DomPlanet>[] fullList = new List<DomPlanet>[12];

            for (int i = 0; i < navamsaList.Count; i++)
            {
                fullList[i] = TransitChart.GetNatalNavamsaPlanetsListByNavamsaZnak(_pdList, navamsaList[i], (i + 1));
            }

            return fullList;
        }

        private void CheckAllAspectBoxes()
        {
            checkBoxMoon.Checked = true;
            checkBoxSun.Checked = true;
            checkBoxVenus.Checked = true;
            checkBoxJupiter.Checked = true;
            checkBoxMercury.Checked = true;
            checkBoxMars.Checked = true;
            checkBoxSaturn.Checked = true;
            checkBoxRahu.Checked = true;
        }

        private void UncheckAllAspectBoxes()
        {
            checkBoxMoon.Checked = false;
            checkBoxSun.Checked = false;
            checkBoxVenus.Checked = false;
            checkBoxJupiter.Checked = false;
            checkBoxMercury.Checked = false;
            checkBoxMars.Checked = false;
            checkBoxSaturn.Checked = false;
            checkBoxRahu.Checked = false;
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                CheckAllAspectBoxes();
                PrepareTransitMapMoon();
                PrepareTransitMapLagna();
                PrepareGeneralTransitMap();

                KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
                PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

                PrepareNatalNavamsa();
                PrepareTransitNavamsa();
            }
            else
            {
                UncheckAllAspectBoxes();
                PrepareTransitMapMoon();
                PrepareTransitMapLagna();
                PrepareGeneralTransitMap();

                KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
                PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

                PrepareNatalNavamsa();
                PrepareTransitNavamsa();
            }
        }

        private void checkBoxMoon_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if(CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void checkBoxSun_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void checkBoxVenus_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void checkBoxJupiter_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void checkBoxMercury_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void checkBoxMars_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void checkBoxSaturn_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private void checkBoxRahu_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
            PrepareGeneralTransitMap();

            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);

            PrepareNatalNavamsa();
            PrepareTransitNavamsa();
        }

        private bool CheckIfAllPlanetCheckBoxChecked()
        {
            bool allChecked = false;
            if (checkBoxMoon.Checked && checkBoxSun.Checked && checkBoxVenus.Checked && checkBoxJupiter.Checked &&
                checkBoxMercury.Checked && checkBoxMars.Checked && checkBoxSaturn.Checked && checkBoxRahu.Checked)
                allChecked = true;
            return allChecked;
        }

        private bool CheckIfAllPlanetCheckBoxUnchecked()
        {
            bool allUnchecked = false;
            if (!checkBoxMoon.Checked && !checkBoxSun.Checked && !checkBoxVenus.Checked && !checkBoxJupiter.Checked &&
                !checkBoxMercury.Checked && !checkBoxMars.Checked && !checkBoxSaturn.Checked && !checkBoxRahu.Checked)
                allUnchecked = true;
            return allUnchecked;
        }

        private void comboBoxRuler_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValueData selectedItem = (KeyValueData)comboBoxRuler.SelectedItem;
            PreparePeriodRulerMap((EPlanet)selectedItem.ItemId);
            pictureBoxPeriodRuler.Focus();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            PersonTransitEvents pteForm = new PersonTransitEvents(_profile, _location.Id, _curDate, _activeLang);
            pteForm.ShowDialog(this);
            TransitEvent selectedEvent = pteForm.SelectedTransitEvent;
            if (selectedEvent != null)
            {
                _curDate = selectedEvent.EventDate;
                toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
                _location = CacheLoad._locationList.Where(i => i.Id == selectedEvent.LocationId).FirstOrDefault();
                textBoxLivingPlace.Text = _location.Locality;
                textBoxEvent.Text = selectedEvent.EventName;
                richTextBoxEventDesc.Text = selectedEvent.Description;
            }
        }

        private void toolStripButtonPreview_Click(object sender, EventArgs e)
        {
            PersonTransitEvents pteForm = new PersonTransitEvents(_profile, _activeLang);
            pteForm.ShowDialog(this);
            TransitEvent selectedEvent = pteForm.SelectedTransitEvent;
            if (selectedEvent != null)
            {
                _curDate = selectedEvent.EventDate;
                toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
                _location = CacheLoad._locationList.Where(i => i.Id == selectedEvent.LocationId).FirstOrDefault();
                textBoxLivingPlace.Text = _location.Locality;
                textBoxEvent.Text = selectedEvent.EventName;
                richTextBoxEventDesc.Text = selectedEvent.Description;
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            CurrentDateRefresh();
        }
    }
}
