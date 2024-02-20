namespace PAD
{
    partial class ProfileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileForm));
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelProfiles = new System.Windows.Forms.Panel();
            this.listViewProfile = new System.Windows.Forms.ListView();
            this.labelPersonSurname = new System.Windows.Forms.Label();
            this.textBoxPersonSurname = new System.Windows.Forms.TextBox();
            this.labelPersonName = new System.Windows.Forms.Label();
            this.textBoxPersonName = new System.Windows.Forms.TextBox();
            this.labelProfileName = new System.Windows.Forms.Label();
            this.textBoxProfileName = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonChoose = new System.Windows.Forms.Button();
            this.toolStripProfileMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.labelLivingPlace = new System.Windows.Forms.Label();
            this.textBoxLivingPlace = new System.Windows.Forms.TextBox();
            this.buttonLivingPlace = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.dataGridViewInfo = new System.Windows.Forms.DataGridView();
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
            this.labelDateBirth = new System.Windows.Forms.Label();
            this.maskedTextBoxDate = new System.Windows.Forms.MaskedTextBox();
            this.buttonBirthPlace = new System.Windows.Forms.Button();
            this.labelPlaceBirth = new System.Windows.Forms.Label();
            this.textBoxBirthPlace = new System.Windows.Forms.TextBox();
            this.buttonGenerateMap = new System.Windows.Forms.Button();
            this.buttonSaveProfile = new System.Windows.Forms.Button();
            this.richTextBoxNotes = new System.Windows.Forms.RichTextBox();
            this.panelProfiles.SuspendLayout();
            this.toolStripProfileMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).BeginInit();
            this.groupBoxAspects.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(930, 592);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 32);
            this.buttonClose.TabIndex = 42;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panelProfiles
            // 
            this.panelProfiles.Controls.Add(this.listViewProfile);
            this.panelProfiles.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelProfiles.Location = new System.Drawing.Point(0, 0);
            this.panelProfiles.Name = "panelProfiles";
            this.panelProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.panelProfiles.Size = new System.Drawing.Size(200, 631);
            this.panelProfiles.TabIndex = 17;
            // 
            // listViewProfile
            // 
            this.listViewProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewProfile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewProfile.HideSelection = false;
            this.listViewProfile.Location = new System.Drawing.Point(3, 3);
            this.listViewProfile.MultiSelect = false;
            this.listViewProfile.Name = "listViewProfile";
            this.listViewProfile.Size = new System.Drawing.Size(194, 625);
            this.listViewProfile.TabIndex = 0;
            this.listViewProfile.UseCompatibleStateImageBehavior = false;
            this.listViewProfile.View = System.Windows.Forms.View.Details;
            this.listViewProfile.SelectedIndexChanged += new System.EventHandler(this.listViewProfile_SelectedIndexChanged);
            // 
            // labelPersonSurname
            // 
            this.labelPersonSurname.AutoSize = true;
            this.labelPersonSurname.Location = new System.Drawing.Point(715, 127);
            this.labelPersonSurname.Name = "labelPersonSurname";
            this.labelPersonSurname.Size = new System.Drawing.Size(61, 16);
            this.labelPersonSurname.TabIndex = 25;
            this.labelPersonSurname.Text = "Surname";
            // 
            // textBoxPersonSurname
            // 
            this.textBoxPersonSurname.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxPersonSurname.Location = new System.Drawing.Point(810, 123);
            this.textBoxPersonSurname.Name = "textBoxPersonSurname";
            this.textBoxPersonSurname.ReadOnly = true;
            this.textBoxPersonSurname.Size = new System.Drawing.Size(200, 22);
            this.textBoxPersonSurname.TabIndex = 24;
            // 
            // labelPersonName
            // 
            this.labelPersonName.AutoSize = true;
            this.labelPersonName.Location = new System.Drawing.Point(715, 99);
            this.labelPersonName.Name = "labelPersonName";
            this.labelPersonName.Size = new System.Drawing.Size(44, 16);
            this.labelPersonName.TabIndex = 23;
            this.labelPersonName.Text = "Name";
            // 
            // textBoxPersonName
            // 
            this.textBoxPersonName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxPersonName.Location = new System.Drawing.Point(810, 96);
            this.textBoxPersonName.Name = "textBoxPersonName";
            this.textBoxPersonName.ReadOnly = true;
            this.textBoxPersonName.Size = new System.Drawing.Size(200, 22);
            this.textBoxPersonName.TabIndex = 22;
            // 
            // labelProfileName
            // 
            this.labelProfileName.AutoSize = true;
            this.labelProfileName.Location = new System.Drawing.Point(715, 72);
            this.labelProfileName.Name = "labelProfileName";
            this.labelProfileName.Size = new System.Drawing.Size(85, 16);
            this.labelProfileName.TabIndex = 21;
            this.labelProfileName.Text = "Profile Name";
            // 
            // textBoxProfileName
            // 
            this.textBoxProfileName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxProfileName.Location = new System.Drawing.Point(810, 69);
            this.textBoxProfileName.Name = "textBoxProfileName";
            this.textBoxProfileName.ReadOnly = true;
            this.textBoxProfileName.Size = new System.Drawing.Size(200, 22);
            this.textBoxProfileName.TabIndex = 20;
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(715, 46);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(53, 16);
            this.labelSearch.TabIndex = 19;
            this.labelSearch.Text = "Search:";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(810, 43);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(200, 22);
            this.textBoxSearch.TabIndex = 18;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // buttonChoose
            // 
            this.buttonChoose.Enabled = false;
            this.buttonChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChoose.Location = new System.Drawing.Point(844, 592);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(80, 32);
            this.buttonChoose.TabIndex = 40;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // toolStripProfileMenu
            // 
            this.toolStripProfileMenu.AutoSize = false;
            this.toolStripProfileMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripProfileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripSeparator1,
            this.toolStripButtonSave});
            this.toolStripProfileMenu.Location = new System.Drawing.Point(200, 0);
            this.toolStripProfileMenu.Name = "toolStripProfileMenu";
            this.toolStripProfileMenu.Size = new System.Drawing.Size(816, 39);
            this.toolStripProfileMenu.TabIndex = 33;
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Enabled = false;
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonAdd.Text = "Add profile";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Enabled = false;
            this.toolStripButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEdit.Image")));
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonEdit.Text = "Edit profile";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Enabled = false;
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonDelete.Text = "Delete profile";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Enabled = false;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSave.Text = "Save changes";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // labelLivingPlace
            // 
            this.labelLivingPlace.AutoSize = true;
            this.labelLivingPlace.Location = new System.Drawing.Point(715, 209);
            this.labelLivingPlace.Name = "labelLivingPlace";
            this.labelLivingPlace.Size = new System.Drawing.Size(94, 16);
            this.labelLivingPlace.TabIndex = 37;
            this.labelLivingPlace.Text = "Place of Living";
            // 
            // textBoxLivingPlace
            // 
            this.textBoxLivingPlace.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLivingPlace.Location = new System.Drawing.Point(811, 206);
            this.textBoxLivingPlace.Name = "textBoxLivingPlace";
            this.textBoxLivingPlace.ReadOnly = true;
            this.textBoxLivingPlace.Size = new System.Drawing.Size(168, 22);
            this.textBoxLivingPlace.TabIndex = 36;
            // 
            // buttonLivingPlace
            // 
            this.buttonLivingPlace.Enabled = false;
            this.buttonLivingPlace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLivingPlace.Image = ((System.Drawing.Image)(resources.GetObject("buttonLivingPlace.Image")));
            this.buttonLivingPlace.Location = new System.Drawing.Point(980, 206);
            this.buttonLivingPlace.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLivingPlace.Name = "buttonLivingPlace";
            this.buttonLivingPlace.Size = new System.Drawing.Size(30, 22);
            this.buttonLivingPlace.TabIndex = 30;
            this.buttonLivingPlace.UseVisualStyleBackColor = true;
            this.buttonLivingPlace.Click += new System.EventHandler(this.buttonLivingPlace_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Enabled = false;
            this.buttonDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDefault.Location = new System.Drawing.Point(718, 592);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(91, 32);
            this.buttonDefault.TabIndex = 38;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Location = new System.Drawing.Point(205, 44);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(500, 350);
            this.pictureBoxMap.TabIndex = 44;
            this.pictureBoxMap.TabStop = false;
            // 
            // dataGridViewInfo
            // 
            this.dataGridViewInfo.AllowUserToAddRows = false;
            this.dataGridViewInfo.AllowUserToDeleteRows = false;
            this.dataGridViewInfo.AllowUserToResizeColumns = false;
            this.dataGridViewInfo.AllowUserToResizeRows = false;
            this.dataGridViewInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInfo.Location = new System.Drawing.Point(205, 401);
            this.dataGridViewInfo.MultiSelect = false;
            this.dataGridViewInfo.Name = "dataGridViewInfo";
            this.dataGridViewInfo.ReadOnly = true;
            this.dataGridViewInfo.RowHeadersVisible = false;
            this.dataGridViewInfo.Size = new System.Drawing.Size(380, 150);
            this.dataGridViewInfo.TabIndex = 45;
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
            this.groupBoxAspects.Location = new System.Drawing.Point(593, 394);
            this.groupBoxAspects.Name = "groupBoxAspects";
            this.groupBoxAspects.Size = new System.Drawing.Size(112, 230);
            this.groupBoxAspects.TabIndex = 46;
            this.groupBoxAspects.TabStop = false;
            this.groupBoxAspects.Text = "Aspects";
            // 
            // checkBoxRahu
            // 
            this.checkBoxRahu.AutoSize = true;
            this.checkBoxRahu.Location = new System.Drawing.Point(7, 193);
            this.checkBoxRahu.Name = "checkBoxRahu";
            this.checkBoxRahu.Size = new System.Drawing.Size(58, 20);
            this.checkBoxRahu.TabIndex = 8;
            this.checkBoxRahu.Text = "Rahu";
            this.checkBoxRahu.UseVisualStyleBackColor = true;
            this.checkBoxRahu.CheckedChanged += new System.EventHandler(this.checkBoxRahu_CheckedChanged);
            // 
            // checkBoxSaturn
            // 
            this.checkBoxSaturn.AutoSize = true;
            this.checkBoxSaturn.Location = new System.Drawing.Point(7, 172);
            this.checkBoxSaturn.Name = "checkBoxSaturn";
            this.checkBoxSaturn.Size = new System.Drawing.Size(64, 20);
            this.checkBoxSaturn.TabIndex = 7;
            this.checkBoxSaturn.Text = "Saturn";
            this.checkBoxSaturn.UseVisualStyleBackColor = true;
            this.checkBoxSaturn.CheckedChanged += new System.EventHandler(this.checkBoxSaturn_CheckedChanged);
            // 
            // checkBoxMars
            // 
            this.checkBoxMars.AutoSize = true;
            this.checkBoxMars.Location = new System.Drawing.Point(7, 151);
            this.checkBoxMars.Name = "checkBoxMars";
            this.checkBoxMars.Size = new System.Drawing.Size(56, 20);
            this.checkBoxMars.TabIndex = 6;
            this.checkBoxMars.Text = "Mars";
            this.checkBoxMars.UseVisualStyleBackColor = true;
            this.checkBoxMars.CheckedChanged += new System.EventHandler(this.checkBoxMars_CheckedChanged);
            // 
            // checkBoxMercury
            // 
            this.checkBoxMercury.AutoSize = true;
            this.checkBoxMercury.Location = new System.Drawing.Point(7, 130);
            this.checkBoxMercury.Name = "checkBoxMercury";
            this.checkBoxMercury.Size = new System.Drawing.Size(74, 20);
            this.checkBoxMercury.TabIndex = 5;
            this.checkBoxMercury.Text = "Mercury";
            this.checkBoxMercury.UseVisualStyleBackColor = true;
            this.checkBoxMercury.CheckedChanged += new System.EventHandler(this.checkBoxMercury_CheckedChanged);
            // 
            // checkBoxJupiter
            // 
            this.checkBoxJupiter.AutoSize = true;
            this.checkBoxJupiter.Location = new System.Drawing.Point(7, 109);
            this.checkBoxJupiter.Name = "checkBoxJupiter";
            this.checkBoxJupiter.Size = new System.Drawing.Size(66, 20);
            this.checkBoxJupiter.TabIndex = 4;
            this.checkBoxJupiter.Text = "Jupiter";
            this.checkBoxJupiter.UseVisualStyleBackColor = true;
            this.checkBoxJupiter.CheckedChanged += new System.EventHandler(this.checkBoxJupiter_CheckedChanged);
            // 
            // checkBoxVenus
            // 
            this.checkBoxVenus.AutoSize = true;
            this.checkBoxVenus.Location = new System.Drawing.Point(7, 88);
            this.checkBoxVenus.Name = "checkBoxVenus";
            this.checkBoxVenus.Size = new System.Drawing.Size(64, 20);
            this.checkBoxVenus.TabIndex = 3;
            this.checkBoxVenus.Text = "Venus";
            this.checkBoxVenus.UseVisualStyleBackColor = true;
            this.checkBoxVenus.CheckedChanged += new System.EventHandler(this.checkBoxVenus_CheckedChanged);
            // 
            // checkBoxSun
            // 
            this.checkBoxSun.AutoSize = true;
            this.checkBoxSun.Location = new System.Drawing.Point(7, 67);
            this.checkBoxSun.Name = "checkBoxSun";
            this.checkBoxSun.Size = new System.Drawing.Size(49, 20);
            this.checkBoxSun.TabIndex = 2;
            this.checkBoxSun.Text = "Sun";
            this.checkBoxSun.UseVisualStyleBackColor = true;
            this.checkBoxSun.CheckedChanged += new System.EventHandler(this.checkBoxSun_CheckedChanged);
            // 
            // checkBoxMoon
            // 
            this.checkBoxMoon.AutoSize = true;
            this.checkBoxMoon.Location = new System.Drawing.Point(7, 46);
            this.checkBoxMoon.Name = "checkBoxMoon";
            this.checkBoxMoon.Size = new System.Drawing.Size(60, 20);
            this.checkBoxMoon.TabIndex = 1;
            this.checkBoxMoon.Text = "Moon";
            this.checkBoxMoon.UseVisualStyleBackColor = true;
            this.checkBoxMoon.CheckedChanged += new System.EventHandler(this.checkBoxMoon_CheckedChanged);
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Location = new System.Drawing.Point(7, 25);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(41, 20);
            this.checkBoxAll.TabIndex = 0;
            this.checkBoxAll.Text = "All";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // labelDateBirth
            // 
            this.labelDateBirth.AutoSize = true;
            this.labelDateBirth.Location = new System.Drawing.Point(715, 154);
            this.labelDateBirth.Name = "labelDateBirth";
            this.labelDateBirth.Size = new System.Drawing.Size(79, 16);
            this.labelDateBirth.TabIndex = 48;
            this.labelDateBirth.Text = "Date of Birth";
            // 
            // maskedTextBoxDate
            // 
            this.maskedTextBoxDate.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.maskedTextBoxDate.Location = new System.Drawing.Point(810, 151);
            this.maskedTextBoxDate.Mask = "00/00/0000 90:00:00";
            this.maskedTextBoxDate.Name = "maskedTextBoxDate";
            this.maskedTextBoxDate.ReadOnly = true;
            this.maskedTextBoxDate.Size = new System.Drawing.Size(200, 22);
            this.maskedTextBoxDate.TabIndex = 26;
            this.maskedTextBoxDate.ValidatingType = typeof(System.DateTime);
            // 
            // buttonBirthPlace
            // 
            this.buttonBirthPlace.Enabled = false;
            this.buttonBirthPlace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBirthPlace.Image = ((System.Drawing.Image)(resources.GetObject("buttonBirthPlace.Image")));
            this.buttonBirthPlace.Location = new System.Drawing.Point(980, 179);
            this.buttonBirthPlace.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBirthPlace.Name = "buttonBirthPlace";
            this.buttonBirthPlace.Size = new System.Drawing.Size(30, 22);
            this.buttonBirthPlace.TabIndex = 28;
            this.buttonBirthPlace.UseVisualStyleBackColor = true;
            this.buttonBirthPlace.Click += new System.EventHandler(this.buttonBirthPlace_Click);
            // 
            // labelPlaceBirth
            // 
            this.labelPlaceBirth.AutoSize = true;
            this.labelPlaceBirth.Location = new System.Drawing.Point(715, 182);
            this.labelPlaceBirth.Name = "labelPlaceBirth";
            this.labelPlaceBirth.Size = new System.Drawing.Size(85, 16);
            this.labelPlaceBirth.TabIndex = 50;
            this.labelPlaceBirth.Text = "Place of Birth";
            // 
            // textBoxBirthPlace
            // 
            this.textBoxBirthPlace.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxBirthPlace.Location = new System.Drawing.Point(811, 179);
            this.textBoxBirthPlace.Name = "textBoxBirthPlace";
            this.textBoxBirthPlace.ReadOnly = true;
            this.textBoxBirthPlace.Size = new System.Drawing.Size(168, 22);
            this.textBoxBirthPlace.TabIndex = 49;
            // 
            // buttonGenerateMap
            // 
            this.buttonGenerateMap.Enabled = false;
            this.buttonGenerateMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGenerateMap.Location = new System.Drawing.Point(718, 249);
            this.buttonGenerateMap.Name = "buttonGenerateMap";
            this.buttonGenerateMap.Size = new System.Drawing.Size(292, 32);
            this.buttonGenerateMap.TabIndex = 32;
            this.buttonGenerateMap.Text = "Generate Chart";
            this.buttonGenerateMap.UseVisualStyleBackColor = true;
            this.buttonGenerateMap.Click += new System.EventHandler(this.buttonGenerateMap_Click);
            // 
            // buttonSaveProfile
            // 
            this.buttonSaveProfile.Enabled = false;
            this.buttonSaveProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveProfile.Location = new System.Drawing.Point(718, 296);
            this.buttonSaveProfile.Name = "buttonSaveProfile";
            this.buttonSaveProfile.Size = new System.Drawing.Size(292, 32);
            this.buttonSaveProfile.TabIndex = 34;
            this.buttonSaveProfile.Text = "Add as new Profile";
            this.buttonSaveProfile.UseVisualStyleBackColor = true;
            this.buttonSaveProfile.Click += new System.EventHandler(this.buttonSaveProfile_Click);
            // 
            // richTextBoxNotes
            // 
            this.richTextBoxNotes.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.richTextBoxNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxNotes.Location = new System.Drawing.Point(718, 344);
            this.richTextBoxNotes.Name = "richTextBoxNotes";
            this.richTextBoxNotes.ReadOnly = true;
            this.richTextBoxNotes.Size = new System.Drawing.Size(292, 229);
            this.richTextBoxNotes.TabIndex = 36;
            this.richTextBoxNotes.Text = "";
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1016, 631);
            this.Controls.Add(this.richTextBoxNotes);
            this.Controls.Add(this.buttonSaveProfile);
            this.Controls.Add(this.buttonGenerateMap);
            this.Controls.Add(this.buttonBirthPlace);
            this.Controls.Add(this.labelPlaceBirth);
            this.Controls.Add(this.textBoxBirthPlace);
            this.Controls.Add(this.labelDateBirth);
            this.Controls.Add(this.maskedTextBoxDate);
            this.Controls.Add(this.groupBoxAspects);
            this.Controls.Add(this.dataGridViewInfo);
            this.Controls.Add(this.pictureBoxMap);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.buttonLivingPlace);
            this.Controls.Add(this.labelLivingPlace);
            this.Controls.Add(this.textBoxLivingPlace);
            this.Controls.Add(this.toolStripProfileMenu);
            this.Controls.Add(this.buttonChoose);
            this.Controls.Add(this.labelPersonSurname);
            this.Controls.Add(this.textBoxPersonSurname);
            this.Controls.Add(this.labelPersonName);
            this.Controls.Add(this.textBoxPersonName);
            this.Controls.Add(this.labelProfileName);
            this.Controls.Add(this.textBoxProfileName);
            this.Controls.Add(this.labelSearch);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.panelProfiles);
            this.Controls.Add(this.buttonClose);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Profiles";
            this.Shown += new System.EventHandler(this.ProfileForm_Shown);
            this.panelProfiles.ResumeLayout(false);
            this.toolStripProfileMenu.ResumeLayout(false);
            this.toolStripProfileMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).EndInit();
            this.groupBoxAspects.ResumeLayout(false);
            this.groupBoxAspects.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panelProfiles;
        private System.Windows.Forms.Label labelPersonSurname;
        private System.Windows.Forms.TextBox textBoxPersonSurname;
        private System.Windows.Forms.Label labelPersonName;
        private System.Windows.Forms.TextBox textBoxPersonName;
        private System.Windows.Forms.Label labelProfileName;
        private System.Windows.Forms.TextBox textBoxProfileName;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.ToolStrip toolStripProfileMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Label labelLivingPlace;
        private System.Windows.Forms.TextBox textBoxLivingPlace;
        private System.Windows.Forms.Button buttonLivingPlace;
        private System.Windows.Forms.ListView listViewProfile;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.DataGridView dataGridViewInfo;
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
        private System.Windows.Forms.Label labelDateBirth;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDate;
        private System.Windows.Forms.Button buttonBirthPlace;
        private System.Windows.Forms.Label labelPlaceBirth;
        private System.Windows.Forms.TextBox textBoxBirthPlace;
        private System.Windows.Forms.Button buttonGenerateMap;
        private System.Windows.Forms.Button buttonSaveProfile;
        private System.Windows.Forms.RichTextBox richTextBoxNotes;
    }
}