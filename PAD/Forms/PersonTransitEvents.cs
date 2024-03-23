using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PAD
{
    public partial class PersonTransitEvents : Form
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

        private Profile _curProfile;
        private int _curLocationId;
        private DateTime _curDate;
        private ELanguage _curLang;
        private int _eventMode;  // 1 - add from Transits Chart, 2 - add from here, 3 - modify from here, 4 - view (for select) from Transits Chart  
        private TransitEvent _transitEvent;
        private List<TransitEvent> _curProfileEventsList;
        

        public TransitEvent SelectedTransitEvent
        {
            get { return _transitEvent; }
            set { _transitEvent = value; }
        }

        public PersonTransitEvents()
        {
            InitializeComponent();
        }

        public PersonTransitEvents(Profile selectedProfile, ELanguage lang)
        {
            InitializeComponent();

            _curProfile = selectedProfile;
            _curDate = DateTime.Now;
            _curLang = lang;
            _eventMode = 4;
            _curLocationId = CacheLoad._locationList.Where(i => i.Id == _curProfile.PlaceOfLivingId).FirstOrDefault()?.Id ?? 0;

            _curProfileEventsList = CacheLoad._transitEventsList.Where(i => i.ProfileId == _curProfile.Id).ToList();
        }

        public PersonTransitEvents(Profile selectedProfile, int locId, DateTime selectedDate, ELanguage lang)
        {
            InitializeComponent();

            _curProfile = selectedProfile;
            _curDate = selectedDate;
            _curLang = lang;
            _eventMode = 1;
            _curLocationId = locId;

            _curProfileEventsList = CacheLoad._transitEventsList.Where(i => i.ProfileId == _curProfile.Id).ToList();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PersonTransitEvents_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _curLang);

            switch (_eventMode)
            {
                case 1:
                    CleanUpControls();
                    EnablingControls();
                    buttonChoose.Visible = false;
                    toolStripButtonSave.Enabled = true;
                    textBoxProfileName.Text = _curProfile.ProfileName;
                    maskedTextBoxDate.Text = _curDate.ToString("dd.MM.yyyy HH:mm:ss");
                    textBoxLocation.Text = CacheLoad._locationList.Where(i => i.Id == _curLocationId).FirstOrDefault()?.Locality ?? string.Empty;
                    textBoxTransitName.Focus();
                    break;

                case 4:
                    FillEventsListViewByData(_curProfileEventsList);
                    buttonChoose.Visible = false;
                    textBoxSearch.Focus();
                    break;
            }
        }

        private void CleanUpControls()
        { 
            textBoxTransitName.Text = string.Empty;
            textBoxProfileName.Text = string.Empty;
            maskedTextBoxDate.Text = string.Empty;
            textBoxLocation.Text = string.Empty;
            richTextBoxDesc.Text = string.Empty;
        }
        private void EnablingControls()
        {
            textBoxTransitName.ReadOnly = false;
            maskedTextBoxDate.ReadOnly = false;
            buttonLocation.Enabled = true;
            richTextBoxDesc.ReadOnly = false;

            textBoxTransitName.BackColor = SystemColors.Window;
            maskedTextBoxDate.BackColor = SystemColors.Window;
            buttonLocation.BackColor = SystemColors.Window;
            richTextBoxDesc.BackColor = SystemColors.Window;
        }

        private void DisablingControls()
        {
            textBoxTransitName.ReadOnly = true;
            maskedTextBoxDate.ReadOnly = true;
            buttonLocation.Enabled = false;
            richTextBoxDesc.ReadOnly = true;

            textBoxTransitName.BackColor = SystemColors.GradientInactiveCaption;
            maskedTextBoxDate.BackColor = SystemColors.GradientInactiveCaption;
            buttonLocation.BackColor = SystemColors.GradientInactiveCaption;
            richTextBoxDesc.BackColor = SystemColors.GradientInactiveCaption;
        }

        private void FillEventsListViewByData(List<TransitEvent> teList)
        {
            if (teList.Count > 0)
            {
                PrepareListView();
                foreach (TransitEvent te in teList)
                {
                   listViewEvents.Items.Add(te.EventName);
                }
            }
        }

        private void PrepareListView()
        {
            listViewEvents.HeaderStyle = ColumnHeaderStyle.None;
            listViewEvents.Columns.Add("", listViewEvents.Width, HorizontalAlignment.Left);
        }

        private void ShowDataByName(string eventName)
        {
            SelectedTransitEvent = _curProfileEventsList.Where(i => i.EventName.Equals(eventName)).FirstOrDefault();
            if (SelectedTransitEvent != null)
            {
                textBoxTransitName.Text = SelectedTransitEvent.EventName;
                textBoxProfileName.Text = _curProfile.ProfileName;
                maskedTextBoxDate.Text = SelectedTransitEvent.EventDate.ToString("dd.MM.yyyy HH:mm:ss");
                textBoxLocation.Text = CacheLoad._locationList.Where(i => i.Id == SelectedTransitEvent.LocationId).FirstOrDefault()?.Locality ?? string.Empty;
                richTextBoxDesc.Text = SelectedTransitEvent.Description;
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxSearch.Text.Trim()) == false)
            {
                listViewEvents.Items.Clear();
                CleanUpControls();
                foreach (TransitEvent te in _curProfileEventsList)
                {
                    PrepareListView();
                    if (te.EventName.ToLower().StartsWith(textBoxSearch.Text.ToLower().Trim()))
                    {
                        listViewEvents.Items.Add(te.EventName);
                    }
                }
            }
            else if (textBoxSearch.Text.Trim() == "")
            {
                listViewEvents.Items.Clear();
                FillEventsListViewByData(_curProfileEventsList);
                CleanUpControls();
                buttonChoose.Visible = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonDelete.Enabled = false;
            }
        }

        private void InsertNewEvent()
        {
            TransitEvent newEvent = new TransitEvent
            {
                EventName = textBoxTransitName.Text,
                ProfileId = _curProfile.Id,
                EventDate = DateTime.ParseExact(maskedTextBoxDate.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                LocationId = _curLocationId, // CacheLoad._locationList.Where(i => i.Id == _curLocationId).FirstOrDefault()?.Id ?? 0,
                Description = richTextBoxDesc.Text
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("insert into TRANSIT_EVENTS (PROFILEID, EVENTDATE, LOCATIONID, EVENTNAME, DESCRIPTION) values (@PROFILEID, @EVENTDATE, @LOCATIONID, @EVENTNAME, @DESCRIPTION)", dbCon);
                    command.Parameters.AddWithValue("@PROFILEID", newEvent.ProfileId);
                    command.Parameters.AddWithValue("@EVENTDATE", newEvent.EventDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@LOCATIONID", newEvent.LocationId);
                    command.Parameters.AddWithValue("@EVENTNAME", newEvent.EventName);
                    command.Parameters.AddWithValue("@DESCRIPTION", newEvent.Description);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void ModifyEvent(int eventId)
        {
            TransitEvent newEvent = new TransitEvent
            {
                EventName = textBoxTransitName.Text,
                ProfileId = _curProfile.Id,
                EventDate = DateTime.ParseExact(maskedTextBoxDate.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                LocationId = _curLocationId, // CacheLoad._locationList.Where(i => i.Id == _curLocationId).FirstOrDefault()?.Id ?? 0,
                Description = richTextBoxDesc.Text
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update TRANSIT_EVENTS set PROFILEID = @PROFILEID, EVENTDATE = @EVENTDATE, LOCATIONID = @LOCATIONID, EVENTNAME = @EVENTNAME, DESCRIPTION = @DESCRIPTION where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@PROFILEID", newEvent.ProfileId);
                    command.Parameters.AddWithValue("@EVENTDATE", newEvent.EventDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@LOCATIONID", newEvent.LocationId);
                    command.Parameters.AddWithValue("@EVENTNAME", newEvent.EventName);
                    command.Parameters.AddWithValue("@DESCRIPTION", newEvent.Description);
                    command.Parameters.AddWithValue("@ID", eventId);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void DeleteEvent(int eventId)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to delete this Transit Event?", _curLang), Utility.GetLocalizedText("Confirmation", _curLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SQLiteConnection dbCon = Utility.GetSQLConnection())
                {
                    dbCon.Open();
                    SQLiteCommand command;
                    try
                    {
                        command = new SQLiteCommand("delete from TRANSIT_EVENTS where ID = @ID", dbCon);
                        command.Parameters.AddWithValue("@ID", eventId);
                        command.ExecuteNonQuery();
                    }
                    catch { }
                    dbCon.Close();
                }
            }
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            int selectedEventId = 0, selectedIndex = -1;
            if (listViewEvents.SelectedIndices.Count <= 0)
            {
                return;
            }

            selectedIndex = listViewEvents.SelectedIndex();
            if (selectedIndex >= 0)
            {
                selectedEventId = _curProfileEventsList.Where(i => i.EventName.Equals(listViewEvents.Items[selectedIndex].Text)).FirstOrDefault()?.Id ?? 0;
                if (selectedEventId != 0)
                {
                    SelectedTransitEvent = CacheLoad._transitEventsList.Where(i => i.Id == selectedEventId).FirstOrDefault();
                }
                else
                {
                    SelectedTransitEvent = null;
                }
            }
            Close();
        }

        private void maskedTextBoxDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            listViewEvents.SelectedIndices.Clear();
            _eventMode = 2;

            CleanUpControls();
            EnablingControls();

            textBoxProfileName.Text = _curProfile.ProfileName;
            maskedTextBoxDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            textBoxLocation.Text = CacheLoad._locationList.Where(i => i.Id == _curLocationId).FirstOrDefault()?.Locality ?? string.Empty;

            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = true;
            textBoxTransitName.Focus();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            _eventMode = 3;

            EnablingControls();
            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = true;

            textBoxTransitName.Focus();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (listViewEvents.SelectedIndices.Count <= 0)
            {
                return;
            }
            int selectedindex = listViewEvents.SelectedIndices[0];
            if (selectedindex >= 0)
            {
                int selectedEventId = _curProfileEventsList[selectedindex].Id;
                DeleteEvent(selectedEventId);

                _curProfileEventsList.RemoveAll(i => i.Id == selectedEventId);
                CacheLoad._transitEventsList.RemoveAll(i => i.Id == selectedEventId);
                FillEventsListViewByData(_curProfileEventsList);

                CleanUpControls();
                buttonChoose.Visible = false;
                textBoxSearch_TextChanged(sender, e);
                textBoxSearch.Focus();
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (_eventMode == 1 || _eventMode == 2)
            {
                InsertNewEvent();
            }
            else if (_eventMode == 3)
            {
                ModifyEvent(SelectedTransitEvent.Id);
            }

            CacheLoad._transitEventsList = CacheLoad.GetTransitEventsList();
            _curProfileEventsList = CacheLoad._transitEventsList.Where(i => i.ProfileId == _curProfile.Id).ToList();
            FillEventsListViewByData(_curProfileEventsList);

            buttonChoose.Visible = false;
            toolStripButtonAdd.Enabled = true; ;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = false;

            CleanUpControls();
            DisablingControls();
            textBoxSearch_TextChanged(sender, e);
            textBoxSearch.Focus();
        }

        private void listViewEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEvents.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewEvents.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                ShowDataByName(listViewEvents.Items[intselectedindex].Text);
                buttonChoose.Visible = true;
                toolStripButtonAdd.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDelete.Enabled = true;
                toolStripButtonSave.Enabled = false;
            }
        }
    }
}
