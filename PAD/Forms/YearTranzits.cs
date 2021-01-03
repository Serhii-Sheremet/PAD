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

        int lineHeight;
        int monthLabelHeight;
        private Image _currentImage;

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
            List<EclipseCalendar> eclipseCList
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
        }

        public List<Day> PrepareYearDays(int year, Profile sPerson)
        {
            List<Day> daysList = new List<Day>();
            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31, 23, 59, 59);

            double latitude, longitude;
            string timeZone = string.Empty;
            if (Utility.GetGeoCoordinateByLocationId(sPerson.PlaceOfLivingId, out latitude, out longitude))
            {
                timeZone = Utility.GetTimeZoneIdByGeoCoordinates(latitude, longitude);
                TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                TimeZoneInfo.AdjustmentRule[] adjustmentRules = currentTimeZone.GetAdjustmentRules();

                List<PlanetCalendar> moonZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_moonZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                moonZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_moonZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                moonZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_moonNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                moonNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> moonPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_moonPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                moonPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                moonPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_sunZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                sunZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_sunZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                sunZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_sunNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                sunNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> sunPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_sunPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                sunPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                sunPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                mercuryZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                mercuryZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                mercuryNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> mercuryPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_mercuryPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                mercuryPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                mercuryPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_venusZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                venusZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_venusZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                venusZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_venusNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                venusNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> venusPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_venusPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                venusPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                venusPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_marsZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                marsZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_marsZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                marsZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_marsNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                marsNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> marsPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_marsPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                marsPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                marsPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                jupiterZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                jupiterZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                jupiterNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> jupiterPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_jupiterPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                jupiterPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                jupiterPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                saturnZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                saturnZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                saturnNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> saturnPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_saturnPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                saturnPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                saturnPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuMeanPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuMeanPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuMeanPadaPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuMeanPCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuMeanPadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTrueZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTrueZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTrueNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTrueNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> rahuTruePadaPeriodCalendar = Utility.ClonePlanetCalendarList(_rahuTruePCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                rahuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                rahuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTrueZCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueZodiakRetroPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTrueZRCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueZodiakRetroPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTrueNakshatraPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTrueNCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTrueNakshatraPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                List<PlanetCalendar> ketuTruePadaPeriodCalendar = Utility.ClonePlanetCalendarList(_ketuTruePCList.Where(i => i.DateEnd > startDate.AddDays(-1) && i.DateStart < endDate.AddDays(+1)).ToList());
                ketuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); i.DateEnd = i.DateEnd.ShiftByUtcOffset(currentTimeZone.BaseUtcOffset); });
                ketuTruePadaPeriodCalendar.ForEach(i => { i.DateStart = i.DateStart.ShiftByDaylightDelta(adjustmentRules); i.DateEnd = i.DateEnd.ShiftByDaylightDelta(adjustmentRules); });

                Day tempDay;
                // Preparing original List<DayCalendars> list
                for (DateTime currentDay = startDate; currentDay <= endDate;)
                {

                    tempDay = new Day(
                                        sPerson,
                                        currentDay,
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
                                        ketuTruePadaPeriodCalendar);
                    daysList.Add(tempDay);
                    currentDay = currentDay.AddDays(+1);
                }
            }
            return daysList.Where(i => i.Date.Year == year).ToList();
        }

        public void YearTranzitDrawing(List<Day> dayList)
        {
            int daysOfYear = DateTimeUtils.GetDaysInYear(dayList[0].Date);
            int dayWidth = pictureBoxYearTranzits.Width / daysOfYear;
            lineHeight = pictureBoxYearTranzits.Height / 50;
            monthLabelHeight = lineHeight + 4;
            int labelsWidth = pictureBoxYearTranzits.Width - dayWidth * daysOfYear - 2;

            Bitmap canvas = new Bitmap(pictureBoxYearTranzits.Width, pictureBoxYearTranzits.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, canvas.Width, canvas.Height));

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            Font textFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANZITTEXT)), lineHeight - 6, Utility.GetFontStyleBySettings(EFontList.TRANZITTEXT), GraphicsUnit.Pixel);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            int posX = labelsWidth, posY = 4, nextPlanetY = 0;
            int posYTranzits = posY + monthLabelHeight + 4;

            EAppSetting tranzitSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            string culture = CacheLoad._languageList.Where(i => i.LanguageCode.Equals(_langCode.ToString())).FirstOrDefault().CultureCode;

            int year = dayList[0].Date.Year;
            int[] monthWidthArray = new int[12];
            for (int i = 0; i < 12; i++)
            {
                int monthWidth = dayWidth * DateTime.DaysInMonth(year, i + 1);
                monthWidthArray[i] = monthWidth;
                string monthName = CultureInfo.GetCultureInfo(culture).DateTimeFormat.MonthNames[i];
                Size textSize = TextRenderer.MeasureText(monthName, textFont);
                int heightPadding = (monthLabelHeight - textSize.Height) / 2;
                g.DrawString(monthName, textFont, textBrush, (posX + (monthWidth / 2 - textSize.Width / 2)), posY + heightPadding);
                Rectangle rec = new Rectangle(posX, posY, monthWidth, monthLabelHeight);
                g.DrawRectangle(pen, rec);
                posX += monthWidth;
            }

            //Draw planets tranzits
            posX = labelsWidth;
            foreach (Day d in dayList)
            {
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.SunZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.SunZodiakRetroLagnaDayList);
                        break;
                }
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.VenusZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.VenusZodiakRetroLagnaDayList);
                        break;
                }
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.JupiterZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.JupiterZodiakRetroLagnaDayList);
                        break;
                }
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.MercuryZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.MercuryZodiakRetroLagnaDayList);
                        break;
                }
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.MarsZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.MarsZodiakRetroLagnaDayList);
                        break;
                }
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posY, dayWidth, height, d.SaturnZodiakRetroDayList);
                        DrawTranzitColoredLine(g, pen, textFont, textBrush, posX, posYHalf, dayWidth, height, d.SaturnZodiakRetroLagnaDayList);
                        break;
                }
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
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
                        int height = lineHeight / 2;
                        int posYHalf = posY + height;
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
        }

        private void SetLineStartZodiak(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> c)
        {
            if (c.Count > 0)
            {
                int endPosX = 0;
                string text = c.First().GetNumberForYear();
                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                
                if (c.Count == 1)
                {
                    endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
                    if (textSize.Width <= endPosX)
                    {
                        g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                    }
                }
                else
                {
                    endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
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
                string text = c.First().GetTranzitNakshatraForYear().ToString();
                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
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
                List<PlanetCalendar> pcList = new List<PlanetCalendar>();
                c.ForEach(i => pcList.Add((PlanetCalendar)i));

                string text = pcList.First().GetTranzitPada();
                if (pcList.First().PlanetCode == EPlanet.SUN || pcList.First().PlanetCode == EPlanet.VENUS || pcList.First().PlanetCode == EPlanet.MERCURY)
                {
                    text = text.Substring(0, 1);
                }

                Size textSize = TextRenderer.MeasureText(text, font);
                int heightPadding = (height - textSize.Height) / 2;
                if (pcList.Count == 1)
                {
                    g.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width * pcList.Last().DateStart.Day, pcList.Last().DateStart);
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
                string text = c.First().GetTranzitTaraBala(_langCode);
                var newText = text.Split('.');
                Size textSize = TextRenderer.MeasureText(newText[0], font);
                int heightPadding = (height - textSize.Height) / 2;
                if (c.Count == 1)
                {
                    g.DrawString(newText[0], font, textBrush, posX + 1, posY + heightPadding);
                }
                else
                {
                    int endPosX = Utility.ConvertHoursToPixels(width * c.Last().DateStart.Day, c.Last().DateStart);
                    if (textSize.Width < endPosX)
                    {
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
                        string text = pc.GetNumberForYear();
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
                        string text = pc.GetTranzitNakshatraForYear().ToString();
                        Size textSize = TextRenderer.MeasureText(text, font);
                        int heightPadding = (height - textSize.Height) / 2;
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

        private void SetPlanetPada(Graphics g, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> cList, DateTime date)
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
                        int currentPada = CacheLoad._padaList.Where(i => i.Id == pc.PadaId).FirstOrDefault().PadaNumber;
                        if (currentPada != previousPada)
                        {
                            int startPosX = Utility.ConvertHoursToPixels(width, pc.DateStart);
                            
                            string text = pc.GetTranzitPada();
                            Size textSize = TextRenderer.MeasureText(text, font);
                            if (pc.PlanetCode == EPlanet.SUN || pc.PlanetCode == EPlanet.VENUS || pc.PlanetCode == EPlanet.MERCURY)
                            {
                                text = text.Substring(0, 1);
                            }
                            int heightPadding = (height - textSize.Height) / 2;
                            g.DrawLine(pen, posX + startPosX, posY, posX + startPosX, posY + height);
                            g.DrawString(text, font, textBrush, posX + startPosX + 1, posY + heightPadding);
                            previousPada = currentPada;
                        }
                    }
                    index++;
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
                        string text = pc.GetTranzitTaraBala(_langCode);
                        Size textSize = TextRenderer.MeasureText(text, font);
                        int heightPadding = (height - textSize.Height) / 2;
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

        private void DrawPlanetBlockRectangles(Graphics g, Pen pen, int posX, int posY, int width, int height)
        {
            for (int i = 0; i < 4; i++)
            {
                DrawLineRectangle(g, pen, posX, posY + i * height, width, height);
            }
        }

        private void DrawLineRectangle(Graphics g, Pen pen, int posX, int posY, int width, int height)
        {
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.DrawRectangle(pen, rect);
        }

        private void DrawLineName(Graphics g, Pen pen, SolidBrush textBrush, int posY, int labelsWidth, int height, string text)
        {
            string localText = Utility.GetLocalizedText(text, _langCode);
            Font textFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular, GraphicsUnit.Point);
            Size textSize = TextRenderer.MeasureText(localText, textFont);
            int posX = labelsWidth - (textSize.Width + 2);
            int heightPadding = (height - textSize.Height) / 2;
            g.DrawString(localText, textFont, textBrush, posX, posY + heightPadding);
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

        private void pictureBoxYearTranzits_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int daysOfYear = DateTimeUtils.GetDaysInYear(dayList[0].Date);
                int dayWidth = pictureBoxYearTranzits.Width / daysOfYear;
                int labelsWidth = pictureBoxYearTranzits.Width - dayWidth * daysOfYear - 2;
                int posX = labelsWidth, posY = 3;
                int height = monthLabelHeight +((lineHeight * 4) + 4) * 9 + 2;

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
                    posX += monthWidthArray[i];
                }

                // Draw blue rectangle for selected day through all lines
                pictureBoxYearTranzits.Image = _currentImage;
                Bitmap canvas = new Bitmap(pictureBoxYearTranzits.Image);
                Graphics g = Graphics.FromImage(canvas);
                Rectangle selectRectangle = new Rectangle(posX, posY, monthWidthArray[month - 1], height);
                Pen pen = new Pen(Color.FromArgb(CacheLoad._colorList.Where(i => i.Code.Equals(EColor.SELECTRECTANGLE.ToString())).FirstOrDefault().ARGBValue), 1);
                g.DrawRectangle(pen, selectRectangle);
                pictureBoxYearTranzits.Image = canvas;
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
