namespace PAD
{
    partial class TransitsMap
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
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.radioButtonLagna = new System.Windows.Forms.RadioButton();
            this.radioButtonNatMoon = new System.Windows.Forms.RadioButton();
            this.groupBoxAspects = new System.Windows.Forms.GroupBox();
            this.checkBoxRahu = new System.Windows.Forms.CheckBox();
            this.checkBoxSaturn = new System.Windows.Forms.CheckBox();
            this.checkBoxMars = new System.Windows.Forms.CheckBox();
            this.checkBoxMercury = new System.Windows.Forms.CheckBox();
            this.checkBoxJupiter = new System.Windows.Forms.CheckBox();
            this.checkBoxVenus = new System.Windows.Forms.CheckBox();
            this.checkBoxSun = new System.Windows.Forms.CheckBox();
            this.checkBoxMoon = new System.Windows.Forms.CheckBox();
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.buttonClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            this.groupBoxMode.SuspendLayout();
            this.groupBoxAspects.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Location = new System.Drawing.Point(15, 15);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(560, 560);
            this.pictureBoxMap.TabIndex = 0;
            this.pictureBoxMap.TabStop = false;
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.Controls.Add(this.radioButtonLagna);
            this.groupBoxMode.Controls.Add(this.radioButtonNatMoon);
            this.groupBoxMode.Location = new System.Drawing.Point(586, 9);
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.Size = new System.Drawing.Size(150, 81);
            this.groupBoxMode.TabIndex = 1;
            this.groupBoxMode.TabStop = false;
            this.groupBoxMode.Text = "Транзиты от";
            // 
            // radioButtonLagna
            // 
            this.radioButtonLagna.AutoSize = true;
            this.radioButtonLagna.Location = new System.Drawing.Point(7, 48);
            this.radioButtonLagna.Name = "radioButtonLagna";
            this.radioButtonLagna.Size = new System.Drawing.Size(65, 20);
            this.radioButtonLagna.TabIndex = 1;
            this.radioButtonLagna.Text = "Лагны";
            this.radioButtonLagna.UseVisualStyleBackColor = true;
            this.radioButtonLagna.CheckedChanged += new System.EventHandler(this.radioButtonLagna_CheckedChanged);
            // 
            // radioButtonNatMoon
            // 
            this.radioButtonNatMoon.AutoSize = true;
            this.radioButtonNatMoon.Checked = true;
            this.radioButtonNatMoon.Location = new System.Drawing.Point(7, 22);
            this.radioButtonNatMoon.Name = "radioButtonNatMoon";
            this.radioButtonNatMoon.Size = new System.Drawing.Size(134, 20);
            this.radioButtonNatMoon.TabIndex = 0;
            this.radioButtonNatMoon.TabStop = true;
            this.radioButtonNatMoon.Text = "Натальной Луны";
            this.radioButtonNatMoon.UseVisualStyleBackColor = true;
            this.radioButtonNatMoon.CheckedChanged += new System.EventHandler(this.radioButtonNatMoon_CheckedChanged);
            // 
            // groupBoxAspects
            // 
            this.groupBoxAspects.Controls.Add(this.checkBoxRahu);
            this.groupBoxAspects.Controls.Add(this.checkBoxSaturn);
            this.groupBoxAspects.Controls.Add(this.checkBoxMars);
            this.groupBoxAspects.Controls.Add(this.checkBoxMercury);
            this.groupBoxAspects.Controls.Add(this.checkBoxJupiter);
            this.groupBoxAspects.Controls.Add(this.checkBoxVenus);
            this.groupBoxAspects.Controls.Add(this.checkBoxSun);
            this.groupBoxAspects.Controls.Add(this.checkBoxMoon);
            this.groupBoxAspects.Controls.Add(this.checkBoxAll);
            this.groupBoxAspects.Location = new System.Drawing.Point(586, 98);
            this.groupBoxAspects.Name = "groupBoxAspects";
            this.groupBoxAspects.Size = new System.Drawing.Size(150, 259);
            this.groupBoxAspects.TabIndex = 2;
            this.groupBoxAspects.TabStop = false;
            this.groupBoxAspects.Text = "Аспекты";
            // 
            // checkBoxRahu
            // 
            this.checkBoxRahu.AutoSize = true;
            this.checkBoxRahu.Location = new System.Drawing.Point(7, 230);
            this.checkBoxRahu.Name = "checkBoxRahu";
            this.checkBoxRahu.Size = new System.Drawing.Size(57, 20);
            this.checkBoxRahu.TabIndex = 8;
            this.checkBoxRahu.Text = "Раху";
            this.checkBoxRahu.UseVisualStyleBackColor = true;
            this.checkBoxRahu.CheckedChanged += new System.EventHandler(this.checkBoxRahu_CheckedChanged);
            // 
            // checkBoxSaturn
            // 
            this.checkBoxSaturn.AutoSize = true;
            this.checkBoxSaturn.Location = new System.Drawing.Point(7, 204);
            this.checkBoxSaturn.Name = "checkBoxSaturn";
            this.checkBoxSaturn.Size = new System.Drawing.Size(74, 20);
            this.checkBoxSaturn.TabIndex = 7;
            this.checkBoxSaturn.Text = "Сатурн";
            this.checkBoxSaturn.UseVisualStyleBackColor = true;
            this.checkBoxSaturn.CheckedChanged += new System.EventHandler(this.checkBoxSaturn_CheckedChanged);
            // 
            // checkBoxMars
            // 
            this.checkBoxMars.AutoSize = true;
            this.checkBoxMars.Location = new System.Drawing.Point(7, 178);
            this.checkBoxMars.Name = "checkBoxMars";
            this.checkBoxMars.Size = new System.Drawing.Size(60, 20);
            this.checkBoxMars.TabIndex = 6;
            this.checkBoxMars.Text = "Марс";
            this.checkBoxMars.UseVisualStyleBackColor = true;
            this.checkBoxMars.CheckedChanged += new System.EventHandler(this.checkBoxMars_CheckedChanged);
            // 
            // checkBoxMercury
            // 
            this.checkBoxMercury.AutoSize = true;
            this.checkBoxMercury.Location = new System.Drawing.Point(7, 152);
            this.checkBoxMercury.Name = "checkBoxMercury";
            this.checkBoxMercury.Size = new System.Drawing.Size(92, 20);
            this.checkBoxMercury.TabIndex = 5;
            this.checkBoxMercury.Text = "Меркурий";
            this.checkBoxMercury.UseVisualStyleBackColor = true;
            this.checkBoxMercury.CheckedChanged += new System.EventHandler(this.checkBoxMercury_CheckedChanged);
            // 
            // checkBoxJupiter
            // 
            this.checkBoxJupiter.AutoSize = true;
            this.checkBoxJupiter.Location = new System.Drawing.Point(7, 126);
            this.checkBoxJupiter.Name = "checkBoxJupiter";
            this.checkBoxJupiter.Size = new System.Drawing.Size(76, 20);
            this.checkBoxJupiter.TabIndex = 4;
            this.checkBoxJupiter.Text = "Юпитер";
            this.checkBoxJupiter.UseVisualStyleBackColor = true;
            this.checkBoxJupiter.CheckedChanged += new System.EventHandler(this.checkBoxJupiter_CheckedChanged);
            // 
            // checkBoxVenus
            // 
            this.checkBoxVenus.AutoSize = true;
            this.checkBoxVenus.Location = new System.Drawing.Point(7, 100);
            this.checkBoxVenus.Name = "checkBoxVenus";
            this.checkBoxVenus.Size = new System.Drawing.Size(75, 20);
            this.checkBoxVenus.TabIndex = 3;
            this.checkBoxVenus.Text = "Венера";
            this.checkBoxVenus.UseVisualStyleBackColor = true;
            this.checkBoxVenus.CheckedChanged += new System.EventHandler(this.checkBoxVenera_CheckedChanged);
            // 
            // checkBoxSun
            // 
            this.checkBoxSun.AutoSize = true;
            this.checkBoxSun.Location = new System.Drawing.Point(7, 74);
            this.checkBoxSun.Name = "checkBoxSun";
            this.checkBoxSun.Size = new System.Drawing.Size(75, 20);
            this.checkBoxSun.TabIndex = 2;
            this.checkBoxSun.Text = "Солнце";
            this.checkBoxSun.UseVisualStyleBackColor = true;
            this.checkBoxSun.CheckedChanged += new System.EventHandler(this.checkBoxSun_CheckedChanged);
            // 
            // checkBoxMoon
            // 
            this.checkBoxMoon.AutoSize = true;
            this.checkBoxMoon.Location = new System.Drawing.Point(7, 48);
            this.checkBoxMoon.Name = "checkBoxMoon";
            this.checkBoxMoon.Size = new System.Drawing.Size(59, 20);
            this.checkBoxMoon.TabIndex = 1;
            this.checkBoxMoon.Text = "Луна";
            this.checkBoxMoon.UseVisualStyleBackColor = true;
            this.checkBoxMoon.CheckedChanged += new System.EventHandler(this.checkBoxMoon_CheckedChanged);
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Location = new System.Drawing.Point(7, 22);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(50, 20);
            this.checkBoxAll.TabIndex = 0;
            this.checkBoxAll.Text = "Все";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(647, 543);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 32);
            this.buttonClose.TabIndex = 17;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // TransitsMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(750, 590);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBoxAspects);
            this.Controls.Add(this.groupBoxMode);
            this.Controls.Add(this.pictureBoxMap);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransitsMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tranzits Map";
            this.Shown += new System.EventHandler(this.TransitsMap_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.groupBoxAspects.ResumeLayout(false);
            this.groupBoxAspects.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.GroupBox groupBoxMode;
        private System.Windows.Forms.RadioButton radioButtonLagna;
        private System.Windows.Forms.RadioButton radioButtonNatMoon;
        private System.Windows.Forms.GroupBox groupBoxAspects;
        private System.Windows.Forms.CheckBox checkBoxRahu;
        private System.Windows.Forms.CheckBox checkBoxSaturn;
        private System.Windows.Forms.CheckBox checkBoxMars;
        private System.Windows.Forms.CheckBox checkBoxMercury;
        private System.Windows.Forms.CheckBox checkBoxJupiter;
        private System.Windows.Forms.CheckBox checkBoxVenus;
        private System.Windows.Forms.CheckBox checkBoxSun;
        private System.Windows.Forms.CheckBox checkBoxMoon;
        private System.Windows.Forms.CheckBox checkBoxAll;
        private System.Windows.Forms.Button buttonClose;
    }
}