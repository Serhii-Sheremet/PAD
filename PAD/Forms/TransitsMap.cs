using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class TransitsMap : Form
    {
        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }

        private Profile _personInfo;
        private ELanguage _activeLang;
        private DateTime _eventDate;

        public TransitsMap()
        {
            InitializeComponent();
        }

        public TransitsMap(Profile profile, ELanguage langCode)
        {
            InitializeComponent();

            _personInfo = profile;
            _activeLang = langCode;
            _eventDate = DateTime.Now;

            maskedTextBoxDate.Text = _eventDate.ToString();
        }

        private void TransitsMap_Shown(object sender, EventArgs e)
        {
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }
        
        private void PrepareTransitMapMoon()
        {
            Bitmap canvas = new Bitmap(pictureBoxMapMoon.Width, pictureBoxMapMoon.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxMapMoon.Width - 1, height = pictureBoxMapMoon.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            // drawing content
            PrepareNatalMoonTransits(g, width, height);
            pictureBoxMapMoon.Image = canvas;
        }

        private void PrepareTransitMapLagna()
        {
            Bitmap canvas = new Bitmap(pictureBoxMapLagna.Width, pictureBoxMapLagna.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxMapLagna.Width - 1, height = pictureBoxMapLagna.Height - 1;

            // drawing doms triangles
            Rectangle rect = new Rectangle(posX, posY, width, height);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, posX, posY, posX + width, posY + height);
            g.DrawLine(pen, posX, posY + height, posX + width, posY);
            g.DrawLine(pen, posX + width / 2, posY, posX, posY + height / 2);
            g.DrawLine(pen, posX + width / 2, posY, posX + width, posY + height / 2);
            g.DrawLine(pen, posX, posY + height / 2, posX + width / 2, posY + height);
            g.DrawLine(pen, posX + width / 2, posY + height, posX + width, posY + height / 2);

            // drawing content
            PrepareLagnaTransits(g, width, height);
            pictureBoxMapLagna.Image = canvas;
        }

        private bool CheckIfAspectsActive()
        {
            bool active = false;
            if (checkBoxAll.Checked || checkBoxMoon.Checked || checkBoxSun.Checked || checkBoxVenus.Checked || checkBoxJupiter.Checked ||
                checkBoxMercury.Checked || checkBoxMars.Checked || checkBoxSaturn.Checked || checkBoxRahu.Checked)
                active = true;
            return active;
        }

        private List<DomPlanet>[] AddAspects(List<DomPlanet>[] planetsList)
        {
            for (int planetListCount = 0; planetListCount < planetsList.Length; planetListCount++)
            {
                for (int planetCount = 0; planetCount < planetsList[planetListCount].Count; planetCount++)
                {
                    if (!planetsList[planetListCount][planetCount].IsActiveAspect)
                    {
                        List<int> aspectList = GetAspectDomsListByPlanet(planetsList[planetListCount][planetCount].PlanetCode);
                        for (int aspectCount = 0; aspectCount < aspectList.Count; aspectCount++)
                        {
                            DomPlanet newDomPlanet = new DomPlanet
                            {
                                PlanetCode = planetsList[planetListCount][planetCount].PlanetCode,
                                Retro = string.Empty,
                                IsActiveAspect = true,
                                ColorCode =EColor.GRAY
                            };
                            if (planetListCount + aspectList[aspectCount] - 1 < 12)
                                planetsList[planetListCount + aspectList[aspectCount] - 1].Add(newDomPlanet);
                            else
                                planetsList[(planetListCount + aspectList[aspectCount] - 1) - 12].Add(newDomPlanet);
                        }
                    }
                }
            }
            return planetsList;
        }

        private List<int> GetAspectDomsListByPlanet(EPlanet planetCode)
        {
            List<int> aspecstList = new List<int>();
            if (checkBoxMoon.Checked && planetCode == EPlanet.MOON)
            {
                aspecstList.Add(7);
            }
            if (checkBoxSun.Checked && planetCode == EPlanet.SUN)
            {
                aspecstList.Add(7);
            }
            if (checkBoxVenus.Checked && planetCode == EPlanet.VENUS)
            {
                aspecstList.Add(7);
            }
            if (checkBoxJupiter.Checked && planetCode == EPlanet.JUPITER)
            {
                aspecstList.Add(5);
                aspecstList.Add(7);
                aspecstList.Add(9);
            }
            if (checkBoxMercury.Checked && planetCode == EPlanet.MERCURY)
            {
                aspecstList.Add(7);
            }
            if (checkBoxMars.Checked && planetCode == EPlanet.MARS)
            {
                aspecstList.Add(4);
                aspecstList.Add(7);
                aspecstList.Add(8);
            }
            if (checkBoxSaturn.Checked && planetCode == EPlanet.SATURN)
            {
                aspecstList.Add(3);
                aspecstList.Add(7);
                aspecstList.Add(10);
            }
            if ((checkBoxRahu.Checked && planetCode == EPlanet.RAHUMEAN) || (checkBoxRahu.Checked && planetCode == EPlanet.RAHUTRUE))
            {
                aspecstList.Add(5);
                aspecstList.Add(7);
                aspecstList.Add(9);
            }
            return aspecstList;
        }

        private List<DomPlanet>[] GetPlanetsListWithAspects(List<Zodiak> zList)
        {
            List<DomPlanet>[] fullList = new List<DomPlanet>[12];

            for (int i = 0; i < zList.Count; i++)
                fullList[i] = GetPlanetsListByZnak(zList[i].Id, (i + 1));

            if (CheckIfAspectsActive())
                fullList = AddAspects(fullList);

            return fullList;
        }

        private List<DomPlanet> GetPlanetsListByZnak(int zodiakId, int dom)
        {
            List<DomPlanet> planetsList = new List<DomPlanet>();
           /* for (int i = 0; i < _planetsListOfaDay.Length; i++)
            {
                DomPlanet dPlanet = GetPlanetIfCurrentZnak(_planetsListOfaDay[i], zodiakId, dom);
                if (dPlanet != null)
                    planetsList.Add(dPlanet);
            }*/
            return planetsList;
        }

        private DomPlanet GetPlanetIfCurrentZnak(List<Calendar> pList, int zodiakId, int dom)
        {
            DomPlanet planet = null;
            string planetExaltation = string.Empty;
            int currentZodiakId = CacheLoad._zodiakList.Where(i => i.Id == zodiakId).FirstOrDefault()?.Id ?? 0;
           /* for (int i = 0; i < pList.Count; i++)
            {
                
                if (pList[i].ZodiakCode == (EZodiak)currentZodiakId)
                {
                    EExaltation exalt = Utility.GetExaltationByPlanetAndZnak(pList[i].PlanetCode, (EZodiak)zodiakId);
                    if (exalt == EExaltation.EXALTATION)
                    {
                        planetExaltation = "↑";
                    }
                    else if (exalt == EExaltation.DEBILITATION)
                    {
                        planetExaltation = "↓";
                    }
                    planet = new DomPlanet {
                        PlanetCode = pList[i].PlanetCode,
                        Retro = pList[i].Retro,
                        Exaltation = planetExaltation,
                        IsActiveAspect = false,
                        ColorCode = (EColor)(CacheLoad._tranzitList.Where(p => p.PlanetId == (int)pList[i].PlanetCode && p.Dom == dom).FirstOrDefault()?.ColorId ?? 0)
                    };
                }
            }*/
            return planet;
        }

        private void PrepareTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0;
            // placing dom numbers based on natal moon
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();
            //List<Zodiak> swappedZodiakList = Utility.SwappingZodiakList(zList, Utility.GetZodiakIdFromNakshatraIdandPada(_personInfo.NakshatraMoonId, _personInfo.PadaMoon));
            SettingNumberInDom(g, posX, posY, width, height, zList);

            //Get list of planets per dom
            List<DomPlanet>[] planetsList = GetPlanetsListWithAspects(zList);

            Font textFont = new Font("Times New Roman", 11, FontStyle.Bold);
            Font aspectFont = new Font("Times New Roman", 11, FontStyle.Regular);
            Size textSize = TextRenderer.MeasureText("СоR", textFont);

            for (int i = 0; i < 12; i++)
                DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i);
        }

        private void PrepareNatalMoonTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0;
            // placing dom numbers based on natal moon
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();
            List<Zodiak> swappedZodiakList = Utility.SwappingZodiakList(zList, Utility.GetZodiakIdFromNakshatraIdandPada(CacheLoad._birthNakshatraMoonId, CacheLoad._birthPadaMoonNumber));
            SettingNumberInDom(g, posX, posY, width, height, swappedZodiakList);

            //Get list of planets per dom
            List<DomPlanet>[] planetsList = GetPlanetsListWithAspects(swappedZodiakList);

            Font textFont = new Font("Times New Roman", 11, FontStyle.Bold);
            Font aspectFont = new Font("Times New Roman", 11, FontStyle.Regular);
            Size textSize = TextRenderer.MeasureText("СоR", textFont);

            for (int i = 0; i < 12; i++)
                DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i);
        }

        private void PrepareLagnaTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0;
            // placing dom numbers based on lagna
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();
            //int lagnaId = Utility.GetZodiakIdFromNakshatraIdandPada(_personInfo.NakshatraLagnaId, _personInfo.PadaLagna);
            //List<Zodiak> swapZodiakList = Utility.SwappingZodiakList(zList, lagnaId);
            //SettingNumberInDom(g, posX, posY, width, height, swapZodiakList);

            //Get list of planets per dom
            //List<DomPlanet>[] planetsList = GetPlanetsListWithAspects(swapZodiakList);

            Font textFont = new Font("Times New Roman", 11, FontStyle.Bold);
            Font aspectFont = new Font("Times New Roman", 11, FontStyle.Regular);
            Size textSize = TextRenderer.MeasureText("СоR", textFont);
            
            //for (int i = 0; i < 12; i++)
            //    DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i);
        }

        private void DrawDomWithPlanets(Graphics g, int width, int height, Font textFont, Font aspectFont, int textHeight, List<DomPlanet>[] planetsList, int index)
        {
            switch (index)
            {
                case 0:
                    DrawDom1(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 1:
                    DrawDom2(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 2:
                    DrawDom3(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 3:
                    DrawDom4(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 4:
                    DrawDom5(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 5:
                    DrawDom6(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 6:
                    DrawDom7(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 7:
                    DrawDom8(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 8:
                    DrawDom9(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 9:
                    DrawDom10(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 10:
                    DrawDom11(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;

                case 11:
                    DrawDom12(g, width, height, textFont, aspectFont, textHeight, planetsList[index]);
                    break;
            }
        }

        private string PreparePlanetName(DomPlanet dp)
        {
            string type = string.Empty, exaltation = string.Empty;
            if (dp.Retro.Equals("R") && dp.PlanetCode != EPlanet.RAHUMEAN && dp.PlanetCode != EPlanet.KETUMEAN && dp.PlanetCode != EPlanet.RAHUTRUE && dp.PlanetCode != EPlanet.KETUTRUE)
            {
                type = dp.Retro;
            }
            else if (!dp.Retro.Equals("R") && dp.PlanetCode != EPlanet.RAHUMEAN && dp.PlanetCode != EPlanet.KETUMEAN && dp.PlanetCode != EPlanet.RAHUTRUE && dp.PlanetCode != EPlanet.KETUTRUE)
            {
                exaltation = dp.Exaltation;
            }
            return Utility.GetLocalizedPlanetNameByCode(dp.PlanetCode, _activeLang).Substring(0, 2) + type + exaltation;
        }

        private void SettingNumberInDom(Graphics g, int posX, int posY, int width, int height, List<Zodiak> zList)
        {
            Font textFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            SolidBrush textBrush = new SolidBrush(Color.Black);

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            int[] domZnakPositionX = GetDomNumbersPositionXCoordinate(posY, width);
            int[] domZnakPositionY = GetDomNumbersPositionYCoordinate(posY, height);
            for (int i = 0; i < zList.Count; i++)
            {
                g.DrawString(zList[i].Id.ToString(), textFont, textBrush, domZnakPositionX[i], domZnakPositionY[i], sf);
            }
        }

        int[] GetDomNumbersPositionXCoordinate(int posX, int width)
        {
            int[] listPositionX = new int[12] {  posX + (width / 2),
                                                 posX + (width / 4),
                                                 posX + (width / 4 - 10),
                                                 posX + (width / 2 - 10),
                                                 posX + (width / 4 - 10),
                                                 posX + (width / 4),
                                                 posX + (width / 2),
                                                 posX + ((width - width / 4)),
                                                 posX + ((width - width / 4) + 10),
                                                 posX + ((width / 2) + 10),
                                                 posX + ((width - width / 4) + 10),
                                                 posX + ((width - width / 4)) };
            return listPositionX;
        }

        int[] GetDomNumbersPositionYCoordinate(int posY, int height)
        {
            int[] listPositionY = new int[12] {  posY + (height / 2 - 10),
                                                 posY + (height / 4 - 10),
                                                 posY + (height / 4),
                                                 posY + (height / 2),
                                                 posY + ((height - height / 4)),
                                                 posY + (height - height / 4) + 10,
                                                 posY + (height / 2) + 10,
                                                 posY + (height - height / 4) + 10,
                                                 posY + ((height - height / 4)),
                                                 posY + (height / 2),
                                                 posY + (height / 4),
                                                 posY + (height / 4 - 10) };
            return listPositionY;
        }

        private void DrawDom1(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 - 40;
            int topLineWidth = (width / 2 * (height / 4 - textHeight) / (height / 4)) - 40;

            bool mainLeft = false, mainRight = false, topLeft = false, topRight = false, bottomLeft = false, bottomRight = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedTopWidthLeft = 0, usedTopWidthRight = 0;
            int usedBottomWidthLeft = 0, usedBottomWidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if(!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedMainWidthRight), posY + (height / 4 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedMainWidthRight), posY + (height / 4 - textSize.Height / 2));
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedTopWidthLeft + usedTopWidthRight + textSize.Width <= topLineWidth)
                    {
                        if (!topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedTopWidthRight), posY + (height / 4 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedTopWidthRight), posY + (height / 4 - textSize.Height - 8));
                            usedTopWidthRight += textSize.Width;
                            topLeft = true;
                            topRight = false;
                        }
                    }
                    else
                    {
                        if (!bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height / 4 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height / 4 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedBottomWidthRight), posY + (height / 4 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedBottomWidthRight), posY + (height / 4 + textSize.Height - 8));
                            usedBottomWidthRight += textSize.Width;
                            bottomLeft = true;
                            bottomRight = false;
                        }
                    }
                }
            }
        }

        private void DrawDom2(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 * (height / 4 - 6) / (height / 4) - 40;
            int Bottom1LineWidth = width / 2 * (height / 4 - textHeight - 6) / (height / 4) - 40;
            int Bottom2LineWidth = width / 2 * (height / 4 * textHeight - 2) / (height / 4);

            bool mainLeft = false, mainRight = false, bottom1Left = false, bottom1Right = false, bottom2Left = false, bottom2Right = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedBottom1WidthLeft = 0, usedBottom1WidthRight = 0;
            int usedBottom2WidthLeft = 0, usedBottom2WidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + 4);
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + 4);
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedBottom1WidthLeft + usedBottom1WidthRight + textSize.Width <= Bottom1LineWidth)
                    {
                        if (!bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            usedBottom1WidthRight += textSize.Width;
                            bottom1Left = true;
                            bottom1Right = false;
                        }
                    }
                    else
                    {
                        if (!bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            usedBottom2WidthRight += textSize.Width;
                            bottom2Left = true;
                            bottom2Right = false;
                        }
                    }
                }
            }
        }

        private void DrawDom3(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidth + textSize.Width <= mainLineWidth)
                {
                    if (!displayPlanetsList[i].IsActiveAspect)
                        g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedMainWidth + 4, posY + (height / 4 - textSize.Height / 2));
                    else
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedMainWidth + 4, posY + (height / 4 - textSize.Height / 2));
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop1Width + 4, posY + (height / 4 - textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop1Width + 4, posY + (height / 4 - textSize.Height - 8));
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom1Width + 4, posY + (height / 4 + textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom1Width + 4, posY + (height / 4 + textSize.Height - 4));
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop2Width + 4, posY + (height / 4 - 2 * textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop2Width + 4, posY + (height / 4 - 2 * textSize.Height - 8));
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom2Width + 4, posY + (height / 4 + 2 * textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom2Width + 4, posY + (height / 4 + 2 * textSize.Height - 4));
                        usedBottom2Width += textSize.Width;
                    }
                }
            }
        }

        private void DrawDom4(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 - 40;
            int topLineWidth = (width / 2 * (height / 4 - textHeight) / (height / 4)) - 40;

            bool mainLeft = false, mainRight = false, topLeft = false, topRight = false, bottomLeft = false, bottomRight = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedTopWidthLeft = 0, usedTopWidthRight = 0;
            int usedBottomWidthLeft = 0, usedBottomWidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedTopWidthLeft + usedTopWidthRight + textSize.Width <= topLineWidth)
                    {
                        if (!topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            usedTopWidthRight += textSize.Width;
                            topLeft = true;
                            topRight = false;
                        }
                    }
                    else
                    {
                        if (!bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            usedBottomWidthRight += textSize.Width;
                            bottomLeft = true;
                            bottomRight = false;
                        }
                    }
                }
            }
        }

        private void DrawDom5(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidth + textSize.Width <= mainLineWidth)
                {
                    if (!displayPlanetsList[i].IsActiveAspect)
                        g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedMainWidth + 4, posY + (height - height / 4 - textSize.Height / 2));
                    else
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedMainWidth + 4, posY + (height - height / 4 - textSize.Height / 2));
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop1Width + 4, posY + (height - height / 4 - textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop1Width + 4, posY + (height - height / 4 - textSize.Height - 8));
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom1Width + 4, posY + (height - height / 4 + textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom1Width + 4, posY + (height - height / 4 + textSize.Height - 4));
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop2Width + 4, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop2Width + 4, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom2Width + 4, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom2Width + 4, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        usedBottom2Width += textSize.Width;
                    }
                }
            }
        }

        private void DrawDom6(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 * (height / 4 - 6) / (height / 4) - 40;
            int Bottom1LineWidth = width / 2 * (height / 4 - textHeight - 6) / (height / 4) - 40;
            int Bottom2LineWidth = width / 2 * (height / 4 * textHeight - 2) / (height / 4);

            bool mainLeft = false, mainRight = false, bottom1Left = false, bottom1Right = false, bottom2Left = false, bottom2Right = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedBottom1WidthLeft = 0, usedBottom1WidthRight = 0;
            int usedBottom2WidthLeft = 0, usedBottom2WidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedBottom1WidthLeft + usedBottom1WidthRight + textSize.Width <= Bottom1LineWidth)
                    {
                        if (!bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            usedBottom1WidthRight += textSize.Width;
                            bottom1Left = true;
                            bottom1Right = false;
                        }
                    }
                    else
                    {
                        if (!bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            usedBottom2WidthRight += textSize.Width;
                            bottom2Left = true;
                            bottom2Right = false;
                        }
                    }
                }
            }
        }

        private void DrawDom7(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 - 40;
            int topLineWidth = (width / 2 * (height / 4 - textHeight) / (height / 4)) - 40;

            bool mainLeft = false, mainRight = false, topLeft = false, topRight = false, bottomLeft = false, bottomRight = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedTopWidthLeft = 0, usedTopWidthRight = 0;
            int usedBottomWidthLeft = 0, usedBottomWidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedMainWidthRight), posY + (height - height / 4 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedMainWidthRight), posY + (height - height / 4 - textSize.Height / 2));
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedTopWidthLeft + usedTopWidthRight + textSize.Width <= topLineWidth)
                    {
                        if (!topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedTopWidthRight), posY + (height - height / 4 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedTopWidthRight), posY + (height - height / 4 - textSize.Height - 8));
                            usedTopWidthRight += textSize.Width;
                            topLeft = true;
                            topRight = false;
                        }
                    }
                    else
                    {
                        if (!bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height - height / 4 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height - height / 4 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedBottomWidthRight), posY + (height - height / 4 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedBottomWidthRight), posY + (height - height / 4 + textSize.Height - 8));
                            usedBottomWidthRight += textSize.Width;
                            bottomLeft = true;
                            bottomRight = false;
                        }
                    }
                }
            }
        }

        private void DrawDom8(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 * (height / 4 - 6) / (height / 4) - 40;
            int Bottom1LineWidth = width / 2 * (height / 4 - textHeight - 6) / (height / 4) - 40;
            int Bottom2LineWidth = width / 2 * (height / 4 * textHeight - 2) / (height / 4);

            bool mainLeft = false, mainRight = false, bottom1Left = false, bottom1Right = false, bottom2Left = false, bottom2Right = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedBottom1WidthLeft = 0, usedBottom1WidthRight = 0;
            int usedBottom2WidthLeft = 0, usedBottom2WidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedBottom1WidthLeft + usedBottom1WidthRight + textSize.Width <= Bottom1LineWidth)
                    {
                        if (!bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            usedBottom1WidthRight += textSize.Width;
                            bottom1Left = true;
                            bottom1Right = false;
                        }
                    }
                    else
                    {
                        if (!bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            usedBottom2WidthRight += textSize.Width;
                            bottom2Left = true;
                            bottom2Right = false;
                        }
                    }
                }
            }
        }

        private void DrawDom9(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidth + textSize.Width <= mainLineWidth)
                {
                    if (!displayPlanetsList[i].IsActiveAspect)
                        g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedMainWidth - textSize.Width, posY + (height - height / 4 - textSize.Height / 2));
                    else
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedMainWidth - textSize.Width, posY + (height - height / 4 - textSize.Height / 2));
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop1Width - textSize.Width, posY + (height - height / 4 - textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop1Width - textSize.Width, posY + (height - height / 4 - textSize.Height - 8));
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom1Width - textSize.Width, posY + (height - height / 4 + textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom1Width - textSize.Width, posY + (height - height / 4 + textSize.Height - 4));
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop2Width - textSize.Width, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop2Width - textSize.Width, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom2Width - textSize.Width, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom2Width - textSize.Width, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        usedBottom2Width += textSize.Width;
                    }
                }
            }
        }

        private void DrawDom10(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 - 40;
            int topLineWidth = (width / 2 * (height / 4 - textHeight) / (height / 4)) - 40;

            bool mainLeft = false, mainRight = false, topLeft = false, topRight = false, bottomLeft = false, bottomRight = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedTopWidthLeft = 0, usedTopWidthRight = 0;
            int usedBottomWidthLeft = 0, usedBottomWidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedTopWidthLeft + usedTopWidthRight + textSize.Width <= topLineWidth)
                    {
                        if (!topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            usedTopWidthRight += textSize.Width;
                            topLeft = true;
                            topRight = false;
                        }
                    }
                    else
                    {
                        if (!bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            usedBottomWidthRight += textSize.Width;
                            bottomLeft = true;
                            bottomRight = false;
                        }
                    }
                }
            }
        }

        private void DrawDom11(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidth + textSize.Width <= mainLineWidth)
                {
                    if (!displayPlanetsList[i].IsActiveAspect)
                        g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedMainWidth - textSize.Width, posY + (height / 4 - textSize.Height / 2));
                    else
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedMainWidth - textSize.Width, posY + (height / 4 - textSize.Height / 2));
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop1Width - textSize.Width, posY + (height / 4 - textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop1Width - textSize.Width, posY + (height / 4 - textSize.Height - 8));
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom1Width - textSize.Width, posY + (height / 4 + textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom1Width - textSize.Width, posY + (height / 4 + textSize.Height - 4));
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop2Width - textSize.Width, posY + (height / 4 - 2 * textSize.Height - 8));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop2Width - textSize.Width, posY + (height / 4 - 2 * textSize.Height - 8));
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom2Width - textSize.Width, posY + (height / 4 + 2 * textSize.Height - 4));
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom2Width - textSize.Width, posY + (height / 4 + 2 * textSize.Height - 4));
                        usedBottom2Width += textSize.Width;
                    }
                }
            }
        }

        private void DrawDom12(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 2 * (height / 4 - 6) / (height / 4) - 40;
            int Bottom1LineWidth = width / 2 * (height / 4 - textHeight - 6) / (height / 4) - 40;
            int Bottom2LineWidth = width / 2 * (height / 4 * textHeight - 2) / (height / 4);

            bool mainLeft = false, mainRight = false, bottom1Left = false, bottom1Right = false, bottom2Left = false, bottom2Right = false;
            int usedMainWidthLeft = 0, usedMainWidthRight = 0;
            int usedBottom1WidthLeft = 0, usedBottom1WidthRight = 0;
            int usedBottom2WidthLeft = 0, usedBottom2WidthRight = 0;

            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i]);
                if (!displayPlanetsList[i].IsActiveAspect)
                    textSize = TextRenderer.MeasureText(text, font);
                else
                    textSize = TextRenderer.MeasureText(text, aspectFont);

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + 4);
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                            g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + 4);
                        else
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + 4);
                        usedMainWidthRight += textSize.Width;
                        mainLeft = true;
                        mainRight = false;
                    }
                }
                else
                {
                    if (usedBottom1WidthLeft + usedBottom1WidthRight + textSize.Width <= Bottom1LineWidth)
                    {
                        if (!bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            usedBottom1WidthRight += textSize.Width;
                            bottom1Left = true;
                            bottom1Right = false;
                        }
                    }
                    else
                    {
                        if (!bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                                g.DrawString(text, font, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            else
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            usedBottom2WidthRight += textSize.Width;
                            bottom2Left = true;
                            bottom2Right = false;
                        }
                    }
                }
            }
        }

        private void CheckAllAspectBoxes()
        {
            checkBoxMoon.Checked = true;
            checkBoxSun.Checked = true;
            checkBoxVenus.Checked = true;
            checkBoxJupiter.Checked = true;
            checkBoxMercury.Checked = true;
            checkBoxMars.Checked = true;
            checkBoxSaturn.Checked = true;
            checkBoxRahu.Checked = true;
        }

        private void UncheckAllAspectBoxes()
        {
            checkBoxMoon.Checked = false;
            checkBoxSun.Checked = false;
            checkBoxVenus.Checked = false;
            checkBoxJupiter.Checked = false;
            checkBoxMercury.Checked = false;
            checkBoxMars.Checked = false;
            checkBoxSaturn.Checked = false;
            checkBoxRahu.Checked = false;
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                CheckAllAspectBoxes();
                PrepareTransitMapMoon();
                PrepareTransitMapLagna();
            }
            else
            {
                UncheckAllAspectBoxes();
                PrepareTransitMapMoon();
                PrepareTransitMapLagna();
            }
        }

        private void checkBoxMoon_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if(CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private void checkBoxSun_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private void checkBoxVenera_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private void checkBoxJupiter_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private void checkBoxMercury_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private void checkBoxMars_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private void checkBoxSaturn_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private void checkBoxRahu_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMapMoon();
            PrepareTransitMapLagna();
        }

        private bool CheckIfAllPlanetCheckBoxChecked()
        {
            bool allChecked = false;
            if (checkBoxMoon.Checked && checkBoxSun.Checked && checkBoxVenus.Checked && checkBoxJupiter.Checked &&
                checkBoxMercury.Checked && checkBoxMars.Checked && checkBoxSaturn.Checked && checkBoxRahu.Checked)
                allChecked = true;
            return allChecked;
        }

        private bool CheckIfAllPlanetCheckBoxUnchecked()
        {
            bool allUnchecked = false;
            if (!checkBoxMoon.Checked && !checkBoxSun.Checked && !checkBoxVenus.Checked && !checkBoxJupiter.Checked &&
                !checkBoxMercury.Checked && !checkBoxMars.Checked && !checkBoxSaturn.Checked && !checkBoxRahu.Checked)
                allUnchecked = true;
            return allUnchecked;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CalculatePlanetsPosition(DateTime date)
        {
            double latitude, longitude;
            string timeZone = string.Empty;
            if (Utility.GetGeoCoordinateByLocationId(_personInfo.PlaceOfLivingId, out latitude, out longitude))
            {
                timeZone = Utility.GetTimeZoneIdByGeoCoordinates(latitude, longitude);
                EpheCalculation eCalc = new EpheCalculation();

                PlanetData moonData = eCalc.CalculatePlanetData_London(EpheConstants.SE_MOON, date);
                PlanetData sunData = eCalc.CalculatePlanetData_London(EpheConstants.SE_SUN, date);
                PlanetData mercuryData = eCalc.CalculatePlanetData_London(EpheConstants.SE_MERCURY, date);
                PlanetData venusData = eCalc.CalculatePlanetData_London(EpheConstants.SE_VENUS, date);
                PlanetData marsData = eCalc.CalculatePlanetData_London(EpheConstants.SE_MARS, date);
                PlanetData jupiterData = eCalc.CalculatePlanetData_London(EpheConstants.SE_JUPITER, date);
                PlanetData saturnData = eCalc.CalculatePlanetData_London(EpheConstants.SE_SATURN, date);
                PlanetData rahuMeanData = eCalc.CalculatePlanetData_London(EpheConstants.SE_MEAN_NODE, date);
                PlanetData rahuTrueData = eCalc.CalculatePlanetData_London(EpheConstants.SE_TRUE_NODE, date);
                PlanetData ketuMeanData = eCalc.CalculateKetu(rahuMeanData);
                PlanetData ketuTrueData = eCalc.CalculateKetu(rahuTrueData);

            }

        }

        
    }
}
