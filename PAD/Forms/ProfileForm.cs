using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static PAD.MainForm;

namespace PAD
{
    public partial class ProfileForm : Form
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

        private List<Profile_old> _proList;
        private ELanguage _activeLang;
        private bool _isNew;
        private bool _isModify;

        private List<NakshatraDescription> _ndList;
        private List<ZodiakDescription> _zdList;
        private Location _selectedLocation;

        private Profile_old _profile;
        public Profile_old SelectedProfile
        {
            get { return _profile; }
            set { _profile = value; }
        }

        private bool _isChosen;
        public bool IsChosen
        {
            get { return _isChosen; }
            set { _isChosen = value; }
        }

        private bool _singleMode;
        public bool SingleMode
        {
            get { return _singleMode; }
            set { _singleMode = value; }
        }

        public ProfileForm()
        {
            InitializeComponent();
        }

        public ProfileForm(List<Profile_old> pList, ELanguage activeLang, bool sMode)
        {
            InitializeComponent();

            _proList = pList;
            _activeLang = activeLang;
            _isNew = false;
            _isModify = false;
            SingleMode = sMode;

            _ndList = CacheLoad._nakshatraDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
            _zdList = CacheLoad._zodiakDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();

            IsChosen = false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            IsChosen = false;
            Close();
        }

        private void ProfileForm_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);

            if (SingleMode)
            {
                buttonChoose.Visible = false;
            }

            PrepareDataGridInfo(_activeLang);

            FillProfileListViewByData(_proList);

            toolStripButtonAdd.Enabled = true;
            textBoxSearch.Focus();
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
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Znak";
            column.Name = Utility.GetLocalizedText("Znak", langCode);
            column.Width = 100;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Nakshatra";
            column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
            column.Width = 140;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            int lastColWidth = (dataGridViewInfo.Width - 270);
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
            public string Zodiak { get; set; }
            public string Nakshatra { get; set; }
            public string Pada { get; set; }
        }

        private void ProfileInfoDataGridViewFillByRow(Profile_old person, ELanguage langCode)
        {
            dgvRowObj rowTemp;
            List<dgvRowObj> rowList = new List<dgvRowObj>();

            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedText("Lg", langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraLagnaId, person.PadaLagna),
                Nakshatra = GetNakshatraNameById(person.NakshatraLagnaId),
                Pada = person.PadaLagna.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SUN, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraSunId, person.PadaSun),
                Nakshatra = GetNakshatraNameById(person.NakshatraSunId),
                Pada = person.PadaSun.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MOON, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraMoonId, person.PadaMoon),
                Nakshatra = GetNakshatraNameById(person.NakshatraMoonId),
                Pada = person.PadaMoon.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MARS, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraMarsId, person.PadaMars),
                Nakshatra = GetNakshatraNameById(person.NakshatraMarsId),
                Pada = person.PadaMars.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MERCURY, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraMercuryId, person.PadaMercury),
                Nakshatra = GetNakshatraNameById(person.NakshatraMercuryId),
                Pada = person.PadaMercury.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.JUPITER, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraJupiterId, person.PadaJupiter),
                Nakshatra = GetNakshatraNameById(person.NakshatraJupiterId),
                Pada = person.PadaJupiter.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.VENUS, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraVenusId, person.PadaVenus),
                Nakshatra = GetNakshatraNameById(person.NakshatraVenusId),
                Pada = person.PadaVenus.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SATURN, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraSaturnId, person.PadaSaturn),
                Nakshatra = GetNakshatraNameById(person.NakshatraSaturnId),
                Pada = person.PadaSaturn.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.RAHUMEAN, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraRahuId, person.PadaRahu),
                Nakshatra = GetNakshatraNameById(person.NakshatraRahuId),
                Pada = person.PadaRahu.ToString()
            };
            rowList.Add(rowTemp);
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.KETUMEAN, langCode).Substring(0, 2),
                Zodiak = GetZodiakNameByIds(person.NakshatraKetuId, person.PadaKetu),
                Nakshatra = GetNakshatraNameById(person.NakshatraKetuId),
                Pada = person.PadaKetu.ToString()
            };
            rowList.Add(rowTemp);

            for (int i = 0; i < rowList.Count; i++)
            {
                string[] row = new string[] {
                        rowList[i].Planet,
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

        }

        private string GetZodiakNameByIds(int nakshatraId, int padaId)
        {
            int id = Utility.GetZodiakIdFromNakshatraIdandPada(nakshatraId, padaId);
            return id + ". " + _zdList.Where(i => i.ZodiakId == id).FirstOrDefault().Name;
        }
        private string GetNakshatraNameById(int id)
        {
            return id + ". " + _ndList.Where(i => i.NakshatraId == id).FirstOrDefault().Name;
        }

        private void FillProfileListViewByData(List<Profile_old> pList)
        {
            if (pList.Count > 0)
            {
                PrepareListView();
                foreach (Profile_old p in pList)
                {
                    if (p.IsChecked)
                    {
                        listViewProfile.Items.Add(p.ProfileName, 0);
                    }
                    else
                    {
                        listViewProfile.Items.Add(p.ProfileName, -1);
                    }
                }
            }
        }

        private void PrepareListView()
        {
            listViewProfile.HeaderStyle = ColumnHeaderStyle.None;
            listViewProfile.Columns.Add("", listViewProfile.Width, HorizontalAlignment.Left);

            ImageList imgs = new ImageList();
            imgs.Images.Add(Properties.Resources.green_check_32);
            listViewProfile.SmallImageList = imgs;
        }

        

        private void ShowDataByName(string profile)
        {
            SelectedProfile = _proList.Where(i => i.ProfileName.Equals(profile)).FirstOrDefault();
            if (SelectedProfile != null)
            {
                textBoxProfileName.Text = SelectedProfile.ProfileName;
                textBoxPersonName.Text = SelectedProfile.PersonName;
                textBoxPersonSurname.Text = SelectedProfile.PersonSurname;
                textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == SelectedProfile.PlaceOfLivingId).FirstOrDefault()?.Locality ?? string.Empty;

                dataGridViewInfo.Rows.Clear();
                ProfileInfoDataGridViewFillByRow(SelectedProfile, _activeLang);

            }
        }


        private void listViewProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewProfile.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                ShowDataByName(listViewProfile.Items[intselectedindex].Text);
                buttonDefault.Enabled = true;
                buttonChoose.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDelete.Enabled = true;
            }
        }
        
        private void checkedListBoxProfile_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (e.Index == 0) e.NewValue = e.CurrentValue;
            /*
            if (e.NewValue == CheckState.Checked && checkedListBoxProfile.CheckedItems.Count > 0)
            {
                checkedListBoxProfile.ItemCheck -= checkedListBoxProfile_ItemCheck;
                checkedListBoxProfile.SetItemChecked(checkedListBoxProfile.CheckedIndices[0], false);
                checkedListBoxProfile.ItemCheck += checkedListBoxProfile_ItemCheck;
            }*/
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            int selectedProfileId = 0;
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewProfile.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                selectedProfileId = _proList.Where(i => i.ProfileName.Equals(listViewProfile.SelectedItems[0].Text)).FirstOrDefault()?.Id ?? 0;
            }
            if (selectedProfileId != 0)
            {
                SelectedProfile = CacheLoad._profileList_old.Where(i => i.Id == selectedProfileId).FirstOrDefault();
                IsChosen = true;
            }
            else
            {
                SelectedProfile = null;

            }
            Close();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxSearch.Text.Trim()) == false)
            {
                listViewProfile.Items.Clear();
                CleanFields();
                foreach (Profile_old p in _proList)
                {
                    PrepareListView();
                    if (p.ProfileName.ToLower().StartsWith(textBoxSearch.Text.ToLower().Trim()))
                    {
                        listViewProfile.Items.Add(p.ProfileName);
                    }
                }
            }
            else if (textBoxSearch.Text.Trim() == "")
            {
                listViewProfile.Items.Clear();
                FillProfileListViewByData(_proList);
                CleanFields();
                buttonDefault.Enabled = false;
                buttonChoose.Enabled = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonDelete.Enabled = false;
            }
        }

        private void CleanFields()
        {
            textBoxProfileName.Text = string.Empty;
            textBoxPersonName.Text = string.Empty;
            textBoxPersonSurname.Text = string.Empty;
            textBoxLivingPlace.Text = string.Empty;
            
        }

        private void MakeTextFieldsEditable(bool isUpdate)
        {
            textBoxProfileName.ReadOnly = false;
            textBoxPersonName.ReadOnly = false;
            textBoxPersonSurname.ReadOnly = false;
            buttonLivingPlace.Enabled = true;

            


            textBoxProfileName.BackColor = SystemColors.Window;
            textBoxPersonName.BackColor = SystemColors.Window;
            textBoxPersonSurname.BackColor = SystemColors.Window;
        }

        private void MakeTextFieldsReadOnly()
        {
            textBoxProfileName.ReadOnly = true;
            textBoxPersonName.ReadOnly = true;
            textBoxPersonSurname.ReadOnly = true;
            buttonLivingPlace.Enabled = false;


            textBoxProfileName.BackColor = SystemColors.GradientInactiveCaption;
            textBoxPersonName.BackColor = SystemColors.GradientInactiveCaption;
            textBoxPersonSurname.BackColor = SystemColors.GradientInactiveCaption;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            _isNew = true;
            _isModify = false;

            listViewProfile.SelectedIndices.Clear();
            CleanFields();
            textBoxSearch.Text = string.Empty;
            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = true;
            buttonChoose.Enabled = false;
            buttonDefault.Enabled = false;
            MakeTextFieldsEditable(false);
            textBoxProfileName.Focus();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            _isNew = false;
            _isModify = true;

            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = true;
            buttonChoose.Enabled = false;
            buttonDefault.Enabled = false;
            MakeTextFieldsEditable(true);
            textBoxProfileName.Focus();
        }

        private void buttonLivingPlace_Click(object sender, EventArgs e)
        {
            LocationForm lForm = new LocationForm(CacheLoad._locationList.ToList(), _activeLang, false);
            lForm.ShowDialog(this);
            if (lForm.SelectedLocation != null)
            {
                _selectedLocation = lForm.SelectedLocation;
                textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == _selectedLocation.Id).FirstOrDefault()?.Locality ?? string.Empty;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            int selectedindex = listViewProfile.SelectedIndices[0];
            if (selectedindex >= 0)
            {
                int selectedProfileId = _proList[selectedindex].Id;
                DeleteProfile(selectedProfileId);

                _proList.RemoveAll(i => i.Id == selectedProfileId);
                CacheLoad._profileList_old.RemoveAll(i => i.Id == selectedProfileId);
                FillProfileListViewByData(_proList);

                CleanFields();
                MakeTextFieldsReadOnly();
                buttonChoose.Visible = false;
                buttonDefault.Enabled = false;
                textBoxSearch_TextChanged(sender, e);
                textBoxSearch.Focus();
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (textBoxProfileName.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Enter profile name.", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }
            if (textBoxLivingPlace.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Choose place of living.", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }

            if (_isNew)
            {
                InsertNewProfile();
            }
            if (_isModify)
            {
                ModifyProfile(SelectedProfile.Id);
            }

            CacheLoad._profileList_old = CacheLoad.GetProfileList();
            _proList = CacheLoad._profileList_old.ToList();
            FillProfileListViewByData(_proList);

            buttonChoose.Enabled = false;
            buttonDefault.Enabled = false;
            toolStripButtonAdd.Enabled = true; ;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = false;

            CleanFields();
            MakeTextFieldsReadOnly();
            textBoxSearch_TextChanged(sender, e);
            textBoxSearch.Focus();
        }

        private void InsertNewProfile()
        {
            
            Profile_old newProfile = new Profile_old
            {
                ProfileName = textBoxProfileName.Text,
                PersonName = textBoxPersonName.Text,
                PersonSurname = textBoxPersonSurname.Text,
                PlaceOfLivingId = CacheLoad._locationList.Where(i => i.Id == _selectedLocation.Id).FirstOrDefault()?.Id ?? 0,
                
                IsChecked = false
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("insert into PROFILE_OLD (PROFILENAME, PERSONNAME, PERSONSURNAME, PLACEOFLIVINGID, NAKSHATRAMOONID, PADAMOON, NAKSHATRALAGNAID, PADALAGNA, NAKSHATRASUNID, PADASUN, NAKSHATRAVENUSID, PADAVENUS, NAKSHATRAJUPITERID, PADAJUPITER, NAKSHATRAMERCURYID, PADAMERCURY, NAKSHATRAMARSID, PADAMARS, NAKSHATRASATURNID, PADASATURN, NAKSHATRARAHUID, PADARAHU, NAKSHATRAKETUID, PADAKETU, CHECKED, GUID) values (@PROFILENAME, @PERSONNAME, @PERSONSURNAME, @PLACEOFLIVINGID, @NAKSHATRAMOONID, @PADAMOON, @NAKSHATRALAGNAID, @PADALAGNA, @NAKSHATRASUNID, @PADASUN, @NAKSHATRAVENUSID, @PADAVENUS, @NAKSHATRAJUPITERID, @PADAJUPITER, @NAKSHATRAMERCURYID, @PADAMERCURY, @NAKSHATRAMARSID, @PADAMARS, @NAKSHATRASATURNID, @PADASATURN, @NAKSHATRARAHUID, @PADARAHU, @NAKSHATRAKETUID, @PADAKETU, @CHECKED, @GUID)", dbCon);
                    command.Parameters.AddWithValue("@PROFILENAME", newProfile.ProfileName);
                    command.Parameters.AddWithValue("@PERSONNAME", newProfile.PersonName);
                    command.Parameters.AddWithValue("@PERSONSURNAME", newProfile.PersonSurname);
                    command.Parameters.AddWithValue("@PLACEOFLIVINGID", newProfile.PlaceOfLivingId);
                    command.Parameters.AddWithValue("@NAKSHATRAMOONID", newProfile.NakshatraMoonId);
                    command.Parameters.AddWithValue("@PADAMOON", newProfile.PadaMoon);
                    command.Parameters.AddWithValue("@NAKSHATRALAGNAID", newProfile.NakshatraLagnaId);
                    command.Parameters.AddWithValue("@PADALAGNA", newProfile.PadaLagna);
                    command.Parameters.AddWithValue("@NAKSHATRASUNID", newProfile.NakshatraSunId);
                    command.Parameters.AddWithValue("@PADASUN", newProfile.PadaSun);
                    command.Parameters.AddWithValue("@NAKSHATRAVENUSID", newProfile.NakshatraVenusId);
                    command.Parameters.AddWithValue("@PADAVENUS", newProfile.PadaVenus);
                    command.Parameters.AddWithValue("@NAKSHATRAJUPITERID", newProfile.NakshatraJupiterId);
                    command.Parameters.AddWithValue("@PADAJUPITER", newProfile.PadaJupiter);
                    command.Parameters.AddWithValue("@NAKSHATRAMERCURYID", newProfile.NakshatraMercuryId);
                    command.Parameters.AddWithValue("@PADAMERCURY", newProfile.PadaMercury);
                    command.Parameters.AddWithValue("@NAKSHATRAMARSID", newProfile.NakshatraMarsId);
                    command.Parameters.AddWithValue("@PADAMARS", newProfile.PadaMars);
                    command.Parameters.AddWithValue("@NAKSHATRASATURNID", newProfile.NakshatraSaturnId);
                    command.Parameters.AddWithValue("@PADASATURN", newProfile.PadaSaturn);
                    command.Parameters.AddWithValue("@NAKSHATRARAHUID", newProfile.NakshatraRahuId);
                    command.Parameters.AddWithValue("@PADARAHU", newProfile.PadaRahu);
                    command.Parameters.AddWithValue("@NAKSHATRAKETUID", newProfile.NakshatraKetuId);
                    command.Parameters.AddWithValue("@PADAKETU", newProfile.PadaKetu);
                    command.Parameters.AddWithValue("@CHECKED", Convert.ToInt32(newProfile.IsChecked));
                    command.Parameters.AddWithValue("@GUID", newProfile.GUID);

                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void ModifyProfile(int pId)
        {
            

            int locationId = SelectedProfile.PlaceOfLivingId;
            if (_selectedLocation != null)
                locationId = _selectedLocation.Id;

            Profile_old newProfile = new Profile_old
            {
                ProfileName = textBoxProfileName.Text,
                PersonName = textBoxPersonName.Text,
                PersonSurname = textBoxPersonSurname.Text,
                PlaceOfLivingId = CacheLoad._locationList.Where(i => i.Id == locationId).FirstOrDefault()?.Id ?? 0,
                
               
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update PROFILE_OLD set PROFILENAME = @PROFILENAME, PERSONNAME = @PERSONNAME, PERSONSURNAME = @PERSONSURNAME, PLACEOFLIVINGID = @PLACEOFLIVINGID, NAKSHATRAMOONID = @NAKSHATRAMOONID, PADAMOON = @PADAMOON, NAKSHATRALAGNAID = @NAKSHATRALAGNAID, PADALAGNA = @PADALAGNA, NAKSHATRASUNID = @NAKSHATRASUNID, PADASUN = @PADASUN, NAKSHATRAVENUSID = @NAKSHATRAVENUSID, PADAVENUS = @PADAVENUS, NAKSHATRAJUPITERID = @NAKSHATRAJUPITERID, PADAJUPITER = @PADAJUPITER, NAKSHATRAMERCURYID = @NAKSHATRAMERCURYID, PADAMERCURY = @PADAMERCURY, NAKSHATRAMARSID = @NAKSHATRAMARSID, PADAMARS = @PADAMARS, NAKSHATRASATURNID = @NAKSHATRASATURNID, PADASATURN = @PADASATURN, NAKSHATRARAHUID = @NAKSHATRARAHUID, PADARAHU = @PADARAHU, NAKSHATRAKETUID = @NAKSHATRAKETUID, PADAKETU = @PADAKETU where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@PROFILENAME", newProfile.ProfileName);
                    command.Parameters.AddWithValue("@PERSONNAME", newProfile.PersonName);
                    command.Parameters.AddWithValue("@PERSONSURNAME", newProfile.PersonSurname);
                    command.Parameters.AddWithValue("@PLACEOFLIVINGID", newProfile.PlaceOfLivingId);
                    command.Parameters.AddWithValue("@NAKSHATRAMOONID", newProfile.NakshatraMoonId);
                    command.Parameters.AddWithValue("@PADAMOON", newProfile.PadaMoon);
                    command.Parameters.AddWithValue("@NAKSHATRALAGNAID", newProfile.NakshatraLagnaId);
                    command.Parameters.AddWithValue("@PADALAGNA", newProfile.PadaLagna);
                    command.Parameters.AddWithValue("@NAKSHATRASUNID", newProfile.NakshatraSunId);
                    command.Parameters.AddWithValue("@PADASUN", newProfile.PadaSun);
                    command.Parameters.AddWithValue("@NAKSHATRAVENUSID", newProfile.NakshatraVenusId);
                    command.Parameters.AddWithValue("@PADAVENUS", newProfile.PadaVenus);
                    command.Parameters.AddWithValue("@NAKSHATRAJUPITERID", newProfile.NakshatraJupiterId);
                    command.Parameters.AddWithValue("@PADAJUPITER", newProfile.PadaJupiter);
                    command.Parameters.AddWithValue("@NAKSHATRAMERCURYID", newProfile.NakshatraMercuryId);
                    command.Parameters.AddWithValue("@PADAMERCURY", newProfile.PadaMercury);
                    command.Parameters.AddWithValue("@NAKSHATRAMARSID", newProfile.NakshatraMarsId);
                    command.Parameters.AddWithValue("@PADAMARS", newProfile.PadaMars);
                    command.Parameters.AddWithValue("@NAKSHATRASATURNID", newProfile.NakshatraSaturnId);
                    command.Parameters.AddWithValue("@PADASATURN", newProfile.PadaSaturn);
                    command.Parameters.AddWithValue("@NAKSHATRARAHUID", newProfile.NakshatraRahuId);
                    command.Parameters.AddWithValue("@PADARAHU", newProfile.PadaRahu);
                    command.Parameters.AddWithValue("@NAKSHATRAKETUID", newProfile.NakshatraKetuId);
                    command.Parameters.AddWithValue("@PADAKETU", newProfile.PadaKetu);
                    command.Parameters.AddWithValue("@ID", pId);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void UpdateCheckStatus(int pId)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                SQLiteCommand command;
                try
                {
                    command = new SQLiteCommand("update PROFILE_OLD set CHECKED = 0", dbCon);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("update PROFILE_OLD set CHECKED = 1 where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@ID", pId);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void DeleteProfile(int pId)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to delete this profile?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SQLiteConnection dbCon = Utility.GetSQLConnection())
                {
                    dbCon.Open();
                    SQLiteCommand command;
                    try
                    {
                        command = new SQLiteCommand("delete from PROFILE_OLD where ID = @ID", dbCon);
                        command.Parameters.AddWithValue("@ID", pId);
                        command.ExecuteNonQuery();
                    }
                    catch { }
                    dbCon.Close();
                }
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            int selectedProfileId = 0;
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewProfile.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                selectedProfileId = _proList.Where(i => i.ProfileName.Equals(listViewProfile.SelectedItems[0].Text)).FirstOrDefault()?.Id ?? 0;
                UpdateCheckStatus(selectedProfileId);

                CacheLoad._profileList_old = CacheLoad.GetProfileList();
                _proList = CacheLoad._profileList_old.ToList();
                FillProfileListViewByData(_proList);

                CleanFields();
                MakeTextFieldsReadOnly();
                textBoxSearch_TextChanged(sender, e);
                textBoxSearch.Focus();
            }
        }


        



    }
}
