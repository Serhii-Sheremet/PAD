using GeoTimeZone;
using Innovative.SolarCalculator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using TimeZoneConverter;

namespace PAD
{
    public static class Utility
    {
        private static string _dbPath = @".\Data\PADDB.dat";

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            bytes = Encoding.Default.GetBytes(str);
            return bytes;
        }

        public static void DBCreation()
        {
            string connectionString = @".\Data\PADDB.dat";
            string passwordString = "pdcPass20!";
            byte[] passwordBytes = GetBytes(passwordString);
            SQLiteConnection.CreateFile(connectionString);
            SQLiteConnection conn = new SQLiteConnection("Data Source = " + connectionString + "; Version = 3;");
            conn.SetPassword(passwordBytes);
        }

        private static SQLiteConnection GetSQLConnection(string dbPath)
        {
            string filename = dbPath;
            string passwordString = "pdcPass20!";
            return new SQLiteConnection("Data Source = " + filename + "; Version = 3; Password = " + passwordString + ";");
        }

        public static SQLiteConnection GetSQLConnection()
        {
            return GetSQLConnection(_dbPath);
        }

        public static void CleanExactFormOnTabPage(TabControl tcName, string tpKey, string fName)
        {
            foreach (Control c in tcName.TabPages[tpKey].Controls)
            {
                if (c is Form && c.Name.Equals(fName))
                    tcName.TabPages[tpKey].Controls.Remove(c);
            }
        }

        public static void CleanExactLabelOnTabPage(TabControl tcName, string tpKey, string lName)
        {
            foreach (Control c in tcName.TabPages[tpKey].Controls)
            {
                if (c is Label && c.Name.Equals(lName))
                    tcName.TabPages[tpKey].Controls.Remove(c);
            }
        }

        public static bool CheckIfTabPageContainsForm(TabControl tcName, string tpKey, string fName)
        {
            foreach (Control c in tcName.TabPages[tpKey].Controls)
            {
                if (c is Form && c.Name.Equals(fName))
                    return true;
            }
            return false;
        }

        public static bool CheckIfTabPageContainsLabel(TabControl tcName, string tpKey, string lName)
        {
            foreach (Control c in tcName.TabPages[tpKey].Controls)
            {
                if (c is Label && c.Name.Equals(lName))
                    return true;
            }
            return false;
        }

        public static bool CheckIfPanelContainsLabel(Panel pName, string lName)
        {
            foreach (Control c in pName.Controls)
            {
                if (c is Label && c.Name.Equals(lName))
                    return true;
            }
            return false;
        }

        public static void CleanExactLabelOnPanel(Panel pName, string lName)
        {
            foreach (Control c in pName.Controls)
            {
                if (c is Label && c.Name.Equals(lName))
                    pName.Controls.Remove(c);
            }
        }

        public static DataGridView GetDataGridFromTabPage(TabControl tcName, string tpKey)
        {
            DataGridView dgv = null;
            foreach (Control c in tcName.TabPages[tpKey].Controls)
            {
                if (c is DataGridView)
                    dgv = (DataGridView)c;
            }
            return dgv;
        }

        public static void SetActiveLanguageCode(int id)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update LANGUAGE set ACTIVE = 0", dbCon);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("update LANGUAGE set ACTIVE = 1 where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        public static List<Zodiak> SwappingZodiakList(List<Zodiak> zList, int id)
        {
            List<Zodiak> newList = new List<Zodiak>();
            newList.AddRange(zList.Where(s => s.Id >= id).ToList());
            newList.AddRange(zList.Where(s => s.Id < id).ToList());
            return newList;
        }

        private static List<DateTime> GetDatesFromJogaList(List<Calendar> jcList, DateTime currentDate)
        {
            List<DateTime> dateList = new List<DateTime>();
            foreach (YogaCalendar jc in jcList)
            {
                if (jc.DateStart < currentDate)
                    dateList.Add(currentDate);
                if (jc.DateStart.Between(currentDate, currentDate.AddDays(+1)))
                    dateList.Add(jc.DateStart);
                if (jc.DateEnd.Between(currentDate, currentDate.AddDays(+1)))
                    dateList.Add(jc.DateEnd);
                if (jc.DateEnd > currentDate.AddDays(+1))
                    dateList.Add(jc.DateEnd);
            }
            return dateList;
        }

        public static List<YogaColoredBlock> GetYogaColoredBlockListForDay(Day dayObj)
        {
            List<DateTime> djDateList = new List<DateTime>();
            if (dayObj.DwipushkarJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.DwipushkarJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.DwipushkarJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.TripushkarJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.TripushkarJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.TripushkarJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.AmritaSiddhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.AmritaSiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.AmritaSiddhaJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.SarvarthaSiddhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.SarvarthaSiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.SarvarthaSiddhaJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.SiddhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.SiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.SiddhaJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.MrityuJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.MrityuJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.MrityuJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.AdhamJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.AdhamJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.AdhamJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.YamaghataJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.YamaghataJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.YamaghataJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.DagdhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.DagdhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.DagdhaJogaDayList, dayObj.Date));
                }
            }
            if (dayObj.UnfarobaleJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.UnfarobaleJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        djDateList.AddRange(GetDatesFromJogaList(dayObj.UnfarobaleJogaDayList, dayObj.Date));
                }
            }

            List<YogaColoredBlock> jcbList = new List<YogaColoredBlock>();
            if (djDateList.Count > 0)
            {
                List<DateTime> djDateListUnique = djDateList.Distinct().ToList();
                djDateListUnique.Sort();
                for (int i = 0; i < djDateListUnique.Count; i++)
                {
                    if (i < djDateListUnique.Count - 1)
                    {
                        YogaColoredBlock temp = new YogaColoredBlock
                        {
                            Date = dayObj.Date,
                            DateStart = djDateListUnique[i],
                            DateEnd = djDateListUnique[i + 1],
                            ColorCode = GetColorFromDayJoga(djDateListUnique[i], djDateListUnique[i + 1], dayObj)
                        };
                        jcbList.Add(temp);
                    }
                }
            }
            return jcbList;
        }

        public static EExaltation GetExaltationByPlanetAndZnak(EPlanet planet, EZodiak zodiak)
        {
            // ↑↓
            // up = "↑", down = "↓";
            // up = EXALTATION, down = DEBILITATION
            EExaltation exaltation = EExaltation.NOEXALTATION;
            switch (planet)
            {
                case EPlanet.MOON:
                    if (zodiak == EZodiak.TAU)
                        exaltation = EExaltation.EXALTATION;
                    if (zodiak == EZodiak.SCO)
                        exaltation = EExaltation.DEBILITATION;
                    break;

                case EPlanet.SUN:
                    if (zodiak == EZodiak.ARI)
                        exaltation = EExaltation.EXALTATION;
                    if (zodiak == EZodiak.LIB)
                        exaltation = EExaltation.DEBILITATION;
                    break;

                case EPlanet.VENUS:
                    if (zodiak == EZodiak.PSC)
                        exaltation = EExaltation.EXALTATION;
                    if (zodiak == EZodiak.VIR)
                        exaltation = EExaltation.DEBILITATION;
                    break;

                case EPlanet.JUPITER:
                    if (zodiak == EZodiak.CNC)
                        exaltation = EExaltation.EXALTATION;
                    if (zodiak == EZodiak.CAP)
                        exaltation = EExaltation.DEBILITATION;
                    break;

                case EPlanet.MERCURY:
                    if (zodiak == EZodiak.VIR)
                        exaltation = EExaltation.EXALTATION;
                    if (zodiak == EZodiak.PSC)
                        exaltation = EExaltation.DEBILITATION;
                    break;

                case EPlanet.MARS:
                    if (zodiak == EZodiak.CAP)
                        exaltation = EExaltation.EXALTATION;
                    if (zodiak == EZodiak.CNC)
                        exaltation = EExaltation.DEBILITATION;
                    break;

                case EPlanet.SATURN:
                    if (zodiak == EZodiak.LIB)
                        exaltation = EExaltation.EXALTATION;
                    if (zodiak == EZodiak.ARI)
                        exaltation = EExaltation.DEBILITATION;
                    break;

                default:
                    exaltation = EExaltation.NOEXALTATION;
                    break;
            }
            return exaltation;
        }

        private static EColor GetColorFromDayJoga(DateTime startDate, DateTime endDate, Day dayObj)
        {
            EColor color = EColor.NOCOLOR;
            if (dayObj.DwipushkarJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.DwipushkarJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.DwipushkarJogaDayList, color);
                }
            }
            if (dayObj.TripushkarJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.TripushkarJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.TripushkarJogaDayList, color);
                }
            }
            if (dayObj.AmritaSiddhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.AmritaSiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.AmritaSiddhaJogaDayList, color);
                }
            }
            if (dayObj.SarvarthaSiddhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.SarvarthaSiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.SarvarthaSiddhaJogaDayList, color);
                }
            }
            if (dayObj.SiddhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.SiddhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.SiddhaJogaDayList, color);
                }
            }
            if (dayObj.MrityuJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.MrityuJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.MrityuJogaDayList, color);
                }
            }
            if (dayObj.AdhamJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.AdhamJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.AdhamJogaDayList, color);
                }
            }
            if (dayObj.YamaghataJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.YamaghataJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.YamaghataJogaDayList, color);
                }
            }
            if (dayObj.DagdhaJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.DagdhaJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.DagdhaJogaDayList, color);
                }
            }
            if (dayObj.UnfarobaleJogaDayList.Count > 0)
            {
                foreach (YogaCalendar jc in dayObj.UnfarobaleJogaDayList)
                {
                    if (jc.DateStart.Between(dayObj.Date, dayObj.Date.AddDays(+1)) || jc.DateEnd.Between(dayObj.Date, dayObj.Date.AddDays(+1)))
                        color = GetJogaColorCode(startDate, endDate, dayObj.UnfarobaleJogaDayList, color);
                }
            }
            return color;
        }

        private static EColor GetJogaColorCode(DateTime startDate, DateTime endDate, List<Calendar> jcList, EColor currentColor)
        {
            foreach (Calendar jc in jcList)
            {
                if (startDate >= jc.DateStart && endDate <= jc.DateEnd)
                {
                    if (currentColor == EColor.NOCOLOR || jc.ColorCode == currentColor)
                        return jc.ColorCode;
                    else
                        return EColor.LIGHTGREEN;
                }
            }
            return currentColor;
        }

        public static List<YogaCalendar> GetYogasListForTimePeriod(Day dayObj, DateTime startDate, DateTime endDate)
        {
            List<YogaCalendar> composedList = new List<YogaCalendar>();

            if (dayObj.DwipushkarJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.DwipushkarJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.TripushkarJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.TripushkarJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.AmritaSiddhaJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.AmritaSiddhaJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.SarvarthaSiddhaJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.SarvarthaSiddhaJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.SiddhaJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.SiddhaJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.MrityuJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.MrityuJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.AdhamJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.AdhamJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.YamaghataJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.YamaghataJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.DagdhaJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.DagdhaJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            if (dayObj.UnfarobaleJogaDayList.Count > 0)
            {
                List<Calendar> selList = dayObj.UnfarobaleJogaDayList.Where(i => i.DateStart <= startDate && i.DateEnd >= endDate).ToList();
                List<YogaCalendar> clonedList = CloneYogaCalendarList(selList);
                composedList.AddRange(clonedList);
            }
            List<YogaCalendar> yogaListSorted = composedList.OrderBy(i => i.DateStart).ToList();
            return yogaListSorted;
        }

        public static Color GetColorByColorCode(EColor cCode)
        {
            return Color.FromArgb(CacheLoad._colorList.Where(i => i.Id == (int)cCode).FirstOrDefault().ARGBValue);
        }

        public static EAppSetting GetActiveLanguageCode(List<AppSettingList> appSetList)
        {
            return (EAppSetting)(appSetList.Where(i => i.GroupCode.Equals(EAppSettingList.LANGUAGE.ToString()) && i.Active == 1).FirstOrDefault()?.Id ?? 1);
        }

        public static string GetActiveCultureCode(ELanguage langCode)
        {
            return CacheLoad._languageList.Where(i => i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.CultureCode ?? string.Empty;
        }
        
        public static EAppSetting GetActiveTranzitMode(List<AppSettingList> appSetList)
        {
            return (EAppSetting)(appSetList.Where(i => i.GroupCode.Equals(EAppSettingList.TRANZIT.ToString()) && i.Active == 1).FirstOrDefault()?.Id ?? 3);
        }

        public static EAppSetting GetActiveHoraMode(List<AppSettingList> appSetList)
        {
            return (EAppSetting)(appSetList.Where(i => i.GroupCode.Equals(EAppSettingList.HORA.ToString()) && i.Active == 1).FirstOrDefault()?.Id ?? 7);
        }

        public static EAppSetting GetActiveMuhurtaGhatiMode(List<AppSettingList> appSetList)
        {
            return (EAppSetting)(appSetList.Where(i => i.GroupCode.Equals(EAppSettingList.MUHURTAGHATI.ToString()) && i.Active == 1).FirstOrDefault()?.Id ?? 10);
        }

        public static EAppSetting GetActiveMrityuBhagaMode(List<AppSettingList> appSetList)
        {
            return (EAppSetting)(appSetList.Where(i => i.GroupCode.Equals(EAppSettingList.MRITYUBHAGA.ToString()) && i.Active == 1).FirstOrDefault()?.Id ?? 12);
        }

        public static EAppSetting GetActiveNodeMode(List<AppSettingList> appSetList)
        {
            return (EAppSetting)(appSetList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault()?.Id ?? 15);
        }

        public static EAppSetting GetActiveWeekMode(List<AppSettingList> appSetList)
        {
            return (EAppSetting)(appSetList.Where(i => i.GroupCode.Equals(EAppSettingList.WEEK.ToString()) && i.Active == 1).FirstOrDefault()?.Id ?? 17);
        }

        public static string GetLocalizedText(string nativeText, ELanguage langCode)
        {
            return CacheLoad._appTextsList.Where(i => i.NativeText.Equals(nativeText) && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.ForeignText ?? nativeText;
        }

        public static void LocalizeControl(Control c, ELanguage langCode)
        {
            if (c is ContextMenuStrip)
            {
                foreach (ToolStripItem tsi in ((ContextMenuStrip)c).Items)
                {
                    if (tsi is ToolStripMenuItem)
                    {
                        LocalizeMenuLabel(tsi, langCode);
                    }
                }
            }
        }

        public static void LocalizeForm(Form form, ELanguage langCode)
        {
            form.Text = GetLocalizedText(form.Text, langCode);
            foreach (Control control in form.Controls)
            {
                if (control is TabControl)
                {
                    TabControl tc = control as TabControl;
                    if (tc == null)
                    {
                        return;
                    }
                    foreach (TabPage tp in tc.TabPages)
                    {
                        tp.Text = GetLocalizedText(tp.Text, langCode);
                        for (int i = 0; i < tp.Controls.Count; i++)
                        {
                            tp.Controls[i].Text = GetLocalizedText(tp.Controls[i].Text, langCode);
                        }
                    }
                }
                else if (control is MenuStrip)
                {
                    foreach (ToolStripItem tsi in ((MenuStrip)control).Items)
                    {
                        if (tsi is ToolStripMenuItem)
                        {
                            LocalizeMenuLabel(tsi, langCode);
                        }
                        if (tsi is ToolStripComboBox)
                        {
                            ToolStripComboBox tscb = tsi as ToolStripComboBox;
                            if (tscb == null)
                            {
                                return;
                            }
                            for (int i = 0; i < tscb.Items.Count; i++)
                            {
                                if (tscb.Items[i] is KeyValueData)
                                {
                                    ((KeyValueData)tscb.Items[i]).Text = GetLocalizedText(((KeyValueData)tscb.Items[i]).Text, langCode);
                                }
                                else
                                {
                                    tscb.Items[i] = GetLocalizedText(tscb.Items[i].ToString(), langCode);
                                }
                            }
                        }
                    }
                }
                else if (control is ToolStrip)
                {
                    foreach (ToolStripItem tsi in ((ToolStrip)control).Items)
                    {
                        if (tsi is ToolStripButton)
                        {
                            LocalizeMenuLabel(tsi, langCode);
                        }
                    }
                }
                else if (control is GroupBox)
                {
                    LocalizeGroupBox((GroupBox)control, langCode);
                }
                else if (control is Label || control is Button)
                {
                    control.Text = GetLocalizedText(control.Text, langCode);
                }
                else
                {
                    control.Text = string.Empty;
                }
            }
        }

        private static void LocalizeGroupBox(GroupBox gbObj, ELanguage languageCode)
        {
            if (!gbObj.Text.Equals(string.Empty))
            {
                gbObj.Text = GetLocalizedText(gbObj.Text, languageCode);
            }
            if (gbObj.HasChildren)
            {
                Control.ControlCollection controls = gbObj.Controls;
                foreach (Control c in controls)
                {
                    if (c is GroupBox)
                    {
                        LocalizeGroupBox((GroupBox)c, languageCode);
                    }
                    if (c is RadioButton || c is Label || c is CheckBox)
                    {
                        c.Text = GetLocalizedText(c.Text, languageCode);
                    }
                }
            }
        }

        private static void LocalizeMenuLabel(ToolStripItem tsi, ELanguage languageCode)
        {
            tsi.Text = GetLocalizedText(tsi.Text, languageCode);
            ToolStripMenuItem tsmi = tsi as ToolStripMenuItem;
            if (tsmi is null)
            {
                return;
            }
            foreach (ToolStripItem tsmiInner in tsmi.DropDownItems)
            {
                LocalizeMenuLabel(tsmiInner, languageCode);
            }
        }

        public static string GetDaysOfWeekName(int day, ELanguage currentLang, EAppSetting weekSetting)
        {
            if (weekSetting == EAppSetting.WEEKMONDAY)
            {
                if (day == 0)
                {
                    day = 1;
                }
                else if (day == 6)
                {
                    day = 0;
                }
                else
                {
                    day += 1;
                }
            }
            string culture = CacheLoad._languageList.Where(i => i.LanguageCode.Equals(currentLang.ToString())).FirstOrDefault().CultureCode;
            return CultureInfo.GetCultureInfo(culture).DateTimeFormat.DayNames[day];
        }

        public static string GetDaysOfWeekName(int day, ELanguage currentLang)
        {
            string culture = CacheLoad._languageList.Where(i => i.LanguageCode.Equals(currentLang.ToString())).FirstOrDefault().CultureCode;
            return CultureInfo.GetCultureInfo(culture).DateTimeFormat.DayNames[day];
        }

        public static int GetDayOfWeekNumberFromDate(DateTime date, EAppSetting weekSetting)
        {
            switch (weekSetting)
            {
                case EAppSetting.WEEKSUNDAY:
                    return (int)date.DayOfWeek;

                case EAppSetting.WEEKMONDAY:
                {
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                        return 6;
                    else
                        return (int)date.DayOfWeek - 1;
                }

                default:
                    return (int)date.DayOfWeek;
            }
        }

        public static DateTime GetAdjustmentDate(TimeZoneInfo.TransitionTime transitionTime, int year)
        {
            if (transitionTime.IsFixedDateRule)
            {
                return new DateTime(year, transitionTime.Month, transitionTime.Day);
            }
            else
            {
                // For non-fixed date rules, get local calendar
                System.Globalization.Calendar cal = CultureInfo.CurrentCulture.Calendar;
                // Get first day of week for transition
                // For example, the 3rd week starts no earlier than the 15th of the month
                int startOfWeek = transitionTime.Week * 7 - 6;
                // What day of the week does the month start on?
                int firstDayOfWeek = (int)cal.GetDayOfWeek(new DateTime(year, transitionTime.Month, 1));
                // Determine how much start date has to be adjusted
                int transitionDay;
                int changeDayOfWeek = (int)transitionTime.DayOfWeek;

                if (firstDayOfWeek <= changeDayOfWeek)
                    transitionDay = startOfWeek + (changeDayOfWeek - firstDayOfWeek);
                else
                    transitionDay = startOfWeek + (7 - firstDayOfWeek + changeDayOfWeek);

                // Adjust for months with no fifth week
                if (transitionDay > cal.GetDaysInMonth(year, transitionTime.Month))
                    transitionDay -= 7;

                return new DateTime(year, transitionTime.Month, transitionDay, transitionTime.TimeOfDay.Hour, transitionTime.TimeOfDay.Minute, transitionTime.TimeOfDay.Second);
            }
        }

        public static DateTime ShiftDateByDaylightDelta(DateTime date, TimeZoneInfo.AdjustmentRule[] adjustmentRules)
        {
            DateTime newDate = date;
            TimeZoneInfo.TransitionTime daylightStart = new TimeZoneInfo.TransitionTime(), daylightEnd = new TimeZoneInfo.TransitionTime();
            TimeSpan daylightDelta = new TimeSpan(0);
            foreach (var adjustmentRule in adjustmentRules)
            {
                if (date >= adjustmentRule.DateStart && date <= adjustmentRule.DateEnd)
                {
                    daylightStart = adjustmentRule.DaylightTransitionStart;
                    daylightEnd = adjustmentRule.DaylightTransitionEnd;
                    daylightDelta = adjustmentRule.DaylightDelta;
                }
            }
            if (daylightStart.Day != 0 && daylightStart.Month != 0 && daylightStart.Week != 0)
            {
                if (date >= GetAdjustmentDate(daylightStart, date.Year) && date <= GetAdjustmentDate(daylightEnd, date.Year))
                    newDate = newDate.Add(daylightDelta);
            }
            return newDate;
        }

        public static DateTime ShiftDateBackByDaylightDelta(DateTime date, TimeZoneInfo.AdjustmentRule[] adjustmentRules)
        {
            DateTime newDate = date;
            TimeZoneInfo.TransitionTime daylightStart = new TimeZoneInfo.TransitionTime(), daylightEnd = new TimeZoneInfo.TransitionTime();
            TimeSpan daylightDelta = new TimeSpan(0);
            foreach (var adjustmentRule in adjustmentRules)
            {
                if (date >= adjustmentRule.DateStart && date <= adjustmentRule.DateEnd)
                {
                    daylightStart = adjustmentRule.DaylightTransitionStart;
                    daylightEnd = adjustmentRule.DaylightTransitionEnd;
                    daylightDelta = adjustmentRule.DaylightDelta;
                }
            }
            if (daylightStart.Day != 0 && daylightStart.Month != 0 && daylightStart.Week != 0)
            {
                if (date >= GetAdjustmentDate(daylightStart, date.Year) && date <= GetAdjustmentDate(daylightEnd, date.Year))
                    newDate = newDate.Add(-daylightDelta);
            }
            return newDate;
        }

        public static DateTime? CalculateSunriseForDateAndLocation(DateTime date, double latitude, double longitude, string timeZoneId)
        {
            DateTime? sunrise = null;
            SolarTimes solarTimes = new SolarTimes(date, latitude, longitude);
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            sunrise = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunrise.ToUniversalTime(), tzi);
            return sunrise;
        }

        public static DateTime? CalculateSunsetForDateAndLocation(DateTime date, double latitude, double longitude, string timeZoneId)
        {
            DateTime? sunset = null;
            SolarTimes solarTimes = new SolarTimes(date, latitude, longitude);
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            sunset = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunset.ToUniversalTime(), tzi);
            return sunset;
        }

        public static string GetSunStatusName(ESun sMode, ELanguage currentLang)
        {
            string name = string.Empty;
            switch (sMode)
            {
                case ESun.SUNRISE:
                    if (currentLang == ELanguage.en)
                        name = "Sr: ";
                    if (currentLang == ELanguage.ru)
                        name = "Вс: ";
                    break;

                case ESun.SUNSET:
                    if (currentLang == ELanguage.en)
                        name = "Ss: ";
                    if (currentLang == ELanguage.ru)
                        name = "Зх: ";
                    break;
            }
            return name;
        }

        public static List<VedhaEntity> PrepareVedhaPlanetList(Day day, PlanetCalendar targetPCItem, int vedhaDom, bool isLagna)
        {
            List<VedhaEntity> vList = new List<VedhaEntity>();

            //Check Moon
            if (targetPCItem.PlanetCode != EPlanet.MOON)
            {
                List<PlanetCalendar> clonedList = null; 
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.MoonZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.MoonZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.MOON))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.MOON,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check Sun
            if (targetPCItem.PlanetCode != EPlanet.SUN)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.SunZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.SunZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.SUN))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.SUN,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check Venus
            if (targetPCItem.PlanetCode != EPlanet.VENUS)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.VenusZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.VenusZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.VENUS))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.VENUS,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check Jupiter
            if (targetPCItem.PlanetCode != EPlanet.JUPITER)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.JupiterZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.JupiterZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.JUPITER))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.JUPITER,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check Mercury
            if (targetPCItem.PlanetCode != EPlanet.MERCURY)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.MercuryZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.MercuryZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.MERCURY))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.MERCURY,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check Mars
            if (targetPCItem.PlanetCode != EPlanet.MARS)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.MarsZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.MarsZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.MARS))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.MARS,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check Saturn
            if (targetPCItem.PlanetCode != EPlanet.SATURN)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.SaturnZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.SaturnZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.SATURN))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.SATURN,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check RahuMean
            if (targetPCItem.PlanetCode != EPlanet.RAHUMEAN)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.RahuMeanZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.RahuMeanZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.RAHUMEAN))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.RAHUMEAN,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check RahuTrue
            if (targetPCItem.PlanetCode != EPlanet.RAHUTRUE)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.RahuTrueZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.RahuTrueZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.RAHUTRUE))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.RAHUTRUE,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check KetuMean
            if (targetPCItem.PlanetCode != EPlanet.KETUMEAN)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.KetuMeanZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.KetuMeanZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.KETUMEAN))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.KETUMEAN,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            //Check KetuTrue
            if (targetPCItem.PlanetCode != EPlanet.KETUTRUE)
            {
                List<PlanetCalendar> clonedList = null;
                PlanetCalendar pcItem = null;
                if (!isLagna)
                {
                    clonedList = ClonePlanetCalendarList(day.KetuTrueZodiakDayList);
                }
                else
                {
                    clonedList = ClonePlanetCalendarList(day.KetuTrueZodiakLagnaDayList);
                }
                pcItem = clonedList.Where(i => i.Dom == vedhaDom).FirstOrDefault();

                if (pcItem != null)
                {
                    if (CheckIfVedhaExistforPlanetCalendarTimeFrame(targetPCItem, EPlanet.KETUTRUE))
                    {
                        DateTimeInterval targetPCItemInterval = new DateTimeInterval { From = targetPCItem.DateStart, To = targetPCItem.DateEnd };
                        DateTimeInterval pcItemInterval = new DateTimeInterval { From = pcItem.DateStart, To = pcItem.DateEnd };
                        DateTimeInterval intersection = DateTimeUtils.GetIntervalIntersection(targetPCItemInterval, pcItemInterval);
                        if (intersection != null)
                        {
                            VedhaEntity temp = new VedhaEntity
                            {
                                PlanetCode = EPlanet.KETUTRUE,
                                DateStart = intersection.From,
                                DateEnd = intersection.To
                            };
                            vList.Add(temp);
                        }
                    }
                }
            }

            return vList;
        }

        public static bool CheckIfVedhaExistforPlanetCalendarTimeFrame(PlanetCalendar pc, EPlanet vedhaPlanet)
        {
            if (pc.PlanetCode == EPlanet.SUN && vedhaPlanet == EPlanet.SATURN)
            {
                return false;
            }
            if (pc.PlanetCode == EPlanet.SATURN && vedhaPlanet == EPlanet.SUN)
            {
                return false;
            }
            if (pc.PlanetCode == EPlanet.MOON && vedhaPlanet == EPlanet.MERCURY)
            {
                return false;
            }
            if (pc.PlanetCode == EPlanet.MERCURY && vedhaPlanet == EPlanet.MOON)
            {
                return false;
            }
            return true;
        }

        public static List<PlanetCalendar> ClonePlanetCalendarList(List<PlanetCalendar> pcList)
        {
            List<PlanetCalendar> newPCList = new List<PlanetCalendar>();
            foreach (PlanetCalendar pc in pcList)
            {
                PlanetCalendar pcObj = new PlanetCalendar
                {
                    DateStart = pc.DateStart,
                    DateEnd = pc.DateEnd,
                    ColorCode = pc.ColorCode,
                    PlanetCode = pc.PlanetCode,
                    Dom = pc.Dom,
                    Retro = pc.Retro,
                    ZodiakCode = pc.ZodiakCode,
                    NakshatraCode = pc.NakshatraCode,
                    PadaId = pc.PadaId,
                    LagnaDom = pc.LagnaDom,
                    TaraBalaId = pc.TaraBalaId,
                    TaraBalaPercent = pc.TaraBalaPercent
                };
                newPCList.Add(pcObj);
            }
            return newPCList;
        }

        public static List<PlanetCalendar> ClonePlanetCalendarList(List<Calendar> pcList)
        {
            List<PlanetCalendar> newPCList = new List<PlanetCalendar>();
            foreach (PlanetCalendar pc in pcList)
            {
                PlanetCalendar pcObj = new PlanetCalendar
                {
                    DateStart = pc.DateStart,
                    DateEnd = pc.DateEnd,
                    ColorCode = pc.ColorCode,
                    PlanetCode = pc.PlanetCode,
                    Dom = pc.Dom,
                    Retro = pc.Retro,
                    ZodiakCode = pc.ZodiakCode,
                    NakshatraCode = pc.NakshatraCode,
                    PadaId = pc.PadaId,
                    LagnaDom = pc.LagnaDom,
                    TaraBalaId = pc.TaraBalaId,
                    TaraBalaPercent = pc.TaraBalaPercent
                };
                newPCList.Add(pcObj);
            }
            return newPCList;
        }

        public static List<NakshatraCalendar> CloneNakshatraCalendarList(List<NakshatraCalendar> ncList)
        {
            List<NakshatraCalendar> newNCList = new List<NakshatraCalendar>();
            foreach (NakshatraCalendar nc in ncList)
            {
                NakshatraCalendar ncObj = new NakshatraCalendar
                {
                    DateStart = nc.DateStart,
                    DateEnd = nc.DateEnd,
                    ColorCode = nc.ColorCode,
                    NakshatraCode = nc.NakshatraCode
                };
                newNCList.Add(ncObj);
            }
            return newNCList;
        }

        public static List<NakshatraCalendar> CloneNakshatraCalendarList(List<Calendar> ncList)
        {
            List<NakshatraCalendar> newNCList = new List<NakshatraCalendar>();
            foreach (NakshatraCalendar nc in ncList)
            {
                NakshatraCalendar ncObj = new NakshatraCalendar
                {
                    DateStart = nc.DateStart,
                    DateEnd = nc.DateEnd,
                    ColorCode = nc.ColorCode,
                    NakshatraCode = nc.NakshatraCode
                };
                newNCList.Add(ncObj);
            }
            return newNCList;
        }

        public static List<TaraBalaCalendar> CloneTaraBalaCalendarList(List<Calendar> tbList)
        {
            List<TaraBalaCalendar> newTBList = new List<TaraBalaCalendar>();
            foreach (TaraBalaCalendar tbc in tbList)
            {
                TaraBalaCalendar tbObj = new TaraBalaCalendar
                {
                    DateStart = tbc.DateStart,
                    DateEnd = tbc.DateEnd,
                    ColorCode = tbc.ColorCode,
                    NakshatraCode = tbc.NakshatraCode,
                    TaraBalaId = tbc.TaraBalaId,
                    Percent = tbc.Percent
                };
                newTBList.Add(tbObj);
            }
            return newTBList;
        }

        public static List<TithiCalendar> CloneTithiCalendarList(List<TithiCalendar> tcList)
        {
            List<TithiCalendar> newTCList = new List<TithiCalendar>();
            foreach (TithiCalendar tc in tcList)
            {
                TithiCalendar tcObj = new TithiCalendar
                {
                    DateStart = tc.DateStart,
                    DateEnd = tc.DateEnd,
                    ColorCode = tc.ColorCode,
                    TithiId = tc.TithiId
                };
                newTCList.Add(tcObj);
            }
            return newTCList;
        }

        public static List<TithiCalendar> CloneTithiCalendarList(List<Calendar> tcList)
        {
            List<TithiCalendar> newTCList = new List<TithiCalendar>();
            foreach (TithiCalendar tc in tcList)
            {
                TithiCalendar tcObj = new TithiCalendar
                {
                    DateStart = tc.DateStart,
                    DateEnd = tc.DateEnd,
                    ColorCode = tc.ColorCode,
                    TithiId = tc.TithiId
                };
                newTCList.Add(tcObj);
            }
            return newTCList;
        }

        public static List<KaranaCalendar> CloneKaranaCalendarList(List<KaranaCalendar> kList)
        {
            List<KaranaCalendar> newKList = new List<KaranaCalendar>();
            foreach (KaranaCalendar k in kList)
            {
                KaranaCalendar kObj = new KaranaCalendar
                {
                    DateStart = k.DateStart,
                    DateEnd = k.DateEnd,
                    ColorCode = k.ColorCode,
                    TithiId = k.TithiId,
                    KaranaId = k.KaranaId
                };
                newKList.Add(kObj);
            }
            return newKList;
        }

        public static List<KaranaCalendar> CloneKaranaCalendarList(List<Calendar> kList)
        {
            List<KaranaCalendar> newKList = new List<KaranaCalendar>();
            foreach (KaranaCalendar k in kList)
            {
                KaranaCalendar kObj = new KaranaCalendar
                {
                    DateStart = k.DateStart,
                    DateEnd = k.DateEnd,
                    ColorCode = k.ColorCode,
                    TithiId = k.TithiId,
                    KaranaId = k.KaranaId
                };
                newKList.Add(kObj);
            }
            return newKList;
        }

        public static List<NityaYogaCalendar> CloneNityaYogaCalendarList(List<Calendar> njList)
        {
            List<NityaYogaCalendar> newNJList = new List<NityaYogaCalendar>();
            foreach (NityaYogaCalendar nj in njList)
            {
                NityaYogaCalendar njObj = new NityaYogaCalendar
                {
                    DateStart = nj.DateStart,
                    DateEnd = nj.DateEnd,
                    ColorCode = nj.ColorCode,
                    NYCode = nj.NYCode,
                    NakshatraId = nj.NakshatraId
                };
                newNJList.Add(njObj);
            };
            return newNJList;
        }

        public static List<NityaYogaCalendar> CloneNityaJogaCalendarList(List<NityaYogaCalendar> njList)
        {
            List<NityaYogaCalendar> newNJList = new List<NityaYogaCalendar>();
            foreach (NityaYogaCalendar nj in njList)
            {
                NityaYogaCalendar njObj = new NityaYogaCalendar
                {
                    DateStart = nj.DateStart,
                    DateEnd = nj.DateEnd,
                    ColorCode = nj.ColorCode,
                    NYCode = nj.NYCode,
                    NakshatraId = nj.NakshatraId
                };
                newNJList.Add(njObj);
            };
            return newNJList;
        }

        public static List<ChandraBalaCalendar> CloneChandraBalaCalendarList(List<ChandraBalaCalendar> cbList)
        {
            List<ChandraBalaCalendar> newCBList = new List<ChandraBalaCalendar>();
            foreach (ChandraBalaCalendar cb in cbList)
            {
                ChandraBalaCalendar cbObj = new ChandraBalaCalendar
                {
                    DateStart = cb.DateStart,
                    DateEnd = cb.DateEnd,
                    ColorCode = cb.ColorCode,
                    ZodiakCode = cb.ZodiakCode,
                    DomNumber = cb.DomNumber,
                    Dom = cb.Dom
                };
                newCBList.Add(cbObj);
            }
            return newCBList;
        }

        public static List<ChandraBalaCalendar> CloneChandraBalaCalendarList(List<Calendar> cbList)
        {
            List<ChandraBalaCalendar> newCBList = new List<ChandraBalaCalendar>();
            foreach (ChandraBalaCalendar cb in cbList)
            {
                ChandraBalaCalendar cbObj = new ChandraBalaCalendar
                {
                    DateStart = cb.DateStart,
                    DateEnd = cb.DateEnd,
                    ColorCode = cb.ColorCode,
                    ZodiakCode = cb.ZodiakCode,
                    DomNumber = cb.DomNumber,
                    Dom = cb.Dom
                };
                newCBList.Add(cbObj);
            }
            return newCBList;
        }

        public static List<HoraCalendar> CloneHoraCalendarList(List<Calendar> hList)
        {
            List<HoraCalendar> newHList = new List<HoraCalendar>();
            foreach (HoraCalendar hb in hList)
            {
                HoraCalendar hObj = new HoraCalendar
                {
                    DateStart = hb.DateStart,
                    DateEnd = hb.DateEnd,
                    ColorCode = hb.ColorCode,
                    PlanetCode = hb.PlanetCode,
                    IsDayLightHora = hb.IsDayLightHora
                };
                newHList.Add(hObj);
            }
            return newHList;
        }

        public static List<Muhurta30Calendar> CloneMuhurta30CalendarList(List<Calendar> m30List)
        {
            List<Muhurta30Calendar> newM30List = new List<Muhurta30Calendar>();
            foreach (Muhurta30Calendar m30 in m30List)
            {
                Muhurta30Calendar m30Obj = new Muhurta30Calendar
                {
                    DateStart = m30.DateStart,
                    DateEnd = m30.DateEnd,
                    ColorCode = m30.ColorCode,
                    Muhurta30Code = m30.Muhurta30Code,
                    IsDayLightMuhurta = m30.IsDayLightMuhurta
                };
                newM30List.Add(m30Obj);
            }
            return newM30List;
        }

        public static List<Ghati60Calendar> CloneGhati60CalendarList(List<Calendar> g60List)
        {
            List<Ghati60Calendar> newG60List = new List<Ghati60Calendar>();
            foreach (Ghati60Calendar g60 in g60List)
            {
                Ghati60Calendar g60Obj = new Ghati60Calendar
                {
                    DateStart = g60.DateStart,
                    DateEnd = g60.DateEnd,
                    ColorCode = g60.ColorCode,
                    Ghati60Code = g60.Ghati60Code,
                    IsDayLightGhati = g60.IsDayLightGhati
                };
                newG60List.Add(g60Obj);
            }
            return newG60List;
        }

        public static List<EclipseCalendar> CloneEclipseCalendarList(List<EclipseCalendar> eList)
        {
            List<EclipseCalendar> newEList = new List<EclipseCalendar>();
            foreach (EclipseCalendar e in eList)
            {
                EclipseCalendar eObj = new EclipseCalendar
                {
                    DateStart = e.DateStart,
                    DateEnd = e.DateEnd,
                    ColorCode = e.ColorCode,
                    EclipseCode = e.EclipseCode
                };
                newEList.Add(eObj);
            }
            return newEList;
        }

        public static List<MasaCalendar> CloneMasaCalendarList(List<MasaCalendar> mList)
        {
            List<MasaCalendar> newEList = new List<MasaCalendar>();
            foreach (MasaCalendar mc in mList)
            {
                MasaCalendar eObj = new MasaCalendar
                {
                    DateStart = mc.DateStart,
                    DateEnd = mc.DateEnd,
                    ColorCode = mc.ColorCode,
                    MasaId = mc.MasaId,
                    FullMoonDate = mc.FullMoonDate
                };
                newEList.Add(eObj);
            }
            return newEList;
        }

        public static List<MasaCalendar> CloneMasaCalendarList(List<Calendar> mList)
        {
            List<MasaCalendar> newEList = new List<MasaCalendar>();
            foreach (MasaCalendar mc in mList)
            {
                MasaCalendar eObj = new MasaCalendar
                {
                    DateStart = mc.DateStart,
                    DateEnd = mc.DateEnd,
                    ColorCode = mc.ColorCode,
                    MasaId = mc.MasaId,
                    FullMoonDate = mc.FullMoonDate
                };
                newEList.Add(eObj);
            }
            return newEList;
        }

        public static List<ShunyaNakshatraCalendar> CloneShunyaNakshatraCalendarList(List<ShunyaNakshatraCalendar> snList)
        {
            List<ShunyaNakshatraCalendar> newEList = new List<ShunyaNakshatraCalendar>();
            foreach (ShunyaNakshatraCalendar mc in snList)
            {
                ShunyaNakshatraCalendar eObj = new ShunyaNakshatraCalendar
                {
                    DateStart = mc.DateStart,
                    DateEnd = mc.DateEnd,
                    ColorCode = mc.ColorCode,
                    MasaId = mc.MasaId,
                    NakshatraCode = mc.NakshatraCode,
                    ShunyaCode = mc.ShunyaCode
                };
                newEList.Add(eObj);
            }
            return newEList;
        }

        public static List<ShunyaNakshatraCalendar> CloneShunyaNakshatraCalendarList(List<Calendar> snList)
        {
            List<ShunyaNakshatraCalendar> newEList = new List<ShunyaNakshatraCalendar>();
            foreach (ShunyaNakshatraCalendar mc in snList)
            {
                ShunyaNakshatraCalendar eObj = new ShunyaNakshatraCalendar
                {
                    DateStart = mc.DateStart,
                    DateEnd = mc.DateEnd,
                    ColorCode = mc.ColorCode,
                    MasaId = mc.MasaId,
                    NakshatraCode = mc.NakshatraCode,
                    ShunyaCode = mc.ShunyaCode
                };
                newEList.Add(eObj);
            }
            return newEList;
        }

        public static List<ShunyaTithiCalendar> CloneShunyaTithiCalendarList(List<ShunyaTithiCalendar> stList)
        {
            List<ShunyaTithiCalendar> newEList = new List<ShunyaTithiCalendar>();
            foreach (ShunyaTithiCalendar mc in stList)
            {
                ShunyaTithiCalendar eObj = new ShunyaTithiCalendar
                {
                    DateStart = mc.DateStart,
                    DateEnd = mc.DateEnd,
                    ColorCode = mc.ColorCode,
                    MasaId = mc.MasaId,
                    TithiId = mc.TithiId,
                    ShunyaCode = mc.ShunyaCode
                };
                newEList.Add(eObj);
            }
            return newEList;
        }

        public static List<ShunyaTithiCalendar> CloneShunyaTithiCalendarList(List<Calendar> stList)
        {
            List<ShunyaTithiCalendar> newEList = new List<ShunyaTithiCalendar>();
            foreach (ShunyaTithiCalendar mc in stList)
            {
                ShunyaTithiCalendar eObj = new ShunyaTithiCalendar
                {
                    DateStart = mc.DateStart,
                    DateEnd = mc.DateEnd,
                    ColorCode = mc.ColorCode,
                    MasaId = mc.MasaId,
                    TithiId = mc.TithiId,
                    ShunyaCode = mc.ShunyaCode
                };
                newEList.Add(eObj);
            }
            return newEList;
        }

        public static List<MuhurtaCalendar> CloneMuhurtaCalendarList(List<Calendar> mList)
        {
            List<MuhurtaCalendar> newMList = new List<MuhurtaCalendar>();
            foreach (MuhurtaCalendar mc in mList)
            {
                MuhurtaCalendar mcObj = new MuhurtaCalendar
                {
                    DateStart = mc.DateStart,
                    DateEnd = mc.DateEnd,
                    ColorCode = mc.ColorCode,
                    MuhurtaCode = mc.MuhurtaCode,
                    OverlapedMuhurtaCode = mc.OverlapedMuhurtaCode
                };
                newMList.Add(mcObj);
            }
            return newMList;
        }

        public static List<YogaCalendar> CloneYogaCalendarList(List<Calendar> yList)
        {
            List<YogaCalendar> newYList = new List<YogaCalendar>();
            foreach (YogaCalendar yc in yList)
            {
                YogaCalendar yObj = new YogaCalendar
                {
                    DateStart = yc.DateStart,
                    DateEnd = yc.DateEnd,
                    ColorCode = yc.ColorCode,
                    Type = yc.Type,
                    YogaCode = yc.YogaCode,
                    Vara = yc.Vara,
                    NakshatraCode = yc.NakshatraCode,
                    TithiId = yc.TithiId
                };
                newYList.Add(yObj);
            }
            return newYList;
        }

        public static string GetPlanetNameByCode(EPlanet pCode, ELanguage lCode)
        {
            return CacheLoad._planetDescList.Where(i => i.PlanetId == (int)pCode && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public static int GetRahuKetuPlanetTranzitIdByCode(EPlanet pCode)
        {
            int id = 0;
            switch (pCode)
            {
                case EPlanet.RAHUMEAN:
                    id = 8;
                    break;
                case EPlanet.KETUMEAN:
                    id = 9;
                    break;
                case EPlanet.RAHUTRUE:
                    id = 8;
                    break;
                case EPlanet.KETUTRUE:
                    id = 9;
                    break;
            }
            return id;
        }

        public static string GetFontNameByCode(EFontList fCode)
        {
            int fontId = CacheLoad._fontList.Where(i => i.Code.Equals(fCode.ToString())).FirstOrDefault().FontId;
            return CacheLoad._systemFontList.Where(i => i.Id == fontId).FirstOrDefault().SystemName;
        }

        public static FontStyle GetFontStyleBySettings(EFontList fCode)
        {
            int styleNumber = CacheLoad._fontList.Where(i => i.Code.Equals(fCode.ToString())).FirstOrDefault().FontStyleId;
            return GetFontStyleById(styleNumber);
        }

        public static FontStyle GetFontStyleById(int number)
        {
            switch (number)
            {
                case 1:
                    return FontStyle.Regular;

                case 2:
                    return FontStyle.Bold;

                case 3:
                    return FontStyle.Italic;
            }
            return FontStyle.Regular;
        }

        public static EColor GetMasaColorById(int id)
        {
            switch (id)
            {
                case 1:
                    return EColor.MASA1;
                case 2:
                    return EColor.MASA2;
                case 3:
                    return EColor.MASA3;
                case 4:
                    return EColor.MASA4;
                case 5:
                    return EColor.MASA5;
                case 6:
                    return EColor.MASA6;
                case 7:
                    return EColor.MASA7;
                case 8:
                    return EColor.MASA8;
                case 9:
                    return EColor.MASA9;
                case 10:
                    return EColor.MASA10;
                case 11:
                    return EColor.MASA11;
                case 12:
                    return EColor.MASA12;
            }
            return EColor.MASA1;
        }

        
        public static int GetNakshatraFullMoonId(MasaCalendar mc, List<PlanetCalendar> pList)
        {
            List<PlanetCalendar> mnPeriodCalendar =pList.Where(i => mc.FullMoonDate.Between(i.DateStart.AddDays(-2), i.DateEnd.AddDays(+2))).ToList();
            return (int)(mnPeriodCalendar.Where(i => mc.FullMoonDate.Between(i.DateStart, i.DateEnd)).FirstOrDefault()?.NakshatraCode ?? 0); 
        }
        

        public static string GetNakshatraUprvitel(int nakId, ELanguage lCode)
        {
            string upravitel = string.Empty;
            string fullUpravitel = CacheLoad._nakshatraDescList.Where(i => i.NakshatraId == nakId && i.LanguageCode.Equals(lCode.ToString())).FirstOrDefault()?.Upravitel ?? string.Empty;
            if (!fullUpravitel.Equals(string.Empty))
            {
                var row = fullUpravitel.Split(new char[] { ',' });
                upravitel = row[0];
            }
            return upravitel;
        }

        public static float CalculateRectangleHeightWithTextWrapping(string wholeText, Font font, int width)
        {
            RectangleF descrRect = new RectangleF();
            Graphics grfx = Graphics.FromImage(new Bitmap(1, 1));
            descrRect.Size = new Size(width, ((int)grfx.MeasureString(wholeText, font, width, StringFormat.GenericDefault).Height));
            return descrRect.Height;
        }

        public static bool GetGeoCoordinateByLocationId(int locId, out double latitude, out double longitude)
        {
            bool isParsedOk = false;
            double lati, longi;
            latitude = 0.0;
            longitude = 0.0;
            using (SQLiteConnection dbCon = GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("select LATITUDE, LONGITUDE from LOCATION where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@ID", locId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (double.TryParse(reader.StringValue(0), NumberStyles.Any, CultureInfo.InvariantCulture, out lati) &&
                                double.TryParse(reader.StringValue(1), NumberStyles.Any, CultureInfo.InvariantCulture, out longi))
                            {
                                latitude = lati;
                                longitude = longi;
                                isParsedOk = true;
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return isParsedOk;
        }

        public static string GetTimeZoneIdByGeoCoordinates(double latitude, double longitude) 
        {
            string tzIana = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
            return TZConvert.IanaToWindows(tzIana);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.GetEncoding(1251).GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.GetEncoding(1251).GetString(base64EncodedBytes);
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static float ConvertHoursToPixels(float width, DateTime date)
        {
            return (date.Hour * 60 + date.Minute) * width / 1440;
        }

        public static int GetZodiakIdFromNakshatraIdandPada(int nakshatraId, int padaNumber)
        {
            return CacheLoad._padaList.Where(i => i.NakshatraId == nakshatraId && i.PadaNumber == padaNumber).FirstOrDefault().ZodiakId;
        }

        public static string GetSpecNavamsha(Pada sPada, ELanguage sLang)
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

        public static string GetBadNavamsha(Profile person, int pId, ELanguage lCode)
        {
            string badNavamsha = string.Empty;
            int[] badNavamshaArray = new int[] { 36, 55, 64, 72, 81, 88, 96 };
            List<Pada> swappedPadaByMoonList = SortingPadaListByBirthMoonOrLagna(CacheLoad._padaList.ToList(), person, false);
            List<Pada> swappedPadaByLagnaList = SortingPadaListByBirthMoonOrLagna(CacheLoad._padaList.ToList(), person, true);

            int indexMoon = 0, indexLagna = 0;
            for (int i = 0; i < badNavamshaArray.Length; i++)
            {
                indexMoon = swappedPadaByMoonList.FindIndex(l => l.Id == pId);
                indexLagna = swappedPadaByLagnaList.FindIndex(l => l.Id == pId);
                if ((indexMoon + 1) == badNavamshaArray[i])
                {
                    badNavamsha += badNavamshaArray[i] + " " + GetLocalizedText("Navamsha from Natal Moon", lCode) + ", ";
                }
                if ((indexLagna + 1) == badNavamshaArray[i])
                {
                    badNavamsha += badNavamshaArray[i] + " " + GetLocalizedText("Navamsha from Lagna", lCode) + ", ";
                }
            }
            return badNavamsha;
        }

        public static List<BadNavamshaEntity> GetBadNavamshaNumbersList(Profile person, int pId, ELanguage lCode)
        {
            List<BadNavamshaEntity> badNavamshaList = new List<BadNavamshaEntity>();
            int[] badNavamshaArray = new int[] { 36, 55, 64, 72, 81, 88, 96 };
            List<Pada> swappedPadaByMoonList = SortingPadaListByBirthMoonOrLagna(CacheLoad._padaList.ToList(), person, false);
            List<Pada> swappedPadaByLagnaList = SortingPadaListByBirthMoonOrLagna(CacheLoad._padaList.ToList(), person, true);

            int indexMoon = 0, indexLagna = 0;
            for (int i = 0; i < badNavamshaArray.Length; i++)
            {
                indexMoon = swappedPadaByMoonList.FindIndex(l => l.Id == pId);
                indexLagna = swappedPadaByLagnaList.FindIndex(l => l.Id == pId);
                if ((indexMoon + 1) == badNavamshaArray[i])
                {
                    badNavamshaList.Add(new BadNavamshaEntity { Navamsha = badNavamshaArray[i], IsLagna = false });
                }
                if ((indexLagna + 1) == badNavamshaArray[i])
                {
                    badNavamshaList.Add(new BadNavamshaEntity { Navamsha = badNavamshaArray[i], IsLagna = true });
                }
            }
            return badNavamshaList;
        }

        public static List<DrekkanaEntity> GetBadDrekkanaList(Profile person, int padaId)
        {
            List<DrekkanaEntity> drekkanaList = new List<DrekkanaEntity>();
            List<Pada> swappedPadaByMoonList = SortingPadaListByBirthMoonOrLagna(CacheLoad._padaList.ToList(), person, false);
            List<Pada> swappedPadaByLagnaList = SortingPadaListByBirthMoonOrLagna(CacheLoad._padaList.ToList(), person, true);
            int birthMoonDrekkana = swappedPadaByMoonList.First().Drekkana;
            int birthLagnaDrekkana = swappedPadaByLagnaList.First().Drekkana;
            int currentMoonDrekkana = 0, currentLagnaDrekkana = 0;
            for (int i = 0; i < swappedPadaByMoonList.Count; i++)
            {
                if ((currentMoonDrekkana + swappedPadaByMoonList[i].Drekkana) == (birthMoonDrekkana + 15) && swappedPadaByMoonList[i].Id == padaId)
                {
                    DrekkanaEntity deTemp = new DrekkanaEntity
                    {
                        Drekkana = 16,
                        NakshatraId = swappedPadaByMoonList[i].NakshatraId,
                        PadaId = swappedPadaByMoonList[i].Id,
                        IsLagna = false
                    };
                    drekkanaList.Add(deTemp);
                }
                if ((currentMoonDrekkana + swappedPadaByMoonList[i].Drekkana) == (birthMoonDrekkana + 21) && swappedPadaByMoonList[i].Id == padaId)
                {
                    DrekkanaEntity deTemp = new DrekkanaEntity
                    {
                        Drekkana = 22,
                        NakshatraId = swappedPadaByMoonList[i].NakshatraId,
                        PadaId = swappedPadaByMoonList[i].Id,
                        IsLagna = false
                    };
                    drekkanaList.Add(deTemp);
                }
                if(i > 2 && swappedPadaByMoonList[i - 2].Drekkana == 36)
                {
                    currentMoonDrekkana = 36;
                }
            }
            for (int i = 0; i < swappedPadaByLagnaList.Count; i++)
            {
                if ((currentLagnaDrekkana + swappedPadaByLagnaList[i].Drekkana) == (birthLagnaDrekkana + 15) && swappedPadaByLagnaList[i].Id == padaId)
                {
                    DrekkanaEntity deTemp = new DrekkanaEntity
                    {
                        Drekkana = 16,
                        NakshatraId = swappedPadaByLagnaList[i].NakshatraId,
                        PadaId = swappedPadaByLagnaList[i].Id,
                        IsLagna = true
                    };
                    drekkanaList.Add(deTemp);
                }
                if ((currentLagnaDrekkana + swappedPadaByLagnaList[i].Drekkana) == (birthLagnaDrekkana + 21) && swappedPadaByLagnaList[i].Id == padaId)
                {
                    DrekkanaEntity deTemp = new DrekkanaEntity
                    {
                        Drekkana = 22,
                        NakshatraId = swappedPadaByLagnaList[i].NakshatraId,
                        PadaId = swappedPadaByLagnaList[i].Id,
                        IsLagna = true
                    };
                    drekkanaList.Add(deTemp);
                }
                if (i > 2 && swappedPadaByLagnaList[i - 2].Drekkana == 36)
                {
                    currentLagnaDrekkana = 36;
                }
            }
            return drekkanaList;
        }

        private static List<Pada> SortingPadaListByBirthMoonOrLagna(List<Pada> pList, Profile person, bool isLagna)
        {
            List<Pada> newList = new List<Pada>();
            int pId = 0;
            if (isLagna)
            {
                pId = pList.Where(i => i.NakshatraId == person.NakshatraLagnaId && i.PadaNumber == person.PadaLagna).FirstOrDefault()?.Id ?? 0;
                newList.AddRange(pList.Where(s => s.Id >= pId).ToList());
                newList.AddRange(pList.Where(s => s.Id < pId).ToList());
            }
            else
            {
                pId = pList.Where(i => i.NakshatraId == person.NakshatraMoonId && i.PadaNumber == person.PadaMoon).FirstOrDefault()?.Id ?? 0;
                newList.AddRange(pList.Where(s => s.Id >= pId).ToList());
                newList.AddRange(pList.Where(s => s.Id < pId).ToList());
            }
            return newList;
        }

        public static string GetNavamshaExaltation(EPlanet pCode, int navamsha)
        {
            string exaltaton = string.Empty;
            switch (pCode)
            {
                case EPlanet.MOON:
                    if (navamsha == 2)
                        exaltaton = "↑";
                    if (navamsha == 8)
                        exaltaton = "↓";
                    break;

                case EPlanet.SUN:
                    if (navamsha == 1)
                        exaltaton = "↑";
                    if (navamsha == 7)
                        exaltaton = "↓";
                    break;

                case EPlanet.VENUS:
                    if (navamsha == 12)
                        exaltaton = "↑";
                    if (navamsha == 6)
                        exaltaton = "↓";
                    break;

                case EPlanet.JUPITER:
                    if (navamsha == 4)
                        exaltaton = "↑";
                    if (navamsha == 10)
                        exaltaton = "↓";
                    break;

                case EPlanet.MERCURY:
                    if (navamsha == 6)
                        exaltaton = "↑";
                    if (navamsha == 12)
                        exaltaton = "↓";
                    break;

                case EPlanet.MARS:
                    if (navamsha == 10)
                        exaltaton = "↑";
                    if (navamsha == 4)
                        exaltaton = "↓";
                    break;

                case EPlanet.SATURN:
                    if (navamsha == 7)
                        exaltaton = "↑";
                    if (navamsha == 1)
                        exaltaton = "↓";
                    break;

                case EPlanet.RAHUMEAN:
                    if (navamsha == 2)
                        exaltaton = "↑";
                    if (navamsha == 8)
                        exaltaton = "↓";
                    break;

                case EPlanet.KETUMEAN:
                    if (navamsha == 8)
                        exaltaton = "↑";
                    if (navamsha == 2)
                        exaltaton = "↓";
                    break;

                case EPlanet.RAHUTRUE:
                    if (navamsha == 2)
                        exaltaton = "↑";
                    if (navamsha == 8)
                        exaltaton = "↓";
                    break;

                case EPlanet.KETUTRUE:
                    if (navamsha == 8)
                        exaltaton = "↑";
                    if (navamsha == 2)
                        exaltaton = "↓";
                    break;
            }
            return exaltaton;
        }

        public static int GetPlanetSWEConstByPlanetId(int planetId)
        {
            switch (planetId)
            {
                case 1:
                    return EpheConstants.SE_MOON;

                case 2:
                    return EpheConstants.SE_SUN;

                case 3:
                    return EpheConstants.SE_VENUS;

                case 4:
                    return EpheConstants.SE_JUPITER;

                case 5:
                    return EpheConstants.SE_MERCURY;

                case 6:
                    return EpheConstants.SE_MARS;

                case 7:
                    return EpheConstants.SE_SATURN;

                case 8:
                    return EpheConstants.SE_MEAN_NODE;

                case 10:
                    return EpheConstants.SE_TRUE_NODE;

                default:
                    return -1;
            }
        }

        public static string GetLocalizedPlanetNameByCode(EPlanet planetCode, ELanguage langCode)
        {
            return CacheLoad._planetDescList.Where(i => i.PlanetId == (int)planetCode && i.LanguageCode.Equals(langCode.ToString())).FirstOrDefault()?.Name ?? string.Empty;
        }

        public static List<PersonsEventsList> GetDayPersonEvents(string guid, DateTime date)
        {
            #region _pevList hack update
            CacheLoad._personEventsList = null;
            CacheLoad._personEventsList = CacheLoad.GetPersonsEventsList();
            #endregion

            return CacheLoad._personEventsList.Where(i => i.DateStart > date && i.DateEnd < date.AddDays(+1) && i.GUID.Equals(guid)).ToList();
        }

        public static List<PersonsEventsList> GetDayEventsList(List<PersonsEventsList> pList, DateTime currentDate)
        {
            return pList.Where(i => i.DateStart > currentDate && i.DateEnd < currentDate.AddDays(+1)).ToList();
        }

        /*
        // Google requires money for using this method ....
        public static string GetTimeZoneIdByGeoCoordinates(double latitude, double longitude, DateTime utcDate)
        {
            RestClient client = new RestClient("https://maps.googleapis.com");
            RestRequest request = new RestRequest("maps/api/timezone/json", Method.GET);
            string location = String.Join(",", latitude.ToString("G", CultureInfo.InvariantCulture), longitude.ToString("G", CultureInfo.InvariantCulture));

            request.AddParameter("location", location);
            request.AddParameter("timestamp", utcDate.ToTimestamp());
            request.AddParameter("sensor", "false");

            var response = client.Execute<GoogleTimeZone>(request);
            if (response.StatusDescription.Equals("OK"))
            {
                return response.Data.TimeZoneId;
            }
            else
            {
                return string.Empty;
            }
        }

        private static double ToTimestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        */
    }
}

