using System.Drawing;
using System.Windows.Forms;

namespace PAD
{
    public partial class TranzitsToolTip : UserControl
    {
        private DataGridView dgView;

        public TranzitsToolTip()
        {
            InitializeComponent();
        }

        public TranzitsToolTip(DataGridView dgv, int width, int height)
        {
            InitializeComponent();
            dgView = dgv;
            this.Width = width;
            this.Height = height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //using (Brush b = new LinearGradientBrush(ClientRectangle, Color.White, BackColor, LinearGradientMode.Vertical))
            using (Brush b = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }
            using (Pen p = new Pen(Color.FromArgb(118, 118, 118), 2))
            {
                Rectangle rect = ClientRectangle;
                rect.Width--;
                rect.Height--;
                e.Graphics.DrawRectangle(p, rect);
            }
            DrawDataGridView(e);
        }

        private void DrawDataGridView(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            dgView.Parent = this;
            this.Controls.Add(dgView);
            dgView.Dock = DockStyle.Fill;
            dgView.AllowUserToAddRows = false;
            dgView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgView.AllowUserToResizeColumns = false;
            dgView.AllowUserToResizeRows = false;
            dgView.RowHeadersVisible = false;
            dgView.ScrollBars = ScrollBars.None;
            dgView.ReadOnly = true;
            dgView.RowsDefaultCellStyle.SelectionBackColor = dgView.RowsDefaultCellStyle.BackColor.IsEmpty ? Color.White : dgView.RowsDefaultCellStyle.BackColor;
            dgView.RowsDefaultCellStyle.SelectionForeColor = dgView.RowsDefaultCellStyle.ForeColor.IsEmpty ? Color.Black : dgView.RowsDefaultCellStyle.ForeColor;
            dgView.Show();

        }

    }
}
