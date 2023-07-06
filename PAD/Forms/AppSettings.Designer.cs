namespace PAD
{
    partial class AppSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppSettings));
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.groupBoxTranzits = new System.Windows.Forms.GroupBox();
            this.checkBoxBoth = new System.Windows.Forms.CheckBox();
            this.checkBoxLagna = new System.Windows.Forms.CheckBox();
            this.checkBoxMoon = new System.Windows.Forms.CheckBox();
            this.groupBoxHora = new System.Windows.Forms.GroupBox();
            this.checkBoxHoraFrom6 = new System.Windows.Forms.CheckBox();
            this.checkBoxHoraEqual = new System.Windows.Forms.CheckBox();
            this.checkBoxHoraSunRise = new System.Windows.Forms.CheckBox();
            this.groupBoxMuhurtaGhati = new System.Windows.Forms.GroupBox();
            this.checkBoxMuhurtsFrom6 = new System.Windows.Forms.CheckBox();
            this.checkBoxMuhurtsEqual = new System.Windows.Forms.CheckBox();
            this.checkBoxMuhurtsSunRise = new System.Windows.Forms.CheckBox();
            this.groupBoxLanguage = new System.Windows.Forms.GroupBox();
            this.labelRussian = new System.Windows.Forms.Label();
            this.labelEnglish = new System.Windows.Forms.Label();
            this.checkBoxRussian = new System.Windows.Forms.CheckBox();
            this.checkBoxEnlish = new System.Windows.Forms.CheckBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.groupBoxMrityuBhaga = new System.Windows.Forms.GroupBox();
            this.checkBoxMrityuErnst = new System.Windows.Forms.CheckBox();
            this.labelExplanation = new System.Windows.Forms.Label();
            this.checkBoxMrityuEqual = new System.Windows.Forms.CheckBox();
            this.checkBoxMrityuMore = new System.Windows.Forms.CheckBox();
            this.checkBoxMrityuLess = new System.Windows.Forms.CheckBox();
            this.groupBoxNodes = new System.Windows.Forms.GroupBox();
            this.checkBoxNodeTrue = new System.Windows.Forms.CheckBox();
            this.checkBoxNodeMean = new System.Windows.Forms.CheckBox();
            this.groupBoxWeek = new System.Windows.Forms.GroupBox();
            this.checkBoxWeekMonday = new System.Windows.Forms.CheckBox();
            this.checkBoxWeekSunday = new System.Windows.Forms.CheckBox();
            this.groupBoxTranzits.SuspendLayout();
            this.groupBoxHora.SuspendLayout();
            this.groupBoxMuhurtaGhati.SuspendLayout();
            this.groupBoxLanguage.SuspendLayout();
            this.groupBoxMrityuBhaga.SuspendLayout();
            this.groupBoxNodes.SuspendLayout();
            this.groupBoxWeek.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(755, 322);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 28);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApply.Location = new System.Drawing.Point(641, 322);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(100, 28);
            this.buttonApply.TabIndex = 7;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // groupBoxTranzits
            // 
            this.groupBoxTranzits.Controls.Add(this.checkBoxBoth);
            this.groupBoxTranzits.Controls.Add(this.checkBoxLagna);
            this.groupBoxTranzits.Controls.Add(this.checkBoxMoon);
            this.groupBoxTranzits.Location = new System.Drawing.Point(12, 148);
            this.groupBoxTranzits.Name = "groupBoxTranzits";
            this.groupBoxTranzits.Size = new System.Drawing.Size(360, 152);
            this.groupBoxTranzits.TabIndex = 10;
            this.groupBoxTranzits.TabStop = false;
            this.groupBoxTranzits.Text = "Planets tranzits";
            // 
            // checkBoxBoth
            // 
            this.checkBoxBoth.AutoSize = true;
            this.checkBoxBoth.Location = new System.Drawing.Point(13, 111);
            this.checkBoxBoth.Name = "checkBoxBoth";
            this.checkBoxBoth.Size = new System.Drawing.Size(310, 20);
            this.checkBoxBoth.TabIndex = 2;
            this.checkBoxBoth.Text = "Planets tranzits from natal Moon and from Lagna";
            this.checkBoxBoth.UseVisualStyleBackColor = true;
            this.checkBoxBoth.CheckedChanged += new System.EventHandler(this.checkBoxBoth_CheckedChanged);
            // 
            // checkBoxLagna
            // 
            this.checkBoxLagna.AutoSize = true;
            this.checkBoxLagna.Location = new System.Drawing.Point(13, 67);
            this.checkBoxLagna.Name = "checkBoxLagna";
            this.checkBoxLagna.Size = new System.Drawing.Size(179, 20);
            this.checkBoxLagna.TabIndex = 1;
            this.checkBoxLagna.Text = "Planets trazits from Lagna";
            this.checkBoxLagna.UseVisualStyleBackColor = true;
            this.checkBoxLagna.CheckedChanged += new System.EventHandler(this.checkBoxLagna_CheckedChanged);
            // 
            // checkBoxMoon
            // 
            this.checkBoxMoon.AutoSize = true;
            this.checkBoxMoon.Location = new System.Drawing.Point(13, 25);
            this.checkBoxMoon.Name = "checkBoxMoon";
            this.checkBoxMoon.Size = new System.Drawing.Size(214, 20);
            this.checkBoxMoon.TabIndex = 0;
            this.checkBoxMoon.Text = "Planets tranzits from natal Moon";
            this.checkBoxMoon.UseVisualStyleBackColor = true;
            this.checkBoxMoon.CheckedChanged += new System.EventHandler(this.checkBoxMoon_CheckedChanged);
            // 
            // groupBoxHora
            // 
            this.groupBoxHora.Controls.Add(this.checkBoxHoraFrom6);
            this.groupBoxHora.Controls.Add(this.checkBoxHoraEqual);
            this.groupBoxHora.Controls.Add(this.checkBoxHoraSunRise);
            this.groupBoxHora.Location = new System.Drawing.Point(381, 6);
            this.groupBoxHora.Name = "groupBoxHora";
            this.groupBoxHora.Size = new System.Drawing.Size(235, 140);
            this.groupBoxHora.TabIndex = 11;
            this.groupBoxHora.TabStop = false;
            this.groupBoxHora.Text = "Hora";
            // 
            // checkBoxHoraFrom6
            // 
            this.checkBoxHoraFrom6.AutoSize = true;
            this.checkBoxHoraFrom6.Location = new System.Drawing.Point(13, 103);
            this.checkBoxHoraFrom6.Name = "checkBoxHoraFrom6";
            this.checkBoxHoraFrom6.Size = new System.Drawing.Size(198, 20);
            this.checkBoxHoraFrom6.TabIndex = 2;
            this.checkBoxHoraFrom6.Text = "From 6:00 AM (Hora = 1 hour)";
            this.checkBoxHoraFrom6.UseVisualStyleBackColor = true;
            this.checkBoxHoraFrom6.CheckedChanged += new System.EventHandler(this.checkBoxHoraFrom6_CheckedChanged);
            // 
            // checkBoxHoraEqual
            // 
            this.checkBoxHoraEqual.AutoSize = true;
            this.checkBoxHoraEqual.Location = new System.Drawing.Point(13, 69);
            this.checkBoxHoraEqual.Name = "checkBoxHoraEqual";
            this.checkBoxHoraEqual.Size = new System.Drawing.Size(172, 20);
            this.checkBoxHoraEqual.TabIndex = 1;
            this.checkBoxHoraEqual.Text = "Sunrise-to-Sunrise (1/24)";
            this.checkBoxHoraEqual.UseVisualStyleBackColor = true;
            this.checkBoxHoraEqual.CheckedChanged += new System.EventHandler(this.checkBoxHoraEqual_CheckedChanged);
            // 
            // checkBoxHoraSunRise
            // 
            this.checkBoxHoraSunRise.Location = new System.Drawing.Point(13, 23);
            this.checkBoxHoraSunRise.Name = "checkBoxHoraSunRise";
            this.checkBoxHoraSunRise.Size = new System.Drawing.Size(210, 40);
            this.checkBoxHoraSunRise.TabIndex = 0;
            this.checkBoxHoraSunRise.Text = "Sunrise-to-Sunset (1/12) + Sunset-to-Sunrise (1/12)";
            this.checkBoxHoraSunRise.UseVisualStyleBackColor = true;
            this.checkBoxHoraSunRise.CheckedChanged += new System.EventHandler(this.checkBoxHoraSunRise_CheckedChanged);
            // 
            // groupBoxMuhurtaGhati
            // 
            this.groupBoxMuhurtaGhati.Controls.Add(this.checkBoxMuhurtsFrom6);
            this.groupBoxMuhurtaGhati.Controls.Add(this.checkBoxMuhurtsEqual);
            this.groupBoxMuhurtaGhati.Controls.Add(this.checkBoxMuhurtsSunRise);
            this.groupBoxMuhurtaGhati.Location = new System.Drawing.Point(381, 148);
            this.groupBoxMuhurtaGhati.Name = "groupBoxMuhurtaGhati";
            this.groupBoxMuhurtaGhati.Size = new System.Drawing.Size(235, 152);
            this.groupBoxMuhurtaGhati.TabIndex = 12;
            this.groupBoxMuhurtaGhati.TabStop = false;
            this.groupBoxMuhurtaGhati.Text = "30 Muhurts (and 60 Ghati)";
            // 
            // checkBoxMuhurtsFrom6
            // 
            this.checkBoxMuhurtsFrom6.AutoSize = true;
            this.checkBoxMuhurtsFrom6.Location = new System.Drawing.Point(13, 111);
            this.checkBoxMuhurtsFrom6.Name = "checkBoxMuhurtsFrom6";
            this.checkBoxMuhurtsFrom6.Size = new System.Drawing.Size(217, 20);
            this.checkBoxMuhurtsFrom6.TabIndex = 3;
            this.checkBoxMuhurtsFrom6.Text = "From 6:00 AM (Muhurta = 48 min)";
            this.checkBoxMuhurtsFrom6.UseVisualStyleBackColor = true;
            this.checkBoxMuhurtsFrom6.CheckedChanged += new System.EventHandler(this.checkBoxMuhurtsFrom6_CheckedChanged);
            // 
            // checkBoxMuhurtsEqual
            // 
            this.checkBoxMuhurtsEqual.AutoSize = true;
            this.checkBoxMuhurtsEqual.Location = new System.Drawing.Point(13, 69);
            this.checkBoxMuhurtsEqual.Name = "checkBoxMuhurtsEqual";
            this.checkBoxMuhurtsEqual.Size = new System.Drawing.Size(172, 20);
            this.checkBoxMuhurtsEqual.TabIndex = 1;
            this.checkBoxMuhurtsEqual.Text = "Sunrise-to-Sunrise (1/30)";
            this.checkBoxMuhurtsEqual.UseVisualStyleBackColor = true;
            this.checkBoxMuhurtsEqual.CheckedChanged += new System.EventHandler(this.checkBoxMuhurtsEqual_CheckedChanged);
            // 
            // checkBoxMuhurtsSunRise
            // 
            this.checkBoxMuhurtsSunRise.Location = new System.Drawing.Point(13, 19);
            this.checkBoxMuhurtsSunRise.Name = "checkBoxMuhurtsSunRise";
            this.checkBoxMuhurtsSunRise.Size = new System.Drawing.Size(210, 40);
            this.checkBoxMuhurtsSunRise.TabIndex = 0;
            this.checkBoxMuhurtsSunRise.Text = "Sunrise-to-Sunset (1/15) + Sunset-to-Sunrise (1/15)";
            this.checkBoxMuhurtsSunRise.UseVisualStyleBackColor = true;
            this.checkBoxMuhurtsSunRise.CheckedChanged += new System.EventHandler(this.checkBoxMuhurtsSunRise_CheckedChanged);
            // 
            // groupBoxLanguage
            // 
            this.groupBoxLanguage.Controls.Add(this.labelRussian);
            this.groupBoxLanguage.Controls.Add(this.labelEnglish);
            this.groupBoxLanguage.Controls.Add(this.checkBoxRussian);
            this.groupBoxLanguage.Controls.Add(this.checkBoxEnlish);
            this.groupBoxLanguage.Location = new System.Drawing.Point(12, 6);
            this.groupBoxLanguage.Name = "groupBoxLanguage";
            this.groupBoxLanguage.Size = new System.Drawing.Size(360, 140);
            this.groupBoxLanguage.TabIndex = 14;
            this.groupBoxLanguage.TabStop = false;
            this.groupBoxLanguage.Text = "Language";
            // 
            // labelRussian
            // 
            this.labelRussian.AutoSize = true;
            this.labelRussian.Location = new System.Drawing.Point(231, 111);
            this.labelRussian.Name = "labelRussian";
            this.labelRussian.Size = new System.Drawing.Size(57, 16);
            this.labelRussian.TabIndex = 6;
            this.labelRussian.Text = "Russian";
            // 
            // labelEnglish
            // 
            this.labelEnglish.AutoSize = true;
            this.labelEnglish.Location = new System.Drawing.Point(65, 111);
            this.labelEnglish.Name = "labelEnglish";
            this.labelEnglish.Size = new System.Drawing.Size(52, 16);
            this.labelEnglish.TabIndex = 5;
            this.labelEnglish.Text = "English";
            // 
            // checkBoxRussian
            // 
            this.checkBoxRussian.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRussian.AutoEllipsis = true;
            this.checkBoxRussian.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkBoxRussian.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxRussian.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxRussian.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxRussian.Image")));
            this.checkBoxRussian.Location = new System.Drawing.Point(205, 24);
            this.checkBoxRussian.Margin = new System.Windows.Forms.Padding(5);
            this.checkBoxRussian.Name = "checkBoxRussian";
            this.checkBoxRussian.Size = new System.Drawing.Size(112, 83);
            this.checkBoxRussian.TabIndex = 4;
            this.checkBoxRussian.UseVisualStyleBackColor = false;
            this.checkBoxRussian.Click += new System.EventHandler(this.checkBoxRussian_Click);
            this.checkBoxRussian.MouseEnter += new System.EventHandler(this.checkBoxRussian_MouseEnter);
            this.checkBoxRussian.MouseLeave += new System.EventHandler(this.checkBoxRussian_MouseLeave);
            // 
            // checkBoxEnlish
            // 
            this.checkBoxEnlish.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEnlish.AutoEllipsis = true;
            this.checkBoxEnlish.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.checkBoxEnlish.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxEnlish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEnlish.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxEnlish.Image")));
            this.checkBoxEnlish.Location = new System.Drawing.Point(39, 24);
            this.checkBoxEnlish.Margin = new System.Windows.Forms.Padding(5);
            this.checkBoxEnlish.Name = "checkBoxEnlish";
            this.checkBoxEnlish.Size = new System.Drawing.Size(112, 83);
            this.checkBoxEnlish.TabIndex = 3;
            this.checkBoxEnlish.UseVisualStyleBackColor = false;
            this.checkBoxEnlish.Click += new System.EventHandler(this.checkBoxEnlish_Click);
            this.checkBoxEnlish.MouseEnter += new System.EventHandler(this.checkBoxEnlish_MouseEnter);
            this.checkBoxEnlish.MouseLeave += new System.EventHandler(this.checkBoxEnlish_MouseLeave);
            // 
            // buttonDefault
            // 
            this.buttonDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefault.Location = new System.Drawing.Point(12, 322);
            this.buttonDefault.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(210, 28);
            this.buttonDefault.TabIndex = 15;
            this.buttonDefault.Text = "Default settings";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // groupBoxMrityuBhaga
            // 
            this.groupBoxMrityuBhaga.Controls.Add(this.checkBoxMrityuErnst);
            this.groupBoxMrityuBhaga.Controls.Add(this.labelExplanation);
            this.groupBoxMrityuBhaga.Controls.Add(this.checkBoxMrityuEqual);
            this.groupBoxMrityuBhaga.Controls.Add(this.checkBoxMrityuMore);
            this.groupBoxMrityuBhaga.Controls.Add(this.checkBoxMrityuLess);
            this.groupBoxMrityuBhaga.Location = new System.Drawing.Point(625, 6);
            this.groupBoxMrityuBhaga.Name = "groupBoxMrityuBhaga";
            this.groupBoxMrityuBhaga.Size = new System.Drawing.Size(210, 140);
            this.groupBoxMrityuBhaga.TabIndex = 16;
            this.groupBoxMrityuBhaga.TabStop = false;
            this.groupBoxMrityuBhaga.Text = "Mrityu Bhaga";
            // 
            // checkBoxMrityuErnst
            // 
            this.checkBoxMrityuErnst.AutoSize = true;
            this.checkBoxMrityuErnst.Location = new System.Drawing.Point(13, 101);
            this.checkBoxMrityuErnst.Name = "checkBoxMrityuErnst";
            this.checkBoxMrityuErnst.Size = new System.Drawing.Size(160, 20);
            this.checkBoxMrityuErnst.TabIndex = 7;
            this.checkBoxMrityuErnst.Text = "From (N - 1)° till (N + 1)°";
            this.checkBoxMrityuErnst.UseVisualStyleBackColor = true;
            this.checkBoxMrityuErnst.CheckedChanged += new System.EventHandler(this.checkBoxMrityuErnst_CheckedChanged);
            // 
            // labelExplanation
            // 
            this.labelExplanation.AutoSize = true;
            this.labelExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelExplanation.Location = new System.Drawing.Point(62, 124);
            this.labelExplanation.Name = "labelExplanation";
            this.labelExplanation.Size = new System.Drawing.Size(144, 13);
            this.labelExplanation.TabIndex = 6;
            this.labelExplanation.Text = "Where \"N\" is a Mrityu Bhaga";
            // 
            // checkBoxMrityuEqual
            // 
            this.checkBoxMrityuEqual.AutoSize = true;
            this.checkBoxMrityuEqual.Location = new System.Drawing.Point(13, 23);
            this.checkBoxMrityuEqual.Name = "checkBoxMrityuEqual";
            this.checkBoxMrityuEqual.Size = new System.Drawing.Size(180, 20);
            this.checkBoxMrityuEqual.TabIndex = 2;
            this.checkBoxMrityuEqual.Text = "From (N° - 30\') till (N° + 30\')";
            this.checkBoxMrityuEqual.UseVisualStyleBackColor = true;
            this.checkBoxMrityuEqual.CheckedChanged += new System.EventHandler(this.checkBoxMrityuEqual_CheckedChanged);
            // 
            // checkBoxMrityuMore
            // 
            this.checkBoxMrityuMore.AutoSize = true;
            this.checkBoxMrityuMore.Location = new System.Drawing.Point(13, 75);
            this.checkBoxMrityuMore.Name = "checkBoxMrityuMore";
            this.checkBoxMrityuMore.Size = new System.Drawing.Size(135, 20);
            this.checkBoxMrityuMore.TabIndex = 1;
            this.checkBoxMrityuMore.Text = "From N° till (N + 1)°";
            this.checkBoxMrityuMore.UseVisualStyleBackColor = true;
            this.checkBoxMrityuMore.CheckedChanged += new System.EventHandler(this.checkBoxMrityuMore_CheckedChanged);
            // 
            // checkBoxMrityuLess
            // 
            this.checkBoxMrityuLess.AutoSize = true;
            this.checkBoxMrityuLess.Location = new System.Drawing.Point(13, 49);
            this.checkBoxMrityuLess.Name = "checkBoxMrityuLess";
            this.checkBoxMrityuLess.Size = new System.Drawing.Size(132, 20);
            this.checkBoxMrityuLess.TabIndex = 0;
            this.checkBoxMrityuLess.Text = "From (N - 1)° till N°";
            this.checkBoxMrityuLess.UseVisualStyleBackColor = true;
            this.checkBoxMrityuLess.CheckedChanged += new System.EventHandler(this.checkBoxMrityuLess_CheckedChanged);
            // 
            // groupBoxNodes
            // 
            this.groupBoxNodes.Controls.Add(this.checkBoxNodeTrue);
            this.groupBoxNodes.Controls.Add(this.checkBoxNodeMean);
            this.groupBoxNodes.Location = new System.Drawing.Point(625, 148);
            this.groupBoxNodes.Name = "groupBoxNodes";
            this.groupBoxNodes.Size = new System.Drawing.Size(210, 75);
            this.groupBoxNodes.TabIndex = 17;
            this.groupBoxNodes.TabStop = false;
            this.groupBoxNodes.Text = "Nodes (Rahu and Ketu)";
            // 
            // checkBoxNodeTrue
            // 
            this.checkBoxNodeTrue.AutoSize = true;
            this.checkBoxNodeTrue.Location = new System.Drawing.Point(13, 45);
            this.checkBoxNodeTrue.Name = "checkBoxNodeTrue";
            this.checkBoxNodeTrue.Size = new System.Drawing.Size(96, 20);
            this.checkBoxNodeTrue.TabIndex = 1;
            this.checkBoxNodeTrue.Text = "True nodes";
            this.checkBoxNodeTrue.UseVisualStyleBackColor = true;
            this.checkBoxNodeTrue.CheckedChanged += new System.EventHandler(this.checkBoxNodeTrue_CheckedChanged);
            // 
            // checkBoxNodeMean
            // 
            this.checkBoxNodeMean.AutoSize = true;
            this.checkBoxNodeMean.Location = new System.Drawing.Point(13, 21);
            this.checkBoxNodeMean.Name = "checkBoxNodeMean";
            this.checkBoxNodeMean.Size = new System.Drawing.Size(102, 20);
            this.checkBoxNodeMean.TabIndex = 0;
            this.checkBoxNodeMean.Text = "Mean nodes";
            this.checkBoxNodeMean.UseVisualStyleBackColor = true;
            this.checkBoxNodeMean.CheckedChanged += new System.EventHandler(this.checkBoxNodeMean_CheckedChanged);
            // 
            // groupBoxWeek
            // 
            this.groupBoxWeek.Controls.Add(this.checkBoxWeekMonday);
            this.groupBoxWeek.Controls.Add(this.checkBoxWeekSunday);
            this.groupBoxWeek.Location = new System.Drawing.Point(625, 225);
            this.groupBoxWeek.Name = "groupBoxWeek";
            this.groupBoxWeek.Size = new System.Drawing.Size(210, 75);
            this.groupBoxWeek.TabIndex = 18;
            this.groupBoxWeek.TabStop = false;
            this.groupBoxWeek.Text = "Week start from";
            // 
            // checkBoxWeekMonday
            // 
            this.checkBoxWeekMonday.AutoSize = true;
            this.checkBoxWeekMonday.Location = new System.Drawing.Point(13, 45);
            this.checkBoxWeekMonday.Name = "checkBoxWeekMonday";
            this.checkBoxWeekMonday.Size = new System.Drawing.Size(76, 20);
            this.checkBoxWeekMonday.TabIndex = 1;
            this.checkBoxWeekMonday.Text = "Monday";
            this.checkBoxWeekMonday.UseVisualStyleBackColor = true;
            this.checkBoxWeekMonday.CheckedChanged += new System.EventHandler(this.checkBoxWeekMonday_CheckedChanged);
            // 
            // checkBoxWeekSunday
            // 
            this.checkBoxWeekSunday.AutoSize = true;
            this.checkBoxWeekSunday.Location = new System.Drawing.Point(13, 21);
            this.checkBoxWeekSunday.Name = "checkBoxWeekSunday";
            this.checkBoxWeekSunday.Size = new System.Drawing.Size(73, 20);
            this.checkBoxWeekSunday.TabIndex = 0;
            this.checkBoxWeekSunday.Text = "Sunday";
            this.checkBoxWeekSunday.UseVisualStyleBackColor = true;
            this.checkBoxWeekSunday.CheckedChanged += new System.EventHandler(this.checkBoxWeekSunday_CheckedChanged);
            // 
            // AppSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(848, 361);
            this.Controls.Add(this.groupBoxWeek);
            this.Controls.Add(this.groupBoxNodes);
            this.Controls.Add(this.groupBoxMrityuBhaga);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.groupBoxLanguage);
            this.Controls.Add(this.groupBoxMuhurtaGhati);
            this.Controls.Add(this.groupBoxHora);
            this.Controls.Add(this.groupBoxTranzits);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonApply);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Application settings";
            this.Shown += new System.EventHandler(this.AppSettings_Shown);
            this.groupBoxTranzits.ResumeLayout(false);
            this.groupBoxTranzits.PerformLayout();
            this.groupBoxHora.ResumeLayout(false);
            this.groupBoxHora.PerformLayout();
            this.groupBoxMuhurtaGhati.ResumeLayout(false);
            this.groupBoxMuhurtaGhati.PerformLayout();
            this.groupBoxLanguage.ResumeLayout(false);
            this.groupBoxLanguage.PerformLayout();
            this.groupBoxMrityuBhaga.ResumeLayout(false);
            this.groupBoxMrityuBhaga.PerformLayout();
            this.groupBoxNodes.ResumeLayout(false);
            this.groupBoxNodes.PerformLayout();
            this.groupBoxWeek.ResumeLayout(false);
            this.groupBoxWeek.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.GroupBox groupBoxTranzits;
        private System.Windows.Forms.CheckBox checkBoxBoth;
        private System.Windows.Forms.CheckBox checkBoxLagna;
        private System.Windows.Forms.CheckBox checkBoxMoon;
        private System.Windows.Forms.GroupBox groupBoxHora;
        private System.Windows.Forms.CheckBox checkBoxHoraEqual;
        private System.Windows.Forms.CheckBox checkBoxHoraSunRise;
        private System.Windows.Forms.GroupBox groupBoxMuhurtaGhati;
        private System.Windows.Forms.CheckBox checkBoxMuhurtsEqual;
        private System.Windows.Forms.CheckBox checkBoxMuhurtsSunRise;
        private System.Windows.Forms.GroupBox groupBoxLanguage;
        private System.Windows.Forms.CheckBox checkBoxEnlish;
        private System.Windows.Forms.CheckBox checkBoxRussian;
        private System.Windows.Forms.CheckBox checkBoxHoraFrom6;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Label labelRussian;
        private System.Windows.Forms.Label labelEnglish;
        private System.Windows.Forms.CheckBox checkBoxMuhurtsFrom6;
        private System.Windows.Forms.GroupBox groupBoxMrityuBhaga;
        private System.Windows.Forms.Label labelExplanation;
        private System.Windows.Forms.CheckBox checkBoxMrityuEqual;
        private System.Windows.Forms.CheckBox checkBoxMrityuMore;
        private System.Windows.Forms.CheckBox checkBoxMrityuLess;
        private System.Windows.Forms.GroupBox groupBoxNodes;
        private System.Windows.Forms.CheckBox checkBoxNodeTrue;
        private System.Windows.Forms.CheckBox checkBoxNodeMean;
        private System.Windows.Forms.GroupBox groupBoxWeek;
        private System.Windows.Forms.CheckBox checkBoxWeekMonday;
        private System.Windows.Forms.CheckBox checkBoxWeekSunday;
        private System.Windows.Forms.CheckBox checkBoxMrityuErnst;
    }
}