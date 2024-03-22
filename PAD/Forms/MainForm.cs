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
using TimeZoneConverter;
//using Innovative.SolarCalculator;
using NodaTime;

namespace PAD
{
    public partial class MainForm : Form
    {
        private Image _closeImage;
        private Image _moonEclipseImage;
        private Image _sunEclipseImage;
        private Image _currentCalendarImage;
        private Image _currentTranzitsImage;
        private Image _currentYearTranzitsImage;

        private int _selectedYear;
        private DateTime _todayDate;
        private DateTime _selectedDate;
        private string[] _daysOfWeek;
        List<Day> _daysList;
        List<Day> _daysOfMonth;

        private ELanguage _activeLanguageCode;
        private Profile _selectedProfile;
        private PdfDocument _pdfDoc;

        private float _daysOfWeekHeight;
        private float _dayWidth;
        private float _dayHeight;
        private float _labelsWidth;
        private float _dayTranzWidth;
        private float _lineTranzHeight;

        Popup toolTip;
        CalendarToolTip calendarToolTip;
        TranzitsToolTip tranzitsToolTip;
        PersonEventTooltip personEventToolTip;

        private List<PlanetCalendar> _moonNakshatraCalendar;

        public DateTime MonthDate
        {
            get { return datePicker.Value; }
            set { datePicker.Value = value; }
        }

        public Image CurrentYearTranzitImage
        {
            get { return _currentYearTranzitsImage; }
            set { _currentYearTranzitsImage = value; }
        }

        public void SetTranzitFocus()
        {
            string tabLabel = Utility.GetLocalizedText("Transits", _activeLanguageCode);
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
            _currentYearTranzitsImage = null;

            _todayDate = DateTime.Now;
            _selectedDate = _todayDate;
            _selectedYear = _todayDate.Year;
            CacheEntitiesListsLoad();

            _activeLanguageCode = (ELanguage)(Utility.GetActiveLanguageCode(CacheLoad._appSettingList));
            _daysOfWeek = PrepareDaysOfWeekArray();

            tabControlCalendar.MakeDoubleBuffered(true);
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
            CacheLoad._profileList = CacheLoad.GetProfilesList();
            CacheLoad._personEventsList = CacheLoad.GetPersonsEventsList();
            CacheLoad._planetList = CacheLoad.GetPlanetsList();
            CacheLoad._planetDescList = CacheLoad.GetPlanetDescList();
            CacheLoad._zodiakList = CacheLoad.GetZodiaksList();
            CacheLoad._zodiakDescList = CacheLoad.GetZodiakDescList();
            CacheLoad._nakshatraList = CacheLoad.GetNakshatrasList();
            CacheLoad._nakshatraDescList = CacheLoad.GetNakshatraDescList();
            CacheLoad._padaList = CacheLoad.GetPadaList();
            CacheLoad._specNavamshaList = CacheLoad.GetSpecNavamshaList();
            CacheLoad._masaList = CacheLoad.GetMasaList();
            CacheLoad._masaDescList = CacheLoad.GetMasaDescList();
            CacheLoad._taraBalaList = CacheLoad.GetTaraBalaList();
            CacheLoad._taraBalaDescList = CacheLoad.GetTaraBalaDescList();
            CacheLoad._tithiList = CacheLoad.GetTithiList();
            CacheLoad._tithiDescList = CacheLoad.GetTithiDescList();
            CacheLoad._karanaList = CacheLoad.GetKaranaList();
            CacheLoad._karanaDescList = CacheLoad.GetKaranaDescList();
            CacheLoad._nityaYogaList = CacheLoad.GetNityaYogaList();
            CacheLoad._nityaYogaDescList = CacheLoad.GetNityaYogaDescList();
            CacheLoad._muhurtaList = CacheLoad.GetMuhurtaList();
            CacheLoad._muhurtaDescList = CacheLoad.GetMuhurtaDescList();
            CacheLoad._yogaList = CacheLoad.GetYogaList();
            CacheLoad._yogaDescList = CacheLoad.GetYogaDescList();
            CacheLoad._eclipseList = CacheLoad.GetEclipseList();
            CacheLoad._eclipseDescList = CacheLoad.GetEclipseDescList();
            CacheLoad._tranzitList = CacheLoad.GetTranzitList();
            CacheLoad._tranzitDescList = CacheLoad.GetTranzitDescList();
            CacheLoad._muhurta30List = CacheLoad.GetMuhurta30List();
            CacheLoad._muhurta30DescList = CacheLoad.GetMuhurta30DescList();
            CacheLoad._ghati60List = CacheLoad.GetGhati60List();
            CacheLoad._ghati60DescList = CacheLoad.GetGhati60DescList();
            CacheLoad._horaPlanetList = CacheLoad.MakeHoraPlanetList();
            CacheLoad._mrityuBhagaList = CacheLoad.GetMrityuBhagaList();
        }

        private List<ShunyaTithiCalendar> CreateShunyaTithiCalendarList(List<MasaCalendar> mcList, List<TithiCalendar> tcList)
        {
            List<ShunyaTithiCalendar> stcList = new List<ShunyaTithiCalendar>();
            List<MasaCalendar> mcClonedList = Utility.CloneMasaCalendarList(mcList);
            List<TithiCalendar> tcClonedList = Utility.CloneTithiCalendarList(tcList);
            foreach (MasaCalendar mc in mcClonedList)
            {
                foreach (TithiCalendar tc in tcClonedList)
                {
                    if (mc.MasaId == 1 && (tc.TithiId == 8 || tc.TithiId == 9 || tc.TithiId == 11 || tc.TithiId == 23 || tc.TithiId == 24 || tc.TithiId == 26) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 2 && (tc.TithiId == 12 || tc.TithiId == 27) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 3 && (tc.TithiId == 13 || tc.TithiId == 28 || tc.TithiId == 29) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 4 && (tc.TithiId == 6 || tc.TithiId == 7 || tc.TithiId == 21) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 5 && (tc.TithiId == 2 || tc.TithiId == 3 || tc.TithiId == 15 || tc.TithiId == 17 || tc.TithiId == 18 || tc.TithiId == 30) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 6 && (tc.TithiId == 1 || tc.TithiId == 2 || tc.TithiId == 7 || tc.TithiId == 16 || tc.TithiId == 17 || tc.TithiId == 22) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 7 && (tc.TithiId == 9 || tc.TithiId == 10 || tc.TithiId == 11 || tc.TithiId == 24 || tc.TithiId == 25 || tc.TithiId == 26) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 8 && (tc.TithiId == 5 || tc.TithiId == 14 || tc.TithiId == 20) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 9 && (tc.TithiId == 2 || tc.TithiId == 7 || tc.TithiId == 8 || tc.TithiId == 9 || tc.TithiId == 17 || tc.TithiId == 22 || tc.TithiId == 23 || tc.TithiId == 24) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 10 && (tc.TithiId == 1 || tc.TithiId == 4 || tc.TithiId == 5 || tc.TithiId == 16 || tc.TithiId == 19 || tc.TithiId == 20) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 11 && (tc.TithiId == 4 || tc.TithiId == 6 || tc.TithiId == 10 || tc.TithiId == 19 || tc.TithiId == 20 || tc.TithiId == 25) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                    if (mc.MasaId == 12 && (tc.TithiId == 3 || tc.TithiId == 14 || tc.TithiId == 19 || tc.TithiId == 29) && (tc.DateStart.Between(mc.DateStart, mc.DateEnd) || tc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        stcList.Add(CreateShunyaTithiPeriod(mc, tc));
                    }
                }
            }
            return stcList;
        }

        private List<ShunyaNakshatraCalendar> CreateShunyaNakshatraCalendarList(List<MasaCalendar> mcList, List<NakshatraCalendar> ncList)
        {
            List<ShunyaNakshatraCalendar> sncList = new List<ShunyaNakshatraCalendar>();
            List<MasaCalendar> mcClonedList = Utility.CloneMasaCalendarList(mcList);
            List<NakshatraCalendar> ncClonedList = Utility.CloneNakshatraCalendarList(ncList);
            foreach (MasaCalendar mc in mcClonedList)
            {
                foreach (NakshatraCalendar nc in ncClonedList)
                {
                    if (mc.MasaId == 1 && (nc.NakshatraCode == ENakshatra.ASHWINI || nc.NakshatraCode == ENakshatra.ROHINI) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 2 && (nc.NakshatraCode == ENakshatra.CHITRA || nc.NakshatraCode == ENakshatra.SWATI) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 3 && (nc.NakshatraCode == ENakshatra.PUNARVASU || nc.NakshatraCode == ENakshatra.PUSHYA || nc.NakshatraCode == ENakshatra.UTTARAASHADHA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 4 && (nc.NakshatraCode == ENakshatra.PURVAPHALGUNI || nc.NakshatraCode == ENakshatra.DHANISHTA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 5 && (nc.NakshatraCode == ENakshatra.PURVAASHADHA || nc.NakshatraCode == ENakshatra.UTTARAASHADHA || nc.NakshatraCode == ENakshatra.SHRAVANA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 6 && (nc.NakshatraCode == ENakshatra.SHATABHISHA || nc.NakshatraCode == ENakshatra.REVATI) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 7 && nc.NakshatraCode == ENakshatra.PURVABHADRAPADA && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 8 && (nc.NakshatraCode == ENakshatra.KRITTIKA || nc.NakshatraCode == ENakshatra.MRIGASHIRA || nc.NakshatraCode == ENakshatra.PUSHYA || nc.NakshatraCode == ENakshatra.MAGHA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 9 && (nc.NakshatraCode == ENakshatra.CHITRA || nc.NakshatraCode == ENakshatra.VISAKHA || nc.NakshatraCode == ENakshatra.ANURADHA || nc.NakshatraCode == ENakshatra.UTTARABHADRAPADA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 10 && (nc.NakshatraCode == ENakshatra.ASHWINI || nc.NakshatraCode == ENakshatra.ARDRA || nc.NakshatraCode == ENakshatra.ASHLESHA || nc.NakshatraCode == ENakshatra.HASTA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 11 && (nc.NakshatraCode == ENakshatra.MULA || nc.NakshatraCode == ENakshatra.SHRAVANA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                    else if (mc.MasaId == 12 && (nc.NakshatraCode == ENakshatra.BHARANI || nc.NakshatraCode == ENakshatra.JYESHTHA) && (nc.DateStart.Between(mc.DateStart, mc.DateEnd) || nc.DateEnd.Between(mc.DateStart, mc.DateEnd)))
                    {
                        sncList.Add(CreateShunyaNakshatraPeriod(mc, nc));
                    }
                }
            }
            return sncList;
        }

        private ShunyaTithiCalendar CreateShunyaTithiPeriod(MasaCalendar mc, TithiCalendar tc)
        {
            DateTime newDateStart = new DateTime();
            DateTime newDateEnd = new DateTime();
            if (tc.DateStart <= mc.DateStart)
            {
                newDateStart = mc.DateStart;
            }
            if (tc.DateStart > mc.DateStart)
            {
                newDateStart = tc.DateStart;
            }
            if (tc.DateEnd >= mc.DateEnd)
            {
                newDateEnd = mc.DateEnd;
            }
            if (tc.DateEnd < mc.DateEnd)
            {
                newDateEnd = tc.DateEnd;
            }
            return new ShunyaTithiCalendar { DateStart = newDateStart, DateEnd = newDateEnd, ColorCode = EColor.SHUNIATITHI, MasaId = mc.MasaId, TithiId = tc.TithiId, ShunyaCode = EShunya.SHUNYATITHI };
        }

        private ShunyaNakshatraCalendar CreateShunyaNakshatraPeriod(MasaCalendar mc, NakshatraCalendar nc)
        {
            DateTime newDateStart = new DateTime();
            DateTime newDateEnd = new DateTime();
            if (nc.DateStart <= mc.DateStart)
            {
                newDateStart = mc.DateStart;
            }
            if(nc.DateStart > mc.DateStart)
            {
                newDateStart = nc.DateStart;
            }
            if (nc.DateEnd >= mc.DateEnd)
            {
                newDateEnd = mc.DateEnd;
            }
            if (nc.DateEnd < mc.DateEnd)
            {
                newDateEnd = nc.DateEnd;
            }
            return new ShunyaNakshatraCalendar { DateStart = newDateStart, DateEnd = newDateEnd, ColorCode = EColor.SHUNYANAKSHATRA, MasaId = mc.MasaId, NakshatraCode = nc.NakshatraCode, ShunyaCode = EShunya.SHUNYANAKSHATRA };
        }

        private List<MasaCalendar> CreateMasaCalendarList(List<PlanetCalendar> moonZodiakCalendarList, List<TithiData> tdList)
        {
            List<MasaCalendar> mList = new List<MasaCalendar>();
            int index = 0;
            DateTime startDate = new DateTime();
            DateTime fullMoonDate = new DateTime();
            int firstTithiId = tdList.First().TithiId;
            if (firstTithiId > 1)
            {
                do
                {
                    if (index == 0)
                    {
                        startDate = new DateTime(tdList.First().Date.Year, 1, 1, 0, 0, 0);
                    }
                    if (tdList[index].TithiId == 16)
                    {
                        fullMoonDate = tdList[index].Date;
                    }
                    if (tdList[index].TithiId == 1)
                    {
                        int masaId = GetMasaIdByDate(moonZodiakCalendarList, tdList[index].Date) - 1;
                        if (masaId == 0)
                        {
                            masaId = 12;
                        }
                        MasaCalendar tTemp = new MasaCalendar
                        {
                            DateStart = startDate,
                            DateEnd = tdList[index].Date,
                            MasaId = masaId,
                            FullMoonDate = fullMoonDate
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
                    if (tdList[index].TithiId == 16)
                    {
                        fullMoonDate = tdList[index].Date;
                    }
                    if (tdList[index].TithiId == 1 && index > 0)
                    {
                        int masaId = GetMasaIdByDate(moonZodiakCalendarList, tdList[index].Date);
                        MasaCalendar tTemp = new MasaCalendar
                        {
                            DateStart = startDate,
                            DateEnd = tdList[index].Date,
                            MasaId = masaId,
                            FullMoonDate = fullMoonDate
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
                int masaId = GetMasaIdByDate(moonZodiakCalendarList,  startDate);
                MasaCalendar tTemp = new MasaCalendar
                {
                    DateStart = startDate,
                    DateEnd = new DateTime(tdList.Last().Date.Year, 12, 31, 23, 59, 59),
                    MasaId = masaId,
                    FullMoonDate = fullMoonDate
                };
                mList.Add(tTemp);
            }
            return mList;
        }

        private int GetMasaIdByDate(List<PlanetCalendar> moonZodiakCalendarList, DateTime date)
        {
            int zodiakId = (int)(moonZodiakCalendarList.Where(i => i.DateStart <= date && i.DateEnd >= date).FirstOrDefault().ZodiakCode);
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
            string tranzitText = Utility.GetLocalizedText("Transits", _activeLanguageCode);

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
            string tranzitText = Utility.GetLocalizedText("Transits", _activeLanguageCode);

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

            //Check if any profile is selected by default. If yes - creating calendar
            Profile workingProfile = CacheLoad._profileList.Where(i => i.Checked == 1).FirstOrDefault();
            if (workingProfile != null)
            {
                _selectedProfile = workingProfile;
                BirthInfoReloadWithNewProfile(_selectedProfile.PlaceOfBirthId);
                PrepareProfileAndTimeZoneLabels();
                using (WaitForm wForm = new WaitForm(DrawCalendarData, _activeLanguageCode))
                {
                    wForm.ShowDialog(this);
                }
            }
        }

        private void BirthInfoReloadWithNewProfile(int locBirthId)
        {
            double latitude, longitude;
            if (CacheLoad._pdBirthList != null)
            {
                CacheLoad._pdBirthList.Clear();
            }
            Location birthLocation = CacheLoad._locationList.Where(i => i.Id == _selectedProfile.PlaceOfBirthId).FirstOrDefault();
            if (double.TryParse(birthLocation.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                double.TryParse(birthLocation.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
            {
                CacheLoad._pdBirthList = Utility.CalculatePlanetsPositionForDate(_selectedProfile.DateOfBirth, latitude, longitude);
                CacheLoad._bithAscendant = Utility.CalculateAscendantForDate(_selectedProfile.DateOfBirth, latitude, longitude, 0, 'O');
                CacheLoad._birthZodiakMoonId = Utility.GetZodiakIdFromDegree(CacheLoad._pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault()?.Longitude ?? 0);
                CacheLoad._birthNakshatraMoonId = Utility.GetNakshatraIdFromDegree(CacheLoad._pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault()?.Longitude ?? 0);
                CacheLoad._birthPadaMoonNumber = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(CacheLoad._pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault()?.Longitude ?? 0));
                CacheLoad._birthLagnaId = Utility.GetZodiakIdFromDegree(CacheLoad._bithAscendant);
                CacheLoad._birthNakshatraLagnaId = Utility.GetNakshatraIdFromDegree(CacheLoad._bithAscendant);
                CacheLoad._birthPadaLagnaNumber = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(CacheLoad._bithAscendant));
            }
        }

        private string GetTimeZoneInfo(int pLivingId)
        {
            string tInfo = string.Empty;
            string timeZone = string.Empty, ianaTZ = string.Empty;
            double latitude, longitude;
            DateTime startDate, endDate;
            TimeZoneInfo.TransitionTime daylightStart = new TimeZoneInfo.TransitionTime(), daylightEnd = new TimeZoneInfo.TransitionTime();
            string cityName = CacheLoad._locationList.Where(i => i.Id == pLivingId).FirstOrDefault()?.Locality ?? string.Empty;

            if (Utility.GetGeoCoordinateByLocationId(pLivingId, out latitude, out longitude))
            {
                timeZone = Utility.GetTimeZoneDotNetIdByGeoCoordinates(latitude, longitude);
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
            
            string nakshatraName = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == CacheLoad._birthNakshatraMoonId && i.LanguageCode.Equals(_activeLanguageCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
            Font labelTitleFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.HEADER)), 10, Utility.GetFontStyleBySettings(EFontList.HEADER));
            Font labelTZFont = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Regular);

            string labelText = _selectedProfile.ProfileName + ", "
                                + Utility.GetLocalizedText("Moon Nakshatra", _activeLanguageCode) + " "
                                + nakshatraName;

            Invoke(new Action(() =>
            {
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
            }));
        }

        private void datePicker_ValueChanged(object sender, CustomControls.CheckDateEventArgs e)
        {
            if (datePicker.Value.Month == _selectedDate.Month)
                return;

            if (_selectedProfile != null)
            {
                //Cursor.Current = Cursors.WaitCursor;
                using (WaitForm wForm = new WaitForm(DrawCalendarData, _activeLanguageCode))
                {
                    wForm.ShowDialog(this);
                }
            }
        }

        private void DrawCalendarData()
        {
            // Redrawing calendar for a new selected month
            _selectedDate = datePicker.Value.Date;
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
            else if (type.Equals("R") && prevZnak != znak) // && planet != EPlanet.RAHUMEAN && planet != EPlanet.KETUMEAN && planet != EPlanet.RAHUTRUE && planet != EPlanet.KETUTRUE)
            {
                if (planet != EPlanet.RAHUMEAN && planet != EPlanet.KETUMEAN && planet != EPlanet.RAHUTRUE && planet != EPlanet.KETUTRUE)
                {
                    pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + "." + type + "→";
                }
                else
                {
                    pName = Utility.GetLocalizedPlanetNameByCode(planet, _activeLanguageCode).Substring(0, 2) + "→";
                }
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

        private void DrawShunyaColoredLine(Graphics g, Pen pen, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date)
        {
            if (cList.Count > 0)
            {
                float startPosX = 0, endPosX = 0;
                foreach (Calendar c in cList)
                {
                    SolidBrush brush = new SolidBrush(Utility.GetColorByColorCode(c.ColorCode));
                    if (c.DateStart <= date)
                    {
                        startPosX = 0;
                        if (c.DateEnd <= date.AddDays(+1))
                            endPosX = Utility.ConvertHoursToPixels(width, c.DateEnd);
                        else
                            endPosX = width;
                    }
                    if (c.DateStart > date)
                    {
                        startPosX = Utility.ConvertHoursToPixels(width, c.DateStart);
                        if (c.DateEnd <= date.AddDays(+1))
                            endPosX = Utility.ConvertHoursToPixels(width, c.DateEnd);
                        else
                            endPosX = width;
                    }

                    if (c.GetShunyaCode() == EShunya.SHUNYANAKSHATRA)
                    {
                        g.FillRectangle(brush, posX + startPosX, posY, endPosX - startPosX, height / 2);
                    }
                    if (c.GetShunyaCode() == EShunya.SHUNYATITHI)
                    {
                        g.FillRectangle(brush, posX + startPosX, posY + height / 2, endPosX - startPosX, height - height / 2);
                    }
                    
                    
                }
            }
        }

        private void DrawYogaColoredLine(Graphics g, Pen pen, float posX, float posY, float width, float height, Day dayObj)
        {
            //Make list of dates for all jogas in day
            List<YogaColoredBlock> jcbList = Utility.GetYogaColoredBlockListForDay(dayObj);
            DrawColoredYogaDay(g, pen, posX, posY, width, height, jcbList);
        }

        private void DrawColoredYogaDay(Graphics g, Pen pen, float posX, float posY, float width, float height, List<YogaColoredBlock> jcbList)
        {
            if (jcbList.Count > 0)
            {
                foreach (YogaColoredBlock jcb in jcbList)
                {
                    if (jcb.DateStart <= jcb.Date.AddDays(+1))
                    {
                        float startPosX = Utility.ConvertHoursToPixels(width, jcb.DateStart);
                        float endPosX = 0;
                        if (jcb.DateEnd <= jcb.Date.AddDays(+1))
                            endPosX = Utility.ConvertHoursToPixels(width, jcb.DateEnd);
                        else
                            endPosX = width + Utility.ConvertHoursToPixels(width, jcb.DateEnd);
                        if (jcb.ColorCode != EColor.NOCOLOR)
                        {
                            SolidBrush brush = new SolidBrush(Utility.GetColorByColorCode(jcb.ColorCode));
                            g.FillRectangle(brush, posX + startPosX, posY, endPosX - startPosX, height);
                        }
                    }
                }
            }
        }

        private void DrawLineName(Graphics g, Pen pen, Font textFont, SolidBrush textBrush, float posY, float labelsWidth, float height, string text)
        {
            string localText = Utility.GetLocalizedText(text, _activeLanguageCode);
            Size textSize = TextRenderer.MeasureText(localText, textFont);
            float posX = labelsWidth - (textSize.Width + 2);
            float heightPadding = (height - textSize.Height) / 2;
            g.DrawString(localText, textFont, textBrush, posX, posY + heightPadding);
        }

        private void TranzitDrawing(List<Day> daysList)
        {
            _daysOfMonth = daysList.Where(i => i.IsDayOfMonth).ToList();
            int daysInMonth = DateTime.DaysInMonth(_daysOfMonth[0].Date.Year, _daysOfMonth[0].Date.Month);

            Font dayOfWeekFontPoint = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel);
            Font textFontPoint = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANZITTEXT)), 8, Utility.GetFontStyleBySettings(EFontList.TRANZITTEXT), GraphicsUnit.Point);

            Size textSizelabel = TextRenderer.MeasureText(Utility.GetLocalizedText("Chandra Bala", _activeLanguageCode), textFontPoint);
            _dayTranzWidth = (pictureBoxTranzits.Width - textSizelabel.Width) / daysInMonth;
            _lineTranzHeight = (pictureBoxTranzits.Height) / 46 - 1;

            float lineWidth = _dayTranzWidth * daysInMonth;
            _labelsWidth = pictureBoxTranzits.Width - _dayTranzWidth * daysInMonth - 1;
            float dayOfMonthHeight = _lineTranzHeight;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);

            Font dayOfWeekFontPixel = new Font(FontFamily.GenericSansSerif, dayOfMonthHeight - 2, FontStyle.Regular, GraphicsUnit.Pixel);
            Font textFontPixel = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANZITTEXT)), _lineTranzHeight - 4, Utility.GetFontStyleBySettings(EFontList.TRANZITTEXT), GraphicsUnit.Point);

            Font dayOfWeekFont, textFont;

            Size textSizeTranzit = TextRenderer.MeasureText("Test text 2345", textFontPoint);
            if ((_lineTranzHeight - 2) >= (textSizeTranzit.Height + textSizeTranzit.Height / 2))
            {
                dayOfWeekFont = dayOfWeekFontPixel;
                textFont = textFontPixel;
            }
            else
            {
                dayOfWeekFont = dayOfWeekFontPoint;
                textFont = textFontPoint;
            }

            Bitmap canvas = new Bitmap(pictureBoxTranzits.Width, pictureBoxTranzits.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, canvas.Width, canvas.Height));

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            float posX = _labelsWidth, posY = 0, nextPlanetY = 0;
            float posYMasa = posY + 2 * dayOfMonthHeight + 4;
            float posYPanchanga = posYMasa + _lineTranzHeight + 4;
            float posYJoga = posYPanchanga + 6 * _lineTranzHeight + 4;
            float posYTranzits = posYJoga + _lineTranzHeight + 4;

            EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting tranzitSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            foreach (Day d in _daysOfMonth)
            {
                //Draw days and days of week
                posY = 0;
                bool isEclipse = false;
                int day = Utility.GetDayOfWeekNumberFromDate(d.Date, weekSetting);
                if (d.EclipseDayList.Count > 0)
                {
                    isEclipse = true;
                }
                DrawDayOfMonth(g, pen, dayOfWeekFont, textBrush, posX, posY, _dayTranzWidth, dayOfMonthHeight, Utility.GetDaysOfWeekName(day, _activeLanguageCode, weekSetting).Substring(0, 3), d.Date, _todayDate, isEclipse);
                DrawDayOfMonth(g, pen, dayOfWeekFont, textBrush, posX, posY + dayOfMonthHeight, _dayTranzWidth, dayOfMonthHeight, d.Date.Day.ToString(), d.Date, _todayDate, isEclipse);

                // Draw masa
                posY = posYMasa;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, _lineTranzHeight, "Masa/Shunya");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MasaDayList);

                //Fill masa line by shunya nakshatra
                posY = posYMasa;
                DrawShunyaColoredLine(g, pen, posX, posY, _dayTranzWidth, _lineTranzHeight, d.ShunyaNakshatraDayList, d.Date);

                //Fill masa line by shunya tithi
                posY = posYMasa;
                DrawShunyaColoredLine(g, pen, posX, posY, _dayTranzWidth, _lineTranzHeight, d.ShunyaTithiDayList, d.Date);

                // Draw panchanga
                posY = posYPanchanga;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, _lineTranzHeight, "Nakshatra");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.NakshatraDayList);

                DrawLineName(g, pen, textFont, textBrush, posY + _lineTranzHeight, _labelsWidth, _lineTranzHeight, "Tara Bala");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.TaraBalaDayList);

                DrawLineName(g, pen, textFont, textBrush, posY + 2 * _lineTranzHeight, _labelsWidth, _lineTranzHeight, "Tithi");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.TithiDayList);

                DrawLineName(g, pen, textFont, textBrush, posY + 3 * _lineTranzHeight, _labelsWidth, _lineTranzHeight, "Karana");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KaranaDayList);

                DrawLineName(g, pen, textFont, textBrush, posY + 4 * _lineTranzHeight, _labelsWidth, _lineTranzHeight, "Nitya Yoga");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 4 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.NityaJogaDayList);

                DrawLineName(g, pen, textFont, textBrush, posY + 5 * _lineTranzHeight, _labelsWidth, _lineTranzHeight, "Chandra Bala");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 5 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.ChandraBalaDayList);

                //Draw Jogas line
                posY = posYJoga;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, _lineTranzHeight, "VTN Yogi");
                DrawYogaColoredLine(g, pen, posX, posY, _dayTranzWidth, _lineTranzHeight, d);

                //Draw planets tranzits
                //Drawing Moon
                nextPlanetY = posYTranzits;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Moon");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MoonZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MoonZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.MoonZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.MoonZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MoonMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MoonNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MoonPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MoonTaraBalaDayList);

                //Drawing Sun
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Sun");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SunZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SunZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.SunZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.SunZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SunMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SunNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SunPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SunTaraBalaDayList);

                //Drawing Venus
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Venus");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.VenusZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.VenusZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.VenusZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.VenusZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.VenusMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.VenusNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.VenusPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.VenusTaraBalaDayList);

                //Drawing Jupiter
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Jupiter");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.JupiterZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.JupiterZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.JupiterZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.JupiterZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.JupiterMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.JupiterNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.JupiterPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.JupiterTaraBalaDayList);

                //Drawing Mercury
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Mercury");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MercuryZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MercuryZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.MercuryZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.MercuryZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MercuryMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MercuryNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MercuryPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MercuryTaraBalaDayList);

                //Drawing Mars
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Mars");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MarsZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MarsZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.MarsZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.MarsZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.MarsMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MarsNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MarsPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.MarsTaraBalaDayList);

                //Drawing Saturn
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Saturn");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SaturnZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SaturnZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, height, d.SaturnZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, _dayTranzWidth, height, d.SaturnZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.SaturnMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SaturnNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SaturnPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.SaturnTaraBalaDayList);

                //Drawing Rahu
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Rahu");
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
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
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
                    DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.RahuMeanMrityuBhagaDayList, d.Date);

                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuMeanPadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuMeanTaraBalaDayList);
                }
                else
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuTrueNakshatraDayList);
                    DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.RahuTrueMrityuBhagaDayList, d.Date);

                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuTruePadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.RahuTrueTaraBalaDayList);
                }

                //Drawing Ketu
                nextPlanetY += 4 * _lineTranzHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textFont, textBrush, posY, _labelsWidth, (4 * _lineTranzHeight), "Ketu");
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
                        float height = _lineTranzHeight / 2;
                        float posYHalf = posY + height;
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
                    DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.KetuMeanMrityuBhagaDayList, d.Date);

                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuMeanPadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuMeanTaraBalaDayList);
                }
                else
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuTrueNakshatraDayList);
                    DrawingMrityaBhaga(g, posX, posY, _dayTranzWidth, _lineTranzHeight, d.KetuTrueMrityuBhagaDayList, d.Date);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuTruePadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * _lineTranzHeight, _dayTranzWidth, _lineTranzHeight, d.KetuTrueTaraBalaDayList);
                }

                posX = posX + _dayTranzWidth;
            }

            // Setting masa changes
            posX = _labelsWidth;
            posY = posYMasa;
            foreach (Day c in _daysOfMonth)
            {
                SetMasaName(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, c.MasaDayList, c.Date);
                posX = posX + _dayTranzWidth;
            }
            // Setting masa current start 
            posX = _labelsWidth;
            posY = posYMasa;            
            SetMasaStartName(g, pen, textFont, textBrush, posX, posY, _dayTranzWidth, _lineTranzHeight, _daysOfMonth.First().MasaDayList);

            // Setting panchanga numbers
            posX = _labelsWidth;
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
            posX = _labelsWidth;
            
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
            posX = _labelsWidth;
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
            posX = _labelsWidth;
            posY = posYMasa;
            DrawLineRectangle(g, pen, posX, posY, lineWidth, _lineTranzHeight);

            // Drawing Panchanga rectangles
            posX = _labelsWidth;
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

        private void SetMasaStartName(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetMasaFullName(_moonNakshatraCalendar.ToList(), _activeLanguageCode); ;
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width <= endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
            }
        }

        private void SetLineStartZodiak(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetNumber(_activeLanguageCode);
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width <= endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
            }
        }

        private void SetLineStartNakshatra(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c, bool isMoon)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetTranzitNakshatra(_activeLanguageCode);
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
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

        private void SetLineStartPada(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                List<PlanetCalendar> pList = Utility.ClonePlanetCalendarList(c);
                string text = string.Empty;
                if (pList.First().PlanetCode == EPlanet.VENUS || pList.First().PlanetCode == EPlanet.MERCURY || pList.First().PlanetCode == EPlanet.JUPITER)
                {
                    text = pList.First().GetTranzitPadaWithoutBadNavamshaAndDreakkana();
                }
                else
                {
                    text = pList.First().GetTranzitPada(_activeLanguageCode);
                    if (pList.First().PlanetCode == EPlanet.MOON)
                        text = text.Substring(0, 1);
                }
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
                    if (textSize.Width < endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
            }
        }

        private void SetLineStartTaraBala(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c, bool isMoon)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetTranzitTaraBala(_activeLanguageCode);
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width, c.Last().DateStart);
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

        private void SetPlanetZodiak(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date)
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
                        float startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                        string text = pc.GetNumber(_activeLanguageCode);
                        Size textSize = TextRenderer.MeasureText(text, font);
                        float heightPadding = (height - textSize.Height) / 2;
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

        private void SetPlanetNakshatra(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date, bool isMoon)
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
                        float startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                        string text = pc.GetTranzitNakshatra(_activeLanguageCode); 
                        Size textSize = TextRenderer.MeasureText(text, font);
                        float heightPadding = (height - textSize.Height) / 2;
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

        private void SetPlanetPada(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date)
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
                            string text = string.Empty;
                            if (pc.PlanetCode == EPlanet.VENUS || pc.PlanetCode == EPlanet.MERCURY || pc.PlanetCode == EPlanet.JUPITER)
                            {
                                text = pc.GetTranzitPadaWithoutBadNavamshaAndDreakkana();
                            }
                            else
                            {
                                text = pc.GetTranzitPada(_activeLanguageCode);
                                if (pc.PlanetCode == EPlanet.MOON)
                                    text = text.Substring(0, 1);
                            }
                            float startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                            Size textSize = TextRenderer.MeasureText(text, font);
                            float heightPadding = (height - textSize.Height) / 2;
                            g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                            g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            previousPada = currentPada;
                        }
                    }
                }
            }
        }

        private void SetPlanetTaraBala(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date, bool isMoon)
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
                        float startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                        string text = pc.GetTranzitTaraBala(_activeLanguageCode);
                        Size textSize = TextRenderer.MeasureText(text, font);
                        float heightPadding = (height - textSize.Height) / 2;
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

        private void SetPanchangaNumbers(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date)
        {
            foreach (Calendar c in cList)
            {
                if (c.DateStart > date)
                {
                    float startPosX = Utility.ConvertHoursToPixels(width, c.DateStart);
                    string text = c.GetNumber(_activeLanguageCode);
                    Size textSize = TextRenderer.MeasureText(text, font);
                    float heightPadding = (height - textSize.Height) / 2;
                    g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                }
            }
        }

        private void SetMasaName(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date)
        {
            string curMasa = cList.First().GetMasaFullName(_moonNakshatraCalendar, _activeLanguageCode);
            foreach (Calendar c in cList)
            {
                if (c.DateStart > date)
                {
                    float startPosX = Utility.ConvertHoursToPixels(width, c.DateStart);
                    string text = c.GetMasaFullName(_moonNakshatraCalendar, _activeLanguageCode); 
                    Size textSize = TextRenderer.MeasureText(text, font);
                    float heightPadding = (height - textSize.Height) / 2;
                    if (!text.Equals(curMasa))
                    {
                        g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                        g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                        curMasa = text;
                    }
                }
            }
        }

        private void DrawPlanetBlockRectangles(Graphics g, Pen pen, float posX, float posY, float width, float height)
        {
            for (int i = 0; i < 4; i++)
            {
                DrawLineRectangle(g, pen, posX, posY + i * _lineTranzHeight, width, height);
            }
        }

        private void DrawLineRectangle(Graphics g, Pen pen, float posX, float posY, float width, float height)
        {
            g.DrawRectangle(pen, posX, posY, width, height);
        }

        private void DrawTranzitColoredLine(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList)
        {
            float drawWidth = 0, usedWidth = 0;
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

        private void DrawTranzitColoredRectangle(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, Calendar cObj)
        {
            SolidBrush brush = new SolidBrush(Utility.GetColorByColorCode(cObj.ColorCode));
            g.FillRectangle(brush, posX, posY, width, height);
        }

        private void DrawDayOfMonth(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, string text, DateTime currentDate, DateTime todayDate, bool isEclipse)
        {
            // Drawing blue rectangle for today
            if (currentDate == todayDate.Date)
            {
                g.FillRectangle(new SolidBrush(Color.LightBlue), posX, posY, width, height);
            }
            // Drawing light red rectangle if Eclipse day
            if (isEclipse)
            {
                g.FillRectangle(new SolidBrush(Utility.GetColorByColorCode(EColor.LIGHTRED)), posX, posY, width, height);
            }

            Size textSize = TextRenderer.MeasureText(text, font);
            float heightPadding = (height - textSize.Height) / 2;
            g.DrawString(text, font, textBrush, (posX + (width / 2 - textSize.Width / 2)), posY + heightPadding);
            g.DrawRectangle(pen, posX, posY, width, height);
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
            _daysOfWeekHeight = 18;
            _dayWidth = (pictureBoxCalendar.Width - 1) / 7;
            _dayHeight = ((pictureBoxCalendar.Height - 1) - _daysOfWeekHeight) / 6;

            float lineHeight = (_dayHeight * 77) / (6 * 100);
            float dayFrameHeight = _dayHeight - (lineHeight * 6);
            float posX = 0, posY = 0;

            Bitmap canvas = new Bitmap(pictureBoxCalendar.Width, pictureBoxCalendar.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, canvas.Width, canvas.Height));

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            SolidBrush outDayBrush = new SolidBrush(Color.Gray);
            SolidBrush eventBrush = new SolidBrush(Color.Blue);

            Font dayOfWeekFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Point);

            Font dayFontPixel = new Font(FontFamily.GenericSansSerif, dayFrameHeight - 4, FontStyle.Bold, GraphicsUnit.Pixel);
            Font tranzitFontPixel = new Font(FontFamily.GenericSansSerif, dayFrameHeight / 3, FontStyle.Regular, GraphicsUnit.Pixel);
            Font sunFontPixel = new Font(FontFamily.GenericSansSerif, dayFrameHeight / 4, FontStyle.Regular, GraphicsUnit.Pixel);
            Font lineFontPixel = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.CALENDARTEXT)), lineHeight - 2, Utility.GetFontStyleBySettings(EFontList.CALENDARTEXT), GraphicsUnit.Pixel);
            Font eclipseFontPixel = new Font(FontFamily.GenericSansSerif, dayFrameHeight / 4, FontStyle.Regular, GraphicsUnit.Pixel);

            Font dayFontPoint = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Bold, GraphicsUnit.Point);
            Font tranzitFontPoint = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular, GraphicsUnit.Point);
            Font sunFontPoint = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Regular, GraphicsUnit.Point);
            Font lineFontPoint = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.CALENDARTEXT)), 8, Utility.GetFontStyleBySettings(EFontList.CALENDARTEXT), GraphicsUnit.Point);
            Font eclipseFontPoint = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Regular, GraphicsUnit.Point);

            Font lineFont, tranzitFont, dayFont;

            Size textSize = new Size(0, 0);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // Drawing days of week
            for (int i = 0; i < _daysOfWeek.Length; i++)
            {
                textSize = TextRenderer.MeasureText(_daysOfWeek[i], dayOfWeekFont);
                g.DrawString(_daysOfWeek[i], dayOfWeekFont, textBrush, (posX + (_dayWidth / 2 - textSize.Width / 2)), 1);
                g.DrawRectangle(pen, posX, posY, _dayWidth, _daysOfWeekHeight);
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
                    Size textSizePoint = TextRenderer.MeasureText(daysList[day].Date.Day.ToString(), dayFontPoint);
                    Size textSizePixel = TextRenderer.MeasureText(daysList[day].Date.Day.ToString(), dayFontPixel);
                    if ((dayFrameHeight - 2) >= (textSize.Height + textSize.Height / 2))
                    {
                        dayFont = dayFontPixel;
                        // Drawing blue rectangle for today
                        if (daysList[day].Date == todayDate.Date)
                        {
                            g.FillRectangle(new SolidBrush(Color.LightBlue), posX + 1, posY + 1, textSizePixel.Width, textSizePixel.Height);
                            g.DrawRectangle(new Pen(Color.DarkBlue, 1), posX + 1, posY + 1, textSizePixel.Width, textSizePixel.Height);
                        }
                    }
                    else
                    {
                        dayFont = dayFontPoint;
                        // Drawing blue rectangle for today
                        if (daysList[day].Date == todayDate.Date)
                        {
                            g.FillRectangle(new SolidBrush(Color.LightBlue), posX + 1, posY + 1, textSizePoint.Width, textSizePoint.Height);
                            g.DrawRectangle(new Pen(Color.DarkBlue, 1), posX + 1, posY + 1, textSizePoint.Width, textSizePoint.Height);
                        }
                    }
                    
                    // Drawing day number
                    Size daySize = new Size(0, 0);
                    if (daysList[day].IsDayOfMonth)
                    {
                        string dayText = daysList[day].Date.Day.ToString();
                        daySize = TextRenderer.MeasureText(dayText, dayFont);
                        g.DrawString(dayText, dayFont, textBrush, posX + 2, posY + 1);
                    }
                    else
                    {
                        string dayText = daysList[day].Date.Day.ToString();
                        daySize = TextRenderer.MeasureText(dayText, dayFont);
                        g.DrawString(dayText, dayFont, outDayBrush, posX + 2, posY + 1);
                    }

                    //Drawing eclipse
                    SizeF imgSize = new SizeF(0F, 0F);
                    if (daysList[day].EclipseDayList.Count > 0)
                    {
                        Image img = null;
                        foreach (EclipseCalendar ec in daysList[day].EclipseDayList)
                        {
                            string text = ec.GetShortName(_activeLanguageCode);
                            textSize = TextRenderer.MeasureText(text, eclipseFontPoint);
                            imgSize = new SizeF(dayFrameHeight / 2, dayFrameHeight / 2);
                            if ((dayFrameHeight - 4) < (imgSize.Height + textSize.Height))
                                imgSize = new SizeF(dayFrameHeight / 3, dayFrameHeight / 3);
                            if (ec.EclipseCode == (int)EEclipse.MOONECLIPSE)
                                img = new Bitmap(_moonEclipseImage, Convert.ToInt32(imgSize.Width), Convert.ToInt32(imgSize.Height));
                            if (ec.EclipseCode == (int)EEclipse.SUNECLIPSE)
                                img = new Bitmap(_sunEclipseImage, Convert.ToInt32(imgSize.Width), Convert.ToInt32(imgSize.Height));

                            g.DrawImage(img, posX + daySize.Width - 2, (posY + dayFrameHeight) -  (textSize.Height + imgSize.Height));
                            g.DrawString(text, eclipseFontPoint, textBrush, posX + daySize.Width - 4, (posY + dayFrameHeight) - (textSize.Height + imgSize.Height) + imgSize.Height);
                        }
                    }

                    //Drawing planets tranzits
                    string planetsListText = FindIfPlanetChangeZnak(daysList[day]);
                    if (!planetsListText.Equals(string.Empty))
                    {
                        Size planetListSize = TextRenderer.MeasureText(planetsListText, tranzitFontPoint);
                        if (((dayFrameHeight / 2 - textSize.Height / 2) - 2) >= (textSize.Height + textSize.Height / 2))
                        {
                            tranzitFont = tranzitFontPixel;
                        }
                        else
                        {
                            tranzitFont = tranzitFontPoint;
                        }
                        g.DrawString(planetsListText, tranzitFont, textBrush, posX + daySize.Width + imgSize.Width + 1, posY + (dayFrameHeight / 2 - planetListSize.Height / 2));
                    }

                    //Drawing sunrise, sunset
                    string sunriseTime = Utility.GetSunStatusName(ESun.SUNRISE, _activeLanguageCode) + daysList[day].SunRise?.ToString("HH:mm:ss");
                    string sunsetTime = Utility.GetSunStatusName(ESun.SUNSET, _activeLanguageCode) + daysList[day].SunSet?.ToString("HH:mm:ss");
                    Size textSise = TextRenderer.MeasureText(sunriseTime, sunFontPoint);
                    float posXSunrise = posX + (_dayWidth - textSise.Width);
                    g.DrawString(sunriseTime, sunFontPoint, textBrush, posXSunrise, posY);
                    g.DrawString(sunsetTime, sunFontPoint, textBrush, posXSunrise, posY + textSise.Height - 1);

                    // Drawing colored lines
                    float posYForLines = posY + dayFrameHeight;
                    textSize = TextRenderer.MeasureText("Test text 2345", lineFontPoint);
                    if ((lineHeight - 2) >= (textSize.Height + textSize.Height / 2))
                    {
                        lineFont = lineFontPixel;
                    }
                    else
                    {
                        lineFont = lineFontPoint;
                    }

                    DrawColoredLine(g, pen, lineFont, textBrush, posX, posYForLines, _dayWidth, lineHeight, daysList[day].NakshatraDayList);
                    DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + lineHeight), _dayWidth, lineHeight, daysList[day].TaraBalaDayList);
                    DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 2 * lineHeight), _dayWidth, lineHeight, daysList[day].TithiDayList);
                    DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 3 * lineHeight), _dayWidth, lineHeight, daysList[day].KaranaDayList);
                    DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 4 * lineHeight), _dayWidth, lineHeight, daysList[day].NityaJogaDayList);
                    DrawColoredLine(g, pen, lineFont, textBrush, posX, (posYForLines + 5 * lineHeight), _dayWidth, lineHeight, daysList[day].ChandraBalaDayList);
                    
                    //Check for events and drawing if exists
                    List<PersonEvent> peDayList = Utility.GetDayPersonEvents(_selectedProfile.Id, daysList[day].Date);
                    if (peDayList.Count > 0)
                    {
                        float triangleX = (posX + _dayWidth) - 10;
                        float triangleY = posY + 10;

                        PointF[] tringlePoint = new PointF[] { new PointF(triangleX, posY), new PointF(posX + _dayWidth, posY), new PointF(posX + _dayWidth, triangleY) };
                        g.FillPolygon(eventBrush, tringlePoint);
                        g.DrawPolygon(pen, tringlePoint);
                    }
                    
                    // Drawing days grid
                    g.DrawRectangle(pen, posX, posY, _dayWidth, _dayHeight);
                    posX += _dayWidth;
                    day++;
                }
                posY += _dayHeight;
            }
            pictureBoxCalendar.Image = canvas;
            _currentCalendarImage = pictureBoxCalendar.Image;
        }

        private void DrawingMrityaBhaga(Graphics g, float posX, float posY, float width, float height, List<MrityuBhagaData> mbList, DateTime date)
        {
            if (mbList.Count > 0)
            {
                float startPosX = 0, endPosX = 0;
                SolidBrush brush = new SolidBrush(Utility.GetColorByColorCode(EColor.MRITYUBHAGA));
                foreach (MrityuBhagaData mb in mbList)
                {
                    if (mb.DateFrom <= date)
                    {
                        startPosX = 0;
                        if (mb.DateTo <= date.AddDays(+1))
                            endPosX = Utility.ConvertHoursToPixels(width, mb.DateTo);
                        else
                            endPosX = width;
                    }
                    if (mb.DateFrom > date)
                    {
                        startPosX = Utility.ConvertHoursToPixels(width, mb.DateFrom);
                        if (mb.DateTo <= date.AddDays(+1))
                            endPosX = Utility.ConvertHoursToPixels(width, mb.DateTo);
                        else
                            endPosX = width;
                    }
                    g.FillRectangle(brush, posX + startPosX, posY, endPosX - startPosX, height);
                }
            }
        }

        private void DrawColoredLine(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> dayList)
        {
            float drawWidth = 0, usedWidth = 0;
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

        private void DrawColoredRectangle(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, Calendar c)
        {
            SolidBrush brush = new SolidBrush(Utility.GetColorByColorCode(c.ColorCode));
            g.FillRectangle(brush, posX, posY, width, height);
            g.DrawRectangle(pen, posX, posY, width, height);
            string text = c.GetShortName(_activeLanguageCode);
            Size textSize = TextRenderer.MeasureText(text, font);
            float heightPadding = (height - textSize.Height) / 2;
            if (textSize.Width + 1 <= width)
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
                timeZone = Utility.GetTimeZoneDotNetIdByGeoCoordinates(latitude, longitude);
                TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                TimeZoneInfo.AdjustmentRule[] adjustmentRules = currentTimeZone.GetAdjustmentRules();

                EpheCalculation eCalc = new EpheCalculation();
                DateTime startPeriodDate, endPeriodDate;
                startPeriodDate = startDateOfMonth.AddDays(-15);
                endPeriodDate = endDate.AddDays(+15);

                //calculate data lists
                List<PlanetData> sunDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_SUN, startPeriodDate, endPeriodDate);
                List<PlanetData> moonDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MOON, startPeriodDate, endPeriodDate);
                List<PlanetData> marsDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MARS, startPeriodDate, endPeriodDate);
                List<PlanetData> mercuryDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MERCURY, startPeriodDate, endPeriodDate);
                List<PlanetData> jupiterDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_JUPITER, startPeriodDate, endPeriodDate);
                List<PlanetData> venusDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_VENUS, startPeriodDate, endPeriodDate);
                List<PlanetData> saturnDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_SATURN, startPeriodDate, endPeriodDate);
                List<PlanetData> rahuMeanDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MEAN_NODE, startPeriodDate, endPeriodDate);
                List<PlanetData> rahuTrueDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_TRUE_NODE, startPeriodDate, endPeriodDate);
                List<PlanetData> ketuMeanDataList = eCalc.PrepareKetuList(rahuMeanDataList);
                List<PlanetData> ketuTrueDataList = eCalc.PrepareKetuList(rahuTrueDataList);
                List<TithiData> tithiDataList = eCalc.CalculateTithiDataList_London(startPeriodDate, endDate);
                List<NityaYogaData> nityaYogaDataList = eCalc.CalculateNityaYogaDataList_London(startPeriodDate, endDate);
                List<EclipseData> eclipseDataList = eCalc.CalculateEclipse_London(startPeriodDate, endDate);

                List<MrityuBhagaData> sunMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.SUN, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> moonMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.MOON, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> marsMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.MARS, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> mercuryMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.MERCURY, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> jupiterMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.JUPITER, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> venusMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.VENUS, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> saturnMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.SATURN, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> rahuMeanMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.RAHUMEAN, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> rahuTrueMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.RAHUTRUE, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> ketuMeanMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.KETUMEAN, startPeriodDate, endPeriodDate);
                List<MrityuBhagaData> ketuTrueMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.KETUTRUE, startPeriodDate, endPeriodDate);

                //prepare Calendars
                List<NakshatraCalendar> nakshatraCalendarList = CacheLoad.CreateNakshatraCalendarList(moonDataList);
                nakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                nakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<TithiCalendar> tithiCalendarList = CacheLoad.CreateTithiCalendarList(tithiDataList);
                tithiCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                tithiCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });
                List<KaranaCalendar> karanaCalendarList = CacheLoad.CreateKaranaCalendarList(tithiCalendarList);

                List<ChandraBalaCalendar> chandraBalaCalendarList = CacheLoad.CreateChandraBalaCalendarList(moonDataList);
                chandraBalaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                chandraBalaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<NityaYogaCalendar> nityaYogaCalendarList = CacheLoad.CreateNityaYogaCalendarList(nityaYogaDataList);
                nityaYogaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                nityaYogaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MOON, moonDataList);
                List<MasaCalendar> masaCalendarList = CreateMasaCalendarList(moonZodiakCalendarList, tithiDataList);
                masaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                masaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });
                List<ShunyaNakshatraCalendar> shunyaNakshatraCalendarList = CreateShunyaNakshatraCalendarList(masaCalendarList, nakshatraCalendarList);
                List<ShunyaTithiCalendar> shunyaTithiCalendarList = CreateShunyaTithiCalendarList(masaCalendarList, tithiCalendarList);
                moonZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MOON, moonDataList);
                moonZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MOON, moonDataList);
                moonNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });
                _moonNakshatraCalendar = Utility.ClonePlanetCalendarList(moonNakshatraCalendarList.ToList());

                List<PlanetCalendar> moonPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MOON, moonDataList);
                moonPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.SUN, sunDataList);
                sunZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.SUN, sunDataList);
                sunZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.SUN, sunDataList);
                sunNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.SUN, sunDataList);
                sunPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.VENUS, venusDataList);
                venusZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.VENUS, venusDataList);
                venusZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.VENUS, venusDataList);
                venusNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.VENUS, venusDataList);
                venusPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MARS, marsDataList);
                marsZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MARS, marsDataList);
                marsZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MARS, marsDataList);
                marsNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MARS, marsDataList);
                marsPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.SATURN, saturnDataList);
                saturnZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.SATURN, saturnDataList);
                saturnZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.SATURN, saturnDataList);
                saturnNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.SATURN, saturnDataList);
                saturnPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTruePadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTruePadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<EclipseCalendar> eclipseCalendarList = CacheLoad.CreateEclipseCalendarList(eclipseDataList);
                eclipseCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                eclipseCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); });

                //Shifting MrityaBhaga by TimeZone
                moonMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                sunMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });
                
                mercuryMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                venusMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                marsMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                jupiterMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                saturnMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                rahuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                rahuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                ketuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                ketuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

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
                                        moonZodiakCalendarList,
                                        moonZodiakRetroCalendarList,
                                        moonNakshatraCalendarList,
                                        moonPadaCalendarList,
                                        sunZodiakCalendarList,
                                        sunZodiakRetroCalendarList,
                                        sunNakshatraCalendarList,
                                        sunPadaCalendarList,
                                        mercuryZodiakCalendarList,
                                        mercuryZodiakRetroCalendarList,
                                        mercuryNakshatraCalendarList,
                                        mercuryPadaCalendarList,
                                        venusZodiakCalendarList,
                                        venusZodiakRetroCalendarList,
                                        venusNakshatraCalendarList,
                                        venusPadaCalendarList,
                                        marsZodiakCalendarList,
                                        marsZodiakRetroCalendarList,
                                        marsNakshatraCalendarList,
                                        marsPadaCalendarList,
                                        jupiterZodiakCalendarList,
                                        jupiterZodiakRetroCalendarList,
                                        jupiterNakshatraCalendarList,
                                        jupiterPadaCalendarList,
                                        saturnZodiakCalendarList,
                                        saturnZodiakRetroCalendarList,
                                        saturnNakshatraCalendarList,
                                        saturnPadaCalendarList,
                                        rahuMeanZodiakCalendarList,
                                        rahuMeanZodiakRetroCalendarList,
                                        rahuMeanNakshatraCalendarList,
                                        rahuMeanPadaCalendarList,
                                        ketuMeanZodiakCalendarList,
                                        ketuMeanZodiakRetroCalendarList,
                                        ketuMeanNakshatraCalendarList,
                                        ketuMeanPadaCalendarList,
                                        rahuTrueZodiakCalendarList,
                                        rahuTrueZodiakRetroCalendarList,
                                        rahuTrueNakshatraCalendarList,
                                        rahuTruePadaCalendarList,
                                        ketuTrueZodiakCalendarList,
                                        ketuTrueZodiakRetroCalendarList,
                                        ketuTrueNakshatraCalendarList,
                                        ketuTruePadaCalendarList,
                                        nakshatraCalendarList,
                                        tithiCalendarList,
                                        karanaCalendarList,
                                        chandraBalaCalendarList,
                                        nityaYogaCalendarList,
                                        eclipseCalendarList,
                                        masaCalendarList,
                                        shunyaNakshatraCalendarList,
                                        shunyaTithiCalendarList,
                                        moonMBDataList,
                                        sunMBDataList,
                                        mercuryMBDataList,
                                        venusMBDataList,
                                        marsMBDataList,
                                        jupiterMBDataList,
                                        saturnMBDataList,
                                        rahuMeanMBDataList,
                                        rahuTrueMBDataList,
                                        ketuMeanMBDataList,
                                        ketuTrueMBDataList
                                        );
                    daysList.Add(tempDay);
                    currentDay = currentDay.AddDays(+1);                   
                }

                for (int day = 0; day < daysList.Count; day++)
                {
                    DateTime? sunrisePrevious = Utility.CalculateSunriseForDateAndLocation(daysList[day].Date.AddDays(-1), latitude, longitude, timeZone);
                    DateTime? sunsetPrevious = Utility.CalculateSunsetForDateAndLocation(daysList[day].Date.AddDays(-1), latitude, longitude, timeZone);
                    DateTime? sunriseNext = Utility.CalculateSunriseForDateAndLocation(daysList[day].Date.AddDays(+1), latitude, longitude, timeZone);

                    // Adding List<JogaCalendar> lists to List<DayCalendars> list
                    daysList[day].DwipushkarJogaDayList = PrepareYogaList(EYoga.DWIPUSHKAR, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].TripushkarJogaDayList = PrepareYogaList(EYoga.TRIPUSHKAR, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].AmritaSiddhaJogaDayList = PrepareYogaList(EYoga.AMRITASIDDHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].SarvarthaSiddhaJogaDayList = PrepareYogaList(EYoga.SARVARTHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].SiddhaJogaDayList = PrepareYogaList(EYoga.SIDDHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].MrityuJogaDayList = PrepareYogaList(EYoga.MRITYU, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].AdhamJogaDayList = PrepareYogaList(EYoga.ADHAM, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].YamaghataJogaDayList = PrepareYogaList(EYoga.YAMAGHATA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].DagdhaJogaDayList = PrepareYogaList(EYoga.DAGDHA, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);
                    daysList[day].UnfarobaleJogaDayList = PrepareYogaList(EYoga.UNFAVORABLE, daysList, daysList[day].Date, daysList[day].SunRise, sunrisePrevious, sunriseNext);

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

        private List<Calendar> PrepareYogaList(EYoga jogaCode, List<Day> daysList, DateTime date, DateTime? sunrise, DateTime? sunrisePrevious, DateTime? sunriseNext)
        {
            YogaCalendar jCal = new YogaCalendar();
            List<YogaCalendar> jogaList = new List<YogaCalendar>();

            switch (jogaCode)
            {
                case EYoga.DWIPUSHKAR:
                    List<YogaCalendar> dwipushkarJogaPrev = jCal.CheckDvipushkarJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckDvipushkarJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in dwipushkarJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.TRIPUSHKAR:
                    List<YogaCalendar> tripushkarJogaPrev = jCal.CheckTripushkarJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckTripushkarJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in tripushkarJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.AMRITASIDDHA:
                    List<YogaCalendar> amritaSiddhaJogaPrev = jCal.CheckAmritaSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckAmritaSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in amritaSiddhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.SARVARTHA:
                    List<YogaCalendar> sarvarthaSiddhaJogaPrev = jCal.CheckSarvarthaSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckSarvarthaSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in sarvarthaSiddhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.SIDDHA:
                    List<YogaCalendar> siddhaJogaPrev = jCal.CheckSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in siddhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    List<YogaCalendar> largeSiddhaJogaPrev = jCal.CheckLargeSiddhaJoga(sunrisePrevious, sunrise, daysList);
                    List<YogaCalendar> largeSiddhaJoga = jCal.CheckLargeSiddhaJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in largeSiddhaJogaPrev)
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

                case EYoga.MRITYU:
                    List<YogaCalendar> mrityuJogaPrev = jCal.CheckMritjuJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckMritjuJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in mrityuJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.ADHAM:
                    List<YogaCalendar> adhamJogaPrev = jCal.CheckAdhamJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckAdhamJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in adhamJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.YAMAGHATA:
                    List<YogaCalendar> yamaghataJogaPrev = jCal.CheckYamaghataJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckYamaghataJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in yamaghataJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.DAGDHA:
                    List<YogaCalendar> dagdhaJogaPrev = jCal.CheckDagdhaJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckDagdhaJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in dagdhaJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                case EYoga.UNFAVORABLE:
                    List<YogaCalendar> unfarobaleJogaPrev = jCal.CheckUnfarobaleJoga(sunrisePrevious, sunrise, daysList);
                    jogaList = jCal.CheckUnfarobaleJoga(sunrise, sunriseNext, daysList);
                    foreach (YogaCalendar jc in unfarobaleJogaPrev)
                    {
                        if (jc.DateEnd > date)
                            jogaList.Insert(0, jc);
                    }
                    break;

                default:
                    break;
            }

            List<Calendar> resList = new List<Calendar>();
            foreach (YogaCalendar jc in jogaList)
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
            if(pForm.SelectedProfile != null && pForm.SelectedProfile != _selectedProfile && pForm.IsChosen)
            {
                _selectedProfile = pForm.SelectedProfile;

                BirthInfoReloadWithNewProfile(_selectedProfile.PlaceOfBirthId);
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
            if (_selectedProfile != null && _currentYearTranzitsImage != null)
            {
                try
                {
                    using (WaitForm wForm = new WaitForm(CreateYearPDF, _activeLanguageCode))
                    {
                        wForm.ShowDialog(this);
                    }
                    string exportfilename = datePicker.Value.ToString("YYYY", CultureInfo.InvariantCulture) + ".pdf";
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

        private void CreateYearPDF()
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

            Bitmap bmpYearTransit = new Bitmap(Convert.ToInt32(page.Width.Point - 20), Convert.ToInt32(page.Height.Point - 100));
            bmpYearTransit = (Bitmap)_currentYearTranzitsImage;
            XImage imageYearTransit = XImage.FromStream(GetStream(bmpYearTransit, ImageFormat.Bmp));
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
            {
                return;
            }
            else
            {
                using (WaitForm wForm = new WaitForm(RefreshCalendar, _activeLanguageCode))
                {
                    wForm.ShowDialog(this);
                }
            }
        }

        private void RefreshCalendar()
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
            }
            _daysList = PrepareMonthDays(new DateTime(_selectedDate.Year, _selectedDate.Month, 1), _selectedProfile);

            //Drawing
            PrepareProfileAndTimeZoneLabels();
            CalendarDrawing(_daysList);
            TranzitDrawing(_daysList);

            // refresh dayView if opened and years calendar
            if (tabControlCalendar.TabPages.Count > 2)
            {
                string calendarText = Utility.GetLocalizedText("Calendar", _activeLanguageCode);
                string tranzitText = Utility.GetLocalizedText("Transits", _activeLanguageCode);
                string yearTranzitText = Utility.GetLocalizedText("Year's transits", _activeLanguageCode);

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
                                List<Day> daysList = yt.PrepareYearDays(year);
                                yt.YearTranzitDrawing(daysList);
                                yt.Refresh();
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
                foreach (YogaCalendar jc in day.DwipushkarJogaDayList)
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
                foreach (YogaCalendar jc in day.TripushkarJogaDayList)
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
                foreach (YogaCalendar jc in day.AmritaSiddhaJogaDayList)
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
                foreach (YogaCalendar jc in day.SarvarthaSiddhaJogaDayList)
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
                foreach (YogaCalendar jc in day.SiddhaJogaDayList)
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
                foreach (YogaCalendar jc in day.MrityuJogaDayList)
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
                foreach (YogaCalendar jc in day.AdhamJogaDayList)
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
                foreach (YogaCalendar jc in day.YamaghataJogaDayList)
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
                foreach (YogaCalendar jc in day.DagdhaJogaDayList)
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
                foreach (YogaCalendar jc in day.UnfarobaleJogaDayList)
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
                int colNumber = e.X / Convert.ToInt32(_dayWidth);
                int rowNumber = (e.Y - Convert.ToInt32(_daysOfWeekHeight)) / Convert.ToInt32(_dayHeight);
                int selectedDayIndex = rowNumber * 7 + colNumber;

                int formWidth = 700;
                int linesCount = 0;
                int formHeight = CalculateCalendarTooltipFormHeight(_daysList[selectedDayIndex], out linesCount);

                float posX = colNumber * _dayWidth + 5;
                float posY = ((rowNumber + 1) * _dayHeight - _dayHeight) + _daysOfWeekHeight + 5;

                if ((posX + formWidth) > pictureBoxCalendar.Width)
                    posX = posX + _dayWidth - formWidth - 10;

                if ((posY + formHeight) > pictureBoxCalendar.Height)
                    posY = posY + _dayHeight - formHeight - 10;

                CalendarTooltipCreation(_selectedProfile, _daysList[selectedDayIndex], formHeight, linesCount, _activeLanguageCode);
                toolTip.Show(pictureBoxCalendar, Convert.ToInt32(posX), Convert.ToInt32(posY));
            }

            if (e.Button == MouseButtons.Left)
            {
                int colNumber = e.X / Convert.ToInt32(_dayWidth);
                int rowNumber = (e.Y - Convert.ToInt32(_daysOfWeekHeight)) / Convert.ToInt32(_dayHeight);
                int selectedDayIndex = rowNumber * 7 + colNumber;

                DateTime date = _daysList[selectedDayIndex].Date;
                List<PersonEvent> peDayList = Utility.GetDayPersonEvents(_selectedProfile.Id, date);
                if (peDayList.Count > 0)
                {
                    if ((e.X > _dayWidth + colNumber * _dayWidth - 10) && (e.X < _dayWidth + colNumber * _dayWidth) &&
                        (e.Y > _daysOfWeekHeight + rowNumber * _dayHeight) && (e.Y < _daysOfWeekHeight + rowNumber * _dayHeight + 10)
                        )
                    {
                        float posX = colNumber * _dayWidth + _dayWidth - 255;
                        float posY = ((rowNumber + 1) * _dayHeight - _dayHeight) + _daysOfWeekHeight + 5;

                        if (posX < 0)
                            posX = 5;

                        if ((posY + 50) > pictureBoxCalendar.Height)
                            posY = posY + _dayHeight - 50 - 10;

                        PersonEventToolTipCreation(peDayList);
                        toolTip.Show(pictureBoxCalendar, Convert.ToInt32(posX), Convert.ToInt32(posY));
                    }
                }
            }
        }

        private void pictureBoxCalendar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null || e.Y < _daysOfWeekHeight || (e.Y >= (_daysOfWeekHeight + 6 * _dayHeight) - 1) || e.X < 0 || e.X >= 7 * _dayWidth)
                return;

            int colNumber = e.X / Convert.ToInt32(_dayWidth);
            int rowNumber = (e.Y - Convert.ToInt32(_daysOfWeekHeight)) / Convert.ToInt32(_dayHeight);
            int selectedDayIndex = rowNumber * 7 + colNumber;

            if (selectedDayIndex < 0 || selectedDayIndex >= 42)
                return;

            DateTime date = _daysList[selectedDayIndex].Date;
            List<PersonEvent> peDayList = Utility.GetDayPersonEvents(_selectedProfile.Id, date);
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

            int colNumber = e.X / Convert.ToInt32(_dayWidth);
            int rowNumber = (e.Y - Convert.ToInt32(_daysOfWeekHeight)) / Convert.ToInt32(_dayHeight);
            int selectedDayIndex = rowNumber * 7 + colNumber;

            OpenDayTab(selectedDayIndex);
        }

        private void pictureBoxTranzits_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null)
                return;

            int day = (e.X - Convert.ToInt32(_labelsWidth)) / Convert.ToInt32(_dayTranzWidth);
            DateTime selectedDate = _daysOfMonth[day].Date;
            int selectedDayIndex = _daysList.FindIndex(i => i.Date == selectedDate);
            OpenDayTab(selectedDayIndex);
        }

        private enum TranzitEntity { TEEmpty, TEMasa, TENakshatra, TETaraBala, TETithi, TEKarana, TENityaYoga, TEYoga, TEMoon, TESun, TEVenus, TEJupiter, TEMercury, TEMars, TESaturn, TERahu, TEKetu }

        private void pictureBoxTranzits_MouseClick(object sender, MouseEventArgs e)
        {
            if (_selectedProfile == null || _daysList == null)
                return;

            if (e.Button == MouseButtons.Right)
            {
                float posX = _labelsWidth, posY = 0;

                int day = (e.X - Convert.ToInt32(_labelsWidth)) / Convert.ToInt32(_dayTranzWidth);

                // Finding line by position
                float posYMasa = posY + 2 * _lineTranzHeight + 4;
                float posYPanchanga = posYMasa + _lineTranzHeight + 4;
                float posYYoga = posYPanchanga + 6 * _lineTranzHeight + 4;
                float posYMoon = posYYoga + _lineTranzHeight + 4;
                float posYSun = posYMoon + 4 * _lineTranzHeight + 4;
                float posYVenus = posYSun + 4 * _lineTranzHeight + 4;
                float posYJupiter = posYVenus + 4 * _lineTranzHeight + 4;
                float posYMercury = posYJupiter + 4 * _lineTranzHeight + 4;
                float posYMars = posYMercury + 4 * _lineTranzHeight + 4;
                float posYSaturn = posYMars + 4 * _lineTranzHeight + 4;
                float posYRahu = posYSaturn + 4 * _lineTranzHeight + 4;
                float posYKetu = posYRahu + 4 * _lineTranzHeight + 4;

                float dayHeight = posYKetu + 4 *_lineTranzHeight + 1;

                // Draw blue rectangle for selected day through all lines
                pictureBoxTranzits.Image = _currentTranzitsImage;
                Bitmap canvas = new Bitmap(pictureBoxTranzits.Image);
                Graphics g = Graphics.FromImage(canvas);
                Pen pen = new Pen(Color.FromArgb(CacheLoad._colorList.Where(i => i.Code.Equals(EColor.SELECTRECTANGLE.ToString())).FirstOrDefault().ARGBValue), 1);
                g.DrawRectangle(pen, posX + day * _dayTranzWidth, posY, _dayTranzWidth, dayHeight);
                pictureBoxTranzits.Image = canvas;

                TranzitEntity trEnt = TranzitEntity.TEEmpty;
                if (e.Y > posYMasa && e.Y < posYMasa + _lineTranzHeight)
                    trEnt = TranzitEntity.TEMasa;
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
                toolTip = null;
                int ttposX = 0, ttposY = 0, height = 0;
                int formWidth = pictureBoxTranzits.Width;
                switch (trEnt)
                {
                    case TranzitEntity.TEMasa:
                        MasaTranzitsTooltipCreation(_daysOfMonth[day].MasaDayList.ToList(), _daysOfMonth[day].ShunyaNakshatraDayList.ToList(), _daysOfMonth[day].ShunyaTithiDayList.ToList(), formWidth, out height, TranzitEntity.TEMasa, _activeLanguageCode);
                        break;

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
            column.DataPropertyName = "Transit";
            column.Name = Utility.GetLocalizedText("Transit", langCode) + " " + planetName;
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
            column.Name = Utility.GetLocalizedText("Zodiac Sign", langCode);
            column.Width = 120;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Nakshatra";
            column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
            column.Width = 160;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Pada";
            column.Name = Utility.GetLocalizedText("Pada", langCode);
            column.Width = 80;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            int lastColWidth = (dgv.Width - 1330);
            column = new DataGridViewColumn();
            column.DataPropertyName = "Description";
            column.Name = Utility.GetLocalizedText("Description", langCode);
            column.Width = lastColWidth;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Vedha";
            column.Name = Utility.GetLocalizedText("Vedha from", langCode);
            column.Width = 330;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "MrityuBhaga";
            column.Name = Utility.GetLocalizedText("Mrityu Bhaga", langCode);
            column.Width = 260;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dgv.Columns.Add(column);

            List<PlanetCalendar> clonedZodiakCal = null;
            List<PlanetCalendar> clonedZodiakRetroCal = null;
            List<PlanetCalendar> clonedNakshatraCal = null;
            List<PlanetCalendar> clonedPadaCal = null;

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
                    clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.MoonPadaDayList);
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
                    clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.SunPadaDayList);
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
                    clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.VenusPadaDayList);
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
                    clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.JupiterPadaDayList);
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
                    clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.MercuryPadaDayList);
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
                    clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.MarsPadaDayList);
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
                    clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.SaturnPadaDayList);
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
                            clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.RahuMeanPadaDayList);
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
                            clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.RahuTruePadaDayList);
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
                            clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.KetuMeanPadaDayList);
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
                            clonedPadaCal = Utility.ClonePlanetCalendarList(pDay.KetuTruePadaDayList);
                            break;
                    }
                    break;
            }

            dgv = PlanetTranzitDataGridViewFillByRow(dgv, pDay, clonedZodiakCal, clonedZodiakRetroCal, clonedNakshatraCal, clonedPadaCal, tranzitSetting, nodeSettings, langCode);
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
            public string Pada { get; set; }
            public string Description { get; set; }
            public string Vedha { get; set; }
            public string MrityuBhaga { get; set; }
        }

        private DataGridView PlanetTranzitDataGridViewFillByRow(DataGridView dgv, Day pDay, List<PlanetCalendar> pzList, List<PlanetCalendar> pzrList, List<PlanetCalendar> pnList, List<PlanetCalendar> ppList, EAppSetting tranzitSetting, EAppSetting nodeSettings, ELanguage langCode)
        {
            List<dgvRowObj> rowList = new List<dgvRowObj>();
            int planetId = 0;
            foreach (PlanetCalendar pc in pzrList)
            {
                planetId = (int)pc.PlanetCode;
                if (planetId == 10)
                {
                    planetId = 8;
                }
                if (planetId == 11)
                {
                    planetId = 9;
                }
                string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == planetId && i.Dom == pc.Dom).FirstOrDefault();
                string trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                string vedha = GetVedhaList(pDay, pc, tr, tranzitSetting, nodeSettings, langCode, false);
                string mrityuBhaga = GetMrityuBhaga(pDay, pc.PlanetCode, langCode);
                dgvRowObj rowTemp = new dgvRowObj {
                    Entity = Utility.GetLocalizedText("Zodiac Sign", langCode),
                    DateStart = pc.DateStart,
                    DateEnd = pc.DateEnd,
                    Zodiac = zodiak,
                    Nakshatra = string.Empty,
                    Pada = string.Empty,
                    Description = trDesc,
                    Vedha = vedha,
                    MrityuBhaga = mrityuBhaga
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
                    Pada = string.Empty,
                    Description = string.Empty,
                    Vedha = string.Empty,
                    MrityuBhaga = string.Empty
                };
                rowList.Add(rowTemp);
            }
            foreach (PlanetCalendar pc in ppList)
            {
                string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                string nakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)pc.NakshatraCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                Pada pada = CacheLoad._padaList.Where(i => i.Id == pc.PadaId).FirstOrDefault();
                string specNavamsha = Utility.GetSpecNavamsha(pada, langCode);
                string badNavamsha = Utility.GetBadNavamsha(pada.Id, langCode);
                if (badNavamsha.Length > 0)
                {
                    badNavamsha = ";  " +  badNavamsha.Substring(0, (badNavamsha.Length - 2));
                }
                string drekkana = string.Empty;
                List<DrekkanaEntity> deList = Utility.GetBadDrekkanaList(pada.Id);
                if (deList.Count > 0)
                {
                    for (int i = 0; i < deList.Count; i++)
                    {
                        if (!deList[i].IsLagna)
                        {
                            drekkana += deList[i].Drekkana + Utility.GetLocalizedText("Drekkana from Natal Moon", langCode) + ", ";
                        }
                        else
                        {
                            drekkana += deList[i].Drekkana + Utility.GetLocalizedText("Drekkana from Lagna", langCode) + ", ";
                        }
                    }
                }
                if (drekkana.Length > 0)
                {
                    drekkana = ";  " + drekkana.Substring(0, (drekkana.Length - 2));
                }

                dgvRowObj rowTemp = new dgvRowObj
                {
                    Entity = Utility.GetLocalizedText("Pada", langCode),
                    DateStart = pc.DateStart,
                    DateEnd = pc.DateEnd,
                    Zodiac = zodiak,
                    Nakshatra = (int)pc.NakshatraCode + "." + nakshatra,
                    Pada = pada.PadaNumber.ToString(),
                    Description = Utility.GetLocalizedText("Navamsa", langCode) + ": " + pada.Navamsha + pc.GetNavamshaExaltation() + " " + specNavamsha + badNavamsha + drekkana,
                    Vedha = string.Empty,
                    MrityuBhaga = string.Empty
                };
                rowList.Add(rowTemp);
            }
            
            for (int i = 0; i < rowList.Count; i++)
            {
                string[] row = new string[] {
                        rowList[i].Entity,
                        rowList[i].DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        rowList[i].DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        rowList[i].Zodiac,
                        rowList[i].Nakshatra,
                        rowList[i].Pada,
                        rowList[i].Description,
                        rowList[i].Vedha,
                        rowList[i].MrityuBhaga
                    };
                dgv.Rows.Add(row);
            }
            return dgv;
        }

        private string GetMrityuBhaga(Day pDay, EPlanet planetCode, ELanguage lCode)
        {
            string mb = string.Empty;
            MrityuBhagaData mbData;
            switch (planetCode)
            {
                case EPlanet.MOON:
                    mbData = pDay.MoonMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.SUN:
                    mbData = pDay.SunMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.VENUS:
                    mbData = pDay.VenusMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.JUPITER:
                    mbData = pDay.JupiterMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.MERCURY:
                    mbData = pDay.MercuryMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.MARS:
                    mbData = pDay.MarsMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.SATURN:
                    mbData = pDay.SaturnMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.RAHUMEAN:
                    mbData = pDay.RahuMeanMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.KETUMEAN:
                    mbData = pDay.KetuMeanMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.RAHUTRUE:
                    mbData = pDay.RahuTrueMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;

                case EPlanet.KETUTRUE:
                    mbData = pDay.KetuTrueMrityuBhagaDayList.FirstOrDefault();
                    if (mbData != null)
                        mb = mbData.DateFrom.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mbData.DateTo.ToString("dd.MM.yyyy HH:mm:ss");
                    break;
            }
            return mb;
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
            foreach (YogaCalendar jc in jcList)
            {
                if (jc.DateStart < date.AddDays(+1))
                {
                    DGVYogaRow row = new DGVYogaRow
                    {
                        DateStart = jc.DateStart,
                        DateEnd = jc.DateEnd,
                        Name = CacheLoad._yogaDescList.Where(i => i.YogaId == (int)jc.YogaCode && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty,
                        Vara = Utility.GetDaysOfWeekName((int)jc.Vara, lang),
                        Nakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)jc.NakshatraCode && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty,
                        Tithi = CacheLoad._tithiDescList.Where(i => i.TithiId == jc.TithiId && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty,
                        Description = CacheLoad._yogaDescList.Where(i => i.YogaId == (int)jc.YogaCode && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Description ?? string.Empty
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

        private void MasaTranzitsTooltipCreation(List<Calendar> mcList, List<Calendar> sncList, List<Calendar> stcList, int width, out int height, TranzitEntity tEnt, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = PrepareMasaDataGridView(mcList, sncList, stcList, width, out height, langCode);
            if (dgv != null)
            {
                toolTip = new Popup(tranzitsToolTip = new TranzitsToolTip(dgv, width, (height + 5)));
                toolTip.AutoClose = false;
                toolTip.FocusOnOpen = false;
                toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
                toolTip.AutoClose = true;
            }
        }

        private DataGridView PrepareMasaDataGridView(List<Calendar> mcList, List<Calendar> sncList, List<Calendar> stcList, int width, out int height, ELanguage langCode)
        {
            height = 0;
            DataGridView dgv = new DataGridView();
            dgv.Width = width;

            Font headerFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
            Font textFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPTEXT));

            if (mcList != null)
            {
                //DGV build
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = headerFont;
                dgv.DefaultCellStyle.Font = textFont;

                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = "Masa_Shunya";
                column.Name = Utility.GetLocalizedText("Masa/Shunya", langCode);
                column.Width = 200;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Names";
                column.Name = Utility.GetLocalizedText("Names", langCode);
                column.Width = 250;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateStart";
                column.Name = Utility.GetLocalizedText("Start", langCode);
                column.Width = 150;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "DateEnd";
                column.Name = Utility.GetLocalizedText("End", langCode);
                column.Width = 150;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "FullMoon";
                column.Name = Utility.GetLocalizedText("Full Moon Nakshatra", langCode);
                column.Width = 250;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                column = new DataGridViewColumn();
                column.DataPropertyName = "Description";
                column.Name = ""; //Utility.GetLocalizedText("", langCode);
                column.Width = dgv.Width - 1000;
                column.CellTemplate = new DataGridViewTextBoxCell();
                dgv.Columns.Add(column);

                List<MasaCalendar> clonedMCList = Utility.CloneMasaCalendarList(mcList);
                List<ShunyaNakshatraCalendar> clonedSNCList = Utility.CloneShunyaNakshatraCalendarList(sncList);
                List<ShunyaTithiCalendar> clonedSTCList = Utility.CloneShunyaTithiCalendarList(stcList);

                
                foreach (MasaCalendar mc in clonedMCList)
                {
                    string masaName = CacheLoad._masaDescList.Where(i => i.MasaId == mc.MasaId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    int nakFullMoonId = Utility.GetNakshatraFullMoonId(mc, _moonNakshatraCalendar);
                    string upravitel = Utility.GetNakshatraUprvitel(nakFullMoonId, langCode);
                    string nakFullMoon = nakFullMoonId + "." + GetNakshatraFullMoon(nakFullMoonId, langCode);
                    string[] row = new string[] {
                        Utility.GetLocalizedText("Masa", langCode),
                        masaName + " (" + upravitel + ")",
                        mc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        mc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        nakFullMoon + " (" + upravitel + ")",
                        string.Empty
                    };
                    dgv.Rows.Add(row);
                }
                

                foreach (ShunyaNakshatraCalendar snc in clonedSNCList)
                {
                    string nakName = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)snc.NakshatraCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    string upravitel = Utility.GetNakshatraUprvitel((int)snc.NakshatraCode, langCode);
                    string[] row = new string[] {
                        Utility.GetLocalizedText("Shunya Nakshatra", langCode),
                        (int)snc.NakshatraCode + "." + nakName + " (" + upravitel + ")",
                        snc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        snc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        string.Empty,
                        string.Empty
                    };
                    dgv.Rows.Add(row);
                }

                foreach (ShunyaTithiCalendar stc in clonedSTCList)
                {
                    string tiName = CacheLoad._tithiDescList.Where(i => i.TithiId == stc.TithiId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    string upravitel = GetTithiUprvitel(stc.TithiId, langCode);
                    string[] row = new string[] {
                        Utility.GetLocalizedText("Shunya Tithi", langCode),
                        stc.TithiId + "." + tiName + " (" + upravitel + ")",
                        stc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        stc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        string.Empty,
                        string.Empty
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

        private string GetNakshatraFullMoon(int nId, ELanguage lCode)
        {
            return CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == nId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        private string GetTithiUprvitel(int tId, ELanguage lCode)
        {
            string upravitel = string.Empty;
            string fullUpravitel = CacheLoad._tithiDescList.Where(i => i.TithiId == tId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Upravitel ?? string.Empty;
            if (!fullUpravitel.Equals(string.Empty))
            {
                var row = fullUpravitel.Split(new char[] { ',' });
                upravitel = row[0];
            }
            return upravitel;
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

                NityaYogaDescription currentNityaYoga;
                foreach (NityaYogaCalendar njc in cList)
                {
                    currentNityaYoga = CacheLoad._nityaYogaDescList.Where(i => i.NityaYogaId == (int)njc.NYCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault();
                    int yogaPlanetId = CacheLoad._nityaYogaList.Where(i => i.Id == (int)njc.NYCode).FirstOrDefault()?.YogiPlanetId ?? 0;
                    string upravitel = CacheLoad._planetDescList.Where(i => i.PlanetId == yogaPlanetId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    string nakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == njc.NakshatraId && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    string[] row = new string[] {
                        njc.DateStart.ToString("dd.MM.yyyy HH:mm:ss"),
                        njc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"),
                        currentNityaYoga.NityaYogaId + "." + currentNityaYoga.Name + " (" + upravitel + ", " + currentNityaYoga.Deity + ")",
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

        private void PersonEventToolTipCreation(List<PersonEvent> peList)
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

            foreach (PersonEvent pe in peList)
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
                List<PersonEvent> pevList = Utility.GetDayPersonEvents(_selectedProfile.Id, _daysList[selectedDayIndex].Date); 
                List<DVLineNameDescription> dvlDescList = CacheLoad._dvLineNamesDescList.Where(i => i.LanguageCode.Equals(_activeLanguageCode.ToString())).ToList();
                TabPage newTab = new TabPage() { Name = "day" + selectedDayIndex.ToString(), Text = tabLabel };
                TabDay tabForm = new TabDay(_daysList[selectedDayIndex], _selectedProfile, CacheLoad._dvLineNamesList.ToList(), dvlDescList, pevList, _activeLanguageCode);
                tabForm.TopLevel = false;
                tabForm.Parent = newTab;
                tabControlCalendar.TabPages.Add(newTab);
                tabControlCalendar.SelectedTab = newTab;
                tabForm.Show();
                tabForm.Dock = DockStyle.Fill;
            }
        }

        private void OpenTransitChartTab()
        {
            int index = -1;
            bool isPresent = false;
            string tabLabel = Utility.GetLocalizedText("Transit Chart", _activeLanguageCode);

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
                TabPage newTab = new TabPage() { Name = tabLabel, Text = tabLabel };
                TransitsMap tabForm = new TransitsMap(_selectedProfile, _activeLanguageCode);
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
            YearTranzitSelect ysForm = new YearTranzitSelect(_activeLanguageCode);
            ysForm.ShowDialog();
            _selectedYear = ysForm.SelectedYear;
            using (WaitForm wForm = new WaitForm(PrepareYearTranzitData, _activeLanguageCode))
            {
                wForm.ShowDialog(this);
            }
        }

        private void PrepareYearTranzitData()
        {
            DateTime startDate = new DateTime(_selectedYear, 1, 1).AddDays(-1);
            DateTime endDate = new DateTime((_selectedYear + 1), 1, 1).AddDays(+1);
            EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;

            double latitude, longitude;
            string timeZone = string.Empty;
            if (Utility.GetGeoCoordinateByLocationId(_selectedProfile.PlaceOfLivingId, out latitude, out longitude))
            {
                timeZone = Utility.GetTimeZoneDotNetIdByGeoCoordinates(latitude, longitude);
                TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                TimeZoneInfo.AdjustmentRule[] adjustmentRules = currentTimeZone.GetAdjustmentRules();

                EpheCalculation eCalc = new EpheCalculation();

                //calculate data lists
                List<PlanetData> moonDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MOON, startDate, endDate);
                List<PlanetData> sunDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_SUN, startDate, endDate);
                List<PlanetData> mercuryDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MERCURY, startDate, endDate);
                List<PlanetData> venusDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_VENUS, startDate, endDate);
                List<PlanetData> marsDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MARS, startDate, endDate);
                List<PlanetData> jupiterDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_JUPITER, startDate, endDate);
                List<PlanetData> saturnDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_SATURN, startDate, endDate);
                List<PlanetData> rahuMeanDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_MEAN_NODE, startDate, endDate);
                List<PlanetData> rahuTrueDataList = eCalc.CalculatePlanetDataList_London(EpheConstants.SE_TRUE_NODE, startDate, endDate);
                List<PlanetData> ketuMeanDataList = eCalc.PrepareKetuList(rahuMeanDataList);
                List<PlanetData> ketuTrueDataList = eCalc.PrepareKetuList(rahuTrueDataList);
                List<TithiData> tithiDataList = eCalc.CalculateTithiDataList_London(startDate, endDate);

                List<MrityuBhagaData> moonMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.MOON, startDate, endDate);
                List<MrityuBhagaData> sunMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.SUN, startDate, endDate);
                List<MrityuBhagaData> mercuryMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.MERCURY, startDate, endDate);
                List<MrityuBhagaData> venusMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.VENUS, startDate, endDate);
                List<MrityuBhagaData> marsMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.MARS, startDate, endDate);
                List<MrityuBhagaData> jupiterMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.JUPITER, startDate, endDate);
                List<MrityuBhagaData> saturnMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.SATURN, startDate, endDate);
                List<MrityuBhagaData> rahuMeanMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.RAHUMEAN, startDate, endDate);
                List<MrityuBhagaData> rahuTrueMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.RAHUTRUE, startDate, endDate);
                List<MrityuBhagaData> ketuMeanMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.KETUMEAN, startDate, endDate);
                List<MrityuBhagaData> ketuTrueMBDataList = eCalc.CalculateMrityuBhagaDataList_London(CacheLoad._mrityuBhagaList, EPlanet.KETUTRUE, startDate, endDate);

                //prepare Calendars
                List<NakshatraCalendar> nakshatraCalendarList = CacheLoad.CreateNakshatraCalendarList(moonDataList);
                nakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                nakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<TithiCalendar> tithiCalendarList = CacheLoad.CreateTithiCalendarList(tithiDataList);
                tithiCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                tithiCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });
               
                List<PlanetCalendar> moonZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MOON, moonDataList);
                List<MasaCalendar> masaCalendarList = CreateMasaCalendarList(moonZodiakCalendarList, tithiDataList);
                masaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                masaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });
                List<ShunyaNakshatraCalendar> shunyaNakshatraCalendarList = CreateShunyaNakshatraCalendarList(masaCalendarList, nakshatraCalendarList);
                List<ShunyaTithiCalendar> shunyaTithiCalendarList = CreateShunyaTithiCalendarList(masaCalendarList, tithiCalendarList);
                moonZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MOON, moonDataList);
                moonZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MOON, moonDataList);
                moonNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });
                _moonNakshatraCalendar = Utility.ClonePlanetCalendarList(moonNakshatraCalendarList.ToList());

                List<PlanetCalendar> moonPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MOON, moonDataList);
                moonPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.SUN, sunDataList);
                sunZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.SUN, sunDataList);
                sunZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.SUN, sunDataList);
                sunNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.SUN, sunDataList);
                sunPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MERCURY, mercuryDataList);
                mercuryPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.VENUS, venusDataList);
                venusZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.VENUS, venusDataList);
                venusZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.VENUS, venusDataList);
                venusNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.VENUS, venusDataList);
                venusPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.MARS, marsDataList);
                marsZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.MARS, marsDataList);
                marsZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.MARS, marsDataList);
                marsNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.MARS, marsDataList);
                marsPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.JUPITER, jupiterDataList);
                jupiterPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.SATURN, saturnDataList);
                saturnZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.SATURN, saturnDataList);
                saturnZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.SATURN, saturnDataList);
                saturnNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.SATURN, saturnDataList);
                saturnPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.RAHUMEAN, rahuMeanDataList);
                rahuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanPadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.KETUMEAN, ketuMeanDataList);
                ketuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanPadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTruePadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.RAHUTRUE, rahuTrueDataList);
                rahuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakCalendarList = CacheLoad.CreatePlanetZodiakCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakRetroCalendarList = CacheLoad.CreatePlanetZodiakRetroCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakRetroCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueNakshatraCalendarList = CacheLoad.CreatePlanetNakshatraCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueNakshatraCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTruePadaCalendarList = CacheLoad.CreatePlanetPadaCalendarList(EPlanet.KETUTRUE, ketuTrueDataList);
                ketuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTruePadaCalendarList.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                //Shifting MrityaBhaga by TimeZone
                moonMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                sunMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                mercuryMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                venusMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                marsMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                jupiterMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                saturnMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                rahuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                rahuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                ketuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                ketuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateTo = i.DateTo.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueMBDataList.ForEach(i => { i.DateFrom = i.DateFrom.ShiftByDaylightDelta(adjustmentRules); i.DateTo = i.DateTo.ShiftByDaylightDelta(adjustmentRules); });

                int index = -1;
                bool isPresent = false;
                string tabLabel = Utility.GetLocalizedText("Year's transits", _activeLanguageCode) + ": " + _selectedYear;
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
                        TabPage newTab = new TabPage() { Name = "yearTranzits" + ": " + _selectedYear, Text = tabLabel };

                        YearTranzits tabForm = new YearTranzits(
                                _selectedYear,
                                _activeLanguageCode,
                                moonZodiakCalendarList,
                                moonZodiakRetroCalendarList,
                                moonNakshatraCalendarList,
                                moonPadaCalendarList,
                                sunZodiakCalendarList,
                                sunZodiakRetroCalendarList,
                                sunNakshatraCalendarList,
                                sunPadaCalendarList,
                                mercuryZodiakCalendarList,
                                mercuryZodiakRetroCalendarList,
                                mercuryNakshatraCalendarList,
                                mercuryPadaCalendarList,
                                venusZodiakCalendarList,
                                venusZodiakRetroCalendarList,
                                venusNakshatraCalendarList,
                                venusPadaCalendarList,
                                marsZodiakCalendarList,
                                marsZodiakRetroCalendarList,
                                marsNakshatraCalendarList,
                                marsPadaCalendarList,
                                jupiterZodiakCalendarList,
                                jupiterZodiakRetroCalendarList,
                                jupiterNakshatraCalendarList,
                                jupiterPadaCalendarList,
                                saturnZodiakCalendarList,
                                saturnZodiakRetroCalendarList,
                                saturnNakshatraCalendarList,
                                saturnPadaCalendarList,
                                rahuMeanZodiakCalendarList,
                                rahuMeanZodiakRetroCalendarList,
                                rahuMeanNakshatraCalendarList,
                                rahuMeanPadaCalendarList,
                                ketuMeanZodiakCalendarList,
                                ketuMeanZodiakRetroCalendarList,
                                ketuMeanNakshatraCalendarList,
                                ketuMeanPadaCalendarList,
                                rahuTrueZodiakCalendarList,
                                rahuTrueZodiakRetroCalendarList,
                                rahuTrueNakshatraCalendarList,
                                rahuTruePadaCalendarList,
                                ketuTrueZodiakCalendarList,
                                ketuTrueZodiakRetroCalendarList,
                                ketuTrueNakshatraCalendarList,
                                ketuTruePadaCalendarList,
                                masaCalendarList,
                                shunyaNakshatraCalendarList,
                                shunyaTithiCalendarList,
                                moonMBDataList,
                                sunMBDataList,
                                mercuryMBDataList,
                                venusMBDataList,
                                marsMBDataList,
                                jupiterMBDataList,
                                saturnMBDataList,
                                rahuMeanMBDataList,
                                rahuTrueMBDataList,
                                ketuMeanMBDataList,
                                ketuTrueMBDataList
                            );

                        Invoke(new Action(() =>
                        {
                            tabForm.MFormAccess = this;
                            tabForm.TopLevel = false;
                            tabForm.Parent = newTab;
                            tabControlCalendar.TabPages.Add(newTab);
                            tabControlCalendar.SelectedTab = newTab;
                            tabForm.Show();
                            tabForm.Dock = DockStyle.Fill;
                        }));
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

        private void swephCalcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sweph_Calc scForm = new Sweph_Calc(CacheLoad._mrityuBhagaList.ToList());
            scForm.ShowDialog(this);
        }

        private void tranzitsMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTransitChartTab();
            
            //TransitsMap tmForm = new TransitsMap(_selectedProfile, _activeLanguageCode);
            //tmForm.ShowDialog(this);
        }

        
    }
}
