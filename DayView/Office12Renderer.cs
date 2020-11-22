using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PAD
{
    public class Office12Renderer : AbstractRenderer
    {
        protected override void Dispose(bool mainThread)
        {
            base.Dispose(mainThread);

            if (baseFont != null)
                baseFont.Dispose();
        }

        Font baseFont;

        public override Font BaseFont
        {
            get
            {
                if (baseFont == null)
                {
                    baseFont = new Font("Segoe UI", 9, FontStyle.Regular); //Font
                }

                return baseFont;
            }
        }

        public override Color HourColor
        {
            get
            {
                return Color.FromArgb(230, 237, 247);
            }
        }

        public override Color HalfHourSeperatorColor
        {
            get
            {
                return Color.FromArgb(165, 191, 225);
            }
        }

        public override Color HourSeperatorColor
        {
            get
            {
                return Color.FromArgb(213, 215, 241);
            }
        }

        public override Color WorkingHourColor
        {
            get
            {
                return Color.FromArgb(255, 255, 255);
            }
        }

        public override Color BackColor
        {
            get
            {
                return Color.FromArgb(213, 228, 242);
            }
        }

        public override Color SelectionColor
        {
            get
            {
                return Color.FromArgb(41, 76, 122);
            }
        }

        public Color TextColor
        {
            get
            {
                return Color.FromArgb(101, 147, 207);
            }
        }

        public override void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool ampm, string cultureCode)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (SolidBrush brush = new SolidBrush(this.TextColor))
            {
                string ampmtime;
                /*
                if (ampm)
                {
                    if (hour < 12)
                        ampmtime = "AM";
                    else
                        ampmtime = "PM";

                    if (hour != 12)
                        hour = hour % 12;
                }
                else*/
                    ampmtime = "00";

                g.DrawString(hour.ToString("##00", System.Globalization.CultureInfo.GetCultureInfo(cultureCode)) /*InvariantCulture)*/, HourFont, brush, rect); //add minutes to free blocks later

                rect.X += 15; // path between hours number and minutes
                g.DrawString(ampmtime, MinuteFont, brush, rect);
            }
        }

        private string GetSunStatusName(string culture, bool isSunRise)
        {
            string name = string.Empty;
            if (isSunRise)
            {

                if (culture.Equals("en-US"))
                    name = "Sr: ";
                if (culture.Equals("ru-RU"))
                    name = "Вс: ";
            }
            else
            {
                if (culture.Equals("en-US"))
                    name = "Ss: ";
                if (culture.Equals("ru-RU"))
                    name = "Зх: ";
            }
            return name;
        }

        public override void DrawSunLabel(Graphics g, Rectangle rect, bool isSunRise, DateTime sunDate, Color color, string cultureCode)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            string text = string.Empty;
            if (isSunRise)
            {
                text = GetSunStatusName(cultureCode, true) + sunDate.ToString("HH:mm");
            }
            else
            {
                text = GetSunStatusName(cultureCode, false) + sunDate.ToString("HH:mm");
            }

            g.DrawString(text, SunFont, new SolidBrush(color), rect);
        }
        
        public override void DrawMinuteLine(Graphics g, Rectangle rect)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (Pen pen = new Pen(InterpolateColors(this.TextColor, Color.White, 0.5f)))
                g.DrawLine(pen, rect.Left, rect.Y, rect.Width, rect.Y);
        }

        public override void DrawMinuteLine(Graphics g, int posX, int posY, int width, Color color)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (Pen pen = new Pen(InterpolateColors(this.TextColor, color, 1f)))
                g.DrawLine(pen, posX, posY, width, posY);
        }

        public override void DrawDayHeader(Graphics g, Rectangle rect, DateTime date, int day, string transName, string cultureCode)
        {
            if (g == null)
                throw new ArgumentNullException("g");


            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;

                using (StringFormat formatdd = new StringFormat())
                {
                    formatdd.Alignment = StringAlignment.Near;
                    formatdd.FormatFlags = StringFormatFlags.NoWrap;
                    formatdd.LineAlignment = StringAlignment.Center;

                    using (SolidBrush brush = new SolidBrush(this.BackColor))
                        g.FillRectangle(brush, rect);

                    using (Pen aPen = new Pen(Color.FromArgb(205, 219, 238)))
                        g.DrawLine(aPen, rect.Left, rect.Top + (int)rect.Height / 2, rect.Right, rect.Top + (int)rect.Height / 2);

                    using (Pen aPen = new Pen(Color.FromArgb(141, 174, 217)))
                        g.DrawRectangle(aPen, rect);

                    rect.X += 1;
                    rect.Width -= 1;
                    using (Pen aPen = new Pen(Color.FromArgb(141, 174, 217)))
                        g.DrawRectangle(aPen, rect);

                    Rectangle topPart = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, (int)(rect.Height / 2) - 1);
                    Rectangle lowPart = new Rectangle(rect.Left + 1, rect.Top + (int)(rect.Height / 2) + 1, rect.Width - 1, (int)(rect.Height / 2) - 1);

                    using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(228, 236, 246), Color.FromArgb(214, 226, 241), LinearGradientMode.Vertical))
                        g.FillRectangle(aGB, topPart);

                    using (LinearGradientBrush aGB = new LinearGradientBrush(lowPart, Color.FromArgb(194, 212, 235), Color.FromArgb(208, 222, 239), LinearGradientMode.Vertical))
                        g.FillRectangle(aGB, lowPart);

                    if (date.Date.Equals(DateTime.Now.Date))
                    {
                        topPart.Inflate((int)(-topPart.Width / 4 + 1), 1); //top left orange area
                        topPart.Offset(rect.Left - topPart.Left + 1, 1);
                        topPart.Inflate(1, 0);
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(247, 207, 114), Color.FromArgb(251, 230, 148), LinearGradientMode.Horizontal))
                        {
                            topPart.Inflate(-1, 0);
                            g.FillRectangle(aGB, topPart);
                        }

                        topPart.Offset(rect.Right - topPart.Right, 0);        //top right orange
                        topPart.Inflate(1, 0);
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(251, 230, 148), Color.FromArgb(247, 207, 114), LinearGradientMode.Horizontal))
                        {
                            topPart.Inflate(-1, 0);
                            g.FillRectangle(aGB, topPart);
                        }

                        using (Pen aPen = new Pen(Color.FromArgb(128, 240, 154, 30))) //center line
                            g.DrawLine(aPen, rect.Left, topPart.Bottom - 1, rect.Right, topPart.Bottom - 1);

                        topPart.Inflate(0, -1);
                        topPart.Offset(0, topPart.Height + 1); //lower right
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(240, 157, 33), Color.FromArgb(250, 226, 142), LinearGradientMode.BackwardDiagonal))
                            g.FillRectangle(aGB, topPart);

                        topPart.Offset(rect.Left - topPart.Left + 1, 0); //lower left
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(240, 157, 33), Color.FromArgb(250, 226, 142), LinearGradientMode.ForwardDiagonal))
                            g.FillRectangle(aGB, topPart);
                        using (Pen aPen = new Pen(Color.FromArgb(238, 147, 17)))
                            g.DrawRectangle(aPen, rect);
                    }

                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    //get short dayabbr. if narrow dayrect
                    string sTodaysName = date.ToString("dd MMMM yyyy", System.Globalization.CultureInfo.GetCultureInfo(cultureCode)); 
                    if (rect.Width < 105)
                        sTodaysName = date.ToString("dd.MM.yyyy"); 

                    rect.Offset(2, 1);

                    if (day == 0)
                    {
                        using (Font fntDay = new Font("Segoe UI", 8))
                            g.DrawString(sTodaysName, fntDay, SystemBrushes.WindowText, rect, format);

                    }
                    else if (day == 1)
                    {
                        using (Font fntDay = new Font("Segoe UI", 8))
                            g.DrawString(transName, fntDay, SystemBrushes.WindowText, rect, format);
                    }

                    rect.Offset(-2, -1);

                }
            }
        }

        private string ChangeTextEncoding(string text)
        {
            Encoding iso = Encoding.GetEncoding("windows-1251");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(text);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            string msg = iso.GetString(isoBytes);

            return msg;
        }

        public override void DrawDayBackground(Graphics g, Rectangle rect)
        {

            using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(0xe6, 0xed, 0xf7)))
                g.FillRectangle(backBrush, rect);
        }

        public override void DrawAppointment(Graphics g, Rectangle rect, Appointment appointment, bool isSelected, Rectangle gripRect)
        {
            if (appointment == null)
                throw new ArgumentNullException("appointment");

            if (g == null)
                throw new ArgumentNullException("g");

            /*
             * Logic for drawing the appointment: 
             * 1) Do something messy with the colours
             * 
             * 2) Determine background pattern
             * 2.1) App is locked -> HatchBrush
             * 2.2) Normal app -> Nothing
             * 
             * 3) Draw the background of appointment
             * 
             * 4) Draw the edges of appointment
             * 4.1) If app is selected -> just draw the selection rectangle
             * 4.2) If not -> draw the gripper, border (if required) and shadows
             */

            if (rect.Width != 0 && rect.Height != 0) // maybe >0 ?
            {

                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near; //Title format
                    format.LineAlignment = StringAlignment.Near;

                    Color start = InterpolateColors(appointment.Color, Color.White, 0.4f);
                    Color end = InterpolateColors(appointment.Color, Color.FromArgb(191, 210, 234), 0.7f);
                    // if appointment is locked, draw different background pattern
                    if ((appointment.Locked))
                    {
                        // Draw back
                        using (Brush m_Brush = new HatchBrush(HatchStyle.Percent05, appointment.Color, appointment.Color)) //background
                            g.FillRectangle(m_Brush, rect);

                        // little transparent      //remove transparent
                        start = Color.FromArgb(230, start);
                        end = Color.FromArgb(180, end);

                        GraphicsPath path = new GraphicsPath();
                        path.AddRectangle(rect);


                    }

                    // Draw the background of the appointment

                    using (LinearGradientBrush aGB = new LinearGradientBrush(rect, start, start, 0.0))
                        g.FillRectangle(aGB, rect);

                    // If the appointment is selected, only need to draw the selection frame

                    if (isSelected)
                    {
                        Rectangle m_BorderRectangle = rect;

                        using (Pen m_Pen = new Pen(appointment.BorderColor, 2))
                            g.DrawRectangle(m_Pen, rect);

                        m_BorderRectangle.Inflate(1, 1);

                        using (Pen m_Pen = new Pen(SystemColors.WindowFrame, 1))
                            g.DrawRectangle(m_Pen, m_BorderRectangle);

                        m_BorderRectangle.Inflate(-3, -3);

                        using (Pen m_Pen = new Pen(SystemColors.WindowFrame, 1))
                            g.DrawRectangle(m_Pen, m_BorderRectangle);
                    }
                    /*
                     //draw line on the left of appointment
                    else
                    {
                        // Draw gripper

                        gripRect.Width += 3; //color rectangle on the left side of each event

                        start = InterpolateColors(appointment.BorderColor, appointment.Color, 0.2f);
                        end = InterpolateColors(appointment.BorderColor, Color.White, 0.6f);

                        using (LinearGradientBrush aGB = new LinearGradientBrush(rect, start, start, 0.0)) // LinearGradientMode.Vertical 0.0
                            g.FillRectangle(aGB, gripRect);
                    */

                    //  Draw border if needed
                    if (appointment.DrawBorder)
                            using (Pen m_Pen = new Pen(SystemColors.WindowFrame, 1))
                                g.DrawRectangle(m_Pen, rect);
                    /*
                        // Draw shadow lines
                        int xLeft = rect.X + 6;
                        int xRight = rect.Right + 1;
                        int yTop = rect.Y + 1;
                        int yButton = rect.Bottom + 1;

                        for (int i = 0; i < 5; i++)
                        {
                            using (Pen shadow_Pen = new Pen(Color.FromArgb(70 - 12 * i, Color.Black)))
                            {
                                g.DrawLine(shadow_Pen, xLeft + i, yButton + i, xRight + i - 1, yButton + i); //horisontal lines
                                g.DrawLine(shadow_Pen, xRight + i, yTop + i, xRight + i, yButton + i); //vertical
                            }
                        }

                    }
                    */

                    // draw appointment text
                    rect.X += gripRect.Width; 

                    //System.Windows.Forms.MessageBox.Show("gripRect.Width=" + gripRect.Width);

                    // width of shadow is 6.
                    rect.Width -= 6;

                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    using (Font fntLine = new Font("Segoe UI", 7))
                        g.DrawString(appointment.Title, fntLine, SystemBrushes.WindowText, rect, format);
                    g.TextRenderingHint = TextRenderingHint.SystemDefault;
                }
            }
        }
    }
}
