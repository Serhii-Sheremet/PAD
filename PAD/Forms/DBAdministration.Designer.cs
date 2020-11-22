namespace PAD
{
    partial class DBAdministration
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBAdministration));
            this.toolStripDBA = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefreshTables = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLoadDataFromFiles = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLocalData = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportCalendarFromFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExportCalendarsToFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExecuteFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExecute = new System.Windows.Forms.ToolStripButton();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.treeViewTables = new System.Windows.Forms.TreeView();
            this.splitContainerInfo = new System.Windows.Forms.SplitContainer();
            this.richTextBoxInput = new System.Windows.Forms.RichTextBox();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.tabPageGrid = new System.Windows.Forms.TabPage();
            this.tabPageText = new System.Windows.Forms.TabPage();
            this.listBoxTextResults = new System.Windows.Forms.ListBox();
            this.toolStripTextMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonTextSave = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStripTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripInput = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDBA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInfo)).BeginInit();
            this.splitContainerInfo.Panel1.SuspendLayout();
            this.splitContainerInfo.Panel2.SuspendLayout();
            this.splitContainerInfo.SuspendLayout();
            this.tabControlResults.SuspendLayout();
            this.tabPageText.SuspendLayout();
            this.toolStripTextMenu.SuspendLayout();
            this.contextMenuStripTree.SuspendLayout();
            this.contextMenuStripInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripDBA
            // 
            this.toolStripDBA.AutoSize = false;
            this.toolStripDBA.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripDBA.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonConnect,
            this.toolStripButtonRefreshTables,
            this.toolStripSeparator1,
            this.toolStripButtonLoadDataFromFiles,
            this.toolStripButtonLocalData,
            this.toolStripButtonImportCalendarFromFile,
            this.toolStripButtonExportCalendarsToFile,
            this.toolStripSeparator2,
            this.toolStripButtonExecuteFile,
            this.toolStripButtonExecute});
            this.toolStripDBA.Location = new System.Drawing.Point(0, 0);
            this.toolStripDBA.Name = "toolStripDBA";
            this.toolStripDBA.Size = new System.Drawing.Size(1077, 49);
            this.toolStripDBA.TabIndex = 0;
            // 
            // toolStripButtonConnect
            // 
            this.toolStripButtonConnect.AutoSize = false;
            this.toolStripButtonConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConnect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonConnect.Image")));
            this.toolStripButtonConnect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConnect.Name = "toolStripButtonConnect";
            this.toolStripButtonConnect.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonConnect.Text = "Connect to database";
            this.toolStripButtonConnect.Click += new System.EventHandler(this.toolStripButtonConnect_Click);
            // 
            // toolStripButtonRefreshTables
            // 
            this.toolStripButtonRefreshTables.AutoSize = false;
            this.toolStripButtonRefreshTables.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefreshTables.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefreshTables.Image")));
            this.toolStripButtonRefreshTables.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRefreshTables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefreshTables.Name = "toolStripButtonRefreshTables";
            this.toolStripButtonRefreshTables.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonRefreshTables.Text = "Refresh tables list";
            this.toolStripButtonRefreshTables.Click += new System.EventHandler(this.toolStripButtonRefreshTables_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 49);
            // 
            // toolStripButtonLoadDataFromFiles
            // 
            this.toolStripButtonLoadDataFromFiles.AutoSize = false;
            this.toolStripButtonLoadDataFromFiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoadDataFromFiles.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLoadDataFromFiles.Image")));
            this.toolStripButtonLoadDataFromFiles.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLoadDataFromFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoadDataFromFiles.Name = "toolStripButtonLoadDataFromFiles";
            this.toolStripButtonLoadDataFromFiles.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonLoadDataFromFiles.Text = "Загрузить данные из файлов";
            this.toolStripButtonLoadDataFromFiles.Click += new System.EventHandler(this.toolStripButtonLoadDataFromFiles_Click);
            // 
            // toolStripButtonLocalData
            // 
            this.toolStripButtonLocalData.AutoSize = false;
            this.toolStripButtonLocalData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLocalData.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLocalData.Image")));
            this.toolStripButtonLocalData.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLocalData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLocalData.Name = "toolStripButtonLocalData";
            this.toolStripButtonLocalData.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonLocalData.Text = "Загрузить локализацию из файлов";
            this.toolStripButtonLocalData.Click += new System.EventHandler(this.toolStripButtonLocalData_Click);
            // 
            // toolStripButtonImportCalendarFromFile
            // 
            this.toolStripButtonImportCalendarFromFile.AutoSize = false;
            this.toolStripButtonImportCalendarFromFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImportCalendarFromFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImportCalendarFromFile.Image")));
            this.toolStripButtonImportCalendarFromFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonImportCalendarFromFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportCalendarFromFile.Name = "toolStripButtonImportCalendarFromFile";
            this.toolStripButtonImportCalendarFromFile.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonImportCalendarFromFile.Text = "Загрузить в базу файл апдейта";
            this.toolStripButtonImportCalendarFromFile.Click += new System.EventHandler(this.toolStripButtonImportCalendarFromFile_Click);
            // 
            // toolStripButtonExportCalendarsToFile
            // 
            this.toolStripButtonExportCalendarsToFile.AutoSize = false;
            this.toolStripButtonExportCalendarsToFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExportCalendarsToFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExportCalendarsToFile.Image")));
            this.toolStripButtonExportCalendarsToFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonExportCalendarsToFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExportCalendarsToFile.Name = "toolStripButtonExportCalendarsToFile";
            this.toolStripButtonExportCalendarsToFile.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonExportCalendarsToFile.Text = "Приготовить файл апдейта календарей";
            this.toolStripButtonExportCalendarsToFile.Click += new System.EventHandler(this.toolStripButtonExportCalendarsToFile_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 49);
            // 
            // toolStripButtonExecuteFile
            // 
            this.toolStripButtonExecuteFile.AutoSize = false;
            this.toolStripButtonExecuteFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExecuteFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExecuteFile.Image")));
            this.toolStripButtonExecuteFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonExecuteFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExecuteFile.Name = "toolStripButtonExecuteFile";
            this.toolStripButtonExecuteFile.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonExecuteFile.Text = "Execute sql script from file";
            this.toolStripButtonExecuteFile.Click += new System.EventHandler(this.toolStripButtonExecuteFile_Click);
            // 
            // toolStripButtonExecute
            // 
            this.toolStripButtonExecute.AutoSize = false;
            this.toolStripButtonExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExecute.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExecute.Image")));
            this.toolStripButtonExecute.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExecute.Name = "toolStripButtonExecute";
            this.toolStripButtonExecute.Size = new System.Drawing.Size(37, 37);
            this.toolStripButtonExecute.Text = "Execute sql script";
            this.toolStripButtonExecute.Click += new System.EventHandler(this.toolStripButtonExecute_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 49);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.treeViewTables);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerInfo);
            this.splitContainerMain.Size = new System.Drawing.Size(1077, 510);
            this.splitContainerMain.SplitterDistance = 253;
            this.splitContainerMain.SplitterWidth = 5;
            this.splitContainerMain.TabIndex = 1;
            // 
            // treeViewTables
            // 
            this.treeViewTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTables.Location = new System.Drawing.Point(0, 0);
            this.treeViewTables.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewTables.Name = "treeViewTables";
            this.treeViewTables.Size = new System.Drawing.Size(253, 510);
            this.treeViewTables.TabIndex = 0;
            this.treeViewTables.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewTables_NodeMouseClick);
            this.treeViewTables.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewTables_MouseDoubleClick);
            // 
            // splitContainerInfo
            // 
            this.splitContainerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerInfo.Location = new System.Drawing.Point(0, 0);
            this.splitContainerInfo.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerInfo.Name = "splitContainerInfo";
            this.splitContainerInfo.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerInfo.Panel1
            // 
            this.splitContainerInfo.Panel1.Controls.Add(this.richTextBoxInput);
            // 
            // splitContainerInfo.Panel2
            // 
            this.splitContainerInfo.Panel2.Controls.Add(this.tabControlResults);
            this.splitContainerInfo.Size = new System.Drawing.Size(819, 510);
            this.splitContainerInfo.SplitterDistance = 285;
            this.splitContainerInfo.SplitterWidth = 5;
            this.splitContainerInfo.TabIndex = 0;
            // 
            // richTextBoxInput
            // 
            this.richTextBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxInput.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxInput.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxInput.Name = "richTextBoxInput";
            this.richTextBoxInput.Size = new System.Drawing.Size(819, 285);
            this.richTextBoxInput.TabIndex = 0;
            this.richTextBoxInput.Text = "";
            this.richTextBoxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBoxInput_KeyDown);
            this.richTextBoxInput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBoxInput_MouseDown);
            // 
            // tabControlResults
            // 
            this.tabControlResults.Controls.Add(this.tabPageGrid);
            this.tabControlResults.Controls.Add(this.tabPageText);
            this.tabControlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResults.Location = new System.Drawing.Point(0, 0);
            this.tabControlResults.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(819, 220);
            this.tabControlResults.TabIndex = 0;
            this.tabControlResults.SelectedIndexChanged += new System.EventHandler(this.tabControlResults_SelectedIndexChanged);
            // 
            // tabPageGrid
            // 
            this.tabPageGrid.AutoScroll = true;
            this.tabPageGrid.Location = new System.Drawing.Point(4, 25);
            this.tabPageGrid.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageGrid.Name = "tabPageGrid";
            this.tabPageGrid.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageGrid.Size = new System.Drawing.Size(811, 191);
            this.tabPageGrid.TabIndex = 0;
            this.tabPageGrid.Text = "Table view";
            this.tabPageGrid.UseVisualStyleBackColor = true;
            // 
            // tabPageText
            // 
            this.tabPageText.AutoScroll = true;
            this.tabPageText.Controls.Add(this.listBoxTextResults);
            this.tabPageText.Controls.Add(this.toolStripTextMenu);
            this.tabPageText.Location = new System.Drawing.Point(4, 22);
            this.tabPageText.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageText.Name = "tabPageText";
            this.tabPageText.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageText.Size = new System.Drawing.Size(811, 194);
            this.tabPageText.TabIndex = 1;
            this.tabPageText.Text = "Text view";
            this.tabPageText.UseVisualStyleBackColor = true;
            // 
            // listBoxTextResults
            // 
            this.listBoxTextResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxTextResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTextResults.FormattingEnabled = true;
            this.listBoxTextResults.ItemHeight = 16;
            this.listBoxTextResults.Location = new System.Drawing.Point(4, 29);
            this.listBoxTextResults.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxTextResults.Name = "listBoxTextResults";
            this.listBoxTextResults.Size = new System.Drawing.Size(803, 161);
            this.listBoxTextResults.TabIndex = 1;
            // 
            // toolStripTextMenu
            // 
            this.toolStripTextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTextSave});
            this.toolStripTextMenu.Location = new System.Drawing.Point(4, 4);
            this.toolStripTextMenu.Name = "toolStripTextMenu";
            this.toolStripTextMenu.Size = new System.Drawing.Size(803, 25);
            this.toolStripTextMenu.TabIndex = 0;
            // 
            // toolStripButtonTextSave
            // 
            this.toolStripButtonTextSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTextSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTextSave.Image")));
            this.toolStripButtonTextSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTextSave.Name = "toolStripButtonTextSave";
            this.toolStripButtonTextSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTextSave.Text = "Сохранить";
            this.toolStripButtonTextSave.Click += new System.EventHandler(this.toolStripButtonTextSave_Click);
            // 
            // contextMenuStripTree
            // 
            this.contextMenuStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyNodeToolStripMenuItem,
            this.deleteNodeToolStripMenuItem});
            this.contextMenuStripTree.Name = "contextMenuStripTree";
            this.contextMenuStripTree.Size = new System.Drawing.Size(108, 48);
            // 
            // copyNodeToolStripMenuItem
            // 
            this.copyNodeToolStripMenuItem.Name = "copyNodeToolStripMenuItem";
            this.copyNodeToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.copyNodeToolStripMenuItem.Text = "Copy";
            this.copyNodeToolStripMenuItem.Click += new System.EventHandler(this.copyNodeToolStripMenuItem_Click);
            // 
            // deleteNodeToolStripMenuItem
            // 
            this.deleteNodeToolStripMenuItem.Name = "deleteNodeToolStripMenuItem";
            this.deleteNodeToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteNodeToolStripMenuItem.Text = "Delete";
            this.deleteNodeToolStripMenuItem.Click += new System.EventHandler(this.deleteNodeToolStripMenuItem_Click);
            // 
            // contextMenuStripInput
            // 
            this.contextMenuStripInput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyInputToolStripMenuItem,
            this.pasteInputToolStripMenuItem});
            this.contextMenuStripInput.Name = "contextMenuStripInput";
            this.contextMenuStripInput.Size = new System.Drawing.Size(103, 48);
            // 
            // copyInputToolStripMenuItem
            // 
            this.copyInputToolStripMenuItem.Name = "copyInputToolStripMenuItem";
            this.copyInputToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyInputToolStripMenuItem.Text = "Copy";
            this.copyInputToolStripMenuItem.Click += new System.EventHandler(this.copyInputToolStripMenuItem_Click);
            // 
            // pasteInputToolStripMenuItem
            // 
            this.pasteInputToolStripMenuItem.Name = "pasteInputToolStripMenuItem";
            this.pasteInputToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.pasteInputToolStripMenuItem.Text = "Paste";
            this.pasteInputToolStripMenuItem.Click += new System.EventHandler(this.pasteInputToolStripMenuItem_Click);
            // 
            // DBAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 559);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStripDBA);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DBAdministration";
            this.ShowIcon = false;
            this.Text = "DBAdministration";
            this.toolStripDBA.ResumeLayout(false);
            this.toolStripDBA.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerInfo.Panel1.ResumeLayout(false);
            this.splitContainerInfo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInfo)).EndInit();
            this.splitContainerInfo.ResumeLayout(false);
            this.tabControlResults.ResumeLayout(false);
            this.tabPageText.ResumeLayout(false);
            this.tabPageText.PerformLayout();
            this.toolStripTextMenu.ResumeLayout(false);
            this.toolStripTextMenu.PerformLayout();
            this.contextMenuStripTree.ResumeLayout(false);
            this.contextMenuStripInput.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripDBA;
        private System.Windows.Forms.ToolStripButton toolStripButtonConnect;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TreeView treeViewTables;
        private System.Windows.Forms.SplitContainer splitContainerInfo;
        private System.Windows.Forms.RichTextBox richTextBoxInput;
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabPageGrid;
        private System.Windows.Forms.TabPage tabPageText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonExecute;
        private System.Windows.Forms.ToolStripButton toolStripButtonExecuteFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefreshTables;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTree;
        private System.Windows.Forms.ToolStripMenuItem copyNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteNodeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripInput;
        private System.Windows.Forms.ToolStripMenuItem copyInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoadDataFromFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportCalendarsToFile;
        private System.Windows.Forms.ListBox listBoxTextResults;
        private System.Windows.Forms.ToolStrip toolStripTextMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonTextSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonLocalData;
        private System.Windows.Forms.ToolStripButton toolStripButtonImportCalendarFromFile;
    }
}