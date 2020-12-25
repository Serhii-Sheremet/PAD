using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class FontSettings : Form
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

        private ELanguage _activeLang;
        private List<FontList> _activeFontList;
        private List<FontList> _changedFontList;
        private List<FontListDescription> _fDescList;

        public FontSettings(List<FontList> fList, ELanguage langCode)
        {
            InitializeComponent();

            _activeFontList = fList;
            _changedFontList = fList.Select(item => (FontList)item.Clone()).ToList(); 
            _activeLang = langCode;
            _fDescList = CacheLoad._fontDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
        }

        private void TextSettings_Shown(object sender, EventArgs e)
        {
            if (CacheLoad._systemFontList != null)
            {
                CacheLoad._systemFontList.ForEach(i => listBoxFont.Items.Add(new KeyValueData(i.SystemName, i.Id)));
            }
            if (_fDescList != null)
            {
                _fDescList.ForEach(i => listBoxSettingsName.Items.Add(new KeyValueData(i.Name, i.FontListId)));
                listBoxSettingsName.SelectedIndex = 0;
            }
            Utility.LocalizeForm(this, _activeLang);
        }

        private void listBoxSettingsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSettingsData();
        }
        
        private void ShowSettingsData()
        {
            KeyValueData lbsnSelectedItem = (KeyValueData)listBoxSettingsName.SelectedItem;
            listBoxFont.SelectedIndex = (_changedFontList.Where(i => i.Id == lbsnSelectedItem.ItemId).FirstOrDefault()?.FontId ?? 0) - 1;
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, GetFontStyleById(_changedFontList.Where(i => i.Id == lbsnSelectedItem.ItemId).FirstOrDefault()?.FontStyleId ?? 0));
            int styleId = _changedFontList.Where(i => i.Id == lbsnSelectedItem.ItemId).FirstOrDefault()?.FontStyleId ?? 0;
            SetFontStyle(styleId);
        }
        
        private void SetFontStyle(int styleId)
        {
            switch (styleId)
            {
                case 1:
                    radioButtonNormal.Checked = true;
                    break;

                case 2:
                    radioButtonBold.Checked = true;
                    break;

                case 3:
                    radioButtonItalic.Checked = true;
                    break;
            }
        }
        
        private FontStyle GetFontStyleById(int id)
        {
            FontStyle curStyle = 0;
            switch (id)
            {
                case 1:
                    curStyle = FontStyle.Regular;
                    break;
                case 2:
                    curStyle = FontStyle.Bold;
                    break;
                case 3:
                    curStyle = FontStyle.Italic;
                    break;
            }
            return curStyle;
        }

        private FontStyle SetFontStyle()
        {
            FontStyle curStyle = 0;
            if (radioButtonNormal.Checked)
            {
                curStyle = FontStyle.Regular;
            }
            if (radioButtonBold.Checked)
            {
                curStyle = FontStyle.Bold;
            }
            if (radioButtonItalic.Checked)
            {
                curStyle = FontStyle.Italic;
            }
            return curStyle;
        }

        private void listBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fName = listBoxFont.GetItemText(listBoxFont.SelectedItem);
            if (!fName.Equals("Monotype Corsiva"))
            {
                radioButtonNormal.Enabled = true;
                radioButtonNormal.Checked = true;
                labelExample.Font = new Font(fName, 20, SetFontStyle());
            }
            else
            {
                radioButtonNormal.Enabled = false;
                radioButtonItalic.Checked = true;
            }

            KeyValueData lbsnSelectedItem = (KeyValueData)listBoxSettingsName.SelectedItem;
            int systemFontId = CacheLoad._systemFontList.Where(i => i.SystemName == listBoxFont.GetItemText(listBoxFont.SelectedItem)).FirstOrDefault()?.Id ?? 0;
            int styleId = _changedFontList.Where(i => i.Id == lbsnSelectedItem.ItemId).FirstOrDefault()?.FontStyleId ?? 0;
            UpdateChangedFontList(lbsnSelectedItem.ItemId, systemFontId, styleId);
        }

        private void UpdateChangedFontList(int fontSettingId, int systemFontId, int styleId)
        {
            foreach (FontList fl in _changedFontList)
            {
                if (fl.Id == fontSettingId)
                {
                    fl.FontId = systemFontId;
                    fl.FontStyleId = styleId;
                    break;
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (CheckFontSettingsChanges())
            {
                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Font settings have been changed. Do you want to apply new settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    UpdateFontsSettings(_changedFontList);
                    CacheLoad._fontList = _changedFontList;
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

        private bool CheckFontSettingsChanges()
        {
            for (int i = 0; i < _changedFontList.Count; i++)
            {
                if (_changedFontList[i].FontId != _activeFontList[i].FontId || _changedFontList[i].FontStyleId != _activeFontList[i].FontStyleId)
                {
                    return true;
                }
            }
            return false;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (CheckFontSettingsChanges())
            {
                DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to save current settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    UpdateFontsSettings(_changedFontList);
                    CacheLoad._fontList = _changedFontList;
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
            //default fonts list
            List<FontList> defaultList = new List<FontList>();
            defaultList.Add(new FontList { Id = 1, FontId = 13, Code = "HEADER", FontStyleId = 2 });
            defaultList.Add(new FontList { Id = 2, FontId = 14, Code = "CALENDARTEXT", FontStyleId = 1 });
            defaultList.Add(new FontList { Id = 3, FontId = 14, Code = "TRANZITTEXT", FontStyleId = 1 });
            defaultList.Add(new FontList { Id = 4, FontId = 8, Code = "TRANSTOOLTIPHEADER", FontStyleId = 2 });
            defaultList.Add(new FontList { Id = 5, FontId = 8, Code = "TRANSTOOLTIPTEXT", FontStyleId = 3 });
            defaultList.Add(new FontList { Id = 6, FontId = 12, Code = "DWTOOLTIPTITLE", FontStyleId = 2 });
            defaultList.Add(new FontList { Id = 7, FontId = 12, Code = "DWTOOLTIPTIME", FontStyleId = 1 });
            defaultList.Add(new FontList { Id = 8, FontId = 12, Code = "DWTOOLTIPTEXT", FontStyleId = 3 });
            defaultList.Add(new FontList { Id = 9, FontId = 12, Code = "PEVTOOLTIPDATE", FontStyleId = 2 });
            defaultList.Add(new FontList { Id = 10, FontId = 12, Code = "PEVTOOLTIPTIME", FontStyleId = 1 });
            defaultList.Add(new FontList { Id = 11, FontId = 12, Code = "PEVTOOLTIPTEXT", FontStyleId = 3 });

            UpdateFontsSettings(defaultList);
            CacheLoad._fontList = defaultList;
            frmShowMessage.Show(Utility.GetLocalizedText("Changes has been applied.", _activeLang), Utility.GetLocalizedText("Information", _activeLang), enumMessageIcon.Information, enumMessageButton.OK);
            Close();
        }
        
        private void UpdateFontsSettings(List<FontList> fList)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                for (int i = 0; i < fList.Count; i++)
                { 
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand("update FONTLIST set FONTID = @FONTID, FONTSTYLEID = @FONTSTYLEID where ID = @ID", dbCon);
                        command.Parameters.AddWithValue("@FONTID", fList[i].FontId);
                        command.Parameters.AddWithValue("@FONTSTYLEID", fList[i].FontStyleId);
                        command.Parameters.AddWithValue("@ID", fList[i].Id);
                        command.ExecuteNonQuery();
                        //id = (int)command.ExecuteScalar();
                    }
                    catch { }
                }
                dbCon.Close();
            }
        }

        private void radioButtonNormal_Click(object sender, EventArgs e)
        {
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, SetFontStyle());

            KeyValueData lbsnSelectedItem = (KeyValueData)listBoxSettingsName.SelectedItem;
            int systemFontId = CacheLoad._systemFontList.Where(i => i.SystemName == listBoxFont.GetItemText(listBoxFont.SelectedItem)).FirstOrDefault()?.Id ?? 0;
            UpdateChangedFontList(lbsnSelectedItem.ItemId, systemFontId, 1);
        }

        private void radioButtonBold_Click(object sender, EventArgs e)
        {
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, SetFontStyle());

            KeyValueData lbsnSelectedItem = (KeyValueData)listBoxSettingsName.SelectedItem;
            int systemFontId = CacheLoad._systemFontList.Where(i => i.SystemName == listBoxFont.GetItemText(listBoxFont.SelectedItem)).FirstOrDefault()?.Id ?? 0;
            UpdateChangedFontList(lbsnSelectedItem.ItemId, systemFontId, 2);
        }

        private void radioButtonItalic_Click(object sender, EventArgs e)
        {
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, SetFontStyle());

            KeyValueData lbsnSelectedItem = (KeyValueData)listBoxSettingsName.SelectedItem;
            int systemFontId = CacheLoad._systemFontList.Where(i => i.SystemName == listBoxFont.GetItemText(listBoxFont.SelectedItem)).FirstOrDefault()?.Id ?? 0;
            UpdateChangedFontList(lbsnSelectedItem.ItemId, systemFontId, 3);
        }
    }
}
