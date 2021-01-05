using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PopupControl;
using System.IO;
using System.Drawing.Imaging;

namespace PAD
{
    public partial class MainForm : Form
    {
        private Image _closeImage;
        private Image _moonEclipseImage;
        private Image _sunEclipseImage;

        private List<int> _yearsList;
        private int _yearMin = 0;
        private int _yearMax = 0;
        private DateTime _todayDate;
        private DateTime _selectedDate;
        private string[] _daysOfWeek;

        private readonly List<PlanetData> _moonDataList;
        private readonly List<PlanetData> _sunDataList;
        private readonly List<PlanetData> _mercuryDataList;
        private readonly List<PlanetData> _venusDataList;
        private readonly List<PlanetData> _marsDataList;
        private readonly List<PlanetData> _jupiterDataList;
        private readonly List<PlanetData> _saturnDataList;
        private readonly List<PlanetData> _rahuMeanDataList;
        private readonly List<PlanetData> _ketuMeanDataList;
        private readonly List<PlanetData> _rahuTrueDataList;
        private readonly List<PlanetData> _ketuTrueDataList;
        private readonly List<TithiData> _tithiDataList;
        private readonly List<NityaJogaData> _nityaJogaDataList;
        private readonly List<EclipseData> _eclipseDataList;

        //Calendars
        private readonly List<NakshatraCalendar> _nakshatraCalendarList;
        private readonly List<ChandraBalaCalendar> _chandraBalaCalendarList;
        private readonly List<TithiCalendar> _tithiCalendarList;
        private readonly List<KaranaCalendar> _karanaCalendarList;
        private readonly List<NityaJogaCalendar> _nityaJogaCalendarList;
        private readonly List<MasaCalendar> _masaCalendarList;

        private readonly List<PlanetCalendar> _moonZodiakCalendarList;
        private readonly List<PlanetCalendar> _moonZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _moonNakshatraCalendarList;
        private readonly List<PlanetCalendar> _moonPadaCalendarList;

        private readonly List<PlanetCalendar> _sunZodiakCalendarList;
        private readonly List<PlanetCalendar> _sunZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _sunNakshatraCalendarList;
        private readonly List<PlanetCalendar> _sunPadaCalendarList;

        private readonly List<PlanetCalendar> _mercuryZodiakCalendarList;
        private readonly List<PlanetCalendar> _mercuryZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _mercuryNakshatraCalendarList;
        private readonly List<PlanetCalendar> _mercuryPadaCalendarList;

        private readonly List<PlanetCalendar> _venusZodiakCalendarList;
        private readonly List<PlanetCalendar> _venusZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _venusNakshatraCalendarList;
        private readonly List<PlanetCalendar> _venusPadaCalendarList;

        private readonly List<PlanetCalendar> _marsZodiakCalendarList;
        private readonly List<PlanetCalendar> _marsZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _marsNakshatraCalendarList;
        private readonly List<PlanetCalendar> _marsPadaCalendarList;

        private readonly List<PlanetCalendar> _jupiterZodiakCalendarList;
        private readonly List<PlanetCalendar> _jupiterZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _jupiterNakshatraCalendarList;
        private readonly List<PlanetCalendar> _jupiterPadaCalendarList;

        private readonly List<PlanetCalendar> _saturnZodiakCalendarList;
        private readonly List<PlanetCalendar> _saturnZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _saturnNakshatraCalendarList;
        private readonly List<PlanetCalendar> _saturnPadaCalendarList;

        private readonly List<PlanetCalendar> _rahuMeanZodiakCalendarList;
        private readonly List<PlanetCalendar> _rahuMeanZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _rahuMeanNakshatraCalendarList;
        private readonly List<PlanetCalendar> _rahuMeanPadaCalendarList;

        private readonly List<PlanetCalendar> _ketuMeanZodiakCalendarList;
        private readonly List<PlanetCalendar> _ketuMeanZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _ketuMeanNakshatraCalendarList;
        private readonly List<PlanetCalendar> _ketuMeanPadaCalendarList;

        private readonly List<PlanetCalendar> _rahuTrueZodiakCalendarList;
        private readonly List<PlanetCalendar> _rahuTrueZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _rahuTrueNakshatraCalendarList;
        private readonly List<PlanetCalendar> _rahuTruePadaCalendarList;

        private readonly List<PlanetCalendar> _ketuTrueZodiakCalendarList;
        private readonly List<PlanetCalendar> _ketuTrueZodiakRetroCalendarList;
        private readonly List<PlanetCalendar> _ketuTrueNakshatraCalendarList;
        private readonly List<PlanetCalendar> _ketuTruePadaCalendarList;

        private readonly List<EclipseCalendar> _eclipseCalendarList;

        private ELanguage _activeLanguageCode;
        private Profile _selectedProfile;

        private int _daysOfWeekHeight;
        private int _dayWidth;
        private int _dayHeight;

        private int _dayTranzWidth;
        private int _lineTranzHeight;
        private Image _currentCalendarImage;
        private Image _currentTranzitsImage;

        Popup toolTip;
        CalendarToolTip calendarToolTip;
        TranzitsToolTip tranzitsToolTip;
        PersonEventTooltip personEventToolTip;

        private List<Day> _daysList;
        private List<Day> _daysOfMonth;
        private PdfDocument _pdfDoc;

        public DateTime MonthDate
        {
            get { return datePicker.Value; }
            set { datePicker.Value = value; }
        }

        public List<TithiCalendar> TithiCalendarList
        {
            get { return _tithiCalendarList.ToList(); }
        }

        public List<PlanetCalendar> MoonZodiakCalendarList
        {
            get { return _moonZodiakCalendarList.ToList(); }
        }

        public void SetTranzitFocus()
        {
            string tabLabel = Utility.GetLocalizedText("Tranzits", _activeLanguageCode);
            for (int i = 0; i < tabControlCalendar.TabPages.Count; i++)
            {
                if (tabControlCalendar.TabPages[i].Text.Equals(tabLabel))
                {
                    tabControlCalendar.SelectedIndex = i;
                    break;
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            
            this.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
            tabControlCalendar.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlCalendar.DrawItem += tabControlCalendar_DrawItem;
            tabControlCalendar.MouseUp += tabControlCalendar_MouseUp;

            _closeImage = Properties.Resources.close_10;
            _moonEclipseImage = Properties.Resources.Moon_Eclipse_alpha;
            _sunEclipseImage = Properties.Resources.Sun_Eclipse_alpha;

            SetMinMaxYears();
            _todayDate = DateTime.Now;
            _selectedDate = _todayDate;
            CacheEntitiesListsLoad();

            _daysList = null;
            _daysOfMonth = null;

            if (_yearMin != 0 && _yearMax != 0)
            {
                _moonDataList = CacheLoad.GetPlanetData(EPlanet.MOON);
                _sunDataList = CacheLoad.GetPlanetData(EPlanet.SUN);
                _mercuryDataList = CacheLoad.GetPlanetData(EPlanet.MERCURY);
                _venusDataList = CacheLoad.GetPlanetData(EPlanet.VENUS);
                _marsDataList = CacheLoad.GetPlanetData(EPlanet.MARS);
                _jupiterDataList = CacheLoad.GetPlanetData(EPlanet.JUPITER);
                _saturnDataList = CacheLoad.GetPlanetData(EPlanet.SATURN);
                _rahuMeanDataList = CacheLoad.GetPlanetData(EPlanet.RAHUMEAN);
                _ketuMeanDataList = CacheLoad.GetPlanetData(EPlanet.KETUMEAN);
                _rahuTrueDataList = CacheLoad.GetPlanetData(EPlanet.RAHUTRUE);
                _ketuTrueDataList = CacheLoad.GetPlanetData(EPlanet.KETUTRUE);
                _tithiDataList = CacheLoad.GetTithiData();
                _nityaJogaDataList = CacheLoad.GetNityaJogaData();
                _eclipseDataList = CacheLoad.GetEclipseData();

                //prepare static Calendars
                _nakshatraCalendarList = CacheLoad.CreateNakshatraCalendarList(_moonDataList.ToList());
                _tithiCalendarList = CacheLoad.CreateTithiCalendarList(_tithiDataList.ToList());
                _chandraBalaCalendarList = CacheLoad.CreateChandraBalaCalendarList(_moonDataList.ToList());
                _karanaCalendarList = CacheLoad.CreateKaranaCalendarList(_tithiCalendarList.ToList());
                _nityaJogaCalendarList = CacheLoad.CreateNityaJogaCalendarList(_nityaJogaDataList.ToList());
                
                _moonZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MOON, _moonDataList.ToList());
                _moonZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MOON, _moonDataList.ToList());
                _moonNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MOON, _moonDataList.ToList());
                _moonPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MOON, _moonDataList.ToList());

                _sunZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.SUN, _sunDataList.ToList());
                _sunZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.SUN, _sunDataList.ToList());
                _sunNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.SUN, _sunDataList.ToList());
                _sunPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.SUN, _sunDataList.ToList());

                _masaCalendarList = CreateMasaCalendarList(_tithiDataList.ToList());

                _mercuryZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MERCURY, _mercuryDataList.ToList());
                _mercuryZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MERCURY, _mercuryDataList.ToList());
                _mercuryNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MERCURY, _mercuryDataList.ToList());
                _mercuryPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MERCURY, _mercuryDataList.ToList());

                _venusZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.VENUS, _venusDataList.ToList());
                _venusZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.VENUS, _venusDataList.ToList());
                _venusNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.VENUS, _venusDataList.ToList());
                _venusPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.VENUS, _venusDataList.ToList());

                _marsZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MARS, _marsDataList.ToList());
                _marsZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MARS, _marsDataList.ToList());
                _marsNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MARS, _marsDataList.ToList());
                _marsPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MARS, _marsDataList.ToList());

                _jupiterZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.JUPITER, _jupiterDataList.ToList());
                _jupiterZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.JUPITER, _jupiterDataList.ToList());
                _jupiterNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.JUPITER, _jupiterDataList.ToList());
                _jupiterPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.JUPITER, _jupiterDataList.ToList());

                _saturnZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.SATURN, _saturnDataList.ToList());
                _saturnZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.SATURN, _saturnDataList.ToList());
                _saturnNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.SATURN, _saturnDataList.ToList());
                _saturnPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.SATURN, _saturnDataList.ToList());

                _rahuMeanZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.RAHUMEAN, _rahuMeanDataList.ToList());
                _rahuMeanZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.RAHUMEAN, _rahuMeanDataList.ToList());
                _rahuMeanNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.RAHUMEAN, _rahuMeanDataList.ToList());
                _rahuMeanPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.RAHUMEAN, _rahuMeanDataList.ToList());

                _ketuMeanZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.KETUMEAN, _ketuMeanDataList.ToList());
                _ketuMeanZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.KETUMEAN, _ketuMeanDataList.ToList());
                _ketuMeanNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.KETUMEAN, _ketuMeanDataList.ToList());
                _ketuMeanPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.KETUMEAN, _ketuMeanDataList.ToList());

                _rahuTrueZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.RAHUTRUE, _rahuTrueDataList.ToList());
                _rahuTrueZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.RAHUTRUE, _rahuTrueDataList.ToList());
                _rahuTrueNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.RAHUTRUE, _rahuTrueDataList.ToList());
                _rahuTruePadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.RAHUTRUE, _rahuTrueDataList.ToList());

                _ketuTrueZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.KETUTRUE, _ketuTrueDataList.ToList());
                _ketuTrueZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.KETUTRUE, _ketuTrueDataList.ToList());
                _ketuTrueNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.KETUTRUE, _ketuTrueDataList.ToList());
                _ketuTruePadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.KETUTRUE, _ketuTrueDataList.ToList());

                _eclipseCalendarList = CacheLoad.CreateEclipseCalendarList(_eclipseDataList.ToList());
            }

            _activeLanguageCode = (ELanguage)(Utility.GetActiveLanguageCode(CacheLoad._appSettingList));
            _daysOfWeek = PrepareDaysOfWeekArray();

            tabControlCalendar.MakeDoubleBuffered(true);
        }

        private void SetMinMaxYears()
        {
            _yearsList = CacheLoad.GetYearsList();
            if (_yearsList.Count > 0)
            {
                _yearMin = _yearsList.FirstOrDefault();
                _yearMax = _yearsList.LastOrDefault();
            }
        }

        private void CacheEntitiesListsLoad()
        {
            CacheLoad._dvLineNamesList = CacheLoad.GetDVLineNamesList();
            CacheLoad._dvLineNamesDescList = CacheLoad.GetDVLineNamesDescList();
            CacheLoad._languageList = CacheLoad.GetLanguagesList();
            CacheLoad._languageDescList = CacheLoad.GetLanguageDescList();
            CacheLoad._appTextsList = CacheLoad.GetAppTextsList();
            CacheLoad._locationList = CacheLoad.GetLocationsList();
            CacheLoad._appSettingList = CacheLoad.GetAppSettingsList();
            CacheLoad._colorList = CacheLoad.GetColorsList();
            CacheLoad._colorDescList = CacheLoad.GetColorDescList();
            CacheLoad._systemFontList = CacheLoad.GetSystemFontList();
            CacheLoad._fontList = CacheLoad.GetFontList();
            CacheLoad._fontDescList = CacheLoad.GetFontDescList();
            CacheLoad._profileList = CacheLoad.GetProfileList();
            CacheLoad._personEventsList = CacheLoad.GetPersonsEventsList();
            CacheLoad._planetList = CacheLoad.GetPlanetsList();
            CacheLoad._planetDescList = CacheLoad.GetPlanetDescList();
            CacheLoad._zodiakList = CacheLoad.GetZodiaksList();
            CacheLoad._zodiakDescList = CacheLoad.GetZodiakDescList();
            CacheLoad._nakshatraList = CacheLoad.GetNakshatrasList();
            CacheLoad._nakshatraDescList = CacheLoad.GetNakshatraDescList();
            CacheLoad._padaList = CacheLoad.GetPadaList();
            CacheLoad._specNavamshaList = CacheLoad.GetSpecNavamshaList();
            CacheLoad._masaList = CacheLoad.GetShunyaList();
            CacheLoad._masaDescList = CacheLoad.GetMasaDescList();
            CacheLoad._taraBalaList = CacheLoad.GetTaraBalaList();
            CacheLoad._taraBalaDescList = CacheLoad.GetTaraBalaDescList();
            CacheLoad._tithiList = CacheLoad.GetTithiList();
            CacheLoad._tithiDescList = CacheLoad.GetTithiDescList();
            CacheLoad._karanaList = CacheLoad.GetKaranaList();
            CacheLoad._karanaDescList = CacheLoad.GetKaranaDescList();
            CacheLoad._nityaJogaList = CacheLoad.GetNityaJogaList();
            CacheLoad._nityaJogaDescList = CacheLoad.GetNityaJogaDescList();
            CacheLoad._muhurtaList = CacheLoad.GetMuhurtaList();
            CacheLoad._muhurtaDescList = CacheLoad.GetMuhurtaDescList();
            CacheLoad._jogaList = CacheLoad.GetJogaList();
            CacheLoad._jogaDescList = CacheLoad.GetJogaDescList();
            CacheLoad._eclipseList = CacheLoad.GetEclipseList();
            CacheLoad._eclipseDescList = CacheLoad.GetEclipseDescList();
            CacheLoad._tranzitList = CacheLoad.GetTranzitList();
            CacheLoad._tranzitDescList = CacheLoad.GetTranzitDescList();
            CacheLoad._muhurta30List = CacheLoad.GetMuhurta30List();
            CacheLoad._muhurta30DescList = CacheLoad.GetMuhurta30DescList();
            CacheLoad._ghati60List = CacheLoad.GetGhati60List();
            CacheLoad._ghati60DescList = CacheLoad.GetGhati60DescList();
            CacheLoad._horaPlanetList = CacheLoad.MakeHoraPlanetList();
        }

        private List<MasaCalendar> CreateMasaCalendarList(List<TithiData> tdList)
        {
            List<MasaCalendar> mList = new List<MasaCalendar>();
            int index = 0;
            DateTime startDate = new DateTime();
            int firstTithiId = tdList.First().TithiId;
            if (firstTithiId > 1)
            {
                do
                {
                    if (index == 0)
                    {
                        startDate = new DateTime(tdList.First().Date.Year, 1, 1, 0, 0, 0);
                    }
                    if (tdList[index].TithiId == 1)
                    {
                        int masaId = GetMasaIdByDate(tdList[index].Date) - 1;
                        if (masaId == 0)
                            masaId = 12;
                        MasaCalendar tTemp = new MasaCalendar
                        {
                            DateStart = startDate,
                            DateEnd = tdList[index].Date,
                            ColorCode = CacheLoad.GetMasaColorById(masaId),
                            MasaId = masaId
                        };
                        mList.Add(tTemp);
                        startDate = tdList[index].Date;
                    }
                    index++;
                }
                while (index < tdList.Count);
            }
            else
            {
                do
                {
                    if (index == 0)
                    {
                        startDate = tdList[index].Date;
                    }
                    if (tdList[index].TithiId == 1 && index > 0)
                    {
                        int masaId = GetMasaIdByDate(tdList[index].Date);
                        MasaCalendar tTemp = new MasaCalendar
                        {
                            DateStart = startDate,
                            DateEnd = tdList[index].Date,
                            ColorCode = CacheLoad.GetMasaColorById(masaId),
                            MasaId = masaId
                        };
                        mList.Add(tTemp);
                        startDate = tdList[index].Date;
                    }
                    index++;
                }
                while (index < tdList.Count);
            }
            if (tdList.Last().TithiId != 1 && tdList.Last().TithiId != 30)
            {
                int masaId = GetMasaIdByDate(startDate);
                MasaCalendar tTemp = new MasaCalendar
                {
                    DateStart = startDate,
                    DateEnd = new DateTime(tdList.Last().Date.Year, 12, 31, 23, 59, 59),
                    ColorCode = CacheLoad.GetMasaColorById(masaId),
                    MasaId = masaId
                };
                mList.Add(tTemp);
            }
            return mList;
        }

        private int GetMasaIdByDate(DateTime date)
        {
            int zodiakId = (int)(_moonZodiakCalendarList.Where(i => i.DateStart <= date && i.DateEnd >= date).FirstOrDefault().ZodiakCode);
            return CacheLoad._masaList.Where(i => i.ZodiakId == zodiakId).FirstOrDefault()?.Id ?? 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            exitToolStripMenuItem_Click(sender, e);
        }

        private void tabControlCalendar_DrawItem(object sender, DrawItemEventArgs e)
        {
            Image img = new Bitmap(_closeImage);
            Rectangle rect = tabControlCalendar.GetTabRect(e.Index);
            Rectangle imgRect = new Rectangle(rect.Right - (img.Width + 4), rect.Top + (rect.Height - img.Height) / 2, img.Width, img.Height);
            rect.Offset(2, 2);

            string calendarText = Utility.GetLocalizedText("Calendar", _activeLanguageCode);
            string tranzitText = Utility.GetLocalizedText("Tranzits", _activeLanguageCode);

            if (tabControlCalendar.TabPages[e.Index].Text.Equals(calendarText) && tabControlCalendar.TabPages[e.Index].Text.Equals(tranzitText))
            {
                rect.Offset(12, 0);
            }

            if (tabControlCalendar.TabPages[e.Index].Text.Equals(calendarText) || tabControlCalendar.TabPages[e.Index].Text.Equals(tranzitText))
            {
                e.Graphics.DrawString(tabControlCalendar.TabPages[e.Index].Text, e.Font, new SolidBrush(Color.Black), new Point(rect.X + 8, rect.Y));
            }

            if (!tabControlCalendar.TabPages[e.Index].Text.Equals(calendarText) && !tabControlCalendar.TabPages[e.Index].Text.Equals(tranzitText))
            {
                e.Graphics.DrawString(tabControlCalendar.TabPages[e.Index].Text, e.Font, new SolidBrush(Color.Black), new Point(rect.X, rect.Y));
                e.Graphics.DrawImage(img, imgRect);
                e.DrawFocusRectangle();
            }
        }

        private void tabControlCalendar_MouseUp(object sender, MouseEventArgs e)
        {
            Image img = new Bitmap(_closeImage);
            Rectangle rect = tabControlCalendar.GetTabRect(tabControlCalendar.SelectedIndex);
            Rectangle closeButton = new Rectangle(rect.Right - (img.Width + 4), rect.Top + (rect.Height - img.Height) / 2, img.Width, img.Height);

            string calendarText = Utility.GetLocalizedText("Calendar", _activeLanguageCode);
            string tranzitText = Utility.GetLocalizedText("Tranzits", _activeLanguageCode);

            if (!tabControlCalendar.TabPages[tabControlCalendar.SelectedIndex].Text.Equals(calendarText) && !tabControlCalendar.TabPages[tabControlCalendar.SelectedIndex].Text.Equals(tranzitText) && closeButton.Contains(e.Location))
            {
                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to close this page?", _activeLanguageCode), Utility.GetLocalizedText("Confirmation", _activeLanguageCode), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    tabControlCalendar.TabPages.Remove(tabControlCalendar.SelectedTab);
                    if (tabControlCalendar.TabPages.Count > 2)
                    {
                        tabControlCalendar.SelectedIndex = tabControlCalendar.TabPages.Count - 1;
                    }
                    else
                    {
                        tabControlCalendar.SelectedIndex = 0;
                    }
                }
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (Control.ModifierKeys & Keys.Shift) == Keys.Shift && e.KeyCode == Keys.F12)
            {
                int index = -1;
                bool isPresent = false;
                string tabLabel = "DB Browser";
                for (int i = 0; i < tabControlCalendar.TabPages.Count; i++)
                {
                    if (tabControlCalendar.TabPages[i].Text.Equals(tabLabel))
                    {
                        isPresent = true;
                        index = i;
                        break;
                    }
                }

                if (isPresent)
                {
                    tabControlCalendar.SelectedIndex = index;
                }
                else
                {
                    DBPassword dpPass = new DBPassword();
                    dpPass.ShowDialog(this);

                    if (dpPass.IsPasswordOK)
                    {
                        TabPage newTab = new TabPage() { Name = "dbBrowser", Text = tabLabel };
                        DBAdministration tabForm = new DBAdministration();
                        tabForm.TopLevel = false;
                        tabForm.Parent = newTab;
                        tabControlCalendar.TabPages.Add(newTab);
                        tabControlCalendar.SelectedTab = newTab;
                        tabForm.Show();
                        tabForm.Dock = DockStyle.Fill;
                    }
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            tabControlCalendar.Padding = new Point(12);
            Utility.LocalizeForm(this, _activeLanguageCode);

            //Configuring DatePicker culture and other settings
            EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
            datePicker.Culture = new CultureInfo(Utility.GetActiveCultureCode(_activeLanguageCode));
            switch (weekSetting)
            {
                case EAppSetting.WEEKSUNDAY:
                    datePicker.FormatProvider.FirstDayOfWeek = DayOfWeek.Sunday;
                    break;
                case EAppSetting.WEEKMONDAY:
                    datePicker.FormatProvider.FirstDayOfWeek = DayOfWeek.Monday;
                    break;
            }
            datePicker.FormatProvider.ShortDatePattern = "dd MMMM yyyy";
            datePicker.PickerDayTextAlignment = ContentAlignment.MiddleCenter;
            if (_yearMin != 0 && _yearMax != 0)
            {
                datePicker.MinDate = new DateTime(_yearMin, 1, 1);
                datePicker.MaxDate = new DateTime(_yearMax, 12, 31, 23, 59, 59);
            }

            //Check if any profile is selected by default. If yes - creating calendar
            Profile workingProfile = CacheLoad._profileList.Where(i => i.IsChecked).FirstOrDefault();
            if (workingProfile != null && _yearMin != 0 && _yearMax != 0)
            {
                _selectedProfile = workingProfile;
                PrepareProfileAndTimeZoneLabels();
                using (WaitForm wForm = new WaitForm(DrawCalendarData, _activeLanguageCode))
                {
                    wForm.ShowDialog(this);
                }

                
               /*
                _daysList = PrepareMonthDays(new DateTime(_selectedDate.Year, _selectedDate.Month, 1), _selectedProfile);

                //Drawing
                CalendarDrawing(_daysList);
                TranzitDrawing(_daysList);*/
            }
        }

        private string GetTimeZoneInfo(int pLivingId)
        {
            string tInfo = string.Empty;
            string timeZone = string.Empty;
            double latitude, longitude;
            DateTime startDate, endDate;
            TimeZoneInfo.TransitionTime daylightStart = new TimeZoneInfo.TransitionTime(), daylightEnd = new TimeZoneInfo.TransitionTime();
            string cityName = CacheLoad._locationList.Where(i => i.Id == pLivingId).FirstOrDefault()?.Locality ?? string.Empty;

            if (Utility.GetGeoCoordinateByLocationId(pLivingId, out latitude, out longitude))
            {
                timeZone = Utility.GetTimeZoneIdByGeoCoordinates(latitude, longitude);
                TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                TimeZoneInfo.AdjustmentRule[] adjustmentRules = currentTimeZone.GetAdjustmentRules();

                foreach (var adjustmentRule in adjustmentRules)
                {
                    if (_selectedDate >= adjustmentRule.DateStart && _selectedDate <= adjustmentRule.DateEnd)
                    {
                        daylightStart = adjustmentRule.DaylightTransitionStart;
                        daylightEnd = adjustmentRule.DaylightTransitionEnd;
                    }
                }
                if (daylightStart.Day != 0 && daylightStart.Month != 0 && daylightStart.Week != 0)
                {
                    startDate = Utility.GetAdjustmentDate(daylightStart, _selectedDate.Year);
                    endDate = Utility.GetAdjustmentDate(daylightEnd, _selectedDate.Year);
                    DateTime dateOfMonth = new DateTime(datePicker.Value.Year, datePicker.Value.Month, 1);
                    if (dateOfMonth.Month >= startDate.Month && dateOfMonth.Month <= endDate.Month)
                    {
                        tInfo = cityName + ", " + Utility.GetLocalizedText("TimeZone", _activeLanguageCode) + ": " + currentTimeZone.DisplayName.Substring(1, 9) + "; "
                                                + Utility.GetLocalizedText("Summer time from", _activeLanguageCode) + " " + startDate.ToString("dd.MM.yyyy hh:mm") + " " + Utility.GetLocalizedText("till", _activeLanguageCode) + " " + endDate.ToString("dd.MM.yyyy hh:mm");
                    }
                    else
                    {
                        tInfo = cityName + ", " + Utility.GetLocalizedText("TimeZone", _activeLanguageCode) + ": " + currentTimeZone.DisplayName.Substring(1, 9);
                    }
                }
                else
                {
                    tInfo = cityName + ", " + Utility.GetLocalizedText("TimeZone", _activeLanguageCode) + ": " + currentTimeZone.DisplayName.Substring(1, 9);
                }
            }
            return tInfo;
        }

        private void PrepareProfileAndTimeZoneLabels()
        {
            string nakshatraName = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == _selectedProfile.NakshatraMoonId && i.LanguageCode.Equals(_activeLanguageCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
            Font labelTitleFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.HEADER)), 10, Utility.GetFontStyleBySettings(EFontList.HEADER));
            Font labelTZFont = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Regular);

            string labelText = _selectedProfile.ProfileName + ", "
                                + Utility.GetLocalizedText("Moon Nakshatra", _activeLanguageCode) + " "
                                + nakshatraName;

            labelProfile.Visible = true;
            labelProfile.Text = labelText;
            labelProfile.Font = labelTitleFont;
            labelProfile.BackColor = Color.Transparent;
            Size textHSize = TextRenderer.MeasureText(labelProfile.Text, labelTitleFont);
            labelProfile.Left = toolStripMain.Width / 2 - textHSize.Width / 2;

            labelTimeZone.Visible = true;
            labelTimeZone.Text = GetTimeZoneInfo(_selectedProfile.PlaceOfLivingId);
            labelTimeZone.Font = labelTZFont;
            labelTimeZone.BackColor = Color.Transparent;
            Size textTZSize = TextRenderer.MeasureText(labelTimeZone.Text, labelTZFont);
            labelTimeZone.Left = toolStripMain.Width - textTZSize.Width - 2;
            labelTimeZone.Top = toolStripMain.Height / 2 - textTZSize.Height / 2;
        }

        private void datePicker_ValueChanged(object sender, CustomControls.CheckDateEventArgs e)
        {
            if (datePicker.Value.Month == _selectedDate.Month)
                return;

            if (_selectedProfile != null && _yearMin != 0 && _yearMax != 0)
            {
                //Cursor.Current = Cursors.WaitCursor;
                using (WaitForm wForm = new WaitForm(DrawCalendarData, _activeLanguageCode))
                {
                    wForm.ShowDialog(this);
                }
                /*
                // Redrawing calendar for a new selected month
                _selectedDate = datePicker.Value.Date;
                _daysList = null;
                _daysOfMonth = null;
                _daysList = PrepareMonthDays(new DateTime(_selectedDate.Year, _selectedDate.Date.Month, 1), _selectedProfile);

                //Drawing
                CalendarDrawing(_daysList);
                TranzitDrawing(_daysList);

                Cursor.Current = Cursors.Default;*/
            }
        }

        private void DrawCalendarData()
        {
            // Redrawing calendar for a new selected month
            _selectedDate = datePicker.Value.Date;
            _daysList = null;
            _daysOfMonth = null;
            _daysList = PrepareMonthDays(new DateTime(_selectedDate.Year, _selectedDate.Date.Month, 1), _selectedProfile);

            //Drawing
            CalendarDrawing(_daysList);
            TranzitDrawing(_daysList);
        }


        private string FindIfPlanetChangeZnak(Day day)
        {
            string pText = string.Empty;
            EAppSetting nodesSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;

            if (day.SunZodiakRetroDayList.Count > 1)
            {
                for (int i = 1; i < day.SunZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.SunZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.SunZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.VenusZodiakRetroDayList.Count > 1)
            {
                for (int i = 1; i < day.VenusZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.VenusZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.VenusZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.JupiterZodiakRetroDayList.Count > 1)
            {
                for (int i = 1; i < day.JupiterZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.JupiterZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.JupiterZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.MercuryZodiakRetroDayList.Count > 1)
            {
                for (int i = 1; i < day.MercuryZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.MercuryZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.MercuryZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.MarsZodiakRetroDayList.Count > 1)
            {
                for (int i = 1; i < day.MarsZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.MarsZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.MarsZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.SaturnZodiakRetroDayList.Count > 1)
            {
                for (int i = 1; i < day.SaturnZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.SaturnZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.SaturnZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.RahuMeanZodiakRetroDayList.Count > 1 && nodesSetting == EAppSetting.NODEMEAN)
            {
                for (int i = 1; i < day.RahuMeanZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.RahuMeanZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.RahuMeanZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.KetuMeanZodiakRetroDayList.Count > 1 && nodesSetting == EAppSetting.NODEMEAN)
            {
                for (int i = 1; i < day.KetuMeanZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.KetuMeanZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.KetuMeanZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.RahuTrueZodiakRetroDayList.Count > 1 && nodesSetting == EAppSetting.NODETRUE)
            {
                for (int i = 1; i < day.RahuTrueZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.RahuTrueZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.RahuTrueZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            if (day.KetuTrueZodiakRetroDayList.Count > 1 && nodesSetting == EAppSetting.NODETRUE)
            {
                for (int i = 1; i < day.KetuTrueZodiakRetroDayList.Count; i++)
                {
                    PlanetCalendar temp = (PlanetCalendar)day.KetuTrueZodiakRetroDayList[i];
                    PlanetCalendar tempPrev = (PlanetCalendar)day.KetuTrueZodiakRetroDayList[i - 1];
                    EZodiak znak = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)temp.ZodiakCode).FirstOrDefault().Id;
                    EZodiak znakPrev = (EZodiak)CacheLoad._zodiakList.Where(s => s.Id == (int)tempPrev.ZodiakCode).FirstOrDefault().Id;
                    if (znak != znakPrev || (temp.Retro.Equals("R") && !tempPrev.Retro.Equals("R")))
                        pText += PreparePlanetToShow(temp.PlanetCode, temp.Retro, znakPrev, znak) + " ";
                }
            }
            return pText;
        }

        private string PreparePlanetToShow(EPlanet planet, string type, EZodiak prevZnak, EZodiak znak)
        {
            string pName = string.Empty;
            string exaltUp = "↑", exaltDown = "↓";

            if (type.Equals("R") && prevZnak == znak && planet != EPlanet.RAHUMEAN && planet != EPlanet.KETUMEAN && planet != EPlanet.RAHUTRUE && planet != EPlanet.KETUTRUE)
            {
                pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + "." + type;
            }
            else if (type.Equals("R") && prevZnak != znak && planet != EPlanet.RAHUMEAN && planet != EPlanet.KETUMEAN && planet != EPlanet.RAHUTRUE && planet != EPlanet.KETUTRUE)
            {
                pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + "." + type + "→";
            }
            else
            {
                EExaltation exaltation = Utility.GetExaltationByPlanetAndZnak(planet, znak);
                if (exaltation != EExaltation.NOEXALTATION && planet != EPlanet.RAHUMEAN && planet != EPlanet.KETUMEAN && planet != EPlanet.RAHUTRUE && planet != EPlanet.KETUTRUE)
                {
                    if (exaltation == EExaltation.EXALTATION)
                    {
                        pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + exaltUp;
                    }
                    else
                    {
                        pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + exaltDown;
                    }
                }
                else if (planet != EPlanet.RAHUMEAN && planet != EPlanet.KETUMEAN && planet != EPlanet.RAHUTRUE && planet != EPlanet.KETUTRUE)
                {
                    pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + "→";
                }
                if ((planet == EPlanet.RAHUMEAN && planet == EPlanet.KETUMEAN && planet == EPlanet.RAHUTRUE && planet == EPlanet.KETUTRUE) && prevZnak != znak)
                {
                    pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + "→";
                }
            }
            return pName;
        }

        private void DrawJogaColoredLine(Graphics g, Pen pen, int posX, int posY, int width, int height, Day dayObj)
        {
            //Make list of dates for all jogas in day
            List<JogaColoredBlock> jcbList = Utility.GetJogaColoredBlockListForDay(dayObj);
            DrawColoredJogaDay(g, pen, posX, posY, width, height, jcbList);
        }

        private void DrawColoredJogaDay(Graphics g, Pen pen, int posX, int posY, int width, int height, List<JogaColoredBlock> jcbList)
        {
            if (jcbList.Count > 0)
            {
                foreach (JogaColoredBlock jcb in jcbList)
                {
                    if (jcb.DateStart <= jcb.Date.AddDays(+1))
                    {
                        int startPosX = Utility.ConvertHoursToPixels(width, jcb.DateStart);
                        int endPosX = 0;
                        if (jcb.DateEnd <= jcb.Date.AddDays(+1))
                            endPosX = Utility.ConvertHoursToPixels(width, jcb.DateEnd);
                        else
                            endPosX = width + Utility.ConvertHoursToPixels(width, jcb.DateEnd);
                        if (jcb.ColorCode != EColor.NOCOLOR)
                        {
                            Rectangle rect = new Rectangle(posX + startPosX, posY, endPosX - startPosX, height);
                            SolidBrush brush = new SolidBrush(Utility.GetColorByColorCode(jcb.ColorCode));
                            g.FillRectangle(brush, rect);
                        }
                    }
                }
            }
        }

        private void DrawLineName(Graphics g, Pen pen, SolidBrush textBrush, int posY, int labelsWidth, int height, string text)
        {
            string localText = Utility.GetLocalizedText(text, _activeLanguageCode);
            Font textFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular, GraphicsUnit.Point);
            Size textSize = TextRenderer.MeasureText(localText, textFont);
            int posX = labelsWidth - (textSize.Width + 2);
            int heightPadding = (height - textSize.Height) / 2;
            g.DrawString(localText, textFont, textBrush, posX, posY + heightPadding);
        }

        private void TranzitDrawing(List<Day> daysList)
        {
            _daysOfMonth = daysList.Where(i => i.IsDayOfMonth).ToList();
            int daysInMonth = DateTime.DaysInMonth(_daysOfMonth[0].Date.Year, _daysOfMonth[0].Date.Month);

            _dayTranzWidth = (pictureBoxTranzits.Width * 95 / 100) / daysInMonth;
            _lineTranzHeight = (pictureBoxTranzits.Height - 28) / 46;

            int lineWidth = _dayTranzWidth * daysInMonth;
            int labelsWidth = pictureBoxTranzits.Width - _dayTranzWidth * daysInMonth - 1;
            int dayOfMonthHeight = _lineTranzHeight;

            Bitmap canvas = new Bitmap(pictureBoxTranzits.Width, pictureBoxTranzits.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, canvas.Width, canvas.Height));

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            Font dayOfWeekFont = new Font(FontFamily.GenericSansSerif, dayOfMonthHeight - 4, FontStyle.Bold, GraphicsUnit.Pixel);
            Font textFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANZITTEXT)), _lineTranzHeight - 6, Utility.GetFontStyleBySettings(EFontList.TRANZITTEXT), GraphicsUnit.Pixel);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            int posX = labelsWidth, posY = 0, nextPlanetY = 0;
            int posYMasa = posY + 2 * dayOfMonthHeight + 4;
            int posYPanchanga = posYMasa + _lineTranzHeight + 4;
            int posYJoga = posYPanchanga + 6 * _lineTranzHeight + 4;
            int posYTranzits = posYJoga + _lineTranzHeight + 4;

            EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting tranzitSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            foreach (Day d in _daysOfMonth)
            {
                //Draw days and days of week
                posY = 0;
                int day = Utility.GetDayOfWeekNumberFromDate(d.Date, weekSetting);
                DrawDayOfMonth(g, pen, dayOfWeekFont, textBrush, posX, posY, _dayTranzWidth, dayOfMonthHeight, Utility.GetDaysOfWeekName(day, _activeLanguageCode, weekSetting).Substring(0, 3), d.Date, _todayDate);
                DrawDayOfMonth(g, pen, dayOfWeekFont, textBrush, posX, posY + dayOfMonthHeight, _dayTranzWidth, dayOfMonthHeight, d.Date.Day.ToString(), d.Date, _todayDate);

                // Draw masa
                posY = posYMasa;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, _lineTranzHeight, "Masa");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MasaDayList);

                // Draw panchanga
                posY = posYPanchanga;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, _lineTranzHeight, "Nakshatra");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.NakshatraDayList);

                DrawLineName(g, pen, textBrush, posY + _lineTranzHeight, labelsWidth, _lineTranzHeight, "Tara Bala");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.TaraBalaDayList);

                DrawLineName(g, pen, textBrush, posY + 2 * _lineTranzHeight, labelsWidth, _lineTranzHeight, "Tithi");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.TithiDayList);

                DrawLineName(g, pen, textBrush, posY + 3 * _lineTranzHeight, labelsWidth, _lineTranzHeight, "Karana");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KaranaDayList);

                DrawLineName(g, pen, textBrush, posY + 4 * _lineTranzHeight, labelsWidth, _lineTranzHeight, "Nitya Yoga");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 4 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.NityaJogaDayList);

                DrawLineName(g, pen, textBrush, posY + 5 * _lineTranzHeight, labelsWidth, _lineTranzHeight, "Chandra Bala");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 5 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.ChandraBalaDayList);

                //Draw Jogas line
                posY = posYJoga;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, _lineTranzHeight, "VTN Yogi");
                DrawJogaColoredLine(g, pen, posX, posY, _dayTranzWidth, _lineTranzHeight, d);

                //Draw planets tranzits
                //Drawing Moon
                nextPlanetY = posYTranzits;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Moon");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MoonZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MoonZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.MoonZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.MoonZodiakRetroLagnaDayList);
                        break;
                }

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MoonNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MoonPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MoonTaraBalaDayList);

                //Drawing Sun
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Sun");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SunZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SunZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.SunZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.SunZodiakRetroLagnaDayList);
                        break;
                }
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SunNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SunPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SunTaraBalaDayList);

                //Drawing Venus
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Venus");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.VenusZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.VenusZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.VenusZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.VenusZodiakRetroLagnaDayList);
                        break;
                }
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.VenusNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.VenusPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.VenusTaraBalaDayList);

                //Drawing Jupiter
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Jupiter");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.JupiterZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.JupiterZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.JupiterZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.JupiterZodiakRetroLagnaDayList);
                        break;
                }
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.JupiterNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.JupiterPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.JupiterTaraBalaDayList);

                //Drawing Mercury
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Mercury");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MercuryZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MercuryZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.MercuryZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.MercuryZodiakRetroLagnaDayList);
                        break;
                }
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MercuryNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MercuryPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MercuryTaraBalaDayList);

                //Drawing Mars
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Mars");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MarsZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MarsZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.MarsZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.MarsZodiakRetroLagnaDayList);
                        break;
                }
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MarsNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MarsPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MarsTaraBalaDayList);

                //Drawing Saturn
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Saturn");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SaturnZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SaturnZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.SaturnZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.SaturnZodiakRetroLagnaDayList);
                        break;
                }
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SaturnNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SaturnPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SaturnTaraBalaDayList);

                //Drawing Rahu
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Rahu");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.RahuMeanZodiakRetroDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.RahuTrueZodiakRetroDayList);
                        }
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.RahuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.RahuTrueZodiakRetroLagnaDayList);
                        }
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.RahuMeanZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.RahuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.RahuTrueZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.RahuTrueZodiakRetroLagnaDayList);
                        }
                        break;
                }
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuMeanNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuMeanPadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuMeanTaraBalaDayList);
                }
                else
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuTrueNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuTruePadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuTrueTaraBalaDayList);
                }

                //Drawing Ketu
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * _lineTranzHeight), "Ketu");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.KetuMeanZodiakRetroDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.KetuTrueZodiakRetroDayList);
                        }
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.KetuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.KetuTrueZodiakRetroLagnaDayList);
                        }
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        int height = _lineTranzHeight / 2;
                        int posYHalf = posY + height;
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.KetuMeanZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.KetuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.KetuTrueZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.KetuTrueZodiakRetroLagnaDayList);
                        }
                        break;
                }
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuMeanNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuMeanPadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuMeanTaraBalaDayList);
                }
                else
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuTrueNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuTruePadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuTrueTaraBalaDayList);
                }


                posX = posX + _dayTranzWidth;
            }

            // Setting masa changes
            posX = labelsWidth;
            posY = posYMasa;
            foreach (Day c in _daysOfMonth)
            {
                SetMasaName(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.MasaDayList, c.Date);
                posX = posX + _dayTranzWidth;
            }
            // Setting masa current start 
            posX = labelsWidth;
            posY = posYMasa;
            SetMasaStartName(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MasaDayList);

            // Setting panchanga numbers
            posX = labelsWidth;
            posY = posYPanchanga;
            foreach (Day c in _daysOfMonth)
            {
                SetPanchangaNumbers(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.NakshatraDayList, c.Date);
                SetPanchangaNumbers(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.TaraBalaDayList, c.Date);
                SetPanchangaNumbers(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.TithiDayList, c.Date);
                SetPanchangaNumbers(g, pen, textFont, textBrush, posX, posY + 4 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.NityaJogaDayList, c.Date);
                SetPanchangaNumbers(g, pen, textFont, textBrush, posX, posY + 5 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.ChandraBalaDayList, c.Date);
                
                posX = posX + _dayTranzWidth;
            }

            // Setting current start zodiak 
            posX = labelsWidth;
            
            // Moon
            nextPlanetY = posYTranzits;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MoonZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MoonNakshatraDayList, true);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MoonPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MoonTaraBalaDayList, true);

            // Sun
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SunZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SunNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SunPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SunTaraBalaDayList, false);

            // Venus
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().VenusZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().VenusNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().VenusPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().VenusTaraBalaDayList, false);

            // Jupiter
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().JupiterZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().JupiterNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().JupiterPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().JupiterTaraBalaDayList, false);

            // Mercury
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MercuryZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MercuryNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MercuryPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MercuryTaraBalaDayList, false);

            // Mars
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MarsZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MarsNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MarsPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MarsTaraBalaDayList, false);

            // Saturn
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SaturnZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SaturnNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SaturnPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().SaturnTaraBalaDayList, false);

            // Rahu
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            if (nodeSettings == EAppSetting.NODEMEAN)
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuMeanZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuMeanNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuMeanPadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuMeanTaraBalaDayList, false);
            }
            else
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuTrueZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuTrueNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuTruePadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().RahuTrueTaraBalaDayList, false);
            }

            // Ketu
            nextPlanetY += 4 * _lineTranzHeight + 4;
            posY = nextPlanetY;
            if (nodeSettings == EAppSetting.NODEMEAN)
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuMeanZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuMeanNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuMeanPadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuMeanTaraBalaDayList, false);
            }
            else
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuTrueZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuTrueNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuTruePadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().KetuTrueTaraBalaDayList, false);
            }


            // Setting zodiak changes
            posX = labelsWidth;
            foreach (Day c in _daysOfMonth)
            {
                // Moon
                nextPlanetY = posYTranzits;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.MoonZodiakRetroDayList, c.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MoonNakshatraDayList, c.Date, true);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MoonPadaDayList, c.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MoonTaraBalaDayList, c.Date, true);

                // Sun
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.SunZodiakRetroDayList, c.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.SunNakshatraDayList, c.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.SunPadaDayList, c.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.SunTaraBalaDayList, c.Date, false);

                // Venus
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.VenusZodiakRetroDayList, c.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.VenusNakshatraDayList, c.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.VenusPadaDayList, c.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.VenusTaraBalaDayList, c.Date, false);

                // Jupiter
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.JupiterZodiakRetroDayList, c.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.JupiterNakshatraDayList, c.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.JupiterPadaDayList, c.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.JupiterTaraBalaDayList, c.Date, false);

                // Mercury
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.MercuryZodiakRetroDayList, c.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MercuryNakshatraDayList, c.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MercuryPadaDayList, c.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MercuryTaraBalaDayList, c.Date, false);

                // Mars
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.MarsZodiakRetroDayList, c.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MarsNakshatraDayList, c.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MarsPadaDayList, c.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.MarsTaraBalaDayList, c.Date, false);

                // Saturn
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.SaturnZodiakRetroDayList, c.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.SaturnNakshatraDayList, c.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.SaturnPadaDayList, c.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.SaturnTaraBalaDayList, c.Date, false);

                // Rahu
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.RahuMeanZodiakRetroDayList, c.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.RahuMeanNakshatraDayList, c.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.RahuMeanPadaDayList, c.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.RahuMeanTaraBalaDayList, c.Date, false);
                }
                else
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.RahuTrueZodiakRetroDayList, c.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.RahuTrueNakshatraDayList, c.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.RahuTruePadaDayList, c.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.RahuTrueTaraBalaDayList, c.Date, false);
                }
                
                // Ketu
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.KetuMeanZodiakRetroDayList, c.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.KetuMeanNakshatraDayList, c.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.KetuMeanPadaDayList, c.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.KetuMeanTaraBalaDayList, c.Date, false);
                }
                else
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.KetuTrueZodiakRetroDayList, c.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.KetuTrueNakshatraDayList, c.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.KetuTruePadaDayList, c.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, c.KetuTrueTaraBalaDayList, c.Date, false);
                }

                posX = posX + _dayTranzWidth;
            }

            // Drawing Masa rectangles
            posX = labelsWidth;
            posY = posYMasa;
            DrawLineRectangle(g, pen, posX, posY, lineWidth, _lineTranzHeight);

            // Drawing Panchanga rectangles
            posX = labelsWidth;
            posY = posYPanchanga;
            for (int i = 0; i < 6; i++)
            {
                DrawLineRectangle(g, pen, posX, posY + i * _lineTranzHeight, lineWidth, _lineTranzHeight);
            }

            // Drawing Jogas rectangle
            posY = posYJoga;
            DrawLineRectangle(g, pen, posX, posY, lineWidth, _lineTranzHeight);

            //Drawing Tranzits rectangles
            nextPlanetY = posYTranzits;
            posY = nextPlanetY;
            for (int i = 0; i < 9; i++)
            {
                DrawPlanetBlockRectangles(g, pen, posX, posY, lineWidth, _lineTranzHeight);
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
            }

            pictureBoxTranzits.Image = canvas;
            _currentTranzitsImage = pictureBoxTranzits.Image;
        }

        private void SetMasaStartName(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetFullName(_activeLanguageCode);
                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width <= endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
            }
        }

        private void SetLineStartZodiak(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetNumber(_activeLanguageCode);
                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width <= endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
            }
        }

        private void SetLineStartNakshatra(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> c, bool isMoon)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetTranzitNakshatra(_activeLanguageCode);
                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width < endPosX && !isMoon)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                    else
                    {
                        var newText = text.Split('.');
                        textSize = TextRenderer.MeasureText(newText[0], font);
                        if (textSize.Width <= endPosX)
                        {
                            g.DrawString(newText[0], font, textBrush, posX + 1, posY + heightPadding);
                        }
                    }
                }
            }
        }

        private void SetLineStartPada(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                List<PlanetCalendar> pList = Utility.ClonePlanetCalendarList(c);
                string text = pList.First().GetTranzitPada();
                if (pList.First().PlanetCode == EPlanet.MOON)
                    text = text.Substring(0, 1);
                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width < endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
            }
        }

        private void SetLineStartTaraBala(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> c, bool isMoon)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetTranzitTaraBala(_activeLanguageCode);
                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width < endPosX && !isMoon)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                    else
                    {
                        var newText = text.Split('.');
                        textSize = TextRenderer.MeasureText(newText[0], font);
                        if (textSize.Width <= endPosX)
                        {
                            g.DrawString(newText[0], font, textBrush, posX + 1, posY + heightPadding);
                        }
                    }
                }
            }
        }

        private void SetPlanetZodiak(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList, DateTime date)
        {
            if (cList.Count > 0)
            {
                List<PlanetCalendar> pcList = new List<PlanetCalendar>();
                cList.ForEach(i => pcList.Add((PlanetCalendar)i));

                EZodiak previousZodiak = pcList.First().ZodiakCode;
                foreach (PlanetCalendar pc in pcList)
                {
                    if (pc.DateStart > date)
                    {
                        int startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                        string text = pc.GetNumber(_activeLanguageCode);
                        Size textSize = TextRenderer.MeasureText(text, font);
                        int heightPadding = (height - textSize.Height) / 2;
                        if (pc.PlanetCode != EPlanet.RAHUMEAN && pc.PlanetCode != EPlanet.KETUMEAN && pc.PlanetCode != EPlanet.RAHUTRUE && pc.PlanetCode != EPlanet.KETUTRUE)
                        {
                            g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                            g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                        }
                        else
                        {
                            if (pc.ZodiakCode != previousZodiak)
                            {
                                g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                                g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            }
                        }
                    }
                    previousZodiak = pc.ZodiakCode;
                }
            }
        }

        private void SetPlanetNakshatra(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList, DateTime date, bool isMoon)
        {
            if (cList.Count > 0)
            {
                List<PlanetCalendar> pcList = new List<PlanetCalendar>();
                cList.ForEach(i => pcList.Add((PlanetCalendar)i));

                ENakshatra previousNakshatra = pcList.First().NakshatraCode;
                foreach (PlanetCalendar pc in pcList)
                {
                    if (pc.DateStart > date)
                    {
                        int startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                        string text = pc.GetTranzitNakshatra(_activeLanguageCode); 
                        Size textSize = TextRenderer.MeasureText(text, font);
                        int heightPadding = (height - textSize.Height) / 2;
                        if (pc.NakshatraCode != previousNakshatra)
                        {
                            if(!isMoon)
                            {
                                g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                                g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            }
                            else
                            {
                                var newText = text.Split('.');
                                textSize = TextRenderer.MeasureText(newText[0], font);
                                g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                                g.DrawString(newText[0], font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            }
                        }
                    }
                    previousNakshatra = pc.NakshatraCode;
                }
            }
        }

        private void SetPlanetPada(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList, DateTime date)
        {
            if (cList.Count > 0)
            {
                List<PlanetCalendar> pcList = new List<PlanetCalendar>();
                cList.ForEach(i => pcList.Add((PlanetCalendar)i));

                int previousPada = CacheLoad._padaList.Where(i => i.Id == pcList.First().PadaId).FirstOrDefault().PadaNumber;
                foreach (PlanetCalendar pc in pcList)
                {
                    if (pc.DateStart > date)
                    {
                        int currentPada = CacheLoad._padaList.Where(i => i.Id == pc.PadaId).FirstOrDefault().PadaNumber;
                        if (currentPada != previousPada)
                        {
                            int startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                            string text = pc.GetTranzitPada();
                            if (pc.PlanetCode == EPlanet.MOON)
                                text = text.Substring(0, 1);
                            Size textSize = TextRenderer.MeasureText(text, font);
                            int heightPadding = (height - textSize.Height) / 2;
                            g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                            g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            previousPada = currentPada;
                        }
                    }
                }
            }
        }

        private void SetPlanetTaraBala(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList, DateTime date, bool isMoon)
        {
            if (cList.Count > 0)
            {
                List<PlanetCalendar> pcList = new List<PlanetCalendar>();
                cList.ForEach(i => pcList.Add((PlanetCalendar)i));

                int previousTaraBala = pcList.First().TaraBalaId;
                foreach (PlanetCalendar pc in pcList)
                {
                    if (pc.DateStart > date)
                    {
                        int startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                        string text = pc.GetTranzitTaraBala(_activeLanguageCode);
                        Size textSize = TextRenderer.MeasureText(text, font);
                        int heightPadding = (height - textSize.Height) / 2;
                        if (pc.TaraBalaId != previousTaraBala)
                        {
                            if (!isMoon)
                            {
                                g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                                g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            }
                            else
                            {
                                var newText = text.Split('.');
                                textSize = TextRenderer.MeasureText(newText[0], font);
                                g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                                g.DrawString(newText[0], font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            }
                        }
                    }
                    previousTaraBala = pc.TaraBalaId;
                }
            }
        }

        private void SetPanchangaNumbers(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList, DateTime date)
        {
            foreach (Calendar c in cList)
            {
                if (c.DateStart > date)
                {
                    int startPosX = Utility.ConvertHoursToPixels(width, c.DateStart);
                    string text = c.GetNumber(_activeLanguageCode);
                    Size textSize = TextRenderer.MeasureText(text, font);
                    int heightPadding = (height - textSize.Height) / 2;
                    g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                }
            }
        }

        private void SetMasaName(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList, DateTime date)
        {
            foreach (Calendar c in cList)
            {
                if (c.DateStart > date)
                {
                    int startPosX = Utility.ConvertHoursToPixels(width, c.DateStart);
                    string text = c.GetFullName(_activeLanguageCode);
                    Size textSize = TextRenderer.MeasureText(text, font);
                    int heightPadding = (height - textSize.Height) / 2;
                    g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                    g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                }
            }
        }

        private void DrawPlanetBlockRectangles(Graphics g, Pen pen, int posX, int posY, int width, int height)
        {
            for (int i = 0; i < 4; i++)
            {
                DrawLineRectangle(g, pen, posX, posY + i * _lineTranzHeight, width, height);
            }
        }

        private void DrawLineRectangle(Graphics g, Pen pen, int posX, int posY, int width, int height)
        {
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.DrawRectangle(pen, rect);
        }

        private void DrawTranzitColoredLine(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList)
        {
            int drawWidth = 0, usedWidth = 0;
            foreach (Calendar c in cList)
            {
                if (c == cList.Last())
                {
                    drawWidth = width - usedWidth;
                    DrawTranzitColoredRectangle(g, pen, font, textBrush, (posX + usedWidth), posY, drawWidth, height, c);
                }
                else
                {
                    drawWidth = Utility.ConvertHoursToPixels(width, c.DateEnd) - usedWidth;
                    DrawTranzitColoredRectangle(g, pen, font, textBrush, (posX + usedWidth), posY, drawWidth, height, c);
                    usedWidth = usedWidth + drawWidth;
                }
            }
        }

        private void DrawTranzitColoredRectangle(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, Calendar cObj)
        {
            SolidBrush brush = new SolidBrush(Color.White);
            Rectangle rect = new Rectangle(posX, posY, width, height);

            brush = new SolidBrush(Utility.GetColorByColorCode(cObj.ColorCode));
            g.FillRectangle(brush, rect);
        }

        private void DrawDayOfMonth(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, string text, DateTime currentDate, DateTime todayDate)
        {
            // Drawing blue rectangle for today
            if (currentDate == todayDate.Date)
            {
                Rectangle rectT = new Rectangle(posX, posY, width, height);
                g.FillRectangle(new SolidBrush(Color.LightBlue), rectT);
            }

            Size textSize = TextRenderer.MeasureText(text, font);
            int heightPadding = (height - textSize.Height) / 2;
            g.DrawString(text, font, textBrush, (posX + (width / 2 - textSize.Width / 2)), posY + heightPadding);
            Rectangle rec = new Rectangle(posX, posY, width, height);
            g.DrawRectangle(pen, rec);
        }

        private string[] PrepareDaysOfWeekArray()
        {
            EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
            string[] daysOfWeek = new string[7];
            for (int i = 0; i < 7; i++)
            {
                daysOfWeek[i] = Utility.GetDaysOfWeekName(i, _activeLanguageCode, weekSetting);
            }
            return daysOfWeek;
        }

        private void CalendarDrawing(List<Day> daysList)
        {
            _daysOfWeekHeight = 22;
            _dayWidth = (pictureBoxCalendar.Width - 1) / 7;
            _dayHeight = ((pictureBoxCalendar.Height - 1) - _daysOfWeekHeight) / 6;

            int lineHeight = (_dayHeight * 70) / (6 * 100);
            int dayFrameHeight = _dayHeight - (lineHeight * 6);
            int posX = 0, posY = 0;

            Bitmap canvas = new Bitmap(pictureBoxCalendar.Width, pictureBoxCalendar.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, canvas.Width, canvas.Height));

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            SolidBrush outDayBrush = new SolidBrush(Color.Gray);
            SolidBrush eventBrush = new SolidBrush(Color.Blue);

            Font dayOfWeekFont = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Regular, GraphicsUnit.Point);
            Font dayFont = new Font(FontFamily.GenericSansSerif, dayFrameHeight - 6, FontStyle.Bold, GraphicsUnit.Pixel);
            Font tranzitFont = new Font(FontFamily.GenericSansSerif, dayFrameHeight / 3, FontStyle.Regular, GraphicsUnit.Pixel);
            Font sunFont = new Font(FontFamily.GenericSansSerif, dayFrameHeight / 4, FontStyle.Regular, GraphicsUnit.Pixel);
            Font lineFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.CALENDARTEXT)), lineHeight - 2, Utility.GetFontStyleBySettings(EFontList.CALENDARTEXT), GraphicsUnit.Pixel);
            Font eclipseFont = new Font(FontFamily.GenericSansSerif, dayFrameHeight / 4, FontStyle.Regular, GraphicsUnit.Pixel);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // Drawing days of week
            for (int i = 0; i < _daysOfWeek.Length; i++)
            {
                Size textSize = TextRenderer.MeasureText(_daysOfWeek[i], dayOfWeekFont);
                g.DrawString(_daysOfWeek[i], dayOfWeekFont, textBrush, (posX + (_dayWidth / 2 - textSize.Width / 2)), 2);
                Rectangle rec = new Rectangle(posX, posY, _dayWidth, _daysOfWeekHeight);
                g.DrawRectangle(pen, rec);
                posX = posX + _dayWidth;
            }

            //Drawing calendar
            int day = 0;
            DateTime todayDate = DateTime.Now;
            posY += _daysOfWeekHeight;
            for (int row = 0; row < 6; row++)
            {
                posX = 0;
                for (int col = 0; col < 7; col++)
                {
                    // Drawing blue rectangle for today
                    if (daysList[day].Date == todayDate.Date)
                    {
                        Size textSize = TextRenderer.MeasureText(daysList[day].Date.Day.ToString(), dayFont);
                        Rectangle rectT = new Rectangle(posX + 1, posY + 1, textSize.Width, textSize.Height);
                        g.FillRectangle(new SolidBrush(Color.LightBlue), rectT);
                        g.DrawRectangle(new Pen(Color.DarkBlue, 1), rectT);
                    }

                    // Drawing day number
                    Size daySize = new Size(0, 0), imgSize = new Size(0, 0);
                    if (daysList[day].IsDayOfMonth)
                    {
                        string dayText = daysList[day].Date.Day.ToString();
                        daySize = TextRenderer.MeasureText(dayText, dayFont);
                        g.DrawString(dayText, dayFont, textBrush, posX + 2, posY + 2);
                    }
                    else
                    {
                        string dayText = daysList[day].Date.Day.ToString();
                        daySize = TextRenderer.MeasureText(dayText, dayFont);
                        g.DrawString(dayText, dayFont, outDayBrush, posX + 2, posY + 2);
                    }

                    //Drawing eclipse
                    if (daysList[day].EclipseDayList.Count > 0)
                    {
                        Image img = null;
                        foreach (EclipseCalendar ec in daysList[day].EclipseDayList)
                        {
                            imgSize = new Size(dayFrameHeight / 3, dayFrameHeight / 3);
                            if (ec.EclipseCode == (int)EEclipse.MOONECLIPSE)
                                img = new Bitmap(_moonEclipseImage, imgSize);
                            if (ec.EclipseCode == (int)EEclipse.SUNECLIPSE)
                                img = new Bitmap(_sunEclipseImage, imgSize);
                            g.DrawImage(img, posX + daySize.Width - 2, posY + (dayFrameHeight / 2 - imgSize.Height / 2));
                            g.DrawString(ec.GetShortName(_activeLanguageCode), eclipseFont, textBrush, posX + daySize.Width - 2, posY + (dayFrameHeight / 2 + imgSize.Height / 2) + 1);
                        }
                    }

                    //Drawing planets tranzits
                    string planetsListText = FindIfPlanetChangeZnak(daysList[day]);
                    if (!planetsListText.Equals(string.Empty))
                    {
                        Size planetListSize = TextRenderer.MeasureText(planetsListText, tranzitFont);
                        g.DrawString(planetsListText, tranzitFont, textBrush, posX + daySize.Width + imgSize.Width + 1, posY + (dayFrameHeight / 2 - planetListSize.Height / 2));
                    }

                    //Drawing sunrise, sunset
                    string sunriseTime = Utility.GetSunStatusName(ESun.SUNRISE, _activeLanguageCode) + daysList[day].SunRise?.ToString("HH:mm:ss");
                    string sunsetTime = Utility.GetSunStatusName(ESun.SUNSET, _activeLanguageCode) + daysList[day].SunSet?.ToString("HH:mm:ss");
                    Size textSise = TextRenderer.MeasureText(sunriseTime, sunFont);
                    int posXSunrise = posX + (_dayWidth - textSise.Width);
                    g.DrawString(sunriseTime, sunFont, textBrush, posXSunrise, posY + (dayFrameHeight / 4 - textSise.Height / 2));
                    g.DrawString(sunsetTime, sunFont, textBrush, posXSunrise, posY + (dayFrameHeight / 4 - textSise.Height / 2) + textSise.Height);

                    // Drawing colored lines
                    int posYForLines = posY + dayFrameHeight;
                    if (daysList[day].Date >= new DateTime(_yearMin, 1, 1) && daysList[day].Date <= new DateTime(_yearMax, 12, 31, 23, 59, 59))
                    {
                        DrawColoredLine(g, pen, lineFont, textBrush, posX, posYForLines, _dayWidth, lineHeight, daysList[day].NakshatraDayList);
                        DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + lineHeight), _dayWidth, lineHeight, daysList[day].TaraBalaDayList);
                        DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 2 * lineHeight), _dayWidth, lineHeight, daysList[day].TithiDayList);
                        DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 3 * lineHeight), _dayWidth, lineHeight, daysList[day].KaranaDayList);
                        DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 4 * lineHeight), _dayWidth, lineHeight, daysList[day].NityaJogaDayList);
                        DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 5 * lineHeight), _dayWidth, lineHeight, daysList[day].ChandraBalaDayList);
                    }
                    
                    //Check for events and drawing if exists
                    List<PersonsEventsList> peDayList = Utility.GetDayPersonEvents(_selectedProfile.GUID, daysList[day].Date);
                    if (peDayList.Count > 0)
                    {
                        int triangleX = (posX + _dayWidth) - 10;
                        int triangleY = posY + 10;

                        Point[] tringlePoint = new Point[] { new Point(triangleX, posY), new Point(posX + _dayWidth, posY), new Point(posX + _dayWidth, triangleY) };
                        g.FillPolygon(eventBrush, tringlePoint);
                        g.DrawPolygon(pen, tringlePoint);
                    }
                    
                    // Drawing days grid
                    Rectangle dRec = new Rectangle(posX, posY, _dayWidth, _dayHeight);
                    g.DrawRectangle(pen, dRec);
                    posX += _dayWidth;
                    day++;
                }
                posY += _dayHeight;
            }
            pictureBoxCalendar.Image = canvas;
            _currentCalendarImage = pictureBoxCalendar.Image;
        }

        private void DrawColoredLine(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> dayList)
        {
            int drawWidth = 0, usedWidth = 0;
            foreach (Calendar c in dayList)
            {
                if (c == dayList.Last())
                {
                    drawWidth = width - usedWidth;
                    DrawColoredRectangle(g, pen, font, textBrush, (posX + usedWidth), posY, drawWidth, height, c);
                }
                else
                {
                    drawWidth = Utility.ConvertHoursToPixels(width, c.DateEnd) - usedWidth;
                    DrawColoredRectangle(g, pen, font, textBrush, (posX + usedWidth), posY, drawWidth, height, c);
                    usedWidth = usedWidth + drawWidth;
                }
            }
        }

        private void DrawColoredRectangle(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, Calendar c)
        {
            Rectangle rect = new Rectangle(posX, posY, width, height);
            SolidBrush brush = new SolidBrush(Utility.GetColorByColorCode(c.ColorCode));
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            string text = c.GetShortName(_activeLanguageCode);
            Size textSize = TextRenderer.MeasureText(text, font);
            int heightPadding = (height - textSize.Height) / 2;
            if (textSize.Width + 1 <= rect.Width)
            {
                g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
            }
        }

        private List<Day> PrepareMonthDays(DateTime startDateOfMonth, Profile sPerson)
        {
            List<Day> daysList = new List<Day>();
            DateTime startDate, endDate;

            EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
            int startDayOfWeek = Utility.GetDayOfWeekNumberFromDate(startDateOfMonth, weekSetting);
            if (startDayOfWeek > 0)
            {
                startDate = startDateOfMonth.AddDays(-startDayOfWeek);
            }
            else
            {
                startDate = startDateOfMonth;
            }

            int addAfterDays = 42 - (startDayOfWeek + DateTime.DaysInMonth(startDateOfMonth.Year, startDateOfMonth.Month));
            endDate = new DateTime(startDateOfMonth.Year, startDateOfMonth.Month, DateTime.DaysInMonth(startDateOfMonth.Year, startDateOfMonth.Month)).AddDays(+addAfterDays);

            double latitude, longitude;
            string timeZone = string.Empty;
            if (Utility.GetGeoCoordinateByLocationId(sPerson.PlaceOfLivingId, out latitude, out longitude))
            {
                timeZone = Utility.GetTimeZoneIdByGeoCoordinates(latitude, longitude);
                TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                TimeZoneInfo.AdjustmentRule[] adjustmentRules = currentTimeZone.GetAdjustmentRules();

                List<PlanetCalendar> moonZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_moonZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                moonZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_moonZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                moonZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_moonNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                moonNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_moonPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                moonPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_sunZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                sunZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_sunZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                sunZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_sunNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                sunNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_sunPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                sunPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                mercuryZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                mercuryZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                mercuryNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                mercuryPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_venusZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                venusZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_venusZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                venusZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_venusNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                venusNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_venusPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                venusPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_marsZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                marsZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_marsZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                marsZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_marsNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                marsNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_marsPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                marsPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                jupiterZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                jupiterZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                jupiterNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                jupiterPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                saturnZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                saturnZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                saturnNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                saturnPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanPadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTrueZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTrueZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTrueNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTruePadaPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTruePadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                rahuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTrueZodiakCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTrueZodiakRetroCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTrueNakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTruePadaPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTruePadaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                ketuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<NakshatraCalendar> nakshatraPeriodCalendar = Utility.CloneNakshatraCalendarList(_nakshatraCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                nakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                nakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<TithiCalendar> tithiPeriodCalendar = Utility.CloneTithiCalendarList(_tithiCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                tithiPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                tithiPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<KaranaCalendar> karanaPeriodCalendar = Utility.CloneKaranaCalendarList(_karanaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                karanaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                karanaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<ChandraBalaCalendar> chandraBalaCalendar = Utility.CloneChandraBalaCalendarList(_chandraBalaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                chandraBalaCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                chandraBalaCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<NityaJogaCalendar> nityaJogaCalendar = Utility.CloneNityaJogaCalendarList(_nityaJogaCalendarList.Where(i => i.DateEnd > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                nityaJogaCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                nityaJogaCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<EclipseCalendar> eclipsePeriodCalendar = Utility.CloneEclipseCalendarList(_eclipseCalendarList.Where(i => i.DateStart > startDate.AddDays(-2) && i.DateStart < endDate.AddDays(+2)).ToList());
                eclipsePeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                eclipsePeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); });

                List<MasaCalendar> masaPeriodCalendar = Utility.CloneMasaCalendarList(_masaCalendarList.ToList());
                masaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                masaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); });

                Day tempDay;
                // Preparing original List<DayCalendars> list
                for (DateTime currentDay = startDate; currentDay <= endDate;)
                {
                    DateTime? sunrise = Utility.CalculateSunriseForDateAndLocation(currentDay, latitude, longitude, timeZone);
                    DateTime? sunset = Utility.CalculateSunsetForDateAndLocation(currentDay, latitude, longitude, timeZone);

                    bool isDayOfMonth = (currentDay.Month == startDateOfMonth.Month) ? true : false;
                    tempDay = new Day (
                                        sPerson,
                                        currentDay,
                                        isDayOfMonth,
                                        sunrise,
                                        sunset,
                                        moonZodiakPeriodCalendar,
                                        moonZodiakRetroPeriodCalendar,
                                        moonNakshatraPeriodCalendar,
                                        moonPadaPeriodCalendar,
                                        sunZodiakPeriodCalendar,
                                        sunZodiakRetroPeriodCalendar,
                                        sunNakshatraPeriodCalendar,
                                        sunPadaPeriodCalendar,
                                        mercuryZodiakPeriodCalendar,
                                        mercuryZodiakRetroPeriodCalendar,
                                        mercuryNakshatraPeriodCalendar,
                                        mercuryPadaPeriodCalendar,
                                        venusZodiakPeriodCalendar,
                                        venusZodiakRetroPeriodCalendar,
                                        venusNakshatraPeriodCalendar,
                                        venusPadaPeriodCalendar,
                                        marsZodiakPeriodCalendar,
                                        marsZodiakRetroPeriodCalendar,
                                        marsNakshatraPeriodCalendar,
                                        marsPadaPeriodCalendar,
                                        jupiterZodiakPeriodCalendar,
                                        jupiterZodiakRetroPeriodCalendar,
                                        jupiterNakshatraPeriodCalendar,
                                        jupiterPadaPeriodCalendar,
                                        saturnZodiakPeriodCalendar,
                                        saturnZodiakRetroPeriodCalendar,
                                        saturnNakshatraPeriodCalendar,
                                        saturnPadaPeriodCalendar,
                                        rahuMeanZodiakPeriodCalendar,
                                        rahuMeanZodiakRetroPeriodCalendar,
                                        rahuMeanNakshatraPeriodCalendar,
                                        rahuMeanPadaPeriodCalendar,
                                        ketuMeanZodiakPeriodCalendar,
                                        ketuMeanZodiakRetroPeriodCalendar,
                                        ketuMeanNakshatraPeriodCalendar,
                                        ketuMeanPadaPeriodCalendar,
                                        rahuTrueZodiakPeriodCalendar,
                                        rahuTrueZodiakRetroPeriodCalendar,
                                        rahuTrueNakshatraPeriodCalendar,
                                        rahuTruePadaPeriodCalendar,
                                        ketuTrueZodiakPeriodCalendar,
                                        ketuTrueZodiakRetroPeriodCalendar,
                                        ketuTrueNakshatraPeriodCalendar,
                                        ketuTruePadaPeriodCalendar,
                                        nakshatraPeriodCalendar,
                                        tithiPeriodCalendar,
                                        karanaPeriodCalendar,
                                        chandraBalaCalendar,
                                        nityaJogaCalendar,
                                        eclipsePeriodCalendar,
                                        masaPeriodCalendar);
                    daysList.Add(tempDay);
                    currentDay = currentDay.AddDays(+1);                   
                }

                for (int day = 0; day < daysList.Count; day++)
                {
                    DateTime? sunrisePrevious = Utility.CalculateSunriseForDateAndLocation(daysList[day].Date.AddDays(-1), latitude, longitude, timeZone);
                    DateTime? sunsetPrevious = Utility.CalculateSunsetForDateAndLocation(daysList[day].Date.AddDays(-1), latitude, longitude, timeZone);
                    DateTime? sunriseNext = Utility.CalculateSunriseForDateAndLocation(daysList[day].Date.AddDays(+1), latitude, longitude, timeZone);

                    // Adding List<JogaCalendar> lists to List<DayCalendars> list
                    daysList[day].DwipushkarJogaDayList = PrepareJogaList(EJoga.DWIPUSHKAR, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].TripushkarJogaDayList = PrepareJogaList(EJoga.TRIPUSHKAR, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].AmritaSiddhaJogaDayList = PrepareJogaList(EJoga.AMRITASIDDHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].SarvarthaSiddhaJogaDayList = PrepareJogaList(EJoga.SARVARTHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].SiddhaJogaDayList = PrepareJogaList(EJoga.SIDDHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].MrityuJogaDayList = PrepareJogaList(EJoga.MRITYU, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].AdhamJogaDayList = PrepareJogaList(EJoga.ADHAM, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].YamaghataJogaDayList = PrepareJogaList(EJoga.YAMAGHATA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].DagdhaJogaDayList = PrepareJogaList(EJoga.DAGDHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].UnfarobaleJogaDayList = PrepareJogaList(EJoga.UNFAVORABLE, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);

                    //Adding List<MuhurtaCalendar> lists to List<DayCalendars> list
                    daysList[day].MuhurtaDayList = PrepareMuhurtaList(daysList[day].Date, daysList[day].SunRise, sunrisePrevious, daysList[day].SunSet);

                    //Adding List<HoraCalendar> to List<DayCalendars> lists
                    daysList[day].Hora12Plus12DayList = Prepare12Plus12HoraList(daysList[day].Date, sunrisePrevious, sunsetPrevious, daysList[day].SunRise, daysList[day].SunSet, sunriseNext);
                    daysList[day].HoraEqualDayList = PrepareEqualHoraList(daysList[day].Date, sunrisePrevious, daysList[day].SunRise, sunriseNext);
                    daysList[day].HoraFrom6DayList = PrepareHoraFrom6List(daysList[day].Date);

                    //Adding List<Muhurta30Calendar> to List<DayCalendars> lists
                    daysList[day].Muhurta15Plus1530DayList = Prepare15Plus15Muhurta30List(daysList[day].Date, sunsetPrevious, daysList[day].SunRise, daysList[day].SunSet, sunriseNext);
                    daysList[day].MuhurtaEqual30DayList = PrepareEqualMuhurta30List(daysList[day].Date, sunrisePrevious, daysList[day].SunRise, sunriseNext);
                    daysList[day].Muhurta30From6DayList = PrepareMuhurta30From6List(daysList[day].Date);

                    //Adding List<Ghati60Calendar> to list<DayCalendars> lists
                    daysList[day].Ghati60_30Plus30DayList = Prepare30Plus30Ghati60List(daysList[day].Date, sunsetPrevious, daysList[day].SunRise, daysList[day].SunSet, sunriseNext);
                    daysList[day].Ghati60EqualDayList = PrepareEqualGhati60List(daysList[day].Date, sunrisePrevious, daysList[day].SunRise, sunriseNext);
                    daysList[day].Ghati60From6DayList = PrepareGhati60From6List(daysList[day].Date);
                }
            }
            else
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Please check geo-coordinates.", _activeLanguageCode), Utility.GetLocalizedText("Information", _activeLanguageCode), enumMessageIcon.Warning, enumMessageButton.OK);
            }
            return daysList;
        }

        private List<Calendar> PrepareJogaList(EJoga jogaCode, List<Day> daysList, DateTime date, DateTime? sunrise, DateTime? sunrisePrevious, DateTime? sunriseNext)
        {
            JogaCalendar jCal = new JogaCalendar();
            List<JogaCalendar> jogaList = new List<JogaCalendar>();

            switch (jogaCode)
            {
                case EJoga.DWIPUSHKAR:
                    List<JogaCalendar> dwipushkarJogaPrev = jCal.CheckDvipushkarJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckDvipushkarJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in dwipushkarJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.TRIPUSHKAR:
                    List<JogaCalendar> tripushkarJogaPrev = jCal.CheckTripushkarJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckTripushkarJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in tripushkarJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.AMRITASIDDHA:
                    List<JogaCalendar> amritaSiddhaJogaPrev = jCal.CheckAmritaSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckAmritaSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in amritaSiddhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.SARVARTHA:
                    List<JogaCalendar> sarvarthaSiddhaJogaPrev = jCal.CheckSarvarthaSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckSarvarthaSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in sarvarthaSiddhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.SIDDHA:
                    List<JogaCalendar> siddhaJogaPrev = jCal.CheckSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in siddhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    List<JogaCalendar> largeSiddhaJogaPrev = jCal.CheckLargeSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    List<JogaCalendar> largeSiddhaJoga = jCal.CheckLargeSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in largeSiddhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            largeSiddhaJoga.Insert(0, jc);
                    }
                    if (largeSiddhaJoga.Count > 0)
                    {
                        jogaList.AddRange(largeSiddhaJoga);
                        jogaList.OrderBy(i => i.DateStart);
                    }
                    break;

                case EJoga.MRITYU:
                    List<JogaCalendar> mrityuJogaPrev = jCal.CheckMritjuJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckMritjuJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in mrityuJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.ADHAM:
                    List<JogaCalendar> adhamJogaPrev = jCal.CheckAdhamJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckAdhamJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in adhamJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.YAMAGHATA:
                    List<JogaCalendar> yamaghataJogaPrev = jCal.CheckYamaghataJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckYamaghataJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in yamaghataJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.DAGDHA:
                    List<JogaCalendar> dagdhaJogaPrev = jCal.CheckDagdhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckDagdhaJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in dagdhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EJoga.UNFAVORABLE:
                    List<JogaCalendar> unfarobaleJogaPrev = jCal.CheckUnfarobaleJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckUnfarobaleJoga(sunrise, sunriseNext, daysList);
                    foreach (JogaCalendar jc in unfarobaleJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                default:
                    break;
            }

            List<Calendar> resList = new List<Calendar>();
            foreach (JogaCalendar jc in jogaList)
            {
                resList.Add(jc);
            }
            return resList;
        }

        private List<Calendar> PrepareMuhurtaList(DateTime date, DateTime? sunrise, DateTime? sunrisePrevious, DateTime? sunset)
        {
            MuhurtaCalendar mCal = new MuhurtaCalendar();
            List<MuhurtaCalendar> muhurtaList = new List<MuhurtaCalendar>();
            TimeSpan sunDayLong = sunset.Value.Subtract(sunrise.Value);

            MuhurtaCalendar abhijit = mCal.CalculateteAbhijitMuhurta(sunrise, sunDayLong, date.DayOfWeek);
            MuhurtaCalendar rahuKala = mCal.CalculateteRahuKala(sunrise, sunDayLong, date.DayOfWeek);
            MuhurtaCalendar brahma = mCal.CalculateBrahmaMuhurta(sunrise, sunrisePrevious);
            MuhurtaCalendar gulikaKala = mCal.CalculateGulikaKala(sunrise, sunDayLong, date.DayOfWeek);
            MuhurtaCalendar yamaganda = mCal.CalculateYamagandaMuhurta(sunrise, sunDayLong, date.DayOfWeek);

            if (abhijit != null) 
                muhurtaList.Add(abhijit);
            if (rahuKala != null)
                muhurtaList.Add(rahuKala);
            if (brahma != null)
                muhurtaList.Add(brahma);
            if (gulikaKala != null)
                muhurtaList.Add(gulikaKala);
            if (yamaganda != null)
                muhurtaList.Add(yamaganda);

            List<MuhurtaCalendar> sortedList = muhurtaList.OrderBy(s => s.DateStart).ToList();
            List<Calendar> resList = new List<Calendar>();
            foreach (MuhurtaCalendar mc in sortedList)
            {
                resList.Add(mc);
            }
            return resList;
        }

        private List<Calendar> Prepare12Plus12HoraList(DateTime date, DateTime? sunrisePrevious, DateTime? sunsetPrevious, DateTime? sunrise, DateTime? sunset, DateTime? sunriseNext)
        {
            HoraCalendar hc = new HoraCalendar();
            List<Calendar> resList = new List<Calendar>();
            List<HoraCalendar> hList = hc.Calculate12Plus12Hora(CacheLoad._horaPlanetList.ToList(), sunrisePrevious, sunsetPrevious, sunrise, sunset, sunriseNext, date);
            foreach (HoraCalendar h in hList)
            {
                resList.Add(h);
            }
            return resList;
        }

        private List<Calendar> PrepareEqualHoraList(DateTime date, DateTime? sunrisePrevious, DateTime? sunrise, DateTime? sunriseNext)
        {
            HoraCalendar hc = new HoraCalendar();
            List<Calendar> resList = new List<Calendar>();
            List<HoraCalendar> hList = hc.CalculateEqualHora(CacheLoad._horaPlanetList.ToList(), sunrisePrevious, sunrise, sunriseNext, date);
            foreach (HoraCalendar h in hList)
            {
                resList.Add(h);
            }
            return resList;
        }

        private List<Calendar> PrepareHoraFrom6List(DateTime date)
        {
            HoraCalendar hc = new HoraCalendar();
            List<Calendar> resList = new List<Calendar>();
            List<HoraCalendar> hList = hc.CalculateHoraFrom6(CacheLoad._horaPlanetList.ToList(), date);
            foreach (HoraCalendar h in hList)
            {
                resList.Add(h);
            }
            return resList;
        }

        private List<Calendar> Prepare15Plus15Muhurta30List(DateTime date, DateTime? sunsetPrevious, DateTime? sunrise, DateTime? sunset, DateTime? sunriseNext)
        {
            Muhurta30Calendar m30 = new Muhurta30Calendar();
            List<Calendar> resList = new List<Calendar>();
            List<Muhurta30Calendar> m30List = m30.Calculate15Plus15Muhurta30(CacheLoad._muhurta30List.ToList(), sunsetPrevious, sunrise, sunset, sunriseNext, date);
            foreach (Muhurta30Calendar ml in m30List)
            {
                resList.Add(ml);
            }
            return resList;
        }

        private List<Calendar> PrepareEqualMuhurta30List(DateTime date, DateTime? sunrisePrev, DateTime? sunrise, DateTime? sunriseNext)
        {
            Muhurta30Calendar m30 = new Muhurta30Calendar();
            List<Calendar> resList = new List<Calendar>();
            List<Muhurta30Calendar> m30List = m30.CalculateEqualMuhurta30(CacheLoad._muhurta30List.ToList(), sunrisePrev, sunrise, sunriseNext, date);
            foreach (Muhurta30Calendar ml in m30List)
            {
                resList.Add(ml);
            }
            return resList;
        }

        private List<Calendar> PrepareMuhurta30From6List(DateTime date)
        {
            Muhurta30Calendar m30 = new Muhurta30Calendar();
            List<Calendar> resList = new List<Calendar>();
            List<Muhurta30Calendar> m30List = m30.CalculateMuhurta30From6(CacheLoad._muhurta30List.ToList(), date);
            foreach (Muhurta30Calendar ml in m30List)
            {
                resList.Add(ml);
            }
            return resList;
        }

        private List<Calendar> Prepare30Plus30Ghati60List(DateTime date, DateTime? sunsetPrevious, DateTime? sunrise, DateTime? sunset, DateTime? sunriseNext)
        {
            Ghati60Calendar g60 = new Ghati60Calendar();
            List<Calendar> resList = new List<Calendar>();
            List<Ghati60Calendar> g60List = g60.Calculate30Plus30Ghati60(CacheLoad._ghati60List.ToList(), sunsetPrevious, sunrise, sunset, sunriseNext, date);
            foreach (Ghati60Calendar gl in g60List)
            {
                resList.Add(gl);
            }
            return resList;
        }

        private List<Calendar> PrepareEqualGhati60List(DateTime date, DateTime? sunrisePrev, DateTime? sunrise, DateTime? sunriseNext)
        {
            Ghati60Calendar g60 = new Ghati60Calendar();
            List<Calendar> resList = new List<Calendar>();
            List<Ghati60Calendar> g60List = g60.CalculateEqualGhati60(CacheLoad._ghati60List.ToList(), sunrisePrev, sunrise, sunriseNext, date);
            foreach (Ghati60Calendar gl in g60List)
            {
                resList.Add(gl);
            }
            return resList;
        }

        private List<Calendar> PrepareGhati60From6List(DateTime date)
        {
            Ghati60Calendar g60 = new Ghati60Calendar();
            List<Calendar> resList = new List<Calendar>();
            List<Ghati60Calendar> g60List = g60.CalculateGhati60From6(CacheLoad._ghati60List.ToList(), date);
            foreach (Ghati60Calendar gl in g60List)
            {
                resList.Add(gl);
            }
            return resList;
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfileForm pForm = new ProfileForm(CacheLoad._profileList.ToList(), _activeLanguageCode, false);
            pForm.ShowDialog(this);
            if(pForm.SelectedProfile != null && pForm.SelectedProfile != _selectedProfile && pForm.IsChosen && _yearMin != 0 && _yearMax != 0)
            { 
                _selectedProfile = pForm.SelectedProfile;
                PrepareProfileAndTimeZoneLabels();
                _daysList = null;
                _daysOfMonth = null;
                _daysList = PrepareMonthDays(new DateTime(_selectedDate.Year, _selectedDate.Month, 1), _selectedProfile);

                //Drawing
                CalendarDrawing(_daysList);
                TranzitDrawing(_daysList);

            }
        }

        private void applicationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppSettings asForm = new AppSettings(CacheLoad._appSettingList, _activeLanguageCode);
            asForm.ShowDialog(this);
            
        }

        private void locationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocationForm lForm = new LocationForm(CacheLoad._locationList.ToList(), _activeLanguageCode, true);
            lForm.ShowDialog(this);
        }

        private void colorSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorSettings csForm = new ColorSettings(CacheLoad._colorList.ToList(), _activeLanguageCode);
            csForm.ShowDialog(this);
        }

        private void fontSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontSettings fsForm = new FontSettings(CacheLoad._fontList.ToList(), _activeLanguageCode);
            fsForm.ShowDialog(this);
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lang = _activeLanguageCode.ToString();
            try
            {

                System.Diagnostics.Process.Start(@".\Manual\pad_help_"+ lang + ".pdf");
            }
            catch
            {
                frmShowMessage.Show(Utility.GetLocalizedText("File not found", _activeLanguageCode), Utility.GetLocalizedText("Error", _activeLanguageCode), enumMessageIcon.Error, enumMessageButton.OK);
            }
        }

        private void aboutPersonalDiaryCalendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aForm = new About(_activeLanguageCode);
            aForm.ShowDialog(this);
        }

        private Stream GetStream(Image img, ImageFormat format)
        {
            var ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }

        private void exportYearToPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportToPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedProfile != null)
            {
                try
                {
                    using (WaitForm wForm = new WaitForm(CreatePDF, _activeLanguageCode))
                    {
                        wForm.ShowDialog(this);
                    }
                    string exportfilename = datePicker.Value.ToString("MMMM", CultureInfo.InvariantCulture) + "_" + datePicker.Value.Year + ".pdf";
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.FileName = exportfilename;
                    savefile.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        _pdfDoc.Save(Path.GetFullPath(savefile.FileName));
                        frmShowMessage.Show(Utility.GetLocalizedText("File", _activeLanguageCode) + " " + exportfilename + " " + Utility.GetLocalizedText("created successfuly!", _activeLanguageCode), Utility.GetLocalizedText("Information", _activeLanguageCode), enumMessageIcon.Information, enumMessageButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    frmShowMessage.Show(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace, Utility.GetLocalizedText("Error", _activeLanguageCode), enumMessageIcon.Error, enumMessageButton.OK);
                }
            }
        }

        private void CreatePDF()
        {
            _pdfDoc = null;
            _pdfDoc = new PdfDocument();

            string filename = string.Empty;
            switch (_activeLanguageCode)
            {
                case ELanguage.ru:
                    filename = "Titul_ru.pdf";
                    break;

                case ELanguage.en:
                    filename = "Titul_en.pdf";
                    break;
            }

            PdfDocument titulPDF = PdfReader.Open(@".\Data\" + filename, PdfDocumentOpenMode.Import);
            for (int i = 0; i < titulPDF.PageCount; i++)
            {
                PdfPage titulPage = titulPDF.Pages[i];
                _pdfDoc.AddPage(titulPage);
            }

            PdfPage page = _pdfDoc.AddPage();
            page.Orientation = PageOrientation.Landscape;
            XSize size = PageSizeConverter.ToSize(PageSize.A4);
            page.Width = size.Height;
            page.Height = size.Width;

            XFont font = new XFont("", 12, XFontStyle.Bold);
            XFont fontTZ = new XFont("", 8, XFontStyle.Regular);
            XFont fontAuthor = new XFont("", 6, XFontStyle.Regular);

            string localInfo = GetTimeZoneInfo(_selectedProfile.PlaceOfLivingId);
            string textAuthor = Utility.GetLocalizedText("Concept: Halyna Sheremet", _activeLanguageCode) + " (VK: Halyna Sheremet http://vk.com/id263300332, Facebook: Halyna Sheremet https://www.facebook.com/halyna.sheremet)     " + Utility.GetLocalizedText("Programming: Sergey Sheremet", _activeLanguageCode);

            XImage[] imageArray = new XImage[2];

            Bitmap bmp42 = new Bitmap(Convert.ToInt32(page.Width.Point - 20), Convert.ToInt32(page.Height.Point - 100));
            bmp42 = (Bitmap)_currentCalendarImage;
            XImage image42 = XImage.FromStream(GetStream(bmp42, ImageFormat.Bmp));

            Bitmap bmpTransit = new Bitmap(Convert.ToInt32(page.Width.Point - 20), Convert.ToInt32(page.Height.Point - 100));
            bmpTransit = (Bitmap)_currentTranzitsImage;
            XImage imageTransit = XImage.FromStream(GetStream(bmpTransit, ImageFormat.Bmp));

            imageArray[0] = image42;
            imageArray[1] = imageTransit;

            XGraphics graph = null;
            for (int i = 0; i < imageArray.Length; i++)
            {
                graph = XGraphics.FromPdfPage(page);
                graph.DrawString(datePicker.Value.ToString("MMMM", CultureInfo.GetCultureInfo(Utility.GetActiveCultureCode(_activeLanguageCode))) + ", " + datePicker.Value.Year, font, XBrushes.Black,
                            new XRect(10, 5, page.Width.Point / 2, page.Height.Point), XStringFormats.TopLeft);
                graph.DrawString(_selectedProfile.PersonName + " " + _selectedProfile.PersonSurname, font, XBrushes.Black,
                                new XRect(page.Width.Point / 2, 5, page.Width.Point / 2 - 10, page.Height.Point), XStringFormats.TopRight);

                graph.DrawString(localInfo, fontTZ, XBrushes.Black, new XRect(0, 8, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                graph.DrawImage(imageArray[i], 10, 20, page.Width.Point - 20, page.Height - 130);
                graph.DrawString(textAuthor, fontAuthor, XBrushes.Black, new XRect(10, page.Height.Point - 8, page.Width.Point - 20, 8), XStringFormats.TopRight);

                if (i < imageArray.Length - 1)
                {
                    page = _pdfDoc.AddPage();
                    page.Orientation = PageOrientation.Landscape;
                    size = PageSizeConverter.ToSize(PageSize.A4);
                    page.Width = size.Height;
                    page.Height = size.Width;
                }
            }

            int day = 0, startIndexForPage = 0, nextIndexForPage = 0;
            int posY = 0, height = 0;
            do
            {
                if (_daysList[day].IsDayOfMonth)
                {

                    if (nextIndexForPage == 0)
                        nextIndexForPage = day;
                    startIndexForPage = day;

                    if (startIndexForPage == nextIndexForPage)
                    {
                        nextIndexForPage = startIndexForPage + 2;
                        page = _pdfDoc.AddPage();
                        posY = 30;
                        //page.Orientation = PageOrientation.Landscape;
                        size = PageSizeConverter.ToSize(PageSize.A4);
                        page.Width = size.Width; //size.Height;
                        page.Height = size.Height; //size.Width;

                        graph = XGraphics.FromPdfPage(page);
                        graph.DrawString(datePicker.Value.ToString("MMMM", CultureInfo.GetCultureInfo(Utility.GetActiveCultureCode(_activeLanguageCode))) + ", " + datePicker.Value.Year, font, XBrushes.Black,
                            new XRect(10, 5, page.Width.Point / 2, page.Height.Point), XStringFormats.TopLeft);
                        graph.DrawString(_selectedProfile.PersonName + " " + _selectedProfile.PersonSurname, font, XBrushes.Black,
                                        new XRect(page.Width.Point / 2, 5, page.Width.Point / 2 - 10, page.Height.Point), XStringFormats.TopRight);
                        graph.DrawString(localInfo, fontTZ, XBrushes.Black, new XRect(0, 8, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);

                        graph.DrawString(textAuthor, fontAuthor, XBrushes.Black, new XRect(10, page.Height.Point - 8, page.Width.Point - 20, 8), XStringFormats.TopRight);
                    }

                    height = Convert.ToInt32(page.Height.Point - 50) / 2;
                    Bitmap bmp = new Bitmap(Convert.ToInt32(page.Width.Point - 20), height);
                    Bitmap bmpReady = DrawDayRectangle(bmp, _selectedProfile, _daysList[day], _activeLanguageCode);
                    XImage image = XImage.FromStream(GetStream(bmpReady, ImageFormat.Bmp));

                    graph.DrawImage(image, 10, posY, page.Width.Point - 20, height);
                    posY = posY + height + 10;
                }
                day++;
            }
            while (day < _daysList.Count);
        }

        private Bitmap DrawDayRectangle(Bitmap bmp, Profile sPerson, Day pDay, ELanguage langCode)
        {
            int linesCount = 0;
            int formHeight = CalculateCalendarTooltipFormHeight(pDay, out linesCount);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bmp.Width, bmp.Height));
            g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1));
            CalendarToolTip ctt = new CalendarToolTip(sPerson, pDay, bmp.Width, bmp.Height, linesCount, langCode);
            ctt.DrawDay(new PaintEventArgs(g, new Rectangle(0, 0, bmp.Width, bmp.Height)));
            return bmp;
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            if (_selectedProfile == null || _daysList == null)
                return;

            if (_selectedProfile != null && _daysList != null)
            {
                EAppSetting currentWeekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
                CacheLoad._appSettingList = null;
                CacheLoad._appSettingList = CacheLoad.GetAppSettingsList();
                EAppSetting refreshedWeekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;

                if (refreshedWeekSetting != currentWeekSetting) // Recalculation only in case when days of week view has been changed
                {
                    _daysOfWeek = null;
                    _daysOfWeek = PrepareDaysOfWeekArray();
                    _daysList = null;
                    _daysOfMonth = null;
                    _daysList = PrepareMonthDays(new DateTime(_selectedDate.Year, _selectedDate.Month, 1), _selectedProfile);
                }

                //Drawing
                PrepareProfileAndTimeZoneLabels();
                CalendarDrawing(_daysList);
                TranzitDrawing(_daysList);

                // refresh dayView if opened and years calendar
                if (tabControlCalendar.TabPages.Count > 2)
                {
                    string calendarText = Utility.GetLocalizedText("Calendar", _activeLanguageCode);
                    string tranzitText = Utility.GetLocalizedText("Tranzits", _activeLanguageCode);
                    string yearTranzitText = Utility.GetLocalizedText("Year's tranzits", _activeLanguageCode);

                    foreach (TabPage tp in tabControlCalendar.TabPages)
                    {
                        if (!tp.Text.Equals(calendarText) && !tp.Text.Equals(tranzitText) && !tp.Text.Contains(yearTranzitText))
                        {
                            // Re-generate events for tabDay refresh
                            foreach (var control in tp.Controls)
                            {
                                if (control is TabDay)
                                {
                                    DateTime openedDate = DateTime.ParseExact(tp.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                    Day openedDay = _daysList.Where(i => i.Date == openedDate).FirstOrDefault();
                                    TabDay td = control as TabDay;
                                    td.ClearAppointments(true);
                                    td.DrawSystemAppointments(openedDay, _activeLanguageCode);
                                    td.Refresh();
                                }
                            }
                        }
                        if (tp.Text.Contains(yearTranzitText))
                        {
                            // re-draw years calendar
                            foreach (var control in tp.Controls)
                            {
                                int year = Convert.ToInt32(tp.Text.Substring(tp.Text.Length - 4));
                                if (control is YearTranzits)
                                {
                                    YearTranzits yt = control as YearTranzits;
                                    List<Day> daysList = yt.PrepareYearDays(year, _selectedProfile);
                                    yt.YearTranzitDrawing(daysList);
                                    yt.Refresh();
                                }
                            }
                        }

                    }
                }

            }
        }

        private int CalculateCalendarTooltipFormHeight(Day day, out int linesCount)
        {
            int lineHeight = 14, dayInfo = 40;
            int yogasCount = GetYogasCount(day);
            linesCount = 6 + 9 + 5 + yogasCount;
            return dayInfo + 2 + 6 * lineHeight + 4 + 9 * lineHeight + 4 + 5 * lineHeight + 4 + yogasCount * lineHeight + 6;
        }

        private int GetYogasCount(Day day)
        {
            int count = 0;
            if (day.DwipushkarJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.DwipushkarJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.TripushkarJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.TripushkarJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.AmritaSiddhaJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.AmritaSiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.SarvarthaSiddhaJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.SarvarthaSiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.SiddhaJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.SiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.MrityuJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.MrityuJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.AdhamJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.AdhamJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.YamaghataJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.YamaghataJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.DagdhaJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.DagdhaJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            if (day.UnfarobaleJogaDayList.Count > 0)
            {
                foreach (JogaCalendar jc in day.UnfarobaleJogaDayList)
                {
                    if (jc.DateStart.Between(day.Date, day.Date.AddDays(+1)) || jc.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        count++;
                        break;
                    }
                }
            }
            return count;
        }

        private void pictureBoxCalendar_MouseClick(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null || e.Y < _daysOfWeekHeight || e.Y > (_daysOfWeekHeight + 6 * _dayHeight) - 1)
                return;

            if (e.Button == MouseButtons.Right)
            {
                int colNumber = e.X / _dayWidth;
                int rowNumber = (e.Y - _daysOfWeekHeight) / _dayHeight;
                int selectedDayIndex = rowNumber * 7 + colNumber;

                int formWidth = 700;
                int linesCount = 0;
                int formHeight = CalculateCalendarTooltipFormHeight(_daysList[selectedDayIndex], out linesCount);

                int posX = colNumber * _dayWidth + 5;
                int posY = ((rowNumber + 1) * _dayHeight - _dayHeight) + _daysOfWeekHeight + 5;

                if ((posX + formWidth) > pictureBoxCalendar.Width)
                    posX = posX + _dayWidth - formWidth - 10;

                if ((posY + formHeight) > pictureBoxCalendar.Height)
                    posY = posY + _dayHeight - formHeight - 10;

                CalendarTooltipCreation(_selectedProfile, _daysList[selectedDayIndex], formHeight, linesCount, _activeLanguageCode);
                toolTip.Show(pictureBoxCalendar, posX, posY);
            }

            if (e.Button == MouseButtons.Left)
            {
                int colNumber = e.X / _dayWidth;
                int rowNumber = (e.Y - _daysOfWeekHeight) / _dayHeight;
                int selectedDayIndex = rowNumber * 7 + colNumber;

                DateTime date = _daysList[selectedDayIndex].Date;
                List<PersonsEventsList> peDayList = Utility.GetDayPersonEvents(_selectedProfile.GUID, date);
                if (peDayList.Count > 0)
                {
                    if ((e.X > _dayWidth + colNumber * _dayWidth - 10) && (e.X < _dayWidth + colNumber * _dayWidth) &&
                        (e.Y > _daysOfWeekHeight + rowNumber * _dayHeight) && (e.Y < _daysOfWeekHeight + rowNumber * _dayHeight + 10)
                        )
                    {
                        int posX = colNumber * _dayWidth + _dayWidth - 255;
                        int posY = ((rowNumber + 1) * _dayHeight - _dayHeight) + _daysOfWeekHeight + 5;

                        if (posX < 0)
                            posX = 5;

                        if ((posY + 50) > pictureBoxCalendar.Height)
                            posY = posY + _dayHeight - 50 - 10;

                        PersonEventToolTipCreation(peDayList);
                        toolTip.Show(pictureBoxCalendar, posX, posY);
                    }
                }
            }
        }

        private void pictureBoxCalendar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null || e.Y < _daysOfWeekHeight || e.Y > (_daysOfWeekHeight + 6 * _dayHeight) - 1)
                return;

            int colNumber = e.X / _dayWidth;
            int rowNumber = (e.Y - _daysOfWeekHeight) / _dayHeight;
            int selectedDayIndex = rowNumber * 7 + colNumber;

            DateTime date = _daysList[selectedDayIndex].Date;
            List<PersonsEventsList> peDayList = Utility.GetDayPersonEvents(_selectedProfile.GUID, date);
            if (peDayList.Count > 0)
            {
                if ((e.X > _dayWidth + colNumber * _dayWidth - 10) && (e.X < _dayWidth + colNumber * _dayWidth) &&
                        (e.Y > _daysOfWeekHeight + rowNumber * _dayHeight) && (e.Y < _daysOfWeekHeight + rowNumber * _dayHeight + 10)
                        )
                    Cursor.Current = Cursors.Hand;
                else
                    Cursor.Current = Cursors.Default;
            }
        }

        private void pictureBoxCalendar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null || e.Y < _daysOfWeekHeight)
                return;

            int colNumber = e.X / _dayWidth;
            int rowNumber = (e.Y - _daysOfWeekHeight) / _dayHeight;
            int selectedDayIndex = rowNumber * 7 + colNumber;

            OpenDayTab(selectedDayIndex);
        }

        private void pictureBoxTranzits_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null)
                return;

            int labelsWidth = pictureBoxTranzits.Width - _dayTranzWidth * _daysOfMonth.Count - 1;
            int day = (e.X - labelsWidth) / _dayTranzWidth;
            DateTime selectedDate = _daysOfMonth[day].Date;
            int selectedDayIndex = _daysList.FindIndex(i => i.Date == selectedDate);
            OpenDayTab(selectedDayIndex);
        }

        private enum TranzitEntity { TEEmpty, TENakshatra, TETaraBala, TETithi, TEKarana, TENityaYoga, TEYoga, TEMoon, TESun, TEVenus, TEJupiter, TEMercury, TEMars, TESaturn, TERahu, TEKetu }

        private void pictureBoxTranzits_MouseClick(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null || _daysList == null)
                return;

            if (e.Button == MouseButtons.Right)
            {
                int labelsWidth = pictureBoxTranzits.Width - _dayTranzWidth * _daysOfMonth.Count - 1;
                int posX = labelsWidth, posY = 0;
                int dayHeight = pictureBoxTranzits.Height - 1;

                int day = (e.X - labelsWidth) / _dayTranzWidth;
                //DateTime currentDate = _daysOfMonth[day].Date;

                // Draw blue rectangle for selected day through all lines
                pictureBoxTranzits.Image = _currentTranzitsImage;
                Bitmap canvas = new Bitmap(pictureBoxTranzits.Image);
                Graphics g = Graphics.FromImage(canvas);
                Rectangle selectRectangle = new Rectangle(posX + day * _dayTranzWidth, posY, _dayTranzWidth, dayHeight);
                Pen pen = new Pen(Color.FromArgb(CacheLoad._colorList.Where(i => i.Code.Equals(EColor.SELECTRECTANGLE.ToString())).FirstOrDefault().ARGBValue), 1);
                g.DrawRectangle(pen, selectRectangle);
                pictureBoxTranzits.Image = canvas;

                // Finding line by position
                int posYPanchanga = posY + 2 * _lineTranzHeight + 4;
                int posYYoga = posYPanchanga + 6 * _lineTranzHeight + 4;
                int posYMoon = posYYoga + _lineTranzHeight + 4;
                int posYSun = posYMoon + 4 * _lineTranzHeight + 4;
                int posYVenus = posYSun + 4 * _lineTranzHeight + 4;
                int posYJupiter = posYVenus + 4 * _lineTranzHeight + 4;
                int posYMercury = posYJupiter + 4 * _lineTranzHeight + 4;
                int posYMars = posYMercury + 4 * _lineTranzHeight + 4;
                int posYSaturn = posYMars + 4 * _lineTranzHeight + 4;
                int posYRahu = posYSaturn + 4 * _lineTranzHeight + 4;
                int posYKetu = posYRahu + 4 * _lineTranzHeight + 4;

                TranzitEntity trEnt = TranzitEntity.TEEmpty;
                if (e.Y > posYPanchanga && e.Y < posYPanchanga + _lineTranzHeight)
                    trEnt = TranzitEntity.TENakshatra;
                if (e.Y > posYPanchanga + _lineTranzHeight && e.Y < posYPanchanga + 2 * _lineTranzHeight)
                    trEnt = TranzitEntity.TETaraBala;
                if (e.Y > posYPanchanga + 2 * _lineTranzHeight && e.Y < posYPanchanga + 3 * _lineTranzHeight)
                    trEnt = TranzitEntity.TETithi;
                if (e.Y > posYPanchanga + 3 * _lineTranzHeight && e.Y < posYPanchanga + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TEKarana;
                if (e.Y > posYPanchanga + 4 * _lineTranzHeight && e.Y < posYPanchanga + 5 * _lineTranzHeight)
                    trEnt = TranzitEntity.TENityaYoga;
                if (e.Y > posYYoga && e.Y < posYYoga + _lineTranzHeight)
                    trEnt = TranzitEntity.TEYoga;
                if (e.Y > posYMoon && e.Y < posYMoon + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TEMoon;
                if (e.Y > posYSun && e.Y < posYSun + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TESun;
                if (e.Y > posYVenus && e.Y < posYVenus + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TEVenus;
                if (e.Y > posYJupiter && e.Y < posYJupiter + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TEJupiter;
                if (e.Y > posYMercury && e.Y < posYMercury + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TEMercury;
                if (e.Y > posYMars && e.Y < posYMars + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TEMars;
                if (e.Y > posYSaturn && e.Y < posYSaturn + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TESaturn;
                if (e.Y > posYRahu && e.Y < posYRahu + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TERahu;
                if (e.Y > posYKetu && e.Y < posYKetu + 4 * _lineTranzHeight)
                    trEnt = TranzitEntity.TEKetu;

                // Show info for selected tranzit line
                int ttposX = 0, ttposY = 0, height = 0;
                int formWidth = pictureBoxTranzits.Width;
                switch (trEnt)
                {
                    case TranzitEntity.TENakshatra:
                        TranzitsTooltipCreation(_daysOfMonth[day].NakshatraDayList.ToList(), formWidth, out height, TranzitEntity.TENakshatra, _activeLanguageCode);
                        break;

                    case TranzitEntity.TETaraBala:
                        TranzitsTooltipCreation(_daysOfMonth[day].TaraBalaDayList.ToList(), formWidth, out height, TranzitEntity.TETaraBala, _activeLanguageCode);
                        break;

                    case TranzitEntity.TETithi:
                        TranzitsTooltipCreation(_daysOfMonth[day].TithiDayList.ToList(), formWidth, out height, TranzitEntity.TETithi, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEKarana:
                        TranzitsTooltipCreation(_daysOfMonth[day].KaranaDayList.ToList(), formWidth, out height, TranzitEntity.TEKarana, _activeLanguageCode);
                        break;

                    case TranzitEntity.TENityaYoga:
                        TranzitsTooltipCreation(_daysOfMonth[day].NityaJogaDayList.ToList(), formWidth, out height, TranzitEntity.TENityaYoga, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEYoga:
                        YogaTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TEYoga, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEMoon:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TEMoon, _activeLanguageCode);
                        break;

                    case TranzitEntity.TESun:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TESun, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEVenus:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TEVenus, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEJupiter:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TEJupiter, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEMercury:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TEMercury, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEMars:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TEMars, _activeLanguageCode);
                        break;

                    case TranzitEntity.TESaturn:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TESaturn, _activeLanguageCode);
                        break;

                    case TranzitEntity.TERahu:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TERahu, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEKetu:
                        PlanetTranzitTooltipCreation(_daysOfMonth[day], formWidth, out height, TranzitEntity.TEKetu, _activeLanguageCode);
                        break;

                    case TranzitEntity.TEEmpty:
                        if (toolTip != null)
                        {
                            toolTip.Dispose();
                            toolTip = null;
                        }
                        break;
                }
                if (toolTip != null)
                {
                    ttposY = pictureBoxTranzits.Height - (height + 4);
                    toolTip.Show(pictureBoxTranzits, ttposX, ttposY);
                }
            }
        }

        private void PlanetTranzitTooltipCreation(Day pDay, int width, out int height, TranzitEntity tEnt, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = PreparePlanetDataGridView(tEnt, pDay, width, out height, langCode);
            
            toolTip = new Popup(tranzitsToolTip = new TranzitsToolTip(dgv, width, (height + 5)));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
            toolTip.AutoClose = true;
        }

        private DataGridView PreparePlanetDataGridView(TranzitEntity tEnt, Day pDay, int width, out int height, ELanguage langCode)
        {
            height = 0;
            int planetId = 0;
            string planetName = string.Empty;
            switch (tEnt)
            {
                
                case TranzitEntity.TEMoon:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.MOON.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TESun:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.SUN.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TEVenus:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.VENUS.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TEJupiter:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.JUPITER.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TEMercury:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.MERCURY.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TEMars:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.MARS.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TESaturn:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.SATURN.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TERahu:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.RAHUMEAN.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;

                case TranzitEntity.TEKetu:
                    planetId = CacheLoad._planetList.Where(i => i.Code.Equals(EPlanet.KETUMEAN.ToString())).FirstOrDefault()?.Id ?? 0;
                    planetName = CacheLoad._planetDescList.Where(i => i.PlanetId == planetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault().Name.Substring(0, 2);
                    break;
            }

            //DGV build
            DataGridView dgv = new DataGridView();
            dgv.Width = width;
            dgv.AutoGenerateColumns = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
            dgv.DefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPTEXT));

            DataGridViewColumn column = new DataGridViewColumn();
            column.DataPropertyName = "Tranzit";
            column.Name = Utility.GetLocalizedText("Tranzit", langCode) + " " + planetName;
            column.Width = 120;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "DateStart";
            column.Name = Utility.GetLocalizedText("Start", langCode);
            column.Width = 130;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "DateEnd";
            column.Name = Utility.GetLocalizedText("End", langCode);
            column.Width = 130;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Zodiac";
            column.Name = Utility.GetLocalizedText("Zodiac", langCode);
            column.Width = 80;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Nakshatra";
            column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
            column.Width = 160;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Vedha";
            column.Name = Utility.GetLocalizedText("Vedha from", langCode);
            column.Width = 330;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            int lastColWidth = (dgv.Width - 950);
            column = new DataGridViewColumn();
            column.DataPropertyName = "Description";
            column.Name = Utility.GetLocalizedText("Description", langCode);
            column.Width = lastColWidth;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            List<PlanetCalendar> clonedZodiakCal = null;
            List<PlanetCalendar> clonedZodiakRetroCal = null;
            List<PlanetCalendar> clonedNakshatraCal = null;

            EAppSetting tranzitSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;

            switch (tEnt)
            {
                case TranzitEntity.TEMoon:
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MoonZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MoonZodiakRetroDayList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MoonZodiakLagnaDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MoonZodiakRetroLagnaDayList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MoonZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MoonZodiakRetroDayList);
                            break;
                    }
                    clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.MoonNakshatraDayList);
                    break;

                case TranzitEntity.TESun:
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.SunZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.SunZodiakRetroDayList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.SunZodiakLagnaDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.SunZodiakRetroLagnaDayList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.SunZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.SunZodiakRetroDayList);
                            break;
                    }
                    clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.SunNakshatraDayList);
                    break;

                case TranzitEntity.TEVenus:
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.VenusZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.VenusZodiakRetroDayList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.VenusZodiakLagnaDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.VenusZodiakRetroLagnaDayList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.VenusZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.VenusZodiakRetroDayList);
                            break;
                    }
                    clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.VenusNakshatraDayList);
                    break;

                case TranzitEntity.TEJupiter:
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.JupiterZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.JupiterZodiakRetroDayList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.JupiterZodiakLagnaDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.JupiterZodiakRetroLagnaDayList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.JupiterZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.JupiterZodiakRetroDayList);
                            break;
                    }
                    clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.JupiterNakshatraDayList);
                    break;

                case TranzitEntity.TEMercury:
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MercuryZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MercuryZodiakRetroDayList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MercuryZodiakLagnaDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MercuryZodiakRetroLagnaDayList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MercuryZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MercuryZodiakRetroDayList);
                            break;
                    }
                    clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.MercuryNakshatraDayList);
                    break;

                case TranzitEntity.TEMars:
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MarsZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MarsZodiakRetroDayList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MarsZodiakLagnaDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MarsZodiakRetroLagnaDayList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.MarsZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.MarsZodiakRetroDayList);
                            break;
                    }
                    clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.MarsNakshatraDayList);
                    break;

                case TranzitEntity.TESaturn:
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.SaturnZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.SaturnZodiakRetroDayList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.SaturnZodiakLagnaDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.SaturnZodiakRetroLagnaDayList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.SaturnZodiakDayList);
                            clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.SaturnZodiakRetroDayList);
                            break;
                    }
                    clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.SaturnNakshatraDayList);
                    break;

                case TranzitEntity.TERahu:
                    switch (nodeSettings)
                    {
                        case EAppSetting.NODEMEAN:
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanZodiakRetroDayList);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanZodiakLagnaDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanZodiakRetroLagnaDayList);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanZodiakRetroDayList);
                                    break;
                            }
                            clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanNakshatraDayList);
                            break;

                        case EAppSetting.NODETRUE:
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.RahuTrueZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.RahuTrueZodiakRetroDayList);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.RahuTrueZodiakLagnaDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.RahuTrueZodiakRetroLagnaDayList);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.RahuTrueZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.RahuTrueZodiakRetroDayList);
                                    break;
                            }
                            clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.RahuTrueNakshatraDayList);
                            break;
                    }
                    break;

                case TranzitEntity.TEKetu:
                    switch (nodeSettings)
                    {
                        case EAppSetting.NODEMEAN:
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanZodiakRetroDayList);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanZodiakLagnaDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanZodiakRetroLagnaDayList);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanZodiakRetroDayList);
                                    break;
                            }
                            clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanNakshatraDayList);
                            break;

                        case EAppSetting.NODETRUE:
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.KetuTrueZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.KetuTrueZodiakRetroDayList);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.KetuTrueZodiakLagnaDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.KetuTrueZodiakRetroLagnaDayList);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    clonedZodiakCal = Utility.ClonePlanetCalendarList(pDay.KetuTrueZodiakDayList);
                                    clonedZodiakRetroCal = Utility.ClonePlanetCalendarList(pDay.KetuTrueZodiakRetroDayList);
                                    break;
                            }
                            clonedNakshatraCal = Utility.ClonePlanetCalendarList(pDay.KetuTrueNakshatraDayList);
                            break;
                    }
                    break;
            }

            dgv = PlanetTranzitDataGridViewFillByRow(dgv, pDay, clonedZodiakCal, clonedZodiakRetroCal, clonedNakshatraCal, tranzitSetting, nodeSettings, langCode);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            //dgv.Sort(dgv.Columns[Utility.GetLocalizedText("Start", langCode)], System.ComponentModel.ListSortDirection.Ascending);

            for (int i = 0; i < dgv.RowCount; i++)
            {
                int rowHeight = dgv.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                height += rowHeight;
            }
            return dgv;
        }

        public struct dgvRowObj
        {
            public string Entity { get; set; }
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
            public string Zodiac { get; set; }
            public string Nakshatra { get; set; }
            public string Vedha { get; set; }
            public string Description { get; set; }
        }

        private DataGridView PlanetTranzitDataGridViewFillByRow(DataGridView dgv, Day pDay, List<PlanetCalendar> pzList, List<PlanetCalendar> pzrList, List<PlanetCalendar> pnList, EAppSetting tranzitSetting, EAppSetting nodeSettings, ELanguage langCode)
        {
            List<dgvRowObj> rowList = new List<dgvRowObj>();
            foreach (PlanetCalendar pc in pzrList)
            {
                string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == pc.Dom).FirstOrDefault();
                string trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                string vedha = GetVedhaList(pDay, pc, tr, tranzitSetting, nodeSettings, langCode, false);
                dgvRowObj rowTemp = new dgvRowObj {
                    Entity = Utility.GetLocalizedText("Zodiac", langCode),
                    DateStart = pc.DateStart,
                    DateEnd = pc.DateEnd,
                    Zodiac = zodiak,
                    Nakshatra = string.Empty,
                    Vedha = vedha,
                    Description = trDesc
                };
                rowList.Add(rowTemp);
            }
            foreach (PlanetCalendar pc in pnList)
            {
                string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                string nakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)pc.NakshatraCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                dgvRowObj rowTemp = new dgvRowObj
                {
                    Entity = Utility.GetLocalizedText("Nakshatra", langCode),
                    DateStart = pc.DateStart,
                    DateEnd = pc.DateEnd,
                    Zodiac = zodiak,
                    Nakshatra = (int)pc.NakshatraCode + "." + nakshatra,
                    Vedha = string.Empty,
                    Description = string.Empty
                };
                rowList.Add(rowTemp);
            }
            List<dgvRowObj> rowListSorted = rowList.OrderBy(i => i.DateStart).ToList();
            for (int i = 0; i < rowListSorted.Count; i++)
            {
                string[] row = new string[] {
                        rowListSorted[i].Entity,
                        rowListSorted[i].DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        rowListSorted[i].DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        rowListSorted[i].Zodiac,
                        rowListSorted[i].Nakshatra,
                        rowListSorted[i].Vedha,
                        rowListSorted[i].Description
                    };
                dgv.Rows.Add(row);
            }
            return dgv;
        }

        private string GetVedhaList(Day pDay, PlanetCalendar pc, Tranzit tr, EAppSetting tranzitSetting, EAppSetting nodeSettings, ELanguage lCode, bool isLagna)
        {
            string vedha = string.Empty;
            if (!tr.Vedha.Equals(string.Empty))
            {
                // Make a list of vedha planets to add into desc2
                List<VedhaEntity> vedhaList = null;
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        vedhaList = Utility.PrepareVedhaPlanetList(pDay, pc, Convert.ToInt32(tr.Vedha), false);
                        break;
                    case EAppSetting.TRANZITLAGNA:
                        vedhaList = Utility.PrepareVedhaPlanetList(pDay, pc, Convert.ToInt32(tr.Vedha), true);
                        break;
                    case EAppSetting.TRANZITMOONANDLAGNA:
                        vedhaList = Utility.PrepareVedhaPlanetList(pDay, pc, Convert.ToInt32(tr.Vedha), false);
                        break;
                }

                string planetList = string.Empty;
                if (vedhaList.Count > 0)
                {
                    switch (nodeSettings)
                    {
                        case EAppSetting.NODEMEAN:
                            for (int i = vedhaList.Count - 1; i > -1; i--)
                            {
                                if (vedhaList[i].PlanetCode == EPlanet.RAHUTRUE || vedhaList[i].PlanetCode == EPlanet.KETUTRUE)
                                {
                                    vedhaList.RemoveAt(i);
                                }
                            }
                            break;
                        case EAppSetting.NODETRUE:
                            for (int i = vedhaList.Count - 1; i > -1; i--)
                            {
                                if (vedhaList[i].PlanetCode == EPlanet.RAHUMEAN || vedhaList[i].PlanetCode == EPlanet.KETUMEAN)
                                {
                                    vedhaList.RemoveAt(i);
                                }
                            }
                            break;
                    }
                    // Combine vedha string
                    foreach (VedhaEntity ve in vedhaList)
                    {
                        string planet = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)ve.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string period = "(" + ve.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + ve.DateEnd.ToString("dd.MM.yyyy HH:mm:ss") + ")";
                        planetList += planet + " " + period + ", ";
                    }
                    vedha = planetList.Substring(0, (planetList.Length - 2));
                }
            }
            return vedha;
        }

        private void YogaTranzitTooltipCreation(Day pDay, int width, out int height, TranzitEntity tEnt, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = PrepareYogaDataGridView(pDay, width, out height, langCode);
            if (dgv != null)
            {
                toolTip = new Popup(tranzitsToolTip = new TranzitsToolTip(dgv, width, (height + 5)));
                toolTip.AutoClose = false;
                toolTip.FocusOnOpen = false;
                toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
                toolTip.AutoClose = true;
            }
        }

        private DataGridView PrepareYogaDataGridView(Day day, int width, out int height, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = null;
            int yogaCount = GetYogasCount(day);

            if (yogaCount > 0)
            {
                //DGV build
                dgv = new DataGridView();
                dgv.Width = width;
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
                dgv.DefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPTEXT));

                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = "DateStart";
                column.Name = Utility.GetLocalizedText("Start", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateEnd";
                column.Name = Utility.GetLocalizedText("End", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Yoga";
                column.Name = Utility.GetLocalizedText("Yoga", langCode);
                column.Width = 200;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Vara";
                column.Name = Utility.GetLocalizedText("Vara", langCode);
                column.Width = 100;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Nakshatra";
                column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
                column.Width = 150;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Tithi";
                column.Name = Utility.GetLocalizedText("Tithi", langCode);
                column.Width = 150;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                int lastColWidth = (dgv.Width - 860);
                column = new DataGridViewColumn();
                column.DataPropertyName = "Description";
                column.Name = Utility.GetLocalizedText("Description", langCode);
                column.Width = lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                List<DGVYogaRow> dgvRowsList = new List<DGVYogaRow>();
                if (day.DwipushkarJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.DwipushkarJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.TripushkarJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.TripushkarJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.AmritaSiddhaJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.AmritaSiddhaJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.SarvarthaSiddhaJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.SarvarthaSiddhaJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.SiddhaJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.SiddhaJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.MrityuJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.MrityuJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.AdhamJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.AdhamJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.YamaghataJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.YamaghataJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.DagdhaJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.DagdhaJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }
                if (day.UnfarobaleJogaDayList.Count > 0)
                {
                    List<DGVYogaRow> rowsList = GetDGVRowFromYogaTime(day.Date, day.UnfarobaleJogaDayList.ToList(), langCode);
                    dgvRowsList.AddRange(rowsList);
                }

                List<DGVYogaRow> dgvRowsListSorted = dgvRowsList.OrderBy(i => i.DateStart).ToList();
                foreach (DGVYogaRow row in dgvRowsListSorted)
                {
                    string[] rowText = new string[] {
                        row.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        row.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        row.Name,
                        row.Vara,
                        row.Nakshatra,
                        row.Tithi,
                        row.Description
                    };
                    dgv.Rows.Add(rowText);
                }

                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

                for (int i = 0; i < dgv.RowCount; i++)
                {
                    int rowHeight = dgv.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                    height += rowHeight;
                }
            }

            return dgv;
        }

        private class DGVYogaRow
        {
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
            public string Name { get; set; }
            public string Vara { get; set; }
            public string Nakshatra { get; set; }
            public string Tithi { get; set; }
            public string Description { get; set; }
        }

        private List<DGVYogaRow> GetDGVRowFromYogaTime(DateTime date, List<Calendar> jcList, ELanguage lang)
        {
            List<DGVYogaRow> rowsList = new List<DGVYogaRow>();
            foreach (JogaCalendar jc in jcList)
            {
                if (jc.DateStart < date.AddDays(+1))
                {
                    DGVYogaRow row = new DGVYogaRow
                    {
                        DateStart = jc.DateStart,
                        DateEnd = jc.DateEnd,
                        Name = CacheLoad._jogaDescList.Where(i => i.JogaId == (int)jc.JogaCode && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty,
                        Vara = Utility.GetDaysOfWeekName((int)jc.Vara, lang),
                        Nakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)jc.NakshatraCode && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty,
                        Tithi = CacheLoad._tithiDescList.Where(i => i.TithiId == jc.TithiId && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty,
                        Description = CacheLoad._jogaDescList.Where(i => i.JogaId == (int)jc.JogaCode && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Description ?? string.Empty
                    };
                    rowsList.Add(row);
                }
            }
            return rowsList;
        }

        private void CalendarTooltipCreation(Profile pers, Day pDay, int formHeight, int linesCount, ELanguage langCode)
        {
            toolTip = new Popup(calendarToolTip = new CalendarToolTip(pers, pDay, formHeight, linesCount, langCode));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
            toolTip.AutoClose = true;
        }

        private void TranzitsTooltipCreation(List<Calendar> cList, int width, out int height, TranzitEntity tEnt, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = null;

            Font headerFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
            Font textFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPTEXT));

            switch (tEnt)
            {
                case TranzitEntity.TENakshatra:
                    dgv = PrepareNakshatraDataGridView(cList, width, out height, headerFont, textFont, langCode);
                    break;

                case TranzitEntity.TETaraBala:
                    dgv = PrepareTaraBalaDataGridView(cList, width, out height, headerFont, textFont, langCode);
                    break;

                case TranzitEntity.TETithi:
                    dgv = PrepareTithiDataGridView(cList, width, out height, headerFont, textFont, langCode);
                    break;

                case TranzitEntity.TEKarana:
                    dgv = PrepareKaranaDataGridView(cList, width, out height, headerFont, textFont, langCode);
                    break;

                case TranzitEntity.TENityaYoga:
                    dgv = PrepareNityaYogaDataGridView(cList, width, out height, headerFont, textFont, langCode);
                    break;
            }

            toolTip = new Popup(tranzitsToolTip = new TranzitsToolTip(dgv, width, (height + 5)));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
            toolTip.AutoClose = true;
        }

        private DataGridView PrepareNakshatraDataGridView(List<Calendar> cList, int width, out int height, Font headerFont, Font textFont, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = new DataGridView();
            dgv.Width = width;

            if (cList != null)
            {
                //DGV build
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = headerFont;
                dgv.DefaultCellStyle.Font = textFont;

                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = "DateStart";
                column.Name = Utility.GetLocalizedText("Start", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateEnd";
                column.Name = Utility.GetLocalizedText("End", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Nakshatra";
                column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
                column.Width = 160;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Nature";
                column.Name = Utility.GetLocalizedText("Nature", langCode);
                column.Width = 120;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                int lastColWidth = (dgv.Width - 520) / 3;
                column = new DataGridViewColumn();
                column.DataPropertyName = "Description";
                column.Name = Utility.GetLocalizedText("Description", langCode);
                column.Width = lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "GoodFor";
                column.Name = Utility.GetLocalizedText("Good for", langCode);
                column.Width = dgv.Width - 540 - 2 * lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "BadFor";
                column.Name = Utility.GetLocalizedText("Bad for", langCode);
                column.Width = lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                NakshatraDescription currentNakshatra;
                foreach (NakshatraCalendar nc in cList)
                {
                    currentNakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)nc.NakshatraCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault();
                    string[] row = new string[] {
                        nc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        nc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        currentNakshatra.NakshatraId + "." + currentNakshatra.Name + " (" + currentNakshatra.Upravitel + ")",
                        currentNakshatra.Nature,
                        currentNakshatra.Description,
                        currentNakshatra.GoodFor,
                        currentNakshatra.BadFor
                    };
                    dgv.Rows.Add(row);
                }
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    int rowHeight = dgv.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                    height += rowHeight;
                }
            }

            return dgv;
        }

        public DataGridView PrepareTaraBalaDataGridView(List<Calendar> cList, int width, out int height, Font headerFont, Font textFont, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = new DataGridView();
            dgv.Width = width;

            if (cList != null)
            {
                //DGV build
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = headerFont;
                dgv.DefaultCellStyle.Font = textFont;

                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = "DateStart";
                column.Name = Utility.GetLocalizedText("Start", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateEnd";
                column.Name = Utility.GetLocalizedText("End", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "TaraBala";
                column.Name = Utility.GetLocalizedText("Tara Bala", langCode);
                column.Width = 140;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Percent";
                column.Name = "%";
                column.Width = 40;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                int lastColWidth = (dgv.Width - 440);
                column = new DataGridViewColumn();
                column.DataPropertyName = "Description";
                column.Name = Utility.GetLocalizedText("Description", langCode);
                column.Width = lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                TaraBalaDescription currentTaraBala;
                foreach (TaraBalaCalendar tbc in cList)
                {
                    currentTaraBala = CacheLoad._taraBalaDescList.Where(i => i.TaraBalaId == tbc.TaraBalaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault();
                    string[] row = new string[] {
                        tbc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        tbc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        currentTaraBala.TaraBalaId + "." + currentTaraBala.Name,
                        tbc.Percent.ToString(),
                        currentTaraBala.Description
                    };
                    dgv.Rows.Add(row);
                }
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

                for (int i = 0; i < dgv.RowCount; i++)
                {
                    int rowHeight = dgv.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                    height += rowHeight;
                }
            }

            return dgv;
        }

        public DataGridView PrepareTithiDataGridView(List<Calendar> cList, int width, out int height, Font headerFont, Font textFont, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = new DataGridView();
            dgv.Width = width;

            if (cList != null)
            {
                //DGV build
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = headerFont;
                dgv.DefaultCellStyle.Font = textFont;

                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = "DateStart";
                column.Name = Utility.GetLocalizedText("Start", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateEnd";
                column.Name = Utility.GetLocalizedText("End", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Tithi";
                column.Name = Utility.GetLocalizedText("Tithi", langCode);
                column.Width = 160;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Type";
                column.Name = Utility.GetLocalizedText("Type", langCode);
                column.Width = 140;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                int lastColWidth = (dgv.Width - 540) / 2;
                column = new DataGridViewColumn();
                column.DataPropertyName = "GoodFor";
                column.Name = Utility.GetLocalizedText("Good for", langCode);
                column.Width = dgv.Width - 560 - lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "BadFor";
                column.Name = Utility.GetLocalizedText("Bad for", langCode);
                column.Width = lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                TithiDescription currentTithi;
                foreach (TithiCalendar tc in cList)
                {
                    currentTithi = CacheLoad._tithiDescList.Where(i => i.TithiId == (int)tc.TithiId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault();
                    string[] row = new string[] {
                        tc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        tc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        currentTithi.TithiId + "." + currentTithi.Name + " (" + currentTithi.Upravitel + ")",
                        currentTithi.Type,
                        currentTithi.GoodFor,
                        currentTithi.BadFor
                    };
                    dgv.Rows.Add(row);
                }
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

                for (int i = 0; i < dgv.RowCount; i++)
                {
                    int rowHeight = dgv.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                    height += rowHeight;
                }
            }

            return dgv;
        }

        public DataGridView PrepareKaranaDataGridView(List<Calendar> cList, int width, out int height, Font headerFont, Font textFont, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = new DataGridView();
            dgv.Width = width;

            if (cList != null)
            {
                //DGV build
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = headerFont;
                dgv.DefaultCellStyle.Font = textFont;

                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = "DateStart";
                column.Name = Utility.GetLocalizedText("Start", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateEnd";
                column.Name = Utility.GetLocalizedText("End", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Karana";
                column.Name = Utility.GetLocalizedText("Karana", langCode);
                column.Width = 160;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                int lastColWidth = (dgv.Width - 400) / 2;
                column = new DataGridViewColumn();
                column.DataPropertyName = "GoodFor";
                column.Name = Utility.GetLocalizedText("Good for", langCode);
                column.Width = dgv.Width - 420 - lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "BadFor";
                column.Name = Utility.GetLocalizedText("Bad for", langCode);
                column.Width = lastColWidth;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                KaranaDescription currentKarana;
                foreach (KaranaCalendar kc in cList)
                {
                    currentKarana = CacheLoad._karanaDescList.Where(i => i.KaranaId == kc.KaranaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault();
                    string[] row = new string[] {
                        kc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        kc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        currentKarana.KaranaId + "." + currentKarana.Name + " (" + currentKarana.Upravitel + ")",
                        currentKarana.GoodFor,
                        currentKarana.BadFor
                    };
                    dgv.Rows.Add(row);
                }
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

                for (int i = 0; i < dgv.RowCount; i++)
                {
                    int rowHeight = dgv.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                    height += rowHeight;
                }
            }

            return dgv;
        }

        private DataGridView PrepareNityaYogaDataGridView(List<Calendar> cList, int width, out int height, Font headerFont, Font textFont, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = new DataGridView();
            dgv.Width = width;

            if (cList != null)
            {
                //DGV build
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = headerFont;
                dgv.DefaultCellStyle.Font = textFont;

                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = "DateStart";
                column.Name = Utility.GetLocalizedText("Start", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateEnd";
                column.Name = Utility.GetLocalizedText("End", langCode);
                column.Width = 130;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "NityaYoga";
                column.Name = Utility.GetLocalizedText("Nitya Yoga", langCode);
                column.Width = 160;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Nakshatra";
                column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
                column.Width = 160;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Meaning";
                column.Name = Utility.GetLocalizedText("Meaning", langCode);
                column.Width = 160;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Description";
                column.Name = Utility.GetLocalizedText("Description", langCode);
                column.Width = dgv.Width - 740;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                NityaJogaDescription currentNityaYoga;
                foreach (NityaJogaCalendar njc in cList)
                {
                    currentNityaYoga = CacheLoad._nityaJogaDescList.Where(i => i.NityaJogaId == (int)njc.NJCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault();
                    int yogaPlanetId = CacheLoad._nityaJogaList.Where(i => i.Id == (int)njc.NJCode).FirstOrDefault()?.JogiPlanetId ?? 0;
                    string upravitel = CacheLoad._planetDescList.Where(i => i.PlanetId == yogaPlanetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    string nakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == njc.NakshatraId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    string[] row = new string[] {
                        njc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        njc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        currentNityaYoga.NityaJogaId + "." + currentNityaYoga.Name + " (" + upravitel + ", " + currentNityaYoga.Deity + ")",
                        njc.NakshatraId + "." + nakshatra,
                        currentNityaYoga.Meaning,
                        currentNityaYoga.Description
                    };
                    dgv.Rows.Add(row);
                }
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

                for (int i = 0; i < dgv.RowCount; i++)
                {
                    int rowHeight = dgv.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                    height += rowHeight;
                }
            }

            return dgv;
        }

        private void PersonEventToolTipCreation(List<PersonsEventsList> peList)
        {
            // still problem for auto close when leaving day
            Font dateFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.PEVTOOLTIPDATE)), 10, Utility.GetFontStyleBySettings(EFontList.PEVTOOLTIPDATE));
            Font timeFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.PEVTOOLTIPTIME)), 8, Utility.GetFontStyleBySettings(EFontList.PEVTOOLTIPTIME));
            Font textFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.PEVTOOLTIPTEXT)), 8, Utility.GetFontStyleBySettings(EFontList.PEVTOOLTIPTEXT));

            //Font dateFont = new Font("Segoe UI", 10, FontStyle.Bold);
            //Font timeFont = new Font("Segoe UI", 8, FontStyle.Regular);
            //Font textFont = new Font("Segoe UI", 8, FontStyle.Italic);
            int formWidth = 250, formHeight = 0;

            string title = peList[0].DateStart.ToString("dd.MM.yyyy");
            Size textSize = TextRenderer.MeasureText(title, dateFont);
            if (textSize.Width > (formWidth - 8))
            {
                int linesCount = textSize.Width / (formWidth - 8) + 1;
                formHeight += (textSize.Height * linesCount) + 4;
            }
            else
            {
                formHeight += textSize.Height + 4;
            }

            foreach (PersonsEventsList pe in peList)
            {
                string tPeriod = Utility.GetLocalizedText("Time period", _activeLanguageCode) + ": " + pe.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + pe.DateEnd.ToString("dd.MM.yyyy HH:mm:ss");
                string desc1 = pe.Name;

                textSize = TextRenderer.MeasureText(tPeriod, timeFont);
                formHeight += textSize.Height + 4;
                if (desc1.Equals(string.Empty))
                {
                    textSize = TextRenderer.MeasureText(desc1, textFont);
                    if (textSize.Width > (formWidth - 8))
                    {
                        int linesCount = textSize.Width / (formWidth - 8) + 2;
                        formHeight += (textSize.Height * linesCount) + 4;
                    }
                    else
                    {
                        formHeight += textSize.Height + 4;
                    }
                }
                formHeight += textSize.Height + 4;
            }
            formHeight += 4;

            toolTip = new Popup(personEventToolTip = new PersonEventTooltip(peList, formWidth, formHeight, dateFont, timeFont, textFont, _activeLanguageCode));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
            toolTip.AutoClose = true;
        }

        private void OpenDayTab(int selectedDayIndex)
        {
            int index = -1;
            bool isPresent = false;
            string tabLabel = _daysList[selectedDayIndex].Date.ToString("dd.MM.yyyy");

            for (int i = 0; i < tabControlCalendar.TabPages.Count; i++)
            {
                if (tabControlCalendar.TabPages[i].Text.Equals(tabLabel))
                {
                    isPresent = true;
                    index = i;
                    break;
                }
            }

            if (isPresent)
            {
                tabControlCalendar.SelectedIndex = index;
            }
            else
            {
                List<PersonsEventsList> pevList = Utility.GetDayPersonEvents(_selectedProfile.GUID, _daysList[selectedDayIndex].Date); 
                List<DVLineNameDescription> dvlDescList = CacheLoad._dvLineNamesDescList.Where(i => i.LanguageCode.Equals(_activeLanguageCode.ToString())).ToList();
                TabPage newTab = new TabPage() { Name = "day" + selectedDayIndex.ToString(), Text = tabLabel };
                TabDay tabForm = new TabDay(_daysList[selectedDayIndex],  _selectedProfile, CacheLoad._dvLineNamesList.ToList(), dvlDescList, pevList, _activeLanguageCode);
                tabForm.TopLevel = false;
                tabForm.Parent = newTab;
                tabControlCalendar.TabPages.Add(newTab);
                tabControlCalendar.SelectedTab = newTab;
                tabForm.Show();
                tabForm.Dock = DockStyle.Fill;
            }
        }

        private void yearsTranzitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = -1;
            bool isPresent = false;
            YearTranzitSelect ysForm = new YearTranzitSelect(_yearsList, _activeLanguageCode);
            ysForm.ShowDialog();
            int year = ysForm.SelectedYear;
            if (year != 0)
            {
                string tabLabel = Utility.GetLocalizedText("Year's tranzits", _activeLanguageCode) + ": " + year;
                for (int i = 0; i < tabControlCalendar.TabPages.Count; i++)
                {
                    if (tabControlCalendar.TabPages[i].Text.Equals(tabLabel))
                    {
                        isPresent = true;
                        index = i;
                        break;
                    }
                }

                if (isPresent)
                {
                    tabControlCalendar.SelectedIndex = index;
                }
                else
                {
                    if (_selectedProfile != null)
                    {
                        TabPage newTab = new TabPage() { Name = "yearTranzits" + ": " + year, Text = tabLabel };
                        YearTranzits tabForm = new YearTranzits(
                                year,
                                _activeLanguageCode,
                                _selectedProfile,
                                _moonZodiakCalendarList.ToList(),
                                _moonZodiakRetroCalendarList.ToList(),
                                _moonNakshatraCalendarList.ToList(),
                                _moonPadaCalendarList.ToList(),
                                _sunZodiakCalendarList.ToList(),
                                _sunZodiakRetroCalendarList.ToList(),
                                _sunNakshatraCalendarList.ToList(),
                                _sunPadaCalendarList.ToList(),
                                _mercuryZodiakCalendarList.ToList(),
                                _mercuryZodiakRetroCalendarList.ToList(),
                                _mercuryNakshatraCalendarList.ToList(),
                                _mercuryPadaCalendarList.ToList(),
                                _venusZodiakCalendarList.ToList(),
                                _venusZodiakRetroCalendarList.ToList(),
                                _venusNakshatraCalendarList.ToList(),
                                _venusPadaCalendarList.ToList(),
                                _marsZodiakCalendarList.ToList(),
                                _marsZodiakRetroCalendarList.ToList(),
                                _marsNakshatraCalendarList.ToList(),
                                _marsPadaCalendarList.ToList(),
                                _jupiterZodiakCalendarList.ToList(),
                                _jupiterZodiakRetroCalendarList.ToList(),
                                _jupiterNakshatraCalendarList.ToList(),
                                _jupiterPadaCalendarList.ToList(),
                                _saturnZodiakCalendarList.ToList(),
                                _saturnZodiakRetroCalendarList.ToList(),
                                _saturnNakshatraCalendarList.ToList(),
                                _saturnPadaCalendarList.ToList(),
                                _rahuMeanZodiakCalendarList.ToList(),
                                _rahuMeanZodiakRetroCalendarList.ToList(),
                                _rahuMeanNakshatraCalendarList.ToList(),
                                _rahuMeanPadaCalendarList.ToList(),
                                _ketuMeanZodiakCalendarList.ToList(),
                                _ketuMeanZodiakRetroCalendarList.ToList(),
                                _ketuMeanNakshatraCalendarList.ToList(),
                                _ketuMeanPadaCalendarList.ToList(),
                                _rahuTrueZodiakCalendarList.ToList(),
                                _rahuTrueZodiakRetroCalendarList.ToList(),
                                _rahuTrueNakshatraCalendarList.ToList(),
                                _rahuTruePadaCalendarList.ToList(),
                                _ketuTrueZodiakCalendarList.ToList(),
                                _ketuTrueZodiakRetroCalendarList.ToList(),
                                _ketuTrueNakshatraCalendarList.ToList(),
                                _ketuTruePadaCalendarList.ToList(),
                                _eclipseCalendarList.ToList()
                            );
                        tabForm.MFormAccess = this;
                        tabForm.TopLevel = false;
                        tabForm.Parent = newTab;
                        tabControlCalendar.TabPages.Add(newTab);
                        tabControlCalendar.SelectedTab = newTab;
                        tabForm.Show();
                        tabForm.Dock = DockStyle.Fill;
                    }
                }
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string year = string.Empty;
            string[] filesList = CheckForCalendarsUpdate();
            if (filesList != null)
            {
                for (int i = 0; i < filesList.Length; i++)
                    year += filesList[i].Substring(filesList[i].Length - 7, 4) + " ";

                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Calendars update for year", _activeLanguageCode) + " " + year + " " + Utility.GetLocalizedText("is available. Do you want to load it now?", _activeLanguageCode), Utility.GetLocalizedText("Information", _activeLanguageCode), enumMessageIcon.Information, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (WaitForm wForm = new WaitForm(SaveCalendarData, _activeLanguageCode))
                    {
                        wForm.ShowDialog(this);
                    }
                    SetMinMaxYears();
                    DeleteUpdates(filesList);
                    DialogResult dialogResultConfirm = frmShowMessage.Show(Utility.GetLocalizedText("Data loaded successfully. In order to use new data application has to be restarted. Do you want to restart application now?", _activeLanguageCode), Utility.GetLocalizedText("Confirmation", _activeLanguageCode), enumMessageIcon.Question, enumMessageButton.YesNo);
                    if (dialogResultConfirm == DialogResult.Yes)
                    {
                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                frmShowMessage.Show(Utility.GetLocalizedText("No updates found.", _activeLanguageCode), Utility.GetLocalizedText("Information", _activeLanguageCode), enumMessageIcon.Information, enumMessageButton.OK);
            }
        }

        void SaveCalendarData()
        {
            DBAdministration db = new DBAdministration();
            string[] filesList = CheckForCalendarsUpdate();
            for (int i = 0; i < filesList.Length; i++)
            {
                string[] rowsList = File.ReadAllLines(filesList[i]);
                db.UploadNewData(rowsList);
            }
        }

        private string[] CheckForCalendarsUpdate()
        {
            string[] filesList = Directory.GetFiles(@".\Update", "*.dt");
            if (filesList.Length > 0)
                return filesList;
            else
                return null;
        }

        private void DeleteUpdates(string[] filesList)
        {
            for (int i = 0; i < filesList.Length; i++)
                File.Delete(filesList[i]);
        }

        
    }
}
