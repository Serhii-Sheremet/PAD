using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PAD
{
    public partial class DayViewToolTip : UserControl
    {
        private List<ToolTipEntity> _ttEList;
        private Font _titleFont;
        private Font _timeFont;
        private Font _textFont;

        public DayViewToolTip(List<ToolTipEntity> tteList, int width, int height, Font titleFont, Font timeFont, Font textFont)
        {
            InitializeComponent();

            this.Width = width;
            this.Height = height;
            _ttEList = tteList;
            _titleFont = titleFont;
            _timeFont = timeFont;
            _textFont = textFont;
        }
       
        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush b = new SolidBrush(Color.LightGoldenrodYellow))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }
            using (Pen p = new Pen(Color.FromArgb(118, 118, 118)))
            {
                Rectangle rect = ClientRectangle;
                rect.Width--;
                rect.Height--;
                e.Graphics.DrawRectangle(p, rect);
            }
            DrawToolTip(e);
        }

        public void DrawToolTip(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            TextFormatFlags flags = TextFormatFlags.WordBreak;

            int posX = 4, posY = 4, rectHeight = 0;
            foreach (ToolTipEntity tte in _ttEList)
            {
                rectHeight = Utility.CalculateRectangleHeightWithTextWrapping(tte.Title, _titleFont, this.Width - 8);
                Rectangle drawRect = new Rectangle(posX, posY, this.Width - 8, rectHeight);
                TextRenderer.DrawText(e.Graphics, tte.Title, _titleFont, drawRect, Color.Black, flags);
                posY += drawRect.Height + 4;

                rectHeight = Utility.CalculateRectangleHeightWithTextWrapping(tte.Period, _timeFont, this.Width - 8);
                drawRect = new Rectangle(posX, posY, this.Width - 8, rectHeight);
                TextRenderer.DrawText(e.Graphics, tte.Period, _timeFont, drawRect, Color.Black, flags);
                posY += drawRect.Height + 4;

                if (!tte.Description1.Equals(string.Empty))
                {
                    rectHeight = Utility.CalculateRectangleHeightWithTextWrapping(tte.Description1, _textFont, this.Width - 8);
                    drawRect = new Rectangle(posX, posY, this.Width - 8, rectHeight);
                    TextRenderer.DrawText(e.Graphics, tte.Description1, _textFont, drawRect, Color.Black, flags);
                    posY += drawRect.Height + 4;
                }
                if (!tte.Description2.Equals(string.Empty))
                {
                    rectHeight = Utility.CalculateRectangleHeightWithTextWrapping(tte.Description2, _textFont, this.Width - 8);
                    drawRect = new Rectangle(posX, posY, this.Width - 8, rectHeight);
                    TextRenderer.DrawText(e.Graphics, tte.Description2, _textFont, drawRect, Color.Black, flags);
                    posY += drawRect.Height + 4;
                }
                if (!tte.Description3.Equals(string.Empty))
                {
                    rectHeight = Utility.CalculateRectangleHeightWithTextWrapping(tte.Description3, _textFont, this.Width - 8);
                    drawRect = new Rectangle(posX, posY, this.Width - 8, rectHeight);
                    TextRenderer.DrawText(e.Graphics, tte.Description3, _textFont, drawRect, Color.Black, flags);
                    posY += drawRect.Height + 4;
                }
                if (!tte.Description4.Equals(string.Empty))
                {
                    rectHeight = Utility.CalculateRectangleHeightWithTextWrapping(tte.Description4, _textFont, this.Width - 8);
                    drawRect = new Rectangle(posX, posY, this.Width - 8, rectHeight);
                    TextRenderer.DrawText(e.Graphics, tte.Description4, _textFont, drawRect, Color.Black, flags);
                    posY += drawRect.Height;
                }
                posY += 4;
            }
        }


    }
}
