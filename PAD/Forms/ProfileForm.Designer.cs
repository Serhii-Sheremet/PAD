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
            this.pictureBoxMapMoon = new System.Windows.Forms.PictureBox();
            this.dataGridViewInfo = new System.Windows.Forms.DataGridView();
            this.panelProfiles.SuspendLayout();
            this.toolStripProfileMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapMoon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(813, 596);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 32);
            this.buttonClose.TabIndex = 16;
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
            this.panelProfiles.Size = new System.Drawing.Size(200, 636);
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
            this.listViewProfile.Size = new System.Drawing.Size(194, 630);
            this.listViewProfile.TabIndex = 0;
            this.listViewProfile.UseCompatibleStateImageBehavior = false;
            this.listViewProfile.View = System.Windows.Forms.View.Details;
            this.listViewProfile.SelectedIndexChanged += new System.EventHandler(this.listViewProfile_SelectedIndexChanged);
            // 
            // labelPersonSurname
            // 
            this.labelPersonSurname.AutoSize = true;
            this.labelPersonSurname.Location = new System.Drawing.Point(208, 529);
            this.labelPersonSurname.Name = "labelPersonSurname";
            this.labelPersonSurname.Size = new System.Drawing.Size(61, 16);
            this.labelPersonSurname.TabIndex = 25;
            this.labelPersonSurname.Text = "Surname";
            // 
            // textBoxPersonSurname
            // 
            this.textBoxPersonSurname.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxPersonSurname.Location = new System.Drawing.Point(303, 525);
            this.textBoxPersonSurname.Name = "textBoxPersonSurname";
            this.textBoxPersonSurname.ReadOnly = true;
            this.textBoxPersonSurname.Size = new System.Drawing.Size(200, 22);
            this.textBoxPersonSurname.TabIndex = 24;
            // 
            // labelPersonName
            // 
            this.labelPersonName.AutoSize = true;
            this.labelPersonName.Location = new System.Drawing.Point(208, 501);
            this.labelPersonName.Name = "labelPersonName";
            this.labelPersonName.Size = new System.Drawing.Size(44, 16);
            this.labelPersonName.TabIndex = 23;
            this.labelPersonName.Text = "Name";
            // 
            // textBoxPersonName
            // 
            this.textBoxPersonName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxPersonName.Location = new System.Drawing.Point(303, 498);
            this.textBoxPersonName.Name = "textBoxPersonName";
            this.textBoxPersonName.ReadOnly = true;
            this.textBoxPersonName.Size = new System.Drawing.Size(200, 22);
            this.textBoxPersonName.TabIndex = 22;
            // 
            // labelProfileName
            // 
            this.labelProfileName.AutoSize = true;
            this.labelProfileName.Location = new System.Drawing.Point(208, 474);
            this.labelProfileName.Name = "labelProfileName";
            this.labelProfileName.Size = new System.Drawing.Size(82, 16);
            this.labelProfileName.TabIndex = 21;
            this.labelProfileName.Text = "Profile name";
            // 
            // textBoxProfileName
            // 
            this.textBoxProfileName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxProfileName.Location = new System.Drawing.Point(303, 471);
            this.textBoxProfileName.Name = "textBoxProfileName";
            this.textBoxProfileName.ReadOnly = true;
            this.textBoxProfileName.Size = new System.Drawing.Size(200, 22);
            this.textBoxProfileName.TabIndex = 20;
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(208, 448);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(53, 16);
            this.labelSearch.TabIndex = 19;
            this.labelSearch.Text = "Search:";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(303, 445);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(200, 22);
            this.textBoxSearch.TabIndex = 18;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // buttonChoose
            // 
            this.buttonChoose.Enabled = false;
            this.buttonChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChoose.Location = new System.Drawing.Point(718, 596);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(80, 32);
            this.buttonChoose.TabIndex = 32;
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
            this.toolStripProfileMenu.Size = new System.Drawing.Size(734, 39);
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
            this.labelLivingPlace.Location = new System.Drawing.Point(208, 555);
            this.labelLivingPlace.Name = "labelLivingPlace";
            this.labelLivingPlace.Size = new System.Drawing.Size(90, 16);
            this.labelLivingPlace.TabIndex = 37;
            this.labelLivingPlace.Text = "Place of living";
            // 
            // textBoxLivingPlace
            // 
            this.textBoxLivingPlace.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLivingPlace.Location = new System.Drawing.Point(303, 552);
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
            this.buttonLivingPlace.Location = new System.Drawing.Point(472, 552);
            this.buttonLivingPlace.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLivingPlace.Name = "buttonLivingPlace";
            this.buttonLivingPlace.Size = new System.Drawing.Size(30, 22);
            this.buttonLivingPlace.TabIndex = 38;
            this.buttonLivingPlace.UseVisualStyleBackColor = true;
            this.buttonLivingPlace.Click += new System.EventHandler(this.buttonLivingPlace_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Enabled = false;
            this.buttonDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDefault.Location = new System.Drawing.Point(270, 596);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(200, 32);
            this.buttonDefault.TabIndex = 42;
            this.buttonDefault.Text = "Make default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // pictureBoxMapMoon
            // 
            this.pictureBoxMapMoon.Location = new System.Drawing.Point(200, 39);
            this.pictureBoxMapMoon.Name = "pictureBoxMapMoon";
            this.pictureBoxMapMoon.Size = new System.Drawing.Size(400, 400);
            this.pictureBoxMapMoon.TabIndex = 44;
            this.pictureBoxMapMoon.TabStop = false;
            // 
            // dataGridViewInfo
            // 
            this.dataGridViewInfo.AllowUserToAddRows = false;
            this.dataGridViewInfo.AllowUserToDeleteRows = false;
            this.dataGridViewInfo.AllowUserToResizeColumns = false;
            this.dataGridViewInfo.AllowUserToResizeRows = false;
            this.dataGridViewInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInfo.Location = new System.Drawing.Point(609, 39);
            this.dataGridViewInfo.MultiSelect = false;
            this.dataGridViewInfo.Name = "dataGridViewInfo";
            this.dataGridViewInfo.ReadOnly = true;
            this.dataGridViewInfo.RowHeadersVisible = false;
            this.dataGridViewInfo.Size = new System.Drawing.Size(320, 150);
            this.dataGridViewInfo.TabIndex = 45;
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(934, 636);
            this.Controls.Add(this.dataGridViewInfo);
            this.Controls.Add(this.pictureBoxMapMoon);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapMoon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBoxMapMoon;
        private System.Windows.Forms.DataGridView dataGridViewInfo;
    }
}