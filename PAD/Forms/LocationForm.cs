using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class LocationForm : Form
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

        private List<Location> _lList;
        private ELanguage _activeLang;
        private bool _isNew;
        private bool _isModify;
        
        private Location _location;
        public Location SelectedLocation
        {
            get { return _location; }
            set { _location = value; }
        }

        private bool _singleMode;
        public bool SingleMode
        {
            get { return _singleMode; }
            set { _singleMode = value; }
        }

        public LocationForm()
        {
            InitializeComponent();
        }

        public LocationForm(List<Location> lList, ELanguage activeLang, bool sMode)
        {
            InitializeComponent();

            _lList = lList;
            _activeLang = activeLang;
            _isNew = false;
            _isModify = false;
            SingleMode = sMode;
        }

        private void linkLabelGeo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch (_activeLang)
            {
                case ELanguage.en:
                    System.Diagnostics.Process.Start("https://dateandtime.info/citycoordinates.php");
                    break;

                case ELanguage.ru:
                    System.Diagnostics.Process.Start("https://dateandtime.info/ru/citycoordinates.php");
                    break;

                default:
                    break;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Location_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);

            if (SingleMode)
            {
                buttonChoose.Visible = false;
            }

            toolStripButtonAdd.Enabled = true;
            textBoxSearch.Focus();
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            KeyValueData selectedItem = (KeyValueData)listBoxLocality.SelectedItem;
            if (selectedItem != null)
            {
                SelectedLocation = CacheLoad._locationList.Where(i => i.Id == selectedItem.ItemId).FirstOrDefault();
            }
            else
            {
                SelectedLocation = null;
                
            }
            Close();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxSearch.Text.Trim()) == false)
            {
                listBoxLocality.Items.Clear();
                CleanTextFields();
                foreach (Location loc in _lList)
                {
                    if (loc.Locality.ToLower().StartsWith(textBoxSearch.Text.ToLower().Trim()))
                    {
                        listBoxLocality.Items.Add(new KeyValueData(loc.Locality, loc.Id));
                    }
                }
            }
            else if (textBoxSearch.Text.Trim() == "")
            {
                listBoxLocality.Items.Clear();
                CleanTextFields();
                buttonChoose.Enabled = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonDelete.Enabled = false;
            }
        }

        private void ShowDataByName(KeyValueData lData)
        {
            Location currentLoc = _lList.Where(i => i.Id == lData.ItemId).FirstOrDefault();
            if (currentLoc != null)
            {
                textBoxLocality.Text = currentLoc.Locality;
                textBoxRegion.Text = currentLoc.Region;
                textBoxState.Text = currentLoc.State;
                textBoxCountry.Text = currentLoc.Country;
                textBoxLatitude.Text = currentLoc.Latitude;
                textBoxLongitude.Text = currentLoc.Longitude;
            }
        }

        private void listBoxLocality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLocality.SelectedItem == null)
                return;

            ShowDataByName((KeyValueData)listBoxLocality.SelectedItem);

            if (listBoxLocality.Items.Count > 0 && listBoxLocality.SelectedItem != null)
            {
                buttonChoose.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDelete.Enabled = true;
            }
        }

        private void CleanTextFields()
        {
            textBoxLocality.Text = string.Empty;
            textBoxRegion.Text = string.Empty;
            textBoxState.Text = string.Empty;
            textBoxCountry.Text = string.Empty;
            textBoxLatitude.Text = string.Empty;
            textBoxLongitude.Text = string.Empty;
        }

        private void MakeTextFieldsEditable()
        {
            textBoxLocality.ReadOnly = false;
            textBoxRegion.ReadOnly = false;
            textBoxState.ReadOnly = false;
            textBoxCountry.ReadOnly = false;
            textBoxLatitude.ReadOnly = false;
            textBoxLongitude.ReadOnly = false;

            textBoxLocality.BackColor = SystemColors.Window;
            textBoxRegion.BackColor = SystemColors.Window;
            textBoxState.BackColor = SystemColors.Window;
            textBoxCountry.BackColor = SystemColors.Window;
            textBoxLatitude.BackColor = SystemColors.Window;
            textBoxLongitude.BackColor = SystemColors.Window;
        }

        private void MakeTextFieldsReadOnly()
        {
            textBoxLocality.ReadOnly = true;
            textBoxRegion.ReadOnly = true;
            textBoxState.ReadOnly = true;
            textBoxCountry.ReadOnly = true;
            textBoxLatitude.ReadOnly = true;
            textBoxLongitude.ReadOnly = true;

            textBoxLocality.BackColor = SystemColors.GradientInactiveCaption;
            textBoxRegion.BackColor = SystemColors.GradientInactiveCaption;
            textBoxState.BackColor = SystemColors.GradientInactiveCaption;
            textBoxCountry.BackColor = SystemColors.GradientInactiveCaption;
            textBoxLatitude.BackColor = SystemColors.GradientInactiveCaption;
            textBoxLongitude.BackColor = SystemColors.GradientInactiveCaption;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            _isNew = true;
            _isModify = false;

            textBoxSearch.Text = string.Empty;
            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = true;
            MakeTextFieldsEditable();
            textBoxLocality.Focus();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            _isNew = false;
            _isModify = true;

            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = true;
            MakeTextFieldsEditable();
            textBoxLocality.Focus();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            KeyValueData selectedItem = (KeyValueData)listBoxLocality.SelectedItem;
            int selectedLocationId = selectedItem.ItemId;

            if (selectedLocationId != 0)
            {
                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to delete this location?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SQLiteConnection dbCon = Utility.GetSQLConnection())
                    {
                        dbCon.Open();
                        try
                        {
                            SQLiteCommand command = new SQLiteCommand("delete from LOCATION where ID = @ID", dbCon);
                            command.Parameters.AddWithValue("@ID", selectedLocationId);
                            command.ExecuteNonQuery();
                        }
                        catch { }
                        dbCon.Close();
                    }
                    CacheLoad._locationList = CacheLoad.GetLocationsList();
                    _lList = CacheLoad._locationList.ToList();
                    
                    buttonChoose.Enabled = false;
                    toolStripButtonEdit.Enabled = false;
                    toolStripButtonDelete.Enabled = false;

                    textBoxSearch_TextChanged(sender, e);
                    textBoxSearch.Focus();
                }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (textBoxLocality.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Enter locality name", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }

            if (_isNew)
            {
                InsertNewLocation();
            }
            if (_isModify)
            {
                KeyValueData selectedItem = (KeyValueData)listBoxLocality.SelectedItem;
                ModifyLocation(selectedItem.ItemId);
            }

            CacheLoad._locationList = CacheLoad.GetLocationsList();
            _lList = CacheLoad._locationList.ToList();

            buttonChoose.Enabled = false;
            toolStripButtonAdd.Enabled = true; ;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = false;

            CleanTextFields();
            MakeTextFieldsReadOnly();
            textBoxSearch_TextChanged(sender, e);
            textBoxSearch.Focus();
        }

        private void InsertNewLocation()
        {
            string latitude = textBoxLatitude.Text;
            if (latitude.Contains(","))
                latitude = latitude.Replace(",", ".");

            string longitude = textBoxLongitude.Text;
            if (longitude.Contains(","))
                longitude = longitude.Replace(",", ".");

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("insert into LOCATION (LOCALITY, LATITUDE, LONGITUDE, REGION, STATE, COUNTRY, LANGUAGECODE) values (@LOCALITY, @LATITUDE, @LONGITUDE, @REGION, @STATE, @COUNTRY, @LANGUAGECODE)", dbCon);
                    command.Parameters.AddWithValue("@LOCALITY", textBoxLocality.Text);
                    command.Parameters.AddWithValue("@LATITUDE", latitude);
                    command.Parameters.AddWithValue("@LONGITUDE", longitude);
                    command.Parameters.AddWithValue("@REGION", textBoxRegion.Text);
                    command.Parameters.AddWithValue("@STATE", textBoxState.Text);
                    command.Parameters.AddWithValue("@COUNTRY", textBoxCountry.Text);
                    command.Parameters.AddWithValue("@LANGUAGECODE", _activeLang.ToString());

                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void ModifyLocation(int locId)
        {
            string latitude = textBoxLatitude.Text;
            if (latitude.Contains(","))
                latitude = latitude.Replace(",", ".");

            string longitude = textBoxLongitude.Text;
            if (longitude.Contains(","))
                longitude = longitude.Replace(",", ".");

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update LOCATION set LOCALITY = @LOCALITY, LATITUDE = @LATITUDE, LONGITUDE = @LONGITUDE, REGION = @REGION, STATE = @STATE, COUNTRY = @COUNTRY, LANGUAGECODE = @LANGUAGECODE where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@LOCALITY", textBoxLocality.Text);
                    command.Parameters.AddWithValue("@LATITUDE", latitude);
                    command.Parameters.AddWithValue("@LONGITUDE", longitude);
                    command.Parameters.AddWithValue("@REGION", textBoxRegion.Text);
                    command.Parameters.AddWithValue("@STATE", textBoxState.Text);
                    command.Parameters.AddWithValue("@COUNTRY", textBoxCountry.Text);
                    command.Parameters.AddWithValue("@LANGUAGECODE", _activeLang.ToString());
                    command.Parameters.AddWithValue("@ID", locId);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                dbCon.Close();
            }
        }

    }
}
