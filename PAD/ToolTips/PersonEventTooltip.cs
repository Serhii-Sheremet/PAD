using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class PersonEventTooltip : UserControl
    {
        private List<PersonsEventsList> _peList;
        private Font _dateFont;
        private Font _timeFont;
        private Font _textFont;
        private ELanguage _langCode;

        public PersonEventTooltip()
        {
            InitializeComponent();
        }

        public PersonEventTooltip(List<PersonsEventsList> peList, int width, int height, Font dateFont, Font timeFont, Font textFont, ELanguage lCode)
        {
            InitializeComponent();
            _peList = peList;
            _dateFont = dateFont;
            _timeFont = timeFont;
            _textFont = textFont;
            _langCode = lCode;
            this.Width = width;
            this.Height = height;
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

            SolidBrush textBrush = new SolidBrush(Color.Black);

            StringFormat format = new StringFormat();
            format.FormatFlags = StringFormatFlags.FitBlackBox;

            int posX = 4, posY = 4, textHeight = 0;
            string text = _peList[0].DateStart.ToString("dd.MM.yyyy");
            Size textSize = TextRenderer.MeasureText(text, _dateFont);
            e.Graphics.DrawString(text, _dateFont, textBrush, posX, posY);
            posY += textSize.Height;

            List<PersonsEventsList> sortedList = _peList.OrderBy(i => i.DateStart).ToList();
            foreach (PersonsEventsList pev in sortedList)
            {
                text = Utility.GetLocalizedText("Time period", _langCode) + ": " + pev.DateStart.ToString("HH:mm") + " - " + pev.DateEnd.ToString("HH:mm");
                textSize = TextRenderer.MeasureText(text, _timeFont);
                e.Graphics.DrawString(text, _timeFont, textBrush, posX, posY);
                posY += textSize.Height + 4;

                text = pev.Name;
                textSize = TextRenderer.MeasureText(text, _textFont);
                if(textSize.Width > (this.Width - 8))
                {
                    int linesCount = textSize.Width / (this.Width - 8) + 2;
                    textHeight = textSize.Height * linesCount;
                }
                else
                {
                    textHeight = textSize.Height;
                }
                Rectangle drawRect = new Rectangle(posX, posY, this.Width - 8, textHeight);
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(pev.ARGBValue)), drawRect);
                e.Graphics.DrawString(text, _textFont, textBrush, drawRect, format);
                posY += drawRect.Height + 4;
            }
        }

    }
}
