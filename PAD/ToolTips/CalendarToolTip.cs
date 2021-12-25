using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class CalendarToolTip : UserControl
    {
        private Day currDay;
        private Profile sPerson;
        private ELanguage lang;
        private int linesCount;
        private int lineHeight;
        int dayFrameHeight = 40;

        public CalendarToolTip()
        {
            InitializeComponent();
        }

        public CalendarToolTip(Profile pers, Day day, int formHeight, int lc, ELanguage langCode)
        {
            InitializeComponent();
            sPerson = pers;
            currDay = day;
            lang = langCode;
            this.Height = formHeight;
            linesCount = lc;
            lineHeight = (this.Height - dayFrameHeight - 12) / linesCount;
        }

        public CalendarToolTip(Profile pers, Day day, int formWidth, int formHeight, int lc, ELanguage langCode)
        {
            InitializeComponent();
            sPerson = pers;
            currDay = day;
            lang = langCode;
            this.Width = formWidth;
            this.Height = formHeight;
            linesCount = lc;
            lineHeight = (this.Height - dayFrameHeight - 12) / linesCount;
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
            DrawDay(e);
        }

        private Color MakeMuhurtaColorByCode(EColor cCode)
        {
            switch (cCode)
            {
                case EColor.GREEN:
                    return Color.Green;
            }
            return Utility.GetColorByColorCode(cCode);
        }

        public void DrawDay(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            SolidBrush outDayBrush = new SolidBrush(Color.Gray);
            SolidBrush sunBrush = new SolidBrush(Color.Green);

            int posX = 3, posY = 0;

            Font dayFont = new Font(FontFamily.GenericSansSerif, dayFrameHeight - 4, FontStyle.Bold, GraphicsUnit.Pixel);
            Font sunFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Point);
            Font lineFont = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANZITTEXT)), 10, Utility.GetFontStyleBySettings(EFontList.TRANZITTEXT));

            EAppSetting nodeSettings = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;

            // Day
            string dayText = currDay.Date.Day.ToString();
            Size dayTextSize = TextRenderer.MeasureText(dayText, dayFont);
            if (currDay.IsDayOfMonth)
                e.Graphics.DrawString(dayText, dayFont, textBrush, posX, posY + 2);
            else
                e.Graphics.DrawString(dayText, dayFont, outDayBrush, posX, posY + 2);

            // Sunrise / sunset
            string sunriseTime = Utility.GetSunStatusName(ESun.SUNRISE, lang) + currDay.SunRise?.ToString("HH:mm:ss");
            string sunsetTime = Utility.GetSunStatusName(ESun.SUNSET, lang) + currDay.SunSet?.ToString("HH:mm:ss");
            Size textSize = TextRenderer.MeasureText(sunsetTime, sunFont);
            int posYSunrise = posY + ((dayFrameHeight / 2  - textSize.Height) / 2);
            int posYSunset = posYSunrise + textSize.Height;
            e.Graphics.DrawString(sunriseTime, sunFont, sunBrush, posX + dayTextSize.Width, posY + posYSunrise + 2);
            e.Graphics.DrawString(sunsetTime, sunFont, sunBrush, posX + dayTextSize.Width, posY + posYSunset + 2);

            // Muhurta
            int posXMuhurta = posX + dayTextSize.Width + textSize.Width - 4;
            int posYMuhurta = 4;
            int muhurtaWidth = (this.Width - posXMuhurta - 3) / 5;
            int muhurtaHeight = (dayFrameHeight - 10) / 2 + 1;
            Font muhurtaFont = new Font(FontFamily.GenericSansSerif, muhurtaHeight / 2 + 1, FontStyle.Regular, GraphicsUnit.Pixel);

            foreach (Calendar c in currDay.MuhurtaDayList)
            {
                MuhurtaCalendar mc = (MuhurtaCalendar)c;
                string mName = CacheLoad._muhurtaDescList.Where(i => i.MuhurtaId == (int)mc.MuhurtaCode && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                Rectangle rectTitle = new Rectangle(posXMuhurta, posYMuhurta, muhurtaWidth, muhurtaHeight);
                Size mSize = TextRenderer.MeasureText(mName, muhurtaFont);
                e.Graphics.DrawString(mName, muhurtaFont, textBrush, posXMuhurta + (muhurtaWidth / 2 - mSize.Width / 2), posYMuhurta + (muhurtaHeight / 2 - mSize.Height / 2));
                e.Graphics.DrawRectangle(pen, rectTitle);

                Rectangle rectTime = new Rectangle(posXMuhurta, posYMuhurta + muhurtaHeight, muhurtaWidth, muhurtaHeight);
                string textTime = mc.DateStart.ToString("HH:mm:ss") + " - " + mc.DateEnd.ToString("HH:mm:ss");
                Size tSize = TextRenderer.MeasureText(textTime, muhurtaFont);
                e.Graphics.DrawString(textTime, muhurtaFont, new SolidBrush(MakeMuhurtaColorByCode(mc.ColorCode)), posXMuhurta + (muhurtaWidth / 2 - tSize.Width / 2), posYMuhurta + muhurtaHeight + (muhurtaHeight / 2 - tSize.Height / 2));
                e.Graphics.DrawRectangle(pen, rectTime);

                posXMuhurta = posXMuhurta + muhurtaWidth;
            }
            
            if (currDay.Date.DayOfWeek == DayOfWeek.Wednesday)
            {
                Rectangle rectTitle = new Rectangle(posXMuhurta, posYMuhurta, muhurtaWidth, muhurtaHeight);
                string mText = CacheLoad._muhurtaDescList.Where(i => i.MuhurtaId == (int)(EMuhurta.ABHIJIT) && i.LanguageCode.Equals(lang.ToString())).FirstOrDefault()?.Name ?? string.Empty;
                Size mSize = TextRenderer.MeasureText(mText, muhurtaFont);
                e.Graphics.DrawString(mText, muhurtaFont, textBrush, posXMuhurta + (muhurtaWidth / 2 - mSize.Width / 2), posYMuhurta + (muhurtaHeight / 2 - mSize.Height / 2));
                e.Graphics.DrawRectangle(pen, rectTitle);

                Rectangle rectTime = new Rectangle(posXMuhurta, posYMuhurta + muhurtaHeight, muhurtaWidth, muhurtaHeight);
                string textTime = Utility.GetLocalizedText("not formed", lang);
                Size tSize = TextRenderer.MeasureText(textTime, muhurtaFont);
                e.Graphics.DrawString(textTime, muhurtaFont, textBrush, posXMuhurta + +(muhurtaWidth / 2 - tSize.Width / 2), posYMuhurta + muhurtaHeight + (muhurtaHeight / 2 - mSize.Height / 2));
                e.Graphics.DrawRectangle(pen, rectTime);
            }
            
            // Panchanga
            int posYForLines = posY + dayFrameHeight + 2;
            DrawColoredLine(e, pen, lineFont, textBrush, posX, posYForLines, this.Width - 8, lineHeight, currDay.NakshatraDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForLines + lineHeight), this.Width - 8, lineHeight, currDay.TaraBalaDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForLines + 2 * lineHeight), this.Width - 8, lineHeight, currDay.TithiDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForLines + 3 * lineHeight), this.Width - 8, lineHeight, currDay.KaranaDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForLines + 4 * lineHeight), this.Width - 8, lineHeight, currDay.NityaJogaDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForLines + 5 * lineHeight), this.Width - 8, lineHeight, currDay.ChandraBalaDayList, currDay.Date);

            // Tranzity
            int posYForTransits = posYForLines + 6 * lineHeight + 4;
            DrawColoredLine(e, pen, lineFont, textBrush, posX, posYForTransits, this.Width - 8, lineHeight, currDay.MoonZodiakRetroDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + lineHeight), this.Width - 8, lineHeight, currDay.SunZodiakRetroDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 2 * lineHeight), this.Width - 8, lineHeight, currDay.VenusZodiakRetroDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 3 * lineHeight), this.Width - 8, lineHeight, currDay.JupiterZodiakRetroDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 4 * lineHeight), this.Width - 8, lineHeight, currDay.MercuryZodiakRetroDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 5 * lineHeight), this.Width - 8, lineHeight, currDay.MarsZodiakRetroDayList, currDay.Date);
            DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 6 * lineHeight), this.Width - 8, lineHeight, currDay.SaturnZodiakRetroDayList, currDay.Date);

            switch (nodeSettings)
            {
                case EAppSetting.NODEMEAN:
                    DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 7 * lineHeight), this.Width - 8, lineHeight, currDay.RahuMeanZodiakRetroDayList, currDay.Date);
                    DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 8 * lineHeight), this.Width - 8, lineHeight, currDay.KetuMeanZodiakRetroDayList, currDay.Date);
                    break;

                case EAppSetting.NODETRUE:
                    DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 7 * lineHeight), this.Width - 8, lineHeight, currDay.RahuTrueZodiakRetroDayList, currDay.Date);
                    DrawColoredLine(e, pen, lineFont, textBrush, posX, (posYForTransits + 8 * lineHeight), this.Width - 8, lineHeight, currDay.KetuTrueZodiakRetroDayList, currDay.Date);
                    break;
            }
            
            // Muhurta
            int posYForMuhurta = posYForTransits + 9 * lineHeight + 4;
            DrawMuhurtaColoredLine(e, pen, lineFont, textBrush, posX, posYForMuhurta, this.Width - 8, lineHeight, currDay.MuhurtaDayList, currDay.Date);

            // Joga
            int posYForJogas = posYForMuhurta + 5 * lineHeight + 4;
            DrawJogaLines(e, pen, lineFont, textBrush, posX, posYForJogas, this.Width - 8, lineHeight, currDay);
            
        }
        
        private void DrawMuhurtaColoredLine(PaintEventArgs e, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, List<Calendar> mList, DateTime date)
        {
            float start = 0, end = 0, drawWidth = 0;
            Size textSize;
            int heightPadding = 0;
            Rectangle emptyRect;
            foreach (Calendar c in mList)
            {
                emptyRect = new Rectangle(posX, posY, width, height);
                e.Graphics.DrawRectangle(pen, emptyRect);

                if (c.DateStart.Between(date, date.AddDays(+1)))
                    start = Utility.ConvertHoursToPixels(width, c.DateStart);
                else
                    start = 0;
                if (c.DateEnd < date.AddDays(+1))
                    end = Utility.ConvertHoursToPixels(width, c.DateEnd);
                else
                    end = width;
                drawWidth = end - start;
                DrawMuhurtaColoredRectangle(e, pen, font, textBrush, (posX + start), posY, width, drawWidth, height, c, date);

                string text = c.GetShortName(lang);
                textSize = TextRenderer.MeasureText(text, font);
                heightPadding = (height - textSize.Height) / 2;
                e.Graphics.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);

                posY += height;
            }
            if (currDay.Date.DayOfWeek == DayOfWeek.Wednesday)
            {
                emptyRect = new Rectangle(posX, posY, width, height);
                string text = "АМ " + Utility.GetLocalizedText("not formed", lang);
                textSize = TextRenderer.MeasureText(text, font);
                heightPadding = (height - textSize.Height) / 2;
                e.Graphics.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                e.Graphics.DrawRectangle(pen, emptyRect);
            }
        }

        private void DrawMuhurtaColoredRectangle(PaintEventArgs e, Pen pen, Font font, SolidBrush textBrush, float posX, int posY, float dayWidth, float width, int height, Calendar c, DateTime date)
        {
            int heightPadding = 0;
            string text = string.Empty;
            Size textSize;
            e.Graphics.FillRectangle(new SolidBrush(Utility.GetColorByColorCode(c.ColorCode)), posX, posY, width, height);
            e.Graphics.DrawRectangle(pen, posX, posY, width, height);

            if (c.DateStart > date)
                text = c.DateStart.ToString("HH:mm");
            textSize = TextRenderer.MeasureText(text, font);
            heightPadding = (height - textSize.Height) / 2;
            e.Graphics.DrawString(text, font, textBrush, posX - textSize.Width, posY + heightPadding);

            if (c.DateEnd < date.AddDays(+1))
                text = c.DateEnd.ToString("HH:mm");
            textSize = TextRenderer.MeasureText(text, font);
            heightPadding = (height - textSize.Height) / 2;
            if (textSize.Width + 1 <= (dayWidth - (posX + width)))
                e.Graphics.DrawString(text, font, textBrush, posX + width + 1, posY + heightPadding);
        }
        
        private void DrawColoredLine(PaintEventArgs e, Pen pen, Font font, SolidBrush textBrush, float posX, int posY, float width, int height, List<Calendar> cList, DateTime date)
        {
            float drawWidth = 0, usedWidth = 0;
            foreach (Calendar c in cList)
            {
                if (c == cList[cList.Count - 1])
                {
                    drawWidth = width - usedWidth;
                    DrawColoredRectangle(e, pen, font, textBrush, (posX + usedWidth), posY, drawWidth, height, c, date);
                }
                else
                {
                    drawWidth = Utility.ConvertHoursToPixels(width, c.DateEnd) - usedWidth;
                    DrawColoredRectangle(e, pen, font, textBrush, (posX + usedWidth), posY, drawWidth, height, c, date);
                    usedWidth = usedWidth + drawWidth;
                }
            }
        }

        private void DrawColoredRectangle(PaintEventArgs e, Pen pen, Font font, SolidBrush textBrush, float posX, int posY, float width, int height, Calendar c, DateTime date)
        {
            string text = string.Empty;
            Size textSize;
            e.Graphics.FillRectangle(new SolidBrush(Utility.GetColorByColorCode(c.ColorCode)), posX, posY, width, height);
            e.Graphics.DrawRectangle(pen, posX, posY, width, height);

            if (c.DateStart > date)
            {
                text = c.DateStart.ToString("HH:mm") + " " + c.GetShortName(lang);
            }
            else
            {
                text = c.GetShortName(lang);
            }

            textSize = TextRenderer.MeasureText(text, font);
            int heightPadding = (height - textSize.Height) / 2;
            if (textSize.Width + 1 <= width)
            {
                e.Graphics.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
            }
            else
            {
                if (c.DateStart > date)
                {
                    text = c.DateStart.ToString("HH:mm");
                    textSize = TextRenderer.MeasureText(text, font);
                    if (textSize.Width + 1 <= width)
                        e.Graphics.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
                }
            }
        }
        
        private void DrawJogaLines(PaintEventArgs e, Pen pen, Font font, SolidBrush textBrush, int posX, int posY, int width, int height, Day day)
        {
            bool used = false;
            if (day.DwipushkarJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.DwipushkarJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.DwipushkarJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.TripushkarJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.TripushkarJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.TripushkarJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.AmritaSiddhaJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.AmritaSiddhaJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.AmritaSiddhaJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.SarvarthaSiddhaJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.SarvarthaSiddhaJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.SarvarthaSiddhaJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.SiddhaJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.SiddhaJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.SiddhaJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.MrityuJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.MrityuJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.MrityuJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.AdhamJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.AdhamJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.AdhamJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.YamaghataJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.YamaghataJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.YamaghataJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.DagdhaJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.DagdhaJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.DagdhaJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
            if (day.UnfarobaleJogaDayList.Count > 0)
            {
                foreach (Calendar c in day.UnfarobaleJogaDayList)
                {
                    if (c.DateStart.Between(day.Date, day.Date.AddDays(+1)) || c.DateEnd.Between(day.Date, day.Date.AddDays(+1)))
                    {
                        DrawColoredJogaLine(e, pen, font, textBrush, posX, posY, width, height, day.UnfarobaleJogaDayList, day.Date);
                        used = true;
                    }
                }
                if (used)
                {
                    posY += height;
                    used = false;
                }
            }
        }

        private void DrawColoredJogaLine(PaintEventArgs e, Pen pen, Font font, SolidBrush textBrush, float posX, int posY, float width, int height, List<Calendar> jogaList, DateTime date)
        {
            float start = 0, end = 0, drawWidth = 0;

            e.Graphics.DrawRectangle(pen, posX, posY, width, height);

            foreach (Calendar c in jogaList)
            {
                if (c.DateStart.Between(date, date.AddDays(+1)))
                {
                    start = Utility.ConvertHoursToPixels(width, c.DateStart);
                }
                else
                {
                    start = 0;
                }
                if (c.DateEnd < date.AddDays(+1))
                {
                    end = Utility.ConvertHoursToPixels(width, c.DateEnd);
                }
                else
                {
                    end = width;
                }
                drawWidth = end - start;
                if (c.DateStart < date.AddDays(+1))
                {
                    DrawJogaColoredRectangle(e, pen, font, textBrush, (posX + start), posY, width, drawWidth, height, c, date);
                }
            }

            string text = jogaList[0].GetShortName(lang);
            Size textSize = TextRenderer.MeasureText(text, font);
            int heightPadding = (height - textSize.Height) / 2;
            e.Graphics.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);
        }

        private void DrawJogaColoredRectangle(PaintEventArgs e, Pen pen, Font font, SolidBrush textBrush, float posX, int posY, float dayWidth, float width, int height, Calendar yoga, DateTime date)
        {
            int heightPadding = 0;
            string text = string.Empty;
            Size textSize;
            e.Graphics.FillRectangle(new SolidBrush(Utility.GetColorByColorCode(yoga.ColorCode)), posX, posY, width, height);
            e.Graphics.DrawRectangle(pen, posX, posY, width, height);

            if (yoga.DateStart > date)
                text = yoga.DateStart.ToString("HH:mm");
            textSize = TextRenderer.MeasureText(text, font);
            heightPadding = (height - textSize.Height) / 2;
            if (textSize.Width + 1 <= width)
                e.Graphics.DrawString(text, font, textBrush, posX + 1, posY + heightPadding);

            if (yoga.DateEnd < date.AddDays(+1))
                text = yoga.DateEnd.ToString("HH:mm");
            textSize = TextRenderer.MeasureText(text, font);
            heightPadding = (height - textSize.Height) / 2;
            if (textSize.Width + 1 <= (dayWidth - (posX + width)))
                e.Graphics.DrawString(text, font, textBrush, posX + width + 1, posY + heightPadding);
        }
        
        
        

    }
}
