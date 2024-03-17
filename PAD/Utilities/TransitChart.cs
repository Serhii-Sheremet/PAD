using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PAD
{
    public static class TransitChart
    {
        public static void SettingNumberInDom(Graphics g, int posX, int posY, int width, int height, List<Zodiak> zList)
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

        static int[] GetDomNumbersPositionXCoordinate(int posX, int width)
        {
            int[] listPositionX = new int[12] {  posX + (width / 2),
                                                 posX + (width / 4),
                                                 posX + (width / 4 - 12),
                                                 posX + (width / 2 - 12),
                                                 posX + (width / 4 - 12),
                                                 posX + (width / 4),
                                                 posX + (width / 2),
                                                 posX + ((width - width / 4)),
                                                 posX + ((width - width / 4) + 12),
                                                 posX + ((width / 2) + 12),
                                                 posX + ((width - width / 4) + 12),
                                                 posX + ((width - width / 4)) };
            return listPositionX;
        }

        static int[] GetDomNumbersPositionYCoordinate(int posY, int height)
        {
            int[] listPositionY = new int[12] {  posY + (height / 2 - 12),
                                                 posY + (height / 4 - 12),
                                                 posY + (height / 4),
                                                 posY + (height / 2),
                                                 posY + ((height - height / 4)),
                                                 posY + (height - height / 4) + 12,
                                                 posY + (height / 2) + 12,
                                                 posY + (height - height / 4) + 12,
                                                 posY + ((height - height / 4)),
                                                 posY + (height / 2),
                                                 posY + (height / 4),
                                                 posY + (height / 4 - 12) };
            return listPositionY;
        }

        public static List<DomPlanet> GetPlanetsListByZnak(List<PlanetData> pdBirthList, List<PlanetData> pdNatalList, int zodiakId, int dom)
        {
            List<DomPlanet> planetsList = new List<DomPlanet>();
            EAppSetting nodeSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;

            List<PlanetData> pdTunedBirthList = Utility.ClonePlanetDataList(pdBirthList);
            List<PlanetData> pdTunedNatalList = Utility.ClonePlanetDataList(pdNatalList);

            if (nodeSetting == EAppSetting.NODEMEAN)
            {
                var planetToRemove = new[] { 10, 11 };
                pdTunedBirthList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
                pdTunedNatalList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
            }
            if (nodeSetting == EAppSetting.NODETRUE)
            {
                var planetToRemove = new[] { 8, 9 };
                pdTunedBirthList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
                pdTunedNatalList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
            }

            for (int i = 0; i < pdTunedBirthList.Count; i++)
            {
                DomPlanet dPlanet = GetPlanetIfCurrentZnak(pdTunedBirthList[i], ETransitType.TRANSITBIRTH, zodiakId, dom);
                if (dPlanet != null)
                {
                    planetsList.Add(dPlanet);
                }
            }
            for (int i = 0; i < pdTunedNatalList.Count; i++)
            {
                DomPlanet dPlanet = GetPlanetIfCurrentZnak(pdTunedNatalList[i], ETransitType.TRANSITNATAL, zodiakId, dom);
                if (dPlanet != null)
                {
                    planetsList.Add(dPlanet);
                }
            }

            List<DomPlanet> sortedList = planetsList.OrderBy(i => i.Longitude).ToList();
            return sortedList;
        }

        public static List<DomPlanet> GetGeneralPlanetsListByZnak(List<PlanetData> pdList, int zodiakId, int dom)
        {
            List<DomPlanet> planetsList = new List<DomPlanet>();

            // remove nodes from list based on config
            EAppSetting nodeSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;

            List<PlanetData> pdTunedList = Utility.ClonePlanetDataList(pdList);
            if (nodeSetting == EAppSetting.NODEMEAN)
            {
                var planetToRemove = new[] { 10, 11 };
                pdTunedList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
            }
            if (nodeSetting == EAppSetting.NODETRUE)
            {
                var planetToRemove = new[] { 8, 9 };
                pdTunedList.RemoveAll(i => planetToRemove.Contains(i.PlanetId));
            }

            for (int i = 0; i < pdTunedList.Count; i++)
            {
                DomPlanet dPlanet = GetPlanetIfCurrentZnak(pdTunedList[i], ETransitType.TRANSITGENERAL, zodiakId, dom);
                if (dPlanet != null)
                {
                    planetsList.Add(dPlanet);
                }
            }
            return planetsList;
        }

        private static DomPlanet GetPlanetIfCurrentZnak(PlanetData pd, ETransitType transitType, int zodiakId, int dom)
        {
            DomPlanet planet = null;
            EColor pColor = EColor.BLACK;
            string planetExaltation = string.Empty;
            int currentZodiakId = CacheLoad._zodiakList.Where(i => i.Id == zodiakId).FirstOrDefault()?.Id ?? 0;

            int planetId = pd.PlanetId;
            if (planetId == 10)
            {
                planetId = 8;
            }
            if (planetId == 11)
            {
                planetId = 9;
            }

            if (pd.ZodiakId == currentZodiakId)
            {
                EExaltation exalt = Utility.GetExaltationByPlanetAndZnak((EPlanet)planetId, (EZodiak)zodiakId);
                if (exalt == EExaltation.EXALTATION)
                {
                    planetExaltation = "↑";
                }
                else if (exalt == EExaltation.DEBILITATION)
                {
                    planetExaltation = "↓";
                }

                if (transitType != ETransitType.TRANSITGENERAL && transitType != ETransitType.TRANSITBIRTH)
                {
                    pColor = (EColor)(CacheLoad._tranzitList.Where(p => p.PlanetId == planetId && p.Dom == dom).FirstOrDefault()?.ColorId ?? 0);
                }

                planet = new DomPlanet
                {
                    PlanetCode = (EPlanet)pd.PlanetId,
                    Longitude = pd.Longitude,
                    TransitType = transitType,
                    Retro = pd.Retro,
                    Exaltation = planetExaltation,
                    IsActiveAspect = false,
                    ColorCode = pColor
                };
            }
            return planet;
        }

        public static void DrawDomWithPlanets(Graphics g, int width, int height, Font textFont, Font aspectFont, int textHeight, List<DomPlanet>[] planetsList, int index, ELanguage lang)
        {
            switch (index)
            {
                case 0:
                    DrawDom1(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 1:
                    DrawDom2(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 2:
                    DrawDom3(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 3:
                    DrawDom4(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 4:
                    DrawDom5(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 5:
                    DrawDom6(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 6:
                    DrawDom7(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 7:
                    DrawDom8(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 8:
                    DrawDom9(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 9:
                    DrawDom10(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 10:
                    DrawDom11(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;

                case 11:
                    DrawDom12(g, width, height, textFont, aspectFont, textHeight, planetsList[index], lang);
                    break;
            }
        }

        private static string PreparePlanetName(DomPlanet dp, ELanguage lang)
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
            return Utility.GetLocalizedPlanetNameByCode(dp.PlanetCode, lang).Substring(0, 2) + type + exaltation;
        }

        private static void DrawDom1(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
                if (!displayPlanetsList[i].IsActiveAspect)
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

        private static void DrawDom2(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom3(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom4(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom5(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom6(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom7(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom8(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom9(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom10(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom11(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
        {
            Size textSize;
            int posX = 0, posY = 0;
            int mainLineWidth = width / 4 - 20;
            int top1LineWidth = width / 4 * (height / 4 - textHeight) / (height / 4);
            int top2LineWidth = width / 4 * (height / 4 - 2 * textHeight) / (height / 4);

            int usedMainWidth = 0, usedTop1Width = 0, usedTop2Width = 0, usedBottom1Width = 0, usedBottom2Width = 0;
            for (int i = 0; i < displayPlanetsList.Count; i++)
            {
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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

        private static void DrawDom12(Graphics g, int width, int height, Font font, Font aspectFont, int textHeight, List<DomPlanet> displayPlanetsList, ELanguage lang)
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
                string text = PreparePlanetName(displayPlanetsList[i], lang);
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










    }
}
