using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PAD
{
    public partial class ProfileForm : Form
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

        private List<Profile> _proList;
        private ELanguage _activeLang;
        private bool _isNew;
        private bool _isModify;

        private List<NakshatraDescription> _ndList;
        private List<ZodiakDescription> _zdList;
        private Location _selectedBirthLocation;
        private Location _selectedLivingLocation;

        private List<PlanetData> _pdBirthList;
        private double _birthAscendant;

        private List<PlanetData> _pdList;
        private double _ascendant;
        private int _lagnaId;
        private int _nakshatralagnaId;
        private int _padaLagna;

        private Profile _profile;
        public Profile SelectedProfile
        {
            get { return _profile; }
            set { _profile = value; }
        }

        private bool _isChosen;
        public bool IsChosen
        {
            get { return _isChosen; }
            set { _isChosen = value; }
        }

        private bool _singleMode;
        public bool SingleMode
        {
            get { return _singleMode; }
            set { _singleMode = value; }
        }

        public ProfileForm()
        {
            InitializeComponent();
        }

        public ProfileForm(List<Profile> pList, ELanguage activeLang, bool sMode)
        {
            InitializeComponent();

            _proList = pList;
            _activeLang = activeLang;
            _isNew = false;
            _isModify = false;
            SingleMode = sMode;

            _ndList = CacheLoad._nakshatraDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();
            _zdList = CacheLoad._zodiakDescList.Where(i => i.LanguageCode.Equals(_activeLang.ToString())).ToList();

            _ascendant = 0.00;
            _lagnaId = -1;
            _nakshatralagnaId = -1;
            _padaLagna = -1;

            IsChosen = false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            IsChosen = false;
            Close();
        }

        private void ProfileForm_Shown(object sender, EventArgs e)
        {
            Utility.LocalizeForm(this, _activeLang);

            if (SingleMode)
            {
                buttonChoose.Visible = false;
            }

            FillProfileListViewByData(_proList);
      
            PrepareDataGridInfo(_activeLang);
            listViewProfile.Items[0].Selected = true;

            toolStripButtonAdd.Enabled = true;
            textBoxSearch.Focus();
        }

        private void PrepareTransitMap()
        {
            Bitmap canvas = new Bitmap(pictureBoxMap.Width, pictureBoxMap.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.LightGoldenrodYellow);

            int posX = 0, posY = 0, width = pictureBoxMap.Width - 1, height = pictureBoxMap.Height - 1;

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
            pictureBoxMap.Image = canvas;
        }

        private void PrepareLagnaTransits(Graphics g, int width, int height)
        {
            int posX = 0, posY = 0, lagnaId = -1, nakshatraId = -1, pada = -1;
            // placing dom numbers based on lagna
            List<Zodiak> zList = CacheLoad._zodiakList.ToList();

            if (SelectedProfile != null)
            {
                Location birthLocation = CacheLoad._locationList.Where(i => i.Id == SelectedProfile.PlaceOfBirthId).FirstOrDefault();
                double latitude, longitude;
                if (double.TryParse(birthLocation.Latitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                    double.TryParse(birthLocation.Longitude, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
                {
                    _pdBirthList = Utility.CalculatePlanetsPositionForDate(SelectedProfile.DateOfBirth, latitude, longitude);
                    _birthAscendant = Utility.CalculateAscendantForDate(SelectedProfile.DateOfBirth, latitude, longitude, 0, 'O');

                    nakshatraId = Utility.GetNakshatraIdFromDegree(_birthAscendant);
                    pada = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_birthAscendant));
                    lagnaId = Utility.GetZodiakIdFromDegree(_birthAscendant);
                }
            }
            else
            {
                if (_ascendant != 0.00)
                {
                    nakshatraId = _nakshatralagnaId;
                    pada = _padaLagna;
                    lagnaId = _lagnaId;
                }
            }

            List<Zodiak> swapZodiakList = Utility.SwappingZodiakList(zList, lagnaId);
            SettingNumberInDom(g, posX, posY, width, height, swapZodiakList);

            //Get list of planets per dom
            List<DomPlanet>[] planetsList = GetPlanetsListWithAspects(swapZodiakList);

            Font textFont = new Font("Times New Roman", 14, FontStyle.Regular);
            Font aspectFont = new Font("Times New Roman", 14, FontStyle.Regular);
            Size textSize = TextRenderer.MeasureText("СоR", textFont);

            for (int i = 0; i < 12; i++)
            {
                DrawDomWithPlanets(g, width, height, textFont, aspectFont, textSize.Height, planetsList, i);
            }
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

        int[] GetDomNumbersPositionXCoordinate(int posX, int width)
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

        int[] GetDomNumbersPositionYCoordinate(int posY, int height)
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

        private List<DomPlanet>[] GetPlanetsListWithAspects(List<Zodiak> zList)
        {
            List<DomPlanet>[] fullList = new List<DomPlanet>[12];

            for (int i = 0; i < zList.Count; i++)
            {
                fullList[i] = GetPlanetsListByZnak(zList[i].Id, (i + 1));
            }

            if (CheckIfAspectsActive())
            {
                fullList = AddAspects(fullList);
            }

            return fullList;
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
                                ColorCode = EColor.GRAY
                            };
                            if (planetListCount + aspectList[aspectCount] - 1 < 12)
                            {
                                planetsList[planetListCount + aspectList[aspectCount] - 1].Add(newDomPlanet);
                            }
                            else
                            {
                                planetsList[(planetListCount + aspectList[aspectCount] - 1) - 12].Add(newDomPlanet);
                            }
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

        private List<DomPlanet> GetPlanetsListByZnak(int zodiakId, int dom)
        {
            List<DomPlanet> planetsList = new List<DomPlanet>();

            // remove nodes from list based on config
            EAppSetting nodeSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;
            List<PlanetData> pdTunedList;
            if (SelectedProfile != null)
            {
                pdTunedList = Utility.ClonePlanetDataList(_pdBirthList);
            }
            else
            {
                pdTunedList = Utility.ClonePlanetDataList(_pdList);
            }
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
                DomPlanet dPlanet = GetPlanetIfCurrentZnak(pdTunedList[i], zodiakId, dom);
                if (dPlanet != null)
                {
                    planetsList.Add(dPlanet);
                }
            }
            return planetsList;
        }

        private DomPlanet GetPlanetIfCurrentZnak(PlanetData pd, int zodiakId, int dom)
        {
            DomPlanet planet = null;
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

                planet = new DomPlanet {
                    PlanetCode = (EPlanet)pd.PlanetId,
                    Retro = pd.Retro,
                    Exaltation = planetExaltation,
                    IsActiveAspect = false,
                    ColorCode = (EColor)(CacheLoad._tranzitList.Where(p => p.PlanetId == planetId && p.Dom == dom).FirstOrDefault()?.ColorId ?? 0)
                };
            }
            return planet;
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
                if (!displayPlanetsList[i].IsActiveAspect)
                {
                    textSize = TextRenderer.MeasureText(text, font);
                }
                else
                {
                    textSize = TextRenderer.MeasureText(text, aspectFont);
                }

                if (usedMainWidthLeft + usedMainWidthRight + textSize.Width <= mainLineWidth)
                {
                    if (!mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 + usedMainWidthRight), posY + (height / 4 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedMainWidthRight), posY + (height / 4 - textSize.Height / 2));
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height / 4 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 + usedTopWidthRight), posY + (height / 4 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedTopWidthRight), posY + (height / 4 - textSize.Height - 8));
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height / 4 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height / 4 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height / 4 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 + usedBottomWidthRight), posY + (height / 4 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedBottomWidthRight), posY + (height / 4 + textSize.Height - 8));
                            }
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
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + 4);
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedMainWidthRight), posY + 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + 4);
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            }
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            }
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            }
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            }
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            }
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
                    {
                        g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedMainWidth + 4, posY + (height / 4 - textSize.Height / 2));
                    }
                    else
                    {
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedMainWidth + 4, posY + (height / 4 - textSize.Height / 2));
                    }
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedTop1Width + 4, posY + (height / 4 - textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop1Width + 4, posY + (height / 4 - textSize.Height - 8));
                        }
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedBottom1Width + 4, posY + (height / 4 + textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom1Width + 4, posY + (height / 4 + textSize.Height - 4));
                        }
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedTop2Width + 4, posY + (height / 4 - 2 * textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop2Width + 4, posY + (height / 4 - 2 * textSize.Height - 8));
                        }
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedBottom2Width + 4, posY + (height / 4 + 2 * textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom2Width + 4, posY + (height / 4 + 2 * textSize.Height - 4));
                        }
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
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            }
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
                    {
                        g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedMainWidth + 4, posY + (height - height / 4 - textSize.Height / 2));
                    }
                    else
                    {
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedMainWidth + 4, posY + (height - height / 4 - textSize.Height / 2));
                    }
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedTop1Width + 4, posY + (height - height / 4 - textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop1Width + 4, posY + (height - height / 4 - textSize.Height - 8));
                        }
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedBottom1Width + 4, posY + (height - height / 4 + textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom1Width + 4, posY + (height - height / 4 + textSize.Height - 4));
                        }
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedTop2Width + 4, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedTop2Width + 4, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        }
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + usedBottom2Width + 4, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + usedBottom2Width + 4, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        }
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
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            }
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            }
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            }
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            }
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            }
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
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedMainWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 + usedMainWidthRight), posY + (height - height / 4 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedMainWidthRight), posY + (height - height / 4 - textSize.Height / 2));
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedTopWidthLeft - textSize.Width), posY + (height - height / 4 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 + usedTopWidthRight), posY + (height - height / 4 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedTopWidthRight), posY + (height - height / 4 - textSize.Height - 8));
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - textSize.Width / 2), posY + (height - height / 4 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height - height / 4 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 - usedBottomWidthLeft - textSize.Width), posY + (height - height / 4 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width / 2 + usedBottomWidthRight), posY + (height - height / 4 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width / 2 + usedBottomWidthRight), posY + (height - height / 4 + textSize.Height - 8));
                            }
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
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - textHeight - 4);
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + height - textHeight - 4);
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + height - textHeight - 4);
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - 2 * textHeight - 4);
                            }
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + height - 2 * textHeight - 4);
                            }
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom1WidthRight), posY + height - 2 * textHeight - 4);
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + height - 3 * textHeight - 4);
                            }
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + height - 3 * textHeight - 4);
                            }
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom2WidthRight), posY + height - 3 * textHeight - 4);
                            }
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
                    {
                        g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedMainWidth - textSize.Width, posY + (height - height / 4 - textSize.Height / 2));
                    }
                    else
                    {
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedMainWidth - textSize.Width, posY + (height - height / 4 - textSize.Height / 2));
                    }
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedTop1Width - textSize.Width, posY + (height - height / 4 - textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop1Width - textSize.Width, posY + (height - height / 4 - textSize.Height - 8));
                        }
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedBottom1Width - textSize.Width, posY + (height - height / 4 + textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom1Width - textSize.Width, posY + (height - height / 4 + textSize.Height - 4));
                        }
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedTop2Width - textSize.Width, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop2Width - textSize.Width, posY + (height - height / 4 - 2 * textSize.Height - 8));
                        }
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedBottom2Width - textSize.Width, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom2Width - textSize.Width, posY + (height - height / 4 + 2 * textSize.Height - 4));
                        }
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
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height / 2));
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + (height / 2 - textSize.Height / 2));
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width / 2;
                            usedTopWidthRight += textSize.Width / 2;
                            topLeft = true;
                        }
                        else if (topLeft && !topRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedTopWidthLeft - textSize.Width), posY + (height / 2 - textSize.Height - 8));
                            }
                            usedTopWidthLeft += textSize.Width;
                            topLeft = false;
                            topRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedTopWidthRight), posY + (height / 2 - textSize.Height - 8));
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + (height / 2 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width / 2;
                            usedBottomWidthRight += textSize.Width / 2;
                            bottomLeft = true;
                        }
                        else if (bottomLeft && !bottomRight)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottomWidthLeft - textSize.Width), posY + (height / 2 + textSize.Height - 8));
                            }
                            usedBottomWidthLeft += textSize.Width;
                            bottomLeft = false;
                            bottomRight = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottomWidthRight), posY + (height / 2 + textSize.Height - 8));
                            }
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
                    {
                        g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedMainWidth - textSize.Width, posY + (height / 4 - textSize.Height / 2));
                    }
                    else
                    {
                        g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedMainWidth - textSize.Width, posY + (height / 4 - textSize.Height / 2));
                    }
                    usedMainWidth += textSize.Width;
                }
                else
                {
                    if (usedTop1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedTop1Width - textSize.Width, posY + (height / 4 - textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop1Width - textSize.Width, posY + (height / 4 - textSize.Height - 8));
                        }
                        usedTop1Width += textSize.Width;
                    }
                    else if (usedBottom1Width + textSize.Width <= top1LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedBottom1Width - textSize.Width, posY + (height / 4 + textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom1Width - textSize.Width, posY + (height / 4 + textSize.Height - 4));
                        }
                        usedBottom1Width += textSize.Width;
                    }
                    else if (usedTop2Width + textSize.Width <= top2LineWidth)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedTop2Width - textSize.Width, posY + (height / 4 - 2 * textSize.Height - 8));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedTop2Width - textSize.Width, posY + (height / 4 - 2 * textSize.Height - 8));
                        }
                        usedTop2Width += textSize.Width;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + width - usedBottom2Width - textSize.Width, posY + (height / 4 + 2 * textSize.Height - 4));
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + width - usedBottom2Width - textSize.Width, posY + (height / 4 + 2 * textSize.Height - 4));
                        }
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
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + 4);
                        }
                        usedMainWidthLeft += textSize.Width / 2;
                        usedMainWidthRight += textSize.Width / 2;
                        mainLeft = true;
                    }
                    else if (mainLeft && !mainRight)
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedMainWidthLeft - textSize.Width), posY + 4);
                        }
                        usedMainWidthLeft += textSize.Width;
                        mainLeft = false;
                        mainRight = true;
                    }
                    else
                    {
                        if (!displayPlanetsList[i].IsActiveAspect)
                        {
                            g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedMainWidthRight), posY + 4);
                        }
                        else
                        {
                            g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedMainWidthRight), posY + 4);
                        }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + textHeight + 4);
                            }
                            usedBottom1WidthLeft += textSize.Width / 2;
                            usedBottom1WidthRight += textSize.Width / 2;
                            bottom1Left = true;
                        }
                        else if (bottom1Left && !bottom1Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom1WidthLeft - textSize.Width), posY + textHeight + 4);
                            }
                            usedBottom1WidthLeft += textSize.Width;
                            bottom1Left = false;
                            bottom1Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom1WidthRight), posY + textHeight + 4);
                            }
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
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - textSize.Width / 2), posY + 2 * textHeight + 4);
                            }
                            usedBottom2WidthLeft += textSize.Width / 2;
                            usedBottom2WidthRight += textSize.Width / 2;
                            bottom2Left = true;
                        }
                        else if (bottom2Left && !bottom2Right)
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 - usedBottom2WidthLeft - textSize.Width), posY + 2 * textHeight + 4);
                            }
                            usedBottom2WidthLeft += textSize.Width;
                            bottom2Left = false;
                            bottom2Right = true;
                        }
                        else
                        {
                            if (!displayPlanetsList[i].IsActiveAspect)
                            {
                                g.DrawString(text, font, new SolidBrush(Color.Black /*Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)*/), posX + (width - width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            }
                            else
                            {
                                g.DrawString(text, aspectFont, new SolidBrush(Utility.GetColorByColorCode(displayPlanetsList[i].ColorCode)), posX + (width - width / 4 + usedBottom2WidthRight), posY + 2 * textHeight + 4);
                            }
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
                PrepareTransitMap();
            }
            else
            {
                UncheckAllAspectBoxes();
                PrepareTransitMap();
            }
        }

        private void checkBoxMoon_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
        }

        private void checkBoxSun_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
        }

        private void checkBoxVenus_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
        }

        private void checkBoxJupiter_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
        }

        private void checkBoxMercury_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
        }

        private void checkBoxMars_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
        }

        private void checkBoxSaturn_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
        }

        private void checkBoxRahu_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckIfAllPlanetCheckBoxChecked())
                checkBoxAll.Checked = true;
            if (CheckIfAllPlanetCheckBoxUnchecked())
                checkBoxAll.Checked = false;
            PrepareTransitMap();
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

        private void PrepareDataGridInfo(ELanguage langCode)
        {
            dataGridViewInfo.AutoGenerateColumns = false;
            dataGridViewInfo.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.TRANSTOOLTIPHEADER)), 10, Utility.GetFontStyleBySettings(EFontList.TRANSTOOLTIPHEADER));
            dataGridViewInfo.DefaultCellStyle.Font = new Font(new FontFamily(Utility.GetFontNameByCode(EFontList.DWTOOLTIPTEXT)), 9, Utility.GetFontStyleBySettings(EFontList.DWTOOLTIPTEXT));

            DataGridViewColumn column = new DataGridViewColumn();
            column.DataPropertyName = "Planet";
            column.Name = Utility.GetLocalizedText("", langCode);
            column.Width = 30;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Degree";
            column.Name = Utility.GetLocalizedText("Degree", langCode);
            column.Width = 60;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Znak";
            column.Name = Utility.GetLocalizedText("Rasi", langCode);
            column.Width = 90;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            column = new DataGridViewColumn();
            column.DataPropertyName = "Nakshatra";
            column.Name = Utility.GetLocalizedText("Nakshatra", langCode);
            column.Width = 150;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridViewInfo.Columns.Add(column);

            int lastColWidth = (dataGridViewInfo.Width - 330);
            column = new DataGridViewColumn();
            column.DataPropertyName = "Pada";
            column.Name = Utility.GetLocalizedText("Pada", langCode);
            column.Width = lastColWidth;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfo.Columns.Add(column);

            dataGridViewInfo.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            foreach (DataGridViewColumn col in dataGridViewInfo.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridViewInfo.EnableHeadersVisualStyles = false;
            dataGridViewInfo.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            dataGridViewInfo.ScrollBars = ScrollBars.None;
            dataGridViewInfo.Height = dataGridViewInfo.ColumnHeadersHeight;
        }

        public struct dgvRowObj
        {
            public string Planet { get; set; }
            public string Degree { get; set; }
            public string Zodiak { get; set; }
            public string Nakshatra { get; set; }
            public string Pada { get; set; }
        }

        private void ProfileInfoDataGridViewFillByRow(ELanguage langCode)
        {
            dgvRowObj rowTemp;
            List<dgvRowObj> rowList = new List<dgvRowObj>();
            string degree = string.Empty, zodiak = string.Empty, nakshatra = string.Empty, pada = string.Empty;
            EAppSetting nodesSetting = (EAppSetting)CacheLoad._appSettingList.Where(i => i.GroupCode.Equals(EAppSettingList.NODE.ToString()) && i.Active == 1).FirstOrDefault().Id;

            if (SelectedProfile != null)
            {
                int nakshatraId = Utility.GetNakshatraIdFromDegree(_birthAscendant);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_birthAscendant));
                degree = Utility.ConvertDecimalToDegree(_birthAscendant);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                degree = Utility.ConvertDecimalToDegree(_ascendant);
                zodiak = GetZodiakNameByIds(_nakshatralagnaId, _padaLagna);
                nakshatra = GetNakshatraNameById(_nakshatralagnaId);
                pada = _padaLagna.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedText("Lg", langCode),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if(SelectedProfile != null)
            {
                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().Longitude;
                int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                degree = Utility.ConvertDecimalToDegree(pLong);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().NakshatraId;
                int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().PadaId);
                double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.SUN).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SUN, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (SelectedProfile != null)
            {
                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().Longitude;
                int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                degree = Utility.ConvertDecimalToDegree(pLong);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().NakshatraId;
                int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().PadaId);
                double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.MOON).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MOON, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (SelectedProfile != null)
            {
                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().Longitude;
                int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                degree = Utility.ConvertDecimalToDegree(pLong);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().NakshatraId;
                int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().PadaId);
                double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.MARS).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MARS, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (SelectedProfile != null)
            {
                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().Longitude;
                int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                degree = Utility.ConvertDecimalToDegree(pLong);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().NakshatraId;
                int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().PadaId);
                double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.MERCURY).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.MERCURY, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (SelectedProfile != null)
            {
                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().Longitude;
                int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                degree = Utility.ConvertDecimalToDegree(pLong);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().NakshatraId;
                int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().PadaId);
                double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.JUPITER).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.JUPITER, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (SelectedProfile != null)
            {
                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().Longitude;
                int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                degree = Utility.ConvertDecimalToDegree(pLong);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().NakshatraId;
                int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().PadaId);
                double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.VENUS).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.VENUS, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (SelectedProfile != null)
            {
                double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().Longitude;
                int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                degree = Utility.ConvertDecimalToDegree(pLong);
                nakshatra = GetNakshatraNameById(nakshatraId);
                pada = padaNum.ToString();
                zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
            }
            else
            {
                int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().NakshatraId;
                int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().PadaId);
                double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.SATURN).FirstOrDefault().Longitude;
                degree = Utility.ConvertDecimalToDegree(pLong);
                zodiak = GetZodiakNameByIds(nakId, padaId);
                nakshatra = GetNakshatraNameById(nakId);
                pada = padaId.ToString();
            }
            rowTemp = new dgvRowObj
            {
                Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.SATURN, langCode).Substring(0, 2),
                Degree = degree,
                Zodiak = zodiak,
                Nakshatra = nakshatra,
                Pada = pada
            };
            rowList.Add(rowTemp);

            if (nodesSetting == EAppSetting.NODEMEAN)
            {
                if (SelectedProfile != null)
                {
                    double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().Longitude;
                    int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                    int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    nakshatra = GetNakshatraNameById(nakshatraId);
                    pada = padaNum.ToString();
                    zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
                }
                else
                {
                    int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().NakshatraId;
                    int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().PadaId);
                    double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUMEAN).FirstOrDefault().Longitude;
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    zodiak = GetZodiakNameByIds(nakId, padaId);
                    nakshatra = GetNakshatraNameById(nakId);
                    pada = padaId.ToString();
                }
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.RAHUMEAN, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);

                if (SelectedProfile != null)
                {
                    double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().Longitude;
                    int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                    int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    nakshatra = GetNakshatraNameById(nakshatraId);
                    pada = padaNum.ToString();
                    zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
                }
                else
                {
                    int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().NakshatraId;
                    int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().PadaId);
                    double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUMEAN).FirstOrDefault().Longitude;
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    zodiak = GetZodiakNameByIds(nakId, padaId);
                    nakshatra = GetNakshatraNameById(nakId);
                    pada = padaId.ToString();
                }
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.KETUMEAN, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);
            }

            if (nodesSetting == EAppSetting.NODETRUE)
            {
                if (SelectedProfile != null)
                {
                    double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().Longitude;
                    int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                    int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    nakshatra = GetNakshatraNameById(nakshatraId);
                    pada = padaNum.ToString();
                    zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
                }
                else
                {
                    int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().NakshatraId;
                    int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().PadaId);
                    double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.RAHUTRUE).FirstOrDefault().Longitude;
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    zodiak = GetZodiakNameByIds(nakId, padaId);
                    nakshatra = GetNakshatraNameById(nakId);
                    pada = padaId.ToString();
                }
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.RAHUTRUE, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);

                if (SelectedProfile != null)
                {
                    double pLong = _pdBirthList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().Longitude;
                    int nakshatraId = Utility.GetNakshatraIdFromDegree(pLong);
                    int padaNum = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(pLong));
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    nakshatra = GetNakshatraNameById(nakshatraId);
                    pada = padaNum.ToString();
                    zodiak = GetZodiakNameByIds(nakshatraId, padaNum);
                }
                else
                {
                    int nakId = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().NakshatraId;
                    int padaId = Utility.GetPadaNumberByPadaId(_pdList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().PadaId);
                    double pLong = _pdList.Where(i => i.PlanetId == (int)EPlanet.KETUTRUE).FirstOrDefault().Longitude;
                    degree = Utility.ConvertDecimalToDegree(pLong);
                    zodiak = GetZodiakNameByIds(nakId, padaId);
                    nakshatra = GetNakshatraNameById(nakId);
                    pada = padaId.ToString();
                }
                rowTemp = new dgvRowObj
                {
                    Planet = Utility.GetLocalizedPlanetNameByCode(EPlanet.KETUTRUE, langCode).Substring(0, 2),
                    Degree = degree,
                    Zodiak = zodiak,
                    Nakshatra = nakshatra,
                    Pada = pada
                };
                rowList.Add(rowTemp);
            }

            dataGridViewInfo.Rows.Clear();
            for (int i = 0; i < rowList.Count; i++)
            {
                string[] row = new string[] {
                        rowList[i].Planet,
                        rowList[i].Degree,
                        rowList[i].Zodiak,
                        rowList[i].Nakshatra,
                        rowList[i].Pada
                };
                dataGridViewInfo.Rows.Add(row);
            }
            
            int heihgt = dataGridViewInfo.ColumnHeadersHeight;
            for (int i = 0; i < dataGridViewInfo.RowCount; i++)
            {
                int rowHeight = dataGridViewInfo.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
                heihgt += rowHeight;
            }
            dataGridViewInfo.Height = heihgt;
            dataGridViewInfo.ClearSelection();
        }

        private string GetZodiakNameByIds(int nakshatraId, int pada)
        {
            int id = Utility.GetZodiakIdFromNakshatraIdandPada(nakshatraId, pada);
            return id + ". " + _zdList.Where(i => i.ZodiakId == id).FirstOrDefault().Name;
        }
        private string GetNakshatraNameById(int id)
        {
            return id + ". " + _ndList.Where(i => i.NakshatraId == id).FirstOrDefault().Name;
        }

        private void FillProfileListViewByData(List<Profile> pList)
        {
            if (pList.Count > 0)
            {
                PrepareListView();
                foreach (Profile p in pList)
                {
                    if (p.Checked == 1)
                    {
                        listViewProfile.Items.Add(p.ProfileName, 0);
                    }
                    else
                    {
                        listViewProfile.Items.Add(p.ProfileName, -1);
                    }
                }
            }
        }

        private void PrepareListView()
        {
            listViewProfile.HeaderStyle = ColumnHeaderStyle.None;
            listViewProfile.Columns.Add("", listViewProfile.Width, HorizontalAlignment.Left);

            ImageList imgs = new ImageList();
            imgs.Images.Add(Properties.Resources.Apply);
            listViewProfile.SmallImageList = imgs;
        }

        

        private void ShowDataByName(string profile)
        {
            SelectedProfile = _proList.Where(i => i.ProfileName.Equals(profile)).FirstOrDefault();
            if (SelectedProfile != null)
            {
                textBoxProfileName.Text = SelectedProfile.ProfileName;
                textBoxPersonName.Text = SelectedProfile.PersonName;
                textBoxPersonSurname.Text = SelectedProfile.PersonSurname; 
                maskedTextBoxDate.Text = SelectedProfile.DateOfBirth.ToString("dd.MM.yyyy HH:mm:ss");
                textBoxBirthPlace.Text = CacheLoad._locationList.Where(i => i.Id == SelectedProfile.PlaceOfBirthId).FirstOrDefault()?.Locality ?? string.Empty;
                textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == SelectedProfile.PlaceOfLivingId).FirstOrDefault()?.Locality ?? string.Empty;
                richTextBoxNotes.Text = SelectedProfile.Message;

                dataGridViewInfo.Rows.Clear();
                PrepareTransitMap();
                ProfileInfoDataGridViewFillByRow(_activeLang);

            }
        }


        private void listViewProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewProfile.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                ShowDataByName(listViewProfile.Items[intselectedindex].Text);
                buttonDefault.Enabled = true;
                buttonChoose.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDelete.Enabled = true;
            }
        }
        
        private void checkedListBoxProfile_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (e.Index == 0) e.NewValue = e.CurrentValue;
            /*
            if (e.NewValue == CheckState.Checked && checkedListBoxProfile.CheckedItems.Count > 0)
            {
                checkedListBoxProfile.ItemCheck -= checkedListBoxProfile_ItemCheck;
                checkedListBoxProfile.SetItemChecked(checkedListBoxProfile.CheckedIndices[0], false);
                checkedListBoxProfile.ItemCheck += checkedListBoxProfile_ItemCheck;
            }*/
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            int selectedProfileId = 0, selectedIndex = -1;
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            
            selectedIndex = listViewProfile.SelectedIndex();
            if (selectedIndex >= 0)
            {
                selectedProfileId = _proList.Where(i => i.ProfileName.Equals(listViewProfile.Items[selectedIndex].Text)).FirstOrDefault()?.Id ?? 0;
                if (selectedProfileId != 0)
                {
                    SelectedProfile = CacheLoad._profileList.Where(i => i.Id == selectedProfileId).FirstOrDefault();
                    IsChosen = true;
                }
                else
                {
                    SelectedProfile = null;

                }
            }
            Close();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxSearch.Text.Trim()) == false)
            {
                listViewProfile.Items.Clear();
                CleanFields();
                foreach (Profile p in _proList)
                {
                    PrepareListView();
                    if (p.ProfileName.ToLower().StartsWith(textBoxSearch.Text.ToLower().Trim()))
                    {
                        listViewProfile.Items.Add(p.ProfileName);
                    }
                }
            }
            else if (textBoxSearch.Text.Trim() == "")
            {
                listViewProfile.Items.Clear();
                FillProfileListViewByData(_proList);
                CleanFields();
                buttonDefault.Enabled = false;
                buttonChoose.Enabled = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonDelete.Enabled = false;
            }
        }

        private void CleanFields()
        {
            textBoxProfileName.Text = string.Empty;
            textBoxPersonName.Text = string.Empty;
            textBoxPersonSurname.Text = string.Empty;
            maskedTextBoxDate.Text = string.Empty;
            textBoxBirthPlace.Text = string.Empty;
            textBoxLivingPlace.Text = string.Empty;
            richTextBoxNotes.Text = string.Empty;
            pictureBoxMap.Image = null;
            dataGridViewInfo.Rows.Clear();
        }

        private void MakeTextFieldsEditable(bool isUpdate)
        {
            textBoxProfileName.ReadOnly = false;
            textBoxPersonName.ReadOnly = false;
            textBoxPersonSurname.ReadOnly = false;
            maskedTextBoxDate.ReadOnly = false;
            richTextBoxNotes.ReadOnly = false;
            buttonBirthPlace.Enabled = true;
            buttonLivingPlace.Enabled = true;
            textBoxProfileName.BackColor = SystemColors.Window;
            textBoxPersonName.BackColor = SystemColors.Window;
            textBoxPersonSurname.BackColor = SystemColors.Window;
            maskedTextBoxDate.BackColor = SystemColors.Window;
            richTextBoxNotes.BackColor = SystemColors.Window;
            buttonBirthPlace.BackColor = SystemColors.Window;
            buttonLivingPlace.BackColor = SystemColors.Window;
        }

        private void MakeTextFieldsReadOnly()
        {
            textBoxProfileName.ReadOnly = true;
            textBoxPersonName.ReadOnly = true;
            textBoxPersonSurname.ReadOnly = true;
            maskedTextBoxDate.ReadOnly = true;
            richTextBoxNotes.ReadOnly = true;
            buttonBirthPlace.Enabled = false;
            buttonLivingPlace.Enabled = false;
            textBoxProfileName.BackColor = SystemColors.GradientInactiveCaption;
            textBoxPersonName.BackColor = SystemColors.GradientInactiveCaption;
            textBoxPersonSurname.BackColor = SystemColors.GradientInactiveCaption;
            maskedTextBoxDate.BackColor= SystemColors.GradientInactiveCaption;
            richTextBoxNotes.BackColor= SystemColors.GradientInactiveCaption;
            buttonBirthPlace.BackColor= SystemColors.GradientInactiveCaption;
            buttonLivingPlace.BackColor= SystemColors.GradientInactiveCaption;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            _isNew = true;
            _isModify = false;

            listViewProfile.SelectedIndices.Clear();
            CleanFields();
            textBoxSearch.Text = string.Empty;
            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = false;
            buttonGenerateMap.Enabled = true;
            buttonGenerateMap.BackColor = SystemColors.Window;
            buttonSaveProfile.Enabled = false;
            buttonChoose.Enabled = false;
            buttonDefault.Enabled = false;
            MakeTextFieldsEditable(false);
            textBoxProfileName.Focus();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            _isNew = false;
            _isModify = true;

            toolStripButtonAdd.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = true;
            buttonChoose.Enabled = false;
            buttonDefault.Enabled = false;
            MakeTextFieldsEditable(true);
            textBoxProfileName.Focus();
        }

        private void buttonBirthPlace_Click(object sender, EventArgs e)
        {
            LocationForm lForm = new LocationForm(CacheLoad._locationList.ToList(), _activeLang, false);
            lForm.ShowDialog(this);
            if (lForm.SelectedLocation != null)
            {
                _selectedBirthLocation = lForm.SelectedLocation;
                textBoxBirthPlace.Text = CacheLoad._locationList.Where(i => i.Id == _selectedBirthLocation.Id).FirstOrDefault()?.Locality ?? string.Empty;
            }
        }

        private void buttonLivingPlace_Click(object sender, EventArgs e)
        {
            LocationForm lForm = new LocationForm(CacheLoad._locationList.ToList(), _activeLang, false);
            lForm.ShowDialog(this);
            if (lForm.SelectedLocation != null)
            {
                _selectedLivingLocation = lForm.SelectedLocation;
                textBoxLivingPlace.Text = CacheLoad._locationList.Where(i => i.Id == _selectedLivingLocation.Id).FirstOrDefault()?.Locality ?? string.Empty;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            int selectedindex = listViewProfile.SelectedIndices[0];
            if (selectedindex >= 0)
            {
                int selectedProfileId = _proList[selectedindex].Id;
                DeleteProfile(selectedProfileId);

                _proList.RemoveAll(i => i.Id == selectedProfileId);
                CacheLoad._profileList.RemoveAll(i => i.Id == selectedProfileId);
                FillProfileListViewByData(_proList);

                CleanFields();
                MakeTextFieldsReadOnly();
                buttonChoose.Visible = false;
                buttonDefault.Enabled = false;
                textBoxSearch_TextChanged(sender, e);
                textBoxSearch.Focus();
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (_isModify)
            {
                ModifyProfile(SelectedProfile.Id);
            }

            CacheLoad._profileList = CacheLoad.GetProfilesList();
            _proList = CacheLoad._profileList.ToList();
            FillProfileListViewByData(_proList);

            buttonChoose.Enabled = false;
            buttonDefault.Enabled = false;
            toolStripButtonAdd.Enabled = true; ;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = false;

            CleanFields();
            MakeTextFieldsReadOnly();
            textBoxSearch_TextChanged(sender, e);
            textBoxSearch.Focus();
        }

        private void InsertNewProfile()
        {
            DateTime date = DateTime.ParseExact(maskedTextBoxDate.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Profile newProfile = new Profile
            {
                ProfileName = textBoxProfileName.Text,
                PersonName = textBoxPersonName.Text,
                PersonSurname = textBoxPersonSurname.Text,
                DateOfBirth = DateTime.ParseExact(maskedTextBoxDate.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                PlaceOfBirthId = CacheLoad._locationList.Where(i => i.Id == _selectedBirthLocation.Id).FirstOrDefault()?.Id ?? 0,
                PlaceOfLivingId = CacheLoad._locationList.Where(i => i.Id == _selectedLivingLocation.Id).FirstOrDefault()?.Id ?? 0,
                Message = richTextBoxNotes.Text,
                Checked = 0
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("insert into PROFILE (PROFILENAME, PERSONNAME, PERSONSURNAME, DATEOFBIRTH, PLACEOFBIRTHID, PLACEOFLIVINGID, MESSAGE, CHECKED) values (@PROFILENAME, @PERSONNAME, @PERSONSURNAME, @DATEOFBIRTH, @PLACEOFBIRTHID, @PLACEOFLIVINGID, @MESSAGE, @CHECKED)", dbCon);
                    command.Parameters.AddWithValue("@PROFILENAME", newProfile.ProfileName);
                    command.Parameters.AddWithValue("@PERSONNAME", newProfile.PersonName);
                    command.Parameters.AddWithValue("@PERSONSURNAME", newProfile.PersonSurname);
                    command.Parameters.AddWithValue("@DATEOFBIRTH", newProfile.DateOfBirth.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@PLACEOFBIRTHID", newProfile.PlaceOfBirthId);
                    command.Parameters.AddWithValue("@PLACEOFLIVINGID", newProfile.PlaceOfLivingId);
                    command.Parameters.AddWithValue("@MESSAGE", newProfile.Message);
                    command.Parameters.AddWithValue("@CHECKED", newProfile.Checked);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void ModifyProfile(int pId)
        {
            int birthLocationId = SelectedProfile.PlaceOfBirthId;
            int currentLocationId = SelectedProfile.PlaceOfLivingId;
            if (_selectedBirthLocation != null)
            {
                birthLocationId = _selectedBirthLocation.Id;
            }
            if (_selectedLivingLocation != null)
            {
                currentLocationId = _selectedLivingLocation.Id;
            }

            Profile newProfile = new Profile
            {
                ProfileName = textBoxProfileName.Text,
                PersonName = textBoxPersonName.Text,
                PersonSurname = textBoxPersonSurname.Text,
                DateOfBirth = DateTime.ParseExact(maskedTextBoxDate.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                PlaceOfBirthId = CacheLoad._locationList.Where(i => i.Id == birthLocationId).FirstOrDefault()?.Id ?? 0,
                PlaceOfLivingId = CacheLoad._locationList.Where(i => i.Id == currentLocationId).FirstOrDefault()?.Id ?? 0,
                Message = richTextBoxNotes.Text
            };

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("update PROFILE set PROFILENAME = @PROFILENAME, PERSONNAME = @PERSONNAME, PERSONSURNAME = @PERSONSURNAME, DATEOFBIRTH = @DATEOFBIRTH, PLACEOFBIRTHID = @PLACEOFBIRTHID, PLACEOFLIVINGID = @PLACEOFLIVINGID, MESSAGE = @MESSAGE where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@PROFILENAME", newProfile.ProfileName);
                    command.Parameters.AddWithValue("@PERSONNAME", newProfile.PersonName);
                    command.Parameters.AddWithValue("@PERSONSURNAME", newProfile.PersonSurname);
                    command.Parameters.AddWithValue("@DATEOFBIRTH", newProfile.DateOfBirth);
                    command.Parameters.AddWithValue("@PLACEOFBIRTHID", newProfile.PlaceOfBirthId);
                    command.Parameters.AddWithValue("@PLACEOFLIVINGID", newProfile.PlaceOfLivingId);
                    command.Parameters.AddWithValue("@MESSAGE", newProfile.Message);
                    command.Parameters.AddWithValue("@ID", pId);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void UpdateCheckStatus(int pId)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                SQLiteCommand command;
                try
                {
                    command = new SQLiteCommand("update PROFILE set CHECKED = 0", dbCon);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("update PROFILE set CHECKED = 1 where ID = @ID", dbCon);
                    command.Parameters.AddWithValue("@ID", pId);
                    command.ExecuteNonQuery();
                }
                catch { }
                dbCon.Close();
            }
        }

        private void DeleteProfile(int pId)
        {
            DialogResult dialogResult = frmShowMessage.Show(Utility.GetLocalizedText("Do you want to delete this profile?", _activeLang), Utility.GetLocalizedText("Confirmation", _activeLang), enumMessageIcon.Question, enumMessageButton.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SQLiteConnection dbCon = Utility.GetSQLConnection())
                {
                    dbCon.Open();
                    SQLiteCommand command;
                    try
                    {
                        command = new SQLiteCommand("delete from PROFILE where ID = @ID", dbCon);
                        command.Parameters.AddWithValue("@ID", pId);
                        command.ExecuteNonQuery();
                    }
                    catch { }
                    dbCon.Close();
                }
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            int selectedProfileId = 0;
            if (listViewProfile.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewProfile.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                selectedProfileId = _proList.Where(i => i.ProfileName.Equals(listViewProfile.SelectedItems[0].Text)).FirstOrDefault()?.Id ?? 0;
                UpdateCheckStatus(selectedProfileId);

                CacheLoad._profileList = CacheLoad.GetProfilesList();
                _proList = CacheLoad._profileList.ToList();
                FillProfileListViewByData(_proList);

                CleanFields();
                MakeTextFieldsReadOnly();
                textBoxSearch_TextChanged(sender, e);
                textBoxSearch.Focus();
            }
        }

        private void buttonGenerateMap_Click(object sender, EventArgs e)
        {
            if (maskedTextBoxDate.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Enter date of Birth.", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }
            if (textBoxBirthPlace.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Choose place of Birth.", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }

            DateTime birthDate;
            double latitude, longitude;
            if (Utility.GetGeoCoordinateByLocationId(_selectedBirthLocation.Id, out latitude, out longitude))
            {
                try
                {
                    DateTime.TryParse(maskedTextBoxDate.Text, out birthDate);

                    buttonChoose.Enabled = false;
                    buttonDefault.Enabled = false;
                    toolStripButtonAdd.Enabled = true; ;
                    toolStripButtonEdit.Enabled = false;
                    toolStripButtonDelete.Enabled = false;
                    toolStripButtonSave.Enabled = false;
                    buttonSaveProfile.Enabled = true;
                    buttonSaveProfile.BackColor = SystemColors.Window;

                    _pdList = Utility.CalculatePlanetsPositionForDate(birthDate, latitude, longitude);
                    _ascendant = Utility.CalculateAscendantForDate(birthDate, latitude, longitude, 0, 'O');
                    _nakshatralagnaId = Utility.GetNakshatraIdFromDegree(_ascendant);
                    _padaLagna = Utility.GetPadaNumberByPadaId(Utility.GetPadaIdFromDegree(_ascendant));
                    _lagnaId = Utility.GetZodiakIdFromDegree(_ascendant);
                    PrepareTransitMap();
                    ProfileInfoDataGridViewFillByRow(_activeLang);
                }
                catch { }
            }
        }
        private void buttonSaveProfile_Click(object sender, EventArgs e)
        {
            if (textBoxProfileName.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Enter profile name.", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }
            if (textBoxBirthPlace.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Choose place of birth.", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }
            if (textBoxLivingPlace.Text.Equals(string.Empty))
            {
                frmShowMessage.Show(Utility.GetLocalizedText("Choose place of living.", _activeLang), Utility.GetLocalizedText("Error", _activeLang), enumMessageIcon.Error, enumMessageButton.OK);
                return;
            }

            if (_isNew)
            {
                InsertNewProfile();
            }

            CacheLoad._profileList = CacheLoad.GetProfilesList();
            _proList = CacheLoad._profileList.ToList();
            FillProfileListViewByData(_proList);

            buttonChoose.Enabled = false;
            buttonDefault.Enabled = false;
            toolStripButtonAdd.Enabled = true; ;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonSave.Enabled = false;
            buttonGenerateMap.Enabled = false;
            buttonSaveProfile.Enabled = false;
            buttonGenerateMap.BackColor = SystemColors.GradientInactiveCaption;
            buttonSaveProfile.BackColor = SystemColors.GradientInactiveCaption;

            CleanFields();
            MakeTextFieldsReadOnly();
            textBoxSearch_TextChanged(sender, e);
            textBoxSearch.Focus();

        }

        private void maskedTextBoxDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }
    }
}
