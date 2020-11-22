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
        private List<FontListDescription> _fDescList;

        public FontSettings(ELanguage langCode)
        {
            InitializeComponent();

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

            listBoxFont.SelectedIndex = (CacheLoad._fontList.Where(i => i.Id == lbsnSelectedItem.ItemId).FirstOrDefault()?.FontId ?? 0) - 1;
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, GetFontStyleById((CacheLoad._fontList.Where(i => i.Id == lbsnSelectedItem.ItemId).FirstOrDefault()?.FontStyleId ?? 0)));
            int styleId = CacheLoad._fontList.Where(i => i.Id == lbsnSelectedItem.ItemId).FirstOrDefault()?.FontStyleId ?? 0;
            SetFontStyle((EFontStyle)styleId);
        }
        
        private void SetFontStyle(EFontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case EFontStyle.REGULAR:
                    radioButtonNormal.Checked = true;
                    break;

                case EFontStyle.BOLD:
                    radioButtonBold.Checked = true;
                    break;

                case EFontStyle.ITALIC:
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

        private int GetFontStyleId(FontStyle fStyle)
        {
            int id = 0;
            switch (fStyle)
            {
                case FontStyle.Regular:
                    id = 1;
                    break;
                case FontStyle.Bold:
                    id = 2;
                    break;
                case FontStyle.Italic:
                    id = 3;
                    break;
            }
            return id;
        }

        private FontStyle SetFontStyle()
        {
            FontStyle curStyle = 0;
            if (radioButtonNormal.Checked)
                curStyle =  FontStyle.Regular;
            if (radioButtonBold.Checked)
                curStyle =  FontStyle.Bold;
            if (radioButtonItalic.Checked)
                curStyle =  FontStyle.Italic;
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
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to save current settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                UpdateFontsSettings();
                CacheLoad._fontList = null;
                CacheLoad._fontList = CacheLoad.GetFontList();
                ShowSettingsData();
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to revert default settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SetDefaultSettings();
                CacheLoad._fontList = CacheLoad.GetFontList();
                ShowSettingsData();
            }
        }

        private void SetDefaultSettings()
        {
            int sysFontId = CacheLoad._systemFontList.Where(i => i.AppMain == 1).FirstOrDefault()?.Id ?? 0;
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update FONTLIST set FONTID = " + sysFontId + ", FONTSTYLEID = 1", dbCon);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }
        
        private void UpdateFontsSettings()
        {
            KeyValueData lbfSelectedItem = (KeyValueData)listBoxFont.SelectedItem;
            KeyValueData lbsnSelectedItem = (KeyValueData)listBoxSettingsName.SelectedItem;
            int fontId = lbfSelectedItem.ItemId;
            int fontStyle = GetFontStyleId(SetFontStyle());
            int settingsId = lbsnSelectedItem.ItemId;
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update FONTLIST set FONTID = @FONTID, FONTSTYLEID = @FONTSTYLEID where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@FONTID", fontId);
                    command.Parameters.AddWithValue("@FONTSTYLEID", fontStyle);
                    command.Parameters.AddWithValue("@ID", settingsId);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void radioButtonNormal_Click(object sender, EventArgs e)
        {
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, SetFontStyle());
        }

        private void radioButtonBold_Click(object sender, EventArgs e)
        {
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, SetFontStyle());
        }

        private void radioButtonItalic_Click(object sender, EventArgs e)
        {
            labelExample.Font = new Font(listBoxFont.GetItemText(listBoxFont.SelectedItem), 20, SetFontStyle());
        }
    }
}
