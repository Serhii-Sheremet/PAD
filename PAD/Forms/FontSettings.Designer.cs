namespace PAD
{
    partial class FontSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontSettings));
            this.groupBoxFontSettings = new System.Windows.Forms.GroupBox();
            this.groupBoxFontType = new System.Windows.Forms.GroupBox();
            this.radioButtonItalic = new System.Windows.Forms.RadioButton();
            this.radioButtonBold = new System.Windows.Forms.RadioButton();
            this.radioButtonNormal = new System.Windows.Forms.RadioButton();
            this.groupBoxExampleSettings = new System.Windows.Forms.GroupBox();
            this.labelExample = new System.Windows.Forms.Label();
            this.listBoxFont = new System.Windows.Forms.ListBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.groupBoxSettingName = new System.Windows.Forms.GroupBox();
            this.listBoxSettingsName = new System.Windows.Forms.ListBox();
            this.groupBoxFontSettings.SuspendLayout();
            this.groupBoxFontType.SuspendLayout();
            this.groupBoxExampleSettings.SuspendLayout();
            this.groupBoxSettingName.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFontSettings
            // 
            this.groupBoxFontSettings.Controls.Add(this.groupBoxFontType);
            this.groupBoxFontSettings.Controls.Add(this.groupBoxExampleSettings);
            this.groupBoxFontSettings.Controls.Add(this.listBoxFont);
            this.groupBoxFontSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFontSettings.Location = new System.Drawing.Point(300, 12);
            this.groupBoxFontSettings.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxFontSettings.Name = "groupBoxFontSettings";
            this.groupBoxFontSettings.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxFontSettings.Size = new System.Drawing.Size(470, 240);
            this.groupBoxFontSettings.TabIndex = 1;
            this.groupBoxFontSettings.TabStop = false;
            this.groupBoxFontSettings.Text = "Font settings";
            // 
            // groupBoxFontType
            // 
            this.groupBoxFontType.Controls.Add(this.radioButtonItalic);
            this.groupBoxFontType.Controls.Add(this.radioButtonBold);
            this.groupBoxFontType.Controls.Add(this.radioButtonNormal);
            this.groupBoxFontType.Location = new System.Drawing.Point(172, 154);
            this.groupBoxFontType.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxFontType.Name = "groupBoxFontType";
            this.groupBoxFontType.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxFontType.Size = new System.Drawing.Size(289, 70);
            this.groupBoxFontType.TabIndex = 2;
            this.groupBoxFontType.TabStop = false;
            // 
            // radioButtonItalic
            // 
            this.radioButtonItalic.AutoSize = true;
            this.radioButtonItalic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonItalic.Location = new System.Drawing.Point(201, 31);
            this.radioButtonItalic.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonItalic.Name = "radioButtonItalic";
            this.radioButtonItalic.Size = new System.Drawing.Size(53, 20);
            this.radioButtonItalic.TabIndex = 3;
            this.radioButtonItalic.TabStop = true;
            this.radioButtonItalic.Text = "Italic";
            this.radioButtonItalic.UseVisualStyleBackColor = true;
            this.radioButtonItalic.Click += new System.EventHandler(this.radioButtonItalic_Click);
            // 
            // radioButtonBold
            // 
            this.radioButtonBold.AutoSize = true;
            this.radioButtonBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonBold.Location = new System.Drawing.Point(106, 31);
            this.radioButtonBold.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonBold.Name = "radioButtonBold";
            this.radioButtonBold.Size = new System.Drawing.Size(58, 20);
            this.radioButtonBold.TabIndex = 2;
            this.radioButtonBold.TabStop = true;
            this.radioButtonBold.Text = "Bold";
            this.radioButtonBold.UseVisualStyleBackColor = true;
            this.radioButtonBold.Click += new System.EventHandler(this.radioButtonBold_Click);
            // 
            // radioButtonNormal
            // 
            this.radioButtonNormal.AutoSize = true;
            this.radioButtonNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNormal.Location = new System.Drawing.Point(6, 31);
            this.radioButtonNormal.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonNormal.Name = "radioButtonNormal";
            this.radioButtonNormal.Size = new System.Drawing.Size(74, 20);
            this.radioButtonNormal.TabIndex = 0;
            this.radioButtonNormal.TabStop = true;
            this.radioButtonNormal.Text = "Regular";
            this.radioButtonNormal.UseVisualStyleBackColor = true;
            this.radioButtonNormal.Click += new System.EventHandler(this.radioButtonNormal_Click);
            // 
            // groupBoxExampleSettings
            // 
            this.groupBoxExampleSettings.Controls.Add(this.labelExample);
            this.groupBoxExampleSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxExampleSettings.Location = new System.Drawing.Point(172, 18);
            this.groupBoxExampleSettings.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxExampleSettings.Name = "groupBoxExampleSettings";
            this.groupBoxExampleSettings.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxExampleSettings.Size = new System.Drawing.Size(289, 131);
            this.groupBoxExampleSettings.TabIndex = 1;
            this.groupBoxExampleSettings.TabStop = false;
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelExample.Location = new System.Drawing.Point(11, 52);
            this.labelExample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(175, 31);
            this.labelExample.TabIndex = 0;
            this.labelExample.Text = "Text example";
            // 
            // listBoxFont
            // 
            this.listBoxFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxFont.FormattingEnabled = true;
            this.listBoxFont.Location = new System.Drawing.Point(13, 25);
            this.listBoxFont.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxFont.Name = "listBoxFont";
            this.listBoxFont.Size = new System.Drawing.Size(150, 199);
            this.listBoxFont.TabIndex = 0;
            this.listBoxFont.SelectedIndexChanged += new System.EventHandler(this.listBoxFont_SelectedIndexChanged);
            // 
            // buttonApply
            // 
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApply.Location = new System.Drawing.Point(560, 280);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(100, 28);
            this.buttonApply.TabIndex = 4;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(668, 280);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 28);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefault.Location = new System.Drawing.Point(15, 280);
            this.buttonDefault.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(200, 28);
            this.buttonDefault.TabIndex = 6;
            this.buttonDefault.Text = "Default settings";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // groupBoxSettingName
            // 
            this.groupBoxSettingName.Controls.Add(this.listBoxSettingsName);
            this.groupBoxSettingName.Location = new System.Drawing.Point(15, 12);
            this.groupBoxSettingName.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxSettingName.Name = "groupBoxSettingName";
            this.groupBoxSettingName.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxSettingName.Size = new System.Drawing.Size(275, 240);
            this.groupBoxSettingName.TabIndex = 7;
            this.groupBoxSettingName.TabStop = false;
            this.groupBoxSettingName.Text = "Settings list";
            // 
            // listBoxSettingsName
            // 
            this.listBoxSettingsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxSettingsName.FormattingEnabled = true;
            this.listBoxSettingsName.HorizontalScrollbar = true;
            this.listBoxSettingsName.Location = new System.Drawing.Point(13, 25);
            this.listBoxSettingsName.Name = "listBoxSettingsName";
            this.listBoxSettingsName.Size = new System.Drawing.Size(250, 199);
            this.listBoxSettingsName.TabIndex = 0;
            this.listBoxSettingsName.SelectedIndexChanged += new System.EventHandler(this.listBoxSettingsName_SelectedIndexChanged);
            // 
            // FontSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(784, 321);
            this.Controls.Add(this.groupBoxSettingName);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.groupBoxFontSettings);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FontSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Font settings";
            this.Shown += new System.EventHandler(this.TextSettings_Shown);
            this.groupBoxFontSettings.ResumeLayout(false);
            this.groupBoxFontType.ResumeLayout(false);
            this.groupBoxFontType.PerformLayout();
            this.groupBoxExampleSettings.ResumeLayout(false);
            this.groupBoxExampleSettings.PerformLayout();
            this.groupBoxSettingName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxFontSettings;
        private System.Windows.Forms.GroupBox groupBoxFontType;
        private System.Windows.Forms.RadioButton radioButtonItalic;
        private System.Windows.Forms.RadioButton radioButtonBold;
        private System.Windows.Forms.RadioButton radioButtonNormal;
        private System.Windows.Forms.GroupBox groupBoxExampleSettings;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.ListBox listBoxFont;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.GroupBox groupBoxSettingName;
        private System.Windows.Forms.ListBox listBoxSettingsName;
    }
}