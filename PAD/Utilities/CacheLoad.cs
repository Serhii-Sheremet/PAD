using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;

namespace PAD
{
    public static class CacheLoad
    {
        public static List<DVLineNames> _dvLineNamesList;
        public static List<DVLineNameDescription> _dvLineNamesDescList;
        public static List<Language> _languageList;
        public static List<LanguageDescription> _languageDescList;
        public static List<AppTexts> _appTextsList;
        public static List<Location> _locationList;
        public static List<Profile> _profileList;
        public static List<PersonsEventsList> _personEventsList;

        public static List<AppSettingList> _appSettingList;
        public static List<Colors> _colorList;
        public static List<ColorDescription> _colorDescList;
        public static List<SystemFont> _systemFontList;
        public static List<FontList> _fontList;
        public static List<FontListDescription> _fontDescList;

        public static List<Planet> _planetList;
        public static List<PlanetDescription> _planetDescList;
        public static List<Zodiak> _zodiakList;
        public static List<ZodiakDescription> _zodiakDescList;
        public static List<Nakshatra> _nakshatraList;
        public static List<NakshatraDescription> _nakshatraDescList;
        public static List<Pada> _padaList;
        public static List<SpecialNavamsha> _specNavamshaList;
        public static List<Masa> _masaList;
        public static List<MasaDescription> _masaDescList;
        public static List<TaraBala> _taraBalaList;
        public static List<TaraBalaDescription> _taraBalaDescList;
        public static List<Tithi> _tithiList;
        public static List<TithiDescription> _tithiDescList;
        public static List<Karana> _karanaList;
        public static List<KaranaDescription> _karanaDescList;
        public static List<NityaJoga> _nityaJogaList;
        public static List<NityaJogaDescription> _nityaJogaDescList;
        public static List<Muhurta> _muhurtaList;
        public static List<MuhurtaDescription> _muhurtaDescList;
        public static List<Joga> _jogaList;
        public static List<JogaDescription> _jogaDescList;
        public static List<Eclipse> _eclipseList;
        public static List<EclipseDescription> _eclipseDescList;
        public static List<Tranzit> _tranzitList;
        public static List<TranzitDescription> _tranzitDescList;
        public static List<Muhurta30> _muhurta30List;
        public static List<Muhurta30Description> _muhurta30DescList;
        public static List<Ghati60> _ghati60List;
        public static List<Ghati60Description> _ghati60DescList;
        public static List<HoraPlanet> _horaPlanetList;

        public static List<DVLineNames> GetDVLineNamesList()
        {
            List<DVLineNames> entityList = new List<DVLineNames>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, CODE from DVLINENAMES order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                DVLineNames temp = new DVLineNames
                                {
                                    Id = reader.IntValue(0),
                                    Code = reader.StringValue(1)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            entityList.Add(new DVLineNames() { Id = 100, Code = "USER" });
            return entityList;
        }

        public static List<DVLineNameDescription> GetDVLineNamesDescList()
        {
            List<DVLineNameDescription> entityList = new List<DVLineNameDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, DVLINENAMESID, SHORTNAME, NAME, LANGUAGECODE from DVLINENAMES_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                DVLineNameDescription temp = new DVLineNameDescription
                                {
                                    Id = reader.IntValue(0),
                                    DVLineNameId = reader.IntValue(1),
                                    ShortName = reader.StringValue(2),
                                    Name = reader.StringValue(3),
                                    LanguageCode = reader.StringValue(4)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Language> GetLanguagesList()
        {
            List<Language> entityList = new List<Language>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, LANGUAGECODE, CULTURECODE from LANGUAGE order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Language temp = new Language
                                {
                                    Id = reader.IntValue(0),
                                    LanguageCode = reader.StringValue(1),
                                    CultureCode = reader.StringValue(2)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<LanguageDescription> GetLanguageDescList()
        {
            List<LanguageDescription> entityList = new List<LanguageDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, LANUAGEID, NAME, LANGUAGECODE from LANGUAGE_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                LanguageDescription temp = new LanguageDescription
                                {
                                    Id = reader.IntValue(0),
                                    LanguageId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<AppTexts> GetAppTextsList()
        {
            List<AppTexts> entityList = new List<AppTexts>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, NATIVETEXT, FOREIGNTEXT, LANGUAGECODE from APP_TEXTS order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                AppTexts temp = new AppTexts
                                {
                                    Id = reader.IntValue(0),
                                    NativeText = reader.StringValue(1),
                                    ForeignText = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Location> GetLocationsList()
        {
            List<Location> entityList = new List<Location>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = "select ID, LOCALITY, LATITUDE, LONGITUDE, REGION, STATE, COUNTRY, COUNTRYCODE, LANGUAGECODE from LOCATION order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Location temp = new Location
                                {
                                    Id = reader.IntValue(0),
                                    Locality = reader.StringValue(1),
                                    Latitude = reader.StringValue(2),
                                    Longitude = reader.StringValue(3),
                                    Region = reader.StringValue(4),
                                    State = reader.StringValue(5),
                                    Country = reader.StringValue(6),
                                    CountryCode = reader.StringValue(7),
                                    LanguageCode = reader.StringValue(8)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<AppSettingList> GetAppSettingsList()
        {
            List<AppSettingList> entityList = new List<AppSettingList>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, GROUPCODE, SETTINGCODE, ACTIVE from APPSETTING order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                AppSettingList temp = new AppSettingList
                                {
                                    Id = reader.IntValue(0),
                                    GroupCode = reader.StringValue(1),
                                    SettingCode = reader.StringValue(2),
                                    Active = reader.IntValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Colors> GetColorsList()
        {
            List<Colors> entityList = new List<Colors>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, CODE, ARGBVALUE from COLOR order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Colors temp = new Colors
                                {
                                    Id = reader.IntValue(0),
                                    Code = reader.StringValue(1),
                                    ARGBValue = reader.IntValue(2)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<ColorDescription> GetColorDescList()
        {
            List<ColorDescription> entityList = new List<ColorDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, COLORID, NAME, LANGUAGECODE from COLOR_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ColorDescription temp = new ColorDescription
                                {
                                    Id = reader.IntValue(0),
                                    ColorId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<SystemFont> GetSystemFontList()
        {
            List<SystemFont> entityList = new List<SystemFont>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, APPMAIN, SYSTEMNAME from SYSTEMFONT order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SystemFont temp = new SystemFont
                                {
                                    Id = reader.IntValue(0),
                                    AppMain = reader.IntValue(1),
                                    SystemName = reader.StringValue(2)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<FontList> GetFontList()
        {
            List<FontList> entityList = new List<FontList>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, FONTID, CODE, FONTSTYLEID from FONTLIST order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                FontList temp = new FontList
                                {
                                    Id = reader.IntValue(0),
                                    FontId = reader.IntValue(1),
                                    Code = reader.StringValue(2),
                                    FontStyleId = reader.IntValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<FontListDescription> GetFontDescList()
        {
            List<FontListDescription> entityList = new List<FontListDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, FONTLISTID, NAME, LANGUAGECODE from FONTLIST_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                FontListDescription temp = new FontListDescription
                                {
                                    Id = reader.IntValue(0),
                                    FontListId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<int> GetYearsList()
        {
            List<DateTime> dateList = new List<DateTime>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("select DATECHANGE from MOON order by DATECHANGE", dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                DateTime date = DateTime.ParseExact(reader.StringValue(0), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                dateList.Add(date);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return dateList.Select(i => i.Year).Distinct().ToList();
        }

        public static List<Profile> GetProfileList()
        {
            List<Profile> entityList = new List<Profile>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = "select ID, PROFILENAME, PERSONNAME, PERSONSURNAME, PLACEOFLIVINGID, NAKSHATRAMOONID, PADAMOON, NAKSHATRALAGNAID, PADALAGNA, NAKSHATRASUNID, PADASUN, NAKSHATRAVENUSID, PADAVENUS, NAKSHATRAJUPITERID, PADAJUPITER, NAKSHATRAMERCURYID, PADAMERCURY, NAKSHATRAMARSID, PADAMARS, NAKSHATRASATURNID, PADASATURN, NAKSHATRARAHUID, PADARAHU, NAKSHATRAKETUID, PADAKETU, CHECKED, GUID from PROFILE order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Profile temp = new Profile
                                {
                                    Id = reader.IntValue(0),
                                    ProfileName = reader.StringValue(1),
                                    PersonName = reader.StringValue(2),
                                    PersonSurname = reader.StringValue(3),
                                    PlaceOfLivingId = reader.IntValue(4),
                                    NakshatraMoonId = reader.IntValue(5),
                                    PadaMoon = reader.IntValue(6),
                                    NakshatraLagnaId = reader.IntValue(7),
                                    PadaLagna = reader.IntValue(8),
                                    NakshatraSunId = reader.IntValue(9),
                                    PadaSun = reader.IntValue(10),
                                    NakshatraVenusId = reader.IntValue(11),
                                    PadaVenus = reader.IntValue(12),
                                    NakshatraJupiterId = reader.IntValue(13),
                                    PadaJupiter = reader.IntValue(14),
                                    NakshatraMercuryId = reader.IntValue(15),
                                    PadaMercury = reader.IntValue(16),
                                    NakshatraMarsId = reader.IntValue(17),
                                    PadaMars = reader.IntValue(18),
                                    NakshatraSaturnId = reader.IntValue(19),
                                    PadaSaturn = reader.IntValue(20),
                                    NakshatraRahuId = reader.IntValue(21),
                                    PadaRahu = reader.IntValue(22),
                                    NakshatraKetuId = reader.IntValue(23),
                                    PadaKetu = reader.IntValue(24),
                                    IsChecked = Convert.ToBoolean(reader.IntValue(25)),
                                    GUID = reader.StringValue(26)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<PersonsEventsList> GetPersonsEventsList()
        {
            List<PersonsEventsList> pevList = new List<PersonsEventsList>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {   
                    SQLiteCommand command = new SQLiteCommand("select ID, DATESTART, DATEEND, NAME, MESSAGE, ARGBVALUE, GUID from USER_EVENTS order by ID", dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PersonsEventsList temp = new PersonsEventsList
                            {
                                Id = reader.IntValue(0),
                                DateStart = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                DateEnd = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                Name = reader.StringValue(3),
                                Message = reader.StringValue(4),
                                ARGBValue = reader.IntValue(5),
                                GUID = reader.StringValue(6)
                            };
                            pevList.Add(temp);
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return pevList;
        }

        public static List<Planet> GetPlanetsList()
        {
            List<Planet> entityList = new List<Planet>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, PLANETCODE from PLANET order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Planet temp = new Planet
                                {
                                    Id = reader.IntValue(0),
                                    Code = reader.StringValue(1)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<PlanetDescription> GetPlanetDescList()
        {
            List<PlanetDescription> entityList = new List<PlanetDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, PLANETID, NAME, LANGUAGECODE from PLANET_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PlanetDescription temp = new PlanetDescription
                                {
                                    Id = reader.IntValue(0),
                                    PlanetId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Zodiak> GetZodiaksList()
        {
            List<Zodiak> entityList = new List<Zodiak>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, ZODIAKCODE from ZODIAK order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Zodiak temp = new Zodiak
                                {
                                    Id = reader.IntValue(0),
                                    Code = reader.StringValue(1)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<ZodiakDescription> GetZodiakDescList()
        {
            List<ZodiakDescription> entityList = new List<ZodiakDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, ZODIAKID, NAME, LANGUAGECODE from ZODIAK_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ZodiakDescription temp = new ZodiakDescription
                                {
                                    Id = reader.IntValue(0),
                                    ZodiakId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Nakshatra> GetNakshatrasList()
        {
            List<Nakshatra> entityList = new List<Nakshatra>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, NAKSHATRACODE, COLORID from NAKSHATRA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Nakshatra temp = new Nakshatra
                                {
                                    Id = reader.IntValue(0),
                                    Code = reader.StringValue(1),
                                    ColorId = reader.IntValue(2)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<NakshatraDescription> GetNakshatraDescList()
        {
            List<NakshatraDescription> entityList = new List<NakshatraDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, NAKSHATRAID, NAME, SHORTNAME, UPRAVITEL, NATURE, DESCRIPTION, GOODFOR, BADFOR, LANGUAGECODE from NAKSHATRA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                NakshatraDescription temp = new NakshatraDescription
                                {
                                    Id = reader.IntValue(0),
                                    NakshatraId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    ShortName = reader.StringValue(3),
                                    Upravitel = reader.StringValue(4),
                                    Nature = reader.StringValue(5),
                                    Description = reader.StringValue(6),
                                    GoodFor = reader.StringValue(7),
                                    BadFor = reader.StringValue(8),
                                    LanguageCode = reader.StringValue(9)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Pada> GetPadaList()
        {
            List<Pada> entityList = new List<Pada>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, ZODIAKID, NAKSHATRAID, PADANUMBER, SPECIALNAVAMSHA, NAVAMSHA, COLORID from PADA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Pada temp = new Pada
                                {
                                    Id = reader.IntValue(0),
                                    ZodiakId = reader.IntValue(1),
                                    NakshatraId = reader.IntValue(2),
                                    PadaNumber = reader.IntValue(3),
                                    SpecialNavamsha = reader.StringValue(4),
                                    Navamsha = reader.IntValue(5),
                                    ColorId = reader.IntValue(6)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<SpecialNavamsha> GetSpecNavamshaList()
        {
            List<SpecialNavamsha> entityList = new List<SpecialNavamsha>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, SPECIALNAVAMSHAID, NAME, LANGUAGECODE from SPECIALNAVAMSHA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SpecialNavamsha temp = new SpecialNavamsha
                                {
                                    Id = reader.IntValue(0),
                                    SpeciaNavamshaId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Masa> GetMasaList()
        {
            List<Masa> entityList = new List<Masa>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, ZODIAKID, SHUNYANAKSHATRA, SHUNYATITHI from MASA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Masa temp = new Masa
                                {
                                    Id = reader.IntValue(0),
                                    ZodiakId = reader.IntValue(1),
                                    ShunyaNakshatra = reader.StringValue(2),
                                    ShunyaTithi = reader.StringValue(3),
                                    ShunyaNakshatraIdArray = MakeIdsArrayFromString(reader.StringValue(2)),
                                    ShunyaTithiIdArray = MakeIdsArrayFromString(reader.StringValue(3))
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        private static int[] MakeIdsArrayFromString(string str)
        {
            var row = str.Split(new char[] { ',' });
            int[] array = new int[row.Length];
            for (int i = 0; i < row.Length; i++)
            {
                array[i] = Convert.ToInt32(row[i]);
            }
            return array;
        }

        public static List<MasaDescription> GetMasaDescList()
        {
            List<MasaDescription> entityList = new List<MasaDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, MASAID, NAME, LANGUAGECODE from MASA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MasaDescription temp = new MasaDescription
                                {
                                    Id = reader.IntValue(0),
                                    MasaId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<TaraBala> GetTaraBalaList()
        {
            List<TaraBala> entityList = new List<TaraBala>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, COLORID from TARABALA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TaraBala temp = new TaraBala
                                {
                                    Id = reader.IntValue(0),
                                    ColorId = reader.IntValue(1)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<TaraBalaDescription> GetTaraBalaDescList()
        {
            List<TaraBalaDescription> entityList = new List<TaraBalaDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, TARABALAID, NAME, SHORTNAME, DESCRIPTION, LANGUAGECODE from TARABALA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TaraBalaDescription temp = new TaraBalaDescription
                                {
                                    Id = reader.IntValue(0),
                                    TaraBalaId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    ShortName = reader.StringValue(3),
                                    Description = reader.StringValue(4),
                                    LanguageCode = reader.StringValue(5)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Tithi> GetTithiList()
        {
            List<Tithi> entityList = new List<Tithi>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, COLORID from TITHI order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Tithi temp = new Tithi
                                {
                                    Id = reader.IntValue(0),
                                    ColorId = reader.IntValue(1)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<TithiDescription> GetTithiDescList()
        {
            List<TithiDescription> entityList = new List<TithiDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, TITHIID, NAME, SHORTNAME, UPRAVITEL, TYPE, GOODFOR, BADFOR, LANGUAGECODE from TITHI_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TithiDescription temp = new TithiDescription
                                {
                                    Id = reader.IntValue(0),
                                    TithiId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    ShortName = reader.StringValue(3),
                                    Upravitel = reader.StringValue(4),
                                    Type = reader.StringValue(5),
                                    GoodFor = reader.StringValue(6),
                                    BadFor = reader.StringValue(7),
                                    LanguageCode = reader.StringValue(8)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Karana> GetKaranaList()
        {
            List<Karana> entityList = new List<Karana>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, TITHIID, POSITION, COLORID from KARANA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Karana temp = new Karana
                                {
                                    Id = reader.IntValue(0),
                                    TithiId = reader.IntValue(1),
                                    Position = reader.IntValue(2),
                                    ColorId = reader.IntValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<KaranaDescription> GetKaranaDescList()
        {
            List<KaranaDescription> entityList = new List<KaranaDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, KARANAID, NAME, UPRAVITEL, GOODFOR, BADFOR, LANGUAGECODE from KARANA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                KaranaDescription temp = new KaranaDescription
                                {
                                    Id = reader.IntValue(0),
                                    KaranaId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    Upravitel = reader.StringValue(3),
                                    GoodFor = reader.StringValue(4),
                                    BadFor = reader.StringValue(5),
                                    LanguageCode = reader.StringValue(6)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<NityaJoga> GetNityaJogaList()
        {
            List<NityaJoga> entityList = new List<NityaJoga>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, NJCODE, COLORID, NAKSHATRAID, JOGIPLANETID, AVAJOGIPLANETID from NITYAJOGA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                NityaJoga temp = new NityaJoga
                                {
                                    Id = reader.IntValue(0),
                                    Code = reader.StringValue(1),
                                    ColorId = reader.IntValue(2),
                                    NakshatraId = reader.IntValue(3),
                                    JogiPlanetId = reader.IntValue(4),
                                    AvaJogiPlanetId = reader.IntValue(5)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<NityaJogaDescription> GetNityaJogaDescList()
        {
            List<NityaJogaDescription> entityList = new List<NityaJogaDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, NITYAJOGAID, NAME, DEITY, MEANING, DESCRIPTION, LANGUAGECODE from NITYAJOGA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                NityaJogaDescription temp = new NityaJogaDescription
                                {
                                    Id = reader.IntValue(0),
                                    NityaJogaId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    Deity = reader.StringValue(3),
                                    Meaning = reader.StringValue(4),
                                    Description = reader.StringValue(5),
                                    LanguageCode = reader.StringValue(6)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Muhurta> GetMuhurtaList()
        {
            List<Muhurta> entityList = new List<Muhurta>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, COLORID, MUHURTACODE from MUHURTA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Muhurta temp = new Muhurta
                                {
                                    Id = reader.IntValue(0),
                                    ColorId = reader.IntValue(1),
                                    Code = reader.StringValue(2)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<MuhurtaDescription> GetMuhurtaDescList()
        {
            List<MuhurtaDescription> entityList = new List<MuhurtaDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, MUHURTAID, NAME, SHORTNAME, LANGUAGECODE from MUHURTA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MuhurtaDescription temp = new MuhurtaDescription
                                {
                                    Id = reader.IntValue(0),
                                    MuhurtaId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    ShortName = reader.StringValue(3),
                                    LanguageCode = reader.StringValue(4)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Joga> GetJogaList()
        {
            List<Joga> entityList = new List<Joga>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, COLORID, JOGACODE from JOGA order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Joga temp = new Joga
                                {
                                    Id = reader.IntValue(0),
                                    ColorId = reader.IntValue(1),
                                    Code = reader.StringValue(2)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<JogaDescription> GetJogaDescList()
        {
            List<JogaDescription> entityList = new List<JogaDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, JOGAID, NAME, SHORTNAME, DESCRIPTION, LANGUAGECODE from JOGA_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                JogaDescription temp = new JogaDescription
                                {
                                    Id = reader.IntValue(0),
                                    JogaId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    ShortName = reader.StringValue(3),
                                    Description = reader.StringValue(4),
                                    LanguageCode = reader.StringValue(5)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Eclipse> GetEclipseList()
        {
            List<Eclipse> entityList = new List<Eclipse>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, ECLIPSECODE from ECLIPSE order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Eclipse temp = new Eclipse
                                {
                                    Id = reader.IntValue(0),
                                    Code = reader.StringValue(1)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<EclipseDescription> GetEclipseDescList()
        {
            List<EclipseDescription> entityList = new List<EclipseDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, ECLIPSEID, NAME, LANGUAGECODE from ECLIPSE_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EclipseDescription temp = new EclipseDescription
                                {
                                    Id = reader.IntValue(0),
                                    EclipseId = reader.IntValue(1),
                                    Name = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Tranzit> GetTranzitList()
        {
            List<Tranzit> entityList = new List<Tranzit>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, PLANETID, DOM, COLORID, VEDHA from TRANZIT order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Tranzit temp = new Tranzit
                                {
                                    Id = reader.IntValue(0),
                                    PlanetId = reader.IntValue(1),
                                    Dom = reader.IntValue(2),
                                    ColorId = reader.IntValue(3),
                                    Vedha = reader.StringValue(4)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<TranzitDescription> GetTranzitDescList()
        {
            List<TranzitDescription> entityList = new List<TranzitDescription>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, TRANZITID, DESCRIPTION, LANGUAGECODE from TRANZIT_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TranzitDescription temp = new TranzitDescription
                                {
                                    Id = reader.IntValue(0),
                                    TranzitId = reader.IntValue(1),
                                    Description = reader.StringValue(2),
                                    LanguageCode = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Muhurta30> GetMuhurta30List()
        {
            List<Muhurta30> entityList = new List<Muhurta30>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, COLORID, MUHURTA30CODE from MUHURTA30 order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Muhurta30 temp = new Muhurta30
                                {
                                    Id = reader.IntValue(0),
                                    ColorId = reader.IntValue(1),
                                    Muhurta30Code = reader.StringValue(2)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Muhurta30Description> GetMuhurta30DescList()
        {
            List<Muhurta30Description> entityList = new List<Muhurta30Description>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, MUHURTA30ID, SHORTNAME, NAME, DESCRIPTION, LANGUAGECODE from MUHURTA30_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Muhurta30Description temp = new Muhurta30Description
                                {
                                    Id = reader.IntValue(0),
                                    Muhurta30Id = reader.IntValue(1),
                                    ShortName = reader.StringValue(2),
                                    Name = reader.StringValue(3),
                                    Description = reader.StringValue(4),
                                    LanguageCode = reader.StringValue(5)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Ghati60> GetGhati60List()
        {
            List<Ghati60> entityList = new List<Ghati60>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, POSITION, COLORID, GHATI60CODE from GHATI60 order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Ghati60 temp = new Ghati60
                                {
                                    Id = reader.IntValue(0),
                                    Position = reader.IntValue(1),
                                    ColorId = reader.IntValue(2),
                                    Ghati60Code = reader.StringValue(3)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<Ghati60Description> GetGhati60DescList()
        {
            List<Ghati60Description> entityList = new List<Ghati60Description>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select ID, GHATI60ID, SHORTNAME, NAME, DESCRIPTION, LANGUAGECODE from GHATI60_DESC order by ID";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Ghati60Description temp = new Ghati60Description
                                {
                                    Id = reader.IntValue(0),
                                    Ghati60Id = reader.IntValue(1),
                                    ShortName = reader.StringValue(2),
                                    Name = reader.StringValue(3),
                                    Description = reader.StringValue(4),
                                    LanguageCode = reader.StringValue(5)
                                };
                                entityList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return entityList;
        }

        public static List<PlanetData> GetPlanetData(EPlanet planetCode)
        {
            string tableName = planetCode.ToString();
            double longitude, latitude, speedinlongitude, speedinlatitude;
            List<PlanetData> pdList = new List<PlanetData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATECHANGE, LONGITUDE, LATITUDE, SPEEDINLONGITUDE, SPEEDINLATITUDE, RETRO, ZODIAKID, NAKSHATRAID, PADAID from {tableName}";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (double.TryParse(reader.StringValue(1), NumberStyles.Any, CultureInfo.InvariantCulture, out longitude) &&
                                    double.TryParse(reader.StringValue(2), NumberStyles.Any, CultureInfo.InvariantCulture, out latitude) &&
                                    double.TryParse(reader.StringValue(3), NumberStyles.Any, CultureInfo.InvariantCulture, out speedinlongitude) &&
                                    double.TryParse(reader.StringValue(4), NumberStyles.Any, CultureInfo.InvariantCulture, out speedinlatitude))
                                {
                                    PlanetData temp = new PlanetData
                                    {
                                        Date = DateTime.ParseExact(reader.StringValue(0), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                        Longitude = longitude,
                                        Latitude = latitude,
                                        SpeedInLongitude = speedinlongitude,
                                        SpedInLatitude = speedinlatitude,
                                        Retro = reader.StringValue(5),
                                        ZodiakId = reader.IntValue(6),
                                        NakshatraId = reader.IntValue(7),
                                        PadaId = reader.IntValue(8)
                                    };
                                    pdList.Add(temp);
                                }
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return pdList;
        }

        public static List<TithiData> GetTithiData()
        {
            double msdifference;
            List<TithiData> tdList = new List<TithiData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATECHANGE, MSDIFFERENCE, TITHIID from TITHIDATA";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (double.TryParse(reader.StringValue(1), NumberStyles.Any, CultureInfo.InvariantCulture, out msdifference))
                                {
                                    TithiData temp = new TithiData
                                    {
                                        Date = DateTime.ParseExact(reader.StringValue(0), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                        MoonSunDifference = msdifference,
                                        TithiId = reader.IntValue(2)
                                    };
                                    tdList.Add(temp);
                                }
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return tdList;
        }

        public static List<NityaJogaData> GetNityaJogaData()
        {
            double longitude;
            List<NityaJogaData> jnList = new List<NityaJogaData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATECHANGE, LONGITUDE, NAKSHATRAID from JNDATA";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (double.TryParse(reader.StringValue(1), NumberStyles.Any, CultureInfo.InvariantCulture, out longitude))
                                {
                                    NityaJogaData temp = new NityaJogaData
                                    {
                                        Date = DateTime.ParseExact(reader.StringValue(0), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                        Longitude = longitude,
                                        NakshatraId = reader.IntValue(2)
                                    };
                                    jnList.Add(temp);
                                }
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return jnList;
        }

        public static List<EclipseData> GetEclipseData()
        {
            List<EclipseData> odList = new List<EclipseData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATE, ECLIPSEID from ECLIPSEDATA";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EclipseData temp = new EclipseData
                                {
                                    Date = DateTime.ParseExact(reader.StringValue(0), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                    EclipseId = reader.IntValue(1)
                                };
                                odList.Add(temp);
                            }
                        }
                        reader.Close();
                    }
                }
                catch { }
                dbCon.Close();
            }
            return odList;
        }

        public static List<NakshatraCalendar> CreateNakshatraCalendarList(List<PlanetData> moonList)
        {
            List<PlanetData> nOnlyList = new List<PlanetData>(); 
            int currentNakshatraId = 0;
            for (int i = 0; i < moonList.Count; i++)
            {
                if (moonList[i].NakshatraId != currentNakshatraId)
                {
                    nOnlyList.Add(moonList[i]);
                    currentNakshatraId = moonList[i].NakshatraId;
                }
            }

            List<NakshatraCalendar> ncList = new List<NakshatraCalendar>();
            int index = 0, max = nOnlyList.Count;
            foreach (PlanetData pd in nOnlyList)
            {
                if (index != (max - 1))
                {
                    NakshatraCalendar nTemp = new NakshatraCalendar
                    {
                        DateStart = pd.Date,
                        DateEnd = nOnlyList[index + 1].Date,
                        ColorCode = GetNakshatraColor((ENakshatra)pd.NakshatraId),
                        NakshatraCode = (ENakshatra)pd.NakshatraId
                    };
                    ncList.Add(nTemp);
                }
                else if(index == (max -1))
                {
                    DateTime endDate = new DateTime(pd.Date.Year, pd.Date.Month, pd.Date.Day, 23, 59, 59);
                    NakshatraCalendar nTemp = new NakshatraCalendar
                    {
                        DateStart = pd.Date,
                        DateEnd = endDate,
                        ColorCode = GetNakshatraColor((ENakshatra)pd.NakshatraId),
                        NakshatraCode = (ENakshatra)pd.NakshatraId
                    };
                    ncList.Add(nTemp);
                }
                index++;
            }
            return ncList;
        }

        public static List<ChandraBalaCalendar> CreateChandraBalaCalendarList(List<PlanetData> moonList)
        {
            List<PlanetData> zOnlyList = new List<PlanetData>();
            int  currentZodiakId = 0;
            for (int i = 0; i < moonList.Count; i++)
            {
                if (moonList[i].ZodiakId != currentZodiakId)
                {
                    zOnlyList.Add(moonList[i]);
                    currentZodiakId = moonList[i].ZodiakId;
                }
            }

            List<ChandraBalaCalendar> cbList = new List<ChandraBalaCalendar>();
            int index = 0, max = zOnlyList.Count;
            foreach (PlanetData pd in zOnlyList)
            {
                if (index != (max - 1))
                {
                    ChandraBalaCalendar cbTemp = new ChandraBalaCalendar
                    {
                        DateStart = pd.Date,
                        DateEnd = zOnlyList[index + 1].Date,
                        ColorCode = EColor.NOCOLOR,
                        ZodiakCode = (EZodiak)pd.ZodiakId
                    };
                    cbList.Add(cbTemp);
                }
                else if (index == (max - 1))
                {
                    DateTime endDate = new DateTime(pd.Date.Year, pd.Date.Month, pd.Date.Day, 23, 59, 59);
                    ChandraBalaCalendar cbTemp = new ChandraBalaCalendar
                    {
                        DateStart = pd.Date,
                        DateEnd = endDate,
                        ColorCode = EColor.NOCOLOR,
                        ZodiakCode = (EZodiak)pd.ZodiakId
                    };
                    cbList.Add(cbTemp);
                }
                index++;
            }
            return cbList;
        }

        public static EColor GetNakshatraColor(ENakshatra nCode)
        {
            return (EColor)(_nakshatraList.Where(i => i.Id == (int)nCode).FirstOrDefault()?.ColorId ?? 0);
        }

        public static List<TithiCalendar> CreateTithiCalendarList(List<TithiData> tdList)
        {
            List<TithiCalendar> tcList = new List<TithiCalendar>();
            int index = 0, max = tdList.Count;
            foreach (TithiData td in tdList)
            {
                if (index != (max - 1))
                {
                    TithiCalendar tTemp = new TithiCalendar
                    {
                        DateStart = td.Date,
                        DateEnd = tdList[index + 1].Date,
                        ColorCode = GetTithiColor(td.TithiId),
                        TithiId = td.TithiId
                    };
                    tcList.Add(tTemp);
                }
                else if (index == (max - 1))
                {
                    DateTime endDate = new DateTime(td.Date.Year, td.Date.Month, td.Date.Day, 23, 59, 59);
                    TithiCalendar tTemp = new TithiCalendar
                    {
                        DateStart = td.Date,
                        DateEnd = endDate,
                        ColorCode = GetTithiColor(td.TithiId),
                        TithiId = td.TithiId
                    };
                    tcList.Add(tTemp);
                }
                index++;
            }
            return tcList;
        }

        public static EColor GetTithiColor(int tId)
        {
            return (EColor)(CacheLoad._tithiList.Where(i => i.Id == tId).FirstOrDefault()?.ColorId ?? 0);
        }

        public static List<KaranaCalendar> CreateKaranaCalendarList(List<TithiCalendar> tcList)
        {
            List<KaranaCalendar> kcList = new List<KaranaCalendar>();
            foreach (TithiCalendar tc in tcList)
            {
                TimeSpan periodHalf = new TimeSpan((tc.DateEnd - tc.DateStart).Ticks / 2);
                DateTime middleDate = tc.DateStart.AddTicks(periodHalf.Ticks);
                int karanaIdPos1 = GetKaranaIdByTithiAndSequence(tc.TithiId, 1);
                int karanaIdPos2 = GetKaranaIdByTithiAndSequence(tc.TithiId, 2);

                kcList.Add(new KaranaCalendar { DateStart = tc.DateStart, DateEnd = middleDate, ColorCode = GetKaranaColor(karanaIdPos1), TithiId = tc.TithiId, KaranaId = karanaIdPos1 });
                kcList.Add(new KaranaCalendar { DateStart = middleDate, DateEnd = tc.DateEnd, ColorCode = GetKaranaColor(karanaIdPos2), TithiId = tc.TithiId, KaranaId = karanaIdPos2 });
            }
            return kcList;
        }

        private static int GetKaranaIdByTithiAndSequence(int tithi, int sequence)
        {
            return CacheLoad._karanaList.Where(i => i.TithiId == tithi && i.Position == sequence).FirstOrDefault()?.Id ?? 0;
        }

        public static EColor GetKaranaColor(int kId)
        {
            return (EColor)(CacheLoad._karanaList.Where(i => i.Id == kId).FirstOrDefault()?.ColorId ?? 0);
        }

        public static List<NityaJogaCalendar> CreateNityaJogaCalendarList(List<NityaJogaData> njdList)
        {
            List<NityaJogaCalendar> njcList = new List<NityaJogaCalendar>();
            int index = 0, max = njdList.Count;
            foreach (NityaJogaData td in njdList)
            {
                ENityaJoga nj = GetNityaJogaCodeByNakshatraId(td.NakshatraId);
                if (index != (max - 1))
                {
                    NityaJogaCalendar jnTemp = new NityaJogaCalendar
                    {
                        DateStart = td.Date,
                        DateEnd = njdList[index + 1].Date,
                        ColorCode = GetNityaJogaColorById((int)nj),
                        NJCode = nj,
                        NakshatraId = td.NakshatraId
                    };
                    njcList.Add(jnTemp);
                }
                else if (index == (max - 1))
                {
                    DateTime endDate = new DateTime(td.Date.Year, td.Date.Month, td.Date.Day, 23, 59, 59);
                    NityaJogaCalendar jnTemp = new NityaJogaCalendar
                    {
                        DateStart = td.Date,
                        DateEnd = endDate,
                        ColorCode = GetNityaJogaColorById((int)nj),
                        NJCode = nj,
                        NakshatraId = td.NakshatraId
                    };
                    njcList.Add(jnTemp);
                }
                index++;
            }
            return njcList;
        }

        public static ENityaJoga GetNityaJogaCodeByNakshatraId(int nId)
        {
            return (ENityaJoga)(_nityaJogaList.Where(i => i.NakshatraId == nId).FirstOrDefault()?.Id ?? 0);
        }

        public static EColor GetNityaJogaColorById(int nId)
        {
            return (EColor)(_nityaJogaList.Where(i => i.Id == nId).FirstOrDefault()?.ColorId ?? 0);
        }

        public static List<PlanetCalendar> CreatePlanetZodiakCalendarList(EPlanet planetCode, List<PlanetData> pdList)
        {
            List<PlanetCalendar> pcList = new List<PlanetCalendar>();
            int currentZodiakId = 0;
            foreach (PlanetData pd in pdList)
            {
                if (pd.ZodiakId != currentZodiakId)
                {
                    PlanetCalendar pdTemp = new PlanetCalendar
                    {
                        DateStart = pd.Date,
                        ColorCode = EColor.NOCOLOR,
                        PlanetCode = planetCode,
                        Dom = 0,
                        Retro = pd.Retro,
                        ZodiakCode = (EZodiak)pd.ZodiakId,
                        NakshatraCode = (ENakshatra)pd.NakshatraId,
                        PadaId = pd.PadaId,
                        LagnaDom = 0,
                        TaraBalaPercent = 0
                    };
                    pcList.Add(pdTemp);
                    currentZodiakId = pd.ZodiakId;
                }
            }
            DateTime endDate = new DateTime(pdList.Last().Date.Year, 12, 31, 23, 59, 59);
            int index = 0, max = pcList.Count;
            foreach (PlanetCalendar pc in pcList)
            {
                if (index != (max - 1))
                {
                    pc.DateEnd = pcList[index + 1].DateStart;
                }
                else if (index == (max - 1))
                {
                    pc.DateEnd = endDate;
                }
                index++;
            }
            return pcList;
        }

        public static List<PlanetCalendar> CreatePlanetZodiakRetroCalendarList(EPlanet planetCode, List<PlanetData> pdList)
        {
            List<PlanetCalendar> pcList = new List<PlanetCalendar>();
            int currentZodiakId = 0;
            string currentRetro = string.Empty;
            foreach (PlanetData pd in pdList)
            {
                if (pd.ZodiakId != currentZodiakId || !pd.Retro.Equals(currentRetro))
                {
                    PlanetCalendar pdTemp = new PlanetCalendar
                    {
                        DateStart = pd.Date,
                        ColorCode = EColor.NOCOLOR,
                        PlanetCode = planetCode,
                        Dom = 0,
                        Retro = pd.Retro,
                        ZodiakCode = (EZodiak)pd.ZodiakId,
                        NakshatraCode = (ENakshatra)pd.NakshatraId,
                        PadaId = pd.PadaId,
                        LagnaDom = 0,
                        TaraBalaPercent = 0
                    };
                    pcList.Add(pdTemp);
                    currentZodiakId = pd.ZodiakId;
                    currentRetro = pd.Retro;
                }
            }
            DateTime endDate = new DateTime(pdList.Last().Date.Year, 12, 31, 23, 59, 59);
            int index = 0, max = pcList.Count;
            foreach (PlanetCalendar pc in pcList)
            {
                if (index != (max - 1))
                {
                    pc.DateEnd = pcList[index + 1].DateStart;
                }
                else if (index == (max - 1))
                {
                    pc.DateEnd = endDate;
                }
                index++;
            }
            return pcList;
        }

        public static List<PlanetCalendar> CreatePlanetNakshatraCalendarList(EPlanet planetCode, List<PlanetData> pdList)
        {
            List<PlanetCalendar> pcList = new List<PlanetCalendar>();
            int currentNakshatraId = 0;
            foreach (PlanetData pd in pdList)
            {
                if (pd.NakshatraId != currentNakshatraId)
                {
                    PlanetCalendar pdTemp = new PlanetCalendar
                    {
                        DateStart = pd.Date,
                        ColorCode = EColor.NOCOLOR,
                        PlanetCode = planetCode,
                        Dom = 0,
                        Retro = pd.Retro,
                        ZodiakCode = (EZodiak)pd.ZodiakId,
                        NakshatraCode = (ENakshatra)pd.NakshatraId,
                        PadaId = pd.PadaId,
                        LagnaDom = 0,
                        TaraBalaPercent = 0
                    };
                    pcList.Add(pdTemp);
                    currentNakshatraId = pd.NakshatraId;
                }
            }
            DateTime endDate = new DateTime(pcList.Last().DateStart.Year, 12, 31, 23, 59, 59);
            int index = 0, max = pcList.Count;
            foreach (PlanetCalendar pc in pcList)
            {
                if (index != (max - 1))
                {
                    pc.DateEnd = pcList[index + 1].DateStart;
                }
                else if (index == (max - 1))
                {
                    pc.DateEnd = endDate;
                }
                index++;
            }
            return pcList;
        }

        public static List<PlanetCalendar> CreatePlanetPadaCalendarList(EPlanet planetCode, List<PlanetData> pdList)
        {
            List<PlanetCalendar> pcList = new List<PlanetCalendar>();
            int currentPadaId = 0;
            foreach (PlanetData pd in pdList)
            {
                if (pd.PadaId != currentPadaId)
                {
                    PlanetCalendar pdTemp = new PlanetCalendar
                    {
                        DateStart = pd.Date,
                        ColorCode = EColor.NOCOLOR,
                        PlanetCode = planetCode,
                        Dom = 0,
                        Retro = pd.Retro,
                        ZodiakCode = (EZodiak)pd.ZodiakId,
                        NakshatraCode = (ENakshatra)pd.NakshatraId,
                        PadaId = pd.PadaId,
                        LagnaDom = 0,
                        TaraBalaPercent = 0
                    };
                    pcList.Add(pdTemp);
                    currentPadaId = pd.PadaId;
                }
            }
            DateTime endDate = new DateTime(pcList.Last().DateStart.Year, 12, 31, 23, 59, 59);
            int index = 0, max = pcList.Count;
            foreach (PlanetCalendar pc in pcList)
            {
                if (index != (max - 1))
                {
                    pc.DateEnd = pcList[index + 1].DateStart;
                }
                else if (index == (max - 1))
                {
                    pc.DateEnd = endDate;
                }
                index++;
            }
            return pcList;
        }

        public static List<EclipseCalendar> CreateEclipseCalendarList(List<EclipseData> edList)
        {
            List<EclipseCalendar> ecList = new List<EclipseCalendar>();
            foreach (EclipseData ed in edList)
            {
                EclipseCalendar temp = new EclipseCalendar
                {
                    DateStart = ed.Date,
                    ColorCode = EColor.NOCOLOR,
                    EclipseCode = ed.EclipseId
                };
                ecList.Add(temp);
            }
            return ecList;
        }
        
        public static List<HoraPlanet> MakeHoraPlanetList()
        {
            List<HoraPlanet> hpList = new List<HoraPlanet>();
            hpList.Add(new HoraPlanet { Id = 1, PlanetCode = EHoraPlanet.SUN, ColorCode = EColor.SUN });
            hpList.Add(new HoraPlanet { Id = 2, PlanetCode = EHoraPlanet.VENUS, ColorCode = EColor.VENUS });
            hpList.Add(new HoraPlanet { Id = 3, PlanetCode = EHoraPlanet.MERCURY, ColorCode = EColor.MERCURY });
            hpList.Add(new HoraPlanet { Id = 4, PlanetCode = EHoraPlanet.MOON, ColorCode = EColor.MOON });
            hpList.Add(new HoraPlanet { Id = 5, PlanetCode = EHoraPlanet.SATURN, ColorCode = EColor.SATURN });
            hpList.Add(new HoraPlanet { Id = 6, PlanetCode = EHoraPlanet.JUPITER, ColorCode = EColor.JUPITER });
            hpList.Add(new HoraPlanet { Id = 7, PlanetCode = EHoraPlanet.MARS, ColorCode = EColor.MARS });
            return hpList;
        }
        

    }
}
