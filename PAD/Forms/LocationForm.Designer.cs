namespace PAD
{
    partial class LocationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationForm));
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.labelLocality = new System.Windows.Forms.Label();
            this.textBoxLocality = new System.Windows.Forms.TextBox();
            this.labelRegion = new System.Windows.Forms.Label();
            this.textBoxRegion = new System.Windows.Forms.TextBox();
            this.labelState = new System.Windows.Forms.Label();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.labelCountry = new System.Windows.Forms.Label();
            this.textBoxCountry = new System.Windows.Forms.TextBox();
            this.labelLatitude = new System.Windows.Forms.Label();
            this.textBoxLatitude = new System.Windows.Forms.TextBox();
            this.labelLongitude = new System.Windows.Forms.Label();
            this.textBoxLongitude = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelLocation = new System.Windows.Forms.Panel();
            this.listBoxLocality = new System.Windows.Forms.ListBox();
            this.toolStripLocationMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.buttonChoose = new System.Windows.Forms.Button();
            this.linkLabelGeo = new System.Windows.Forms.LinkLabel();
            this.panelLocation.SuspendLayout();
            this.toolStripLocationMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(502, 59);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(250, 22);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(308, 62);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(54, 16);
            this.labelSearch.TabIndex = 2;
            this.labelSearch.Text = "Search:";
            // 
            // labelLocality
            // 
            this.labelLocality.AutoSize = true;
            this.labelLocality.Location = new System.Drawing.Point(308, 100);
            this.labelLocality.Name = "labelLocality";
            this.labelLocality.Size = new System.Drawing.Size(54, 16);
            this.labelLocality.TabIndex = 4;
            this.labelLocality.Text = "Locality";
            // 
            // textBoxLocality
            // 
            this.textBoxLocality.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLocality.Location = new System.Drawing.Point(502, 97);
            this.textBoxLocality.Name = "textBoxLocality";
            this.textBoxLocality.ReadOnly = true;
            this.textBoxLocality.Size = new System.Drawing.Size(250, 22);
            this.textBoxLocality.TabIndex = 3;
            // 
            // labelRegion
            // 
            this.labelRegion.AutoSize = true;
            this.labelRegion.Location = new System.Drawing.Point(308, 139);
            this.labelRegion.Name = "labelRegion";
            this.labelRegion.Size = new System.Drawing.Size(102, 16);
            this.labelRegion.TabIndex = 6;
            this.labelRegion.Text = "Region / District";
            // 
            // textBoxRegion
            // 
            this.textBoxRegion.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxRegion.Location = new System.Drawing.Point(502, 136);
            this.textBoxRegion.Name = "textBoxRegion";
            this.textBoxRegion.ReadOnly = true;
            this.textBoxRegion.Size = new System.Drawing.Size(250, 22);
            this.textBoxRegion.TabIndex = 5;
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(308, 180);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(39, 16);
            this.labelState.TabIndex = 8;
            this.labelState.Text = "State";
            // 
            // textBoxState
            // 
            this.textBoxState.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxState.Location = new System.Drawing.Point(502, 177);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.ReadOnly = true;
            this.textBoxState.Size = new System.Drawing.Size(250, 22);
            this.textBoxState.TabIndex = 7;
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(308, 219);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(53, 16);
            this.labelCountry.TabIndex = 10;
            this.labelCountry.Text = "Country";
            // 
            // textBoxCountry
            // 
            this.textBoxCountry.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxCountry.Location = new System.Drawing.Point(502, 216);
            this.textBoxCountry.Name = "textBoxCountry";
            this.textBoxCountry.ReadOnly = true;
            this.textBoxCountry.Size = new System.Drawing.Size(250, 22);
            this.textBoxCountry.TabIndex = 9;
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.Location = new System.Drawing.Point(308, 260);
            this.labelLatitude.Name = "labelLatitude";
            this.labelLatitude.Size = new System.Drawing.Size(55, 16);
            this.labelLatitude.TabIndex = 12;
            this.labelLatitude.Text = "Latitude";
            // 
            // textBoxLatitude
            // 
            this.textBoxLatitude.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLatitude.Location = new System.Drawing.Point(502, 257);
            this.textBoxLatitude.Name = "textBoxLatitude";
            this.textBoxLatitude.ReadOnly = true;
            this.textBoxLatitude.Size = new System.Drawing.Size(250, 22);
            this.textBoxLatitude.TabIndex = 11;
            // 
            // labelLongitude
            // 
            this.labelLongitude.AutoSize = true;
            this.labelLongitude.Location = new System.Drawing.Point(308, 300);
            this.labelLongitude.Name = "labelLongitude";
            this.labelLongitude.Size = new System.Drawing.Size(67, 16);
            this.labelLongitude.TabIndex = 14;
            this.labelLongitude.Text = "Longitude";
            // 
            // textBoxLongitude
            // 
            this.textBoxLongitude.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLongitude.Location = new System.Drawing.Point(502, 297);
            this.textBoxLongitude.Name = "textBoxLongitude";
            this.textBoxLongitude.ReadOnly = true;
            this.textBoxLongitude.Size = new System.Drawing.Size(250, 22);
            this.textBoxLongitude.TabIndex = 13;
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(672, 350);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 32);
            this.buttonClose.TabIndex = 15;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panelLocation
            // 
            this.panelLocation.Controls.Add(this.listBoxLocality);
            this.panelLocation.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLocation.Location = new System.Drawing.Point(0, 0);
            this.panelLocation.Name = "panelLocation";
            this.panelLocation.Size = new System.Drawing.Size(270, 393);
            this.panelLocation.TabIndex = 16;
            // 
            // listBoxLocality
            // 
            this.listBoxLocality.FormattingEnabled = true;
            this.listBoxLocality.ItemHeight = 16;
            this.listBoxLocality.Location = new System.Drawing.Point(10, 10);
            this.listBoxLocality.Name = "listBoxLocality";
            this.listBoxLocality.Size = new System.Drawing.Size(250, 372);
            this.listBoxLocality.TabIndex = 1;
            this.listBoxLocality.SelectedIndexChanged += new System.EventHandler(this.listBoxLocality_SelectedIndexChanged);
            // 
            // toolStripLocationMenu
            // 
            this.toolStripLocationMenu.AutoSize = false;
            this.toolStripLocationMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripLocationMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripSeparator1,
            this.toolStripButtonSave});
            this.toolStripLocationMenu.Location = new System.Drawing.Point(270, 0);
            this.toolStripLocationMenu.Name = "toolStripLocationMenu";
            this.toolStripLocationMenu.Size = new System.Drawing.Size(514, 39);
            this.toolStripLocationMenu.TabIndex = 17;
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Enabled = false;
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonAdd.Text = "Add location";
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
            this.toolStripButtonEdit.Text = "Edit location";
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
            this.toolStripButtonDelete.Text = "Delete location";
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
            // buttonChoose
            // 
            this.buttonChoose.Enabled = false;
            this.buttonChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChoose.Location = new System.Drawing.Point(580, 350);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(80, 32);
            this.buttonChoose.TabIndex = 18;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // linkLabelGeo
            // 
            this.linkLabelGeo.AutoSize = true;
            this.linkLabelGeo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabelGeo.Location = new System.Drawing.Point(308, 356);
            this.linkLabelGeo.Name = "linkLabelGeo";
            this.linkLabelGeo.Size = new System.Drawing.Size(155, 18);
            this.linkLabelGeo.TabIndex = 19;
            this.linkLabelGeo.TabStop = true;
            this.linkLabelGeo.Text = "Find Geo-Coordinates";
            this.linkLabelGeo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGeo_LinkClicked);
            // 
            // LocationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(784, 393);
            this.Controls.Add(this.linkLabelGeo);
            this.Controls.Add(this.buttonChoose);
            this.Controls.Add(this.toolStripLocationMenu);
            this.Controls.Add(this.panelLocation);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelLongitude);
            this.Controls.Add(this.textBoxLongitude);
            this.Controls.Add(this.labelLatitude);
            this.Controls.Add(this.textBoxLatitude);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.textBoxCountry);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.textBoxState);
            this.Controls.Add(this.labelRegion);
            this.Controls.Add(this.textBoxRegion);
            this.Controls.Add(this.labelLocality);
            this.Controls.Add(this.textBoxLocality);
            this.Controls.Add(this.labelSearch);
            this.Controls.Add(this.textBoxSearch);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Locations";
            this.Shown += new System.EventHandler(this.Location_Shown);
            this.panelLocation.ResumeLayout(false);
            this.toolStripLocationMenu.ResumeLayout(false);
            this.toolStripLocationMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.Label labelLocality;
        private System.Windows.Forms.TextBox textBoxLocality;
        private System.Windows.Forms.Label labelRegion;
        private System.Windows.Forms.TextBox textBoxRegion;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.TextBox textBoxState;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.TextBox textBoxCountry;
        private System.Windows.Forms.Label labelLatitude;
        private System.Windows.Forms.TextBox textBoxLatitude;
        private System.Windows.Forms.Label labelLongitude;
        private System.Windows.Forms.TextBox textBoxLongitude;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panelLocation;
        private System.Windows.Forms.ListBox listBoxLocality;
        private System.Windows.Forms.ToolStrip toolStripLocationMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.LinkLabel linkLabelGeo;
    }
}