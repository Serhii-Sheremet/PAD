using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        private List<Profile> _proList;
        private ELanguage _activeLang;
        private bool _isNew;
        private bool _isModify;

        private List<NakshatraDescription> _ndList;
        private Location _selectedLocation;

        private Profile _profile;
        public Profile SelectedProfile
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

        public ProfileForm(List<Profile> pList, ELanguage activeLang, bool sMode)
        {
            InitializeComponent();

            _proList = pList;
            _activeLang = activeLang;
            _isNew = false;
            _isModify = false;
            SingleMode = sMode;

            _ndList = CacheLoad._nakshatraDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();

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

            FillProfileListViewByData(_proList);
            FillNakshatraComboBox();

            toolStripButtonAdd.Enabled = true;
            textBoxSearch.Focus();
        }

        private void FillProfileListViewByData(List<Profile> pList)
        {
            if (pList.Count > 0)
            {
                PrepareListView();
                foreach (Profile p in pList)
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

        private void FillNakshatraComboBox()
        {
            foreach (NakshatraDescription nd in _ndList)
            {
                comboBoxNakshatraMoon.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraLagna.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraSun.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraVenus.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraJupiter.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraMercury.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraMars.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraSaturn.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraRahu.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
                comboBoxNakshatraKetu.Items.Add(new KeyValueData(nd.NakshatraId + "." + nd.Name, nd.NakshatraId));
            }
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

                comboBoxNakshatraMoon.SelectedIndex = SelectedProfile.NakshatraMoonId - 1;
                comboBoxNakshatraLagna.SelectedIndex = SelectedProfile.NakshatraLagnaId - 1;
                comboBoxNakshatraSun.SelectedIndex = SelectedProfile.NakshatraSunId - 1;
                comboBoxNakshatraVenus.SelectedIndex = SelectedProfile.NakshatraVenusId - 1;
                comboBoxNakshatraJupiter.SelectedIndex = SelectedProfile.NakshatraJupiterId - 1;
                comboBoxNakshatraMercury.SelectedIndex = SelectedProfile.NakshatraMercuryId - 1;
                comboBoxNakshatraMars.SelectedIndex = SelectedProfile.NakshatraMarsId - 1;
                comboBoxNakshatraSaturn.SelectedIndex = SelectedProfile.NakshatraSaturnId - 1;
                comboBoxNakshatraRahu.SelectedIndex = SelectedProfile.NakshatraRahuId - 1;
                comboBoxNakshatraKetu.SelectedIndex = SelectedProfile.NakshatraKetuId - 1;

                SetPadaMoonRadioButton(SelectedProfile.PadaMoon);
                SetPadaLagnaRadioButton(SelectedProfile.PadaLagna);
                SetPadaSunRadioButton(SelectedProfile.PadaSun);
                SetPadaVenusRadioButton(SelectedProfile.PadaVenus);
                SetPadaJupiterRadioButton(SelectedProfile.PadaJupiter);
                SetPadaMercuryRadioButton(SelectedProfile.PadaMercury);
                SetPadaMarsRadioButton(SelectedProfile.PadaMars);
                SetPadaSaturnRadioButton(SelectedProfile.PadaSaturn);
                SetPadaRahuRadioButton(SelectedProfile.PadaRahu);
                SetPadaKetuRadioButton(SelectedProfile.PadaKetu);
            }
        }

        private void SetPadaMoonRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonMoon1.Checked = true;
                    break;
                case 2:
                    radioButtonMoon2.Checked = true;
                    break;
                case 3:
                    radioButtonMoon3.Checked = true;
                    break;
                case 4:
                    radioButtonMoon4.Checked = true;
                    break;
            }
        }

        private void SetPadaLagnaRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonLagna1.Checked = true;
                    break;
                case 2:
                    radioButtonLagna2.Checked = true;
                    break;
                case 3:
                    radioButtonLagna3.Checked = true;
                    break;
                case 4:
                    radioButtonLagna4.Checked = true;
                    break;
            }
        }

        private void SetPadaSunRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonSun1.Checked = true;
                    break;
                case 2:
                    radioButtonSun2.Checked = true;
                    break;
                case 3:
                    radioButtonSun3.Checked = true;
                    break;
                case 4:
                    radioButtonSun4.Checked = true;
                    break;
            }
        }

        private void SetPadaVenusRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonVenus1.Checked = true;
                    break;
                case 2:
                    radioButtonVenus2.Checked = true;
                    break;
                case 3:
                    radioButtonVenus3.Checked = true;
                    break;
                case 4:
                    radioButtonVenus4.Checked = true;
                    break;
            }
        }

        private void SetPadaJupiterRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonJupiter1.Checked = true;
                    break;
                case 2:
                    radioButtonJupiter2.Checked = true;
                    break;
                case 3:
                    radioButtonJupiter3.Checked = true;
                    break;
                case 4:
                    radioButtonJupiter4.Checked = true;
                    break;
            }
        }

        private void SetPadaMercuryRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonMercury1.Checked = true;
                    break;
                case 2:
                    radioButtonMercury2.Checked = true;
                    break;
                case 3:
                    radioButtonMercury3.Checked = true;
                    break;
                case 4:
                    radioButtonMercury4.Checked = true;
                    break;
            }
        }

        private void SetPadaMarsRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonMars1.Checked = true;
                    break;
                case 2:
                    radioButtonMars2.Checked = true;
                    break;
                case 3:
                    radioButtonMars3.Checked = true;
                    break;
                case 4:
                    radioButtonMars4.Checked = true;
                    break;
            }
        }

        private void SetPadaSaturnRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonSaturn1.Checked = true;
                    break;
                case 2:
                    radioButtonSaturn2.Checked = true;
                    break;
                case 3:
                    radioButtonSaturn3.Checked = true;
                    break;
                case 4:
                    radioButtonSaturn4.Checked = true;
                    break;
            }
        }

        private void SetPadaRahuRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonRahu1.Checked = true;
                    break;
                case 2:
                    radioButtonRahu2.Checked = true;
                    break;
                case 3:
                    radioButtonRahu3.Checked = true;
                    break;
                case 4:
                    radioButtonRahu4.Checked = true;
                    break;
            }
        }

        private void SetPadaKetuRadioButton(int pada)
        {
            switch (pada)
            {
                case 1:
                    radioButtonKetu1.Checked = true;
                    break;
                case 2:
                    radioButtonKetu2.Checked = true;
                    break;
                case 3:
                    radioButtonKetu3.Checked = true;
                    break;
                case 4:
                    radioButtonKetu4.Checked = true;
                    break;
            }
        }

        private int GetPadaMoonFromRadioButtons()
        {
            if (radioButtonMoon1.Checked)
                return 1;
            if (radioButtonMoon2.Checked)
                return 2;
            if (radioButtonMoon3.Checked)
                return 3;
            if (radioButtonMoon4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaLagnaFromRadioButtons()
        {
            if (radioButtonLagna1.Checked)
                return 1;
            if (radioButtonLagna2.Checked)
                return 2;
            if (radioButtonLagna3.Checked)
                return 3;
            if (radioButtonLagna4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaSunFromRadioButtons()
        {
            if (radioButtonSun1.Checked)
                return 1;
            if (radioButtonSun2.Checked)
                return 2;
            if (radioButtonSun3.Checked)
                return 3;
            if (radioButtonSun4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaVenusFromRadioButtons()
        {
            if (radioButtonVenus1.Checked)
                return 1;
            if (radioButtonVenus2.Checked)
                return 2;
            if (radioButtonVenus3.Checked)
                return 3;
            if (radioButtonVenus4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaJupiterFromRadioButtons()
        {
            if (radioButtonJupiter1.Checked)
                return 1;
            if (radioButtonJupiter2.Checked)
                return 2;
            if (radioButtonJupiter3.Checked)
                return 3;
            if (radioButtonJupiter4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaMercuryFromRadioButtons()
        {
            if (radioButtonMercury1.Checked)
                return 1;
            if (radioButtonMercury2.Checked)
                return 2;
            if (radioButtonMercury3.Checked)
                return 3;
            if (radioButtonMercury4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaMarsFromRadioButtons()
        {
            if (radioButtonMars1.Checked)
                return 1;
            if (radioButtonMars2.Checked)
                return 2;
            if (radioButtonMars3.Checked)
                return 3;
            if (radioButtonMars4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaSaturnFromRadioButtons()
        {
            if (radioButtonSaturn1.Checked)
                return 1;
            if (radioButtonSaturn2.Checked)
                return 2;
            if (radioButtonSaturn3.Checked)
                return 3;
            if (radioButtonSaturn4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaRahuFromRadioButtons()
        {
            if (radioButtonRahu1.Checked)
                return 1;
            if (radioButtonRahu2.Checked)
                return 2;
            if (radioButtonRahu3.Checked)
                return 3;
            if (radioButtonRahu4.Checked)
                return 4;
            return 0;
        }

        private int GetPadaKetuFromRadioButtons()
        {
            if (radioButtonKetu1.Checked)
                return 1;
            if (radioButtonKetu2.Checked)
                return 2;
            if (radioButtonKetu3.Checked)
                return 3;
            if (radioButtonKetu4.Checked)
                return 4;
            return 0;
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
                SelectedProfile = CacheLoad._profileList.Where(i => i.Id == selectedProfileId).FirstOrDefault();
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
                foreach (Profile p in _proList)
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
            comboBoxNakshatraMoon.SelectedIndex = -1;
            comboBoxNakshatraLagna.SelectedIndex = -1;
            comboBoxNakshatraSun.SelectedIndex = -1;
            comboBoxNakshatraVenus.SelectedIndex = -1;
            comboBoxNakshatraJupiter.SelectedIndex = -1;
            comboBoxNakshatraMercury.SelectedIndex = -1;
            comboBoxNakshatraMars.SelectedIndex = -1;
            comboBoxNakshatraSaturn.SelectedIndex = -1;
            comboBoxNakshatraRahu.SelectedIndex = -1;
            comboBoxNakshatraKetu.SelectedIndex = -1;
            CleanRadioButtons();
        }

        private void CleanRadioButtons()
        {
            radioButtonMoon1.Checked = false;
            radioButtonMoon2.Checked = false;
            radioButtonMoon3.Checked = false;
            radioButtonMoon4.Checked = false;

            radioButtonLagna1.Checked = false;
            radioButtonLagna2.Checked = false;
            radioButtonLagna3.Checked = false;
            radioButtonLagna4.Checked = false;

            radioButtonSun1.Checked = false;
            radioButtonSun2.Checked = false;
            radioButtonSun3.Checked = false;
            radioButtonSun4.Checked = false;

            radioButtonVenus1.Checked = false;
            radioButtonVenus2.Checked = false;
            radioButtonVenus3.Checked = false;
            radioButtonVenus4.Checked = false;

            radioButtonJupiter1.Checked = false;
            radioButtonJupiter2.Checked = false;
            radioButtonJupiter3.Checked = false;
            radioButtonJupiter4.Checked = false;

            radioButtonMercury1.Checked = false;
            radioButtonMercury2.Checked = false;
            radioButtonMercury3.Checked = false;
            radioButtonMercury4.Checked = false;

            radioButtonMars1.Checked = false;
            radioButtonMars2.Checked = false;
            radioButtonMars3.Checked = false;
            radioButtonMars4.Checked = false;

            radioButtonSaturn1.Checked = false;
            radioButtonSaturn2.Checked = false;
            radioButtonSaturn3.Checked = false;
            radioButtonSaturn4.Checked = false;

            radioButtonRahu1.Checked = false;
            radioButtonRahu2.Checked = false;
            radioButtonRahu3.Checked = false;
            radioButtonRahu4.Checked = false;

            radioButtonKetu1.Checked = false;
            radioButtonKetu2.Checked = false;
            radioButtonKetu3.Checked = false;
            radioButtonKetu4.Checked = false;
        }

        private void MakeTextFieldsEditable(bool isUpdate)
        {
            textBoxProfileName.ReadOnly = false;
            textBoxPersonName.ReadOnly = false;
            textBoxPersonSurname.ReadOnly = false;
            buttonLivingPlace.Enabled = true;

            comboBoxNakshatraMoon.Enabled = true;
            groupBoxPadaMoon.Enabled = true;

            comboBoxNakshatraLagna.Enabled = true;
            groupBoxPadaLagna.Enabled = true;

            comboBoxNakshatraSun.Enabled = true;
            groupBoxPadaSun.Enabled = true;

            comboBoxNakshatraVenus.Enabled = true;
            groupBoxPadaVenus.Enabled = true;
            
            comboBoxNakshatraJupiter.Enabled = true;
            groupBoxPadaJupiter.Enabled = true;

            comboBoxNakshatraMercury.Enabled = true;
            groupBoxPadaMercury.Enabled = true;

            comboBoxNakshatraMars.Enabled = true;
            groupBoxPadaMars.Enabled = true;

            comboBoxNakshatraSaturn.Enabled = true;
            groupBoxPadaSaturn.Enabled = true;
            
            comboBoxNakshatraRahu.Enabled = true;
            groupBoxPadaRahu.Enabled = true;

            comboBoxNakshatraKetu.Enabled = true;
            groupBoxPadaKetu.Enabled = true;

            if (SelectedProfile == null || !isUpdate)
            {
                comboBoxNakshatraMoon.SelectedIndex = 0;
                comboBoxNakshatraLagna.SelectedIndex = 0;
                comboBoxNakshatraSun.SelectedIndex = 0;
                comboBoxNakshatraVenus.SelectedIndex = 0;
                comboBoxNakshatraJupiter.SelectedIndex = 0;
                comboBoxNakshatraMercury.SelectedIndex = 0;
                comboBoxNakshatraMars.SelectedIndex = 0;
                comboBoxNakshatraSaturn.SelectedIndex = 0;
                comboBoxNakshatraRahu.SelectedIndex = 0;
                comboBoxNakshatraKetu.SelectedIndex = 0;

                radioButtonMoon1.Checked = true;
                radioButtonLagna1.Checked = true;
                radioButtonSun1.Checked = true;
                radioButtonVenus1.Checked = true;
                radioButtonJupiter1.Checked = true;
                radioButtonMercury1.Checked = true;
                radioButtonMars1.Checked = true;
                radioButtonSaturn1.Checked = true;
                radioButtonRahu1.Checked = true;
                radioButtonKetu1.Checked = true;
            }
            else if (SelectedProfile != null && isUpdate)
            {
                comboBoxNakshatraMoon.SelectedIndex = SelectedProfile.NakshatraMoonId - 1;
                comboBoxNakshatraLagna.SelectedIndex = SelectedProfile.NakshatraLagnaId - 1;
                comboBoxNakshatraSun.SelectedIndex = SelectedProfile.NakshatraSunId - 1;
                comboBoxNakshatraVenus.SelectedIndex = SelectedProfile.NakshatraVenusId - 1;
                comboBoxNakshatraJupiter.SelectedIndex = SelectedProfile.NakshatraJupiterId - 1;
                comboBoxNakshatraMercury.SelectedIndex = SelectedProfile.NakshatraMercuryId - 1;
                comboBoxNakshatraMars.SelectedIndex = SelectedProfile.NakshatraMarsId - 1;
                comboBoxNakshatraSaturn.SelectedIndex = SelectedProfile.NakshatraSaturnId - 1;
                comboBoxNakshatraRahu.SelectedIndex = SelectedProfile.NakshatraRahuId - 1;
                comboBoxNakshatraKetu.SelectedIndex = SelectedProfile.NakshatraKetuId - 1;

                SetPadaMoonRadioButton(SelectedProfile.PadaMoon);
                SetPadaLagnaRadioButton(SelectedProfile.PadaLagna);
                SetPadaSunRadioButton(SelectedProfile.PadaSun);
                SetPadaVenusRadioButton(SelectedProfile.PadaVenus);
                SetPadaJupiterRadioButton(SelectedProfile.PadaJupiter);
                SetPadaMercuryRadioButton(SelectedProfile.PadaMercury);
                SetPadaMarsRadioButton(SelectedProfile.PadaMars);
                SetPadaSaturnRadioButton(SelectedProfile.PadaSaturn);
                SetPadaRahuRadioButton(SelectedProfile.PadaRahu);
                SetPadaKetuRadioButton(SelectedProfile.PadaKetu);
            }

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

            comboBoxNakshatraMoon.Enabled = false;
            comboBoxNakshatraMoon.SelectedIndex = -1;
            groupBoxPadaMoon.Enabled = false;

            comboBoxNakshatraLagna.Enabled = false;
            comboBoxNakshatraLagna.SelectedIndex = -1;
            groupBoxPadaLagna.Enabled = false;

            comboBoxNakshatraSun.Enabled = false;
            comboBoxNakshatraSun.SelectedIndex = -1;
            groupBoxPadaSun.Enabled = false;

            comboBoxNakshatraVenus.Enabled = false;
            comboBoxNakshatraVenus.SelectedIndex = -1;
            groupBoxPadaVenus.Enabled = false;

            comboBoxNakshatraJupiter.Enabled = false;
            comboBoxNakshatraJupiter.SelectedIndex = -1;
            groupBoxPadaJupiter.Enabled = false;

            comboBoxNakshatraMercury.Enabled = false;
            comboBoxNakshatraMercury.SelectedIndex = -1;
            groupBoxPadaMercury.Enabled = false;

            comboBoxNakshatraMars.Enabled = false;
            comboBoxNakshatraMars.SelectedIndex = -1;
            groupBoxPadaMars.Enabled = false;

            comboBoxNakshatraSaturn.Enabled = false;
            comboBoxNakshatraSaturn.SelectedIndex = -1;
            groupBoxPadaSaturn.Enabled = false;

            comboBoxNakshatraRahu.Enabled = false;
            comboBoxNakshatraRahu.SelectedIndex = -1;
            groupBoxPadaRahu.Enabled = false;

            comboBoxNakshatraKetu.Enabled = false;
            comboBoxNakshatraKetu.SelectedIndex = -1;
            groupBoxPadaKetu.Enabled = false;

            CleanRadioButtons();

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
            _selectedLocation = lForm.SelectedLocation;
            textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == _selectedLocation.Id).FirstOrDefault()?.Locality ?? string.Empty;
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
                CacheLoad._profileList.RemoveAll(i => i.Id == selectedProfileId);
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
                frmShowMessage.Show(Utility.GetLocalizedText("Enter profile name", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
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

            CacheLoad._profileList = CacheLoad.GetProfileList();
            _proList = CacheLoad._profileList.ToList();
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
            KeyValueData selectedNakshatraItem = (KeyValueData)comboBoxNakshatraMoon.SelectedItem;
            KeyValueData selectedNakshatraLagnaItem = (KeyValueData)comboBoxNakshatraLagna.SelectedItem;
            KeyValueData selectedNakshatraSunItem = (KeyValueData)comboBoxNakshatraSun.SelectedItem;
            KeyValueData selectedNakshatraVenusItem = (KeyValueData)comboBoxNakshatraVenus.SelectedItem;
            KeyValueData selectedNakshatraJupiterItem = (KeyValueData)comboBoxNakshatraJupiter.SelectedItem;
            KeyValueData selectedNakshatraMercuryItem = (KeyValueData)comboBoxNakshatraMercury.SelectedItem;
            KeyValueData selectedNakshatraMarsItem = (KeyValueData)comboBoxNakshatraMars.SelectedItem;
            KeyValueData selectedNakshatraSaturnItem = (KeyValueData)comboBoxNakshatraSaturn.SelectedItem;
            KeyValueData selectedNakshatraRahuItem = (KeyValueData)comboBoxNakshatraRahu.SelectedItem;
            KeyValueData selectedNakshatraKetuItem = (KeyValueData)comboBoxNakshatraKetu.SelectedItem;

            Profile newProfile = new Profile
            {
                ProfileName = textBoxProfileName.Text,
                PersonName = textBoxPersonName.Text,
                PersonSurname = textBoxPersonSurname.Text,
                PlaceOfLivingId = CacheLoad._locationList.Where(i => i.Id == _selectedLocation.Id).FirstOrDefault()?.Id ?? 0,
                NakshatraMoonId = selectedNakshatraItem.ItemId,
                PadaMoon = GetPadaMoonFromRadioButtons(),
                NakshatraLagnaId = selectedNakshatraLagnaItem.ItemId,
                PadaLagna = GetPadaLagnaFromRadioButtons(),
                NakshatraSunId = selectedNakshatraSunItem.ItemId,
                PadaSun = GetPadaSunFromRadioButtons(),
                NakshatraVenusId = selectedNakshatraVenusItem.ItemId,
                PadaVenus = GetPadaVenusFromRadioButtons(),
                NakshatraJupiterId = selectedNakshatraJupiterItem.ItemId,
                PadaJupiter = GetPadaJupiterFromRadioButtons(),
                NakshatraMercuryId = selectedNakshatraMercuryItem.ItemId,
                PadaMercury = GetPadaMercuryFromRadioButtons(),
                NakshatraMarsId = selectedNakshatraMarsItem.ItemId,
                PadaMars = GetPadaMarsFromRadioButtons(),
                NakshatraSaturnId = selectedNakshatraSaturnItem.ItemId,
                PadaSaturn = GetPadaSaturnFromRadioButtons(),
                NakshatraRahuId = selectedNakshatraRahuItem.ItemId,
                PadaRahu = GetPadaRahuFromRadioButtons(),
                NakshatraKetuId = selectedNakshatraKetuItem.ItemId,
                PadaKetu = GetPadaKetuFromRadioButtons(),
                IsChecked = false
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("insert into PROFILE (PROFILENAME, PERSONNAME, PERSONSURNAME, PLACEOFLIVINGID, NAKSHATRAMOONID, PADAMOON, NAKSHATRALAGNAID, PADALAGNA, NAKSHATRASUNID, PADASUN, NAKSHATRAVENUSID, PADAVENUS, NAKSHATRAJUPITERID, PADAJUPITER, NAKSHATRAMERCURYID, PADAMERCURY, NAKSHATRAMARSID, PADAMARS, NAKSHATRASATURNID, PADASATURN, NAKSHATRARAHUID, PADARAHU, NAKSHATRAKETUID, PADAKETU, CHECKED, GUID) values (@PROFILENAME, @PERSONNAME, @PERSONSURNAME, @PLACEOFLIVINGID, @NAKSHATRAMOONID, @PADAMOON, @NAKSHATRALAGNAID, @PADALAGNA, @NAKSHATRASUNID, @PADASUN, @NAKSHATRAVENUSID, @PADAVENUS, @NAKSHATRAJUPITERID, @PADAJUPITER, @NAKSHATRAMERCURYID, @PADAMERCURY, @NAKSHATRAMARSID, @PADAMARS, @NAKSHATRASATURNID, @PADASATURN, @NAKSHATRARAHUID, @PADARAHU, @NAKSHATRAKETUID, @PADAKETU, @CHECKED, @GUID)", dbCon);
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
            KeyValueData selectedNakshatraMoonItem = (KeyValueData)comboBoxNakshatraMoon.SelectedItem;
            KeyValueData selectedNakshatraLagnaItem = (KeyValueData)comboBoxNakshatraLagna.SelectedItem;
            KeyValueData selectedNakshatraSunItem = (KeyValueData)comboBoxNakshatraSun.SelectedItem;
            KeyValueData selectedNakshatraVenusItem = (KeyValueData)comboBoxNakshatraVenus.SelectedItem;
            KeyValueData selectedNakshatraJupiterItem = (KeyValueData)comboBoxNakshatraJupiter.SelectedItem;
            KeyValueData selectedNakshatraMercuryItem = (KeyValueData)comboBoxNakshatraMercury.SelectedItem;
            KeyValueData selectedNakshatraMarsItem = (KeyValueData)comboBoxNakshatraMars.SelectedItem;
            KeyValueData selectedNakshatraSaturnItem = (KeyValueData)comboBoxNakshatraSaturn.SelectedItem;
            KeyValueData selectedNakshatraRahuItem = (KeyValueData)comboBoxNakshatraRahu.SelectedItem;
            KeyValueData selectedNakshatraKetuItem = (KeyValueData)comboBoxNakshatraKetu.SelectedItem;

            int locationId = SelectedProfile.PlaceOfLivingId;
            if (_selectedLocation != null)
                locationId = _selectedLocation.Id;

            Profile newProfile = new Profile
            {
                ProfileName = textBoxProfileName.Text,
                PersonName = textBoxPersonName.Text,
                PersonSurname = textBoxPersonSurname.Text,
                PlaceOfLivingId = CacheLoad._locationList.Where(i => i.Id == locationId).FirstOrDefault()?.Id ?? 0,
                NakshatraMoonId = selectedNakshatraMoonItem.ItemId,
                PadaMoon = GetPadaMoonFromRadioButtons(),
                NakshatraLagnaId = selectedNakshatraLagnaItem.ItemId,
                PadaLagna = GetPadaLagnaFromRadioButtons(),
                NakshatraSunId = selectedNakshatraSunItem.ItemId,
                PadaSun = GetPadaSunFromRadioButtons(),
                NakshatraVenusId = selectedNakshatraVenusItem.ItemId,
                PadaVenus = GetPadaVenusFromRadioButtons(),
                NakshatraJupiterId = selectedNakshatraJupiterItem.ItemId,
                PadaJupiter = GetPadaJupiterFromRadioButtons(),
                NakshatraMercuryId = selectedNakshatraMercuryItem.ItemId,
                PadaMercury = GetPadaMercuryFromRadioButtons(),
                NakshatraMarsId = selectedNakshatraMarsItem.ItemId,
                PadaMars = GetPadaMarsFromRadioButtons(),
                NakshatraSaturnId = selectedNakshatraSaturnItem.ItemId,
                PadaSaturn = GetPadaSaturnFromRadioButtons(),
                NakshatraRahuId = selectedNakshatraRahuItem.ItemId,
                PadaRahu = GetPadaRahuFromRadioButtons(),
                NakshatraKetuId = selectedNakshatraKetuItem.ItemId,
                PadaKetu = GetPadaKetuFromRadioButtons()
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update PROFILE set PROFILENAME = @PROFILENAME, PERSONNAME = @PERSONNAME, PERSONSURNAME = @PERSONSURNAME, PLACEOFLIVINGID = @PLACEOFLIVINGID, NAKSHATRAMOONID = @NAKSHATRAMOONID, PADAMOON = @PADAMOON, NAKSHATRALAGNAID = @NAKSHATRALAGNAID, PADALAGNA = @PADALAGNA, NAKSHATRASUNID = @NAKSHATRASUNID, PADASUN = @PADASUN, NAKSHATRAVENUSID = @NAKSHATRAVENUSID, PADAVENUS = @PADAVENUS, NAKSHATRAJUPITERID = @NAKSHATRAJUPITERID, PADAJUPITER = @PADAJUPITER, NAKSHATRAMERCURYID = @NAKSHATRAMERCURYID, PADAMERCURY = @PADAMERCURY, NAKSHATRAMARSID = @NAKSHATRAMARSID, PADAMARS = @PADAMARS, NAKSHATRASATURNID = @NAKSHATRASATURNID, PADASATURN = @PADASATURN, NAKSHATRARAHUID = @NAKSHATRARAHUID, PADARAHU = @PADARAHU, NAKSHATRAKETUID = @NAKSHATRAKETUID, PADAKETU = @PADAKETU where ID = @ID", dbCon);
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
                    command = new SQLiteCommand("update PROFILE set CHECKED = 0", dbCon);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("update PROFILE set CHECKED = 1 where ID = @ID", dbCon);
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
                        command = new SQLiteCommand("delete from PROFILE where ID = @ID", dbCon);
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

                CacheLoad._profileList = CacheLoad.GetProfileList();
                _proList = CacheLoad._profileList.ToList();
                FillProfileListViewByData(_proList);

                CleanFields();
                MakeTextFieldsReadOnly();
                textBoxSearch_TextChanged(sender, e);
                textBoxSearch.Focus();
            }
        }

        
    }
}
