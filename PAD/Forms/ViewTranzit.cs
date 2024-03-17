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

        private List<NakshatraDescription> _ndList;
        private List<ZodiakDescription> _zdList;
        private Location _location;

        private List<PlanetData> _pdList;
        private double _ascendant;
        private int _nakshatralagnaId;
        private int _padaLagna;

        private Profile _profile;

        public ViewTranzit()
        {
            InitializeComponent();
        }

        public ViewTranzit(Profile selProfile, ELanguage lanCode)
        {
            InitializeComponent();

            _activeLang = lanCode;
            _curDate = DateTime.Now;

            _ndList = CacheLoad._nakshatraDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
            _zdList = CacheLoad._zodiakDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();

            _ascendant = 0.00;
            _nakshatralagnaId = -1;
            _padaLagna = -1;

            _profile = selProfile;
            _location = CacheLoad._locationList.Where(i => i.Id == _profile.PlaceOfLivingId).FirstOrDefault();
            PrepareDataGridInfo(_activeLang);
        }

        private void ViewTranzit_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);

            toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
            textBoxSeconds.Text = _curDate.Second.ToString();
            textBoxMinutes.Text = _curDate.Minute.ToString();
            textBoxHours.Text = _curDate.Hour.ToString();
            textBoxDay.Text = _curDate.Day.ToString();
            textBoxMonth.Text = _curDate.Month.ToString();
            textBoxYear.Text = _curDate.Year.ToString();
            textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == _location.Id).FirstOrDefault()?.Locality ?? string.Empty;
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
            toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
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

            double latitude, longitude;
            if (double.TryParse(_location.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                double.TryParse(_location.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
            {
                _pdList = Utility.CalculatePlanetsPositionForDate(_curDate, latitude, longitude);
                _ascendant = Utility.CalculateAscendantForDate(_curDate, latitude, longitude, 0, 'O');

                nakshatraId = Utility.GetNakshatraIdFromDegree(_ascendant);
                pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_ascendant));
                lagnaId = Utility.GetZodiakIdFromDegree(_ascendant);

                List<Zodiak> swapZodiakList = Utility.SwappingZodiakList(zList, lagnaId);
                TransitChart.SettingNumberInDom(g, posX, posY, width, height, swapZodiakList);

                //Get list of planets per dom
                List<DomPlanet>[] planetsList = GetGeneralPlanetsListWithAspects(swapZodiakList);

                Font textFont = new Font("Times New Roman", 12, FontStyle.Regular);
                Font aspectFont = new Font("Times New Roman", 12, FontStyle.Regular);
                Size textSize = TextRenderer.MeasureText("СоR", textFont);

                for (int i = 0; i < 12; i++)
                {
                    TransitChart.DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i, _activeLang);
                }
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
                            DomPlanet newDomPlanet = new DomPlanet
                            {
                                PlanetCode = planetsList[planetListCount][planetCount].PlanetCode,
                                Retro = string.Empty,
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
                PrepareGeneralTransitMap();
            }
            else
            {
                UncheckAllAspectBoxes();
                PrepareGeneralTransitMap();
            }
        }

        private void checkBoxMoon_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
        }

        private void checkBoxSun_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
        }

        private void checkBoxVenus_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
        }

        private void checkBoxJupiter_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
        }

        private void checkBoxMercury_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
        }

        private void checkBoxMars_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
        }

        private void checkBoxSaturn_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
        }

        private void checkBoxRahu_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareGeneralTransitMap();
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

        private void PrepareDataGridInfo(ELanguage langCode)
        {
            dataGridViewInfo.AutoGenerateColumns = false;
            dataGridViewInfo.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
            dataGridViewInfo.DefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.DWTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.DWTOOLTIPTEXT));

            DataGridViewColumn column = new DataGridViewColumn();
            column.DataPropertyName = "Planet";
            column.Name = Utility.GetLocalizedText("", langCode);
            column.Width = 30;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Degree";
            column.Name = Utility.GetLocalizedText("Degree", langCode);
            column.Width = 60;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Znak";
            column.Name = Utility.GetLocalizedText("Rasi", langCode);
            column.Width = 90;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Nakshatra";
            column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
            column.Width = 150;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            int lastColWidth = (dataGridViewInfo.Width - 330);
            column = new DataGridViewColumn();
            column.DataPropertyName = "Pada";
            column.Name = Utility.GetLocalizedText("Pada", langCode);
            column.Width = lastColWidth;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfo.Columns.Add(column);

            dataGridViewInfo.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            foreach (DataGridViewColumn col in dataGridViewInfo.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridViewInfo.EnableHeadersVisualStyles = false;
            dataGridViewInfo.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            dataGridViewInfo.ScrollBars = ScrollBars.None;
            dataGridViewInfo.Height = dataGridViewInfo.ColumnHeadersHeight;
        }

        public struct dgvRowObj
        {
            public string Planet { get; set; }
            public string Degree { get; set; }
            public string Zodiak { get; set; }
            public string Nakshatra { get; set; }
            public string Pada { get; set; }
        }

        private void ProfileInfoDataGridViewFillByRow(ELanguage langCode)
        {
            dgvRowObj rowTemp;
            List<dgvRowObj> rowList = new List<dgvRowObj>();
            string degree = string.Empty, zodiak = string.Empty, nakshatra = string.Empty, pada = string.Empty;
            EAppSetting nodesSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            
            degree = Utility.ConvertDecimalToDegree(_ascendant);
            _nakshatralagnaId = Utility.GetNakshatraIdFromDegree(_ascendant);
            _padaLagna = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_ascendant));
            zodiak = GetZodiakNameByIds(_nakshatralagnaId, _padaLagna);
            nakshatra = GetNakshatraNameById(_nakshatralagnaId);
            pada = _padaLagna.ToString();
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

            if (nodesSetting == EAppSetting.NODEMEAN)
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

            if (nodesSetting == EAppSetting.NODETRUE)
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

            dataGridViewInfo.Rows.Clear();
            for (int i = 0; i < rowList.Count; i++)
            {
                string[] row = new string[] {
                        rowList[i].Planet,
                        rowList[i].Degree,
                        rowList[i].Zodiak,
                        rowList[i].Nakshatra,
                        rowList[i].Pada
                };
                dataGridViewInfo.Rows.Add(row);
            }

            int heihgt = dataGridViewInfo.ColumnHeadersHeight;
            for (int i = 0; i < dataGridViewInfo.RowCount; i++)
            {
                int rowHeight = dataGridViewInfo.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                heihgt += rowHeight;
            }
            dataGridViewInfo.Height = heihgt;
            dataGridViewInfo.ClearSelection();
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
            }
        }

        private void textBoxLivingPlace_TextChanged(object sender, EventArgs e)
        {
            PrepareGeneralTransitMap();
            ProfileInfoDataGridViewFillByRow(_activeLang);
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            _curDate = DateTime.Now;
            toolStripTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
            textBoxSeconds.Text = _curDate.Second.ToString();
            textBoxMinutes.Text = _curDate.Minute.ToString();
            textBoxHours.Text = _curDate.Hour.ToString();
            textBoxDay.Text = _curDate.Day.ToString();
            textBoxMonth.Text = _curDate.Month.ToString();
            textBoxYear.Text = _curDate.Year.ToString();
        }

        private void toolStripTextBoxDate_TextChanged(object sender, EventArgs e)
        {
            PrepareGeneralTransitMap();
            ProfileInfoDataGridViewFillByRow(_activeLang);
        }
    }
}
