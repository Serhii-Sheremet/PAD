using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class ColorSettings : Form
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

        private List<Colors> _activeColorsList;
        private List<Colors> _changedColorsList;
        private List<ColorDescription> _colorDescList;
        private ELanguage _activeLang;

        public ColorSettings()
        {
            InitializeComponent();
        }

        public ColorSettings(List<Colors> cList, ELanguage langCode)
        {
            InitializeComponent();
            _activeLang = langCode;
            _activeColorsList = cList;
            _changedColorsList = cList.Select(item => (Colors)item.Clone()).ToList();
            _colorDescList = CacheLoad._colorDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
            
        }

        private void ColorSettings_Shown(object sender, EventArgs e)
        {
            if (_colorDescList != null)
            {
                _colorDescList.ForEach(i => listBoxColor.Items.Add(new KeyValueData(i.Name, i.ColorId)));
                listBoxColor.SelectedIndex = 0;
            }
            Utility.LocalizeForm(this, _activeLang);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (CheckColorSettingsChanges())
            {
                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Color settings have been changed. Do you want to apply new settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    UpdateColorSettings(_changedColorsList);
                    CacheLoad._colorList = _changedColorsList;
                    Close();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        private bool CheckColorSettingsChanges()
        {
            for (int i = 0; i < _changedColorsList.Count; i++)
            {
                if (_changedColorsList[i].ARGBValue != _activeColorsList[i].ARGBValue )
                {
                    return true;
                }
            }
            return false;
        }

        private void listBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSettingsData();
        }

        private void ShowSettingsData()
        {
            KeyValueData selectedColorId = (KeyValueData)listBoxColor.SelectedItem;
            int ColorValue = _changedColorsList.Where(i => i.Id == selectedColorId.ItemId).FirstOrDefault()?.ARGBValue ?? 0;
            pictureBoxColor.BackColor = Color.FromArgb(ColorValue);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (CheckColorSettingsChanges())
            {
                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to save current settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    UpdateColorSettings(_changedColorsList);
                    CacheLoad._colorList = _changedColorsList;
                    frmShowMessage.Show(Utility.GetLocalizedText("Changes has been applied.", _activeLang), Utility.GetLocalizedText("Information", _activeLang), enumMessageIcon.Information, enumMessageButton.OK);
                    Close();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Nothing has been changed.", _activeLang), Utility.GetLocalizedText("Information", _activeLang), enumMessageIcon.Information, enumMessageButton.OK);
            }
        }

        private void pictureBoxColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;

            pictureBoxColor.BackColor = colorDialog.Color;
            KeyValueData lbcSelectedItem = (KeyValueData)listBoxColor.SelectedItem;
            int argbValue = pictureBoxColor.BackColor.ToArgb();
            UpdateChangedColorsList(lbcSelectedItem.ItemId, argbValue);
        }

        private void UpdateChangedColorsList(int colorSettingsId, int argbValue)
        {
            foreach (Colors c in _changedColorsList)
            {
                if (c.Id == colorSettingsId)
                {
                    c.ARGBValue = argbValue;
                    break;
                }
            }
        }

        private void UpdateColorSettings(List<Colors> cList)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                for (int i = 0; i < cList.Count; i++)
                {
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand("update COLOR set ARGBVALUE = @ARGBVALUE where ID = @ID", dbCon);
                        command.Parameters.AddWithValue("@ARGBVALUE", cList[i].ARGBValue);
                        command.Parameters.AddWithValue("@ID", cList[i].Id);
                        command.ExecuteNonQuery();
                    }
                    catch { }
                }
                dbCon.Close();
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to revert default settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SetDefaultSettings();
            }
        }

        private void SetDefaultSettings()
        {
            //default clolors list
            List<Colors> defaultList = new List<Colors>();
            defaultList.Add(new Colors { Id = 1, Code = "GREEN", ARGBValue = -13631697 });
            defaultList.Add(new Colors { Id = 2, Code = "RED", ARGBValue = -45233 });
            defaultList.Add(new Colors { Id = 3, Code = "LIGHTGREEN", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 4, Code = "LIGHTRED", ARGBValue = -14650 });
            defaultList.Add(new Colors { Id = 5, Code = "PINK", ARGBValue = -16181 });
            defaultList.Add(new Colors { Id = 6, Code = "JOGAMERGE", ARGBValue = -3211314 });
            defaultList.Add(new Colors { Id = 7, Code = "MUHURTAMERGE", ARGBValue = -16181 });
            defaultList.Add(new Colors { Id = 8, Code = "SELECTRECTANGLE", ARGBValue = -16776961 });
            defaultList.Add(new Colors { Id = 9, Code = "SUN", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 10, Code = "VENUS", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 11, Code = "MERCURY", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 12, Code = "MOON", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 13, Code = "SATURN", ARGBValue = -14650 });
            defaultList.Add(new Colors { Id = 14, Code = "JUPITER", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 15, Code = "MARS", ARGBValue = -14650 });
            defaultList.Add(new Colors { Id = 16, Code = "MASA1", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 17, Code = "MASA2", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 18, Code = "MASA3", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 19, Code = "MASA4", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 20, Code = "MASA5", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 21, Code = "MASA6", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 22, Code = "MASA7", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 23, Code = "MASA8", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 24, Code = "MASA9", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 25, Code = "MASA10", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 26, Code = "MASA11", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 27, Code = "MASA12", ARGBValue = -4587591 });
            defaultList.Add(new Colors { Id = 28, Code = "SHUNYANAKSHATRA", ARGBValue = -16181 });
            defaultList.Add(new Colors { Id = 29, Code = "SHUNIATITHI", ARGBValue = -16181 });
            defaultList.Add(new Colors { Id = 30, Code = "GRAY", ARGBValue = -4144960 });

            UpdateColorSettings(defaultList);
            CacheLoad._colorList = defaultList;
            frmShowMessage.Show(Utility.GetLocalizedText("Changes has been applied.", _activeLang), Utility.GetLocalizedText("Information", _activeLang), enumMessageIcon.Information, enumMessageButton.OK);
            Close();
        }

    }
}
