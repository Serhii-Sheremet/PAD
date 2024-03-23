namespace PAD
{
    partial class PersonTransitEvents
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonTransitEvents));
            this.panelEvents = new System.Windows.Forms.Panel();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.toolStripEventMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.buttonChoose = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonLocation = new System.Windows.Forms.Button();
            this.labelLocation = new System.Windows.Forms.Label();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.labelTransitName = new System.Windows.Forms.Label();
            this.textBoxTransitName = new System.Windows.Forms.TextBox();
            this.labelProfileName = new System.Windows.Forms.Label();
            this.textBoxProfileName = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.maskedTextBoxDate = new System.Windows.Forms.MaskedTextBox();
            this.groupBoxDescription = new System.Windows.Forms.GroupBox();
            this.richTextBoxDesc = new System.Windows.Forms.RichTextBox();
            this.panelEvents.SuspendLayout();
            this.toolStripEventMenu.SuspendLayout();
            this.groupBoxDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEvents
            // 
            this.panelEvents.Controls.Add(this.listViewEvents);
            this.panelEvents.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEvents.Location = new System.Drawing.Point(0, 0);
            this.panelEvents.Margin = new System.Windows.Forms.Padding(4);
            this.panelEvents.Name = "panelEvents";
            this.panelEvents.Padding = new System.Windows.Forms.Padding(4);
            this.panelEvents.Size = new System.Drawing.Size(300, 411);
            this.panelEvents.TabIndex = 18;
            // 
            // listViewEvents
            // 
            this.listViewEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewEvents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewEvents.HideSelection = false;
            this.listViewEvents.Location = new System.Drawing.Point(4, 4);
            this.listViewEvents.Margin = new System.Windows.Forms.Padding(4);
            this.listViewEvents.MultiSelect = false;
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(292, 403);
            this.listViewEvents.TabIndex = 0;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            this.listViewEvents.SelectedIndexChanged += new System.EventHandler(this.listViewEvents_SelectedIndexChanged);
            // 
            // toolStripEventMenu
            // 
            this.toolStripEventMenu.AutoSize = false;
            this.toolStripEventMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripEventMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripSeparator2,
            this.toolStripButtonSave,
            this.toolStripLabel2});
            this.toolStripEventMenu.Location = new System.Drawing.Point(300, 0);
            this.toolStripEventMenu.Name = "toolStripEventMenu";
            this.toolStripEventMenu.Size = new System.Drawing.Size(364, 48);
            this.toolStripEventMenu.TabIndex = 65;
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Enabled = false;
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(36, 45);
            this.toolStripButtonAdd.Text = "Add Event";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Enabled = false;
            this.toolStripButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEdit.Image")));
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(36, 45);
            this.toolStripButtonEdit.Text = "Edit Event";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Enabled = false;
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(36, 45);
            this.toolStripButtonDelete.Text = "Delete Event";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 48);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Enabled = false;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(36, 45);
            this.toolStripButtonSave.Text = "Save Event";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(10, 36);
            // 
            // buttonChoose
            // 
            this.buttonChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChoose.Location = new System.Drawing.Point(454, 367);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(80, 32);
            this.buttonChoose.TabIndex = 32;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(568, 367);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 32);
            this.buttonClose.TabIndex = 34;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonLocation
            // 
            this.buttonLocation.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.buttonLocation.Enabled = false;
            this.buttonLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLocation.Image = ((System.Drawing.Image)(resources.GetObject("buttonLocation.Image")));
            this.buttonLocation.Location = new System.Drawing.Point(617, 178);
            this.buttonLocation.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLocation.Name = "buttonLocation";
            this.buttonLocation.Size = new System.Drawing.Size(30, 22);
            this.buttonLocation.TabIndex = 28;
            this.buttonLocation.UseVisualStyleBackColor = false;
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(312, 180);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(58, 16);
            this.labelLocation.TabIndex = 76;
            this.labelLocation.Text = "Location";
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLocation.Location = new System.Drawing.Point(448, 178);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.ReadOnly = true;
            this.textBoxLocation.Size = new System.Drawing.Size(168, 22);
            this.textBoxLocation.TabIndex = 27;
            // 
            // labelTransitName
            // 
            this.labelTransitName.AutoSize = true;
            this.labelTransitName.Location = new System.Drawing.Point(312, 84);
            this.labelTransitName.Name = "labelTransitName";
            this.labelTransitName.Size = new System.Drawing.Size(88, 16);
            this.labelTransitName.TabIndex = 73;
            this.labelTransitName.Text = "Transit Name";
            // 
            // textBoxTransitName
            // 
            this.textBoxTransitName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxTransitName.Location = new System.Drawing.Point(448, 81);
            this.textBoxTransitName.Name = "textBoxTransitName";
            this.textBoxTransitName.ReadOnly = true;
            this.textBoxTransitName.Size = new System.Drawing.Size(200, 22);
            this.textBoxTransitName.TabIndex = 22;
            // 
            // labelProfileName
            // 
            this.labelProfileName.AutoSize = true;
            this.labelProfileName.Location = new System.Drawing.Point(312, 115);
            this.labelProfileName.Name = "labelProfileName";
            this.labelProfileName.Size = new System.Drawing.Size(85, 16);
            this.labelProfileName.TabIndex = 71;
            this.labelProfileName.Text = "Profile Name";
            // 
            // textBoxProfileName
            // 
            this.textBoxProfileName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxProfileName.Location = new System.Drawing.Point(448, 112);
            this.textBoxProfileName.Name = "textBoxProfileName";
            this.textBoxProfileName.ReadOnly = true;
            this.textBoxProfileName.Size = new System.Drawing.Size(200, 22);
            this.textBoxProfileName.TabIndex = 24;
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(312, 54);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(53, 16);
            this.labelSearch.TabIndex = 69;
            this.labelSearch.Text = "Search:";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(448, 51);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(200, 22);
            this.textBoxSearch.TabIndex = 20;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(312, 150);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(80, 16);
            this.labelDate.TabIndex = 78;
            this.labelDate.Text = "Transit Date";
            // 
            // maskedTextBoxDate
            // 
            this.maskedTextBoxDate.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.maskedTextBoxDate.Location = new System.Drawing.Point(448, 144);
            this.maskedTextBoxDate.Mask = "00/00/0000 90:00:00";
            this.maskedTextBoxDate.Name = "maskedTextBoxDate";
            this.maskedTextBoxDate.ReadOnly = true;
            this.maskedTextBoxDate.Size = new System.Drawing.Size(200, 22);
            this.maskedTextBoxDate.TabIndex = 26;
            this.maskedTextBoxDate.ValidatingType = typeof(System.DateTime);
            this.maskedTextBoxDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedTextBoxDate_KeyPress);
            // 
            // groupBoxDescription
            // 
            this.groupBoxDescription.Controls.Add(this.richTextBoxDesc);
            this.groupBoxDescription.Location = new System.Drawing.Point(315, 206);
            this.groupBoxDescription.Name = "groupBoxDescription";
            this.groupBoxDescription.Padding = new System.Windows.Forms.Padding(5);
            this.groupBoxDescription.Size = new System.Drawing.Size(333, 152);
            this.groupBoxDescription.TabIndex = 79;
            this.groupBoxDescription.TabStop = false;
            this.groupBoxDescription.Text = "Description";
            // 
            // richTextBoxDesc
            // 
            this.richTextBoxDesc.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.richTextBoxDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDesc.Location = new System.Drawing.Point(5, 20);
            this.richTextBoxDesc.Name = "richTextBoxDesc";
            this.richTextBoxDesc.ReadOnly = true;
            this.richTextBoxDesc.Size = new System.Drawing.Size(323, 127);
            this.richTextBoxDesc.TabIndex = 30;
            this.richTextBoxDesc.Text = "";
            // 
            // PersonTransitEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 411);
            this.Controls.Add(this.groupBoxDescription);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.maskedTextBoxDate);
            this.Controls.Add(this.buttonLocation);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.textBoxLocation);
            this.Controls.Add(this.labelTransitName);
            this.Controls.Add(this.textBoxTransitName);
            this.Controls.Add(this.labelProfileName);
            this.Controls.Add(this.textBoxProfileName);
            this.Controls.Add(this.labelSearch);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.buttonChoose);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.toolStripEventMenu);
            this.Controls.Add(this.panelEvents);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersonTransitEvents";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transit Events";
            this.Shown += new System.EventHandler(this.PersonTransitEvents_Shown);
            this.panelEvents.ResumeLayout(false);
            this.toolStripEventMenu.ResumeLayout(false);
            this.toolStripEventMenu.PerformLayout();
            this.groupBoxDescription.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelEvents;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.ToolStrip toolStripEventMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonLocation;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.TextBox textBoxLocation;
        private System.Windows.Forms.Label labelTransitName;
        private System.Windows.Forms.TextBox textBoxTransitName;
        private System.Windows.Forms.Label labelProfileName;
        private System.Windows.Forms.TextBox textBoxProfileName;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDate;
        private System.Windows.Forms.GroupBox groupBoxDescription;
        private System.Windows.Forms.RichTextBox richTextBoxDesc;
    }
}