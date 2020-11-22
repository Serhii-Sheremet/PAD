namespace PAD
{
    partial class YearTranzits
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
            this.pictureBoxYearTranzits = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxYearTranzits)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxYearTranzits
            // 
            this.pictureBoxYearTranzits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxYearTranzits.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxYearTranzits.Name = "pictureBoxYearTranzits";
            this.pictureBoxYearTranzits.Size = new System.Drawing.Size(762, 427);
            this.pictureBoxYearTranzits.TabIndex = 0;
            this.pictureBoxYearTranzits.TabStop = false;
            this.pictureBoxYearTranzits.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxYearTranzits_MouseClick);
            this.pictureBoxYearTranzits.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxYearTranzits_MouseDoubleClick);
            // 
            // YearTranzits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(762, 427);
            this.Controls.Add(this.pictureBoxYearTranzits);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "YearTranzits";
            this.Text = "YearTranzits";
            this.Shown += new System.EventHandler(this.YearTranzits_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxYearTranzits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxYearTranzits;
    }
}