using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace PAD
{
    public partial class AppSettings : Form
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

        private bool _isChanged;
        public bool IsChanged
        {
            get { return _isChanged; }
            set { _isChanged = value; }
        }

        private List<AppSettingList> _appSetList;
        private ELanguage _activeLang;
        private EAppSetting _activeLanguageSetting;
        private EAppSetting _newSelectedLanguage;
        private EAppSetting _newSelectedTranzitMode;
        private EAppSetting _newSelectedHoraMode;
        private EAppSetting _newSelectedMGMode;
        private EAppSetting _newSelectedMBMode;
        private EAppSetting _newSelectedNodeMode;
        private EAppSetting _newSelectedWeekMode;
        private bool _langChanged;
        private bool _tranChanged;
        private bool _horaChanged;
        private bool _mgChanged;
        private bool _mbChanged;
        private bool _nodeChanged;
        private bool _weekChanged;

        public AppSettings()
        {
            InitializeComponent();
        }
        
        public AppSettings(List<AppSettingList> appSetList, ELanguage aLang)
        {
            InitializeComponent();

            _isChanged = false;
            _langChanged = false;
            _tranChanged = false;
            _horaChanged = false;
            _mgChanged = false;
            _mbChanged = false;
            _nodeChanged = false;
            _weekChanged = false;
            _appSetList = appSetList;
            _activeLang = aLang;
            _activeLanguageSetting = Utility.GetActiveLanguageCode(_appSetList);
            _newSelectedLanguage = _activeLanguageSetting;
        }
        
        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (_langChanged || _tranChanged || _horaChanged || _mgChanged || _mbChanged || _nodeChanged || _weekChanged)
            {
                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Configuration settings has been changed. Do you want to apply new settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ApplyingChanges();
                    if (_newSelectedLanguage != _activeLanguageSetting)
                    {
                        ApllyNewLanguage();
                        DialogResult dialogResultLang = frmShowMessage.Show(Utility.GetLocalizedText("In order to apply changes application has to be restarted. Do you want to restart application now?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                        if (dialogResultLang == DialogResult.Yes)
                        {
                            Application.Restart();
                            Environment.Exit(0);
                        }
                    }
                }
                IsChanged = true;
                Close();
            }
            else
            {
                Close();
            }
        }

        private void AppSettings_Shown(object sender, EventArgs e)
        {
            SetFlagBackColor();
            PrepareCheckboxes();
            Utility.LocalizeForm(this, _activeLang);
        }

        private void SetFlagBackColor()
        {
            switch (_activeLanguageSetting)
            {
                case EAppSetting.ENGLISH:
                    checkBoxEnlish.BackColor = Color.DeepSkyBlue;
                    checkBoxRussian.BackColor = Color.LightGray;
                    break;

                case EAppSetting.RUSSIAN:
                    checkBoxEnlish.BackColor = Color.LightGray;
                    checkBoxRussian.BackColor = Color.DeepSkyBlue;
                    break;
            }
        }
        
        private void PrepareCheckboxes()
        {
            EAppSetting activeTranzit = Utility.GetActiveTranzitMode(_appSetList);
            EAppSetting activeHora = Utility.GetActiveHoraMode(_appSetList);
            EAppSetting activeMG = Utility.GetActiveMuhurtaGhatiMode(_appSetList);
            EAppSetting activeMB = Utility.GetActiveMrityuBhagaMode(_appSetList);
            EAppSetting activeNode = Utility.GetActiveNodeMode(_appSetList);
            EAppSetting activeWeek = Utility.GetActiveWeekMode(_appSetList);
            switch (activeTranzit)
            {
                case EAppSetting.TRANZITMOON:
                    checkBoxMoon.Checked = true;
                    break;
                case EAppSetting.TRANZITLAGNA:
                    checkBoxLagna.Checked = true;
                    break;
                case EAppSetting.TRANZITMOONANDLAGNA:
                    checkBoxBoth.Checked = true;
                    break;
            }
            switch (activeHora)
            {
                case EAppSetting.HORADAYNIGHT:
                    checkBoxHoraSunRise.Checked = true;
                    break;
                case EAppSetting.HORAEQUAL:
                    checkBoxHoraEqual.Checked = true;
                    break;
                case EAppSetting.HORAFROM6:
                    checkBoxHoraFrom6.Checked = true;
                    break;
            }
            switch (activeMG)
            {
                case EAppSetting.MUHURTAGHATIDAYNIGHT:
                    checkBoxMuhurtsSunRise.Checked = true;
                    break;
                case EAppSetting.MUHURTAGHATIEQUAL:
                    checkBoxMuhurtsEqual.Checked = true;
                    break;
                case EAppSetting.MUHURTAGHATIFROM6:
                    checkBoxMuhurtsFrom6.Checked = true;
                    break;
            }
            switch (activeMB)
            {
                case EAppSetting.MRITYUBHAGANEQUAL:
                    checkBoxMrityuEqual.Checked = true;
                    break;
                case EAppSetting.MRITYUBHAGANLESS:
                    checkBoxMrityuLess.Checked = true;
                    break;
                case EAppSetting.MRITYUBHAGANMORE:
                    checkBoxMrityuMore.Checked = true;
                    break;

                case EAppSetting.MRITYUBHAGAERNST:
                    checkBoxMrityuErnst.Checked = true;
                    break;
            }
            switch (activeNode)
            {
                case EAppSetting.NODEMEAN:
                    checkBoxNodeMean.Checked = true;
                    break;
                case EAppSetting.NODETRUE:
                    checkBoxNodeTrue.Checked = true;
                    break;
            }
            switch (activeWeek)
            {
                case EAppSetting.WEEKSUNDAY:
                    checkBoxWeekSunday.Checked = true;
                    break;
                case EAppSetting.WEEKMONDAY:
                    checkBoxWeekMonday.Checked = true;
                    break;
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            DialogResult dialogResultLang = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to revert default settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResultLang == DialogResult.Yes)
            {
                UpdateAppSetting(EAppSettingList.LANGUAGE, EAppSetting.ENGLISH);
                UpdateAppSetting(EAppSettingList.TRANZIT, EAppSetting.TRANZITMOON);
                UpdateAppSetting(EAppSettingList.HORA, EAppSetting.HORAEQUAL);
                UpdateAppSetting(EAppSettingList.MUHURTAGHATI, EAppSetting.MUHURTAGHATIEQUAL);
                UpdateAppSetting(EAppSettingList.MRITYUBHAGA, EAppSetting.MRITYUBHAGANEQUAL);
                UpdateAppSetting(EAppSettingList.NODE, EAppSetting.NODEMEAN);
                UpdateAppSetting(EAppSettingList.WEEK, EAppSetting.WEEKSUNDAY);
                CacheLoad._appSettingList = CacheLoad.GetAppSettingsList();

                DialogResult dialogRestartResultLang = frmShowMessage.Show(Utility.GetLocalizedText("In order to apply changes application has to be restarted. Do you want to restart application now?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogRestartResultLang == DialogResult.Yes)
                {
                    Application.Restart();
                    Environment.Exit(0);
                }
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplyingChanges();
            CacheLoad._appSettingList = CacheLoad.GetAppSettingsList();
            if (_newSelectedLanguage != _activeLanguageSetting)
            {
                ApllyNewLanguage();
                DialogResult dialogResultLang = frmShowMessage.Show(Utility.GetLocalizedText("In order to apply changes application has to be restarted. Do you want to restart application now?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResultLang == DialogResult.Yes)
                {
                    Application.Restart();
                    Environment.Exit(0);
                }
            }
            IsChanged = true;
            frmShowMessage.Show(Utility.GetLocalizedText("Changes has been applied.", _activeLang), Utility.GetLocalizedText("Information", _activeLang), enumMessageIcon.Information, enumMessageButton.OK);
            Close();
        }

        private void ApllyNewLanguage()
        {
            if (_langChanged)
            {
                UpdateAppSetting(EAppSettingList.LANGUAGE, _newSelectedLanguage);
            }
        }

        private void ApplyingChanges()
        {
            if (_tranChanged)
            {
                _newSelectedTranzitMode = GetSelectedTranzitMode();
                UpdateAppSetting(EAppSettingList.TRANZIT, _newSelectedTranzitMode);
            }
            if (_horaChanged)
            {
                _newSelectedHoraMode = GetSelectedHoraMode();
                UpdateAppSetting(EAppSettingList.HORA, _newSelectedHoraMode);
            }
            if (_mgChanged)
            {
                _newSelectedMGMode = GetSelectedMuhurtaGhatiMode();
                UpdateAppSetting(EAppSettingList.MUHURTAGHATI, _newSelectedMGMode);
            }
            if (_mbChanged)
            {
                _newSelectedMBMode = GetSelectedMrityuBhagaMode();
                UpdateAppSetting(EAppSettingList.MRITYUBHAGA, _newSelectedMBMode);
            }
            if (_nodeChanged)
            {
                _newSelectedNodeMode = GetSelecteNodeMode();
                UpdateAppSetting(EAppSettingList.NODE, _newSelectedNodeMode);
            }
            if (_weekChanged)
            {
                _newSelectedWeekMode = GetSelecteWeekMode();
                UpdateAppSetting(EAppSettingList.WEEK, _newSelectedWeekMode);
            }
        }

        private void UpdateAppSetting(EAppSettingList appGroup, EAppSetting appSet)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                SQLiteCommand command;
                try
                {
                    command = new SQLiteCommand("update APPSETTING set ACTIVE = 0 where GROUPCODE = @GROUPCODE", dbCon);
                    command.Parameters.AddWithValue("@GROUPCODE", appGroup.ToString());
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("update APPSETTING set ACTIVE = 1 where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@ID", appSet);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private EAppSetting GetSelectedTranzitMode()
        {
            if (checkBoxMoon.Checked)
                return EAppSetting.TRANZITMOON;
            if (checkBoxLagna.Checked)
                return EAppSetting.TRANZITLAGNA;
            if (checkBoxBoth.Checked)
                return EAppSetting.TRANZITMOONANDLAGNA;

            return EAppSetting.TRANZITMOON;
        }

        private EAppSetting GetSelectedHoraMode()
        {
            if (checkBoxHoraSunRise.Checked)
                return EAppSetting.HORADAYNIGHT;
            if (checkBoxHoraEqual.Checked)
                return EAppSetting.HORAEQUAL;
            if (checkBoxHoraFrom6.Checked)
                return EAppSetting.HORAFROM6;

            return EAppSetting.HORAEQUAL;
        }

        private EAppSetting GetSelectedMuhurtaGhatiMode()
        {
            if (checkBoxMuhurtsSunRise.Checked)
                return EAppSetting.MUHURTAGHATIDAYNIGHT;
            if (checkBoxMuhurtsEqual.Checked)
                return EAppSetting.MUHURTAGHATIEQUAL;
            if (checkBoxMuhurtsFrom6.Checked)
                return EAppSetting.MUHURTAGHATIFROM6;

            return EAppSetting.MUHURTAGHATIEQUAL;
        }

        private EAppSetting GetSelectedMrityuBhagaMode()
        {
            if (checkBoxMrityuEqual.Checked)
                return EAppSetting.MRITYUBHAGANEQUAL;
            if (checkBoxMrityuLess.Checked)
                return EAppSetting.MRITYUBHAGANLESS;
            if (checkBoxMrityuMore.Checked)
                return EAppSetting.MRITYUBHAGANMORE;
            if (checkBoxMrityuErnst.Checked)
                return EAppSetting.MRITYUBHAGAERNST;

            return EAppSetting.MRITYUBHAGANEQUAL;
        }

        private EAppSetting GetSelecteNodeMode()
        {
            if (checkBoxNodeMean.Checked)
                return EAppSetting.NODEMEAN;
            if (checkBoxNodeTrue.Checked)
                return EAppSetting.NODETRUE;

            return EAppSetting.NODEMEAN;
        }

        private EAppSetting GetSelecteWeekMode()
        {
            if (checkBoxWeekSunday.Checked)
                return EAppSetting.WEEKSUNDAY;
            if (checkBoxWeekMonday.Checked)
                return EAppSetting.WEEKMONDAY;

            return EAppSetting.WEEKSUNDAY;
        }

        private void CheckTranzitCheckboxDefault()
        {
            if (!checkBoxMoon.Checked && !checkBoxLagna.Checked && !checkBoxBoth.Checked)
            {
                checkBoxMoon.Checked = true;
                checkBoxLagna.Checked = false;
                checkBoxBoth.Checked = false;
            }
        }

        private void checkBoxMoon_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMoon.Checked)
            {
                checkBoxLagna.Checked = false;
                checkBoxBoth.Checked = false;
            }
            CheckTranzitCheckboxDefault();

            if (checkBoxMoon.Checked)
            {
                if (Utility.GetActiveTranzitMode(_appSetList) != EAppSetting.TRANZITMOON)
                {
                    _tranChanged = true;
                }
            }
        }

        private void checkBoxLagna_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLagna.Checked)
            {
                checkBoxMoon.Checked = false;
                checkBoxBoth.Checked = false;
            }
            CheckTranzitCheckboxDefault();

            if (checkBoxLagna.Checked)
            {
                if (Utility.GetActiveTranzitMode(_appSetList) != EAppSetting.TRANZITLAGNA)
                {
                    _tranChanged = true;
                }
            }
        }

        private void checkBoxBoth_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBoth.Checked)
            {
                checkBoxMoon.Checked = false;
                checkBoxLagna.Checked = false;
            }
            CheckTranzitCheckboxDefault();

            if (checkBoxBoth.Checked)
            {
                if (Utility.GetActiveTranzitMode(_appSetList) != EAppSetting.TRANZITMOONANDLAGNA)
                {
                    _tranChanged = true;
                }
            }
        }

        private void CheckHoraCheckboxDefault()
        {
            if (!checkBoxHoraSunRise.Checked && !checkBoxHoraEqual.Checked && !checkBoxHoraFrom6.Checked)
            {
                checkBoxHoraSunRise.Checked = false;
                checkBoxHoraEqual.Checked = true;
                checkBoxHoraFrom6.Checked = false;
            }
        }

        private void checkBoxHoraSunRise_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHoraSunRise.Checked)
            {
                checkBoxHoraEqual.Checked = false;
                checkBoxHoraFrom6.Checked = false;
            }
            CheckHoraCheckboxDefault();

            if (checkBoxHoraSunRise.Checked)
            {
                if (Utility.GetActiveHoraMode(_appSetList) != EAppSetting.HORADAYNIGHT)
                {
                    _horaChanged = true;
                }
            }
        }

        private void checkBoxHoraEqual_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHoraEqual.Checked)
            {
                checkBoxHoraSunRise.Checked = false;
                checkBoxHoraFrom6.Checked = false;
            }
            CheckHoraCheckboxDefault();

            if (checkBoxHoraEqual.Checked)
            {
                if (Utility.GetActiveHoraMode(_appSetList) != EAppSetting.HORAEQUAL)
                {
                    _horaChanged = true;
                }
            }
        }

        private void checkBoxHoraFrom6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHoraFrom6.Checked)
            {
                checkBoxHoraSunRise.Checked = false;
                checkBoxHoraEqual.Checked = false;
            }
            CheckHoraCheckboxDefault();

            if (checkBoxHoraFrom6.Checked)
            {
                if (Utility.GetActiveHoraMode(_appSetList) != EAppSetting.HORAFROM6)
                {
                    _horaChanged = true;
                }
            }
        }

        private void CheckMuhurtaCheckboxDefault()
        {
            if (!checkBoxMuhurtsSunRise.Checked && !checkBoxMuhurtsEqual.Checked && !checkBoxMuhurtsFrom6.Checked)
            {
                checkBoxMuhurtsSunRise.Checked = false;
                checkBoxMuhurtsEqual.Checked = true;
                checkBoxMuhurtsFrom6.Checked = false;
            }
        }

        private void checkBoxMuhurtsSunRise_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMuhurtsSunRise.Checked)
            {
                checkBoxMuhurtsEqual.Checked = false;
                checkBoxMuhurtsFrom6.Checked = false;
            }
            CheckMuhurtaCheckboxDefault();

            if (checkBoxMuhurtsSunRise.Checked)
            {
                if (Utility.GetActiveMuhurtaGhatiMode(_appSetList) != EAppSetting.MUHURTAGHATIDAYNIGHT)
                {
                    _mgChanged = true;
                }
            }
        }

        private void checkBoxMuhurtsEqual_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMuhurtsEqual.Checked)
            {
                checkBoxMuhurtsSunRise.Checked = false;
                checkBoxMuhurtsFrom6.Checked = false;
            }
            CheckMuhurtaCheckboxDefault();

            if (checkBoxMuhurtsEqual.Checked)
            {
                if (Utility.GetActiveMuhurtaGhatiMode(_appSetList) != EAppSetting.MUHURTAGHATIEQUAL)
                {
                    _mgChanged = true;
                }
            }
        }

        private void checkBoxMuhurtsFrom6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMuhurtsFrom6.Checked)
            {
                checkBoxMuhurtsSunRise.Checked = false;
                checkBoxMuhurtsEqual.Checked = false;
            }
            CheckMuhurtaCheckboxDefault();

            if (checkBoxMuhurtsFrom6.Checked)
            {
                if (Utility.GetActiveMuhurtaGhatiMode(_appSetList) != EAppSetting.MUHURTAGHATIFROM6)
                {
                    _mgChanged = true;
                }
            }
        }

        private void CheckMrityuBhagaCheckboxDefault()
        {
            if (!checkBoxMrityuEqual.Checked && !checkBoxMrityuLess.Checked && !checkBoxMrityuMore.Checked && !checkBoxMrityuErnst.Checked)
            {
                checkBoxMrityuEqual.Checked = true;
                checkBoxMrityuLess.Checked = false;
                checkBoxMrityuMore.Checked = false;
                checkBoxMrityuErnst.Checked = false;
            }
        }

        private void checkBoxMrityuEqual_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMrityuEqual.Checked)
            {
                checkBoxMrityuLess.Checked = false;
                checkBoxMrityuMore.Checked = false;
                checkBoxMrityuErnst.Checked = false;
            }
            CheckMrityuBhagaCheckboxDefault();

            if (checkBoxMrityuEqual.Checked)
            {
                if (Utility.GetActiveMrityuBhagaMode(_appSetList) != EAppSetting.MRITYUBHAGANEQUAL)
                {
                    _mbChanged = true;
                }
            }
        }

        private void checkBoxMrityuLess_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMrityuLess.Checked)
            {
                checkBoxMrityuEqual.Checked = false;
                checkBoxMrityuMore.Checked = false;
                checkBoxMrityuErnst.Checked = false;
            }
            CheckMrityuBhagaCheckboxDefault();

            if (checkBoxMrityuLess.Checked)
            {
                if (Utility.GetActiveMrityuBhagaMode(_appSetList) != EAppSetting.MRITYUBHAGANLESS)
                {
                    _mbChanged = true;
                }
            }
        }

        private void checkBoxMrityuMore_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMrityuMore.Checked)
            {
                checkBoxMrityuEqual.Checked = false;
                checkBoxMrityuLess.Checked = false;
                checkBoxMrityuErnst.Checked = false;
            }
            CheckMrityuBhagaCheckboxDefault();

            if (checkBoxMrityuMore.Checked)
            {
                if (Utility.GetActiveMrityuBhagaMode(_appSetList) != EAppSetting.MRITYUBHAGANMORE)
                {
                    _mbChanged = true;
                }
            }
        }

        private void checkBoxMrityuErnst_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMrityuErnst.Checked)
            {
                checkBoxMrityuEqual.Checked = false;
                checkBoxMrityuLess.Checked = false;
                checkBoxMrityuMore.Checked = false;
            }
            CheckMrityuBhagaCheckboxDefault();

            if (checkBoxMrityuErnst.Checked)
            {
                if (Utility.GetActiveMrityuBhagaMode(_appSetList) != EAppSetting.MRITYUBHAGAERNST)
                {
                    _mbChanged = true;
                }
            }
        }

        private void CheckNodeCheckboxDefault()
        {
            if (!checkBoxNodeMean.Checked && !checkBoxNodeTrue.Checked)
            {
                checkBoxNodeMean.Checked = true;
                checkBoxNodeTrue.Checked = false;
            }
        }

        private void checkBoxNodeMean_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNodeMean.Checked)
            {
                checkBoxNodeTrue.Checked = false;
            }
            CheckNodeCheckboxDefault();

            if (checkBoxNodeMean.Checked)
            {
                if (Utility.GetActiveNodeMode(_appSetList) != EAppSetting.NODEMEAN)
                {
                    _nodeChanged = true;
                }
            }
        }

        private void checkBoxNodeTrue_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNodeTrue.Checked)
            {
                checkBoxNodeMean.Checked = false;
            }
            CheckNodeCheckboxDefault();

            if (checkBoxNodeTrue.Checked)
            {
                if (Utility.GetActiveNodeMode(_appSetList) != EAppSetting.NODETRUE)
                {
                    _nodeChanged = true;
                }
            }
        }

        private void CheckWeekCheckboxDefault()
        {
            if (!checkBoxWeekSunday.Checked && !checkBoxWeekMonday.Checked)
            {
                checkBoxWeekSunday.Checked = true;
                checkBoxWeekMonday.Checked = false;
            }
        }

        private void checkBoxWeekSunday_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWeekSunday.Checked)
            {
                checkBoxWeekMonday.Checked = false;
            }
            CheckWeekCheckboxDefault();

            if (checkBoxWeekSunday.Checked)
            {
                if (Utility.GetActiveWeekMode(_appSetList) != EAppSetting.WEEKSUNDAY)
                {
                    _weekChanged = true;
                }
            }
        }

        private void checkBoxWeekMonday_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWeekMonday.Checked)
            {
                checkBoxWeekSunday.Checked = false;
            }
            CheckWeekCheckboxDefault();

            if (checkBoxWeekMonday.Checked)
            {
                if (Utility.GetActiveWeekMode(_appSetList) != EAppSetting.WEEKMONDAY)
                {
                    _weekChanged = true;
                }
            }
        }

        private void checkBoxEnlish_MouseEnter(object sender, EventArgs e)
        {
            checkBoxEnlish.BackColor = Color.Yellow;
        }

        private void checkBoxEnlish_MouseLeave(object sender, EventArgs e)
        {
            if (_activeLanguageSetting != EAppSetting.ENGLISH && !_langChanged)
            {
                checkBoxEnlish.BackColor = Color.LightGray;
            }
            else
            {
                if (_newSelectedLanguage == EAppSetting.ENGLISH)
                {
                    checkBoxEnlish.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    checkBoxEnlish.BackColor = Color.LightGray;
                }
            }
        }

        private void checkBoxRussian_MouseEnter(object sender, EventArgs e)
        {
            checkBoxRussian.BackColor = Color.Yellow;
        }

        private void checkBoxRussian_MouseLeave(object sender, EventArgs e)
        {
            if (_activeLanguageSetting != EAppSetting.RUSSIAN && !_langChanged)
            {
                checkBoxRussian.BackColor = Color.LightGray;
            }
            else
            {
                if (_newSelectedLanguage == EAppSetting.RUSSIAN)
                {
                    checkBoxRussian.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    checkBoxRussian.BackColor = Color.LightGray;
                }
            }
        }

        private void checkBoxEnlish_Click(object sender, EventArgs e)
        {
            checkBoxEnlish.BackColor = Color.DeepSkyBlue;
            checkBoxRussian.BackColor = Color.LightGray;
            _newSelectedLanguage = EAppSetting.ENGLISH;
            if (Utility.GetActiveLanguageCode(_appSetList) != _newSelectedLanguage)
            {
                _langChanged = true;
            }
            else
            {
                _langChanged = false;
            }
        }

        private void checkBoxRussian_Click(object sender, EventArgs e)
        {
            checkBoxRussian.BackColor = Color.DeepSkyBlue;
            checkBoxEnlish.BackColor = Color.LightGray;
            _newSelectedLanguage = EAppSetting.RUSSIAN;
            if (Utility.GetActiveLanguageCode(_appSetList) != _newSelectedLanguage)
            {
                _langChanged = true;
            }
            else
            {
                _langChanged = false;
            }
        }
        
    }
}
