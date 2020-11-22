namespace PAD
{
    partial class TabDay
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
        /// 



        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            PAD.DrawTool drawTool1 = new PAD.DrawTool();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabDay));
            this.dayView = new PAD.DayView();
            this.contextMenuStripEvents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addStartLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addingStartLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removingStartLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEndLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addingEndLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removingEndLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDefaultEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createConfiguredEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // dayView
            // 
            drawTool1.DayView = this.dayView;
            this.dayView.ActiveTool = drawTool1;
            this.dayView.AmPmDisplay = false;
            this.dayView.AppHeightMode = PAD.DayView.AppHeightDrawMode.TrueHeightAll;
            this.dayView.ArrayOfNames = new string[] {
        "MUHURTA"};
            this.dayView.CultureCode = "";
            this.dayView.DaysToShow = 2;
            this.dayView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dayView.DrawAllAppBorder = false;
            this.dayView.EndLineDate = new System.DateTime(((long)(0)));
            this.dayView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dayView.Location = new System.Drawing.Point(0, 0);
            this.dayView.MinHalfHourApp = false;
            this.dayView.Name = "dayView";
            this.dayView.SelectionEnd = new System.DateTime(((long)(0)));
            this.dayView.SelectionStart = new System.DateTime(((long)(0)));
            this.dayView.Size = new System.Drawing.Size(800, 450);
            this.dayView.StartDate = new System.DateTime(((long)(0)));
            this.dayView.StartLineDate = new System.DateTime(((long)(0)));
            this.dayView.SunRiseDate = new System.DateTime(((long)(0)));
            this.dayView.SunSetDate = new System.DateTime(((long)(0)));
            this.dayView.TabIndex = 1;
            this.dayView.DoubleClick += new System.EventHandler(this.dayView_DoubleClick);
            this.dayView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dayView_MouseClick);
            // 
            // contextMenuStripEvents
            // 
            this.contextMenuStripEvents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.toolStripSeparator1,
            this.addEventToolStripMenuItem,
            this.editEventToolStripMenuItem,
            this.deleteEventToolStripMenuItem,
            this.toolStripSeparator2,
            this.addStartLineToolStripMenuItem,
            this.addEndLineToolStripMenuItem,
            this.createEventToolStripMenuItem});
            this.contextMenuStripEvents.Name = "contextMenuStrip1";
            this.contextMenuStripEvents.Size = new System.Drawing.Size(297, 170);
            this.contextMenuStripEvents.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripEvents_Opening);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("infoToolStripMenuItem.Image")));
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.infoToolStripMenuItem.Text = "Information";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(293, 6);
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addEventToolStripMenuItem.Image")));
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            this.addEventToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.addEventToolStripMenuItem.Text = "Add appointment";
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.addEventToolStripMenuItem_Click);
            // 
            // editEventToolStripMenuItem
            // 
            this.editEventToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editEventToolStripMenuItem.Image")));
            this.editEventToolStripMenuItem.Name = "editEventToolStripMenuItem";
            this.editEventToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.editEventToolStripMenuItem.Text = "Edit appointment";
            this.editEventToolStripMenuItem.Click += new System.EventHandler(this.editEventToolStripMenuItem_Click);
            // 
            // deleteEventToolStripMenuItem
            // 
            this.deleteEventToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteEventToolStripMenuItem.Image")));
            this.deleteEventToolStripMenuItem.Name = "deleteEventToolStripMenuItem";
            this.deleteEventToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.deleteEventToolStripMenuItem.Text = "Remove appointment";
            this.deleteEventToolStripMenuItem.Click += new System.EventHandler(this.deleteEventToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(293, 6);
            // 
            // addStartLineToolStripMenuItem
            // 
            this.addStartLineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addingStartLineToolStripMenuItem,
            this.removingStartLineToolStripMenuItem});
            this.addStartLineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addStartLineToolStripMenuItem.Image")));
            this.addStartLineToolStripMenuItem.Name = "addStartLineToolStripMenuItem";
            this.addStartLineToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.addStartLineToolStripMenuItem.Text = "Add line to start";
            // 
            // addingStartLineToolStripMenuItem
            // 
            this.addingStartLineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addingStartLineToolStripMenuItem.Image")));
            this.addingStartLineToolStripMenuItem.Name = "addingStartLineToolStripMenuItem";
            this.addingStartLineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addingStartLineToolStripMenuItem.Text = "Add line";
            this.addingStartLineToolStripMenuItem.Click += new System.EventHandler(this.addingStartLineToolStripMenuItem_Click);
            // 
            // removingStartLineToolStripMenuItem
            // 
            this.removingStartLineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removingStartLineToolStripMenuItem.Image")));
            this.removingStartLineToolStripMenuItem.Name = "removingStartLineToolStripMenuItem";
            this.removingStartLineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removingStartLineToolStripMenuItem.Text = "Remove line";
            this.removingStartLineToolStripMenuItem.Click += new System.EventHandler(this.removingStartLineToolStripMenuItem_Click);
            // 
            // addEndLineToolStripMenuItem
            // 
            this.addEndLineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addingEndLineToolStripMenuItem,
            this.removingEndLineToolStripMenuItem});
            this.addEndLineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addEndLineToolStripMenuItem.Image")));
            this.addEndLineToolStripMenuItem.Name = "addEndLineToolStripMenuItem";
            this.addEndLineToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.addEndLineToolStripMenuItem.Text = "Add line to end";
            // 
            // addingEndLineToolStripMenuItem
            // 
            this.addingEndLineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addingEndLineToolStripMenuItem.Image")));
            this.addingEndLineToolStripMenuItem.Name = "addingEndLineToolStripMenuItem";
            this.addingEndLineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addingEndLineToolStripMenuItem.Text = "Add line";
            this.addingEndLineToolStripMenuItem.Click += new System.EventHandler(this.addingEndLineToolStripMenuItem_Click);
            // 
            // removingEndLineToolStripMenuItem
            // 
            this.removingEndLineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removingEndLineToolStripMenuItem.Image")));
            this.removingEndLineToolStripMenuItem.Name = "removingEndLineToolStripMenuItem";
            this.removingEndLineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removingEndLineToolStripMenuItem.Text = "Remove line";
            this.removingEndLineToolStripMenuItem.Click += new System.EventHandler(this.removingEndLineToolStripMenuItem_Click);
            // 
            // createEventToolStripMenuItem
            // 
            this.createEventToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDefaultEventToolStripMenuItem,
            this.createConfiguredEventToolStripMenuItem});
            this.createEventToolStripMenuItem.Enabled = false;
            this.createEventToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createEventToolStripMenuItem.Image")));
            this.createEventToolStripMenuItem.Name = "createEventToolStripMenuItem";
            this.createEventToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.createEventToolStripMenuItem.Text = "Add appointment for selected time period";
            // 
            // createDefaultEventToolStripMenuItem
            // 
            this.createDefaultEventToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createDefaultEventToolStripMenuItem.Image")));
            this.createDefaultEventToolStripMenuItem.Name = "createDefaultEventToolStripMenuItem";
            this.createDefaultEventToolStripMenuItem.Size = new System.Drawing.Size(268, 22);
            this.createDefaultEventToolStripMenuItem.Text = "Default appointment from start";
            this.createDefaultEventToolStripMenuItem.Click += new System.EventHandler(this.createDefaultEventToolStripMenuItem_Click);
            // 
            // createConfiguredEventToolStripMenuItem
            // 
            this.createConfiguredEventToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createConfiguredEventToolStripMenuItem.Image")));
            this.createConfiguredEventToolStripMenuItem.Name = "createConfiguredEventToolStripMenuItem";
            this.createConfiguredEventToolStripMenuItem.Size = new System.Drawing.Size(268, 22);
            this.createConfiguredEventToolStripMenuItem.Text = "Appointment in selected time period";
            this.createConfiguredEventToolStripMenuItem.Click += new System.EventHandler(this.createConfiguredEventToolStripMenuItem_Click);
            // 
            // TabDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dayView);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TabDay";
            this.Text = "TabDay";
            this.Shown += new System.EventHandler(this.TabDay_Shown);
            this.contextMenuStripEvents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEvents;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem addStartLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addingStartLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removingStartLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEndLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addingEndLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removingEndLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDefaultEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createConfiguredEventToolStripMenuItem;
        private DayView dayView;
    }
}