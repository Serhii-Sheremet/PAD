using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class YearTranzits : Form
    {
        
        MainForm mform; 
        public MainForm MFormAccess
        {
            get { return mform; }
            set { mform = value; }
        }
        
        private ELanguage _langCode;
        private Profile _sProfile;
        private int _sYear;
        private List<Day> dayList;

        //Calendars
        private List<MasaCalendar> _mCList;
        private List<ShunyaNakshatraCalendar> _sNCList;
        private List<ShunyaTithiCalendar> _sTCList;

        private List<PlanetCalendar> _moonZCList;
        private List<PlanetCalendar> _moonZRCList;
        private List<PlanetCalendar> _moonNCList;
        private List<PlanetCalendar> _moonPCList;

        private List<PlanetCalendar> _sunZCList;
        private List<PlanetCalendar> _sunZRCList;
        private List<PlanetCalendar> _sunNCList;
        private List<PlanetCalendar> _sunPCList;

        private List<PlanetCalendar> _mercuryZCList;
        private List<PlanetCalendar> _mercuryZRCList;
        private List<PlanetCalendar> _mercuryNCList;
        private List<PlanetCalendar> _mercuryPCList;

        private List<PlanetCalendar> _venusZCList;
        private List<PlanetCalendar> _venusZRCList;
        private List<PlanetCalendar> _venusNCList;
        private List<PlanetCalendar> _venusPCList;

        private List<PlanetCalendar> _marsZCList;
        private List<PlanetCalendar> _marsZRCList;
        private List<PlanetCalendar> _marsNCList;
        private List<PlanetCalendar> _marsPCList;

        private List<PlanetCalendar> _jupiterZCList;
        private List<PlanetCalendar> _jupiterZRCList;
        private List<PlanetCalendar> _jupiterNCList;
        private List<PlanetCalendar> _jupiterPCList;

        private List<PlanetCalendar> _saturnZCList;
        private List<PlanetCalendar> _saturnZRCList;
        private List<PlanetCalendar> _saturnNCList;
        private List<PlanetCalendar> _saturnPCList;

        private List<PlanetCalendar> _rahuMeanZCList;
        private List<PlanetCalendar> _rahuMeanZRCList;
        private List<PlanetCalendar> _rahuMeanNCList;
        private List<PlanetCalendar> _rahuMeanPCList;

        private List<PlanetCalendar> _ketuMeanZCList;
        private List<PlanetCalendar> _ketuMeanZRCList;
        private List<PlanetCalendar> _ketuMeanNCList;
        private List<PlanetCalendar> _ketuMeanPCList;

        private List<PlanetCalendar> _rahuTrueZCList;
        private List<PlanetCalendar> _rahuTrueZRCList;
        private List<PlanetCalendar> _rahuTrueNCList;
        private List<PlanetCalendar> _rahuTruePCList;

        private List<PlanetCalendar> _ketuTrueZCList;
        private List<PlanetCalendar> _ketuTrueZRCList;
        private List<PlanetCalendar> _ketuTrueNCList;
        private List<PlanetCalendar> _ketuTruePCList;

        private List<MrityuBhagaData> _moonMBList;
        private List<MrityuBhagaData> _sunMBList;
        private List<MrityuBhagaData> _mercuryMBList;
        private List<MrityuBhagaData> _venusMBList;
        private List<MrityuBhagaData> _marsMBList;
        private List<MrityuBhagaData> _jupiterMBList;
        private List<MrityuBhagaData> _saturnMBList;
        private List<MrityuBhagaData> _rahuMeanMBList;
        private List<MrityuBhagaData> _rahuTrueMBList;


        private float lineHeight;
        int labelsWidth;
        float dayWidth;
        private float monthLabelHeight;
        private Image _currentImage;
        private float[] monthWidthArray;


        public YearTranzits()
        {
            InitializeComponent();
        }

        private void YearTranzits_Shown(object sender, EventArgs e)
        {
            using (WaitForm wForm = new WaitForm(DrawYearTranzits, _langCode))
            {
                wForm.ShowDialog(this);
            }
        }

        private void DrawYearTranzits()
        {
            dayList = PrepareYearDays(_sYear, _sProfile);
            YearTranzitDrawing(dayList);
        }

        public YearTranzits(
            int year,
            ELanguage langCode,
            Profile sProfile,
            List<PlanetCalendar> moonZCList,
            List<PlanetCalendar> moonZRCList,
            List<PlanetCalendar> moonNCList,
            List<PlanetCalendar> moonPCList,
            List<PlanetCalendar> sunZCList,
            List<PlanetCalendar> sunZRCList,
            List<PlanetCalendar> sunNCList,
            List<PlanetCalendar> sunPCList,
            List<PlanetCalendar> mercuryZCList,
            List<PlanetCalendar> mercuryZRCList,
            List<PlanetCalendar> mercuryNCList,
            List<PlanetCalendar> mercuryPCList,
            List<PlanetCalendar> venusZCList,
            List<PlanetCalendar> venusZRCList,
            List<PlanetCalendar> venusNCList,
            List<PlanetCalendar> venusPCList,
            List<PlanetCalendar> marsZCList,
            List<PlanetCalendar> marsZRCList,
            List<PlanetCalendar> marsNCList,
            List<PlanetCalendar> marsPCList,
            List<PlanetCalendar> jupiterZCList,
            List<PlanetCalendar> jupiterZRCList,
            List<PlanetCalendar> jupiterNCList,
            List<PlanetCalendar> jupiterPCList,
            List<PlanetCalendar> saturnZCList,
            List<PlanetCalendar> saturnZRCList,
            List<PlanetCalendar> saturnNCList,
            List<PlanetCalendar> saturnPCList,
            List<PlanetCalendar> rahuMeanZCList,
            List<PlanetCalendar> rahuMeanZRCList,
            List<PlanetCalendar> rahuMeanNCList,
            List<PlanetCalendar> rahuMeanPCList,
            List<PlanetCalendar> ketuMeanZCList,
            List<PlanetCalendar> ketuMeanZRCList,
            List<PlanetCalendar> ketuMeanNCList,
            List<PlanetCalendar> ketuMeanPCList,
            List<PlanetCalendar> rahuTrueZCList,
            List<PlanetCalendar> rahuTrueZRCList,
            List<PlanetCalendar> rahuTrueNCList,
            List<PlanetCalendar> rahuTruePCList,
            List<PlanetCalendar> ketuTrueZCList,
            List<PlanetCalendar> ketuTrueZRCList,
            List<PlanetCalendar> ketuTrueNCList,
            List<PlanetCalendar> ketuTruePCList,
            List<MasaCalendar> masaCList,
            List<ShunyaNakshatraCalendar> shuNCList,
            List<ShunyaTithiCalendar> shuTCList,
            List<MrityuBhagaData> moonMBList,
            List<MrityuBhagaData> sunMBList,
            List<MrityuBhagaData> mercuryMBList,
            List<MrityuBhagaData> venusMBList,
            List<MrityuBhagaData> marsMBList,
            List<MrityuBhagaData> jupiterMBList,
            List<MrityuBhagaData> saturnMBList,
            List<MrityuBhagaData> rahuMeanMBList,
            List<MrityuBhagaData> rahuTrueMBList
        )
        {
            InitializeComponent();

            _sYear = year;
            _langCode = langCode;
            _sProfile = sProfile;
            _moonZCList = moonZCList;
            _moonZRCList = moonZRCList;
            _moonNCList = moonNCList;
            _moonPCList = moonPCList;
            _sunZCList = sunZCList;
            _sunZRCList = sunZRCList;
            _sunNCList = sunNCList;
            _sunPCList = sunPCList;
            _mercuryZCList = mercuryZCList;
            _mercuryZRCList = mercuryZRCList;
            _mercuryNCList = mercuryNCList;
            _mercuryPCList = mercuryPCList;
            _venusZCList = venusZCList;
            _venusZRCList = venusZRCList;
            _venusNCList = venusNCList;
            _venusPCList = venusPCList;
            _marsZCList = marsZCList;
            _marsZRCList = marsZRCList;
            _marsNCList = marsNCList;
            _marsPCList = marsPCList;
            _jupiterZCList = jupiterZCList;
            _jupiterZRCList = jupiterZRCList;
            _jupiterNCList = jupiterNCList;
            _jupiterPCList = jupiterPCList;
            _saturnZCList = saturnZCList;
            _saturnZRCList = saturnZRCList;
            _saturnNCList = saturnNCList;
            _saturnPCList = saturnPCList;
            _rahuMeanZCList = rahuMeanZCList;
            _rahuMeanZRCList = rahuMeanZRCList;
            _rahuMeanNCList = rahuMeanNCList;
            _rahuMeanPCList = rahuMeanPCList;
            _ketuMeanZCList = ketuMeanZCList;
            _ketuMeanZRCList = ketuMeanZRCList;
            _ketuMeanNCList = ketuMeanNCList;
            _ketuMeanPCList = ketuMeanPCList;
            _rahuTrueZCList = rahuTrueZCList;
            _rahuTrueZRCList = rahuTrueZRCList;
            _rahuTrueNCList = rahuTrueNCList;
            _rahuTruePCList = rahuTruePCList;
            _ketuTrueZCList = ketuTrueZCList;
            _ketuTrueZRCList = ketuTrueZRCList;
            _ketuTrueNCList = ketuTrueNCList;
            _ketuTruePCList = ketuTruePCList;
            _mCList = masaCList;
            _sNCList = shuNCList;
            _sTCList = shuTCList;
            _moonMBList = moonMBList;
            _sunMBList = sunMBList;
            _mercuryMBList = mercuryMBList;
            _venusMBList = venusMBList;
            _marsMBList = marsMBList;
            _jupiterMBList = jupiterMBList;
            _saturnMBList = saturnMBList;
            _rahuMeanMBList = rahuMeanMBList;
            _rahuTrueMBList = rahuTrueMBList;

            monthWidthArray = new float[12];
        }

        public List<Day> PrepareYearDays(int year, Profile sPerson)
        {
            List<Day> daysList = new List<Day>();
            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31, 23, 59, 59);

            Day tempDay;
            for (DateTime currentDay = startDate; currentDay <= endDate;)
            {

                tempDay = new Day(
                                    sPerson,
                                    currentDay,
                                    _moonZCList,
                                    _moonZRCList,
                                    _moonNCList,
                                    _moonPCList,
                                    _sunZCList,
                                    _sunZRCList,
                                    _sunNCList,
                                    _sunPCList,
                                    _mercuryZCList,
                                    _mercuryZRCList,
                                    _mercuryNCList,
                                    _mercuryPCList,
                                    _venusZCList,
                                    _venusZRCList,
                                    _venusNCList,
                                    _venusPCList,
                                    _marsZCList,
                                    _marsZRCList,
                                    _marsNCList,
                                    _marsPCList,
                                    _jupiterZCList,
                                    _jupiterZRCList,
                                    _jupiterNCList,
                                    _jupiterPCList,
                                    _saturnZCList,
                                    _saturnZRCList,
                                    _saturnNCList,
                                    _saturnPCList,
                                    _rahuMeanZCList,
                                    _rahuMeanZRCList,
                                    _rahuMeanNCList, 
                                    _rahuMeanPCList,
                                    _ketuMeanZCList,
                                    _ketuMeanZRCList,
                                    _ketuMeanNCList, 
                                    _ketuMeanPCList, 
                                    _rahuTrueZCList, 
                                    _rahuTrueZRCList,
                                    _rahuTrueNCList, 
                                    _rahuTruePCList, 
                                    _ketuTrueZCList, 
                                    _ketuTrueZRCList,
                                    _ketuTrueNCList, 
                                    _ketuTruePCList, 
                                    _mCList,
                                    _sNCList,
                                    _sTCList,
                                    _moonMBList,
                                    _sunMBList,
                                    _mercuryMBList,
                                    _venusMBList,
                                    _marsMBList,
                                    _jupiterMBList,
                                    _saturnMBList,
                                    _rahuMeanMBList,
                                    _rahuTrueMBList
                                    );
                daysList.Add(tempDay);
                currentDay = currentDay.AddDays(+1);
            }
            return daysList.Where(i => i.Date.Year == year).ToList();
        }

        public void YearTranzitDrawing(List<Day> dayList)
        {
            float daysOfYear = DateTimeUtils.GetDaysInYear(dayList[0].Date);

            Font textFontPoint = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANZITTEXT)), 8, Utility.GetFontStyleBySettings(EFontList.TRANZITTEXT), GraphicsUnit.Point);
            Size textSizelabel = TextRenderer.MeasureText(Utility.GetLocalizedText("Masa/Shunya", _langCode), textFontPoint);

            labelsWidth = textSizelabel.Width;
            dayWidth = (pictureBoxYearTranzits.Width - labelsWidth - 1) / daysOfYear;
            lineHeight = (pictureBoxYearTranzits.Height - 42 - 1) / 38;
            monthLabelHeight = lineHeight;

            Bitmap canvas = new Bitmap(pictureBoxYearTranzits.Width, pictureBoxYearTranzits.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, canvas.Width, canvas.Height));

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);

            Font textFont;
            Font textFontPixel = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANZITTEXT)), lineHeight - 4, Utility.GetFontStyleBySettings(EFontList.TRANZITTEXT), GraphicsUnit.Pixel);
            if ((lineHeight - 2) >= (textSizelabel.Height + textSizelabel.Height / 2))
            {
                textFont = textFontPixel;
            }
            {
                textFont = textFontPoint;
            }

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            float posX = labelsWidth; 
            float posY = 2, nextPlanetY = 0;
            float posYMasa = posY + monthLabelHeight + 4;
            float posYTranzits = posYMasa + lineHeight + 4;

            EAppSetting tranzitSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            string culture = CacheLoad._languageList.Where(i => i.LanguageCode.Equals(_langCode.ToString())).FirstOrDefault().CultureCode;

            int year = dayList[0].Date.Year;
            for (int i = 0; i < 12; i++)
            {
                float monthWidth = dayWidth * DateTime.DaysInMonth(year, i + 1);
                monthWidthArray[i] = monthWidth;
                string monthName = CultureInfo.GetCultureInfo(culture).DateTimeFormat.MonthNames[i];
                Size textSize = TextRenderer.MeasureText(monthName, textFont);
                float heightPadding = (monthLabelHeight - textSize.Height) / 2;
                g.DrawString(monthName, textFont, textBrush, (posX + (monthWidth / 2 - textSize.Width / 2)), posY + heightPadding);
                //RectangleF rec = new RectangleF(posX, posY, monthWidth, monthLabelHeight);
                g.DrawRectangle(pen, posX, posY, monthWidth, monthLabelHeight);
                posX += monthWidth;
            }

            //Draw planets tranzits
            posX = labelsWidth;
            foreach (Day d in dayList)
            {
                //Drawing masa
                posY = posYMasa;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, lineHeight, "Masa/Shunya");
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MasaDayList);

                //Fill masa line by shunya nakshatra
                posY = posYMasa;
                DrawShunyaColoredLine(g, pen, posX, posY, dayWidth, lineHeight, d.ShunyaNakshatraDayList, d.Date);

                //Fill masa line by shunya tithi
                posY = posYMasa;
                DrawShunyaColoredLine(g, pen, posX, posY, dayWidth, lineHeight, d.ShunyaTithiDayList, d.Date);

                //Drawing Moon
                nextPlanetY = posYTranzits;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Moon");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MoonZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MoonZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.MoonZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.MoonZodiakRetroLagnaDayList);
                        break;
                }
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.MoonNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.MoonPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.MoonTaraBalaDayList);

                //Drawing Sun
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Sun");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.SunZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.SunZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.SunZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.SunZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, dayWidth, lineHeight, d.SunMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.SunNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.SunPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.SunTaraBalaDayList);

                //Drawing Venus
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Venus");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.VenusZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.VenusZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.VenusZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.VenusZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, dayWidth, lineHeight, d.VenusMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.VenusNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.VenusPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.VenusTaraBalaDayList);

                //Drawing Jupiter
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Jupiter");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.JupiterZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.JupiterZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.JupiterZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.JupiterZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, dayWidth, lineHeight, d.JupiterMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.JupiterNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.JupiterPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.JupiterTaraBalaDayList);

                //Drawing Mercury
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Mercury");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MercuryZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MercuryZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.MercuryZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.MercuryZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, dayWidth, lineHeight, d.MercuryMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.MercuryNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.MercuryPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.MercuryTaraBalaDayList);

                //Drawing Mars
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Mars");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MarsZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MarsZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.MarsZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.MarsZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, dayWidth, lineHeight, d.MarsMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.MarsNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.MarsPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.MarsTaraBalaDayList);

                //Drawing Saturn
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Saturn");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.SaturnZodiakRetroDayList);
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.SaturnZodiakRetroLagnaDayList);
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.SaturnZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.SaturnZodiakRetroLagnaDayList);
                        break;
                }
                DrawingMrityaBhaga(g, posX, posY, dayWidth, lineHeight, d.SaturnMrityuBhagaDayList, d.Date);

                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.SaturnNakshatraDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.SaturnPadaDayList);
                DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.SaturnTaraBalaDayList);

                //Drawing Rahu
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Rahu");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.RahuMeanZodiakRetroDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.RahuTrueZodiakRetroDayList);
                        }
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.RahuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.RahuTrueZodiakRetroLagnaDayList);
                        }
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.RahuMeanZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.RahuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.RahuTrueZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.RahuTrueZodiakRetroLagnaDayList);
                        }
                        break;
                }
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.RahuMeanNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.RahuMeanPadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.RahuMeanTaraBalaDayList);
                }
                else
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.RahuTrueNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.RahuTruePadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.RahuTrueTaraBalaDayList);
                }

                //Drawing Ketu
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                DrawLineName(g, pen, textBrush, posY, labelsWidth, (4 * lineHeight), "Ketu");
                switch (tranzitSetting)
                {
                    case EAppSetting.TRANZITMOON:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.KetuMeanZodiakRetroDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.KetuTrueZodiakRetroDayList);
                        }
                        break;

                    case EAppSetting.TRANZITLAGNA:
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.KetuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.KetuTrueZodiakRetroLagnaDayList);
                        }
                        break;

                    case EAppSetting.TRANZITMOONANDLAGNA:
                        float height = lineHeight / 2;
                        float posYHalf = posY + height;
                        if (nodeSettings == EAppSetting.NODEMEAN)
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.KetuMeanZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.KetuMeanZodiakRetroLagnaDayList);
                        }
                        else
                        {
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.KetuTrueZodiakRetroDayList);
                            DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.KetuTrueZodiakRetroLagnaDayList);
                        }
                        break;
                }
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.KetuMeanNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.KetuMeanPadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.KetuMeanTaraBalaDayList);
                }
                else
                {
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.KetuTrueNakshatraDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.KetuTruePadaDayList);
                    DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.KetuTrueTaraBalaDayList);
                }

                posX = posX + dayWidth;
            }

            // Setting current start zodiak 
            posX = labelsWidth;

            // Setting masa current start 
            posX = labelsWidth;
            posY = posYMasa;
            SetMasaStartName(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().MasaDayList, _langCode);

            // Moon
            nextPlanetY = posYTranzits;
            posY = nextPlanetY;
            //SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().MoonZodiakRetroDayList);
            //SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().MoonNakshatraDayList, true);
            //SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().MoonPadaDayList);
            //SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().MoonTaraBalaDayList, true);

            // Sun
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().SunZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().SunNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().SunPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().SunTaraBalaDayList, false);

            // Venus
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().VenusZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().VenusNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().VenusPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().VenusTaraBalaDayList, false);

            // Jupiter
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().JupiterZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().JupiterNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().JupiterPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().JupiterTaraBalaDayList, false);

            // Mercury
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().MercuryZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().MercuryNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().MercuryPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().MercuryTaraBalaDayList, false);

            // Mars
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().MarsZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().MarsNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().MarsPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().MarsTaraBalaDayList, false);

            // Saturn
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().SaturnZodiakRetroDayList);
            SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().SaturnNakshatraDayList, false);
            SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().SaturnPadaDayList);
            SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().SaturnTaraBalaDayList, false);

            // Rahu
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            if (nodeSettings == EAppSetting.NODEMEAN)
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().RahuMeanZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().RahuMeanNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().RahuMeanPadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().RahuMeanTaraBalaDayList, false);
            }
            else
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().RahuTrueZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().RahuTrueNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().RahuTruePadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().RahuTrueTaraBalaDayList, false);
            }

            // Ketu
            nextPlanetY += 4 * lineHeight + 4;
            posY = nextPlanetY;
            if (nodeSettings == EAppSetting.NODEMEAN)
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().KetuMeanZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().KetuMeanNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().KetuMeanPadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().KetuMeanTaraBalaDayList, false);
            }
            else
            {
                SetLineStartZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, dayList.First().KetuTrueZodiakRetroDayList);
                SetLineStartNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, dayList.First().KetuTrueNakshatraDayList, false);
                SetLineStartPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, dayList.First().KetuTruePadaDayList);
                SetLineStartTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, dayList.First().KetuTrueTaraBalaDayList, false);
            }

            // Setting masa changes
            posX = labelsWidth;
            posY = posYMasa;
            foreach (Day d in dayList)
            {
                SetMasaName(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MasaDayList, d.Date, _langCode);
                posX = posX + dayWidth;
            }


            //Drawing zodiac, nakshatra and pada changes 
            posX = labelsWidth;
            foreach (Day d in dayList)
            {
                // Moon
                nextPlanetY = posYTranzits;
                posY = nextPlanetY;
                //SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MoonZodiakRetroDayList, d.Date);
                //SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.MoonNakshatraDayList, d.Date, true);
                //SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.MoonPadaDayList, d.Date);
                //SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.MoonTaraBalaDayList, d.Date, true);

                // Sun
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.SunZodiakRetroDayList, d.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.SunNakshatraDayList, d.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.SunPadaDayList, d.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.SunTaraBalaDayList, d.Date, false);

                // Venus
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.VenusZodiakRetroDayList, d.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.VenusNakshatraDayList, d.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.VenusPadaDayList, d.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.VenusTaraBalaDayList, d.Date, false);

                // Jupiter
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.JupiterZodiakRetroDayList, d.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.JupiterNakshatraDayList, d.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.JupiterPadaDayList, d.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.JupiterTaraBalaDayList, d.Date, false);

                // Mercury
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MercuryZodiakRetroDayList, d.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.MercuryNakshatraDayList, d.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.MercuryPadaDayList, d.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.MercuryTaraBalaDayList, d.Date, false);

                // Mars
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.MarsZodiakRetroDayList, d.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.MarsNakshatraDayList, d.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.MarsPadaDayList, d.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.MarsTaraBalaDayList, d.Date, false);

                // Saturn
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.SaturnZodiakRetroDayList, d.Date);
                SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.SaturnNakshatraDayList, d.Date, false);
                SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.SaturnPadaDayList, d.Date);
                SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.SaturnTaraBalaDayList, d.Date, false);

                // Rahu
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.RahuMeanZodiakRetroDayList, d.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.RahuMeanNakshatraDayList, d.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.RahuMeanPadaDayList, d.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.RahuMeanTaraBalaDayList, d.Date, false);
                }
                else
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.RahuTrueZodiakRetroDayList, d.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.RahuTrueNakshatraDayList, d.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.RahuTruePadaDayList, d.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.RahuTrueTaraBalaDayList, d.Date, false);
                }

                // Ketu
                nextPlanetY += 4 * lineHeight + 4;
                posY = nextPlanetY;
                if (nodeSettings == EAppSetting.NODEMEAN)
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.KetuMeanZodiakRetroDayList, d.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.KetuMeanNakshatraDayList, d.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.KetuMeanPadaDayList, d.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.KetuMeanTaraBalaDayList, d.Date, false);
                }
                else
                {
                    SetPlanetZodiak(g, pen, textFont, textBrush, posX, posY, dayWidth, lineHeight, d.KetuTrueZodiakRetroDayList, d.Date);
                    SetPlanetNakshatra(g, pen, textFont, textBrush, posX, posY + lineHeight, dayWidth, lineHeight, d.KetuTrueNakshatraDayList, d.Date, false);
                    SetPlanetPada(g, pen, textFont, textBrush, posX, posY + 2 * lineHeight, dayWidth, lineHeight, d.KetuTruePadaDayList, d.Date);
                    SetPlanetTaraBala(g, pen, textFont, textBrush, posX, posY + 3 * lineHeight, dayWidth, lineHeight, d.KetuTrueTaraBalaDayList, d.Date, false);
                }

                posX = posX + dayWidth;
            }

            // Drawing Masa rectangles
            posX = labelsWidth;
            posY = posYMasa;
            for (int month = 0; month < monthWidthArray.Length; month++)
            {
                DrawLineRectangle(g, pen, posX, posY, monthWidthArray[month], lineHeight);
                posX += monthWidthArray[month];
            }

            //Drawing Tranzits rectangles
            posX = labelsWidth;
            nextPlanetY = posYTranzits;
            posY = nextPlanetY;
            for (int month = 0; month < monthWidthArray.Length; month++)
            {
                for (int i = 0; i < 9; i++)
                {
                    DrawPlanetBlockRectangles(g, pen, posX, posY, monthWidthArray[month], lineHeight);
                    nextPlanetY += 4 * lineHeight + 4;
                    posY = nextPlanetY;
                }
                posX += monthWidthArray[month];
                nextPlanetY = posYTranzits;
                posY = nextPlanetY;
            }
            pictureBoxYearTranzits.Image = canvas;
            _currentImage = pictureBoxYearTranzits.Image;
            //((MainForm)this.MFormAccess).CurrentYearTranzitImage = _currentImage;
        }

        private void SetLineStartZodiak(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                float endPosX = 0;
                string text = c.First().GetNumberForYear();
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
                    //if (textSize.Width <= endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
                else
                {
                    endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
                    //if (textSize.Width <= endPosX)
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
                string text = c.First().GetTranzitNakshatraForYear().ToString();
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
                    //if (textSize.Width < endPosX && !isMoon)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                    /*else
                    {
                        var newText = text.Split('.');
                        textSize = TextRenderer.MeasureText(newText[0], font);
                        if (textSize.Width <= endPosX)
                        {
                            g.DrawString(newText[0], font, textBrush, posX + 1, posY + heightPadding);
                        }
                    }*/
                }
            }
        }

        private void SetLineStartPada(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                List<PlanetCalendar> pcList = new List<PlanetCalendar>();
                c.ForEach(i => pcList.Add((PlanetCalendar)i));

                string text = string.Empty; 
                if (pcList.First().PlanetCode == EPlanet.JUPITER || pcList.First().PlanetCode == EPlanet.MARS)
                {
                    text = pcList.First().GetTranzitPadaWithoutBadNavamshaAndDreakkana();
                }
                else
                {
                    text = pcList.First().GetTranzitPada(_sProfile, _langCode);
                    if (pcList.First().PlanetCode == EPlanet.SUN || pcList.First().PlanetCode == EPlanet.VENUS || pcList.First().PlanetCode == EPlanet.MERCURY)
                    {
                        text = text.Substring(0, 1);
                    }
                }
                Size textSize = TextRenderer.MeasureText(text, font);
                float heightPadding = (height - textSize.Height) / 2;
                if (pcList.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width * pcList.Last().DateStart.Day, pcList.Last().DateStart);
                    //if (textSize.Width < endPosX)
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
                string text = c.First().GetTranzitTaraBala(_langCode);
                var newText = text.Split('.');
                Size textSize = TextRenderer.MeasureText(newText[0], font);
                float heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(newText[0], font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    float endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
                    //if (textSize.Width < endPosX)
                    {
                        //if (textSize.Width <= endPosX)
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
                        string text = pc.GetNumberForYear();
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
                        string text = pc.GetTranzitNakshatraForYear().ToString();
                        Size textSize = TextRenderer.MeasureText(text, font);
                        float heightPadding = (height - textSize.Height) / 2;
                        if (pc.NakshatraCode != previousNakshatra)
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
                int index = 0;
                foreach (PlanetCalendar pc in pcList)
                {
                    if (pc.DateStart > date)
                    {
                        string text = string.Empty;
                        int currentPada = CacheLoad._padaList.Where(i => i.Id == pc.PadaId).FirstOrDefault().PadaNumber;
                        if (currentPada != previousPada)
                        {
                            float startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                            if (pc.PlanetCode == EPlanet.JUPITER || pc.PlanetCode == EPlanet.MARS)
                            {
                                text = pc.GetTranzitPadaWithoutBadNavamshaAndDreakkana();
                            }
                            else
                            {
                                text = pc.GetTranzitPada(_sProfile, _langCode);
                                if (pc.PlanetCode == EPlanet.SUN || pc.PlanetCode == EPlanet.VENUS || pc.PlanetCode == EPlanet.MERCURY)
                                {
                                    text = text.Substring(0, 1);
                                }
                            }
                            Size textSize = TextRenderer.MeasureText(text, font);
                            float heightPadding = (height - textSize.Height) / 2;
                            g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                            g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            previousPada = currentPada;
                        }
                    }
                    index++;
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
                        string text = pc.GetTranzitTaraBala(_langCode);
                        Size textSize = TextRenderer.MeasureText(text, font);
                        float heightPadding = (height - textSize.Height) / 2;
                        if (pc.TaraBalaId != previousTaraBala)
                        {
                            var newText = text.Split('.');
                            textSize = TextRenderer.MeasureText(newText[0], font);
                            g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                            g.DrawString(newText[0], font, textBrush, posX + startPosX + 1, posY + heightPadding);
                        }
                    }
                    previousTaraBala = pc.TaraBalaId;
                }
            }
        }

        private void DrawPlanetBlockRectangles(Graphics g, Pen pen, float posX, float posY, float width, float height)
        {
            for (int i = 0; i < 4; i++)
            {
                DrawLineRectangle(g, pen, posX, posY + i * height, width, height);
            }
        }

        private void DrawLineRectangle(Graphics g, Pen pen, float posX, float posY, float width, float height)
        {
            g.DrawRectangle(pen, posX, posY, width, height);
        }

        private void DrawLineName(Graphics g, Pen pen, SolidBrush textBrush, float posY, float labelsWidth, float height, string text)
        {
            string localText = Utility.GetLocalizedText(text, _langCode);
            Font textFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular, GraphicsUnit.Point);
            Size textSize = TextRenderer.MeasureText(localText, textFont);
            float posX = labelsWidth - (textSize.Width + 2);
            float heightPadding = (height - textSize.Height) / 2;
            g.DrawString(localText, textFont, textBrush, posX, posY + heightPadding);
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
            SolidBrush brush = new SolidBrush(Color.White);
            brush = new SolidBrush(Utility.GetColorByColorCode(cObj.ColorCode));
            g.FillRectangle(brush, posX, posY, width, height);
        }

        private void pictureBoxYearTranzits_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int daysOfYear = DateTimeUtils.GetDaysInYear(dayList[0].Date);
                float posX = labelsWidth, posY = 1;
                float height = monthLabelHeight + lineHeight + 4 +((lineHeight * 4) + 4) * 9 + 2;

                float monthPos = e.X - labelsWidth;
                float curX = 0; 
                int month = 0;
                for (int i = 0; i < 12; i++)
                {
                    curX += monthWidthArray[i];
                    if (curX > monthPos)
                    {
                        month = i + 1;
                        break;
                    }
                    posX += monthWidthArray[i];
                }

                // Draw blue rectangle for selected day through all lines
                pictureBoxYearTranzits.Image = _currentImage;
                Bitmap canvas = new Bitmap(pictureBoxYearTranzits.Image);
                Graphics g = Graphics.FromImage(canvas);
                Pen pen = new Pen(Color.FromArgb(CacheLoad._colorList.Where(i => i.Code.Equals(EColor.SELECTRECTANGLE.ToString())).FirstOrDefault().ARGBValue), 1);
                g.DrawRectangle(pen, posX, posY, monthWidthArray[month - 1], height);
                pictureBoxYearTranzits.Image = canvas;
            }
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

        private void SetMasaStartName(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> c, ELanguage lCode)
        {
            if (c.Count > 0)
            {
                string text = c.First().GetMasaName(lCode); 
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

        private void SetMasaName(Graphics g, Pen pen, Font font, SolidBrush textBrush, float posX, float posY, float width, float height, List<Calendar> cList, DateTime date, ELanguage lCode)
        {
            string curMasa = cList.First().GetMasaName(lCode);
            foreach (Calendar c in cList)
            {
                if (c.DateStart > date)
                {
                    float startPosX = Utility.ConvertHoursToPixels(width, c.DateStart);
                    string text = c.GetMasaName(lCode); 
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

        private void pictureBoxYearTranzits_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int daysOfYear = DateTimeUtils.GetDaysInYear(dayList[0].Date);
            int dayWidth = pictureBoxYearTranzits.Width / daysOfYear;
            int labelsWidth = pictureBoxYearTranzits.Width - dayWidth * daysOfYear - 2;

            int[] monthWidthArray = new int[12];
            for (int i = 0; i < 12; i++)
            {
                int monthWidth = dayWidth * DateTime.DaysInMonth(_sYear, i + 1);
                monthWidthArray[i] = monthWidth;
            }

            int monthPos = e.X - labelsWidth;
            int curX = 0, month = 0;
            for (int i = 0; i < 12; i++)
            {
                curX += monthWidthArray[i];
                if (curX > monthPos)
                {
                    month = i + 1;
                    break;
                }
            }
            DateTime sDate = new DateTime(_sYear, month, 1);
            ((MainForm)this.MFormAccess).MonthDate = sDate;
            ((MainForm)this.MFormAccess).SetTranzitFocus();
        }
        
    }
}
