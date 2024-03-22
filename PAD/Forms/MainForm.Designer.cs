namespace PAD
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelEmpty = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.tabControlCalendar = new System.Windows.Forms.TabControl();
            this.tabPageCalendar = new System.Windows.Forms.TabPage();
            this.pictureBoxCalendar = new System.Windows.Forms.PictureBox();
            this.tabPageTranzits = new System.Windows.Forms.TabPage();
            this.pictureBoxTranzits = new System.Windows.Forms.PictureBox();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportYearToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.colorSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.profileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tranzitsMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yearsTranzitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swephCalcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutPersonalDiaryCalendarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.datePicker = new CustomControls.DatePicker();
            this.labelProfile = new System.Windows.Forms.Label();
            this.labelTimeZone = new System.Windows.Forms.Label();
            this.toolStripMain.SuspendLayout();
            this.tabControlCalendar.SuspendLayout();
            this.tabPageCalendar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCalendar)).BeginInit();
            this.tabPageTranzits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTranzits)).BeginInit();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            resources.ApplyResources(this.toolStripMain, "toolStripMain");
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelEmpty,
            this.toolStripSeparator5,
            this.toolStripButtonExit,
            this.toolStripSeparator6,
            this.toolStripButtonRefresh});
            this.toolStripMain.Name = "toolStripMain";
            // 
            // toolStripLabelEmpty
            // 
            resources.ApplyResources(this.toolStripLabelEmpty, "toolStripLabelEmpty");
            this.toolStripLabelEmpty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripLabelEmpty.Name = "toolStripLabelEmpty";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // toolStripButtonExit
            // 
            this.toolStripButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonExit, "toolStripButtonExit");
            this.toolStripButtonExit.Name = "toolStripButtonExit";
            this.toolStripButtonExit.Click += new System.EventHandler(this.toolStripButtonExit_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonRefresh, "toolStripButtonRefresh");
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // tabControlCalendar
            // 
            this.tabControlCalendar.Controls.Add(this.tabPageCalendar);
            this.tabControlCalendar.Controls.Add(this.tabPageTranzits);
            resources.ApplyResources(this.tabControlCalendar, "tabControlCalendar");
            this.tabControlCalendar.Name = "tabControlCalendar";
            this.tabControlCalendar.SelectedIndex = 0;
            // 
            // tabPageCalendar
            // 
            this.tabPageCalendar.Controls.Add(this.pictureBoxCalendar);
            resources.ApplyResources(this.tabPageCalendar, "tabPageCalendar");
            this.tabPageCalendar.Name = "tabPageCalendar";
            this.tabPageCalendar.Tag = "Calendar";
            this.tabPageCalendar.UseVisualStyleBackColor = true;
            // 
            // pictureBoxCalendar
            // 
            resources.ApplyResources(this.pictureBoxCalendar, "pictureBoxCalendar");
            this.pictureBoxCalendar.Name = "pictureBoxCalendar";
            this.pictureBoxCalendar.TabStop = false;
            this.pictureBoxCalendar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCalendar_MouseClick);
            this.pictureBoxCalendar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCalendar_MouseDoubleClick);
            this.pictureBoxCalendar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCalendar_MouseMove);
            // 
            // tabPageTranzits
            // 
            this.tabPageTranzits.Controls.Add(this.pictureBoxTranzits);
            resources.ApplyResources(this.tabPageTranzits, "tabPageTranzits");
            this.tabPageTranzits.Name = "tabPageTranzits";
            this.tabPageTranzits.Tag = "Tranzits";
            this.tabPageTranzits.UseVisualStyleBackColor = true;
            // 
            // pictureBoxTranzits
            // 
            resources.ApplyResources(this.pictureBoxTranzits, "pictureBoxTranzits");
            this.pictureBoxTranzits.Name = "pictureBoxTranzits";
            this.pictureBoxTranzits.TabStop = false;
            this.pictureBoxTranzits.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTranzits_MouseClick);
            this.pictureBoxTranzits.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTranzits_MouseDoubleClick);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToPDFToolStripMenuItem,
            this.exportYearToPDFToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // exportToPDFToolStripMenuItem
            // 
            resources.ApplyResources(this.exportToPDFToolStripMenuItem, "exportToPDFToolStripMenuItem");
            this.exportToPDFToolStripMenuItem.Name = "exportToPDFToolStripMenuItem";
            this.exportToPDFToolStripMenuItem.Click += new System.EventHandler(this.exportToPDFToolStripMenuItem_Click);
            // 
            // exportYearToPDFToolStripMenuItem
            // 
            resources.ApplyResources(this.exportYearToPDFToolStripMenuItem, "exportYearToPDFToolStripMenuItem");
            this.exportYearToPDFToolStripMenuItem.Name = "exportYearToPDFToolStripMenuItem";
            this.exportYearToPDFToolStripMenuItem.Click += new System.EventHandler(this.exportYearToPDFToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationSettingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.colorSettingsToolStripMenuItem,
            this.fontSettingsToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            resources.ApplyResources(this.configurationToolStripMenuItem, "configurationToolStripMenuItem");
            // 
            // applicationSettingsToolStripMenuItem
            // 
            resources.ApplyResources(this.applicationSettingsToolStripMenuItem, "applicationSettingsToolStripMenuItem");
            this.applicationSettingsToolStripMenuItem.Name = "applicationSettingsToolStripMenuItem";
            this.applicationSettingsToolStripMenuItem.Click += new System.EventHandler(this.applicationSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // colorSettingsToolStripMenuItem
            // 
            resources.ApplyResources(this.colorSettingsToolStripMenuItem, "colorSettingsToolStripMenuItem");
            this.colorSettingsToolStripMenuItem.Name = "colorSettingsToolStripMenuItem";
            this.colorSettingsToolStripMenuItem.Click += new System.EventHandler(this.colorSettingsToolStripMenuItem_Click);
            // 
            // fontSettingsToolStripMenuItem
            // 
            resources.ApplyResources(this.fontSettingsToolStripMenuItem, "fontSettingsToolStripMenuItem");
            this.fontSettingsToolStripMenuItem.Name = "fontSettingsToolStripMenuItem";
            this.fontSettingsToolStripMenuItem.Click += new System.EventHandler(this.fontSettingsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profileToolStripMenuItem,
            this.locationToolStripMenuItem,
            this.toolStripSeparator1,
            this.tranzitsMapToolStripMenuItem,
            this.yearsTranzitsToolStripMenuItem,
            this.swephCalcToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // profileToolStripMenuItem
            // 
            resources.ApplyResources(this.profileToolStripMenuItem, "profileToolStripMenuItem");
            this.profileToolStripMenuItem.Name = "profileToolStripMenuItem";
            this.profileToolStripMenuItem.Click += new System.EventHandler(this.profileToolStripMenuItem_Click);
            // 
            // locationToolStripMenuItem
            // 
            resources.ApplyResources(this.locationToolStripMenuItem, "locationToolStripMenuItem");
            this.locationToolStripMenuItem.Name = "locationToolStripMenuItem";
            this.locationToolStripMenuItem.Click += new System.EventHandler(this.locationToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tranzitsMapToolStripMenuItem
            // 
            this.tranzitsMapToolStripMenuItem.Name = "tranzitsMapToolStripMenuItem";
            resources.ApplyResources(this.tranzitsMapToolStripMenuItem, "tranzitsMapToolStripMenuItem");
            this.tranzitsMapToolStripMenuItem.Click += new System.EventHandler(this.tranzitsMapToolStripMenuItem_Click);
            // 
            // yearsTranzitsToolStripMenuItem
            // 
            this.yearsTranzitsToolStripMenuItem.Name = "yearsTranzitsToolStripMenuItem";
            resources.ApplyResources(this.yearsTranzitsToolStripMenuItem, "yearsTranzitsToolStripMenuItem");
            this.yearsTranzitsToolStripMenuItem.Click += new System.EventHandler(this.yearsTranzitsToolStripMenuItem_Click);
            // 
            // swephCalcToolStripMenuItem
            // 
            this.swephCalcToolStripMenuItem.Name = "swephCalcToolStripMenuItem";
            resources.ApplyResources(this.swephCalcToolStripMenuItem, "swephCalcToolStripMenuItem");
            this.swephCalcToolStripMenuItem.Click += new System.EventHandler(this.swephCalcToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripSeparator3,
            this.aboutPersonalDiaryCalendarToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // viewHelpToolStripMenuItem
            // 
            resources.ApplyResources(this.viewHelpToolStripMenuItem, "viewHelpToolStripMenuItem");
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelpToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            resources.ApplyResources(this.checkForUpdatesToolStripMenuItem, "checkForUpdatesToolStripMenuItem");
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // aboutPersonalDiaryCalendarToolStripMenuItem
            // 
            resources.ApplyResources(this.aboutPersonalDiaryCalendarToolStripMenuItem, "aboutPersonalDiaryCalendarToolStripMenuItem");
            this.aboutPersonalDiaryCalendarToolStripMenuItem.Name = "aboutPersonalDiaryCalendarToolStripMenuItem";
            this.aboutPersonalDiaryCalendarToolStripMenuItem.Click += new System.EventHandler(this.aboutPersonalDiaryCalendarToolStripMenuItem_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStripMain, "menuStripMain");
            this.menuStripMain.Name = "menuStripMain";
            // 
            // datePicker
            // 
            resources.ApplyResources(this.datePicker, "datePicker");
            this.datePicker.FormatProvider.ShortDatePattern = "dd/MMMM/yyyy";
            this.datePicker.Name = "datePicker";
            this.datePicker.UseNativeDigits = true;
            this.datePicker.ValueChanged += new System.EventHandler<CustomControls.CheckDateEventArgs>(this.datePicker_ValueChanged);
            // 
            // labelProfile
            // 
            resources.ApplyResources(this.labelProfile, "labelProfile");
            this.labelProfile.Name = "labelProfile";
            // 
            // labelTimeZone
            // 
            resources.ApplyResources(this.labelTimeZone, "labelTimeZone");
            this.labelTimeZone.Name = "labelTimeZone";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelTimeZone);
            this.Controls.Add(this.labelProfile);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.tabControlCalendar);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.tabControlCalendar.ResumeLayout(false);
            this.tabPageCalendar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCalendar)).EndInit();
            this.tabPageTranzits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTranzits)).EndInit();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonExit;
        private System.Windows.Forms.TabControl tabControlCalendar;
        private System.Windows.Forms.TabPage tabPageCalendar;
        private System.Windows.Forms.TabPage tabPageTranzits;
        private System.Windows.Forms.PictureBox pictureBoxCalendar;
        private System.Windows.Forms.PictureBox pictureBoxTranzits;
        private System.Windows.Forms.ToolStripLabel toolStripLabelEmpty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem colorSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem profileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem aboutPersonalDiaryCalendarToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private CustomControls.DatePicker datePicker;
        private System.Windows.Forms.Label labelProfile;
        private System.Windows.Forms.Label labelTimeZone;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem yearsTranzitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportYearToPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swephCalcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tranzitsMapToolStripMenuItem;
    }
}