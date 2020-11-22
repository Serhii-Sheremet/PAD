using System.Windows.Forms;

namespace PAD
{
    partial class UserEventMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserEventMenu));
            this.buttonOK = new System.Windows.Forms.Button();
            this.colorDialogMessage = new System.Windows.Forms.ColorDialog();
            this.comboBoxTimeFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxTimeTo = new System.Windows.Forms.ComboBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.groupBoxColor = new System.Windows.Forms.GroupBox();
            this.pictureBoxColor = new System.Windows.Forms.PictureBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.datePickerFrom = new CustomControls.DatePicker();
            this.datePickerTo = new CustomControls.DatePicker();
            this.groupBoxTitle = new System.Windows.Forms.GroupBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.groupBoxMessage = new System.Windows.Forms.GroupBox();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.groupBoxColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).BeginInit();
            this.groupBoxTitle.SuspendLayout();
            this.groupBoxMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.AutoSize = true;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Location = new System.Drawing.Point(402, 311);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 28);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxTimeFrom
            // 
            this.comboBoxTimeFrom.DropDownHeight = 66;
            this.comboBoxTimeFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTimeFrom.FormattingEnabled = true;
            this.comboBoxTimeFrom.IntegralHeight = false;
            this.comboBoxTimeFrom.Location = new System.Drawing.Point(233, 70);
            this.comboBoxTimeFrom.Name = "comboBoxTimeFrom";
            this.comboBoxTimeFrom.Size = new System.Drawing.Size(67, 23);
            this.comboBoxTimeFrom.TabIndex = 8;
            // 
            // comboBoxTimeTo
            // 
            this.comboBoxTimeTo.DropDownHeight = 66;
            this.comboBoxTimeTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTimeTo.FormattingEnabled = true;
            this.comboBoxTimeTo.IntegralHeight = false;
            this.comboBoxTimeTo.Location = new System.Drawing.Point(233, 96);
            this.comboBoxTimeTo.Name = "comboBoxTimeTo";
            this.comboBoxTimeTo.Size = new System.Drawing.Size(67, 23);
            this.comboBoxTimeTo.TabIndex = 9;
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(24, 73);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(39, 16);
            this.labelFrom.TabIndex = 10;
            this.labelFrom.Text = "From";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(24, 99);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(25, 16);
            this.labelTo.TabIndex = 11;
            this.labelTo.Text = "To";
            // 
            // groupBoxColor
            // 
            this.groupBoxColor.Controls.Add(this.pictureBoxColor);
            this.groupBoxColor.Location = new System.Drawing.Point(305, 62);
            this.groupBoxColor.Name = "groupBoxColor";
            this.groupBoxColor.Size = new System.Drawing.Size(265, 57);
            this.groupBoxColor.TabIndex = 12;
            this.groupBoxColor.TabStop = false;
            this.groupBoxColor.Text = "Color example";
            // 
            // pictureBoxColor
            // 
            this.pictureBoxColor.Location = new System.Drawing.Point(8, 18);
            this.pictureBoxColor.Name = "pictureBoxColor";
            this.pictureBoxColor.Size = new System.Drawing.Size(250, 30);
            this.pictureBoxColor.TabIndex = 0;
            this.pictureBoxColor.TabStop = false;
            this.pictureBoxColor.Click += new System.EventHandler(this.pictureBoxColor_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(490, 311);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 28);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // datePickerFrom
            // 
            this.datePickerFrom.Location = new System.Drawing.Point(68, 69);
            this.datePickerFrom.Name = "datePickerFrom";
            this.datePickerFrom.Size = new System.Drawing.Size(160, 24);
            this.datePickerFrom.TabIndex = 15;
            // 
            // datePickerTo
            // 
            this.datePickerTo.Location = new System.Drawing.Point(68, 95);
            this.datePickerTo.Name = "datePickerTo";
            this.datePickerTo.Size = new System.Drawing.Size(160, 24);
            this.datePickerTo.TabIndex = 16;
            // 
            // groupBoxTitle
            // 
            this.groupBoxTitle.Controls.Add(this.textBoxName);
            this.groupBoxTitle.Location = new System.Drawing.Point(16, 7);
            this.groupBoxTitle.Name = "groupBoxTitle";
            this.groupBoxTitle.Size = new System.Drawing.Size(554, 53);
            this.groupBoxTitle.TabIndex = 17;
            this.groupBoxTitle.TabStop = false;
            this.groupBoxTitle.Text = "Title";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(10, 20);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxName.MaxLength = 80;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(537, 22);
            this.textBoxName.TabIndex = 3;
            // 
            // groupBoxMessage
            // 
            this.groupBoxMessage.Controls.Add(this.richTextBoxMessage);
            this.groupBoxMessage.Location = new System.Drawing.Point(16, 122);
            this.groupBoxMessage.Name = "groupBoxMessage";
            this.groupBoxMessage.Padding = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.groupBoxMessage.Size = new System.Drawing.Size(556, 180);
            this.groupBoxMessage.TabIndex = 18;
            this.groupBoxMessage.TabStop = false;
            this.groupBoxMessage.Text = "Description";
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessage.Location = new System.Drawing.Point(8, 18);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(540, 154);
            this.richTextBoxMessage.TabIndex = 15;
            this.richTextBoxMessage.Text = "";
            // 
            // UserEventMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(584, 351);
            this.Controls.Add(this.groupBoxMessage);
            this.Controls.Add(this.groupBoxTitle);
            this.Controls.Add(this.datePickerTo);
            this.Controls.Add(this.datePickerFrom);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBoxColor);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.comboBoxTimeTo);
            this.Controls.Add(this.comboBoxTimeFrom);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserEventMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add appointment";
            this.Shown += new System.EventHandler(this.UserEventMenu_Shown);
            this.groupBoxColor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).EndInit();
            this.groupBoxTitle.ResumeLayout(false);
            this.groupBoxTitle.PerformLayout();
            this.groupBoxMessage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private ColorDialog colorDialogMessage;
        private ComboBox comboBoxTimeFrom;
        private ComboBox comboBoxTimeTo;
        private Label labelFrom;
        private Label labelTo;
        private GroupBox groupBoxColor;
        private PictureBox pictureBoxColor;
        private Button buttonCancel;
        private CustomControls.DatePicker datePickerFrom;
        private CustomControls.DatePicker datePickerTo;
        private GroupBox groupBoxTitle;
        private TextBox textBoxName;
        private GroupBox groupBoxMessage;
        private RichTextBox richTextBoxMessage;
    }
}