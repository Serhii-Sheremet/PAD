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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransitsMap));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripProfileMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxDate = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelDate = new System.Windows.Forms.ToolStripLabel();
            this.groupBoxEventInfo = new System.Windows.Forms.GroupBox();
            this.richTextBoxEventDesc = new System.Windows.Forms.RichTextBox();
            this.textBoxEvent = new System.Windows.Forms.TextBox();
            this.buttonLivingPlace = new System.Windows.Forms.Button();
            this.textBoxLivingPlace = new System.Windows.Forms.TextBox();
            this.groupBoxDateInput = new System.Windows.Forms.GroupBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.maskedTextBoxDate = new System.Windows.Forms.MaskedTextBox();
            this.comboBoxDateStep = new System.Windows.Forms.ComboBox();
            this.textBoxStep = new System.Windows.Forms.TextBox();
            this.arrowButtonRight = new ArrowButton.ArrowButton();
            this.arrowButtonLeft = new ArrowButton.ArrowButton();
            this.panelTranzits = new System.Windows.Forms.Panel();
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
            this.labelTranzit = new System.Windows.Forms.Label();
            this.labelNatal = new System.Windows.Forms.Label();
            this.dataGridViewInfoNatal = new System.Windows.Forms.DataGridView();
            this.dataGridViewInfoTranzit = new System.Windows.Forms.DataGridView();
            this.labelLagna = new System.Windows.Forms.Label();
            this.labelNatalMoon = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.labelPeriodRuler = new System.Windows.Forms.Label();
            this.comboBoxRuler = new System.Windows.Forms.ComboBox();
            this.labelNatalNavamsa = new System.Windows.Forms.Label();
            this.labelTransitNavamsa = new System.Windows.Forms.Label();
            this.pictureBoxTransitNavamsa = new System.Windows.Forms.PictureBox();
            this.pictureBoxNatalNavamsa = new System.Windows.Forms.PictureBox();
            this.pictureBoxPeriodRuler = new System.Windows.Forms.PictureBox();
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.pictureBoxMapLagna = new System.Windows.Forms.PictureBox();
            this.pictureBoxMapMoon = new System.Windows.Forms.PictureBox();
            this.toolStripProfileMenu.SuspendLayout();
            this.groupBoxEventInfo.SuspendLayout();
            this.groupBoxDateInput.SuspendLayout();
            this.groupBoxAspects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfoNatal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfoTranzit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransitNavamsa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNatalNavamsa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPeriodRuler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapLagna)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapMoon)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripProfileMenu
            // 
            this.toolStripProfileMenu.AutoSize = false;
            this.toolStripProfileMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripProfileMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripProfileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonPreview,
            this.toolStripSeparator1,
            this.toolStripButtonRefresh,
            this.toolStripLabel2,
            this.toolStripTextBoxDate,
            this.toolStripLabelDate});
            this.toolStripProfileMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripProfileMenu.Name = "toolStripProfileMenu";
            this.toolStripProfileMenu.Size = new System.Drawing.Size(1178, 28);
            this.toolStripProfileMenu.TabIndex = 64;
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(28, 25);
            this.toolStripButtonAdd.Text = "Add Event";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonPreview
            // 
            this.toolStripButtonPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPreview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPreview.Image")));
            this.toolStripButtonPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreview.Name = "toolStripButtonPreview";
            this.toolStripButtonPreview.Size = new System.Drawing.Size(28, 25);
            this.toolStripButtonPreview.Text = "Preview Event";
            this.toolStripButtonPreview.Click += new System.EventHandler(this.toolStripButtonPreview_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(28, 25);
            this.toolStripButtonRefresh.Text = "Refresh";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(10, 36);
            // 
            // toolStripTextBoxDate
            // 
            this.toolStripTextBoxDate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxDate.AutoSize = false;
            this.toolStripTextBoxDate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolStripTextBoxDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolStripTextBoxDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripTextBoxDate.Name = "toolStripTextBoxDate";
            this.toolStripTextBoxDate.ReadOnly = true;
            this.toolStripTextBoxDate.Size = new System.Drawing.Size(140, 15);
            this.toolStripTextBoxDate.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripTextBoxDate.TextChanged += new System.EventHandler(this.toolStripTextBoxDate_TextChanged);
            // 
            // toolStripLabelDate
            // 
            this.toolStripLabelDate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabelDate.Name = "toolStripLabelDate";
            this.toolStripLabelDate.Size = new System.Drawing.Size(92, 25);
            this.toolStripLabelDate.Text = "Transit Date";
            // 
            // groupBoxEventInfo
            // 
            this.groupBoxEventInfo.Controls.Add(this.richTextBoxEventDesc);
            this.groupBoxEventInfo.Controls.Add(this.textBoxEvent);
            this.groupBoxEventInfo.Controls.Add(this.buttonLivingPlace);
            this.groupBoxEventInfo.Controls.Add(this.textBoxLivingPlace);
            this.groupBoxEventInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxEventInfo.Location = new System.Drawing.Point(576, 448);
            this.groupBoxEventInfo.Name = "groupBoxEventInfo";
            this.groupBoxEventInfo.Size = new System.Drawing.Size(266, 155);
            this.groupBoxEventInfo.TabIndex = 71;
            this.groupBoxEventInfo.TabStop = false;
            this.groupBoxEventInfo.Text = "Information";
            // 
            // richTextBoxEventDesc
            // 
            this.richTextBoxEventDesc.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.richTextBoxEventDesc.Location = new System.Drawing.Point(5, 83);
            this.richTextBoxEventDesc.Name = "richTextBoxEventDesc";
            this.richTextBoxEventDesc.ReadOnly = true;
            this.richTextBoxEventDesc.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxEventDesc.Size = new System.Drawing.Size(113, 64);
            this.richTextBoxEventDesc.TabIndex = 77;
            this.richTextBoxEventDesc.Text = "";
            // 
            // textBoxEvent
            // 
            this.textBoxEvent.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxEvent.Location = new System.Drawing.Point(5, 41);
            this.textBoxEvent.MaxLength = 2;
            this.textBoxEvent.Name = "textBoxEvent";
            this.textBoxEvent.ReadOnly = true;
            this.textBoxEvent.Size = new System.Drawing.Size(222, 20);
            this.textBoxEvent.TabIndex = 76;
            // 
            // buttonLivingPlace
            // 
            this.buttonLivingPlace.BackColor = System.Drawing.SystemColors.Window;
            this.buttonLivingPlace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLivingPlace.Image = ((System.Drawing.Image)(resources.GetObject("buttonLivingPlace.Image")));
            this.buttonLivingPlace.Location = new System.Drawing.Point(167, 16);
            this.buttonLivingPlace.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLivingPlace.Name = "buttonLivingPlace";
            this.buttonLivingPlace.Size = new System.Drawing.Size(30, 22);
            this.buttonLivingPlace.TabIndex = 65;
            this.buttonLivingPlace.UseVisualStyleBackColor = false;
            this.buttonLivingPlace.Click += new System.EventHandler(this.buttonLivingPlace_Click);
            // 
            // textBoxLivingPlace
            // 
            this.textBoxLivingPlace.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxLivingPlace.Location = new System.Drawing.Point(5, 18);
            this.textBoxLivingPlace.Name = "textBoxLivingPlace";
            this.textBoxLivingPlace.ReadOnly = true;
            this.textBoxLivingPlace.Size = new System.Drawing.Size(160, 20);
            this.textBoxLivingPlace.TabIndex = 66;
            // 
            // groupBoxDateInput
            // 
            this.groupBoxDateInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDateInput.Controls.Add(this.buttonApply);
            this.groupBoxDateInput.Controls.Add(this.maskedTextBoxDate);
            this.groupBoxDateInput.Controls.Add(this.comboBoxDateStep);
            this.groupBoxDateInput.Controls.Add(this.textBoxStep);
            this.groupBoxDateInput.Controls.Add(this.arrowButtonRight);
            this.groupBoxDateInput.Controls.Add(this.arrowButtonLeft);
            this.groupBoxDateInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxDateInput.Location = new System.Drawing.Point(311, 366);
            this.groupBoxDateInput.Name = "groupBoxDateInput";
            this.groupBoxDateInput.Size = new System.Drawing.Size(210, 109);
            this.groupBoxDateInput.TabIndex = 65;
            this.groupBoxDateInput.TabStop = false;
            this.groupBoxDateInput.Text = "Setting date";
            // 
            // buttonApply
            // 
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonApply.Location = new System.Drawing.Point(46, 78);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(100, 24);
            this.buttonApply.TabIndex = 103;
            this.buttonApply.Text = "Apply new Date";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // maskedTextBoxDate
            // 
            this.maskedTextBoxDate.BackColor = System.Drawing.SystemColors.Window;
            this.maskedTextBoxDate.Location = new System.Drawing.Point(8, 19);
            this.maskedTextBoxDate.Mask = "00/00/0000 90:00:00";
            this.maskedTextBoxDate.Name = "maskedTextBoxDate";
            this.maskedTextBoxDate.Size = new System.Drawing.Size(194, 20);
            this.maskedTextBoxDate.TabIndex = 60;
            this.maskedTextBoxDate.ValidatingType = typeof(System.DateTime);
            this.maskedTextBoxDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedTextBoxDate_KeyPress);
            // 
            // comboBoxDateStep
            // 
            this.comboBoxDateStep.FormattingEnabled = true;
            this.comboBoxDateStep.Location = new System.Drawing.Point(32, 45);
            this.comboBoxDateStep.Name = "comboBoxDateStep";
            this.comboBoxDateStep.Size = new System.Drawing.Size(120, 21);
            this.comboBoxDateStep.TabIndex = 59;
            this.comboBoxDateStep.SelectedIndexChanged += new System.EventHandler(this.comboBoxDateStep_SelectedIndexChanged);
            // 
            // textBoxStep
            // 
            this.textBoxStep.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxStep.Location = new System.Drawing.Point(184, 44);
            this.textBoxStep.MaxLength = 2;
            this.textBoxStep.Name = "textBoxStep";
            this.textBoxStep.Size = new System.Drawing.Size(18, 20);
            this.textBoxStep.TabIndex = 58;
            this.textBoxStep.Text = "1";
            this.textBoxStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxStep.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxStep_KeyPress);
            // 
            // arrowButtonRight
            // 
            this.arrowButtonRight.ArrowEnabled = true;
            this.arrowButtonRight.HoverEndColor = System.Drawing.Color.Silver;
            this.arrowButtonRight.HoverStartColor = System.Drawing.Color.Silver;
            this.arrowButtonRight.Location = new System.Drawing.Point(156, 43);
            this.arrowButtonRight.Name = "arrowButtonRight";
            this.arrowButtonRight.NormalEndColor = System.Drawing.Color.Silver;
            this.arrowButtonRight.NormalStartColor = System.Drawing.Color.Silver;
            this.arrowButtonRight.Rotation = 90;
            this.arrowButtonRight.Size = new System.Drawing.Size(24, 24);
            this.arrowButtonRight.TabIndex = 57;
            this.arrowButtonRight.Click += new System.EventHandler(this.arrowButtonRight_Click);
            // 
            // arrowButtonLeft
            // 
            this.arrowButtonLeft.ArrowEnabled = true;
            this.arrowButtonLeft.HoverEndColor = System.Drawing.Color.Silver;
            this.arrowButtonLeft.HoverStartColor = System.Drawing.Color.Silver;
            this.arrowButtonLeft.Location = new System.Drawing.Point(3, 43);
            this.arrowButtonLeft.Name = "arrowButtonLeft";
            this.arrowButtonLeft.NormalEndColor = System.Drawing.Color.Silver;
            this.arrowButtonLeft.NormalStartColor = System.Drawing.Color.Silver;
            this.arrowButtonLeft.Rotation = 270;
            this.arrowButtonLeft.Size = new System.Drawing.Size(24, 24);
            this.arrowButtonLeft.TabIndex = 55;
            this.arrowButtonLeft.Click += new System.EventHandler(this.arrowButtonLeft_Click);
            // 
            // panelTranzits
            // 
            this.panelTranzits.AutoSize = true;
            this.panelTranzits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTranzits.Location = new System.Drawing.Point(0, 42);
            this.panelTranzits.Name = "panelTranzits";
            this.panelTranzits.Size = new System.Drawing.Size(0, 0);
            this.panelTranzits.TabIndex = 72;
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
            this.groupBoxAspects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxAspects.Location = new System.Drawing.Point(1027, 409);
            this.groupBoxAspects.Name = "groupBoxAspects";
            this.groupBoxAspects.Size = new System.Drawing.Size(124, 208);
            this.groupBoxAspects.TabIndex = 81;
            this.groupBoxAspects.TabStop = false;
            this.groupBoxAspects.Text = "Aspects";
            // 
            // checkBoxRahu
            // 
            this.checkBoxRahu.AutoSize = true;
            this.checkBoxRahu.Location = new System.Drawing.Point(7, 186);
            this.checkBoxRahu.Name = "checkBoxRahu";
            this.checkBoxRahu.Size = new System.Drawing.Size(52, 17);
            this.checkBoxRahu.TabIndex = 8;
            this.checkBoxRahu.Text = "Rahu";
            this.checkBoxRahu.UseVisualStyleBackColor = true;
            this.checkBoxRahu.CheckedChanged += new System.EventHandler(this.checkBoxRahu_CheckedChanged);
            // 
            // checkBoxSaturn
            // 
            this.checkBoxSaturn.AutoSize = true;
            this.checkBoxSaturn.Location = new System.Drawing.Point(7, 165);
            this.checkBoxSaturn.Name = "checkBoxSaturn";
            this.checkBoxSaturn.Size = new System.Drawing.Size(57, 17);
            this.checkBoxSaturn.TabIndex = 7;
            this.checkBoxSaturn.Text = "Saturn";
            this.checkBoxSaturn.UseVisualStyleBackColor = true;
            this.checkBoxSaturn.CheckedChanged += new System.EventHandler(this.checkBoxSaturn_CheckedChanged);
            // 
            // checkBoxMars
            // 
            this.checkBoxMars.AutoSize = true;
            this.checkBoxMars.Location = new System.Drawing.Point(7, 81);
            this.checkBoxMars.Name = "checkBoxMars";
            this.checkBoxMars.Size = new System.Drawing.Size(49, 17);
            this.checkBoxMars.TabIndex = 6;
            this.checkBoxMars.Text = "Mars";
            this.checkBoxMars.UseVisualStyleBackColor = true;
            this.checkBoxMars.CheckedChanged += new System.EventHandler(this.checkBoxMars_CheckedChanged);
            // 
            // checkBoxMercury
            // 
            this.checkBoxMercury.AutoSize = true;
            this.checkBoxMercury.Location = new System.Drawing.Point(7, 102);
            this.checkBoxMercury.Name = "checkBoxMercury";
            this.checkBoxMercury.Size = new System.Drawing.Size(64, 17);
            this.checkBoxMercury.TabIndex = 5;
            this.checkBoxMercury.Text = "Mercury";
            this.checkBoxMercury.UseVisualStyleBackColor = true;
            this.checkBoxMercury.CheckedChanged += new System.EventHandler(this.checkBoxMercury_CheckedChanged);
            // 
            // checkBoxJupiter
            // 
            this.checkBoxJupiter.AutoSize = true;
            this.checkBoxJupiter.Location = new System.Drawing.Point(7, 123);
            this.checkBoxJupiter.Name = "checkBoxJupiter";
            this.checkBoxJupiter.Size = new System.Drawing.Size(57, 17);
            this.checkBoxJupiter.TabIndex = 4;
            this.checkBoxJupiter.Text = "Jupiter";
            this.checkBoxJupiter.UseVisualStyleBackColor = true;
            this.checkBoxJupiter.CheckedChanged += new System.EventHandler(this.checkBoxJupiter_CheckedChanged);
            // 
            // checkBoxVenus
            // 
            this.checkBoxVenus.AutoSize = true;
            this.checkBoxVenus.Location = new System.Drawing.Point(7, 144);
            this.checkBoxVenus.Name = "checkBoxVenus";
            this.checkBoxVenus.Size = new System.Drawing.Size(56, 17);
            this.checkBoxVenus.TabIndex = 3;
            this.checkBoxVenus.Text = "Venus";
            this.checkBoxVenus.UseVisualStyleBackColor = true;
            this.checkBoxVenus.CheckedChanged += new System.EventHandler(this.checkBoxVenus_CheckedChanged);
            // 
            // checkBoxSun
            // 
            this.checkBoxSun.AutoSize = true;
            this.checkBoxSun.Location = new System.Drawing.Point(7, 39);
            this.checkBoxSun.Name = "checkBoxSun";
            this.checkBoxSun.Size = new System.Drawing.Size(45, 17);
            this.checkBoxSun.TabIndex = 2;
            this.checkBoxSun.Text = "Sun";
            this.checkBoxSun.UseVisualStyleBackColor = true;
            this.checkBoxSun.CheckedChanged += new System.EventHandler(this.checkBoxSun_CheckedChanged);
            // 
            // checkBoxMoon
            // 
            this.checkBoxMoon.AutoSize = true;
            this.checkBoxMoon.Location = new System.Drawing.Point(7, 60);
            this.checkBoxMoon.Name = "checkBoxMoon";
            this.checkBoxMoon.Size = new System.Drawing.Size(53, 17);
            this.checkBoxMoon.TabIndex = 1;
            this.checkBoxMoon.Text = "Moon";
            this.checkBoxMoon.UseVisualStyleBackColor = true;
            this.checkBoxMoon.CheckedChanged += new System.EventHandler(this.checkBoxMoon_CheckedChanged);
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Location = new System.Drawing.Point(7, 18);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(37, 17);
            this.checkBoxAll.TabIndex = 0;
            this.checkBoxAll.Text = "All";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // labelTranzit
            // 
            this.labelTranzit.AutoSize = true;
            this.labelTranzit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTranzit.Location = new System.Drawing.Point(765, 228);
            this.labelTranzit.Name = "labelTranzit";
            this.labelTranzit.Size = new System.Drawing.Size(127, 13);
            this.labelTranzit.TabIndex = 92;
            this.labelTranzit.Text = "Transit position of planets";
            // 
            // labelNatal
            // 
            this.labelNatal.AutoSize = true;
            this.labelNatal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNatal.Location = new System.Drawing.Point(469, 228);
            this.labelNatal.Name = "labelNatal";
            this.labelNatal.Size = new System.Drawing.Size(120, 13);
            this.labelNatal.TabIndex = 91;
            this.labelNatal.Text = "Natal position of planets";
            // 
            // dataGridViewInfoNatal
            // 
            this.dataGridViewInfoNatal.AllowUserToAddRows = false;
            this.dataGridViewInfoNatal.AllowUserToDeleteRows = false;
            this.dataGridViewInfoNatal.AllowUserToResizeColumns = false;
            this.dataGridViewInfoNatal.AllowUserToResizeRows = false;
            this.dataGridViewInfoNatal.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewInfoNatal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInfoNatal.Location = new System.Drawing.Point(472, 247);
            this.dataGridViewInfoNatal.MultiSelect = false;
            this.dataGridViewInfoNatal.Name = "dataGridViewInfoNatal";
            this.dataGridViewInfoNatal.ReadOnly = true;
            this.dataGridViewInfoNatal.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewInfoNatal.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewInfoNatal.Size = new System.Drawing.Size(216, 108);
            this.dataGridViewInfoNatal.TabIndex = 90;
            // 
            // dataGridViewInfoTranzit
            // 
            this.dataGridViewInfoTranzit.AllowUserToAddRows = false;
            this.dataGridViewInfoTranzit.AllowUserToDeleteRows = false;
            this.dataGridViewInfoTranzit.AllowUserToResizeColumns = false;
            this.dataGridViewInfoTranzit.AllowUserToResizeRows = false;
            this.dataGridViewInfoTranzit.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewInfoTranzit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInfoTranzit.Location = new System.Drawing.Point(760, 244);
            this.dataGridViewInfoTranzit.MultiSelect = false;
            this.dataGridViewInfoTranzit.Name = "dataGridViewInfoTranzit";
            this.dataGridViewInfoTranzit.ReadOnly = true;
            this.dataGridViewInfoTranzit.RowHeadersVisible = false;
            this.dataGridViewInfoTranzit.Size = new System.Drawing.Size(207, 94);
            this.dataGridViewInfoTranzit.TabIndex = 89;
            // 
            // labelLagna
            // 
            this.labelLagna.AutoSize = true;
            this.labelLagna.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLagna.Location = new System.Drawing.Point(8, 46);
            this.labelLagna.Name = "labelLagna";
            this.labelLagna.Size = new System.Drawing.Size(100, 13);
            this.labelLagna.TabIndex = 88;
            this.labelLagna.Text = "Transits from Lagna";
            // 
            // labelNatalMoon
            // 
            this.labelNatalMoon.AutoSize = true;
            this.labelNatalMoon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNatalMoon.Location = new System.Drawing.Point(353, 46);
            this.labelNatalMoon.Name = "labelNatalMoon";
            this.labelNatalMoon.Size = new System.Drawing.Size(125, 13);
            this.labelNatalMoon.TabIndex = 87;
            this.labelNatalMoon.Text = "Transits from Natal Moon";
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCurrent.Location = new System.Drawing.Point(678, 46);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(81, 13);
            this.labelCurrent.TabIndex = 94;
            this.labelCurrent.Text = "Current Transits";
            // 
            // labelPeriodRuler
            // 
            this.labelPeriodRuler.AutoSize = true;
            this.labelPeriodRuler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPeriodRuler.Location = new System.Drawing.Point(13, 237);
            this.labelPeriodRuler.Name = "labelPeriodRuler";
            this.labelPeriodRuler.Size = new System.Drawing.Size(128, 13);
            this.labelPeriodRuler.TabIndex = 96;
            this.labelPeriodRuler.Text = "Transits from Period Ruler";
            // 
            // comboBoxRuler
            // 
            this.comboBoxRuler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxRuler.FormattingEnabled = true;
            this.comboBoxRuler.Location = new System.Drawing.Point(181, 229);
            this.comboBoxRuler.Name = "comboBoxRuler";
            this.comboBoxRuler.Size = new System.Drawing.Size(140, 21);
            this.comboBoxRuler.TabIndex = 97;
            this.comboBoxRuler.SelectedIndexChanged += new System.EventHandler(this.comboBoxRuler_SelectedIndexChanged);
            // 
            // labelNatalNavamsa
            // 
            this.labelNatalNavamsa.AutoSize = true;
            this.labelNatalNavamsa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNatalNavamsa.Location = new System.Drawing.Point(18, 468);
            this.labelNatalNavamsa.Name = "labelNatalNavamsa";
            this.labelNatalNavamsa.Size = new System.Drawing.Size(80, 13);
            this.labelNatalNavamsa.TabIndex = 100;
            this.labelNatalNavamsa.Text = "Natal Navamsa";
            // 
            // labelTransitNavamsa
            // 
            this.labelTransitNavamsa.AutoSize = true;
            this.labelTransitNavamsa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTransitNavamsa.Location = new System.Drawing.Point(274, 478);
            this.labelTransitNavamsa.Name = "labelTransitNavamsa";
            this.labelTransitNavamsa.Size = new System.Drawing.Size(87, 13);
            this.labelTransitNavamsa.TabIndex = 101;
            this.labelTransitNavamsa.Text = "Transit Navamsa";
            // 
            // pictureBoxTransitNavamsa
            // 
            this.pictureBoxTransitNavamsa.Location = new System.Drawing.Point(277, 497);
            this.pictureBoxTransitNavamsa.Name = "pictureBoxTransitNavamsa";
            this.pictureBoxTransitNavamsa.Size = new System.Drawing.Size(232, 98);
            this.pictureBoxTransitNavamsa.TabIndex = 99;
            this.pictureBoxTransitNavamsa.TabStop = false;
            // 
            // pictureBoxNatalNavamsa
            // 
            this.pictureBoxNatalNavamsa.Location = new System.Drawing.Point(21, 487);
            this.pictureBoxNatalNavamsa.Name = "pictureBoxNatalNavamsa";
            this.pictureBoxNatalNavamsa.Size = new System.Drawing.Size(232, 104);
            this.pictureBoxNatalNavamsa.TabIndex = 98;
            this.pictureBoxNatalNavamsa.TabStop = false;
            // 
            // pictureBoxPeriodRuler
            // 
            this.pictureBoxPeriodRuler.Location = new System.Drawing.Point(12, 256);
            this.pictureBoxPeriodRuler.Name = "pictureBoxPeriodRuler";
            this.pictureBoxPeriodRuler.Size = new System.Drawing.Size(232, 181);
            this.pictureBoxPeriodRuler.TabIndex = 95;
            this.pictureBoxPeriodRuler.TabStop = false;
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Location = new System.Drawing.Point(681, 69);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(200, 144);
            this.pictureBoxMap.TabIndex = 93;
            this.pictureBoxMap.TabStop = false;
            // 
            // pictureBoxMapLagna
            // 
            this.pictureBoxMapLagna.Location = new System.Drawing.Point(11, 69);
            this.pictureBoxMapLagna.Name = "pictureBoxMapLagna";
            this.pictureBoxMapLagna.Size = new System.Drawing.Size(242, 157);
            this.pictureBoxMapLagna.TabIndex = 86;
            this.pictureBoxMapLagna.TabStop = false;
            // 
            // pictureBoxMapMoon
            // 
            this.pictureBoxMapMoon.Location = new System.Drawing.Point(356, 69);
            this.pictureBoxMapMoon.Name = "pictureBoxMapMoon";
            this.pictureBoxMapMoon.Size = new System.Drawing.Size(198, 157);
            this.pictureBoxMapMoon.TabIndex = 85;
            this.pictureBoxMapMoon.TabStop = false;
            // 
            // TransitsMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1178, 691);
            this.Controls.Add(this.labelTransitNavamsa);
            this.Controls.Add(this.labelNatalNavamsa);
            this.Controls.Add(this.pictureBoxTransitNavamsa);
            this.Controls.Add(this.pictureBoxNatalNavamsa);
            this.Controls.Add(this.comboBoxRuler);
            this.Controls.Add(this.labelPeriodRuler);
            this.Controls.Add(this.pictureBoxPeriodRuler);
            this.Controls.Add(this.labelCurrent);
            this.Controls.Add(this.pictureBoxMap);
            this.Controls.Add(this.labelTranzit);
            this.Controls.Add(this.labelNatal);
            this.Controls.Add(this.dataGridViewInfoNatal);
            this.Controls.Add(this.dataGridViewInfoTranzit);
            this.Controls.Add(this.labelLagna);
            this.Controls.Add(this.labelNatalMoon);
            this.Controls.Add(this.pictureBoxMapLagna);
            this.Controls.Add(this.pictureBoxMapMoon);
            this.Controls.Add(this.groupBoxAspects);
            this.Controls.Add(this.panelTranzits);
            this.Controls.Add(this.groupBoxEventInfo);
            this.Controls.Add(this.groupBoxDateInput);
            this.Controls.Add(this.toolStripProfileMenu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransitsMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transit Chart";
            this.Shown += new System.EventHandler(this.TransitsMap_Shown);
            this.toolStripProfileMenu.ResumeLayout(false);
            this.toolStripProfileMenu.PerformLayout();
            this.groupBoxEventInfo.ResumeLayout(false);
            this.groupBoxEventInfo.PerformLayout();
            this.groupBoxDateInput.ResumeLayout(false);
            this.groupBoxDateInput.PerformLayout();
            this.groupBoxAspects.ResumeLayout(false);
            this.groupBoxAspects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfoNatal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfoTranzit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransitNavamsa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNatalNavamsa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPeriodRuler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapLagna)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapMoon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripProfileMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelDate;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxDate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.GroupBox groupBoxEventInfo;
        private System.Windows.Forms.Button buttonLivingPlace;
        private System.Windows.Forms.TextBox textBoxLivingPlace;
        private System.Windows.Forms.GroupBox groupBoxDateInput;
        private System.Windows.Forms.TextBox textBoxStep;
        private ArrowButton.ArrowButton arrowButtonRight;
        private ArrowButton.ArrowButton arrowButtonLeft;
        private System.Windows.Forms.RichTextBox richTextBoxEventDesc;
        private System.Windows.Forms.TextBox textBoxEvent;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreview;
        private System.Windows.Forms.Panel panelTranzits;
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
        private System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.Label labelTranzit;
        private System.Windows.Forms.Label labelNatal;
        private System.Windows.Forms.DataGridView dataGridViewInfoNatal;
        private System.Windows.Forms.DataGridView dataGridViewInfoTranzit;
        private System.Windows.Forms.Label labelLagna;
        private System.Windows.Forms.Label labelNatalMoon;
        private System.Windows.Forms.PictureBox pictureBoxMapLagna;
        private System.Windows.Forms.PictureBox pictureBoxMapMoon;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.PictureBox pictureBoxPeriodRuler;
        private System.Windows.Forms.Label labelPeriodRuler;
        private System.Windows.Forms.ComboBox comboBoxRuler;
        private System.Windows.Forms.PictureBox pictureBoxTransitNavamsa;
        private System.Windows.Forms.PictureBox pictureBoxNatalNavamsa;
        private System.Windows.Forms.Label labelNatalNavamsa;
        private System.Windows.Forms.Label labelTransitNavamsa;
        private System.Windows.Forms.ComboBox comboBoxDateStep;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDate;
        private System.Windows.Forms.Button buttonApply;
    }
}