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

        private int[] _colorsDefault;
        private List<ColorDescription> _colorDescList;
        private ELanguage _activeLang;

        public ColorSettings()
        {
            InitializeComponent();
        }

        public ColorSettings(ELanguage langCode)
        {
            InitializeComponent();
            _activeLang = langCode;
            _colorDescList = CacheLoad._colorDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
            _colorsDefault = new int[] {
                                    -13631697,      // 1|GREEN
                                    -45233,         // 2|RED
                                    -3211314,       // 3|LIGHTGREEN
                                    -361121,        // 4|LIGHTRED
                                    -16181,         // 5|PINK
                                    -3211314,       // 6|JOGAMERGE
                                    -16181,         // 7|MUHURTAMERGE
                                    -16776961,      // 8|SELECTRECTANGLE
                                    -4587591,       // 9|SUN
                                    -4587591,       // 10|VENUS
                                    -4587591,       // 11|MERCURY
                                    -4587591,       // 12|MOON
                                    -14650,         // 13|SATURN
                                    -4587591,       // 14|JUPITER
                                    -14650,         // 15|MARS
                                    -4144960        // 16|GRAY
            };
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
            Close();
        }

        private void listBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSettingsData();
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to revert default settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SetDefaultSettings();
                CacheLoad._colorList = CacheLoad.GetColorsList();
                ShowSettingsData();
            }
        }

        private void ShowSettingsData()
        {
            KeyValueData selectedColorId = (KeyValueData)listBoxColor.SelectedItem;
            int ColorValue = CacheLoad._colorList.Where(i => i.Id == selectedColorId.ItemId).FirstOrDefault()?.ARGBValue ?? 0;
            pictureBoxColor.BackColor = Color.FromArgb(ColorValue);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to save current settings?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                KeyValueData selectedColorId = (KeyValueData)listBoxColor.SelectedItem;
                UpdateColorSettings(selectedColorId.ItemId, pictureBoxColor.BackColor.ToArgb());
                CacheLoad._colorList = CacheLoad.GetColorsList();
                ShowSettingsData();
            }
        }

        private void pictureBoxColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;

            pictureBoxColor.BackColor = colorDialog.Color;
        }

        private void UpdateColorSettings(int id, int argbValue)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update COLOR set ARGBVALUE = @ARGBVALUE where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@ARGBVALUE", argbValue);
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
                catch {}
                dbCon.Close();
            }
        }

        private void SetDefaultSettings()
        {
            for (int i = 0; i < listBoxColor.Items.Count; i++)
            {
                KeyValueData currentColor = (KeyValueData)listBoxColor.Items[i];
                UpdateColorSettings(currentColor.ItemId, _colorsDefault[i]);
            }
        }

    }
}
