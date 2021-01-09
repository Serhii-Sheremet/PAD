using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using PopupControl;

namespace PAD
{
    public partial class TabDay : Form
    {
        MainForm mform;
        public MainForm MFormAccess
        {
            get { return mform; }
            set { mform = value; }
        }

        private ELanguage _langCode;
        private Day _currDay;
        private Profile _curProfile;
        private List<PersonsEventsList> _pevList;
        private List<DVLineNames> _dvlnList;
        private List<DVLineNameDescription> _dvlnDescList;
        private bool isAppCreated;

        Popup toolTip; 
        DayViewToolTip dayViewToolTip;

        private Dictionary<string, string> _groupsDict;
        
        private string groupTags(string tag)
        {
            return _groupsDict[tag];
        }
        
        private List<Appointment> m_Appointments;

        public TabDay()
        {
            InitializeComponent();
        }
        
        public TabDay(Day day, Profile sProfile, List<DVLineNames> dvlNamesList, List<DVLineNameDescription> dvlNamesDescList, List<PersonsEventsList> evList, ELanguage langCode)
        {
            InitializeComponent();

            m_Appointments = new List<Appointment>();
            _langCode = langCode;
            _currDay = day;
            _curProfile = sProfile;
            _pevList = evList;
            _dvlnList = dvlNamesList;
            _dvlnDescList = dvlNamesDescList;

            List<string> snList = new List<string>();
            _dvlnDescList.ForEach(i => { snList.Add(i.Name); });

            dayView.StartDate = _currDay.Date;
            dayView.SunRiseDate = _currDay.SunRise;
            dayView.SunSetDate = _currDay.SunSet;
            dayView.WorkingHourStart = _currDay.SunRise.Value.Hour;
            dayView.WorkingMinuteStart = _currDay.SunRise.Value.Minute;
            dayView.WorkingHourEnd = _currDay.SunSet.Value.Hour;
            dayView.WorkingMinuteEnd = _currDay.SunSet.Value.Minute;
            dayView.CultureCode = Utility.GetActiveCultureCode(langCode); 
            dayView.ArrayOfNames = snList.ToArray();

            InitializeGroupDictionary();
            isAppCreated = false;

            dayView.NewAppointment += new NewAppointmentEventHandler(dayView_NewAppointment);
            //dayView.SelectionChanged += new EventHandler(dayView_SelectionChanged);
            dayView.ResolveAppointments += new ResolveAppointmentsEventHandler(this.dayView_ResolveAppointments);
        }
        
        private void InitializeGroupDictionary()
        {
            _groupsDict = new Dictionary<string, string>();
            for (int i = 0; i < _dvlnList.Count; i++)
            {
                _groupsDict.Add(_dvlnList[i].Code, _dvlnList[i].Id.ToString());
            }
        }

        private void DrawDayLine(List<Calendar> cList, string group, bool isPlanet)
        {
            #region Check count
            if (cList.Count==0)
            {
                setAppointments(_currDay.Date, _currDay.Date, "", Color.White, true, group, true);
            }
            #endregion

            DateTime dayEndDate = new DateTime(_currDay.Date.Year, _currDay.Date.Month, _currDay.Date.Day, 23, 59, 59);
            DateTime startDate = _currDay.Date;
            DateTime endDate = new DateTime();

            foreach (Calendar c in cList)
            {
                endDate = c.DateEnd;
                if (c.DateStart > _currDay.Date)
                    startDate = c.DateStart;
                if (c.DateEnd > dayEndDate)
                    endDate = dayEndDate;

                if (isPlanet)
                {
                    setAppointments(startDate, endDate, c.GetPlanetDayViewShortName(_langCode), Utility.GetColorByColorCode(c.ColorCode), true, group, true);
                }
                else
                {
                    setAppointments(startDate, endDate, c.GetShortName(_langCode), Utility.GetColorByColorCode(c.ColorCode), true, group, true);
                }
            }
        }

        private bool CheckIfJogaTimeIsToday(List<Calendar> cList, DateTime startDate, DateTime endDate)
        {
            bool isToday = false;
            foreach (Calendar c in cList)
            {
                if (c.DateStart.Between(startDate, endDate) || c.DateEnd.Between(startDate, endDate))
                {
                    isToday = true;
                    break;
                }
            }
            return isToday;
        }

        private void DrawJogaLine(Day day, string group, ELanguage lang)
        {
            DateTime startDate = day.Date;
            DateTime dayEndDate = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, 23, 59, 59);
            DateTime endDate = new DateTime();

            #region Check if Joga exist
            if (    !CheckIfJogaTimeIsToday(day.AdhamJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.AmritaSiddhaJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.DagdhaJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.DwipushkarJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.YamaghataJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.MrityuJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.UnfarobaleJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.SarvarthaSiddhaJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.SiddhaJogaDayList, startDate, dayEndDate) &&
                    !CheckIfJogaTimeIsToday(day.TripushkarJogaDayList, startDate, dayEndDate))
            {
                setAppointments(day.Date, day.Date, "", Color.White, true, group, true);
            }
            #endregion

            else
            {
                List<JogaColoredBlock> jcbList = Utility.GetJogaColoredBlockListForDay(day);
                foreach (JogaColoredBlock jcb in jcbList)
                {
                    endDate = jcb.DateEnd;
                    if (jcb.DateStart > day.Date)
                        startDate = jcb.DateStart;
                    if (jcb.DateEnd > dayEndDate)
                        endDate = dayEndDate;
                    if (jcb.ColorCode != EColor.NOCOLOR)
                    {
                        string text = string.Empty;
                        List<JogaCalendar> jcList = Utility.GetYogasListForTimePeriod(day, startDate, endDate);
                        foreach (JogaCalendar jc in jcList)
                        {
                            text += jc.GetShortName(lang) + " ";
                        }
                        setAppointments(startDate, endDate, text, Utility.GetColorByColorCode(jcb.ColorCode), true, group, true);
                    }
                }
            }
        }

        private void DrawMuhurtaLine(List<Calendar> mList, DateTime date, string group, ELanguage lang)
        {
            #region Check count
            if (mList.Count==0)
            {
                setAppointments(date, date, "", Color.White, true, group, true);
            }
            #endregion

            DateTime startDate = date;
            DateTime dayEndDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            DateTime endDate = new DateTime();

            List<MuhurtaCalendar> clonedMList = Utility.CloneMuhurtaCalendarList(mList);

            List<MuhurtaCalendar> mcList = new List<MuhurtaCalendar>();
            MuhurtaCalendar currentMC = null;
            foreach (MuhurtaCalendar mc in clonedMList)
            {
                if (currentMC != null && (currentMC.DateStart.StrictBetween(mc.DateStart, mc.DateEnd) || currentMC.DateEnd.StrictBetween(mc.DateStart, mc.DateEnd)))
                {
                    DateTime end = mcList.Last().DateEnd;
                    mcList.Last().DateEnd = mc.DateStart;
                    MuhurtaCalendar temp = new MuhurtaCalendar(currentMC.MuhurtaCode, mc.MuhurtaCode, mc.DateStart, end);
                    mcList.Add(temp);
                    mc.DateStart = end;
                }
                mcList.Add(mc);
                currentMC = mc;
            }
            List<MuhurtaCalendar> sortedMuhurtaList = mcList.OrderBy(s => s.DateStart).ToList();

            MuhurtaCalendar previousMuhurta = sortedMuhurtaList.First();
            foreach (MuhurtaCalendar c in sortedMuhurtaList)
            {
               
                endDate = c.DateEnd;
                if (c.DateStart > date)
                    startDate = c.DateStart;
                if (c.DateEnd > dayEndDate)
                    endDate = dayEndDate;
                if (c.OverlapedMuhurtaCode == EMuhurta.NOMUHURTA && previousMuhurta.OverlapedMuhurtaCode == EMuhurta.NOMUHURTA)
                {
                    setAppointments(startDate, endDate, c.GetFullName(lang), Utility.GetColorByColorCode(c.ColorCode), true, group, true);
                }
                else if (c.OverlapedMuhurtaCode == EMuhurta.NOMUHURTA && previousMuhurta.OverlapedMuhurtaCode != EMuhurta.NOMUHURTA)
                {
                    setAppointments(startDate, endDate, "", Utility.GetColorByColorCode(c.ColorCode), true, group, true);
                }
                else if (c.OverlapedMuhurtaCode != EMuhurta.NOMUHURTA)
                {
                    setAppointments(startDate, endDate, c.GetOverlapedShortName(lang), Utility.GetColorByColorCode(c.ColorCode), true, group, true);
                }
                previousMuhurta = c;
            }
        }
        
        private void TabDay_Shown(object sender, EventArgs e)
        {
            DrawSystemAppointments(_currDay, _langCode);

            //existing user events
            if (_pevList != null)
            {
                string group = ((int)EDVNames.USER).ToString();
                foreach (PersonsEventsList pev in _pevList)
                {
                    setAppointments(pev.DateStart, pev.DateEnd, pev.Name, Color.FromArgb(pev.ARGBValue), false, group, false);
                }
            }
        }
        
        public void DrawSystemAppointments(Day day, ELanguage lang)
        {
            EAppSetting tranzitSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting horaSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.HORA.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting muhGhatiSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.MUHURTAGHATI.ToString()) && i.Active == 1).FirstOrDefault().Id;

            DrawMuhurtaLine(day.MuhurtaDayList, day.Date, groupTags(EDVNames.MUHURTA.ToString()), lang);
            DrawJogaLine(day, groupTags(EDVNames.YOGA.ToString()), lang);
            DrawDayLine(day.NakshatraDayList, groupTags(EDVNames.NAKSHATRA.ToString()), false);
            DrawDayLine(day.TaraBalaDayList, groupTags(EDVNames.TARABALA.ToString()), false);
            DrawDayLine(day.TithiDayList, groupTags(EDVNames.TITHI.ToString()), false);
            DrawDayLine(day.KaranaDayList, groupTags(EDVNames.KARANA.ToString()), false);
            DrawDayLine(day.NityaJogaDayList, groupTags(EDVNames.NITYAYOGA.ToString()), false);
            DrawDayLine(day.ChandraBalaDayList, groupTags(EDVNames.CHANDRABALA.ToString()), false);

            switch (horaSettings)
            {
                case EAppSetting.HORADAYNIGHT:
                    DrawDayLine(day.Hora12Plus12DayList, groupTags(EDVNames.HORA.ToString()), false);
                    break;
                case EAppSetting.HORAEQUAL:
                    DrawDayLine(day.HoraEqualDayList, groupTags(EDVNames.HORA.ToString()), false);
                    break;
                case EAppSetting.HORAFROM6:
                    DrawDayLine(day.HoraFrom6DayList, groupTags(EDVNames.HORA.ToString()), false);
                    break;
            }

            switch (muhGhatiSettings)
            {
                case EAppSetting.MUHURTAGHATIDAYNIGHT:
                    DrawDayLine(day.Muhurta15Plus1530DayList, groupTags(EDVNames.MUHURTA30.ToString()), false);
                    DrawDayLine(day.Ghati60_30Plus30DayList, groupTags(EDVNames.GHTATI60.ToString()), false);
                    break;
                case EAppSetting.MUHURTAGHATIEQUAL:
                    DrawDayLine(day.MuhurtaEqual30DayList, groupTags(EDVNames.MUHURTA30.ToString()), false);
                    DrawDayLine(day.Ghati60EqualDayList, groupTags(EDVNames.GHTATI60.ToString()), false);
                    break;
                case EAppSetting.MUHURTAGHATIFROM6:
                    DrawDayLine(day.Muhurta30From6DayList, groupTags(EDVNames.MUHURTA30.ToString()), false);
                    DrawDayLine(day.Ghati60From6DayList, groupTags(EDVNames.GHTATI60.ToString()), false);
                    break;
            }

            DrawDayLine(day.MoonPadaDayList, groupTags(EDVNames.MOONPADA.ToString()), true);
            DrawDayLine(day.SunPadaDayList, groupTags(EDVNames.SUNPADA.ToString()), true);
            DrawDayLine(day.VenusPadaDayList, groupTags(EDVNames.VENUSPADA.ToString()), true);
            DrawDayLine(day.JupiterPadaDayList, groupTags(EDVNames.JUPITERPADA.ToString()), true);
            DrawDayLine(day.MercuryPadaDayList, groupTags(EDVNames.MERCURYPADA.ToString()), true);
            DrawDayLine(day.MarsPadaDayList, groupTags(EDVNames.MARSPADA.ToString()), true);
            DrawDayLine(day.SaturnPadaDayList, groupTags(EDVNames.SATURNPADA.ToString()), true);
            
            switch (nodeSettings)
            {
                case EAppSetting.NODEMEAN:
                    DrawDayLine(day.RahuMeanPadaDayList, groupTags(EDVNames.RAHUPADA.ToString()), true);
                    break;
                case EAppSetting.NODETRUE:
                    DrawDayLine(day.RahuTruePadaDayList, groupTags(EDVNames.RAHUPADA.ToString()), true);
                    break;
            }

            switch (nodeSettings)
            {
                case EAppSetting.NODEMEAN:
                    DrawDayLine(day.KetuMeanPadaDayList, groupTags(EDVNames.KETUPADA.ToString()), true);
                    break;
                case EAppSetting.NODETRUE:
                    DrawDayLine(day.KetuTruePadaDayList, groupTags(EDVNames.KETUPADA.ToString()), true);
                    break;
            }
        }

        public void setAppointments(DateTime startDate, DateTime endDate, String title, Color color, Boolean astrologicalEvent, string group, bool system)
        {
            Appointment m_Appointment = new Appointment();

            if (astrologicalEvent)
            {
                m_Appointment.StartDate = startDate.AddDays(1);
                m_Appointment.EndDate = endDate.AddDays(1);
                m_Appointment.Locked = true;
            }
            else
            {
                m_Appointment.StartDate = startDate;
                m_Appointment.EndDate = endDate;
                m_Appointment.Locked = false;
            }

            m_Appointment.Title = title;
            m_Appointment.AllDayEvent = false;
            m_Appointment.Color = color;

            m_Appointment.IsSystem = system;

            if (startDate.Equals(endDate))
            {
                m_Appointment.DrawBorder = false;
            }
            else
            {
                m_Appointment.DrawBorder = true;
            }
            m_Appointment.BorderColor = Color.Black;

            m_Appointment.Group = group;

            m_Appointments.Add(m_Appointment);
        }

        private void dayView_ResolveAppointments(object sender, ResolveAppointmentsEventArgs args)
        {
           List<Appointment> m_Apps = new List<Appointment>();

            foreach (Appointment m_App in m_Appointments)
                if ((m_App.StartDate >= args.StartDate) &&
                    (m_App.StartDate <= args.EndDate))
                    m_Apps.Add(m_App);

            args.Appointments = m_Apps;
        }

        void dayView_NewAppointment(object sender, NewAppointmentEventArgs args)
        {
            Appointment m_Appointment = new Appointment();

            m_Appointment.StartDate = args.StartDate;
            m_Appointment.EndDate = args.EndDate;
            m_Appointment.Title = args.Title;

            m_Appointments.Add(m_Appointment);
        }

        private void dayView_DoubleClick(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment != null && !dayView.SelectedAppointment.IsSystem)
            {
                EditAppointment();
            }
            else if (dayView.SelectedAppointment != null && dayView.SelectedAppointment.IsSystem)
            {
                int posX = MousePosition.X - 30;
                int posY = MousePosition.Y - 120;

                EDVNames group = (EDVNames)Convert.ToInt32(dayView.SelectedAppointment.Group);
                
                DayViewToolTipCreationSystem(group, _langCode);
                if (toolTip != null)
                    toolTip.Show(dayView, posX, posY);
                toolTip = null;
            }
            else
            {
                CreateNewAppointment();
            }
        }

        private void dayView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int posX = MousePosition.X - 30;
                int posY = MousePosition.Y - 120;

                contextMenuStripEvents.Show(dayView, posX, posY);
            }
        }

        private void addEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectionStart == DateTime.Parse("01.01.0001 00:00:00") || dayView.SelectionEnd == DateTime.Parse("01.01.0001 00:00:00"))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Please select a time period.", _langCode), Utility.GetLocalizedText("Error", _langCode), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }

            CreateNewAppointment();
        }

        private void editEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment == null)
                return;

            EditAppointment();
        }

        private void deleteEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment == null)
                return;

            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to remove this appointment?", _langCode), Utility.GetLocalizedText("Confirmation", _langCode), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DeleteAppointment();
            }
        }

        public void CreateNewAppointment()
        {
            // here should be check for second part (where lines) 
            if (dayView.SelectedAppointment == null)
            {
                using (var UserEventMenu = new UserEventMenu(_curProfile, dayView.SlotsPerHour, _langCode))
                {
                    Appointment m_App = new Appointment();
                    m_App.BorderColor = Color.Black;
                    m_App.Locked = true;

                    UserEventMenu.EVStartDate = dayView.SelectionStart;
                    UserEventMenu.EVEndDate = dayView.SelectionEnd;

                    UserEventMenu.ShowDialog();

                    m_App.StartDate = UserEventMenu.EVStartDate;
                    m_App.EndDate = UserEventMenu.EVEndDate;
                    m_App.Title = UserEventMenu.EventName;
                    m_App.Message = UserEventMenu.EventMessage;
                    m_App.Color = UserEventMenu.controlColor;
                    m_App.IsSystem = false;
                    m_App.Group = ((int)EDVNames.USER).ToString();

                    if (UserEventMenu.ToCreate)
                        m_Appointments.Add(m_App);

                    dayView.Invalidate();

                    //hard appointment refresh
                    #region appointments refresh
                    ClearAppointments(false);

                    //refresh list of user appointments from db
                    _pevList = Utility.GetDayEventsList(Utility.GetDayPersonEvents(_curProfile.GUID, _currDay.Date), _currDay.Date);

                    //creates new user appointments
                    foreach (PersonsEventsList pev in _pevList)
                    {
                        setAppointments(pev.DateStart, pev.DateEnd, pev.Name, Color.FromArgb(pev.ARGBValue), false, ((int)EDVNames.USER).ToString(), false);
                    }

                    #endregion
                }
            }
        }

        public void CreateNewAppointment(DateTime startDate)
        {
            using (var UserEventMenu = new UserEventMenu(_curProfile, dayView.SlotsPerHour, _langCode))
            {
                Appointment m_App = new Appointment();
                m_App.BorderColor = Color.Black;
                m_App.Locked = true;

                UserEventMenu.EVStartDate = startDate;
                UserEventMenu.EVEndDate = startDate.AddMinutes(+15);

                UserEventMenu.ShowDialog();

                m_App.StartDate = UserEventMenu.EVStartDate;
                m_App.EndDate = UserEventMenu.EVEndDate;
                m_App.Title = UserEventMenu.EventName;
                m_App.Message = UserEventMenu.EventMessage;
                m_App.Color = UserEventMenu.controlColor;
                m_App.IsSystem = false;
                m_App.Group = ((int)EDVNames.USER).ToString();

                if (UserEventMenu.ToCreate)
                {
                    m_Appointments.Add(m_App);
                    isAppCreated = true;
                }

                dayView.Invalidate();

                //hard appointment refresh
                #region appointments refresh
                ClearAppointments(false);

                //refresh list of user appointments from db
                _pevList = Utility.GetDayEventsList(Utility.GetDayPersonEvents(_curProfile.GUID, _currDay.Date), _currDay.Date);

                //creates new user appointments
                foreach (PersonsEventsList pev in _pevList)
                {
                    setAppointments(pev.DateStart, pev.DateEnd, pev.Name, Color.FromArgb(pev.ARGBValue), false, ((int)EDVNames.USER).ToString(), false);
                }

                #endregion
            }
        }

        public void CreateNewAppointment(DateTime startDate, DateTime endDate)
        {
            using (var UserEventMenu = new UserEventMenu(_curProfile, dayView.SlotsPerHour, _langCode))
            {
                Appointment m_App = new Appointment();
                m_App.BorderColor = Color.Black;
                m_App.Locked = true;

                UserEventMenu.EVStartDate = startDate;
                UserEventMenu.EVEndDate = endDate;

                UserEventMenu.ShowDialog();

                m_App.StartDate = UserEventMenu.EVStartDate;
                m_App.EndDate = UserEventMenu.EVEndDate;
                m_App.Title = UserEventMenu.EventName;
                m_App.Message = UserEventMenu.EventMessage;
                m_App.Color = UserEventMenu.controlColor;
                m_App.IsSystem = false;
                m_App.Group = ((int)EDVNames.USER).ToString();

                if (UserEventMenu.ToCreate)
                {
                    m_Appointments.Add(m_App);
                    isAppCreated = true;
                }

                dayView.Invalidate();

                //hard appointment refresh
                #region appointments refresh
                ClearAppointments(false);

                //refresh list of user appointments from db
                _pevList = Utility.GetDayEventsList(Utility.GetDayPersonEvents(_curProfile.GUID, _currDay.Date), _currDay.Date);

                //creates new user appointments
                foreach (PersonsEventsList pev in _pevList)
                {
                    setAppointments(pev.DateStart, pev.DateEnd, pev.Name, Color.FromArgb(pev.ARGBValue), false, ((int)EDVNames.USER).ToString(), false);
                }

                #endregion
            }
        }

        public void ClearAppointments(bool isSys)
        {
            int[] _tempIndex = { };
            List<int> _tempListIndex = new List<int>();

            if (isSys)
            {
                for (int index = 0; index < m_Appointments.Count; index++)
                {
                    if (m_Appointments[index].IsSystem)
                    {
                        _tempListIndex.Add(index); //found out what app to delete
                    }
                }
            }
            else
            {
                for (int index = 0; index < m_Appointments.Count; index++)
                {
                    if (!m_Appointments[index].IsSystem)
                    {
                        _tempListIndex.Add(index); //found out what app to delete
                    }
                }
            }

            if (_tempListIndex.Count > 0)
            {
                m_Appointments.RemoveRange(_tempListIndex[0], _tempListIndex.Count); //with condition that user events will be generated one be one
            }
        }

        public void EditAppointment()
        {
            if (dayView.SelectedAppointment != null)
            {
                int index = _pevList.FindIndex(i => i.Name.Equals(dayView.SelectedAppointment.Title) && i.DateStart == dayView.SelectedAppointment.StartDate && i.DateEnd == dayView.SelectedAppointment.EndDate);
                using (var UserEventMenu = new UserEventMenu(_curProfile, _pevList[index], dayView.SlotsPerHour, _langCode))
                {
                    UserEventMenu.EVStartDate = dayView.SelectedAppointment.StartDate;
                    UserEventMenu.EVEndDate = dayView.SelectedAppointment.EndDate;
                    UserEventMenu.ShowDialog();

                    if (UserEventMenu.ToCreate)
                    {
                        dayView.SelectedAppointment.StartDate = UserEventMenu.EVStartDate;
                        dayView.SelectedAppointment.EndDate = UserEventMenu.EVEndDate;
                        dayView.SelectedAppointment.Title = UserEventMenu.EventName;
                        dayView.SelectedAppointment.Message = UserEventMenu.EventMessage;
                        dayView.SelectedAppointment.Color = UserEventMenu.controlColor;
                    }
                    dayView.Invalidate();
                }
                //we need to refresh _pevList here so it will read db and get new appointments
                #region _pevList hack update
                _pevList = Utility.GetDayEventsList(Utility.GetDayPersonEvents(_curProfile.GUID, _currDay.Date), _currDay.Date);
                #endregion
            }
        }

        public void DeleteAppointment()
        {
            if (dayView.SelectedAppointment != null)
            {
                //we need to refresh _pevList here so it will read db and get new appointments
                #region _pevList hack update
                _pevList = Utility.GetDayEventsList(Utility.GetDayPersonEvents(_curProfile.GUID, _currDay.Date), _currDay.Date);
                #endregion

                int index = _pevList.FindIndex(i => i.Name.Equals(dayView.SelectedAppointment.Title) && i.DateStart == dayView.SelectedAppointment.StartDate && i.DateEnd == dayView.SelectedAppointment.EndDate);
                using (SQLiteConnection dbCon = Utility.GetSQLConnection())
                {
                    dbCon.Open();
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand("delete from USER_EVENTS where ID = @ID", dbCon);
                        command.Parameters.AddWithValue("@ID", _pevList[index].Id);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    dbCon.Close();
                }

                m_Appointments.Remove(dayView.SelectedAppointment);
                dayView.Invalidate();
            }
        }

        private List<ToolTipEntity> GetToolTipEntitiesList(EDVNames group, DateTime startDate, DateTime endDate, ELanguage lCode)
        {
            List<ToolTipEntity> ttEList = new List<ToolTipEntity>();

            EAppSetting tranzitSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting horaSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.HORA.ToString()) && i.Active == 1).FirstOrDefault().Id;
            EAppSetting muhGhatiSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.MUHURTAGHATI.ToString()) && i.Active == 1).FirstOrDefault().Id;

            List<Calendar> pList, zList, lList;
            List<PlanetCalendar> clonedZList = null;
            List<PlanetCalendar> clonedPList = null;

            string desc1 = string.Empty, desc2 = string.Empty, desc3 = string.Empty, desc4 = string.Empty, desc5 = string.Empty;
            switch (group)
            {
                case EDVNames.USER:
                    string title = Utility.GetLocalizedText("Title", lCode) + ": " + dayView.SelectedAppointment.Title;
                    string tPeriod = Utility.GetLocalizedText("Time period", _langCode) + ": " + startDate.ToString("dd.MM.yyyy HH:mm:ss") + " - " + endDate.ToString("dd.MM.yyyy HH:mm:ss");
                    desc2 = Utility.GetLocalizedText("Description", lCode) + ": " + _pevList.Where(i => i.Name.Equals(dayView.SelectedAppointment.Title)).FirstOrDefault().Message;
                    ttEList.Add(new ToolTipEntity { Code = EDVNames.USER, Title = title, Period = tPeriod, Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    break;

                case EDVNames.MUHURTA:
                    List<Calendar> mcList = _currDay.MuhurtaDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<MuhurtaCalendar> clonedMCList = Utility.CloneMuhurtaCalendarList(mcList);
                    foreach (MuhurtaCalendar mc in clonedMCList)
                    {
                        string mName = CacheLoad._muhurtaDescList.Where(i => i.MuhurtaId == (int)mc.MuhurtaCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.MUHURTA, Title = mName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + mc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + mc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.YOGA:
                    List<JogaCalendar> jogaList = Utility.GetYogasListForTimePeriod(_currDay, startDate.AddDays(-1), endDate.AddDays(-1));
                    EAppSetting weekSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault().Id;
                    foreach (JogaCalendar jc in jogaList)
                    {
                        string yName = CacheLoad._jogaDescList.Where(i => i.JogaId == (int)jc.JogaCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string yDesc = CacheLoad._jogaDescList.Where(i => i.JogaId == (int)jc.JogaCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        string nName = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)jc.NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string tName = CacheLoad._tithiDescList.Where(i => i.TithiId == jc.TithiId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        int day = Utility.GetDayOfWeekNumberFromDate(jc.DateStart, weekSetting);
                        string vara = Utility.GetDaysOfWeekName(day, lCode, weekSetting);
                        string nakshatra = string.Empty, tithi = string.Empty;
                        if (jc.NakshatraCode != ENakshatra.NULL)
                        {
                            nakshatra = Utility.GetLocalizedText("Nakshatra", lCode) + ": " + (int)jc.NakshatraCode + "." + nName + "; ";
                        }
                        if (jc.TithiId != 0)
                        {
                            tithi = Utility.GetLocalizedText("Tithi", lCode) + ": " + jc.TithiId + "." + tName + "; ";
                        }
                        desc2 = Utility.GetLocalizedText("Vara", lCode) + ": " + vara + "; " + nakshatra + tithi + yDesc;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.YOGA, Title = yName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + jc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + jc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.NAKSHATRA:
                    List<Calendar> ncList = _currDay.NakshatraDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<NakshatraCalendar> clonedNakshatraList = Utility.CloneNakshatraCalendarList(ncList);
                    List<Calendar> sncList = _currDay.ShunyaNakshatraDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<ShunyaNakshatraCalendar> clonedSNCList = Utility.CloneShunyaNakshatraCalendarList(sncList);
                    foreach (NakshatraCalendar nc in clonedNakshatraList)
                    {
                        NakshatraDescription nak = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)nc.NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string nakshatra = (int)nc.NakshatraCode + "." + nak.Name + " (" + nak.Upravitel + ") ";
                        foreach (ShunyaNakshatraCalendar snc in clonedSNCList)
                        {
                            if(snc.DateStart >= nc.DateStart || snc.DateEnd <= nc.DateEnd)
                            {
                                desc1 = Utility.GetLocalizedText("Shunya Nakshatra", lCode) + ": " + snc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + snc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss");
                            }
                        }

                        desc2 = Utility.GetLocalizedText("Nature", lCode) + ": " + nak.Nature;
                        desc3 = Utility.GetLocalizedText("Description", lCode) + ": " + nak.Description;
                        if (!nak.GoodFor.Equals(string.Empty))
                        {
                            desc4 = Utility.GetLocalizedText("Good for", lCode) + ": " + nak.GoodFor;
                        }
                        if (!nak.BadFor.Equals(string.Empty))
                        {
                            desc5 = Utility.GetLocalizedText("Bad for", lCode) + ": " + nak.BadFor;
                        }
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.NAKSHATRA, Title = nakshatra, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + nc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + nc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.TARABALA:
                    List<Calendar> tbList = _currDay.TaraBalaDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<TaraBalaCalendar> clonedTaraBalaList = Utility.CloneTaraBalaCalendarList(tbList);
                    foreach (TaraBalaCalendar tbc in clonedTaraBalaList)
                    {
                        TaraBalaDescription tb = CacheLoad._taraBalaDescList.Where(i => i.TaraBalaId == tbc.TaraBalaId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        desc2 = Utility.GetLocalizedText("Description", lCode) + ": " + tb.Description;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.TARABALA, Title = tbc.GetShortName(lCode), Period = Utility.GetLocalizedText("Time period", lCode) + ": " + tbc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + tbc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.TITHI:
                    List<Calendar> tList = _currDay.TithiDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<TithiCalendar> clonedTithiList = Utility.CloneTithiCalendarList(tList);
                    List<Calendar> stcList = _currDay.ShunyaTithiDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<ShunyaTithiCalendar> clonedSTCList = Utility.CloneShunyaTithiCalendarList(stcList);
                    foreach (TithiCalendar tc in clonedTithiList)
                    {
                        foreach (ShunyaTithiCalendar stc in clonedSTCList)
                        {
                            if (stc.DateStart >= tc.DateStart || stc.DateEnd <= tc.DateEnd)
                            {
                                desc1 = Utility.GetLocalizedText("Shunya Tithi", lCode) + ": " + stc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + stc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss");
                            }
                        }

                        TithiDescription th = CacheLoad._tithiDescList.Where(i => i.TithiId == tc.TithiId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        desc2 = Utility.GetLocalizedText("Type", lCode) + ": " + th.Type;
                        if (!th.GoodFor.Equals(string.Empty))
                        {
                            desc3 = Utility.GetLocalizedText("Good for", lCode) + ": " + th.GoodFor;
                        }
                        if (!th.BadFor.Equals(string.Empty))
                        {
                            desc4 = Utility.GetLocalizedText("Bad for", lCode) + ": " + th.BadFor;
                        }
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.TITHI, Title = tc.GetShortName(lCode), Period = Utility.GetLocalizedText("Time period", lCode) + ": " + tc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + tc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.KARANA:
                    List<Calendar> kList = _currDay.KaranaDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<KaranaCalendar> clonedKaranaList = Utility.CloneKaranaCalendarList(kList);
                    foreach (KaranaCalendar kc in clonedKaranaList)
                    {
                        KaranaDescription kar = CacheLoad._karanaDescList.Where(i => i.KaranaId == kc.KaranaId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string kName = kar.Name + " (" + kar.Upravitel + ")";
                        if(!kar.GoodFor.Equals(string.Empty))
                        {
                            desc2 = Utility.GetLocalizedText("Good for", lCode) + ": " + kar.GoodFor;
                        }
                        if (!kar.BadFor.Equals(string.Empty))
                        {
                            desc3 = Utility.GetLocalizedText("Bad for", lCode) + ": " + kar.BadFor;
                        }
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.KARANA, Title = kName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + kc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + kc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.NITYAYOGA:
                    List<Calendar> njList = _currDay.NityaJogaDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<NityaJogaCalendar> clonedNityaJogaList = Utility.CloneNityaJogaCalendarList(njList);
                    foreach (NityaJogaCalendar njc in clonedNityaJogaList)
                    {
                        NityaJogaDescription njd = CacheLoad._nityaJogaDescList.Where(i => i.NityaJogaId == (int)njc.NJCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        int yogaPlanetId = CacheLoad._nityaJogaList.Where(i => i.Id == (int)njc.NJCode).FirstOrDefault()?.JogiPlanetId ?? 0;
                        string upravitel = CacheLoad._planetDescList.Where(i => i.PlanetId == yogaPlanetId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string name = njc.GetShortName(lCode) + " (" + upravitel + ", " + njd.Deity + ")";
                        string nakshatra = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == njc.NakshatraId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        desc2 = Utility.GetLocalizedText("Nakshatra", lCode) + ": " + njc.NakshatraId + "." + nakshatra;
                        desc3 = Utility.GetLocalizedText("Meaning", lCode) + ": " + njd.Meaning;
                        desc4 = Utility.GetLocalizedText("Description", lCode) + ": " + njd.Description;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.NITYAYOGA, Title = name, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + njc.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + njc.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.CHANDRABALA:
                    List<Calendar> cbList = _currDay.ChandraBalaDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                    List<ChandraBalaCalendar> clonedCBList = Utility.CloneChandraBalaCalendarList(cbList);
                    foreach (ChandraBalaCalendar cb in clonedCBList)
                    {
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.CHANDRABALA, Title = cb.GetShortName(lCode), Period = Utility.GetLocalizedText("Time period", lCode) + ": " + cb.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + cb.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.HORA:
                    List<Calendar> hList = null;
                    switch (horaSettings)
                    {
                        case EAppSetting.HORADAYNIGHT:
                            hList = _currDay.Hora12Plus12DayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                        case EAppSetting.HORAEQUAL:
                            hList = _currDay.HoraEqualDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                        case EAppSetting.HORAFROM6:
                            hList = _currDay.HoraFrom6DayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                    }
                    List<HoraCalendar> clonedHList = Utility.CloneHoraCalendarList(hList);
                    foreach (HoraCalendar h in clonedHList)
                    {
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.HORA, Title = h.GetShortName(lCode), Period = Utility.GetLocalizedText("Time period", lCode) + ": " + h.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + h.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.MUHURTA30:
                    List<Calendar> m30List = null;
                    switch (muhGhatiSettings)
                    {
                        case EAppSetting.MUHURTAGHATIDAYNIGHT:
                            m30List = _currDay.Muhurta15Plus1530DayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                        case EAppSetting.MUHURTAGHATIEQUAL:
                            m30List = _currDay.MuhurtaEqual30DayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                        case EAppSetting.MUHURTAGHATIFROM6:
                            m30List = _currDay.Muhurta30From6DayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                    }
                    List<Muhurta30Calendar> clonedM30List = Utility.CloneMuhurta30CalendarList(m30List);
                    foreach (Muhurta30Calendar m30 in clonedM30List)
                    {
                        Muhurta30Description m30Desc = CacheLoad._muhurta30DescList.Where(i => i.Muhurta30Id == (int)m30.Muhurta30Code && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        desc2 = Utility.GetLocalizedText("Description", lCode) + ": " + m30Desc.Description;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.MUHURTA30, Title = m30Desc.Name, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + m30.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + m30.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.GHTATI60:
                    List<Calendar> g60List = null;
                    switch (muhGhatiSettings)
                    {
                        case EAppSetting.MUHURTAGHATIDAYNIGHT:
                            g60List = _currDay.Ghati60_30Plus30DayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                        case EAppSetting.MUHURTAGHATIEQUAL:
                            g60List = _currDay.Ghati60EqualDayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                        case EAppSetting.MUHURTAGHATIFROM6:
                            g60List = _currDay.Ghati60From6DayList.Where(i => i.DateStart <= startDate.AddDays(-1) && i.DateEnd >= endDate.AddDays(-1)).ToList();
                            break;
                    }
                    List<Ghati60Calendar> clonedG60List = Utility.CloneGhati60CalendarList(g60List);
                    foreach (Ghati60Calendar g60 in clonedG60List)
                    {
                        Ghati60Description g60Desc = CacheLoad._ghati60DescList.Where(i => i.Ghati60Id == (int)g60.Ghati60Code && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        desc2 = Utility.GetLocalizedText("Description", lCode) + ": " + g60Desc.Description;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.GHTATI60, Title = g60Desc.Name, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + g60.DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + g60.DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
                    
                case EDVNames.MOONPADA:
                    pList = _currDay.MoonPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    clonedPList = Utility.ClonePlanetCalendarList(pList);
                    zList = _currDay.MoonZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (zList.Count == 0)
                    {
                        zList = _currDay.MoonZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                    }
                    lList = _currDay.MoonZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1)) ).ToList();
                    if (lList.Count == 0)
                    {
                        lList = _currDay.MoonZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc,  Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.MOONPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.SUNPADA:
                    pList = _currDay.SunPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    clonedPList = Utility.ClonePlanetCalendarList(pList);
                    zList = _currDay.SunZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (zList.Count == 0)
                    {
                        zList = _currDay.SunZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                    }
                    lList = _currDay.SunZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (lList.Count == 0)
                    {
                        lList = _currDay.SunZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.SUNPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.VENUSPADA:
                    pList = _currDay.VenusPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    clonedPList = Utility.ClonePlanetCalendarList(pList);
                    zList = _currDay.VenusZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (zList.Count == 0)
                    {
                        zList = _currDay.VenusZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                    }
                    lList = _currDay.VenusZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (lList.Count == 0)
                    {
                        lList = _currDay.VenusZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc3 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.VENUSPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.JUPITERPADA:
                    pList = _currDay.JupiterPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    clonedPList = Utility.ClonePlanetCalendarList(pList);
                    zList = _currDay.JupiterZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (zList.Count == 0)
                    {
                        zList = _currDay.JupiterZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                    }
                    lList = _currDay.JupiterZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (lList.Count == 0)
                    {
                        lList = _currDay.JupiterZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                            }

                            string planetList = string.Empty;
                            if (vedhaList != null)
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.JUPITERPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.MERCURYPADA:
                    pList = _currDay.MercuryPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    clonedPList = Utility.ClonePlanetCalendarList(pList);
                    zList = _currDay.MercuryZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (zList.Count == 0)
                    {
                        zList = _currDay.MercuryZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                    }
                    lList = _currDay.MercuryZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (lList.Count == 0)
                    {
                        lList = _currDay.MercuryZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.MERCURYPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.MARSPADA:
                    pList = _currDay.MarsPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    clonedPList = Utility.ClonePlanetCalendarList(pList);
                    zList = _currDay.MarsZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (zList.Count == 0)
                    {
                        zList = _currDay.MarsZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                    }
                    lList = _currDay.MarsZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (lList.Count == 0)
                    {
                        lList = _currDay.MarsZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.MARSPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.SATURNPADA:
                    pList = _currDay.SaturnPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    clonedPList = Utility.ClonePlanetCalendarList(pList);
                    zList = _currDay.SaturnZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (zList.Count == 0)
                    {
                        zList = _currDay.SaturnZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                    }
                    lList = _currDay.SaturnZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                    if (lList.Count == 0)
                    {
                        lList = _currDay.SaturnZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.SATURNPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.RAHUPADA:
                    if (nodeSettings == EAppSetting.NODEMEAN)
                    {
                        pList = _currDay.RahuMeanPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        clonedPList = Utility.ClonePlanetCalendarList(pList);
                        zList = _currDay.RahuMeanZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (zList.Count == 0)
                        {
                            zList = _currDay.RahuMeanZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                        }
                        lList = _currDay.RahuMeanZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (lList.Count == 0)
                        {
                            lList = _currDay.RahuMeanZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                        }
                    }
                    else
                    {
                        pList = _currDay.RahuTruePadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        clonedPList = Utility.ClonePlanetCalendarList(pList);
                        zList = _currDay.RahuTrueZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (zList.Count == 0)
                        {
                            zList = _currDay.RahuTrueZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                        }
                        lList = _currDay.RahuTrueZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (lList.Count == 0)
                        {
                            lList = _currDay.RahuTrueZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                        }
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.RAHUPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;

                case EDVNames.KETUPADA:
                    if (nodeSettings == EAppSetting.NODEMEAN)
                    {
                        pList = _currDay.KetuMeanPadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        clonedPList = Utility.ClonePlanetCalendarList(pList);
                        zList = _currDay.KetuMeanZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (zList.Count == 0)
                        {
                            zList = _currDay.KetuMeanZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                        }
                        lList = _currDay.KetuMeanZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (lList.Count == 0)
                        {
                            lList = _currDay.KetuMeanZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                        }
                    }
                    else
                    {
                        pList = _currDay.KetuTruePadaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        clonedPList = Utility.ClonePlanetCalendarList(pList);
                        zList = _currDay.KetuTrueZodiakDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (zList.Count == 0)
                        {
                            zList = _currDay.KetuTrueZodiakDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < endDate.AddDays(-1))).ToList();
                        }
                        lList = _currDay.KetuTrueZodiakLagnaDayList.Where(i => (i.DateStart <= startDate.AddDays(-1) && i.DateEnd > startDate.AddDays(-1))).ToList();
                        if (lList.Count == 0)
                        {
                            lList = _currDay.KetuTrueZodiakLagnaDayList.Where(i => (i.DateEnd >= startDate.AddDays(-1) && i.DateStart < startDate.AddDays(-1))).ToList();
                        }
                    }
                    switch (tranzitSetting)
                    {
                        case EAppSetting.TRANZITMOON:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                        case EAppSetting.TRANZITLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(lList);
                            break;
                        case EAppSetting.TRANZITMOONANDLAGNA:
                            clonedZList = Utility.ClonePlanetCalendarList(zList);
                            break;
                    }
                    foreach (PlanetCalendar pc in clonedZList)
                    {
                        int dom = pc.Dom;
                        Tranzit tr = CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.Dom == dom).FirstOrDefault();
                        TranzitDescription trDesc = CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault();
                        string pName = CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pc.PlanetCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string zodiak = CacheLoad._zodiakDescList.Where(i => i.ZodiakId == (int)pc.ZodiakCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        string nakshatra = (int)clonedPList.First().NakshatraCode + "." + CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == (int)clonedPList.First().NakshatraCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                        Pada sPada = CacheLoad._padaList.Where(i => i.Id == clonedPList.First().PadaId).FirstOrDefault();
                        int padaNum = sPada.PadaNumber;
                        int navamsha = sPada.Navamsha;
                        string specNavamsha = GetSpecNavamsha(sPada, lCode);

                        string vedha = string.Empty;
                        if (!tr.Vedha.Equals(string.Empty))
                        {
                            // Make a list of vedha planets to add into desc2
                            List<VedhaEntity> vedhaList = null;
                            switch (tranzitSetting)
                            {
                                case EAppSetting.TRANZITMOON:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
                                    break;
                                case EAppSetting.TRANZITLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), true);
                                    break;
                                case EAppSetting.TRANZITMOONANDLAGNA:
                                    vedhaList = Utility.PrepareVedhaPlanetList(_currDay, pc, Convert.ToInt32(tr.Vedha), false);
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
                                if (planetList.Length > 2)
                                {
                                    vedha = "; " + Utility.GetLocalizedText("Vedha from", lCode) + ": " + planetList.Substring(0, (planetList.Length - 2));
                                }
                            }
                        }
                        desc2 = zodiak + ", " + nakshatra + ", " + Utility.GetLocalizedText("Pada", lCode) + ": " + padaNum + ", " + Utility.GetLocalizedText("Navamsa", lCode) + ": " + navamsha + specNavamsha;
                        if (tranzitSetting == EAppSetting.TRANZITMOON || tranzitSetting == EAppSetting.TRANZITMOONANDLAGNA)
                        {
                            desc3 = Utility.GetLocalizedText("House from Moon", lCode) + ": " + dom + vedha;
                        }
                        else
                        {
                            desc3 = Utility.GetLocalizedText("House from Lagna", lCode) + ": " + dom + vedha;
                        }
                        desc4 = Utility.GetLocalizedText("Description of tranzit", lCode) + ": " + CacheLoad._tranzitDescList.Where(i => i.TranzitId == tr.Id && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Description ?? string.Empty;
                        ttEList.Add(new ToolTipEntity { Code = EDVNames.KETUPADA, Title = pName, Period = Utility.GetLocalizedText("Time period", lCode) + ": " + clonedPList.First().DateStart.ToString("dd.MM.yyyy HH:mm:ss") + " - " + clonedPList.First().DateEnd.ToString("dd.MM.yyyy HH:mm:ss"), Description1 = desc1, Description2 = desc2, Description3 = desc3, Description4 = desc4, Description5 = desc5 });
                    }
                    break;
            }

            return ttEList;
        }

        private string GetSpecNavamsha(Pada sPada, ELanguage sLang)
        {
            try
            {
                string specNavamsha = string.Empty;
                var row = sPada.SpecialNavamsha.Split(new char[] { ',' });
                int[] idList = new int[row.Length];
                for (int i = 0; i < row.Length; i++)
                {
                    idList[i] = Convert.ToInt32(row[i]);
                }
                for (int i = 0; i < idList.Length; i++)
                {
                    string text = CacheLoad._specNavamshaList.Where(r => r.SpeciaNavamshaId == idList[i] && r.LanguageCode.Equals(sLang.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                    specNavamsha += text + ", ";
                }
                specNavamsha = specNavamsha.Substring(0, specNavamsha.Length - 2);
                return (", " + specNavamsha);
            }
            catch
            {
                return string.Empty;
            }
        }

        private void DayViewToolTipCreationSystem(EDVNames group, ELanguage lCode)
        {
            DateTime startDate = dayView.SelectedAppointment.StartDate;
            DateTime endDate = dayView.SelectedAppointment.EndDate;

            Font titleFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.DWTOOLTIPTITLE)), 12, Utility.GetFontStyleBySettings(EFontList.DWTOOLTIPTITLE));
            Font timeFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.DWTOOLTIPTIME)), 10, Utility.GetFontStyleBySettings(EFontList.DWTOOLTIPTIME));
            Font textFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.DWTOOLTIPTEXT)), 10, Utility.GetFontStyleBySettings(EFontList.DWTOOLTIPTEXT));

            // getting system event info
            int formWidth = 600, formHeight = 0;
            List<ToolTipEntity> ttEList = GetToolTipEntitiesList(group, startDate, endDate, lCode);
            if (ttEList.Count > 0)
            {
                if (ttEList[0].Code == EDVNames.MUHURTA || ttEList[0].Code == EDVNames.YOGA || ttEList[0].Code == EDVNames.TARABALA || ttEList[0].Code == EDVNames.CHANDRABALA || ttEList[0].Code == EDVNames.NITYAYOGA || ttEList[0].Code == EDVNames.HORA || ttEList[0].Code == EDVNames.MUHURTA30 || ttEList[0].Code == EDVNames.GHTATI60)
                {
                    Size texSize = TextRenderer.MeasureText(ttEList[0].Period, timeFont);
                    formWidth = texSize.Width + 8;
                }
                foreach (ToolTipEntity tte in ttEList)
                {
                    formHeight += Utility.CalculateRectangleHeightWithTextWrapping(tte.Title, titleFont, formWidth - 8);
                    formHeight += 8;
                    formHeight += Utility.CalculateRectangleHeightWithTextWrapping(tte.Period, timeFont, formWidth - 8);
                    formHeight += 8;
                    if (!tte.Description1.Equals(string.Empty))
                    {
                        formHeight += Utility.CalculateRectangleHeightWithTextWrapping(tte.Description1, textFont, formWidth - 8);
                        formHeight += 8;
                    }
                    if (!tte.Description2.Equals(string.Empty))
                    {
                        formHeight += Utility.CalculateRectangleHeightWithTextWrapping(tte.Description2, textFont, formWidth - 8);
                        formHeight += 8;
                    }
                    if (!tte.Description3.Equals(string.Empty))
                    {
                        formHeight += Utility.CalculateRectangleHeightWithTextWrapping(tte.Description3, textFont, formWidth - 8);
                        formHeight += 12;
                    }
                    if (!tte.Description4.Equals(string.Empty))
                    {
                        formHeight += Utility.CalculateRectangleHeightWithTextWrapping(tte.Description4, textFont, formWidth - 8);
                        formHeight += 30;
                    }
                    if (!tte.Description5.Equals(string.Empty))
                    {
                        formHeight += Utility.CalculateRectangleHeightWithTextWrapping(tte.Description5, textFont, formWidth - 8);
                        formHeight += 8;
                    }
                }
                formHeight += 16;

                toolTip = new Popup(dayViewToolTip = new DayViewToolTip(ttEList, formWidth, formHeight, titleFont, timeFont, textFont));
                toolTip.AutoClose = false;
                toolTip.FocusOnOpen = false;
                toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
                toolTip.AutoClose = true;
            }
        }
        
        private void contextMenuStripEvents_Opening(object sender, CancelEventArgs e)
        {
            Utility.LocalizeControl(contextMenuStripEvents, _langCode);

            if (dayView.SelectedAppointment != null && dayView.SelectedAppointment.IsSystem)
            {
                infoToolStripMenuItem.Enabled = true;
                addEventToolStripMenuItem.Enabled = false;
                editEventToolStripMenuItem.Enabled = false;
                deleteEventToolStripMenuItem.Enabled = false;
                addStartLineToolStripMenuItem.Enabled = true;
                if (dayView.DrawFirstLine)
                {
                    addEndLineToolStripMenuItem.Enabled = true;
                    createEventToolStripMenuItem.Enabled = true;
                    if (dayView.DrawSecondLine)
                    {
                        createConfiguredEventToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        createConfiguredEventToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                    addEndLineToolStripMenuItem.Enabled = false;
                    createEventToolStripMenuItem.Enabled = false;
                }
            }
            else if (dayView.SelectedAppointment != null && !dayView.SelectedAppointment.IsSystem)
            {
                infoToolStripMenuItem.Enabled = true;
                addEventToolStripMenuItem.Enabled = false;
                editEventToolStripMenuItem.Enabled = true;
                deleteEventToolStripMenuItem.Enabled = true;
                addStartLineToolStripMenuItem.Enabled = false;
                addEndLineToolStripMenuItem.Enabled = false;
            }
            else
            {
                infoToolStripMenuItem.Enabled = false;
                addEventToolStripMenuItem.Enabled = true;
                editEventToolStripMenuItem.Enabled = false;
                deleteEventToolStripMenuItem.Enabled = false;
                addStartLineToolStripMenuItem.Enabled = false;
                addEndLineToolStripMenuItem.Enabled = false;
                if (dayView.DrawFirstLine)
                {
                    createEventToolStripMenuItem.Enabled = true;
                    if (dayView.DrawSecondLine)
                    {
                        createConfiguredEventToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        createConfiguredEventToolStripMenuItem.Enabled = false;
                    }
                }
            }
        }

        private void addingStartLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment != null && dayView.SelectedAppointment.IsSystem)
            {
                dayView.DrawStartLine(dayView.SelectedAppointment.StartDate, Utility.GetColorByColorCode(EColor.SELECTRECTANGLE));
                dayView.Refresh();
            }
        }

        private void removingStartLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment != null && dayView.SelectedAppointment.IsSystem)
            {
                dayView.RemoveStartLine(dayView.SelectedAppointment.StartDate);
                if (dayView.DrawSecondLine)
                {
                    dayView.RemoveEndLine(dayView.EndLineDate);
                }
                dayView.Refresh();
            }
        }

        private void addingEndLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment != null && dayView.SelectedAppointment.IsSystem)
            {
                dayView.DrawEndLine(dayView.SelectedAppointment.EndDate, Utility.GetColorByColorCode(EColor.SELECTRECTANGLE));
                dayView.Refresh();
            }
        }

        private void removingEndLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment != null && dayView.SelectedAppointment.IsSystem)
            {
                dayView.RemoveEndLine(dayView.SelectedAppointment.EndDate);
                dayView.Refresh();
            }
        }

        private void createDefaultEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewAppointment(dayView.StartLineDate.AddHours(-24));
            if (isAppCreated)
            {
                dayView.RemoveStartLine(dayView.StartLineDate);
                dayView.RemoveEndLine(dayView.EndLineDate);
            }
            isAppCreated = false;
            createEventToolStripMenuItem.Enabled = false;
        }

        private void createConfiguredEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewAppointment(dayView.StartLineDate.AddHours(-24), dayView.EndLineDate.AddHours(-24));
            if (isAppCreated)
            {
                dayView.RemoveStartLine(dayView.StartLineDate);
                dayView.RemoveEndLine(dayView.EndLineDate);
            }
            isAppCreated = false;
            createEventToolStripMenuItem.Enabled = false;
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dayView.SelectedAppointment == null)
                return;

            int posX = MousePosition.X - 30;
            int posY = MousePosition.Y - 120;

            EDVNames group = (EDVNames)(_dvlnList.Where(i => i.Id.ToString().Equals(dayView.SelectedAppointment.Group)).FirstOrDefault()?.Id ?? 0);
            DayViewToolTipCreationSystem(group, _langCode);

            if (toolTip != null)
            {
                toolTip.Show(dayView, posX, posY);
            }
            toolTip = null;
        }

        

    }
}
